// Copyright (c) 2022 TrakHound Inc., All Rights Reserved.

// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System.Collections.Generic;

namespace MTConnect.Observations.Input
{
    /// <summary>
    /// An Information Model Input that describes Streaming Data reported by a piece of equipment.
    /// </summary>
    public interface IObservationInput
    {
        /// <summary>
        /// The UUID of the Device that the Observation is associated with
        /// </summary>
        string DeviceKey { get; set; }

        /// <summary>
        /// The (ID, Name, or Source) of the DataItem that the Observation is associated with
        /// </summary>
        string DataItemKey { get; set; }

        /// <summary>
        /// The Values recorded during the Observation
        /// </summary>
        IEnumerable<ObservationValue> Values { get; set; }

        /// <summary>
        /// The timestamp (UnixTime in Milliseconds) that the observation was recorded at
        /// </summary>
        long Timestamp { get; set; }

        /// <summary>
        /// The time-period over which the data was collected.
        /// </summary>
        double Duration { get; set; }

        /// <summary>
        /// For those DataItem elements that report data that may be periodically reset to an initial value, 
        /// resetTriggered identifies when a reported value has been reset and what has caused that reset to occur.
        /// </summary>
        ResetTriggered ResetTriggered { get; set; }

        /// <summary>
        /// Gets or Sets whether the Observation is Unavailable
        /// </summary>
        bool IsUnavailable { get; set; }

        /// <summary>
        /// A MD5 Hash of the Observation that can be used for comparison
        /// </summary>
        string ChangeId { get; }


        void AddValue(string valueKey, object value);

        void AddValue(ObservationValue observationValue);

        string GetValue(string valueKey);
    }
}
