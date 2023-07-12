using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.IO;

namespace ShutDownDetails
{
    public class Log
    {
        public void writelog(string content)
        {
            try
            {
                FileStream fs = null;
                string fileloc = @"Log\" + DateTime.Today.ToString("ddMMyyyy") + ".txt";
                fs = new FileStream(fileloc, FileMode.Append, FileAccess.Write);
                StreamWriter sw = new StreamWriter(fs);
                sw.WriteLine(DateTime.Now.ToString() + "::" + content);
                sw.AutoFlush = true;
                fs.Close();
                fs.Dispose();
            }
            catch (Exception ex)
            {
                //System.Windows.Forms.MessageBox.Show("Cannoty write log : " + ex.Message);
            }
        }
    }
}