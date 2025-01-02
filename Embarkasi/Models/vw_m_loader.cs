using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Embarkasi.Models
{
    [Table("vw_m_loader")]
    public class vw_m_loader
    {

        [Column("sektor")]
        public string? sektor { get; set; }
        [Key]
        [Column("loader")]
        public string? loader { get; set; }

        [Column("front")]
        public string? front { get; set; }

        [Column("transportasi")]
        public string? transportasi { get; set; }
    }
}
