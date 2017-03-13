using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer;

namespace BusinessLayer
{
    public class DatabaseConnection
    {
        public static bool TestConnection()
        {
            return Database.Test();
        }
        public static void ChangeDatabaseFile(string filename)
        {
            Database.InitializeConnection(filename);
        }
    }
}
