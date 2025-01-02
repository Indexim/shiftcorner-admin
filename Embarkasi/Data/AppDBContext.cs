using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Embarkasi.Models;
using Npgsql;

namespace Embarkasi.Data
{
    public class AppDBContext : DbContext
    {
        protected readonly IConfiguration Configuration;

        public AppDBContext(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseNpgsql(Configuration.GetConnectionString("WebApiDatabase"))
                .LogTo(Console.WriteLine);
        }

        public DbSet<tbl_t_setting_fleet> tbl_t_setting_fleet { get; set; }
        public DbSet<tbl_r_dept> tbl_r_depts { get; set; }
        public DbSet<tbl_m_user_login> tbl_m_user_login { get; set; }
        public DbSet<tbl_r_kategori_user> tbl_r_kategori_user { get; set; }
        public DbSet<tbl_r_menu> tbl_r_menu { get; set; }
        public DbSet<tbl_m_setting_aplikasi> tbl_m_setting_aplikasi { get; set; }
        public DbSet<tbl_m_unit> tbl_m_unit { get; set; }
        public DbSet<tbl_t_change_fleet_log> tbl_t_change_fleet_logs { get; set; }
        public DbSet<vw_t_setting_fleet> vw_t_setting_fleet { get; set; }
        public DbSet<tbl_m_unit_bus> tbl_m_unit_bus { get; set; }
        public DbSet<tbl_m_area> tbl_m_area { get; set; }
        public DbSet<tbl_m_tempudo> tbl_m_tempudo { get; set; }
        public DbSet<tbl_m_setting_operator> tbl_m_setting_operator { get; set; }
        public DbSet<vw_m_ftw_karyawan> vw_m_ftw_karyawan { get; set; }
        public DbSet<tbl_t_fingerprint> tbl_t_fingerprint { get; set; }
        public DbSet<vw_m_finger_operator> vw_m_finger_operator { get; set; }
        public DbSet<tbl_m_absen_to_finger> tbl_m_absen_to_finger { get; set; }
        public DbSet<vw_m_loader> vw_m_loader { get; set; }
        public DbSet<vw_m_setting_operator> vw_m_setting_operator { get; set; }
        public DbSet<tbl_m_running_text> tbl_m_running_text { get; set; }
        public DbSet<tbl_m_user_smartwatch> tbl_m_user_smartwatch { get; set; }
        public DbSet<vw_m_in_out_karyawan> vw_m_in_out_karyawan { get; set; }
        public DbSet<vw_m_notifikasi_finger> vw_m_notifikasi_finger { get; set; }
        public DbSet<vw_m_operator_ready> vw_m_operator_ready { get; set; }
        public DbSet<vw_summary_kategory_tidur> vw_summary_kategory_tidur { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<tbl_m_absen_to_finger>(entity =>
            {
                entity.HasKey(e => e.id);
            });

            modelBuilder.Entity<vw_summary_kategory_tidur>(entity =>
            {
                entity.HasKey(e => e.id);
            });

            modelBuilder.Entity<tbl_m_setting_aplikasi>(entity =>
            {
                entity.HasKey(e => e.id);
            });

            modelBuilder.Entity<tbl_r_dept>(entity =>
            {
                entity.HasKey(e => e.id);
            });

            modelBuilder.Entity<tbl_m_running_text>(entity =>
            {
                entity.HasKey(e => e.id);
            });

            modelBuilder.Entity<tbl_m_user_smartwatch>(entity =>
            {
                entity.HasKey(e => e.id);
            });


            modelBuilder.Entity<vw_m_loader>(entity =>
            {
                entity.HasKey(e => e.loader);
            });

            modelBuilder.Entity<tbl_m_user_login>(entity =>
            {
                entity.HasKey(e => e.id);
            });

            modelBuilder.Entity<tbl_r_kategori_user>(entity =>
            {
                entity.HasKey(e => e.id);
            });

            modelBuilder.Entity<tbl_r_menu>(entity =>
            {
                entity.HasKey(e => e.id);
            });

            modelBuilder.Entity<tbl_m_unit>(entity =>
            {
                entity.HasKey(e => e.id);
            });

            modelBuilder.Entity<tbl_t_setting_fleet>(entity =>
            {
                entity.HasKey(e => e.id);
            });
            modelBuilder.Entity<tbl_t_change_fleet_log>(entity =>
            {
                entity.HasKey(e => e.pid);
            });

            modelBuilder.Entity<vw_t_setting_fleet>(entity =>
            {
                entity.HasKey(e => e.id);
            });
            modelBuilder.Entity<vw_m_ftw_karyawan>(entity =>
            {
                entity.HasKey(e => e.id);
            });
            modelBuilder.Entity<tbl_m_unit_bus>(entity =>
            {
                entity.HasKey(e => e.id);
            });
            modelBuilder.Entity<tbl_m_area>(entity =>
            {
                entity.HasKey(e => e.id);
            });

            modelBuilder.Entity<tbl_m_tempudo>(entity =>
            {
                entity.HasKey(e => e.id);
            });
            modelBuilder.Entity<tbl_m_setting_operator>(entity =>
            {
                entity.HasKey(e => e.id);
            });
            modelBuilder.Entity<vw_m_setting_operator>(entity =>
            {
                entity.HasKey(e => e.id);
            });
            modelBuilder.Entity<tbl_t_fingerprint>(entity =>
            {
                entity.HasKey(e => e.id);
            });

            modelBuilder.Entity<vw_m_finger_operator>(entity =>
            {
                entity.HasKey(e => e.id);
            });
            modelBuilder.Entity<vw_m_notifikasi_finger>(entity =>
            {
                entity.HasKey(e => e.id);
            });
            modelBuilder.Entity<vw_m_operator_ready>(entity =>
            {
                entity.HasKey(e => e.nik);
            });

            modelBuilder.Entity<vw_m_in_out_karyawan>(entity =>
            {
                entity.Property(e => e.id)
             .HasDefaultValueSql("gen_random_uuid()");
            });
        }

        public async Task UpdateSektorByLoaderAsync(string sektor, string loader, string transportasi)
        {
            var sektorParam = new NpgsqlParameter("p_sektor", sektor);
            var loaderParam = new NpgsqlParameter("p_loader", loader);
            var transportasiParam = new NpgsqlParameter("p_transportasi", transportasi);

            await Database.ExecuteSqlRawAsync(
                "CALL update_sektor_by_loader(@p_sektor, @p_loader, @p_transportasi)",
                sektorParam,
                loaderParam,
                transportasiParam);
        }

    }
}
