using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Embarkasi.Models
{
    [Table("tbl_t_change_fleet_log")]
    public class tbl_t_change_fleet_log
    {
        [Key]
        public Guid pid { get; set; } = Guid.NewGuid();
        [MaxLength(10)]
        public string? unit { get; set; }
        [MaxLength(50)]
        public string? fleet_from { get; set; }
        [MaxLength(50)]
        public string? fleet_to { get; set; }
        public DateTime created_at { get; set; } = DateTime.Now;
        [MaxLength(50)]
        public string? created_by { get; set; }
    }

}
