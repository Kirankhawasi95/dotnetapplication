using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Oracle.ManagedDataAccess.Client;
using System.Configuration;
using System.Globalization;
using System.Web.Configuration;
using com.toml.dp.util;

namespace ShutDownDetails
{
    public partial class Upload : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["user"] == null || Session["username"] == null)
            {
                getValuesfromCA();
            }
            if (!IsPostBack)
            {
                getValuesfromCA();
                filldropdowns();
                displayrecords();
            }
        }

        private void getValuesfromCA()
        {
            if (Request.QueryString["empno"] == null)
            {
                Server.Transfer("logout.aspx", false);
            }
            string appkey = WebConfigurationManager.AppSettings["APP_KEY"];
            string empno = AES128Bit.Decrypt(Request.QueryString["empno"].ToString(), appkey, 128);
            Session["user"] = empno;
            Session["username"] = empname(empno);
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

        public void showblankgrid()
        {
            try
            {
                DataTable dtshow = new DataTable();
                dtshow.Columns.Add("SITE_ID", typeof(String));
                dtshow.Columns.Add("DATE_OIL_OUT", typeof(String));
                dtshow.Columns.Add("UNIT", typeof(String));
                dtshow.Columns.Add("SHUTDOWN", typeof(String));
                dtshow.Columns.Add("DATE_OIL_IN", typeof(String));
                dtshow.Columns.Add("START_UP", typeof(String));
                dtshow.Columns.Add("DURATION", typeof(String));
                dtshow.Columns.Add("REASONS", typeof(String));
                dtshow.Rows.Add(dtshow.NewRow());
                resultgridview.DataSource = dtshow;
                resultgridview.DataBind();
                ViewState["griddt"] = dtshow;
                //int columncount = resultgridview.Rows[0].Cells.Count;
                //resultgridview.Rows[0].Cells.Clear();
                //resultgridview.Rows[0].Cells.Add(new TableCell());                
                //resultgridview.Rows[0].Cells[0].ColumnSpan = columncount;             
                //txtCopied.Text = "";
                //resultgridview.Rows[0].Cells[0].Controls.Add(txtCopied);
                txtCopied.Visible = true;
                btncancel.Visible = true;
                btnreset.Visible = false;
            }
            catch (Exception ex)
            { }
        }

        public void filldropdowns()
        {
            DataTable dtdd = new DataTable();
            dtdd.Columns.Add("MONTH", typeof(String));
            dtdd.Columns.Add("ID", typeof(Int32));
            ddmonth.Items.Clear();

            ddyear.Items.Clear();
            ddmonth.Items.Clear();
            int year = Convert.ToInt32(DateTime.Now.Year);
            List<string> monthstring = new List<string>(new string[] { "Jan", "Feb", "March", "April", "May", "June", "July", "Aug", "Sept", "Oct", "Nov", "Dec" });
            for (int i = year; i > 2000; i--)
            {
                ddyear.Items.Add(i.ToString());
            }
            int month = DateTime.Now.Month;
            for (int i = month; i > 0; i--)
            {
                DataRow dr = dtdd.NewRow();
                dr[0] = monthstring[i - 1];
                dr[1] = i;
                dtdd.Rows.Add(dr);
            }
            ddmonth.DataSource = dtdd;
            ddmonth.DataValueField = "ID";
            ddmonth.DataTextField = "MONTH";
            ddmonth.DataBind();
        }

        protected void btnreset_Click(object sender, EventArgs e)
        {

            if (ddsiteid.SelectedValue == "0" || ddsiteid.SelectedIndex == 0)
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, typeof(string), "alertscript", "alert('Please select the Site ID');", true);
                ddsiteid.Focus();
            }
            else
            {
                string schemaname = ConfigurationManager.AppSettings["schemaname"];
                string confirmValue = Request.Form["confirm_value"];
                if (confirmValue == "Yes")
                {
                    string siteid = ddsiteid.SelectedItem.Text;
                    string mon = ddmonth.SelectedItem.Value;
                    string year = ddyear.SelectedItem.Text;
                    string month = "";
                    if (mon.Length == 1)
                    {
                        month += "0";
                    }
                    month += mon + year;
                    string qry = "DELETE FROM " + schemaname + ".SHUT_DOWN_DETAILS WHERE MONTH='" + month + "' AND SITE_ID='" + siteid + "'";
                    Processing objprocessing = new Processing();
                    objprocessing.insertupdateqry(qry);
                    showblankgrid();
                    resultgridview.Columns[0].Visible = false;
                }
            }

        }

        protected void ddyear_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Convert.ToInt32(ddyear.SelectedItem.Text) == DateTime.Now.Year)
            {
                filldropdowns();
            }
            else
            {
                DataTable dtdd = new DataTable();
                dtdd.Columns.Add("MONTH", typeof(String));
                dtdd.Columns.Add("ID", typeof(Int32));
                ddmonth.Items.Clear();
                List<string> monthstring = new List<string>(new string[] { "Jan", "Feb", "March", "April", "May", "June", "July", "Aug", "Sept", "Oct", "Nov", "Dec" });

                for (int i = 0; i < 12; i++)
                {
                    DataRow dr = dtdd.NewRow();
                    dr[0] = monthstring[i];
                    dr[1] = i + 1;
                    dtdd.Rows.Add(dr);
                }
                ddmonth.DataSource = dtdd;
                ddmonth.DataValueField = "ID";
                ddmonth.DataTextField = "MONTH";
                ddmonth.DataBind();
            }
        }

        public void displayrecords()
        {
            string schemaname = ConfigurationManager.AppSettings["schemaname"];
            resultgridview.Columns[0].Visible = true;
            string mon = ddmonth.SelectedItem.Value;
            string year = ddyear.SelectedItem.Text;
            string month = "";
            if (mon.Length == 1)
            {
                month += "0";
            }
            month += mon + year;
            string query = "";
            if (ddsiteid.SelectedIndex == 0 || ddsiteid.SelectedValue == "0")
            {
                query = @"SELECT SITE_ID,TO_DATE(DATE_OIL_OUT,'DD-MM-YYYY') AS DATE_OIL_OUT,UNIT,SHUTDOWN,TO_DATE(DATE_OIL_IN,'DD-MM-YYYY') AS DATE_OIL_IN,START_UP,DURATION,REASONS
                            FROM " + schemaname + ".SHUT_DOWN_DETAILS WHERE MONTH='" + month + "' ORDER BY SITE_ID,DATE_OIL_OUT,UNIT";
            }
            else
            {
                query = @"SELECT SITE_ID,TO_DATE(DATE_OIL_OUT,'DD-MM-YYYY') AS DATE_OIL_OUT,UNIT,SHUTDOWN,TO_DATE(DATE_OIL_IN,'DD-MM-YYYY') AS DATE_OIL_IN,START_UP,DURATION,REASONS
                            FROM " + schemaname + ".SHUT_DOWN_DETAILS WHERE MONTH='" + month + "' AND SITE_ID='" + ddsiteid.SelectedItem.Text + "' ORDER BY SITE_ID,DATE_OIL_OUT,UNIT";
            }
            Processing objp = new Processing();
            DataSet ds = objp.selectquery(query);
            resultgridview.DataSource = ds.Tables[0];
            resultgridview.DataBind();
            ViewState["griddt"] = ds.Tables[0];

        }

        protected void lnkbtn_Click(object sender, EventArgs e)
        {
            displayrecords();
            resultgridview.Columns[0].Visible = true;
        }

        protected void PasteToGridView(object sender, EventArgs e)
        {
            try
            {
                DataTable dtshow = new DataTable();
                dtshow.Columns.Add("SITE_ID", typeof(String));
                dtshow.Columns.Add("DATE_OIL_OUT", typeof(String));
                dtshow.Columns.Add("UNIT", typeof(String));
                dtshow.Columns.Add("SHUTDOWN", typeof(String));
                dtshow.Columns.Add("DATE_OIL_IN", typeof(String));
                dtshow.Columns.Add("START_UP", typeof(String));
                dtshow.Columns.Add("DURATION", typeof(String));
                dtshow.Columns.Add("REASONS", typeof(String));


                string copiedContent = Request.Form[txtCopied.UniqueID];
                foreach (string row in copiedContent.Split('\n'))
                {
                    if (!string.IsNullOrEmpty(row))
                    {
                        dtshow.Rows.Add();
                        int i = 0;
                        foreach (string cell in row.Split('\t'))
                        {
                            dtshow.Rows[dtshow.Rows.Count - 1][0] = ' ';
                            dtshow.Rows[dtshow.Rows.Count - 1][i + 1] = cell;
                            i++;
                        }
                    }
                }

                DataTable dtfinalshow = new DataTable();
                dtfinalshow.Columns.Add("SITE_ID", typeof(String));
                dtfinalshow.Columns.Add("DATE_OIL_OUT", typeof(String));
                dtfinalshow.Columns.Add("UNIT", typeof(String));
                dtfinalshow.Columns.Add("SHUTDOWN", typeof(String));
                dtfinalshow.Columns.Add("DATE_OIL_IN", typeof(String));
                dtfinalshow.Columns.Add("START_UP", typeof(String));
                dtfinalshow.Columns.Add("DURATION", typeof(String));
                dtfinalshow.Columns.Add("REASONS", typeof(String));

                for (int i = 0; i < dtshow.Rows.Count; i++)
                {
                    DataRow dr = dtfinalshow.NewRow();
                    dr[0] = dtshow.Rows[i][0];
                    string[] formats = new string[8] { "dd/MM/yyyy", "dd/M/yyyy", "d/MM/yyyy", "dd/MON/yyyy", "dd-MM-yyyy", "dd-M-yyyy", "dd-MM-yyyy", "dd-MON-yyyy" };
                    try
                    {
                        DateTime datecheckout = DateTime.ParseExact(dtshow.Rows[i][1].ToString(), formats, System.Globalization.CultureInfo.InvariantCulture, DateTimeStyles.AllowWhiteSpaces | DateTimeStyles.AssumeUniversal);
                    }
                    catch (Exception ex)
                    {

                        txtCopied.Text = "";
                        //txtCopied.Visible = false;
                        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, typeof(string), "alertscript", "alert('Check date and time format date: dd/mm/yyyy time- hh:mm');", true);
                        return;
                    }
                    try
                    {
                        if (dtshow.Rows[i][4].ToString().Length > 0)
                        {
                            DateTime datecheckin = DateTime.ParseExact(dtshow.Rows[i][4].ToString(), formats, System.Globalization.CultureInfo.InvariantCulture, DateTimeStyles.AllowWhiteSpaces | DateTimeStyles.AssumeUniversal);
                        }
                    }
                    catch (Exception ex)
                    {
                        txtCopied.Text = "";
                        //txtCopied.Visible = false;
                        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, typeof(string), "alertscript", "alert('Check date and time format date: dd/mm/yyyy time- hh:mm');", true);
                        return;
                    }
                    dr[1] = dtshow.Rows[i][1];
                    dr[2] = dtshow.Rows[i][2];
                    dr[3] = dtshow.Rows[i][3];
                    dr[4] = dtshow.Rows[i][4];
                    dr[5] = dtshow.Rows[i][5];
                    if (dtshow.Rows[i][6].ToString().Length > 0)
                    {
                        string[] days = dtshow.Rows[i][6].ToString().Split('.');
                        if (days.Length == 1)
                        {
                            dr[6] = days[0] + ".00";
                        }
                        else if (days.Length == 2)
                        {
                            if (days[1].Length >= 2)
                            {
                                dr[6] = days[0] + "." + days[1].Substring(0, 2);
                            }
                            else if (days[1].Length == 1)
                            {
                                dr[6] = days[0] + "." + days[1] + "0";
                            }

                        }
                        else
                        {
                            txtCopied.Text = "";
                            txtCopied.Visible = false;
                            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, typeof(string), "alertscript", "alert('Check Number of days format');", true);
                            return;
                        }
                    }
                    dr[7] = dtshow.Rows[i][7];
                    dtfinalshow.Rows.Add(dr);
                }
                txtCopied.Text = "";
                txtCopied.Visible = false;
                resultgridview.DataSource = dtfinalshow;
                resultgridview.DataBind();
                ViewState["griddt"] = dtshow;

            }
            catch (Exception ex)
            {

            }
        }

        public void getdtfromgrid()
        {
            string uid = Session["user"].ToString();
            string siteid = ddsiteid.SelectedItem.Text;
            string mon = ddmonth.SelectedItem.Value;
            string year = ddyear.SelectedItem.Text;
            string month = "";
            if (mon.Length == 1)
            {
                month += "0";
            }
            month += mon + year;
            DataTable dtbl = ViewState["griddt"] as DataTable;
            Processing obj = new Processing();
            string result = obj.insertorupdate(dtbl, uid, siteid, month);
            if (result == "true")
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, typeof(string), "alertscript", "alert('Details submitted Successfully');", true);
                //filldropdowns();
                displayrecords();
                resultgridview.Columns[0].Visible = true;
            }
            else
            {
                if (result.Contains("Blank"))
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, typeof(string), "alertscript", "alert('Date out, Unit and Shutdown Time cannot left blank');", true);
                }
                else if (result.Contains("Time"))
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, typeof(string), "alertscript", "alert('Check date and time format date: dd/mm/yyyy time- hh:mm');", true);
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, typeof(string), "alertscript", "alert('Cannot submit the details try agian or recheck values');", true);
                }
            }

        }

        protected void btnsave_Click(object sender, EventArgs e)
        {

            if (ddsiteid.SelectedValue == "0" || ddsiteid.SelectedIndex == 0)
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, typeof(string), "alertscript", "alert('Please select the Site ID');", true);
                ddsiteid.Focus();
            }
            else
            {
                getdtfromgrid();
                btnreset.Visible = true;
                btncancel.Visible = false;
            }
        }

        protected void btncancel_Click(object sender, EventArgs e)
        {
            displayrecords();
            btnreset.Visible = true;
            btncancel.Visible = false;
            txtCopied.Visible = false; ;
        }
    }
}