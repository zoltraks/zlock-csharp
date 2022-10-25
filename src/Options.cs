using System;
using System.Collections.Generic;

namespace ZLock
{
    public class Options
    {
        public bool Verbose;
        public bool NonBlock;
        public int WaitTime;
        public int LockExitCode;
        public int TimeLimit;

        public IList<string> CommandLine = new List<string>();

        public string FileName { get => CommandLine.Count == 0 ? "" : CommandLine[0]; }
        public string Arguments { get => GetArguments(); }
        public string LockFile { get; internal set; }
        public bool PrintHelp { get; internal set; }
        public bool PrintVersion { get; internal set; }
        public int LimitExitCode { get; internal set; }
        public bool Kill { get; internal set; }

        private string GetArguments()
        {
            if (CommandLine.Count < 2)
            {
                return "";
            }
            List<string> list = new List<string>(CommandLine.Count - 1);
            for (int i = 1, n = CommandLine.Count; i < n; i++)
            {
                list.Add(CommandLine[i]);
            }
            return string.Join(" ", list.ToArray());
        }
    }
}