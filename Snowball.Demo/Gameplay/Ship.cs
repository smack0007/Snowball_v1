using System;
using Snowball;
using Snowball.Content;
using Snowball.GameFramework;
using Snowball.Graphics;
using Snowball.Input;
using Snowball.Sound;

namespace Snowball.Demo.Gameplay
{
	public class Ship
	{
		IGraphicsDevice graphics;
		IKeyboard keyboard;
		IGamePad gamePad;
						
		Sprite sprite;
		float flameTimer;

		SoundEffect blasterSoundEffect;

		public Ship(IGraphicsDevice graphics, IKeyboard keyboard, IGamePad gamePad)
			: base()
		{
			if (graphics == null)
				throw new ArgumentNullException("graphics");

			if (keyboard == null)
				throw new ArgumentNullException("keyboard");

			if (gamePad == null)
				throw new ArgumentNullException("gamePad");

			this.graphics = graphics;
			this.keyboard = keyboard;
			this.gamePad = gamePad;
		}

		public void LoadContent(IContentManager<DemoGameContent> contentManager)
		{
			this.sprite = new Sprite(contentManager.Load<SpriteSheet>(DemoGameContent.Ship));
			this.sprite.Frame = 1;
			this.sprite.Origin = new Vector2(40, 40);
			
			this.sprite.AddChild(new Sprite(contentManager.Load<SpriteSheet>(DemoGameContent.ShipFlame)));
			this.sprite.Children[0].Frame = 0;
			this.sprite.Children[0].Origin = new Vector2(8, 8);
			this.sprite.Children[0].Position = new Vector2(40, 88);

			this.blasterSoundEffect = contentManager.Load<SoundEffect>(DemoGameContent.Blaster);
		}

		public void Initialize()
		{
			this.sprite.Position = new Vector2(this.graphics.BackBufferWidth / 2, this.graphics.BackBufferHeight - 60);
		}
		
		public void Update(GameTime gameTime)
		{
			this.flameTimer += gameTime.ElapsedTotalSeconds;
			if (this.flameTimer >= 0.1f)
			{
				this.sprite.Children[0].Frame++;
				if (this.sprite.Children[0].Frame >= 2)
					this.sprite.Children[0].Frame = 0;

				this.flameTimer -= 0.1f;
			}

			Vector2 movement = this.gamePad.LeftThumbStick;
			this.sprite.X += 200.0f * movement.X * gameTime.ElapsedTotalSeconds;
			this.sprite.Y += 200.0f * movement.Y * gameTime.ElapsedTotalSeconds;

			//if (this.Keyboard.IsKeyDown(Keys.Left))
			//    this.sprite.X -= 200.0f * gameTime.ElapsedTotalSeconds;
			//else if (this.Keyboard.IsKeyDown(Keys.Right))
			//    this.sprite.X += 200.0f * gameTime.ElapsedTotalSeconds;

			//if (this.Keyboard.IsKeyDown(Keys.Up))
			//    this.sprite.Y -= 100.0f * gameTime.ElapsedTotalSeconds;
			//else if (this.Keyboard.IsKeyDown(Keys.Down))
			//    this.sprite.Y += 100.0f * gameTime.ElapsedTotalSeconds;

			if (this.keyboard.IsKeyPressed(Keys.Space) || this.gamePad.IsButtonPressed(GamePadButtons.X))
				this.blasterSoundEffect.Play();
		}

		public void Draw(IGraphicsBatch graphics)
		{
			graphics.DrawSprite(this.sprite);
		}
	}
}
