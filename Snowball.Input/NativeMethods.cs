using System;
using System.Runtime.InteropServices;
using System.Security;

namespace Snowball.Input
{
	internal class NativeMethods
	{
		[DllImport("user32.dll")]
		internal static extern IntPtr CreateWindowEx(int dwExStyle, string lpClassName, string lpWindowName, uint dwStyle, int x, int y, int nWidth, int nHeight,
												     IntPtr hWndParent, IntPtr hMenu, IntPtr hInstance, IntPtr lpParam);

		[DllImport("user32.dll")]
		internal static extern IntPtr DispatchMessage([In] ref Win32Message msg);

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
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool PeekMessage(out Win32Message msg, IntPtr hWnd, uint messageFilterMin, uint messageFilterMax, uint flags);

		[DllImport("user32.dll")]
		internal static extern bool ScreenToClient(IntPtr hWnd, ref Win32Point lpPoint);
		
		[DllImport("user32.dll")]
		internal static extern bool TranslateMessage([In] ref Win32Message msg);
	}
}
