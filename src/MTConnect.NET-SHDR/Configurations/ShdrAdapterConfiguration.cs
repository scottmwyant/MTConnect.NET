// Copyright (c) 2022 TrakHound Inc., All Rights Reserved.

// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System.Text.Json.Serialization;

namespace MTConnect.Configurations
{
    /// <summary>
    /// Configuration for an MTConnect SHDR Adapter
    /// </summary>
    public class ShdrAdapterConfiguration : ShdrClientConfiguration
    {
        /// <summary>
        /// For devices that do not have the ability to provide available events, if yes, this sets the Availability to AVAILABLE upon connection.
        /// </summary>
        [JsonPropertyName("availableOnConnection")]
        public bool AvailableOnConnection { get; set; }

        /// <summary>
        /// Overwrite timestamps with the agent time. This will correct clock drift but will not give as accurate relative time since it will not take into consideration network latencies. 
        /// This can be overridden on a per adapter basis.
        /// </summary>
        [JsonPropertyName("ignoreTimestamps")]
        public bool IgnoreTimestamps { get; set; }

        /// <summary>
        /// Adapter setting for data item units conversion in the agent. Assumes the adapter has already done unit conversion. Defaults to global.
        /// </summary>
        [JsonPropertyName("convertUnits")]
        public bool ConvertUnits { get; set; }

        /// <summary>
        /// Gets or Sets the default for Ignoring the case of Observation values
        /// </summary>
        [JsonPropertyName("ignoreObservationCase")]
        public bool IgnoreObservationCase { get; set; }

        /// <summary>
        /// Gets or Sets whether the Connection Information (Host / Port) is output to the Agent to be collected by a client
        /// </summary>
        [JsonPropertyName("outputConnectionInformation")]
        public bool OutputConnectionInformation { get; set; }


        public ShdrAdapterConfiguration()
        {
            AvailableOnConnection = false;
            IgnoreTimestamps = false;
            ConvertUnits = true;
            IgnoreObservationCase = true;
            OutputConnectionInformation = true;
        }
    }
}
