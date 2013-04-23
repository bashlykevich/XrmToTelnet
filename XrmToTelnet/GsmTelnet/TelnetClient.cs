using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Threading;
using System.Text.RegularExpressions;

namespace XrmToTelnet
{
    class TelnetClient
    {        
        TcpClient tcpClient;
        NetworkStream ns;
        int timeOut = 1000;

        public TelnetClient(string ip, int port, int timeOutInMs)
        {            
            tcpClient = new TcpClient(ip, port);
            timeOut = timeOutInMs;                
            ns = tcpClient.GetStream();
        }
        public bool Connect(string username, string password)
        {
            int tryCount = 0;            
            while (tryCount++ < 10)
            {                
                Byte[] output = new Byte[1024];
                String responseoutput = String.Empty;
                Byte[] cmd = System.Text.Encoding.ASCII.GetBytes("\n");
                ns.Write(cmd, 0, cmd.Length);

                Thread.Sleep(timeOut);
                Int32 bytes = ns.Read(output, 0, output.Length);
                responseoutput = System.Text.Encoding.ASCII.GetString(output, 0, bytes);
                Console.Write(responseoutput);

                Regex objToMatch = new Regex("username:");
                if (objToMatch.IsMatch(responseoutput))
                {
                    cmd = System.Text.Encoding.ASCII.GetBytes(username + "\r");
                    ns.Write(cmd, 0, cmd.Length);
                }
                
                Thread.Sleep(timeOut);
                bytes = ns.Read(output, 0, output.Length);
                responseoutput = System.Text.Encoding.ASCII.GetString(output, 0, bytes);
                Console.Write(responseoutput);
                objToMatch = new Regex("password:");
                if (objToMatch.IsMatch(responseoutput))
                {
                    cmd = System.Text.Encoding.ASCII.GetBytes(password + "\r");
                    ns.Write(cmd, 0, cmd.Length);
                }

                Thread.Sleep(timeOut);
                bytes = ns.Read(output, 0, output.Length);
                responseoutput = System.Text.Encoding.ASCII.GetString(output, 0, bytes);
                Console.Write(responseoutput);

                //objToMatch = new Regex("#");
                //if (objToMatch.IsMatch(responseoutput))
                if(responseoutput.Contains("admin"))
                {
                    return true;
                }
            }            
            return false;
        }

        public bool Command(string command)
        {
            bool flag = true;
            Byte[] cmd = System.Text.Encoding.ASCII.GetBytes("\n");
            Byte[] output = new Byte[1024];
            String responseoutput = String.Empty;

            cmd = System.Text.Encoding.ASCII.GetBytes(command + "\r");
            ns.Write(cmd, 0, cmd.Length);                
            Thread.Sleep(timeOut);
            Int32 bytes = ns.Read(output, 0, output.Length);
            responseoutput = System.Text.Encoding.ASCII.GetString(output, 0, bytes);
            Console.Write(responseoutput);
            return flag;
        }
        public void Disconnect()
        {
            if (tcpClient.Connected)
                tcpClient.Close();
        }
    }
}
