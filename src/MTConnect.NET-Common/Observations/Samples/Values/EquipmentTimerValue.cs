// Copyright (c) 2022 TrakHound Inc., All Rights Reserved.

// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using MTConnect.Devices.DataItems.Samples;

namespace MTConnect.Observations.Samples.Values
{
    /// <summary>
    /// The measurement of the amount of time a piece of equipment or a sub-part of a piece of equipment has performed specific activities.
    /// </summary>
    public class EquipmentTimerValue : SampleValue
    {
        public EquipmentTimerValue(double nativeValue, string nativeUnits = EquipmentTimerDataItem.DefaultUnits)
        {
            Value = nativeValue;
            _units = EquipmentTimerDataItem.DefaultUnits;
            _nativeUnits = nativeUnits;
        }
    }
}
