using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using Oracle.ManagedDataAccess.Client;

namespace ShutDownDetails
{
    public static class StaticConnection
    {
        static Log objlog = new Log();
        static OracleConnection con;
        public static OracleConnection getconnection()
        {
            try
            {
                if (con == null)
                {
                    string constr = ConfigurationManager.ConnectionStrings["oracleconstr"].ConnectionString;
                    con = new OracleConnection(constr);
                }
            }
            catch (Exception ex)
            {
                objlog.writelog("In dbConnection.getconnection :: " + ex.Message);
            }
            return con;
        }
    }
}