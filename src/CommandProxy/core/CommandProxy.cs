using System;
                    Write(CreateErrorDocument("Invalid Message Format").OuterXml);
                    Write(CreateErrorDocument("Message Not Authorized").OuterXml);
                    response = CreateErrorDocument("Command not recognized.").OuterXml;
            requestDocument.LoadXml(s);

            XmlNode commandNode = requestDocument.SelectSingleNode("/command");
                return CreateErrorDocument("Input not recognized").OuterXml;

            XmlDocument responseDocument = new XmlDocument();
            XmlNode responseElement = responseDocument.CreateNode(XmlNodeType.Element, "response", "");
            responseDocument.AppendChild(responseElement);

            string id = ExtractId(commandNode);
            XmlAttribute idAttribute = responseDocument.CreateAttribute("id");
            idAttribute.Value = id;
            responseElement.Attributes.SetNamedItem(idAttribute);
            XmlAttribute typeAttribute = responseDocument.CreateAttribute("type");
                            responseDocument = command.Exec(requestDocument, responseDocument);
                            typeAttribute.Value = "success";
                            responseDocument.FirstChild.Attributes.SetNamedItem(typeAttribute);

                            return responseDocument.OuterXml;
                            return CreateErrorDocument(e.Message, id).OuterXml;
                            responseDocument = command.Exec(requestDocument, responseDocument);

                            typeAttribute.Value = "success";
                            responseDocument.FirstChild.Attributes.SetNamedItem(typeAttribute);

                            return responseDocument.OuterXml;
                            return CreateErrorDocument(e.Message, id).OuterXml;

        public XmlDocument CreateErrorDocument(string msg)
            xmlDocument.AppendChild(xmlDocument.ReadNode(xmlReader));
            return xmlDocument;

        public XmlDocument CreateErrorDocument(string msg, string id)

            if (id == null)
            {
                return errorResponse;
            }

            XmlAttribute idAttribute = errorResponse.CreateAttribute("id");
            idAttribute.Value = id;

            errorResponse.FirstChild.Attributes.SetNamedItem(idAttribute);