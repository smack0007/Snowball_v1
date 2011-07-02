using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Xml;

namespace Snowball.Graphics
{
	public class GraphicsManager : IGraphicsManager, IDisposable
	{
		SlimDX.Direct3D9.Device device;
		IGameWindow window;

		int displayWidth, displayHeight;

		/// <summary>
		/// Whether or not the graphics device has been created.
		/// </summary>
		public bool IsDeviceCreated
		{
			get { return this.device != null; }
		}

		internal SlimDX.Direct3D9.Device GraphicsDevice
		{
			get { return this.device; }
		}

		/// <summary>
		/// The width of the display area.
		/// </summary>
		public int DisplayWidth
		{
			get { return this.displayWidth; }
		}

		/// <summary>
		/// The height of the display area.
		/// </summary>
		public int DisplayHeight
		{
			get { return this.displayHeight; }
		}
		
		/// <summary>
		/// Constructor.
		/// </summary>
		public GraphicsManager()
		{
		}

		/// <summary>
		/// Destructor.
		/// </summary>
		~GraphicsManager()
		{
			this.Dispose(false);
		}

		/// <summary>
		/// Disposes of the GraphicsManager.
		/// </summary>
		public void Dispose()
		{
			this.Dispose(true);
		}

		/// <summary>
		/// Called when the GraphicsManager is being disposed.
		/// </summary>
		/// <param name="disposing"></param>
		protected virtual void Dispose(bool disposing)
		{
			if(disposing)
			{
				if(this.window != null)
				{
					this.window.ClientSizeChanged -= this.Window_ClientSizeChanged;
					this.window = null;
				}

				if(this.device != null)
				{
					this.device.Dispose();
					this.device = null;
				}
			}
		}

		/// <summary>
		/// Creates the graphics device using the client area for the desired display size.
		/// </summary>
		/// <param name="window">The game window.</param>
		public void CreateDevice(IGameWindow window)
		{
			this.CreateDevice(window, window.ClientWidth, window.ClientHeight);
		}

		/// <summary>
		/// Creates the graphics device using the given display size.
		/// </summary>
		/// <param name="window"></param>
		/// <param name="displayWidth"></param>
		/// <param name="displayHeight"></param>
		public void CreateDevice(IGameWindow window, int displayWidth, int displayHeight)
		{
			if(window == null)
				throw new ArgumentNullException("window");

			this.window = window;
			this.displayWidth = displayWidth;
			this.displayHeight = displayHeight;

			this.window.ClientWidth = this.displayWidth;
			this.window.ClientHeight = this.displayHeight;
			this.window.ClientSizeChanged += this.Window_ClientSizeChanged;

			SlimDX.Direct3D9.PresentParameters pp = new SlimDX.Direct3D9.PresentParameters()
			{
				BackBufferWidth = this.displayWidth,
				BackBufferHeight = this.displayHeight
			};

			this.device = new SlimDX.Direct3D9.Device(new SlimDX.Direct3D9.Direct3D(), 0, SlimDX.Direct3D9.DeviceType.Hardware, window.Handle,
															  SlimDX.Direct3D9.CreateFlags.HardwareVertexProcessing, pp);
		}

		/// <summary>
		/// Informs the manager drawing is beginning.
		/// </summary>
		public void BeginDraw()
		{
			this.device.BeginScene();
		}

		/// <summary>
		/// Informs the manager drawing is ending.
		/// </summary>
		public void EndDraw()
		{
			this.device.EndScene();
		}

		/// <summary>
		/// Clears the back buffer by filling it with the given color.
		/// </summary>
		/// <param name="color"></param>
		public void Clear(Color color)
		{
			this.device.Clear(SlimDX.Direct3D9.ClearFlags.Target | SlimDX.Direct3D9.ClearFlags.ZBuffer, TypeConverter.Convert(color), 1.0f, 0);
		}

		/// <summary>
		/// Presents the back buffer.
		/// </summary>
		public void Present()
		{
			this.device.Present();
		}

		/// <summary>
		/// Creates a new Texture.
		/// </summary>
		/// <param name="width"></param>
		/// <param name="height"></param>
		/// <returns></returns>
		public Texture CreateTexture(int width, int height)
		{
			SlimDX.Direct3D9.Texture texture = new SlimDX.Direct3D9.Texture(this.device, width, height, 0, SlimDX.Direct3D9.Usage.None, SlimDX.Direct3D9.Format.A8R8G8B8, SlimDX.Direct3D9.Pool.Managed);
			return new Texture(texture, width, height);
		}

		/// <summary>
		/// Loads a Texture.
		/// </summary>
		/// <param name="fileName">The file name to load.</param>
		/// <param name="colorKey">A color which should be used for transparency.</param>
		/// <returns></returns>
		public Texture LoadTexture(string fileName, Color? colorKey)
		{
			if(!File.Exists(fileName))
				throw new FileNotFoundException("Unable to load file " + fileName + ".");

			Image image = Image.FromFile(fileName);
			int width = image.Width;
			int height = image.Height;
			image.Dispose();

			int argb = 0;
			if(colorKey != null)
				argb = colorKey.Value.ToArgb();

			SlimDX.Direct3D9.Texture texture = SlimDX.Direct3D9.Texture.FromFile(this.device, fileName, width, height, 0,
																				 SlimDX.Direct3D9.Usage.None, SlimDX.Direct3D9.Format.A8R8G8B8,
																				 SlimDX.Direct3D9.Pool.Managed, SlimDX.Direct3D9.Filter.Point,
																				 SlimDX.Direct3D9.Filter.Point, argb);
			
			return new Texture(texture, width, height);
		}

		/// <summary>
		/// Loads a TextureFont from an XML file.
		/// </summary>
		/// <param name="fileName"></param>
		/// <param name="colorKey"></param>
		/// <returns></returns>
		public TextureFont LoadTextureFont(string fileName, Color? colorKey)
		{
			if(!File.Exists(fileName))
				throw new FileNotFoundException("Unable to load file " + fileName + ".");

			Dictionary<char, Rectangle> rectangles = new Dictionary<char, Rectangle>();
			string textureFile = null;

			using(var xml = new XmlTextReader(fileName))
			{
				xml.WhitespaceHandling = WhitespaceHandling.None;

				xml.Read();

				if(xml.NodeType == XmlNodeType.XmlDeclaration)
					xml.Read();

				if(xml.NodeType != XmlNodeType.Element && xml.Name != "TextureFont")
					throw new XmlException("Invalid TextureFont xml file.");

				string name = xml["Name"];
				textureFile = xml["Texture"];

				xml.Read();
				while(xml.Name == "Character")
				{
					Rectangle rectangle = new Rectangle(Int32.Parse(xml["X"]), Int32.Parse(xml["Y"]), Int32.Parse(xml["Width"]), Int32.Parse(xml["Height"]));
					rectangles.Add(xml["Value"][0], rectangle);
					xml.Read();
				}
			}

			return new TextureFont(this.LoadTexture(textureFile, colorKey), rectangles);
		}

		/// <summary>
		/// Called when the client size of the IGameWindow changes.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Window_ClientSizeChanged(object sender, EventArgs e)
		{
			if(this.window.ClientWidth != this.displayWidth)
				this.window.ClientWidth = this.displayWidth;

			if(this.window.ClientHeight != this.displayHeight)
				this.window.ClientHeight = this.displayHeight;
		}
	}
}
