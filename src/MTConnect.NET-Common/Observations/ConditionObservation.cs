﻿// Copyright (c) 2022 TrakHound Inc., All Rights Reserved.

// This file is subject to the terms and conditions defined in
// file 'LICENSE', which is part of this source code package.

using MTConnect.Devices;
using MTConnect.Observations.Input;
using System;
using System.Text.Json.Serialization;
using System.Xml;
using System.Xml.Serialization;

namespace MTConnect.Observations
{
    /// <summary>
    /// An abstract XML Element. Replaced in the XML document by type(s) of Condition type Data Elements representing CONDITION category data items defined for a Device in the Device Information Model.
    /// There can be multiple types of Condition type XML Elements in a Condition container.
    /// </summary>
    public class ConditionObservation : Observation, IConditionObservation
    {
        /// <summary>
        /// Type associated with the DataItem
        /// </summary>
        [XmlAttribute("type")]
        [JsonPropertyName("type")]
        public override string Type => base.Type;


        /// <summary>
        /// Level of the Condition (Normal, Warning, Fault, or Unavailable)
        /// </summary>
        [XmlIgnore]
        [JsonPropertyName("level")]
        public ConditionLevel Level
        {
            get => GetValue(ValueKeys.Level).ConvertEnum<ConditionLevel>();
            set => AddValue(ValueKeys.Level, value.ToString());
        }

        /// <summary>
        /// The native code (usually an alpha-numeric value) generated by the controller of a piece of equipment providing a reference identifier for a condition state or alarm.
        /// This is the same information an operator or maintenance personnel would see as a reference code designating a specific type of Condition when viewed at the piece of equipment.Usually this reference code is used to point to a more detailed description of the Condition.
        /// </summary>
        [XmlAttribute("nativeCode")]
        [JsonPropertyName("nativeCode")]
        public string NativeCode
        {
            get => GetValue(ValueKeys.NativeCode);
            set => AddValue(ValueKeys.NativeCode, value);
        }

        internal bool NativeCodeOutput => !string.IsNullOrEmpty(NativeCode);

        /// <summary>
        /// If the data source assigns a severity level to a Condition, nativeSeverity is used to report that severity information to a client software application.
        /// </summary>
        [XmlAttribute("nativeSeverity")]
        [JsonPropertyName("nativeSeverity")]
        public string NativeSeverity
        {
            get => GetValue(ValueKeys.NativeSeverity);
            set => AddValue(ValueKeys.NativeSeverity, value);
        }

        internal bool NativeSeverityOutput => !string.IsNullOrEmpty(NativeSeverity);

        /// <summary>
        /// Qualifies the Condition and adds context or additional clarification.
        /// This optional attribute can be used to convey information such as HIGH or LOW type Warning and Fault condition to indicate differing types of condition states
        /// </summary>
        [XmlAttribute("qualifier")]
        [JsonPropertyName("qualifier")]
        public ConditionQualifier Qualifier
        {
            get => GetValue(ValueKeys.Qualifier).ConvertEnum<ConditionQualifier>();
            set => AddValue(ValueKeys.Qualifier, value.ToString());
        }

        internal bool QualifierOutput => Qualifier != ConditionQualifier.NOT_SPECIFIED;

        /// <summary>
        /// The type of statistical calculation specified for the DataItem defined in the Device Information Model that this Condition element represents.
        /// </summary>
        [XmlAttribute("statistic")]
        [JsonPropertyName("statistic")]
        public DataItemStatistic Statistic
        {
            get => GetValue(ValueKeys.Statistic).ConvertEnum<DataItemStatistic>();
            set => AddValue(ValueKeys.Statistic, value.ToString());
        }

        internal bool StatisticOutput => Statistic != DataItemStatistic.NONE;

        /// <summary>
        /// Used to describe a value (text or data) published as part of an XML element.
        /// </summary>
        [XmlText]
        [JsonPropertyName("cdata")]
        public string CDATA
        {
            get => GetValue(ValueKeys.CDATA);
            set => AddValue(ValueKeys.CDATA, value);
        }

        internal bool CDATAOutput => false;

        public ConditionObservation()
        {
            SetProperty("Category", DataItemCategory.CONDITION);
        }


        /// <summary>
        /// Determine if the DataItem with the specified Observation is valid in the specified MTConnectVersion
        /// </summary>
        /// <param name="mtconnectVersion">The Version of the MTConnect Standard</param>
        /// <param name="observation">The Observation to validate</param>
        /// <returns>A DataItemValidationResult indicating if Validation was successful and a Message</returns>
        protected ObservationValidationResult Validate<T>(Version mtconnectVersion, IObservationInput observation) where T : struct
        {
            if (observation != null && !observation.Values.IsNullOrEmpty())
            {
                // Get the CDATA Value for the Observation
                var cdata = observation.GetValue(ValueKeys.CDATA);
                if (cdata != null)
                {
                    // Check Valid values in Enum
                    var validValues = Enum.GetValues(typeof(T));
                    foreach (var validValue in validValues)
                    {
                        if (cdata == validValue.ToString())
                        {
                            return new ObservationValidationResult(true);
                        }
                    }

                    return new ObservationValidationResult(false, "'" + cdata + "' is not a valid value");
                }
                else
                {
                    return new ObservationValidationResult(false, "No CDATA is specified for the Observation");
                }
            }

            return new ObservationValidationResult(false, "No Observation is Specified");
        }


        public static ConditionObservation Create(DataItem dataItem)
        {
            if (dataItem != null)
            {
                var observation = Create(dataItem.Type, dataItem.Representation);
                observation.SetProperty(nameof(DataItemId), dataItem.Id);
                observation.SetProperty(nameof(Representation), dataItem.Representation);
                observation.SetProperty(nameof(Type), dataItem.Type);
                observation.SetProperty(nameof(SubType), dataItem.SubType);
                observation.SetProperty(nameof(Name), dataItem.Name);
                observation.SetProperty(nameof(CompositionId), dataItem.CompositionId);
                return observation;
            }

            return null;
        }

        public static ConditionObservation Create(string type, DataItemRepresentation representation)
        {
            if (!string.IsNullOrEmpty(type))
            {
                if (_types == null) _types = GetAllTypes();

                if (!_types.IsNullOrEmpty())
                {
                    var key = $"{type.ToPascalCase()}{representation.ToString().ToPascalCase()}";

                    if (_types.TryGetValue(key, out Type t))
                    {
                        var constructor = t.GetConstructor(System.Type.EmptyTypes);
                        if (constructor != null)
                        {
                            try
                            {
                                switch (representation)
                                {
                                    case DataItemRepresentation.VALUE: return (ConditionObservation)Activator.CreateInstance(t);
                                }
                            }
                            catch { }
                        }
                    }
                }
            }

            switch (representation)
            {
                default: return new ConditionObservation();
            }
        }
    }
}
