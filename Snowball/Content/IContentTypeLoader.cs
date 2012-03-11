using System;
using Snowball.Storage;

namespace Snowball.Content
{
	/// <summary>
	/// Interface for content type loaders.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public interface IContentTypeLoader<T>
	{
		/// <summary>
		/// Registers content for loading later.
		/// </summary>
		/// <param name="key"></param>
		/// <param name="args"></param>
		void Register(string key, LoadContentArgs args);

		/// <summary>
		/// Returns true if content has been registered under the given key.
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		bool IsRegistered<T>(string key);

		/// <summary>
		/// Loads a previously registered content.
		/// </summary>
		/// <param name="storage"></param>
		/// <param name="key"></param>
		/// <returns></returns>
		T Load(IStorage storage, string key);

		/// <summary>
		/// Loads content.
		/// </summary>
		/// <param name="storage"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		T Load(IStorage storage, LoadContentArgs args);
	}
}
