using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace obsGrup5.Controllers
{
    public class LoginController : Controller
    {

        ObsMssqlEntities mydb = new ObsMssqlEntities();
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult amenu(DersSorumlulari d)
        {
            return View("AkademisyenAnamenu", d);
        }
        public ActionResult omenu(Ogrenci o)
        {
            return View("OgrenciAnamenu",o);
        }
        public ActionResult imenu()
        {
            return View("IdariAnamenu");
        }
    
        [HttpPost]
        public ActionResult Index(string kullaniciadi, string parola)
        {

            var _kullanici =( from kul in mydb.Ogrenci
                             where kullaniciadi == kul.OgrenciNo.ToString() && parola == kul.OgrenciNo.ToString()
                             select (kul)).ToList();
           
            
            if (_kullanici.Count() > 0)
            {
                Ogrenci o = _kullanici[0];
                return View("OgrenciAnamenu",o);
            }
            var _kullanici2 = (from kul in mydb.DersSorumlulari
                              where kullaniciadi == kul.KullaniciAdi.ToString() && parola == kul.Sifre.ToString()
                              select (kul)).ToList();
            if (_kullanici2.Count > 0)
            {
              DersSorumlulari k= _kullanici2[0];
                return View("AkademisyenAnamenu",k);
            }
            var _kullanici3 = (from kul in mydb.Idari
                            where kullaniciadi == kul.KullaniciAdi.ToString() && parola == kul.Sifre.ToString()
                            select (kul)).ToList();
            if (_kullanici3.Count() > 0)
            {
                return View("IdariAnamenu");
            }
            else
            {
                ViewBag.mesaj = "Yanlis Kullanici Ismi Yada Parola Girildi";
                return View("Index");
            }
        }
        public ActionResult Anamenu()
        {
            return View();
        }
        
    }
}