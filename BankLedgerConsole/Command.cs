using System;
using System.Collections.Generic;
using System.Text;

namespace BankLedgerConsole
{
    class Command
    {
        public string Name { get; set; }
        public bool Available { get; set; }

        public Command(string name, bool available)
        {
            Name = name;
            Available = available;
        }
    }
}
