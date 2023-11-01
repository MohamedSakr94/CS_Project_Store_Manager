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
    public partial class UpdateTrans : Form
    {
        public UpdateTrans(Transaction selectedTrans)
        {
            InitializeComponent();
            SelectedTrans = selectedTrans;
        }
        CFModel Ent = new CFModel();
        Transaction SelectedTrans;
        List<Trans_Products> addedProductsList = new List<Trans_Products>();

        /////////////////////////////////////////////
        /////////////////ON LOAD ////////////////////
        /////////////////////////////////////////////

        private void UpdateTrans_Load(object sender, EventArgs e)
        {
            currentProducts();
            ShowProdListBox();
            DisplayTrans();
        }



        /////////////////////////////////////////////
        ///////////////// Functions /////////////////
        /////////////////////////////////////////////

        #region
        public void DisplayTrans()
        {
            showpresentProducts();

            foreach (var prod in Ent.Products)
            {
                comboBox7.Items.Add(prod.product_name);
            }

            textBox21.Text = SelectedTrans.trans_ID.ToString();
            comboBox8.Text = SelectedTrans.Store.st_name;
            foreach (var str in Ent.Stores)
            {
                comboBox8.Items.Add(str.st_name);
            }

            comboBox9.Text = SelectedTrans.Supplier.supp_name;
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

        public List<Trans_Products> currentProducts()
        {
            var transId = SelectedTrans.trans_ID;
            var presentProducts = (from product in Ent.Trans_Products
                                   where product.trans_ID == transId
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
            Trans_Products Trans_Pro = new Trans_Products();
            string st = comboBox7.Text;
            var prod = (from product in Ent.Products
                        where product.product_name == st
                        select product).FirstOrDefault();
            if (comboBox7.Text != "")
            {
                Trans_Pro.product_id = prod.product_id;
            }
            else { MessageBox.Show($"Select Product"); }

            if (textBox21.Text != "" && comboBox8.Text != "" && comboBox9.Text != "")
            {
                if (textBox22.Text != "")
                {
                    Trans_Pro.trans_ID = int.Parse(textBox21.Text);
                    Trans_Pro.quantity = int.Parse(textBox22.Text);
                    comboBox1.Items.Add(textBox22.Text);
                    comboBox7.Items.Remove(prod.product_name);
                    textBox22.Text = "";
                    comboBox7.Text = "";
                    addedProductsList.Add(Trans_Pro);
                    DialogResult confirm_delete = MessageBox.Show(
                    "Are you sure you want to add product from this PO?", "confirm add", MessageBoxButtons.YesNo);
                    if (confirm_delete == DialogResult.Yes)
                    {
                        Ent.Trans_Products.Add(Trans_Pro);
                        comboBox1.Text = "";
                        comboBox1.Items.Clear();
                        Ent.SaveChanges();
                        ShowProdListBox();
                        showpresentProducts();
                    }
                }
                else MessageBox.Show($"Enter valid Product Quantity!");
            }
            else MessageBox.Show("Enter Transaction details first");
        }


        public void UpdateTransaction()
        {
            int Trans_ID = int.Parse(textBox21.Text);
            var Trans = Ent.Transactions.Find(Trans_ID);
            if (Trans != null)
            {
                Trans.trans_ID = int.Parse(textBox21.Text);
                Trans.trans_date = dateTimePicker3.Value;
                if (comboBox8.Text != "")
                {
                    string st = comboBox8.Text;
                    var str = (from store in Ent.Stores
                               where store.st_name == st
                               select store).FirstOrDefault();
                    Trans.st_id = str.st_id;
                }
                else { Empty_errMsg("store"); }
                if (comboBox9.Text != "")
                {
                    string st = comboBox9.Text;
                    var supp = (from sup in Ent.Suppliers
                                where sup.supp_name == st
                                select sup).FirstOrDefault();
                    Trans.supp_id = supp.supp_id;
                }
                else { Empty_errMsg("supplier"); }

                Ent.SaveChanges();
                MessageBox.Show("Transation updated");
                this.Close();
            }
            else { MessageBox.Show("Transation doesn't exist"); }
        }
        #endregion


        /////////////////////////////////////////////
        ///////////////// Button events /////////////
        /////////////////////////////////////////////

        #region
        private void button12_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text != "")
            {
                var prodId = (from product in Ent.Products
                              where product.product_name == comboBox1.Text
                              select product.product_id).FirstOrDefault();
                var productToRemove = (from product in Ent.Trans_Products
                                       where product.product_id == prodId && product.trans_ID == SelectedTrans.trans_ID
                                       select product).FirstOrDefault();

                DialogResult confirm_delete = MessageBox.Show(
                    "Are you sure you want to remove product from this Transaction?", "confirm remove", MessageBoxButtons.YesNo);
                if (confirm_delete == DialogResult.Yes)
                {
                    Ent.Trans_Products.Remove(productToRemove);
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

        private void UpdateTrans_FormClosing(object sender, FormClosingEventArgs e)
        {
            Form1 form1 = new Form1();
            form1.Show();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            UpdateTransaction();
        }
        #endregion
    }
}
