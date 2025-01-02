using Embarkasi.Data;
using Humanizer.Localisation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Text;

namespace Embarkasi.Controllers
{
    public class SettingFleetController : Controller
    {
        private readonly AppDBContext _context;
        private readonly ILogger<SettingFleetController> _logger;
        private const string ControllerName = "SettingFleet";
        private const string TitleName = "SettingFleet";

        public SettingFleetController(AppDBContext context, ILogger<SettingFleetController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [Authorize]
        public ActionResult Index()
        {
            if (HttpContext.Session.GetString("is_login") == null)
                return RedirectToAction("Index", "Login");

            var kategori_user_id = HttpContext.Session.GetString("kategori_user_id");
            if (!_context.tbl_r_menu.Any(x => x.link_controller == ControllerName && x.kategori_user_id == kategori_user_id))
                return RedirectToAction("Index", "Login");

            ViewBag.Title = TitleName;
            ViewBag.Controller = ControllerName;
            ViewBag.Setting = _context.tbl_m_setting_aplikasi.FirstOrDefault();
            ViewBag.Menu = _context.tbl_r_menu
                .Where(x => x.kategori_user_id == kategori_user_id)
                .OrderBy(x => x.title)
                .ToList();
            ViewBag.MenuMasterCount = _context.tbl_r_menu.Count(x => x.kategori_user_id == kategori_user_id && x.type == "Master");
            ViewBag.LineUpCount = _context.tbl_r_menu.Count(x => x.kategori_user_id == kategori_user_id && x.type == "Line Up");
            ViewBag.insert_by = HttpContext.Session.GetString("nik");
            ViewBag.departemen = HttpContext.Session.GetString("dept_code");

            return View();
        }

        [HttpGet]
        public ActionResult GetLoader(string sektor)
        {
            try
            {
                var resultLoader = _context.vw_t_setting_fleet
                    .Where(x =>
                        (x.eq_class == "EX" || x.eq_class == "SH" || x.eq_class == "PM") &&
                        x.sektor == sektor &&
                        !(x.cn_unit.StartsWith("EX40") || x.cn_unit.StartsWith("EX20") || x.cn_unit.StartsWith("EX30"))
                    )
                    .Distinct()
                    .OrderBy(x => x.cn_unit)
                    .ToList();

                var lineLoader = new StringBuilder();
                foreach (var loader in resultLoader)
                {
                    lineLoader.Append($@"
                <div class='loader' style='width: 200px; margin: 0 10px;'>
                    <div class='card bg-light text-black shadow-sm' style='border-radius: 10px;'>
                        <div class='btn btn-lg btn-secondary' onclick='btnEdit(""{loader.cn_unit}"")' value='{loader.cn_unit}' id='cn_unit'>
                            <div class='row'>
                                <div class='col-4 d-flex align-items-center'>
                                    <img src='/img/excavator.png' alt='{loader.cn_unit}' style='width: 100%; height: auto; border-radius: 5px;' />
                                </div>
                                <div class='col-8 d-flex align-items-center'>
                                    <b style='font-size: 16pt;'>{loader.cn_unit}</b>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>");
                }

                return Json(new { status = true, data = lineLoader.ToString() });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetLoader");
                return Json(new { status = false, remarks = "Gagal", data = ex.Message });
            }
        }

        [HttpGet]
        public ActionResult GetHauler(string sektor)
        {
            try
            {
                var resultLoader = _context.vw_t_setting_fleet
                    .Where(x =>
                        (x.eq_class == "EX" || x.eq_class == "SH" || x.eq_class == "PM") &&
                        x.sektor == sektor &&
                        !(x.cn_unit.StartsWith("EX40") || x.cn_unit.StartsWith("EX20") || x.cn_unit.StartsWith("EX30"))
                    )
                    .Distinct()
                    .OrderBy(x => x.cn_unit)
                    .ToList();


                var lineHauler = new StringBuilder();
                foreach (var loader in resultLoader)
                {
                    lineHauler.Append($@"
                <div id='{loader.loader}' class='hauler' style='width: 200px; margin: 0 10px; padding-bottom:100px;' ondragover='onDragOver(event);' ondrop='onDrop(event);'>");

                    var resultHauler = _context.vw_t_setting_fleet
                        .Where(x => x.loader == loader.loader && (x.cn_unit.Contains("RD") || x.cn_unit.Contains("DZ") || x.eq_class == "GD"))
                          .Distinct()
                        .OrderBy(x => x.hauler)
                        .ToList();

                    foreach (var hauler in resultHauler)
                    {
                        string imgSrc = hauler.eq_class switch
                        {
                            "DZ" => "/img/dozer.png",
                            "GD" => "/img/grader.png",
                            _ => "/img/truck.png"
                        };

                        lineHauler.Append($@"
                    <div id='{hauler.hauler}' class='hauler-item' style='margin-bottom: 5px;' draggable='true' ondragstart='onDragStart(event);'>
                        <div class='card' style='background-color: #f8f9fa; border: 3px solid {ViewBag.Setting?.border ?? "#000"}; border-radius: 10px; box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);'>
                            <div class='card-body'>
                                <div class='row'>
                                    <div class='col-4 d-flex align-items-center'>
                                        <img src='{imgSrc}' alt='{hauler.cn_unit}' style='width: 100%; height: auto; border-radius: 5px;' />
                                    </div>
                                    <div class='col-8 d-flex align-items-center'>
                                        <b style='font-size: 16pt;'>{hauler.cn_unit}</b>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>");
                    }

                    lineHauler.Append("</div>");
                }

                return Json(new { status = true, data = lineHauler.ToString() });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetHauler");
                return Json(new { status = false, remarks = "Gagal", data = ex.Message });
            }
        }

        [HttpGet]
        public ActionResult GetHaulerBreakdown()
        {
            try
            {
                var resultUnit = _context.vw_t_setting_fleet
                    .Where(x =>
                                            ( x.loader == "unit_breakdown") &&
                                (x.cn_unit.Contains("RD") || x.cn_unit.Contains("DZ") || x.eq_class == "GD"))
                    .Distinct()
                    .OrderBy(x => x.eq_class == "DZ" || x.eq_class == "GD")
                    .ThenBy(x => x.cn_unit)
                    .ToList();

                var lineUnit = new StringBuilder();
                foreach (var unit in resultUnit)
                {
                    string imgSrc = unit.eq_class switch
                    {
                        "DZ" => "/img/dozer.png",
                        "GD" => "/img/grader.png",
                        _ => "/img/truck.png"
                    };

                    lineUnit.Append($@"
<div id='{unit.cn_unit}' class='hauler' style='margin-bottom: 5px;' draggable='true' ondragstart='onDragStart(event);'>
    <div class='card bg-light text-black shadow-sm' style='border: 3px solid {ViewBag.Setting?.border ?? "#ff2d00"}; border-radius: 10px;'>
        <div class='card-body'>
            <div class='row'>
                <div class='col-4 d-flex align-items-center'>
                    <img src='{imgSrc}' alt='{unit.cn_unit}' style='width: 100%; height: auto; border-radius: 5px;' />
                </div>
                <div class='col-8 d-flex align-items-center'>
                    <b style='font-size: 16pt;'>{unit.cn_unit}</b>
                </div>
            </div>
        </div>
    </div>
</div>");
                }

                return Json(new { status = true, data = lineUnit.ToString() });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetHaulerBreakdown");
                return Json(new { status = false, remarks = "Gagal", data = ex.Message });
            }
        }

        [HttpGet]
        public ActionResult GetHaulerStandBy()
        {
            try
            {
                var resultUnit = _context.vw_t_setting_fleet
                    .Where(x =>
                        (x.loader == "unit_standby") &&
                        (x.cn_unit.Contains("RD") || x.cn_unit.Contains("DZ") || x.eq_class == "GD"))
                    .Distinct()
                    .OrderBy(x => x.eq_class == "DZ" || x.eq_class == "GD")
                    .ThenBy(x => x.cn_unit)
                    .ToList();

                var lineUnit = new StringBuilder();
                foreach (var unit in resultUnit)
                {
                    string imgSrc = unit.eq_class switch
                    {
                        "DZ" => "/img/dozer.png",
                        "GD" => "/img/grader.png",
                        _ => "/img/truck.png"
                    };

                    lineUnit.Append($@"
<div id='{unit.cn_unit}' class='hauler' style='margin-bottom: 5px;' draggable='true' ondragstart='onDragStart(event);'>
    <div class='card bg-light text-black shadow-sm' style='border: 3px solid {ViewBag.Setting?.border ?? "#FFC300"}; borde  r-radius: 10px;'>
        <div class='card-body'>
            <div class='row'>
                <div class='col-4 d-flex align-items-center'>
                    <img src='{imgSrc}' alt='{unit.cn_unit}' style='width: 100%; height: auto; border-radius: 5px;' />
                </div>
                <div class='col-8 d-flex align-items-center'>
                    <b style='font-size: 16pt;'>{unit.cn_unit}</b>
                </div>
            </div>
        </div>
    </div>
</div>");
                }

                return Json(new { status = true, data = lineUnit.ToString() });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetHaulerStandBy");
                return Json(new { status = false, remarks = "Gagal", data = ex.Message });
            }
        }

        [HttpGet]
        public ActionResult GetHaulerNoSetting()
        {
            try
            {
                var resultUnit = _context.vw_t_setting_fleet
                    .Where(x =>
                        (x.loader == "unit_no_setting" || x.loader == null) &&
                        (x.cn_unit.Contains("RD") || x.cn_unit.Contains("DZ") || x.eq_class == "GD")
                    )
                    .Distinct()
                    .OrderBy(x => x.eq_class == "DZ" || x.eq_class == "GD" ? 0 : 1) 
                    .ThenBy(x => x.cn_unit) 
                    .ToList();


                var lineUnit = new StringBuilder();
                foreach (var unit in resultUnit)
                {
                    string imgSrc;
                    if (unit.eq_class == "DZ")
                    {
                        imgSrc = "/img/dozer.png";
                    }
                    else if (unit.eq_class == "GD")
                    {
                        imgSrc = "/img/grader.png";
                    }
                    else
                    {
                        imgSrc = "/img/truck.png";
                    }
                    lineUnit.Append($@"
            <div id='{unit.cn_unit}' class='hauler' style='margin-bottom: 5px;' draggable='true' ondragstart='onDragStart(event);'>
                <div class='card bg-light text-black shadow-sm' style='border: 3px solid {ViewBag.Setting?.border ?? "#696cff"}; border-radius: 10px;'>
                    <div class='card-body'>
                        <div class='row'>
                            <div class='col-4 d-flex align-items-center'>
                                <img src='{imgSrc}' alt='{unit.cn_unit}' style='width: 100%; height: auto; border-radius: 5px;' />
                            </div>
                            <div class='col-8 d-flex align-items-center'>
                                <b style='font-size: 16pt;'>{unit.cn_unit}</b>
                            </div>
                        </div>
                    </div>
                </div>
            </div>");
                }

                return Json(new { status = true, data = lineUnit.ToString() });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetHaulerNoSetting");
                return Json(new { status = false, remarks = "Gagal", data = ex.Message });
            }
        }

    }
}
