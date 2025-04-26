using System.Security.AccessControl;
using TemperatureMonitor.Classes;

namespace TemperatureMonitor.Views;

public partial class CurveControlView : ContentView
{
    public BindableProperty TargetProperty = BindableProperty.Create(nameof(Target), typeof(CurveView), typeof(CurveControlView));
    public BindableProperty DirectionProperty = BindableProperty.Create(nameof(Direction), typeof(string), typeof(CurveControlView));

    protected string _text = "";
    protected bool _direction = false; //false: ˮƽ
    public string Text
    {
        get => _text;
        set { _text = value; }
    }
    public CurveView Target
    {
        get => (CurveView)GetValue(TargetProperty);
        set => SetValue(TargetProperty, value); 
    }
    public string Direction
    {
        get => (string)GetValue(DirectionProperty);
        set
        {
            _direction = false;
            if (value.ToLower() == "vertical") _direction = true;
            SetValue(DirectionProperty, value);
        }
    }
    private CurveDrawable s=> (CurveDrawable)Target.Drawable;
    public CurveControlView()
	{
		InitializeComponent();
        label.Text = _text;
	}

    private void OnReset(object sender, TappedEventArgs e)
    {
        if(_direction)
        {
            float v = s.GetCellHeightPercent(1.0f);
            s.vzoom_factor = 1.0f;
            slider.Value = v;
            Target.Invalidate();
        }
        else
        {
            float v = s.GetCellWidthPercent(1.0f);
            s.hzoom_factor = 1.0f;
            slider.Value = v;
            Target.Invalidate();
        }
    }

    private void OnSlide(object sender, ValueChangedEventArgs e)
    {
        if(_direction)
        {
            s.OnChangeCellHeight((float)slider.Value);
            Target.Invalidate();
        }
        else
        {
            s.OnChangeCellWidth((float)slider.Value);
            Target.Invalidate();
        }
    }

    private void OnMinus(object sender, EventArgs e)
    {
        if(_direction)
        {
            if (s.vscale_level >= s.max_vscale_level) return;
            s.vscale_level += 1;
            slider.Value = 1;
            Target.Invalidate();
        }
        else
        {
            if (s.hscale_level >= s.max_hscale_level) return;
            s.hscale_level += 1;
            slider.Value = 1;
            Target.Invalidate();
        }
    }

    private void OnPlus(object sender, EventArgs e)
    {
        if(_direction)
        {
            if (s.vscale_level >= s.max_vscale_level) return;
            s.vscale_level += 1;
            slider.Value = 1;
            Target.Invalidate();
        }
        else
        {
            if (s.hscale_level >= s.max_hscale_level) return;
            s.hscale_level += 1;
            slider.Value = 1;
            Target.Invalidate();
        }
    }
    public void ChangeSliderValue(float value)
    {
        slider.Value = value;
    }
}