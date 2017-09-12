using System;
using System.Collections.Generic;
using System.Text;

namespace BankLedgerConsole
{
    class Ledger
    {
        public static List<Account> Accounts { get; set; }
        public static List<Transaction> Transactions { get; set; }
        public static bool Authenticated { get; set; }
        public static int CurrentAcctNum { get; set; }
        public string[] Messages { get; set; }
        public static int MessageIndex { get; set; }
        public static List<Command> Commands { get; set; }

        public Ledger()
        {
            Accounts = new List<Account> { };
            Transactions = new List<Transaction> { };
            Authenticated = false;
            Messages = new string[]
            {
                "",
                "No accounts in ledger. Please create a new account.",
                "Account created. Log in to make an initial deposit.",
                "Passwords do not match. Try again.",
                "Incorrect password.",
                "That account number does not exist.",
                "You are not logged in to the account you tried to access.",
                "You have logged out."
            };
            MessageIndex = 1;
            Commands = new List<Command>
            {
                new Command("Status", true),
                new Command("Create new account", true),
                new Command("Login", false),
                new Command("Deposit", false),
                new Command("Withdraw", false),
                new Command("Check balance", false),
                new Command("Transaction history", false),
                new Command("Log out", false)
            };
        }

        public static string Status()
        {
            string authenticated = Authenticated ? "" : "not ";
            string account = Authenticated ? " to account number " + CurrentAcctNum.ToString() : "";

            List<Command> availableCommands = Commands.FindAll(x => x.Available == true);
            string availableCommandsString = "Available commands:";
            for (int i = 0; i < availableCommands.Count; i++)
            {
                availableCommandsString += " |" + availableCommands[i].Name + "|";
            }

            return string.Format("You are {0}logged in{1}. {2}", authenticated, account, availableCommandsString);
        }

        public static string CreateAccount()
        {
            int newAcctNum = Ledger.Accounts.Count + 1;
            Console.WriteLine(string.Format("Creating account {0}. Enter a password for this account.", newAcctNum));
            string newAcctPswd = Console.ReadLine();

            //Create account, add to ledger, and make "login" available.
            Ledger.Accounts.Add(new Account(newAcctNum, newAcctPswd));
            Ledger.Commands[2].Available = true;

            return string.Format("Your account number is: {0}. Log in with your password to make an initial deposit.", newAcctNum);
        }
    }
}
