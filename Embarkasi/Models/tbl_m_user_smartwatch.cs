using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Embarkasi.Models
{
    [Table("tbl_m_user_smartwatch")]
    public class tbl_m_user_smartwatch
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column(TypeName = "uuid")]
        public Guid? id { get; set; }

        [Column("nik")]
        [StringLength(50)]
        public string? nik { get; set; }

        [Column("smartwatch")]
        [StringLength(100)]
        public string? smartwatch { get; set; }

        [Column("status")]
        public bool? status { get; set; }

        [Column("created_at")]
        public DateTime? created_at { get; set; } = DateTime.Now;

        [Column("createc_by")]
        public string? createc_by { get; set; }

    }
}
