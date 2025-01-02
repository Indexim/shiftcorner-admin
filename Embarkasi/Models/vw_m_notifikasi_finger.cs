using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Embarkasi.Models
{
    [Table("vw_m_notifikasi_finger")]
    public class vw_m_notifikasi_finger
    {
        [Key]
        [Column("id")]
        public int id { get; set; }
        [Column("nik")]
        public string? nik { get; set; }
        [Column("nama_lengkap")]
        public string? nama_lengkap { get; set; }
    }
}
