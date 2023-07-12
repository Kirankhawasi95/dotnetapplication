using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.InteropServices;
using System.Data;
using System.IO;
using System.Data.OleDb;
using System.Net;
using System.Configuration;
using Oracle.ManagedDataAccess.Client;
using System.Globalization;

namespace ShutDownDetails
{
    public class Processing
    {
        Log objlog;
        OracleConnection con= StaticConnection.getconnection();
        OracleTransaction oratran;
     

        public DataTable createdatatablefromexcel(string filepath)
        {
            var ds = new DataSet();
            try
            {
                bool readfileio = false;
                //string content = System.IO.File.ReadAllText("e://Shut Down Details 31 July 2016.xlsx");
                var connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filepath + ";Extended Properties=\"Excel 12.0;IMEX=1;HDR=NO;TypeGuessRows=0;ImportMixedTypes=Text\"";
                using (var conn = new OleDbConnection(connectionString))
                {
                    try
                    {
                        conn.Open();
                    }
                    catch (Exception ex)
                    {
                        if (ex.Message.Contains("External table is not in the expected format"))
                        {
                            readfileio = true;
                        }
                        objlog.writelog("In Processing.getdatafromexcelandinsert.excelreading " + ex.Message);
                    }
                    if (readfileio == true)
                    {

                       // readiofile(content, filename, bankname);
                    }
                    else
                    {
                        var sheets = conn.GetOleDbSchemaTable(System.Data.OleDb.OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });
                        using (var cmd = conn.CreateCommand())
                        {
                            cmd.CommandText = "SELECT * FROM [" + sheets.Rows[0]["TABLE_NAME"].ToString() + "] ";
                            var adapter = new OleDbDataAdapter(cmd);                           
                            adapter.Fill(ds);
                            conn.Close();                           
                            //insertdata(ds, filename, bankname);
                        }
                    }

                }
                return ds.Tables[0];
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public string insertorupdate(DataTable dt, string empno, string siteid,string month)
        {
            string schemaname = ConfigurationManager.AppSettings["schemaname"];
            string result = "false";     
            if (con == null)
                con = StaticConnection.getconnection();
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            oratran = con.BeginTransaction(IsolationLevel.ReadCommitted);
            try
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i][1].ToString() != "" && dt.Rows[i][2].ToString() != "" && dt.Rows[i][3].ToString() != "")
                    {
                        if (dt.Rows[i][3].ToString().Length <= 5 && dt.Rows[i][3].ToString().Contains(":"))
                        {
                            string dateout = "";
                            String datein = "";
                            try
                            {
                                string[] formats = new string[8] { "dd/MM/yyyy", "dd/M/yyyy", "d/MM/yyyy", "dd/MON/yyyy", "dd-MM-yyyy", "dd-M-yyyy", "dd-MM-yyyy","dd-MON-yyyy" };
                                DateTime datecheck = DateTime.ParseExact(dt.Rows[i][1].ToString(), formats, System.Globalization.CultureInfo.InvariantCulture, DateTimeStyles.AllowWhiteSpaces | DateTimeStyles.AssumeUniversal);
                                dateout = datecheck.ToString("dd/MM/yyyy");
                               
                                if (dt.Rows[i][4].ToString().Length > 0)
                                {
                                    datecheck = DateTime.ParseExact(dt.Rows[i][4].ToString(), formats, System.Globalization.CultureInfo.InvariantCulture, DateTimeStyles.AllowWhiteSpaces | DateTimeStyles.AssumeUniversal);
                                    datein = datecheck.ToString("dd/MM/yyyy");
                                }
                            }
                            catch (Exception ex)
                            {
                                result = "false + Time";
                                return result;
                            }
                            //string checkqry = "SELECT COUNT(*) FROM SHUT_DOWN_DETAILS WHERE SITE_ID='" + siteid + "' AND DATE_OIL_OUT=TO_DATE('" + dt.Rows[i][1].ToString() + "','DD/MM/YYYY') AND UNIT='" + dt.Rows[i][2].ToString() + "' AND SHUTDOWN='" + dt.Rows[i][3].ToString() + "'";
                            //bool present = checkprsent(checkqry);
                            string qry = "";
                            //if (present == true)
                            // {
                            qry = "INSERT INTO " + schemaname + ".SHUT_DOWN_DETAILS(DATE_OIL_OUT,UNIT,SHUTDOWN,DATE_OIL_IN,START_UP,DURATION,REASONS,SITE_ID,EMPLOYEE_ID,DATE_MODIFIED,MONTH) VALUES (TO_DATE('" + dateout + "','dd/mm/yyyy'),'" + dt.Rows[i][2].ToString() + "','" + dt.Rows[i][3].ToString() + "',TO_DATE('" + datein + "','dd/mm/yyyy'),'" + dt.Rows[i][5].ToString() + "','" + dt.Rows[i][6].ToString() + "','" + dt.Rows[i][7].ToString() + "','" + siteid + "','" + empno + "',TO_DATE('" + DateTime.Now.ToString("dd/MM/yyyy") + "','dd/mm/yyyy'),'" + month + "')";
                            //}
                            //else
                            //{
                            //    result = "false";
                            //    break;
                            //    //qry = "UPDATE SHUT_DOWN_DETAILS SET DATE_OIL_IN=TO_DATE('" + dt.Rows[i][4].ToString() + "','dd/mm/yyyy'),START_UP='" + dt.Rows[i][5].ToString() + "',DURATION='" + dt.Rows[i][6].ToString() + "',REASONS='" + dt.Rows[i][7].ToString() + "',DATE_MODIFIED=TO_DATE('" + DateTime.Now.ToString("dd/MM/yyyy") + "','dd/mm/yyyy'),EMPLOYEE_ID=" + empno + " WHERE DATE_OIL_OUT=TO_DATE('" + dt.Rows[i][1].ToString() + "','dd/mm/yyyy') AND UNIT='" + dt.Rows[i][2].ToString() + "' AND SHUTDOWN='" + dt.Rows[i][3].ToString() + "' AND SITE_ID='" + siteid + "'";
                            //}
                            if (!insertupdateqry(qry))
                            {
                                result = "false";
                                break;
                            }
                        }
                        else
                        {                            
                            result = "false + Time";                          
                            return result;
                        }
                    }
                    else
                    {
                        result = "false + Blank";                       
                        return result;
                       
                    }
                }

                oratran.Commit();
                con.Close();
                result = "true";
            }
            catch (Exception ex)
            {
                if (con.State == ConnectionState.Open)
                {
                    oratran.Rollback();
                }
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
            return result;
        }

        public bool checkprsent(string qry)
        {
            bool result = true;            
            try
            {
                OracleDataAdapter adpt = new OracleDataAdapter(qry, con);
                DataSet ds = new DataSet();
                adpt.Fill(ds);
                if (Convert.ToInt32(ds.Tables[0].Rows[0][0]) > 0)
                {
                    result = false;
                }
            }
            catch (Exception ex)
            {

            }         
            return result;
        }

        public DataSet selectquery(string qry)
        {
            DataSet result = new DataSet();
            if (con == null)
                con = StaticConnection.getconnection();
            try
            {
                OracleDataAdapter adpt = new OracleDataAdapter(qry, con);
                adpt.Fill(result);
               
            }
            catch (Exception ex)
            {

            }
            finally
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
            return result;
        }

        public bool insertupdateqry(string qry)
        {
            bool result = false;
            if (con == null)
                con = StaticConnection.getconnection();
            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                OracleCommand cmd = new OracleCommand(qry, con);
                cmd.Transaction =  oratran;    
                cmd.ExecuteNonQuery();               
                result = true;              
            }
            catch (Exception ex)
            {
                oratran.Rollback();
            }            
            return result;
        }

        public bool delete(string qry)
        {
            bool result = false;
            if (con == null)
                con = StaticConnection.getconnection();
            try
            {
                OracleCommand cmd = new OracleCommand(qry, con);                
                cmd.ExecuteNonQuery();
                result = true;
            }
            catch (Exception ex)
            {
                oratran.Rollback();
            }
            return result;
        }
    }
}