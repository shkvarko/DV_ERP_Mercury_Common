namespace ERP_Mercury.Common
{
    partial class frmSelectAddressForRtt
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.treeListAddress = new DevExpress.XtraTreeList.TreeList();
            this.colFullAddress = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.repItemMemoEditAddress = new DevExpress.XtraEditors.Repository.RepositoryItemMemoEdit();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.btnSave = new DevExpress.XtraEditors.SimpleButton();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.treeListAddress)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repItemMemoEditAddress)).BeginInit();
            this.tableLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.treeListAddress, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 27F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(401, 189);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // treeListAddress
            // 
            this.treeListAddress.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.treeListAddress.Columns.AddRange(new DevExpress.XtraTreeList.Columns.TreeListColumn[] {
            this.colFullAddress});
            this.treeListAddress.Location = new System.Drawing.Point(1, 1);
            this.treeListAddress.Margin = new System.Windows.Forms.Padding(1);
            this.treeListAddress.Name = "treeListAddress";
            this.treeListAddress.OptionsBehavior.Editable = false;
            this.treeListAddress.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repItemMemoEditAddress});
            this.treeListAddress.Size = new System.Drawing.Size(399, 160);
            this.treeListAddress.TabIndex = 1;
            this.treeListAddress.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.treeListAddress_MouseDoubleClick);
            // 
            // colFullAddress
            // 
            this.colFullAddress.Caption = "Адрес";
            this.colFullAddress.ColumnEdit = this.repItemMemoEditAddress;
            this.colFullAddress.FieldName = "Адрес";
            this.colFullAddress.MinWidth = 50;
            this.colFullAddress.Name = "colFullAddress";
            this.colFullAddress.OptionsColumn.AllowEdit = false;
            this.colFullAddress.OptionsColumn.AllowFocus = false;
            this.colFullAddress.OptionsColumn.AllowMove = false;
            this.colFullAddress.OptionsColumn.AllowSize = false;
            this.colFullAddress.OptionsColumn.ReadOnly = true;
            this.colFullAddress.Visible = true;
            this.colFullAddress.VisibleIndex = 0;
            // 
            // repItemMemoEditAddress
            // 
            this.repItemMemoEditAddress.Name = "repItemMemoEditAddress";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 27F));
            this.tableLayoutPanel2.Controls.Add(this.btnCancel, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.btnSave, 0, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 162);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(401, 27);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Image = global::ERP_Mercury.Common.Properties.Resources.cancel_16;
            this.btnCancel.Location = new System.Drawing.Point(375, 1);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(1);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(25, 25);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.ToolTip = "Отменить изменения";
            this.btnCancel.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Information;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Image = global::ERP_Mercury.Common.Properties.Resources.ok_16;
            this.btnSave.Location = new System.Drawing.Point(348, 1);
            this.btnSave.Margin = new System.Windows.Forms.Padding(1);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(25, 25);
            this.btnSave.TabIndex = 5;
            this.btnSave.ToolTip = "Подтвердить изменения";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // frmSelectAddressForRtt
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(401, 189);
            this.Controls.Add(this.tableLayoutPanel1);
            this.MinimumSize = new System.Drawing.Size(400, 200);
            this.Name = "frmSelectAddressForRtt";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Список адресов";
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.treeListAddress)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repItemMemoEditAddress)).EndInit();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private DevExpress.XtraEditors.SimpleButton btnSave;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraTreeList.TreeList treeListAddress;
        private DevExpress.XtraTreeList.Columns.TreeListColumn colFullAddress;
        private DevExpress.XtraEditors.Repository.RepositoryItemMemoEdit repItemMemoEditAddress;
    }
}