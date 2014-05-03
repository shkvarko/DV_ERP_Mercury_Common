using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ERP_Mercury.Common;

namespace ERP_Mercury.Common
{
    public partial class frmPartsList : DevExpress.XtraEditors.XtraForm
    {
        #region Свойства
        private UniXP.Common.CProfile m_objProfile;
        private UniXP.Common.MENUITEM m_objMenuItem;
        private ERP_Mercury.Common.ctrlPartsDetail frmProductDetailEditor;
        public ERP_Mercury.Common.ctrlPartsDetail ProductDetailEditor
        {
            get { return frmProductDetailEditor; }
        }
        private List<CProduct> m_objProductList;
        public List<CProduct> ProductList
        {
            get { return m_objProductList; }
            set { m_objProductList = value; }
        }
        public List<CProduct> SelectedProductList;
        public System.Boolean SelectProductListByChecked { get; set; }
        #endregion

        #region Конструктор
        public frmPartsList(UniXP.Common.CProfile objProfile, UniXP.Common.MENUITEM objMenuItem, List<CProduct> objProductList)
        {
            System.Globalization.CultureInfo ci = new System.Globalization.CultureInfo("ru-RU");
            ci.NumberFormat.CurrencyDecimalSeparator = ".";
            ci.NumberFormat.NumberDecimalSeparator = ".";
            System.Threading.Thread.CurrentThread.CurrentCulture = ci;


            AppDomain currentDomain = AppDomain.CurrentDomain;
            currentDomain.AssemblyResolve += new ResolveEventHandler(MyResolveEventHandler);

            InitializeComponent();

            m_objProfile = objProfile;
            m_objMenuItem = objMenuItem;
            m_objProductList = objProductList;
            SelectedProductList = null;
            SelectProductListByChecked = false;

            frmProductDetailEditor = new ERP_Mercury.Common.ctrlPartsDetail(m_objProfile, m_objMenuItem, m_objProductList);
            frmProductDetailEditor.SetAccessToMultiSelect();

            tableLayoutPanelBgrnd.Controls.Add(frmProductDetailEditor, 0, 0);
            frmProductDetailEditor.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top) | (System.Windows.Forms.AnchorStyles.Bottom)
                      | System.Windows.Forms.AnchorStyles.Left)
                      | System.Windows.Forms.AnchorStyles.Right)));
        }

        private System.Reflection.Assembly MyResolveEventHandler(object sender, ResolveEventArgs args)
        {
            System.Reflection.Assembly MyAssembly = null;
            System.Reflection.Assembly objExecutingAssemblies = System.Reflection.Assembly.GetExecutingAssembly();

            System.Reflection.AssemblyName[] arrReferencedAssmbNames = objExecutingAssemblies.GetReferencedAssemblies();

            //Loop through the array of referenced assembly names.
            System.String strDllName = "";
            foreach (System.Reflection.AssemblyName strAssmbName in arrReferencedAssmbNames)
            {

                //Check for the assembly names that have raised the "AssemblyResolve" event.
                if (strAssmbName.FullName.Substring(0, strAssmbName.FullName.IndexOf(",")) == args.Name.Substring(0, args.Name.IndexOf(",")))
                {
                    strDllName = args.Name.Substring(0, args.Name.IndexOf(",")) + ".dll";
                    break;
                }

            }
            if (strDllName != "")
            {
                System.String strFileFullName = "";
                System.Boolean bError = false;
                foreach (System.String strPath in this.m_objProfile.ResourcePathList)
                {
                    //Load the assembly from the specified path. 
                    strFileFullName = strPath + "\\" + strDllName;
                    if (System.IO.File.Exists(strFileFullName))
                    {
                        try
                        {
                            MyAssembly = System.Reflection.Assembly.LoadFrom(strFileFullName);
                            break;
                        }
                        catch (System.Exception f)
                        {
                            bError = true;
                            DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка загрузки библиотеки: " +
                                strFileFullName + "\n\nТекст ошибки: " + f.Message, "Ошибка",
                                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                        }
                    }
                    if (bError) { break; }
                }
            }

            //Return the loaded assembly.
            if (MyAssembly == null)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("Не удалось найти библиотеку: " +
                                strDllName, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);

            }
            return MyAssembly;
        }
        public void EnablDiscountTools()
        {
            try
            {
                frmProductDetailEditor.EnablDiscountTools();
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("EnablDiscountTools. Текс ошибки: " + f.Message, "Внимание",
               System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
            }
            finally
            {
            }

            return;
        }

        #endregion

        #region Отменить выбор
        private void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                if (frmProductDetailEditor != null) { frmProductDetailEditor.SaveLayoutToRegistry(); }
                DialogResult = System.Windows.Forms.DialogResult.Cancel;
                Close();
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                    "btnCancel_Click.\nТекст ошибки: " + f.Message, "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }

            return;

        }
        #endregion 

        #region Подтвердить выбор
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                frmProductDetailEditor.SaveLayoutToRegistry(); 
                List<CProduct> objSelectedProductList = ( ( SelectProductListByChecked == true ) ? frmProductDetailEditor.GetSelectedProductListByCheck() : frmProductDetailEditor.GetSelectedProductList() );
                if ((objSelectedProductList == null) || (objSelectedProductList.Count == 0))
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show(
                        "Укажите хотя бы один товар.", "Внимание",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                    return;
                }

                SelectedProductList = objSelectedProductList;
                DialogResult = System.Windows.Forms.DialogResult.OK;
                Close();
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                    "btnCancel_Click.\nТекст ошибки: " + f.Message, "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }

            return;
        }
        #endregion
    }
}
