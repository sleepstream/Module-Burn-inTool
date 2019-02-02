using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Threading;
using System.IO.Ports;
using System.Globalization;
using System.Diagnostics;

namespace Module_Burn_inTool
{
    public class Primes
    {
        private string rsPort;
        private SerialPort comPrimes;
        private double PWR;
        private double TIN;
        private double WF;

        public Primes(string Port)
        {
            Debug.Write("Primes constructor\r");
            rsPort = Port;
            Debug.Write(rsPort+"++++++\r");
            comPrimes = new SerialPort(rsPort, 9600, System.IO.Ports.Parity.Space, 8, StopBits.One);
            comPrimes.DtrEnable = true;
            comPrimes.RtsEnable = true;
            comPrimes.Encoding = Encoding.UTF8;
            Debug.Write(rsPort + "++++++\r");
        }
        public void Connect()
        {
            int rd;
            byte[] rcv = new byte[255];
            
            comPrimes.Open();
            Debug.Write(rsPort + "$$$$\r");
            Thread.Sleep(100);
            
            
            byte[] snd = new byte[11];
            snd[0] = (int)'q';
            snd[1] = (int)'@';
            snd[2] = 0;
            snd[3] = 0;
            snd[4] = 2;
            snd[5] = (int)'q';
            snd[6] = (int)'l';
            snd[7] = (int)'V';
            snd[8] = (int)'p';
            snd[9] = (int)'@';
            snd[10] = 216;
            comPrimes.Write(snd, 0, 11);

            
           
            Thread.Sleep(100);
            rd = comPrimes.Read(rcv, 0, comPrimes.BytesToRead);
            

        }
        public void Disconnect()
        {
            Debug.Write("Primes disconnect\r");
            comPrimes.Close();
        }
        public void GetData()
        {

            
            byte[] snd = new byte[11];
            snd[0] = (int)'q';
            snd[1] = (int)'@';
            snd[2] = 0;
            snd[3] = 0;
            snd[4] = 2;
            snd[5] = (int)'q';
            snd[6] = (int)'l';
            snd[7] = (int)'V';
            snd[8] = (int)'p';
            snd[9] = (int)'@';
            snd[10] = 216;
            comPrimes.Write(snd, 0, 11);
            Thread.Sleep(500);
            

            int rd;
            byte[] rcv = new byte[255];
            rd = comPrimes.Read(rcv, 0, comPrimes.BytesToRead);
            for (int i = 0; i < 255; i++)
            {
                if (rcv[i] == 0) rcv[i] = 63;
            }
            string s1 = System.Text.Encoding.ASCII.GetString(rcv);

            s1.Replace('?', ' ');
            string si1 = "PL=";
            string si2 = "shutter";
            int i1 = s1.IndexOf(si1);
            if (i1 == -1) i1 = 0;
            int i2 = s1.IndexOf(si2);
            s1 = s1.Substring(i1, 55);

            //s1 = s1.Replace('.', ',');
            string pwr;
            string tin;

            string wf;
            pwr = s1.Substring(s1.IndexOf("PL")+3, 10);

            tin = s1.Substring(s1.IndexOf("Te") + 3, 10);
            wf = s1.Substring(s1.IndexOf("Fl")+3, 10);
            //textBox3.Text = "PWR=" + PWR + Environment.NewLine + "TIN=" + TIN + Environment.NewLine + "WF=" + WF; 

            try
            {
                PWR = Convert.ToDouble(pwr.Trim(), CultureInfo.InvariantCulture);
                TIN = Convert.ToDouble(tin.Trim(), CultureInfo.InvariantCulture);
                WF = Convert.ToDouble(wf.Trim(), CultureInfo.InvariantCulture);
            }
            catch (Exception ex)
            {
                TIN = -1;
            }


        }
        public double GetTin()
        {

            return TIN;
        }
        public double GetPower()
        {

            return PWR;
        }
        public double GetWF()
        {

            return WF;
        }
        ~Primes()
        {
            Debug.Write("Primes destructor\r");
            comPrimes.Close();
        }
    }
}
