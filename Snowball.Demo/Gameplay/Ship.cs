using System;
using Snowball;
using Snowball.Content;
using Snowball.Graphics;
using Snowball.Input;
using System.IO;

namespace Snowball.Demo.Gameplay
{
	public class Ship : GameComponent
	{
		[GameComponentDependency]
		public IGraphicsDevice Graphics
		{
			get;
			private set;
		}
		
		[GameComponentDependency]
		public IKeyboardDevice Keyboard
		{
			get;
			private set;
		}
				
		Sprite sprite;
		float flameTimer;

		public Ship(IServiceProvider services)
			: base(services)
		{
		}

		public override void Initialize()
		{
			base.Initialize();

			this.sprite = new Sprite(new SpriteSheet(this.Graphics.LoadTexture(File.OpenRead("Ship.png"), Color.Magenta), 80, 80));
			this.sprite.Frame = 1;
			this.sprite.Origin = new Vector2(40, 40);
			this.sprite.Position = new Vector2(this.Graphics.DisplayWidth / 2, this.Graphics.DisplayHeight - 60);
			this.sprite.AddChild(new Sprite(new SpriteSheet(this.Graphics.LoadTexture(File.OpenRead("ShipFlame.png"), null), 16, 16)));
			this.sprite.Children[0].Frame = 0;
			this.sprite.Children[0].Origin = new Vector2(8, 8);
			this.sprite.Children[0].Position = new Vector2(40, 88);
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

			if(this.Keyboard.IsKeyDown(Keys.Left))
				this.sprite.X -= 200.0f * gameTime.ElapsedTotalSeconds;
			else if(this.Keyboard.IsKeyDown(Keys.Right))
				this.sprite.X += 200.0f * gameTime.ElapsedTotalSeconds;

			if(this.Keyboard.IsKeyDown(Keys.Up))
				this.sprite.Y -= 100.0f * gameTime.ElapsedTotalSeconds;
			else if(this.Keyboard.IsKeyDown(Keys.Down))
				this.sprite.Y += 100.0f * gameTime.ElapsedTotalSeconds;
		}

		public override void Draw(IRenderer renderer)
		{
			renderer.DrawSprite(this.sprite);
		}
	}
}
