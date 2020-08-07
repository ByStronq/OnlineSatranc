using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OnlineSatranc.Models.EntityFramework;

namespace OnlineSatranc.Models
{
    public class cookieDbUser
    {
        private int userID = 0;

        public kullanicilar getDb()
        {
            var user = HttpContext.Current.User.Identity;

                if(user.IsAuthenticated)    Int32.TryParse(user.Name, out userID);

            OnlineSatrancEntities db = new OnlineSatrancEntities();
            var kullanici = db.kullanicilar.Where(s => s.ID == userID).FirstOrDefault();

            return kullanici;
        }
    }
}