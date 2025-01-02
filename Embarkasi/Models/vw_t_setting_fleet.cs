using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Embarkasi.Models
{
    [Table("vw_t_setting_fleet")]
    public class vw_t_setting_fleet
    {
        [Key]
        public Guid id { get; set; } // Sesuaikan pid dengan id
        public string? cn_unit { get; set; }
        public string? eq_class { get; set; }
        public string? egi { get; set; }
        public bool? is_active { get; set; }
        public bool? is_standby { get; set; }
        public bool? is_breakdown { get; set; }
        public DateTime? tanggal { get; set; }
        public string? shift { get; set; }
        public string? sektor { get; set; }
        public string? loader { get; set; }
        public string? hauler { get; set; }
        public string? front { get; set; }
        public string? disposal { get; set; }
        public string? group_leader { get; set; }
        public bool? loader_is_breadown { get; set; } 
        public bool? hauler_is_breadown { get; set; }
        public string? loader_operator_nrp { get; set; }
        public string? loader_operator_nama { get; set; }
        public string? loader_ftw_status { get; set; }
        public int? loader_ftw_jam_fatigue { get; set; }
        public DateTime? loader_absensi_in { get; set; }
        public string? loader_absensi_in_lokasi { get; set; }
        public string? hauler_operator_nrp { get; set; }
        public string? hauler_operator_nama { get; set; }
        public string? hauler_ftw_status { get; set; }
        public int? hauler_ftw_jam_fatigue { get; set; }
        public DateTime? hauler_absensi_in { get; set; }
        public string? hauler_absensi_in_lokasi { get; set; }
        public DateTime? loader_created_at { get; set; }
        public string? loader_created_by { get; set; }
        public DateTime? loader_updated_at { get; set; }
        public string? loader_updated_by { get; set; }
        public DateTime? loader_is_breakdown_updated_at { get; set; }
        public DateTime? loader_operator_updated_at { get; set; }
        public DateTime? loader_ftw_updated_at { get; set; }
        public DateTime? loader_absensi_updated_at { get; set; }
        public DateTime? hauler_created_at { get; set; }
        public string? hauler_created_by { get; set; }
        public DateTime? hauler_is_breakdown_updated_at { get; set; }
        public DateTime? hauler_operator_updated_at { get; set; }
        public DateTime? hauler_ftw_updated_at { get; set; }
        public DateTime? hauler_absensi_updated_at { get; set; }
        public DateTime? sektor_updated_at { get; set; }
        public DateTime? gl_updated_at { get; set; }
        public string? sektor_updated_by { get; set; }
        public DateTime? front_updated_at { get; set; }
        public string? front_updated_by { get; set; }
        public DateTime? disposal_updated_at { get; set; }
        public string? disposal_updated_by { get; set; }
        public string? transportasi { get; set; }
        public string? status_warna { get; set; }
        public string? data_source { get; set; }
        public string? status_absen { get; set; }
    }
}
