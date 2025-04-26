using System.Numerics;
using System.Threading.Channels;
using TemperatureMonitor.Classes;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TemperatureMonitor
{
    public partial class MainPage : ContentPage
    {

        public MainPage()
        {
            InitializeComponent();


            vPanel.Target = curveView;
            hPanel.Target = curveView;

            ch1ControlView.Source.curveColor = Colors.Red;
            ch1ControlView.State.Battery = 40;
            ch1ControlView.State.Temperature = 20f;
        }

        private void OnTick()
        {

        }
        private void AddData(CurvePoint pointData,int channel)
        {
            CurveDrawable drawable = (CurveDrawable)curveView.Drawable;
            drawable.data[channel].AddPoint(pointData);
        }
        private void SetDatas(List<CurvePoint> initialPointData,int channel)
        {
            CurveDrawable drawable = (CurveDrawable)curveView.Drawable;
            drawable.data[channel].SetSource(initialPointData);
        }

        private void test()
        {
            Random rand=new Random();
            CurveDrawable drawable = (CurveDrawable)curveView.Drawable;
            if (drawable == null) return;
            List<CurvePoint> points = new List<CurvePoint>();
            CurvePoint p;
            DateTime dt= DateTime.Now.AddSeconds(-20);
            for (int i = 0; i < 20; i++)
            {
                    p = new CurvePoint
                    {
                        timePos = dt.AddSeconds(i),
                        tempPos = rand.Next(100) / 100f + 18 + 0.5f * i
                    };
                    points.Add(p);
            }
            CurveData d = new CurveData
            {
                curveColor = Colors.Red,
                Source = points
            };
            drawable.data.Clear();
            drawable.data.Add(d);
            dt= DateTime.Now.AddSeconds(-20);
            points.Clear();
            for (int i = 0; i < 20; i++) 
            {
                p = new CurvePoint
                {
                    timePos = dt.AddSeconds(i),
                    tempPos = 5 + rand.Next(100) / 100f + 18 + 0.5f * i
                };
                points.Add(p);
            }
            d = new CurveData
            {
                curveColor = Colors.Blue,
                Source = points
            };

            drawable.data.Add(d);

            drawable.voffset = 18;
            drawable.vscale_level = 2;
            drawable.hscale_level = 0;
            drawable.hoffset = dt;
            curveView.Invalidate();
        }


        bool is_started = false;
        private void startButton_Clicked(object sender, EventArgs e)
        {
            is_started = !is_started;

            CurveDrawable drawable = (CurveDrawable)curveView.Drawable;
            test();
            ch1ControlView.Source = drawable.data[0];
        }
        private void autoButton_Clicked(object sender, EventArgs e)
        {
            CurveDrawable s = (CurveDrawable)curveView.Drawable;
            s.Auto();
            vPanel.ChangeSliderValue(s.GetCellHeightPercent(s.vzoom_factor));
            curveView.Invalidate();
        }

        bool is_track = false;
        private void trackButton_Clicked(object sender, EventArgs e)
        {
            is_track = !is_track;
            if(is_track)
            {
                CurveDrawable s = (CurveDrawable)curveView.Drawable;
                s.Track();
                curveView.Invalidate();
            }
        }

    }

}
