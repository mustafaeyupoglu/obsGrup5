using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using obsGrup5;
using obsGrup5.Models;

namespace obsGrup5.Controllers
{
    public class DerslersController : Controller
    {
        private ObsMssqlEntities db = new ObsMssqlEntities();
        int tarih = DateTime.Now.Year;
        // GET: Derslers
        public ActionResult Index()
        {
            var dersler = db.Dersler.Include(d => d.Donemler);
            return View(dersler.ToList());
        }

        public ActionResult DersKayit(int ogrNo)
        {
            Ogrenci o = db.Ogrenci.Find(ogrNo);
            o.OnayDurum = 0;
            db.SaveChanges();
            List<Kayit> klist=new List<Kayit>();
            
            foreach (var item in sl)
            {
                 klist.Add(new Kayit { OgrenciNo = ogrNo, ADId = item.ADId });
                
            }
            db.Kayit.AddRange(klist);
            db.SaveChanges();
            sl.Clear();
            return RedirectToAction("omenu", "Login",o);
        }

        public ActionResult Derssecme(int ogrNo)
        {
            var list = SecilebilirDersListele(ogrNo);

            DersSec d = new DersSec { OgrenciNo = ogrNo };
            ViewBag.ADId = new SelectList(list, "ADId", "DersAdi");
            return View(d);
        }
      static  List<Secilen> sl = new List<Secilen>();
        public ActionResult DerssecPartial(int adId,string adi)
        {
            Secilen s = new Secilen { ADId = adId, DersAdi = adi };
            bool check = sl.Exists(x => x.ADId == s.ADId);
            if (!check)
            {
                sl.Add(s);
            }
            
            return PartialView("DerssecPartial", sl);
        }
        public ActionResult DerssecPartial2()
        {
            sl.Clear();

            return PartialView("DerssecPartial", sl);
        }
        private IEnumerable<object> SecilebilirDersListele(int ogrNo)
        {

           string ogAk= db.Ogrenci.Find(ogrNo).AktifKayitDonemi;
          ogAk=  DonemBul(ogAk);
          
          if(DonemBul2(DateTime.Now.Month)==1)
          {
              var  liste = (from ad in db.AcilanDersler
                           from k in db.Kayit
                           from n in db.Notlar
                           where ad.YilDers == tarih && k.OgrenciNo == ogrNo
                                 && k.KayitId == n.KayitId && n.YilNot < 50
                                 && k.ADId == ad.ADId
                                 && (ad.YariYil == "1.1" || ad.YariYil == "1.1" || ad.YariYil == "1.1" || ad.YariYil == "1.1")
                           select new { ad.ADId, ad.DersAdi }).Union(from ad in db.AcilanDersler
                                                                     where (ad.YariYil == ogAk && ad.YilDers == tarih)
                                                                     select new { ad.ADId, ad.DersAdi }).ToList();
              return liste;
          }
          else
          {
              var liste = (from ad in db.AcilanDersler
                           from k in db.Kayit
                           from n in db.Notlar
                           where ad.YilDers == tarih && k.OgrenciNo == ogrNo
                                 && k.KayitId == n.KayitId && n.YilNot < 50
                                 && k.ADId == ad.ADId
                                 && (ad.YariYil == "1.2" || ad.YariYil == "2.2" || ad.YariYil == "3.2" || ad.YariYil == "4.2")
                           select new { ad.ADId, ad.DersAdi }).Union(from ad in db.AcilanDersler
                                                                     where (ad.YariYil == ogAk && ad.YilDers == tarih)
                                                                     select new { ad.ADId, ad.DersAdi }).ToList();
               return liste;
          }

          
          
         
        }

        private int DonemBul2(int tarih)
        {
           switch(tarih){
               case 1: return 1;
               case 2: return 2;
               case 3: return 2;
               case 4: return 2;
               case 5: return 2;
               case 6: return 2;
               case 7: return 2;
               case 8: return 1;
               case 9: return 1;
               case 10: return 1;
               case 11: return 1;
               case 12: return 1;
               default: return 1;
           }
        }



        private string DonemBul(string ogAk)
        {
            if (ogAk == "1.1")
            {
                return "1.2";
            }
            else if (ogAk == "1.2")
            {
                return "2.1";
            }
            else if (ogAk == "2.1")
            {
                return "2.2";
            }
            else if (ogAk == "2.2")
            {
                return "3.1";
            }
            else if (ogAk == "3.1")
            {
                return "3.2";
            }
            else if (ogAk == "3.2")
            {
                return "4.1";
            }
            else if (ogAk == "4.1")
            {
                return "4.2";
            }
            else
            {
                return ogAk;
            }
            
        }

        public ActionResult Dersacma()
        {
            comboDoldur();
            return View();
        }
        [HttpPost]
        public ActionResult Dersacma(AcilanDersler ad)
        {
            if (ad.DersKodu != null || ad.DersKodu=="" && ad.AkademisyenId == null)
            {
                dersAc(ad);
                return RedirectToAction("imenu", "Login");
            }

            comboDoldur();
            return View();
        }

        private void dersAc(AcilanDersler ad)
        {
            var a = (from dk in db.Dersler
                     where dk.DersKodu == ad.DersKodu
                     select new { dk.DersAdi,dk.Donem}).ToList()[0];
           
            ad.DersAdi = a.DersAdi;
            ad.YariYil = a.Donem;
            ad.YilDers = tarih;

            db.AcilanDersler.Add(ad);
            db.SaveChanges();
        }
        void comboDoldur()
        {

            var a = (from dk in db.Dersler                 
                     select new { dk.DersKodu,dk.DersAdi }).ToList();
            ViewBag.DersKodu = new SelectList(a, "DersKodu", "DersAdi");

            var dersSorum = (from ds in db.DersSorumlulari
                             let adSoyad = ds.Adi + " " + ds.Soyadi
                             select new { ds.AkademisyenID, adSoyad }).ToList();
            ViewBag.AkademisyenID = new SelectList(dersSorum, "AkademisyenID", "adSoyad");
        }
        // GET: Derslers/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Dersler dersler = db.Dersler.Find(id);
            if (dersler == null)
            {
                return HttpNotFound();
            }
            return View(dersler);
        }

        // GET: Derslers/Create
        public ActionResult Create()
        {
            ViewBag.Donem = new SelectList(db.Donemler, "Donem", "Donem");
            return View();
        }

        // POST: Derslers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "DersKodu,DersAdi,Tur,Kredi,AKTS,TeoriDersSaati,UygulamaDersSaati,Donem")] Dersler dersler)
        {
            if (ModelState.IsValid)
            {
                db.Dersler.Add(dersler);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Donem = new SelectList(db.Donemler, "Donem", "Donem", dersler.Donem);
            return View(dersler);
        }

        // GET: Derslers/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Dersler dersler = db.Dersler.Find(id);
            if (dersler == null)
            {
                return HttpNotFound();
            }
            ViewBag.Donem = new SelectList(db.Donemler, "Donem", "Donem", dersler.Donem);
            return View(dersler);
        }

        // POST: Derslers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "DersKodu,DersAdi,Tur,Kredi,AKTS,TeoriDersSaati,UygulamaDersSaati,Donem")] Dersler dersler)
        {
            if (ModelState.IsValid)
            {
                db.Entry(dersler).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Donem = new SelectList(db.Donemler, "Donem", "Donem", dersler.Donem);
            return View(dersler);
        }

        // GET: Derslers/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Dersler dersler = db.Dersler.Find(id);
            if (dersler == null)
            {
                return HttpNotFound();
            }
            return View(dersler);
        }

        // POST: Derslers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Dersler dersler = db.Dersler.Find(id);
            db.Dersler.Remove(dersler);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
                
            }
            base.Dispose(disposing);
        }
    }
}
