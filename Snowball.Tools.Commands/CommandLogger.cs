using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Snowball.Tools.Commands
{
    public abstract class CommandLogger : ICommandLogger
    {
        public abstract void WriteError(string text);

        public void WriteError(string format, params object[] args)
        {
            this.WriteError(string.Format(format, args));
        }
    }
}
