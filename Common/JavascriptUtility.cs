using System;
using System.Text;

namespace Netricity.Common
{
	public static class JavascriptUtility
	{
		/// <summary>
		/// Encodes the given string so that it can be embedded into javascript as a string literal.
		/// The returned value is wrapped in "double-quotes".
		/// If <paramref name="str" /> is null, <see cref="String.Empty" /> is returned.
		/// </summary>
		/// <param name="str">The string of html/text to encode.</param>
		public static string EncodeJsString(string str)
		{
			if (str == null)
				return string.Empty;

			StringBuilder sb = new StringBuilder();
			sb.Append("\"");
			foreach (char c in str)
			{
				switch (c)
				{
					case '"':
						sb.Append("\\\"");
						break;

					case '\\':
						sb.Append("\\\\");
						break;

					case ControlChars.Back:
						sb.Append("\\b");
						break;

					case ControlChars.FormFeed:
						sb.Append("\\f");
						break;

					case ControlChars.Lf:
						sb.Append("\\n");
						break;

					case ControlChars.Cr:
						sb.Append("\\r");
						break;

					case ControlChars.Tab:
						sb.Append("\\t");
						break;

					default:
						int i = (int)c;
						if (i < 32 || i > 127)
							sb.AppendFormat("\\u{0:X04}", i);
						else
							sb.Append(c);
						break;
				}
			}

			sb.Append("\"");

			return sb.ToString();
		}
	}
}
