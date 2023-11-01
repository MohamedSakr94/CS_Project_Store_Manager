namespace CS_Project_Store_Manager
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class StoreToStore_Transfers
    {
        [Key]
        [Column(Order = 0)]
        public int transfer_id { get; set; }

        [Key]
        [Column(Order = 1)]
        public int product_id { get; set; }

        [Key]
        [Column(Order = 2, TypeName = "date")]
        public DateTime production_date { get; set; }

        [Column(TypeName = "date")]
        public DateTime? expiry_date { get; set; }

        public int? from_store { get; set; }

        public int? to_store { get; set; }

        public int? quantity { get; set; }

        public int? supp_id { get; set; }

        public virtual Product Product { get; set; }

        public virtual Supplier Supplier { get; set; }
    }
}
