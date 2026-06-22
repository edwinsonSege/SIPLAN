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
    public partial class _Default : Page
    {
        clsAccesoBBDD dao = new clsAccesoBBDD();
        string sql;
        DataTable tabla = new DataTable();
        protected void Page_Load(object sender, EventArgs e)
        {
            sql = "SELECT SPV$ENLACE FROM SCHE$SIPLAN20.SP20$CONFIGURACION_VARIAS WHERE SPV$DESCRIPCION = 'ENLACE DEFAULT' AND SPV$RESTRICTIVA = 'N'";
            int estado = dao.consulta(sql);
            if (estado == 1)
            {
                tabla = dao.tabla;
                if (tabla.Rows.Count > 0)
                    Response.Redirect(tabla.Rows[0]["SPV$ENLACE"].ToString());

            }

            /*Response.Redirect("../siplancapa/login/login.aspx");
            Response.Redirect("../siplan/login/login.aspx");*/
        }
    }
}