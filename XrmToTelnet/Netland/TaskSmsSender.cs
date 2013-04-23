using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XrmToTelnet.DataAccess;

namespace XrmToTelnet.Netland
{
    class TaskSmsSender
    {
        public void SendSms(string[] args)
        {
            List<tbl_Task> tasks = DataConnector.Instance.ProcessUrgentTasks();

            foreach (tbl_Task t in tasks)
            {
                string sms = "Terrasoft: " + Transliter.Transliterate(t.Title);
                if (sms.Length > 160)
                {
                    sms.Remove(157);
                    sms += "...";
                }

                List<string> nums = DataConnector.Instance.GetContactPhoneNumbers(t.OwnerID);
                if (nums.Count == 0)
                {
                    sms = "Terrasoft: not found " + Transliter.Transliterate(DataConnector.Instance.GetContactName(t.OwnerID));
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
