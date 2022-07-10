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
    public partial class ModifyPartForm : Form
    {
        public ModifyPartForm(Inhouse ihPart, Outsourced osPart)
        {
            InitializeComponent();
            if (ihPart != null) { LoadInhousePart(ihPart); }
            else if (osPart != null) { LoadOutsourcedPart(osPart); }
        }

        private void LoadInhousePart(Inhouse ihPart)
        {
            rbInHouse.Checked = true;
            txtId.Text = ihPart.PartId.ToString();
            txtName.Text = ihPart.Name;
            txtInventory.Text = ihPart.InStock.ToString();
            txtPrice.Text = ihPart.Price.ToString();
            txtMin.Text = ihPart.Min.ToString();
            txtMax.Text = ihPart.Max.ToString();
            txtMachineId.Text = ihPart.MachineId.ToString();
        }

        private void LoadOutsourcedPart(Outsourced osPart)
        {
            rbOutsourced.Checked = true;
            txtId.Text = osPart.PartId.ToString();
            txtName.Text = osPart.Name;
            txtInventory.Text = osPart.InStock.ToString();
            txtPrice.Text = osPart.Price.ToString();
            txtMin.Text = osPart.Min.ToString();
            txtMax.Text = osPart.Max.ToString();
            txtCompanyName.Text = osPart.CompanyName;
        }

        private void rbInHouse_CheckedChanged(object sender, EventArgs e)
        {
            if (rbInHouse.Checked)
            {
                lblCompanyName.Visible = false;
                txtCompanyName.Visible = false;
                txtCompanyName.Enabled = false;
                txtCompanyName.Text = "";
                lblMachineId.Visible = true;
                txtMachineId.Visible = true;
                txtMachineId.Enabled = true;
                txtMachineId.Text = "";
            }
        }

        private void rbOutsourced_CheckedChanged(object sender, EventArgs e)
        {
            if (rbOutsourced.Checked)
            {
                lblCompanyName.Visible = true;
                txtCompanyName.Visible = true;
                txtCompanyName.Enabled = true;
                txtCompanyName.Text = "";
                lblMachineId.Visible = false;
                txtMachineId.Visible = false;
                txtMachineId.Enabled = false;
                txtMachineId.Text = "";
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string errorMessage = Inventory.CheckFieldValidity(true, rbInHouse.Checked, rbOutsourced.Checked, txtName.Text, txtInventory.Text, txtPrice.Text, txtMin.Text, txtMax.Text, txtMachineId.Text, txtCompanyName.Text);
                if (string.IsNullOrEmpty(errorMessage))
                {
                    int partId = int.Parse(txtId.Text);
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

                    if (rbInHouse.Checked)
                    {
                        int machineId = int.Parse(txtMachineId.Text);
                        Inhouse updatedInhousePart = new Inhouse(partId, txtName.Text, price, inventory, min, max, machineId);
                        Inventory.updatePart(partId, updatedInhousePart);
                        Close();
                    }
                    else if (rbOutsourced.Checked)
                    {
                        Outsourced updatedOutsourcedPart = new Outsourced(partId, txtName.Text, price, inventory, min, max, txtCompanyName.Text);
                        Inventory.updatePart(partId, updatedOutsourcedPart);
                        Close();
                    }
                }
                else
                {
                    MessageBox.Show(errorMessage);
                }
            }
            catch
            {
                MessageBox.Show("Failed to save Part information.");
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
