using System;
using Snowball;
using Snowball.Graphics;
using Snowball.Input;

namespace Snowball.Demo.Gameplay
{
	public class Ship : GameEntity
	{
		IGraphicsManager graphics;
		IRenderer renderer;
		IKeyboardDevice keyboard;

		Sprite sprite;
		float flameTimer;

		public Ship(IGraphicsManager graphics, IRenderer renderer, IKeyboardDevice keyboard)
		{
			if(graphics == null)
				throw new ArgumentNullException("graphics");

			if(renderer == null)
				throw new ArgumentNullException("renderer");

			if(keyboard == null)
				throw new ArgumentNullException("keyboard");

			this.graphics = graphics;
			this.renderer = renderer;
			this.keyboard = keyboard;
		}

		public override void Initialize()
		{
			this.sprite = new Sprite(new SpriteSheet(this.graphics.LoadTexture("Ship.png", Color.Magenta), 80, 80));
			this.sprite.Frame = 1;
			this.sprite.Origin = new Vector2(40, 40);
			this.sprite.Position = new Vector2(this.graphics.DisplayWidth / 2, this.graphics.DisplayHeight - 60);
			this.sprite.AddChild(new Sprite(new SpriteSheet(this.graphics.LoadTexture("ShipFlame.png", null), 16, 16)));
			this.sprite.Children[0].Frame = 0;
			this.sprite.Children[0].Origin = new Vector2(8, 8);
			this.sprite.Children[0].Position = new Vector2(40, 88);

			this.IsInitialized = true;
		}

		public override void Update(GameTime gameTime)
		{
			this.flameTimer += gameTime.ElapsedTotalSeconds;
			if(this.flameTimer >= 0.1f)
			{
				this.sprite.Children[0].Frame++;
				if(this.sprite.Children[0].Frame >= 2)
					this.sprite.Children[0].Frame = 0;

				this.flameTimer -= 0.1f;
			}

			if(this.keyboard.IsKeyDown(Keys.Left))
				this.sprite.X -= 200.0f * gameTime.ElapsedTotalSeconds;
			else if(this.keyboard.IsKeyDown(Keys.Right))
				this.sprite.X += 200.0f * gameTime.ElapsedTotalSeconds;

			if(this.keyboard.IsKeyDown(Keys.Up))
				this.sprite.Y -= 100.0f * gameTime.ElapsedTotalSeconds;
			else if(this.keyboard.IsKeyDown(Keys.Down))
				this.sprite.Y += 100.0f * gameTime.ElapsedTotalSeconds;
		}

		public override void Draw(IRenderer renderer)
		{
			this.renderer.Draw(this.sprite);
		}
	}
}
