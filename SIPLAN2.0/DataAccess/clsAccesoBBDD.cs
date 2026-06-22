using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Data;
using System.Configuration;
//using System.Data.OracleClient;
using Oracle.ManagedDataAccess.Client;

namespace SIPLAN2._0.DataAccess
{
    public class clsAccesoBBDD
    {
        private OracleConnection con;
        OracleCommand cmd;
        OracleDataAdapter ada;
        DataSet ds;


        public string cad = ConfigurationManager.ConnectionStrings["strOraSPL"].ConnectionString;
        public string mensaje;
        public int estado;
        public DataTable tabla = new DataTable();

        public int comando(string parquery)
        {
            string reemplazo = "";
            string mensajeestado;

            con = new OracleConnection(cad);
            con.Open();
            cmd = new OracleCommand(parquery, con);

            try
            {
                cmd.ExecuteNonQuery();
                mensaje = "Operación correcta";
                estado = 1;
                con.Close();
                return estado;
            }
            catch (Exception e)
            {
                mensajeestado = e.Message;
                mensaje = mensajeestado.Replace("\r\n", reemplazo).Replace("\n", reemplazo).Replace("\r", reemplazo);

                estado = 0;
                con.Close();
                return estado;
            }

        }

        public int consulta(string parquery)
        {
            string reemplazo = "";
            string mensajeestado;
            con = new OracleConnection(cad);
            con.Open();
            ada = new OracleDataAdapter(parquery, con);
            ds = new DataSet();
            ds.Clear();


            try
            {
                ada.Fill(ds, "CONSULTA");
                mensaje = "Operación Correcta";
                //tabla.Clear();
                tabla = ds.Tables[0];
                estado = 1;
                con.Close();
                return estado;

            }
            catch (Exception e)
            {
                mensajeestado = e.Message;
                mensaje = mensajeestado.Replace("\r\n", reemplazo).Replace("\n", reemplazo).Replace("\r", reemplazo);

                estado = 0;
                return estado;
            }

        }

        public int comando2(string parquery)
        {
            string reemplazo = "";
            string mensajeestado;

            con = new OracleConnection(cad);
            con.Open();
            cmd = new OracleCommand(parquery, con);


            try
            {
                cmd.ExecuteNonQuery();
                mensaje = "Operación correcta";
                estado = 1;
                con.Close();
              
                return estado;
            }
            catch (Exception e)
            {
                mensajeestado = e.Message;
                mensaje = mensajeestado.Replace("\r\n", reemplazo).Replace("\n", reemplazo).Replace("\r", reemplazo);
                estado = 0;
                con.Close();               
                return estado;
            }

        }


      

        public bool comando3(string to, string from, string subject, string message)
        {
            OracleConnection con;
            bool estados;
            string reemplazo = "";
            string mensajeestado;
            string cad = ConfigurationManager.ConnectionStrings["cx"].ConnectionString;

            con = new OracleConnection(cad);
            con.Open();
            cmd = new OracleCommand();
            cmd.Connection = con;
            cmd.CommandText = "SCHE$SISCO.SCCPGST$EMAIL.Enviar";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("prmTo", OracleDbType.Varchar2).Value = to;
            cmd.Parameters.Add("prmFrom", OracleDbType.Varchar2).Value = from;
            cmd.Parameters.Add("prmSubject", OracleDbType.Varchar2).Value = subject;
            cmd.Parameters.Add("prmMessage", OracleDbType.Varchar2).Value = message;
            cmd.Parameters.Add("prmSmtpHost", OracleDbType.Varchar2).Value = "segeplan-gob-gt.mail.protection.outlook.com";
            cmd.Parameters.Add("prmSmtpPort", OracleDbType.Int32).Value = 25;

            try
            {
                cmd.ExecuteNonQuery();
                mensaje = "Operación correcta";
                estados = true;
                con.Close();
                
                return estados;
            }
            catch (Exception e)
            {
                mensajeestado = e.Message;
                mensaje = mensajeestado.Replace("\r\n", reemplazo).Replace("\n", reemplazo).Replace("\r", reemplazo);
                estados = false;
                con.Close();
                
                return estados;
            }

        }

        public int comando4(string parquery)
        {
            string reemplazo = "";
            string mensajeestado;

            con = new OracleConnection(cad);
            con.Open();
            cmd = new OracleCommand(parquery, con);


            try
            {
                cmd.ExecuteNonQuery();
                mensaje = "Operación correcta";
                estado = 1;
                con.Close();
                OracleConnection.ClearAllPools(); // refuerzo
                return estado;
            }
            catch (Exception e)
            {
                mensajeestado = e.Message;
                mensaje = mensajeestado.Replace("\r\n", reemplazo).Replace("\n", reemplazo).Replace("\r", reemplazo);
                estado = 0;
                OracleConnection.ClearAllPools(); // refuerzo
                con.Close();
                return estado;
            }

        }
        public void salir()
        {

            con.Dispose();
        }

    }

   




    

}