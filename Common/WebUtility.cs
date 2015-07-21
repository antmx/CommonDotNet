using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Runtime.CompilerServices;

namespace Netricity.Common
{
	public static class WebUtility
	{
		public static string[] ParseRequestParamCsv(NameValueCollection @params, string paramName)
		{
			if (@params != null && !string.IsNullOrEmpty(paramName))
			{
				var array = StringUtility.ConvertCsvToStringArray(@params[paramName]);

				return array;
			}

			return null;
		}

		/// <summary>
		/// Tries to parse a parameter in the given RouteData.
		/// </summary>
		/// <typeparam name="T">The type of the data to parse.</typeparam>
		/// <param name="routeData">Usually Page.RouteData.Values.</param>
		/// <param name="paramName">Name of the parameter, e.g. 'id'</param>
		/// <param name="defaultValue">The default value to return if an item with the given key doesn't exist or its value can't be parsed to T.</param>
		public static T ParseRouteData<T>(IDictionary<string, object> routeData, string paramName, T defaultValue)
		{
			if (routeData != null && routeData.ContainsKey(paramName))
			{
				try
				{
					return StringUtility.Convert<T>((string)routeData[paramName]);
				}
				catch
				{
				}
			}

			return defaultValue;
		}

		public static T ParseRequestParam<T>(NameValueCollection @params, IEnumerable<string> paramNames, T defaultValue)
		{
			foreach (var paramName in paramNames)
			{
				var value = ParseRequestParam(@params, paramName, defaultValue);

				if (!defaultValue.Equals(value))
					return value;
			}

			return defaultValue;
		}

		public static T ParseRequestParam<T>(NameValueCollection @params, string paramName, T defaultValue)
		{
			T t;

			if (@params != null && !string.IsNullOrEmpty(paramName))
			{
				try
				{
					Type type = typeof(T);
					object objectValue = RuntimeHelpers.GetObjectValue(StringUtility.Convert(@params[paramName], type));
					t = (T)objectValue;
					return t;
				}
				catch
				{

				}
			}

			t = defaultValue;

			return t;
		}
	}
}
