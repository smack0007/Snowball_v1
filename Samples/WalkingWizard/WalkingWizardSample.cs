﻿using System;
using Snowball;
using Snowball.Graphics;
using Snowball.Input;

namespace WalkingWizard
{
	public class WalkingWizardSample : Game
	{
		GraphicsManager graphics;
		Renderer renderer;
		KeyboardDevice keyboard;

		Sprite sprite;

		int animationOffset;
		float animationTimer;
		int animationFrame;
		
		public WalkingWizardSample()
			: base()
		{
			this.Window.Title = "Snowball Walking Wizard Sample";

			this.graphics = new GraphicsManager();

			this.keyboard = new KeyboardDevice();
		}

		public override void Initialize()
		{
			base.Initialize();

			this.graphics.CreateDevice(this.Window);
			this.renderer = new Renderer(this.graphics);

			this.sprite = new Sprite(new SpriteSheet(this.graphics.LoadTexture("wizard.png", null), 32, 32));
			this.sprite.Position = new Vector2(this.Window.ClientWidth / 2, this.Window.ClientHeight / 2);
			this.sprite.Origin = new Vector2(16, 16);

			this.animationOffset = 1;
		}

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);

			this.keyboard.Update(gameTime);

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

		public override void Draw(GameTime gameTime)
		{
			base.Draw(gameTime);

			this.graphics.Clear(new Color(192, 192, 192, 255));
			this.graphics.BeginDraw();

			this.renderer.Begin();
			this.renderer.DrawSprite(this.sprite);
			this.renderer.End();

			this.graphics.EndDraw();
			this.graphics.Present();
		}

		public static void Main()
		{
			using(WalkingWizardSample sample = new WalkingWizardSample())
				sample.Run();
		}
	}
}