// Copyright (c) 2022 TrakHound Inc., All Rights Reserved.

// This file is subject to the terms and conditions defined in
// file 'LICENSE', which is part of this source code package.

using System.Xml.Serialization;
using System.Text.Json.Serialization;

namespace MTConnect.Streams
{
    /// <summary>
    /// An abstract XML Element. Replaced in the XML document by type(s) of Condition type Data Elements representing CONDITION category data items defined for a Device in the Device Information Model.
    /// There can be multiple types of Condition type XML Elements in a Condition container.
    /// </summary>
    public class Condition : DataItem, ICondition
    {
        public Condition()
        {
            Category = Devices.DataItemCategory.CONDITION;
            Level = ConditionLevel.UNAVAILABLE;
        }


        /// <summary>
        /// Level of the Condition (Normal, Warning, Fault, or Unavailable)
        /// </summary>
        [XmlIgnore]
        [JsonPropertyName("level")]
        public ConditionLevel Level { get; set; }

        /// <summary>
        /// The native code (usually an alpha-numeric value) generated by the controller of a piece of equipment providing a reference identifier for a condition state or alarm.
        /// This is the same information an operator or maintenance personnel would see as a reference code designating a specific type of Condition when viewed at the piece of equipment.Usually this reference code is used to point to a more detailed description of the Condition.
        /// </summary>
        [XmlAttribute("nativeCode")]
        [JsonPropertyName("nativeCode")]
        public string NativeCode { get; set; }

        /// <summary>
        /// If the data source assigns a severity level to a Condition, nativeSeverity is used to report that severity information to a client software application.
        /// </summary>
        [XmlAttribute("nativeSeverity")]
        [JsonPropertyName("nativeSeverity")]
        public string NativeSeverity { get; set; }

        /// <summary>
        /// Qualifies the Condition and adds context or additional clarification.
        /// This optional attribute can be used to convey information such as HIGH or LOW type Warning and Fault condition to indicate differing types of condition states
        /// </summary>
        [XmlAttribute("qualifier")]
        [JsonPropertyName("qualifier")]
        public ConditionQualifier Qualifier { get; set; }

        /// <summary>
        /// The type of statistical calculation specified for the DataItem defined in the Device Information Model that this Condition element represents.
        /// </summary>
        [XmlAttribute("statistic")]
        [JsonPropertyName("statistic")]
        public string Statistic { get; set; }


        public static Condition Normal()
        {
            return new Condition
            {
                Level = ConditionLevel.NORMAL,
                Timestamp = System.DateTime.UtcNow
            };
        }

        public static Condition Warning(
            string nativeCode = null,
            string nativeSeverity = null,
            ConditionQualifier qualifier = ConditionQualifier.NOT_SPECIFIED,
            string message = null
            )
        {
            return new Condition
            {
                Level = ConditionLevel.WARNING,
                NativeCode = nativeCode,
                NativeSeverity = nativeSeverity,
                Qualifier = qualifier,
                CDATA = message,
                Timestamp = System.DateTime.UtcNow
            };
        }

        public static Condition Fault(
            string nativeCode = null,
            string nativeSeverity = null,
            ConditionQualifier qualifier = ConditionQualifier.NOT_SPECIFIED,
            string message = null
            )
        {
            return new Condition
            {
                Level = ConditionLevel.FAULT,
                NativeCode = nativeCode,
                NativeSeverity = nativeSeverity,
                Qualifier = qualifier,
                CDATA = message,
                Timestamp = System.DateTime.UtcNow
            };
        }
    }
}
