
namespace UnityTest
{
    class DetectorManager
    {
        Dictionary<string, Detector> detectorMap = new Dictionary<string, Detector>();
        public DetectorManager() { }

        public void CreateAndOpenDetector(string? path, string? prefix, string? args)
        {
            Detector detector = new Detector(path, prefix, args);
            detectorMap.Add(detector.GetPrefix(), detector);
            detector.Run();
        }
        public void RemoveDetector(string prefix) {
            if (detectorMap.ContainsKey(prefix))
            {
                Detector? detector;
                detectorMap.TryGetValue(prefix, out detector);
                if(detector != null)
                {
                    Console.WriteLine($"Close Deterctor: {prefix}");
                    detector.Stop();
                    detectorMap.Remove(prefix);
                }
            }
        }
    }
}
