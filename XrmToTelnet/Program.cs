using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telnet;
using System.Threading;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using XrmToTelnet.DataAccess;

namespace XrmToTelnet
{
    class Program
    {

        static void Main(string[] args)
        {
            List<tbl_Task> tasks = DataConnector.Instance.ProcessUrgentTasks();
            //debugConsole.WriteLine("Urgent tasks: " + tasks.Count);
            //debugList<string> nophones = new List<string>();
            //debugint i = 1;
            foreach (tbl_Task t in tasks)
            {
                string sms = "Terrasoft: " + Transliter.Transliterate(t.Title);
                if(sms.Length>160)                    
                {
                    sms.Remove(157);
                    sms+="...";
                }                
                //debugstring contact = DataConnector.Instance.GetContactName(t.OwnerID);
                //debugConsole.WriteLine("");
                //debugConsole.WriteLine("Task #" + i++ + " title: " + );
                //debugConsole.WriteLine("Task owner: " + contact);

                List<string> nums = DataConnector.Instance.GetContactPhoneNumbers(t.OwnerID);
                if (nums.Count == 0)                
                {
                    sms = "Terrasoft: not found " + DataConnector.Instance.GetContactName(t.OwnerID);
                    string AdminPhone = System.Configuration.ConfigurationManager.AppSettings["admin_phone"];
                    GSMGate.SendSms(AdminPhone, sms);                 
                }
                foreach (string num in nums)
                {             
                    GSMGate.SendSms(num, sms);
                }
                
            }            
        }
    }
}
