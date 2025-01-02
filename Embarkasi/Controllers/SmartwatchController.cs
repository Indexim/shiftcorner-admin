using Embarkasi.Data;
using Embarkasi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Embarkasi.Controllers
{
    public class SmartwatchController : Controller
    {
        private readonly AppDBContext _context;
        private readonly ILogger<SmartwatchController> _logger;
        private string controller_name = "Smartwatch";
        private string title_name = "Smartwatch";

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
        public SmartwatchController(AppDBContext context, ILogger<SmartwatchController> logger)
        {
            _context = context;
            _logger = logger;
        }
        public IActionResult GetAll()
        {
            try
            {
                var data = _context.tbl_m_user_smartwatch.OrderBy(x => x.nik).ToList();
                return Json(new { data = data });
            }
            catch (Exception ex)
            {
                var innerExceptionMessage = ex.InnerException?.Message ?? ex.Message;
                _logger.LogError(ex, "Error fetching data.");
                return Json(new { success = false, message = $"Terjadi kesalahan saat mengambil data ${ex.Message}." });
            }
        }

        public IActionResult Get(Guid id)
        {
            try
            {
                var result = _context.tbl_m_user_smartwatch.Where(x => x.id == id).FirstOrDefault();
                if (result == null)
                {
                    return Json(new { success = false, message = "Data tidak ditemukan." });
                }

                return Json(new { success = true, data = result });
            }
            catch (Exception ex)
            {
                var innerExceptionMessage = ex.InnerException?.Message ?? ex.Message;
                _logger.LogError(ex, "Terjadi kesalahan saat mengambil data karyawan.");
                return Json(new { success = false, message = $"Terjadi kesalahan saat mengambil data: {ex.Message}" });
            }
        }

        [Authorize]
        [HttpPost]
        public IActionResult Insert(tbl_m_user_smartwatch a)
        {
            try
            {

                var createdBy = HttpContext.Session.GetString("nik");
                if (string.IsNullOrEmpty(createdBy))
                {
                    return Json(new { success = false, message = "Pengguna tidak terautentikasi." });
                }


                a.createc_by = createdBy;
                // Menyimpan data ke database
                _context.tbl_m_user_smartwatch.Add(a);
                _context.SaveChanges();

                return Json(new { success = true, message = "Data berhasil disimpan." });
            }
            catch (Exception ex)
            {
                // Log error dan kembalikan pesan kesalahan dengan detail inner exception jika ada
                _logger.LogError(ex, "Terjadi kesalahan saat menambahkan data.");
                var innerExceptionMessage = ex.InnerException != null ? ex.InnerException.Message : "Tidak ada detail tambahan";
                return Json(new { success = false, message = $"Terjadi kesalahan saat menambahkan data: {ex.Message}. Detail: {innerExceptionMessage}" });
            }
        }


        [Authorize]

        [HttpPost]
        public ActionResult Update(tbl_m_user_smartwatch a)
        {
            try
            {
                var tbl_ = _context.tbl_m_user_smartwatch.FirstOrDefault(f => f.id == a.id);
                if (tbl_ != null)
                {
                    tbl_.id = a.id;
                    tbl_.nik = a.nik;
                    tbl_.smartwatch = a.smartwatch;
                    tbl_.status = a.status;
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
                var tbl_ = _context.tbl_m_user_smartwatch.FirstOrDefault(f => f.id == id);
                if (tbl_ != null)
                {
                    _context.tbl_m_user_smartwatch.Remove(tbl_);
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
