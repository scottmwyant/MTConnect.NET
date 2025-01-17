﻿// Copyright (c) 2022 TrakHound Inc., All Rights Reserved.

// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System.Collections.Generic;
using System.Linq;

namespace MTConnect.Observations
{
    /// <summary>
    /// A Table represents two-dimensional sets of key-value pairs where the Entry represents rows containing sets of key-value pairs given by Cell elements.
    /// </summary>
    internal static class TableObservation
    {
        public const string EntryRemovedValue = "[!ENTRY_REMOVED!]";


        public static IEnumerable<ITableEntry> GetEntries(IEnumerable<ObservationValue> values)
        {
            var entries = new List<ITableEntry>();

            if (!values.IsNullOrEmpty())
            {
                var entryValues = values.Where(o => o.Key != null && o.Key.StartsWith(ValueKeys.TablePrefix));
                if (!entryValues.IsNullOrEmpty())
                {
                    var keys = entryValues.Select(o => ValueKeys.GetTableKey(o.Key)).Distinct();
                    if (!keys.IsNullOrEmpty())
                    {
                        foreach (var key in keys)
                        {
                            var keyValues = entryValues.Where(o => ValueKeys.GetTableKey(o.Key) == key);
                            if (!keyValues.IsNullOrEmpty())
                            {
                                var removed = keyValues.Any(o => o.Value == EntryRemovedValue);
                                if (removed)
                                {
                                    entries.Add(new TableEntry(key, true));
                                }
                                else
                                {
                                    var cells = new List<TableCell>();
                                    foreach (var keyValue in keyValues)
                                    {
                                        cells.Add(new TableCell(ValueKeys.GetTableValue(keyValue.Key, key), keyValue.Value));
                                    }

                                    entries.Add(new TableEntry(key, cells));
                                }
                            }
                        }
                    }
                }
            }

            return entries;
        }
    }
}
