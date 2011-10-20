﻿using System;
using Snowball.Graphics;

namespace Snowball.Content
{
	public interface IContentLoader
	{
		Texture LoadTexture(string key);

		TextureFont LoadTextureFont(string key);

		SpriteSheet LoadSpriteSheet(string key);
	}
}
