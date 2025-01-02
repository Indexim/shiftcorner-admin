using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Embarkasi.Models
{
    [Table("vw_m_in_out_karyawan")]
    public class vw_m_in_out_karyawan
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Menandakan ID dihasilkan otomatis
        [Column("id")]
        public Guid id { get; set; } // Ubah tipe menjadi Guid

        [Column("nik")]
        public string? nik { get; set; }

        [Column("nama")]
        public string? nama { get; set; }

        [Column("jam_finger_in")]
        public DateTime? jam_finger_in { get; set; }

        [Column("jam_finger_out")]
        public DateTime? jam_finger_out { get; set; }

        [Column("fingerprint_ip_in")]
        public string? fingerprint_ip_in { get; set; }

        [Column("fingerprint_ip_out")]
        public string? fingerprint_ip_out { get; set; }

        [Column("shift_system")]
        public string? shift_system { get; set; }

        [Column("shift_settingan")]
        public string? shift_settingan { get; set; }

        [Column("unit")]
        public string? unit { get; set; }
    }
}
