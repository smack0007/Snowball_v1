using System;
using System.IO;

namespace Snowball.Tools.Commands
{
    public interface ICommand
    {
        Type OptionsType { get; }

        string Description { get; }

        /// <summary>
        /// Allows the Command to set defaults for options as needed and indicate any non simple errors in options.
        /// </summary>
        /// <param name="options"></param>
        bool EnsureOptions(object options, ICommandLogger logger);

        void Execute(object options, ICommandLogger logger);
    }
}
