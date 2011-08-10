using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;
using System.Xml;


namespace Snowball.Graphics
{
	public class GraphicsManager : IGraphicsManager, IDisposable
	{
		SlimDX.Direct3D9.Device device;
		SlimDX.Direct3D9.PresentParameters presentParams;
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
		/// Constructor.
		/// </summary>
		public GraphicsManager()
		{
			this.HasDrawBegun = false;
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
			GC.SuppressFinalize(this);
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
			this.CreateDevice(window, window.ClientWidth, window.ClientHeight, false);
		}

		/// <summary>
		/// Creates the graphics device using the given display size.
		/// </summary>
		/// <param name="window"></param>
		/// <param name="displayWidth"></param>
		/// <param name="displayHeight"></param>
		public void CreateDevice(IGameWindow window, int displayWidth, int displayHeight)
		{
			this.CreateDevice(window, displayWidth, displayHeight, false);
		}

		/// <summary>
		/// Creates the graphics device using the given display size.
		/// </summary>
		/// <param name="window"></param>
		/// <param name="displayWidth"></param>
		/// <param name="displayHeight"></param>
		/// <param name="fullscreen"></param>
		public void CreateDevice(IGameWindow window, int displayWidth, int displayHeight, bool fullscreen)
		{
			if(window == null)
				throw new ArgumentNullException("window");

			this.window = window;
			this.displayWidth = displayWidth;
			this.displayHeight = displayHeight;

			this.window.ClientWidth = this.displayWidth;
			this.window.ClientHeight = this.displayHeight;
			this.window.ClientSizeChanged += this.Window_ClientSizeChanged;

			this.presentParams = new SlimDX.Direct3D9.PresentParameters()
			{
				BackBufferWidth = this.displayWidth,
				BackBufferHeight = this.displayHeight,
				Windowed = !fullscreen
			};

			this.device = new SlimDX.Direct3D9.Device(new SlimDX.Direct3D9.Direct3D(), 0, SlimDX.Direct3D9.DeviceType.Hardware, window.Handle,
													  SlimDX.Direct3D9.CreateFlags.HardwareVertexProcessing, this.presentParams);
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

		/// <summary>
		/// Ensures the graphics device has been created.
		/// </summary>
		private void EnsureDevice()
		{
			if(this.device == null)
				throw new InvalidOperationException("The graphics device has not yet been created.");
		}

		/// <summary>
		/// Toggles a transition to fullscreen.
		/// </summary>
		public void ToggleFullscreen()
		{
			this.EnsureDevice();

			this.presentParams.Windowed = !this.presentParams.Windowed;

			this.device.Reset(this.presentParams);
		}

		private void EnsureHasDrawBegun()
		{
			if(!this.HasDrawBegun)
				throw new InvalidOperationException("Not within BeginDraw / EndDraw pair.");
		}

		/// <summary>
		/// Informs the manager drawing is beginning.
		/// </summary>
		public void BeginDraw()
		{
			this.EnsureDevice();

			if(this.HasDrawBegun)
				throw new InvalidOperationException("Already within BeginDraw / EndDraw pair.");

			if(this.RenderTarget == null)
			{
				this.device.BeginScene();
			}
			else
			{
				SlimDX.Direct3D9.Viewport viewport = new SlimDX.Direct3D9.Viewport(0, 0, this.RenderTarget.Width, this.RenderTarget.Height);
				this.RenderTarget.renderToSurface.BeginScene(this.RenderTarget.texture.GetSurfaceLevel(0), viewport);
			}

			this.HasDrawBegun = true;
		}

		/// <summary>
		/// Informs the manager drawing is ending.
		/// </summary>
		public void EndDraw()
		{
			this.EnsureDevice();
			this.EnsureHasDrawBegun();

			if(this.RenderTarget == null)
			{
				this.device.EndScene();
			}
			else
			{
				this.RenderTarget.renderToSurface.EndScene(SlimDX.Direct3D9.Filter.None);
			}

			this.HasDrawBegun = false;
		}

		/// <summary>
		/// Clears the back buffer by filling it with the given color.
		/// </summary>
		/// <param name="color"></param>
		public void Clear(Color color)
		{
			this.EnsureDevice();
			
			if(this.RenderTarget == null)
				this.device.Clear(SlimDX.Direct3D9.ClearFlags.Target | SlimDX.Direct3D9.ClearFlags.ZBuffer, color.ToArgb(), 1.0f, 0);
			else
				this.device.Clear(SlimDX.Direct3D9.ClearFlags.Target, color.ToArgb(), 0.0f, 0);
		}

		/// <summary>
		/// Presents the back buffer.
		/// </summary>
		public void Present()
		{
			this.EnsureDevice();
			
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
			this.EnsureDevice();

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
			this.EnsureDevice();

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
		/// Renders a character to a Bitmap.
		/// </summary>
		/// <param name="graphics"></param>
		/// <param name="font"></param>
		/// <param name="ch"></param>
		/// <param name="antialias"></param>
		/// <returns></returns>
		private Bitmap RenderCharcater(System.Drawing.Graphics graphics, Font font, char ch, bool antialias)
		{
			string text = ch.ToString();
			SizeF size = graphics.MeasureString(text, font);

			int charWidth = (int)Math.Ceiling(size.Width);
			int charHeight = (int)Math.Ceiling(size.Height);

			Bitmap charBitmap = new Bitmap(charWidth, charHeight, PixelFormat.Format32bppArgb);

			using(System.Drawing.Graphics bitmapGraphics = System.Drawing.Graphics.FromImage(charBitmap))
			{
				if(antialias)
					bitmapGraphics.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
				else
					bitmapGraphics.TextRenderingHint = TextRenderingHint.SingleBitPerPixelGridFit;

				bitmapGraphics.Clear(System.Drawing.Color.Transparent);

				using(Brush brush = new SolidBrush(System.Drawing.Color.White))
				using(StringFormat format = new StringFormat())
				{
					format.Alignment = StringAlignment.Near;
					format.LineAlignment = StringAlignment.Near;

					bitmapGraphics.DrawString(text, font, brush, 0, 0, format);
				}

				bitmapGraphics.Flush();
			}

			return this.CropCharacter(charBitmap);
		}

		private Bitmap CropCharacter(Bitmap charBitmap)
		{
			int left = 0;
			int right = charBitmap.Width - 1;
			bool go = true;

			// See how far we can crop on the left
			while(go)
			{
				for(int y = 0; y < charBitmap.Height; y++)
				{
					if(charBitmap.GetPixel(left, y).A != 0)
					{
						go = false;
						break;
					}
				}

				if(go)
				{
					left++;

					if(left >= charBitmap.Width)
						break;
				}
			}

			go = true;

			// See how far we can crop on the right
			while(go)
			{
				for(int y = 0; y < charBitmap.Height; y++)
				{
					if(charBitmap.GetPixel(right, y).A != 0)
					{
						go = false;
						break;
					}
				}

				if(go)
				{
					right--;

					if(right < 0)
						break;
				}
			}

			// We can't crop or don't need to crop
			if(left > right || (left == 0 && right == charBitmap.Width - 1))
				return charBitmap;

			Bitmap croppedBitmap = new Bitmap((right - left) + 1, charBitmap.Height, PixelFormat.Format32bppArgb);

			using(System.Drawing.Graphics croppedGraphics = System.Drawing.Graphics.FromImage(croppedBitmap))
			{
				croppedGraphics.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceCopy;

				System.Drawing.RectangleF dest = new System.Drawing.RectangleF(0, 0, (right - left) + 1, charBitmap.Height);
				System.Drawing.RectangleF src = new System.Drawing.RectangleF(left, 0, (right - left) + 1, charBitmap.Height);
				croppedGraphics.DrawImage(charBitmap, dest, src, GraphicsUnit.Pixel);
				croppedGraphics.Flush();
			}

			return croppedBitmap;
		}

		/// <summary>
		/// Creates a TextureFont.
		/// </summary>
		/// <param name="fontName"></param>
		/// <param name="fontSize"></param>
		/// <param name="antialias"></param>
		/// <returns></returns>
		public TextureFont CreateTextureFont(string fontName, int fontSize, bool antialias)
		{
			this.EnsureDevice();

			Font font = new Font(fontName, fontSize);

			int minChar = 0x20;
			int maxChar = 0x7F;

			System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage(new Bitmap(1, 1, PixelFormat.Format32bppArgb));
			List<Bitmap> charBitmaps = new List<Bitmap>();
			Dictionary<char, Rectangle> rectangles = new Dictionary<char, Rectangle>();
			int bitmapWidth = 0;
			int bitmapHeight = 0;
			int lineHeight = 0;
			int rows = 1;

			int count = 0;
			int x = 0;
			int y = 0;
			const int padding = 4;

			MemoryStream stream = new MemoryStream();

			for(char ch = (char)minChar; ch < maxChar; ch++)
			{
				Bitmap charBitmap = this.RenderCharcater(graphics, font, ch, antialias);

				charBitmaps.Add(charBitmap);
								
				x += charBitmap.Width + padding;
				lineHeight = Math.Max(lineHeight, charBitmap.Height);

				count++;
				if(count >= 16)
				{
					bitmapWidth = Math.Max(bitmapWidth, x);
					rows++;
					x = 0;
					count = 0;
				}
			}

			bitmapHeight = (lineHeight * rows) + (padding * rows);
						
			using(Bitmap bitmap = new Bitmap(bitmapWidth, bitmapHeight, PixelFormat.Format32bppArgb))
			{
				using(System.Drawing.Graphics bitmapGraphics = System.Drawing.Graphics.FromImage(bitmap))
				{
					count = 0;
					x = 0;
					y = 0;

					char ch = (char)minChar;
					for(int i = 0; i < charBitmaps.Count; i++)
					{
						bitmapGraphics.DrawImage(charBitmaps[i], x, y);
												
						rectangles.Add(ch, new Rectangle(x, y, charBitmaps[i].Width, lineHeight));
						ch++;

						x += charBitmaps[i].Width + padding;
						charBitmaps[i].Dispose();

						count++;
						if(count >= 16)
						{
							x = 0;
							y += lineHeight + padding;
							count = 0;
						}
					}
				}

				bitmap.Save(stream, ImageFormat.Bmp);
				stream.Position = 0;
			}
	
			SlimDX.Direct3D9.Texture texture = SlimDX.Direct3D9.Texture.FromStream(this.device, stream, bitmapWidth, bitmapHeight, 0,
																				   SlimDX.Direct3D9.Usage.None, SlimDX.Direct3D9.Format.A8R8G8B8,
																				   SlimDX.Direct3D9.Pool.Managed, SlimDX.Direct3D9.Filter.Point,
																				   SlimDX.Direct3D9.Filter.Point, 0);

			stream.Dispose();

			return new TextureFont(new Texture(texture, bitmapWidth, bitmapHeight), rectangles);
		}

		/// <summary>
		/// Loads a TextureFont from an XML file.
		/// </summary>
		/// <param name="fileName"></param>
		/// <param name="colorKey"></param>
		/// <returns></returns>
		public TextureFont LoadTextureFont(string fileName, Color? colorKey)
		{
			this.EnsureDevice();

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
		/// Creates a new RenderTarget.
		/// </summary>
		/// <param name="width"></param>
		/// <param name="height"></param>
		/// <returns></returns>
		public RenderTarget CreateRenderTarget(int width, int height)
		{
			this.EnsureDevice();

			SlimDX.Direct3D9.RenderToSurface renderToSurface = new SlimDX.Direct3D9.RenderToSurface(this.device, width, height, SlimDX.Direct3D9.Format.A8R8G8B8);

			SlimDX.Direct3D9.Texture texture = new SlimDX.Direct3D9.Texture(this.device, width, height, 0, SlimDX.Direct3D9.Usage.RenderTarget,
				                                                            SlimDX.Direct3D9.Format.A8R8G8B8, SlimDX.Direct3D9.Pool.Default);


			return new RenderTarget(renderToSurface, texture, width, height);
		}

		/// <summary>
		/// Sets the render target used in the next draw. Pass in null to use the backbuffer.
		/// </summary>
		/// <param name="renderTarget"></param>
		public void SetRenderTarget(RenderTarget renderTarget)
		{
			this.EnsureDevice();

			if(this.HasDrawBegun)
				throw new InvalidOperationException("Render target cannot be set within BeginDraw / EndDraw pair.");

			this.RenderTarget = renderTarget;
		}
	}
}
