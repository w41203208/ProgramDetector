using System.Diagnostics;

namespace UnityTest
{
    interface ICommand
    {
        void Execute();
    }

    class Command
    {
        protected string? _argsString;
        public Command(string? argsString)
        {
            _argsString = argsString;
        }
    }

    class OpenCommand : Command,  ICommand
    {
        private DetectorManager _detecterMgr;
        private string _path;
        public OpenCommand(DetectorManager detecterMgr, string path, string? argsString): base(argsString)
        {
            _detecterMgr = detecterMgr;
            _path = path;
        }
        public void Execute()
        {
            Console.WriteLine($"Open Handle Execute");
            string? prefix = null;
            string? args = null;
            List<string> tags = new();
            if (_argsString != null)
            {
                tags = Utils.PraseMessageTags(_argsString);
                prefix = Utils.GetTagString(tags, "prefix");
                Console.WriteLine($"Prefix: {prefix}");
                if (prefix != null)
                {
                    tags = Utils.RemoveTagString(tags, "prefix");
                }
                args = Utils.TransformTags(tags);
            }
            Console.WriteLine($"Args: {args}");
            _detecterMgr.CreateAndOpenDetector(_path, prefix, args);
        }

    }

    class CloseCommand : Command, ICommand
    {
        private DetectorManager _detecterMgr;
        public CloseCommand(DetectorManager detecterMgr, string? argsString) : base(argsString)
        {
            _detecterMgr = detecterMgr;
        }
        public void Execute()
        {
            Console.WriteLine($"Close Handle Execute");
            List<string> tags = new();
            string? prefix;
            if (_argsString != null )
            {
                tags = Utils.PraseMessageTags(_argsString);
                prefix = Utils.GetTagString(tags, "prefix");
                Console.WriteLine($"Prefix: {prefix}");
                if (prefix != null)
                {
                    _detecterMgr.RemoveDetector(prefix);
                }
                else
                {
                    // Error
                }
            }
            else
            {
                // Error
            }
        }
    }
}
