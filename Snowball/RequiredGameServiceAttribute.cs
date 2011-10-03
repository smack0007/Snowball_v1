using System;

namespace Snowball
{
	[AttributeUsage(AttributeTargets.Property)]
	public class RequiredGameServiceAttribute : Attribute
	{
		public RequiredGameServiceAttribute()
		{
		}
	}
}
