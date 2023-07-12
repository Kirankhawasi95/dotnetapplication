using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
using System.Data;
using System.Web.Security;
using System.Data.SqlClient;
using System.DirectoryServices;
using Oracle.ManagedDataAccess.Client;
using System.Security.Cryptography;
using System.IO;
using System.Text;
using System.Net;
using System.Web.Configuration;
using System.Collections.Specialized;
using com.toml.dp.util;

namespace ShutDownDetails
{
    public partial class login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
     
            redirecttoCA();
        }

        private void redirecttoCA()
        {
            string globalkey = WebConfigurationManager.AppSettings["GLOBAL_KEY"];
            string appkey = WebConfigurationManager.AppSettings["APP_KEY"]; ;
            string appid = WebConfigurationManager.AppSettings["APPID"]; ;
            string caurl = WebConfigurationManager.AppSettings["login_link"];           


            NameValueCollection collections = new NameValueCollection();
            collections.Add("appid", AES128Bit.Encrypt(appid, globalkey, 128));
            collections.Add("version", AES128Bit.Encrypt("1.0", appkey, 128));
            collections.Add("reqTime", AES128Bit.Encrypt(DateTime.Now.ToString("yyyyMMddHHmmss"), appkey, 128));
            string remoteUrl = caurl;

            string html = "<html><head>";
            html += "</head><body onload='document.forms[0].submit()'>";
            html += string.Format("<form name='PostForm' method='POST' action='{0}'>", remoteUrl);
            foreach (string key in collections.Keys)
            {
                html += string.Format("<input name='{0}' type='text' value='{1}'>", key, collections[key]);
            }
            html += "</form></body></html>";
            Response.Clear();
            Response.ContentEncoding = Encoding.GetEncoding("ISO-8859-1");
            Response.HeaderEncoding = Encoding.GetEncoding("ISO-8859-1");
            Response.Charset = "ISO-8859-1";
            Response.Write(html);
            Response.End();
        }        

        protected void btnlogin_Click(object sender, EventArgs e)
        {
            try
            {
                string @empno = txtuserid.Text;
                string @pass = txtpassword.Text;

                if (@empno != "" && @pass != "")
                {
                    if (txtuserid.Text.Length < 8)
                    {
                        lbstatus.Text = "Please enter Valid Employee Number";
                        return;
                    }
                    else
                    {
                        if (txtuserid.Text[0] != '3')
                        {
                            lbstatus.Text = "Please enter Valid Employee Number";
                            return;
                        }
                        if (txtuserid.Text[7] != '0')
                        {
                            lbstatus.Text = "Please enter Valid Employee Number";
                            return;
                        }
                    }
                    if (IsAuthenticated(@empno, @pass) == true)
                    {
                        //objlog.writelog("User " + @empno + " login success");
                        string empid = @empno.Substring(1, 6);
                        string emplname = empname(empid);
                        if (emplname.Trim() == "")
                        {
                            emplname = @empno;
                        }
                        else
                            emplname = emplname.Trim();

                        Session["username"] = emplname;

                        Session["user"] = txtuserid.Text;

                        string role = "A";
                        if (role == "A")
                        {
                            //FormsAuthentication.SetAuthCookie(txtuserid.Text, false);                           
                            Server.Transfer("Upload.aspx", false);
                            //Response.Redirect("~/MROperations.aspx");
                        }

                    }
                    else
                    {
                        lbstatus.Text = "Login Failed Please enter valid Employee Number and ADS Password";
                        txtpassword.Text = "";
                        txtpassword.Focus();
                    }

                }
                else
                {
                    if (@empno == "" && @pass == "")
                    {
                        lbstatus.Text = "Please enter Employee Number and password";
                    }
                    else if (@empno == "")
                    {
                        lbstatus.Text = "Please enter Employee Number";
                    }
                    else if (@pass == "")
                    {
                        lbstatus.Text = "Please enter the Password";
                    }
                }

            }
            catch (Exception ex)
            {

            }
        }


        public bool IsAuthenticated(string usr, string pwd)
        {
            bool authenticated = false;
            try
            {
                DirectoryEntry entry = new DirectoryEntry(@"LDAP://hpcl.co.in:389", usr, pwd);
                object nativeObject = entry.NativeObject;
                authenticated = true;
            }
            catch (DirectoryServicesCOMException cex)
            {
                //objlog.writelog("In Login.btnlogin_Click : " + cex.Message);
            }
            catch (Exception ex)
            {
                //objlog.writelog("In Login.btnlogin_Click : " + ex.Message);
            }
            return authenticated;
        }

        public string empname(string id)
        {
            string constr = ConfigurationManager.ConnectionStrings["oracleconstr"].ConnectionString;

            string empname = "";
            try
            {
                string qry = "SELECT EMP_NAME FROM EMP_MASTER WHERE EMP_NO=:empno";
                using (OracleConnection con = new OracleConnection(constr))
                {
                    using (OracleCommand cmd = con.CreateCommand())
                    {
                        cmd.CommandText = qry;
                        cmd.Parameters.Add(":empno", id);
                        con.Open();
                        empname = cmd.ExecuteScalar().ToString();
                        con.Close();
                    }
                }

            }

            catch (Exception ex)
            {

            }
            return empname;
        }

    }
}