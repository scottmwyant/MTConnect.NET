// Copyright (c) 2022 TrakHound Inc., All Rights Reserved.

// This file is subject to the terms and conditions defined in
// file 'LICENSE', which is part of this source code package.

namespace MTConnect.Devices.References
{
    /// <summary>
    /// DataItemRef XML element is a pointer to a Data Entity associated with another Structural Element defined elsewhere in the XML document for a piece of equipment.
    /// DataItemRef allows the data associated with a data item defined in another Structural Element to be directly associated with this XML element.
    /// </summary>
    public class DataItemReference : Reference 
    {
        public new const string DescriptionText = "DataItemRef XML element is a pointer to a Data Entity associated with another Structural Element defined elsewhere in the XML document for a piece of equipment. DataItemRef allows the data associated with a data item defined in another Structural Element to be directly associated with this XML element.";

        public override string TypeDescription => DescriptionText;
    }
}
