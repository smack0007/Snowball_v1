using System;
using System.IO;

namespace Snowball.Content
{
	/// <summary>
	/// Interface for objects which provide access to content storage.
	/// </summary>
	public interface IStorage
	{
		/// <summary>
		/// Gets a stream handle to the given file.
		/// </summary>
		/// <param name="fileName"></param>
		/// <returns></returns>
		Stream GetStream(string fileName);
	}
}
