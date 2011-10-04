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
		IGameServicesContainer services;

		public GameComponent(IGameServicesContainer services)
		{
			if(services == null)
				throw new ArgumentNullException("services");

			this.services = services;
		}

		private void InjectServices()
		{
			PropertyInfo[] properties = this.GetType().GetProperties(BindingFlags.Public | BindingFlags.NonPublic);
						
			foreach(PropertyInfo property in properties)
			{
				foreach(Attribute attribute in property.GetCustomAttributes(true))
				{
					if(attribute is RequiredGameServiceAttribute)
					{
						if(!property.CanWrite)
							throw new InvalidOperationException(property.Name + " is not writable.");

						object service = this.services.GetRequiredService(property.PropertyType);
						property.SetValue(this, service, null);
					}
				}
			}
		}

		public virtual void Initialize()
		{
			this.InjectServices();
		}

		public virtual void Update(GameTime gameTime)
		{
		}

		public virtual void Draw(GameTime gameTime)
		{
		}
	}
}
