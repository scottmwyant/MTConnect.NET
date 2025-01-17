// Copyright (c) 2022 TrakHound Inc., All Rights Reserved.

// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using MTConnect.Devices.Compositions;
using MTConnect.Devices.DataItems.Conditions;

namespace MTConnect.Models.Compositions
{
    /// <summary>
    /// A mechanism to measure position.
    /// </summary>
    public class EncoderModel : CompositionModel, IEncoderModel
    {
        /// <summary>
        /// An indication of a fault associated with a piece of equipment or component that cannot be classified as a specific type.
        /// </summary>
        public Observations.ConditionObservation SystemCondition
        {
            get => GetCondition(Devices.DataItems.Conditions.SystemCondition.NameId);
            set => AddCondition(new SystemCondition(Id), value);
        }

        /// <summary>
        /// An indication of a fault associated with the hardware subsystem of the Structural Element.
        /// </summary>
        public Observations.ConditionObservation HardwareCondition
        {
            get => GetCondition(Devices.DataItems.Conditions.HardwareCondition.NameId);
            set => AddCondition(new HardwareCondition(Id), value);
        }

        /// <summary>
        /// An indication that the piece of equipment has experienced a communications failure.
        /// </summary>
        public Observations.ConditionObservation CommunicationsCondition
        {
            get => GetCondition(Devices.DataItems.Conditions.CommunicationsCondition.NameId);
            set => AddCondition(new CommunicationsCondition(Id), value);
        }


        public EncoderModel() 
        {
            Type = EncoderComposition.TypeId;
        }

        public EncoderModel(string compositionId)
        {
            Id = compositionId;
            Type = EncoderComposition.TypeId;
        }
    }
}
