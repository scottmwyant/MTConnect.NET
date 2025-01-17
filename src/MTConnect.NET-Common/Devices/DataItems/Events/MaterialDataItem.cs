// Copyright (c) 2022 TrakHound Inc., All Rights Reserved.

// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

namespace MTConnect.Devices.DataItems.Events
{
    /// <summary>
    /// The identifier of a material used or consumed in the manufacturing process.
    /// </summary>
    public class MaterialDataItem : DataItem
    {
        public const DataItemCategory CategoryId = DataItemCategory.EVENT;
        public const string TypeId = "MATERIAL";
        public const string NameId = "material";
        public new const string DescriptionText = "The identifier of a material used or consumed in the manufacturing process.";

        public override string TypeDescription => DescriptionText;

        public override System.Version MinimumVersion => MTConnectVersions.Version15;


        public MaterialDataItem()
        {
            Category = CategoryId;
            Type = TypeId;
        }

        public MaterialDataItem(string parentId)
        {
            Id = CreateId(parentId, NameId);
            Category = CategoryId;
            Type = TypeId;
            Name = NameId;
        }
    }
}
