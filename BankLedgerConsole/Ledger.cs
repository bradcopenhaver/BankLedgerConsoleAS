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
                new Command("Log in", false),
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

        public static string LogIn()
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

            //Retrieve account and login
            Account currentAcct = Ledger.Accounts.Find(x => x.AcctNumber == acctNum);
            currentAcct.Login();            

            return Status();
        }

        public static string Deposit()
        {
            //Check command availability
            if (!Commands[3].Available)
            {
                return string.Format("{0} is not an available command.", Commands[3].Name);
            }

            Console.Write("Amount to deposit: ");
            string inputAmount = Console.ReadLine();

            //Validate input
            try
            {
                double depAmtTry = double.Parse(inputAmount, System.Globalization.NumberStyles.Currency);
            }
            catch
            {
                return string.Format("{0} is not a valid deposit amount. Please submit the amount in dollars (d) and cents (c) in the format 'ddd.cc'", inputAmount);
            }
            double depAmt = double.Parse(inputAmount, System.Globalization.NumberStyles.Currency);
            if(depAmt <= 0) return string.Format("{0} is not a valid deposit amount. Deposits must be greater than zero", inputAmount);

            //Retrieve account and apply deposit.
            Account currentAcct = Ledger.Accounts.Find(x => x.AcctNumber == Ledger.CurrentAcctNum);            
            currentAcct.Deposit(depAmt);

            return CheckBalance();
        }

        public static string Withdraw()
        {
            //Check command availability
            if (!Commands[4].Available)
            {
                return string.Format("{0} is not an available command.", Commands[4].Name);
            }

            Console.Write("Amount to withdraw: ");
            string inputAmount = Console.ReadLine();

            //Retrieve account 
            Account currentAcct = Ledger.Accounts.Find(x => x.AcctNumber == Ledger.CurrentAcctNum);

            //Validate input
            try
            {
                double wdAmtTry = double.Parse(inputAmount, System.Globalization.NumberStyles.Currency);                
            }
            catch
            {
                return string.Format("{0} is not a valid withdraw amount. Please submit the amount in dollars (d) and cents (c) in the format 'ddd.cc'", inputAmount);
            }
            double wdAmt = double.Parse(inputAmount, System.Globalization.NumberStyles.Currency);
            if (wdAmt < 0) return string.Format("{0} is not a valid withdraw amount. Withdraws must be greater than zero.", inputAmount);
            if (wdAmt > currentAcct.Balance) return string.Format("{0} is not a valid withdraw amount. The amount exceeds the current account balance.", inputAmount);
            
            //Apply withdraw
            currentAcct.Withdraw(wdAmt);

            return CheckBalance();
        }

        public static string CheckBalance()
        {
            //Check command availability
            if (!Commands[5].Available)
            {
                return string.Format("{0} is not an available command.", Commands[5].Name);
            }

            //Retrieve account 
            Account currentAcct = Ledger.Accounts.Find(x => x.AcctNumber == Ledger.CurrentAcctNum);

            return string.Format("Account {0} current balance: {1}", currentAcct.AcctNumber, currentAcct.Balance);
        }

        public static string TransactionHistory()
        {
            //Check command availability
            if (!Commands[6].Available)
            {
                return string.Format("{0} is not an available command.", Commands[6].Name);
            }

            //Compile relevant transactions
            List<Transaction> relevantTransactions = Ledger.Transactions.FindAll(x => x.AccountNumber == Ledger.CurrentAcctNum);

            //Display list
            for(int i=0; i<relevantTransactions.Count; i++)
            {
                string transactionType = "Deposit";
                if (relevantTransactions[i].StartingBalance > relevantTransactions[i].EndingBalance) transactionType = "Withdraw";
                Console.WriteLine(string.Format("{0}: |{1}| Initial balance {2} ; Ending balance {3}", relevantTransactions[i].TimeOfTransaction, transactionType, relevantTransactions[i].StartingBalance, relevantTransactions[i].EndingBalance));
            }

            return "------End of list------";
        }

        public static string LogOut()
        {
            //Check command availability
            if (!Commands[7].Available)
            {
                return string.Format("{0} is not an available command.", Commands[7].Name);
            }

            //Retrieve account and logouts
            Account currentAcct = Ledger.Accounts.Find(x => x.AcctNumber == Ledger.CurrentAcctNum);
            currentAcct.Logout();

            return Status();
        }
    }
}
