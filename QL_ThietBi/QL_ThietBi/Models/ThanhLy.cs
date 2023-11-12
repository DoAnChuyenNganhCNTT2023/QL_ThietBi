using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QL_ThietBi.Models
{
    public class ThanhLy
    {
        QL_THIETBIDataContext dt = new QL_THIETBIDataContext();
        string MAPHIEU;
        string MATB;
        int SOLUONG;
        string TINHTRANG;
        string HUONTHANHLY;
        float TIENTHANHLY;

        public string MAPHIEU1 { get => MAPHIEU; set => MAPHIEU = value; }
        public string MATB1 { get => MATB; set => MATB = value; }
        public int SOLUONG1 { get => SOLUONG; set => SOLUONG = value; }
        public string TINHTRANG1 { get => TINHTRANG; set => TINHTRANG = value; }
        public string HUONTHANHLY1 { get => HUONTHANHLY; set => HUONTHANHLY = value; }
        public float TIENTHANHLY1 { get => TIENTHANHLY; set => TIENTHANHLY = value; }
        public ThietBi ThietBi { get; set; }
        public ThanhLy()
        { }
        //public List<CTPHEUXL> Layds (string id,string maphieuhh)
        //{
        //    var ds = from item in dt.CTPHIEUHHs where item.ID_PHIEUGNHH == maphieuhh select item;
        //    List<CTPHIEUHH> ct = ds.ToList<CTPHIEUHH>();
        //    for(int i=0;i<ct.Count;i++)
        //    {
        //        ThanhLy tl = new ThanhLy();
        //        tl.MAPHIEU = id;
        //        tl.MATB = ct[i].MATB;
        //        tl.SOLUONG = ct[i].SOLUONG.GetValueOrDefault();
        //        tl.TINHTRANG = "";
        //        tl.HUONTHANHLY = "";
        //        tl.TIENTHANHLY = 0;
        //    }
           

        //}    
    }
    public class ThietBi
    {
        public int MATB { get; set; }
        public string TENTB { get; set; }
        public double GIA { get; set; }
        // Các thuộc tính khác của ChucVu
    }
}