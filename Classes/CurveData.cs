using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemperatureMonitor.Classes
{
    public class CurveData
    {
        protected SortedList<uint, float> data = new();
        public bool visable = true;
        public Color curveColor = Colors.Red;
        public int Count => data.Count; 

        private int LowerBound(uint d)
        {
            int l = 0, r = data.Count(), mid;
            while (l < r)
            {
                mid = (l + r) >> 1;
                if (data.Keys[mid] > d)
                {
                    r = mid;
                }
                else
                {
                    l = mid + 1;
                }
            }
            return r;
        }
        public float this[UInt32 t]
        {
            get => data[t];
            set => data[t] = value;
        }
        public float this[DateTime t]
        {
            get =>data[TimeHelper.ConvertToUnixTime(t)];
            set =>data[TimeHelper.ConvertToUnixTime(t)] = value;
        }
        public float this[uint start, uint end]
        {
            get
            {
                float sum = 0;
                int count = 0;
                for (int i = LowerBound(start); i < data.Keys.Count &&data.Keys[i] <= end; i++,count++) 
                {
                    sum += data.Values[i];
                }
                return sum / count;
            }
        }
        public float this[DateTime start, DateTime end] => this[TimeHelper.ConvertToUnixTime(start),TimeHelper.ConvertToUnixTime(end)];
    }
}
