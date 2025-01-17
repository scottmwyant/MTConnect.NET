// Copyright (c) 2022 TrakHound Inc., All Rights Reserved.

// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

namespace MTConnect.Devices.DataItems.Events
{
    /// <summary>
    /// An optional identifier for a BLOCK of code in a PROGRAM.
    /// </summary>
    public class LineLabelDataItem : DataItem
    {
        public const DataItemCategory CategoryId = DataItemCategory.EVENT;
        public const string TypeId = "LINE_LABEL";
        public const string NameId = "lineLabel";
        public new const string DescriptionText = "An optional identifier for a BLOCK of code in a PROGRAM.";

        public override string TypeDescription => DescriptionText;

        public override System.Version MinimumVersion => MTConnectVersions.Version14;


        public LineLabelDataItem()
        {
            Category = CategoryId;
            Type = TypeId;
        }

        public LineLabelDataItem(string parentId)
        {
            Id = CreateId(parentId, NameId);
            Category = CategoryId;
            Type = TypeId;
            Name = NameId;
        }
    }
}
