

using System.Diagnostics;
using System.Threading;

namespace UnityTest
{

    interface IExecuter
    {
        void Run();
    }
    class Detector
    {
        private string? _pname; //process-name
        private readonly string _prefix;
        private readonly string? _args;
        private readonly string? _path;
        private bool _closeSignal;
        private DateTime _lastUpdateTime;

        private Thread? _t;
        private Process? _proc;
        public Detector(string? path, string? prefix, string? args)
        {
            _t = null;
            _prefix = prefix ?? Utils.GetProcessNumber().ToString();
            _path = path;
            _args = args;
            _proc = null;
            _closeSignal = true;
            _lastUpdateTime = DateTime.Now;
        }
        public string GetPrefix()
        {
            return _prefix;
        }

        public void Run()
        {
            Thread t = new Thread(StartProccess);
            t.Start();

            if (t.IsAlive)
            {
                _t = t;
                Console.WriteLine($"Thread State {_t.ThreadState}");
            }
        }

        private void StartProccess()
        {
            if (_path == null)
            {
                return;
            }

            // set proccess info
            ProcessStartInfo startInfo;
            if (_args != null)
            {
                startInfo = new ProcessStartInfo(_path, _args);
            }
            else
            {
                startInfo = new ProcessStartInfo(_path);
            }

            startInfo.WindowStyle = ProcessWindowStyle.Minimized;
            //string instanceName = Utils.GetProcessInstanceName(_proc.Id);
            //Console.WriteLine(instanceName);
            //Console.WriteLine(_proc.ProcessName);

            try
            {
                using(_proc = Process.Start(startInfo))
                {
                    if (_proc != null && !_proc.HasExited)
                    {
                        _pname = _proc.ProcessName + " - " + _prefix;

                        do
                        {
                            var now = DateTime.Now;
                            var timeDuration = now - _lastUpdateTime;
                            _lastUpdateTime = now;

                            if (timeDuration.TotalSeconds > 1)
                            {
                                Console.Clear();
                                //// use pc
                                //PerformanceCounter pc = new PerformanceCounter("Processor", "% Processor Time", instanceName, true);
                                //Console.WriteLine("CPU: {0:n1}%", pc.NextValue());

                                Console.WriteLine($"{_pname}");
                                Console.WriteLine("-------------------------------------");
                                Console.WriteLine($"  Physical memory usage     : {_proc.WorkingSet64}");
                                Console.WriteLine($"  Base priority             : {_proc.BasePriority}");
                                Console.WriteLine($"  Priority class            : {_proc.PriorityClass}");
                                Console.WriteLine($"  User processor time       : {_proc.UserProcessorTime}");
                                Console.WriteLine($"  Privileged processor time : {_proc.PrivilegedProcessorTime}");
                                Console.WriteLine($"  Total processor time      : {_proc.TotalProcessorTime}");
                                Console.WriteLine($"  Paged system memory size  : {_proc.PagedSystemMemorySize64}");
                                Console.WriteLine($"  Paged memory size         : {_proc.PagedMemorySize64}");
                                Console.WriteLine($"  Print signal: {_closeSignal}");
                            }
                        } while (!_proc.WaitForExit(1000) && _closeSignal);

                        Console.WriteLine($"  Process exit code          : {_proc.ExitCode}");

                        //Checking proccess has closed
                        if (_proc.HasExited)
                        {
                            Console.WriteLine($"Proccess - [{_pname}] is closed");
                        }
                        else
                        {
                            Console.WriteLine($"Proccess - [{_pname}] isn't closed");
                        }

                    }
                }
            }
            catch(Exception e)
            {
                Console.WriteLine($"Error: {e}");

                if (_proc != null)
                {
                    // kill proccess
                    if (!_proc.CloseMainWindow())
                    {
                        _proc.Kill();
                    }
                }
            }
            finally
            {
                if (_proc != null)
                {
                    _proc.Dispose();
                }
                _proc = null;
            }
            
        }

        public void Stop()
        {
            try
            {
                if (_proc != null)
                {
                    if (!_proc.HasExited)
                    {
                        //_closeSignal = false;
                        _proc.CloseMainWindow();

                        //_proc.CloseMainWindow();
                    }
                }
            }
            catch(Exception e)
            {

            }
        }
    }
}
