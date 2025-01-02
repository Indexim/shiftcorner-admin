using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Embarkasi.Models
{
    [Table("vw_m_finger_operator")]
    public class vw_m_finger_operator
    {
        [Key]
        [Column("id")]
        public int id { get; set; }
        [Column("nama_lengkap")]
        public string? nama_lengkap { get; set; }

        [Column("unit")]
        public string? unit { get; set; }


        [Column("shift")]
        public string? shift { get; set; }

        [Column("nik")]
        public string? nik { get; set; }

        [Column("jam_finger")]
        public DateTime? jam_finger { get; set; }

        [Column("status_kerja")]
        public string? status_kerja { get; set; }

        [Column("fingerprint_id")]
        public string? fingerprint_id { get; set; }

        [Column("fingerprint_ip")]
        public string? fingerprint_ip { get; set; }
        [Column("status_cetak")]
        public string? status_cetak { get; set; }
        [Column("status_waktu_with_kerja")]
        public string? status_waktu_with_kerja { get; set; }
    }
}
