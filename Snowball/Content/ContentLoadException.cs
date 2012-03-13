using System;

namespace Snowball.Content
{
	/// <summary>
	/// Exception thrown whenever there is an error related to loading content.
	/// </summary>
	public class ContentLoadException : InvalidOperationException
	{
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="message"></param>
		public ContentLoadException(string message)
			: base(message)
		{
		}
	}
}
