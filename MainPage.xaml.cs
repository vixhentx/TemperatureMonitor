using TemperatureMonitor.Classes;
using TemperatureMonitor.ViewModels;
using TemperatureMonitor.Views;

namespace TemperatureMonitor
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            curveView.Source = viewModel.Curves;

            vPanel.Target = curveView;
            hPanel.Target = curveView;

            
            //Test

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
            CurveData d1 = new CurveData
            {
                curveColor = Colors.Red,
                Source = points
            };
            viewModel.Curves.Clear();
            viewModel.Curves.Add(d1);
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
            CurveData d2 = new CurveData
            {
                curveColor = Colors.Blue,
                Source = points
            };

            viewModel.Curves.Add(d2);

            drawable.voffset = 18;
            drawable.vscale_level = 2;
            drawable.hscale_level = 0;
            drawable.hoffset = dt;

            viewModel.Channels.Clear();

            viewModel.Channels.Add(new()
            {
                Channel = 1,
                Curve = curveView.Source[0],
                Name = "Left",
                State = new ChannelState()
                {
                    Battery = 50,
                    Temperature = 20
                }
            });
            viewModel.Channels.Add(new()
            {
                Channel = 2,
                Curve = curveView.Source[1],
                Name = "R",
                State = new ChannelState()
                {
                    Battery = 5,
                    Temperature = 30
                }
            });

            curveView.Invalidate();

        }


        bool is_started = false;
        private void startButton_Clicked(object sender, EventArgs e)
        {
            is_started = !is_started;

            CurveDrawable drawable = (CurveDrawable)curveView.Drawable;
            test();
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
