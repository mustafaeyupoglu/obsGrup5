using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using obsGrup5.Models;
namespace obsGrup5.Controllers
{
    public class NotsController : Controller
    {
        ObsMssqlEntities db = new ObsMssqlEntities();
        int tarih = DateTime.Now.Year ;
        // GET: Nots
        public ActionResult Index(int id)
        {
         

            var query = (from ad in db.AcilanDersler
                        from k in db.Kayit
                        from notlar in db.Notlar
                        where id == ad.AkademisyenId && ad.ADId == k.ADId && k.KayitId == notlar.KayitId
                        select new Nots {AkademisyenId=id ,OgrenciNo = k.OgrenciNo, DersAdi = ad.DersAdi, DersKodu = ad.DersKodu, YariYil = ad.YariYil,
                            YilDers = ad.YilDers,Vize=notlar.Vize,Final=notlar.Final,But=notlar.But,YilNot=notlar.YilNot,HarfNotu=notlar.HarfNotu
                         }).ToList<Nots>();

            return View(query);
        }
        public ActionResult Transkript(int ogrNo)
        {
            var query = (from ad in db.AcilanDersler
                         from k in db.Kayit
                         from notlar in db.Notlar
                         from d in db.Dersler
                         where ogrNo == k.OgrenciNo && ad.ADId == k.ADId && k.KayitId == notlar.KayitId && d.DersKodu==ad.DersKodu
                         select new Transkript
                         {
                             AkademisyenId = ad.AkademisyenId,
                             OgrenciNo = k.OgrenciNo,
                             DersAdi = ad.DersAdi,
                             DersKodu = ad.DersKodu,
                             YariYil = ad.YariYil,
                             YilDers = ad.YilDers,
                             Kredi = d.Kredi,
                             AKTS = d.AKTS,
                             YilNot = notlar.YilNot,
                             HarfNotu = notlar.HarfNotu
                         }).ToList<Transkript>();
            ViewBag.gno = gnoHesapla(query);


            return View(query);
        }
        private double gnoHesapla(List<Transkript> query)
        {
            double katsayi = 0;
            int krediTop = 0;
            double gno = 0;
            foreach (var item in query)
            {
                katsayi = katsayiBul(item.HarfNotu);
                krediTop += Convert.ToInt32(item.Kredi);
                gno += katsayi * Convert.ToDouble(item.Kredi);
            }
            gno = gno / krediTop;
            return gno;

        }

        private double katsayiBul(string harf)
        {
            if (harf == "AA")
            {
                return 4;
            }
            else if (harf == "BA")
            {
                return 3.5;
            }
            else if (harf == "BB")
            {
                return 3;
            }
            else if (harf == "CB")
            {
                return 2.5;
            }
            else if (harf == "CC")
            {
                return 2;
            }
            else if (harf == "DC")
            {
                return 1.5;
            }
            else if (harf == "DD")
            {
                return 1;
            }
            else if (harf == "FD")
            {
                return 0.5;
            }
            else if (harf == "FF")
            {
                return 0;
            }
            else
            {
                return 2;
            }

        }
        public ActionResult OgrenciNot(int ogrNo)
        {
            var query=(from ad in db.AcilanDersler
                        from k in db.Kayit
                        from notlar in db.Notlar
                           where ogrNo == k.OgrenciNo && ad.ADId == k.ADId && k.KayitId == notlar.KayitId
                           select new Nots {AkademisyenId=ad.AkademisyenId ,OgrenciNo = k.OgrenciNo, DersAdi = ad.DersAdi, DersKodu = ad.DersKodu, YariYil = ad.YariYil,
                            YilDers = ad.YilDers,Vize=notlar.Vize,Final=notlar.Final,But=notlar.But,YilNot=notlar.YilNot,HarfNotu=notlar.HarfNotu
                         }).ToList<Nots>();

            return View(query);
        }
        public ActionResult Create(int id)
        {
           
            comboDoldur(id);
            Nots n = new Nots();
            n.AkademisyenId = id;
            return View(n);
        }
        [HttpPost]
        public ActionResult Create(Nots n)
        {
            
            if (n.OgrenciNo!=null && n.DersKodu!="" || n.DersKodu==null)
            {
                 NotEkle(n);
                 return RedirectToAction("Create", new { id = n.AkademisyenId });
            }

            comboDoldur(int.Parse(n.AkademisyenId.ToString()));
            Nots nn = new Nots { AkademisyenId = n.AkademisyenId };
            return View(nn);
        }
        

        private void NotEkle(Nots n)
        {
           
            int a = (from dk in db.AcilanDersler
                     where dk.DersKodu == n.DersKodu && dk.YilDers == tarih
                     select new { dk.ADId }).ToList()[0].ADId;
            Kayit k=new Kayit{ADId=a,OgrenciNo=n.OgrenciNo};
            db.Kayit.Add(k);
            db.SaveChanges();

            double yilNot=0;
            
           if (n.But==0)
	       {
	  	        yilNot=((int.Parse(n.Vize.ToString())*4)/10)+((int.Parse(n.Final.ToString())*6)/10);
	       }
        else
	        {
               yilNot=((int.Parse(n.Vize.ToString())*4)/10)+((int.Parse(n.But.ToString())*6)/10);
	        }
            
            string harfNotu=HarfHesapla(yilNot);

             //   AcilanDersler
              Notlar notlar = new Notlar {KayitId=k.KayitId,Vize=n.Vize,Final=n.Final,But=n.But,YilNot=yilNot,HarfNotu=harfNotu,OtomatikMi="FALSE" };
              db.Notlar.Add(notlar);
              db.SaveChanges();

              
        }

        private string HarfHesapla(double yilNot)
        {
            if (yilNot<40)
            {
                return "FF";
            }
            else if(yilNot>=40 && yilNot<50)
            {
                return "FD";
            }
            else if (yilNot >= 50 && yilNot < 54)
            {
                return "DD";
            }
            else if (yilNot >= 55 && yilNot < 60)
            {
                return "DC";
            }
            else if (yilNot >= 60 && yilNot < 70)
            {
                return "CC";
            }
            else if (yilNot >= 70 && yilNot < 80)
            {
                return "CB";
            }
            else if (yilNot >= 80 && yilNot < 85)
            {
                return "BB";
            }
            else if (yilNot >= 85 && yilNot < 90)
            {
                return "BA";
            }
            else if (yilNot >= 90 && yilNot < 101)
            {
                return "AA";
            }
            return "-";
        }


        void comboDoldur(int id)
        {
            
            var a = (from dk in db.AcilanDersler
                     where dk.AkademisyenId == id && dk.YilDers==tarih
                     select new { dk.DersKodu ,dk.DersAdi}).ToList();
            ViewBag.DersKodu = new SelectList(a, "DersKodu", "DersAdi");


        }
    }
}