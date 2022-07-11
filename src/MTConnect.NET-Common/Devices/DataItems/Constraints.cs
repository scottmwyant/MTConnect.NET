// Copyright (c) 2022 TrakHound Inc., All Rights Reserved.

// This file is subject to the terms and conditions defined in
// file 'LICENSE', which is part of this source code package.

using System.Collections.Generic;
using System.Xml.Serialization;
using System.Text.Json.Serialization;

namespace MTConnect.Devices.DataItems
{
    /// <summary>
    /// Constraints provide a way to define the allowable value(s) or the upper and lower limits for the range of values that can be reported for the data by an MTConnect Agent in response to a Current or Sample request.
    /// Constraints also provide a means to suppress multiple occurrences of data values where the change in value is below a threshold defined bya Filter attribute.
    /// This is effective to reduce the amount of data generated by a 'noisy' data source.
    /// </summary>
    public class Constraints : IConstraints
    {
        public const string DescriptionText = "Constraints provide a way to define the allowable value(s) or the upper and lower limits for the range of values that can be reported for the data by an MTConnect Agent in response to a Current or Sample request. Constraints also provide a means to suppress multiple occurrences of data values where the change in value is below a threshold defined bya Filter attribute. This is effective to reduce the amount of data generated by a 'noisy' data source.";


        /// <summary>
        /// If data reported for a DataItem is a range of numeric values, then the value reported MAY be bounded with an upper limit defined by this constraint.
        /// </summary>
        [XmlElement("Maximum")]
        [JsonPropertyName("maximum")]
        public string Maximum { get; set; }

        /// <summary>
        /// If the data reported for a DataItem is a range of numeric values, the value reported MAY be bounded with a lower limit defined by this constraint.
        /// </summary>
        [XmlElement("Minimum")]
        [JsonPropertyName("minimum")]
        public string Minimum { get; set; }

        /// <summary>
        /// The target or expected value for this data item.
        /// </summary>
        [XmlElement("Nominal")]
        [JsonPropertyName("nominal")]
        public string Nominal { get; set; }

        /// <summary>
        /// A Data Element that defines a valid value for the data provided for a DataItem.
        /// When the data reported for a DataItem is a descriptive type of data (not numeric data), then Value MAY be used to define a valid descriptor for the DataItem.
        /// Multiple Value Data Elements may be defined for any DataItem and each valid value MUST be defined by a Value Data Element.
        /// If there is only one Value Data Element defined for a DataItem, the value will be constant and cannot change. 
        /// In the case of a constant value, the value is not required to be supplied in the XML document provided by an MTConnect Agent in response to a Current or Sample request.
        /// </summary>
        [XmlElement("Value")]
        [JsonPropertyName("value")]
        public List<string> Values { get; set; }

        /// <summary>
        /// DEPRECATED in Version 1.4 – Moved to the Filters element of a DataItem.
        /// If the data reported for a DataItem is a numeric value, a new value MUST NOT be reported if the change from the last reported value is less than the delta given as the Value of this element.
        /// Filter is an abstract type XML element. As such, Filter will never appear in the XML document, but will be replaced by a Filter type.
        /// The only currently supported Filter type if MINIMUM_DELTA.
        /// The Value MUST be an absolute value using the same Units as the reported data.
        /// Additional filter type MAY be supported in the future.
        /// </summary>
        [XmlElement("Filter")]
        [JsonPropertyName("filter")]
        public Filter Filter { get; set; }
    }
}
