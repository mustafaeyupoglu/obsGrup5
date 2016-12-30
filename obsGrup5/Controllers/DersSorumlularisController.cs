using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using obsGrup5;

namespace obsGrup5.Controllers
{
    public class DersSorumlularisController : Controller
    {
        private ObsMssqlEntities db = new ObsMssqlEntities();

        // GET: DersSorumlularis
        public ActionResult Index()
        {
            var dersSorumlulari = db.DersSorumlulari.Include(d => d.Bolum);
            return View(dersSorumlulari.ToList());
        }

        // GET: DersSorumlularis/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DersSorumlulari dersSorumlulari = db.DersSorumlulari.Find(id);
            if (dersSorumlulari == null)
            {
                return HttpNotFound();
            }
            return View(dersSorumlulari);
        }
        public ActionResult Onay(int id)
        {
            List<Ogrenci> query = (from o in db.Ogrenci
                         where o.DanismanId == id && o.OnayDurum == 0
                         select (o)).ToList<Ogrenci>();
            return View(query);
        }
        public ActionResult Onaylama(int id)
        {
            Ogrenci o = db.Ogrenci.Find(id);

            string ogAk = o.AktifKayitDonemi;
            ogAk = DonemBul(ogAk);

            o.AktifKayitDonemi = ogAk;
            o.OnayDurum = 1;
            db.SaveChanges();
            DersSorumlulari a=db.DersSorumlulari.Find(o.DanismanId);
            return RedirectToAction("amenu", "Login", a);
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
        // GET: DersSorumlularis/Create
        public ActionResult Create()
        {
            ViewBag.BolumKodu = new SelectList(db.Bolum, "Id", "BolumAdi");
            return View();
        }

        // POST: DersSorumlularis/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AkademisyenID,Adi,Soyadi,Unvani,Telefonu,Maili,OdaNo,BolumKodu,KullaniciAdi,Sifre")] DersSorumlulari dersSorumlulari)
        {
            if (ModelState.IsValid)
            {
                db.DersSorumlulari.Add(dersSorumlulari);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.BolumKodu = new SelectList(db.Bolum, "Id", "BolumAdi", dersSorumlulari.BolumKodu);
            return View(dersSorumlulari);
        }

        // GET: DersSorumlularis/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DersSorumlulari dersSorumlulari = db.DersSorumlulari.Find(id);
            if (dersSorumlulari == null)
            {
                return HttpNotFound();
            }
            ViewBag.BolumKodu = new SelectList(db.Bolum, "Id", "BolumAdi", dersSorumlulari.BolumKodu);
            return View(dersSorumlulari);
        }

        // POST: DersSorumlularis/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AkademisyenID,Adi,Soyadi,Unvani,Telefonu,Maili,OdaNo,BolumKodu,KullaniciAdi,Sifre")] DersSorumlulari dersSorumlulari)
        {
            if (ModelState.IsValid)
            {
                db.Entry(dersSorumlulari).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.BolumKodu = new SelectList(db.Bolum, "Id", "BolumAdi", dersSorumlulari.BolumKodu);
            return View(dersSorumlulari);
        }

        // GET: DersSorumlularis/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DersSorumlulari dersSorumlulari = db.DersSorumlulari.Find(id);
            if (dersSorumlulari == null)
            {
                return HttpNotFound();
            }
            return View(dersSorumlulari);
        }

        // POST: DersSorumlularis/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DersSorumlulari dersSorumlulari = db.DersSorumlulari.Find(id);
            db.DersSorumlulari.Remove(dersSorumlulari);
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
