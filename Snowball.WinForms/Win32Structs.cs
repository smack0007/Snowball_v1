using System;
using System.Runtime.InteropServices;

namespace Snowball.WinForms
{
	/// <summary>
	/// Struct for native Win32 messages.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	internal struct Win32Message
	{
		public IntPtr hWnd;
		public int msg;
		public IntPtr wParam;
		public IntPtr lParam;
		public uint time;
		public Win32Point p;
	}

	[StructLayout(LayoutKind.Sequential)]
	internal struct Win32Point
	{
		public int X;
		public int Y;
	}
}
