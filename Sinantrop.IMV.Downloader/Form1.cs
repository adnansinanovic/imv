using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Sinantrop.IMV.Sync;

namespace Sinantrop.IMV.Downloader
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            label1.Text = @"Sync or Exit";
            tableLayoutPanel2.BackColor = Color.FromArgb(255, 255, 0, 0);
            label1.BackColor = Color.Red;
        }

        private async void _btnDownload_Click(object sender, EventArgs e)
        {

            try
            {
                _btnDownload.Enabled = false;                
                var files = await DownloaderFileDiff.ListDifference();
              
                var down = new Sync.Downloader(files.ToList(), Callback);
                down.StartDownloading();                                           
            }
            catch (Exception ex)
            {
                Error.Write(ex);
            }            
        }

        private void Callback(CallbackArgs obj)
        {
            _btnExit.Enabled = true;
            if (InvokeRequired)
                Invoke(new Action<CallbackArgs>(Callback), obj);
            else
            {
                label1.Text = obj.Message;
                if (obj.TotalItems < 0 || obj.CurrentItem < 0)
                    return;

                progressBar1.Maximum = obj.TotalItems;
                progressBar1.Value = obj.CurrentItem;
                
                if (obj.TotalItems == obj.CurrentItem)
                {
                    string message = obj.TotalItems == 0 ? obj.Message : @"Sync completed";
                    MessageBox.Show(this, message, @"Info", MessageBoxButtons.OK);                    
                    Close();
                }
            }
        }

        private void _btnExit_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
