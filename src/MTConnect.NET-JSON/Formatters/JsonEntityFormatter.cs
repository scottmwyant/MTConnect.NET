﻿// Copyright (c) 2022 TrakHound Inc., All Rights Reserved.

// This file is subject to the terms and conditions defined in
// file 'LICENSE', which is part of this source code package.

using MTConnect.Assets;
using MTConnect.Devices;
using MTConnect.Observations;
using System;
using System.Collections.Generic;
using System.Linq;
using MTConnect.Assets.Json;
using MTConnect.Devices.Json;
using MTConnect.Streams.Json;

namespace MTConnect.Formatters
{
    public class JsonEntityFormatter : IEntityFormatter
    {
        public string Id => "JSON";

        public string ContentType => "application/json";


        public string Format(IDevice device)
        {
            if (device != null)
            {
                return new JsonDevice(device).ToString();
            }

            return null;
        }

        public string Format(IComponent component)
        {
            if (component != null)
            {
                return new JsonComponent(component).ToString();
            }

            return null;
        }

        public string Format(IComposition composition)
        {
            if (composition != null)
            {
                return new JsonComposition(composition).ToString();
            }

            return null;
        }

        public string Format(IDataItem dataItem)
        {
            if (dataItem != null)
            {
                return new JsonDataItem(dataItem).ToString();
            }

            return null;
        }

        public string Format(IObservation observation)
        {
            if (observation != null)
            {
                switch (observation.Category)
                {
                    case Devices.DataItems.DataItemCategory.SAMPLE:
                        var sampleObservation = SampleObservation.Create(observation);
                        if (sampleObservation != null)
                        {
                            return JsonFunctions.Convert(new JsonSample(sampleObservation));
                        }
                        break;

                    case Devices.DataItems.DataItemCategory.EVENT:
                        var eventObservation = EventObservation.Create(observation);
                        if (eventObservation != null)
                        {
                            return JsonFunctions.Convert(new JsonEvent(eventObservation));
                        }
                        break;
                }



                //return new JsonObservation(observation).ToString();
            }

            return null;
        }

        public string Format(IAsset asset)
        {
            if (asset != null)
            {
                return JsonFunctions.Convert(asset);
            }

            return null;
        }


        public FormattedEntityReadResult<IDevice> CreateDevice(string content, IEnumerable<KeyValuePair<string, string>> options = null)
        {
            //var messages = new List<string>();
            //var warnings = new List<string>();
            //var errors = new List<string>();

            //// Read Document
            //var entity = JsonDevice.FromJson(content);
            //var success = entity != null;

            //return new FormattedEntityReadResult<IDevice>(entity, success, messages, warnings, errors);

            return new FormattedEntityReadResult<IDevice>();
        }

        public FormattedEntityReadResult<IAsset> CreateAsset(string assetType, string content, IEnumerable<KeyValuePair<string, string>> options = null)
        {
            //var messages = new List<string>();
            //var warnings = new List<string>();
            //var errors = new List<string>();

            //// Read Document
            //var entity = JsonAsset.FromJson(assetType, content);
            //var success = entity != null;

            //return new FormattedEntityReadResult<IAsset>(entity, success, messages, warnings, errors);

            return new FormattedEntityReadResult<IAsset>();
        }


        private static T GetFormatterOption<T>(IEnumerable<KeyValuePair<string, string>> options, string key)
        {
            if (!options.IsNullOrEmpty())
            {
                var x = options.FirstOrDefault(o => o.Key == key).Value;
                if (!string.IsNullOrEmpty(x))
                {
                    try
                    {
                        return (T)Convert.ChangeType(x, typeof(T));
                    }
                    catch { }
                }
            }

            return default;
        }

        private static IEnumerable<T> GetFormatterOptions<T>(IEnumerable<KeyValuePair<string, string>> options, string key)
        {
            var l = new List<T>();

            if (!options.IsNullOrEmpty())
            {
                var x = options.Where(o => o.Key == key);
                if (!x.IsNullOrEmpty())
                {
                    foreach (var y in x)
                    {
                        if (!string.IsNullOrEmpty(y.Value))
                        {
                            try
                            {
                                var obj = (T)Convert.ChangeType(y.Value, typeof(T));
                                l.Add(obj);
                            }
                            catch { }
                        }
                    }
                }
            }

            return l;
        }
    }
}
