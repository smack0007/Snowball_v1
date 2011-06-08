using System;
using Snowball.Win32;

namespace Snowball.Input
{
	/// <summary>
	/// Manages reading of the state of the keyboard.
	/// </summary>
	public class KeyboardDevice : GameSubsystem, IKeyboardDevice
	{
		const int KeyCount = 256;
		const byte HighBitOnlyMask = 0x80;

		byte[] keys;
		byte[] oldKeys;
		
		/// <summary>
		/// Constructor.
		/// </summary>
		public KeyboardDevice()
		{
			this.keys = new byte[KeyCount];
			this.oldKeys = new byte[KeyCount];

			this.Enabled = true;
		}

		/// <summary>
		/// Updates the state of the keyboard.
		/// </summary>
		public override void Update(GameTime gameTime)
		{
			for(int i = 0; i < KeyCount; i++)
				this.oldKeys[i] = this.keys[i];

			Win32Methods.GetKeyboardState(this.keys);
		}

		/// <summary>
		/// Returns true if the given key is currently down.
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public bool IsKeyDown(Keys key)
		{
			return (this.keys[(int)key] & HighBitOnlyMask) != 0;
		}

		/// <summary>
		/// Returns true if the given key is currently down and last update was not.
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public bool IsKeyPressed(Keys key)
		{
			// If key currently down and was not down before.
			return ((this.keys[(int)key] & HighBitOnlyMask) != 0) &&
				   ((this.oldKeys[(int)key] & HighBitOnlyMask) == 0);
		}

		/// <summary>
		/// Returns true if the given key is currently up.
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public bool IsKeyUp(Keys key)
		{
			return (this.keys[(int)key] & HighBitOnlyMask) == 0;
		}
		
		/// <summary>
		/// Returns true if the given key is currently up and last update was not.
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public bool IsKeyReleased(Keys key)
		{
			// If key not currently pressed and was pressed before.
			return ((this.keys[(int)key] & HighBitOnlyMask) == 0) &&
				   ((this.oldKeys[(int)key] & HighBitOnlyMask) != 0);
		}
	}
}
