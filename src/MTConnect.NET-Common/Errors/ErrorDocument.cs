// Copyright (c) 2022 TrakHound Inc., All Rights Reserved.

// This file is subject to the terms and conditions defined in
// file 'LICENSE', which is part of this source code package.

using MTConnect.Headers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Serialization;

namespace MTConnect.Errors
{
    /// <summary>
    /// The Error Information Model establishes the rules and terminology that describes the Response Document
    /// returned by an Agent when it encounters an error while interpreting a Request for information from a client
    /// software application or when an Agent experiences an error while publishing the Response to a Request for information.
    /// </summary>
    [XmlRoot("MTConnectError")]
    public class ErrorResponseDocument : IErrorResponseDocument
    {
        //private static readonly XmlSerializer _serializer = new XmlSerializer(typeof(ErrorResponseDocument));


        /// <summary>
        /// The Header element for an MTConnectErrors Response Document defines information regarding
        /// the creation of the document and the data storage capability of the Agent that generated the document.
        /// </summary>
        [XmlElement("Header")]
        [JsonPropertyName("header")]
        public IMTConnectErrorHeader Header { get; set; }

        /// <summary>
        /// An XML container element in an MTConnectErrors Response Document provided by an Agent when an error
        /// is encountered associated with a Request for information from a client software application.
        /// </summary>
        [XmlArray("Errors")]
        [JsonPropertyName("errors")]
        public IEnumerable<IError> Errors { get; set; }

        /// <summary>
        /// The MTConnect Version of the Response document
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        public Version Version { get; set; }


        //public static ErrorResponseDocument Create(string xml)
        //{
        //    try
        //    {
        //        xml = xml.Trim();

        //        var version = MTConnectVersion.Get(xml);

        //        using (var textReader = new StringReader(Namespaces.Clear(xml)))
        //        {
        //            using (var xmlReader = XmlReader.Create(textReader))
        //            {
        //                var doc = (ErrorResponseDocument)_serializer.Deserialize(xmlReader);
        //                if (doc != null)
        //                {
        //                    doc.Version = version;
        //                    return doc;
        //                }
        //            }
        //        }
        //    }
        //    catch { }

        //    return null;
        //}


        //public static ErrorResponseDocument FromXml(string xml)
        //{
        //    if (!string.IsNullOrEmpty(xml))
        //    {
        //        try
        //        {
        //            xml = xml.Trim();

        //            var version = MTConnectVersion.Get(xml);

        //            using (var textReader = new StringReader(Namespaces.Clear(xml)))
        //            {
        //                using (var xmlReader = XmlReader.Create(textReader))
        //                {
        //                    var doc = (ErrorResponseDocument)_serializer.Deserialize(xmlReader);
        //                    if (doc != null)
        //                    {
        //                        doc.Version = version;
        //                        return doc;
        //                    }
        //                }
        //            }
        //        }
        //        catch { }
        //    }

        //    return null;
        //}

        //public static ErrorResponseDocument FromJson(string json)
        //{
        //    return JsonFunctions.FromJson<ErrorResponseDocument>(json);
        //}


        //public string ToXml(bool indent = false)
        //{
        //    return ToXml(this, indent);
        //}

        //public static string ToXml(IErrorResponseDocument document, bool indent = false)
        //{
        //    if (document != null && document.Header != null)
        //    {
        //        try
        //        {
        //            var ns = Namespaces.GetError(document.Version.Major, document.Version.Minor);
        //            var schemaLocation = Schemas.GetError(document.Version.Major, document.Version.Minor);

        //            using (var writer = new StringWriter())
        //            {
        //                _serializer.Serialize(writer, document);

        //                var xml = writer.ToString();

        //                // Remove the XSD namespace
        //                string regex = @"\s{1}xmlns:xsi=\""http:\/\/www\.w3\.org\/2001\/XMLSchema-instance\""\s{1}xmlns:xsd=\""http:\/\/www\.w3\.org\/2001\/XMLSchema\""";
        //                xml = Regex.Replace(xml, regex, "");
        //                regex = @"\s{1}xmlns:xsd=\""http:\/\/www\.w3\.org\/2001\/XMLSchema\""\s{1}xmlns:xsi=\""http:\/\/www\.w3\.org\/2001\/XMLSchema-instance\""";
        //                xml = Regex.Replace(xml, regex, "");

        //                // Add the default namespace, "m" namespace, xsi, and schemaLocation
        //                regex = @"<MTConnectError";
        //                string replace = "<MTConnectError xmlns:m=\"" + ns + "\" xmlns=\"" + ns + "\" xmlns:xsi=\"" + Namespaces.DefaultXmlSchemaInstance + "\" xsi:schemaLocation=\"" + schemaLocation + "\"";
        //                xml = Regex.Replace(xml, regex, replace);

        //                if (indent) return XmlFunctions.IndentXml(xml);
        //                else return xml;
        //            }                      
        //        }
        //        catch { }
        //    }

        //    return null;
        //}

        //public string ToJson(bool indent = false)
        //{
        //    return ToJson(this, indent);
        //}

        //public static string ToJson(IErrorResponseDocument document, bool indent = false)
        //{
        //    return JsonFunctions.ToJson(document, indent);
        //}
    }
}
