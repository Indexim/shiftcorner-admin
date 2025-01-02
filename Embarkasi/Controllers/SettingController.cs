using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using Embarkasi.Data;
using Embarkasi.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using Embarkasi.Controllers;

namespace Embarkasi.Controllers
{
    public class SettingController : Controller
    {
        private readonly AppDBContext _context;
        private readonly ILogger<SettingController> _logger;
        private string controller_name = "Setting";
        private string title_name = "Setting";

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
        public SettingController(AppDBContext context, ILogger<SettingController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IActionResult Get()
        {
            try
            {
                var result = _context.tbl_m_setting_aplikasi.FirstOrDefault();

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
        public ActionResult Update(tbl_m_setting_aplikasi a)
        {
            try
            {
                var tbl_ = _context.tbl_m_setting_aplikasi.FirstOrDefault(f => f.id == a.id);
                if (tbl_ != null)
                {
                    tbl_.nama = a.nama;
                    tbl_.description = a.description;
                    tbl_.icon = a.icon;
                    tbl_.theme = a.theme;
                    tbl_.background = a.background;
                    tbl_.border = a.border;
                    tbl_.updated_at = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                    _context.SaveChanges();

                    return Json(new { success = true, message = "Data berhasil diperbarui." });
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



    }
}