// Copyright (c) 2022 TrakHound Inc., All Rights Reserved.

// This file is subject to the terms and conditions defined in
// file 'LICENSE', which is part of this source code package.

namespace MTConnect.Assets.CuttingTools.Measurements
{
    public abstract class CommonMeasurement : Measurement
    {
        public CommonMeasurement() { }

        public CommonMeasurement(Measurement measurement) : base(measurement) { }
    }
}
