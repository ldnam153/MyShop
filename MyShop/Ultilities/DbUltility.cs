
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MyShop.Models;
namespace MyShop.Ultilities
{
    public class DbUltility
    {
        public static DbUltility _instance = null;
        public static DbUltility Instance {
            get {
                if (_instance == null)
                    _instance = new DbUltility();
                return _instance;
            }
        }
        public ProjectMyShopContext Db;
        private DbUltility()
        {
            Db = new ProjectMyShopContext();
        }

    }
}
