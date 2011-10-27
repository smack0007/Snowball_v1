using System;

namespace Snowball.Content
{
	public interface IContentTypeLoader<T>
	{
		void Register(string key, LoadContentArgs args);

		T Load(IContentStorageSystem storage, string key);
	}
}
