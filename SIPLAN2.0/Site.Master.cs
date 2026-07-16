using SIPLAN2._0.DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SIPLAN2._0
{
    public partial class SiteMaster : MasterPage
    {
        string sql;
        int estado;
        string mensaje;
        clsAccesoBBDD dao = new clsAccesoBBDD();
        DataTable infoUser = new DataTable();
        protected void Page_Load(object sender, EventArgs e)
        {
            DateTime anio = DateTime.Today;
            //anios.Text = Convert.ToString(anio.Year);

            if (Session["Usuario"] == null || Session["usuario_nombre"] == null || Session["institucion_nombre"] == null)
            {
                lblUsuario.Style["display"] = "none";
                lblInsto.Style["display"] = "none";
                logout.Style["display"] = "none";
                //pom.Style["display"] = "none";
                //oficioSolicitud.Style["display"] = "none";
                //oficioRepro.Style["display"] = "none";
                manualrepro.Style["display"] = "none";
                manualusuario.Style["display"] = "none";
                reset.Style["display"] = "none";
                atencionUsuario.Style["display"] = "block";
                informacion_usuariosss.Style["display"] = "none";

            }
            else
            {
                lblUsuario.Style["display"] = "block";
                lblInsto.Style["display"] = "block";
                logout.Style["display"] = "block";
                //pom.Style["display"] = "block";
                //oficioSolicitud.Style["display"] = "block";
                //oficioRepro.Style["display"] = "block";
                manualrepro.Style["display"] = "block";
                manualusuario.Style["display"] = "block";
                reset.Style["display"] = "block";
                lblUsuario.Text = Session["usuario_nombre"].ToString();
                lblInsto.Text = Session["institucion_nombre"].ToString();
                atencionUsuario.Style["display"] = "none";
                informacion_usuariosss.Style["display"] = "block";
            }
            
        }

        protected void atencionUsuario_Click(object sender, EventArgs e)
        {
            popDocumento.ContentUrl = "https://siplan.segeplan.gob.gt/imagenes/usuarios_SIPLAN.jpg";
            popDocumento.ShowOnPageLoad = true;
        }
    }
}