using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QL_ThietBi.Models;
using PagedList;
using System.Web.UI;
using System.Data.SqlClient;

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
            var ds = from d in dt.THIETBIs select d;
            List<THIETBI> products = ds.ToList<THIETBI>();
            ViewBag.ThietBi = new SelectList(products, "MATB", "TENTB");
          

            return View(links.ToPagedList(pageNumber, pageSize));
        }
        public ActionResult Add_PhieuHH()
        {
            DateTime d = DateTime.Now;
            string maph = "PHH" + d.Year + d.Month + d.Day + d.Hour + d.Minute + d.Second;
            PHIEUGHINHANHUHONG ph = new PHIEUGHINHANHUHONG();
            ph.ID_PHIEUGNHH = maph;
            ph.NGAYLAP = d;
            ph.MANV = "NV001";
            dt.PHIEUGHINHANHUHONGs.InsertOnSubmit(ph);
            dt.SubmitChanges();

            return RedirectToAction("KiemKe");
        }
      
        public ActionResult Add_CTHH(string ID,string thietbi,string soluong,string ghichu)
        {
            CTPHIEUHH p = (from item in dt.CTPHIEUHHs where item.MATB == thietbi && item.ID_PHIEUGNHH == ID select item).FirstOrDefault();
            CTPHIEUHH ph = new CTPHIEUHH();
            if (p == null)
            {
                try
                {
                    ph.MATB = thietbi;
                    ph.SOLUONG = int.Parse(soluong);
                    ph.ID_PHIEUGNHH = ID;
                    ph.GHICHU = ghichu;
                    dt.CTPHIEUHHs.InsertOnSubmit(ph);
                    dt.SubmitChanges();
                }
                catch (SqlException ex)
                {
                    // Bắt lỗi từ SQL Server
                    foreach (SqlError error in ex.Errors)
                    {
                        if (error.Class == 16) // Class 16 thường là lỗi do người dùng
                        {
                            // Xử lý lỗi trigger
                            string errorMessage = error.Message;

                            // Hiển thị thông báo lỗi cho người dùng sử dụng mã JavaScript
                            string script = $"alert('{errorMessage}');";
                       //     ScriptManager.RegisterStartupScript(HttpContext.Current.Handler as Page, typeof(Page), "ServerControlScript", script, true);

                            // Hoặc thực hiện các hành động khác
                            // ...
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Bắt lỗi chung
                    // Xử lý lỗi hoặc hiển thị thông báo cho người dùng
                    string errorMessage = ex.Message;
                    string script = $"alert('{errorMessage}');";
                  //  ScriptManager.RegisterStartupScript(HttpContext.Current.Handler as Page, typeof(Page), "ServerControlScript", script, true);
                    // ...
                }
            }
            else
            {
                p.SOLUONG += int.Parse(soluong);
               
                dt.SubmitChanges();
            }
            
            
                return RedirectToAction("CT_DSHuHong", new { ID });
            
         

        }
        public ActionResult Delete_CTHH(string id,string thietbi)
        {
            CTPHIEUHH ph = (from item in dt.CTPHIEUHHs where item.MATB == thietbi && item.ID_PHIEUGNHH == id select item).FirstOrDefault();
            dt.CTPHIEUHHs.DeleteOnSubmit(ph);
            dt.SubmitChanges();
            return RedirectToAction("CT_DSHuHong", new { id });
        }
        public ActionResult Update_CTHH(string id,string thietbi,string soluong,string submitButton,string ghichu)
        {
            CTPHIEUHH ph = (from item in dt.CTPHIEUHHs where item.MATB == thietbi && item.ID_PHIEUGNHH == id select item).FirstOrDefault();
            //ph.SOLUONG = int.Parse(soluong);
            //dt.SubmitChanges();
            if (submitButton == "Update")
            {
                // Xử lý cập nhật nhân viên
                
                if (ph != null)
                {
                   ph.SOLUONG = int.Parse(soluong);
                    ph.GHICHU = ghichu;
                    dt.SubmitChanges();
                  
                }
            }
            else if (submitButton == "Delete")
            {
                // Xử lý xóa nhân viên
          
                if (ph != null)
                {
                    dt.CTPHIEUHHs.DeleteOnSubmit(ph);
                    dt.SubmitChanges();
                }
            }
            return RedirectToAction("CT_DSHuHong", new { id });
        }
        public ActionResult ThanhLy()
        {
            return View();
        }
        
    }
}