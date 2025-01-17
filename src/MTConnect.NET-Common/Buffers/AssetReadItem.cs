﻿// Copyright (c) 2022 TrakHound Inc., All Rights Reserved.

// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using MTConnect.Assets;

namespace MTConnect.Buffers
{
    struct AssetReadItem
    {
        public int Index { get; set; }

        public IAsset Asset { get; set; }


        public AssetReadItem(int index, IAsset asset)
        {
            Index = index;
            Asset = asset;
        }
    }
}
