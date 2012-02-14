using System;
using Snowball.Graphics;
using Snowball.Content;

namespace Snowball
{
	/// <summary>
	/// Base class for components in your game.
	/// </summary>
	public class GameComponent : IGameComponent
	{
		public bool IsInitialized
		{
			get;
			protected set;
		}

		public bool IsContentLoaded
		{
			get;
			protected set;
		}

		public GameComponent()
		{
		}
				
		public virtual void Initialize()
		{
			this.IsInitialized = true;
		}

		public virtual void LoadContent(IContentLoader contentLoader)
		{
			this.IsContentLoaded = true;
		}

		public virtual void UnloadContent()
		{
			this.IsContentLoaded = false;
		}

		public virtual void Update(GameTime gameTime)
		{
		}

		public virtual void Draw(IRenderer renderer)
		{
		}
	}
}
