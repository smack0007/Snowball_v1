using System;

namespace Snowball
{
	/// <summary>
	/// Exception thrown when a required service provider for a game is not found.
	/// </summary>
	public class ServiceProviderNotFoundException : InvalidOperationException
	{
		/// <summary>
		/// The type of service provider which could not be found.
		/// </summary>
		public Type Type
		{
			get;
			private set;
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="type"></param>
		public ServiceProviderNotFoundException(Type type)
			: base("The required service (" + type + ") was not found.")
		{
			if (type == null)
				throw new ArgumentNullException("type");

			this.Type = type;
		}
	}
}
