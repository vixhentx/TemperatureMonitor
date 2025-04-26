using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemperatureMonitor.Classes
{
    //思路：先用旧的比例尺等参数计算，再根据算出来的方格大小来决定更适合的比例尺
    //当方格大小超过设定范围时，就会通过GetScaleLevel设定新的比例尺，并重新计算方格大小
    //缩放控制和画曲线的函数分开
    public class CurveDrawable : IDrawable
    {

        public List<CurveData> data = new List<CurveData>();

        const int vaxis_width = 80, haxis_height = 50;
        static float[] vscale_base = { 0.1F, 0.5F, 1, 5, 10, 20 };//一格多少度
        static float[] hscale_base = { 1, 10, 30, 60, 600, 1800, 3600 };//一格多秒
        const float max_cell_height = 0.3f, max_cell_width = 0.3f, min_cell_height = 0.05f, min_cell_width = 0.05f;
        const float cell_height_base = 0.1f, cell_width_base = 0.1f;
        const float ymax = 120, ymin = -30;

        public int vscale_level = 4, hscale_level = 0;
        public float vzoom_factor = 1.0f, hzoom_factor = 1.0f;
        public DateTime hoffset = DateTime.Now;
        public float voffset = 0.0f;
        const float reserved_area_for_curve = 30, reserved_area_for_vscale = 10, reserved_area_for_hscale = 15;

        static float[] stroke_dash_pattern = { 2, 2 };
        const string temperatur_unit = "℃";



        protected int GetScaleLevel(float span, float cell_length, float[] scale)
        {
            int l = 0, r = scale.Count(), mid;
            while (l < r)
            {
                mid = (l + r) / 2;
                if (cell_length >= scale[mid] / span)
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
        void Check()
        {
            if (hoffset.AddSeconds(hcount) > DateTime.Now) hoffset = DateTime.Now.AddSeconds(-hcount);
            voffset = Math.Clamp(voffset, ymin, ymax);
            hscale_level = Math.Clamp(hscale_level, 0, max_hscale_level);
            vscale_level = Math.Clamp(vscale_level, 0, max_vscale_level);
        }
        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            Check();

            DrawAxis(canvas, dirtyRect);
            foreach (var d in data)
            {
                if (d.visable)
                {
                    DrawCurve(d, canvas, dirtyRect);
                }
            }
        }
        void DrawAxis(ICanvas canvas, RectF dirtyRect)
        {
            float width = dirtyRect.Width, height = dirtyRect.Height;
            float bottom = dirtyRect.Bottom - reserved_area_for_curve, top = dirtyRect.Top + reserved_area_for_curve;
            float left = dirtyRect.Left + vaxis_width, right = dirtyRect.Right - reserved_area_for_curve;

            float xbegin = left, ybegin = bottom;


            /*画轴*/
            //画虚线
            canvas.StrokeColor = Colors.Gray;
            canvas.StrokeSize = 2;
            canvas.StrokeDashPattern = stroke_dash_pattern;
            for (int i = 0; i < mspan; i++)
            {
                float y = ybegin - i * cell_height * height;//voffset处是最底端
                canvas.DrawLine(reserved_area_for_vscale, y, right, y);
            }

            //画数字
            canvas.StrokeColor = Colors.LightGray;
            canvas.StrokeSize = 2;
            canvas.StrokeDashPattern = null;
            //垂直
            for (int i = 0; i < mspan; i++)
            {
                float y = ybegin - i * cell_height * height;
                canvas.DrawString((voffset + i * vscale).ToString("0.0") + temperatur_unit, reserved_area_for_vscale, y, HorizontalAlignment.Left);
            }
            //下标刻度
            canvas.StrokeDashPattern = null;
            canvas.StrokeSize = 5;
            for (int i = 0; i < hcount; i++)
            {
                float x = xbegin + (i - hoffset.Millisecond / 1000f / hscale) * cell_width * width;
                float y = ybegin + reserved_area_for_hscale;
                string s = hoffset.AddSeconds(((int)hscale * (2 * i + 1) - 1) / 2).ToString("HH:mm:ss");
                canvas.DrawString(s, x, y, HorizontalAlignment.Center);
            }
        }
        void DrawCurve(CurveData d, ICanvas canvas, RectF dirtyRect)
        {

            float width = dirtyRect.Width, height = dirtyRect.Height;
            float bottom = dirtyRect.Bottom - reserved_area_for_curve, top = dirtyRect.Top + reserved_area_for_curve;
            float left = dirtyRect.Left + vaxis_width, right = dirtyRect.Right - reserved_area_for_curve;

            float xbegin = left, ybegin = bottom;


            /*画线*/
            if (hcount <= 0) return;
            PointF[] points = new PointF[hcount];

            for (int i = 0; i < hcount; i++)
            {
                points[i].X = xbegin + (i - hoffset.Millisecond / 1000f / hscale) * cell_width * width;
                points[i].Y = ybegin - (d[hoffset.AddSeconds(i * hscale - hoffset.Millisecond / 1000f), hoffset.AddSeconds((i + 1) * hscale - hoffset.Millisecond / 1000f)] - voffset) / vscale * cell_height * height;
            }


            //曲线区域
            //创建折现轨迹
            PathF path = new PathF();
            path.MoveTo(points[0]);
            for (int i = 1; i < hcount; i++)
            {
                path.LineTo(points[i]);
            }
            //画折现轨迹
            canvas.StrokeDashPattern = null;
            canvas.StrokeSize = 10;
            canvas.StrokeColor = d.curveColor;
            canvas.StrokeLineJoin = LineJoin.Round;
            canvas.DrawPath(path);

        }
        public void Auto()
        {
            float min = ymax, max = ymin;
            foreach (var d in data)
            {
                for (int i = 0; i < hcount; i++)
                {
                    var v = d[hoffset.AddSeconds((int)hscale * i), hoffset.AddSeconds((int)hscale * (i + 1))];
                    if (v < min) min = v;
                    if (v > max) max = v;
                }
            }

            vscale_level = Math.Clamp(GetScaleLevel(max - min, 0.1f, vscale_base), 0, max_vscale_level);
            Check();
            vzoom_factor = vscale / (cell_height_base * (max - min));
            voffset = min - 0.5f * cell_height * vscale;

        }
        public void Track()
        {
            hoffset = DateTime.Now.AddSeconds(-nspan);
        }
        public void OnChangeCellHeight(float vPercentage)
        {
            float min, max;
            if (vscale_level > 0)
            {
                min = vscale_base[vscale_level - 1] / vscale_base[vscale_level];
            }
            else
            {
                min = vscale_base[vscale_level];
            }
            if (vscale_level < vscale_base.Length - 1)
            {
                max = vscale_base[vscale_level + 1] / vscale_base[vscale_level];
            }
            else
            {
                max = vscale_base[vscale_level];
            }
            vzoom_factor = min + vPercentage * (max - min);
            Check();
        }
        public float GetCellHeightPercent(float vzf)
        {
            float min, max;
            if (vscale_level > 0)
            {
                min = vscale_base[vscale_level - 1] / vscale_base[vscale_level];
            }
            else
            {
                min = vscale_base[vscale_level];
            }
            if (vscale_level < vscale_base.Length - 1)
            {
                max = vscale_base[vscale_level + 1] / vscale_base[vscale_level];
            }
            else
            {
                max = vscale_base[vscale_level];
            }
            return (vzf - min) / (max - min);
        }
        public void OnChangeCellWidth(float hPercentage)
        {
            float min, max;
            if (hscale_level > 0)
            {
                min = hscale_base[hscale_level - 1] / hscale_base[hscale_level];
            }
            else
            {
                min = hscale_base[hscale_level];
            }
            if (hscale_level < hscale_base.Length - 1)
            {
                max = hscale_base[hscale_level + 1] / hscale_base[hscale_level];
            }
            else
            {
                max = hscale_base[hscale_level];
            }
            hzoom_factor = min + hPercentage * (max - min);
            Check();
        }
        public float GetCellWidthPercent(float hzf)
        {
            float min, max;
            if (hscale_level > 0)
            {
                min = hscale_base[hscale_level - 1] / hscale_base[hscale_level];
            }
            else
            {
                min = hscale_base[hscale_level];
            }
            if (hscale_level < hscale_base.Length - 1)
            {
                max = hscale_base[hscale_level + 1] / hscale_base[hscale_level];
            }
            else
            {
                max = hscale_base[hscale_level];
            }
            return (hzf - min) / (max - min);
        }
        public void OnZoom(float zoomX_Percentage, float zoomY_Percentage)
        {
            hzoom_factor += zoomX_Percentage;
            vzoom_factor += zoomY_Percentage;

            float cell_width = cell_width_base * hzoom_factor;
            float cell_height = cell_height_base * vzoom_factor;
            if (vscale_level <= vscale_base[vscale_base.Count() - 1])
            {
                float new_vscale = vscale_base[vscale_level + 1];
                if (cell_height * vscale > min_cell_height * new_vscale)
                {
                    vscale_level++;
                    cell_height *= vscale * new_vscale;
                }
            }
        }
        public void OnPan(float panX_Percentage, float panY_Percentage)
        {
            float cell_width = cell_width_base * hzoom_factor;
            float cell_height = cell_height_base * vzoom_factor;

            hoffset = hoffset.AddSeconds(-panX_Percentage / cell_width * hscale);
            voffset += panY_Percentage / cell_height * vscale;
            Check();
        }



        public int max_hscale_level { get { return hscale_base.Length - 1; } }
        public int max_vscale_level { get { return vscale_base.Length - 1; } }
        public int maxCount
        {
            get
            {
                int ans = -1;
                foreach (var v in data)
                {
                    if (v.Count > ans)
                    {
                        ans = v.Count;
                    }
                }
                return ans;
            }
        }
        float hscale { get { return hscale_base[hscale_level]; } }
        float vscale { get { return vscale_base[vscale_level]; } }
        float cell_height { get { return cell_height_base * vzoom_factor; } }
        float cell_width { get { return cell_width_base * hzoom_factor; } }

        int nspan { get { return int.Min((int)float.Floor(hscale / cell_width) + 1, maxCount); } }
        int mspan { get { return (int)float.Floor(1 / cell_height); } }

        int hcount { get { return int.Min((int)float.Floor(1 / cell_width) + 1, (int)float.Floor(maxCount / hscale)); } }
    }
}
