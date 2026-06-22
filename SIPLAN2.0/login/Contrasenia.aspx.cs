using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SIPLAN2._0.DataAccess;
using DevExpress.XtraRichEdit.Layout.Export;

namespace SIPLAN2._0.login
{
	public partial class Contrasenia : System.Web.UI.Page
	{
        int estado;
        string mensaje, sql;
        clsAccesoBBDD dao = new clsAccesoBBDD();
        DataTable tabla = new DataTable();
        protected void Page_Load(object sender, EventArgs e)
		{

		}

        protected void btnCancela_Click(object sender, EventArgs e)
        {
            Response.Redirect("../login/login.aspx");
        }

        protected void btnContrasena_Click(object sender, EventArgs e)
        {
            string usuario = "";
            string telefono = "";
            string telefonoConfirma = "";
            string correo = "";
            string tempPass = "";
            string hash = "";
            usuario = txtUsuario.Text;
            telefono = txtTelefono.Text;


            sql = @"SELECT U.ADSCGUS$USUARIO,ADSCGUS$EMAIL,ADSCGUS$TELEFONOS
                    FROM SCHE$ADSIS.ADSTBCG$USUARIOS U
                    INNER JOIN SCHE$ADSIS.ADSTBCG$USUARIOS_ENTIDADES E ON E.ADSCGEN$USUARIO = U.ADSCGUS$USUARIO AND U.ADSCGUS$RESTRICTIVA = 'N' AND E.ADSCGEN$RESTRICTIVA = 'N'  
                    WHERE ADSCGUS$USUARIO = '"+usuario+"'";
            estado = dao.consulta(sql);
            if (estado == 1)
            {
                tabla = dao.tabla;
                if (tabla.Rows.Count > 0)
                {
                    telefonoConfirma = tabla.Rows[0]["ADSCGUS$TELEFONOS"].ToString();
                    telefonoConfirma = telefonoConfirma.Length >= 4 ? telefonoConfirma.Substring(telefonoConfirma.Length - 4) : telefonoConfirma;

                    if (usuario.Equals(tabla.Rows[0]["ADSCGUS$USUARIO"].ToString()) && telefono.Equals(telefonoConfirma))
                    {
                        tempPass = PasswordHelper.GenerarPassword();
                        hash = Seguridad.HashPassword(tempPass);
                        estado = ActualizaPass(usuario, hash);
                        if (estado == 1)
                        {
                            correo = tabla.Rows[0]["ADSCGUS$EMAIL"].ToString();
                            EnviarCorreo(correo, tempPass);
                        }

                    }
                }
                

            }
            mensaje = "La contraseña ha sido envidada al correo registrado, favor de revisar su bandeja de entrada, en caso de recibir el correo, favor de contactarse a soporte@segeplan.gob.gt";
            Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "alert('" + mensaje + "');", true);
            //string script = "setTimeout(function(){ window.location='../Login/logout.aspx'; }, 8000);";
            //ClientScript.RegisterStartupScript(this.GetType(), "redirect", script, true);
            Response.Redirect("../Login/Login.aspx");
        }


        protected int ActualizaPass(string user, string pass)
        {
            int estados = 0;
            sql= "UPDATE SCHE$ADSIS.ADSTBCG$USUARIOS_ENTIDADES SET ADSCGEN$TEMPORAL = 1, ADSCGEN$PASS_TEMP = '"+pass+ "', ADSCGEN$FECHA_EXPIRA = SYSDATE WHERE ADSCGEN$USUARIO='"+user+"'";
            estados = dao.comando(sql);
            return estados;
        }
        public static class PasswordHelper
        {
            static string may = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            static string min = "abcdefghijklmnopqrstuvwxyz";
            static string num = "0123456789";
            static string espe = "+-_!";

            public static string GenerarPassword()
            {
                var rnd = new RNGCryptoServiceProvider();
                int length = new Random().Next(8, 13);

                string all = may + min + num;
                string pass;

                do
                {
                    pass = "";
                    pass += RandomChar(may);
                    pass += RandomChar(min);
                    pass += RandomChar(num);
                    pass += RandomChar(espe);

                    for (int i = pass.Length; i < length; i++)
                        pass += all[new Random().Next(all.Length)];

                    pass = new string(pass.OrderBy(x => Guid.NewGuid()).ToArray());

                } while (TieneConsecutivos(pass));

                return pass;
            }

            static char RandomChar(string s)
            {
                return s[new Random().Next(s.Length)];
            }

            static bool TieneConsecutivos(string p)
            {
                for (int i = 0; i < p.Length - 1; i++)
                    if (char.IsDigit(p[i]) && char.IsDigit(p[i + 1]) && p[i + 1] == p[i] + 1)
                        return true;

                return false;
            }
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

        public void EnviarCorreo(string destino, string password)
        {
            /*MailMessage mail = new MailMessage();
            mail.To.Add(destino);
            mail.From = new MailAddress("desarrollo11@sgeplan.gob.gt");
            mail.Subject = "Contraseña temporal";
            mail.Body = $"Tu contraseña temporal es: {password}\nDebes cambiarla al ingresar.";
            mail.IsBodyHtml = false;

            SmtpClient smtp = new SmtpClient("segeplan-gob-gt.mail.protection.outlook.com");
            smtp.Credentials = new NetworkCredential("desarollo11@segeplan.gob.gt", "SoftwareSegeplan2032");
            smtp.Port = 25;
            smtp.EnableSsl = true;

            smtp.Send(mail);*/
            bool estados = false;
            string from = "sistemas@segeplan.gob.gt";
            string asunto = "Clave temporal";
            string mensaje = $"Su clave es: {password}\n\n su temporalidad es de 10 minutos, debe cambiarla al ingresar.";

            estados = dao.comando3(destino, from, asunto, mensaje);
        }
    }
}