using System;

namespace Snowball.Content
{
	/// <summary>
	/// Args for loading TextureFont(s).
	/// </summary>
	public class LoadTextureFontArgs : LoadTextureArgs
	{
		public override bool RequiresStream
		{
			get { return this.LoadType == ContentLoadType.FromFile; }
		}

		/// <summary>
		/// Specifies how the TextureFont should be loaded.
		/// </summary>
		public ContentLoadType LoadType
		{
			get;
			set;
		}

		/// <summary>
		/// Specifies the name of the font. Only used if LoadType is set to Construct.
		/// </summary>
		public string FontName
		{
			get;
			set;
		}

		/// <summary>
		/// Specifies the size of the font. Only used if LoadType is set to Construct.
		/// </summary>
		public int FontSize
		{
			get;
			set;
		}

		/// <summary>
		/// Specifies if antialiasing should be used in the font. Only used if LoadType is set to Construct.
		/// </summary>
		public bool Antialias
		{
			get;
			set;
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		public LoadTextureFontArgs()
			: base()
		{
			this.Antialias = true;
		}
	}
}
