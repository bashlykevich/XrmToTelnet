using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XrmToTelnet
{
    class GSMGate
    {
        public static void SendSms(string number, string sms)
        {
            string ip = System.Configuration.ConfigurationManager.AppSettings["gsmgate_ip"];
            int port = 8023;
            Int32.TryParse(System.Configuration.ConfigurationManager.AppSettings["gsmgate_port"], out port);
            int timeout = 2000;
            Int32.TryParse(System.Configuration.ConfigurationManager.AppSettings["gsmgate_timeout"], out timeout);
            string login = System.Configuration.ConfigurationManager.AppSettings["gsmgate_login"];
            string password = System.Configuration.ConfigurationManager.AppSettings["gsmgate_password"];

            TelnetClient tc = new TelnetClient(ip, port, timeout);
            if (!tc.Connect(login, password))
            {
                Console.WriteLine("\nConnection error!");
                return;
            }
            tc.Command("\n");
            tc.Command("module1");
            tc.Command("ATE1");
            tc.Command("AT+CMGF=1");
            tc.Command("AT+CMGS=\"" + number + "\"");
            tc.Command(sms);

            char CtrlZ = (char)26;
            tc.Command(CtrlZ.ToString());

            tc.Disconnect();
        }
    }
}
