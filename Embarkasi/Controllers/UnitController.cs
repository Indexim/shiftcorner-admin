using Embarkasi.Data;
using Embarkasi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Embarkasi.Controllers;

namespace Embarkasi.Controllers
{
    public class UnitController : Controller
    {
        private readonly AppDBContext _context;
        private readonly ILogger<UnitController> _logger;
        private string controller_name = "Unit";
        private string title_name = "Unit";

        [Authorize]
        public ActionResult Index()
        {
            if (HttpContext.Session.GetString("is_login") == null)
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                var kategori_user_id = HttpContext.Session.GetString("kategori_user_id");

                var cek_kategori_user_id = _context.tbl_r_menu
                    .Where(x => x.link_controller == controller_name)
                    .Where(x => x.kategori_user_id == kategori_user_id)
                    .Count();

                if (cek_kategori_user_id > 0)
                {
                    ViewBag.Title = title_name;
                    ViewBag.Controller = controller_name;
                    ViewBag.Setting = _context.tbl_m_setting_aplikasi.FirstOrDefault();
                    ViewBag.Menu = _context.tbl_r_menu
                        .Where(x => x.kategori_user_id == kategori_user_id)
                        .OrderBy(x => x.title)
                        .ToList();
                    ViewBag.MenuMasterCount = _context.tbl_r_menu
                        .Where(x => x.kategori_user_id == kategori_user_id)
                        .Where(x => x.type == "Master")
                        .OrderBy(x => x.title)
                        .Count();
                    ViewBag.LineUpCount = _context.tbl_r_menu
                        .Where(x => x.kategori_user_id == kategori_user_id)
                        .Where(x => x.type == "Line Up")
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
        }
        public UnitController(AppDBContext context, ILogger<UnitController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IActionResult GetAll()
        {
            try
            {
                var data = _context.tbl_m_unit.OrderBy(x => x.created_at).ToList();
                return Json(new { success = true, data = data });
            }
            catch (Exception ex)
            {
                var innerExceptionMessage = ex.InnerException?.Message ?? ex.Message;
                _logger.LogError(ex, "Error fetching data.");
                return Json(new { success = false, message = $"Terjadi kesalahan saat mengambil data: {innerExceptionMessage}." });
            }
        }

        public IActionResult Get(string id)
        {
            try
            {
                _logger.LogInformation($"Received ID: {id}");

                if (!Guid.TryParse(id, out Guid guidId))
                {
                    _logger.LogWarning($"Invalid GUID format: {id}");
                    return Json(new { success = false, message = "ID tidak valid." });
                }

                _logger.LogInformation($"Parsed GUID: {guidId}");

                var result = _context.tbl_m_unit
                    .Where(x => x.id == guidId)
                    .FirstOrDefault();

                if (result == null)
                {
                    _logger.LogInformation($"Data not found for ID: {guidId}");
                    return Json(new { success = false, message = "Data tidak ditemukan." });
                }

                return Json(new { success = true, data = result });
            }
            catch (Exception ex)
            {
                var innerExceptionMessage = ex.InnerException?.Message ?? ex.Message;
                _logger.LogError(ex, "Terjadi kesalahan saat mengambil data.");
                return Json(new { success = false, message = $"Terjadi kesalahan saat mengambil data: {ex.Message}" });
            }
        }



        [Authorize]
        [HttpPost]
        public IActionResult Insert(tbl_m_unit a)
        {
            try
            {
                _context.tbl_m_unit.Add(a);
                _context.SaveChanges();
                return Json(new { success = true, message = "Data berhasil disimpan." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Terjadi kesalahan saat menambahkan data.");

                var innerExceptionMessage = ex.InnerException != null ? ex.InnerException.Message : "Tidak ada detail tambahan";
                return Json(new { success = false, message = $"Terjadi kesalahan saat menambahkan data: {ex.Message}. Detail: {innerExceptionMessage}" });
            }
        }


        [Authorize]

        [HttpPost]
        public ActionResult Update(tbl_m_unit a)
        {
            try
            {
                var tbl_ = _context.tbl_m_unit.FirstOrDefault(f => f.id == a.id);
                if (tbl_ != null)
                {
                    tbl_.cn_unit = a.cn_unit;
                    tbl_.eq_class = a.eq_class;
                    tbl_.egi = a.egi;
                    tbl_.is_active = a.is_active;
                    tbl_.is_standby = a.is_standby;
                    tbl_.is_breakdown = a.is_breakdown;
                    tbl_.product = a.product;
                    //tbl_.updated_at = DateTime.Now;
                    _context.SaveChanges();
                    return Json(new { success = true, message = "Data berhasil diubah." });
                }
                else
                {
                    return Json(new { success = false, message = "Data tidak ditemukan." });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Terjadi kesalahan saat update data.");
                return Json(new { success = false, message = $"Terjadi kesalahan saat update data: {ex.Message}" });
            }
        }

        [Authorize]
        [HttpPost]
        public IActionResult Delete(Guid id)
        {
            try
            {
                var tbl_ = _context.tbl_m_unit.FirstOrDefault(f => id == id);
                if (tbl_ != null)
                {
                    _context.tbl_m_unit.Remove(tbl_);
                    _context.SaveChanges();
                    return Json(new { success = true, message = "Data berhasil dihapus." });
                }
                else
                {
                    return Json(new { success = false, message = "Data tidak ditemukan." });
                }
            }
            catch (Exception ex)
            {
                var innerExceptionMessage = ex.InnerException?.Message ?? ex.Message;
                _logger.LogError(ex, "Terjadi kesalahan saat menghapus data.");
                return Json(new { success = false, message = $"Terjadi kesalahan saat menghapus data: {ex.Message}" });
            }
        }

    }
}
