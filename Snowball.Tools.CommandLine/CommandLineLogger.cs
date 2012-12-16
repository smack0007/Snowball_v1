using Snowball.Tools.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Snowball.Tools.CommandLine
{
    public class CommandLineLogger : CommandLogger
    {
        public override void WriteError(string text)
        {
            Console.Error.Write("ERROR: ");
            Console.Error.WriteLine(text);
        }
    }
}
