// Copyright (c) 2022 TrakHound Inc., All Rights Reserved.

// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

namespace MTConnect.Observations.Events.Values
{
    /// <summary>
    /// An indicator of the state of the axis lockout function when power has been removed and the axis is allowed to move freely.
    /// </summary>
    public static class AxisInterlockDescriptions
    {
        /// <summary>
        /// The axis lockout function has not been activated, the axis may be powered, and the axis is capable of being controlled by another component.
        /// </summary>
        public const string INACTIVE = "The axis lockout function has not been activated, the axis may be powered, and the axis is capable of being controlled by another component.";

        /// <summary>
        /// The axis lockout function is activated, power has been removed from the axis, and the axis is allowed to move freely.
        /// </summary>
        public const string ACTIVE = "The axis lockout function is activated, power has been removed from the axis, and the axis is allowed to move freely.";


        public static string Get(AxisInterlock value)
        {
            switch (value)
            {
                case AxisInterlock.INACTIVE: return INACTIVE;
                case AxisInterlock.ACTIVE: return ACTIVE;
            }

            return null;
        }
    }
}
