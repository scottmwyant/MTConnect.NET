using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.Extensions.Logging;
using MTConnect;
using MTConnect.Adapters.Shdr;
using MTConnect.Agents;
using MTConnect.Configurations;
using MTConnect.Clients.Rest;
using MTConnect.Devices;
using MTConnect.Errors;
using MTConnect.Http;
using MTConnect.Observations.Samples.Values;
using MTConnect.Shdr;
using MTConnect.Streams;
using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace IntegrationTests
{
    public class MTAgentFixture
    {
        #region Fields

        public int CurrentAgentPort = 5000;
        public int CurrentAdapterPort = 7878;

        #endregion
    }

    public class ClientAgentCommunicationTests : IClassFixture<MTAgentFixture>, IDisposable
    {
        #region Fields

        private const int c_maxWaitTimeout = 10000;

        private readonly ShdrAdapter _adapter;

        private readonly IMTConnectAgent _agent;
        private readonly MTConnectHttpServer _server;

        private readonly MTAgentFixture _fixture;
        private readonly ILogger _logger;

        private readonly string _machineId;
        private readonly string _machineName;

        #endregion

        public ClientAgentCommunicationTests(
            MTAgentFixture fixture,
            ITestOutputHelper testOutputHelper)
        {
            _fixture = fixture;
            _logger = testOutputHelper.BuildLogger(LogLevel.Trace);

            _machineId = Guid.NewGuid().ToString();
            _machineName = "M12346";
            //_machineName = $"Machine{_fixture.CurrentAgentPort}";

            var devicesFile = "devices.xml";

            var configuration = new ShdrAgentConfiguration
            {
                Port = _fixture.CurrentAgentPort,
                Adapters = new List<ShdrAdapterConfiguration>()
                {
                    new()
                    {
                        DeviceKey = _machineName, 
                        Hostname = "localhost", 
                        Port = _fixture.CurrentAdapterPort
                    }
                }
            };

            GenerateDevicesXml(
                _machineId,
                _machineName,
                devicesFile,
                _logger);

            _adapter = new ShdrAdapter(_machineName, _fixture.CurrentAdapterPort, 2000)
            {
                Interval = 100
                
            };
            _adapter.Start();

            AddCuttingTools();

            _agent = new MTConnectAgent(configuration);
            //_agent.Version = new Version(1, 8);

            // Add Adapter Clients
            var devices = DeviceConfiguration.FromFile(devicesFile, DocumentFormat.XML).ToList();
            if (!devices.IsNullOrEmpty())
            {
                // Add Device(s) to Agent
                foreach (var device in devices)
                {
                    _agent.AddDevice(device);
                }

                foreach (var adapterConfiguration in configuration.Adapters)
                {
                    var device = devices.FirstOrDefault(o => o.Name == adapterConfiguration.DeviceKey);
                    if (device != null)
                    {
                        var adapterClient = new ShdrAdapterClient(adapterConfiguration, _agent, device);

                        adapterClient.Start();
                    }
                }
            }

            _server = new MTConnectHttpServer(configuration, _agent);
            _server.Start();
        }

        public void Dispose()
        {
            // Stop are not awaitable, so we cannot guarantee that it finishes before next test start
            _server.Stop();
            _adapter.Stop();

            // Therefore we use a new port for every test.
            _fixture.CurrentAgentPort++;
            _fixture.CurrentAdapterPort++;

        }

        #region Private Tests

        private void AddCuttingTools()
        {
            var tool = new MTConnect.Assets.CuttingTools.CuttingToolAsset
            {
                //tool.Description = new Devices.Description
                //{
                //    Manufacturer = "Sandvik",
                //    Model = "B5632",
                //    SerialNumber = "12345678946"
                //};
                AssetId = "5.12",
                ToolId = "12",
                CuttingToolLifeCycle = new MTConnect.Assets.CuttingTools.CuttingToolLifeCycle
                {
                    Location = new MTConnect.Assets.CuttingTools.Location { Type = MTConnect.Assets.CuttingTools.LocationType.SPINDLE },
                    ProgramToolNumber = "12",
                    ProgramToolGroup = "5"
                }
            };
            tool.CuttingToolLifeCycle.Measurements.Add(new MTConnect.Assets.CuttingTools.Measurements.FunctionalLengthMeasurement(7.6543));
            tool.CuttingToolLifeCycle.Measurements.Add(new MTConnect.Assets.CuttingTools.Measurements.CuttingDiameterMaxMeasurement(0.375));
            tool.CuttingToolLifeCycle.CuttingItems.Add(new MTConnect.Assets.CuttingTools.CuttingItem
            {
                ItemId = "12.1",
                Locus = MTConnect.Assets.CuttingTools.CuttingItemLocas.FLUTE.ToString()
            });
            tool.CuttingToolLifeCycle.CutterStatus.Add(MTConnect.Assets.CuttingTools.CutterStatus.AVAILABLE);
            tool.CuttingToolLifeCycle.CutterStatus.Add(MTConnect.Assets.CuttingTools.CutterStatus.NEW);
            tool.CuttingToolLifeCycle.CutterStatus.Add(MTConnect.Assets.CuttingTools.CutterStatus.MEASURED);
            tool.Timestamp = UnixDateTime.Now;

            _adapter.AddAsset(tool);
        }

        private Task<MTConnectClient?> Connect(
            string url, 
            string deviceName,
            ILogger logger,
            EventHandler<IStreamsResponseDocument> onCurrent,
            EventHandler<IStreamsResponseDocument> onSample)
        {
            var tcs = new TaskCompletionSource<MTConnectClient?>();

            var client = new MTConnectClient(url, deviceName)
            {
                Interval = 500
            };
            client.OnCurrentReceived += onCurrent;
            client.OnSampleReceived += onSample;
            client.OnConnectionError += (
                sender,
                exception) =>
            {
                _logger.LogDebug(exception, "Connection error happened.");
                tcs.TrySetResult(null);
            };
            client.OnInternalError += (
                sender,
                exception) =>
            {
                _logger.LogDebug(exception, "Internal error happened.");
                tcs.TrySetResult(null);
            };
            client.OnMTConnectError += (
                sender,
                exception) =>
            {
                foreach (var ex in exception.Errors)
                {
                    _logger.LogDebug(
                        "MTConnect error {0} happened: {1}.",
                        ex.ErrorCode,
                        ex.Value);
                }

                tcs.TrySetResult(null);
            };
            client.OnClientStarted += (
                sender,
                args) =>
            {
                _logger.LogTrace("Connection established");
                tcs.TrySetResult(client);
            };
            client.Start();

            return tcs.Task;
        }

        private static void GenerateDevicesXml(
            string machineId,
            string machineName,
            string fileName,
            ILogger logger)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "IntegrationTests.devices-tpl.xml";

            using var stream = assembly.GetManifestResourceStream(resourceName);
            if (stream is null)
            {
                logger.LogError("Cannot find resource {0}", resourceName);
                return;
            }

            using var reader = new StreamReader(stream);
            var xml = reader.ReadToEnd();
            var xDocument = XDocument.Parse(xml);

            XNamespace ns = "urn:mtconnect.org:MTConnectDevices:1.7";
            var xDevice = xDocument.Descendants(ns + "Device").First();
            var uuidAttr = xDevice.Attribute("uuid");
            if (uuidAttr is null)
            {
                logger.LogError("UUID Device attribute cannot be found in\n{0}", xml);
                return;
            }

            uuidAttr.Value = machineId;

            var nameAttr = xDevice.Attribute("name");
            if (nameAttr is null)
            {
                logger.LogError("UUID Device attribute cannot be found in\n{0}", xml);
                return;
            }

            nameAttr.Value = machineName;

            using var config = File.Create("devices.xml");
            xDocument.Save(config);
        }

        #endregion

        [Fact]
        public async void GetCurrentFieldShouldReturnUpdatedValue()
        {
            var cts = new CancellationTokenSource();
            cts.CancelAfter(c_maxWaitTimeout);

            var currentClient = new MTConnectCurrentClient(
                $"127.0.0.1:{_fixture.CurrentAgentPort}", 
                _machineName, 
                $"//*[@id='program']");

            var document = await currentClient.GetAsync(cts.Token);
            if (document is null || document.Streams.IsNullOrEmpty())
            {
                throw new XunitException("Document is null or empty.");
            }

            var current = document.Streams
                .First()
                .Observations
                .Select(o => o.GetValue("CDATA"))
                .FirstOrDefault();

            Assert.Equal("UNAVAILABLE", current);

            var observationEvt = new AutoResetEvent(false);
            _agent.ObservationAdded += (
                sender,
                observation) =>
            {
                _logger.LogTrace($"to {observation.DataItemId} {observation.GetValue("CDATA")}");
                observationEvt.Set();
            };

            var item = new ShdrDataItem("program", "SuperProg42");
            _adapter.AddDataItem(item);

            Assert.True(observationEvt.WaitOne(c_maxWaitTimeout));

            document = await currentClient.GetAsync(cts.Token);
            if (document is null || document.Streams.IsNullOrEmpty())
            {
                throw new XunitException("Document is null or empty.");
            }

            current = document.Streams
                .First()
                .Observations
                .Select(o => o.GetValue("CDATA"))
                .FirstOrDefault();

            Assert.Equal("SuperProg42", current);
        }

        [Fact]
        public async void WaitForSampleShouldSucceedAfterFirstItemIsSent()
        {
            var cts = new CancellationTokenSource();
            cts.CancelAfter(c_maxWaitTimeout);

            void OnCurrent(object? sender, IStreamsResponseDocument document) { }

            var observationEvt = new AutoResetEvent(false);
            void OnSample(
                object? sender,
                IStreamsResponseDocument document)
            {
                if (document.Streams.IsNullOrEmpty())
                {
                    return;
                }

                foreach (var observation in document.GetObservations())
                {
                    if (observation.DataItemId == "servotemp1" && observation.GetValue("CDATA") == "120")
                    {
                        observationEvt.Set();
                    }
                }
            }

            var client = await Connect(
                $"127.0.0.1:{_fixture.CurrentAgentPort}",
                _machineName,
                _logger,
                OnCurrent,
                OnSample);
            if (client is null)
            {
                throw new XunitException("Client is null.");
            }

            _adapter.AddDataItem(new ShdrDataItem("servotemp1", new TemperatureValue(120)));
            
            Assert.True(observationEvt.WaitOne(c_maxWaitTimeout));
        }
    }
}
