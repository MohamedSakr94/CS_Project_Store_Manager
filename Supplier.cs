namespace CS_Project_Store_Manager
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Supplier
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Supplier()
        {
            Purchase_orders = new HashSet<Purchase_orders>();
            StoreToStore_Transfers = new HashSet<StoreToStore_Transfers>();
            Transactions = new HashSet<Transaction>();
        }

        [Key]
        public int supp_id { get; set; }

        [Required]
        [StringLength(30)]
        public string supp_name { get; set; }

        [StringLength(20)]
        public string supp_tel { get; set; }

        [StringLength(20)]
        public string supp_fax { get; set; }

        [StringLength(20)]
        public string supp_mob { get; set; }

        [StringLength(50)]
        public string supp_email { get; set; }

        [StringLength(50)]
        public string supp_site { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Purchase_orders> Purchase_orders { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<StoreToStore_Transfers> StoreToStore_Transfers { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Transaction> Transactions { get; set; }
    }
}
