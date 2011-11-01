using System;

namespace Snowball.Input
{
	/// <summary>
	/// Provides dead zone values for GamePadDevice.
	/// </summary>
	public class GamePadDeadZone
	{
		public static readonly GamePadDeadZone Standard = new GamePadDeadZone(new Vector2(0.2f, 0.2f));

		/// <summary>
		/// The dead zone to use for the LeftThumbStick.
		/// </summary>
		public Vector2 LeftThumbStick
		{
			get;
			private set;
		}

		/// <summary>
		/// The dead zone to use for the RightThumbStick.
		/// </summary>
		public Vector2 RightThumbStick
		{
			get;
			private set;
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="thumbStick"></param>
		public GamePadDeadZone(Vector2 thumbStick)
		{
			this.LeftThumbStick = thumbStick;
			this.RightThumbStick = thumbStick;
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="leftThumbStick"></param>
		/// <param name="rightThumbStick"></param>
		public GamePadDeadZone(Vector2 leftThumbStick, Vector2 rightThumbStick)
		{
			this.LeftThumbStick = leftThumbStick;
			this.RightThumbStick = rightThumbStick;
		}
	}
}
