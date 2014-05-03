namespace ERP_Mercury.Common
{
    partial class frmImportXLSData
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmImportXLSData));
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.toolTipController = new DevExpress.Utils.ToolTipController(this.components);
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.cboxSettings = new DevExpress.XtraEditors.ComboBoxEdit();
            this.treeListImportOrder = new DevExpress.XtraTreeList.TreeList();
            this.colProductID = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.colProductIBId = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.colProductArticle = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.colProductFullName = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.colStockResQty = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.repositoryItemCalcEdit2 = new DevExpress.XtraEditors.Repository.RepositoryItemCalcEdit();
            this.colStockQty = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.colOrderQty = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.colOrderPrice = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.colOrderAllPrice = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.colDescrpn = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.repositoryItemMemoEdit2 = new DevExpress.XtraEditors.Repository.RepositoryItemMemoEdit();
            this.repositoryItemCheckEdit2 = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.imglNodes = new System.Windows.Forms.ImageList(this.components);
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.btnOk = new DevExpress.XtraEditors.SimpleButton();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.btnFileOpenDialog = new DevExpress.XtraEditors.SimpleButton();
            this.txtID_Ib = new DevExpress.XtraEditors.TextEdit();
            this.labelControl17 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.checkEditImportPrices = new DevExpress.XtraEditors.CheckEdit();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.cboxSheet = new DevExpress.XtraEditors.ComboBoxEdit();
            this.btnLoadDataFromFile = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.listEditLog = new DevExpress.XtraEditors.ListBoxControl();
            this.tabControl = new DevExpress.XtraTab.XtraTabControl();
            this.tabImportDataInOrder = new DevExpress.XtraTab.XtraTabPage();
            this.tabTools = new DevExpress.XtraTab.XtraTabPage();
            this.tableLayoutPanel7 = new System.Windows.Forms.TableLayoutPanel();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.lblOrderInfo = new DevExpress.XtraEditors.LabelControl();
            this.labelControl7 = new DevExpress.XtraEditors.LabelControl();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.btnSaveSetings = new DevExpress.XtraEditors.SimpleButton();
            this.treeListSettings = new DevExpress.XtraTreeList.TreeList();
            this.colSettingsName = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.colSettingsColumnNum = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.repositoryItemCalcEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemCalcEdit();
            this.colSettingsDescription = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.repositoryItemMemoEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemMemoEdit();
            this.repositoryItemCheckEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cboxSettings.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.treeListImportOrder)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCalcEdit2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemMemoEdit2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit2)).BeginInit();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtID_Ib.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEditImportPrices.Properties)).BeginInit();
            this.tableLayoutPanel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cboxSheet.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.listEditLog)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tabControl)).BeginInit();
            this.tabControl.SuspendLayout();
            this.tabImportDataInOrder.SuspendLayout();
            this.tabTools.SuspendLayout();
            this.tableLayoutPanel7.SuspendLayout();
            this.tableLayoutPanel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.treeListSettings)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCalcEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemMemoEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit1)).BeginInit();
            this.SuspendLayout();
            // 
            // openFileDialog
            // 
            this.openFileDialog.Filter = "MS Excel files (*.xlsx)|*.xlsx|Excel 2003 files (*.xls)|*.xls|DBF files (*.dbf)|*" +
    ".dbf|All files (*.*)|*.*";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel7, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.treeListImportOrder, 0, 6);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 9);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel3, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.labelControl2, 0, 7);
            this.tableLayoutPanel1.Controls.Add(this.checkEditImportPrices, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel4, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.listEditLog, 0, 8);
            this.tableLayoutPanel1.Controls.Add(this.lblOrderInfo, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.labelControl7, 0, 4);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 10;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(667, 623);
            this.toolTipController.SetSuperTip(this.tableLayoutPanel1, null);
            this.tableLayoutPanel1.TabIndex = 2;
            // 
            // cboxSettings
            // 
            this.cboxSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.cboxSettings.Location = new System.Drawing.Point(121, 2);
            this.cboxSettings.Name = "cboxSettings";
            this.cboxSettings.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboxSettings.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.cboxSettings.Size = new System.Drawing.Size(543, 20);
            this.cboxSettings.TabIndex = 41;
            this.cboxSettings.SelectedValueChanged += new System.EventHandler(this.cboxSettings_SelectedValueChanged);
            // 
            // treeListImportOrder
            // 
            this.treeListImportOrder.Columns.AddRange(new DevExpress.XtraTreeList.Columns.TreeListColumn[] {
            this.colProductID,
            this.colProductIBId,
            this.colProductArticle,
            this.colProductFullName,
            this.colStockResQty,
            this.colStockQty,
            this.colOrderQty,
            this.colOrderPrice,
            this.colOrderAllPrice,
            this.colDescrpn});
            this.treeListImportOrder.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeListImportOrder.Location = new System.Drawing.Point(3, 142);
            this.treeListImportOrder.Name = "treeListImportOrder";
            this.treeListImportOrder.OptionsView.AutoWidth = false;
            this.treeListImportOrder.OptionsView.ShowSummaryFooter = true;
            this.treeListImportOrder.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemMemoEdit2,
            this.repositoryItemCheckEdit2,
            this.repositoryItemCalcEdit2});
            this.treeListImportOrder.SelectImageList = this.imglNodes;
            this.treeListImportOrder.Size = new System.Drawing.Size(661, 326);
            this.treeListImportOrder.TabIndex = 43;
            this.treeListImportOrder.CustomDrawNodeCell += new DevExpress.XtraTreeList.CustomDrawNodeCellEventHandler(this.treeListImportOrder_CustomDrawNodeCell);
            this.treeListImportOrder.CustomDrawNodeImages += new DevExpress.XtraTreeList.CustomDrawNodeImagesEventHandler(this.treeListImportOrder_CustomDrawNodeImages);
            // 
            // colProductID
            // 
            this.colProductID.Caption = "colProductID";
            this.colProductID.FieldName = "Цена";
            this.colProductID.MinWidth = 80;
            this.colProductID.Name = "colProductID";
            this.colProductID.OptionsColumn.AllowEdit = false;
            this.colProductID.OptionsColumn.AllowFocus = false;
            this.colProductID.OptionsColumn.ReadOnly = true;
            this.colProductID.Width = 183;
            // 
            // colProductIBId
            // 
            this.colProductIBId.Caption = "Код товара";
            this.colProductIBId.FieldName = "Код товара";
            this.colProductIBId.MinWidth = 27;
            this.colProductIBId.Name = "colProductIBId";
            this.colProductIBId.Visible = true;
            this.colProductIBId.VisibleIndex = 0;
            // 
            // colProductArticle
            // 
            this.colProductArticle.Caption = "Артикул";
            this.colProductArticle.FieldName = "Артикул";
            this.colProductArticle.Name = "colProductArticle";
            this.colProductArticle.Visible = true;
            this.colProductArticle.VisibleIndex = 1;
            // 
            // colProductFullName
            // 
            this.colProductFullName.Caption = "Наименование";
            this.colProductFullName.FieldName = "Наименование";
            this.colProductFullName.Name = "colProductFullName";
            this.colProductFullName.Visible = true;
            this.colProductFullName.VisibleIndex = 2;
            // 
            // colStockResQty
            // 
            this.colStockResQty.Caption = "Резерв, шт.";
            this.colStockResQty.ColumnEdit = this.repositoryItemCalcEdit2;
            this.colStockResQty.FieldName = "Резерв, шт.";
            this.colStockResQty.Format.FormatString = "### ### ##0.00";
            this.colStockResQty.Format.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colStockResQty.Name = "colStockResQty";
            this.colStockResQty.RowFooterSummary = DevExpress.XtraTreeList.SummaryItemType.Sum;
            this.colStockResQty.RowFooterSummaryStrFormat = "{0:### ### ##0.00}";
            this.colStockResQty.SummaryFooter = DevExpress.XtraTreeList.SummaryItemType.Sum;
            this.colStockResQty.SummaryFooterStrFormat = "{0:### ### ##0.00}";
            this.colStockResQty.Visible = true;
            this.colStockResQty.VisibleIndex = 3;
            // 
            // repositoryItemCalcEdit2
            // 
            this.repositoryItemCalcEdit2.AutoHeight = false;
            this.repositoryItemCalcEdit2.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemCalcEdit2.DisplayFormat.FormatString = "### ### ##0.00";
            this.repositoryItemCalcEdit2.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.repositoryItemCalcEdit2.Name = "repositoryItemCalcEdit2";
            // 
            // colStockQty
            // 
            this.colStockQty.Caption = "Остаток, шт.";
            this.colStockQty.ColumnEdit = this.repositoryItemCalcEdit2;
            this.colStockQty.FieldName = "Остаток, шт.";
            this.colStockQty.Format.FormatString = "### ### ##0.00";
            this.colStockQty.Format.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colStockQty.Name = "colStockQty";
            this.colStockQty.RowFooterSummary = DevExpress.XtraTreeList.SummaryItemType.Sum;
            this.colStockQty.RowFooterSummaryStrFormat = "{0:### ### ##0.00}";
            this.colStockQty.SummaryFooter = DevExpress.XtraTreeList.SummaryItemType.Sum;
            this.colStockQty.SummaryFooterStrFormat = "{0:### ### ##0.00}";
            this.colStockQty.Visible = true;
            this.colStockQty.VisibleIndex = 4;
            // 
            // colOrderQty
            // 
            this.colOrderQty.Caption = "Заказ, шт.";
            this.colOrderQty.ColumnEdit = this.repositoryItemCalcEdit2;
            this.colOrderQty.FieldName = "Заказ, шт.";
            this.colOrderQty.Format.FormatString = "### ### ##0.00";
            this.colOrderQty.Format.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colOrderQty.Name = "colOrderQty";
            this.colOrderQty.RowFooterSummary = DevExpress.XtraTreeList.SummaryItemType.Sum;
            this.colOrderQty.RowFooterSummaryStrFormat = "{0:### ### ##0.00}";
            this.colOrderQty.SummaryFooter = DevExpress.XtraTreeList.SummaryItemType.Sum;
            this.colOrderQty.SummaryFooterStrFormat = "{0:### ### ##0.00}";
            this.colOrderQty.Visible = true;
            this.colOrderQty.VisibleIndex = 5;
            // 
            // colOrderPrice
            // 
            this.colOrderPrice.Caption = "Цена";
            this.colOrderPrice.ColumnEdit = this.repositoryItemCalcEdit2;
            this.colOrderPrice.FieldName = "Цена";
            this.colOrderPrice.Format.FormatString = "### ### ##0.00";
            this.colOrderPrice.Format.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colOrderPrice.Name = "colOrderPrice";
            this.colOrderPrice.Visible = true;
            this.colOrderPrice.VisibleIndex = 6;
            // 
            // colOrderAllPrice
            // 
            this.colOrderAllPrice.Caption = "Сумма (заказ)";
            this.colOrderAllPrice.ColumnEdit = this.repositoryItemCalcEdit2;
            this.colOrderAllPrice.FieldName = "Сумма (заказ)";
            this.colOrderAllPrice.Format.FormatString = "### ### ##0.00";
            this.colOrderAllPrice.Format.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colOrderAllPrice.Name = "colOrderAllPrice";
            this.colOrderAllPrice.RowFooterSummary = DevExpress.XtraTreeList.SummaryItemType.Sum;
            this.colOrderAllPrice.RowFooterSummaryStrFormat = "{0:### ### ##0.00}";
            this.colOrderAllPrice.SummaryFooter = DevExpress.XtraTreeList.SummaryItemType.Sum;
            this.colOrderAllPrice.SummaryFooterStrFormat = "{0:### ### ##0.00}";
            this.colOrderAllPrice.Visible = true;
            this.colOrderAllPrice.VisibleIndex = 7;
            // 
            // colDescrpn
            // 
            this.colDescrpn.Caption = "Примечание";
            this.colDescrpn.FieldName = "Примечание";
            this.colDescrpn.Name = "colDescrpn";
            this.colDescrpn.Visible = true;
            this.colDescrpn.VisibleIndex = 8;
            // 
            // repositoryItemMemoEdit2
            // 
            this.repositoryItemMemoEdit2.Name = "repositoryItemMemoEdit2";
            // 
            // repositoryItemCheckEdit2
            // 
            this.repositoryItemCheckEdit2.AutoHeight = false;
            this.repositoryItemCheckEdit2.Name = "repositoryItemCheckEdit2";
            // 
            // imglNodes
            // 
            this.imglNodes.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imglNodes.ImageStream")));
            this.imglNodes.TransparentColor = System.Drawing.Color.Magenta;
            this.imglNodes.Images.SetKeyName(0, "ok_16.png");
            this.imglNodes.Images.SetKeyName(1, "warning.png");
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 87F));
            this.tableLayoutPanel2.Controls.Add(this.btnCancel, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.btnOk, 0, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 591);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(667, 32);
            this.toolTipController.SetSuperTip(this.tableLayoutPanel2, null);
            this.tableLayoutPanel2.TabIndex = 0;
            this.tableLayoutPanel2.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Image = global::ERP_Mercury.Common.Properties.Resources.delete2;
            this.btnCancel.Location = new System.Drawing.Point(584, 4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 25);
            this.btnCancel.TabIndex = 48;
            this.btnCancel.Text = "Выход";
            this.btnCancel.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Information;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.Image = global::ERP_Mercury.Common.Properties.Resources.ok_16;
            this.btnOk.Location = new System.Drawing.Point(497, 4);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(80, 25);
            this.btnOk.TabIndex = 47;
            this.btnOk.Text = "Запись";
            this.btnOk.ToolTip = "Подтвердить выбор файла";
            this.btnOk.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Information;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 3;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 116F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tableLayoutPanel3.Controls.Add(this.btnFileOpenDialog, 2, 0);
            this.tableLayoutPanel3.Controls.Add(this.txtID_Ib, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.labelControl17, 0, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(0, 45);
            this.tableLayoutPanel3.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(667, 25);
            this.toolTipController.SetSuperTip(this.tableLayoutPanel3, null);
            this.tableLayoutPanel3.TabIndex = 34;
            // 
            // btnFileOpenDialog
            // 
            this.btnFileOpenDialog.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.btnFileOpenDialog.Location = new System.Drawing.Point(642, 3);
            this.btnFileOpenDialog.Name = "btnFileOpenDialog";
            this.btnFileOpenDialog.Size = new System.Drawing.Size(22, 19);
            this.btnFileOpenDialog.TabIndex = 1;
            this.btnFileOpenDialog.Text = "...";
            this.btnFileOpenDialog.ToolTipController = this.toolTipController;
            this.btnFileOpenDialog.Click += new System.EventHandler(this.btnFileOpenDialog_Click);
            // 
            // txtID_Ib
            // 
            this.txtID_Ib.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtID_Ib.Location = new System.Drawing.Point(119, 3);
            this.txtID_Ib.Name = "txtID_Ib";
            this.txtID_Ib.Properties.ReadOnly = true;
            this.txtID_Ib.Size = new System.Drawing.Size(517, 20);
            this.txtID_Ib.TabIndex = 0;
            this.txtID_Ib.ToolTipController = this.toolTipController;
            // 
            // labelControl17
            // 
            this.labelControl17.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.labelControl17.Location = new System.Drawing.Point(3, 6);
            this.labelControl17.Name = "labelControl17";
            this.labelControl17.Size = new System.Drawing.Size(81, 13);
            this.labelControl17.TabIndex = 1;
            this.labelControl17.Text = "Файл с заказом:";
            // 
            // labelControl2
            // 
            this.labelControl2.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.labelControl2.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.labelControl2.Appearance.Options.UseFont = true;
            this.labelControl2.Location = new System.Drawing.Point(3, 474);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(115, 13);
            this.labelControl2.TabIndex = 42;
            this.labelControl2.Text = "Журнал сообщений:";
            // 
            // checkEditImportPrices
            // 
            this.checkEditImportPrices.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.checkEditImportPrices.Location = new System.Drawing.Point(3, 118);
            this.checkEditImportPrices.Name = "checkEditImportPrices";
            this.checkEditImportPrices.Properties.Caption = "импортировать цены из файла в протокол";
            this.checkEditImportPrices.Size = new System.Drawing.Size(311, 19);
            this.checkEditImportPrices.TabIndex = 47;
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 3;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 116F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 102F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.Controls.Add(this.cboxSheet, 1, 0);
            this.tableLayoutPanel4.Controls.Add(this.btnLoadDataFromFile, 2, 0);
            this.tableLayoutPanel4.Controls.Add(this.labelControl1, 0, 0);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(0, 70);
            this.tableLayoutPanel4.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 1;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(667, 25);
            this.toolTipController.SetSuperTip(this.tableLayoutPanel4, null);
            this.tableLayoutPanel4.TabIndex = 48;
            // 
            // cboxSheet
            // 
            this.cboxSheet.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.cboxSheet.Location = new System.Drawing.Point(119, 3);
            this.cboxSheet.Name = "cboxSheet";
            this.cboxSheet.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboxSheet.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.cboxSheet.Size = new System.Drawing.Size(96, 20);
            this.cboxSheet.TabIndex = 41;
            this.cboxSheet.SelectedValueChanged += new System.EventHandler(this.cboxSheet_SelectedValueChanged);
            // 
            // btnLoadDataFromFile
            // 
            this.btnLoadDataFromFile.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.btnLoadDataFromFile.Location = new System.Drawing.Point(219, 2);
            this.btnLoadDataFromFile.Margin = new System.Windows.Forms.Padding(1);
            this.btnLoadDataFromFile.Name = "btnLoadDataFromFile";
            this.btnLoadDataFromFile.Size = new System.Drawing.Size(120, 21);
            this.btnLoadDataFromFile.TabIndex = 42;
            this.btnLoadDataFromFile.Text = "Загрузить данные";
            this.btnLoadDataFromFile.ToolTip = "Загрузить данные из файла";
            this.btnLoadDataFromFile.ToolTipController = this.toolTipController;
            this.btnLoadDataFromFile.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Information;
            this.btnLoadDataFromFile.Click += new System.EventHandler(this.btnLoadDataFromFile_Click);
            // 
            // labelControl1
            // 
            this.labelControl1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.labelControl1.Location = new System.Drawing.Point(3, 6);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(84, 13);
            this.labelControl1.TabIndex = 35;
            this.labelControl1.Text = "Лист с данными:";
            // 
            // listEditLog
            // 
            this.listEditLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listEditLog.Location = new System.Drawing.Point(3, 494);
            this.listEditLog.Name = "listEditLog";
            this.listEditLog.Size = new System.Drawing.Size(661, 94);
            this.listEditLog.TabIndex = 49;
            // 
            // tabControl
            // 
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedTabPage = this.tabImportDataInOrder;
            this.tabControl.Size = new System.Drawing.Size(676, 654);
            this.tabControl.TabIndex = 2;
            this.tabControl.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.tabImportDataInOrder,
            this.tabTools});
            this.tabControl.Text = "xtraTabControl1";
            // 
            // tabImportDataInOrder
            // 
            this.tabImportDataInOrder.Controls.Add(this.tableLayoutPanel1);
            this.tabImportDataInOrder.Name = "tabImportDataInOrder";
            this.tabImportDataInOrder.Size = new System.Drawing.Size(667, 623);
            this.tabImportDataInOrder.Text = "Импорт данных";
            // 
            // tabTools
            // 
            this.tabTools.Controls.Add(this.tableLayoutPanel5);
            this.tabTools.Name = "tabTools";
            this.tabTools.Size = new System.Drawing.Size(667, 623);
            this.tabTools.Text = "Настройки для импорта данных";
            // 
            // tableLayoutPanel7
            // 
            this.tableLayoutPanel7.ColumnCount = 2;
            this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 116F));
            this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel7.Controls.Add(this.labelControl4, 0, 0);
            this.tableLayoutPanel7.Controls.Add(this.cboxSettings, 1, 0);
            this.tableLayoutPanel7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel7.Location = new System.Drawing.Point(0, 20);
            this.tableLayoutPanel7.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel7.Name = "tableLayoutPanel7";
            this.tableLayoutPanel7.RowCount = 1;
            this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel7.Size = new System.Drawing.Size(667, 25);
            this.toolTipController.SetSuperTip(this.tableLayoutPanel7, null);
            this.tableLayoutPanel7.TabIndex = 42;
            // 
            // labelControl4
            // 
            this.labelControl4.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.labelControl4.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.labelControl4.Appearance.Options.UseFont = true;
            this.labelControl4.Location = new System.Drawing.Point(3, 6);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(103, 13);
            this.labelControl4.TabIndex = 36;
            this.labelControl4.Text = "Вариант импорта:";
            // 
            // lblOrderInfo
            // 
            this.lblOrderInfo.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblOrderInfo.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.lblOrderInfo.Appearance.Options.UseFont = true;
            this.lblOrderInfo.Location = new System.Drawing.Point(3, 3);
            this.lblOrderInfo.Name = "lblOrderInfo";
            this.lblOrderInfo.Size = new System.Drawing.Size(106, 13);
            this.lblOrderInfo.TabIndex = 52;
            this.lblOrderInfo.Text = "Реквизиты заказа";
            // 
            // labelControl7
            // 
            this.labelControl7.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.labelControl7.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.labelControl7.Appearance.Options.UseFont = true;
            this.labelControl7.Location = new System.Drawing.Point(3, 98);
            this.labelControl7.Name = "labelControl7";
            this.labelControl7.Size = new System.Drawing.Size(126, 13);
            this.labelControl7.TabIndex = 53;
            this.labelControl7.Text = "Данные для импорта";
            // 
            // tableLayoutPanel5
            // 
            this.tableLayoutPanel5.ColumnCount = 1;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel5.Controls.Add(this.btnSaveSetings, 0, 1);
            this.tableLayoutPanel5.Controls.Add(this.treeListSettings, 0, 0);
            this.tableLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel5.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.RowCount = 2;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.tableLayoutPanel5.Size = new System.Drawing.Size(667, 623);
            this.toolTipController.SetSuperTip(this.tableLayoutPanel5, null);
            this.tableLayoutPanel5.TabIndex = 44;
            // 
            // btnSaveSetings
            // 
            this.btnSaveSetings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSaveSetings.Image = global::ERP_Mercury.Common.Properties.Resources.check2;
            this.btnSaveSetings.Location = new System.Drawing.Point(584, 595);
            this.btnSaveSetings.Name = "btnSaveSetings";
            this.btnSaveSetings.Size = new System.Drawing.Size(80, 25);
            this.btnSaveSetings.TabIndex = 48;
            this.btnSaveSetings.Text = "Запись";
            this.btnSaveSetings.ToolTip = "Подтвердить выбор файла";
            this.btnSaveSetings.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Information;
            this.btnSaveSetings.Click += new System.EventHandler(this.btnSaveSetings_Click);
            // 
            // treeListSettings
            // 
            this.treeListSettings.Columns.AddRange(new DevExpress.XtraTreeList.Columns.TreeListColumn[] {
            this.colSettingsName,
            this.colSettingsColumnNum,
            this.colSettingsDescription});
            this.treeListSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeListSettings.Location = new System.Drawing.Point(3, 3);
            this.treeListSettings.Name = "treeListSettings";
            this.treeListSettings.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemMemoEdit1,
            this.repositoryItemCheckEdit1,
            this.repositoryItemCalcEdit1});
            this.treeListSettings.Size = new System.Drawing.Size(661, 585);
            this.treeListSettings.TabIndex = 41;
            // 
            // colSettingsName
            // 
            this.colSettingsName.Caption = "Параметр";
            this.colSettingsName.FieldName = "Цена";
            this.colSettingsName.MinWidth = 80;
            this.colSettingsName.Name = "colSettingsName";
            this.colSettingsName.OptionsColumn.AllowEdit = false;
            this.colSettingsName.OptionsColumn.AllowFocus = false;
            this.colSettingsName.OptionsColumn.ReadOnly = true;
            this.colSettingsName.Visible = true;
            this.colSettingsName.VisibleIndex = 0;
            this.colSettingsName.Width = 183;
            // 
            // colSettingsColumnNum
            // 
            this.colSettingsColumnNum.Caption = "Значение";
            this.colSettingsColumnNum.ColumnEdit = this.repositoryItemCalcEdit1;
            this.colSettingsColumnNum.FieldName = "№ столбца";
            this.colSettingsColumnNum.MinWidth = 50;
            this.colSettingsColumnNum.Name = "colSettingsColumnNum";
            this.colSettingsColumnNum.OptionsColumn.AllowMove = false;
            this.colSettingsColumnNum.OptionsColumn.AllowSort = false;
            this.colSettingsColumnNum.Visible = true;
            this.colSettingsColumnNum.VisibleIndex = 1;
            this.colSettingsColumnNum.Width = 86;
            // 
            // repositoryItemCalcEdit1
            // 
            this.repositoryItemCalcEdit1.AutoHeight = false;
            this.repositoryItemCalcEdit1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemCalcEdit1.Name = "repositoryItemCalcEdit1";
            // 
            // colSettingsDescription
            // 
            this.colSettingsDescription.Caption = "Описание";
            this.colSettingsDescription.FieldName = "Описание";
            this.colSettingsDescription.Name = "colSettingsDescription";
            this.colSettingsDescription.Visible = true;
            this.colSettingsDescription.VisibleIndex = 2;
            this.colSettingsDescription.Width = 290;
            // 
            // repositoryItemMemoEdit1
            // 
            this.repositoryItemMemoEdit1.Name = "repositoryItemMemoEdit1";
            // 
            // repositoryItemCheckEdit1
            // 
            this.repositoryItemCheckEdit1.AutoHeight = false;
            this.repositoryItemCheckEdit1.Name = "repositoryItemCheckEdit1";
            // 
            // frmImportXLSData
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(676, 654);
            this.Controls.Add(this.tabControl);
            this.MinimumSize = new System.Drawing.Size(600, 500);
            this.Name = "frmImportXLSData";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.toolTipController.SetSuperTip(this, null);
            this.Text = "Импорт данных из MS Excel";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmImportXLSData_FormClosed);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cboxSettings.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.treeListImportOrder)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCalcEdit2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemMemoEdit2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit2)).EndInit();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtID_Ib.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEditImportPrices.Properties)).EndInit();
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cboxSheet.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.listEditLog)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tabControl)).EndInit();
            this.tabControl.ResumeLayout(false);
            this.tabImportDataInOrder.ResumeLayout(false);
            this.tabTools.ResumeLayout(false);
            this.tableLayoutPanel7.ResumeLayout(false);
            this.tableLayoutPanel7.PerformLayout();
            this.tableLayoutPanel5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.treeListSettings)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCalcEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemMemoEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private DevExpress.Utils.ToolTipController toolTipController;
        private DevExpress.XtraTab.XtraTabControl tabControl;
        private DevExpress.XtraTab.XtraTabPage tabTools;
        private DevExpress.XtraTab.XtraTabPage tabImportDataInOrder;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraEditors.SimpleButton btnOk;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private DevExpress.XtraEditors.SimpleButton btnFileOpenDialog;
        private DevExpress.XtraEditors.TextEdit txtID_Ib;
        private DevExpress.XtraEditors.LabelControl labelControl17;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraTreeList.TreeList treeListImportOrder;
        private DevExpress.XtraTreeList.Columns.TreeListColumn colProductID;
        private DevExpress.XtraTreeList.Columns.TreeListColumn colProductIBId;
        private DevExpress.XtraTreeList.Columns.TreeListColumn colProductArticle;
        private DevExpress.XtraTreeList.Columns.TreeListColumn colProductFullName;
        private DevExpress.XtraTreeList.Columns.TreeListColumn colStockResQty;
        private DevExpress.XtraEditors.Repository.RepositoryItemCalcEdit repositoryItemCalcEdit2;
        private DevExpress.XtraTreeList.Columns.TreeListColumn colStockQty;
        private DevExpress.XtraTreeList.Columns.TreeListColumn colOrderQty;
        private DevExpress.XtraEditors.Repository.RepositoryItemMemoEdit repositoryItemMemoEdit2;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit repositoryItemCheckEdit2;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.CheckEdit checkEditImportPrices;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private DevExpress.XtraEditors.ComboBoxEdit cboxSheet;
        private DevExpress.XtraEditors.SimpleButton btnLoadDataFromFile;
        private DevExpress.XtraEditors.ListBoxControl listEditLog;
        private DevExpress.XtraTreeList.Columns.TreeListColumn colOrderPrice;
        private DevExpress.XtraTreeList.Columns.TreeListColumn colDescrpn;
        private System.Windows.Forms.ImageList imglNodes;
        private DevExpress.XtraTreeList.Columns.TreeListColumn colOrderAllPrice;
        private DevExpress.XtraEditors.ComboBoxEdit cboxSettings;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel7;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.LabelControl lblOrderInfo;
        private DevExpress.XtraEditors.LabelControl labelControl7;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
        private DevExpress.XtraEditors.SimpleButton btnSaveSetings;
        private DevExpress.XtraTreeList.TreeList treeListSettings;
        private DevExpress.XtraTreeList.Columns.TreeListColumn colSettingsName;
        private DevExpress.XtraTreeList.Columns.TreeListColumn colSettingsColumnNum;
        private DevExpress.XtraEditors.Repository.RepositoryItemCalcEdit repositoryItemCalcEdit1;
        private DevExpress.XtraTreeList.Columns.TreeListColumn colSettingsDescription;
        private DevExpress.XtraEditors.Repository.RepositoryItemMemoEdit repositoryItemMemoEdit1;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit repositoryItemCheckEdit1;
    }
}