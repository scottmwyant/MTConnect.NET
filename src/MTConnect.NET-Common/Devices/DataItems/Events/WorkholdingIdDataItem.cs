// Copyright (c) 2022 TrakHound Inc., All Rights Reserved.

// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

namespace MTConnect.Devices.DataItems.Events
{
    /// <summary>
    /// The identifier for the current workholding or part clamp in use by a piece of equipment.
    /// </summary>
    public class WorkHoldingIdDataItem : DataItem
    {
        public const DataItemCategory CategoryId = DataItemCategory.EVENT;
        public const string TypeId = "WORKHOLDING_ID";
        public const string NameId = "workholdingId";
        public new const string DescriptionText = "The identifier for the current workholding or part clamp in use by a piece of equipment.";

        public override string TypeDescription => DescriptionText;

        public override System.Version MinimumVersion => MTConnectVersions.Version11;


        public WorkHoldingIdDataItem()
        {
            Category = CategoryId;
            Type = TypeId;
        }

        public WorkHoldingIdDataItem(string parentId)
        {
            Id = CreateId(parentId, NameId);
            Category = CategoryId;
            Type = TypeId;
            Name = NameId;
        }
    }
}
