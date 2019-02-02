using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Globalization;
using System.Diagnostics;
using System.IO.Ports;
using System.Windows.Forms;
using System.Text.RegularExpressions;
namespace Module_Burn_inTool
{
    public class RackLaser
    {
        public event EventHandler NewDataAvaliable;
        private Object thisLock = new Object();
        public bool QCWmode;
        private Thread WorkThread;
        private Thread StreamReaderThread;
        public string ComPortName;
        private SerialPort myStream;
        public bool comPortErr = false;
        public bool resetError { get; set; }

        public int Iset;


        private byte[] Data; 
        private byte[] Data1;

        private bool Work;
        private bool StreamWork;
        public bool EmissionOn { get; set; }

        private int SleepTime = 100;

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
            public int ElapsedTime { get; set; }


        }

        public struct modAlarms
        {
            public bool PD1 { get; set; }
            public bool PD2 { get; set; }
            public bool PD3 { get; set; }
            public bool PD4 { get; set; }
            public bool PD5 { get; set; }
            public bool Overheat { get; set; }
            public bool GndLeak { get; set; }
            public bool CurrentLeak { get; set; }
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
        private static string[] requestMap =
        {
            "$9;113\r","$9;114\r","$9;115\r","$9;116\r","$9;117\r","$9;118\r", //Current in chains 1-6  index 0-5
            "$9;27\r","$9;32\r","$9;37\r","$9;47\r",   //PD1-4   index6-9
            "$9;13\r","$9;19\r","$9;110\r",// PS voltage, Vout_Mon,  S_ILD1  index 10-12
            "ROP\r","STA\r","RMEC\r","RCT\r","RET\r", "RNC\r" ,"$9;75\r", //OutputPower, Status 32byte, Error code or 0, Module temperature, Elapsed Time , current minimum, PD3 QCW mode   index 13-19
            "$9;20\r","$9;25\r","$9;30\r","$9;35\r" // DP1,  DP2, DP3, DP4  20-24
        };

        private string requestCurrent= "$9;113\r$9;114\r$9;115\r$9;116\r$9;117\r$9;118\r";
        private string requestPDn="$9;27\r$9;32\r$9;37\r$9;47\r";
        private string requestDPn = "$9;20\r$9;25\r$9;30\r$9;35\r";
        private string requestVoltage = "$9;13\r$9;19\r$9;110\r";
        private string requestStatus = "ROP\rSTA\rRMEC\rRCT\rRET\rRNC\r$9;75\r";

        string[] unswerMap = new string[requestMap.Length];
        private int requestMapCount = requestMap.Length; 
        protected virtual void RaiseNewDataAvaliable()
        {
            // Raise the event by using the () operator.
            if (NewDataAvaliable != null)
                NewDataAvaliable(this, new EventArgs());
        }
        public RackLaser()
        {
            myStream = new SerialPort();
            myStream.BaudRate = 57600;
            myStream.DataBits = 8;
            myStream.Encoding = System.Text.Encoding.GetEncoding(1252);
            myStream.Handshake = Handshake.None;
            myStream.NewLine = "\r";
            myStream.Parity = Parity.None;
            myStream.ReadTimeout = 600;
            myStream.StopBits = StopBits.One;
            myStream.WriteTimeout = 600;
            myStream.ReceivedBytesThreshold = 3;
            Iset = 0;
            Work = false;
            this.Settings.currentChangeStep = 10;
            this.Settings.currentSetMax = 100;
            this.Settings.currentSetMinimum = 10;
            this.EmissionOn = false;
        }
        ~RackLaser()
        {
        }

        public void StartCommunication()
        {
            try
            {
                myStream.PortName = this.ComPortName;
                myStream.Open();
                
                //myStream.RtsEnable = false;
                //ReadLine module ID
                this.DataMod.ModuleID = this.SN;
                this.DataMod.ModuleFirmware = this.Firmware;
                
                string pattern2 = @"[0-9 \-:A-Za-z]{1,1}";
                MatchCollection matches = Regex.Matches(this.DataMod.ModuleID,pattern2);
                
                //MessageBox.Show(this.DataMod.ModuleID.ToString());
                if ((matches.Count) != this.DataMod.ModuleID.Length)
                    this.DataMod.Error = true;
                else
                    this.DataMod.Error = false;
                this.Status.PowerSupply = false;
                for (int i = 0; i < requestMap.Length; i++)
                {
                    unswerMap[i] = "";
                }

                 StreamReaderThread = new Thread(streamReader);
                StreamReaderThread.IsBackground = true;
                StreamWork = true;
                StreamReaderThread.Start();
                Work = true;
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                this.comPortErr = true;
            }
        }
        public void StopCommunication()
        {
            try
            {
                Work = false;
                StreamWork = false;
                if (StreamReaderThread != null)
                {
                    StreamReaderThread.Join();
                }
                if (WorkThread != null)
                {
                    WorkThread.Join();
                }
                byte[] Data;
                string request = "EMOFF\r";
                Data = System.Text.Encoding.ASCII.GetBytes(request);
                writeGetDataCom(Data, false);
                Thread.Sleep(SleepTime);
                myStream.Close();
            }
            catch(Exception ex)
            {
            }

        }

        public void streamReader()
        {
            while (StreamWork)
            {
                string request;

                string Response = "";
                Data = System.Text.Encoding.ASCII.GetBytes(requestCurrent);
                Response += writeGetDataCom(Data, true);

                Data = System.Text.Encoding.ASCII.GetBytes(requestPDn);
                Response += writeGetDataCom(Data, true);

                Data = System.Text.Encoding.ASCII.GetBytes(requestDPn);
                Response += writeGetDataCom(Data, true);

                Data = System.Text.Encoding.ASCII.GetBytes(requestVoltage);
                Response += writeGetDataCom(Data, true);

                Data = System.Text.Encoding.ASCII.GetBytes(requestStatus);
                Response += writeGetDataCom(Data, true);

                    if (StreamWork)
                    {
                        {
                            request = "SDC " + this.Iset + "\r";// "RERR\r";
                            if(resetError)
                                request += "RERR\r";
                            if (this.EmissionOn && this.Iset >= this.Settings.currentSetMinimum)
                            {
                                request += "EMON\r";
                            }
                            else
                            {
                                request += "EMOFF\r";
                            }
                            if (this.QCWmode)
                            {
                                request += "EPM\r";
                                //request += "EEC\r";
                            }
                            else
                            {
                                request += "DPM\r";
                                //request += "DEC\r";
                            }
                            Data = System.Text.Encoding.ASCII.GetBytes(request);
                            writeGetDataCom(Data, false);
                            resetError = false;
                        }
                        Response = Response.Replace("Off", "0");
                        Response = Response.Replace("Low", "0");
                        Dictionary<string, string> matches = new Dictionary<string, string>();

                        try
                        {
                            foreach (string item in requestMap)
                            {
                                string searchIndex = "";
                                if (item.Contains("$"))
                                    searchIndex = item.Remove(0, 1).Replace("\r", "");
                                else
                                    searchIndex = item.Replace("\r", "");
                                if (Response.Contains(searchIndex))
                                {
                                    Regex rgx2 = new Regex(searchIndex);
                                    string result = rgx2.Split(Response)[1];
                                    result = result.Remove(result.IndexOf("\r")).Replace(";", "");
                                    string pattern2 = @"[ 0-9.]";
                                    MatchCollection matchesResult = Regex.Matches(result, pattern2);
                                    if (matchesResult.Count > 0)
                                        matches.Add(item, result);
                                }
                            }
                        }
                        catch (Exception ex)
                        { }

                        //string[] unswerMap = new string[matches.Count];//Response.Split(new char[] { '\r' });
                        string pattern1 = @"9;\d+;";
                        Regex rgx1 = new Regex(pattern1);
                        for (int i = 0; i < unswerMap.Length; i++)
                        {
                            try
                            {
                                string[] tmp = matches[requestMap[i]].Split(':');
                                if (tmp.Length > 1)
                                { matches[requestMap[i]] = tmp[1]; }
                                matches[requestMap[i]] = matches[requestMap[i]].Replace("\r", "");
                                matches[requestMap[i]] = matches[requestMap[i]].Replace(" ", "");
                                matches[requestMap[i]] = matches[requestMap[i]].Replace(":", "");
                                if (matches[requestMap[i]] != "")
                                    unswerMap[i] = matches[requestMap[i]];
                            }
                            catch (Exception ex)
                            {
                                if (unswerMap[i] == "")
                                unswerMap[i] = "0"; 
                            }
                        }
                       
                        /*
                        if (Response.Contains("BCMD"))
                            unswerMap[i] = "0";
                        else
                        {
                            if (item.Contains("$"))
                                Response = Response.Remove(0, item.Length - 1).Trim();
                            else
                                Response = Response.Remove(0,item.Length).Trim();
                            Response = Response.Replace("Off", "0");
                            Response = Response.Replace("Low", "0");
                            unswerMap[i] = Response;
                            
                        }*/
                    }
                    else
                        break;
                if (WorkThread == null)
                {
                    WorkThread = new Thread(RealWork);
                    WorkThread.IsBackground = true;
                    WorkThread.Start();
                }
            }
 
        }


        private void RealWork(object obj)
        {
            while (Work)
            {
                //wait
                try
                {
                    if (this.Iset == 0)
                        this.Iset = this.Settings.currentSetMinimum;
                    //set ild, update data
                    //form the command
                    /*string request="";
                    foreach (string item in requestMap)
                    {
                        request += item;
                    }
                    if (this.Iset == 0)
                        this.Iset = this.Settings.currentSetMinimum;
                    request += "SDC " + this.Iset + "\rRERR\r";
                    if (this.EmissionOn && this.Iset>=this.Settings.currentSetMinimum)
                    {
                        request += "EMON\r";
                    }
                    else
                    {
                        request += "EMOFF\r";
                    }
                    if (this.QCWmode)
                    {
                        request += "EPM\r";
                        //request += "EEC\r";
                    }
                    else
                    {
                        request += "DPM\r";
                        //request += "DEC\r";
                    } 

                    Data = System.Text.Encoding.ASCII.GetBytes(request);
                    Data1 = new byte[256];
                    
                    string Response = string.Empty;
                    Response = writeGetDataCom(Data, true);

                    int deletPosition = Response.IndexOf("SDC");
                    if(deletPosition>0)
                    Response = Response.Remove(deletPosition);
                    Response = Response.Replace("Off", "0");
                    Response = Response.Replace("Low", "0");
                    Dictionary<string, string> matches = new Dictionary<string, string>();
                    try
                    {
                        foreach (string item in requestMap)
                        {
                            string searchIndex = "";
                            if (item.Contains("$"))
                                searchIndex = item.Remove(0, 1).Replace("\r", "");
                            else
                                searchIndex = item.Replace("\r", "");
                            if (Response.Contains(searchIndex))
                            {
                                Regex rgx2 = new Regex(searchIndex);
                                string result = rgx2.Split(Response)[1];
                                result = result.Remove(result.IndexOf("\r")).Replace(";", "");
                                string pattern2 = @"[ 0-9.]";
                                MatchCollection matchesResult = Regex.Matches(result, pattern2);
                                if (matchesResult.Count > 0)
                                    matches.Add(item, result);
                            }
                        }
                    }
                    catch (Exception ex)
                    { }*/
                    
                    if ( true)//requestMapCount == matches.Count)
                    {
                        /*string[] unswerMap = new string[matches.Count];//Response.Split(new char[] { '\r' });
                        string pattern1 = @"9;\d+;";
                        Regex rgx1 = new Regex(pattern1);
                        for (int i = 0; i < matches.Count; i++)
                        {
                            string[] tmp = matches[requestMap[i]].Split(':');
                            if (tmp.Length > 1)
                            { matches[requestMap[i]] = tmp[1]; }
                            matches[requestMap[i]] = matches[requestMap[i]].Replace("\r", "");
                            matches[requestMap[i]] = matches[requestMap[i]].Replace(" ", "");
                            matches[requestMap[i]] = matches[requestMap[i]].Replace(":", "");
                            if (matches[requestMap[i]] != "")
                                unswerMap[i] = matches[requestMap[i]];
                            else
                                unswerMap[i] = "0";
                           
                        }*/
                        /*if (unswerMap[18].Contains("."))
                        {
                            unswerMap[18] = unswerMap[18].Remove(unswerMap[18].IndexOf("."), 2);
                            //unswerMap[18] = unswerMap[18].Replace("\r", "");
                            //MessageBox.Show("!"+unswerMap[18]+"!");
                        }*/
                        //this.Settings.currentSetMinimum = Convert.ToInt32(Convert.ToDouble(unswerMap[18]));
                        //if(this.Iset<currentMinimum)

                        DataMod.Power = Convert.ToDouble(unswerMap[13], CultureInfo.InvariantCulture);
                        DataMod.Temp = Convert.ToDouble(unswerMap[16], CultureInfo.InvariantCulture);
                        
                        if (this.QCWmode)
                        {
                            if (Convert.ToInt32(unswerMap[6], CultureInfo.InvariantCulture) > 1)
                                DataMod.PD1 = Convert.ToInt32(unswerMap[6], CultureInfo.InvariantCulture);
                            if (Convert.ToInt32(unswerMap[7], CultureInfo.InvariantCulture) > 1)
                                DataMod.PD2 = Convert.ToInt32(unswerMap[7], CultureInfo.InvariantCulture);
                            DataMod.PD3 = Convert.ToInt32(unswerMap[19], CultureInfo.InvariantCulture);
                            if (Convert.ToInt32(unswerMap[9], CultureInfo.InvariantCulture) > 1)
                                DataMod.PD4 = Convert.ToInt32(unswerMap[9], CultureInfo.InvariantCulture);


                            if(Convert.ToDouble(unswerMap[0]) / 1000 > 1)
                                DataMod.ILD1 = Convert.ToDouble(unswerMap[0]) / 1000;
                            if(Convert.ToDouble(unswerMap[1]) / 1000 > 1)
                                DataMod.ILD2 = Convert.ToDouble(unswerMap[1]) / 1000;
                            if(Convert.ToDouble(unswerMap[2]) / 1000 > 1)
                                DataMod.ILD3 = Convert.ToDouble(unswerMap[2]) / 1000;
                            if(Convert.ToDouble(unswerMap[3]) / 1000 > 1 )
                                DataMod.ILD4 = Convert.ToDouble(unswerMap[3]) / 1000;
                            if(Convert.ToDouble(unswerMap[4]) / 1000 > 1)
                                DataMod.ILD5 = Convert.ToDouble(unswerMap[4]) / 1000;
                            if(Convert.ToDouble(unswerMap[5]) / 1000 > 1)
                                DataMod.ILD6 = Convert.ToDouble(unswerMap[5]) / 1000;
                        }
                        else
                        {
                            DataMod.ILD1 = Convert.ToDouble(unswerMap[0]) / 1000;
                            DataMod.ILD2 = Convert.ToDouble(unswerMap[1]) / 1000;
                            DataMod.ILD3 = Convert.ToDouble(unswerMap[2]) / 1000;
                            DataMod.ILD4 = Convert.ToDouble(unswerMap[3]) / 1000;
                            DataMod.ILD5 = Convert.ToDouble(unswerMap[4]) / 1000;
                            DataMod.ILD6 = Convert.ToDouble(unswerMap[5]) / 1000;
                            DataMod.PD1 = Convert.ToInt32(unswerMap[6], CultureInfo.InvariantCulture);
                            DataMod.PD2 = Convert.ToInt32(unswerMap[7], CultureInfo.InvariantCulture);
                            DataMod.PD3 = Convert.ToInt32(unswerMap[8], CultureInfo.InvariantCulture);
                            DataMod.PD4 = Convert.ToInt32(unswerMap[9], CultureInfo.InvariantCulture);
                        }

                        DataMod.DP1 = Convert.ToInt32(unswerMap[20], CultureInfo.InvariantCulture);
                        DataMod.DP2 = Convert.ToInt32(unswerMap[21], CultureInfo.InvariantCulture);
                        DataMod.DP3 = Convert.ToInt32(unswerMap[22], CultureInfo.InvariantCulture);
                        DataMod.DP4 = Convert.ToInt32(unswerMap[23], CultureInfo.InvariantCulture);
                        
                        //Debug.Write("\r!"+unswerMap[11]+"! \r");
                        DataMod.Uset = Convert.ToDouble(unswerMap[11], CultureInfo.InvariantCulture);
                        DataMod.PSVoltage = Convert.ToDouble(unswerMap[10], CultureInfo.InvariantCulture);

                        int Status = Convert.ToInt32(unswerMap[14]);
                        
                        this.Status.Emission = GetBit(Status, StatusBits.Emission_On);
                        //this.Status.PowerSupply = GetBit(Status, StatusBits.PowerSupply);
                        //MessageBox.Show( "!" + (Status << StatusBits.PowerSupply));
                        if (GetBit(Status, StatusBits.PowerSupply))
                        {
                            this.Status.PowerSupply = false;
                            this.Status.Error = true;
                        }
                        else
                        {
                            this.Status.PowerSupply = true;
                            this.Status.Error = false;
                        }
                        this.Status.PulsedMode = GetBit(Status, StatusBits.PulsedMode);
                        this.Status.ElapsedTime = Convert.ToInt32(unswerMap[17]);
                        //MessageBox.Show(unswerMap[15]);


                        //if (Convert.ToInt64(unswerMap[15])>0)
                          //  this.Status.Error = true;

                        
                        //Debug.WriteLine("Status=" + Status.ToString());
                        //this.Status.ExternalStart = GetBit(Status, StatusBits);
                        //this.Status.ExternalShutdown = GetBit(Status, StatusBits);
                        //this.Status.Enabled = GetBit(Status, StatusBits);




                        this.Alarms.Overheat = GetBit(Status, StatusBits.Overheat);
                        this.Alarms.DutyCycle = GetBit(Status, StatusBits.DutyCycle);
                        this.Alarms.PowerSupply = GetBit(Status, StatusBits.PowerSupply);
                        this.Alarms.HighBackReflection = GetBit(Status, StatusBits.HighBackReflection);
                        this.Alarms.GndLeak = GetBit(Status, StatusBits.GndLeak);

                       // if (this.Alarms.Overheat || this.Alarms.HighBackReflection || this.Alarms.GndLeak || this.Alarms.DutyCycle)
                         //   this.Status.Error = true;
                        //Need Error codes here!
                        /*
                        //17th is Alarms
                        Status = Convert.ToInt32(tmp[16]);
                        this.Alarms.PD1 = GetBit(Status, 0);
                        this.Alarms.PD2 = GetBit(Status, 1);
                        this.Alarms.PD3 = GetBit(Status, 2);
                        this.Alarms.PD4 = GetBit(Status, 3);
                        this.Alarms.PD5 = GetBit(Status, 4);

                    
                    
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
                        */
                    }
                }
                catch (TimeoutException ex)
                {
                    Alarms.TimeOut = true;
                    DataMod.Power = 0;
                    DataMod.Temp = 0;
                    DataMod.ILD1 = 0;
                    DataMod.ILD2 = 0;
                    DataMod.ILD3 = 0;
                    DataMod.ILD4 = 0;
                    DataMod.ILD5 = 0;
                    DataMod.ILD6 = 0;
                    DataMod.PD1 = 0;
                    DataMod.PD2 = 0;
                    DataMod.PD3 = 0;
                    DataMod.PD4 = 0;
                    DataMod.PD5 = 0;
                    DataMod.Uset = 0;
                    //Debug.WriteLine("Status=" + Status.ToString());
                    this.Status.Emission = false;
                    this.Status.Enabled = false;
                    this.Status.PowerSupply = false;
                    this.Status.ExternalStart = false;
                    this.Status.ExternalShutdown = false;
                    this.Status.PulsedMode = false;
                    this.Status.Error = false;


                    this.Alarms.Overheat = false;
                    this.Alarms.PowerSupply = false;
                    this.Alarms.HighBackReflection = false;
                    //Need Error codes here!
                    /*

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
                     * */
                }
                //Thread.Sleep(50);
                //RaiseNewDataAvaliable();
            }
        }

        private string[] getData()
        {
            string[] tmp = { "" };
            return tmp;
        }

        public enum PowerControl
        {
            Software,
            DigitalModulation,
            AnalogControl
        }

        public enum OperationMode
        {
            Pulsed,
            CW
        }

        private static class StatusBits
        {
            public const int PulsedMode = 10;
            public const int RemoteState = 21;
            public const int PS_On = 11;
            public const int Emission_On = 2;
            //public const int Enabled = ;
            public const int PowerSupply = 11;
            //public const int ExternalStart = ;
            //public const int ExternalShutdown = ;
            public const int Error = 29;
            public const int Overheat =1;
            public const int GndLeak = 26; 
            public const int HighBackReflection=3;
            public const int DutyCycle = 23;
        }
        private static bool GetBit(int b, int bitNumber)
        {
            return (b & (1 << bitNumber)) != 0;
        }

        public bool Connected
        {
            get;
            private set;
        }

        

       /* public bool QcwOn
        {
            get 
            {
                Data = System.Text.Encoding.ASCII.GetBytes("EPM\r");
                myStream.Write(Data, 0, Data.Length);
                Data = new byte[256];
                string Response = string.Empty;
                Thread.Sleep(SleepTime);
                Int32 bytes = myStream.Read(Data, 0, Data.Length);
                Response = System.Text.Encoding.ASCII.GetString(Data, 0, bytes);
                if (Response == "EPM") return true; else return false;
            }
        }
        /*
        public double ILD
        {
            get
            {
                Data = System.Text.Encoding.ASCII.GetBytes("$9;113\r");
                myStream.Write(Data, 0, Data.Length);
                Data = new byte[1024];
                string Response = string.Empty;
                Thread.Sleep(SleepTime);
                Int32 bytes = myStream.Read(Data, 0, Data.Length);
                Response = System.Text.Encoding.ASCII.GetString(Data, 0, bytes);
                Response = Response.Remove(0, 6);
                double tmp = Double.Parse(Response, CultureInfo.InvariantCulture);
                Debug.WriteLine("ILD1=" + tmp.ToString());
                return tmp / (double)1000;
            }
        }
        //get 1-6 current chains 
        public string[] IldALL
        {
            get
            {
                Data = new byte[256];
                string request = "$9;113\r$9;114\r$9;115\r$9;116\r$9;117\r$9;118\r";
                Data = System.Text.Encoding.ASCII.GetBytes(request);
                myStream.Write(Data, 0, Data.Length);
                Data = new byte[256];
                string Response = string.Empty;
                Thread.Sleep(SleepTime);
                Int32 bytes = myStream.Read(Data, 0, Data.Length);
                Response = System.Text.Encoding.ASCII.GetString(Data, 0, bytes);
                string pattern = @"9;\d+;\d+\r";
                string[] ILD = Regex.Split(Response, pattern);
                for(int i=0; i<ILD.Length; i++)
                {
                    ILD[i] = ILD[i].Remove(0, 6);
                }
                return ILD;
            }
        }
        public string[] PdALL
        {
            get
            {
                string Request = "$9;27\r$9;32\r$9;37\r$9;47\r";
                    Data = System.Text.Encoding.ASCII.GetBytes(Request);
                    myStream.Write(Data, 0, Data.Length);
                    Data = new byte[256];
                    string Response = string.Empty;
                    Thread.Sleep(SleepTime);
                    Int32 bytes = myStream.Read(Data, 0, Data.Length);
                    Response = System.Text.Encoding.ASCII.GetString(Data, 0, bytes);
                    string pattern = @"9;(\d+);(\d+)";
                string [] PD=Regex.Split(Response, pattern);
                return PD;
            }
        }
        */
        public void Disconnect()
        {

            Connected = false;
        }
        
        public string SN
        {
            get
            {
                
                Data = System.Text.Encoding.ASCII.GetBytes("RSN\r");
                Data1 = new byte[256];
                string Response = string.Empty;
                Response = writeGetDataCom(Data, true);
                string Sn = Response.Remove(0, Response.IndexOf("RSN:") + 5);
                Sn = Sn.Remove(Sn.IndexOf("\r")); 
                return Sn;
            }

        }

        public void SetSN(string SN)
        {
                Data = System.Text.Encoding.ASCII.GetBytes("$8;912;"+SN+"\r");
                Data1 = new byte[256];
                string Response = string.Empty;
                Response = writeGetDataCom(Data, false);
                this.DataMod.ModuleID = this.SN;
        }


        public string Firmware
        {
            get
            {
                Data = System.Text.Encoding.ASCII.GetBytes("RFV\r");
                Data1 = new byte[256];
                string Response = string.Empty;
                Response = writeGetDataCom(Data, true);
                Response = Response.Replace("\r", "");
                Response = Response.Remove(0, 5);
                myStream.DiscardInBuffer();
                myStream.DiscardOutBuffer();
                //send comand for work (strange code, by send it programm can connect to laser)
                /*Data = System.Text.Encoding.ASCII.GetBytes("$8;912:F05-100\r");
                myStream.Write(Data, 0, Data.Length);*/
                return Response;
            }
        }
        
        public bool IsInRemoteState()
        {

            return ReadStateBit(StatusBits.RemoteState);
        }


        private bool ReadStateBit(int bitNumber)
        {
            Data = System.Text.Encoding.ASCII.GetBytes("STA\r");
            Data1 = new byte[256];
            string Response = string.Empty;
            Response = writeGetDataCom(Data, true);

            Response = Response.Remove(0, 5);
            Int32 State = Int32.Parse(Response);
            //MessageBox.Show(State.ToString("X8"));
            //MessageBox.Show(((Int32)(State & (1 << 21))).ToString());
            if ((State & (1 << bitNumber)) > 0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
       
        public bool IsPSOn()
        {
            return !this.ReadStateBit(StatusBits.PS_On);
        }

        public void setDPn(string DPn, int value)
        {
            switch (DPn)
            {
                case "DP1":
                    ThrowCommand("$8;20;" + value);
                    break;
                case "DP2":
                    ThrowCommand("$8;25;" + value);
                    break;
                case "DP3":
                    ThrowCommand("$8;30;" + value);
                    break;
                case "DP4":
                    ThrowCommand("$8;35;" + value);
                    break;
            }
        }

        public bool PD3Calibr( int key, string value)
        {
            switch (key)
            {
                case 1:
                    return ThrowCommand("PD3Calibr");
                case 2:
                    return ThrowCommand("PWRCalibr "+value);
            }
            return false;
        }

        private bool ThrowCommand(string cmd)
        {
            Data = System.Text.Encoding.ASCII.GetBytes(cmd + "\r");
            Data1 = new byte[256];
            string Response = string.Empty;
            Response = writeGetDataCom(Data, true);
            if (Response.Contains("ERR"))
                return false;
            else
                return true;
        }
        /*
        public void SetPowerControl(PowerControl cnt)
        {
            switch (cnt)
            {
                case PowerControl.Software:
                    {
                        ThrowCommand("DEC");//disable analog control (external)
                        ThrowCommand("DMOD");//disable modulation
                        break;
                    }
                case PowerControl.AnalogControl:
                    {
                        ThrowCommand("EEC");//enable analog control   (external)
                        ThrowCommand("DMOD");//disable modulation
                        break;
                    }
                case PowerControl.DigitalModulation:
                    {
                        ThrowCommand("DEC");
                        ThrowCommand("EMOD");
                        break;
                    }
            }
        }

        

        public void SetHardwareEmissionControl(bool HarwareEmissionControl)
        {
            if (HarwareEmissionControl)
            {
                ThrowCommand("ELE");
            }
            else
            {
                ThrowCommand("DLE");
            }
        }

        /*
        public OperationMode Mode
        {
            get
            {
                if (this.ReadStateBit(StatusBits.PulsedMode))
                {
                    return OperationMode.Pulsed;
                }
                else
                {
                    return OperationMode.CW;
                }
            }
            set
            {
                if (value == OperationMode.CW)
                {
                    ThrowCommand("DPM");
                }
                else
                {
                    ThrowCommand("EPM");
                }
            }
        }
        /*
        public double PulseWidth
        {
            get
            {
                Data = System.Text.Encoding.ASCII.GetBytes("RPW\r");
                myStream.Write(Data, 0, Data.Length);
                Data = new byte[256];
                string Response = string.Empty;
                Thread.Sleep(SleepTime);
                Int32 bytes = myStream.Read(Data, 0, Data.Length);
                Response = System.Text.Encoding.ASCII.GetString(Data, 0, bytes);
                Response = Response.Remove(0, 6);
                return double.Parse(Response, CultureInfo.InvariantCulture);
            }
            set
            {
                string temp = value.ToString("F01", CultureInfo.InvariantCulture);
                ThrowCommand("SPW " + temp);
            }
        }

        public double PulseRepetitionRate
        {
            get
            {
                Data = System.Text.Encoding.ASCII.GetBytes("RPRR\r");
                myStream.Write(Data, 0, Data.Length);
                Data = new byte[256];
                string Response = string.Empty;
                Thread.Sleep(SleepTime);
                Int32 bytes = myStream.Read(Data, 0, Data.Length);
                Response = System.Text.Encoding.ASCII.GetString(Data, 0, bytes);
                Response = Response.Remove(0, 6);
                return double.Parse(Response, CultureInfo.InvariantCulture);
            }
            set
            {
                string temp = value.ToString("F01", CultureInfo.InvariantCulture);
                ThrowCommand("RPRR " + temp);
            }
        }

        public double PeakPower
        {
            get
            {
                Data = System.Text.Encoding.ASCII.GetBytes("RPP\r");
                myStream.Write(Data, 0, Data.Length);
                Data = new byte[256];
                string Response = string.Empty;
                Thread.Sleep(SleepTime);
                Int32 bytes = myStream.Read(Data, 0, Data.Length);
                Response = System.Text.Encoding.ASCII.GetString(Data, 0, bytes);
                Response = Response.Remove(0, 5);
                Response = Response.Replace("\r", "");
                if (Response == "Low")
                {
                    Response = "0";
                }
                return double.Parse(Response, CultureInfo.InvariantCulture);
            }
        }
        */
        public string writeGetDataCom(byte[] Data, bool GetData)
        {
            lock (thisLock)
            {
                myStream.Write(Data, 0, Data.Length);
                if (GetData)
                {
                    Thread.Sleep(30);
                    Data = new byte[512];
                    string Response = string.Empty;
                    Int32 bytes = myStream.Read(Data, 0, Data.Length);
                    
                    Response = System.Text.Encoding.ASCII.GetString(Data, 0, bytes);
                    return Response;
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
