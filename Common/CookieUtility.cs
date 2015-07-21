using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;

namespace Netricity.Common
{
public static	class CookieUtility
	{
		private static HttpCookiesSection _httpCookiesSection;

		public static T GetCookieValue<T>(HttpContext context, string cookieName, string subKey, T defaultValue)
		{
			if (context != null && context.Request != null)
			{
				var cookie = context.Request.Cookies[cookieName];

				if (cookie != null)
				{
					string elementValue = null;

					if (subKey != null)
						elementValue = cookie.Values[subKey];
					else
						elementValue = cookie.Value;

					try
					{
						T obj = (T)StringUtility.Convert(elementValue, typeof(T));

						if (obj == null)
							obj = defaultValue;

						return obj;
					}
					catch
					{
						return defaultValue;
					}
				}
			}

			// If we get this far, return the default value for T, e.g. 0 for int, false for bool, null for reference types.
			//return default(T);
			return defaultValue;
		}

		public static T GetCookieValue<T>(HttpContext context, string cookieName, T defaultValue)
		{
			return GetCookieValue<T>(context, cookieName, null, defaultValue);
		}

		public static void SetCookieValue(HttpContext context, string cookieName, object value, DateTime? expires = null)
		{
			SetCookieValue(context, cookieName, null, value);
		}

		public static void SetCookieValue(HttpContext context, string cookieName, string subKey, object value, DateTime? expires = null)
		{
			if (context == null)
				throw new ArgumentException(nameof(context));

			if (string.IsNullOrWhiteSpace(cookieName))
				throw new ArgumentException(nameof(cookieName));

			if (context.Response != null)
			{
				if (subKey != null)
				{
					// Maintain any subkeys that may have been set previously
					string reqVals = null;

					if (context.Request.Cookies.AllKeys.Contains(cookieName))
						reqVals = context.Request.Cookies[cookieName].Value;

					if (reqVals != null)
					{
						// reqVals will be a querystring-like value, e.g 'foo=lorem&bar=ipsum',
						// and setting the cookie's value to it also restores the cookie's subkeys
						context.Response.Cookies[cookieName].Value = reqVals;
					}

					context.Response.Cookies[cookieName][subKey] = value.ToString();
				}
				else
				{
					context.Response.Cookies[cookieName].Value = value.ToString();
				}

				if (expires.HasValue)
					context.Response.Cookies[cookieName].Expires = expires.Value;
			}
		}

		/// <summary>
		/// Removes the cookie.
		/// </summary>
		/// <param name="cookieName">Name of the cookie.</param>
		/// <param name="pathIsAppRoot">if set to <c>true</c>, the cookie's Path property is set to that virtual path of the current application, e.g '/admin'.
		/// This is useful for situations where 2 cookies exist with the same name and domain, but are used for different virtual folder paths.</param>
		public static void RemoveCookie(HttpContext context, string cookieName, bool pathIsAppRoot = false)
		{
			if(context == null)
				throw new ArgumentException(nameof(context));

			if (string.IsNullOrWhiteSpace(cookieName))
				throw new ArgumentException(nameof(cookieName));

			try
			{
				if (context.Request.Cookies[cookieName] == null)
				{
					return;
				}

				var objCookie = new HttpCookie(cookieName);
				var now = DateTime.Now;
				objCookie.Expires = now.AddYears(-1); // Set cookie's expiry to 1 year in past, thus telling browser to 'remove' the cookie.

				if (pathIsAppRoot)
					objCookie.Path = context.Request.ApplicationPath;

				objCookie.HttpOnly = HttpCookiesSection.HttpOnlyCookies;

				context.Response.Cookies.Add(objCookie);
			}
			catch
			{
			}
		}

		/// <summary>
		/// Gets an object representing the values configured in the 'configuration/system.web/httpCookies' section of web.config.
		/// </summary>
		public static HttpCookiesSection HttpCookiesSection
		{
			get
			{
				if (_httpCookiesSection == null)
				{
					_httpCookiesSection = ConfigurationManager.GetSection("system.web/httpCookies") as HttpCookiesSection;

					if (_httpCookiesSection == null)
						_httpCookiesSection = new HttpCookiesSection();
				}

				return _httpCookiesSection;
			}
		}

		/// <summary>
		/// Extracts the value of the cookie parameter.
		/// </summary>
		/// <param name="cookieKvp">e.g. 'UserId=123', returns '123'</param>
		public static string ExtractCookieParamValue(string cookieKvp)
		{
			if (cookieKvp == null)
				return null;

			int eqIdx = cookieKvp.IndexOf('=');

			if (eqIdx < 0 || eqIdx == cookieKvp.Length - 1)
				return null;

			string value = cookieKvp.Substring(eqIdx + 1);

			return value;
		}
	}
}
