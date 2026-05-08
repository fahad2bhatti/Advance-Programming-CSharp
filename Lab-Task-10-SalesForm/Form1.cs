using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace SalesForm
{
    public partial class Form1 : Form
    {
        // Controls
        Label lblProduct;
        ComboBox cmbProducts;
        Button btnShowStock;
        Button btnSell;

        // Data
        Dictionary<string, int> products = new Dictionary<string, int>()
        {
            { "Laptop",     10 },
            { "Mobile",     25 },
            { "Tablet",     15 },
            { "Headphones", 30 },
            { "Keyboard",   20 }
        };

        public Form1()
        {
            InitializeComponent();
            SetupForm();
            LoadProducts();
        }

        private void SetupForm()
        {
            this.Text = "Sales Form";
            this.Size = new Size(400, 250);

            lblProduct = new Label();
            lblProduct.Text = "Select Product:";
            lblProduct.Location = new Point(30, 40);
            lblProduct.AutoSize = true;

            cmbProducts = new ComboBox();
            cmbProducts.Location = new Point(30, 65);
            cmbProducts.Width = 320;
            cmbProducts.DropDownStyle = ComboBoxStyle.DropDownList;

            btnShowStock = new Button();
            btnShowStock.Text = "Show Stock";
            btnShowStock.Location = new Point(30, 120);
            btnShowStock.Width = 150;
            btnShowStock.Click += btnShowStock_Click;

            btnSell = new Button();
            btnSell.Text = "Sell Product";
            btnSell.Location = new Point(200, 120);
            btnSell.Width = 150;
            btnSell.Click += btnSell_Click;

            this.Controls.Add(lblProduct);
            this.Controls.Add(cmbProducts);
            this.Controls.Add(btnShowStock);
            this.Controls.Add(btnSell);
        }

        private void LoadProducts()
        {
            cmbProducts.Items.Clear();
            foreach (var product in products.Keys)
            {
                cmbProducts.Items.Add(product);
            }
            cmbProducts.SelectedIndex = 0;
        }

        private void btnShowStock_Click(object sender, EventArgs e)
        {
            if (cmbProducts.SelectedItem == null)
            {
                MessageBox.Show("Please select a product first.", "Warning",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string selectedProduct = cmbProducts.SelectedItem.ToString();
            int stock = products[selectedProduct];

            MessageBox.Show($"Product: {selectedProduct}\nAvailable Stock: {stock} units",
                "Stock Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnSell_Click(object sender, EventArgs e)
        {
            if (cmbProducts.SelectedItem == null)
            {
                MessageBox.Show("Please select a product first.", "Warning",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string selectedProduct = cmbProducts.SelectedItem.ToString();
            int stock = products[selectedProduct];

            if (stock <= 0)
            {
                MessageBox.Show($"Sorry! {selectedProduct} is OUT OF STOCK.",
                    "Out of Stock", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            products[selectedProduct]--;

            MessageBox.Show($"1 unit of {selectedProduct} sold!\nRemaining Stock: {products[selectedProduct]} units",
                "Sale Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}