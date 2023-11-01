using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualBasic;

namespace CS_Project_Store_Manager
{

    public partial class Form1 : Form
    {
        CFModel Ent = new CFModel();
        public Form1()
        {
            InitializeComponent();
        }

        List<Inventory> inventoriesList = new List<Inventory>();

        /////////////////////////////////////////////
        /////////////////ON LOAD ////////////////////
        /////////////////////////////////////////////
        #region
        public void Form1_Load(object sender, EventArgs e)
        {
            //Stores
            DisplayStores();
            

            //Products
            DisplayProducts();
            

            //Suppliers
            DisplaySuppliers();


            //Clients
            DisplayClients();


            //Purchase Order
            DisplayPO();


            //Sales Transactions
            DisplayTrans();


            //Store to store transfer
            DisplayAllStores();


            //stores report
            DisplayStoresReport();


            //products report
            DisplayProducts_report();


            //Products in/out
            DisplayProducts_combo(comboBox16);


            //Products date range report
            DisplayStores_Combo(comboBox15);

        }

        #endregion

        /////////////////////////////////////////////
        ///////////////// Functions /////////////////
        /////////////////////////////////////////////
        #region

        //General/////////////////////
        #region
        public bool IsEmailValid(string email)
        {
            var valid = true;

            try
            {
                var emailAddress = new MailAddress(email);
            }
            catch
            {
                MessageBox.Show("Enter valid Email adress!");
                valid = false;
            }

            return valid;
        }

        public void NumNotValid(string name, int num)
        {
            MessageBox.Show($"{name} number must contain {num} digits");
        }

        public string IsNotEmpty(TextBox txtbox)
        {
            if (txtbox.Text != "")
            {
                return txtbox.Text;
            }
            return "";
        }

        public void ClearTextBoxes()
        {
            var offsprings = this.GetOffsprings();
            foreach (Control c in this.GetOffsprings())
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

        #endregion


        //Error Messages/////////////////////////
        #region
        public DialogResult enterData_errMsg(string col, string row)
        {
            return MessageBox.Show($"Enter atleast {col} to create a new {row}!");
        }

        public DialogResult Duplicate_errMsg(string row)
        {
            return MessageBox.Show($"This {row} already exists!");
        }

        public DialogResult Invalid_errMsg(string col)
        {
            return MessageBox.Show($"{col} is empty or doesn't exist, enter valid {col}");
        }

        #endregion


        //Stores/////////////////////
        #region
        public void DisplayStores()
        {
            listBox1.Items.Clear();
            listBox1.Items.Add("Store name" + "\t" + "Store address" + "\t" + "Store keeper");
            foreach (var str in Ent.Stores)
            {
                listBox1.Items.Add(str.st_name + "\t" + "\t" + str.st_address + "\t" + "\t" + str.st_keeper);
            }
            comboBox1.Items.Clear();
            DisplayStores_Combo(comboBox1);
        }

        public void DisplayStores_Combo(ComboBox comb)
        {
            foreach (var str in Ent.Stores)
            {
                comb.Items.Add(str.st_name);
            }
        }

        public void CreateStore()
        {
            Store store = new Store();
            store.st_name = textBox1.Text;
            store.st_address = IsNotEmpty(textBox2);
            store.st_keeper = IsNotEmpty(textBox3);
            ClearTextBoxes();
            Ent.Stores.Add(store);
            Ent.SaveChanges();
            comboBox1.Items.Add(store.st_name);
            comboBox1.Text = "";
            MessageBox.Show("New store is added");
        }

        public void UpdateStore(Store str)
        {
            str.st_name = textBox1.Text;
            str.st_address = textBox2.Text;
            str.st_keeper = textBox3.Text;
            Ent.SaveChanges();
            ClearTextBoxes();
            comboBox1.Text = "";
            MessageBox.Show("The store is updated");
        }
        #endregion


        //Products//////////////////////////////
        #region
        public void DisplayProducts()
        {
            listBox2.Items.Clear();
            listBox2.Items.Add("Product ID" + "\t" + "Product name");
            foreach (var pr in Ent.Products)
            {
                listBox2.Items.Add(pr.product_id + "\t" + "\t" + pr.product_name);
            }
            comboBox2.Items.Clear();
            DisplayProducts_combo(comboBox2);
        }

        public void DisplayProducts_combo(ComboBox comb)
        {
            foreach (var pr in Ent.Products)
            {
                comb.Items.Add(pr.product_name);
            }
        }

        public void CreateProduct()
        {
            Product product = new Product();
            product.product_id = int.Parse(textBox4.Text);
            product.product_name = IsNotEmpty(textBox5);
            comboBox2.Items.Add(product.product_id);
            Ent.Products.Add(product);
            Ent.SaveChanges();
            ClearTextBoxes();
            MessageBox.Show("New Product is added");
        }

        public void UpdateProduct(Product prod)
        {
            prod.product_id = int.Parse(textBox4.Text);
            if (textBox5.Text != "")
            {
                prod.product_name = textBox5.Text;
            }
            else { MessageBox.Show("Choose supplier"); }
            Ent.SaveChanges();
            ClearTextBoxes();
            MessageBox.Show("The Product is updated");
        }
        #endregion


        ////Suppliers/////////////
        #region
        public void DisplaySuppliers_Combo(ComboBox comb)
        {
            foreach (var supp in Ent.Suppliers)
            {
                comb.Items.Add(supp.supp_name);
            }
        }

        public void DisplaySuppliers()
        {
            listBox3.Items.Clear();
            listBox3.Items.Add("Supplier Name" + "\t" + "Supplier Telephone" + "\t" + "Supplier Fax" + "\t" +
                "Supplier Mobile" + "\t" + "Supplier Email" + "\t" + "Supplier Site");
            foreach (var supp in Ent.Suppliers)
            {
                listBox3.Items.Add(supp.supp_name + "\t" + "\t" + supp.supp_tel + "\t" + supp.supp_fax
                     + "\t" + supp.supp_mob + "\t" + supp.supp_email + "\t" + supp.supp_site);
            }
            comboBox4.Items.Clear();
            DisplaySuppliers_Combo(comboBox4);
        }

        public void CreateSupplier()
        {

                Supplier supplier = new Supplier();
                supplier.supp_name = textBox6.Text;
                supplier.supp_tel = IsNotEmpty(textBox7);
                supplier.supp_fax = IsNotEmpty(textBox8);
                supplier.supp_mob = IsNotEmpty(textBox9);
                supplier.supp_email = IsNotEmpty(textBox10);
                supplier.supp_site = IsNotEmpty(textBox11);
                Ent.Suppliers.Add(supplier);
                Ent.SaveChanges();
                ClearTextBoxes();
                MessageBox.Show("New supplier is added");

        }

        public void UpdateSupplier(Supplier supplier)
        {
            supplier.supp_name = textBox6.Text;
            supplier.supp_tel = IsNotEmpty(textBox7);
            supplier.supp_fax = IsNotEmpty(textBox8);
            supplier.supp_mob = IsNotEmpty(textBox9);
            supplier.supp_email = IsNotEmpty(textBox10);
            supplier.supp_site = IsNotEmpty(textBox11);
            Ent.SaveChanges();
            ClearTextBoxes();
            MessageBox.Show("Supplier is updated");
        }
        #endregion


        ////Clients////////////////////
        #region
        
        public void DisplayClients_Combo(ComboBox comb)
        {
            foreach (var cl in Ent.Clients)
            {
                comb.Items.Add(cl.cl_name);
            }
        }
        public void DisplayClients()
        {
            listBox4.Items.Clear();
            listBox4.Items.Add("Client Name" + "\t" + "Client Telephone" + "\t" + "Client Fax" + "\t" + "\t" + "Client Mobile" + "\t" + "Client Email" + "\t" + "Client Site");
            foreach (var client in Ent.Clients)
            {
                listBox4.Items.Add(client.cl_name + "\t" + "\t" + client.cl_tel + "\t" + client.cl_fax
                   + "\t" + client.cl_mob + "\t" + client.cl_email + "\t" + client.cl_site);
            }
            comboBox5.Items.Clear();
            DisplayClients_Combo(comboBox5);
        }

        public void CreateClient()
        {
            Client client = new Client();
            client.cl_name = textBox12.Text;
            client.cl_tel = IsNotEmpty(textBox13);
            client.cl_fax = IsNotEmpty(textBox14);
            client.cl_mob = IsNotEmpty(textBox15);
            client.cl_email = IsNotEmpty(textBox16);
            client.cl_site = IsNotEmpty(textBox17);
            Ent.Clients.Add(client);
            Ent.SaveChanges();
            ClearTextBoxes();
            MessageBox.Show("New client is added");
        }

        public void UpdateClient(Client client)
        {
            client.cl_name = textBox12.Text;
            client.cl_tel = IsNotEmpty(textBox13);
            client.cl_fax = IsNotEmpty(textBox14);
            client.cl_mob = IsNotEmpty(textBox15);
            client.cl_email = IsNotEmpty(textBox16);
            client.cl_site = IsNotEmpty(textBox17);
            Ent.SaveChanges();
            ClearTextBoxes();
            MessageBox.Show("Client is updated");
        }
        #endregion


        ////Purchase Orders////////////////////

        #region
        public void DisplayPO_Combo(ComboBox comb)
        {
            foreach (var po in Ent.Purchase_orders)
            {
                comb.Items.Add(po.po_ID);
            }
        }
        public void DisplayPO()
        {
            listBox5.Items.Clear();
            listBox5.Items.Add("PO ID" + "\t" + "Po Date" + "\t" + "\t" + "Store Name" + "\t" + "Supplier name");

            foreach (var po in Ent.Purchase_orders)
            {
                var d = Convert.ToDateTime(po.po_date).Date.ToString("d");
                listBox5.Items.Add(po.po_ID + "\t" + d + "\t" + "\t" + po.Store.st_name + "\t" + "\t" + po.Supplier.supp_name);
            }
            comboBox6.Items.Clear();
            DisplayPO_Combo(comboBox6);
        }

        #endregion

        ////Transactions////////////////////

        
        #region

        public void DisplayTrans_Combo(ComboBox comb)
        {
            foreach (var trans in Ent.Transactions)
            {
                comb.Items.Add(trans.trans_ID);
            }
        }
        public void DisplayTrans()
        {
            listBox6.Items.Clear();
            listBox6.Items.Add("Trans ID" + "\t" + "\t" + "Trans Date" + "\t" + "Store Name" + "\t" + "Supplier name");

            foreach (var trans in Ent.Transactions)
            {
                var d = Convert.ToDateTime(trans.trans_date).Date.ToString("d");
                listBox6.Items.Add(trans.trans_ID + "\t" + "\t" + d + "\t" + "\t" + trans.Store.st_name + "\t" + "\t" + trans.Supplier.supp_name);
            }
            comboBox3.Items.Clear();
            DisplayTrans_Combo(comboBox3);
        }
        #endregion

        ////Store to store transfer////////////////////

        #region
        public void DisplayAllStores()
        {
            comboBox7.Items.Clear();
            comboBox8.Items.Clear();
            DisplayStores_Combo(comboBox7);
            DisplayStores_Combo(comboBox8);
        }


        #endregion

        //stores report
        #region
        public void DisplayStoresReport()
        {
            listBox7.Items.Clear();
            listBox7.Items.Add("Product Name \t Quantity");
            comboBox12.Items.Clear();
            DisplayStores_Combo(comboBox12);
        }

        #endregion

        // Products report
        #region
        public void DisplayProducts_report()
        {
            DisplayProducts_combo(comboBox13);
            listBox8.Items.Clear();
            listBox8.Items.Add("Store Name \t Quantity");
        }



        #endregion

        #endregion


        /////////////////////////////////////////////
        ////////////////BUTTON EVENTS////////////////
        /////////////////////////////////////////////
        #region
        ///////////Stores/////////
        #region

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string name = comboBox1.Text;
            var store = (from str in Ent.Stores
                         where str.st_name == name
                         select str).FirstOrDefault();
            textBox1.Text = store.st_name;
            textBox2.Text = store.st_address;
            textBox3.Text = store.st_keeper;
            textBox18.Text = store.st_id.ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "") 
            {
                string st = textBox1.Text;
                Store str = (from store in Ent.Stores
                             where store.st_name == st
                             select store).FirstOrDefault();

                if (str == null)
                {
                    CreateStore();
                    DisplayStores();
                }
                else { Duplicate_errMsg("store"); }

            }
            else { enterData_errMsg("store name", "store"); }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                Store str = Ent.Stores.Find(int.Parse(textBox18.Text));
                if (str != null)
                {
                    UpdateStore(str);
                    DisplayStores();
                }
                else { Invalid_errMsg("store name"); }
            }
            else { Invalid_errMsg("store name"); }

        }


        #endregion


        ///////////Products/////////
        #region

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            Product prod = (from product in Ent.Products
                            where product.product_name == comboBox2.Text
                            select product).FirstOrDefault();
            textBox4.Text = prod.product_id.ToString();
            textBox5.Text = prod.product_name;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if ((textBox4.Text != "") || (textBox4.Text != "" && textBox5.Text != ""))
            {
                int proID = int.Parse(textBox4.Text);
                Product prod = Ent.Products.Find(proID);
                if (prod == null)
                {
                    CreateProduct();
                    DisplayProducts();
                }
                else { Duplicate_errMsg("product"); }

            }
            else { enterData_errMsg("product ID", "product"); }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (textBox4.Text != "")
            {
                int proID = int.Parse(textBox4.Text);
                Product prod = Ent.Products.Find(proID);
                if (prod != null)
                {
                    UpdateProduct(prod);
                    DisplayProducts();
                }
                else { Invalid_errMsg("product ID"); }
            }
            else { Invalid_errMsg("product ID"); }

        }


        #endregion


        ///////////Suppliers/////////
        #region
        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            string Name = comboBox4.Text;
            var supp = (from sup in Ent.Suppliers
                        where sup.supp_name == Name
                        select sup).FirstOrDefault();
            textBox6.Text = supp.supp_name;
            textBox7.Text = supp.supp_tel;
            textBox8.Text = supp.supp_fax;
            textBox9.Text = supp.supp_mob;
            textBox10.Text = supp.supp_email;
            textBox11.Text = supp.supp_site;
            textBox19.Text = supp.supp_id.ToString();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (textBox6.Text == "")
            {
                enterData_errMsg("supplier name", "supplier");
            }
            else
            {
                string Name = textBox6.Text;
                var supp = (from sup in Ent.Suppliers
                            where sup.supp_name == Name
                            select sup).FirstOrDefault();
                if (supp == null)
                {
                    if (textBox10.Text == "")
                    {
                        CreateSupplier();
                        DisplaySuppliers();
                    }
                    else
                    {
                        if (IsEmailValid(textBox10.Text))
                        {
                            CreateSupplier();
                            DisplaySuppliers();
                        }
                    } 
                }
                else { Duplicate_errMsg("supplier"); }
            }

        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (textBox6.Text != "")
            {
                Supplier supplier = Ent.Suppliers.Find(int.Parse(textBox19.Text));
                if (supplier != null)
                {
                    if (textBox10.Text == "")
                    {
                        UpdateSupplier(supplier);
                        DisplaySuppliers();
                    }
                    else
                    {
                        if (IsEmailValid(textBox10.Text))
                        {
                            UpdateSupplier(supplier);
                            DisplaySuppliers();
                        }
                    }
                }
                else { Invalid_errMsg("supplier name"); }
            }
            else { Invalid_errMsg("supplier name"); }
        }


        #endregion


        ///////////Clients/////////
        #region

        private void comboBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
            string Name = comboBox5.Text;
            var client = (from cl in Ent.Clients
                          where cl.cl_name == Name
                          select cl).FirstOrDefault();
            textBox12.Text = client.cl_name;
            textBox13.Text = client.cl_tel;
            textBox14.Text = client.cl_fax;
            textBox15.Text = client.cl_mob;
            textBox16.Text = client.cl_email;
            textBox17.Text = client.cl_site;
            textBox20.Text = client.cl_id.ToString();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (textBox12.Text == "")
            {
                enterData_errMsg("client name", "client");
            }
            else
            {
                string Name = textBox12.Text;
                var client = (from cl in Ent.Clients
                              where cl.cl_name == Name
                              select cl).FirstOrDefault();
                if (client == null)
                {
                    if (textBox16.Text == "")
                    {
                        CreateClient();
                        DisplayClients();
                    }
                    else
                    {
                        if (IsEmailValid(textBox16.Text))
                        {
                            CreateClient();
                            DisplayClients();
                        }
                    }
                }
                else { Duplicate_errMsg("client"); }
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (textBox12.Text != "")
            {
                string Name = textBox12.Text;
                var client = Ent.Clients.Find(int.Parse(textBox20.Text));
                if (client != null)
                {
                    if (textBox16.Text == "")
                    {
                        UpdateClient(client);
                        DisplayClients();
                    }
                    else
                    {
                        if (IsEmailValid(textBox16.Text))
                        {
                            UpdateClient(client);
                            DisplayClients();
                        }
                    }
                }
                else { Invalid_errMsg("supplier name"); }
            }
            else { Invalid_errMsg("supplier name"); }
        }




        #endregion


        ///////////Purchase Orders/////////
        #region

        private void button9_Click(object sender, EventArgs e)
        {
            AddNewPO addform = new AddNewPO();
            addform.Show();
            this.Hide();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            if (comboBox6.Text != "")
            {
                int id = int.Parse(comboBox6.Text);
                var SelectedPo = Ent.Purchase_orders.Find(id);
                UpdatePO updateform = new UpdatePO(SelectedPo);
                updateform.Show();
                this.Hide();
            }
            else MessageBox.Show("select PO you want to update!");
            
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }


        #endregion


        ///////////////// Transactions /////////////////
        #region


        private void button11_Click(object sender, EventArgs e)
        {
            AddNewTrans addform = new AddNewTrans();
            addform.Show();
            this.Hide();
        }

        private void button12_Click(object sender, EventArgs e)
        {
            if (comboBox3.Text != "")
            {
                int id = int.Parse(comboBox3.Text);
                var selectedTrans = Ent.Transactions.Find(id);
                UpdateTrans updateform = new UpdateTrans(selectedTrans);
                updateform.Show();
                this.Hide();
            }
            else MessageBox.Show("select Transaction you want to update!");
        }

        #endregion


        /////////////// Store to Store transfer /////////////

        #region
        private void comboBox7_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox9.Items.Clear();
            comboBox9.Text = "";
            var name = comboBox7.Text;
            var store = (from str in Ent.Stores
                          where str.st_name == name
                          select str).FirstOrDefault();

            var selectedStore = (from products in Ent.Inventories
                                 where products.Store.st_id == store.st_id
                                 select products.Product.product_name).ToList();
            foreach(var item in selectedStore)
            {
                comboBox9.Items.Add(item);
            }

        }

        private void comboBox9_SelectedIndexChanged(object sender, EventArgs e)
        {
            var name = comboBox7.Text;
            var store = (from str in Ent.Stores
                         where str.st_name == name
                         select str).FirstOrDefault();
            string st = comboBox9.Text;
            var Pro_Id = (from product in Ent.Products
                         where product.product_name == st
                         select product.product_id).FirstOrDefault();
            var selectedProduct = (from prod in Ent.Inventories
                                  where prod.Product_id == Pro_Id && prod.Store_id == store.st_id
                                  select prod).FirstOrDefault();
            comboBox10.Items.Clear();
            comboBox11.Items.Clear();
            comboBox10.Text = comboBox11.Text = "";
            comboBox10.Items.Add(selectedProduct.Production_date);
            comboBox11.Items.Add(selectedProduct.Expiry_date);
            var supp = Ent.Suppliers.Find(selectedProduct.Supplier_id);
            textBox23.Text = supp.supp_name;
            textBox24.Text = selectedProduct.Quantity.ToString();
        }

        private void button13_Click(object sender, EventArgs e)
        {
            if (comboBox7.Text != "" && comboBox8.Text != "" && comboBox9.Text != ""
                && comboBox10.Text != "" && comboBox11.Text != "" && textBox22.Text != "" && textBox23.Text != "")
            {
                var selected_product = (from product in Ent.Products
                                         where product.product_name == comboBox9.Text
                                        select product).FirstOrDefault();

                var to_Store = (from stor in Ent.Stores
                                where stor.st_name == comboBox8.Text
                                select stor).FirstOrDefault();

                var from_store = (from str in Ent.Stores
                             where str.st_name == comboBox7.Text
                                  select str).FirstOrDefault();

                var from_store_inv = (from str in Ent.Inventories
                                      where str.Store_id == from_store.st_id && str.Product_id == selected_product.product_id 
                                      select str).FirstOrDefault();
                var to_Store_inv = (from str in Ent.Inventories
                                      where str.Store_id == to_Store.st_id && str.Product_id == selected_product.product_id
                                    select str).FirstOrDefault();
                var supp = Ent.Suppliers.Find(from_store_inv.Supplier_id);
                if (to_Store_inv == null)
                {
                    Inventory tostoreinv = new Inventory();

                    tostoreinv.Store_id = to_Store.st_id;
                    tostoreinv.Product_id = selected_product.product_id;
                    tostoreinv.Quantity = int.Parse(textBox22.Text);
                    tostoreinv.Supplier_id = supp.supp_id;
                    tostoreinv.Production_date = Convert.ToDateTime(comboBox10.Text);
                    tostoreinv.Expiry_date = Convert.ToDateTime(comboBox11.Text);

                    Ent.Inventories.Add(tostoreinv);
                    

                }
                else if (to_Store_inv != null)
                {
                    to_Store_inv.Store_id = to_Store.st_id;
                    to_Store_inv.Product_id = selected_product.product_id;
                    to_Store_inv.Quantity = int.Parse(textBox22.Text);
                    to_Store_inv.Supplier_id = supp.supp_id;
                    to_Store_inv.Production_date = DateTime.Parse(comboBox10.Text);
                    to_Store_inv.Expiry_date = DateTime.Parse(comboBox11.Text);
                    
                }
                var transfer_q = int.Parse(textBox22.Text);
                if (transfer_q <= from_store_inv.Quantity)
                {
                    var new_quantity = from_store_inv.Quantity - transfer_q;
                    if(new_quantity == 0)
                    {
                        Ent.Inventories.Remove(from_store_inv);
                    }
                    else
                    {
                        from_store_inv.Quantity = new_quantity;
                    }
                    
                    if(textBox25.Text != "")
                    {
                        StoreToStore_Transfers new_transfer = new StoreToStore_Transfers();
                        new_transfer.transfer_id = int.Parse(textBox25.Text);
                        new_transfer.product_id = selected_product.product_id;
                        new_transfer.from_store = from_store_inv.Store_id;
                        new_transfer.to_store = to_Store.st_id;
                        new_transfer.quantity = int.Parse(textBox22.Text);
                        new_transfer.production_date = DateTime.Parse(comboBox10.Text);
                        new_transfer.expiry_date = DateTime.Parse(comboBox11.Text);
                        new_transfer.supp_id = supp.supp_id;
                        Ent.StoreToStore_Transfers.Add(new_transfer);
                    }
                    else MessageBox.Show("enter transfer id");

                    Ent.SaveChanges();
                    ClearTextBoxes();
                    MessageBox.Show("Transfer Successful");
                } 
                else MessageBox.Show("no suffecient quantity to transfer");

            }
            else MessageBox.Show("Please fill all data");
        }
        #endregion


        ////////////////////Store Report/////////////////////
        #region
        private void comboBox12_SelectedIndexChanged(object sender, EventArgs e)
        {
            listBox7.Items.Clear();
            listBox7.Items.Add("Product Name \t Quantity");
            var selected_str = (from str in Ent.Inventories
                                where str.Store.st_name == comboBox12.Text
                                select str).ToList();

            foreach(var item in selected_str)
            {
                listBox7.Items.Add(item.Product.product_name + "\t" + "\t" + item.Quantity);
            }
            
        }
        #endregion


        ////////////////////Product Report/////////////////////
        #region
        private void comboBox13_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selected_product = (from prod in Ent.Products
                                    where prod.product_name == comboBox13.Text
                                    select prod).FirstOrDefault();
            var prd_str_inv = (from item in Ent.Inventories
                               where item.Product_id == selected_product.product_id
                               select item).ToList(); 
            listBox8.Items.Clear();
            listBox8.Items.Add("Store Name \t Quantity");
            comboBox14.Items.Clear();
            comboBox14.Text = "";
            foreach (var x in prd_str_inv)
            {
                listBox8.Items.Add(x.Store.st_name + "\t " + "\t " + x.Quantity);
                comboBox14.Items.Add(x.Store.st_name);
            }
            comboBox14.Enabled = true;
            
        }

        private void comboBox14_SelectedIndexChanged(object sender, EventArgs e)
        {

            var selected_product = (from prod in Ent.Products
                                    where prod.product_name == comboBox13.Text
                                    select prod).FirstOrDefault();
            var selected_store = (from str in Ent.Stores
                                  where str.st_name == comboBox14.Text
                                  select str).FirstOrDefault();
            var prd_str_inv = (from item in Ent.Inventories
                               where item.Product_id == selected_product.product_id && item.Store_id == selected_store.st_id
                               select item).ToList();


            listBox8.Items.Clear();
            listBox8.Items.Add("Store Name \t Quantity");
            foreach (var x in prd_str_inv)
            {
                listBox8.Items.Add(x.Store.st_name + "\t " + "\t " + x.Quantity);
            }

        }
        #endregion


        /////////////////Product IN/OUT Report/////////////////
        #region
        private void button16_Click(object sender, EventArgs e)
        {
            if (comboBox16.Text != "")
            {
                var selected_prod = (from prod in Ent.Products
                                     where prod.product_name == comboBox16.Text
                                     select prod).FirstOrDefault();
                var PO_selectedProd = (from poProd in Ent.PO_Products
                                       where poProd.product_id == selected_prod.product_id
                                       select poProd).ToList();
                listBox11.Items.Clear();listBox12.Items.Clear();listBox13.Items.Clear();
                listBox11.Items.Add("Purchase order date \t Store \t Quantity \t supplier");
                foreach (var item in PO_selectedProd)
                {
                    listBox11.Items.Add($"{item.Purchase_orders.po_date} \t {item.Purchase_orders.Store.st_name} \t {item.quantity} \t {item.Purchase_orders.Supplier.supp_name}");
                }
            }
            else { MessageBox.Show("select product"); }
        }

        private void button17_Click(object sender, EventArgs e)
        {
            if (comboBox16.Text != "")
            {
                var selected_prod = (from prod in Ent.Products
                                     where prod.product_name == comboBox16.Text
                                     select prod).FirstOrDefault();
                var Trans_selectedProd = (from transProd in Ent.Trans_Products
                                          where transProd.product_id == selected_prod.product_id
                                          select transProd).ToList();
                listBox11.Items.Clear(); listBox12.Items.Clear(); listBox13.Items.Clear();
                listBox12.Items.Add("Transaction date \t Store \t Quantity");
                foreach (var item in Trans_selectedProd)
                {
                    listBox12.Items.Add($"{item.Transaction.trans_date} \t {item.Transaction.Store.st_name} \t {item.quantity}");
                }
            }
            else { MessageBox.Show("select product"); }
        }

        private void button18_Click(object sender, EventArgs e)
        {
            if (comboBox16.Text != "")
            {
                var selected_prod = (from prod in Ent.Products
                                     where prod.product_name == comboBox16.Text
                                     select prod).FirstOrDefault();
                var transfer_report = (from transStr in Ent.StoreToStore_Transfers
                                       where transStr.product_id == selected_prod.product_id
                                       select transStr).ToList();
                listBox11.Items.Clear(); listBox12.Items.Clear(); listBox13.Items.Clear();
                listBox13.Items.Add("From Store       To Store       Quantity");
                foreach (var item in transfer_report)
                {
                    var from_storename = Ent.Stores.Find(item.from_store);
                    var to_storename = Ent.Stores.Find(item.to_store);
                    listBox13.Items.Add($"{from_storename.st_name}\t      {to_storename.st_name}\t    {item.quantity}");
                }
            }
            else { MessageBox.Show("select product"); }
        }

        #endregion


        /////////////////Product Date range////////////////////
        #region
        private void button15_Click(object sender, EventArgs e)
        {
            var from_date = dateTimePicker3.Value;
            var to_date = dateTimePicker4.Value;

            var prod_po_purchased = (from prod in Ent.PO_Products
                                      where prod.Purchase_orders.po_date >= from_date && prod.Purchase_orders.po_date <= to_date
                                      select prod).ToList();
            var products_trans_sold = (from prod in Ent.Trans_Products
                                        where prod.Transaction.trans_date >= from_date && prod.Transaction.trans_date <= to_date
                                        select prod).ToList();
            var selected_store = (from str in Ent.Stores
                                  where str.st_name == comboBox15.Text
                                  select str).FirstOrDefault();
            List<Product> displayed_products = new List<Product>();
            foreach (var item1 in prod_po_purchased)
            {
                displayed_products.Add(item1.Product);
            }
            foreach (var item2 in products_trans_sold)
            {
                displayed_products.Remove(item2.Product);
            }

            listBox10.Items.Clear();
            listBox10.Items.Add("Product Name \t Store Name \t Quantity");
            foreach(var prod in displayed_products)
            {
                var str_prod_inv = (from x in Ent.Inventories
                                    where x.Product_id == prod.product_id && x.Store_id == selected_store.st_id
                                    select x).FirstOrDefault();

                   listBox10.Items.Add(str_prod_inv.Product.product_name + "\t " + "\t " + str_prod_inv.Store.st_name + "\t " + "\t " + str_prod_inv.Quantity); 

            }


        }
        #endregion


        ////////////////////Near Expired///////////////////////
        #region
        private void button14_Click(object sender, EventArgs e)
        {
            var from_date = dateTimePicker1.Value;
            var to_date = dateTimePicker2.Value;
            var current_date = DateTime.Now.Date;
            var displayed_products = (from prod in Ent.Inventories
                                      where prod.Expiry_date >= from_date && prod.Expiry_date <= to_date
                                      select prod).ToList();
            listBox9.Items.Clear();
            listBox9.Items.Add("Product \t \t remaining days until expiry");
            foreach (var item in displayed_products)
            {
                var remaining_days = (current_date.Date - item.Expiry_date.Value.Date).Days;
                if (remaining_days > 0)
                { listBox9.Items.Add(item.Product.product_name + "\t " + "\t " + "\t " + remaining_days + "days"); }
                else { listBox9.Items.Add(item.Product.product_name + "\t " + "\t " + "\t " + "Expired"); }
            }
        }
        #endregion

        #endregion
    }


    public static class ControlExtensionMethods
    {
        public static IEnumerable GetOffsprings(this Control @this)
        {
            foreach (Control child in @this.Controls)
            {
                yield return child;
                foreach (var offspring in GetOffsprings(child))
                    yield return offspring;
            }
        }
    }
    
}

