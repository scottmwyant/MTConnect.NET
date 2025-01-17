// Copyright (c) 2022 TrakHound Inc., All Rights Reserved.

// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MTConnect.Devices;
using MTConnect.Agents;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MTConnect.Http.Controllers
{
    [ApiController]
    [Route("")]
    public class GlobalController : ControllerBase
    {
        private readonly IMTConnectAgent _agent;
        private readonly ILogger<DeviceController> _logger;


        public GlobalController(IMTConnectAgent agent, ILogger<DeviceController> logger)
        {
            _agent = agent;
            _logger = logger;
        }


        /// <summary>
        /// An Agent responds to a Probe Request with an MTConnectDevices Response Document that contains the 
        /// Equipment Metadata for pieces of equipment that are requested and currently represented in the Agent.
        /// </summary>
        /// <param name="version">The MTConnect Version of the response document</param>
        /// <param name="documentFormat">The format of the response document</param>
        /// <param name="indentOutput">A boolean flag to indent the response document (pretty)</param>
        /// <returns>MTConnectDevices Response Document</returns>
        [HttpGet]
        [HttpGet("probe")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DevicesResponseDocument))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status405MethodNotAllowed)]
        [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
        [ProducesResponseType(StatusCodes.Status431RequestHeaderFieldsTooLarge)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetProbe(
            [FromQuery] string deviceType = null,
            [FromQuery] Version version = null,
            [FromQuery] string documentFormat = DocumentFormat.XML,
            [FromQuery] bool? indentOutput = null,
            [FromQuery] bool? outputComments = null
            )
        {
            _logger.LogInformation($"[Api-Interface] : {Request.Host} : Requested a MTConnectDevices Document : [{Request.Method}] : {Request.Path} : {Request.QueryString}");

            // Set Format Options
            var formatOptions = new List<KeyValuePair<string, string>>();

            if (indentOutput.HasValue) formatOptions.Add(new KeyValuePair<string, string>("indentOutput", indentOutput.Value.ToString()));
            else formatOptions.Add(new KeyValuePair<string, string>("indentOutput", _agent.Configuration.IndentOutput.ToString()));

            if (outputComments.HasValue) formatOptions.Add(new KeyValuePair<string, string>("outputComments", outputComments.Value.ToString()));
            else formatOptions.Add(new KeyValuePair<string, string>("outputComments", _agent.Configuration.OutputComments.ToString()));

            var response = await MTConnectHttpRequests.GetProbeRequest(_agent, deviceType, version, documentFormat, formatOptions);

            _logger.LogInformation($"[Api-Interface] : {Request.Host} : [{Request.Method}] : {Request.Path} : {Request.QueryString} : Response ({response.StatusCode}) in {response.ResponseDuration}ms");

            return CreateResult(response);
        }

        /// <summary>
        /// An Agent responds to a Current Request with an MTConnectStreams Response Document that contains
        /// the current value of Data Entities associated with each piece of Streaming Data available from the Agent, subject to any filtering defined in the Request.
        /// </summary>
        /// <param name="path">An XPath that defines specific information or a set of information to be included in an MTConnectStreams Response Document.</param>
        /// <param name="at">Requests that the MTConnect Response Documents MUST include the current value for all Data Entities relative to the time that a specific sequence number was recorded.</param>
        /// <param name="interval">The Agent MUST continuously publish Response Documents when the query parameters include interval using the value as the minimum period between adjacent publications.</param>
        /// <param name="version">The MTConnect Version of the response document</param>
        /// <param name="documentFormat">The format of the response document</param>
        /// <param name="indentOutput">A boolean flag to indent the response document (pretty)</param>
        /// <returns></returns>
        [HttpGet("current")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status405MethodNotAllowed)]
        [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
        [ProducesResponseType(StatusCodes.Status431RequestHeaderFieldsTooLarge)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCurrent(
            [FromQuery] string deviceType = null,
            [FromQuery] string path = null,
            [FromQuery] long at = 0,
            [FromQuery] int interval = -1,
            [FromQuery] int heartbeat = 1000,
            [FromQuery] Version version = null,
            [FromQuery] string documentFormat = DocumentFormat.XML,
            [FromQuery] bool? indentOutput = null,
            [FromQuery] bool? outputComments = null
            )
        {
            // Set Format Options
            var formatOptions = new List<KeyValuePair<string, string>>();

            if (indentOutput.HasValue) formatOptions.Add(new KeyValuePair<string, string>("indentOutput", indentOutput.Value.ToString()));
            else formatOptions.Add(new KeyValuePair<string, string>("indentOutput", _agent.Configuration.IndentOutput.ToString()));

            if (outputComments.HasValue) formatOptions.Add(new KeyValuePair<string, string>("outputComments", outputComments.Value.ToString()));
            else formatOptions.Add(new KeyValuePair<string, string>("outputComments", _agent.Configuration.OutputComments.ToString()));

            if (interval > 0)
            {
                try
                {

                    _logger.LogInformation($"[Api-Interface] : {Request.Host} : Current Stream Requested at {interval}ms Interval (Heartbeat = {heartbeat}ms) : [{Request.Method}] : {Request.Path} : {Request.QueryString}");

                    // Create Sample Stream
                    var stream = new MTConnectHttpCurrentStream(_agent, null, path, interval, heartbeat, documentFormat, formatOptions);
                    stream.StreamStarted += (s, id) =>
                    {
                        _logger.LogInformation($"[Api-Interface] : {Request.Host} : Current Stream [{id}] Started at {interval}ms Interval (Heartbeat = {heartbeat}ms) : [{Request.Method}] : {Request.Path} : {Request.QueryString}");
                    };
                    stream.StreamStopped += (s, id) =>
                    {
                        _logger.LogInformation($"[Api-Interface] : {Request.Host} : Current Stream [{id}] Stopped : [{Request.Method}] : {Request.Path} : {Request.QueryString}");
                    };
                    stream.StreamException += (s, ex) =>
                    {
                        _logger.LogInformation($"[Api-Interface] : {Request.Host} : Current Stream [{stream.Id}] Error : {Request.Host} : {Request.Path} : {Request.QueryString} : {ex.Message}");
                    };
                    stream.HeartbeatReceived += async (s, args) =>
                    {
                        try
                        {
                            // Write the Multipart Chunk to the Response Stream
                            await Response.WriteAsync(args.Message, HttpContext.RequestAborted);

                            _logger.LogInformation($"[Api-Interface] : {Request.Host} : Current Stream [{args.StreamId}] : {Request.Host} : {Request.Path} : {Request.QueryString} : Heartbeat Sent : {args.ResponseDuration}ms");
                        }
                        catch { }
                    };
                    stream.DocumentReceived += async (s, args) =>
                    {
                        try
                        {
                            // Write the Multipart Chunk to the Response Stream
                            await Response.WriteAsync(args.Message, HttpContext.RequestAborted);

                            _logger.LogInformation($"[Api-Interface] : {Request.Host} : Current Stream [{args.StreamId}] : {Request.Host} : {Request.Path} : {Request.QueryString} : MTConnectStreams Document Sent : {args.ResponseDuration}ms");
                        }
                        catch { }
                    };

                    // Set HTTP Response Headers
                    Response.Headers.Add("Server", "MTConnectAgent");
                    Response.Headers.Add("Expires", "-1");
                    Response.Headers.Add("Connection", "close");
                    Response.Headers.Add("Cache-Control", "no-cache, private, max-age=0");
                    Response.Headers.Add("Content-Type", $"multipart/x-mixed-replace;boundary={stream.Boundary}");

                    // Start the MTConnectHttpStream
                    stream.Start(HttpContext.RequestAborted);

                    while (!HttpContext.RequestAborted.IsCancellationRequested) { await Task.Delay(100); }

                    _logger.LogInformation($"[Api-Interface] : {Request.Host} : Current Stream Request Closed : [{Request.Method}] : {Request.Path} : {Request.QueryString}");

                }
                catch { }

                return new EmptyResult();
            }
            else
            {
                _logger.LogInformation($"[Api-Interface] : {Request.Host} : Current Requested a MTConnectStreams Document : [{Request.Method}] : {Request.Path} : {Request.QueryString}");

                var response = await MTConnectHttpRequests.GetCurrentRequest(_agent, deviceType, path, at, interval, version, documentFormat, formatOptions);

                _logger.LogInformation($"[Api-Interface] : {Request.Host} : [{Request.Method}] : {Request.Path} : {Request.QueryString} : Current Response ({response.StatusCode}) in {response.ResponseDuration}ms");

                return CreateResult(response);
            }
        }

        /// <summary>
        /// An Agent responds to a Sample Request with an MTConnectStreams Response Document that contains a set of values for Data Entities
        /// currently available for Streaming Data from the Agent, subject to any filtering defined in the Request.
        /// </summary>
        /// <param name="path">An XPath that defines specific information or a set of information to be included in an MTConnectStreams Response Document.</param>
        /// <param name="from">The from parameter designates the sequence number of the first observation in the buffer the Agent MUST consider publishing in the Response Document.</param>
        /// <param name="to">The to parameter specifies the sequence number of the observation in the buffer that will be the upper bound of the observations in the Response Document.</param>
        /// <param name="count">The count parameter designates the maximum number of observations the Agent MUST publish in the Response Document.</param>
        /// <param name="interval">The Agent MUST continuously publish Response Documents when the query parameters include interval using the value as the minimum period between adjacent publications.</param>
        /// <param name="heartbeat">Sets the time period for the heartbeat function in an Agent.</param>
        /// <param name="version">The MTConnect Version of the response document</param>
        /// <param name="documentFormat">The format of the response document</param>
        /// <param name="indentOutput">A boolean flag to indent the response document (pretty)</param>
        [HttpGet("sample")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status405MethodNotAllowed)]
        [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
        [ProducesResponseType(StatusCodes.Status431RequestHeaderFieldsTooLarge)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetSample(
            [FromQuery] string deviceType = null,
            [FromQuery] string path = null,
            [FromQuery] long from = 0,
            [FromQuery] long to = 0,
            [FromQuery] int count = 100,
            [FromQuery] int interval = -1,
            [FromQuery] int heartbeat = 1000,
            [FromQuery] Version version = null,
            [FromQuery] string documentFormat = DocumentFormat.XML,
            [FromQuery] bool? indentOutput = null,
            [FromQuery] bool? outputComments = null
            )
        {
            // Set Format Options
            var formatOptions = new List<KeyValuePair<string, string>>();

            if (indentOutput.HasValue) formatOptions.Add(new KeyValuePair<string, string>("indentOutput", indentOutput.Value.ToString()));
            else formatOptions.Add(new KeyValuePair<string, string>("indentOutput", _agent.Configuration.IndentOutput.ToString()));

            if (outputComments.HasValue) formatOptions.Add(new KeyValuePair<string, string>("outputComments", outputComments.Value.ToString()));
            else formatOptions.Add(new KeyValuePair<string, string>("outputComments", _agent.Configuration.OutputComments.ToString()));

            if (interval > 0)
            {
                try
                {

                    _logger.LogInformation($"[Api-Interface] : {Request.Host} : Samples Stream Requested at {interval}ms Interval (Heartbeat = {heartbeat}ms) : [{Request.Method}] : {Request.Path} : {Request.QueryString}");

                    // Create Sample Stream
                    var stream = new MTConnectHttpSampleStream(_agent, null, path, from, count, interval, heartbeat, documentFormat, formatOptions);
                    stream.StreamStarted += (s, id) =>
                    {
                        _logger.LogInformation($"[Api-Interface] : {Request.Host} : Samples Stream [{id}] Started at {interval}ms Interval (Heartbeat = {heartbeat}ms) : [{Request.Method}] : {Request.Path} : {Request.QueryString}");
                    };
                    stream.StreamStopped += (s, id) =>
                    {
                        _logger.LogInformation($"[Api-Interface] : {Request.Host} : Samples Stream [{id}] Stopped : [{Request.Method}] : {Request.Path} : {Request.QueryString}");
                    };
                    stream.StreamException += (s, ex) =>
                    {
                        _logger.LogInformation($"[Api-Interface] : {Request.Host} : Sample Stream [{stream.Id}] Error : {Request.Host} : {Request.Path} : {Request.QueryString} : {ex.Message}");
                    };
                    stream.HeartbeatReceived += async (s, args) =>
                    {
                        try
                        {
                            // Write the Multipart Chunk to the Response Stream
                            await Response.WriteAsync(args.Message, HttpContext.RequestAborted);

                            _logger.LogInformation($"[Api-Interface] : {Request.Host} : Sample Stream [{args.StreamId}] : {Request.Host} : {Request.Path} : {Request.QueryString} : Heartbeat Sent : {args.ResponseDuration}ms");
                        }
                        catch { }
                    };
                    stream.DocumentReceived += async (s, args) =>
                    {
                        try
                        {
                            // Write the Multipart Chunk to the Response Stream
                            await Response.WriteAsync(args.Message, HttpContext.RequestAborted);

                            _logger.LogInformation($"[Api-Interface] : {Request.Host} : Sample Stream [{args.StreamId}] : {Request.Host} : {Request.Path} : {Request.QueryString} : MTConnectStreams Document Sent : {args.ResponseDuration}ms");
                        }
                        catch { }
                    };

                    // Set HTTP Response Headers
                    Response.Headers.Add("Server", "MTConnectAgent");
                    Response.Headers.Add("Expires", "-1");
                    Response.Headers.Add("Connection", "close");
                    Response.Headers.Add("Cache-Control", "no-cache, private, max-age=0");
                    Response.Headers.Add("Content-Type", $"multipart/x-mixed-replace;boundary={stream.Boundary}");

                    // Start the MTConnectHttpStream
                    stream.Start(HttpContext.RequestAborted);

                    while (!HttpContext.RequestAborted.IsCancellationRequested) { await Task.Delay(100); }

                    _logger.LogInformation($"[Api-Interface] : {Request.Host} : Samples Stream Request Closed : [{Request.Method}] : {Request.Path} : {Request.QueryString}");

                }
                catch { }

                return new EmptyResult();
            }
            else
            {
                _logger.LogInformation($"[Api-Interface] : {Request.Host} : Samples Requested a MTConnectStreams Document : [{Request.Method}] : {Request.Path} : {Request.QueryString}");

                var response = await MTConnectHttpRequests.GetSampleRequest(_agent, deviceType, path, from, to, count, version, documentFormat, formatOptions);

                _logger.LogInformation($"[Api-Interface] : {Request.Host} : [{Request.Method}] : {Request.Path} : {Request.QueryString} : Samples Response ({response.StatusCode}) in {response.ResponseDuration}ms");

                return CreateResult(response);
            }
        }


        [HttpPut]
        [DataItemsQueryFilter]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PutDataItems([FromRoute] string deviceName, [FromQuery] Dictionary<string, string> items)
        {
            if (_agent != null)
            {
                if (!items.IsNullOrEmpty())
                {
                    foreach (var item in items)
                    {
                        //var dataItem = await _agent.GetDataItem(deviceName, item.Key);
                        //if (dataItem != null)
                        //{
                        //    switch (dataItem.DataItemCategory)
                        //    {
                        //        case DataItemCategory.SAMPLE:
                        //            await _agent.AddObservationAsync(deviceName, dataItem.Id, "CDATA", item.Value);
                        //            _logger?.LogInformation($"Sample Added via HTTP PUT : {deviceName} : {dataItem.Id} = {item.Value} (CDATA)");
                        //            break;

                        //        case DataItemCategory.EVENT:
                        //            await _agent.AddObservationAsync(deviceName, dataItem.Id, "CDATA", item.Value);
                        //            _logger?.LogInformation($"Event Added via HTTP PUT : {deviceName} : {dataItem.Id} = {item.Value} (CDATA)");
                        //            break;

                        //        case DataItemCategory.CONDITION:

                        //            ////string level = null;
                        //            ////string nativeCode = null;
                        //            ////string nativeSeverity = null;
                        //            ////string qualifier = null;
                        //            ////string cdata = null;

                        //            //var condition = new Adapters.Condition();
                        //            //condition.DataItemId = dataItem.Id;
                        //            //condition.Timestamp = UnixDateTime.Now;

                        //            //// Set Level
                        //            //var x = GetNextConditionValue(item.Value);
                        //            //var y = GetNextConditionSegment(item.Value);
                        //            //condition.Level = x;

                        //            //if (y != null)
                        //            //{
                        //            //    // Set NativeCode
                        //            //    x = GetNextConditionValue(y);
                        //            //    y = GetNextConditionSegment(y);
                        //            //    condition.NativeCode = x;

                        //            //    if (y != null)
                        //            //    {
                        //            //        // Set NativeSeverity
                        //            //        x = GetNextConditionValue(y);
                        //            //        y = GetNextConditionSegment(y);
                        //            //        condition.NativeSeverity = x;

                        //            //        if (y != null)
                        //            //        {
                        //            //            // Set Qualifier
                        //            //            x = GetNextConditionValue(y);
                        //            //            y = GetNextConditionSegment(y);
                        //            //            condition.Qualifier = x;
                        //            //        }
                        //            //    }
                        //            //}
                        //            ////else
                        //            ////{
                        //            //    // Set CDATA
                        //            //    x = GetNextConditionValue(y);
                        //            //    condition.Text = x;
                        //            ////}

                        //            // system=fault|XXX|1|LOW|Feeling%20low
                        //            //var now = UnixDateTime.Now;

                        //            var condition = Adapters.Shdr.ShdrCondition.FromString(item.Value);
                        //            condition.DeviceName = deviceName;
                        //            condition.Key = item.Key;

                        //            await _agent.AddConditionObservationAsync(deviceName, condition);
                        //            _logger?.LogInformation($"Condition Added via HTTP PUT : {deviceName} : {dataItem.Id} = {item.Value} (Level)");

                        //            break;



                        //            //case "SAMPLE":
                        //            //    await _agent.AddDataItem(deviceName, dataItem.Id, "CDATA", item.Value);
                        //            //    _logger?.LogInformation($"Sample Added via HTTP PUT : {deviceName} : {dataItem.Id} = {item.Value} (CDATA)");
                        //            //    break;

                        //            //case "EVENT":
                        //            //    await _agent.AddDataItem(deviceName, dataItem.Id, "CDATA", item.Value);
                        //            //    _logger?.LogInformation($"Event Added via HTTP PUT : {deviceName} : {dataItem.Id} = {item.Value} (CDATA)");
                        //            //    break;

                        //            //case "CONDITION":

                        //            //    //string level = null;
                        //            //    //string nativeCode = null;
                        //            //    //string nativeSeverity = null;
                        //            //    //string qualifier = null;
                        //            //    //string cdata = null;

                        //            //    var condition = new Adapters.Condition();
                        //            //    condition.DataItemId = dataItem.Id;
                        //            //    condition.Timestamp = UnixDateTime.Now;

                        //            //    // Set Level
                        //            //    var x = GetNextConditionValue(item.Value);
                        //            //    var y = GetNextConditionSegment(item.Value);
                        //            //    condition.Level = x;

                        //            //    y = GetNextConditionSegment(y);
                        //            //    if (y != null)
                        //            //    {
                        //            //        // Set NativeCode
                        //            //        x = GetNextConditionValue(y);
                        //            //        y = GetNextConditionSegment(y);
                        //            //        condition.NativeCode = x;
                        //            //    }
                        //            //    else
                        //            //    {
                        //            //        // Set CDATA
                        //            //        x = GetNextConditionValue(y);
                        //            //        condition.Text = x;
                        //            //    }

                        //            //    // system=fault|XXX|1|LOW|Feeling%20low
                        //            //    //var now = UnixDateTime.Now;
                        //            //    await _agent.AddCondition(deviceName, dataItem.Id, condition);
                        //            //    _logger?.LogInformation($"Condition Added via HTTP PUT : {deviceName} : {dataItem.Id} = {item.Value} (Level)");

                        //            //    break;
                        //    }
                        //}
                    }

                    return new ContentResult
                    {
                        Content = "<success/>",
                        ContentType = "text/xml",
                        StatusCode = 200
                    };
                }
                else
                {
                    return BadRequest();
                }
            }

            return StatusCode(500);
        }


        private IActionResult CreateResult(MTConnectHttpResponse response)
        {
            return new ContentResult
            {
                Content = response.Content,
                ContentType = response.ContentType,
                StatusCode = response.StatusCode
            };
        }
    }
}
