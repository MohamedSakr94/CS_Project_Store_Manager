namespace CS_Project_Store_Manager
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class PO_Products
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int product_id { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int po_ID { get; set; }

        [Column(TypeName = "date")]
        public DateTime production_date { get; set; }

        [Column(TypeName = "date")]
        public DateTime expiry_date { get; set; }

        public int quantity { get; set; }

        public virtual Product Product { get; set; }

        public virtual Purchase_orders Purchase_orders { get; set; }
    }
}
