using System;
using System.Collections.Generic;
using System.Text;

namespace BankLedgerConsole
{
    class Account
    {
        public int AcctNumber { get; set; }
        public string Password { get; set; }
        public double Balance { get; set; }

        public Account(int acctNum, string pswd)
        {
            AcctNumber = acctNum;
            Password = pswd;
            Balance = 0d;
        }

        public void Deposit(double depositAmt)
        {
            double initialBal = Balance;
            Balance = Balance + depositAmt;
            double endingBal = Balance;

            //Log transaction
            Ledger.Transactions.Add(new Transaction(AcctNumber, initialBal, endingBal));
        }

        public void Withdraw(double withdrawAmt)
        {
            double initialBal = Balance;
            Balance = Balance - withdrawAmt;
            double endingBal = Balance;

            //Log transaction
            Ledger.Transactions.Add(new Transaction(AcctNumber, initialBal, endingBal));
        }

        public void Login()
        {
            Console.Write("Password: ");
            string pswdInput = Console.ReadLine();

            //Validate password
            if (pswdInput != Password)
            {
                Console.WriteLine(string.Format("Incorrect password for account number {0}.", AcctNumber));
            }
            else
            {
                //Set authentication status
                Ledger.Authenticated = true;
                Ledger.CurrentAcctNum = AcctNumber;

                //Enable and disable appropriate commands
                Ledger.Commands[1].Available = false;
                Ledger.Commands[2].Available = false;
                Ledger.Commands[3].Available = true;
                Ledger.Commands[4].Available = true;
                Ledger.Commands[5].Available = true;
                Ledger.Commands[6].Available = true;
                Ledger.Commands[7].Available = true;
            }            
        }

        public void Logout()
        {
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
        }
    }
}
