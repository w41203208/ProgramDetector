

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
            _lastUpdateTime = DateTime.Now;
        }
        public string GetPrefix()
        {
            return _prefix;
        }

        public void Run()
        {
            Thread t = new Thread(startProcess);
            t.Start();

            if(t.IsAlive)
            {
                _t = t;
                Console.WriteLine($"Thread State {_t.ThreadState}");
            }
        }

        private void startProcess()
        {
            if(_path == null)
            {
                return;
            }
            ProcessStartInfo startInfo;
            if (_args != null)
            {
                startInfo = new ProcessStartInfo(_path, _args);
            }
            else{
                startInfo = new ProcessStartInfo(_path);
            }
            

            startInfo.WindowStyle = ProcessWindowStyle.Minimized;

            _proc = Process.Start(startInfo);

            _pname = _proc.ProcessName + " - " + _prefix;

            //string instanceName = Utils.GetProcessInstanceName(_proc.Id);
            //Console.WriteLine(instanceName);

            //Console.WriteLine(_proc.ProcessName);

            try
            {
                
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
                    }
                    
                } while (!_proc.WaitForExit(1000));

                Console.WriteLine($"Process exit code          : {_proc.ExitCode}");
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
                _proc.Kill();
            }
            
        }

        public void Stop()
        {
            try
            {
                if (_t != null)
                {
                    if (_t.IsAlive)
                    {

                        _t.Interrupt();
                        //_proc?.Close();
                    }
                }
            }
            catch(Exception e)
            {

            }
            
        }

    }
}
