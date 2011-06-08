using System;
using System.Collections.Generic;
using Snowball.Graphics;

namespace Snowball
{
	public class GameStateManager
	{
		Dictionary<string, GameState> states;
		List<GameState> statesStack;
		bool isInitialized;

		public GameStateManager()
		{
			this.states = new Dictionary<string, GameState>();
			this.statesStack = new List<GameState>();
		}

		public void Add(string key, GameState state)
		{
			if(this.states.ContainsKey(key))
				throw new InvalidOperationException("A different state is already registered with this manager under that name.");

			if(this.states.ContainsValue(state))
				throw new InvalidOperationException("The state is already registered with this manager.");
			
			this.states.Add(key, state);
			this.statesStack.Add(state);
			state.Manager = this;

			if(this.isInitialized && !state.IsInitialized)
				state.Initialize();
		}

		public void Initialize()
		{
			this.isInitialized = true;

			foreach(GameState state in this.states.Values)
				if(!state.IsInitialized)
					state.Initialize();
		}

		public void Update(GameTime gameTime)
		{
			for(int i = 0; i < this.statesStack.Count; i++)
			{
				this.statesStack[i].Update(gameTime);

				if(this.statesStack[i].BlocksUpdate)
					break;
			}
		}

		public void Draw(IRenderer renderer)
		{
			int i;
			for(i = 0; i < this.statesStack.Count; i++)
			{
				if(this.statesStack[i].BlocksDraw)
					break;
			}

			if(i >= this.statesStack.Count)
				i = this.statesStack.Count - 1;

			for(; i >= 0; i--)
				this.statesStack[i].Draw(renderer);
		}
	}
}
