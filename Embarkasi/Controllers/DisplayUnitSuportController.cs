using Embarkasi.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace Embarkasi.Controllers
{
    public class DisplayUnitSuportController : Controller
    {
        private readonly AppDBContext _context;
        private readonly ILogger<DisplayUnitSuportController> _logger;
        private string controller_name = "DisplayUnitSuport";
        private string title_name = "DisplayUnitSuport";



        public DisplayUnitSuportController(AppDBContext context, ILogger<DisplayUnitSuportController> logger)
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


        [HttpGet]
        public ActionResult GetHauler(string sektor)
        {
            try
            {
                var resultLoader = _context.vw_t_setting_fleet
                    .Where(x => (x.eq_class == "EX" || x.eq_class == "SH" || x.eq_class == "PM") && x.sektor == sektor)
                    .OrderBy(x => x.cn_unit)
                    .ToList();

                var loaderUnits = resultLoader.Select(l => l.cn_unit).ToList();
                var resultHauler = _context.vw_t_setting_fleet
                    .Where(x => loaderUnits.Contains(x.loader) && (x.cn_unit.Contains("RD") || x.cn_unit.Contains("DT") || x.eq_class == "GR"))
                    .OrderBy(x => x.hauler)
                    .ToList();

                var lineHauler = new StringBuilder();
                foreach (var loader in resultLoader)
                {
                    lineHauler.Append($@"
                <div style='margin-left: 0px; margin-right: 0px;'>");

                    var filteredHaulers = resultHauler.Where(h => h.loader == loader.cn_unit).ToList();
                    foreach (var hauler in filteredHaulers)
                    {
                        // Determine the class based on hauler_ftw_status
                        string statusClass;
                        switch (hauler.hauler_ftw_status)
                        {
                            case "-":
                                statusClass = "loader-subtext-grey";
                                break;
                            case "Cukup Tidur":
                                statusClass = "loader-subtext-green"; // Corrected from 'grenn' to 'green'
                                break;
                            case "Dalam Pengawasan":
                                statusClass = "loader-subtext-yellow";
                                break;
                            case "Kurang Tidur":
                                statusClass = "loader-subtext-red";
                                break;
                            default:
                                statusClass = "loader-subtext-default"; // Fallback class if needed
                                break;
                        }

                        lineHauler.Append($@"
                    <div class='area-card-container' style='margin-left: 2px; margin-top: 30px;'>
                        <center>
                            <div style='display: inline-block; perspective: 1200px;'>
                                <div class='card bg-light text-black area-card'>
                                    <div class='px-1 py-1 btn btn-lg btn-secondary loader-card' id='loader-card'>
                                        {hauler.hauler}
                                    </div>
                                    <div class='btn-lg {statusClass}'>
                                        {hauler.hauler_operator_nama}
                                    </div>
                                </div>
                            </div>
                        </center>
                    </div>");
                    }

                    lineHauler.Append("</div>");
                }

                return Json(new { status = true, data = lineHauler.ToString() });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetHauler");
                return Json(new { status = false, remarks = "Gagal", data = "Terjadi kesalahan, silakan coba lagi." });
            }
        }



    }
}
