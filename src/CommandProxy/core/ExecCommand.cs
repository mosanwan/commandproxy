using System.Xml;

            XmlNode commandNode = requestDocument.FirstChild.SelectSingleNode("exec");

            XmlNode pathNode = commandNode.SelectSingleNode("path");

            XmlNodeList argNodes = commandNode.SelectNodes("arguments/arg");

                XmlTextReader xmlReader = new XmlTextReader(new StringReader("<output>" + output + "</output>"));
                XmlNode outputNode = responseDocument.ReadNode(xmlReader);
                responseDocument.FirstChild.AppendChild(outputNode);