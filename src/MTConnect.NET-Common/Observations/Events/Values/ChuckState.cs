// Copyright (c) 2022 TrakHound Inc., All Rights Reserved.

// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

namespace MTConnect.Observations.Events.Values
{
    /// <summary>
    /// An indication of the operating state of a mechanism that holds a part or stock material during a manufacturing process. 
    /// It may also represent a mechanism that holds any other mechanism in place within a piece of equipment.
    /// </summary>
    public enum ChuckState
    {
        /// <summary>
        /// The CHUCK component or composition element is open to the point of a positive confirmation
        /// </summary>
        OPEN,

        /// <summary>
        /// The CHUCK component or composition element is not closed to the point of a positive confirmation and not open to the point of a positive confirmation.
        /// It is in an intermediate position.
        /// </summary>
        UNLATCHED,

        /// <summary>
        /// The CHUCK component or composition element is closed to the point of a positive confirmation
        /// </summary>
        CLOSED
    }
}
