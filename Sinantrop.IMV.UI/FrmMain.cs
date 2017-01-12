using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using Sinantrop.IMV.Sync;
using Sinantrop.IMV.UI.Components;
using Sinantrop.IMV.ViewModels;

namespace Sinantrop.IMV.UI
{
    public partial class FrmMain : Form
    {
        IMainScreenComponent _mainScreenComponent;

        public FrmMain()
        {
            InitializeComponent();

            CreateMainComponent(typeof(UcFastMessageViewer));            

            ucUserList1.SelectedUserChanged += UcUserList1_SelectedUserChanged;
            ucConversationList1.SelectedConversationChanged += UcConversationList1_SelectedConversationChanged;

            ConfigureSyncButton();
        }

        private void ConfigureSyncButton()
        {
            _btnSync.Visible = false;            
            string location = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            if (location != null)
            {
                DirectoryInfo dir = new DirectoryInfo(location);
                if (dir.Exists)
                {
                    FileInfo[] files = dir.GetFiles("Sinantrop.IMV.Downloader.exe", SearchOption.TopDirectoryOnly);
                    _btnSync.Visible = files.Length > 0;
                }
            }
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            ucUserList1.LoadUsers();
        }

        private void UcUserList1_SelectedUserChanged(UiUser user)
        {
            ucConversationList1.LoadConversations(user);
        }

        private void UcConversationList1_SelectedConversationChanged(Conversation conversation)
        {
            SetMainComponentData();
        }

        private void _btnCharts_Click(object sender, EventArgs e)
        {
            CreateMainComponent(typeof(UcDoubleChart));
            SetMainComponentData();
            
        }

        private void _btnMessages_Click(object sender, EventArgs e)
        {
            CreateMainComponent(typeof(UcFastMessageViewer));
            SetMainComponentData();            
        }

        private void SetMainComponentData()
        {
            if (_mainScreenComponent != null)
            {
                Conversation conversation = ucConversationList1.SelectedConversation;
                if (conversation != null)
                {
                    MainScreenComponentArgs args = CreateMainComponentArgs(conversation);
                    _mainScreenComponent.UseData(args);
                }                
            }
        }

        private MainScreenComponentArgs CreateMainComponentArgs(Conversation conversation)
        {
            if (conversation == null)
                return new MainScreenComponentArgs() { Conversation = new Conversation() };

            return new MainScreenComponentArgs() { Conversation = conversation, DtFrom = ucFromToFilter1.DtFrom, DtTo = ucFromToFilter1.DtTo };
        }


        private void CreateMainComponent(Type type)
        {
            label1.Text = type == typeof(UcChart) ? string.Empty : $"INFO: Max {Manager.MaxMessages} messages at once";

            Control removeMe = null;
            foreach (Control control in splitContainer5.Panel1.Controls)
            {
                if ((control as IMainScreenComponent) != null)
                {
                    removeMe = control;
                    break;
                }
            }

            if (removeMe != null)
                splitContainer5.Panel1.Controls.Remove(removeMe);


            _mainScreenComponent = Activator.CreateInstance(type) as IMainScreenComponent;
            Control ctrl = _mainScreenComponent as Control;
            if (ctrl != null)
            {
                ctrl.Dock = DockStyle.Fill;
                splitContainer5.Panel1.Controls.Add(ctrl);
            }            
        }

        private void _btnSync_Click(object sender, EventArgs e)
        {
            
            string location = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            if (location != null)
            {
                Process p = new Process();
                string fullpath = Path.Combine(location, "Sinantrop.IMV.Downloader.exe");

                p.StartInfo = new ProcessStartInfo(fullpath) {Arguments = DropboxOAuthProvider.GetOauth()};
                p.Start();
                p.WaitForExit();

                Manager.Instance.Reinitialize();
                ucUserList1.LoadUsers();
            }
            
        }
    }
}
