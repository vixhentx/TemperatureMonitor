using TemperatureMonitor.Classes;
using TemperatureMonitor.ViewModels;
using TemperatureMonitor.Views;

namespace TemperatureMonitor
{
    public partial class MainPage : ContentPage
    {
        private CurveDrawable drawable => (CurveDrawable)curveView.Drawable;

        Timer timer ;

        public MainPage()
        {
            InitializeComponent();

            curveView.Source = viewModel.Curves;

            vPanel.Target = curveView;
            hPanel.Target = curveView;


            //Test
            //timer = new((s) => OnTick(),null,0,1000);

        }

        private void OnTick()
        {
            if(is_started)
            {
                if (is_track) drawable.Track();
                test_append();
            }
        }
        private void AddData(CurvePoint pointData,int channel)
        {

        }
        private void SetDatas(List<CurvePoint> initialPointData,int channel)
        {

        }
        private void test_append()
        {
            Random rand = new();
            CurvePoint p1, p2;
            p1 = new()
            {
                tempPos = rand.Next(180, 280) / 10.0f,
                timePos = DateTime.Now
            };
            p2= new()
            {
                tempPos = rand.Next(180, 280) / 10.0f,
                timePos = DateTime.Now
            };
            viewModel.Curves[0].AddPoint(p1);
            viewModel.Curves[1].AddPoint(p2);
            viewModel.Channels[0].State.Temperature = p1.tempPos;
            viewModel.Channels[1].State.Temperature = p2.tempPos;
        }
        private void test_init()
        {
            Random rand=new Random();
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

            viewModel.Channels.Add(new()
            {
                Channel = 1,
                Curve = curveView.Source[0],
                Name = "Left",
                State = new ChannelState()
                {
                    Battery = rand.Next(0,100),
                    Temperature = d1.LastPoint.tempPos,
                }
            });
            viewModel.Channels.Add(new()
            {
                Channel = 2,
                Curve = curveView.Source[1],
                Name = "R",
                State = new ChannelState()
                {
                    Battery = rand.Next(0, 100),
                    Temperature = d2.LastPoint.tempPos
                }
            });

            curveView.Invalidate();

        }


        bool is_started = false;
        private void startButton_Clicked(object sender, EventArgs e)
        {
            is_started = !is_started;

            if (viewModel.Curves.Count == 0) test_init();
            else test_append();
        }
        private void autoButton_Clicked(object sender, EventArgs e)
        {
            drawable.Auto();
            vPanel.ChangeSliderValue(drawable.GetCellHeightPercent(drawable.vzoom_factor));
            curveView.Invalidate();
        }

        bool is_track = false;
        private void trackButton_Clicked(object sender, EventArgs e)
        {
            is_track = !is_track;
            if(is_track)
            {
                drawable.Track();
                curveView.Invalidate();
            }
        }

    }

}
