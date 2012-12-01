using System;

namespace Snowball.Input
{
	/// <summary>
	/// Reads from a Keyboard input device.
	/// </summary>
	public sealed class Keyboard : IKeyboard
	{
		const int KeyCount = 256;
		const byte HighBitOnlyMask = 0x80;

		byte[] keys;
		byte[] oldKeys;
		
		/// <summary>
		/// Constructor.
		/// </summary>
		public Keyboard()
		{
			this.keys = new byte[KeyCount];
			this.oldKeys = new byte[KeyCount];			
		}

		/// <summary>
		/// Updates the state of the keyboard.
		/// </summary>
		public void Update()
		{
			for (int i = 0; i < KeyCount; i++)
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
