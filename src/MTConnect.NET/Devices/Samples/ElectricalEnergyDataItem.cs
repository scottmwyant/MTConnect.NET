// Copyright (c) 2022 TrakHound Inc., All Rights Reserved.

// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

namespace MTConnect.Devices.Samples
{
    /// <summary>
    /// The value of Wattage used or generated by a component over an interval of time
    /// </summary>
    public class ElectricalEnergyDataItem : DataItem
    {
        public const DataItemCategory CategoryId = DataItemCategory.SAMPLE;
        public const string TypeId = "ELECTRICAL_ENERGY";
        public const string NameId = "elecEnergy";


        public ElectricalEnergyDataItem()
        {
            DataItemCategory = CategoryId;
            Type = TypeId;
            Units = Devices.Units.WATT_SECOND;
        }

        public ElectricalEnergyDataItem(string parentId)
        {
            Id = CreateId(parentId, NameId);
            DataItemCategory = CategoryId;
            Type = TypeId;
            Name = NameId;
            Units = Devices.Units.WATT_SECOND;
        }
    }
}