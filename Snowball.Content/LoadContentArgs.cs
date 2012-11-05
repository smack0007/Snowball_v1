using System;

namespace Snowball.Content
{
	/// <summary>
	/// Base class for arguments related to loading content.
	/// </summary>
	public class LoadContentArgs
	{		
		/// <summary>
		/// The file name to load.
		/// </summary>
		public string FileName
		{
			get;
			set;
		}
		
		/// <summary>
		/// Constructor.
		/// </summary>
		public LoadContentArgs()
		{
		}
	}
}
