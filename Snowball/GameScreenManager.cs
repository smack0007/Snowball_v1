using System;
using System.Collections.Generic;
using Snowball.Graphics;

namespace Snowball
{
	public class GameScreenManager
	{
		Dictionary<string, GameScreen> screens;
		List<GameScreen> screensStack;
		bool isInitialized;

		public GameScreen this[string key]
		{
			get
			{
				if(!this.screens.ContainsKey(key))
					throw new IndexOutOfRangeException("No screen is registered under that key.");
				
				return this.screens[key];
			}
		}

		public GameScreenManager()
		{
			this.screens = new Dictionary<string, GameScreen>();
			this.screensStack = new List<GameScreen>();
		}

		public void Add(string key, GameScreen screen)
		{
			if(this.screens.ContainsKey(key))
				throw new InvalidOperationException("A different screen is already registered with this manager under that key.");

			if(this.screens.ContainsValue(screen))
				throw new InvalidOperationException("The screen is already registered with this manager.");
			
			this.screens.Add(key, screen);
			this.screensStack.Add(screen);
			screen.Manager = this;

			if(this.isInitialized && !screen.IsInitialized)
				screen.Initialize();
		}

		public void Initialize()
		{
			this.isInitialized = true;

			foreach(GameScreen screen in this.screens.Values)
				if(!screen.IsInitialized)
					screen.Initialize();
		}

		public void Update(GameTime gameTime)
		{
			for(int i = 0; i < this.screensStack.Count; i++)
			{
				this.screensStack[i].Update(gameTime);

				if(this.screensStack[i].BlocksUpdate)
					break;
			}
		}

		public void Draw(IRenderer renderer)
		{
			int i;
			for(i = 0; i < this.screensStack.Count; i++)
			{
				if(this.screensStack[i].BlocksDraw)
					break;
			}

			if(i >= this.screensStack.Count)
				i = this.screensStack.Count - 1;

			for(; i >= 0; i--)
				this.screensStack[i].Draw(renderer);
		}

		public void BringToFront(GameScreen screen)
		{
			if(screen.Manager != this)
				throw new InvalidOperationException("Screen is not currently managed by this manager.");

			this.screensStack.Remove(screen);
			this.screensStack.Insert(0, screen);
		}

		public void SendToBack(GameScreen screen)
		{
			if(screen.Manager != this)
				throw new InvalidOperationException("Screen is not currently managed by this manager.");

			this.screensStack.Remove(screen);
			this.screensStack.Insert(this.screensStack.Count, screen);
		}
	}
}
