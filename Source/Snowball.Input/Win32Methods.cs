using System;
using System.Runtime.InteropServices;
using System.Security;

namespace Snowball.Input
{
	internal class Win32Methods
	{		
		[DllImport("user32.dll")]
		internal static extern short GetAsyncKeyState([In] int vKey); 

		[DllImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool GetCursorPos(out Win32Point lpPoint);
		
		[DllImport("user32.dll")]
		internal static extern uint GetDoubleClickTime();

		[DllImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool GetKeyboardState(byte[] lpKeyState);
						
		[DllImport("user32.dll")]
		internal static extern bool ScreenToClient(IntPtr hWnd, ref Win32Point lpPoint);
	}
}
