// Copyright (c) 2022 TrakHound Inc., All Rights Reserved.

// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

namespace MTConnect.Devices.DataItems.Samples
{
    /// <summary>
    /// The measurement of the feedrate for the axes, or a single axis, associated with a Path component-a vector.
    /// </summary>
    public class PathFeedratePerRevolutionDataItem : DataItem
    {
        public const DataItemCategory CategoryId = DataItemCategory.SAMPLE;
        public const string TypeId = "PATH_FEEDRATE_PER_REVOLUTION";
        public const string NameId = "pathFeedPerRev";
        public const string DefaultUnits = Devices.Units.MILLIMETER_PER_REVOLUTION;
        public new const string DescriptionText = "The measurement of the feedrate for the axes, or a single axis, associated with a Path component-a vector.";

        public override string TypeDescription => DescriptionText;

        public override System.Version MinimumVersion => MTConnectVersions.Version15;

        public enum SubTypes
        {
            /// <summary>
            /// The measured or reported value of an observation.
            /// </summary>
            ACTUAL,

            /// <summary>
            /// Directive value including adjustments such as an offset or overrides.
            /// </summary>
            COMMANDED,

            /// <summary>
            /// Directive value without offsets and adjustments.
            /// </summary>
            PROGRAMMED
        }


        public PathFeedratePerRevolutionDataItem()
        {
            Category = CategoryId;
            Type = TypeId;
            Units = DefaultUnits;
        }

        public PathFeedratePerRevolutionDataItem(
            string parentId,
            SubTypes subType)
        {
            Id = CreateId(parentId, NameId, GetSubTypeId(subType));
            Category = CategoryId;
            Type = TypeId;
            SubType = subType.ToString();
            Name = NameId;
            Units = DefaultUnits;
        }

        public override string SubTypeDescription => GetSubTypeDescription(SubType);

        public static string GetSubTypeDescription(string subType)
        {
            var s = subType.ConvertEnum<SubTypes>();
            switch (s)
            {
                case SubTypes.ACTUAL: return "The measured or reported value of an observation.";
                case SubTypes.COMMANDED: return "Directive value including adjustments such as an offset or overrides.";
                case SubTypes.PROGRAMMED: return "Directive value without offsets and adjustments.";
            }

            return null;
        }

        public static string GetSubTypeId(SubTypes subType)
        {
            switch (subType)
            { 
                case SubTypes.ACTUAL: return "act";
                case SubTypes.COMMANDED: return "cmd";
                case SubTypes.PROGRAMMED: return "prg";
            }

            return null;
        }
    }
}
