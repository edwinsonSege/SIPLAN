using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
//using System.Data.OracleClient;
using Oracle.ManagedDataAccess.Client;
using System.Configuration;
using SIPLAN2._0.DataAccess;
using System.Data;
using DevExpress.XtraRichEdit.Model;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;

namespace SIPLAN2._0.login
{
    public partial class login : System.Web.UI.Page
    {
        string mensaje, sql;

        int estado;
        clsAccesoBBDD dao = new clsAccesoBBDD();
        DataTable tabla = new DataTable();
        DataTable temp = new DataTable();
        DataTable Reinicia = new DataTable();
        int entidad = -1;
       

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Usuario"] != null && Session["Insto"] != null && Session["ROL"] != null)
                Response.Redirect("../modulos/frmModulos.aspx");


        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            int estado;
            int temporal;
            temporal = entrar_temporal();
            string cripto = "";
            bool expira = true;

            DateTime ahora = DateTime.Now;

            temporal = entrar_temporal();

            if (temporal == 1)
            {
                Reinicia = (DataTable)Session["TEMPORAL"];
                if (Reinicia.Rows.Count > 0)
                {
                    DateTime fechaBD = Convert.ToDateTime(Reinicia.Rows[0]["ADSCGEN$FECHA_EXPIRA"]);
                    cripto = Seguridad.HashPassword(txtPass.Text);
                    TimeSpan diferencia = ahora - fechaBD;
                    if (diferencia.TotalMinutes > 10)
                        expira = true;
                    else
                        expira = false;

                    if (txtUser.Text.Equals(Reinicia.Rows[0]["ADSCGUS$USUARIO"].ToString()) && cripto.Equals(Reinicia.Rows[0]["ADSCGEN$PASS_TEMP"].ToString()) && expira == false)
                    {
                        Session["VIENE"] = true;
                        Session["Usuario"] = Reinicia.Rows[0]["ADSCGUS$USUARIO"].ToString();
                        Response.Redirect("../Login/reincia.aspx");

                    }
                        
                    else
                    {
                        Session["VIENE"] = null;
                        mensaje = "Credenciales incorrectas";
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + "<br/>',2);", true);
                    }


                }
                else
                {
                    Session["VIENE"] = null;
                    mensaje = "Credenciales incorrectas";
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + "<br/>',2);", true);
                }

            }
            else
            {
                estado = entrar();
                String institucion = "";

                if (estado == 0)
                {
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + "<br/>',2);", true);
                    //Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + Session["mensaje"].ToString() + "<br/>',2);", true);
                }

                else
                {
                    sql = "SELECT ADSCGUS$EXPIRA_PASS FROM SCHE$ADSIS.ADSTBCG$USUARIOS WHERE ADSCGUS$USUARIO = '" + Session["USUARIO"].ToString() + "' AND ADSCGUS$RESTRICTIVA = 'N'";
                    estado = dao.consulta(sql);

                    if (estado == 1)
                    {

                        tabla = dao.tabla;
                        if (Convert.ToInt32(tabla.Rows[0]["ADSCGUS$EXPIRA_PASS"]) == 0)
                        {
                            Response.Redirect("../login/reset.aspx");
                        }
                        else 
                        {
                            sql = "SELECT * FROM SCHE$ADSIS.ADSTBCG$USUARIOS_ROLES R WHERE R.ADSCGUR$RESTRICTIVA = 'N' AND R.ADSCGUR$SISTEMA = 'SPL' AND  R.ADSCGUR$ROL IN ('SPLROL$VER_TODAS_ENTIDADES','SPLROL$ADMINISTRADOR','SPLROL$DPS','SPLROL$CONSULTA','SPLROL$SISTEMA', 'SPLROL$ELIMINAR_PLAN','SPLROL$ROLCAPACITACION') AND R.ADSCGUR$RESTRICTIVA = 'N' AND R.ADSCGUR$USUARIO = '" + Session["USUARIO"].ToString() + "'";
                            estado = dao.consulta(sql);
                            if (estado == 0)
                            {
                                mensaje = dao.mensaje;
                                Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + "<br/>',2);", true);
                            }
                            else
                            {
                                temp = dao.tabla;
                                if (temp.Rows.Count <= 0)
                                {
                                    mensaje = "No esta autorizado para utilizar este sistema, contacte al administrador";
                                    Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + "<br/>',2);", true);
                                }
                                else
                                {
                                    temp.PrimaryKey = new[] { temp.Columns["ADSCGUR$ROL"] };
                                    sesiones(Session["USUARIO"].ToString());

                                    sql = "SELECT US.ADSCGUS$NOMBRE, CG.ENTIDAD, CG.NOMBRE FROM SCHE$ADSIS.ADSTBCG$USUARIOS US INNER JOIN SCHE$ADSIS.ADSTBCG$USUARIOS_ENTIDADES EN ON US.ADSCGUS$USUARIO = EN.ADSCGEN$USUARIO AND US.ADSCGUS$RESTRICTIVA = 'N' AND EN.ADSCGEN$RESTRICTIVA = 'N' INNER JOIN SINIP.CG_ENTIDADES CG ON EN.ADSCGEN$ID_ENTIDAD = CG.ENTIDAD AND EN.ADSCGEN$RESTRICTIVA = 'N' AND CG.RESTRICTIVA = 'N' WHERE US.ADSCGUS$USUARIO = '" + Session["USUARIO"].ToString() + "'";
                                    estado = dao.consulta(sql);
                                    if (estado == 0)
                                    {
                                        mensaje = dao.mensaje;
                                        Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + "<br/>',2);", true);
                                    }
                                    else
                                    {
                                        tabla = dao.tabla;

                                        if (tabla.Rows.Count > 0)
                                        {
                                            entidad = Convert.ToInt32(tabla.Rows[0]["ENTIDAD"].ToString());

                                            if (temp.Rows.Contains("SPLROL$SISTEMA"))
                                                Session["ROL"] = "USER";
                                            if (temp.Rows.Contains("SPLROL$VER_TODAS_ENTIDADES"))
                                                Session["ROL"] = "ENTIDAD";
                                            if (temp.Rows.Contains("SPLROL$ADMINISTRADOR"))
                                                Session["ROL"] = "ADMIN";
                                            if (temp.Rows.Contains("SPLROL$ADMINISTRADOR") && temp.Rows.Contains("SPLROL$ROLCAPACITACION"))
                                                Session["ROL"] = "CAPA";
                                            /*if (!temp.Rows.Contains("SPLROL$ADMINISTRADOR") && temp.Rows.Contains("SPLROL$ROLCAPACITACION"))
                                                Session["ROL"] = "CAPA";
                                                */


                                            Session["usuario_nombre"] = tabla.Rows[0]["ADSCGUS$NOMBRE"].ToString();
                                            Session["institucion_nombre"] = tabla.Rows[0]["NOMBRE"].ToString();
                                            Session["insto"] = tabla.Rows[0]["ENTIDAD"];
                                            institucion = tabla.Rows[0]["NOMBRE"].ToString();

                                            if (txtUser.Text == txtPass.Text && txtUser.Text != "CONSULTA")
                                                Session["cambiar"] = 0;
                                            else
                                                Session["cambiar"] = 1;


                                            sql = "SELECT * FROM SCHE$SIPLAN20.SP20$GERO_INSTO WHERE SPSG$INSTO = " + entidad + " AND SPSG$RESTRICTIVA = 'N'";
                                            estado = dao.consulta(sql);
                                            if (estado == 1)
                                            {
                                                tabla = dao.tabla;
                                                if (tabla.Rows.Count > 0)
                                                    Session["GERO"] = true;
                                                else
                                                    Session["GERO"] = false;
                                            }
                                            else
                                                Session["GERO"] = false;



                                            if (institucion.Contains("MUNICIPALIDAD") && Session["ROL"].ToString() != "ADMIN" && Session["ROL"].ToString() != "ENTIDAD")
                                            {

                                                Response.Redirect("../login/logout.aspx");
                                            }



                                            else

                                                Response.Redirect("../modulos/frmModulos.aspx");
                                        }

                                        else
                                        {
                                            txtUser.Text = "";
                                            txtPass.Text = "";
                                            mensaje = "Es posible que tenga mal configurado sus roles de ingreso a sistema o que su cuenta de usuario se encuentre inactiva, por favor comunique el problema al administrador del sistema indicado su credenciales de usuario";
                                            Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + "<br/>',2);", true);
                                        }

                                    }







                                }


                            }
                        }

                    }



                    


                }

            }


             



        }

        public int entrar_temporal()
        {
            int tempora = 0;
            sql = @"SELECT U.ADSCGUS$USUARIO,U.ADSCGUS$EMAIL,U.ADSCGUS$TELEFONOS, E.ADSCGEN$FECHA_EXPIRA, E.ADSCGEN$PASS_TEMP
                    FROM SCHE$ADSIS.ADSTBCG$USUARIOS U
                    INNER JOIN SCHE$ADSIS.ADSTBCG$USUARIOS_ENTIDADES E ON E.ADSCGEN$USUARIO = U.ADSCGUS$USUARIO AND U.ADSCGUS$RESTRICTIVA = 'N' AND E.ADSCGEN$RESTRICTIVA = 'N'  
                    WHERE ADSCGUS$USUARIO = '" + txtUser.Text+"' AND ADSCGEN$TEMPORAL = 1";
            estado = dao.consulta(sql);
            if (estado == 1)
            {
                tabla = dao.tabla;
                if (tabla.Rows.Count > 0)
                {
                    tempora = 1;
                    Session["TEMPORAL"] = tabla;
                }
                    
                else

                    tempora = 0;

            }
            else
                tempora = 0;

                return tempora;
        }

        public static class Seguridad
        {
            public static string HashPassword(string password)
            {
                using (var sha = SHA256.Create())
                {
                    byte[] salt = Encoding.UTF8.GetBytes("SALT_FIJO_123"); // mejor desde config
                    byte[] bytes = Encoding.UTF8.GetBytes(password);

                    byte[] combinado = bytes.Concat(salt).ToArray();
                    byte[] hash = sha.ComputeHash(combinado);

                    return Convert.ToBase64String(hash);
                }
            }
        }
        /*public int entrar()
        {
            OracleConnection ora = new OracleConnection("Data Source=192.168.8.11/DESA;Persist Security Info=True;User ID=" + txtUser.Text + ";Password=" + txtPass.Text);
            //OracleConnection ora = new OracleConnection("Data Source=SIPLAN;Persist Security Info=True;User ID=" + txtUser.Text + ";Password=" + txtPass.Text);
            try
            {
                ora.Open();
                estado = 1;
                Session["USUARIO"] = txtUser.Text;
                ora.Close();
                ora.Dispose();
                return estado;

            }
            catch (Exception e)
            {
                estado = 0;

                txtUser.Text = "";
                txtPass.Text = "";
                e.Message.ToString();
                mensaje = "Usuario o contraseña incorrectos";
                //mensaje = e.Message.ToString();
                //Session["mensaje"] = mensaje;
                return estado;


            }
        }*/

        public int entrar()
        {
            string usuario = txtUser.Text.Trim();
            string password = txtPass.Text;

            if (string.IsNullOrWhiteSpace(usuario) || string.IsNullOrWhiteSpace(password))
            {
                mensaje = "Debe ingresar usuario y contraseña";
                return 0;
            }

            try
            {
                // 🔥 IMPORTANTE en tu caso
                OracleConnection.ClearAllPools();

                var builder = new OracleConnectionStringBuilder
                {
                    DataSource = "192.168.8.11/OSNIP",
                    //DataSource = "192.168.8.11/DESA",
                    UserID = usuario,
                    Password = password,
                    PersistSecurityInfo = true
                };

                using (OracleConnection ora = new OracleConnection(builder.ToString()))
                {
                    ora.Open();

                    estado = 1;
                    Session["USUARIO"] = usuario;
                }

                return estado;
            }
            catch (OracleException ex)
            {
                estado = 0;

                txtUser.Text = "";
                txtPass.Text = "";

                // 🔍 Manejo real de errores Oracle
                switch (ex.Number)
                {
                    case 1017: // ORA-01017
                        mensaje = "Usuario o contraseña incorrectos";
                        break;

                    case 28000: // usuario bloqueado
                        mensaje = "Usuario o contraseña incorrectos";
                        break;

                    case 12541: // TNS:no listener
                        mensaje = "Usuario o contraseña incorrectos";
                        break;

                    case 12514:
                        mensaje = "Usuario o contraseña incorrectos";
                        break;

                    default:
                        mensaje = "Usuario o contraseña incorrectos: " + ex.Message;
                        break;
                }

                return estado;
            }
        }
        protected void txtPass_TextChanged(object sender, EventArgs e)
        {
            btnLogin_Click(sender, e);
        }

        public void sesiones(string usuario)
        {
            sql = "INSERT INTO SCHE$SIPLAN20.SPP$SESIONES (SES$OPERACION, SES$DESCRIPCION) VALUES ('INICIO SE SESION', 'INICIA = '||TO_CHAR(SYSDATE,'DD/MM/YYYY HH24:MI')||' '||'" + usuario + "')";
            estado = dao.comando(sql);
        }

    }
}