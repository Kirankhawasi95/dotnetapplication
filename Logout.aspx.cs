﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace ShutDownDetails
{
    public partial class Logout : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Session.Clear();            
            Session.RemoveAll();
            Session.Abandon();
            Server.Transfer("login.aspx", false);
        }
    }
}