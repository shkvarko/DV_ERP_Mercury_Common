namespace ERP_Mercury.Common
{
    partial class ctrlAddress
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
            this.components = new System.ComponentModel.Container();
            this.toolTipController = new DevExpress.Utils.ToolTipController(this.components);
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuRefresh = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.menuAdd = new System.Windows.Forms.ToolStripMenuItem();
            this.menuEdit = new System.Windows.Forms.ToolStripMenuItem();
            this.menuDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.splitContainerControl = new DevExpress.XtraEditors.SplitContainerControl();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.treeListAddress = new DevExpress.XtraTreeList.TreeList();
            this.colFullAddress = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.repItemMemoEditAddress = new DevExpress.XtraEditors.Repository.RepositoryItemMemoEdit();
            this.xtraScrollableControl1 = new DevExpress.XtraEditors.XtraScrollableControl();
            this.txtDescription = new DevExpress.XtraEditors.MemoEdit();
            this.cboxOblast = new DevExpress.XtraEditors.ComboBoxEdit();
            this.btnSelectAddress = new DevExpress.XtraEditors.SimpleButton();
            this.txtFlatCode = new DevExpress.XtraEditors.TextEdit();
            this.cboxAddressType = new DevExpress.XtraEditors.ComboBoxEdit();
            this.cboxFlat = new DevExpress.XtraEditors.ComboBoxEdit();
            this.lblPostIndex = new DevExpress.XtraEditors.LabelControl();
            this.txtSubBuildingCode = new DevExpress.XtraEditors.TextEdit();
            this.cboxCountry = new DevExpress.XtraEditors.ComboBoxEdit();
            this.cboxSubBuilding = new DevExpress.XtraEditors.ComboBoxEdit();
            this.txtPostIndex = new DevExpress.XtraEditors.TextEdit();
            this.txtBuildingCode = new DevExpress.XtraEditors.TextEdit();
            this.cboxRegion = new DevExpress.XtraEditors.ComboBoxEdit();
            this.cboxBuilding = new DevExpress.XtraEditors.ComboBoxEdit();
            this.cboxCity = new DevExpress.XtraEditors.ComboBoxEdit();
            this.cboxStreet = new DevExpress.XtraEditors.ComboBoxEdit();
            this.cboxAddressPrefix = new DevExpress.XtraEditors.ComboBoxEdit();
            this.tableLayoutPanel6 = new System.Windows.Forms.TableLayoutPanel();
            this.btnSave = new DevExpress.XtraEditors.SimpleButton();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.btnAddAddress = new DevExpress.XtraEditors.SimpleButton();
            this.btnEditAddress = new DevExpress.XtraEditors.SimpleButton();
            this.btnDeleteAddress = new DevExpress.XtraEditors.SimpleButton();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.contextMenuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl)).BeginInit();
            this.splitContainerControl.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.treeListAddress)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repItemMemoEditAddress)).BeginInit();
            this.xtraScrollableControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtDescription.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboxOblast.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFlatCode.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboxAddressType.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboxFlat.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSubBuildingCode.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboxCountry.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboxSubBuilding.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPostIndex.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBuildingCode.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboxRegion.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboxBuilding.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboxCity.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboxStreet.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboxAddressPrefix.Properties)).BeginInit();
            this.tableLayoutPanel6.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuRefresh,
            this.toolStripSeparator1,
            this.menuAdd,
            this.menuEdit,
            this.menuDelete});
            this.contextMenuStrip.Name = "contextMenuStrip";
            this.contextMenuStrip.Size = new System.Drawing.Size(155, 98);
            this.toolTipController.SetSuperTip(this.contextMenuStrip, null);
            // 
            // menuRefresh
            // 
            this.menuRefresh.Name = "menuRefresh";
            this.menuRefresh.Size = new System.Drawing.Size(154, 22);
            this.menuRefresh.Text = "Обновить";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(151, 6);
            // 
            // menuAdd
            // 
            this.menuAdd.Name = "menuAdd";
            this.menuAdd.Size = new System.Drawing.Size(154, 22);
            this.menuAdd.Text = "Новый адрес";
            this.menuAdd.Click += new System.EventHandler(this.menuAdd_Click);
            // 
            // menuEdit
            // 
            this.menuEdit.Name = "menuEdit";
            this.menuEdit.Size = new System.Drawing.Size(154, 22);
            this.menuEdit.Text = "Редактировать";
            this.menuEdit.Click += new System.EventHandler(this.menuEdit_Click);
            // 
            // menuDelete
            // 
            this.menuDelete.Name = "menuDelete";
            this.menuDelete.Size = new System.Drawing.Size(154, 22);
            this.menuDelete.Text = "Удалить";
            this.menuDelete.Click += new System.EventHandler(this.menuDelete_Click);
            // 
            // splitContainerControl
            // 
            this.splitContainerControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerControl.FixedPanel = DevExpress.XtraEditors.SplitFixedPanel.None;
            this.splitContainerControl.Location = new System.Drawing.Point(3, 30);
            this.splitContainerControl.Name = "splitContainerControl";
            this.splitContainerControl.Panel1.Controls.Add(this.tableLayoutPanel4);
            this.splitContainerControl.Panel1.MinSize = 200;
            this.splitContainerControl.Panel1.Text = "Panel1";
            this.splitContainerControl.Panel2.Controls.Add(this.xtraScrollableControl1);
            this.splitContainerControl.Panel2.Text = "Panel2";
            this.splitContainerControl.Size = new System.Drawing.Size(772, 135);
            this.splitContainerControl.SplitterPosition = 210;
            this.toolTipController.SetSuperTip(this.splitContainerControl, null);
            this.splitContainerControl.TabIndex = 0;
            this.splitContainerControl.Text = "splitContainerControl1";
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 1;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.Controls.Add(this.treeListAddress, 0, 0);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel4.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 1;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 139F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(206, 131);
            this.toolTipController.SetSuperTip(this.tableLayoutPanel4, null);
            this.tableLayoutPanel4.TabIndex = 1;
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
            this.treeListAddress.Size = new System.Drawing.Size(204, 129);
            this.treeListAddress.TabIndex = 0;
            this.treeListAddress.ToolTipController = this.toolTipController;
            this.treeListAddress.BeforeFocusNode += new DevExpress.XtraTreeList.BeforeFocusNodeEventHandler(this.treeListAddress_BeforeFocusNode);
            this.treeListAddress.FocusedNodeChanged += new DevExpress.XtraTreeList.FocusedNodeChangedEventHandler(this.treeListAddress_FocusedNodeChanged);
            this.treeListAddress.MouseClick += new System.Windows.Forms.MouseEventHandler(this.treeListAddress_MouseClick);
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
            // xtraScrollableControl1
            // 
            this.xtraScrollableControl1.Controls.Add(this.txtDescription);
            this.xtraScrollableControl1.Controls.Add(this.cboxOblast);
            this.xtraScrollableControl1.Controls.Add(this.btnSelectAddress);
            this.xtraScrollableControl1.Controls.Add(this.txtFlatCode);
            this.xtraScrollableControl1.Controls.Add(this.cboxAddressType);
            this.xtraScrollableControl1.Controls.Add(this.cboxFlat);
            this.xtraScrollableControl1.Controls.Add(this.lblPostIndex);
            this.xtraScrollableControl1.Controls.Add(this.txtSubBuildingCode);
            this.xtraScrollableControl1.Controls.Add(this.cboxCountry);
            this.xtraScrollableControl1.Controls.Add(this.cboxSubBuilding);
            this.xtraScrollableControl1.Controls.Add(this.txtPostIndex);
            this.xtraScrollableControl1.Controls.Add(this.txtBuildingCode);
            this.xtraScrollableControl1.Controls.Add(this.cboxRegion);
            this.xtraScrollableControl1.Controls.Add(this.cboxBuilding);
            this.xtraScrollableControl1.Controls.Add(this.cboxCity);
            this.xtraScrollableControl1.Controls.Add(this.cboxStreet);
            this.xtraScrollableControl1.Controls.Add(this.cboxAddressPrefix);
            this.xtraScrollableControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.xtraScrollableControl1.Location = new System.Drawing.Point(0, 0);
            this.xtraScrollableControl1.Name = "xtraScrollableControl1";
            this.xtraScrollableControl1.Size = new System.Drawing.Size(552, 131);
            this.toolTipController.SetSuperTip(this.xtraScrollableControl1, null);
            this.xtraScrollableControl1.TabIndex = 17;
            // 
            // txtDescription
            // 
            this.txtDescription.Location = new System.Drawing.Point(3, 85);
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(545, 40);
            this.txtDescription.TabIndex = 16;
            this.txtDescription.EditValueChanging += new DevExpress.XtraEditors.Controls.ChangingEventHandler(this.txtAddressProperties_EditValueChanging);
            // 
            // cboxOblast
            // 
            this.cboxOblast.Location = new System.Drawing.Point(102, 33);
            this.cboxOblast.Name = "cboxOblast";
            this.cboxOblast.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboxOblast.Properties.PopupSizeable = true;
            this.cboxOblast.Properties.Sorted = true;
            this.cboxOblast.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.cboxOblast.Size = new System.Drawing.Size(86, 20);
            this.cboxOblast.TabIndex = 3;
            this.cboxOblast.ToolTip = "Область";
            this.cboxOblast.ToolTipController = this.toolTipController;
            this.cboxOblast.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Information;
            this.cboxOblast.EditValueChanged += new System.EventHandler(this.cboxOblast_EditValueChanged);
            // 
            // btnSelectAddress
            // 
            this.btnSelectAddress.Location = new System.Drawing.Point(141, 4);
            this.btnSelectAddress.Name = "btnSelectAddress";
            this.btnSelectAddress.Size = new System.Drawing.Size(46, 23);
            this.btnSelectAddress.TabIndex = 1;
            this.btnSelectAddress.Text = "...";
            this.btnSelectAddress.Click += new System.EventHandler(this.btnSelectAddress_Click);
            // 
            // txtFlatCode
            // 
            this.txtFlatCode.Location = new System.Drawing.Point(498, 59);
            this.txtFlatCode.Name = "txtFlatCode";
            this.txtFlatCode.Size = new System.Drawing.Size(50, 20);
            this.txtFlatCode.TabIndex = 15;
            this.txtFlatCode.ToolTip = "№ помещения";
            this.txtFlatCode.ToolTipController = this.toolTipController;
            this.txtFlatCode.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Information;
            this.txtFlatCode.EditValueChanging += new DevExpress.XtraEditors.Controls.ChangingEventHandler(this.txtAddressProperties_EditValueChanging);
            // 
            // cboxAddressType
            // 
            this.cboxAddressType.Location = new System.Drawing.Point(3, 7);
            this.cboxAddressType.Name = "cboxAddressType";
            this.cboxAddressType.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboxAddressType.Properties.PopupSizeable = true;
            this.cboxAddressType.Properties.Sorted = true;
            this.cboxAddressType.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.cboxAddressType.Size = new System.Drawing.Size(93, 20);
            this.cboxAddressType.TabIndex = 0;
            this.cboxAddressType.ToolTip = "Тип адреса";
            this.cboxAddressType.ToolTipController = this.toolTipController;
            this.cboxAddressType.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Information;
            this.cboxAddressType.SelectedValueChanged += new System.EventHandler(this.AddressProperties_SelectedValueChanged);
            // 
            // cboxFlat
            // 
            this.cboxFlat.Location = new System.Drawing.Point(440, 59);
            this.cboxFlat.Name = "cboxFlat";
            this.cboxFlat.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboxFlat.Properties.PopupSizeable = true;
            this.cboxFlat.Properties.Sorted = true;
            this.cboxFlat.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.cboxFlat.Size = new System.Drawing.Size(52, 20);
            this.cboxFlat.TabIndex = 14;
            this.cboxFlat.ToolTip = "Помещение";
            this.cboxFlat.ToolTipController = this.toolTipController;
            this.cboxFlat.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Information;
            this.cboxFlat.SelectedValueChanged += new System.EventHandler(this.AddressProperties_SelectedValueChanged);
            // 
            // lblPostIndex
            // 
            this.lblPostIndex.Location = new System.Drawing.Point(324, 36);
            this.lblPostIndex.Name = "lblPostIndex";
            this.lblPostIndex.Size = new System.Drawing.Size(41, 13);
            this.lblPostIndex.TabIndex = 5;
            this.lblPostIndex.Text = "Индекс:";
            // 
            // txtSubBuildingCode
            // 
            this.txtSubBuildingCode.Location = new System.Drawing.Point(397, 59);
            this.txtSubBuildingCode.Name = "txtSubBuildingCode";
            this.txtSubBuildingCode.Size = new System.Drawing.Size(37, 20);
            this.txtSubBuildingCode.TabIndex = 13;
            this.txtSubBuildingCode.ToolTip = "№ корпуса";
            this.txtSubBuildingCode.ToolTipController = this.toolTipController;
            this.txtSubBuildingCode.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Information;
            this.txtSubBuildingCode.EditValueChanging += new DevExpress.XtraEditors.Controls.ChangingEventHandler(this.txtAddressProperties_EditValueChanging);
            // 
            // cboxCountry
            // 
            this.cboxCountry.Location = new System.Drawing.Point(3, 33);
            this.cboxCountry.Name = "cboxCountry";
            this.cboxCountry.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboxCountry.Properties.PopupSizeable = true;
            this.cboxCountry.Properties.Sorted = true;
            this.cboxCountry.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.cboxCountry.Size = new System.Drawing.Size(93, 20);
            this.cboxCountry.TabIndex = 2;
            this.cboxCountry.ToolTip = "Страна";
            this.cboxCountry.ToolTipController = this.toolTipController;
            this.cboxCountry.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Information;
            this.cboxCountry.EditValueChanged += new System.EventHandler(this.cboxCountry_EditValueChanged);
            // 
            // cboxSubBuilding
            // 
            this.cboxSubBuilding.Location = new System.Drawing.Point(336, 59);
            this.cboxSubBuilding.Name = "cboxSubBuilding";
            this.cboxSubBuilding.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboxSubBuilding.Properties.PopupSizeable = true;
            this.cboxSubBuilding.Properties.Sorted = true;
            this.cboxSubBuilding.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.cboxSubBuilding.Size = new System.Drawing.Size(55, 20);
            this.cboxSubBuilding.TabIndex = 12;
            this.cboxSubBuilding.ToolTip = "Корпус";
            this.cboxSubBuilding.ToolTipController = this.toolTipController;
            this.cboxSubBuilding.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Information;
            this.cboxSubBuilding.SelectedValueChanged += new System.EventHandler(this.AddressProperties_SelectedValueChanged);
            // 
            // txtPostIndex
            // 
            this.txtPostIndex.Location = new System.Drawing.Point(371, 33);
            this.txtPostIndex.Name = "txtPostIndex";
            this.txtPostIndex.Size = new System.Drawing.Size(61, 20);
            this.txtPostIndex.TabIndex = 6;
            this.txtPostIndex.ToolTip = "Почтовый индекс";
            this.txtPostIndex.ToolTipController = this.toolTipController;
            this.txtPostIndex.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Information;
            this.txtPostIndex.EditValueChanging += new DevExpress.XtraEditors.Controls.ChangingEventHandler(this.txtPostIndex_EditValueChanging);
            // 
            // txtBuildingCode
            // 
            this.txtBuildingCode.Location = new System.Drawing.Point(290, 59);
            this.txtBuildingCode.Name = "txtBuildingCode";
            this.txtBuildingCode.Size = new System.Drawing.Size(40, 20);
            this.txtBuildingCode.TabIndex = 11;
            this.txtBuildingCode.ToolTip = "№ здания";
            this.txtBuildingCode.ToolTipController = this.toolTipController;
            this.txtBuildingCode.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Information;
            this.txtBuildingCode.EditValueChanging += new DevExpress.XtraEditors.Controls.ChangingEventHandler(this.txtAddressProperties_EditValueChanging);
            // 
            // cboxRegion
            // 
            this.cboxRegion.Location = new System.Drawing.Point(194, 33);
            this.cboxRegion.Name = "cboxRegion";
            this.cboxRegion.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboxRegion.Properties.PopupSizeable = true;
            this.cboxRegion.Properties.Sorted = true;
            this.cboxRegion.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.cboxRegion.Size = new System.Drawing.Size(124, 20);
            this.cboxRegion.TabIndex = 4;
            this.cboxRegion.ToolTip = "Район";
            this.cboxRegion.ToolTipController = this.toolTipController;
            this.cboxRegion.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Information;
            this.cboxRegion.EditValueChanged += new System.EventHandler(this.cboxRegion_EditValueChanged);
            // 
            // cboxBuilding
            // 
            this.cboxBuilding.Location = new System.Drawing.Point(232, 59);
            this.cboxBuilding.Name = "cboxBuilding";
            this.cboxBuilding.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboxBuilding.Properties.PopupSizeable = true;
            this.cboxBuilding.Properties.Sorted = true;
            this.cboxBuilding.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.cboxBuilding.Size = new System.Drawing.Size(52, 20);
            this.cboxBuilding.TabIndex = 10;
            this.cboxBuilding.ToolTip = "Дом";
            this.cboxBuilding.ToolTipController = this.toolTipController;
            this.cboxBuilding.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Information;
            this.cboxBuilding.SelectedValueChanged += new System.EventHandler(this.AddressProperties_SelectedValueChanged);
            // 
            // cboxCity
            // 
            this.cboxCity.Location = new System.Drawing.Point(438, 33);
            this.cboxCity.Name = "cboxCity";
            this.cboxCity.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboxCity.Properties.PopupSizeable = true;
            this.cboxCity.Properties.Sorted = true;
            this.cboxCity.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.cboxCity.Properties.EditValueChanged += new System.EventHandler(this.cboxCity_EditValueChanged);
            this.cboxCity.Size = new System.Drawing.Size(109, 20);
            this.cboxCity.TabIndex = 7;
            this.cboxCity.ToolTip = "Населенный пункт";
            this.cboxCity.ToolTipController = this.toolTipController;
            this.cboxCity.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Information;
            this.cboxCity.EditValueChanged += new System.EventHandler(this.cboxCity_EditValueChanged);
            // 
            // cboxStreet
            // 
            this.cboxStreet.Location = new System.Drawing.Point(61, 59);
            this.cboxStreet.Name = "cboxStreet";
            this.cboxStreet.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboxStreet.Properties.PopupSizeable = true;
            this.cboxStreet.Properties.Sorted = true;
            this.cboxStreet.Size = new System.Drawing.Size(165, 20);
            this.cboxStreet.TabIndex = 9;
            this.cboxStreet.ToolTip = "Улица";
            this.cboxStreet.ToolTipController = this.toolTipController;
            this.cboxStreet.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Information;
            this.cboxStreet.EditValueChanging += new DevExpress.XtraEditors.Controls.ChangingEventHandler(this.txtAddressProperties_EditValueChanging);
            // 
            // cboxAddressPrefix
            // 
            this.cboxAddressPrefix.Location = new System.Drawing.Point(3, 59);
            this.cboxAddressPrefix.Name = "cboxAddressPrefix";
            this.cboxAddressPrefix.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboxAddressPrefix.Properties.PopupSizeable = true;
            this.cboxAddressPrefix.Properties.Sorted = true;
            this.cboxAddressPrefix.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.cboxAddressPrefix.Size = new System.Drawing.Size(52, 20);
            this.cboxAddressPrefix.TabIndex = 8;
            this.cboxAddressPrefix.ToolTip = "Улица";
            this.cboxAddressPrefix.ToolTipController = this.toolTipController;
            this.cboxAddressPrefix.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Information;
            this.cboxAddressPrefix.SelectedValueChanged += new System.EventHandler(this.AddressProperties_SelectedValueChanged);
            // 
            // tableLayoutPanel6
            // 
            this.tableLayoutPanel6.ColumnCount = 5;
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 27F));
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 27F));
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 27F));
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 27F));
            this.tableLayoutPanel6.Controls.Add(this.btnSave, 3, 0);
            this.tableLayoutPanel6.Controls.Add(this.btnCancel, 4, 0);
            this.tableLayoutPanel6.Controls.Add(this.btnAddAddress, 0, 0);
            this.tableLayoutPanel6.Controls.Add(this.btnEditAddress, 1, 0);
            this.tableLayoutPanel6.Controls.Add(this.btnDeleteAddress, 2, 0);
            this.tableLayoutPanel6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel6.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel6.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel6.Name = "tableLayoutPanel6";
            this.tableLayoutPanel6.RowCount = 1;
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel6.Size = new System.Drawing.Size(778, 27);
            this.toolTipController.SetSuperTip(this.tableLayoutPanel6, null);
            this.tableLayoutPanel6.TabIndex = 1;
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSave.Image = global::ERP_Mercury.Common.Properties.Resources.diskette_16;
            this.btnSave.Location = new System.Drawing.Point(725, 1);
            this.btnSave.Margin = new System.Windows.Forms.Padding(1);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(25, 25);
            this.btnSave.TabIndex = 0;
            this.btnSave.ToolTip = "Подтвердить изменения";
            this.btnSave.ToolTipController = this.toolTipController;
            this.btnSave.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Information;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Image = global::ERP_Mercury.Common.Properties.Resources.cancel_16;
            this.btnCancel.Location = new System.Drawing.Point(752, 1);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(1);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(25, 25);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.ToolTip = "Отменить изменения";
            this.btnCancel.ToolTipController = this.toolTipController;
            this.btnCancel.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Information;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnAddAddress
            // 
            this.btnAddAddress.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAddAddress.Image = global::ERP_Mercury.Common.Properties.Resources.add_16;
            this.btnAddAddress.ImageAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.btnAddAddress.Location = new System.Drawing.Point(1, 1);
            this.btnAddAddress.Margin = new System.Windows.Forms.Padding(1);
            this.btnAddAddress.Name = "btnAddAddress";
            this.btnAddAddress.Size = new System.Drawing.Size(25, 25);
            this.btnAddAddress.TabIndex = 0;
            this.btnAddAddress.ToolTip = "Новый адрес";
            this.btnAddAddress.ToolTipController = this.toolTipController;
            this.btnAddAddress.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Information;
            this.btnAddAddress.Click += new System.EventHandler(this.btnAddAddress_Click);
            // 
            // btnEditAddress
            // 
            this.btnEditAddress.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnEditAddress.Image = global::ERP_Mercury.Common.Properties.Resources.document_unlock_16;
            this.btnEditAddress.ImageAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.btnEditAddress.Location = new System.Drawing.Point(28, 1);
            this.btnEditAddress.Margin = new System.Windows.Forms.Padding(1);
            this.btnEditAddress.Name = "btnEditAddress";
            this.btnEditAddress.Size = new System.Drawing.Size(25, 25);
            this.btnEditAddress.TabIndex = 1;
            this.btnEditAddress.ToolTip = "Редактировать адрес";
            this.btnEditAddress.ToolTipController = this.toolTipController;
            this.btnEditAddress.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Information;
            this.btnEditAddress.Click += new System.EventHandler(this.menuEdit_Click);
            // 
            // btnDeleteAddress
            // 
            this.btnDeleteAddress.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDeleteAddress.Image = global::ERP_Mercury.Common.Properties.Resources.delete_16;
            this.btnDeleteAddress.ImageAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.btnDeleteAddress.Location = new System.Drawing.Point(55, 1);
            this.btnDeleteAddress.Margin = new System.Windows.Forms.Padding(1);
            this.btnDeleteAddress.Name = "btnDeleteAddress";
            this.btnDeleteAddress.Size = new System.Drawing.Size(25, 25);
            this.btnDeleteAddress.TabIndex = 2;
            this.btnDeleteAddress.ToolTip = "Удалить адрес";
            this.btnDeleteAddress.ToolTipController = this.toolTipController;
            this.btnDeleteAddress.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Information;
            this.btnDeleteAddress.Click += new System.EventHandler(this.btnDeleteAddress_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel6, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.splitContainerControl, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 27F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(778, 168);
            this.toolTipController.SetSuperTip(this.tableLayoutPanel1, null);
            this.tableLayoutPanel1.TabIndex = 2;
            // 
            // ctrlAddress
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "ctrlAddress";
            this.Size = new System.Drawing.Size(778, 168);
            this.toolTipController.SetSuperTip(this, null);
            this.Load += new System.EventHandler(this.ctrlAddress_Load);
            this.contextMenuStrip.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl)).EndInit();
            this.splitContainerControl.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.treeListAddress)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repItemMemoEditAddress)).EndInit();
            this.xtraScrollableControl1.ResumeLayout(false);
            this.xtraScrollableControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtDescription.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboxOblast.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFlatCode.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboxAddressType.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboxFlat.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSubBuildingCode.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboxCountry.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboxSubBuilding.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPostIndex.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBuildingCode.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboxRegion.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboxBuilding.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboxCity.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboxStreet.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboxAddressPrefix.Properties)).EndInit();
            this.tableLayoutPanel6.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.Utils.ToolTipController toolTipController;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem menuRefresh;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem menuAdd;
        private System.Windows.Forms.ToolStripMenuItem menuEdit;
        private System.Windows.Forms.ToolStripMenuItem menuDelete;
        private DevExpress.XtraEditors.SplitContainerControl splitContainerControl;
        private DevExpress.XtraTreeList.TreeList treeListAddress;
        private DevExpress.XtraTreeList.Columns.TreeListColumn colFullAddress;
        private DevExpress.XtraEditors.SimpleButton btnSave;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraEditors.Repository.RepositoryItemMemoEdit repItemMemoEditAddress;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel6;
        private DevExpress.XtraEditors.SimpleButton btnAddAddress;
        private DevExpress.XtraEditors.SimpleButton btnEditAddress;
        private DevExpress.XtraEditors.SimpleButton btnDeleteAddress;
        private DevExpress.XtraEditors.XtraScrollableControl xtraScrollableControl1;
        private DevExpress.XtraEditors.TextEdit txtFlatCode;
        private DevExpress.XtraEditors.ComboBoxEdit cboxAddressType;
        private DevExpress.XtraEditors.ComboBoxEdit cboxFlat;
        private DevExpress.XtraEditors.LabelControl lblPostIndex;
        private DevExpress.XtraEditors.TextEdit txtSubBuildingCode;
        private DevExpress.XtraEditors.ComboBoxEdit cboxCountry;
        private DevExpress.XtraEditors.ComboBoxEdit cboxSubBuilding;
        private DevExpress.XtraEditors.TextEdit txtPostIndex;
        private DevExpress.XtraEditors.TextEdit txtBuildingCode;
        private DevExpress.XtraEditors.ComboBoxEdit cboxRegion;
        private DevExpress.XtraEditors.ComboBoxEdit cboxBuilding;
        private DevExpress.XtraEditors.ComboBoxEdit cboxCity;
        private DevExpress.XtraEditors.ComboBoxEdit cboxStreet;
        private DevExpress.XtraEditors.ComboBoxEdit cboxAddressPrefix;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private DevExpress.XtraEditors.SimpleButton btnSelectAddress;
        private DevExpress.XtraEditors.ComboBoxEdit cboxOblast;
        private DevExpress.XtraEditors.MemoEdit txtDescription;
    }
}
