using System;

namespace Snowball.Input
{
	public class GamePadDeadZone
	{
		public static readonly GamePadDeadZone Standard = new GamePadDeadZone(new Vector2(0.2f, 0.2f));

		public Vector2 LeftThumbStick;

		public Vector2 RightThumbStick;

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
