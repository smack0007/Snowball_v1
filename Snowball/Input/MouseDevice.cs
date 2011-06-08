using System;
using Snowball.Win32;

namespace Snowball.Input
{
	public class MouseDevice : GameSubsystem, IMouseDevice
	{
		const int ButtonCount = 5;
		
		IGameWindow window;

		Point position;
		Point oldPosition;

		bool[] buttons;
		bool[] oldButtons;
		
		/// <summary>
		/// Returns true if the mouse is within the game window client area.
		/// </summary>
		public bool IsWithinClientArea
		{
			get;
			private set;
		}

		/// <summary>
		/// The position of the cursor.
		/// </summary>
		public Point Position
		{
			get { return this.position; }
		}

		/// <summary>
		/// The delta of the position from the last update.
		/// </summary>
		public Point PositionDelta
		{
			get { return new Point(this.position.X - this.oldPosition.X, this.position.Y - this.oldPosition.Y); }
		}

		/// <summary>
		/// The X position of the cursor.
		/// </summary>
		public int X
		{
			get { return this.position.X; }
		}

		/// <summary>
		/// The Y position of the cursor.
		/// </summary>
		public int Y
		{
			get { return this.position.Y; }
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		public MouseDevice()
			: this(GameWindow.Current)
		{
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="window"></param>
		public MouseDevice(IGameWindow window)
		{
			if(window == null)
				throw new ArgumentNullException("window", "Window was null. Please provide an instance of IGameWindow.");

			this.window = window;

			this.position = Point.Zero;
			this.oldPosition = Point.Zero;

			this.buttons = new bool[ButtonCount];
			this.oldButtons = new bool[ButtonCount];

			this.Enabled = true;
		}

		/// <summary>
		/// Updates the state of the mouse.
		/// </summary>
		public override void Update(GameTime gameTime)
		{
			Win32Point point;
			Win32Methods.GetCursorPos(out point);
			Win32Methods.ScreenToClient(this.window.Handle, ref point);

			if(point.X < 0 || point.Y < 0 ||
			   point.X >= this.window.ClientWidth ||
			   point.Y >= this.window.ClientHeight)
			{
				this.IsWithinClientArea = false;
			}
			else
			{
				this.IsWithinClientArea = true;
			}

			this.oldPosition = this.position;
			this.position = new Point(point.X, point.Y);

			for(int i = 0; i < ButtonCount; i++)
				this.oldButtons[i] = this.buttons[i];

			this.buttons[0] = (Win32Methods.GetAsyncKeyState(Win32Constants.VK_LBUTTON) != 0);
			this.buttons[1] = (Win32Methods.GetAsyncKeyState(Win32Constants.VK_RBUTTON) != 0);
			this.buttons[2] = (Win32Methods.GetAsyncKeyState(Win32Constants.VK_MBUTTON) != 0);
			this.buttons[3] = (Win32Methods.GetAsyncKeyState(Win32Constants.VK_XBUTTON1) != 0);
			this.buttons[4] = (Win32Methods.GetAsyncKeyState(Win32Constants.VK_XBUTTON2) != 0);
		}

		/// <summary>
		/// Returns true if the given mouse button is currently down.
		/// </summary>
		/// <param name="button"></param>
		/// <returns></returns>
		public bool IsButtonDown(MouseButtons button)
		{
			return this.buttons[(int)button];
		}

		/// <summary>
		/// Returns true if the given mouse button is currently down and was not on the last update.
		/// </summary>
		/// <param name="button"></param>
		/// <returns></returns>
		public bool IsButtonPressed(MouseButtons button)
		{
			return this.buttons[(int)button] &&
				   !this.oldButtons[(int)button];
		}

		/// <summary>
		/// Returns true if the given mouse button is currently up.
		/// </summary>
		/// <param name="button"></param>
		/// <returns></returns>
		public bool IsButtonUp(MouseButtons button)
		{
			return !this.buttons[(int)button];
		}

		/// <summary>
		/// Returns true if the given mouse button is currently up and was not up on the last update.
		/// </summary>
		/// <param name="button"></param>
		/// <returns></returns>
		public bool IsButtonReleased(MouseButtons button)
		{
			return !this.buttons[(int)button] &&
				   this.oldButtons[(int)button];
		}
	}
}
