using System;
using NUnit.Framework;

namespace Snowball.Tools.Utilities.Tests
{
	public class CommandLineOptionsParserTests
	{
		class OptionsObjectWithBasicTypes
		{
			public string String;

			public int Integer;

			public float Float;

			public bool Boolean;
		}

		class OptionsObjectWith1RequiredStringOption
		{
			[CommandLineOptionRequired]
			public string String1;
		}

		class OptionsObjectWith2RequiredStringOptions
		{
			[CommandLineOptionRequired(1)]
			public string String2;

			[CommandLineOptionRequired(0)]
			public string String1;
		}

		class OptionsObjectWith2RequiredAnd1OptionalStringOptions
		{
			[CommandLineOptionRequired(0)]
			public string String1;

			[CommandLineOptionRequired(1)]
			public string String2;

			public string String3;
		}

		class OptionsObjectWithCustomNames
		{
			[CommandLineOptionName("Foo")]
			public string String1;

			[CommandLineOptionName("Bar")]
			public string String2;
		}

		[TestFixture]
		public class Parse
		{
			[Test]
			[ExpectedException(typeof(ArgumentNullException))]
			public void NullArgs_ThrowsArgumentNullException()
			{
				OptionsObjectWith1RequiredStringOption options;

				var parser = new CommandLineArgsParser<OptionsObjectWith1RequiredStringOption>();
				parser.Parse(null, out options);
			}

			[Test]
			public void NotEnoughArgsProvided_ReturnsFalse()
			{
				OptionsObjectWith1RequiredStringOption options;

				var parser = new CommandLineArgsParser<OptionsObjectWith1RequiredStringOption>();
				Assert.IsFalse(parser.Parse(new string[] { }, out options));
			}

			[Test]
			public void TooManyArgsProvided_ReturnsFalse()
			{
				OptionsObjectWith1RequiredStringOption options;

				var parser = new CommandLineArgsParser<OptionsObjectWith1RequiredStringOption>();
				Assert.IsFalse(parser.Parse(new string[] { "foo", "bar" }, out options));
			}

			[Test]
			public void With1RequiredStringOption_ExpectedOutput()
			{
				OptionsObjectWith1RequiredStringOption options;

				var parser = new CommandLineArgsParser<OptionsObjectWith1RequiredStringOption>();
				Assert.IsTrue(parser.Parse(new string[] { "foo" }, out options));

				Assert.IsNotNull(options);
				Assert.AreEqual("foo", options.String1);
			}

			[Test]
			public void With2RequiredStringOption_ExpectedOutput()
			{
				OptionsObjectWith2RequiredStringOptions options;

				var parser = new CommandLineArgsParser<OptionsObjectWith2RequiredStringOptions>();
				Assert.IsTrue(parser.Parse(new string[] { "foo", "bar" }, out options));

				Assert.IsNotNull(options);
				Assert.AreEqual("foo", options.String1);
				Assert.AreEqual("bar", options.String2);
			}

			[Test]
			public void With2RequiredAnd1OptionalStringOption_ExpectedOutput()
			{
				OptionsObjectWith2RequiredAnd1OptionalStringOptions options;

				var parser = new CommandLineArgsParser<OptionsObjectWith2RequiredAnd1OptionalStringOptions>();
				Assert.IsTrue(parser.Parse(new string[] { "foo", "bar", "/String3:abc" }, out options));

				Assert.IsNotNull(options);
				Assert.AreEqual("foo", options.String1);
				Assert.AreEqual("bar", options.String2);
				Assert.AreEqual("abc", options.String3);
			}

			[Test]
			public void With2RequiredAnd1OptionalStringOption_NoOptionalOptionsProvided_ExpectedOutput()
			{
				OptionsObjectWith2RequiredAnd1OptionalStringOptions options;

				var parser = new CommandLineArgsParser<OptionsObjectWith2RequiredAnd1OptionalStringOptions>();
				Assert.IsTrue(parser.Parse(new string[] { "foo", "bar" }, out options));

				Assert.IsNotNull(options);
				Assert.AreEqual("foo", options.String1);
				Assert.AreEqual("bar", options.String2);
				Assert.IsNull(options.String3);
			}

			[Test]
			public void BasicTypes_ExpectedOutput()
			{
				OptionsObjectWithBasicTypes options;

				var parser = new CommandLineArgsParser<OptionsObjectWithBasicTypes>();
				Assert.IsTrue(parser.Parse(new string[] { "/String:Hello", "/Integer:42", "/Float:123.4", "/Boolean" }, out options));

				Assert.IsNotNull(options);
				Assert.AreEqual("Hello", options.String);
				Assert.AreEqual(42, options.Integer);
				Assert.AreEqual(123.4f, options.Float);
				Assert.IsTrue(options.Boolean);
			}

			[Test]
			public void WithCustomNames_ExpectedOutput()
			{
				OptionsObjectWithCustomNames options;

				var parser = new CommandLineArgsParser<OptionsObjectWithCustomNames>();
				Assert.IsTrue(parser.Parse(new string[] { "/Foo:Hello", "/Bar:World!" }, out options));

				Assert.IsNotNull(options);
				Assert.AreEqual("Hello", options.String1);
				Assert.AreEqual("World!", options.String2);
			}
		}
	}
}
