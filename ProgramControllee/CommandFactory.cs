

namespace UnityTest
{
    public enum CommandType
    {
        Open = 0,
        Close = 1,
    }
    class CommandFactory
    {
        private DetectorManager _detecterMgr;
        
        public CommandFactory(DetectorManager detecterMgr) 
        {
            _detecterMgr = detecterMgr;
        }

        public ICommand? CreateCommand(CommandType type, string? msgTags)
        {
            ICommand? newCmd;
            switch(type)
            {
                case CommandType.Open:
                    newCmd = CreateOpenCommand(msgTags);
                    break;
                case CommandType.Close:
                    newCmd = CreateCloseCommand(msgTags);
                    break;
                default:
                    newCmd = null;
                    break;
            }

            return newCmd;
        }

        public ICommand? CreateOpenCommand(string? msgTags)
        {
            string path = "D:\\MingProgram\\unity-learning\\build\\Coin Pusher\\Coin Pusher.exe";
            return new OpenCommand(_detecterMgr, path, msgTags);
        }

        public ICommand? CreateCloseCommand(string? msgTags)
        {
            return new CloseCommand(_detecterMgr, msgTags);
        }
    }
}
