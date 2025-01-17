﻿// Copyright (c) 2022 TrakHound Inc., All Rights Reserved.

// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System.Collections.Generic;

namespace MTConnect.Http
{
    public struct MTConnectHttpMessage
    {
        public IEnumerable<KeyValuePair<string, string>> Headers { get; set; }

        public string Content { get; set; }

        public string ContentType { get; set; }

        public int StatusCode { get; set; }

        public long ResponseDuration { get; set; }
    }
}
