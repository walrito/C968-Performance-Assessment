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
    public partial class AddPartForm : Form
    {
        public AddPartForm()
        {
            InitializeComponent();
        }

        private void AddPartForm_Load(object sender, EventArgs e)
        {
            txtId.Text = Inventory.GetNextId("part").ToString();
        }

        //Displays and enables Machine ID field for In-House entry
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

        //Displays and enables Company Name field for Outsourced entry
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
                        Inhouse newInhousePart = new Inhouse(Inventory.GetNextId("part"), txtName.Text, price, inventory, min, max, machineId);
                        Inventory.addPart(newInhousePart);
                        Close();
                    }
                    else if (rbOutsourced.Checked)
                    {
                        Outsourced newOutsourcedPart = new Outsourced(Inventory.GetNextId("part"), txtName.Text, price, inventory, min, max, txtCompanyName.Text);
                        Inventory.addPart(newOutsourcedPart);
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
