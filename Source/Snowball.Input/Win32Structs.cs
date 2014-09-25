using System;
using System.Runtime.InteropServices;
using System.Security;

namespace Snowball.Input
{	
	[StructLayout(LayoutKind.Sequential)]
	internal struct Win32Point
	{
		public int X;
		public int Y;
	}
}
