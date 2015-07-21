using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Netricity.Common
{
	/// <summary>
	/// Helper functions for working with PropertyInfo.
	/// </summary>
	/// <typeparam name="T">The type of the object whose PropertyInfo is being worked with.</typeparam>
	public class PropertyInfoHelper<T>
	{
		/// <summary>
		/// Gets an attribute value for a field that is decorated by the specified attribute type.
		/// </summary>
		/// <typeparam name="TValue">The type of the decorated field.</typeparam>
		/// <typeparam name="TAttribute">The type of the attribute.</typeparam>
		/// <typeparam name="TAttributeProperty">The type of the attribute property.</typeparam>
		/// <param name="propertySelector">The decorated property selector.</param>
		/// <param name="attributePropertySelector">The attribute property selector.</param>
		public TAttributeProperty GetAttributeValue<TValue, TAttribute, TAttributeProperty>(
			Expression<Func<T, TValue>> propertySelector,
			Expression<Func<TAttribute, TAttributeProperty>> attributePropertySelector)
		{
			var propertyInfo = GetProperty<TValue>(propertySelector);

			var propertyAttribs = propertyInfo.GetCustomAttributes(false);

			var attrib = propertyAttribs
				.OfType<TAttribute>()
				.FirstOrDefault();

			if (attrib != null)
			{
				var helper = new PropertyInfoHelper<TAttribute>();
				var value = helper.GetPropertyValue(attrib, attributePropertySelector);

				return value;
			}

			return default(TAttributeProperty);
		}

		/// <summary>
		/// Gets the PropertyInfo for the property identified by the given lambda expression.
		/// </summary>
		/// <typeparam name="TValue">The type of the value of the property to get.</typeparam>
		/// <param name="propertySelector">The property selector lamda expression.</param>
		/// <exception cref="System.InvalidOperationException">selector must be a field or property</exception>
		public PropertyInfo GetProperty<TValue>(Expression<Func<T, TValue>> propertySelector)
		{
			if (propertySelector == null)
				throw new ArgumentNullException("propertySelector");

			Expression body = propertySelector.Body;

			switch (body.NodeType)
			{
				case ExpressionType.MemberAccess:
					var objPropertyInfo = (PropertyInfo)((MemberExpression)body).Member;

					return objPropertyInfo;

				default:
					throw new InvalidOperationException("selector must be a field or property");
			}
		}

		/// <summary>
		/// Gets the value of the property identified by the given lambda expression.
		/// </summary>
		/// <typeparam name="TValue">The type of the value of the property to get.</typeparam>
		/// <param name="obj">The object whose property will be interrogated.</param>
		/// <param name="propertySelector">The property selector lambda expression.</param>
		/// <param name="index">Optional index values for indexed properties. This value should be null for non-indexed properties.</param>
		public TValue GetPropertyValue<TValue>(T obj, Expression<Func<T, TValue>> propertySelector, object[] index = null)
		{
			var objPropertyInfo = GetProperty<TValue>(propertySelector);

			var value = objPropertyInfo.GetValue(obj, index);

			return (TValue)value;
		}
	}
}
