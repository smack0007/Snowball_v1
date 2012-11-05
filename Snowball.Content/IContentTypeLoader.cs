using System;

namespace Snowball.Content
{
	public interface IContentTypeLoader
	{
		/// <summary>
		/// The parent content loader which is managing the type loader.
		/// </summary>
		IContentLoader ContentLoader { get; set; }

		/// <summary>
		/// Allows the content type loader to ensure the given args are valid.
		/// </summary>
		/// <param name="args"></param>
		void EnsureArgs(LoadContentArgs args);
	}

	/// <summary>
	/// Strongly typed interface for content type loaders.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public interface IContentTypeLoader<T> : IContentTypeLoader
		where T : class
	{
		/// <summary>
		/// Loads content.
		/// </summary>
		/// <param name="storage"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		T Load(IStorage storage, LoadContentArgs args);
	}
}
