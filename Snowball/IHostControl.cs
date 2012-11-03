using System;

namespace Snowball
{
	public interface IHostControl
	{
		/// <summary>
		/// Gets a handle to the window.
		/// </summary>
		IntPtr Handle { get; }

		/// <summary>
		/// Gets or sets the width of the display area of the host.
		/// </summary>
		int DisplayWidth { get; set; }

		/// <summary>
		/// Gets or sets the height of the display area of the host.
		/// </summary>
		int DisplayHeight { get; set; }

		/// <summary>
		/// Triggered when the size of the display area changes.
		/// </summary>
		event EventHandler DisplaySizeChanged;
	}
}
