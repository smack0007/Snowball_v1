using System;

namespace Snowball.Graphics
{
	/// <summary>
	/// Exception thrown when something related to graphics happens.
	/// </summary>
	public class GraphicsException : SnowballException
	{
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="message"></param>
		public GraphicsException(string message)
			: base(message)
		{
		}

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public GraphicsException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
	}
}
