using Embarkasi.Data;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using Embarkasi.Controllers;

namespace Embarkasi.Controllers
{
    public class Display3Controller : Controller
    {
        private readonly AppDBContext _context;
        private readonly ILogger<Display3Controller> _logger;
        private string controller_name = "Display3";
        private string title_name = "Display3";



        public Display3Controller(AppDBContext context, ILogger<Display3Controller> logger)
        {
            _context = context;
            _logger = logger;
        }
        public IActionResult Index()
        {
            var resultSmartDLastUpdate = _context.tbl_t_setting_fleet.OrderByDescending(x => x.updated_at).FirstOrDefault().updated_at;
            ViewBag.smartd_last_update = resultSmartDLastUpdate;
            return View();
        }


        [HttpGet("proxy")]
        public IActionResult Proxy(string url)
        {
            var client = new HttpClient();
            var result = client.GetAsync(url).Result;
            var content = result.Content.ReadAsStringAsync().Result;
            return Content(content, result.Content.Headers.ContentType.ToString());
        }

        [HttpGet]
        public ActionResult GetLoader(string sektor)
        {
            try
            {
                var resultLoader = _context.vw_t_setting_fleet
                    .Where(x => (x.cn_unit.StartsWith("EX40") || x.cn_unit.StartsWith("EX20") || x.cn_unit.StartsWith("EX30")) && x.sektor == sektor)
                    .Distinct()
                    .OrderBy(x => x.cn_unit)
                    .ToList();

                var baseUrl = string.Format("{0}://{1}{2}", Request.Scheme, Request.Host, Url.Content("~"));
                var lineLoader = new StringBuilder();

                for (int i = 0; i < resultLoader.Count; i++)
                {
                    lineLoader.Append("<div style='width: 130px; height: 80px; margin-left: 1px; margin-right: 2px; background-color: #000000; margin-bottom: 10px;'>");
                    lineLoader.Append("<center>");
                    lineLoader.Append("<div class='px-1 py-1 font-weight-bold text-white' style='background-color:#000000; font-size: 25px; margin: 1px;'>" + resultLoader[i].transportasi + "</div>");
                    lineLoader.Append("<div>");
                    lineLoader.Append("<div class='card text-black shadow' style='margin: 0; padding: 0; background-color: #D3D3D3;'>");


                    if (resultLoader[i].loader_operator_nama != "-" && !string.IsNullOrEmpty(resultLoader[i].loader_operator_nama))
                    {
                        // Conditional color based on status_absen
                        string statusColor = resultLoader[i].status_absen == "absen" ? "#00FF00" : "#FF0000"; // Green for 'absen', Red for others
                        lineLoader.Append($"<div style='position: absolute; top: 2px; left: 3px; width: 15px; height: 15px; background-color: {statusColor}; border-radius: 50%; animation: blink 1s infinite; box-shadow: 0 0 10px {statusColor};'></div>\r\n");
                    }

                    lineLoader.Append($"<div class='px-1 py-1 btn btn-lg' id='loader' style='font-size: 25px; font-weight: bold; color: #FFFFFF; background-color: #00008B;'>{resultLoader[i].loader}</div>");
                    lineLoader.Append("<div class='px-1 py-1 btn-lg' style='font-size: 13px; font-weight: bold; color:#F0FFFF;background-color:" + resultLoader[i].status_warna + ";'>" + resultLoader[i].loader_operator_nama + "</div>");

                    if (resultLoader[i].data_source == "goodeva")
                    {
                        lineLoader.Append("<div style='position: absolute; top: 56px; right: 0px; width: 20px; height: 20px; background-color: #228B22; color: #FFFFFF; font-weight: bold; text-align: center;'>G</div>");
                    }
                    else if (resultLoader[i].data_source == "savera")
                    {
                        lineLoader.Append("<div style='position: absolute; top: 56px; right: 0px; width: 20px; height: 20px; background-color: #2471a3; color: #FFFFFF; font-weight: bold; text-align: center;'>S</div>");
                    }

                    lineLoader.Append("</div>");
                    lineLoader.Append("</div>");
                    lineLoader.Append("</center>");
                    lineLoader.Append("</div>");
                }

                return Json(new { status = true, data = lineLoader.ToString() });
            }
            catch (Exception ex)
            {
                return Json(new { status = false, remarks = "Gagal", data = ex.Message.ToString() });
            }
        }


        [HttpGet]
        public ActionResult GetHauler(string sektor)
        {
            try
            {
                // Filtering 'resultLoader' using consistent style with SC
                var resultLoader = _context.vw_t_setting_fleet
                    .Where(x => (x.cn_unit.StartsWith("EX40") || x.cn_unit.StartsWith("EX20") || x.cn_unit.StartsWith("EX30")) && x.sektor == sektor)
                    .Distinct()
                    .OrderBy(x => x.cn_unit)
                    .ToList();

                var lineHauler = new StringBuilder();
                var baseUrl = $"{Request.Scheme}://{Request.Host}{Url.Content("~")}";

                foreach (var loader in resultLoader)
                {
                    // Add loader div with consistent styling
                    lineHauler.Append("<div style='width: 130px; height: 100px; margin-left: 1px; margin-right: 2px; margin-bottom: 30px; margin-top: 10px;'>");
                    lineHauler.Append("<div style='margin-bottom: 20px;'></div>");

                    // Filtering 'resultHauler' using consistent style with SC
                    var resultHauler = _context.vw_t_setting_fleet
                        .Where(x => x.loader == loader.loader && (x.cn_unit.Contains("LD") || x.cn_unit.Contains("DZ") || x.eq_class == "GD"))
                        .Distinct()
                        .OrderBy(x => x.hauler)
                        .ToList();

                    foreach (var hauler in resultHauler)
                    {
                        lineHauler.Append("<center style='margin-bottom: 15px;'>");
                        lineHauler.Append("<div style='margin: 0; padding: 0; position: relative;'>");
                        lineHauler.Append("<div class='card text-black shadow' style='margin: 0; padding: 0; background-color: #D3D3D3;'>");

                        if (hauler.hauler_operator_nama != "-" && !string.IsNullOrEmpty(hauler.hauler_operator_nama))
                        {
                            // Conditional color based on status_absen
                            string statusColor = hauler.status_absen == "absen" ? "#00FF00" : "#FF0000"; // Green for 'absen', Red for others
                            lineHauler.Append($"<div style='position: absolute; top: 2px; left: 3px; width: 15px; height: 15px; background-color: {statusColor}; border-radius: 50%; animation: blink 1s infinite; box-shadow: 0 0 10px {statusColor};'></div>\r\n");
                        }
                        // Card content for hauler
                        lineHauler.Append($"<div class='px-1 py-1 btn btn-lg' id='loader' style='font-size: 25px; font-weight: bold; color: #FFFFFF; background-color: #00008B;'>{hauler.hauler}</div>");
                        lineHauler.Append($"<div class='px-1 py-1 btn-lg' style='font-size: 13px; font-weight: bold; color:#F0FFFF;background-color:{hauler.status_warna};'>{hauler.hauler_operator_nama}</div>");

                        // Data source indicator
                        if (hauler.data_source == "goodeva")
                        {
                            lineHauler.Append("<div style='position: absolute; top: 52px; right: 0px; width: 20px; height: 20px; background-color: #228B22; color: #FFFFFF; font-weight: bold; text-align: center;'>G</div>");
                        }
                        else if (hauler.data_source == "savera")
                        {
                            lineHauler.Append("<div style='position: absolute; top: 52px; right: 0px; width: 20px; height: 20px; background-color: #2471a3 ; color: #FFFFFF; font-weight: bold; text-align: center;'>S</div>");
                        }

                        lineHauler.Append("</div>");
                        lineHauler.Append("</div>");
                        lineHauler.Append("</center>");
                    }

                    lineHauler.Append("</div>");
                }

                return Json(new { status = true, data = lineHauler.ToString() });
            }
            catch (Exception ex)
            {
                return Json(new { status = false, remarks = "Gagal", data = ex.Message.ToString() });
            }
        }


        public ActionResult GetNoSet()
        {
            try
            {
                // Ambil data unit dengan loader null atau belum disetel
                var resultLoader = _context.vw_t_setting_fleet
                    .Where(x =>
                        (x.loader == null || x.id == null || x.loader == "unit_no_setting") &&
                        (x.eq_class == "LD")
                    )
                    .Distinct()
                    .OrderBy(x => x.cn_unit)
                    .ToList();

                // Gunakan StringBuilder untuk membangun HTML
                var lineLoader = new StringBuilder();

                foreach (var loader in resultLoader)
                {
                    lineLoader.Append("<div style='width: 130px; height: 80px; margin-left: 1px; margin-right: 2px; background-color: #000000; margin-bottom: 10px;'>");

                    lineLoader.Append("<center>");
                    lineLoader.Append("<div class='px-1 py-1 font-weight-bold text-white' style='background-color:#000000; font-size: 15px; margin: 1px;'></div>");
                    lineLoader.Append("<div>");

                    lineLoader.Append("<div class='card bg-light text-black shadow'>");

                    // Define the status color
                    string statusColor = loader.status_absen == "absen" ? "#00FF00" : "#FF0000";

                    // Check if 'hauler_operator_nama' is not "-"
                    if (loader.hauler_operator_nama != "-")
                    {
                        // Append the status dot
                        lineLoader.Append($"<div style='position: absolute; top: 2px; left: 3px; width: 15px; height: 15px; background-color: {statusColor}; border-radius: 50%; animation: blink 1s infinite; box-shadow: 0 0 10px {statusColor};'></div>");
                    }

                    // Tambahkan data hauler jika ada
                    lineLoader.Append($"<div class='px-1 py-1 btn btn-lg' id='loader' style='font-size: 25px; font-weight: bold; color: #FFFFFF; background-color: #00008B;'>{loader.hauler}</div>");

                    // Tambahkan nama operator jika ada
                    lineLoader.Append($"<div class='px-1 py-1 btn-lg' style='font-size: 13px; font-weight: bold; color:#F0FFFF; background-color:{loader.status_warna ?? "#FFFFFF"};'>{loader.hauler_operator_nama ?? "No Operator"}</div>");

                    // Tambahkan kotak G atau S berdasarkan data_source
                    if (loader.data_source == "goodeva")
                    {
                        lineLoader.Append("<div style='position: absolute; top: 56px; right: 0px; width: 20px; height: 20px; background-color: #228B22; color: #FFFFFF; font-weight: bold; text-align: center;'>G</div>");
                    }
                    else if (loader.data_source == "savera")
                    {
                        lineLoader.Append("<div style='position: absolute; top: 56px; right: 0px; width: 20px; height: 20px; background-color: #2471a3; color: #FFFFFF; font-weight: bold; text-align: center;'>S</div>");
                    }

                    lineLoader.Append("</div>"); // Tutup card
                    lineLoader.Append("</div>"); // Tutup wrapper card
                    lineLoader.Append("</center>");
                    lineLoader.Append("</div>"); // Tutup wrapper utama
                }

                return Json(new { status = true, data = lineLoader.ToString() });
            }
            catch (Exception ex)
            {
                return Json(new { status = false, remarks = "Gagal", data = ex.Message.ToString() });
            }
        }



    }
}
