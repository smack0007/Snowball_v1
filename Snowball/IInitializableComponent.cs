using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Snowball
{
	/// <summary>
	/// Interface for game components which need to be initialized.
	/// </summary>
	public interface IInitializableComponent : IGameComponent
	{
		/// <summary>
		/// Whether or not the component has been initialized.
		/// </summary>
		bool IsInitialized { get; }

		/// <summary>
		/// Allows the component to initialize itself.
		/// </summary>
		void Initialize();
	}
}
