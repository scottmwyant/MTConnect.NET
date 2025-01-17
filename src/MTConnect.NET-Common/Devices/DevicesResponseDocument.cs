// Copyright (c) 2022 TrakHound Inc., All Rights Reserved.

// This file is subject to the terms and conditions defined in
// file 'LICENSE', which is part of this source code package.

using MTConnect.Headers;
using MTConnect.Interfaces;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Xml;
using System.Xml.Serialization;

namespace MTConnect.Devices
{
    /// <summary>
    /// A document that contains information describing both the physical and logical structure of the piece of equipment
    /// and a detailed description of each Data Entity that can be reported by the Agent associated with the piece of equipment.
    /// </summary>
    [XmlRoot("MTConnectDevices")]
    public class DevicesResponseDocument : IDevicesResponseDocument
    {
        /// <summary>
        /// An XML container in an MTConnect Response Document that provides information from an Agent
        /// defining version information, storage capacity, and parameters associated with the data management within the Agent.
        /// </summary>
        [XmlElement("Header")]
        [JsonPropertyName("header")]
        public IMTConnectDevicesHeader Header { get; set; }

        /// <summary>
        /// The first, or highest level, Structural Element in a MTConnectDevices document.Devices is a container type XML element.
        /// </summary>
        [XmlIgnore]
        [JsonPropertyName("devices")]
        public virtual IEnumerable<IDevice> Devices { get; set; }


        [XmlArray("Interfaces")]
        [JsonPropertyName("interfaces")]
        public IEnumerable<IInterface> Interfaces { get; set; }

        [XmlIgnore]
        [JsonIgnore]
        public Version Version { get; set; }


        public IEnumerable<IComponent> GetComponents()
        {
            var components = new List<IComponent>();

            if (!Devices.IsNullOrEmpty())
            {
                foreach (var device in Devices)
                {
                    var deviceComponents = device.GetComponents();
                    if (!deviceComponents.IsNullOrEmpty()) components.AddRange(deviceComponents);
                }
            }

            return components;
        }

        public IEnumerable<IDataItem> GetDataItems()
        {
            var dataItems = new List<IDataItem>();

            if (!Devices.IsNullOrEmpty())
            {
                foreach (var device in Devices)
                {
                    var deviceDataItems = device.GetDataItems();
                    if (!deviceDataItems.IsNullOrEmpty()) dataItems.AddRange(deviceDataItems);
                }
            }

            return dataItems;
        }
    }
}
