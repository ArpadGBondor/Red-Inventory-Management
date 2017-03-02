using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlServerCe;
using System.Windows;

namespace Red_software
{
    class Database
    {
    

        public static void Test()
        {
            string connectionstring;
            string filename = "database.sdf";
            string password = "Red_software";


            connectionstring = string.Format("datasource = \"{0}\"; password = '{1}'", filename, password);

            if (!File.Exists(filename))
            {
                SqlCeEngine en = new SqlCeEngine(connectionstring);

                en.CreateDatabase();
            }

            SqlCeConnection conn = new SqlCeConnection(connectionstring);
            conn.Open();
            SqlCeTransaction tx = conn.BeginTransaction(System.Data.IsolationLevel.ReadCommitted);
            SqlCeCommand cmd2 = new SqlCeCommand("create table elso(elso int, masodik int);", conn, tx);
            SqlCeDataReader rdr2 = cmd2.ExecuteReader();
            SqlCeCommand cmd1 = new SqlCeCommand("insert into elso values(66,99);", conn, tx);
            SqlCeDataReader rdr1 = cmd1.ExecuteReader();
            SqlCeCommand cmd = new SqlCeCommand("select * from elso;", conn, tx);
            SqlCeDataReader rdr = cmd.ExecuteReader();

            try
            {
                // iterate through the results
                //
                while (rdr.Read())
                {
                    int val1 = rdr.GetInt32(0);
                    int val2 = rdr.GetInt32(1);
                    MessageBox.Show(string.Format("{0}, {1}", val1, val2), "confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);


                }
            }
            finally
            {
                // always call close when done reading
                //
                rdr.Close();

                // always call close when done reading
                //
                conn.Close();
            }
        }
    }
}
