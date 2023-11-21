using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnityTest
{
    class CommandManager
    {
        public Queue<ICommand> commnadList = new Queue<ICommand>();
        public CommandManager() { }

        public void AddCommand(ICommand? command)
        {
            if(command != null)
            {
                commnadList.Enqueue(command);
            }
        }

        public void Consume()
        {
            
            while (true)
            {
                if(commnadList.Count > 0)
                {
                    ICommand cmd = commnadList.Dequeue();
                    cmd.Execute();
                }
            }
        }
    }
}
