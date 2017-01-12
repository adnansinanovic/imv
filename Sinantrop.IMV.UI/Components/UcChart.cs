using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using Sinantrop.IMV.ViewModels;
using Sinantrop.Helper;

namespace Sinantrop.IMV.UI.Components
{
    public partial class UcChart : UserControl, IMainScreenComponent
    {
        Manager _manager;
        public UcChart()
        {
            InitializeComponent();
            _manager = Manager.Instance;
        }

        public void UseData(MainScreenComponentArgs args)
        {
            var conversation = args.Conversation != null ? args.Conversation : new Conversation() { Id = -1};
            List<ConversationStatistics> statistics = _manager.GetStatistics(conversation, args.DtFrom, args.DtTo, args.StatisticsType);

            ChartInfo chartInfo = ChartInfo.Create(statistics, args);

            CreateChart(chartInfo);            
        }

        private void CreateChart(ChartInfo chartInfo)
        {
            chart.Series.Clear();
            chart.Titles.Clear();
            chart.Titles.Add(chartInfo.Title);
            chart.Legends[0].Docking = Docking.Bottom;

            chart.ChartAreas[0].AxisY.Title = "Messages";
            chart.ChartAreas[0].AxisX.MinorGrid.LineWidth = 0;
            chart.ChartAreas[0].AxisX.MajorGrid.LineWidth = 0;
            chart.ChartAreas[0].AxisX.MajorTickMark.TickMarkStyle = TickMarkStyle.None;
            chart.ChartAreas[0].AxisX.MinorTickMark.TickMarkStyle = TickMarkStyle.None;

            chart.ChartAreas[0].AxisX.MajorTickMark.Enabled = false;
            chart.ChartAreas[0].AxisX.MinorTickMark.Enabled = false;

            chart.ChartAreas[0].AxisX.Interval = 0;
            chart.ChartAreas[0].AxisX.IsMarginVisible = false;
            chart.ChartAreas[0].AxisX.Enabled = AxisEnabled.False;

            List<ChartItem> items = chartInfo.Items.OrderBy(o => o.GetValue("Count")).ToList(); ;
            int total = items.AsEnumerable().Sum(a => (Convert.ToInt32(a.GetValue("Count"))));

            if (items.Count < 3)
            {

                Series series = chart.Series.Add("Count");
                series.ChartType = SeriesChartType.Pie;

                foreach (var item in items)
                {
                    ulong itemValue = Convert.ToUInt64(item.GetValue("Count"));
                    decimal perc = total > 0 ? itemValue / (decimal)total : 0;

                    series.Points.Add(itemValue);
                    series.Points.Last().Label = $"{itemValue} = {itemValue} ({decimal.Round(perc * 100, 2)}%)";
                    series.Points.Last().LegendText = item.Author;
                }
            }
            else
            {
                foreach (var item in items)
                {
                    Series series = chart.Series.Add(item.Author);
                    series["PointWidth"] = "1.5";
                    series.ChartType = SeriesChartType.Column;

                    ulong itemValue = Convert.ToUInt64(item.GetValue("Count"));
                    decimal perc = total > 0 ? itemValue / (decimal)total : 0;

                    series.Points.Add(itemValue);
                    series.Points.Last().Label = $"{itemValue} ({decimal.Round(perc * 100, 2)}%)";                    
                }
            }
        }        
    }        
}
