using System;
using System.IO;
using System.Collections.Generic;

namespace Snowball.Graphics
{
	public sealed partial class GraphicsDevice : DisposableObject, IGraphicsDevice
	{			
        IHostControl host;
        
		/// <summary>
		/// The width of the display area.
		/// </summary>
		public int BackBufferWidth
		{
			get;
			private set;
		}

		/// <summary>
		/// The height of the display area.
		/// </summary>
		public int BackBufferHeight
		{
			get;
			private set;
		}

		/// <summary>
		/// Indicates if a draw is currently in progress.
		/// </summary>
		public bool HasDrawBegun
		{
			get;
			private set;
		}

		/// <summary>
		/// The Texture which is currently being drawn to as a render target. If null, the backbuffer is being drawn to.
		/// </summary>
		public Texture RenderTarget
		{
			get;
			private set;
		}
		
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="host"></param>
		/// <param name="fullscreen"></param>
		public GraphicsDevice(IHostControl host, bool fullscreen)
		{
			if (host == null)
				throw new ArgumentNullException("host");

			this.host = host;

			this.Construct(host, fullscreen, host.DisplayWidth, host.DisplayHeight);
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		public GraphicsDevice(IHostControl host,  bool fullscreen, int backBufferWidth, int backBufferHeight)
		{
			if (host == null)
				throw new ArgumentNullException("host");

			this.host = host;

			this.Construct(host, fullscreen, backBufferWidth, backBufferHeight);
		}	
						
		/// <summary>
		/// Requests to begin drawing.
		/// </summary>
		public bool BeginDraw()
		{
			if (this.HasDrawBegun)
				throw new InvalidOperationException("Already within BeginDraw / EndDraw pair.");

			if (!this.BeginDrawInternal())
				return false;

			this.HasDrawBegun = true;

			return true;
		}

		/// <summary>
		/// Requests to begin drawing.
		/// </summary>
		/// <param name="renderTarget"></param>
		/// <returns></returns>
		public bool BeginDraw(Texture renderTarget)
		{
			if (renderTarget == null)
				throw new ArgumentNullException("renderTarget");

			if (renderTarget.Usage != TextureUsage.RenderTarget)
				throw new InvalidOperationException("Texture has not been created with RenderTarget usage specified.");

			this.RenderTarget = renderTarget;

			return this.BeginDraw();
		}

		/// <summary>
		/// Informs the manager drawing is ending.
		/// </summary>
		public void EndDraw()
		{
			if (!this.HasDrawBegun)
				throw new InvalidOperationException("Not within BeginDraw / EndDraw pair.");

			this.EndDrawInternal();

			this.HasDrawBegun = false;
		}

		/// <summary>
		/// Clears the back buffer by filling it with the given color.
		/// </summary>
		/// <param name="color"></param>
		public void Clear(Color color)
		{
			this.ClearInternal(color);
		}

		private void EnsureCanPresent()
		{
			if (this.HasDrawBegun)
				throw new InvalidOperationException("Present should not be called within BeginDraw / EndDraw pair.");
		}

		/// <summary>
		/// Presents the back buffer.
		/// </summary>
		public void Present()
		{
			this.EnsureCanPresent();
			this.PresentInternal();			
		}

		/// <summary>
		/// Presents the back buffer.
		/// </summary>
		/// <param name="source">The source rectangle of the back buffer to present.</param>
		/// <param name="destination">The destination rectangle to present the back buffer into.</param>
		public void Present(Rectangle source, Rectangle destination)
		{
			this.EnsureCanPresent();
			this.PresentInternal(source, destination, this.host.Handle);
		}

		/// <summary>
		/// Presents the back buffer.
		/// </summary>
		/// <param name="source">The source rectangle of the back buffer to present.</param>
		/// <param name="destination">The destination rectangle to present the back buffer into.</param>
		/// <param name="window">The window to present the back buffer to.</param>
		public void Present(Rectangle source, Rectangle destination, IntPtr window)
		{
			this.EnsureCanPresent();
			this.PresentInternal(source, destination, window);
		}
				
		/// <summary>
		/// Loads an Effect.
		/// </summary>
		/// <param name="stream"></param>
		/// <returns></returns>
		public Effect LoadEffect(Stream stream)
		{
			return Effect.FromStream(this, stream);
		}

		/// <summary>
		/// Loads a Texture.
		/// </summary>
		/// <param name="stream">The stream to load from.</param>
		/// <param name="colorKey">A color which should be used for transparency.</param>
		/// <returns></returns>
		public Texture LoadTexture(Stream stream, Color? colorKey)
		{
			return Texture.FromStream(this, stream, colorKey);			
		}

        /// <summary>
        /// Loads a SpriteSheet from an XML file.
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="loadTextureFunc"></param>
        /// <returns></returns>
        public SpriteSheet LoadSpriteSheet(Stream stream, Func<string, Color?, Texture> loadTextureFunc)
        {
            return SpriteSheet.FromStream(this, stream, loadTextureFunc);
        }
		
		/// <summary>
		/// Loads a TextureFont from an XML file.
		/// </summary>
		/// <param name="stream"></param>
		/// <param name="loadTextureFunc"></param>
		/// <returns></returns>
		public TextureFont LoadTextureFont(Stream stream, Func<string, Color?, Texture> loadTextureFunc)
		{
			return TextureFont.FromStream(this, stream, loadTextureFunc);
		}
	}
}
