using System;
using System.IO;

namespace Snowball.Graphics
{
	public sealed class GraphicsDevice : IGraphicsDevice, IDisposable
	{
		internal SlimDX.Direct3D9.Device InternalDevice;

		SlimDX.Direct3D9.PresentParameters presentParams;
		IGameWindow window;
		bool isDeviceLost;
			
		/// <summary>
		/// Whether or not the graphics device has been created.
		/// </summary>
		public bool IsDeviceCreated
		{
			get { return this.InternalDevice != null; }
		}

		/// <summary>
		/// Whether or not the graphics device is lost. If the device is lost,
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
			get { return this.presentParams.BackBufferWidth; }
		}

		/// <summary>
		/// The height of the display area.
		/// </summary>
		public int DisplayHeight
		{
			get { return this.presentParams.BackBufferHeight; }
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
		/// The RenderTarget which is currently being drawn to. If null, the backbuffer is being drawn to.
		/// </summary>
		public RenderTarget RenderTarget
		{
			get;
			private set;
		}
				
		/// <summary>
		/// Triggered when the graphics device has been reset. 
		/// </summary>
		internal event EventHandler DeviceReset;

		/// <summary>
		/// Triggered when the graphics device is lost.
		/// </summary>
		internal event EventHandler DeviceLost;
		
		/// <summary>
		/// Triggered when after switching to or from fullscreen.
		/// </summary>
		public event EventHandler FullscreenToggled;
		
		/// <summary>
		/// Constructor.
		/// </summary>
		public GraphicsDevice(IGameWindow window)
		{
			if (window == null)
				throw new ArgumentNullException("window");

			this.window = window;
						
			this.HasDrawBegun = false;
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

		/// <summary>
		/// Creates the graphics device using the client area for the desired display size.
		/// </summary>
		public void CreateDevice()
		{
			this.CreateDevice(this.window.ClientWidth, this.window.ClientHeight, false);
		}

		/// <summary>
		/// Creates the graphics device using the given display size.
		/// </summary>
		/// <param name="displayWidth"></param>
		/// <param name="displayHeight"></param>
		public void CreateDevice(int displayWidth, int displayHeight)
		{
			this.CreateDevice(displayWidth, displayHeight, false);
		}

		/// <summary>
		/// Creates the graphics device using the given display size.
		/// </summary>
		/// <param name="displayWidth"></param>
		/// <param name="displayHeight"></param>
		/// <param name="fullscreen"></param>
		public void CreateDevice(int displayWidth, int displayHeight, bool fullscreen)
		{
			this.window.ClientWidth = displayWidth;
			this.window.ClientHeight = displayHeight;
			this.window.ClientSizeChanged += this.Window_ClientSizeChanged;

			this.presentParams = new SlimDX.Direct3D9.PresentParameters()
			{
				DeviceWindowHandle = this.window.Handle,
				BackBufferWidth = displayWidth,
				BackBufferHeight = displayHeight,
				Windowed = !fullscreen
			};
			
			if (fullscreen)
				this.window.BeforeToggleFullscreen(true);
	
			this.InternalDevice = new SlimDX.Direct3D9.Device(new SlimDX.Direct3D9.Direct3D(), 0, SlimDX.Direct3D9.DeviceType.Hardware, window.Handle,
													          SlimDX.Direct3D9.CreateFlags.HardwareVertexProcessing, this.presentParams);

			if (fullscreen)
				this.window.AfterToggleFullscreen(true);

			this.IsDeviceLost = false;
		}

		/// <summary>
		/// Ensures the graphics device has been created.
		/// </summary>
		internal void EnsureDeviceCreated()
		{
			if (this.InternalDevice == null)
				throw new InvalidOperationException("The graphics device has not yet been created.");
		}

		/// <summary>
		/// Attempts to reset the graphics device.
		/// </summary>
		/// <returns></returns>
		private bool ResetDevice()
		{
			if (!this.IsDeviceLost)
				this.IsDeviceLost = true;

			if (this.InternalDevice.Reset(this.presentParams) == SlimDX.Direct3D9.ResultCode.Success)
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
			if (this.window.ClientWidth != this.presentParams.BackBufferWidth)
				this.window.ClientWidth = this.presentParams.BackBufferWidth;

			if (this.window.ClientHeight != this.presentParams.BackBufferHeight)
				this.window.ClientHeight = this.presentParams.BackBufferHeight;
		}
				
		/// <summary>
		/// Toggles a transition to fullscreen.
		/// </summary>
		public void ToggleFullscreen()
		{
			this.EnsureDeviceCreated();
			
			this.presentParams.Windowed = !this.presentParams.Windowed;

			this.window.BeforeToggleFullscreen(!this.presentParams.Windowed);
						
			this.ResetDevice();

			this.window.AfterToggleFullscreen(!this.presentParams.Windowed);

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

			if (this.RenderTarget == null)
			{
				this.InternalDevice.BeginScene();
			}
			else
			{
				SlimDX.Direct3D9.Viewport viewport = new SlimDX.Direct3D9.Viewport(0, 0, this.RenderTarget.Width, this.RenderTarget.Height);
				this.RenderTarget.InternalRenderToSurface.BeginScene(this.RenderTarget.InternalTexture.GetSurfaceLevel(0), viewport);
			}

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
		public bool BeginDraw(RenderTarget renderTarget)
		{
			if (renderTarget == null)
				throw new ArgumentNullException("renderTarget");

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

			if (this.RenderTarget == null)
			{
				this.InternalDevice.EndScene();
			}
			else
			{
				this.RenderTarget.InternalRenderToSurface.EndScene(SlimDX.Direct3D9.Filter.None);
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
			
			if (this.RenderTarget == null)
				this.InternalDevice.Clear(SlimDX.Direct3D9.ClearFlags.Target | SlimDX.Direct3D9.ClearFlags.ZBuffer, color.ToArgb(), 1.0f, 0);
			else
				this.InternalDevice.Clear(SlimDX.Direct3D9.ClearFlags.Target, color.ToArgb(), 0.0f, 0);
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
			catch(SlimDX.Direct3D9.Direct3D9Exception ex)
			{
				if (ex.ResultCode == SlimDX.Direct3D9.ResultCode.DeviceLost)
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
	}
}
