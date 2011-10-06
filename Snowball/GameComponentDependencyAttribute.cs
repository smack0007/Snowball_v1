using System;

namespace Snowball
{
	[AttributeUsage(AttributeTargets.Property)]
	public class GameComponentDependencyAttribute : Attribute
	{
		public GameComponentDependencyAttribute()
		{
		}
	}
}
