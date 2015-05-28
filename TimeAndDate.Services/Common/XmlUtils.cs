using System;
using System.Xml;
using TimeAndDate.Services.Common;


namespace TimeAndDate.Services
{
	internal static class XmlUtils
	{
		internal static void CheckForErrors (string result)
		{
			var xml = new XmlDocument ();
			xml.LoadXml (result);
			
			var dataNode = xml.DocumentElement;
			if (dataNode.Attributes ["version"] == null)
				throw new MalformedXMLException ("Expected 'version' attribute in data node"); 				 
			
			if (dataNode.SelectSingleNode ("error") != null)
				HandleError (dataNode.SelectSingleNode ("error"));
		}
		
		internal static void HandleError (XmlNode node)
		{
			var errorMessage = node != null ? node.InnerText : "Unspecified error";
			throw new ServerSideException("An error occured on server-side: " + errorMessage);
		}
	}
}

