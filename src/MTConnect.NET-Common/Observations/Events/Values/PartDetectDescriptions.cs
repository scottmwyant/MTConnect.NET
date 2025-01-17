// Copyright (c) 2022 TrakHound Inc., All Rights Reserved.

// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

namespace MTConnect.Observations.Events.Values
{
    /// <summary>
    /// An indication designating whether a part or work piece has been detected or is present.
    /// </summary>
    public static class PartDetectDescriptions
    {
        /// <summary>
        /// If a part or work piece is not detected or is not present
        /// </summary>
        public const string NOT_PRESENT = "If a part or work piece is not detected or is not present";

        /// <summary>
        /// If a part or work piece has been detected or is present.
        /// </summary>
        public const string PRESENT = "If a part or work piece has been detected or is present.";


        public static string Get(PartDetect value)
        {
            switch (value)
            {
                case PartDetect.NOT_PRESENT: return NOT_PRESENT;
                case PartDetect.PRESENT: return PRESENT;
            }

            return null;
        }
    }
}
