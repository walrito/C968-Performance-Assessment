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
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            Inventory.PopulateDummyData();

            BindingSource bsParts = new BindingSource { DataSource = Inventory.Parts };
            dgvParts.DataSource = bsParts;

            BindingSource bsProducts = new BindingSource { DataSource = Inventory.Products };
            dgvProducts.DataSource = bsProducts;
        }

        //Parts list search
        private void btnPartSearch_Click(object sender, EventArgs e)
        {
            dgvParts.ClearSelection();

            if (!string.IsNullOrEmpty(txtPartSearch.Text) && dgvParts.Rows.Count > 0)
            {
                foreach (DataGridViewRow dgvr in dgvParts.Rows)
                {
                    if (dgvr.Cells[0].Value.ToString().Contains(txtPartSearch.Text) || dgvr.Cells[1].Value.ToString().Contains(txtPartSearch.Text)) { dgvr.Selected = true; }
                    if (dgvr.Selected) { break; }
                }
            }
            if (dgvParts.SelectedRows.Count == 0) { MessageBox.Show("No matching items found."); }
        }

        //Open "Add Part" form
        private void btnPartAdd_Click(object sender, EventArgs e)
        {
            Form f = new AddPartForm();
            f.ShowDialog();
        }

        //Open "Modify Part" form
        private void btnPartModify_Click(object sender, EventArgs e)
        {
            if (dgvParts.SelectedRows.Count == 1)
            {
                if (dgvParts.SelectedRows[0].DataBoundItem.GetType() == typeof(Inhouse))
                {
                    Form f = new ModifyPartForm((Inhouse)dgvParts.SelectedRows[0].DataBoundItem, null);
                    f.ShowDialog();
                }
                else if (dgvParts.SelectedRows[0].DataBoundItem.GetType() == typeof(Outsourced))
                {
                    Form f = new ModifyPartForm(null, (Outsourced)dgvParts.SelectedRows[0].DataBoundItem);
                    f.ShowDialog();
                }
                dgvParts.Refresh();
            }
            else
            {
                MessageBox.Show("Please select one Part to modify.");
            }
        }

        //Delete part from list
        private void btnPartDelete_Click(object sender, EventArgs e)
        {
            if (dgvParts.SelectedRows.Count == 0)
            {
                MessageBox.Show("No Part(s) selected to delete.");
            }
            else
            {
                if (MessageBox.Show("Delete selected part(s)?", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    foreach (DataGridViewRow dgvr in dgvParts.SelectedRows) { if (dgvr.Selected) { dgvParts.Rows.RemoveAt(dgvr.Index); } }
                }
            }
        }

        //Products list search
        private void btnProductSearch_Click(object sender, EventArgs e)
        {
            dgvProducts.ClearSelection();

            if (!string.IsNullOrEmpty(txtProductSearch.Text) && dgvProducts.Rows.Count > 0)
            {
                foreach (DataGridViewRow dgvr in dgvProducts.Rows)
                {
                    if (dgvr.Cells[0].Value.ToString().Contains(txtProductSearch.Text) || dgvr.Cells[1].Value.ToString().Contains(txtProductSearch.Text)) { dgvr.Selected = true; }
                    if (dgvr.Selected) { break; }
                }
            }
            if (dgvProducts.SelectedRows.Count == 0) { MessageBox.Show("No matching items found."); }
        }

        //Open "Add Product" form
        private void btnProductAdd_Click(object sender, EventArgs e)
        {
            Form f = new AddProductForm();
            f.ShowDialog();
        }

        //Open "Modify Product" form
        private void btnProductModify_Click(object sender, EventArgs e)
        {
            if (dgvProducts.SelectedRows.Count == 1)
            {
                Form f = new ModifyProductForm((Product)dgvProducts.SelectedRows[0].DataBoundItem);
                f.ShowDialog();
                dgvParts.Refresh();
            }
            else
            {
                MessageBox.Show("Please select one Product to modify.");
            }
        }

        //Delete product from list
        private void btnProductDelete_Click(object sender, EventArgs e)
        {
            bool hasParts = false;
            if (dgvProducts.SelectedRows.Count == 0)
            {
                MessageBox.Show("No Product(s) selected to delete.");
            }
            else
            {
                if (MessageBox.Show("Delete selected product(s)?", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    foreach (DataGridViewRow dgvr in dgvProducts.SelectedRows)
                    {
                        Product p = (Product)dgvr.DataBoundItem;
                        if (p.AssociatedParts.Count > 0) { hasParts = true; }
                        if (p.AssociatedParts.Count == 0) { if (dgvr.Selected) { dgvProducts.Rows.RemoveAt(dgvr.Index); } }
                    }
                    if (hasParts) { MessageBox.Show("Cannot delete Products with associated Parts. Some selected items not deleted."); }
                }
            }
        }

        //Exits program
        private void btnExit_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}