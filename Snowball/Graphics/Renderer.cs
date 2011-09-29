using System;
using System.Runtime.InteropServices;
using SlimDX;
using SlimDX.Direct3D9;

namespace Snowball.Graphics
{
	public class Renderer : IRenderer
	{
		struct Vertex
		{
			public SlimDX.Vector3 Position;
			public int Color;
			public SlimDX.Vector2 UV;
		}

		enum RendererMode
		{
			None = 0,
			Lines,
			Quads,
			TexturedQuads
		}

		Device graphicsDevice;
		VertexDeclaration vertexDeclaration;

		RendererSettings settings;
		RendererMode mode;
		Vertex[] vertices;
		int vertexCount;
		short[] indices;
		
		SlimDX.Direct3D9.Texture texture;
		int textureWidth;
		int textureHeight;

		Matrix[] matrixStack;
		int matrixStackCount;

		Color[] colorStack;
		int colorStackCount;

		public bool HasBegun
		{
			get;
			private set;
		}

		public Renderer(IGraphicsManager graphicsManager)
			: this(graphicsManager, RendererSettings.Default)
		{
		}

		public Renderer(IGraphicsManager graphicsManager, RendererSettings settings)
		{
			if(graphicsManager == null)
				throw new ArgumentNullException("graphicsManager");

			if(!graphicsManager.IsDeviceCreated)
				throw new InvalidOperationException("Graphics device not yet created.");
			
			if(graphicsManager is GraphicsManager)
				this.graphicsDevice = ((GraphicsManager)graphicsManager).GraphicsDevice;

			if(this.graphicsDevice != null)
			{
				this.vertexDeclaration = new VertexDeclaration(this.graphicsDevice, new[] {
        			new VertexElement(0, 0, DeclarationType.Float3, DeclarationMethod.Default, DeclarationUsage.PositionTransformed, 0),
        			new VertexElement(0, 12, DeclarationType.Color, DeclarationMethod.Default, DeclarationUsage.Color, 0),
					new VertexElement(0, 16, DeclarationType.Float2, DeclarationMethod.Default, DeclarationUsage.TextureCoordinate, 0),
					VertexElement.VertexDeclarationEnd
        		});
			}
						
			this.settings = settings;
			this.mode = RendererMode.None;
			this.vertices = new Vertex[settings.VertexBufferSize * 4];
			this.vertexCount = 0;
			this.indices = new short[settings.VertexBufferSize * 6];
			this.texture = null;
									
			for(short i = 0, vertex = 0; i < this.indices.Length; i += 6, vertex += 4)
			{
				this.indices[i] = vertex;
				this.indices[i + 1] = (short)(vertex + 1);
				this.indices[i + 2] = (short)(vertex + 2);
				this.indices[i + 3] = vertex;
				this.indices[i + 4] = (short)(vertex + 2);
				this.indices[i + 5] = (short)(vertex + 3);
			}

			this.matrixStack = new Matrix[settings.MatrixStackSize];
			this.matrixStackCount = 0;

			this.colorStack = new Color[settings.ColorStackSize];
			this.colorStackCount = 0;
		}

		public void Begin()
		{
			if(this.HasBegun)
				throw new InvalidOperationException("Already within Begin / End pair.");

			if(this.graphicsDevice != null)
			{
				this.graphicsDevice.SetRenderState(RenderState.AlphaBlendEnable, true);
				this.graphicsDevice.SetRenderState<Blend>(RenderState.SourceBlend, Blend.SourceAlpha);
				this.graphicsDevice.SetRenderState<Blend>(RenderState.DestinationBlend, Blend.InverseSourceAlpha);
				this.graphicsDevice.SetRenderState<BlendOperation>(RenderState.BlendOperation, BlendOperation.Add);

				this.graphicsDevice.SetTextureStageState(0, TextureStage.ColorOperation, TextureOperation.Modulate);
				this.graphicsDevice.SetTextureStageState(0, TextureStage.ColorArg1, TextureArgument.Texture);
				this.graphicsDevice.SetTextureStageState(0, TextureStage.ColorArg2, TextureArgument.Diffuse);
				this.graphicsDevice.SetTextureStageState(0, TextureStage.AlphaOperation, TextureOperation.Modulate);
			}

			this.HasBegun = true;
		}

		private void EnsureHasBegun()
		{
			if(!this.HasBegun)
				throw new InvalidOperationException("Not within Begin / End pair.");
		}

		public void End()
		{
			this.EnsureHasBegun();

			this.Flush();

			this.matrixStackCount = 0;
			this.colorStackCount = 0;

			this.HasBegun = false;
		}

		public void PushMatrix(Matrix matrix)
		{
			this.EnsureHasBegun();

			if(this.matrixStackCount == this.matrixStack.Length)
				throw new InvalidOperationException("Matrix stack full.");

			this.matrixStack[this.matrixStackCount] = matrix;
			this.matrixStackCount++;
		}

		public void PopMatrix()
		{
			this.EnsureHasBegun();

			if(this.matrixStackCount <= 0)
				throw new InvalidOperationException("Matrix stack empty.");

			this.matrixStackCount--;
		}

		public void PushColor(Color color)
		{
			this.EnsureHasBegun();

			if(this.colorStackCount == this.colorStack.Length)
				throw new InvalidOperationException("Color stack full.");

			this.colorStack[this.colorStackCount] = color;
			this.colorStackCount++;
		}

		public void PopColor()
		{
			this.EnsureHasBegun();

			if(this.colorStackCount <= 0)
				throw new InvalidOperationException("Color stack empty.");

			this.colorStackCount--;
		}
		
		private void EnsureMode(RendererMode mode)
		{
			this.EnsureHasBegun();

			if(mode != this.mode)
				Flush();
						
			this.mode = mode;

			if(this.mode != RendererMode.TexturedQuads)
				this.texture = null;
		}

		private Vector2 Transform(Vector2 input)
		{
			for(int i = 0; i < this.matrixStackCount; i++)
				input = Vector2.Transform(input, this.matrixStack[i]);
						
			return input;
		}

		private Color Transform(Color input)
		{
			for(int i = 0; i < this.colorStackCount; i++)
			{
				if(this.settings.ColorStackFunction == ColorFunction.Limit)
					input = input.Limit(this.colorStack[i]);
			}
			
			return input;
		}

		public void DrawLine(Vector2 v1, Vector2 v2, Color color)
		{
			this.EnsureMode(RendererMode.Lines);

			if(this.vertexCount >= this.vertices.Length)
				this.Flush();

			v1 = this.Transform(v1);
			v2 = this.Transform(v2);
			color = this.Transform(color);
	
			this.vertices[this.vertexCount].Position = new SlimDX.Vector3(v1.X, v1.Y, 0.5f);
			this.vertices[this.vertexCount].Color = color.ToArgb();

			this.vertices[this.vertexCount + 1].Position = new SlimDX.Vector3(v2.X, v2.Y, 0.5f);
			this.vertices[this.vertexCount + 1].Color = color.ToArgb();

			this.vertexCount += 2;
		}

		private void EnsureTexture(SlimDX.Direct3D9.Texture texture, int width, int height)
		{
			if(texture != this.texture)
				Flush();

			this.texture = texture;
			this.textureWidth = width;
			this.textureHeight = height;
		}

		private SlimDX.Vector2 CalculateUV(float x, float y)
		{
			return new SlimDX.Vector2(x / (float)this.textureWidth, y / (float)this.textureHeight);
		}
				
		private void AddQuad(Vector2 v1, Color c1, Vector2 v2, Color c2,
							 Vector2 v3, Color c3, Vector2 v4, Color c4)
		{
			this.AddQuad(v1, c1, v2, c2, v3, c3, v4, c4, null, null);
		}

		private void AddQuad(Vector2 v1, Color c1, Vector2 v2, Color c2,
			                 Vector2 v3, Color c3, Vector2 v4, Color c4,
			                 SlimDX.Direct3D9.Texture texture, Rectangle? source)
		{
			if(this.vertexCount >= this.vertices.Length)
				this.Flush();

			if(texture != null && source == null)
				throw new ArgumentNullException("source", "Source rectangle cannot be null when texture is not null.");

			v1 = this.Transform(v1);
			v2 = this.Transform(v2);
			v3 = this.Transform(v3);
			v4 = this.Transform(v4);

			c1 = this.Transform(c1);
			c2 = this.Transform(c2);
			c3 = this.Transform(c3);
			c4 = this.Transform(c4);

			this.vertices[this.vertexCount].Position = new SlimDX.Vector3((int)v1.X - 0.5f, (int)v1.Y - 0.5f, 0.5f);
			this.vertices[this.vertexCount].Color = c1.ToArgb();

			this.vertices[this.vertexCount + 1].Position = new SlimDX.Vector3((int)v2.X - 0.5f, (int)v2.Y - 0.5f, 0.5f);
			this.vertices[this.vertexCount + 1].Color = c2.ToArgb();

			this.vertices[this.vertexCount + 2].Position = new SlimDX.Vector3((int)v3.X - 0.5f, (int)v3.Y - 0.5f, 0.5f);
			this.vertices[this.vertexCount + 2].Color = c3.ToArgb();

			this.vertices[this.vertexCount + 3].Position = new SlimDX.Vector3((int)v4.X - 0.5f, (int)v4.Y - 0.5f, 0.5f);
			this.vertices[this.vertexCount + 3].Color = c4.ToArgb();

			if(texture != null)
			{
				this.vertices[this.vertexCount].UV = this.CalculateUV(source.Value.Left, source.Value.Top);
				this.vertices[this.vertexCount + 1].UV = this.CalculateUV(source.Value.Right, source.Value.Top);
				this.vertices[this.vertexCount + 2].UV = this.CalculateUV(source.Value.Right, source.Value.Bottom);
				this.vertices[this.vertexCount + 3].UV = this.CalculateUV(source.Value.Left, source.Value.Bottom);
			}

			this.vertexCount += 4;
		}

		public void DrawFilledRectangle(Rectangle rectangle, Color color)
		{
			this.EnsureMode(RendererMode.Quads);

			this.AddQuad(new Vector2(rectangle.X, rectangle.Y), color,
						 new Vector2(rectangle.X + rectangle.Width, rectangle.Y), color,
						 new Vector2(rectangle.X + rectangle.Width, rectangle.Y + rectangle.Height), color,
						 new Vector2(rectangle.X, rectangle.Y + rectangle.Height), color);
		}

		public void DrawRectangle(Rectangle rectangle, Color color)
		{
			this.DrawLine(new Vector2(rectangle.Left, rectangle.Top), new Vector2(rectangle.Right, rectangle.Top), color);
			this.DrawLine(new Vector2(rectangle.Right, rectangle.Top), new Vector2(rectangle.Right, rectangle.Bottom), color);
			this.DrawLine(new Vector2(rectangle.Right, rectangle.Bottom), new Vector2(rectangle.Left, rectangle.Bottom), color);
			this.DrawLine(new Vector2(rectangle.Left, rectangle.Bottom), new Vector2(rectangle.Left, rectangle.Top), color);
		}

		public void DrawRectangle(RotatableRectangle rectangle, Color color)
		{
			this.DrawLine(rectangle.TopLeft, rectangle.TopRight, color);
			this.DrawLine(rectangle.TopRight, rectangle.BottomRight, color);
			this.DrawLine(rectangle.BottomRight, rectangle.BottomLeft, color);
			this.DrawLine(rectangle.BottomLeft, rectangle.TopLeft, color);
		}

		public void DrawTexture(Texture texture, Vector2 position, Color color)
		{
			this.EnsureMode(RendererMode.TexturedQuads);
			
			this.EnsureTexture(texture.texture, texture.Width, texture.Height);

			this.AddQuad(new Vector2(position.X, position.Y), color,
						 new Vector2(position.X + texture.Width, position.Y), color,
						 new Vector2(position.X + texture.Width, position.Y + texture.Height), color,
						 new Vector2(position.X, position.Y + texture.Height), color,
						 texture.texture, new Rectangle(0, 0, texture.Width, texture.Height));
		}

		public void DrawTexture(Texture texture, Rectangle destination, Rectangle? source, Color color)
		{
			this.EnsureMode(RendererMode.TexturedQuads);

			this.EnsureTexture(texture.texture, texture.Width, texture.Height);

			if(source == null)
				source = new Rectangle(0, 0, texture.Width, texture.Height);

			this.AddQuad(new Vector2(destination.X, destination.Y), color,
						 new Vector2(destination.X + destination.Width, destination.Y), color,
						 new Vector2(destination.X + destination.Width, destination.Y + destination.Height), color,
						 new Vector2(destination.X, destination.Y + destination.Height), color,
						 texture.texture, source);
		}

		public void DrawSprite(Sprite sprite)
		{
			sprite.Draw(this);
		}

		public void DrawSprite(SpriteSheet spriteSheet, int frame, Vector2 position, Color color)
		{
			this.EnsureMode(RendererMode.TexturedQuads);

			this.EnsureTexture(spriteSheet.Texture.texture, spriteSheet.Texture.Width, spriteSheet.Texture.Height);

			Rectangle frameRect = spriteSheet[frame];

			this.AddQuad(new Vector2(position.X, position.Y), color,
						 new Vector2(position.X + frameRect.Width, position.Y), color,
						 new Vector2(position.X + frameRect.Width, position.Y + frameRect.Height), color,
						 new Vector2(position.X, position.Y + frameRect.Height), color,
						 texture, frameRect);
		}

		public void DrawSprite(SpriteSheet spriteSheet, int frame, Matrix transform, Color color)
		{
			this.EnsureMode(RendererMode.TexturedQuads);

			this.EnsureTexture(spriteSheet.Texture.texture, spriteSheet.Texture.Width, spriteSheet.Texture.Height);

			Rectangle frameRect = spriteSheet[frame];

			Vector2 v1 = Vector2.Transform(new Vector2(0, 0), transform);
			Vector2 v2 = Vector2.Transform(new Vector2(frameRect.Width, 0), transform);
			Vector2 v3 = Vector2.Transform(new Vector2(frameRect.Width, frameRect.Height), transform);
			Vector2 v4 = Vector2.Transform(new Vector2(0, frameRect.Height), transform);

			this.AddQuad(v1, color, v2, color, v3, color, v4, color, texture, frameRect);
		}

		public void DrawString(TextureFont textureFont, string text, Vector2 position, Color color)
		{
			if(textureFont == null)
				throw new ArgumentNullException("textureFont");

			position.X = (int)position.X;
			position.Y = (int)position.Y;

			Vector2 cursor = position;

			for(int i = 0; i < text.Length; i++)
			{
				if(text[i] == '\n')
				{
					cursor.X = (int)position.X;
					cursor.Y += textureFont.LineHeight;
					continue;
				}

				Rectangle source = textureFont[text[i]];
				Rectangle destination = new Rectangle((int)cursor.X, (int)cursor.Y, source.Width, source.Height);

				this.DrawTexture(textureFont.Texture, destination, source, color);

				cursor.X += source.Width + textureFont.CharacterSpacing;
			}
		}

		public void DrawRenderTarget(RenderTarget renderTarget, Vector2 position, Color color)
		{
			this.EnsureMode(RendererMode.TexturedQuads);

			this.EnsureTexture(renderTarget.texture, renderTarget.Width, renderTarget.Height);

			this.AddQuad(new Vector2(position.X, position.Y), color,
						 new Vector2(position.X + renderTarget.Width, position.Y), color,
						 new Vector2(position.X + renderTarget.Width, position.Y + renderTarget.Height), color,
						 new Vector2(position.X, position.Y + renderTarget.Height), color,
						 renderTarget.texture, new Rectangle(0, 0, renderTarget.Width, renderTarget.Height));
		}

		public void DrawRenderTarget(RenderTarget renderTarget, Rectangle destination, Rectangle? source, Color color)
		{
			this.EnsureMode(RendererMode.TexturedQuads);

			this.EnsureTexture(renderTarget.texture, renderTarget.Width, renderTarget.Height);

			if(source == null)
				source = new Rectangle(0, 0, renderTarget.Width, renderTarget.Height);

			this.AddQuad(new Vector2(destination.X, destination.Y), color,
						 new Vector2(destination.X + destination.Width, destination.Y), color,
						 new Vector2(destination.X + destination.Width, destination.Y + destination.Height), color,
						 new Vector2(destination.X, destination.Y + destination.Height), color,
						 renderTarget.texture, source);
		}

		private void Flush()
		{
			if(this.vertexCount > 0)
			{
				if(this.graphicsDevice != null)
				{
					this.graphicsDevice.VertexDeclaration = this.vertexDeclaration;

					if(this.texture != null)
						this.graphicsDevice.SetTexture(0, this.texture);
					else
						this.graphicsDevice.SetTexture(0, null);

					if(this.mode == RendererMode.Quads || this.mode == RendererMode.TexturedQuads)
					{
						this.graphicsDevice.DrawIndexedUserPrimitives<short, Vertex>(PrimitiveType.TriangleList, 0, this.vertexCount, (this.vertexCount / 4) * 2,
																					 this.indices, Format.Index16, this.vertices, Marshal.SizeOf(typeof(Vertex)));
					}
					else if(this.mode == RendererMode.Lines)
					{
						this.graphicsDevice.DrawUserPrimitives<Vertex>(PrimitiveType.LineList, this.vertexCount / 2, this.vertices);
					}
				}

				this.vertexCount = 0;
			}

			this.texture = null;
		}
	}
}
