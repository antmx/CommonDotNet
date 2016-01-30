using System;

namespace Netricity.Common
{
	public static class UriExtensions
	{
		public static bool IsEqual(this Uri url, Uri other, bool caseSensitive)
		{
			if (url == null || other == null)
				return false;

			var compairisonType = caseSensitive
				? StringComparison.Ordinal
				: StringComparison.OrdinalIgnoreCase;

			var equal = string.Compare(url.ToString(), other.ToString(), compairisonType);

			return equal == 0;
		}
	}
}
