using System;

namespace Snowball
{
	public static class IServiceProviderExtensions
	{
		/// <summary>
		/// Gets a service but throws an GameServiceNotFoundException if the given service is not available.
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		public static object GetRequiredGameService(this IServiceProvider services, Type type)
		{
			object provider = services.GetService(type);

			if (provider == null)
				throw new GameServiceNotFoundException(type);

			return provider;
		}
	}
}
