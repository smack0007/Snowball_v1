using System;
using System.IO;
using System.Reflection;

namespace Snowball.Graphics
{
	public class BasicTextureFont : ITextureFont
	{
		TextureFont textureFont;

		public Texture Texture
		{
			get { return this.textureFont.Texture; }
		}

		public int LineHeight
		{
			get { return this.textureFont.LineHeight; }
		}

		public int CharacterSpacing
		{
			get { return this.textureFont.CharacterSpacing; }
		}

		public Rectangle this[char ch]
		{
			get { return this.textureFont[ch]; }
		}

		public BasicTextureFont(GraphicsDevice graphicsDevice)
		{
			if (graphicsDevice == null)
				throw new ArgumentNullException("graphicsDevice");

			graphicsDevice.EnsureDeviceCreated();

			Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(@"Snowball.Graphics.BasicTextureFont.xml");

			if (stream == null)
				throw new FileNotFoundException("Failed to load BasicTextureFont.xml.");

			this.textureFont = TextureFont.FromStream(graphicsDevice, stream, (fileName, colorKey) =>
			{
				Stream textureStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(@"Snowball.Graphics.BasicTextureFont.png");

				if (textureStream == null)
					throw new FileNotFoundException("Failed to load BasicTextureFont.png.");

				return Texture.FromStream(graphicsDevice, textureStream, colorKey);
			});
		}

		public bool ContainsCharacter(char ch)
		{
			return this.textureFont.ContainsCharacter(ch);
		}

		public Vector2 MeasureString(string s)
		{
			return this.textureFont.MeasureString(s);
		}

		public Vector2 MeasureString(string s, int start, int length)
		{
			return this.MeasureString(s, start, length);
		}
	}
}
