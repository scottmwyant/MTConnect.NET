// Copyright (c) 2022 TrakHound Inc., All Rights Reserved.

// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

namespace MTConnect.Devices.DataItems.Events
{
    /// <summary>
    /// DeviceRemoved is an Event that provides the UUID of a device removed from an MTConnect Agent.
    /// </summary>
    public class DeviceRemovedDataItem : DataItem
    {
        public const DataItemCategory CategoryId = DataItemCategory.EVENT;
        public const string TypeId = "DEVICE_REMOVED";
        public const string NameId = "deviceRemoved";
        public new const string DescriptionText = "DeviceRemoved is an Event that provides the UUID of a device removed from an MTConnect Agent.";

        public override string TypeDescription => DescriptionText;

        public override System.Version MinimumVersion => MTConnectVersions.Version17;


        public DeviceRemovedDataItem()
        {
            Category = CategoryId;
            Type = TypeId;
        }

        public DeviceRemovedDataItem(string parentId)
        {
            Id = CreateId(parentId, NameId);
            Category = CategoryId;
            Type = TypeId;
            Name = NameId;
        }
    }
}
