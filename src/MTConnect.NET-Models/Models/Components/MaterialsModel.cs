// Copyright (c) 2022 TrakHound Inc., All Rights Reserved.

// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using MTConnect.Devices.Components;

namespace MTConnect.Models.Components
{
    /// <summary>
    /// Materials provides information about materials or other items consumed or used by the piece of equipment
    /// for production of parts, materials, or other types of goods.Materials also represents parts or part stock
    /// that are present at a piece of equipment or location to which work is applied to transform the part or stock material into a more finished state.
    /// </summary>
    public class MaterialsModel : ResourceModel
    {
        public MaterialsModel()
        {
            Type = MaterialsComponent.TypeId;
        }

        public MaterialsModel(string componentId)
        {
            Id = componentId;
            Type = MaterialsComponent.TypeId;
        }
    }
}
