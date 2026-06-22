using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web;
using SIPLAN2._0.DataAccess;

namespace SIPLAN2._0.pom
{
    public partial class correcciones : System.Web.UI.Page
    {
        int pom = -1;
        int insto = -1;
        int estado = -1;
        string sql = "";
        string mensaje = "";
        DataTable tabla = new DataTable();
        clsAccesoBBDD dao = new clsAccesoBBDD();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Usuario"] == null)
                Response.Redirect("../login/logout.aspx");
            if (Session["Usuario"].ToString() != "EDWINSON")
                Response.Redirect("../login/logout.aspx");
            {
                pom = 9708;
                insto = 7000;
                //cargaProductosResultadosInstitucionales(pom, insto);
                cargaSubproducto(pom, insto);
            }

        }


        protected void cargaSubproducto(int pom, int insto)
        {
            int orden = -1;
            DataTable poms = new DataTable();
            DataTable producto = new DataTable();
            DataTable resultado = new DataTable();
            DataTable subproductos = new DataTable();
            DataTable tempo = new DataTable();

            sql = "SELECT P.SPP$ORDEN FROM SCHE$SIPLAN20.SP20$PERIODO P INNER JOIN SCHE$SIPLAN20.SP20$POM PO ON P.SPP$ID_PERIODO = PO.SPPO$ID_PERIODO AND P.SPP$RESTRICTIVA = 'N' AND PO.SPPO$RESTRICTIVA = 'N' WHERE PO.SPPO$ID_POM = " + pom + " AND PO.SPPO$ID_INSTITUCION = " + insto;
            estado = dao.consulta(sql);
            if (estado == 1)
            {
                tabla = dao.tabla;
                if (tabla.Rows.Count > 0)
                {
                    if (Convert.ToInt32(tabla.Rows[0]["SPP$ORDEN"]) != 1)
                    {
                        orden = Convert.ToInt32(tabla.Rows[0]["SPP$ORDEN"]) - 1;
                        sql = "SELECT PO.SPPO$ID_POM, PO.SPPO$ID_INSTITUCION, PO.SPPO$ID_PERIODO FROM SCHE$SIPLAN20.SP20$PERIODO P INNER JOIN SCHE$SIPLAN20.SP20$POM PO ON P.SPP$ID_PERIODO = PO.SPPO$ID_PERIODO AND P.SPP$RESTRICTIVA = 'N' AND PO.SPPO$RESTRICTIVA = 'N' WHERE PO.SPPO$ID_INSTITUCION= " + insto + " AND P.SPP$ORDEN =" + orden;
                        estado = dao.consulta(sql);
                        if (estado == 1)
                        {
                            poms = dao.tabla;
                            {
                                if (poms.Rows.Count > 0)
                                {
                                    sql = "SELECT P.SPPRO$ID_PRODUCTO, R.SPRES$DESCRIPCION RESULTADO, P.SPPRO$DESCRIPCION, P.SPPRO$ID_MEDIDA, P.SPPRO$OBJETIVO_CENTRAL, P.SPPRO$ACCION_ESTRATEGICA, P.SPPRO$ID_RESULTADO, P.SPPRO$PRESUPUESTO, P.SPPRO$POM, P.SPPRO$INSTO, P.SPPRO$RESULTADO2 FROM SCHE$SIPLAN20.SP20$PRODUCTO P INNER JOIN SCHE$SIPLAN20.SP20$RESULTADOS R ON P.SPPRO$ID_RESULTADO = R.SPRES$ID_RESULTADO AND P.SPPRO$RESTRICTIVA = 'N' AND R.SPRES$RESTRICTIVA = 'N'  WHERE P.SPPRO$POM = " + poms.Rows[0]["SPPO$ID_POM"] + " AND P.SPPRO$INSTO = " + poms.Rows[0]["SPPO$ID_INSTITUCION"] + " AND SPPRO$ID_PRODUCTO IN (20063,20062,20069,20076,20058,20072,20061,20071,20078,20060,20067,20066,20073,20080)";
                                    estado = dao.consulta(sql);
                                    if (estado == 1)
                                    {
                                        producto = dao.tabla;
                                        if (producto.Rows.Count > 0)
                                        {
                                           /* for (int i = 0; i < producto.Rows.Count; i++)
                                            {
                                                sql = "SELECT * FROM SCHE$SIPLAN20.SP20$RESULTADOS WHERE SPRES$DESCRIPCION = '" + producto.Rows[i]["RESULTADO"] + "' AND SPRES$RESTRICTIVA = 'N' AND SPRES$POM = " + pom + " AND SPRES$INSTITUCION = " + insto + " AND SPRES$TIPO = 1 ";
                                                estado = dao.consulta(sql);
                                                if (estado == 1)
                                                {
                                                    resultado = dao.tabla;
                                                    if (resultado.Rows.Count > 0)
                                                    {
                                                        sql = "INSERT INTO SCHE$SIPLAN20.SP20$PRODUCTO (SPPRO$DESCRIPCION, SPPRO$ID_MEDIDA, SPPRO$OBJETIVO_CENTRAL, SPPRO$ACCION_ESTRATEGICA, SPPRO$ID_RESULTADO, SPPRO$FECHA_INSERTA, SPPRO$PRESUPUESTO, SPPRO$POM, SPPRO$INSTO,SPPRO$RESULTADO2) VALUES ('" + producto.Rows[i]["SPPRO$DESCRIPCION"] + "'," + producto.Rows[i]["SPPRO$ID_MEDIDA"];
                                                        if (producto.Rows[i]["SPPRO$OBJETIVO_CENTRAL"] == DBNull.Value)
                                                            sql = sql + ",NULL,NULL";
                                                        else
                                                            sql = sql + "," + producto.Rows[i]["SPPRO$OBJETIVO_CENTRAL"] + "," + producto.Rows[i]["SPPRO$ACCION_ESTRATEGICA"];
                                                        sql = sql + "," + resultado.Rows[0]["SPRES$ID_RESULTADO"] + ",'INSERT = '||TO_CHAR(SYSDATE,'DD/MM/YYYY HH:MI')||' '||'" + Session["USUARIO"].ToString() + "'," + producto.Rows[i]["SPPRO$PRESUPUESTO"] + "," + pom + "," + insto + "," + producto.Rows[i]["SPPRO$RESULTADO2"] + ")";
                                                        estado = dao.comando(sql);
                                                        if (estado == 0)
                                                            break;
                                                    }
                                                }
                                            }
                                            */
                                            if (estado == 1)
                                            {
                                                sql = "SELECT R.SPRES$ID_RESULTADO, R.SPRES$DESCRIPCION, PR.SPPRO$ID_PROGRAMA_PRESUPUESTO, PR.SPPRO$DESCRIPCION PRESUPUESTO, PO.SPPRO$ID_PRODUCTO, PO.SPPRO$DESCRIPCION PRODUCTO, PO.SPPRO$ID_MEDIDA, UM.SNCGUM$NOMBRE, PO.SPPRO$OBJETIVO_CENTRAL, PO.SPPRO$ACCION_ESTRATEGICA, ";
                                                sql = sql + "(SELECT R.SPPRES$DESCRIPCION FROM SCHE$SIPLAN20.SP20$RESULTADOS_ESTRATEGICOS R WHERE R.SPPRES$RESTRICTIVA ='N' AND R.SPPRES$ID_RESULTADO = PO.SPPRO$OBJETIVO_CENTRAL AND R.SPPRES$NIVEL = 3) OBJETIVO_SECTORIAL, ";
                                                sql = sql + "(SELECT R.SPPRES$DESCRIPCION FROM SCHE$SIPLAN20.SP20$RESULTADOS_ESTRATEGICOS R WHERE R.SPPRES$RESTRICTIVA ='N' AND R.SPPRES$ID_RESULTADO = PO.SPPRO$ACCION_ESTRATEGICA AND R.SPPRES$NIVEL = 4) ACCCION_ESTRATEGICA, ";
                                                sql = sql + "PO.SPPRO$PROPIETARIO, (SELECT CASE WHEN COUNT(M.NOMBRE) > 0 THEN 'SI' ELSE 'NO' END AS  MUNICIPIO FROM SINIP.CG_GEOGRAFICO M INNER JOIN SCHE$SIPLAN20.SP20$MUNOSPRIORIZADOS MP ON M.GEOGRAFICO = MP.GEOGRAFICO AND MP.RESTRICTIVA = 'N' WHERE MP.SPPRO$ID_PRODUCTO = PO.SPPRO$ID_PRODUCTO) MUNICIPIOS FROM SCHE$SIPLAN20.SP20$RESULTADOS R INNER JOIN SCHE$SIPLAN20.SP20$PRODUCTO PO ON R.SPRES$ID_RESULTADO = PO.SPPRO$ID_RESULTADO ";
                                                sql = sql + " AND R.SPRES$POM = PO.SPPRO$POM AND R.SPRES$INSTITUCION = PO.SPPRO$INSTO AND R.SPRES$RESTRICTIVA = 'N' AND PO.SPPRO$RESTRICTIVA = 'N' INNER JOIN SCHE$SIPLAN20.SP20$PROGRAMA_PRESUPUESTARIO PR ON PO.SPPRO$PRESUPUESTO = PR.SPPRO$ID_PROGRAMA_PRESUPUESTO AND PO.SPPRO$POM = PR.SPPRO$ID_POM AND PR.SPPRO$ID_INSTITUCION = PO.SPPRO$INSTO AND PR.SPPRO$RESTRICTIVA = 'N' AND PO.SPPRO$RESTRICTIVA = 'N'";
                                                sql = sql + " INNER JOIN SINIP.SNTBCG$UNIDAD_MEDIDA UM ON PO.SPPRO$ID_MEDIDA = UM.SNCGUM$UNIDAD_MEDIDA WHERE R.SPRES$POM = " + pom + " AND R.SPRES$INSTITUCION = " + insto + " AND PO.SPPRO$ID_PRODUCTO IN (24020,24021,24022,24023,24024,24026,24027,24030,24031,24032,24033,24034,24035,24036)   ORDER BY R.SPRES$ID_RESULTADO, PR.SPPRO$ID_PROGRAMA_PRESUPUESTO, PO.SPPRO$ID_PRODUCTO ASC";

                                                estado = dao.consulta(sql);
                                                if (estado == 0)
                                                {
                                                    mensaje = dao.mensaje;
                                                    Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
                                                }

                                                else
                                                {
                                                    tabla = dao.tabla;
                                                    if (tabla.Rows.Count > 0)
                                                    {
                                                        for (int i = 0; i < producto.Rows.Count; i++)
                                                        {
                                                            sql = "SELECT * FROM SCHE$SIPLAN20.SP20$SUB_PRODUCTO WHERE SPPSUB$ID_PRODUCTO = " + producto.Rows[i]["SPPRO$ID_PRODUCTO"] + " AND SPPSUB$RESTRICTIVA = 'N'";
                                                            estado = dao.consulta(sql);
                                                            if (estado == 1)
                                                            {
                                                                subproductos = dao.tabla;
                                                                if (subproductos.Rows.Count > 0)
                                                                {
                                                                    for (int j = 0; j < subproductos.Rows.Count; j++)
                                                                    {
                                                                        sql = "SELECT * FROM SCHE$SIPLAN20.SP20$PRODUCTO WHERE SPPRO$DESCRIPCION = '" + producto.Rows[i]["SPPRO$DESCRIPCION"] + "'  AND SPPRO$PRESUPUESTO = " + producto.Rows[i]["SPPRO$PRESUPUESTO"] + "  " +
                                                                               //"AND  SPPRO$ID_RESULTADO = "+tabla.Rows[i]["SPRES$ID_RESULTADO"] +" " +
                                                                               "AND SPPRO$POM = " + pom + " AND SPPRO$INSTO = " + insto + " AND SPPRO$RESTRICTIVA ='N'";
                                                                        estado = dao.consulta(sql);
                                                                        if (estado == 1)
                                                                        {
                                                                            tempo = dao.tabla;
                                                                            if (tempo.Rows.Count > 0)
                                                                            {
                                                                                sql = "INSERT INTO SCHE$SIPLAN20.SP20$SUB_PRODUCTO (SPPSUB$DESCRIPCION, SPPSUB$ID_MEDIDA, SPPSUB$SNIP, SPPSUB$ID_PRODUCTO,SPPSUB$FECHA_INSERTA,SPPSUB$PROPIETARIO, SPPSUB$ID_ANTERIOR) VALUES (";
                                                                                if (subproductos.Rows[j]["SPPSUB$SNIP"] == DBNull.Value)
                                                                                {
                                                                                    sql = sql + "'" + subproductos.Rows[j]["SPPSUB$DESCRIPCION"] + "'," + subproductos.Rows[j]["SPPSUB$ID_MEDIDA"] + ",NULL," + tempo.Rows[0]["SPPRO$ID_PRODUCTO"] + ",'INSERT = '||TO_CHAR(SYSDATE,'DD/MM/YYYY HH:MI')||' '||'" + Session["USUARIO"].ToString() + "', '" + Session["USUARIO"].ToString() + "',"+ subproductos.Rows[j]["SPPSUB$ID_SUBPRODUCTO"] + ")";
                                                                                }

                                                                                else
                                                                                {
                                                                                    sql = sql + "NULL,NULL," + subproductos.Rows[j]["SPPSUB$SNIP"] + "," + tempo.Rows[0]["SPPRO$ID_PRODUCTO"] + ",'INSERT = '||TO_CHAR(SYSDATE,'DD/MM/YYYY HH:MI')||' '||'" + Session["USUARIO"].ToString() + "', '" + Session["USUARIO"].ToString() + "', "+ subproductos.Rows[j]["SPPSUB$ID_SUBPRODUCTO"] + ")";
                                                                                }

                                                                                estado = dao.comando(sql);
                                                                                if (estado == 0)
                                                                                    break;
                                                                            }
                                                                        }
                                                                    }
                                                                }

                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

            }
        }
        protected void cargaProductosResultadosInstitucionales(int pom, int insto)
        {
            int orden = -1;
            DataTable poms = new DataTable();
            DataTable producto = new DataTable();
            DataTable resultado = new DataTable();
            DataTable subproductos = new DataTable();
            DataTable tempo = new DataTable();

            sql = "SELECT P.SPP$ORDEN FROM SCHE$SIPLAN20.SP20$PERIODO P INNER JOIN SCHE$SIPLAN20.SP20$POM PO ON P.SPP$ID_PERIODO = PO.SPPO$ID_PERIODO AND P.SPP$RESTRICTIVA = 'N' AND PO.SPPO$RESTRICTIVA = 'N' WHERE PO.SPPO$ID_POM = " + pom + " AND PO.SPPO$ID_INSTITUCION = " + insto;
            estado = dao.consulta(sql);
            if (estado == 1)
            {
                tabla = dao.tabla;
                if (tabla.Rows.Count > 0)
                {
                    if (Convert.ToInt32(tabla.Rows[0]["SPP$ORDEN"]) != 1)
                    {
                        orden = Convert.ToInt32(tabla.Rows[0]["SPP$ORDEN"]) - 1;
                        sql = "SELECT PO.SPPO$ID_POM, PO.SPPO$ID_INSTITUCION, PO.SPPO$ID_PERIODO FROM SCHE$SIPLAN20.SP20$PERIODO P INNER JOIN SCHE$SIPLAN20.SP20$POM PO ON P.SPP$ID_PERIODO = PO.SPPO$ID_PERIODO AND P.SPP$RESTRICTIVA = 'N' AND PO.SPPO$RESTRICTIVA = 'N' WHERE PO.SPPO$ID_INSTITUCION= " + insto + " AND P.SPP$ORDEN =" + orden;
                        estado = dao.consulta(sql);
                        if (estado == 1)
                        {
                            poms = dao.tabla;
                            {
                                if (poms.Rows.Count > 0)
                                {
                                    sql = "SELECT P.SPPRO$ID_PRODUCTO, R.SPRES$DESCRIPCION RESULTADO, P.SPPRO$DESCRIPCION, P.SPPRO$ID_MEDIDA, P.SPPRO$OBJETIVO_CENTRAL, P.SPPRO$ACCION_ESTRATEGICA, P.SPPRO$ID_RESULTADO, P.SPPRO$PRESUPUESTO, P.SPPRO$POM, P.SPPRO$INSTO, P.SPPRO$RESULTADO2 FROM SCHE$SIPLAN20.SP20$PRODUCTO P INNER JOIN SCHE$SIPLAN20.SP20$RESULTADOS R ON P.SPPRO$ID_RESULTADO = R.SPRES$ID_RESULTADO AND P.SPPRO$RESTRICTIVA = 'N' AND R.SPRES$RESTRICTIVA = 'N'  WHERE P.SPPRO$POM = " + poms.Rows[0]["SPPO$ID_POM"] + " AND P.SPPRO$INSTO = " + poms.Rows[0]["SPPO$ID_INSTITUCION"] + " AND R.SPRES$TIPO = 1 AND R.SPRES$ID_RESULTADO IN (6464,7603,6466)";
                                    estado = dao.consulta(sql);
                                    if (estado == 1)
                                    {
                                        producto = dao.tabla;
                                        if (producto.Rows.Count > 0)
                                        {
                                            for (int i = 0; i < producto.Rows.Count; i++)
                                            {
                                                sql = "SELECT * FROM SCHE$SIPLAN20.SP20$RESULTADOS WHERE SPRES$DESCRIPCION = '" + producto.Rows[i]["RESULTADO"] + "' AND SPRES$RESTRICTIVA = 'N' AND SPRES$POM = " + pom + " AND SPRES$INSTITUCION = " + insto + " AND SPRES$TIPO = 1 ";
                                                estado = dao.consulta(sql);
                                                if (estado == 1)
                                                {
                                                    resultado = dao.tabla;
                                                    if (resultado.Rows.Count > 0)
                                                    {
                                                        sql = "INSERT INTO SCHE$SIPLAN20.SP20$PRODUCTO (SPPRO$DESCRIPCION, SPPRO$ID_MEDIDA, SPPRO$OBJETIVO_CENTRAL, SPPRO$ACCION_ESTRATEGICA, SPPRO$ID_RESULTADO, SPPRO$FECHA_INSERTA, SPPRO$PRESUPUESTO, SPPRO$POM, SPPRO$INSTO,SPPRO$RESULTADO2) VALUES ('" + producto.Rows[i]["SPPRO$DESCRIPCION"] + "'," + producto.Rows[i]["SPPRO$ID_MEDIDA"];
                                                        if (producto.Rows[i]["SPPRO$OBJETIVO_CENTRAL"] == DBNull.Value)
                                                            sql = sql + ",NULL,NULL";
                                                        else
                                                            sql = sql + "," + producto.Rows[i]["SPPRO$OBJETIVO_CENTRAL"] + "," + producto.Rows[i]["SPPRO$ACCION_ESTRATEGICA"];
                                                        sql = sql + "," + resultado.Rows[0]["SPRES$ID_RESULTADO"] + ",'INSERT = '||TO_CHAR(SYSDATE,'DD/MM/YYYY HH:MI')||' '||'" + Session["USUARIO"].ToString() + "'," + producto.Rows[i]["SPPRO$PRESUPUESTO"] + "," + pom + "," + insto + "," + producto.Rows[i]["SPPRO$RESULTADO2"] + ")";
                                                        estado = dao.comando(sql);
                                                        if (estado == 0)
                                                            break;
                                                    }
                                                }
                                            }

                                            if (estado == 1)
                                            {
                                                sql = "SELECT R.SPRES$ID_RESULTADO, R.SPRES$DESCRIPCION, PR.SPPRO$ID_PROGRAMA_PRESUPUESTO, PR.SPPRO$DESCRIPCION PRESUPUESTO, PO.SPPRO$ID_PRODUCTO, PO.SPPRO$DESCRIPCION PRODUCTO, PO.SPPRO$ID_MEDIDA, UM.SNCGUM$NOMBRE, PO.SPPRO$OBJETIVO_CENTRAL, PO.SPPRO$ACCION_ESTRATEGICA, ";
                                                sql = sql + "(SELECT R.SPPRES$DESCRIPCION FROM SCHE$SIPLAN20.SP20$RESULTADOS_ESTRATEGICOS R WHERE R.SPPRES$RESTRICTIVA ='N' AND R.SPPRES$ID_RESULTADO = PO.SPPRO$OBJETIVO_CENTRAL AND R.SPPRES$NIVEL = 3) OBJETIVO_SECTORIAL, ";
                                                sql = sql + "(SELECT R.SPPRES$DESCRIPCION FROM SCHE$SIPLAN20.SP20$RESULTADOS_ESTRATEGICOS R WHERE R.SPPRES$RESTRICTIVA ='N' AND R.SPPRES$ID_RESULTADO = PO.SPPRO$ACCION_ESTRATEGICA AND R.SPPRES$NIVEL = 4) ACCCION_ESTRATEGICA, ";
                                                sql = sql + "PO.SPPRO$PROPIETARIO, (SELECT CASE WHEN COUNT(M.NOMBRE) > 0 THEN 'SI' ELSE 'NO' END AS  MUNICIPIO FROM SINIP.CG_GEOGRAFICO M INNER JOIN SCHE$SIPLAN20.SP20$MUNOSPRIORIZADOS MP ON M.GEOGRAFICO = MP.GEOGRAFICO AND MP.RESTRICTIVA = 'N' WHERE MP.SPPRO$ID_PRODUCTO = PO.SPPRO$ID_PRODUCTO) MUNICIPIOS FROM SCHE$SIPLAN20.SP20$RESULTADOS R INNER JOIN SCHE$SIPLAN20.SP20$PRODUCTO PO ON R.SPRES$ID_RESULTADO = PO.SPPRO$ID_RESULTADO ";
                                                sql = sql + " AND R.SPRES$POM = PO.SPPRO$POM AND R.SPRES$INSTITUCION = PO.SPPRO$INSTO AND R.SPRES$RESTRICTIVA = 'N' AND PO.SPPRO$RESTRICTIVA = 'N' INNER JOIN SCHE$SIPLAN20.SP20$PROGRAMA_PRESUPUESTARIO PR ON PO.SPPRO$PRESUPUESTO = PR.SPPRO$ID_PROGRAMA_PRESUPUESTO AND PO.SPPRO$POM = PR.SPPRO$ID_POM AND PR.SPPRO$ID_INSTITUCION = PO.SPPRO$INSTO AND PR.SPPRO$RESTRICTIVA = 'N' AND PO.SPPRO$RESTRICTIVA = 'N'";
                                                sql = sql + " INNER JOIN SINIP.SNTBCG$UNIDAD_MEDIDA UM ON PO.SPPRO$ID_MEDIDA = UM.SNCGUM$UNIDAD_MEDIDA WHERE R.SPRES$POM = " + pom + " AND R.SPRES$INSTITUCION = " + insto + " AND R.SPRES$TIPO = 1 ORDER BY R.SPRES$ID_RESULTADO, PR.SPPRO$ID_PROGRAMA_PRESUPUESTO, PO.SPPRO$ID_PRODUCTO ASC";

                                                estado = dao.consulta(sql);
                                                if (estado == 0)
                                                {
                                                    mensaje = dao.mensaje;
                                                    Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
                                                }

                                                else
                                                {
                                                    tabla = dao.tabla;
                                                    if (tabla.Rows.Count > 0)
                                                    {
                                                        for (int i = 0; i < producto.Rows.Count; i++)
                                                        {
                                                            sql = "SELECT * FROM SCHE$SIPLAN20.SP20$SUB_PRODUCTO WHERE SPPSUB$ID_PRODUCTO = " + producto.Rows[i]["SPPRO$ID_PRODUCTO"] + " AND SPPSUB$RESTRICTIVA = 'N'";
                                                            estado = dao.consulta(sql);
                                                            if (estado == 1)
                                                            {
                                                                subproductos = dao.tabla;
                                                                if (subproductos.Rows.Count > 0)
                                                                {
                                                                    for (int j = 0; j < subproductos.Rows.Count; j++)
                                                                    {
                                                                        sql = "SELECT * FROM SCHE$SIPLAN20.SP20$PRODUCTO WHERE SPPRO$DESCRIPCION = '" + producto.Rows[i]["SPPRO$DESCRIPCION"] + "'  AND SPPRO$PRESUPUESTO = " + producto.Rows[i]["SPPRO$PRESUPUESTO"] + "  " +
                                                                               //"AND  SPPRO$ID_RESULTADO = "+tabla.Rows[i]["SPRES$ID_RESULTADO"] +" " +
                                                                               "AND SPPRO$POM = " + pom + " AND SPPRO$INSTO = " + insto + " AND SPPRO$RESTRICTIVA ='N'";
                                                                        estado = dao.consulta(sql);
                                                                        if (estado == 1)
                                                                        {
                                                                            tempo = dao.tabla;
                                                                            if (tempo.Rows.Count > 0)
                                                                            {
                                                                                sql = "INSERT INTO SCHE$SIPLAN20.SP20$SUB_PRODUCTO (SPPSUB$DESCRIPCION, SPPSUB$ID_MEDIDA, SPPSUB$SNIP, SPPSUB$ID_PRODUCTO,SPPSUB$FECHA_INSERTA,SPPSUB$PROPIETARIO) VALUES (";
                                                                                if (subproductos.Rows[j]["SPPSUB$SNIP"] == DBNull.Value)
                                                                                {
                                                                                    sql = sql + "'" + subproductos.Rows[j]["SPPSUB$DESCRIPCION"] + "'," + subproductos.Rows[j]["SPPSUB$ID_MEDIDA"] + ",NULL," + tempo.Rows[0]["SPPRO$ID_PRODUCTO"] + ",'INSERT = '||TO_CHAR(SYSDATE,'DD/MM/YYYY HH:MI')||' '||'" + Session["USUARIO"].ToString() + "', '" + Session["USUARIO"].ToString() + "')";
                                                                                }

                                                                                else
                                                                                {
                                                                                    sql = sql + "NULL,NULL," + subproductos.Rows[j]["SPPSUB$SNIP"] + "," + tempo.Rows[0]["SPPRO$ID_PRODUCTO"] + ",'INSERT = '||TO_CHAR(SYSDATE,'DD/MM/YYYY HH:MI')||' '||'" + Session["USUARIO"].ToString() + "', '" + Session["USUARIO"].ToString() + "')";
                                                                                }

                                                                                estado = dao.comando(sql);
                                                                                if (estado == 0)
                                                                                    break;
                                                                            }
                                                                        }
                                                                    }
                                                                }

                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

            }

        }
    }
}