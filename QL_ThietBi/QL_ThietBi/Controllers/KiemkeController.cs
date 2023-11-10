using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QL_ThietBi.Models;

namespace QL_ThietBi.Controllers
{
    public class KiemkeController : Controller
    {
        // GET: Kiemke
        QL_THIETBIDataContext dt = new QL_THIETBIDataContext();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult DS_ThietBi()
        {
            var ds = from item in dt.THIETBIs select item;
            return View(ds.ToList());
        }

        [HttpGet]
        public ActionResult KiemKe()
        {
            return View();
        }
    }
}