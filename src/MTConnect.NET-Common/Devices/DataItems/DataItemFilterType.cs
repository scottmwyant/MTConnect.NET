// Copyright (c) 2022 TrakHound Inc., All Rights Reserved.

// This file is subject to the terms and conditions defined in
// file 'LICENSE', which is part of this source code package.

namespace MTConnect.Devices.DataItems
{
    /// <summary>
    /// Filter provides a means to control when an Agent records updated information for a data item.
    /// </summary>
    public enum DataItemFilterType
    {
        /// <summary>
        /// For a MINIMUM_DELTA type Filter, a new value MUST NOT be reported for a data item unless the measured value
        /// has changed from the last reported value by at least the delta given as the Value of this element.
        /// </summary>
        MINIMUM_DELTA,

        /// <summary>
        /// For a PERIOD type Filter, the data reported for a data item is provided on a periodic basis. The PERIOD for reporting data is defined in the Value for the Filter.
        /// </summary>
        PERIOD
    }
}
