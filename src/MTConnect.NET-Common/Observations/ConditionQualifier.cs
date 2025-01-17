﻿// Copyright (c) 2022 TrakHound Inc., All Rights Reserved.

// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

namespace MTConnect.Observations
{
    /// <summary>
    /// Provides additional information regarding a Fault State associated with the measured value of a process variable.
    /// </summary>
    public enum ConditionQualifier
    {
        /// <summary>
        /// A measured value that is less than the expected value for the process variable
        /// </summary>
        LOW = -1,

        /// <summary>
        /// No Qualifier is Specified
        /// </summary>
        NOT_SPECIFIED = 0,

        /// <summary>
        /// A measured value that is greater than the expected value for the process variable
        /// </summary>
        HIGH = 1
    }
}
