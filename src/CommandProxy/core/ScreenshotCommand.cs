using System;
        public XmlDocument Exec(XmlDocument requestDocument, XmlDocument responseDocument)

            XmlNode commandNode = requestDocument.FirstChild.SelectSingleNode("screenshot");

            XmlTextReader xmlReader = new XmlTextReader(new StringReader("<path>" + path + "</path>"));
            XmlNode pathElement = responseDocument.ReadNode(xmlReader);
            responseDocument.FirstChild.AppendChild(pathElement);

            return responseDocument;