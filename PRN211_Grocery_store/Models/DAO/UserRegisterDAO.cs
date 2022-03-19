using PRN211_Grocery_store.Data;
using PRN211_Grocery_store.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PRN211_Grocery_store.Models.DAO
{
    public class UserRegisterDAO
    {
        private static UserRegisterDAO instance = null;
        private static readonly object instanceLock = new object();
        public static UserRegisterDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null) instance = new UserRegisterDAO();
                    return instance;
                }
            }

        }
        public User CheckUserDuplicate(string email)
        {
            User user = null;
            try
            {
                using (var context = new ApplicationDBContext())
                {
                    user = context.Users.SingleOrDefault((user) => user.Email == email);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return user;
        }
    }
}
