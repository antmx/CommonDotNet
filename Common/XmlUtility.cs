using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Netricity.Common
{
	public class XmlUtility
	{
		/// <summary>
		/// Serialises an object to an XML string.
		/// <example>
		/// <code>
		/// obj.XmlField = XmlUtil.Serialize<type>(instance);
		/// </code>
		/// </example>
		/// </summary>
		/// <typeparam name="T">The type of the object to serialise.</typeparam>
		/// <param name="value">The object to serialise.</param>
		public static string Serialize<T>(T value)
		{
			if (value == null)
			{
				return null;
			}
			var serializer = new XmlSerializer(typeof(T));
			var settings = new XmlWriterSettings();
			settings.Encoding = new UnicodeEncoding(false, false);
			settings.Indent = false;
			settings.OmitXmlDeclaration = false;
			using (var textWriter = new StringWriter())
			{
				using (XmlWriter xmlWriter = XmlWriter.Create(textWriter, settings))
				{
					serializer.Serialize(xmlWriter, value);
				}
				return textWriter.ToString();
			}
		}

		/// <summary>
		/// Deserialises an XML string to an object of the given type.
		/// </summary>
		/// <typeparam name="T">The type of the object to deserialise.</typeparam>
		/// <param name="xml">The XML containing the serialised object.</param>
		public static T Deserialize<T>(string xml)
		{
			if (String.IsNullOrEmpty(xml))
			{
				return default(T);
			}
			XmlSerializer serializer = new XmlSerializer(typeof(T));
			XmlReaderSettings settings = new XmlReaderSettings();
			using (StringReader textReader = new StringReader(xml))
			{
				using (XmlReader xmlReader = XmlReader.Create(textReader, settings))
				{
					return (T)serializer.Deserialize(xmlReader);
				}
			}
		}
	}
}
