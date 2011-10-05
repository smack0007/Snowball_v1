using System;
using System.IO;

namespace Snowball.Content
{
	public class FileStorageSystem : IContentStorageSystem
	{
		public string BasePath
		{
			get;
			private set;
		}

		public FileStorageSystem()
		{
			this.BasePath = Environment.CurrentDirectory;
		}

		public FileStorageSystem(string basePath)
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
