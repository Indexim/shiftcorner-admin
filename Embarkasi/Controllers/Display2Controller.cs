using Embarkasi.Data;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using Embarkasi.Controllers;
using System.Linq;

namespace Embarkasi.Controllers
{
    public class Display2Controller : Controller
    {
        private readonly AppDBContext _context;
        private readonly ILogger<Display2Controller> _logger;
        private string controller_name = "Display2";
        private string title_name = "Display2";



        public Display2Controller(AppDBContext context, ILogger<Display2Controller> logger)
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

        public ActionResult GetGrader()
        {
            try
            {
                // Ambil data grader
                var resultLoader = _context.vw_t_setting_fleet
                    .Where(x => x.cn_unit != null && x.eq_class.StartsWith("GD"))
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

                    lineLoader.Append($"<div class='px-1 py-1 btn btn-lg' id='loader' style='font-size: 25px; font-weight: bold; color: #FFFFFF; background-color: #00008B;'>{loader.hauler}</div>");

                    // Tambahkan nama operator
                    lineLoader.Append($"<div class='px-1 py-1 btn-lg' style='font-size: 12px; font-weight: bold; color:#F0FFFF; background-color:{loader.status_warna ?? "#FFFFFF"};'>{loader.hauler_operator_nama ?? "No Operator"}</div>");

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

                // Return JSON response
                return Json(new { status = true, data = lineLoader.ToString() });
            }
            catch (Exception ex)
            {
                return Json(new { status = false, remarks = "Gagal", data = ex.Message.ToString() });
            }
        }


        public ActionResult GetDozer()
        {
            try
            {

                var resultLoader = _context.vw_t_setting_fleet
                    .Where(x => x.cn_unit != null && !x.cn_unit.Contains("HD") && x.cn_unit.StartsWith("DZ"))
                    .OrderBy(x => x.cn_unit)
                    .ToList();
                var lineLoader = new StringBuilder();

                foreach (var loader in resultLoader)
                {
                    lineLoader.Append("<div style='width: 130px; height: 80px; margin-left: 1px; margin-right: 2px; background-color: #000000; margin-bottom: 10px; position: relative;'>");
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


                    lineLoader.Append($"<div class='px-1 py-1 btn btn-lg' id='loader' style='font-size: 25px; font-weight: bold; color: #FFFFFF; background-color: #00008B;'>{loader.hauler}</div>");


                    lineLoader.Append($"<div class='px-1 py-1 btn-lg' style='font-size: 12px; font-weight: bold; color:#F0FFFF; background-color:{loader.status_warna ?? "#FFFFFF"};'>{loader.hauler_operator_nama ?? "No Operator"}</div>");

                    if (loader.data_source == "goodeva")
                    {
                        lineLoader.Append("<div style='position: absolute; top: 56px; right: 0px; width: 20px; height: 20px; background-color: #228B22; color: #FFFFFF; font-weight: bold; text-align: center;'>G</div>");
                    }
                    else if (loader.data_source == "savera")
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

        public ActionResult GetExSuport()
        {
            try
            {

                var resultLoader = _context.vw_t_setting_fleet
                    .Where(x => x.cn_unit != null && !x.cn_unit.Contains("HD") &&
                                (x.cn_unit.StartsWith("EX40") || x.cn_unit.StartsWith("EX21") || x.cn_unit.StartsWith("EX20") || x.cn_unit.StartsWith("EX30")))
                    .OrderBy(x => x.cn_unit)
                    .ToList();


                var lineLoader = new StringBuilder();

                foreach (var loader in resultLoader)
                {
                    lineLoader.Append("<div style='width: 130px; height: 80px; margin-left: 1px; margin-right: 2px; background-color: #000000; margin-bottom: 10px; position: relative;'>");
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


                    lineLoader.Append($"<div class='px-1 py-1 btn btn-lg' id='loader' style='font-size: 25px; font-weight: bold; color: #FFFFFF; background-color: #00008B;'>{loader.loader}</div>");


                    lineLoader.Append($"<div class='px-1 py-1 btn-lg' style='font-size: 12px; font-weight: bold; color:#F0FFFF; background-color:{loader.status_warna ?? "#FFFFFF"};'>{loader.hauler_operator_nama ?? "No Operator"}</div>");

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

                // Return JSON response
                return Json(new { status = true, data = lineLoader.ToString() });
            }
            catch (Exception ex)
            {
                // Tangani kesalahan dan return pesan error
                return Json(new { status = false, remarks = "Gagal", data = ex.Message.ToString() });
            }
        }
        public ActionResult GetDtSuport()
        {
            try
            {

                var resultLoader = _context.vw_t_setting_fleet
                    .Where(x => x.cn_unit != null && !x.cn_unit.Contains("HD") &&
                                (x.cn_unit.StartsWith("DT")))
                    .OrderBy(x => x.cn_unit)
                    .ToList();


                var lineLoader = new StringBuilder();

                foreach (var loader in resultLoader)
                {
                    lineLoader.Append("<div style='width: 130px; height: 80px; margin-left: 1px; margin-right: 2px; background-color: #000000; margin-bottom: 10px; position: relative;'>");
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


                    lineLoader.Append($"<div class='px-1 py-1 btn btn-lg' id='loader' style='font-size: 25px; font-weight: bold; color: #FFFFFF; background-color: #00008B;'>{loader.hauler}</div>");


                    lineLoader.Append($"<div class='px-1 py-1 btn-lg' style='font-size: 12px; font-weight: bold; color:#F0FFFF; background-color:{loader.status_warna ?? "#FFFFFF"};'>{loader.hauler_operator_nama ?? "No Operator"}</div>");

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

                // Return JSON response
                return Json(new { status = true, data = lineLoader.ToString() });
            }
            catch (Exception ex)
            {
                // Tangani kesalahan dan return pesan error
                return Json(new { status = false, remarks = "Gagal", data = ex.Message.ToString() });
            }
        }

    }
}
