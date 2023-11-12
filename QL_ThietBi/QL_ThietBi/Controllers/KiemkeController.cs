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
            if (Session["userNV"] != null)
            {
                NHANVIEN nv = Session["userNV"] as NHANVIEN;
                string manv = nv.MANV;
                DateTime d = DateTime.Now;
                string maph = "PHH" + d.Year + d.Month + d.Day + d.Hour + d.Minute + d.Second;
                PHIEUGHINHANHUHONG ph = new PHIEUGHINHANHUHONG();
                ph.ID_PHIEUGNHH = maph;
                ph.NGAYLAP = d;
                ph.MANV = manv;
                dt.PHIEUGHINHANHUHONGs.InsertOnSubmit(ph);
                dt.SubmitChanges();
            }
           

            return RedirectToAction("KiemKe");
        }
      public void CapNhatSLKho(string Matb,string soluong)
        {
            THIETBI tb = (from t in dt.THIETBIs where t.MATB == Matb select t).FirstOrDefault();
            try
            {
                tb.SOLUONG = tb.SOLUONG - int.Parse(soluong);
                dt.SubmitChanges();
            }
            catch
            {
                ViewBag.Error = "Lỗi";
            }
        }
        public void CapNhatSLKhoDelete(string Matb, string soluong)
        {
            THIETBI tb = (from t in dt.THIETBIs where t.MATB == Matb select t).FirstOrDefault();
            try
            {
                tb.SOLUONG = tb.SOLUONG +int.Parse(soluong);
                dt.SubmitChanges();
            }
            catch
            {
                ViewBag.Error = "Lỗi";
            }
        }
        public ActionResult Add_CTHH(string ID,string thietbi,string soluong,string ghichu)
        {
            CTPHIEUHH p = (from item in dt.CTPHIEUHHs where item.MATB == thietbi && item.ID_PHIEUGNHH == ID select item).FirstOrDefault();
            CTPHIEUHH ph = new CTPHIEUHH();
            THIETBI tb = (from t in dt.THIETBIs where t.MATB == thietbi select t).FirstOrDefault();
         
           if(tb.SOLUONG < int.Parse(soluong))
            {
                Session["Error"] = "Số lượng lớn hơn số lượng còn lại trong kho";
            }
            else if (p == null)
            {
                try
                {
                    Session["Error"] = null;
                    ph.MATB = thietbi;
                    ph.SOLUONG = int.Parse(soluong);
                    ph.ID_PHIEUGNHH = ID;
                    ph.GHICHU = ghichu;
                    dt.CTPHIEUHHs.InsertOnSubmit(ph);
                    dt.SubmitChanges();
                }
                catch
                {
                    ViewBag.Error = "Không thành công";
                }
                //Update lại số lượng trong kho
                CapNhatSLKho(thietbi, soluong);

            }
            else
            {
                try
                {
                    Session["Error"] = null;
                    p.SOLUONG += int.Parse(soluong);

                    dt.SubmitChanges();
                }
                catch
                {
                    ViewBag.Error = "Không thành công";
                }
                CapNhatSLKho(thietbi, soluong);
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
            THIETBI tb = (from t in dt.THIETBIs where t.MATB == thietbi select t).FirstOrDefault();
            int sl;
            int SOLUONG = int.Parse(soluong);
            int soluonghtphieu = ph.SOLUONG.GetValueOrDefault();
            int kq=0;
            if (int.Parse(soluong)>ph.SOLUONG)
            {
                 sl = SOLUONG - soluonghtphieu;
                 kq = tb.SOLUONG.GetValueOrDefault() + soluonghtphieu;

                tb.SOLUONG = kq;
                dt.SubmitChanges();
            }   
            else 
            {
                sl = soluonghtphieu - SOLUONG;
                CapNhatSLKhoDelete(thietbi,sl.ToString());
            }

            if (submitButton == "Update")
            {
                // Xử lý cập nhật nhân viên
                if (tb.SOLUONG < int.Parse(soluong))
                {
                    Session["Error"] = "Số lượng lớn hơn số lượng còn lại trong kho";
                }
                else if (ph != null)
                {
                    Session["Error"] = null;
                    ph.SOLUONG = int.Parse(soluong);
                    ph.GHICHU = ghichu;
                    dt.SubmitChanges();
                    CapNhatSLKho(thietbi, (kq - sl).ToString());
                  
                }
            }
            else if (submitButton == "Delete")
            {
                
          
                if (ph != null)
                {
                    //cập nhật lại số lượng khi xóa
                    CapNhatSLKhoDelete(thietbi, soluong);
                    dt.CTPHIEUHHs.DeleteOnSubmit(ph);
                    dt.SubmitChanges();
                }
            }
            return RedirectToAction("CT_DSHuHong", new { id });
        }
        public List<ThanhLy> laygiohang()
        {
            List<ThanhLy> lstGH = Session["Giohang"] as List<ThanhLy>;
            if (lstGH == null)
            {
                lstGH = new List<ThanhLy>();
                Session["Giohang"] = lstGH;
            }
            return lstGH;
        }
        public ActionResult ThanhLyTB(string ID)
        {
            DateTime d = DateTime.Now;
            string ma = "PTT" + Guid.NewGuid().ToString().Substring(0, 6);
            

                NHANVIEN NV = Session["userNV"] as NHANVIEN;
                PHIEUTHANHLY tl = new PHIEUTHANHLY();
                tl.MANV = NV.MANV;
                tl.ID_PHIEUGNHH = ID;
                tl.NGAYLAP = DateTime.Now;
                tl.GHICHU = "";
                tl.TONGTIEN = 0;
                tl.TRANGTHAI = false;
                tl.MAPHIEU = ma;
                dt.PHIEUTHANHLies.InsertOnSubmit(tl);
                dt.SubmitChanges();
                var ds = from item in dt.CTPHIEUHHs
                         join t in dt.THIETBIs on item.MATB equals t.MATB
                         where item.ID_PHIEUGNHH == ID
                         select new ThanhLy
                         {
                             MAPHIEU1 = ma,
                             MATB1 = item.MATB,
                             SOLUONG1 = item.SOLUONG.GetValueOrDefault(),
                             HUONTHANHLY1 = "",
                             TIENTHANHLY1 = 0,
                             TINHTRANG1 = "Hỏng",
                             ThietBi = new ThietBi
                             {
                                 //MATB = t.MATB,
                                 TENTB = t.TENTB,
                                 GIA = t.GIA.GetValueOrDefault()

                             }
                         };
                List<ThanhLy> thanhly = ds.ToList<ThanhLy>();
                ViewBag.MaPhieu = ma;

                return View(thanhly);

            //}
            //else
            //{
            //    var dsd = from item in dt.CTPHEUXLs
            //              join t in dt.THIETBIs on item.MATB equals t.MATB
            //              where item.MAPHIEUXL==ma
            //              select new ThanhLy
            //              {
            //                  MAPHIEU1 = ma,
            //                  MATB1 = item.MATB,
            //                  SOLUONG1 = item.SOLUONG.GetValueOrDefault(),
            //                  HUONTHANHLY1 = "",
            //                  TIENTHANHLY1 = 0,
            //                  TINHTRANG1 = "Hỏng",
            //                  ThietBi = new ThietBi
            //                  {
            //                      //MATB = t.MATB,
            //                      TENTB = t.TENTB,
            //                      GIA = t.GIA.GetValueOrDefault()

            //                  }
            //              };
            //    return View(dsd);

           // }

           
        }
        public ActionResult Delete_PhieuHH(string ID)
        {
            PHIEUGHINHANHUHONG ph = (from item in dt.PHIEUGHINHANHUHONGs where item.ID_PHIEUGNHH==ID select item).FirstOrDefault();
            try
            {
                dt.PHIEUGHINHANHUHONGs.DeleteOnSubmit(ph);
               
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
            dt.SubmitChanges();
            return RedirectToAction("KiemKe");
        }



    }
}