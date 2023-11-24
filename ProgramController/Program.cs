// See https://aka.ms/new-console-template for more information
using System.Diagnostics;
using System.IO.Pipes;

class Program
{
    static void Main(string[] args)
    {
        ProcessStartInfo clientStartInfo = new ProcessStartInfo 
        {
            FileName = "D:\\MingProgram\\asp.net-learning\\ProgramDetector\\ProgramControllee\\bin\\Debug\\net7.0\\ProgramControllee.exe", 
            UseShellExecute = true,
            CreateNoWindow = false,
            WindowStyle = ProcessWindowStyle.Normal,
        };

        Process? subProcess = Process.Start(clientStartInfo);

        using (NamedPipeServerStream pipeServer = new NamedPipeServerStream("pipe", PipeDirection.Out))
        {
            Console.WriteLine("[SERVER] Current TransmissionMode: {0}.",
                pipeServer.TransmissionMode);

            // Wait pipe connection
            pipeServer.WaitForConnection();

            try
            {
                // Read user input and send that to the client process.
                StreamWriter sw = new StreamWriter(pipeServer);
                sw.AutoFlush = true;
                while (true && !subProcess.HasExited)
                {
                    // Send the console input to the client process.
                    Console.Write("[SERVER] Enter text: ");
                    string userInput = Console.ReadLine();

                    if (pipeServer.CanWrite)
                    {
                        sw.WriteLine(userInput);
                    }
                }
            }
            // Catch the IOException that is raised if the pipe is broken
            // or disconnected.
            catch (IOException e)
            {
                Console.WriteLine("[SERVER] Error: {0}", e.Message);
            }
        }


        subProcess.WaitForExit();
        subProcess.Close();
        Console.WriteLine("[SERVER] Client quit. Server terminating.");
    }
}
