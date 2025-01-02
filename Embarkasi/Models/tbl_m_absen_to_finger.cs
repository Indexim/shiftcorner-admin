using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Embarkasi.Models
{
    [Table("tbl_m_absen_to_finger")]
    public class tbl_m_absen_to_finger
    {
        [Key]
        [Column("id")]
        public int id { get; set; }

        [Column("ip_mesin_finger")]
        public string? ip_mesin_finger { get; set; }

        [Column("ip_mesin_print")]
        public string? ip_mesin_print { get; set; }
        [Column("nama_printer")]
        public string? nama_printer { get; set; }

        [Column("status")]
        public Boolean? status { get; set; }

        [Column("created_at")]
        public string? created_at { get; set; }

        [Column("updated_at")]
        public string? updated_at { get; set; }
    }
}
