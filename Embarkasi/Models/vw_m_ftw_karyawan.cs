using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Embarkasi.Models
{
    [Table("vw_m_ftw_karyawan")]
    public class vw_m_ftw_karyawan
    {
        [Key]
        [Column("id")]
        public int id { get; set; }

        [Column("nrp")]
        public string? nrp { get; set; }

        [Column("name")]
        public string? name { get; set; }

        [Column("site")]
        public string? site { get; set; }

        [Column("site_name")]
        public string? site_name { get; set; }

        [Column("shift")]
        public string? shift { get; set; }

        [Column("department")]
        public string? department { get; set; }

        [Column("tanggal_input")]
        public DateTime? tanggal_input { get; set; }

        [Column("tanggal_terakhir_absen")]
        public DateTime? tanggal_terakhir_absen { get; set; }

        [Column("shift_hari_ini")]
        public string? shift_hari_ini { get; set; }

        [Column("shift_kemarin")]
        public string? shift_kemarin { get; set; }

        [Column("kategori_tidur")]
        public string? kategori_tidur { get; set; }
    }
}
