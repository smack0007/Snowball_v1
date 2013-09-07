using System;

using XInput = SharpDX.XInput;

namespace Snowball.Input
{
	/// <summary>
	/// Reads from a GamePad input device.
	/// </summary>
	public sealed class GamePad : IGamePad
	{
		GamePadDeadZone deadZone;

		XInput.Controller controller;
		XInput.Gamepad state;
		XInput.Gamepad oldState;

		/// <summary>
		/// The index of the player this GamePadDevice is reading from.
		/// </summary>
		public GamePadIndex Index
		{
			get;
			private set;
		}

        /// <summary>
        /// Whether or not the controller is connected.
        /// </summary>
        public bool IsConnected
        {
            get { return this.controller.IsConnected; }
        }

		/// <summary>
		/// Whether or not DPadUp is down.
		/// </summary>
		public bool DPadUp
		{
			get { return this.state.Buttons.HasFlag(XInput.GamepadButtonFlags.DPadUp); }
		}

		/// <summary>
		/// Whether or not DPadRight is down.
		/// </summary>
		public bool DPadRight
		{
			get { return this.state.Buttons.HasFlag(XInput.GamepadButtonFlags.DPadRight); }
		}

		/// <summary>
		/// Whether or not DPadDown is down.
		/// </summary>
		public bool DPadDown
		{
			get { return this.state.Buttons.HasFlag(XInput.GamepadButtonFlags.DPadDown); }
		}

		/// <summary>
		/// Whether or not DPadLeft is down.
		/// </summary>
		public bool DPadLeft
		{
			get { return this.state.Buttons.HasFlag(XInput.GamepadButtonFlags.DPadLeft); }
		}

		/// <summary>
		/// Whether or not LeftThumb is down.
		/// </summary>
		public bool LeftThumb
		{
			get { return this.state.Buttons.HasFlag(XInput.GamepadButtonFlags.LeftThumb); }
		}

		/// <summary>
		/// Whether or not RightThumb is down.
		/// </summary>
		public bool RightThumb
		{
			get { return this.state.Buttons.HasFlag(XInput.GamepadButtonFlags.RightThumb); }
		}

		/// <summary>
		/// Whether or not LeftShoulder is down.
		/// </summary>
		public bool LeftShoulder
		{
			get { return this.state.Buttons.HasFlag(XInput.GamepadButtonFlags.LeftShoulder); }
		}

		/// <summary>
		/// Whether or not RightShoulder is down.
		/// </summary>
		public bool RightShoulder
		{
			get { return this.state.Buttons.HasFlag(XInput.GamepadButtonFlags.RightShoulder); }
		}

		/// <summary>
		/// Whether or not A is down.
		/// </summary>
		public bool A
		{
			get { return this.state.Buttons.HasFlag(XInput.GamepadButtonFlags.A); }
		}

		/// <summary>
		/// Whether or not B is down.
		/// </summary>
		public bool B
		{
			get { return this.state.Buttons.HasFlag(XInput.GamepadButtonFlags.B); }
		}

		/// <summary>
		/// Whether or not X is down.
		/// </summary>
		public bool X
		{
			get { return this.state.Buttons.HasFlag(XInput.GamepadButtonFlags.X); }
		}

		/// <summary>
		/// Whether or not Y is down.
		/// </summary>
		public bool Y
		{
			get { return this.state.Buttons.HasFlag(XInput.GamepadButtonFlags.Y); }
		}

		/// <summary>
		/// Whether or not Start is down.
		/// </summary>
		public bool Start
		{
			get { return this.state.Buttons.HasFlag(XInput.GamepadButtonFlags.Start); }
		}

		/// <summary>
		/// Whether or not Back is down.
		/// </summary>
		public bool Back
		{
			get { return this.state.Buttons.HasFlag(XInput.GamepadButtonFlags.Back); }
		}

		/// <summary>
		/// The current reading for the left thumb stick.
		/// </summary>
		public Vector2 LeftThumbStick
		{
			get;
			private set;
		}

		/// <summary>
		/// The current reading for the right thumb stick.
		/// </summary>
		public Vector2 RightThumbStick
		{
			get;
			private set;
		}

		/// <summary>
		/// The current reading for the left trigger.
		/// </summary>
		public float LeftTrigger
		{
			get;
			private set;
		}

		/// <summary>
		/// The current reading for the right trigger.
		/// </summary>
		public float RightTrigger
		{
			get;
			private set;
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="gamePadIndex"></param>
		public GamePad(GamePadIndex gamePadIndex)
			: this(gamePadIndex, GamePadDeadZone.Standard)
		{
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="gamePadIndex"></param>
		/// <param name="deadZone"></param>
		public GamePad(GamePadIndex gamePadIndex, GamePadDeadZone deadZone)
		{
			this.Index = gamePadIndex;
			this.deadZone = deadZone;

			this.controller = new XInput.Controller(ConvertGamePadIndexToUserIndex(this.Index));
			this.state = new XInput.Gamepad();
		}

		/// <summary>
		/// Updates the state of the GamePad.
		/// </summary>
		public void Update()
		{
            if (this.controller.IsConnected)
            {
                this.oldState = this.state;
                this.state = this.controller.GetState().Gamepad;

                this.LeftThumbStick = ApplyThumbStickDeadZone(new Vector2(ConvertThumbStickAxisValue(this.state.LeftThumbX), -ConvertThumbStickAxisValue(this.state.LeftThumbY)), this.deadZone.LeftThumbStick);
                this.RightThumbStick = ApplyThumbStickDeadZone(new Vector2(ConvertThumbStickAxisValue(this.state.RightThumbX), -ConvertThumbStickAxisValue(this.state.RightThumbY)), this.deadZone.RightThumbStick);

                this.LeftTrigger = ConvertTriggerValue(this.state.LeftTrigger);
                this.RightTrigger = ConvertTriggerValue(this.state.RightTrigger);
            }
		}

		/// <summary>
		/// Converts a PlayerIndex to a XInput.UserIndex.
		/// </summary>
		/// <param name="playerIndex"></param>
		/// <returns></returns>
		private static XInput.UserIndex ConvertGamePadIndexToUserIndex(GamePadIndex playerIndex)
		{
			switch(playerIndex)
			{
				case GamePadIndex.One: return XInput.UserIndex.One;
				case GamePadIndex.Two: return XInput.UserIndex.Two;
				case GamePadIndex.Three: return XInput.UserIndex.Three;
				case GamePadIndex.Four: return XInput.UserIndex.Four;
			}

			throw new ArgumentOutOfRangeException("PlayerIndex must be between 1 and 4.");
		}

		/// <summary>
		/// Converts a short ThumbStick axis value to a float.
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		private static float ConvertThumbStickAxisValue(short input)
		{
			if (input == 0)
				return 0.0f;

			if (input > 0)
				return (float)input / short.MaxValue;
			else
				return -((float)input / short.MinValue);
		}

		/// <summary>
		/// Applys a dead zone calculation to the given input.
		/// </summary>
		/// <param name="input"></param>
		/// <param name="deadZone"></param>
		/// <returns></returns>
		private static Vector2 ApplyThumbStickDeadZone(Vector2 input, Vector2 deadZone)
		{
			if (Math.Abs(input.X) < deadZone.X)
				input.X = 0.0f;

			if (Math.Abs(input.Y) < deadZone.Y)
				input.Y = 0.0f;

			return input;
		}

		/// <summary>
		/// Converts a Trigger byte value to a float.
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		private static float ConvertTriggerValue(byte input)
		{
			return (float)input / byte.MaxValue;
		}

		private static XInput.GamepadButtonFlags ConvertButton(GamePadButtons button)
		{
			switch(button)
			{
				case GamePadButtons.A: return XInput.GamepadButtonFlags.A;
				case GamePadButtons.B: return XInput.GamepadButtonFlags.B;
				case GamePadButtons.Back: return XInput.GamepadButtonFlags.Back;
				case GamePadButtons.DPadDown: return XInput.GamepadButtonFlags.DPadDown;
				case GamePadButtons.DPadLeft: return XInput.GamepadButtonFlags.DPadLeft;
				case GamePadButtons.DPadRight: return XInput.GamepadButtonFlags.DPadRight;
				case GamePadButtons.DPadUp: return XInput.GamepadButtonFlags.DPadUp;
				case GamePadButtons.LeftShoulder: return XInput.GamepadButtonFlags.LeftShoulder;
				case GamePadButtons.LeftThumb: return XInput.GamepadButtonFlags.LeftThumb;
				case GamePadButtons.RightShoulder: return XInput.GamepadButtonFlags.RightShoulder;
				case GamePadButtons.RightThumb: return XInput.GamepadButtonFlags.RightThumb;
				case GamePadButtons.Start: return XInput.GamepadButtonFlags.Start;
				case GamePadButtons.X: return XInput.GamepadButtonFlags.X;
				case GamePadButtons.Y: return XInput.GamepadButtonFlags.Y;
			}

			throw new InvalidOperationException("Unknown GamePadButton.");
		}

		/// <summary>
		/// Returns true if the given button is currently down.
		/// </summary>
		/// <param name="button"></param>
		/// <returns></returns>
		public bool IsButtonDown(GamePadButtons button)
		{
			return this.state.Buttons.HasFlag(ConvertButton(button));
		}

		/// <summary>
		/// Returns true if the given button is currently up.
		/// </summary>
		/// <param name="button"></param>
		/// <returns></returns>
		public bool IsButtonUp(GamePadButtons button)
		{
			return !this.state.Buttons.HasFlag(ConvertButton(button));
		}

		/// <summary>
		/// Returns true if the given button is currently down and was up during the last update.
		/// </summary>
		/// <param name="button"></param>
		/// <returns></returns>
		public bool IsButtonPressed(GamePadButtons button)
		{
			XInput.GamepadButtonFlags internalButton = ConvertButton(button);
			return this.state.Buttons.HasFlag(internalButton) &&
				   !this.oldState.Buttons.HasFlag(internalButton);
		}

		/// <summary>
		/// Returns true if the given button is currently up and was down during the last update.
		/// </summary>
		/// <param name="button"></param>
		/// <returns></returns>
		public bool IsButtonReleased(GamePadButtons button)
		{
			XInput.GamepadButtonFlags internalButton = ConvertButton(button);
			return this.state.Buttons.HasFlag(internalButton) &&
				   !this.oldState.Buttons.HasFlag(internalButton);
		}
	}
}
