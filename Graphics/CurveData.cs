using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemperatureMonitor.Graphics
{
    public class CurveData 
    {
        List<double> prefix_sum = new List<double>();
        protected List<CurvePoint> _source = new List<CurvePoint>();
        public bool visable = true;
        public Color curveColor=Colors.Red;
        public List<CurvePoint> Source
        {
            get { return _source; }
            set
            {
                _source = value;
                prefix_sum.Clear();
                prefix_sum.Add(0);
                for (int i = 0; i < _source.Count; i++)
                {
                    prefix_sum.Add(prefix_sum[prefix_sum.Count - 1] + _source[i].tempPos);
                }
            }
        }
        public void AddPoint(CurvePoint point)
        {
            prefix_sum.Add(prefix_sum[prefix_sum.Count - 1] + point.tempPos);
            _source.Add(point);
        }
        public float GetAverage(int l_index, int r_index)
        {
            return (float)((prefix_sum[r_index + 1] - prefix_sum[l_index]) / (r_index - l_index + 1));
        }
    }
}
