using System;

namespace Snowball
{
	/// <summary>
	/// Base class for resources.
	/// </summary>
	public abstract class Resource : IDisposable
	{
		public bool IsDisposed
		{
			get;
			private set;
		}

		public Resource()
		{
		}

		~Resource()
		{
			this.Dispose(false);
		}

		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
			this.IsDisposed = true;
		}

		protected virtual void Dispose(bool disposing)
		{
		}
	}
}
