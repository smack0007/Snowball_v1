using System;
using System.Reflection;
using Snowball.Graphics;

namespace Snowball
{
	/// <summary>
	/// Base class for components in your game.
	/// </summary>
	public class GameComponent : IGameComponent
	{
		IServiceProvider services;

		public GameComponent(IServiceProvider services)
		{
			if (services == null)
				throw new ArgumentNullException("services");

			this.services = services;
		}

		private void InjectDependencies()
		{
			PropertyInfo[] properties = this.GetType().GetProperties();
						
			foreach(PropertyInfo property in properties)
			{
				foreach(Attribute attribute in property.GetCustomAttributes(true))
				{
					if (attribute is GameComponentDependencyAttribute)
					{
						if (!property.CanWrite)
							throw new InvalidOperationException(property.Name + " is not writable.");

						object dependency = this.services.GetRequiredGameService(property.PropertyType);
						property.SetValue(this, dependency, null);
					}
				}
			}
		}

		public virtual void Initialize()
		{
			this.InjectDependencies();
		}

		public virtual void Update(GameTime gameTime)
		{
		}

		public virtual void Draw(IRenderer renderer)
		{
		}
	}
}
