// Copyright (c) 2022 TrakHound Inc., All Rights Reserved.

// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

namespace MTConnect.Devices.DataItems.Conditions
{
    /// <summary>
    /// An indication that the end of a piece of bar stock has been reached.
    /// </summary>
    public class EndOfBarCondition : DataItem
    {
        public const DataItemCategory CategoryId = DataItemCategory.CONDITION;
        public const string TypeId = "END_OF_BAR";
        public const string NameId = "eobCond";
        public new const string DescriptionText = "An indication that the end of a piece of bar stock has been reached.";

        public override string TypeDescription => DescriptionText;

        public override System.Version MinimumVersion => MTConnectVersions.Version13;


        public EndOfBarCondition()
        {
            Category = CategoryId;
            Type = TypeId;
        }

        public EndOfBarCondition(string parentId)
        {
            Id = CreateId(parentId, NameId);
            Category = CategoryId;
            Type = TypeId;
            Name = NameId;
        }
    }
}
