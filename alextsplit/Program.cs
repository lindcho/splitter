using System;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.IO;

namespace alextsplit
{
    class Program
    {
        private static readonly Script Script = new Script();
        static void Main(string[] args)
        {
            var rootCommand = CreateOptions();

            rootCommand.Name = "alextsplit";

            rootCommand.Handler =
                CommandHandler.Create<string, int>(ExecuteFileAndReturnExitCode);

            rootCommand.InvokeAsync(args).Wait();
        }

        private static RootCommand CreateOptions()
        {
            RootCommand rootCommand = new RootCommand(
                description: "Splits a log file into small text files with the specified number of lines each.");
            Option nameOption = new Option<string>(
                aliases: new string[] { "--name", "-n" }
                , description: "The log file path to be processed.");
            rootCommand.AddOption(nameOption);

            Option linesOption = new Option<int>(
                aliases: new string[] { "--lines", "-l" }
                , description: "The number of lines to write to each file." +
                               "The default number of lines if not provided is 2000",
                getDefaultValue: () => 2000);
            rootCommand.AddOption(linesOption);

            return rootCommand;
        }

        private static int ExecuteFileAndReturnExitCode(string name, int lines)
        {
            if (!File.Exists(name))
            {

                Console.WriteLine($"option {name} not recognized");
                Console.WriteLine("Run 'alextsplit  --help' for more information on a command..");

            }
            else
            {
                Script.ExecuteFile(name, lines);
            }

            return 0;
        }

    }
}
