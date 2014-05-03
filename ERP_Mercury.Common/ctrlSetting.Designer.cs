namespace ERP_Mercury.Common
{
    partial class ctrlSetting
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.treeListParams = new DevExpress.XtraTreeList.TreeList();
            this.colParamName = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.colParamValue = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.btnSave = new DevExpress.XtraEditors.SimpleButton();
            this.btnEdit = new DevExpress.XtraEditors.SimpleButton();
            this.btnPrint = new DevExpress.XtraEditors.SimpleButton();
            this.lblSettingName = new DevExpress.XtraEditors.LabelControl();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.treeListParams)).BeginInit();
            this.tableLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.treeListParams, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.lblSettingName, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 23F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 34F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(522, 507);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // treeListParams
            // 
            this.treeListParams.Columns.AddRange(new DevExpress.XtraTreeList.Columns.TreeListColumn[] {
            this.colParamName,
            this.colParamValue});
            this.treeListParams.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeListParams.Location = new System.Drawing.Point(3, 26);
            this.treeListParams.Name = "treeListParams";
            this.treeListParams.Size = new System.Drawing.Size(516, 444);
            this.treeListParams.TabIndex = 11;
            this.treeListParams.BeforeFocusNode += new DevExpress.XtraTreeList.BeforeFocusNodeEventHandler(this.treeListParams_BeforeFocusNode);
            this.treeListParams.CellValueChanging += new DevExpress.XtraTreeList.CellValueChangedEventHandler(this.treeListParams_CellValueChanging);
            // 
            // colParamName
            // 
            this.colParamName.Caption = "Имя";
            this.colParamName.FieldName = "Имя";
            this.colParamName.MinWidth = 50;
            this.colParamName.Name = "colParamName";
            this.colParamName.OptionsColumn.AllowEdit = false;
            this.colParamName.OptionsColumn.AllowFocus = false;
            this.colParamName.OptionsColumn.AllowMove = false;
            this.colParamName.OptionsColumn.ReadOnly = true;
            this.colParamName.Visible = true;
            this.colParamName.VisibleIndex = 0;
            this.colParamName.Width = 171;
            // 
            // colParamValue
            // 
            this.colParamValue.Caption = "Значение";
            this.colParamValue.FieldName = "Значение";
            this.colParamValue.MinWidth = 50;
            this.colParamValue.Name = "colParamValue";
            this.colParamValue.Visible = true;
            this.colParamValue.VisibleIndex = 1;
            this.colParamValue.Width = 324;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 4;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 89F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 82F));
            this.tableLayoutPanel2.Controls.Add(this.btnCancel, 3, 0);
            this.tableLayoutPanel2.Controls.Add(this.btnSave, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.btnEdit, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.btnPrint, 1, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 474);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(522, 33);
            this.tableLayoutPanel2.TabIndex = 10;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Image = global::ERP_Mercury.Common.Properties.Resources.cancel_16;
            this.btnCancel.Location = new System.Drawing.Point(444, 7);
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
            this.btnSave.Image = global::ERP_Mercury.Common.Properties.Resources.disk_blue_ok;
            this.btnSave.Location = new System.Drawing.Point(362, 7);
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
            this.btnEdit.Image = global::ERP_Mercury.Common.Properties.Resources.document_write_16;
            this.btnEdit.Location = new System.Drawing.Point(3, 7);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(83, 23);
            this.btnEdit.TabIndex = 0;
            this.btnEdit.Text = "Изменить";
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // btnPrint
            // 
            this.btnPrint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnPrint.Location = new System.Drawing.Point(92, 7);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(23, 23);
            this.btnPrint.TabIndex = 1;
            // 
            // lblSettingName
            // 
            this.lblSettingName.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblSettingName.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.lblSettingName.Appearance.Options.UseFont = true;
            this.lblSettingName.Location = new System.Drawing.Point(3, 5);
            this.lblSettingName.Name = "lblSettingName";
            this.lblSettingName.Size = new System.Drawing.Size(75, 13);
            this.lblSettingName.TabIndex = 12;
            this.lblSettingName.Text = "labelControl1";
            // 
            // ctrlSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "ctrlSetting";
            this.Size = new System.Drawing.Size(522, 507);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.treeListParams)).EndInit();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraEditors.SimpleButton btnSave;
        private DevExpress.XtraEditors.SimpleButton btnEdit;
        private DevExpress.XtraEditors.SimpleButton btnPrint;
        private DevExpress.XtraTreeList.TreeList treeListParams;
        private DevExpress.XtraTreeList.Columns.TreeListColumn colParamName;
        private DevExpress.XtraTreeList.Columns.TreeListColumn colParamValue;
        private DevExpress.XtraEditors.LabelControl lblSettingName;
    }
}
