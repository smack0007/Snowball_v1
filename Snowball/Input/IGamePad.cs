using System;

namespace Snowball.Input
{
	/// <summary>
	/// Interface for GamePad input devices.
	/// </summary>
	public interface IGamePad
	{
		/// <summary>
		/// The index of the player this GamePadDevice is reading from.
		/// </summary>
		PlayerIndex PlayerIndex { get; }

		/// <summary>
		/// Whether or not DPadUp is pressed.
		/// </summary>
		bool DPadUp { get; }

		/// <summary>
		/// Whether or not DPadRight is pressed.
		/// </summary>
		bool DPadRight { get; }

		/// <summary>
		/// Whether or not DPadDown is pressed.
		/// </summary>
		bool DPadDown { get; }

		/// <summary>
		/// Whether or not DPadLeft is pressed.
		/// </summary>
		bool DPadLeft { get; }

		/// <summary>
		/// Whether or not LeftThumb is pressed.
		/// </summary>
		bool LeftThumb { get; }

		/// <summary>
		/// Whether or not RightThumb is pressed.
		/// </summary>
		bool RightThumb { get; }

		/// <summary>
		/// Whether or not LeftShoulder is pressed.
		/// </summary>
		bool LeftShoulder { get; }

		/// <summary>
		/// Whether or not RightShoulder is pressed.
		/// </summary>
		bool RightShoulder { get; }

		/// <summary>
		/// Whether or not A is pressed.
		/// </summary>
		bool A { get; }

		/// <summary>
		/// Whether or not B is pressed.
		/// </summary>
		bool B { get; }

		/// <summary>
		/// Whether or not X is pressed.
		/// </summary>
		bool X { get; }

		/// <summary>
		/// Whether or not Y is pressed.
		/// </summary>
		bool Y { get; }

		/// <summary>
		/// Whether or not Start is pressed.
		/// </summary>
		bool Start { get; }

		/// <summary>
		/// Whether or not Back is pressed.
		/// </summary>
		bool Back { get; }

		/// <summary>
		/// The current reading for the left thumb stick.
		/// </summary>
		Vector2 LeftThumbStick { get; }

		/// <summary>
		/// The current reading for the right thumb stick.
		/// </summary>
		Vector2 RightThumbStick { get; }

		/// <summary>
		/// The current reading for the left trigger.
		/// </summary>
		float LeftTrigger { get; }

		/// <summary>
		/// The current reading for the right trigger.
		/// </summary>
		float RightTrigger { get; }

		/// <summary>
		/// Returns true if the given button is currently down.
		/// </summary>
		/// <param name="button"></param>
		/// <returns></returns>
		bool IsButtonDown(GamePadButtons button);

		/// <summary>
		/// Returns true if the given button is currently up.
		/// </summary>
		/// <param name="button"></param>
		/// <returns></returns>
		bool IsButtonUp(GamePadButtons button);

		/// <summary>
		/// Returns true if the given button is currently down and was up during the last update.
		/// </summary>
		/// <param name="button"></param>
		/// <returns></returns>
		bool IsButtonPressed(GamePadButtons button);

		/// <summary>
		/// Returns true if the given button is currently up and was down during the last update.
		/// </summary>
		/// <param name="button"></param>
		/// <returns></returns>
		bool IsButtonReleased(GamePadButtons button);
	}
}
