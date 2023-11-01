namespace CS_Project_Store_Manager
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Inventory")]
    public partial class Inventory
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Store_id { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Product_id { get; set; }

        public int? Quantity { get; set; }

        public int? Supplier_id { get; set; }

        [Key]
        [Column(Order = 2, TypeName = "date")]
        public DateTime Production_date { get; set; }

        [Column(TypeName = "date")]
        public DateTime? Expiry_date { get; set; }

        public virtual Product Product { get; set; }

        public virtual Store Store { get; set; }
    }
}
