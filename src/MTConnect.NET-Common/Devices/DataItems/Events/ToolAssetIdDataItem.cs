// Copyright (c) 2022 TrakHound Inc., All Rights Reserved.

// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

namespace MTConnect.Devices.DataItems.Events
{
    /// <summary>
    /// The identifier of an individual tool asset.
    /// </summary>
    public class ToolAssetIdDataItem : DataItem
    {
        public const DataItemCategory CategoryId = DataItemCategory.EVENT;
        public const string TypeId = "TOOL_ASSET_ID";
        public const string NameId = "toolAssetId";
        public new const string DescriptionText = "The identifier of an individual tool asset.";

        public override string TypeDescription => DescriptionText;

        public override System.Version MinimumVersion => MTConnectVersions.Version12;


        public ToolAssetIdDataItem()
        {
            Category = CategoryId;
            Type = TypeId;
        }

        public ToolAssetIdDataItem(string parentId)
        {
            Id = CreateId(parentId, NameId);
            Category = CategoryId;
            Type = TypeId;
            Name = NameId;
        }
    }
}
