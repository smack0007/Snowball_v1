using System;

namespace Snowball
{
	/// <summary>
	/// Base class for exceptions in the framework.
	/// </summary>
	public class SnowballException : Exception
	{
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="message"></param>
		public SnowballException(string message)
			: base(message)
		{
		}

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public SnowballException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
	}
}
