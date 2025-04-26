using System.Threading.Channels;
using TemperatureMonitor.Classes;
using TemperatureMonitor.Graphics;

namespace TemperatureMonitor.Views;

public partial class ChannelControlView : ContentView
{
	protected string _name = "";
	public int Channel{ get; set; }

    public string Name
	{
		get
		{
			if(string.IsNullOrEmpty(_name))
			{
                _name = "Channel " + Channel.ToString();
			}
			return _name;
		}
		set
		{
			_name = value;
		}
	}
	public Color Color => Source.curveColor;
	public readonly BindableProperty SourceProperty = BindableProperty.Create(nameof(Source),typeof(CurveData),typeof(ChannelControlView));
	public readonly BindableProperty StateProperty = BindableProperty.Create(nameof(State),typeof(ChannelState),typeof(ChannelControlView));
	public CurveData Source
	{
		get => (CurveData)GetValue(SourceProperty);
		set => SetValue(SourceProperty, value);
	}
    public ChannelState State
    {
        get => (ChannelState)GetValue(StateProperty);
        set => SetValue(StateProperty, value);
    }
	public string Battery
	{
		get
		{
			return State.Battery.ToString() + "%";
		}
	}
    public string Temperature
    {
        get
        {
			return State.Temperature.ToString() + "℃";
        }
    }
    static string[] bat_icontable = { "\ue631", "\ue62c", "\ue62e", "\ue632", "\ue630", "\ue62d" , "\ue62f" };
	public string BatteryIcon
	{
		get
		{
			//本来想照抄win10逻辑的，找不到，只好自己写一个 （win10的神秘逻辑确实也挺误导的
			int bat = State.Battery, state=0;
			if (bat <= 1) state = 0;
			else if (bat <= 5) state = 1;
			else if (bat <= 10) state = 2;
			else if (bat <= 20) state = 3;
			else if (bat <= 30) state = 4;
			else if (bat <= 80) state = 5;
			else state = 6;
			return bat_icontable[state];
        }
	}
	static string[] temp_icontable = { "\ue63a", "\ue639", "\ue638" };//普通，升高，降低
    public string TemperatureIcon
    {
        get
        {
			//本来想照抄win10逻辑的，找不到，只好自己写一个 （win10的神秘逻辑确实也挺误导的
			float temp = State.Temperature, ltemp=State.last_temperature;
			int state = 0;
			if (temp > ltemp) state = 1;
			else if (temp < ltemp) state = 2;

            return temp_icontable[state];
        }
    }
    public ChannelControlView()
	{
		Source = new CurveData();
		State = new ChannelState();
		InitializeComponent();
	}
}