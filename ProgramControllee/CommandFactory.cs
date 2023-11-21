namespace UnityTest
{
    class CommandFactory
    {
        private DetectorManager _detectorMgr;
        public enum CommandType
        {
            Open = 0,
            Close = 1,
        }
        public CommandFactory(DetectorManager dMgr) 
        {
            _detectorMgr = dMgr;
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
            string path = "D:\\MingProgram\\unity-learning\\build\\CoinPusher\\Coin Pusher.exe";
            string? prefix = null;
            if (msgTags != null)
            {
                prefix = Utils.GetMessageTagString(msgTags, "prefix");
            }

            return new OpenCommand(_detectorMgr, path, prefix);
        }

        public ICommand? CreateCloseCommand(string? msgTags)
        {
            try
            {
                string? prefix = null;
                if (msgTags != null)
                {
                    prefix = Utils.GetMessageTagString(msgTags, "prefix")!;
                }

                if(prefix == null)
                {
                    new Exception("no prefix");
                }
                return new CloseCommand(_detectorMgr, prefix!);
            }
            catch (Exception e)
            {
                return null;
            }
            
        }
    }
}
