// Copyright (c) 2022 TrakHound Inc., All Rights Reserved.

// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using MTConnect.Devices.Compositions;
using MTConnect.Devices.DataItems.Events;
using MTConnect.Models.Assets;
using MTConnect.Models.DataItems;

namespace MTConnect.Models.Compositions
{
    /// <summary>
    /// A POT for a tool awaiting transfer from a ToolMagazine to Spindle or Turret.
    /// </summary>
    public class TransferPotModel : CompositionModel, ITransferPotModel
    {
        public CuttingToolModel CuttingTool
        {
            get => GetCuttingTool();
            set => SetCuttingTool(value);
        }


        public TransferPotModel() 
        {
            Type = TransferPotComposition.TypeId;
        }

        public TransferPotModel(string compositionId)
        {
            Id = compositionId;
            Type = TransferPotComposition.TypeId;
        }


        private CuttingToolModel GetCuttingTool()
        {
            var x = new CuttingToolModel();

            x.Number = GetDataItemValue(ToolNumberDataItem.NameId);
            x.NumberDataItem = GetDataItem(ToolNumberDataItem.NameId);

            x.Group = GetDataItemValue(ToolGroupDataItem.NameId);
            x.GroupDataItem = GetDataItem(ToolGroupDataItem.NameId);

            var offsetLength = GetDataItem(ToolOffsetDataItem.NameId, ToolOffsetDataItem.GetSubTypeId(ToolOffsetDataItem.SubTypes.LENGTH));
            var offsetRadial = GetDataItem(ToolOffsetDataItem.NameId, ToolOffsetDataItem.GetSubTypeId(ToolOffsetDataItem.SubTypes.RADIAL));

            if (offsetLength != null || offsetRadial != null)
            {
                x.Offset = new ToolOffsetModel
                {
                    Length = offsetLength != null ? GetDataItemValue(ToolOffsetDataItem.NameId, ToolOffsetDataItem.GetSubTypeId(ToolOffsetDataItem.SubTypes.LENGTH)) : null,
                    LengthDataItem = offsetLength,
                    Radial = offsetRadial != null ? GetDataItemValue(ToolOffsetDataItem.NameId, ToolOffsetDataItem.GetSubTypeId(ToolOffsetDataItem.SubTypes.RADIAL)) : null,
                    RadialDataItem = offsetRadial
                };
            }

            x.AssetId = GetDataItemValue(ToolAssetIdDataItem.NameId);
            return x;
        }

        private void SetCuttingTool(CuttingToolModel tool)
        {
            if (tool != null)
            {
                AddDataItem(new ToolNumberDataItem(Id), tool.Number);
                AddDataItem(new ToolGroupDataItem(Id), tool.Group);
                AddDataItem(new ToolOffsetDataItem(Id, ToolOffsetDataItem.SubTypes.LENGTH), tool.Offset?.Length);
                AddDataItem(new ToolOffsetDataItem(Id, ToolOffsetDataItem.SubTypes.RADIAL), tool.Offset?.Radial);
                AddDataItem(new ToolAssetIdDataItem(Id), tool.AssetId);
            }
        }
    }
}
