using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Embarkasi.Models
{
    [Table("tbl_m_running_text")]
    public class tbl_m_running_text
    {
        [Key]
        [Column("id")]
        public int id { get; set; }

        [Column("text")]
        public string? text { get; set; }

        [Column("created_by")]
        public string? created_by { get; set; }

        [Column("updated_by")]
        public string? updated_by { get; set; }

        [Column("color")]
        public string? color { get; set; }
    }
}
