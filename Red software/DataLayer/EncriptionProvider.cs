using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    class EncriptionProvider
    {
        public static string Encrypt(string original, string salt = "Salt")
        {
            return original + salt;
        }
    }
}
