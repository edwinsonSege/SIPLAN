using SIPLAN2._0.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Oracle.ManagedDataAccess.Client;
namespace SIPLAN2._0.login
{
    public partial class logout : System.Web.UI.Page
    {
        clsAccesoBBDD dao = new clsAccesoBBDD();
        string sql;
        int estado;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["USUARIO"] != null)
                sesiones(Session["USUARIO"].ToString());
            try
            {
                dao.salir();
                Session.Contents.RemoveAll();
                System.Web.Security.FormsAuthentication.SignOut();
                //Session.Abandon()
                HttpCookie cookie = new HttpCookie("ASP.NET_SessionId");
                Response.Cookies.Add(cookie);
                OracleConnection.ClearAllPools(); // refuerzo
                Response.Redirect("../Login/Login.aspx");
            }
            catch
            {

                Session.Contents.RemoveAll();
                System.Web.Security.FormsAuthentication.SignOut();
                //Session.Abandon()
                HttpCookie cookie = new HttpCookie("ASP.NET_SessionId");
                Response.Cookies.Add(cookie);
                OracleConnection.ClearAllPools(); // refuerzo
                Response.Redirect("../Login/Login.aspx");
            }
            
        }

        public void sesiones(string usuario)
        {
            sql = "INSERT INTO SCHE$SIPLAN20.SPP$SESIONES (SES$OPERACION, SES$DESCRIPCION) VALUES ('CIERRA SESION', 'CIERRA = '||TO_CHAR(SYSDATE,'DD/MM/YYYY HH24:MI')||' '||'" + usuario + "')";
            estado = dao.comando(sql);
        }
    }
}