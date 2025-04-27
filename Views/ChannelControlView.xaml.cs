using System.Security.Cryptography;
using TemperatureMonitor.Classes;
using TemperatureMonitor.ViewModels;

namespace TemperatureMonitor.Views
{
    public partial class ChannelControlView : ContentView
    {
        
        /*
        public readonly BindableProperty CurveProperty = BindableProperty.Create(nameof(Curve),typeof(CurveData),typeof(ChannelControlView));
        public readonly BindableProperty StateProperty = BindableProperty.Create(nameof(State),typeof(ChannelState),typeof(ChannelControlView));
        public readonly BindableProperty NameProperty = BindableProperty.Create(nameof(Name),typeof(string),typeof(ChannelControlView));
        public readonly BindableProperty ChannelProperty = BindableProperty.Create(nameof(Channel),typeof(int),typeof(ChannelControlView));
        // public readonly BindableProperty SourceProperty = BindableProperty.Create(nameof(Source), typeof(ChannelControllerModel), typeof(ChannelControlView));
        */
        /*
        public ChannelControllerModel Source
        {
            get => (ChannelControllerModel)GetValue(SourceProperty);
            set => SetValue(SourceProperty, value);
        }
        */
        
        protected string _name;
        protected CurveData _curve;
        protected ChannelState _state;
        protected int _channel;
        
        public string Name
        {
            get => _name;
            set => _name = value;
        }
        public CurveData Curve
        {
            get => _curve;
            set => _curve = value;
        }
        public ChannelState State
        {
            get => _state;
            set => _state = value;
        }
        public int Channel
        {
            get => _channel;
            set => _channel = value;
        }
        /*
        public object Name
        {
            get => _name;
            set => _name = value as string;
        }
        public object Curve
        {
            get => _curve;
            set => _curve =  value as CurveData;
        }
        public object State
        {
            get => _state;
            set => _state = value as ChannelState;
        }
        public object Channel
        {
            get => _channel;
            set => _channel = (int)value;
        }
        */
        /*
        public object Name
        {
            get
            {
                string str = (string)GetValue(NameProperty);
                if (string.IsNullOrEmpty(str))
                {
                    str = "Channel " + Channel.ToString();
                }
                SetValue(NameProperty, str);
                return str;
            }
            set 
            {
                SetValue(NameProperty, value);
                _name = (string)value;
            }
        }
        public object Curve
        {
            get=> (CurveData)GetValue(CurveProperty);
            set => SetValue(CurveProperty, value);
        }
        public object State
        {
            get => (ChannelState)GetValue(StateProperty);
            set => SetValue(StateProperty, value);
        }
        
        public int Channel
        {
            get => (int)GetValue(ChannelProperty);
            set => SetValue(ChannelProperty, value);
        }
        */

        public Color Color => _curve.curveColor;
        public string Battery => _state.Battery.ToString() + "%";
        public string Temperature => _state.Temperature.ToString() + "℃";

        static string[] bat_icontable = { "\ue631", "\ue62c", "\ue62e", "\ue632", "\ue630", "\ue62d", "\ue62f" };
        public string BatteryIcon
        {
            get
            {
                //本来想照抄win10逻辑的，找不到，只好自己写一个 （win10的神秘逻辑确实也挺误导的
                int bat = _state.Battery, state = 0;
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
                float temp = _state.Temperature, ltemp = _state.last_temperature;
                int state = 0;
                if (temp > ltemp) state = 1;
                else if (temp < ltemp) state = 2;

                return temp_icontable[state];
            }
        }

        public ChannelControlView()
        {
            /*
            if(Source!=null&& Source is ChannelControllerModel)
            {
                ChannelControllerModel s = (ChannelControllerModel)Source;
                Name = (string)s.Name;
                Curve= (CurveData)s.Curve;
                State= (ChannelState)s.State;
                Channel= s.Channel;
            }*/
            Name ??= "";
            Curve ??= new();
            State ??= new();
            BindingContext = this;
            InitializeComponent();
        }
    }
}
