using System;

namespace Snowball.GameFramework
{
	/// <summary>
	/// Contains constants relating to Win32.
	/// </summary>
	internal static class Win32Constants
	{
		internal const int PM_REMOVE = 0x0001;

		internal const int WM_KEYDOWN = 0x0100;
		internal const int WM_KEYUP = 0x0101;
		internal const int WM_CHAR = 0x0102;
		internal const int WM_SYSKEYDOWN = 0x0104;
		internal const int WM_SYSKEYUP = 0x0105;
		internal const int WM_UNICHAR = 0x0109;
		internal const int WM_SYSCOMMAND = 0x0112;
		internal const int WM_MOUSEMOVE = 0x0200;
		internal const int WM_LBUTTONDOWN = 0x0201;
		internal const int WM_LBUTTONUP = 0x0202;
		internal const int WM_RBUTTONDOWN = 0x0204;
		internal const int WM_RBUTTONUP = 0x0205;
		internal const int WM_MBUTTONDOWN = 0x0207;
		internal const int WM_MBUTTONUP = 0x0208;
		internal const int WM_ENTERSIZEMOVE = 0x0231;
		internal const int WM_EXITSIZEMOVE = 0x0232;
		internal const int WM_MOUSEHOVER = 0x02A1;
		internal const int WM_MOUSELEAVE = 0x02A3;

		internal const int SC_MINIMIZE = 0xF020;
		internal const int SC_RESTORE = 0xF120;

		internal const int TME_HOVER = 0x0001;
		internal const int TME_LEAVE = 0x0002;
		
		internal static ushort LowWord(uint value)
		{
			return (ushort)value;
		}

		internal static ushort HighWord(uint value)
		{
			return (ushort)(value >> 16);
		}
	}
}
