using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemperatureMonitor.Classes
{
    static public class TimeHelper
    {
        static readonly DateTime unix_start = new(1970, 1, 1);
        public static UInt32 ConvertToUnixTime(DateTime time)
        {
            return (UInt32)(time - unix_start).TotalSeconds;
        }
        public static DateTime ConvertToDateTime(uint time)
        {
            return unix_start.AddSeconds(time);
        }
    }
}
