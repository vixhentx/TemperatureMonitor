using System;
using System.Collections.Generic;
using System.Linq;
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
        public ChannelControllerModel()
        {
            Name??= string.Empty;
            State ??= new();
        }
    }
}
