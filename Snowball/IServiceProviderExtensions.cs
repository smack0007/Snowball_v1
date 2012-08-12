using System;

namespace Snowball
{
	public static class IServiceProviderExtensions
	{
		public static T GetService<T>(this IServiceProvider services)
		{
			return (T)services.GetService(typeof(T));
		}

		/// <summary>
		/// Gets a service but throws an GameServiceNotFoundException if the given service is not available.
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		public static T GetRequiredService<T>(this IServiceProvider services)
		{
			T provider = services.GetService<T>();

			if (provider == null)
				throw new ServiceProviderNotFoundException(typeof(T));

			return provider;
		}
	}
}
