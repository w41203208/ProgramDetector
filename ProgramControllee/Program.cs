﻿// See https://aka.ms/new-console-template for more information


using System.Diagnostics;
using System.IO.Pipes;
using UnityTest;

class Program
{
    static void Main(string[] args)
    {
        DetectorManager detecterMgr = new DetectorManager();
        CommandManager cmdMgr = new CommandManager();
        CommandFactory cmdFactory = new CommandFactory(detecterMgr);

        PipeChannel pipe = new PipeChannel(cmdFactory, cmdMgr, PipeDirection.In, "unity-pipe");
        Thread pipeThread = new Thread(pipe.Start);
        pipeThread.Start();


        Thread cmdThread = new Thread(cmdMgr.Consume);
        cmdThread.Start();




        return;
    }
}



//bool endApp = false;
//// Display title as the C# console calculator app.
//Console.WriteLine("Console Calculator in C#\r");
//Console.WriteLine("------------------------\n");

//while (!endApp)
//{
//    // Declare variables and set to empty.
//    string numInput1 = "";
//    string numInput2 = "";
//    double result = 0;

//    // Ask the user to type the first number.
//    Console.Write("Type a number, and then press Enter: ");
//    numInput1 = Console.ReadLine();

//    double cleanNum1 = 0;
//    while (!double.TryParse(numInput1, out cleanNum1))
//    {
//        Console.Write("This is not valid input. Please enter an integer value: ");
//        numInput1 = Console.ReadLine();
//    }

//    // Ask the user to type the second number.
//    Console.Write("Type another number, and then press Enter: ");
//    numInput2 = Console.ReadLine();

//    double cleanNum2 = 0;
//    while (!double.TryParse(numInput2, out cleanNum2))
//    {
//        Console.Write("This is not valid input. Please enter an integer value: ");
//        numInput2 = Console.ReadLine();
//    }

//    // Ask the user to choose an operator.
//    Console.WriteLine("Choose an operator from the following list:");
//    Console.WriteLine("\ta - Add");
//    Console.WriteLine("\ts - Subtract");
//    Console.WriteLine("\tm - Multiply");
//    Console.WriteLine("\td - Divide");
//    Console.Write("Your option? ");

//    string op = Console.ReadLine();



//    try
//    {
//        result = Calculator.DoOperation(cleanNum1, cleanNum2, op);
//        if (double.IsNaN(result))
//        {
//            Console.WriteLine("This operation will result in a mathematical error.\n");
//        }
//        else Console.WriteLine("Your result: {0:0.##}\n", result);
//    }
//    catch (Exception e)
//    {
//        Console.WriteLine("Oh no! An exception occurred trying to do the math.\n - Details: " + e.Message);
//    }

//    Console.WriteLine("------------------------\n");

//    // Wait for the user to respond before closing.
//    Console.Write("Press 'n' and Enter to close the app, or press any other key and Enter to continue: ");
//    if (Console.ReadLine() == "n") endApp = true;

//    Console.WriteLine("\n"); // Friendly linespacing.
//}