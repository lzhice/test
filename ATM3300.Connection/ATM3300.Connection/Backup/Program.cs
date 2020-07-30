using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using ATM3300.Connection.Forms;

namespace ATM3300.Connection
{
    static class Program
    {
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.Run(new TcpipRcuDiagnostic());
        }
    }
}
