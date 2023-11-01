using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CS_Project_Store_Manager
{
    public partial class UpdatePO : Form
    {
        public UpdatePO(Purchase_orders selectedPo)
        {
            InitializeComponent();
            SelectedPo = selectedPo;
        }
        CFModel Ent = new CFModel();
        Purchase_orders SelectedPo;
        List<PO_Products> addedProductsList = new List<PO_Products>();



        /////////////////////////////////////////////
        /////////////////ON LOAD ////////////////////
        /////////////////////////////////////////////
        private void UpdatePO_Load(object sender, EventArgs e)
        {
            currentProducts();
            ShowProdListBox();
            DisplayPO();
        }


        /////////////////////////////////////////////
        ///////////////// Functions /////////////////
        /////////////////////////////////////////////
        #region
        public void DisplayPO()
        {
            showpresentProducts();

            foreach (var prod in Ent.Products)
            {
                comboBox7.Items.Add(prod.product_name);
            }
            
            textBox21.Text = SelectedPo.po_ID.ToString();
            comboBox8.Text = SelectedPo.Store.st_name;
            foreach(var str in Ent.Stores)
            {
                comboBox8.Items.Add(str.st_name);
            }

            comboBox9.Text = SelectedPo.Supplier.supp_name;
            foreach (var supp in Ent.Suppliers)
            {
                comboBox9.Items.Add(supp.supp_name);
            }


        }
        
        public void ShowProdListBox()
        {
            listBox6.Items.Clear();
            listBox6.Items.Add("Product Name \t Quantity");
            var presentProducts = currentProducts();
            foreach (var item in presentProducts)
            {
                var product = Ent.Products.Find(item.product_id);
                listBox6.Items.Add(product.product_name + "\t" + "\t" + item.quantity);
            }
        }
        public DialogResult Empty_errMsg(string str)
        {
            return MessageBox.Show($"Select {str}");
        }

        public List<PO_Products> currentProducts()
        {
            var poId = SelectedPo.po_ID;
            var presentProducts = (from product in Ent.PO_Products
                               where product.po_ID == poId
                               select product).ToList();
            return presentProducts;
        }
        public void showpresentProducts()
        {
            var presentProducts = currentProducts();
            foreach (var item in presentProducts)
            {
                comboBox1.Items.Add(item.Product.product_name);
            }
        }

        public void AddProduct()
        {
            PO_Products Po_Pro = new PO_Products();
            string st = comboBox7.Text;
            var prod = (from product in Ent.Products
                        where product.product_name == st
                        select product).FirstOrDefault();
            if (comboBox7.Text != "")
            {
                Po_Pro.product_id = prod.product_id;
            }
            else { MessageBox.Show($"Select Product"); }

            if (textBox21.Text != "" && comboBox8.Text != "" && comboBox9.Text != "")
            {
                if (textBox22.Text != "")
                {
                    Po_Pro.po_ID = int.Parse(textBox21.Text);
                    Po_Pro.quantity = int.Parse(textBox22.Text);
                    Po_Pro.production_date = dateTimePicker1.Value;
                    Po_Pro.expiry_date = dateTimePicker2.Value;
                    comboBox1.Items.Add(textBox22.Text);
                    comboBox7.Items.Remove(prod.product_name);
                    textBox22.Text = "";
                    comboBox7.Text = "";
                    addedProductsList.Add(Po_Pro);
                    DialogResult confirm_delete = MessageBox.Show(
                    "Are you sure you want to add product from this PO?", "confirm add", MessageBoxButtons.YesNo);
                    if (confirm_delete == DialogResult.Yes)
                    {
                        Ent.PO_Products.Add(Po_Pro);
                        comboBox1.Text = "";
                        comboBox1.Items.Clear();
                        Ent.SaveChanges();
                        ShowProdListBox();
                        showpresentProducts();
                    }
                }
                else MessageBox.Show($"Enter valid Product Quantity!");
            }
            else MessageBox.Show("Enter Po details first");
        }


        public void UpdatePurO()
        {
            int PO_ID = int.Parse(textBox21.Text);
            var po = Ent.Purchase_orders.Find(PO_ID);
            if (po != null)
            {
                po.po_ID = int.Parse(textBox21.Text);
                po.po_date = dateTimePicker3.Value;
                if (comboBox8.Text != "")
                {
                    string st = comboBox8.Text;
                    var str = (from store in Ent.Stores
                               where store.st_name == st
                               select store).FirstOrDefault();
                    po.st_id = str.st_id;
                }
                else { Empty_errMsg("store"); }
                if (comboBox9.Text != "")
                {
                    string st = comboBox9.Text;
                    var supp = (from sup in Ent.Suppliers
                                where sup.supp_name == st
                                select sup).FirstOrDefault();
                    po.supp_id = supp.supp_id;
                }
                else { Empty_errMsg("supplier"); }

                Ent.SaveChanges();
                MessageBox.Show("Purchase Order updated");
                this.Close();
            }
            else { MessageBox.Show("Purchase order doesn't exist"); }
        }

        #endregion

        /////////////////////////////////////////////
        ///////////////// Buttons /////////////////
        /////////////////////////////////////////////
        #region
        private void button12_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text != "")
            {
                var prodId = (from product in Ent.Products
                              where product.product_name == comboBox1.Text
                              select product.product_id).FirstOrDefault();
                var productToRemove = (from product in Ent.PO_Products
                                       where product.product_id == prodId && product.po_ID == SelectedPo.po_ID
                                       select product).FirstOrDefault();

                DialogResult confirm_delete = MessageBox.Show(
                    "Are you sure you want to remove product from this PO?", "confirm remove", MessageBoxButtons.YesNo);
                if (confirm_delete == DialogResult.Yes)
                {
                    Ent.PO_Products.Remove(productToRemove);
                    comboBox1.Text = "";
                    comboBox1.Items.Clear();
                    Ent.SaveChanges();
                    ShowProdListBox();
                    showpresentProducts();
                }
            }
            else MessageBox.Show("Choose product you want to remove from PO");
            
        }

        private void button11_Click(object sender, EventArgs e)
        {
            AddProduct();
        }

        private void button1_Click(object sender, EventArgs e)
        {
                this.Close();
        }

        private void UpdatePO_FormClosing(object sender, FormClosingEventArgs e)
        {
            Form1 form1 = new Form1();
            form1.Show();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            UpdatePurO();
        }
        #endregion
    }
}
