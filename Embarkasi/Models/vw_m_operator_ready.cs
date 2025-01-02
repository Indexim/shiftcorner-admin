using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Embarkasi.Models
{
    [Table("vw_m_operator_ready")]
    public class vw_m_operator_ready
    {
        [Key]
        [Column("tanggal")]
        public DateOnly? tanggal { get; set; }

        [Column("finger_date")]
        public DateTime finger_date { get; set; }

        [Column("nik")]
        public string? nik { get; set; }

        [Column("nama_lengkap")]
        public string? nama_lengkap { get; set; }

        [Column("kategori_tidur")]
        public string? kategori_tidur { get; set; }
    }
}
