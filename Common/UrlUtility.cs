using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Netricity.Common
{
	public static class UrlUtility
	{
		/// <summary>
		/// Characters that are valid in a URL path segment, i.e. between the forward slashed, e.g. '/path/segment/'
		/// </summary>
		public static readonly string ValidUrlPathSegmentChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789-_ ";

		/// <summary>
		/// Forms a safe URL path segment, .e.g. 'page name' becomes 'page-name'.
		/// Invalid characters are replaced by '-'.
		/// </summary>
		/// <param name="segment">The segment string to make safe.</param>
		public static string FormSafeUrlPathSegment(string segment)
		{
			if (segment == null)
			{
				return string.Empty;
			}

			segment = segment.Trim();

			// Filter to valid path segment chars.
			segment = new string(segment
				.Where(c => ValidUrlPathSegmentChars.Contains(c))
				.ToArray());

			// Condense multiple spaces to one space.
			if (segment.Contains("  "))
			{
				var pattern = "[ ]{2,}"; // 2 or more space characters
				var rxCondense = new Regex(pattern, RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.CultureInvariant);
				segment = rxCondense.Replace(segment, " ");
			}

			var spaceReplacementChar = "-";

			// Replace space chars with the configured space-replacement char.
			if (segment.Contains(' '))
			{
				segment = segment.Replace(' ', '-');
			}

			// Condense multiple space-replacement chars to one space-replacement char.
			if (segment.Contains(spaceReplacementChar + spaceReplacementChar))
			{
				var pattern = "[" + spaceReplacementChar + "]{2,}";
				var rxCondense = new Regex(pattern, RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.CultureInvariant);
				segment = rxCondense.Replace(segment, spaceReplacementChar);
			}

			segment = segment.Trim(spaceReplacementChar.ToCharArray());

			if (segment.Length == 0)
			{
				throw new CustomException("An empty URL segment has been formed. Please provide some alpha-numeric characters.");
			}

			return segment;
		}

		public static string AppendQueryParam(string url, string paramName, object paramValue)
		{
			if (url == null)
				throw new ArgumentNullException(nameof(url));

			if (paramName == null)
				throw new ArgumentNullException(nameof(paramName));

			if (paramValue != null)
			{
				if (url.Contains('?'))
					url += '&';
				else
					url += '?';

				url += paramName + '=' + paramValue;
			}

			return url;
		}

		public static string RemoveQueryParam(string url, string paramName)
		{
			return ReplaceQueryParam(url, paramName);
		}

		public static string ReplaceQueryParam(string url, string paramName, string replacement = null)
		{
			if (paramName == null)
				throw new ArgumentNullException(nameof(paramName));

			if (string.IsNullOrWhiteSpace(url))
				return url;

			var schemeSep = "://";
			var schemeSepIdx = url.IndexOf(schemeSep);
			var qIdx = url.IndexOf("?");
			var ampIdx = url.IndexOf("&");

			var query = string.Empty;

			if (qIdx > -1)
				query = url.Substring(qIdx);
			else
				query = url;

			//if (schemSepIdx > -1 && (schemSepIdx < qIdx || qIdx == -1) && (schemSepIdx < ampIdx || ampIdx == -1))
			//{
			//   throw new ArgumentException("query must not be a fully qualified URL");
			//}

			if (schemeSepIdx > -1 && qIdx == -1)
				return url;

			if (url.LastIndexOf(schemeSep) > schemeSepIdx)
				throw new ArgumentException("querystring must not contain a fully qualified URL");

			var preQuery = url.Substring(0, Math.Max(qIdx, 0));

			var queryColl = HttpUtility.ParseQueryString(query);

			if (replacement != null)
				queryColl[paramName] = replacement; // Add or replace param
			else
				queryColl.Remove(paramName); // remove param

			//string newUrl = "" + queryColl;
			string newUrl = preQuery;

			//if (preQuery.Length > 0)
			//   newUrl += (preQuery + "?");

			if (newUrl.Length > 0 && queryColl.Count > 0)
				newUrl += "?";

			newUrl += queryColl;

			return newUrl;
		}
	}
}
