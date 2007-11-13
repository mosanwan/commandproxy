/*
    The MIT License

    Copyright (c) 2007 Mike Chambers

    Permission is hereby granted, free of charge, to any person obtaining a copy
    of this software and associated documentation files (the "Software"), to deal
    in the Software without restriction, including without limitation the rights
    to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
    copies of the Software, and to permit persons to whom the Software is
    furnished to do so, subject to the following conditions:

    The above copyright notice and this permission notice shall be included in
    all copies or substantial portions of the Software.

    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
    AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
    LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
    OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
    THE SOFTWARE.
*/

using System.Xml;
using System.Diagnostics;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.ComponentModel;
using System;


/*
<command authtoken="">
	<exec>
		<path></path>
		<arguments>
			<arg></arg>
		</arguments>
	</exec>
</command>

<command authtoken=""><exec><path>/Applications/TextEdit.app/Contents/MacOS/TextEdit</path><arguments><arg></arg></arguments></exec></command>
*/

namespace CommandProxy.Commands
{
	public class ExecCommand :IProxyCommand
	{
		public XmlDocument Exec(XmlDocument requestDocument, XmlDocument responseDocument)
		{
			string path = null;
            StringBuilder args = new StringBuilder();

            XmlNode commandNode = requestDocument.FirstChild.SelectSingleNode("exec");

            XmlNode pathNode = commandNode.SelectSingleNode("path");

            if (pathNode == null)
            {
                throw new Exception("Path not found");
                //return CommandProxy.CreateErrorNode("Path not found");
            }

            path = pathNode.InnerXml;

            XmlNodeList argNodes = commandNode.SelectNodes("arguments/arg");

            if (argNodes != null && argNodes.Count > 0)
            {
                    foreach (XmlNode a in argNodes)
                    {
                        //todo: wrap these in double quotes?
                        args.Append(CleanArgument(a.InnerXml));
                        args.Append(" ");
                    }
            }

            XmlAttribute captureAt = commandNode.Attributes["capture"];

            bool capture = false;
            if (captureAt != null)
            {
                capture = (captureAt.Value == "true") ? true : false;
            }
	
			Process p = new Process();
			p.StartInfo.FileName = path;

            string argStr = args.ToString();
            if (argStr.Length > 0)
            {
                p.StartInfo.Arguments = argStr.Remove(argStr.Length - 1);
            }

            if (capture)
            {
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.RedirectStandardOutput = true;
            }

            try
            {
                p.Start();
            }
            catch (Win32Exception e)
            {
                throw e;
            }

            if(capture)
            {
                string output = p.StandardOutput.ReadToEnd();
                p.WaitForExit();

                XmlTextReader xmlReader = new XmlTextReader(new StringReader("<output>" + output + "</output>"));
                XmlNode outputNode = responseDocument.ReadNode(xmlReader);
                responseDocument.FirstChild.AppendChild(outputNode);

            }

            return responseDocument;
		}

        private string CleanArgument(string s)
        {
            //todo: need to figure out how to escape the strings
            //string outStr = "\"" + s.Replace("\"", "\"\"") + "\"";
            return s;
        }
	}
}
