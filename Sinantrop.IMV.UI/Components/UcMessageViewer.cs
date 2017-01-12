using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using Sinantrop.Helper;
using Message = Sinantrop.IMV.ViewModels.Message;

namespace Sinantrop.IMV.UI.Components
{
    public partial class UcMessageViewer : UserControl, IMainScreenComponent
    {
        readonly BindingList<Message> _filteredMessages = new BindingList<Message>();
        List<Message> _messages = new List<Message>();
        readonly Manager _manger;

        public UcMessageViewer()
        {
            InitializeComponent();

            
           
            _manger = Manager.Instance;
        }

        public void UseData(MainScreenComponentArgs args)
        {            
            GetMessages(args);
        }

     

        private void GetMessages(MainScreenComponentArgs args)
        {
            _dgv.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing; //or even better .DisableResizing. Most time consumption enum is DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders

            // set it to false if not needed
            _dgv.RowHeadersVisible = false;

            if (args.Conversation == null)
                return;

            Cursor = Cursors.WaitCursor;

            ClearMessages();            
            
            _messages = _manger.GetMessages(args.Conversation, args.DtFrom, args.DtTo).OrderByDescending(x => x.Date).ToList();

            FilterMessages(args);

            _dgv.DoubleBuffered(true);
            _dgv.DataSource = _filteredMessages;

            _dgv.Columns["Content"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            _dgv.Columns["Timestamp"].Visible = false;
            _dgv.Columns["ConversationId"].Visible = false;
            _dgv.Columns["Id"].Visible = false;
            _dgv.Columns["MessengerType"].Visible = false;
            _dgv.Columns["Username"].Visible = false;
            _dgv.Columns["Identity"].Visible = false;            

            Cursor = Cursors.Default;
        }

        private void ClearMessages()
        {
            _dgv.DataSource = null;
            _filteredMessages.Clear();
            _messages.Clear();
        }       

        private void FilterMessages(MainScreenComponentArgs args)
        {
            _filteredMessages.Clear();

            foreach (var item in _messages)
            {
                if (args.FilterUrls)
                {
                    string url = item.Content.GetUrl();
                    if (!string.IsNullOrEmpty(url))
                    {
                        //we want to extract url from content, and show only url

                        _filteredMessages.Add(new Message()
                        {
                            Author = item.Author,
                            ConversationId = item.ConversationId,
                            Id = item.Id,
                            MessengerType = item.MessengerType,
                            Timestamp = item.Timestamp,
                            Username = item.Username,
                            Content = url
                        });
                    }
                }
                else
                    _filteredMessages.Add(item);
            }
        }       
    }
}
