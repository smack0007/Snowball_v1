using System;

namespace Snowball.Content
{
	/// <summary>
	/// Interface for ContentLoader(s).
	/// </summary>
	public interface IContentLoader
	{
		/// <summary>
		/// Returns true if content has been registered under the given key.
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		bool IsRegistered<T>(string key);

		/// <summary>
		/// Loads a previously registered content.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="key"></param>
		/// <returns></returns>
		T Load<T>(string key);
		
		/// <summary>
		/// Loads content.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="args"></param>
		/// <returns></returns>
		T Load<T>(LoadContentArgs args);
	}
}
