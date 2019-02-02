using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FieldMax2DLLServer;
using System.Windows.Forms;
using System.Diagnostics;
using System.Globalization;

namespace Module_Burn_inTool
{

    //Main wrapper class for DLLServer
    //The simplicity is everything - so it doesn't support multiple FM's - instead it works with first one; otherwise messaging with error "Multiple FMs"
    public class SimpleFM
    {
        cFM2Listener cListener;
        IFM2Listener iListener;
        cFM2ScanForData ScanData;
        cFM2ScanUSBForChange ScanUSB;
        cFM2Notify Notify;
        public cFM2Devices Devices;
        string CurIndex;
        int FMCounter;
        bool PowerOn;
        bool ProbePresent;
        public Dictionary<string, string> Lastdata = new Dictionary<string, string>();
        public Dictionary<string, string> metterArray = new Dictionary<string, string>();


        public bool Zeroed { get; private set; }

        public bool Ready
        {
            get
            {
                if ((FMCounter >= 1) && PowerOn && ProbePresent)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public SimpleFM()
        {

            Zeroed = false;
            FMCounter = 0;
            PowerOn = false;
            ProbePresent = false;
            //auto connecting to dll server when class init;
            cListener = new cFM2Listener();
            iListener = (IFM2Listener)cListener;
            ScanData = new cFM2ScanForData();
            ScanUSB = new cFM2ScanUSBForChange();
            Notify = new cFM2Notify();
            Devices = new cFM2Devices();

            cListener.DeviceEvents = Notify;
            ScanUSB.CheckTimer(iListener);
            Notify.NewStatusMessage += Notify_NewStatusMessage;
            Notify.NewMessage += Notify_NewMessage;
            FMCounter = 0;
            PowerOn = false;
            ProbePresent = false;
            System.Diagnostics.Debug.Write("SimpleFM constructor!\r");
            //double pwr = Power;
        }

        void Notify_NewMessage(object sender, FMDataEventArgs e)
        {

            CurIndex = e.DeviceIndex.ToString();
            Debug.Write(CurIndex);
            IFM2Device device = (IFM2Device)Devices.Item(CurIndex);
            if (e.DataNotification == DataNotifications.MeasurementData)
            {
                Power = device.LastData;
            }
            if (e.DataNotification == DataNotifications.PowerOn)
            {
                PowerOn = true;
            }
            if (e.DataNotification == DataNotifications.PowerOff)
            {
                PowerOn = false;
            }
            if (e.DataNotification == DataNotifications.ProbeAdded)
            {
                ProbePresent = true;
            }
            if (e.DataNotification == DataNotifications.ProbeRemoved)
            {
                ProbePresent = false;
            }
            if (e.DataNotification == DataNotifications.Zeroing)
            {
                Debug.WriteLine("Zero event" + Notify.ZeroDeviceTimeoutCounter.ToString());
                if (Notify.ZeroDeviceTimeoutCounter == 0)
                {
                    Zeroed = true;
                }
                else
                {
                    Zeroed = false;
                }
            }

        }
        public string SerialNumber()
        {
            return Notify.SerialNumber;
        }


        void Notify_NewStatusMessage(object sender, FMStatusEventArgs e)
        {
            IFM2Device device;
            Dictionary<string, string> Lastdata1 = new Dictionary<string, string>();
            Devices = ((cFM2Notify)sender).Devices;
            if (e.StatusChangeNotification == StatusChangeNotifications.MeterAdded)
            {


                FMCounter = FMCounter + 1;
                device = (IFM2Device)Devices.Item(((cFM2Notify)sender).DeviceIndex.ToString());
                device.DeviceEvents = Notify;
                Lastdata.Add(device.SerialNumber, "");
                metterArray.Add(device.DeviceIndex.ToString(), device.SerialNumber);
                //MessageBox.Show(metterArray[device.DeviceIndex.ToString()]);
                if (FMCounter >= 1)
                {
                    ScanData.CheckTimer(Devices);
                }
            }
            if (e.StatusChangeNotification == StatusChangeNotifications.MeterRemoved)
            {
                metterArray.Remove(((cFM2Notify)sender).DeviceIndex.ToString());
                Lastdata.Remove(((cFM2Notify)sender).SerialNumber);
                FMCounter = FMCounter - 1;
                if (FMCounter >= 1)
                {
                    ScanData.CheckTimer(Devices);
                }

            }
        }

        public void resetError()
        {
            if (Devices != null)
            {
                if (Devices.Count != 0)
                {
                    foreach (IFM2Device device in Devices)
                    {
                        device.DismissFault();
                    }


                }
            }
        }
        public string reedError()
        {
            string err = "";
            if (Devices != null)
            {
                if (Devices.Count != 0)
                {
                    foreach (IFM2Device device in Devices)
                    {
                        err += device.LastFault.ToString() + " ";
                    }
                }
            }
            return err;
        }
        public void GetData(string deviceIndex)
        {
            IFM2Device device;
            device = (IFM2Device)Devices.Item(deviceIndex);
            device.GetUSBDeviceData(device.DeviceHandle);
            if (device.LastData > 10000)
                Lastdata[device.SerialNumber] = "-0.66";
            else
                Lastdata[device.SerialNumber] = device.LastData.ToString("###0.00");



        }

        public void Attenuation(double correction, string deviceIndex)
        {
            IFM2Device device;
            device = (IFM2Device)Devices.Item(deviceIndex);
            device.AttenuationCorrectionFactorCommand(correction);
        }


        public double Power
        {
            get;
            private set;
        }
        public void AttenuationOff(bool key, string deviceIndex)
        {
            IFM2Device device;
            device = (IFM2Device)Devices.Item(deviceIndex);
            device.AttenuationCorrectionModeEnabledCommand(key);

        }

        public void Backlight(string index)
        {
            if (Devices.Item(index).Backlight == true)
                Devices.Item(index).BacklightCommand(false);
            else
                Devices.Item(index).BacklightCommand(true);
        }

        public void Zero(string index)
        {
            Devices.Item(index).ZeroDevice();
        }

        public void Disconnect()
        {
            try
            {
                ScanUSB.StopTimer();
                ScanData.StopTimer();
                if (Devices != null)
                {
                    if (Devices.Count != 0)
                    {
                        foreach (IFM2Device device in Devices)
                        {
                            device.CloseAllUSBDeviceDrivers(device.DeviceHandle);
                        }
                    }
                }


                Notify = null;
                ScanData = null;
                ScanUSB = null;
                cListener = null;
                iListener = null;

                Debug.Write("SimpleFM disconnect!\r");

            }
            catch (Exception ex)
            { }
        }
        ~SimpleFM()
        {/*
            try
            {
               
                if (Devices != null)
                {
                    if (Devices.Count != 0)
                    {
                        foreach (IFM2Device device in Devices)
                        {
                            device.CloseAllUSBDeviceDrivers(device.DeviceHandle);
                        }
                    }
                }
                ScanUSB.StopTimer();
                ScanData.StopTimer();
                //Devices = null;
                Notify = null;
                ScanData = null;
                ScanUSB = null;
                cListener = null;
                iListener = null;


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }*/

        }


    }

    class FMStatusEventArgs : EventArgs
    {
        private StatusChangeNotifications msg;


        public FMStatusEventArgs(StatusChangeNotifications StatusChangeNotification)
        {

            msg = StatusChangeNotification;
        }

        public StatusChangeNotifications StatusChangeNotification
        {
            get
            {
                return msg;
            }
        }
    }

    class FMDataEventArgs : EventArgs
    {
        private DataNotifications msg;
        private int DIndex;
        public FMDataEventArgs(DataNotifications Datanotification, int deviceindex)
        {
            msg = Datanotification;
            DIndex = deviceindex;
        }
        public DataNotifications DataNotification
        {
            get
            {
                return msg;
            }
        }
        public int DeviceIndex
        {
            get
            {
                return DIndex;
            }
        }

    }

    public enum StatusChangeNotifications
    {
        MeterAdded, MeterRemoved
    }

    public enum DataNotifications
    {
        Fault, ProbeRemoved, ProbeAdded, PowerOn, PowerOff, MeasurementData, PacketIsOverrange, OverTemperature, MeasurementDataLost, Zeroing
    }

    class cFM2Notify : FieldMax2DLLServer.IFM2DeviceEvents
    {
        public event EventHandler<FMStatusEventArgs> NewStatusMessage;
        public event EventHandler<FMDataEventArgs> NewMessage;

        string m_CallbackEvent;
        string m_callbackMessage;
        short m_DeviceIndex;
        string m_SerialNumber;
        short m_ZeroDeviceTimeoutCounter;
        public cFM2Devices Devices;

        public string CallbackEvent
        {
            get
            {
                return m_CallbackEvent;
            }
            set
            {
                m_CallbackEvent = value;
            }
        }

        public string CallbackMessage
        {
            get
            {
                return m_callbackMessage;
            }
            set
            {
                m_callbackMessage = value;
            }
        }

        public short DeviceIndex
        {
            get
            {
                return m_DeviceIndex;
            }
            set
            {
                m_DeviceIndex = value;
            }
        }

        public void DisplayErrorToClient()
        {
            NewMessage(this, new FMDataEventArgs(DataNotifications.Fault, m_DeviceIndex));
        }

        public void DisplayZeroDeviceProgressToClient()
        {
            NewMessage(this, new FMDataEventArgs(DataNotifications.Zeroing, m_DeviceIndex));
        }

        public void NotifyData(FieldMax2DLLServer.IFM2DeviceEvents CallbackData)
        {

            NewMessage(this, new FMDataEventArgs((DataNotifications)System.Enum.Parse(typeof(DataNotifications), CallbackData.CallbackEvent, true), CallbackData.DeviceIndex));
        }

        public void NotifyDeviceStatus(FieldMax2DLLServer.IFM2DeviceEvents CallbackData, FieldMax2DLLServer.cFM2Devices DeviceList)
        {
            this.Devices = DeviceList;
            NewStatusMessage(this, new FMStatusEventArgs((StatusChangeNotifications)System.Enum.Parse(typeof(StatusChangeNotifications), CallbackData.CallbackEvent, true)));
        }

        public string SerialNumber
        {
            get
            {
                return m_SerialNumber;
            }
            set
            {
                m_SerialNumber = value;
            }
        }

        public short ZeroDeviceTimeoutCounter
        {
            set
            {

                m_ZeroDeviceTimeoutCounter = value;
            }
            get
            {
                return m_ZeroDeviceTimeoutCounter;
            }
        }
    }




}