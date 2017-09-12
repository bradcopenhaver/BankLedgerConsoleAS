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
        public List<Command> Commands { get; set; }

        public Account(int acctNum, string pswd)
        {
            AcctNumber = acctNum;
            Password = pswd;
            Balance = 0d;
            Commands = new List<Command>
            {
                new Command("Deposit", true),
                new Command("Withdraw", false)
            };
        }

        public void Deposit(double depositAmt)
        {
            double initialBal = Balance;
            Balance = Balance + depositAmt;
            double endingBal = Balance;

            //Log transaction
            Ledger.Transactions.Add(new Transaction(AcctNumber, initialBal, endingBal));

            //Enable Withdraw
            Commands[1].Available = true;
        }

        public void Withdraw(double withdrawAmt)
        {
            double initialBal = Balance;
            Balance = Balance - withdrawAmt;
            double endingBal = Balance;

            //Log transaction
            Ledger.Transactions.Add(new Transaction(AcctNumber, initialBal, endingBal));
        }        
    }
}
