using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Snowball.Tools.Commands
{
	[AttributeUsage(AttributeTargets.Field)]
	public class OptionRequiredAttribute : Attribute
	{
		public int Index
		{
			get;
			set;
		}

		public OptionRequiredAttribute()
			: base()
		{
		}

		public OptionRequiredAttribute(int index)
			: base()
		{
			this.Index = index;
		}
	}
}
