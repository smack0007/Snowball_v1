using System;

namespace Snowball.Content
{
	public interface IContentLoader
	{
		T Load<T>(string key);
	}
}
