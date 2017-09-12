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
            //Check login status
            string authenticated = Authenticated ? "" : "not ";
            string account = Authenticated ? " to account number " + CurrentAcctNum.ToString() : "";

            //Create list of available commands
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
            //Check command availability
            if(!Commands[1].Available)
            {
                return string.Format("{0} is not an available command.", Commands[1].Name);
            }

            //Set account number
            int newAcctNum = Ledger.Accounts.Count + 1;

            //Input password
            Console.WriteLine(string.Format("Creating account {0}. Enter a password for this account.", newAcctNum));
            string newAcctPswd = Console.ReadLine();

            //Create account, add to ledger, and make "login" available.
            Ledger.Accounts.Add(new Account(newAcctNum, newAcctPswd));
            Ledger.Commands[2].Available = true;

            return string.Format("Your account number is: {0}. Log in with your password to make an initial deposit.", newAcctNum);
        }

        public static string Login()
        {
            //Check command availability
            if (!Commands[2].Available)
            {
                return string.Format("{0} is not an available command.", Commands[2].Name);
            }

            Console.Write("Account number: ");
            string acctNumInput = Console.ReadLine();

            //Validate account number input
            try
            {
                int acctNumTry = Int32.Parse(acctNumInput);
            }
            catch
            {
                return string.Format("{0} is not a valid account number. Login aborted.", acctNumInput);
            }
            int acctNum = Int32.Parse(acctNumInput);
            if(acctNum < 1 || acctNum > Ledger.Accounts.Count)
            {
                return string.Format("{0} is not a valid account number. Login aborted.", acctNumInput);
            }

            Console.Write("Password: ");
            string pswdInput = Console.ReadLine();

            //Validate password
            Account currentAcct = Ledger.Accounts.Find(x => x.AcctNumber == acctNum);
            if(pswdInput != currentAcct.Password)
            {
                return string.Format("Incorrect password for account number {0}.", acctNum);
            }

            //Set authentication status
            Ledger.Authenticated = true;
            Ledger.CurrentAcctNum = acctNum;

            //Enable and disable appropriate commands
            Ledger.Commands[1].Available = false;
            Ledger.Commands[2].Available = false;
            Ledger.Commands[3].Available = true;
            Ledger.Commands[4].Available = true;
            Ledger.Commands[5].Available = true;
            Ledger.Commands[6].Available = true;
            Ledger.Commands[7].Available = true;

            return Status();
        }



        public static string Logout()
        {
            //Check command availability
            if (!Commands[7].Available)
            {
                return string.Format("{0} is not an available command.", Commands[7].Name);
            }

            //Set authentication status
            Ledger.Authenticated = false;
            Ledger.CurrentAcctNum = 0;

            //Enable and disable appropriate commands
            Ledger.Commands[1].Available = true;
            Ledger.Commands[2].Available = true;
            Ledger.Commands[3].Available = false;
            Ledger.Commands[4].Available = false;
            Ledger.Commands[5].Available = false;
            Ledger.Commands[6].Available = false;
            Ledger.Commands[7].Available = false;

            return Status();
        }
    }
}
