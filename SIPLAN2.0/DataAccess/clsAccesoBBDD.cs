using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Data;
using System.Configuration;
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

        private void CleanExMessage(Exception e)
        {
            string reemplazo = "";
            string mensajeestado = e.Message;
            mensaje = mensajeestado.Replace("\r\n", reemplazo).Replace("\n", reemplazo).Replace("\r", reemplazo);
        }

        public int comando(string parquery)
        {
            return comando(parquery, null);
        }

        public int comando(string parquery, OracleParameter[] parameters)
        {
            try
            {
                using (con = new OracleConnection(cad))
                {
                    con.Open();
                    using (cmd = new OracleCommand(parquery, con))
                    {
                        if (parameters != null)
                        {
                            cmd.Parameters.AddRange(parameters);
                        }
                        cmd.ExecuteNonQuery();
                        mensaje = "Operación correcta";
                        estado = 1;
                        return estado;
                    }
                }
            }
            catch (Exception e)
            {
                CleanExMessage(e);
                estado = 0;
                return estado;
            }
        }

        public int consulta(string parquery)
        {
            return consulta(parquery, null);
        }

        public int consulta(string parquery, OracleParameter[] parameters)
        {
            try
            {
                using (con = new OracleConnection(cad))
                {
                    con.Open();
                    using (ada = new OracleDataAdapter(parquery, con))
                    {
                        if (parameters != null)
                        {
                            ada.SelectCommand.Parameters.AddRange(parameters);
                        }
                        ds = new DataSet();
                        ada.Fill(ds, "CONSULTA");
                        mensaje = "Operación Correcta";
                        tabla = ds.Tables[0];
                        estado = 1;
                        return estado;
                    }
                }
            }
            catch (Exception e)
            {
                CleanExMessage(e);
                estado = 0;
                return estado;
            }
        }

        public int comando2(string parquery)
        {
            return comando(parquery);
        }

        public int comando2(string parquery, OracleParameter[] parameters)
        {
            return comando(parquery, parameters);
        }

        public bool comando3(string to, string from, string subject, string message)
        {
            bool estados;
            string cadLocal = ConfigurationManager.ConnectionStrings["cx"].ConnectionString;

            try
            {
                using (OracleConnection localCon = new OracleConnection(cadLocal))
                {
                    localCon.Open();
                    using (OracleCommand localCmd = new OracleCommand())
                    {
                        localCmd.Connection = localCon;
                        localCmd.CommandText = "SCHE$SISCO.SCCPGST$EMAIL.Enviar";
                        localCmd.CommandType = CommandType.StoredProcedure;
                        localCmd.Parameters.Add("prmTo", OracleDbType.Varchar2).Value = to;
                        localCmd.Parameters.Add("prmFrom", OracleDbType.Varchar2).Value = from;
                        localCmd.Parameters.Add("prmSubject", OracleDbType.Varchar2).Value = subject;
                        localCmd.Parameters.Add("prmMessage", OracleDbType.Varchar2).Value = message;
                        localCmd.Parameters.Add("prmSmtpHost", OracleDbType.Varchar2).Value = "segeplan-gob-gt.mail.protection.outlook.com";
                        localCmd.Parameters.Add("prmSmtpPort", OracleDbType.Int32).Value = 25;

                        localCmd.ExecuteNonQuery();
                        mensaje = "Operación correcta";
                        estados = true;
                        return estados;
                    }
                }
            }
            catch (Exception e)
            {
                CleanExMessage(e);
                estados = false;
                return estados;
            }
        }

        public int comando4(string parquery)
        {
            return comando4(parquery, null);
        }

        public int comando4(string parquery, OracleParameter[] parameters)
        {
            try
            {
                int res = comando(parquery, parameters);
                OracleConnection.ClearAllPools(); // refuerzo
                return res;
            }
            catch (Exception e)
            {
                CleanExMessage(e);
                estado = 0;
                OracleConnection.ClearAllPools(); // refuerzo
                return estado;
            }
        }

        public void salir()
        {
            if (con != null)
            {
                con.Dispose();
                con = null;
            }
        }
    }
}