using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QL_ThietBi.Models;

namespace QL_ThietBi.Controllers
{
    public class OrderController : Controller
    {
        // GET: Order
        QL_THIETBIDataContext data = new QL_THIETBIDataContext();
        public ActionResult Index()
        {
            return View();
        }
    }
}