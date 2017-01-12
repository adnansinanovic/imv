using System.Windows.Forms;

namespace Sinantrop.IMV.UI.Components
{
    public partial class UcDoubleChart : UserControl, IMainScreenComponent
    {
        Manager _manager;
        public UcDoubleChart()
        {
            InitializeComponent();
            _manager = Manager.Instance;
        }

        public void UseData(MainScreenComponentArgs args)
        {
            ucChart1.UseData(args);

            args.StatisticsType = StatisticsType.Urls;
            ucChart2.UseData(args);
        }
    }
}
