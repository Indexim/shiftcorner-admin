using Embarkasi.Data;
using Embarkasi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Embarkasi.Controllers
{
    public class LokasiExController : Controller
    {
        private readonly AppDBContext _context;
        private readonly ILogger<LokasiExController> _logger; 
        private readonly string controllerName = "LokasiEx";
        private readonly string titleName = "LokasiEx";

        public LokasiExController(AppDBContext context, ILogger<LokasiExController> logger) 
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

            var kategoriUserId = HttpContext.Session.GetString("kategori_user_id");

            var cekKategoriUserId = _context.tbl_r_menu
                .Where(x => x.link_controller == controllerName && x.kategori_user_id == kategoriUserId)
                .Count();

            if (cekKategoriUserId > 0)
            {
                ViewBag.Title = titleName;
                ViewBag.Controller = controllerName;
                ViewBag.Setting = _context.tbl_m_setting_aplikasi.FirstOrDefault();
                ViewBag.Menu = _context.tbl_r_menu
                    .Where(x => x.kategori_user_id == kategoriUserId)
                    .OrderBy(x => x.title)
                    .ToList();
                ViewBag.MenuMasterCount = _context.tbl_r_menu
                    .Where(x => x.kategori_user_id == kategoriUserId && x.type == "Master")
                    .Count();
                ViewBag.LineUpCount = _context.tbl_r_menu
                    .Where(x => x.kategori_user_id == kategoriUserId)
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

        [HttpGet]
        [Route("LokasiEx/Get/{loader}")]
        public IActionResult Get(string loader)  // Tangkap dari URL
        {
            try
            {
                var result = _context.vw_m_loader.Where(x => x.loader == loader).FirstOrDefault();

                if (result == null)
                {
                    return Json(new { success = false, message = "Data tidak ditemukan." });
                }

                return Json(new { success = true, data = result });
            }
            catch (Exception ex)
            {
                var innerExceptionMessage = ex.InnerException?.Message ?? ex.Message;
                _logger.LogError(ex, "Terjadi kesalahan saat mengambil data loader.");
                return Json(new { success = false, message = $"Terjadi kesalahan saat mengambil data: {ex.Message}" });
            }
        }


        public IActionResult GetAll() 
        {
            try
            {
                var results = _context.vw_m_loader.OrderBy(x => x.loader).ToList();
                return Json(new { status = true, remarks = "Sukses", data = results });
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error while getting all ASM Exes.");
                return Json(new { status = false, remarks = "Gagal", data = e.Message });
            }
        }


        [HttpPost]
        public async Task<IActionResult> Update(UpdateSektorByLoaderModel a) 
        {
            try
            {

                await _context.UpdateSektorByLoaderAsync(a.Sektor, a.Loader, a.Transportasi);

                return Json(new { status = true, remarks = "Sukses memperbarui data." });
            }
            catch (Exception e)
            {

                _logger.LogError(e, "Terjadi kesalahan saat memperbarui pengaturan.");
                return Json(new { status = false, remarks = "Gagal memperbarui data.", data = e.Message });
            }
        }



    }
}
