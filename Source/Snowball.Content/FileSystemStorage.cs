using System;
using System.IO;

namespace Snowball.Content
{
	/// <summary>
	/// Storage implementation which reads from the file system.
	/// </summary>
	public class FileSystemStorage : IStorage
	{
		/// <summary>
		/// The base path being read from in the file system.
		/// </summary>
		public string BasePath
		{
			get;
			private set;
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		public FileSystemStorage()
		{
			this.BasePath = Environment.CurrentDirectory;
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="basePath"></param>
		public FileSystemStorage(string basePath)
		{
			if (string.IsNullOrEmpty(basePath))
				throw new ArgumentNullException("basePath");

			this.BasePath = basePath;
		}
		
		/// <summary>
		/// Gets a Stream to a file in the file system.
		/// </summary>
		/// <param name="fileName"></param>
		/// <returns></returns>
		public Stream GetStream(string fileName)
		{
			return File.OpenRead(Path.Combine(this.BasePath, fileName));	
		}
	}
}
