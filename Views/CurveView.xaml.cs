using System.Collections.Generic;
using TemperatureMonitor.Classes;

namespace TemperatureMonitor.Views;

public partial class CurveView : GraphicsView
{

    BindableProperty SourceProperty = BindableProperty.Create(nameof(Source), typeof(List<CurveData>), typeof(CurveView), new List<CurveData>());

    public List<CurveData> Source
    {
        get => (List<CurveData>)GetValue(SourceProperty);
        set => SetValue(SourceProperty, value);
    }

    CurveDrawable drawable;
    public CurveView()
    {
        Drawable = new CurveDrawable();
        drawable = (CurveDrawable)Drawable;
        drawable.data = Source;
        InitializeComponent();

    }
    double last_tx = 0, last_ty = 0, now_x = 0, now_y = 0;
    private void PanGestureRecognizer_PanUpdated(object? sender, PanUpdatedEventArgs e)
    {
        float width = (float)Width, height = (float)Height;
        switch (e.StatusType)
        {
            case GestureStatus.Running:

                now_x = e.TotalX - last_tx;
                now_y = e.TotalY - last_ty;
                drawable.OnPan((float)now_x / width, (float)now_y / height);
                Invalidate();
                last_tx = e.TotalX;
                last_ty = e.TotalY;
                break;
            case GestureStatus.Completed:
                last_tx = 0;
                last_ty = 0;
                break;
        }
    }
}