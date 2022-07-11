// Copyright (c) 2022 TrakHound Inc., All Rights Reserved.

// This file is subject to the terms and conditions defined in
// file 'LICENSE', which is part of this source code package.

namespace MTConnect.Observations
{
    public static class ValueKeysDescriptions
    {
        /// <summary>
        /// DATA is used to describe a value (text or data) published as part of an XML element.
        /// </summary>
        public const string Result = "DATA is used to describe a value (text or data) published as part of an XML element.";

        /// <summary>
        /// Level of the Condition (Normal, Warning, Fault, or Unavailable)
        /// </summary>
        public const string Level = "Level of the Condition (Normal, Warning, Fault, or Unavailable)";

        /// <summary>
        /// The native code (usually an alpha-numeric value) generated by the controller of a piece of equipment providing a reference identifier for a condition state or alarm.
        /// </summary>
        public const string NativeCode = "The native code (usually an alpha-numeric value) generated by the controller of a piece of equipment providing a reference identifier for a condition state or alarm.";

        /// <summary>
        /// If the data source assigns a severity level to a Condition, nativeSeverity is used to report that severity information to a client software application.
        /// </summary>
        public const string NativeSeverity = "If the data source assigns a severity level to a Condition, nativeSeverity is used to report that severity information to a client software application.";

        /// <summary>
        /// Qualifies the Condition and adds context or additional clarification.
        /// </summary>
        public const string Qualifier = "Qualifies the Condition and adds context or additional clarification.";


        public static string Get(string valueKey)
        {
            switch (valueKey)
            {
                case ValueKeys.Result: return Result;
                case ValueKeys.Level: return Level;
                case ValueKeys.NativeCode: return NativeCode;
                case ValueKeys.NativeSeverity: return NativeSeverity;
                case ValueKeys.Qualifier: return Qualifier;
            }

            return "";
        }
    }
}
