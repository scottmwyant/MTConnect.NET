// Copyright (c) 2022 TrakHound Inc., All Rights Reserved.

// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

namespace MTConnect.Observations.Events.Values
{
    /// <summary>
    /// The indication of the status of the source of energy for a Structural Element to allow it to perform
    /// its intended function or the state of an enabling signal providing permission for the Structural Element to perform its functions.
    /// </summary>
    public enum PowerState
    {
        /// <summary>
        /// The source of energy for a Structural Element or the enabling signal providing permission for the Structural Element to perform its function(s) is not present or is disconnected.
        /// </summary>
        OFF,

        /// <summary>
        /// The source of energy for a Structural Element or the enabling signal providing permission for the Structural Element to perform its function(s) is present and active.
        /// </summary>
        ON
    }
}
