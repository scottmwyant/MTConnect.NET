﻿// Copyright (c) 2022 TrakHound Inc., All Rights Reserved.

// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using MTConnect.Observations;
using System;
using System.Collections.Generic;

namespace MTConnect.Adapters.Shdr
{
    /// <summary>
    /// An Event Observation with a Type of MESSAGE
    /// </summary>
    public class ShdrMessage : ShdrDataItem
    {
        public string NativeCode
        {
            get => GetValue(ValueKeys.NativeCode);
            set => AddValue(new ObservationValue(ValueKeys.NativeCode, value));
        }


        public ShdrMessage() { }

        public ShdrMessage(string dataItemKey)
        {
            DataItemKey = dataItemKey;
            Timestamp = 0;
        }

        public ShdrMessage(string dataItemKey, object value)
        {
            DataItemKey = dataItemKey;
            Values = new List<ObservationValue>
            {
                new ObservationValue(ValueKeys.CDATA, value != null ? value.ToString() : string.Empty)
            };
            Timestamp = 0;
        }

        public ShdrMessage(string dataItemKey, object value, string nativeCode)
        {
            DataItemKey = dataItemKey;
            var values = new List<ObservationValue>();
            values.Add(new ObservationValue(ValueKeys.CDATA, value != null ? value.ToString() : string.Empty));
            if (!string.IsNullOrEmpty(nativeCode)) values.Add(new ObservationValue(ValueKeys.NativeCode, nativeCode));
            Values = values;
            Timestamp = 0;
        }

        public ShdrMessage(string dataItemKey, object value, long timestamp)
        {
            DataItemKey = dataItemKey;
            Values = new List<ObservationValue>
            {
                new ObservationValue(ValueKeys.CDATA, value != null ? value.ToString() : string.Empty)
            };
            Timestamp = timestamp;
        }

        public ShdrMessage(string dataItemKey, object value, string nativeCode, long timestamp)
        {
            DataItemKey = dataItemKey;
            var values = new List<ObservationValue>();
            values.Add(new ObservationValue(ValueKeys.CDATA, value != null ? value.ToString() : string.Empty));
            if (!string.IsNullOrEmpty(nativeCode)) values.Add(new ObservationValue(ValueKeys.NativeCode, nativeCode));
            Values = values;
            Timestamp = timestamp;
        }

        public ShdrMessage(string dataItemKey, object value, DateTime timestamp)
        {
            DataItemKey = dataItemKey;
            Values = new List<ObservationValue>
            {
                new ObservationValue(ValueKeys.CDATA, value != null ? value.ToString() : string.Empty)
            };
            Timestamp = timestamp.ToUnixTime();
        }

        public ShdrMessage(string dataItemKey, object value, string nativeCode, DateTime timestamp)
        {
            DataItemKey = dataItemKey;
            var values = new List<ObservationValue>();
            values.Add(new ObservationValue(ValueKeys.CDATA, value != null ? value.ToString() : string.Empty));
            if (!string.IsNullOrEmpty(nativeCode)) values.Add(new ObservationValue(ValueKeys.NativeCode, nativeCode));
            Values = values;
            Timestamp = timestamp.ToUnixTime();
        }


        /// <summary>
        /// Convert ShdrMessage to an SHDR string
        /// </summary>
        /// <returns>SHDR string</returns>
        public override string ToString()
        {
            if (!string.IsNullOrEmpty(DataItemKey))
            {
                var valueString = GetValue(ValueKeys.CDATA);
                var nativeCodeString = GetValue(ValueKeys.NativeCode);

                if (valueString != null || nativeCodeString != null)
                {
                    var value = valueString != null ? valueString.Replace("|", @"\|") : "";
                    var nativeCode = nativeCodeString != null ? nativeCodeString.Replace("|", @"\|") : "";
                    var resetTriggered = ResetTriggered != ResetTriggered.NOT_SPECIFIED ? $":{ResetTriggered} " : "";

                    if (Timestamp > 0 && Duration > 0)
                    {
                        return $"{Timestamp.ToDateTime().ToString("o")}@{Duration}|{DataItemKey}|{nativeCode}|{resetTriggered}{value}";
                    }
                    else if (Timestamp > 0)
                    {
                        return $"{Timestamp.ToDateTime().ToString("o")}|{DataItemKey}|{nativeCode}|{resetTriggered}{value}";
                    }
                    else
                    {
                        return $"{DataItemKey}|{resetTriggered}{value}";
                    }
                }
            }

            return null;
        }

        private static string ToString(ShdrDataItem dataItem, bool ignoreTimestamp = false)
        {
            if (dataItem != null && !string.IsNullOrEmpty(dataItem.DataItemKey))
            {
                var valueString = dataItem.GetValue(ValueKeys.CDATA);
                var nativeCodeString = dataItem.GetValue(ValueKeys.NativeCode);

                if (valueString != null || nativeCodeString != null)
                {
                    var value = valueString != null ? valueString.Replace("|", @"\|") : "";
                    var nativeCode = nativeCodeString != null ? nativeCodeString.Replace("|", @"\|") : "";
                    var resetTriggered = dataItem.ResetTriggered != ResetTriggered.NOT_SPECIFIED ? $":{dataItem.ResetTriggered} " : "";

                    if (dataItem.Timestamp > 0 && dataItem.Duration > 0)
                    {
                        return $"{dataItem.Timestamp.ToDateTime().ToString("o")}@{dataItem.Duration}|{dataItem.DataItemKey}|{nativeCode}|{resetTriggered}{value}";
                    }
                    else if (dataItem.Timestamp > 0 && !ignoreTimestamp)
                    {
                        return $"{GetTimestampString(dataItem.Timestamp, dataItem.Duration)}|{dataItem.DataItemKey}|{nativeCode}|{resetTriggered}{value}";
                    }
                    else
                    {
                        return $"{dataItem.DataItemKey}|{nativeCode}|{resetTriggered}{value}";
                    }
                }
            }

            return "";
        }

        /// <summary>
        /// Read a ShdrCondition object from an SHDR line
        /// </summary>
        /// <param name="input">SHDR Input String</param>
        public static ShdrMessage FromString(string input)
        {
            if (!string.IsNullOrEmpty(input))
            {
                // Expected format : <timestamp>|<data_item_name>|<native_code>|<message>
                // Expected format : 2014-09-29T23:59:33.460470Z|htemp|WARNING|HTEMP|1|HIGH|Oil Temperature High

                // Start reading input and read Timestamp first (if specified)
                var x = ShdrLine.GetNextValue(input);

                if (DateTime.TryParse(x, null, System.Globalization.DateTimeStyles.AdjustToUniversal, out var timestamp))
                {
                    var y = ShdrLine.GetNextSegment(input);
                    return FromLine(y, timestamp.ToUnixTime());
                }
                else
                {
                    return FromLine(input);
                }
            }

            return null;
        }

        private static ShdrMessage FromLine(string input, long timestamp = 0)
        {
            if (!string.IsNullOrEmpty(input))
            {
                try
                {
                    var message = new ShdrMessage();
                    message.Timestamp = timestamp;

                    // Set DataItemKey
                    var x = ShdrLine.GetNextValue(input);
                    var y = ShdrLine.GetNextSegment(input);
                    message.DataItemKey = x;

                    if (y != null)
                    {
                        // Set NativeCode
                        x = ShdrLine.GetNextValue(y);
                        y = ShdrLine.GetNextSegment(y);
                        message.NativeCode = x;

                        // Set Text
                        x = ShdrLine.GetNextValue(y);
                        message.CDATA = x;
                    }

                    return message;
                }
                catch { }
            }

            return null;
        }

        private static string GetTimestampString(long timestamp, double duration = 0)
        {
            if (duration > 0)
            {
                return $"{timestamp.ToDateTime().ToString("o")}@{duration}";
            }
            else
            {
                return timestamp.ToDateTime().ToString("o");
            }
        }

        private static string GetTimestampString(DateTime timestamp, double duration = 0)
        {
            if (duration > 0)
            {
                return $"{timestamp.ToString("o")}@{duration}";
            }
            else
            {
                return timestamp.ToString("o");
            }
        }
    }
}
