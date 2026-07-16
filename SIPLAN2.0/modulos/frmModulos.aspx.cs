using SIPLAN2._0.DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SIPLAN2._0.modulos
{
    public partial class frmModulos : System.Web.UI.Page
    {
        String mensaje = "";
        string sql;
        DataTable tabla = new DataTable();
        clsAccesoBBDD dao = new clsAccesoBBDD();
        int estado = -1;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Usuario"] == null || Session["Insto"] == null)
            {
                Response.Redirect("../login/login.aspx");
            }
            else
            {



                sql = "SELECT SPV$ENLACE, SPV$MUESTRA, SPV$DESCRIPCION FROM SCHE$SIPLAN20.SP20$CONFIGURACION_VARIAS WHERE SPV$RESTRICTIVA = 'N' AND SPCV$TIPO = 0";

                estado = dao.consulta(sql);
                if (estado == 1)
                {
                    tabla = dao.tabla;
                    if (tabla.Rows.Count > 0)
                    {
                        if (Convert.ToInt32(tabla.Rows[0]["SPV$MUESTRA"]) == 1)
                        {
                            //popDocumento.ContentUrl = "../../imagenes/" + tabla.Rows[0]["SPV$ENLACE"];
                            popDocumento.HeaderText = tabla.Rows[0]["SPV$DESCRIPCION"].ToString();
                            imgPopup.ImageUrl = "https://siplan.segeplan.gob.gt/imagenes/" + tabla.Rows[0]["SPV$ENLACE"].ToString();
                            popDocumento.ShowOnPageLoad = true;
                        }
                        else
                        {
                            imgPopup.ImageUrl = "";
                            popDocumento.ShowOnPageLoad = false;
                        }

                    }
                    else
                    {
                        imgPopup.ImageUrl = "";
                        popDocumento.ShowOnPageLoad = false;
                    }


                }
                else
                {
                    imgPopup.ImageUrl = "";
                    popDocumento.ShowOnPageLoad = false;

                }


                if (Session["ROL"].ToString() == "ADMIN" || Session["ROL"].ToString() == "ENTIDAD")
                {
                    reportes.Style.Add("display", "block");
                    
                }

                if ((Session["Usuario"].ToString() == "EDWINSON" && Session["ROL"].ToString() == "ADMIN") || (Session["Usuario"].ToString() == "DHGALINDO" && Session["ROL"].ToString() == "ADMIN") || (Session["Usuario"].ToString() == "JDRAMIREZ" && Session["ROL"].ToString() == "ADMIN") || (Session["Usuario"].ToString() == "SCITALAN" && Session["ROL"].ToString() == "ADMIN") || (Session["Usuario"].ToString() == "HEGONZALEZ" && Session["ROL"].ToString() == "ADMIN"))
                {
                    procesos.Style.Add("display", "block");
                }

                if (Session["cambiar"] != null)
                    {
                        if (Convert.ToInt32(Session["cambiar"]) == 0 && Session["Usuario"].ToString() != "EDWINSON")
                        {
                            mensaje = "<p>Esta iniciando sesión con una contraseña generica. Para mejorar la seguridad en el uso de nuestros sistemas le recomendamos que cambie su contraseña.</p><p>presione el boton Cambiar contraseña, ubicado en el encabezado del sistema</p><p>¿Desea cambiarla ahora?, presione ACEPTAR para cambiar, presione CANCELAR para cerrar esta ventana y continuar el uso del sistema con sus mismas credenciales</p>";
                            Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',3);", true);
                        }

                    }
                    else
                    {
                        Response.Redirect("../login/login.aspx");
                    }
                
                
            }
        }
    }
}