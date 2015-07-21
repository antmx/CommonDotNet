using System;

namespace Netricity.Common
{
	/// <summary>
	/// System.Type extension methods.
	/// </summary>
	public static class TypeExtensions
	{
		/// <summary>
		/// Returns the underlying/base type of nullable types.
		/// </summary>
		/// <param name="type">The type.</param>
		public static Type GetBaseType(this Type type)
		{
			// If the passed Type is valid, .IsValueType and is logicially nullable, .Get(its)UnderlyingType
			if (type != null && type.IsValueType && type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
			{
				return Nullable.GetUnderlyingType(type);
			}
			else
			{
				// The passed Type was null or was not logicially nullable, so simply return the passed Type
				return type;
			}
		}
	}
}
