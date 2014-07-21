using System;
using System.IO;
using System.Collections.Generic;

using D3D = SharpDX.Direct3D9;

namespace Snowball.Graphics
{
	public sealed class GraphicsDevice : IGraphicsDevice, IDisposable
	{
		internal D3D.Device InternalDevice;

		D3D.PresentParameters? presentParams;
		D3D.Capabilities? capabilities;
		
        IHostControl host;
        bool isDeviceLost;

		D3D.Surface backBuffer;
		
		/// <summary>
		/// Whether or not the GraphicsDevice is lost. If the device is lost,
		/// the BeginDraw() method will fail.
		/// </summary>
		internal bool IsDeviceLost
		{
			get { return this.isDeviceLost; }

			set
			{
				if (value != this.isDeviceLost)
				{
					this.isDeviceLost = value;

					if (this.isDeviceLost && this.DeviceLost != null)
						this.DeviceLost(this, EventArgs.Empty);
				}
			}
		}

		/// <summary>
		/// The width of the display area.
		/// </summary>
		public int BackBufferWidth
		{
			get
			{
				if (this.presentParams != null)
					return this.presentParams.Value.BackBufferWidth;

				return -1;
			}
		}

		/// <summary>
		/// The height of the display area.
		/// </summary>
		public int BackBufferHeight
		{
			get
			{
				if (this.presentParams != null)
					return this.presentParams.Value.BackBufferHeight;

				return -1;
			}
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

		internal bool TexturesMustBePowerOf2
		{
			get
			{
				if (this.capabilities != null)
				{
					if (this.capabilities.Value.TextureCaps.HasFlag(D3D.TextureCaps.Pow2))
					{
						if (this.capabilities.Value.TextureCaps.HasFlag(D3D.TextureCaps.NonPow2Conditional))
						{
							return false;
						}
						else
						{
							return true;
						}
					}
					else
					{
						return false;
					}
				}

				return true; // Assume true.
			}
		}

		internal bool TexturesMustBeSquare
		{
			get
			{
				if (this.capabilities != null)
				{
					return this.capabilities.Value.TextureCaps.HasFlag(D3D.TextureCaps.SquareOnly);
				}

				return true; // Assume true.
			}
		}
				
		/// <summary>
		/// Triggered when the GraphicsDevice has been reset. 
		/// </summary>
		internal event EventHandler DeviceReset;

		/// <summary>
		/// Triggered when the GraphicsDevice is lost.
		/// </summary>
		internal event EventHandler DeviceLost;
				
		/// <summary>
		/// Constructor.
		/// </summary>
		public GraphicsDevice(IHostControl host, int backBufferWidth, int backBufferHeight, bool fullscreen)
		{
			if (host == null)
				throw new ArgumentNullException("host");

			this.host = host;

			D3D.Direct3D direct3d = new D3D.Direct3D();

			this.presentParams = new D3D.PresentParameters(backBufferWidth, backBufferHeight) 
			{
				DeviceWindowHandle = host.Handle,
				Windowed = !fullscreen
			};

			if (fullscreen) // Only check display mode if we're going fullscreen.
			{
				D3D.DisplayModeCollection availableDisplayModes = direct3d.Adapters[0].GetDisplayModes(D3D.Format.X8R8G8B8);
				D3D.DisplayMode? displayMode = null;

				foreach (D3D.DisplayMode availableDisplayMode in availableDisplayModes)
				{
					if (availableDisplayMode.Width == this.presentParams.Value.BackBufferWidth && availableDisplayMode.Height == this.presentParams.Value.BackBufferHeight)
					{
						displayMode = availableDisplayMode;
						break;
					}
				}

				if (displayMode == null)
					throw new GraphicsException("The given display mode is not valid.");

				D3D.PresentParameters tempParams = this.presentParams.Value;
				tempParams.FullScreenRefreshRateInHz = displayMode.Value.RefreshRate;
				this.presentParams = tempParams;
			}

			this.capabilities = direct3d.GetDeviceCaps(0, D3D.DeviceType.Hardware);

			D3D.CreateFlags createFlags = D3D.CreateFlags.SoftwareVertexProcessing;

			if (this.capabilities.Value.DeviceCaps.HasFlag(D3D.DeviceCaps.HWTransformAndLight))
				createFlags = D3D.CreateFlags.HardwareVertexProcessing;

			try
			{
				this.InternalDevice = new D3D.Device(direct3d, 0, D3D.DeviceType.Hardware, this.host.Handle, createFlags, this.presentParams.Value);
			}
			catch (SharpDX.SharpDXException ex)
			{
				throw new GraphicsException("Unable to create GraphicsDevice.", ex);
			}

			this.IsDeviceLost = false;
		}

		/// <summary>
		/// Destructor.
		/// </summary>
		~GraphicsDevice()
		{
			this.Dispose(false);
		}

		/// <summary>
		/// Disposes of the GraphicsManager.
		/// </summary>
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// Called when the GraphicsManager is being disposed.
		/// </summary>
		/// <param name="disposing"></param>
		private void Dispose(bool disposing)
		{
			if (disposing)
			{				
				if (this.InternalDevice != null)
				{
					this.InternalDevice.Dispose();
					this.InternalDevice = null;
				}
			}
		}

		/// <summary>
		/// Attempts to reset the GraphicsDevice.
		/// </summary>
		/// <returns></returns>
		private bool ResetDevice()
		{
			if (!this.IsDeviceLost)
				this.IsDeviceLost = true;
				
			bool success = true;

			try
			{
				this.InternalDevice.Reset(this.presentParams.Value);
			}
			catch (SharpDX.SharpDXException)
			{
				success = false;
			}
			
			if (success)
			{
				this.IsDeviceLost = false;

				if (this.DeviceReset != null)
					this.DeviceReset(this, EventArgs.Empty);

				return true;
			}

			return false;
		}	
						
		private void EnsureHasDrawBegun()
		{
			if (!this.HasDrawBegun)
				throw new InvalidOperationException("Not within BeginDraw / EndDraw pair.");
		}
				
		private bool DoBeginDraw()
		{
			if (this.HasDrawBegun)
				throw new InvalidOperationException("Already within BeginDraw / EndDraw pair.");
						
			SharpDX.Result result = this.InternalDevice.TestCooperativeLevel();

			if (!result.Success)
			{
				if (result.Code == D3D.ResultCode.DeviceLost.Result.Code && !this.IsDeviceLost)
				{
					this.IsDeviceLost = true;
				}
				else if (result.Code == D3D.ResultCode.DriverInternalError.Result.Code)
				{
					// TODO: Should this be handled?
				}
			}

			if (this.IsDeviceLost && !this.ResetDevice())
				return false;

            SharpDX.Viewport viewport = new SharpDX.Viewport(0, 0, this.BackBufferWidth, this.BackBufferHeight, 0, 1);
			
			if (this.RenderTarget != null)
			{
				this.backBuffer = this.InternalDevice.GetRenderTarget(0);
				this.InternalDevice.SetRenderTarget(0, this.RenderTarget.InternalTexture.GetSurfaceLevel(0));

				viewport.Width = this.RenderTarget.Width;
				viewport.Height = this.RenderTarget.Height;
			}
			
			this.InternalDevice.Viewport = viewport;
			
			this.InternalDevice.BeginScene();

			this.HasDrawBegun = true;

			return true;
		}

		/// <summary>
		/// Requests to begin drawing.
		/// </summary>
		public bool BeginDraw()
		{
			return this.DoBeginDraw();
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

			return this.DoBeginDraw();
		}

		/// <summary>
		/// Informs the manager drawing is ending.
		/// </summary>
		public void EndDraw()
		{
			this.EnsureHasDrawBegun();

			this.InternalDevice.EndScene();
			
			if (this.RenderTarget != null)
			{
				this.InternalDevice.SetRenderTarget(0, this.backBuffer);
				this.backBuffer = null; // Don't hold onto the backbuffer any longer than necessary.
				this.RenderTarget = null;				
			}

			this.HasDrawBegun = false;
		}

		/// <summary>
		/// Clears the back buffer by filling it with the given color.
		/// </summary>
		/// <param name="color"></param>
		public void Clear(Color color)
		{
			this.InternalDevice.Clear(D3D.ClearFlags.Target | D3D.ClearFlags.ZBuffer, new SharpDX.ColorBGRA(color.R, color.G, color.B, color.A), 1.0f, 0);
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

			try
			{
				this.InternalDevice.Present();
			}
			catch (SharpDX.SharpDXException)
			{
				this.IsDeviceLost = true;
			}
		}

		/// <summary>
		/// Presents the back buffer.
		/// </summary>
		/// <param name="source">The source rectangle of the back buffer to present.</param>
		/// <param name="destination">The destination rectangle to present the back buffer into.</param>
		public void Present(Rectangle source, Rectangle destination)
		{
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
			this.PresentInternal(source, destination, window);
		}

		private void PresentInternal(Rectangle source, Rectangle destination, IntPtr window)
		{
			this.EnsureCanPresent();

			try
			{
				SharpDX.Rectangle dxSource = new SharpDX.Rectangle(source.Left, source.Top, source.Right, source.Bottom);
				SharpDX.Rectangle dxDestination = new SharpDX.Rectangle(destination.Left, destination.Top, destination.Right, destination.Bottom);
				this.InternalDevice.Present(dxSource, dxDestination, window);
			}
			catch (SharpDX.SharpDXException)
			{
				this.IsDeviceLost = true;
			}
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
