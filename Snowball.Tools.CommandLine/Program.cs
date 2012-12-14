using Snowball.Tools.Commands;
using Snowball.Tools.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Snowball.Tools.CommandLine
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            Dictionary<string, Type> commands = typeof(ICommand).Assembly
                .GetTypes()
                .Where(x => x.GetInterfaces().Contains(typeof(ICommand)))
                .ToDictionary(x => x.Name.Substring(0, x.Name.Length - "Command".Length));

            if (args.Length > 0)
            {
                if (commands.ContainsKey(args[0]))
                {
                    ICommand command = CreateCommandInstance(commands[args[0]]);

                    ICommandLineOptionsParser optionsParser = (ICommandLineOptionsParser)Activator.CreateInstance(typeof(CommandLineOptionsParser<>).MakeGenericType(command.OptionsType));

                    string errorText = string.Empty;
                    Action<string> onError = (error) => { errorText = error; };

                    object options;
                    if (optionsParser.Parse(args.Skip(1).Take(args.Length - 1).ToArray(), out options, onError) &&
                        command.EnsureOptions(options, onError))
                    {
                        command.Execute(options);
                    }
                    else
                    {
                        Console.WriteLine("ERROR: Options not valid for command \"{0}\".", args[0]);
                        Console.WriteLine(errorText);
                    }
                }
                else
                {
                    Console.WriteLine("ERROR: Unknown command \"{0}\".", args[0]);
                    Console.WriteLine();
                    ListCommands(commands);
                }
            }
            else
            {
                Console.WriteLine("ERROR: Please prvoide a command to execute.");
                Console.WriteLine();
                ListCommands(commands);
            }
        }

        private static ICommand CreateCommandInstance(Type commandType)
        {
            return (ICommand)Activator.CreateInstance(commandType);
        }

        private static void ListCommands(Dictionary<string, Type> commands)
        {
            Console.WriteLine("Commands:");
            Console.WriteLine();

            foreach (string key in commands.Keys)
            {
                ICommand command = CreateCommandInstance(commands[key]);

                Console.WriteLine(key);
                Console.WriteLine("  {0}", command.Description);
            }
        }
    }
}
