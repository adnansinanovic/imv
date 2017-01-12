using System.Collections.Generic;
using Sinantrop.IMV.ViewModels;

namespace Sinantrop.IMV.UI.Components
{
    public class ChartInfo
    {
        public ChartInfo()
        {
            Items = new List<ChartItem>();
        }

        public string Title { get; set; }

        public List<ChartItem> Items { get; set; }

        internal static ChartInfo Create(List<ConversationStatistics> statistics, MainScreenComponentArgs args)
        {            
            ChartInfo info = new ChartInfo();
            info.Title = string.Format("{0} [{1}]", args.Conversation.Title, args.StatisticsType);

            foreach (ConversationStatistics item in statistics)
            {
                ChartItem chartItem = new ChartItem(item.Identity);
                chartItem.Count = item.TotalMessages;
                info.Items.Add(chartItem);
            }

            return info;
        }
    }
}
