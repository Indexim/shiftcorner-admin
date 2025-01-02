using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Embarkasi.Models
{
    [Table("tbl_m_tempudo")]
    public class tbl_m_tempudo
    {
        [Key]
        [Column("id")]
        public int id { get; set; }

        [Column("tempudo")]
        public string? tempudo { get; set; }

        [Column("status")]
        public Boolean? status { get; set; }

        [Column("type")]
        public string? type { get; set; }

        [Column("created_at")]
        public string? created_at { get; set; }

        [Column("updated_at")]
        public string? updated_at { get; set; }
    }
}
