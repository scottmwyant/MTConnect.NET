// Copyright (c) 2022 TrakHound Inc., All Rights Reserved.

// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using MTConnect.Devices.Components;

namespace MTConnect.Models.Components
{
    /// <summary>
    /// ToolRack is a ToolingDelivery that represents a linear or matrixed tool storage mechanism that holds any number of tools. Tools are located in STATIONs.
    /// </summary>
    public class ToolRackModel : ToolingDeliveryModel
    {
        public ToolRackModel()
        {
            Type = ToolRackComponent.TypeId;
        }

        public ToolRackModel(string componentId)
        {
            Id = componentId;
            Type = ToolRackComponent.TypeId;
        }
    }
}
