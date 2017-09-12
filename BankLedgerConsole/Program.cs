using System;
using System.Collections.Generic;

namespace BankLedgerConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = typeof(Program).Name;

            // Instantiate Ledger
            Ledger ledger = new Ledger();

            Run();
        }

        //Basic input/feedback loop (from https://www.codeproject.com/Articles/816301/Csharp-Building-a-Useful-Extensible-NET-Console-Ap)
        static void Run()
        {
            while (true)
            {
                var consoleInput = ReadFromConsole();
                if (string.IsNullOrWhiteSpace(consoleInput)) continue;

                try
                {
                    // Execute the command:
                    string result = Execute(consoleInput);

                    // Write out the result:
                    WriteToConsole(result);
                }
                catch (Exception ex)
                {
                    // OOPS! Something went wrong - Write out the problem:
                    WriteToConsole(ex.Message + "/" + ex.TargetSite);
                }
            }
        }

        //Examine input and react accordingly
        static string Execute(string command)
        {
            
            switch(command)
            {
                case "Status":
                    return Ledger.Status();

                case "Create new account":
                    return Ledger.CreateAccount();

                default:
                    return string.Format("{0} is not a valid command.", command);
            }
            
        }


        public static void WriteToConsole(string message = "")
        {
            if (message.Length > 0)
            {
                Console.WriteLine(message);
            }
        }


        const string _readPrompt = "console> ";
        public static string ReadFromConsole(string promptMessage = "")
        {
            // Show a prompt, and get input:
            Console.Write(_readPrompt + promptMessage);
            return Console.ReadLine();
        }

    }
}