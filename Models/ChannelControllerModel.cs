using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace TemperatureMonitor.Classes
{
    public class ChannelControllerModel
    {
        public string Name {  get; set; }
        public required CurveData Curve { get; set; }
        public ChannelState State { get; set; }
        public int Channel {  get; set; }
        public Color Color => Curve.curveColor;
        public string Battery => State.Battery.ToString() + "%";
        public string Temperature => State.Temperature.ToString() + "℃";

        static string[] bat_icontable = { "\ue631", "\ue62c", "\ue62e", "\ue632", "\ue630", "\ue62d", "\ue62f" };
        public string BatteryIcon
        {
            get
            {
                //本来想照抄win10逻辑的，找不到，只好自己写一个 （win10的神秘逻辑确实也挺误导的
                int bat = State.Battery, state = 0;
                if (bat <= 1) state = 0;
                else if (bat <= 5) state = 1;
                else if (bat <= 10) state = 2;
                else if (bat <= 20) state = 3;
                else if (bat <= 30) state = 4;
                else if (bat <= 80) state = 5;
                else state = 6;
                return bat_icontable[state];
            }
        }
        static string[] temp_icontable = { "\ue63a", "\ue639", "\ue638" };//普通，升高，降低
        public string TemperatureIcon
        {
            get
            {
                float temp = State.Temperature, ltemp = State.last_temperature;
                int state = 0;
                if (temp > ltemp) state = 1;
                else if (temp < ltemp) state = 2;

                return temp_icontable[state];
            }
        }
        public ChannelControllerModel()
        {
            Name??= string.Empty;
            State ??= new();
        }
    }
}
