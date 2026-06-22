using SIPLAN2._0.DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SIPLAN2._0.login
{
	public partial class reincia : System.Web.UI.Page
	{
        int estado;
        string mensaje, sql;
        clsAccesoBBDD dao = new clsAccesoBBDD();
        DataTable tabla = new DataTable();
        protected void Page_Load(object sender, EventArgs e)
		{
            if (Session["Usuario"] == null && Session["VIENE"] == null)
                Response.Redirect("../Login/Login.aspx");
            else
            {
                sql = "SELECT E.ENTIDAD, E.NOMBRE||'-'||E.SIGLA NOMBRE, U.ADSCGUS$USUARIO, U.ADSCGUS$NOMBRE FROM SCHE$ADSIS.ADSTBCG$USUARIOS U INNER JOIN SCHE$ADSIS.ADSTBCG$USUARIOS_ENTIDADES ED ON U.ADSCGUS$USUARIO = ED.ADSCGEN$USUARIO INNER JOIN SINIP.CG_ENTIDADES E ON E.ENTIDAD = ED.ADSCGEN$ID_ENTIDAD  WHERE U.ADSCGUS$USUARIO = '" + Session["USUARIO"].ToString() + "' AND U.ADSCGUS$RESTRICTIVA = 'N' AND ED.ADSCGEN$RESTRICTIVA = 'N' AND E.RESTRICTIVA = 'N'";
                estado = dao.consulta(sql);

                if (estado == 0)
                {
                    mensaje = dao.mensaje;
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + "<br/>',2);", true);

                }

                else
                {
                    tabla = dao.tabla;

                    if (tabla.Rows.Count <= 0 || tabla.Rows.Count < 1)
                    {
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('Usuario invalido, se cerrará sesión <br/>',2);", true);
                        Response.Redirect("../Login/logout.aspx");
                    }

                    else
                    {
                        lblUsseroi.Text = tabla.Rows[0]["ADSCGUS$USUARIO"].ToString();
                        lblNombrecompleto.Text = tabla.Rows[0]["ADSCGUS$NOMBRE"].ToString();
                        lblDependencia.Text = tabla.Rows[0]["NOMBRE"].ToString();

                        lblNombrecompleto.ForeColor = System.Drawing.Color.Red;
                        lblDependencia.ForeColor = System.Drawing.Color.Red;

                    }

                }


            }
        }

        protected void btnCancela_Click(object sender, EventArgs e)
        {
            Response.Redirect("../login/logout.aspx");
        }

        protected void btnContrasena_Click(object sender, EventArgs e)
        {
            string contra, confirma, sql, primero, contiene;
            Boolean valida, contienes;
            contra = txtContrasena.Text;
            confirma = txtConfirme.Text;

            primero = contra.Substring(0, 1);

            valida = ValidarPassword(contra, Session["Usuario"].ToString());
            contiene = ".-/()[]{}*+'<>|;:¿?¡!$%~,";
            if (valida == false)
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel5, GetType(), "script", "Alerta('La contraseña no puede ser actualizada, por favor atienda las recomendaciones para una contraseña segura <br/>',2);", true);
                //ScriptManager.RegisterStartupScript(this.UpdatePanel5, GetType(), "script", "Alerta('Ingrese la descripción del producto <br/>',2);", true);
                txtContrasena.Text = "";
                txtConfirme.Text = "";
            }
            else
            {

                contienes = contra.Contains(contiene);


                if (contienes == true)
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel5, GetType(), "script", "Alerta('Contraseña invalida: Su contraseña tiene caracteres invalidos o no cumple con las espeficaciones de una contraseña segúra <br/>',2);", true);
                    txtContrasena.Text = "";
                    txtConfirme.Text = "";
                }

                else
                {
                    if ((contra == confirma) && valida == true && contienes == false)
                    {
                        sql = "UPDATE SCHE$ADSIS.ADSTBCG$USUARIOS_ENTIDADES SET ADSCGEN$TEMPORAL = 0 WHERE ADSCGEN$USUARIO = '"+ Session["USUARIO"].ToString() + "'";

                        estado = dao.comando(sql);
                        if (estado == 1)
                        {
                            sql = "ALTER USER " + Session["USUARIO"] + " ACCOUNT UNLOCK";
                            estado = dao.comando2(sql);

                            if (estado == 0)
                            {
                                mensaje = dao.mensaje;
                                ScriptManager.RegisterStartupScript(this.UpdatePanel5, GetType(), "script", "Alerta('" + mensaje + "<br/>',2);", true);

                            }
                            else
                            {
                                //sql = "ALTER USER " + Session["USUARIO"] + " IDENTIFIED BY \"" + confirma + "\"";
                                sql = $"ALTER USER {Session["USUARIO"]} IDENTIFIED BY \"{confirma}\"";
                                estado = dao.comando4(sql);

                                if (estado == 0)
                                {
                                    mensaje = dao.mensaje;
                                    ScriptManager.RegisterStartupScript(this.UpdatePanel5, GetType(), "script", "Alerta('" + mensaje + "<br/>',2);", true);

                                }

                                else
                                {
                                    sql = "UPDATE SCHE$ADSIS.ADSTBCG$USUARIOS SET ADSCGUS$EXPIRA_PASS = 1 WHERE ADSCGUS$USUARIO = '" + Session["USUARIO"] + "' AND ADSCGUS$RESTRICTIVA = 'N'";
                                    estado = dao.comando(sql);

                                    if (estado == 1)
                                    {
                                        txtContrasena.Text = "";
                                        txtConfirme.Text = "";

                                        ScriptManager.RegisterStartupScript(
                                            this.UpdatePanel5,
                                            GetType(),
                                            "script",
                                            "Alerta('Su contraseña fue actualizada correctamente <br/>',1); setTimeout(function(){ window.top.location='../login/logout.aspx'; }, 2000);",
                                            true
                                        );
                                    }
                                }
                            }
                               

                        }
                        
                    }

                    else
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel5, GetType(), "script", "Alerta('Contraseña invalida: La palabra ingresada en los campos de Nueva contraseña y Confirme contraseña; No coincide, verifique por favor <br/>',2);", true);
                        txtContrasena.Text = "";
                        txtConfirme.Text = "";

                    }

                }


            }
        }

        public bool ValidarPassword(string password, string usuario)
        {
            string pattern = @"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[+\-_!])[A-Za-z\d+\-_!]{8,12}$";

            if (!System.Text.RegularExpressions.Regex.IsMatch(password, pattern))
            { 
                mensaje = "Contraseña invalida: La contraseña debe tener entre 8 y 12 caracteres, contener al menos una letra mayúscula, una letra minúscula, un número y un carácter especial (+ - _ !)";
                ScriptManager.RegisterStartupScript(this.UpdatePanel5, GetType(), "script", "Alerta('" + mensaje + "<br/>',2);", true);
                return false;
            }


            if (!string.IsNullOrEmpty(usuario) &&
                password.ToLower().Contains(usuario.ToLower()))
            {
                mensaje = "Contraseña invalida: La contraseña no debe contener el nombre de usuario";
                ScriptManager.RegisterStartupScript(this.UpdatePanel5, GetType(), "script", "Alerta('" + mensaje + "<br/>',2);", true);
                return false;
            }
                

            return true;
        }
    }
}