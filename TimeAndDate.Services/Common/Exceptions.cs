using System;
namespace TimeAndDate.Services.Common
{
	public class ServerSideException : Exception { public ServerSideException(string message) : base(message) { } }
	public class MalformedXMLException : Exception { public MalformedXMLException(string message) : base(message) { } }
	public class IdFormatException : Exception { public IdFormatException(string message) : base(message) { } }
	public class QueriedDateOutOfRangeException : Exception { public QueriedDateOutOfRangeException(string message) : base(message) { } }
	public class LocalTimeDoesNotExistException : Exception { public LocalTimeDoesNotExistException(string message) : base(message) { } }
	public class MissingTimeChangesException : Exception { public MissingTimeChangesException(string message) : base(message) { } }
	public class InvalidIsoStringException : Exception { public InvalidIsoStringException(string message) : base(message) { } }
}

