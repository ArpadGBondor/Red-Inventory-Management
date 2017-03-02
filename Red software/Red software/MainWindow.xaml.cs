using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.SqlServerCe;


namespace Red_software
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public class testBase
        {
            public virtual void test(int i = 15)
            {
                MessageBox.Show(string.Format("testBase.test: {0}", i), "confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
            }
        }
        public class testDerived : testBase
        {
            public override void test(int i = 20)
            {
                MessageBox.Show(string.Format("testDerived.test: {0}", i), "confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
            }
        }
        public MainWindow()
        {

            testBase T = new testDerived();

            T.test();

            //string connectionstring;
            //string filename = "database.sdf";
            //string password = "red_software";


            //connectionstring = string.format("datasource = \"{0}\"; password = '{1}'", filename, password);

            //if (!file.exists(filename))
            //{
            //    sqlceengine en = new sqlceengine(connectionstring);

            //    en.createdatabase();
            //}

            //sqlceconnection conn = new sqlceconnection(connectionstring);
            //conn.open();

            //sqlcetransaction tx = conn.begintransaction(system.data.isolationlevel.readcommitted);

            //sqlcecommand cmd2 = new sqlcecommand("create table elso(elso int, masodik int);", conn, tx);
            //sqlcedatareader rdr2 = cmd2.executereader();
            //sqlcecommand cmd1 = new sqlcecommand("insert into elso values(66,99);", conn, tx);
            //sqlcedatareader rdr1 = cmd1.executereader();
            //sqlcecommand cmd = new sqlcecommand("select * from elso;", conn, tx);
            //sqlcedatareader rdr = cmd.executereader();

            //try
            //{
            //    // iterate through the results
            //    //
            //    while (rdr.read())
            //    {
            //        int val1 = rdr.getint32(0);
            //        int val2 = rdr.getint32(1);
            //        // string val2 = rdr.getstring(1);

            //        messagebox.show(string.format("{0}, {1}", val1, val2), "confirmation", messageboxbutton.yesno, messageboximage.question);


            //    }
            //}
            //finally
            //{
            //    // always call close when done reading
            //    //
            //    rdr.close();

            //    // always call close when done reading
            //    //
            //    conn.close();
            //}

            InitializeComponent();
        }
    }
}
