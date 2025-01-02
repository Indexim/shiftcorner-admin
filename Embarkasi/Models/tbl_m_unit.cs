using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Embarkasi.Models
{
    [Table("tbl_m_unit")]
    public class tbl_m_unit
    {
        [Key]
        [Column("id")]
        public Guid id { get; set; } = Guid.NewGuid();

        [Column("cn_unit")]
        public string? cn_unit { get; set; }

        [Column("eq_class")]
        public string? eq_class { get; set; }

        [Column("egi")]
        public string? egi { get; set; }

        [Column("is_active")]
        public bool? is_active { get; set; }

        [Column("is_standby")]
        public bool? is_standby { get; set; }

        [Column("is_breakdown")]
        public bool? is_breakdown { get; set; }

        [Column("product")]
        public string? product { get; set; }


        [Column("created_at")]
        public DateTime created_at { get; set; }

        [Column("created_by")]
        public string? created_by { get; set; }

        [Column("updated_at")]
        public DateTime? updated_at { get; set; }

        [Column("updated_by")]
        public string? updated_by { get; set; }
    }
}
