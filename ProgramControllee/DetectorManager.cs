using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnityTest
{
    class DetectorManager
    {
        Dictionary<string, Detector> detectorMap = new Dictionary<string, Detector>();
        public DetectorManager() { }

        public void CreateAndOpenDetector(string? path, string? prefix)
        {
            Detector detector = new Detector(path, prefix);
            detectorMap.Add(detector.GetPrefix(), detector);
            detector.Run();
        }
        public void RemoveDetector(string prefix) {
            if (detectorMap.ContainsKey(prefix))
            {
                detectorMap.Remove(prefix);
            }
        }
    }
}
