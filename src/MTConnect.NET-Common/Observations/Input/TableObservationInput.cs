// Copyright (c) 2022 TrakHound Inc., All Rights Reserved.

// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;
using System.Collections.Generic;

namespace MTConnect.Observations.Input
{
    /// <summary>
    /// An Information Model that describes Streaming Data reported by a piece of equipment
    /// where the reported value(s) are represented as rows containing sets of key-value pairs given by Cell elements.
    /// </summary>
    public class TableObservationInput : ObservationInput
    {
        /// <summary>
        /// Key-value pairs published as part of a Data Set observation
        /// </summary>
        public IEnumerable<ITableEntry> Entries
        {
            get => TableObservation.GetEntries(Values);
            set
            {
                if (!value.IsNullOrEmpty())
                {
                    foreach (var entry in value)
                    {
                        if (entry.Removed)
                        {
                            AddValue(new ObservationValue(ValueKeys.CreateTableValueKey(entry.Key), TableObservation.EntryRemovedValue));
                        }
                        else
                        {
                            if (!entry.Cells.IsNullOrEmpty())
                            {
                                foreach (var cell in entry.Cells)
                                {
                                    AddValue(new ObservationValue(ValueKeys.CreateTableValueKey(entry.Key, cell.Key), cell.Value));
                                }
                            }
                        }
                    }
                }
            }
        }


        public TableObservationInput() { }

        public TableObservationInput(string dataItemKey, IEnumerable<ITableEntry> entries)
        {
            DataItemKey = dataItemKey;
            Entries = entries;
        }

        public TableObservationInput(string dataItemKey, IEnumerable<ITableEntry> entries, long timestamp)
        {
            DataItemKey = dataItemKey;
            Entries = entries;
            Timestamp = timestamp;
        }

        public TableObservationInput(string dataItemKey, IEnumerable<ITableEntry> entries, DateTime timestamp)
        {
            DataItemKey = dataItemKey;
            Entries = entries;
            Timestamp = timestamp.ToUnixTime();
        }
    }
}
