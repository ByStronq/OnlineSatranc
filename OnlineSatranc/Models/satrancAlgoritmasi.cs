using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OnlineSatranc.Models.EntityFramework;

namespace OnlineSatranc.Models
{
    public class satrancAlgoritmasi
    {
        OnlineSatrancEntities db = new OnlineSatrancEntities();

        private string notasyon, pozisyon, yenenTas;
        private int odaNo;
        private bool tasRenk;
        private char[] hamleKomut;
        private char sutun, satir;

        public satrancAlgoritmasi(int gelenOdaNo)
        {
            odaNo = gelenOdaNo;
        }

        public satrancAlgoritmasi(string gelenNotasyon, string gelenPozisyon, int gelenOdaNo, bool gelenTasRenk, string gelenYenenTas)
        {
            notasyon = gelenNotasyon;
            pozisyon = gelenPozisyon;
            odaNo = gelenOdaNo;
            tasRenk = gelenTasRenk;
            yenenTas = gelenYenenTas;

                hamleKomut = notasyon.ToCharArray();
                sutun = pozisyon.ToCharArray()[0];
                satir = pozisyon.ToCharArray()[1];
        }

        public List<string> olasiHamleler()
        {
            List<string> hamleList = new List<string>();
            return hamleList;
        }

        public bool HamleKontrol()
        {
            if (hamleKomut[1] == 'x')
            {
                hamleKomut[1] = hamleKomut[2];
                hamleKomut[2] = hamleKomut[3];
            }

            switch (hamleKomut[0])
            {
                case 'Ş': // Taş Şah ise
                        if (hamleKomut[1] == sutun && hamleKomut[2] != satir) // Aşağı-Yukarı
                        {
                            if (hamleKomut[2] == satir + 1) return true; // 1 Adım Yukarı
                            else if (hamleKomut[2] == satir - 1) return true; // 1 Adım Aşağı
                        }
                        else if (hamleKomut[1] != sutun && hamleKomut[2] == satir) // Sağa-Sola
                        {
                            if (hamleKomut[1] == sutun + 1) return true; // 1 Adım Sağa
                            else if (hamleKomut[1] == sutun - 1) return true; // 1 Adım Sola
                        }
                        else // Çapraz
                            if (hamleKomut[1] == sutun + 1 && hamleKomut[2] == satir + 1) return true; // 1 Adım Sağ-Yukarı
                            else if (hamleKomut[1] == sutun + 1 && hamleKomut[2] == satir - 1) return true; // 1 Adım Sağ-Aşağı
                            else if (hamleKomut[1] == sutun - 1 && hamleKomut[2] == satir + 1) return true; // 1 Adım Sol-Yukarı
                            else if (hamleKomut[1] == sutun - 1 && hamleKomut[2] == satir - 1) return true; // 1 Adım Sol-Aşağı
                    break;
                case 'V': // Taş Vezir ise
                        if (kabiliyet("Yukarı-Aşağı") || kabiliyet("Sağa-Sola") || kabiliyet("Çapraz"))   return true;
                    break;
                case 'F': // Taş Fil ise
                        if (kabiliyet("Çapraz")) return true;
                    break;
                case 'A': // Taş At ise
                        if(hamleKomut[1] == sutun + 2 && hamleKomut[2] == satir + 1) return true; // 3 sağa 1 yukarı
                        else if (hamleKomut[1] == sutun + 2 && hamleKomut[2] == satir - 1) return true; // 3 sağa 1 aşağı
                        else if(hamleKomut[1] == sutun - 2 && hamleKomut[2] == satir + 1) return true; // 3 sola 1 yukarı
                        else if (hamleKomut[1] == sutun - 2 && hamleKomut[2] == satir - 1) return true; // 3 sola 1 aşağı
                        else if (hamleKomut[1] == sutun + 1 && hamleKomut[2] == satir + 2) return true; // 3 yukarı 1 sağa
                        else if (hamleKomut[1] == sutun - 1 && hamleKomut[2] == satir + 2) return true; // 3 yukarı 1 sola
                        else if (hamleKomut[1] == sutun + 1 && hamleKomut[2] == satir - 2) return true; // 3 aşağı 1 sağa
                        else if (hamleKomut[1] == sutun - 1 && hamleKomut[2] == satir - 2) return true; // 3 aşağı 1 sola
                    break;
                case 'K': // Taş Kale ise
                        if (kabiliyet("Yukarı-Aşağı") || kabiliyet("Sağa-Sola")) return true;
                    break;
                default: // Taş piyon ise
                         /*if (hamleKomut.Length == 5 && hamleKomut[2] == 'x')
                         {
                             hamleKomut[0] = hamleKomut[3];
                             hamleKomut[1] = hamleKomut[4];

                                 if (tasRenk == true && hamleKomut[0] == sutun + 1 && hamleKomut[1] == satir + 1 || tasRenk == false && hamleKomut[0] == sutun + 1 && hamleKomut[1] == satir - 1) return true; // Sağ-Çapraz yeme
                                 else if (tasRenk == true && hamleKomut[0] == sutun - 1 && hamleKomut[1] == satir + 1 || tasRenk == false && hamleKomut[0] == sutun - 1 && hamleKomut[1] == satir - 1) return true; // Sol-Çapraz yeme
                         }else
                         {*/
                    if (hamleKomut[0] == sutun && hamleKomut[1] != satir) { // Yukarı-Aşağı
                            if ((tasRenk == true && hamleKomut[1] == satir + 1) || (tasRenk == false && hamleKomut[1] == satir - 1)) return true; // 1 Adım Yukarı
                            else if ((tasRenk == true && satir == '2' && hamleKomut[1] == satir + 2) || (tasRenk == false && satir == '7' && hamleKomut[1] == satir - 2)) return true; // piyon ilk hamlede iki adım gidebilir
                    }
                    else if (hamleKomut[3] != sutun && hamleKomut[4] != satir) // Capraz
                            if ((tasRenk == true && hamleKomut[4] == satir + 1 && (hamleKomut[3] == sutun + 1 || hamleKomut[3] == sutun - 1)) || (tasRenk == false && hamleKomut[4] == satir - 1 && (hamleKomut[3] == sutun + 1 || hamleKomut[3] == sutun - 1))) return true; // Çapraz taş yeme
                          //}
                    break;
            }

            return false; // Notasyon uymuyorsa;
        }

        public bool kabiliyet(string yon)
        {
            switch (yon)
            {
                case "Yukarı-Aşağı":
                        if (hamleKomut[1] == sutun && hamleKomut[2] != satir) // Yukarı-Aşağı
                            if (hamleKomut[2] > satir) { if (aradaTasVarMi("Yukarı") == false) return true; }  // Yukarı
                            else if (hamleKomut[2] < satir) { if (aradaTasVarMi("Aşağı") == false) return true; }  // Aşağı
                    break;
                case "Sağa-Sola":
                        if (hamleKomut[1] != sutun && hamleKomut[2] == satir) // Sağa-Sola
                            if (hamleKomut[1] > sutun)  { if (aradaTasVarMi("Sağ") == false) return true; }  // Sağa
                            else if (hamleKomut[1] < sutun)  { if (aradaTasVarMi("Sol") == false) return true; }  // Sola
                    break;
                case "Çapraz":
                        if (hamleKomut[1] == sutun + 1 && hamleKomut[2] == satir + 1 || hamleKomut[1] == sutun + 2 && hamleKomut[2] == satir + 2 || hamleKomut[1] == sutun + 3 && hamleKomut[2] == satir + 3 
                            || hamleKomut[1] == sutun + 4 && hamleKomut[2] == satir + 4 || hamleKomut[1] == sutun + 5 && hamleKomut[2] == satir + 5 || hamleKomut[1] == sutun + 6 && hamleKomut[2] == satir + 6
                            || hamleKomut[1] == sutun + 7 && hamleKomut[2] == satir + 7) { if (aradaTasVarMi("Sağ-Yukarı") == false) return true; } //Sağ-Yukarı arada taş yoksa
                        else if (hamleKomut[1] == sutun + 1 && hamleKomut[2] == satir - 1 || hamleKomut[1] == sutun + 2 && hamleKomut[2] == satir - 2 || hamleKomut[1] == sutun + 3 && hamleKomut[2] == satir - 3
                                || hamleKomut[1] == sutun + 4 && hamleKomut[2] == satir - 4 || hamleKomut[1] == sutun + 5 && hamleKomut[2] == satir - 5 || hamleKomut[1] == sutun + 6 && hamleKomut[2] == satir - 6
                                || hamleKomut[1] == sutun + 7 && hamleKomut[2] == satir - 7) { if (aradaTasVarMi("Sağ-Aşağı") == false) return true; } //Sağ-Aşağı arada taş yoksa
                        else if (hamleKomut[1] == sutun - 1 && hamleKomut[2] == satir + 1 || hamleKomut[1] == sutun - 2 && hamleKomut[2] == satir + 2 || hamleKomut[1] == sutun - 3 && hamleKomut[2] == satir + 3 
                                || hamleKomut[1] == sutun - 4 && hamleKomut[2] == satir + 4 || hamleKomut[1] == sutun - 5 && hamleKomut[2] == satir + 5 || hamleKomut[1] == sutun - 6 && hamleKomut[2] == satir + 6
                                || hamleKomut[1] == sutun - 7 && hamleKomut[2] == satir + 7) { if (aradaTasVarMi("Sol-Yukarı") == false) return true; } // Sol-Yukarı arada taş yoksa
                        else if (hamleKomut[1] == sutun - 1 && hamleKomut[2] == satir - 1 || hamleKomut[1] == sutun - 2 && hamleKomut[2] == satir - 2 || hamleKomut[1] == sutun - 3 && hamleKomut[2] == satir - 3
                                || hamleKomut[1] == sutun - 4 && hamleKomut[2] == satir - 4 || hamleKomut[1] == sutun - 5 && hamleKomut[2] == satir - 5 || hamleKomut[1] == sutun - 6 && hamleKomut[2] == satir - 6
                                || hamleKomut[1] == sutun - 7 && hamleKomut[2] == satir - 7) { if (aradaTasVarMi("Sol-Aşağı") == false) return true; } // Sol-Aşağı arada taş yoksa
                    break;
            }

            return false;
        }

        public bool aradaTasVarMi(string yon)
        {
            string[] ayir = yon.Split('-');
            List<string> yonler = new List<string>();

            if (ayir.Count() == 1) { yonler.Add(ayir[0]); yonler.Add(""); }
            else if (ayir.Count() == 2) { yonler.Add(ayir[0]); yonler.Add(ayir[1]); }

            List<char> satirlar = new List<char>();
            satirlar.Add(satir);
            List<char> sutunlar = new List<char>();
            sutunlar.Add(sutun);

            var notasyonDefteri = db.satrancTahtalari.Where(x => x.ID == odaNo).FirstOrDefault().hamleler.OrderByDescending(u => u.ID);

            if (yonler[0] == "Yukarı" || yonler[1] == "Yukarı") for (char n = (char)(satir + 1); n <= (char)(hamleKomut[2] - 1); n++) satirlar.Add(n);
            else if (yonler[0] == "Aşağı" || yonler[1] == "Aşağı") for (char n = (char)(satir - 1); n >= (char)(hamleKomut[2] + 1); n--) satirlar.Add(n);
            if (yonler[0] == "Sağ") for (char n = (char)(sutun + 1); n <= (char)(hamleKomut[1] - 1); n++) sutunlar.Add(n);
            else if (yonler[0] == "Sol") for (char n = (char)(sutun - 1); n >= (char)(hamleKomut[1] + 1); n--) sutunlar.Add(n);


            foreach (var aralik1 in satirlar)
                foreach (var aralik2 in sutunlar)
                {
                    if (aralik1 == satirlar.ElementAt(0) && aralik2 == sutunlar.ElementAt(0)) continue;
                    var varMi = notasyonDefteri.Where(y => y.notasyon.ToCharArray()[y.notasyon.Length - 1] == aralik1 && y.notasyon.ToCharArray()[y.notasyon.Length - 2] == aralik2).FirstOrDefault();
                    var halaOrdaMi = notasyonDefteri.Where(y => y.pozisyon.ToCharArray()[y.pozisyon.Length - 1] == aralik1 && y.pozisyon.ToCharArray()[y.pozisyon.Length - 2] == aralik2).FirstOrDefault();
                    if (varMi != null)
                    {
                        if (halaOrdaMi != null)
                            if (halaOrdaMi.ID > varMi.ID) return true;
                    }
                    else continue;
                }

            return false;
        }

        public void notasyonGonder()
        {
            var hamle = db.hamleler.Add(new hamleler { notasyon = notasyon, pozisyon = pozisyon, odaNo = odaNo });
            int puan = 0;

            if (yenenTas != "-")
                if (yenenTas == "Ş") puan = 99;
                else if (yenenTas == "V") puan = 9;
                else if (yenenTas == "F") puan = 3;
                else if (yenenTas == "A") puan = 3;
                else if (yenenTas == "K") puan = 5;
                else puan = 1;

                if (tasRenk == true)
                {
                    db.satrancTahtalari.Find(odaNo).kullanicilar.ELO += puan;
                    db.satrancTahtalari.Find(odaNo).kullanicilar1.ELO -= puan;
                }
                else if (tasRenk == false)
                {
                    db.satrancTahtalari.Find(odaNo).kullanicilar.ELO -= puan;
                    db.satrancTahtalari.Find(odaNo).kullanicilar1.ELO += puan;
                }

            if (yenenTas == "Ş") // Şah yeniyorsa
            {
                db.hamleler.RemoveRange(db.hamleler.Where(w => w.odaNo == odaNo)); // Hamleleri sil
                db.satrancTahtalari.Remove(db.satrancTahtalari.Where(w => w.ID == odaNo).FirstOrDefault()); // Odayı sil.
            }

            db.SaveChanges();
        }

        public object notasyonGetir()
        {
            string notasyon = "", pozisyon = "", yeniHucre = "";
            char tasCik = '-';
            bool sahCek = false;

                var hamle = db.hamleler.Where(s => s.odaNo == odaNo).OrderByDescending(s => s.ID).FirstOrDefault();
                notasyon = hamle.notasyon;
                pozisyon = hamle.pozisyon;

                    if (hamle == null) return 0;

                char[] notasyonParca = notasyon.ToCharArray();

                // NOTASYON KONTROL BAŞLANGICI = TAŞIN ESKİ HÜCRESİ => TAŞIN YENİ HÜCRESİ //
                if (notasyonParca[0] == 'Ş' || notasyonParca[0] == 'V' || notasyonParca[0] == 'F' || notasyonParca[0] == 'A' || notasyonParca[0] == 'K') // Şah, Vezir, Fil, At veya Kale;
                    if (notasyonParca[1] == 'x' || notasyon.ToCharArray()[1] == '+')
                    {
                        yeniHucre = (notasyonParca[2] + "" + notasyonParca[3]);   // Bir taşı yiyor veya şah çekiyorsa yeniPozisyonu
                        if (notasyonParca[1] == '+') sahCek = true;
                    }
                    else yeniHucre = (notasyonParca[1] + "" + notasyonParca[2]);       // Yer değiştiriyorsa yeniPozisyonu
                else        // Taş piyon ise;    
                    if (notasyonParca.Count() >= 3 && (notasyonParca[2] == 'x' || notasyon.ToCharArray()[2] == '+'))
                {
                    yeniHucre = (notasyonParca[3] + "" + notasyonParca[4]);       // Bir taşı yiyor veya şah çekiyorsa yeniPozisyonu
                    if (notasyonParca[2] == '+') sahCek = true;
                }
                else
                {
                    yeniHucre = (notasyonParca[0] + "" + notasyonParca[1]);       // Yer değiştiriyorsa yeniPozisyonu
                    if (notasyonParca.Count() >= 3 && (notasyonParca[2] == 'V' || notasyonParca[2] == 'K')) tasCik = notasyonParca[2];
                }    // Piyon Vezir veya Kale taşı çıkıyorsa
                     // NOTASYON KONTROL BİTİŞİ = TAŞIN ESKİ HÜCRESİ => TAŞIN YENİ HÜCRESİ //

                var hamleBilgileri = new
                {
                    eskiPozisyon = pozisyon,
                    yeniPozisyon = yeniHucre,
                    tasCikma = tasCik,
                    sahCekme = sahCek
                };

                return hamleBilgileri;

        }
            
    }
}