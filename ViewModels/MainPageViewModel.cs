using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TemperatureMonitor.Classes;

namespace TemperatureMonitor.ViewModels
{
    public class MainPageViewModel
    {
        public ObservableCollection<ChannelControllerModel> Channels { get; set; }
        public List<CurveData> Curves { get; set; }

        public MainPageViewModel()
        {
            // Test
            Channels = new();
            Curves = new();
        }
    
    }

}
