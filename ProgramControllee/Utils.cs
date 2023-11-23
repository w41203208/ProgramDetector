using System.Diagnostics;

namespace UnityTest
{
    class Message
    {
        public string? Type { get; set; }
        public string? Tags { get; set; }
    }

    class Utils
    {
        public static int process_number = 0;
        public static int GetProcessNumber()
        {

            return process_number++;
        }

        // may have to fix this function 
        public static Message SplitMessageString(string msg)
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
        public static List<string> PraseMessageTags(string tagsStr)
        {

            List<string> tags = new ();
            string[] words = tagsStr.Split('$');
            for (int i = 1; i < words.Length; i++)
            {
                string[] keyValue = words[i].Split("=");
                if (keyValue.Length == 2)
                {
                    tags.Add(keyValue[0] + "=" + keyValue[1]);
                }
                else
                {
                    // Error
                }
            }
            return tags;
        }
        public static string? GetTagString(List<string> tags,string tag)
        {
            foreach (var tagg in tags)
            {
                string[] keyValue = tagg.Split("=");
                if (keyValue.Length == 2)
                {
                    if (keyValue[0] == tag)
                    {
                        return keyValue[1];
                    }
                }
            }
            return null;
        }
        public static List<string> RemoveTagString(List<string> tags, string tag)
        {
            List<string> newTags = new();
            foreach (var tagg in tags)
            {
                string[] keyValue = tagg.Split("=");
                if (keyValue.Length == 2)
                {
                    if (keyValue[0] != tag)
                    {
                        newTags.Add(tagg);
                    }
                }
            }
            return newTags;
        }

        public static string TransformTags(List<string> tags)
        {
            string newTagStr = "";
            foreach (var tag in tags)
            {
                string[] keyValue = tag.Split("=");
                if (keyValue.Length == 2)
                {
                    string key = keyValue[0];
                    string value = keyValue[1];
                    if (key != tag)
                    {
                        newTagStr += key;
                        newTagStr += "=";
                        newTagStr += value;
                        newTagStr += " ";
                    }
                }
                else
                {
                    // Error
                }
            }
            return newTagStr;
        }

        public static string GetProcessInstanceName(int pid)
        {
            PerformanceCounterCategory cat = new PerformanceCounterCategory("Process");

            string[] instances = cat.GetInstanceNames();
            foreach (string instance in instances)
            {

                using (PerformanceCounter cnt = new PerformanceCounter("Process",
                     "ID Process", instance, true))
                {
                    int val = (int)cnt.RawValue;
                    if (val == pid)
                    {
                        return instance;
                    }
                }
            }
            throw new Exception("Could not find performance counter " +
                "instance name for current process. This is truly strange ...");
        }
    }
}
