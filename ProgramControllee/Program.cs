// See https://aka.ms/new-console-template for more information

using System.IO.Pipes;
using UnityTest;

class Program
{
    static void Main(string[] args)
    {
        DetectorManager detecterMgr = new DetectorManager();
        CommandManager cmdMgr = new CommandManager();
        CommandFactory cmdFactory = new CommandFactory(detecterMgr);
        PipeChannel pipe = new PipeChannel(cmdFactory, cmdMgr, PipeDirection.In, "pipe");

        Thread pipeThread = new Thread(pipe.Start);
        pipeThread.Start();

        Thread cmdThread = new Thread(cmdMgr.Consume);
        cmdThread.Start();

        return;
    }
}

