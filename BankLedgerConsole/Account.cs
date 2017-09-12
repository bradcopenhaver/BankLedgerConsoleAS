﻿using System;
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

        public void Deposit(float depositAmt)
        {
            Balance = Balance + depositAmt;
        }

        public void Withdraw(float withdrawAmt)
        {
            Balance = Balance - withdrawAmt;
        }
    }
}
