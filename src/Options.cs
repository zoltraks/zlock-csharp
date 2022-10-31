using System;
using System.Collections.Generic;

namespace ZLock
{
    public class Options
    {
        public IList<string> CommandLine = new List<string>();

        public string FileName { get => CommandLine.Count == 0 ? "" : CommandLine[0]; }
        public string Arguments { get => GetArguments(); }

        public bool Verbose { get; internal set; }
        public bool NonBlock { get; internal set; }
        public int WaitTime { get; internal set; }
        public int LockExitCode { get; internal set; }
        public int TimeLimit { get; internal set; }
        public string LockFile { get; internal set; }
        public bool PrintHelp { get; internal set; }
        public bool PrintVersion { get; internal set; }
        public int LimitExitCode { get; internal set; }
        public bool Kill { get; internal set; }
        public bool Keep { get; internal set; }
        public bool Quiet { get; internal set; }

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