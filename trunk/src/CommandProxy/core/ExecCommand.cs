using System.Xml;using System.Diagnostics;using System.Collections.Generic;using System.Text;using System.IO;using System.ComponentModel;using System;/*<command authtoken="">	<exec>		<path></path>		<arguments>			<arg></arg>		</arguments>	</exec></command><command authtoken=""><exec><path>/Applications/TextEdit.app/Contents/MacOS/TextEdit</path><arguments><arg></arg></arguments></exec></command>*/namespace CommandProxy.Commands{	public class ExecCommand :IProxyCommand	{		public XmlDocument Exec(XmlDocument requestDocument, XmlDocument responseDocument)		{			string path = null;            StringBuilder args = new StringBuilder();            XmlNode commandNode = requestDocument.FirstChild.SelectSingleNode("exec");            XmlNode pathNode = commandNode.SelectSingleNode("path");            if (pathNode == null)            {                throw new Exception("Path not found");                //return CommandProxy.CreateErrorNode("Path not found");            }            path = pathNode.InnerXml;            XmlNodeList argNodes = commandNode.SelectNodes("arguments/arg");            if (argNodes != null && argNodes.Count > 0)            {                    foreach (XmlNode a in argNodes)                    {                        //todo: wrap these in double quotes?                        args.Append(CleanArgument(a.InnerXml));                        args.Append(" ");                    }            }            XmlAttribute captureAt = commandNode.Attributes["capture"];            bool capture = false;            if (captureAt != null)            {                capture = (captureAt.Value == "true") ? true : false;            }				Process p = new Process();			p.StartInfo.FileName = path;            string argStr = args.ToString();            if (argStr.Length > 0)            {                p.StartInfo.Arguments = argStr.Remove(argStr.Length - 1);            }            if (capture)            {                p.StartInfo.UseShellExecute = false;                p.StartInfo.RedirectStandardOutput = true;            }            try            {                p.Start();            }            catch (Win32Exception e)            {                throw e;            }            if(capture)            {                string output = p.StandardOutput.ReadToEnd();                p.WaitForExit();                XmlTextReader xmlReader = new XmlTextReader(new StringReader("<output>" + output + "</output>"));                XmlNode outputNode = responseDocument.ReadNode(xmlReader);                responseDocument.FirstChild.AppendChild(outputNode);            }            return responseDocument;		}        private string CleanArgument(string s)        {            //todo: need to figure out how to escape the strings            //string outStr = "\"" + s.Replace("\"", "\"\"") + "\"";            return s;        }	}}