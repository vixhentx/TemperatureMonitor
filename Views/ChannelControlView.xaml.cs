using System.Threading.Channels;
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
				return "CH " + Channel.ToString();
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
	public CurveData Source
	{
		get => (CurveData)GetValue(SourceProperty);
		set => SetValue(SourceProperty, value);
	}
	public ChannelControlView()
	{
		InitializeComponent();
	}
}