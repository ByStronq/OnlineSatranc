using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OnlineSatranc.Models;
using OnlineSatranc.Models.EntityFramework;

namespace OnlineSatranc.Controllers
{
    public class SatrancTahtasiController : Controller
    {
        OnlineSatrancEntities db = new OnlineSatrancEntities();
        kullanicilar bilgilerim = new cookieDbUser().getDb();

        [Route("oda/{odaNumarasi}")]
        public ActionResult Index(int odaNumarasi)
        {
            var odalar = db.satrancTahtalari;
            var odaBilgileri = odalar.Where(s => s.ID == odaNumarasi);

            var odaBilgisi = odaBilgileri.FirstOrDefault();

            if (odaBilgisi.odaSahibi != bilgilerim.ID && odaBilgisi.kullanicilar.ELO >= bilgilerim.ELO - 500 && odaBilgisi.kullanicilar.ELO <= bilgilerim.ELO + 500) // Odanın sahibi ben değilsem
            {
                odalar.Find(odaNumarasi).rakip = bilgilerim.ID; // Rakip kısmına benim ismimi yaz
                db.SaveChanges();
                var guncelOdaBilgisi = odalar.Where(s => s.ID == odaNumarasi).FirstOrDefault();
                return View(guncelOdaBilgisi);
            }

            var model = odaBilgisi;

            return View(model);
        }

        [Route("kur"), HttpPost]
        public JsonResult OdaKur(bool BOT)
        {
            var model = db.satrancTahtalari.Add(new satrancTahtalari { odaSahibi = bilgilerim.ID });

            if (BOT) model.rakip = 6258; // Botlarla oyun başlatılmışsa

            db.SaveChanges();

            var satrancTahtasi = db.satrancTahtalari.Where(s => s.odaSahibi == bilgilerim.ID).ToList();
            int modelCount = satrancTahtasi.Count, odaNumarasi = 0;

                if (modelCount != 0)
                {
                    odaNumarasi = satrancTahtasi.FirstOrDefault().ID;
                }

            var JsonModel = new
            {
                odaID = odaNumarasi
            };

            return Json(JsonModel, JsonRequestBehavior.AllowGet);
        }
        
        [Route("eslestir"), HttpPost]
        public JsonResult Eslestir()
        {
            Random rnd = new Random();
            var model = db.satrancTahtalari.Where(s => s.kullanicilar.ELO >= bilgilerim.ELO - 500 && s.kullanicilar.ELO <= bilgilerim.ELO + 500 && s.rakip== null).ToList();
            int modelCount = model.Count, odaNumarasi = 0;
                
                if (modelCount != 0)
                {
                    int indeks = rnd.Next(0, modelCount - 1);
                        odaNumarasi = model.ElementAt(indeks).ID;
                }

            var JsonModel = new
            {
                odaID = odaNumarasi
            };
            return Json(JsonModel, JsonRequestBehavior.AllowGet);
        }

        [Route("rakipGetir"), HttpPost]
        public JsonResult rakipGetir(int odaNo)
        {
            kullanicilar rakipBilgileri;
            int rakipBilgileriID = 0, rakipBilgileriELO = 0;
            string rakipBilgileriKadi = "";
            bool rakipBilgileriCinsiyet = true;


            if (db.satrancTahtalari.Where(s => s.ID == odaNo).FirstOrDefault().rakip != null)
            {
                rakipBilgileri = db.satrancTahtalari.Where(s => s.ID == odaNo).FirstOrDefault().kullanicilar1;
                rakipBilgileriID = rakipBilgileri.ID;
                rakipBilgileriKadi = rakipBilgileri.kAdi;
                rakipBilgileriCinsiyet = rakipBilgileri.cinsiyet;
                rakipBilgileriELO = rakipBilgileri.ELO;
            }

            var JsonModel = new
            {
                rakipID = rakipBilgileriID,
                rakipKadi = rakipBilgileriKadi,
                rakipCinsiyet = rakipBilgileriCinsiyet,
                rakipELO = rakipBilgileriELO
            };

            return Json(JsonModel, JsonRequestBehavior.AllowGet);
        }

        [Route("notasyonGonder"), HttpPost]
        public JsonResult notasyonGonder(string notasyon, string pozisyon, int odaNo, string yenenTas)
        {
            var jsonSonuc = false;
            var tasRenk = false;

            if (db.satrancTahtalari.Where(s => s.ID == odaNo).FirstOrDefault().kullanicilar.ID == new OnlineSatranc.Models.cookieDbUser().getDb().ID)
                tasRenk = true;

            satrancAlgoritmasi satranc = new satrancAlgoritmasi(notasyon, pozisyon, odaNo, tasRenk, yenenTas);

            if (satranc.HamleKontrol())
            {
                satranc.notasyonGonder();
                jsonSonuc = true;
            }

            var JsonModel = new
            {
                sonuc = jsonSonuc
            };

            return Json(JsonModel, JsonRequestBehavior.AllowGet);
        }

        [Route("notasyonGetir"), HttpPost]
        public JsonResult notasyonGetir(int odaNo)
        {
            satrancAlgoritmasi satranc = new satrancAlgoritmasi(odaNo);
            var hamleBilgisi = satranc.notasyonGetir();

            var JsonModel = new
            {
                hamle = hamleBilgisi
            };

            return Json(JsonModel, JsonRequestBehavior.AllowGet);
        }

        [Route("odaKontrol"), HttpPost]
        public JsonResult odaKontrol(int ID, int odaNo)
        {
            var sahip = false;
            var rakip = true;
            var sira = true;
            bool sahipHamleYaptiMi = false, rakipHamleYaptiMi = false;
            var bilgilerim = new cookieDbUser().getDb();

            if (bilgilerim.ID == ID && db.satrancTahtalari.Where(s => s.ID == odaNo && s.odaSahibi == bilgilerim.ID).Count() > 0)
            {
                sahip = true;
                rakip = false;
            }
            else if (bilgilerim.ID != ID && db.satrancTahtalari.Where(s => s.ID == odaNo && s.rakip == bilgilerim.ID).Count() > 0)
            {
                sahip = false;
                rakip = true;
            }
            else
            {
                sahip = false;
                rakip = false;
            }

            var hamleAdedi = db.satrancTahtalari.Where(s => s.ID == odaNo).FirstOrDefault().hamleler.Count();

            if (hamleAdedi % 2 == 0) {
                    if (rakip == true)
                        sira = false;

                sahipHamleYaptiMi = false;
                rakipHamleYaptiMi = true;

                if (hamleAdedi == 0) rakipHamleYaptiMi = false;
            }
            else if (hamleAdedi % 2 == 1)
            {
                    if (sahip == true)
                        sira = false;

                sahipHamleYaptiMi = true;
                rakipHamleYaptiMi = false;

                if (db.satrancTahtalari.Where(s => s.ID == odaNo).FirstOrDefault().rakip == 6258) // Odada bot var ise
                {
                    int hamleSayisi = db.hamleler.Where(s => s.odaNo == odaNo).Count();

                    int hamle = hamleSayisi / 2; // Botun kayıtlı hamlelerinde sırada olan

                    db.hamleler.Add(new hamleler
                    {
                        notasyon = db.hamleler.Where(s => s.odaNo == 9805).ToList().ElementAt(hamle).notasyon,
                        pozisyon = db.hamleler.Where(s => s.odaNo == 9805).ToList().ElementAt(hamle).pozisyon,
                        odaNo = odaNo
                    });
                    db.SaveChanges();

                    sira = true;
                    sahipHamleYaptiMi = false;
                    rakipHamleYaptiMi = true;

                }
            }

            var JsonModel = new
            {
                odaSahibi = sahip,
                odaRakip = rakip,
                hamleSirasi = sira,
                sahipHamle = sahipHamleYaptiMi,
                rakipHamle = rakipHamleYaptiMi
                
            };

            return Json(JsonModel, JsonRequestBehavior.AllowGet);
        }
    }
}