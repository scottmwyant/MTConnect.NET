// Copyright (c) 2022 TrakHound Inc., All Rights Reserved.

// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;

namespace MTConnect.Devices.DataItems.Events
{
    /// <summary>
    /// The aggregate count of parts.
    /// </summary>
    public class PartCountDataItem : DataItem
    {
        public const DataItemCategory CategoryId = DataItemCategory.EVENT;
        public const string TypeId = "PART_COUNT";
        public const string NameId = "partCount";
        public new const string DescriptionText = "The aggregate count of parts.";

        public override string TypeDescription => DescriptionText;

        public enum SubTypes
        {
            /// <summary>
            /// An accumulation representing all actions, items, or activities being counted independent of the outcome.ALL is the default subType.
            /// </summary>
            ALL,

            /// <summary>
            /// An accumulation representing actions, items, or activities being counted that do not conform to specification or expectation.
            /// </summary>
            BAD,

            /// <summary>
            /// An accumulation representing actions, items, or activities being counted that conform to specification or expectation.
            /// </summary>
            GOOD,

            /// <summary>
            /// The goal of the operation or process.
            /// </summary>
            TARGET,

            /// <summary>
            /// An accumulation representing actions, items, or activities yet to be counted.
            /// </summary>
            REMAINING,

            /// <summary>
            /// An accumulation representing actions, items, or activities that have been completed, independent of the outcome.
            /// </summary>
            COMPLETE,

            /// <summary>
            /// An accumulation representing actions or activities that were attempted, but failed to complete or resulted in an unexpected or unacceptable outcome.
            /// </summary>
            FAILED,

            /// <summary>
            /// An accumulation representing actions or activities that were attempted, but terminated before they could be completed.
            /// </summary>
            ABORTED
        }


        public PartCountDataItem()
        {
            Category = CategoryId;
            Type = TypeId;
        }

        public PartCountDataItem(
            string parentId,
            SubTypes subType
            )
        {
            Id = CreateId(parentId, NameId, GetSubTypeId(subType));
            Category = CategoryId;
            Type = TypeId;
            SubType = subType.ToString();
            Name = NameId;
            Units = Devices.Units.COUNT;
        }


        protected override IDataItem OnProcess(IDataItem dataItem, Version mtconnectVersion)
        {
            if (SubType == SubTypes.ABORTED.ToString() && mtconnectVersion < MTConnectVersions.Version18) return null;
            if (SubType == SubTypes.COMPLETE.ToString() && mtconnectVersion < MTConnectVersions.Version18) return null;
            if (SubType == SubTypes.FAILED.ToString() && mtconnectVersion < MTConnectVersions.Version18) return null;
            if (SubType == SubTypes.REMAINING.ToString() && mtconnectVersion < MTConnectVersions.Version13) return null;
            if (SubType == SubTypes.TARGET.ToString() && mtconnectVersion < MTConnectVersions.Version13) return null;

            return dataItem;
        }

        public override string SubTypeDescription => GetSubTypeDescription(SubType);

        public static string GetSubTypeDescription(string subType)
        {
            var s = subType.ConvertEnum<SubTypes>();
            switch (s)
            {
                case SubTypes.ALL: return "An accumulation representing all actions, items, or activities being counted independent of the outcome. ALL is the default subType.";
                case SubTypes.BAD: return "An accumulation representing actions, items, or activities being counted that do not conform to specification or expectation.";
                case SubTypes.GOOD: return "An accumulation representing actions, items, or activities being counted that conform to specification or expectation.";
                case SubTypes.TARGET: return "The goal of the operation or process.";
                case SubTypes.REMAINING: return "An accumulation representing actions, items, or activities yet to be counted.";
                case SubTypes.COMPLETE: return "An accumulation representing actions, items, or activities that have been completed, independent of the outcome.";
                case SubTypes.FAILED: return "An accumulation representing actions or activities that were attempted, but failed to complete or resulted in an unexpected or unacceptable outcome.";
                case SubTypes.ABORTED: return "An accumulation representing actions or activities that were attempted, but terminated before they could be completed.";
            }

            return null;
        }

        public static string GetSubTypeId(SubTypes subType)
        {
            switch (subType)
            {
                case SubTypes.ALL: return "all";
                case SubTypes.BAD: return "bad";
                case SubTypes.GOOD: return "good";
                case SubTypes.TARGET: return "target";
                case SubTypes.REMAINING: return "remaining";
                case SubTypes.COMPLETE: return "complete";
                case SubTypes.FAILED: return "failed";
                case SubTypes.ABORTED: return "aborted";
            }

            return null;
        }
    }
}
