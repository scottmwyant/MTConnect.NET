// Copyright (c) 2022 TrakHound Inc., All Rights Reserved.

// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using MTConnect.Devices.Components;

namespace MTConnect.Models.Components
{
    /// <summary>
    /// Feeder is a System that represents the information for a system that manages the delivery of materials within a piece of equipment. 
    /// For example, this could describe the wire delivery system for an EDM or welding process; 
    /// conveying system or pump and valve system distributing material to a blending station; or a fuel delivery system feeding a furnace.
    /// </summary>
    public class FeederModel : SystemModel, IFeederModel
    {
        public FeederModel()
        {
            Type = FeederComponent.TypeId;
        }

        public FeederModel(string componentId)
        {
            Id = componentId;
            Type = FeederComponent.TypeId;
        }
    }
}
