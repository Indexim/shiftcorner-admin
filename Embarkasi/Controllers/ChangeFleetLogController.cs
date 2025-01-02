using Embarkasi.Data;
using Embarkasi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Embarkasi.Controllers
{
    [Authorize]
    public class ChangeFleetLogController : Controller
    {
        private readonly AppDBContext _context;
        private readonly ILogger<ChangeFleetLogController> _logger;
        private readonly string controller_name = "ChangeFleetLog";
        private readonly string title_name = "CHANGE FLEET LOG";

        public ChangeFleetLogController(AppDBContext context, ILogger<ChangeFleetLogController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("is_login") == null)
            {
                return RedirectToAction("Index", "Login");
            }

            var kategori_user_id = HttpContext.Session.GetString("kategori_user_id");

            var cek_kategori_user_id = _context.tbl_r_menu
                .AsNoTracking()
                .Count(x => x.link_controller == controller_name && x.kategori_user_id == kategori_user_id);

            if (cek_kategori_user_id > 0)
            {
                ViewBag.Title = title_name;
                ViewBag.Controller = controller_name;
                ViewBag.Setting = _context.tbl_m_setting_aplikasi.AsNoTracking().FirstOrDefault();
                ViewBag.Menu = _context.tbl_r_menu
                    .Where(x => x.kategori_user_id == kategori_user_id)
                    .OrderBy(x => x.title)
                    .AsNoTracking()
                    .ToList();
                ViewBag.MenuMasterCount = _context.tbl_r_menu
                    .AsNoTracking()
                    .Count(x => x.kategori_user_id == kategori_user_id && x.type == "Master");
                ViewBag.LineUpCount = _context.tbl_r_menu
                    .Where(x => x.kategori_user_id == kategori_user_id && x.type == "Line Up")
                    .AsNoTracking()
                    .OrderBy(x => x.title)
                    .Count();
                ViewBag.insert_by = HttpContext.Session.GetString("nik");
                ViewBag.departemen = HttpContext.Session.GetString("dept_code");
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }

        public IActionResult GetAll()
        {
            try
            {
                var results = _context.tbl_t_change_fleet_logs
                    .AsNoTracking()
                    .OrderBy(x => x.created_at)
                    .ToList();
                return Json(new { status = true, remarks = "Sukses", data = results });
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error while getting all fleet logs.");
                return Json(new { status = false, remarks = "Gagal", data = e.Message });
            }
        }

        public IActionResult Get(Guid pid)
        {
            try
            {
                var result = _context.tbl_t_change_fleet_logs.AsNoTracking().FirstOrDefault(x => x.pid == pid);
                if (result == null)
                {
                    return Json(new { status = false, remarks = "Data tidak ditemukan" });
                }
                return Json(new { status = true, remarks = "Sukses", data = result });
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error while getting fleet log.");
                return Json(new { status = false, remarks = "Gagal", data = e.Message });
            }
        }

        [HttpPost]
        public IActionResult Insert(string sektor, string unit, string fleet_from, string fleet_to)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                var tbl_ = _context.tbl_t_setting_fleet.FirstOrDefault(f => f.hauler == unit);
                if (tbl_ != null)
                {
                    tbl_.sektor = sektor;
                    tbl_.loader = fleet_to;
                    tbl_.sektor_updated_at = DateTime.UtcNow;
                    tbl_.disposal_updated_by = HttpContext.Session.GetString("nik");
                }

                InsertLog(unit, fleet_from, fleet_to);
                UpdateUnitStatus(unit, fleet_from, fleet_to);

                _context.SaveChanges();
                transaction.Commit();

                return Json(new { status = true, remarks = "Sukses" });
            }
            catch (Exception e)
            {
                transaction.Rollback();
                var errorMessage = e.Message;
                var innerExceptionMessage = e.InnerException?.Message ?? "No inner exception.";
                _logger.LogError(e, "Error while inserting fleet log. Error: {ErrorMessage}, Inner Exception: {InnerExceptionMessage}", errorMessage, innerExceptionMessage);
                return Json(new { status = false, remarks = "Gagal", data = errorMessage, innerException = innerExceptionMessage });
            }
        }

        private void InsertLog(string unit, string fleet_from, string fleet_to)
        {
            try
            {
                var a = new tbl_t_change_fleet_log
                {
                    pid = Guid.NewGuid(),
                    unit = unit,
                    fleet_from = fleet_from,
                    fleet_to = fleet_to,
                    created_by = HttpContext.Session.GetString("nik"),
                    created_at = DateTime.UtcNow
                };

                _context.tbl_t_change_fleet_logs.Add(a);
            }
            catch (Exception ex)
            {
                var innerExceptionMessage = ex.InnerException?.Message ?? ex.Message;
                _logger.LogError(ex, $"Error while inserting fleet log: {innerExceptionMessage}");
                throw;
            }
        }

        [HttpPost]
        public IActionResult Update(tbl_t_change_fleet_log a)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                var tbl_ = _context.tbl_t_change_fleet_logs.FirstOrDefault(f => f.pid == a.pid);
                if (tbl_ != null)
                {
                    tbl_.unit = a.unit;
                    tbl_.fleet_from = a.fleet_from;
                    tbl_.fleet_to = a.fleet_to;
                    _context.SaveChanges();
                }
                transaction.Commit();
                return Json(new { status = true, remarks = "Sukses" });
            }
            catch (Exception e)
            {
                transaction.Rollback();
                _logger.LogError(e, "Error while updating fleet log.");
                return Json(new { status = false, remarks = "Gagal", data = e.Message });
            }
        }

        [HttpPost]
        public IActionResult Delete(Guid pid)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                var tbl_ = _context.tbl_t_change_fleet_logs.FirstOrDefault(f => f.pid == pid);
                if (tbl_ != null)
                {
                    _context.tbl_t_change_fleet_logs.Remove(tbl_);
                    _context.SaveChanges();
                }
                transaction.Commit();
                return Json(new { status = true, remarks = "Sukses" });
            }
            catch (Exception e)
            {
                transaction.Rollback();
                _logger.LogError(e, "Error while deleting fleet log.");
                return Json(new { status = false, remarks = "Gagal", data = e.Message });
            }
        }

        private void UpdateUnitStatus(string unit, string fleet_from, string fleet_to)
        {
            var tbl_unit = _context.tbl_m_unit.FirstOrDefault(f => f.cn_unit == unit);
            if (tbl_unit == null) return;

            tbl_unit.is_breakdown = fleet_to == "unit_breakdown";
            tbl_unit.is_standby = fleet_to == "unit_standby";
            tbl_unit.updated_at = DateTime.UtcNow;
            tbl_unit.updated_by = HttpContext.Session.GetString("nik");
        }
    }
}
