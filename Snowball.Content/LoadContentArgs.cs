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
		/// If true, the content need only be loaded once and cached for all other load requests.
		/// </summary>
		public bool UseCache
		{
			get;
			set;
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		public LoadContentArgs()
		{
			this.UseCache = true;
		}
	}
}
