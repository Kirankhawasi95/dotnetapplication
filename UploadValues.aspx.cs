using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Globalization;
using System.IO;

namespace ShutDownDetails
{
    public partial class UploadValues : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
          
            if (!IsPostBack)
            {
                initailvalues();
            }
            getdtfromgrid();
        }

        public void initailvalues()
        {
            foreach (string filename in Directory.GetFiles(Server.MapPath("~/Files")))
            {               
                showresult(filename);  
            }
        }

        public void showresult(string filepath)
        {
            try
            {
                Processing objprocess = new Processing();
                DataTable dtfromexcel = objprocess.createdatatablefromexcel(filepath);               
                DataTable dtshow = new DataTable();
                dtshow.Columns.Add("DATE (OIL OUT)", typeof(String));
                dtshow.Columns.Add("UNIT", typeof(String));
                dtshow.Columns.Add("SHUTDOWN", typeof(String));
                dtshow.Columns.Add("DATE(OIL IN)", typeof(String));
                dtshow.Columns.Add("START UP", typeof(String));
                dtshow.Columns.Add("DURATION [DAYS]", typeof(String));
                dtshow.Columns.Add("REASONS", typeof(String));
                int colcount = dtfromexcel.Columns.Count;
                List<Tuple<string, int>> colnames = new List<Tuple<string, int>>();
                int rowstart = 0;
                for (int i = 0; i < colcount; i++)
                {
                    if (dtfromexcel.Columns[i].ColumnName.Contains("OIL OUT") || dtfromexcel.Columns[i].ColumnName.Contains("UNIT") || dtfromexcel.Columns[i].ColumnName.Contains("SHUTDOWN") || dtfromexcel.Columns[i].ColumnName.Contains("START") || dtfromexcel.Columns[i].ColumnName.Contains("OIL IN") || dtfromexcel.Columns[i].ColumnName.Contains("DURATION") || dtfromexcel.Columns[i].ColumnName.Contains("REASONS"))
                    {
                        colnames.Add(new Tuple<string, int>(dtfromexcel.Columns[i].ColumnName, i));
                    }
                }
                if (colnames.Count != 7)
                {
                    for (int i = 0; i < dtfromexcel.Rows.Count; i++)
                    {
                        for (int j = 0; j < dtfromexcel.Columns.Count; j++)
                        {
                            if (dtfromexcel.Rows[i][j].ToString().Contains("OIL OUT"))
                            {
                                colnames.Add(new Tuple<string, int>(dtfromexcel.Rows[i][j].ToString(), j));
                            }                           
                            else if (dtfromexcel.Rows[i][j].ToString()=="UNIT")
                            {
                                colnames.Add(new Tuple<string, int>(dtfromexcel.Rows[i][j].ToString(), j));
                            }
                            else if (dtfromexcel.Rows[i][j].ToString()=="SHUTDOWN")
                            {
                                colnames.Add(new Tuple<string, int>(dtfromexcel.Rows[i][j].ToString(), j));
                            }
                            else if (dtfromexcel.Rows[i][j].ToString().Contains("OIL IN"))
                            {
                                colnames.Add(new Tuple<string, int>(dtfromexcel.Rows[i][j].ToString(), j));
                            }
                            else if (dtfromexcel.Rows[i][j].ToString().Contains("START"))
                            {
                                colnames.Add(new Tuple<string, int>(dtfromexcel.Rows[i][j].ToString(), j));
                            }
                            else if (dtfromexcel.Rows[i][j].ToString().Contains("DURATION"))
                            {
                                colnames.Add(new Tuple<string, int>(dtfromexcel.Rows[i][j].ToString(), j));
                            }
                            else if (dtfromexcel.Rows[i][j].ToString().Contains("REASON"))
                            {
                                colnames.Add(new Tuple<string, int>(dtfromexcel.Rows[i][j].ToString(), j));
                            }
                        }
                        if (colnames.Count == 7)
                        {
                            rowstart = i + 1;
                            break;
                        }                       
                    }
                }
                for (int i = rowstart; i < dtfromexcel.Rows.Count; i++)
                {
                    DataRow dr = dtshow.NewRow();
                    for (int j = 0; j < colnames.Count; j++)
                    {
                        if (colnames[j].Item1.Contains("OIL OUT"))
                        {
                            dr[0] = dtfromexcel.Rows[i][colnames[j].Item2];
                        }
                        else if (colnames[j].Item1.Contains("UNIT"))
                        {
                            dr[1] = dtfromexcel.Rows[i][colnames[j].Item2];
                        }
                        else if (colnames[j].Item1.Contains("SHUTDOWN"))
                        {
                            dr[2] = dtfromexcel.Rows[i][colnames[j].Item2];
                        }
                        else if (colnames[j].Item1.Contains("OIL IN"))
                        {
                            dr[3] = dtfromexcel.Rows[i][colnames[j].Item2];
                        }
                        else if (colnames[j].Item1.Contains("START"))
                        {
                            dr[4] = dtfromexcel.Rows[i][colnames[j].Item2];
                        }
                        else if (colnames[j].Item1.Contains("DURATION"))
                        {
                            dr[5] = dtfromexcel.Rows[i][colnames[j].Item2];
                        }
                        else if (colnames[j].Item1.Contains("REASON"))
                        {
                            dr[6] = dtfromexcel.Rows[i][colnames[j].Item2];
                        }
                    }
                    dtshow.Rows.Add(dr);
                }
                resultgridview.DataSource = dtshow;
                resultgridview.DataBind();
                
            }
            catch (Exception ex)
            {

            }
        }

        protected void PasteToGridView(object sender, EventArgs e)
        {
            try
            {
                DataTable dtshow = new DataTable();
                dtshow.Columns.Add("DATE (OIL OUT)", typeof(String));
                dtshow.Columns.Add("UNIT", typeof(String));
                dtshow.Columns.Add("SHUTDOWN", typeof(String));
                dtshow.Columns.Add("DATE(OIL IN)", typeof(String));
                dtshow.Columns.Add("START UP", typeof(String));
                dtshow.Columns.Add("DURATION [DAYS]", typeof(String));
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
                            dtshow.Rows[dtshow.Rows.Count - 1][i] = cell;
                            i++;
                        }
                    }
                }
                resultgridview.DataSource = dtshow;
                resultgridview.DataBind();
                txtCopied.Text = "";
                txtCopied.Visible = false;
                chkpaste.Checked = false;
            }
            catch (Exception ex)
            { 
            
            }
          
        }

        protected void chkpaste_CheckedChanged(object sender, EventArgs e)
        {
            if (chkpaste.Checked == true)
            {
                txtCopied.Visible = true;
            }
            else
            {
                txtCopied.Visible = false;
            }
        }

        protected void btnupload_Click(object sender, EventArgs e)
        {
            String filename = Path.GetFileName(fileUpload1.PostedFile.FileName);
            fileUpload1.SaveAs(Server.MapPath("~/Files/" + filename));                       
            showresult(Server.MapPath("~/Files/" + filename));  
        }

        protected void btnaddtodb_Click(object sender, EventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {

            }
        }
        protected void btncancelgrid_Click(object sender, EventArgs e)
        {
            resultgridview.FooterRow.Visible = false;
        }

        public void fileselected()
        {
            btnupload.Visible = true;
        }

        public void filedownload()
        {
            Response.ContentType = "Application/xlsx";
            Response.AppendHeader("Content-Disposition", "attachment; filename=SampleDetails.xlsx");
            Response.TransmitFile(Server.MapPath("~/Sample/SampleDetails.xlsx"));
            Response.End();
        }

        protected void lnkbuttonsample_Click(object sender, EventArgs e)
        {
            filedownload();
        }

        public void getdtfromgrid()
        {
            DataTable dtbl = resultgridview.DataSource as DataTable;
            Processing obj = new Processing();
            //obj.insertorupdate(dtbl);
        }
    }
}