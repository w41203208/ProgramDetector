using System.Diagnostics;
using System.Net.Sockets;

namespace UnityTest
{
    class Message
    {
        public string? Type { get; set; }
        public string? Tag { get; set; }
    }

    class Utils
    {
        public static int process_number = 0;
        public static int GetProcessNumber()
        {

            return process_number++;
        }
        public static Message SplitMessageString(string msg)
        {
            string[] words = msg.Split(" ");
            if(words.Length < 2 ) 
            {
                return new Message
                {
                    Type = words[0],
                    Tag = null,
                };
            }
            else
            {
                if (words[1] == "")
                {
                    return new Message
                    {
                        Type = words[0],
                        Tag = null,
                    };
                }
                else
                {
                    return new Message
                    {
                        Type = words[0],
                        Tag = words[1]
                    };
                }
            }
        }
        public static string? GetMessageTagString(string tagsStr,string tag)
        {
            string[] words = tagsStr.Split('$');
            for(int i = 1; i < words.Length; i++)
            {
                string[] keyValue = words[i].Split("=");
                if(keyValue.Length == 2)
                {
                    if (keyValue[0] == tag)
                    {
                        return keyValue[1];
                    }
                }
            }
            return null;
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
