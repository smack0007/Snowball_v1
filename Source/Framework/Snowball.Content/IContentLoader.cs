using System;

namespace Snowball.Content
{
	/// <summary>
	/// Interface for ContentLoader(s).
	/// </summary>
	public interface IContentLoader
	{
		/// <summary>
		/// Allows the content loader to ensure the given args are valid for the given content type.
		/// </summary>
		/// <param name="args"></param>
		void EnsureArgs<T>(LoadContentArgs args)
			where T : class;
		
		/// <summary>
		/// Loads content.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="args"></param>
		/// <returns></returns>
		T Load<T>(LoadContentArgs args)
			where T : class;
	}
}
