using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemperatureMonitor.Classes
{
    public class CurveData
    {
        List<double> prefix_sum = new List<double>();
        protected List<CurvePoint> _source = new List<CurvePoint>();
        public bool visable = true;
        public Color curveColor = Colors.Red;
        public int Count =>_source.Count; 
        public CurvePoint LastPoint => _source[_source.Count - 1];

        public List<CurvePoint> Source
        { set { SetSource(value); } }
        public void AddPoint(CurvePoint point)
        {
            int d = (point.timePos - _source[_source.Count - 1].timePos).Seconds;
            prefix_sum.Add(prefix_sum[prefix_sum.Count - 1] + point.tempPos * d);
            _source.Add(point);
        }
        public void SetSource(List<CurvePoint> s)
        {
            _source = s;
            prefix_sum.Clear();
            prefix_sum.Add(0);
            prefix_sum.Add(_source[0].tempPos);
            int d;
            for (int i = 1; i < _source.Count; i++)
            {
                d = (_source[i].timePos - _source[i - 1].timePos).Seconds;
                prefix_sum.Add(prefix_sum[prefix_sum.Count - 1] + _source[i].tempPos * d);
            }
        }
        protected int GetIndexFromDateTime(DateTime time)
        {
            //二分跑的够快，放心（
            int l = 0, r = _source.Count, mid;
            while (l < r)
            {
                mid = (l + r) / 2;
                if (_source[mid].timePos < time)
                {
                    l = mid + 1;
                }
                else
                {
                    r = mid;
                }
            }
            return r;
        }
        protected float GetAverage(int l_index, int r_index)
        {
            l_index = Math.Clamp(l_index, 0, _source.Count - 1);
            r_index = Math.Clamp(r_index, 0, _source.Count - 1);
            return (float)((prefix_sum[r_index + 1] - prefix_sum[l_index]) / (r_index - l_index + 1));
        }
        public float this[int i]
        {
            get
            {
                return _source[i].tempPos;
            }
            set
            {
                _source[i].tempPos = value;
            }
        }
        public float this[int l, int r]
        {
            get
            {
                return GetAverage(l, r);
            }
        }
        public float this[DateTime t]
        {
            get
            {
                return _source[GetIndexFromDateTime(t)].tempPos;
            }
            set
            {
                _source[GetIndexFromDateTime(t)].tempPos = value;
            }
        }
        public float this[DateTime start, DateTime end]
        {
            get
            {
                int l = GetIndexFromDateTime(start), r = GetIndexFromDateTime(end);
                return this[l, r];
            }
        }
    }
}
