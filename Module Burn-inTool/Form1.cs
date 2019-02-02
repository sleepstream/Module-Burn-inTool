using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;
using System.Threading;
using System.IO;
using System.Globalization;
using System.Diagnostics;
using System.Windows.Controls;
using System.Text.RegularExpressions;

namespace Module_Burn_inTool
{
    public partial class Form1 : Form
    {
        modData DataMod;
        modStatus StatusMod;
        modAlarms AlarmsMod;
        modSettings SettingsMod;
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
            public bool PD1;
            public bool PD2;
            public bool PD3;
            public bool PD4 ;
            public bool PD5;
            public bool Overheat ;
            public bool CurrentLeak;
            public bool GndLeak ;
            public bool PowerSupply ;
            public bool HighBackReflection ;
            public bool CS1 ;
            public bool CS2 ;
            public bool CS3 ;
            public bool CS4 ;
            public bool CS5 ;
            public bool CS6;
            public bool UnexpectedPump1 ;
            public bool UnexpectedPump2 ;
            public bool UnexpectedPump3 ;
            public bool UnexpectedPump4 ;
            public bool UnexpectedPump5 ;
            public bool UnexpectedPump6 ;
            public bool DutyCycle ;
            public  modAlarms(bool init)
            {
                this.PD1 = init;
                this.PD2 = init;
                this.PD3 = init;
                this.PD4 = init;
                this.PD5 = init;
                this.CS1 = init;
                this.CS2 = init;
                this.CS3 = init;
                this.CS4 = init;
                this.CS5 = init;
                this.CS5 = init;
                this.CS6 = init;
                this.UnexpectedPump1 = init;
                this.UnexpectedPump2 = init;
                this.UnexpectedPump3 = init;
                this.UnexpectedPump4 = init;
                this.UnexpectedPump5 = init;
                this.UnexpectedPump6 = init;
                this.DutyCycle = init;
                this.Overheat = init;
                this.CurrentLeak = init;
                this.GndLeak = init;
                this.PowerSupply = init;
                this.HighBackReflection = init;
            }
        }

        bool Connected=false;
        bool PowerMetersError = false;
        double PrimesOffset = 0;
        string PwrMeas;
        string CycleTime;
        string FullTime;

        //string PD3Message;
       
        int countAveraging=0;
        int averagingPD3Value;


        //monitoring PD3
        int startPD3Monitoring = 0;
        int identValues = 0;
        int countMonitoringAveraging;
        int monitoringAveragingPD3Value;
        int counter = 1;
        bool monitoring = true;
        int lastPD3;
        DateTime StartMonitoringPD3;
        DateTime fullTimeMonitoring;
        int monitoringPD3Interval;
        int minMonitoringPD3;

        BackgroundWorker powerMetr;
        BackgroundWorker powerMetrConnect;
        BackgroundWorker powerMetrCalibrate;
       // BackgroundWorker PD3BackgroundMonitoring;

        SimpleFM FieldMaxPM;
       // SimpleFM FieldMaxPM1;
        Primes PM;

        DateTime startAveraging;
        DateTime changeTimetoHoursFull;
        DateTime changeTimetoHoursCycle;
        DateTime BurnInStart;
        DateTime autoStartBrnTimer;
        TimeSpan CycleTimeValue;

        Double BurnInFullTime=0;
        Double BurnInFullTimeSec=0;
        Module mModule;
        RackLaser rackLaser;
        string mainText;
        string logDirectory1 = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\Stability\\";
        string logDirectory;
        string logDirectoryIG243 = "\\\\10.100.80.66\\Modules\\Stability modules logs\\IG243\\";
        string logDirectoryIG337 = "\\\\10.100.80.66\\Modules\\Stability modules logs\\IG337\\";
        string errorModuleMessage = ""; //error line
        string[] txtStatus1L=new string[4];
        string[] txtStatus2L = new string[4];
        string txtStatus2oldText="";
        string txtStatus1oldText = "";
        string logSeparator="___";
        bool WriteStability;
        int IGFLAG;
        //int PrevAlarmPower;
        //int PrevPower;
        //int Power;
        int PLD_DROP_MIN;
        string[] IGTYPE = { "IG243-9", "IG337"};


        public Form1()
        {
            InitializeComponent();
        }
        private void InitializeBackgroundWorker()
        {
            PowerMetersError = false;
            powerMetr = new BackgroundWorker();
            powerMetrConnect = new BackgroundWorker();
            powerMetrCalibrate = new BackgroundWorker();

            /*
            PD3BackgroundMonitoring = new BackgroundWorker();
            PD3BackgroundMonitoring.DoWork += new DoWorkEventHandler(PD3BackgroundMonitoring_DoWork);
            PD3BackgroundMonitoring.RunWorkerCompleted += new RunWorkerCompletedEventHandler(PD3BackgroundMonitoring_Completed);
            PD3BackgroundMonitoring.ProgressChanged += new ProgressChangedEventHandler(PD3BackgroundMonitoring_Changed);
            PD3BackgroundMonitoring.WorkerSupportsCancellation = true;
            PD3BackgroundMonitoring.WorkerReportsProgress = true;
            */
 
            powerMetr.DoWork += new DoWorkEventHandler(powerMetr_DoWork);
            powerMetr.RunWorkerCompleted += new RunWorkerCompletedEventHandler(powerMetr_Completed);
            powerMetr.ProgressChanged += new ProgressChangedEventHandler(powerMetr_Changed);
            powerMetr.WorkerSupportsCancellation = true;
            powerMetr.WorkerReportsProgress = true;

            powerMetrConnect.DoWork += new DoWorkEventHandler(powerMetrConnect_DoWork);
            powerMetrConnect.RunWorkerCompleted += new RunWorkerCompletedEventHandler(powerMetrConnect_Completed);
            powerMetrConnect.ProgressChanged += new ProgressChangedEventHandler(powerMetrConnect_Changed);
            powerMetrConnect.WorkerSupportsCancellation = true;
            powerMetrConnect.WorkerReportsProgress = true;

            powerMetrCalibrate.DoWork += new DoWorkEventHandler(powerMetrCalibrate_DoWork);
            powerMetrCalibrate.RunWorkerCompleted += new RunWorkerCompletedEventHandler(powerMetrCalibrate_Completed);
            powerMetrCalibrate.ProgressChanged += new ProgressChangedEventHandler(powerMetrCalibrate_Changed);
            powerMetrCalibrate.WorkerSupportsCancellation = true;
            powerMetrCalibrate.WorkerReportsProgress = true;
        }

        private void dataModuleReciver()
        {
            switch (IGFLAG)
            {
                case 1:
                    {
                        DataMod.ILD1 = mModule.DataMod.ILD1;
                        DataMod.ILD2 = mModule.DataMod.ILD2;
                        DataMod.ILD3 = mModule.DataMod.ILD3;
                        DataMod.ILD4 = mModule.DataMod.ILD4;
                        DataMod.ILD5 = mModule.DataMod.ILD5;
                        DataMod.ILD6 = mModule.DataMod.ILD6;

                        DataMod.PD1 = mModule.DataMod.PD1;
                        DataMod.PD2 = mModule.DataMod.PD2;
                        DataMod.PD3 = mModule.DataMod.PD3;
                        DataMod.PD4 = mModule.DataMod.PD4;
                        DataMod.PD5 = mModule.DataMod.PD5;

                        DataMod.Power = mModule.DataMod.Power;
                        DataMod.PSVoltage = mModule.DataMod.PSVoltage;
                        DataMod.Temp = mModule.DataMod.Temp;
                        DataMod.Uset = mModule.DataMod.Uset;
                        DataMod.ModuleID = mModule.DataMod.ModuleID;
                        DataMod.ModuleFirmware = mModule.DataMod.ModuleFirmware;
                        DataMod.Error = mModule.DataMod.Error;


                        StatusMod.Emission = mModule.Status.Emission;
                        StatusMod.Enabled = mModule.Status.Enabled;
                        StatusMod.Error = mModule.Status.Error;
                        StatusMod.ExternalShutdown = mModule.Status.ExternalShutdown;
                        StatusMod.ExternalStart = mModule.Status.ExternalStart;
                        StatusMod.PowerSupply = mModule.Status.PowerSupply;
                        StatusMod.PulsedMode = mModule.Status.PulsedMode;
                        StatusMod.ElapsedTime = 0;


                        AlarmsMod.CS1 = mModule.Alarms.CS1;
                        AlarmsMod.CS2 = mModule.Alarms.CS2;
                        AlarmsMod.CS3 = mModule.Alarms.CS3;
                        AlarmsMod.CS4 = mModule.Alarms.CS4;
                        AlarmsMod.CS5 = mModule.Alarms.CS5;
                        AlarmsMod.CS6 = mModule.Alarms.CS6;
                        AlarmsMod.CurrentLeak = mModule.Alarms.CurrentLeak;
                        AlarmsMod.DutyCycle = mModule.Alarms.DutyCycle;
                        AlarmsMod.Overheat = mModule.Alarms.Overheat;
                        AlarmsMod.PD1 = mModule.Alarms.PD1;
                        AlarmsMod.PD2 = mModule.Alarms.PD2;
                        AlarmsMod.PD3 = mModule.Alarms.PD3;
                        AlarmsMod.PD4 = mModule.Alarms.PD4;
                        AlarmsMod.PD5 = mModule.Alarms.PD5;
                        AlarmsMod.PowerSupply = mModule.Alarms.PowerSupply;
                        AlarmsMod.UnexpectedPump1 = mModule.Alarms.UnexpectedPump1;
                        AlarmsMod.UnexpectedPump2 = mModule.Alarms.UnexpectedPump2;
                        AlarmsMod.UnexpectedPump3 = mModule.Alarms.UnexpectedPump3;
                        AlarmsMod.UnexpectedPump4 = mModule.Alarms.UnexpectedPump4;
                        AlarmsMod.UnexpectedPump5 = mModule.Alarms.UnexpectedPump5;
                        AlarmsMod.UnexpectedPump6 = mModule.Alarms.UnexpectedPump6;
                        AlarmsMod.GndLeak = mModule.Alarms.GndLeak;


                    }
                    break;
                case 0:
                    {
                        DataMod.ILD1 = rackLaser.DataMod.ILD1;
                        DataMod.ILD2 = rackLaser.DataMod.ILD2;
                        DataMod.ILD3 = rackLaser.DataMod.ILD3;
                        DataMod.ILD4 = rackLaser.DataMod.ILD4;
                        DataMod.ILD5 = rackLaser.DataMod.ILD5;
                        DataMod.ILD6 = rackLaser.DataMod.ILD6;

                        DataMod.PD1 = rackLaser.DataMod.PD1;
                        DataMod.PD2 = rackLaser.DataMod.PD2;
                        DataMod.PD3 = rackLaser.DataMod.PD3;
                        DataMod.PD4 = rackLaser.DataMod.PD4;
                        DataMod.PD5 = rackLaser.DataMod.PD5;

                        DataMod.Power = rackLaser.DataMod.Power;
                        DataMod.PSVoltage = rackLaser.DataMod.PSVoltage;
                        DataMod.Temp = rackLaser.DataMod.Temp;
                        DataMod.Uset = rackLaser.DataMod.Uset;
                        DataMod.ModuleID = rackLaser.DataMod.ModuleID;
                        DataMod.ModuleFirmware = rackLaser.DataMod.ModuleFirmware;
                        DataMod.Error = rackLaser.DataMod.Error;

                        StatusMod.Emission = rackLaser.Status.Emission;
                        StatusMod.Enabled = rackLaser.Status.Enabled;
                        StatusMod.Error = rackLaser.Status.Error;
                        StatusMod.ExternalShutdown = rackLaser.Status.ExternalShutdown;
                        StatusMod.ExternalStart = rackLaser.Status.ExternalStart;
                        StatusMod.PowerSupply = rackLaser.Status.PowerSupply;
                        StatusMod.PulsedMode = rackLaser.Status.PulsedMode;
                        StatusMod.ElapsedTime = rackLaser.Status.ElapsedTime;


                        AlarmsMod.CS1 = rackLaser.Alarms.CS1;
                        AlarmsMod.CS2 = rackLaser.Alarms.CS2;
                        AlarmsMod.CS3 = rackLaser.Alarms.CS3;
                        AlarmsMod.CS4 = rackLaser.Alarms.CS4;
                        AlarmsMod.CS5 = rackLaser.Alarms.CS5;
                        AlarmsMod.CS6 = rackLaser.Alarms.CS6;
                        AlarmsMod.CurrentLeak = rackLaser.Alarms.CurrentLeak;
                        AlarmsMod.DutyCycle = rackLaser.Alarms.DutyCycle;
                        AlarmsMod.Overheat = rackLaser.Alarms.Overheat;
                        AlarmsMod.PD1 = rackLaser.Alarms.PD1;
                        AlarmsMod.PD2 = rackLaser.Alarms.PD2;
                        AlarmsMod.PD3 = rackLaser.Alarms.PD3;
                        AlarmsMod.PD4 = rackLaser.Alarms.PD4;
                        AlarmsMod.PD5 = rackLaser.Alarms.PD5;
                        AlarmsMod.PowerSupply = rackLaser.Alarms.PowerSupply;
                        AlarmsMod.UnexpectedPump1 = rackLaser.Alarms.UnexpectedPump1;
                        AlarmsMod.UnexpectedPump2 = rackLaser.Alarms.UnexpectedPump2;
                        AlarmsMod.UnexpectedPump3 = rackLaser.Alarms.UnexpectedPump3;
                        AlarmsMod.UnexpectedPump4 = rackLaser.Alarms.UnexpectedPump4;
                        AlarmsMod.UnexpectedPump5 = rackLaser.Alarms.UnexpectedPump5;
                        AlarmsMod.UnexpectedPump6 = rackLaser.Alarms.UnexpectedPump6;
                        AlarmsMod.HighBackReflection = rackLaser.Alarms.HighBackReflection;
                        AlarmsMod.GndLeak = rackLaser.Alarms.GndLeak;
                    }
                    break;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Debug.Write("Load\r");
            changeTimetoHoursFull = changeTimetoHoursCycle = new DateTime(1970, 1, 1);
            this.Width = 504;
            cbMeasuringDevice.Items.Add("Primes PM");
            cbMeasuringDevice.Items.Add("Coherent FieldMax");
            cbMeasuringDevice.Items.Add("NONE");

            //udCheckPeriod.Value = Module_Burn_inTool.Properties.Settings.Default.LogPeriod;
            modAlarms AlarmsMod = new modAlarms(false);
            
            foreach (string s in SerialPort.GetPortNames())
            {
                cbPort.Items.Add(s);
                cmbPMport.Items.Add(s);
                if (s == Module_Burn_inTool.Properties.Settings.Default.ModulePort)
                {
                    cbPort.SelectedItem = s;
                }
                if (s == Module_Burn_inTool.Properties.Settings.Default.PrimesPort && Module_Burn_inTool.Properties.Settings.Default.PrimesPort != Module_Burn_inTool.Properties.Settings.Default.ModulePort)
                {
                    cmbPMport.SelectedItem = s;
                }
            }
            //cbPort.SelectedIndex = 0;
            // cbMeasuringDevice.SelectedIndex = 0;
            foreach (string s in cbMeasuringDevice.Items)
            {
                if (s == Module_Burn_inTool.Properties.Settings.Default.PowerMeter)
                {
                    cbMeasuringDevice.SelectedItem = s;
                }
            }
            //IgType.SelectedIndex = 0;
            foreach (string s in IGTYPE)
            {
                IgType.Items.Add(s);
                if (s == Module_Burn_inTool.Properties.Settings.Default.IgType)
                {
                    IgType.SelectedItem = s;
                }
            }

            WriteStability = false;
            this.Text += " v." + PublishVersion;
            autoStartBrn.Value = Module_Burn_inTool.Properties.Settings.Default.autoStartBrn;
            PD3Monitoring.Value = Module_Burn_inTool.Properties.Settings.Default.PD3Monitoring;
            PD3ChangeDirect.Value = Module_Burn_inTool.Properties.Settings.Default.PD3Stabilization;
            PD3MaxTimeMonitoringCycle.Value = Module_Burn_inTool.Properties.Settings.Default.PD3MaxTimeMonitoringCycle;
            averagingPD3.Value = Module_Burn_inTool.Properties.Settings.Default.averagingPD3;
            txtStatus1L[0] = Convert.ToString(txtStatus1.Location.X);
            txtStatus1L[1] = Convert.ToString(txtStatus1.Location.Y);
            txtStatus1L[2] = Convert.ToString(txtStatus1.Height);
            txtStatus1L[3] = Convert.ToString(txtStatus1.Font.Size);
            txtStatus2L[0] = Convert.ToString(txtStatus2.Location.X);
            txtStatus2L[1] = Convert.ToString(txtStatus2.Location.Y);
            txtStatus2L[2] = Convert.ToString(txtStatus2.Height);
            txtStatus2L[3] = Convert.ToString(txtStatus2.Font.Size);
            mainText = this.Text;
            PLD_DROP_MIN = Convert.ToInt32(udMinDrop.Value);

        }
        private void Form1_FormClosing(Object sender, FormClosingEventArgs e)
        {
            try
            {
                if (Connected)
                {
                    switch (IGFLAG)
                    {
                        case 1:
                            mModule.StopCommunication();
                            break;
                        case 0:
                            rackLaser.StopCommunication();
                            break;
                    }
                }
                Connected = false;
            }
            catch (Exception ex)
            { }
            Module_Burn_inTool.Properties.Settings.Default.PowerMeter = cbMeasuringDevice.Text;
            if (cmbPMport.Text!="")
            Module_Burn_inTool.Properties.Settings.Default.PrimesPort = cmbPMport.Text;
            Module_Burn_inTool.Properties.Settings.Default.ModulePort = cbPort.Text;
            Module_Burn_inTool.Properties.Settings.Default.IgType = IgType.Text;
            Module_Burn_inTool.Properties.Settings.Default.autoStartBrn = autoStartBrn.Value;
            Module_Burn_inTool.Properties.Settings.Default.PD3Monitoring = PD3Monitoring.Value;
            Module_Burn_inTool.Properties.Settings.Default.PD3Stabilization= PD3ChangeDirect.Value;
            Module_Burn_inTool.Properties.Settings.Default.PD3MaxTimeMonitoringCycle=PD3MaxTimeMonitoringCycle.Value;
            Module_Burn_inTool.Properties.Settings.Default.averagingPD3=averagingPD3.Value;
            Module_Burn_inTool.Properties.Settings.Default.CalValue = udCalCoeff.Text;
            Module_Burn_inTool.Properties.Settings.Default.MinPdDropIg243 = udMinDrop.Value;
            Module_Burn_inTool.Properties.Settings.Default.Save();
            //powerMeterKill();
        }

       

        private void powerMetrConnect_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                double PWR;
                
                    if(e.Argument.ToString()== "Primes PM")
                        {
                            Debug.Write("Connect primes\r");
                            if (powerMetrConnect == null || powerMetrConnect.CancellationPending)
                            {
                                e.Cancel = true;
                                return;
                            }
                            PM = new Primes(Settings.PM_Port);
                            Settings.Powermeter = Settings.PowerMeters.Primes;
                            powerMetrConnect.ReportProgress(0, "Connecting to Primes Power Monitor...");
                            PM.Connect();
                            PM.GetData();
                            Thread.Sleep(100);
                            PM.GetData();
                            PWR = PM.GetPower();
                            if (PM.GetTin() > 0)
                            {
                                powerMetrConnect.ReportProgress(50, "Primes Power Monitor connected, measured power= " + PWR.ToString("F02") + " W");
                                PowerMetersError = false;
                            }
                            else
                            {
                                powerMetrConnect.ReportProgress(100, "Connection to Primes Power Monitor failed or com problem");
                                PowerMetersError = true;
                            }
                            

                        }
                    else if(e.Argument.ToString()=="Coherent FieldMax")
                        {
                            
                            Debug.Write("Connect Fieldmax\r");
                            FieldMaxPM = new SimpleFM();
                            powerMetrConnect.ReportProgress(0, "Connecting to FieldMax II power meter...");
                            Settings.Powermeter = Settings.PowerMeters.Filedmax2;
                            int i = 0; 
                            while ((!FieldMaxPM.Ready) && (i < Settings.FIELDMAX_WAIT_READY / 10))
                            {
                                
                                i += 1;
                                if (powerMetrConnect == null || powerMetrConnect.CancellationPending)
                                {
                                    e.Cancel = true;
                                    return;
                                }
                                if (mesPower.InvokeRequired) mesPower.Invoke(new Action(() => mesPower.Text = "0.00"));
                                System.Threading.Thread.Sleep(10);
                                

                            }
                        foreach(var item in FieldMaxPM.metterArray)
                        {
                            powerMetrConnect.ReportProgress(10, item+"\r");
                        }
                            if (!FieldMaxPM.Ready)
                            {
                                powerMetrConnect.ReportProgress(10, "Connection to Fieldmax failed or fieldmax com problem");
                                System.Threading.Thread.Sleep(1000);
                                PowerMetersError = true;
                            }
                            else
                            {
                                PWR = FieldMaxPM.Power;
                                //FieldMaxPM.AttenuationOff(false);
                                powerMetrConnect.ReportProgress(50, "FieldMax II connected, measured power=" + PWR.ToString("F03") + "W");
                                PowerMetersError = false;
                            }
                        }
                        
                    else
                        {
                            PowerMetersError = true;
                        }
                
            }
            catch (Exception ex)
            {                
                if(powerMetrConnect!=null)
                    if (powerMetrConnect.CancellationPending)
                    {
                        e.Cancel = true;
                        return;
                    }
                PowerMetersError = true;
                return;
            }
        }
        private void powerMetrConnect_Changed(object sender, ProgressChangedEventArgs e)
        {
            lstFailuresAdd(e.UserState.ToString());
        }
        private void powerMetrConnect_Completed(object sender, RunWorkerCompletedEventArgs e)
        {
            if (!e.Cancelled && powerMetrConnect != null)
            {
                if (!PowerMetersError)
                {
                    button2.Enabled = true;

                    if (!powerMetrCalibrate.IsBusy && Settings.PowerMetersCalibrate)
                    {
                        lstFailuresAdd("Calibrating power meter.....");
                        powerMetrCalibrate.RunWorkerAsync();
                    }
                    else
                        lstFailuresAdd("Calibration turnOff");
                }
                else
                {
                    lstFailuresAdd("No connection with power meter!");
                    mesPower.Text = "0.00";
                    button2.Enabled = false;
                    button3.Text = "Connect power meter";
                }
            }
            else
            {
                Debug.Write("Cancel powerConnect!\r");
                lstFailuresAdd("Connection aborted");
                button2.Enabled = false;
                /*try
                {
                    PM.Disconnect();
                    PM = null;
                    FieldMaxPM = null;
                }
                catch (Exception ex)
                { 
                    return; 
                }*/
            }
        }

        private void powerMetrCalibrate_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                //check status of calibration  turn off/turn on in Settings
                
                    powerMetrCalibrate.ReportProgress(0, "Start calibration...");
                    if (!PowerMetersError && powerMetrCalibrate != null && !powerMetrCalibrate.CancellationPending)
                    {

                        if (Settings.PowerMeters.Primes == Settings.Powermeter)
                        {
                            int i = 0;
                            while (i <= Settings.PM_WAIT_TIME / 10)
                            {
                                i++;
                                if (powerMetrConnect == null || powerMetrConnect.CancellationPending)
                                {
                                    e.Cancel = true;
                                    return;
                                }
                                System.Threading.Thread.Sleep(10);
                            }
                            PM.GetData();
                            PrimesOffset = PM.GetPower();

                        }
                        else if (Settings.PowerMeters.Filedmax2 == Settings.Powermeter)
                        {

                            PrimesOffset = 0;
                            FieldMaxPM.Zero();
                            if (powerMetrConnect == null || powerMetrConnect.CancellationPending)
                            {
                                e.Cancel = true;
                                return;
                            }
                            int i = 0;
                            //powerMetrCalibrate.ReportProgress(10, FieldMaxPM.reedError());
                            while ((i < Settings.FIELDMAX_ZERO_TIMEOUT) && (FieldMaxPM.reedError().Replace(" ", "") != "0") || !FieldMaxPM.Zeroed)
                            {
                                FieldMaxPM.resetError();
                                System.Threading.Thread.Sleep(10);
                                i += 10;
                                if (powerMetrConnect == null || powerMetrConnect.CancellationPending)
                                {
                                    e.Cancel = true;
                                    return;
                                }
                                if (FieldMaxPM.reedError().Replace(" ", "") != "0")
                                {
                                    powerMetrCalibrate.ReportProgress(10, "!" + FieldMaxPM.reedError().Replace(" ", "") + "!");
                                    FieldMaxPM.resetError();
                                    FieldMaxPM.Zero();
                                }
                                System.Threading.Thread.Sleep(10);

                            }
                            if (!FieldMaxPM.Zeroed)
                            {
                                PowerMetersError = true;
                                powerMetrCalibrate.ReportProgress(100, "Zeroing FieldMax II failed.");
                            }
                        }


                    }
                    else if (powerMetrCalibrate == null || powerMetrCalibrate.CancellationPending)
                    {
                        e.Cancel = true;
                        return;
                    }
                    else
                    {
                        powerMetrCalibrate.ReportProgress(100, "No power meter connected!");
                    }
                    return;
                
            }
            catch (Exception ex)
            {
                if (powerMetrCalibrate != null)
                    if (powerMetrCalibrate.CancellationPending)
                    {
                        e.Cancel = true;
                        return;
                    }
                PowerMetersError = true;
                return;
            }
        }
        private void powerMetrCalibrate_Changed(object sender, ProgressChangedEventArgs e)
        {
            if (e.ProgressPercentage == 0)
            {
                button2.Text = "Stop calibrating";
            }
            lstFailuresAdd(e.UserState.ToString());
        }
        private void powerMetrCalibrate_Completed(object sender, RunWorkerCompletedEventArgs e)
        {
            button2.Text = "Calibrate power metter";
            if (!e.Cancelled && powerMetrCalibrate != null)
            {
                if (!PowerMetersError)
                {
                    //check status of calibration  turn off/turn on in Settings
                    
                        lstFailuresAdd("Calibrate succesful!");
                    if (!powerMetr.IsBusy)
                        powerMetr.RunWorkerAsync();
                }
                else
                    lstFailuresAdd("Calibrate Error!");
                
                return;
            }
            else
            {
                Debug.Write("Cancel powerCalibrate!\r");
                lstFailuresAdd("Calibrate aborted!");
                /*try
                {
                    PM.Disconnect();
                    PM = null;
                    FieldMaxPM = null;
                }
                catch (Exception ex)
                {
                    return;
                }
                return;*/
            }

        }

        private void powerMetr_DoWork(object sender, DoWorkEventArgs e)
        {
            if (powerMetr.CancellationPending)
            {
                e.Cancel = true;
                return;
            }
            if (!PowerMetersError && !powerMetr.CancellationPending)
            {
                try
                {
                    switch (Settings.Powermeter)
                    {
                        case(Settings.PowerMeters.Primes):
                            {
                                PM.GetData();
                                PwrMeas = ((PM.GetPower() - PrimesOffset) * Settings.CalValue).ToString("F02", CultureInfo.InvariantCulture);
                            }
                            break;
                        case(Settings.PowerMeters.Filedmax2):
                            {

                                PwrMeas = (FieldMaxPM.Power * Settings.CalValue).ToString("F02", CultureInfo.InvariantCulture);
                            }
                            break;
                        default:
                            {
                                PwrMeas = "0.00";
                                break;
                            }
                    }
                    string pattern2 = @"[0-9]+.[0-9]{2,2}";
                    MatchCollection matchesResult = Regex.Matches(PwrMeas, pattern2);
                    if (matchesResult.Count > 0)
                    {
                        pattern2 = @"0.00";
                        MatchCollection zeroContains = Regex.Matches(matchesResult[0].ToString(), pattern2);
                        if (zeroContains.Count <= 0)
                        {
                            PwrMeas = matchesResult[0].ToString();
                        }
                    }
                    else if (mesPower.Text != "0" && matchesResult.Count < 0)
                        PwrMeas = "0.00";
                    powerMetr.ReportProgress(100);
                    System.Threading.Thread.Sleep(tmrMain.Interval);
                    return;
                }
                catch (Exception ex)
                {
                    if(powerMetr != null)
                    if (powerMetr.CancellationPending)
                    {
                        e.Cancel = true;
                        return;
                    }
                    PwrMeas = "0.00";
                    return;
                }
                    
                

            }
            else
            {
                if (powerMetrConnect == null || powerMetrConnect.CancellationPending)
                {
                    e.Cancel = true;
                    return;
                }
                PwrMeas = "0";
                powerMetr.ReportProgress(100, "Error power mesarming!");
                return;
            }

        }
        private void powerMetr_Changed(object sender, ProgressChangedEventArgs e)
        {
            mesPower.Text = PwrMeas;
            //MessageBox.Show(PwrMeas+" "+e.UserState.ToString()+e.ProgressPercentage.ToString());
        }
        private void powerMetr_Completed(object sender, RunWorkerCompletedEventArgs e)
        {
            //MessageBox.Show(FieldMaxPM.Power.ToString());
            //MessageBox.Show(e.Cancelled.ToString());
            if (!e.Cancelled && powerMetr != null)
            {
                powerMetr.RunWorkerAsync();
            }
            else
            {
                Debug.Write("Cancel powerMetr!\r");
                mesPower.Text = "0.00";
                powerMeterKill();
                /*try
                {
                    PM.Disconnect();
                    PM = null;
                    FieldMaxPM = null;
                }
                catch (Exception ex)
                {
                    return;
                }
                return;*/
            }
        }

        private void powerMeterRun()
        {
            if(!powerMetrConnect.IsBusy)
            {
                powerMetrConnect.RunWorkerAsync(cbMeasuringDevice.Text);
                /*if(!powerMetrCalibrate.IsBusy)
                {
                    powerMetrCalibrate.RunWorkerAsync();
                    if(!powerMetr.IsBusy)
                    {
                        powerMetr.RunWorkerAsync();
                    }
                    else
                        lstFailuresAdd("Error start powermesarming");
                }
                else
                    lstFailuresAdd("Error start calibarting power meter");
                 */
            }
            else
                lstFailuresAdd("Error connect to power meter");
        }
        private void powerMeterKill()
        {
            if (button3.Text == "Disconnect power meter")
            {
                button2.Enabled = false;
                //powerMetr.RunWorkerCompleted -= new RunWorkerCompletedEventHandler(powerMetr_Completed);
                //powerMetr.DoWork -= new DoWorkEventHandler(powerMetr_DoWork);
                powerMetr.CancelAsync();
                powerMetr.Dispose();
                powerMetr = null;
                // GC.Collect();

                //powerMetrConnect.RunWorkerCompleted -= new RunWorkerCompletedEventHandler(powerMetrConnect_Completed);
                //powerMetrConnect.DoWork -= new DoWorkEventHandler(powerMetrConnect_DoWork);
                powerMetrConnect.CancelAsync();
                powerMetrConnect.Dispose();
                powerMetrConnect = null;
                // GC.Collect();

                //powerMetrCalibrate.RunWorkerCompleted -= new RunWorkerCompletedEventHandler(powerMetrCalibrate_Completed);
                //powerMetrCalibrate.DoWork -= new DoWorkEventHandler(powerMetrCalibrate_DoWork);
                powerMetrCalibrate.CancelAsync();
                powerMetrCalibrate.Dispose();
                powerMetrCalibrate = null;
                GC.Collect();
                //FieldMaxPM.AttenuationOff(true);
                //System.Threading.Thread.Sleep(1000);
                PowerMetersError = true;
                try
                {
                    FieldMaxPM.Disconnect();
                    FieldMaxPM = null;
                    GC.Collect();
                    //MessageBox.Show(FieldMaxPM.Ready.ToString());
                }
                catch (Exception ex)
                {
                    Debug.Write(ex.Message+"#######\r");
                    FieldMaxPM = null;
                    GC.Collect();
                }
                try
                {
                    PM.Disconnect();
                    PM = null;
                    GC.Collect();
                }
                catch (Exception ex)
                {
                    Debug.Write(ex.Message + "##########\r");
                    PM = null;
                    GC.Collect();
                }
                
                
            }
            
        }

        /*
        private void PD3BackgroundMonitoring_DoWork(object sender, DoWorkEventArgs e)
        {
            PD3Message = lastPD3.ToString();
            PD3BackgroundMonitoring.ReportProgress(0);
        }
        private void PD3BackgroundMonitoring_Changed(object sender, ProgressChangedEventArgs e)
        {
            lstFailuresAdd(PD3Message);
        }
        private void PD3BackgroundMonitoring_Completed(object sender, RunWorkerCompletedEventArgs e)
        {
            lstFailuresAdd(PD3Message);
            PD3BackgroundMonitoring.RunWorkerAsync();
        }
        */
        void mModule_NewDataAvaliable(object sender, EventArgs e)
        {
            
            
        }



        private void tmrMain_Tick(object sender, EventArgs e)
        {
                    dataModuleReciver();
            /*
                    if (!PD3BackgroundMonitoring.IsBusy)
                    {
                        PD3BackgroundMonitoring.RunWorkerAsync();
                    }
             */
                    txtTemp.Text = DataMod.Temp.ToString("F01");
                    indPower.Text = DataMod.Power.ToString("F00");
                    txtPSVoltage.Text = DataMod.PSVoltage.ToString("F01");
            
           
                    txtUset.Text = DataMod.Uset.ToString("F00");

                    txtILD1.Text = DataMod.ILD1.ToString("F02");
                    txtILD2.Text = DataMod.ILD2.ToString("F02");
                    txtILD3.Text = DataMod.ILD3.ToString("F02");
                    txtILD4.Text = DataMod.ILD4.ToString("F02");
                    txtILD5.Text = DataMod.ILD5.ToString("F02");
                    txtILD6.Text = DataMod.ILD6.ToString("F02");

                    txtPD1.Text = DataMod.PD1.ToString("F00");
                    txtPD2.Text = DataMod.PD2.ToString("F00");
                    

                    //averaging PD3 value
                    if (DateTime.Now.Subtract(startAveraging).TotalSeconds <= Convert.ToDouble(averagingPD3.Value) && DataMod.PD3 != 0)
                    {
                        countAveraging += 1;
                        averagingPD3Value += DataMod.PD3;
                    }
                    else if (DataMod.PD3 == 0 || averagingPD3.Value==0)
                    {
                        txtPD3.Text = DataMod.PD3.ToString("F00");
                        startAveraging = DateTime.Now;
                    }
                    else
                    {
                        //DataMod.PD3 = averagingPD3Value / countAveraging;
                        txtPD3.Text = (averagingPD3Value / countAveraging).ToString("F00");
                        countAveraging = 0;
                        averagingPD3Value = 0;
                        startAveraging = DateTime.Now;
                    }

                    txtPD4.Text = DataMod.PD4.ToString("F00");
                    txtPD5.Text = DataMod.PD5.ToString("F00");

                    this.Text = txtModuleID.Text + " - "+mainText;
                    if (StatusMod.Enabled)
                    {
                        lblEnabled.ForeColor = Color.Black;
                        lblEnabled.BackColor = Color.Lime;
                    }
                    else
                    {
                        lblEnabled.BackColor = SystemColors.Control;
                        lblEnabled.ForeColor = Color.Gray;
                    }

                    if (StatusMod.PowerSupply && DataMod.PSVoltage>10 && !DataMod.Error)
                    {
                        lblPowerSupply.BackColor = Color.Lime;
                        lblPowerSupply.ForeColor = Color.Black;
                        chbEmissionOn.Enabled = true;
                        chkQCW.Enabled = true;
                    }
                    else if (DataMod.Error || DataMod.PSVoltage<10)
                    {
                        if (StatusMod.PowerSupply || DataMod.PSVoltage < 10)
                        {
                            lblPowerSupply.BackColor = Color.Red;
                            lblPowerSupply.ForeColor = Color.Black;
                        }
                        else
                        {
                            lblPowerSupply.BackColor = SystemColors.Control;
                            lblPowerSupply.ForeColor = Color.Gray;
                        }
                        chbEmissionOn.Enabled = false;
                        chbEmissionOn.Checked = false;
                        tbIset.Enabled = false;
                        chkQCW.Enabled = false;
                        udIset.Enabled = false;
                        tbIset.Value = tbIset.Minimum;
                        udIset.Value = tbIset.Minimum;

                        
                    }
                    else
                    {

                        lblPowerSupply.BackColor = SystemColors.Control;
                        lblPowerSupply.ForeColor = Color.Gray;
                        chbEmissionOn.Enabled = false;
                        tbIset.Enabled = false;
                        chkQCW.Enabled = false;
                        udIset.Enabled = false;
                    }
                    if (StatusMod.Emission)
                    {
                        lblEmission.BackColor = Color.Orange;
                        lblEmission.ForeColor = Color.Black;
                        gbPLD.Enabled = true;
                    }
                    else
                    {
                        lblEmission.BackColor = SystemColors.Control;
                        lblEmission.ForeColor = Color.Gray;
                        //impossible to start burnIn without Emission
                        gbPLD.Enabled = false;
                        
                    }
                    if (!chbEmissionOn.Checked)
                    {
                        tbIset.Enabled = false;
                        udIset.Enabled = false;
                        tbIset.Value = tbIset.Minimum;
                        udIset.Value = tbIset.Minimum;
                    }
                    else
                    {
                        tbIset.Enabled = true;
                        udIset.Enabled = true;
                    }
                    if (StatusMod.ExternalShutdown)
                    {
                        lblExternalShutdown.BackColor = Color.Lime;
                        lblExternalShutdown.ForeColor = Color.Black;
                    }
                    else
                    {
                        lblExternalShutdown.BackColor = SystemColors.Control;
                        lblExternalShutdown.ForeColor = Color.Gray;
                    }

                    if (StatusMod.PulsedMode)
                    {
                        lblPulsedMode.BackColor = Color.Lime;
                        lblPulsedMode.ForeColor = Color.Black;
                    }
                    else
                    {
                        lblPulsedMode.BackColor = SystemColors.Control;
                        lblPulsedMode.ForeColor = Color.Gray;
                    }

                    if (StatusMod.Error)// || DataMod.Error)
                    {
                        lblError.BackColor = Color.Red;
                        lblError.ForeColor = Color.Black;
                    }
                    else
                    {
                        lblError.BackColor = SystemColors.Control;
                        lblError.ForeColor = Color.Gray;
                    }
                    if (DateTime.Now.Subtract(autoStartBrnTimer).TotalSeconds > (Convert.ToDouble(autoStartBrn.Value) * 60) && btnStartBurnIn.Text == "Start" && StatusMod.Emission)
                    {
                        btnStartBurnIn_Click(this, new EventArgs());
                        autoStartBrnTimer = new DateTime(2020, 1, 1);
                    }
                    if (WriteStability)
                    {

                        txtTimerAdd();
                        // control trubles in module when PD3 drop too much
                        if (startPD3Monitoring == 0) 
                        {
                            startPD3Monitoring = DataMod.PD3;
                        }
                        if ((startPD3Monitoring - DataMod.PD3) >= (int)udMinDropPowerDrop.Value)//monitoring real Power Drop value without averaging
                        {
                            lblPLDFailure.ForeColor = Color.Black;
                            lblPLDFailure.BackColor = Color.Red;
                            if (chbEmissionOffwhenPowerDrop.Checked)
                            {
                                lstFailuresAdd(DateTime.Now.ToString("G", CultureInfo.GetCultureInfo("en-GB")) + " Power drop!" + " Cycle Time: " + CycleTime + " Full Time: " + FullTime);
                                lstFailuresAdd(DateTime.Now.ToString("G", CultureInfo.GetCultureInfo("en-GB")) + " Emission Off.");
                                btnStartBurnIn_Click(this, new EventArgs());
                                chbEmissionOn.Checked = false;
                            }

                        }

                        //monitoring PD3
                        try
                        {

                            
                            if (monitoring)
                            {
                                identValues = 0;
                                countMonitoringAveraging = 0;
                                monitoringAveragingPD3Value = 0;
                                counter = 0;
                                lastPD3 = Convert.ToInt32(txtPD3.Text);
                                minMonitoringPD3 = lastPD3;
                                fullTimeMonitoring = DateTime.Now;
                                StartMonitoringPD3 = DateTime.Now;
                                monitoring = false;
                                //lstFailuresAdd("Start Monitoring");
                                

                            }
                            if (averagingPD3.Value == 0)
                            {
                                countMonitoringAveraging += 1;
                                monitoringAveragingPD3Value += Convert.ToInt32(txtPD3.Text);
                            }
                            //waiting time
                            
                            if (DateTime.Now.Subtract(StartMonitoringPD3).TotalSeconds >= counter)
                            {
                                counter += 1;
                                int pd3Value;
                                if (averagingPD3.Value == 0)
                                {
                                    pd3Value = Convert.ToInt32(monitoringAveragingPD3Value / countMonitoringAveraging);
                                }
                                else
                                {
                                    pd3Value = Convert.ToInt32(txtPD3.Text);
                                }
                                countMonitoringAveraging = 0;
                                monitoringAveragingPD3Value = 0;
                                //lstFailuresAdd(lastPD3 + "  " + pd3Value + "  " + minMonitoringPD3 + "  " + DateTime.Now.Subtract(StartMonitoringPD3).TotalSeconds);
                                if ((DateTime.Now.Subtract(StartMonitoringPD3).TotalSeconds < monitoringPD3Interval) && (pd3Value < minMonitoringPD3))
                                {
                                    if (pd3Value < minMonitoringPD3)
                                    {
                                        minMonitoringPD3 = pd3Value;
                                        StartMonitoringPD3 = DateTime.Now;
                                        counter = 0;
                                        identValues = 0;
                                    }

                                }
                                else if (pd3Value >= minMonitoringPD3 && identValues < PD3ChangeDirect.Value)
                                {
                                    identValues += 1;
                                }
                                else
                                {
                                    // limit time for PD3 getting down if fiber block degrodating
                                    if ((lastPD3 - minMonitoringPD3) > udMinDrop.Value && DateTime.Now.Subtract(fullTimeMonitoring).TotalSeconds <= (double)PD3MaxTimeMonitoringCycle.Value)
                                    {
                                        lblPLDFailure.ForeColor = Color.Black;
                                        lblPLDFailure.BackColor = Color.Red;
                                        //lstFailuresAdd(DateTime.Now.ToString("G", CultureInfo.GetCultureInfo("en-GB")) + " Drop:" +
                                         //    lastPD3.ToString("F00") + "->" + minMonitoringPD3.ToString("F00") + "   Cycle Time: " + txtStatus1.Text + "   Full Time: " + txtStatus2.Text);
                                        using (StreamWriter sw = new StreamWriter(txtLogFile.Text, true))
                                        {
                                            lblPLDFailure.ForeColor = Color.Black;
                                            lblPLDFailure.BackColor = Color.Red;
                                            lstFailuresAdd(DateTime.Now.ToString("G", CultureInfo.GetCultureInfo("en-GB")) + " Drop:" +
                                                 lastPD3.ToString("F00") + "->" + minMonitoringPD3.ToString("F00") + "   Cycle Time: " + txtStatus1.Text + "   Full Time: " + txtStatus2.Text);
                                            sw.WriteLine(Convert.ToInt32(BurnInFullTimeSec) + logSeparator + DateTime.Now.ToString("G",
                                                    CultureInfo.GetCultureInfo("en-GB")) + logSeparator + txtILD1.Text + logSeparator + txtPD3.Text + logSeparator + txtTemp.Text + logSeparator + mesPower.Text + logSeparator +
                                                    "Drop:" + lastPD3.ToString("F00") + "->" + minMonitoringPD3.ToString("F00") + logSeparator + "Cycle Time:" + txtStatus1.Text + logSeparator + "Full Time:" + txtStatus2.Text);
                                        }
                                    }
                                    monitoring = true;
                                    //countMonitoringAveraging = 0;
                                    //monitoringAveragingPD3Value = 0;
                                    //counter = 0;

                                }
                            }
                            
                            
                        }
                        catch (Exception ex)
                        {
                            //lstFailuresAdd(ex.Message);
                            monitoring = true;
                        }


                        if (StatusMod.Error)
                        {
                            Debug.Write("Making error report\r");
                            txtStatus1.Text = "Error";
                            txtStatus1.BackColor = Color.Red;
                            lblPLDFailure.ForeColor = Color.Black;
                            lblPLDFailure.BackColor = Color.Red;
                            {//forming alarms

                                if (AlarmsMod.CS1)
                                {
                                    errorModuleMessage = errorModuleMessage + "CS1;";
                                }
                                if (AlarmsMod.CS2)
                                {
                                    errorModuleMessage = errorModuleMessage + "CS2;";
                                }
                                if (AlarmsMod.CS3)
                                {
                                    errorModuleMessage = errorModuleMessage + "CS3;";
                                }
                                if (AlarmsMod.CS4)
                                {
                                    errorModuleMessage = errorModuleMessage + "CS4;";
                                }
                                if (AlarmsMod.CS5)
                                {
                                    errorModuleMessage = errorModuleMessage + "CS5;";
                                }
                                if (AlarmsMod.CS6)
                                {
                                    errorModuleMessage = errorModuleMessage + "CS6;";
                                }
                                if (AlarmsMod.CurrentLeak)
                                {
                                    errorModuleMessage = errorModuleMessage + "Current Leak;";
                                }
                                if (AlarmsMod.DutyCycle)
                                {
                                    errorModuleMessage = errorModuleMessage + "Duty Cycle;";
                                }
                                if (AlarmsMod.Overheat)
                                {
                                    errorModuleMessage = errorModuleMessage + "Overheat;";
                                }
                                if (AlarmsMod.PD1)
                                {
                                    errorModuleMessage = errorModuleMessage + "PD1;";
                                }
                                if (AlarmsMod.PD2)
                                {
                                    errorModuleMessage = errorModuleMessage + "PD2;";
                                }
                                if (AlarmsMod.PD3)
                                {
                                    errorModuleMessage = errorModuleMessage + "PD3;";
                                }
                                if (AlarmsMod.PD4)
                                {
                                    errorModuleMessage = errorModuleMessage + "PD4;";
                                }
                                if (AlarmsMod.PD5)
                                {
                                    errorModuleMessage = errorModuleMessage + "PD5;";
                                }
                                if (AlarmsMod.PowerSupply || DataMod.PSVoltage == 0)
                                {
                                    errorModuleMessage = errorModuleMessage + "Power Supply;";
                                }
                                if (AlarmsMod.HighBackReflection)
                                {
                                    errorModuleMessage = errorModuleMessage + "High Back Reflection;";
                                }
                                if (AlarmsMod.GndLeak)
                                {
                                    errorModuleMessage = errorModuleMessage + "GND Leakage;";
                                }
                                if (AlarmsMod.UnexpectedPump1)
                                {
                                    errorModuleMessage = errorModuleMessage + "Unexpected Pump Current 1;";
                                }
                                if (AlarmsMod.UnexpectedPump2)
                                {
                                    errorModuleMessage = errorModuleMessage + "Unexpected Pump Current 2;";
                                }
                                if (AlarmsMod.UnexpectedPump3)
                                {
                                    errorModuleMessage = errorModuleMessage + "Unexpected Pump Current 3;";
                                }
                                if (AlarmsMod.UnexpectedPump4)
                                {
                                    errorModuleMessage = errorModuleMessage + "Unexpected Pump Current 4;";
                                }
                                if (AlarmsMod.UnexpectedPump5)
                                {
                                    errorModuleMessage = errorModuleMessage + "Unexpected Pump Current 5;";
                                }
                                if (AlarmsMod.UnexpectedPump6)
                                {
                                    errorModuleMessage = errorModuleMessage + "Unexpected Pump Current 6;";
                                }

                            }
                            Debug.WriteLine(errorModuleMessage);
                            tmrLog_Tick(this, new EventArgs());
                            lstFailuresAdd(DateTime.Now.ToString("G", CultureInfo.GetCultureInfo("en-GB")) + " " + errorModuleMessage + " Cycle time: " + CycleTime + " Full Time: " + FullTime);
                            btnStartBurnIn_Click(this, new EventArgs());
                            chbEmissionOn.Checked = false;
                            errorModuleMessage = "";
                        }
                    }
                    else
                    {
                        //reset PD3 monitoring counters
                        monitoring = true;
                        countMonitoringAveraging = 0;
                        monitoringAveragingPD3Value = 0;
                        counter = 0;
                    }
        }
        
        private void tmrLog_Tick(object sender, EventArgs e)
        {
            if (WriteStability)
            {
                /*try
                {
                    Power = Convert.ToInt32(txtPD3.Text);
                }
                catch (Exception ex)
                {
                    lstFailuresAdd(txtPD3.Text.ToString());// check error, comment after.
                    if (PrevPower > 0)
                    {
                        Power = PrevPower;
                    }
                    else
                    Power = 0;
                }*/
               // if (Convert.ToInt32(BurnInStart) != 0)
               // {
                 //   BurnInFullTimeSec = BurnInFullTime + DateTime.Now.Subtract(BurnInStart).TotalSeconds;
               /* }
                else
                {
                    BurnInStart = DateTime.Now;
                    BurnInFullTimeSec = 0;
                }*/
                //CycleTimeValue = DateTime.Now - BurnInStart;
                using (StreamWriter sw = new StreamWriter(txtLogFile.Text, true))
                {
                    /*
                    //Analize 'on the way' if there is PLD failures
                    if ((PrevPower - Power) >= PLD_DROP_MIN)
                    {
                        lblPLDFailure.ForeColor = Color.Black;
                        lblPLDFailure.BackColor = Color.Red;
                        lstFailuresAdd(DateTime.Now.ToString("G", CultureInfo.GetCultureInfo("en-GB")) + " Drop:" + 
                                     PrevPower.ToString("F00") + "->" + Power.ToString("F00") + "   Cycle Time: " + txtStatus1.Text + "   Full Time: " + txtStatus2.Text);
                        sw.WriteLine(Convert.ToInt32(BurnInFullTimeSec) + logSeparator + DateTime.Now.ToString("G", 
                                    CultureInfo.GetCultureInfo("en-GB")) + logSeparator + txtILD1.Text + logSeparator + txtPD3.Text + logSeparator+txtTemp.Text+logSeparator + mesPower.Text + logSeparator +
                                    "Drop:" + PrevPower.ToString("F00") + "->" + Power.ToString("F00") + logSeparator + "Cycle Time:" + txtStatus1.Text + logSeparator + "Full Time:" + txtStatus2.Text);

                    }
                    else
                    {
                      */
                        BurnInFullTimeSec = BurnInFullTime + DateTime.Now.Subtract(BurnInStart).TotalSeconds;
                        CycleTimeValue = DateTime.Now - BurnInStart;
                        if (errorModuleMessage == "")
                            sw.WriteLine(Convert.ToInt32(BurnInFullTimeSec) + logSeparator + DateTime.Now.ToString("G",
                                CultureInfo.GetCultureInfo("en-GB")) + logSeparator + txtILD1.Text + logSeparator + txtPD3.Text + logSeparator + txtTemp.Text + logSeparator + mesPower.Text);
                        else
                            sw.WriteLine(Convert.ToInt32(BurnInFullTimeSec) + logSeparator + DateTime.Now.ToString("G",
                                CultureInfo.GetCultureInfo("en-GB")) + logSeparator + txtILD1.Text + logSeparator + txtPD3.Text + logSeparator + txtTemp.Text + logSeparator + mesPower.Text + logSeparator + errorModuleMessage);
                   // }

                }


            }
        }

        

        private void tbIset_Scroll(object sender, EventArgs e)
        {
            switch (IGFLAG)
            {
                case 1:
                    udIset.Value = tbIset.Value;
                    mModule.Iset = tbIset.Value;
                    if (tbIset.Value > 0)
                    autoStartBrnTimer = DateTime.Now;
                    break;
                case 0:

                    udIset.Value = tbIset.Value;
                    rackLaser.Iset = tbIset.Value;
                    if (rackLaser.Settings.currentSetMinimum > 0)
                    autoStartBrnTimer = DateTime.Now;
                    break;
            }
            
        }

        private void udIset_ValueChanged(object sender, EventArgs e)
        {
            switch (IGFLAG)
            {
                case 1:
                    tbIset.Value = (int)udIset.Value;
                    mModule.Iset = tbIset.Value;
                    if(udIset.Value>0)
                    autoStartBrnTimer = DateTime.Now;
                    break;
                case 0:
                    tbIset.Value = (int)udIset.Value;
                    rackLaser.Iset = tbIset.Value;
                    if (udIset.Value >= rackLaser.Settings.currentSetMinimum)
                    autoStartBrnTimer = DateTime.Now;
                    break;
            }
            
        }



        private void chbEmissionOn_CheckedChanged(object sender, EventArgs e)
        {
            if (chbEmissionOn.Checked)
            {
                chbEmissionOn.BackColor = Color.Orange;
            }
            else
            {
                if (btnStartBurnIn.Text == "Stop")
                {
                    btnStartBurnIn_Click(this, new EventArgs());
                }
                chbEmissionOn.BackColor = Color.Transparent;
            }
            switch (IGFLAG)
            {
                case 1:
                    mModule.EmissionOn = chbEmissionOn.Checked;
                    break;
                case 0:
                    rackLaser.EmissionOn = chbEmissionOn.Checked;
                    break;
            }
            
        }

        private void chkQCW_CheckedChanged(object sender, EventArgs e)
        {
            switch (IGFLAG)
            {
                case 1:
                    mModule.QCWmode = chkQCW.Checked;
                    break;
                case 0:
                    rackLaser.QCWmode = chkQCW.Checked;
                    break;
            }

        }

        private void cbMeasuringDevice_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbMeasuringDevice.Text == "Primes PM")
            {
                cmbPMport.Enabled = true;
                button3.Enabled = true;
                udCalCoeff.Enabled = true;
            }
            else if (cbMeasuringDevice.Text == "NONE")
            {
                cmbPMport.Enabled = false;
                button3.Enabled = false;
                udCalCoeff.Enabled = false;
                button2.Enabled = false;
            }
            else
            {
                cmbPMport.Enabled = false;
                button3.Enabled = true;
                udCalCoeff.Enabled = true;
            }
        }

        private void udCalCoeff_ValueChanged(object sender, EventArgs e)
        {
            Settings.CalValue = Convert.ToDouble(udCalCoeff.Value);  
        }



        private void button1_Click(object sender, EventArgs e)
        {
            //string pwr = FieldMaxPM.Power.ToString();
            if (IgType.Text == "IG243-9" && !Connected)
            {
                IGFLAG = 1;
                mModule = new Module();
                {
                    SettingsMod.currentChangeStep = mModule.Settings.currentChangeStep;
                    SettingsMod.currentSetMax = mModule.Settings.currentSetMax;
                    SettingsMod.currentSetMinimum = mModule.Settings.currentSetMinimum;
                    logDirectory1 = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\Stability\\IG243\\";
                    logDirectory = logDirectoryIG243;
                }

            }
            else if (IgType.Text == "IG337" && !Connected)
            {
                IGFLAG = 0;
                rackLaser = new RackLaser();
                {
                    SettingsMod.currentChangeStep = rackLaser.Settings.currentChangeStep;
                    SettingsMod.currentSetMax = rackLaser.Settings.currentSetMax;
                    SettingsMod.currentSetMinimum = rackLaser.Settings.currentSetMinimum;
                    logDirectory1 = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\Stability\\IG337\\";
                    logDirectory = logDirectoryIG337;
                }

            }

            switch (Connected)
            {
                case false:
                    {

                        if (cbPort.Text != "")
                        {
                            Connected = true;
                            startAveraging = DateTime.Now;
                            button1.Text = "Disconnect";
                            if (button3.Text == "Connect power meter")
                                button3_Click(this, new EventArgs());
                            switch (IGFLAG)
                            {
                                case 1:
                                    mModule.NewDataAvaliable += mModule_NewDataAvaliable;
                                    mModule.ComPortName = cbPort.Text;
                                    mModule.StartCommunication();

                                    txtModuleID.Text = mModule.DataMod.ModuleID;
                                    txtModuleFirmware.Text = mModule.DataMod.ModuleFirmware;

                                    udMinDrop.Value = Module_Burn_inTool.Properties.Settings.Default.MinPdDropIg243;
                                    lblError.Text = "Error";
                                    if (DataMod.Error)
                                    {
                                        txtModuleID.ForeColor = Color.Black;
                                        txtModuleID.BackColor = Color.Red;
                                    }


                                    break;
                                case 0:

                                    rackLaser.NewDataAvaliable += mModule_NewDataAvaliable;
                                    rackLaser.ComPortName = cbPort.Text;
                                    rackLaser.StartCommunication();

                                    txtModuleID.Text = rackLaser.DataMod.ModuleID;
                                    txtModuleFirmware.Text = rackLaser.DataMod.ModuleFirmware;
                                    udMinDrop.Value = Module_Burn_inTool.Properties.Settings.Default.MinPdDropIg337;
                                    lblError.Text = "Critical Error";
                                    if (DataMod.Error)
                                    {
                                        txtModuleID.ForeColor = Color.Black;
                                        txtModuleID.BackColor = Color.Red;
                                    }
                                    break;
                            }
                            tbIset.Maximum = SettingsMod.currentSetMax;
                            udIset.Maximum = SettingsMod.currentSetMax;
                            tbIset.Minimum = SettingsMod.currentSetMinimum;
                            udIset.Minimum = SettingsMod.currentSetMinimum;
                            tbIset.LargeChange = SettingsMod.currentChangeStep;


                            
                            autoStartBrnTimer = new DateTime(2020, 1, 1);
                            lblPLDFailure.ForeColor = Color.Gray;
                            lblPLDFailure.BackColor = SystemColors.Control;
                            lstFailures.Items.Clear();
                            tmrMain.Enabled = true;
                            IgType.Enabled = false;
                            //buttons turn on if no Errors. do not uncomment!
                            //chbEmissionOn.Enabled = true;
                            //chkQCW.Enabled = true;
                            //tbIset.Enabled = true;
                            //udIset.Enabled = true;
                            //gbPLD.Enabled = true;
                            txtLogFile.Text = logDirectory + txtModuleID.Text + "_stability.txt";
                            switch (IGFLAG)
                            {
                                case 1:

                                    if (mModule.comPortErr)
                                    {
                                        button1_Click(this, new EventArgs());
                                        MessageBox.Show("Error connecting to Module");
                                    }

                                    break;
                                case 0:

                                    if (rackLaser.comPortErr)
                                    {
                                        button1_Click(this, new EventArgs());
                                        MessageBox.Show("Error connecting to Module");
                                    }
                                    break;
                            }


                        }
                        else
                        {
                            MessageBox.Show("No com port selected");
                        }


                    }
                    break;
                case true:
                    {
                        //stop birnIn if it's run
                        if (btnStartBurnIn.Text == "Stop")
                            btnStartBurnIn_Click(this, new EventArgs());
                        tmrMain.Enabled = false;
                        //Settings.Powermeter;
                        switch (IGFLAG)
                        {
                            case 1:
                                mModule.StopCommunication();

                                break;
                            case 0:
                                rackLaser.StopCommunication();
                                break;
                        }

                        Connected = false;
                        //resize Time box
                        txtStatus1.Location = new Point(Convert.ToInt32(txtStatus1L[0]), Convert.ToInt32(txtStatus1L[1]));
                        txtStatus1.Size = new Size(txtStatus1.Width, Convert.ToInt32(txtStatus1L[2]));
                        txtStatus1.Font = new System.Drawing.Font("Microsoft Sans Serif", Convert.ToInt32(txtStatus1L[3]), FontStyle.Bold);
                        txtStatus2.Location = new Point(Convert.ToInt32(txtStatus2L[0]), Convert.ToInt32(txtStatus2L[1]));
                        txtStatus2.Size = new Size(txtStatus2.Width, Convert.ToInt32(txtStatus2L[2]));
                        txtStatus2.Font = new System.Drawing.Font("Microsoft Sans Serif", Convert.ToInt32(txtStatus2L[3]), FontStyle.Bold);

                        button1.Text = "Connect";

                        this.Text = mainText;
                        txtTemp.Text = "";
                        indPower.Text = "";
                        mesPower.Text = "";
                        txtStatus1.Text = "";
                        txtStatus2.Text = "";
                        txtUset.Text = "";

                        txtILD1.Text = "";
                        txtILD2.Text = "";
                        txtILD3.Text = "";
                        txtILD4.Text = "";
                        txtILD5.Text = "";
                        txtILD6.Text = "";

                        txtPD1.Text = "";
                        txtPD2.Text = "";
                        txtPD3.Text = "";
                        txtPD4.Text = "";
                        txtPD5.Text = "";
                        txtPSVoltage.Text = "";
                        txtModuleID.Text = "";
                        txtModuleFirmware.Text = "";
                        chbEmissionOn.Enabled = false;
                        chbEmissionOn.BackColor = Color.Transparent;
                        chbEmissionOn.Checked = false;
                        IgType.Enabled = true;
                        tbIset.Minimum = 0;
                        udIset.Minimum = 0;
                        tbIset.Value = 0;
                        udIset.Value = 0;
                        chkQCW.Enabled = false;
                        tbIset.Enabled = false;
                        udIset.Enabled = false;
                        gbPLD.Enabled = false;
                        lblEmission.ForeColor = Color.Gray;
                        lblEmission.BackColor = SystemColors.Control;
                        lblEnabled.ForeColor = Color.Gray;
                        lblEnabled.BackColor = SystemColors.Control;
                        lblError.ForeColor = Color.Gray;
                        lblError.BackColor = SystemColors.Control;
                        lblExternalShutdown.ForeColor = Color.Gray;
                        lblExternalShutdown.BackColor = SystemColors.Control;
                        lblPowerSupply.ForeColor = Color.Gray;
                        lblPowerSupply.BackColor = SystemColors.Control;
                        lblPulsedMode.ForeColor = Color.Gray;
                        lblPulsedMode.BackColor = SystemColors.Control;
                        txtModuleID.ForeColor = Color.Gray;
                        txtModuleID.BackColor = SystemColors.Control; ;
                    }
                    break;
            }



        }

        private void button2_Click(object sender, EventArgs e)
        {

            if (button2.Text == "Stop calibrating" && powerMetrCalibrate.IsBusy && !StatusMod.Emission)
            {
                powerMetrCalibrate.CancelAsync();
            }
            else if (StatusMod.Emission)
            {
                lstFailuresAdd("Turn Off power before calibration!");
            }
            else if (!powerMetrCalibrate.IsBusy)
            {
                powerMetrCalibrate.RunWorkerAsync();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (button3.Text == "Connect power meter")
            {
                if (cmbPMport.Text != "" && cmbPMport.Enabled)
                {
                    Settings.PM_Port = cmbPMport.Text;
                    button3.Text = "Disconnect power meter";
                    cbMeasuringDevice.Enabled = false;
                    cmbPMport.Enabled = false;
                    Settings.CalValue = Convert.ToDouble(udCalCoeff.Value);
                    InitializeBackgroundWorker();
                    powerMeterRun();
                }
                else if (!cmbPMport.Enabled)
                {
                    button3.Text = "Disconnect power meter";
                    cbMeasuringDevice.Enabled = false;
                    Settings.CalValue = Convert.ToDouble(udCalCoeff.Value);
                    InitializeBackgroundWorker();
                    powerMeterRun();
                }
                else
                {
                    lstFailuresAdd("No COM port selected!");
                }

            }
            else
            {
                button3.Enabled = false;
                powerMeterKill();
                button3.Text = "Connect power meter";
                button3.Enabled = true;
                cbMeasuringDevice_SelectedIndexChanged(this, new EventArgs());
                cbMeasuringDevice.Enabled = true;
            }
        }

        private void btnStartBurnIn_Click(object sender, EventArgs e)
        {
            WriteStability = !WriteStability;
            if (btnStartBurnIn.Text == "Start")
            {
                //timer monitoring PD3 value
                if (averagingPD3.Value != 0)
                {
                    if (PD3Monitoring.Value < 25)
                        monitoringPD3Interval = Convert.ToInt32(averagingPD3.Value) * Convert.ToInt32(PD3Monitoring.Value);
                    else
                        monitoringPD3Interval = Convert.ToInt32(PD3Monitoring.Value);
                }
                else
                {
                    monitoringPD3Interval = Convert.ToInt32(PD3Monitoring.Value);
                }

                lblPLDFailure.ForeColor = Color.Gray;
                lblPLDFailure.BackColor = SystemColors.Control;
                lstFailures.Items.Clear();
                btnStartBurnIn.Text = "Stop";
                udCalCoeff.Enabled = false; //disable  possibility to cgange coeff during Burning
                
                try
                {
                    if (!Directory.Exists(logDirectory))
                    {
                        logDirectory = logDirectory1;
                        txtLogFile.Text = logDirectory + txtModuleID.Text + "_stability.txt";
                        if (!Directory.Exists(logDirectory))
                            Directory.CreateDirectory(logDirectory);
 
                    }
                        
                    using (StreamReader sr = new StreamReader(txtLogFile.Text))
                    {
                        string currentLine = "";
                        string[] LineArray;
                        while (sr.Peek() >= 0)
                        {
                            currentLine = sr.ReadLine();
                            //display history of drops in error window
                            if (currentLine.Contains("Drop"))
                            {
                                
                                
                                if (currentLine.Contains(logSeparator))
                                {
                                    //don't turn red light on if it's too old Error
                                    Regex regex = new Regex(logSeparator);
                                    string[] match = regex.Split(currentLine);
                                    if (DateTime.Now.Subtract(Convert.ToDateTime(match[1])).TotalDays <= 2)
                                    {
                                        lblPLDFailure.BackColor = Color.Red;
                                        lblPLDFailure.ForeColor = Color.Black;
                                    }
                                    string displayString = currentLine.Replace(logSeparator, "   ");
                                    string time = displayString.Remove(displayString.IndexOf(" "));
                                    displayString = displayString.Remove(0, displayString.IndexOf(" ") + 3);
                                    if (displayString.Length > 80)
                                    {
                                        string[] separated=displayString.Split('D');
                                        lstFailuresAdd(separated[0]);
                                        lstFailuresAdd("D"+separated[1]);
                                        lstFailuresAdd("");
                                    }
                                    else
                                    lstFailuresAdd(displayString);
                                }
                                else
                                    lstFailuresAdd(currentLine);

                            }

                           
                        }
                        Regex rgx = new Regex(logSeparator);
                        LineArray = rgx.Split(currentLine);
                        BurnInFullTime = Convert.ToInt32(LineArray[0]);
                        BurnInFullTimeSec = BurnInFullTime;
                        BurnInStart = DateTime.Now;
                    }
                }
                catch (Exception ex)
                {
                    BurnInFullTime = 0;
                    BurnInFullTimeSec = 0;
                    BurnInStart = DateTime.Now;
                }
                CycleTimeValue = DateTime.Now - BurnInStart;
                tmrLog.Interval = Convert.ToInt32(Settings.udCheckPeriod) * 1000;
                tmrLog.Enabled = true;
                lstFailuresAdd(DateTime.Now.ToString("G", CultureInfo.GetCultureInfo("en-GB")) + " Burn in Start.");

            }
            else
            {
                txtStatus1.BackColor = SystemColors.Control;
                tmrLog.Enabled = false;
                lstFailuresAdd(DateTime.Now.ToString("G", CultureInfo.InvariantCulture) + " Burn in Stop.");
                btnStartBurnIn.Text = "Start";
                BurnInFullTime = 0;
            }

        }

        private void btnLogFile_Click(object sender, EventArgs e)
        {
            saveFileDialog1.AddExtension = true;
            saveFileDialog1.DefaultExt = ".txt";
            saveFileDialog1.FileName = txtModuleID.Text + "_stability";
            saveFileDialog1.DefaultExt = "txt";
            saveFileDialog1.Filter = "txt files (*.txt)|*.txt";
            saveFileDialog1.OverwritePrompt=false;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                txtLogFile.Text = saveFileDialog1.FileName; 
            }
            logDirectory = txtLogFile.Text;
        }
        
        private void btnSettings_Click(object sender, EventArgs e)
        {
            switch (this.Width)
            {
                case 764:
                    {
                        this.Width = 504;
                        break;
                    }
                case 504:
                    {
                        this.Width = 764;
                        break;
                    }

            }
        }

        private void txtStatus1_click(object sender, EventArgs e)
        {

            if (txtStatus1.Text.Contains("D"))
            {
                txtStatus1oldText = txtStatus1.Text;

                string days = txtStatus1.Text.Remove(txtStatus1.Text.IndexOf("D")).Replace(" ", "");
                string time = txtStatus1.Text.Remove(0, txtStatus1.Text.IndexOf("D") + 1);
                string[] timeDif = time.Split(new char[] { ':' });
                string hours = timeDif[0].Replace(" ", "");
                string minutes = timeDif[1].Replace(" ", "");
                string totalTimeHours = (Convert.ToInt32(days) * 24 + Convert.ToInt32(hours)).ToString() + ":" + minutes;
                if (Convert.ToInt32(days) * 24 > 999)
                    txtTimerChangeWindow("txtStatus1", 2);
                else
                    txtTimerChangeWindow("txtStatus1", 3);
                changeTimetoHoursCycle = DateTime.Now;
                txtStatus1.Text = totalTimeHours;
            }
            else if (txtStatus1oldText.Contains("D"))
            {
                txtTimerChangeWindow("txtStatus1", 1);
                txtStatus1.Text = txtStatus1oldText;
            }

        }

        private void txtStatus2_click(object sender, EventArgs e)
        {

            if (txtStatus2.Text.Contains("D"))
            {
                txtStatus2oldText = txtStatus2.Text;

                string days = txtStatus2.Text.Remove(txtStatus2.Text.IndexOf("D")).Replace(" ", "");
                string time = txtStatus2.Text.Remove(0, txtStatus2.Text.IndexOf("D") + 1);
                string[] timeDif = time.Split(new char[] { ':' });
                string hours = timeDif[0].Replace(" ", "");
                string minutes = timeDif[1].Replace(" ", "");
                string totalTimeHours = (Convert.ToInt32(days) * 24 + Convert.ToInt32(hours)).ToString() + ":" + minutes;
                if (Convert.ToInt32(days) * 24 > 999)
                    txtTimerChangeWindow("txtStatus2", 2);
                else
                    txtTimerChangeWindow("txtStatus2", 3);
                changeTimetoHoursFull = DateTime.Now;
                txtStatus2.Text = totalTimeHours;

            }
            else if (txtStatus2oldText.Contains("D"))
            {
                txtTimerChangeWindow("txtStatus2", 1);
                txtStatus2.Text = txtStatus2oldText;
            }
        }



        private void lstFailuresAdd(string message)
        {
            lstFailures.Items.Add(message);
            //lstFailures.Items.Add("");
            lstFailures.SelectedIndex = lstFailures.Items.Count - 1;
            lstFailures.SelectedIndex = -1;

        }

        private void txtTimerAdd()
        {
            string tmp2 = "";
            string tmp3 = "";

            //cycle time timer constructor
            if (CycleTimeValue.TotalMinutes > Convert.ToDouble(udMinBurnIn.Value))
            {
                txtStatus1.BackColor = Color.Lime;
            }
            else
            {
                txtStatus1.BackColor = SystemColors.Control;
            }
            if (DateTime.Now.Subtract(changeTimetoHoursCycle).TotalSeconds > 10)//check waiting time if timer was clicked
            {
                if (CycleTimeValue.Days != 0)
                {
                    tmp2 = CycleTimeValue.Days + "D ";
                    txtTimerChangeWindow("txtStatus1", 1);
                }
                tmp2 += CycleTimeValue.Hours + ":" + CycleTimeValue.Minutes.ToString("00");
                txtStatus1.Text = tmp2;
                CycleTime = tmp2;
            }

            if (DateTime.Now.Subtract(changeTimetoHoursFull).TotalSeconds > 10)//check waiting time if timer was clicked
            {
                double tmpBrn;
                //check Elapsed Time im module data 
                if (BurnInFullTimeSec / 60 < StatusMod.ElapsedTime)
                    tmpBrn = StatusMod.ElapsedTime * 60;
                else
                    tmpBrn = BurnInFullTimeSec;
                double tmp1 = Math.Truncate(tmpBrn / 86400);
                if (tmp1 > 0)
                {
                    tmp3 += Convert.ToString(tmp1) + "D ";
                    tmpBrn -= tmp1 * 86400;
                    txtTimerChangeWindow("txtStatus2", 1);

                }
                tmp1 = Math.Truncate(tmpBrn / 3600);
                if (tmp1 > 0)
                {
                    tmp3 += Convert.ToString(tmp1) + ":";
                    tmpBrn -= tmp1 * 3600;
                    tmp3 += Math.Truncate(tmpBrn / 60).ToString("00");

                }
                else
                    tmp3 += "0:" + Math.Truncate(tmpBrn / 60).ToString("00");

                txtStatus2.Text = tmp3;
                FullTime = tmp3;
            }

        }

        private void txtTimerChangeWindow(string txtStatus, int key)
        {
            switch (txtStatus)
            {
                case "txtStatus1":
                    {
                        switch (key)
                        {
                            case 1://contains "D"
                                {
                                    txtStatus1.Font = new System.Drawing.Font("Microsoft Sans Serif", Convert.ToInt32(txtStatus1L[3]) - 16, FontStyle.Bold);
                                    txtStatus1.Location = new Point(txtStatus1.Location.X, Convert.ToInt32(txtStatus1L[1]) + 10);
                                }
                                break;
                            case 2: //hours more 999
                                {
                                    txtStatus1.Font = new System.Drawing.Font("Microsoft Sans Serif", Convert.ToInt32(txtStatus1L[3]) - 16, FontStyle.Bold);
                                    txtStatus1.Location = new Point(txtStatus1.Location.X, Convert.ToInt32(txtStatus1L[1]) + 10);
                                }
                                break;
                            case 3: //normal
                                {
                                    txtStatus1.Font = new System.Drawing.Font("Microsoft Sans Serif", Convert.ToInt32(txtStatus1L[3]), FontStyle.Bold);
                                    txtStatus1.Location = new Point(txtStatus1.Location.X, Convert.ToInt32(txtStatus1L[1]));
                                }
                                break;
                        }
                    }
                    break;
                case "txtStatus2":
                    {
                        switch (key)
                        {
                            case 1://contains "D"
                                {
                                    txtStatus2.Font = new System.Drawing.Font("Microsoft Sans Serif", Convert.ToInt32(txtStatus2L[3]) - 16, FontStyle.Bold);
                                    txtStatus2.Location = new Point(txtStatus2.Location.X, Convert.ToInt32(txtStatus2L[1]) + 10);
                                }
                                break;
                            case 2: //hours more 999
                                {
                                    txtStatus2.Font = new System.Drawing.Font("Microsoft Sans Serif", Convert.ToInt32(txtStatus2L[3]) - 16, FontStyle.Bold);
                                    txtStatus2.Location = new Point(txtStatus2.Location.X, Convert.ToInt32(txtStatus2L[1]) + 10);
                                }
                                break;
                            case 3: //normal
                                {
                                    txtStatus2.Font = new System.Drawing.Font("Microsoft Sans Serif", Convert.ToInt32(txtStatus2L[3]), FontStyle.Bold);
                                    txtStatus2.Location = new Point(txtStatus2.Location.X, Convert.ToInt32(txtStatus2L[1]));
                                }
                                break;
                        }
                    }
                    break;
            }
        }


        public string PublishVersion
        {
            get
            {
                if (System.Deployment.Application.ApplicationDeployment.IsNetworkDeployed)
                {
                    Version ver = System.Deployment.Application.ApplicationDeployment.CurrentDeployment.CurrentVersion;
                    return string.Format("{0}.{1}.{2}.{3}", ver.Major, ver.Minor, ver.Build, ver.Revision);
                }
                else
                    return "Not Published";
            }
        }

        private void udMinDrop_ValueChanged(object sender, EventArgs e)
        {
            PLD_DROP_MIN = Convert.ToInt32(udMinDrop.Value);
        }

        private void PD3Monitoring_ValueChanged(object sender, EventArgs e)
        {
            if (averagingPD3.Value != 0)
            {
                if (PD3Monitoring.Value < 25)
                    monitoringPD3Interval = Convert.ToInt32(averagingPD3.Value) * Convert.ToInt32(PD3Monitoring.Value);
                else
                    monitoringPD3Interval = Convert.ToInt32(PD3Monitoring.Value);
            }
            else
            {
                monitoringPD3Interval = Convert.ToInt32(PD3Monitoring.Value);
            }
        }

        private void PD3MaxTimeMonitoringCycle_ValueChanged(object sender, EventArgs e)
        {
            if (PD3MaxTimeMonitoringCycle.Value <= PD3Monitoring.Value)
            {
                PD3MaxTimeMonitoringCycle.Value = PD3Monitoring.Value + 5;
                //MessageBox.Show("VAlue of 'PD3 max time monitoring cycle' must be bigger then time of 'waiting for PD3 stabilization'");
            }
        }

        private void PD3ChangeDirect_ValueChanged(object sender, EventArgs e)
        {
            if (PD3ChangeDirect.Value <= averagingPD3.Value)
                PD3ChangeDirect.Value = averagingPD3.Value + 5;
        }
    }
}
