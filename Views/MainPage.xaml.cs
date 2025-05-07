using TemperatureMonitor.Classes;
using TemperatureMonitor.ViewModels;
using TemperatureMonitor.Views;

namespace TemperatureMonitor
{
    public partial class MainPage : ContentPage
    {
        private CurveDrawable drawable => (CurveDrawable)curveView.Drawable;

        //Timer timer ;

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
        private void test_append()
        {
            Random rand = new();
            var t1= viewModel.Curves[0][DateTime.Now]= rand.Next(180, 280) / 10.0f;
            var t2= viewModel.Curves[1][DateTime.Now] = rand.Next(180, 280) / 10.0f;
            viewModel.Channels[0].State.Temperature = t1;
            viewModel.Channels[1].State.Temperature = t2;
        }
        private void test_init()
        {
            Random rand=new Random();
            if (drawable == null) return;
            DateTime dt= DateTime.Now.AddSeconds(-20);

            CurveData d1 = new CurveData
            {
                curveColor = Colors.Red
            };
            for (int i = 0; i < 20; i++)
            {
                d1[dt.AddSeconds(i)] = rand.Next(100) / 100f + 18 + 0.5f * i;
            }
            viewModel.Curves.Add(d1);

            CurveData d2 = new CurveData
            {
                curveColor = Colors.Blue
            };
            for (int i = 0; i < 20; i++) 
            {
                d2[dt.AddSeconds(i)] = 5 + rand.Next(100) / 100f + 18 + 0.5f * i;
            }
            

            viewModel.Curves.Add(d2);

            drawable.voffset = 18;
            drawable.vscale_level = 2;
            drawable.hscale_level = 0;
            drawable.hoffset = dt;

            dt = dt.AddSeconds(19);

            viewModel.Channels.Add(new()
            {
                Channel = 1,
                Curve = curveView.Source[0],
                Name = "T_Dec1",
                State = new ChannelState()
                {
                    Battery = rand.Next(0,100),
                    Temperature = d1[dt],
                }
            });
            viewModel.Channels.Add(new()
            {
                Channel = 2,
                Curve = curveView.Source[1],
                Name = "T_Dec2",
                State = new ChannelState()
                {
                    Battery = rand.Next(0, 100),
                    Temperature = d2[dt]
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
