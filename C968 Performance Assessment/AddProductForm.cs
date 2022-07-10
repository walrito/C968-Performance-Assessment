using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace C968_Performance_Assessment
{
    public partial class AddProductForm : Form
    {
        private BindingList<Part> associatedParts = new BindingList<Part>();

        public AddProductForm()
        {
            InitializeComponent();
        }

        private void AddProductForm_Load(object sender, EventArgs e)
        {
            txtId.Text = Inventory.GetNextId("product").ToString();

            BindingSource bsAllParts = new BindingSource { DataSource = Inventory.Parts };
            dgvAllParts.DataSource = bsAllParts;

            BindingSource bsProductParts = new BindingSource { DataSource = associatedParts };
            dgvProductParts.DataSource = bsProductParts;
        }

        private void btnPartSearch_Click(object sender, EventArgs e)
        {
            dgvAllParts.ClearSelection();

            if (!string.IsNullOrEmpty(txtPartSearch.Text) && dgvAllParts.Rows.Count > 0)
            {
                foreach (DataGridViewRow dgvr in dgvAllParts.Rows)
                {
                    if (dgvr.Cells[0].Value.ToString().Contains(txtPartSearch.Text) || dgvr.Cells[1].Value.ToString().Contains(txtPartSearch.Text)) { dgvr.Selected = true; }
                    if (dgvr.Selected) { break; }
                }
            }
            if (dgvAllParts.SelectedRows.Count == 0) { MessageBox.Show("No matching items found."); }
        }

        private void btnAddPartToProduct_Click(object sender, EventArgs e)
        {
            if (dgvAllParts.SelectedRows.Count == 0)
            {
                MessageBox.Show("No Part(s) selected to add to Product.");
            }
            else
            {
                foreach (DataGridViewRow dgvr in dgvAllParts.SelectedRows) { associatedParts.Add((Part)dgvr.DataBoundItem); }
            }
        }

        private void btnDeletePartFromProduct_Click(object sender, EventArgs e)
        {
            if (dgvProductParts.SelectedRows.Count == 0)
            {
                MessageBox.Show("No Part(s) selected to delete from Product.");
            }
            else
            {
                if (MessageBox.Show("Delete selected part(s)?", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    foreach (DataGridViewRow dgvr in dgvProductParts.SelectedRows) { if (dgvr.Selected) { dgvProductParts.Rows.RemoveAt(dgvr.Index); } }
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string errorMessage = Inventory.CheckFieldValidity(false, false, false, txtName.Text, txtInventory.Text, txtPrice.Text, txtMin.Text, txtMax.Text, "", "");
                if (string.IsNullOrEmpty(errorMessage))
                {
                    int inventory = int.Parse(txtInventory.Text);
                    decimal price = decimal.Parse(txtPrice.Text);
                    int min = int.Parse(txtMin.Text);
                    int max = int.Parse(txtMax.Text);

                    if (min > max)
                    {
                        MessageBox.Show("Min value should be less than Max value.");
                        return;
                    }

                    if (inventory < min || inventory > max)
                    {
                        MessageBox.Show("Inventory should be within Min/Max range.");
                        return;
                    }

                    Product newProduct = new Product(Inventory.GetNextId("product"), txtName.Text, price, inventory, min, max);
                    foreach (DataGridViewRow dgvr in dgvProductParts.Rows) { newProduct.addAssociatedPart((Part)dgvr.DataBoundItem); }
                    Inventory.addProduct(newProduct);
                    Close();
                }
                else
                {
                    MessageBox.Show(errorMessage);
                }
            }
            catch
            {
                MessageBox.Show("Failed to save Product information.");
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}