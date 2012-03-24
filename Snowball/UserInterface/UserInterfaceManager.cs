using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Snowball.Graphics;

namespace Snowball.UserInterface
{
	public class UserInterfaceManager
	{
		List<UserInterfaceControl> controls;

		public IGameWindow GameWindow
		{
			get;
			private set;
		}

		public TextureFont Font
		{
			get;
			set;
		}

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
