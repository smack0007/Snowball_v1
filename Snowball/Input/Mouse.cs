using System;
using Snowball.Win32;

namespace Snowball.Input
{
	/// <summary>
	/// Reads from a mouse input device.
	/// </summary>
	public sealed class Mouse : IMouse
	{
		const int ButtonCount = 5;

		IGameWindow window;

		Point position;
		Point oldPosition;

		bool[] buttons;
		bool[] oldButtons;

		MouseButtons? lastClickedButton;
		TimeSpan elapsedSinceClick;
		MouseButtons? doubleClickedButton;

		/// <summary>
		/// Returns true if the mouse is within the game window client area.
		/// </summary>
		public bool IsWithinDisplayArea
		{
			get;
			private set;
		}

		/// <summary>
		/// Returns the double click rate for the mouse.
		/// </summary>
		public TimeSpan DoubleClickRate
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
		/// Returns true if the left mouse button is currently down.
		/// </summary>
		public bool LeftButton
		{
			get { return this.buttons[(int)MouseButtons.Left]; }
		}

		/// <summary>
		/// Returns true if the right mouse button is currently down.
		/// </summary>
		public bool RightButton
		{
			get { return this.buttons[(int)MouseButtons.Right]; }
		}

		/// <summary>
		/// Returns true if the middle mouse button is currently down.
		/// </summary>
		public bool MiddleButton
		{
			get { return this.buttons[(int)MouseButtons.Middle]; }
		}

		/// <summary>
		/// Returns true if XButton1 mouse button is currently down.
		/// </summary>
		public bool XButton1
		{
			get { return this.buttons[(int)MouseButtons.XButton1]; }
		}

		/// <summary>
		/// Returns true if XButton2 mouse button is currently down.
		/// </summary>
		public bool XButton2
		{
			get { return this.buttons[(int)MouseButtons.XButton2]; }
		}
				
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="window"></param>
		public Mouse(IGameWindow window)
		{
			if (window == null)
				throw new ArgumentNullException("window", "Window was null. Please provide an instance of IGameWindow.");

			this.window = window;

			this.DoubleClickRate = TimeSpan.FromMilliseconds(Win32Methods.GetDoubleClickTime());

			this.position = Point.Zero;
			this.oldPosition = Point.Zero;

			this.buttons = new bool[ButtonCount];
			this.oldButtons = new bool[ButtonCount];
		}

		/// <summary>
		/// Updates the state of the mouse.
		/// </summary>
		public void Update(GameTime gameTime)
		{
			Win32Point point;
			Win32Methods.GetCursorPos(out point);
			Win32Methods.ScreenToClient(this.window.Handle, ref point);

			if (point.X < 0 ||
				point.Y < 0 ||
			    point.X >= this.window.DisplayWidth ||
			    point.Y >= this.window.DisplayHeight)
			{
				this.IsWithinDisplayArea = false;
			}
			else
			{
				this.IsWithinDisplayArea = true;
			}

			this.oldPosition = this.position;
			this.position = new Point(point.X, point.Y);

			for(int i = 0; i < ButtonCount; i++)
				this.oldButtons[i] = this.buttons[i];

			this.buttons[(int)MouseButtons.Left] = (Win32Methods.GetAsyncKeyState(Win32Constants.VK_LBUTTON) != 0);
			this.buttons[(int)MouseButtons.Right] = (Win32Methods.GetAsyncKeyState(Win32Constants.VK_RBUTTON) != 0);
			this.buttons[(int)MouseButtons.Middle] = (Win32Methods.GetAsyncKeyState(Win32Constants.VK_MBUTTON) != 0);
			this.buttons[(int)MouseButtons.XButton1] = (Win32Methods.GetAsyncKeyState(Win32Constants.VK_XBUTTON1) != 0);
			this.buttons[(int)MouseButtons.XButton2] = (Win32Methods.GetAsyncKeyState(Win32Constants.VK_XBUTTON2) != 0);

			// Double click detection.
			
			this.doubleClickedButton = null;

			if (this.lastClickedButton != null)
			{
				this.elapsedSinceClick += gameTime.ElapsedTime;

				if (this.elapsedSinceClick > this.DoubleClickRate ||
					this.elapsedSinceClick > TimeSpan.FromSeconds(5)) // Give up updating after 5 seconds
				{
					this.lastClickedButton = null;
				}
			}

			MouseButtons? clickedButton = null;

			if (this.IsButtonClicked(MouseButtons.Left))
			{
				clickedButton = MouseButtons.Left;
			}
			else if (this.IsButtonClicked(MouseButtons.Right))
			{
				clickedButton = MouseButtons.Right;
			}
			else if (this.IsButtonClicked(MouseButtons.Middle))
			{
				clickedButton = MouseButtons.Middle;
			}
			else if (this.IsButtonClicked(MouseButtons.XButton1))
			{
				clickedButton = MouseButtons.XButton1;
			}
			else if (this.IsButtonClicked(MouseButtons.XButton2))
			{
				clickedButton = MouseButtons.XButton2;
			}

			if (clickedButton != null)
			{
				if (clickedButton.Value == this.lastClickedButton)
				{
					if (this.elapsedSinceClick <= this.DoubleClickRate)
					{
						this.doubleClickedButton = clickedButton;
						this.lastClickedButton = null;
						this.elapsedSinceClick = TimeSpan.Zero;
					}
				}
				else
				{
					this.lastClickedButton = clickedButton;
					this.elapsedSinceClick = TimeSpan.Zero;
				}
			}
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
		/// Returns true if the given mouse button is currently down and was not on the last update. Method is same as IsButtonClicked().
		/// </summary>
		/// <param name="button"></param>
		/// <returns></returns>
		public bool IsButtonPressed(MouseButtons button)
		{
			return this.buttons[(int)button] && !this.oldButtons[(int)button];
		}

		/// <summary>
		/// Returns true if the given mouse button is currently down and was not on the last update. Method is same as IsButtonPressed().
		/// </summary>
		/// <param name="button"></param>
		/// <returns></returns>
		public bool IsButtonClicked(MouseButtons button)
		{
			return this.IsButtonPressed(button);
		}

		/// <summary>
		/// Returns true if the given mouse button is clicked and was clicked twice within the time span specified by DoubleClickRate.
		/// </summary>
		/// <param name="button"></param>
		/// <returns></returns>
		public bool IsButtonDoubleClicked(MouseButtons button)
		{
			return this.doubleClickedButton != null && this.doubleClickedButton.Value == button;
		}

		/// <summary>
		/// Resets double click tracking for the mouse.
		/// </summary>
		public void ResetDoubleClick()
		{
			this.doubleClickedButton = null;
			this.lastClickedButton = null;
			this.elapsedSinceClick = TimeSpan.Zero;
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
			return !this.buttons[(int)button] && this.oldButtons[(int)button];
		}
	}
}
