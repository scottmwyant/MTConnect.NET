// Copyright (c) 2022 TrakHound Inc., All Rights Reserved.

// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using MTConnect.Observations;
using MTConnect.Observations.Input;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace MTConnect.Shdr
{
    /// <summary>
    /// An Observation representing an MTConnect Sample or EVENT with a Representation of DATA_SET
    /// </summary>
    public class ShdrDataSet : DataSetObservationInput
    {
        private static readonly Regex _resetTriggeredRegex = new Regex(@":([A-Z_]+)\s+(.*)");


        /// <summary>
        /// Flag to set whether the Observation has been sent by the adapter or not
        /// </summary>
        internal bool IsSent { get; set; }


        public ShdrDataSet() { }

        public ShdrDataSet(string dataItemKey)
        {
            DataItemKey = dataItemKey;
        }

        public ShdrDataSet(string dataItemKey, IEnumerable<IDataSetEntry> entries)
        {
            DataItemKey = dataItemKey;
            Entries = entries;
        }

        public ShdrDataSet(string dataItemKey, IEnumerable<IDataSetEntry> entries, long timestamp)
        {
            DataItemKey = dataItemKey;
            Entries = entries;
            Timestamp = timestamp;
        }

        public ShdrDataSet(string dataItemKey, IEnumerable<IDataSetEntry> entries, DateTime timestamp)
        {
            DataItemKey = dataItemKey;
            Entries = entries;
            Timestamp = timestamp.ToUnixTime();
        }

        public ShdrDataSet(DataSetObservationInput dataSetObservation)
        {
            if (dataSetObservation != null)
            {
                DeviceKey = dataSetObservation.DeviceKey;
                DataItemKey = dataSetObservation.DataItemKey;
                Entries = dataSetObservation.Entries;
                Timestamp = dataSetObservation.Timestamp;
            }
        }


        /// <summary>
        /// Convert ShdrDataSet to an SHDR string
        /// </summary>
        /// <returns>SHDR string</returns>
        public override string ToString()
        {
            if (!string.IsNullOrEmpty(DataItemKey))
            {
                var resetTriggered = ResetTriggered != ResetTriggered.NOT_SPECIFIED ? $":{ResetTriggered} " : "";

                if (Timestamp > 0)
                {
                    if (!IsUnavailable)
                    {
                        return $"{Timestamp.ToDateTime().ToString("o")}|{DataItemKey}|{resetTriggered}{PrintEntries(Entries)}";
                    }
                    else
                    {
                        return $"{Timestamp.ToDateTime().ToString("o")}|{DataItemKey}|{Observation.Unavailable}";
                    }
                }
                else
                {
                    if (!IsUnavailable)
                    {
                        return $"{DataItemKey}|{resetTriggered}{PrintEntries(Entries)}";
                    }
                    else
                    {
                        return $"{DataItemKey}|{Observation.Unavailable}";
                    }
                }
            }

            return null;
        }

        private static string PrintEntries(IEnumerable<IDataSetEntry> entries)
        {
            if (!entries.IsNullOrEmpty())
            {
                var pairs = new List<string>();

                foreach (var entry in entries)
                {
                    pairs.Add(new ShdrDataSetEntry(entry).ToString());
                }

                return string.Join(" ", pairs);
            }

            return "";
        }

        /// <summary>
        /// Read a ShdrDataSet object from an SHDR line
        /// </summary>
        /// <param name="input">SHDR Input String</param>
        public static ShdrDataSet FromString(string input)
        {
            if (!string.IsNullOrEmpty(input))
            {
                // Start reading input and read Timestamp first (if specified)
                var x = ShdrLine.GetNextValue(input);
                var timestamp = ShdrLine.GetTimestamp(x);
                var duration = ShdrLine.GetDuration(x);

                if (timestamp.HasValue)
                {
                    var y = ShdrLine.GetNextSegment(input);
                    return FromLine(y, timestamp.Value.ToUnixTime(), duration.HasValue ? duration.Value : 0);
                }
                else
                {
                    return FromLine(input);
                }
            }

            return null;
        }

        private static ShdrDataSet FromLine(string input, long timestamp = 0, double duration = 0)
        {
            if (!string.IsNullOrEmpty(input))
            {
                try
                {
                    var dataSet = new ShdrDataSet();
                    dataSet.Timestamp = timestamp;

                    // Set DataItemKey
                    var x = ShdrLine.GetNextValue(input);
                    var y = ShdrLine.GetNextSegment(input);
                    dataSet.DataItemKey = x;

                    if (y != null)
                    {
                        x = ShdrLine.GetNextValue(y);
                        if (!string.IsNullOrEmpty(x))
                        {
                            dataSet.ResetTriggered = ResetTriggered.NOT_SPECIFIED;
                            var entriesString = x;

                            // Parse the ResetTriggered (if exists)
                            var resetMatch = _resetTriggeredRegex.Match(x);
                            if (resetMatch.Success && resetMatch.Groups.Count > 2)
                            {
                                dataSet.ResetTriggered = resetMatch.Groups[1].Value.ConvertEnum<ResetTriggered>();
                                entriesString = resetMatch.Groups[2].Value;
                            }

                            var dataSetEntries = new List<ShdrDataSetEntry>();

                            // Get a List of Entries representing DataSetEntry objects
                            var entries = ShdrLine.GetEntries(entriesString);
                            if (!entries.IsNullOrEmpty())
                            {
                                foreach (var entry in entries)
                                {
                                    // Create new ShdrDataSetEntry and add to return list
                                    var dataSetEntry = new ShdrDataSetEntry(entry.Key, entry.Value, entry.Removed);
                                    dataSetEntries.Add(dataSetEntry);
                                }
                            }

                            dataSet.Entries = dataSetEntries;
                        }
                    }

                    return dataSet;
                }
                catch { }
            }

            return null;
        }
    }
}
