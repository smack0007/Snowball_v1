using System;
using Snowball.Graphics;

namespace Snowball.Content
{
	public interface IContentLoader
	{
		Texture LoadTexture(string key);
	}
}
