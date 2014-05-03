using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ERP_Mercury.Common
{
    public partial class frmSelectAddressForRtt : DevExpress.XtraEditors.XtraForm
    {
        private UniXP.Common.CProfile m_objProfile;
        private CAddress m_objAddressSelected;
        public CAddress AddressSelected
        {
            get { return m_objAddressSelected; }
        }
        private System.Guid m_uuidOwnerId;

        public frmSelectAddressForRtt(UniXP.Common.CProfile objProfile, System.Guid OwnerId)
        {
            InitializeComponent();

            m_objProfile = objProfile;
            m_uuidOwnerId = OwnerId;
            m_objAddressSelected = null;
        }

        public void LoadAddressList()
        {
            try
            {
                treeListAddress.Nodes.Clear();

                List<CAddress> objAddressList = CAddress.GetAddressList(m_objProfile, null, EnumObject.RttCustomer, m_uuidOwnerId);
                    if (objAddressList != null)
                    {
                        foreach (CAddress objAddress in objAddressList)
                        {
                            DevExpress.XtraTreeList.Nodes.TreeListNode objNode = treeListAddress.AppendNode(new object[] { objAddress.VisitingCard2 }, null);
                            objNode.Tag = objAddress;
                        }
                    }
                    objAddressList = null;

                if (treeListAddress.Nodes.Count > 0)
                {
                    treeListAddress.FocusedNode = treeListAddress.Nodes[0];
                }

            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка обновления списка.\n\nТекст ошибки:\n" + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }

            return ;
        }

        private void SelectAddress()
        {
            try
            {
                if ((treeListAddress.Nodes.Count > 0) && (treeListAddress.FocusedNode != null))
                {
                    m_objAddressSelected = (CAddress)treeListAddress.FocusedNode.Tag;
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("SelectAddress.\n\nТекст ошибки:\n" + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }

            return;
        }

        private void Cancel()
        {
            try
            {
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("Cancel.\n\nТекст ошибки:\n" + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }

            return;
        }

        private void treeListAddress_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                if ((treeListAddress.Nodes.Count > 0) && (treeListAddress.FocusedNode != null))
                {
                    SelectAddress();
                }
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("treeListAddress_MouseDoubleClick.\n\nТекст ошибки:\n" + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }

            return;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if ((treeListAddress.Nodes.Count > 0) && (treeListAddress.FocusedNode != null))
                {
                    SelectAddress();
                }
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("btnSave_Click.\n\nТекст ошибки:\n" + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }

            return;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                Cancel();
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("btnCancel_Click.\n\nТекст ошибки:\n" + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }

            return;
        }

    }
}
