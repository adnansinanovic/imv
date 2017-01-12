using System.Linq;
using System.Windows.Forms;

namespace Sinantrop.IMV.UI.Components
{
    public partial class UcFastMessageViewer : UserControl, IMainScreenComponent
    {        
        public UcFastMessageViewer()
        {
            InitializeComponent();         
        }

        public void UseData(MainScreenComponentArgs args)
        {
            GetMessages(args);
        }

        private void GetMessages(MainScreenComponentArgs args)
        {            
            if (args.Conversation == null)
                return;

            Cursor = Cursors.WaitCursor;          

            olvFast.Objects = Manager.Instance.GetMessages(args.Conversation, args.DtFrom, args.DtTo).OrderByDescending(x => x.Date).ToList();

            Cursor = Cursors.Default;
        }
    }
}
