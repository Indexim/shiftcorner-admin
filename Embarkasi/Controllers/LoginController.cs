using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Embarkasi.Data;
using Embarkasi.Models;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.DirectoryServices.AccountManagement;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Embarkasi.Controllers
{
    public class LoginController : Controller
    {
        private readonly AppDBContext _context;
        private readonly ILogger<LoginController> _logger;
        private const string ControllerName = "Login";
        private const string TitleName = "Login";

        public LoginController(AppDBContext context, ILogger<LoginController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // Halaman Login
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("is_login") != null)
            {
                var kategoriUserId = HttpContext.Session.GetString("kategori_user_id");

                var cekKategoriUserId = _context.tbl_r_kategori_user
                    .FirstOrDefault(x => x.kategori == kategoriUserId);

                if (cekKategoriUserId != null)
                {
                    return RedirectToAction(cekKategoriUserId.login_function, cekKategoriUserId.login_controller);
                }
                else
                {
                    _logger.LogWarning("Category user ID not found: {KategoriUserId}", kategoriUserId);
                    return View();
                }
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ProsesLogin(string nik, string password)
        {
            try
            {
                bool isAuthenticated = false;
                string loginMethod = "AD";
                int adTimeoutInMilliseconds = 3000;

                try
                {
                    using (var adContext = new PrincipalContext(ContextType.Domain, "MC.COM", "ldap://172.16.1.200"))
                    {
                        isAuthenticated = adContext.ValidateCredentials(nik, password);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogWarning("Gagal menghubungkan ke AD: {Message}", ex.Message);
                    // Jika ada kesalahan pada koneksi AD, lanjutkan menggunakan database
                }

                tbl_m_user_login user = null;

                if (isAuthenticated)
                {
                    // Jika berhasil login melalui AD, cek pengguna di database
                    user = await _context.tbl_m_user_login
                        .SingleOrDefaultAsync(x => x.nik == nik);

                    if (user == null)
                    {
                        return Json(new { status = false, remarks = "Pengguna ditemukan di AD, tetapi tidak terdaftar di database." });
                    }
                }
                else
                {
                    // Jika gagal login melalui AD atau terjadi kesalahan, coba login menggunakan database
                    user = await _context.tbl_m_user_login
                        .SingleOrDefaultAsync(x => x.nik == nik && x.password == password);

                    if (user == null)
                    {
                        return Json(new { status = false, remarks = "Login gagal. Username/password salah." });
                    }
                    loginMethod = "Database"; // Ubah metode login menjadi Database
                }

                // Menyimpan data sesi
                HttpContext.Session.SetString("is_login", "true");
                HttpContext.Session.SetString("nik", user.nik);
                HttpContext.Session.SetString("nama", user.nama);

                var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.nik),
            new Claim("nama", user.nama),
            new Claim("kategori_user_id", user.kategori_user_id ?? ""),
            new Claim("login_method", loginMethod)
        };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30)
                };

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties
                );

                var kategoriUser = await _context.tbl_m_user_login
                    .Where(x => x.nik == user.nik)
                    .ToListAsync();

                return Json(new { status = true, remarks = "Login Sukses dengan metode: " + loginMethod, data = kategoriUser });
            }
            catch (Exception ex)
            {
                return Json(new { status = false, remarks = "Terjadi kesalahan", data = ex.Message });
            }
        }

        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Clear();
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", ControllerName);
        }

        [HttpGet]
        public ActionResult CekKategoriUser(string kategori_user_id)
        {
            try
            {
                var results = _context.tbl_r_kategori_user
                    .FirstOrDefault(x => x.kategori == kategori_user_id);

                if (results != null)
                {
                    HttpContext.Session.SetString("kategori_user_id", results.kategori);
                    return Json(new { status = true, remarks = "Sukses", data = results });
                }
                else
                {
                    return Json(new { status = false, remarks = "Kategori user tidak ditemukan" });
                }
            }
            catch (Exception e)
            {
                return Json(new { status = false, remarks = "Gagal", data = e.Message });
            }
        }
    }
}
