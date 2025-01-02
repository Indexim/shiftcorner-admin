using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Embarkasi.Models
{
    [Table("tbl_m_unit_bus")]
    public class tbl_m_unit_bus
    {
        [Key]
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }


        [Column("unit")]
        public string? unit { get; set; }

        [Column("status")]
        public int? status { get; set; }

        [Column("type")]
        public string? type { get; set; }

        [Column("created_at")]
        public string? created_at { get; set; }

        [Column("updated_at")]
        public string? updated_at { get; set; }
    }
}
