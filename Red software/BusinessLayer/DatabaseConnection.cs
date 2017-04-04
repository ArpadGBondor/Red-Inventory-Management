using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer;
using System.IO;

namespace BusinessLayer
{
    public class DatabaseConnection
    {
        public static string Directory { get { return Database.Directory; } }
        public static string DbName { get { return Database.DbName; } }

        private static string BaseDir
        {
            get
            {
                string baseDir = AppDomain.CurrentDomain.BaseDirectory;
                if (!baseDir.EndsWith("\\"))
                    baseDir += "\\";
                return baseDir;
            }
        }
        private static string DbSettingFile { get { return BaseDir + "DatabaseSettings.txt"; } }

        private static void ReadDbSettings(ref string directory, ref string dbname)
        {
            //Open the stream and read it back.
            using (StreamReader sr = new StreamReader(DbSettingFile))
            {
                string line = "";
                if ((line = sr.ReadLine()) != null)
                {
                    directory = line;
                }
                if ((line = sr.ReadLine()) != null)
                {
                    dbname = line;
                }
            }
        }

        private static void WriteDbSettings(string directory, string dbname)
        {
            // Delete the file if it exists.
            if (File.Exists(DbSettingFile))
            {
                File.Delete(DbSettingFile);
            }
            //Create the file.
            using (StreamWriter sw = new StreamWriter(DbSettingFile))
            {
                sw.WriteLine(directory);
                sw.WriteLine(dbname);
            }
        }

        public static bool TestConnection()
        {
            if (string.IsNullOrWhiteSpace(Directory) || string.IsNullOrWhiteSpace(DbName))
            {
                string dirName = BaseDir;
                string dbName = "Database";
                if (File.Exists(DbSettingFile))
                    ReadDbSettings(ref dirName, ref dbName);
                ChangeDatabase(dirName, dbName);
            }
            return Database.Test();
        }
        public static bool ChangeDatabase(string directory, string database)
        {
            if (Database.InitializeConnection(directory, database))
            {
                WriteDbSettings(directory, database);
                return true;
            }
            return false;
        }

        public static bool CreateDatabase(string directory, string database)
        {
            if (Database.CreateDatabase(directory, database))
            {
                WriteDbSettings(directory, database);
                return true;
            }
            return false;
        }


    }
}
