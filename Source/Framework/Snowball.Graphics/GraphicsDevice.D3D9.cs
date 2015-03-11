using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using DX = SharpDX;
using D3D9 = SharpDX.Direct3D9;

namespace Snowball.Graphics
{
	public partial class GraphicsDevice
	{
		internal D3D9.Device d3d9Device;

		D3D9.PresentParameters? presentParams;
		D3D9.Capabilities? capabilities;

		D3D9.Surface backBuffer;

		bool isDeviceLost;

		internal event EventHandler DeviceReset;

		internal event EventHandler DeviceLost;

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

		internal bool TexturesMustBePowerOf2
		{
			get
			{
				if (this.capabilities != null)
				{
					if (this.capabilities.Value.TextureCaps.HasFlag(D3D9.TextureCaps.Pow2))
					{
						if (this.capabilities.Value.TextureCaps.HasFlag(D3D9.TextureCaps.NonPow2Conditional))
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
					return this.capabilities.Value.TextureCaps.HasFlag(D3D9.TextureCaps.SquareOnly);
				}

				return true; // Assume true.
			}
		}

		private void Construct(IHostControl host, bool fullscreen, int backBufferWidth, int backBufferHeight)
		{
			D3D9.Direct3D direct3d = new D3D9.Direct3D();

			this.presentParams = new D3D9.PresentParameters(backBufferWidth, backBufferHeight)
			{
				DeviceWindowHandle = host.Handle,
				Windowed = !fullscreen
			};

			if (fullscreen) // Only check display mode if we're going fullscreen.
			{
				D3D9.DisplayModeCollection availableDisplayModes = direct3d.Adapters[0].GetDisplayModes(D3D9.Format.X8R8G8B8);
				D3D9.DisplayMode? displayMode = null;

				foreach (D3D9.DisplayMode availableDisplayMode in availableDisplayModes)
				{
					if (availableDisplayMode.Width == this.presentParams.Value.BackBufferWidth && availableDisplayMode.Height == this.presentParams.Value.BackBufferHeight)
					{
						displayMode = availableDisplayMode;
						break;
					}
				}

				if (displayMode == null)
					throw new GraphicsException("The given display mode is not valid.");

				D3D9.PresentParameters tempParams = this.presentParams.Value;
				tempParams.FullScreenRefreshRateInHz = displayMode.Value.RefreshRate;
				this.presentParams = tempParams;
			}

			this.capabilities = direct3d.GetDeviceCaps(0, D3D9.DeviceType.Hardware);

			D3D9.CreateFlags createFlags = D3D9.CreateFlags.SoftwareVertexProcessing;

			if (this.capabilities.Value.DeviceCaps.HasFlag(D3D9.DeviceCaps.HWTransformAndLight))
				createFlags = D3D9.CreateFlags.HardwareVertexProcessing;

			try
			{
				this.d3d9Device = new D3D9.Device(direct3d, 0, D3D9.DeviceType.Hardware, this.host.Handle, createFlags, this.presentParams.Value);
			}
			catch (DX.SharpDXException ex)
			{
				throw new GraphicsException("Unable to create GraphicsDevice.", ex);
			}

			this.BackBufferWidth = backBufferWidth;
			this.BackBufferHeight = backBufferHeight;

			this.IsDeviceLost = false;
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (this.d3d9Device != null)
				{
					this.d3d9Device.Dispose();
					this.d3d9Device = null;
				}
			}
		}

		private bool ResetDevice()
		{
			if (!this.IsDeviceLost)
				this.IsDeviceLost = true;

			bool success = true;

			try
			{
				this.d3d9Device.Reset(this.presentParams.Value);
			}
			catch (DX.SharpDXException)
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

		private bool BeginDrawInternal()
		{
			DX.Result result = this.d3d9Device.TestCooperativeLevel();

			if (!result.Success)
			{
				if (result.Code == D3D9.ResultCode.DeviceLost.Result.Code && !this.IsDeviceLost)
				{
					this.IsDeviceLost = true;
				}
				else if (result.Code == D3D9.ResultCode.DriverInternalError.Result.Code)
				{
					// TODO: Should this be handled?
				}
			}

			if (this.IsDeviceLost && !this.ResetDevice())
				return false;

			DX.Viewport viewport = new DX.Viewport(0, 0, this.BackBufferWidth, this.BackBufferHeight, 0, 1);

			if (this.RenderTarget != null)
			{
				this.backBuffer = this.d3d9Device.GetRenderTarget(0);
				this.d3d9Device.SetRenderTarget(0, this.RenderTarget.d3d9Texture.GetSurfaceLevel(0));

				viewport.Width = this.RenderTarget.Width;
				viewport.Height = this.RenderTarget.Height;
			}

			this.d3d9Device.Viewport = viewport;

			this.d3d9Device.BeginScene();

			return true;
		}

		private void EndDrawInternal()
		{
			this.d3d9Device.EndScene();

			if (this.RenderTarget != null)
			{
				this.d3d9Device.SetRenderTarget(0, this.backBuffer);
				this.backBuffer = null; // Don't hold onto the backbuffer any longer than necessary.
				this.RenderTarget = null;
			}
		}

		private void ClearInternal(Color color)
		{
			this.d3d9Device.Clear(D3D9.ClearFlags.Target | D3D9.ClearFlags.ZBuffer, new DX.ColorBGRA(color.R, color.G, color.B, color.A), 1.0f, 0);
		}

		private void PresentInternal()
		{
			try
			{
				this.d3d9Device.Present();
			}
			catch (DX.SharpDXException)
			{
				this.IsDeviceLost = true;
			}
		}

		private void PresentInternal(Rectangle source, Rectangle destination, IntPtr window)
		{
			try
			{
				DX.Rectangle dxSource = new DX.Rectangle(source.Left, source.Top, source.Right, source.Bottom);
				DX.Rectangle dxDestination = new DX.Rectangle(destination.Left, destination.Top, destination.Right, destination.Bottom);
				this.d3d9Device.Present(dxSource, dxDestination, window);
			}
			catch (DX.SharpDXException)
			{
				this.IsDeviceLost = true;
			}
		}
	}
}
