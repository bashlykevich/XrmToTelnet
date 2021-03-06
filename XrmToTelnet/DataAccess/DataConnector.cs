﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XrmToTelnet.DataAccess
{
    public class DataConnector
    {
        static TSXRM3Entities db;
        private static DataConnector instance;
        private DataConnector() { }
        public static DataConnector Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new DataConnector();
                    db = new TSXRM3Entities();
                }
                return instance;
            }
        }

        public List<tbl_Task> ProcessUrgentTasks()
        {            
            List<tbl_Task> tasks = new List<tbl_Task>();
            foreach (tbl_SmsJournal queue in db.tbl_SmsJournal)
            {                
                tasks.Add(queue.Task);
            }            
            while (db.tbl_SmsJournal.Count() > 0)
            {
                db.tbl_SmsJournal.DeleteObject(db.tbl_SmsJournal.First());
                db.SaveChanges();
            }
            return tasks;
        }
        public string GetContactName(Guid? ContactID)
        {
            return db.tbl_Contact.FirstOrDefault(x => x.ID == ContactID).Name;
        }
        public List<string> GetContactPhoneNumbers(Guid? ContactID)
        {
            List<string> nums = new List<string>();
            tbl_Contact contact = db.tbl_Contact.FirstOrDefault(c => c.ID == ContactID);
            if (contact == null)
                return nums;            
            string s1 = isBelarusMobilePhone(contact.Communication1);
            if (!String.IsNullOrEmpty(s1))
                nums.Add(s1);
            string s2 = isBelarusMobilePhone(contact.Communication2);
            if (!String.IsNullOrEmpty(s2))
                nums.Add(s2);
            string s3 = isBelarusMobilePhone(contact.Communication3);
            if (!String.IsNullOrEmpty(s3))
                nums.Add(s3);
            string s4 = isBelarusMobilePhone(contact.Communication4);
            if (!String.IsNullOrEmpty(s4))
                nums.Add(s4);
            return nums;            
        }
        string isBelarusMobilePhone(string s)
        {
            if (String.IsNullOrEmpty(s))
                return String.Empty;
            s = s.Trim();
            s = s.Replace(" ", "");
            if (s.Contains("37525") || s.Contains("37529") || s.Contains("37533") || s.Contains("37544"))
                return s;
            else
                return String.Empty;
        }
    }
}