using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Snowball.Content
{
	public interface IContentManager<TKey>
	{
		/// <summary>
		/// Returns true if content has been registered under the given key.
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		bool IsRegistered(TKey key);

		/// <summary>
		/// Loads registered content.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="key"></param>
		/// <returns></returns>
		T Load<T>(TKey key)
			where T : class;

		/// <summary>
		/// Unloads registered content if it is loaded.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="key"></param>
		void Unload<T>(TKey key)
			where T : class;

		/// <summary>
		/// Unloads all registered content if it is loaded. 
		/// </summary>
		void UnloadAll();
	}
}
