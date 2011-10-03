using System;

namespace Snowball
{
	/// <summary>
	/// Interface for service providers in a game.
	/// </summary>
	public interface IGameServicesContainer : IServiceProvider
	{
		/// <summary>
		/// Gets a service provider but throws an GameServiceNotFoundException if the given service is not available.
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		object GetRequiredService(Type type);
	}
}
