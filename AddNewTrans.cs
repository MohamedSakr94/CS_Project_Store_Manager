using System;
using System.Collections;
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
    public partial class AddNewTrans : Form
    {
        public AddNewTrans()
        {
            InitializeComponent();
        }
        CFModel Ent = new CFModel();
        List<Trans_Products> addedProductsList = new List<Trans_Products>();


        /////////////////////////////////////////////
        /////////////////ON LOAD ////////////////////
        /////////////////////////////////////////////


        private void AddNewTrans_Load(object sender, EventArgs e)
        {
            listBox6.Items.Add("Product Name \t Quantity");
            DisplayStores_Combo(comboBox8);
        }




        /////////////////////////////////////////////
        ///////////////// Functions /////////////////
        /////////////////////////////////////////////


        #region
        public void DisplayTrans()
        {
            listBox6.Items.Add("Product Name \t Quantity");
            foreach (var prod in Ent.Products)
            {
                comboBox7.Items.Add(prod.product_name);
            }
        }

        public string IsNotEmpty(TextBox txtbox)
        {
            if (txtbox.Text != "")
            {
                return txtbox.Text;
            }
            return "";
        }

        public string IsNotEmpty(ComboBox cmbbox)
        {
            if (cmbbox.Text != "")
            {
                return cmbbox.Text;
            }
            return "";
        }

        public void ClearTextBoxes3()
        {
            var offsprings = this.GetOffsprings2();
            foreach (Control c in this.GetOffsprings2())
            {
                if (c is TextBox)
                {
                    c.Text = "";
                }
                if (c is ComboBox)
                {
                    c.Text = "";
                }
            }
        }

        public DialogResult enterQty_errMsg()
        {
            return MessageBox.Show($"Enter valid Product Quantity!");
        }

        public DialogResult Empty_errMsg(string str)
        {
            return MessageBox.Show($"Select {str}");
        }

        public void DisplaySuppliers_Combo(ComboBox comb)
        {
            foreach (var supp in Ent.Suppliers)
            {
                comb.Items.Add(supp.supp_name);
            }
        }

        public void DisplayStores_Combo(ComboBox comb)
        {
            foreach (var str in Ent.Stores)
            {
                comb.Items.Add(str.st_name);
            }
        }
        public void DisplayPrdslist()
        {
            listBox6.Items.Clear();
            foreach (var item in Ent.Trans_Products)
            {
                var product = Ent.Products.Find(item.product_id);
                listBox6.Items.Add(product.product_name + "\t" + item.quantity);
            }

        }


        ///////////////////////////////


        public void CreateTrans()
        {
            int Trans_ID = int.Parse(textBox21.Text);
            var trans = Ent.Transactions.Find(Trans_ID);
            if (trans == null)
            {
                Transaction transaction = new Transaction();
                transaction.trans_ID = int.Parse(textBox21.Text);
                transaction.trans_date = dateTimePicker3.Value;
                if (comboBox8.Text != "")
                {
                    string st = comboBox8.Text;
                    var str = (from store in Ent.Stores
                               where store.st_name == st
                               select store).FirstOrDefault();
                    transaction.st_id = str.st_id;
                }
                else { Empty_errMsg("store"); }
                if (comboBox9.Text != "")
                {
                    string st = comboBox9.Text;
                    var supp = (from sup in Ent.Suppliers
                                where sup.supp_name == st
                                select sup).FirstOrDefault();
                    transaction.supp_id = supp.supp_id;
                }
                else { Empty_errMsg("supplier"); }

                ClearTextBoxes3();
                Ent.Transactions.Add(transaction);
                Ent.SaveChanges();
                foreach (var item in addedProductsList)
                {
                    int prod_Id = item.product_id;
                    int st_Id = transaction.st_id;
                    var isStAndProdFound = (from st_Prod in Ent.Inventories
                                            where st_Prod.Product_id == prod_Id && st_Prod.Store_id == st_Id
                                            select st_Prod).FirstOrDefault();

                    isStAndProdFound.Quantity -= item.quantity;
                    if (isStAndProdFound.Quantity == 0)
                    {
                        Ent.Inventories.Remove(isStAndProdFound);
                    }
                    Ent.Trans_Products.Add(item);
                }
                Ent.SaveChanges();
                listBox6.Items.Clear();
                DisplayTrans();
                MessageBox.Show("New transaction is added");
            }
            else { MessageBox.Show("Transaction already present!"); }
        }

        public void AddTransPro()
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
            else { Empty_errMsg("Product"); }

            if (textBox21.Text != "" && comboBox8.Text != "" && comboBox9.Text != "")
            {
                if (textBox22.Text != "")
                {
                    Trans_Pro.trans_ID = int.Parse(textBox21.Text);
                    Trans_Pro.quantity = int.Parse(textBox22.Text);
                    listBox6.Items.Add(comboBox7.Text + "\t" + "\t" + textBox22.Text.ToString());
                    comboBox1.Items.Add(textBox22.Text);
                    comboBox7.Items.Remove(prod.product_name);
                    textBox22.Text = "";
                    comboBox7.Text = "";
                    addedProductsList.Add(Trans_Pro);
                }
                else enterQty_errMsg();
            }
            else MessageBox.Show("Enter Transaction details first");
        }
        #endregion

        /////////////////////////////////////////////
        ////////////////BUTTON EVENTS////////////////
        /////////////////////////////////////////////

        #region
        private void button11_Click(object sender, EventArgs e)
        {
            if (textBox21.Text != "")
            {
                int Trans_ID = int.Parse(textBox21.Text);
                var trans = Ent.Transactions.Find(Trans_ID);
                if (trans == null)
                {
                    if (textBox22.Text != "")
                    {
                        if (int.Parse(textBox22.Text) <= int.Parse(textBox24.Text))
                        {
                            AddTransPro();
                            button1.Enabled = false;
                        }
                        else MessageBox.Show("Quantity must be equal or lessthan available quantity in store");
                    }
                    else MessageBox.Show("Must enter quantity");
                }
                else { MessageBox.Show("Transaction already present! create new id"); }
            }
            else MessageBox.Show("Enter transaction details first");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (comboBox1.Items.Count != 0)
            {
                CreateTrans();
                this.Close();
            }
            else MessageBox.Show("Transaction can't be added without items");
        }

        private void AddNewTrans_FormClosing(object sender, FormClosingEventArgs e)
        {
            Form1 form1 = new Form1();
            form1.Show();
        }

        private void comboBox8_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox7.Items.Clear();
            comboBox7.Text = "";
            var name = comboBox8.Text;
            var store = (from str in Ent.Stores
                         where str.st_name == name
                         select str).FirstOrDefault();

            var selectedStore = (from products in Ent.Inventories
                                 where products.Store.st_id == store.st_id
                                 select products.Product.product_name).ToList();
            foreach (var item in selectedStore)
            {
                comboBox7.Items.Add(item);
            }
        }

        private void comboBox7_SelectedIndexChanged(object sender, EventArgs e)
        {
            var name = comboBox8.Text;
            var store = (from str in Ent.Stores
                         where str.st_name == name
                         select str).FirstOrDefault();
            string st = comboBox7.Text;
            var Pro_Id = (from product in Ent.Products
                          where product.product_name == st
                          select product.product_id).FirstOrDefault();
            var selectedProduct = (from prod in Ent.Inventories
                                   where prod.Product_id == Pro_Id && prod.Store_id == store.st_id
                                   select prod).FirstOrDefault();
            var supp = Ent.Suppliers.Find(selectedProduct.Supplier_id);
            comboBox9.Text = Ent.Suppliers.Find(supp.supp_id).supp_name;
            textBox24.Text = selectedProduct.Quantity.ToString();
        }
        #endregion

    }

    public static class ControlExtensionMethods3
    {
        public static IEnumerable GetOffsprings3(this Control @this)
        {
            foreach (Control child in @this.Controls)
            {
                yield return child;
                foreach (var offspring in GetOffsprings3(child))
                    yield return offspring;
            }
        }
    }
}
