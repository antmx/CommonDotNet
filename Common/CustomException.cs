using System;

namespace Netricity.Common
{
	public class CustomException : ApplicationException
	{
		public CustomException(string message)
			 : base(message)
		{

		}

		public CustomException(string format, params object[] args)
			 : base(string.Format(format, args))
		{

		}
	}
}