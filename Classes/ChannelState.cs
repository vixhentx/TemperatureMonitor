using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemperatureMonitor.Classes
{
    /*
     * ChannelState，给蓝牙和ChannelControl用
     * 
     */
    public class ChannelState
    {
        protected float _temperature=20;
        public float last_temperature=20;
        public int Battery { get; set; } /* 0~100 */
        public float Temperature
        {
            get { return _temperature; }
            set 
            {
                last_temperature = _temperature;
               _temperature = value;
            }
        }
    }
}
