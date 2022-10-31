using System;
using System.Threading;
using System.IO;
using System.Security;
using System.Diagnostics;

namespace ZLock
{
    public class Execution
    {
        const int DEFAULT_SHORT_DELAY = 1000;

        const int DEFAULT_LONG_DELAY = 30000;

        private ManualResetEvent _Semaphore = new ManualResetEvent(false);

        private FileStream _LockFileStream;

        private Logger Logger;

        public Options Options { get; }
        public Process RunningProcess { get; private set; }
        public int ExitCode { get; private set; }

        public event EventHandler OnStart;

        public event EventHandler OnStop;

        public Execution(Logger logger, Options options)
        {
            Logger = logger;
            Options = options;
        }

        public bool Lock()
        {
            int delay;
            long limit;
            limit = Options.WaitTime * 1000;
            do
            {
                if (!Options.NonBlock && limit <= 0)
                {
                    limit = long.MaxValue;
                }
                try
                {
                    string file = Options.LockFile;
                    _LockFileStream = new FileStream(file, FileMode.Create, FileAccess.ReadWrite, FileShare.None);
                    return true;
                }
                catch (SecurityException _SecurityException)
                {
                    delay = DEFAULT_LONG_DELAY;
                    Logger.Write((Exception)_SecurityException);
                }
                catch (Exception _Exception)
                {
                    delay = DEFAULT_SHORT_DELAY;
                    Logger.Write((Exception)_Exception);
                }
                if (delay > limit)
                {
                    delay = (int)limit;
                }
                Thread.Sleep(delay);
            } while ((limit -= delay) > 0);
            return false;
        }

        public bool Run()
        {
            if (!Lock())
            {
                ExitCode = Options.LockExitCode;
                return false;
            }
            long limit = Options.TimeLimit * 1000;
            int delay = 60000;
            try
            {
                Start();

                if (limit <= 0)
                {
                    _Semaphore.WaitOne();
                }
                else
                {
                    bool sky = true;
                    while (limit > 0)
                    {
                        if (limit < delay)
                        {
                            delay = (int)limit;
                        }
                        if (_Semaphore.WaitOne(delay))
                        {
                            sky = false;
                            break;
                        }
                        limit -= delay;
                    }
                    if (sky)
                    {
                        ExitCode = Options.LimitExitCode;
                    }
                }

                Stop();

                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                Unlock();
            }
        }

        private void Start()
        {
            Thread thread = new Thread(new ThreadStart(() =>
            {
                Process process = new Process();

                process.StartInfo.FileName = Options.FileName;
                process.StartInfo.Arguments = Options.Arguments;
                process.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
                process.StartInfo.UseShellExecute = false;

                RunningProcess = process;

                process.Start();

                process.WaitForExit();

                if (!Options.Kill)
                {
                    try
                    {
                        var exitCode = process.ExitCode;
                        ExitCode = exitCode;
                    }
                    catch (InvalidOperationException)
                    {
                        Debug.WriteLine("Process killed");
                    }
                }

                _Semaphore.Set();
            }))
            {
                IsBackground = true,
            };
            thread.Start();
            if (OnStart != null)
            {
                OnStart(this, null);
            }
        }

        private void Stop()
        {
            try
            {
                Process process = RunningProcess;

                if (process == null || process.HasExited)
                {
                    return;
                }

                try
                {
                    process.CloseMainWindow();
                }
                catch (Exception x)
                {
                    Debug.WriteLine(x.Message);
                }

                Options.Kill = true;

                process.Kill();

                process.Close();
            }
            finally
            {
                if (OnStop != null)
                {
                    OnStop(this, null);
                }
            }
        }

        public void Unlock()
        {
            if (_LockFileStream != null)
            {
                try
                {
                    _LockFileStream.Dispose();
                    if (!Options.Keep)
                    {
                        File.Delete(Options.LockFile);
                    }
                }
                catch (Exception _Exception)
                {
                    Logger.Write(_Exception);
                }
            }
        }
    }
}
