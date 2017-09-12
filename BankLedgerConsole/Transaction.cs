using System;
using System.Collections.Generic;
using System.Text;

namespace BankLedgerConsole
{
    class Transaction
    {
        public int AccountNumber { get; set; }
        public double StartingBalance { get; set; }
        public double EndingBalance { get; set; }
        public DateTime TimeOfTransaction { get; set; }

        public Transaction(int acctNum, double startBal, double endBal)
        {
            AccountNumber = acctNum;
            StartingBalance = startBal;
            EndingBalance = endBal;
            TimeOfTransaction = DateTime.Now;
        }
    }
}
