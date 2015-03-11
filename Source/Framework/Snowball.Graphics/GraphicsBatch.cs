using System;

using D3D = SharpDX.Direct3D9;

namespace Snowball.Graphics
{
	/// <summary>
	/// Pushes vertex data to the GraphicsDevice.
	/// </summary>
	public sealed class GraphicsBatch : IDisposable
	{
		struct Vertex
		{
			public SharpDX.Vector4 Position;
			public SharpDX.Vector4 Color;
			public SharpDX.Vector2 UV;
		}
				
		D3D.VertexDeclaration vertexDeclaration;
		Vertex[] vertices;
		int vertexCount;
		short[] indices;

		BasicEffect basicEffect;
		IEffect effect;
		int effectTechnique;
		int effectPass;

		D3D.Texture pixel;
		Rectangle pixelSource;

		D3D.Texture texture;
		int textureWidth;
		int textureHeight;

		Matrix transformMatrix;
		Matrix[] matrixStack;
		int matrixStackCount;

		public GraphicsDevice GraphicsDevice
		{
			get;
			private set;
		}

		public bool HasBegun
		{
			get;
			private set;
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="graphicsDevice"></param>
		public GraphicsBatch(GraphicsDevice graphicsDevice)
			: this(graphicsDevice, 1024, 8)
		{
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="graphicsDevice"></param>
		/// <param name="vertexBufferSize"></param>
		/// <param name="matrixStackSize"></param>
		/// <param name="colorStackSize"></param>
		public GraphicsBatch(GraphicsDevice graphicsDevice, int vertexBufferSize, int matrixStackSize)
		{
			if (graphicsDevice == null)
				throw new ArgumentNullException("graphicsDevice");

			this.GraphicsDevice = graphicsDevice;

			this.vertexDeclaration = new D3D.VertexDeclaration(this.GraphicsDevice.d3d9Device, new[] {
        		new D3D.VertexElement(0, 0, D3D.DeclarationType.Float4, D3D.DeclarationMethod.Default, D3D.DeclarationUsage.Position, 0),
        		new D3D.VertexElement(0, 16, D3D.DeclarationType.Float4, D3D.DeclarationMethod.Default, D3D.DeclarationUsage.Color, 0),
				new D3D.VertexElement(0, 32, D3D.DeclarationType.Float2, D3D.DeclarationMethod.Default, D3D.DeclarationUsage.TextureCoordinate, 0),
				D3D.VertexElement.VertexDeclarationEnd
        	});

			this.vertices = new Vertex[vertexBufferSize * 4];
			this.vertexCount = 0;
			
			this.indices = new short[vertexBufferSize * 6];

			for (short i = 0, vertex = 0; i < this.indices.Length; i += 6, vertex += 4)
			{
				this.indices[i] = vertex;
				this.indices[i + 1] = (short)(vertex + 1);
				this.indices[i + 2] = (short)(vertex + 2);
				this.indices[i + 3] = vertex;
				this.indices[i + 4] = (short)(vertex + 2);
				this.indices[i + 5] = (short)(vertex + 3);
			}

			this.basicEffect = new BasicEffect(this.GraphicsDevice);

			this.pixel = D3DHelper.CreateTexture(this.GraphicsDevice.d3d9Device, 1, 1, TextureUsage.None);

			SharpDX.DataRectangle dataRectangle = this.pixel.LockRectangle(0, D3D.LockFlags.None);

			using (SharpDX.DataStream dataStream = new SharpDX.DataStream(dataRectangle.DataPointer, dataRectangle.Pitch, true, true))
			{
				dataStream.WriteRange(new Color[] { Color.White });
			}

			this.pixel.UnlockRectangle(0);

			this.pixelSource = new Rectangle(0, 0, 1, 1);

			this.matrixStack = new Matrix[matrixStackSize];
			this.matrixStackCount = 0;
			
			this.texture = null;
		}

		~GraphicsBatch()
		{
			this.Dispose(false);
		}

		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		private void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (this.vertexDeclaration != null)
				{
					this.vertexDeclaration.Dispose();
					this.vertexDeclaration = null;
				}
			}
		}
		
		private void EnsureGraphicsDeviceHasDrawBegun()
		{
			if (!this.GraphicsDevice.HasDrawBegun)
				throw new InvalidOperationException("The GraphicsDevice has not yet began drawing.");
		}

		/// <summary>
		/// Begins rendering.
		/// </summary>
		public void Begin()
		{
			this.Begin(this.basicEffect, 0, 0);
		}

		private void CalculateTransformMatrix()
		{
			this.transformMatrix = new Matrix()
			{
				M11 = 2f / this.GraphicsDevice.BackBufferWidth,
				M22 = -2f / this.GraphicsDevice.BackBufferHeight,
				M33 = 1f,
				M44 = 1f,
				M41 = -1f,
				M42 = 1f
			};

		}
		
		/// <summary>
		/// Begins rendering using the given Effect.
		/// </summary>
		/// <param name="effect"></param>
		/// <param name="technique"></param>
		/// <param name="pass"></param>
		public void Begin(IEffect effect, int technique, int pass)
		{
			if (effect == null)
				throw new ArgumentNullException("effect");

			this.EnsureGraphicsDeviceHasDrawBegun();

			if (this.HasBegun)
				throw new InvalidOperationException("Already within Begin / End pair.");

			this.effect = effect;
			this.effectTechnique = technique;
			this.effectPass = pass;

			this.CalculateTransformMatrix();

			this.HasBegun = true;
		}

		private void EnsureHasBegun()
		{
			if (!this.HasBegun)
				throw new InvalidOperationException("Not within Begin / End pair.");
		}

		public void End()
		{
			this.EnsureHasBegun();

			this.Flush();

			this.effect = null;
			this.matrixStackCount = 0;

			this.HasBegun = false;
		}

		public void PushMatrix(Matrix matrix)
		{
			this.EnsureHasBegun();

			if (this.matrixStackCount == this.matrixStack.Length)
				throw new InvalidOperationException("Matrix stack full.");

			this.matrixStack[this.matrixStackCount] = matrix;
			this.matrixStackCount++;
		}

		public void PopMatrix()
		{
			this.EnsureHasBegun();

			if (this.matrixStackCount <= 0)
				throw new InvalidOperationException("Matrix stack empty.");

			this.matrixStackCount--;
		}

		private Vector2 Transform(Vector2 input)
		{
			for (int i = 0; i < this.matrixStackCount; i++)
				input = Vector2.Transform(input, this.matrixStack[i]);
						
			return input;
		}
				
		private void SetTexture(D3D.Texture texture, int width, int height)
		{
			if (texture == null)
				throw new ArgumentNullException("texture");

			if (texture != this.texture)
				Flush();

			this.texture = texture;
			this.textureWidth = width;
			this.textureHeight = height;
		}

		private void SetPixelTexture()
		{
			this.SetTexture(this.pixel, 1, 1);
		}

		private SharpDX.Vector2 CalculateUV(float x, float y)
		{
			SharpDX.Vector2 uv = SharpDX.Vector2.Zero;

			if (this.textureWidth != 1 || this.textureHeight != 1)
			{
				uv = new SharpDX.Vector2(x / (float)this.textureWidth, y / (float)this.textureHeight);
			}

			return uv;
		}
					
		private void AddQuad(
			Vector2 v1,
			Color c1,
			Vector2 v2,
			Color c2,
			Vector2 v3,
			Color c3,
			Vector2 v4,
			Color c4,
			Rectangle source)
		{
			if (this.vertexCount >= this.vertices.Length)
				this.Flush();
						
			v1 = this.Transform(v1);
			v2 = this.Transform(v2);
			v3 = this.Transform(v3);
			v4 = this.Transform(v4);

			this.vertices[this.vertexCount].Position = new SharpDX.Vector4(v1.X, v1.Y, 0.5f, 1);
			this.vertices[this.vertexCount].Color = new SharpDX.Vector4(c1.R / 255.0f, c1.G / 255.0f, c1.B / 255.0f, c1.A / 255.0f);

			this.vertices[this.vertexCount + 1].Position = new SharpDX.Vector4(v2.X, v2.Y, 0.5f, 1);
			this.vertices[this.vertexCount + 1].Color = new SharpDX.Vector4(c2.R / 255.0f, c2.G / 255.0f, c2.B / 255.0f, c2.A / 255.0f);

			this.vertices[this.vertexCount + 2].Position = new SharpDX.Vector4(v3.X, v3.Y, 0.5f, 1);
			this.vertices[this.vertexCount + 2].Color = new SharpDX.Vector4(c3.R / 255.0f, c3.G / 255.0f, c3.B / 255.0f, c3.A / 255.0f);

			this.vertices[this.vertexCount + 3].Position = new SharpDX.Vector4(v4.X, v4.Y, 0.5f, 1);
			this.vertices[this.vertexCount + 3].Color = new SharpDX.Vector4(c4.R / 255.0f, c4.G / 255.0f, c4.B / 255.0f, c4.A / 255.0f);
						
			this.vertices[this.vertexCount].UV = this.CalculateUV(source.Left, source.Top);
			this.vertices[this.vertexCount + 1].UV = this.CalculateUV(source.Right, source.Top);
			this.vertices[this.vertexCount + 2].UV = this.CalculateUV(source.Right, source.Bottom);
			this.vertices[this.vertexCount + 3].UV = this.CalculateUV(source.Left, source.Bottom);

			this.vertexCount += 4;
		}

		public void DrawFilledRectangle(Rectangle rectangle, Color color)
		{
			this.SetPixelTexture();
		
			this.AddQuad(
				new Vector2(rectangle.X, rectangle.Y),
				color,
				new Vector2(rectangle.X + rectangle.Width, rectangle.Y),
				color,
				new Vector2(rectangle.X + rectangle.Width, rectangle.Y + rectangle.Height),
				color,
				new Vector2(rectangle.X, rectangle.Y + rectangle.Height),
				color,
				this.pixelSource);
		}

		public void DrawFilledRectangle(Vector2 topLeft, Vector2 bottomRight, Color color)
		{
			this.SetPixelTexture();

			this.AddQuad(
				topLeft,
				color,
				new Vector2(bottomRight.X, topLeft.Y),
				color,
				bottomRight,
				color,
				new Vector2(topLeft.X, bottomRight.Y),
				color,
				this.pixelSource);
		}

		public void DrawRectangle(Rectangle rectangle, Color color)
		{
			this.DrawFilledRectangle(new Vector2(rectangle.Left, rectangle.Top), new Vector2(rectangle.Right, rectangle.Top), color);
			this.DrawFilledRectangle(new Vector2(rectangle.Right, rectangle.Top), new Vector2(rectangle.Right, rectangle.Bottom), color);
			this.DrawFilledRectangle(new Vector2(rectangle.Right, rectangle.Bottom), new Vector2(rectangle.Left, rectangle.Bottom), color);
			this.DrawFilledRectangle(new Vector2(rectangle.Left, rectangle.Bottom), new Vector2(rectangle.Left, rectangle.Top), color);
		}

		public void DrawTexture(Texture texture, Vector2 position, Color color)
		{
			this.DrawTexture(texture, position, new Rectangle(0, 0, texture.Width, texture.Height), color);
		}

		public void DrawTexture(Texture texture, Vector2 position, Rectangle source, Color color)
		{
			if (texture == null)
				throw new ArgumentNullException("texture");

			this.SetTexture(texture.InternalTexture, texture.InternalWidth, texture.InternalHeight);

			this.AddQuad(
				new Vector2(position.X, position.Y),
				color,
				new Vector2(position.X + source.Width, position.Y),
				color,
				new Vector2(position.X + source.Width, position.Y + source.Height),
				color,
				new Vector2(position.X, position.Y + source.Height),
				color,
				source);
		}

		public void DrawTexture(Texture texture, Rectangle destination, Color color)
		{
			this.DrawTexture(texture, destination, new Rectangle(0, 0, texture.Width, texture.Height), color);
		}

		public void DrawTexture(Texture texture, Rectangle destination, Rectangle source, Color color)
		{
			if (texture == null)
				throw new ArgumentNullException("texture");

			this.SetTexture(texture.InternalTexture, texture.InternalWidth, texture.InternalHeight);
						
			this.AddQuad(
				new Vector2(destination.X, destination.Y),
				color,
				new Vector2(destination.X + destination.Width, destination.Y),
				color,
				new Vector2(destination.X + destination.Width, destination.Y + destination.Height),
				color,
				new Vector2(destination.X, destination.Y + destination.Height),
				color,
				source);
		}

		public void DrawTexture(Texture texture, Matrix transform, Color color)
		{
			this.DrawTexture(texture, new Rectangle(0, 0, texture.Width, texture.Height), transform, color);
		}

		public void DrawTexture(Texture texture, Rectangle source, Matrix transform, Color color)
		{
			if (texture == null)
				throw new ArgumentNullException("texture");

			this.SetTexture(texture.InternalTexture, texture.InternalWidth, texture.InternalHeight);
						
			Vector2 v1 = Vector2.Transform(new Vector2(0, 0), transform);
			Vector2 v2 = Vector2.Transform(new Vector2(source.Width, 0), transform);
			Vector2 v3 = Vector2.Transform(new Vector2(source.Width, source.Height), transform);
			Vector2 v4 = Vector2.Transform(new Vector2(0, source.Height), transform);

			this.AddQuad(v1, color, v2, color, v3, color, v4, color, source);
		}

		public void DrawSprite(Sprite sprite)
		{
			if (sprite == null)
				throw new ArgumentNullException("sprite");

			sprite.Draw(this);
		}

		public void DrawSprite(SpriteSheet spriteSheet, int frame, Vector2 position, Color color)
		{
			if (spriteSheet == null)
				throw new ArgumentNullException("spriteSheet");

			this.SetTexture(spriteSheet.Texture.InternalTexture, spriteSheet.Texture.InternalWidth, spriteSheet.Texture.InternalHeight);

			Rectangle frameRect = spriteSheet[frame];

			this.AddQuad(
				new Vector2(position.X, position.Y),
				color,
				new Vector2(position.X + frameRect.Width, position.Y),
				color,
				new Vector2(position.X + frameRect.Width, position.Y + frameRect.Height),
				color,
				new Vector2(position.X, position.Y + frameRect.Height),
				color,
				frameRect);
		}

		public void DrawSprite(SpriteSheet spriteSheet, int frame, Matrix transform, Color color)
		{
			if (spriteSheet == null)
				throw new ArgumentNullException("spriteSheet");

			this.SetTexture(spriteSheet.Texture.InternalTexture, spriteSheet.Texture.InternalWidth, spriteSheet.Texture.InternalHeight);

			Rectangle source = spriteSheet[frame];

			Vector2 v1 = Vector2.Transform(new Vector2(0, 0), transform);
			Vector2 v2 = Vector2.Transform(new Vector2(source.Width, 0), transform);
			Vector2 v3 = Vector2.Transform(new Vector2(source.Width, source.Height), transform);
			Vector2 v4 = Vector2.Transform(new Vector2(0, source.Height), transform);

			this.AddQuad(v1, color, v2, color, v3, color, v4, color, source);
		}

		private void EnsureDrawStringParams(TextureFont font, string text)
		{
			if (font == null)
				throw new ArgumentNullException("font");

			if (text == null)
				throw new ArgumentNullException("text");
		}

		public void DrawString(TextureFont font, string text, Vector2 position, Color color)
		{
			this.DrawString(font, text, position, Vector2.One, color);
		}

		public void DrawString(TextureFont font, string text, Vector2 position, Vector2 scale, Color color)
		{
			this.EnsureDrawStringParams(font, text);

			Size textSize = font.MeasureString(text);

			this.DrawString(font, text, new Rectangle((int)position.X, (int)position.Y, textSize.Width, textSize.Height), scale, color);
		}

		public void DrawString(TextureFont font, string text, Rectangle destination, Color color)
		{			
			this.DrawString(font, text, destination, Vector2.One, color);
		}

		public void DrawString(TextureFont font, string text, Rectangle destination, Vector2 scale, Color color)
		{
			this.EnsureDrawStringParams(font, text);

			if (text.Length == 0)
				return;

			float heightOfSingleLine = font.LineHeight * scale.Y;

			if (heightOfSingleLine > destination.Height) // We can't draw anything
				return;

			Vector2 cursor = new Vector2(destination.X, destination.Y);

			for (int i = 0; i < text.Length; i++)
			{
				// Skip characters we can't render.
				if (text[i] == '\r')
					continue;

				float widthOfChar = 0;

				if (text[i] == '\n' || cursor.X + (widthOfChar =  font[text[i]].Width * scale.X) > destination.Right)
				{
					cursor.X = destination.X;
					cursor.Y += heightOfSingleLine + font.LineSpacing;

					// If the next line extends past the destination, quit.
					if (cursor.Y + heightOfSingleLine > destination.Bottom)
						return;

					// We can't render a new line.
					if (text[i] == '\n')
						continue;
				}

				Rectangle letterSource = font[text[i]];
				Rectangle letterDestination = new Rectangle((int)cursor.X, (int)cursor.Y, (int)widthOfChar, (int)heightOfSingleLine);

				this.DrawTexture(font.Texture, letterDestination, letterSource, color);

				cursor.X += widthOfChar + font.CharacterSpacing;
			}
		}

		public void DrawTextBlock(TextBlock textBlock, Vector2 position, Color color)
		{
			if (textBlock == null)
				throw new ArgumentNullException("textBlock");

			for (int i = 0; i < textBlock.Length; i++)
			{
				TextBlock.Character character = textBlock[i];
				
				Rectangle destination = character.Destination;
				destination.X += (int)position.X;
				destination.Y += (int)position.Y;
				
				this.DrawTexture(textBlock.Font.Texture, destination, character.Source, color);
			}
		}

		public void Flush()
		{
			this.EnsureHasBegun();

			if (this.vertexCount > 0)
			{
				this.GraphicsDevice.d3d9Device.VertexDeclaration = this.vertexDeclaration;

				this.effect.Begin(this.effectTechnique, this.effectPass);
				this.effect.TransformMatrix = transformMatrix;

				if (this.texture != null)
				{
					this.GraphicsDevice.d3d9Device.SetTexture(0, this.texture);
				}
				else
				{
					this.GraphicsDevice.d3d9Device.SetTexture(0, null);
				}

				this.GraphicsDevice.d3d9Device.DrawIndexedUserPrimitives<short, Vertex>(
					D3D.PrimitiveType.TriangleList,
					0,
					this.vertexCount,
					(this.vertexCount / 4) * 2,
					this.indices,
					D3D.Format.Index16,
					this.vertices);

				this.effect.End();

				this.vertexCount = 0;
			}

			this.texture = null;
		}
	}
}
