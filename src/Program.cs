using System;

namespace ZLock
{
    internal class Program
    {
        static int Main(string[] args)
        {
            int exitCode = 1;
            bool verbose = false;
            try
            {
                var app = new App(args);
                app.Run();
                exitCode = app.ExitCode;
                verbose = app.Options.Verbose;
            }
            catch (Error.NotEnoughArguments)
            {
                Console.WriteLine("Not enough arguments. Use --help for more information.");
            }
            catch (Exception x)
            {
                Console.WriteLine(x.Message);
            }
            if (verbose)
            {
                Console.WriteLine($"Exit with %ERRORLEVEL% {exitCode}");
            }
            return exitCode;
        }
    }
}
