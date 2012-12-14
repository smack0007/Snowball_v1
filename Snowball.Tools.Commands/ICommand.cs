using System;

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
        bool EnsureOptions(object options, Action<string> onError);

        void Execute(object options);
    }
}
