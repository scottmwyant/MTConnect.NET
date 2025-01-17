// Copyright (c) 2022 TrakHound Inc., All Rights Reserved.

// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using MTConnect.Devices.Components;

namespace MTConnect.Models.Components
{
    /// <summary>
    /// GangToolBar is a ToolingDelivery that represents a tool mounting mechanism that holds any number of tools.
    /// Tools are located in STATIONs. Tools are positioned for use in the manufacturing process by linearly positioning the GangToolBar.
    /// </summary>
    public class GangToolBarModel : ToolingDeliveryModel
    {
        public GangToolBarModel()
        {
            Type = GangToolbarComponent.TypeId;
        }

        public GangToolBarModel(string componentId)
        {
            Id = componentId;
            Type = GangToolbarComponent.TypeId;
        }
    }
}
