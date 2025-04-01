using System.Numerics;
using TemperatureMonitor.Graphics;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TemperatureMonitor
{
    public partial class MainPage : ContentPage
    {

        public MainPage()
        {
            InitializeComponent();
        }

        private void testButton_Clicked(object sender, EventArgs e)
        {
            Random rand=new Random();
            CurveDrawable drawable = (CurveDrawable)curveView.Drawable;
            if (drawable == null) return;
            List<CurvePoint> points = new List<CurvePoint>();
            CurvePoint p;
            DateTime dt= DateTime.Now;
            for (int i = 0; i < 20; i++)
            {
                    p = new CurvePoint
                    {
                        timePos = dt.AddSeconds(i),
                        tempPos = rand.Next(100) / 100f + 18 + 0.5f * i
                    };
                    points.Add(p);
            }
            CurveData d = new CurveData()
            {
                curveColor = Colors.Red,
                Source = points
            };
            drawable.data.Clear();
            drawable.data.Add(d);
            drawable.voffset = 18;
            drawable.vscale_level = 2;
            drawable.hscale_level = 0;
            drawable.start_time = dt;
            curveView.Invalidate();
        }


        double last_tx=0, last_ty=0, now_x=0, now_y=0;


        private void PanGestureRecognizer_PanUpdated(object sender, PanUpdatedEventArgs e)
        {
            CurveDrawable s = (CurveDrawable)curveView.Drawable;
            float width = (float) curveView.Width , height = (float) curveView.Height ;
            switch (e.StatusType)
            {
                case GestureStatus.Running:
                    
                    now_x = e.TotalX - last_tx;
                    now_y = e .TotalY - last_ty;
                    s.OnPan((float)now_x/width, (float)now_y/height);
                    curveView.Invalidate();
                    last_tx = e.TotalX;
                    last_ty = e .TotalY;
                    break;
                case GestureStatus.Completed:
                    last_tx = 0;
                    last_ty = 0;
                    break;
            }
        }
        private void vscaleSlider_ValueChanged(object sender, EventArgs e)
        {
            CurveDrawable s = (CurveDrawable)curveView.Drawable;
            s.OnChangeCellHeight((float)vscaleSlider.Value);
            curveView.Invalidate();
        }

        private void hscaleMinusButton_Clicked(object sender, EventArgs e)
        {
            CurveDrawable s = (CurveDrawable) curveView.Drawable;
            if (s.hscale_level >= s.max_hscale_level) return;
            s.hscale_level += 1;
            hscaleSlider.Value = 1;
            curveView.Invalidate();
        }

        private void hscalePlusButton_Clicked(object sender, EventArgs e)
        {
            CurveDrawable s = (CurveDrawable)curveView.Drawable;
            if (s.hscale_level <=0) return;
            s.hscale_level -= 1;
            hscaleSlider.Value = 0;
            curveView.Invalidate();
        }

        private void vscaleMinusButton_Clicked(object sender, EventArgs e)
        {
            CurveDrawable s = (CurveDrawable)curveView.Drawable;
            if (s.vscale_level >= s.max_vscale_level) return;
            s.vscale_level += 1;
            vscaleSlider.Value = 1;
            curveView.Invalidate();
        }

        private void vscalePlusButton_Clicked(object sender, EventArgs e)
        {
            CurveDrawable s = (CurveDrawable)curveView.Drawable;
            if (s.vscale_level <=0) return;
            s.vscale_level -= 1;
            vscaleSlider.Value = 0;
            curveView.Invalidate();
        }

        private void hscaleSlider_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            CurveDrawable s = (CurveDrawable)curveView.Drawable;
            s.OnChangeCellWidth((float)hscaleSlider.Value);
            curveView.Invalidate();
        }
        private void vPannel_Reset(object sender, TappedEventArgs e)
        {
            CurveDrawable s = (CurveDrawable)curveView.Drawable;
            float v = s.GetCellHeightPercent(1.0f);
            s.vzoom_factor = 1.0f;
            vscaleSlider.Value = v;
            curveView.Invalidate();
        }
        private void hPannel_Reset(object sender, TappedEventArgs e)
        {
            CurveDrawable s = (CurveDrawable)curveView.Drawable;
            float v = s.GetCellWidthPercent(1.0f);
            s.hzoom_factor = 1.0f;
            hscaleSlider.Value = v;
            curveView.Invalidate();
        }
    }

}
