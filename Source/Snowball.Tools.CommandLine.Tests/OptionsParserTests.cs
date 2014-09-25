using System;
using NUnit.Framework;
using Snowball.Tools.Commands;

namespace Snowball.Tools.CommandLine.Tests
{
	public class OptionsParserTests
	{

#pragma warning disable 649
		class OptionsObjectWithBasicTypes
		{
			public string String;

			public int Integer;

			public float Float;

			public bool Boolean;
		}

		class OptionsObjectWith1RequiredStringOption
		{
			[OptionRequired]
			public string String1;
		}

		class OptionsObjectWith2RequiredStringOptions
		{
			[OptionRequired(1)]
			public string String2;

			[OptionRequired(0)]
			public string String1;
		}

		class OptionsObjectWith2RequiredAnd1OptionalStringOptions
		{
			[OptionRequired(0)]
			public string String1;

			[OptionRequired(1)]
			public string String2;

			public string String3;
		}

		class OptionsObjectWithCustomNames
		{
			[OptionName("Foo")]
			public string String1;

			[OptionName("Bar")]
			public string String2;
		}
#pragma warning restore 649

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void Parse_NullArgs_ThrowsArgumentNullException()
		{
            object output;

			var parser = new OptionsParser(typeof(OptionsObjectWith1RequiredStringOption));
            parser.Parse(null, out output, new CommandLineLogger());
		}

		[Test]
        public void Parse_NotEnoughArgsProvided_ReturnsFalse()
		{
            object output;

			var parser = new OptionsParser(typeof(OptionsObjectWith1RequiredStringOption));
            Assert.IsFalse(parser.Parse(new string[] { }, out output, new CommandLineLogger()));
		}

		[Test]
        public void Parse_TooManyArgsProvided_ReturnsFalse()
		{
			object output;

			var parser = new OptionsParser(typeof(OptionsObjectWith1RequiredStringOption));
            Assert.IsFalse(parser.Parse(new string[] { "foo", "bar" }, out output, new CommandLineLogger()));
		}

		[Test]
		public void With1RequiredStringOption_ExpectedOutput()
		{
            object output;

			var parser = new OptionsParser(typeof(OptionsObjectWith1RequiredStringOption));
            Assert.IsTrue(parser.Parse(new string[] { "foo" }, out output, new CommandLineLogger()));

            OptionsObjectWith1RequiredStringOption options = (OptionsObjectWith1RequiredStringOption)output;

			Assert.IsNotNull(options);
			Assert.AreEqual("foo", options.String1);
		}

		[Test]
		public void With2RequiredStringOption_ExpectedOutput()
		{
            object output;

			var parser = new OptionsParser(typeof(OptionsObjectWith2RequiredStringOptions));
            Assert.IsTrue(parser.Parse(new string[] { "foo", "bar" }, out output, new CommandLineLogger()));

            OptionsObjectWith2RequiredStringOptions options = (OptionsObjectWith2RequiredStringOptions)output;

			Assert.IsNotNull(options);
			Assert.AreEqual("foo", options.String1);
			Assert.AreEqual("bar", options.String2);
		}

		[Test]
		public void With2RequiredAnd1OptionalStringOption_ExpectedOutput()
		{
			object output;

			var parser = new OptionsParser(typeof(OptionsObjectWith2RequiredAnd1OptionalStringOptions));
            Assert.IsTrue(parser.Parse(new string[] { "foo", "bar", "/String3:abc" }, out output, new CommandLineLogger()));

            OptionsObjectWith2RequiredAnd1OptionalStringOptions options = (OptionsObjectWith2RequiredAnd1OptionalStringOptions)output;

			Assert.IsNotNull(options);
			Assert.AreEqual("foo", options.String1);
			Assert.AreEqual("bar", options.String2);
			Assert.AreEqual("abc", options.String3);
		}

		[Test]
		public void With2RequiredAnd1OptionalStringOption_NoOptionalOptionsProvided_ExpectedOutput()
		{
            object output;

			var parser = new OptionsParser(typeof(OptionsObjectWith2RequiredAnd1OptionalStringOptions));
            Assert.IsTrue(parser.Parse(new string[] { "foo", "bar" }, out output, new CommandLineLogger()));

            OptionsObjectWith2RequiredAnd1OptionalStringOptions options = (OptionsObjectWith2RequiredAnd1OptionalStringOptions)output;

			Assert.IsNotNull(options);
			Assert.AreEqual("foo", options.String1);
			Assert.AreEqual("bar", options.String2);
			Assert.IsNull(options.String3);
		}

		[Test]
		public void BasicTypes_ExpectedOutput()
		{
            object output;
			
			var parser = new OptionsParser(typeof(OptionsObjectWithBasicTypes));
            Assert.IsTrue(parser.Parse(new string[] { "/String:Hello", "/Integer:42", "/Float:123.4", "/Boolean" }, out output, new CommandLineLogger()));

            OptionsObjectWithBasicTypes options = (OptionsObjectWithBasicTypes)output;

			Assert.IsNotNull(options);
			Assert.AreEqual("Hello", options.String);
			Assert.AreEqual(42, options.Integer);
			Assert.AreEqual(123.4f, options.Float);
			Assert.IsTrue(options.Boolean);
		}

		[Test]
		public void WithCustomNames_ExpectedOutput()
		{
            object output;
			
			var parser = new OptionsParser(typeof(OptionsObjectWithCustomNames));
            Assert.IsTrue(parser.Parse(new string[] { "/Foo:Hello", "/Bar:World!" }, out output, new CommandLineLogger()));

            OptionsObjectWithCustomNames options = (OptionsObjectWithCustomNames)output;

			Assert.IsNotNull(options);
			Assert.AreEqual("Hello", options.String1);
			Assert.AreEqual("World!", options.String2);
		}
	}
}
