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
using System.Media;
using System.Xml;
using Microsoft.Office.Interop.Excel;

namespace Module_Burn_inTool
{
    public partial class mainForm : Form
    {
        public bool Work;
        private Thread WorkThread;
        public bool PowerMesActive = false;
        public bool StopSetCurrent = true;
        //калибровка модуля
        public bool currentSet = false;

        bool btnCalibrationStatus = false;
        int PD2Max = 60;
        int PD2Min = 10;
        int PD2Nominal = 50;

        int PD3Max = 1100;
        int PD3Min = 1000;
        int PD3Nominal = 1050;
        int PD30counter = 0;

        int PD4Max = 30;
        int PD4Min = 10;
        int PD4Nominal = 20;

        int PD5Max = 30;
        int PD5Min = 10;
        int PD5Nominal = 20;

        bool stopDP2 = false;
        bool stopDP3 = false;
        bool stopDP4 = false;
        bool stopDP5 = false;

        SetCurrent SCurrent;
        struct SetCurrent
        {
           public int current;
           public bool restartBurn;
        }

        Dictionary<string, List<int>> dict;

        public int changeEvStatusLbl;

        public modData DataMod;
        modStatus StatusMod;
        modAlarms AlarmsMod;
        modSettings SettingsMod;
        public int PD3Text 
        {
            get 
            {
                if (txtPD3.Text != "")
                    return Convert.ToInt32(txtPD3.Text);
                else
                    return 0;
            }
        }
        public bool checkDiodRun=false;
        DiodTestForm form2;
        InputBox inputBox;


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
            public bool TimeOut;
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
                this.TimeOut = init;
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
        int instabilityPD3min = 0;
        int instabilityPD3max = 0;
        int startPD3Monitoring = 0;
        int PD3FireResistance = 0;
        int stopPD3Monitoring = 0;
        string startPowerMonitoring;
        string stopPowerMonitoring;

        int identValues = 0;
        int countMonitoringAveraging;
        int monitoringAveragingPD3Value;
        int counter = 1;
        bool monitoring = true;
        int lastPD3;
        DateTime StartMonitoringPD3;
        DateTime fullTimeMonitoring;
        DateTime startWaiting = new DateTime(2100,1,1);
        int monitoringPD3Interval;
        int minMonitoringPD3;

        //monitoring Power
        double maxPowerMonitoring;
        double minPowerMonitoring;

        BackgroundWorker powerMetr;
        BackgroundWorker powerMetrConnect;
        BackgroundWorker powerMetrCalibrate;
       // BackgroundWorker PD3BackgroundMonitoring;

        SimpleFM FieldMaxPM;
        string PowerMeterIndex="0";
        string PowerMeterSerialNumber;
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
        string logDirectory1;
        public string logDirectory;
        public string[] logDirectoryXML =new string[2];
        public string reportXMLFile;
        string logDirectoryIG243 =Module_Burn_inTool.Properties.Settings.Default.logDirectory + "IG243\\";
        string logDirectoryIG337 = Module_Burn_inTool.Properties.Settings.Default.logDirectory + "IG337\\";
        string errorModuleMessage = ""; //error line
        string[] txtCycleTimeL=new string[4];
        string[] txtFullTimeL = new string[4];
        string txtFullTimeoldText="";
        string txtCycleTimeoldText = "";
        string logSeparator="\t";
        string logSeparatorOld="___";
        bool WriteStability;
        public int IGFLAG;
        //int PrevAlarmPower;
        //int PrevPower;
        //int Power;
        int PLD_DROP_MIN;
        string[] IGTYPE = { "IG243-9", "IG337"};
        public decimal averagingPD3_Value
        {
            get
            {
                return averagingPD3.Value;
            }
            set
            {
                averagingPD3.Value=value;
            }

        }



        public mainForm()
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

                        DataMod.DP1 = mModule.DataMod.DP1;
                        DataMod.DP2 = mModule.DataMod.DP2;
                        DataMod.DP3 = mModule.DataMod.DP3;
                        DataMod.DP4 = mModule.DataMod.DP4;
                        DataMod.DP5 = mModule.DataMod.DP5;

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
                        AlarmsMod.TimeOut = mModule.Alarms.TimeOut;


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

                        DataMod.DP1 = rackLaser.DataMod.DP1;
                        DataMod.DP2 = rackLaser.DataMod.DP2;
                        DataMod.DP3 = rackLaser.DataMod.DP3;
                        DataMod.DP4 = rackLaser.DataMod.DP4;
                        DataMod.DP5 = rackLaser.DataMod.DP5;

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

        private void mainForm_Load(object sender, EventArgs e)
        {

           
            burnInCurrent.Items.Add("Обычный тест");
            changeEvStatusLbl = 1;
            statusLblText("Модуль отключен или проблемы в подключении", 1);
            Debug.Write("Load\r");
            changeTimetoHoursFull = changeTimetoHoursCycle = new DateTime(1970, 1, 1);
            this.Width = 620;
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

            if (!Settings.PowerMetersCalibrate)
                btnCalibratePwM.Enabled = false;


            WriteStability = false;
            this.Text += " v." + PublishVersion;
            autoStartBrn.Value = Module_Burn_inTool.Properties.Settings.Default.autoStartBrn;
            PD3Monitoring.Value = Module_Burn_inTool.Properties.Settings.Default.PD3Monitoring;
            PD3ChangeDirect.Value = Module_Burn_inTool.Properties.Settings.Default.PD3Stabilization;
            PD3MaxTimeMonitoringCycle.Value = Module_Burn_inTool.Properties.Settings.Default.PD3MaxTimeMonitoringCycle;
            averagingPD3.Value = Module_Burn_inTool.Properties.Settings.Default.averagingPD3;
            udMinDropPowerDrop.Value = Module_Burn_inTool.Properties.Settings.Default.udMinDropPowerDrop;

            txtCycleTimeL[0] = Convert.ToString(txtCycleTime.Location.X);
            txtCycleTimeL[1] = Convert.ToString(txtCycleTime.Location.Y);
            txtCycleTimeL[2] = Convert.ToString(txtCycleTime.Height);
            txtCycleTimeL[3] = Convert.ToString(txtCycleTime.Font.Size);
            txtFullTimeL[0] = Convert.ToString(txtFullTime.Location.X);
            txtFullTimeL[1] = Convert.ToString(txtFullTime.Location.Y);
            txtFullTimeL[2] = Convert.ToString(txtFullTime.Height);
            txtFullTimeL[3] = Convert.ToString(txtFullTime.Font.Size);
            mainText = this.Text;
            PLD_DROP_MIN = Convert.ToInt32(udMinDrop.Value);
            
        }
        private void mainForm_FormClosing(Object sender, FormClosingEventArgs e)
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
            Module_Burn_inTool.Properties.Settings.Default.burnInCurrent = burnInCurrent.SelectedIndex;
            Module_Burn_inTool.Properties.Settings.Default.udMinDropPowerDrop = udMinDropPowerDrop.Value;

            if (IGFLAG == 1)
                Module_Burn_inTool.Properties.Settings.Default.MinPdDropIg243 = udMinDrop.Value;
            else
                Module_Burn_inTool.Properties.Settings.Default.MinPdDropIg337 = udMinDrop.Value;
            Module_Burn_inTool.Properties.Settings.Default.Save();
            //powerMeterKill();
        }

       

        private void powerMetrConnect_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                string PWR;
                
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
                            PWR = PM.GetPower().ToString("###0.00");
                            if (PM.GetTin() > 0)
                            {
                                powerMetrConnect.ReportProgress(50, "Primes Power Monitor connected, measured power= " + PWR + " W");
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
                                //if (mesPower.InvokeRequired) mesPower.Invoke(new Action(() => mesPower.Text = "0.00"));
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
                                PowerMeterSerialNumber = FieldMaxPM.metterArray[PowerMeterIndex];
                                //FieldMaxPM.AttenuationOff(false);
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
                    if (Settings.PowerMetersCalibrate)
                    {
                        btnCalibratePwM.Enabled = true;
                        lstFailuresAdd("Calibrating power meter.....");
                        if (!powerMetrCalibrate.IsBusy)
                            powerMetrCalibrate.RunWorkerAsync();
                    }
                    else
                    {
                        if (!powerMetr.IsBusy)
                            powerMetr.RunWorkerAsync();
                    }
                }
                else
                {
                    lstFailuresAdd("No connection with power meter!");
                    txtMesPower.Text = "0.00";
                    btnCalibratePwM.Enabled = false;
                    button3.Text = "Connect power meter";
                }
            }
            else
            {
                Debug.Write("Cancel powerConnect!\r");
                lstFailuresAdd("Connection aborted");
                btnCalibratePwM.Enabled = false;
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
                powerMetrCalibrate.ReportProgress(0, "Start calibration...");
                if (!PowerMetersError &&  powerMetrCalibrate!=null && !powerMetrCalibrate.CancellationPending)
                {
                    
                        if( Settings.PowerMeters.Primes==Settings.Powermeter)
                            {
                                int i=0;
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
                        else if( Settings.PowerMeters.Filedmax2==Settings.Powermeter)
                            {

                                PrimesOffset = 0;
                                FieldMaxPM.Zero(PowerMeterIndex);
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
                                    if (FieldMaxPM.reedError().Replace(" ", "")!="0")
                                    {
                                        powerMetrCalibrate.ReportProgress(10, "!" + FieldMaxPM.reedError().Replace(" ", "") + "!");
                                        FieldMaxPM.resetError();
                                        FieldMaxPM.Zero(PowerMeterIndex);
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
                btnCalibratePwM.Text = "Stop calibrating";
            }
            lstFailuresAdd(e.UserState.ToString());
        }
        private void powerMetrCalibrate_Completed(object sender, RunWorkerCompletedEventArgs e)
        {
            btnCalibratePwM.Text = "Calibrate power metter";
            if (!e.Cancelled && powerMetrCalibrate != null)
            {
                if (!PowerMetersError)
                {
                    //check status of calibration  turn off/turn on in Settings
                    if (Settings.PowerMetersCalibrate)
                    {
                        lstFailuresAdd("Calibration turnOff");
                    }
                    else
                    {
                        lstFailuresAdd("Calibrate succesful!");
                    }
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
                                FieldMaxPM.GetData(PowerMeterIndex);
                                PwrMeas = (Convert.ToDouble(FieldMaxPM.Lastdata[PowerMeterSerialNumber]) * Convert.ToDouble(udCalCoeff.Value)).ToString("###0.00");
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
                    else if (txtMesPower.Text != "0" && matchesResult.Count < 0)
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
            txtMesPower.Text = PwrMeas;
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
                txtMesPower.Text = "0.00";
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
                btnCalibratePwM.Enabled = false;
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
            SetCurrent SCurrent = new SetCurrent();
            txtTemp.Text = DataMod.Temp.ToString("F01");
            indPower.Text = DataMod.Power.ToString("F00");
            txtPSVoltage.Text = DataMod.PSVoltage.ToString("F01");
            txtModuleID.Text = DataMod.ModuleID;






            txtUset.Text = DataMod.Uset.ToString("F00");

            txtILD1.Text = DataMod.ILD1.ToString("F02");
            txtILD2.Text = DataMod.ILD2.ToString("F02");
            txtILD3.Text = DataMod.ILD3.ToString("F02");
            txtILD4.Text = DataMod.ILD4.ToString("F02");
            txtILD5.Text = DataMod.ILD5.ToString("F02");
            txtILD6.Text = DataMod.ILD6.ToString("F02");
            if (txtPD1.Enabled)
                txtPD1.Text = DataMod.PD1.ToString("F00");
            txtPD2.Text = DataMod.PD2.ToString("F00");

            //averaging PD3 value
            if (DateTime.Now.Subtract(startAveraging).TotalSeconds <= Convert.ToDouble(averagingPD3.Value) && DataMod.PD3 > 10)
            {
                countAveraging += 1;
                averagingPD3Value += DataMod.PD3;
            }
            else if (averagingPD3.Value == 0)
            {
                txtPD3.Text = DataMod.PD3.ToString("F00");
                startAveraging = DateTime.Now;
            }
            else
            {
                //DataMod.PD3 = averagingPD3Value / countAveraging;
                if (countAveraging != 0)
                {
                    txtPD3.Text = (averagingPD3Value / countAveraging).ToString("F00");
                }
                else
                    txtPD3.Text = "0";
                countAveraging = 0;
                averagingPD3Value = 0;
                startAveraging = DateTime.Now;
            }

            //запрет прозвонки диодов на малых значениях PD3
            if (Convert.ToDouble(txtPD3.Text) < 1000)
                btnPLDTest.Enabled = false;
            else
                btnPLDTest.Enabled = true;

            txtPD4.Text = DataMod.PD4.ToString("F00");
            txtPD5.Text = DataMod.PD5.ToString("F00");

            this.Text = txtModuleID.Text + " - " + mainText;
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


            if (StatusMod.PowerSupply)// && DataMod.PSVoltage>10 && !DataMod.Error)
            {
                //lstFailures.Items.Add(StatusMod.PowerSupply);
                lblPowerSupply.BackColor = Color.Lime;
                lblPowerSupply.ForeColor = Color.Black;
                if (DateTime.Now.Subtract(startWaiting).TotalSeconds < 0)
                {
                    chbEmissionOn.Enabled = true;
                    chkQCW.Enabled = true;
                    gbPLD.Enabled = true;
                    btnLogFile.Enabled = true;

                }
                pwrMesure.Enabled = true;
                btnCalibration.Enabled = true;
                btnStartBurnIn.Enabled = true;
            }
            else// if ((DataMod.Error || DataMod.PSVoltage < 10) && !StatusMod.PowerSupply)
            {
                //lstFailures.Items.Add(StatusMod.PowerSupply);
                //if (!StatusMod.PowerSupply)// || DataMod.PSVoltage < 10)
                //{
                lblPowerSupply.BackColor = Color.Red;
                lblPowerSupply.ForeColor = Color.Black;
                //}
                /*else
                {
                    lblPowerSupply.BackColor = SystemColors.Control;
                    lblPowerSupply.ForeColor = Color.Gray;
                }*/
                btnToOnOFF(false);
                chbEmissionOn.Checked = false;
                tbIset.Value = tbIset.Minimum;
                udIset.Value = tbIset.Minimum;
                //gbPLD.Enabled = false;
                btnStartBurnIn.Enabled = false;
                btnCalibration.Enabled = false;
                pwrMesure.Enabled = false;

            }/*
                    else
                    {
                        lstFailuresAdd("серое");
                        lstFailures.Items.Add(StatusMod.PowerSupply);
                        lblPowerSupply.BackColor = SystemColors.Control;
                        lblPowerSupply.ForeColor = Color.Gray;
                        btnToOnOFF(false);
                    }*/
            if (StatusMod.Emission)
            {
                lblEmission.BackColor = Color.Orange;
                lblEmission.ForeColor = Color.Black;
                statusLblText("Внимание! Включена эмиссия!", 3);
                //gbPLD.Enabled = true;
            }
            else
            {
                lblEmission.BackColor = SystemColors.Control;
                lblEmission.ForeColor = Color.Gray;
                //impossible to start burnIn without Emission
                //gbPLD.Enabled = false;
                statusLblText("Эмиссия отключена", 0);
            }
            if (DateTime.Now.Subtract(startWaiting).TotalSeconds < 0)
            {
                if (!chbEmissionOn.Checked)
                {
                    tbIset.Enabled = false;
                    udIset.Enabled = false;
                    tbIset.Value = tbIset.Minimum;
                    udIset.Value = tbIset.Minimum;
                    btnPLDTest.Enabled = false;
                }
                else
                {
                    tbIset.Enabled = true;
                    udIset.Enabled = true;
                    //btnPLDTest.Enabled = true;
                }
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
            if (DateTime.Now.Subtract(autoStartBrnTimer).TotalSeconds > (Convert.ToDouble(autoStartBrn.Value) * 60) && StatusMod.Emission)
            {
                lstFailuresAdd("Автостарт записи стабильности");
                btnStartBurnIn_Click(this, new EventArgs());
                autoStartBrnTimer = new DateTime(2100, 1, 1);
            }
            // отслеживаем нестабильность PD3 
            if (DateTime.Now.Subtract(BurnInStart).TotalMilliseconds >= 5000 && WriteStability &&
                DateTime.Now.Subtract(BurnInStart).TotalSeconds <= Settings.DefaultTimePD3Instability)
            {
                int pd3 = DataMod.PD3;
                if ((instabilityPD3min > pd3 || instabilityPD3min == 0) && pd3 != 0)
                {
                    instabilityPD3min = pd3;
                }
                else if ((instabilityPD3max < pd3 || instabilityPD3max == 0) && pd3 != 0)
                {
                    instabilityPD3max = pd3;
                }
                if (instabilityPD3max != 0)
                {
                    double averagePd3 = Convert.ToDouble((instabilityPD3max + instabilityPD3min) / 2);
                    double instabMax = (instabilityPD3max - averagePd3) / (averagePd3 + instabilityPD3max) * 2;
                    double instabMin = (averagePd3 - instabilityPD3min) / (averagePd3 + instabilityPD3min) * 2;
                    double instab = (instabMax + instabMin) * 100;
                    if (instab > Settings.DefaultValuePD3Instability && IGFLAG == 1)
                    {
                        txtInstabPD3.BackColor = Color.Red;
                        txtInstabPD3.ForeColor = Color.Black;
                    }
                    else if (instab > Settings.DefaultValuePD3Instability / 2 && IGFLAG == 0)
                    {
                        txtInstabPD3.BackColor = Color.Red;
                        txtInstabPD3.ForeColor = Color.Black;
                    }
                    else
                    {
                        txtInstabPD3.BackColor = SystemColors.Control;
                    }
                    txtInstabPD3.Text = instab.ToString("#0.00");
                    //status.Text = instabilityPD3max + " " + instabilityPD3min;
                }

            }
            else if (!WriteStability && txtInstabPD3.Text == "" && txtInstabPower.Text == "")
            {
                txtInstabPD3.Text = "0.00";
                txtInstabPower.Text = "0.00";
            }
            //выводит таймер сразу после запуска эмиссии при вклюенной стабильности
            if (WriteStability)
                txtTimerAdd();

            if (DateTime.Now.Subtract(BurnInStart).TotalSeconds > Settings.DefaultTimePD3Instability &&
                WriteStability && currentSet)
            {
                StopSetCurrent = false;
                WorkThread = new Thread(setCurrent);
                WorkThread.IsBackground = true;
                //BeginInvoke(new System.Action(() => { udIset.Value = udIset.Maximum; }));
                switch (burnInCurrent.SelectedIndex)
                {
                    case 1:
                        SCurrent.current = 11;
                        WorkThread.Start(SCurrent);
                        break;
                    case 2:
                        SCurrent.current = 10;
                        WorkThread.Start(SCurrent);
                        break;
                }

                currentSet = false;

            }
            //если запущена стабильность контролируем просадку PD3 на случай аварийного отключения
            if (WriteStability)
            {
                // control trubles in module when PD3 drop too much
                if (DataMod.PD3 != 0 && DateTime.Now.Subtract(BurnInStart).TotalSeconds < 2)
                {
                    PD3FireResistance = DataMod.PD3;
                    startPD3Monitoring = DataMod.PD3;
                }
                if (DataMod.PD3 != 0 && Math.Round(DateTime.Now.Subtract(BurnInStart).TotalSeconds % 10) == 0 )
                {
                    PD3FireResistance = DataMod.PD3;
                    //status.Text = PD3FireResistance.ToString();

                }
                if (txtMesPower.Text != "" && DateTime.Now.Subtract(BurnInStart).TotalSeconds < 10)
                {
                    startPowerMonitoring = txtMesPower.Text;
                }
                //запись конечного значения прогонных параметров на случай отключения питания или ошибки
                if (DataMod.PD3 != 0)
                {
                    stopPD3Monitoring = DataMod.PD3;
                    stopPowerMonitoring = txtMesPower.Text;
                }
                if ((PD3FireResistance - DataMod.PD3) >= (int)udMinDropPowerDrop.Value && DataMod.PD3 !=0)  //monitoring real Power Drop value without averaging
                {
                    lblPLDFailure.ForeColor = Color.Black;
                    lblPLDFailure.BackColor = Color.Red;
                    if (chbEmissionOffwhenPowerDrop.Checked)
                    {
                        lstFailuresAdd(DateTime.Now.ToString("G", CultureInfo.GetCultureInfo("en-GB")) + " Power drop!" + " Cycle Time: " + CycleTime + " Full Time: " + FullTime);
                        lstFailuresAdd(DateTime.Now.ToString("G", CultureInfo.GetCultureInfo("en-GB")) + " Emission Off.");
                        btnStartBurnIn_Click(this, new EventArgs());
                        chbEmissionOn.Checked = false;
                        MessageBox.Show("Необходимо проверить диодную сторону и убедиться в отсутствии возгорания!!!!", "ACHTUNG!!!WARNING!!!ВНИМАНИЕ!!! " + PD3FireResistance +"-" + DataMod.PD3);
                    }

                }
                else if (DataMod.PD3 == 0 && PD30counter < 5)
                {
                    PD30counter += 1;
                }
                else if (DataMod.PD3 == 0 && PD30counter > 5)
                {
                    PD30counter = 0;
                    lblPLDFailure.ForeColor = Color.Black;
                    lblPLDFailure.BackColor = Color.Red;
                    if (chbEmissionOffwhenPowerDrop.Checked)
                    {
                        lstFailuresAdd(DateTime.Now.ToString("G", CultureInfo.GetCultureInfo("en-GB")) + " Power drop!" + " Cycle Time: " + CycleTime + " Full Time: " + FullTime);
                        lstFailuresAdd(DateTime.Now.ToString("G", CultureInfo.GetCultureInfo("en-GB")) + " Emission Off.");
                        btnStartBurnIn_Click(this, new EventArgs());
                        chbEmissionOn.Checked = false;
                        MessageBox.Show("Отсутствуют данные с датчика PD3!!!!", "ACHTUNG!!!WARNING!!!ВНИМАНИЕ!!! " + PD3FireResistance + "-" + DataMod.PD3);
                    }

                }

            }
             //если запущена стабильность и прошло время ожидания для стабилизации PD3
            if (WriteStability && DateTime.Now.Subtract(BurnInStart).TotalSeconds > Convert.ToDouble(autoStartBrn.Value) * 60)
            {
                try
                {
                    //проверка нестабильности мощности за все время прогона
                    if (Convert.ToDouble(txtMesPower.Text) > 0)
                    {
                        if (maxPowerMonitoring == 0)
                        {
                            maxPowerMonitoring = Convert.ToDouble(txtMesPower.Text);
                        }
                        if (minPowerMonitoring == 0)
                        {
                            minPowerMonitoring = Convert.ToDouble(txtMesPower.Text);
                        }
                        if (Convert.ToDouble(txtMesPower.Text) > maxPowerMonitoring)
                            maxPowerMonitoring = Convert.ToDouble(txtMesPower.Text);
                        if (Convert.ToDouble(txtMesPower.Text) < minPowerMonitoring)
                            minPowerMonitoring = Convert.ToDouble(txtMesPower.Text);
                        try
                        {
                            txtInstabPower.Text = (100 * (maxPowerMonitoring - minPowerMonitoring) / maxPowerMonitoring).ToString("#0.00");
                        }
                        catch (Exception ex)
                        { }
                    }
                    else
                        txtInstabPower.Text = "0.00";
                }
                catch (Exception ex)
                { }



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
                                //    lastPD3.ToString("F00") + "->" + minMonitoringPD3.ToString("F00") + "   Cycle Time: " + txtCycleTime.Text + "   Full Time: " + txtFullTime.Text);
                                using (StreamWriter sw = new StreamWriter(txtLogFile.Text, true))
                                {
                                    lblPLDFailure.ForeColor = Color.Black;
                                    lblPLDFailure.BackColor = Color.Red;
                                    lstFailuresAdd(DateTime.Now.ToString("G", CultureInfo.GetCultureInfo("en-GB")) + " Drop:" +
                                         lastPD3.ToString("F00") + "->" + minMonitoringPD3.ToString("F00") + "   Cycle Time: " + txtCycleTime.Text + "   Full Time: " + txtFullTime.Text);
                                    sw.WriteLine(Convert.ToInt32(BurnInFullTimeSec) + logSeparator + DateTime.Now.ToString("HH:mm:ss dd'/'MM'/'yyyy")
                                        + logSeparator + txtPD3.Text + logSeparator + txtILD1.Text + logSeparator + txtTemp.Text + logSeparator + txtMesPower.Text + logSeparator +
                                            "Drop:" + lastPD3.ToString("F00") + "->" + minMonitoringPD3.ToString("F00") + logSeparator + "Cycle Time:" + txtCycleTime.Text + logSeparator + "Full Time:" + txtFullTime.Text);
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
                    monitoring = true;
                    throw new Exception(ex.Message);
                    //lstFailuresAdd(ex.Message);

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
            if (StatusMod.Error && errorModuleMessage == "")
            {
                Debug.Write("Making error report\r");
                txtCycleTime.Text = "Error";
                txtCycleTime.BackColor = Color.Red;
                lblPLDFailure.ForeColor = Color.Black;
                lblPLDFailure.BackColor = Color.Red;
                {//forming alarms

                    if (AlarmsMod.CS1)
                    {
                        errorModuleMessage = errorModuleMessage + "CS1 Alarm;";
                    }
                    if (AlarmsMod.CS2)
                    {
                        errorModuleMessage = errorModuleMessage + "CS2 Alarm;";
                    }
                    if (AlarmsMod.CS3)
                    {
                        errorModuleMessage = errorModuleMessage + "CS3 Alarm;";
                    }
                    if (AlarmsMod.CS4)
                    {
                        errorModuleMessage = errorModuleMessage + "CS4 Alarm;";
                    }
                    if (AlarmsMod.CS5)
                    {
                        errorModuleMessage = errorModuleMessage + "CS5 Alarm;";
                    }
                    if (AlarmsMod.CS6)
                    {
                        errorModuleMessage = errorModuleMessage + "CS6 Alarm;";
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
                        errorModuleMessage = errorModuleMessage + "PD1 Alarm;";
                    }
                    if (AlarmsMod.PD2)
                    {
                        errorModuleMessage = errorModuleMessage + "PD2 Alarm;";
                    }
                    if (AlarmsMod.PD3)
                    {
                        errorModuleMessage = errorModuleMessage + "PD3 Alarm;";
                    }
                    if (AlarmsMod.PD4)
                    {
                        errorModuleMessage = errorModuleMessage + "PD4 Alarm;";
                    }
                    if (AlarmsMod.PD5)
                    {
                        errorModuleMessage = errorModuleMessage + "PD5 Alarm;";
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
                string[] errorMass = errorModuleMessage.Split(new Char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                if (errorModuleMessage != "")
                    lstError.Items.Clear();
                foreach (string error in errorMass)
                {
                    lstErrorAdd(DateTime.Now.ToString("G", CultureInfo.GetCultureInfo("en-GB")) + " " + error);
                }


                chbEmissionOn.Checked = false;

                if (WriteStability)
                {
                    tmrLog_Tick(this, new EventArgs());
                    changeEvStatusLbl = 1;
                    statusLblText("Прогон остановлен из-за ошибки!", 2);
                    btnStartBurnIn_Click(this, new EventArgs());
                }
            }
            if (AlarmsMod.TimeOut)
            {
                changeEvStatusLbl = 1;
                statusLblText("Модуль отключен или проблемы в подключении", 2);
                //lstFailuresAdd(DateTime.Now.ToString("G", CultureInfo.GetCultureInfo("en-GB")) + " " + "Модуль отключен или проблемы в подключении");
                connectBtn_Click(this, new EventArgs());
            }
        }
        
        private void tmrLog_Tick(object sender, EventArgs e)
        {
            if (WriteStability && !checkDiodRun)
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
                string line = "";
                try
                {
                    BurnInFullTimeSec = BurnInFullTime + DateTime.Now.Subtract(BurnInStart).TotalSeconds;
                    CycleTimeValue = DateTime.Now - BurnInStart;
                    if (errorModuleMessage == "")
                        line = Convert.ToInt32(BurnInFullTimeSec) + logSeparator + DateTime.Now.ToString("HH:mm:ss dd'/'MM'/'yyyy")
                            + logSeparator + DataMod.PD3 + logSeparator + txtILD1.Text + logSeparator + txtTemp.Text + logSeparator + txtMesPower.Text;

                    else
                        line = Convert.ToInt32(BurnInFullTimeSec) + logSeparator + DateTime.Now.ToString("HH:mm:ss dd'/'MM'/'yyyy")
                            + logSeparator + txtPD3.Text + logSeparator + txtILD1.Text + logSeparator + txtTemp.Text + logSeparator
                            + txtMesPower.Text + logSeparator + errorModuleMessage;
                    using (StreamWriter sw = new StreamWriter(txtLogFile.Text, true))
                    {
                        sw.WriteLine(line);
                    }
                }
                catch (Exception ex)
                {
 
                }


            }
            else if (checkDiodRun)
            {
                if(btnStartBurnIn.Text != "Start")
                btnStartBurnIn_Click(this, new EventArgs());
            }
        }


        private void tbIset_Scroll(object sender, EventArgs e)
        {
            switch (IGFLAG)
            {
                case 1:
                    udIset.Value = tbIset.Value;
                    mModule.Iset = tbIset.Value;
                    if (tbIset.Value > 0 && !WriteStability)
                    autoStartBrnTimer = DateTime.Now;
                    break;
                case 0:

                    udIset.Value = tbIset.Value;
                    rackLaser.Iset = tbIset.Value;
                    if (rackLaser.Settings.currentSetMinimum > 0 && !WriteStability)
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
                    if (udIset.Value > 0 && !WriteStability)
                    autoStartBrnTimer = DateTime.Now;
                    break;
                case 0:
                    tbIset.Value = (int)udIset.Value;
                    rackLaser.Iset = tbIset.Value;
                    if (udIset.Value >= rackLaser.Settings.currentSetMinimum && !WriteStability)
                    autoStartBrnTimer = DateTime.Now;
                    break;
            }
            
        }



        private void chbEmissionOn_CheckedChanged(object sender, EventArgs e)
        {
            if (chbEmissionOn.Checked && chbEmissionOn.Enabled)
            {
                chbEmissionOn.BackColor = Color.Orange;
                changeEvStatusLbl = 1;
                statusLblText("Внимание! Включена Эмиссия!", 3);
            }
            else
            {
                if (btnStartBurnIn.Text == "Stop")
                {
                    btnStartBurnIn_Click(this, new EventArgs());
                }
                chbEmissionOn.BackColor = Color.Transparent;
                statusLblText(" ", 0);
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
                btnCalibratePwM.Enabled = false;
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




        private void connectBtn_Click(object sender, EventArgs e)
        {
           
           
            errorModuleMessage = "";
            //string pwr = FieldMaxPM.Power.ToString();
            if (IgType.Text == "IG243-9" && !Connected)
            {
                IGFLAG = 1;
                averagingPD3.Value = 0;
                txtPD1.Enabled = false;
                txtPD1.Text = "---";
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
                averagingPD3.Value = 2;
                IGFLAG = 0;
                txtPD1.Enabled = true;
                txtPD1.Text = "";
                rackLaser = new RackLaser();
                {
                    SettingsMod.currentChangeStep = rackLaser.Settings.currentChangeStep;
                    SettingsMod.currentSetMax = rackLaser.Settings.currentSetMax;
                    SettingsMod.currentSetMinimum = rackLaser.Settings.currentSetMinimum;
                    logDirectory1 = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\Stability\\IG337\\";
                    logDirectory = logDirectoryIG337;
                    
                }

            }
            burnInCurrent.Items.Clear();
            burnInCurrent.Items.Add("Обычный тест");
            if (IGFLAG == 1)
            {
                burnInCurrent.Items.Add("24 часа");
                burnInCurrent.Items.Add("100 часов");
                burnInCurrent.Items.Add("ZLF 10 ампер");
                burnInCurrent.Items.Add("ZLF 11 ампер");
                burnInCurrent.SelectedIndex = Module_Burn_inTool.Properties.Settings.Default.burnInCurrent;
            }
            else if(IGFLAG == 0)
            {
                burnInCurrent.Items.Add("15 диодов");
                burnInCurrent.Items.Add("20 диодов");
                burnInCurrent.Items.Add("27 диодов");
                burnInCurrent.Items.Add("36 диодов");
                burnInCurrent.SelectedIndex = 0;
            }
            
            logDirectoryXML[0] = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)+"\\XML\\";
            logDirectoryXML[1] =Module_Burn_inTool.Properties.Settings.Default.logDirectory+"\\XML\\";

            switch (Connected)
            {
                case false:
                    {

                        if (cbPort.Text != "")
                        {
                            Connected = true;
                            startAveraging = DateTime.Now;
                            connectBtn.Text = "Disconnect";
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
                                    else
                                    {
                                        txtModuleID.BackColor = SystemColors.Control;
                                        txtModuleID.ForeColor = Color.Black;
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
                                    else
                                    {
                                        txtModuleID.BackColor = SystemColors.Control;
                                        txtModuleID.ForeColor = Color.Black;
                                    }
                                    break;
                            }
                            tbIset.Maximum = SettingsMod.currentSetMax;
                            udIset.Maximum = SettingsMod.currentSetMax;
                            tbIset.Minimum = SettingsMod.currentSetMinimum;
                            udIset.Minimum = SettingsMod.currentSetMinimum;
                            tbIset.LargeChange = SettingsMod.currentChangeStep;

                            string pattern2 = @"[A-Z]{1,}[0-9]*[-]*[0-9]{1,}";
                            MatchCollection matchesResult = Regex.Matches(txtModuleID.Text, pattern2);
                            /*if (matchesResult.Count > 0)
                            {
                                txtModuleID.BackColor = SystemColors.Control;
                            }
                            else
                            {
                                txtModuleID.ForeColor = Color.Black;
                                txtModuleID.BackColor = Color.Red;
                                connectBtn_Click(this, new EventArgs());
                                MessageBox.Show("Ошибка в записи серийного номера!");
                                break;
                            }*/

                            
                            autoStartBrnTimer = new DateTime(2100, 1, 1);
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
                            gbPLD.Enabled = true;
                            txtLogFile.Text = logDirectory + txtModuleID.Text + "_stability.txt";
                            changeEvStatusLbl = 1;
                            switch (IGFLAG)
                            {
                                case 1:

                                    if (mModule.comPortErr)
                                    {
                                        MessageBox.Show("Error connecting to Module");
                                        connectBtn_Click(this, new EventArgs());
                                        
                                    }
                                    else if (matchesResult.Count <= 0)
                                    {
                                        string SN = checkSN("");
                                        if (SN != null && SN != "")
                                        mModule.SetSN(SN);
                                        

                                    }
                                    else
                                    {
                                        statusLblText("Подключение выполнено!", 2);
                                    }

                                    break;
                                case 0:

                                    if (rackLaser.comPortErr)
                                    {
                                        MessageBox.Show("Error connecting to Module");
                                        connectBtn_Click(this, new EventArgs());
                                    }
                                    else if (matchesResult.Count <= 0)
                                    {
                                        string SN = checkSN("");
                                        if (SN != null && SN != "")
                                        {
                                            rackLaser.SetSN(SN);
                                        }
                                    }
                                    else
                                    {

                                        statusLblText("Подключение выполнено!", 2);
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
                        if (btnStartBurnIn.Text != "Start BurnIn")
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
                        txtCycleTime.Location = new System.Drawing.Point(Convert.ToInt32(txtCycleTimeL[0]), Convert.ToInt32(txtCycleTimeL[1]));
                        txtCycleTime.Size = new Size(txtCycleTime.Width, Convert.ToInt32(txtCycleTimeL[2]));
                        txtCycleTime.Font = new System.Drawing.Font("Microsoft Sans Serif", Convert.ToInt32(txtCycleTimeL[3]), FontStyle.Bold);
                        txtFullTime.Location = new System.Drawing.Point(Convert.ToInt32(txtFullTimeL[0]), Convert.ToInt32(txtFullTimeL[1]));
                        txtFullTime.Size = new Size(txtFullTime.Width, Convert.ToInt32(txtFullTimeL[2]));
                        txtFullTime.Font = new System.Drawing.Font("Microsoft Sans Serif", Convert.ToInt32(txtFullTimeL[3]), FontStyle.Bold);

                        connectBtn.Text = "Connect";

                        this.Text = mainText;
                        txtTemp.Text = "";
                        indPower.Text = "";
                        txtMesPower.Text = "";
                        txtCycleTime.Text = "";
                        txtFullTime.Text = "";
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
                        //gbPLD.Enabled = false;
                        btnStartBurnIn.Enabled = false;
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
                        changeEvStatusLbl = 1;
                        statusLblText("Модуль отключен или проблемы в подключении!", 1);
                    }
                    break;
            }



        }

        private string checkSN(string SN)
        {
            string pattern2 = @"[A-Z]{1,}[0-9]*[-]*[0-9]{1,}";
            MatchCollection matchesResult = Regex.Matches(SN, pattern2);
            if (matchesResult.Count <= 0)
            {
                SN = InputBoxShow("Ошибка в записи серийного номера!", "Введите серийный номер", SN);
                if (SN != "" && SN != null)
                {
                    return checkSN(SN);
                }
                else
                    return null;
            }
            else
            {
                return SN;
            }
            
        }

        private void btnCalibratePwM_Click(object sender, EventArgs e)
        {

            if (btnCalibratePwM.Text == "Stop calibrating" && powerMetrCalibrate.IsBusy)
            {
                powerMetrCalibrate.CancelAsync();
            }
            else if (StatusMod.Emission)
            {
                lstFailuresAdd("Turn Off power before calibration!");
            }
            else if (!powerMetrCalibrate.IsBusy && StatusMod.Emission)
            {
                powerMetrCalibrate.RunWorkerAsync();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (button3.Text == "Connect power meter" && cbMeasuringDevice.SelectedItem.ToString() != "NONE")
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
                if (cbMeasuringDevice.SelectedItem.ToString() != "NONE")
                powerMeterKill();
                button3.Text = "Connect power meter";
                button3.Enabled = true;
                cbMeasuringDevice_SelectedIndexChanged(this, new EventArgs());
                cbMeasuringDevice.Enabled = true;
            }
        }

        private void setCurrent(object obj)
        {
            if (obj.GetType() != typeof(SetCurrent))
            {
                //если принимаемая структура не соответствует 
                BeginInvoke(new System.Action(() => { lstFailuresAdd("Неверное задание тока!"); }));
                return; //указанной - воззвращаемся
            }
            SetCurrent SC = (SetCurrent)obj;
            double current = (double)SC.current;
            double ILD = Convert.ToDouble(txtILD1.Text);
            bool set = true;

            if (SCurrent.restartBurn )
            {
                WriteStability = false;
            }

            //BeginInvoke(new System.Action(() => { lstFailuresAdd(current+" "); }));
            if (current >= 0 && current <=12)
            {
                while (set && !StopSetCurrent && chbEmissionOn.Checked)
                {                    
                    BeginInvoke(new System.Action(() => { startPD3Monitoring = DataMod.PD3; }));
                    BeginInvoke(new System.Action(() => { PD3FireResistance = DataMod.PD3; }));
                    BeginInvoke(new System.Action(() => { lastPD3 = DataMod.PD3; }));

                    if (txtILD1.Text != "")
                    {
                        ILD = Convert.ToDouble(txtILD1.Text);
                        if (ILD > current + 0.01)
                        {
                            if (udIset.InvokeRequired)
                                BeginInvoke(new System.Action(() => { udIset.Value = udIset.Value - 1; }));
                            else
                                udIset.Value = udIset.Value - 1;
                        }
                        else if (ILD < current - 0.01)
                        {
                            if (udIset.InvokeRequired)
                            {
                                if (udIset.Value < 4095)
                                    BeginInvoke(new System.Action(() => { udIset.Value = udIset.Value + 1; }));
                                else
                                    set = false;
                            }
                            else
                            {
                                if (udIset.Value < 4095)
                                {
                                    udIset.Value = udIset.Value + 1;
                                }
                                else
                                    set = false;
                            }
                        }
                        else
                            set = false;

                    }
                    else if (!chbEmissionOn.Checked)
                        break;

                    Thread.Sleep(20);
                }
            }
            if (SCurrent.restartBurn)
            {
                StopSetCurrent = true;
                SCurrent.restartBurn = false;
                BeginInvoke(new System.Action(() => { btnStartBurnIn_Click(this, new EventArgs());  }));
            }
        }

        private void btnStartBurnIn_Click(object sender, EventArgs e)
        {
            errorModuleMessage = "";
            if (!checkDiodRun)
            {
                if (btnStartBurnIn.Text == "Start BurnIn")
                {
                    burnInCurrent.Enabled = false;
                    changeEvStatusLbl = 1;
                    statusLblText("Выполняется запуск прогона", 0);
                    txtInstabPD3.Text = "";
                    instabilityPD3max = 0;
                    instabilityPD3min = 0;
                    maxPowerMonitoring = 0;
                    minPowerMonitoring = 0;
                    startPowerMonitoring = "";
                    stopPowerMonitoring = "";
                    startPD3Monitoring = 0;
                    stopPD3Monitoring = 0;
                    PD3FireResistance = 0;

                    //выключаем эмиссию если она была включена
                    if (chbEmissionOn.Checked)
                    {
                        chbEmissionOn.Checked = false;
                    }/*
                    if(Convert.ToInt32(txtTemp.Text) > Settings.MaxTempCooling)
                        Settings.CoolingModuleTime = 60;
                    else
                        Settings.CoolingModuleTime = 20;
                    */ 
                        
                    //даем модулю остыть
                    //btnStartBurnIn.Text = "Cooling";
                    lstFailuresAdd("Охлаждение модуля......" + Settings.MaxTempCooling +" "+ Convert.ToDouble(txtTemp.Text));
                    if (Settings.MaxTempCooling > Convert.ToDouble(txtTemp.Text))
                    {
                        Settings.CoolingModuleTime = 20;
                    }
                    else
                    {
                        Settings.CoolingModuleTime = 60;
                    }
                    if (burnInCurrent.SelectedIndex > 2 && IGFLAG == 1)
                        SCurrent.restartBurn = true;
                    waitingTmr.Enabled = true;
                    startWaiting = DateTime.Now;
                    //отключаем кнопки
                    btnToOnOFF(false);
                    chbEmissionOn.BackColor = SystemColors.Control;
                    btnStartBurnIn.Enabled = false;
                }
                else if (btnStartBurnIn.Text == "0" && waitingTmr.Enabled == false &&  SCurrent.restartBurn)
                {
                    chbEmissionOn.Checked = true;
                    StopSetCurrent = false;
                    WorkThread = new Thread(setCurrent);
                    WorkThread.IsBackground = true;
                    switch (burnInCurrent.SelectedIndex)
                    {
                        case 3:
                            udIset.Value = 3400;
                            SCurrent.current = 10;
                            WorkThread.Start(SCurrent);
                            break;
                        case 4:
                            udIset.Value = 3750;
                            SCurrent.current = 11;
                            WorkThread.Start(SCurrent);
                            break;
                    }
                }
                else if (btnStartBurnIn.Text == "0" && waitingTmr.Enabled == false && !SCurrent.restartBurn && StopSetCurrent)
                {
                    btnStartBurnIn.Enabled = true;
                    btnStartBurnIn.BackColor = Color.Orange;
                    //включаем эмиссию
                    if (burnInCurrent.SelectedIndex <= 2 && IGFLAG == 1)
                    {
                        chbEmissionOn.Checked = true;
                        udIset.Value = udIset.Maximum;
                    }
                    else if(IGFLAG == 0)
                    {
                        chbEmissionOn.Checked = true;
                        udIset.Value = udIset.Maximum;
                    }

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
                    //сброс значения мониторинга падения PD3
                    startPD3Monitoring = 0;
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
                         /*
                        logDirectory = logDirectory1;
                        txtLogFile.Text = logDirectory + txtModuleID.Text + "_stability.txt";
                        if (!Directory.Exists(logDirectory))
                        {
                            Directory.CreateDirectory(logDirectory);
                        }
                        */
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
                                            string[] separated = displayString.Split('D');
                                            lstFailuresAdd(separated[0]);
                                            lstFailuresAdd("D" + separated[1]);
                                            lstFailuresAdd("");
                                        }
                                        else
                                            lstFailuresAdd(displayString);
                                    }
                                    //поддержка лог файлов из старой версии программы (новый разделитель табуляция)
                                    else if (currentLine.Contains(logSeparatorOld))
                                    {
                                        //don't turn red light on if it's too old Error
                                        Regex regex = new Regex(logSeparatorOld);
                                        string[] match = regex.Split(currentLine);
                                        if (DateTime.Now.Subtract(Convert.ToDateTime(match[1])).TotalDays <= 2)
                                        {
                                            lblPLDFailure.BackColor = Color.Red;
                                            lblPLDFailure.ForeColor = Color.Black;
                                        }
                                        string displayString = currentLine.Replace(logSeparatorOld, "   ");
                                        string time = displayString.Remove(displayString.IndexOf(" "));
                                        displayString = displayString.Remove(0, displayString.IndexOf(" ") + 3);
                                        if (displayString.Length > 80)
                                        {
                                            string[] separated = displayString.Split('D');
                                            lstFailuresAdd(separated[0]);
                                            lstFailuresAdd("D" + separated[1]);
                                            lstFailuresAdd("");
                                        }
                                        else
                                            lstFailuresAdd(displayString);
                                    }
                                    else
                                        lstFailuresAdd(currentLine);

                                }


                            }
                            if (currentLine.Contains(logSeparator))
                            {
                                Regex rgx = new Regex(logSeparator);
                                LineArray = rgx.Split(currentLine);
                                BurnInFullTime = Convert.ToInt32(LineArray[0]);
                            }
                            //поддержка лог файлов из старой версии программы (новый разделитель табуляция)
                            else if (currentLine.Contains(logSeparatorOld))
                            {
                                Regex rgx = new Regex(logSeparatorOld);
                                LineArray = rgx.Split(currentLine);
                                BurnInFullTime = Convert.ToInt32(LineArray[0]);
                            }
                            else
                            {
                                BurnInFullTime = 0;
                            }
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
                    WriteStability = true;
                    autoStartBrnTimer = new DateTime(2100, 1, 1);
                    changeEvStatusLbl = 0;
                    statusLblText("Прогон модуля активен!", 3);
                    currentSet = false;
                    if(IGFLAG == 1)
                        currentSet = true;
                }
                else
                {
                    //System.IO.File.Copy(txtLogFile.Text, logDirectory + txtModuleID.Text + "_stability.txt", false);
                    StopSetCurrent = true;
                    burnInCurrent.Enabled = true;
                    tmrLog.Enabled = false;
                    if (DataMod.PD3 != 0)
                    {
                        stopPD3Monitoring = DataMod.PD3;
                        stopPowerMonitoring = txtMesPower.Text;
                    }
                    //status.Text = startPD3Monitoring + " " + stopPD3Monitoring;
                    btnStartBurnIn.Text = "Start BurnIn";
                    WriteStability = false;
                    if (chbEmissionOn.Checked)
                    {
                        chbEmissionOn.Checked = false;
                    }
                    btnStartBurnIn.BackColor = Color.Transparent;
                    txtCycleTime.BackColor = SystemColors.Control;
                    lstFailuresAdd(DateTime.Now.ToString("G", CultureInfo.InvariantCulture) + " Burn in Stop.");
                    BurnInFullTime = 0;
                    waitingTmr.Enabled = false;
                   
                    autoStartBrnTimer = new DateTime(2100, 1, 1);
                    //включаем кнопки
                    btnToOnOFF(true);
                    startWaiting = new DateTime(2100, 1, 1);
                    xmlReport();
                }
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
            logDirectory = System.IO.Path.GetDirectoryName(txtLogFile.Text);
        }
        
        private void btnSettings_Click(object sender, EventArgs e)
        {
            switch (this.Width)
            {
                case 901:
                    {
                        this.Width = 620;
                        break;
                    }
                case 620:
                    {
                        this.Width = 901;
                        break;
                    }

            }
        }

        private void btnPldTest_Click(object sender, EventArgs e)
        {
            bool stop = false;
            if (DataMod.ModuleID.Contains("С10")
            || DataMod.ModuleID.Contains("B10")
            || DataMod.ModuleID.Contains("ZLF")
            || DataMod.ModuleID.Contains("ZVF"))
            {
                if (DataMod.PD3 < 1800)
                {
                    MessageBox.Show("Напряжение PD3  должно быть больше 1800мВ");
                    stop = true;
                }
            }
            else
            {
                if (DataMod.PD3 < 1000)
                {
                    MessageBox.Show("Напряжение PD3  должно быть больше 1000мВ");
                    stop = true;
                }
            }
             
            if (!checkDiodRun && !stop)
            {
                form2 = new DiodTestForm();
                autoStartBrnTimer = new DateTime(2100, 1, 1);
                form2.Show();
            }
        }

        private void xmlReport()
        {
            if (!Directory.Exists(logDirectoryXML[1]))
            {
                if (!Directory.Exists(logDirectoryXML[0]))
                    Directory.CreateDirectory(logDirectoryXML[0]);
                reportXMLFile = logDirectoryXML[0] + txtModuleID.Text;
            }
            else
            {
                reportXMLFile = logDirectoryXML[1] + txtModuleID.Text;
            }
            XmlTextWriter textWritter = new XmlTextWriter(reportXMLFile + ".xml", null);
            textWritter.WriteStartDocument();
            textWritter.WriteStartElement("Stability");
            textWritter.Close();

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.CreateXmlDeclaration("1.0", "utf-8", null);

            xmlDoc.Load(reportXMLFile + ".xml");

            XmlElement subRoot = xmlDoc.CreateElement("TestProgramVersion");
            subRoot.InnerText = PublishVersion;
            xmlDoc.DocumentElement.AppendChild(subRoot);

            subRoot = xmlDoc.CreateElement("UserID");
            subRoot.InnerText = Environment.UserName;
            xmlDoc.DocumentElement.AppendChild(subRoot);

            subRoot = xmlDoc.CreateElement("ComputerName");
            subRoot.InnerText = SystemInformation.ComputerName;
            xmlDoc.DocumentElement.AppendChild(subRoot);
            
            subRoot = xmlDoc.CreateElement("ModuleID");
            subRoot.InnerText = txtModuleID.Text;
            xmlDoc.DocumentElement.AppendChild(subRoot);

            subRoot = xmlDoc.CreateElement("Date");
            subRoot.InnerText = DateTime.Now.ToString();
            xmlDoc.DocumentElement.AppendChild(subRoot);
                
            subRoot = xmlDoc.CreateElement("StartPD3");
            subRoot.InnerText = startPD3Monitoring.ToString();
            xmlDoc.DocumentElement.AppendChild(subRoot);

            subRoot = xmlDoc.CreateElement("StopPD3");
            subRoot.InnerText = stopPD3Monitoring.ToString();
            xmlDoc.DocumentElement.AppendChild(subRoot);

            subRoot = xmlDoc.CreateElement("StartPower");
            subRoot.InnerText = startPowerMonitoring;
            xmlDoc.DocumentElement.AppendChild(subRoot);

            subRoot = xmlDoc.CreateElement("StopPower");
            subRoot.InnerText = stopPowerMonitoring;
            xmlDoc.DocumentElement.AppendChild(subRoot);

            subRoot = xmlDoc.CreateElement("InstabilityPD3");
            subRoot.InnerText = txtInstabPD3.Text;
            xmlDoc.DocumentElement.AppendChild(subRoot);

            subRoot = xmlDoc.CreateElement("InstabilityPower");
            subRoot.InnerText = txtInstabPower.Text;
            xmlDoc.DocumentElement.AppendChild(subRoot);

            subRoot = xmlDoc.CreateElement("BurnInTimeFull");
            subRoot.InnerText = Math.Round(BurnInFullTimeSec).ToString();
            xmlDoc.DocumentElement.AppendChild(subRoot);

            subRoot = xmlDoc.CreateElement("BurnInTimeCycle");
            subRoot.InnerText = (Convert.ToInt32(CycleTimeValue.TotalSeconds)).ToString();
            xmlDoc.DocumentElement.AppendChild(subRoot);

            subRoot = xmlDoc.CreateElement("StabilityLogFile");
            subRoot.InnerText = txtLogFile.Text;
            xmlDoc.DocumentElement.AppendChild(subRoot);


            xmlDoc.Save(reportXMLFile + ".xml");
                


        }

        private void txtCycleTime_click(object sender, EventArgs e)
        {

            if (txtCycleTime.Text.Contains("D"))
            {
                txtCycleTimeoldText = txtCycleTime.Text;

                string days = txtCycleTime.Text.Remove(txtCycleTime.Text.IndexOf("D")).Replace(" ", "");
                string time = txtCycleTime.Text.Remove(0, txtCycleTime.Text.IndexOf("D") + 1);
                string[] timeDif = time.Split(new char[] { ':' });
                string hours = timeDif[0].Replace(" ", "");
                string minutes = timeDif[1].Replace(" ", "");
                string totalTimeHours = (Convert.ToInt32(days) * 24 + Convert.ToInt32(hours)).ToString() + ":" + minutes;
                if (Convert.ToInt32(days) * 24 > 999)
                    txtTimerChangeWindow("txtCycleTime", 2);
                else
                    txtTimerChangeWindow("txtCycleTime", 3);
                changeTimetoHoursCycle = DateTime.Now;
                txtCycleTime.Text = totalTimeHours;
            }
            else if (txtCycleTimeoldText.Contains("D"))
            {
                txtTimerChangeWindow("txtCycleTime", 1);
                txtCycleTime.Text = txtCycleTimeoldText;
            }

        }

        private void txtFullTime_click(object sender, EventArgs e)
        {

            if (txtFullTime.Text.Contains("D"))
            {
                txtFullTimeoldText = txtFullTime.Text;

                string days = txtFullTime.Text.Remove(txtFullTime.Text.IndexOf("D")).Replace(" ", "");
                string time = txtFullTime.Text.Remove(0, txtFullTime.Text.IndexOf("D") + 1);
                string[] timeDif = time.Split(new char[] { ':' });
                string hours = timeDif[0].Replace(" ", "");
                string minutes = timeDif[1].Replace(" ", "");
                string totalTimeHours = (Convert.ToInt32(days) * 24 + Convert.ToInt32(hours)).ToString() + ":" + minutes;
                if (Convert.ToInt32(days) * 24 > 999)
                    txtTimerChangeWindow("txtFullTime", 2);
                else
                    txtTimerChangeWindow("txtFullTime", 3);
                changeTimetoHoursFull = DateTime.Now;
                txtFullTime.Text = totalTimeHours;

            }
            else if (txtFullTimeoldText.Contains("D"))
            {
                txtTimerChangeWindow("txtFullTime", 1);
                txtFullTime.Text = txtFullTimeoldText;
            }
        }
        
        private void lstFailuresAdd(string message)
        {
            lstFailures.Items.Add(message);
            //lstFailures.Items.Add("");
            lstFailures.SelectedIndex = lstFailures.Items.Count - 1;
            lstFailures.SelectedIndex = -1;

        }
        private void lstErrorAdd(string message)
        {
            lstError.Items.Add(message);
            lstError.SelectedIndex = lstError.Items.Count - 1;
            lstError.SelectedIndex = -1;

        }


        private void txtTimerAdd()
        {
            string tmp2 = "";
            string tmp3 = "";

            //cycle time timer constructor
            if (CycleTimeValue.TotalMinutes > Convert.ToDouble(udMinBurnIn.Value))
            {
                txtCycleTime.BackColor = Color.Lime;
            }
            else
            {
                txtCycleTime.BackColor = SystemColors.Control;
            }
            if (DateTime.Now.Subtract(changeTimetoHoursCycle).TotalSeconds > 10)//check waiting time if timer was clicked
            {
                if (CycleTimeValue.Days != 0)
                {
                    tmp2 = CycleTimeValue.Days + "D ";
                    txtTimerChangeWindow("txtCycleTime", 1);
                }
                tmp2 += CycleTimeValue.Hours + ":" + CycleTimeValue.Minutes.ToString("00");
                txtCycleTime.Text = tmp2;
                CycleTime = tmp2;
            }

            if (DateTime.Now.Subtract(changeTimetoHoursFull).TotalSeconds > 10)//check waiting time if timer was clicked
            {
                double tmpBrn;
                //check Elapsed Time im module data 
                if (BurnInFullTimeSec / 60 < StatusMod.ElapsedTime)
                    BurnInFullTimeSec = StatusMod.ElapsedTime * 60;
                
                tmpBrn = BurnInFullTimeSec;

                double tmp1 = Math.Truncate(tmpBrn / 86400);
                if (tmp1 > 0)
                {
                    tmp3 += Convert.ToString(tmp1) + "D ";
                    tmpBrn -= tmp1 * 86400;
                    txtTimerChangeWindow("txtFullTime", 1);

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

                txtFullTime.Text = tmp3;
                FullTime = tmp3;
            }

        }

        private void txtTimerChangeWindow(string txtStatus, int key)
        {
            switch (txtStatus)
            {
                case "txtCycleTime":
                    {
                        switch (key)
                        {
                            case 1://contains "D"
                                {
                                    txtCycleTime.Font = new System.Drawing.Font("Microsoft Sans Serif", Convert.ToInt32(txtCycleTimeL[3]) - 16, FontStyle.Bold);
                                    txtCycleTime.Location = new System.Drawing.Point(txtCycleTime.Location.X, Convert.ToInt32(txtCycleTimeL[1]) + 10);
                                }
                                break;
                            case 2: //hours more 999
                                {
                                    txtCycleTime.Font = new System.Drawing.Font("Microsoft Sans Serif", Convert.ToInt32(txtCycleTimeL[3]) - 16, FontStyle.Bold);
                                    txtCycleTime.Location = new System.Drawing.Point(txtCycleTime.Location.X, Convert.ToInt32(txtCycleTimeL[1]) + 10);
                                }
                                break;
                            case 3: //normal
                                {
                                    txtCycleTime.Font = new System.Drawing.Font("Microsoft Sans Serif", Convert.ToInt32(txtCycleTimeL[3]), FontStyle.Bold);
                                    txtCycleTime.Location = new System.Drawing.Point(txtCycleTime.Location.X, Convert.ToInt32(txtCycleTimeL[1]));
                                }
                                break;
                        }
                    }
                    break;
                case "txtFullTime":
                    {
                        switch (key)
                        {
                            case 1://contains "D"
                                {
                                    txtFullTime.Font = new System.Drawing.Font("Microsoft Sans Serif", Convert.ToInt32(txtFullTimeL[3]) - 16, FontStyle.Bold);
                                    txtFullTime.Location = new System.Drawing.Point(txtFullTime.Location.X, Convert.ToInt32(txtFullTimeL[1]) + 10);
                                }
                                break;
                            case 2: //hours more 999
                                {
                                    txtFullTime.Font = new System.Drawing.Font("Microsoft Sans Serif", Convert.ToInt32(txtFullTimeL[3]) - 16, FontStyle.Bold);
                                    txtFullTime.Location = new System.Drawing.Point(txtFullTime.Location.X, Convert.ToInt32(txtFullTimeL[1]) + 10);
                                }
                                break;
                            case 3: //normal
                                {
                                    txtFullTime.Font = new System.Drawing.Font("Microsoft Sans Serif", Convert.ToInt32(txtFullTimeL[3]), FontStyle.Bold);
                                    txtFullTime.Location = new System.Drawing.Point(txtFullTime.Location.X, Convert.ToInt32(txtFullTimeL[1]));
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

        

        private void waitingTmr_Tick(object sender, EventArgs e)
        {
            

            if (DateTime.Now.Subtract(startWaiting).TotalSeconds >= Settings.CoolingModuleTime)
            {
                waitingTmr.Enabled = false;
                btnStartBurnIn.Text = "0";
                btnStartBurnIn_Click(this, new EventArgs());
            }
            else
            {
                int tmp = Convert.ToInt32(Settings.CoolingModuleTime - DateTime.Now.Subtract(startWaiting).TotalSeconds);
                if (tmp < 0)
                    tmp = 0;
                btnStartBurnIn.Text = tmp.ToString("#0");
            }
        }

        public void statusLblText(string text, int key)
        {
            if (changeEvStatusLbl == 1)
            {
                
                if (text != "")
                    statusLbl.Text = text;

                switch (key)
                {
                    case 0:
                        statusLbl.BackColor = SystemColors.Control;
                        break;
                    case 1:
                        statusLbl.BackColor = Color.Red;
                        break;
                    case 2:
                        statusLbl.BackColor = Color.Lime;
                        break;
                    case 3:
                        statusLbl.BackColor = Color.Orange;
                        break;
                    default:
                        statusLbl.BackColor = SystemColors.Control;
                        break;
                }
            }
                
        }


        private void btnToOnOFF(bool position)
        {
            if (position)
            {
                btnLogFile.Enabled = true;
                chbEmissionOn.Enabled = true;
                chkQCW.Enabled = true;
                btnCalibration.Enabled = true;
                udIset.Enabled = true;
                tbIset.Enabled = true;
                //btnCalibration.Enabled = true;
                //pwrMesure.Enabled = true;
            }
            else
            {
                btnLogFile.Enabled = false;
                chbEmissionOn.Enabled = false;
                chkQCW.Enabled = false;
                udIset.Enabled = false;
                tbIset.Enabled = false;
                //btnCalibration.Enabled = false;
                //pwrMesure.Enabled = false;
            }
        }


        //преобразование файла прошивки в набор байтов
        public byte[] PrintHexBytes(string  fileName)  
        {
            string currentLine = "";
            string firstS = "";
            string SecondS = "";
            string lastS="";
            
            using (StreamReader sr = new StreamReader(fileName))
            {
                
                while (sr.Peek() >= 0)
                {

                    SecondS = sr.ReadLine().Remove(0, 1);
                    lastS = SecondS;
                    if (firstS != "")
                    {
                        string adressHex = firstS.Remove(0, 2).Remove(4);
                        int adressDec = int.Parse(adressHex, System.Globalization.NumberStyles.HexNumber);
                        int byteColFirst = 0;
                        int byteColSecond = 0;
                        int adressDecSecond = 0;

                        SecondS = SecondS.Remove(SecondS.Length - 2);

                        adressHex = SecondS.Remove(0, 2).Remove(4);
                        adressDecSecond = int.Parse(adressHex, System.Globalization.NumberStyles.HexNumber);
                        byteColSecond = int.Parse(SecondS.Remove(2), System.Globalization.NumberStyles.HexNumber);

                        firstS = firstS.Remove(firstS.Length - 2);
                        byteColFirst = int.Parse(firstS.Remove(2), System.Globalization.NumberStyles.HexNumber);

                        if (byteColFirst > 0)
                        {

                            if (byteColSecond > 0)
                            {
                                for (int i = 0; i < adressDecSecond - adressDec - byteColFirst; i++)
                                {
                                    firstS += "FF";
                                }
                                //MessageBox.Show(adressDecSecond - adressDec - byteColFirst + "");
                            }
                            firstS = firstS.Remove(0, 8);

                            byte[] bytes = new byte[firstS.Length / 2];
                            
                            for (int i = 0; i < bytes.Length; i++)
                            {
                                bytes[i] = (byte)((Hex(firstS[i * 2]) << 4) | Hex(firstS[i * 2 + 1]));
                            }
                            currentLine += System.Text.Encoding.GetEncoding(1252).GetString(bytes);
                        }
 
                    }
                    firstS = lastS;                    
                }
            }
            return System.Text.Encoding.GetEncoding(1252).GetBytes(currentLine);
        }


        public int Hex(char c)
        {
            if (c >= 'a') return 10 + c - 'a';
            if (c >= 'A') return 10 + c - 'A';
            return c - '0';
        }

        //запись прошивки в модуль
        private void setFlash()
        {
            openFileDialog1.AddExtension = true;
            openFileDialog1.DefaultExt = ".hex";
            openFileDialog1.Filter = "hex files (*.hex)|*.hex";
            openFileDialog1.FileName = "*.hex";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string flashFile = openFileDialog1.FileName; 
                byte[] flashByte = PrintHexBytes(flashFile);
                
                    mModule.setFlash(flashByte);
                
            }
            
        }


        public byte[]  PrintCountsAndBytes(String s, Encoding enc)
        {
            
            byte[] bytes = enc.GetBytes(s);

            return bytes;

        }

        private void btnCalibration_Click(object sender, EventArgs e)
        {
            lstFailures.Items.Clear();
            btnCalibrationStatus = !btnCalibrationStatus;
            if(btnCalibrationStatus)
            {
                changeEvStatusLbl = 1;
                lstFailuresAdd("Выполняется калибровка...");
                changeEvStatusLbl = 0;

                if (Convert.ToInt32(txtPD2.Text) > 20)
                {
                    lstFailuresAdd("Высокое нулевое значение датчика PD2!");
                    return;
                }
            
                if (Convert.ToInt32(txtPD3.Text) > 20)
                {
                    lstFailuresAdd("Высокое нулевое значение датчика PD3!");
                    return;
                }
            
                if (Convert.ToInt32(txtPD4.Text) > 20)
                {
                    lstFailuresAdd("Высокое нулевое значение датчика PD4!");
                    return;
                }

                if (Convert.ToInt32(txtPD5.Text) > 20)
                {
                    lstFailuresAdd("Высокое нулевое значение датчика PD5!");
                    return;
                }
            
                

                chbEmissionOn.Checked = true;
                udIset.Value = udIset.Maximum;
                Thread.Sleep(1000);
                dict = new Dictionary<string, List<int>>();
            
            
               /* mModule.Work = !mModule.Work;
                if (!mModule.Work)
                {*/
                    WorkThread = new Thread(checkPDValue);
                    WorkThread.IsBackground = true;
                    Work = true;
                    WorkThread.Start();
            
               
            
                /*}
                else
                {
                    mModule.startWork();
                    chbEmissionOn.Checked = false;
                }*/
                btnCalibrationStatus = !btnCalibrationStatus;
            }
            else
            {
                Work = false;
                //chbEmissionOn.Checked = false;
            }

        }

        private void powerMesarment()
        {
            string WriteFile;
            bool fileEx = false;
            string txtReportFile;
            int add = 1;
            SetCurrent SCurrent = new SetCurrent();
            Microsoft.Office.Interop.Excel.Workbook wb;
            Microsoft.Office.Interop.Excel.Application app;
            string tmplogDirectory = Module_Burn_inTool.Properties.Settings.Default.logDirectory + "\\Power\\";
            string tmplogDirectory1 = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\Stability\\Power\\";

            if (!Directory.Exists(tmplogDirectory))
            {
                tmplogDirectory = tmplogDirectory1;

                if (!Directory.Exists(tmplogDirectory))
                {
                    Directory.CreateDirectory(tmplogDirectory);

                }
                fileEx = true;
            }
            else
                fileEx = true;
            
            if (fileEx)
            {
                    txtReportFile = tmplogDirectory + DataMod.ModuleID + " powerMesarment.xlsx";
                    WriteFile = txtReportFile;
                    Object FileName = WriteFile;
                    Object Missing = Type.Missing;
                    Microsoft.Office.Interop.Excel.Workbook xlWbSource;
                    app = new Microsoft.Office.Interop.Excel.Application();
                    
                    Microsoft.Office.Interop.Excel.Range excelcells;

                    if (!File.Exists(WriteFile))
                    {
                        File.Copy("templatePower.xlsx", WriteFile, true);
                        wb = app.Workbooks.Open(WriteFile, Missing, Missing, Missing,
                           Missing, Missing, Missing, Missing, Missing, Missing, Missing, Missing, Missing, Missing, Missing);
                    }
                    else
                    {
                        File.Copy("templatePower.xlsx", tmplogDirectory + "templatePower.xlsx", true);
                        wb = app.Workbooks.Open(WriteFile, Missing, Missing, Missing,
                            Missing, Missing, Missing, Missing, Missing, Missing, Missing, Missing, Missing, Missing, Missing);
                        //вставляем новый лист из шаблона в существующий файл протокола
                        xlWbSource = app.Workbooks.Open(tmplogDirectory + "templatePower.xlsx");
                        (xlWbSource.Worksheets[1] as Microsoft.Office.Interop.Excel.Worksheet).Copy(Before: wb.Worksheets[1]);
                        xlWbSource.Close();
                        wb.Save();
                    }

                    //имя листа текущее время и дата
                    (wb.Worksheets[1] as Microsoft.Office.Interop.Excel.Worksheet).Name = DateTime.Now.ToString("HH.mm.ss dd'.'MM'.'yyyy");
                    //date
                    ((Microsoft.Office.Interop.Excel.Worksheet)wb.Worksheets[1]).Cells.Replace("[DATE]", DateTime.Now.ToShortDateString(), XlLookAt.xlWhole, XlSearchOrder.xlByRows, false, false, true, false);
                    //SN
                    ((Microsoft.Office.Interop.Excel.Worksheet)wb.Worksheets[1]).Cells.Replace("[SNUMBER]", DataMod.ModuleID, XlLookAt.xlWhole, XlSearchOrder.xlByRows, false, false, true, false);
                    //operator
                    ((Microsoft.Office.Interop.Excel.Worksheet)wb.Worksheets[1]).Cells.Replace("[OPERATOR]", Environment.UserName, XlLookAt.xlWhole, XlSearchOrder.xlByRows, false, false, true, false);



                    double step = Convert.ToDouble(SettingsMod.currentSetMax) / 100;

                    BeginInvoke(new System.Action(() => { warmUpTimePower.Enabled = false; }));

                    BeginInvoke(new System.Action(() => { progressBar2.Value = 0; }));
                    BeginInvoke(new System.Action(() => { progressBar2.Visible = true; }));
                   
                    int maxCurrent = 12;
                    int currentStep= 2;
                    switch (IGFLAG)
                    {
                        case 1:
                            maxCurrent = 12;
                            currentStep = 2;
                            break;
                        case 0:
                            maxCurrent = 100;
                            currentStep = 20;
                            break;
                    }

                    switch (burnInCurrent.SelectedIndex)
                    {
                        case 3:
                            maxCurrent = 10;
                            break;
                        case 4:
                            maxCurrent = 11;
                            break;
                    }

                    if (!chbEmissionOn.Checked)
                    {
                        BeginInvoke(new System.Action(() => { chbEmissionOn.Checked = true; }));
                        Thread.Sleep(500);
                    }

                    
        
                    BeginInvoke(new System.Action(() => { progressBar2.Maximum = maxCurrent; }));
                    for (int i = currentStep; i <= maxCurrent; )
                    {
                        if (PowerMesActive)
                        {
                            //BeginInvoke(new System.Action(() => { udIset.Value = Convert.ToInt32(i * step); }));
                            if (!chbEmissionOn.Checked)
                            {
                                //BeginInvoke(new System.Action(() => { chbEmissionOn.Checked = true; }));
                                BeginInvoke(new System.Action(() => { lstFailuresAdd("Эмиссия отключена!"); }));
                                BeginInvoke(new System.Action(() => { pwrMesure_Click(this, new EventArgs()); }));
                                //break;
                            }
                            switch (IGFLAG)
                            {
                                case 1:
                                    if (i == 12)
                                    {
                                        BeginInvoke(new System.Action(() => { udIset.Value = udIset.Maximum; }));
                                    }
                                    else
                                    {
                                        SCurrent.current = i;
                                        setCurrent(SCurrent);
                                    }
                                    break;
                                case 0:
                                    BeginInvoke(new System.Action(() => { udIset.Value = i; }));
                                    break;
                            }
                            
                            
                            int j = 0;
                            BeginInvoke(new System.Action(() => { progressBar1.Value = 0; }));
                            BeginInvoke(new System.Action(() => { progressBar1.Visible = true; }));
                            BeginInvoke(new System.Action(() => { progressBar1.Maximum = Convert.ToInt32(warmUpTimePower.Value); }));


                            BeginInvoke(new System.Action(() => {progressBar1.Maximum = Convert.ToInt32(warmUpTimePower.Value);}));
                            while (warmUpTimePower.Value - j >= 0 && PowerMesActive)
                            {
                                Thread.Sleep(1000);
                                if (progressBar1.Value != progressBar1.Maximum)
                                    BeginInvoke(new System.Action(() => { progressBar1.Value += 1; }));

                                j += 1;
                                if (!PowerMesActive || !chbEmissionOn.Checked)
                                    break;
                            }

                            if (PowerMesActive)
                            {
                                //fill digital
                                ((Microsoft.Office.Interop.Excel.Worksheet)wb.Worksheets[1]).Cells.Replace("[D" + (add).ToString("F00") + "Percent]", udIset.Value, XlLookAt.xlWhole, XlSearchOrder.xlByRows, false, false, true, false);
                                ((Microsoft.Office.Interop.Excel.Worksheet)wb.Worksheets[1]).Cells.Replace("[D" + (add).ToString("F00") + "I1]", txtILD1.Text, XlLookAt.xlWhole, XlSearchOrder.xlByRows, false, false, true, false);
                                ((Microsoft.Office.Interop.Excel.Worksheet)wb.Worksheets[1]).Cells.Replace("[D" + (add).ToString("F00") + "Power]", txtMesPower.Text, XlLookAt.xlWhole, XlSearchOrder.xlByRows, false, false, true, false);
                                ((Microsoft.Office.Interop.Excel.Worksheet)wb.Worksheets[1]).Cells.Replace("[D" + (add).ToString("F00") + "Temp]", txtTemp.Text, XlLookAt.xlWhole, XlSearchOrder.xlByRows, false, false, true, false);
                                add += 1;
                            }
                            else
                                break;
                            i += currentStep;
                            BeginInvoke(new System.Action(() => { progressBar2.Value += currentStep; }));
                        }
                        else
                            break;
                    }
                    BeginInvoke(new System.Action(() => { progressBar2.Value = 0; }));
                    BeginInvoke(new System.Action(() => { progressBar1.Value = 0; }));
                    BeginInvoke(new System.Action(() => { warmUpTimePower.Enabled = true; }));
                    for (int i = add; i <= 10; i++)
                    {
                        add = i;
                        ((Microsoft.Office.Interop.Excel.Worksheet)wb.Worksheets[1]).Cells.Replace("[D" + (add).ToString("F00") + "Percent]", "", XlLookAt.xlWhole, XlSearchOrder.xlByRows, false, false, true, false);
                        ((Microsoft.Office.Interop.Excel.Worksheet)wb.Worksheets[1]).Cells.Replace("[D" + (add).ToString("F00") + "I1]", "", XlLookAt.xlWhole, XlSearchOrder.xlByRows, false, false, true, false);
                        ((Microsoft.Office.Interop.Excel.Worksheet)wb.Worksheets[1]).Cells.Replace("[D" + (add).ToString("F00") + "Power]", "", XlLookAt.xlWhole, XlSearchOrder.xlByRows, false, false, true, false);
                        ((Microsoft.Office.Interop.Excel.Worksheet)wb.Worksheets[1]).Cells.Replace("[D" + (add).ToString("F00") + "Temp]", "", XlLookAt.xlWhole, XlSearchOrder.xlByRows, false, false, true, false);
                    }
                    if (PowerMesActive)
                    {
                        app.ActiveWorkbook.Save();
                        app.Visible = true;
                        BeginInvoke(new System.Action(() => { pwrMesure_Click(this, new EventArgs()); }));
                    }
                    else
                    {
                        app.Visible = true;
                        app.Quit();
                    }


            }
            if (chbEmissionOn.Checked)
            {
                BeginInvoke(new System.Action(() => { chbEmissionOn.Checked = false; }));
            }
            
        }

        private void checkPDValue()
        {
            if(IGFLAG == 1)
                mModule.getDPn();
            int j = 0;
            int oldaveragingPD3=0;
            BeginInvoke(new System.Action(() => { lstFailuresAdd("Прогрев " + warmUpTimePower.Value + " секунд"); }));
            while (warmUpTimePower.Value - j >= 0)
            {
                BeginInvoke(new System.Action(() => { progressBar1.Maximum = Convert.ToInt32(warmUpTimePower.Value); }));
                Thread.Sleep(1000);
                if (progressBar1.Value != progressBar1.Maximum)
                    BeginInvoke(new System.Action(() => { progressBar1.Value += 1; }));

                j += 1;
            }
            BeginInvoke(new System.Action(() => { progressBar1.Value =0; }));
            BeginInvoke(new System.Action(() => { progressBar1.Maximum = 4; }));
            //  !!!!!  Установить задержку перед калибровкой для стабилизации датчиков    !!!!!
            //BeginInvoke(new System.Action(() => { lstFailuresAdd(DataMod.DP2 + " " + DataMod.DP3 + " " + DataMod.DP4); }));
            int maxDPvalue = 99;

            //0стартовое значение DP  1текущеее напряжение PD, 2прошлое значение DP,  3 имзенять или нет,  4 прошлое значение PD, 5 необходимое значение PD, 6допуск, 7 ошибка или нет 
            List<int> DPn = new List<int> { DataMod.DP2, Convert.ToInt32(txtPD2.Text), 0, 0, Convert.ToInt32(txtPD2.Text), PD2Nominal, 1, 0, PD2Max, PD2Min };
            dict.Add("DP2", DPn);

            DPn = new List<int> { DataMod.DP3, Convert.ToInt32(txtPD3.Text), 0, 0, Convert.ToInt32(txtPD3.Text), PD3Nominal, 1, 0, PD3Max, PD3Min };
            dict.Add("DP3", DPn);

            DPn = new List<int> { DataMod.DP4, Convert.ToInt32(txtPD4.Text), 0, 0, Convert.ToInt32(txtPD4.Text), PD4Nominal, 1, 0, PD4Max, PD4Min };
            dict.Add("DP4", DPn);

            if (IGFLAG == 1)
            {
                DPn = new List<int> { DataMod.DP5, Convert.ToInt32(txtPD5.Text), 0, 0, Convert.ToInt32(txtPD5.Text), PD5Nominal, 1, 0, PD5Max, PD5Min };
                dict.Add("DP5", DPn);
            }
            else
            {
                stopDP5 = true;
                maxDPvalue = 127;
                oldaveragingPD3=(int)averagingPD3.Value;
                averagingPD3.Value = 0;
            }

            bool stopWrite = false;
            j = 0;
            int interval = 1;
            while (Work)
            {
                if (StatusMod.Emission)
                {
                    string query = "";
                    //chbEmissionOn.Checked && udIset.Value == udIset.Maximum
                    if (chbEmissionOn.Checked && udIset.Value == udIset.Maximum)
                    {
                        
                        foreach (string key in dict.Keys)
                        {
                            
                            List<int> tmp = dict[key];
                            
                            // если значение еще не выставленно проверяем и  меняем.
                            if (dict[key][3] == 0)
                            {
                                //считаем величину полуинтервала
                                BeginInvoke(new System.Action(() => { lstFailuresAdd(DataMod.DP2 + " " + DataMod.DP3 + " " + DataMod.DP4 + " " + DataMod.DP5); }));

                                BeginInvoke(new System.Action(() => { lstFailuresAdd(key + " " + tmp[4] + " " + dict[key][2] + " " + tmp[1] + " - " + dict[key][0]); }));
                                Thread.Sleep(50);
                                if (Math.Abs(tmp[4] - tmp[1]) < 3 &&  tmp[2] != 0)
                                    dict[key][6] += 1;
                                else
                                    dict[key][6] = 1;
                                interval = dict[key][6];
                                //MessageBox.Show(tmp[0].ToString());
                                //проверка на попадание в границы значения датчика

                                if ((tmp[1] > tmp[5] && tmp[4] < tmp[5]) || (tmp[1] < tmp[5] && tmp[4] > tmp[5]) && (tmp[0] < 70 && tmp[1] > 10))
                                {
                                    BeginInvoke(new System.Action(() => { lstFailuresAdd(key + " " + tmp[5] + " - " + tmp[1] + " " + tmp[5] + " - " + tmp[4]); }));
                                    if (Math.Abs(tmp[5] - tmp[1]) > Math.Abs(tmp[5] - tmp[4]))
                                    {

                                        dict[key][3] = 1;
                                        dict[key][7] = 0;
                                        tmp[0] = dict[key][2];
                                        BeginInvoke(new System.Action(() => { lstFailuresAdd(key + " больше " + tmp[0] + " DP " + tmp[0]); }));
                                    }
                                    else
                                    {

                                        dict[key][3] = 1;
                                        dict[key][7] = 0;
                                        tmp[0] = dict[key][0];
                                        BeginInvoke(new System.Action(() => { lstFailuresAdd(key + " иначе " + dict[key][0] + " DP " + tmp[0]); }));
                                    }
                                }
                                else
                                {
                                    if (tmp[1] > tmp[5] && tmp[0] > 0)
                                    {
                                        tmp[0] -= interval;
                                    }
                                    else if (tmp[1] < tmp[5] && tmp[0] < maxDPvalue)
                                    {
                                        tmp[0] += interval;
                                    }
                                    else
                                    {
                                        dict[key][3] = 1;
                                        dict[key][7] = 0;
                                        tmp[0] = dict[key][0];
                                    }
                                }
                                /*
                                if (IGFLAG != 1 && tmp[0] == maxDPvalue && )
                                {
                                    dict[key][3] = 1;
                                }*/
                                if (tmp[0] > 70 && tmp[1] < 10)
                                {
                                    if (IGFLAG == 1)
                                    {
                                        dict[key][3] = 1;
                                        dict[key][7] = 1;
                                        tmp[0] = 1;
                                    }
                                    else
                                    {
                                        //dict[key][7] = 1;
                                        tmp[0] = maxDPvalue;
                                    }
                                }
                                if (tmp[0] == 0)
                                {
                                    tmp[0] = 1;
                                    dict[key][3] = 1;
                                }
                               BeginInvoke(new System.Action(() => { lstFailuresAdd(dict[key][3] + " " + key); }));

                                switch (key)
                                {
                                    case "DP2":
                                        stopDP2 = Convert.ToBoolean(dict[key][3]);
                                        break;
                                    case "DP3":
                                        stopDP3 = Convert.ToBoolean(dict[key][3]);
                                        break;
                                    case "DP4":
                                        stopDP4 = Convert.ToBoolean(dict[key][3]);
                                        break;
                                    case "DP5":
                                        stopDP5 = Convert.ToBoolean(dict[key][3]);
                                        break;

                                }
                                //dict[key][2] = dict[key][0];
                                dict[key][0] = tmp[0];

                                if (dict[key][3] == 1)
                                {
                                    BeginInvoke(new System.Action(() => { progressBar1.Value +=1; }));
                                }

                                
                            }

                            query += tmp[0] + ";";
                            

                        }
                        BeginInvoke(new System.Action(() => { lstFailuresAdd(query); }));
                        BeginInvoke(new System.Action(() => { lstFailuresAdd(DataMod.DP2 + " " + DataMod.DP3 + " " + DataMod.DP4 + " " + DataMod.DP5); }));
                        if (!stopWrite)
                        {
                            //записываем значения PD до изменения усиления
                            dict["DP2"][4] = Convert.ToInt32(txtPD2.Text);
                            dict["DP3"][4] = Convert.ToInt32(txtPD3.Text);
                            dict["DP4"][4] = Convert.ToInt32(txtPD4.Text);

                            dict["DP2"][2] = DataMod.DP2;
                            dict["DP3"][2] = DataMod.DP3;
                            dict["DP4"][2] = DataMod.DP4;
                            if (IGFLAG == 1)
                            {
                                dict["DP5"][4] = Convert.ToInt32(txtPD5.Text);
                                dict["DP5"][2] = DataMod.DP5;
                            }
                            switch (IGFLAG)
                            {
                                case 1:
                                    mModule.setDPn(query);
                                    Thread.Sleep(100);
                                    mModule.getDPn();
                                    break;
                                case 0:
                                    rackLaser.setDPn("DP2", dict["DP2"][0]);
                                    rackLaser.setDPn("DP3", dict["DP3"][0]);
                                    rackLaser.setDPn("DP4", dict["DP4"][0]);
                                    break;
                            }
                            /*if (!chbEmissionOn.Checked)
                            {
                                BeginInvoke(new System.Action(() => { chbEmissionOn.Checked = true; }));
                                Thread.Sleep(200);
                            }
                            */
                            Thread.Sleep(500);
                            if (DataMod.DP2 == 0 || DataMod.DP3 == 0 || DataMod.DP4 == 0 || DataMod.DP5 == 0)
                            {
                                Thread.Sleep(500);
                            }
                            DPn = new List<int> { DataMod.DP2, Convert.ToInt32(txtPD2.Text), dict["DP2"][2], dict["DP2"][3], dict["DP2"][1], dict["DP2"][5], dict["DP2"][6], dict["DP2"][7], dict["DP2"][8], dict["DP2"][9] };
                            dict["DP2"] = DPn;

                            DPn = new List<int> { DataMod.DP3, Convert.ToInt32(txtPD3.Text), dict["DP3"][2], dict["DP3"][3], dict["DP3"][1], dict["DP3"][5], dict["DP3"][6], dict["DP3"][7], dict["DP3"][8], dict["DP3"][9] };
                            dict["DP3"] = DPn;

                            DPn = new List<int> { DataMod.DP4, Convert.ToInt32(txtPD4.Text), dict["DP4"][2], dict["DP4"][3], dict["DP4"][1], dict["DP4"][5], dict["DP4"][6], dict["DP4"][7], dict["DP4"][8], dict["DP4"][9] };
                            dict["DP4"] = DPn;

                            if (IGFLAG == 1)
                            {
                                DPn = new List<int> { DataMod.DP5, Convert.ToInt32(txtPD5.Text), dict["DP5"][2], dict["DP5"][3], dict["DP5"][1], dict["DP5"][5], dict["DP5"][6], dict["DP5"][7], dict["DP5"][8], dict["DP5"][9] };
                                dict["DP5"] = DPn;
                            }
                            //checkPDValue();
                        }
                        else
                        {
                            foreach (string key in dict.Keys)
                            {
                                if (dict[key][7] != 0)
                                {
                                    BeginInvoke(new System.Action(() => { lstFailuresAdd("Ошибка калбибровки датчика " + key + ": низкий уровень сигнала"); }));
                                }
                            }
                            if (txtMesPower.Text != "" && dict["DP3"][7] == 0)
                            {
                                BeginInvoke(new System.Action(() => { lstFailuresAdd("Выполняется калибровка мощности!"); }));
                                double power = Math.Round(Convert.ToDouble(txtMesPower.Text), 0);
                                if (power > 1)
                                {
                                    switch (IGFLAG)
                                    {
                                        case 1:
                                            //BeginInvoke(new System.Action(() => { lstFailuresAdd("Калибровка мощности: " +power); }));
                                            mModule.setPower(power);
                                            break;
                                        case 0:
                                            if(!rackLaser.PD3Calibr(1, ""))
                                                BeginInvoke(new System.Action(() => { lstFailuresAdd("Ошибка калибровки датчика PD3"); }));
                                            rackLaser.PD3Calibr(2, Convert.ToString(power));
                                            break;
                                    }
                                }

                            }
                            else
                            {
                                string power = InputBoxShow("Введите мощность:", "Ручная калибровка мощности", "");
                                if (power != "" && power != null)
                                {
                                    string pattern2 = @"[0-9.,]+";
                                    MatchCollection matchesResult = Regex.Matches(power, pattern2);
                                    if (matchesResult.Count > 0)
                                    {
                                        switch (IGFLAG)
                                        {
                                            case 1:
                                                BeginInvoke(new System.Action(() => { lstFailuresAdd("Калибровка мощности: " + power); }));
                                                mModule.setPower(Math.Round(Convert.ToDouble(power), 0));
                                                break;
                                            case 0:
                                                if (!rackLaser.PD3Calibr(1, ""))
                                                    BeginInvoke(new System.Action(() => { lstFailuresAdd("Ошибка калибровки датчика PD3"); }));
                                                rackLaser.PD3Calibr(2, power);
                                                break;
                                        }

                                    }
                                }
                            }

                            
                            Thread.Sleep(100);
                            //changeEvStatusLbl = 1;
                            BeginInvoke(new System.Action(() => { lstFailuresAdd("Калибровка выолнена"); }));
                            BeginInvoke(new System.Action(() => { lstFailuresAdd("Установленные усиления датчиков:"); }));
                            foreach (string key in dict.Keys)
                            {
                                BeginInvoke(new System.Action(() => { lstFailuresAdd(key + " - " + dict[key][0]); }));
                            }
                            //changeEvStatusLbl = 0;
                            Work = false;
                        }
                        //BeginInvoke(new System.Action(() => { lstFailuresAdd(stopDP2 + " " + stopDP3 + " " + stopDP4 + " " + stopDP5); }));
                        if (stopDP2 && stopDP3 && stopDP4 && stopDP5)
                        {
                            stopWrite = true;
                           // BeginInvoke(new System.Action(() => { lstFailuresAdd("stop calibr"); }));
                        }
                    }
                }
                else
                {
                    Work = false;
                    BeginInvoke(new System.Action(() => { lstFailuresAdd("Ошибка включения эмиссии!"); }));
                }
            }
            if (oldaveragingPD3 > 0)
                averagingPD3.Value= (decimal)oldaveragingPD3;

            BeginInvoke(new System.Action(() => { chbEmissionOn.Checked = false; }));
            BeginInvoke(new System.Action(() => { progressBar1.Value = 0; }));
        }

        //окно ввода значения
        private string InputBoxShow(string text, string label, string inputValue)
        {
            inputBox = new InputBox();
            inputBox.Text = text;
            inputBox.label1.Text = label;
            inputBox.inputValue.Text = inputValue;
            if (inputBox.ShowDialog() == DialogResult.OK)
            {
                return inputBox.inputValue.Text.ToString();
            }
            else
                return null;
        }

        //изменение серийного номера 
        private void ModuleId_Click(object sender, EventArgs e)
        {
            string SN = InputBoxShow("Серийный номер", "Измените серийный номер", txtModuleID.Text);
            
            if (SN != null)
            {
                SN = checkSN(SN);
                if (SN != "" && SN != null)
                {
                    switch (IGFLAG)
                    {
                        case 1:
                            mModule.SetSN(SN);
                            break;
                        case 0:
                            rackLaser.SetSN(SN);
                            break;
                    }
                }
 
            }
                
        }


        private void ModuleFimware_Click(object sender, EventArgs e)
        {
            if (Connected)
            {
                switch (IGFLAG)
                {
                    case 1:
                        setFlash();
                        break;
                    case 0:
                        break;
                }
                
            }
            else
                MessageBox.Show("Нет подключения!");
            
        }

        private void pwrMesure_Click(object sender, EventArgs e)
        {
            lstFailures.Items.Clear();
            PowerMesActive = !PowerMesActive;
            StopSetCurrent = !StopSetCurrent;
            if (PowerMesActive)
            {
                if (System.IO.File.Exists("templatePower.xlsx") == false)
                {
                    MessageBox.Show("No template found. Creating new template file...");
                    System.IO.StreamWriter OutStream;
                    System.IO.BinaryWriter BinStream;

                    OutStream = new System.IO.StreamWriter("templatePower.xlsx", false);
                    BinStream = new System.IO.BinaryWriter(OutStream.BaseStream);

                    BinStream.Write(Module_Burn_inTool.Properties.Resources.templatePower);
                    BinStream.Close();
                }
                pwrMesure.Text = "Остановить";
                WorkThread = new Thread(powerMesarment);
                WorkThread.IsBackground = true;
                WorkThread.Start();
                
            }
            else
            {
                pwrMesure.Text = "Измерение мощности";
 
            }

        }

        private void burnInCurrent_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (IGFLAG == 0)
            {
                switch (burnInCurrent.SelectedIndex)
                {
                    case 1:
                        udMinDropPowerDrop.Value = 60;
                        break;
                    case 2:
                        udMinDropPowerDrop.Value = 45;
                        break;
                    case 3:
                        udMinDropPowerDrop.Value = 30;
                        break;
                    case 4:
                        udMinDropPowerDrop.Value = 20;
                        break;
                }
            }
        }

        private void btnReseterror_Click(object sender, EventArgs e)
        {
            
            switch (IGFLAG)
            {
                case 1:
                    mModule.resetError = true;
                    break;
                case 0:
                    rackLaser.resetError = true;
                    break;
            }
        }

        private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            //
        }
    }
}
