using System;
using System.Windows.Forms;
using Sinantrop.IMV.ViewModels;

namespace Sinantrop.IMV.UI.Components
{
    public partial class UcUserList : UserControl
    {        
        public event Action<UiUser> SelectedUserChanged;

        readonly Manager _manager;
        public UcUserList()
        {
            InitializeComponent();

            _manager = Manager.Instance;
        }

        public void LoadUsers()
        {
            Cursor = Cursors.WaitCursor;

            _lstUsers.DataSource = _manager.GetUsers();

            _lstUsers.DisplayMember = "Display";
            _lstUsers.ValueMember = "Username";
            if (_lstUsers.Items.Count > 0)
            _lstUsers.SetSelected(0, true);

            Cursor = Cursors.Default;
        }

        private void _lstUsers_SelectedIndexChanged(object sender, EventArgs e)
        {
            UiUser user = _lstUsers.SelectedItem as UiUser;

            if (SelectedUserChanged != null && user != null)
                SelectedUserChanged(user);
        }
    }
}
