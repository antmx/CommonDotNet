using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Netricity.Common
{
	/// <summary>
	/// Format provider for formatting boolean values.
	/// </summary>
	public class BoolFormatProvider : IFormatProvider, ICustomFormatter
	{
		#region "Methods"

		/// <summary>
		/// Initializes a new instance of the <see cref="BoolFormatProvider" /> class.
		/// </summary>
		public BoolFormatProvider()
			: base()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="BoolFormatProvider" /> class.
		/// </summary>
		/// <param name="trueString">The yes string.</param>
		/// <param name="falseString">The no string.</param>
		public BoolFormatProvider(string trueString, string falseString)
			: base()
		{
			this.TrueString = trueString;
			this.FalseString = falseString;
		}

		/// <summary>
		/// Converts the value of a specified object to an equivalent string representation using specified format and culture-specific formatting information.
		/// </summary>
		/// <param name="formatStr">A format string containing formatting specifications.</param>
		/// <param name="arg">An object to format.</param>
		/// <param name="formatProvider">An object that supplies format information about the current instance.</param>
		/// <returns></returns>
		public string Format(string formatStr, object arg, IFormatProvider formatProvider)
		{
			bool value = Convert.ToBoolean(arg);
			formatStr = (formatStr == null ? null : formatStr.Trim().ToLower());

			switch (formatStr)
			{
				case "yn":
					return value ? this.TrueString : this.FalseString;
				default:
					if (arg is IFormattable)
						return ((IFormattable)arg).ToString(formatStr, formatProvider);
					else
						return arg.ToString();
			}
		}

		/// <summary>
		/// Returns an object that provides formatting services for the specified type.
		/// </summary>
		/// <param name="formatType">An object that specifies the type of format object to return.</param>
		/// <returns>
		/// An instance of the object specified by <paramref name="formatType" />, if the <see cref="T:System.IFormatProvider" /> implementation can supply that type of object; otherwise, null.
		/// </returns>
		public object GetFormat(Type formatType)
		{
			if (formatType == typeof(ICustomFormatter))
			{
				return this;
			}

			return null;
		}

		public static string FormatYesNo(bool b)
		{
			string formatted = string.Format(new BoolFormatProvider(), "{0:yn}", b);

			return formatted;
		}

		public static string FormatYesNo(bool b, string trueString, string falseString)
		{
			string formatted = string.Format(new BoolFormatProvider(trueString, falseString), "{0:yn}", b);

			return formatted;
		}

		#endregion

		#region "Properties"

		string _trueString = "Yes";
		string _falseString = "No";

		public string TrueString
		{
			get { return this._trueString; }
			set { this._trueString = value; }
		}

		public string FalseString
		{
			get { return this._falseString; }
			set { this._falseString = value; }
		}

		#endregion
	}
}
