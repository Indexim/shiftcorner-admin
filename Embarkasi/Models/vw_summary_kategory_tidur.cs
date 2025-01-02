using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Embarkasi.Models
{
    [Table("vw_summary_kategory_tidur", Schema = "public")]
    public class vw_summary_kategory_tidur
    {
        [Key]
        [Column("id")]
        public Guid id { get; set; } 

        [Column("smartwatch")]
        public string? smartwatch { get; set; }

        [Column("tanggal")]
        [StringLength(50)]
        public DateTime? tanggal { get; set; }

        [Column("nik")]
        [StringLength(50)]
        public string? nik { get; set; }

        [Column("nama_lengkap")]
        [StringLength(100)]
        public string? nama_lengkap { get; set; }

        [Column("unit")]
        public string? unit { get; set; }

        [Column("shift")]
        public string? shift { get; set; }

        [Column("kategori_tidur")]
        public string? kategori_tidur { get; set; }

        [Column("finger_date")]
        public DateTime? finger_date { get; set; }

        [Column("tidak_upload_ke")]
        [StringLength(100)]
        public string? tidak_upload_ke { get; set; }
    }
}
