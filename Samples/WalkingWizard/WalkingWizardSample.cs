using System;
using Snowball;
using Snowball.Content;
using Snowball.Graphics;
using Snowball.Input;

namespace WalkingWizard
{
	public class WalkingWizardSample : Game
	{
		Renderer renderer;
		KeyboardDevice keyboard;
		ContentLoader content;
		
		Sprite sprite;

		int animationOffset;
		float animationTimer;
		int animationFrame;
		
		public WalkingWizardSample()
			: base()
		{
			this.Window.Title = "Snowball Walking Wizard Sample";

			this.keyboard = new KeyboardDevice();

			this.content = new ContentLoader(this.Services);
		}

		protected override void Initialize()
		{
			base.Initialize();

			// Renderer must be created after the Graphics Device has been created.
			this.Graphics.CreateDevice(this.Window);
			this.renderer = new Renderer(this.Graphics);

			// Load a texture and wrap it in a SpriteSheet. The shee contains frame which are 32x32.
			this.content.Register<SpriteSheet>("wizard", new LoadSpriteSheetArgs()
			{
				FileName = "wizard.png",
				FrameWidth = 32,
				FrameHeight = 32
			});

			SpriteSheet spriteSheet = this.content.Load<SpriteSheet>("wizard");
			
			this.sprite = new Sprite(spriteSheet);
			this.sprite.Position = new Vector2(this.Window.ClientWidth / 2, this.Window.ClientHeight / 2);
			this.sprite.Origin = new Vector2(16, 16);

			this.animationOffset = 1;
		}

		protected override void Update(GameTime gameTime)
		{
			base.Update(gameTime);

			// Update the keyboard.
			this.keyboard.Update(gameTime);

			// Run logic to move the sprite on the screen.

			Vector2 delta = Vector2.Zero;

			if(this.keyboard.IsKeyDown(Keys.Left))
				delta.X -= 100.0f * gameTime.ElapsedTotalSeconds;

			if(this.keyboard.IsKeyDown(Keys.Right))
				delta.X += 100.0f * gameTime.ElapsedTotalSeconds;

			if(this.keyboard.IsKeyDown(Keys.Up))
				delta.Y -= 100.0f * gameTime.ElapsedTotalSeconds;

			if(this.keyboard.IsKeyDown(Keys.Down))
				delta.Y += 100.0f * gameTime.ElapsedTotalSeconds;

			this.sprite.Position += delta;

			this.animationTimer += gameTime.ElapsedTotalSeconds;
			if(this.animationTimer >= 0.5f)
			{
				this.animationTimer -= 0.5f;

				this.animationFrame++;
				if(this.animationFrame > 1)
					this.animationFrame = 0;
			}

			if(delta.Y != 0.0f)
			{
				if(delta.Y < 0.0f)
				{
					this.animationOffset = 3;
				}
				else
				{
					this.animationOffset = 1;
				}
			}
			else if(delta.X != 0.0f)
			{
				if(delta.X < 0.0f)
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
			base.Draw(gameTime);

			// Clear the backbuffer and begin drawing.
			this.Graphics.Clear(new Color(192, 192, 192, 255));
			this.Graphics.BeginDraw();

			// Draw the single sprite.
			this.renderer.Begin();
			this.renderer.DrawSprite(this.sprite);
			this.renderer.End();

			// End drawing and present the backbuffer.
			this.Graphics.EndDraw();
			this.Graphics.Present();
		}

		public static void Main()
		{
			using(WalkingWizardSample sample = new WalkingWizardSample())
				sample.Run();
		}
	}
}
