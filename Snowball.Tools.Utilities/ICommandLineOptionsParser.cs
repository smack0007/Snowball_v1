using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Snowball.Tools.Utilities
{
    public interface ICommandLineOptionsParser
    {
        bool Parse(string[] args, out object optionsObject, Action<string> onError);

        void WriteUsage(TextWriter textWriter, string name);

        void WriteOptions(TextWriter textWriter);
    }
}
