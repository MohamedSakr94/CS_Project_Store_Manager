namespace CS_Project_Store_Manager
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Client
    {
        [Key]
        public int cl_id { get; set; }

        [Required]
        [StringLength(30)]
        public string cl_name { get; set; }

        [StringLength(20)]
        public string cl_tel { get; set; }

        [StringLength(20)]
        public string cl_fax { get; set; }

        [StringLength(20)]
        public string cl_mob { get; set; }

        [StringLength(50)]
        public string cl_email { get; set; }

        [StringLength(50)]
        public string cl_site { get; set; }
    }
}
