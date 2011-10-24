using System;
using Snowball.Graphics;
using Snowball.Sound;

namespace Snowball.Content
{
	public interface IContentLoader
	{
		Texture LoadTexture(string key);

		TextureFont LoadTextureFont(string key);

		SpriteSheet LoadSpriteSheet(string key);

		SoundEffect LoadSoundEffect(string key);
	}
}
