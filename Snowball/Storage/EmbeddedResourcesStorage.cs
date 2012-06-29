using System;
using System.IO;
using System.Reflection;

namespace Snowball.Storage
{
	public class EmbeddedResourcesStorage : IStorage
	{
		Assembly assembly;

		public string BaseNamespace
		{
			get;
			private set;
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		public EmbeddedResourcesStorage()
			: this(Assembly.GetEntryAssembly(), string.Empty)
		{
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		public EmbeddedResourcesStorage(Assembly assembly)
			: this(assembly, string.Empty)
		{
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		public EmbeddedResourcesStorage(string baseNamespace)
			: this(Assembly.GetEntryAssembly(), baseNamespace)
		{
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="assembly"></param>
		public EmbeddedResourcesStorage(Assembly assembly, string baseNamespace)
		{
			if (assembly == null)
				throw new ArgumentNullException("assembly");

			this.assembly = assembly;
			this.BaseNamespace = baseNamespace;
		}

		public Stream GetStream(string fileName)
		{
			if (this.BaseNamespace != null && this.BaseNamespace.Length > 0)
				fileName = this.BaseNamespace + "." + fileName;

			Stream stream = this.assembly.GetManifestResourceStream(fileName);

			if (stream == null)
				throw new FileNotFoundException("Unable to get Stream to \"" + fileName + "\".");

			return stream;
		}
	}
}
