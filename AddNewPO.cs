using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;
using Microsoft.VisualBasic;

namespace CS_Project_Store_Manager
{
    public partial class AddNewPO : Form
    {
        public AddNewPO()
        {
            InitializeComponent();
        }

        CFModel Ent = new CFModel();
        List<PO_Products> addedProductsList = new List<PO_Products>();

        /////////////////////////////////////////////
        /////////////////ON LOAD ////////////////////
        /////////////////////////////////////////////

        #region
        private void AddNewPO_Load(object sender, EventArgs e)
        {
            DisplayPO();
            DisplayStores_Combo(comboBox8);
            DisplaySuppliers_Combo(comboBox9);
        }




        #endregion

        /////////////////////////////////////////////
        ///////////////// Functions /////////////////
        /////////////////////////////////////////////

        #region

        public void DisplayPO()
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


        public void ClearTextBoxes2()
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

        public DialogResult EnterQty_errMsg()
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
            foreach (var item in Ent.PO_Products) 
            {
                var product = Ent.Products.Find(item.product_id);
                listBox6.Items.Add(product.product_name + "\t" + item.quantity);
            }
            
        }




        ///////////////////////////////

        public void CreatePO()
        {
            int PO_ID = int.Parse(textBox21.Text);
            var po = Ent.Purchase_orders.Find(PO_ID);
            if (po == null)
            {
                Purchase_orders purch = new Purchase_orders();
                purch.po_ID = int.Parse(textBox21.Text);
                purch.po_date = dateTimePicker3.Value;
                if (comboBox8.Text != "")
                {
                    string st = comboBox8.Text;
                    var str = (from store in Ent.Stores
                               where store.st_name == st
                               select store).FirstOrDefault();
                    purch.st_id = str.st_id;
                }
                else { Empty_errMsg("store"); }
                if (comboBox9.Text != "")
                {
                    string st = comboBox9.Text;
                    var supp = (from sup in Ent.Suppliers
                                where sup.supp_name == st
                                select sup).FirstOrDefault();
                    purch.supp_id = supp.supp_id;
                }
                else { Empty_errMsg("supplier"); }

                ClearTextBoxes2();
                Ent.Purchase_orders.Add(purch);
                Ent.SaveChanges();

                foreach (var item in addedProductsList)
                {
                    int prod_Id = item.product_id;
                    int st_Id = purch.st_id;
                    var isStore_And_Prod_Found = (from st_Prod in Ent.Inventories
                                            where st_Prod.Product_id == prod_Id && st_Prod.Store_id == st_Id
                                            select st_Prod).FirstOrDefault();
                    if (isStore_And_Prod_Found == null)
                    {
                        Inventory inventory = new Inventory();
                        inventory.Product_id = prod_Id;
                        inventory.Store_id = st_Id;
                        inventory.Supplier_id = purch.supp_id;
                        inventory.Production_date = item.production_date;
                        inventory.Expiry_date = item.expiry_date;
                        inventory.Quantity = item.quantity;
                        Ent.Inventories.Add(inventory);
                    }
                    else if(isStore_And_Prod_Found != null)
                    {
                        isStore_And_Prod_Found.Quantity += item.quantity;
                    }
                    Ent.PO_Products.Add(item);
                }
                Ent.SaveChanges();
                listBox6.Items.Clear();
                DisplayPO();
                MessageBox.Show("New Purchase Order is added");
            }
            else { MessageBox.Show("Purchase order already present!"); }
        }

        public void AddProduct()
        {
            PO_Products Po_Pro = new PO_Products();
            Inventory addInventory = new Inventory();
            string st = comboBox7.Text;
            var prod = (from product in Ent.Products
                       where product.product_name == st
                       select product).FirstOrDefault();
            if (comboBox7.Text != "")
            {
                Po_Pro.product_id = prod.product_id;
            } else { Empty_errMsg("Product"); }

            if (textBox21.Text != "" && comboBox8.Text != "" && comboBox9.Text != "")
            {
                if (textBox22.Text != "")
                {
                    Po_Pro.po_ID = int.Parse(textBox21.Text);
                    Po_Pro.quantity = int.Parse(textBox22.Text);
                    Po_Pro.production_date = dateTimePicker1.Value;
                    Po_Pro.expiry_date = dateTimePicker2.Value;
                    listBox6.Items.Add(comboBox7.Text + "\t" + "\t" + textBox22.Text.ToString());
                    comboBox1.Items.Add(textBox22.Text);
                    comboBox7.Items.Remove(prod.product_name);
                    textBox22.Text = "";
                    comboBox7.Text = "";
                    groupBox2.Enabled = false;
                    addedProductsList.Add(Po_Pro);
                    
                }
                else EnterQty_errMsg();
            }
            else MessageBox.Show("Enter Po details first");
            

        }



        #endregion


        /////////////////////////////////////////////
        ////////////////BUTTON EVENTS////////////////
        /////////////////////////////////////////////

        #region
        private void button11_Click(object sender, EventArgs e)
        {
            int PO_ID = int.Parse(textBox21.Text);
            var po = Ent.Purchase_orders.Find(PO_ID);
            if (po == null)
            {
                AddProduct();
            }
            else { MessageBox.Show("Purchase order already present! create new id"); }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (comboBox1.Items.Count != 0)
            {
                CreatePO();
                this.Close();
            }
            else MessageBox.Show("Purchase order can't be added without items");

        }

        private void AddNewPO_FormClosing(object sender, FormClosingEventArgs e)
        {
            Form1 form1 = new Form1();
            form1.Show();
        }



        #endregion

    }


    public static class ControlExtensionMethods2
    {
        public static IEnumerable GetOffsprings2(this Control @this)
        {
            foreach (Control child in @this.Controls)
            {
                yield return child;
                foreach (var offspring in GetOffsprings2(child))
                    yield return offspring;
            }
        }
    }
}
