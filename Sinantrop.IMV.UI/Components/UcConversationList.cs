using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Sinantrop.IMV.ViewModels;

namespace Sinantrop.IMV.UI.Components
{
    public partial class UcConversationList : UserControl
    {
        public delegate void SelectedConversationChangedHandler(Conversation conversation);
        public event SelectedConversationChangedHandler SelectedConversationChanged;

        readonly Manager _manager;
        private Conversation _previousConversation;
        public UcConversationList()
        {
            InitializeComponent();

            _manager = Manager.Instance;
        }

        public void LoadConversations(UiUser user)
        {
            Cursor = Cursors.WaitCursor;

            List<Conversation> conversations = _manager.GetConversation(user).OrderBy(x => x.Title).ToList();

            _lstConversations.DataSource = null;
            _lstConversations.DataSource = conversations;
            _lstConversations.DisplayMember = "Title";
            _lstConversations.ValueMember = "Id";

            if (_lstConversations.Items.Count > 0)
            _lstConversations.SetSelected(0, true);

            Cursor = Cursors.Default;
        }

        private void _lstConversations_SelectedIndexChanged(object sender, EventArgs e)
        {
            Conversation conversation = _lstConversations.SelectedItem as Conversation;

            if (conversation == null)
                return;


            bool isChanged = _previousConversation == null
                             || _previousConversation.MessengerType != conversation.MessengerType
                             || _previousConversation.Id != conversation.Id;
            
            if (!isChanged)
                return;

            _previousConversation = conversation;

            SelectedConversationChanged?.Invoke(conversation);
        }

        public Conversation SelectedConversation { get { return _lstConversations.SelectedItem as Conversation; } }
    }
}
