using System;

namespace Netricity.Common
{
	/// <summary>
	/// System.String extension methods.
	/// </summary>
	public static class StringExtensions
	{
		/// <summary>
		/// Gets the current string or, if null/whitespace, the specified default.
		/// </summary>
		/// <param name="str">The string.</param>
		/// <param name="defaultValue">The default value to return if the string is null/empty/whitespace.</param>
		public static string GetValueOrDefault(this string str, string defaultValue)
		{
			if (string.IsNullOrWhiteSpace(str))
				return defaultValue;

			return str;
		}

		/// <summary>
		/// Gets a value indicating if the string has content (true) or is null/emtpy/whitespace (false).
		/// </summary>
		/// <param name="str">The string.</param>
		public static bool HasValue(this string str)
		{
			return !string.IsNullOrWhiteSpace(str);
		}

		public static string ReplaceCI(this string str, string oldValue, string newValue)
		{
			return StringUtility.ReplaceCI(str, oldValue, newValue);
		}

		public static bool ContainsCI(this string str, params string[] values)
		{
			return StringUtility.ContainsCI(str, values);
		}

		/// <summary>
		/// Returns a value indicating if the string matches the specified value, using a case-insensitive comparison.
		/// </summary>
		/// <param name="str">The string.</param>
		/// <param name="value">The value to compare with.</param>
		public static bool EqualsCI(this string str, string value)
		{
			return StringUtility.EqualsCI(str, value);
		}

		public static bool StartsWithCI(this string str, string value)
		{
			return StringUtility.StartsWithCI(str, value);
		}

		public static bool EndsWithCI(this string str, string value)
		{
			return StringUtility.EndsWithCI(str, value);
		}

		public static string SplitCaps(this string str)
		{
			return StringUtility.SplitCaps(str);
		}

		/// <summary>
		/// Returns the string converted to PascalCase.
		/// </summary>
		/// <param name="str">The string.</param>
		public static string ToPascalCase(this string str)
		{
			return StringUtility.ToPascalCase(str);
		}

		public static bool IsIn(this string str, params string[] list)
		{
			return StringUtility.IsIn(str, false, list);
		}

		public static bool IsIn(this string str, bool caseSensitive, params string[] list)
		{
			return StringUtility.IsIn(str, caseSensitive, list);
		}
   }
}
