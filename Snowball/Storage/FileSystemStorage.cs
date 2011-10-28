using System;
using System.IO;

namespace Snowball.Storage
{
	public class FileSystemStorage : IStorage
	{
		public string BasePath
		{
			get;
			private set;
		}

		public FileSystemStorage()
		{
			this.BasePath = Environment.CurrentDirectory;
		}

		public FileSystemStorage(string basePath)
		{
			if(string.IsNullOrEmpty(basePath))
				throw new ArgumentNullException("basePath");

			this.BasePath = basePath;
		}
		
		public Stream GetStream(string fileName)
		{
			return File.OpenRead(Path.Combine(this.BasePath, fileName));	
		}
	}
}
