﻿using System;
using System.Runtime.InteropServices;

namespace Snowball.Graphics
{
	public sealed class Renderer : IRenderer, IDisposable
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
		
		SlimDX.Direct3D9.VertexDeclaration vertexDeclaration;
				
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

		RendererSettings settings;

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
		public Renderer(GraphicsDevice graphicsDevice)
			: this(graphicsDevice, 1024, 8, 8)
		{
		}
		
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="graphicsDevice"></param>
		/// <param name="vertexBufferSize"></param>
		/// <param name="matrixStackSize"></param>
		/// <param name="colorStackSize"></param>
		public Renderer(GraphicsDevice graphicsDevice, int vertexBufferSize, int matrixStackSize, int colorStackSize)
		{
			if (graphicsDevice == null)
				throw new ArgumentNullException("graphicsDevice");

			if (!graphicsDevice.IsDeviceCreated)
				throw new InvalidOperationException("Graphics device not yet created.");

			this.GraphicsDevice = graphicsDevice;

			this.vertexDeclaration = new SlimDX.Direct3D9.VertexDeclaration(this.GraphicsDevice.InternalDevice, new[] {
        		new SlimDX.Direct3D9.VertexElement(0, 0, SlimDX.Direct3D9.DeclarationType.Float3, SlimDX.Direct3D9.DeclarationMethod.Default, SlimDX.Direct3D9.DeclarationUsage.PositionTransformed, 0),
        		new SlimDX.Direct3D9.VertexElement(0, 12, SlimDX.Direct3D9.DeclarationType.Color, SlimDX.Direct3D9.DeclarationMethod.Default, SlimDX.Direct3D9.DeclarationUsage.Color, 0),
				new SlimDX.Direct3D9.VertexElement(0, 16, SlimDX.Direct3D9.DeclarationType.Float2, SlimDX.Direct3D9.DeclarationMethod.Default, SlimDX.Direct3D9.DeclarationUsage.TextureCoordinate, 0),
				SlimDX.Direct3D9.VertexElement.VertexDeclarationEnd
        	});
			
			this.mode = RendererMode.None;
			this.vertices = new Vertex[vertexBufferSize * 4];
			this.vertexCount = 0;
			this.indices = new short[vertexBufferSize * 6];
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

			this.matrixStack = new Matrix[matrixStackSize];
			this.matrixStackCount = 0;

			this.colorStack = new Color[colorStackSize];
			this.colorStackCount = 0;
		}

		~Renderer()
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
			this.Begin(RendererSettings.Default);
		}

		/// <summary>
		/// Begins rendering.
		/// </summary>
		/// <param name="settings"></param>
		public void Begin(RendererSettings settings)
		{
			if (settings == null)
				throw new ArgumentNullException("settings");

			this.EnsureGraphicsDeviceHasDrawBegun();

			if (this.HasBegun)
				throw new InvalidOperationException("Already within Begin / End pair.");

			this.settings = settings;
			this.ApplyGraphicsState();

			this.HasBegun = true;
		}

		private void ApplyGraphicsState()
		{
			this.GraphicsDevice.InternalDevice.SetRenderState(SlimDX.Direct3D9.RenderState.AlphaBlendEnable, true);
			this.GraphicsDevice.InternalDevice.SetRenderState<SlimDX.Direct3D9.Blend>(SlimDX.Direct3D9.RenderState.SourceBlend, SlimDX.Direct3D9.Blend.SourceAlpha);
			this.GraphicsDevice.InternalDevice.SetRenderState<SlimDX.Direct3D9.Blend>(SlimDX.Direct3D9.RenderState.DestinationBlend, SlimDX.Direct3D9.Blend.InverseSourceAlpha);
			this.GraphicsDevice.InternalDevice.SetRenderState<SlimDX.Direct3D9.BlendOperation>(SlimDX.Direct3D9.RenderState.BlendOperation, SlimDX.Direct3D9.BlendOperation.Add);

			this.GraphicsDevice.InternalDevice.SetTextureStageState(0, SlimDX.Direct3D9.TextureStage.ColorOperation, SlimDX.Direct3D9.TextureOperation.Modulate);
			this.GraphicsDevice.InternalDevice.SetTextureStageState(0, SlimDX.Direct3D9.TextureStage.ColorArg1, SlimDX.Direct3D9.TextureArgument.Texture);
			this.GraphicsDevice.InternalDevice.SetTextureStageState(0, SlimDX.Direct3D9.TextureStage.ColorArg2, SlimDX.Direct3D9.TextureArgument.Diffuse);
			this.GraphicsDevice.InternalDevice.SetTextureStageState(0, SlimDX.Direct3D9.TextureStage.AlphaOperation, SlimDX.Direct3D9.TextureOperation.Modulate);

			this.GraphicsDevice.InternalDevice.SetSamplerState(0, SlimDX.Direct3D9.SamplerState.AddressU, SlimDX.Direct3D9.TextureAddress.Clamp);
			this.GraphicsDevice.InternalDevice.SetSamplerState(0, SlimDX.Direct3D9.SamplerState.AddressV, SlimDX.Direct3D9.TextureAddress.Clamp);
			this.GraphicsDevice.InternalDevice.SetSamplerState(0, SlimDX.Direct3D9.SamplerState.AddressW, SlimDX.Direct3D9.TextureAddress.Clamp);

			if (this.settings.TextureFilter == TextureFilter.Linear)
			{
				this.GraphicsDevice.InternalDevice.SetSamplerState(0, SlimDX.Direct3D9.SamplerState.MinFilter, SlimDX.Direct3D9.TextureFilter.Linear);
				this.GraphicsDevice.InternalDevice.SetSamplerState(0, SlimDX.Direct3D9.SamplerState.MagFilter, SlimDX.Direct3D9.TextureFilter.Linear);
				this.GraphicsDevice.InternalDevice.SetSamplerState(0, SlimDX.Direct3D9.SamplerState.MipFilter, SlimDX.Direct3D9.TextureFilter.Linear);
			}
			else if (this.settings.TextureFilter == TextureFilter.Point)
			{
				this.GraphicsDevice.InternalDevice.SetSamplerState(0, SlimDX.Direct3D9.SamplerState.MinFilter, SlimDX.Direct3D9.TextureFilter.Point);
				this.GraphicsDevice.InternalDevice.SetSamplerState(0, SlimDX.Direct3D9.SamplerState.MagFilter, SlimDX.Direct3D9.TextureFilter.Point);
				this.GraphicsDevice.InternalDevice.SetSamplerState(0, SlimDX.Direct3D9.SamplerState.MipFilter, SlimDX.Direct3D9.TextureFilter.Point);
			}
		}

		private void EnsureHasBegun()
		{
			this.EnsureGraphicsDeviceHasDrawBegun();

			if (!this.HasBegun)
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

		public void PushColor(Color color)
		{
			this.EnsureHasBegun();

			if (this.colorStackCount == this.colorStack.Length)
				throw new InvalidOperationException("Color stack full.");

			this.colorStack[this.colorStackCount] = color;
			this.colorStackCount++;
		}

		public void PopColor()
		{
			this.EnsureHasBegun();

			if (this.colorStackCount <= 0)
				throw new InvalidOperationException("Color stack empty.");

			this.colorStackCount--;
		}
		
		private void EnsureMode(RendererMode mode)
		{
			this.EnsureHasBegun();

			if (mode != this.mode)
				Flush();
						
			this.mode = mode;

			if (this.mode != RendererMode.TexturedQuads)
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
				if (this.settings.ColorStackFunction == ColorFunction.Limit)
					input = input.Limit(this.colorStack[i]);
			}
			
			return input;
		}

		public void DrawLine(Vector2 v1, Vector2 v2, Color color)
		{
			this.EnsureMode(RendererMode.Lines);

			if (this.vertexCount >= this.vertices.Length)
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
			if (texture != this.texture)
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
			if (this.vertexCount >= this.vertices.Length)
				this.Flush();

			if (texture != null && source == null)
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

			if (texture != null)
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
			
			this.EnsureTexture(texture.InternalTexture, texture.Width, texture.Height);

			this.AddQuad(new Vector2(position.X, position.Y), color,
						 new Vector2(position.X + texture.Width, position.Y), color,
						 new Vector2(position.X + texture.Width, position.Y + texture.Height), color,
						 new Vector2(position.X, position.Y + texture.Height), color,
						 texture.InternalTexture, new Rectangle(0, 0, texture.Width, texture.Height));
		}

		public void DrawTexture(Texture texture, Rectangle destination, Rectangle? source, Color color)
		{
			this.EnsureMode(RendererMode.TexturedQuads);

			this.EnsureTexture(texture.InternalTexture, texture.Width, texture.Height);

			if (source == null)
				source = new Rectangle(0, 0, texture.Width, texture.Height);

			this.AddQuad(new Vector2(destination.X, destination.Y), color,
						 new Vector2(destination.X + destination.Width, destination.Y), color,
						 new Vector2(destination.X + destination.Width, destination.Y + destination.Height), color,
						 new Vector2(destination.X, destination.Y + destination.Height), color,
						 texture.InternalTexture, source);
		}

		public void DrawSprite(Sprite sprite)
		{
			sprite.Draw(this);
		}

		public void DrawSprite(SpriteSheet spriteSheet, int frame, Vector2 position, Color color)
		{
			this.EnsureMode(RendererMode.TexturedQuads);

			this.EnsureTexture(spriteSheet.Texture.InternalTexture, spriteSheet.Texture.Width, spriteSheet.Texture.Height);

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

			this.EnsureTexture(spriteSheet.Texture.InternalTexture, spriteSheet.Texture.Width, spriteSheet.Texture.Height);

			Rectangle frameRect = spriteSheet[frame];

			Vector2 v1 = Vector2.Transform(new Vector2(0, 0), transform);
			Vector2 v2 = Vector2.Transform(new Vector2(frameRect.Width, 0), transform);
			Vector2 v3 = Vector2.Transform(new Vector2(frameRect.Width, frameRect.Height), transform);
			Vector2 v4 = Vector2.Transform(new Vector2(0, frameRect.Height), transform);

			this.AddQuad(v1, color, v2, color, v3, color, v4, color, texture, frameRect);
		}

		public void DrawString(TextureFont textureFont, string text, Vector2 position, Color color)
		{
			if (textureFont == null)
				throw new ArgumentNullException("textureFont");

			position.X = (int)position.X;
			position.Y = (int)position.Y;

			Vector2 cursor = position;

			for(int i = 0; i < text.Length; i++)
			{
				if (text[i] == '\n')
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

			this.EnsureTexture(renderTarget.InternalTexture, renderTarget.Width, renderTarget.Height);

			this.AddQuad(new Vector2(position.X, position.Y), color,
						 new Vector2(position.X + renderTarget.Width, position.Y), color,
						 new Vector2(position.X + renderTarget.Width, position.Y + renderTarget.Height), color,
						 new Vector2(position.X, position.Y + renderTarget.Height), color,
						 renderTarget.InternalTexture, new Rectangle(0, 0, renderTarget.Width, renderTarget.Height));
		}

		public void DrawRenderTarget(RenderTarget renderTarget, Rectangle destination, Rectangle? source, Color color)
		{
			this.EnsureMode(RendererMode.TexturedQuads);

			this.EnsureTexture(renderTarget.InternalTexture, renderTarget.Width, renderTarget.Height);

			if (source == null)
				source = new Rectangle(0, 0, renderTarget.Width, renderTarget.Height);

			this.AddQuad(new Vector2(destination.X, destination.Y), color,
						 new Vector2(destination.X + destination.Width, destination.Y), color,
						 new Vector2(destination.X + destination.Width, destination.Y + destination.Height), color,
						 new Vector2(destination.X, destination.Y + destination.Height), color,
						 renderTarget.InternalTexture, source);
		}

		private void Flush()
		{
			if (this.vertexCount > 0)
			{
				this.GraphicsDevice.InternalDevice.VertexDeclaration = this.vertexDeclaration;

				if (this.texture != null)
					this.GraphicsDevice.InternalDevice.SetTexture(0, this.texture);
				else
					this.GraphicsDevice.InternalDevice.SetTexture(0, null);

				if (this.mode == RendererMode.Quads || this.mode == RendererMode.TexturedQuads)
				{
					this.GraphicsDevice.InternalDevice.DrawIndexedUserPrimitives<short, Vertex>(SlimDX.Direct3D9.PrimitiveType.TriangleList, 0, this.vertexCount, (this.vertexCount / 4) * 2,
																					this.indices, SlimDX.Direct3D9.Format.Index16, this.vertices, Marshal.SizeOf(typeof(Vertex)));
				}
				else if (this.mode == RendererMode.Lines)
				{
					this.GraphicsDevice.InternalDevice.DrawUserPrimitives<Vertex>(SlimDX.Direct3D9.PrimitiveType.LineList, this.vertexCount / 2, this.vertices);
				}

				this.vertexCount = 0;
			}

			this.texture = null;
		}
	}
}
