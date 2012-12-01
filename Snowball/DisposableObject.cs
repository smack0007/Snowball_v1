using System;

namespace Snowball
{
	/// <summary>
	/// Base class for game resources.
	/// </summary>
	public abstract class DisposableObject : IDisposable
	{
		/// <summary>
		/// Indicates if the resource has already been disposed of.
		/// </summary>
		public bool IsDisposed
		{
			get;
			private set;
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		public DisposableObject()
		{
		}

		/// <summary>
		/// Destructor.
		/// </summary>
		~DisposableObject()
		{
			this.Dispose(false);
		}

		/// <summary>
		/// Disposes of the resource.
		/// </summary>
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
			this.IsDisposed = true;
		}

		/// <summary>
		/// Implemented in child classes to handle disposing of unmanaged resources.
		/// </summary>
		/// <param name="disposing"></param>
		protected virtual void Dispose(bool disposing)
		{
		}
	}
}
