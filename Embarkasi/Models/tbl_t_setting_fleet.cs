using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Embarkasi.Models
{
    public class tbl_t_setting_fleet
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column(TypeName = "uuid")] 
        public Guid? id { get; set; }

        [Column(TypeName = "date")]
        public DateTime? tanggal { get; set; }

        [StringLength(255)]
        public string? shift { get; set; }

        [StringLength(255)]
        public string? sektor { get; set; }

        [StringLength(255)]
        public string? loader { get; set; }

        [StringLength(255)]
        public string? hauler { get; set; }

        [StringLength(255)]
        public string? front { get; set; }

        [StringLength(255)]
        public string? disposal { get; set; }

        [StringLength(255)]
        public string? group_leader { get; set; }

        public bool? loader_is_breadown { get; set; }

        public bool? hauler_is_breadown { get; set; }

        [StringLength(255)]
        public string? loader_operator_nrp { get; set; }

        [StringLength(255)]
        public string? loader_operator_nama { get; set; }

        [StringLength(255)]
        public string? loader_ftw_status { get; set; }

        public int? loader_ftw_jam_fatigue { get; set; }

        public TimeSpan? loader_absensi_in { get; set; }

        [StringLength(255)]
        public string? loader_absensi_in_lokasi { get; set; }

        [StringLength(255)]
        public string? hauler_operator_nrp { get; set; }

        [StringLength(255)]
        public string? hauler_operator_nama { get; set; }

        [StringLength(255)]
        public string? hauler_ftw_status { get; set; }

        public int? hauler_ftw_jam_fatigue { get; set; }

        public TimeSpan? hauler_absensi_in { get; set; }

        [StringLength(255)]
        public string? hauler_absensi_in_lokasi { get; set; }

        public DateTime? loader_created_at { get; set; }

        [StringLength(255)]
        public string? loader_created_by { get; set; }

        public DateTime? loader_updated_at { get; set; }

        [StringLength(255)]
        public string? loader_updated_by { get; set; }

        public DateTime? loader_is_breakdown_updated_at { get; set; }

        public DateTime? loader_operator_updated_at { get; set; }

        public DateTime? loader_ftw_updated_at { get; set; }

        public DateTime? loader_absensi_updated_at { get; set; }

        public DateTime? hauler_created_at { get; set; }

        [StringLength(255)]
        public string? hauler_created_by { get; set; }

        public DateTime? hauler_is_breakdown_updated_at { get; set; }

        public DateTime? hauler_operator_updated_at { get; set; }

        public DateTime? hauler_ftw_updated_at { get; set; }

        public DateTime? hauler_absensi_updated_at { get; set; }

        public DateTime? sektor_updated_at { get; set; }

        public DateTime? gl_updated_at { get; set; }

        [StringLength(255)]
        public string? sektor_updated_by { get; set; }
        [Column(TypeName = "timestamp with time zone")]
        public DateTime? front_updated_at { get; set; }

        [StringLength(255)]
        public string? front_updated_by { get; set; }

        public DateTime? disposal_updated_at { get; set; }

        [StringLength(255)]
        public string? disposal_updated_by { get; set; }

        public DateTime? updated_at { get; set; }
        public string? transportasi { get; set; }
    }
}
