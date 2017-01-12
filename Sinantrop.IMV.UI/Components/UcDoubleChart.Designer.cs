namespace Sinantrop.IMV.UI.Components
{
    partial class UcDoubleChart
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.ucChart1 = new UcChart();
            this.ucChart2 = new UcChart();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.ucChart1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.ucChart2);
            this.splitContainer1.Size = new System.Drawing.Size(600, 244);
            this.splitContainer1.SplitterDistance = 300;
            this.splitContainer1.SplitterWidth = 2;
            this.splitContainer1.TabIndex = 0;
            // 
            // ucChart1
            // 
            this.ucChart1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucChart1.Location = new System.Drawing.Point(0, 0);
            this.ucChart1.Name = "ucChart1";
            this.ucChart1.Size = new System.Drawing.Size(300, 244);
            this.ucChart1.TabIndex = 0;
            // 
            // ucChart2
            // 
            this.ucChart2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucChart2.Location = new System.Drawing.Point(0, 0);
            this.ucChart2.Name = "ucChart2";
            this.ucChart2.Size = new System.Drawing.Size(298, 244);
            this.ucChart2.TabIndex = 0;
            // 
            // UcDoubleChart
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Name = "UcDoubleChart";
            this.Size = new System.Drawing.Size(600, 244);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private UcChart ucChart1;
        private UcChart ucChart2;
    }
}
