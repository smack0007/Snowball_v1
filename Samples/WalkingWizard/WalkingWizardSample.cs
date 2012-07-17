using System;
using Snowball;
using Snowball.Content;
using Snowball.Graphics;
using Snowball.Input;

namespace WalkingWizard
{
	public class WalkingWizardSample : Game
	{
		GraphicsDevice graphicsDevice;
		GraphicsBatch graphics;
		Keyboard keyboard;
		ContentLoader content;
		
		Sprite sprite;

		int animationOffset;
		float animationTimer;
		int animationFrame;
		
		public WalkingWizardSample()
			: base()
		{
			this.Window.Title = "Snowball Walking Wizard Sample";

			this.graphicsDevice = new GraphicsDevice(this.Window);
			
			// Add the GraphicsDevice to the list of services. This is how ContentLoader finds the GraphicsDevice.
			this.Services.AddService(typeof(IGraphicsDevice), this.graphicsDevice);

			this.keyboard = new Keyboard();

			this.content = new ContentLoader(this.Services);
		}
				
		protected override void Initialize()
		{
			this.graphicsDevice.CreateDevice(800, 600);

			// GraphicsBatch must be created after the graphics device is created.
			this.graphics = new GraphicsBatch(this.graphicsDevice);
						
			this.animationOffset = 1;
		
			// Load a texture and wrap it in a SpriteSheet. The sheet contains frames which are 32x32.
			SpriteSheet spriteSheet = this.content.Load<SpriteSheet>(new LoadSpriteSheetArgs()
			{
				FileName = "wizard.png",
				FrameWidth = 32,
				FrameHeight = 32
			});

			this.sprite = new Sprite(spriteSheet);
			this.sprite.Position = new Vector2(this.Window.DisplayWidth / 2, this.Window.DisplayHeight / 2);
			this.sprite.Origin = new Vector2(16, 16);
		}
		
		protected override void Update(GameTime gameTime)
		{
			base.Update(gameTime);

			// Update the keyboard.
			this.keyboard.Update();

			// Run logic to move the sprite on the screen.

			Vector2 delta = Vector2.Zero;

			if (this.keyboard.IsKeyDown(Keys.Left))
				delta.X -= 100.0f * gameTime.ElapsedTotalSeconds;

			if (this.keyboard.IsKeyDown(Keys.Right))
				delta.X += 100.0f * gameTime.ElapsedTotalSeconds;

			if (this.keyboard.IsKeyDown(Keys.Up))
				delta.Y -= 100.0f * gameTime.ElapsedTotalSeconds;

			if (this.keyboard.IsKeyDown(Keys.Down))
				delta.Y += 100.0f * gameTime.ElapsedTotalSeconds;

			this.sprite.Position += delta;

			this.animationTimer += gameTime.ElapsedTotalSeconds;
			if (this.animationTimer >= 0.5f)
			{
				this.animationTimer -= 0.5f;

				this.animationFrame++;
				if (this.animationFrame > 1)
					this.animationFrame = 0;
			}

			if (delta.Y != 0.0f)
			{
				if (delta.Y < 0.0f)
				{
					this.animationOffset = 3;
				}
				else
				{
					this.animationOffset = 1;
				}
			}
			else if (delta.X != 0.0f)
			{
				if (delta.X < 0.0f)
				{
					this.animationOffset = 2;
				}
				else
				{
					this.animationOffset = 0;
				}
			}

			this.sprite.Frame = animationOffset + (this.animationFrame * 4);
		}

		protected override void Draw(GameTime gameTime)
		{
			this.graphicsDevice.Clear(new Color(192, 192, 192, 255));

			if (this.graphicsDevice.BeginDraw())
			{
				// Draw the single sprite.
				this.graphics.Begin();
				this.graphics.DrawSprite(this.sprite);
				this.graphics.End();

				this.graphicsDevice.EndDraw();

				this.graphicsDevice.Present();
			}
		}

		public static void Main()
		{
			using(WalkingWizardSample sample = new WalkingWizardSample())
				sample.Run();
		}
	}
}
