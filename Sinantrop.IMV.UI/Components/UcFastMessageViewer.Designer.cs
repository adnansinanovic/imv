namespace Sinantrop.IMV.UI.Components
{
    partial class UcFastMessageViewer
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
            this.olvFast = new BrightIdeasSoftware.FastObjectListView();
            this.olvColumn1 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumn2 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumn3 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            ((System.ComponentModel.ISupportInitialize)(this.olvFast)).BeginInit();
            this.SuspendLayout();
            // 
            // olvFast
            // 
            this.olvFast.AllColumns.Add(this.olvColumn1);
            this.olvFast.AllColumns.Add(this.olvColumn2);
            this.olvFast.AllColumns.Add(this.olvColumn3);
            this.olvFast.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.olvFast.CellEditActivation = BrightIdeasSoftware.ObjectListView.CellEditActivateMode.DoubleClick;
            this.olvFast.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.olvColumn1,
            this.olvColumn2,
            this.olvColumn3});
            this.olvFast.Cursor = System.Windows.Forms.Cursors.Default;
            this.olvFast.EmptyListMsg = "Empty";
            this.olvFast.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.olvFast.FullRowSelect = true;
            this.olvFast.GridLines = true;
            this.olvFast.HideSelection = false;
            this.olvFast.Location = new System.Drawing.Point(0, 0);
            this.olvFast.Name = "olvFast";
            this.olvFast.OwnerDraw = true;
            this.olvFast.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.olvFast.SelectColumnsOnRightClick = false;
            this.olvFast.SelectColumnsOnRightClickBehaviour = BrightIdeasSoftware.ObjectListView.ColumnSelectBehaviour.None;
            this.olvFast.SelectedColumnTint = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))), ((int)(((byte)(0)))));
            this.olvFast.ShowGroups = false;
            this.olvFast.Size = new System.Drawing.Size(150, 150);
            this.olvFast.SpaceBetweenGroups = 20;
            this.olvFast.TabIndex = 0;
            this.olvFast.UseAlternatingBackColors = true;
            this.olvFast.UseCompatibleStateImageBehavior = false;
            this.olvFast.View = System.Windows.Forms.View.Details;
            this.olvFast.VirtualMode = true;
            // 
            // olvColumn1
            // 
            this.olvColumn1.AspectName = "Date";
            this.olvColumn1.AspectToStringFormat = "{0:yyyy/MM/dd HH:mm:ss}";
            this.olvColumn1.Groupable = false;
            this.olvColumn1.IsEditable = false;
            this.olvColumn1.Searchable = false;
            this.olvColumn1.Text = "Date";
            this.olvColumn1.UseFiltering = false;
            this.olvColumn1.Width = 120;
            // 
            // olvColumn2
            // 
            this.olvColumn2.AspectName = "Author";
            this.olvColumn2.Groupable = false;
            this.olvColumn2.IsEditable = false;
            this.olvColumn2.Searchable = false;
            this.olvColumn2.Sortable = false;
            this.olvColumn2.Text = "Author";
            this.olvColumn2.UseFiltering = false;
            this.olvColumn2.Width = 120;
            // 
            // olvColumn3
            // 
            this.olvColumn3.AspectName = "Content";
            this.olvColumn3.FillsFreeSpace = true;
            this.olvColumn3.Groupable = false;
            this.olvColumn3.IsEditable = false;
            this.olvColumn3.MinimumWidth = 120;
            this.olvColumn3.Searchable = false;
            this.olvColumn3.Sortable = false;
            this.olvColumn3.Text = "Content";
            this.olvColumn3.UseFiltering = false;
            this.olvColumn3.Width = 120;
            // 
            // UcFastMessageViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.olvFast);
            this.Name = "UcFastMessageViewer";
            ((System.ComponentModel.ISupportInitialize)(this.olvFast)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private BrightIdeasSoftware.FastObjectListView olvFast;
        private BrightIdeasSoftware.OLVColumn olvColumn1;
        private BrightIdeasSoftware.OLVColumn olvColumn2;
        private BrightIdeasSoftware.OLVColumn olvColumn3;
    }
}
