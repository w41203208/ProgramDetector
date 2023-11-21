using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnityTest
{
    interface ICommand
    {
        void Execute();
    }

    class OpenCommand : ICommand
    {
        private DetectorManager _dMgr;
        private string? _prefix = null;
        private string? _path = null;
        public OpenCommand(DetectorManager mgr, string? path, string? prefix)
        {
            _dMgr = mgr;
            _path = path;
            _prefix = prefix;
        }
        public void Execute()
        {
            _dMgr.CreateAndOpenDetector(_path, _prefix);
        }
    }

    class CloseCommand : ICommand
    {
        private DetectorManager _dMgr;
        private string _prefix;
        public CloseCommand(DetectorManager mgr, string prefix)
        {
            _dMgr = mgr;
            _prefix = prefix;
        }
        public void Execute()
        {
            _dMgr.RemoveDetector(_prefix);
        }
    }
}
