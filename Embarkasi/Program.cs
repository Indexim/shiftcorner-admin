using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Embarkasi.Data;

var builder = WebApplication.CreateBuilder(args);

// Menambahkan layanan ke container DI.
builder.Services.AddControllersWithViews();

// Registrasi DbContext
builder.Services.AddDbContext<AppDBContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("WebApiDatabase")));

// Tambahkan layanan sesi
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Konfigurasi otentikasi
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Login/Index"; // Ganti dengan path login Anda
        options.LogoutPath = "/Login/Logout"; // Ganti dengan path logout Anda
        options.AccessDeniedPath = "/Home/AccessDenied"; // Ganti dengan path akses ditolak Anda
    });

var app = builder.Build();

// Konfigurasi agar aplikasi mendengarkan di semua alamat
app.Urls.Add("http://172.16.1.95:5003/");

//app.Urls.Add("http://localhost:5003/");

// Konfigurasi middleware HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// Hanya gunakan HTTPS redirection jika HTTPS diaktifkan
// app.UseHttpsRedirection(); // Hapus jika hanya menggunakan HTTP

app.UseStaticFiles();

app.UseRouting();

// Tambahkan middleware sesi sebelum middleware otorisasi
app.UseSession();

// Tambahkan middleware otentikasi sebelum middleware otorisasi
app.UseAuthentication();
app.UseAuthorization();

// Tambahkan pengaturan CORS jika diperlukan (jika diakses dari domain yang berbeda)
app.UseCors(builder =>
    builder.AllowAnyOrigin()
           .AllowAnyMethod()
           .AllowAnyHeader()
);

// Tambahkan header keamanan tambahan
app.Use(async (context, next) =>
{
    context.Response.Headers.Add("X-Frame-Options", "DENY");
    context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
    context.Response.Headers.Add("Referrer-Policy", "no-referrer");
    await next();
});

// Konfigurasi rute aplikasi
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Dept}/{action=Index}/{id?}");

// Menjalankan aplikasi
app.Run();
