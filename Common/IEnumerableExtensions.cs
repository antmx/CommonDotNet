using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace Netricity.Common
{
	/// <summary>
	/// IEnumerable<T> extension methods.
	/// </summary>
	public static class IEnumerableExtensions
	{
		/// <summary>
		/// Builds and returns a DataTable from the items in the collection.
		/// </summary>
		/// <typeparam name="T">The type of the items in the collection.</typeparam>
		/// <param name="items">The items in the collection.</param>
		/// <param name="tableName">Name of the DataTable.</param>
		public static DataTable ToDataTable<T>(this IEnumerable<T> items, string tableName = null)
		{
			if (items == null)
				return null;

			var table = new DataTable(tableName ?? typeof(T).Name);

			// Get the type's public instance properties.
			PropertyInfo[] props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

			// Add columns for each property
			foreach (var prop in props)
			{
				table.Columns.Add(prop.Name, prop.PropertyType.GetBaseType());
			}

			// Add rows for each item in the collection
			foreach (var item in items)
			{
				// Populate row's column data
				var values = new object[props.Length];

				for (var i = 0; i < props.Length; i++)
				{
					values[i] = props[i].GetValue(item, null);
				}

				table.Rows.Add(values);
			}

			return table;
		}
	}
}
