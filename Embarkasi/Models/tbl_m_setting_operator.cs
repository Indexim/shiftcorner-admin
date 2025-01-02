using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Embarkasi.Models
{
    [Table("tbl_m_setting_operator")]
    public class tbl_m_setting_operator
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column(TypeName = "uuid")]
        public Guid? id { get; set; }

        [Column("tanggal")]
        public DateOnly? tanggal { get; set; }

        [Column("nik")]
        [StringLength(50)]
        public string? nik { get; set; }

        [Column("unit")]
        [StringLength(100)]
        public string? unit { get; set; }

        [Column("status")]
        public bool? status { get; set; }

        [Column("shift")]
        public string? shift { get; set; }

        [Column("created_at")]
        public DateTime? createdat { get; set; } = DateTime.Now;

        [Column("updated_at")]
        public DateTime? updatedat { get; set; } = DateTime.Now;

        [Column("updated_by")]
        [StringLength(100)]
        public string? updatedby { get; set; }
    }
}
