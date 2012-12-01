using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Snowball.GameFramework
{
	/// <summary>
	/// Container for service providers.
	/// </summary>
	public class GameServicesContainer : IServiceProvider
	{
		Dictionary<Type, object> services;

		/// <summary>
		/// Constructor.
		/// </summary>
		public GameServicesContainer()
		{
			this.services = new Dictionary<Type, object>();
		}

		/// <summary>
		/// Registers a service provider.
		/// </summary>
		/// <param name="type"></param>
		/// <param name="provider"></param>
		public void AddService(Type type, object provider)
		{
			if (type == null)
				throw new ArgumentNullException("type");

			if (provider == null)
				throw new ArgumentNullException("provider");

			if (this.services.ContainsKey(type))
				throw new InvalidOperationException("A provider is already registered the type " + type);

			var providerType = provider.GetType();

			if (!type.IsAssignableFrom(providerType))
				throw new InvalidOperationException(providerType + " is not an instance of " + type);

			this.services.Add(type, provider);
		}

		/// <summary>
		/// Returns a registered service provider.
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		public object GetService(Type type)
		{
			if (type == null)
				throw new ArgumentNullException("type");

			if (this.services.ContainsKey(type))
				return this.services[type];

			return null;
		}
		
		/// <summary>
		/// Unregisters a service provider.
		/// </summary>
		/// <param name="type"></param>
		public void RemoveService(Type type)
		{
			if (null == type)
				throw new ArgumentNullException("type");

			this.services.Remove(type);
		}
	}
}
