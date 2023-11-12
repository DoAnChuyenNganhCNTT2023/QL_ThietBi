using QL_ThietBi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QL_ThietBi.Controllers
{
    public class DangNhapController : Controller
    {
        QL_THIETBIDataContext dt = new QL_THIETBIDataContext();
        // GET: DangNhap
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Login()
        {
            return View();

        }
        [HttpPost]
        public ActionResult Login(string username, string pass)
        {
            if (string.IsNullOrEmpty(username) == true | string.IsNullOrEmpty(pass) == true)
            {
                ViewBag.erro = "Chưa Nhập Tài Khoản và Mật Khẩu!";
                return View();
            }
            var user = dt.GIAOVIENs.SingleOrDefault(m => m.EMAIL.ToLower() == username.ToLower());
            if (user == null)
            {
                ViewBag.erro = "Tài Khoản Không Tồn Tại!";
                ViewBag.username = username;
                return View();
            }
            if (user.PASSWORD != pass)
            {
                ViewBag.erroPass = "Sai Mật Khẩu !";
                return View();
            }
            Session["TenGV"] = user.TEN;
            Session["user"] = user;

            return RedirectToAction("DS_ThietBi", "Kiemke");

        }
        //đăng xuất
        public ActionResult Logout()
        {
            NHANVIEN nv = Session["userNV"] as NHANVIEN;
            if (Session["user"] != null)
            {
                Session.Remove("user");
                return RedirectToAction("Login");
            }
            else
            {
                if (Session["userNV"] != null)
                  
                    Session.Remove("userNV");
           
                return RedirectToAction("Index","Home");
            }

        }
        public ActionResult LoginNV()
        {
            return View();

        }
        [HttpPost]
        public ActionResult LoginNV(string Email, string password)
        {

            if (string.IsNullOrEmpty(Email) == true | string.IsNullOrEmpty(password) == true)
            {
                ViewBag.erro = "Chưa Nhập Tài Khoản và Mật Khẩu!";
                return View();
            }
            var user = dt.NHANVIENs.SingleOrDefault(m => m.EMAIL.ToLower() == Email.ToLower());
            if (user == null)
            {
                ViewBag.erro = "Tài Khoản Không Tồn Tại!";
                ViewBag.username = Email;
                return View();
            }
            if (user.MATKHAU != password)
            {
                ViewBag.erroPass = "Sai Mật Khẩu !";
                return View();
            }
            Session["TenNV"] = user.TENNV;
            Session["userNV"] = user;

            return RedirectToAction("DS_ThietBi", "Kiemke");
        }

    }
}