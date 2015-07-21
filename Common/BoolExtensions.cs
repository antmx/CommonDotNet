using System;

namespace Netricity.Common
{
	/// <summary>
	/// System.Boolean extension methods.
	/// </summary>
	public static class BoolEx
	{
		/// <summary>
		/// Gets the current bool as a string or, if null, the default.
		/// </summary>
		/// <param name="b">The bool.</param>
		/// <param name="defaultValue">The default value.</param>
		public static string ToString(this bool? b, string defaultValue)
		{
			if (!b.HasValue)
				return defaultValue;

			return b.ToString();
		}

		public static string ToYesNo(this bool b)
		{
			return b ? "Yes" : "No";
		}
	}
}
