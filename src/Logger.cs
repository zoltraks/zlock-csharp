using System;

namespace ZLock
{
    public class Logger
    {
        private Options Options;

        public Logger(Options configuration)
        {
            Options = configuration;
        }

        public void Write(Exception x)
        {
            if (Options.Verbose)
            {
                DateTime now = DateTime.Now;
                var line = ""
                    + now.ToString("yyyy-MM-dd HH:mm:ss.fff ")
                    + x.Message
                    ;
                Console.Error.WriteLine(line);
            }
        }

        public void Write(string message)
        {
            DateTime now = DateTime.Now;
            var line = ""
                + now.ToString("yyyy-MM-dd HH:mm:ss.fff ")
                + message
                ;
            Console.WriteLine(line);
        }
    }
}
