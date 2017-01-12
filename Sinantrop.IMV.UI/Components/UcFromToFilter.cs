using System;
using System.Windows.Forms;

namespace Sinantrop.IMV.UI.Components
{
    public partial class UcFromToFilter : UserControl
    {        
        public UcFromToFilter()
        {
            InitializeComponent();
            _dtTo.Value = DateTime.Now;
            _dtFrom.Value = _dtTo.Value.AddDays(-7);
        }

        public DateTime DtFrom { get { return _dtFrom.Value; } }
        public DateTime DtTo { get { return _dtTo.Value; } }

        private void _dtFrom_ValueChanged(object sender, System.EventArgs e)
        {

        }

        private void _dtTo_ValueChanged(object sender, System.EventArgs e)
        {

        }
    }
}
