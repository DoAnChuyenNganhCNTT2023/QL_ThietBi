using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QL_ThietBi.Models;
using PagedList;

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
        public ActionResult DS_ThietBi(int ?page)
        {
          
            if (page == null) page = 1;

            var links = (from item in dt.THIETBIs select item).OrderBy(x => x.MALOAI);


            int pageSize = 10;

            int pageNumber = (page ?? 1);

     
            return View(links.ToPagedList(pageNumber, pageSize));
        }

       
        public ActionResult KiemKe(int ?page)
        {

            if (page == null) page = 1;

            var links = (from item in dt.PHIEUGHINHANHUHONGs select item).OrderBy(x => x.MANV);


            int pageSize = 10;

            int pageNumber = (page ?? 1);


            return View(links.ToPagedList(pageNumber, pageSize));
        }
        public ActionResult CT_DSHuHong(int ?page,string ID)
        {
            if (page == null) page = 1;

            var links = (from item in dt.CTPHIEUHHs where item.ID_PHIEUGNHH==ID select item).OrderBy(x => x.MATB);


            int pageSize = 10;

            int pageNumber = (page ?? 1);
            ViewBag.Ma = ID;

            return View(links.ToPagedList(pageNumber, pageSize));
        }
        public ActionResult Add_PhieuHH()
        {
            return View();
        }
    }
}