using System;
using System.Runtime.InteropServices;
using System.Security;

namespace Snowball.GameFramework
{
	internal class Win32Methods
	{		
		[DllImport("user32.dll")]
		internal static extern IntPtr DispatchMessage([In] ref Win32Message msg);
				
		[DllImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool PeekMessage(out Win32Message msg, IntPtr hWnd, uint messageFilterMin, uint messageFilterMax, uint flags);
						
		[DllImport("user32.dll")]
		internal static extern bool TranslateMessage([In] ref Win32Message msg);
	}
}
