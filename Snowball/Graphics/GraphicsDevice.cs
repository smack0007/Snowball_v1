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
		public int DisplayWidth
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
		public int DisplayHeight
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
		/// Triggered when after switching to or from fullscreen.
		/// </summary>
		public event EventHandler FullscreenToggled;

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
					this.window.ClientSizeChanged -= this.Window_ClientSizeChanged;
					this.window = null;
				}

				if (this.InternalDevice != null)
				{
					this.InternalDevice.Dispose();
					this.InternalDevice = null;
				}
			}
		}

		private void EnsureGameWindow(string method)
		{
			if (this.window == null)
				throw new InvalidOperationException("This overload of " + method + " may only be called when IGameWindow has been provided in the constructor of GraphicsDevice.");
		}

		/// <summary>
		/// Creates the GraphicsDevice using the client area for the desired display size.
		/// </summary>
		public void CreateDevice()
		{
			this.EnsureGameWindow("CreateDevice");
			this.CreateDevice(this.window.ClientWidth, this.window.ClientHeight, false);
		}

		/// <summary>
		/// Creates the GraphicsDevice using the given display size.
		/// </summary>
		/// <param name="displayWidth"></param>
		/// <param name="displayHeight"></param>
		public void CreateDevice(int displayWidth, int displayHeight)
		{
			this.EnsureGameWindow("CreateDevice");
			this.CreateDevice(displayWidth, displayHeight, false);
		}
  
		/// <summary>
		/// Creates the GraphicsDevice using the given display size.
		/// </summary>
		/// <param name="displayWidth"></param>
		/// <param name="displayHeight"></param>
		/// <param name="fullscreen"></param>
		public void CreateDevice(int displayWidth, int displayHeight, bool fullscreen)
		{
			this.EnsureGameWindow("CreateDevice");
			this.CreateDeviceInternal(this.window.Handle, displayWidth, displayHeight, fullscreen);
			
			this.window.ClientWidth = displayWidth;
			this.window.ClientHeight = displayHeight;
			this.window.ClientSizeChanged += this.Window_ClientSizeChanged;
		}

		/// <summary>
		/// Creates the GraphicsDevice using the given window and display size.
		/// </summary>
		/// <param name="window"></param>
		/// <param name="displayWidth"></param>
		/// <param name="displayHeight"></param>
		public void CreateDevice(IntPtr window, int displayWidth, int displayHeight)
		{
			this.CreateDeviceInternal(window, displayWidth, displayHeight, false);
		}

		private void CreateDeviceInternal(IntPtr window, int displayWidth, int displayHeight, bool fullscreen)
		{
			if (window == null)
				throw new ArgumentNullException("window");

			D3D.Direct3D direct3d = new D3D.Direct3D();

			D3D.DisplayModeCollection availableDisplayModes = direct3d.Adapters[0].GetDisplayModes(D3D.Format.X8R8G8B8);
			D3D.DisplayMode? displayMode = null;

			foreach (D3D.DisplayMode availableDisplayMode in availableDisplayModes)
			{
				if (availableDisplayMode.Width == displayWidth && availableDisplayMode.Height == displayHeight)
				{
					displayMode = availableDisplayMode;
					break;
				}
			}

			if (displayMode == null)
				throw new GraphicsException("The given display mode is not valid.");

			this.presentParams = new D3D.PresentParameters()
			{
				AutoDepthStencilFormat = D3D.Format.D24X8,
				BackBufferCount = 1,
				BackBufferFormat = D3D.Format.X8R8G8B8,
				BackBufferHeight = displayHeight,
				BackBufferWidth = displayWidth,
				DeviceWindowHandle = window,
				EnableAutoDepthStencil = true,
				FullScreenRefreshRateInHz = 0,
				MultiSampleQuality = 0,
				MultiSampleType = D3D.MultisampleType.None,
				PresentationInterval = D3D.PresentInterval.Immediate,
				PresentFlags = D3D.PresentFlags.None,
				SwapEffect = D3D.SwapEffect.Discard,
				Windowed = !fullscreen
			};

			//bool deviceTypeCheck = direct3d.CheckDeviceType(0, D3D.DeviceType.Hardware, D3D.Format.X8R8G8B8, D3D.Format.X8R8G8B8, !fullscreen);

			//if (!deviceTypeCheck)
			//    throw new GraphicsException("Unable to create GraphicsDevice.");

			this.capabilities = direct3d.GetDeviceCaps(0, D3D.DeviceType.Hardware);

			D3D.CreateFlags createFlags = D3D.CreateFlags.SoftwareVertexProcessing;

			if (this.capabilities.Value.DeviceCaps.HasFlag(D3D.DeviceCaps.HWTransformAndLight))
			    createFlags = D3D.CreateFlags.HardwareVertexProcessing;

			if (fullscreen)
				this.window.BeforeToggleFullscreen(true);

			try
			{
				this.InternalDevice = new D3D.Device(direct3d, 0, D3D.DeviceType.Hardware, window, createFlags, this.presentParams.Value);
			}
			catch (SharpDX.SharpDXException ex)
			{
				throw new GraphicsException("Unable to create GraphicsDevice.", ex);
			}

			if (fullscreen)
				this.window.AfterToggleFullscreen(true);

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

			D3D.PresentParameters presentParams = this.presentParams.Value;
			if (this.InternalDevice.Reset(ref presentParams) == D3D.ResultCode.Success)
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
				if (this.window.ClientWidth != this.presentParams.Value.BackBufferWidth)
					this.window.ClientWidth = this.presentParams.Value.BackBufferWidth;

				if (this.window.ClientHeight != this.presentParams.Value.BackBufferHeight)
					this.window.ClientHeight = this.presentParams.Value.BackBufferHeight;
			}
		}
				
		/// <summary>
		/// Toggles a transition to fullscreen.
		/// </summary>
		public void ToggleFullscreen()
		{
			this.EnsureDeviceCreated();

			D3D.PresentParameters temp = this.presentParams.Value;
			temp.Windowed = !temp.Windowed;
			this.presentParams = temp;

			this.window.BeforeToggleFullscreen(!this.presentParams.Value.Windowed);
						
			this.ResetDevice();

			this.window.AfterToggleFullscreen(!this.presentParams.Value.Windowed);

			if (this.FullscreenToggled != null)
				this.FullscreenToggled(this, EventArgs.Empty);
		}

		private void EnsureHasDrawBegun()
		{
			if (!this.HasDrawBegun)
				throw new InvalidOperationException("Not within BeginDraw / EndDraw pair.");
		}

		private bool DoBeginDraw()
		{
			this.EnsureDeviceCreated();

			if (this.IsDeviceLost && !this.ResetDevice())
				return false;

			if (this.HasDrawBegun)
				throw new InvalidOperationException("Already within BeginDraw / EndDraw pair.");

			D3D.Viewport viewport = new D3D.Viewport(0, 0, this.DisplayWidth, this.DisplayHeight, 0, 1);
			
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
			this.EnsureGameWindow("Present");

			Rectangle rect = new Rectangle(0, 0, this.window.ClientWidth, this.window.ClientHeight);
			this.Present(rect, rect, this.window.Handle); 
		}

		/// <summary>
		/// Presents the back buffer.
		/// </summary>
		/// <param name="source">The source rectangle of the back buffer to present.</param>
		/// <param name="destination">The destination rectangle to present the back buffer into.</param>
		/// <param name="window">The window to present the back buffer to.</param>
		public void Present(Rectangle source, Rectangle destination, IntPtr window)
		{
			this.EnsureDeviceCreated();

			try
			{
				this.InternalDevice.Present(TypeConverter.Convert(source), TypeConverter.Convert(destination), window);
			}
			catch (SharpDX.SharpDXException ex)
			{
				if (ex.ResultCode == D3D.ResultCode.DeviceLost)
					this.IsDeviceLost = true;
				else
					throw;
			}
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
