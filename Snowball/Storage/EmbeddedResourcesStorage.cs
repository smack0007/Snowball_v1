using System;
using System.IO;
using System.Reflection;

namespace Snowball.Storage
{
	public class EmbeddedResourcesStorage : IStorage
	{
		Assembly assembly;

		/// <summary>
		/// Constructor.
		/// </summary>
		public EmbeddedResourcesStorage()
			: this(Assembly.GetEntryAssembly())
		{
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="assembly"></param>
		public EmbeddedResourcesStorage(Assembly assembly)
		{
			if (assembly == null)
				throw new ArgumentNullException("assembly");

			this.assembly = assembly;
		}

		public Stream GetStream(string fileName)
		{
			return this.assembly.GetManifestResourceStream("fileName");
		}
	}
}
