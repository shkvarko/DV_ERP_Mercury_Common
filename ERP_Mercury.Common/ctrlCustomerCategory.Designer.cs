namespace ERP_Mercury.Common
{
    partial class ctrlCustomerCategory
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
            this.tableLayoutPanelBgrnd = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.btnSave = new DevExpress.XtraEditors.SimpleButton();
            this.btnEdit = new DevExpress.XtraEditors.SimpleButton();
            this.btnPrint = new DevExpress.XtraEditors.SimpleButton();
            this.lblCustomerIfo = new DevExpress.XtraEditors.LabelControl();
            this.treeList = new DevExpress.XtraTreeList.TreeList();
            this.colCompany = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.repItemCheckEdit = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.tableLayoutPanelBgrnd.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.treeList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repItemCheckEdit)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanelBgrnd
            // 
            this.tableLayoutPanelBgrnd.ColumnCount = 1;
            this.tableLayoutPanelBgrnd.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelBgrnd.Controls.Add(this.tableLayoutPanel1, 0, 2);
            this.tableLayoutPanelBgrnd.Controls.Add(this.lblCustomerIfo, 0, 0);
            this.tableLayoutPanelBgrnd.Controls.Add(this.treeList, 0, 1);
            this.tableLayoutPanelBgrnd.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelBgrnd.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelBgrnd.Name = "tableLayoutPanelBgrnd";
            this.tableLayoutPanelBgrnd.RowCount = 3;
            this.tableLayoutPanelBgrnd.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 23F));
            this.tableLayoutPanelBgrnd.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelBgrnd.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.tableLayoutPanelBgrnd.Size = new System.Drawing.Size(485, 529);
            this.tableLayoutPanelBgrnd.TabIndex = 0;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 89F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 82F));
            this.tableLayoutPanel1.Controls.Add(this.btnCancel, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnSave, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnEdit, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnPrint, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 497);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(485, 32);
            this.tableLayoutPanel1.TabIndex = 5;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Image = global::ERP_Mercury.Common.Properties.Resources.cancel_16;
            this.btnCancel.Location = new System.Drawing.Point(407, 6);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Отмена";
            this.btnCancel.ToolTip = "Отменить изменения";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Image = global::ERP_Mercury.Common.Properties.Resources.diskette_16;
            this.btnSave.Location = new System.Drawing.Point(325, 6);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 2;
            this.btnSave.Text = "ОК";
            this.btnSave.ToolTip = "Сохранить изменения";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnEdit
            // 
            this.btnEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnEdit.Image = global::ERP_Mercury.Common.Properties.Resources.document_unlock_16;
            this.btnEdit.Location = new System.Drawing.Point(3, 6);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(83, 23);
            this.btnEdit.TabIndex = 0;
            this.btnEdit.Text = "Изменить";
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // btnPrint
            // 
            this.btnPrint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnPrint.Image = global::ERP_Mercury.Common.Properties.Resources.printer2;
            this.btnPrint.Location = new System.Drawing.Point(92, 6);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(23, 23);
            this.btnPrint.TabIndex = 1;
            // 
            // lblCustomerIfo
            // 
            this.lblCustomerIfo.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblCustomerIfo.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.lblCustomerIfo.Appearance.Options.UseFont = true;
            this.lblCustomerIfo.Location = new System.Drawing.Point(3, 5);
            this.lblCustomerIfo.Name = "lblCustomerIfo";
            this.lblCustomerIfo.Size = new System.Drawing.Size(75, 13);
            this.lblCustomerIfo.TabIndex = 6;
            this.lblCustomerIfo.Text = "labelControl1";
            // 
            // treeList
            // 
            this.treeList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.treeList.Columns.AddRange(new DevExpress.XtraTreeList.Columns.TreeListColumn[] {
            this.colCompany});
            this.treeList.Location = new System.Drawing.Point(3, 26);
            this.treeList.Name = "treeList";
            this.treeList.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repItemCheckEdit});
            this.treeList.Size = new System.Drawing.Size(479, 468);
            this.treeList.TabIndex = 7;
            this.treeList.MouseClick += new System.Windows.Forms.MouseEventHandler(this.treeList_MouseClick);
            this.treeList.CellValueChanging += new DevExpress.XtraTreeList.CellValueChangedEventHandler(this.treeList_CellValueChanging);
            // 
            // colCompany
            // 
            this.colCompany.AppearanceHeader.Options.UseTextOptions = true;
            this.colCompany.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colCompany.Caption = "Компания";
            this.colCompany.FieldName = "Компания";
            this.colCompany.MinWidth = 50;
            this.colCompany.Name = "colCompany";
            this.colCompany.OptionsColumn.AllowEdit = false;
            this.colCompany.OptionsColumn.AllowFocus = false;
            this.colCompany.OptionsColumn.AllowSort = false;
            this.colCompany.OptionsColumn.ReadOnly = true;
            this.colCompany.Visible = true;
            this.colCompany.VisibleIndex = 0;
            // 
            // repItemCheckEdit
            // 
            this.repItemCheckEdit.AutoHeight = false;
            this.repItemCheckEdit.Name = "repItemCheckEdit";
            // 
            // ctrlCustomerCategory
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanelBgrnd);
            this.Name = "ctrlCustomerCategory";
            this.Size = new System.Drawing.Size(485, 529);
            this.tableLayoutPanelBgrnd.ResumeLayout(false);
            this.tableLayoutPanelBgrnd.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.treeList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repItemCheckEdit)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelBgrnd;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraEditors.SimpleButton btnSave;
        private DevExpress.XtraEditors.SimpleButton btnEdit;
        private DevExpress.XtraEditors.SimpleButton btnPrint;
        private DevExpress.XtraEditors.LabelControl lblCustomerIfo;
        private DevExpress.XtraTreeList.TreeList treeList;
        private DevExpress.XtraTreeList.Columns.TreeListColumn colCompany;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit repItemCheckEdit;
    }
}
