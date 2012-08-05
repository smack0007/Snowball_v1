using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Snowball
{
	internal static class XmlTextReaderExtensions
	{
		private static void EnsureTypeIsParsable<T>()
		{
			Type type = typeof(T);

			if (!type.IsPrimitive)
				throw new InvalidOperationException("T must be a primitive type or a string.");
		}

		private static bool Parse<T>(string input, out object output)
		{
			Type type = typeof(T);

			if (type == typeof(int))
			{
				int temp;
				if (int.TryParse(input, out temp))
				{
					output = temp;
					return true;
				}
			}
			else if (type == typeof(float))
			{
				float temp;
				if (float.TryParse(input, out temp))
				{
					output = temp;
					return true;
				}
			}

			output = null;
			return false;
		}

		/// <summary>
		/// Reads an attribute value if it exists or returns a default value otherwise.
		/// </summary>
		/// <param name="xml"></param>
		/// <param name="attributeName"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		internal static string ReadAttributeValueOrDefault(this XmlTextReader xml, string attributeName, string defaultValue)
		{
			if (string.IsNullOrEmpty(attributeName))
				throw new ArgumentNullException("attributeName");
						
			string attributeValue = xml.GetAttribute(attributeName);

			if (attributeValue == null)
				return defaultValue;
			
			return attributeValue;
		}

		/// <summary>
		/// Reads an attribute value and parses it if it exists or returns a default value otherwise.
		/// </summary>
		/// <param name="xml"></param>
		/// <param name="attributeName"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		internal static T ReadAttributeValueOrDefault<T>(this XmlTextReader xml, string attributeName, T defaultValue) where T : struct
		{
			if (string.IsNullOrEmpty(attributeName))
				throw new ArgumentNullException("attributeName");

			EnsureTypeIsParsable<T>();

			string attributeValue = xml.GetAttribute(attributeName);

			if (attributeValue == null)
				return defaultValue;

			object parsedValue;
			if (Parse<T>(attributeValue, out parsedValue))
				return (T)parsedValue;

			return defaultValue;
		}

		private static void ThrowAttributeNotFoundException(XmlTextReader xml, string attributeName)
		{
			throw new XmlException("The required attribute \"" + attributeName + "\" is missing from the element \"" + xml.Name + "\".");
		}

		/// <summary>
		/// Reads an attribute value if it exists or returns a default value otherwise.
		/// </summary>
		/// <param name="xml"></param>
		/// <param name="attributeName"></param>
		/// <returns></returns>
		internal static string ReadRequiredAttributeValue(this XmlTextReader xml, string attributeName)
		{
			if (string.IsNullOrEmpty(attributeName))
				throw new ArgumentNullException("attributeName");

			string attributeValue = xml.GetAttribute(attributeName);

			if (attributeValue == null)
				ThrowAttributeNotFoundException(xml, attributeName);

			return attributeValue;
		}

		/// <summary>
		/// Reads an attribute value if it exists or returns a default value otherwise.
		/// </summary>
		/// <param name="xml"></param>
		/// <param name="attributeName"></param>
		/// <returns></returns>
		internal static T ReadRequiredAttributeValue<T>(this XmlTextReader xml, string attributeName) where T : struct
		{
			if (string.IsNullOrEmpty(attributeName))
				throw new ArgumentNullException("attributeName");

			string attributeValue = xml.GetAttribute(attributeName);

			if (attributeValue == null)
				ThrowAttributeNotFoundException(xml, attributeName);

			object parsedValue;
			if (Parse<T>(attributeValue, out parsedValue))
				return (T)parsedValue;

			throw new XmlException("The required attribute \"" + attributeName + "\" from the element \"" + xml.Name + "\" could not be parsed as \"" + typeof(T) + "\".");
		}
	}
}
