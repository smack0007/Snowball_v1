using System;

namespace Snowball.Collections
{
	/// <summary>
	/// Interface for objects which manage a collection of preallocated objects.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public interface IObjectPool<T>
	{
		/// <summary>
		/// Returns the next inactive object from the pool.
		/// </summary>
		T Next();
				
		/// <summary>
		/// Marks an object from the pool as inactive.
		/// </summary>
		/// <param name="obj"></param>
		void Return(T obj);
	}
}
