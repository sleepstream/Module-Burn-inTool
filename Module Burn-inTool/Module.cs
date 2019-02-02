using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO.Ports;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Module_Burn_inTool
{
    class Module
    {
        public bool getDnValue = false;

        private Object thisLock = new Object();

        public event EventHandler NewDataAvaliable;
        public bool QCWmode;
        private Thread WorkThread;
        public string ComPortName;
        private SerialPort ComPort;
        public bool comPortErr = false;

        public int Iset;

        public bool Work;
        public bool comInWork = false;
        public bool EmissionOn {  get;  set; }
        public bool resetError { get; set; }
        

        public modStatus Status;
        public modAlarms Alarms;
        public modSettings Settings;
        public modData DataMod;

        public struct modSettings
        {
            public int currentSetMinimum { get; set; }
            public int currentSetMax { get; set; }
            public int currentChangeStep { get; set; }
        }

        public struct modData
        {
            public double ILD1 { get; set; }
            public double ILD2 { get; set; }
            public double ILD3 { get; set; }
            public double ILD4 { get; set; }
            public double ILD5 { get; set; }
            public double ILD6 { get; set; }
            public int PD1 { get; set; }
            public int PD2 { get; set; }
            public int PD3 { get; set; }
            public int PD4 { get; set; }
            public int PD5 { get; set; }
            public double PSVoltage { get; set; }
            public double Uset { get; set; }
            public double Temp { get; set; }
            public string ModuleID { get; set; }
            public string ModuleFirmware { get; set; }
            public double Power { get; set; }
            public bool Error { get; set; }
            public int DP1 { get; set; }
            public int DP2 { get; set; }
            public int DP3 { get; set; }
            public int DP4 { get; set; }
            public int DP5 { get; set; }

        }
        public struct modStatus
        {
            public bool Emission { get; set; }
            public bool Enabled { get; set; }
            public bool PowerSupply { get; set; }
            public bool ExternalStart { get; set; }
            public bool ExternalShutdown { get; set; }
            public bool PulsedMode { get; set; }
            public bool Error { get; set; }
        }

        
        public struct modAlarms
        {
            public bool PD1 { get; set; }
            public bool PD2 { get; set; }
            public bool PD3 { get; set; }
            public bool PD4 { get; set; }
            public bool PD5 { get; set; }
            public bool Overheat { get; set; }
            public bool CurrentLeak { get; set; }
            public bool GndLeak { get; set; }
            public bool PowerSupply { get; set; }
            public bool HighBackReflection { get; set; }
            public bool CS1 { get; set; }
            public bool CS2 { get; set; }
            public bool CS3 { get; set; }
            public bool CS4 { get; set; }
            public bool CS5 { get; set; }
            public bool CS6 { get; set; }
            public bool UnexpectedPump1 { get; set; }
            public bool UnexpectedPump2 { get; set; }
            public bool UnexpectedPump3 { get; set; }
            public bool UnexpectedPump4 { get; set; }
            public bool UnexpectedPump5 { get; set; }
            public bool UnexpectedPump6 { get; set; }
            public bool DutyCycle { get; set; }
            public bool TimeOut { get; set; }
        }

        private static bool GetBit(int b, int bitNumber)
        {
            return (b & (1 << bitNumber)) != 0;
        }

        protected virtual void RaiseNewDataAvaliable()
        {
            // Raise the event by using the () operator.
            if (NewDataAvaliable != null)
                NewDataAvaliable(this, new EventArgs());
        }





        public Module()
        {
            ComPort = new SerialPort();
            ComPort.BaudRate = 115200;
            ComPort.DataBits = 8;
            ComPort.Encoding = System.Text.Encoding.GetEncoding(1252);
            ComPort.Handshake = Handshake.None;
            ComPort.NewLine = "\r";
            ComPort.Parity = Parity.None;
            ComPort.ReadTimeout = 1000;
            ComPort.StopBits = StopBits.One;
            ComPort.WriteTimeout = 3000;
            Iset = 0;
            Work = false;
            this.Settings.currentChangeStep = 500;
            this.Settings.currentSetMax = 4095;
            this.Settings.currentSetMinimum = 0;
            
        }

        ~Module()
        {
            if (ComPort.IsOpen)
            {
                ComPort.Close();
            }
            ComPort.Dispose();
        }

        public void StartCommunication()
        {

            try
            {
                ComPort.PortName = ComPortName;
                ComPort.Open();
               
                //read module ID
                string s = System.Text.Encoding.GetEncoding(1252).GetString(new byte[2] { 0x81, 0x05 });
                try
                {
                    this.DataMod.ModuleID = writeGetDataCom(s, true).Remove(0, 2).Split(';')[0];
                }
                catch (Exception ex)
                {
                    this.comPortErr = true;
                }
                
                //string pattern2 = @"[0-9 \-:A-Za-z]{1,1}";
                /*MatchCollection matches = Regex.Matches(this.DataMod.ModuleID, pattern2);
                if ((matches.Count) != this.DataMod.ModuleID.Length)
                    this.DataMod.Error = true;
                else
                    this.DataMod.Error = false;*/


                s = System.Text.Encoding.GetEncoding(1252).GetString(new byte[2] { 0x81, 0x0A });
                this.DataMod.ModuleFirmware = writeGetDataCom(s, true);

                startWork();
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                this.comPortErr = true;
                
            }
        }
        public void startWork()
        {
            WorkThread = new Thread(RealWork);
            WorkThread.IsBackground = true;
            Work = true;
            WorkThread.Start();
 
        }
        public void StopCommunication()
        {
            try
            {
                Work = false;
                if (WorkThread != null)
                {
                    WorkThread.Join();
                }
                string s = System.Text.Encoding.GetEncoding(1252).GetString(new byte[2] { 0x81, 0x12 });
                s = s + 0 + ";";//Iset0
                s = s + (int)0 + ";";//Iset1
                {//forming "command" sequence
                    int tmp = 0;
                    tmp = tmp + (int)Math.Pow(2, 0) * Convert.ToInt32(false);//emission
                    tmp = tmp + (int)Math.Pow(2, 1) * 0;//DAC0 Iset
                    tmp = tmp + (int)Math.Pow(2, 2) * 0;//No residual current
                    tmp = tmp + (int)Math.Pow(2, 3) * Convert.ToInt32(false);//external start
                    tmp = tmp + (int)Math.Pow(2, 4) * 0;//reset errors
                    tmp = tmp + (int)Math.Pow(2, 5) * 0;//disable module emission (for single module operation)
                    tmp = tmp + (int)Math.Pow(2, 6) * Convert.ToInt32(false);//Pulsed mode
                    s = s + tmp.ToString() + ";";


                }

                s = s + (int)1 + ";";//???adress
                s = s + Crc16.ComputeChecksum(Encoding.GetEncoding(1252).GetBytes(s)).ToString();
                writeGetDataCom(s, false);
                ComPort.Close();
            }
            catch (Exception ex)
            {
            }

        }


        //загрузка прошивки модуля
        public void setFlash(byte[] power)
        {
            this.Work = false;

            Thread.Sleep(100);
            //команда ожидания прошивки
            string s = System.Text.Encoding.GetEncoding(1252).GetString(new byte[2] { 0x80, 0x10 });
            s = s +"FLASH";//
            writeGetDataCom(s, false);

            Thread.Sleep(1000);
            //загрузка прошивки
            writeByteDataCom(power);

            Thread.Sleep(1000);

            this.startWork();
        }



        public void setPower(double power)
        {
            string s = System.Text.Encoding.GetEncoding(1252).GetString(new byte[2] { 0x81, 0x07 });
            s = s + power +";";//
            s = s + Crc16.ComputeChecksum(Encoding.GetEncoding(1252).GetBytes(s)).ToString();
            writeGetDataCom(s, false); 
        }

        public void setDPn(string DPvalue)
        {

            string s = System.Text.Encoding.GetEncoding(1252).GetString(new byte[2] { 0x81, 0x14 });
            s = s + 1 + ";" + DPvalue;//
            s = s + Crc16.ComputeChecksum(Encoding.GetEncoding(1252).GetBytes(s)).ToString();
            writeGetDataCom(s, false);
             
        }

        public void SetSN(string SN)
        {
            this.Work = false;
            Thread.Sleep(100);
            string s = System.Text.Encoding.GetEncoding(1252).GetString(new byte[2] { 0x81, 0x04 });
            s = s + SN+";";//
            s = s + Crc16.ComputeChecksum(Encoding.GetEncoding(1252).GetBytes(s)).ToString();
            writeGetDataCom(s, false);
            Thread.Sleep(100);
            s = System.Text.Encoding.GetEncoding(1252).GetString(new byte[2] { 0x81, 0x05 });
            DataMod.ModuleID = writeGetDataCom(s, true).Remove(0, 2).Split(';')[0];
            this.startWork();
 
        }

        public void getDPn()
        {
            this.Work = false;
            Thread.Sleep(100);

            string s = System.Text.Encoding.GetEncoding(1252).GetString(new byte[2] { 0x81, 0x13 });
            s = s + Crc16.ComputeChecksum(Encoding.GetEncoding(1252).GetBytes(s)).ToString();

            s = writeGetDataCom(s, true);
            //MessageBox.Show(s);
            //strip s from 1st two system symbols
            if (s.IndexOf(System.Text.Encoding.GetEncoding(1252).GetString(new byte[2] { 0x81, 0x13 })) == 0)
            {
                s = s.Remove(0, 2);
                string[] tmp = s.Split(';');
                DataMod.DP1 = Convert.ToInt32(new String(tmp[0].Where(c => char.IsDigit(c)).ToArray())); 
                DataMod.DP2 = Convert.ToInt32(tmp[1]);
                DataMod.DP3 = Convert.ToInt32(tmp[2]);
                DataMod.DP4 = Convert.ToInt32(tmp[3]);
                DataMod.DP5 = Convert.ToInt32(tmp[4]);
            }

            this.startWork();

        }

        public void RealWork()
        {
            //try
            //{
            string Checksum;
                //dummy here
                while (Work)
                {
                    //set ild, update data
                    //form the command

                    string s = System.Text.Encoding.GetEncoding(1252).GetString(new byte[2] { 0x81, 0x12 });
                    s = s + this.Iset + ";";//Iset0
                    s = s + (int)0 + ";";//Iset1
                    {//forming "command" sequence
                        int tmp = 0;
                        tmp = tmp + (int)Math.Pow(2, 0) * Convert.ToInt32(EmissionOn);//emission
                        tmp = tmp + (int)Math.Pow(2, 1) * 0;//DAC0 Iset
                        tmp = tmp + (int)Math.Pow(2, 2) * 0;//No residual current
                        tmp = tmp + (int)Math.Pow(2, 3) * Convert.ToInt32(QCWmode);//external start
                        if(resetError)
                            tmp = tmp + (int)Math.Pow(2, 4) * 1;//reset errors
                        else
                            tmp = tmp + (int)Math.Pow(2, 4) * 0;//reset errors
                        tmp = tmp + (int)Math.Pow(2, 5) * 0;//disable module emission (for single module operation)
                        tmp = tmp + (int)Math.Pow(2, 6) * Convert.ToInt32(QCWmode);//Pulsed mode
                        s = s + tmp.ToString() + ";";

                        resetError = false;
                    }

                    s = s + (int)1 + ";";//???adress
                    Checksum = Crc16.ComputeChecksum(Encoding.GetEncoding(1252).GetBytes(s)).ToString();
                    s = s + Checksum;
                   
                    //wait
                    try
                    {
                        s = writeGetDataCom(s, true);
                        int index = s.IndexOf(System.Text.Encoding.GetEncoding(1252).GetString(new byte[2] { 0x81, 0x12 }));
                        if (s.IndexOf(System.Text.Encoding.GetEncoding(1252).GetString(new byte[2] { 0x81, 0x12 })) == 0)
                        {
                            //strip s from 1st two system symbols
                            s=s.Remove(0, 2);
                            string[] tmp = s.Split(';');
                            //1st string is output power
                            //clear it off control signals
                            tmp[0] = new String(tmp[0].Where(c => char.IsDigit(c)).ToArray());
                            DataMod.Power = Convert.ToDouble(tmp[0]);//2nd string is Temp*10
                            DataMod.Temp = Convert.ToDouble(tmp[1]) / 10;
                            //3rd, 4th, 5th, 6th, 7th and 8th is Current 1 * 100
                            DataMod.ILD1 = Convert.ToDouble(tmp[2]) / 100;
                            DataMod.ILD2 = Convert.ToDouble(tmp[3]) / 100;
                            DataMod.ILD3 = Convert.ToDouble(tmp[4]) / 100;
                            DataMod.ILD4 = Convert.ToDouble(tmp[5]) / 100;
                            DataMod.ILD5 = Convert.ToDouble(tmp[6]) / 100;
                            DataMod.ILD6 = Convert.ToDouble(tmp[7]) / 100;
                            //8th is PD1
                            DataMod.PD1 = Convert.ToInt32(tmp[8]);
                            //9th is PD2
                            DataMod.PD2 = Convert.ToInt32(tmp[9]);
                            //10th is PD3
                            DataMod.PD3 = Convert.ToInt32(tmp[10]);
                            //11th is PD4
                            DataMod.PD4 = Convert.ToInt32(tmp[11]);
                            //12th is PD5
                            DataMod.PD5 = Convert.ToInt32(tmp[12]);
                            //13th is Uset
                            DataMod.Uset = Convert.ToDouble(tmp[13]);
                            //14th is PS Voltage * 10
                            DataMod.PSVoltage = Convert.ToDouble(tmp[14]) / 10;
                            //15th is Status

                            int Status = Convert.ToInt32(tmp[15]);
                            //Debug.WriteLine("Status=" + Status.ToString());
                            this.Status.Emission = GetBit(Status, 0);
                            this.Status.Enabled = GetBit(Status, 1);
                            this.Status.PowerSupply = GetBit(Status, 2);
                            this.Status.ExternalStart = GetBit(Status, 3);
                            this.Status.ExternalShutdown = GetBit(Status, 4);
                            this.Status.PulsedMode = GetBit(Status, 6);
                            this.Status.Error = GetBit(Status, 8);




                            //17th is Alarms
                            Status = Convert.ToInt32(tmp[16]);
                            this.Alarms.PD1 = GetBit(Status, 0);
                            this.Alarms.PD2 = GetBit(Status, 1);
                            this.Alarms.PD3 = GetBit(Status, 2);
                            this.Alarms.PD4 = GetBit(Status, 3);
                            this.Alarms.PD5 = GetBit(Status, 4);

                            this.Alarms.Overheat = GetBit(Status, 5);
                            this.Alarms.CurrentLeak = GetBit(Status, 6);
                            this.Alarms.PowerSupply = GetBit(Status, 7);
                            this.Alarms.CS1 = GetBit(Status, 8);
                            this.Alarms.CS2 = GetBit(Status, 9);
                            this.Alarms.CS3 = GetBit(Status, 10);
                            this.Alarms.CS4 = GetBit(Status, 11);
                            this.Alarms.CS5 = GetBit(Status, 12);
                            this.Alarms.CS6 = GetBit(Status, 13);

                            this.Alarms.UnexpectedPump1 = GetBit(Status, 14);
                            this.Alarms.UnexpectedPump2 = GetBit(Status, 15);
                            this.Alarms.UnexpectedPump3 = GetBit(Status, 16);
                            this.Alarms.UnexpectedPump4 = GetBit(Status, 17);
                            this.Alarms.UnexpectedPump5 = GetBit(Status, 18);
                            this.Alarms.UnexpectedPump6 = GetBit(Status, 19);

                            this.Alarms.DutyCycle = GetBit(Status, 20);
                        }
                        
                    }
                    catch (TimeoutException ex)
                    {
                        DataMod.Power = 0;                //2nd string is Temp*10
                        DataMod.Temp = 0;
                        //3rd, 4th, 5th, 6th, 7th and 8th is Current 1 * 100
                        DataMod.ILD1 = 0;
                        DataMod.ILD2 = 0;
                        DataMod.ILD3 = 0;
                        DataMod.ILD4 = 0;
                        DataMod.ILD5 = 0;
                        DataMod.ILD6 = 0;
                        //8th is PD1
                        DataMod.PD1 = 0;
                        //9th is PD2
                        DataMod.PD2 = 0;
                        //10th is PD3
                        DataMod.PD3 = 0;
                        //11th is PD4
                        DataMod.PD4 = 0;
                        //12th is PD5
                        DataMod.PD5 = 0;
                        //13th is Uset
                        DataMod.Uset = 0;
                        //15th is Status

                        //Debug.WriteLine("Status=" + Status.ToString());
                        this.Status.Emission = false;
                        this.Status.Enabled = false;
                        this.Status.PowerSupply = false;
                        this.Status.ExternalStart = false;
                        this.Status.ExternalShutdown = false;
                        this.Status.PulsedMode = false;
                        this.Status.Error = false;




                        //17th is Alarms

                        this.Alarms.PD1 = false;
                        this.Alarms.PD2 = false;
                        this.Alarms.PD3 = false;
                        this.Alarms.PD4 = false;
                        this.Alarms.PD5 = false;

                        this.Alarms.Overheat = false;
                        this.Alarms.CurrentLeak = false;
                        this.Alarms.PowerSupply = false;
                        this.Alarms.CS1 = false;
                        this.Alarms.CS2 = false;
                        this.Alarms.CS3 = false;
                        this.Alarms.CS4 = false;
                        this.Alarms.CS5 = false;
                        this.Alarms.CS6 = false;

                        this.Alarms.UnexpectedPump1 = false;
                        this.Alarms.UnexpectedPump2 = false;
                        this.Alarms.UnexpectedPump3 = false;
                        this.Alarms.UnexpectedPump4 = false;
                        this.Alarms.UnexpectedPump5 = false;
                        this.Alarms.UnexpectedPump6 = false;

                        this.Alarms.DutyCycle = false;
                        //this.Alarms.TimeOut = true;
                    }

                    Thread.Sleep(50);
                    RaiseNewDataAvaliable();
                }
            //}
            /*
            catch (TimeoutException ex)
            {
                Work = false;
                ComPort.Close();
            }*/
        }

        public void writeByteCom(byte[] bytes)
        {
            ComPort.Write(bytes, 0, bytes.Length);

        }

        public void writeByteDataCom(byte[] s)
        {
            ComPort.Write(s, 0, s.Count());
        }

        public string writeGetDataCom(string s, bool GetData)
        {
            lock (thisLock)
            {
                ComPort.WriteLine(s);
                if (GetData)
                {
                    string tmp = ComPort.ReadLine();

                    return tmp;
                }
                else
                {
                    Thread.Sleep(100);
                    return null;
                }
            }
        }
    }
}
