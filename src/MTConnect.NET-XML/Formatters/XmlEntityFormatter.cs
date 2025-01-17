﻿// Copyright (c) 2022 TrakHound Inc., All Rights Reserved.

// This file is subject to the terms and conditions defined in
// file 'LICENSE', which is part of this source code package.

using MTConnect.Assets;
using MTConnect.Devices;
using MTConnect.Observations;
using MTConnect.Streams;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MTConnect.Formatters
{
    public class XmlEntityFormatter : IEntityFormatter
    {
        public string Id => "XML";

        public string ContentType => "application/xml";


        public string Format(IDevice device)
        {
            if (device != null)
            {
                return new XmlDevice(device).ToString();
            }

            return null;
        }

        public string Format(IComponent component)
        {
            if (component != null)
            {
                return new XmlComponent(component).ToString();
            }

            return null;
        }

        public string Format(IComposition composition)
        {
            if (composition != null)
            {
                return new XmlComposition(composition).ToString();
            }

            return null;
        }

        public string Format(IDataItem dataItem)
        {
            if (dataItem != null)
            {
                return new XmlDataItem(dataItem).ToString();
            }

            return null;
        }

        public string Format(IObservation observation)
        {
            if (observation != null)
            {
                return XmlObservation.ToXml(observation, true);
            }

            return null;
        }

        public string Format(IAsset asset)
        {
            if (asset != null)
            {
                return XmlAsset.ToXml(asset, true);
            }

            return null;
        }


        public FormattedEntityReadResult<IDevice> CreateDevice(string content, IEnumerable<KeyValuePair<string, string>> options = null)
        {
            var messages = new List<string>();
            var warnings = new List<string>();
            var errors = new List<string>();

            // Read Document
            var entity = XmlDevice.FromXml(content);
            var success = entity != null;

            return new FormattedEntityReadResult<IDevice>(entity, success, messages, warnings, errors);
        }

        public FormattedEntityReadResult<IAsset> CreateAsset(string assetType, string content, IEnumerable<KeyValuePair<string, string>> options = null)
        {
            var messages = new List<string>();
            var warnings = new List<string>();
            var errors = new List<string>();

            // Read Document
            var entity = XmlAsset.FromXml(assetType, content);
            var success = entity != null;

            return new FormattedEntityReadResult<IAsset>(entity, success, messages, warnings, errors);
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
