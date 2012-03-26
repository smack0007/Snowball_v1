using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Snowball.Graphics;

namespace Snowball.UserInterface
{
	public class UserInterfaceManager
	{
		TextureFont font;
		List<UserInterfaceControl> controls;

		UserInterfacePropertyChangedEventArgs propertyChangedEventArgs;

		public IGameWindow GameWindow
		{
			get;
			private set;
		}

		public TextureFont Font
		{
			get { return this.font; }

			set
			{
				if (value != this.font)
				{
					this.font = value;
					this.TriggerSharedPropertyChanged(UserInterfaceProperties.Font);
				}
			}
		}

		public event EventHandler<UserInterfacePropertyChangedEventArgs> SharedPropertyChanged;

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="gameWindow"></param>
		public UserInterfaceManager(IGameWindow gameWindow)
		{
			if (gameWindow == null)
				throw new ArgumentNullException("gameWindow");

			this.GameWindow = gameWindow;

			this.controls = new List<UserInterfaceControl>();

			this.propertyChangedEventArgs = new UserInterfacePropertyChangedEventArgs();
		}

		private void TriggerSharedPropertyChanged(UserInterfaceProperties property)
		{
			if (this.SharedPropertyChanged != null)
			{
				this.propertyChangedEventArgs.Property = property;
				this.SharedPropertyChanged(this, this.propertyChangedEventArgs);
			}
		}

		public void AddControl(UserInterfaceControl control)
		{
			if (control == null)
				throw new ArgumentNullException("control");

			if (control.Manager != null)
				throw new InvalidOperationException("Control is already being managed by a different manager.");

			this.controls.Add(control);
			control.Manager = this;
		}

		public void RemoveControl(UserInterfaceControl control)
		{
			if (control == null)
				throw new ArgumentNullException("control");

			if (control.Manager != this)
				throw new InvalidOperationException("Control is not being managed by this manager.");

			this.controls.Remove(control);
			control.Manager = null;
		}

		public void Update(GameTime gameTime)
		{
			//foreach (UserInterfaceControl control in this.controls)
			//{
			//}
		}

		public void Draw(IRenderer renderer)
		{
			if (renderer == null)
				throw new ArgumentNullException("renderer");

			foreach (UserInterfaceControl control in this.controls)
			{
				control.Draw(renderer);
			}
		}
	}
}
