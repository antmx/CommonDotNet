using System;

namespace Netricity.Common
{
	/// <summary>
	/// System.Int32 extension methods.
	/// </summary>
	public static class IntExtensions
	{
		/// <summary>
		/// Gets the current integer as a string or, if null, the default.
		/// </summary>
		/// <param name="i">The integer.</param>
		/// <param name="defaultValue">The default value.</param>
		public static string ToString(this int? i, string defaultValue)
		{
			if (!i.HasValue)
				return defaultValue;

			return i.ToString();
		}

		/// <summary>
		/// Determines if the current integer value falls within a given range.
		/// </summary>
		/// <param name="i"></param>
		/// <param name="from"></param>
		/// <param name="to"></param>
		/// <returns></returns>
		public static bool Between(this int i, int from, int to)
		{
			if (from > to)
				throw new ArgumentException("from must be less than or equal to to");

			if (i >= from && i <= to)
				return true;

			return false;
		}
	}
}
