using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using SIPLAN2._0.DataAccess;

namespace SIPLAN2._0.Rutas
{
    public class Rutas
    {
        clsAccesoBBDD dao = new clsAccesoBBDD();
        string sql = "";
        int estado = 0;
        DataTable tabla = new DataTable();

        public string carpetas()
        {
            sql = "SELECT SPV$ENLACE FROM SCHE$SIPLAN20.SP20$CONFIGURACION_VARIAS WHERE SPV$DESCRIPCION = 'CARPETA DOCUMENTOS' AND SPV$RESTRICTIVA = 'N'";
            estado = dao.consulta(sql);
            tabla = dao.tabla;
            if (estado == 1)
            {
                if (tabla.Rows.Count > 0)

                    Caminos.camino = tabla.Rows[0]["SPV$ENLACE"].ToString();

                else

                    Caminos.camino = "";

            }

            else
                Caminos.camino = "";

            //Caminos.camino = "../../DocumentosCapa/";
            return Caminos.camino;

        }

        public string full_path()
        {
            sql = "SELECT SPV$ENLACE FROM SCHE$SIPLAN20.SP20$CONFIGURACION_VARIAS WHERE SPV$DESCRIPCION = 'FULL CARPETA DOCUMENTOS' AND SPV$RESTRICTIVA = 'N'";
            estado = dao.consulta(sql);
            tabla = dao.tabla;
            if (estado == 1)
            {
                if (tabla.Rows.Count > 0)
                    Caminos.camino = tabla.Rows[0]["SPV$ENLACE"].ToString();
                else
                    Caminos.camino = "";
            }
            else
                Caminos.camino = "";
            //Caminos.camino = "E:\\Documentos\\";
            //Caminos.camino = "E:\\DocumentosCapa\\";
            return Caminos.camino;


        }

        public string carpetas_resolucion()
        {
            sql = "SELECT SPV$ENLACE FROM SCHE$SIPLAN20.SP20$CONFIGURACION_VARIAS WHERE SPV$DESCRIPCION = 'CARPETA RESOLUCIONES' AND SPV$RESTRICTIVA = 'N'";

            estado = dao.consulta(sql);
            tabla = dao.tabla;
            if (estado == 1)
            {
                if (tabla.Rows.Count > 0)
                    Caminos.camino = tabla.Rows[0]["SPV$ENLACE"].ToString();
                else
                    Caminos.camino = "";
            }
            else
                Caminos.camino = "";
            //Caminos.camino = "../../Documentos/resoluciones/";
            //Caminos.camino = "../../DocumentosCapa/resoluciones/";
            return Caminos.camino;

        }

        public string full_path_resoluciones()
        {

            sql = "SELECT SPV$ENLACE FROM SCHE$SIPLAN20.SP20$CONFIGURACION_VARIAS WHERE SPV$DESCRIPCION = 'FULL CARPETA RESOLUCIONES ' AND SPV$RESTRICTIVA = 'N'";

            estado = dao.consulta(sql);
            tabla = dao.tabla;
            if (estado == 1)
            {
                if (tabla.Rows.Count > 0)
                    Caminos.camino = tabla.Rows[0]["SPV$ENLACE"].ToString();
                else
                    Caminos.camino = "";
            }
            else
                Caminos.camino = "";
            //Caminos.camino = "E:\\Documentos\\resoluciones\\";
            //Caminos.camino = "E:\\DocumentosCapa\\resoluciones\\";
            return Caminos.camino;


        }
    }
}