using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telnet;
using System.Threading;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using XrmToTelnet.DataAccess;
using XrmToTelnet.Netland;

namespace XrmToTelnet
{
    class Program
    {

        static void Main(string[] args)
        {
            TaskSmsSender sender = new TaskSmsSender();
            sender.SendSms(args);
        }
    }
}
