namespace Sinantrop.IMV.UI.Components
{
    partial class UcConversationList
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this._lstConversations = new System.Windows.Forms.ListBox();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this._lstConversations);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(150, 146);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Conversations, groups, contacts";
            // 
            // _lstConversations
            // 
            this._lstConversations.Dock = System.Windows.Forms.DockStyle.Fill;
            this._lstConversations.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._lstConversations.FormattingEnabled = true;
            this._lstConversations.ItemHeight = 16;
            this._lstConversations.Location = new System.Drawing.Point(3, 16);
            this._lstConversations.Name = "_lstConversations";
            this._lstConversations.Size = new System.Drawing.Size(144, 127);
            this._lstConversations.TabIndex = 0;
            this._lstConversations.SelectedIndexChanged += new System.EventHandler(this._lstConversations_SelectedIndexChanged);
            // 
            // UcConversationList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox2);
            this.Name = "UcConversationList";
            this.Size = new System.Drawing.Size(150, 146);
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ListBox _lstConversations;
    }
}
