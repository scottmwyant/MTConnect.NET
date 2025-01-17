// Copyright (c) 2022 TrakHound Inc., All Rights Reserved.

// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

namespace MTConnect.Observations.Events.Values
{
    /// <summary>
    /// An indication that a piece of equipment, or a sub-part of a piece of equipment, is performing specific types of activities.
    /// </summary>
    public static class EquipmentModeDescriptions
    {
        /// <summary>
        /// The equipment is not functioning in the mode designated by the subType.
        /// </summary>
        public const string OFF = "The equipment is not functioning in the mode designated by the subType.";

        /// <summary>
        /// The equipment is functioning in the mode designated by the subType.
        /// </summary>
        public const string ON = "The equipment is functioning in the mode designated by the subType.";


        public static string Get(EquipmentMode value)
        {
            switch (value)
            {
                case EquipmentMode.OFF: return OFF;
                case EquipmentMode.ON: return ON;
            }

            return null;
        }
    }
}
