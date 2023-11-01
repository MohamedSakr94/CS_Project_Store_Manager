using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace CS_Project_Store_Manager
{
    public partial class CFModel : DbContext
    {
        public CFModel()
            : base("name=CFModel6")
        {
        }

        public virtual DbSet<Client> Clients { get; set; }
        public virtual DbSet<Inventory> Inventories { get; set; }
        public virtual DbSet<PO_Products> PO_Products { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Products_MeasuringUnit> Products_MeasuringUnit { get; set; }
        public virtual DbSet<Purchase_orders> Purchase_orders { get; set; }
        public virtual DbSet<Store> Stores { get; set; }
        public virtual DbSet<StoreToStore_Transfers> StoreToStore_Transfers { get; set; }
        public virtual DbSet<Supplier> Suppliers { get; set; }
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
        public virtual DbSet<Trans_Products> Trans_Products { get; set; }
        public virtual DbSet<Transaction> Transactions { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>()
                .HasMany(e => e.Inventories)
                .WithRequired(e => e.Product)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Product>()
                .HasMany(e => e.PO_Products)
                .WithRequired(e => e.Product)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Product>()
                .HasMany(e => e.Products_MeasuringUnit)
                .WithRequired(e => e.Product)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Product>()
                .HasMany(e => e.StoreToStore_Transfers)
                .WithRequired(e => e.Product)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Product>()
                .HasMany(e => e.Trans_Products)
                .WithRequired(e => e.Product)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Products_MeasuringUnit>()
                .Property(e => e.product_MesUnit)
                .IsFixedLength();

            modelBuilder.Entity<Purchase_orders>()
                .HasMany(e => e.PO_Products)
                .WithRequired(e => e.Purchase_orders)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Store>()
                .HasMany(e => e.Inventories)
                .WithRequired(e => e.Store)
                .HasForeignKey(e => e.Store_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Store>()
                .HasMany(e => e.Purchase_orders)
                .WithRequired(e => e.Store)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Store>()
                .HasMany(e => e.Transactions)
                .WithRequired(e => e.Store)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Supplier>()
                .HasMany(e => e.Purchase_orders)
                .WithRequired(e => e.Supplier)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Supplier>()
                .HasMany(e => e.Transactions)
                .WithRequired(e => e.Supplier)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Transaction>()
                .HasMany(e => e.Trans_Products)
                .WithRequired(e => e.Transaction)
                .WillCascadeOnDelete(false);
        }
    }
}
