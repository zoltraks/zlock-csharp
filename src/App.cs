using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ZLock
{
    internal class App
    {
        private readonly string[] args;

        public Options Options;
        private Logger Logger;

        public int ExitCode { get; internal set; }
        public Execution Execution { get; private set; }
        public bool Running { get; private set; }

        public App(string[] args)
            : this()
        {
            this.args = args;
        }

        private App()
        {
            this.Options = new Options();
            this.Logger = new Logger(this.Options);
        }

        public bool Run()
        {
            Parse();
            if (Options.PrintHelp)
            {
                Help();
                return true;
            }
            if (Options.PrintVersion)
            {
                Version();
                return true;
            }
            if (0 == Options.CommandLine.Count)
            {
                throw new Error.NotEnoughArguments();
            }
            Prepare();
            var execution = new Execution(this.Logger, this.Options);
            execution.OnStart += (x, y) => Running = true;
            execution.OnStop += (x, y) => Running = false;
            bool success = execution.Run();
            ExitCode = execution.ExitCode;
            return success;
        }

        private void Prepare()
        {
            Console.TreatControlCAsInput = false;
            Console.CancelKeyPress += (object sender, ConsoleCancelEventArgs e) =>
            {
                if (Running)
                {
                    e.Cancel = true;
                }
                else
                {
                    Console.WriteLine("Control+C");
                }
            };
        }

        private void Version()
        {
            Console.WriteLine("1.0");
        }

        private void Help()
        {
            Console.Write(@"
Execute process with exclusive file lock

USAGE

    zlock [options] <lock> <command> [<arguments>...]

OPTIONS

    -h
    --help               
                             display this help

    -V
    --version            
                             display version

    -v
    --verbose            
                             increase verbosity

    -n
    --nb
    --nonblock      
                             fail rather than wait

    -w <secs>
    --wait <secs>
    --timeout <secs>     
                             wait for a limited amount of time

    -E <number>
    --conflict-exit-code <number>
    --lock-exit-code <number>
                             exit code after conflict on lock file

    -L <number>
    --limit-exit-code <number>
                             exit code after execution limit reached

    -t <secs>
    --time-limit <secs>
                             limit execution time

AUTHORS

    Filip Golewski

");
        }

        private void Parse()
        {
            var options = Options;
            string next;
            bool skip = false;
            options.LockExitCode = 1;
            options.LimitExitCode = 2;
            for (int i = 0, l = args.Length; i < l; i++)
            {
                string value = args[i];
                if (!skip && value.StartsWith("-"))
                {
                    switch (value)
                    {
                        default:
                            throw new Error.UnknownOption(value);

                        case "-?":
                        case "-h":
                        case "--help":
                            options.PrintHelp = true;
                            break;

                        case "-V":
                        case "--version":
                            options.PrintVersion = true;
                            break;

                        case "-v":
                        case "--verbose":
                            options.Verbose = true;
                            break;

                        case "-n":
                        case "--nb":
                        case "--nonblock":
                            options.NonBlock = true;
                            break;

                        case "-E":
                        case "--conflict-exit-code":
                        case "--lock-exit-code":
                            try
                            {
                                next = args[++i];
                            } catch (NullReferenceException)
                            {
                                throw new Error.MissingParameterValue(value);
                            }
                            try
                            {
                                options.LockExitCode = int.Parse(next);
                            }
                            catch (Exception)
                            {
                                throw new Error.InvalidParameterValue(value, next);
                            }
                            break;

                        case "-L":
                        case "--limit-exit-code":
                            try
                            {
                                next = args[++i];
                            }
                            catch (NullReferenceException)
                            {
                                throw new Error.MissingParameterValue(value);
                            }
                            try
                            {
                                options.LimitExitCode = int.Parse(next);
                            }
                            catch (Exception)
                            {
                                throw new Error.InvalidParameterValue(value, next);
                            }
                            break;

                        case "-t":
                        case "--time":
                        case "--time-limit":
                            try
                            {
                                next = args[++i];
                            }
                            catch (IndexOutOfRangeException)
                            {
                                throw new Error.MissingParameterValue(value);
                            }
                            try
                            {
                                options.TimeLimit = int.Parse(next);
                            }
                            catch (Exception)
                            {
                                throw new Error.InvalidParameterValue(value, next);
                            }
                            break;

                        case "-w":
                        case "--wait":
                        case "--timeout":
                            try
                            {
                                next = args[++i];
                            }
                            catch (NullReferenceException)
                            {
                                throw new Error.MissingParameterValue(value);
                            }
                            try
                            {
                                options.WaitTime = int.Parse(next);
                            }
                            catch (Exception)
                            {
                                throw new Error.InvalidParameterValue(value, next);
                            }
                            break;
                    }
                }
                else
                {
                    skip = true;
                    if (null == options.LockFile)
                    {
                        options.LockFile = value;
                    } else
                    {
                        options.CommandLine.Add(value);
                    }
                }
            }
        }
    }
}
