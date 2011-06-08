using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Xml;

namespace Snowball.Graphics
{
	public class GraphicsManager : IGraphicsManager
	{
		SlimDX.Direct3D9.Device graphicsDevice;
		IGameWindow window;

		/// <summary>
		/// Whether or not the graphics device has been created.
		/// </summary>
		public bool IsGraphicsDeviceCreated
		{
			get { return this.graphicsDevice != null; }
		}

		internal SlimDX.Direct3D9.Device GraphicsDevice
		{
			get { return this.graphicsDevice; }
		}

		/// <summary>
		/// The width of the display area.
		/// </summary>
		public int DisplayWidth
		{
			get
			{
				if(this.window != null)
					return this.window.ClientWidth;

				return 0;
			}
		}

		/// <summary>
		/// The height of the display area.
		/// </summary>
		public int DisplayHeight
		{
			get
			{
				if(this.window != null)
					return this.window.ClientHeight;

				return 0;
			}
		}
		
		/// <summary>
		/// Initializes a new instance of GraphicsDevice.
		/// </summary>
		public GraphicsManager()
		{
		}

		/// <summary>
		/// Creates the graphics device.
		/// </summary>
		/// <param name="window">The game window.</param>
		public void CreateDevice(IGameWindow window)
		{
			if(window == null)
				throw new ArgumentNullException("window");

			this.window = window;

			SlimDX.Direct3D9.PresentParameters pp = new SlimDX.Direct3D9.PresentParameters()
			{
				BackBufferWidth = window.ClientWidth,
				BackBufferHeight = window.ClientHeight
			};

			this.graphicsDevice = new SlimDX.Direct3D9.Device(new SlimDX.Direct3D9.Direct3D(), 0, SlimDX.Direct3D9.DeviceType.Hardware, window.Handle,
				                                              SlimDX.Direct3D9.CreateFlags.HardwareVertexProcessing, pp);
		}

		/// <summary>
		/// Informs the manager drawing is beginning.
		/// </summary>
		public void BeginDraw()
		{
			this.graphicsDevice.BeginScene();
		}

		/// <summary>
		/// Informs the manager drawing is ending.
		/// </summary>
		public void EndDraw()
		{
			this.graphicsDevice.EndScene();
		}

		/// <summary>
		/// Clears the back buffer by filling it with the given color.
		/// </summary>
		/// <param name="color"></param>
		public void Clear(Color color)
		{
			this.graphicsDevice.Clear(SlimDX.Direct3D9.ClearFlags.Target | SlimDX.Direct3D9.ClearFlags.ZBuffer, TypeConverter.Convert(color), 1.0f, 0);
		}

		/// <summary>
		/// Presents the back buffer.
		/// </summary>
		public void Present()
		{
			this.graphicsDevice.Present();
		}

		/// <summary>
		/// Creates a new texture.
		/// </summary>
		/// <param name="width"></param>
		/// <param name="height"></param>
		/// <returns></returns>
		public Texture CreateTexture(int width, int height)
		{
			SlimDX.Direct3D9.Texture texture = new SlimDX.Direct3D9.Texture(this.graphicsDevice, width, height, 0, SlimDX.Direct3D9.Usage.None, SlimDX.Direct3D9.Format.A8R8G8B8, SlimDX.Direct3D9.Pool.Managed);
			return new Texture(texture, width, height);
		}

		/// <summary>
		/// Loads a texture.
		/// </summary>
		/// <param name="fileName">The file name to load.</param>
		/// <param name="colorKey">A color which should be used for transparency.</param>
		/// <returns></returns>
		public Texture LoadTexture(string fileName, Color? colorKey)
		{
			if(!File.Exists(fileName))
				throw new FileNotFoundException(fileName);

			Image image = Image.FromFile(fileName);
			int width = image.Width;
			int height = image.Height;
			image.Dispose();

			int argb = 0;
			if(colorKey != null)
				argb = colorKey.Value.ToArgb();

			SlimDX.Direct3D9.Texture texture = SlimDX.Direct3D9.Texture.FromFile(this.graphicsDevice, fileName, width, height, 0,
																				 SlimDX.Direct3D9.Usage.None, SlimDX.Direct3D9.Format.A8R8G8B8,
																				 SlimDX.Direct3D9.Pool.Managed, SlimDX.Direct3D9.Filter.Point,
																				 SlimDX.Direct3D9.Filter.Point, argb);
			
			return new Texture(texture, width, height);
		}

		public TextureFont LoadTextureFont(string fileName, Color? colorKey)
		{
			var rectangles = new Dictionary<char, Rectangle>();

			var xml = new XmlTextReader(fileName);
			xml.WhitespaceHandling = WhitespaceHandling.None;

			xml.Read();

			if(xml.NodeType == XmlNodeType.XmlDeclaration)
				xml.Read();

			if(xml.NodeType != XmlNodeType.Element && xml.Name != "TextureFont")
				throw new XmlException("Invalid TextureFont xml file.");

			var name = xml["Name"];
			var textureFile = xml["Texture"];

			xml.Read();
			while(xml.Name == "Character")
			{
				Rectangle rectangle = new Rectangle(Int32.Parse(xml["X"]), Int32.Parse(xml["Y"]), Int32.Parse(xml["Width"]), Int32.Parse(xml["Height"]));
				rectangles.Add(xml["Value"][0], rectangle);
				xml.Read();
			}

			return new TextureFont(this.LoadTexture(textureFile, colorKey), rectangles);
		}
	}
}
