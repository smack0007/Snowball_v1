using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Snowball.Tools.Commands
{
    public interface ICommandLogger
    {
        void WriteError(string text);

        void WriteError(string format, params object[] args);
    }
}
