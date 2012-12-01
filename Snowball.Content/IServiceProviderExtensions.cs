using System;

namespace Snowball.Content
{
	public static class IServiceProviderExtensions
	{
		/// <summary>
		/// Gets a service and automatically casts to the given type.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="services"></param>
		/// <returns></returns>
		public static T GetService<T>(this IServiceProvider services)
		{
			return (T)services.GetService(typeof(T));
		}

		/// <summary>
		/// Gets a service provider but throws an GameServiceNotFoundException if the given service is not available.
		/// </summary>
		/// <param name="services"></param>
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
