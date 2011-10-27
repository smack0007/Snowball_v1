﻿using System;
using Snowball.Content;
using Snowball.Graphics;
using Snowball.Input;
using Snowball.Sound;
using Snowball.Demo.Gameplay;

namespace Snowball.Demo
{
	public class DemoGame : Game
	{
		Renderer renderer;
		DemoContentLoader content;
		KeyboardDevice keyboard;
		GameConsole console;

		Starfield starfield;
		Ship ship;
				
		RenderTarget renderTarget;

		int fps;
		float fpsTime;

		public DemoGame()
			: base()
		{
			this.Window.Title = "Snowball Demo Game";

			this.content = new DemoContentLoader(this.Services);
			this.Services.AddService(typeof(IContentLoader), this.content);

			this.keyboard = new KeyboardDevice();
			this.Services.AddService(typeof(IKeyboardDevice), this.keyboard);

			this.console = new GameConsole(this.Services);
			
			this.ship = new Ship(this.Services);
		}

		protected override void Initialize()
		{
			base.Initialize();

			this.Graphics.CreateDevice(this.Window, 800, 600);

			this.console.Initialize();
			this.console.Font = new TextureFont(this.Graphics, "Arial", 12, true);
			this.console.BackgroundTexture = this.content.Load<Texture>("ConsoleBackground");
			this.console.InputColor = Color.Blue;
			this.console.CommandEntered += (s, e) =>
			{
			    this.console.WriteLine(e.Command);
			};

			this.starfield = new Starfield(this.Graphics.DisplayWidth, this.Graphics.DisplayHeight);

			this.ship.Initialize();

			this.renderer = new Renderer(this.Graphics);

			this.renderTarget = this.Graphics.CreateRenderTarget(200, 200);
			this.DrawRenderTarget();
		}

		protected override void OnToggleFullscreen()
		{
			this.DrawRenderTarget();
		}

		private void DrawRenderTarget()
		{
			this.Graphics.SetRenderTarget(this.renderTarget);
			this.Graphics.BeginDraw();
			this.Graphics.Clear(Color.Blue);
			this.renderer.Begin();
			this.renderer.DrawLine(new Vector2(0, 0), new Vector2(200, 200), Color.Red);
			this.renderer.End();
			this.Graphics.EndDraw();
			this.Graphics.SetRenderTarget(null);
		}

		protected override void Update(GameTime gameTime)
		{
			this.keyboard.Update(gameTime);

			if(this.keyboard.IsKeyPressed(Keys.Escape))
			{
				this.Exit();
			}

			if(this.keyboard.IsKeyPressed(Keys.F12))
			{
				this.Graphics.ToggleFullscreen();
			}

			if(!this.console.IsVisible)
			{
				this.starfield.Update(gameTime);
				this.ship.Update(gameTime);
			}

			this.console.Update(gameTime);
		}

		protected override void Draw(GameTime gameTime)
		{			
			this.Graphics.Clear(Color.Black);
			this.Graphics.BeginDraw();
			this.renderer.Begin();
			
			this.starfield.Draw(this.renderer);
			this.ship.Draw(this.renderer);
			//this.renderer.DrawRenderTarget(this.renderTarget, Vector2.Zero, Color.White);

			this.console.Draw(this.renderer);

			this.renderer.End();
			this.Graphics.EndDraw();
			this.Graphics.Present();
			
			this.fps++;
			this.fpsTime += gameTime.ElapsedTotalSeconds;
			if(this.fpsTime >= 1.0f)
			{
			    this.console.WriteLine(this.fps.ToString() + " FPS", Color.Green);
			    this.fps = 0;
			    this.fpsTime -= 1.0f;
			}
		}

		public static void Main()
		{
			using(DemoGame game = new DemoGame())
				game.Run();
		}
	}
}
