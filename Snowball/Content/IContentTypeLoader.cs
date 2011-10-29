using System;
using Snowball.Storage;

namespace Snowball.Content
{
	public interface IContentTypeLoader<T>
	{
		void Register(string key, LoadContentArgs args);

		T Load(IStorage storage, string key);

		T Load(IStorage storage, LoadContentArgs args);
	}
}
