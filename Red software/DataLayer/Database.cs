using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Reflection;
using System.Diagnostics;
using System.Data.SqlClient;
using EntityLayer;

namespace DataLayer
{
    public class Database
    {
        private static SqlConnection connection;
        private static string connectionString;
        private static string server;
        //private static string database;
        //private static string uid;
        //private static string password;
        private static string file;
        public static string Get_File
        {
            get
            {
                return file;
            }
        }
        public static SqlConnection get_connection { get { return connection; } }
        public static string get_connectionString { get { return connectionString; } }

        static Database()
        {
            file = AppDomain.CurrentDomain.BaseDirectory + "Database.mdf";
            InitializeConnection();
        }

        public static void InitializeConnection(string _file = "")
        {
            server = "(LocalDB)\\MSSQLLocalDB";
            if (File.Exists(_file))
                file = _file;
            connectionString = "Data Source=" + server + ";AttachDbFilename=\"" + file + "\";Integrated Security=True;";
            //connectionString = "server=localhost;user id=software;database=software";
            connection = new SqlConnection(connectionString);
        }

        //open connection to database
        public static bool OpenConnection()
        {
            try
            {
                connection.Open();
                return true;
            }
            catch /*(SqlException ex)*/
            {
                //When handling errors, you can your application's response based 
                //on the error number.
                //The two most common error numbers when connecting are as follows:
                //0: Cannot connect to server.
                //1045: Invalid user name and/or password.
                //switch (ex.Number)
                //{
                //    //case 1045:
                //    //    MessageBox.Show("Invalid username/password, please try again");
                //    //    break;
                //    //default:
                //    //    MessageBox.Show("Cannot connect to database. \nSelect the Database.mdf file, or contact an administrator."/* + ex.Number.ToString()*/);
                //    //    break;
                //}
                return false;
            }
        }

        //Close connection
        public static bool CloseConnection()
        {
            try
            {
                connection.Close();
                return true;
            }
            catch /*(SqlException ex)*/
            {
                return false;
            }
        }


        public static bool Test()
        {
            if (OpenConnection())
            {
                CloseConnection();
                return true;
            }
            else
            {
                return false;
            }
        }


        public static void CreateTable(Type linqTableClass)
        {
            DataContext tempDc = new DataContext(connectionString);
            var metaTable = tempDc.Mapping.GetTable(linqTableClass);
            var typeName = "System.Data.Linq.SqlClient.SqlBuilder";
            var type = typeof(DataContext).Assembly.GetType(typeName);
            var bf = BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.InvokeMethod;
            var sql = type.InvokeMember("GetCreateTableCommand", bf, null, null, new[] { metaTable });
            var sqlAsString = sql.ToString();
            tempDc.ExecuteCommand(sqlAsString);
        }

        public static bool TableExists(Type linqTableClass)
        {        
            try
            {
                DataContext tempDc = new DataContext(connectionString);
                SqlTransaction tx = connection.BeginTransaction(System.Data.IsolationLevel.ReadCommitted);
                SqlCommand cmd = new SqlCommand("select case when exists((select * from information_schema.tables where table_name = '" + tempDc.Mapping.GetTable(linqTableClass).TableName + "')) then 1 else 0 end", connection, tx);               
                SqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.Read())
                {
                    return rdr.GetInt32(0) == 1;
                }
                else
                    return false;
            }
            catch /*(Exception e)*/
            {
                return false;
            }
        }

        public static void InitializeTable(Type linqTableClass)
        {
            if (OpenConnection())
            {
                if (!TableExists(linqTableClass))
                {
                    CreateTable(linqTableClass);
                }
                CloseConnection();
            }
            // TODO: automatic
        }


        //
        ////CreateTable(typeof(Customer), db);

        //Table<Customer> Customers = db.GetTable<Customer>();

        //Customers.InsertOnSubmit(new Customer("ID007", "London"));
        //// Submit the change to the database.
        //try
        //{
        //    db.SubmitChanges();
        //}
        //catch (Exception e)
        //{
        //    MessageBox.Show(e.ToString());
        //}


        //// Query for customers from London
        //var q =
        //   from c in Customers
        //   where c.City == "London"
        //   select c;
        //foreach (var cust in q)
        //    MessageBox.Show(String.Format("id = {0}, City = {1}", cust.CustomerID, cust.City));




        //    string connectionstring = "server=localhost:3306; uid=" + '"' + "root" + '"' + "; pwd=" + '"' + "Red_software" + '"' + ";";


        //    SqlConnection conn = new SqlConnection(connectionstring);
        //    conn.Open();
        //    SqlTransaction tx = conn.BeginTransaction(System.Data.IsolationLevel.ReadCommitted);
        //    SqlCommand cmd2 = new SqlCommand("create table elso(elso int, masodik int);", conn, tx);
        //    SqlDataReader rdr2 = cmd2.ExecuteReader();
        //    SqlCommand cmd1 = new SqlCommand("insert into elso values(66,99);", conn, tx);
        //    SqlDataReader rdr1 = cmd1.ExecuteReader();
        //    SqlCommand cmd = new SqlCommand("select * from elso;", conn, tx);
        //    SqlDataReader rdr = cmd.ExecuteReader();

        //    try
        //    {
        //        // iterate through the results
        //        //
        //        while (rdr.Read())
        //        {
        //            int val1 = rdr.GetInt32(0);
        //            int val2 = rdr.GetInt32(1);
        //            MessageBox.Show(string.Format("{0}, {1}", val1, val2), "confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);


        //        }
        //    }
        //    finally
        //    {
        //        // always call close when done reading
        //        //
        //        rdr.Close();

        //        // always call close when done reading
        //        //
        //        conn.Close();
        //    }




        //public void Test()
        //{
        //    // DataContext takes a connection string 
        //    //if (!db.DatabaseExists())
        //    //    db.CreateDatabase();
        //    // Get a typed table to run queries


        //}



        //public void Test()
        //{
        //    if (OpenConnection())
        //    {

        //        //DataContext db = new DataContext(connectionString);
        //        ////CreateTable(typeof(Customer), db);

        //        //Table<Customer> Customers = db.GetTable<Customer>();

        //        //Customers.InsertOnSubmit(new Customer("ID007", "London"));
        //        //// Submit the change to the database.
        //        //try
        //        //{
        //        //    db.SubmitChanges();
        //        //}
        //        //catch (Exception e)
        //        //{
        //        //    MessageBox.Show(e.ToString());
        //        //}


        //        //// Query for customers from London
        //        //var q =
        //        //   from c in Customers
        //        //   where c.City == "London"
        //        //   select c;
        //        //foreach (var cust in q)
        //        //    MessageBox.Show(String.Format("id = {0}, City = {1}", cust.CustomerID, cust.City));




        //        CloseConnection();
        //    }
        //    //    string connectionstring = "server=localhost:3306; uid=" + '"' + "root" + '"' + "; pwd=" + '"' + "Red_software" + '"' + ";";


        //    //    SqlConnection conn = new SqlConnection(connectionstring);
        //    //    conn.Open();
        //    //    SqlTransaction tx = conn.BeginTransaction(System.Data.IsolationLevel.ReadCommitted);
        //    //    SqlCommand cmd2 = new SqlCommand("create table elso(elso int, masodik int);", conn, tx);
        //    //    SqlDataReader rdr2 = cmd2.ExecuteReader();
        //    //    SqlCommand cmd1 = new SqlCommand("insert into elso values(66,99);", conn, tx);
        //    //    SqlDataReader rdr1 = cmd1.ExecuteReader();
        //    //    SqlCommand cmd = new SqlCommand("select * from elso;", conn, tx);
        //    //    SqlDataReader rdr = cmd.ExecuteReader();

        //    //    try
        //    //    {
        //    //        // iterate through the results
        //    //        //
        //    //        while (rdr.Read())
        //    //        {
        //    //            int val1 = rdr.GetInt32(0);
        //    //            int val2 = rdr.GetInt32(1);
        //    //            MessageBox.Show(string.Format("{0}, {1}", val1, val2), "confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);


        //    //        }
        //    //    }
        //    //    finally
        //    //    {
        //    //        // always call close when done reading
        //    //        //
        //    //        rdr.Close();

        //    //        // always call close when done reading
        //    //        //
        //    //        conn.Close();
        //    //    }
        //}


        ///// <summary>
        ///// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///// </summary>
        ///// <returns></returns>


        ////Insert statement
        //public void Insert()
        //{
        //    string query = "INSERT INTO tableinfo (name, age) VALUES('John Smith', '33')";

        //    //open connection
        //    if (this.OpenConnection() == true)
        //    {
        //        //create command and assign the query and connection from the constructor
        //        MySqlCommand cmd = new MySqlCommand(query, connection);

        //        //Execute command
        //        cmd.ExecuteNonQuery();

        //        //close connection
        //        this.CloseConnection();
        //    }
        //}

        ////Update statement
        //public void Update()
        //{
        //    string query = "UPDATE tableinfo SET name='Joe', age='22' WHERE name='John Smith'";

        //    //Open connection
        //    if (this.OpenConnection() == true)
        //    {
        //        //create mysql command
        //        MySqlCommand cmd = new MySqlCommand();
        //        //Assign the query using CommandText
        //        cmd.CommandText = query;
        //        //Assign the connection using Connection
        //        cmd.Connection = connection;

        //        //Execute query
        //        cmd.ExecuteNonQuery();

        //        //close connection
        //        this.CloseConnection();
        //    }
        //}

        ////Delete statement
        //public void Delete()
        //{
        //    string query = "DELETE FROM tableinfo WHERE name='John Smith'";

        //    if (this.OpenConnection() == true)
        //    {
        //        MySqlCommand cmd = new MySqlCommand(query, connection);
        //        cmd.ExecuteNonQuery();
        //        this.CloseConnection();
        //    }
        //}

        ////Select statement
        //public List<string>[] Select()
        //{
        //    string query = "SELECT * FROM tableinfo";

        //    //Create a list to store the result
        //    List<string>[] list = new List<string>[3];
        //    list[0] = new List<string>();
        //    list[1] = new List<string>();
        //    list[2] = new List<string>();

        //    //Open connection
        //    if (this.OpenConnection() == true)
        //    {
        //        //Create Command
        //        MySqlCommand cmd = new MySqlCommand(query, connection);
        //        //Create a data reader and Execute the command
        //        MySqlDataReader dataReader = cmd.ExecuteReader();

        //        //Read the data and store them in the list
        //        while (dataReader.Read())
        //        {
        //            list[0].Add(dataReader["id"] + "");
        //            list[1].Add(dataReader["name"] + "");
        //            list[2].Add(dataReader["age"] + "");
        //        }

        //        //close Data Reader
        //        dataReader.Close();

        //        //close Connection
        //        this.CloseConnection();

        //        //return list to be displayed
        //        return list;
        //    }
        //    else
        //    {
        //        return list;
        //    }
        //}

        ////Count statement
        //public int Count()
        //{
        //    string query = "SELECT Count(*) FROM tableinfo";
        //    int Count = -1;

        //    //Open Connection
        //    if (this.OpenConnection() == true)
        //    {
        //        //Create Mysql Command
        //        MySqlCommand cmd = new MySqlCommand(query, connection);

        //        //ExecuteScalar will return one value
        //        Count = int.Parse(cmd.ExecuteScalar() + "");

        //        //close Connection
        //        this.CloseConnection();

        //        return Count;
        //    }
        //    else
        //    {
        //        return Count;
        //    }
        //}

        ////Backup
        //public void Backup()
        //{
        //    try
        //    {
        //        DateTime Time = DateTime.Now;
        //        int year = Time.Year;
        //        int month = Time.Month;
        //        int day = Time.Day;
        //        int hour = Time.Hour;
        //        int minute = Time.Minute;
        //        int second = Time.Second;
        //        int millisecond = Time.Millisecond;

        //        //Save file to C:\ with the current date as a filename
        //        string path;
        //        path = "C:\\MySqlBackup" + year + "-" + month + "-" + day +
        //    "-" + hour + "-" + minute + "-" + second + "-" + millisecond + ".sql";
        //        StreamWriter file = new StreamWriter(path);


        //        ProcessStartInfo psi = new ProcessStartInfo();
        //        psi.FileName = "mysqldump";
        //        psi.RedirectStandardInput = false;
        //        psi.RedirectStandardOutput = true;
        //        psi.Arguments = string.Format(@"-u{0} -p{1} -h{2} {3}",
        //            uid, password, server, database);
        //        psi.UseShellExecute = false;

        //        Process process = Process.Start(psi);

        //        string output;
        //        output = process.StandardOutput.ReadToEnd();
        //        file.WriteLine(output);
        //        process.WaitForExit();
        //        file.Close();
        //        process.Close();
        //    }
        //    catch (IOException ex)
        //    {
        //        MessageBox.Show("Error , unable to backup!");
        //    }
        //}

        ////Restore
        //public void Restore()
        //{
        //    try
        //    {
        //        //Read file from C:\
        //        string path;
        //        path = "C:\\MySqlBackup.sql";
        //        StreamReader file = new StreamReader(path);
        //        string input = file.ReadToEnd();
        //        file.Close();

        //        ProcessStartInfo psi = new ProcessStartInfo();
        //        psi.FileName = "mysql";
        //        psi.RedirectStandardInput = true;
        //        psi.RedirectStandardOutput = false;
        //        psi.Arguments = string.Format(@"-u{0} -p{1} -h{2} {3}",
        //            uid, password, server, database);
        //        psi.UseShellExecute = false;


        //        Process process = Process.Start(psi);
        //        process.StandardInput.WriteLine(input);
        //        process.StandardInput.Close();
        //        process.WaitForExit();
        //        process.Close();
        //    }
        //    catch (IOException ex)
        //    {
        //        MessageBox.Show("Error , unable to Restore!");
        //    }
        //}

    }
}
