using System.IO.Pipes;

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
            Message msg = splitMessageString(input);
            ICommand? cmd = null;
            switch (msg.Type)
            {
                case "open":
                    cmd = _factory.CreateCommand(CommandType.Open, msg.Tags);
                    break;
                case "close":
                    cmd = _factory.CreateCommand(CommandType.Close, msg.Tags);
                    break;
            }
            _cmdMgr.AddCommand(cmd);
        }

        private Message splitMessageString(string msg)
        {
            string[] words = msg.Split(" ");

            if (words.Length == 1)
            {
                return new Message
                {
                    Type = words[0],
                    Tags = null,
                };
            }

            if (words.Length > 2 || words.Length < 1)
            {
                // Error format is invalid
                return new Message
                {
                    Type = null,
                    Tags = null,
                };
            }
            else
            {
                return new Message
                {
                    Type = words[0],
                    Tags = words[1]
                };
            }
        }

        private class Message
        {
            public string? Type { get; set; }
            public string? Tags { get; set; }
        }
    }
}
