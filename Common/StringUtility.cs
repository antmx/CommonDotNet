using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;

namespace Netricity.Common
{
	public class StringUtility
	{
		/// <summary>
		/// Chops the start off a string. Case-insensitive.
		/// </summary>
		/// <param name="text">The text.</param>
		/// <param name="start">The start.</param>
		/// <returns></returns>
		public static string ChopStart(string text, string start)
		{
			if (!string.IsNullOrEmpty(text) && !string.IsNullOrEmpty(start) && text.ToLower().StartsWith(start.ToLower(), true, CultureInfo.InvariantCulture))
			{
				return text.Substring(start.Length);
			}
			return text;
		}

		/// <summary>
		/// Chops the end off a string. Case-insensitive.
		/// </summary>
		/// <param name="text">The text.</param>
		/// <param name="end">The end.</param>
		/// <returns></returns>
		public static string ChopEnd(string text, string end)
		{
			if (!string.IsNullOrEmpty(text) && !string.IsNullOrEmpty(end) && text.ToLower().EndsWith(end.ToLower(), true, CultureInfo.InvariantCulture))
			{
				int length = text.LastIndexOf(end, StringComparison.InvariantCultureIgnoreCase);
				return text.Substring(0, length);
			}
			return text;
		}

		public static string[] ConvertCsvToStringArray(string str, char separator = ',', StringSplitOptions options = StringSplitOptions.RemoveEmptyEntries)
		{
			if (str == null)
				return null;

			var array = str.Split(new[] { separator }, options);

			return array;
		}

		public static object Convert(string str, Type toType)
		{
			object objectValue = null;
			bool canConvert;
			TypeConverter converter = TypeDescriptor.GetConverter(toType);

			if (converter == null || !converter.CanConvertFrom(typeof(string)))
			{
				canConvert = false;
			}
			else
			{
				canConvert = true;
			}

			try
			{
				if (canConvert)
				{
					objectValue = RuntimeHelpers.GetObjectValue(converter.ConvertFrom(str));
				}
				else
				{
					objectValue = RuntimeHelpers.GetObjectValue(System.Convert.ChangeType(str, toType));
				}
			}
			catch (FormatException)
			{
			}

			return objectValue;
		}

		public static T Convert<T>(string str)
		{
			Type objType = typeof(T);
			T objT = (T)Convert(str, objType);
			return objT;
		}

		/// <summary>
		/// Converts the specified string to a value of the given type.
		/// If conversion fails, <paramref name="defaultVal"/> is returned.
		/// </summary>
		/// <typeparam name="T">The type to conver to.</typeparam>
		/// <param name="str">The string to convert.</param>
		/// <param name="defaultVal">The default value for when conversion fails.</param>
		public static T Convert<T>(string str, T defaultVal)
		{
			Type objType = typeof(T);

			try
			{
				T objT = (T)Convert(str, objType);

				return objT;
			}
			catch (Exception)
			{
				return defaultVal;
			}
		}

		///// <summary>
		///// Dtermines if 2 strings are the same, case-insensitive.
		///// </summary>
		///// <param name="str1">The first string.</param>
		///// <param name="str2">The second string.</param>
		//public static bool EqualsIgnoreCase(string str1, string str2)
		//{
		//	bool equal = string.Equals(str1, str2, StringComparison.OrdinalIgnoreCase);
		//	return equal;
		//}



		public static bool Contains(string str, bool ignoreCase, params string[] args)
		{
			if (str != null && args != null)
			{
				foreach (string arg in args)
				{
					if (arg != null && arg.Length <= str.Length)
					{
						if (ignoreCase)
						{
							if (str.IndexOf(arg, StringComparison.OrdinalIgnoreCase) > -1)
							{
								return true;
							}
						}
						else if (str.IndexOf(arg) > -1)
						{
							return true;
						}
					}
				}
			}

			return false;
		}

		public static string Left(string str, int length)
		{
			if (str.Length < length)
			{
				return str;
			}

			return str.Substring(0, length);
		}

		public static string Lower(string str)
		{
			if (str != null)
			{
				return str.ToLower();
			}
			return null;
		}

		public static string Right(string str, int length)
		{
			if (str == null || str.Length < length)
			{
				return str;
			}
			return str.Substring(str.Length - length);
		}

		/// <summary>
		/// Safely gets the length of the given string.
		/// Returns zero if str is null.
		/// </summary>
		/// <param name="str">The string whose length is needed.</param>
		public static int Length(string str)
		{
			if (str != null)
				return str.Length;

			return 0;
		}

		public static string TruncateToSpace(string textToTruncate, int maxLength, string textToAppendIfTruncated = " ...")
		{
			if (maxLength < 1 || textToTruncate.Length < maxLength || !textToTruncate.Contains(' '))
			{
				return textToTruncate;
			}

			// Search for position of first space char after maxLength.
			int length = textToTruncate.IndexOf(" ", maxLength - 1);

			if (length == -1)
			{
				return textToTruncate;
			}

			return textToTruncate.Substring(0, length) + textToAppendIfTruncated;

		}

		public static string Append(string target, string toAppend, string seperator)
		{
			if (target == null)
				target = string.Empty;

			if (!string.IsNullOrWhiteSpace(toAppend))
			{
				if (target.Length > 0)
					target += seperator;

				target += toAppend;
			}

			return target;
		}

		public static string ToSpacedCaps(string str)
		{
			if (str == null)
				return null;

			str = Regex.Replace(str, "([a-z](?=[A-Z])|[A-Z](?=[A-Z][a-z]))", "$1 ");

			return str;
		}

		/// <summary>
		/// e.g. '5' 11"'
		/// </summary>
		/// <param name="centimetres"></param>
		/// <param name="strDefault"></param>
		public static string FormatCmAsFeetAndInches(double? centimetres, string strDefault = "-")
		{
			if (!(centimetres > 0))
				return strDefault;

			// Convert CMs to feet
			var feet = (centimetres.Value / 100D) * 3.2808399D;

			feet = Math.Round(feet, 2);

			var wholeFeet = Math.Floor(feet);

			var inches = feet - wholeFeet;

			// Handle inch component
			if (inches > 0D)
			{
				// Convert the decimal places to inches
				inches = inches * 12;

				// Round to nearest whole inch
				inches = Math.Round(inches, 0);

				if (inches == 12)
				{
					inches = 0;
					wholeFeet++;
				}
			}

			// Return somthing like 5' 11"
			return string.Format("{0}' {1:0}\"", wholeFeet, inches);
		}

		/// <summary>
		/// e.g. '11st 6lb'
		/// </summary>
		/// <param name="kilos"></param>
		/// <param name="strDefault"></param>
		public static string FormatKilosAsStonesAndPounds(double? kilos, string strDefault = "-")
		{
			if (!(kilos > 0))
				return strDefault;

			// Convert kilos to stones
			var stone = System.Convert.ToDecimal(kilos.Value) * 0.157473M;

			stone = Math.Round(stone, 2);

			var wholeStone = Math.Floor(stone);

			var pounds = stone - wholeStone;

			// Handle pounds component
			if (pounds > 0M)
			{
				// Convert the decimal places to pounds
				pounds = pounds * 14;

				// Round to nearest whole inch
				pounds = Math.Round(pounds, 0);

				if (pounds == 14)
				{
					pounds = 0;
					wholeStone++;
				}
			}

			// Return somthing like 5' 11"
			return string.Format("{0}st {1:0}lb", wholeStone, pounds);
		}

		/// <summary>
		/// Returns an Excel-style column name for the given number.
		/// e.g. 1 = A, 2 = B...27 = AA...703 = AAA
		/// </summary>
		/// <param name="columnNumber">The 1-based column number.</param>
		public static string GetColNameFromNumber(int columnNumber)
		{
			int dividend = columnNumber;
			string columnName = String.Empty;
			int modulo;

			while (dividend > 0)
			{
				modulo = (dividend - 1) % 26;
				columnName = System.Convert.ToChar(65 + modulo).ToString() + columnName;
				dividend = (int)((dividend - modulo) / 26);
			}

			return columnName;
		}

		/// <summary>
		/// Strips HTML tags from the giving string, returning just the plain text.
		/// </summary>
		/// <param name="html">A string of HTML.</param>
		public static string StripHtml(string html)
		{
			if (html == null)
				return null;

			string pattern = "<(.|\\n)*?>";
			return Regex.Replace(html, pattern, string.Empty);
		}

		/// <summary>
		/// Builds and returns a delimited string from the given strings that aren't empty.
		/// </summary>
		/// <param name="separator">The delimiter, e.g. ', '</param>
		/// <param name="values">The strings to merge.</param>
		public static string Join(string separator, params string[] values)
		{
			if (values == null)
				throw new ArgumentNullException(nameof(values));

			var str = String.Join(
				separator,
				values.Where(s => !string.IsNullOrWhiteSpace(s))
			);

			return str;
		}

		public static string ListItemSeparator(int currentIndex, int total, bool fullStopOnLast = false)
		{
			if (currentIndex == total - 2)
				return " and ";
			else if (currentIndex < total - 1)
				return ", ";
			else if (currentIndex == total - 1 && fullStopOnLast)
				return ".";

			return null;
		}

		public static bool IsIn(string str, params string[] list)
		{
			return IsIn(str, false, list);
		}

		public static bool IsIn(string str, bool caseSensitive, params string[] list)
		{
			if (str == null || list == null)
				return false;

			var comparison = caseSensitive
				? StringComparison.Ordinal
				: StringComparison.OrdinalIgnoreCase;

			if (list.Any(s => str.Equals(s, comparison)))
				return true;

			return false;
		}

      public static bool IsEmailAddress(string str)
      {
         if (string.IsNullOrEmpty(str) || !Regex.IsMatch(str, "^.+@.+\\..{2,10}$"))
         {
            return false;
         }

         return true;
      }

      /// <summary>
      /// Returns the string converted to PascalCase.
      /// </summary>
      /// <param name="str">The string.</param>
      public static string ToPascalCase(string str)
		{
			if (str == null)
				return null;

			int idx = 0; // index in str
			bool isFirst = true; // first letter flag
			string o = string.Empty; // output string
			string whitespaceChars = string.Empty + (char)13 + (char)10 + (char)9 + (char)160 + ' '; // characters considered as white space

			while (idx < str.Length)
			{
				char c = str[idx];

				if (isFirst)
				{
					o += Char.ToUpper(c);
					isFirst = false;
				}
				else
				{
					o += Char.ToLower(c);
				}

				if (whitespaceChars.Contains(c))
					isFirst = true;

				idx++;
			}

			return o;
		}

		public static string SplitCaps(string str)
		{
			if (str == null)
				return null;

			var output = new StringBuilder("");

			foreach (char letter in str)
			{
				if (Char.IsUpper(letter) && output.Length > 0)
					output.Append(" " + letter);
				else
					output.Append(letter);
			}

			return output.ToString();
		}

		/// <summary>
		/// In a specified input string, uses a case-insensitive comparison to replace all strings that match a specified regular expression with a specified replacement string.
		/// </summary>
		/// <param name="input">The string to search in for a match.</param>
		/// <param name="pattern">The string or regular expression pattern to match in <paramref name="input"/>. Can be repeated in <paramref name="input"/>.</param>
		/// <param name="replacement">The new string to replace any occurances of <paramref name="pattern"/> with.</param>
		/// <returns>A new string that is identical to the input string, except that the replacement string takes the place of each matched string. If pattern Is Not matched in the current instance, the method returns the current instance unchanged.</returns>
		public static string ReplaceCI(string input, string pattern, string replacement)
		{
		    if (input == null || string.IsNullOrEmpty(pattern))
		        return input;
		
		    if (replacement == null)
		        replacement = string.Empty;
		
		    string replaced = Regex.Replace(input, pattern, replacement, RegexOptions.IgnoreCase);
		
		    return Regex.Unescape(replaced);
		}

		public static bool ContainsCI(string str, params string[] values)
		{
			if (str == null)
				return false;

			return Contains(str, true, values);
		}

		/// <summary>
		/// Returns a value indicating if the string matches the specified value, using a case-insensitive comparison.
		/// </summary>
		/// <param name="str">The string.</param>
		/// <param name="value">The value to compare with.</param>
		public static bool EqualsCI(string str1, string str2)
		{
			if (str1 == null)
				return false;

			bool equal = string.Equals(str1, str2, StringComparison.OrdinalIgnoreCase);

			return equal;
		}

		public static bool StartsWithCI(string str, string value)
		{
			if (str == null)
				return false;

			return str.StartsWith(value, StringComparison.OrdinalIgnoreCase);
		}

		public static bool EndsWithCI(string str, string value)
		{
			if (str == null)
				return false;

			return str.EndsWith(value, StringComparison.OrdinalIgnoreCase);
		}

		public static string StripCarriageReturns(string text)
		{
			return Regex.Replace(text, @"(\r|\n)+", "", RegexOptions.None | RegexOptions.Multiline);
		}

		public static string RemoveWhitespace(string value)
		{
			return value.Replace(" ", "").Trim();
		}
		
		public static string TruncateAtWord(string str, int length)
		{
		    if (str == null || str.Length < length) {
			return str;
		    }
			
		    int iNextSpace = str.LastIndexOf(" ", length, StringComparison.Ordinal);
			
		    return string.Format("{0}…", str.Substring(0, (iNextSpace > 0) ? iNextSpace : length).Trim());
		}		
	}
}
