﻿// Copyright (c) 2022 TrakHound Inc., All Rights Reserved.

// This file is subject to the terms and conditions defined in
// file 'LICENSE', which is part of this source code package.

using MTConnect.Configurations;
using MTConnect.Devices.Components;
using MTConnect.Devices.DataItems;
using MTConnect.Devices.DataItems.Events;
using MTConnect.Devices.DataItems.Samples;
using System.Collections.Generic;

namespace MTConnect.Devices
{
    /// <summary>
    /// An MTConnect Component that represents an Http Adapter
    /// </summary>
    public class HttpAdapterComponent : AdapterComponent
    {
        public string BaseUri { get; set; }

        public string Uri { get; set; }


        /// <summary>
        /// Add a new Adapter Component to the Agent Device
        /// </summary>
        public HttpAdapterComponent(HttpClientConfiguration configuration)
        {
            if (configuration != null && !string.IsNullOrEmpty(configuration.Address))
            {
                Id = configuration.Id;
                Name = "adapterHttp";
                BaseUri = HttpClientConfiguration.CreateBaseUri(configuration);
                if (!string.IsNullOrEmpty(BaseUri))
                {
                    Uri = $"{BaseUri.TrimEnd('/')}/{configuration.DeviceKey}";
                }


                var dataItems = new List<IDataItem>();

                // Add Connection Status
                var connectionStatusDataItem = new ConnectionStatusDataItem(Id);
                dataItems.Add(connectionStatusDataItem);

                // Add Adapter URI DataItem
                if (configuration.OutputConnectionInformation)
                {
                    var adapterUriDataItem = new AdapterUriDataItem(Id);
                    var adapterUriConstraint = new Constraints();
                    adapterUriConstraint.Values = new List<string> { Uri };
                    adapterUriDataItem.Constraints = adapterUriConstraint;
                    dataItems.Add(adapterUriDataItem);
                }

                // Add Observation Update Rate
                dataItems.Add(new ObservationUpdateRateDataItem(Id));

                // Add Asset Update Rate
                dataItems.Add(new AssetUpdateRateDataItem(Id));

                DataItems = dataItems;
            }
        }
    }
}
