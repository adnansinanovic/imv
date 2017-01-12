using System;
using Sinantrop.IMV.UI.Components;
using Sinantrop.IMV.ViewModels;

namespace Sinantrop.IMV.UI
{
    public interface IMainScreenComponent
    {
        void UseData(MainScreenComponentArgs args);
    }

    public class MainScreenComponentArgs
    {
        public MainScreenComponentArgs()
        {            
            FilterUrls = false;
            DtTo = DateTime.Now;
            DtFrom = DtTo.AddDays(-7);
            StatisticsType = StatisticsType.Full;
        }

        public ChartInfo ChartInfo { get; set; }

        public Conversation Conversation { get; set; }
        public DateTime DtFrom { get; set; }
        public DateTime DtTo { get; set; }
        public bool FilterUrls { get; internal set; }
        public StatisticsType StatisticsType { get; internal set; }
    }
}
