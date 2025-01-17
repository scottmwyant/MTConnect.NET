// Copyright (c) 2022 TrakHound Inc., All Rights Reserved.

// This file is subject to the terms and conditions defined in
// file 'LICENSE', which is part of this source code package.

using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace MTConnect.Errors
{
    /// <summary>
    /// When an Agent encounters an error when responding to a Request for information from a client software application,
    /// the information describing the error(s) is reported as a Data Entity in an MTConnectErrors Response Document. 
    /// Data Entities are organized in the Errors XML container.
    /// </summary>
    public class Error : IError
    {
        /// <summary>
        /// Provides a descriptive code that indicates the type of error that was encountered
        /// by an Agent when attempting to respond to a Request for information.
        /// </summary>
        [XmlAttribute("errorCode")]
        [JsonPropertyName("errorCode")]
        public ErrorCode ErrorCode { get; set; }

        /// <summary>
        /// The Value for Error contains a textual description of the error and any additional
        /// information an Agent is capable of providing regarding a specific error. The Valid Data Value returned for Error MAY be any text string.
        /// </summary>
        [XmlText]
        [JsonPropertyName("value")]
        public string Value { get; set; }


        public Error() { }

        public Error(ErrorCode errorCode, string value = null)
        {
            ErrorCode = errorCode;
            Value = value;
        }
    }
}
