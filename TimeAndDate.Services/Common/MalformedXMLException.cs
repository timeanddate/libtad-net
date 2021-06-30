using System;
namespace TimeAndDate.Services
{
	public class MalformedXMLException : Exception { public MalformedXMLException(string message) : base(message) { } }
}
