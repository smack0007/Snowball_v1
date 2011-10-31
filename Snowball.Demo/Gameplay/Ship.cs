using System;
using Snowball;
using Snowball.Content;
using Snowball.Graphics;
using Snowball.Input;
using Snowball.Sound;

namespace Snowball.Demo.Gameplay
{
	public class Ship : GameComponent
	{
		GamePadDevice gamePad;

		[GameComponentDependency]
		public IGraphicsDevice Graphics
		{
			get;
			private set;
		}

		[GameComponentDependency]
		public IContentLoader ContentLoader
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

		SoundEffect blasterSoundEffect;

		public Ship(IServiceProvider services, GamePadDevice gamePad)
			: base(services)
		{
			this.gamePad = gamePad;
		}

		public override void Initialize()
		{
			base.Initialize();

			this.sprite = new Sprite(this.ContentLoader.Load<SpriteSheet>("Ship"));
			this.sprite.Frame = 1;
			this.sprite.Origin = new Vector2(40, 40);
			this.sprite.Position = new Vector2(this.Graphics.DisplayWidth / 2, this.Graphics.DisplayHeight - 60);
			this.sprite.AddChild(new Sprite(this.ContentLoader.Load<SpriteSheet>("ShipFlame")));
			this.sprite.Children[0].Frame = 0;
			this.sprite.Children[0].Origin = new Vector2(8, 8);
			this.sprite.Children[0].Position = new Vector2(40, 88);

			this.blasterSoundEffect = this.ContentLoader.Load<SoundEffect>("Blaster");
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

			Vector2 movement = this.gamePad.LeftThumbStick;
			this.sprite.X += 200.0f * movement.X * gameTime.ElapsedTotalSeconds;
			this.sprite.Y += 200.0f * movement.Y * gameTime.ElapsedTotalSeconds;

			//if(this.Keyboard.IsKeyDown(Keys.Left))
			//    this.sprite.X -= 200.0f * gameTime.ElapsedTotalSeconds;
			//else if(this.Keyboard.IsKeyDown(Keys.Right))
			//    this.sprite.X += 200.0f * gameTime.ElapsedTotalSeconds;

			//if(this.Keyboard.IsKeyDown(Keys.Up))
			//    this.sprite.Y -= 100.0f * gameTime.ElapsedTotalSeconds;
			//else if(this.Keyboard.IsKeyDown(Keys.Down))
			//    this.sprite.Y += 100.0f * gameTime.ElapsedTotalSeconds;

			if(this.Keyboard.IsKeyPressed(Keys.Space))
				this.blasterSoundEffect.Play();
		}

		public override void Draw(IRenderer renderer)
		{
			renderer.DrawSprite(this.sprite);
		}
	}
}
