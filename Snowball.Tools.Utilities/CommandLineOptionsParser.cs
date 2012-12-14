﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.ComponentModel;
using System.Collections;
using System.IO;
using System.Diagnostics;

namespace Snowball.Tools.Utilities
{    
	public class CommandLineOptionsParser<T> : ICommandLineOptionsParser
        where T : class, new()
	{
        private const string ParameterIndicator = "/";
		private static readonly char[] ParameterSeperator = new char[] { ':' };
		
		readonly Dictionary<string, FieldInfo> requiredOptionsMap;
        readonly Dictionary<string, FieldInfo> optionalOptionsMap;
        readonly Dictionary<string, string> usageMap;

		public CommandLineOptionsParser()
		{			
			Type type = typeof(T);
			
			this.requiredOptionsMap = new Dictionary<string, FieldInfo>();
			this.optionalOptionsMap = new Dictionary<string, FieldInfo>();
			this.usageMap = new Dictionary<string, string>();

			var requiredOptionsSortMap = new Dictionary<int, KeyValuePair<string, FieldInfo>>(); 

			foreach (FieldInfo fieldInfo in type.GetFields())
			{
				bool isRequired = false;
				int index = -1;
				string name = null;
				string description = null;

				foreach (object attribute in fieldInfo.GetCustomAttributes(false))
				{
					if (attribute is CommandLineOptionRequiredAttribute)
					{
						isRequired = true;
						index = ((CommandLineOptionRequiredAttribute)attribute).Index;					
					}
					else if (attribute is CommandLineOptionNameAttribute)
					{
						name = ((CommandLineOptionNameAttribute)attribute).Name;
					}
					else if (attribute is CommandLineOptionDescriptionAttribute)
					{
						description = ((CommandLineOptionDescriptionAttribute)attribute).Description;
					}
				}

				if (name == null)
					name = fieldInfo.Name;

				if (isRequired)
				{
					requiredOptionsSortMap.Add(index, new KeyValuePair<string, FieldInfo>(name, fieldInfo));
				}
				else
				{
					this.optionalOptionsMap.Add(name, fieldInfo);
				}

				this.usageMap.Add(name, description);
			}

			List<int> requiredOptionsKeys = requiredOptionsSortMap.Keys.ToList();
			requiredOptionsKeys.Sort();

			this.requiredOptionsMap = new Dictionary<string, FieldInfo>();

			foreach (int index in requiredOptionsKeys)
				this.requiredOptionsMap.Add(requiredOptionsSortMap[index].Key, requiredOptionsSortMap[index].Value);			
		}

        public bool Parse(string[] args, out object optionsObject, Action<string> onError)
        {
            T typedOptions;
            bool result = this.Parse(args, out typedOptions, onError);

            optionsObject = typedOptions;
            return result;
        }

		public bool Parse(string[] args, out T optionsObject, Action<string> onError)
		{
			if (args == null)
				throw new ArgumentNullException("args");

            if (onError == null)
                throw new ArgumentNullException("onError");

			optionsObject = new T();
			bool success = true;

			Queue<FieldInfo> requiredOptions = new Queue<FieldInfo>();

			foreach (FieldInfo fieldInfo in this.requiredOptionsMap.Values)
				requiredOptions.Enqueue(fieldInfo);

			foreach (string arg in args)
			{
				if (arg.StartsWith(ParameterIndicator))
				{
                    string[] parts = arg.Substring(ParameterIndicator.Length).Split(ParameterSeperator, 2, StringSplitOptions.None);

					string name = null;
					string value = null;

					if (parts.Length == 2)
					{
						name = parts[0];
						value = parts[1];
					}
					else if (parts.Length == 1)
					{
						name = parts[0];
						value = "true";
					}
					else
					{
						onError(string.Format("Failure while parsing '{0}'.", arg));
						return false;
					}

					if (this.optionalOptionsMap.ContainsKey(name))
					{
						if (!this.SetOption(optionsObject, this.optionalOptionsMap[name], value, onError))
							return false;
					}
					else
					{
						onError(string.Format("Unkown option '{0}'.", parts[0]));
						return false;
					}
				}
				else
				{					
					if (requiredOptions.Count == 0)
					{
						onError("Too many arguments provided.");
						return false;
					}

					FieldInfo field = requiredOptions.Peek();

					if (!IsList(field))
					{
						requiredOptions.Dequeue();
					}

					if (!this.SetOption(optionsObject, field, arg, onError))
						return false;
				}
			}

			if (requiredOptions.Count > 0)
			{
				onError("Not enough arguments provided.");
				return false;
			}

			return success;
		}

		private bool SetOption(T optionsObject, FieldInfo fieldInfo, string value, Action<string> onError)
		{
			try
			{
				if (IsList(fieldInfo))
				{
					// Append this value to a list of options.
					GetList(optionsObject, fieldInfo).Add(ChangeType(value, ListElementType(fieldInfo)));
				}
				else
				{
					// Set the value of a single option.
					fieldInfo.SetValue(optionsObject, ChangeType(value, fieldInfo.FieldType));
				}

				return true;
			}
			catch
			{
				onError(string.Format("Invalid value '{0}' for option '{1}'", value, GetOptionName(fieldInfo)));
				return false;
			}
		}

		private static T GetAttribute<T>(ICustomAttributeProvider provider) where T : Attribute
		{
			return provider.GetCustomAttributes(typeof(T), false).OfType<T>().FirstOrDefault();
		}

		private static string GetOptionName(FieldInfo field)
		{
			var nameAttribute = GetAttribute<CommandLineOptionNameAttribute>(field);

			if (nameAttribute != null)
			{
				return nameAttribute.Name;
			}
			else
			{
				return field.Name;
			}
		}

		private static object ChangeType(string value, Type type)
		{
			TypeConverter converter = TypeDescriptor.GetConverter(type);
			return converter.ConvertFromInvariantString(value);
		}
		
		private static bool IsList(FieldInfo field)
		{
			return typeof(IList).IsAssignableFrom(field.FieldType);
		}
		
		private static IList GetList(T optionsObject, FieldInfo field)
		{
			return (IList)field.GetValue(optionsObject);
		}
		
		private static Type ListElementType(FieldInfo field)
		{
			var interfaces = from i in field.FieldType.GetInterfaces()
							 where i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEnumerable<>)
							 select i;

			return interfaces.First().GetGenericArguments()[0];
		}

        public void WriteUsage(TextWriter textWriter, string name)
        {
            if (textWriter == null)
                throw new ArgumentNullException("textWriter");

            textWriter.WriteLine("Usage: {0} {1}", name, string.Join(" ", this.requiredOptionsMap.Keys));
        }

		public void WriteOptions(TextWriter textWriter)
		{
            if (textWriter == null)
                throw new ArgumentNullException("textWriter");
            				
			textWriter.WriteLine("Options:");

			foreach (KeyValuePair<string, string> usage in this.usageMap)
			{
				textWriter.WriteLine("  {0}: {1}", usage.Key, usage.Value);
			}
		}
	}
}
