using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Snowball.Tools.Utilities
{
	[AttributeUsage(AttributeTargets.Field)]
	public class CommandLineOptionRequiredAttribute : Attribute
	{
		public int Index
		{
			get;
			set;
		}

		public CommandLineOptionRequiredAttribute()
			: base()
		{
		}

		public CommandLineOptionRequiredAttribute(int index)
			: base()
		{
			this.Index = index;
		}
	}
}
