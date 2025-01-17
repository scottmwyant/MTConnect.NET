// Copyright (c) 2022 TrakHound Inc., All Rights Reserved.

// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

namespace MTConnect.Devices.DataItems.Events
{
    /// <summary>
    /// The event generated when an asset is added or changed. 
    /// AssetChanged MUST be discrete and the value of the DataItem’s discrete attribute MUST be true.
    /// </summary>
    public class AssetChangedDataItem : DataItem
    {
        public const DataItemCategory CategoryId = DataItemCategory.EVENT;
        public const string TypeId = "ASSET_CHANGED";
        public const string NameId = "assetChanged";
        public new const string DescriptionText = "The event generated when an asset is added or changed. AssetChanged MUST be discrete and the value of the DataItem’s discrete attribute MUST be true.";

        public override string TypeDescription => DescriptionText;

        public override System.Version MinimumVersion => MTConnectVersions.Version12;


        public AssetChangedDataItem()
        {
            Category = CategoryId;
            Type = TypeId;
        }

        public AssetChangedDataItem(string parentId)
        {
            Id = CreateId(parentId, NameId);
            Category = CategoryId;
            Type = TypeId;
            Name = NameId;
        }
    }
}
