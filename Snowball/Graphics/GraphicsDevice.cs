﻿using System;
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
		IGameWindow window;
		bool isDeviceLost;

		D3D.Surface backBuffer;
			
		/// <summary>
		/// Whether or not the GraphicsDevice has been created.
		/// </summary>
		public bool IsDeviceCreated
		{
			get { return this.InternalDevice != null; }
		}

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
		public GraphicsDevice()
		{
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		public GraphicsDevice(IGameWindow window)
		{
			if (window == null)
				throw new ArgumentNullException("window");

			this.window = window;
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
				if (this.window != null)
				{
					this.window.DisplaySizeChanged -= this.Window_ClientSizeChanged;
					this.window = null;
				}

				if (this.InternalDevice != null)
				{
					this.InternalDevice.Dispose();
					this.InternalDevice = null;
				}
			}
		}

		private void EnsureGameWindowProvided(string method)
		{
			if (this.window == null)
				throw new InvalidOperationException("This overload of " + method + " may only be called when IGameWindow has been provided in the constructor of GraphicsDevice.");
		}

		private void EnsureGameWindowNotProvided(string method)
		{
			if (this.window == null)
				throw new InvalidOperationException("This overload of " + method + " may only be called when IGameWindow has not been provided in the constructor of GraphicsDevice.");
		}

		/// <summary>
		/// Creates the GraphicsDevice using the client area for the desired display size.
		/// </summary>
		public void CreateDevice()
		{
			this.EnsureGameWindowProvided("CreateDevice");
			this.CreateDevice(this.window.DisplayWidth, this.window.DisplayHeight, false);
		}

		/// <summary>
		/// Creates the GraphicsDevice using the given display size.
		/// </summary>
		/// <param name="backBufferWidth"></param>
		/// <param name="backBufferHeight"></param>
		public void CreateDevice(int backBufferWidth, int backBufferHeight)
		{
			this.EnsureGameWindowProvided("CreateDevice");
			this.CreateDevice(backBufferWidth, backBufferHeight, false);
		}
  		
		/// <summary>
		/// Creates the GraphicsDevice using the given display size.
		/// </summary>
		/// <param name="backBufferWidth"></param>
		/// <param name="backBufferHeight"></param>
		/// <param name="fullscreen">If true, the GameWindow will be made fullscreen.</param>
		public void CreateDevice(int backBufferWidth, int backBufferHeight, bool fullscreen)
		{
			this.EnsureGameWindowProvided("CreateDevice");
			this.CreateDeviceInternal(this.window.Handle, backBufferWidth, backBufferHeight, fullscreen);
			
			this.window.DisplayWidth = backBufferWidth;
			this.window.DisplayHeight = backBufferHeight;
			this.window.DisplaySizeChanged += this.Window_ClientSizeChanged;
		}

		/// <summary>
		/// Creates the GraphicsDevice using the given window and display size.
		/// </summary>
		/// <param name="window"></param>
		/// <param name="backBufferWidth"></param>
		/// <param name="backBufferHeight"></param>
		public void CreateDevice(IntPtr window, int backBufferWidth, int backBufferHeight)
		{
			this.EnsureGameWindowNotProvided("CreateDevice");
			this.CreateDeviceInternal(window, backBufferWidth, backBufferHeight, false);
		}

		private void CreateDeviceInternal(IntPtr window, int backBufferWidth, int backBufferHeight, bool fullscreen)
		{
			if (window == null)
				throw new ArgumentNullException("window");

			D3D.Direct3D direct3d = new D3D.Direct3D();

			this.presentParams = new D3D.PresentParameters(backBufferWidth, backBufferHeight) { Windowed = !fullscreen };

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
				this.InternalDevice = new D3D.Device(direct3d, 0, D3D.DeviceType.Hardware, window, createFlags, this.presentParams.Value);
			}
			catch (SharpDX.SharpDXException ex)
			{
				throw new GraphicsException("Unable to create GraphicsDevice.", ex);
			}

			this.IsDeviceLost = false;
		}

		/// <summary>
		/// Ensures the GraphicsDevice has been created.
		/// </summary>
		internal void EnsureDeviceCreated()
		{
			if (this.InternalDevice == null)
				throw new InvalidOperationException("The GraphicsDevice has not yet been created.");
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
				
		/// <summary>
		/// Called when the client size of the IGameWindow changes.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Window_ClientSizeChanged(object sender, EventArgs e)
		{
			if (this.presentParams != null)
			{
				if (this.window.DisplayWidth != this.presentParams.Value.BackBufferWidth)
					this.window.DisplayWidth = this.presentParams.Value.BackBufferWidth;

				if (this.window.DisplayHeight != this.presentParams.Value.BackBufferHeight)
					this.window.DisplayHeight = this.presentParams.Value.BackBufferHeight;
			}
		}

		private void EnsureHasDrawBegun()
		{
			if (!this.HasDrawBegun)
				throw new InvalidOperationException("Not within BeginDraw / EndDraw pair.");
		}

		private bool DoBeginDraw()
		{
			this.EnsureDeviceCreated();

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

			if (this.HasDrawBegun)
				throw new InvalidOperationException("Already within BeginDraw / EndDraw pair.");

			D3D.Viewport viewport = new D3D.Viewport(0, 0, this.BackBufferWidth, this.BackBufferHeight, 0, 1);
			
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
		/// 
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
			this.EnsureDeviceCreated();
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
			this.EnsureDeviceCreated();
			this.InternalDevice.Clear(D3D.ClearFlags.Target | D3D.ClearFlags.ZBuffer, color.ToArgb(), 1.0f, 0);
		}
		
		/// <summary>
		/// Presents the back buffer.
		/// </summary>
		public void Present()
		{
			this.EnsureDeviceCreated();

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
			this.EnsureGameWindowProvided("Present");
			this.PresentInternal(source, destination, this.window.Handle);
		}

		/// <summary>
		/// Presents the back buffer.
		/// </summary>
		/// <param name="source">The source rectangle of the back buffer to present.</param>
		/// <param name="destination">The destination rectangle to present the back buffer into.</param>
		/// <param name="window">The window to present the back buffer to.</param>
		public void Present(Rectangle source, Rectangle destination, IntPtr window)
		{
			this.EnsureGameWindowNotProvided("Present");
			this.PresentInternal(source, destination, window);
		}

		private void PresentInternal(Rectangle source, Rectangle destination, IntPtr window)
		{
			this.EnsureDeviceCreated();

			try
			{
				this.InternalDevice.Present(TypeConverter.Convert(source), TypeConverter.Convert(destination), window);
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
			this.EnsureDeviceCreated();
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
			this.EnsureDeviceCreated();
			return Texture.FromStream(this, stream, colorKey);			
		}
				
		/// <summary>
		/// Loads a TextureFont from an XML file.
		/// </summary>
		/// <param name="stream"></param>
		/// <param name="colorKey"></param>
		/// <returns></returns>
		public TextureFont LoadTextureFont(Stream stream, Color? colorKey)
		{
			this.EnsureDeviceCreated();
			return TextureFont.FromStream(this, stream, colorKey);
		}

		/// <summary>
		/// Constructs a TextureFont.
		/// </summary>
		/// <param name="fontName"></param>
		/// <param name="fontSize"></param>
		/// <param name="antialias"></param>
		/// <returns></returns>
		public TextureFont ConstructTextureFont(string fontName, int fontSize, bool antialias)
		{
			this.EnsureDeviceCreated();
			return new TextureFont(this, fontName, fontSize, antialias);
		}
	}
}
