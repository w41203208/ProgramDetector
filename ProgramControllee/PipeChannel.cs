using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnityTest
{
    class PipeChannel
    {
        private NamedPipeClientStream _pipeConn;
        private StreamReader? _sr;
        private CommandFactory _factory;


        private CommandManager _cmdMgr;
        public PipeChannel(CommandFactory cmdFactory, CommandManager cmdMgr, PipeDirection direction, string connectionServerPipeName)
        {
            _factory = cmdFactory;
            _cmdMgr = cmdMgr;
            _sr = null;
            _pipeConn = new NamedPipeClientStream(".", connectionServerPipeName, direction); // "unity-pipe PipeDirection.In
            _pipeConn.Connect();
        }

        public void Start()
        {
            _sr = new StreamReader(_pipeConn);
            string receive;
            while (true)
            {
                while ((receive = _sr.ReadLine()) != null)
                {
                    Console.WriteLine("[CLIENT] Echo: " + receive);
                    handle(receive);
                }
            }
        }

        private void handle(string input)
        {
            Message msg = Utils.SplitMessageString(input);
            ICommand? cmd = null;
            switch (msg.Type)
            {
                case "open":
                    cmd = _factory.CreateCommand(CommandFactory.CommandType.Open, msg.Tag);
                    break;
                case "close":
                    cmd = _factory.CreateCommand(CommandFactory.CommandType.Close, msg.Tag);
                    break;
            }
            _cmdMgr.AddCommand(cmd);
        }
    }
}
