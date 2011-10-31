using System;

namespace Snowball.Input
{
	public class GamePadDevice
	{
		SlimDX.XInput.Controller controller;
		SlimDX.XInput.Gamepad state;
		SlimDX.XInput.Gamepad oldState;

		public bool DPadUp
		{
			get { return this.state.Buttons.HasFlag(SlimDX.XInput.GamepadButtonFlags.DPadUp); }
		}

		public bool DPadRight
		{
			get { return this.state.Buttons.HasFlag(SlimDX.XInput.GamepadButtonFlags.DPadRight); }
		}

		public bool DPadDown
		{
			get { return this.state.Buttons.HasFlag(SlimDX.XInput.GamepadButtonFlags.DPadDown); }
		}

		public bool DPadLeft
		{
			get { return this.state.Buttons.HasFlag(SlimDX.XInput.GamepadButtonFlags.DPadLeft); }
		}

		public bool LeftThumb
		{
			get { return this.state.Buttons.HasFlag(SlimDX.XInput.GamepadButtonFlags.LeftThumb); }
		}

		public bool RightThumb
		{
			get { return this.state.Buttons.HasFlag(SlimDX.XInput.GamepadButtonFlags.RightThumb); }
		}

		public bool LeftShoulder
		{
			get { return this.state.Buttons.HasFlag(SlimDX.XInput.GamepadButtonFlags.LeftShoulder); }
		}

		public bool RightShoulder
		{
			get { return this.state.Buttons.HasFlag(SlimDX.XInput.GamepadButtonFlags.RightShoulder); }
		}

		public bool A
		{
			get { return this.state.Buttons.HasFlag(SlimDX.XInput.GamepadButtonFlags.A); }
		}

		public bool B
		{
			get { return this.state.Buttons.HasFlag(SlimDX.XInput.GamepadButtonFlags.B); }
		}

		public bool X
		{
			get { return this.state.Buttons.HasFlag(SlimDX.XInput.GamepadButtonFlags.X); }
		}

		public bool Y
		{
			get { return this.state.Buttons.HasFlag(SlimDX.XInput.GamepadButtonFlags.Y); }
		}

		public bool Start
		{
			get { return this.state.Buttons.HasFlag(SlimDX.XInput.GamepadButtonFlags.Start); }
		}

		public bool Back
		{
			get { return this.state.Buttons.HasFlag(SlimDX.XInput.GamepadButtonFlags.Back); }
		}

		public Vector2 LeftThumbStick
		{
			get { return new Vector2(ConvertThumbStickValue(this.state.LeftThumbX), -ConvertThumbStickValue(this.state.LeftThumbY)); }
		}
				
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="playerIndex"></param>
		public GamePadDevice(PlayerIndex playerIndex)
		{
			this.controller = new SlimDX.XInput.Controller(ConvertPlayerIndexToUserIndex(playerIndex));
			this.state = new SlimDX.XInput.Gamepad();
		}

		/// <summary>
		/// Converts a PlayerIndex to a SlimDX.XInput.UserIndex.
		/// </summary>
		/// <param name="playerIndex"></param>
		/// <returns></returns>
		private static SlimDX.XInput.UserIndex ConvertPlayerIndexToUserIndex(PlayerIndex playerIndex)
		{
			switch(playerIndex)
			{
				case PlayerIndex.One: return SlimDX.XInput.UserIndex.One;
				case PlayerIndex.Two: return SlimDX.XInput.UserIndex.Two;
				case PlayerIndex.Three: return SlimDX.XInput.UserIndex.Three;
				case PlayerIndex.Four: return SlimDX.XInput.UserIndex.Four;
			}

			throw new ArgumentOutOfRangeException("PlayerIndex must be between 1 and 4.");
		}

		private static float ConvertThumbStickValue(short input)
		{
			if(input == 0)
				return 0.0f;

			if(input > 0)
				return input / short.MaxValue;
			else
				return -(input / short.MinValue);
		}

		public void Update(GameTime gameTime)
		{
			this.oldState = this.state;
			this.state = this.controller.GetState().Gamepad;
		}
	}
}
