using System;

namespace Netricity.Common
{
	/// <summary>
	/// A generic, serializable key-value pair.
	/// </summary>
	/// <typeparam name="K">The type of the Key.</typeparam>
	/// <typeparam name="V">The type of the Value.</typeparam>
	[Serializable]
	public class SerializableKeyValuePair<K, V>
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="SerializableKeyValuePair`2" /> struct.
		/// Parameterless ctor required for Serializable.
		/// </summary>
		public SerializableKeyValuePair()
		{

		}

		/// <summary>
		/// Initializes a new instance of the <see cref="SerializableKeyValuePair{V}" /> class.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="value">The value.</param>
		public SerializableKeyValuePair(K key, V value)
		{
			this.Key = key;
			this.Value = value;
		}

		/// <summary>
		/// Gets or sets the key.
		/// </summary>
		public K Key { get; set; }

		/// <summary>
		/// Gets or sets the value.
		/// </summary>
		public V Value { get; set; }
	}
}