using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Netricity.Common
{
	/// <summary>
	/// Contains helper methods for working with basic objects.
	/// </summary>
	public static class ObjectExtensions
	{
		/// <summary>
		/// Returns a dictionary containing the property names and values of the given object.
		/// </summary>
		/// <param name="obj">The object.</param>
		public static IDictionary<string, object> AnonymousObjectToDictionary(this object obj)
		{
			var dictionary = new Dictionary<string, object>();

			if (obj != null)
			{
				foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(obj))
				{
					dictionary.Add(property.Name.Replace('\u005F', '-'), property.GetValue(obj));
				}
			}

			return dictionary;
		}

		public static string AnonymousObjectToQueryString(this object obj)
		{
			string query = string.Empty;

			var dictionary = AnonymousObjectToDictionary(obj);

			foreach (var param in dictionary)
			{
				if (param.Key.HasValue() && param.Value != null && param.Value.ToString().HasValue())
				{
					if (query.Length > 0)
						query += "&";

					query += param.Key + "=" + param.Value;
				}
			}

			return query;
		}
	}
}
