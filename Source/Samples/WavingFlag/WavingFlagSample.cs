using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Snowball;
using Snowball.Content;
using Snowball.GameFramework;
using Snowball.Graphics;

namespace WavingFlag
{
	public class WavingFlagSample : Game
	{
		[StructLayout(LayoutKind.Sequential)]
		struct Vertex
		{
			[VertexElement(VertexElementUsage.Position, 0)]
			public Vector2 Position;

			[VertexElement(VertexElementUsage.TextureCoordinates, 0)]
			public Vector2 UV;
		}

		GraphicsDevice graphicsDevice;
		GraphicsBatch graphics;
		
		ContentLoader contentLoader;

		Effect effect;
		Texture texture;
		Texture[] textureList;

		VertexBuffer<Vertex> vertexBuffer;
		
		public WavingFlagSample()
			: base()
		{
			this.Window.Title = "Snowball Waving Flag Sample";

			this.graphicsDevice = new GraphicsDevice(this.Window, false);
			this.Services.AddService(typeof(IGraphicsDevice), this.graphicsDevice);

			this.contentLoader = new ContentLoader(this.Services);
		}
				
		protected override void Initialize()
		{
			// Renderer must be created after the graphics device is created.
			this.graphics = new GraphicsBatch(this.graphicsDevice);

			this.effect = this.contentLoader.Load<Effect>(new LoadEffectArgs()
			{
				FileName = "WavingFlag.fx"
			});

			this.texture = this.contentLoader.Load<Texture>(new LoadTextureArgs()
			{
				FileName = "Flag.png"
			});

			this.textureList = new Texture[] { this.texture };

			const int totalChunks = 100;
			int chunkSize = this.texture.Width / totalChunks;

			this.vertexBuffer = new VertexBuffer<Vertex>(this.graphicsDevice, totalChunks * 6, VertexBufferUsage.Static);

			Rectangle destination = new Rectangle(100, 142, chunkSize, this.texture.Height);
			Rectangle source = new Rectangle(0, 0, chunkSize, this.texture.Height);

			List<Vertex> vertices = new List<Vertex>(this.vertexBuffer.Capacity);

			for (int i = 0; i < totalChunks; i++)
			{
				vertices.Add(new Vertex() { Position = new Vector2(destination.Left, destination.Top), UV = new Vector2((float)source.Left / this.texture.Width, (float)source.Top / this.texture.Height) });
				vertices.Add(new Vertex() { Position = new Vector2(destination.Right, destination.Top), UV = new Vector2((float)source.Right / this.texture.Width, (float)source.Top / this.texture.Height) });
				vertices.Add(new Vertex() { Position = new Vector2(destination.Left, destination.Bottom), UV = new Vector2((float)source.Left / this.texture.Width, (float)source.Bottom / this.texture.Height) });

				vertices.Add(new Vertex() { Position = new Vector2(destination.Left, destination.Bottom), UV = new Vector2((float)source.Left / this.texture.Width, (float)source.Bottom / this.texture.Height) });
				vertices.Add(new Vertex() { Position = new Vector2(destination.Right, destination.Top), UV = new Vector2((float)source.Right / this.texture.Width, (float)source.Top / this.texture.Height) });
				vertices.Add(new Vertex() { Position = new Vector2(destination.Right, destination.Bottom), UV = new Vector2((float)source.Right / this.texture.Width, (float)source.Bottom / this.texture.Height) });

				destination.X += chunkSize;
				source.X += chunkSize;
			}

			this.vertexBuffer.SetData(vertices.ToArray());
		}
		
		protected override void Draw(GameTime gameTime)
		{
			if (this.graphicsDevice.BeginDraw())
			{
				this.graphicsDevice.Clear(Color.CornflowerBlue);

				var command = this.graphicsDevice.CreateDrawCommand();
				command.VertexBuffer = this.vertexBuffer;
				command.Textures = this.textureList;

				command.Effect = this.effect;
				this.effect.SetValue<float>("StartX", 100.0f);
				this.effect.SetValue<float>("Time", (float)gameTime.TotalTime.TotalSeconds);

				this.graphicsDevice.Draw(command);				

				this.graphicsDevice.EndDraw();

				this.graphicsDevice.Present();
			}
		}

		public static void Main()
		{
			using(WavingFlagSample sample = new WavingFlagSample())
				sample.Run();
		}
	}
}
