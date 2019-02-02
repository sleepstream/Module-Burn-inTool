using System;
using System.Collections.Generic;
using System.Text;

namespace Module_Burn_inTool
{
    public static class Settings
    {
        public enum PowerMeters
        {
            Primes, Filedmax2
        }

        public static bool PowerMetersCalibrate = false;
        public static PowerMeters Powermeter;
        public static int PM_WAIT_TIME=10000;
        public const int PM_AVERAGING = 5;
        public const int PM_WAIT_AVERAGING = 1000;
        public const int monitoringPD3Time = 5;
        public const int udCheckPeriod = 1;

        public static string PM_Port;
        public static string Laser_IP;
        public static string IO_IP;

        public const int FIELDMAX_ZERO_TIMEOUT = 60000;
        public const int FIELDMAX_WAIT_READY = 30000;
        public const int LASER_WAIT_PS_ON = 5000;
        public const int LASER_WAIT_EM_ON = 5000;

        public const bool MeasureAnalog = true;

        public static string Laser_SN;
        public static string Laser_PN;
        public static double Laser_PS_Voltage;
        public static int Laser_N_Current_Chains;
        public static Int32 Emission_Time;

        public static double CalValue;
        public static double DefaultTimePD3Instability = 600;
        public static double DefaultValuePD3Instability = 8;
        public static double DefaultValuePowerInstability = 1;
        public static int CoolingModuleTime = 60;
        public static double MaxTempCooling = 20;

        public static int warmingTime = 2;



    }
}
