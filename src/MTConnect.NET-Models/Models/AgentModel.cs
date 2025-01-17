// Copyright (c) 2022 TrakHound Inc., All Rights Reserved.

// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using MTConnect.Devices.Components;
using MTConnect.Devices.DataItems.Events;
using MTConnect.Models.Components;
using MTConnect.Observations.Events.Values;

namespace MTConnect.Models
{
    /// <summary>
    /// MTConnect Device Model used to access MTConnectDevices, MTConnectSreams, and MTConnectAssets data in a single object
    /// </summary>
    public class AgentModel : AbstractDeviceModel, IAgentModel
    {
        /// <summary>
        /// Represents the Agent’s ability to communicate with the data source.
        /// </summary>
        public Availability Availability
        {
            get => DataItemManager.GetDataItemValue<Availability>(Devices.DataItem.CreateId(Id, Devices.DataItems.Events.AvailabilityDataItem.NameId));
            set => DataItemManager.AddDataItem(new AvailabilityDataItem(Id), value);
        }
        public IDataItemModel AvailabilityDataItem => DataItemManager.GetDataItem(Devices.DataItems.Events.AvailabilityDataItem.NameId);


        /// <summary>
        /// Adapter is a Component that represents the connectivity state of a data source for the MTConnect Agent.
        /// </summary>
        public IAdaptersModel Adapters => ComponentManager.GetComponentModel<AdaptersModel>(typeof(AdaptersComponent));
    }
}
