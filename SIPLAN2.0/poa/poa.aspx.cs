using DevExpress.Web;
using SIPLAN2._0.DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SIPLAN2._0.poa
{
    public partial class poa1 : System.Web.UI.Page
    {
        string sql;
        int estado;
        string mensaje;
        clsAccesoBBDD dao = new clsAccesoBBDD();
        DataTable tabla = new DataTable();
        DataTable temp = new DataTable();
        DataTable periodo = new DataTable();
        DataTable poass = new DataTable();
        int numero_productos = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            txtPrecio.Attributes.Add("onblur", "suma(this);");

            txtCUA1.Attributes.Add("Onblur", "sumaCuat(this);");
            txtCUA2.Attributes.Add("Onblur", "sumaCuat(this);");
            txtCUA3.Attributes.Add("Onblur", "sumaCuat(this);");

            Session["VINCULADOS"] = 0;

            if (Session["Usuario"] == null)
                Response.Redirect("../login/login.aspx");
            else
            {
                if (Session["insto"] == null)
                {
                    Response.Redirect("../login/login.aspx");
                }
                else
                {
                    if (Session["ROL"].ToString() == "ENTIDAD" || (Session["abierto"] != null && Convert.ToInt32(Session["abierto"]) == 1))
                        bntEnviar.Enabled = false;




                    //sql = "SELECT P.SPP$ID_PERIODO, P.SPP$INICIO, P.SPP$FINAL, POM.SPPO$ID_POM, POM.SPPO$ID_INSTITUCION,P.SPP$ORDEN, CASE WHEN  POM.SPPO$ESTADO = 'D' THEN 'POM No enviado' ELSE 'POM Enviado' END AS ESTADO, P.SPP$ABIERTO FROM SCHE$SIPLAN20.SP20$PERIODO P INNER JOIN SCHE$SIPLAN20.SP20$POM POM ON P.SPP$ID_PERIODO = POM.SPPO$ID_PERIODO AND P.SPP$RESTRICTIVA = 'N' AND POM.SPPO$RESTRICTIVA = 'N' WHERE POM.SPPO$ID_POM = " + Convert.ToInt32(Session["pom"]) + " AND POM.SPPO$ID_INSTITUCION = " + Convert.ToInt32(Convert.ToInt32(Session["insto"]));
                    sql = @"SELECT
                            SPP$ID_PERIODO
                            ,SPP$INICIO
                            ,SPP$FINAL
                            ,SPPO$ID_POM
                            ,SPPO$ID_INSTITUCION
                            ,CASE WHEN SPPO$ESTADO = 'A' THEN 'Programación cerrada' 
                                  WHEN SPPO$ESTADO = 'D' AND FECHA_PRORROGA IS NOT NULL AND TRUNC(SYSDATE) <= TRUNC(FECHA_PRORROGA) THEN 'Programación abierta'
                                  WHEN  SPPO$ESTADO = 'D' AND SYSDATE <= FECHA_CIERRE then 'Programación abierta' 
                                  WHEN  SPPO$ESTADO = 'D' AND SYSDATE > FECHA_CIERRE THEN  'Programación cerrada' 
                                  WHEN SPPO$ESTADO = 'D' AND FECHA_CIERRE IS NULL THEN  'Programación cerrada'  
                             END AS ESTADO
                            ,SPP$ABIERTO 
                            ,SPP$ORDEN
                            FROM
                            (SELECT P.SPP$ID_PERIODO
                                    ,P.SPP$INICIO
                                    ,P.SPP$FINAL
                                    ,POM.SPPO$ID_POM
                                    ,POM.SPPO$ID_INSTITUCION                                    
                                    ,(SELECT SPFC$FECHA_CIERRE FROM SCHE$SIPLAN20.SP20$FECHAS_CIERRE WHERE SPFC$TIPO_FECHA = 2 AND SPFC$RESTRICTIVA = 'N' AND SPFC$PERIODO_POM = POM.SPPO$ID_PERIODO AND SPFC$POM = -1 AND SPFC$INSTITUCION = -1) FECHA_CIERRE
                                    ,(SELECT MAX(SPFC$FECHA_CIERRE) FECHA_CIERRA FROM SCHE$SIPLAN20.SP20$FECHAS_CIERRE WHERE SPFC$TIPO_FECHA = 2 AND SPFC$RESTRICTIVA = 'N' AND SPFC$PERIODO_POM = POM.SPPO$ID_PERIODO AND SPFC$POM = POM.SPPO$ID_POM AND SPFC$INSTITUCION = POM.SPPO$ID_INSTITUCION) FECHA_PRORROGA
                                    ,P.SPP$ABIERTO 
                                    ,POM.SPPO$ESTADO
                                    ,P.SPP$ORDEN                   
                                    FROM SCHE$SIPLAN20.SP20$PERIODO P 
                                    INNER JOIN SCHE$SIPLAN20.SP20$POM POM ON P.SPP$ID_PERIODO = POM.SPPO$ID_PERIODO AND P.SPP$RESTRICTIVA = 'N' AND POM.SPPO$RESTRICTIVA = 'N' WHERE POM.SPPO$ID_POM = " + Convert.ToInt32(Session["pom"]) + " AND POM.SPPO$ID_INSTITUCION = "+ Convert.ToInt32(Convert.ToInt32(Session["insto"])) + ")"; 





                    estado = dao.consulta(sql);
                    if (estado == 0)
                    {
                        mensaje = dao.mensaje;
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
                    }

                    else
                    {
                        periodo = dao.tabla;
                        Session["periodo"] = periodo.Rows[0]["SPP$ID_PERIODO"];
                        Session["abierto"] = periodo.Rows[0]["SPP$ABIERTO"];
                        if (Convert.ToInt32(periodo.Rows[0]["SPP$ID_PERIODO"]) == 0)
                        {
                            sql = "SELECT CASE WHEN SPOA$ESTADO = 'D' THEN 0 WHEN SPOA$ESTADO = 'A' THEN 1 END AS ESTADO FROM SCHE$SIPLAN20.SP20$POA WHERE SPOA$ID_POM = " + periodo.Rows[0]["SPPO$ID_POM"] + " AND SPOA$RESTRICTIVA = 'N' AND SPOA$ID_INSTITUCION = " + periodo.Rows[0]["SPPO$ID_INSTITUCION"] + " AND SPOA$ANIO = 2020";
                            estado = dao.consulta(sql);
                            poass = dao.tabla;



                        }

                        Session["SPPO$ID_POM"] = periodo.Rows[0]["SPPO$ID_POM"];
                        Session["SPPO$ID_INSTITUCION"] = periodo.Rows[0]["SPPO$ID_INSTITUCION"];
                        Session["SPP$ORDEN"] = periodo.Rows[0]["SPP$ORDEN"];
                        Session["INICIO"] = periodo.Rows[0]["SPP$INICIO"];
                        Session["FINAL"] = periodo.Rows[0]["SPP$FINAL"];


                        if (Convert.ToInt32(periodo.Rows[0]["SPP$ID_PERIODO"]) == 0)
                        {
                            if (Convert.ToInt32(poass.Rows[0]["ESTADO"]) == 0)
                                lblTitulo.Text = Session["institucion"].ToString() + " " + "periodo: " + periodo.Rows[0]["SPP$INICIO"] + " - " + periodo.Rows[0]["SPP$FINAL"] + " POA NO ENVIADO";
                            else if (Convert.ToInt32(poass.Rows[0]["ESTADO"]) == 1)
                                lblTitulo.Text = Session["institucion"].ToString() + " " + "periodo: " + periodo.Rows[0]["SPP$INICIO"] + " - " + periodo.Rows[0]["SPP$FINAL"] + " POA ENVIADO";

                            gvProgramas.Columns["PRESUPUESTO2025"].Visible = false;
                            gvProduccion.Columns["PR0GRAMADO2025"].Visible = false;
                            gvProduccion.Columns["PRESUPUESTO2025"].Visible = false;
                            GridViewBandColumn bandCol = gvProduccion.Columns["CLASPRES2025"] as GridViewBandColumn;
                            bandCol.Visible = false;


                            gvProgramas.Columns["PRESUPUESTO2026"].Visible = false;
                            gvProduccion.Columns["PR0GRAMADO2026"].Visible = false;
                            gvProduccion.Columns["PRESUPUESTO2026"].Visible = false;
                            GridViewBandColumn bandCol2 = gvProduccion.Columns["CLASPRES2026"] as GridViewBandColumn;
                            bandCol2.Visible = false;


                            gvProgramas.Columns["PRESUPUESTO2027"].Visible = false;
                            gvProduccion.Columns["PR0GRAMADO2027"].Visible = false;
                            gvProduccion.Columns["PRESUPUESTO2027"].Visible = false;
                            GridViewBandColumn bandCol3 = gvProduccion.Columns["CLASPRES2027"] as GridViewBandColumn;
                            bandCol3.Visible = false;


                            gvProgramas.Columns["PRESUPUESTO2028"].Visible = false;
                            gvProduccion.Columns["PR0GRAMADO2028"].Visible = false;
                            gvProduccion.Columns["PRESUPUESTO2028"].Visible = false;
                            GridViewBandColumn bandCol4 = gvProduccion.Columns["CLASPRES2028"] as GridViewBandColumn;
                            bandCol4.Visible = false;

                            gvProgramas.Columns["PRESUPUESTO2029"].Visible = false;
                            gvProduccion.Columns["PR0GRAMADO2029"].Visible = false;
                            gvProduccion.Columns["PRESUPUESTO2029"].Visible = false;
                            GridViewBandColumn bandCol5 = gvProduccion.Columns["CLASPRES2029"] as GridViewBandColumn;
                            bandCol5.Visible = false;

                            gvProgramas.Columns["PRESUPUESTO2030"].Visible = false;
                            gvProduccion.Columns["PR0GRAMADO2030"].Visible = false;
                            gvProduccion.Columns["PRESUPUESTO2030"].Visible = false;
                            GridViewBandColumn bandCol6 = gvProduccion.Columns["CLASPRES2030"] as GridViewBandColumn;
                            bandCol6.Visible = false;


                        }

                        else
                        {
                            if (Convert.ToInt32(periodo.Rows[0]["SPP$ID_PERIODO"]) == 20)
                            {
                                gvProgramas.Columns["PRESUPUESTO2020"].Visible = false;
                                gvProduccion.Columns["PR0GRAMADO2020"].Visible = false;
                                gvProduccion.Columns["PRESUPUESTO2020"].Visible = false;
                                GridViewBandColumn bandCol = gvProduccion.Columns["CLASPRES2020"] as GridViewBandColumn;
                                bandCol.Visible = false;


                                gvProgramas.Columns["PRESUPUESTO2026"].Visible = false;
                                gvProduccion.Columns["PR0GRAMADO2026"].Visible = false;
                                gvProduccion.Columns["PRESUPUESTO2026"].Visible = false;
                                GridViewBandColumn bandCol2 = gvProduccion.Columns["CLASPRES2026"] as GridViewBandColumn;
                                bandCol2.Visible = false;

                                gvProgramas.Columns["PRESUPUESTO2027"].Visible = false;
                                gvProduccion.Columns["PR0GRAMADO2027"].Visible = false;
                                gvProduccion.Columns["PRESUPUESTO2027"].Visible = false;
                                GridViewBandColumn bandCol3 = gvProduccion.Columns["CLASPRES2027"] as GridViewBandColumn;
                                bandCol3.Visible = false;

                                gvProgramas.Columns["PRESUPUESTO2028"].Visible = false;
                                gvProduccion.Columns["PR0GRAMADO2028"].Visible = false;
                                gvProduccion.Columns["PRESUPUESTO2028"].Visible = false;
                                GridViewBandColumn bandCol4 = gvProduccion.Columns["CLASPRES2028"] as GridViewBandColumn;
                                bandCol4.Visible = false;

                                gvProgramas.Columns["PRESUPUESTO2029"].Visible = false;
                                gvProduccion.Columns["PR0GRAMADO2029"].Visible = false;
                                gvProduccion.Columns["PRESUPUESTO2029"].Visible = false;
                                GridViewBandColumn bandCol5 = gvProduccion.Columns["CLASPRES2029"] as GridViewBandColumn;
                                bandCol5.Visible = false;

                                gvProgramas.Columns["PRESUPUESTO2030"].Visible = false;
                                gvProduccion.Columns["PR0GRAMADO2030"].Visible = false;
                                gvProduccion.Columns["PRESUPUESTO2030"].Visible = false;
                                GridViewBandColumn bandCol6 = gvProduccion.Columns["CLASPRES2030"] as GridViewBandColumn;
                                bandCol6.Visible = false;

                            }

                            else if (Convert.ToInt32(periodo.Rows[0]["SPP$ID_PERIODO"]) == 21)
                            {
                                gvProgramas.Columns["PRESUPUESTO2020"].Visible = false;
                                gvProduccion.Columns["PR0GRAMADO2020"].Visible = false;
                                gvProduccion.Columns["PRESUPUESTO2020"].Visible = false;
                                GridViewBandColumn bandCol = gvProduccion.Columns["CLASPRES2020"] as GridViewBandColumn;
                                bandCol.Visible = false;


                                gvProgramas.Columns["PRESUPUESTO2021"].Visible = false;
                                gvProduccion.Columns["PR0GRAMADO2021"].Visible = false;
                                gvProduccion.Columns["PRESUPUESTO2021"].Visible = false;
                                GridViewBandColumn bandCol2 = gvProduccion.Columns["CLASPRES2021"] as GridViewBandColumn;
                                bandCol2.Visible = false;

                                gvProgramas.Columns["PRESUPUESTO2027"].Visible = false;
                                gvProduccion.Columns["PR0GRAMADO2027"].Visible = false;
                                gvProduccion.Columns["PRESUPUESTO2027"].Visible = false;
                                GridViewBandColumn bandCol3 = gvProduccion.Columns["CLASPRES2027"] as GridViewBandColumn;
                                bandCol3.Visible = false;

                                gvProgramas.Columns["PRESUPUESTO2028"].Visible = false;
                                gvProduccion.Columns["PR0GRAMADO2028"].Visible = false;
                                gvProduccion.Columns["PRESUPUESTO2028"].Visible = false;
                                GridViewBandColumn bandCol4 = gvProduccion.Columns["CLASPRES2028"] as GridViewBandColumn;
                                bandCol4.Visible = false;

                                gvProgramas.Columns["PRESUPUESTO2029"].Visible = false;
                                gvProduccion.Columns["PR0GRAMADO2029"].Visible = false;
                                gvProduccion.Columns["PRESUPUESTO2029"].Visible = false;
                                GridViewBandColumn bandCol5 = gvProduccion.Columns["CLASPRES2029"] as GridViewBandColumn;
                                bandCol5.Visible = false;


                                gvProgramas.Columns["PRESUPUESTO2030"].Visible = false;
                                gvProduccion.Columns["PR0GRAMADO2030"].Visible = false;
                                gvProduccion.Columns["PRESUPUESTO2030"].Visible = false;
                                GridViewBandColumn bandCol6 = gvProduccion.Columns["CLASPRES2030"] as GridViewBandColumn;
                                bandCol6.Visible = false;

                            }


                            else if (Convert.ToInt32(periodo.Rows[0]["SPP$ID_PERIODO"]) == 22)
                            {
                                gvProgramas.Columns["PRESUPUESTO2020"].Visible = false;
                                gvProduccion.Columns["PR0GRAMADO2020"].Visible = false;
                                gvProduccion.Columns["PRESUPUESTO2020"].Visible = false;
                                GridViewBandColumn bandCol = gvProduccion.Columns["CLASPRES2020"] as GridViewBandColumn;
                                bandCol.Visible = false;


                                gvProgramas.Columns["PRESUPUESTO2021"].Visible = false;
                                gvProduccion.Columns["PR0GRAMADO2021"].Visible = false;
                                gvProduccion.Columns["PRESUPUESTO2021"].Visible = false;
                                GridViewBandColumn bandCol2 = gvProduccion.Columns["CLASPRES2021"] as GridViewBandColumn;
                                bandCol2.Visible = false;

                                gvProgramas.Columns["PRESUPUESTO2022"].Visible = false;
                                gvProduccion.Columns["PR0GRAMADO2022"].Visible = false;
                                gvProduccion.Columns["PRESUPUESTO2022"].Visible = false;
                                GridViewBandColumn bandCol3 = gvProduccion.Columns["CLASPRES2022"] as GridViewBandColumn;
                                bandCol3.Visible = false;

                                gvProgramas.Columns["PRESUPUESTO2028"].Visible = false;
                                gvProduccion.Columns["PR0GRAMADO2028"].Visible = false;
                                gvProduccion.Columns["PRESUPUESTO2028"].Visible = false;
                                GridViewBandColumn bandCol4 = gvProduccion.Columns["CLASPRES2028"] as GridViewBandColumn;
                                bandCol4.Visible = false;

                                gvProgramas.Columns["PRESUPUESTO2029"].Visible = false;
                                gvProduccion.Columns["PR0GRAMADO2029"].Visible = false;
                                gvProduccion.Columns["PRESUPUESTO2029"].Visible = false;
                                GridViewBandColumn bandCol5 = gvProduccion.Columns["CLASPRES2029"] as GridViewBandColumn;
                                bandCol5.Visible = false;

                                gvProgramas.Columns["PRESUPUESTO2030"].Visible = false;
                                gvProduccion.Columns["PR0GRAMADO2030"].Visible = false;
                                gvProduccion.Columns["PRESUPUESTO2030"].Visible = false;
                                GridViewBandColumn bandCol6 = gvProduccion.Columns["CLASPRES2030"] as GridViewBandColumn;
                                bandCol6.Visible = false;

                            }

                            else if (Convert.ToInt32(periodo.Rows[0]["SPP$ID_PERIODO"]) == 23)
                            {
                                sql = @"SELECT COUNT(*) CONTEO FROM SCHE$SIPLAN20.SP20$PRODUCTO P 
                                        INNER JOIN SCHE$SIPLAN20.SP20$RESULTADOS R ON P.SPPRO$ID_RESULTADO = R.SPRES$ID_RESULTADO AND P.SPPRO$RESTRICTIVA = 'N' AND R.SPRES$RESTRICTIVA = 'N'
                                        INNER JOIN SCHE$SIPLAN20.SP20$RESULTADOS_ESTRATEGICOS ER ON ER.SPPRES$ID_RESULTADO = R.SPRES$COD_ESTRATEGICO AND ER.SPPRES$RESTRICTIVA = 'N' AND R.SPRES$RESTRICTIVA = 'N'
                                        INNER JOIN SCHE$SIPLAN20.SP20$RESULTADOS_ESTRATEGICOS PGG ON ER.SPPRES$DEPENDE = PGG.SPPRES$ID_RESULTADO AND ER.SPPRES$RESTRICTIVA = 'N' AND PGG.SPPRES$RESTRICTIVA = 'N'
                                        INNER JOIN SCHE$SIPLAN20.S20$PERIODO_RESULTADO PR ON PR.SPR$ID_RESULTADO = PGG.SPPRES$ID_RESULTADO AND PGG.SPPRES$RESTRICTIVA = 'N' AND PR.SPR$RESTRICTIVA = 'N'
                                        INNER JOIN SCHE$SIPLAN20.SP20$PERIODO_GOBIERNO PG ON PG.SPG$ID_PERIODO = PR.SPR$ID_PERIODO AND PG.SPG$RESTRICTIVA = 'N' AND PR.SPR$RESTRICTIVA = 'N'
                                        INNER JOIN SCHE$SIPLAN20.SP20$POM POM ON POM.SPPO$ID_POM = P.SPPRO$POM AND P.SPPRO$RESTRICTIVA = 'N' AND POM.SPPO$RESTRICTIVA = 'N'
                                        WHERE PG.SPG$ID_PERIODO = 0 AND POM.SPPO$ID_PERIODO = " + Convert.ToInt32(periodo.Rows[0]["SPP$ID_PERIODO"]) + @" AND POM.SPPO$ID_POM = " + Convert.ToInt32(Session["pom"]) + " AND R.SPRES$TIPO = 0";

                                estado = dao.consulta(sql);

                                if (estado == 1)
                                    tabla = dao.tabla;

                                if (tabla.Rows.Count > 0)
                                    numero_productos = Convert.ToInt32(tabla.Rows[0]["CONTEO"]);
                                else numero_productos = 0;

                                if (numero_productos > 0)
                                {
                                    Session["VINCULADOS"] = numero_productos;
                                    mensaje = "Tiene " + numero_productos + " productos vinculados a la anterior estructura de programa de gobierno 2020-2024, se recomienda actualizar la vinculación de estos productos a la estructura vigente de programa de gobierno 2024-2028, a un Resultado Estrátegico y/o resultado institucional";
                                    Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',3);", true);
                                }



                                gvProgramas.Columns["PRESUPUESTO2020"].Visible = false;
                                gvProduccion.Columns["PR0GRAMADO2020"].Visible = false;
                                gvProduccion.Columns["PRESUPUESTO2020"].Visible = false;
                                GridViewBandColumn bandCol = gvProduccion.Columns["CLASPRES2020"] as GridViewBandColumn;
                                bandCol.Visible = false;


                                gvProgramas.Columns["PRESUPUESTO2021"].Visible = false;
                                gvProduccion.Columns["PR0GRAMADO2021"].Visible = false;
                                gvProduccion.Columns["PRESUPUESTO2021"].Visible = false;
                                GridViewBandColumn bandCol2 = gvProduccion.Columns["CLASPRES2021"] as GridViewBandColumn;
                                bandCol2.Visible = false;

                                gvProgramas.Columns["PRESUPUESTO2022"].Visible = false;
                                gvProduccion.Columns["PR0GRAMADO2022"].Visible = false;
                                gvProduccion.Columns["PRESUPUESTO2022"].Visible = false;
                                GridViewBandColumn bandCol3 = gvProduccion.Columns["CLASPRES2022"] as GridViewBandColumn;
                                bandCol3.Visible = false;

                                gvProgramas.Columns["PRESUPUESTO2023"].Visible = false;
                                gvProduccion.Columns["PR0GRAMADO2023"].Visible = false;
                                gvProduccion.Columns["PRESUPUESTO2023"].Visible = false;
                                GridViewBandColumn bandCol4 = gvProduccion.Columns["CLASPRES2023"] as GridViewBandColumn;
                                bandCol4.Visible = false;

                                gvProgramas.Columns["PRESUPUESTO2029"].Visible = false;
                                gvProduccion.Columns["PR0GRAMADO2029"].Visible = false;
                                gvProduccion.Columns["PRESUPUESTO2029"].Visible = false;
                                GridViewBandColumn bandCol5 = gvProduccion.Columns["CLASPRES2029"] as GridViewBandColumn;
                                bandCol5.Visible = false;

                                gvProgramas.Columns["PRESUPUESTO2030"].Visible = false;
                                gvProduccion.Columns["PR0GRAMADO2030"].Visible = false;
                                gvProduccion.Columns["PRESUPUESTO2030"].Visible = false;
                                GridViewBandColumn bandCol6 = gvProduccion.Columns["CLASPRES2030"] as GridViewBandColumn;
                                bandCol5.Visible = false;

                            }

                            else if (Convert.ToInt32(periodo.Rows[0]["SPP$ID_PERIODO"]) == 24)
                            {

                                sql = @"SELECT COUNT(*) CONTEO FROM SCHE$SIPLAN20.SP20$PRODUCTO P 
                                        INNER JOIN SCHE$SIPLAN20.SP20$RESULTADOS R ON P.SPPRO$ID_RESULTADO = R.SPRES$ID_RESULTADO AND P.SPPRO$RESTRICTIVA = 'N' AND R.SPRES$RESTRICTIVA = 'N'
                                        INNER JOIN SCHE$SIPLAN20.SP20$RESULTADOS_ESTRATEGICOS ER ON ER.SPPRES$ID_RESULTADO = R.SPRES$COD_ESTRATEGICO AND ER.SPPRES$RESTRICTIVA = 'N' AND R.SPRES$RESTRICTIVA = 'N'
                                        INNER JOIN SCHE$SIPLAN20.SP20$RESULTADOS_ESTRATEGICOS PGG ON ER.SPPRES$DEPENDE = PGG.SPPRES$ID_RESULTADO AND ER.SPPRES$RESTRICTIVA = 'N' AND PGG.SPPRES$RESTRICTIVA = 'N'
                                        INNER JOIN SCHE$SIPLAN20.S20$PERIODO_RESULTADO PR ON PR.SPR$ID_RESULTADO = PGG.SPPRES$ID_RESULTADO AND PGG.SPPRES$RESTRICTIVA = 'N' AND PR.SPR$RESTRICTIVA = 'N'
                                        INNER JOIN SCHE$SIPLAN20.SP20$PERIODO_GOBIERNO PG ON PG.SPG$ID_PERIODO = PR.SPR$ID_PERIODO AND PG.SPG$RESTRICTIVA = 'N' AND PR.SPR$RESTRICTIVA = 'N'
                                        INNER JOIN SCHE$SIPLAN20.SP20$POM POM ON POM.SPPO$ID_POM = P.SPPRO$POM AND P.SPPRO$RESTRICTIVA = 'N' AND POM.SPPO$RESTRICTIVA = 'N'
                                        WHERE PG.SPG$ID_PERIODO = 0 AND POM.SPPO$ID_PERIODO = " + Convert.ToInt32(periodo.Rows[0]["SPP$ID_PERIODO"]) + @" AND POM.SPPO$ID_POM = " + Convert.ToInt32(Session["pom"]) + " AND R.SPRES$TIPO = 0";

                                estado = dao.consulta(sql);

                                if (estado == 1)
                                    tabla = dao.tabla;

                                if (tabla.Rows.Count > 0)
                                    numero_productos = Convert.ToInt32(tabla.Rows[0]["CONTEO"]);
                                else numero_productos = 0;

                                if (numero_productos > 0)
                                {
                                    Session["VINCULADOS"] = numero_productos;
                                    mensaje = "Tiene " + numero_productos + " productos vinculados a la anterior estructura de programa de gobierno 2020-2024, se recomienda actualizar la vinculación de estos productos a la estructura vigente de programa de gobierno 2024-2028, a un Resultado Estrátegico y/o resultado institucional";
                                    Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',3);", true);
                                }


                                gvProgramas.Columns["PRESUPUESTO2020"].Visible = false;
                                gvProduccion.Columns["PR0GRAMADO2020"].Visible = false;
                                gvProduccion.Columns["PRESUPUESTO2020"].Visible = false;
                                GridViewBandColumn bandCol = gvProduccion.Columns["CLASPRES2020"] as GridViewBandColumn;
                                bandCol.Visible = false;


                                gvProgramas.Columns["PRESUPUESTO2021"].Visible = false;
                                gvProduccion.Columns["PR0GRAMADO2021"].Visible = false;
                                gvProduccion.Columns["PRESUPUESTO2021"].Visible = false;
                                GridViewBandColumn bandCol2 = gvProduccion.Columns["CLASPRES2021"] as GridViewBandColumn;
                                bandCol2.Visible = false;

                                gvProgramas.Columns["PRESUPUESTO2022"].Visible = false;
                                gvProduccion.Columns["PR0GRAMADO2022"].Visible = false;
                                gvProduccion.Columns["PRESUPUESTO2022"].Visible = false;
                                GridViewBandColumn bandCol3 = gvProduccion.Columns["CLASPRES2022"] as GridViewBandColumn;
                                bandCol3.Visible = false;

                                gvProgramas.Columns["PRESUPUESTO2023"].Visible = false;
                                gvProduccion.Columns["PR0GRAMADO2023"].Visible = false;
                                gvProduccion.Columns["PRESUPUESTO2023"].Visible = false;
                                GridViewBandColumn bandCol4 = gvProduccion.Columns["CLASPRES2023"] as GridViewBandColumn;
                                bandCol4.Visible = false;

                                gvProgramas.Columns["PRESUPUESTO2024"].Visible = false;
                                gvProduccion.Columns["PR0GRAMADO2024"].Visible = false;
                                gvProduccion.Columns["PRESUPUESTO2024"].Visible = false;
                                GridViewBandColumn bandCol5 = gvProduccion.Columns["CLASPRES2024"] as GridViewBandColumn;
                                bandCol5.Visible = false;


                                gvProgramas.Columns["PRESUPUESTO2030"].Visible = false;
                                gvProduccion.Columns["PR0GRAMADO2030"].Visible = false;
                                gvProduccion.Columns["PRESUPUESTO2030"].Visible = false;
                                GridViewBandColumn bandCol6 = gvProduccion.Columns["CLASPRES2030"] as GridViewBandColumn;
                                bandCol6.Visible = false;

                            }


                            else if (Convert.ToInt32(periodo.Rows[0]["SPP$ID_PERIODO"]) == 25)
                            {

                                sql = @"SELECT COUNT(*) CONTEO FROM SCHE$SIPLAN20.SP20$PRODUCTO P 
                                        INNER JOIN SCHE$SIPLAN20.SP20$RESULTADOS R ON P.SPPRO$ID_RESULTADO = R.SPRES$ID_RESULTADO AND P.SPPRO$RESTRICTIVA = 'N' AND R.SPRES$RESTRICTIVA = 'N'
                                        INNER JOIN SCHE$SIPLAN20.SP20$RESULTADOS_ESTRATEGICOS ER ON ER.SPPRES$ID_RESULTADO = R.SPRES$COD_ESTRATEGICO AND ER.SPPRES$RESTRICTIVA = 'N' AND R.SPRES$RESTRICTIVA = 'N'
                                        INNER JOIN SCHE$SIPLAN20.SP20$RESULTADOS_ESTRATEGICOS PGG ON ER.SPPRES$DEPENDE = PGG.SPPRES$ID_RESULTADO AND ER.SPPRES$RESTRICTIVA = 'N' AND PGG.SPPRES$RESTRICTIVA = 'N'
                                        INNER JOIN SCHE$SIPLAN20.S20$PERIODO_RESULTADO PR ON PR.SPR$ID_RESULTADO = PGG.SPPRES$ID_RESULTADO AND PGG.SPPRES$RESTRICTIVA = 'N' AND PR.SPR$RESTRICTIVA = 'N'
                                        INNER JOIN SCHE$SIPLAN20.SP20$PERIODO_GOBIERNO PG ON PG.SPG$ID_PERIODO = PR.SPR$ID_PERIODO AND PG.SPG$RESTRICTIVA = 'N' AND PR.SPR$RESTRICTIVA = 'N'
                                        INNER JOIN SCHE$SIPLAN20.SP20$POM POM ON POM.SPPO$ID_POM = P.SPPRO$POM AND P.SPPRO$RESTRICTIVA = 'N' AND POM.SPPO$RESTRICTIVA = 'N'
                                        WHERE PG.SPG$ID_PERIODO = 0 AND POM.SPPO$ID_PERIODO = " + Convert.ToInt32(periodo.Rows[0]["SPP$ID_PERIODO"]) + @" AND POM.SPPO$ID_POM = " + Convert.ToInt32(Session["pom"]) + " AND R.SPRES$TIPO = 0";

                                estado = dao.consulta(sql);

                                if (estado == 1)
                                    tabla = dao.tabla;

                                if (tabla.Rows.Count > 0)
                                    numero_productos = Convert.ToInt32(tabla.Rows[0]["CONTEO"]);
                                else numero_productos = 0;

                                if (numero_productos > 0)
                                {
                                    Session["VINCULADOS"] = numero_productos;
                                    mensaje = "Tiene " + numero_productos + " productos vinculados a la anterior estructura de programa de gobierno 2020-2024, se recomienda actualizar la vinculación de estos productos a la estructura vigente de programa de gobierno 2024-2028, a un Resultado Estrátegico y/o resultado institucional";
                                    Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',3);", true);
                                }


                                gvProgramas.Columns["PRESUPUESTO2020"].Visible = false;
                                gvProduccion.Columns["PR0GRAMADO2020"].Visible = false;
                                gvProduccion.Columns["PRESUPUESTO2020"].Visible = false;
                                GridViewBandColumn bandCol = gvProduccion.Columns["CLASPRES2020"] as GridViewBandColumn;
                                bandCol.Visible = false;


                                gvProgramas.Columns["PRESUPUESTO2021"].Visible = false;
                                gvProduccion.Columns["PR0GRAMADO2021"].Visible = false;
                                gvProduccion.Columns["PRESUPUESTO2021"].Visible = false;
                                GridViewBandColumn bandCol2 = gvProduccion.Columns["CLASPRES2021"] as GridViewBandColumn;
                                bandCol2.Visible = false;

                                gvProgramas.Columns["PRESUPUESTO2022"].Visible = false;
                                gvProduccion.Columns["PR0GRAMADO2022"].Visible = false;
                                gvProduccion.Columns["PRESUPUESTO2022"].Visible = false;
                                GridViewBandColumn bandCol3 = gvProduccion.Columns["CLASPRES2022"] as GridViewBandColumn;
                                bandCol3.Visible = false;

                                gvProgramas.Columns["PRESUPUESTO2023"].Visible = false;
                                gvProduccion.Columns["PR0GRAMADO2023"].Visible = false;
                                gvProduccion.Columns["PRESUPUESTO2023"].Visible = false;
                                GridViewBandColumn bandCol4 = gvProduccion.Columns["CLASPRES2023"] as GridViewBandColumn;
                                bandCol4.Visible = false;

                                gvProgramas.Columns["PRESUPUESTO2024"].Visible = false;
                                gvProduccion.Columns["PR0GRAMADO2024"].Visible = false;
                                gvProduccion.Columns["PRESUPUESTO2024"].Visible = false;
                                GridViewBandColumn bandCol5 = gvProduccion.Columns["CLASPRES2024"] as GridViewBandColumn;
                                bandCol5.Visible = false;


                                gvProgramas.Columns["PRESUPUESTO2025"].Visible = false;
                                gvProduccion.Columns["PR0GRAMADO2025"].Visible = false;
                                gvProduccion.Columns["PRESUPUESTO2025"].Visible = false;
                                GridViewBandColumn bandCol6 = gvProduccion.Columns["CLASPRES2025"] as GridViewBandColumn;
                                bandCol6.Visible = false;

                            }


                            lblTitulo.Text = Session["institucion"].ToString() + " " + "periodo: " + periodo.Rows[0]["SPP$INICIO"] + " - " + periodo.Rows[0]["SPP$FINAL"] + " " + periodo.Rows[0]["ESTADO"];
                        }

                        cargaprogramas();
                        if (IsPostBack)
                        {

                            if (Session["poa"] != null)
                            {
                                temp = (DataTable)Session["poa"];
                                if (temp.Rows.Count > 0)
                                {
                                    if (Session["carga"] != null && Convert.ToInt32(Session["carga"]) == 1)
                                        cargaSubproductos(temp, Convert.ToInt32(cbTipoProduccion.Value));
                                    else if (Session["carga"] != null && Convert.ToInt32(Session["carga"]) == 2)
                                    {
                                        cargaComboPOAS();
                                        cargaProduccionPOA(Convert.ToInt32(cbAniPOA.Text), Convert.ToInt32(cbTipoProd.Value));
                                    }

                                }
                            }
                        }


                    }

                    bntEnviar.Attributes.Add("onclick", "return confirm('Esta por aprobar la metas fisicas y financieras multianuales de su POM, luego de esta operación no podrá modificar la metas ingresadas, por favor confirme la operación')");
                }
            }
        }

        protected void btnRegresa_Click(object sender, EventArgs e)
        {
            Response.Redirect("../pom/pom.aspx");
        }

        protected void cargaprogramas()
        {
            DataTable tablas = new DataTable();
            DataTable tablas2 = new DataTable();
            sql = "SELECT P.SPP$ID_PERIODO, P.SPP$INICIO, P.SPP$FINAL, POA.SPOA$ESTADO,  POA.SPOA$ID_POA, POA.SPOA$ANIO, POA.SPOA$ID_POM, POA.SPOA$ID_INSTITUCION, P.SPP$ORDEN FROM SCHE$SIPLAN20.SP20$POA POA INNER JOIN SCHE$SIPLAN20.SP20$POM POM ON POM.SPPO$ID_POM = POA.SPOA$ID_POM AND POM.SPPO$ID_INSTITUCION = POA.SPOA$ID_INSTITUCION AND POM.SPPO$RESTRICTIVA = 'N' AND POA.SPOA$RESTRICTIVA = 'N' ";
            sql = sql + "INNER JOIN SCHE$SIPLAN20.SP20$PERIODO P ON P.SPP$ID_PERIODO = POM.SPPO$ID_PERIODO AND P.SPP$RESTRICTIVA = 'N' AND POM.SPPO$RESTRICTIVA = 'N' WHERE POM.SPPO$ID_POM = " + Session["pom"] + " AND POM.SPPO$ID_INSTITUCION = " + Session["insto"] + " AND P.SPP$ID_PERIODO = " + Session["periodo"] + " ORDER BY  POA.SPOA$ANIO ASC";
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
                    Session["poa"] = tabla;
                    if (Convert.ToInt32(Session["periodo"]) == 20)
                    {

                        sql = "SELECT * FROM SCHE$SIPLAN20.SP20$PROGRA_PRESUPUESTO WHERE SPRES$POM = " + tabla.Rows[0]["SPOA$ID_POM"] + " AND SPPRES$RESTRICTIVA = 'N'";
                        estado = dao.consulta(sql);
                        if (estado == 1)
                            tablas = dao.tabla;
                        if (tablas.Rows.Count > 0)
                        {
                            sql = "SELECT NOPROGRAMA, PROGRAMA,ESSUBPROGRAMADE, SPPRO$ID_POM, SPPRO$ID_INSTITUCION, NULL POA2020, NULL PROGRA2020, NULL PRESUPUESTO2020 ,POA2021, PROGRA2021, PRESUPUESTO2021, POA2022, PROGRA2022, PRESUPUESTO2022, POA2023, PROGRA2023, PRESUPUESTO2023, POA2024, PROGRA2024, PRESUPUESTO2024, POA2025, PROGRA2025, PRESUPUESTO2025,   NULL POA2026, NULL PROGRA2026, NULL PRESUPUESTO2026, NULL POA2027, NULL PROGRA2027, NULL PRESUPUESTO2027,  NULL POA2028, NULL PROGRA2028, NULL PRESUPUESTO2028,  SPPRO$RESTRICTIVA  FROM (SELECT P.SPPRO$ID_PROGRAMA_PRESUPUESTO NOPROGRAMA, P.SPPRO$DESCRIPCION PROGRAMA, CASE WHEN P.SPPRO$ID_PROGRAMA_DEPENDE IS NULL THEN P.SPPRO$ID_PROGRAMA_PRESUPUESTO ELSE P.SPPRO$ID_PROGRAMA_DEPENDE END AS ESSUBPROGRAMADE, P.SPPRO$ID_POM, P.SPPRO$ID_INSTITUCION, ";
                            for (int i = 0; i < tabla.Rows.Count; i++)
                            {
                                sql = sql + "SCHE$SIPLAN20.FNC$POA(P.SPPRO$ID_POM, P.SPPRO$ID_INSTITUCION," + tabla.Rows[i]["SPOA$ANIO"] + ") POA" + tabla.Rows[i]["SPOA$ANIO"] + ",SCHE$SIPLAN20.FNC$PROGRAPRESUPUESTO(P.SPPRO$ID_PROGRAMA_PRESUPUESTO," + tabla.Rows[i]["SPOA$ID_POA"] + " ," + tabla.Rows[i]["SPOA$ANIO"] + ", P.SPPRO$ID_POM, P.SPPRO$ID_INSTITUCION) PROGRA" + tabla.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$OBTIENEPRESUPUESTO(P.SPPRO$ID_PROGRAMA_PRESUPUESTO," + tabla.Rows[i]["SPOA$ID_POA"] + " ," + tabla.Rows[i]["SPOA$ANIO"] + ", P.SPPRO$ID_POM, P.SPPRO$ID_INSTITUCION) PRESUPUESTO" + tabla.Rows[i]["SPOA$ANIO"] + ", ";
                            }

                            sql = sql + "P.SPPRO$RESTRICTIVA FROM SCHE$SIPLAN20.SP20$PROGRAMA_PRESUPUESTARIO P WHERE P.SPPRO$ID_PROGRAMA_DEPENDE IS NULL AND P.SPPRO$RESTRICTIVA = 'N' UNION ";
                            sql = sql + "SELECT P.SPPRO$ID_PROGRAMA_PRESUPUESTO NOPROGRAMA, P.SPPRO$DESCRIPCION PROGRAMA, P.SPPRO$ID_PROGRAMA_DEPENDE ESSUBPROGRAMADE,  P.SPPRO$ID_POM, P.SPPRO$ID_INSTITUCION, ";
                            for (int i = 0; i < tabla.Rows.Count; i++)
                            {
                                sql = sql + "SCHE$SIPLAN20.FNC$POA(P.SPPRO$ID_POM, P.SPPRO$ID_INSTITUCION," + tabla.Rows[i]["SPOA$ANIO"] + ") POA" + tabla.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$PROGRAPRESUPUESTO(P.SPPRO$ID_PROGRAMA_PRESUPUESTO," + tabla.Rows[i]["SPOA$ID_POA"] + " ," + tabla.Rows[i]["SPOA$ANIO"] + ", P.SPPRO$ID_POM, P.SPPRO$ID_INSTITUCION) PROGRA" + tabla.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$OBTIENEPRESUPUESTO(P.SPPRO$ID_PROGRAMA_PRESUPUESTO," + tabla.Rows[i]["SPOA$ID_POA"] + " ," + tabla.Rows[i]["SPOA$ANIO"] + ", P.SPPRO$ID_POM, P.SPPRO$ID_INSTITUCION) PRESUPUESTO" + tabla.Rows[i]["SPOA$ANIO"] + ", ";
                            }

                            sql = sql + "P.SPPRO$RESTRICTIVA FROM SCHE$SIPLAN20.SP20$PROGRAMA_PRESUPUESTARIO P WHERE P.SPPRO$ID_PROGRAMA_DEPENDE IS NOT NULL AND P.SPPRO$RESTRICTIVA = 'N') WHERE SPPRO$ID_POM = " + Session["pom"] + "  AND SPPRO$ID_INSTITUCION = " + Session["insto"] + " ORDER BY NOPROGRAMA ASC ";
                        }
                        else
                        {
                            sql = "SELECT (SELECT SPOA$ID_POA FROM SCHE$SIPLAN20.SP20$POA POA WHERE POA.SPOA$ID_POM = " + tabla.Rows[0]["SPOA$ID_POM"] + " AND POA.SPOA$ID_INSTITUCION = " + tabla.Rows[0]["SPOA$ID_INSTITUCION"] + " AND SPOA$RESTRICTIVA = 'N' AND SPOA$ANIO = SPPRES$ANIO) POA, PR.SPPRES$ID_PROGRAMA_PRESUPUESTO,PR.SPPRES$ANIO,PR.SPPRES$PRESUPUESTO, PR.SPPRES$PRESUPUESTO_VIGENTE FROM SCHE$SIPLAN20.SP20$PROGRA_PRESUPUESTO PR INNER JOIN SCHE$SIPLAN20.SP20$PROGRAMA_PRESUPUESTARIO PO ON PR.SPRES$POM = PO.SPPRO$ID_POM AND PR.SPRES$INSTO = PO.SPPRO$ID_INSTITUCION AND PR.SPPRES$RESTRICTIVA = 'N' AND PO.SPPRO$RESTRICTIVA = 'N' INNER JOIN SCHE$SIPLAN20.SP20$POM POM ON POM.SPPO$ID_POM = PO.SPPRO$ID_POM AND POM.SPPO$ID_INSTITUCION = PO.SPPRO$ID_INSTITUCION AND POM.SPPO$RESTRICTIVA = 'N' AND PO.SPPRO$RESTRICTIVA = 'N' INNER JOIN SCHE$SIPLAN20.SP20$PERIODO P ON P.SPP$ID_PERIODO = POM.SPPO$ID_PERIODO AND P.SPP$RESTRICTIVA = 'N' AND POM.SPPO$RESTRICTIVA = 'N' WHERE P.SPP$ORDEN = " + (Convert.ToInt32(tabla.Rows[0]["SPP$ORDEN"]) - 1) + " AND POM.SPPO$ID_INSTITUCION = " + tabla.Rows[0]["SPOA$ID_INSTITUCION"] + " AND PR.SPPRES$ANIO BETWEEN " + Convert.ToInt32(tabla.Rows[0]["SPP$INICIO"]) + " AND " + (Convert.ToInt32(tabla.Rows[0]["SPP$FINAL"]) - 1) + " GROUP BY PR.SPPRES$ID_PROGRAMA_PRESUPUESTO,PR.SPPRES$ANIO,PR.SPPRES$PRESUPUESTO, PR.SPPRES$PRESUPUESTO_VIGENTE";
                            estado = dao.consulta(sql);
                            if (estado == 1)
                                tablas2 = dao.tabla;
                            if (tablas2.Rows.Count > 0)
                            {
                                for (int i = 0; i < tablas2.Rows.Count; i++)
                                {
                                    estado = insertaPresupuesto(Convert.ToDouble(tablas2.Rows[i]["SPPRES$PRESUPUESTO"]), Convert.ToDouble(tablas2.Rows[i]["SPPRES$ID_PROGRAMA_PRESUPUESTO"].ToString()), Convert.ToInt32(tablas2.Rows[i]["POA"].ToString()), Convert.ToInt32(tablas2.Rows[i]["SPPRES$ANIO"].ToString()), Convert.ToInt32(tabla.Rows[0]["SPOA$ID_POM"].ToString()), Convert.ToInt32(tabla.Rows[0]["SPOA$ID_INSTITUCION"].ToString()));
                                    if (estado == 0)
                                        break;
                                }


                                sql = "SELECT NOPROGRAMA, PROGRAMA,ESSUBPROGRAMADE, SPPRO$ID_POM, SPPRO$ID_INSTITUCION, NULL POA2020, NULL PROGRA2020, NULL PRESUPUESTO2020 ,POA2021, PROGRA2021, PRESUPUESTO2021, POA2022, PROGRA2022, PRESUPUESTO2022, POA2023, PROGRA2023, PRESUPUESTO2023, POA2024, PROGRA2024, PRESUPUESTO2024, POA2025, PROGRA2025, PRESUPUESTO2025,  NULL POA2026, NULL PROGRA2026, NULL PRESUPUESTO2026,  NULL POA2027, NULL PROGRA2027, NULL PRESUPUESTO2027,  NULL POA2028, NULL PROGRA2028, NULL PRESUPUESTO2028,  SPPRO$RESTRICTIVA  FROM (SELECT P.SPPRO$ID_PROGRAMA_PRESUPUESTO NOPROGRAMA, P.SPPRO$DESCRIPCION PROGRAMA, CASE WHEN P.SPPRO$ID_PROGRAMA_DEPENDE IS NULL THEN P.SPPRO$ID_PROGRAMA_PRESUPUESTO ELSE P.SPPRO$ID_PROGRAMA_DEPENDE END AS ESSUBPROGRAMADE, P.SPPRO$ID_POM, P.SPPRO$ID_INSTITUCION, ";
                                for (int i = 0; i < tabla.Rows.Count; i++)
                                {
                                    sql = sql + "SCHE$SIPLAN20.FNC$POA(P.SPPRO$ID_POM, P.SPPRO$ID_INSTITUCION," + tabla.Rows[i]["SPOA$ANIO"] + ") POA" + tabla.Rows[i]["SPOA$ANIO"] + ",SCHE$SIPLAN20.FNC$PROGRAPRESUPUESTO(P.SPPRO$ID_PROGRAMA_PRESUPUESTO," + tabla.Rows[i]["SPOA$ID_POA"] + " ," + tabla.Rows[i]["SPOA$ANIO"] + ", P.SPPRO$ID_POM, P.SPPRO$ID_INSTITUCION) PROGRA" + tabla.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$OBTIENEPRESUPUESTO(P.SPPRO$ID_PROGRAMA_PRESUPUESTO," + tabla.Rows[i]["SPOA$ID_POA"] + " ," + tabla.Rows[i]["SPOA$ANIO"] + ", P.SPPRO$ID_POM, P.SPPRO$ID_INSTITUCION) PRESUPUESTO" + tabla.Rows[i]["SPOA$ANIO"] + ", ";
                                }

                                sql = sql + "P.SPPRO$RESTRICTIVA FROM SCHE$SIPLAN20.SP20$PROGRAMA_PRESUPUESTARIO P WHERE P.SPPRO$ID_PROGRAMA_DEPENDE IS NULL AND P.SPPRO$RESTRICTIVA = 'N' UNION ";
                                sql = sql + "SELECT P.SPPRO$ID_PROGRAMA_PRESUPUESTO NOPROGRAMA, P.SPPRO$DESCRIPCION PROGRAMA, P.SPPRO$ID_PROGRAMA_DEPENDE ESSUBPROGRAMADE,  P.SPPRO$ID_POM, P.SPPRO$ID_INSTITUCION, ";
                                for (int i = 0; i < tabla.Rows.Count; i++)
                                {
                                    sql = sql + "SCHE$SIPLAN20.FNC$POA(P.SPPRO$ID_POM, P.SPPRO$ID_INSTITUCION," + tabla.Rows[i]["SPOA$ANIO"] + ") POA" + tabla.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$PROGRAPRESUPUESTO(P.SPPRO$ID_PROGRAMA_PRESUPUESTO," + tabla.Rows[i]["SPOA$ID_POA"] + " ," + tabla.Rows[i]["SPOA$ANIO"] + ", P.SPPRO$ID_POM, P.SPPRO$ID_INSTITUCION) PROGRA" + tabla.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$OBTIENEPRESUPUESTO(P.SPPRO$ID_PROGRAMA_PRESUPUESTO," + tabla.Rows[i]["SPOA$ID_POA"] + " ," + tabla.Rows[i]["SPOA$ANIO"] + ", P.SPPRO$ID_POM, P.SPPRO$ID_INSTITUCION) PRESUPUESTO" + tabla.Rows[i]["SPOA$ANIO"] + ", ";
                                }
                                sql = sql + "P.SPPRO$RESTRICTIVA FROM SCHE$SIPLAN20.SP20$PROGRAMA_PRESUPUESTARIO P WHERE P.SPPRO$ID_PROGRAMA_DEPENDE IS NOT NULL AND P.SPPRO$RESTRICTIVA = 'N') WHERE SPPRO$ID_POM = " + Session["pom"] + "  AND SPPRO$ID_INSTITUCION = " + Session["insto"] + " ORDER BY NOPROGRAMA ASC ";

                            }

                            else
                            {
                                sql = "SELECT NOPROGRAMA, PROGRAMA,ESSUBPROGRAMADE, SPPRO$ID_POM, SPPRO$ID_INSTITUCION, NULL POA2020, NULL PROGRA2020, NULL PRESUPUESTO2020 ,POA2021, PROGRA2021, PRESUPUESTO2021, POA2022, PROGRA2022, PRESUPUESTO2022, POA2023, PROGRA2023, PRESUPUESTO2023, POA2024, PROGRA2024, PRESUPUESTO2024, POA2025, PROGRA2025, PRESUPUESTO2025, NULL POA2026, NULL PROGRA2026, NULL PRESUPUESTO2026, NULL POA2027, NULL PROGRA2027, NULL PRESUPUESTO2027,  NULL POA2028, NULL PROGRA2028, NULL PRESUPUESTO2028,  SPPRO$RESTRICTIVA  FROM (SELECT P.SPPRO$ID_PROGRAMA_PRESUPUESTO NOPROGRAMA, P.SPPRO$DESCRIPCION PROGRAMA, CASE WHEN P.SPPRO$ID_PROGRAMA_DEPENDE IS NULL THEN P.SPPRO$ID_PROGRAMA_PRESUPUESTO ELSE P.SPPRO$ID_PROGRAMA_DEPENDE END AS ESSUBPROGRAMADE, P.SPPRO$ID_POM, P.SPPRO$ID_INSTITUCION, ";
                                for (int i = 0; i < tabla.Rows.Count; i++)
                                {
                                    sql = sql + "SCHE$SIPLAN20.FNC$POA(P.SPPRO$ID_POM, P.SPPRO$ID_INSTITUCION," + tabla.Rows[i]["SPOA$ANIO"] + ") POA" + tabla.Rows[i]["SPOA$ANIO"] + ",SCHE$SIPLAN20.FNC$PROGRAPRESUPUESTO(P.SPPRO$ID_PROGRAMA_PRESUPUESTO," + tabla.Rows[i]["SPOA$ID_POA"] + " ," + tabla.Rows[i]["SPOA$ANIO"] + ", P.SPPRO$ID_POM, P.SPPRO$ID_INSTITUCION) PROGRA" + tabla.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$OBTIENEPRESUPUESTO(P.SPPRO$ID_PROGRAMA_PRESUPUESTO," + tabla.Rows[i]["SPOA$ID_POA"] + " ," + tabla.Rows[i]["SPOA$ANIO"] + ", P.SPPRO$ID_POM, P.SPPRO$ID_INSTITUCION) PRESUPUESTO" + tabla.Rows[i]["SPOA$ANIO"] + ", ";
                                }

                                sql = sql + "P.SPPRO$RESTRICTIVA FROM SCHE$SIPLAN20.SP20$PROGRAMA_PRESUPUESTARIO P WHERE P.SPPRO$ID_PROGRAMA_DEPENDE IS NULL AND P.SPPRO$RESTRICTIVA = 'N' UNION ";
                                sql = sql + "SELECT P.SPPRO$ID_PROGRAMA_PRESUPUESTO NOPROGRAMA, P.SPPRO$DESCRIPCION PROGRAMA, P.SPPRO$ID_PROGRAMA_DEPENDE ESSUBPROGRAMADE,  P.SPPRO$ID_POM, P.SPPRO$ID_INSTITUCION, ";
                                for (int i = 0; i < tabla.Rows.Count; i++)
                                {
                                    sql = sql + "SCHE$SIPLAN20.FNC$POA(P.SPPRO$ID_POM, P.SPPRO$ID_INSTITUCION," + tabla.Rows[i]["SPOA$ANIO"] + ") POA" + tabla.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$PROGRAPRESUPUESTO(P.SPPRO$ID_PROGRAMA_PRESUPUESTO," + tabla.Rows[i]["SPOA$ID_POA"] + " ," + tabla.Rows[i]["SPOA$ANIO"] + ", P.SPPRO$ID_POM, P.SPPRO$ID_INSTITUCION) PROGRA" + tabla.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$OBTIENEPRESUPUESTO(P.SPPRO$ID_PROGRAMA_PRESUPUESTO," + tabla.Rows[i]["SPOA$ID_POA"] + " ," + tabla.Rows[i]["SPOA$ANIO"] + ", P.SPPRO$ID_POM, P.SPPRO$ID_INSTITUCION) PRESUPUESTO" + tabla.Rows[i]["SPOA$ANIO"] + ", ";
                                }
                                sql = sql + "P.SPPRO$RESTRICTIVA FROM SCHE$SIPLAN20.SP20$PROGRAMA_PRESUPUESTARIO P WHERE P.SPPRO$ID_PROGRAMA_DEPENDE IS NOT NULL AND P.SPPRO$RESTRICTIVA = 'N') WHERE SPPRO$ID_POM = " + Session["pom"] + "  AND SPPRO$ID_INSTITUCION = " + Session["insto"] + " ORDER BY NOPROGRAMA ASC ";

                            }


                        }

                    }

                    else if (Convert.ToInt32(Session["periodo"]) == 21)
                    {

                        sql = "SELECT * FROM SCHE$SIPLAN20.SP20$PROGRA_PRESUPUESTO WHERE SPRES$POM = " + tabla.Rows[0]["SPOA$ID_POM"] + " AND SPPRES$RESTRICTIVA = 'N'";
                        estado = dao.consulta(sql);
                        if (estado == 1)
                            tablas = dao.tabla;
                        if (tablas.Rows.Count > 0)
                        {
                            sql = "SELECT NOPROGRAMA, PROGRAMA,ESSUBPROGRAMADE, SPPRO$ID_POM, SPPRO$ID_INSTITUCION, NULL POA2020, NULL PROGRA2020, NULL PRESUPUESTO2020 , NULL POA2021, NULL PROGRA2021, NULL PRESUPUESTO2021, POA2022, PROGRA2022, PRESUPUESTO2022, POA2023, PROGRA2023, PRESUPUESTO2023, POA2024, PROGRA2024, PRESUPUESTO2024, POA2025, PROGRA2025, PRESUPUESTO2025,   POA2026, PROGRA2026, PRESUPUESTO2026, NULL POA2027, NULL PROGRA2027, NULL PRESUPUESTO2027,   NULL POA2028, NULL PROGRA2028, NULL PRESUPUESTO2028,   SPPRO$RESTRICTIVA  FROM (SELECT P.SPPRO$ID_PROGRAMA_PRESUPUESTO NOPROGRAMA, P.SPPRO$DESCRIPCION PROGRAMA, CASE WHEN P.SPPRO$ID_PROGRAMA_DEPENDE IS NULL THEN P.SPPRO$ID_PROGRAMA_PRESUPUESTO ELSE P.SPPRO$ID_PROGRAMA_DEPENDE END AS ESSUBPROGRAMADE, P.SPPRO$ID_POM, P.SPPRO$ID_INSTITUCION, ";
                            for (int i = 0; i < tabla.Rows.Count; i++)
                            {
                                sql = sql + "SCHE$SIPLAN20.FNC$POA(P.SPPRO$ID_POM, P.SPPRO$ID_INSTITUCION," + tabla.Rows[i]["SPOA$ANIO"] + ") POA" + tabla.Rows[i]["SPOA$ANIO"] + ",SCHE$SIPLAN20.FNC$PROGRAPRESUPUESTO(P.SPPRO$ID_PROGRAMA_PRESUPUESTO," + tabla.Rows[i]["SPOA$ID_POA"] + " ," + tabla.Rows[i]["SPOA$ANIO"] + ", P.SPPRO$ID_POM, P.SPPRO$ID_INSTITUCION) PROGRA" + tabla.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$OBTIENEPRESUPUESTO(P.SPPRO$ID_PROGRAMA_PRESUPUESTO," + tabla.Rows[i]["SPOA$ID_POA"] + " ," + tabla.Rows[i]["SPOA$ANIO"] + ", P.SPPRO$ID_POM, P.SPPRO$ID_INSTITUCION) PRESUPUESTO" + tabla.Rows[i]["SPOA$ANIO"] + ", ";
                            }

                            sql = sql + "P.SPPRO$RESTRICTIVA FROM SCHE$SIPLAN20.SP20$PROGRAMA_PRESUPUESTARIO P WHERE P.SPPRO$ID_PROGRAMA_DEPENDE IS NULL AND P.SPPRO$RESTRICTIVA = 'N' UNION ";
                            sql = sql + "SELECT P.SPPRO$ID_PROGRAMA_PRESUPUESTO NOPROGRAMA, P.SPPRO$DESCRIPCION PROGRAMA, P.SPPRO$ID_PROGRAMA_DEPENDE ESSUBPROGRAMADE,  P.SPPRO$ID_POM, P.SPPRO$ID_INSTITUCION, ";
                            for (int i = 0; i < tabla.Rows.Count; i++)
                            {
                                sql = sql + "SCHE$SIPLAN20.FNC$POA(P.SPPRO$ID_POM, P.SPPRO$ID_INSTITUCION," + tabla.Rows[i]["SPOA$ANIO"] + ") POA" + tabla.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$PROGRAPRESUPUESTO(P.SPPRO$ID_PROGRAMA_PRESUPUESTO," + tabla.Rows[i]["SPOA$ID_POA"] + " ," + tabla.Rows[i]["SPOA$ANIO"] + ", P.SPPRO$ID_POM, P.SPPRO$ID_INSTITUCION) PROGRA" + tabla.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$OBTIENEPRESUPUESTO(P.SPPRO$ID_PROGRAMA_PRESUPUESTO," + tabla.Rows[i]["SPOA$ID_POA"] + " ," + tabla.Rows[i]["SPOA$ANIO"] + ", P.SPPRO$ID_POM, P.SPPRO$ID_INSTITUCION) PRESUPUESTO" + tabla.Rows[i]["SPOA$ANIO"] + ", ";
                            }
                            sql = sql + "P.SPPRO$RESTRICTIVA FROM SCHE$SIPLAN20.SP20$PROGRAMA_PRESUPUESTARIO P WHERE P.SPPRO$ID_PROGRAMA_DEPENDE IS NOT NULL AND P.SPPRO$RESTRICTIVA = 'N') WHERE SPPRO$ID_POM = " + Session["pom"] + "  AND SPPRO$ID_INSTITUCION = " + Session["insto"] + " ORDER BY NOPROGRAMA ASC ";
                        }
                        else
                        {
                            sql = "SELECT (SELECT SPOA$ID_POA FROM SCHE$SIPLAN20.SP20$POA POA WHERE POA.SPOA$ID_POM = " + tabla.Rows[0]["SPOA$ID_POM"] + " AND POA.SPOA$ID_INSTITUCION = " + tabla.Rows[0]["SPOA$ID_INSTITUCION"] + " AND SPOA$RESTRICTIVA = 'N' AND SPOA$ANIO = SPPRES$ANIO) POA, PR.SPPRES$ID_PROGRAMA_PRESUPUESTO,PR.SPPRES$ANIO,PR.SPPRES$PRESUPUESTO, PR.SPPRES$PRESUPUESTO_VIGENTE FROM SCHE$SIPLAN20.SP20$PROGRA_PRESUPUESTO PR INNER JOIN SCHE$SIPLAN20.SP20$PROGRAMA_PRESUPUESTARIO PO ON PR.SPRES$POM = PO.SPPRO$ID_POM AND PR.SPRES$INSTO = PO.SPPRO$ID_INSTITUCION AND PR.SPPRES$RESTRICTIVA = 'N' AND PO.SPPRO$RESTRICTIVA = 'N' INNER JOIN SCHE$SIPLAN20.SP20$POM POM ON POM.SPPO$ID_POM = PO.SPPRO$ID_POM AND POM.SPPO$ID_INSTITUCION = PO.SPPRO$ID_INSTITUCION AND POM.SPPO$RESTRICTIVA = 'N' AND PO.SPPRO$RESTRICTIVA = 'N' INNER JOIN SCHE$SIPLAN20.SP20$PERIODO P ON P.SPP$ID_PERIODO = POM.SPPO$ID_PERIODO AND P.SPP$RESTRICTIVA = 'N' AND POM.SPPO$RESTRICTIVA = 'N' WHERE P.SPP$ORDEN = " + (Convert.ToInt32(tabla.Rows[0]["SPP$ORDEN"]) - 1) + " AND POM.SPPO$ID_INSTITUCION = " + tabla.Rows[0]["SPOA$ID_INSTITUCION"] + " AND PR.SPPRES$ANIO BETWEEN " + Convert.ToInt32(tabla.Rows[0]["SPP$INICIO"]) + " AND " + (Convert.ToInt32(tabla.Rows[0]["SPP$FINAL"]) - 1) + " GROUP BY PR.SPPRES$ID_PROGRAMA_PRESUPUESTO,PR.SPPRES$ANIO,PR.SPPRES$PRESUPUESTO, PR.SPPRES$PRESUPUESTO_VIGENTE order by PR.SPPRES$ID_PROGRAMA_PRESUPUESTO asc";
                            estado = dao.consulta(sql);
                            if (estado == 1)
                                tablas2 = dao.tabla;
                            if (tablas2.Rows.Count > 0)
                            {
                                for (int i = 0; i < tablas2.Rows.Count; i++)
                                {
                                    estado = insertaPresupuesto(Convert.ToDouble(tablas2.Rows[i]["SPPRES$PRESUPUESTO"]), Convert.ToDouble(tablas2.Rows[i]["SPPRES$ID_PROGRAMA_PRESUPUESTO"].ToString()), Convert.ToInt32(tablas2.Rows[i]["POA"].ToString()), Convert.ToInt32(tablas2.Rows[i]["SPPRES$ANIO"].ToString()), Convert.ToInt32(tabla.Rows[0]["SPOA$ID_POM"].ToString()), Convert.ToInt32(tabla.Rows[0]["SPOA$ID_INSTITUCION"].ToString()));
                                    //if (estado == 0)
                                    //  break;
                                }


                                sql = "SELECT NOPROGRAMA, PROGRAMA,ESSUBPROGRAMADE, SPPRO$ID_POM, SPPRO$ID_INSTITUCION, NULL POA2020, NULL PROGRA2020, NULL PRESUPUESTO2020 ,NULL POA2021, NULL PROGRA2021, NULL PRESUPUESTO2021, POA2022, PROGRA2022, PRESUPUESTO2022, POA2023, PROGRA2023, PRESUPUESTO2023, POA2024, PROGRA2024, PRESUPUESTO2024, POA2025, PROGRA2025, PRESUPUESTO2025, POA2026, PROGRA2026, PRESUPUESTO2026, NULL POA2027, NULL PROGRA2027, NULL PRESUPUESTO2027,  NULL POA2028, NULL PROGRA2028, NULL PRESUPUESTO2028, SPPRO$RESTRICTIVA  FROM (SELECT P.SPPRO$ID_PROGRAMA_PRESUPUESTO NOPROGRAMA, P.SPPRO$DESCRIPCION PROGRAMA, CASE WHEN P.SPPRO$ID_PROGRAMA_DEPENDE IS NULL THEN P.SPPRO$ID_PROGRAMA_PRESUPUESTO ELSE P.SPPRO$ID_PROGRAMA_DEPENDE END AS ESSUBPROGRAMADE, P.SPPRO$ID_POM, P.SPPRO$ID_INSTITUCION, ";
                                for (int i = 0; i < tabla.Rows.Count; i++)
                                {
                                    sql = sql + "SCHE$SIPLAN20.FNC$POA(P.SPPRO$ID_POM, P.SPPRO$ID_INSTITUCION," + tabla.Rows[i]["SPOA$ANIO"] + ") POA" + tabla.Rows[i]["SPOA$ANIO"] + ",SCHE$SIPLAN20.FNC$PROGRAPRESUPUESTO(P.SPPRO$ID_PROGRAMA_PRESUPUESTO," + tabla.Rows[i]["SPOA$ID_POA"] + " ," + tabla.Rows[i]["SPOA$ANIO"] + ", P.SPPRO$ID_POM, P.SPPRO$ID_INSTITUCION) PROGRA" + tabla.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$OBTIENEPRESUPUESTO(P.SPPRO$ID_PROGRAMA_PRESUPUESTO," + tabla.Rows[i]["SPOA$ID_POA"] + " ," + tabla.Rows[i]["SPOA$ANIO"] + ", P.SPPRO$ID_POM, P.SPPRO$ID_INSTITUCION) PRESUPUESTO" + tabla.Rows[i]["SPOA$ANIO"] + ", ";
                                }

                                sql = sql + "P.SPPRO$RESTRICTIVA FROM SCHE$SIPLAN20.SP20$PROGRAMA_PRESUPUESTARIO P WHERE P.SPPRO$ID_PROGRAMA_DEPENDE IS NULL AND P.SPPRO$RESTRICTIVA = 'N' UNION ";
                                sql = sql + "SELECT P.SPPRO$ID_PROGRAMA_PRESUPUESTO NOPROGRAMA, P.SPPRO$DESCRIPCION PROGRAMA, P.SPPRO$ID_PROGRAMA_DEPENDE ESSUBPROGRAMADE,  P.SPPRO$ID_POM, P.SPPRO$ID_INSTITUCION, ";
                                for (int i = 0; i < tabla.Rows.Count; i++)
                                {
                                    sql = sql + "SCHE$SIPLAN20.FNC$POA(P.SPPRO$ID_POM, P.SPPRO$ID_INSTITUCION," + tabla.Rows[i]["SPOA$ANIO"] + ") POA" + tabla.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$PROGRAPRESUPUESTO(P.SPPRO$ID_PROGRAMA_PRESUPUESTO," + tabla.Rows[i]["SPOA$ID_POA"] + " ," + tabla.Rows[i]["SPOA$ANIO"] + ", P.SPPRO$ID_POM, P.SPPRO$ID_INSTITUCION) PROGRA" + tabla.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$OBTIENEPRESUPUESTO(P.SPPRO$ID_PROGRAMA_PRESUPUESTO," + tabla.Rows[i]["SPOA$ID_POA"] + " ," + tabla.Rows[i]["SPOA$ANIO"] + ", P.SPPRO$ID_POM, P.SPPRO$ID_INSTITUCION) PRESUPUESTO" + tabla.Rows[i]["SPOA$ANIO"] + ", ";
                                }
                                sql = sql + "P.SPPRO$RESTRICTIVA FROM SCHE$SIPLAN20.SP20$PROGRAMA_PRESUPUESTARIO P WHERE P.SPPRO$ID_PROGRAMA_DEPENDE IS NOT NULL AND P.SPPRO$RESTRICTIVA = 'N') WHERE SPPRO$ID_POM = " + Session["pom"] + "  AND SPPRO$ID_INSTITUCION = " + Session["insto"] + " ORDER BY NOPROGRAMA ASC ";

                            }

                            else
                            {
                                sql = "SELECT NOPROGRAMA, PROGRAMA,ESSUBPROGRAMADE, SPPRO$ID_POM, SPPRO$ID_INSTITUCION, NULL POA2020, NULL PROGRA2020, NULL PRESUPUESTO2020 ,NULL POA2021, NULL PROGRA2021, NULL PRESUPUESTO2021, POA2022, PROGRA2022, PRESUPUESTO2022, POA2023, PROGRA2023, PRESUPUESTO2023, POA2024, PROGRA2024, PRESUPUESTO2024, POA2025, PROGRA2025, PRESUPUESTO2025,  POA2026, PROGRA2026, PRESUPUESTO2026,  NULL POA2027, NULL PROGRA2027, NULL PRESUPUESTO2027,  NULL POA2028, NULL PROGRA2028, NULL PRESUPUESTO2028,  SPPRO$RESTRICTIVA  FROM (SELECT P.SPPRO$ID_PROGRAMA_PRESUPUESTO NOPROGRAMA, P.SPPRO$DESCRIPCION PROGRAMA, CASE WHEN P.SPPRO$ID_PROGRAMA_DEPENDE IS NULL THEN P.SPPRO$ID_PROGRAMA_PRESUPUESTO ELSE P.SPPRO$ID_PROGRAMA_DEPENDE END AS ESSUBPROGRAMADE, P.SPPRO$ID_POM, P.SPPRO$ID_INSTITUCION, ";
                                for (int i = 0; i < tabla.Rows.Count; i++)
                                {
                                    sql = sql + "SCHE$SIPLAN20.FNC$POA(P.SPPRO$ID_POM, P.SPPRO$ID_INSTITUCION," + tabla.Rows[i]["SPOA$ANIO"] + ") POA" + tabla.Rows[i]["SPOA$ANIO"] + ",SCHE$SIPLAN20.FNC$PROGRAPRESUPUESTO(P.SPPRO$ID_PROGRAMA_PRESUPUESTO," + tabla.Rows[i]["SPOA$ID_POA"] + " ," + tabla.Rows[i]["SPOA$ANIO"] + ", P.SPPRO$ID_POM, P.SPPRO$ID_INSTITUCION) PROGRA" + tabla.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$OBTIENEPRESUPUESTO(P.SPPRO$ID_PROGRAMA_PRESUPUESTO," + tabla.Rows[i]["SPOA$ID_POA"] + " ," + tabla.Rows[i]["SPOA$ANIO"] + ", P.SPPRO$ID_POM, P.SPPRO$ID_INSTITUCION) PRESUPUESTO" + tabla.Rows[i]["SPOA$ANIO"] + ", ";
                                }

                                sql = sql + "P.SPPRO$RESTRICTIVA FROM SCHE$SIPLAN20.SP20$PROGRAMA_PRESUPUESTARIO P WHERE P.SPPRO$ID_PROGRAMA_DEPENDE IS NULL AND P.SPPRO$RESTRICTIVA = 'N' UNION ";
                                sql = sql + "SELECT P.SPPRO$ID_PROGRAMA_PRESUPUESTO NOPROGRAMA, P.SPPRO$DESCRIPCION PROGRAMA, P.SPPRO$ID_PROGRAMA_DEPENDE ESSUBPROGRAMADE,  P.SPPRO$ID_POM, P.SPPRO$ID_INSTITUCION, ";
                                for (int i = 0; i < tabla.Rows.Count; i++)
                                {
                                    sql = sql + "SCHE$SIPLAN20.FNC$POA(P.SPPRO$ID_POM, P.SPPRO$ID_INSTITUCION," + tabla.Rows[i]["SPOA$ANIO"] + ") POA" + tabla.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$PROGRAPRESUPUESTO(P.SPPRO$ID_PROGRAMA_PRESUPUESTO," + tabla.Rows[i]["SPOA$ID_POA"] + " ," + tabla.Rows[i]["SPOA$ANIO"] + ", P.SPPRO$ID_POM, P.SPPRO$ID_INSTITUCION) PROGRA" + tabla.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$OBTIENEPRESUPUESTO(P.SPPRO$ID_PROGRAMA_PRESUPUESTO," + tabla.Rows[i]["SPOA$ID_POA"] + " ," + tabla.Rows[i]["SPOA$ANIO"] + ", P.SPPRO$ID_POM, P.SPPRO$ID_INSTITUCION) PRESUPUESTO" + tabla.Rows[i]["SPOA$ANIO"] + ", ";
                                }
                                sql = sql + "P.SPPRO$RESTRICTIVA FROM SCHE$SIPLAN20.SP20$PROGRAMA_PRESUPUESTARIO P WHERE P.SPPRO$ID_PROGRAMA_DEPENDE IS NOT NULL AND P.SPPRO$RESTRICTIVA = 'N') WHERE SPPRO$ID_POM = " + Session["pom"] + "  AND SPPRO$ID_INSTITUCION = " + Session["insto"] + " ORDER BY NOPROGRAMA ASC ";

                            }


                        }

                    }


                    else if (Convert.ToInt32(Session["periodo"]) == 22)
                    {

                        sql = "SELECT * FROM SCHE$SIPLAN20.SP20$PROGRA_PRESUPUESTO WHERE SPRES$POM = " + tabla.Rows[0]["SPOA$ID_POM"] + " AND SPPRES$RESTRICTIVA = 'N'";
                        estado = dao.consulta(sql);
                        if (estado == 1)
                            tablas = dao.tabla;
                        if (tablas.Rows.Count > 0)
                        {
                            sql = "SELECT NOPROGRAMA, PROGRAMA,ESSUBPROGRAMADE, SPPRO$ID_POM, SPPRO$ID_INSTITUCION, NULL POA2020, NULL PROGRA2020, NULL PRESUPUESTO2020 , NULL POA2021, NULL PROGRA2021, NULL PRESUPUESTO2021, NULL POA2022, NULL PROGRA2022, NULL PRESUPUESTO2022, POA2023, PROGRA2023, PRESUPUESTO2023, POA2024, PROGRA2024, PRESUPUESTO2024, POA2025, PROGRA2025, PRESUPUESTO2025,   POA2026, PROGRA2026, PRESUPUESTO2026,  POA2027, PROGRA2027, PRESUPUESTO2027, NULL POA2028,  NULL PROGRA2028, NULL PRESUPUESTO2028, SPPRO$RESTRICTIVA  FROM (SELECT P.SPPRO$ID_PROGRAMA_PRESUPUESTO NOPROGRAMA, P.SPPRO$DESCRIPCION PROGRAMA, CASE WHEN P.SPPRO$ID_PROGRAMA_DEPENDE IS NULL THEN P.SPPRO$ID_PROGRAMA_PRESUPUESTO ELSE P.SPPRO$ID_PROGRAMA_DEPENDE END AS ESSUBPROGRAMADE, P.SPPRO$ID_POM, P.SPPRO$ID_INSTITUCION, ";
                            for (int i = 0; i < tabla.Rows.Count; i++)
                            {
                                sql = sql + "SCHE$SIPLAN20.FNC$POA(P.SPPRO$ID_POM, P.SPPRO$ID_INSTITUCION," + tabla.Rows[i]["SPOA$ANIO"] + ") POA" + tabla.Rows[i]["SPOA$ANIO"] + ",SCHE$SIPLAN20.FNC$PROGRAPRESUPUESTO(P.SPPRO$ID_PROGRAMA_PRESUPUESTO," + tabla.Rows[i]["SPOA$ID_POA"] + " ," + tabla.Rows[i]["SPOA$ANIO"] + ", P.SPPRO$ID_POM, P.SPPRO$ID_INSTITUCION) PROGRA" + tabla.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$OBTIENEPRESUPUESTO(P.SPPRO$ID_PROGRAMA_PRESUPUESTO," + tabla.Rows[i]["SPOA$ID_POA"] + " ," + tabla.Rows[i]["SPOA$ANIO"] + ", P.SPPRO$ID_POM, P.SPPRO$ID_INSTITUCION) PRESUPUESTO" + tabla.Rows[i]["SPOA$ANIO"] + ", ";
                            }

                            sql = sql + "P.SPPRO$RESTRICTIVA FROM SCHE$SIPLAN20.SP20$PROGRAMA_PRESUPUESTARIO P WHERE P.SPPRO$ID_PROGRAMA_DEPENDE IS NULL AND P.SPPRO$RESTRICTIVA = 'N' UNION ";
                            sql = sql + "SELECT P.SPPRO$ID_PROGRAMA_PRESUPUESTO NOPROGRAMA, P.SPPRO$DESCRIPCION PROGRAMA, P.SPPRO$ID_PROGRAMA_DEPENDE ESSUBPROGRAMADE,  P.SPPRO$ID_POM, P.SPPRO$ID_INSTITUCION, ";
                            for (int i = 0; i < tabla.Rows.Count; i++)
                            {
                                sql = sql + "SCHE$SIPLAN20.FNC$POA(P.SPPRO$ID_POM, P.SPPRO$ID_INSTITUCION," + tabla.Rows[i]["SPOA$ANIO"] + ") POA" + tabla.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$PROGRAPRESUPUESTO(P.SPPRO$ID_PROGRAMA_PRESUPUESTO," + tabla.Rows[i]["SPOA$ID_POA"] + " ," + tabla.Rows[i]["SPOA$ANIO"] + ", P.SPPRO$ID_POM, P.SPPRO$ID_INSTITUCION) PROGRA" + tabla.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$OBTIENEPRESUPUESTO(P.SPPRO$ID_PROGRAMA_PRESUPUESTO," + tabla.Rows[i]["SPOA$ID_POA"] + " ," + tabla.Rows[i]["SPOA$ANIO"] + ", P.SPPRO$ID_POM, P.SPPRO$ID_INSTITUCION) PRESUPUESTO" + tabla.Rows[i]["SPOA$ANIO"] + ", ";
                            }
                            sql = sql + "P.SPPRO$RESTRICTIVA FROM SCHE$SIPLAN20.SP20$PROGRAMA_PRESUPUESTARIO P WHERE P.SPPRO$ID_PROGRAMA_DEPENDE IS NOT NULL AND P.SPPRO$RESTRICTIVA = 'N') WHERE SPPRO$ID_POM = " + Session["pom"] + "  AND SPPRO$ID_INSTITUCION = " + Session["insto"] + " ORDER BY NOPROGRAMA ASC ";
                        }
                        else
                        {
                            sql = "SELECT (SELECT SPOA$ID_POA FROM SCHE$SIPLAN20.SP20$POA POA WHERE POA.SPOA$ID_POM = " + tabla.Rows[0]["SPOA$ID_POM"] + " AND POA.SPOA$ID_INSTITUCION = " + tabla.Rows[0]["SPOA$ID_INSTITUCION"] + " AND SPOA$RESTRICTIVA = 'N' AND SPOA$ANIO = SPPRES$ANIO) POA, PR.SPPRES$ID_PROGRAMA_PRESUPUESTO,PR.SPPRES$ANIO,PR.SPPRES$PRESUPUESTO, PR.SPPRES$PRESUPUESTO_VIGENTE FROM SCHE$SIPLAN20.SP20$PROGRA_PRESUPUESTO PR INNER JOIN SCHE$SIPLAN20.SP20$PROGRAMA_PRESUPUESTARIO PO ON PR.SPRES$POM = PO.SPPRO$ID_POM AND PR.SPRES$INSTO = PO.SPPRO$ID_INSTITUCION AND PR.SPPRES$RESTRICTIVA = 'N' AND PO.SPPRO$RESTRICTIVA = 'N' INNER JOIN SCHE$SIPLAN20.SP20$POM POM ON POM.SPPO$ID_POM = PO.SPPRO$ID_POM AND POM.SPPO$ID_INSTITUCION = PO.SPPRO$ID_INSTITUCION AND POM.SPPO$RESTRICTIVA = 'N' AND PO.SPPRO$RESTRICTIVA = 'N' INNER JOIN SCHE$SIPLAN20.SP20$PERIODO P ON P.SPP$ID_PERIODO = POM.SPPO$ID_PERIODO AND P.SPP$RESTRICTIVA = 'N' AND POM.SPPO$RESTRICTIVA = 'N' WHERE P.SPP$ORDEN = " + (Convert.ToInt32(tabla.Rows[0]["SPP$ORDEN"]) - 1) + " AND POM.SPPO$ID_INSTITUCION = " + tabla.Rows[0]["SPOA$ID_INSTITUCION"] + " AND PR.SPPRES$ANIO BETWEEN " + Convert.ToInt32(tabla.Rows[0]["SPP$INICIO"]) + " AND " + (Convert.ToInt32(tabla.Rows[0]["SPP$FINAL"]) - 1) + " GROUP BY PR.SPPRES$ID_PROGRAMA_PRESUPUESTO,PR.SPPRES$ANIO,PR.SPPRES$PRESUPUESTO, PR.SPPRES$PRESUPUESTO_VIGENTE order by PR.SPPRES$ID_PROGRAMA_PRESUPUESTO asc";
                            estado = dao.consulta(sql);
                            if (estado == 1)
                                tablas2 = dao.tabla;
                            if (tablas2.Rows.Count > 0)
                            {
                                for (int i = 0; i < tablas2.Rows.Count; i++)
                                {
                                    estado = insertaPresupuesto(Convert.ToDouble(tablas2.Rows[i]["SPPRES$PRESUPUESTO"]), Convert.ToDouble(tablas2.Rows[i]["SPPRES$ID_PROGRAMA_PRESUPUESTO"].ToString()), Convert.ToInt32(tablas2.Rows[i]["POA"].ToString()), Convert.ToInt32(tablas2.Rows[i]["SPPRES$ANIO"].ToString()), Convert.ToInt32(tabla.Rows[0]["SPOA$ID_POM"].ToString()), Convert.ToInt32(tabla.Rows[0]["SPOA$ID_INSTITUCION"].ToString()));
                                    //if (estado == 0)
                                    //  break;
                                }


                                sql = "SELECT NOPROGRAMA, PROGRAMA,ESSUBPROGRAMADE, SPPRO$ID_POM, SPPRO$ID_INSTITUCION, NULL POA2020, NULL PROGRA2020, NULL PRESUPUESTO2020 ,NULL POA2021, NULL PROGRA2021, NULL PRESUPUESTO2021, NULL POA2022, NULL PROGRA2022, NULL PRESUPUESTO2022, POA2023, PROGRA2023, PRESUPUESTO2023, POA2024, PROGRA2024, PRESUPUESTO2024, POA2025, PROGRA2025, PRESUPUESTO2025, POA2026, PROGRA2026, PRESUPUESTO2026,   POA2027, PROGRA2027, PRESUPUESTO2027, NULL POA2028,  NULL PROGRA2028, NULL PRESUPUESTO2028,  SPPRO$RESTRICTIVA  FROM (SELECT P.SPPRO$ID_PROGRAMA_PRESUPUESTO NOPROGRAMA, P.SPPRO$DESCRIPCION PROGRAMA, CASE WHEN P.SPPRO$ID_PROGRAMA_DEPENDE IS NULL THEN P.SPPRO$ID_PROGRAMA_PRESUPUESTO ELSE P.SPPRO$ID_PROGRAMA_DEPENDE END AS ESSUBPROGRAMADE, P.SPPRO$ID_POM, P.SPPRO$ID_INSTITUCION, ";
                                for (int i = 0; i < tabla.Rows.Count; i++)
                                {
                                    sql = sql + "SCHE$SIPLAN20.FNC$POA(P.SPPRO$ID_POM, P.SPPRO$ID_INSTITUCION," + tabla.Rows[i]["SPOA$ANIO"] + ") POA" + tabla.Rows[i]["SPOA$ANIO"] + ",SCHE$SIPLAN20.FNC$PROGRAPRESUPUESTO(P.SPPRO$ID_PROGRAMA_PRESUPUESTO," + tabla.Rows[i]["SPOA$ID_POA"] + " ," + tabla.Rows[i]["SPOA$ANIO"] + ", P.SPPRO$ID_POM, P.SPPRO$ID_INSTITUCION) PROGRA" + tabla.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$OBTIENEPRESUPUESTO(P.SPPRO$ID_PROGRAMA_PRESUPUESTO," + tabla.Rows[i]["SPOA$ID_POA"] + " ," + tabla.Rows[i]["SPOA$ANIO"] + ", P.SPPRO$ID_POM, P.SPPRO$ID_INSTITUCION) PRESUPUESTO" + tabla.Rows[i]["SPOA$ANIO"] + ", ";
                                }

                                sql = sql + "P.SPPRO$RESTRICTIVA FROM SCHE$SIPLAN20.SP20$PROGRAMA_PRESUPUESTARIO P WHERE P.SPPRO$ID_PROGRAMA_DEPENDE IS NULL AND P.SPPRO$RESTRICTIVA = 'N' UNION ";
                                sql = sql + "SELECT P.SPPRO$ID_PROGRAMA_PRESUPUESTO NOPROGRAMA, P.SPPRO$DESCRIPCION PROGRAMA, P.SPPRO$ID_PROGRAMA_DEPENDE ESSUBPROGRAMADE,  P.SPPRO$ID_POM, P.SPPRO$ID_INSTITUCION, ";
                                for (int i = 0; i < tabla.Rows.Count; i++)
                                {
                                    sql = sql + "SCHE$SIPLAN20.FNC$POA(P.SPPRO$ID_POM, P.SPPRO$ID_INSTITUCION," + tabla.Rows[i]["SPOA$ANIO"] + ") POA" + tabla.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$PROGRAPRESUPUESTO(P.SPPRO$ID_PROGRAMA_PRESUPUESTO," + tabla.Rows[i]["SPOA$ID_POA"] + " ," + tabla.Rows[i]["SPOA$ANIO"] + ", P.SPPRO$ID_POM, P.SPPRO$ID_INSTITUCION) PROGRA" + tabla.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$OBTIENEPRESUPUESTO(P.SPPRO$ID_PROGRAMA_PRESUPUESTO," + tabla.Rows[i]["SPOA$ID_POA"] + " ," + tabla.Rows[i]["SPOA$ANIO"] + ", P.SPPRO$ID_POM, P.SPPRO$ID_INSTITUCION) PRESUPUESTO" + tabla.Rows[i]["SPOA$ANIO"] + ", ";
                                }
                                sql = sql + "P.SPPRO$RESTRICTIVA FROM SCHE$SIPLAN20.SP20$PROGRAMA_PRESUPUESTARIO P WHERE P.SPPRO$ID_PROGRAMA_DEPENDE IS NOT NULL AND P.SPPRO$RESTRICTIVA = 'N') WHERE SPPRO$ID_POM = " + Session["pom"] + "  AND SPPRO$ID_INSTITUCION = " + Session["insto"] + " ORDER BY NOPROGRAMA ASC ";

                            }

                            else
                            {
                                sql = "SELECT NOPROGRAMA, PROGRAMA,ESSUBPROGRAMADE, SPPRO$ID_POM, SPPRO$ID_INSTITUCION, NULL POA2020, NULL PROGRA2020, NULL PRESUPUESTO2020 ,NULL POA2021, NULL PROGRA2021, NULL PRESUPUESTO2021, NULL POA2022, NULL PROGRA2022, NULL PRESUPUESTO2022, POA2023, PROGRA2023, PRESUPUESTO2023, POA2024, PROGRA2024, PRESUPUESTO2024, POA2025, PROGRA2025, PRESUPUESTO2025,  POA2026, PROGRA2026, PRESUPUESTO2026, POA2027, PROGRA2027, PRESUPUESTO2027, NULL POA2028,  NULL PROGRA2028, NULL PRESUPUESTO2028, SPPRO$RESTRICTIVA  FROM (SELECT P.SPPRO$ID_PROGRAMA_PRESUPUESTO NOPROGRAMA, P.SPPRO$DESCRIPCION PROGRAMA, CASE WHEN P.SPPRO$ID_PROGRAMA_DEPENDE IS NULL THEN P.SPPRO$ID_PROGRAMA_PRESUPUESTO ELSE P.SPPRO$ID_PROGRAMA_DEPENDE END AS ESSUBPROGRAMADE, P.SPPRO$ID_POM, P.SPPRO$ID_INSTITUCION, ";
                                for (int i = 0; i < tabla.Rows.Count; i++)
                                {
                                    sql = sql + "SCHE$SIPLAN20.FNC$POA(P.SPPRO$ID_POM, P.SPPRO$ID_INSTITUCION," + tabla.Rows[i]["SPOA$ANIO"] + ") POA" + tabla.Rows[i]["SPOA$ANIO"] + ",SCHE$SIPLAN20.FNC$PROGRAPRESUPUESTO(P.SPPRO$ID_PROGRAMA_PRESUPUESTO," + tabla.Rows[i]["SPOA$ID_POA"] + " ," + tabla.Rows[i]["SPOA$ANIO"] + ", P.SPPRO$ID_POM, P.SPPRO$ID_INSTITUCION) PROGRA" + tabla.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$OBTIENEPRESUPUESTO(P.SPPRO$ID_PROGRAMA_PRESUPUESTO," + tabla.Rows[i]["SPOA$ID_POA"] + " ," + tabla.Rows[i]["SPOA$ANIO"] + ", P.SPPRO$ID_POM, P.SPPRO$ID_INSTITUCION) PRESUPUESTO" + tabla.Rows[i]["SPOA$ANIO"] + ", ";
                                }

                                sql = sql + "P.SPPRO$RESTRICTIVA FROM SCHE$SIPLAN20.SP20$PROGRAMA_PRESUPUESTARIO P WHERE P.SPPRO$ID_PROGRAMA_DEPENDE IS NULL AND P.SPPRO$RESTRICTIVA = 'N' UNION ";
                                sql = sql + "SELECT P.SPPRO$ID_PROGRAMA_PRESUPUESTO NOPROGRAMA, P.SPPRO$DESCRIPCION PROGRAMA, P.SPPRO$ID_PROGRAMA_DEPENDE ESSUBPROGRAMADE,  P.SPPRO$ID_POM, P.SPPRO$ID_INSTITUCION, ";
                                for (int i = 0; i < tabla.Rows.Count; i++)
                                {
                                    sql = sql + "SCHE$SIPLAN20.FNC$POA(P.SPPRO$ID_POM, P.SPPRO$ID_INSTITUCION," + tabla.Rows[i]["SPOA$ANIO"] + ") POA" + tabla.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$PROGRAPRESUPUESTO(P.SPPRO$ID_PROGRAMA_PRESUPUESTO," + tabla.Rows[i]["SPOA$ID_POA"] + " ," + tabla.Rows[i]["SPOA$ANIO"] + ", P.SPPRO$ID_POM, P.SPPRO$ID_INSTITUCION) PROGRA" + tabla.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$OBTIENEPRESUPUESTO(P.SPPRO$ID_PROGRAMA_PRESUPUESTO," + tabla.Rows[i]["SPOA$ID_POA"] + " ," + tabla.Rows[i]["SPOA$ANIO"] + ", P.SPPRO$ID_POM, P.SPPRO$ID_INSTITUCION) PRESUPUESTO" + tabla.Rows[i]["SPOA$ANIO"] + ", ";
                                }
                                sql = sql + "P.SPPRO$RESTRICTIVA FROM SCHE$SIPLAN20.SP20$PROGRAMA_PRESUPUESTARIO P WHERE P.SPPRO$ID_PROGRAMA_DEPENDE IS NOT NULL AND P.SPPRO$RESTRICTIVA = 'N') WHERE SPPRO$ID_POM = " + Session["pom"] + "  AND SPPRO$ID_INSTITUCION = " + Session["insto"] + " ORDER BY NOPROGRAMA ASC ";

                            }


                        }

                    }




                    else if (Convert.ToInt32(Session["periodo"]) == 23)
                    {

                        sql = "SELECT * FROM SCHE$SIPLAN20.SP20$PROGRA_PRESUPUESTO WHERE SPRES$POM = " + tabla.Rows[0]["SPOA$ID_POM"] + " AND SPPRES$RESTRICTIVA = 'N'";
                        estado = dao.consulta(sql);
                        if (estado == 1)
                            tablas = dao.tabla;
                        if (tablas.Rows.Count > 0)
                        {
                            sql = "SELECT NOPROGRAMA, PROGRAMA,ESSUBPROGRAMADE, SPPRO$ID_POM, SPPRO$ID_INSTITUCION, NULL POA2020, NULL PROGRA2020, NULL PRESUPUESTO2020 , NULL POA2021, NULL PROGRA2021, NULL PRESUPUESTO2021, NULL POA2022, NULL PROGRA2022, NULL PRESUPUESTO2022, NULL POA2023, NULL PROGRA2023, NULL PRESUPUESTO2023, POA2024, PROGRA2024, PRESUPUESTO2024, POA2025, PROGRA2025, PRESUPUESTO2025,   POA2026, PROGRA2026, PRESUPUESTO2026,  POA2027, PROGRA2027, PRESUPUESTO2027, POA2028,  PROGRA2028, PRESUPUESTO2028, SPPRO$RESTRICTIVA  FROM (SELECT P.SPPRO$ID_PROGRAMA_PRESUPUESTO NOPROGRAMA, P.SPPRO$DESCRIPCION PROGRAMA, CASE WHEN P.SPPRO$ID_PROGRAMA_DEPENDE IS NULL THEN P.SPPRO$ID_PROGRAMA_PRESUPUESTO ELSE P.SPPRO$ID_PROGRAMA_DEPENDE END AS ESSUBPROGRAMADE, P.SPPRO$ID_POM, P.SPPRO$ID_INSTITUCION, ";
                            for (int i = 0; i < tabla.Rows.Count; i++)
                            {
                                sql = sql + "SCHE$SIPLAN20.FNC$POA(P.SPPRO$ID_POM, P.SPPRO$ID_INSTITUCION," + tabla.Rows[i]["SPOA$ANIO"] + ") POA" + tabla.Rows[i]["SPOA$ANIO"] + ",SCHE$SIPLAN20.FNC$PROGRAPRESUPUESTO(P.SPPRO$ID_PROGRAMA_PRESUPUESTO," + tabla.Rows[i]["SPOA$ID_POA"] + " ," + tabla.Rows[i]["SPOA$ANIO"] + ", P.SPPRO$ID_POM, P.SPPRO$ID_INSTITUCION) PROGRA" + tabla.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$OBTIENEPRESUPUESTO(P.SPPRO$ID_PROGRAMA_PRESUPUESTO," + tabla.Rows[i]["SPOA$ID_POA"] + " ," + tabla.Rows[i]["SPOA$ANIO"] + ", P.SPPRO$ID_POM, P.SPPRO$ID_INSTITUCION) PRESUPUESTO" + tabla.Rows[i]["SPOA$ANIO"] + ", ";
                            }

                            sql = sql + "P.SPPRO$RESTRICTIVA FROM SCHE$SIPLAN20.SP20$PROGRAMA_PRESUPUESTARIO P WHERE P.SPPRO$ID_PROGRAMA_DEPENDE IS NULL AND P.SPPRO$RESTRICTIVA = 'N' UNION ";
                            sql = sql + "SELECT P.SPPRO$ID_PROGRAMA_PRESUPUESTO NOPROGRAMA, P.SPPRO$DESCRIPCION PROGRAMA, P.SPPRO$ID_PROGRAMA_DEPENDE ESSUBPROGRAMADE,  P.SPPRO$ID_POM, P.SPPRO$ID_INSTITUCION, ";
                            for (int i = 0; i < tabla.Rows.Count; i++)
                            {
                                sql = sql + "SCHE$SIPLAN20.FNC$POA(P.SPPRO$ID_POM, P.SPPRO$ID_INSTITUCION," + tabla.Rows[i]["SPOA$ANIO"] + ") POA" + tabla.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$PROGRAPRESUPUESTO(P.SPPRO$ID_PROGRAMA_PRESUPUESTO," + tabla.Rows[i]["SPOA$ID_POA"] + " ," + tabla.Rows[i]["SPOA$ANIO"] + ", P.SPPRO$ID_POM, P.SPPRO$ID_INSTITUCION) PROGRA" + tabla.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$OBTIENEPRESUPUESTO(P.SPPRO$ID_PROGRAMA_PRESUPUESTO," + tabla.Rows[i]["SPOA$ID_POA"] + " ," + tabla.Rows[i]["SPOA$ANIO"] + ", P.SPPRO$ID_POM, P.SPPRO$ID_INSTITUCION) PRESUPUESTO" + tabla.Rows[i]["SPOA$ANIO"] + ", ";
                            }
                            sql = sql + "P.SPPRO$RESTRICTIVA FROM SCHE$SIPLAN20.SP20$PROGRAMA_PRESUPUESTARIO P WHERE P.SPPRO$ID_PROGRAMA_DEPENDE IS NOT NULL AND P.SPPRO$RESTRICTIVA = 'N') WHERE SPPRO$ID_POM = " + Session["pom"] + "  AND SPPRO$ID_INSTITUCION = " + Session["insto"] + " ORDER BY NOPROGRAMA ASC ";
                        }
                        else
                        {
                            sql = "SELECT (SELECT SPOA$ID_POA FROM SCHE$SIPLAN20.SP20$POA POA WHERE POA.SPOA$ID_POM = " + tabla.Rows[0]["SPOA$ID_POM"] + " AND POA.SPOA$ID_INSTITUCION = " + tabla.Rows[0]["SPOA$ID_INSTITUCION"] + " AND SPOA$RESTRICTIVA = 'N' AND SPOA$ANIO = SPPRES$ANIO) POA, PR.SPPRES$ID_PROGRAMA_PRESUPUESTO,PR.SPPRES$ANIO,PR.SPPRES$PRESUPUESTO, PR.SPPRES$PRESUPUESTO_VIGENTE FROM SCHE$SIPLAN20.SP20$PROGRA_PRESUPUESTO PR INNER JOIN SCHE$SIPLAN20.SP20$PROGRAMA_PRESUPUESTARIO PO ON PR.SPRES$POM = PO.SPPRO$ID_POM AND PR.SPRES$INSTO = PO.SPPRO$ID_INSTITUCION AND PR.SPPRES$RESTRICTIVA = 'N' AND PO.SPPRO$RESTRICTIVA = 'N' INNER JOIN SCHE$SIPLAN20.SP20$POM POM ON POM.SPPO$ID_POM = PO.SPPRO$ID_POM AND POM.SPPO$ID_INSTITUCION = PO.SPPRO$ID_INSTITUCION AND POM.SPPO$RESTRICTIVA = 'N' AND PO.SPPRO$RESTRICTIVA = 'N' INNER JOIN SCHE$SIPLAN20.SP20$PERIODO P ON P.SPP$ID_PERIODO = POM.SPPO$ID_PERIODO AND P.SPP$RESTRICTIVA = 'N' AND POM.SPPO$RESTRICTIVA = 'N' WHERE P.SPP$ORDEN = " + (Convert.ToInt32(tabla.Rows[0]["SPP$ORDEN"]) - 1) + " AND POM.SPPO$ID_INSTITUCION = " + tabla.Rows[0]["SPOA$ID_INSTITUCION"] + " AND PR.SPPRES$ANIO BETWEEN " + Convert.ToInt32(tabla.Rows[0]["SPP$INICIO"]) + " AND " + (Convert.ToInt32(tabla.Rows[0]["SPP$FINAL"]) - 1) + " GROUP BY PR.SPPRES$ID_PROGRAMA_PRESUPUESTO,PR.SPPRES$ANIO,PR.SPPRES$PRESUPUESTO, PR.SPPRES$PRESUPUESTO_VIGENTE order by PR.SPPRES$ID_PROGRAMA_PRESUPUESTO asc";
                            estado = dao.consulta(sql);
                            if (estado == 1)
                                tablas2 = dao.tabla;
                            if (tablas2.Rows.Count > 0)
                            {
                                for (int i = 0; i < tablas2.Rows.Count; i++)
                                {
                                    estado = insertaPresupuesto(Convert.ToDouble(tablas2.Rows[i]["SPPRES$PRESUPUESTO"]), Convert.ToDouble(tablas2.Rows[i]["SPPRES$ID_PROGRAMA_PRESUPUESTO"].ToString()), Convert.ToInt32(tablas2.Rows[i]["POA"].ToString()), Convert.ToInt32(tablas2.Rows[i]["SPPRES$ANIO"].ToString()), Convert.ToInt32(tabla.Rows[0]["SPOA$ID_POM"].ToString()), Convert.ToInt32(tabla.Rows[0]["SPOA$ID_INSTITUCION"].ToString()));
                                    //if (estado == 0)
                                    //  break;
                                }


                                sql = "SELECT NOPROGRAMA, PROGRAMA,ESSUBPROGRAMADE, SPPRO$ID_POM, SPPRO$ID_INSTITUCION, NULL POA2020, NULL PROGRA2020, NULL PRESUPUESTO2020 ,NULL POA2021, NULL PROGRA2021, NULL PRESUPUESTO2021, NULL POA2022, NULL PROGRA2022, NULL PRESUPUESTO2022, NULL POA2023, NULL PROGRA2023, NULL PRESUPUESTO2023, POA2024, PROGRA2024, PRESUPUESTO2024, POA2025, PROGRA2025, PRESUPUESTO2025, POA2026, PROGRA2026, PRESUPUESTO2026,   POA2027, PROGRA2027, PRESUPUESTO2027, POA2028,  PROGRA2028, PRESUPUESTO2028,  SPPRO$RESTRICTIVA  FROM (SELECT P.SPPRO$ID_PROGRAMA_PRESUPUESTO NOPROGRAMA, P.SPPRO$DESCRIPCION PROGRAMA, CASE WHEN P.SPPRO$ID_PROGRAMA_DEPENDE IS NULL THEN P.SPPRO$ID_PROGRAMA_PRESUPUESTO ELSE P.SPPRO$ID_PROGRAMA_DEPENDE END AS ESSUBPROGRAMADE, P.SPPRO$ID_POM, P.SPPRO$ID_INSTITUCION, ";
                                for (int i = 0; i < tabla.Rows.Count; i++)
                                {
                                    sql = sql + "SCHE$SIPLAN20.FNC$POA(P.SPPRO$ID_POM, P.SPPRO$ID_INSTITUCION," + tabla.Rows[i]["SPOA$ANIO"] + ") POA" + tabla.Rows[i]["SPOA$ANIO"] + ",SCHE$SIPLAN20.FNC$PROGRAPRESUPUESTO(P.SPPRO$ID_PROGRAMA_PRESUPUESTO," + tabla.Rows[i]["SPOA$ID_POA"] + " ," + tabla.Rows[i]["SPOA$ANIO"] + ", P.SPPRO$ID_POM, P.SPPRO$ID_INSTITUCION) PROGRA" + tabla.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$OBTIENEPRESUPUESTO(P.SPPRO$ID_PROGRAMA_PRESUPUESTO," + tabla.Rows[i]["SPOA$ID_POA"] + " ," + tabla.Rows[i]["SPOA$ANIO"] + ", P.SPPRO$ID_POM, P.SPPRO$ID_INSTITUCION) PRESUPUESTO" + tabla.Rows[i]["SPOA$ANIO"] + ", ";
                                }

                                sql = sql + "P.SPPRO$RESTRICTIVA FROM SCHE$SIPLAN20.SP20$PROGRAMA_PRESUPUESTARIO P WHERE P.SPPRO$ID_PROGRAMA_DEPENDE IS NULL AND P.SPPRO$RESTRICTIVA = 'N' UNION ";
                                sql = sql + "SELECT P.SPPRO$ID_PROGRAMA_PRESUPUESTO NOPROGRAMA, P.SPPRO$DESCRIPCION PROGRAMA, P.SPPRO$ID_PROGRAMA_DEPENDE ESSUBPROGRAMADE,  P.SPPRO$ID_POM, P.SPPRO$ID_INSTITUCION, ";
                                for (int i = 0; i < tabla.Rows.Count; i++)
                                {
                                    sql = sql + "SCHE$SIPLAN20.FNC$POA(P.SPPRO$ID_POM, P.SPPRO$ID_INSTITUCION," + tabla.Rows[i]["SPOA$ANIO"] + ") POA" + tabla.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$PROGRAPRESUPUESTO(P.SPPRO$ID_PROGRAMA_PRESUPUESTO," + tabla.Rows[i]["SPOA$ID_POA"] + " ," + tabla.Rows[i]["SPOA$ANIO"] + ", P.SPPRO$ID_POM, P.SPPRO$ID_INSTITUCION) PROGRA" + tabla.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$OBTIENEPRESUPUESTO(P.SPPRO$ID_PROGRAMA_PRESUPUESTO," + tabla.Rows[i]["SPOA$ID_POA"] + " ," + tabla.Rows[i]["SPOA$ANIO"] + ", P.SPPRO$ID_POM, P.SPPRO$ID_INSTITUCION) PRESUPUESTO" + tabla.Rows[i]["SPOA$ANIO"] + ", ";
                                }
                                sql = sql + "P.SPPRO$RESTRICTIVA FROM SCHE$SIPLAN20.SP20$PROGRAMA_PRESUPUESTARIO P WHERE P.SPPRO$ID_PROGRAMA_DEPENDE IS NOT NULL AND P.SPPRO$RESTRICTIVA = 'N') WHERE SPPRO$ID_POM = " + Session["pom"] + "  AND SPPRO$ID_INSTITUCION = " + Session["insto"] + " ORDER BY NOPROGRAMA ASC ";

                            }

                            else
                            {
                                sql = "SELECT NOPROGRAMA, PROGRAMA,ESSUBPROGRAMADE, SPPRO$ID_POM, SPPRO$ID_INSTITUCION, NULL POA2020, NULL PROGRA2020, NULL PRESUPUESTO2020 ,NULL POA2021, NULL PROGRA2021, NULL PRESUPUESTO2021, NULL POA2022, NULL PROGRA2022, NULL PRESUPUESTO2022, NULL POA2023, NULL PROGRA2023, NULL PRESUPUESTO2023, POA2024, PROGRA2024, PRESUPUESTO2024, POA2025, PROGRA2025, PRESUPUESTO2025,  POA2026, PROGRA2026, PRESUPUESTO2026, POA2027, PROGRA2027, PRESUPUESTO2027, POA2028, PROGRA2028, PRESUPUESTO2028, SPPRO$RESTRICTIVA  FROM (SELECT P.SPPRO$ID_PROGRAMA_PRESUPUESTO NOPROGRAMA, P.SPPRO$DESCRIPCION PROGRAMA, CASE WHEN P.SPPRO$ID_PROGRAMA_DEPENDE IS NULL THEN P.SPPRO$ID_PROGRAMA_PRESUPUESTO ELSE P.SPPRO$ID_PROGRAMA_DEPENDE END AS ESSUBPROGRAMADE, P.SPPRO$ID_POM, P.SPPRO$ID_INSTITUCION, ";
                                for (int i = 0; i < tabla.Rows.Count; i++)
                                {
                                    sql = sql + "SCHE$SIPLAN20.FNC$POA(P.SPPRO$ID_POM, P.SPPRO$ID_INSTITUCION," + tabla.Rows[i]["SPOA$ANIO"] + ") POA" + tabla.Rows[i]["SPOA$ANIO"] + ",SCHE$SIPLAN20.FNC$PROGRAPRESUPUESTO(P.SPPRO$ID_PROGRAMA_PRESUPUESTO," + tabla.Rows[i]["SPOA$ID_POA"] + " ," + tabla.Rows[i]["SPOA$ANIO"] + ", P.SPPRO$ID_POM, P.SPPRO$ID_INSTITUCION) PROGRA" + tabla.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$OBTIENEPRESUPUESTO(P.SPPRO$ID_PROGRAMA_PRESUPUESTO," + tabla.Rows[i]["SPOA$ID_POA"] + " ," + tabla.Rows[i]["SPOA$ANIO"] + ", P.SPPRO$ID_POM, P.SPPRO$ID_INSTITUCION) PRESUPUESTO" + tabla.Rows[i]["SPOA$ANIO"] + ", ";
                                }

                                sql = sql + "P.SPPRO$RESTRICTIVA FROM SCHE$SIPLAN20.SP20$PROGRAMA_PRESUPUESTARIO P WHERE P.SPPRO$ID_PROGRAMA_DEPENDE IS NULL AND P.SPPRO$RESTRICTIVA = 'N' UNION ";
                                sql = sql + "SELECT P.SPPRO$ID_PROGRAMA_PRESUPUESTO NOPROGRAMA, P.SPPRO$DESCRIPCION PROGRAMA, P.SPPRO$ID_PROGRAMA_DEPENDE ESSUBPROGRAMADE,  P.SPPRO$ID_POM, P.SPPRO$ID_INSTITUCION, ";
                                for (int i = 0; i < tabla.Rows.Count; i++)
                                {
                                    sql = sql + "SCHE$SIPLAN20.FNC$POA(P.SPPRO$ID_POM, P.SPPRO$ID_INSTITUCION," + tabla.Rows[i]["SPOA$ANIO"] + ") POA" + tabla.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$PROGRAPRESUPUESTO(P.SPPRO$ID_PROGRAMA_PRESUPUESTO," + tabla.Rows[i]["SPOA$ID_POA"] + " ," + tabla.Rows[i]["SPOA$ANIO"] + ", P.SPPRO$ID_POM, P.SPPRO$ID_INSTITUCION) PROGRA" + tabla.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$OBTIENEPRESUPUESTO(P.SPPRO$ID_PROGRAMA_PRESUPUESTO," + tabla.Rows[i]["SPOA$ID_POA"] + " ," + tabla.Rows[i]["SPOA$ANIO"] + ", P.SPPRO$ID_POM, P.SPPRO$ID_INSTITUCION) PRESUPUESTO" + tabla.Rows[i]["SPOA$ANIO"] + ", ";
                                }
                                sql = sql + "P.SPPRO$RESTRICTIVA FROM SCHE$SIPLAN20.SP20$PROGRAMA_PRESUPUESTARIO P WHERE P.SPPRO$ID_PROGRAMA_DEPENDE IS NOT NULL AND P.SPPRO$RESTRICTIVA = 'N') WHERE SPPRO$ID_POM = " + Session["pom"] + "  AND SPPRO$ID_INSTITUCION = " + Session["insto"] + " ORDER BY NOPROGRAMA ASC ";

                            }


                        }

                    }


                    //periodo nueva pgg

                    else if (Convert.ToInt32(Session["periodo"]) == 24)
                    {

                        sql = "SELECT * FROM SCHE$SIPLAN20.SP20$PROGRA_PRESUPUESTO WHERE SPRES$POM = " + tabla.Rows[0]["SPOA$ID_POM"] + " AND SPRES$INSTO = " + tabla.Rows[0]["SPOA$ID_INSTITUCION"] + " AND SPPRES$RESTRICTIVA = 'N'";
                        estado = dao.consulta(sql);
                        if (estado == 1)
                            tablas = dao.tabla;
                        if (tablas.Rows.Count > 0)
                        {
                            sql = "SELECT NOPROGRAMA, PROGRAMA,ESSUBPROGRAMADE, SPPRO$ID_POM, SPPRO$ID_INSTITUCION, NULL POA2020, NULL PROGRA2020, NULL PRESUPUESTO2020 , NULL POA2021, NULL PROGRA2021, NULL PRESUPUESTO2021, NULL POA2022, NULL PROGRA2022, NULL PRESUPUESTO2022, NULL POA2023, NULL PROGRA2023, NULL PRESUPUESTO2023, NULL POA2024, NULL PROGRA2024, NULL PRESUPUESTO2024, POA2025, PROGRA2025, PRESUPUESTO2025,   POA2026, PROGRA2026, PRESUPUESTO2026,  POA2027, PROGRA2027, PRESUPUESTO2027, POA2028,  PROGRA2028, PRESUPUESTO2028,POA2029,PROGRA2029,PRESUPUESTO2029,SPPRO$RESTRICTIVA  FROM (SELECT P.SPPRO$ID_PROGRAMA_PRESUPUESTO NOPROGRAMA, P.SPPRO$DESCRIPCION PROGRAMA, CASE WHEN P.SPPRO$ID_PROGRAMA_DEPENDE IS NULL THEN P.SPPRO$ID_PROGRAMA_PRESUPUESTO ELSE P.SPPRO$ID_PROGRAMA_DEPENDE END AS ESSUBPROGRAMADE, P.SPPRO$ID_POM, P.SPPRO$ID_INSTITUCION, ";
                            for (int i = 0; i < tabla.Rows.Count; i++)
                            {
                                sql = sql + "SCHE$SIPLAN20.FNC$POA(P.SPPRO$ID_POM, P.SPPRO$ID_INSTITUCION," + tabla.Rows[i]["SPOA$ANIO"] + ") POA" + tabla.Rows[i]["SPOA$ANIO"] + ",SCHE$SIPLAN20.FNC$PROGRAPRESUPUESTO(P.SPPRO$ID_PROGRAMA_PRESUPUESTO," + tabla.Rows[i]["SPOA$ID_POA"] + " ," + tabla.Rows[i]["SPOA$ANIO"] + ", P.SPPRO$ID_POM, P.SPPRO$ID_INSTITUCION) PROGRA" + tabla.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$OBTIENEPRESUPUESTO(P.SPPRO$ID_PROGRAMA_PRESUPUESTO," + tabla.Rows[i]["SPOA$ID_POA"] + " ," + tabla.Rows[i]["SPOA$ANIO"] + ", P.SPPRO$ID_POM, P.SPPRO$ID_INSTITUCION) PRESUPUESTO" + tabla.Rows[i]["SPOA$ANIO"] + ", ";
                            }

                            sql = sql + "P.SPPRO$RESTRICTIVA FROM SCHE$SIPLAN20.SP20$PROGRAMA_PRESUPUESTARIO P WHERE P.SPPRO$ID_PROGRAMA_DEPENDE IS NULL AND P.SPPRO$RESTRICTIVA = 'N' UNION ";
                            sql = sql + "SELECT P.SPPRO$ID_PROGRAMA_PRESUPUESTO NOPROGRAMA, P.SPPRO$DESCRIPCION PROGRAMA, P.SPPRO$ID_PROGRAMA_DEPENDE ESSUBPROGRAMADE,  P.SPPRO$ID_POM, P.SPPRO$ID_INSTITUCION, ";
                            for (int i = 0; i < tabla.Rows.Count; i++)
                            {
                                sql = sql + "SCHE$SIPLAN20.FNC$POA(P.SPPRO$ID_POM, P.SPPRO$ID_INSTITUCION," + tabla.Rows[i]["SPOA$ANIO"] + ") POA" + tabla.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$PROGRAPRESUPUESTO(P.SPPRO$ID_PROGRAMA_PRESUPUESTO," + tabla.Rows[i]["SPOA$ID_POA"] + " ," + tabla.Rows[i]["SPOA$ANIO"] + ", P.SPPRO$ID_POM, P.SPPRO$ID_INSTITUCION) PROGRA" + tabla.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$OBTIENEPRESUPUESTO(P.SPPRO$ID_PROGRAMA_PRESUPUESTO," + tabla.Rows[i]["SPOA$ID_POA"] + " ," + tabla.Rows[i]["SPOA$ANIO"] + ", P.SPPRO$ID_POM, P.SPPRO$ID_INSTITUCION) PRESUPUESTO" + tabla.Rows[i]["SPOA$ANIO"] + ", ";
                            }
                            sql = sql + "P.SPPRO$RESTRICTIVA FROM SCHE$SIPLAN20.SP20$PROGRAMA_PRESUPUESTARIO P WHERE P.SPPRO$ID_PROGRAMA_DEPENDE IS NOT NULL AND P.SPPRO$RESTRICTIVA = 'N') WHERE SPPRO$ID_POM = " + Session["pom"] + "  AND SPPRO$ID_INSTITUCION = " + Session["insto"] + " ORDER BY NOPROGRAMA ASC ";
                        }
                        else
                        {
                            sql = "SELECT (SELECT SPOA$ID_POA FROM SCHE$SIPLAN20.SP20$POA POA WHERE POA.SPOA$ID_POM = " + tabla.Rows[0]["SPOA$ID_POM"] + " AND POA.SPOA$ID_INSTITUCION = " + tabla.Rows[0]["SPOA$ID_INSTITUCION"] + " AND SPOA$RESTRICTIVA = 'N' AND SPOA$ANIO = SPPRES$ANIO) POA, PR.SPPRES$ID_PROGRAMA_PRESUPUESTO,PR.SPPRES$ANIO,PR.SPPRES$PRESUPUESTO, PR.SPPRES$PRESUPUESTO_VIGENTE FROM SCHE$SIPLAN20.SP20$PROGRA_PRESUPUESTO PR INNER JOIN SCHE$SIPLAN20.SP20$PROGRAMA_PRESUPUESTARIO PO ON PR.SPRES$POM = PO.SPPRO$ID_POM AND PR.SPRES$INSTO = PO.SPPRO$ID_INSTITUCION AND PR.SPPRES$RESTRICTIVA = 'N' AND PO.SPPRO$RESTRICTIVA = 'N' INNER JOIN SCHE$SIPLAN20.SP20$POM POM ON POM.SPPO$ID_POM = PO.SPPRO$ID_POM AND POM.SPPO$ID_INSTITUCION = PO.SPPRO$ID_INSTITUCION AND POM.SPPO$RESTRICTIVA = 'N' AND PO.SPPRO$RESTRICTIVA = 'N' INNER JOIN SCHE$SIPLAN20.SP20$PERIODO P ON P.SPP$ID_PERIODO = POM.SPPO$ID_PERIODO AND P.SPP$RESTRICTIVA = 'N' AND POM.SPPO$RESTRICTIVA = 'N' WHERE P.SPP$ORDEN = " + (Convert.ToInt32(tabla.Rows[0]["SPP$ORDEN"]) - 1) + " AND POM.SPPO$ID_INSTITUCION = " + tabla.Rows[0]["SPOA$ID_INSTITUCION"] + " AND PR.SPPRES$ANIO BETWEEN " + Convert.ToInt32(tabla.Rows[0]["SPP$INICIO"]) + " AND " + (Convert.ToInt32(tabla.Rows[0]["SPP$FINAL"]) - 1) + " GROUP BY PR.SPPRES$ID_PROGRAMA_PRESUPUESTO,PR.SPPRES$ANIO,PR.SPPRES$PRESUPUESTO, PR.SPPRES$PRESUPUESTO_VIGENTE order by PR.SPPRES$ID_PROGRAMA_PRESUPUESTO asc";
                            estado = dao.consulta(sql);
                            if (estado == 1)
                                tablas2 = dao.tabla;
                            if (tablas2.Rows.Count > 0)
                            {
                                for (int i = 0; i < tablas2.Rows.Count; i++)
                                {
                                    estado = insertaPresupuesto(Convert.ToDouble(tablas2.Rows[i]["SPPRES$PRESUPUESTO"]), Convert.ToDouble(tablas2.Rows[i]["SPPRES$ID_PROGRAMA_PRESUPUESTO"].ToString()), Convert.ToInt32(tablas2.Rows[i]["POA"].ToString()), Convert.ToInt32(tablas2.Rows[i]["SPPRES$ANIO"].ToString()), Convert.ToInt32(tabla.Rows[0]["SPOA$ID_POM"].ToString()), Convert.ToInt32(tabla.Rows[0]["SPOA$ID_INSTITUCION"].ToString()));
                                    //if (estado == 0)
                                    //  break;
                                }


                                sql = "SELECT NOPROGRAMA, PROGRAMA,ESSUBPROGRAMADE, SPPRO$ID_POM, SPPRO$ID_INSTITUCION, NULL POA2020, NULL PROGRA2020, NULL PRESUPUESTO2020 ,NULL POA2021, NULL PROGRA2021, NULL PRESUPUESTO2021, NULL POA2022, NULL PROGRA2022, NULL PRESUPUESTO2022, NULL POA2023, NULL PROGRA2023, NULL PRESUPUESTO2023, NULL POA2024, NULL PROGRA2024, NULL PRESUPUESTO2024, POA2025, PROGRA2025, PRESUPUESTO2025, POA2026, PROGRA2026, PRESUPUESTO2026,   POA2027, PROGRA2027, PRESUPUESTO2027, POA2028,  PROGRA2028, PRESUPUESTO2028, POA2029, PROGRA2029,PRESUPUESTO2029,SPPRO$RESTRICTIVA  FROM (SELECT P.SPPRO$ID_PROGRAMA_PRESUPUESTO NOPROGRAMA, P.SPPRO$DESCRIPCION PROGRAMA, CASE WHEN P.SPPRO$ID_PROGRAMA_DEPENDE IS NULL THEN P.SPPRO$ID_PROGRAMA_PRESUPUESTO ELSE P.SPPRO$ID_PROGRAMA_DEPENDE END AS ESSUBPROGRAMADE, P.SPPRO$ID_POM, P.SPPRO$ID_INSTITUCION, ";
                                for (int i = 0; i < tabla.Rows.Count; i++)
                                {
                                    sql = sql + "SCHE$SIPLAN20.FNC$POA(P.SPPRO$ID_POM, P.SPPRO$ID_INSTITUCION," + tabla.Rows[i]["SPOA$ANIO"] + ") POA" + tabla.Rows[i]["SPOA$ANIO"] + ",SCHE$SIPLAN20.FNC$PROGRAPRESUPUESTO(P.SPPRO$ID_PROGRAMA_PRESUPUESTO," + tabla.Rows[i]["SPOA$ID_POA"] + " ," + tabla.Rows[i]["SPOA$ANIO"] + ", P.SPPRO$ID_POM, P.SPPRO$ID_INSTITUCION) PROGRA" + tabla.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$OBTIENEPRESUPUESTO(P.SPPRO$ID_PROGRAMA_PRESUPUESTO," + tabla.Rows[i]["SPOA$ID_POA"] + " ," + tabla.Rows[i]["SPOA$ANIO"] + ", P.SPPRO$ID_POM, P.SPPRO$ID_INSTITUCION) PRESUPUESTO" + tabla.Rows[i]["SPOA$ANIO"] + ", ";
                                }

                                sql = sql + "P.SPPRO$RESTRICTIVA FROM SCHE$SIPLAN20.SP20$PROGRAMA_PRESUPUESTARIO P WHERE P.SPPRO$ID_PROGRAMA_DEPENDE IS NULL AND P.SPPRO$RESTRICTIVA = 'N' UNION ";
                                sql = sql + "SELECT P.SPPRO$ID_PROGRAMA_PRESUPUESTO NOPROGRAMA, P.SPPRO$DESCRIPCION PROGRAMA, P.SPPRO$ID_PROGRAMA_DEPENDE ESSUBPROGRAMADE,  P.SPPRO$ID_POM, P.SPPRO$ID_INSTITUCION, ";
                                for (int i = 0; i < tabla.Rows.Count; i++)
                                {
                                    sql = sql + "SCHE$SIPLAN20.FNC$POA(P.SPPRO$ID_POM, P.SPPRO$ID_INSTITUCION," + tabla.Rows[i]["SPOA$ANIO"] + ") POA" + tabla.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$PROGRAPRESUPUESTO(P.SPPRO$ID_PROGRAMA_PRESUPUESTO," + tabla.Rows[i]["SPOA$ID_POA"] + " ," + tabla.Rows[i]["SPOA$ANIO"] + ", P.SPPRO$ID_POM, P.SPPRO$ID_INSTITUCION) PROGRA" + tabla.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$OBTIENEPRESUPUESTO(P.SPPRO$ID_PROGRAMA_PRESUPUESTO," + tabla.Rows[i]["SPOA$ID_POA"] + " ," + tabla.Rows[i]["SPOA$ANIO"] + ", P.SPPRO$ID_POM, P.SPPRO$ID_INSTITUCION) PRESUPUESTO" + tabla.Rows[i]["SPOA$ANIO"] + ", ";
                                }
                                sql = sql + "P.SPPRO$RESTRICTIVA FROM SCHE$SIPLAN20.SP20$PROGRAMA_PRESUPUESTARIO P WHERE P.SPPRO$ID_PROGRAMA_DEPENDE IS NOT NULL AND P.SPPRO$RESTRICTIVA = 'N') WHERE SPPRO$ID_POM = " + Session["pom"] + "  AND SPPRO$ID_INSTITUCION = " + Session["insto"] + " ORDER BY NOPROGRAMA ASC ";

                            }

                            else
                            {
                                sql = "SELECT NOPROGRAMA, PROGRAMA,ESSUBPROGRAMADE, SPPRO$ID_POM, SPPRO$ID_INSTITUCION, NULL POA2020, NULL PROGRA2020, NULL PRESUPUESTO2020 ,NULL POA2021, NULL PROGRA2021, NULL PRESUPUESTO2021, NULL POA2022, NULL PROGRA2022, NULL PRESUPUESTO2022, NULL POA2023, NULL PROGRA2023, NULL PRESUPUESTO2023, NULL POA2024, NULL PROGRA2024, NULL PRESUPUESTO2024, POA2025, PROGRA2025, PRESUPUESTO2025,  POA2026, PROGRA2026, PRESUPUESTO2026, POA2027, PROGRA2027, PRESUPUESTO2027, POA2028, PROGRA2028, PRESUPUESTO2028, POA2029, PROGRA2029, PRESUPUESTO2029,SPPRO$RESTRICTIVA  FROM (SELECT P.SPPRO$ID_PROGRAMA_PRESUPUESTO NOPROGRAMA, P.SPPRO$DESCRIPCION PROGRAMA, CASE WHEN P.SPPRO$ID_PROGRAMA_DEPENDE IS NULL THEN P.SPPRO$ID_PROGRAMA_PRESUPUESTO ELSE P.SPPRO$ID_PROGRAMA_DEPENDE END AS ESSUBPROGRAMADE, P.SPPRO$ID_POM, P.SPPRO$ID_INSTITUCION, ";
                                for (int i = 0; i < tabla.Rows.Count; i++)
                                {
                                    sql = sql + "SCHE$SIPLAN20.FNC$POA(P.SPPRO$ID_POM, P.SPPRO$ID_INSTITUCION," + tabla.Rows[i]["SPOA$ANIO"] + ") POA" + tabla.Rows[i]["SPOA$ANIO"] + ",SCHE$SIPLAN20.FNC$PROGRAPRESUPUESTO(P.SPPRO$ID_PROGRAMA_PRESUPUESTO," + tabla.Rows[i]["SPOA$ID_POA"] + " ," + tabla.Rows[i]["SPOA$ANIO"] + ", P.SPPRO$ID_POM, P.SPPRO$ID_INSTITUCION) PROGRA" + tabla.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$OBTIENEPRESUPUESTO(P.SPPRO$ID_PROGRAMA_PRESUPUESTO," + tabla.Rows[i]["SPOA$ID_POA"] + " ," + tabla.Rows[i]["SPOA$ANIO"] + ", P.SPPRO$ID_POM, P.SPPRO$ID_INSTITUCION) PRESUPUESTO" + tabla.Rows[i]["SPOA$ANIO"] + ", ";
                                }

                                sql = sql + "P.SPPRO$RESTRICTIVA FROM SCHE$SIPLAN20.SP20$PROGRAMA_PRESUPUESTARIO P WHERE P.SPPRO$ID_PROGRAMA_DEPENDE IS NULL AND P.SPPRO$RESTRICTIVA = 'N' UNION ";
                                sql = sql + "SELECT P.SPPRO$ID_PROGRAMA_PRESUPUESTO NOPROGRAMA, P.SPPRO$DESCRIPCION PROGRAMA, P.SPPRO$ID_PROGRAMA_DEPENDE ESSUBPROGRAMADE,  P.SPPRO$ID_POM, P.SPPRO$ID_INSTITUCION, ";
                                for (int i = 0; i < tabla.Rows.Count; i++)
                                {
                                    sql = sql + "SCHE$SIPLAN20.FNC$POA(P.SPPRO$ID_POM, P.SPPRO$ID_INSTITUCION," + tabla.Rows[i]["SPOA$ANIO"] + ") POA" + tabla.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$PROGRAPRESUPUESTO(P.SPPRO$ID_PROGRAMA_PRESUPUESTO," + tabla.Rows[i]["SPOA$ID_POA"] + " ," + tabla.Rows[i]["SPOA$ANIO"] + ", P.SPPRO$ID_POM, P.SPPRO$ID_INSTITUCION) PROGRA" + tabla.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$OBTIENEPRESUPUESTO(P.SPPRO$ID_PROGRAMA_PRESUPUESTO," + tabla.Rows[i]["SPOA$ID_POA"] + " ," + tabla.Rows[i]["SPOA$ANIO"] + ", P.SPPRO$ID_POM, P.SPPRO$ID_INSTITUCION) PRESUPUESTO" + tabla.Rows[i]["SPOA$ANIO"] + ", ";
                                }
                                sql = sql + "P.SPPRO$RESTRICTIVA FROM SCHE$SIPLAN20.SP20$PROGRAMA_PRESUPUESTARIO P WHERE P.SPPRO$ID_PROGRAMA_DEPENDE IS NOT NULL AND P.SPPRO$RESTRICTIVA = 'N') WHERE SPPRO$ID_POM = " + Session["pom"] + "  AND SPPRO$ID_INSTITUCION = " + Session["insto"] + " ORDER BY NOPROGRAMA ASC ";

                            }


                        }

                    }

                    //periodo nueva pgg

                    //inicio periodo 2026-2030

                    else if (Convert.ToInt32(Session["periodo"]) == 25)
                    {

                        sql = "SELECT * FROM SCHE$SIPLAN20.SP20$PROGRA_PRESUPUESTO WHERE SPRES$POM = " + tabla.Rows[0]["SPOA$ID_POM"] + " AND SPRES$INSTO = " + tabla.Rows[0]["SPOA$ID_INSTITUCION"] + " AND SPPRES$RESTRICTIVA = 'N'";
                        estado = dao.consulta(sql);
                        if (estado == 1)
                            tablas = dao.tabla;
                        if (tablas.Rows.Count > 0)
                        {
                            sql = "SELECT NOPROGRAMA, PROGRAMA,ESSUBPROGRAMADE, SPPRO$ID_POM, SPPRO$ID_INSTITUCION, NULL POA2020, NULL PROGRA2020, NULL PRESUPUESTO2020 , NULL POA2021, NULL PROGRA2021, NULL PRESUPUESTO2021, NULL POA2022, NULL PROGRA2022, NULL PRESUPUESTO2022, NULL POA2023, NULL PROGRA2023, NULL PRESUPUESTO2023, NULL POA2024, NULL PROGRA2024, NULL PRESUPUESTO2024, NULL POA2025, NULL PROGRA2025, NULL PRESUPUESTO2025,   POA2026, PROGRA2026, PRESUPUESTO2026,  POA2027, PROGRA2027, PRESUPUESTO2027, POA2028,  PROGRA2028, PRESUPUESTO2028,POA2029,PROGRA2029,PRESUPUESTO2029,  POA2030,PROGRA2030,PRESUPUESTO2030  ,SPPRO$RESTRICTIVA  FROM (SELECT P.SPPRO$ID_PROGRAMA_PRESUPUESTO NOPROGRAMA, P.SPPRO$DESCRIPCION PROGRAMA, CASE WHEN P.SPPRO$ID_PROGRAMA_DEPENDE IS NULL THEN P.SPPRO$ID_PROGRAMA_PRESUPUESTO ELSE P.SPPRO$ID_PROGRAMA_DEPENDE END AS ESSUBPROGRAMADE, P.SPPRO$ID_POM, P.SPPRO$ID_INSTITUCION, ";
                            for (int i = 0; i < tabla.Rows.Count; i++)
                            {
                                sql = sql + "SCHE$SIPLAN20.FNC$POA(P.SPPRO$ID_POM, P.SPPRO$ID_INSTITUCION," + tabla.Rows[i]["SPOA$ANIO"] + ") POA" + tabla.Rows[i]["SPOA$ANIO"] + ",SCHE$SIPLAN20.FNC$PROGRAPRESUPUESTO(P.SPPRO$ID_PROGRAMA_PRESUPUESTO," + tabla.Rows[i]["SPOA$ID_POA"] + " ," + tabla.Rows[i]["SPOA$ANIO"] + ", P.SPPRO$ID_POM, P.SPPRO$ID_INSTITUCION) PROGRA" + tabla.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$OBTIENEPRESUPUESTO(P.SPPRO$ID_PROGRAMA_PRESUPUESTO," + tabla.Rows[i]["SPOA$ID_POA"] + " ," + tabla.Rows[i]["SPOA$ANIO"] + ", P.SPPRO$ID_POM, P.SPPRO$ID_INSTITUCION) PRESUPUESTO" + tabla.Rows[i]["SPOA$ANIO"] + ", ";
                            }

                            sql = sql + "P.SPPRO$RESTRICTIVA FROM SCHE$SIPLAN20.SP20$PROGRAMA_PRESUPUESTARIO P WHERE P.SPPRO$ID_PROGRAMA_DEPENDE IS NULL AND P.SPPRO$RESTRICTIVA = 'N' UNION ";
                            sql = sql + "SELECT P.SPPRO$ID_PROGRAMA_PRESUPUESTO NOPROGRAMA, P.SPPRO$DESCRIPCION PROGRAMA, P.SPPRO$ID_PROGRAMA_DEPENDE ESSUBPROGRAMADE,  P.SPPRO$ID_POM, P.SPPRO$ID_INSTITUCION, ";
                            for (int i = 0; i < tabla.Rows.Count; i++)
                            {
                                sql = sql + "SCHE$SIPLAN20.FNC$POA(P.SPPRO$ID_POM, P.SPPRO$ID_INSTITUCION," + tabla.Rows[i]["SPOA$ANIO"] + ") POA" + tabla.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$PROGRAPRESUPUESTO(P.SPPRO$ID_PROGRAMA_PRESUPUESTO," + tabla.Rows[i]["SPOA$ID_POA"] + " ," + tabla.Rows[i]["SPOA$ANIO"] + ", P.SPPRO$ID_POM, P.SPPRO$ID_INSTITUCION) PROGRA" + tabla.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$OBTIENEPRESUPUESTO(P.SPPRO$ID_PROGRAMA_PRESUPUESTO," + tabla.Rows[i]["SPOA$ID_POA"] + " ," + tabla.Rows[i]["SPOA$ANIO"] + ", P.SPPRO$ID_POM, P.SPPRO$ID_INSTITUCION) PRESUPUESTO" + tabla.Rows[i]["SPOA$ANIO"] + ", ";
                            }
                            sql = sql + "P.SPPRO$RESTRICTIVA FROM SCHE$SIPLAN20.SP20$PROGRAMA_PRESUPUESTARIO P WHERE P.SPPRO$ID_PROGRAMA_DEPENDE IS NOT NULL AND P.SPPRO$RESTRICTIVA = 'N') WHERE SPPRO$ID_POM = " + Session["pom"] + "  AND SPPRO$ID_INSTITUCION = " + Session["insto"] + " ORDER BY NOPROGRAMA ASC ";
                        }
                        else
                        {
                            sql = "SELECT (SELECT SPOA$ID_POA FROM SCHE$SIPLAN20.SP20$POA POA WHERE POA.SPOA$ID_POM = " + tabla.Rows[0]["SPOA$ID_POM"] + " AND POA.SPOA$ID_INSTITUCION = " + tabla.Rows[0]["SPOA$ID_INSTITUCION"] + " AND SPOA$RESTRICTIVA = 'N' AND SPOA$ANIO = SPPRES$ANIO) POA, PR.SPPRES$ID_PROGRAMA_PRESUPUESTO,PR.SPPRES$ANIO,PR.SPPRES$PRESUPUESTO, PR.SPPRES$PRESUPUESTO_VIGENTE FROM SCHE$SIPLAN20.SP20$PROGRA_PRESUPUESTO PR INNER JOIN SCHE$SIPLAN20.SP20$PROGRAMA_PRESUPUESTARIO PO ON PR.SPRES$POM = PO.SPPRO$ID_POM AND PR.SPRES$INSTO = PO.SPPRO$ID_INSTITUCION AND PR.SPPRES$RESTRICTIVA = 'N' AND PO.SPPRO$RESTRICTIVA = 'N' INNER JOIN SCHE$SIPLAN20.SP20$POM POM ON POM.SPPO$ID_POM = PO.SPPRO$ID_POM AND POM.SPPO$ID_INSTITUCION = PO.SPPRO$ID_INSTITUCION AND POM.SPPO$RESTRICTIVA = 'N' AND PO.SPPRO$RESTRICTIVA = 'N' INNER JOIN SCHE$SIPLAN20.SP20$PERIODO P ON P.SPP$ID_PERIODO = POM.SPPO$ID_PERIODO AND P.SPP$RESTRICTIVA = 'N' AND POM.SPPO$RESTRICTIVA = 'N' WHERE P.SPP$ORDEN = " + (Convert.ToInt32(tabla.Rows[0]["SPP$ORDEN"]) - 1) + " AND POM.SPPO$ID_INSTITUCION = " + tabla.Rows[0]["SPOA$ID_INSTITUCION"] + " AND PR.SPPRES$ANIO BETWEEN " + Convert.ToInt32(tabla.Rows[0]["SPP$INICIO"]) + " AND " + (Convert.ToInt32(tabla.Rows[0]["SPP$FINAL"]) - 1) + " GROUP BY PR.SPPRES$ID_PROGRAMA_PRESUPUESTO,PR.SPPRES$ANIO,PR.SPPRES$PRESUPUESTO, PR.SPPRES$PRESUPUESTO_VIGENTE order by PR.SPPRES$ID_PROGRAMA_PRESUPUESTO asc";
                            estado = dao.consulta(sql);
                            if (estado == 1)
                                tablas2 = dao.tabla;
                            if (tablas2.Rows.Count > 0)
                            {
                                for (int i = 0; i < tablas2.Rows.Count; i++)
                                {
                                    estado = insertaPresupuesto(Convert.ToDouble(tablas2.Rows[i]["SPPRES$PRESUPUESTO"]), Convert.ToDouble(tablas2.Rows[i]["SPPRES$ID_PROGRAMA_PRESUPUESTO"].ToString()), Convert.ToInt32(tablas2.Rows[i]["POA"].ToString()), Convert.ToInt32(tablas2.Rows[i]["SPPRES$ANIO"].ToString()), Convert.ToInt32(tabla.Rows[0]["SPOA$ID_POM"].ToString()), Convert.ToInt32(tabla.Rows[0]["SPOA$ID_INSTITUCION"].ToString()));
                                    //if (estado == 0)
                                    //  break;
                                }


                                sql = "SELECT NOPROGRAMA, PROGRAMA,ESSUBPROGRAMADE, SPPRO$ID_POM, SPPRO$ID_INSTITUCION, NULL POA2020, NULL PROGRA2020, NULL PRESUPUESTO2020 ,NULL POA2021, NULL PROGRA2021, NULL PRESUPUESTO2021, NULL POA2022, NULL PROGRA2022, NULL PRESUPUESTO2022, NULL POA2023, NULL PROGRA2023, NULL PRESUPUESTO2023, NULL POA2024, NULL PROGRA2024, NULL PRESUPUESTO2024, NULL POA2025, NULL PROGRA2025, NULL PRESUPUESTO2025, POA2026, PROGRA2026, PRESUPUESTO2026,   POA2027, PROGRA2027, PRESUPUESTO2027, POA2028,  PROGRA2028, PRESUPUESTO2028, POA2029, PROGRA2029,PRESUPUESTO2029, POA2030, PROGRA2030,PRESUPUESTO2030,  SPPRO$RESTRICTIVA  FROM (SELECT P.SPPRO$ID_PROGRAMA_PRESUPUESTO NOPROGRAMA, P.SPPRO$DESCRIPCION PROGRAMA, CASE WHEN P.SPPRO$ID_PROGRAMA_DEPENDE IS NULL THEN P.SPPRO$ID_PROGRAMA_PRESUPUESTO ELSE P.SPPRO$ID_PROGRAMA_DEPENDE END AS ESSUBPROGRAMADE, P.SPPRO$ID_POM, P.SPPRO$ID_INSTITUCION, ";
                                for (int i = 0; i < tabla.Rows.Count; i++)
                                {
                                    sql = sql + "SCHE$SIPLAN20.FNC$POA(P.SPPRO$ID_POM, P.SPPRO$ID_INSTITUCION," + tabla.Rows[i]["SPOA$ANIO"] + ") POA" + tabla.Rows[i]["SPOA$ANIO"] + ",SCHE$SIPLAN20.FNC$PROGRAPRESUPUESTO(P.SPPRO$ID_PROGRAMA_PRESUPUESTO," + tabla.Rows[i]["SPOA$ID_POA"] + " ," + tabla.Rows[i]["SPOA$ANIO"] + ", P.SPPRO$ID_POM, P.SPPRO$ID_INSTITUCION) PROGRA" + tabla.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$OBTIENEPRESUPUESTO(P.SPPRO$ID_PROGRAMA_PRESUPUESTO," + tabla.Rows[i]["SPOA$ID_POA"] + " ," + tabla.Rows[i]["SPOA$ANIO"] + ", P.SPPRO$ID_POM, P.SPPRO$ID_INSTITUCION) PRESUPUESTO" + tabla.Rows[i]["SPOA$ANIO"] + ", ";
                                }

                                sql = sql + "P.SPPRO$RESTRICTIVA FROM SCHE$SIPLAN20.SP20$PROGRAMA_PRESUPUESTARIO P WHERE P.SPPRO$ID_PROGRAMA_DEPENDE IS NULL AND P.SPPRO$RESTRICTIVA = 'N' UNION ";
                                sql = sql + "SELECT P.SPPRO$ID_PROGRAMA_PRESUPUESTO NOPROGRAMA, P.SPPRO$DESCRIPCION PROGRAMA, P.SPPRO$ID_PROGRAMA_DEPENDE ESSUBPROGRAMADE,  P.SPPRO$ID_POM, P.SPPRO$ID_INSTITUCION, ";
                                for (int i = 0; i < tabla.Rows.Count; i++)
                                {
                                    sql = sql + "SCHE$SIPLAN20.FNC$POA(P.SPPRO$ID_POM, P.SPPRO$ID_INSTITUCION," + tabla.Rows[i]["SPOA$ANIO"] + ") POA" + tabla.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$PROGRAPRESUPUESTO(P.SPPRO$ID_PROGRAMA_PRESUPUESTO," + tabla.Rows[i]["SPOA$ID_POA"] + " ," + tabla.Rows[i]["SPOA$ANIO"] + ", P.SPPRO$ID_POM, P.SPPRO$ID_INSTITUCION) PROGRA" + tabla.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$OBTIENEPRESUPUESTO(P.SPPRO$ID_PROGRAMA_PRESUPUESTO," + tabla.Rows[i]["SPOA$ID_POA"] + " ," + tabla.Rows[i]["SPOA$ANIO"] + ", P.SPPRO$ID_POM, P.SPPRO$ID_INSTITUCION) PRESUPUESTO" + tabla.Rows[i]["SPOA$ANIO"] + ", ";
                                }
                                sql = sql + "P.SPPRO$RESTRICTIVA FROM SCHE$SIPLAN20.SP20$PROGRAMA_PRESUPUESTARIO P WHERE P.SPPRO$ID_PROGRAMA_DEPENDE IS NOT NULL AND P.SPPRO$RESTRICTIVA = 'N') WHERE SPPRO$ID_POM = " + Session["pom"] + "  AND SPPRO$ID_INSTITUCION = " + Session["insto"] + " ORDER BY NOPROGRAMA ASC ";

                            }

                            else
                            {
                                sql = "SELECT NOPROGRAMA, PROGRAMA,ESSUBPROGRAMADE, SPPRO$ID_POM, SPPRO$ID_INSTITUCION, NULL POA2020, NULL PROGRA2020, NULL PRESUPUESTO2020 ,NULL POA2021, NULL PROGRA2021, NULL PRESUPUESTO2021, NULL POA2022, NULL PROGRA2022, NULL PRESUPUESTO2022, NULL POA2023, NULL PROGRA2023, NULL PRESUPUESTO2023, NULL POA2024, NULL PROGRA2024, NULL PRESUPUESTO2024, NULL POA2025, NULL PROGRA2025, NULL PRESUPUESTO2025,  POA2026, PROGRA2026, PRESUPUESTO2026, POA2027, PROGRA2027, PRESUPUESTO2027, POA2028, PROGRA2028, PRESUPUESTO2028, POA2029, PROGRA2029, PRESUPUESTO2029, POA2030, PROGRA2030, PRESUPUESTO2030,  SPPRO$RESTRICTIVA  FROM (SELECT P.SPPRO$ID_PROGRAMA_PRESUPUESTO NOPROGRAMA, P.SPPRO$DESCRIPCION PROGRAMA, CASE WHEN P.SPPRO$ID_PROGRAMA_DEPENDE IS NULL THEN P.SPPRO$ID_PROGRAMA_PRESUPUESTO ELSE P.SPPRO$ID_PROGRAMA_DEPENDE END AS ESSUBPROGRAMADE, P.SPPRO$ID_POM, P.SPPRO$ID_INSTITUCION, ";
                                for (int i = 0; i < tabla.Rows.Count; i++)
                                {
                                    sql = sql + "SCHE$SIPLAN20.FNC$POA(P.SPPRO$ID_POM, P.SPPRO$ID_INSTITUCION," + tabla.Rows[i]["SPOA$ANIO"] + ") POA" + tabla.Rows[i]["SPOA$ANIO"] + ",SCHE$SIPLAN20.FNC$PROGRAPRESUPUESTO(P.SPPRO$ID_PROGRAMA_PRESUPUESTO," + tabla.Rows[i]["SPOA$ID_POA"] + " ," + tabla.Rows[i]["SPOA$ANIO"] + ", P.SPPRO$ID_POM, P.SPPRO$ID_INSTITUCION) PROGRA" + tabla.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$OBTIENEPRESUPUESTO(P.SPPRO$ID_PROGRAMA_PRESUPUESTO," + tabla.Rows[i]["SPOA$ID_POA"] + " ," + tabla.Rows[i]["SPOA$ANIO"] + ", P.SPPRO$ID_POM, P.SPPRO$ID_INSTITUCION) PRESUPUESTO" + tabla.Rows[i]["SPOA$ANIO"] + ", ";
                                }

                                sql = sql + "P.SPPRO$RESTRICTIVA FROM SCHE$SIPLAN20.SP20$PROGRAMA_PRESUPUESTARIO P WHERE P.SPPRO$ID_PROGRAMA_DEPENDE IS NULL AND P.SPPRO$RESTRICTIVA = 'N' UNION ";
                                sql = sql + "SELECT P.SPPRO$ID_PROGRAMA_PRESUPUESTO NOPROGRAMA, P.SPPRO$DESCRIPCION PROGRAMA, P.SPPRO$ID_PROGRAMA_DEPENDE ESSUBPROGRAMADE,  P.SPPRO$ID_POM, P.SPPRO$ID_INSTITUCION, ";
                                for (int i = 0; i < tabla.Rows.Count; i++)
                                {
                                    sql = sql + "SCHE$SIPLAN20.FNC$POA(P.SPPRO$ID_POM, P.SPPRO$ID_INSTITUCION," + tabla.Rows[i]["SPOA$ANIO"] + ") POA" + tabla.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$PROGRAPRESUPUESTO(P.SPPRO$ID_PROGRAMA_PRESUPUESTO," + tabla.Rows[i]["SPOA$ID_POA"] + " ," + tabla.Rows[i]["SPOA$ANIO"] + ", P.SPPRO$ID_POM, P.SPPRO$ID_INSTITUCION) PROGRA" + tabla.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$OBTIENEPRESUPUESTO(P.SPPRO$ID_PROGRAMA_PRESUPUESTO," + tabla.Rows[i]["SPOA$ID_POA"] + " ," + tabla.Rows[i]["SPOA$ANIO"] + ", P.SPPRO$ID_POM, P.SPPRO$ID_INSTITUCION) PRESUPUESTO" + tabla.Rows[i]["SPOA$ANIO"] + ", ";
                                }
                                sql = sql + "P.SPPRO$RESTRICTIVA FROM SCHE$SIPLAN20.SP20$PROGRAMA_PRESUPUESTARIO P WHERE P.SPPRO$ID_PROGRAMA_DEPENDE IS NOT NULL AND P.SPPRO$RESTRICTIVA = 'N') WHERE SPPRO$ID_POM = " + Session["pom"] + "  AND SPPRO$ID_INSTITUCION = " + Session["insto"] + " ORDER BY NOPROGRAMA ASC ";

                            }


                        }

                    }

                    //fin periodo 2026-2030


                    else
                    {
                        sql = "SELECT NOPROGRAMA, PROGRAMA,ESSUBPROGRAMADE, SPPRO$ID_POM, SPPRO$ID_INSTITUCION, POA2020, PROGRA2020, PRESUPUESTO2020, POA2021, PROGRA2021, PRESUPUESTO2021, POA2022, PROGRA2022, PRESUPUESTO2022, POA2023, PROGRA2023, PRESUPUESTO2023, POA2024, PROGRA2024, PRESUPUESTO2024, NULL POA2025, NULL PROGRA2025, NULL  PRESUPUESTO2025, NULL POA2026, NULL PROGRA2026, NULL  PRESUPUESTO2026,  NULL POA2027, NULL PROGRA2027, NULL  PRESUPUESTO2027,  NULL POA2028, NULL PROGRA2028, NULL  PRESUPUESTO2028,   SPPRO$RESTRICTIVA  FROM (SELECT P.SPPRO$ID_PROGRAMA_PRESUPUESTO NOPROGRAMA, P.SPPRO$DESCRIPCION PROGRAMA, CASE WHEN P.SPPRO$ID_PROGRAMA_DEPENDE IS NULL THEN P.SPPRO$ID_PROGRAMA_PRESUPUESTO ELSE P.SPPRO$ID_PROGRAMA_DEPENDE END AS ESSUBPROGRAMADE, P.SPPRO$ID_POM, P.SPPRO$ID_INSTITUCION, ";
                        for (int i = 0; i < tabla.Rows.Count; i++)
                        {
                            sql = sql + "SCHE$SIPLAN20.FNC$POA(P.SPPRO$ID_POM, P.SPPRO$ID_INSTITUCION," + tabla.Rows[i]["SPOA$ANIO"] + ") POA" + tabla.Rows[i]["SPOA$ANIO"] + ",SCHE$SIPLAN20.FNC$PROGRAPRESUPUESTO(P.SPPRO$ID_PROGRAMA_PRESUPUESTO," + tabla.Rows[i]["SPOA$ID_POA"] + " ," + tabla.Rows[i]["SPOA$ANIO"] + ", P.SPPRO$ID_POM, P.SPPRO$ID_INSTITUCION) PROGRA" + tabla.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$OBTIENEPRESUPUESTO(P.SPPRO$ID_PROGRAMA_PRESUPUESTO," + tabla.Rows[i]["SPOA$ID_POA"] + " ," + tabla.Rows[i]["SPOA$ANIO"] + ", P.SPPRO$ID_POM, P.SPPRO$ID_INSTITUCION) PRESUPUESTO" + tabla.Rows[i]["SPOA$ANIO"] + ", ";
                        }

                        sql = sql + "P.SPPRO$RESTRICTIVA FROM SCHE$SIPLAN20.SP20$PROGRAMA_PRESUPUESTARIO P WHERE P.SPPRO$ID_PROGRAMA_DEPENDE IS NULL AND P.SPPRO$RESTRICTIVA = 'N' UNION ";
                        sql = sql + "SELECT P.SPPRO$ID_PROGRAMA_PRESUPUESTO NOPROGRAMA, P.SPPRO$DESCRIPCION PROGRAMA, P.SPPRO$ID_PROGRAMA_DEPENDE ESSUBPROGRAMADE,  P.SPPRO$ID_POM, P.SPPRO$ID_INSTITUCION, ";
                        for (int i = 0; i < tabla.Rows.Count; i++)
                        {
                            sql = sql + "SCHE$SIPLAN20.FNC$POA(P.SPPRO$ID_POM, P.SPPRO$ID_INSTITUCION," + tabla.Rows[i]["SPOA$ANIO"] + ") POA" + tabla.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$PROGRAPRESUPUESTO(P.SPPRO$ID_PROGRAMA_PRESUPUESTO," + tabla.Rows[i]["SPOA$ID_POA"] + " ," + tabla.Rows[i]["SPOA$ANIO"] + ", P.SPPRO$ID_POM, P.SPPRO$ID_INSTITUCION) PROGRA" + tabla.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$OBTIENEPRESUPUESTO(P.SPPRO$ID_PROGRAMA_PRESUPUESTO," + tabla.Rows[i]["SPOA$ID_POA"] + " ," + tabla.Rows[i]["SPOA$ANIO"] + ", P.SPPRO$ID_POM, P.SPPRO$ID_INSTITUCION) PRESUPUESTO" + tabla.Rows[i]["SPOA$ANIO"] + ", ";
                        }
                        sql = sql + "P.SPPRO$RESTRICTIVA FROM SCHE$SIPLAN20.SP20$PROGRAMA_PRESUPUESTARIO P WHERE P.SPPRO$ID_PROGRAMA_DEPENDE IS NOT NULL AND P.SPPRO$RESTRICTIVA = 'N') WHERE SPPRO$ID_POM = " + Session["pom"] + "  AND SPPRO$ID_INSTITUCION = " + Session["insto"] + " ORDER BY NOPROGRAMA ASC ";
                    }
                    estado = dao.consulta(sql);
                    if (estado == 0)
                    {
                        mensaje = dao.mensaje;
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
                    }
                    else
                    {
                        tabla = dao.tabla;
                        Session["programas"] = tabla;
                        gvProgramas.DataSource = tabla;

                        gvProgramas.DataBind();
                        gvProgramas.ExpandAll();
                    }

                }
            }


        }

        protected void gvProgramas_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            DataTable programas = new DataTable();
            DataTable presupuestos = new DataTable();
            programas = (DataTable)Session["programas"];
            Double sumasubs = 0;
            Double presupuestopadre = 0;


            for (int i = 0; i < programas.Rows.Count; i++)
            {
                if (Convert.ToInt32(Session["periodo"]) == 20)
                {
                    if (programas.Rows[i]["NOPROGRAMA"].ToString() == e.Keys["NOPROGRAMA"].ToString()) //inicio
                    {
                        if (programas.Rows[i]["PROGRA2021"] == DBNull.Value)
                        {
                            estado = insertaPresupuesto(Convert.ToDouble(e.NewValues["PRESUPUESTO2021"]), Convert.ToDouble(programas.Rows[i]["NOPROGRAMA"].ToString()), Convert.ToInt32(programas.Rows[i]["POA2021"].ToString()), 2021, Convert.ToInt32(programas.Rows[i]["SPPRO$ID_POM"].ToString()), Convert.ToInt32(programas.Rows[i]["SPPRO$ID_INSTITUCION"].ToString()));
                            if (estado == 0)
                            {
                                mensaje = dao.mensaje;
                                gvProgramas.JSProperties["cpError"] = "Ocurrio un error " + mensaje;
                                e.Cancel = true;
                                gvProgramas.CancelEdit();
                                cargaprogramas();
                                break;
                            }

                        }
                        else
                        {
                            estado = ActualizaPresupuesto(Convert.ToDouble(e.NewValues["PRESUPUESTO2021"]), Convert.ToDouble(programas.Rows[i]["NOPROGRAMA"].ToString()), Convert.ToInt32(programas.Rows[i]["POA2021"].ToString()), 2021, Convert.ToInt32(programas.Rows[i]["SPPRO$ID_POM"].ToString()), Convert.ToInt32(programas.Rows[i]["SPPRO$ID_INSTITUCION"].ToString()), Convert.ToInt32(programas.Rows[i]["PROGRA2021"].ToString()));
                            if (estado == 0)
                            {
                                mensaje = dao.mensaje;
                                gvProgramas.JSProperties["cpError"] = "Ocurrio un error " + mensaje;
                                e.Cancel = true;
                                gvProgramas.CancelEdit();
                                cargaprogramas();
                                break;
                            }

                        }


                        if (programas.Rows[i]["PROGRA2022"] == DBNull.Value)
                        {
                            estado = insertaPresupuesto(Convert.ToDouble(e.NewValues["PRESUPUESTO2022"]), Convert.ToDouble(programas.Rows[i]["NOPROGRAMA"].ToString()), Convert.ToInt32(programas.Rows[i]["POA2022"].ToString()), 2022, Convert.ToInt32(programas.Rows[i]["SPPRO$ID_POM"].ToString()), Convert.ToInt32(programas.Rows[i]["SPPRO$ID_INSTITUCION"].ToString()));
                            if (estado == 0)
                            {
                                mensaje = dao.mensaje;
                                gvProgramas.JSProperties["cpError"] = "Ocurrio un error " + mensaje;
                                e.Cancel = true;
                                gvProgramas.CancelEdit();
                                cargaprogramas();
                                break;
                            }

                        }
                        else
                        {
                            estado = ActualizaPresupuesto(Convert.ToDouble(e.NewValues["PRESUPUESTO2022"]), Convert.ToDouble(programas.Rows[i]["NOPROGRAMA"].ToString()), Convert.ToInt32(programas.Rows[i]["POA2022"].ToString()), 2022, Convert.ToInt32(programas.Rows[i]["SPPRO$ID_POM"].ToString()), Convert.ToInt32(programas.Rows[i]["SPPRO$ID_INSTITUCION"].ToString()), Convert.ToInt32(programas.Rows[i]["PROGRA2022"].ToString()));
                            if (estado == 0)
                            {
                                mensaje = dao.mensaje;
                                gvProgramas.JSProperties["cpError"] = "Ocurrio un error " + mensaje;
                                e.Cancel = true;
                                gvProgramas.CancelEdit();
                                cargaprogramas();
                                break;
                            }

                        }


                        if (programas.Rows[i]["PROGRA2023"] == DBNull.Value)
                        {
                            estado = insertaPresupuesto(Convert.ToDouble(e.NewValues["PRESUPUESTO2023"]), Convert.ToDouble(programas.Rows[i]["NOPROGRAMA"].ToString()), Convert.ToInt32(programas.Rows[i]["POA2023"].ToString()), 2023, Convert.ToInt32(programas.Rows[i]["SPPRO$ID_POM"].ToString()), Convert.ToInt32(programas.Rows[i]["SPPRO$ID_INSTITUCION"].ToString()));
                            if (estado == 0)
                            {
                                mensaje = dao.mensaje;
                                gvProgramas.JSProperties["cpError"] = "Ocurrio un error " + mensaje;
                                e.Cancel = true;
                                gvProgramas.CancelEdit();
                                cargaprogramas();
                                break;
                            }

                        }
                        else
                        {
                            estado = ActualizaPresupuesto(Convert.ToDouble(e.NewValues["PRESUPUESTO2023"]), Convert.ToDouble(programas.Rows[i]["NOPROGRAMA"].ToString()), Convert.ToInt32(programas.Rows[i]["POA2023"].ToString()), 2023, Convert.ToInt32(programas.Rows[i]["SPPRO$ID_POM"].ToString()), Convert.ToInt32(programas.Rows[i]["SPPRO$ID_INSTITUCION"].ToString()), Convert.ToInt32(programas.Rows[i]["PROGRA2023"].ToString()));
                            if (estado == 0)
                            {
                                mensaje = dao.mensaje;
                                gvProgramas.JSProperties["cpError"] = "Ocurrio un error " + mensaje;
                                e.Cancel = true;
                                gvProgramas.CancelEdit();
                                cargaprogramas();
                                break;
                            }

                        }


                        if (programas.Rows[i]["PROGRA2024"] == DBNull.Value)
                        {
                            estado = insertaPresupuesto(Convert.ToDouble(e.NewValues["PRESUPUESTO2024"]), Convert.ToDouble(programas.Rows[i]["NOPROGRAMA"].ToString()), Convert.ToInt32(programas.Rows[i]["POA2024"].ToString()), 2024, Convert.ToInt32(programas.Rows[i]["SPPRO$ID_POM"].ToString()), Convert.ToInt32(programas.Rows[i]["SPPRO$ID_INSTITUCION"].ToString()));
                            if (estado == 0)
                            {
                                mensaje = dao.mensaje;
                                gvProgramas.JSProperties["cpError"] = "Ocurrio un error " + mensaje;
                                e.Cancel = true;
                                gvProgramas.CancelEdit();
                                cargaprogramas();
                                break;
                            }

                        }
                        else
                        {
                            estado = ActualizaPresupuesto(Convert.ToDouble(e.NewValues["PRESUPUESTO2024"]), Convert.ToDouble(programas.Rows[i]["NOPROGRAMA"].ToString()), Convert.ToInt32(programas.Rows[i]["POA2024"].ToString()), 2024, Convert.ToInt32(programas.Rows[i]["SPPRO$ID_POM"].ToString()), Convert.ToInt32(programas.Rows[i]["SPPRO$ID_INSTITUCION"].ToString()), Convert.ToInt32(programas.Rows[i]["PROGRA2024"].ToString()));
                            if (estado == 0)
                            {
                                mensaje = dao.mensaje;
                                gvProgramas.JSProperties["cpError"] = "Ocurrio un error " + mensaje;
                                e.Cancel = true;
                                gvProgramas.CancelEdit();
                                cargaprogramas();
                                break;
                            }

                        }


                        if (programas.Rows[i]["PROGRA2025"] == DBNull.Value)
                        {
                            estado = insertaPresupuesto(Convert.ToDouble(e.NewValues["PRESUPUESTO2025"]), Convert.ToDouble(programas.Rows[i]["NOPROGRAMA"].ToString()), Convert.ToInt32(programas.Rows[i]["POA2025"].ToString()), 2025, Convert.ToInt32(programas.Rows[i]["SPPRO$ID_POM"].ToString()), Convert.ToInt32(programas.Rows[i]["SPPRO$ID_INSTITUCION"].ToString()));
                            if (estado == 0)
                            {
                                mensaje = dao.mensaje;
                                gvProgramas.JSProperties["cpError"] = "Ocurrio un error " + mensaje;
                                e.Cancel = true;
                                gvProgramas.CancelEdit();
                                cargaprogramas();
                                break;
                            }

                        }
                        else
                        {
                            estado = ActualizaPresupuesto(Convert.ToDouble(e.NewValues["PRESUPUESTO2025"]), Convert.ToDouble(programas.Rows[i]["NOPROGRAMA"].ToString()), Convert.ToInt32(programas.Rows[i]["POA2025"].ToString()), 2025, Convert.ToInt32(programas.Rows[i]["SPPRO$ID_POM"].ToString()), Convert.ToInt32(programas.Rows[i]["SPPRO$ID_INSTITUCION"].ToString()), Convert.ToInt32(programas.Rows[i]["PROGRA2025"].ToString()));
                            if (estado == 0)
                            {
                                mensaje = dao.mensaje;
                                gvProgramas.JSProperties["cpError"] = "Ocurrio un error " + mensaje;
                                e.Cancel = true;
                                gvProgramas.CancelEdit();
                                cargaprogramas();
                                break;
                            }

                        }


                        if (estado == 1)
                        {
                            mensaje = "Presupuestos registrados correctamente";
                            gvProgramas.JSProperties["cpError"] = "Información: " + mensaje;
                            e.Cancel = true;
                            gvProgramas.CancelEdit();
                            cargaprogramas();
                            break;
                        }

                    }

                }



                else if (Convert.ToInt32(Session["periodo"]) == 21)
                {
                    if (programas.Rows[i]["NOPROGRAMA"].ToString() == e.Keys["NOPROGRAMA"].ToString()) //inicio
                    {
                        if (programas.Rows[i]["PROGRA2022"] == DBNull.Value)
                        {
                            estado = insertaPresupuesto(Convert.ToDouble(e.NewValues["PRESUPUESTO2022"]), Convert.ToDouble(programas.Rows[i]["NOPROGRAMA"].ToString()), Convert.ToInt32(programas.Rows[i]["POA2022"].ToString()), 2022, Convert.ToInt32(programas.Rows[i]["SPPRO$ID_POM"].ToString()), Convert.ToInt32(programas.Rows[i]["SPPRO$ID_INSTITUCION"].ToString()));
                            if (estado == 0)
                            {
                                mensaje = dao.mensaje;
                                gvProgramas.JSProperties["cpError"] = "Ocurrio un error " + mensaje;
                                e.Cancel = true;
                                gvProgramas.CancelEdit();
                                cargaprogramas();
                                break;
                            }

                        }
                        else
                        {
                            estado = ActualizaPresupuesto(Convert.ToDouble(e.NewValues["PRESUPUESTO2022"]), Convert.ToDouble(programas.Rows[i]["NOPROGRAMA"].ToString()), Convert.ToInt32(programas.Rows[i]["POA2022"].ToString()), 2022, Convert.ToInt32(programas.Rows[i]["SPPRO$ID_POM"].ToString()), Convert.ToInt32(programas.Rows[i]["SPPRO$ID_INSTITUCION"].ToString()), Convert.ToInt32(programas.Rows[i]["PROGRA2022"].ToString()));
                            if (estado == 0)
                            {
                                mensaje = dao.mensaje;
                                gvProgramas.JSProperties["cpError"] = "Ocurrio un error " + mensaje;
                                e.Cancel = true;
                                gvProgramas.CancelEdit();
                                cargaprogramas();
                                break;
                            }

                        }


                        if (programas.Rows[i]["PROGRA2023"] == DBNull.Value)
                        {
                            estado = insertaPresupuesto(Convert.ToDouble(e.NewValues["PRESUPUESTO2023"]), Convert.ToDouble(programas.Rows[i]["NOPROGRAMA"].ToString()), Convert.ToInt32(programas.Rows[i]["POA2023"].ToString()), 2023, Convert.ToInt32(programas.Rows[i]["SPPRO$ID_POM"].ToString()), Convert.ToInt32(programas.Rows[i]["SPPRO$ID_INSTITUCION"].ToString()));
                            if (estado == 0)
                            {
                                mensaje = dao.mensaje;
                                gvProgramas.JSProperties["cpError"] = "Ocurrio un error " + mensaje;
                                e.Cancel = true;
                                gvProgramas.CancelEdit();
                                cargaprogramas();
                                break;
                            }

                        }
                        else
                        {
                            estado = ActualizaPresupuesto(Convert.ToDouble(e.NewValues["PRESUPUESTO2023"]), Convert.ToDouble(programas.Rows[i]["NOPROGRAMA"].ToString()), Convert.ToInt32(programas.Rows[i]["POA2023"].ToString()), 2023, Convert.ToInt32(programas.Rows[i]["SPPRO$ID_POM"].ToString()), Convert.ToInt32(programas.Rows[i]["SPPRO$ID_INSTITUCION"].ToString()), Convert.ToInt32(programas.Rows[i]["PROGRA2023"].ToString()));
                            if (estado == 0)
                            {
                                mensaje = dao.mensaje;
                                gvProgramas.JSProperties["cpError"] = "Ocurrio un error " + mensaje;
                                e.Cancel = true;
                                gvProgramas.CancelEdit();
                                cargaprogramas();
                                break;
                            }

                        }


                        if (programas.Rows[i]["PROGRA2024"] == DBNull.Value)
                        {
                            estado = insertaPresupuesto(Convert.ToDouble(e.NewValues["PRESUPUESTO2024"]), Convert.ToDouble(programas.Rows[i]["NOPROGRAMA"].ToString()), Convert.ToInt32(programas.Rows[i]["POA2024"].ToString()), 2024, Convert.ToInt32(programas.Rows[i]["SPPRO$ID_POM"].ToString()), Convert.ToInt32(programas.Rows[i]["SPPRO$ID_INSTITUCION"].ToString()));
                            if (estado == 0)
                            {
                                mensaje = dao.mensaje;
                                gvProgramas.JSProperties["cpError"] = "Ocurrio un error " + mensaje;
                                e.Cancel = true;
                                gvProgramas.CancelEdit();
                                cargaprogramas();
                                break;
                            }

                        }
                        else
                        {
                            estado = ActualizaPresupuesto(Convert.ToDouble(e.NewValues["PRESUPUESTO2024"]), Convert.ToDouble(programas.Rows[i]["NOPROGRAMA"].ToString()), Convert.ToInt32(programas.Rows[i]["POA2024"].ToString()), 2024, Convert.ToInt32(programas.Rows[i]["SPPRO$ID_POM"].ToString()), Convert.ToInt32(programas.Rows[i]["SPPRO$ID_INSTITUCION"].ToString()), Convert.ToInt32(programas.Rows[i]["PROGRA2024"].ToString()));
                            if (estado == 0)
                            {
                                mensaje = dao.mensaje;
                                gvProgramas.JSProperties["cpError"] = "Ocurrio un error " + mensaje;
                                e.Cancel = true;
                                gvProgramas.CancelEdit();
                                cargaprogramas();
                                break;
                            }

                        }


                        if (programas.Rows[i]["PROGRA2025"] == DBNull.Value)
                        {
                            estado = insertaPresupuesto(Convert.ToDouble(e.NewValues["PRESUPUESTO2025"]), Convert.ToDouble(programas.Rows[i]["NOPROGRAMA"].ToString()), Convert.ToInt32(programas.Rows[i]["POA2025"].ToString()), 2025, Convert.ToInt32(programas.Rows[i]["SPPRO$ID_POM"].ToString()), Convert.ToInt32(programas.Rows[i]["SPPRO$ID_INSTITUCION"].ToString()));
                            if (estado == 0)
                            {
                                mensaje = dao.mensaje;
                                gvProgramas.JSProperties["cpError"] = "Ocurrio un error " + mensaje;
                                e.Cancel = true;
                                gvProgramas.CancelEdit();
                                cargaprogramas();
                                break;
                            }

                        }
                        else
                        {
                            estado = ActualizaPresupuesto(Convert.ToDouble(e.NewValues["PRESUPUESTO2025"]), Convert.ToDouble(programas.Rows[i]["NOPROGRAMA"].ToString()), Convert.ToInt32(programas.Rows[i]["POA2025"].ToString()), 2025, Convert.ToInt32(programas.Rows[i]["SPPRO$ID_POM"].ToString()), Convert.ToInt32(programas.Rows[i]["SPPRO$ID_INSTITUCION"].ToString()), Convert.ToInt32(programas.Rows[i]["PROGRA2025"].ToString()));
                            if (estado == 0)
                            {
                                mensaje = dao.mensaje;
                                gvProgramas.JSProperties["cpError"] = "Ocurrio un error " + mensaje;
                                e.Cancel = true;
                                gvProgramas.CancelEdit();
                                cargaprogramas();
                                break;
                            }

                        }


                        if (programas.Rows[i]["PROGRA2026"] == DBNull.Value)
                        {
                            estado = insertaPresupuesto(Convert.ToDouble(e.NewValues["PRESUPUESTO2026"]), Convert.ToDouble(programas.Rows[i]["NOPROGRAMA"].ToString()), Convert.ToInt32(programas.Rows[i]["POA2026"].ToString()), 2026, Convert.ToInt32(programas.Rows[i]["SPPRO$ID_POM"].ToString()), Convert.ToInt32(programas.Rows[i]["SPPRO$ID_INSTITUCION"].ToString()));
                            if (estado == 0)
                            {
                                mensaje = dao.mensaje;
                                gvProgramas.JSProperties["cpError"] = "Ocurrio un error " + mensaje;
                                e.Cancel = true;
                                gvProgramas.CancelEdit();
                                cargaprogramas();
                                break;
                            }

                        }
                        else
                        {
                            estado = ActualizaPresupuesto(Convert.ToDouble(e.NewValues["PRESUPUESTO2026"]), Convert.ToDouble(programas.Rows[i]["NOPROGRAMA"].ToString()), Convert.ToInt32(programas.Rows[i]["POA2026"].ToString()), 2026, Convert.ToInt32(programas.Rows[i]["SPPRO$ID_POM"].ToString()), Convert.ToInt32(programas.Rows[i]["SPPRO$ID_INSTITUCION"].ToString()), Convert.ToInt32(programas.Rows[i]["PROGRA2026"].ToString()));
                            if (estado == 0)
                            {
                                mensaje = dao.mensaje;
                                gvProgramas.JSProperties["cpError"] = "Ocurrio un error " + mensaje;
                                e.Cancel = true;
                                gvProgramas.CancelEdit();
                                cargaprogramas();
                                break;
                            }

                        }


                        if (estado == 1)
                        {
                            mensaje = "Presupuestos registrados correctamente";
                            gvProgramas.JSProperties["cpError"] = "Información: " + mensaje;
                            e.Cancel = true;
                            gvProgramas.CancelEdit();
                            cargaprogramas();
                            break;
                        }

                    }

                }



                else if (Convert.ToInt32(Session["periodo"]) == 22)
                {
                    if (programas.Rows[i]["NOPROGRAMA"].ToString() == e.Keys["NOPROGRAMA"].ToString()) //inicio
                    {

                        if (programas.Rows[i]["PROGRA2023"] == DBNull.Value)
                        {
                            estado = insertaPresupuesto(Convert.ToDouble(e.NewValues["PRESUPUESTO2023"]), Convert.ToDouble(programas.Rows[i]["NOPROGRAMA"].ToString()), Convert.ToInt32(programas.Rows[i]["POA2023"].ToString()), 2023, Convert.ToInt32(programas.Rows[i]["SPPRO$ID_POM"].ToString()), Convert.ToInt32(programas.Rows[i]["SPPRO$ID_INSTITUCION"].ToString()));
                            if (estado == 0)
                            {
                                mensaje = dao.mensaje;
                                gvProgramas.JSProperties["cpError"] = "Ocurrio un error " + mensaje;
                                e.Cancel = true;
                                gvProgramas.CancelEdit();
                                cargaprogramas();
                                break;
                            }

                        }
                        else
                        {
                            estado = ActualizaPresupuesto(Convert.ToDouble(e.NewValues["PRESUPUESTO2023"]), Convert.ToDouble(programas.Rows[i]["NOPROGRAMA"].ToString()), Convert.ToInt32(programas.Rows[i]["POA2023"].ToString()), 2023, Convert.ToInt32(programas.Rows[i]["SPPRO$ID_POM"].ToString()), Convert.ToInt32(programas.Rows[i]["SPPRO$ID_INSTITUCION"].ToString()), Convert.ToInt32(programas.Rows[i]["PROGRA2023"].ToString()));
                            if (estado == 0)
                            {
                                mensaje = dao.mensaje;
                                gvProgramas.JSProperties["cpError"] = "Ocurrio un error " + mensaje;
                                e.Cancel = true;
                                gvProgramas.CancelEdit();
                                cargaprogramas();
                                break;
                            }

                        }


                        if (programas.Rows[i]["PROGRA2024"] == DBNull.Value)
                        {
                            estado = insertaPresupuesto(Convert.ToDouble(e.NewValues["PRESUPUESTO2024"]), Convert.ToDouble(programas.Rows[i]["NOPROGRAMA"].ToString()), Convert.ToInt32(programas.Rows[i]["POA2024"].ToString()), 2024, Convert.ToInt32(programas.Rows[i]["SPPRO$ID_POM"].ToString()), Convert.ToInt32(programas.Rows[i]["SPPRO$ID_INSTITUCION"].ToString()));
                            if (estado == 0)
                            {
                                mensaje = dao.mensaje;
                                gvProgramas.JSProperties["cpError"] = "Ocurrio un error " + mensaje;
                                e.Cancel = true;
                                gvProgramas.CancelEdit();
                                cargaprogramas();
                                break;
                            }

                        }
                        else
                        {
                            estado = ActualizaPresupuesto(Convert.ToDouble(e.NewValues["PRESUPUESTO2024"]), Convert.ToDouble(programas.Rows[i]["NOPROGRAMA"].ToString()), Convert.ToInt32(programas.Rows[i]["POA2024"].ToString()), 2024, Convert.ToInt32(programas.Rows[i]["SPPRO$ID_POM"].ToString()), Convert.ToInt32(programas.Rows[i]["SPPRO$ID_INSTITUCION"].ToString()), Convert.ToInt32(programas.Rows[i]["PROGRA2024"].ToString()));
                            if (estado == 0)
                            {
                                mensaje = dao.mensaje;
                                gvProgramas.JSProperties["cpError"] = "Ocurrio un error " + mensaje;
                                e.Cancel = true;
                                gvProgramas.CancelEdit();
                                cargaprogramas();
                                break;
                            }

                        }


                        if (programas.Rows[i]["PROGRA2025"] == DBNull.Value)
                        {
                            estado = insertaPresupuesto(Convert.ToDouble(e.NewValues["PRESUPUESTO2025"]), Convert.ToDouble(programas.Rows[i]["NOPROGRAMA"].ToString()), Convert.ToInt32(programas.Rows[i]["POA2025"].ToString()), 2025, Convert.ToInt32(programas.Rows[i]["SPPRO$ID_POM"].ToString()), Convert.ToInt32(programas.Rows[i]["SPPRO$ID_INSTITUCION"].ToString()));
                            if (estado == 0)
                            {
                                mensaje = dao.mensaje;
                                gvProgramas.JSProperties["cpError"] = "Ocurrio un error " + mensaje;
                                e.Cancel = true;
                                gvProgramas.CancelEdit();
                                cargaprogramas();
                                break;
                            }

                        }
                        else
                        {
                            estado = ActualizaPresupuesto(Convert.ToDouble(e.NewValues["PRESUPUESTO2025"]), Convert.ToDouble(programas.Rows[i]["NOPROGRAMA"].ToString()), Convert.ToInt32(programas.Rows[i]["POA2025"].ToString()), 2025, Convert.ToInt32(programas.Rows[i]["SPPRO$ID_POM"].ToString()), Convert.ToInt32(programas.Rows[i]["SPPRO$ID_INSTITUCION"].ToString()), Convert.ToInt32(programas.Rows[i]["PROGRA2025"].ToString()));
                            if (estado == 0)
                            {
                                mensaje = dao.mensaje;
                                gvProgramas.JSProperties["cpError"] = "Ocurrio un error " + mensaje;
                                e.Cancel = true;
                                gvProgramas.CancelEdit();
                                cargaprogramas();
                                break;
                            }

                        }


                        if (programas.Rows[i]["PROGRA2026"] == DBNull.Value)
                        {
                            estado = insertaPresupuesto(Convert.ToDouble(e.NewValues["PRESUPUESTO2026"]), Convert.ToDouble(programas.Rows[i]["NOPROGRAMA"].ToString()), Convert.ToInt32(programas.Rows[i]["POA2026"].ToString()), 2026, Convert.ToInt32(programas.Rows[i]["SPPRO$ID_POM"].ToString()), Convert.ToInt32(programas.Rows[i]["SPPRO$ID_INSTITUCION"].ToString()));
                            if (estado == 0)
                            {
                                mensaje = dao.mensaje;
                                gvProgramas.JSProperties["cpError"] = "Ocurrio un error " + mensaje;
                                e.Cancel = true;
                                gvProgramas.CancelEdit();
                                cargaprogramas();
                                break;
                            }

                        }
                        else
                        {
                            estado = ActualizaPresupuesto(Convert.ToDouble(e.NewValues["PRESUPUESTO2026"]), Convert.ToDouble(programas.Rows[i]["NOPROGRAMA"].ToString()), Convert.ToInt32(programas.Rows[i]["POA2026"].ToString()), 2026, Convert.ToInt32(programas.Rows[i]["SPPRO$ID_POM"].ToString()), Convert.ToInt32(programas.Rows[i]["SPPRO$ID_INSTITUCION"].ToString()), Convert.ToInt32(programas.Rows[i]["PROGRA2026"].ToString()));
                            if (estado == 0)
                            {
                                mensaje = dao.mensaje;
                                gvProgramas.JSProperties["cpError"] = "Ocurrio un error " + mensaje;
                                e.Cancel = true;
                                gvProgramas.CancelEdit();
                                cargaprogramas();
                                break;
                            }

                        }


                        if (programas.Rows[i]["PROGRA2027"] == DBNull.Value)
                        {
                            estado = insertaPresupuesto(Convert.ToDouble(e.NewValues["PRESUPUESTO2027"]), Convert.ToDouble(programas.Rows[i]["NOPROGRAMA"].ToString()), Convert.ToInt32(programas.Rows[i]["POA2027"].ToString()), 2027, Convert.ToInt32(programas.Rows[i]["SPPRO$ID_POM"].ToString()), Convert.ToInt32(programas.Rows[i]["SPPRO$ID_INSTITUCION"].ToString()));
                            if (estado == 0)
                            {
                                mensaje = dao.mensaje;
                                gvProgramas.JSProperties["cpError"] = "Ocurrio un error " + mensaje;
                                e.Cancel = true;
                                gvProgramas.CancelEdit();
                                cargaprogramas();
                                break;
                            }

                        }
                        else
                        {
                            estado = ActualizaPresupuesto(Convert.ToDouble(e.NewValues["PRESUPUESTO2027"]), Convert.ToDouble(programas.Rows[i]["NOPROGRAMA"].ToString()), Convert.ToInt32(programas.Rows[i]["POA2027"].ToString()), 2027, Convert.ToInt32(programas.Rows[i]["SPPRO$ID_POM"].ToString()), Convert.ToInt32(programas.Rows[i]["SPPRO$ID_INSTITUCION"].ToString()), Convert.ToInt32(programas.Rows[i]["PROGRA2027"].ToString()));
                            if (estado == 0)
                            {
                                mensaje = dao.mensaje;
                                gvProgramas.JSProperties["cpError"] = "Ocurrio un error " + mensaje;
                                e.Cancel = true;
                                gvProgramas.CancelEdit();
                                cargaprogramas();
                                break;
                            }

                        }


                        if (estado == 1)
                        {
                            mensaje = "Presupuestos registrados correctamente";
                            gvProgramas.JSProperties["cpError"] = "Información: " + mensaje;
                            e.Cancel = true;
                            gvProgramas.CancelEdit();
                            cargaprogramas();
                            break;
                        }

                    }

                }






                else if (Convert.ToInt32(Session["periodo"]) == 23)
                {
                    if (programas.Rows[i]["NOPROGRAMA"].ToString() == e.Keys["NOPROGRAMA"].ToString()) //inicio
                    {


                        if (programas.Rows[i]["PROGRA2024"] == DBNull.Value)
                        {
                            estado = insertaPresupuesto(Convert.ToDouble(e.NewValues["PRESUPUESTO2024"]), Convert.ToDouble(programas.Rows[i]["NOPROGRAMA"].ToString()), Convert.ToInt32(programas.Rows[i]["POA2024"].ToString()), 2024, Convert.ToInt32(programas.Rows[i]["SPPRO$ID_POM"].ToString()), Convert.ToInt32(programas.Rows[i]["SPPRO$ID_INSTITUCION"].ToString()));
                            if (estado == 0)
                            {
                                mensaje = dao.mensaje;
                                gvProgramas.JSProperties["cpError"] = "Ocurrio un error " + mensaje;
                                e.Cancel = true;
                                gvProgramas.CancelEdit();
                                cargaprogramas();
                                break;
                            }

                        }
                        else
                        {
                            estado = ActualizaPresupuesto(Convert.ToDouble(e.NewValues["PRESUPUESTO2024"]), Convert.ToDouble(programas.Rows[i]["NOPROGRAMA"].ToString()), Convert.ToInt32(programas.Rows[i]["POA2024"].ToString()), 2024, Convert.ToInt32(programas.Rows[i]["SPPRO$ID_POM"].ToString()), Convert.ToInt32(programas.Rows[i]["SPPRO$ID_INSTITUCION"].ToString()), Convert.ToInt32(programas.Rows[i]["PROGRA2024"].ToString()));
                            if (estado == 0)
                            {
                                mensaje = dao.mensaje;
                                gvProgramas.JSProperties["cpError"] = "Ocurrio un error " + mensaje;
                                e.Cancel = true;
                                gvProgramas.CancelEdit();
                                cargaprogramas();
                                break;
                            }

                        }


                        if (programas.Rows[i]["PROGRA2025"] == DBNull.Value)
                        {
                            estado = insertaPresupuesto(Convert.ToDouble(e.NewValues["PRESUPUESTO2025"]), Convert.ToDouble(programas.Rows[i]["NOPROGRAMA"].ToString()), Convert.ToInt32(programas.Rows[i]["POA2025"].ToString()), 2025, Convert.ToInt32(programas.Rows[i]["SPPRO$ID_POM"].ToString()), Convert.ToInt32(programas.Rows[i]["SPPRO$ID_INSTITUCION"].ToString()));
                            if (estado == 0)
                            {
                                mensaje = dao.mensaje;
                                gvProgramas.JSProperties["cpError"] = "Ocurrio un error " + mensaje;
                                e.Cancel = true;
                                gvProgramas.CancelEdit();
                                cargaprogramas();
                                break;
                            }

                        }
                        else
                        {
                            estado = ActualizaPresupuesto(Convert.ToDouble(e.NewValues["PRESUPUESTO2025"]), Convert.ToDouble(programas.Rows[i]["NOPROGRAMA"].ToString()), Convert.ToInt32(programas.Rows[i]["POA2025"].ToString()), 2025, Convert.ToInt32(programas.Rows[i]["SPPRO$ID_POM"].ToString()), Convert.ToInt32(programas.Rows[i]["SPPRO$ID_INSTITUCION"].ToString()), Convert.ToInt32(programas.Rows[i]["PROGRA2025"].ToString()));
                            if (estado == 0)
                            {
                                mensaje = dao.mensaje;
                                gvProgramas.JSProperties["cpError"] = "Ocurrio un error " + mensaje;
                                e.Cancel = true;
                                gvProgramas.CancelEdit();
                                cargaprogramas();
                                break;
                            }

                        }


                        if (programas.Rows[i]["PROGRA2026"] == DBNull.Value)
                        {
                            estado = insertaPresupuesto(Convert.ToDouble(e.NewValues["PRESUPUESTO2026"]), Convert.ToDouble(programas.Rows[i]["NOPROGRAMA"].ToString()), Convert.ToInt32(programas.Rows[i]["POA2026"].ToString()), 2026, Convert.ToInt32(programas.Rows[i]["SPPRO$ID_POM"].ToString()), Convert.ToInt32(programas.Rows[i]["SPPRO$ID_INSTITUCION"].ToString()));
                            if (estado == 0)
                            {
                                mensaje = dao.mensaje;
                                gvProgramas.JSProperties["cpError"] = "Ocurrio un error " + mensaje;
                                e.Cancel = true;
                                gvProgramas.CancelEdit();
                                cargaprogramas();
                                break;
                            }

                        }
                        else
                        {
                            estado = ActualizaPresupuesto(Convert.ToDouble(e.NewValues["PRESUPUESTO2026"]), Convert.ToDouble(programas.Rows[i]["NOPROGRAMA"].ToString()), Convert.ToInt32(programas.Rows[i]["POA2026"].ToString()), 2026, Convert.ToInt32(programas.Rows[i]["SPPRO$ID_POM"].ToString()), Convert.ToInt32(programas.Rows[i]["SPPRO$ID_INSTITUCION"].ToString()), Convert.ToInt32(programas.Rows[i]["PROGRA2026"].ToString()));
                            if (estado == 0)
                            {
                                mensaje = dao.mensaje;
                                gvProgramas.JSProperties["cpError"] = "Ocurrio un error " + mensaje;
                                e.Cancel = true;
                                gvProgramas.CancelEdit();
                                cargaprogramas();
                                break;
                            }

                        }


                        if (programas.Rows[i]["PROGRA2027"] == DBNull.Value)
                        {
                            estado = insertaPresupuesto(Convert.ToDouble(e.NewValues["PRESUPUESTO2027"]), Convert.ToDouble(programas.Rows[i]["NOPROGRAMA"].ToString()), Convert.ToInt32(programas.Rows[i]["POA2027"].ToString()), 2027, Convert.ToInt32(programas.Rows[i]["SPPRO$ID_POM"].ToString()), Convert.ToInt32(programas.Rows[i]["SPPRO$ID_INSTITUCION"].ToString()));
                            if (estado == 0)
                            {
                                mensaje = dao.mensaje;
                                gvProgramas.JSProperties["cpError"] = "Ocurrio un error " + mensaje;
                                e.Cancel = true;
                                gvProgramas.CancelEdit();
                                cargaprogramas();
                                break;
                            }

                        }
                        else
                        {
                            estado = ActualizaPresupuesto(Convert.ToDouble(e.NewValues["PRESUPUESTO2027"]), Convert.ToDouble(programas.Rows[i]["NOPROGRAMA"].ToString()), Convert.ToInt32(programas.Rows[i]["POA2027"].ToString()), 2027, Convert.ToInt32(programas.Rows[i]["SPPRO$ID_POM"].ToString()), Convert.ToInt32(programas.Rows[i]["SPPRO$ID_INSTITUCION"].ToString()), Convert.ToInt32(programas.Rows[i]["PROGRA2027"].ToString()));
                            if (estado == 0)
                            {
                                mensaje = dao.mensaje;
                                gvProgramas.JSProperties["cpError"] = "Ocurrio un error " + mensaje;
                                e.Cancel = true;
                                gvProgramas.CancelEdit();
                                cargaprogramas();
                                break;
                            }

                        }



                        if (programas.Rows[i]["PROGRA2028"] == DBNull.Value)
                        {
                            estado = insertaPresupuesto(Convert.ToDouble(e.NewValues["PRESUPUESTO2028"]), Convert.ToDouble(programas.Rows[i]["NOPROGRAMA"].ToString()), Convert.ToInt32(programas.Rows[i]["POA2028"].ToString()), 2028, Convert.ToInt32(programas.Rows[i]["SPPRO$ID_POM"].ToString()), Convert.ToInt32(programas.Rows[i]["SPPRO$ID_INSTITUCION"].ToString()));
                            if (estado == 0)
                            {
                                mensaje = dao.mensaje;
                                gvProgramas.JSProperties["cpError"] = "Ocurrio un error " + mensaje;
                                e.Cancel = true;
                                gvProgramas.CancelEdit();
                                cargaprogramas();
                                break;
                            }

                        }
                        else
                        {
                            estado = ActualizaPresupuesto(Convert.ToDouble(e.NewValues["PRESUPUESTO2028"]), Convert.ToDouble(programas.Rows[i]["NOPROGRAMA"].ToString()), Convert.ToInt32(programas.Rows[i]["POA2028"].ToString()), 2028, Convert.ToInt32(programas.Rows[i]["SPPRO$ID_POM"].ToString()), Convert.ToInt32(programas.Rows[i]["SPPRO$ID_INSTITUCION"].ToString()), Convert.ToInt32(programas.Rows[i]["PROGRA2028"].ToString()));
                            if (estado == 0)
                            {
                                mensaje = dao.mensaje;
                                gvProgramas.JSProperties["cpError"] = "Ocurrio un error " + mensaje;
                                e.Cancel = true;
                                gvProgramas.CancelEdit();
                                cargaprogramas();
                                break;
                            }

                        }

                        if (estado == 1)
                        {
                            mensaje = "Presupuestos registrados correctamente";
                            gvProgramas.JSProperties["cpError"] = "Información: " + mensaje;
                            e.Cancel = true;
                            gvProgramas.CancelEdit();
                            cargaprogramas();
                            break;
                        }

                    }

                }

                //PERIODO NUEVA PGG

                else if (Convert.ToInt32(Session["periodo"]) == 24)
                {
                    if (programas.Rows[i]["NOPROGRAMA"].ToString() == e.Keys["NOPROGRAMA"].ToString()) //inicio
                    {


                        if (programas.Rows[i]["PROGRA2025"] == DBNull.Value)
                        {
                            estado = insertaPresupuesto(Convert.ToDouble(e.NewValues["PRESUPUESTO2025"]), Convert.ToDouble(programas.Rows[i]["NOPROGRAMA"].ToString()), Convert.ToInt32(programas.Rows[i]["POA2025"].ToString()), 2025, Convert.ToInt32(programas.Rows[i]["SPPRO$ID_POM"].ToString()), Convert.ToInt32(programas.Rows[i]["SPPRO$ID_INSTITUCION"].ToString()));
                            if (estado == 0)
                            {
                                mensaje = dao.mensaje;
                                gvProgramas.JSProperties["cpError"] = "Ocurrio un error " + mensaje;
                                e.Cancel = true;
                                gvProgramas.CancelEdit();
                                cargaprogramas();
                                break;
                            }

                        }
                        else
                        {
                            estado = ActualizaPresupuesto(Convert.ToDouble(e.NewValues["PRESUPUESTO2025"]), Convert.ToDouble(programas.Rows[i]["NOPROGRAMA"].ToString()), Convert.ToInt32(programas.Rows[i]["POA2025"].ToString()), 2025, Convert.ToInt32(programas.Rows[i]["SPPRO$ID_POM"].ToString()), Convert.ToInt32(programas.Rows[i]["SPPRO$ID_INSTITUCION"].ToString()), Convert.ToInt32(programas.Rows[i]["PROGRA2025"].ToString()));
                            if (estado == 0)
                            {
                                mensaje = dao.mensaje;
                                gvProgramas.JSProperties["cpError"] = "Ocurrio un error " + mensaje;
                                e.Cancel = true;
                                gvProgramas.CancelEdit();
                                cargaprogramas();
                                break;
                            }

                        }


                        if (programas.Rows[i]["PROGRA2026"] == DBNull.Value)
                        {
                            estado = insertaPresupuesto(Convert.ToDouble(e.NewValues["PRESUPUESTO2026"]), Convert.ToDouble(programas.Rows[i]["NOPROGRAMA"].ToString()), Convert.ToInt32(programas.Rows[i]["POA2026"].ToString()), 2026, Convert.ToInt32(programas.Rows[i]["SPPRO$ID_POM"].ToString()), Convert.ToInt32(programas.Rows[i]["SPPRO$ID_INSTITUCION"].ToString()));
                            if (estado == 0)
                            {
                                mensaje = dao.mensaje;
                                gvProgramas.JSProperties["cpError"] = "Ocurrio un error " + mensaje;
                                e.Cancel = true;
                                gvProgramas.CancelEdit();
                                cargaprogramas();
                                break;
                            }

                        }
                        else
                        {
                            estado = ActualizaPresupuesto(Convert.ToDouble(e.NewValues["PRESUPUESTO2026"]), Convert.ToDouble(programas.Rows[i]["NOPROGRAMA"].ToString()), Convert.ToInt32(programas.Rows[i]["POA2026"].ToString()), 2026, Convert.ToInt32(programas.Rows[i]["SPPRO$ID_POM"].ToString()), Convert.ToInt32(programas.Rows[i]["SPPRO$ID_INSTITUCION"].ToString()), Convert.ToInt32(programas.Rows[i]["PROGRA2026"].ToString()));
                            if (estado == 0)
                            {
                                mensaje = dao.mensaje;
                                gvProgramas.JSProperties["cpError"] = "Ocurrio un error " + mensaje;
                                e.Cancel = true;
                                gvProgramas.CancelEdit();
                                cargaprogramas();
                                break;
                            }

                        }


                        if (programas.Rows[i]["PROGRA2027"] == DBNull.Value)
                        {
                            estado = insertaPresupuesto(Convert.ToDouble(e.NewValues["PRESUPUESTO2027"]), Convert.ToDouble(programas.Rows[i]["NOPROGRAMA"].ToString()), Convert.ToInt32(programas.Rows[i]["POA2027"].ToString()), 2027, Convert.ToInt32(programas.Rows[i]["SPPRO$ID_POM"].ToString()), Convert.ToInt32(programas.Rows[i]["SPPRO$ID_INSTITUCION"].ToString()));
                            if (estado == 0)
                            {
                                mensaje = dao.mensaje;
                                gvProgramas.JSProperties["cpError"] = "Ocurrio un error " + mensaje;
                                e.Cancel = true;
                                gvProgramas.CancelEdit();
                                cargaprogramas();
                                break;
                            }

                        }
                        else
                        {
                            estado = ActualizaPresupuesto(Convert.ToDouble(e.NewValues["PRESUPUESTO2027"]), Convert.ToDouble(programas.Rows[i]["NOPROGRAMA"].ToString()), Convert.ToInt32(programas.Rows[i]["POA2027"].ToString()), 2027, Convert.ToInt32(programas.Rows[i]["SPPRO$ID_POM"].ToString()), Convert.ToInt32(programas.Rows[i]["SPPRO$ID_INSTITUCION"].ToString()), Convert.ToInt32(programas.Rows[i]["PROGRA2027"].ToString()));
                            if (estado == 0)
                            {
                                mensaje = dao.mensaje;
                                gvProgramas.JSProperties["cpError"] = "Ocurrio un error " + mensaje;
                                e.Cancel = true;
                                gvProgramas.CancelEdit();
                                cargaprogramas();
                                break;
                            }

                        }


                        if (programas.Rows[i]["PROGRA2028"] == DBNull.Value)
                        {
                            estado = insertaPresupuesto(Convert.ToDouble(e.NewValues["PRESUPUESTO2028"]), Convert.ToDouble(programas.Rows[i]["NOPROGRAMA"].ToString()), Convert.ToInt32(programas.Rows[i]["POA2028"].ToString()), 2028, Convert.ToInt32(programas.Rows[i]["SPPRO$ID_POM"].ToString()), Convert.ToInt32(programas.Rows[i]["SPPRO$ID_INSTITUCION"].ToString()));
                            if (estado == 0)
                            {
                                mensaje = dao.mensaje;
                                gvProgramas.JSProperties["cpError"] = "Ocurrio un error " + mensaje;
                                e.Cancel = true;
                                gvProgramas.CancelEdit();
                                cargaprogramas();
                                break;
                            }

                        }
                        else
                        {
                            estado = ActualizaPresupuesto(Convert.ToDouble(e.NewValues["PRESUPUESTO2028"]), Convert.ToDouble(programas.Rows[i]["NOPROGRAMA"].ToString()), Convert.ToInt32(programas.Rows[i]["POA2028"].ToString()), 2028, Convert.ToInt32(programas.Rows[i]["SPPRO$ID_POM"].ToString()), Convert.ToInt32(programas.Rows[i]["SPPRO$ID_INSTITUCION"].ToString()), Convert.ToInt32(programas.Rows[i]["PROGRA2028"].ToString()));
                            if (estado == 0)
                            {
                                mensaje = dao.mensaje;
                                gvProgramas.JSProperties["cpError"] = "Ocurrio un error " + mensaje;
                                e.Cancel = true;
                                gvProgramas.CancelEdit();
                                cargaprogramas();
                                break;
                            }

                        }



                        if (programas.Rows[i]["PROGRA2029"] == DBNull.Value)
                        {
                            estado = insertaPresupuesto(Convert.ToDouble(e.NewValues["PRESUPUESTO2029"]), Convert.ToDouble(programas.Rows[i]["NOPROGRAMA"].ToString()), Convert.ToInt32(programas.Rows[i]["POA2029"].ToString()), 2029, Convert.ToInt32(programas.Rows[i]["SPPRO$ID_POM"].ToString()), Convert.ToInt32(programas.Rows[i]["SPPRO$ID_INSTITUCION"].ToString()));
                            if (estado == 0)
                            {
                                mensaje = dao.mensaje;
                                gvProgramas.JSProperties["cpError"] = "Ocurrio un error " + mensaje;
                                e.Cancel = true;
                                gvProgramas.CancelEdit();
                                cargaprogramas();
                                break;
                            }

                        }
                        else
                        {
                            estado = ActualizaPresupuesto(Convert.ToDouble(e.NewValues["PRESUPUESTO2029"]), Convert.ToDouble(programas.Rows[i]["NOPROGRAMA"].ToString()), Convert.ToInt32(programas.Rows[i]["POA2029"].ToString()), 2029, Convert.ToInt32(programas.Rows[i]["SPPRO$ID_POM"].ToString()), Convert.ToInt32(programas.Rows[i]["SPPRO$ID_INSTITUCION"].ToString()), Convert.ToInt32(programas.Rows[i]["PROGRA2029"].ToString()));
                            if (estado == 0)
                            {
                                mensaje = dao.mensaje;
                                gvProgramas.JSProperties["cpError"] = "Ocurrio un error " + mensaje;
                                e.Cancel = true;
                                gvProgramas.CancelEdit();
                                cargaprogramas();
                                break;
                            }

                        }

                        if (estado == 1)
                        {
                            mensaje = "Presupuestos registrados correctamente";
                            gvProgramas.JSProperties["cpError"] = "Información: " + mensaje;
                            e.Cancel = true;
                            gvProgramas.CancelEdit();
                            cargaprogramas();
                            break;
                        }

                    }

                }

                //PERIODO NUEVA PGG

                //INICIO PERIODO 2026-2030

                else if (Convert.ToInt32(Session["periodo"]) == 25)
                {
                    if (programas.Rows[i]["NOPROGRAMA"].ToString() == e.Keys["NOPROGRAMA"].ToString()) //inicio
                    {


                        if (programas.Rows[i]["PROGRA2026"] == DBNull.Value)
                        {
                            estado = insertaPresupuesto(Convert.ToDouble(e.NewValues["PRESUPUESTO2026"]), Convert.ToDouble(programas.Rows[i]["NOPROGRAMA"].ToString()), Convert.ToInt32(programas.Rows[i]["POA2026"].ToString()), 2026, Convert.ToInt32(programas.Rows[i]["SPPRO$ID_POM"].ToString()), Convert.ToInt32(programas.Rows[i]["SPPRO$ID_INSTITUCION"].ToString()));
                            if (estado == 0)
                            {
                                mensaje = dao.mensaje;
                                gvProgramas.JSProperties["cpError"] = "Ocurrio un error " + mensaje;
                                e.Cancel = true;
                                gvProgramas.CancelEdit();
                                cargaprogramas();
                                break;
                            }

                        }
                        else
                        {
                            estado = ActualizaPresupuesto(Convert.ToDouble(e.NewValues["PRESUPUESTO2026"]), Convert.ToDouble(programas.Rows[i]["NOPROGRAMA"].ToString()), Convert.ToInt32(programas.Rows[i]["POA2026"].ToString()), 2026, Convert.ToInt32(programas.Rows[i]["SPPRO$ID_POM"].ToString()), Convert.ToInt32(programas.Rows[i]["SPPRO$ID_INSTITUCION"].ToString()), Convert.ToInt32(programas.Rows[i]["PROGRA2026"].ToString()));
                            if (estado == 0)
                            {
                                mensaje = dao.mensaje;
                                gvProgramas.JSProperties["cpError"] = "Ocurrio un error " + mensaje;
                                e.Cancel = true;
                                gvProgramas.CancelEdit();
                                cargaprogramas();
                                break;
                            }

                        }


                        if (programas.Rows[i]["PROGRA2027"] == DBNull.Value)
                        {
                            estado = insertaPresupuesto(Convert.ToDouble(e.NewValues["PRESUPUESTO2027"]), Convert.ToDouble(programas.Rows[i]["NOPROGRAMA"].ToString()), Convert.ToInt32(programas.Rows[i]["POA2027"].ToString()), 2027, Convert.ToInt32(programas.Rows[i]["SPPRO$ID_POM"].ToString()), Convert.ToInt32(programas.Rows[i]["SPPRO$ID_INSTITUCION"].ToString()));
                            if (estado == 0)
                            {
                                mensaje = dao.mensaje;
                                gvProgramas.JSProperties["cpError"] = "Ocurrio un error " + mensaje;
                                e.Cancel = true;
                                gvProgramas.CancelEdit();
                                cargaprogramas();
                                break;
                            }

                        }
                        else
                        {
                            estado = ActualizaPresupuesto(Convert.ToDouble(e.NewValues["PRESUPUESTO2027"]), Convert.ToDouble(programas.Rows[i]["NOPROGRAMA"].ToString()), Convert.ToInt32(programas.Rows[i]["POA2027"].ToString()), 2027, Convert.ToInt32(programas.Rows[i]["SPPRO$ID_POM"].ToString()), Convert.ToInt32(programas.Rows[i]["SPPRO$ID_INSTITUCION"].ToString()), Convert.ToInt32(programas.Rows[i]["PROGRA2027"].ToString()));
                            if (estado == 0)
                            {
                                mensaje = dao.mensaje;
                                gvProgramas.JSProperties["cpError"] = "Ocurrio un error " + mensaje;
                                e.Cancel = true;
                                gvProgramas.CancelEdit();
                                cargaprogramas();
                                break;
                            }

                        }


                        if (programas.Rows[i]["PROGRA2028"] == DBNull.Value)
                        {
                            estado = insertaPresupuesto(Convert.ToDouble(e.NewValues["PRESUPUESTO2028"]), Convert.ToDouble(programas.Rows[i]["NOPROGRAMA"].ToString()), Convert.ToInt32(programas.Rows[i]["POA2028"].ToString()), 2028, Convert.ToInt32(programas.Rows[i]["SPPRO$ID_POM"].ToString()), Convert.ToInt32(programas.Rows[i]["SPPRO$ID_INSTITUCION"].ToString()));
                            if (estado == 0)
                            {
                                mensaje = dao.mensaje;
                                gvProgramas.JSProperties["cpError"] = "Ocurrio un error " + mensaje;
                                e.Cancel = true;
                                gvProgramas.CancelEdit();
                                cargaprogramas();
                                break;
                            }

                        }
                        else
                        {
                            estado = ActualizaPresupuesto(Convert.ToDouble(e.NewValues["PRESUPUESTO2028"]), Convert.ToDouble(programas.Rows[i]["NOPROGRAMA"].ToString()), Convert.ToInt32(programas.Rows[i]["POA2028"].ToString()), 2028, Convert.ToInt32(programas.Rows[i]["SPPRO$ID_POM"].ToString()), Convert.ToInt32(programas.Rows[i]["SPPRO$ID_INSTITUCION"].ToString()), Convert.ToInt32(programas.Rows[i]["PROGRA2028"].ToString()));
                            if (estado == 0)
                            {
                                mensaje = dao.mensaje;
                                gvProgramas.JSProperties["cpError"] = "Ocurrio un error " + mensaje;
                                e.Cancel = true;
                                gvProgramas.CancelEdit();
                                cargaprogramas();
                                break;
                            }

                        }


                        if (programas.Rows[i]["PROGRA2029"] == DBNull.Value)
                        {
                            estado = insertaPresupuesto(Convert.ToDouble(e.NewValues["PRESUPUESTO2029"]), Convert.ToDouble(programas.Rows[i]["NOPROGRAMA"].ToString()), Convert.ToInt32(programas.Rows[i]["POA2029"].ToString()), 2029, Convert.ToInt32(programas.Rows[i]["SPPRO$ID_POM"].ToString()), Convert.ToInt32(programas.Rows[i]["SPPRO$ID_INSTITUCION"].ToString()));
                            if (estado == 0)
                            {
                                mensaje = dao.mensaje;
                                gvProgramas.JSProperties["cpError"] = "Ocurrio un error " + mensaje;
                                e.Cancel = true;
                                gvProgramas.CancelEdit();
                                cargaprogramas();
                                break;
                            }

                        }
                        else
                        {
                            estado = ActualizaPresupuesto(Convert.ToDouble(e.NewValues["PRESUPUESTO2029"]), Convert.ToDouble(programas.Rows[i]["NOPROGRAMA"].ToString()), Convert.ToInt32(programas.Rows[i]["POA2029"].ToString()), 2029, Convert.ToInt32(programas.Rows[i]["SPPRO$ID_POM"].ToString()), Convert.ToInt32(programas.Rows[i]["SPPRO$ID_INSTITUCION"].ToString()), Convert.ToInt32(programas.Rows[i]["PROGRA2029"].ToString()));
                            if (estado == 0)
                            {
                                mensaje = dao.mensaje;
                                gvProgramas.JSProperties["cpError"] = "Ocurrio un error " + mensaje;
                                e.Cancel = true;
                                gvProgramas.CancelEdit();
                                cargaprogramas();
                                break;
                            }

                        }



                        if (programas.Rows[i]["PROGRA2030"] == DBNull.Value)
                        {
                            estado = insertaPresupuesto(Convert.ToDouble(e.NewValues["PRESUPUESTO2030"]), Convert.ToDouble(programas.Rows[i]["NOPROGRAMA"].ToString()), Convert.ToInt32(programas.Rows[i]["POA2030"].ToString()), 2030, Convert.ToInt32(programas.Rows[i]["SPPRO$ID_POM"].ToString()), Convert.ToInt32(programas.Rows[i]["SPPRO$ID_INSTITUCION"].ToString()));
                            if (estado == 0)
                            {
                                mensaje = dao.mensaje;
                                gvProgramas.JSProperties["cpError"] = "Ocurrio un error " + mensaje;
                                e.Cancel = true;
                                gvProgramas.CancelEdit();
                                cargaprogramas();
                                break;
                            }

                        }
                        else
                        {
                            estado = ActualizaPresupuesto(Convert.ToDouble(e.NewValues["PRESUPUESTO2030"]), Convert.ToDouble(programas.Rows[i]["NOPROGRAMA"].ToString()), Convert.ToInt32(programas.Rows[i]["POA2030"].ToString()), 2030, Convert.ToInt32(programas.Rows[i]["SPPRO$ID_POM"].ToString()), Convert.ToInt32(programas.Rows[i]["SPPRO$ID_INSTITUCION"].ToString()), Convert.ToInt32(programas.Rows[i]["PROGRA2030"].ToString()));
                            if (estado == 0)
                            {
                                mensaje = dao.mensaje;
                                gvProgramas.JSProperties["cpError"] = "Ocurrio un error " + mensaje;
                                e.Cancel = true;
                                gvProgramas.CancelEdit();
                                cargaprogramas();
                                break;
                            }

                        }

                        if (estado == 1)
                        {
                            mensaje = "Presupuestos registrados correctamente";
                            gvProgramas.JSProperties["cpError"] = "Información: " + mensaje;
                            e.Cancel = true;
                            gvProgramas.CancelEdit();
                            cargaprogramas();
                            break;
                        }

                    }

                }


                //FIN PERIODO 2026-2030

                else
                {
                    if (programas.Rows[i]["NOPROGRAMA"].ToString() == e.Keys["NOPROGRAMA"].ToString()) //inicio
                    {
                        //----------------------inicia 2020

                        if (programas.Rows[i]["PROGRA2020"] == DBNull.Value)
                        {
                            estado = insertaPresupuesto(Convert.ToDouble(e.NewValues["PRESUPUESTO2020"]), Convert.ToDouble(programas.Rows[i]["NOPROGRAMA"].ToString()), Convert.ToInt32(programas.Rows[i]["POA2020"].ToString()), 2020, Convert.ToInt32(programas.Rows[i]["SPPRO$ID_POM"].ToString()), Convert.ToInt32(programas.Rows[i]["SPPRO$ID_INSTITUCION"].ToString()));
                            if (estado == 0)
                            {
                                mensaje = dao.mensaje;
                                gvProgramas.JSProperties["cpError"] = "Ocurrio un error " + mensaje;
                                e.Cancel = true;
                                gvProgramas.CancelEdit();
                                cargaprogramas();
                                break;
                            }
                        }
                        else
                        {
                            estado = ActualizaPresupuesto(Convert.ToDouble(e.NewValues["PRESUPUESTO2020"]), Convert.ToDouble(programas.Rows[i]["NOPROGRAMA"].ToString()), Convert.ToInt32(programas.Rows[i]["POA2020"].ToString()), 2020, Convert.ToInt32(programas.Rows[i]["SPPRO$ID_POM"].ToString()), Convert.ToInt32(programas.Rows[i]["SPPRO$ID_INSTITUCION"].ToString()), Convert.ToInt32(programas.Rows[i]["PROGRA2020"].ToString()));
                            if (estado == 0)
                            {
                                mensaje = dao.mensaje;
                                gvProgramas.JSProperties["cpError"] = "Ocurrio un error " + mensaje;
                                e.Cancel = true;
                                gvProgramas.CancelEdit();
                                cargaprogramas();
                                break;
                            }
                        }
                        //----------------------fin 2020




                        //----------------------inicia 2021

                        if (programas.Rows[i]["PROGRA2021"] == DBNull.Value)
                        {
                            estado = insertaPresupuesto(Convert.ToDouble(e.NewValues["PRESUPUESTO2021"]), Convert.ToDouble(programas.Rows[i]["NOPROGRAMA"].ToString()), Convert.ToInt32(programas.Rows[i]["POA2021"].ToString()), 2021, Convert.ToInt32(programas.Rows[i]["SPPRO$ID_POM"].ToString()), Convert.ToInt32(programas.Rows[i]["SPPRO$ID_INSTITUCION"].ToString()));
                            if (estado == 0)
                            {
                                mensaje = dao.mensaje;
                                gvProgramas.JSProperties["cpError"] = "Ocurrio un error " + mensaje;
                                e.Cancel = true;
                                gvProgramas.CancelEdit();
                                cargaprogramas();
                                break;
                            }
                        }
                        else
                        {
                            estado = ActualizaPresupuesto(Convert.ToDouble(e.NewValues["PRESUPUESTO2021"]), Convert.ToDouble(programas.Rows[i]["NOPROGRAMA"].ToString()), Convert.ToInt32(programas.Rows[i]["POA2021"].ToString()), 2021, Convert.ToInt32(programas.Rows[i]["SPPRO$ID_POM"].ToString()), Convert.ToInt32(programas.Rows[i]["SPPRO$ID_INSTITUCION"].ToString()), Convert.ToInt32(programas.Rows[i]["PROGRA2021"].ToString()));
                            if (estado == 0)
                            {
                                mensaje = dao.mensaje;
                                gvProgramas.JSProperties["cpError"] = "Ocurrio un error " + mensaje;
                                e.Cancel = true;
                                gvProgramas.CancelEdit();
                                cargaprogramas();
                                break;
                            }
                        }
                        //----------------------fin 2021


                        //----------------------inicia 2022

                        if (programas.Rows[i]["PROGRA2022"] == DBNull.Value)
                        {
                            estado = insertaPresupuesto(Convert.ToDouble(e.NewValues["PRESUPUESTO2022"]), Convert.ToDouble(programas.Rows[i]["NOPROGRAMA"].ToString()), Convert.ToInt32(programas.Rows[i]["POA2022"].ToString()), 2022, Convert.ToInt32(programas.Rows[i]["SPPRO$ID_POM"].ToString()), Convert.ToInt32(programas.Rows[i]["SPPRO$ID_INSTITUCION"].ToString()));
                            if (estado == 0)
                            {
                                mensaje = dao.mensaje;
                                gvProgramas.JSProperties["cpError"] = "Ocurrio un error " + mensaje;
                                e.Cancel = true;
                                gvProgramas.CancelEdit();
                                cargaprogramas();
                                break;
                            }
                        }
                        else
                        {
                            estado = ActualizaPresupuesto(Convert.ToDouble(e.NewValues["PRESUPUESTO2022"]), Convert.ToDouble(programas.Rows[i]["NOPROGRAMA"].ToString()), Convert.ToInt32(programas.Rows[i]["POA2022"].ToString()), 2022, Convert.ToInt32(programas.Rows[i]["SPPRO$ID_POM"].ToString()), Convert.ToInt32(programas.Rows[i]["SPPRO$ID_INSTITUCION"].ToString()), Convert.ToInt32(programas.Rows[i]["PROGRA2022"].ToString()));
                            if (estado == 0)
                            {
                                mensaje = dao.mensaje;
                                gvProgramas.JSProperties["cpError"] = "Ocurrio un error " + mensaje;
                                e.Cancel = true;
                                gvProgramas.CancelEdit();
                                cargaprogramas();
                                break;
                            }
                        }
                        //----------------------fin 2022


                        //----------------------inicia 2023

                        if (programas.Rows[i]["PROGRA2023"] == DBNull.Value)
                        {
                            estado = insertaPresupuesto(Convert.ToDouble(e.NewValues["PRESUPUESTO2023"]), Convert.ToDouble(programas.Rows[i]["NOPROGRAMA"].ToString()), Convert.ToInt32(programas.Rows[i]["POA2023"].ToString()), 2023, Convert.ToInt32(programas.Rows[i]["SPPRO$ID_POM"].ToString()), Convert.ToInt32(programas.Rows[i]["SPPRO$ID_INSTITUCION"].ToString()));
                            if (estado == 0)
                            {
                                mensaje = dao.mensaje;
                                gvProgramas.JSProperties["cpError"] = "Ocurrio un error " + mensaje;
                                e.Cancel = true;
                                gvProgramas.CancelEdit();
                                cargaprogramas();
                                break;
                            }
                        }
                        else
                        {
                            estado = ActualizaPresupuesto(Convert.ToDouble(e.NewValues["PRESUPUESTO2023"]), Convert.ToDouble(programas.Rows[i]["NOPROGRAMA"].ToString()), Convert.ToInt32(programas.Rows[i]["POA2023"].ToString()), 2023, Convert.ToInt32(programas.Rows[i]["SPPRO$ID_POM"].ToString()), Convert.ToInt32(programas.Rows[i]["SPPRO$ID_INSTITUCION"].ToString()), Convert.ToInt32(programas.Rows[i]["PROGRA2023"].ToString()));
                            if (estado == 0)
                            {
                                mensaje = dao.mensaje;
                                gvProgramas.JSProperties["cpError"] = "Ocurrio un error " + mensaje;
                                e.Cancel = true;
                                gvProgramas.CancelEdit();
                                cargaprogramas();
                                break;
                            }
                        }
                        //----------------------fin 2023



                        //----------------------inicia 2024

                        if (programas.Rows[i]["PROGRA2024"] == DBNull.Value)
                        {
                            estado = insertaPresupuesto(Convert.ToDouble(e.NewValues["PRESUPUESTO2024"]), Convert.ToDouble(programas.Rows[i]["NOPROGRAMA"].ToString()), Convert.ToInt32(programas.Rows[i]["POA2024"].ToString()), 2024, Convert.ToInt32(programas.Rows[i]["SPPRO$ID_POM"].ToString()), Convert.ToInt32(programas.Rows[i]["SPPRO$ID_INSTITUCION"].ToString()));
                            if (estado == 0)
                            {
                                mensaje = dao.mensaje;
                                gvProgramas.JSProperties["cpError"] = "Ocurrio un error " + mensaje;
                                e.Cancel = true;
                                gvProgramas.CancelEdit();
                                cargaprogramas();
                                break;
                            }
                        }
                        else
                        {
                            estado = ActualizaPresupuesto(Convert.ToDouble(e.NewValues["PRESUPUESTO2024"]), Convert.ToDouble(programas.Rows[i]["NOPROGRAMA"].ToString()), Convert.ToInt32(programas.Rows[i]["POA2024"].ToString()), 2024, Convert.ToInt32(programas.Rows[i]["SPPRO$ID_POM"].ToString()), Convert.ToInt32(programas.Rows[i]["SPPRO$ID_INSTITUCION"].ToString()), Convert.ToInt32(programas.Rows[i]["PROGRA2024"].ToString()));
                            if (estado == 0)
                            {
                                mensaje = dao.mensaje;
                                gvProgramas.JSProperties["cpError"] = "Ocurrio un error " + mensaje;
                                e.Cancel = true;
                                gvProgramas.CancelEdit();
                                cargaprogramas();
                                break;
                            }
                        }
                        //----------------------fin 2024








                        /*
                        //----------------------inicia 2025

                        if (programas.Rows[i]["PROGRA2025"] == DBNull.Value)
                        {
                            estado = insertaPresupuesto(Convert.ToDouble(e.NewValues["PRESUPUESTO2025"]), Convert.ToDouble(programas.Rows[i]["NOPROGRAMA"].ToString()), Convert.ToInt32(programas.Rows[i]["POA2025"].ToString()), 2025, Convert.ToInt32(programas.Rows[i]["SPPRO$ID_POM"].ToString()), Convert.ToInt32(programas.Rows[i]["SPPRO$ID_INSTITUCION"].ToString()));
                            if (estado == 0)
                            {
                                mensaje = dao.mensaje;
                                gvProgramas.JSProperties["cpError"] = "Ocurrio un error " + mensaje;
                                e.Cancel = true;
                                gvProgramas.CancelEdit();
                                cargaprogramas();
                                break;
                            }
                        }
                        else
                        {
                            estado = ActualizaPresupuesto(Convert.ToDouble(e.NewValues["PRESUPUESTO2025"]), Convert.ToDouble(programas.Rows[i]["NOPROGRAMA"].ToString()), Convert.ToInt32(programas.Rows[i]["POA2025"].ToString()), 2025, Convert.ToInt32(programas.Rows[i]["SPPRO$ID_POM"].ToString()), Convert.ToInt32(programas.Rows[i]["SPPRO$ID_INSTITUCION"].ToString()), Convert.ToInt32(programas.Rows[i]["PROGRA2025"].ToString()));
                            if (estado == 0)
                            {
                                mensaje = dao.mensaje;
                                gvProgramas.JSProperties["cpError"] = "Ocurrio un error " + mensaje;
                                e.Cancel = true;
                                gvProgramas.CancelEdit();
                                cargaprogramas();
                                break;
                            }
                        }
                        //----------------------fin 2025

                        */

                        if (estado == 0)
                        {
                            mensaje = dao.mensaje;
                            gvProgramas.JSProperties["cpError"] = "Ocurrio un error " + mensaje;
                            e.Cancel = true;
                            gvProgramas.CancelEdit();
                            cargaprogramas();
                            break;
                        }
                        else
                        {
                            mensaje = "Presupuestos registrados correctamente";
                            gvProgramas.JSProperties["cpError"] = "Información: " + mensaje;
                            e.Cancel = true;
                            gvProgramas.CancelEdit();
                            cargaprogramas();
                            break;
                        }



                    }//inicio
                }

            }


        }

        protected int insertaPresupuesto(double presupuesto, double programa, int poa, int anio, int pom, int insto)
        {
            int estadoss;
            sql = "INSERT INTO SCHE$SIPLAN20.SP20$PROGRA_PRESUPUESTO(SPPRES$PRESUPUESTO, SPPRES$PRESUPUESTO_VIGENTE, SPPRES$ID_PROGRAMA_PRESUPUESTO, SPPRES$ID_POA, SPPRES$ANIO, SPPRES$FECHA_INSERTA, SPRES$POM, SPRES$INSTO, SPRES$TIPO) VALUES(" + presupuesto + ", " + presupuesto + ", " + programa + ", " + poa + ", " + anio + ", 'INSERTA = ' || TO_CHAR(SYSDATE, 'DD/MM/YYYY HH:MI') || ' ' || '" + Session["USUARIO"].ToString() + "', " + pom + ", " + insto + ", 0)";
            estadoss = dao.comando(sql);
            return estadoss;
        }


        protected int ActualizaPresupuesto(double presupuesto, double programa, int poa, int anio, int pom, int insto, int progra)
        {
            int estadoss = 0;
            DataTable tabla = new DataTable();
            /*sql = @"SELECT CASE WHEN SPOA$ESTADO = 'A' THEN  1 
                                WHEN  SPOA$ESTADO = 'D'  THEN 0 END AS ESTADO FROM SCHE$SIPLAN20.SP20$POA WHERE SPOA$ID_POA = " + poa + " AND SPOA$ANIO = " + anio + " AND SPOA$ID_POM = " + pom + " AND SPOA$ID_INSTITUCION = " + insto;
            */

            sql = @"SELECT 
                        CASE WHEN SPPO$ESTADO = 'A' THEN 1 
                        WHEN SPPO$ESTADO = 'D' AND CONTEO_PRORROGA > 0 AND TRUNC(SYSDATE) <= TRUNC(FECHA_PRORROGA) THEN 0 
                        WHEN SPPO$ESTADO = 'D' AND CONTEO = 0 THEN 1 
                        WHEN SPPO$ESTADO = 'D' AND CONTEO > 0 AND TRUNC(SYSDATE) > TRUNC(FECHA_CIERRE) THEN 1
                        WHEN SPPO$ESTADO = 'D' AND CONTEO > 0 AND TRUNC(SYSDATE) <= TRUNC(FECHA_CIERRE) THEN 0
                        WHEN SPPO$ESTADO = 'D' AND CONTEO > 0 AND FECHA_CIERRE IS NULL THEN 0
                        END AS ESTADO
                        FROM 
                                
                                (SELECT 
                             POM.SPPO$ESTADO
                            ,(SELECT SPFC$FECHA_CIERRE FROM SCHE$SIPLAN20.SP20$FECHAS_CIERRE WHERE SPFC$TIPO_FECHA = 2 AND SPFC$RESTRICTIVA = 'N' AND SPFC$PERIODO_POM = POM.SPPO$ID_PERIODO AND SPFC$EJERCICIO = POA.SPOA$ANIO AND SPFC$POM = -1 AND SPFC$INSTITUCION = -1) FECHA_CIERRE
                            ,(SELECT COUNT(SPFC$FECHA_CIERRE) CONTEO FROM SCHE$SIPLAN20.SP20$FECHAS_CIERRE WHERE SPFC$TIPO_FECHA = 2 AND SPFC$RESTRICTIVA = 'N' AND SPFC$PERIODO_POM = POM.SPPO$ID_PERIODO AND SPFC$POM = -1 AND SPFC$INSTITUCION = -1) CONTEO

                                
                               ,(SELECT MAX(SPFC$FECHA_CIERRE) FROM SCHE$SIPLAN20.SP20$FECHAS_CIERRE WHERE SPFC$TIPO_FECHA = 2 AND SPFC$RESTRICTIVA = 'N' AND SPFC$PERIODO_POM = POM.SPPO$ID_PERIODO AND SPFC$EJERCICIO = POA.SPOA$ANIO AND SPFC$POM = POM.SPPO$ID_POM AND  SPFC$INSTITUCION = POM.SPPO$ID_INSTITUCION) FECHA_PRORROGA
                               ,(SELECT COUNT(SPFC$FECHA_CIERRE) CONTEO FROM SCHE$SIPLAN20.SP20$FECHAS_CIERRE WHERE SPFC$TIPO_FECHA = 2 AND SPFC$RESTRICTIVA = 'N' AND SPFC$PERIODO_POM = POM.SPPO$ID_PERIODO AND SPFC$EJERCICIO = POA.SPOA$ANIO AND SPFC$POM = POM.SPPO$ID_POM AND  SPFC$INSTITUCION = POM.SPPO$ID_INSTITUCION) CONTEO_PRORROGA
                                
                                FROM SCHE$SIPLAN20.SP20$POA POA
                                INNER JOIN SCHE$SIPLAN20.SP20$POM POM ON POM.SPPO$ID_POM = POA.SPOA$ID_POM AND POM.SPPO$ID_INSTITUCION = POA.SPOA$ID_INSTITUCION AND POM.SPPO$RESTRICTIVA = 'N' AND POA.SPOA$RESTRICTIVA = 'N'

                                WHERE SPOA$ID_POA = "+poa+" AND SPOA$ANIO = "+anio+" AND SPOA$ID_POM = "+pom+" AND SPOA$ID_INSTITUCION = "+insto+")";
            estado = dao.consulta(sql);
            if (estado != 0)
            {
                tabla = dao.tabla;
                if (Convert.ToInt32(tabla.Rows[0]["ESTADO"]) == 0)
                {
                    sql = "UPDATE SCHE$SIPLAN20.SP20$PROGRA_PRESUPUESTO SET SPPRES$PRESUPUESTO =" + presupuesto + ", SPPRES$PRESUPUESTO_VIGENTE = " + presupuesto + ", SPPRES$FECHA_UPDATE = 'UPDATE = ' || TO_CHAR(SYSDATE, 'DD/MM/YYYY HH:MI') || ' ' || '" + Session["USUARIO"].ToString() + "' WHERE SPPRES$ID_POA =  " + poa + " AND SPPRES$ANIO = " + anio + " AND SPRES$POM = " + pom + " AND SPRES$INSTO = " + insto + " AND SPPRES$RESTRICTIVA = 'N' AND SPPRES$ID_PROGRAMACIONPRES = " + progra;
                    estadoss = dao.comando(sql);
                }
                else
                {
                    estadoss = 1;
                }
            }

            else
                estadoss = 0;


            return estadoss;
        }

        protected void btnProgramas_Click(object sender, EventArgs e)
        {
            cargaprogramas();
            MultiView1.ActiveViewIndex = 0;
        }

        protected void btnSuproductos_Click(object sender, EventArgs e)
        {
            DataTable poas = (DataTable)Session["poa"];
            if (poas.Rows.Count > 0)
            {
                Session["carga"] = 1;
                cargaSubproductos(poas, Convert.ToInt32(cbTipoProduccion.Value));
                MultiView1.ActiveViewIndex = 1;
            }

        }

        protected DataTable cargaResultados(Double programa, int tipo)
        {
            String sql;
            DataTable tablas = new DataTable();
            if (tipo == 0)
            {
                sql = "SELECT ID_RESULTADO||'-'||SPPRO$RESULTADO2 LLAVE, SPPRO$ID_PROGRAMA_PRESUPUESTO, ID_RESULTADO, RESULTADO, EJE_ACCION, NULL RED, TIPO, SPPRO$RESULTADO2 FROM(SELECT VES.SPPRO$ID_PROGRAMA_PRESUPUESTO, VES.ID_RESULTADO, CASE WHEN VES.RESULTADO IS NULL THEN 'SIN VINCULACIÓN' ELSE VES.RESULTADO END AS RESULTADO, CASE WHEN VES.ID_EJE IS NULL THEN 'SIN VINCULACION' ELSE 'EJE: '||VES.EJE_ESTRATEGICO||' ACCION:'||VES.ACCION_ESTRATEGICA END AS EJE_ACCION, VES.TIPO, VES.SPPRO$RESULTADO2 FROM SCHE$SIPLAN20.SPPVST$SUBESTRATEGICOV2 VES WHERE VES.SPRES$POM = " + Session["pom"] + " AND VES.SPRES$INSTITUCION = " + Session["insto"] + " AND VES.SPPRO$ID_PROGRAMA_PRESUPUESTO = " + programa + ")  GROUP BY SPPRO$ID_PROGRAMA_PRESUPUESTO, ID_RESULTADO, RESULTADO, EJE_ACCION, TIPO, SPPRO$RESULTADO2 ORDER BY TIPO ASC";
            }

            else
            {
                sql = "SELECT ID_RESULTADO||'-'||SPPRO$RESULTADO2 LLAVE, SPPRO$ID_PROGRAMA_PRESUPUESTO, ID_RESULTADO, NULL RESULTADO, EJE_ACCION,  RED, 2 TIPO, SPPRO$RESULTADO2 FROM(SELECT VES.SPPRO$ID_PROGRAMA_PRESUPUESTO, VES.ID_RESULTADO, CASE WHEN VES.EJE IS NULL THEN 'S/N' ELSE 'EJE: '||VES.EJE||'-'||'ACCION:'||VES.ACCION END AS EJE_ACCION, VES.RED, VES.SPPRO$RESULTADO2 FROM SCHE$SIPLAN20.SPPSVT$RED_INSTITUCIONAL VES WHERE VES.SPRES$POM = " + Session["pom"] + " AND VES.SPRES$INSTITUCION = " + Session["insto"] + " AND VES.SPPRO$ID_PROGRAMA_PRESUPUESTO = " + programa + ")  GROUP BY SPPRO$ID_PROGRAMA_PRESUPUESTO, ID_RESULTADO, EJE_ACCION, RED, SPPRO$RESULTADO2";
            }

            estado = dao.consulta(sql);
            if (estado != 0)
                tablas = dao.tabla;
            return tablas;

        }

        protected void cargaSubproductos(DataTable tabla, int tipo)
        {
            Session["carga"] = 1;
            DataTable produccion = new DataTable();
            DataTable datos = new DataTable();
            DataTable productos = new DataTable();
            DataTable subproductos = new DataTable();

            if (Convert.ToInt32(Session["periodo"]) != 0)
            {
                sql = "SELECT SPPMFS$ID_PROGRAMACION_FISICA, SPPMFS$ID_POA, SPPMFS$ANIO, SPPMFS$ID_PRODUCTO, SPPMFS$META, SPPMF$TIPO  FROM SCHE$SIPLAN20.SP20$PROGRAMACION_MFPRODUCTO ";
                sql = sql + " INNER JOIN SCHE$SIPLAN20.SP20$POA POA ON POA.SPOA$ID_POA = SPPMFS$ID_POA AND POA.SPOA$ANIO = SPPMFS$ANIO AND POA.SPOA$RESTRICTIVA = 'N' WHERE SPPMFS$RESTRICTIVA = 'N' AND SPPMFS$ID_POA IN (";

                for (int i = 0; i < tabla.Rows.Count; i++)
                {
                    if (i == 0)
                        sql = sql + tabla.Rows[i]["SPOA$ID_POA"];
                    else
                        sql = sql + "," + tabla.Rows[i]["SPOA$ID_POA"];

                }
                sql = sql + ") AND POA.SPOA$ID_INSTITUCION = " + Session["SPPO$ID_INSTITUCION"] + " AND POA.SPOA$ID_POM = " + Session["SPPO$ID_POM"];

                estado = dao.consulta(sql);

                if (estado == 1)
                    datos = dao.tabla;

                if (datos.Rows.Count > 0)
                {
                    if (Convert.ToInt32(Session["periodo"]) == 20)
                    {
                        if (tipo == 0)
                        {
                            sql = "SELECT SPPRO$ID_PROGRAMA_PRESUPUESTO, PROGRAMA, NULL P0A2020, NULL PRESUPUESTO2020, NULL PR0GRAMADO2020, P0A2021, PRESUPUESTO2021, PR0GRAMADO2021, P0A2022, PRESUPUESTO2022, PR0GRAMADO2022, P0A2023, PRESUPUESTO2023, PR0GRAMADO2023, P0A2024, PRESUPUESTO2024, PR0GRAMADO2024, P0A2025, PRESUPUESTO2025,  PR0GRAMADO2025, NULL P0A2026, NULL PRESUPUESTO2026,  NULL PR0GRAMADO2026, NULL P0A2027, NULL PRESUPUESTO2027,  NULL PR0GRAMADO2027 FROM (SELECT VES.ID_RESULTADO, VES.RESULTADO,VES.SPPRO$ID_PROGRAMA_PRESUPUESTO, VES.SPPRO$ID_PROGRAMA_PRESUPUESTO||'-'||VES.PROGRAMA PROGRAMA";
                            for (int i = 0; i < tabla.Rows.Count; i++)
                            {
                                sql = sql + " ,SCHE$SIPLAN20.FNC$POA(VES.SPRES$POM,VES.SPRES$INSTITUCION," + tabla.Rows[i]["SPOA$ANIO"] + ") P0A" + tabla.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$OBTIENEPRESUPUESTO(VES.SPPRO$ID_PROGRAMA_PRESUPUESTO," + tabla.Rows[i]["SPOA$ID_POA"] + ", " + tabla.Rows[i]["SPOA$ANIO"] + ",VES.SPRES$POM, VES.SPRES$INSTITUCION) PRESUPUESTO" + tabla.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$OBTIENEPROGRAMADO(VES.SPRES$POM, " + tabla.Rows[i]["SPOA$ID_POA"] + ", " + tabla.Rows[i]["SPOA$ANIO"] + ", VES.SPRES$INSTITUCION, VES.SPPRO$ID_PROGRAMA_PRESUPUESTO) PR0GRAMADO" + tabla.Rows[i]["SPOA$ANIO"];
                            }


                            sql = sql + " ,VES.SPPSUB$PROPIETARIO ";
                            sql = sql + " FROM SCHE$SIPLAN20.SPPVST$SUBESTRATEGICOV2 VES WHERE VES.SPRES$POM = " + Session["pom"] + " AND VES.SPRES$INSTITUCION = " + Session["insto"] + ") GROUP BY  SPPRO$ID_PROGRAMA_PRESUPUESTO, PROGRAMA,P0A2021, PRESUPUESTO2021, PR0GRAMADO2021, P0A2022, PRESUPUESTO2022, PR0GRAMADO2022, P0A2023, PRESUPUESTO2023, PR0GRAMADO2023, P0A2024, PRESUPUESTO2024, PR0GRAMADO2024, P0A2025, PRESUPUESTO2025, PR0GRAMADO2025 ORDER BY  SPPRO$ID_PROGRAMA_PRESUPUESTO ASC";
                            lblTipodeProduccion.Text = "PRODUCTOS INSTITUCIONALES";
                        }


                        else if (tipo == 1)
                        {
                            sql = "SELECT  SPPRO$ID_PROGRAMA_PRESUPUESTO, PROGRAMA, NULL P0A2020, NULL PRESUPUESTO2020, NULL PR0GRAMADO2020, P0A2021, PRESUPUESTO2021, PR0GRAMADO2021, P0A2022, PRESUPUESTO2022, PR0GRAMADO2022, P0A2023, PRESUPUESTO2023, PR0GRAMADO2023, P0A2024, PRESUPUESTO2024, PR0GRAMADO2024, P0A2025, PRESUPUESTO2025,  PR0GRAMADO2025, NULL P0A2026, NULL PRESUPUESTO2026,  NULL PR0GRAMADO2026, NULL P0A2027, NULL PRESUPUESTO2027,  NULL PR0GRAMADO2027 FROM  (SELECT VES.ID_RESULTADO,VES.RED ,VES.SPPRO$ID_PROGRAMA_PRESUPUESTO, VES.SPPRO$ID_PROGRAMA_PRESUPUESTO||'-'||VES.PROGRAMA PROGRAMA";
                            for (int i = 0; i < tabla.Rows.Count; i++)
                            {
                                sql = sql + " ,SCHE$SIPLAN20.FNC$POA(VES.SPRES$POM,VES.SPRES$INSTITUCION," + tabla.Rows[i]["SPOA$ANIO"] + ") P0A" + tabla.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$OBTIENEPRESUPUESTO(VES.SPPRO$ID_PROGRAMA_PRESUPUESTO," + tabla.Rows[i]["SPOA$ID_POA"] + ", " + tabla.Rows[i]["SPOA$ANIO"] + ",VES.SPRES$POM, VES.SPRES$INSTITUCION) PRESUPUESTO" + tabla.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$OBTIENEPROGRAMADO(VES.SPRES$POM, " + tabla.Rows[i]["SPOA$ID_POA"] + ", " + tabla.Rows[i]["SPOA$ANIO"] + ", VES.SPRES$INSTITUCION, VES.SPPRO$ID_PROGRAMA_PRESUPUESTO) PR0GRAMADO" + tabla.Rows[i]["SPOA$ANIO"];
                            }

                            sql = sql + " ,VES.SPPSUB$PROPIETARIO";
                            sql = sql + " FROM SCHE$SIPLAN20.SPPSVT$RED_INSTITUCIONAL VES WHERE VES.SPRES$POM = " + Session["pom"] + " AND VES.SPRES$INSTITUCION = " + Session["insto"] + ") GROUP BY  SPPRO$ID_PROGRAMA_PRESUPUESTO,PROGRAMA, P0A2021, PRESUPUESTO2021, PR0GRAMADO2021, P0A2022, PRESUPUESTO2022, PR0GRAMADO2022, P0A2023, PRESUPUESTO2023, PR0GRAMADO2023, P0A2024, PRESUPUESTO2024, PR0GRAMADO2024, P0A2025, PRESUPUESTO2025,  PR0GRAMADO2025 ORDER BY  SPPRO$ID_PROGRAMA_PRESUPUESTO ASC";
                            lblTipodeProduccion.Text = "PRODUCTOS VINCULADOS A RED";
                        }


                    }


                    else if (Convert.ToInt32(Session["periodo"]) == 21)
                    {
                        if (tipo == 0)
                        {
                            sql = "SELECT SPPRO$ID_PROGRAMA_PRESUPUESTO, PROGRAMA, NULL P0A2020, NULL PRESUPUESTO2020, NULL PR0GRAMADO2020, NULL P0A2021,  NULL PRESUPUESTO2021, NULL PR0GRAMADO2021, P0A2022, PRESUPUESTO2022, PR0GRAMADO2022, P0A2023, PRESUPUESTO2023, PR0GRAMADO2023, P0A2024, PRESUPUESTO2024, PR0GRAMADO2024, P0A2025, PRESUPUESTO2025,  PR0GRAMADO2025, P0A2026, PRESUPUESTO2026,  PR0GRAMADO2026, NULL P0A2027, NULL PRESUPUESTO2027,  NULL PR0GRAMADO2027 FROM (SELECT VES.ID_RESULTADO, VES.RESULTADO,VES.SPPRO$ID_PROGRAMA_PRESUPUESTO, VES.SPPRO$ID_PROGRAMA_PRESUPUESTO||'-'||VES.PROGRAMA PROGRAMA";
                            for (int i = 0; i < tabla.Rows.Count; i++)
                            {
                                sql = sql + " ,SCHE$SIPLAN20.FNC$POA(VES.SPRES$POM,VES.SPRES$INSTITUCION," + tabla.Rows[i]["SPOA$ANIO"] + ") P0A" + tabla.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$OBTIENEPRESUPUESTO(VES.SPPRO$ID_PROGRAMA_PRESUPUESTO," + tabla.Rows[i]["SPOA$ID_POA"] + ", " + tabla.Rows[i]["SPOA$ANIO"] + ",VES.SPRES$POM, VES.SPRES$INSTITUCION) PRESUPUESTO" + tabla.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$OBTIENEPROGRAMADO(VES.SPRES$POM, " + tabla.Rows[i]["SPOA$ID_POA"] + ", " + tabla.Rows[i]["SPOA$ANIO"] + ", VES.SPRES$INSTITUCION, VES.SPPRO$ID_PROGRAMA_PRESUPUESTO) PR0GRAMADO" + tabla.Rows[i]["SPOA$ANIO"];
                            }


                            sql = sql + " ,VES.SPPSUB$PROPIETARIO ";
                            sql = sql + " FROM SCHE$SIPLAN20.SPPVST$SUBESTRATEGICOV2 VES WHERE VES.SPRES$POM = " + Session["pom"] + " AND VES.SPRES$INSTITUCION = " + Session["insto"] + ") GROUP BY  SPPRO$ID_PROGRAMA_PRESUPUESTO, PROGRAMA,P0A2022, PRESUPUESTO2022, PR0GRAMADO2022, P0A2023, PRESUPUESTO2023, PR0GRAMADO2023, P0A2024, PRESUPUESTO2024, PR0GRAMADO2024, P0A2025, PRESUPUESTO2025, PR0GRAMADO2025, P0A2026, PRESUPUESTO2026, PR0GRAMADO2026 ORDER BY  SPPRO$ID_PROGRAMA_PRESUPUESTO ASC";
                            lblTipodeProduccion.Text = "PRODUCTOS INSTITUCIONALES";
                        }


                        else if (tipo == 1)
                        {
                            sql = "SELECT  SPPRO$ID_PROGRAMA_PRESUPUESTO, PROGRAMA, NULL P0A2020, NULL PRESUPUESTO2020, NULL PR0GRAMADO2020, NULL P0A2021, NULL PRESUPUESTO2021, NULL PR0GRAMADO2021, P0A2022, PRESUPUESTO2022, PR0GRAMADO2022, P0A2023, PRESUPUESTO2023, PR0GRAMADO2023, P0A2024, PRESUPUESTO2024, PR0GRAMADO2024, P0A2025, PRESUPUESTO2025,  PR0GRAMADO2025, P0A2026, PRESUPUESTO2026,  PR0GRAMADO2026, NULL P0A2027, NULL PRESUPUESTO2027,  NULL PR0GRAMADO2027 FROM  (SELECT VES.ID_RESULTADO, VES.RED,VES.SPPRO$ID_PROGRAMA_PRESUPUESTO, VES.SPPRO$ID_PROGRAMA_PRESUPUESTO||'-'||VES.PROGRAMA PROGRAMA";
                            for (int i = 0; i < tabla.Rows.Count; i++)
                            {
                                sql = sql + " ,SCHE$SIPLAN20.FNC$POA(VES.SPRES$POM,VES.SPRES$INSTITUCION," + tabla.Rows[i]["SPOA$ANIO"] + ") P0A" + tabla.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$OBTIENEPRESUPUESTO(VES.SPPRO$ID_PROGRAMA_PRESUPUESTO," + tabla.Rows[i]["SPOA$ID_POA"] + ", " + tabla.Rows[i]["SPOA$ANIO"] + ",VES.SPRES$POM, VES.SPRES$INSTITUCION) PRESUPUESTO" + tabla.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$OBTIENEPROGRAMADO(VES.SPRES$POM, " + tabla.Rows[i]["SPOA$ID_POA"] + ", " + tabla.Rows[i]["SPOA$ANIO"] + ", VES.SPRES$INSTITUCION, VES.SPPRO$ID_PROGRAMA_PRESUPUESTO) PR0GRAMADO" + tabla.Rows[i]["SPOA$ANIO"];
                            }

                            sql = sql + " ,VES.SPPSUB$PROPIETARIO";
                            sql = sql + " FROM SCHE$SIPLAN20.SPPSVT$RED_INSTITUCIONAL VES WHERE VES.SPRES$POM = " + Session["pom"] + " AND VES.SPRES$INSTITUCION = " + Session["insto"] + ") GROUP BY  SPPRO$ID_PROGRAMA_PRESUPUESTO,PROGRAMA, P0A2022, PRESUPUESTO2022, PR0GRAMADO2022, P0A2023, PRESUPUESTO2023, PR0GRAMADO2023, P0A2024, PRESUPUESTO2024, PR0GRAMADO2024, P0A2025, PRESUPUESTO2025, PR0GRAMADO2025, P0A2026, PRESUPUESTO2026,  PR0GRAMADO2026 ORDER BY  SPPRO$ID_PROGRAMA_PRESUPUESTO ASC";
                            lblTipodeProduccion.Text = "PRODUCTOS VINCULADOS A RED";
                        }


                    }



                    else if (Convert.ToInt32(Session["periodo"]) == 22)
                    {
                        if (tipo == 0)
                        {
                            sql = "SELECT SPPRO$ID_PROGRAMA_PRESUPUESTO, PROGRAMA, NULL P0A2020, NULL PRESUPUESTO2020, NULL PR0GRAMADO2020, NULL P0A2021,  NULL PRESUPUESTO2021, NULL PR0GRAMADO2021, NULL P0A2022, NULL PRESUPUESTO2022, NULL PR0GRAMADO2022, P0A2023, PRESUPUESTO2023, PR0GRAMADO2023, P0A2024, PRESUPUESTO2024, PR0GRAMADO2024, P0A2025, PRESUPUESTO2025,  PR0GRAMADO2025, P0A2026, PRESUPUESTO2026,  PR0GRAMADO2026, P0A2027, PRESUPUESTO2027,  PR0GRAMADO2027 FROM (SELECT VES.ID_RESULTADO, VES.RESULTADO,VES.SPPRO$ID_PROGRAMA_PRESUPUESTO, VES.SPPRO$ID_PROGRAMA_PRESUPUESTO||'-'||VES.PROGRAMA PROGRAMA";
                            for (int i = 0; i < tabla.Rows.Count; i++)
                            {
                                sql = sql + " ,SCHE$SIPLAN20.FNC$POA(VES.SPRES$POM,VES.SPRES$INSTITUCION," + tabla.Rows[i]["SPOA$ANIO"] + ") P0A" + tabla.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$OBTIENEPRESUPUESTO(VES.SPPRO$ID_PROGRAMA_PRESUPUESTO," + tabla.Rows[i]["SPOA$ID_POA"] + ", " + tabla.Rows[i]["SPOA$ANIO"] + ",VES.SPRES$POM, VES.SPRES$INSTITUCION) PRESUPUESTO" + tabla.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$OBTIENEPROGRAMADO(VES.SPRES$POM, " + tabla.Rows[i]["SPOA$ID_POA"] + ", " + tabla.Rows[i]["SPOA$ANIO"] + ", VES.SPRES$INSTITUCION, VES.SPPRO$ID_PROGRAMA_PRESUPUESTO) PR0GRAMADO" + tabla.Rows[i]["SPOA$ANIO"];
                            }


                            sql = sql + " ,VES.SPPSUB$PROPIETARIO ";
                            sql = sql + " FROM SCHE$SIPLAN20.SPPVST$SUBESTRATEGICOV2 VES WHERE VES.SPRES$POM = " + Session["pom"] + " AND VES.SPRES$INSTITUCION = " + Session["insto"] + ") GROUP BY  SPPRO$ID_PROGRAMA_PRESUPUESTO, PROGRAMA, P0A2023, PRESUPUESTO2023, PR0GRAMADO2023, P0A2024, PRESUPUESTO2024, PR0GRAMADO2024, P0A2025, PRESUPUESTO2025, PR0GRAMADO2025, P0A2026, PRESUPUESTO2026, PR0GRAMADO2026, P0A2027, PRESUPUESTO2027, PR0GRAMADO2027  ORDER BY  SPPRO$ID_PROGRAMA_PRESUPUESTO ASC";
                            lblTipodeProduccion.Text = "PRODUCTOS INSTITUCIONALES";
                        }


                        else if (tipo == 1)
                        {
                            sql = "SELECT  SPPRO$ID_PROGRAMA_PRESUPUESTO, PROGRAMA, NULL P0A2020, NULL PRESUPUESTO2020, NULL PR0GRAMADO2020, NULL P0A2021, NULL PRESUPUESTO2021, NULL PR0GRAMADO2021, NULL P0A2022, NULL PRESUPUESTO2022, NULL PR0GRAMADO2022, P0A2023, PRESUPUESTO2023, PR0GRAMADO2023, P0A2024, PRESUPUESTO2024, PR0GRAMADO2024, P0A2025, PRESUPUESTO2025,  PR0GRAMADO2025, P0A2026, PRESUPUESTO2026,  PR0GRAMADO2026, P0A2027, PRESUPUESTO2027, PR0GRAMADO2027 FROM  (SELECT VES.ID_RESULTADO, VES.RED,VES.SPPRO$ID_PROGRAMA_PRESUPUESTO, VES.SPPRO$ID_PROGRAMA_PRESUPUESTO||'-'||VES.PROGRAMA PROGRAMA";
                            for (int i = 0; i < tabla.Rows.Count; i++)
                            {
                                sql = sql + " ,SCHE$SIPLAN20.FNC$POA(VES.SPRES$POM,VES.SPRES$INSTITUCION," + tabla.Rows[i]["SPOA$ANIO"] + ") P0A" + tabla.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$OBTIENEPRESUPUESTO(VES.SPPRO$ID_PROGRAMA_PRESUPUESTO," + tabla.Rows[i]["SPOA$ID_POA"] + ", " + tabla.Rows[i]["SPOA$ANIO"] + ",VES.SPRES$POM, VES.SPRES$INSTITUCION) PRESUPUESTO" + tabla.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$OBTIENEPROGRAMADO(VES.SPRES$POM, " + tabla.Rows[i]["SPOA$ID_POA"] + ", " + tabla.Rows[i]["SPOA$ANIO"] + ", VES.SPRES$INSTITUCION, VES.SPPRO$ID_PROGRAMA_PRESUPUESTO) PR0GRAMADO" + tabla.Rows[i]["SPOA$ANIO"];
                            }

                            sql = sql + " ,VES.SPPSUB$PROPIETARIO";
                            sql = sql + " FROM SCHE$SIPLAN20.SPPSVT$RED_INSTITUCIONAL VES WHERE VES.SPRES$POM = " + Session["pom"] + " AND VES.SPRES$INSTITUCION = " + Session["insto"] + ") GROUP BY  SPPRO$ID_PROGRAMA_PRESUPUESTO,PROGRAMA, P0A2023, PRESUPUESTO2023, PR0GRAMADO2023, P0A2024, PRESUPUESTO2024, PR0GRAMADO2024, P0A2025, PRESUPUESTO2025, PR0GRAMADO2025, P0A2026, PRESUPUESTO2026,  PR0GRAMADO2026, P0A2027, PRESUPUESTO2027, PR0GRAMADO2027  ORDER BY  SPPRO$ID_PROGRAMA_PRESUPUESTO ASC";
                            lblTipodeProduccion.Text = "PRODUCTOS VINCULADOS A RED";
                        }


                    }




                    else if (Convert.ToInt32(Session["periodo"]) == 23)
                    {
                        if (tipo == 0)
                        {
                            sql = "SELECT SPPRO$ID_PROGRAMA_PRESUPUESTO, PROGRAMA, NULL P0A2020, NULL PRESUPUESTO2020, NULL PR0GRAMADO2020, NULL P0A2021,  NULL PRESUPUESTO2021, NULL PR0GRAMADO2021, NULL P0A2022, NULL PRESUPUESTO2022, NULL PR0GRAMADO2022, NULL P0A2023, NULL PRESUPUESTO2023, NULL PR0GRAMADO2023, P0A2024, PRESUPUESTO2024, PR0GRAMADO2024, P0A2025, PRESUPUESTO2025,  PR0GRAMADO2025, P0A2026, PRESUPUESTO2026,  PR0GRAMADO2026, P0A2027, PRESUPUESTO2027,  PR0GRAMADO2027    , P0A2028, PRESUPUESTO2028,  PR0GRAMADO2028 FROM (SELECT VES.ID_RESULTADO, VES.RESULTADO,VES.SPPRO$ID_PROGRAMA_PRESUPUESTO, VES.SPPRO$ID_PROGRAMA_PRESUPUESTO||'-'||VES.PROGRAMA PROGRAMA";
                            for (int i = 0; i < tabla.Rows.Count; i++)
                            {
                                sql = sql + " ,SCHE$SIPLAN20.FNC$POA(VES.SPRES$POM,VES.SPRES$INSTITUCION," + tabla.Rows[i]["SPOA$ANIO"] + ") P0A" + tabla.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$OBTIENEPRESUPUESTO(VES.SPPRO$ID_PROGRAMA_PRESUPUESTO," + tabla.Rows[i]["SPOA$ID_POA"] + ", " + tabla.Rows[i]["SPOA$ANIO"] + ",VES.SPRES$POM, VES.SPRES$INSTITUCION) PRESUPUESTO" + tabla.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$OBTIENEPROGRAMADO(VES.SPRES$POM, " + tabla.Rows[i]["SPOA$ID_POA"] + ", " + tabla.Rows[i]["SPOA$ANIO"] + ", VES.SPRES$INSTITUCION, VES.SPPRO$ID_PROGRAMA_PRESUPUESTO) PR0GRAMADO" + tabla.Rows[i]["SPOA$ANIO"];
                            }


                            sql = sql + " ,VES.SPPSUB$PROPIETARIO ";
                            sql = sql + " FROM SCHE$SIPLAN20.SPPVST$SUBESTRATEGICOV2 VES WHERE VES.SPRES$POM = " + Session["pom"] + " AND VES.SPRES$INSTITUCION = " + Session["insto"] + ") GROUP BY  SPPRO$ID_PROGRAMA_PRESUPUESTO, PROGRAMA, P0A2024, PRESUPUESTO2024, PR0GRAMADO2024, P0A2025, PRESUPUESTO2025, PR0GRAMADO2025, P0A2026, PRESUPUESTO2026, PR0GRAMADO2026, P0A2027, PRESUPUESTO2027, PR0GRAMADO2027, P0A2028, PRESUPUESTO2028, PR0GRAMADO2028 ORDER BY  SPPRO$ID_PROGRAMA_PRESUPUESTO ASC";
                            lblTipodeProduccion.Text = "PRODUCTOS INSTITUCIONALES";
                        }


                        else if (tipo == 1)
                        {
                            sql = "SELECT  SPPRO$ID_PROGRAMA_PRESUPUESTO, PROGRAMA, NULL P0A2020, NULL PRESUPUESTO2020, NULL PR0GRAMADO2020, NULL P0A2021, NULL PRESUPUESTO2021, NULL PR0GRAMADO2021, NULL P0A2022, NULL PRESUPUESTO2022, NULL PR0GRAMADO2022, NULL P0A2023, NULL PRESUPUESTO2023, NULL PR0GRAMADO2023, P0A2024, PRESUPUESTO2024, PR0GRAMADO2024, P0A2025, PRESUPUESTO2025,  PR0GRAMADO2025, P0A2026, PRESUPUESTO2026,  PR0GRAMADO2026, P0A2027, PRESUPUESTO2027, PR0GRAMADO2027    , P0A2028, PRESUPUESTO2028, PR0GRAMADO2028 FROM  (SELECT VES.ID_RESULTADO, VES.RED,VES.SPPRO$ID_PROGRAMA_PRESUPUESTO, VES.SPPRO$ID_PROGRAMA_PRESUPUESTO||'-'||VES.PROGRAMA PROGRAMA";
                            for (int i = 0; i < tabla.Rows.Count; i++)
                            {
                                sql = sql + " ,SCHE$SIPLAN20.FNC$POA(VES.SPRES$POM,VES.SPRES$INSTITUCION," + tabla.Rows[i]["SPOA$ANIO"] + ") P0A" + tabla.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$OBTIENEPRESUPUESTO(VES.SPPRO$ID_PROGRAMA_PRESUPUESTO," + tabla.Rows[i]["SPOA$ID_POA"] + ", " + tabla.Rows[i]["SPOA$ANIO"] + ",VES.SPRES$POM, VES.SPRES$INSTITUCION) PRESUPUESTO" + tabla.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$OBTIENEPROGRAMADO(VES.SPRES$POM, " + tabla.Rows[i]["SPOA$ID_POA"] + ", " + tabla.Rows[i]["SPOA$ANIO"] + ", VES.SPRES$INSTITUCION, VES.SPPRO$ID_PROGRAMA_PRESUPUESTO) PR0GRAMADO" + tabla.Rows[i]["SPOA$ANIO"];
                            }

                            sql = sql + " ,VES.SPPSUB$PROPIETARIO";
                            sql = sql + " FROM SCHE$SIPLAN20.SPPSVT$RED_INSTITUCIONAL VES WHERE VES.SPRES$POM = " + Session["pom"] + " AND VES.SPRES$INSTITUCION = " + Session["insto"] + ") GROUP BY  SPPRO$ID_PROGRAMA_PRESUPUESTO,PROGRAMA, P0A2024, PRESUPUESTO2024, PR0GRAMADO2024, P0A2025, PRESUPUESTO2025, PR0GRAMADO2025, P0A2026, PRESUPUESTO2026,  PR0GRAMADO2026, P0A2027, PRESUPUESTO2027, PR0GRAMADO2027, P0A2028, PRESUPUESTO2028, PR0GRAMADO2028 ORDER BY  SPPRO$ID_PROGRAMA_PRESUPUESTO ASC";
                            lblTipodeProduccion.Text = "PRODUCTOS VINCULADOS A RED";
                        }


                    }
                    //NUEVA PGG

                    else if (Convert.ToInt32(Session["periodo"]) == 24)
                    {
                        if (tipo == 0)
                        {
                            sql = "SELECT SPPRO$ID_PROGRAMA_PRESUPUESTO, PROGRAMA, NULL P0A2020, NULL PRESUPUESTO2020, NULL PR0GRAMADO2020, NULL P0A2021,  NULL PRESUPUESTO2021, NULL PR0GRAMADO2021, NULL P0A2022, NULL PRESUPUESTO2022, NULL PR0GRAMADO2022, NULL P0A2023, NULL PRESUPUESTO2023, NULL PR0GRAMADO2023, NULL P0A2024, NULL PRESUPUESTO2024, NULL PR0GRAMADO2024, P0A2025, PRESUPUESTO2025,  PR0GRAMADO2025, P0A2026, PRESUPUESTO2026,  PR0GRAMADO2026, P0A2027, PRESUPUESTO2027,  PR0GRAMADO2027    , P0A2028, PRESUPUESTO2028,  PR0GRAMADO2028, P0A2029, PRESUPUESTO2029,  PR0GRAMADO2029  FROM (SELECT VES.ID_RESULTADO, VES.RESULTADO,VES.SPPRO$ID_PROGRAMA_PRESUPUESTO, VES.SPPRO$ID_PROGRAMA_PRESUPUESTO||'-'||VES.PROGRAMA PROGRAMA";
                            for (int i = 0; i < tabla.Rows.Count; i++)
                            {
                                sql = sql + " ,SCHE$SIPLAN20.FNC$POA(VES.SPRES$POM,VES.SPRES$INSTITUCION," + tabla.Rows[i]["SPOA$ANIO"] + ") P0A" + tabla.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$OBTIENEPRESUPUESTO(VES.SPPRO$ID_PROGRAMA_PRESUPUESTO," + tabla.Rows[i]["SPOA$ID_POA"] + ", " + tabla.Rows[i]["SPOA$ANIO"] + ",VES.SPRES$POM, VES.SPRES$INSTITUCION) PRESUPUESTO" + tabla.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$OBTIENEPROGRAMADO(VES.SPRES$POM, " + tabla.Rows[i]["SPOA$ID_POA"] + ", " + tabla.Rows[i]["SPOA$ANIO"] + ", VES.SPRES$INSTITUCION, VES.SPPRO$ID_PROGRAMA_PRESUPUESTO) PR0GRAMADO" + tabla.Rows[i]["SPOA$ANIO"];
                            }


                            sql = sql + " ,VES.SPPSUB$PROPIETARIO ";
                            sql = sql + " FROM SCHE$SIPLAN20.SPPVST$SUBESTRATEGICOV2 VES WHERE VES.SPRES$POM = " + Session["pom"] + " AND VES.SPRES$INSTITUCION = " + Session["insto"] + ") GROUP BY  SPPRO$ID_PROGRAMA_PRESUPUESTO, PROGRAMA, P0A2025, PRESUPUESTO2025, PR0GRAMADO2025, P0A2026, PRESUPUESTO2026, PR0GRAMADO2026, P0A2027, PRESUPUESTO2027, PR0GRAMADO2027, P0A2028, PRESUPUESTO2028, PR0GRAMADO2028, P0A2029, PRESUPUESTO2029, PR0GRAMADO2029 ORDER BY  SPPRO$ID_PROGRAMA_PRESUPUESTO ASC";
                            lblTipodeProduccion.Text = "PRODUCTOS INSTITUCIONALES";
                        }


                        else if (tipo == 1)
                        {
                            sql = "SELECT  SPPRO$ID_PROGRAMA_PRESUPUESTO, PROGRAMA, NULL P0A2020, NULL PRESUPUESTO2020, NULL PR0GRAMADO2020, NULL P0A2021, NULL PRESUPUESTO2021, NULL PR0GRAMADO2021, NULL P0A2022, NULL PRESUPUESTO2022, NULL PR0GRAMADO2022, NULL P0A2023, NULL PRESUPUESTO2023, NULL PR0GRAMADO2023, NULL P0A2024, NULL PRESUPUESTO2024, NULL PR0GRAMADO2024, P0A2025, PRESUPUESTO2025,  PR0GRAMADO2025, P0A2026, PRESUPUESTO2026,  PR0GRAMADO2026, P0A2027, PRESUPUESTO2027, PR0GRAMADO2027, P0A2028, PRESUPUESTO2028, PR0GRAMADO2028, P0A2029, PRESUPUESTO2029, PR0GRAMADO2029 FROM  (SELECT VES.ID_RESULTADO, VES.RED,VES.SPPRO$ID_PROGRAMA_PRESUPUESTO, VES.SPPRO$ID_PROGRAMA_PRESUPUESTO||'-'||VES.PROGRAMA PROGRAMA";
                            for (int i = 0; i < tabla.Rows.Count; i++)
                            {
                                sql = sql + " ,SCHE$SIPLAN20.FNC$POA(VES.SPRES$POM,VES.SPRES$INSTITUCION," + tabla.Rows[i]["SPOA$ANIO"] + ") P0A" + tabla.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$OBTIENEPRESUPUESTO(VES.SPPRO$ID_PROGRAMA_PRESUPUESTO," + tabla.Rows[i]["SPOA$ID_POA"] + ", " + tabla.Rows[i]["SPOA$ANIO"] + ",VES.SPRES$POM, VES.SPRES$INSTITUCION) PRESUPUESTO" + tabla.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$OBTIENEPROGRAMADO(VES.SPRES$POM, " + tabla.Rows[i]["SPOA$ID_POA"] + ", " + tabla.Rows[i]["SPOA$ANIO"] + ", VES.SPRES$INSTITUCION, VES.SPPRO$ID_PROGRAMA_PRESUPUESTO) PR0GRAMADO" + tabla.Rows[i]["SPOA$ANIO"];
                            }

                            sql = sql + " ,VES.SPPSUB$PROPIETARIO";
                            sql = sql + " FROM SCHE$SIPLAN20.SPPSVT$RED_INSTITUCIONAL VES WHERE VES.SPRES$POM = " + Session["pom"] + " AND VES.SPRES$INSTITUCION = " + Session["insto"] + ") GROUP BY  SPPRO$ID_PROGRAMA_PRESUPUESTO,PROGRAMA, P0A2025, PRESUPUESTO2025, PR0GRAMADO2025, P0A2026, PRESUPUESTO2026,  PR0GRAMADO2026, P0A2027, PRESUPUESTO2027, PR0GRAMADO2027, P0A2028, PRESUPUESTO2028, PR0GRAMADO2028, P0A2029, PRESUPUESTO2029, PR0GRAMADO2029 ORDER BY  SPPRO$ID_PROGRAMA_PRESUPUESTO ASC";
                            lblTipodeProduccion.Text = "PRODUCTOS VINCULADOS A RED";
                        }


                    }

                    //inicio 2026-2030

                    else if (Convert.ToInt32(Session["periodo"]) == 25)
                    {
                        if (tipo == 0)
                        {
                            sql = "SELECT SPPRO$ID_PROGRAMA_PRESUPUESTO, PROGRAMA, NULL P0A2020, NULL PRESUPUESTO2020, NULL PR0GRAMADO2020, NULL P0A2021,  NULL PRESUPUESTO2021, NULL PR0GRAMADO2021, NULL P0A2022, NULL PRESUPUESTO2022, NULL PR0GRAMADO2022, NULL P0A2023, NULL PRESUPUESTO2023, NULL PR0GRAMADO2023, NULL P0A2024, NULL PRESUPUESTO2024, NULL PR0GRAMADO2024, NULL P0A2025, NULL PRESUPUESTO2025,  NULL PR0GRAMADO2025, P0A2026, PRESUPUESTO2026,  PR0GRAMADO2026, P0A2027, PRESUPUESTO2027,  PR0GRAMADO2027    , P0A2028, PRESUPUESTO2028,  PR0GRAMADO2028, P0A2029, PRESUPUESTO2029,  PR0GRAMADO2029, P0A2030, PRESUPUESTO2030,  PR0GRAMADO2030  FROM (SELECT VES.ID_RESULTADO, VES.RESULTADO,VES.SPPRO$ID_PROGRAMA_PRESUPUESTO, VES.SPPRO$ID_PROGRAMA_PRESUPUESTO||'-'||VES.PROGRAMA PROGRAMA";
                            for (int i = 0; i < tabla.Rows.Count; i++)
                            {
                                sql = sql + " ,SCHE$SIPLAN20.FNC$POA(VES.SPRES$POM,VES.SPRES$INSTITUCION," + tabla.Rows[i]["SPOA$ANIO"] + ") P0A" + tabla.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$OBTIENEPRESUPUESTO(VES.SPPRO$ID_PROGRAMA_PRESUPUESTO," + tabla.Rows[i]["SPOA$ID_POA"] + ", " + tabla.Rows[i]["SPOA$ANIO"] + ",VES.SPRES$POM, VES.SPRES$INSTITUCION) PRESUPUESTO" + tabla.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$OBTIENEPROGRAMADO(VES.SPRES$POM, " + tabla.Rows[i]["SPOA$ID_POA"] + ", " + tabla.Rows[i]["SPOA$ANIO"] + ", VES.SPRES$INSTITUCION, VES.SPPRO$ID_PROGRAMA_PRESUPUESTO) PR0GRAMADO" + tabla.Rows[i]["SPOA$ANIO"];
                            }


                            sql = sql + " ,VES.SPPSUB$PROPIETARIO ";
                            sql = sql + " FROM SCHE$SIPLAN20.SPPVST$SUBESTRATEGICOV2 VES WHERE VES.SPRES$POM = " + Session["pom"] + " AND VES.SPRES$INSTITUCION = " + Session["insto"] + ") GROUP BY  SPPRO$ID_PROGRAMA_PRESUPUESTO, PROGRAMA, P0A2026, PRESUPUESTO2026, PR0GRAMADO2026, P0A2027, PRESUPUESTO2027, PR0GRAMADO2027, P0A2028, PRESUPUESTO2028, PR0GRAMADO2028, P0A2029, PRESUPUESTO2029, PR0GRAMADO2029, P0A2030, PRESUPUESTO2030, PR0GRAMADO2030 ORDER BY  SPPRO$ID_PROGRAMA_PRESUPUESTO ASC";
                            lblTipodeProduccion.Text = "PRODUCTOS INSTITUCIONALES";
                        }


                        else if (tipo == 1)
                        {
                            sql = "SELECT  SPPRO$ID_PROGRAMA_PRESUPUESTO, PROGRAMA, NULL P0A2020, NULL PRESUPUESTO2020, NULL PR0GRAMADO2020, NULL P0A2021, NULL PRESUPUESTO2021, NULL PR0GRAMADO2021, NULL P0A2022, NULL PRESUPUESTO2022, NULL PR0GRAMADO2022, NULL P0A2023, NULL PRESUPUESTO2023, NULL PR0GRAMADO2023, NULL P0A2024, NULL PRESUPUESTO2024, NULL PR0GRAMADO2024, NULL P0A2025, NULL PRESUPUESTO2025,  NULL PR0GRAMADO2025, P0A2026, PRESUPUESTO2026,  PR0GRAMADO2026, P0A2027, PRESUPUESTO2027, PR0GRAMADO2027, P0A2028, PRESUPUESTO2028, PR0GRAMADO2028, P0A2029, PRESUPUESTO2029, PR0GRAMADO2029, P0A2030, PRESUPUESTO2030, PR0GRAMADO2030 FROM  (SELECT VES.ID_RESULTADO, VES.RED,VES.SPPRO$ID_PROGRAMA_PRESUPUESTO, VES.SPPRO$ID_PROGRAMA_PRESUPUESTO||'-'||VES.PROGRAMA PROGRAMA";
                            for (int i = 0; i < tabla.Rows.Count; i++)
                            {
                                sql = sql + " ,SCHE$SIPLAN20.FNC$POA(VES.SPRES$POM,VES.SPRES$INSTITUCION," + tabla.Rows[i]["SPOA$ANIO"] + ") P0A" + tabla.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$OBTIENEPRESUPUESTO(VES.SPPRO$ID_PROGRAMA_PRESUPUESTO," + tabla.Rows[i]["SPOA$ID_POA"] + ", " + tabla.Rows[i]["SPOA$ANIO"] + ",VES.SPRES$POM, VES.SPRES$INSTITUCION) PRESUPUESTO" + tabla.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$OBTIENEPROGRAMADO(VES.SPRES$POM, " + tabla.Rows[i]["SPOA$ID_POA"] + ", " + tabla.Rows[i]["SPOA$ANIO"] + ", VES.SPRES$INSTITUCION, VES.SPPRO$ID_PROGRAMA_PRESUPUESTO) PR0GRAMADO" + tabla.Rows[i]["SPOA$ANIO"];
                            }

                            sql = sql + " ,VES.SPPSUB$PROPIETARIO";
                            sql = sql + " FROM SCHE$SIPLAN20.SPPSVT$RED_INSTITUCIONAL VES WHERE VES.SPRES$POM = " + Session["pom"] + " AND VES.SPRES$INSTITUCION = " + Session["insto"] + ") GROUP BY  SPPRO$ID_PROGRAMA_PRESUPUESTO,PROGRAMA, P0A2026, PRESUPUESTO2026,  PR0GRAMADO2026, P0A2027, PRESUPUESTO2027, PR0GRAMADO2027, P0A2028, PRESUPUESTO2028, PR0GRAMADO2028, P0A2029, PRESUPUESTO2029, PR0GRAMADO2029, P0A2030, PRESUPUESTO2030, PR0GRAMADO2030 ORDER BY  SPPRO$ID_PROGRAMA_PRESUPUESTO ASC";
                            lblTipodeProduccion.Text = "PRODUCTOS VINCULADOS A RED";
                        }


                    }

                    //fin 2026-2030
                    //NUEVA PGG

                }

                else
                {
                    /* sql = "SELECT P.SPPRO$POM, P.SPPRO$INSTO,(SELECT SPPRO$ID_PRODUCTO FROM SCHE$SIPLAN20.SP20$PRODUCTO WHERE SPPRO$RESTRICTIVA = 'N' AND SPPRO$DESCRIPCION = P.SPPRO$DESCRIPCION AND SPPRO$PRESUPUESTO = P.SPPRO$PRESUPUESTO AND SPPRO$POM = "+ Session["SPPO$ID_POM"] + " AND SPPRO$INSTO = "+ Session["SPPO$ID_INSTITUCION"] + ") IDPRODUCTO ";
                     sql = sql + ",P.SPPRO$ID_PRODUCTO, P.SPPRO$DESCRIPCION, P.SPPRO$PRESUPUESTO, M.SPPMFS$ID_POA,(SELECT SPOA$ID_POA FROM SCHE$SIPLAN20.SP20$POA WHERE SPOA$RESTRICTIVA = 'N' AND SPOA$ID_POM = " + Session["SPPO$ID_POM"] + " AND SPOA$ID_INSTITUCION = "+ Session["SPPO$ID_INSTITUCION"] +" AND SPOA$ANIO = M.SPPMFS$ANIO) POA ";
                     sql = sql + ",M.SPPMFS$ANIO, M.SPPMFS$META, M.SPPMF$TPROGRA, M.SPPMF$TIPO FROM SCHE$SIPLAN20.SP20$PRODUCTO P INNER JOIN SCHE$SIPLAN20.SP20$PROGRAMACION_MFPRODUCTO M ON M.SPPMFS$ID_PRODUCTO = P.SPPRO$ID_PRODUCTO AND M.SPPMFS$RESTRICTIVA = 'N' AND P.SPPRO$RESTRICTIVA = 'N' INNER JOIN SCHE$SIPLAN20.SP20$POM POM ON P.SPPRO$POM = POM.SPPO$ID_POM AND P.SPPRO$INSTO = POM.SPPO$ID_INSTITUCION AND P.SPPRO$RESTRICTIVA = 'N' AND POM.SPPO$RESTRICTIVA = 'N' ";
                     sql = sql +" INNER JOIN SCHE$SIPLAN20.SP20$PERIODO PE ON PE.SPP$ID_PERIODO = POM.SPPO$ID_PERIODO AND PE.SPP$RESTRICTIVA = 'N' AND POM.SPPO$RESTRICTIVA = 'N' WHERE P.SPPRO$RESTRICTIVA = 'N' AND PE.SPP$ORDEN = "+ (Convert.ToInt32(Session["SPP$ORDEN"]) -1)+" AND POM.SPPO$ID_INSTITUCION = "+ Session["SPPO$ID_INSTITUCION"] + " AND M.SPPMFS$ANIO IN(SELECT SPOA$ANIO FROM SCHE$SIPLAN20.SP20$POA WHERE SPOA$ID_POM = "+ Session["SPPO$ID_POM"] + " AND SPOA$ID_INSTITUCION = "+ Session["SPPO$ID_INSTITUCION"] + " AND SPOA$RESTRICTIVA = 'N')  ORDER BY P.SPPRO$ID_PRODUCTO, M.SPPMFS$ANIO ASC";*/

                    sql = @"SELECT SPPRO$POM
,SPPRO$INSTO
,(SELECT SPPRO$ID_PRODUCTO FROM SCHE$SIPLAN20.SP20$PRODUCTO WHERE SPPRO$RESTRICTIVA = 'N' AND SPPRO$DESCRIPCION = PRODUCTO AND SPPRO$PRESUPUESTO = PRESUPUESTO AND SPPRO$ID_RESULTADO = CODRESULTADO  AND SPPRO$POM = " + Session["SPPO$ID_POM"] + @" AND SPPRO$INSTO = " + Session["SPPO$ID_INSTITUCION"] + @") IDPRODUCTO 
,SPPRO$ID_PRODUCTO
,PRODUCTO SPPRO$DESCRIPCION
,PRESUPUESTO SPPRO$PRESUPUESTO
,SPPMFS$ID_POA
,POA
,SPPMFS$ANIO
,SPPMFS$META
,SPPMF$TPROGRA
,SPPMF$TIPO 
FROM 
(SELECT SPPRO$POM
,SPPRO$INSTO
,CASE WHEN RESULTADO IS NULL THEN (SELECT SPRES$ID_RESULTADO FROM SCHE$SIPLAN20.SP20$RESULTADOS WHERE SPRES$COD_ESTRATEGICO = CODIGO AND SPRES$POM = " + Session["SPPO$ID_POM"] + @" AND SPRES$INSTITUCION = " + Session["SPPO$ID_INSTITUCION"] + @" AND SPRES$RESTRICTIVA = 'N') ELSE (SELECT SPRES$ID_RESULTADO FROM SCHE$SIPLAN20.SP20$RESULTADOS WHERE SPRES$DESCRIPCION = RESULTADO AND SPRES$POM = " + Session["SPPO$ID_POM"] + @" AND SPRES$INSTITUCION = " + Session["SPPO$ID_INSTITUCION"] + @" AND SPRES$RESTRICTIVA = 'N') END AS CODRESULTADO
,SPPRO$ID_PRODUCTO
,SPPRO$DESCRIPCION PRODUCTO
,SPPRO$PRESUPUESTO PRESUPUESTO
,SPPMFS$ID_POA
,POA 
,SPPMFS$ANIO
,SPPMFS$META
,SPPMF$TPROGRA
,SPPMF$TIPO
FROM
(SELECT P.SPPRO$POM
,P.SPPRO$INSTO
,P.SPPRO$ID_RESULTADO
,(SELECT SPRES$DESCRIPCION FROM SCHE$SIPLAN20.SP20$RESULTADOS WHERE SPRES$ID_RESULTADO = P.SPPRO$ID_RESULTADO AND SPRES$POM = P.SPPRO$POM AND SPRES$INSTITUCION = P.SPPRO$INSTO AND SPRES$RESTRICTIVA = 'N') RESULTADO
,(SELECT SPRES$COD_ESTRATEGICO FROM SCHE$SIPLAN20.SP20$RESULTADOS WHERE SPRES$ID_RESULTADO = P.SPPRO$ID_RESULTADO AND SPRES$POM = P.SPPRO$POM AND SPRES$INSTITUCION = P.SPPRO$INSTO AND SPRES$RESTRICTIVA = 'N') CODIGO
,P.SPPRO$ID_PRODUCTO
,P.SPPRO$DESCRIPCION 
,P.SPPRO$PRESUPUESTO
,M.SPPMFS$ID_POA
,(SELECT SPOA$ID_POA FROM SCHE$SIPLAN20.SP20$POA WHERE SPOA$RESTRICTIVA = 'N' AND SPOA$ID_POM = " + Session["SPPO$ID_POM"] + @" AND SPOA$ID_INSTITUCION = " + Session["SPPO$ID_INSTITUCION"] + @" AND SPOA$ANIO = M.SPPMFS$ANIO) POA 
,M.SPPMFS$ANIO
, M.SPPMFS$META
, M.SPPMF$TPROGRA
, M.SPPMF$TIPO FROM 
SCHE$SIPLAN20.SP20$PRODUCTO P 
INNER JOIN SCHE$SIPLAN20.SP20$PROGRAMACION_MFPRODUCTO M ON M.SPPMFS$ID_PRODUCTO = P.SPPRO$ID_PRODUCTO AND M.SPPMFS$RESTRICTIVA = 'N' AND P.SPPRO$RESTRICTIVA = 'N' 
INNER JOIN SCHE$SIPLAN20.SP20$POM POM ON P.SPPRO$POM = POM.SPPO$ID_POM AND P.SPPRO$INSTO = POM.SPPO$ID_INSTITUCION AND P.SPPRO$RESTRICTIVA = 'N' AND POM.SPPO$RESTRICTIVA = 'N'  
INNER JOIN SCHE$SIPLAN20.SP20$PERIODO PE ON PE.SPP$ID_PERIODO = POM.SPPO$ID_PERIODO AND PE.SPP$RESTRICTIVA = 'N' AND POM.SPPO$RESTRICTIVA = 'N' WHERE P.SPPRO$RESTRICTIVA = 'N' AND PE.SPP$ORDEN = " + (Convert.ToInt32(Session["SPP$ORDEN"]) - 1) + @" AND POM.SPPO$ID_INSTITUCION = " + Session["SPPO$ID_INSTITUCION"] + @" 
AND M.SPPMFS$ANIO IN(SELECT SPOA$ANIO FROM SCHE$SIPLAN20.SP20$POA WHERE SPOA$ID_POM = " + Session["SPPO$ID_POM"] + " AND SPOA$ID_INSTITUCION = " + Session["SPPO$ID_INSTITUCION"] + " AND SPOA$RESTRICTIVA = 'N')  ORDER BY P.SPPRO$ID_PRODUCTO, M.SPPMFS$ANIO ASC))";
                    estado = dao.consulta(sql);

                    if (estado == 1)
                        productos = dao.tabla;

                    if (productos.Rows.Count > 0)
                    {
                        for (int i = 0; i < productos.Rows.Count; i++)
                        {
                            if (productos.Rows[i]["IDPRODUCTO"] != DBNull.Value)
                                estado = insertaMetaFisica(Convert.ToInt32(productos.Rows[i]["IDPRODUCTO"]), Convert.ToInt32(productos.Rows[i]["POA"]), Convert.ToInt32(productos.Rows[i]["SPPMFS$ANIO"]), Convert.ToDouble(productos.Rows[i]["SPPMFS$META"]));
                            if (estado == 0)
                                break;
                        }



                        if (estado == 1)
                        {
                            /*sql = "SELECT P.SPPRO$ID_PRODUCTO,(SELECT SPPRO$ID_PRODUCTO FROM SCHE$SIPLAN20.SP20$PRODUCTO WHERE SPPRO$RESTRICTIVA = 'N' AND SPPRO$DESCRIPCION = P.SPPRO$DESCRIPCION AND SPPRO$PRESUPUESTO = P.SPPRO$PRESUPUESTO AND SPPRO$POM = " + Session["SPPO$ID_POM"] + " AND SPPRO$INSTO = " + Session["SPPO$ID_INSTITUCION"] + ") IDPRODUCTO, P.SPPRO$DESCRIPCION, P.SPPRO$PRESUPUESTO, P.SPPRO$POM, P.SPPRO$INSTO,S.SPPSUB$ID_SUBPRODUCTO, S.SPPSUB$DESCRIPCION, S.SPPSUB$SNIP, (SELECT SPOA$ID_POA FROM SCHE$SIPLAN20.SP20$POA WHERE SPOA$RESTRICTIVA = 'N' AND SPOA$ID_POM = " + Session["SPPO$ID_POM"] + " AND SPOA$ID_INSTITUCION = " + Session["SPPO$ID_INSTITUCION"] + " AND SPOA$ANIO = PFF.SPPMFS$ANIO) POA, PFF.SPPMFS$ANIO, PFF.SPPMFS$META, PFF.SPPMFS$VIGENTE, PFF.SPPMFS$TPROGRA, PFF.SPPMFS$TIPO_PROGRAMACION ";
                            sql = sql+" FROM SCHE$SIPLAN20.SP20$PRODUCTO P INNER JOIN SCHE$SIPLAN20.SP20$SUB_PRODUCTO S ON P.SPPRO$ID_PRODUCTO = S.SPPSUB$ID_PRODUCTO AND P.SPPRO$RESTRICTIVA = 'N' AND S.SPPSUB$RESTRICTIVA = 'N' INNER JOIN SCHE$SIPLAN20.SP20$POM POM ON P.SPPRO$POM = POM.SPPO$ID_POM AND P.SPPRO$INSTO = POM.SPPO$ID_INSTITUCION AND P.SPPRO$RESTRICTIVA = 'N' AND POM.SPPO$RESTRICTIVA = 'N' INNER JOIN SCHE$SIPLAN20.SP20$PERIODO PE ON POM.SPPO$ID_PERIODO = PE.SPP$ID_PERIODO AND POM.SPPO$RESTRICTIVA = 'N' AND PE.SPP$RESTRICTIVA = 'N' INNER JOIN SCHE$SIPLAN20.SP20$POA POA ON POA.SPOA$ID_POM = POM.SPPO$ID_POM AND POA.SPOA$ID_INSTITUCION = POM.SPPO$ID_INSTITUCION INNER JOIN SCHE$SIPLAN20.SP20$PROGRAMETA_FI_FIN_SUB PFF ON POA.SPOA$ID_POA = PFF.SPPMFS$ID_POA AND POA.SPOA$ANIO = PFF.SPPMFS$ANIO AND PFF.SPPMFS$ID_SUBPRODUCTO = S.SPPSUB$ID_SUBPRODUCTO AND PFF.SPPMFS$RESTRICTIVA = 'N' AND S.SPPSUB$RESTRICTIVA = 'N' ";
                            sql = sql+" WHERE POM.SPPO$ID_INSTITUCION = "+ Session["SPPO$ID_INSTITUCION"] + " AND PE.SPP$ORDEN = "+(Convert.ToInt32(Session["SPP$ORDEN"]) -1)+" AND POA.SPOA$ANIO BETWEEN "+ Session["INICIO"] + " AND "+ Session["FINAL"];*/
                            sql = @"SELECT 
SPPRO$ID_PRODUCTO
,(SELECT SPPRO$ID_PRODUCTO FROM SCHE$SIPLAN20.SP20$PRODUCTO WHERE SPPRO$RESTRICTIVA = 'N' AND SPPRO$DESCRIPCION = PRODUCTO AND SPPRO$PRESUPUESTO = PRESUPUESTO AND SPPRO$ID_RESULTADO = CODRESULTADO AND SPPRO$POM = " + Session["SPPO$ID_POM"] + @" AND SPPRO$INSTO = " + Session["SPPO$ID_INSTITUCION"] + @") IDPRODUCTO
,PRODUCTO SPPRO$DESCRIPCION
,PRESUPUESTO SPPRO$PRESUPUESTO
,SPPRO$POM
,SPPRO$INSTO
,SPPSUB$ID_SUBPRODUCTO
,SPPSUB$DESCRIPCION
,SPPSUB$SNIP
,POA
,SPPMFS$ANIO
,SPPMFS$META
,SPPMFS$VIGENTE
,SPPMFS$TPROGRA
,SPPMFS$TIPO_PROGRAMACION
FROM 
(SELECT 
SPPRO$ID_PRODUCTO
,CASE WHEN RESULTADO IS NULL THEN (SELECT SPRES$ID_RESULTADO FROM SCHE$SIPLAN20.SP20$RESULTADOS WHERE SPRES$COD_ESTRATEGICO = CODIGO AND SPRES$POM = " + Session["SPPO$ID_POM"] + @" AND SPRES$INSTITUCION = " + Session["SPPO$ID_INSTITUCION"] + " AND SPRES$RESTRICTIVA = 'N') ELSE (SELECT SPRES$ID_RESULTADO FROM SCHE$SIPLAN20.SP20$RESULTADOS WHERE SPRES$DESCRIPCION = RESULTADO AND SPRES$POM = " + Session["SPPO$ID_POM"] + @" AND SPRES$INSTITUCION = " + Session["SPPO$ID_INSTITUCION"] + @"  AND SPRES$RESTRICTIVA = 'N') END AS CODRESULTADO
,SPPRO$DESCRIPCION PRODUCTO
,SPPRO$PRESUPUESTO PRESUPUESTO
,SPPRO$POM
,SPPRO$INSTO
,SPPSUB$ID_SUBPRODUCTO
,SPPSUB$DESCRIPCION
,SPPSUB$SNIP
,POA
,SPPMFS$ANIO
,SPPMFS$META
,SPPMFS$VIGENTE
,SPPMFS$TPROGRA
,SPPMFS$TIPO_PROGRAMACION
FROM 

(SELECT P.SPPRO$ID_PRODUCTO
,(SELECT SPRES$DESCRIPCION FROM SCHE$SIPLAN20.SP20$RESULTADOS WHERE SPRES$ID_RESULTADO = P.SPPRO$ID_RESULTADO AND SPRES$POM = P.SPPRO$POM AND SPRES$INSTITUCION = P.SPPRO$INSTO AND SPRES$RESTRICTIVA = 'N') RESULTADO
,(SELECT SPRES$COD_ESTRATEGICO FROM SCHE$SIPLAN20.SP20$RESULTADOS WHERE SPRES$ID_RESULTADO = P.SPPRO$ID_RESULTADO AND SPRES$POM = P.SPPRO$POM AND SPRES$INSTITUCION = P.SPPRO$INSTO AND SPRES$RESTRICTIVA = 'N') CODIGO
 ,P.SPPRO$DESCRIPCION
 ,P.SPPRO$PRESUPUESTO
 ,P.SPPRO$POM
 ,P.SPPRO$INSTO
 ,S.SPPSUB$ID_SUBPRODUCTO
 ,S.SPPSUB$DESCRIPCION
 ,S.SPPSUB$SNIP
 ,(SELECT SPOA$ID_POA FROM SCHE$SIPLAN20.SP20$POA WHERE SPOA$RESTRICTIVA = 'N' AND SPOA$ID_POM = " + Session["SPPO$ID_POM"] + @" AND SPOA$ID_INSTITUCION = " + Session["SPPO$ID_INSTITUCION"] + @" AND SPOA$ANIO = PFF.SPPMFS$ANIO) POA
 ,PFF.SPPMFS$ANIO
 ,PFF.SPPMFS$META
 ,PFF.SPPMFS$VIGENTE
 ,PFF.SPPMFS$TPROGRA
 ,PFF.SPPMFS$TIPO_PROGRAMACION  FROM SCHE$SIPLAN20.SP20$PRODUCTO P 
 INNER JOIN SCHE$SIPLAN20.SP20$SUB_PRODUCTO S ON P.SPPRO$ID_PRODUCTO = S.SPPSUB$ID_PRODUCTO AND P.SPPRO$RESTRICTIVA = 'N' AND S.SPPSUB$RESTRICTIVA = 'N' 
 INNER JOIN SCHE$SIPLAN20.SP20$POM POM ON P.SPPRO$POM = POM.SPPO$ID_POM AND P.SPPRO$INSTO = POM.SPPO$ID_INSTITUCION AND P.SPPRO$RESTRICTIVA = 'N' AND POM.SPPO$RESTRICTIVA = 'N' 
 INNER JOIN SCHE$SIPLAN20.SP20$PERIODO PE ON POM.SPPO$ID_PERIODO = PE.SPP$ID_PERIODO AND POM.SPPO$RESTRICTIVA = 'N' AND PE.SPP$RESTRICTIVA = 'N' 
 INNER JOIN SCHE$SIPLAN20.SP20$POA POA ON POA.SPOA$ID_POM = POM.SPPO$ID_POM AND POA.SPOA$ID_INSTITUCION = POM.SPPO$ID_INSTITUCION 
 INNER JOIN SCHE$SIPLAN20.SP20$PROGRAMETA_FI_FIN_SUB PFF ON POA.SPOA$ID_POA = PFF.SPPMFS$ID_POA AND POA.SPOA$ANIO = PFF.SPPMFS$ANIO AND PFF.SPPMFS$ID_SUBPRODUCTO = S.SPPSUB$ID_SUBPRODUCTO AND PFF.SPPMFS$RESTRICTIVA = 'N' AND S.SPPSUB$RESTRICTIVA = 'N'  
 WHERE POM.SPPO$ID_INSTITUCION = " + Session["SPPO$ID_INSTITUCION"] + " AND PE.SPP$ORDEN = " + (Convert.ToInt32(Session["SPP$ORDEN"]) - 1) + " AND POA.SPOA$ANIO BETWEEN " + Session["INICIO"] + " AND " + Session["FINAL"] + "))";
                            estado = dao.consulta(sql);
                            if (estado == 1)
                                productos = dao.tabla;

                            if (productos.Rows.Count > 0)
                            {
                                for (int i = 0; i < productos.Rows.Count; i++)
                                {
                                    sql = "SELECT DISTINCT SPPSUB$ID_SUBPRODUCTO IDSUB FROM SCHE$SIPLAN20.SP20$SUB_PRODUCTO WHERE ";
                                    if (productos.Rows[i]["SPPSUB$DESCRIPCION"] == DBNull.Value)
                                        sql = sql + " SPPSUB$SNIP = " + productos.Rows[i]["SPPSUB$SNIP"];
                                    else
                                        sql = sql + " SPPSUB$DESCRIPCION = '" + productos.Rows[i]["SPPSUB$DESCRIPCION"] + "'";

                                    sql = sql + " AND SPPSUB$ID_PRODUCTO = " + productos.Rows[i]["IDPRODUCTO"] + " AND SPPSUB$RESTRICTIVA = 'N'";
                                    estado = dao.consulta(sql);
                                    if (estado == 1)
                                        subproductos = dao.tabla;
                                    if (subproductos.Rows.Count > 0)
                                    {

                                        if (Convert.ToInt32(productos.Rows[i]["SPPMFS$TIPO_PROGRAMACION"]) == 0)
                                            estado = insertaFisicasub(Convert.ToInt32(subproductos.Rows[0]["IDSUB"].ToString()), Convert.ToInt32(productos.Rows[i]["POA"]), Convert.ToInt32(productos.Rows[i]["SPPMFS$ANIO"]), Convert.ToDouble(productos.Rows[i]["SPPMFS$META"]));
                                        if (estado == 0)
                                            break;
                                        if (Convert.ToInt32(productos.Rows[i]["SPPMFS$TIPO_PROGRAMACION"]) == 1)
                                            estado = insertaFinanciera(Convert.ToInt32(subproductos.Rows[0]["IDSUB"].ToString()), Convert.ToInt32(productos.Rows[i]["POA"]), Convert.ToInt32(productos.Rows[i]["SPPMFS$ANIO"]), Convert.ToDouble(productos.Rows[i]["SPPMFS$META"]));
                                        if (estado == 0)
                                            break;



                                    }

                                }
                            }



                        }


                        if (Convert.ToInt32(Session["periodo"]) == 20)
                        {
                            if (tipo == 0)
                            {
                                sql = "SELECT SPPRO$ID_PROGRAMA_PRESUPUESTO, PROGRAMA, NULL P0A2020, NULL PRESUPUESTO2020, NULL PR0GRAMADO2020, P0A2021, PRESUPUESTO2021, PR0GRAMADO2021, P0A2022, PRESUPUESTO2022, PR0GRAMADO2022, P0A2023, PRESUPUESTO2023, PR0GRAMADO2023, P0A2024, PRESUPUESTO2024, PR0GRAMADO2024, P0A2025, PRESUPUESTO2025,  PR0GRAMADO2025, NULL P0A2026, NULL PRESUPUESTO2026,  NULL PR0GRAMADO2026, NULL P0A2027, NULL PRESUPUESTO2027,  NULL PR0GRAMADO2027 FROM (SELECT VES.ID_RESULTADO, VES.RESULTADO,VES.SPPRO$ID_PROGRAMA_PRESUPUESTO, VES.SPPRO$ID_PROGRAMA_PRESUPUESTO||'-'||VES.PROGRAMA PROGRAMA";
                                for (int i = 0; i < tabla.Rows.Count; i++)
                                {
                                    sql = sql + " ,SCHE$SIPLAN20.FNC$POA(VES.SPRES$POM,VES.SPRES$INSTITUCION," + tabla.Rows[i]["SPOA$ANIO"] + ") P0A" + tabla.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$OBTIENEPRESUPUESTO(VES.SPPRO$ID_PROGRAMA_PRESUPUESTO," + tabla.Rows[i]["SPOA$ID_POA"] + ", " + tabla.Rows[i]["SPOA$ANIO"] + ",VES.SPRES$POM, VES.SPRES$INSTITUCION) PRESUPUESTO" + tabla.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$OBTIENEPROGRAMADO(VES.SPRES$POM, " + tabla.Rows[i]["SPOA$ID_POA"] + ", " + tabla.Rows[i]["SPOA$ANIO"] + ", VES.SPRES$INSTITUCION, VES.SPPRO$ID_PROGRAMA_PRESUPUESTO) PR0GRAMADO" + tabla.Rows[i]["SPOA$ANIO"];
                                }


                                sql = sql + " ,VES.SPPSUB$PROPIETARIO ";
                                sql = sql + " FROM SCHE$SIPLAN20.SPPVST$SUBESTRATEGICOV2 VES WHERE VES.SPRES$POM = " + Session["pom"] + " AND VES.SPRES$INSTITUCION = " + Session["insto"] + ") GROUP BY  SPPRO$ID_PROGRAMA_PRESUPUESTO, PROGRAMA,P0A2021, PRESUPUESTO2021, PR0GRAMADO2021, P0A2022, PRESUPUESTO2022, PR0GRAMADO2022, P0A2023, PRESUPUESTO2023, PR0GRAMADO2023, P0A2024, PRESUPUESTO2024, PR0GRAMADO2024, P0A2025, PRESUPUESTO2025, PR0GRAMADO2025 ORDER BY  SPPRO$ID_PROGRAMA_PRESUPUESTO ASC";
                                lblTipodeProduccion.Text = "PRODUCTOS INSTITUCIONALES";
                            }


                            else if (tipo == 1)
                            {
                                sql = "SELECT  SPPRO$ID_PROGRAMA_PRESUPUESTO, PROGRAMA, NULL P0A2020, NULL PRESUPUESTO2020, NULL PR0GRAMADO2020, P0A2021, PRESUPUESTO2021, PR0GRAMADO2021, P0A2022, PRESUPUESTO2022, PR0GRAMADO2022, P0A2023, PRESUPUESTO2023, PR0GRAMADO2023, P0A2024, PRESUPUESTO2024, PR0GRAMADO2024, P0A2025, PRESUPUESTO2025,  PR0GRAMADO2025, NULL P0A2026,  NULL PRESUPUESTO2026,  NULL PR0GRAMADO2026, NULL P0A2027, NULL PRESUPUESTO2027,  NULL PR0GRAMADO2027 FROM  (SELECT VES.ID_RESULTADO, VES.RED,VES.SPPRO$ID_PROGRAMA_PRESUPUESTO, VES.SPPRO$ID_PROGRAMA_PRESUPUESTO||'-'||VES.PROGRAMA PROGRAMA";
                                for (int i = 0; i < tabla.Rows.Count; i++)
                                {
                                    sql = sql + " ,SCHE$SIPLAN20.FNC$POA(VES.SPRES$POM,VES.SPRES$INSTITUCION," + tabla.Rows[i]["SPOA$ANIO"] + ") P0A" + tabla.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$OBTIENEPRESUPUESTO(VES.SPPRO$ID_PROGRAMA_PRESUPUESTO," + tabla.Rows[i]["SPOA$ID_POA"] + ", " + tabla.Rows[i]["SPOA$ANIO"] + ",VES.SPRES$POM, VES.SPRES$INSTITUCION) PRESUPUESTO" + tabla.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$OBTIENEPROGRAMADO(VES.SPRES$POM, " + tabla.Rows[i]["SPOA$ID_POA"] + ", " + tabla.Rows[i]["SPOA$ANIO"] + ", VES.SPRES$INSTITUCION, VES.SPPRO$ID_PROGRAMA_PRESUPUESTO) PR0GRAMADO" + tabla.Rows[i]["SPOA$ANIO"];
                                }

                                sql = sql + " ,VES.SPPSUB$PROPIETARIO";
                                sql = sql + " FROM SCHE$SIPLAN20.SPPSVT$RED_INSTITUCIONAL VES WHERE VES.SPRES$POM = " + Session["pom"] + " AND VES.SPRES$INSTITUCION = " + Session["insto"] + ") GROUP BY  SPPRO$ID_PROGRAMA_PRESUPUESTO,PROGRAMA, P0A2021, PRESUPUESTO2021, PR0GRAMADO2021, P0A2022, PRESUPUESTO2022, PR0GRAMADO2022, P0A2023, PRESUPUESTO2023, PR0GRAMADO2023, P0A2024, PRESUPUESTO2024, PR0GRAMADO2024, P0A2025, PRESUPUESTO2025,  PR0GRAMADO2025 ORDER BY  SPPRO$ID_PROGRAMA_PRESUPUESTO ASC";
                                lblTipodeProduccion.Text = "PRODUCTOS VINCULADOS A RED";
                            }

                        }

                        else if (Convert.ToInt32(Session["periodo"]) == 21)
                        {
                            if (tipo == 0)
                            {
                                sql = "SELECT SPPRO$ID_PROGRAMA_PRESUPUESTO, PROGRAMA, NULL P0A2020, NULL PRESUPUESTO2020, NULL PR0GRAMADO2020, NULL P0A2021, NULL PRESUPUESTO2021, NULL PR0GRAMADO2021, P0A2022, PRESUPUESTO2022, PR0GRAMADO2022, P0A2023, PRESUPUESTO2023, PR0GRAMADO2023, P0A2024, PRESUPUESTO2024, PR0GRAMADO2024, P0A2025, PRESUPUESTO2025,  PR0GRAMADO2025, P0A2026, PRESUPUESTO2026,  PR0GRAMADO2026, NULL P0A2027, NULL PRESUPUESTO2027,  NULL PR0GRAMADO2027 FROM (SELECT VES.ID_RESULTADO, VES.RESULTADO,VES.SPPRO$ID_PROGRAMA_PRESUPUESTO, VES.SPPRO$ID_PROGRAMA_PRESUPUESTO||'-'||VES.PROGRAMA PROGRAMA";
                                for (int i = 0; i < tabla.Rows.Count; i++)
                                {
                                    sql = sql + " ,SCHE$SIPLAN20.FNC$POA(VES.SPRES$POM,VES.SPRES$INSTITUCION," + tabla.Rows[i]["SPOA$ANIO"] + ") P0A" + tabla.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$OBTIENEPRESUPUESTO(VES.SPPRO$ID_PROGRAMA_PRESUPUESTO," + tabla.Rows[i]["SPOA$ID_POA"] + ", " + tabla.Rows[i]["SPOA$ANIO"] + ",VES.SPRES$POM, VES.SPRES$INSTITUCION) PRESUPUESTO" + tabla.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$OBTIENEPROGRAMADO(VES.SPRES$POM, " + tabla.Rows[i]["SPOA$ID_POA"] + ", " + tabla.Rows[i]["SPOA$ANIO"] + ", VES.SPRES$INSTITUCION, VES.SPPRO$ID_PROGRAMA_PRESUPUESTO) PR0GRAMADO" + tabla.Rows[i]["SPOA$ANIO"];
                                }


                                sql = sql + " ,VES.SPPSUB$PROPIETARIO ";
                                sql = sql + " FROM SCHE$SIPLAN20.SPPVST$SUBESTRATEGICOV2 VES WHERE VES.SPRES$POM = " + Session["pom"] + " AND VES.SPRES$INSTITUCION = " + Session["insto"] + ") GROUP BY  SPPRO$ID_PROGRAMA_PRESUPUESTO, PROGRAMA,P0A2022, PRESUPUESTO2022, PR0GRAMADO2022, P0A2023, PRESUPUESTO2023, PR0GRAMADO2023, P0A2024, PRESUPUESTO2024, PR0GRAMADO2024, P0A2025, PRESUPUESTO2025, PR0GRAMADO2025,  P0A2026, PRESUPUESTO2026, PR0GRAMADO2026 ORDER BY  SPPRO$ID_PROGRAMA_PRESUPUESTO ASC";
                                lblTipodeProduccion.Text = "PRODUCTOS INSTITUCIONALES";
                            }


                            else if (tipo == 1)
                            {
                                sql = "SELECT  SPPRO$ID_PROGRAMA_PRESUPUESTO, PROGRAMA, NULL P0A2020, NULL PRESUPUESTO2020, NULL PR0GRAMADO2020, NULL P0A2021, NULL PRESUPUESTO2021, NULL PR0GRAMADO2021, P0A2022, PRESUPUESTO2022, PR0GRAMADO2022, P0A2023, PRESUPUESTO2023, PR0GRAMADO2023, P0A2024, PRESUPUESTO2024, PR0GRAMADO2024, P0A2025, PRESUPUESTO2025,  PR0GRAMADO2025, P0A2026, PRESUPUESTO2026,  PR0GRAMADO2026, NULL P0A2027, NULL PRESUPUESTO2027,  NULL PR0GRAMADO2027 FROM  (SELECT VES.ID_RESULTADO, VES.RED,VES.SPPRO$ID_PROGRAMA_PRESUPUESTO, VES.SPPRO$ID_PROGRAMA_PRESUPUESTO||'-'||VES.PROGRAMA PROGRAMA";
                                for (int i = 0; i < tabla.Rows.Count; i++)
                                {
                                    sql = sql + " ,SCHE$SIPLAN20.FNC$POA(VES.SPRES$POM,VES.SPRES$INSTITUCION," + tabla.Rows[i]["SPOA$ANIO"] + ") P0A" + tabla.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$OBTIENEPRESUPUESTO(VES.SPPRO$ID_PROGRAMA_PRESUPUESTO," + tabla.Rows[i]["SPOA$ID_POA"] + ", " + tabla.Rows[i]["SPOA$ANIO"] + ",VES.SPRES$POM, VES.SPRES$INSTITUCION) PRESUPUESTO" + tabla.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$OBTIENEPROGRAMADO(VES.SPRES$POM, " + tabla.Rows[i]["SPOA$ID_POA"] + ", " + tabla.Rows[i]["SPOA$ANIO"] + ", VES.SPRES$INSTITUCION, VES.SPPRO$ID_PROGRAMA_PRESUPUESTO) PR0GRAMADO" + tabla.Rows[i]["SPOA$ANIO"];
                                }

                                sql = sql + " ,VES.SPPSUB$PROPIETARIO";
                                sql = sql + " FROM SCHE$SIPLAN20.SPPSVT$RED_INSTITUCIONAL VES WHERE VES.SPRES$POM = " + Session["pom"] + " AND VES.SPRES$INSTITUCION = " + Session["insto"] + ") GROUP BY  SPPRO$ID_PROGRAMA_PRESUPUESTO,PROGRAMA, P0A2022, PRESUPUESTO2022, PR0GRAMADO2022, P0A2023, PRESUPUESTO2023, PR0GRAMADO2023, P0A2024, PRESUPUESTO2024, PR0GRAMADO2024, P0A2025, PRESUPUESTO2025,  PR0GRAMADO2025,  P0A2026, PRESUPUESTO2026, PR0GRAMADO2026 ORDER BY  SPPRO$ID_PROGRAMA_PRESUPUESTO ASC";
                                lblTipodeProduccion.Text = "PRODUCTOS VINCULADOS A RED";
                            }


                        }



                        else if (Convert.ToInt32(Session["periodo"]) == 22)
                        {
                            if (tipo == 0)
                            {
                                sql = "SELECT SPPRO$ID_PROGRAMA_PRESUPUESTO, PROGRAMA, NULL P0A2020, NULL PRESUPUESTO2020, NULL PR0GRAMADO2020, NULL P0A2021, NULL PRESUPUESTO2021, NULL PR0GRAMADO2021, NULL P0A2022, NULL PRESUPUESTO2022, NULL PR0GRAMADO2022, P0A2023, PRESUPUESTO2023, PR0GRAMADO2023, P0A2024, PRESUPUESTO2024, PR0GRAMADO2024, P0A2025, PRESUPUESTO2025,  PR0GRAMADO2025, P0A2026, PRESUPUESTO2026,  PR0GRAMADO2026, P0A2027,  PRESUPUESTO2027,   PR0GRAMADO2027 FROM (SELECT VES.ID_RESULTADO, VES.RESULTADO,VES.SPPRO$ID_PROGRAMA_PRESUPUESTO, VES.SPPRO$ID_PROGRAMA_PRESUPUESTO||'-'||VES.PROGRAMA PROGRAMA";
                                for (int i = 0; i < tabla.Rows.Count; i++)
                                {
                                    sql = sql + " ,SCHE$SIPLAN20.FNC$POA(VES.SPRES$POM,VES.SPRES$INSTITUCION," + tabla.Rows[i]["SPOA$ANIO"] + ") P0A" + tabla.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$OBTIENEPRESUPUESTO(VES.SPPRO$ID_PROGRAMA_PRESUPUESTO," + tabla.Rows[i]["SPOA$ID_POA"] + ", " + tabla.Rows[i]["SPOA$ANIO"] + ",VES.SPRES$POM, VES.SPRES$INSTITUCION) PRESUPUESTO" + tabla.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$OBTIENEPROGRAMADO(VES.SPRES$POM, " + tabla.Rows[i]["SPOA$ID_POA"] + ", " + tabla.Rows[i]["SPOA$ANIO"] + ", VES.SPRES$INSTITUCION, VES.SPPRO$ID_PROGRAMA_PRESUPUESTO) PR0GRAMADO" + tabla.Rows[i]["SPOA$ANIO"];
                                }


                                sql = sql + " ,VES.SPPSUB$PROPIETARIO ";
                                sql = sql + " FROM SCHE$SIPLAN20.SPPVST$SUBESTRATEGICOV2 VES WHERE VES.SPRES$POM = " + Session["pom"] + " AND VES.SPRES$INSTITUCION = " + Session["insto"] + ") GROUP BY  SPPRO$ID_PROGRAMA_PRESUPUESTO, PROGRAMA, P0A2023, PRESUPUESTO2023, PR0GRAMADO2023, P0A2024, PRESUPUESTO2024, PR0GRAMADO2024, P0A2025, PRESUPUESTO2025, PR0GRAMADO2025,  P0A2026, PRESUPUESTO2026, PR0GRAMADO2026,P0A2027, PRESUPUESTO2027, PR0GRAMADO2027 ORDER BY  SPPRO$ID_PROGRAMA_PRESUPUESTO ASC";
                                lblTipodeProduccion.Text = "PRODUCTOS INSTITUCIONALES";
                            }


                            else if (tipo == 1)
                            {
                                sql = "SELECT  SPPRO$ID_PROGRAMA_PRESUPUESTO, PROGRAMA, NULL P0A2020, NULL PRESUPUESTO2020, NULL PR0GRAMADO2020, NULL P0A2021, NULL PRESUPUESTO2021, NULL PR0GRAMADO2021, NULL P0A2022, NULL PRESUPUESTO2022, NULL PR0GRAMADO2022, P0A2023, PRESUPUESTO2023, PR0GRAMADO2023, P0A2024, PRESUPUESTO2024, PR0GRAMADO2024, P0A2025, PRESUPUESTO2025,  PR0GRAMADO2025, P0A2026, PRESUPUESTO2026,  PR0GRAMADO2026, P0A2027, PRESUPUESTO2027,  PR0GRAMADO2027 FROM  (SELECT VES.ID_RESULTADO, VES.RED,VES.SPPRO$ID_PROGRAMA_PRESUPUESTO, VES.SPPRO$ID_PROGRAMA_PRESUPUESTO||'-'||VES.PROGRAMA PROGRAMA";
                                for (int i = 0; i < tabla.Rows.Count; i++)
                                {
                                    sql = sql + " ,SCHE$SIPLAN20.FNC$POA(VES.SPRES$POM,VES.SPRES$INSTITUCION," + tabla.Rows[i]["SPOA$ANIO"] + ") P0A" + tabla.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$OBTIENEPRESUPUESTO(VES.SPPRO$ID_PROGRAMA_PRESUPUESTO," + tabla.Rows[i]["SPOA$ID_POA"] + ", " + tabla.Rows[i]["SPOA$ANIO"] + ",VES.SPRES$POM, VES.SPRES$INSTITUCION) PRESUPUESTO" + tabla.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$OBTIENEPROGRAMADO(VES.SPRES$POM, " + tabla.Rows[i]["SPOA$ID_POA"] + ", " + tabla.Rows[i]["SPOA$ANIO"] + ", VES.SPRES$INSTITUCION, VES.SPPRO$ID_PROGRAMA_PRESUPUESTO) PR0GRAMADO" + tabla.Rows[i]["SPOA$ANIO"];
                                }

                                sql = sql + " ,VES.SPPSUB$PROPIETARIO";
                                sql = sql + " FROM SCHE$SIPLAN20.SPPSVT$RED_INSTITUCIONAL VES WHERE VES.SPRES$POM = " + Session["pom"] + " AND VES.SPRES$INSTITUCION = " + Session["insto"] + ") GROUP BY  SPPRO$ID_PROGRAMA_PRESUPUESTO,PROGRAMA, P0A2022, PRESUPUESTO2022, PR0GRAMADO2022, P0A2023, PRESUPUESTO2023, PR0GRAMADO2023, P0A2024, PRESUPUESTO2024, PR0GRAMADO2024, P0A2025, PRESUPUESTO2025,  PR0GRAMADO2025,  P0A2026, PRESUPUESTO2026, PR0GRAMADO2026, P0A2027, PRESUPUESTO2027, PR0GRAMADO2027 ORDER BY  SPPRO$ID_PROGRAMA_PRESUPUESTO ASC";
                                lblTipodeProduccion.Text = "PRODUCTOS VINCULADOS A RED";
                            }


                        }




                        else if (Convert.ToInt32(Session["periodo"]) == 23)
                        {
                            if (tipo == 0)
                            {
                                sql = "SELECT SPPRO$ID_PROGRAMA_PRESUPUESTO, PROGRAMA, NULL P0A2020, NULL PRESUPUESTO2020, NULL PR0GRAMADO2020, NULL P0A2021, NULL PRESUPUESTO2021, NULL PR0GRAMADO2021, NULL P0A2022, NULL PRESUPUESTO2022, NULL PR0GRAMADO2022, NULL P0A2023, NULL PRESUPUESTO2023, NULL PR0GRAMADO2023, P0A2024, PRESUPUESTO2024, PR0GRAMADO2024, P0A2025, PRESUPUESTO2025,  PR0GRAMADO2025, P0A2026, PRESUPUESTO2026,  PR0GRAMADO2026, P0A2027,  PRESUPUESTO2027, PR0GRAMADO2027, P0A2028, PRESUPUESTO2028, PR0GRAMADO2028 FROM (SELECT VES.ID_RESULTADO, VES.RESULTADO,VES.SPPRO$ID_PROGRAMA_PRESUPUESTO, VES.SPPRO$ID_PROGRAMA_PRESUPUESTO||'-'||VES.PROGRAMA PROGRAMA";
                                for (int i = 0; i < tabla.Rows.Count; i++)
                                {
                                    sql = sql + " ,SCHE$SIPLAN20.FNC$POA(VES.SPRES$POM,VES.SPRES$INSTITUCION," + tabla.Rows[i]["SPOA$ANIO"] + ") P0A" + tabla.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$OBTIENEPRESUPUESTO(VES.SPPRO$ID_PROGRAMA_PRESUPUESTO," + tabla.Rows[i]["SPOA$ID_POA"] + ", " + tabla.Rows[i]["SPOA$ANIO"] + ",VES.SPRES$POM, VES.SPRES$INSTITUCION) PRESUPUESTO" + tabla.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$OBTIENEPROGRAMADO(VES.SPRES$POM, " + tabla.Rows[i]["SPOA$ID_POA"] + ", " + tabla.Rows[i]["SPOA$ANIO"] + ", VES.SPRES$INSTITUCION, VES.SPPRO$ID_PROGRAMA_PRESUPUESTO) PR0GRAMADO" + tabla.Rows[i]["SPOA$ANIO"];
                                }


                                sql = sql + " ,VES.SPPSUB$PROPIETARIO ";
                                sql = sql + " FROM SCHE$SIPLAN20.SPPVST$SUBESTRATEGICOV2 VES WHERE VES.SPRES$POM = " + Session["pom"] + " AND VES.SPRES$INSTITUCION = " + Session["insto"] + ") GROUP BY  SPPRO$ID_PROGRAMA_PRESUPUESTO, PROGRAMA, P0A2024, PRESUPUESTO2024, PR0GRAMADO2024, P0A2025, PRESUPUESTO2025, PR0GRAMADO2025,  P0A2026, PRESUPUESTO2026, PR0GRAMADO2026,P0A2027, PRESUPUESTO2027, PR0GRAMADO2027, P0A2028, PRESUPUESTO2028, PR0GRAMADO2028 ORDER BY  SPPRO$ID_PROGRAMA_PRESUPUESTO ASC";
                                lblTipodeProduccion.Text = "PRODUCTOS INSTITUCIONALES";
                            }


                            else if (tipo == 1)
                            {
                                sql = "SELECT  SPPRO$ID_PROGRAMA_PRESUPUESTO, PROGRAMA, NULL P0A2020, NULL PRESUPUESTO2020, NULL PR0GRAMADO2020, NULL P0A2021, NULL PRESUPUESTO2021, NULL PR0GRAMADO2021, NULL P0A2022, NULL PRESUPUESTO2022, NULL PR0GRAMADO2022, NULL P0A2023, NULL PRESUPUESTO2023, NULL PR0GRAMADO2023, P0A2024, PRESUPUESTO2024, PR0GRAMADO2024, P0A2025, PRESUPUESTO2025,  PR0GRAMADO2025, P0A2026, PRESUPUESTO2026,  PR0GRAMADO2026, P0A2027, PRESUPUESTO2027,  PR0GRAMADO2027  , P0A2028, PRESUPUESTO2028, PR0GRAMADO2028 FROM  (SELECT VES.ID_RESULTADO, VES.RED,VES.SPPRO$ID_PROGRAMA_PRESUPUESTO, VES.SPPRO$ID_PROGRAMA_PRESUPUESTO||'-'||VES.PROGRAMA PROGRAMA";
                                for (int i = 0; i < tabla.Rows.Count; i++)
                                {
                                    sql = sql + " ,SCHE$SIPLAN20.FNC$POA(VES.SPRES$POM,VES.SPRES$INSTITUCION," + tabla.Rows[i]["SPOA$ANIO"] + ") P0A" + tabla.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$OBTIENEPRESUPUESTO(VES.SPPRO$ID_PROGRAMA_PRESUPUESTO," + tabla.Rows[i]["SPOA$ID_POA"] + ", " + tabla.Rows[i]["SPOA$ANIO"] + ",VES.SPRES$POM, VES.SPRES$INSTITUCION) PRESUPUESTO" + tabla.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$OBTIENEPROGRAMADO(VES.SPRES$POM, " + tabla.Rows[i]["SPOA$ID_POA"] + ", " + tabla.Rows[i]["SPOA$ANIO"] + ", VES.SPRES$INSTITUCION, VES.SPPRO$ID_PROGRAMA_PRESUPUESTO) PR0GRAMADO" + tabla.Rows[i]["SPOA$ANIO"];
                                }

                                sql = sql + " ,VES.SPPSUB$PROPIETARIO";
                                sql = sql + " FROM SCHE$SIPLAN20.SPPSVT$RED_INSTITUCIONAL VES WHERE VES.SPRES$POM = " + Session["pom"] + " AND VES.SPRES$INSTITUCION = " + Session["insto"] + ") GROUP BY  SPPRO$ID_PROGRAMA_PRESUPUESTO,PROGRAMA ,P0A2024, PRESUPUESTO2024, PR0GRAMADO2024, P0A2025, PRESUPUESTO2025,  PR0GRAMADO2025,  P0A2026, PRESUPUESTO2026, PR0GRAMADO2026, P0A2027, PRESUPUESTO2027, PR0GRAMADO2027, P0A2028, PRESUPUESTO2028, PR0GRAMADO2028 ORDER BY  SPPRO$ID_PROGRAMA_PRESUPUESTO ASC";
                                lblTipodeProduccion.Text = "PRODUCTOS VINCULADOS A RED";
                            }


                        }


                        //NUEVA PGG
                        else if (Convert.ToInt32(Session["periodo"]) == 24)
                        {
                            if (tipo == 0)
                            {
                                sql = "SELECT SPPRO$ID_PROGRAMA_PRESUPUESTO, PROGRAMA, NULL P0A2020, NULL PRESUPUESTO2020, NULL PR0GRAMADO2020, NULL P0A2021, NULL PRESUPUESTO2021, NULL PR0GRAMADO2021, NULL P0A2022, NULL PRESUPUESTO2022, NULL PR0GRAMADO2022, NULL P0A2023, NULL PRESUPUESTO2023, NULL PR0GRAMADO2023, NULL P0A2024, NULL PRESUPUESTO2024, NULL PR0GRAMADO2024, P0A2025, PRESUPUESTO2025,  PR0GRAMADO2025, P0A2026, PRESUPUESTO2026,  PR0GRAMADO2026, P0A2027,  PRESUPUESTO2027, PR0GRAMADO2027, P0A2028, PRESUPUESTO2028, PR0GRAMADO2028, P0A2029, PRESUPUESTO2029, PR0GRAMADO2029  FROM (SELECT VES.ID_RESULTADO, VES.RESULTADO,VES.SPPRO$ID_PROGRAMA_PRESUPUESTO, VES.SPPRO$ID_PROGRAMA_PRESUPUESTO||'-'||VES.PROGRAMA PROGRAMA";
                                for (int i = 0; i < tabla.Rows.Count; i++)
                                {
                                    sql = sql + " ,SCHE$SIPLAN20.FNC$POA(VES.SPRES$POM,VES.SPRES$INSTITUCION," + tabla.Rows[i]["SPOA$ANIO"] + ") P0A" + tabla.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$OBTIENEPRESUPUESTO(VES.SPPRO$ID_PROGRAMA_PRESUPUESTO," + tabla.Rows[i]["SPOA$ID_POA"] + ", " + tabla.Rows[i]["SPOA$ANIO"] + ",VES.SPRES$POM, VES.SPRES$INSTITUCION) PRESUPUESTO" + tabla.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$OBTIENEPROGRAMADO(VES.SPRES$POM, " + tabla.Rows[i]["SPOA$ID_POA"] + ", " + tabla.Rows[i]["SPOA$ANIO"] + ", VES.SPRES$INSTITUCION, VES.SPPRO$ID_PROGRAMA_PRESUPUESTO) PR0GRAMADO" + tabla.Rows[i]["SPOA$ANIO"];
                                }


                                sql = sql + " ,VES.SPPSUB$PROPIETARIO ";
                                sql = sql + " FROM SCHE$SIPLAN20.SPPVST$SUBESTRATEGICOV2 VES WHERE VES.SPRES$POM = " + Session["pom"] + " AND VES.SPRES$INSTITUCION = " + Session["insto"] + ") GROUP BY  SPPRO$ID_PROGRAMA_PRESUPUESTO, PROGRAMA, P0A2025, PRESUPUESTO2025, PR0GRAMADO2025,  P0A2026, PRESUPUESTO2026, PR0GRAMADO2026,P0A2027, PRESUPUESTO2027, PR0GRAMADO2027, P0A2028, PRESUPUESTO2028, PR0GRAMADO2028, P0A2029, PRESUPUESTO2029, PR0GRAMADO2029 ORDER BY  SPPRO$ID_PROGRAMA_PRESUPUESTO ASC";
                                lblTipodeProduccion.Text = "PRODUCTOS INSTITUCIONALES";
                            }


                            else if (tipo == 1)
                            {
                                sql = "SELECT  SPPRO$ID_PROGRAMA_PRESUPUESTO, PROGRAMA, NULL P0A2020, NULL PRESUPUESTO2020, NULL PR0GRAMADO2020, NULL P0A2021, NULL PRESUPUESTO2021, NULL PR0GRAMADO2021, NULL P0A2022, NULL PRESUPUESTO2022, NULL PR0GRAMADO2022, NULL P0A2023, NULL PRESUPUESTO2023, NULL PR0GRAMADO2023, NULL P0A2024, NULL PRESUPUESTO2024, NULL PR0GRAMADO2024, P0A2025, PRESUPUESTO2025,  PR0GRAMADO2025, P0A2026, PRESUPUESTO2026,  PR0GRAMADO2026, P0A2027, PRESUPUESTO2027,  PR0GRAMADO2027  , P0A2028, PRESUPUESTO2028, PR0GRAMADO2028, P0A2029, PRESUPUESTO2029, PR0GRAMADO2029 FROM  (SELECT VES.ID_RESULTADO, VES.RED,VES.SPPRO$ID_PROGRAMA_PRESUPUESTO, VES.SPPRO$ID_PROGRAMA_PRESUPUESTO||'-'||VES.PROGRAMA PROGRAMA";
                                for (int i = 0; i < tabla.Rows.Count; i++)
                                {
                                    sql = sql + " ,SCHE$SIPLAN20.FNC$POA(VES.SPRES$POM,VES.SPRES$INSTITUCION," + tabla.Rows[i]["SPOA$ANIO"] + ") P0A" + tabla.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$OBTIENEPRESUPUESTO(VES.SPPRO$ID_PROGRAMA_PRESUPUESTO," + tabla.Rows[i]["SPOA$ID_POA"] + ", " + tabla.Rows[i]["SPOA$ANIO"] + ",VES.SPRES$POM, VES.SPRES$INSTITUCION) PRESUPUESTO" + tabla.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$OBTIENEPROGRAMADO(VES.SPRES$POM, " + tabla.Rows[i]["SPOA$ID_POA"] + ", " + tabla.Rows[i]["SPOA$ANIO"] + ", VES.SPRES$INSTITUCION, VES.SPPRO$ID_PROGRAMA_PRESUPUESTO) PR0GRAMADO" + tabla.Rows[i]["SPOA$ANIO"];
                                }

                                sql = sql + " ,VES.SPPSUB$PROPIETARIO";
                                sql = sql + " FROM SCHE$SIPLAN20.SPPSVT$RED_INSTITUCIONAL VES WHERE VES.SPRES$POM = " + Session["pom"] + " AND VES.SPRES$INSTITUCION = " + Session["insto"] + ") GROUP BY  SPPRO$ID_PROGRAMA_PRESUPUESTO,PROGRAMA, P0A2025, PRESUPUESTO2025,  PR0GRAMADO2025,  P0A2026, PRESUPUESTO2026, PR0GRAMADO2026, P0A2027, PRESUPUESTO2027, PR0GRAMADO2027, P0A2028, PRESUPUESTO2028, PR0GRAMADO2028, P0A2029, PRESUPUESTO2029, PR0GRAMADO2029 ORDER BY  SPPRO$ID_PROGRAMA_PRESUPUESTO ASC";
                                lblTipodeProduccion.Text = "PRODUCTOS VINCULADOS A RED";
                            }


                        }

                        //NUEVA PGG

                        //INICIO 2026-2030 
                        else if (Convert.ToInt32(Session["periodo"]) == 25)
                        {
                            if (tipo == 0)
                            {
                                sql = "SELECT SPPRO$ID_PROGRAMA_PRESUPUESTO, PROGRAMA, NULL P0A2020, NULL PRESUPUESTO2020, NULL PR0GRAMADO2020, NULL P0A2021, NULL PRESUPUESTO2021, NULL PR0GRAMADO2021, NULL P0A2022, NULL PRESUPUESTO2022, NULL PR0GRAMADO2022, NULL P0A2023, NULL PRESUPUESTO2023, NULL PR0GRAMADO2023, NULL P0A2024, NULL PRESUPUESTO2024, NULL PR0GRAMADO2024, NULL P0A2025, NULL PRESUPUESTO2025,  NULL PR0GRAMADO2025, P0A2026, PRESUPUESTO2026,  PR0GRAMADO2026, P0A2027,  PRESUPUESTO2027, PR0GRAMADO2027, P0A2028, PRESUPUESTO2028, PR0GRAMADO2028, P0A2029, PRESUPUESTO2029, PR0GRAMADO2029,   P0A2030, PRESUPUESTO2030, PR0GRAMADO2030  FROM (SELECT VES.ID_RESULTADO, VES.RESULTADO,VES.SPPRO$ID_PROGRAMA_PRESUPUESTO, VES.SPPRO$ID_PROGRAMA_PRESUPUESTO||'-'||VES.PROGRAMA PROGRAMA";
                                for (int i = 0; i < tabla.Rows.Count; i++)
                                {
                                    sql = sql + " ,SCHE$SIPLAN20.FNC$POA(VES.SPRES$POM,VES.SPRES$INSTITUCION," + tabla.Rows[i]["SPOA$ANIO"] + ") P0A" + tabla.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$OBTIENEPRESUPUESTO(VES.SPPRO$ID_PROGRAMA_PRESUPUESTO," + tabla.Rows[i]["SPOA$ID_POA"] + ", " + tabla.Rows[i]["SPOA$ANIO"] + ",VES.SPRES$POM, VES.SPRES$INSTITUCION) PRESUPUESTO" + tabla.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$OBTIENEPROGRAMADO(VES.SPRES$POM, " + tabla.Rows[i]["SPOA$ID_POA"] + ", " + tabla.Rows[i]["SPOA$ANIO"] + ", VES.SPRES$INSTITUCION, VES.SPPRO$ID_PROGRAMA_PRESUPUESTO) PR0GRAMADO" + tabla.Rows[i]["SPOA$ANIO"];
                                }


                                sql = sql + " ,VES.SPPSUB$PROPIETARIO ";
                                sql = sql + " FROM SCHE$SIPLAN20.SPPVST$SUBESTRATEGICOV2 VES WHERE VES.SPRES$POM = " + Session["pom"] + " AND VES.SPRES$INSTITUCION = " + Session["insto"] + ") GROUP BY  SPPRO$ID_PROGRAMA_PRESUPUESTO, PROGRAMA, P0A2026, PRESUPUESTO2026, PR0GRAMADO2026,P0A2027, PRESUPUESTO2027, PR0GRAMADO2027, P0A2028, PRESUPUESTO2028, PR0GRAMADO2028, P0A2029, PRESUPUESTO2029, PR0GRAMADO2029,  P0A2030, PRESUPUESTO2030, PR0GRAMADO2030 ORDER BY  SPPRO$ID_PROGRAMA_PRESUPUESTO ASC";
                                lblTipodeProduccion.Text = "PRODUCTOS INSTITUCIONALES";
                            }


                            else if (tipo == 1)
                            {
                                sql = "SELECT  SPPRO$ID_PROGRAMA_PRESUPUESTO, PROGRAMA, NULL P0A2020, NULL PRESUPUESTO2020, NULL PR0GRAMADO2020, NULL P0A2021, NULL PRESUPUESTO2021, NULL PR0GRAMADO2021, NULL P0A2022, NULL PRESUPUESTO2022, NULL PR0GRAMADO2022, NULL P0A2023, NULL PRESUPUESTO2023, NULL PR0GRAMADO2023, NULL P0A2024, NULL PRESUPUESTO2024, NULL PR0GRAMADO2024, NULL P0A2025, NULL PRESUPUESTO2025,  NULL PR0GRAMADO2025, P0A2026, PRESUPUESTO2026,  PR0GRAMADO2026, P0A2027, PRESUPUESTO2027,  PR0GRAMADO2027  , P0A2028, PRESUPUESTO2028, PR0GRAMADO2028, P0A2029, PRESUPUESTO2029, PR0GRAMADO2029,   P0A2030, PRESUPUESTO2030, PR0GRAMADO2030 FROM  (SELECT VES.ID_RESULTADO, VES.RED,VES.SPPRO$ID_PROGRAMA_PRESUPUESTO, VES.SPPRO$ID_PROGRAMA_PRESUPUESTO||'-'||VES.PROGRAMA PROGRAMA";
                                for (int i = 0; i < tabla.Rows.Count; i++)
                                {
                                    sql = sql + " ,SCHE$SIPLAN20.FNC$POA(VES.SPRES$POM,VES.SPRES$INSTITUCION," + tabla.Rows[i]["SPOA$ANIO"] + ") P0A" + tabla.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$OBTIENEPRESUPUESTO(VES.SPPRO$ID_PROGRAMA_PRESUPUESTO," + tabla.Rows[i]["SPOA$ID_POA"] + ", " + tabla.Rows[i]["SPOA$ANIO"] + ",VES.SPRES$POM, VES.SPRES$INSTITUCION) PRESUPUESTO" + tabla.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$OBTIENEPROGRAMADO(VES.SPRES$POM, " + tabla.Rows[i]["SPOA$ID_POA"] + ", " + tabla.Rows[i]["SPOA$ANIO"] + ", VES.SPRES$INSTITUCION, VES.SPPRO$ID_PROGRAMA_PRESUPUESTO) PR0GRAMADO" + tabla.Rows[i]["SPOA$ANIO"];
                                }

                                sql = sql + " ,VES.SPPSUB$PROPIETARIO";
                                sql = sql + " FROM SCHE$SIPLAN20.SPPSVT$RED_INSTITUCIONAL VES WHERE VES.SPRES$POM = " + Session["pom"] + " AND VES.SPRES$INSTITUCION = " + Session["insto"] + ") GROUP BY  SPPRO$ID_PROGRAMA_PRESUPUESTO,PROGRAMA, P0A2026, PRESUPUESTO2026, PR0GRAMADO2026, P0A2027, PRESUPUESTO2027, PR0GRAMADO2027, P0A2028, PRESUPUESTO2028, PR0GRAMADO2028, P0A2029, PRESUPUESTO2029, PR0GRAMADO2029, P0A2030, PRESUPUESTO2030, PR0GRAMADO2030 ORDER BY  SPPRO$ID_PROGRAMA_PRESUPUESTO ASC";
                                lblTipodeProduccion.Text = "PRODUCTOS VINCULADOS A RED";
                            }


                        }


                        //FIN 2026-2030 


                    }

                    else
                    {


                        if (Convert.ToInt32(Session["periodo"]) == 20)
                        {
                            if (tipo == 0)
                            {
                                sql = "SELECT SPPRO$ID_PROGRAMA_PRESUPUESTO, PROGRAMA, NULL P0A2020, NULL PRESUPUESTO2020, NULL PR0GRAMADO2020, P0A2021, PRESUPUESTO2021, PR0GRAMADO2021, P0A2022, PRESUPUESTO2022, PR0GRAMADO2022, P0A2023, PRESUPUESTO2023, PR0GRAMADO2023, P0A2024, PRESUPUESTO2024, PR0GRAMADO2024, P0A2025, PRESUPUESTO2025,  PR0GRAMADO2025, NULL P0A2026, NULL PRESUPUESTO2026,  NULL PR0GRAMADO2026, NULL P0A2027, NULL PRESUPUESTO2027,  NULL PR0GRAMADO2027 FROM (SELECT VES.ID_RESULTADO, VES.RESULTADO,VES.SPPRO$ID_PROGRAMA_PRESUPUESTO, VES.SPPRO$ID_PROGRAMA_PRESUPUESTO||'-'||VES.PROGRAMA PROGRAMA";
                                for (int i = 0; i < tabla.Rows.Count; i++)
                                {
                                    sql = sql + " ,SCHE$SIPLAN20.FNC$POA(VES.SPRES$POM,VES.SPRES$INSTITUCION," + tabla.Rows[i]["SPOA$ANIO"] + ") P0A" + tabla.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$OBTIENEPRESUPUESTO(VES.SPPRO$ID_PROGRAMA_PRESUPUESTO," + tabla.Rows[i]["SPOA$ID_POA"] + ", " + tabla.Rows[i]["SPOA$ANIO"] + ",VES.SPRES$POM, VES.SPRES$INSTITUCION) PRESUPUESTO" + tabla.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$OBTIENEPROGRAMADO(VES.SPRES$POM, " + tabla.Rows[i]["SPOA$ID_POA"] + ", " + tabla.Rows[i]["SPOA$ANIO"] + ", VES.SPRES$INSTITUCION, VES.SPPRO$ID_PROGRAMA_PRESUPUESTO) PR0GRAMADO" + tabla.Rows[i]["SPOA$ANIO"];
                                }


                                sql = sql + " ,VES.SPPSUB$PROPIETARIO ";
                                sql = sql + " FROM SCHE$SIPLAN20.SPPVST$SUBESTRATEGICOV2 VES WHERE VES.SPRES$POM = " + Session["pom"] + " AND VES.SPRES$INSTITUCION = " + Session["insto"] + ") GROUP BY  SPPRO$ID_PROGRAMA_PRESUPUESTO, PROGRAMA,P0A2021, PRESUPUESTO2021, PR0GRAMADO2021, P0A2022, PRESUPUESTO2022, PR0GRAMADO2022, P0A2023, PRESUPUESTO2023, PR0GRAMADO2023, P0A2024, PRESUPUESTO2024, PR0GRAMADO2024, P0A2025, PRESUPUESTO2025, PR0GRAMADO2025 ORDER BY  SPPRO$ID_PROGRAMA_PRESUPUESTO ASC";
                                lblTipodeProduccion.Text = "PRODUCTOS INSTITUCIONALES";
                            }


                            else if (tipo == 1)
                            {
                                sql = "SELECT  SPPRO$ID_PROGRAMA_PRESUPUESTO, PROGRAMA, NULL P0A2020, NULL PRESUPUESTO2020, NULL PR0GRAMADO2020, P0A2021, PRESUPUESTO2021, PR0GRAMADO2021, P0A2022, PRESUPUESTO2022, PR0GRAMADO2022, P0A2023, PRESUPUESTO2023, PR0GRAMADO2023, P0A2024, PRESUPUESTO2024, PR0GRAMADO2024, P0A2025, PRESUPUESTO2025,  PR0GRAMADO2025, NULL P0A2026, NULL PRESUPUESTO2026,  NULL PR0GRAMADO2026, NULL P0A2027, NULL PRESUPUESTO2027,  NULL PR0GRAMADO2027 FROM  (SELECT VES.ID_RESULTADO, VES.RED,VES.SPPRO$ID_PROGRAMA_PRESUPUESTO, VES.SPPRO$ID_PROGRAMA_PRESUPUESTO||'-'||VES.PROGRAMA PROGRAMA";
                                for (int i = 0; i < tabla.Rows.Count; i++)
                                {
                                    sql = sql + " ,SCHE$SIPLAN20.FNC$POA(VES.SPRES$POM,VES.SPRES$INSTITUCION," + tabla.Rows[i]["SPOA$ANIO"] + ") P0A" + tabla.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$OBTIENEPRESUPUESTO(VES.SPPRO$ID_PROGRAMA_PRESUPUESTO," + tabla.Rows[i]["SPOA$ID_POA"] + ", " + tabla.Rows[i]["SPOA$ANIO"] + ",VES.SPRES$POM, VES.SPRES$INSTITUCION) PRESUPUESTO" + tabla.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$OBTIENEPROGRAMADO(VES.SPRES$POM, " + tabla.Rows[i]["SPOA$ID_POA"] + ", " + tabla.Rows[i]["SPOA$ANIO"] + ", VES.SPRES$INSTITUCION, VES.SPPRO$ID_PROGRAMA_PRESUPUESTO) PR0GRAMADO" + tabla.Rows[i]["SPOA$ANIO"];
                                }

                                sql = sql + " ,VES.SPPSUB$PROPIETARIO";
                                sql = sql + " FROM SCHE$SIPLAN20.SPPSVT$RED_INSTITUCIONAL VES WHERE VES.SPRES$POM = " + Session["pom"] + " AND VES.SPRES$INSTITUCION = " + Session["insto"] + ") GROUP BY  SPPRO$ID_PROGRAMA_PRESUPUESTO,PROGRAMA, P0A2021, PRESUPUESTO2021, PR0GRAMADO2021, P0A2022, PRESUPUESTO2022, PR0GRAMADO2022, P0A2023, PRESUPUESTO2023, PR0GRAMADO2023, P0A2024, PRESUPUESTO2024, PR0GRAMADO2024, P0A2025, PRESUPUESTO2025,  PR0GRAMADO2025 ORDER BY  SPPRO$ID_PROGRAMA_PRESUPUESTO ASC";
                                lblTipodeProduccion.Text = "PRODUCTOS VINCULADOS A RED";
                            }
                        }

                        else if (Convert.ToInt32(Session["periodo"]) == 21)
                        {
                            if (tipo == 0)
                            {
                                sql = "SELECT SPPRO$ID_PROGRAMA_PRESUPUESTO, PROGRAMA, NULL P0A2020, NULL PRESUPUESTO2020, NULL PR0GRAMADO2020, NULL P0A2021, NULL PRESUPUESTO2021, NULL PR0GRAMADO2021, P0A2022, PRESUPUESTO2022, PR0GRAMADO2022, P0A2023, PRESUPUESTO2023, PR0GRAMADO2023, P0A2024, PRESUPUESTO2024, PR0GRAMADO2024, P0A2025, PRESUPUESTO2025,  PR0GRAMADO2025, P0A2026,  PRESUPUESTO2026,  PR0GRAMADO2026, NULL P0A2027, NULL PRESUPUESTO2027,  NULL PR0GRAMADO2027 FROM (SELECT VES.ID_RESULTADO, VES.RESULTADO,VES.SPPRO$ID_PROGRAMA_PRESUPUESTO, VES.SPPRO$ID_PROGRAMA_PRESUPUESTO||'-'||VES.PROGRAMA PROGRAMA";
                                for (int i = 0; i < tabla.Rows.Count; i++)
                                {
                                    sql = sql + " ,SCHE$SIPLAN20.FNC$POA(VES.SPRES$POM,VES.SPRES$INSTITUCION," + tabla.Rows[i]["SPOA$ANIO"] + ") P0A" + tabla.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$OBTIENEPRESUPUESTO(VES.SPPRO$ID_PROGRAMA_PRESUPUESTO," + tabla.Rows[i]["SPOA$ID_POA"] + ", " + tabla.Rows[i]["SPOA$ANIO"] + ",VES.SPRES$POM, VES.SPRES$INSTITUCION) PRESUPUESTO" + tabla.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$OBTIENEPROGRAMADO(VES.SPRES$POM, " + tabla.Rows[i]["SPOA$ID_POA"] + ", " + tabla.Rows[i]["SPOA$ANIO"] + ", VES.SPRES$INSTITUCION, VES.SPPRO$ID_PROGRAMA_PRESUPUESTO) PR0GRAMADO" + tabla.Rows[i]["SPOA$ANIO"];
                                }


                                sql = sql + " ,VES.SPPSUB$PROPIETARIO ";
                                sql = sql + " FROM SCHE$SIPLAN20.SPPVST$SUBESTRATEGICOV2 VES WHERE VES.SPRES$POM = " + Session["pom"] + " AND VES.SPRES$INSTITUCION = " + Session["insto"] + ") GROUP BY  SPPRO$ID_PROGRAMA_PRESUPUESTO, PROGRAMA,P0A2022, PRESUPUESTO2022, PR0GRAMADO2022, P0A2023, PRESUPUESTO2023, PR0GRAMADO2023, P0A2024, PRESUPUESTO2024, PR0GRAMADO2024, P0A2025, PRESUPUESTO2025, PR0GRAMADO2025, P0A2026, PRESUPUESTO2026, PR0GRAMADO2026  ORDER BY  SPPRO$ID_PROGRAMA_PRESUPUESTO ASC";
                                lblTipodeProduccion.Text = "PRODUCTOS INSTITUCIONALES";
                            }


                            else if (tipo == 1)
                            {
                                sql = "SELECT  SPPRO$ID_PROGRAMA_PRESUPUESTO, PROGRAMA, NULL P0A2020, NULL PRESUPUESTO2020, NULL PR0GRAMADO2020, NULL P0A2021, NULL PRESUPUESTO2021, NULL PR0GRAMADO2021, P0A2022, PRESUPUESTO2022, PR0GRAMADO2022, P0A2023, PRESUPUESTO2023, PR0GRAMADO2023, P0A2024, PRESUPUESTO2024, PR0GRAMADO2024, P0A2025, PRESUPUESTO2025,  PR0GRAMADO2025, P0A2026, PRESUPUESTO2026,  PR0GRAMADO2026, NULL P0A2027, NULL PRESUPUESTO2027,  NULL PR0GRAMADO2027 FROM  (SELECT VES.ID_RESULTADO, VES.RED,VES.SPPRO$ID_PROGRAMA_PRESUPUESTO, VES.SPPRO$ID_PROGRAMA_PRESUPUESTO||'-'||VES.PROGRAMA PROGRAMA";
                                for (int i = 0; i < tabla.Rows.Count; i++)
                                {
                                    sql = sql + " ,SCHE$SIPLAN20.FNC$POA(VES.SPRES$POM,VES.SPRES$INSTITUCION," + tabla.Rows[i]["SPOA$ANIO"] + ") P0A" + tabla.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$OBTIENEPRESUPUESTO(VES.SPPRO$ID_PROGRAMA_PRESUPUESTO," + tabla.Rows[i]["SPOA$ID_POA"] + ", " + tabla.Rows[i]["SPOA$ANIO"] + ",VES.SPRES$POM, VES.SPRES$INSTITUCION) PRESUPUESTO" + tabla.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$OBTIENEPROGRAMADO(VES.SPRES$POM, " + tabla.Rows[i]["SPOA$ID_POA"] + ", " + tabla.Rows[i]["SPOA$ANIO"] + ", VES.SPRES$INSTITUCION, VES.SPPRO$ID_PROGRAMA_PRESUPUESTO) PR0GRAMADO" + tabla.Rows[i]["SPOA$ANIO"];
                                }

                                sql = sql + " ,VES.SPPSUB$PROPIETARIO";
                                sql = sql + " FROM SCHE$SIPLAN20.SPPSVT$RED_INSTITUCIONAL VES WHERE VES.SPRES$POM = " + Session["pom"] + " AND VES.SPRES$INSTITUCION = " + Session["insto"] + ") GROUP BY  SPPRO$ID_PROGRAMA_PRESUPUESTO,PROGRAMA, P0A2022, PRESUPUESTO2022, PR0GRAMADO2022, P0A2023, PRESUPUESTO2023, PR0GRAMADO2023, P0A2024, PRESUPUESTO2024, PR0GRAMADO2024, P0A2025, PRESUPUESTO2025,  PR0GRAMADO2025, P0A2026, PRESUPUESTO2026, PR0GRAMADO2026  ORDER BY  SPPRO$ID_PROGRAMA_PRESUPUESTO ASC";
                                lblTipodeProduccion.Text = "PRODUCTOS VINCULADOS A RED";
                            }

                        }



                        else if (Convert.ToInt32(Session["periodo"]) == 22)
                        {
                            if (tipo == 0)
                            {
                                sql = "SELECT SPPRO$ID_PROGRAMA_PRESUPUESTO, PROGRAMA, NULL P0A2020, NULL PRESUPUESTO2020, NULL PR0GRAMADO2020, NULL P0A2021, NULL PRESUPUESTO2021, NULL PR0GRAMADO2021, NULL P0A2022, NULL PRESUPUESTO2022, NULL PR0GRAMADO2022, P0A2023, PRESUPUESTO2023, PR0GRAMADO2023, P0A2024, PRESUPUESTO2024, PR0GRAMADO2024, P0A2025, PRESUPUESTO2025,  PR0GRAMADO2025, P0A2026,  PRESUPUESTO2026,  PR0GRAMADO2026, P0A2027, PRESUPUESTO2027,  PR0GRAMADO2027 FROM (SELECT VES.ID_RESULTADO, VES.RESULTADO,VES.SPPRO$ID_PROGRAMA_PRESUPUESTO, VES.SPPRO$ID_PROGRAMA_PRESUPUESTO||'-'||VES.PROGRAMA PROGRAMA";
                                for (int i = 0; i < tabla.Rows.Count; i++)
                                {
                                    sql = sql + " ,SCHE$SIPLAN20.FNC$POA(VES.SPRES$POM,VES.SPRES$INSTITUCION," + tabla.Rows[i]["SPOA$ANIO"] + ") P0A" + tabla.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$OBTIENEPRESUPUESTO(VES.SPPRO$ID_PROGRAMA_PRESUPUESTO," + tabla.Rows[i]["SPOA$ID_POA"] + ", " + tabla.Rows[i]["SPOA$ANIO"] + ",VES.SPRES$POM, VES.SPRES$INSTITUCION) PRESUPUESTO" + tabla.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$OBTIENEPROGRAMADO(VES.SPRES$POM, " + tabla.Rows[i]["SPOA$ID_POA"] + ", " + tabla.Rows[i]["SPOA$ANIO"] + ", VES.SPRES$INSTITUCION, VES.SPPRO$ID_PROGRAMA_PRESUPUESTO) PR0GRAMADO" + tabla.Rows[i]["SPOA$ANIO"];
                                }


                                sql = sql + " ,VES.SPPSUB$PROPIETARIO ";
                                sql = sql + " FROM SCHE$SIPLAN20.SPPVST$SUBESTRATEGICOV2 VES WHERE VES.SPRES$POM = " + Session["pom"] + " AND VES.SPRES$INSTITUCION = " + Session["insto"] + ") GROUP BY  SPPRO$ID_PROGRAMA_PRESUPUESTO, PROGRAMA, P0A2023, PRESUPUESTO2023, PR0GRAMADO2023, P0A2024, PRESUPUESTO2024, PR0GRAMADO2024, P0A2025, PRESUPUESTO2025, PR0GRAMADO2025, P0A2026, PRESUPUESTO2026, PR0GRAMADO2026, P0A2027, PRESUPUESTO2027, PR0GRAMADO2027  ORDER BY  SPPRO$ID_PROGRAMA_PRESUPUESTO ASC";
                                lblTipodeProduccion.Text = "PRODUCTOS INSTITUCIONALES";
                            }


                            else if (tipo == 1)
                            {
                                sql = "SELECT  SPPRO$ID_PROGRAMA_PRESUPUESTO, PROGRAMA, NULL P0A2020, NULL PRESUPUESTO2020, NULL PR0GRAMADO2020, NULL P0A2021, NULL PRESUPUESTO2021, NULL PR0GRAMADO2021, NULL P0A2022, NULL PRESUPUESTO2022, NULL PR0GRAMADO2022, P0A2023, PRESUPUESTO2023, PR0GRAMADO2023, P0A2024, PRESUPUESTO2024, PR0GRAMADO2024, P0A2025, PRESUPUESTO2025,  PR0GRAMADO2025,  P0A2026, PRESUPUESTO2026,  PR0GRAMADO2026,  P0A2027, PRESUPUESTO2027,  PR0GRAMADO2027 FROM  (SELECT VES.ID_RESULTADO, VES.RED,VES.SPPRO$ID_PROGRAMA_PRESUPUESTO, VES.SPPRO$ID_PROGRAMA_PRESUPUESTO||'-'||VES.PROGRAMA PROGRAMA";
                                for (int i = 0; i < tabla.Rows.Count; i++)
                                {
                                    sql = sql + " ,SCHE$SIPLAN20.FNC$POA(VES.SPRES$POM,VES.SPRES$INSTITUCION," + tabla.Rows[i]["SPOA$ANIO"] + ") P0A" + tabla.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$OBTIENEPRESUPUESTO(VES.SPPRO$ID_PROGRAMA_PRESUPUESTO," + tabla.Rows[i]["SPOA$ID_POA"] + ", " + tabla.Rows[i]["SPOA$ANIO"] + ",VES.SPRES$POM, VES.SPRES$INSTITUCION) PRESUPUESTO" + tabla.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$OBTIENEPROGRAMADO(VES.SPRES$POM, " + tabla.Rows[i]["SPOA$ID_POA"] + ", " + tabla.Rows[i]["SPOA$ANIO"] + ", VES.SPRES$INSTITUCION, VES.SPPRO$ID_PROGRAMA_PRESUPUESTO) PR0GRAMADO" + tabla.Rows[i]["SPOA$ANIO"];
                                }

                                sql = sql + " ,VES.SPPSUB$PROPIETARIO";
                                sql = sql + " FROM SCHE$SIPLAN20.SPPSVT$RED_INSTITUCIONAL VES WHERE VES.SPRES$POM = " + Session["pom"] + " AND VES.SPRES$INSTITUCION = " + Session["insto"] + ") GROUP BY  SPPRO$ID_PROGRAMA_PRESUPUESTO,PROGRAMA,  P0A2023, PRESUPUESTO2023, PR0GRAMADO2023, P0A2024, PRESUPUESTO2024, PR0GRAMADO2024, P0A2025, PRESUPUESTO2025,  PR0GRAMADO2025, P0A2026, PRESUPUESTO2026, PR0GRAMADO2026, P0A2027, PRESUPUESTO2027, PR0GRAMADO2027  ORDER BY  SPPRO$ID_PROGRAMA_PRESUPUESTO ASC";
                                lblTipodeProduccion.Text = "PRODUCTOS VINCULADOS A RED";
                            }

                        }



                        else if (Convert.ToInt32(Session["periodo"]) == 23)
                        {
                            if (tipo == 0)
                            {
                                sql = "SELECT SPPRO$ID_PROGRAMA_PRESUPUESTO, PROGRAMA, NULL P0A2020, NULL PRESUPUESTO2020, NULL PR0GRAMADO2020, NULL P0A2021, NULL PRESUPUESTO2021, NULL PR0GRAMADO2021, NULL P0A2022, NULL PRESUPUESTO2022, NULL PR0GRAMADO2022, NULL P0A2023, NULL PRESUPUESTO2023, NULL PR0GRAMADO2023, P0A2024, PRESUPUESTO2024, PR0GRAMADO2024, P0A2025, PRESUPUESTO2025,  PR0GRAMADO2025, P0A2026,  PRESUPUESTO2026,  PR0GRAMADO2026, P0A2027, PRESUPUESTO2027,  PR0GRAMADO2027,    P0A2028, PRESUPUESTO2028, PR0GRAMADO2028  FROM (SELECT VES.ID_RESULTADO, VES.RESULTADO,VES.SPPRO$ID_PROGRAMA_PRESUPUESTO, VES.SPPRO$ID_PROGRAMA_PRESUPUESTO||'-'||VES.PROGRAMA PROGRAMA";
                                for (int i = 0; i < tabla.Rows.Count; i++)
                                {
                                    sql = sql + " ,SCHE$SIPLAN20.FNC$POA(VES.SPRES$POM,VES.SPRES$INSTITUCION," + tabla.Rows[i]["SPOA$ANIO"] + ") P0A" + tabla.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$OBTIENEPRESUPUESTO(VES.SPPRO$ID_PROGRAMA_PRESUPUESTO," + tabla.Rows[i]["SPOA$ID_POA"] + ", " + tabla.Rows[i]["SPOA$ANIO"] + ",VES.SPRES$POM, VES.SPRES$INSTITUCION) PRESUPUESTO" + tabla.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$OBTIENEPROGRAMADO(VES.SPRES$POM, " + tabla.Rows[i]["SPOA$ID_POA"] + ", " + tabla.Rows[i]["SPOA$ANIO"] + ", VES.SPRES$INSTITUCION, VES.SPPRO$ID_PROGRAMA_PRESUPUESTO) PR0GRAMADO" + tabla.Rows[i]["SPOA$ANIO"];
                                }


                                sql = sql + " ,VES.SPPSUB$PROPIETARIO ";
                                sql = sql + " FROM SCHE$SIPLAN20.SPPVST$SUBESTRATEGICOV2 VES WHERE VES.SPRES$POM = " + Session["pom"] + " AND VES.SPRES$INSTITUCION = " + Session["insto"] + ") GROUP BY  SPPRO$ID_PROGRAMA_PRESUPUESTO, PROGRAMA,P0A2024, PRESUPUESTO2024, PR0GRAMADO2024, P0A2025, PRESUPUESTO2025, PR0GRAMADO2025, P0A2026, PRESUPUESTO2026, PR0GRAMADO2026, P0A2027, PRESUPUESTO2027, PR0GRAMADO2027,  P0A2028, PRESUPUESTO2028, PR0GRAMADO2028 ORDER BY  SPPRO$ID_PROGRAMA_PRESUPUESTO ASC";
                                lblTipodeProduccion.Text = "PRODUCTOS INSTITUCIONALES";
                            }


                            else if (tipo == 1)
                            {
                                sql = "SELECT  SPPRO$ID_PROGRAMA_PRESUPUESTO, PROGRAMA, NULL P0A2020, NULL PRESUPUESTO2020, NULL PR0GRAMADO2020, NULL P0A2021, NULL PRESUPUESTO2021, NULL PR0GRAMADO2021, NULL P0A2022, NULL PRESUPUESTO2022, NULL PR0GRAMADO2022, NULL P0A2023, NULL PRESUPUESTO2023, NULL PR0GRAMADO2023, P0A2024, PRESUPUESTO2024, PR0GRAMADO2024, P0A2025, PRESUPUESTO2025,  PR0GRAMADO2025,  P0A2026, PRESUPUESTO2026,  PR0GRAMADO2026,  P0A2027, PRESUPUESTO2027,  PR0GRAMADO2027,   P0A2028, PRESUPUESTO2028, PR0GRAMADO2028 FROM  (SELECT VES.ID_RESULTADO, VES.RED,VES.SPPRO$ID_PROGRAMA_PRESUPUESTO, VES.SPPRO$ID_PROGRAMA_PRESUPUESTO||'-'||VES.PROGRAMA PROGRAMA";
                                for (int i = 0; i < tabla.Rows.Count; i++)
                                {
                                    sql = sql + " ,SCHE$SIPLAN20.FNC$POA(VES.SPRES$POM,VES.SPRES$INSTITUCION," + tabla.Rows[i]["SPOA$ANIO"] + ") P0A" + tabla.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$OBTIENEPRESUPUESTO(VES.SPPRO$ID_PROGRAMA_PRESUPUESTO," + tabla.Rows[i]["SPOA$ID_POA"] + ", " + tabla.Rows[i]["SPOA$ANIO"] + ",VES.SPRES$POM, VES.SPRES$INSTITUCION) PRESUPUESTO" + tabla.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$OBTIENEPROGRAMADO(VES.SPRES$POM, " + tabla.Rows[i]["SPOA$ID_POA"] + ", " + tabla.Rows[i]["SPOA$ANIO"] + ", VES.SPRES$INSTITUCION, VES.SPPRO$ID_PROGRAMA_PRESUPUESTO) PR0GRAMADO" + tabla.Rows[i]["SPOA$ANIO"];
                                }

                                sql = sql + " ,VES.SPPSUB$PROPIETARIO";
                                sql = sql + " FROM SCHE$SIPLAN20.SPPSVT$RED_INSTITUCIONAL VES WHERE VES.SPRES$POM = " + Session["pom"] + " AND VES.SPRES$INSTITUCION = " + Session["insto"] + ") GROUP BY  SPPRO$ID_PROGRAMA_PRESUPUESTO,PROGRAMA, P0A2024, PRESUPUESTO2024, PR0GRAMADO2024, P0A2025, PRESUPUESTO2025,  PR0GRAMADO2025, P0A2026, PRESUPUESTO2026, PR0GRAMADO2026, P0A2027, PRESUPUESTO2027, PR0GRAMADO2027,    P0A2028, PRESUPUESTO2028, PR0GRAMADO2028 ORDER BY  SPPRO$ID_PROGRAMA_PRESUPUESTO ASC";
                                lblTipodeProduccion.Text = "PRODUCTOS VINCULADOS A RED";
                            }

                        }

                        //NUEVA PGG

                        else if (Convert.ToInt32(Session["periodo"]) == 24)
                        {
                            if (tipo == 0)
                            {
                                sql = "SELECT SPPRO$ID_PROGRAMA_PRESUPUESTO, PROGRAMA, NULL P0A2020, NULL PRESUPUESTO2020, NULL PR0GRAMADO2020, NULL P0A2021, NULL PRESUPUESTO2021, NULL PR0GRAMADO2021, NULL P0A2022, NULL PRESUPUESTO2022, NULL PR0GRAMADO2022, NULL P0A2023, NULL PRESUPUESTO2023, NULL PR0GRAMADO2023, NULL P0A2024, NULL PRESUPUESTO2024, NULL PR0GRAMADO2024, P0A2025, PRESUPUESTO2025,  PR0GRAMADO2025, P0A2026,  PRESUPUESTO2026,  PR0GRAMADO2026, P0A2027, PRESUPUESTO2027,  PR0GRAMADO2027, P0A2028, PRESUPUESTO2028, PR0GRAMADO2028, P0A2029, PRESUPUESTO2029, PR0GRAMADO2029  FROM (SELECT VES.ID_RESULTADO, VES.RESULTADO,VES.SPPRO$ID_PROGRAMA_PRESUPUESTO, VES.SPPRO$ID_PROGRAMA_PRESUPUESTO||'-'||VES.PROGRAMA PROGRAMA";
                                for (int i = 0; i < tabla.Rows.Count; i++)
                                {
                                    sql = sql + " ,SCHE$SIPLAN20.FNC$POA(VES.SPRES$POM,VES.SPRES$INSTITUCION," + tabla.Rows[i]["SPOA$ANIO"] + ") P0A" + tabla.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$OBTIENEPRESUPUESTO(VES.SPPRO$ID_PROGRAMA_PRESUPUESTO," + tabla.Rows[i]["SPOA$ID_POA"] + ", " + tabla.Rows[i]["SPOA$ANIO"] + ",VES.SPRES$POM, VES.SPRES$INSTITUCION) PRESUPUESTO" + tabla.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$OBTIENEPROGRAMADO(VES.SPRES$POM, " + tabla.Rows[i]["SPOA$ID_POA"] + ", " + tabla.Rows[i]["SPOA$ANIO"] + ", VES.SPRES$INSTITUCION, VES.SPPRO$ID_PROGRAMA_PRESUPUESTO) PR0GRAMADO" + tabla.Rows[i]["SPOA$ANIO"];
                                }


                                sql = sql + " ,VES.SPPSUB$PROPIETARIO ";
                                sql = sql + " FROM SCHE$SIPLAN20.SPPVST$SUBESTRATEGICOV2 VES WHERE VES.SPRES$POM = " + Session["pom"] + " AND VES.SPRES$INSTITUCION = " + Session["insto"] + ") GROUP BY  SPPRO$ID_PROGRAMA_PRESUPUESTO, PROGRAMA, P0A2025, PRESUPUESTO2025, PR0GRAMADO2025, P0A2026, PRESUPUESTO2026, PR0GRAMADO2026, P0A2027, PRESUPUESTO2027, PR0GRAMADO2027,  P0A2028, PRESUPUESTO2028, PR0GRAMADO2028, P0A2029, PRESUPUESTO2029, PR0GRAMADO2029 ORDER BY  SPPRO$ID_PROGRAMA_PRESUPUESTO ASC";
                                lblTipodeProduccion.Text = "PRODUCTOS INSTITUCIONALES";
                            }


                            else if (tipo == 1)
                            {
                                sql = "SELECT  SPPRO$ID_PROGRAMA_PRESUPUESTO, PROGRAMA, NULL P0A2020, NULL PRESUPUESTO2020, NULL PR0GRAMADO2020, NULL P0A2021, NULL PRESUPUESTO2021, NULL PR0GRAMADO2021, NULL P0A2022, NULL PRESUPUESTO2022, NULL PR0GRAMADO2022, NULL P0A2023, NULL PRESUPUESTO2023, NULL PR0GRAMADO2023, NULL P0A2024, NULL PRESUPUESTO2024, NULL PR0GRAMADO2024, P0A2025, PRESUPUESTO2025,  PR0GRAMADO2025,  P0A2026, PRESUPUESTO2026,  PR0GRAMADO2026,  P0A2027, PRESUPUESTO2027,  PR0GRAMADO2027, P0A2028, PRESUPUESTO2028, PR0GRAMADO2028, P0A2029, PRESUPUESTO2029, PR0GRAMADO2029 FROM  (SELECT VES.ID_RESULTADO, VES.RED,VES.SPPRO$ID_PROGRAMA_PRESUPUESTO, VES.SPPRO$ID_PROGRAMA_PRESUPUESTO||'-'||VES.PROGRAMA PROGRAMA";
                                for (int i = 0; i < tabla.Rows.Count; i++)
                                {
                                    sql = sql + " ,SCHE$SIPLAN20.FNC$POA(VES.SPRES$POM,VES.SPRES$INSTITUCION," + tabla.Rows[i]["SPOA$ANIO"] + ") P0A" + tabla.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$OBTIENEPRESUPUESTO(VES.SPPRO$ID_PROGRAMA_PRESUPUESTO," + tabla.Rows[i]["SPOA$ID_POA"] + ", " + tabla.Rows[i]["SPOA$ANIO"] + ",VES.SPRES$POM, VES.SPRES$INSTITUCION) PRESUPUESTO" + tabla.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$OBTIENEPROGRAMADO(VES.SPRES$POM, " + tabla.Rows[i]["SPOA$ID_POA"] + ", " + tabla.Rows[i]["SPOA$ANIO"] + ", VES.SPRES$INSTITUCION, VES.SPPRO$ID_PROGRAMA_PRESUPUESTO) PR0GRAMADO" + tabla.Rows[i]["SPOA$ANIO"];
                                }

                                sql = sql + " ,VES.SPPSUB$PROPIETARIO";
                                sql = sql + " FROM SCHE$SIPLAN20.SPPSVT$RED_INSTITUCIONAL VES WHERE VES.SPRES$POM = " + Session["pom"] + " AND VES.SPRES$INSTITUCION = " + Session["insto"] + ") GROUP BY  SPPRO$ID_PROGRAMA_PRESUPUESTO, PROGRAMA,  P0A2025, PRESUPUESTO2025,  PR0GRAMADO2025, P0A2026, PRESUPUESTO2026, PR0GRAMADO2026, P0A2027, PRESUPUESTO2027, PR0GRAMADO2027, P0A2028, PRESUPUESTO2028, PR0GRAMADO2028, P0A2029, PRESUPUESTO2029, PR0GRAMADO2029 ORDER BY  SPPRO$ID_PROGRAMA_PRESUPUESTO ASC";
                                lblTipodeProduccion.Text = "PRODUCTOS VINCULADOS A RED";
                            }

                        }

                        //NUEVA PGG
                        //INICIO 2026-2030
                        else if (Convert.ToInt32(Session["periodo"]) == 25)
                        {
                            if (tipo == 0)
                            {
                                sql = "SELECT SPPRO$ID_PROGRAMA_PRESUPUESTO, PROGRAMA, NULL P0A2020, NULL PRESUPUESTO2020, NULL PR0GRAMADO2020, NULL P0A2021, NULL PRESUPUESTO2021, NULL PR0GRAMADO2021, NULL P0A2022, NULL PRESUPUESTO2022, NULL PR0GRAMADO2022, NULL P0A2023, NULL PRESUPUESTO2023, NULL PR0GRAMADO2023, NULL P0A2024, NULL PRESUPUESTO2024, NULL PR0GRAMADO2024, NULL P0A2025, NULL PRESUPUESTO2025,  NULL PR0GRAMADO2025, P0A2026,  PRESUPUESTO2026,  PR0GRAMADO2026, P0A2027, PRESUPUESTO2027,  PR0GRAMADO2027, P0A2028, PRESUPUESTO2028, PR0GRAMADO2028, P0A2029, PRESUPUESTO2029, PR0GRAMADO2029,  P0A2030, PRESUPUESTO2030, PR0GRAMADO2030  FROM (SELECT VES.ID_RESULTADO, VES.RESULTADO,VES.SPPRO$ID_PROGRAMA_PRESUPUESTO, VES.SPPRO$ID_PROGRAMA_PRESUPUESTO||'-'||VES.PROGRAMA PROGRAMA";
                                for (int i = 0; i < tabla.Rows.Count; i++)
                                {
                                    sql = sql + " ,SCHE$SIPLAN20.FNC$POA(VES.SPRES$POM,VES.SPRES$INSTITUCION," + tabla.Rows[i]["SPOA$ANIO"] + ") P0A" + tabla.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$OBTIENEPRESUPUESTO(VES.SPPRO$ID_PROGRAMA_PRESUPUESTO," + tabla.Rows[i]["SPOA$ID_POA"] + ", " + tabla.Rows[i]["SPOA$ANIO"] + ",VES.SPRES$POM, VES.SPRES$INSTITUCION) PRESUPUESTO" + tabla.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$OBTIENEPROGRAMADO(VES.SPRES$POM, " + tabla.Rows[i]["SPOA$ID_POA"] + ", " + tabla.Rows[i]["SPOA$ANIO"] + ", VES.SPRES$INSTITUCION, VES.SPPRO$ID_PROGRAMA_PRESUPUESTO) PR0GRAMADO" + tabla.Rows[i]["SPOA$ANIO"];
                                }


                                sql = sql + " ,VES.SPPSUB$PROPIETARIO ";
                                sql = sql + " FROM SCHE$SIPLAN20.SPPVST$SUBESTRATEGICOV2 VES WHERE VES.SPRES$POM = " + Session["pom"] + " AND VES.SPRES$INSTITUCION = " + Session["insto"] + ") GROUP BY  SPPRO$ID_PROGRAMA_PRESUPUESTO, PROGRAMA, P0A2026, PRESUPUESTO2026, PR0GRAMADO2026, P0A2027, PRESUPUESTO2027, PR0GRAMADO2027,  P0A2028, PRESUPUESTO2028, PR0GRAMADO2028, P0A2029, PRESUPUESTO2029, PR0GRAMADO2029,  P0A2030, PRESUPUESTO2030, PR0GRAMADO2030 ORDER BY  SPPRO$ID_PROGRAMA_PRESUPUESTO ASC";
                                lblTipodeProduccion.Text = "PRODUCTOS INSTITUCIONALES";
                            }


                            else if (tipo == 1)
                            {
                                sql = "SELECT  SPPRO$ID_PROGRAMA_PRESUPUESTO, PROGRAMA, NULL P0A2020, NULL PRESUPUESTO2020, NULL PR0GRAMADO2020, NULL P0A2021, NULL PRESUPUESTO2021, NULL PR0GRAMADO2021, NULL P0A2022, NULL PRESUPUESTO2022, NULL PR0GRAMADO2022, NULL P0A2023, NULL PRESUPUESTO2023, NULL PR0GRAMADO2023, NULL P0A2024, NULL PRESUPUESTO2024, NULL PR0GRAMADO2024, NULL P0A2025, NULL PRESUPUESTO2025,  NULL PR0GRAMADO2025,  P0A2026, PRESUPUESTO2026,  PR0GRAMADO2026,  P0A2027, PRESUPUESTO2027,  PR0GRAMADO2027, P0A2028, PRESUPUESTO2028, PR0GRAMADO2028, P0A2029, PRESUPUESTO2029, PR0GRAMADO2029,  P0A2030, PRESUPUESTO2030, PR0GRAMADO2030 FROM  (SELECT VES.ID_RESULTADO, VES.RED,VES.SPPRO$ID_PROGRAMA_PRESUPUESTO, VES.SPPRO$ID_PROGRAMA_PRESUPUESTO||'-'||VES.PROGRAMA PROGRAMA";
                                for (int i = 0; i < tabla.Rows.Count; i++)
                                {
                                    sql = sql + " ,SCHE$SIPLAN20.FNC$POA(VES.SPRES$POM,VES.SPRES$INSTITUCION," + tabla.Rows[i]["SPOA$ANIO"] + ") P0A" + tabla.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$OBTIENEPRESUPUESTO(VES.SPPRO$ID_PROGRAMA_PRESUPUESTO," + tabla.Rows[i]["SPOA$ID_POA"] + ", " + tabla.Rows[i]["SPOA$ANIO"] + ",VES.SPRES$POM, VES.SPRES$INSTITUCION) PRESUPUESTO" + tabla.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$OBTIENEPROGRAMADO(VES.SPRES$POM, " + tabla.Rows[i]["SPOA$ID_POA"] + ", " + tabla.Rows[i]["SPOA$ANIO"] + ", VES.SPRES$INSTITUCION, VES.SPPRO$ID_PROGRAMA_PRESUPUESTO) PR0GRAMADO" + tabla.Rows[i]["SPOA$ANIO"];
                                }

                                sql = sql + " ,VES.SPPSUB$PROPIETARIO";
                                sql = sql + " FROM SCHE$SIPLAN20.SPPSVT$RED_INSTITUCIONAL VES WHERE VES.SPRES$POM = " + Session["pom"] + " AND VES.SPRES$INSTITUCION = " + Session["insto"] + ") GROUP BY  SPPRO$ID_PROGRAMA_PRESUPUESTO, PROGRAMA, P0A2026, PRESUPUESTO2026, PR0GRAMADO2026, P0A2027, PRESUPUESTO2027, PR0GRAMADO2027, P0A2028, PRESUPUESTO2028, PR0GRAMADO2028, P0A2029, PRESUPUESTO2029, PR0GRAMADO2029,  P0A2030, PRESUPUESTO2030, PR0GRAMADO2030 ORDER BY  SPPRO$ID_PROGRAMA_PRESUPUESTO ASC";
                                lblTipodeProduccion.Text = "PRODUCTOS VINCULADOS A RED";
                            }

                        }

                        //FIN 2026-2030



                    }


                }



            }
            else
            {

                if (tipo == 0)
                {
                    sql = "SELECT SPPRO$ID_PROGRAMA_PRESUPUESTO, PROGRAMA, P0A2020, PRESUPUESTO2020, PR0GRAMADO2020, P0A2021, PRESUPUESTO2021, PR0GRAMADO2021, P0A2022, PRESUPUESTO2022, PR0GRAMADO2022, P0A2023, PRESUPUESTO2023, PR0GRAMADO2023, P0A2024, PRESUPUESTO2024, PR0GRAMADO2024, NULL P0A2025, NULL PRESUPUESTO2025, NULL PR0GRAMADO2025, NULL P0A2026, NULL PRESUPUESTO2026, NULL PR0GRAMADO2026, NULL P0A2027, NULL PRESUPUESTO2027, NULL PR0GRAMADO2027 FROM (SELECT VES.ID_RESULTADO, VES.RESULTADO,VES.SPPRO$ID_PROGRAMA_PRESUPUESTO, VES.SPPRO$ID_PROGRAMA_PRESUPUESTO||'-'||VES.PROGRAMA PROGRAMA";
                    for (int i = 0; i < tabla.Rows.Count; i++)
                    {
                        sql = sql + " ,SCHE$SIPLAN20.FNC$POA(VES.SPRES$POM,VES.SPRES$INSTITUCION," + tabla.Rows[i]["SPOA$ANIO"] + ") P0A" + tabla.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$OBTIENEPRESUPUESTO(VES.SPPRO$ID_PROGRAMA_PRESUPUESTO," + tabla.Rows[i]["SPOA$ID_POA"] + ", " + tabla.Rows[i]["SPOA$ANIO"] + ",VES.SPRES$POM, VES.SPRES$INSTITUCION) PRESUPUESTO" + tabla.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$OBTIENEPROGRAMADO(VES.SPRES$POM, " + tabla.Rows[i]["SPOA$ID_POA"] + ", " + tabla.Rows[i]["SPOA$ANIO"] + ", VES.SPRES$INSTITUCION, VES.SPPRO$ID_PROGRAMA_PRESUPUESTO) PR0GRAMADO" + tabla.Rows[i]["SPOA$ANIO"];
                    }


                    sql = sql + " ,VES.SPPSUB$PROPIETARIO ";
                    sql = sql + " FROM SCHE$SIPLAN20.SPPVST$SUBESTRATEGICOV2 VES WHERE VES.SPRES$POM = " + Session["pom"] + " AND VES.SPRES$INSTITUCION = " + Session["insto"] + ") GROUP BY  SPPRO$ID_PROGRAMA_PRESUPUESTO, PROGRAMA, P0A2020, PRESUPUESTO2020, PR0GRAMADO2020, P0A2021, PRESUPUESTO2021, PR0GRAMADO2021, P0A2022, PRESUPUESTO2022, PR0GRAMADO2022, P0A2023, PRESUPUESTO2023, PR0GRAMADO2023, P0A2024, PRESUPUESTO2024, PR0GRAMADO2024 ORDER BY  SPPRO$ID_PROGRAMA_PRESUPUESTO ASC";
                    lblTipodeProduccion.Text = "PRODUCTOS INSTITUCIONALES";
                }


                else if (tipo == 1)
                {
                    sql = "SELECT  SPPRO$ID_PROGRAMA_PRESUPUESTO, PROGRAMA, P0A2020, PRESUPUESTO2020, PR0GRAMADO2020, P0A2021, PRESUPUESTO2021, PR0GRAMADO2021, P0A2022, PRESUPUESTO2022, PR0GRAMADO2022, P0A2023, PRESUPUESTO2023, PR0GRAMADO2023, P0A2024, PRESUPUESTO2024, PR0GRAMADO2024, NULL P0A2025, NULL PRESUPUESTO2025, NULL PR0GRAMADO2025, NULL P0A2026, NULL PRESUPUESTO2026, NULL PR0GRAMADO2026, NULL P0A2027, NULL PRESUPUESTO2027, NULL PR0GRAMADO2027 FROM  (SELECT VES.ID_RESULTADO, VES.RED,VES.SPPRO$ID_PROGRAMA_PRESUPUESTO, VES.SPPRO$ID_PROGRAMA_PRESUPUESTO||'-'||VES.PROGRAMA PROGRAMA";
                    for (int i = 0; i < tabla.Rows.Count; i++)
                    {
                        sql = sql + " ,SCHE$SIPLAN20.FNC$POA(VES.SPRES$POM,VES.SPRES$INSTITUCION," + tabla.Rows[i]["SPOA$ANIO"] + ") P0A" + tabla.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$OBTIENEPRESUPUESTO(VES.SPPRO$ID_PROGRAMA_PRESUPUESTO," + tabla.Rows[i]["SPOA$ID_POA"] + ", " + tabla.Rows[i]["SPOA$ANIO"] + ",VES.SPRES$POM, VES.SPRES$INSTITUCION) PRESUPUESTO" + tabla.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$OBTIENEPROGRAMADO(VES.SPRES$POM, " + tabla.Rows[i]["SPOA$ID_POA"] + ", " + tabla.Rows[i]["SPOA$ANIO"] + ", VES.SPRES$INSTITUCION, VES.SPPRO$ID_PROGRAMA_PRESUPUESTO) PR0GRAMADO" + tabla.Rows[i]["SPOA$ANIO"];
                    }


                    sql = sql + " ,VES.SPPSUB$PROPIETARIO";
                    sql = sql + " FROM SCHE$SIPLAN20.SPPSVT$RED_INSTITUCIONAL VES WHERE VES.SPRES$POM = " + Session["pom"] + " AND VES.SPRES$INSTITUCION = " + Session["insto"] + ") GROUP BY  SPPRO$ID_PROGRAMA_PRESUPUESTO,PROGRAMA, P0A2020, PRESUPUESTO2020, PR0GRAMADO2020,P0A2021, PRESUPUESTO2021, PR0GRAMADO2021, P0A2022, PRESUPUESTO2022, PR0GRAMADO2022, P0A2023, PRESUPUESTO2023, PR0GRAMADO2023, P0A2024, PRESUPUESTO2024, PR0GRAMADO2024 ORDER BY  SPPRO$ID_PROGRAMA_PRESUPUESTO ASC";
                    lblTipodeProduccion.Text = "PRODUCTOS VINCULADOS A RED";
                }
            }



            estado = dao.consulta(sql);
            if (estado == 0)
            {
                mensaje = dao.mensaje;
                Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
            }
            else
            {
                produccion = dao.tabla;
                gvProduccion.DataSource = produccion;
                gvProduccion.DataBind();
                gvProduccion.ExpandAll();
            }

        }

        protected void cargaProduccionPOA(int anio, int tipo)
        {
            Session["carga"] = 2;
            DataTable poa = new DataTable();
            DataTable produccionPOA = new DataTable();
            poa = obtienepoa(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), anio);
            if (poa.Rows.Count > 0)
            {
                Session["poactual"] = poa;
                if (tipo == 0)
                {
                    sql = "SELECT  SPPRO$ID_PROGRAMA_PRESUPUESTO, PROGRAMA ";
                    for (int i = 1; i <= 12; i++)
                    {
                        sql = sql + " ,MES" + i;
                    }
                    sql = sql + " ,SPRES$POM, ANUAL, CUA1, CUA2, CUA3 ";
                    sql = sql + " FROM (SELECT VES.SPPRO$ID_PROGRAMA_PRESUPUESTO, VES.SPPRO$ID_PROGRAMA_PRESUPUESTO||'-'||VES.PROGRAMA PROGRAMA ";
                    for (int i = 1; i <= 12; i++)
                    {
                        sql = sql + " ,SCHE$SIPLAN20.FNC$PRESUPUESTOMENSUAL(VES.SPRES$POM, " + poa.Rows[0]["SPOA$ID_POA"] + ", " + poa.Rows[0]["SPOA$ANIO"] + ", VES.SPRES$INSTITUCION, VES.SPPRO$ID_PROGRAMA_PRESUPUESTO, " + i + ") MES" + i;
                    }


                    sql = sql + " ,VES.SPRES$POM, SCHE$SIPLAN20.FNC$OBTIENEPRESUPUESTO(VES.SPPRO$ID_PROGRAMA_PRESUPUESTO," + poa.Rows[0]["SPOA$ID_POA"] + ", " + poa.Rows[0]["SPOA$ANIO"] + ", VES.SPRES$POM, VES.SPRES$INSTITUCION) ANUAL, SCHE$SIPLAN20.FNC$PRESUPUESTOCUATRIMESTRE(VES.SPRES$POM, " + poa.Rows[0]["SPOA$ID_POA"] + ", " + poa.Rows[0]["SPOA$ANIO"] + ", VES.SPRES$INSTITUCION, VES.SPPRO$ID_PROGRAMA_PRESUPUESTO, 1, 4) CUA1, SCHE$SIPLAN20.FNC$PRESUPUESTOCUATRIMESTRE(VES.SPRES$POM, " + poa.Rows[0]["SPOA$ID_POA"] + ", " + poa.Rows[0]["SPOA$ANIO"] + ", VES.SPRES$INSTITUCION, VES.SPPRO$ID_PROGRAMA_PRESUPUESTO, 5, 8) CUA2, SCHE$SIPLAN20.FNC$PRESUPUESTOCUATRIMESTRE(VES.SPRES$POM, " + poa.Rows[0]["SPOA$ID_POA"] + ", " + poa.Rows[0]["SPOA$ANIO"] + ", VES.SPRES$INSTITUCION, VES.SPPRO$ID_PROGRAMA_PRESUPUESTO, 9, 12) CUA3 ";
                    sql = sql + " FROM SCHE$SIPLAN20.SPPVST$SUBESTRATEGICOV2 VES WHERE VES.SPRES$POM = " + Session["pom"] + " AND VES.SPRES$INSTITUCION = " + Session["insto"] + ") GROUP BY SPPRO$ID_PROGRAMA_PRESUPUESTO, PROGRAMA ";
                    for (int i = 1; i <= 12; i++)
                    {
                        sql = sql + " ,MES" + i;
                    }
                    sql = sql + " ,SPRES$POM, ANUAL, CUA1, CUA2, CUA3 ORDER BY  SPPRO$ID_PROGRAMA_PRESUPUESTO ASC";
                }

                else if (tipo == 1)
                {
                    sql = "SELECT  SPPRO$ID_PROGRAMA_PRESUPUESTO, PROGRAMA ";
                    for (int i = 1; i <= 12; i++)
                    {
                        sql = sql + " ,MES" + i;
                    }
                    sql = sql + " ,SPRES$POM, ANUAL, CUA1, CUA2, CUA3 ";
                    sql = sql + " FROM (SELECT VES.SPPRO$ID_PROGRAMA_PRESUPUESTO, VES.SPPRO$ID_PROGRAMA_PRESUPUESTO||'-'||VES.PROGRAMA PROGRAMA ";
                    for (int i = 1; i <= 12; i++)
                    {
                        sql = sql + " ,SCHE$SIPLAN20.FNC$PRESUPUESTOMENSUAL(VES.SPRES$POM, " + poa.Rows[0]["SPOA$ID_POA"] + ", " + poa.Rows[0]["SPOA$ANIO"] + ", VES.SPRES$INSTITUCION, VES.SPPRO$ID_PROGRAMA_PRESUPUESTO, " + i + ") MES" + i;
                    }


                    sql = sql + " ,VES.SPRES$POM, SCHE$SIPLAN20.FNC$OBTIENEPRESUPUESTO(VES.SPPRO$ID_PROGRAMA_PRESUPUESTO," + poa.Rows[0]["SPOA$ID_POA"] + ", " + poa.Rows[0]["SPOA$ANIO"] + ", VES.SPRES$POM, VES.SPRES$INSTITUCION) ANUAL, SCHE$SIPLAN20.FNC$PRESUPUESTOCUATRIMESTRE(VES.SPRES$POM, " + poa.Rows[0]["SPOA$ID_POA"] + ", " + poa.Rows[0]["SPOA$ANIO"] + ", VES.SPRES$INSTITUCION, VES.SPPRO$ID_PROGRAMA_PRESUPUESTO, 1, 4) CUA1, SCHE$SIPLAN20.FNC$PRESUPUESTOCUATRIMESTRE(VES.SPRES$POM, " + poa.Rows[0]["SPOA$ID_POA"] + ", " + poa.Rows[0]["SPOA$ANIO"] + ", VES.SPRES$INSTITUCION, VES.SPPRO$ID_PROGRAMA_PRESUPUESTO, 5, 8) CUA2, SCHE$SIPLAN20.FNC$PRESUPUESTOCUATRIMESTRE(VES.SPRES$POM, " + poa.Rows[0]["SPOA$ID_POA"] + ", " + poa.Rows[0]["SPOA$ANIO"] + ", VES.SPRES$INSTITUCION, VES.SPPRO$ID_PROGRAMA_PRESUPUESTO, 9, 12) CUA3 ";
                    sql = sql + " FROM SCHE$SIPLAN20.SPPSVT$RED_INSTITUCIONAL VES WHERE VES.SPRES$POM = " + Session["pom"] + " AND VES.SPRES$INSTITUCION = " + Session["insto"] + ") GROUP BY  SPPRO$ID_PROGRAMA_PRESUPUESTO, PROGRAMA ";
                    for (int i = 1; i <= 12; i++)
                    {
                        sql = sql + " ,MES" + i;
                    }
                    sql = sql + " ,SPRES$POM, ANUAL, CUA1, CUA2, CUA3 ORDER BY SPPRO$ID_PROGRAMA_PRESUPUESTO ASC";

                }

                estado = dao.consulta(sql);
                if (estado == 0)
                {
                    mensaje = dao.mensaje;
                    // Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
                    ScriptManager.RegisterStartupScript(this.upNT, GetType(), "script", "Alerta('" + mensaje + " <br/>',2);", true);
                }
                else
                {
                    produccionPOA = dao.tabla;
                    gvPOA.DataSource = produccionPOA;
                    gvPOA.DataBind();
                    gvPOA.ExpandAll();
                }
            }

        }

        protected DataTable cargaProductosPOA(int anio, int tipo, int resultado, Double programa, int pom, int insto, int resultado2)
        {
            DataTable poa = new DataTable();
            DataTable productos = new DataTable();
            poa = obtienepoa(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), anio);
            if (poa.Rows.Count > 0)
            {
                if (tipo == 0)
                {
                    sql = "SELECT ID_RESULTADO, SPPRO$ID_PROGRAMA_PRESUPUESTO, SPPRO$ID_PRODUCTO, PRODUCTO, SPPRO$ID_MEDIDA, MEDIDA_PRODUCTO ";
                    for (int i = 1; i <= 12; i++)
                    {
                        sql = sql + ",MFINMES" + i + ",IDMETMES" + i + ", MFISMES" + i;
                    }
                    sql = sql + " ,SPRES$POM, MFPRODANUAL, MFINPRODANUAL, CUAFIS1, CUAFIS2, CUAFIS3, CUAFINPROD1, CUAFINPROD2, CUAFINPROD3, ESTADO FROM (";
                    sql = sql + "SELECT VES.ID_RESULTADO, VES.SPPRO$ID_PROGRAMA_PRESUPUESTO, VES.SPPRO$ID_PRODUCTO, VES.PRODUCTO, VES.SPPRO$ID_MEDIDA, VES.MEDIDA_PRODUCTO";
                    for (int i = 1; i <= 12; i++)
                    {
                        sql = sql + ",SCHE$SIPLAN20.FNC$POAMENFINPRODUCTO(" + poa.Rows[0]["SPOA$ID_POA"] + ", " + poa.Rows[0]["SPOA$ANIO"] + ", VES.SPPRO$ID_PRODUCTO, " + i + ") MFINMES" + i + ",SCHE$SIPLAN20.FNC$POAPROMENID(" + poa.Rows[0]["SPOA$ID_POA"] + ", " + poa.Rows[0]["SPOA$ANIO"] + ", VES.SPPRO$ID_PRODUCTO, " + i + ") IDMETMES" + i + ", SCHE$SIPLAN20.FNC$POAPROMETAMENSUAL(" + poa.Rows[0]["SPOA$ID_POA"] + ", " + poa.Rows[0]["SPOA$ANIO"] + ", VES.SPPRO$ID_PRODUCTO, " + i + ") MFISMES" + i;
                    }


                    sql = sql + " ,VES.SPRES$POM, SCHE$SIPLAN20.FNC$OBTIENEMETAFISICAPRO(" + poa.Rows[0]["SPOA$ID_POA"] + ", " + poa.Rows[0]["SPOA$ANIO"] + ",VES.SPPRO$ID_PRODUCTO) MFPRODANUAL, SCHE$SIPLAN20.FNC$OBTIENEMETAFINACIERAPROD(" + poa.Rows[0]["SPOA$ID_POA"] + ", " + poa.Rows[0]["SPOA$ANIO"] + ",VES.SPPRO$ID_PRODUCTO,0) MFINPRODANUAL, SCHE$SIPLAN20.FNC$POAPRODMFCUATRIMESTRAL(" + poa.Rows[0]["SPOA$ID_POA"] + ", " + poa.Rows[0]["SPOA$ANIO"] + ",VES.SPPRO$ID_PRODUCTO, 1, 4) CUAFIS1, SCHE$SIPLAN20.FNC$POAPRODMFCUATRIMESTRAL(" + poa.Rows[0]["SPOA$ID_POA"] + ", " + poa.Rows[0]["SPOA$ANIO"] + ",VES.SPPRO$ID_PRODUCTO, 5, 8) CUAFIS2, SCHE$SIPLAN20.FNC$POAPRODMFCUATRIMESTRAL(" + poa.Rows[0]["SPOA$ID_POA"] + ", " + poa.Rows[0]["SPOA$ANIO"] + ",VES.SPPRO$ID_PRODUCTO, 9, 12) CUAFIS3, SCHE$SIPLAN20.FNC$POAMENFINCUATPROD(" + poa.Rows[0]["SPOA$ID_POA"] + ", " + poa.Rows[0]["SPOA$ANIO"] + ",VES.SPPRO$ID_PRODUCTO, 1, 4) CUAFINPROD1, SCHE$SIPLAN20.FNC$POAMENFINCUATPROD(" + poa.Rows[0]["SPOA$ID_POA"] + ", " + poa.Rows[0]["SPOA$ANIO"] + ",VES.SPPRO$ID_PRODUCTO, 5, 8) CUAFINPROD2, SCHE$SIPLAN20.FNC$POAMENFINCUATPROD(" + poa.Rows[0]["SPOA$ID_POA"] + ", " + poa.Rows[0]["SPOA$ANIO"] + ",VES.SPPRO$ID_PRODUCTO, 9, 12) CUAFINPROD3, SCHE$SIPLAN20.FNC$POAESTADOPRODUCTO(" + poa.Rows[0]["SPOA$ID_POA"] + ", " + poa.Rows[0]["SPOA$ANIO"] + ",VES.SPPRO$ID_PRODUCTO,1) ESTADO";
                    sql = sql + " FROM SCHE$SIPLAN20.SPPVST$SUBESTRATEGICOV2 VES WHERE VES.ID_RESULTADO =" + resultado + "  AND VES.SPPRO$ID_PROGRAMA_PRESUPUESTO = " + programa + " AND  VES.SPRES$POM = " + pom + " AND  VES.SPRES$INSTITUCION =  " + insto + " AND VES.SPPRO$RESULTADO2 = " + resultado2 + ") GROUP BY ID_RESULTADO, SPPRO$ID_PROGRAMA_PRESUPUESTO, SPPRO$ID_PRODUCTO, PRODUCTO, SPPRO$ID_MEDIDA, MEDIDA_PRODUCTO ";
                    for (int i = 1; i <= 12; i++)
                    {
                        sql = sql + ",MFINMES" + i + ",IDMETMES" + i + ", MFISMES" + i;
                    }
                    sql = sql + " ,SPRES$POM, MFPRODANUAL, MFINPRODANUAL, CUAFIS1, CUAFIS2, CUAFIS3, CUAFINPROD1, CUAFINPROD2, CUAFINPROD3, ESTADO ORDER BY SPPRO$ID_PROGRAMA_PRESUPUESTO, SPPRO$ID_PRODUCTO ASC";
                }
                else if (tipo == 1)
                {
                    sql = "SELECT ID_RESULTADO, SPPRO$ID_PROGRAMA_PRESUPUESTO, SPPRO$ID_PRODUCTO, PRODUCTO, SPPRO$ID_MEDIDA, MEDIDA_PRODUCTO ";
                    for (int i = 1; i <= 12; i++)
                    {
                        sql = sql + ",MFINMES" + i + ",IDMETMES" + i + ", MFISMES" + i;
                    }
                    sql = sql + " ,SPRES$POM, MFPRODANUAL, MFINPRODANUAL, CUAFIS1, CUAFIS2, CUAFIS3, CUAFINPROD1, CUAFINPROD2, CUAFINPROD3, ESTADO FROM (";
                    sql = sql + "SELECT VES.ID_RESULTADO, VES.SPPRO$ID_PROGRAMA_PRESUPUESTO, VES.SPPRO$ID_PRODUCTO, VES.PRODUCTO, VES.SPPRO$ID_MEDIDA, VES.MEDIDA_PRODUCTO";
                    for (int i = 1; i <= 12; i++)
                    {
                        sql = sql + ",SCHE$SIPLAN20.FNC$POAMENFINPRODUCTO(" + poa.Rows[0]["SPOA$ID_POA"] + ", " + poa.Rows[0]["SPOA$ANIO"] + ", VES.SPPRO$ID_PRODUCTO, " + i + ") MFINMES" + i + ",SCHE$SIPLAN20.FNC$POAPROMENID(" + poa.Rows[0]["SPOA$ID_POA"] + ", " + poa.Rows[0]["SPOA$ANIO"] + ", VES.SPPRO$ID_PRODUCTO, " + i + ") IDMETMES" + i + ", SCHE$SIPLAN20.FNC$POAPROMETAMENSUAL(" + poa.Rows[0]["SPOA$ID_POA"] + ", " + poa.Rows[0]["SPOA$ANIO"] + ", VES.SPPRO$ID_PRODUCTO, " + i + ") MFISMES" + i;
                    }


                    sql = sql + " ,VES.SPRES$POM, SCHE$SIPLAN20.FNC$OBTIENEMETAFISICAPRO(" + poa.Rows[0]["SPOA$ID_POA"] + ", " + poa.Rows[0]["SPOA$ANIO"] + ",VES.SPPRO$ID_PRODUCTO) MFPRODANUAL, SCHE$SIPLAN20.FNC$OBTIENEMETAFINACIERAPROD(" + poa.Rows[0]["SPOA$ID_POA"] + ", " + poa.Rows[0]["SPOA$ANIO"] + ",VES.SPPRO$ID_PRODUCTO,0) MFINPRODANUAL, SCHE$SIPLAN20.FNC$POAPRODMFCUATRIMESTRAL(" + poa.Rows[0]["SPOA$ID_POA"] + ", " + poa.Rows[0]["SPOA$ANIO"] + ",VES.SPPRO$ID_PRODUCTO, 1, 4) CUAFIS1, SCHE$SIPLAN20.FNC$POAPRODMFCUATRIMESTRAL(" + poa.Rows[0]["SPOA$ID_POA"] + ", " + poa.Rows[0]["SPOA$ANIO"] + ",VES.SPPRO$ID_PRODUCTO, 5, 8) CUAFIS2, SCHE$SIPLAN20.FNC$POAPRODMFCUATRIMESTRAL(" + poa.Rows[0]["SPOA$ID_POA"] + ", " + poa.Rows[0]["SPOA$ANIO"] + ",VES.SPPRO$ID_PRODUCTO, 9, 12) CUAFIS3, SCHE$SIPLAN20.FNC$POAMENFINCUATPROD(" + poa.Rows[0]["SPOA$ID_POA"] + ", " + poa.Rows[0]["SPOA$ANIO"] + ",VES.SPPRO$ID_PRODUCTO, 1, 4) CUAFINPROD1, SCHE$SIPLAN20.FNC$POAMENFINCUATPROD(" + poa.Rows[0]["SPOA$ID_POA"] + ", " + poa.Rows[0]["SPOA$ANIO"] + ",VES.SPPRO$ID_PRODUCTO, 5, 8) CUAFINPROD2, SCHE$SIPLAN20.FNC$POAMENFINCUATPROD(" + poa.Rows[0]["SPOA$ID_POA"] + ", " + poa.Rows[0]["SPOA$ANIO"] + ",VES.SPPRO$ID_PRODUCTO, 9, 12) CUAFINPROD3, SCHE$SIPLAN20.FNC$POAESTADOPRODUCTO(" + poa.Rows[0]["SPOA$ID_POA"] + ", " + poa.Rows[0]["SPOA$ANIO"] + ",VES.SPPRO$ID_PRODUCTO,1) ESTADO";
                    sql = sql + " FROM SCHE$SIPLAN20.SPPSVT$RED_INSTITUCIONAL VES WHERE VES.ID_RESULTADO =" + resultado + "  AND VES.SPPRO$ID_PROGRAMA_PRESUPUESTO = " + programa + " AND  VES.SPRES$POM = " + pom + " AND  VES.SPRES$INSTITUCION =  " + insto + " AND VES.SPPRO$RESULTADO2 = " + resultado2 + ") GROUP BY ID_RESULTADO, SPPRO$ID_PROGRAMA_PRESUPUESTO, SPPRO$ID_PRODUCTO, PRODUCTO, SPPRO$ID_MEDIDA, MEDIDA_PRODUCTO ";
                    for (int i = 1; i <= 12; i++)
                    {
                        sql = sql + ",MFINMES" + i + ",IDMETMES" + i + ", MFISMES" + i;
                    }
                    sql = sql + " ,SPRES$POM, MFPRODANUAL, MFINPRODANUAL, CUAFIS1, CUAFIS2, CUAFIS3, CUAFINPROD1, CUAFINPROD2, CUAFINPROD3, ESTADO ORDER BY SPPRO$ID_PROGRAMA_PRESUPUESTO, SPPRO$ID_PRODUCTO ASC";
                }

                estado = dao.consulta(sql);
                if (estado != 0)
                    productos = dao.tabla;
            }

            return productos;
        }
        protected DataTable cargaProgramaEstrategico(DataTable tabla, Double programa, int tipo)
        {
            DataTable programas = new DataTable();

            if (tipo == 0)
            {
                sql = "SELECT SPPRO$ID_PROGRAMA_PRESUPUESTO ";
                for (int i = 0; i < tabla.Rows.Count; i++)
                {
                    sql = sql + " ,PRESUPUESTO" + tabla.Rows[i]["SPOA$ANIO"] + ",  PR0GRAMADO" + tabla.Rows[i]["SPOA$ANIO"];
                }
                sql = sql + ", SPPRO$ID_PRODUCTO FROM (";
                sql = sql + "SELECT  VES.SPPRO$ID_PROGRAMA_PRESUPUESTO ";
                for (int i = 0; i < tabla.Rows.Count; i++)
                {
                    sql = sql + " ,SCHE$SIPLAN20.FNC$OBTIENEPRESUPUESTO(VES.SPPRO$ID_PROGRAMA_PRESUPUESTO," + tabla.Rows[i]["SPOA$ID_POA"] + ", " + tabla.Rows[i]["SPOA$ANIO"] + ",VES.SPRES$POM, VES.SPRES$INSTITUCION) PRESUPUESTO" + tabla.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$OBTIENEPROGRAMADO(VES.SPRES$POM, " + tabla.Rows[i]["SPOA$ID_POA"] + ", " + tabla.Rows[i]["SPOA$ANIO"] + ", VES.SPRES$INSTITUCION, VES.SPPRO$ID_PROGRAMA_PRESUPUESTO) PR0GRAMADO" + tabla.Rows[i]["SPOA$ANIO"];
                }


                sql = sql + " ,VES.SPPRO$ID_PRODUCTO ";
                sql = sql + " FROM SCHE$SIPLAN20.SPPVST$SUBESTRATEGICOV2 VES WHERE VES.SPRES$POM = " + Session["pom"] + " AND VES.SPRES$INSTITUCION = " + Session["insto"] + " AND VES.SPPRO$ID_PROGRAMA_PRESUPUESTO = " + programa + ") GROUP BY SPPRO$ID_PROGRAMA_PRESUPUESTO";
                for (int i = 0; i < tabla.Rows.Count; i++)
                {
                    sql = sql + " ,PRESUPUESTO" + tabla.Rows[i]["SPOA$ANIO"] + ",  PR0GRAMADO" + tabla.Rows[i]["SPOA$ANIO"];
                }
                sql = sql + " , SPPRO$ID_PRODUCTO";
            }

            else if (tipo == 1)
            {
                sql = "SELECT SPPRO$ID_PROGRAMA_PRESUPUESTO ";
                for (int i = 0; i < tabla.Rows.Count; i++)
                {
                    sql = sql + " ,PRESUPUESTO" + tabla.Rows[i]["SPOA$ANIO"] + ",  PR0GRAMADO" + tabla.Rows[i]["SPOA$ANIO"];
                }
                sql = sql + ", SPPRO$ID_PRODUCTO FROM (";
                sql = sql + "SELECT  VES.SPPRO$ID_PROGRAMA_PRESUPUESTO ";
                for (int i = 0; i < tabla.Rows.Count; i++)
                {
                    sql = sql + " ,SCHE$SIPLAN20.FNC$OBTIENEPRESUPUESTO(VES.SPPRO$ID_PROGRAMA_PRESUPUESTO," + tabla.Rows[i]["SPOA$ID_POA"] + ", " + tabla.Rows[i]["SPOA$ANIO"] + ",VES.SPRES$POM, VES.SPRES$INSTITUCION) PRESUPUESTO" + tabla.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$OBTIENEPROGRAMADO(VES.SPRES$POM, " + tabla.Rows[i]["SPOA$ID_POA"] + ", " + tabla.Rows[i]["SPOA$ANIO"] + ", VES.SPRES$INSTITUCION, VES.SPPRO$ID_PROGRAMA_PRESUPUESTO) PR0GRAMADO" + tabla.Rows[i]["SPOA$ANIO"];
                }


                sql = sql + " ,VES.SPPRO$ID_PRODUCTO ";
                sql = sql + " FROM SCHE$SIPLAN20.SPPSVT$RED_INSTITUCIONAL VES WHERE VES.SPRES$POM = " + Session["pom"] + " AND VES.SPRES$INSTITUCION = " + Session["insto"] + " AND VES.SPPRO$ID_PROGRAMA_PRESUPUESTO = " + programa + ") GROUP BY SPPRO$ID_PROGRAMA_PRESUPUESTO";
                for (int i = 0; i < tabla.Rows.Count; i++)
                {
                    sql = sql + " ,PRESUPUESTO" + tabla.Rows[i]["SPOA$ANIO"] + ",  PR0GRAMADO" + tabla.Rows[i]["SPOA$ANIO"];
                }
                sql = sql + " , SPPRO$ID_PRODUCTO";
            }


            estado = dao.consulta(sql);
            if (estado != 0)
            {
                programas = dao.tabla;
            }

            return programas;
        }

        //protected void refrescaprogramas(int programa)
        //{
        //    DataTable estrategicos = new DataTable();
        //    DataTable tabla = new DataTable();
        //    tabla = (DataTable)Session["poa"];
        //    sql = "SELECT SPPRO$ID_PROGRAMA_PRESUPUESTO ";
        //    for (int i = 0; i < tabla.Rows.Count; i++)
        //    {
        //        sql = sql + " ,PRESUPUESTO" + tabla.Rows[i]["SPOA$ANIO"] + ",  PR0GRAMADO" + tabla.Rows[i]["SPOA$ANIO"];
        //    }
        //    sql = sql + ", SPPRO$ID_PRODUCTO FROM (";
        //    sql = sql + "SELECT  VES.SPPRO$ID_PROGRAMA_PRESUPUESTO ";
        //    for (int i = 0; i < tabla.Rows.Count; i++)
        //    {
        //        sql = sql + " ,SCHE$SIPLAN20.FNC$OBTIENEPRESUPUESTO(VES.SPPRO$ID_PROGRAMA_PRESUPUESTO," + tabla.Rows[i]["SPOA$ID_POA"] + ", " + tabla.Rows[i]["SPOA$ANIO"] + ",VES.SPRES$POM, VES.SPRES$INSTITUCION) PRESUPUESTO" + tabla.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$OBTIENEPROGRAMADO(VES.SPRES$POM, " + tabla.Rows[i]["SPOA$ID_POA"] + ", " + tabla.Rows[i]["SPOA$ANIO"] + ", VES.SPRES$INSTITUCION, VES.SPPRO$ID_PROGRAMA_PRESUPUESTO) PR0GRAMADO" + tabla.Rows[i]["SPOA$ANIO"];
        //    }


        //    sql = sql + " ,VES.SPPRO$ID_PRODUCTO ";
        //    sql = sql + " FROM SCHE$SIPLAN20.SPPVST$SUBESTRATEGICO VES WHERE VES.SPRES$POM = " + Session["pom"] + " AND VES.SPRES$INSTITUCION = " + Session["insto"] + " AND VES.SPPRO$ID_PROGRAMA_PRESUPUESTO = " + programa + ") GROUP BY SPPRO$ID_PROGRAMA_PRESUPUESTO";
        //    for (int i = 0; i < tabla.Rows.Count; i++)
        //    {
        //        sql = sql + " ,PRESUPUESTO" + tabla.Rows[i]["SPOA$ANIO"] + ",  PR0GRAMADO" + tabla.Rows[i]["SPOA$ANIO"];
        //    }
        //    sql = sql + " , SPPRO$ID_PRODUCTO";
        //    estado = dao.consulta(sql);
        //    if (estado == 0)
        //    {
        //        mensaje = dao.mensaje;
        //        Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
        //    }
        //    else
        //    {
        //        estrategicos = dao.tabla;
        //        //Session["programasub"] = estrategicos;
        //        gvDistroPresupuesto.DataSource = estrategicos;
        //        gvDistroPresupuesto.DataBind();

        //    }
        //}
        protected void btnMetas_Click(object sender, EventArgs e)
        {
            /*int nivel = 0;
            int producto = -1;
            int programa = -1;
            DataTable metas = new DataTable();
            DataTable metasfisicas = new DataTable();
            DataTable financiero = new DataTable();
            if (gvProduccion.FocusedRowIndex != -1)
            {
                nivel = gvProduccion.GetRowLevel(gvProduccion.FocusedRowIndex);
                if (nivel == 2 || nivel == 3)
                {
                    if (nivel == 2)
                    {
                        producto = Convert.ToInt32(gvProduccion.GetRowValues(gvProduccion.FocusedRowIndex, "SPPRO$ID_PRODUCTO").ToString());
                        Session["producto"] = producto;
                        //lblPilar.Text = "Pilar: " + gvProduccion.GetRowValues(gvProduccion.FocusedRowIndex, "PILAR_PGG").ToString();
                        lblResultado.Text = "Resultado: " + gvProduccion.GetRowValues(gvProduccion.FocusedRowIndex, "RESULTADO").ToString();
                        lblPrograma.Text = "Programa: " + gvProduccion.GetRowValues(gvProduccion.FocusedRowIndex, "PROGRAMA").ToString();
                        lblProducto.Text = "Producto: " + gvProduccion.GetRowValues(gvProduccion.FocusedRowIndex, "PRODUCTO").ToString();

                        if (Session["poa"] != null)
                        {
                            temp = (DataTable)Session["poa"];
                            if (temp.Rows.Count > 0)
                            {
                                metas = cargametafisicaProducto(temp, producto);
                                if (metas.Rows.Count > 0)
                                {
                                    Session["mfproductos"] = metas;
                                    gvMetasFisicas.DataSource = metas;
                                    gvMetasFisicas.DataBind();

                                    financiero = cargametafinancieraProducto(temp, producto);
                                    if (metas.Rows.Count > 0)
                                    {
                                        gvMetaFinacieraProd.DataSource = financiero;
                                        gvMetaFinacieraProd.DataBind();
                                    }
                                    popProducto.ShowOnPageLoad = true;
                                }
                            }
                        }
                    }

                    else if (nivel == 3)
                    {
                        producto = Convert.ToInt32(gvProduccion.GetRowValues(gvProduccion.FocusedRowIndex, "SPPSUB$ID_SUBPRODUCTO").ToString());
                        programa = Convert.ToInt32(gvProduccion.GetRowValues(gvProduccion.FocusedRowIndex, "SPPRO$ID_PROGRAMA_PRESUPUESTO").ToString());
                        Session["producto"] = producto;
                        lblproductosub.Text = "Producto: " + gvProduccion.GetRowValues(gvProduccion.FocusedRowIndex, "PRODUCTO").ToString();
                        lbsubproducto.Text = "Subproducto: " + gvProduccion.GetRowValues(gvProduccion.FocusedRowIndex, "SUBPRODUCTO").ToString();
                        lblmedida.Text = "Medida: " + gvProduccion.GetRowValues(gvProduccion.FocusedRowIndex, "MEDIDA_SUBPRODUCTO").ToString();
                        if (gvProduccion.GetRowValues(gvProduccion.FocusedRowIndex, "SPPSUB$SNIP") != DBNull.Value)
                        {
                            lblSNIP.Text = "SNIP: " + gvProduccion.GetRowValues(gvProduccion.FocusedRowIndex, "SPPSUB$SNIP").ToString();
                        }
                        else
                        {
                            lblSNIP.Text = gvProduccion.GetRowValues(gvProduccion.FocusedRowIndex, "SPPSUB$SNIP").ToString();
                        }
                        lblPogramaSub.Text = gvProduccion.GetRowValues(gvProduccion.FocusedRowIndex, "PROGRAMA").ToString();

                        if (Session["poa"] != null)
                        {
                            temp = (DataTable)Session["poa"];
                            if (temp.Rows.Count > 0)
                            {
                                metas = cargametaFinacierasub(temp, producto);
                                metasfisicas = cargaMetaFisicaSub(temp, producto);

                                if (metas.Rows.Count > 0)
                                {
                                    Session["metasfinsub"] = metas;
                                    cargaProgramaEstrategico(temp, programa, Convert.ToInt32(cbTipoProduccion.Value));
                                    Session["programapres"] = programa;
                                    Session["mfinsubproductos"] = metas;
                                    gvMetaFinancieraSub.DataSource = metas;
                                    gvMetaFinancieraSub.DataBind();
                                    Session["metasfisicassub"] = metasfisicas;
                                    gvmetasfisicassub.DataSource = metasfisicas;
                                    gvmetasfisicassub.DataBind();
                                    popSubproductos.ShowOnPageLoad = true;

                                }
                            }
                        }

                    }

                }
                else
                {
                    mensaje = "Seleccione  fila con la descripción del producto, o que contenga la descripción de un subproducto";
                    ScriptManager.RegisterStartupScript(this.panBotonArchivos, GetType(), "script", "Alerta('" + mensaje + " <br/>',2);", true);
                }
            }
            else
            {
                mensaje = "Seleccione un producto";
                ScriptManager.RegisterStartupScript(this.panBotonArchivos, GetType(), "script", "Alerta('" + mensaje + " <br/>',2);", true);
            }
            */
            btnSuproductos_Click(sender, e);
        }

        //protected DataTable cargaDetalleEstrategico(int pilar,int resultado, int programa)
        //{
        //    DataTable estrategicos = new DataTable();
        //    sql = "SELECT  ROWNUM NUMERO, VES.SPPRO$ID_PRODUCTO, VES.PRODUCTO, VES.MEDIDA_PRODUCTO, VES.SPPRO$ID_MEDIDA, VES.SNCGUM$NOMBRE, VES.SPPSUB$ID_SUBPRODUCTO, VES.SUBPRODUCTO, VES.SPPSUB$ID_MEDIDA, VES.MEDIDA_SUBPRODUCTO, VES.SPPSUB$SNIP, VES.SPRES$POM, VES.SPRES$INSTITUCION, VES.SPPSUB$PROPIETARIO  FROM SCHE$SIPLAN20.SPPVST$SUBESTRATEGICO VES WHERE VES.SPRES$POM = " + Session["pom"] + " AND VES.SPRES$INSTITUCION = " + Session["insto"];
        //    sql = sql + " AND VES.ID_PILAR= "  +pilar + " AND VES.ID_RESULTADO = "+resultado+ " AND VES.SPPRO$ID_PROGRAMA_PRESUPUESTO = "+programa;
        //    estado = dao.consulta(sql);
        //    if (estado == 0)
        //    {
        //        mensaje = dao.mensaje;
        //        Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
        //    }
        //    else
        //    {
        //        estrategicos = dao.tabla;
        //    }

        //    return estrategicos;
        //}
        //protected void gvproductosEsDet_BeforePerformDataSelect(object sender, EventArgs e)
        //{
        //    int pilar, resultado, programa;
        //    DataTable detalle = new DataTable();
        //    ASPxGridView gvDetailInter = sender as ASPxGridView;
        //    int masterInterPoaID = Convert.ToInt32(gvDetailInter.GetMasterRowKeyValue());
        //    pilar = Convert.ToInt32((sender as ASPxGridView).GetMasterRowFieldValues("ID_PILAR"));
        //    resultado = Convert.ToInt32((sender as ASPxGridView).GetMasterRowFieldValues("ID_RESULTADO"));
        //    programa = Convert.ToInt32((sender as ASPxGridView).GetMasterRowFieldValues("SPPRO$ID_PROGRAMA_PRESUPUESTO"));
        //    //gvDetailInter.DataSource = loadIntervencionesPOA(masterInterPoaID);
        //    detalle = cargaDetalleEstrategico(pilar, resultado, programa);
        //    if (detalle.Rows.Count > 0)
        //    {
        //        gvDetailInter.DataSource = detalle;
        //        Session["masterInterPoaID"] = masterInterPoaID.ToString();
        //    }



        //}


        //protected void detailGrid_DataSelect(object sender, EventArgs e)
        //{
        //    int numero, pilar;
        //    //Session["CustomerID"] = (sender as ASPxGridView).GetMasterRowKeyValue();
        //    numero = Convert.ToInt32((sender as ASPxGridView).GetMasterRowFieldValues("NUMERO"));
        //    pilar = Convert.ToInt32((sender as ASPxGridView).GetMasterRowFieldValues("ID_PILAR"));
        //}

        protected DataTable cargametafisicaProducto(DataTable temp, int producto)
        {
            DataTable metasfis = new DataTable();
            sql = "SELECT ROWNUM NUMERO,";
            for (int i = 0; i < temp.Rows.Count; i++)
            {
                sql = sql + "SCHE$SIPLAN20.FNC$OBTIENEIDMETAFISICAPRO(" + temp.Rows[i]["SPOA$ID_POA"] + "," + temp.Rows[i]["SPOA$ANIO"] + "," + producto + ") ID" + temp.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$OBTIENEMETAFISICAPRO(" + temp.Rows[i]["SPOA$ID_POA"] + "," + temp.Rows[i]["SPOA$ANIO"] + "," + producto + ") FISICA" + temp.Rows[i]["SPOA$ANIO"] + ", ";
            }
            sql = sql + " SPPRO$RESTRICTIVA FROM  SCHE$SIPLAN20.SP20$PRODUCTO WHERE SPPRO$ID_PRODUCTO = " + producto + " AND SPPRO$RESTRICTIVA = 'N' ";
            estado = dao.consulta(sql);
            if (estado != 0)
                metasfis = dao.tabla;
            return metasfis;
        }

        protected DataTable cargametafinancieraProducto(DataTable temp, int producto)
        {
            DataTable metasfin = new DataTable();
            sql = "SELECT ROWNUM NUMERO,";
            for (int i = 0; i < temp.Rows.Count; i++)
            {
                sql = sql + "SCHE$SIPLAN20.FNC$OBTIENEMETAFINACIERAPROD(" + temp.Rows[i]["SPOA$ID_POA"] + ", " + temp.Rows[i]["SPOA$ANIO"] + "," + producto + ",0) FINANCIERO" + temp.Rows[i]["SPOA$ANIO"] + ", ";
            }
            sql = sql + " SPPRO$RESTRICTIVA FROM  SCHE$SIPLAN20.SP20$PRODUCTO WHERE SPPRO$ID_PRODUCTO = " + producto + " AND SPPRO$RESTRICTIVA = 'N' ";
            estado = dao.consulta(sql);
            if (estado != 0)
                metasfin = dao.tabla;
            return metasfin;
        }


        protected DataTable cargametaFinacierasub(DataTable temp, int sub)
        {
            DataTable metasfin = new DataTable();
            sql = "SELECT ROWNUM NUMERO,";
            for (int i = 0; i < temp.Rows.Count; i++)
            {
                sql = sql + "SCHE$SIPLAN20.FNC$IDMETAFINSUB(" + temp.Rows[i]["SPOA$ID_POA"] + ", " + temp.Rows[i]["SPOA$ANIO"] + "," + sub + ") ID" + temp.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$METAFINANCIERASUB(" + temp.Rows[i]["SPOA$ID_POA"] + ", " + temp.Rows[i]["SPOA$ANIO"] + "," + sub + ",0) FIN" + temp.Rows[i]["SPOA$ANIO"] + ", ";
            }
            sql = sql + "SPPSUB$RESTRICTIVA FROM  SCHE$SIPLAN20.SP20$SUB_PRODUCTO WHERE SPPSUB$ID_SUBPRODUCTO = " + sub + " AND SPPSUB$RESTRICTIVA = 'N' ";
            estado = dao.consulta(sql);
            if (estado != 0)
                metasfin = dao.tabla;
            return metasfin;
        }


        protected DataTable cargaMetaFisicaSub(DataTable temp, int sub)
        {
            DataTable metasfis = new DataTable();
            sql = "SELECT ROWNUM NUMERO,";
            for (int i = 0; i < temp.Rows.Count; i++)
            {
                sql = sql + "SCHE$SIPLAN20.FNC$IDMETAFISUB(" + temp.Rows[i]["SPOA$ID_POA"] + ", " + temp.Rows[i]["SPOA$ANIO"] + "," + sub + ") ID" + temp.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$OBTIENEMETAFISICASUB(" + temp.Rows[i]["SPOA$ID_POA"] + ", " + temp.Rows[i]["SPOA$ANIO"] + "," + sub + ",0) FIS" + temp.Rows[i]["SPOA$ANIO"] + ", ";
            }
            sql = sql + "SPPSUB$RESTRICTIVA FROM  SCHE$SIPLAN20.SP20$SUB_PRODUCTO WHERE SPPSUB$ID_SUBPRODUCTO = " + sub + " AND SPPSUB$RESTRICTIVA = 'N' ";
            estado = dao.consulta(sql);
            if (estado != 0)
                metasfis = dao.tabla;
            return metasfis;
        }





        protected int insertaMetaFisica(int producto, int poa, int anio, double meta)
        {
            int estados = 0;
            sql = "INSERT INTO SCHE$SIPLAN20.SP20$PROGRAMACION_MFPRODUCTO (SPPMFS$META, SPPMFS$ID_POA, SPPMFS$ANIO, SPPMFS$ID_PRODUCTO, SPPMFS$FECHA_INSERTA) VALUES(" + meta + ", " + poa + ", " + anio + ", " + producto + ", 'INSERTA = ' || TO_CHAR(SYSDATE, 'DD/MM/YYYY HH:MI') || ' ' || '" + Session["USUARIO"].ToString() + "')";
            estados = dao.comando(sql);
            return estados;
        }


        protected int insertaFinanciera(int sub, int poa, int anio, double meta)
        {
            int estados = 0;
            sql = "INSERT INTO SCHE$SIPLAN20.SP20$PROGRAMETA_FI_FIN_SUB (SPPMFS$META, SPPMFS$ID_POA, SPPMFS$ANIO, SPPMFS$ID_SUBPRODUCTO, SPPMFS$FECHA_INSERTA, SPPMFS$TIPO_PROGRAMACION) VALUES(" + meta + ", " + poa + ", " + anio + ", " + sub + ", 'INSERTA = ' || TO_CHAR(SYSDATE, 'DD/MM/YYYY HH:MI') || ' ' || '" + Session["USUARIO"].ToString() + "', 1)";
            estados = dao.comando(sql);
            return estados;
        }

        protected int insertaFisicasub(int sub, int poa, int anio, double meta)
        {
            int estados = 0;
            sql = "INSERT INTO SCHE$SIPLAN20.SP20$PROGRAMETA_FI_FIN_SUB (SPPMFS$META, SPPMFS$ID_POA, SPPMFS$ANIO, SPPMFS$ID_SUBPRODUCTO, SPPMFS$FECHA_INSERTA, SPPMFS$TIPO_PROGRAMACION) VALUES(" + meta + ", " + poa + ", " + anio + ", " + sub + ", 'INSERTA = ' || TO_CHAR(SYSDATE, 'DD/MM/YYYY HH:MI') || ' ' || '" + Session["USUARIO"].ToString() + "', 0)";
            estados = dao.comando(sql);
            return estados;
        }


        protected int insertaMetasubPOA(int poa, int anio, int sub, int mes, double meta, int tipo)
        {
            int estados = 0;
            sql = "INSERT INTO SCHE$SIPLAN20.SP20$POASUBPRODUCTOS (SPOAS$POA, SPOAS$ANIO, SPOAS$SUBPRODUCTO, SPOAS$MES, SPOAS$META, SPOAS$TIPO, SPOAS$FECHA_INSERTA) VALUES(" + poa + ", " + anio + ", " + sub + ", " + mes + "," + meta + ", " + tipo + ", 'INSERTA = ' || TO_CHAR(SYSDATE, 'DD/MM/YYYY HH:MI') || ' ' || '" + Session["USUARIO"].ToString() + "')";
            estados = dao.comando(sql);
            return estados;
        }

        protected int insertaFisicaProdmensual(int poa, int anio, int mes, int producto, double meta)
        {
            int estados = 0;
            sql = "INSERT INTO SCHE$SIPLAN20.SP20$POAPRODUCTOS (SPPR$POA, SPPR$ANIO, SPPR$MES, SPPR$PRODUCTO, SPPR$META, SPPR$FECHA_INSERTA, SPPR$TIPOPROGRA) VALUES(" + poa + ", " + anio + ", " + mes + ", " + producto + ", " + meta + ", 'INSERTA = ' || TO_CHAR(SYSDATE, 'DD/MM/YYYY HH:MI') || ' ' || '" + Session["USUARIO"].ToString() + "', 'I')";
            estados = dao.comando(sql);
            return estados;
        }


        protected int AcualizametafisicaMensualProd(int poa, int anio, double meta, int mes, int id_meta, int producto)
        {
            int estados = 0;

            sql = "UPDATE SCHE$SIPLAN20.SP20$POAPRODUCTOS SET SPPR$META = " + meta + ", SPPR$FECHA_UPDATE = 'UPDATE = ' || TO_CHAR(SYSDATE, 'DD/MM/YYYY HH:MI') || ' ' || '" + Session["USUARIO"].ToString() + "'  WHERE SPPR$POA =" + poa + " AND SPPR$ANIO = " + anio + " AND SPPR$PRODUCTO = " + producto + " AND SPPR$IDPROGRA = " + id_meta + " AND SPPR$MES = " + mes;
            estados = dao.comando(sql);
            return estados;
        }

        protected int Acualizametafinanciera(int sub, int poa, int anio, double meta, int id_meta)
        {
            int estados = 0;
            DataTable tabla = new DataTable();
            //sql = "SELECT CASE WHEN SPOA$ESTADO = 'A' THEN  1 WHEN  SPOA$ESTADO = 'D'  THEN 0 END AS ESTADO FROM SCHE$SIPLAN20.SP20$POA WHERE SPOA$ID_POA = " + poa + " AND SPOA$ANIO = " + anio + " AND SPOA$ID_POM = " + Session["pom"] + " AND SPOA$ID_INSTITUCION = " + Session["insto"];
            sql = @"SELECT 
                        CASE WHEN SPPO$ESTADO = 'A' THEN 1 
                        WHEN SPPO$ESTADO = 'D' AND CONTEO_PRORROGA > 0 AND TRUNC(SYSDATE) <= TRUNC(FECHA_PRORROGA) THEN 0 
                        WHEN SPPO$ESTADO = 'D' AND CONTEO = 0 THEN 1 
                        WHEN SPPO$ESTADO = 'D' AND CONTEO > 0 AND TRUNC(SYSDATE) > TRUNC(FECHA_CIERRE) THEN 1
                        WHEN SPPO$ESTADO = 'D' AND CONTEO > 0 AND TRUNC(SYSDATE) <= TRUNC(FECHA_CIERRE) THEN 0
                        WHEN SPPO$ESTADO = 'D' AND CONTEO > 0 AND FECHA_CIERRE IS NULL THEN 0
                        END AS ESTADO
                        FROM
                        (SELECT 
                             POM.SPPO$ESTADO
                            ,(SELECT SPFC$FECHA_CIERRE FROM SCHE$SIPLAN20.SP20$FECHAS_CIERRE WHERE SPFC$TIPO_FECHA = 2 AND SPFC$RESTRICTIVA = 'N' AND SPFC$PERIODO_POM = POM.SPPO$ID_PERIODO AND SPFC$EJERCICIO = POA.SPOA$ANIO AND SPFC$POM = -1 AND SPFC$INSTITUCION = -1) FECHA_CIERRE
                            ,(SELECT COUNT(SPFC$FECHA_CIERRE) CONTEO FROM SCHE$SIPLAN20.SP20$FECHAS_CIERRE WHERE SPFC$TIPO_FECHA = 2 AND SPFC$RESTRICTIVA = 'N' AND SPFC$PERIODO_POM = POM.SPPO$ID_PERIODO AND SPFC$POM = -1 AND SPFC$INSTITUCION = -1) CONTEO

                            ,(SELECT MAX(SPFC$FECHA_CIERRE) FROM SCHE$SIPLAN20.SP20$FECHAS_CIERRE WHERE SPFC$TIPO_FECHA = 2 AND SPFC$RESTRICTIVA = 'N' AND SPFC$PERIODO_POM = POM.SPPO$ID_PERIODO AND SPFC$EJERCICIO = POA.SPOA$ANIO AND SPFC$POM = POM.SPPO$ID_POM AND  SPFC$INSTITUCION = POM.SPPO$ID_INSTITUCION) FECHA_PRORROGA
                            ,(SELECT COUNT(SPFC$FECHA_CIERRE) CONTEO FROM SCHE$SIPLAN20.SP20$FECHAS_CIERRE WHERE SPFC$TIPO_FECHA = 2 AND SPFC$RESTRICTIVA = 'N' AND SPFC$PERIODO_POM = POM.SPPO$ID_PERIODO AND SPFC$EJERCICIO = POA.SPOA$ANIO AND SPFC$POM = POM.SPPO$ID_POM AND  SPFC$INSTITUCION = POM.SPPO$ID_INSTITUCION) CONTEO_PRORROGA

                            FROM SCHE$SIPLAN20.SP20$POA POA
                            INNER JOIN SCHE$SIPLAN20.SP20$POM POM ON POM.SPPO$ID_POM = POA.SPOA$ID_POM AND POM.SPPO$ID_INSTITUCION = POA.SPOA$ID_INSTITUCION AND POM.SPPO$RESTRICTIVA = 'N' AND POA.SPOA$RESTRICTIVA = 'N'
                            WHERE POA.SPOA$ID_POA = " + poa + " AND POA.SPOA$ANIO = " + anio + " AND POA.SPOA$ID_POM = " + Session["pom"] + " AND POA.SPOA$ID_INSTITUCION = " + Session["insto"] + ")";
            estados = dao.consulta(sql);
            if (estados != 0)
            {
                tabla = dao.tabla;
                if (Convert.ToInt32(tabla.Rows[0]["ESTADO"]) == 0)
                {
                    //sql = "UPDATE SCHE$SIPLAN20.SP20$PROGRAMETA_FI_FIN_SUB SET SPPMFS$META = " + meta + ", SPPMFS$FECHA_UPDATE = 'UPDATE = ' || TO_CHAR(SYSDATE, 'DD/MM/YYYY HH:MI') || ' ' || '" + Session["USUARIO"].ToString() + "'  WHERE SPPMFS$ID_POA =" + poa + " AND SPPMFS$ANIO = " + anio + " AND SPPMFS$ID_SUBPRODUCTO = " + sub + " AND SPPMFS$ID_PROGRAMACION_FISFIN = " + id_meta + " AND SPPMFS$TIPO_PROGRAMACION = 1";
                    sql = "INSERT INTO SCHE$SIPLAN20.SP20$PROGRAMETA_FI_FIN_SUB (SPPMFS$META, SPPMFS$ID_POA, SPPMFS$ANIO, SPPMFS$ID_SUBPRODUCTO, SPPMFS$FECHA_INSERTA, SPPMFS$TIPO_PROGRAMACION) VALUES(" + meta + ", " + poa + ", " + anio + ", " + sub + ", 'INSERTA = ' || TO_CHAR(SYSDATE, 'DD/MM/YYYY HH:MI') || ' ' || '" + Session["USUARIO"].ToString() + "', 1)";
                    estados = dao.comando(sql);
                }

                else
                    estados = 1;
            }


            return estados;
        }

        protected int Eliminametafinanciera(int sub, int poa, int anio, int id_meta)
        {
            int estados = 0;
            DataTable tabla = new DataTable();
            //sql = "SELECT CASE WHEN SPOA$ESTADO = 'A' THEN  1 WHEN  SPOA$ESTADO = 'D'  THEN 0 END AS ESTADO FROM SCHE$SIPLAN20.SP20$POA WHERE SPOA$ID_POA = " + poa + " AND SPOA$ANIO = " + anio + " AND SPOA$ID_POM = " + Session["pom"] + " AND SPOA$ID_INSTITUCION = " + Session["insto"];
            sql = @"SELECT 
                        CASE WHEN SPPO$ESTADO = 'A' THEN 1 
                        WHEN SPPO$ESTADO = 'D' AND CONTEO_PRORROGA > 0 AND TRUNC(SYSDATE) <= TRUNC(FECHA_PRORROGA) THEN 0 
                        WHEN SPPO$ESTADO = 'D' AND CONTEO = 0 THEN 1 
                        WHEN SPPO$ESTADO = 'D' AND CONTEO > 0 AND TRUNC(SYSDATE) > TRUNC(FECHA_CIERRE) THEN 1
                        WHEN SPPO$ESTADO = 'D' AND CONTEO > 0 AND TRUNC(SYSDATE) <= TRUNC(FECHA_CIERRE) THEN 0
                        WHEN SPPO$ESTADO = 'D' AND CONTEO > 0 AND FECHA_CIERRE IS NULL THEN 0
                        END AS ESTADO
                        FROM
                        (SELECT 
                             POM.SPPO$ESTADO
                            ,(SELECT SPFC$FECHA_CIERRE FROM SCHE$SIPLAN20.SP20$FECHAS_CIERRE WHERE SPFC$TIPO_FECHA = 2 AND SPFC$RESTRICTIVA = 'N' AND SPFC$PERIODO_POM = POM.SPPO$ID_PERIODO AND SPFC$EJERCICIO = POA.SPOA$ANIO AND SPFC$POM = -1 AND SPFC$INSTITUCION = -1) FECHA_CIERRE
                            ,(SELECT COUNT(SPFC$FECHA_CIERRE) CONTEO FROM SCHE$SIPLAN20.SP20$FECHAS_CIERRE WHERE SPFC$TIPO_FECHA = 2 AND SPFC$RESTRICTIVA = 'N' AND SPFC$PERIODO_POM = POM.SPPO$ID_PERIODO AND SPFC$POM = -1 AND SPFC$INSTITUCION = -1)  CONTEO

                            ,(SELECT MAX(SPFC$FECHA_CIERRE) FROM SCHE$SIPLAN20.SP20$FECHAS_CIERRE WHERE SPFC$TIPO_FECHA = 2 AND SPFC$RESTRICTIVA = 'N' AND SPFC$PERIODO_POM = POM.SPPO$ID_PERIODO AND SPFC$EJERCICIO = POA.SPOA$ANIO AND SPFC$POM = POM.SPPO$ID_POM AND  SPFC$INSTITUCION = POM.SPPO$ID_INSTITUCION) FECHA_PRORROGA
                            ,(SELECT COUNT(SPFC$FECHA_CIERRE) CONTEO FROM SCHE$SIPLAN20.SP20$FECHAS_CIERRE WHERE SPFC$TIPO_FECHA = 2 AND SPFC$RESTRICTIVA = 'N' AND SPFC$PERIODO_POM = POM.SPPO$ID_PERIODO AND SPFC$EJERCICIO = POA.SPOA$ANIO AND SPFC$POM = POM.SPPO$ID_POM AND  SPFC$INSTITUCION = POM.SPPO$ID_INSTITUCION) CONTEO_PRORROGA

                            FROM SCHE$SIPLAN20.SP20$POA POA
                            INNER JOIN SCHE$SIPLAN20.SP20$POM POM ON POM.SPPO$ID_POM = POA.SPOA$ID_POM AND POM.SPPO$ID_INSTITUCION = POA.SPOA$ID_INSTITUCION AND POM.SPPO$RESTRICTIVA = 'N' AND POA.SPOA$RESTRICTIVA = 'N'
                            WHERE POA.SPOA$ID_POA = " + poa + " AND POA.SPOA$ANIO = " + anio + " AND POA.SPOA$ID_POM = " + Session["pom"] + " AND POA.SPOA$ID_INSTITUCION = " + Session["insto"] + ")";
            estados = dao.consulta(sql);
            if (estado != 0)
            {
                tabla = dao.tabla;
                if (Convert.ToInt32(tabla.Rows[0]["ESTADO"]) == 0)
                {
                    sql = "UPDATE SCHE$SIPLAN20.SP20$PROGRAMETA_FI_FIN_SUB SET SPPMFS$RESTRICTIVA = 'S', SPPMFS$FECHA_DELETE = 'DELETE = ' || TO_CHAR(SYSDATE, 'DD/MM/YYYY HH:MI') || ' ' || '" + Session["USUARIO"].ToString() + "'  WHERE SPPMFS$ID_POA =" + poa + " AND SPPMFS$ANIO = " + anio + " AND SPPMFS$ID_SUBPRODUCTO = " + sub + " AND SPPMFS$ID_PROGRAMACION_FISFIN = " + id_meta + " AND SPPMFS$TIPO_PROGRAMACION = 1";
                    estados = dao.comando(sql);
                }
                else
                {
                    estados = 1;
                }

            }

            return estados;
        }

        protected int Acualizametafisicasub(int sub, int poa, int anio, double meta, int id_meta)
        {
            int estados = 0;
            DataTable tabla = new DataTable();
            //sql = "SELECT CASE WHEN SPOA$ESTADO = 'A' THEN  1 WHEN  SPOA$ESTADO = 'D'  THEN 0 END AS ESTADO FROM SCHE$SIPLAN20.SP20$POA WHERE SPOA$ID_POA = " + poa + " AND SPOA$ANIO = " + anio + " AND SPOA$ID_POM = " + Session["pom"] + " AND SPOA$ID_INSTITUCION = " + Session["insto"];
            sql = @"SELECT 
                        CASE WHEN SPPO$ESTADO = 'A' THEN 1 
                        WHEN SPPO$ESTADO = 'D' AND CONTEO_PRORROGA > 0 AND TRUNC(SYSDATE) <= TRUNC(FECHA_PRORROGA) THEN 0 
                        WHEN SPPO$ESTADO = 'D' AND CONTEO = 0 THEN 1 
                        WHEN SPPO$ESTADO = 'D' AND CONTEO > 0 AND TRUNC(SYSDATE) > TRUNC(FECHA_CIERRE) THEN 1
                        WHEN SPPO$ESTADO = 'D' AND CONTEO > 0 AND TRUNC(SYSDATE) <= TRUNC(FECHA_CIERRE) THEN 0
                        WHEN SPPO$ESTADO = 'D' AND CONTEO > 0 AND FECHA_CIERRE IS NULL THEN 0
                        END AS ESTADO
                        FROM
                        (SELECT 
                             POM.SPPO$ESTADO
                            ,(SELECT SPFC$FECHA_CIERRE FROM SCHE$SIPLAN20.SP20$FECHAS_CIERRE WHERE SPFC$TIPO_FECHA = 2 AND SPFC$RESTRICTIVA = 'N' AND SPFC$PERIODO_POM = POM.SPPO$ID_PERIODO AND SPFC$EJERCICIO = POA.SPOA$ANIO AND SPFC$POM = -1 AND SPFC$INSTITUCION = -1) FECHA_CIERRE
                            ,(SELECT COUNT(SPFC$FECHA_CIERRE) CONTEO FROM SCHE$SIPLAN20.SP20$FECHAS_CIERRE WHERE SPFC$TIPO_FECHA = 2 AND SPFC$RESTRICTIVA = 'N' AND SPFC$PERIODO_POM = POM.SPPO$ID_PERIODO AND SPFC$POM = -1 AND SPFC$INSTITUCION = -1) CONTEO

                            ,(SELECT MAX(SPFC$FECHA_CIERRE) FROM SCHE$SIPLAN20.SP20$FECHAS_CIERRE WHERE SPFC$TIPO_FECHA = 2 AND SPFC$RESTRICTIVA = 'N' AND SPFC$PERIODO_POM = POM.SPPO$ID_PERIODO AND SPFC$EJERCICIO = POA.SPOA$ANIO AND SPFC$POM = POM.SPPO$ID_POM AND  SPFC$INSTITUCION = POM.SPPO$ID_INSTITUCION) FECHA_PRORROGA
                            ,(SELECT COUNT(SPFC$FECHA_CIERRE) CONTEO FROM SCHE$SIPLAN20.SP20$FECHAS_CIERRE WHERE SPFC$TIPO_FECHA = 2 AND SPFC$RESTRICTIVA = 'N' AND SPFC$PERIODO_POM = POM.SPPO$ID_PERIODO AND SPFC$EJERCICIO = POA.SPOA$ANIO AND SPFC$POM = POM.SPPO$ID_POM AND  SPFC$INSTITUCION = POM.SPPO$ID_INSTITUCION) CONTEO_PRORROGA

                            FROM SCHE$SIPLAN20.SP20$POA POA
                            INNER JOIN SCHE$SIPLAN20.SP20$POM POM ON POM.SPPO$ID_POM = POA.SPOA$ID_POM AND POM.SPPO$ID_INSTITUCION = POA.SPOA$ID_INSTITUCION AND POM.SPPO$RESTRICTIVA = 'N' AND POA.SPOA$RESTRICTIVA = 'N'
                            WHERE POA.SPOA$ID_POA = " + poa + " AND POA.SPOA$ANIO = " + anio + " AND POA.SPOA$ID_POM = " + Session["pom"] + " AND POA.SPOA$ID_INSTITUCION = " + Session["insto"] + ")";
            estados = dao.consulta(sql);
            if (estado != 0)
            {
                tabla = dao.tabla;
                if (Convert.ToInt32(tabla.Rows[0]["ESTADO"]) == 0)
                {
                    sql = "INSERT INTO SCHE$SIPLAN20.SP20$PROGRAMETA_FI_FIN_SUB (SPPMFS$META, SPPMFS$ID_POA, SPPMFS$ANIO, SPPMFS$ID_SUBPRODUCTO, SPPMFS$FECHA_INSERTA, SPPMFS$TIPO_PROGRAMACION) VALUES(" + meta + ", " + poa + ", " + anio + ", " + sub + ", 'INSERTA = ' || TO_CHAR(SYSDATE, 'DD/MM/YYYY HH:MI') || ' ' || '" + Session["USUARIO"].ToString() + "', 0)";
                    //sql = "UPDATE SCHE$SIPLAN20.SP20$PROGRAMETA_FI_FIN_SUB SET SPPMFS$META = " + meta + ", SPPMFS$FECHA_UPDATE = 'UPDATE = ' || TO_CHAR(SYSDATE, 'DD/MM/YYYY HH:MI') || ' ' || '" + Session["USUARIO"].ToString() + "'  WHERE SPPMFS$ID_POA =" + poa + " AND SPPMFS$ANIO = " + anio + " AND SPPMFS$ID_SUBPRODUCTO = " + sub + " AND SPPMFS$ID_PROGRAMACION_FISFIN = " + id_meta + " AND SPPMFS$TIPO_PROGRAMACION = 0";
                    estados = dao.comando(sql);
                }
                else
                    estados = 1;

            }

            return estados;
        }

        protected int Eliminafisicasub(int sub, int poa, int anio, int id_meta)
        {
            int estados = 0;
            DataTable tabla = new DataTable();
            //sql = "SELECT CASE WHEN SPOA$ESTADO = 'A' THEN  1 WHEN  SPOA$ESTADO = 'D'  THEN 0 END AS ESTADO FROM SCHE$SIPLAN20.SP20$POA WHERE SPOA$ID_POA = " + poa + " AND SPOA$ANIO = " + anio + " AND SPOA$ID_POM = " + Session["pom"] + " AND SPOA$ID_INSTITUCION = " + Session["insto"];
            sql = @"SELECT 
                        CASE WHEN SPPO$ESTADO = 'A' THEN 1 
                        WHEN SPPO$ESTADO = 'D' AND CONTEO_PRORROGA > 0 AND TRUNC(SYSDATE) <= TRUNC(FECHA_PRORROGA) THEN 0 
                        WHEN SPPO$ESTADO = 'D' AND CONTEO = 0 THEN 1 
                        WHEN SPPO$ESTADO = 'D' AND CONTEO > 0 AND TRUNC(SYSDATE) > TRUNC(FECHA_CIERRE) THEN 1
                        WHEN SPPO$ESTADO = 'D' AND CONTEO > 0 AND TRUNC(SYSDATE) <= TRUNC(FECHA_CIERRE) THEN 0
                        WHEN SPPO$ESTADO = 'D' AND CONTEO > 0 AND FECHA_CIERRE IS NULL THEN 0
                        END AS ESTADO
                        FROM
                        (SELECT 
                             POM.SPPO$ESTADO
                            ,(SELECT SPFC$FECHA_CIERRE FROM SCHE$SIPLAN20.SP20$FECHAS_CIERRE WHERE SPFC$TIPO_FECHA = 2 AND SPFC$RESTRICTIVA = 'N' AND SPFC$PERIODO_POM = POM.SPPO$ID_PERIODO AND SPFC$EJERCICIO = POA.SPOA$ANIO AND SPFC$INSTITUCION = -1 AND SPFC$POM = -1) FECHA_CIERRE
                            ,(SELECT COUNT(SPFC$FECHA_CIERRE) CONTEO FROM SCHE$SIPLAN20.SP20$FECHAS_CIERRE WHERE SPFC$TIPO_FECHA = 2 AND SPFC$RESTRICTIVA = 'N' AND SPFC$PERIODO_POM = POM.SPPO$ID_PERIODO AND SPFC$INSTITUCION = -1 AND SPFC$POM = -1) CONTEO

                            ,(SELECT MAX(SPFC$FECHA_CIERRE) FROM SCHE$SIPLAN20.SP20$FECHAS_CIERRE WHERE SPFC$TIPO_FECHA = 2 AND SPFC$RESTRICTIVA = 'N' AND SPFC$PERIODO_POM = POM.SPPO$ID_PERIODO AND SPFC$EJERCICIO = POA.SPOA$ANIO AND SPFC$POM = POM.SPPO$ID_POM AND  SPFC$INSTITUCION = POM.SPPO$ID_INSTITUCION) FECHA_PRORROGA
                            ,(SELECT COUNT(SPFC$FECHA_CIERRE) CONTEO FROM SCHE$SIPLAN20.SP20$FECHAS_CIERRE WHERE SPFC$TIPO_FECHA = 2 AND SPFC$RESTRICTIVA = 'N' AND SPFC$PERIODO_POM = POM.SPPO$ID_PERIODO AND SPFC$EJERCICIO = POA.SPOA$ANIO AND SPFC$POM = POM.SPPO$ID_POM AND  SPFC$INSTITUCION = POM.SPPO$ID_INSTITUCION) CONTEO_PRORROGA

                            FROM SCHE$SIPLAN20.SP20$POA POA
                            INNER JOIN SCHE$SIPLAN20.SP20$POM POM ON POM.SPPO$ID_POM = POA.SPOA$ID_POM AND POM.SPPO$ID_INSTITUCION = POA.SPOA$ID_INSTITUCION AND POM.SPPO$RESTRICTIVA = 'N' AND POA.SPOA$RESTRICTIVA = 'N'
                            WHERE POA.SPOA$ID_POA = " + poa + " AND POA.SPOA$ANIO = " + anio + " AND POA.SPOA$ID_POM = " + Session["pom"] + " AND POA.SPOA$ID_INSTITUCION = " + Session["insto"] + ")";

            estados = dao.consulta(sql);
            if (estados != 0)
            {
                tabla = dao.tabla;
                if (Convert.ToInt32(tabla.Rows[0]["ESTADO"]) == 0)
                {
                    sql = "UPDATE SCHE$SIPLAN20.SP20$PROGRAMETA_FI_FIN_SUB SET SPPMFS$RESTRICTIVA =  'S', SPPMFS$FECHA_DELETE = 'DELETE = ' || TO_CHAR(SYSDATE, 'DD/MM/YYYY HH:MI') || ' ' || '" + Session["USUARIO"].ToString() + "'  WHERE SPPMFS$ID_POA =" + poa + " AND SPPMFS$ANIO = " + anio + " AND SPPMFS$ID_SUBPRODUCTO = " + sub + " AND SPPMFS$ID_PROGRAMACION_FISFIN = " + id_meta + " AND SPPMFS$TIPO_PROGRAMACION = 0";
                    estados = dao.comando(sql);
                }
                else
                    estados = 1;

            }

            return estados;
        }

        protected int AcualizametasubPOA(int sub, int poa, int anio, double meta, int id_meta, int tipo, int mes)
        {
            int estados = 0;

            sql = "UPDATE SCHE$SIPLAN20.SP20$POASUBPRODUCTOS SET SPOAS$META = " + meta + ", SPOAS$FECHA_UPDATE = 'UPDATE = ' || TO_CHAR(SYSDATE, 'DD/MM/YYYY HH:MI') || ' ' || '" + Session["USUARIO"].ToString() + "'  WHERE SPOAS$POA =" + poa + " AND SPOAS$ANIO = " + anio + " AND SPOAS$SUBPRODUCTO = " + sub + " AND SPOAS$IDSPROGRA = " + id_meta + " AND SPOAS$MES = " + mes + " AND SPOAS$TIPO = " + tipo;
            estados = dao.comando(sql);
            return estados;
        }
        protected int AcualizametafiscaProd(int producto, int poa, int anio, double meta, int id_meta)
        {
            int estados = 0;
            //sql = "SELECT CASE WHEN SPOA$ESTADO = 'A' THEN  1 WHEN  SPOA$ESTADO = 'D'  THEN 0 END AS ESTADO FROM SCHE$SIPLAN20.SP20$POA WHERE SPOA$ID_POA = " + poa + " AND SPOA$ANIO = " + anio + " AND SPOA$ID_POM = " + Session["pom"] + " AND SPOA$ID_INSTITUCION = " + Session["insto"];
            sql = @"SELECT 
                        CASE WHEN SPPO$ESTADO = 'A' THEN 1 
                        WHEN SPPO$ESTADO = 'D' AND CONTEO_PRORROGA > 0 AND TRUNC(SYSDATE) <= TRUNC(FECHA_PRORROGA) THEN 0 
                        WHEN SPPO$ESTADO = 'D' AND CONTEO = 0 THEN 1 
                        WHEN SPPO$ESTADO = 'D' AND CONTEO > 0 AND TRUNC(SYSDATE) > TRUNC(FECHA_CIERRE) THEN 1
                        WHEN SPPO$ESTADO = 'D' AND CONTEO > 0 AND TRUNC(SYSDATE) <= TRUNC(FECHA_CIERRE) THEN 0
                        WHEN SPPO$ESTADO = 'D' AND CONTEO > 0 AND FECHA_CIERRE IS NULL THEN 0
                        END AS ESTADO
                        FROM
                        (SELECT 
                             POM.SPPO$ESTADO
                            ,(SELECT SPFC$FECHA_CIERRE FROM SCHE$SIPLAN20.SP20$FECHAS_CIERRE WHERE SPFC$TIPO_FECHA = 2 AND SPFC$RESTRICTIVA = 'N' AND SPFC$PERIODO_POM = POM.SPPO$ID_PERIODO AND SPFC$EJERCICIO = POA.SPOA$ANIO AND SPFC$POM = -1 AND SPFC$INSTITUCION = -1) FECHA_CIERRE
                            ,(SELECT COUNT(SPFC$FECHA_CIERRE) CONTEO FROM SCHE$SIPLAN20.SP20$FECHAS_CIERRE WHERE SPFC$TIPO_FECHA = 2 AND SPFC$RESTRICTIVA = 'N' AND SPFC$PERIODO_POM = POM.SPPO$ID_PERIODO AND SPFC$POM = -1 AND SPFC$INSTITUCION = -1) CONTEO

                            ,(SELECT MAX(SPFC$FECHA_CIERRE) FROM SCHE$SIPLAN20.SP20$FECHAS_CIERRE WHERE SPFC$TIPO_FECHA = 2 AND SPFC$RESTRICTIVA = 'N' AND SPFC$PERIODO_POM = POM.SPPO$ID_PERIODO AND SPFC$EJERCICIO = POA.SPOA$ANIO AND SPFC$POM = POM.SPPO$ID_POM AND  SPFC$INSTITUCION = POM.SPPO$ID_INSTITUCION) FECHA_PRORROGA
                            ,(SELECT COUNT(SPFC$FECHA_CIERRE) CONTEO FROM SCHE$SIPLAN20.SP20$FECHAS_CIERRE WHERE SPFC$TIPO_FECHA = 2 AND SPFC$RESTRICTIVA = 'N' AND SPFC$PERIODO_POM = POM.SPPO$ID_PERIODO AND SPFC$EJERCICIO = POA.SPOA$ANIO AND SPFC$POM = POM.SPPO$ID_POM AND  SPFC$INSTITUCION = POM.SPPO$ID_INSTITUCION) CONTEO_PRORROGA

                            FROM SCHE$SIPLAN20.SP20$POA POA
                            INNER JOIN SCHE$SIPLAN20.SP20$POM POM ON POM.SPPO$ID_POM = POA.SPOA$ID_POM AND POM.SPPO$ID_INSTITUCION = POA.SPOA$ID_INSTITUCION AND POM.SPPO$RESTRICTIVA = 'N' AND POA.SPOA$RESTRICTIVA = 'N'
                            WHERE POA.SPOA$ID_POA = " + poa+" AND POA.SPOA$ANIO = "+anio+" AND POA.SPOA$ID_POM = "+Session["pom"]+" AND POA.SPOA$ID_INSTITUCION = "+ Session["insto"] + ")";
            estado = dao.consulta(sql);
            if (estado != 0)
            {
                tabla = dao.tabla;
                if (Convert.ToInt32(tabla.Rows[0]["ESTADO"]) == 0)
                {
                    //sql = "UPDATE SCHE$SIPLAN20.SP20$PROGRAMACION_MFPRODUCTO SET SPPMFS$META = " + meta + ", SPPMFS$FECHA_UPDATE = 'UPDATE = ' || TO_CHAR(SYSDATE, 'DD/MM/YYYY HH:MI') || ' ' || '" + Session["USUARIO"].ToString() + "'  WHERE SPPMFS$ID_POA =" + poa + " AND SPPMFS$ANIO = " + anio + " AND SPPMFS$ID_PRODUCTO = " + producto + " AND SPPMFS$ID_PROGRAMACION_FISICA = " + id_meta;
                    sql = "INSERT INTO SCHE$SIPLAN20.SP20$PROGRAMACION_MFPRODUCTO (SPPMFS$META, SPPMFS$ID_POA, SPPMFS$ANIO, SPPMFS$ID_PRODUCTO, SPPMFS$FECHA_INSERTA) VALUES(" + meta + ", " + poa + ", " + anio + ", " + producto + ", 'INSERTA = ' || TO_CHAR(SYSDATE, 'DD/MM/YYYY HH:MI') || ' ' || '" + Session["USUARIO"].ToString() + "')";
                    estados = dao.comando(sql);
                }

                estado = 1;

            }

            else
                estado = 0;

            return estados;
        }

        protected int eliminaMetafisica(int producto, int poa, int anio, int id_meta)
        {
            DataTable tabla = new DataTable();

            int estado = 0;
            //sql = "SELECT CASE WHEN SPOA$ESTADO = 'A' THEN  1 WHEN  SPOA$ESTADO = 'D'  THEN 0 END AS ESTADO FROM SCHE$SIPLAN20.SP20$POA WHERE SPOA$ID_POA = " + poa + " AND SPOA$ANIO = " + anio + " AND SPOA$ID_POM = " + Session["pom"] + " AND SPOA$ID_INSTITUCION = " + Session["insto"];
            sql = @"SELECT 
                        CASE WHEN SPPO$ESTADO = 'A' THEN 1 
                        WHEN SPPO$ESTADO = 'D' AND CONTEO_PRORROGA > 0 AND TRUNC(SYSDATE) <= TRUNC(FECHA_PRORROGA) THEN 0 
                        WHEN SPPO$ESTADO = 'D' AND CONTEO = 0 THEN 1 
                        WHEN SPPO$ESTADO = 'D' AND CONTEO > 0 AND TRUNC(SYSDATE) > TRUNC(FECHA_CIERRE) THEN 1
                        WHEN SPPO$ESTADO = 'D' AND CONTEO > 0 AND TRUNC(SYSDATE) <= TRUNC(FECHA_CIERRE) THEN 0
                        WHEN SPPO$ESTADO = 'D' AND CONTEO > 0 AND FECHA_CIERRE IS NULL THEN 0
                        END AS ESTADO
                        FROM
                        (SELECT 
                             POM.SPPO$ESTADO
                            ,(SELECT SPFC$FECHA_CIERRE FROM SCHE$SIPLAN20.SP20$FECHAS_CIERRE WHERE SPFC$TIPO_FECHA = 2 AND SPFC$RESTRICTIVA = 'N' AND SPFC$PERIODO_POM = POM.SPPO$ID_PERIODO AND SPFC$EJERCICIO = POA.SPOA$ANIO AND SPFC$POM = -1 AND SPFC$INSTITUCION = -1) FECHA_CIERRE
                            ,(SELECT COUNT(SPFC$FECHA_CIERRE) CONTEO FROM SCHE$SIPLAN20.SP20$FECHAS_CIERRE WHERE SPFC$TIPO_FECHA = 2 AND SPFC$RESTRICTIVA = 'N' AND SPFC$PERIODO_POM = POM.SPPO$ID_PERIODO AND SPFC$POM = -1 AND SPFC$INSTITUCION = -1) CONTEO

                            ,(SELECT MAX(SPFC$FECHA_CIERRE) FROM SCHE$SIPLAN20.SP20$FECHAS_CIERRE WHERE SPFC$TIPO_FECHA = 2 AND SPFC$RESTRICTIVA = 'N' AND SPFC$PERIODO_POM = POM.SPPO$ID_PERIODO AND SPFC$EJERCICIO = POA.SPOA$ANIO AND SPFC$POM = POM.SPPO$ID_POM AND  SPFC$INSTITUCION = POM.SPPO$ID_INSTITUCION) FECHA_PRORROGA
                            ,(SELECT COUNT(SPFC$FECHA_CIERRE) CONTEO FROM SCHE$SIPLAN20.SP20$FECHAS_CIERRE WHERE SPFC$TIPO_FECHA = 2 AND SPFC$RESTRICTIVA = 'N' AND SPFC$PERIODO_POM = POM.SPPO$ID_PERIODO AND SPFC$EJERCICIO = POA.SPOA$ANIO AND SPFC$POM = POM.SPPO$ID_POM AND  SPFC$INSTITUCION = POM.SPPO$ID_INSTITUCION) CONTEO_PRORROGA

                            FROM SCHE$SIPLAN20.SP20$POA POA
                            INNER JOIN SCHE$SIPLAN20.SP20$POM POM ON POM.SPPO$ID_POM = POA.SPOA$ID_POM AND POM.SPPO$ID_INSTITUCION = POA.SPOA$ID_INSTITUCION AND POM.SPPO$RESTRICTIVA = 'N' AND POA.SPOA$RESTRICTIVA = 'N'
                            WHERE POA.SPOA$ID_POA = " + poa + " AND POA.SPOA$ANIO = " + anio + " AND POA.SPOA$ID_POM = " + Session["pom"] + " AND POA.SPOA$ID_INSTITUCION = " + Session["insto"] + ")";
            estado = dao.consulta(sql);
            if (estado != 0)
            {
                tabla = dao.tabla;
                if (Convert.ToInt32(tabla.Rows[0]["ESTADO"]) == 0)
                {
                    sql = "UPDATE SCHE$SIPLAN20.SP20$PROGRAMACION_MFPRODUCTO SET SPPMFS$RESTRICTIVA = 'S', SPPMFS$FECHA_DELETE = 'DELETE = ' || TO_CHAR(SYSDATE, 'DD/MM/YYYY HH:MI') || ' ' || '" + Session["USUARIO"].ToString() + "'  WHERE SPPMFS$ID_POA =" + poa + " AND SPPMFS$ANIO = " + anio + " AND SPPMFS$ID_PRODUCTO = " + producto + " AND SPPMFS$ID_PROGRAMACION_FISICA = " + id_meta;
                    estado = dao.comando(sql);
                }
                else
                    estado = 1;

            }



            return estado;
        }

        protected DataTable obtienepoa(int pom, int insto, int anio)
        {
            DataTable poas = new DataTable();
            sql = "SELECT * FROM SCHE$SIPLAN20.SP20$POA WHERE SPOA$ID_POM = " + pom + " AND SPOA$ID_INSTITUCION = " + insto + " AND SPOA$ANIO = " + anio + " AND SPOA$RESTRICTIVA = 'N'";
            estado = dao.consulta(sql);
            if (estado != 0)
                poas = dao.tabla;
            return poas;
        }



        protected void ActuAsigna_Click(object sender, EventArgs e)
        {
            btnMetas_Click(sender, e);
        }



        protected void cbTipoProduccion_ValueChanged(object sender, EventArgs e)
        {
            Session["carga"] = 1;
            //CollapseAllDetailRows();
            //gvProduccion.DetailRows.CollapseAllRows();
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, GetType(), "script", "onCollapseClick();", true);


        }

        protected void gvProductos_BeforePerformDataSelect(object sender, EventArgs e)
        {
            Session["carga"] = 1;
            ASPxGridView gvDetail = sender as ASPxGridView;
            DataTable productos = new DataTable();
            int resultado;
            int resultado2;

            Double programa;
            resultado = Convert.ToInt32((sender as ASPxGridView).GetMasterRowFieldValues("ID_RESULTADO"));
            resultado2 = Convert.ToInt32((sender as ASPxGridView).GetMasterRowFieldValues("SPPRO$RESULTADO2"));
            programa = Convert.ToDouble((sender as ASPxGridView).GetMasterRowFieldValues("SPPRO$ID_PROGRAMA_PRESUPUESTO"));
            Session["IDRESULTADO"] = resultado;
            Session["IDPROGRAMA"] = programa;



            if (Convert.ToInt32(Session["periodo"]) == 0)
            {
                gvDetail.Columns["MF2025"].Visible = false;
                gvDetail.Columns["MFIN2025"].Visible = false;
                GridViewBandColumn bandCol = gvDetail.Columns["2025"] as GridViewBandColumn;
                bandCol.Visible = false;

                gvDetail.Columns["MF2026"].Visible = false;
                gvDetail.Columns["MFIN2026"].Visible = false;
                GridViewBandColumn bandCol1 = gvDetail.Columns["2026"] as GridViewBandColumn;
                bandCol1.Visible = false;

                gvDetail.Columns["MF2027"].Visible = false;
                gvDetail.Columns["MFIN2027"].Visible = false;
                GridViewBandColumn bandCol3 = gvDetail.Columns["2027"] as GridViewBandColumn;
                bandCol3.Visible = false;

                gvDetail.Columns["MF2028"].Visible = false;
                gvDetail.Columns["MFIN2028"].Visible = false;
                GridViewBandColumn bandCol4 = gvDetail.Columns["2028"] as GridViewBandColumn;
                bandCol4.Visible = false;

                gvDetail.Columns["MF2029"].Visible = false;
                gvDetail.Columns["MFIN2029"].Visible = false;
                GridViewBandColumn bandCol5 = gvDetail.Columns["2029"] as GridViewBandColumn;
                bandCol5.Visible = false;


                gvDetail.Columns["MF2030"].Visible = false;
                gvDetail.Columns["MFIN2030"].Visible = false;
                GridViewBandColumn bandCol6 = gvDetail.Columns["2030"] as GridViewBandColumn;
                bandCol5.Visible = false;


            }

            else if (Convert.ToInt32(Session["periodo"]) == 20)
            {
                gvDetail.Columns["MF2020"].Visible = false;
                gvDetail.Columns["MFIN2020"].Visible = false;
                GridViewBandColumn bandCol = gvDetail.Columns["2020"] as GridViewBandColumn;
                bandCol.Visible = false;

                gvDetail.Columns["MF2026"].Visible = false;
                gvDetail.Columns["MFIN2026"].Visible = false;
                GridViewBandColumn bandCol1 = gvDetail.Columns["2026"] as GridViewBandColumn;
                bandCol1.Visible = false;

                gvDetail.Columns["MF2027"].Visible = false;
                gvDetail.Columns["MFIN2027"].Visible = false;
                GridViewBandColumn bandCol2 = gvDetail.Columns["2027"] as GridViewBandColumn;
                bandCol2.Visible = false;

                gvDetail.Columns["MF2028"].Visible = false;
                gvDetail.Columns["MFIN2028"].Visible = false;
                GridViewBandColumn bandCol3 = gvDetail.Columns["2028"] as GridViewBandColumn;
                bandCol3.Visible = false;

                gvDetail.Columns["MF2029"].Visible = false;
                gvDetail.Columns["MFIN2029"].Visible = false;
                GridViewBandColumn bandCol4 = gvDetail.Columns["2029"] as GridViewBandColumn;
                bandCol4.Visible = false;


                gvDetail.Columns["MF2030"].Visible = false;
                gvDetail.Columns["MFIN2030"].Visible = false;
                GridViewBandColumn bandCol5 = gvDetail.Columns["2030"] as GridViewBandColumn;
                bandCol5.Visible = false;


            }


            else if (Convert.ToInt32(Session["periodo"]) == 21)
            {
                gvDetail.Columns["MF2020"].Visible = false;
                gvDetail.Columns["MFIN2020"].Visible = false;
                GridViewBandColumn bandCol = gvDetail.Columns["2020"] as GridViewBandColumn;
                bandCol.Visible = false;

                gvDetail.Columns["MF2021"].Visible = false;
                gvDetail.Columns["MFIN2021"].Visible = false;
                GridViewBandColumn bandCol1 = gvDetail.Columns["2021"] as GridViewBandColumn;
                bandCol1.Visible = false;

                gvDetail.Columns["MF2027"].Visible = false;
                gvDetail.Columns["MFIN2027"].Visible = false;
                GridViewBandColumn bandCol2 = gvDetail.Columns["2027"] as GridViewBandColumn;
                bandCol2.Visible = false;

                gvDetail.Columns["MF2028"].Visible = false;
                gvDetail.Columns["MFIN2028"].Visible = false;
                GridViewBandColumn bandCol3 = gvDetail.Columns["2028"] as GridViewBandColumn;
                bandCol3.Visible = false;

                gvDetail.Columns["MF2029"].Visible = false;
                gvDetail.Columns["MFIN2029"].Visible = false;
                GridViewBandColumn bandCol4 = gvDetail.Columns["2029"] as GridViewBandColumn;
                bandCol4.Visible = false;

                gvDetail.Columns["MF2030"].Visible = false;
                gvDetail.Columns["MFIN2030"].Visible = false;
                GridViewBandColumn bandCol5 = gvDetail.Columns["2030"] as GridViewBandColumn;
                bandCol5.Visible = false;


            }


            else if (Convert.ToInt32(Session["periodo"]) == 22)
            {
                gvDetail.Columns["MF2020"].Visible = false;
                gvDetail.Columns["MFIN2020"].Visible = false;
                GridViewBandColumn bandCol = gvDetail.Columns["2020"] as GridViewBandColumn;
                bandCol.Visible = false;

                gvDetail.Columns["MF2021"].Visible = false;
                gvDetail.Columns["MFIN2021"].Visible = false;
                GridViewBandColumn bandCol1 = gvDetail.Columns["2021"] as GridViewBandColumn;
                bandCol1.Visible = false;

                gvDetail.Columns["MF2022"].Visible = false;
                gvDetail.Columns["MFIN2022"].Visible = false;
                GridViewBandColumn bandCol2 = gvDetail.Columns["2022"] as GridViewBandColumn;
                bandCol2.Visible = false;


                gvDetail.Columns["MF2028"].Visible = false;
                gvDetail.Columns["MFIN2028"].Visible = false;
                GridViewBandColumn bandCol3 = gvDetail.Columns["2028"] as GridViewBandColumn;
                bandCol3.Visible = false;

                gvDetail.Columns["MF2029"].Visible = false;
                gvDetail.Columns["MFIN2029"].Visible = false;
                GridViewBandColumn bandCol4 = gvDetail.Columns["2029"] as GridViewBandColumn;
                bandCol4.Visible = false;


                gvDetail.Columns["MF2030"].Visible = false;
                gvDetail.Columns["MFIN2030"].Visible = false;
                GridViewBandColumn bandCol5 = gvDetail.Columns["2030"] as GridViewBandColumn;
                bandCol5.Visible = false;

            }


            else if (Convert.ToInt32(Session["periodo"]) == 23)
            {
                gvDetail.Columns["MF2020"].Visible = false;
                gvDetail.Columns["MFIN2020"].Visible = false;
                GridViewBandColumn bandCol = gvDetail.Columns["2020"] as GridViewBandColumn;
                bandCol.Visible = false;

                gvDetail.Columns["MF2021"].Visible = false;
                gvDetail.Columns["MFIN2021"].Visible = false;
                GridViewBandColumn bandCol1 = gvDetail.Columns["2021"] as GridViewBandColumn;
                bandCol1.Visible = false;

                gvDetail.Columns["MF2022"].Visible = false;
                gvDetail.Columns["MFIN2022"].Visible = false;
                GridViewBandColumn bandCol2 = gvDetail.Columns["2022"] as GridViewBandColumn;
                bandCol2.Visible = false;


                gvDetail.Columns["MF2023"].Visible = false;
                gvDetail.Columns["MFIN2023"].Visible = false;
                GridViewBandColumn bandCol3 = gvDetail.Columns["2023"] as GridViewBandColumn;
                bandCol3.Visible = false;

                gvDetail.Columns["MF2029"].Visible = false;
                gvDetail.Columns["MFIN2029"].Visible = false;
                GridViewBandColumn bandCol4 = gvDetail.Columns["2029"] as GridViewBandColumn;
                bandCol4.Visible = false;

                gvDetail.Columns["MF2030"].Visible = false;
                gvDetail.Columns["MFIN2030"].Visible = false;
                GridViewBandColumn bandCol5 = gvDetail.Columns["2030"] as GridViewBandColumn;
                bandCol5.Visible = false;

            }



            else if (Convert.ToInt32(Session["periodo"]) == 24)
            {
                gvDetail.Columns["MF2020"].Visible = false;
                gvDetail.Columns["MFIN2020"].Visible = false;
                GridViewBandColumn bandCol = gvDetail.Columns["2020"] as GridViewBandColumn;
                bandCol.Visible = false;

                gvDetail.Columns["MF2021"].Visible = false;
                gvDetail.Columns["MFIN2021"].Visible = false;
                GridViewBandColumn bandCol1 = gvDetail.Columns["2021"] as GridViewBandColumn;
                bandCol1.Visible = false;

                gvDetail.Columns["MF2022"].Visible = false;
                gvDetail.Columns["MFIN2022"].Visible = false;
                GridViewBandColumn bandCol2 = gvDetail.Columns["2022"] as GridViewBandColumn;
                bandCol2.Visible = false;


                gvDetail.Columns["MF2023"].Visible = false;
                gvDetail.Columns["MFIN2023"].Visible = false;
                GridViewBandColumn bandCol3 = gvDetail.Columns["2023"] as GridViewBandColumn;
                bandCol3.Visible = false;

                gvDetail.Columns["MF2024"].Visible = false;
                gvDetail.Columns["MFIN2024"].Visible = false;
                GridViewBandColumn bandCol4 = gvDetail.Columns["2024"] as GridViewBandColumn;
                bandCol4.Visible = false;

                gvDetail.Columns["MF2030"].Visible = false;
                gvDetail.Columns["MFIN2030"].Visible = false;
                GridViewBandColumn bandCol5 = gvDetail.Columns["2030"] as GridViewBandColumn;
                bandCol5.Visible = false;

            }


            else if (Convert.ToInt32(Session["periodo"]) == 25)
            {
                gvDetail.Columns["MF2020"].Visible = false;
                gvDetail.Columns["MFIN2020"].Visible = false;
                GridViewBandColumn bandCol = gvDetail.Columns["2020"] as GridViewBandColumn;
                bandCol.Visible = false;

                gvDetail.Columns["MF2021"].Visible = false;
                gvDetail.Columns["MFIN2021"].Visible = false;
                GridViewBandColumn bandCol1 = gvDetail.Columns["2021"] as GridViewBandColumn;
                bandCol1.Visible = false;

                gvDetail.Columns["MF2022"].Visible = false;
                gvDetail.Columns["MFIN2022"].Visible = false;
                GridViewBandColumn bandCol2 = gvDetail.Columns["2022"] as GridViewBandColumn;
                bandCol2.Visible = false;


                gvDetail.Columns["MF2023"].Visible = false;
                gvDetail.Columns["MFIN2023"].Visible = false;
                GridViewBandColumn bandCol3 = gvDetail.Columns["2023"] as GridViewBandColumn;
                bandCol3.Visible = false;

                gvDetail.Columns["MF2024"].Visible = false;
                gvDetail.Columns["MFIN2024"].Visible = false;
                GridViewBandColumn bandCol4 = gvDetail.Columns["2024"] as GridViewBandColumn;
                bandCol4.Visible = false;

                gvDetail.Columns["MF2025"].Visible = false;
                gvDetail.Columns["MFIN2025"].Visible = false;
                GridViewBandColumn bandCol5 = gvDetail.Columns["2025"] as GridViewBandColumn;
                bandCol5.Visible = false;

            }


            if (Session["poa"] != null)
            {
                productos = cargaProductos((DataTable)Session["poa"], Convert.ToInt32(cbTipoProduccion.Value), resultado, programa, Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), resultado2);
                Session["mfproductos"] = productos;
            }

            gvDetail.DataSource = productos;

        }

        protected DataTable cargaProductos(DataTable temp, int tipo, int resultado, Double programa, int pom, int insto, int resultado2)
        {
            DataTable productos = new DataTable();

            if (Convert.ToInt32(Session["periodo"]) == 20)
            {
                if (tipo == 0)
                {
                    sql = "SELECT ID_RESULTADO, SPPRO$ID_PROGRAMA_PRESUPUESTO, SPPRO$ID_PRODUCTO, PRODUCTO, SPPRO$ID_MEDIDA, MEDIDA_PRODUCTO,  NULL IDFP2020, NULL MF2020, NULL MFIN2020, NULL TP2020";
                    for (int i = 0; i < temp.Rows.Count; i++)
                    {
                        sql = sql + ",IDFP" + temp.Rows[i]["SPOA$ANIO"] + ", MF" + temp.Rows[i]["SPOA$ANIO"] + ", MFIN" + temp.Rows[i]["SPOA$ANIO"] + ", TP" + temp.Rows[i]["SPOA$ANIO"];
                    }
                    sql = sql + " ,NULL IDFP2026, NULL MF2026, NULL MFIN2026, NULL TP2026, NULL IDFP2027, NULL MF2027, NULL MFIN2027, NULL TP2027, NULL IDFP2028, NULL MF2028, NULL MFIN2028, NULL TP2028,   SPRES$POM FROM (";
                    sql = sql + "SELECT VES.ID_RESULTADO, VES.SPPRO$ID_PROGRAMA_PRESUPUESTO, VES.SPPRO$ID_PRODUCTO, VES.PRODUCTO, VES.SPPRO$ID_MEDIDA, VES.MEDIDA_PRODUCTO";
                    for (int i = 0; i < temp.Rows.Count; i++)
                    {
                        sql = sql + ",SCHE$SIPLAN20.FNC$OBTIENEIDMETAFISICAPRO(" + temp.Rows[i]["SPOA$ID_POA"] + ", " + temp.Rows[i]["SPOA$ANIO"] + ", VES.SPPRO$ID_PRODUCTO) IDFP" + temp.Rows[i]["SPOA$ANIO"] + ",SCHE$SIPLAN20.FNC$OBTIENEMETAFISICAPRO(" + temp.Rows[i]["SPOA$ID_POA"] + ", " + temp.Rows[i]["SPOA$ANIO"] + ", VES.SPPRO$ID_PRODUCTO) MF" + temp.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$OBTIENEMETAFINACIERAPROD(" + temp.Rows[i]["SPOA$ID_POA"] + ", " + temp.Rows[i]["SPOA$ANIO"] + ", VES.SPPRO$ID_PRODUCTO,0) MFIN" + temp.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$OBTIENETIPOPROGRA(" + temp.Rows[i]["SPOA$ID_POA"] + ", " + temp.Rows[i]["SPOA$ANIO"] + ", VES.SPPRO$ID_PRODUCTO) TP" + temp.Rows[i]["SPOA$ANIO"];
                    }


                    sql = sql + " ,VES.SPRES$POM ";
                    sql = sql + " FROM SCHE$SIPLAN20.SPPVST$SUBESTRATEGICOV2 VES WHERE VES.ID_RESULTADO =" + resultado + "  AND VES.SPPRO$ID_PROGRAMA_PRESUPUESTO = " + programa + " AND  VES.SPRES$POM = " + pom + " AND  VES.SPRES$INSTITUCION =  " + insto + " AND VES.SPPRO$RESULTADO2 = " + resultado2 + ") GROUP BY  ID_RESULTADO, SPPRO$ID_PROGRAMA_PRESUPUESTO, SPPRO$ID_PRODUCTO, PRODUCTO, SPPRO$ID_MEDIDA, MEDIDA_PRODUCTO ";
                    for (int i = 0; i < temp.Rows.Count; i++)
                    {
                        sql = sql + ",IDFP" + temp.Rows[i]["SPOA$ANIO"] + ", MF" + temp.Rows[i]["SPOA$ANIO"] + ", MFIN" + temp.Rows[i]["SPOA$ANIO"] + ", TP" + temp.Rows[i]["SPOA$ANIO"];
                    }
                    sql = sql + " ,SPRES$POM ORDER BY SPPRO$ID_PROGRAMA_PRESUPUESTO, SPPRO$ID_PRODUCTO ASC";
                }
                else if (tipo == 1)
                {
                    sql = " SELECT ID_RESULTADO, SPPRO$ID_PROGRAMA_PRESUPUESTO, SPPRO$ID_PRODUCTO, PRODUCTO, SPPRO$ID_MEDIDA, MEDIDA_PRODUCTO, NULL IDFP2020, NULL MF2020, NULL MFIN2020, NULL TP2020";
                    for (int i = 0; i < temp.Rows.Count; i++)
                    {
                        sql = sql + ",IDFP" + temp.Rows[i]["SPOA$ANIO"] + ", MF" + temp.Rows[i]["SPOA$ANIO"] + ", MFIN" + temp.Rows[i]["SPOA$ANIO"] + ", TP" + temp.Rows[i]["SPOA$ANIO"];
                    }
                    sql = sql + " ,NULL IDFP2026, NULL MF2026, NULL MFIN2026, NULL TP2026,    NULL IDFP2027, NULL MF2027, NULL MFIN2027, NULL TP2027, NULL IDFP2028, NULL MF2028, NULL MFIN2028, NULL TP2028, SPRES$POM FROM (";
                    sql = sql + "SELECT VES.ID_RESULTADO, VES.SPPRO$ID_PROGRAMA_PRESUPUESTO, VES.SPPRO$ID_PRODUCTO, VES.PRODUCTO, VES.SPPRO$ID_MEDIDA, VES.MEDIDA_PRODUCTO";
                    for (int i = 0; i < temp.Rows.Count; i++)
                    {
                        sql = sql + ",SCHE$SIPLAN20.FNC$OBTIENEIDMETAFISICAPRO(" + temp.Rows[i]["SPOA$ID_POA"] + ", " + temp.Rows[i]["SPOA$ANIO"] + ", VES.SPPRO$ID_PRODUCTO) IDFP" + temp.Rows[i]["SPOA$ANIO"] + ",SCHE$SIPLAN20.FNC$OBTIENEMETAFISICAPRO(" + temp.Rows[i]["SPOA$ID_POA"] + ", " + temp.Rows[i]["SPOA$ANIO"] + ", VES.SPPRO$ID_PRODUCTO) MF" + temp.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$OBTIENEMETAFINACIERAPROD(" + temp.Rows[i]["SPOA$ID_POA"] + ", " + temp.Rows[i]["SPOA$ANIO"] + ", VES.SPPRO$ID_PRODUCTO,0) MFIN" + temp.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$OBTIENETIPOPROGRA(" + temp.Rows[i]["SPOA$ID_POA"] + ", " + temp.Rows[i]["SPOA$ANIO"] + ", VES.SPPRO$ID_PRODUCTO) TP" + temp.Rows[i]["SPOA$ANIO"];
                    }


                    sql = sql + " ,VES.SPRES$POM ";
                    sql = sql + " FROM SCHE$SIPLAN20.SPPSVT$RED_INSTITUCIONAL VES WHERE VES.ID_RESULTADO =" + resultado + "  AND VES.SPPRO$ID_PROGRAMA_PRESUPUESTO = " + programa + " AND  VES.SPRES$POM = " + pom + " AND  VES.SPRES$INSTITUCION =  " + insto + " AND VES.SPPRO$RESULTADO2 = " + resultado2 + ") GROUP BY ID_RESULTADO, SPPRO$ID_PROGRAMA_PRESUPUESTO, SPPRO$ID_PRODUCTO, PRODUCTO, SPPRO$ID_MEDIDA, MEDIDA_PRODUCTO ";
                    for (int i = 0; i < temp.Rows.Count; i++)
                    {
                        sql = sql + ",IDFP" + temp.Rows[i]["SPOA$ANIO"] + ", MF" + temp.Rows[i]["SPOA$ANIO"] + ", MFIN" + temp.Rows[i]["SPOA$ANIO"] + ", TP" + temp.Rows[i]["SPOA$ANIO"];
                    }
                    sql = sql + " ,SPRES$POM ORDER BY SPPRO$ID_PROGRAMA_PRESUPUESTO, SPPRO$ID_PRODUCTO ASC";
                }
            }



            else if (Convert.ToInt32(Session["periodo"]) == 21)
            {
                if (tipo == 0)
                {
                    sql = "SELECT ID_RESULTADO, SPPRO$ID_PROGRAMA_PRESUPUESTO, SPPRO$ID_PRODUCTO, PRODUCTO, SPPRO$ID_MEDIDA, MEDIDA_PRODUCTO,  NULL IDFP2020, NULL MF2020, NULL MFIN2020, NULL TP2020,  NULL IDFP2021, NULL MF2021, NULL MFIN2021, NULL TP2021";
                    for (int i = 0; i < temp.Rows.Count; i++)
                    {
                        sql = sql + ",IDFP" + temp.Rows[i]["SPOA$ANIO"] + ", MF" + temp.Rows[i]["SPOA$ANIO"] + ", MFIN" + temp.Rows[i]["SPOA$ANIO"] + ", TP" + temp.Rows[i]["SPOA$ANIO"];
                    }
                    sql = sql + ",NULL IDFP2027, NULL MF2027, NULL MFIN2027, NULL TP2027,  NULL IDFP2028, NULL MF2028, NULL MFIN2028, NULL TP2028, SPRES$POM FROM (";
                    sql = sql + "SELECT VES.ID_RESULTADO, VES.SPPRO$ID_PROGRAMA_PRESUPUESTO, VES.SPPRO$ID_PRODUCTO, VES.PRODUCTO, VES.SPPRO$ID_MEDIDA, VES.MEDIDA_PRODUCTO";
                    for (int i = 0; i < temp.Rows.Count; i++)
                    {
                        sql = sql + ",SCHE$SIPLAN20.FNC$OBTIENEIDMETAFISICAPRO(" + temp.Rows[i]["SPOA$ID_POA"] + ", " + temp.Rows[i]["SPOA$ANIO"] + ", VES.SPPRO$ID_PRODUCTO) IDFP" + temp.Rows[i]["SPOA$ANIO"] + ",SCHE$SIPLAN20.FNC$OBTIENEMETAFISICAPRO(" + temp.Rows[i]["SPOA$ID_POA"] + ", " + temp.Rows[i]["SPOA$ANIO"] + ", VES.SPPRO$ID_PRODUCTO) MF" + temp.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$OBTIENEMETAFINACIERAPROD(" + temp.Rows[i]["SPOA$ID_POA"] + ", " + temp.Rows[i]["SPOA$ANIO"] + ", VES.SPPRO$ID_PRODUCTO,0) MFIN" + temp.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$OBTIENETIPOPROGRA(" + temp.Rows[i]["SPOA$ID_POA"] + ", " + temp.Rows[i]["SPOA$ANIO"] + ", VES.SPPRO$ID_PRODUCTO) TP" + temp.Rows[i]["SPOA$ANIO"];
                    }


                    sql = sql + " ,VES.SPRES$POM ";
                    sql = sql + " FROM SCHE$SIPLAN20.SPPVST$SUBESTRATEGICOV2 VES WHERE VES.ID_RESULTADO =" + resultado + "  AND VES.SPPRO$ID_PROGRAMA_PRESUPUESTO = " + programa + " AND  VES.SPRES$POM = " + pom + " AND  VES.SPRES$INSTITUCION =  " + insto + " AND VES.SPPRO$RESULTADO2 = " + resultado2 + ") GROUP BY  ID_RESULTADO, SPPRO$ID_PROGRAMA_PRESUPUESTO, SPPRO$ID_PRODUCTO, PRODUCTO, SPPRO$ID_MEDIDA, MEDIDA_PRODUCTO ";
                    for (int i = 0; i < temp.Rows.Count; i++)
                    {
                        sql = sql + ",IDFP" + temp.Rows[i]["SPOA$ANIO"] + ", MF" + temp.Rows[i]["SPOA$ANIO"] + ", MFIN" + temp.Rows[i]["SPOA$ANIO"] + ", TP" + temp.Rows[i]["SPOA$ANIO"];
                    }
                    sql = sql + " ,SPRES$POM ORDER BY SPPRO$ID_PROGRAMA_PRESUPUESTO, SPPRO$ID_PRODUCTO ASC";
                }
                else if (tipo == 1)
                {
                    sql = " SELECT ID_RESULTADO, SPPRO$ID_PROGRAMA_PRESUPUESTO, SPPRO$ID_PRODUCTO, PRODUCTO, SPPRO$ID_MEDIDA, MEDIDA_PRODUCTO, NULL IDFP2020, NULL MF2020, NULL MFIN2020, NULL TP2020 , NULL IDFP2021, NULL MF2021, NULL MFIN2021, NULL TP2021";
                    for (int i = 0; i < temp.Rows.Count; i++)
                    {
                        sql = sql + ",IDFP" + temp.Rows[i]["SPOA$ANIO"] + ", MF" + temp.Rows[i]["SPOA$ANIO"] + ", MFIN" + temp.Rows[i]["SPOA$ANIO"] + ", TP" + temp.Rows[i]["SPOA$ANIO"];
                    }
                    sql = sql + " ,NULL IDFP2027, NULL MF2027, NULL MFIN2027, NULL TP2027,  NULL IDFP2028, NULL MF2028, NULL MFIN2028, NULL TP2028, SPRES$POM FROM (";
                    sql = sql + "SELECT VES.ID_RESULTADO, VES.SPPRO$ID_PROGRAMA_PRESUPUESTO, VES.SPPRO$ID_PRODUCTO, VES.PRODUCTO, VES.SPPRO$ID_MEDIDA, VES.MEDIDA_PRODUCTO";
                    for (int i = 0; i < temp.Rows.Count; i++)
                    {
                        sql = sql + ",SCHE$SIPLAN20.FNC$OBTIENEIDMETAFISICAPRO(" + temp.Rows[i]["SPOA$ID_POA"] + ", " + temp.Rows[i]["SPOA$ANIO"] + ", VES.SPPRO$ID_PRODUCTO) IDFP" + temp.Rows[i]["SPOA$ANIO"] + ",SCHE$SIPLAN20.FNC$OBTIENEMETAFISICAPRO(" + temp.Rows[i]["SPOA$ID_POA"] + ", " + temp.Rows[i]["SPOA$ANIO"] + ", VES.SPPRO$ID_PRODUCTO) MF" + temp.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$OBTIENEMETAFINACIERAPROD(" + temp.Rows[i]["SPOA$ID_POA"] + ", " + temp.Rows[i]["SPOA$ANIO"] + ", VES.SPPRO$ID_PRODUCTO,0) MFIN" + temp.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$OBTIENETIPOPROGRA(" + temp.Rows[i]["SPOA$ID_POA"] + ", " + temp.Rows[i]["SPOA$ANIO"] + ", VES.SPPRO$ID_PRODUCTO) TP" + temp.Rows[i]["SPOA$ANIO"];
                    }


                    sql = sql + " ,VES.SPRES$POM ";
                    sql = sql + " FROM SCHE$SIPLAN20.SPPSVT$RED_INSTITUCIONAL VES WHERE VES.ID_RESULTADO =" + resultado + "  AND VES.SPPRO$ID_PROGRAMA_PRESUPUESTO = " + programa + " AND  VES.SPRES$POM = " + pom + " AND  VES.SPRES$INSTITUCION =  " + insto + " AND VES.SPPRO$RESULTADO2 = " + resultado2 + ") GROUP BY ID_RESULTADO, SPPRO$ID_PROGRAMA_PRESUPUESTO, SPPRO$ID_PRODUCTO, PRODUCTO, SPPRO$ID_MEDIDA, MEDIDA_PRODUCTO ";
                    for (int i = 0; i < temp.Rows.Count; i++)
                    {
                        sql = sql + ",IDFP" + temp.Rows[i]["SPOA$ANIO"] + ", MF" + temp.Rows[i]["SPOA$ANIO"] + ", MFIN" + temp.Rows[i]["SPOA$ANIO"] + ", TP" + temp.Rows[i]["SPOA$ANIO"];
                    }
                    sql = sql + " ,SPRES$POM ORDER BY SPPRO$ID_PROGRAMA_PRESUPUESTO, SPPRO$ID_PRODUCTO ASC";
                }
            }



            else if (Convert.ToInt32(Session["periodo"]) == 22)
            {
                if (tipo == 0)
                {
                    sql = "SELECT ID_RESULTADO, SPPRO$ID_PROGRAMA_PRESUPUESTO, SPPRO$ID_PRODUCTO, PRODUCTO, SPPRO$ID_MEDIDA, MEDIDA_PRODUCTO,  NULL IDFP2020, NULL MF2020, NULL MFIN2020, NULL TP2020,  NULL IDFP2021, NULL MF2021, NULL MFIN2021, NULL TP2021,    NULL IDFP2022, NULL MF2022, NULL MFIN2022, NULL TP2022";
                    for (int i = 0; i < temp.Rows.Count; i++)
                    {
                        sql = sql + ",IDFP" + temp.Rows[i]["SPOA$ANIO"] + ", MF" + temp.Rows[i]["SPOA$ANIO"] + ", MFIN" + temp.Rows[i]["SPOA$ANIO"] + ", TP" + temp.Rows[i]["SPOA$ANIO"];
                    }
                    sql = sql + " ,NULL IDFP2028, NULL MF2028, NULL MFIN2028, NULL TP2028,SPRES$POM FROM (";
                    sql = sql + "SELECT VES.ID_RESULTADO, VES.SPPRO$ID_PROGRAMA_PRESUPUESTO, VES.SPPRO$ID_PRODUCTO, VES.PRODUCTO, VES.SPPRO$ID_MEDIDA, VES.MEDIDA_PRODUCTO";
                    for (int i = 0; i < temp.Rows.Count; i++)
                    {
                        sql = sql + ",SCHE$SIPLAN20.FNC$OBTIENEIDMETAFISICAPRO(" + temp.Rows[i]["SPOA$ID_POA"] + ", " + temp.Rows[i]["SPOA$ANIO"] + ", VES.SPPRO$ID_PRODUCTO) IDFP" + temp.Rows[i]["SPOA$ANIO"] + ",SCHE$SIPLAN20.FNC$OBTIENEMETAFISICAPRO(" + temp.Rows[i]["SPOA$ID_POA"] + ", " + temp.Rows[i]["SPOA$ANIO"] + ", VES.SPPRO$ID_PRODUCTO) MF" + temp.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$OBTIENEMETAFINACIERAPROD(" + temp.Rows[i]["SPOA$ID_POA"] + ", " + temp.Rows[i]["SPOA$ANIO"] + ", VES.SPPRO$ID_PRODUCTO,0) MFIN" + temp.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$OBTIENETIPOPROGRA(" + temp.Rows[i]["SPOA$ID_POA"] + ", " + temp.Rows[i]["SPOA$ANIO"] + ", VES.SPPRO$ID_PRODUCTO) TP" + temp.Rows[i]["SPOA$ANIO"];
                    }


                    sql = sql + " ,VES.SPRES$POM ";
                    sql = sql + " FROM SCHE$SIPLAN20.SPPVST$SUBESTRATEGICOV2 VES WHERE VES.ID_RESULTADO =" + resultado + "  AND VES.SPPRO$ID_PROGRAMA_PRESUPUESTO = " + programa + " AND  VES.SPRES$POM = " + pom + " AND  VES.SPRES$INSTITUCION =  " + insto + " AND VES.SPPRO$RESULTADO2 = " + resultado2 + ") GROUP BY  ID_RESULTADO, SPPRO$ID_PROGRAMA_PRESUPUESTO, SPPRO$ID_PRODUCTO, PRODUCTO, SPPRO$ID_MEDIDA, MEDIDA_PRODUCTO ";
                    for (int i = 0; i < temp.Rows.Count; i++)
                    {
                        sql = sql + ",IDFP" + temp.Rows[i]["SPOA$ANIO"] + ", MF" + temp.Rows[i]["SPOA$ANIO"] + ", MFIN" + temp.Rows[i]["SPOA$ANIO"] + ", TP" + temp.Rows[i]["SPOA$ANIO"];
                    }
                    sql = sql + " ,SPRES$POM ORDER BY SPPRO$ID_PROGRAMA_PRESUPUESTO, SPPRO$ID_PRODUCTO ASC";
                }
                else if (tipo == 1)
                {
                    sql = " SELECT ID_RESULTADO, SPPRO$ID_PROGRAMA_PRESUPUESTO, SPPRO$ID_PRODUCTO, PRODUCTO, SPPRO$ID_MEDIDA, MEDIDA_PRODUCTO, NULL IDFP2020, NULL MF2020, NULL MFIN2020, NULL TP2020 ,NULL IDFP2021, NULL MF2021, NULL MFIN2021, NULL TP2021, NULL IDFP2022, NULL MF2022, NULL MFIN2022, NULL TP2022";
                    for (int i = 0; i < temp.Rows.Count; i++)
                    {
                        sql = sql + ",IDFP" + temp.Rows[i]["SPOA$ANIO"] + ", MF" + temp.Rows[i]["SPOA$ANIO"] + ", MFIN" + temp.Rows[i]["SPOA$ANIO"] + ", TP" + temp.Rows[i]["SPOA$ANIO"];
                    }
                    sql = sql + " ,NULL IDFP2028, NULL MF2028, NULL MFIN2028, NULL TP2028, SPRES$POM FROM (";
                    sql = sql + "SELECT VES.ID_RESULTADO, VES.SPPRO$ID_PROGRAMA_PRESUPUESTO, VES.SPPRO$ID_PRODUCTO, VES.PRODUCTO, VES.SPPRO$ID_MEDIDA, VES.MEDIDA_PRODUCTO";
                    for (int i = 0; i < temp.Rows.Count; i++)
                    {
                        sql = sql + ",SCHE$SIPLAN20.FNC$OBTIENEIDMETAFISICAPRO(" + temp.Rows[i]["SPOA$ID_POA"] + ", " + temp.Rows[i]["SPOA$ANIO"] + ", VES.SPPRO$ID_PRODUCTO) IDFP" + temp.Rows[i]["SPOA$ANIO"] + ",SCHE$SIPLAN20.FNC$OBTIENEMETAFISICAPRO(" + temp.Rows[i]["SPOA$ID_POA"] + ", " + temp.Rows[i]["SPOA$ANIO"] + ", VES.SPPRO$ID_PRODUCTO) MF" + temp.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$OBTIENEMETAFINACIERAPROD(" + temp.Rows[i]["SPOA$ID_POA"] + ", " + temp.Rows[i]["SPOA$ANIO"] + ", VES.SPPRO$ID_PRODUCTO,0) MFIN" + temp.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$OBTIENETIPOPROGRA(" + temp.Rows[i]["SPOA$ID_POA"] + ", " + temp.Rows[i]["SPOA$ANIO"] + ", VES.SPPRO$ID_PRODUCTO) TP" + temp.Rows[i]["SPOA$ANIO"];
                    }


                    sql = sql + " ,VES.SPRES$POM ";
                    sql = sql + " FROM SCHE$SIPLAN20.SPPSVT$RED_INSTITUCIONAL VES WHERE VES.ID_RESULTADO =" + resultado + "  AND VES.SPPRO$ID_PROGRAMA_PRESUPUESTO = " + programa + " AND  VES.SPRES$POM = " + pom + " AND  VES.SPRES$INSTITUCION =  " + insto + " AND VES.SPPRO$RESULTADO2 = " + resultado2 + ") GROUP BY ID_RESULTADO, SPPRO$ID_PROGRAMA_PRESUPUESTO, SPPRO$ID_PRODUCTO, PRODUCTO, SPPRO$ID_MEDIDA, MEDIDA_PRODUCTO ";
                    for (int i = 0; i < temp.Rows.Count; i++)
                    {
                        sql = sql + ",IDFP" + temp.Rows[i]["SPOA$ANIO"] + ", MF" + temp.Rows[i]["SPOA$ANIO"] + ", MFIN" + temp.Rows[i]["SPOA$ANIO"] + ", TP" + temp.Rows[i]["SPOA$ANIO"];
                    }
                    sql = sql + " ,SPRES$POM ORDER BY SPPRO$ID_PROGRAMA_PRESUPUESTO, SPPRO$ID_PRODUCTO ASC";
                }
            }



            else if (Convert.ToInt32(Session["periodo"]) == 23)
            {
                if (tipo == 0)
                {
                    sql = "SELECT ID_RESULTADO, SPPRO$ID_PROGRAMA_PRESUPUESTO, SPPRO$ID_PRODUCTO, PRODUCTO, SPPRO$ID_MEDIDA, MEDIDA_PRODUCTO,  NULL IDFP2020, NULL MF2020, NULL MFIN2020, NULL TP2020,  NULL IDFP2021, NULL MF2021, NULL MFIN2021, NULL TP2021,    NULL IDFP2022, NULL MF2022, NULL MFIN2022, NULL TP2022,   NULL IDFP2023, NULL MF2023, NULL MFIN2023, NULL TP2023";
                    for (int i = 0; i < temp.Rows.Count; i++)
                    {
                        sql = sql + ",IDFP" + temp.Rows[i]["SPOA$ANIO"] + ", MF" + temp.Rows[i]["SPOA$ANIO"] + ", MFIN" + temp.Rows[i]["SPOA$ANIO"] + ", TP" + temp.Rows[i]["SPOA$ANIO"];
                    }
                    sql = sql + ",SPRES$POM FROM (";
                    sql = sql + "SELECT VES.ID_RESULTADO, VES.SPPRO$ID_PROGRAMA_PRESUPUESTO, VES.SPPRO$ID_PRODUCTO, VES.PRODUCTO, VES.SPPRO$ID_MEDIDA, VES.MEDIDA_PRODUCTO";
                    for (int i = 0; i < temp.Rows.Count; i++)
                    {
                        sql = sql + ",SCHE$SIPLAN20.FNC$OBTIENEIDMETAFISICAPRO(" + temp.Rows[i]["SPOA$ID_POA"] + ", " + temp.Rows[i]["SPOA$ANIO"] + ", VES.SPPRO$ID_PRODUCTO) IDFP" + temp.Rows[i]["SPOA$ANIO"] + ",SCHE$SIPLAN20.FNC$OBTIENEMETAFISICAPRO(" + temp.Rows[i]["SPOA$ID_POA"] + ", " + temp.Rows[i]["SPOA$ANIO"] + ", VES.SPPRO$ID_PRODUCTO) MF" + temp.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$OBTIENEMETAFINACIERAPROD(" + temp.Rows[i]["SPOA$ID_POA"] + ", " + temp.Rows[i]["SPOA$ANIO"] + ", VES.SPPRO$ID_PRODUCTO,0) MFIN" + temp.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$OBTIENETIPOPROGRA(" + temp.Rows[i]["SPOA$ID_POA"] + ", " + temp.Rows[i]["SPOA$ANIO"] + ", VES.SPPRO$ID_PRODUCTO) TP" + temp.Rows[i]["SPOA$ANIO"];
                    }


                    sql = sql + " ,VES.SPRES$POM ";
                    sql = sql + " FROM SCHE$SIPLAN20.SPPVST$SUBESTRATEGICOV2 VES WHERE VES.ID_RESULTADO =" + resultado + "  AND VES.SPPRO$ID_PROGRAMA_PRESUPUESTO = " + programa + " AND  VES.SPRES$POM = " + pom + " AND  VES.SPRES$INSTITUCION =  " + insto + " AND VES.SPPRO$RESULTADO2 = " + resultado2 + ") GROUP BY  ID_RESULTADO, SPPRO$ID_PROGRAMA_PRESUPUESTO, SPPRO$ID_PRODUCTO, PRODUCTO, SPPRO$ID_MEDIDA, MEDIDA_PRODUCTO ";
                    for (int i = 0; i < temp.Rows.Count; i++)
                    {
                        sql = sql + ",IDFP" + temp.Rows[i]["SPOA$ANIO"] + ", MF" + temp.Rows[i]["SPOA$ANIO"] + ", MFIN" + temp.Rows[i]["SPOA$ANIO"] + ", TP" + temp.Rows[i]["SPOA$ANIO"];
                    }
                    sql = sql + " ,SPRES$POM ORDER BY SPPRO$ID_PROGRAMA_PRESUPUESTO, SPPRO$ID_PRODUCTO ASC";
                }
                else if (tipo == 1)
                {
                    sql = " SELECT ID_RESULTADO, SPPRO$ID_PROGRAMA_PRESUPUESTO, SPPRO$ID_PRODUCTO, PRODUCTO, SPPRO$ID_MEDIDA, MEDIDA_PRODUCTO, NULL IDFP2020, NULL MF2020, NULL MFIN2020, NULL TP2020 ,NULL IDFP2021, NULL MF2021, NULL MFIN2021, NULL TP2021, NULL IDFP2022, NULL MF2022, NULL MFIN2022, NULL TP2022,    NULL IDFP2023, NULL MF2023, NULL MFIN2023, NULL TP2023";
                    for (int i = 0; i < temp.Rows.Count; i++)
                    {
                        sql = sql + ",IDFP" + temp.Rows[i]["SPOA$ANIO"] + ", MF" + temp.Rows[i]["SPOA$ANIO"] + ", MFIN" + temp.Rows[i]["SPOA$ANIO"] + ", TP" + temp.Rows[i]["SPOA$ANIO"];
                    }
                    sql = sql + " ,SPRES$POM FROM (";
                    sql = sql + "SELECT VES.ID_RESULTADO, VES.SPPRO$ID_PROGRAMA_PRESUPUESTO, VES.SPPRO$ID_PRODUCTO, VES.PRODUCTO, VES.SPPRO$ID_MEDIDA, VES.MEDIDA_PRODUCTO";
                    for (int i = 0; i < temp.Rows.Count; i++)
                    {
                        sql = sql + ",SCHE$SIPLAN20.FNC$OBTIENEIDMETAFISICAPRO(" + temp.Rows[i]["SPOA$ID_POA"] + ", " + temp.Rows[i]["SPOA$ANIO"] + ", VES.SPPRO$ID_PRODUCTO) IDFP" + temp.Rows[i]["SPOA$ANIO"] + ",SCHE$SIPLAN20.FNC$OBTIENEMETAFISICAPRO(" + temp.Rows[i]["SPOA$ID_POA"] + ", " + temp.Rows[i]["SPOA$ANIO"] + ", VES.SPPRO$ID_PRODUCTO) MF" + temp.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$OBTIENEMETAFINACIERAPROD(" + temp.Rows[i]["SPOA$ID_POA"] + ", " + temp.Rows[i]["SPOA$ANIO"] + ", VES.SPPRO$ID_PRODUCTO,0) MFIN" + temp.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$OBTIENETIPOPROGRA(" + temp.Rows[i]["SPOA$ID_POA"] + ", " + temp.Rows[i]["SPOA$ANIO"] + ", VES.SPPRO$ID_PRODUCTO) TP" + temp.Rows[i]["SPOA$ANIO"];
                    }


                    sql = sql + " ,VES.SPRES$POM ";
                    sql = sql + " FROM SCHE$SIPLAN20.SPPSVT$RED_INSTITUCIONAL VES WHERE VES.ID_RESULTADO =" + resultado + "  AND VES.SPPRO$ID_PROGRAMA_PRESUPUESTO = " + programa + " AND  VES.SPRES$POM = " + pom + " AND  VES.SPRES$INSTITUCION =  " + insto + " AND VES.SPPRO$RESULTADO2 = " + resultado2 + ") GROUP BY ID_RESULTADO, SPPRO$ID_PROGRAMA_PRESUPUESTO, SPPRO$ID_PRODUCTO, PRODUCTO, SPPRO$ID_MEDIDA, MEDIDA_PRODUCTO ";
                    for (int i = 0; i < temp.Rows.Count; i++)
                    {
                        sql = sql + ",IDFP" + temp.Rows[i]["SPOA$ANIO"] + ", MF" + temp.Rows[i]["SPOA$ANIO"] + ", MFIN" + temp.Rows[i]["SPOA$ANIO"] + ", TP" + temp.Rows[i]["SPOA$ANIO"];
                    }
                    sql = sql + " ,SPRES$POM ORDER BY SPPRO$ID_PROGRAMA_PRESUPUESTO, SPPRO$ID_PRODUCTO ASC";
                }
            }

            //NUEVA PGG

            else if (Convert.ToInt32(Session["periodo"]) == 24)
            {
                if (tipo == 0)
                {
                    sql = "SELECT ID_RESULTADO, SPPRO$ID_PROGRAMA_PRESUPUESTO, SPPRO$ID_PRODUCTO, PRODUCTO, SPPRO$ID_MEDIDA, MEDIDA_PRODUCTO,  NULL IDFP2020, NULL MF2020, NULL MFIN2020, NULL TP2020,  NULL IDFP2021, NULL MF2021, NULL MFIN2021, NULL TP2021,    NULL IDFP2022, NULL MF2022, NULL MFIN2022, NULL TP2022, NULL IDFP2023, NULL MF2023, NULL MFIN2023, NULL TP2023, NULL IDFP2024, NULL MF2024, NULL MFIN2024, NULL TP2024";
                    for (int i = 0; i < temp.Rows.Count; i++)
                    {
                        sql = sql + ",IDFP" + temp.Rows[i]["SPOA$ANIO"] + ", MF" + temp.Rows[i]["SPOA$ANIO"] + ", MFIN" + temp.Rows[i]["SPOA$ANIO"] + ", TP" + temp.Rows[i]["SPOA$ANIO"];
                    }
                    sql = sql + ",SPRES$POM FROM (";
                    sql = sql + "SELECT VES.ID_RESULTADO, VES.SPPRO$ID_PROGRAMA_PRESUPUESTO, VES.SPPRO$ID_PRODUCTO, VES.PRODUCTO, VES.SPPRO$ID_MEDIDA, VES.MEDIDA_PRODUCTO";
                    for (int i = 0; i < temp.Rows.Count; i++)
                    {
                        sql = sql + ",SCHE$SIPLAN20.FNC$OBTIENEIDMETAFISICAPRO(" + temp.Rows[i]["SPOA$ID_POA"] + ", " + temp.Rows[i]["SPOA$ANIO"] + ", VES.SPPRO$ID_PRODUCTO) IDFP" + temp.Rows[i]["SPOA$ANIO"] + ",SCHE$SIPLAN20.FNC$OBTIENEMETAFISICAPRO(" + temp.Rows[i]["SPOA$ID_POA"] + ", " + temp.Rows[i]["SPOA$ANIO"] + ", VES.SPPRO$ID_PRODUCTO) MF" + temp.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$OBTIENEMETAFINACIERAPROD(" + temp.Rows[i]["SPOA$ID_POA"] + ", " + temp.Rows[i]["SPOA$ANIO"] + ", VES.SPPRO$ID_PRODUCTO,0) MFIN" + temp.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$OBTIENETIPOPROGRA(" + temp.Rows[i]["SPOA$ID_POA"] + ", " + temp.Rows[i]["SPOA$ANIO"] + ", VES.SPPRO$ID_PRODUCTO) TP" + temp.Rows[i]["SPOA$ANIO"];
                    }


                    sql = sql + " ,VES.SPRES$POM ";
                    sql = sql + " FROM SCHE$SIPLAN20.SPPVST$SUBESTRATEGICOV2 VES WHERE VES.ID_RESULTADO =" + resultado + "  AND VES.SPPRO$ID_PROGRAMA_PRESUPUESTO = " + programa + " AND  VES.SPRES$POM = " + pom + " AND  VES.SPRES$INSTITUCION =  " + insto + " AND VES.SPPRO$RESULTADO2 = " + resultado2 + ") GROUP BY  ID_RESULTADO, SPPRO$ID_PROGRAMA_PRESUPUESTO, SPPRO$ID_PRODUCTO, PRODUCTO, SPPRO$ID_MEDIDA, MEDIDA_PRODUCTO ";
                    for (int i = 0; i < temp.Rows.Count; i++)
                    {
                        sql = sql + ",IDFP" + temp.Rows[i]["SPOA$ANIO"] + ", MF" + temp.Rows[i]["SPOA$ANIO"] + ", MFIN" + temp.Rows[i]["SPOA$ANIO"] + ", TP" + temp.Rows[i]["SPOA$ANIO"];
                    }
                    sql = sql + " ,SPRES$POM ORDER BY SPPRO$ID_PROGRAMA_PRESUPUESTO, SPPRO$ID_PRODUCTO ASC";
                }
                else if (tipo == 1)
                {
                    sql = " SELECT ID_RESULTADO, SPPRO$ID_PROGRAMA_PRESUPUESTO, SPPRO$ID_PRODUCTO, PRODUCTO, SPPRO$ID_MEDIDA, MEDIDA_PRODUCTO, NULL IDFP2020, NULL MF2020, NULL MFIN2020, NULL TP2020 ,NULL IDFP2021, NULL MF2021, NULL MFIN2021, NULL TP2021, NULL IDFP2022, NULL MF2022, NULL MFIN2022, NULL TP2022, NULL IDFP2023, NULL MF2023, NULL MFIN2023, NULL TP2023,  NULL IDFP2024, NULL MF2024, NULL MFIN2024, NULL TP2024";
                    for (int i = 0; i < temp.Rows.Count; i++)
                    {
                        sql = sql + ",IDFP" + temp.Rows[i]["SPOA$ANIO"] + ", MF" + temp.Rows[i]["SPOA$ANIO"] + ", MFIN" + temp.Rows[i]["SPOA$ANIO"] + ", TP" + temp.Rows[i]["SPOA$ANIO"];
                    }
                    sql = sql + " ,SPRES$POM FROM (";
                    sql = sql + "SELECT VES.ID_RESULTADO, VES.SPPRO$ID_PROGRAMA_PRESUPUESTO, VES.SPPRO$ID_PRODUCTO, VES.PRODUCTO, VES.SPPRO$ID_MEDIDA, VES.MEDIDA_PRODUCTO";
                    for (int i = 0; i < temp.Rows.Count; i++)
                    {
                        sql = sql + ",SCHE$SIPLAN20.FNC$OBTIENEIDMETAFISICAPRO(" + temp.Rows[i]["SPOA$ID_POA"] + ", " + temp.Rows[i]["SPOA$ANIO"] + ", VES.SPPRO$ID_PRODUCTO) IDFP" + temp.Rows[i]["SPOA$ANIO"] + ",SCHE$SIPLAN20.FNC$OBTIENEMETAFISICAPRO(" + temp.Rows[i]["SPOA$ID_POA"] + ", " + temp.Rows[i]["SPOA$ANIO"] + ", VES.SPPRO$ID_PRODUCTO) MF" + temp.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$OBTIENEMETAFINACIERAPROD(" + temp.Rows[i]["SPOA$ID_POA"] + ", " + temp.Rows[i]["SPOA$ANIO"] + ", VES.SPPRO$ID_PRODUCTO,0) MFIN" + temp.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$OBTIENETIPOPROGRA(" + temp.Rows[i]["SPOA$ID_POA"] + ", " + temp.Rows[i]["SPOA$ANIO"] + ", VES.SPPRO$ID_PRODUCTO) TP" + temp.Rows[i]["SPOA$ANIO"];
                    }


                    sql = sql + " ,VES.SPRES$POM ";
                    sql = sql + " FROM SCHE$SIPLAN20.SPPSVT$RED_INSTITUCIONAL VES WHERE VES.ID_RESULTADO =" + resultado + "  AND VES.SPPRO$ID_PROGRAMA_PRESUPUESTO = " + programa + " AND  VES.SPRES$POM = " + pom + " AND  VES.SPRES$INSTITUCION =  " + insto + " AND VES.SPPRO$RESULTADO2 = " + resultado2 + ") GROUP BY ID_RESULTADO, SPPRO$ID_PROGRAMA_PRESUPUESTO, SPPRO$ID_PRODUCTO, PRODUCTO, SPPRO$ID_MEDIDA, MEDIDA_PRODUCTO ";
                    for (int i = 0; i < temp.Rows.Count; i++)
                    {
                        sql = sql + ",IDFP" + temp.Rows[i]["SPOA$ANIO"] + ", MF" + temp.Rows[i]["SPOA$ANIO"] + ", MFIN" + temp.Rows[i]["SPOA$ANIO"] + ", TP" + temp.Rows[i]["SPOA$ANIO"];
                    }
                    sql = sql + " ,SPRES$POM ORDER BY SPPRO$ID_PROGRAMA_PRESUPUESTO, SPPRO$ID_PRODUCTO ASC";
                }
            }
            //NUEVA PGG

            //INICIO 2026-2030
            else if (Convert.ToInt32(Session["periodo"]) == 25)
            {
                if (tipo == 0)
                {
                    sql = "SELECT ID_RESULTADO, SPPRO$ID_PROGRAMA_PRESUPUESTO, SPPRO$ID_PRODUCTO, PRODUCTO, SPPRO$ID_MEDIDA, MEDIDA_PRODUCTO,  NULL IDFP2020, NULL MF2020, NULL MFIN2020, NULL TP2020,  NULL IDFP2021, NULL MF2021, NULL MFIN2021, NULL TP2021,    NULL IDFP2022, NULL MF2022, NULL MFIN2022, NULL TP2022, NULL IDFP2023, NULL MF2023, NULL MFIN2023, NULL TP2023, NULL IDFP2024, NULL MF2024, NULL MFIN2024, NULL TP2024, NULL IDFP2025, NULL MF2025, NULL MFIN2025, NULL TP2025";
                    for (int i = 0; i < temp.Rows.Count; i++)
                    {
                        sql = sql + ",IDFP" + temp.Rows[i]["SPOA$ANIO"] + ", MF" + temp.Rows[i]["SPOA$ANIO"] + ", MFIN" + temp.Rows[i]["SPOA$ANIO"] + ", TP" + temp.Rows[i]["SPOA$ANIO"];
                    }
                    sql = sql + ",SPRES$POM FROM (";
                    sql = sql + "SELECT VES.ID_RESULTADO, VES.SPPRO$ID_PROGRAMA_PRESUPUESTO, VES.SPPRO$ID_PRODUCTO, VES.PRODUCTO, VES.SPPRO$ID_MEDIDA, VES.MEDIDA_PRODUCTO";
                    for (int i = 0; i < temp.Rows.Count; i++)
                    {
                        sql = sql + ",SCHE$SIPLAN20.FNC$OBTIENEIDMETAFISICAPRO(" + temp.Rows[i]["SPOA$ID_POA"] + ", " + temp.Rows[i]["SPOA$ANIO"] + ", VES.SPPRO$ID_PRODUCTO) IDFP" + temp.Rows[i]["SPOA$ANIO"] + ",SCHE$SIPLAN20.FNC$OBTIENEMETAFISICAPRO(" + temp.Rows[i]["SPOA$ID_POA"] + ", " + temp.Rows[i]["SPOA$ANIO"] + ", VES.SPPRO$ID_PRODUCTO) MF" + temp.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$OBTIENEMETAFINACIERAPROD(" + temp.Rows[i]["SPOA$ID_POA"] + ", " + temp.Rows[i]["SPOA$ANIO"] + ", VES.SPPRO$ID_PRODUCTO,0) MFIN" + temp.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$OBTIENETIPOPROGRA(" + temp.Rows[i]["SPOA$ID_POA"] + ", " + temp.Rows[i]["SPOA$ANIO"] + ", VES.SPPRO$ID_PRODUCTO) TP" + temp.Rows[i]["SPOA$ANIO"];
                    }


                    sql = sql + " ,VES.SPRES$POM ";
                    sql = sql + " FROM SCHE$SIPLAN20.SPPVST$SUBESTRATEGICOV2 VES WHERE VES.ID_RESULTADO =" + resultado + "  AND VES.SPPRO$ID_PROGRAMA_PRESUPUESTO = " + programa + " AND  VES.SPRES$POM = " + pom + " AND  VES.SPRES$INSTITUCION =  " + insto + " AND VES.SPPRO$RESULTADO2 = " + resultado2 + ") GROUP BY  ID_RESULTADO, SPPRO$ID_PROGRAMA_PRESUPUESTO, SPPRO$ID_PRODUCTO, PRODUCTO, SPPRO$ID_MEDIDA, MEDIDA_PRODUCTO ";
                    for (int i = 0; i < temp.Rows.Count; i++)
                    {
                        sql = sql + ",IDFP" + temp.Rows[i]["SPOA$ANIO"] + ", MF" + temp.Rows[i]["SPOA$ANIO"] + ", MFIN" + temp.Rows[i]["SPOA$ANIO"] + ", TP" + temp.Rows[i]["SPOA$ANIO"];
                    }
                    sql = sql + " ,SPRES$POM ORDER BY SPPRO$ID_PROGRAMA_PRESUPUESTO, SPPRO$ID_PRODUCTO ASC";
                }
                else if (tipo == 1)
                {
                    sql = " SELECT ID_RESULTADO, SPPRO$ID_PROGRAMA_PRESUPUESTO, SPPRO$ID_PRODUCTO, PRODUCTO, SPPRO$ID_MEDIDA, MEDIDA_PRODUCTO, NULL IDFP2020, NULL MF2020, NULL MFIN2020, NULL TP2020 ,NULL IDFP2021, NULL MF2021, NULL MFIN2021, NULL TP2021, NULL IDFP2022, NULL MF2022, NULL MFIN2022, NULL TP2022, NULL IDFP2023, NULL MF2023, NULL MFIN2023, NULL TP2023,  NULL IDFP2024, NULL MF2024, NULL MFIN2024, NULL TP2024, NULL IDFP2025, NULL MF2025, NULL MFIN2025, NULL TP2025";
                    for (int i = 0; i < temp.Rows.Count; i++)
                    {
                        sql = sql + ",IDFP" + temp.Rows[i]["SPOA$ANIO"] + ", MF" + temp.Rows[i]["SPOA$ANIO"] + ", MFIN" + temp.Rows[i]["SPOA$ANIO"] + ", TP" + temp.Rows[i]["SPOA$ANIO"];
                    }
                    sql = sql + " ,SPRES$POM FROM (";
                    sql = sql + "SELECT VES.ID_RESULTADO, VES.SPPRO$ID_PROGRAMA_PRESUPUESTO, VES.SPPRO$ID_PRODUCTO, VES.PRODUCTO, VES.SPPRO$ID_MEDIDA, VES.MEDIDA_PRODUCTO";
                    for (int i = 0; i < temp.Rows.Count; i++)
                    {
                        sql = sql + ",SCHE$SIPLAN20.FNC$OBTIENEIDMETAFISICAPRO(" + temp.Rows[i]["SPOA$ID_POA"] + ", " + temp.Rows[i]["SPOA$ANIO"] + ", VES.SPPRO$ID_PRODUCTO) IDFP" + temp.Rows[i]["SPOA$ANIO"] + ",SCHE$SIPLAN20.FNC$OBTIENEMETAFISICAPRO(" + temp.Rows[i]["SPOA$ID_POA"] + ", " + temp.Rows[i]["SPOA$ANIO"] + ", VES.SPPRO$ID_PRODUCTO) MF" + temp.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$OBTIENEMETAFINACIERAPROD(" + temp.Rows[i]["SPOA$ID_POA"] + ", " + temp.Rows[i]["SPOA$ANIO"] + ", VES.SPPRO$ID_PRODUCTO,0) MFIN" + temp.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$OBTIENETIPOPROGRA(" + temp.Rows[i]["SPOA$ID_POA"] + ", " + temp.Rows[i]["SPOA$ANIO"] + ", VES.SPPRO$ID_PRODUCTO) TP" + temp.Rows[i]["SPOA$ANIO"];
                    }


                    sql = sql + " ,VES.SPRES$POM ";
                    sql = sql + " FROM SCHE$SIPLAN20.SPPSVT$RED_INSTITUCIONAL VES WHERE VES.ID_RESULTADO =" + resultado + "  AND VES.SPPRO$ID_PROGRAMA_PRESUPUESTO = " + programa + " AND  VES.SPRES$POM = " + pom + " AND  VES.SPRES$INSTITUCION =  " + insto + " AND VES.SPPRO$RESULTADO2 = " + resultado2 + ") GROUP BY ID_RESULTADO, SPPRO$ID_PROGRAMA_PRESUPUESTO, SPPRO$ID_PRODUCTO, PRODUCTO, SPPRO$ID_MEDIDA, MEDIDA_PRODUCTO ";
                    for (int i = 0; i < temp.Rows.Count; i++)
                    {
                        sql = sql + ",IDFP" + temp.Rows[i]["SPOA$ANIO"] + ", MF" + temp.Rows[i]["SPOA$ANIO"] + ", MFIN" + temp.Rows[i]["SPOA$ANIO"] + ", TP" + temp.Rows[i]["SPOA$ANIO"];
                    }
                    sql = sql + " ,SPRES$POM ORDER BY SPPRO$ID_PROGRAMA_PRESUPUESTO, SPPRO$ID_PRODUCTO ASC";
                }
            }
            //FIN 2026 - 2030

            else
            {
                if (tipo == 0)
                {
                    sql = "SELECT ID_RESULTADO, SPPRO$ID_PROGRAMA_PRESUPUESTO, SPPRO$ID_PRODUCTO, PRODUCTO, SPPRO$ID_MEDIDA, MEDIDA_PRODUCTO ";
                    for (int i = 0; i < temp.Rows.Count; i++)
                    {
                        sql = sql + ",IDFP" + temp.Rows[i]["SPOA$ANIO"] + ", MF" + temp.Rows[i]["SPOA$ANIO"] + ", MFIN" + temp.Rows[i]["SPOA$ANIO"] + ", TP" + temp.Rows[i]["SPOA$ANIO"];
                    }
                    sql = sql + " ,NULL IDFP2025, NULL MF2025, NULL MFIN2025, NULL TP2025 ,NULL IDFP2026, NULL MF2026, NULL MFIN2026, NULL TP2026,    NULL IDFP2027, NULL MF2027, NULL MFIN2027, NULL TP2027,    SPRES$POM FROM (";
                    sql = sql + "SELECT VES.ID_RESULTADO, VES.SPPRO$ID_PROGRAMA_PRESUPUESTO, VES.SPPRO$ID_PRODUCTO, VES.PRODUCTO, VES.SPPRO$ID_MEDIDA, VES.MEDIDA_PRODUCTO";
                    for (int i = 0; i < temp.Rows.Count; i++)
                    {
                        sql = sql + ",SCHE$SIPLAN20.FNC$OBTIENEIDMETAFISICAPRO(" + temp.Rows[i]["SPOA$ID_POA"] + ", " + temp.Rows[i]["SPOA$ANIO"] + ", VES.SPPRO$ID_PRODUCTO) IDFP" + temp.Rows[i]["SPOA$ANIO"] + ",SCHE$SIPLAN20.FNC$OBTIENEMETAFISICAPRO(" + temp.Rows[i]["SPOA$ID_POA"] + ", " + temp.Rows[i]["SPOA$ANIO"] + ", VES.SPPRO$ID_PRODUCTO) MF" + temp.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$OBTIENEMETAFINACIERAPROD(" + temp.Rows[i]["SPOA$ID_POA"] + ", " + temp.Rows[i]["SPOA$ANIO"] + ", VES.SPPRO$ID_PRODUCTO,0) MFIN" + temp.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$OBTIENETIPOPROGRA(" + temp.Rows[i]["SPOA$ID_POA"] + ", " + temp.Rows[i]["SPOA$ANIO"] + ", VES.SPPRO$ID_PRODUCTO) TP" + temp.Rows[i]["SPOA$ANIO"];
                    }


                    sql = sql + " ,VES.SPRES$POM ";
                    sql = sql + " FROM SCHE$SIPLAN20.SPPVST$SUBESTRATEGICOV2 VES WHERE VES.ID_RESULTADO =" + resultado + "  AND VES.SPPRO$ID_PROGRAMA_PRESUPUESTO = " + programa + " AND  VES.SPRES$POM = " + pom + " AND  VES.SPRES$INSTITUCION =  " + insto + ") GROUP BY  ID_RESULTADO, SPPRO$ID_PROGRAMA_PRESUPUESTO, SPPRO$ID_PRODUCTO, PRODUCTO, SPPRO$ID_MEDIDA, MEDIDA_PRODUCTO ";
                    for (int i = 0; i < temp.Rows.Count; i++)
                    {
                        sql = sql + ",IDFP" + temp.Rows[i]["SPOA$ANIO"] + ", MF" + temp.Rows[i]["SPOA$ANIO"] + ", MFIN" + temp.Rows[i]["SPOA$ANIO"] + ", TP" + temp.Rows[i]["SPOA$ANIO"];
                    }
                    sql = sql + " ,SPRES$POM ORDER BY SPPRO$ID_PROGRAMA_PRESUPUESTO, SPPRO$ID_PRODUCTO ASC";
                }
                else if (tipo == 1)
                {
                    sql = " SELECT ID_RESULTADO, SPPRO$ID_PROGRAMA_PRESUPUESTO, SPPRO$ID_PRODUCTO, PRODUCTO, SPPRO$ID_MEDIDA, MEDIDA_PRODUCTO ";
                    for (int i = 0; i < temp.Rows.Count; i++)
                    {
                        sql = sql + ",IDFP" + temp.Rows[i]["SPOA$ANIO"] + ", MF" + temp.Rows[i]["SPOA$ANIO"] + ", MFIN" + temp.Rows[i]["SPOA$ANIO"] + ", TP" + temp.Rows[i]["SPOA$ANIO"];
                    }
                    sql = sql + ", NULL IDFP2025, NULL MF2025, NULL MFIN2025, NULL TP2025,    NULL IDFP2026, NULL MF2026, NULL MFIN2026, NULL TP2026,  NULL IDFP2027, NULL MF2027, NULL MFIN2027, NULL TP2027,   SPRES$POM FROM (";
                    sql = sql + "SELECT VES.ID_RESULTADO, VES.SPPRO$ID_PROGRAMA_PRESUPUESTO, VES.SPPRO$ID_PRODUCTO, VES.PRODUCTO, VES.SPPRO$ID_MEDIDA, VES.MEDIDA_PRODUCTO";
                    for (int i = 0; i < temp.Rows.Count; i++)
                    {
                        sql = sql + ",SCHE$SIPLAN20.FNC$OBTIENEIDMETAFISICAPRO(" + temp.Rows[i]["SPOA$ID_POA"] + ", " + temp.Rows[i]["SPOA$ANIO"] + ", VES.SPPRO$ID_PRODUCTO) IDFP" + temp.Rows[i]["SPOA$ANIO"] + ",SCHE$SIPLAN20.FNC$OBTIENEMETAFISICAPRO(" + temp.Rows[i]["SPOA$ID_POA"] + ", " + temp.Rows[i]["SPOA$ANIO"] + ", VES.SPPRO$ID_PRODUCTO) MF" + temp.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$OBTIENEMETAFINACIERAPROD(" + temp.Rows[i]["SPOA$ID_POA"] + ", " + temp.Rows[i]["SPOA$ANIO"] + ", VES.SPPRO$ID_PRODUCTO,0) MFIN" + temp.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$OBTIENETIPOPROGRA(" + temp.Rows[i]["SPOA$ID_POA"] + ", " + temp.Rows[i]["SPOA$ANIO"] + ", VES.SPPRO$ID_PRODUCTO) TP" + temp.Rows[i]["SPOA$ANIO"];
                    }


                    sql = sql + ", VES.SPRES$POM ";
                    sql = sql + " FROM SCHE$SIPLAN20.SPPSVT$RED_INSTITUCIONAL VES WHERE VES.ID_RESULTADO =" + resultado + "  AND VES.SPPRO$ID_PROGRAMA_PRESUPUESTO = " + programa + " AND  VES.SPRES$POM = " + pom + " AND  VES.SPRES$INSTITUCION =  " + insto + ") GROUP BY ID_RESULTADO, SPPRO$ID_PROGRAMA_PRESUPUESTO, SPPRO$ID_PRODUCTO, PRODUCTO, SPPRO$ID_MEDIDA, MEDIDA_PRODUCTO ";
                    for (int i = 0; i < temp.Rows.Count; i++)
                    {
                        sql = sql + ",IDFP" + temp.Rows[i]["SPOA$ANIO"] + ", MF" + temp.Rows[i]["SPOA$ANIO"] + ", MFIN" + temp.Rows[i]["SPOA$ANIO"] + ", TP" + temp.Rows[i]["SPOA$ANIO"];
                    }
                    sql = sql + " ,SPRES$POM ORDER BY SPPRO$ID_PROGRAMA_PRESUPUESTO, SPPRO$ID_PRODUCTO ASC";
                }
            }



            estado = dao.consulta(sql);
            if (estado != 0)
                productos = dao.tabla;
            return productos;
        }

        protected void gvsubProductos_BeforePerformDataSelect(object sender, EventArgs e)
        {
            Session["carga"] = 1;
            ASPxGridView gvDetail = sender as ASPxGridView;
            DataTable productos = new DataTable();
            DataTable programas = new DataTable();
            int producto, resultado;
            Double programa;
            producto = Convert.ToInt32((sender as ASPxGridView).GetMasterRowFieldValues("SPPRO$ID_PRODUCTO"));
            programa = Convert.ToDouble((sender as ASPxGridView).GetMasterRowFieldValues("SPPRO$ID_PROGRAMA_PRESUPUESTO"));
            resultado = Convert.ToInt32((sender as ASPxGridView).GetMasterRowFieldValues("ID_RESULTADO"));




            if (Session["poa"] != null)
            {
                productos = cargaSubproductosdet((DataTable)Session["poa"], Convert.ToInt32(cbTipoProduccion.Value), resultado, programa, Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), producto);
                Session["subproductos"] = productos;
                programas = cargaProgramaEstrategico((DataTable)Session["poa"], programa, Convert.ToInt32(cbTipoProduccion.Value));
                Session["programapres"] = programas;


            }


            if (Convert.ToInt32(Session["periodo"]) == 0)
            {
                gvDetail.Columns["MFSUB2025"].Visible = false;
                gvDetail.Columns["MFINSUB2025"].Visible = false;
                GridViewBandColumn bandCol = gvDetail.Columns["2025"] as GridViewBandColumn;
                bandCol.Visible = false;

                gvDetail.Columns["MFSUB2026"].Visible = false;
                gvDetail.Columns["MFINSUB2026"].Visible = false;
                GridViewBandColumn bandCol1 = gvDetail.Columns["2026"] as GridViewBandColumn;
                bandCol1.Visible = false;


                gvDetail.Columns["MFSUB2027"].Visible = false;
                gvDetail.Columns["MFINSUB2027"].Visible = false;
                GridViewBandColumn bandCol2 = gvDetail.Columns["2027"] as GridViewBandColumn;
                bandCol2.Visible = false;

                gvDetail.Columns["MFSUB2028"].Visible = false;
                gvDetail.Columns["MFINSUB2028"].Visible = false;
                GridViewBandColumn bandCol3 = gvDetail.Columns["2028"] as GridViewBandColumn;
                bandCol3.Visible = false;


                gvDetail.Columns["MFSUB2029"].Visible = false;
                gvDetail.Columns["MFINSUB2029"].Visible = false;
                GridViewBandColumn bandCol4 = gvDetail.Columns["2029"] as GridViewBandColumn;
                bandCol4.Visible = false;

                gvDetail.Columns["MFSUB2030"].Visible = false;
                gvDetail.Columns["MFINSUB2030"].Visible = false;
                GridViewBandColumn bandCol5 = gvDetail.Columns["2030"] as GridViewBandColumn;
                bandCol5.Visible = false;

            }


            else if (Convert.ToInt32(Session["periodo"]) == 20)
            {
                gvDetail.Columns["MFSUB2020"].Visible = false;
                gvDetail.Columns["MFINSUB2020"].Visible = false;
                GridViewBandColumn bandCol = gvDetail.Columns["2020"] as GridViewBandColumn;
                bandCol.Visible = false;

                gvDetail.Columns["MFSUB2026"].Visible = false;
                gvDetail.Columns["MFINSUB2026"].Visible = false;
                GridViewBandColumn bandCol1 = gvDetail.Columns["2026"] as GridViewBandColumn;
                bandCol1.Visible = false;


                gvDetail.Columns["MFSUB2027"].Visible = false;
                gvDetail.Columns["MFINSUB2027"].Visible = false;
                GridViewBandColumn bandCol2 = gvDetail.Columns["2027"] as GridViewBandColumn;
                bandCol2.Visible = false;

                gvDetail.Columns["MFSUB2028"].Visible = false;
                gvDetail.Columns["MFINSUB2028"].Visible = false;
                GridViewBandColumn bandCol3 = gvDetail.Columns["2028"] as GridViewBandColumn;
                bandCol3.Visible = false;

                gvDetail.Columns["MFSUB2029"].Visible = false;
                gvDetail.Columns["MFINSUB2029"].Visible = false;
                GridViewBandColumn bandCol4 = gvDetail.Columns["2029"] as GridViewBandColumn;
                bandCol4.Visible = false;

                gvDetail.Columns["MFSUB2030"].Visible = false;
                gvDetail.Columns["MFINSUB2030"].Visible = false;
                GridViewBandColumn bandCol5 = gvDetail.Columns["2030"] as GridViewBandColumn;
                bandCol5.Visible = false;


            }


            else if (Convert.ToInt32(Session["periodo"]) == 21)
            {
                gvDetail.Columns["MFSUB2020"].Visible = false;
                gvDetail.Columns["MFINSUB2020"].Visible = false;
                GridViewBandColumn bandCol = gvDetail.Columns["2020"] as GridViewBandColumn;
                bandCol.Visible = false;

                gvDetail.Columns["MFSUB2021"].Visible = false;
                gvDetail.Columns["MFINSUB2021"].Visible = false;
                GridViewBandColumn bandCol1 = gvDetail.Columns["2021"] as GridViewBandColumn;
                bandCol1.Visible = false;


                gvDetail.Columns["MFSUB2027"].Visible = false;
                gvDetail.Columns["MFINSUB2027"].Visible = false;
                GridViewBandColumn bandCol2 = gvDetail.Columns["2027"] as GridViewBandColumn;
                bandCol2.Visible = false;


                gvDetail.Columns["MFSUB2028"].Visible = false;
                gvDetail.Columns["MFINSUB2028"].Visible = false;
                GridViewBandColumn bandCol3 = gvDetail.Columns["2028"] as GridViewBandColumn;
                bandCol3.Visible = false;


                gvDetail.Columns["MFSUB2029"].Visible = false;
                gvDetail.Columns["MFINSUB2029"].Visible = false;
                GridViewBandColumn bandCol4 = gvDetail.Columns["2029"] as GridViewBandColumn;
                bandCol4.Visible = false;

                gvDetail.Columns["MFSUB2030"].Visible = false;
                gvDetail.Columns["MFINSUB2030"].Visible = false;
                GridViewBandColumn bandCol5 = gvDetail.Columns["2030"] as GridViewBandColumn;
                bandCol5.Visible = false;

            }



            else if (Convert.ToInt32(Session["periodo"]) == 22)
            {
                gvDetail.Columns["MFSUB2020"].Visible = false;
                gvDetail.Columns["MFINSUB2020"].Visible = false;
                GridViewBandColumn bandCol = gvDetail.Columns["2020"] as GridViewBandColumn;
                bandCol.Visible = false;

                gvDetail.Columns["MFSUB2021"].Visible = false;
                gvDetail.Columns["MFINSUB2021"].Visible = false;
                GridViewBandColumn bandCol1 = gvDetail.Columns["2021"] as GridViewBandColumn;
                bandCol1.Visible = false;


                gvDetail.Columns["MFSUB2022"].Visible = false;
                gvDetail.Columns["MFINSUB2022"].Visible = false;
                GridViewBandColumn bandCol2 = gvDetail.Columns["2022"] as GridViewBandColumn;
                bandCol2.Visible = false;

                gvDetail.Columns["MFSUB2028"].Visible = false;
                gvDetail.Columns["MFINSUB2028"].Visible = false;
                GridViewBandColumn bandCol3 = gvDetail.Columns["2028"] as GridViewBandColumn;
                bandCol3.Visible = false;


                gvDetail.Columns["MFSUB2029"].Visible = false;
                gvDetail.Columns["MFINSUB2029"].Visible = false;
                GridViewBandColumn bandCol4 = gvDetail.Columns["2029"] as GridViewBandColumn;
                bandCol4.Visible = false;

                gvDetail.Columns["MFSUB2030"].Visible = false;
                gvDetail.Columns["MFINSUB2030"].Visible = false;
                GridViewBandColumn bandCol5 = gvDetail.Columns["2030"] as GridViewBandColumn;
                bandCol5.Visible = false;
            }


            else if (Convert.ToInt32(Session["periodo"]) == 23)
            {
                gvDetail.Columns["MFSUB2020"].Visible = false;
                gvDetail.Columns["MFINSUB2020"].Visible = false;
                GridViewBandColumn bandCol = gvDetail.Columns["2020"] as GridViewBandColumn;
                bandCol.Visible = false;

                gvDetail.Columns["MFSUB2021"].Visible = false;
                gvDetail.Columns["MFINSUB2021"].Visible = false;
                GridViewBandColumn bandCol1 = gvDetail.Columns["2021"] as GridViewBandColumn;
                bandCol1.Visible = false;


                gvDetail.Columns["MFSUB2022"].Visible = false;
                gvDetail.Columns["MFINSUB2022"].Visible = false;
                GridViewBandColumn bandCol2 = gvDetail.Columns["2022"] as GridViewBandColumn;
                bandCol2.Visible = false;

                gvDetail.Columns["MFSUB2023"].Visible = false;
                gvDetail.Columns["MFINSUB2023"].Visible = false;
                GridViewBandColumn bandCol3 = gvDetail.Columns["2023"] as GridViewBandColumn;
                bandCol3.Visible = false;


                gvDetail.Columns["MFSUB2029"].Visible = false;
                gvDetail.Columns["MFINSUB2029"].Visible = false;
                GridViewBandColumn bandCol4 = gvDetail.Columns["2029"] as GridViewBandColumn;
                bandCol4.Visible = false;

                gvDetail.Columns["MFSUB2030"].Visible = false;
                gvDetail.Columns["MFINSUB2030"].Visible = false;
                GridViewBandColumn bandCol5 = gvDetail.Columns["2030"] as GridViewBandColumn;
                bandCol5.Visible = false;
            }



            else if (Convert.ToInt32(Session["periodo"]) == 24)
            {
                gvDetail.Columns["MFSUB2020"].Visible = false;
                gvDetail.Columns["MFINSUB2020"].Visible = false;
                GridViewBandColumn bandCol = gvDetail.Columns["2020"] as GridViewBandColumn;
                bandCol.Visible = false;

                gvDetail.Columns["MFSUB2021"].Visible = false;
                gvDetail.Columns["MFINSUB2021"].Visible = false;
                GridViewBandColumn bandCol1 = gvDetail.Columns["2021"] as GridViewBandColumn;
                bandCol1.Visible = false;


                gvDetail.Columns["MFSUB2022"].Visible = false;
                gvDetail.Columns["MFINSUB2022"].Visible = false;
                GridViewBandColumn bandCol2 = gvDetail.Columns["2022"] as GridViewBandColumn;
                bandCol2.Visible = false;

                gvDetail.Columns["MFSUB2023"].Visible = false;
                gvDetail.Columns["MFINSUB2023"].Visible = false;
                GridViewBandColumn bandCol3 = gvDetail.Columns["2023"] as GridViewBandColumn;
                bandCol3.Visible = false;


                gvDetail.Columns["MFSUB2024"].Visible = false;
                gvDetail.Columns["MFINSUB2024"].Visible = false;
                GridViewBandColumn bandCol4 = gvDetail.Columns["2024"] as GridViewBandColumn;
                bandCol4.Visible = false;


                gvDetail.Columns["MFSUB2030"].Visible = false;
                gvDetail.Columns["MFINSUB2030"].Visible = false;
                GridViewBandColumn bandCol5 = gvDetail.Columns["2030"] as GridViewBandColumn;
                bandCol5.Visible = false;
            }


            else if (Convert.ToInt32(Session["periodo"]) == 25)
            {
                gvDetail.Columns["MFSUB2020"].Visible = false;
                gvDetail.Columns["MFINSUB2020"].Visible = false;
                GridViewBandColumn bandCol = gvDetail.Columns["2020"] as GridViewBandColumn;
                bandCol.Visible = false;

                gvDetail.Columns["MFSUB2021"].Visible = false;
                gvDetail.Columns["MFINSUB2021"].Visible = false;
                GridViewBandColumn bandCol1 = gvDetail.Columns["2021"] as GridViewBandColumn;
                bandCol1.Visible = false;


                gvDetail.Columns["MFSUB2022"].Visible = false;
                gvDetail.Columns["MFINSUB2022"].Visible = false;
                GridViewBandColumn bandCol2 = gvDetail.Columns["2022"] as GridViewBandColumn;
                bandCol2.Visible = false;

                gvDetail.Columns["MFSUB2023"].Visible = false;
                gvDetail.Columns["MFINSUB2023"].Visible = false;
                GridViewBandColumn bandCol3 = gvDetail.Columns["2023"] as GridViewBandColumn;
                bandCol3.Visible = false;


                gvDetail.Columns["MFSUB2024"].Visible = false;
                gvDetail.Columns["MFINSUB2024"].Visible = false;
                GridViewBandColumn bandCol4 = gvDetail.Columns["2024"] as GridViewBandColumn;
                bandCol4.Visible = false;


                gvDetail.Columns["MFSUB2025"].Visible = false;
                gvDetail.Columns["MFINSUB2025"].Visible = false;
                GridViewBandColumn bandCol5 = gvDetail.Columns["2025"] as GridViewBandColumn;
                bandCol5.Visible = false;
            }

            gvDetail.DataSource = productos;

            //gvProductos_FocusedRowChanged(sender, e);



        }

        protected DataTable cargaSubproductosdet(DataTable temp, int tipo, int resultado, Double programa, int pom, int insto, int producto)
        {
            DataTable subproductos = new DataTable();
            if (Convert.ToInt32(Session["periodo"]) == 20)
            {
                if (tipo == 0)
                {
                    sql = "SELECT SPPRO$ID_PRODUCTO, SPPSUB$ID_SUBPRODUCTO, SUBPRODUCTO, MEDIDA_SUBPRODUCTO, SPPSUB$SNIP, NULL IDFISUB2020, NULL IDFINSUB2020, NULL MFSUB2020, NULL MFINSUB2020, NULL TSF2020, NULL TSFIN2020 ";
                    for (int i = 0; i < temp.Rows.Count; i++)
                    {
                        sql = sql + ",IDFISUB" + temp.Rows[i]["SPOA$ANIO"] + ", IDFINSUB" + temp.Rows[i]["SPOA$ANIO"] + ", MFSUB" + temp.Rows[i]["SPOA$ANIO"] + ", MFINSUB" + temp.Rows[i]["SPOA$ANIO"] + ",TSF" + temp.Rows[i]["SPOA$ANIO"] + ",TSFIN" + temp.Rows[i]["SPOA$ANIO"];
                    }
                    sql = sql + " ,NULL IDFISUB2026, NULL IDFINSUB2026, NULL MFSUB2026, NULL MFINSUB2026, NULL TSF2026, NULL TSFIN2026  ,NULL IDFISUB2027, NULL IDFINSUB2027, NULL MFSUB2027, NULL MFINSUB2027, NULL TSF2027, NULL TSFIN2027  ,NULL IDFISUB2028, NULL IDFINSUB2028, NULL MFSUB2028, NULL MFINSUB2028, NULL TSF2028, NULL TSFIN2028,  SPRES$POM FROM (";
                    sql = sql + "SELECT VES.SPPRO$ID_PRODUCTO, VES.SPPSUB$ID_SUBPRODUCTO, VES.SUBPRODUCTO, VES.MEDIDA_SUBPRODUCTO, VES.SPPSUB$SNIP ";
                    for (int i = 0; i < temp.Rows.Count; i++)
                    {
                        sql = sql + ",SCHE$SIPLAN20.FNC$IDMETAFISUB(" + temp.Rows[i]["SPOA$ID_POA"] + ", " + temp.Rows[i]["SPOA$ANIO"] + ", VES.SPPSUB$ID_SUBPRODUCTO) IDFISUB" + temp.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$IDMETAFINSUB(" + temp.Rows[i]["SPOA$ID_POA"] + ", " + temp.Rows[i]["SPOA$ANIO"] + ",VES.SPPSUB$ID_SUBPRODUCTO) IDFINSUB" + temp.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$OBTIENEMETAFISICASUB(" + temp.Rows[i]["SPOA$ID_POA"] + ", " + temp.Rows[i]["SPOA$ANIO"] + ",  VES.SPPSUB$ID_SUBPRODUCTO,0)  MFSUB" + temp.Rows[i]["SPOA$ANIO"] + ",  SCHE$SIPLAN20.FNC$METAFINANCIERASUB(" + temp.Rows[i]["SPOA$ID_POA"] + ", " + temp.Rows[i]["SPOA$ANIO"] + ", VES.SPPSUB$ID_SUBPRODUCTO,0) MFINSUB" + temp.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$TPFISSUB(" + temp.Rows[i]["SPOA$ID_POA"] + ", " + temp.Rows[i]["SPOA$ANIO"] + ", VES.SPPSUB$ID_SUBPRODUCTO) TSF" + temp.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$TPFINSUB(" + temp.Rows[i]["SPOA$ID_POA"] + ", " + temp.Rows[i]["SPOA$ANIO"] + ", VES.SPPSUB$ID_SUBPRODUCTO) TSFIN" + temp.Rows[i]["SPOA$ANIO"];
                    }
                    sql = sql + " ,VES.SPRES$POM ";
                    sql = sql + " FROM SCHE$SIPLAN20.SPPVST$SUBESTRATEGICOV2 VES WHERE VES.ID_RESULTADO =" + resultado + "  AND VES.SPPRO$ID_PROGRAMA_PRESUPUESTO = " + programa + " AND  VES.SPRES$POM = " + pom + " AND  VES.SPRES$INSTITUCION =  " + insto + " AND VES.SPPRO$ID_PRODUCTO =" + producto + ") GROUP BY SPPRO$ID_PRODUCTO, SPPSUB$ID_SUBPRODUCTO, SUBPRODUCTO, MEDIDA_SUBPRODUCTO, SPPSUB$SNIP ";
                    for (int i = 0; i < temp.Rows.Count; i++)
                    {
                        sql = sql + ",IDFISUB" + temp.Rows[i]["SPOA$ANIO"] + ", IDFINSUB" + temp.Rows[i]["SPOA$ANIO"] + ", MFSUB" + temp.Rows[i]["SPOA$ANIO"] + ", MFINSUB" + temp.Rows[i]["SPOA$ANIO"] + " ,TSF" + temp.Rows[i]["SPOA$ANIO"] + " ,TSFIN" + temp.Rows[i]["SPOA$ANIO"];
                    }
                    sql = sql + " ,SPRES$POM ORDER BY SPPRO$ID_PRODUCTO, SPPSUB$ID_SUBPRODUCTO ASC";
                }
                else if (tipo == 1)
                {
                    sql = "SELECT SPPRO$ID_PRODUCTO, SPPSUB$ID_SUBPRODUCTO, SUBPRODUCTO, MEDIDA_SUBPRODUCTO, SPPSUB$SNIP, NULL IDFISUB2020, NULL IDFINSUB2020, NULL MFSUB2020, NULL MFINSUB2020, NULL TSF2020, NULL TSFIN2020 ";
                    for (int i = 0; i < temp.Rows.Count; i++)
                    {
                        sql = sql + ",IDFISUB" + temp.Rows[i]["SPOA$ANIO"] + ", IDFINSUB" + temp.Rows[i]["SPOA$ANIO"] + ", MFSUB" + temp.Rows[i]["SPOA$ANIO"] + ", MFINSUB" + temp.Rows[i]["SPOA$ANIO"] + " ,TSF" + temp.Rows[i]["SPOA$ANIO"] + " ,TSFIN" + temp.Rows[i]["SPOA$ANIO"];
                    }
                    sql = sql + " ,NULL IDFISUB2026, NULL IDFINSUB2026, NULL MFSUB2026, NULL MFINSUB2026, NULL TSF2026, NULL TSFIN2026  ,NULL IDFISUB2027, NULL IDFINSUB2027, NULL MFSUB2027, NULL MFINSUB2027, NULL TSF2027, NULL TSFIN2027  ,NULL IDFISUB2028, NULL IDFINSUB2028, NULL MFSUB2028, NULL MFINSUB2028, NULL TSF2028, NULL TSFIN2028, SPRES$POM FROM (";
                    sql = sql + "SELECT VES.SPPRO$ID_PRODUCTO, VES.SPPSUB$ID_SUBPRODUCTO, VES.SUBPRODUCTO, VES.MEDIDA_SUBPRODUCTO, VES.SPPSUB$SNIP ";
                    for (int i = 0; i < temp.Rows.Count; i++)
                    {
                        sql = sql + ",SCHE$SIPLAN20.FNC$IDMETAFISUB(" + temp.Rows[i]["SPOA$ID_POA"] + ", " + temp.Rows[i]["SPOA$ANIO"] + ", VES.SPPSUB$ID_SUBPRODUCTO) IDFISUB" + temp.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$IDMETAFINSUB(" + temp.Rows[i]["SPOA$ID_POA"] + ", " + temp.Rows[i]["SPOA$ANIO"] + ",VES.SPPSUB$ID_SUBPRODUCTO) IDFINSUB" + temp.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$OBTIENEMETAFISICASUB(" + temp.Rows[i]["SPOA$ID_POA"] + ", " + temp.Rows[i]["SPOA$ANIO"] + ",  VES.SPPSUB$ID_SUBPRODUCTO,0)  MFSUB" + temp.Rows[i]["SPOA$ANIO"] + ",  SCHE$SIPLAN20.FNC$METAFINANCIERASUB(" + temp.Rows[i]["SPOA$ID_POA"] + ", " + temp.Rows[i]["SPOA$ANIO"] + ", VES.SPPSUB$ID_SUBPRODUCTO,0) MFINSUB" + temp.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$TPFISSUB(" + temp.Rows[i]["SPOA$ID_POA"] + ", " + temp.Rows[i]["SPOA$ANIO"] + ", VES.SPPSUB$ID_SUBPRODUCTO) TSF" + temp.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$TPFINSUB(" + temp.Rows[i]["SPOA$ID_POA"] + ", " + temp.Rows[i]["SPOA$ANIO"] + ", VES.SPPSUB$ID_SUBPRODUCTO) TSFIN" + temp.Rows[i]["SPOA$ANIO"];
                    }
                    sql = sql + " ,VES.SPRES$POM ";
                    sql = sql + " FROM SCHE$SIPLAN20.SPPSVT$RED_INSTITUCIONAL VES WHERE VES.ID_RESULTADO =" + resultado + "  AND VES.SPPRO$ID_PROGRAMA_PRESUPUESTO = " + programa + " AND  VES.SPRES$POM = " + pom + " AND  VES.SPRES$INSTITUCION =  " + insto + " AND VES.SPPRO$ID_PRODUCTO =" + producto + ") GROUP BY SPPRO$ID_PRODUCTO, SPPSUB$ID_SUBPRODUCTO, SUBPRODUCTO, MEDIDA_SUBPRODUCTO, SPPSUB$SNIP ";
                    for (int i = 0; i < temp.Rows.Count; i++)
                    {
                        sql = sql + ",IDFISUB" + temp.Rows[i]["SPOA$ANIO"] + ", IDFINSUB" + temp.Rows[i]["SPOA$ANIO"] + ", MFSUB" + temp.Rows[i]["SPOA$ANIO"] + ", MFINSUB" + temp.Rows[i]["SPOA$ANIO"] + " ,TSF" + temp.Rows[i]["SPOA$ANIO"] + " ,TSFIN" + temp.Rows[i]["SPOA$ANIO"];
                    }
                    sql = sql + " ,SPRES$POM ORDER BY SPPRO$ID_PRODUCTO, SPPSUB$ID_SUBPRODUCTO ASC";
                }
            }


            else if (Convert.ToInt32(Session["periodo"]) == 21)
            {
                if (tipo == 0)
                {
                    sql = "SELECT SPPRO$ID_PRODUCTO, SPPSUB$ID_SUBPRODUCTO, SUBPRODUCTO, MEDIDA_SUBPRODUCTO, SPPSUB$SNIP, NULL IDFISUB2020, NULL IDFINSUB2020, NULL MFSUB2020, NULL MFINSUB2020, NULL TSF2020, NULL TSFIN2020,  NULL IDFISUB2021, NULL IDFINSUB2021, NULL MFSUB2021, NULL MFINSUB2021, NULL TSF2021, NULL TSFIN2021 ";
                    for (int i = 0; i < temp.Rows.Count; i++)
                    {
                        sql = sql + ",IDFISUB" + temp.Rows[i]["SPOA$ANIO"] + ", IDFINSUB" + temp.Rows[i]["SPOA$ANIO"] + ", MFSUB" + temp.Rows[i]["SPOA$ANIO"] + ", MFINSUB" + temp.Rows[i]["SPOA$ANIO"] + ",TSF" + temp.Rows[i]["SPOA$ANIO"] + ",TSFIN" + temp.Rows[i]["SPOA$ANIO"];
                    }
                    sql = sql + " ,NULL IDFISUB2027, NULL IDFINSUB2027, NULL MFSUB2027, NULL MFINSUB2027, NULL TSF2027, NULL TSFIN2027  ,NULL IDFISUB2028, NULL IDFINSUB2028, NULL MFSUB2028, NULL MFINSUB2028, NULL TSF2028, NULL TSFIN2028,  SPRES$POM FROM (";
                    sql = sql + "SELECT VES.SPPRO$ID_PRODUCTO, VES.SPPSUB$ID_SUBPRODUCTO, VES.SUBPRODUCTO, VES.MEDIDA_SUBPRODUCTO, VES.SPPSUB$SNIP ";
                    for (int i = 0; i < temp.Rows.Count; i++)
                    {
                        sql = sql + ",SCHE$SIPLAN20.FNC$IDMETAFISUB(" + temp.Rows[i]["SPOA$ID_POA"] + ", " + temp.Rows[i]["SPOA$ANIO"] + ", VES.SPPSUB$ID_SUBPRODUCTO) IDFISUB" + temp.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$IDMETAFINSUB(" + temp.Rows[i]["SPOA$ID_POA"] + ", " + temp.Rows[i]["SPOA$ANIO"] + ",VES.SPPSUB$ID_SUBPRODUCTO) IDFINSUB" + temp.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$OBTIENEMETAFISICASUB(" + temp.Rows[i]["SPOA$ID_POA"] + ", " + temp.Rows[i]["SPOA$ANIO"] + ",  VES.SPPSUB$ID_SUBPRODUCTO,0)  MFSUB" + temp.Rows[i]["SPOA$ANIO"] + ",  SCHE$SIPLAN20.FNC$METAFINANCIERASUB(" + temp.Rows[i]["SPOA$ID_POA"] + ", " + temp.Rows[i]["SPOA$ANIO"] + ", VES.SPPSUB$ID_SUBPRODUCTO,0) MFINSUB" + temp.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$TPFISSUB(" + temp.Rows[i]["SPOA$ID_POA"] + ", " + temp.Rows[i]["SPOA$ANIO"] + ", VES.SPPSUB$ID_SUBPRODUCTO) TSF" + temp.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$TPFINSUB(" + temp.Rows[i]["SPOA$ID_POA"] + ", " + temp.Rows[i]["SPOA$ANIO"] + ", VES.SPPSUB$ID_SUBPRODUCTO) TSFIN" + temp.Rows[i]["SPOA$ANIO"];
                    }
                    sql = sql + " ,VES.SPRES$POM ";
                    sql = sql + " FROM SCHE$SIPLAN20.SPPVST$SUBESTRATEGICOV2 VES WHERE VES.ID_RESULTADO =" + resultado + "  AND VES.SPPRO$ID_PROGRAMA_PRESUPUESTO = " + programa + " AND  VES.SPRES$POM = " + pom + " AND  VES.SPRES$INSTITUCION =  " + insto + " AND VES.SPPRO$ID_PRODUCTO =" + producto + ") GROUP BY SPPRO$ID_PRODUCTO, SPPSUB$ID_SUBPRODUCTO, SUBPRODUCTO, MEDIDA_SUBPRODUCTO, SPPSUB$SNIP ";
                    for (int i = 0; i < temp.Rows.Count; i++)
                    {
                        sql = sql + ",IDFISUB" + temp.Rows[i]["SPOA$ANIO"] + ", IDFINSUB" + temp.Rows[i]["SPOA$ANIO"] + ", MFSUB" + temp.Rows[i]["SPOA$ANIO"] + ", MFINSUB" + temp.Rows[i]["SPOA$ANIO"] + " ,TSF" + temp.Rows[i]["SPOA$ANIO"] + " ,TSFIN" + temp.Rows[i]["SPOA$ANIO"];
                    }
                    sql = sql + " ,SPRES$POM ORDER BY SPPRO$ID_PRODUCTO, SPPSUB$ID_SUBPRODUCTO ASC";
                }
                else if (tipo == 1)
                {
                    sql = "SELECT SPPRO$ID_PRODUCTO, SPPSUB$ID_SUBPRODUCTO, SUBPRODUCTO, MEDIDA_SUBPRODUCTO, SPPSUB$SNIP, NULL IDFISUB2020, NULL IDFINSUB2020, NULL MFSUB2020, NULL MFINSUB2020, NULL TSF2020, NULL TSFIN2020,  NULL IDFISUB2021, NULL IDFINSUB2021, NULL MFSUB2021, NULL MFINSUB2021, NULL TSF2021, NULL TSFIN2021 ";
                    for (int i = 0; i < temp.Rows.Count; i++)
                    {
                        sql = sql + ",IDFISUB" + temp.Rows[i]["SPOA$ANIO"] + ", IDFINSUB" + temp.Rows[i]["SPOA$ANIO"] + ", MFSUB" + temp.Rows[i]["SPOA$ANIO"] + ", MFINSUB" + temp.Rows[i]["SPOA$ANIO"] + " ,TSF" + temp.Rows[i]["SPOA$ANIO"] + " ,TSFIN" + temp.Rows[i]["SPOA$ANIO"];
                    }
                    sql = sql + ",NULL IDFISUB2027, NULL IDFINSUB2027, NULL MFSUB2027, NULL MFINSUB2027, NULL TSF2027, NULL TSFIN2027,NULL IDFISUB2028, NULL IDFINSUB2028, NULL MFSUB2028, NULL MFINSUB2028, NULL TSF2028, NULL TSFIN2028,SPRES$POM FROM (";
                    sql = sql + "SELECT VES.SPPRO$ID_PRODUCTO, VES.SPPSUB$ID_SUBPRODUCTO, VES.SUBPRODUCTO, VES.MEDIDA_SUBPRODUCTO, VES.SPPSUB$SNIP ";
                    for (int i = 0; i < temp.Rows.Count; i++)
                    {
                        sql = sql + ",SCHE$SIPLAN20.FNC$IDMETAFISUB(" + temp.Rows[i]["SPOA$ID_POA"] + ", " + temp.Rows[i]["SPOA$ANIO"] + ", VES.SPPSUB$ID_SUBPRODUCTO) IDFISUB" + temp.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$IDMETAFINSUB(" + temp.Rows[i]["SPOA$ID_POA"] + ", " + temp.Rows[i]["SPOA$ANIO"] + ",VES.SPPSUB$ID_SUBPRODUCTO) IDFINSUB" + temp.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$OBTIENEMETAFISICASUB(" + temp.Rows[i]["SPOA$ID_POA"] + ", " + temp.Rows[i]["SPOA$ANIO"] + ",  VES.SPPSUB$ID_SUBPRODUCTO,0)  MFSUB" + temp.Rows[i]["SPOA$ANIO"] + ",  SCHE$SIPLAN20.FNC$METAFINANCIERASUB(" + temp.Rows[i]["SPOA$ID_POA"] + ", " + temp.Rows[i]["SPOA$ANIO"] + ", VES.SPPSUB$ID_SUBPRODUCTO,0) MFINSUB" + temp.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$TPFISSUB(" + temp.Rows[i]["SPOA$ID_POA"] + ", " + temp.Rows[i]["SPOA$ANIO"] + ", VES.SPPSUB$ID_SUBPRODUCTO) TSF" + temp.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$TPFINSUB(" + temp.Rows[i]["SPOA$ID_POA"] + ", " + temp.Rows[i]["SPOA$ANIO"] + ", VES.SPPSUB$ID_SUBPRODUCTO) TSFIN" + temp.Rows[i]["SPOA$ANIO"];
                    }
                    sql = sql + " ,VES.SPRES$POM ";
                    sql = sql + " FROM SCHE$SIPLAN20.SPPSVT$RED_INSTITUCIONAL VES WHERE VES.ID_RESULTADO =" + resultado + "  AND VES.SPPRO$ID_PROGRAMA_PRESUPUESTO = " + programa + " AND  VES.SPRES$POM = " + pom + " AND  VES.SPRES$INSTITUCION =  " + insto + " AND VES.SPPRO$ID_PRODUCTO =" + producto + ") GROUP BY SPPRO$ID_PRODUCTO, SPPSUB$ID_SUBPRODUCTO, SUBPRODUCTO, MEDIDA_SUBPRODUCTO, SPPSUB$SNIP ";
                    for (int i = 0; i < temp.Rows.Count; i++)
                    {
                        sql = sql + ",IDFISUB" + temp.Rows[i]["SPOA$ANIO"] + ", IDFINSUB" + temp.Rows[i]["SPOA$ANIO"] + ", MFSUB" + temp.Rows[i]["SPOA$ANIO"] + ", MFINSUB" + temp.Rows[i]["SPOA$ANIO"] + " ,TSF" + temp.Rows[i]["SPOA$ANIO"] + " ,TSFIN" + temp.Rows[i]["SPOA$ANIO"];
                    }
                    sql = sql + " ,SPRES$POM ORDER BY SPPRO$ID_PRODUCTO, SPPSUB$ID_SUBPRODUCTO ASC";
                }
            }


            else if (Convert.ToInt32(Session["periodo"]) == 22)
            {
                if (tipo == 0)
                {
                    sql = "SELECT SPPRO$ID_PRODUCTO, SPPSUB$ID_SUBPRODUCTO, SUBPRODUCTO, MEDIDA_SUBPRODUCTO, SPPSUB$SNIP, NULL IDFISUB2020, NULL IDFINSUB2020, NULL MFSUB2020, NULL MFINSUB2020, NULL TSF2020, NULL TSFIN2020,  NULL IDFISUB2021, NULL IDFINSUB2021, NULL MFSUB2021, NULL MFINSUB2021, NULL TSF2021, NULL TSFIN2021,     NULL IDFISUB2022, NULL IDFINSUB2022, NULL MFSUB2022, NULL MFINSUB2022, NULL TSF2022, NULL TSFIN2022 ";
                    for (int i = 0; i < temp.Rows.Count; i++)
                    {
                        sql = sql + ",IDFISUB" + temp.Rows[i]["SPOA$ANIO"] + ", IDFINSUB" + temp.Rows[i]["SPOA$ANIO"] + ", MFSUB" + temp.Rows[i]["SPOA$ANIO"] + ", MFINSUB" + temp.Rows[i]["SPOA$ANIO"] + ",TSF" + temp.Rows[i]["SPOA$ANIO"] + ",TSFIN" + temp.Rows[i]["SPOA$ANIO"];
                    }
                    sql = sql + ",NULL IDFISUB2028, NULL IDFINSUB2028, NULL MFSUB2028, NULL MFINSUB2028, NULL TSF2028, NULL TSFIN2028,SPRES$POM FROM (";
                    sql = sql + "SELECT VES.SPPRO$ID_PRODUCTO, VES.SPPSUB$ID_SUBPRODUCTO, VES.SUBPRODUCTO, VES.MEDIDA_SUBPRODUCTO, VES.SPPSUB$SNIP ";
                    for (int i = 0; i < temp.Rows.Count; i++)
                    {
                        sql = sql + ",SCHE$SIPLAN20.FNC$IDMETAFISUB(" + temp.Rows[i]["SPOA$ID_POA"] + ", " + temp.Rows[i]["SPOA$ANIO"] + ", VES.SPPSUB$ID_SUBPRODUCTO) IDFISUB" + temp.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$IDMETAFINSUB(" + temp.Rows[i]["SPOA$ID_POA"] + ", " + temp.Rows[i]["SPOA$ANIO"] + ",VES.SPPSUB$ID_SUBPRODUCTO) IDFINSUB" + temp.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$OBTIENEMETAFISICASUB(" + temp.Rows[i]["SPOA$ID_POA"] + ", " + temp.Rows[i]["SPOA$ANIO"] + ",  VES.SPPSUB$ID_SUBPRODUCTO,0)  MFSUB" + temp.Rows[i]["SPOA$ANIO"] + ",  SCHE$SIPLAN20.FNC$METAFINANCIERASUB(" + temp.Rows[i]["SPOA$ID_POA"] + ", " + temp.Rows[i]["SPOA$ANIO"] + ", VES.SPPSUB$ID_SUBPRODUCTO,0) MFINSUB" + temp.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$TPFISSUB(" + temp.Rows[i]["SPOA$ID_POA"] + ", " + temp.Rows[i]["SPOA$ANIO"] + ", VES.SPPSUB$ID_SUBPRODUCTO) TSF" + temp.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$TPFINSUB(" + temp.Rows[i]["SPOA$ID_POA"] + ", " + temp.Rows[i]["SPOA$ANIO"] + ", VES.SPPSUB$ID_SUBPRODUCTO) TSFIN" + temp.Rows[i]["SPOA$ANIO"];
                    }
                    sql = sql + " ,VES.SPRES$POM ";
                    sql = sql + " FROM SCHE$SIPLAN20.SPPVST$SUBESTRATEGICOV2 VES WHERE VES.ID_RESULTADO =" + resultado + "  AND VES.SPPRO$ID_PROGRAMA_PRESUPUESTO = " + programa + " AND  VES.SPRES$POM = " + pom + " AND  VES.SPRES$INSTITUCION =  " + insto + " AND VES.SPPRO$ID_PRODUCTO =" + producto + ") GROUP BY SPPRO$ID_PRODUCTO, SPPSUB$ID_SUBPRODUCTO, SUBPRODUCTO, MEDIDA_SUBPRODUCTO, SPPSUB$SNIP ";
                    for (int i = 0; i < temp.Rows.Count; i++)
                    {
                        sql = sql + ",IDFISUB" + temp.Rows[i]["SPOA$ANIO"] + ", IDFINSUB" + temp.Rows[i]["SPOA$ANIO"] + ", MFSUB" + temp.Rows[i]["SPOA$ANIO"] + ", MFINSUB" + temp.Rows[i]["SPOA$ANIO"] + " ,TSF" + temp.Rows[i]["SPOA$ANIO"] + " ,TSFIN" + temp.Rows[i]["SPOA$ANIO"];
                    }
                    sql = sql + " ,SPRES$POM ORDER BY SPPRO$ID_PRODUCTO, SPPSUB$ID_SUBPRODUCTO ASC";
                }
                else if (tipo == 1)
                {
                    sql = "SELECT SPPRO$ID_PRODUCTO, SPPSUB$ID_SUBPRODUCTO, SUBPRODUCTO, MEDIDA_SUBPRODUCTO, SPPSUB$SNIP, NULL IDFISUB2020, NULL IDFINSUB2020, NULL MFSUB2020, NULL MFINSUB2020, NULL TSF2020, NULL TSFIN2020,  NULL IDFISUB2021, NULL IDFINSUB2021, NULL MFSUB2021, NULL MFINSUB2021, NULL TSF2021, NULL TSFIN2021,    NULL IDFISUB2022, NULL IDFINSUB2022, NULL MFSUB2022, NULL MFINSUB2022, NULL TSF2022, NULL TSFIN2022 ";
                    for (int i = 0; i < temp.Rows.Count; i++)
                    {
                        sql = sql + ",IDFISUB" + temp.Rows[i]["SPOA$ANIO"] + ", IDFINSUB" + temp.Rows[i]["SPOA$ANIO"] + ", MFSUB" + temp.Rows[i]["SPOA$ANIO"] + ", MFINSUB" + temp.Rows[i]["SPOA$ANIO"] + " ,TSF" + temp.Rows[i]["SPOA$ANIO"] + " ,TSFIN" + temp.Rows[i]["SPOA$ANIO"];
                    }
                    sql = sql + ",NULL IDFISUB2028, NULL IDFINSUB2028, NULL MFSUB2028, NULL MFINSUB2028, NULL TSF2028, NULL TSFIN2028,SPRES$POM FROM (";
                    sql = sql + "SELECT VES.SPPRO$ID_PRODUCTO, VES.SPPSUB$ID_SUBPRODUCTO, VES.SUBPRODUCTO, VES.MEDIDA_SUBPRODUCTO, VES.SPPSUB$SNIP ";
                    for (int i = 0; i < temp.Rows.Count; i++)
                    {
                        sql = sql + ",SCHE$SIPLAN20.FNC$IDMETAFISUB(" + temp.Rows[i]["SPOA$ID_POA"] + ", " + temp.Rows[i]["SPOA$ANIO"] + ", VES.SPPSUB$ID_SUBPRODUCTO) IDFISUB" + temp.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$IDMETAFINSUB(" + temp.Rows[i]["SPOA$ID_POA"] + ", " + temp.Rows[i]["SPOA$ANIO"] + ",VES.SPPSUB$ID_SUBPRODUCTO) IDFINSUB" + temp.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$OBTIENEMETAFISICASUB(" + temp.Rows[i]["SPOA$ID_POA"] + ", " + temp.Rows[i]["SPOA$ANIO"] + ",  VES.SPPSUB$ID_SUBPRODUCTO,0)  MFSUB" + temp.Rows[i]["SPOA$ANIO"] + ",  SCHE$SIPLAN20.FNC$METAFINANCIERASUB(" + temp.Rows[i]["SPOA$ID_POA"] + ", " + temp.Rows[i]["SPOA$ANIO"] + ", VES.SPPSUB$ID_SUBPRODUCTO,0) MFINSUB" + temp.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$TPFISSUB(" + temp.Rows[i]["SPOA$ID_POA"] + ", " + temp.Rows[i]["SPOA$ANIO"] + ", VES.SPPSUB$ID_SUBPRODUCTO) TSF" + temp.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$TPFINSUB(" + temp.Rows[i]["SPOA$ID_POA"] + ", " + temp.Rows[i]["SPOA$ANIO"] + ", VES.SPPSUB$ID_SUBPRODUCTO) TSFIN" + temp.Rows[i]["SPOA$ANIO"];
                    }
                    sql = sql + " ,VES.SPRES$POM ";
                    sql = sql + " FROM SCHE$SIPLAN20.SPPSVT$RED_INSTITUCIONAL VES WHERE VES.ID_RESULTADO =" + resultado + "  AND VES.SPPRO$ID_PROGRAMA_PRESUPUESTO = " + programa + " AND  VES.SPRES$POM = " + pom + " AND  VES.SPRES$INSTITUCION =  " + insto + " AND VES.SPPRO$ID_PRODUCTO =" + producto + ") GROUP BY SPPRO$ID_PRODUCTO, SPPSUB$ID_SUBPRODUCTO, SUBPRODUCTO, MEDIDA_SUBPRODUCTO, SPPSUB$SNIP ";
                    for (int i = 0; i < temp.Rows.Count; i++)
                    {
                        sql = sql + ",IDFISUB" + temp.Rows[i]["SPOA$ANIO"] + ", IDFINSUB" + temp.Rows[i]["SPOA$ANIO"] + ", MFSUB" + temp.Rows[i]["SPOA$ANIO"] + ", MFINSUB" + temp.Rows[i]["SPOA$ANIO"] + " ,TSF" + temp.Rows[i]["SPOA$ANIO"] + " ,TSFIN" + temp.Rows[i]["SPOA$ANIO"];
                    }
                    sql = sql + " ,SPRES$POM ORDER BY SPPRO$ID_PRODUCTO, SPPSUB$ID_SUBPRODUCTO ASC";
                }
            }



            else if (Convert.ToInt32(Session["periodo"]) == 23)
            {
                if (tipo == 0)
                {
                    sql = "SELECT SPPRO$ID_PRODUCTO, SPPSUB$ID_SUBPRODUCTO, SUBPRODUCTO, MEDIDA_SUBPRODUCTO, SPPSUB$SNIP, NULL IDFISUB2020, NULL IDFINSUB2020, NULL MFSUB2020, NULL MFINSUB2020, NULL TSF2020, NULL TSFIN2020,  NULL IDFISUB2021, NULL IDFINSUB2021, NULL MFSUB2021, NULL MFINSUB2021, NULL TSF2021, NULL TSFIN2021, NULL IDFISUB2022, NULL IDFINSUB2022, NULL MFSUB2022, NULL MFINSUB2022, NULL TSF2022, NULL TSFIN2022,NULL IDFISUB2023, NULL IDFINSUB2023, NULL MFSUB2023, NULL MFINSUB2023, NULL TSF2023, NULL TSFIN2023";
                    for (int i = 0; i < temp.Rows.Count; i++)
                    {
                        sql = sql + ",IDFISUB" + temp.Rows[i]["SPOA$ANIO"] + ", IDFINSUB" + temp.Rows[i]["SPOA$ANIO"] + ", MFSUB" + temp.Rows[i]["SPOA$ANIO"] + ", MFINSUB" + temp.Rows[i]["SPOA$ANIO"] + ",TSF" + temp.Rows[i]["SPOA$ANIO"] + ",TSFIN" + temp.Rows[i]["SPOA$ANIO"];
                    }
                    sql = sql + ",SPRES$POM FROM (";
                    sql = sql + "SELECT VES.SPPRO$ID_PRODUCTO, VES.SPPSUB$ID_SUBPRODUCTO, VES.SUBPRODUCTO, VES.MEDIDA_SUBPRODUCTO, VES.SPPSUB$SNIP ";
                    for (int i = 0; i < temp.Rows.Count; i++)
                    {
                        sql = sql + ",SCHE$SIPLAN20.FNC$IDMETAFISUB(" + temp.Rows[i]["SPOA$ID_POA"] + ", " + temp.Rows[i]["SPOA$ANIO"] + ", VES.SPPSUB$ID_SUBPRODUCTO) IDFISUB" + temp.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$IDMETAFINSUB(" + temp.Rows[i]["SPOA$ID_POA"] + ", " + temp.Rows[i]["SPOA$ANIO"] + ",VES.SPPSUB$ID_SUBPRODUCTO) IDFINSUB" + temp.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$OBTIENEMETAFISICASUB(" + temp.Rows[i]["SPOA$ID_POA"] + ", " + temp.Rows[i]["SPOA$ANIO"] + ",  VES.SPPSUB$ID_SUBPRODUCTO,0)  MFSUB" + temp.Rows[i]["SPOA$ANIO"] + ",  SCHE$SIPLAN20.FNC$METAFINANCIERASUB(" + temp.Rows[i]["SPOA$ID_POA"] + ", " + temp.Rows[i]["SPOA$ANIO"] + ", VES.SPPSUB$ID_SUBPRODUCTO,0) MFINSUB" + temp.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$TPFISSUB(" + temp.Rows[i]["SPOA$ID_POA"] + ", " + temp.Rows[i]["SPOA$ANIO"] + ", VES.SPPSUB$ID_SUBPRODUCTO) TSF" + temp.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$TPFINSUB(" + temp.Rows[i]["SPOA$ID_POA"] + ", " + temp.Rows[i]["SPOA$ANIO"] + ", VES.SPPSUB$ID_SUBPRODUCTO) TSFIN" + temp.Rows[i]["SPOA$ANIO"];
                    }
                    sql = sql + " ,VES.SPRES$POM ";
                    sql = sql + " FROM SCHE$SIPLAN20.SPPVST$SUBESTRATEGICOV2 VES WHERE VES.ID_RESULTADO =" + resultado + "  AND VES.SPPRO$ID_PROGRAMA_PRESUPUESTO = " + programa + " AND  VES.SPRES$POM = " + pom + " AND  VES.SPRES$INSTITUCION =  " + insto + " AND VES.SPPRO$ID_PRODUCTO =" + producto + ") GROUP BY SPPRO$ID_PRODUCTO, SPPSUB$ID_SUBPRODUCTO, SUBPRODUCTO, MEDIDA_SUBPRODUCTO, SPPSUB$SNIP ";
                    for (int i = 0; i < temp.Rows.Count; i++)
                    {
                        sql = sql + ",IDFISUB" + temp.Rows[i]["SPOA$ANIO"] + ", IDFINSUB" + temp.Rows[i]["SPOA$ANIO"] + ", MFSUB" + temp.Rows[i]["SPOA$ANIO"] + ", MFINSUB" + temp.Rows[i]["SPOA$ANIO"] + " ,TSF" + temp.Rows[i]["SPOA$ANIO"] + " ,TSFIN" + temp.Rows[i]["SPOA$ANIO"];
                    }
                    sql = sql + " ,SPRES$POM ORDER BY SPPRO$ID_PRODUCTO, SPPSUB$ID_SUBPRODUCTO ASC";
                }
                else if (tipo == 1)
                {
                    sql = "SELECT SPPRO$ID_PRODUCTO, SPPSUB$ID_SUBPRODUCTO, SUBPRODUCTO, MEDIDA_SUBPRODUCTO, SPPSUB$SNIP, NULL IDFISUB2020, NULL IDFINSUB2020, NULL MFSUB2020, NULL MFINSUB2020, NULL TSF2020, NULL TSFIN2020,  NULL IDFISUB2021, NULL IDFINSUB2021, NULL MFSUB2021, NULL MFINSUB2021, NULL TSF2021, NULL TSFIN2021,    NULL IDFISUB2022, NULL IDFINSUB2022, NULL MFSUB2022, NULL MFINSUB2022, NULL TSF2022, NULL TSFIN2022, NULL IDFISUB2023, NULL IDFINSUB2023, NULL MFSUB2023, NULL MFINSUB2023, NULL TSF2023, NULL TSFIN2023 ";
                    for (int i = 0; i < temp.Rows.Count; i++)
                    {
                        sql = sql + ",IDFISUB" + temp.Rows[i]["SPOA$ANIO"] + ", IDFINSUB" + temp.Rows[i]["SPOA$ANIO"] + ", MFSUB" + temp.Rows[i]["SPOA$ANIO"] + ", MFINSUB" + temp.Rows[i]["SPOA$ANIO"] + " ,TSF" + temp.Rows[i]["SPOA$ANIO"] + " ,TSFIN" + temp.Rows[i]["SPOA$ANIO"];
                    }
                    sql = sql + ",SPRES$POM FROM (";
                    sql = sql + "SELECT VES.SPPRO$ID_PRODUCTO, VES.SPPSUB$ID_SUBPRODUCTO, VES.SUBPRODUCTO, VES.MEDIDA_SUBPRODUCTO, VES.SPPSUB$SNIP ";
                    for (int i = 0; i < temp.Rows.Count; i++)
                    {
                        sql = sql + ",SCHE$SIPLAN20.FNC$IDMETAFISUB(" + temp.Rows[i]["SPOA$ID_POA"] + ", " + temp.Rows[i]["SPOA$ANIO"] + ", VES.SPPSUB$ID_SUBPRODUCTO) IDFISUB" + temp.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$IDMETAFINSUB(" + temp.Rows[i]["SPOA$ID_POA"] + ", " + temp.Rows[i]["SPOA$ANIO"] + ",VES.SPPSUB$ID_SUBPRODUCTO) IDFINSUB" + temp.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$OBTIENEMETAFISICASUB(" + temp.Rows[i]["SPOA$ID_POA"] + ", " + temp.Rows[i]["SPOA$ANIO"] + ",  VES.SPPSUB$ID_SUBPRODUCTO,0)  MFSUB" + temp.Rows[i]["SPOA$ANIO"] + ",  SCHE$SIPLAN20.FNC$METAFINANCIERASUB(" + temp.Rows[i]["SPOA$ID_POA"] + ", " + temp.Rows[i]["SPOA$ANIO"] + ", VES.SPPSUB$ID_SUBPRODUCTO,0) MFINSUB" + temp.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$TPFISSUB(" + temp.Rows[i]["SPOA$ID_POA"] + ", " + temp.Rows[i]["SPOA$ANIO"] + ", VES.SPPSUB$ID_SUBPRODUCTO) TSF" + temp.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$TPFINSUB(" + temp.Rows[i]["SPOA$ID_POA"] + ", " + temp.Rows[i]["SPOA$ANIO"] + ", VES.SPPSUB$ID_SUBPRODUCTO) TSFIN" + temp.Rows[i]["SPOA$ANIO"];
                    }
                    sql = sql + " ,VES.SPRES$POM ";
                    sql = sql + " FROM SCHE$SIPLAN20.SPPSVT$RED_INSTITUCIONAL VES WHERE VES.ID_RESULTADO =" + resultado + "  AND VES.SPPRO$ID_PROGRAMA_PRESUPUESTO = " + programa + " AND  VES.SPRES$POM = " + pom + " AND  VES.SPRES$INSTITUCION =  " + insto + " AND VES.SPPRO$ID_PRODUCTO =" + producto + ") GROUP BY SPPRO$ID_PRODUCTO, SPPSUB$ID_SUBPRODUCTO, SUBPRODUCTO, MEDIDA_SUBPRODUCTO, SPPSUB$SNIP ";
                    for (int i = 0; i < temp.Rows.Count; i++)
                    {
                        sql = sql + ",IDFISUB" + temp.Rows[i]["SPOA$ANIO"] + ", IDFINSUB" + temp.Rows[i]["SPOA$ANIO"] + ", MFSUB" + temp.Rows[i]["SPOA$ANIO"] + ", MFINSUB" + temp.Rows[i]["SPOA$ANIO"] + " ,TSF" + temp.Rows[i]["SPOA$ANIO"] + " ,TSFIN" + temp.Rows[i]["SPOA$ANIO"];
                    }
                    sql = sql + " ,SPRES$POM ORDER BY SPPRO$ID_PRODUCTO, SPPSUB$ID_SUBPRODUCTO ASC";
                }
            }

            //NUEVA PGG
            else if (Convert.ToInt32(Session["periodo"]) == 24)
            {
                if (tipo == 0)
                {
                    sql = "SELECT SPPRO$ID_PRODUCTO, SPPSUB$ID_SUBPRODUCTO, SUBPRODUCTO, MEDIDA_SUBPRODUCTO, SPPSUB$SNIP, NULL IDFISUB2020, NULL IDFINSUB2020, NULL MFSUB2020, NULL MFINSUB2020, NULL TSF2020, NULL TSFIN2020,  NULL IDFISUB2021, NULL IDFINSUB2021, NULL MFSUB2021, NULL MFINSUB2021, NULL TSF2021, NULL TSFIN2021, NULL IDFISUB2022, NULL IDFINSUB2022, NULL MFSUB2022, NULL MFINSUB2022, NULL TSF2022, NULL TSFIN2022,NULL IDFISUB2023, NULL IDFINSUB2023, NULL MFSUB2023, NULL MFINSUB2023, NULL TSF2023, NULL TSFIN2023,  NULL IDFISUB2024, NULL IDFINSUB2024, NULL MFSUB2024, NULL MFINSUB2024, NULL TSF2024, NULL TSFIN2024";
                    for (int i = 0; i < temp.Rows.Count; i++)
                    {
                        sql = sql + ",IDFISUB" + temp.Rows[i]["SPOA$ANIO"] + ", IDFINSUB" + temp.Rows[i]["SPOA$ANIO"] + ", MFSUB" + temp.Rows[i]["SPOA$ANIO"] + ", MFINSUB" + temp.Rows[i]["SPOA$ANIO"] + ",TSF" + temp.Rows[i]["SPOA$ANIO"] + ",TSFIN" + temp.Rows[i]["SPOA$ANIO"];
                    }
                    sql = sql + ",SPRES$POM FROM (";
                    sql = sql + "SELECT VES.SPPRO$ID_PRODUCTO, VES.SPPSUB$ID_SUBPRODUCTO, VES.SUBPRODUCTO, VES.MEDIDA_SUBPRODUCTO, VES.SPPSUB$SNIP ";
                    for (int i = 0; i < temp.Rows.Count; i++)
                    {
                        sql = sql + ",SCHE$SIPLAN20.FNC$IDMETAFISUB(" + temp.Rows[i]["SPOA$ID_POA"] + ", " + temp.Rows[i]["SPOA$ANIO"] + ", VES.SPPSUB$ID_SUBPRODUCTO) IDFISUB" + temp.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$IDMETAFINSUB(" + temp.Rows[i]["SPOA$ID_POA"] + ", " + temp.Rows[i]["SPOA$ANIO"] + ",VES.SPPSUB$ID_SUBPRODUCTO) IDFINSUB" + temp.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$OBTIENEMETAFISICASUB(" + temp.Rows[i]["SPOA$ID_POA"] + ", " + temp.Rows[i]["SPOA$ANIO"] + ",  VES.SPPSUB$ID_SUBPRODUCTO,0)  MFSUB" + temp.Rows[i]["SPOA$ANIO"] + ",  SCHE$SIPLAN20.FNC$METAFINANCIERASUB(" + temp.Rows[i]["SPOA$ID_POA"] + ", " + temp.Rows[i]["SPOA$ANIO"] + ", VES.SPPSUB$ID_SUBPRODUCTO,0) MFINSUB" + temp.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$TPFISSUB(" + temp.Rows[i]["SPOA$ID_POA"] + ", " + temp.Rows[i]["SPOA$ANIO"] + ", VES.SPPSUB$ID_SUBPRODUCTO) TSF" + temp.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$TPFINSUB(" + temp.Rows[i]["SPOA$ID_POA"] + ", " + temp.Rows[i]["SPOA$ANIO"] + ", VES.SPPSUB$ID_SUBPRODUCTO) TSFIN" + temp.Rows[i]["SPOA$ANIO"];
                    }
                    sql = sql + " ,VES.SPRES$POM ";
                    sql = sql + " FROM SCHE$SIPLAN20.SPPVST$SUBESTRATEGICOV2 VES WHERE VES.ID_RESULTADO =" + resultado + "  AND VES.SPPRO$ID_PROGRAMA_PRESUPUESTO = " + programa + " AND  VES.SPRES$POM = " + pom + " AND  VES.SPRES$INSTITUCION =  " + insto + " AND VES.SPPRO$ID_PRODUCTO =" + producto + ") GROUP BY SPPRO$ID_PRODUCTO, SPPSUB$ID_SUBPRODUCTO, SUBPRODUCTO, MEDIDA_SUBPRODUCTO, SPPSUB$SNIP ";
                    for (int i = 0; i < temp.Rows.Count; i++)
                    {
                        sql = sql + ",IDFISUB" + temp.Rows[i]["SPOA$ANIO"] + ", IDFINSUB" + temp.Rows[i]["SPOA$ANIO"] + ", MFSUB" + temp.Rows[i]["SPOA$ANIO"] + ", MFINSUB" + temp.Rows[i]["SPOA$ANIO"] + " ,TSF" + temp.Rows[i]["SPOA$ANIO"] + " ,TSFIN" + temp.Rows[i]["SPOA$ANIO"];
                    }
                    sql = sql + " ,SPRES$POM ORDER BY SPPRO$ID_PRODUCTO, SPPSUB$ID_SUBPRODUCTO ASC";
                }
                else if (tipo == 1)
                {
                    sql = "SELECT SPPRO$ID_PRODUCTO, SPPSUB$ID_SUBPRODUCTO, SUBPRODUCTO, MEDIDA_SUBPRODUCTO, SPPSUB$SNIP, NULL IDFISUB2020, NULL IDFINSUB2020, NULL MFSUB2020, NULL MFINSUB2020, NULL TSF2020, NULL TSFIN2020,  NULL IDFISUB2021, NULL IDFINSUB2021, NULL MFSUB2021, NULL MFINSUB2021, NULL TSF2021, NULL TSFIN2021,    NULL IDFISUB2022, NULL IDFINSUB2022, NULL MFSUB2022, NULL MFINSUB2022, NULL TSF2022, NULL TSFIN2022, NULL IDFISUB2023, NULL IDFINSUB2023, NULL MFSUB2023, NULL MFINSUB2023, NULL TSF2023, NULL TSFIN2023, NULL IDFISUB2024, NULL IDFINSUB2024, NULL MFSUB2024, NULL MFINSUB2024, NULL TSF2024, NULL TSFIN2024";
                    for (int i = 0; i < temp.Rows.Count; i++)
                    {
                        sql = sql + ",IDFISUB" + temp.Rows[i]["SPOA$ANIO"] + ", IDFINSUB" + temp.Rows[i]["SPOA$ANIO"] + ", MFSUB" + temp.Rows[i]["SPOA$ANIO"] + ", MFINSUB" + temp.Rows[i]["SPOA$ANIO"] + " ,TSF" + temp.Rows[i]["SPOA$ANIO"] + " ,TSFIN" + temp.Rows[i]["SPOA$ANIO"];
                    }
                    sql = sql + ",SPRES$POM FROM (";
                    sql = sql + "SELECT VES.SPPRO$ID_PRODUCTO, VES.SPPSUB$ID_SUBPRODUCTO, VES.SUBPRODUCTO, VES.MEDIDA_SUBPRODUCTO, VES.SPPSUB$SNIP ";
                    for (int i = 0; i < temp.Rows.Count; i++)
                    {
                        sql = sql + ",SCHE$SIPLAN20.FNC$IDMETAFISUB(" + temp.Rows[i]["SPOA$ID_POA"] + ", " + temp.Rows[i]["SPOA$ANIO"] + ", VES.SPPSUB$ID_SUBPRODUCTO) IDFISUB" + temp.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$IDMETAFINSUB(" + temp.Rows[i]["SPOA$ID_POA"] + ", " + temp.Rows[i]["SPOA$ANIO"] + ",VES.SPPSUB$ID_SUBPRODUCTO) IDFINSUB" + temp.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$OBTIENEMETAFISICASUB(" + temp.Rows[i]["SPOA$ID_POA"] + ", " + temp.Rows[i]["SPOA$ANIO"] + ",  VES.SPPSUB$ID_SUBPRODUCTO,0)  MFSUB" + temp.Rows[i]["SPOA$ANIO"] + ",  SCHE$SIPLAN20.FNC$METAFINANCIERASUB(" + temp.Rows[i]["SPOA$ID_POA"] + ", " + temp.Rows[i]["SPOA$ANIO"] + ", VES.SPPSUB$ID_SUBPRODUCTO,0) MFINSUB" + temp.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$TPFISSUB(" + temp.Rows[i]["SPOA$ID_POA"] + ", " + temp.Rows[i]["SPOA$ANIO"] + ", VES.SPPSUB$ID_SUBPRODUCTO) TSF" + temp.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$TPFINSUB(" + temp.Rows[i]["SPOA$ID_POA"] + ", " + temp.Rows[i]["SPOA$ANIO"] + ", VES.SPPSUB$ID_SUBPRODUCTO) TSFIN" + temp.Rows[i]["SPOA$ANIO"];
                    }
                    sql = sql + " ,VES.SPRES$POM ";
                    sql = sql + " FROM SCHE$SIPLAN20.SPPSVT$RED_INSTITUCIONAL VES WHERE VES.ID_RESULTADO =" + resultado + "  AND VES.SPPRO$ID_PROGRAMA_PRESUPUESTO = " + programa + " AND  VES.SPRES$POM = " + pom + " AND  VES.SPRES$INSTITUCION =  " + insto + " AND VES.SPPRO$ID_PRODUCTO =" + producto + ") GROUP BY SPPRO$ID_PRODUCTO, SPPSUB$ID_SUBPRODUCTO, SUBPRODUCTO, MEDIDA_SUBPRODUCTO, SPPSUB$SNIP ";
                    for (int i = 0; i < temp.Rows.Count; i++)
                    {
                        sql = sql + ",IDFISUB" + temp.Rows[i]["SPOA$ANIO"] + ", IDFINSUB" + temp.Rows[i]["SPOA$ANIO"] + ", MFSUB" + temp.Rows[i]["SPOA$ANIO"] + ", MFINSUB" + temp.Rows[i]["SPOA$ANIO"] + " ,TSF" + temp.Rows[i]["SPOA$ANIO"] + " ,TSFIN" + temp.Rows[i]["SPOA$ANIO"];
                    }
                    sql = sql + " ,SPRES$POM ORDER BY SPPRO$ID_PRODUCTO, SPPSUB$ID_SUBPRODUCTO ASC";
                }
            }


            else if (Convert.ToInt32(Session["periodo"]) == 25)
            {
                if (tipo == 0)
                {
                    sql = "SELECT SPPRO$ID_PRODUCTO, SPPSUB$ID_SUBPRODUCTO, SUBPRODUCTO, MEDIDA_SUBPRODUCTO, SPPSUB$SNIP, NULL IDFISUB2020, NULL IDFINSUB2020, NULL MFSUB2020, NULL MFINSUB2020, NULL TSF2020, NULL TSFIN2020,  NULL IDFISUB2021, NULL IDFINSUB2021, NULL MFSUB2021, NULL MFINSUB2021, NULL TSF2021, NULL TSFIN2021, NULL IDFISUB2022, NULL IDFINSUB2022, NULL MFSUB2022, NULL MFINSUB2022, NULL TSF2022, NULL TSFIN2022,NULL IDFISUB2023, NULL IDFINSUB2023, NULL MFSUB2023, NULL MFINSUB2023, NULL TSF2023, NULL TSFIN2023,  NULL IDFISUB2024, NULL IDFINSUB2024, NULL MFSUB2024, NULL MFINSUB2024, NULL TSF2024, NULL TSFIN2024,   NULL IDFISUB2025, NULL IDFINSUB2025, NULL MFSUB2025, NULL MFINSUB2025, NULL TSF2025, NULL TSFIN2025";
                    for (int i = 0; i < temp.Rows.Count; i++)
                    {
                        sql = sql + ",IDFISUB" + temp.Rows[i]["SPOA$ANIO"] + ", IDFINSUB" + temp.Rows[i]["SPOA$ANIO"] + ", MFSUB" + temp.Rows[i]["SPOA$ANIO"] + ", MFINSUB" + temp.Rows[i]["SPOA$ANIO"] + ",TSF" + temp.Rows[i]["SPOA$ANIO"] + ",TSFIN" + temp.Rows[i]["SPOA$ANIO"];
                    }
                    sql = sql + ",SPRES$POM FROM (";
                    sql = sql + "SELECT VES.SPPRO$ID_PRODUCTO, VES.SPPSUB$ID_SUBPRODUCTO, VES.SUBPRODUCTO, VES.MEDIDA_SUBPRODUCTO, VES.SPPSUB$SNIP ";
                    for (int i = 0; i < temp.Rows.Count; i++)
                    {
                        sql = sql + ",SCHE$SIPLAN20.FNC$IDMETAFISUB(" + temp.Rows[i]["SPOA$ID_POA"] + ", " + temp.Rows[i]["SPOA$ANIO"] + ", VES.SPPSUB$ID_SUBPRODUCTO) IDFISUB" + temp.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$IDMETAFINSUB(" + temp.Rows[i]["SPOA$ID_POA"] + ", " + temp.Rows[i]["SPOA$ANIO"] + ",VES.SPPSUB$ID_SUBPRODUCTO) IDFINSUB" + temp.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$OBTIENEMETAFISICASUB(" + temp.Rows[i]["SPOA$ID_POA"] + ", " + temp.Rows[i]["SPOA$ANIO"] + ",  VES.SPPSUB$ID_SUBPRODUCTO,0)  MFSUB" + temp.Rows[i]["SPOA$ANIO"] + ",  SCHE$SIPLAN20.FNC$METAFINANCIERASUB(" + temp.Rows[i]["SPOA$ID_POA"] + ", " + temp.Rows[i]["SPOA$ANIO"] + ", VES.SPPSUB$ID_SUBPRODUCTO,0) MFINSUB" + temp.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$TPFISSUB(" + temp.Rows[i]["SPOA$ID_POA"] + ", " + temp.Rows[i]["SPOA$ANIO"] + ", VES.SPPSUB$ID_SUBPRODUCTO) TSF" + temp.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$TPFINSUB(" + temp.Rows[i]["SPOA$ID_POA"] + ", " + temp.Rows[i]["SPOA$ANIO"] + ", VES.SPPSUB$ID_SUBPRODUCTO) TSFIN" + temp.Rows[i]["SPOA$ANIO"];
                    }
                    sql = sql + " ,VES.SPRES$POM ";
                    sql = sql + " FROM SCHE$SIPLAN20.SPPVST$SUBESTRATEGICOV2 VES WHERE VES.ID_RESULTADO =" + resultado + "  AND VES.SPPRO$ID_PROGRAMA_PRESUPUESTO = " + programa + " AND  VES.SPRES$POM = " + pom + " AND  VES.SPRES$INSTITUCION =  " + insto + " AND VES.SPPRO$ID_PRODUCTO =" + producto + ") GROUP BY SPPRO$ID_PRODUCTO, SPPSUB$ID_SUBPRODUCTO, SUBPRODUCTO, MEDIDA_SUBPRODUCTO, SPPSUB$SNIP ";
                    for (int i = 0; i < temp.Rows.Count; i++)
                    {
                        sql = sql + ",IDFISUB" + temp.Rows[i]["SPOA$ANIO"] + ", IDFINSUB" + temp.Rows[i]["SPOA$ANIO"] + ", MFSUB" + temp.Rows[i]["SPOA$ANIO"] + ", MFINSUB" + temp.Rows[i]["SPOA$ANIO"] + " ,TSF" + temp.Rows[i]["SPOA$ANIO"] + " ,TSFIN" + temp.Rows[i]["SPOA$ANIO"];
                    }
                    sql = sql + " ,SPRES$POM ORDER BY SPPRO$ID_PRODUCTO, SPPSUB$ID_SUBPRODUCTO ASC";
                }
                else if (tipo == 1)
                {
                    sql = "SELECT SPPRO$ID_PRODUCTO, SPPSUB$ID_SUBPRODUCTO, SUBPRODUCTO, MEDIDA_SUBPRODUCTO, SPPSUB$SNIP, NULL IDFISUB2020, NULL IDFINSUB2020, NULL MFSUB2020, NULL MFINSUB2020, NULL TSF2020, NULL TSFIN2020,  NULL IDFISUB2021, NULL IDFINSUB2021, NULL MFSUB2021, NULL MFINSUB2021, NULL TSF2021, NULL TSFIN2021,    NULL IDFISUB2022, NULL IDFINSUB2022, NULL MFSUB2022, NULL MFINSUB2022, NULL TSF2022, NULL TSFIN2022, NULL IDFISUB2023, NULL IDFINSUB2023, NULL MFSUB2023, NULL MFINSUB2023, NULL TSF2023, NULL TSFIN2023, NULL IDFISUB2024, NULL IDFINSUB2024, NULL MFSUB2024, NULL MFINSUB2024, NULL TSF2024, NULL TSFIN2024,   NULL IDFISUB2025, NULL IDFINSUB2025, NULL MFSUB2025, NULL MFINSUB2025, NULL TSF2025, NULL TSFIN2025";
                    for (int i = 0; i < temp.Rows.Count; i++)
                    {
                        sql = sql + ",IDFISUB" + temp.Rows[i]["SPOA$ANIO"] + ", IDFINSUB" + temp.Rows[i]["SPOA$ANIO"] + ", MFSUB" + temp.Rows[i]["SPOA$ANIO"] + ", MFINSUB" + temp.Rows[i]["SPOA$ANIO"] + " ,TSF" + temp.Rows[i]["SPOA$ANIO"] + " ,TSFIN" + temp.Rows[i]["SPOA$ANIO"];
                    }
                    sql = sql + ",SPRES$POM FROM (";
                    sql = sql + "SELECT VES.SPPRO$ID_PRODUCTO, VES.SPPSUB$ID_SUBPRODUCTO, VES.SUBPRODUCTO, VES.MEDIDA_SUBPRODUCTO, VES.SPPSUB$SNIP ";
                    for (int i = 0; i < temp.Rows.Count; i++)
                    {
                        sql = sql + ",SCHE$SIPLAN20.FNC$IDMETAFISUB(" + temp.Rows[i]["SPOA$ID_POA"] + ", " + temp.Rows[i]["SPOA$ANIO"] + ", VES.SPPSUB$ID_SUBPRODUCTO) IDFISUB" + temp.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$IDMETAFINSUB(" + temp.Rows[i]["SPOA$ID_POA"] + ", " + temp.Rows[i]["SPOA$ANIO"] + ",VES.SPPSUB$ID_SUBPRODUCTO) IDFINSUB" + temp.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$OBTIENEMETAFISICASUB(" + temp.Rows[i]["SPOA$ID_POA"] + ", " + temp.Rows[i]["SPOA$ANIO"] + ",  VES.SPPSUB$ID_SUBPRODUCTO,0)  MFSUB" + temp.Rows[i]["SPOA$ANIO"] + ",  SCHE$SIPLAN20.FNC$METAFINANCIERASUB(" + temp.Rows[i]["SPOA$ID_POA"] + ", " + temp.Rows[i]["SPOA$ANIO"] + ", VES.SPPSUB$ID_SUBPRODUCTO,0) MFINSUB" + temp.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$TPFISSUB(" + temp.Rows[i]["SPOA$ID_POA"] + ", " + temp.Rows[i]["SPOA$ANIO"] + ", VES.SPPSUB$ID_SUBPRODUCTO) TSF" + temp.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$TPFINSUB(" + temp.Rows[i]["SPOA$ID_POA"] + ", " + temp.Rows[i]["SPOA$ANIO"] + ", VES.SPPSUB$ID_SUBPRODUCTO) TSFIN" + temp.Rows[i]["SPOA$ANIO"];
                    }
                    sql = sql + " ,VES.SPRES$POM ";
                    sql = sql + " FROM SCHE$SIPLAN20.SPPSVT$RED_INSTITUCIONAL VES WHERE VES.ID_RESULTADO =" + resultado + "  AND VES.SPPRO$ID_PROGRAMA_PRESUPUESTO = " + programa + " AND  VES.SPRES$POM = " + pom + " AND  VES.SPRES$INSTITUCION =  " + insto + " AND VES.SPPRO$ID_PRODUCTO =" + producto + ") GROUP BY SPPRO$ID_PRODUCTO, SPPSUB$ID_SUBPRODUCTO, SUBPRODUCTO, MEDIDA_SUBPRODUCTO, SPPSUB$SNIP ";
                    for (int i = 0; i < temp.Rows.Count; i++)
                    {
                        sql = sql + ",IDFISUB" + temp.Rows[i]["SPOA$ANIO"] + ", IDFINSUB" + temp.Rows[i]["SPOA$ANIO"] + ", MFSUB" + temp.Rows[i]["SPOA$ANIO"] + ", MFINSUB" + temp.Rows[i]["SPOA$ANIO"] + " ,TSF" + temp.Rows[i]["SPOA$ANIO"] + " ,TSFIN" + temp.Rows[i]["SPOA$ANIO"];
                    }
                    sql = sql + " ,SPRES$POM ORDER BY SPPRO$ID_PRODUCTO, SPPSUB$ID_SUBPRODUCTO ASC";
                }
            }


            else
            {
                if (tipo == 0)
                {
                    sql = "SELECT SPPRO$ID_PRODUCTO, SPPSUB$ID_SUBPRODUCTO, SUBPRODUCTO, MEDIDA_SUBPRODUCTO, SPPSUB$SNIP ";
                    for (int i = 0; i < temp.Rows.Count; i++)
                    {
                        sql = sql + ",IDFISUB" + temp.Rows[i]["SPOA$ANIO"] + ", IDFINSUB" + temp.Rows[i]["SPOA$ANIO"] + ", MFSUB" + temp.Rows[i]["SPOA$ANIO"] + ", MFINSUB" + temp.Rows[i]["SPOA$ANIO"] + ",TSF" + temp.Rows[i]["SPOA$ANIO"] + ",TSFIN" + temp.Rows[i]["SPOA$ANIO"];
                    }
                    sql = sql + " ,NULL IDFINSUB2025, NULL MFSUB2025, NULL MFINSUB2025, NULL TSF2025, NULL TSFIN2025  ,NULL IDFINSUB2026, NULL MFSUB2026, NULL MFINSUB2026, NULL TSF2026, NULL TSFIN2026,    ,NULL IDFINSUB2027, NULL MFSUB2027, NULL MFINSUB2027, NULL TSF2027, NULL TSFIN2027,    SPRES$POM FROM (";
                    sql = sql + "SELECT VES.SPPRO$ID_PRODUCTO, VES.SPPSUB$ID_SUBPRODUCTO, VES.SUBPRODUCTO, VES.MEDIDA_SUBPRODUCTO, VES.SPPSUB$SNIP ";
                    for (int i = 0; i < temp.Rows.Count; i++)
                    {
                        sql = sql + ",SCHE$SIPLAN20.FNC$IDMETAFISUB(" + temp.Rows[i]["SPOA$ID_POA"] + ", " + temp.Rows[i]["SPOA$ANIO"] + ", VES.SPPSUB$ID_SUBPRODUCTO) IDFISUB" + temp.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$IDMETAFINSUB(" + temp.Rows[i]["SPOA$ID_POA"] + ", " + temp.Rows[i]["SPOA$ANIO"] + ",VES.SPPSUB$ID_SUBPRODUCTO) IDFINSUB" + temp.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$OBTIENEMETAFISICASUB(" + temp.Rows[i]["SPOA$ID_POA"] + ", " + temp.Rows[i]["SPOA$ANIO"] + ",  VES.SPPSUB$ID_SUBPRODUCTO,0)  MFSUB" + temp.Rows[i]["SPOA$ANIO"] + ",  SCHE$SIPLAN20.FNC$METAFINANCIERASUB(" + temp.Rows[i]["SPOA$ID_POA"] + ", " + temp.Rows[i]["SPOA$ANIO"] + ", VES.SPPSUB$ID_SUBPRODUCTO,0) MFINSUB" + temp.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$TPFISSUB(" + temp.Rows[i]["SPOA$ID_POA"] + ", " + temp.Rows[i]["SPOA$ANIO"] + ", VES.SPPSUB$ID_SUBPRODUCTO) TSF" + temp.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$TPFINSUB(" + temp.Rows[i]["SPOA$ID_POA"] + ", " + temp.Rows[i]["SPOA$ANIO"] + ", VES.SPPSUB$ID_SUBPRODUCTO) TSFIN" + temp.Rows[i]["SPOA$ANIO"];
                    }
                    sql = sql + " ,VES.SPRES$POM ";
                    sql = sql + " FROM SCHE$SIPLAN20.SPPVST$SUBESTRATEGICOV2 VES WHERE VES.ID_RESULTADO =" + resultado + "  AND VES.SPPRO$ID_PROGRAMA_PRESUPUESTO = " + programa + " AND  VES.SPRES$POM = " + pom + " AND  VES.SPRES$INSTITUCION =  " + insto + " AND VES.SPPRO$ID_PRODUCTO =" + producto + ") GROUP BY SPPRO$ID_PRODUCTO, SPPSUB$ID_SUBPRODUCTO, SUBPRODUCTO, MEDIDA_SUBPRODUCTO, SPPSUB$SNIP ";
                    for (int i = 0; i < temp.Rows.Count; i++)
                    {
                        sql = sql + ",IDFISUB" + temp.Rows[i]["SPOA$ANIO"] + ", IDFINSUB" + temp.Rows[i]["SPOA$ANIO"] + ", MFSUB" + temp.Rows[i]["SPOA$ANIO"] + ", MFINSUB" + temp.Rows[i]["SPOA$ANIO"] + " ,TSF" + temp.Rows[i]["SPOA$ANIO"] + " ,TSFIN" + temp.Rows[i]["SPOA$ANIO"];
                    }
                    sql = sql + " ,SPRES$POM ORDER BY SPPRO$ID_PRODUCTO, SPPSUB$ID_SUBPRODUCTO ASC";
                }
                else if (tipo == 1)
                {
                    sql = "SELECT SPPRO$ID_PRODUCTO, SPPSUB$ID_SUBPRODUCTO, SUBPRODUCTO, MEDIDA_SUBPRODUCTO, SPPSUB$SNIP ";
                    for (int i = 0; i < temp.Rows.Count; i++)
                    {
                        sql = sql + ",IDFISUB" + temp.Rows[i]["SPOA$ANIO"] + ", IDFINSUB" + temp.Rows[i]["SPOA$ANIO"] + ", MFSUB" + temp.Rows[i]["SPOA$ANIO"] + ", MFINSUB" + temp.Rows[i]["SPOA$ANIO"] + " ,TSF" + temp.Rows[i]["SPOA$ANIO"] + " ,TSFIN" + temp.Rows[i]["SPOA$ANIO"];
                    }
                    sql = sql + ",NULL IDFINSUB2025, NULL MFSUB2025, NULL MFINSUB2025, NULL TSF2025, NULL TSFIN2025,  NULL IDFINSUB2026, NULL MFSUB2026, NULL MFINSUB2026, NULL TSF2026, NULL TSFIN2026,     ,NULL IDFINSUB2027, NULL MFSUB2027, NULL MFINSUB2027, NULL TSF2027, NULL TSFIN2027,     SPRES$POM FROM (";
                    sql = sql + "SELECT VES.SPPRO$ID_PRODUCTO, VES.SPPSUB$ID_SUBPRODUCTO, VES.SUBPRODUCTO, VES.MEDIDA_SUBPRODUCTO, VES.SPPSUB$SNIP ";
                    for (int i = 0; i < temp.Rows.Count; i++)
                    {
                        sql = sql + ",SCHE$SIPLAN20.FNC$IDMETAFISUB(" + temp.Rows[i]["SPOA$ID_POA"] + ", " + temp.Rows[i]["SPOA$ANIO"] + ", VES.SPPSUB$ID_SUBPRODUCTO) IDFISUB" + temp.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$IDMETAFINSUB(" + temp.Rows[i]["SPOA$ID_POA"] + ", " + temp.Rows[i]["SPOA$ANIO"] + ",VES.SPPSUB$ID_SUBPRODUCTO) IDFINSUB" + temp.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$OBTIENEMETAFISICASUB(" + temp.Rows[i]["SPOA$ID_POA"] + ", " + temp.Rows[i]["SPOA$ANIO"] + ",  VES.SPPSUB$ID_SUBPRODUCTO,0)  MFSUB" + temp.Rows[i]["SPOA$ANIO"] + ",  SCHE$SIPLAN20.FNC$METAFINANCIERASUB(" + temp.Rows[i]["SPOA$ID_POA"] + ", " + temp.Rows[i]["SPOA$ANIO"] + ", VES.SPPSUB$ID_SUBPRODUCTO,0) MFINSUB" + temp.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$TPFISSUB(" + temp.Rows[i]["SPOA$ID_POA"] + ", " + temp.Rows[i]["SPOA$ANIO"] + ", VES.SPPSUB$ID_SUBPRODUCTO) TSF" + temp.Rows[i]["SPOA$ANIO"] + ", SCHE$SIPLAN20.FNC$TPFINSUB(" + temp.Rows[i]["SPOA$ID_POA"] + ", " + temp.Rows[i]["SPOA$ANIO"] + ", VES.SPPSUB$ID_SUBPRODUCTO) TSFIN" + temp.Rows[i]["SPOA$ANIO"];
                    }
                    sql = sql + " ,VES.SPRES$POM ";
                    sql = sql + " FROM SCHE$SIPLAN20.SPPSVT$RED_INSTITUCIONAL VES WHERE VES.ID_RESULTADO =" + resultado + "  AND VES.SPPRO$ID_PROGRAMA_PRESUPUESTO = " + programa + " AND  VES.SPRES$POM = " + pom + " AND  VES.SPRES$INSTITUCION =  " + insto + " AND VES.SPPRO$ID_PRODUCTO =" + producto + ") GROUP BY SPPRO$ID_PRODUCTO, SPPSUB$ID_SUBPRODUCTO, SUBPRODUCTO, MEDIDA_SUBPRODUCTO, SPPSUB$SNIP ";
                    for (int i = 0; i < temp.Rows.Count; i++)
                    {
                        sql = sql + ",IDFISUB" + temp.Rows[i]["SPOA$ANIO"] + ", IDFINSUB" + temp.Rows[i]["SPOA$ANIO"] + ", MFSUB" + temp.Rows[i]["SPOA$ANIO"] + ", MFINSUB" + temp.Rows[i]["SPOA$ANIO"] + " ,TSF" + temp.Rows[i]["SPOA$ANIO"] + " ,TSFIN" + temp.Rows[i]["SPOA$ANIO"];
                    }
                    sql = sql + " ,SPRES$POM ORDER BY SPPRO$ID_PRODUCTO, SPPSUB$ID_SUBPRODUCTO ASC";
                }

            }



            estado = dao.consulta(sql);
            if (estado != 0)
                subproductos = dao.tabla;
            return subproductos;

        }

        protected DataTable cargasubproductosPOA(int anio, int tipo, int resultado, Double programa, int pom, int insto, int producto)
        {
            DataTable subproductos = new DataTable();
            DataTable poa = new DataTable();
            poa = obtienepoa(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), anio);
            if (poa.Rows.Count > 0)
            {
                if (tipo == 0)
                {
                    sql = "SELECT ID_RESULTADO, SPPRO$ID_PRODUCTO, SPPSUB$ID_SUBPRODUCTO, SUBPRODUCTO, MEDIDA_SUBPRODUCTO, SPPSUB$SNIP ";
                    for (int i = 1; i <= 12; i++)
                    {
                        sql = sql + ",IDFISUB" + i + ", IDFINSUB" + i + ", MFSUB" + i + ", MFINSUB" + i;
                    }
                    sql = sql + " ,SPRES$POM, ANUALFISICO, MFCUA1, MFCUA2, MFCUA3, ANUALFINANCIERO, MFINCUA1, MFINCUA2, MFINCUA3, ESTADOFISICO, ESTADOFINANCIERO FROM (";
                    sql = sql + "SELECT VES.ID_RESULTADO, VES.SPPRO$ID_PRODUCTO, VES.SPPSUB$ID_SUBPRODUCTO, VES.SUBPRODUCTO, VES.MEDIDA_SUBPRODUCTO, VES.SPPSUB$SNIP ";
                    for (int i = 1; i <= 12; i++)
                    {
                        sql = sql + ",SCHE$SIPLAN20.FNC$POAIDMETAFISSUB(" + poa.Rows[0]["SPOA$ID_POA"] + ", " + poa.Rows[0]["SPOA$ANIO"] + ", VES.SPPSUB$ID_SUBPRODUCTO, " + i + ") IDFISUB" + i + ", SCHE$SIPLAN20.FNC$POAIDMETAFINSUB(" + poa.Rows[0]["SPOA$ID_POA"] + ", " + poa.Rows[0]["SPOA$ANIO"] + ",VES.SPPSUB$ID_SUBPRODUCTO," + i + ") IDFINSUB" + i + ", SCHE$SIPLAN20.FNC$POAMETAFISSUB(" + poa.Rows[0]["SPOA$ID_POA"] + ", " + poa.Rows[0]["SPOA$ANIO"] + ",  VES.SPPSUB$ID_SUBPRODUCTO, " + i + ")  MFSUB" + i + ",  SCHE$SIPLAN20.FNC$POAMETAFINMENSUALSUB(" + poa.Rows[0]["SPOA$ID_POA"] + ", " + poa.Rows[0]["SPOA$ANIO"] + ", VES.SPPSUB$ID_SUBPRODUCTO, " + i + ") MFINSUB" + i;
                    }
                    sql = sql + " ,VES.SPRES$POM, SCHE$SIPLAN20.FNC$OBTIENEMETAFISICASUB(" + poa.Rows[0]["SPOA$ID_POA"] + ", " + poa.Rows[0]["SPOA$ANIO"] + ",   VES.SPPSUB$ID_SUBPRODUCTO,0) ANUALFISICO, SCHE$SIPLAN20.FNC$POAMETAFISCUATRIMESTRAL(" + poa.Rows[0]["SPOA$ID_POA"] + ", " + poa.Rows[0]["SPOA$ANIO"] + ",   VES.SPPSUB$ID_SUBPRODUCTO, 1, 4) MFCUA1,  SCHE$SIPLAN20.FNC$POAMETAFISCUATRIMESTRAL(" + poa.Rows[0]["SPOA$ID_POA"] + ", " + poa.Rows[0]["SPOA$ANIO"] + ",   VES.SPPSUB$ID_SUBPRODUCTO, 5, 8) MFCUA2, SCHE$SIPLAN20.FNC$POAMETAFISCUATRIMESTRAL(" + poa.Rows[0]["SPOA$ID_POA"] + ", " + poa.Rows[0]["SPOA$ANIO"] + ",   VES.SPPSUB$ID_SUBPRODUCTO, 9, 12) MFCUA3, SCHE$SIPLAN20.FNC$METAFINANCIERASUB(" + poa.Rows[0]["SPOA$ID_POA"] + ", " + poa.Rows[0]["SPOA$ANIO"] + ",   VES.SPPSUB$ID_SUBPRODUCTO,0) ANUALFINANCIERO, SCHE$SIPLAN20.FNC$POAMETAFINCUATSUB(" + poa.Rows[0]["SPOA$ID_POA"] + ", " + poa.Rows[0]["SPOA$ANIO"] + ",   VES.SPPSUB$ID_SUBPRODUCTO, 1, 4) MFINCUA1, SCHE$SIPLAN20.FNC$POAMETAFINCUATSUB(" + poa.Rows[0]["SPOA$ID_POA"] + ", " + poa.Rows[0]["SPOA$ANIO"] + ",   VES.SPPSUB$ID_SUBPRODUCTO, 5, 8) MFINCUA2, SCHE$SIPLAN20.FNC$POAMETAFINCUATSUB(" + poa.Rows[0]["SPOA$ID_POA"] + ", " + poa.Rows[0]["SPOA$ANIO"] + ",   VES.SPPSUB$ID_SUBPRODUCTO, 9, 12) MFINCUA3, SCHE$SIPLAN20.FNC$POAESTADOSUB(" + poa.Rows[0]["SPOA$ID_POA"] + ", " + poa.Rows[0]["SPOA$ANIO"] + ",   VES.SPPSUB$ID_SUBPRODUCTO, 1, 0) ESTADOFISICO,  SCHE$SIPLAN20.FNC$POAESTADOSUB(" + poa.Rows[0]["SPOA$ID_POA"] + ", " + poa.Rows[0]["SPOA$ANIO"] + ",   VES.SPPSUB$ID_SUBPRODUCTO, 1, 1) ESTADOFINANCIERO";
                    sql = sql + " FROM SCHE$SIPLAN20.SPPVST$SUBESTRATEGICOV2 VES WHERE VES.ID_RESULTADO =" + resultado + "  AND VES.SPPRO$ID_PROGRAMA_PRESUPUESTO = " + programa + " AND  VES.SPRES$POM = " + pom + " AND  VES.SPRES$INSTITUCION =  " + insto + " AND VES.SPPRO$ID_PRODUCTO =" + producto + ") GROUP BY ID_RESULTADO, SPPRO$ID_PRODUCTO, SPPSUB$ID_SUBPRODUCTO, SUBPRODUCTO, MEDIDA_SUBPRODUCTO, SPPSUB$SNIP ";
                    for (int i = 1; i <= 12; i++)
                    {
                        sql = sql + ",IDFISUB" + i + ", IDFINSUB" + i + ", MFSUB" + i + ", MFINSUB" + i;
                    }
                    sql = sql + " ,SPRES$POM, ANUALFISICO, MFCUA1, MFCUA2, MFCUA3, ANUALFINANCIERO, MFINCUA1, MFINCUA2, MFINCUA3, ESTADOFISICO, ESTADOFINANCIERO ORDER BY SPPRO$ID_PRODUCTO, SPPSUB$ID_SUBPRODUCTO ASC";
                }

                else if (tipo == 1)
                {
                    sql = "SELECT ID_RESULTADO, SPPRO$ID_PRODUCTO, SPPSUB$ID_SUBPRODUCTO, SUBPRODUCTO, MEDIDA_SUBPRODUCTO, SPPSUB$SNIP ";
                    for (int i = 1; i <= 12; i++)
                    {
                        sql = sql + ",IDFISUB" + i + ", IDFINSUB" + i + ", MFSUB" + i + ", MFINSUB" + i;
                    }
                    sql = sql + " ,SPRES$POM, ANUALFISICO, MFCUA1, MFCUA2, MFCUA3, ANUALFINANCIERO, MFINCUA1, MFINCUA2, MFINCUA3, ESTADOFISICO, ESTADOFINANCIERO FROM (";
                    sql = sql + "SELECT VES.ID_RESULTADO, VES.SPPRO$ID_PRODUCTO, VES.SPPSUB$ID_SUBPRODUCTO, VES.SUBPRODUCTO, VES.MEDIDA_SUBPRODUCTO, VES.SPPSUB$SNIP ";
                    for (int i = 1; i <= 12; i++)
                    {
                        sql = sql + ",SCHE$SIPLAN20.FNC$POAIDMETAFISSUB(" + poa.Rows[0]["SPOA$ID_POA"] + ", " + poa.Rows[0]["SPOA$ANIO"] + ", VES.SPPSUB$ID_SUBPRODUCTO, " + i + ") IDFISUB" + i + ", SCHE$SIPLAN20.FNC$POAIDMETAFINSUB(" + poa.Rows[0]["SPOA$ID_POA"] + ", " + poa.Rows[0]["SPOA$ANIO"] + ",VES.SPPSUB$ID_SUBPRODUCTO," + i + ") IDFINSUB" + i + ", SCHE$SIPLAN20.FNC$POAMETAFISSUB(" + poa.Rows[0]["SPOA$ID_POA"] + ", " + poa.Rows[0]["SPOA$ANIO"] + ",  VES.SPPSUB$ID_SUBPRODUCTO, " + i + ")  MFSUB" + i + ",  SCHE$SIPLAN20.FNC$POAMETAFINMENSUALSUB(" + poa.Rows[0]["SPOA$ID_POA"] + ", " + poa.Rows[0]["SPOA$ANIO"] + ", VES.SPPSUB$ID_SUBPRODUCTO, " + i + ") MFINSUB" + i;
                    }
                    sql = sql + " ,VES.SPRES$POM, SCHE$SIPLAN20.FNC$OBTIENEMETAFISICASUB(" + poa.Rows[0]["SPOA$ID_POA"] + ", " + poa.Rows[0]["SPOA$ANIO"] + ",   VES.SPPSUB$ID_SUBPRODUCTO,0) ANUALFISICO, SCHE$SIPLAN20.FNC$POAMETAFISCUATRIMESTRAL(" + poa.Rows[0]["SPOA$ID_POA"] + ", " + poa.Rows[0]["SPOA$ANIO"] + ",   VES.SPPSUB$ID_SUBPRODUCTO, 1, 4) MFCUA1,  SCHE$SIPLAN20.FNC$POAMETAFISCUATRIMESTRAL(" + poa.Rows[0]["SPOA$ID_POA"] + ", " + poa.Rows[0]["SPOA$ANIO"] + ",   VES.SPPSUB$ID_SUBPRODUCTO, 5, 8) MFCUA2, SCHE$SIPLAN20.FNC$POAMETAFISCUATRIMESTRAL(" + poa.Rows[0]["SPOA$ID_POA"] + ", " + poa.Rows[0]["SPOA$ANIO"] + ",   VES.SPPSUB$ID_SUBPRODUCTO, 9, 12) MFCUA3, SCHE$SIPLAN20.FNC$METAFINANCIERASUB(" + poa.Rows[0]["SPOA$ID_POA"] + ", " + poa.Rows[0]["SPOA$ANIO"] + ",   VES.SPPSUB$ID_SUBPRODUCTO,0) ANUALFINANCIERO, SCHE$SIPLAN20.FNC$POAMETAFINCUATSUB(" + poa.Rows[0]["SPOA$ID_POA"] + ", " + poa.Rows[0]["SPOA$ANIO"] + ",   VES.SPPSUB$ID_SUBPRODUCTO, 1, 4) MFINCUA1, SCHE$SIPLAN20.FNC$POAMETAFINCUATSUB(" + poa.Rows[0]["SPOA$ID_POA"] + ", " + poa.Rows[0]["SPOA$ANIO"] + ",   VES.SPPSUB$ID_SUBPRODUCTO, 5, 8) MFINCUA2, SCHE$SIPLAN20.FNC$POAMETAFINCUATSUB(" + poa.Rows[0]["SPOA$ID_POA"] + ", " + poa.Rows[0]["SPOA$ANIO"] + ",   VES.SPPSUB$ID_SUBPRODUCTO, 9, 12) MFINCUA3, SCHE$SIPLAN20.FNC$POAESTADOSUB(" + poa.Rows[0]["SPOA$ID_POA"] + ", " + poa.Rows[0]["SPOA$ANIO"] + ",   VES.SPPSUB$ID_SUBPRODUCTO, 1, 0) ESTADOFISICO,  SCHE$SIPLAN20.FNC$POAESTADOSUB(" + poa.Rows[0]["SPOA$ID_POA"] + ", " + poa.Rows[0]["SPOA$ANIO"] + ",   VES.SPPSUB$ID_SUBPRODUCTO, 1, 1) ESTADOFINANCIERO";
                    sql = sql + " FROM SCHE$SIPLAN20.SPPSVT$RED_INSTITUCIONAL VES WHERE VES.ID_RESULTADO =" + resultado + "  AND VES.SPPRO$ID_PROGRAMA_PRESUPUESTO = " + programa + " AND  VES.SPRES$POM = " + pom + " AND  VES.SPRES$INSTITUCION =  " + insto + " AND VES.SPPRO$ID_PRODUCTO =" + producto + ") GROUP BY ID_RESULTADO, SPPRO$ID_PRODUCTO, SPPSUB$ID_SUBPRODUCTO, SUBPRODUCTO, MEDIDA_SUBPRODUCTO, SPPSUB$SNIP ";
                    for (int i = 1; i <= 12; i++)
                    {
                        sql = sql + ",IDFISUB" + i + ", IDFINSUB" + i + ", MFSUB" + i + ", MFINSUB" + i;
                    }
                    sql = sql + " ,SPRES$POM, ANUALFISICO, MFCUA1, MFCUA2, MFCUA3, ANUALFINANCIERO, MFINCUA1, MFINCUA2, MFINCUA3, ESTADOFISICO, ESTADOFINANCIERO ORDER BY SPPRO$ID_PRODUCTO, SPPSUB$ID_SUBPRODUCTO ASC";
                }

                estado = dao.consulta(sql);
                if (estado != 0)
                {
                    subproductos = dao.tabla;
                }

            }
            return subproductos;
        }
        protected void gvProductos_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            Session["carga"] = 1;
            ASPxGridView grid = sender as ASPxGridView;
            DataTable metas = new DataTable();
            DataTable poas = new DataTable();
            int fallos = 0;
            String mensajesFisico = "";
            if (Session["mfproductos"] != null)
            {
                metas = (DataTable)Session["mfproductos"];
                if (metas.Rows.Count > 0)
                {
                    for (int i = 0; i < metas.Rows.Count; i++)
                    {
                        if (Convert.ToInt32(Session["periodo"]) == 20)
                        {
                            if (metas.Rows[i]["SPPRO$ID_PRODUCTO"].ToString() == e.Keys["SPPRO$ID_PRODUCTO"].ToString())
                            {
                                //INICIO 2021
                                if (metas.Rows[i]["IDFP2021"] == DBNull.Value)
                                {
                                    //if (Convert.ToInt32(e.NewValues["MF2021"]) != 0)
                                    if (e.NewValues["MF2021"] != DBNull.Value)
                                    {
                                        poas = obtienepoa(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), 2021);
                                        estado = insertaMetaFisica(Convert.ToInt32(e.Keys["SPPRO$ID_PRODUCTO"]), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MF2021"]));
                                        if (estado == 0)
                                        {
                                            fallos++;
                                            mensajesFisico = mensajesFisico + " 2021 ";
                                        }

                                    }
                                }
                                else
                                {
                                    poas = obtienepoa(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), 2021);

                                    //if (Convert.ToInt32(e.NewValues["MF2021"]) == 0)
                                    if (e.NewValues["MF2021"] == DBNull.Value)
                                    {
                                        estado = eliminaMetafisica(Convert.ToInt32(e.Keys["SPPRO$ID_PRODUCTO"]), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToInt32(metas.Rows[i]["IDFP2021"]));
                                    }
                                    else
                                    {
                                        estado = AcualizametafiscaProd(Convert.ToInt32(e.Keys["SPPRO$ID_PRODUCTO"]), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MF2021"]), Convert.ToInt32(metas.Rows[i]["IDFP2021"]));
                                    }

                                    if (estado == 0)
                                    {
                                        fallos++;
                                        mensajesFisico = mensajesFisico + " 2021 ";
                                    }
                                }
                                //FIN 2021

                                //INICIO 2022
                                if (metas.Rows[i]["IDFP2022"] == DBNull.Value)
                                {
                                    //if (Convert.ToInt32(e.NewValues["MF2022"]) != 0)
                                    if (e.NewValues["MF2022"] != DBNull.Value)
                                    {
                                        poas = obtienepoa(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), 2022);
                                        estado = insertaMetaFisica(Convert.ToInt32(e.Keys["SPPRO$ID_PRODUCTO"]), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MF2022"]));
                                        if (estado == 0)
                                        {
                                            fallos++;
                                            mensajesFisico = mensajesFisico + " 2022 ";
                                        }


                                    }
                                }
                                else
                                {
                                    poas = obtienepoa(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), 2022);
                                    //if (Convert.ToInt32(e.NewValues["MF2022"]) == 0)
                                    if (e.NewValues["MF2022"] == DBNull.Value)
                                    {
                                        estado = eliminaMetafisica(Convert.ToInt32(e.Keys["SPPRO$ID_PRODUCTO"]), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToInt32(metas.Rows[i]["IDFP2022"]));
                                    }
                                    else
                                    {
                                        estado = AcualizametafiscaProd(Convert.ToInt32(e.Keys["SPPRO$ID_PRODUCTO"]), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MF2022"]), Convert.ToInt32(metas.Rows[i]["IDFP2022"]));
                                    }

                                    if (estado == 0)
                                    {
                                        fallos++;
                                        mensajesFisico = mensajesFisico + " 2022 ";
                                    }
                                }
                                //FIN 2022

                                //INICIO 2023
                                if (metas.Rows[i]["IDFP2023"] == DBNull.Value)
                                {
                                    //if (Convert.ToInt32(e.NewValues["MF2023"]) != 0)
                                    if (e.NewValues["MF2023"] != DBNull.Value)
                                    {
                                        poas = obtienepoa(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), 2023);
                                        estado = insertaMetaFisica(Convert.ToInt32(e.Keys["SPPRO$ID_PRODUCTO"]), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MF2023"]));
                                        if (estado == 0)
                                        {
                                            fallos++;
                                            mensajesFisico = mensajesFisico + " 2023 ";
                                        }
                                    }
                                }
                                else
                                {
                                    poas = obtienepoa(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), 2023);
                                    //if (Convert.ToInt32(e.NewValues["MF2023"]) == 0)
                                    if (e.NewValues["MF2023"] == DBNull.Value)
                                    {
                                        estado = eliminaMetafisica(Convert.ToInt32(e.Keys["SPPRO$ID_PRODUCTO"]), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToInt32(metas.Rows[i]["IDFP2023"]));
                                    }
                                    else
                                    {
                                        estado = AcualizametafiscaProd(Convert.ToInt32(e.Keys["SPPRO$ID_PRODUCTO"]), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MF2023"]), Convert.ToInt32(metas.Rows[i]["IDFP2023"]));
                                    }

                                    if (estado == 0)
                                    {
                                        fallos++;
                                        mensajesFisico = mensajesFisico + " 2023 ";
                                    }
                                }
                                //FIN 2023

                                //INICIO 2024
                                if (metas.Rows[i]["IDFP2024"] == DBNull.Value)
                                {
                                    //if (Convert.ToInt32(e.NewValues["MF2024"]) != 0)
                                    if (e.NewValues["MF2024"] != DBNull.Value)
                                    {
                                        poas = obtienepoa(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), 2024);
                                        estado = insertaMetaFisica(Convert.ToInt32(e.Keys["SPPRO$ID_PRODUCTO"]), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MF2024"]));
                                        if (estado == 0)
                                        {
                                            fallos++;
                                            mensajesFisico = mensajesFisico + " 2024 ";
                                        }
                                    }
                                }
                                else
                                {
                                    poas = obtienepoa(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), 2024);
                                    //if (Convert.ToInt32(e.NewValues["MF2024"]) == 0)
                                    if (e.NewValues["MF2024"] == DBNull.Value)
                                    {
                                        estado = eliminaMetafisica(Convert.ToInt32(e.Keys["SPPRO$ID_PRODUCTO"]), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToInt32(metas.Rows[i]["IDFP2024"]));
                                    }
                                    else
                                    {
                                        estado = AcualizametafiscaProd(Convert.ToInt32(e.Keys["SPPRO$ID_PRODUCTO"]), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MF2024"]), Convert.ToInt32(metas.Rows[i]["IDFP2024"]));
                                    }

                                    if (estado == 0)
                                    {
                                        fallos++;
                                        mensajesFisico = mensajesFisico + " 2024 ";
                                    }
                                }
                                //FIN 2024

                                //INICIO 2025
                                if (metas.Rows[i]["IDFP2025"] == DBNull.Value)
                                {
                                    //if (Convert.ToInt32(e.NewValues["MF2025"]) != 0)
                                    if (e.NewValues["MF2025"] != DBNull.Value)
                                    {
                                        poas = obtienepoa(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), 2025);
                                        estado = insertaMetaFisica(Convert.ToInt32(e.Keys["SPPRO$ID_PRODUCTO"]), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MF2025"]));
                                        if (estado == 0)
                                        {
                                            fallos++;
                                            mensajesFisico = mensajesFisico + " 2025 ";
                                        }
                                    }
                                }
                                else
                                {
                                    poas = obtienepoa(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), 2025);
                                    //if (Convert.ToInt32(e.NewValues["MF2025"]) == 0)
                                    if (e.NewValues["MF2025"] == DBNull.Value)
                                    {
                                        estado = eliminaMetafisica(Convert.ToInt32(e.Keys["SPPRO$ID_PRODUCTO"]), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToInt32(metas.Rows[i]["IDFP2025"]));
                                    }
                                    else
                                    {
                                        estado = AcualizametafiscaProd(Convert.ToInt32(e.Keys["SPPRO$ID_PRODUCTO"]), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MF2025"]), Convert.ToInt32(metas.Rows[i]["IDFP2025"]));
                                    }

                                    if (estado == 0)
                                    {
                                        fallos++;
                                        mensajesFisico = mensajesFisico + " 2025 ";
                                    }
                                }
                                //FIN 2025

                                if (fallos > 0)
                                {
                                    mensaje = "Error en la grabación de metas de los años: " + mensajesFisico;
                                    grid.JSProperties["cpError"] = "Información " + mensaje;
                                    e.Cancel = true;
                                    grid.CancelEdit();
                                    break;
                                }

                                else
                                {
                                    mensaje = "Metas guardadas correctamente";
                                    grid.JSProperties["cpError"] = "Información " + mensaje;
                                    e.Cancel = true;
                                    grid.CancelEdit();
                                    break;
                                }
                            }
                        }


                        else if (Convert.ToInt32(Session["periodo"]) == 21)
                        {
                            if (metas.Rows[i]["SPPRO$ID_PRODUCTO"].ToString() == e.Keys["SPPRO$ID_PRODUCTO"].ToString())
                            {
                                //INICIO 2022
                                if (metas.Rows[i]["IDFP2022"] == DBNull.Value)
                                {
                                    //if (Convert.ToInt32(e.NewValues["MF2022"]) != 0)
                                    if (e.NewValues["MF2022"] != DBNull.Value)
                                    {
                                        poas = obtienepoa(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), 2022);
                                        estado = insertaMetaFisica(Convert.ToInt32(e.Keys["SPPRO$ID_PRODUCTO"]), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MF2022"]));
                                        if (estado == 0)
                                        {
                                            fallos++;
                                            mensajesFisico = mensajesFisico + " 2022 ";
                                        }

                                    }
                                }
                                else
                                {
                                    poas = obtienepoa(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), 2022);

                                    //if (Convert.ToInt32(e.NewValues["MF2022"]) == 0)
                                    if (e.NewValues["MF2022"] == DBNull.Value)
                                    {
                                        estado = eliminaMetafisica(Convert.ToInt32(e.Keys["SPPRO$ID_PRODUCTO"]), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToInt32(metas.Rows[i]["IDFP2022"]));
                                    }
                                    else
                                    {
                                        estado = AcualizametafiscaProd(Convert.ToInt32(e.Keys["SPPRO$ID_PRODUCTO"]), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MF2022"]), Convert.ToInt32(metas.Rows[i]["IDFP2022"]));
                                    }

                                    if (estado == 0)
                                    {
                                        fallos++;
                                        mensajesFisico = mensajesFisico + " 2022 ";
                                    }
                                }
                                //FIN 2022

                                //INICIO 2023
                                if (metas.Rows[i]["IDFP2023"] == DBNull.Value)
                                {
                                    //if (Convert.ToInt32(e.NewValues["MF2023"]) != 0)
                                    if (e.NewValues["MF2023"] != DBNull.Value)
                                    {
                                        poas = obtienepoa(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), 2023);
                                        estado = insertaMetaFisica(Convert.ToInt32(e.Keys["SPPRO$ID_PRODUCTO"]), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MF2023"]));
                                        if (estado == 0)
                                        {
                                            fallos++;
                                            mensajesFisico = mensajesFisico + " 2023 ";
                                        }


                                    }
                                }
                                else
                                {
                                    poas = obtienepoa(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), 2023);
                                    //if (Convert.ToInt32(e.NewValues["MF2023"]) == 0)
                                    if (e.NewValues["MF2023"] == DBNull.Value)
                                    {
                                        estado = eliminaMetafisica(Convert.ToInt32(e.Keys["SPPRO$ID_PRODUCTO"]), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToInt32(metas.Rows[i]["IDFP2023"]));
                                    }
                                    else
                                    {
                                        estado = AcualizametafiscaProd(Convert.ToInt32(e.Keys["SPPRO$ID_PRODUCTO"]), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MF2023"]), Convert.ToInt32(metas.Rows[i]["IDFP2023"]));
                                    }

                                    if (estado == 0)
                                    {
                                        fallos++;
                                        mensajesFisico = mensajesFisico + " 2023 ";
                                    }
                                }
                                //FIN 2023

                                //INICIO 2024
                                if (metas.Rows[i]["IDFP2024"] == DBNull.Value)
                                {
                                    //if (Convert.ToInt32(e.NewValues["MF2024"]) != 0)
                                    if (e.NewValues["MF2024"] != DBNull.Value)

                                    {
                                        poas = obtienepoa(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), 2024);
                                        estado = insertaMetaFisica(Convert.ToInt32(e.Keys["SPPRO$ID_PRODUCTO"]), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MF2024"]));
                                        if (estado == 0)
                                        {
                                            fallos++;
                                            mensajesFisico = mensajesFisico + " 2024 ";
                                        }
                                    }
                                }
                                else
                                {
                                    poas = obtienepoa(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), 2024);
                                    //if (Convert.ToInt32(e.NewValues["MF2024"]) == 0)
                                    if (e.NewValues["MF2024"] == DBNull.Value)
                                    {
                                        estado = eliminaMetafisica(Convert.ToInt32(e.Keys["SPPRO$ID_PRODUCTO"]), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToInt32(metas.Rows[i]["IDFP2024"]));
                                    }
                                    else
                                    {
                                        estado = AcualizametafiscaProd(Convert.ToInt32(e.Keys["SPPRO$ID_PRODUCTO"]), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MF2024"]), Convert.ToInt32(metas.Rows[i]["IDFP2024"]));
                                    }

                                    if (estado == 0)
                                    {
                                        fallos++;
                                        mensajesFisico = mensajesFisico + " 2024 ";
                                    }
                                }
                                //FIN 2024

                                //INICIO 2025
                                if (metas.Rows[i]["IDFP2025"] == DBNull.Value)
                                {
                                    //if (Convert.ToInt32(e.NewValues["MF2025"]) != 0)
                                    if (e.NewValues["MF2025"] != DBNull.Value)
                                    {
                                        poas = obtienepoa(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), 2025);
                                        estado = insertaMetaFisica(Convert.ToInt32(e.Keys["SPPRO$ID_PRODUCTO"]), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MF2025"]));
                                        if (estado == 0)
                                        {
                                            fallos++;
                                            mensajesFisico = mensajesFisico + " 2025 ";
                                        }
                                    }
                                }
                                else
                                {
                                    poas = obtienepoa(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), 2025);
                                    //if (Convert.ToInt32(e.NewValues["MF2025"]) == 0)
                                    if (e.NewValues["MF2025"] == DBNull.Value)
                                    {
                                        estado = eliminaMetafisica(Convert.ToInt32(e.Keys["SPPRO$ID_PRODUCTO"]), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToInt32(metas.Rows[i]["IDFP2025"]));
                                    }
                                    else
                                    {
                                        estado = AcualizametafiscaProd(Convert.ToInt32(e.Keys["SPPRO$ID_PRODUCTO"]), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MF2025"]), Convert.ToInt32(metas.Rows[i]["IDFP2025"]));
                                    }

                                    if (estado == 0)
                                    {
                                        fallos++;
                                        mensajesFisico = mensajesFisico + " 2025 ";
                                    }
                                }
                                //FIN 2025

                                //INICIO 2026
                                if (metas.Rows[i]["IDFP2026"] == DBNull.Value)
                                {
                                    //if (Convert.ToInt32(e.NewValues["MF2026"]) != 0)
                                    if (e.NewValues["MF2026"] != DBNull.Value)
                                    {
                                        poas = obtienepoa(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), 2026);
                                        estado = insertaMetaFisica(Convert.ToInt32(e.Keys["SPPRO$ID_PRODUCTO"]), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MF2026"]));
                                        if (estado == 0)
                                        {
                                            fallos++;
                                            mensajesFisico = mensajesFisico + " 2026 ";
                                        }
                                    }
                                }
                                else
                                {
                                    poas = obtienepoa(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), 2026);
                                    //if (Convert.ToInt32(e.NewValues["MF2026"]) == 0)
                                    if (e.NewValues["MF2026"] == DBNull.Value)
                                    {
                                        estado = eliminaMetafisica(Convert.ToInt32(e.Keys["SPPRO$ID_PRODUCTO"]), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToInt32(metas.Rows[i]["IDFP2026"]));
                                    }
                                    else
                                    {
                                        estado = AcualizametafiscaProd(Convert.ToInt32(e.Keys["SPPRO$ID_PRODUCTO"]), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MF2026"]), Convert.ToInt32(metas.Rows[i]["IDFP2026"]));
                                    }

                                    if (estado == 0)
                                    {
                                        fallos++;
                                        mensajesFisico = mensajesFisico + " 2026 ";
                                    }
                                }
                                //FIN 2026

                                if (fallos > 0)
                                {
                                    mensaje = "Error en la grabación de metas de los años: " + mensajesFisico;
                                    grid.JSProperties["cpError"] = "Información " + mensaje;
                                    e.Cancel = true;
                                    grid.CancelEdit();
                                    break;
                                }

                                else
                                {
                                    mensaje = "Metas guardadas correctamente";
                                    grid.JSProperties["cpError"] = "Información " + mensaje;
                                    e.Cancel = true;
                                    grid.CancelEdit();
                                    break;
                                }
                            }
                        }





                        else if (Convert.ToInt32(Session["periodo"]) == 22)
                        {
                            if (metas.Rows[i]["SPPRO$ID_PRODUCTO"].ToString() == e.Keys["SPPRO$ID_PRODUCTO"].ToString())
                            {

                                if (metas.Rows[i]["IDFP2023"] == DBNull.Value)
                                {
                                    //if (Convert.ToInt32(e.NewValues["MF2023"]) != 0)
                                    if (e.NewValues["MF2023"] != DBNull.Value)
                                    {
                                        poas = obtienepoa(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), 2023);
                                        estado = insertaMetaFisica(Convert.ToInt32(e.Keys["SPPRO$ID_PRODUCTO"]), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MF2023"]));
                                        if (estado == 0)
                                        {
                                            fallos++;
                                            mensajesFisico = mensajesFisico + " 2023 ";
                                        }


                                    }
                                }
                                else
                                {
                                    poas = obtienepoa(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), 2023);
                                    //if (Convert.ToInt32(e.NewValues["MF2023"]) == 0)
                                    if (e.NewValues["MF2023"] == DBNull.Value)
                                    {
                                        estado = eliminaMetafisica(Convert.ToInt32(e.Keys["SPPRO$ID_PRODUCTO"]), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToInt32(metas.Rows[i]["IDFP2023"]));
                                    }
                                    else
                                    {
                                        estado = AcualizametafiscaProd(Convert.ToInt32(e.Keys["SPPRO$ID_PRODUCTO"]), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MF2023"]), Convert.ToInt32(metas.Rows[i]["IDFP2023"]));
                                    }

                                    if (estado == 0)
                                    {
                                        fallos++;
                                        mensajesFisico = mensajesFisico + " 2023 ";
                                    }
                                }
                                //FIN 2023

                                //INICIO 2024
                                if (metas.Rows[i]["IDFP2024"] == DBNull.Value)
                                {
                                    //if (Convert.ToInt32(e.NewValues["MF2024"]) != 0)
                                    if (e.NewValues["MF2024"] != DBNull.Value)

                                    {
                                        poas = obtienepoa(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), 2024);
                                        estado = insertaMetaFisica(Convert.ToInt32(e.Keys["SPPRO$ID_PRODUCTO"]), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MF2024"]));
                                        if (estado == 0)
                                        {
                                            fallos++;
                                            mensajesFisico = mensajesFisico + " 2024 ";
                                        }
                                    }
                                }
                                else
                                {
                                    poas = obtienepoa(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), 2024);
                                    //if (Convert.ToInt32(e.NewValues["MF2024"]) == 0)
                                    if (e.NewValues["MF2024"] == DBNull.Value)
                                    {
                                        estado = eliminaMetafisica(Convert.ToInt32(e.Keys["SPPRO$ID_PRODUCTO"]), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToInt32(metas.Rows[i]["IDFP2024"]));
                                    }
                                    else
                                    {
                                        estado = AcualizametafiscaProd(Convert.ToInt32(e.Keys["SPPRO$ID_PRODUCTO"]), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MF2024"]), Convert.ToInt32(metas.Rows[i]["IDFP2024"]));
                                    }

                                    if (estado == 0)
                                    {
                                        fallos++;
                                        mensajesFisico = mensajesFisico + " 2024 ";
                                    }
                                }
                                //FIN 2024

                                //INICIO 2025
                                if (metas.Rows[i]["IDFP2025"] == DBNull.Value)
                                {
                                    //if (Convert.ToInt32(e.NewValues["MF2025"]) != 0)
                                    if (e.NewValues["MF2025"] != DBNull.Value)
                                    {
                                        poas = obtienepoa(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), 2025);
                                        estado = insertaMetaFisica(Convert.ToInt32(e.Keys["SPPRO$ID_PRODUCTO"]), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MF2025"]));
                                        if (estado == 0)
                                        {
                                            fallos++;
                                            mensajesFisico = mensajesFisico + " 2025 ";
                                        }
                                    }
                                }
                                else
                                {
                                    poas = obtienepoa(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), 2025);
                                    //if (Convert.ToInt32(e.NewValues["MF2025"]) == 0)
                                    if (e.NewValues["MF2025"] == DBNull.Value)
                                    {
                                        estado = eliminaMetafisica(Convert.ToInt32(e.Keys["SPPRO$ID_PRODUCTO"]), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToInt32(metas.Rows[i]["IDFP2025"]));
                                    }
                                    else
                                    {
                                        estado = AcualizametafiscaProd(Convert.ToInt32(e.Keys["SPPRO$ID_PRODUCTO"]), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MF2025"]), Convert.ToInt32(metas.Rows[i]["IDFP2025"]));
                                    }

                                    if (estado == 0)
                                    {
                                        fallos++;
                                        mensajesFisico = mensajesFisico + " 2025 ";
                                    }
                                }
                                //FIN 2025

                                //INICIO 2026
                                if (metas.Rows[i]["IDFP2026"] == DBNull.Value)
                                {
                                    //if (Convert.ToInt32(e.NewValues["MF2026"]) != 0)
                                    if (e.NewValues["MF2026"] != DBNull.Value)
                                    {
                                        poas = obtienepoa(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), 2026);
                                        estado = insertaMetaFisica(Convert.ToInt32(e.Keys["SPPRO$ID_PRODUCTO"]), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MF2026"]));
                                        if (estado == 0)
                                        {
                                            fallos++;
                                            mensajesFisico = mensajesFisico + " 2026 ";
                                        }
                                    }
                                }
                                else
                                {
                                    poas = obtienepoa(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), 2026);
                                    //if (Convert.ToInt32(e.NewValues["MF2026"]) == 0)
                                    if (e.NewValues["MF2026"] == DBNull.Value)
                                    {
                                        estado = eliminaMetafisica(Convert.ToInt32(e.Keys["SPPRO$ID_PRODUCTO"]), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToInt32(metas.Rows[i]["IDFP2026"]));
                                    }
                                    else
                                    {
                                        estado = AcualizametafiscaProd(Convert.ToInt32(e.Keys["SPPRO$ID_PRODUCTO"]), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MF2026"]), Convert.ToInt32(metas.Rows[i]["IDFP2026"]));
                                    }

                                    if (estado == 0)
                                    {
                                        fallos++;
                                        mensajesFisico = mensajesFisico + " 2026 ";
                                    }
                                }
                                //FIN 2026




                                if (metas.Rows[i]["IDFP2027"] == DBNull.Value)
                                {
                                    //if (Convert.ToInt32(e.NewValues["MF2022"]) != 0)
                                    if (e.NewValues["MF2027"] != DBNull.Value)
                                    {
                                        poas = obtienepoa(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), 2027);
                                        estado = insertaMetaFisica(Convert.ToInt32(e.Keys["SPPRO$ID_PRODUCTO"]), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MF2027"]));
                                        if (estado == 0)
                                        {
                                            fallos++;
                                            mensajesFisico = mensajesFisico + " 2027 ";
                                        }

                                    }
                                }
                                else
                                {
                                    poas = obtienepoa(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), 2027);

                                    //if (Convert.ToInt32(e.NewValues["MF2022"]) == 0)
                                    if (e.NewValues["MF2027"] == DBNull.Value)
                                    {
                                        estado = eliminaMetafisica(Convert.ToInt32(e.Keys["SPPRO$ID_PRODUCTO"]), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToInt32(metas.Rows[i]["IDFP2027"]));
                                    }
                                    else
                                    {
                                        estado = AcualizametafiscaProd(Convert.ToInt32(e.Keys["SPPRO$ID_PRODUCTO"]), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MF2027"]), Convert.ToInt32(metas.Rows[i]["IDFP2027"]));
                                    }

                                    if (estado == 0)
                                    {
                                        fallos++;
                                        mensajesFisico = mensajesFisico + " 2027 ";
                                    }
                                }





                                if (fallos > 0)
                                {
                                    mensaje = "Error en la grabación de metas de los años: " + mensajesFisico;
                                    grid.JSProperties["cpError"] = "Información " + mensaje;
                                    e.Cancel = true;
                                    grid.CancelEdit();
                                    break;
                                }

                                else
                                {
                                    mensaje = "Metas guardadas correctamente";
                                    grid.JSProperties["cpError"] = "Información " + mensaje;
                                    e.Cancel = true;
                                    grid.CancelEdit();
                                    break;
                                }
                            }
                        }


                        else if (Convert.ToInt32(Session["periodo"]) == 23)
                        {
                            if (metas.Rows[i]["SPPRO$ID_PRODUCTO"].ToString() == e.Keys["SPPRO$ID_PRODUCTO"].ToString())
                            {

                                if (metas.Rows[i]["IDFP2024"] == DBNull.Value)
                                {
                                    //if (Convert.ToInt32(e.NewValues["MF2023"]) != 0)
                                    if (e.NewValues["MF2024"] != DBNull.Value)
                                    {
                                        poas = obtienepoa(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), 2024);
                                        estado = insertaMetaFisica(Convert.ToInt32(e.Keys["SPPRO$ID_PRODUCTO"]), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MF2024"]));
                                        if (estado == 0)
                                        {
                                            fallos++;
                                            mensajesFisico = mensajesFisico + " 2024 ";
                                        }


                                    }
                                }
                                else
                                {
                                    poas = obtienepoa(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), 2024);
                                    //if (Convert.ToInt32(e.NewValues["MF2023"]) == 0)
                                    if (e.NewValues["MF2024"] == DBNull.Value)
                                    {
                                        estado = eliminaMetafisica(Convert.ToInt32(e.Keys["SPPRO$ID_PRODUCTO"]), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToInt32(metas.Rows[i]["IDFP2024"]));
                                    }
                                    else
                                    {
                                        estado = AcualizametafiscaProd(Convert.ToInt32(e.Keys["SPPRO$ID_PRODUCTO"]), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MF2024"]), Convert.ToInt32(metas.Rows[i]["IDFP2024"]));
                                    }

                                    if (estado == 0)
                                    {
                                        fallos++;
                                        mensajesFisico = mensajesFisico + " 2024 ";
                                    }
                                }
                                //FIN 2023

                                //INICIO 2024
                                if (metas.Rows[i]["IDFP2025"] == DBNull.Value)
                                {
                                    //if (Convert.ToInt32(e.NewValues["MF2024"]) != 0)
                                    if (e.NewValues["MF2025"] != DBNull.Value)

                                    {
                                        poas = obtienepoa(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), 2025);
                                        estado = insertaMetaFisica(Convert.ToInt32(e.Keys["SPPRO$ID_PRODUCTO"]), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MF2025"]));
                                        if (estado == 0)
                                        {
                                            fallos++;
                                            mensajesFisico = mensajesFisico + " 2025 ";
                                        }
                                    }
                                }
                                else
                                {
                                    poas = obtienepoa(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), 2025);
                                    //if (Convert.ToInt32(e.NewValues["MF2024"]) == 0)
                                    if (e.NewValues["MF2025"] == DBNull.Value)
                                    {
                                        estado = eliminaMetafisica(Convert.ToInt32(e.Keys["SPPRO$ID_PRODUCTO"]), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToInt32(metas.Rows[i]["IDFP2025"]));
                                    }
                                    else
                                    {
                                        estado = AcualizametafiscaProd(Convert.ToInt32(e.Keys["SPPRO$ID_PRODUCTO"]), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MF2025"]), Convert.ToInt32(metas.Rows[i]["IDFP2025"]));
                                    }

                                    if (estado == 0)
                                    {
                                        fallos++;
                                        mensajesFisico = mensajesFisico + " 2025 ";
                                    }
                                }
                                //FIN 2024

                                //INICIO 2025
                                if (metas.Rows[i]["IDFP2026"] == DBNull.Value)
                                {
                                    //if (Convert.ToInt32(e.NewValues["MF2025"]) != 0)
                                    if (e.NewValues["MF2026"] != DBNull.Value)
                                    {
                                        poas = obtienepoa(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), 2026);
                                        estado = insertaMetaFisica(Convert.ToInt32(e.Keys["SPPRO$ID_PRODUCTO"]), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MF2026"]));
                                        if (estado == 0)
                                        {
                                            fallos++;
                                            mensajesFisico = mensajesFisico + " 2026 ";
                                        }
                                    }
                                }
                                else
                                {
                                    poas = obtienepoa(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), 2026);
                                    //if (Convert.ToInt32(e.NewValues["MF2025"]) == 0)
                                    if (e.NewValues["MF2026"] == DBNull.Value)
                                    {
                                        estado = eliminaMetafisica(Convert.ToInt32(e.Keys["SPPRO$ID_PRODUCTO"]), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToInt32(metas.Rows[i]["IDFP2026"]));
                                    }
                                    else
                                    {
                                        estado = AcualizametafiscaProd(Convert.ToInt32(e.Keys["SPPRO$ID_PRODUCTO"]), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MF2026"]), Convert.ToInt32(metas.Rows[i]["IDFP2026"]));
                                    }

                                    if (estado == 0)
                                    {
                                        fallos++;
                                        mensajesFisico = mensajesFisico + " 2026 ";
                                    }
                                }
                                //FIN 2025

                                //INICIO 2026
                                if (metas.Rows[i]["IDFP2027"] == DBNull.Value)
                                {
                                    //if (Convert.ToInt32(e.NewValues["MF2026"]) != 0)
                                    if (e.NewValues["MF2027"] != DBNull.Value)
                                    {
                                        poas = obtienepoa(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), 2027);
                                        estado = insertaMetaFisica(Convert.ToInt32(e.Keys["SPPRO$ID_PRODUCTO"]), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MF2027"]));
                                        if (estado == 0)
                                        {
                                            fallos++;
                                            mensajesFisico = mensajesFisico + " 2027 ";
                                        }
                                    }
                                }
                                else
                                {
                                    poas = obtienepoa(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), 2027);
                                    //if (Convert.ToInt32(e.NewValues["MF2026"]) == 0)
                                    if (e.NewValues["MF2027"] == DBNull.Value)
                                    {
                                        estado = eliminaMetafisica(Convert.ToInt32(e.Keys["SPPRO$ID_PRODUCTO"]), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToInt32(metas.Rows[i]["IDFP2027"]));
                                    }
                                    else
                                    {
                                        estado = AcualizametafiscaProd(Convert.ToInt32(e.Keys["SPPRO$ID_PRODUCTO"]), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MF2027"]), Convert.ToInt32(metas.Rows[i]["IDFP2027"]));
                                    }

                                    if (estado == 0)
                                    {
                                        fallos++;
                                        mensajesFisico = mensajesFisico + " 2027 ";
                                    }
                                }
                                //FIN 2026




                                if (metas.Rows[i]["IDFP2028"] == DBNull.Value)
                                {
                                    //if (Convert.ToInt32(e.NewValues["MF2022"]) != 0)
                                    if (e.NewValues["MF2028"] != DBNull.Value)
                                    {
                                        poas = obtienepoa(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), 2028);
                                        estado = insertaMetaFisica(Convert.ToInt32(e.Keys["SPPRO$ID_PRODUCTO"]), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MF2028"]));
                                        if (estado == 0)
                                        {
                                            fallos++;
                                            mensajesFisico = mensajesFisico + " 2028 ";
                                        }

                                    }
                                }
                                else
                                {
                                    poas = obtienepoa(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), 2028);

                                    //if (Convert.ToInt32(e.NewValues["MF2022"]) == 0)
                                    if (e.NewValues["MF2028"] == DBNull.Value)
                                    {
                                        estado = eliminaMetafisica(Convert.ToInt32(e.Keys["SPPRO$ID_PRODUCTO"]), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToInt32(metas.Rows[i]["IDFP2028"]));
                                    }
                                    else
                                    {
                                        estado = AcualizametafiscaProd(Convert.ToInt32(e.Keys["SPPRO$ID_PRODUCTO"]), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MF2028"]), Convert.ToInt32(metas.Rows[i]["IDFP2028"]));
                                    }

                                    if (estado == 0)
                                    {
                                        fallos++;
                                        mensajesFisico = mensajesFisico + " 2028 ";
                                    }
                                }





                                if (fallos > 0)
                                {
                                    mensaje = "Error en la grabación de metas de los años: " + mensajesFisico;
                                    grid.JSProperties["cpError"] = "Información " + mensaje;
                                    e.Cancel = true;
                                    grid.CancelEdit();
                                    break;
                                }

                                else
                                {
                                    mensaje = "Metas guardadas correctamente";
                                    grid.JSProperties["cpError"] = "Información " + mensaje;
                                    e.Cancel = true;
                                    grid.CancelEdit();
                                    break;
                                }
                            }
                        }



                        //NUEVA PGG
                        else if (Convert.ToInt32(Session["periodo"]) == 24)
                        {
                            if (metas.Rows[i]["SPPRO$ID_PRODUCTO"].ToString() == e.Keys["SPPRO$ID_PRODUCTO"].ToString())
                            {

                                if (metas.Rows[i]["IDFP2025"] == DBNull.Value)
                                {
                                    //if (Convert.ToInt32(e.NewValues["MF2023"]) != 0)
                                    if (e.NewValues["MF2025"] != DBNull.Value)
                                    {
                                        poas = obtienepoa(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), 2025);
                                        estado = insertaMetaFisica(Convert.ToInt32(e.Keys["SPPRO$ID_PRODUCTO"]), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MF2025"]));
                                        if (estado == 0)
                                        {
                                            fallos++;
                                            mensajesFisico = mensajesFisico + " 2025 ";
                                        }


                                    }
                                }
                                else
                                {
                                    poas = obtienepoa(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), 2025);
                                    //if (Convert.ToInt32(e.NewValues["MF2023"]) == 0)
                                    if (e.NewValues["MF2025"] == DBNull.Value)
                                    {
                                        estado = eliminaMetafisica(Convert.ToInt32(e.Keys["SPPRO$ID_PRODUCTO"]), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToInt32(metas.Rows[i]["IDFP2025"]));
                                    }
                                    else
                                    {
                                        estado = AcualizametafiscaProd(Convert.ToInt32(e.Keys["SPPRO$ID_PRODUCTO"]), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MF2025"]), Convert.ToInt32(metas.Rows[i]["IDFP2025"]));
                                    }

                                    if (estado == 0)
                                    {
                                        fallos++;
                                        mensajesFisico = mensajesFisico + " 2025 ";
                                    }
                                }
                                //FIN 2025

                                //INICIO 2026
                                if (metas.Rows[i]["IDFP2026"] == DBNull.Value)
                                {
                                    //if (Convert.ToInt32(e.NewValues["MF2024"]) != 0)
                                    if (e.NewValues["MF2026"] != DBNull.Value)

                                    {
                                        poas = obtienepoa(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), 2026);
                                        estado = insertaMetaFisica(Convert.ToInt32(e.Keys["SPPRO$ID_PRODUCTO"]), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MF2026"]));
                                        if (estado == 0)
                                        {
                                            fallos++;
                                            mensajesFisico = mensajesFisico + " 2026 ";
                                        }
                                    }
                                }
                                else
                                {
                                    poas = obtienepoa(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), 2026);
                                    //if (Convert.ToInt32(e.NewValues["MF2024"]) == 0)
                                    if (e.NewValues["MF2026"] == DBNull.Value)
                                    {
                                        estado = eliminaMetafisica(Convert.ToInt32(e.Keys["SPPRO$ID_PRODUCTO"]), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToInt32(metas.Rows[i]["IDFP2026"]));
                                    }
                                    else
                                    {
                                        estado = AcualizametafiscaProd(Convert.ToInt32(e.Keys["SPPRO$ID_PRODUCTO"]), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MF2026"]), Convert.ToInt32(metas.Rows[i]["IDFP2026"]));
                                    }

                                    if (estado == 0)
                                    {
                                        fallos++;
                                        mensajesFisico = mensajesFisico + " 2026 ";
                                    }
                                }
                                //FIN 2026

                                //INICIO 2027
                                if (metas.Rows[i]["IDFP2027"] == DBNull.Value)
                                {
                                    //if (Convert.ToInt32(e.NewValues["MF2025"]) != 0)
                                    if (e.NewValues["MF2027"] != DBNull.Value)
                                    {
                                        poas = obtienepoa(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), 2027);
                                        estado = insertaMetaFisica(Convert.ToInt32(e.Keys["SPPRO$ID_PRODUCTO"]), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MF2027"]));
                                        if (estado == 0)
                                        {
                                            fallos++;
                                            mensajesFisico = mensajesFisico + " 2027 ";
                                        }
                                    }
                                }
                                else
                                {
                                    poas = obtienepoa(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), 2027);
                                    //if (Convert.ToInt32(e.NewValues["MF2025"]) == 0)
                                    if (e.NewValues["MF2027"] == DBNull.Value)
                                    {
                                        estado = eliminaMetafisica(Convert.ToInt32(e.Keys["SPPRO$ID_PRODUCTO"]), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToInt32(metas.Rows[i]["IDFP2027"]));
                                    }
                                    else
                                    {
                                        estado = AcualizametafiscaProd(Convert.ToInt32(e.Keys["SPPRO$ID_PRODUCTO"]), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MF2027"]), Convert.ToInt32(metas.Rows[i]["IDFP2027"]));
                                    }

                                    if (estado == 0)
                                    {
                                        fallos++;
                                        mensajesFisico = mensajesFisico + " 2027 ";
                                    }
                                }
                                //FIN 2027

                                //INICIO 2028
                                if (metas.Rows[i]["IDFP2028"] == DBNull.Value)
                                {
                                    //if (Convert.ToInt32(e.NewValues["MF2026"]) != 0)
                                    if (e.NewValues["MF2028"] != DBNull.Value)
                                    {
                                        poas = obtienepoa(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), 2028);
                                        estado = insertaMetaFisica(Convert.ToInt32(e.Keys["SPPRO$ID_PRODUCTO"]), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MF2028"]));
                                        if (estado == 0)
                                        {
                                            fallos++;
                                            mensajesFisico = mensajesFisico + " 2028 ";
                                        }
                                    }
                                }
                                else
                                {
                                    poas = obtienepoa(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), 2028);
                                    //if (Convert.ToInt32(e.NewValues["MF2026"]) == 0)
                                    if (e.NewValues["MF2028"] == DBNull.Value)
                                    {
                                        estado = eliminaMetafisica(Convert.ToInt32(e.Keys["SPPRO$ID_PRODUCTO"]), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToInt32(metas.Rows[i]["IDFP2028"]));
                                    }
                                    else
                                    {
                                        estado = AcualizametafiscaProd(Convert.ToInt32(e.Keys["SPPRO$ID_PRODUCTO"]), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MF2028"]), Convert.ToInt32(metas.Rows[i]["IDFP2028"]));
                                    }

                                    if (estado == 0)
                                    {
                                        fallos++;
                                        mensajesFisico = mensajesFisico + " 2028 ";
                                    }
                                }
                                //FIN 2028




                                if (metas.Rows[i]["IDFP2029"] == DBNull.Value)
                                {
                                    //if (Convert.ToInt32(e.NewValues["MF2022"]) != 0)
                                    if (e.NewValues["MF2029"] != DBNull.Value)
                                    {
                                        poas = obtienepoa(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), 2029);
                                        estado = insertaMetaFisica(Convert.ToInt32(e.Keys["SPPRO$ID_PRODUCTO"]), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MF2029"]));
                                        if (estado == 0)
                                        {
                                            fallos++;
                                            mensajesFisico = mensajesFisico + " 2029 ";
                                        }

                                    }
                                }
                                else
                                {
                                    poas = obtienepoa(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), 2029);

                                    //if (Convert.ToInt32(e.NewValues["MF2022"]) == 0)
                                    if (e.NewValues["MF2029"] == DBNull.Value)
                                    {
                                        estado = eliminaMetafisica(Convert.ToInt32(e.Keys["SPPRO$ID_PRODUCTO"]), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToInt32(metas.Rows[i]["IDFP2029"]));
                                    }
                                    else
                                    {
                                        estado = AcualizametafiscaProd(Convert.ToInt32(e.Keys["SPPRO$ID_PRODUCTO"]), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MF2029"]), Convert.ToInt32(metas.Rows[i]["IDFP2029"]));
                                    }

                                    if (estado == 0)
                                    {
                                        fallos++;
                                        mensajesFisico = mensajesFisico + " 2029 ";
                                    }
                                }





                                if (fallos > 0)
                                {
                                    mensaje = "Error en la grabación de metas de los años: " + mensajesFisico;
                                    grid.JSProperties["cpError"] = "Información " + mensaje;
                                    e.Cancel = true;
                                    grid.CancelEdit();
                                    break;
                                }

                                else
                                {
                                    mensaje = "Metas guardadas correctamente";
                                    grid.JSProperties["cpError"] = "Información " + mensaje;
                                    e.Cancel = true;
                                    grid.CancelEdit();
                                    break;
                                }
                            }
                        }

                        //NUEVA PGG
                        //INICIO 2026-2030

                        //NUEVA PGG
                        else if (Convert.ToInt32(Session["periodo"]) == 25)
                        {
                            if (metas.Rows[i]["SPPRO$ID_PRODUCTO"].ToString() == e.Keys["SPPRO$ID_PRODUCTO"].ToString())
                            {

                                if (metas.Rows[i]["IDFP2026"] == DBNull.Value)
                                {
                                    //if (Convert.ToInt32(e.NewValues["MF2023"]) != 0)
                                    if (e.NewValues["MF2026"] != DBNull.Value)
                                    {
                                        poas = obtienepoa(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), 2026);
                                        estado = insertaMetaFisica(Convert.ToInt32(e.Keys["SPPRO$ID_PRODUCTO"]), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MF2026"]));
                                        if (estado == 0)
                                        {
                                            fallos++;
                                            mensajesFisico = mensajesFisico + " 2026 ";
                                        }


                                    }
                                }
                                else
                                {
                                    poas = obtienepoa(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), 2026);
                                    //if (Convert.ToInt32(e.NewValues["MF2023"]) == 0)
                                    if (e.NewValues["MF2026"] == DBNull.Value)
                                    {
                                        estado = eliminaMetafisica(Convert.ToInt32(e.Keys["SPPRO$ID_PRODUCTO"]), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToInt32(metas.Rows[i]["IDFP2026"]));
                                    }
                                    else
                                    {
                                        estado = AcualizametafiscaProd(Convert.ToInt32(e.Keys["SPPRO$ID_PRODUCTO"]), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MF2026"]), Convert.ToInt32(metas.Rows[i]["IDFP2026"]));
                                    }

                                    if (estado == 0)
                                    {
                                        fallos++;
                                        mensajesFisico = mensajesFisico + " 2026 ";
                                    }
                                }
                                //FIN 2026

                                //INICIO 2027
                                if (metas.Rows[i]["IDFP2027"] == DBNull.Value)
                                {
                                    //if (Convert.ToInt32(e.NewValues["MF2024"]) != 0)
                                    if (e.NewValues["MF2027"] != DBNull.Value)

                                    {
                                        poas = obtienepoa(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), 2027);
                                        estado = insertaMetaFisica(Convert.ToInt32(e.Keys["SPPRO$ID_PRODUCTO"]), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MF2027"]));
                                        if (estado == 0)
                                        {
                                            fallos++;
                                            mensajesFisico = mensajesFisico + " 2027 ";
                                        }
                                    }
                                }
                                else
                                {
                                    poas = obtienepoa(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), 2027);
                                    //if (Convert.ToInt32(e.NewValues["MF2024"]) == 0)
                                    if (e.NewValues["MF2026"] == DBNull.Value)
                                    {
                                        estado = eliminaMetafisica(Convert.ToInt32(e.Keys["SPPRO$ID_PRODUCTO"]), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToInt32(metas.Rows[i]["IDFP2027"]));
                                    }
                                    else
                                    {
                                        estado = AcualizametafiscaProd(Convert.ToInt32(e.Keys["SPPRO$ID_PRODUCTO"]), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MF2027"]), Convert.ToInt32(metas.Rows[i]["IDFP2027"]));
                                    }

                                    if (estado == 0)
                                    {
                                        fallos++;
                                        mensajesFisico = mensajesFisico + " 2027 ";
                                    }
                                }
                                //FIN 2027

                                //INICIO 2028
                                if (metas.Rows[i]["IDFP2028"] == DBNull.Value)
                                {
                                    //if (Convert.ToInt32(e.NewValues["MF2025"]) != 0)
                                    if (e.NewValues["MF2028"] != DBNull.Value)
                                    {
                                        poas = obtienepoa(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), 2028);
                                        estado = insertaMetaFisica(Convert.ToInt32(e.Keys["SPPRO$ID_PRODUCTO"]), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MF2028"]));
                                        if (estado == 0)
                                        {
                                            fallos++;
                                            mensajesFisico = mensajesFisico + " 2028 ";
                                        }
                                    }
                                }
                                else
                                {
                                    poas = obtienepoa(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), 2028);
                                    //if (Convert.ToInt32(e.NewValues["MF2025"]) == 0)
                                    if (e.NewValues["MF2028"] == DBNull.Value)
                                    {
                                        estado = eliminaMetafisica(Convert.ToInt32(e.Keys["SPPRO$ID_PRODUCTO"]), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToInt32(metas.Rows[i]["IDFP2028"]));
                                    }
                                    else
                                    {
                                        estado = AcualizametafiscaProd(Convert.ToInt32(e.Keys["SPPRO$ID_PRODUCTO"]), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MF2028"]), Convert.ToInt32(metas.Rows[i]["IDFP2028"]));
                                    }

                                    if (estado == 0)
                                    {
                                        fallos++;
                                        mensajesFisico = mensajesFisico + " 2028 ";
                                    }
                                }
                                //FIN 2028

                                //INICIO 2028
                                if (metas.Rows[i]["IDFP2029"] == DBNull.Value)
                                {
                                    //if (Convert.ToInt32(e.NewValues["MF2026"]) != 0)
                                    if (e.NewValues["MF2029"] != DBNull.Value)
                                    {
                                        poas = obtienepoa(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), 2029);
                                        estado = insertaMetaFisica(Convert.ToInt32(e.Keys["SPPRO$ID_PRODUCTO"]), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MF2029"]));
                                        if (estado == 0)
                                        {
                                            fallos++;
                                            mensajesFisico = mensajesFisico + " 2029 ";
                                        }
                                    }
                                }
                                else
                                {
                                    poas = obtienepoa(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), 2029);
                                    //if (Convert.ToInt32(e.NewValues["MF2026"]) == 0)
                                    if (e.NewValues["MF2029"] == DBNull.Value)
                                    {
                                        estado = eliminaMetafisica(Convert.ToInt32(e.Keys["SPPRO$ID_PRODUCTO"]), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToInt32(metas.Rows[i]["IDFP2029"]));
                                    }
                                    else
                                    {
                                        estado = AcualizametafiscaProd(Convert.ToInt32(e.Keys["SPPRO$ID_PRODUCTO"]), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MF2029"]), Convert.ToInt32(metas.Rows[i]["IDFP2029"]));
                                    }

                                    if (estado == 0)
                                    {
                                        fallos++;
                                        mensajesFisico = mensajesFisico + " 2029 ";
                                    }
                                }
                                //FIN 2029




                                if (metas.Rows[i]["IDFP2030"] == DBNull.Value)
                                {
                                    //if (Convert.ToInt32(e.NewValues["MF2022"]) != 0)
                                    if (e.NewValues["MF2030"] != DBNull.Value)
                                    {
                                        poas = obtienepoa(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), 2030);
                                        estado = insertaMetaFisica(Convert.ToInt32(e.Keys["SPPRO$ID_PRODUCTO"]), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MF2030"]));
                                        if (estado == 0)
                                        {
                                            fallos++;
                                            mensajesFisico = mensajesFisico + " 2030 ";
                                        }

                                    }
                                }
                                else
                                {
                                    poas = obtienepoa(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), 2030);

                                    //if (Convert.ToInt32(e.NewValues["MF2022"]) == 0)
                                    if (e.NewValues["MF2030"] == DBNull.Value)
                                    {
                                        estado = eliminaMetafisica(Convert.ToInt32(e.Keys["SPPRO$ID_PRODUCTO"]), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToInt32(metas.Rows[i]["IDFP2030"]));
                                    }
                                    else
                                    {
                                        estado = AcualizametafiscaProd(Convert.ToInt32(e.Keys["SPPRO$ID_PRODUCTO"]), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MF2030"]), Convert.ToInt32(metas.Rows[i]["IDFP2030"]));
                                    }

                                    if (estado == 0)
                                    {
                                        fallos++;
                                        mensajesFisico = mensajesFisico + " 2030 ";
                                    }
                                }





                                if (fallos > 0)
                                {
                                    mensaje = "Error en la grabación de metas de los años: " + mensajesFisico;
                                    grid.JSProperties["cpError"] = "Información " + mensaje;
                                    e.Cancel = true;
                                    grid.CancelEdit();
                                    break;
                                }

                                else
                                {
                                    mensaje = "Metas guardadas correctamente";
                                    grid.JSProperties["cpError"] = "Información " + mensaje;
                                    e.Cancel = true;
                                    grid.CancelEdit();
                                    break;
                                }
                            }
                        }

                        //FIN 2026-2030


                        else
                        {
                            if (metas.Rows[i]["SPPRO$ID_PRODUCTO"].ToString() == e.Keys["SPPRO$ID_PRODUCTO"].ToString())
                            {
                                //INICIO 2020
                                if (metas.Rows[i]["IDFP2020"] == DBNull.Value)
                                {
                                    //if (Convert.ToInt32(e.NewValues["MF2020"]) != 0)
                                    if (e.NewValues["MF2020"] != DBNull.Value)
                                    {
                                        poas = obtienepoa(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), 2020);
                                        estado = insertaMetaFisica(Convert.ToInt32(e.Keys["SPPRO$ID_PRODUCTO"]), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MF2020"]));
                                        if (estado == 0)
                                        {
                                            fallos++;
                                            mensajesFisico = mensajesFisico + " 2020 ";
                                        }

                                    }
                                }
                                else
                                {
                                    poas = obtienepoa(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), 2020);

                                    //if (Convert.ToInt32(e.NewValues["MF2020"]) != 0)
                                    if (e.NewValues["MF2020"] != DBNull.Value)
                                    {
                                        estado = eliminaMetafisica(Convert.ToInt32(e.Keys["SPPRO$ID_PRODUCTO"]), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToInt32(metas.Rows[i]["IDFP2020"]));
                                    }
                                    else
                                    {
                                        estado = AcualizametafiscaProd(Convert.ToInt32(e.Keys["SPPRO$ID_PRODUCTO"]), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MF2020"]), Convert.ToInt32(metas.Rows[i]["IDFP2020"]));
                                    }

                                    if (estado == 0)
                                    {
                                        fallos++;
                                        mensajesFisico = mensajesFisico + " 2020 ";
                                    }
                                }
                                //FIN 2020

                                //INICIO 2021
                                if (metas.Rows[i]["IDFP2021"] == DBNull.Value)
                                {
                                    //if (Convert.ToInt32(e.NewValues["MF2021"]) != 0)
                                    if (e.NewValues["MF2021"] != DBNull.Value)
                                    {
                                        poas = obtienepoa(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), 2021);
                                        estado = insertaMetaFisica(Convert.ToInt32(e.Keys["SPPRO$ID_PRODUCTO"]), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MF2021"]));
                                        if (estado == 0)
                                        {
                                            fallos++;
                                            mensajesFisico = mensajesFisico + " 2021 ";
                                        }


                                    }
                                }
                                else
                                {
                                    poas = obtienepoa(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), 2021);
                                    //if (Convert.ToInt32(e.NewValues["MF2021"]) == 0)
                                    if (e.NewValues["MF2021"] == DBNull.Value)
                                    {
                                        estado = eliminaMetafisica(Convert.ToInt32(e.Keys["SPPRO$ID_PRODUCTO"]), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToInt32(metas.Rows[i]["IDFP2021"]));
                                    }
                                    else
                                    {
                                        estado = AcualizametafiscaProd(Convert.ToInt32(e.Keys["SPPRO$ID_PRODUCTO"]), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MF2021"]), Convert.ToInt32(metas.Rows[i]["IDFP2021"]));
                                    }

                                    if (estado == 0)
                                    {
                                        fallos++;
                                        mensajesFisico = mensajesFisico + " 2021 ";
                                    }
                                }
                                //FIN 2021

                                //INICIO 2022
                                if (metas.Rows[i]["IDFP2022"] == DBNull.Value)
                                {
                                    //if (Convert.ToInt32(e.NewValues["MF2022"]) != 0)
                                    if (e.NewValues["MF2022"] != DBNull.Value)
                                    {
                                        poas = obtienepoa(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), 2022);
                                        estado = insertaMetaFisica(Convert.ToInt32(e.Keys["SPPRO$ID_PRODUCTO"]), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MF2022"]));
                                        if (estado == 0)
                                        {
                                            fallos++;
                                            mensajesFisico = mensajesFisico + " 2022 ";
                                        }
                                    }
                                }
                                else
                                {
                                    poas = obtienepoa(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), 2022);
                                    //if (Convert.ToInt32(e.NewValues["MF2022"]) == 0)
                                    if (e.NewValues["MF2022"] == DBNull.Value)
                                    {
                                        estado = eliminaMetafisica(Convert.ToInt32(e.Keys["SPPRO$ID_PRODUCTO"]), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToInt32(metas.Rows[i]["IDFP2022"]));
                                    }
                                    else
                                    {
                                        estado = AcualizametafiscaProd(Convert.ToInt32(e.Keys["SPPRO$ID_PRODUCTO"]), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MF2022"]), Convert.ToInt32(metas.Rows[i]["IDFP2022"]));
                                    }

                                    if (estado == 0)
                                    {
                                        fallos++;
                                        mensajesFisico = mensajesFisico + " 2022 ";
                                    }
                                }
                                //FIN 2022

                                //INICIO 2023
                                if (metas.Rows[i]["IDFP2023"] == DBNull.Value)
                                {
                                    //if (Convert.ToInt32(e.NewValues["MF2023"]) != 0)
                                    if (e.NewValues["MF2023"] != DBNull.Value)
                                    {
                                        poas = obtienepoa(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), 2023);
                                        estado = insertaMetaFisica(Convert.ToInt32(e.Keys["SPPRO$ID_PRODUCTO"]), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MF2023"]));
                                        if (estado == 0)
                                        {
                                            fallos++;
                                            mensajesFisico = mensajesFisico + " 2023 ";
                                        }
                                    }
                                }
                                else
                                {
                                    poas = obtienepoa(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), 2023);
                                    //if (Convert.ToInt32(e.NewValues["MF2023"]) == 0)
                                    if (e.NewValues["MF2023"] == DBNull.Value)
                                    {
                                        estado = eliminaMetafisica(Convert.ToInt32(e.Keys["SPPRO$ID_PRODUCTO"]), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToInt32(metas.Rows[i]["IDFP2023"]));
                                    }
                                    else
                                    {
                                        estado = AcualizametafiscaProd(Convert.ToInt32(e.Keys["SPPRO$ID_PRODUCTO"]), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MF2023"]), Convert.ToInt32(metas.Rows[i]["IDFP2023"]));
                                    }

                                    if (estado == 0)
                                    {
                                        fallos++;
                                        mensajesFisico = mensajesFisico + " 2023 ";
                                    }
                                }
                                //FIN 2023

                                //INICIO 2024
                                if (metas.Rows[i]["IDFP2024"] == DBNull.Value)
                                {
                                    //if (Convert.ToInt32(e.NewValues["MF2024"]) != 0)
                                    if (e.NewValues["MF2024"] != DBNull.Value)
                                    {
                                        poas = obtienepoa(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), 2024);
                                        estado = insertaMetaFisica(Convert.ToInt32(e.Keys["SPPRO$ID_PRODUCTO"]), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MF2024"]));
                                        if (estado == 0)
                                        {
                                            fallos++;
                                            mensajesFisico = mensajesFisico + " 2024 ";
                                        }
                                    }
                                }
                                else
                                {
                                    poas = obtienepoa(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), 2024);
                                    //if (Convert.ToInt32(e.NewValues["MF2024"]) == 0)
                                    if (e.NewValues["MF2024"] == DBNull.Value)
                                    {
                                        estado = eliminaMetafisica(Convert.ToInt32(e.Keys["SPPRO$ID_PRODUCTO"]), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToInt32(metas.Rows[i]["IDFP2024"]));
                                    }
                                    else
                                    {
                                        estado = AcualizametafiscaProd(Convert.ToInt32(e.Keys["SPPRO$ID_PRODUCTO"]), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MF2024"]), Convert.ToInt32(metas.Rows[i]["IDFP2024"]));
                                    }

                                    if (estado == 0)
                                    {
                                        fallos++;
                                        mensajesFisico = mensajesFisico + " 2024 ";
                                    }
                                }
                                //FIN 2024

                                if (fallos > 0)
                                {
                                    mensaje = "Error en la grabación de metas de los años: " + mensajesFisico;
                                    grid.JSProperties["cpError"] = "Información " + mensaje;
                                    e.Cancel = true;
                                    grid.CancelEdit();
                                    break;
                                }

                                else
                                {
                                    mensaje = "Metas guardadas correctamente";
                                    grid.JSProperties["cpError"] = "Información " + mensaje;
                                    e.Cancel = true;
                                    grid.CancelEdit();
                                    break;
                                }
                            }

                        }


                    }
                }
                else
                {
                    mensaje = "Error en la sesión";
                    grid.JSProperties["cpError"] = "Información " + mensaje;
                    e.Cancel = true;
                    grid.CancelEdit();

                }
            }
            else
            {
                mensaje = "Error en la sesión";
                grid.JSProperties["cpError"] = "Información " + mensaje;
                e.Cancel = true;
                grid.CancelEdit();

            }



        }

        protected void gvsubProductos_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            Session["carga"] = 1;
            ASPxGridView grid = sender as ASPxGridView;
            DataTable metas = new DataTable();
            DataTable poas = new DataTable();
            DataTable programas = new DataTable();
            int fallofisico = 0;
            int fallosfinanciero = 0;
            String mensajesFisico = " ";
            String mensajesfinanciero = " ";
            double programa2020;
            double programa2021;
            double programa2022;
            double programa2023;
            double programa2024;
            double programa2025;
            double programa2026;
            double programa2027;
            double programa2028;
            double programa2029;
            double programa2030;
            double pre2020;
            double pre2021;
            double pre2022;
            double pre2023;
            double pre2024;
            double pre2025;
            double pre2026;
            double pre2027;
            double pre2028;
            double pre2029;
            double pre2030;

            if (Session["subproductos"] != null)
            {
                metas = (DataTable)Session["subproductos"];
                programas = (DataTable)Session["programapres"];

                if (metas.Rows.Count > 0)
                {
                    for (int i = 0; i < metas.Rows.Count; i++)
                    {
                        if (Convert.ToInt32(Session["periodo"]) == 20)
                        {
                            if (metas.Rows[i]["SPPSUB$ID_SUBPRODUCTO"].ToString() == e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString())
                            {
                                if (programas.Rows[0]["PRESUPUESTO2021"] == DBNull.Value)
                                    programa2021 = 0;
                                else
                                    programa2021 = Convert.ToDouble(programas.Rows[0]["PRESUPUESTO2021"]);

                                if (programas.Rows[0]["PRESUPUESTO2022"] == DBNull.Value)
                                    programa2022 = 0;
                                else
                                    programa2022 = Convert.ToDouble(programas.Rows[0]["PRESUPUESTO2022"]);

                                if (programas.Rows[0]["PRESUPUESTO2023"] == DBNull.Value)
                                    programa2023 = 0;
                                else
                                    programa2023 = Convert.ToDouble(programas.Rows[0]["PRESUPUESTO2023"]);

                                if (programas.Rows[0]["PRESUPUESTO2024"] == DBNull.Value)
                                    programa2024 = 0;
                                else
                                    programa2024 = Convert.ToDouble(programas.Rows[0]["PRESUPUESTO2024"]);

                                if (programas.Rows[0]["PRESUPUESTO2025"] == DBNull.Value)
                                    programa2025 = 0;
                                else
                                    programa2025 = Convert.ToDouble(programas.Rows[0]["PRESUPUESTO2025"]);

                                //inicio 2021
                                if (programas.Rows[0]["PR0GRAMADO2021"] == DBNull.Value)
                                    pre2021 = 0;
                                else
                                    pre2021 = Convert.ToDouble(programas.Rows[0]["PR0GRAMADO2021"]);

                                if ((programas.Rows[0]["PR0GRAMADO2022"] == DBNull.Value))
                                    pre2022 = 0;
                                else
                                    pre2022 = Convert.ToDouble(programas.Rows[0]["PR0GRAMADO2022"]);

                                if ((programas.Rows[0]["PR0GRAMADO2023"] == DBNull.Value))
                                    pre2023 = 0;
                                else
                                    pre2023 = Convert.ToDouble(programas.Rows[0]["PR0GRAMADO2023"]);

                                if ((programas.Rows[0]["PR0GRAMADO2024"] == DBNull.Value))
                                    pre2024 = 0;
                                else
                                    pre2024 = Convert.ToDouble(programas.Rows[0]["PR0GRAMADO2024"]);

                                if ((programas.Rows[0]["PR0GRAMADO2025"] == DBNull.Value))
                                    pre2025 = 0;
                                else
                                    pre2025 = Convert.ToDouble(programas.Rows[0]["PR0GRAMADO2025"]);
                                //fin financiero

                                // inicio 2021
                                if (programas.Rows[0]["PR0GRAMADO2021"] == DBNull.Value)
                                    pre2021 = 0;
                                else
                                    pre2021 = Convert.ToDouble(programas.Rows[0]["PR0GRAMADO2021"]);

                                if ((programas.Rows[0]["PR0GRAMADO2022"] == DBNull.Value))
                                    pre2022 = 0;
                                else
                                    pre2022 = Convert.ToDouble(programas.Rows[0]["PR0GRAMADO2022"]);

                                if ((programas.Rows[0]["PR0GRAMADO2023"] == DBNull.Value))
                                    pre2023 = 0;
                                else
                                    pre2023 = Convert.ToDouble(programas.Rows[0]["PR0GRAMADO2023"]);

                                if ((programas.Rows[0]["PR0GRAMADO2024"] == DBNull.Value))
                                    pre2024 = 0;
                                else
                                    pre2024 = Convert.ToDouble(programas.Rows[0]["PR0GRAMADO2024"]);

                                if ((programas.Rows[0]["PR0GRAMADO2025"] == DBNull.Value))
                                    pre2025 = 0;
                                else
                                    pre2025 = Convert.ToDouble(programas.Rows[0]["PR0GRAMADO2025"]);

                                poas = obtienepoa(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), 2021);
                                //inicio 2020
                                if (metas.Rows[i]["IDFISUB2021"] == DBNull.Value)
                                {
                                    //if (Convert.ToDouble(e.NewValues["MFSUB2021"]) != 0)
                                    if (e.NewValues["MFSUB2021"] != DBNull.Value)
                                    {
                                        estado = insertaFisicasub(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MFSUB2021"]));
                                        if (estado == 0)
                                        {
                                            mensajesFisico = mensajesFisico + " 2021 ";
                                            fallofisico++;
                                        }
                                    }


                                }

                                else
                                {
                                    //if (Convert.ToDouble(e.NewValues["MFSUB2021"]) != 0)
                                    if (e.NewValues["MFSUB2021"] != DBNull.Value)
                                        estado = Acualizametafisicasub(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MFSUB2021"]), Convert.ToInt32(metas.Rows[i]["IDFISUB2021"]));
                                    else
                                        estado = Eliminafisicasub(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToInt32(metas.Rows[i]["IDFISUB2021"]));

                                    if (estado == 0)
                                    {
                                        mensajesFisico = mensajesFisico + " 2021 ";
                                        fallofisico++;
                                    }
                                }

                                //fin 2021

                                poas = obtienepoa(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), 2022);
                                //inicio 2022
                                if (metas.Rows[i]["IDFISUB2022"] == DBNull.Value)
                                {
                                    //if (Convert.ToDouble(e.NewValues["MFSUB2022"]) != 0)
                                    if (e.NewValues["MFSUB2022"] != DBNull.Value)
                                    {
                                        estado = insertaFisicasub(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MFSUB2022"]));
                                        if (estado == 0)
                                        {
                                            mensajesFisico = mensajesFisico + " 2022 ";
                                            fallofisico++;
                                        }
                                    }

                                }

                                else
                                {
                                    //if (Convert.ToDouble(e.NewValues["MFSUB2022"]) != 0)
                                    if (e.NewValues["MFSUB2022"] != DBNull.Value)

                                        estado = Acualizametafisicasub(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MFSUB2022"]), Convert.ToInt32(metas.Rows[i]["IDFISUB2022"]));
                                    else
                                        estado = Eliminafisicasub(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToInt32(metas.Rows[i]["IDFISUB2022"]));


                                    if (estado == 0)
                                    {
                                        mensajesFisico = mensajesFisico + " 2022 ";
                                        fallofisico++;
                                    }
                                }

                                //fin 2022

                                poas = obtienepoa(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), 2023);
                                //inicio 2022
                                if (metas.Rows[i]["IDFISUB2023"] == DBNull.Value)
                                {
                                    //if (Convert.ToDouble(e.NewValues["MFSUB2023"]) != 0)
                                    if (e.NewValues["MFSUB2023"] != DBNull.Value)
                                    {
                                        estado = insertaFisicasub(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MFSUB2023"]));
                                        if (estado == 0)
                                        {
                                            mensajesFisico = mensajesFisico + " 2023 ";
                                            fallofisico++;
                                        }
                                    }

                                }

                                else
                                {
                                    //if (Convert.ToDouble(e.NewValues["MFSUB2023"]) != 0)
                                    if (e.NewValues["MFSUB2023"] != DBNull.Value)

                                        estado = Acualizametafisicasub(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MFSUB2023"]), Convert.ToInt32(metas.Rows[i]["IDFISUB2023"]));
                                    else
                                        estado = Eliminafisicasub(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToInt32(metas.Rows[i]["IDFISUB2023"]));

                                    if (estado == 0)
                                    {
                                        mensajesFisico = mensajesFisico + " 2023 ";
                                        fallofisico++;
                                    }
                                }

                                //fin 2023

                                poas = obtienepoa(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), 2024);
                                //inicio 2024
                                if (metas.Rows[i]["IDFISUB2024"] == DBNull.Value)
                                {
                                    //if (Convert.ToDouble(e.NewValues["MFSUB2024"]) != 0)
                                    if (e.NewValues["MFSUB2024"] != DBNull.Value)
                                    {
                                        estado = insertaFisicasub(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MFSUB2024"]));
                                        if (estado == 0)
                                        {
                                            mensajesFisico = mensajesFisico + " 2024 ";
                                            fallofisico++;
                                        }
                                    }

                                }

                                else
                                {
                                    //if (Convert.ToDouble(e.NewValues["MFSUB2024"]) != 0)
                                    if (e.NewValues["MFSUB2024"] != DBNull.Value)

                                        estado = Acualizametafisicasub(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MFSUB2024"]), Convert.ToInt32(metas.Rows[i]["IDFISUB2024"]));
                                    else
                                        estado = Eliminafisicasub(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToInt32(metas.Rows[i]["IDFISUB2024"]));

                                    if (estado == 0)
                                    {
                                        mensajesFisico = mensajesFisico + " 2024 ";
                                        fallofisico++;
                                    }
                                }

                                //fin 2024

                                poas = obtienepoa(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), 2025);
                                //inicio 2025
                                if (metas.Rows[i]["IDFISUB2025"] == DBNull.Value)
                                {
                                    //if (Convert.ToDouble(e.NewValues["MFSUB2025"]) != 0)
                                    if (e.NewValues["MFSUB2025"] != DBNull.Value)
                                    {
                                        estado = insertaFisicasub(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MFSUB2025"]));
                                        if (estado == 0)
                                        {
                                            mensajesFisico = mensajesFisico + " 2025 ";
                                            fallofisico++;
                                        }
                                    }

                                }

                                else
                                {
                                    //if (Convert.ToDouble(e.NewValues["MFSUB2025"]) != 0)
                                    if (e.NewValues["MFSUB2025"] != DBNull.Value)
                                        estado = Acualizametafisicasub(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MFSUB2025"]), Convert.ToInt32(metas.Rows[i]["IDFISUB2025"]));
                                    else
                                        estado = Eliminafisicasub(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToInt32(metas.Rows[i]["IDFISUB2025"]));

                                    if (estado == 0)
                                    {
                                        mensaje = mensaje + " 2025 ";
                                        fallofisico++;
                                    }
                                }

                                //fin 2025
                                //inicio 2021

                                if (metas.Rows[i]["MFINSUB2021"] == DBNull.Value || e.NewValues["MFINSUB2021"] == DBNull.Value)
                                    pre2021 = 0;
                                else if ((metas.Rows[i]["MFINSUB2021"] != DBNull.Value && e.NewValues["MFINSUB2021"] != DBNull.Value) && (Convert.ToDouble(metas.Rows[i]["MFINSUB2021"]) == Convert.ToDouble(e.NewValues["MFINSUB2021"])))
                                    pre2021 = 0;
                                else if (Convert.ToDouble(metas.Rows[i]["MFINSUB2021"]) > Convert.ToDouble(e.NewValues["MFINSUB2021"]))
                                {
                                    pre2021 = pre2021 + Convert.ToDouble(e.NewValues["MFINSUB2021"]);
                                    pre2021 = Convert.ToDouble(e.NewValues["MFINSUB2021"]) - Convert.ToDouble(metas.Rows[i]["MFINSUB2021"]);
                                    pre2021 = pre2021 + Convert.ToDouble(metas.Rows[i]["MFINSUB2021"]);
                                }

                                else
                                {
                                    pre2021 = Convert.ToDouble(e.NewValues["MFINSUB2021"]) - Convert.ToDouble(metas.Rows[i]["MFINSUB2021"]);
                                    pre2021 = pre2021 + Convert.ToDouble(metas.Rows[i]["MFINSUB2021"]);
                                }

                                //if (pre2021 > programa2021)
                                //{
                                //    mensajesfinanciero = mensajesfinanciero + " 2021 ";
                                //    //fallosfinanciero++;
                                //}
                                //else
                                //{
                                poas = obtienepoa(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), 2021);
                                if (metas.Rows[i]["IDFINSUB2021"] == DBNull.Value)
                                {
                                    //if (Convert.ToDouble(e.NewValues["MFINSUB2021"]) != 0)
                                    if (e.NewValues["MFINSUB2021"] != DBNull.Value)
                                        estado = insertaFinanciera(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MFINSUB2021"]));
                                }
                                else
                                {
                                    //if (Convert.ToDouble(e.NewValues["MFINSUB2021"]) != 0)
                                    if (e.NewValues["MFINSUB2021"] != DBNull.Value)
                                        estado = Acualizametafinanciera(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MFINSUB2021"]), Convert.ToInt32(metas.Rows[i]["IDFINSUB2021"]));
                                    else
                                        estado = Eliminametafinanciera(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToInt32(metas.Rows[i]["IDFINSUB2021"]));
                                }
                                //}
                                //fin 2021

                                //inicio 2022
                                if (metas.Rows[i]["MFINSUB2022"] == DBNull.Value || e.NewValues["MFINSUB2022"] == DBNull.Value)
                                    pre2022 = 0;
                                else if ((metas.Rows[i]["MFINSUB2022"] != DBNull.Value && e.NewValues["MFINSUB2022"] != DBNull.Value) && (Convert.ToDouble(metas.Rows[i]["MFINSUB2022"]) == Convert.ToDouble(e.NewValues["MFINSUB2022"])))
                                    pre2022 = 0;
                                else if (Convert.ToDouble(metas.Rows[i]["MFINSUB2022"]) > Convert.ToDouble(e.NewValues["MFINSUB2022"]))
                                {
                                    pre2022 = pre2022 + Convert.ToDouble(e.NewValues["MFINSUB2022"]);
                                    pre2022 = Convert.ToDouble(e.NewValues["MFINSUB2022"]) - Convert.ToDouble(metas.Rows[i]["MFINSUB2022"]);
                                    pre2022 = pre2022 + Convert.ToDouble(metas.Rows[i]["MFINSUB2022"]);
                                }

                                else
                                {
                                    pre2022 = Convert.ToDouble(e.NewValues["MFINSUB2022"]) - Convert.ToDouble(metas.Rows[i]["MFINSUB2022"]);
                                    pre2022 = pre2022 + Convert.ToDouble(metas.Rows[i]["MFINSUB2022"]);
                                }

                                //pre2021 = pre2021 + Convert.ToDouble(e.NewValues["FIN2021"]);
                                //if (pre2021 > programa2021)
                                //{
                                //    mensajesfinanciero = mensajesfinanciero + " 2021 ";
                                //    fallosfinanciero++;
                                //}
                                //else
                                //{
                                poas = obtienepoa(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), 2022);
                                if (metas.Rows[i]["IDFINSUB2022"] == DBNull.Value)
                                {
                                    //if (Convert.ToDouble(e.NewValues["MFINSUB2022"]) != 0)
                                    if (e.NewValues["MFINSUB2022"] != DBNull.Value)
                                        estado = insertaFinanciera(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MFINSUB2022"]));
                                }
                                else
                                {
                                    //if (Convert.ToDouble(e.NewValues["MFINSUB2022"]) != 0)
                                    if (e.NewValues["MFINSUB2022"] != DBNull.Value)
                                        estado = estado = Acualizametafinanciera(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MFINSUB2022"]), Convert.ToInt32(metas.Rows[i]["IDFINSUB2022"]));
                                    else
                                        estado = Eliminametafinanciera(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToInt32(metas.Rows[i]["IDFINSUB2022"]));
                                }
                                //}
                                //fin 2022


                                //inicio 2023
                                if (metas.Rows[i]["MFSUB2023"] == DBNull.Value || e.NewValues["MFSUB20223"] == DBNull.Value)
                                    pre2023 = 0;
                                else if ((metas.Rows[i]["MFSUB2023"] != DBNull.Value && e.NewValues["MFSUB2023"] != DBNull.Value) && (Convert.ToDouble(metas.Rows[i]["MFSUB2023"]) == Convert.ToDouble(e.NewValues["MFSUB2023"])))
                                    pre2023 = 0;
                                else if (Convert.ToDouble(metas.Rows[i]["MFSUB2023"]) > Convert.ToDouble(e.NewValues["MFSUB2023"]))
                                {
                                    pre2023 = pre2023 + Convert.ToDouble(e.NewValues["MFSUB2023"]);
                                    pre2023 = Convert.ToDouble(e.NewValues["MFSUB2023"]) - Convert.ToDouble(metas.Rows[i]["MFSUB2023"]);
                                    pre2023 = pre2023 + Convert.ToDouble(metas.Rows[i]["MFSUB2023"]);
                                }

                                else
                                {
                                    pre2023 = Convert.ToDouble(e.NewValues["MFSUB2023"]) - Convert.ToDouble(metas.Rows[i]["MFSUB2023"]);
                                    pre2023 = pre2022 + Convert.ToDouble(metas.Rows[i]["MFSUB2023"]);
                                }
                                //pre2022 = pre2022 + Convert.ToDouble(e.NewValues["FIN2022"]);
                                //if (pre2023 > programa2023)
                                //{
                                //    mensajesfinanciero = mensajesfinanciero + " 2023 ";
                                //    fallosfinanciero++;

                                //}
                                //else
                                //{
                                poas = obtienepoa(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), 2023);
                                if (metas.Rows[i]["IDFINSUB2023"] == DBNull.Value)
                                {
                                    //if (Convert.ToDouble(e.NewValues["MFINSUB2023"]) != 0)
                                    if (e.NewValues["MFINSUB2023"] != DBNull.Value)
                                        estado = insertaFinanciera(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MFINSUB2023"]));
                                }
                                else
                                {
                                    //if (Convert.ToDouble(e.NewValues["MFINSUB2023"]) != 0)
                                    if (e.NewValues["MFINSUB2023"] != DBNull.Value)
                                        estado = Acualizametafinanciera(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MFINSUB2023"]), Convert.ToInt32(metas.Rows[i]["IDFINSUB2023"]));
                                    else
                                        estado = Eliminametafinanciera(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToInt32(metas.Rows[i]["IDFINSUB2023"]));
                                }
                                //}
                                //fin 2023

                                //inicio 2024
                                if (metas.Rows[i]["MFINSUB2024"] == DBNull.Value || e.NewValues["MFINSUB2024"] == DBNull.Value)
                                    pre2024 = 0;
                                else if ((metas.Rows[i]["MFINSUB2024"] != DBNull.Value && e.NewValues["MFINSUB2024"] != DBNull.Value) && (Convert.ToDouble(metas.Rows[i]["MFINSUB2024"]) == Convert.ToDouble(e.NewValues["MFINSUB2024"])))
                                    pre2024 = 0;
                                else if (Convert.ToDouble(metas.Rows[i]["MFINSUB2024"]) > Convert.ToDouble(e.NewValues["MFINSUB2024"]))
                                {
                                    pre2024 = pre2024 + Convert.ToDouble(e.NewValues["MFINSUB2024"]);
                                    pre2024 = Convert.ToDouble(e.NewValues["MFINSUB2024"]) - Convert.ToDouble(metas.Rows[i]["MFINSUB2024"]);
                                    pre2024 = pre2024 + Convert.ToDouble(metas.Rows[i]["MFINSUB2024"]);
                                }

                                else
                                {
                                    pre2024 = Convert.ToDouble(e.NewValues["MFINSUB2024"]) - Convert.ToDouble(metas.Rows[i]["MFINSUB2024"]);
                                    pre2024 = pre2024 + Convert.ToDouble(metas.Rows[i]["MFINSUB2024"]);
                                }
                                //pre2023 = pre2023 + Convert.ToDouble(e.NewValues["FIN2023"]);
                                //if (pre2023 > programa2023)
                                //{
                                //    mensajesfinanciero = mensajesfinanciero + " 2023 ";
                                //    fallosfinanciero++;

                                //}
                                //else
                                //{
                                poas = obtienepoa(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), 2024);
                                if (metas.Rows[i]["IDFINSUB2024"] == DBNull.Value)
                                {
                                    //if (Convert.ToDouble(e.NewValues["MFINSUB2024"]) != 0)
                                    if (e.NewValues["MFINSUB2024"] != DBNull.Value)
                                        estado = insertaFinanciera(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MFINSUB2024"]));
                                }
                                else
                                {
                                    //if (Convert.ToDouble(e.NewValues["MFINSUB2024"]) != 0)
                                    if (e.NewValues["MFINSUB2024"] != DBNull.Value)
                                        estado = Acualizametafinanciera(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MFINSUB2024"]), Convert.ToInt32(metas.Rows[i]["IDFINSUB2024"]));
                                    else
                                        estado = Eliminametafinanciera(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToInt32(metas.Rows[i]["IDFINSUB2024"]));
                                }
                                //}
                                //fin 2024

                                //inicio 2025
                                if (metas.Rows[i]["MFINSUB2025"] == DBNull.Value || e.NewValues["MFINSUB2025"] == DBNull.Value)
                                    pre2025 = 0;
                                else if ((metas.Rows[i]["MFINSUB2025"] != DBNull.Value && e.NewValues["MFINSUB2025"] != DBNull.Value) && (Convert.ToDouble(metas.Rows[i]["MFINSUB2025"]) == Convert.ToDouble(e.NewValues["MFINSUB2025"])))
                                    pre2025 = 0;
                                else if (Convert.ToDouble(metas.Rows[i]["MFINSUB2025"]) > Convert.ToDouble(e.NewValues["MFINSUB2025"]))
                                {
                                    pre2025 = pre2025 + Convert.ToDouble(e.NewValues["MFINSUB2025"]);
                                    pre2025 = Convert.ToDouble(e.NewValues["MFINSUB2025"]) - Convert.ToDouble(metas.Rows[i]["MFINSUB2025"]);
                                    pre2025 = pre2025 + Convert.ToDouble(metas.Rows[i]["MFINSUB2025"]);
                                }

                                else
                                {
                                    pre2025 = Convert.ToDouble(e.NewValues["MFINSUB2025"]) - Convert.ToDouble(metas.Rows[i]["MFINSUB2025"]);
                                    pre2025 = pre2025 + Convert.ToDouble(metas.Rows[i]["MFINSUB2025"]);
                                }
                                //pre2024 = pre2024 + Convert.ToDouble(e.NewValues["FIN2024"]);
                                //if (pre2024 > programa2024)
                                //{
                                //    mensajesfinanciero = mensajesfinanciero + " 2024 ";
                                //    fallosfinanciero++;

                                //}
                                //else
                                //{
                                poas = obtienepoa(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), 2025);
                                if (metas.Rows[i]["IDFINSUB2025"] == DBNull.Value)
                                {
                                    //if (Convert.ToDouble(e.NewValues["MFINSUB2025"]) != 0)
                                    if (e.NewValues["MFINSUB2025"] != DBNull.Value)
                                        estado = insertaFinanciera(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MFINSUB2025"]));
                                }
                                else
                                {
                                    //if (Convert.ToDouble(e.NewValues["MFINSUB2025"]) != 0)
                                    if (e.NewValues["MFINSUB2025"] != DBNull.Value)
                                        estado = Acualizametafinanciera(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MFINSUB2025"]), Convert.ToInt32(metas.Rows[i]["IDFINSUB2025"]));
                                    else
                                        estado = Eliminametafinanciera(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToInt32(metas.Rows[i]["IDFINSUB2025"]));
                                }
                                //}

                                if (fallofisico > 0)
                                    mensaje = "Las metas de los años " + mensajesFisico + ", no pudieron ser guardadas por posibles fallos en el sistema, intententelo mas tarde o contacte al administrador";
                                if (fallosfinanciero > 0)
                                    mensaje = mensaje + " Las metas financieras de los años " + mensajesfinanciero + ", no pudieron ser guardadas al estar programando una cantidad mayor, a la cantidad asignada  a su programa, por favor revise las cantidades en los años descritos";
                                if (fallofisico == 0 & fallosfinanciero == 0)
                                    mensaje = "Metas guardadas correctamente";

                                grid.JSProperties["cpError"] = "Información " + mensaje;
                                e.Cancel = true;
                                grid.CancelEdit();
                                break;


                            }

                        }



                        //periodo 21

                        if (Convert.ToInt32(Session["periodo"]) == 21)
                        {
                            if (metas.Rows[i]["SPPSUB$ID_SUBPRODUCTO"].ToString() == e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString())
                            {
                                if (programas.Rows[0]["PRESUPUESTO2022"] == DBNull.Value)
                                    programa2022 = 0;
                                else
                                    programa2022 = Convert.ToDouble(programas.Rows[0]["PRESUPUESTO2022"]);


                                if (programas.Rows[0]["PRESUPUESTO2023"] == DBNull.Value)
                                    programa2023 = 0;
                                else
                                    programa2023 = Convert.ToDouble(programas.Rows[0]["PRESUPUESTO2023"]);



                                if (programas.Rows[0]["PRESUPUESTO2024"] == DBNull.Value)
                                    programa2024 = 0;
                                else
                                    programa2024 = Convert.ToDouble(programas.Rows[0]["PRESUPUESTO2024"]);



                                if (programas.Rows[0]["PRESUPUESTO2025"] == DBNull.Value)
                                    programa2025 = 0;
                                else
                                    programa2025 = Convert.ToDouble(programas.Rows[0]["PRESUPUESTO2025"]);



                                if (programas.Rows[0]["PRESUPUESTO2026"] == DBNull.Value)
                                    programa2026 = 0;
                                else
                                    programa2026 = Convert.ToDouble(programas.Rows[0]["PRESUPUESTO2026"]);




                                //inicio 2022
                                if (programas.Rows[0]["PR0GRAMADO2022"] == DBNull.Value)
                                    pre2022 = 0;
                                else
                                    pre2022 = Convert.ToDouble(programas.Rows[0]["PR0GRAMADO2022"]);

                                if ((programas.Rows[0]["PR0GRAMADO2023"] == DBNull.Value))
                                    pre2023 = 0;
                                else
                                    pre2023 = Convert.ToDouble(programas.Rows[0]["PR0GRAMADO2023"]);

                                if ((programas.Rows[0]["PR0GRAMADO2024"] == DBNull.Value))
                                    pre2024 = 0;
                                else
                                    pre2024 = Convert.ToDouble(programas.Rows[0]["PR0GRAMADO2024"]);

                                if ((programas.Rows[0]["PR0GRAMADO2025"] == DBNull.Value))
                                    pre2025 = 0;
                                else
                                    pre2025 = Convert.ToDouble(programas.Rows[0]["PR0GRAMADO2025"]);

                                if ((programas.Rows[0]["PR0GRAMADO2026"] == DBNull.Value))
                                    pre2026 = 0;
                                else
                                    pre2026 = Convert.ToDouble(programas.Rows[0]["PR0GRAMADO2026"]);
                                //fin financiero




                                // inicio 2022
                                if (programas.Rows[0]["PR0GRAMADO2022"] == DBNull.Value)
                                    pre2022 = 0;
                                else
                                    pre2022 = Convert.ToDouble(programas.Rows[0]["PR0GRAMADO2022"]);

                                if ((programas.Rows[0]["PR0GRAMADO2023"] == DBNull.Value))
                                    pre2023 = 0;
                                else
                                    pre2023 = Convert.ToDouble(programas.Rows[0]["PR0GRAMADO2023"]);

                                if ((programas.Rows[0]["PR0GRAMADO2024"] == DBNull.Value))
                                    pre2024 = 0;
                                else
                                    pre2024 = Convert.ToDouble(programas.Rows[0]["PR0GRAMADO2024"]);

                                if ((programas.Rows[0]["PR0GRAMADO2025"] == DBNull.Value))
                                    pre2025 = 0;
                                else
                                    pre2025 = Convert.ToDouble(programas.Rows[0]["PR0GRAMADO2025"]);

                                if ((programas.Rows[0]["PR0GRAMADO2026"] == DBNull.Value))
                                    pre2026 = 0;
                                else
                                    pre2026 = Convert.ToDouble(programas.Rows[0]["PR0GRAMADO2026"]);




                                poas = obtienepoa(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), 2022);
                                //inicio 2022
                                if (metas.Rows[i]["IDFISUB2022"] == DBNull.Value)
                                {
                                    //if (Convert.ToDouble(e.NewValues["MFSUB2022"]) != 0)
                                    if (e.NewValues["MFSUB2022"] != DBNull.Value)
                                    {
                                        estado = insertaFisicasub(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MFSUB2022"]));
                                        if (estado == 0)
                                        {
                                            mensajesFisico = mensajesFisico + " 2022 ";
                                            fallofisico++;
                                        }
                                    }


                                }

                                else
                                {
                                    //if (Convert.ToDouble(e.NewValues["MFSUB2022"]) != 0)
                                    if (e.NewValues["MFSUB2022"] != DBNull.Value)

                                        estado = Acualizametafisicasub(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MFSUB2022"]), Convert.ToInt32(metas.Rows[i]["IDFISUB2022"]));
                                    else
                                        estado = Eliminafisicasub(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToInt32(metas.Rows[i]["IDFISUB2022"]));

                                    if (estado == 0)
                                    {
                                        mensajesFisico = mensajesFisico + " 2022 ";
                                        fallofisico++;
                                    }
                                }

                                //fin 2022

                                poas = obtienepoa(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), 2023);
                                //inicio 2023
                                if (metas.Rows[i]["IDFISUB2023"] == DBNull.Value)
                                {
                                    //if (Convert.ToDouble(e.NewValues["MFSUB2023"]) != 0)
                                    if (e.NewValues["MFSUB2023"] != DBNull.Value)
                                    {
                                        estado = insertaFisicasub(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MFSUB2023"]));
                                        if (estado == 0)
                                        {
                                            mensajesFisico = mensajesFisico + " 2023 ";
                                            fallofisico++;
                                        }
                                    }

                                }

                                else
                                {
                                    //if (Convert.ToDouble(e.NewValues["MFSUB2023"]) != 0)
                                    if (e.NewValues["MFSUB2023"] != DBNull.Value)
                                        estado = Acualizametafisicasub(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MFSUB2023"]), Convert.ToInt32(metas.Rows[i]["IDFISUB2023"]));
                                    else
                                        estado = Eliminafisicasub(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToInt32(metas.Rows[i]["IDFISUB2023"]));


                                    if (estado == 0)
                                    {
                                        mensajesFisico = mensajesFisico + " 2023 ";
                                        fallofisico++;
                                    }
                                }

                                //fin 2023

                                poas = obtienepoa(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), 2024);
                                //inicio 2024
                                if (metas.Rows[i]["IDFISUB2024"] == DBNull.Value)
                                {
                                    //if (Convert.ToDouble(e.NewValues["MFSUB2024"]) != 0)
                                    if (e.NewValues["MFSUB2024"] != DBNull.Value)
                                    {
                                        estado = insertaFisicasub(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MFSUB2024"]));
                                        if (estado == 0)
                                        {
                                            mensajesFisico = mensajesFisico + " 2024 ";
                                            fallofisico++;
                                        }
                                    }

                                }

                                else
                                {
                                    //if (Convert.ToDouble(e.NewValues["MFSUB2024"]) != 0)
                                    if (e.NewValues["MFSUB2024"] != DBNull.Value)
                                        estado = Acualizametafisicasub(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MFSUB2024"]), Convert.ToInt32(metas.Rows[i]["IDFISUB2024"]));
                                    else
                                        estado = Eliminafisicasub(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToInt32(metas.Rows[i]["IDFISUB2024"]));

                                    if (estado == 0)
                                    {
                                        mensajesFisico = mensajesFisico + " 2024 ";
                                        fallofisico++;
                                    }
                                }

                                //fin 2024

                                poas = obtienepoa(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), 2025);
                                //inicio 2025
                                if (metas.Rows[i]["IDFISUB2025"] == DBNull.Value)
                                {
                                    //if (Convert.ToDouble(e.NewValues["MFSUB2025"]) != 0)
                                    if (e.NewValues["MFSUB2025"] != DBNull.Value)
                                    {
                                        estado = insertaFisicasub(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MFSUB2025"]));
                                        if (estado == 0)
                                        {
                                            mensajesFisico = mensajesFisico + " 2025 ";
                                            fallofisico++;
                                        }
                                    }

                                }

                                else
                                {
                                    //if (Convert.ToDouble(e.NewValues["MFSUB2025"]) != 0)
                                    if (e.NewValues["MFSUB2025"] != DBNull.Value)

                                        estado = Acualizametafisicasub(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MFSUB2025"]), Convert.ToInt32(metas.Rows[i]["IDFISUB2025"]));
                                    else
                                        estado = Eliminafisicasub(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToInt32(metas.Rows[i]["IDFISUB2025"]));

                                    if (estado == 0)
                                    {
                                        mensajesFisico = mensajesFisico + " 2025 ";
                                        fallofisico++;
                                    }
                                }

                                //fin 2025

                                poas = obtienepoa(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), 2026);
                                //inicio 2025
                                if (metas.Rows[i]["IDFISUB2026"] == DBNull.Value)
                                {
                                    //if (Convert.ToDouble(e.NewValues["MFSUB2026"]) != 0)
                                    if (e.NewValues["MFSUB2026"] != DBNull.Value)
                                    {
                                        estado = insertaFisicasub(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MFSUB2026"]));
                                        if (estado == 0)
                                        {
                                            mensajesFisico = mensajesFisico + " 2026 ";
                                            fallofisico++;
                                        }
                                    }

                                }

                                else
                                {
                                    //if (Convert.ToDouble(e.NewValues["MFSUB2026"]) != 0)
                                    if (e.NewValues["MFSUB2026"] != DBNull.Value)


                                        estado = Acualizametafisicasub(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MFSUB2026"]), Convert.ToInt32(metas.Rows[i]["IDFISUB2026"]));
                                    else
                                        estado = Eliminafisicasub(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToInt32(metas.Rows[i]["IDFISUB2026"]));

                                    if (estado == 0)
                                    {
                                        mensaje = mensaje + " 2026 ";
                                        fallofisico++;
                                    }
                                }

                                //fin 2026
                                //inicio 2022

                                if (metas.Rows[i]["MFINSUB2022"] == DBNull.Value || e.NewValues["MFINSUB2022"] == DBNull.Value)
                                    pre2022 = 0;
                                else if ((metas.Rows[i]["MFINSUB2022"] != DBNull.Value && e.NewValues["MFINSUB2022"] != DBNull.Value) && (Convert.ToDouble(metas.Rows[i]["MFINSUB2022"]) == Convert.ToDouble(e.NewValues["MFINSUB2022"])))
                                    pre2022 = 0;
                                else if (Convert.ToDouble(metas.Rows[i]["MFINSUB2022"]) > Convert.ToDouble(e.NewValues["MFINSUB2022"]))
                                {
                                    pre2022 = pre2022 + Convert.ToDouble(e.NewValues["MFINSUB2022"]);
                                    pre2022 = Convert.ToDouble(e.NewValues["MFINSUB2022"]) - Convert.ToDouble(metas.Rows[i]["MFINSUB2022"]);
                                    pre2022 = pre2022 + Convert.ToDouble(metas.Rows[i]["MFINSUB2022"]);
                                }

                                else
                                {
                                    pre2022 = Convert.ToDouble(e.NewValues["MFINSUB2022"]) - Convert.ToDouble(metas.Rows[i]["MFINSUB2022"]);
                                    pre2022 = pre2022 + Convert.ToDouble(metas.Rows[i]["MFINSUB2022"]);
                                }

                                //if (pre2021 > programa2021)
                                //{
                                //    mensajesfinanciero = mensajesfinanciero + " 2021 ";
                                //    //fallosfinanciero++;
                                //}
                                //else
                                //{
                                poas = obtienepoa(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), 2022);
                                if (metas.Rows[i]["IDFINSUB2022"] == DBNull.Value)
                                {
                                    //if (Convert.ToDouble(e.NewValues["MFINSUB2022"]) != 0)
                                    if (e.NewValues["MFINSUB2022"] != DBNull.Value)
                                        estado = insertaFinanciera(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MFINSUB2022"]));
                                }
                                else
                                {
                                    //if (Convert.ToDouble(e.NewValues["MFINSUB2022"]) != 0)
                                    if (e.NewValues["MFINSUB2022"] != DBNull.Value)
                                        estado = Acualizametafinanciera(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MFINSUB2022"]), Convert.ToInt32(metas.Rows[i]["IDFINSUB2022"]));
                                    else
                                        estado = Eliminametafinanciera(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToInt32(metas.Rows[i]["IDFINSUB2022"]));
                                }
                                //}
                                //fin 2022

                                //inicio 2023
                                if (metas.Rows[i]["MFINSUB2023"] == DBNull.Value || e.NewValues["MFINSUB2023"] == DBNull.Value)
                                    pre2023 = 0;
                                else if ((metas.Rows[i]["MFINSUB2023"] != DBNull.Value && e.NewValues["MFINSUB2023"] != DBNull.Value) && (Convert.ToDouble(metas.Rows[i]["MFINSUB2023"]) == Convert.ToDouble(e.NewValues["MFINSUB2023"])))
                                    pre2023 = 0;
                                else if (Convert.ToDouble(metas.Rows[i]["MFINSUB2023"]) > Convert.ToDouble(e.NewValues["MFINSUB2023"]))
                                {
                                    pre2023 = pre2023 + Convert.ToDouble(e.NewValues["MFINSUB2023"]);
                                    pre2023 = Convert.ToDouble(e.NewValues["MFINSUB2023"]) - Convert.ToDouble(metas.Rows[i]["MFINSUB2023"]);
                                    pre2023 = pre2023 + Convert.ToDouble(metas.Rows[i]["MFINSUB2023"]);
                                }

                                else
                                {
                                    pre2023 = Convert.ToDouble(e.NewValues["MFINSUB2023"]) - Convert.ToDouble(metas.Rows[i]["MFINSUB2023"]);
                                    pre2023 = pre2023 + Convert.ToDouble(metas.Rows[i]["MFINSUB2023"]);
                                }

                                //pre2021 = pre2021 + Convert.ToDouble(e.NewValues["FIN2021"]);
                                //if (pre2021 > programa2021)
                                //{
                                //    mensajesfinanciero = mensajesfinanciero + " 2021 ";
                                //    fallosfinanciero++;
                                //}
                                //else
                                //{
                                poas = obtienepoa(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), 2023);
                                if (metas.Rows[i]["IDFINSUB2023"] == DBNull.Value)
                                {
                                    //if (Convert.ToDouble(e.NewValues["MFINSUB2023"]) != 0)
                                    if (e.NewValues["MFINSUB2023"] != DBNull.Value)
                                        estado = insertaFinanciera(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MFINSUB2023"]));
                                }
                                else
                                {
                                    //if (Convert.ToDouble(e.NewValues["MFINSUB2023"]) != 0)
                                    if (e.NewValues["MFINSUB2023"] != DBNull.Value)
                                        estado = estado = Acualizametafinanciera(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MFINSUB2023"]), Convert.ToInt32(metas.Rows[i]["IDFINSUB2023"]));
                                    else
                                        estado = Eliminametafinanciera(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToInt32(metas.Rows[i]["IDFINSUB2023"]));
                                }
                                //}
                                //fin 2023


                                //inicio 2024
                                if (metas.Rows[i]["MFSUB2024"] == DBNull.Value || e.NewValues["MFSUB20224"] == DBNull.Value)
                                    pre2024 = 0;
                                else if ((metas.Rows[i]["MFSUB2024"] != DBNull.Value && e.NewValues["MFSUB2024"] != DBNull.Value) && (Convert.ToDouble(metas.Rows[i]["MFSUB2024"]) == Convert.ToDouble(e.NewValues["MFSUB2024"])))
                                    pre2024 = 0;
                                else if (Convert.ToDouble(metas.Rows[i]["MFSUB2024"]) > Convert.ToDouble(e.NewValues["MFSUB2024"]))
                                {
                                    pre2024 = pre2024 + Convert.ToDouble(e.NewValues["MFSUB2024"]);
                                    pre2024 = Convert.ToDouble(e.NewValues["MFSUB2024"]) - Convert.ToDouble(metas.Rows[i]["MFSUB2024"]);
                                    pre2024 = pre2024 + Convert.ToDouble(metas.Rows[i]["MFSUB2024"]);
                                }

                                else
                                {
                                    pre2024 = Convert.ToDouble(e.NewValues["MFSUB2024"]) - Convert.ToDouble(metas.Rows[i]["MFSUB2024"]);
                                    pre2024 = pre2024 + Convert.ToDouble(metas.Rows[i]["MFSUB2024"]);
                                }
                                //pre2022 = pre2022 + Convert.ToDouble(e.NewValues["FIN2022"]);
                                //if (pre2023 > programa2023)
                                //{
                                //    mensajesfinanciero = mensajesfinanciero + " 2023 ";
                                //    fallosfinanciero++;

                                //}
                                //else
                                //{
                                poas = obtienepoa(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), 2024);
                                if (metas.Rows[i]["IDFINSUB2024"] == DBNull.Value)
                                {
                                    //if (Convert.ToDouble(e.NewValues["MFINSUB2024"]) != 0)
                                    if (e.NewValues["MFINSUB2024"] != DBNull.Value)
                                        estado = insertaFinanciera(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MFINSUB2024"]));
                                }
                                else
                                {
                                    //if (Convert.ToDouble(e.NewValues["MFINSUB2024"]) != 0)
                                    if (e.NewValues["MFINSUB2024"] != DBNull.Value)
                                        estado = Acualizametafinanciera(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MFINSUB2024"]), Convert.ToInt32(metas.Rows[i]["IDFINSUB2024"]));
                                    else
                                        estado = Eliminametafinanciera(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToInt32(metas.Rows[i]["IDFINSUB2024"]));
                                }
                                //}
                                //fin 2024

                                //inicio 2025
                                if (metas.Rows[i]["MFINSUB2025"] == DBNull.Value || e.NewValues["MFINSUB2025"] == DBNull.Value)
                                    pre2025 = 0;
                                else if ((metas.Rows[i]["MFINSUB2025"] != DBNull.Value && e.NewValues["MFINSUB2025"] != DBNull.Value) && (Convert.ToDouble(metas.Rows[i]["MFINSUB2025"]) == Convert.ToDouble(e.NewValues["MFINSUB2025"])))
                                    pre2025 = 0;
                                else if (Convert.ToDouble(metas.Rows[i]["MFINSUB2025"]) > Convert.ToDouble(e.NewValues["MFINSUB2025"]))
                                {
                                    pre2025 = pre2025 + Convert.ToDouble(e.NewValues["MFINSUB2025"]);
                                    pre2025 = Convert.ToDouble(e.NewValues["MFINSUB2025"]) - Convert.ToDouble(metas.Rows[i]["MFINSUB2025"]);
                                    pre2025 = pre2025 + Convert.ToDouble(metas.Rows[i]["MFINSUB2025"]);
                                }

                                else
                                {
                                    pre2025 = Convert.ToDouble(e.NewValues["MFINSUB2025"]) - Convert.ToDouble(metas.Rows[i]["MFINSUB2025"]);
                                    pre2025 = pre2025 + Convert.ToDouble(metas.Rows[i]["MFINSUB2025"]);
                                }
                                //pre2023 = pre2023 + Convert.ToDouble(e.NewValues["FIN2023"]);
                                //if (pre2023 > programa2023)
                                //{
                                //    mensajesfinanciero = mensajesfinanciero + " 2023 ";
                                //    fallosfinanciero++;

                                //}
                                //else
                                //{
                                poas = obtienepoa(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), 2025);
                                if (metas.Rows[i]["IDFINSUB2025"] == DBNull.Value)
                                {
                                    //if (Convert.ToDouble(e.NewValues["MFINSUB2025"]) != 0)
                                    if (e.NewValues["MFINSUB2025"] != DBNull.Value)
                                        estado = insertaFinanciera(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MFINSUB2025"]));
                                }
                                else
                                {
                                    //if (Convert.ToDouble(e.NewValues["MFINSUB2025"]) != 0)
                                    if (e.NewValues["MFINSUB2025"] != DBNull.Value)
                                        estado = Acualizametafinanciera(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MFINSUB2025"]), Convert.ToInt32(metas.Rows[i]["IDFINSUB2025"]));
                                    else
                                        estado = Eliminametafinanciera(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToInt32(metas.Rows[i]["IDFINSUB2025"]));
                                }
                                //}
                                //fin 2025

                                //inicio 2026
                                if (metas.Rows[i]["MFINSUB2026"] == DBNull.Value || e.NewValues["MFINSUB2026"] == DBNull.Value)
                                    pre2026 = 0;
                                else if ((metas.Rows[i]["MFINSUB2026"] != DBNull.Value && e.NewValues["MFINSUB2026"] != DBNull.Value) && (Convert.ToDouble(metas.Rows[i]["MFINSUB2026"]) == Convert.ToDouble(e.NewValues["MFINSUB2026"])))
                                    pre2026 = 0;
                                else if (Convert.ToDouble(metas.Rows[i]["MFINSUB2026"]) > Convert.ToDouble(e.NewValues["MFINSUB2026"]))
                                {
                                    pre2026 = pre2026 + Convert.ToDouble(e.NewValues["MFINSUB2026"]);
                                    pre2026 = Convert.ToDouble(e.NewValues["MFINSUB2026"]) - Convert.ToDouble(metas.Rows[i]["MFINSUB2026"]);
                                    pre2026 = pre2026 + Convert.ToDouble(metas.Rows[i]["MFINSUB2026"]);
                                }

                                else
                                {
                                    pre2026 = Convert.ToDouble(e.NewValues["MFINSUB2026"]) - Convert.ToDouble(metas.Rows[i]["MFINSUB2026"]);
                                    pre2026 = pre2026 + Convert.ToDouble(metas.Rows[i]["MFINSUB2026"]);
                                }
                                //pre2024 = pre2024 + Convert.ToDouble(e.NewValues["FIN2024"]);
                                //if (pre2024 > programa2024)
                                //{
                                //    mensajesfinanciero = mensajesfinanciero + " 2024 ";
                                //    fallosfinanciero++;

                                //}
                                //else
                                //{
                                poas = obtienepoa(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), 2026);
                                if (metas.Rows[i]["IDFINSUB2026"] == DBNull.Value)
                                {
                                    //if (Convert.ToDouble(e.NewValues["MFINSUB2026"]) != 0)
                                    if (e.NewValues["MFINSUB2026"] != DBNull.Value)
                                        estado = insertaFinanciera(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MFINSUB2026"]));
                                }
                                else
                                {
                                    //if (Convert.ToDouble(e.NewValues["MFINSUB2026"]) != 0)
                                    if (e.NewValues["MFINSUB2026"] != DBNull.Value)
                                        estado = Acualizametafinanciera(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MFINSUB2026"]), Convert.ToInt32(metas.Rows[i]["IDFINSUB2026"]));
                                    else
                                        estado = Eliminametafinanciera(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToInt32(metas.Rows[i]["IDFINSUB2026"]));
                                }
                                //}

                                if (fallofisico > 0)
                                    mensaje = "Las metas de los años " + mensajesFisico + ", no pudieron ser guardadas por posibles fallos en el sistema, intententelo mas tarde o contacte al administrador";
                                if (fallosfinanciero > 0)
                                    mensaje = mensaje + " Las metas financieras de los años " + mensajesfinanciero + ", no pudieron ser guardadas al estar programando una cantidad mayor, a la cantidad asignada  a su programa, por favor revise las cantidades en los años descritos";
                                if (fallofisico == 0 & fallosfinanciero == 0)
                                    mensaje = "Metas guardadas correctamente";

                                grid.JSProperties["cpError"] = "Información " + mensaje;
                                e.Cancel = true;
                                grid.CancelEdit();
                                break;


                            }

                        }

                        //fin periodo 21




                        //periodo 22

                        if (Convert.ToInt32(Session["periodo"]) == 22)
                        {
                            if (metas.Rows[i]["SPPSUB$ID_SUBPRODUCTO"].ToString() == e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString())
                            {


                                if (programas.Rows[0]["PRESUPUESTO2023"] == DBNull.Value)
                                    programa2023 = 0;
                                else
                                    programa2023 = Convert.ToDouble(programas.Rows[0]["PRESUPUESTO2023"]);



                                if (programas.Rows[0]["PRESUPUESTO2024"] == DBNull.Value)
                                    programa2024 = 0;
                                else
                                    programa2024 = Convert.ToDouble(programas.Rows[0]["PRESUPUESTO2024"]);



                                if (programas.Rows[0]["PRESUPUESTO2025"] == DBNull.Value)
                                    programa2025 = 0;
                                else
                                    programa2025 = Convert.ToDouble(programas.Rows[0]["PRESUPUESTO2025"]);



                                if (programas.Rows[0]["PRESUPUESTO2026"] == DBNull.Value)
                                    programa2026 = 0;
                                else
                                    programa2026 = Convert.ToDouble(programas.Rows[0]["PRESUPUESTO2026"]);




                                if (programas.Rows[0]["PRESUPUESTO2027"] == DBNull.Value)
                                    programa2022 = 0;
                                else
                                    programa2022 = Convert.ToDouble(programas.Rows[0]["PRESUPUESTO2027"]);






                                if ((programas.Rows[0]["PR0GRAMADO2023"] == DBNull.Value))
                                    pre2023 = 0;
                                else
                                    pre2023 = Convert.ToDouble(programas.Rows[0]["PR0GRAMADO2023"]);

                                if ((programas.Rows[0]["PR0GRAMADO2024"] == DBNull.Value))
                                    pre2024 = 0;
                                else
                                    pre2024 = Convert.ToDouble(programas.Rows[0]["PR0GRAMADO2024"]);

                                if ((programas.Rows[0]["PR0GRAMADO2025"] == DBNull.Value))
                                    pre2025 = 0;
                                else
                                    pre2025 = Convert.ToDouble(programas.Rows[0]["PR0GRAMADO2025"]);

                                if ((programas.Rows[0]["PR0GRAMADO2026"] == DBNull.Value))
                                    pre2026 = 0;
                                else
                                    pre2026 = Convert.ToDouble(programas.Rows[0]["PR0GRAMADO2026"]);


                                if (programas.Rows[0]["PR0GRAMADO2027"] == DBNull.Value)
                                    pre2027 = 0;
                                else
                                    pre2027 = Convert.ToDouble(programas.Rows[0]["PR0GRAMADO2027"]);

                                //fin financiero






                                if ((programas.Rows[0]["PR0GRAMADO2023"] == DBNull.Value))
                                    pre2023 = 0;
                                else
                                    pre2023 = Convert.ToDouble(programas.Rows[0]["PR0GRAMADO2023"]);

                                if ((programas.Rows[0]["PR0GRAMADO2024"] == DBNull.Value))
                                    pre2024 = 0;
                                else
                                    pre2024 = Convert.ToDouble(programas.Rows[0]["PR0GRAMADO2024"]);

                                if ((programas.Rows[0]["PR0GRAMADO2025"] == DBNull.Value))
                                    pre2025 = 0;
                                else
                                    pre2025 = Convert.ToDouble(programas.Rows[0]["PR0GRAMADO2025"]);

                                if ((programas.Rows[0]["PR0GRAMADO2026"] == DBNull.Value))
                                    pre2026 = 0;
                                else
                                    pre2026 = Convert.ToDouble(programas.Rows[0]["PR0GRAMADO2026"]);



                                if (programas.Rows[0]["PR0GRAMADO2027"] == DBNull.Value)
                                    pre2027 = 0;
                                else
                                    pre2027 = Convert.ToDouble(programas.Rows[0]["PR0GRAMADO2027"]);




                                poas = obtienepoa(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), 2023);
                                //inicio 2023
                                if (metas.Rows[i]["IDFISUB2023"] == DBNull.Value)
                                {
                                    //if (Convert.ToDouble(e.NewValues["MFSUB2023"]) != 0)
                                    if (e.NewValues["MFSUB2023"] != DBNull.Value)
                                    {
                                        estado = insertaFisicasub(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MFSUB2023"]));
                                        if (estado == 0)
                                        {
                                            mensajesFisico = mensajesFisico + " 2023 ";
                                            fallofisico++;
                                        }
                                    }

                                }

                                else
                                {
                                    //if (Convert.ToDouble(e.NewValues["MFSUB2023"]) != 0)
                                    if (e.NewValues["MFSUB2023"] != DBNull.Value)
                                        estado = Acualizametafisicasub(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MFSUB2023"]), Convert.ToInt32(metas.Rows[i]["IDFISUB2023"]));
                                    else
                                        estado = Eliminafisicasub(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToInt32(metas.Rows[i]["IDFISUB2023"]));


                                    if (estado == 0)
                                    {
                                        mensajesFisico = mensajesFisico + " 2023 ";
                                        fallofisico++;
                                    }
                                }

                                //fin 2023

                                poas = obtienepoa(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), 2024);
                                //inicio 2024
                                if (metas.Rows[i]["IDFISUB2024"] == DBNull.Value)
                                {
                                    //if (Convert.ToDouble(e.NewValues["MFSUB2024"]) != 0)
                                    if (e.NewValues["MFSUB2024"] != DBNull.Value)
                                    {
                                        estado = insertaFisicasub(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MFSUB2024"]));
                                        if (estado == 0)
                                        {
                                            mensajesFisico = mensajesFisico + " 2024 ";
                                            fallofisico++;
                                        }
                                    }

                                }

                                else
                                {
                                    //if (Convert.ToDouble(e.NewValues["MFSUB2024"]) != 0)
                                    if (e.NewValues["MFSUB2024"] != DBNull.Value)
                                        estado = Acualizametafisicasub(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MFSUB2024"]), Convert.ToInt32(metas.Rows[i]["IDFISUB2024"]));
                                    else
                                        estado = Eliminafisicasub(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToInt32(metas.Rows[i]["IDFISUB2024"]));

                                    if (estado == 0)
                                    {
                                        mensajesFisico = mensajesFisico + " 2024 ";
                                        fallofisico++;
                                    }
                                }

                                //fin 2024

                                poas = obtienepoa(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), 2025);
                                //inicio 2025
                                if (metas.Rows[i]["IDFISUB2025"] == DBNull.Value)
                                {
                                    //if (Convert.ToDouble(e.NewValues["MFSUB2025"]) != 0)
                                    if (e.NewValues["MFSUB2025"] != DBNull.Value)
                                    {
                                        estado = insertaFisicasub(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MFSUB2025"]));
                                        if (estado == 0)
                                        {
                                            mensajesFisico = mensajesFisico + " 2025 ";
                                            fallofisico++;
                                        }
                                    }

                                }

                                else
                                {
                                    //if (Convert.ToDouble(e.NewValues["MFSUB2025"]) != 0)
                                    if (e.NewValues["MFSUB2025"] != DBNull.Value)

                                        estado = Acualizametafisicasub(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MFSUB2025"]), Convert.ToInt32(metas.Rows[i]["IDFISUB2025"]));
                                    else
                                        estado = Eliminafisicasub(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToInt32(metas.Rows[i]["IDFISUB2025"]));

                                    if (estado == 0)
                                    {
                                        mensajesFisico = mensajesFisico + " 2025 ";
                                        fallofisico++;
                                    }
                                }

                                //fin 2025

                                poas = obtienepoa(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), 2026);
                                //inicio 2025
                                if (metas.Rows[i]["IDFISUB2026"] == DBNull.Value)
                                {
                                    //if (Convert.ToDouble(e.NewValues["MFSUB2026"]) != 0)
                                    if (e.NewValues["MFSUB2026"] != DBNull.Value)
                                    {
                                        estado = insertaFisicasub(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MFSUB2026"]));
                                        if (estado == 0)
                                        {
                                            mensajesFisico = mensajesFisico + " 2026 ";
                                            fallofisico++;
                                        }
                                    }

                                }

                                else
                                {
                                    //if (Convert.ToDouble(e.NewValues["MFSUB2026"]) != 0)
                                    if (e.NewValues["MFSUB2026"] != DBNull.Value)


                                        estado = Acualizametafisicasub(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MFSUB2026"]), Convert.ToInt32(metas.Rows[i]["IDFISUB2026"]));
                                    else
                                        estado = Eliminafisicasub(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToInt32(metas.Rows[i]["IDFISUB2026"]));

                                    if (estado == 0)
                                    {
                                        mensaje = mensaje + " 2026 ";
                                        fallofisico++;
                                    }
                                }

                                //fin 2026

                                poas = obtienepoa(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), 2027);
                                //inicio 2027
                                if (metas.Rows[i]["IDFISUB2027"] == DBNull.Value)
                                {
                                    //if (Convert.ToDouble(e.NewValues["MFSUB2022"]) != 0)
                                    if (e.NewValues["MFSUB2027"] != DBNull.Value)
                                    {
                                        estado = insertaFisicasub(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MFSUB2027"]));
                                        if (estado == 0)
                                        {
                                            mensajesFisico = mensajesFisico + " 2027 ";
                                            fallofisico++;
                                        }
                                    }


                                }

                                else
                                {
                                    //if (Convert.ToDouble(e.NewValues["MFSUB2022"]) != 0)
                                    if (e.NewValues["MFSUB2027"] != DBNull.Value)

                                        estado = Acualizametafisicasub(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MFSUB2027"]), Convert.ToInt32(metas.Rows[i]["IDFISUB2027"]));
                                    else
                                        estado = Eliminafisicasub(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToInt32(metas.Rows[i]["IDFISUB2027"]));

                                    if (estado == 0)
                                    {
                                        mensajesFisico = mensajesFisico + " 2027 ";
                                        fallofisico++;
                                    }
                                }

                                //fin 2027






                                //inicio 2023
                                if (metas.Rows[i]["MFINSUB2023"] == DBNull.Value || e.NewValues["MFINSUB2023"] == DBNull.Value)
                                    pre2023 = 0;
                                else if ((metas.Rows[i]["MFINSUB2023"] != DBNull.Value && e.NewValues["MFINSUB2023"] != DBNull.Value) && (Convert.ToDouble(metas.Rows[i]["MFINSUB2023"]) == Convert.ToDouble(e.NewValues["MFINSUB2023"])))
                                    pre2023 = 0;
                                else if (Convert.ToDouble(metas.Rows[i]["MFINSUB2023"]) > Convert.ToDouble(e.NewValues["MFINSUB2023"]))
                                {
                                    pre2023 = pre2023 + Convert.ToDouble(e.NewValues["MFINSUB2023"]);
                                    pre2023 = Convert.ToDouble(e.NewValues["MFINSUB2023"]) - Convert.ToDouble(metas.Rows[i]["MFINSUB2023"]);
                                    pre2023 = pre2023 + Convert.ToDouble(metas.Rows[i]["MFINSUB2023"]);
                                }

                                else
                                {
                                    pre2023 = Convert.ToDouble(e.NewValues["MFINSUB2023"]) - Convert.ToDouble(metas.Rows[i]["MFINSUB2023"]);
                                    pre2023 = pre2023 + Convert.ToDouble(metas.Rows[i]["MFINSUB2023"]);
                                }

                                //pre2021 = pre2021 + Convert.ToDouble(e.NewValues["FIN2021"]);
                                //if (pre2021 > programa2021)
                                //{
                                //    mensajesfinanciero = mensajesfinanciero + " 2021 ";
                                //    fallosfinanciero++;
                                //}
                                //else
                                //{
                                poas = obtienepoa(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), 2023);
                                if (metas.Rows[i]["IDFINSUB2023"] == DBNull.Value)
                                {
                                    //if (Convert.ToDouble(e.NewValues["MFINSUB2023"]) != 0)
                                    if (e.NewValues["MFINSUB2023"] != DBNull.Value)
                                        estado = insertaFinanciera(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MFINSUB2023"]));
                                }
                                else
                                {
                                    //if (Convert.ToDouble(e.NewValues["MFINSUB2023"]) != 0)
                                    if (e.NewValues["MFINSUB2023"] != DBNull.Value)
                                        estado = estado = Acualizametafinanciera(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MFINSUB2023"]), Convert.ToInt32(metas.Rows[i]["IDFINSUB2023"]));
                                    else
                                        estado = Eliminametafinanciera(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToInt32(metas.Rows[i]["IDFINSUB2023"]));
                                }
                                //}
                                //fin 2023


                                //inicio 2024
                                if (metas.Rows[i]["MFSUB2024"] == DBNull.Value || e.NewValues["MFSUB20224"] == DBNull.Value)
                                    pre2024 = 0;
                                else if ((metas.Rows[i]["MFSUB2024"] != DBNull.Value && e.NewValues["MFSUB2024"] != DBNull.Value) && (Convert.ToDouble(metas.Rows[i]["MFSUB2024"]) == Convert.ToDouble(e.NewValues["MFSUB2024"])))
                                    pre2024 = 0;
                                else if (Convert.ToDouble(metas.Rows[i]["MFSUB2024"]) > Convert.ToDouble(e.NewValues["MFSUB2024"]))
                                {
                                    pre2024 = pre2024 + Convert.ToDouble(e.NewValues["MFSUB2024"]);
                                    pre2024 = Convert.ToDouble(e.NewValues["MFSUB2024"]) - Convert.ToDouble(metas.Rows[i]["MFSUB2024"]);
                                    pre2024 = pre2024 + Convert.ToDouble(metas.Rows[i]["MFSUB2024"]);
                                }

                                else
                                {
                                    pre2024 = Convert.ToDouble(e.NewValues["MFSUB2024"]) - Convert.ToDouble(metas.Rows[i]["MFSUB2024"]);
                                    pre2024 = pre2024 + Convert.ToDouble(metas.Rows[i]["MFSUB2024"]);
                                }
                                //pre2022 = pre2022 + Convert.ToDouble(e.NewValues["FIN2022"]);
                                //if (pre2023 > programa2023)
                                //{
                                //    mensajesfinanciero = mensajesfinanciero + " 2023 ";
                                //    fallosfinanciero++;

                                //}
                                //else
                                //{
                                poas = obtienepoa(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), 2024);
                                if (metas.Rows[i]["IDFINSUB2024"] == DBNull.Value)
                                {
                                    //if (Convert.ToDouble(e.NewValues["MFINSUB2024"]) != 0)
                                    if (e.NewValues["MFINSUB2024"] != DBNull.Value)
                                        estado = insertaFinanciera(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MFINSUB2024"]));
                                }
                                else
                                {
                                    //if (Convert.ToDouble(e.NewValues["MFINSUB2024"]) != 0)
                                    if (e.NewValues["MFINSUB2024"] != DBNull.Value)
                                        estado = Acualizametafinanciera(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MFINSUB2024"]), Convert.ToInt32(metas.Rows[i]["IDFINSUB2024"]));
                                    else
                                        estado = Eliminametafinanciera(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToInt32(metas.Rows[i]["IDFINSUB2024"]));
                                }
                                //}
                                //fin 2024

                                //inicio 2025
                                if (metas.Rows[i]["MFINSUB2025"] == DBNull.Value || e.NewValues["MFINSUB2025"] == DBNull.Value)
                                    pre2025 = 0;
                                else if ((metas.Rows[i]["MFINSUB2025"] != DBNull.Value && e.NewValues["MFINSUB2025"] != DBNull.Value) && (Convert.ToDouble(metas.Rows[i]["MFINSUB2025"]) == Convert.ToDouble(e.NewValues["MFINSUB2025"])))
                                    pre2025 = 0;
                                else if (Convert.ToDouble(metas.Rows[i]["MFINSUB2025"]) > Convert.ToDouble(e.NewValues["MFINSUB2025"]))
                                {
                                    pre2025 = pre2025 + Convert.ToDouble(e.NewValues["MFINSUB2025"]);
                                    pre2025 = Convert.ToDouble(e.NewValues["MFINSUB2025"]) - Convert.ToDouble(metas.Rows[i]["MFINSUB2025"]);
                                    pre2025 = pre2025 + Convert.ToDouble(metas.Rows[i]["MFINSUB2025"]);
                                }

                                else
                                {
                                    pre2025 = Convert.ToDouble(e.NewValues["MFINSUB2025"]) - Convert.ToDouble(metas.Rows[i]["MFINSUB2025"]);
                                    pre2025 = pre2025 + Convert.ToDouble(metas.Rows[i]["MFINSUB2025"]);
                                }
                                //pre2023 = pre2023 + Convert.ToDouble(e.NewValues["FIN2023"]);
                                //if (pre2023 > programa2023)
                                //{
                                //    mensajesfinanciero = mensajesfinanciero + " 2023 ";
                                //    fallosfinanciero++;

                                //}
                                //else
                                //{
                                poas = obtienepoa(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), 2025);
                                if (metas.Rows[i]["IDFINSUB2025"] == DBNull.Value)
                                {
                                    //if (Convert.ToDouble(e.NewValues["MFINSUB2025"]) != 0)
                                    if (e.NewValues["MFINSUB2025"] != DBNull.Value)
                                        estado = insertaFinanciera(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MFINSUB2025"]));
                                }
                                else
                                {
                                    //if (Convert.ToDouble(e.NewValues["MFINSUB2025"]) != 0)
                                    if (e.NewValues["MFINSUB2025"] != DBNull.Value)
                                        estado = Acualizametafinanciera(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MFINSUB2025"]), Convert.ToInt32(metas.Rows[i]["IDFINSUB2025"]));
                                    else
                                        estado = Eliminametafinanciera(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToInt32(metas.Rows[i]["IDFINSUB2025"]));
                                }
                                //}
                                //fin 2025

                                //inicio 2026
                                if (metas.Rows[i]["MFINSUB2026"] == DBNull.Value || e.NewValues["MFINSUB2026"] == DBNull.Value)
                                    pre2026 = 0;
                                else if ((metas.Rows[i]["MFINSUB2026"] != DBNull.Value && e.NewValues["MFINSUB2026"] != DBNull.Value) && (Convert.ToDouble(metas.Rows[i]["MFINSUB2026"]) == Convert.ToDouble(e.NewValues["MFINSUB2026"])))
                                    pre2026 = 0;
                                else if (Convert.ToDouble(metas.Rows[i]["MFINSUB2026"]) > Convert.ToDouble(e.NewValues["MFINSUB2026"]))
                                {
                                    pre2026 = pre2026 + Convert.ToDouble(e.NewValues["MFINSUB2026"]);
                                    pre2026 = Convert.ToDouble(e.NewValues["MFINSUB2026"]) - Convert.ToDouble(metas.Rows[i]["MFINSUB2026"]);
                                    pre2026 = pre2026 + Convert.ToDouble(metas.Rows[i]["MFINSUB2026"]);
                                }

                                else
                                {
                                    pre2026 = Convert.ToDouble(e.NewValues["MFINSUB2026"]) - Convert.ToDouble(metas.Rows[i]["MFINSUB2026"]);
                                    pre2026 = pre2026 + Convert.ToDouble(metas.Rows[i]["MFINSUB2026"]);
                                }
                                //pre2024 = pre2024 + Convert.ToDouble(e.NewValues["FIN2024"]);
                                //if (pre2024 > programa2024)
                                //{
                                //    mensajesfinanciero = mensajesfinanciero + " 2024 ";
                                //    fallosfinanciero++;

                                //}
                                //else
                                //{
                                poas = obtienepoa(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), 2026);
                                if (metas.Rows[i]["IDFINSUB2026"] == DBNull.Value)
                                {
                                    //if (Convert.ToDouble(e.NewValues["MFINSUB2026"]) != 0)
                                    if (e.NewValues["MFINSUB2026"] != DBNull.Value)
                                        estado = insertaFinanciera(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MFINSUB2026"]));
                                }
                                else
                                {
                                    //if (Convert.ToDouble(e.NewValues["MFINSUB2026"]) != 0)
                                    if (e.NewValues["MFINSUB2026"] != DBNull.Value)
                                        estado = Acualizametafinanciera(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MFINSUB2026"]), Convert.ToInt32(metas.Rows[i]["IDFINSUB2026"]));
                                    else
                                        estado = Eliminametafinanciera(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToInt32(metas.Rows[i]["IDFINSUB2026"]));
                                }
                                //}



                                //inicio 2022

                                if (metas.Rows[i]["MFINSUB2027"] == DBNull.Value || e.NewValues["MFINSUB2027"] == DBNull.Value)
                                    pre2022 = 0;
                                else if ((metas.Rows[i]["MFINSUB2027"] != DBNull.Value && e.NewValues["MFINSUB2027"] != DBNull.Value) && (Convert.ToDouble(metas.Rows[i]["MFINSUB2027"]) == Convert.ToDouble(e.NewValues["MFINSUB2027"])))
                                    pre2022 = 0;
                                else if (Convert.ToDouble(metas.Rows[i]["MFINSUB2027"]) > Convert.ToDouble(e.NewValues["MFINSUB2027"]))
                                {
                                    pre2027 = pre2027 + Convert.ToDouble(e.NewValues["MFINSUB2027"]);
                                    pre2027 = Convert.ToDouble(e.NewValues["MFINSUB2027"]) - Convert.ToDouble(metas.Rows[i]["MFINSUB2027"]);
                                    pre2027 = pre2027 + Convert.ToDouble(metas.Rows[i]["MFINSUB2027"]);
                                }

                                else
                                {
                                    pre2027 = Convert.ToDouble(e.NewValues["MFINSUB2027"]) - Convert.ToDouble(metas.Rows[i]["MFINSUB2027"]);
                                    pre2022 = pre2027 + Convert.ToDouble(metas.Rows[i]["MFINSUB2027"]);
                                }

                                //if (pre2021 > programa2021)
                                //{
                                //    mensajesfinanciero = mensajesfinanciero + " 2021 ";
                                //    //fallosfinanciero++;
                                //}
                                //else
                                //{
                                poas = obtienepoa(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), 2027);
                                if (metas.Rows[i]["IDFINSUB2027"] == DBNull.Value)
                                {
                                    //if (Convert.ToDouble(e.NewValues["MFINSUB2022"]) != 0)
                                    if (e.NewValues["MFINSUB2027"] != DBNull.Value)
                                        estado = insertaFinanciera(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MFINSUB2027"]));
                                }
                                else
                                {
                                    //if (Convert.ToDouble(e.NewValues["MFINSUB2022"]) != 0)
                                    if (e.NewValues["MFINSUB2027"] != DBNull.Value)
                                        estado = Acualizametafinanciera(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MFINSUB2027"]), Convert.ToInt32(metas.Rows[i]["IDFINSUB2027"]));
                                    else
                                        estado = Eliminametafinanciera(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToInt32(metas.Rows[i]["IDFINSUB2027"]));
                                }
                                //}
                                //fin 2027


                                if (fallofisico > 0)
                                    mensaje = "Las metas de los años " + mensajesFisico + ", no pudieron ser guardadas por posibles fallos en el sistema, intententelo mas tarde o contacte al administrador";
                                if (fallosfinanciero > 0)
                                    mensaje = mensaje + " Las metas financieras de los años " + mensajesfinanciero + ", no pudieron ser guardadas al estar programando una cantidad mayor, a la cantidad asignada  a su programa, por favor revise las cantidades en los años descritos";
                                if (fallofisico == 0 & fallosfinanciero == 0)
                                    mensaje = "Metas guardadas correctamente";

                                grid.JSProperties["cpError"] = "Información " + mensaje;
                                e.Cancel = true;
                                grid.CancelEdit();
                                break;


                            }

                        }





                        //periodo 23

                        if (Convert.ToInt32(Session["periodo"]) == 23)
                        {
                            if (metas.Rows[i]["SPPSUB$ID_SUBPRODUCTO"].ToString() == e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString())
                            {



                                if (programas.Rows[0]["PRESUPUESTO2024"] == DBNull.Value)
                                    programa2024 = 0;
                                else
                                    programa2024 = Convert.ToDouble(programas.Rows[0]["PRESUPUESTO2024"]);



                                if (programas.Rows[0]["PRESUPUESTO2025"] == DBNull.Value)
                                    programa2025 = 0;
                                else
                                    programa2025 = Convert.ToDouble(programas.Rows[0]["PRESUPUESTO2025"]);



                                if (programas.Rows[0]["PRESUPUESTO2026"] == DBNull.Value)
                                    programa2026 = 0;
                                else
                                    programa2026 = Convert.ToDouble(programas.Rows[0]["PRESUPUESTO2026"]);


                                if (programas.Rows[0]["PRESUPUESTO2027"] == DBNull.Value)
                                    programa2027 = 0;
                                else
                                    programa2027 = Convert.ToDouble(programas.Rows[0]["PRESUPUESTO2027"]);


                                if (programas.Rows[0]["PRESUPUESTO2028"] == DBNull.Value)
                                    programa2028 = 0;
                                else
                                    programa2028 = Convert.ToDouble(programas.Rows[0]["PRESUPUESTO2028"]);







                                if ((programas.Rows[0]["PR0GRAMADO2024"] == DBNull.Value))
                                    pre2024 = 0;
                                else
                                    pre2024 = Convert.ToDouble(programas.Rows[0]["PR0GRAMADO2024"]);

                                if ((programas.Rows[0]["PR0GRAMADO2025"] == DBNull.Value))
                                    pre2025 = 0;
                                else
                                    pre2025 = Convert.ToDouble(programas.Rows[0]["PR0GRAMADO2025"]);

                                if ((programas.Rows[0]["PR0GRAMADO2026"] == DBNull.Value))
                                    pre2026 = 0;
                                else
                                    pre2026 = Convert.ToDouble(programas.Rows[0]["PR0GRAMADO2026"]);


                                if (programas.Rows[0]["PR0GRAMADO2027"] == DBNull.Value)
                                    pre2027 = 0;
                                else
                                    pre2027 = Convert.ToDouble(programas.Rows[0]["PR0GRAMADO2027"]);


                                if ((programas.Rows[0]["PR0GRAMADO2028"] == DBNull.Value))
                                    pre2028 = 0;
                                else
                                    pre2028 = Convert.ToDouble(programas.Rows[0]["PR0GRAMADO2028"]);

                                //fin financiero








                                if ((programas.Rows[0]["PR0GRAMADO2024"] == DBNull.Value))
                                    pre2024 = 0;
                                else
                                    pre2024 = Convert.ToDouble(programas.Rows[0]["PR0GRAMADO2024"]);

                                if ((programas.Rows[0]["PR0GRAMADO2025"] == DBNull.Value))
                                    pre2025 = 0;
                                else
                                    pre2025 = Convert.ToDouble(programas.Rows[0]["PR0GRAMADO2025"]);

                                if ((programas.Rows[0]["PR0GRAMADO2026"] == DBNull.Value))
                                    pre2026 = 0;
                                else
                                    pre2026 = Convert.ToDouble(programas.Rows[0]["PR0GRAMADO2026"]);



                                if (programas.Rows[0]["PR0GRAMADO2027"] == DBNull.Value)
                                    pre2027 = 0;
                                else
                                    pre2027 = Convert.ToDouble(programas.Rows[0]["PR0GRAMADO2027"]);


                                if ((programas.Rows[0]["PR0GRAMADO2028"] == DBNull.Value))
                                    pre2028 = 0;
                                else
                                    pre2028 = Convert.ToDouble(programas.Rows[0]["PR0GRAMADO2028"]);




                                poas = obtienepoa(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), 2024);
                                //inicio 2024
                                if (metas.Rows[i]["IDFISUB2024"] == DBNull.Value)
                                {
                                    //if (Convert.ToDouble(e.NewValues["MFSUB2024"]) != 0)
                                    if (e.NewValues["MFSUB2024"] != DBNull.Value)
                                    {
                                        estado = insertaFisicasub(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MFSUB2024"]));
                                        if (estado == 0)
                                        {
                                            mensajesFisico = mensajesFisico + " 2024 ";
                                            fallofisico++;
                                        }
                                    }

                                }

                                else
                                {
                                    //if (Convert.ToDouble(e.NewValues["MFSUB2024"]) != 0)
                                    if (e.NewValues["MFSUB2024"] != DBNull.Value)
                                        estado = Acualizametafisicasub(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MFSUB2024"]), Convert.ToInt32(metas.Rows[i]["IDFISUB2024"]));
                                    else
                                        estado = Eliminafisicasub(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToInt32(metas.Rows[i]["IDFISUB2024"]));

                                    if (estado == 0)
                                    {
                                        mensajesFisico = mensajesFisico + " 2024 ";
                                        fallofisico++;
                                    }
                                }

                                //fin 2024

                                poas = obtienepoa(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), 2025);
                                //inicio 2025
                                if (metas.Rows[i]["IDFISUB2025"] == DBNull.Value)
                                {
                                    //if (Convert.ToDouble(e.NewValues["MFSUB2025"]) != 0)
                                    if (e.NewValues["MFSUB2025"] != DBNull.Value)
                                    {
                                        estado = insertaFisicasub(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MFSUB2025"]));
                                        if (estado == 0)
                                        {
                                            mensajesFisico = mensajesFisico + " 2025 ";
                                            fallofisico++;
                                        }
                                    }

                                }

                                else
                                {
                                    //if (Convert.ToDouble(e.NewValues["MFSUB2025"]) != 0)
                                    if (e.NewValues["MFSUB2025"] != DBNull.Value)

                                        estado = Acualizametafisicasub(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MFSUB2025"]), Convert.ToInt32(metas.Rows[i]["IDFISUB2025"]));
                                    else
                                        estado = Eliminafisicasub(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToInt32(metas.Rows[i]["IDFISUB2025"]));

                                    if (estado == 0)
                                    {
                                        mensajesFisico = mensajesFisico + " 2025 ";
                                        fallofisico++;
                                    }
                                }

                                //fin 2025

                                poas = obtienepoa(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), 2026);
                                //inicio 2025
                                if (metas.Rows[i]["IDFISUB2026"] == DBNull.Value)
                                {
                                    //if (Convert.ToDouble(e.NewValues["MFSUB2026"]) != 0)
                                    if (e.NewValues["MFSUB2026"] != DBNull.Value)
                                    {
                                        estado = insertaFisicasub(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MFSUB2026"]));
                                        if (estado == 0)
                                        {
                                            mensajesFisico = mensajesFisico + " 2026 ";
                                            fallofisico++;
                                        }
                                    }

                                }

                                else
                                {
                                    //if (Convert.ToDouble(e.NewValues["MFSUB2026"]) != 0)
                                    if (e.NewValues["MFSUB2026"] != DBNull.Value)


                                        estado = Acualizametafisicasub(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MFSUB2026"]), Convert.ToInt32(metas.Rows[i]["IDFISUB2026"]));
                                    else
                                        estado = Eliminafisicasub(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToInt32(metas.Rows[i]["IDFISUB2026"]));

                                    if (estado == 0)
                                    {
                                        mensaje = mensaje + " 2026 ";
                                        fallofisico++;
                                    }
                                }

                                //fin 2026

                                poas = obtienepoa(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), 2027);
                                //inicio 2027
                                if (metas.Rows[i]["IDFISUB2027"] == DBNull.Value)
                                {
                                    //if (Convert.ToDouble(e.NewValues["MFSUB2022"]) != 0)
                                    if (e.NewValues["MFSUB2027"] != DBNull.Value)
                                    {
                                        estado = insertaFisicasub(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MFSUB2027"]));
                                        if (estado == 0)
                                        {
                                            mensajesFisico = mensajesFisico + " 2027 ";
                                            fallofisico++;
                                        }
                                    }


                                }

                                else
                                {
                                    //if (Convert.ToDouble(e.NewValues["MFSUB2022"]) != 0)
                                    if (e.NewValues["MFSUB2027"] != DBNull.Value)

                                        estado = Acualizametafisicasub(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MFSUB2027"]), Convert.ToInt32(metas.Rows[i]["IDFISUB2027"]));
                                    else
                                        estado = Eliminafisicasub(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToInt32(metas.Rows[i]["IDFISUB2027"]));

                                    if (estado == 0)
                                    {
                                        mensajesFisico = mensajesFisico + " 2027 ";
                                        fallofisico++;
                                    }
                                }

                                //fin 2027




                                poas = obtienepoa(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), 2028);
                                //inicio 2028
                                if (metas.Rows[i]["IDFISUB2028"] == DBNull.Value)
                                {
                                    //if (Convert.ToDouble(e.NewValues["MFSUB2023"]) != 0)
                                    if (e.NewValues["MFSUB2028"] != DBNull.Value)
                                    {
                                        estado = insertaFisicasub(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MFSUB2028"]));
                                        if (estado == 0)
                                        {
                                            mensajesFisico = mensajesFisico + " 2028 ";
                                            fallofisico++;
                                        }
                                    }

                                }

                                else
                                {
                                    //if (Convert.ToDouble(e.NewValues["MFSUB2023"]) != 0)
                                    if (e.NewValues["MFSUB2028"] != DBNull.Value)
                                        estado = Acualizametafisicasub(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MFSUB2028"]), Convert.ToInt32(metas.Rows[i]["IDFISUB2028"]));
                                    else
                                        estado = Eliminafisicasub(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToInt32(metas.Rows[i]["IDFISUB2028"]));


                                    if (estado == 0)
                                    {
                                        mensajesFisico = mensajesFisico + " 2028 ";
                                        fallofisico++;
                                    }
                                }

                                //fin 2028





                                //inicio 2024
                                if (metas.Rows[i]["MFSUB2024"] == DBNull.Value || e.NewValues["MFSUB20224"] == DBNull.Value)
                                    pre2024 = 0;
                                else if ((metas.Rows[i]["MFSUB2024"] != DBNull.Value && e.NewValues["MFSUB2024"] != DBNull.Value) && (Convert.ToDouble(metas.Rows[i]["MFSUB2024"]) == Convert.ToDouble(e.NewValues["MFSUB2024"])))
                                    pre2024 = 0;
                                else if (Convert.ToDouble(metas.Rows[i]["MFSUB2024"]) > Convert.ToDouble(e.NewValues["MFSUB2024"]))
                                {
                                    pre2024 = pre2024 + Convert.ToDouble(e.NewValues["MFSUB2024"]);
                                    pre2024 = Convert.ToDouble(e.NewValues["MFSUB2024"]) - Convert.ToDouble(metas.Rows[i]["MFSUB2024"]);
                                    pre2024 = pre2024 + Convert.ToDouble(metas.Rows[i]["MFSUB2024"]);
                                }

                                else
                                {
                                    pre2024 = Convert.ToDouble(e.NewValues["MFSUB2024"]) - Convert.ToDouble(metas.Rows[i]["MFSUB2024"]);
                                    pre2024 = pre2024 + Convert.ToDouble(metas.Rows[i]["MFSUB2024"]);
                                }
                                //pre2022 = pre2022 + Convert.ToDouble(e.NewValues["FIN2022"]);
                                //if (pre2023 > programa2023)
                                //{
                                //    mensajesfinanciero = mensajesfinanciero + " 2023 ";
                                //    fallosfinanciero++;

                                //}
                                //else
                                //{
                                poas = obtienepoa(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), 2024);
                                if (metas.Rows[i]["IDFINSUB2024"] == DBNull.Value)
                                {
                                    //if (Convert.ToDouble(e.NewValues["MFINSUB2024"]) != 0)
                                    if (e.NewValues["MFINSUB2024"] != DBNull.Value)
                                        estado = insertaFinanciera(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MFINSUB2024"]));
                                }
                                else
                                {
                                    //if (Convert.ToDouble(e.NewValues["MFINSUB2024"]) != 0)
                                    if (e.NewValues["MFINSUB2024"] != DBNull.Value)
                                        estado = Acualizametafinanciera(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MFINSUB2024"]), Convert.ToInt32(metas.Rows[i]["IDFINSUB2024"]));
                                    else
                                        estado = Eliminametafinanciera(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToInt32(metas.Rows[i]["IDFINSUB2024"]));
                                }
                                //}
                                //fin 2024

                                //inicio 2025
                                if (metas.Rows[i]["MFINSUB2025"] == DBNull.Value || e.NewValues["MFINSUB2025"] == DBNull.Value)
                                    pre2025 = 0;
                                else if ((metas.Rows[i]["MFINSUB2025"] != DBNull.Value && e.NewValues["MFINSUB2025"] != DBNull.Value) && (Convert.ToDouble(metas.Rows[i]["MFINSUB2025"]) == Convert.ToDouble(e.NewValues["MFINSUB2025"])))
                                    pre2025 = 0;
                                else if (Convert.ToDouble(metas.Rows[i]["MFINSUB2025"]) > Convert.ToDouble(e.NewValues["MFINSUB2025"]))
                                {
                                    pre2025 = pre2025 + Convert.ToDouble(e.NewValues["MFINSUB2025"]);
                                    pre2025 = Convert.ToDouble(e.NewValues["MFINSUB2025"]) - Convert.ToDouble(metas.Rows[i]["MFINSUB2025"]);
                                    pre2025 = pre2025 + Convert.ToDouble(metas.Rows[i]["MFINSUB2025"]);
                                }

                                else
                                {
                                    pre2025 = Convert.ToDouble(e.NewValues["MFINSUB2025"]) - Convert.ToDouble(metas.Rows[i]["MFINSUB2025"]);
                                    pre2025 = pre2025 + Convert.ToDouble(metas.Rows[i]["MFINSUB2025"]);
                                }
                                //pre2023 = pre2023 + Convert.ToDouble(e.NewValues["FIN2023"]);
                                //if (pre2023 > programa2023)
                                //{
                                //    mensajesfinanciero = mensajesfinanciero + " 2023 ";
                                //    fallosfinanciero++;

                                //}
                                //else
                                //{
                                poas = obtienepoa(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), 2025);
                                if (metas.Rows[i]["IDFINSUB2025"] == DBNull.Value)
                                {
                                    //if (Convert.ToDouble(e.NewValues["MFINSUB2025"]) != 0)
                                    if (e.NewValues["MFINSUB2025"] != DBNull.Value)
                                        estado = insertaFinanciera(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MFINSUB2025"]));
                                }
                                else
                                {
                                    //if (Convert.ToDouble(e.NewValues["MFINSUB2025"]) != 0)
                                    if (e.NewValues["MFINSUB2025"] != DBNull.Value)
                                        estado = Acualizametafinanciera(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MFINSUB2025"]), Convert.ToInt32(metas.Rows[i]["IDFINSUB2025"]));
                                    else
                                        estado = Eliminametafinanciera(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToInt32(metas.Rows[i]["IDFINSUB2025"]));
                                }
                                //}
                                //fin 2025

                                //inicio 2026
                                if (metas.Rows[i]["MFINSUB2026"] == DBNull.Value || e.NewValues["MFINSUB2026"] == DBNull.Value)
                                    pre2026 = 0;
                                else if ((metas.Rows[i]["MFINSUB2026"] != DBNull.Value && e.NewValues["MFINSUB2026"] != DBNull.Value) && (Convert.ToDouble(metas.Rows[i]["MFINSUB2026"]) == Convert.ToDouble(e.NewValues["MFINSUB2026"])))
                                    pre2026 = 0;
                                else if (Convert.ToDouble(metas.Rows[i]["MFINSUB2026"]) > Convert.ToDouble(e.NewValues["MFINSUB2026"]))
                                {
                                    pre2026 = pre2026 + Convert.ToDouble(e.NewValues["MFINSUB2026"]);
                                    pre2026 = Convert.ToDouble(e.NewValues["MFINSUB2026"]) - Convert.ToDouble(metas.Rows[i]["MFINSUB2026"]);
                                    pre2026 = pre2026 + Convert.ToDouble(metas.Rows[i]["MFINSUB2026"]);
                                }

                                else
                                {
                                    pre2026 = Convert.ToDouble(e.NewValues["MFINSUB2026"]) - Convert.ToDouble(metas.Rows[i]["MFINSUB2026"]);
                                    pre2026 = pre2026 + Convert.ToDouble(metas.Rows[i]["MFINSUB2026"]);
                                }
                                //pre2024 = pre2024 + Convert.ToDouble(e.NewValues["FIN2024"]);
                                //if (pre2024 > programa2024)
                                //{
                                //    mensajesfinanciero = mensajesfinanciero + " 2024 ";
                                //    fallosfinanciero++;

                                //}
                                //else
                                //{
                                poas = obtienepoa(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), 2026);
                                if (metas.Rows[i]["IDFINSUB2026"] == DBNull.Value)
                                {
                                    //if (Convert.ToDouble(e.NewValues["MFINSUB2026"]) != 0)
                                    if (e.NewValues["MFINSUB2026"] != DBNull.Value)
                                        estado = insertaFinanciera(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MFINSUB2026"]));
                                }
                                else
                                {
                                    //if (Convert.ToDouble(e.NewValues["MFINSUB2026"]) != 0)
                                    if (e.NewValues["MFINSUB2026"] != DBNull.Value)
                                        estado = Acualizametafinanciera(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MFINSUB2026"]), Convert.ToInt32(metas.Rows[i]["IDFINSUB2026"]));
                                    else
                                        estado = Eliminametafinanciera(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToInt32(metas.Rows[i]["IDFINSUB2026"]));
                                }
                                //}



                                //inicio 2027

                                if (metas.Rows[i]["MFINSUB2027"] == DBNull.Value || e.NewValues["MFINSUB2027"] == DBNull.Value)
                                    pre2027 = 0;
                                else if ((metas.Rows[i]["MFINSUB2027"] != DBNull.Value && e.NewValues["MFINSUB2027"] != DBNull.Value) && (Convert.ToDouble(metas.Rows[i]["MFINSUB2027"]) == Convert.ToDouble(e.NewValues["MFINSUB2027"])))
                                    pre2027 = 0;
                                else if (Convert.ToDouble(metas.Rows[i]["MFINSUB2027"]) > Convert.ToDouble(e.NewValues["MFINSUB2027"]))
                                {
                                    pre2027 = pre2027 + Convert.ToDouble(e.NewValues["MFINSUB2027"]);
                                    pre2027 = Convert.ToDouble(e.NewValues["MFINSUB2027"]) - Convert.ToDouble(metas.Rows[i]["MFINSUB2027"]);
                                    pre2027 = pre2027 + Convert.ToDouble(metas.Rows[i]["MFINSUB2027"]);
                                }

                                else
                                {
                                    pre2027 = Convert.ToDouble(e.NewValues["MFINSUB2027"]) - Convert.ToDouble(metas.Rows[i]["MFINSUB2027"]);
                                    pre2022 = pre2027 + Convert.ToDouble(metas.Rows[i]["MFINSUB2027"]);
                                }

                                //if (pre2021 > programa2021)
                                //{
                                //    mensajesfinanciero = mensajesfinanciero + " 2021 ";
                                //    //fallosfinanciero++;
                                //}
                                //else
                                //{
                                poas = obtienepoa(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), 2027);
                                if (metas.Rows[i]["IDFINSUB2027"] == DBNull.Value)
                                {
                                    //if (Convert.ToDouble(e.NewValues["MFINSUB2022"]) != 0)
                                    if (e.NewValues["MFINSUB2027"] != DBNull.Value)
                                        estado = insertaFinanciera(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MFINSUB2027"]));
                                }
                                else
                                {
                                    //if (Convert.ToDouble(e.NewValues["MFINSUB2022"]) != 0)
                                    if (e.NewValues["MFINSUB2027"] != DBNull.Value)
                                        estado = Acualizametafinanciera(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MFINSUB2027"]), Convert.ToInt32(metas.Rows[i]["IDFINSUB2027"]));
                                    else
                                        estado = Eliminametafinanciera(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToInt32(metas.Rows[i]["IDFINSUB2027"]));
                                }
                                //}
                                //fin 2027




                                //inicio 2028
                                if (metas.Rows[i]["MFINSUB2028"] == DBNull.Value || e.NewValues["MFINSUB2028"] == DBNull.Value)
                                    pre2028 = 0;
                                else if ((metas.Rows[i]["MFINSUB2028"] != DBNull.Value && e.NewValues["MFINSUB2028"] != DBNull.Value) && (Convert.ToDouble(metas.Rows[i]["MFINSUB2028"]) == Convert.ToDouble(e.NewValues["MFINSUB2028"])))
                                    pre2028 = 0;
                                else if (Convert.ToDouble(metas.Rows[i]["MFINSUB2028"]) > Convert.ToDouble(e.NewValues["MFINSUB2028"]))
                                {
                                    pre2028 = pre2028 + Convert.ToDouble(e.NewValues["MFINSUB2028"]);
                                    pre2028 = Convert.ToDouble(e.NewValues["MFINSUB2028"]) - Convert.ToDouble(metas.Rows[i]["MFINSUB2028"]);
                                    pre2028 = pre2028 + Convert.ToDouble(metas.Rows[i]["MFINSUB2028"]);
                                }

                                else
                                {
                                    pre2028 = Convert.ToDouble(e.NewValues["MFINSUB2028"]) - Convert.ToDouble(metas.Rows[i]["MFINSUB2028"]);
                                    pre2028 = pre2028 + Convert.ToDouble(metas.Rows[i]["MFINSUB2028"]);
                                }

                                //pre2021 = pre2021 + Convert.ToDouble(e.NewValues["FIN2021"]);
                                //if (pre2021 > programa2021)
                                //{
                                //    mensajesfinanciero = mensajesfinanciero + " 2021 ";
                                //    fallosfinanciero++;
                                //}
                                //else
                                //{
                                poas = obtienepoa(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), 2028);
                                if (metas.Rows[i]["IDFINSUB2028"] == DBNull.Value)
                                {
                                    //if (Convert.ToDouble(e.NewValues["MFINSUB2023"]) != 0)
                                    if (e.NewValues["MFINSUB2028"] != DBNull.Value)
                                        estado = insertaFinanciera(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MFINSUB2028"]));
                                }
                                else
                                {
                                    //if (Convert.ToDouble(e.NewValues["MFINSUB2023"]) != 0)
                                    if (e.NewValues["MFINSUB2028"] != DBNull.Value)
                                        estado = estado = Acualizametafinanciera(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MFINSUB2028"]), Convert.ToInt32(metas.Rows[i]["IDFINSUB2028"]));
                                    else
                                        estado = Eliminametafinanciera(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToInt32(metas.Rows[i]["IDFINSUB2028"]));
                                }
                                //}
                                //fin 2028




                                if (fallofisico > 0)
                                    mensaje = "Las metas de los años " + mensajesFisico + ", no pudieron ser guardadas por posibles fallos en el sistema, intententelo mas tarde o contacte al administrador";
                                if (fallosfinanciero > 0)
                                    mensaje = mensaje + " Las metas financieras de los años " + mensajesfinanciero + ", no pudieron ser guardadas al estar programando una cantidad mayor, a la cantidad asignada  a su programa, por favor revise las cantidades en los años descritos";
                                if (fallofisico == 0 & fallosfinanciero == 0)
                                    mensaje = "Metas guardadas correctamente";

                                grid.JSProperties["cpError"] = "Información " + mensaje;
                                e.Cancel = true;
                                grid.CancelEdit();
                                break;


                            }

                        }


                        //NUEVA PGG

                        if (Convert.ToInt32(Session["periodo"]) == 24)
                        {
                            if (metas.Rows[i]["SPPSUB$ID_SUBPRODUCTO"].ToString() == e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString())
                            {



                                if (programas.Rows[0]["PRESUPUESTO2025"] == DBNull.Value)
                                    programa2025 = 0;
                                else
                                    programa2025 = Convert.ToDouble(programas.Rows[0]["PRESUPUESTO2025"]);



                                if (programas.Rows[0]["PRESUPUESTO2026"] == DBNull.Value)
                                    programa2026 = 0;
                                else
                                    programa2026 = Convert.ToDouble(programas.Rows[0]["PRESUPUESTO2026"]);



                                if (programas.Rows[0]["PRESUPUESTO2027"] == DBNull.Value)
                                    programa2027 = 0;
                                else
                                    programa2027 = Convert.ToDouble(programas.Rows[0]["PRESUPUESTO2027"]);


                                if (programas.Rows[0]["PRESUPUESTO2028"] == DBNull.Value)
                                    programa2028 = 0;
                                else
                                    programa2028 = Convert.ToDouble(programas.Rows[0]["PRESUPUESTO2027"]);


                                if (programas.Rows[0]["PRESUPUESTO2029"] == DBNull.Value)
                                    programa2029 = 0;
                                else
                                    programa2029 = Convert.ToDouble(programas.Rows[0]["PRESUPUESTO2029"]);







                                if ((programas.Rows[0]["PR0GRAMADO2025"] == DBNull.Value))
                                    pre2025 = 0;
                                else
                                    pre2025 = Convert.ToDouble(programas.Rows[0]["PR0GRAMADO2025"]);

                                if ((programas.Rows[0]["PR0GRAMADO2026"] == DBNull.Value))
                                    pre2026 = 0;
                                else
                                    pre2025 = Convert.ToDouble(programas.Rows[0]["PR0GRAMADO2026"]);

                                if ((programas.Rows[0]["PR0GRAMADO2027"] == DBNull.Value))
                                    pre2027 = 0;
                                else
                                    pre2027 = Convert.ToDouble(programas.Rows[0]["PR0GRAMADO2027"]);


                                if (programas.Rows[0]["PR0GRAMADO2028"] == DBNull.Value)
                                    pre2028 = 0;
                                else
                                    pre2028 = Convert.ToDouble(programas.Rows[0]["PR0GRAMADO2028"]);


                                if ((programas.Rows[0]["PR0GRAMADO2029"] == DBNull.Value))
                                    pre2029 = 0;
                                else
                                    pre2029 = Convert.ToDouble(programas.Rows[0]["PR0GRAMADO2029"]);

                                //fin financiero








                                if ((programas.Rows[0]["PR0GRAMADO2025"] == DBNull.Value))
                                    pre2025 = 0;
                                else
                                    pre2025 = Convert.ToDouble(programas.Rows[0]["PR0GRAMADO2025"]);

                                if ((programas.Rows[0]["PR0GRAMADO2026"] == DBNull.Value))
                                    pre2026 = 0;
                                else
                                    pre2026 = Convert.ToDouble(programas.Rows[0]["PR0GRAMADO2026"]);

                                if ((programas.Rows[0]["PR0GRAMADO2027"] == DBNull.Value))
                                    pre2027 = 0;
                                else
                                    pre2027 = Convert.ToDouble(programas.Rows[0]["PR0GRAMADO2027"]);



                                if (programas.Rows[0]["PR0GRAMADO2028"] == DBNull.Value)
                                    pre2028 = 0;
                                else
                                    pre2028 = Convert.ToDouble(programas.Rows[0]["PR0GRAMADO2028"]);


                                if ((programas.Rows[0]["PR0GRAMADO2029"] == DBNull.Value))
                                    pre2029 = 0;
                                else
                                    pre2029 = Convert.ToDouble(programas.Rows[0]["PR0GRAMADO2029"]);




                                poas = obtienepoa(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), 2025);
                                //inicio 2025
                                if (metas.Rows[i]["IDFISUB2025"] == DBNull.Value)
                                {
                                    //if (Convert.ToDouble(e.NewValues["MFSUB2024"]) != 0)
                                    if (e.NewValues["MFSUB2025"] != DBNull.Value)
                                    {
                                        estado = insertaFisicasub(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MFSUB2025"]));
                                        if (estado == 0)
                                        {
                                            mensajesFisico = mensajesFisico + " 2025 ";
                                            fallofisico++;
                                        }
                                    }

                                }

                                else
                                {
                                    //if (Convert.ToDouble(e.NewValues["MFSUB2024"]) != 0)
                                    if (e.NewValues["MFSUB2025"] != DBNull.Value)
                                        estado = Acualizametafisicasub(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MFSUB2025"]), Convert.ToInt32(metas.Rows[i]["IDFISUB2025"]));
                                    else
                                        estado = Eliminafisicasub(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToInt32(metas.Rows[i]["IDFISUB2025"]));

                                    if (estado == 0)
                                    {
                                        mensajesFisico = mensajesFisico + " 2025 ";
                                        fallofisico++;
                                    }
                                }

                                //fin 2025

                                poas = obtienepoa(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), 2026);
                                //inicio 2026
                                if (metas.Rows[i]["IDFISUB2026"] == DBNull.Value)
                                {
                                    //if (Convert.ToDouble(e.NewValues["MFSUB2025"]) != 0)
                                    if (e.NewValues["MFSUB2026"] != DBNull.Value)
                                    {
                                        estado = insertaFisicasub(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MFSUB2026"]));
                                        if (estado == 0)
                                        {
                                            mensajesFisico = mensajesFisico + " 2026 ";
                                            fallofisico++;
                                        }
                                    }

                                }

                                else
                                {
                                    //if (Convert.ToDouble(e.NewValues["MFSUB2025"]) != 0)
                                    if (e.NewValues["MFSUB2026"] != DBNull.Value)

                                        estado = Acualizametafisicasub(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MFSUB2026"]), Convert.ToInt32(metas.Rows[i]["IDFISUB2026"]));
                                    else
                                        estado = Eliminafisicasub(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToInt32(metas.Rows[i]["IDFISUB2026"]));

                                    if (estado == 0)
                                    {
                                        mensajesFisico = mensajesFisico + " 2026 ";
                                        fallofisico++;
                                    }
                                }

                                //fin 2026

                                poas = obtienepoa(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), 2027);
                                //inicio 2027
                                if (metas.Rows[i]["IDFISUB2027"] == DBNull.Value)
                                {
                                    //if (Convert.ToDouble(e.NewValues["MFSUB2026"]) != 0)
                                    if (e.NewValues["MFSUB2027"] != DBNull.Value)
                                    {
                                        estado = insertaFisicasub(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MFSUB2027"]));
                                        if (estado == 0)
                                        {
                                            mensajesFisico = mensajesFisico + " 2027 ";
                                            fallofisico++;
                                        }
                                    }

                                }

                                else
                                {
                                    //if (Convert.ToDouble(e.NewValues["MFSUB2026"]) != 0)
                                    if (e.NewValues["MFSUB2027"] != DBNull.Value)


                                        estado = Acualizametafisicasub(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MFSUB2027"]), Convert.ToInt32(metas.Rows[i]["IDFISUB2027"]));
                                    else
                                        estado = Eliminafisicasub(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToInt32(metas.Rows[i]["IDFISUB2027"]));

                                    if (estado == 0)
                                    {
                                        mensaje = mensaje + " 2027 ";
                                        fallofisico++;
                                    }
                                }

                                //fin 2027

                                poas = obtienepoa(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), 2028);
                                //inicio 2028
                                if (metas.Rows[i]["IDFISUB2028"] == DBNull.Value)
                                {
                                    //if (Convert.ToDouble(e.NewValues["MFSUB2022"]) != 0)
                                    if (e.NewValues["MFSUB2028"] != DBNull.Value)
                                    {
                                        estado = insertaFisicasub(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MFSUB2028"]));
                                        if (estado == 0)
                                        {
                                            mensajesFisico = mensajesFisico + " 2028 ";
                                            fallofisico++;
                                        }
                                    }


                                }

                                else
                                {
                                    //if (Convert.ToDouble(e.NewValues["MFSUB2022"]) != 0)
                                    if (e.NewValues["MFSUB2028"] != DBNull.Value)

                                        estado = Acualizametafisicasub(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MFSUB2028"]), Convert.ToInt32(metas.Rows[i]["IDFISUB2028"]));
                                    else
                                        estado = Eliminafisicasub(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToInt32(metas.Rows[i]["IDFISUB2028"]));

                                    if (estado == 0)
                                    {
                                        mensajesFisico = mensajesFisico + " 2028 ";
                                        fallofisico++;
                                    }
                                }

                                //fin 2028




                                poas = obtienepoa(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), 2029);
                                //inicio 2028
                                if (metas.Rows[i]["IDFISUB2029"] == DBNull.Value)
                                {
                                    //if (Convert.ToDouble(e.NewValues["MFSUB2023"]) != 0)
                                    if (e.NewValues["MFSUB2029"] != DBNull.Value)
                                    {
                                        estado = insertaFisicasub(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MFSUB2029"]));
                                        if (estado == 0)
                                        {
                                            mensajesFisico = mensajesFisico + " 2029 ";
                                            fallofisico++;
                                        }
                                    }

                                }

                                else
                                {
                                    //if (Convert.ToDouble(e.NewValues["MFSUB2023"]) != 0)
                                    if (e.NewValues["MFSUB2029"] != DBNull.Value)
                                        estado = Acualizametafisicasub(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MFSUB2029"]), Convert.ToInt32(metas.Rows[i]["IDFISUB2029"]));
                                    else
                                        estado = Eliminafisicasub(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToInt32(metas.Rows[i]["IDFISUB2029"]));


                                    if (estado == 0)
                                    {
                                        mensajesFisico = mensajesFisico + " 2029 ";
                                        fallofisico++;
                                    }
                                }

                                //fin 2029





                                //inicio 2024
                                if (metas.Rows[i]["MFSUB2025"] == DBNull.Value || e.NewValues["MFSUB20225"] == DBNull.Value)
                                    pre2025 = 0;
                                else if ((metas.Rows[i]["MFSUB2025"] != DBNull.Value && e.NewValues["MFSUB2025"] != DBNull.Value) && (Convert.ToDouble(metas.Rows[i]["MFSUB2025"]) == Convert.ToDouble(e.NewValues["MFSUB2025"])))
                                    pre2025 = 0;
                                else if (Convert.ToDouble(metas.Rows[i]["MFSUB2025"]) > Convert.ToDouble(e.NewValues["MFSUB2025"]))
                                {
                                    pre2025 = pre2025 + Convert.ToDouble(e.NewValues["MFSUB2025"]);
                                    pre2025 = Convert.ToDouble(e.NewValues["MFSUB2025"]) - Convert.ToDouble(metas.Rows[i]["MFSUB2025"]);
                                    pre2025 = pre2025 + Convert.ToDouble(metas.Rows[i]["MFSUB2025"]);
                                }

                                else
                                {
                                    pre2025 = Convert.ToDouble(e.NewValues["MFSUB2025"]) - Convert.ToDouble(metas.Rows[i]["MFSUB2025"]);
                                    pre2025 = pre2025 + Convert.ToDouble(metas.Rows[i]["MFSUB2025"]);
                                }
                                //pre2022 = pre2022 + Convert.ToDouble(e.NewValues["FIN2022"]);
                                //if (pre2023 > programa2023)
                                //{
                                //    mensajesfinanciero = mensajesfinanciero + " 2023 ";
                                //    fallosfinanciero++;

                                //}
                                //else
                                //{
                                poas = obtienepoa(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), 2025);
                                if (metas.Rows[i]["IDFINSUB2025"] == DBNull.Value)
                                {
                                    //if (Convert.ToDouble(e.NewValues["MFINSUB2024"]) != 0)
                                    if (e.NewValues["MFINSUB2025"] != DBNull.Value)
                                        estado = insertaFinanciera(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MFINSUB2025"]));
                                }
                                else
                                {
                                    //if (Convert.ToDouble(e.NewValues["MFINSUB2024"]) != 0)
                                    if (e.NewValues["MFINSUB2025"] != DBNull.Value)
                                        estado = Acualizametafinanciera(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MFINSUB2025"]), Convert.ToInt32(metas.Rows[i]["IDFINSUB2025"]));
                                    else
                                        estado = Eliminametafinanciera(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToInt32(metas.Rows[i]["IDFINSUB2025"]));
                                }
                                //}
                                //fin 2024

                                //inicio 2025
                                if (metas.Rows[i]["MFINSUB2026"] == DBNull.Value || e.NewValues["MFINSUB2026"] == DBNull.Value)
                                    pre2026 = 0;
                                else if ((metas.Rows[i]["MFINSUB2026"] != DBNull.Value && e.NewValues["MFINSUB2026"] != DBNull.Value) && (Convert.ToDouble(metas.Rows[i]["MFINSUB2026"]) == Convert.ToDouble(e.NewValues["MFINSUB2026"])))
                                    pre2026 = 0;
                                else if (Convert.ToDouble(metas.Rows[i]["MFINSUB2026"]) > Convert.ToDouble(e.NewValues["MFINSUB2026"]))
                                {
                                    pre2026 = pre2026 + Convert.ToDouble(e.NewValues["MFINSUB2026"]);
                                    pre2026 = Convert.ToDouble(e.NewValues["MFINSUB2026"]) - Convert.ToDouble(metas.Rows[i]["MFINSUB2026"]);
                                    pre2026 = pre2026 + Convert.ToDouble(metas.Rows[i]["MFINSUB2026"]);
                                }

                                else
                                {
                                    pre2026 = Convert.ToDouble(e.NewValues["MFINSUB2026"]) - Convert.ToDouble(metas.Rows[i]["MFINSUB2026"]);
                                    pre2026 = pre2026 + Convert.ToDouble(metas.Rows[i]["MFINSUB2026"]);
                                }
                                //pre2023 = pre2023 + Convert.ToDouble(e.NewValues["FIN2023"]);
                                //if (pre2023 > programa2023)
                                //{
                                //    mensajesfinanciero = mensajesfinanciero + " 2023 ";
                                //    fallosfinanciero++;

                                //}
                                //else
                                //{
                                poas = obtienepoa(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), 2026);
                                if (metas.Rows[i]["IDFINSUB2026"] == DBNull.Value)
                                {
                                    //if (Convert.ToDouble(e.NewValues["MFINSUB2025"]) != 0)
                                    if (e.NewValues["MFINSUB2026"] != DBNull.Value)
                                        estado = insertaFinanciera(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MFINSUB2026"]));
                                }
                                else
                                {
                                    //if (Convert.ToDouble(e.NewValues["MFINSUB2025"]) != 0)
                                    if (e.NewValues["MFINSUB2026"] != DBNull.Value)
                                        estado = Acualizametafinanciera(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MFINSUB2026"]), Convert.ToInt32(metas.Rows[i]["IDFINSUB2026"]));
                                    else
                                        estado = Eliminametafinanciera(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToInt32(metas.Rows[i]["IDFINSUB2026"]));
                                }
                                //}
                                //fin 2025

                                //inicio 2026
                                if (metas.Rows[i]["MFINSUB2027"] == DBNull.Value || e.NewValues["MFINSUB2027"] == DBNull.Value)
                                    pre2027 = 0;
                                else if ((metas.Rows[i]["MFINSUB2027"] != DBNull.Value && e.NewValues["MFINSUB2027"] != DBNull.Value) && (Convert.ToDouble(metas.Rows[i]["MFINSUB2027"]) == Convert.ToDouble(e.NewValues["MFINSUB2027"])))
                                    pre2027 = 0;
                                else if (Convert.ToDouble(metas.Rows[i]["MFINSUB2027"]) > Convert.ToDouble(e.NewValues["MFINSUB2027"]))
                                {
                                    pre2027 = pre2027 + Convert.ToDouble(e.NewValues["MFINSUB2027"]);
                                    pre2027 = Convert.ToDouble(e.NewValues["MFINSUB2027"]) - Convert.ToDouble(metas.Rows[i]["MFINSUB2027"]);
                                    pre2027 = pre2027 + Convert.ToDouble(metas.Rows[i]["MFINSUB2027"]);
                                }

                                else
                                {
                                    pre2027 = Convert.ToDouble(e.NewValues["MFINSUB2027"]) - Convert.ToDouble(metas.Rows[i]["MFINSUB2027"]);
                                    pre2027 = pre2027 + Convert.ToDouble(metas.Rows[i]["MFINSUB2027"]);
                                }
                                //pre2024 = pre2024 + Convert.ToDouble(e.NewValues["FIN2024"]);
                                //if (pre2024 > programa2024)
                                //{
                                //    mensajesfinanciero = mensajesfinanciero + " 2024 ";
                                //    fallosfinanciero++;

                                //}
                                //else
                                //{
                                poas = obtienepoa(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), 2027);
                                if (metas.Rows[i]["IDFINSUB2027"] == DBNull.Value)
                                {
                                    //if (Convert.ToDouble(e.NewValues["MFINSUB2026"]) != 0)
                                    if (e.NewValues["MFINSUB2027"] != DBNull.Value)
                                        estado = insertaFinanciera(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MFINSUB2027"]));
                                }
                                else
                                {
                                    //if (Convert.ToDouble(e.NewValues["MFINSUB2026"]) != 0)
                                    if (e.NewValues["MFINSUB2027"] != DBNull.Value)
                                        estado = Acualizametafinanciera(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MFINSUB2027"]), Convert.ToInt32(metas.Rows[i]["IDFINSUB2027"]));
                                    else
                                        estado = Eliminametafinanciera(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToInt32(metas.Rows[i]["IDFINSUB2027"]));
                                }
                                //}



                                //inicio 2027

                                if (metas.Rows[i]["MFINSUB2028"] == DBNull.Value || e.NewValues["MFINSUB2027"] == DBNull.Value)
                                    pre2028 = 0;
                                else if ((metas.Rows[i]["MFINSUB2028"] != DBNull.Value && e.NewValues["MFINSUB2027"] != DBNull.Value) && (Convert.ToDouble(metas.Rows[i]["MFINSUB2028"]) == Convert.ToDouble(e.NewValues["MFINSUB2028"])))
                                    pre2028 = 0;
                                else if (Convert.ToDouble(metas.Rows[i]["MFINSUB2028"]) > Convert.ToDouble(e.NewValues["MFINSUB2028"]))
                                {
                                    pre2028 = pre2028 + Convert.ToDouble(e.NewValues["MFINSUB2028"]);
                                    pre2028 = Convert.ToDouble(e.NewValues["MFINSUB2028"]) - Convert.ToDouble(metas.Rows[i]["MFINSUB2028"]);
                                    pre2028 = pre2028 + Convert.ToDouble(metas.Rows[i]["MFINSUB2028"]);
                                }

                                else
                                {
                                    pre2028 = Convert.ToDouble(e.NewValues["MFINSUB2028"]) - Convert.ToDouble(metas.Rows[i]["MFINSUB2028"]);
                                    pre2028 = pre2028 + Convert.ToDouble(metas.Rows[i]["MFINSUB2028"]);
                                }

                                //if (pre2021 > programa2021)
                                //{
                                //    mensajesfinanciero = mensajesfinanciero + " 2021 ";
                                //    //fallosfinanciero++;
                                //}
                                //else
                                //{
                                poas = obtienepoa(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), 2028);
                                if (metas.Rows[i]["IDFINSUB2028"] == DBNull.Value)
                                {
                                    //if (Convert.ToDouble(e.NewValues["MFINSUB2022"]) != 0)
                                    if (e.NewValues["MFINSUB2028"] != DBNull.Value)
                                        estado = insertaFinanciera(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MFINSUB2028"]));
                                }
                                else
                                {
                                    //if (Convert.ToDouble(e.NewValues["MFINSUB2022"]) != 0)
                                    if (e.NewValues["MFINSUB2028"] != DBNull.Value)
                                        estado = Acualizametafinanciera(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MFINSUB2028"]), Convert.ToInt32(metas.Rows[i]["IDFINSUB2028"]));
                                    else
                                        estado = Eliminametafinanciera(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToInt32(metas.Rows[i]["IDFINSUB2028"]));
                                }
                                //}
                                //fin 2027




                                //inicio 2028
                                if (metas.Rows[i]["MFINSUB2029"] == DBNull.Value || e.NewValues["MFINSUB2029"] == DBNull.Value)
                                    pre2029 = 0;
                                else if ((metas.Rows[i]["MFINSUB2029"] != DBNull.Value && e.NewValues["MFINSUB2029"] != DBNull.Value) && (Convert.ToDouble(metas.Rows[i]["MFINSUB2029"]) == Convert.ToDouble(e.NewValues["MFINSUB2029"])))
                                    pre2029 = 0;
                                else if (Convert.ToDouble(metas.Rows[i]["MFINSUB2029"]) > Convert.ToDouble(e.NewValues["MFINSUB2029"]))
                                {
                                    pre2029 = pre2029 + Convert.ToDouble(e.NewValues["MFINSUB2029"]);
                                    pre2029 = Convert.ToDouble(e.NewValues["MFINSUB2029"]) - Convert.ToDouble(metas.Rows[i]["MFINSUB2029"]);
                                    pre2029 = pre2029 + Convert.ToDouble(metas.Rows[i]["MFINSUB2029"]);
                                }

                                else
                                {
                                    pre2029 = Convert.ToDouble(e.NewValues["MFINSUB2029"]) - Convert.ToDouble(metas.Rows[i]["MFINSUB2029"]);
                                    pre2029 = pre2029 + Convert.ToDouble(metas.Rows[i]["MFINSUB2029"]);
                                }

                                //pre2021 = pre2021 + Convert.ToDouble(e.NewValues["FIN2021"]);
                                //if (pre2021 > programa2021)
                                //{
                                //    mensajesfinanciero = mensajesfinanciero + " 2021 ";
                                //    fallosfinanciero++;
                                //}
                                //else
                                //{
                                poas = obtienepoa(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), 2029);
                                if (metas.Rows[i]["IDFINSUB2029"] == DBNull.Value)
                                {
                                    //if (Convert.ToDouble(e.NewValues["MFINSUB2023"]) != 0)
                                    if (e.NewValues["MFINSUB2029"] != DBNull.Value)
                                        estado = insertaFinanciera(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MFINSUB2029"]));
                                }
                                else
                                {
                                    //if (Convert.ToDouble(e.NewValues["MFINSUB2023"]) != 0)
                                    if (e.NewValues["MFINSUB2029"] != DBNull.Value)
                                        estado = estado = Acualizametafinanciera(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MFINSUB2029"]), Convert.ToInt32(metas.Rows[i]["IDFINSUB2029"]));
                                    else
                                        estado = Eliminametafinanciera(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToInt32(metas.Rows[i]["IDFINSUB2029"]));
                                }
                                //}
                                //fin 2028




                                if (fallofisico > 0)
                                    mensaje = "Las metas de los años " + mensajesFisico + ", no pudieron ser guardadas por posibles fallos en el sistema, intententelo mas tarde o contacte al administrador";
                                if (fallosfinanciero > 0)
                                    mensaje = mensaje + " Las metas financieras de los años " + mensajesfinanciero + ", no pudieron ser guardadas al estar programando una cantidad mayor, a la cantidad asignada  a su programa, por favor revise las cantidades en los años descritos";
                                if (fallofisico == 0 & fallosfinanciero == 0)
                                    mensaje = "Metas guardadas correctamente";

                                grid.JSProperties["cpError"] = "Información " + mensaje;
                                e.Cancel = true;
                                grid.CancelEdit();
                                break;


                            }

                        }
                        //NUEVA PGG

                        //INICIO 2026-2030
                        if (Convert.ToInt32(Session["periodo"]) == 25)
                        {
                            if (metas.Rows[i]["SPPSUB$ID_SUBPRODUCTO"].ToString() == e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString())
                            {

                                
                                if (programas.Rows[0]["PRESUPUESTO2026"] == DBNull.Value)
                                    programa2026 = 0;
                                else
                                    programa2026 = Convert.ToDouble(programas.Rows[0]["PRESUPUESTO2026"]);



                                if (programas.Rows[0]["PRESUPUESTO2027"] == DBNull.Value)
                                    programa2027 = 0;
                                else
                                    programa2027 = Convert.ToDouble(programas.Rows[0]["PRESUPUESTO2027"]);


                                if (programas.Rows[0]["PRESUPUESTO2028"] == DBNull.Value)
                                    programa2028 = 0;
                                else
                                    programa2028 = Convert.ToDouble(programas.Rows[0]["PRESUPUESTO2027"]);


                                if (programas.Rows[0]["PRESUPUESTO2029"] == DBNull.Value)
                                    programa2029 = 0;
                                else
                                    programa2029 = Convert.ToDouble(programas.Rows[0]["PRESUPUESTO2029"]);



                                if (programas.Rows[0]["PRESUPUESTO2030"] == DBNull.Value)
                                    programa2030 = 0;
                                else
                                    programa2030 = Convert.ToDouble(programas.Rows[0]["PRESUPUESTO2030"]);



                                

                                if ((programas.Rows[0]["PR0GRAMADO2026"] == DBNull.Value))
                                    pre2026 = 0;
                                else
                                    pre2026 = Convert.ToDouble(programas.Rows[0]["PR0GRAMADO2026"]);

                                if ((programas.Rows[0]["PR0GRAMADO2027"] == DBNull.Value))
                                    pre2027 = 0;
                                else
                                    pre2027 = Convert.ToDouble(programas.Rows[0]["PR0GRAMADO2027"]);


                                if (programas.Rows[0]["PR0GRAMADO2028"] == DBNull.Value)
                                    pre2028 = 0;
                                else
                                    pre2028 = Convert.ToDouble(programas.Rows[0]["PR0GRAMADO2028"]);


                                if ((programas.Rows[0]["PR0GRAMADO2029"] == DBNull.Value))
                                    pre2029 = 0;
                                else
                                    pre2029 = Convert.ToDouble(programas.Rows[0]["PR0GRAMADO2029"]);


                                if ((programas.Rows[0]["PR0GRAMADO2030"] == DBNull.Value))
                                    pre2030 = 0;
                                else
                                    pre2030 = Convert.ToDouble(programas.Rows[0]["PR0GRAMADO2030"]);

                                //fin financiero








                               

                                if ((programas.Rows[0]["PR0GRAMADO2026"] == DBNull.Value))
                                    pre2026 = 0;
                                else
                                    pre2026 = Convert.ToDouble(programas.Rows[0]["PR0GRAMADO2026"]);

                                if ((programas.Rows[0]["PR0GRAMADO2027"] == DBNull.Value))
                                    pre2027 = 0;
                                else
                                    pre2027 = Convert.ToDouble(programas.Rows[0]["PR0GRAMADO2027"]);



                                if (programas.Rows[0]["PR0GRAMADO2028"] == DBNull.Value)
                                    pre2028 = 0;
                                else
                                    pre2028 = Convert.ToDouble(programas.Rows[0]["PR0GRAMADO2028"]);


                                if ((programas.Rows[0]["PR0GRAMADO2029"] == DBNull.Value))
                                    pre2029 = 0;
                                else
                                    pre2029 = Convert.ToDouble(programas.Rows[0]["PR0GRAMADO2029"]);


                                if ((programas.Rows[0]["PR0GRAMADO2030"] == DBNull.Value))
                                    pre2030 = 0;
                                else
                                    pre2030 = Convert.ToDouble(programas.Rows[0]["PR0GRAMADO2030"]);

                                                                

                                

                                poas = obtienepoa(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), 2026);
                                //inicio 2026
                                if (metas.Rows[i]["IDFISUB2026"] == DBNull.Value)
                                {
                                    //if (Convert.ToDouble(e.NewValues["MFSUB2025"]) != 0)
                                    if (e.NewValues["MFSUB2026"] != DBNull.Value)
                                    {
                                        estado = insertaFisicasub(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MFSUB2026"]));
                                        if (estado == 0)
                                        {
                                            mensajesFisico = mensajesFisico + " 2026 ";
                                            fallofisico++;
                                        }
                                    }

                                }

                                else
                                {
                                    //if (Convert.ToDouble(e.NewValues["MFSUB2025"]) != 0)
                                    if (e.NewValues["MFSUB2026"] != DBNull.Value)

                                        estado = Acualizametafisicasub(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MFSUB2026"]), Convert.ToInt32(metas.Rows[i]["IDFISUB2026"]));
                                    else
                                        estado = Eliminafisicasub(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToInt32(metas.Rows[i]["IDFISUB2026"]));

                                    if (estado == 0)
                                    {
                                        mensajesFisico = mensajesFisico + " 2026 ";
                                        fallofisico++;
                                    }
                                }

                                //fin 2026

                                poas = obtienepoa(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), 2027);
                                //inicio 2027
                                if (metas.Rows[i]["IDFISUB2027"] == DBNull.Value)
                                {
                                    //if (Convert.ToDouble(e.NewValues["MFSUB2026"]) != 0)
                                    if (e.NewValues["MFSUB2027"] != DBNull.Value)
                                    {
                                        estado = insertaFisicasub(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MFSUB2027"]));
                                        if (estado == 0)
                                        {
                                            mensajesFisico = mensajesFisico + " 2027 ";
                                            fallofisico++;
                                        }
                                    }

                                }

                                else
                                {
                                    //if (Convert.ToDouble(e.NewValues["MFSUB2026"]) != 0)
                                    if (e.NewValues["MFSUB2027"] != DBNull.Value)


                                        estado = Acualizametafisicasub(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MFSUB2027"]), Convert.ToInt32(metas.Rows[i]["IDFISUB2027"]));
                                    else
                                        estado = Eliminafisicasub(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToInt32(metas.Rows[i]["IDFISUB2027"]));

                                    if (estado == 0)
                                    {
                                        mensaje = mensaje + " 2027 ";
                                        fallofisico++;
                                    }
                                }

                                //fin 2027

                                poas = obtienepoa(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), 2028);
                                //inicio 2028
                                if (metas.Rows[i]["IDFISUB2028"] == DBNull.Value)
                                {
                                    //if (Convert.ToDouble(e.NewValues["MFSUB2022"]) != 0)
                                    if (e.NewValues["MFSUB2028"] != DBNull.Value)
                                    {
                                        estado = insertaFisicasub(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MFSUB2028"]));
                                        if (estado == 0)
                                        {
                                            mensajesFisico = mensajesFisico + " 2028 ";
                                            fallofisico++;
                                        }
                                    }


                                }

                                else
                                {
                                    //if (Convert.ToDouble(e.NewValues["MFSUB2022"]) != 0)
                                    if (e.NewValues["MFSUB2028"] != DBNull.Value)

                                        estado = Acualizametafisicasub(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MFSUB2028"]), Convert.ToInt32(metas.Rows[i]["IDFISUB2028"]));
                                    else
                                        estado = Eliminafisicasub(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToInt32(metas.Rows[i]["IDFISUB2028"]));

                                    if (estado == 0)
                                    {
                                        mensajesFisico = mensajesFisico + " 2028 ";
                                        fallofisico++;
                                    }
                                }

                                //fin 2028




                                poas = obtienepoa(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), 2029);
                                //inicio 2028
                                if (metas.Rows[i]["IDFISUB2029"] == DBNull.Value)
                                {
                                    //if (Convert.ToDouble(e.NewValues["MFSUB2023"]) != 0)
                                    if (e.NewValues["MFSUB2029"] != DBNull.Value)
                                    {
                                        estado = insertaFisicasub(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MFSUB2029"]));
                                        if (estado == 0)
                                        {
                                            mensajesFisico = mensajesFisico + " 2029 ";
                                            fallofisico++;
                                        }
                                    }

                                }

                                else
                                {
                                    //if (Convert.ToDouble(e.NewValues["MFSUB2023"]) != 0)
                                    if (e.NewValues["MFSUB2029"] != DBNull.Value)
                                        estado = Acualizametafisicasub(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MFSUB2029"]), Convert.ToInt32(metas.Rows[i]["IDFISUB2029"]));
                                    else
                                        estado = Eliminafisicasub(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToInt32(metas.Rows[i]["IDFISUB2029"]));


                                    if (estado == 0)
                                    {
                                        mensajesFisico = mensajesFisico + " 2029 ";
                                        fallofisico++;
                                    }
                                }

                                //fin 2029

                                poas = obtienepoa(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), 2030);
                                //inicio 2030
                                if (metas.Rows[i]["IDFISUB2030"] == DBNull.Value)
                                {
                                    //if (Convert.ToDouble(e.NewValues["MFSUB2024"]) != 0)
                                    if (e.NewValues["MFSUB2030"] != DBNull.Value)
                                    {
                                        estado = insertaFisicasub(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MFSUB2030"]));
                                        if (estado == 0)
                                        {
                                            mensajesFisico = mensajesFisico + " 2030 ";
                                            fallofisico++;
                                        }
                                    }

                                }

                                else
                                {
                                    //if (Convert.ToDouble(e.NewValues["MFSUB2024"]) != 0)
                                    if (e.NewValues["MFSUB2030"] != DBNull.Value)
                                        estado = Acualizametafisicasub(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MFSUB2030"]), Convert.ToInt32(metas.Rows[i]["IDFISUB2030"]));
                                    else
                                        estado = Eliminafisicasub(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToInt32(metas.Rows[i]["IDFISUB2030"]));

                                    if (estado == 0)
                                    {
                                        mensajesFisico = mensajesFisico + " 2030 ";
                                        fallofisico++;
                                    }
                                }



                                
                                //inicio 2026
                                if (metas.Rows[i]["MFINSUB2026"] == DBNull.Value || e.NewValues["MFINSUB2026"] == DBNull.Value)
                                    pre2026 = 0;
                                else if ((metas.Rows[i]["MFINSUB2026"] != DBNull.Value && e.NewValues["MFINSUB2026"] != DBNull.Value) && (Convert.ToDouble(metas.Rows[i]["MFINSUB2026"]) == Convert.ToDouble(e.NewValues["MFINSUB2026"])))
                                    pre2026 = 0;
                                else if (Convert.ToDouble(metas.Rows[i]["MFINSUB2026"]) > Convert.ToDouble(e.NewValues["MFINSUB2026"]))
                                {
                                    pre2026 = pre2026 + Convert.ToDouble(e.NewValues["MFINSUB2026"]);
                                    pre2026 = Convert.ToDouble(e.NewValues["MFINSUB2026"]) - Convert.ToDouble(metas.Rows[i]["MFINSUB2026"]);
                                    pre2026 = pre2026 + Convert.ToDouble(metas.Rows[i]["MFINSUB2026"]);
                                }

                                else
                                {
                                    pre2026 = Convert.ToDouble(e.NewValues["MFINSUB2026"]) - Convert.ToDouble(metas.Rows[i]["MFINSUB2026"]);
                                    pre2026 = pre2026 + Convert.ToDouble(metas.Rows[i]["MFINSUB2026"]);
                                }
                                //pre2023 = pre2023 + Convert.ToDouble(e.NewValues["FIN2023"]);
                                //if (pre2023 > programa2023)
                                //{
                                //    mensajesfinanciero = mensajesfinanciero + " 2023 ";
                                //    fallosfinanciero++;

                                //}
                                //else
                                //{
                                poas = obtienepoa(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), 2026);
                                if (metas.Rows[i]["IDFINSUB2026"] == DBNull.Value)
                                {
                                    //if (Convert.ToDouble(e.NewValues["MFINSUB2025"]) != 0)
                                    if (e.NewValues["MFINSUB2026"] != DBNull.Value)
                                        estado = insertaFinanciera(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MFINSUB2026"]));
                                }
                                else
                                {
                                    //if (Convert.ToDouble(e.NewValues["MFINSUB2025"]) != 0)
                                    if (e.NewValues["MFINSUB2026"] != DBNull.Value)
                                        estado = Acualizametafinanciera(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MFINSUB2026"]), Convert.ToInt32(metas.Rows[i]["IDFINSUB2026"]));
                                    else
                                        estado = Eliminametafinanciera(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToInt32(metas.Rows[i]["IDFINSUB2026"]));
                                }
                                //}
                                //fin 2025

                                //inicio 2026
                                if (metas.Rows[i]["MFINSUB2027"] == DBNull.Value || e.NewValues["MFINSUB2027"] == DBNull.Value)
                                    pre2027 = 0;
                                else if ((metas.Rows[i]["MFINSUB2027"] != DBNull.Value && e.NewValues["MFINSUB2027"] != DBNull.Value) && (Convert.ToDouble(metas.Rows[i]["MFINSUB2027"]) == Convert.ToDouble(e.NewValues["MFINSUB2027"])))
                                    pre2027 = 0;
                                else if (Convert.ToDouble(metas.Rows[i]["MFINSUB2027"]) > Convert.ToDouble(e.NewValues["MFINSUB2027"]))
                                {
                                    pre2027 = pre2027 + Convert.ToDouble(e.NewValues["MFINSUB2027"]);
                                    pre2027 = Convert.ToDouble(e.NewValues["MFINSUB2027"]) - Convert.ToDouble(metas.Rows[i]["MFINSUB2027"]);
                                    pre2027 = pre2027 + Convert.ToDouble(metas.Rows[i]["MFINSUB2027"]);
                                }

                                else
                                {
                                    pre2027 = Convert.ToDouble(e.NewValues["MFINSUB2027"]) - Convert.ToDouble(metas.Rows[i]["MFINSUB2027"]);
                                    pre2027 = pre2027 + Convert.ToDouble(metas.Rows[i]["MFINSUB2027"]);
                                }
                                //pre2024 = pre2024 + Convert.ToDouble(e.NewValues["FIN2024"]);
                                //if (pre2024 > programa2024)
                                //{
                                //    mensajesfinanciero = mensajesfinanciero + " 2024 ";
                                //    fallosfinanciero++;

                                //}
                                //else
                                //{
                                poas = obtienepoa(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), 2027);
                                if (metas.Rows[i]["IDFINSUB2027"] == DBNull.Value)
                                {
                                    //if (Convert.ToDouble(e.NewValues["MFINSUB2026"]) != 0)
                                    if (e.NewValues["MFINSUB2027"] != DBNull.Value)
                                        estado = insertaFinanciera(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MFINSUB2027"]));
                                }
                                else
                                {
                                    //if (Convert.ToDouble(e.NewValues["MFINSUB2026"]) != 0)
                                    if (e.NewValues["MFINSUB2027"] != DBNull.Value)
                                        estado = Acualizametafinanciera(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MFINSUB2027"]), Convert.ToInt32(metas.Rows[i]["IDFINSUB2027"]));
                                    else
                                        estado = Eliminametafinanciera(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToInt32(metas.Rows[i]["IDFINSUB2027"]));
                                }
                                //}



                                //inicio 2027

                                if (metas.Rows[i]["MFINSUB2028"] == DBNull.Value || e.NewValues["MFINSUB2027"] == DBNull.Value)
                                    pre2028 = 0;
                                else if ((metas.Rows[i]["MFINSUB2028"] != DBNull.Value && e.NewValues["MFINSUB2027"] != DBNull.Value) && (Convert.ToDouble(metas.Rows[i]["MFINSUB2028"]) == Convert.ToDouble(e.NewValues["MFINSUB2028"])))
                                    pre2028 = 0;
                                else if (Convert.ToDouble(metas.Rows[i]["MFINSUB2028"]) > Convert.ToDouble(e.NewValues["MFINSUB2028"]))
                                {
                                    pre2028 = pre2028 + Convert.ToDouble(e.NewValues["MFINSUB2028"]);
                                    pre2028 = Convert.ToDouble(e.NewValues["MFINSUB2028"]) - Convert.ToDouble(metas.Rows[i]["MFINSUB2028"]);
                                    pre2028 = pre2028 + Convert.ToDouble(metas.Rows[i]["MFINSUB2028"]);
                                }

                                else
                                {
                                    pre2028 = Convert.ToDouble(e.NewValues["MFINSUB2028"]) - Convert.ToDouble(metas.Rows[i]["MFINSUB2028"]);
                                    pre2028 = pre2028 + Convert.ToDouble(metas.Rows[i]["MFINSUB2028"]);
                                }

                                //if (pre2021 > programa2021)
                                //{
                                //    mensajesfinanciero = mensajesfinanciero + " 2021 ";
                                //    //fallosfinanciero++;
                                //}
                                //else
                                //{
                                poas = obtienepoa(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), 2028);
                                if (metas.Rows[i]["IDFINSUB2028"] == DBNull.Value)
                                {
                                    //if (Convert.ToDouble(e.NewValues["MFINSUB2022"]) != 0)
                                    if (e.NewValues["MFINSUB2028"] != DBNull.Value)
                                        estado = insertaFinanciera(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MFINSUB2028"]));
                                }
                                else
                                {
                                    //if (Convert.ToDouble(e.NewValues["MFINSUB2022"]) != 0)
                                    if (e.NewValues["MFINSUB2028"] != DBNull.Value)
                                        estado = Acualizametafinanciera(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MFINSUB2028"]), Convert.ToInt32(metas.Rows[i]["IDFINSUB2028"]));
                                    else
                                        estado = Eliminametafinanciera(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToInt32(metas.Rows[i]["IDFINSUB2028"]));
                                }
                                //}
                                //fin 2027




                                //inicio 2028
                                if (metas.Rows[i]["MFINSUB2029"] == DBNull.Value || e.NewValues["MFINSUB2029"] == DBNull.Value)
                                    pre2029 = 0;
                                else if ((metas.Rows[i]["MFINSUB2029"] != DBNull.Value && e.NewValues["MFINSUB2029"] != DBNull.Value) && (Convert.ToDouble(metas.Rows[i]["MFINSUB2029"]) == Convert.ToDouble(e.NewValues["MFINSUB2029"])))
                                    pre2029 = 0;
                                else if (Convert.ToDouble(metas.Rows[i]["MFINSUB2029"]) > Convert.ToDouble(e.NewValues["MFINSUB2029"]))
                                {
                                    pre2029 = pre2029 + Convert.ToDouble(e.NewValues["MFINSUB2029"]);
                                    pre2029 = Convert.ToDouble(e.NewValues["MFINSUB2029"]) - Convert.ToDouble(metas.Rows[i]["MFINSUB2029"]);
                                    pre2029 = pre2029 + Convert.ToDouble(metas.Rows[i]["MFINSUB2029"]);
                                }

                                else
                                {
                                    pre2029 = Convert.ToDouble(e.NewValues["MFINSUB2029"]) - Convert.ToDouble(metas.Rows[i]["MFINSUB2029"]);
                                    pre2029 = pre2029 + Convert.ToDouble(metas.Rows[i]["MFINSUB2029"]);
                                }

                                //pre2021 = pre2021 + Convert.ToDouble(e.NewValues["FIN2021"]);
                                //if (pre2021 > programa2021)
                                //{
                                //    mensajesfinanciero = mensajesfinanciero + " 2021 ";
                                //    fallosfinanciero++;
                                //}
                                //else
                                //{
                                poas = obtienepoa(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), 2029);
                                if (metas.Rows[i]["IDFINSUB2029"] == DBNull.Value)
                                {
                                    //if (Convert.ToDouble(e.NewValues["MFINSUB2023"]) != 0)
                                    if (e.NewValues["MFINSUB2029"] != DBNull.Value)
                                        estado = insertaFinanciera(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MFINSUB2029"]));
                                }
                                else
                                {
                                    //if (Convert.ToDouble(e.NewValues["MFINSUB2023"]) != 0)
                                    if (e.NewValues["MFINSUB2029"] != DBNull.Value)
                                        estado = estado = Acualizametafinanciera(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MFINSUB2029"]), Convert.ToInt32(metas.Rows[i]["IDFINSUB2029"]));
                                    else
                                        estado = Eliminametafinanciera(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToInt32(metas.Rows[i]["IDFINSUB2029"]));
                                }
                                //}
                                //fin 2028



                                //INICIO 2030
                                
                                if (metas.Rows[i]["MFSUB2030"] == DBNull.Value || e.NewValues["MFSUB2030"] == DBNull.Value)
                                    pre2030 = 0;
                                else if ((metas.Rows[i]["MFSUB2030"] != DBNull.Value && e.NewValues["MFSUB2030"] != DBNull.Value) && (Convert.ToDouble(metas.Rows[i]["MFSUB2030"]) == Convert.ToDouble(e.NewValues["MFSUB2030"])))
                                    pre2030 = 0;
                                else if (Convert.ToDouble(metas.Rows[i]["MFSUB2030"]) > Convert.ToDouble(e.NewValues["MFSUB2030"]))
                                {
                                    pre2030 = pre2030 + Convert.ToDouble(e.NewValues["MFSUB2030"]);
                                    pre2030 = Convert.ToDouble(e.NewValues["MFSUB2030"]) - Convert.ToDouble(metas.Rows[i]["MFSUB2030"]);
                                    pre2030 = pre2030 + Convert.ToDouble(metas.Rows[i]["MFSUB2030"]);
                                }

                                else
                                {
                                    pre2030 = Convert.ToDouble(e.NewValues["MFSUB2030"]) - Convert.ToDouble(metas.Rows[i]["MFSUB2030"]);
                                    pre2030 = pre2030 + Convert.ToDouble(metas.Rows[i]["MFSUB2030"]);
                                }

                                poas = obtienepoa(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), 2030);
                                if (metas.Rows[i]["IDFINSUB2030"] == DBNull.Value)
                                {
                                    //if (Convert.ToDouble(e.NewValues["MFINSUB2024"]) != 0)
                                    if (e.NewValues["MFINSUB2030"] != DBNull.Value)
                                        estado = insertaFinanciera(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MFINSUB2030"]));
                                }
                                else
                                {
                                    //if (Convert.ToDouble(e.NewValues["MFINSUB2024"]) != 0)
                                    if (e.NewValues["MFINSUB2030"] != DBNull.Value)
                                        estado = Acualizametafinanciera(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MFINSUB2030"]), Convert.ToInt32(metas.Rows[i]["IDFINSUB2030"]));
                                    else
                                        estado = Eliminametafinanciera(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToInt32(metas.Rows[i]["IDFINSUB2030"]));
                                }
                                
                               



                                //FIN 2030




                                if (fallofisico > 0)
                                    mensaje = "Las metas de los años " + mensajesFisico + ", no pudieron ser guardadas por posibles fallos en el sistema, intententelo mas tarde o contacte al administrador";
                                if (fallosfinanciero > 0)
                                    mensaje = mensaje + " Las metas financieras de los años " + mensajesfinanciero + ", no pudieron ser guardadas al estar programando una cantidad mayor, a la cantidad asignada  a su programa, por favor revise las cantidades en los años descritos";
                                if (fallofisico == 0 & fallosfinanciero == 0)
                                    mensaje = "Metas guardadas correctamente";

                                grid.JSProperties["cpError"] = "Información " + mensaje;
                                e.Cancel = true;
                                grid.CancelEdit();
                                break;


                            }

                        }

                        //FIN 2026-2030


                        else
                        {
                            if (metas.Rows[i]["SPPSUB$ID_SUBPRODUCTO"].ToString() == e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString())
                            {
                                if (programas.Rows[0]["PRESUPUESTO2020"] == DBNull.Value)
                                    programa2020 = 0;
                                else
                                    programa2020 = Convert.ToDouble(programas.Rows[0]["PRESUPUESTO2020"]);

                                if (programas.Rows[0]["PRESUPUESTO2021"] == DBNull.Value)
                                    programa2021 = 0;
                                else
                                    programa2021 = Convert.ToDouble(programas.Rows[0]["PRESUPUESTO2021"]);

                                if (programas.Rows[0]["PRESUPUESTO2022"] == DBNull.Value)
                                    programa2022 = 0;
                                else
                                    programa2022 = Convert.ToDouble(programas.Rows[0]["PRESUPUESTO2022"]);

                                if (programas.Rows[0]["PRESUPUESTO2023"] == DBNull.Value)
                                    programa2023 = 0;
                                else
                                    programa2023 = Convert.ToDouble(programas.Rows[0]["PRESUPUESTO2023"]);

                                if (programas.Rows[0]["PRESUPUESTO2024"] == DBNull.Value)
                                    programa2024 = 0;
                                else
                                    programa2024 = Convert.ToDouble(programas.Rows[0]["PRESUPUESTO2024"]);

                                //inicio 2020
                                if (programas.Rows[0]["PR0GRAMADO2020"] == DBNull.Value)
                                    pre2020 = 0;
                                else
                                    pre2020 = Convert.ToDouble(programas.Rows[0]["PR0GRAMADO2020"]);

                                if ((programas.Rows[0]["PR0GRAMADO2021"] == DBNull.Value))
                                    pre2021 = 0;
                                else
                                    pre2021 = Convert.ToDouble(programas.Rows[0]["PR0GRAMADO2021"]);

                                if ((programas.Rows[0]["PR0GRAMADO2022"] == DBNull.Value))
                                    pre2022 = 0;
                                else
                                    pre2022 = Convert.ToDouble(programas.Rows[0]["PR0GRAMADO2022"]);

                                if ((programas.Rows[0]["PR0GRAMADO2023"] == DBNull.Value))
                                    pre2023 = 0;
                                else
                                    pre2023 = Convert.ToDouble(programas.Rows[0]["PR0GRAMADO2023"]);

                                if ((programas.Rows[0]["PR0GRAMADO2024"] == DBNull.Value))
                                    pre2024 = 0;
                                else
                                    pre2024 = Convert.ToDouble(programas.Rows[0]["PR0GRAMADO2024"]);
                                //fin financiero

                                // inicio 2020
                                if (programas.Rows[0]["PR0GRAMADO2020"] == DBNull.Value)
                                    pre2020 = 0;
                                else
                                    pre2020 = Convert.ToDouble(programas.Rows[0]["PR0GRAMADO2020"]);

                                if ((programas.Rows[0]["PR0GRAMADO2021"] == DBNull.Value))
                                    pre2021 = 0;
                                else
                                    pre2021 = Convert.ToDouble(programas.Rows[0]["PR0GRAMADO2021"]);

                                if ((programas.Rows[0]["PR0GRAMADO2022"] == DBNull.Value))
                                    pre2022 = 0;
                                else
                                    pre2022 = Convert.ToDouble(programas.Rows[0]["PR0GRAMADO2022"]);

                                if ((programas.Rows[0]["PR0GRAMADO2023"] == DBNull.Value))
                                    pre2023 = 0;
                                else
                                    pre2023 = Convert.ToDouble(programas.Rows[0]["PR0GRAMADO2023"]);

                                if ((programas.Rows[0]["PR0GRAMADO2024"] == DBNull.Value))
                                    pre2024 = 0;
                                else
                                    pre2024 = Convert.ToDouble(programas.Rows[0]["PR0GRAMADO2024"]);

                                poas = obtienepoa(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), 2020);
                                //inicio 2020
                                if (metas.Rows[i]["IDFISUB2020"] == DBNull.Value)
                                {
                                    //if (Convert.ToDouble(e.NewValues["MFSUB2020"]) != 0)
                                    if (e.NewValues["MFSUB2020"] != DBNull.Value)
                                    {
                                        estado = insertaFisicasub(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MFSUB2020"]));
                                        if (estado == 0)
                                        {
                                            mensajesFisico = mensajesFisico + " 2020 ";
                                            fallofisico++;
                                        }
                                    }


                                }

                                else
                                {
                                    //if (Convert.ToDouble(e.NewValues["MFSUB2020"]) != 0)
                                    if (e.NewValues["MFSUB2020"] != DBNull.Value)

                                        estado = Acualizametafisicasub(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MFSUB2020"]), Convert.ToInt32(metas.Rows[i]["IDFISUB2020"]));
                                    else
                                        estado = Eliminafisicasub(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToInt32(metas.Rows[i]["IDFISUB2020"]));

                                    if (estado == 0)
                                    {
                                        mensajesFisico = mensajesFisico + " 2020 ";
                                        fallofisico++;
                                    }
                                }

                                //fin 2020

                                poas = obtienepoa(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), 2021);
                                //inicio 2021
                                if (metas.Rows[i]["IDFISUB2021"] == DBNull.Value)
                                {
                                    //if (Convert.ToDouble(e.NewValues["MFSUB2021"]) != 0)
                                    if (e.NewValues["MFSUB2021"] != DBNull.Value)
                                    {
                                        estado = insertaFisicasub(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MFSUB2021"]));
                                        if (estado == 0)
                                        {
                                            mensajesFisico = mensajesFisico + " 2021 ";
                                            fallofisico++;
                                        }
                                    }

                                }

                                else
                                {
                                    //if (Convert.ToDouble(e.NewValues["MFSUB2021"]) != 0)
                                    if (e.NewValues["MFSUB2021"] != DBNull.Value)

                                        estado = Acualizametafisicasub(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MFSUB2021"]), Convert.ToInt32(metas.Rows[i]["IDFISUB2021"]));
                                    else
                                        estado = Eliminafisicasub(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToInt32(metas.Rows[i]["IDFISUB2021"]));


                                    if (estado == 0)
                                    {
                                        mensajesFisico = mensajesFisico + " 2021 ";
                                        fallofisico++;
                                    }
                                }

                                //fin 2021

                                poas = obtienepoa(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), 2022);
                                //inicio 2022
                                if (metas.Rows[i]["IDFISUB2022"] == DBNull.Value)
                                {
                                    //if (Convert.ToDouble(e.NewValues["MFSUB2022"]) != 0)
                                    if (e.NewValues["MFSUB2022"] != DBNull.Value)
                                    {
                                        estado = insertaFisicasub(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MFSUB2022"]));
                                        if (estado == 0)
                                        {
                                            mensajesFisico = mensajesFisico + " 2022 ";
                                            fallofisico++;
                                        }
                                    }

                                }

                                else
                                {
                                    //if (Convert.ToDouble(e.NewValues["MFSUB2022"]) != 0)
                                    if (e.NewValues["MFSUB2022"] != DBNull.Value)
                                        estado = Acualizametafisicasub(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MFSUB2022"]), Convert.ToInt32(metas.Rows[i]["IDFISUB2022"]));
                                    else
                                        estado = Eliminafisicasub(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToInt32(metas.Rows[i]["IDFISUB2022"]));

                                    if (estado == 0)
                                    {
                                        mensajesFisico = mensajesFisico + " 2022 ";
                                        fallofisico++;
                                    }
                                }

                                //fin 2022

                                poas = obtienepoa(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), 2023);
                                //inicio 2023
                                if (metas.Rows[i]["IDFISUB2023"] == DBNull.Value)
                                {
                                    if (e.NewValues["MFSUB2023"] != DBNull.Value)
                                    //if (Convert.ToDouble(e.NewValues["MFSUB2023"]) != 0)
                                    {
                                        estado = insertaFisicasub(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MFSUB2023"]));
                                        if (estado == 0)
                                        {
                                            mensajesFisico = mensajesFisico + " 2023 ";
                                            fallofisico++;
                                        }
                                    }

                                }

                                else
                                {
                                    //if (Convert.ToDouble(e.NewValues["MFSUB2023"]) != 0)
                                    if (e.NewValues["MFSUB2023"] != DBNull.Value)
                                        estado = Acualizametafisicasub(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MFSUB2023"]), Convert.ToInt32(metas.Rows[i]["IDFISUB2023"]));
                                    else
                                        estado = Eliminafisicasub(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToInt32(metas.Rows[i]["IDFISUB2023"]));

                                    if (estado == 0)
                                    {
                                        mensajesFisico = mensajesFisico + " 2023 ";
                                        fallofisico++;
                                    }
                                }

                                //fin 2023

                                poas = obtienepoa(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), 2024);
                                //inicio 2024
                                if (metas.Rows[i]["IDFISUB2024"] == DBNull.Value)
                                {
                                    //if (Convert.ToDouble(e.NewValues["MFSUB2024"]) != 0)
                                    if (e.NewValues["MFSUB2024"] != DBNull.Value)
                                    {
                                        estado = insertaFisicasub(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MFSUB2024"]));
                                        if (estado == 0)
                                        {
                                            mensajesFisico = mensajesFisico + " 2024 ";
                                            fallofisico++;
                                        }
                                    }

                                }

                                else
                                {
                                    //if (Convert.ToDouble(e.NewValues["MFSUB2024"]) != 0)
                                    if (e.NewValues["MFSUB2024"] != DBNull.Value)

                                        estado = Acualizametafisicasub(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MFSUB2024"]), Convert.ToInt32(metas.Rows[i]["IDFISUB2024"]));
                                    else
                                        estado = Eliminafisicasub(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToInt32(metas.Rows[i]["IDFISUB2024"]));

                                    if (estado == 0)
                                    {
                                        mensaje = mensaje + " 2024 ";
                                        fallofisico++;
                                    }
                                }

                                //fin 2024
                                //inicio 2020

                                if (metas.Rows[i]["MFINSUB2020"] == DBNull.Value || e.NewValues["MFINSUB2020"] == DBNull.Value)
                                    pre2020 = 0;
                                else if ((metas.Rows[i]["MFINSUB2020"] != DBNull.Value && e.NewValues["MFINSUB2020"] != DBNull.Value) && (Convert.ToDouble(metas.Rows[i]["MFINSUB2020"]) == Convert.ToDouble(e.NewValues["MFINSUB2020"])))
                                    pre2020 = 0;
                                else if (Convert.ToDouble(metas.Rows[i]["MFINSUB2020"]) > Convert.ToDouble(e.NewValues["MFINSUB2020"]))
                                {
                                    pre2020 = pre2020 + Convert.ToDouble(e.NewValues["MFINSUB2020"]);
                                    pre2020 = Convert.ToDouble(e.NewValues["MFINSUB2020"]) - Convert.ToDouble(metas.Rows[i]["MFINSUB2020"]);
                                    pre2020 = pre2020 + Convert.ToDouble(metas.Rows[i]["MFINSUB2020"]);
                                }

                                else
                                {
                                    pre2020 = Convert.ToDouble(e.NewValues["MFINSUB2020"]) - Convert.ToDouble(metas.Rows[i]["MFINSUB2020"]);
                                    pre2020 = pre2020 + Convert.ToDouble(metas.Rows[i]["MFINSUB2020"]);
                                }

                                //if (pre2020 > programa2020)
                                //{
                                //    mensajesfinanciero = mensajesfinanciero + " 2020 ";
                                //    fallosfinanciero++;
                                //}
                                //else
                                //{
                                poas = obtienepoa(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), 2020);
                                if (metas.Rows[i]["IDFINSUB2020"] == DBNull.Value)
                                {
                                    //if (Convert.ToDouble(e.NewValues["MFINSUB2020"]) != 0)
                                    if (e.NewValues["MFINSUB2020"] != DBNull.Value)
                                        estado = insertaFinanciera(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MFINSUB2020"]));
                                }
                                else
                                {
                                    //if (Convert.ToDouble(e.NewValues["MFINSUB2020"]) != 0)
                                    if (e.NewValues["MFINSUB2020"] != DBNull.Value)
                                        estado = Acualizametafinanciera(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MFINSUB2020"]), Convert.ToInt32(metas.Rows[i]["IDFINSUB2020"]));
                                    else
                                        estado = Eliminametafinanciera(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToInt32(metas.Rows[i]["IDFINSUB2020"]));
                                }
                                //}
                                //fin 2020

                                //inicio 2021
                                if (metas.Rows[i]["MFINSUB2021"] == DBNull.Value || e.NewValues["MFINSUB2021"] == DBNull.Value)
                                    pre2021 = 0;
                                else if ((metas.Rows[i]["MFINSUB2021"] != DBNull.Value && e.NewValues["MFINSUB2021"] != DBNull.Value) && (Convert.ToDouble(metas.Rows[i]["MFINSUB2021"]) == Convert.ToDouble(e.NewValues["MFINSUB2021"])))
                                    pre2021 = 0;
                                else if (Convert.ToDouble(metas.Rows[i]["MFINSUB2021"]) > Convert.ToDouble(e.NewValues["MFINSUB2021"]))
                                {
                                    pre2021 = pre2021 + Convert.ToDouble(e.NewValues["MFINSUB2021"]);
                                    pre2021 = Convert.ToDouble(e.NewValues["MFINSUB2021"]) - Convert.ToDouble(metas.Rows[i]["MFINSUB2021"]);
                                    pre2021 = pre2021 + Convert.ToDouble(metas.Rows[i]["MFINSUB2021"]);
                                }

                                else
                                {
                                    pre2021 = Convert.ToDouble(e.NewValues["MFINSUB2021"]) - Convert.ToDouble(metas.Rows[i]["MFINSUB2021"]);
                                    pre2021 = pre2021 + Convert.ToDouble(metas.Rows[i]["MFINSUB2021"]);
                                }

                                //pre2021 = pre2021 + Convert.ToDouble(e.NewValues["FIN2021"]);
                                //if (pre2021 > programa2021)
                                //{
                                //    mensajesfinanciero = mensajesfinanciero + " 2021 ";
                                //    fallosfinanciero++;
                                //}
                                //else
                                //{
                                poas = obtienepoa(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), 2021);
                                if (metas.Rows[i]["IDFINSUB2021"] == DBNull.Value)
                                {
                                    //if (Convert.ToDouble(e.NewValues["MFINSUB2021"]) != 0)
                                    if (e.NewValues["MFINSUB2021"] != DBNull.Value)
                                        estado = insertaFinanciera(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MFINSUB2021"]));
                                }
                                else
                                {
                                    //if (Convert.ToDouble(e.NewValues["MFINSUB2021"]) != 0)
                                    if (e.NewValues["MFINSUB2021"] != DBNull.Value)
                                        estado = estado = Acualizametafinanciera(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MFINSUB2021"]), Convert.ToInt32(metas.Rows[i]["IDFINSUB2021"]));
                                    else
                                        estado = Eliminametafinanciera(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToInt32(metas.Rows[i]["IDFINSUB2021"]));
                                }
                                //}
                                //fin 2021


                                //inicio 2022
                                if (metas.Rows[i]["MFSUB2022"] == DBNull.Value || e.NewValues["MFSUB20222"] == DBNull.Value)
                                    pre2022 = 0;
                                else if ((metas.Rows[i]["MFSUB2022"] != DBNull.Value && e.NewValues["MFSUB2022"] != DBNull.Value) && (Convert.ToDouble(metas.Rows[i]["MFSUB2022"]) == Convert.ToDouble(e.NewValues["MFSUB2022"])))
                                    pre2022 = 0;
                                else if (Convert.ToDouble(metas.Rows[i]["MFSUB2022"]) > Convert.ToDouble(e.NewValues["MFSUB2022"]))
                                {
                                    pre2022 = pre2022 + Convert.ToDouble(e.NewValues["MFSUB2022"]);
                                    pre2022 = Convert.ToDouble(e.NewValues["MFSUB2022"]) - Convert.ToDouble(metas.Rows[i]["MFSUB2022"]);
                                    pre2022 = pre2022 + Convert.ToDouble(metas.Rows[i]["MFSUB2022"]);
                                }

                                else
                                {
                                    pre2022 = Convert.ToDouble(e.NewValues["MFSUB2022"]) - Convert.ToDouble(metas.Rows[i]["MFSUB2022"]);
                                    pre2022 = pre2022 + Convert.ToDouble(metas.Rows[i]["MFSUB2022"]);
                                }
                                //pre2022 = pre2022 + Convert.ToDouble(e.NewValues["FIN2022"]);
                                //if (pre2022 > programa2022)
                                //{
                                //    mensajesfinanciero = mensajesfinanciero + " 2022 ";
                                //    fallosfinanciero++;

                                //}
                                //else
                                //{
                                poas = obtienepoa(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), 2022);
                                if (metas.Rows[i]["IDFINSUB2022"] == DBNull.Value)
                                {
                                    //if (Convert.ToDouble(e.NewValues["MFINSUB2022"]) != 0)
                                    if (e.NewValues["MFINSUB2022"] != DBNull.Value)
                                        estado = insertaFinanciera(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MFINSUB2022"]));
                                }
                                else
                                {
                                    //if (Convert.ToDouble(e.NewValues["MFINSUB2022"]) != 0)
                                    if (e.NewValues["MFINSUB2022"] != DBNull.Value)
                                        estado = Acualizametafinanciera(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MFINSUB2022"]), Convert.ToInt32(metas.Rows[i]["IDFINSUB2022"]));
                                    else
                                        estado = Eliminametafinanciera(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToInt32(metas.Rows[i]["IDFINSUB2022"]));
                                }
                                //}
                                //fin 2022

                                //inicio 2023
                                if (metas.Rows[i]["MFINSUB2023"] == DBNull.Value || e.NewValues["MFINSUB2023"] == DBNull.Value)
                                    pre2023 = 0;
                                else if ((metas.Rows[i]["MFINSUB2023"] != DBNull.Value && e.NewValues["MFINSUB2023"] != DBNull.Value) && (Convert.ToDouble(metas.Rows[i]["MFINSUB2023"]) == Convert.ToDouble(e.NewValues["MFINSUB2023"])))
                                    pre2023 = 0;
                                else if (Convert.ToDouble(metas.Rows[i]["MFINSUB2023"]) > Convert.ToDouble(e.NewValues["MFINSUB2023"]))
                                {
                                    pre2023 = pre2023 + Convert.ToDouble(e.NewValues["MFINSUB2023"]);
                                    pre2023 = Convert.ToDouble(e.NewValues["MFINSUB2023"]) - Convert.ToDouble(metas.Rows[i]["MFINSUB2023"]);
                                    pre2023 = pre2023 + Convert.ToDouble(metas.Rows[i]["MFINSUB2023"]);
                                }

                                else
                                {
                                    pre2023 = Convert.ToDouble(e.NewValues["MFINSUB2023"]) - Convert.ToDouble(metas.Rows[i]["MFINSUB2023"]);
                                    pre2023 = pre2023 + Convert.ToDouble(metas.Rows[i]["MFINSUB2023"]);
                                }
                                //pre2023 = pre2023 + Convert.ToDouble(e.NewValues["FIN2023"]);
                                //if (pre2023 > programa2023)
                                //{
                                //    mensajesfinanciero = mensajesfinanciero + " 2023 ";
                                //    fallosfinanciero++;

                                //}
                                //else
                                //{
                                poas = obtienepoa(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), 2023);
                                if (metas.Rows[i]["IDFINSUB2023"] == DBNull.Value)
                                {
                                    //if (Convert.ToDouble(e.NewValues["MFINSUB2023"]) != 0)
                                    if (e.NewValues["MFINSUB2023"] != DBNull.Value)
                                        estado = insertaFinanciera(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MFINSUB2023"]));
                                }
                                else
                                {
                                    //if (Convert.ToDouble(e.NewValues["MFINSUB2023"]) != 0)
                                    if (e.NewValues["MFINSUB2023"] != DBNull.Value)
                                        estado = Acualizametafinanciera(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MFINSUB2023"]), Convert.ToInt32(metas.Rows[i]["IDFINSUB2023"]));
                                    else
                                        estado = Eliminametafinanciera(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToInt32(metas.Rows[i]["IDFINSUB2023"]));
                                }
                                //}
                                //fin 2023

                                //inicio 2024
                                if (metas.Rows[i]["MFINSUB2024"] == DBNull.Value || e.NewValues["MFINSUB2024"] == DBNull.Value)
                                    pre2024 = 0;
                                else if ((metas.Rows[i]["MFINSUB2024"] != DBNull.Value && e.NewValues["MFINSUB2024"] != DBNull.Value) && (Convert.ToDouble(metas.Rows[i]["MFINSUB2024"]) == Convert.ToDouble(e.NewValues["MFINSUB2024"])))
                                    pre2024 = 0;
                                else if (Convert.ToDouble(metas.Rows[i]["MFINSUB2024"]) > Convert.ToDouble(e.NewValues["MFINSUB2024"]))
                                {
                                    pre2024 = pre2024 + Convert.ToDouble(e.NewValues["MFINSUB2024"]);
                                    pre2024 = Convert.ToDouble(e.NewValues["MFINSUB2024"]) - Convert.ToDouble(metas.Rows[i]["MFINSUB2024"]);
                                    pre2024 = pre2024 + Convert.ToDouble(metas.Rows[i]["MFINSUB2024"]);
                                }

                                else
                                {
                                    pre2024 = Convert.ToDouble(e.NewValues["MFINSUB2024"]) - Convert.ToDouble(metas.Rows[i]["MFINSUB2024"]);
                                    pre2024 = pre2024 + Convert.ToDouble(metas.Rows[i]["MFINSUB2024"]);
                                }
                                //pre2024 = pre2024 + Convert.ToDouble(e.NewValues["FIN2024"]);
                                //if (pre2024 > programa2024)
                                //{
                                //    mensajesfinanciero = mensajesfinanciero + " 2024 ";
                                //    fallosfinanciero++;

                                //}
                                //else
                                //{
                                poas = obtienepoa(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), 2024);
                                if (metas.Rows[i]["IDFINSUB2024"] == DBNull.Value)
                                {
                                    //if (Convert.ToDouble(e.NewValues["MFINSUB2024"]) != 0)
                                    if (e.NewValues["MFINSUB2024"] != DBNull.Value)
                                        estado = insertaFinanciera(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MFINSUB2024"]));
                                }
                                else
                                {
                                    //if (Convert.ToDouble(e.NewValues["MFINSUB2024"]) != 0)
                                    if (e.NewValues["MFINSUB2024"] != DBNull.Value)
                                        estado = Acualizametafinanciera(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToDouble(e.NewValues["MFINSUB2024"]), Convert.ToInt32(metas.Rows[i]["IDFINSUB2024"]));
                                    else
                                        estado = Eliminametafinanciera(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString()), Convert.ToInt32(poas.Rows[0]["SPOA$ID_POA"]), Convert.ToInt32(poas.Rows[0]["SPOA$ANIO"]), Convert.ToInt32(metas.Rows[i]["IDFINSUB2024"]));
                                }
                                //}

                                if (fallofisico > 0)
                                    mensaje = "Las metas de los años " + mensajesFisico + ", no pudieron ser guardadas por posibles fallos en el sistema, intententelo mas tarde o contacte al administrador";
                                if (fallosfinanciero > 0)
                                    mensaje = mensaje + " Las metas financieras de los años " + mensajesfinanciero + ", no pudieron ser guardadas al estar programando una cantidad mayor, a la cantidad asignada  a su programa, por favor revise las cantidades en los años descritos";
                                if (fallofisico == 0 & fallosfinanciero == 0)
                                    mensaje = "Metas guardadas correctamente";

                                grid.JSProperties["cpError"] = "Información " + mensaje;
                                e.Cancel = true;
                                grid.CancelEdit();
                                break;


                            }


                        }


                    }
                }
                else
                {
                    grid.JSProperties["cpError"] = "Error en las sesión";
                    e.Cancel = true;
                    grid.CancelEdit();
                }

            }
            else
            {
                grid.JSProperties["cpError"] = "Error en las sesión";
                e.Cancel = true;
                grid.CancelEdit();
            }

        }

        protected void btnPOA_Click(object sender, EventArgs e)
        {
            Session["carga"] = 2;
            cargaComboPOAS();
            cargaProduccionPOA(Convert.ToInt32(cbAniPOA.Text), Convert.ToInt32(cbTipoProd.Value));
            MultiView1.ActiveViewIndex = 2;

        }

        protected void cargaComboPOAS()
        {
            Session["carga"] = 2;
            DataTable POA = new DataTable();
            if (Session["poa"] != null)
            {
                POA = (DataTable)Session["poa"];
                if (POA.Rows.Count > 0)
                {
                    cbAniPOA.DataSource = POA;
                    cbAniPOA.ValueField = "SPOA$ID_POA";
                    cbAniPOA.TextField = "SPOA$ANIO";
                    cbAniPOA.DataBind();
                    cbAniPOA.SelectedIndex = 0;
                }
            }
        }

        protected void cbTipoProd_ValueChanged(object sender, EventArgs e)
        {
            Session["carga"] = 2;
            ScriptManager.RegisterStartupScript(this.upNS, GetType(), "script", "onCollapseClick2();", true);
            cargaProduccionPOA(Convert.ToInt32(cbAniPOA.Text), Convert.ToInt32(cbTipoProd.Value));

        }

        protected void cbAniPOA_ValueChanged(object sender, EventArgs e)
        {
            Session["carga"] = 2;
            cargaProduccionPOA(Convert.ToInt32(cbAniPOA.Text), Convert.ToInt32(cbTipoProd.Value));
        }

        protected void gvPOAproductos_BeforePerformDataSelect(object sender, EventArgs e)
        {
            Session["carga"] = 2;
            ASPxGridView gvDetail = sender as ASPxGridView;
            DataTable productos = new DataTable();
            int resultado, resultado2;
            Double programa;

            resultado = Convert.ToInt32((sender as ASPxGridView).GetMasterRowFieldValues("ID_RESULTADO"));
            resultado2 = Convert.ToInt32((sender as ASPxGridView).GetMasterRowFieldValues("SPPRO$RESULTADO2"));
            programa = Convert.ToDouble((sender as ASPxGridView).GetMasterRowFieldValues("SPPRO$ID_PROGRAMA_PRESUPUESTO"));
            Session["IDRESULTADO"] = resultado;
            Session["IDPROGRAMA"] = programa;
            if (Session["poa"] != null)
            {
                productos = cargaProductosPOA(Convert.ToInt32(cbAniPOA.Text), Convert.ToInt32(cbTipoProd.Value), resultado, programa, Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), resultado2);
                Session["poaproductos"] = productos;
            }

            gvDetail.DataSource = productos;

        }

        protected void gvPOAproductos_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            Session["carga"] = 2;
            ASPxGridView grid = sender as ASPxGridView;
            DataTable metas = new DataTable();
            String tabla;
            int fallos;
            double totalfisico, en, feb, mar, ab, may, jun, jul, ag, sep, oc, nov, dic, total;
            fallos = 0;
            if (Session["poaproductos"] != null)
            {
                metas = (DataTable)Session["poaproductos"];
                if (metas.Rows.Count > 0)
                {
                    for (int i = 0; i < metas.Rows.Count; i++)
                    {
                        if (metas.Rows[i]["SPPRO$ID_PRODUCTO"].ToString() == e.Keys["SPPRO$ID_PRODUCTO"].ToString())
                        {
                            if (metas.Rows[i]["MFPRODANUAL"] == DBNull.Value)
                                totalfisico = 0;
                            else
                                totalfisico = Convert.ToDouble(metas.Rows[i]["MFPRODANUAL"]);

                            if (e.NewValues["MFISMES1"] == DBNull.Value)
                                en = 0;
                            else
                                en = Convert.ToDouble(e.NewValues["MFISMES1"]);

                            if (e.NewValues["MFISMES2"] == DBNull.Value)
                                feb = 0;
                            else
                                feb = Convert.ToDouble(e.NewValues["MFISMES2"]);

                            if (e.NewValues["MFISMES3"] == DBNull.Value)
                                mar = 0;
                            else
                                mar = Convert.ToDouble(e.NewValues["MFISMES3"]);

                            if (e.NewValues["MFISMES4"] == DBNull.Value)
                                ab = 0;
                            else
                                ab = Convert.ToDouble(e.NewValues["MFISMES4"]);


                            if (e.NewValues["MFISMES5"] == DBNull.Value)
                                may = 0;
                            else
                                may = Convert.ToDouble(e.NewValues["MFISMES5"]);

                            if (e.NewValues["MFISMES6"] == DBNull.Value)
                                jun = 0;
                            else
                                jun = Convert.ToDouble(e.NewValues["MFISMES6"]);

                            if (e.NewValues["MFISMES7"] == DBNull.Value)
                                jul = 0;
                            else
                                jul = Convert.ToDouble(e.NewValues["MFISMES7"]);


                            if (e.NewValues["MFISMES8"] == DBNull.Value)
                                ag = 0;
                            else
                                ag = Convert.ToDouble(e.NewValues["MFISMES8"]);

                            if (e.NewValues["MFISMES9"] == DBNull.Value)
                                sep = 0;
                            else
                                sep = Convert.ToDouble(e.NewValues["MFISMES9"]);

                            if (e.NewValues["MFISMES10"] == DBNull.Value)
                                oc = 0;
                            else
                                oc = Convert.ToDouble(e.NewValues["MFISMES10"]);

                            if (e.NewValues["MFISMES11"] == DBNull.Value)
                                nov = 0;
                            else
                                nov = Convert.ToDouble(e.NewValues["MFISMES11"]);

                            if (e.NewValues["MFISMES12"] == DBNull.Value)
                                dic = 0;
                            else
                                dic = Convert.ToDouble(e.NewValues["MFISMES12"]);

                            total = en + feb + mar + ab + may + jun + jul + ag + sep + oc + nov + dic;

                            //if (total <= totalfisico)
                            //{
                            //inicio enero
                            if (metas.Rows[i]["IDMETMES1"] == DBNull.Value)
                            {
                                estado = insertaFisicaProdmensual(Convert.ToInt32(cbAniPOA.Value), Convert.ToInt32(cbAniPOA.Text), 1, Convert.ToInt32(e.Keys["SPPRO$ID_PRODUCTO"]), Convert.ToDouble(e.NewValues["MFISMES1"]));
                                if (estado == 0)
                                {
                                    mensaje = mensaje + " Enero ";
                                    fallos++;
                                }

                            }

                            else
                            {
                                estado = AcualizametafisicaMensualProd(Convert.ToInt32(cbAniPOA.Value), Convert.ToInt32(cbAniPOA.Text), Convert.ToDouble(e.NewValues["MFISMES1"]), 1, Convert.ToInt32(metas.Rows[i]["IDMETMES1"]), Convert.ToInt32(e.Keys["SPPRO$ID_PRODUCTO"]));

                                if (estado == 0)
                                {
                                    mensaje = mensaje + " Enero ";
                                    fallos++;
                                }
                            }
                            //        //fin enero

                            //inicio febrero
                            if (metas.Rows[i]["IDMETMES2"] == DBNull.Value)
                            {
                                estado = insertaFisicaProdmensual(Convert.ToInt32(cbAniPOA.Value), Convert.ToInt32(cbAniPOA.Text), 2, Convert.ToInt32(e.Keys["SPPRO$ID_PRODUCTO"]), Convert.ToDouble(e.NewValues["MFISMES2"]));
                                if (estado == 0)
                                {
                                    mensaje = mensaje + " febrero ";
                                    fallos++;
                                }

                            }

                            else
                            {
                                estado = AcualizametafisicaMensualProd(Convert.ToInt32(cbAniPOA.Value), Convert.ToInt32(cbAniPOA.Text), Convert.ToDouble(e.NewValues["MFISMES2"]), 2, Convert.ToInt32(metas.Rows[i]["IDMETMES2"]), Convert.ToInt32(e.Keys["SPPRO$ID_PRODUCTO"]));

                                if (estado == 0)
                                {
                                    mensaje = mensaje + " febrero ";
                                    fallos++;
                                }
                            }
                            //        //fin febrero

                            //inicio marzo
                            if (metas.Rows[i]["IDMETMES3"] == DBNull.Value)
                            {
                                estado = insertaFisicaProdmensual(Convert.ToInt32(cbAniPOA.Value), Convert.ToInt32(cbAniPOA.Text), 3, Convert.ToInt32(e.Keys["SPPRO$ID_PRODUCTO"]), Convert.ToDouble(e.NewValues["MFISMES3"]));
                                if (estado == 0)
                                {
                                    mensaje = mensaje + " marzo ";
                                    fallos++;
                                }

                            }

                            else
                            {
                                estado = AcualizametafisicaMensualProd(Convert.ToInt32(cbAniPOA.Value), Convert.ToInt32(cbAniPOA.Text), Convert.ToDouble(e.NewValues["MFISMES3"]), 3, Convert.ToInt32(metas.Rows[i]["IDMETMES3"]), Convert.ToInt32(e.Keys["SPPRO$ID_PRODUCTO"]));

                                if (estado == 0)
                                {
                                    mensaje = mensaje + " marzo ";
                                    fallos++;
                                }
                            }
                            //        //fin marzo

                            //inicio abril
                            if (metas.Rows[i]["IDMETMES4"] == DBNull.Value)
                            {
                                estado = insertaFisicaProdmensual(Convert.ToInt32(cbAniPOA.Value), Convert.ToInt32(cbAniPOA.Text), 4, Convert.ToInt32(e.Keys["SPPRO$ID_PRODUCTO"]), Convert.ToDouble(e.NewValues["MFISMES4"]));
                                if (estado == 0)
                                {
                                    mensaje = mensaje + " abril ";
                                    fallos++;
                                }

                            }

                            else
                            {
                                estado = AcualizametafisicaMensualProd(Convert.ToInt32(cbAniPOA.Value), Convert.ToInt32(cbAniPOA.Text), Convert.ToDouble(e.NewValues["MFISMES4"]), 4, Convert.ToInt32(metas.Rows[i]["IDMETMES4"]), Convert.ToInt32(e.Keys["SPPRO$ID_PRODUCTO"]));

                                if (estado == 0)
                                {
                                    mensaje = mensaje + " abril ";
                                    fallos++;
                                }
                            }
                            //        //fin abril

                            //inicio mayo
                            if (metas.Rows[i]["IDMETMES5"] == DBNull.Value)
                            {
                                estado = insertaFisicaProdmensual(Convert.ToInt32(cbAniPOA.Value), Convert.ToInt32(cbAniPOA.Text), 5, Convert.ToInt32(e.Keys["SPPRO$ID_PRODUCTO"]), Convert.ToDouble(e.NewValues["MFISMES5"]));
                                if (estado == 0)
                                {
                                    mensaje = mensaje + " mayo ";
                                    fallos++;
                                }

                            }

                            else
                            {
                                estado = AcualizametafisicaMensualProd(Convert.ToInt32(cbAniPOA.Value), Convert.ToInt32(cbAniPOA.Text), Convert.ToDouble(e.NewValues["MFISMES5"]), 5, Convert.ToInt32(metas.Rows[i]["IDMETMES5"]), Convert.ToInt32(e.Keys["SPPRO$ID_PRODUCTO"]));

                                if (estado == 0)
                                {
                                    mensaje = mensaje + " mayo ";
                                    fallos++;
                                }
                            }
                            //        //fin mayo


                            //inicio junio
                            if (metas.Rows[i]["IDMETMES6"] == DBNull.Value)
                            {
                                estado = insertaFisicaProdmensual(Convert.ToInt32(cbAniPOA.Value), Convert.ToInt32(cbAniPOA.Text), 6, Convert.ToInt32(e.Keys["SPPRO$ID_PRODUCTO"]), Convert.ToDouble(e.NewValues["MFISMES6"]));
                                if (estado == 0)
                                {
                                    mensaje = mensaje + " junio ";
                                    fallos++;
                                }

                            }

                            else
                            {
                                estado = AcualizametafisicaMensualProd(Convert.ToInt32(cbAniPOA.Value), Convert.ToInt32(cbAniPOA.Text), Convert.ToDouble(e.NewValues["MFISMES6"]), 6, Convert.ToInt32(metas.Rows[i]["IDMETMES6"]), Convert.ToInt32(e.Keys["SPPRO$ID_PRODUCTO"]));

                                if (estado == 0)
                                {
                                    mensaje = mensaje + " junio ";
                                    fallos++;
                                }
                            }
                            //        //fin junio


                            //inicio julio
                            if (metas.Rows[i]["IDMETMES7"] == DBNull.Value)
                            {
                                estado = insertaFisicaProdmensual(Convert.ToInt32(cbAniPOA.Value), Convert.ToInt32(cbAniPOA.Text), 7, Convert.ToInt32(e.Keys["SPPRO$ID_PRODUCTO"]), Convert.ToDouble(e.NewValues["MFISMES7"]));
                                if (estado == 0)
                                {
                                    mensaje = mensaje + " julio ";
                                    fallos++;
                                }

                            }

                            else
                            {
                                estado = AcualizametafisicaMensualProd(Convert.ToInt32(cbAniPOA.Value), Convert.ToInt32(cbAniPOA.Text), Convert.ToDouble(e.NewValues["MFISMES7"]), 7, Convert.ToInt32(metas.Rows[i]["IDMETMES7"]), Convert.ToInt32(e.Keys["SPPRO$ID_PRODUCTO"]));

                                if (estado == 0)
                                {
                                    mensaje = mensaje + " julio ";
                                    fallos++;
                                }
                            }
                            //        //fin julio


                            //inicio agosto
                            if (metas.Rows[i]["IDMETMES8"] == DBNull.Value)
                            {
                                estado = insertaFisicaProdmensual(Convert.ToInt32(cbAniPOA.Value), Convert.ToInt32(cbAniPOA.Text), 8, Convert.ToInt32(e.Keys["SPPRO$ID_PRODUCTO"]), Convert.ToDouble(e.NewValues["MFISMES8"]));
                                if (estado == 0)
                                {
                                    mensaje = mensaje + " agosto ";
                                    fallos++;
                                }

                            }

                            else
                            {
                                estado = AcualizametafisicaMensualProd(Convert.ToInt32(cbAniPOA.Value), Convert.ToInt32(cbAniPOA.Text), Convert.ToDouble(e.NewValues["MFISMES8"]), 8, Convert.ToInt32(metas.Rows[i]["IDMETMES8"]), Convert.ToInt32(e.Keys["SPPRO$ID_PRODUCTO"]));

                                if (estado == 0)
                                {
                                    mensaje = mensaje + " agosto ";
                                    fallos++;
                                }
                            }
                            //        //fin agosto


                            //inicio septiembre
                            if (metas.Rows[i]["IDMETMES9"] == DBNull.Value)
                            {
                                estado = insertaFisicaProdmensual(Convert.ToInt32(cbAniPOA.Value), Convert.ToInt32(cbAniPOA.Text), 9, Convert.ToInt32(e.Keys["SPPRO$ID_PRODUCTO"]), Convert.ToDouble(e.NewValues["MFISMES9"]));
                                if (estado == 0)
                                {
                                    mensaje = mensaje + " septiembre ";
                                    fallos++;
                                }

                            }

                            else
                            {
                                estado = AcualizametafisicaMensualProd(Convert.ToInt32(cbAniPOA.Value), Convert.ToInt32(cbAniPOA.Text), Convert.ToDouble(e.NewValues["MFISMES9"]), 9, Convert.ToInt32(metas.Rows[i]["IDMETMES9"]), Convert.ToInt32(e.Keys["SPPRO$ID_PRODUCTO"]));

                                if (estado == 0)
                                {
                                    mensaje = mensaje + " septiembre ";
                                    fallos++;
                                }
                            }
                            //        //fin septiembre


                            //inicio octubre
                            if (metas.Rows[i]["IDMETMES10"] == DBNull.Value)
                            {
                                estado = insertaFisicaProdmensual(Convert.ToInt32(cbAniPOA.Value), Convert.ToInt32(cbAniPOA.Text), 10, Convert.ToInt32(e.Keys["SPPRO$ID_PRODUCTO"]), Convert.ToDouble(e.NewValues["MFISMES10"]));
                                if (estado == 0)
                                {
                                    mensaje = mensaje + " octubre ";
                                    fallos++;
                                }

                            }

                            else
                            {
                                estado = AcualizametafisicaMensualProd(Convert.ToInt32(cbAniPOA.Value), Convert.ToInt32(cbAniPOA.Text), Convert.ToDouble(e.NewValues["MFISMES10"]), 10, Convert.ToInt32(metas.Rows[i]["IDMETMES10"]), Convert.ToInt32(e.Keys["SPPRO$ID_PRODUCTO"]));

                                if (estado == 0)
                                {
                                    mensaje = mensaje + " octubre ";
                                    fallos++;
                                }
                            }
                            //        //fin octubre


                            //inicio noviembre
                            if (metas.Rows[i]["IDMETMES11"] == DBNull.Value)
                            {
                                estado = insertaFisicaProdmensual(Convert.ToInt32(cbAniPOA.Value), Convert.ToInt32(cbAniPOA.Text), 11, Convert.ToInt32(e.Keys["SPPRO$ID_PRODUCTO"]), Convert.ToDouble(e.NewValues["MFISMES11"]));
                                if (estado == 0)
                                {
                                    mensaje = mensaje + " noviembre ";
                                    fallos++;
                                }

                            }

                            else
                            {
                                estado = AcualizametafisicaMensualProd(Convert.ToInt32(cbAniPOA.Value), Convert.ToInt32(cbAniPOA.Text), Convert.ToDouble(e.NewValues["MFISMES11"]), 11, Convert.ToInt32(metas.Rows[i]["IDMETMES11"]), Convert.ToInt32(e.Keys["SPPRO$ID_PRODUCTO"]));

                                if (estado == 0)
                                {
                                    mensaje = mensaje + " noviembre ";
                                    fallos++;
                                }
                            }
                            //        //fin noviembre

                            //inicio diciembre
                            if (metas.Rows[i]["IDMETMES12"] == DBNull.Value)
                            {
                                estado = insertaFisicaProdmensual(Convert.ToInt32(cbAniPOA.Value), Convert.ToInt32(cbAniPOA.Text), 12, Convert.ToInt32(e.Keys["SPPRO$ID_PRODUCTO"]), Convert.ToDouble(e.NewValues["MFISMES12"]));
                                if (estado == 0)
                                {
                                    mensaje = mensaje + " diciembre ";
                                    fallos++;
                                }

                            }

                            else
                            {
                                estado = AcualizametafisicaMensualProd(Convert.ToInt32(cbAniPOA.Value), Convert.ToInt32(cbAniPOA.Text), Convert.ToDouble(e.NewValues["MFISMES12"]), 12, Convert.ToInt32(metas.Rows[i]["IDMETMES12"]), Convert.ToInt32(e.Keys["SPPRO$ID_PRODUCTO"]));

                                if (estado == 0)
                                {
                                    mensaje = mensaje + " diciembre ";
                                    fallos++;
                                }
                            }
                            //        //fin diciembre


                            if (fallos > 0)
                            {
                                mensaje = "Las metas de los meses " + mensaje + ", no pudieron ser guardadas por posibles fallos en el sistema, intententelo mas tarde o contacte al administrador";
                                grid.JSProperties["cp1"] = "Información: " + mensaje;
                                e.Cancel = true;
                                grid.CancelEdit();
                                break;
                            }


                            else
                            {
                                mensaje = "La metas fueron guardadas correctamente";


                                grid.JSProperties["cpError"] = "Información: " + mensaje;
                                e.Cancel = true;
                                grid.CancelEdit();
                                break;
                            }



                            //}

                            //else
                            //{
                            //    mensaje = "La suma de las metas mensuales ingresadas, excede el valor de la meta fisica anual, verifique sus cantitades";
                            //    tabla = "<p>Error en metas fisicas: " + mensaje + "</p>";
                            //        tabla = tabla + "<table class='table'>";
                            //        tabla = tabla + "<tr><th>Mes</th><th>Meta mensual</th><th>Meta anual</th></tr>";
                            //        tabla = tabla + "<tr><td>Enero</td><td>" + String.Format("{0:#,0}", en) + "</td><td></td></tr>";
                            //        tabla = tabla + "<tr><td>Febrero</td><td>" + String.Format("{0:#,0}", feb) + "</td><td></td></tr>";
                            //        tabla = tabla + "<tr><td>Marzo</td><td>" + String.Format("{0:#,0}", mar) + "</td><td></td></tr>";
                            //        tabla = tabla + "<tr><td>Abril</td><td>" + String.Format("{0:#,0}", ab) + "</td><td></td></tr>";
                            //        tabla = tabla + "<tr><td>Mayo</td><td>" + String.Format("{0:#,0}", may) + "</td><td></td></tr>";
                            //        tabla = tabla + "<tr><td>Junio</td><td>" + String.Format("{0:#,0}", jun) + "</td><td></td></tr>";
                            //        tabla = tabla + "<tr><td>Julio</td><td>" + String.Format("{0:#,0}", jul) + "</td><td></td></tr>";
                            //        tabla = tabla + "<tr><td>Agosto</td><td>" + String.Format("{0:#,0}", ag) + "</td><td></td></tr>";
                            //        tabla = tabla + "<tr><td>Septiembre</td><td>" + String.Format("{0:#,0}", sep) + "</td><td></td></tr>";
                            //        tabla = tabla + "<tr><td>Octubre</td><td>" + String.Format("{0:#,0}", oc) + "</td><td></td></tr>";
                            //        tabla = tabla + "<tr><td>Noviembre</td><td>" + String.Format("{0:#,0}", nov) + "</td><td></td></tr>";
                            //        tabla = tabla + "<tr><td>Diciembre</td><td>" + String.Format("{0:#,0}", dic) + "</td><td></td></tr>";
                            //        tabla = tabla + "<tfoot><tr><td>Suma metas</td><td style='color:red;font-weight: bold'>" + String.Format("{0:#,0}", total) + "</td><td style='font-weight: bold'>" + String.Format("{0:#,0}", Convert.ToDouble(metas.Rows[i]["MFPRODANUAL"])) + "</td></tr></tfoot>";
                            //        tabla = tabla + "</ table > ";
                            //        grid.JSProperties["cp3"] = tabla;
                            //        e.Cancel = true;
                            //        grid.CancelEdit();
                            //        break;


                            //    grid.JSProperties["cpError"] = "Información: " + mensaje;
                            //    e.Cancel = true;
                            //    grid.CancelEdit();
                            //    break;
                            //}





                        }

                    }
                }
                else
                {
                    mensaje = "Error en la sesión";
                    grid.JSProperties["cp1"] = "Información: " + mensaje;
                    e.Cancel = true;
                    grid.CancelEdit();
                }
            }
            else
            {
                mensaje = "Error en la sesión";
                grid.JSProperties["cp1"] = "Información: " + mensaje;
                e.Cancel = true;
                grid.CancelEdit();
            }

        }



        protected void gvPOASubproductos_BeforePerformDataSelect(object sender, EventArgs e)
        {
            Session["carga"] = 2;
            ASPxGridView gvDetail = sender as ASPxGridView;
            DataTable subproductos = new DataTable();
            int producto, resultado;
            Double programa;
            producto = Convert.ToInt32((sender as ASPxGridView).GetMasterRowFieldValues("SPPRO$ID_PRODUCTO"));
            programa = Convert.ToDouble((sender as ASPxGridView).GetMasterRowFieldValues("SPPRO$ID_PROGRAMA_PRESUPUESTO"));
            resultado = Convert.ToInt32((sender as ASPxGridView).GetMasterRowFieldValues("ID_RESULTADO"));

            if (Convert.ToInt32(Session["abierto"]) == 0)



                if (Session["poa"] != null)
                {
                    subproductos = cargasubproductosPOA(Convert.ToInt32(cbAniPOA.Text), Convert.ToInt32(cbTipoProd.Value), resultado, programa, Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), producto);
                    Session["subproductospoa"] = subproductos;


                }
            gvDetail.DataSource = subproductos;

        }

        protected void gvPOASubproductos_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            Session["carga"] = 2;
            ASPxGridView grid = sender as ASPxGridView;
            DataTable metas = new DataTable();
            int en, feb, mar, ab, may, jun, jul, ag, sep, oc, nov, dic, totalfisico, fallofisico, fallofinanciero, fallotodo, anualfisico;
            Double enf, febf, marf, abf, mayf, junf, julf, agf, sepf, ocf, novf, dicf, totalfinanciero, anualfinanciero;
            fallofisico = 0;
            fallofinanciero = 0;
            fallotodo = 0;
            String mensajefisico, mensajefinanciero, tabla;
            mensajefisico = "";
            mensajefinanciero = "";

            if (Session["subproductospoa"] != null)
            {
                metas = (DataTable)Session["subproductospoa"];
                if (metas.Rows.Count > 0)
                {
                    for (int i = 0; i < metas.Rows.Count; i++)
                    {
                        if (metas.Rows[i]["SPPSUB$ID_SUBPRODUCTO"].ToString() == e.Keys["SPPSUB$ID_SUBPRODUCTO"].ToString())
                        {
                            if (metas.Rows[i]["ANUALFISICO"] == DBNull.Value)
                                anualfisico = 0;
                            else
                                anualfisico = Convert.ToInt32(metas.Rows[i]["ANUALFISICO"]);

                            if (metas.Rows[i]["ANUALFINANCIERO"] == DBNull.Value)
                                anualfinanciero = 0;
                            else
                                anualfinanciero = Convert.ToDouble(metas.Rows[i]["ANUALFINANCIERO"]);

                            if (e.NewValues["MFSUB1"] == DBNull.Value)
                                en = 0;
                            else
                                en = Convert.ToInt32(e.NewValues["MFSUB1"]);

                            if (e.NewValues["MFSUB2"] == DBNull.Value)
                                feb = 0;
                            else
                                feb = Convert.ToInt32(e.NewValues["MFSUB2"]);

                            if (e.NewValues["MFSUB3"] == DBNull.Value)
                                mar = 0;
                            else
                                mar = Convert.ToInt32(e.NewValues["MFSUB3"]);

                            if (e.NewValues["MFSUB4"] == DBNull.Value)
                                ab = 0;
                            else
                                ab = Convert.ToInt32(e.NewValues["MFSUB4"]);


                            if (e.NewValues["MFSUB5"] == DBNull.Value)
                                may = 0;
                            else
                                may = Convert.ToInt32(e.NewValues["MFSUB5"]);

                            if (e.NewValues["MFSUB6"] == DBNull.Value)
                                jun = 0;
                            else
                                jun = Convert.ToInt32(e.NewValues["MFSUB6"]);

                            if (e.NewValues["MFSUB7"] == DBNull.Value)
                                jul = 0;
                            else
                                jul = Convert.ToInt32(e.NewValues["MFSUB7"]);


                            if (e.NewValues["MFSUB8"] == DBNull.Value)
                                ag = 0;
                            else
                                ag = Convert.ToInt32(e.NewValues["MFSUB8"]);

                            if (e.NewValues["MFSUB9"] == DBNull.Value)
                                sep = 0;
                            else
                                sep = Convert.ToInt32(e.NewValues["MFSUB9"]);

                            if (e.NewValues["MFSUB10"] == DBNull.Value)
                                oc = 0;
                            else
                                oc = Convert.ToInt32(e.NewValues["MFSUB10"]);

                            if (e.NewValues["MFSUB11"] == DBNull.Value)
                                nov = 0;
                            else
                                nov = Convert.ToInt32(e.NewValues["MFSUB11"]);

                            if (e.NewValues["MFSUB12"] == DBNull.Value)
                                dic = 0;
                            else
                                dic = Convert.ToInt32(e.NewValues["MFSUB12"]);

                            totalfisico = en + feb + mar + ab + may + jun + jul + ag + sep + oc + nov + dic;

                            if (e.NewValues["MFINSUB1"] == DBNull.Value)
                                enf = 0;
                            else
                                enf = Convert.ToDouble(e.NewValues["MFINSUB1"]);

                            if (e.NewValues["MFINSUB2"] == DBNull.Value)
                                febf = 0;
                            else
                                febf = Convert.ToDouble(e.NewValues["MFINSUB2"]);

                            if (e.NewValues["MFINSUB3"] == DBNull.Value)
                                marf = 0;
                            else
                                marf = Convert.ToDouble(e.NewValues["MFINSUB3"]);

                            if (e.NewValues["MFINSUB4"] == DBNull.Value)
                                abf = 0;
                            else
                                abf = Convert.ToDouble(e.NewValues["MFINSUB4"]);


                            if (e.NewValues["MFINSUB5"] == DBNull.Value)
                                mayf = 0;
                            else
                                mayf = Convert.ToDouble(e.NewValues["MFINSUB5"]);

                            if (e.NewValues["MFINSUB6"] == DBNull.Value)
                                junf = 0;
                            else
                                junf = Convert.ToDouble(e.NewValues["MFINSUB6"]);

                            if (e.NewValues["MFINSUB7"] == DBNull.Value)
                                julf = 0;
                            else
                                julf = Convert.ToDouble(e.NewValues["MFINSUB7"]);


                            if (e.NewValues["MFINSUB8"] == DBNull.Value)
                                agf = 0;
                            else
                                agf = Convert.ToDouble(e.NewValues["MFINSUB8"]);

                            if (e.NewValues["MFINSUB9"] == DBNull.Value)
                                sepf = 0;
                            else
                                sepf = Convert.ToDouble(e.NewValues["MFINSUB9"]);

                            if (e.NewValues["MFINSUB10"] == DBNull.Value)
                                ocf = 0;
                            else
                                ocf = Convert.ToDouble(e.NewValues["MFINSUB10"]);

                            if (e.NewValues["MFINSUB11"] == DBNull.Value)
                                novf = 0;
                            else
                                novf = Convert.ToDouble(e.NewValues["MFINSUB11"]);

                            if (e.NewValues["MFINSUB12"] == DBNull.Value)
                                dicf = 0;
                            else
                                dicf = Convert.ToDouble(e.NewValues["MFINSUB12"]);


                            // totalfinanciero = enf + febf + marf + abf + mayf + junf + julf + agf + sepf + ocf + novf + dicf;

                            // totalfinanciero = Math.Round(totalfinanciero, 2);

                            //anualfinanciero = Math.Round(anualfinanciero, 2);

                            //if (totalfisico > anualfisico && totalfinanciero > anualfinanciero)
                            //{
                            //    mensaje = "La suma de las metas mensuales ingresadas, excede el valor de la meta física anual y meta financiera anual, verifique sus cantitades";
                            //    fallotodo++;
                            //    fallofisico++;
                            //    fallofinanciero++;
                            //}

                            //else
                            //{
                            //if (totalfisico > anualfisico)
                            //{
                            //    mensajefisico = "La suma de las metas fisicas excede el valor de la meta física anual, por favor verifique sus cantidades";
                            //    fallofisico++;
                            //}
                            //else
                            //{
                            //    //inicio enero
                            if (metas.Rows[i]["IDFISUB1"] == DBNull.Value)
                            {
                                estado = insertaMetasubPOA(Convert.ToInt32(cbAniPOA.Value), Convert.ToInt32(cbAniPOA.Text), Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"]), 1, Convert.ToDouble(e.NewValues["MFSUB1"]), 0);
                                if (estado == 0)
                                {
                                    mensajefisico = mensajefisico + " enero ";
                                    fallofisico++;
                                }

                            }

                            else
                            {
                                estado = AcualizametasubPOA(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"]), Convert.ToInt32(cbAniPOA.Value), Convert.ToInt32(cbAniPOA.Text), Convert.ToDouble(e.NewValues["MFSUB1"]), Convert.ToInt32(metas.Rows[i]["IDFISUB1"]), 0, 1);

                                if (estado == 0)
                                {
                                    mensajefisico = mensajefisico + " enero ";
                                    fallofisico++;
                                }
                            }

                            //fin enero

                            //inicio febrero
                            if (metas.Rows[i]["IDFISUB2"] == DBNull.Value)
                            {
                                estado = insertaMetasubPOA(Convert.ToInt32(cbAniPOA.Value), Convert.ToInt32(cbAniPOA.Text), Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"]), 2, Convert.ToDouble(e.NewValues["MFSUB2"]), 0);
                                if (estado == 0)
                                {
                                    mensajefisico = mensajefisico + " febrero ";
                                    fallofisico++;
                                }

                            }

                            else
                            {
                                estado = AcualizametasubPOA(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"]), Convert.ToInt32(cbAniPOA.Value), Convert.ToInt32(cbAniPOA.Text), Convert.ToDouble(e.NewValues["MFSUB2"]), Convert.ToInt32(metas.Rows[i]["IDFISUB2"]), 0, 2);

                                if (estado == 0)
                                {
                                    mensajefisico = mensajefisico + " febrero ";
                                    fallofisico++;
                                }
                            }

                            //fin febrero

                            //inicio marzo
                            if (metas.Rows[i]["IDFISUB3"] == DBNull.Value)
                            {
                                estado = insertaMetasubPOA(Convert.ToInt32(cbAniPOA.Value), Convert.ToInt32(cbAniPOA.Text), Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"]), 3, Convert.ToDouble(e.NewValues["MFSUB3"]), 0);
                                if (estado == 0)
                                {
                                    mensajefisico = mensajefisico + " marzo ";
                                    fallofisico++;
                                }

                            }

                            else
                            {
                                estado = AcualizametasubPOA(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"]), Convert.ToInt32(cbAniPOA.Value), Convert.ToInt32(cbAniPOA.Text), Convert.ToDouble(e.NewValues["MFSUB3"]), Convert.ToInt32(metas.Rows[i]["IDFISUB3"]), 0, 3);

                                if (estado == 0)
                                {
                                    mensajefisico = mensajefisico + " marzo ";
                                    fallofisico++;
                                }
                            }

                            //fin marzo

                            //inicio abril
                            if (metas.Rows[i]["IDFISUB4"] == DBNull.Value)
                            {
                                estado = insertaMetasubPOA(Convert.ToInt32(cbAniPOA.Value), Convert.ToInt32(cbAniPOA.Text), Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"]), 4, Convert.ToDouble(e.NewValues["MFSUB4"]), 0);
                                if (estado == 0)
                                {
                                    mensajefisico = mensajefisico + " abril ";
                                    fallofisico++;
                                }

                            }

                            else
                            {
                                estado = AcualizametasubPOA(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"]), Convert.ToInt32(cbAniPOA.Value), Convert.ToInt32(cbAniPOA.Text), Convert.ToDouble(e.NewValues["MFSUB4"]), Convert.ToInt32(metas.Rows[i]["IDFISUB4"]), 0, 4);

                                if (estado == 0)
                                {
                                    mensajefisico = mensajefisico + " abril ";
                                    fallofisico++;
                                }
                            }

                            //fin abril


                            //inicio mayo
                            if (metas.Rows[i]["IDFISUB5"] == DBNull.Value)
                            {
                                estado = insertaMetasubPOA(Convert.ToInt32(cbAniPOA.Value), Convert.ToInt32(cbAniPOA.Text), Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"]), 5, Convert.ToDouble(e.NewValues["MFSUB5"]), 0);
                                if (estado == 0)
                                {
                                    mensajefisico = mensajefisico + " mayo ";
                                    fallofisico++;
                                }

                            }

                            else
                            {
                                estado = AcualizametasubPOA(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"]), Convert.ToInt32(cbAniPOA.Value), Convert.ToInt32(cbAniPOA.Text), Convert.ToDouble(e.NewValues["MFSUB5"]), Convert.ToInt32(metas.Rows[i]["IDFISUB5"]), 0, 5);

                                if (estado == 0)
                                {
                                    mensajefisico = mensajefisico + " mayo ";
                                    fallofisico++;
                                }
                            }

                            //fin mayo


                            //inicio junio
                            if (metas.Rows[i]["IDFISUB6"] == DBNull.Value)
                            {
                                estado = insertaMetasubPOA(Convert.ToInt32(cbAniPOA.Value), Convert.ToInt32(cbAniPOA.Text), Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"]), 6, Convert.ToDouble(e.NewValues["MFSUB6"]), 0);
                                if (estado == 0)
                                {
                                    mensajefisico = mensajefisico + " junio ";
                                    fallofisico++;
                                }

                            }

                            else
                            {
                                estado = AcualizametasubPOA(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"]), Convert.ToInt32(cbAniPOA.Value), Convert.ToInt32(cbAniPOA.Text), Convert.ToDouble(e.NewValues["MFSUB6"]), Convert.ToInt32(metas.Rows[i]["IDFISUB6"]), 0, 6);

                                if (estado == 0)
                                {
                                    mensajefisico = mensajefisico + " junio ";
                                    fallofisico++;
                                }
                            }

                            //fin junio


                            //inicio julio
                            if (metas.Rows[i]["IDFISUB7"] == DBNull.Value)
                            {
                                estado = insertaMetasubPOA(Convert.ToInt32(cbAniPOA.Value), Convert.ToInt32(cbAniPOA.Text), Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"]), 7, Convert.ToDouble(e.NewValues["MFSUB7"]), 0);
                                if (estado == 0)
                                {
                                    mensajefisico = mensajefisico + " julio ";
                                    fallofisico++;
                                }

                            }

                            else
                            {
                                estado = AcualizametasubPOA(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"]), Convert.ToInt32(cbAniPOA.Value), Convert.ToInt32(cbAniPOA.Text), Convert.ToDouble(e.NewValues["MFSUB7"]), Convert.ToInt32(metas.Rows[i]["IDFISUB7"]), 0, 7);

                                if (estado == 0)
                                {
                                    mensajefisico = mensajefisico + " julio ";
                                    fallofisico++;
                                }
                            }

                            //fin julio


                            //inicio agosto
                            if (metas.Rows[i]["IDFISUB8"] == DBNull.Value)
                            {
                                estado = insertaMetasubPOA(Convert.ToInt32(cbAniPOA.Value), Convert.ToInt32(cbAniPOA.Text), Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"]), 8, Convert.ToDouble(e.NewValues["MFSUB8"]), 0);
                                if (estado == 0)
                                {
                                    mensajefisico = mensajefisico + " agosto ";
                                    fallofisico++;
                                }

                            }

                            else
                            {
                                estado = AcualizametasubPOA(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"]), Convert.ToInt32(cbAniPOA.Value), Convert.ToInt32(cbAniPOA.Text), Convert.ToDouble(e.NewValues["MFSUB8"]), Convert.ToInt32(metas.Rows[i]["IDFISUB8"]), 0, 8);

                                if (estado == 0)
                                {
                                    mensajefisico = mensajefisico + " agosto ";
                                    fallofisico++;
                                }
                            }

                            //fin agosto


                            //inicio septiembre
                            if (metas.Rows[i]["IDFISUB9"] == DBNull.Value)
                            {
                                estado = insertaMetasubPOA(Convert.ToInt32(cbAniPOA.Value), Convert.ToInt32(cbAniPOA.Text), Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"]), 9, Convert.ToDouble(e.NewValues["MFSUB9"]), 0);
                                if (estado == 0)
                                {
                                    mensajefisico = mensajefisico + " septiembre ";
                                    fallofisico++;
                                }

                            }

                            else
                            {
                                estado = AcualizametasubPOA(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"]), Convert.ToInt32(cbAniPOA.Value), Convert.ToInt32(cbAniPOA.Text), Convert.ToDouble(e.NewValues["MFSUB9"]), Convert.ToInt32(metas.Rows[i]["IDFISUB9"]), 0, 9);

                                if (estado == 0)
                                {
                                    mensajefisico = mensajefisico + " septiembre ";
                                    fallofisico++;
                                }
                            }

                            //fin septiembre


                            //inicio octubre
                            if (metas.Rows[i]["IDFISUB10"] == DBNull.Value)
                            {
                                estado = insertaMetasubPOA(Convert.ToInt32(cbAniPOA.Value), Convert.ToInt32(cbAniPOA.Text), Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"]), 10, Convert.ToDouble(e.NewValues["MFSUB10"]), 0);
                                if (estado == 0)
                                {
                                    mensajefisico = mensajefisico + " octubre ";
                                    fallofisico++;
                                }

                            }

                            else
                            {
                                estado = AcualizametasubPOA(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"]), Convert.ToInt32(cbAniPOA.Value), Convert.ToInt32(cbAniPOA.Text), Convert.ToDouble(e.NewValues["MFSUB10"]), Convert.ToInt32(metas.Rows[i]["IDFISUB10"]), 0, 10);

                                if (estado == 0)
                                {
                                    mensajefisico = mensajefisico + " octubre ";
                                    fallofisico++;
                                }
                            }

                            //fin octubre


                            //inicio noviembre
                            if (metas.Rows[i]["IDFISUB11"] == DBNull.Value)
                            {
                                estado = insertaMetasubPOA(Convert.ToInt32(cbAniPOA.Value), Convert.ToInt32(cbAniPOA.Text), Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"]), 11, Convert.ToDouble(e.NewValues["MFSUB11"]), 0);
                                if (estado == 0)
                                {
                                    mensajefisico = mensajefisico + " noviembre ";
                                    fallofisico++;
                                }

                            }

                            else
                            {
                                estado = AcualizametasubPOA(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"]), Convert.ToInt32(cbAniPOA.Value), Convert.ToInt32(cbAniPOA.Text), Convert.ToDouble(e.NewValues["MFSUB11"]), Convert.ToInt32(metas.Rows[i]["IDFISUB11"]), 0, 11);

                                if (estado == 0)
                                {
                                    mensajefisico = mensajefisico + " noviembre ";
                                    fallofisico++;
                                }
                            }

                            //fin noviembre


                            //inicio diciembre
                            if (metas.Rows[i]["IDFISUB12"] == DBNull.Value)
                            {
                                estado = insertaMetasubPOA(Convert.ToInt32(cbAniPOA.Value), Convert.ToInt32(cbAniPOA.Text), Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"]), 12, Convert.ToDouble(e.NewValues["MFSUB12"]), 0);
                                if (estado == 0)
                                {
                                    mensajefisico = mensajefisico + " diciembre ";
                                    fallofisico++;
                                }

                            }

                            else
                            {
                                estado = AcualizametasubPOA(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"]), Convert.ToInt32(cbAniPOA.Value), Convert.ToInt32(cbAniPOA.Text), Convert.ToDouble(e.NewValues["MFSUB12"]), Convert.ToInt32(metas.Rows[i]["IDFISUB12"]), 0, 12);

                                if (estado == 0)
                                {
                                    mensajefisico = mensajefisico + " diciembre ";
                                    fallofisico++;
                                }
                            }

                            //fin diciembre



                            //}

                            //if (totalfinanciero > anualfinanciero)
                            //{
                            //    mensajefinanciero = "La suma de las metas finanacieras excede el valor de la meta financiera anual, por favor verifique sus cantidades";
                            //    fallofinanciero++;
                            //}
                            //else
                            //{
                            //inicio enero
                            if (metas.Rows[i]["IDFINSUB1"] == DBNull.Value)
                            {
                                estado = insertaMetasubPOA(Convert.ToInt32(cbAniPOA.Value), Convert.ToInt32(cbAniPOA.Text), Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"]), 1, Convert.ToDouble(e.NewValues["MFINSUB1"]), 1);
                                if (estado == 0)
                                {
                                    mensajefinanciero = mensajefinanciero + " enero ";
                                    fallofinanciero++;
                                }

                            }

                            else
                            {
                                estado = AcualizametasubPOA(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"]), Convert.ToInt32(cbAniPOA.Value), Convert.ToInt32(cbAniPOA.Text), Convert.ToDouble(e.NewValues["MFINSUB1"]), Convert.ToInt32(metas.Rows[i]["IDFINSUB1"]), 1, 1);

                                if (estado == 0)
                                {
                                    mensajefinanciero = mensajefinanciero + " enero ";
                                    fallofinanciero++;
                                }
                            }
                            //fin enero


                            //inicio febrero
                            if (metas.Rows[i]["IDFINSUB2"] == DBNull.Value)
                            {
                                estado = insertaMetasubPOA(Convert.ToInt32(cbAniPOA.Value), Convert.ToInt32(cbAniPOA.Text), Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"]), 2, Convert.ToDouble(e.NewValues["MFINSUB2"]), 1);
                                if (estado == 0)
                                {
                                    mensajefinanciero = mensajefinanciero + " febrero ";
                                    fallofinanciero++;
                                }

                            }

                            else
                            {
                                estado = AcualizametasubPOA(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"]), Convert.ToInt32(cbAniPOA.Value), Convert.ToInt32(cbAniPOA.Text), Convert.ToDouble(e.NewValues["MFINSUB2"]), Convert.ToInt32(metas.Rows[i]["IDFINSUB2"]), 1, 2);

                                if (estado == 0)
                                {
                                    mensajefinanciero = mensajefinanciero + " febrero ";
                                    fallofinanciero++;
                                }
                            }
                            //fin febrero

                            //inicio marzo
                            if (metas.Rows[i]["IDFINSUB3"] == DBNull.Value)
                            {
                                estado = insertaMetasubPOA(Convert.ToInt32(cbAniPOA.Value), Convert.ToInt32(cbAniPOA.Text), Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"]), 3, Convert.ToDouble(e.NewValues["MFINSUB3"]), 1);
                                if (estado == 0)
                                {
                                    mensajefinanciero = mensajefinanciero + " marzo ";
                                    fallofinanciero++;
                                }

                            }

                            else
                            {
                                estado = AcualizametasubPOA(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"]), Convert.ToInt32(cbAniPOA.Value), Convert.ToInt32(cbAniPOA.Text), Convert.ToDouble(e.NewValues["MFINSUB3"]), Convert.ToInt32(metas.Rows[i]["IDFINSUB3"]), 1, 3);

                                if (estado == 0)
                                {
                                    mensajefinanciero = mensajefinanciero + " marzo ";
                                    fallofinanciero++;
                                }
                            }
                            //fin marzo


                            //inicio abril
                            if (metas.Rows[i]["IDFINSUB4"] == DBNull.Value)
                            {
                                estado = insertaMetasubPOA(Convert.ToInt32(cbAniPOA.Value), Convert.ToInt32(cbAniPOA.Text), Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"]), 4, Convert.ToDouble(e.NewValues["MFINSUB4"]), 1);
                                if (estado == 0)
                                {
                                    mensajefinanciero = mensajefinanciero + " abril ";
                                    fallofinanciero++;
                                }

                            }

                            else
                            {
                                estado = AcualizametasubPOA(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"]), Convert.ToInt32(cbAniPOA.Value), Convert.ToInt32(cbAniPOA.Text), Convert.ToDouble(e.NewValues["MFINSUB4"]), Convert.ToInt32(metas.Rows[i]["IDFINSUB4"]), 1, 4);

                                if (estado == 0)
                                {
                                    mensajefinanciero = mensajefinanciero + " abril ";
                                    fallofinanciero++;
                                }
                            }
                            //fin abril


                            //inicio mayo
                            if (metas.Rows[i]["IDFINSUB5"] == DBNull.Value)
                            {
                                estado = insertaMetasubPOA(Convert.ToInt32(cbAniPOA.Value), Convert.ToInt32(cbAniPOA.Text), Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"]), 5, Convert.ToDouble(e.NewValues["MFINSUB5"]), 1);
                                if (estado == 0)
                                {
                                    mensajefinanciero = mensajefinanciero + " mayo ";
                                    fallofinanciero++;
                                }

                            }

                            else
                            {
                                estado = AcualizametasubPOA(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"]), Convert.ToInt32(cbAniPOA.Value), Convert.ToInt32(cbAniPOA.Text), Convert.ToDouble(e.NewValues["MFINSUB5"]), Convert.ToInt32(metas.Rows[i]["IDFINSUB5"]), 1, 5);

                                if (estado == 0)
                                {
                                    mensajefinanciero = mensajefinanciero + " mayo ";
                                    fallofinanciero++;
                                }
                            }
                            //fin mayo


                            //inicio junio
                            if (metas.Rows[i]["IDFINSUB6"] == DBNull.Value)
                            {
                                estado = insertaMetasubPOA(Convert.ToInt32(cbAniPOA.Value), Convert.ToInt32(cbAniPOA.Text), Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"]), 6, Convert.ToDouble(e.NewValues["MFINSUB6"]), 1);
                                if (estado == 0)
                                {
                                    mensajefinanciero = mensajefinanciero + " junio ";
                                    fallofinanciero++;
                                }

                            }

                            else
                            {
                                estado = AcualizametasubPOA(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"]), Convert.ToInt32(cbAniPOA.Value), Convert.ToInt32(cbAniPOA.Text), Convert.ToDouble(e.NewValues["MFINSUB6"]), Convert.ToInt32(metas.Rows[i]["IDFINSUB6"]), 1, 6);

                                if (estado == 0)
                                {
                                    mensajefinanciero = mensajefinanciero + " junio ";
                                    fallofinanciero++;
                                }
                            }
                            //fin junio


                            //inicio julio
                            if (metas.Rows[i]["IDFINSUB7"] == DBNull.Value)
                            {
                                estado = insertaMetasubPOA(Convert.ToInt32(cbAniPOA.Value), Convert.ToInt32(cbAniPOA.Text), Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"]), 7, Convert.ToDouble(e.NewValues["MFINSUB7"]), 1);
                                if (estado == 0)
                                {
                                    mensajefinanciero = mensajefinanciero + " julio ";
                                    fallofinanciero++;
                                }

                            }

                            else
                            {
                                estado = AcualizametasubPOA(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"]), Convert.ToInt32(cbAniPOA.Value), Convert.ToInt32(cbAniPOA.Text), Convert.ToDouble(e.NewValues["MFINSUB7"]), Convert.ToInt32(metas.Rows[i]["IDFINSUB7"]), 1, 7);

                                if (estado == 0)
                                {
                                    mensajefinanciero = mensajefinanciero + " julio ";
                                    fallofinanciero++;
                                }
                            }
                            //fin julio

                            //inicio agosto
                            if (metas.Rows[i]["IDFINSUB8"] == DBNull.Value)
                            {
                                estado = insertaMetasubPOA(Convert.ToInt32(cbAniPOA.Value), Convert.ToInt32(cbAniPOA.Text), Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"]), 8, Convert.ToDouble(e.NewValues["MFINSUB8"]), 1);
                                if (estado == 0)
                                {
                                    mensajefinanciero = mensajefinanciero + " agosto ";
                                    fallofinanciero++;
                                }

                            }

                            else
                            {
                                estado = AcualizametasubPOA(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"]), Convert.ToInt32(cbAniPOA.Value), Convert.ToInt32(cbAniPOA.Text), Convert.ToDouble(e.NewValues["MFINSUB8"]), Convert.ToInt32(metas.Rows[i]["IDFINSUB8"]), 1, 8);

                                if (estado == 0)
                                {
                                    mensajefinanciero = mensajefinanciero + " agosto ";
                                    fallofinanciero++;
                                }
                            }
                            //fin agosto


                            //inicio septiembre
                            if (metas.Rows[i]["IDFINSUB9"] == DBNull.Value)
                            {
                                estado = insertaMetasubPOA(Convert.ToInt32(cbAniPOA.Value), Convert.ToInt32(cbAniPOA.Text), Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"]), 9, Convert.ToDouble(e.NewValues["MFINSUB9"]), 1);
                                if (estado == 0)
                                {
                                    mensajefinanciero = mensajefinanciero + " septiembre ";
                                    fallofinanciero++;
                                }

                            }

                            else
                            {
                                estado = AcualizametasubPOA(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"]), Convert.ToInt32(cbAniPOA.Value), Convert.ToInt32(cbAniPOA.Text), Convert.ToDouble(e.NewValues["MFINSUB9"]), Convert.ToInt32(metas.Rows[i]["IDFINSUB9"]), 1, 9);

                                if (estado == 0)
                                {
                                    mensajefinanciero = mensajefinanciero + " septiembre ";
                                    fallofinanciero++;
                                }
                            }
                            //fin septiembre

                            //inicio septiembre
                            if (metas.Rows[i]["IDFINSUB10"] == DBNull.Value)
                            {
                                estado = insertaMetasubPOA(Convert.ToInt32(cbAniPOA.Value), Convert.ToInt32(cbAniPOA.Text), Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"]), 10, Convert.ToDouble(e.NewValues["MFINSUB10"]), 1);
                                if (estado == 0)
                                {
                                    mensajefinanciero = mensajefinanciero + " octubre ";
                                    fallofinanciero++;
                                }

                            }

                            else
                            {
                                estado = AcualizametasubPOA(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"]), Convert.ToInt32(cbAniPOA.Value), Convert.ToInt32(cbAniPOA.Text), Convert.ToDouble(e.NewValues["MFINSUB10"]), Convert.ToInt32(metas.Rows[i]["IDFINSUB10"]), 1, 10);

                                if (estado == 0)
                                {
                                    mensajefinanciero = mensajefinanciero + " octubre ";
                                    fallofinanciero++;
                                }
                            }
                            //fin octubre


                            //inicio noviembre
                            if (metas.Rows[i]["IDFINSUB11"] == DBNull.Value)
                            {
                                estado = insertaMetasubPOA(Convert.ToInt32(cbAniPOA.Value), Convert.ToInt32(cbAniPOA.Text), Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"]), 11, Convert.ToDouble(e.NewValues["MFINSUB11"]), 1);
                                if (estado == 0)
                                {
                                    mensajefinanciero = mensajefinanciero + " noviembre ";
                                    fallofinanciero++;
                                }

                            }

                            else
                            {
                                estado = AcualizametasubPOA(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"]), Convert.ToInt32(cbAniPOA.Value), Convert.ToInt32(cbAniPOA.Text), Convert.ToDouble(e.NewValues["MFINSUB11"]), Convert.ToInt32(metas.Rows[i]["IDFINSUB11"]), 1, 11);

                                if (estado == 0)
                                {
                                    mensajefinanciero = mensajefinanciero + " noviembre ";
                                    fallofinanciero++;
                                }
                            }
                            //fin noviembre

                            //inicio diciembre
                            if (metas.Rows[i]["IDFINSUB12"] == DBNull.Value)
                            {
                                estado = insertaMetasubPOA(Convert.ToInt32(cbAniPOA.Value), Convert.ToInt32(cbAniPOA.Text), Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"]), 12, Convert.ToDouble(e.NewValues["MFINSUB12"]), 1);
                                if (estado == 0)
                                {
                                    mensajefinanciero = mensajefinanciero + " diciembre ";
                                    fallofinanciero++;
                                }

                            }

                            else
                            {
                                estado = AcualizametasubPOA(Convert.ToInt32(e.Keys["SPPSUB$ID_SUBPRODUCTO"]), Convert.ToInt32(cbAniPOA.Value), Convert.ToInt32(cbAniPOA.Text), Convert.ToDouble(e.NewValues["MFINSUB12"]), Convert.ToInt32(metas.Rows[i]["IDFINSUB12"]), 1, 12);

                                if (estado == 0)
                                {
                                    mensajefinanciero = mensajefinanciero + " diciembre ";
                                    fallofinanciero++;
                                }
                            }
                            //fin diciembre

                            //}
                            //}

                            if (fallofisico == 0 && fallofinanciero == 0 && fallotodo == 0)
                            {
                                mensaje = "Metas guadardas correctemente";
                                grid.JSProperties["cpError"] = "Información: " + mensaje;
                                e.Cancel = true;
                                grid.CancelEdit();
                                break;


                            }
                            else
                            {
                                if (fallotodo > 0)
                                {
                                    mensaje = "Error: " + mensaje;
                                    grid.JSProperties["cp2"] = mensaje;
                                    e.Cancel = true;
                                    grid.CancelEdit();
                                    break;
                                }


                                else
                                {
                                    grid.JSProperties["cpError"] = "Información: " + mensaje;
                                    e.Cancel = true;
                                    grid.CancelEdit();
                                    break;
                                }

                            }
                        }
                    }

                }
            }



        }//fin edita sub

        protected void gvPOAproductos_CommandButtonInitialize(object sender, ASPxGridViewCommandButtonEventArgs e)
        {
            Session["carga"] = 2;
            ASPxGridView grid = sender as ASPxGridView;
            int columna;
            if (e.VisibleIndex != -1)
            {
                if (grid.GetRowValues(e.VisibleIndex, "ESTADO") != DBNull.Value)
                {
                    columna = Convert.ToInt32(grid.GetRowValues(e.VisibleIndex, "ESTADO").ToString());
                    if (columna == 2 || Session["ROL"].ToString() == "ENTIDAD" || Convert.ToInt32(Session["abierto"]) == 1)
                    {
                        e.Visible = false;
                    }

                }



            }
        }

        protected void gvPOASubproductos_CommandButtonInitialize(object sender, ASPxGridViewCommandButtonEventArgs e)
        {
            Session["carga"] = 2;
            ASPxGridView grid = sender as ASPxGridView;
            int columna1;
            int columna2;
            if (e.VisibleIndex != -1)
            {
                if (grid.GetRowValues(e.VisibleIndex, "ESTADOFISICO") != DBNull.Value && grid.GetRowValues(e.VisibleIndex, "ESTADOFINANCIERO") != DBNull.Value)
                {
                    columna1 = Convert.ToInt32(grid.GetRowValues(e.VisibleIndex, "ESTADOFISICO").ToString());
                    columna2 = Convert.ToInt32(grid.GetRowValues(e.VisibleIndex, "ESTADOFINANCIERO").ToString());
                    if ((columna1 == 2 && columna2 == 2) || Session["ROL"].ToString() == "ENTIDAD" || Convert.ToInt32(Session["abierto"]) == 1)
                    {
                        e.Visible = false;
                    }

                }



            }
        }

        protected void bntEnviar_Click(object sender, EventArgs e)
        {

            if (Session["VINCULADOS"] != null && Convert.ToInt32(Session["VINCULADOS"]) > 0)
            {
                mensaje = "Tiene " + Convert.ToInt32(Session["VINCULADOS"]) + " productos vinculados a la anterior estructura de programa de gobierno 2020-2024, se recomienda actualizar la vinculación de estos productos a la estructura vigente de programa de gobierno 2024-2028, a un Resultado Estrátegico y/o resultado institucional";
                Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',3);", true);
            }

            //sql = "UPDATE SCHE$SIPLAN20.SP20$POA SET  SPOA$ESTADO = 'A', SPOA$FECHA_APROBACION = 'APROBACION = ' || TO_CHAR(SYSDATE, 'DD/MM/YYYY HH:MI') || ' ' || '" + Session["USUARIO"].ToString() + "',  SPOA$USUARIOAPROBACION =  '" + Session["USUARIO"].ToString() + "' WHERE SPOA$ID_POA = "+ Convert.ToInt32(cbAniPOA.Value)+ " AND SPOA$ANIO = "+ Convert.ToInt32(cbAniPOA.Text) + " AND SPOA$ID_POM = "+ Session["pom"] + " AND SPOA$ID_INSTITUCION = "+ Session["insto"];
            sql = "UPDATE SCHE$SIPLAN20.SP20$POA SET  SPOA$ESTADO = 'A', SPOA$FECHA_APROBACION = 'APROBACION = ' || TO_CHAR(SYSDATE, 'DD/MM/YYYY HH:MI') || ' ' || '" + Session["USUARIO"].ToString() + "',  SPOA$USUARIOAPROBACION =  '" + Session["USUARIO"].ToString() + "' WHERE SPOA$ID_POM = " + Session["pom"] + " AND SPOA$ID_INSTITUCION = " + Session["insto"];
            estado = dao.comando(sql);
            if (estado == 0)
            {
                mensaje = dao.mensaje;
                //ScriptManager.RegisterStartupScript(this.UpdatePanel2, GetType(), "script", "Alerta('" + mensaje + " <br/>',2);", true);
                Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + "<br/>',2);", true);
            }
            else
            {
                sql = "UPDATE SCHE$SIPLAN20.SP20$POM SET SPPO$ESTADO = 'A' WHERE SPPO$ID_POM = " + Session["pom"] + " AND SPPO$ID_INSTITUCION = " + Session["insto"];
                estado = dao.comando(sql);
                if (estado == 0)
                {
                    mensaje = dao.mensaje;
                    //ScriptManager.RegisterStartupScript(this.UpdatePanel2, GetType(), "script", "Alerta('" + mensaje + " <br/>',2);", true);
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + "<br/>',2);", true);
                }

                else

                {
                    sql = "SELECT P.SPP$ID_PERIODO, P.SPP$INICIO, P.SPP$FINAL, POM.SPPO$ID_POM, POM.SPPO$ID_INSTITUCION,P.SPP$ORDEN, CASE WHEN  POM.SPPO$ESTADO = 'D' THEN 'POM No enviado' ELSE 'POM Enviado' END AS ESTADO FROM SCHE$SIPLAN20.SP20$PERIODO P INNER JOIN SCHE$SIPLAN20.SP20$POM POM ON P.SPP$ID_PERIODO = POM.SPPO$ID_PERIODO AND P.SPP$RESTRICTIVA = 'N' AND POM.SPPO$RESTRICTIVA = 'N' WHERE POM.SPPO$ID_POM = " + Convert.ToInt32(Session["pom"]) + " AND POM.SPPO$ID_INSTITUCION = " + Convert.ToInt32(Convert.ToInt32(Session["insto"]));
                    estado = dao.consulta(sql);
                    periodo = dao.tabla;
                    lblTitulo.Text = Session["institucion"].ToString() + " " + "periodo: " + periodo.Rows[0]["SPP$INICIO"] + " - " + periodo.Rows[0]["SPP$FINAL"] + " " + periodo.Rows[0]["ESTADO"];
                    mensaje = "POM aproabado correctamente";
                    //ScriptManager.RegisterStartupScript(this.UpdatePanel2, GetType(), "script", "Alerta('" + mensaje + " <br/>',1);", true);
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + "<br/>',1);", true);
                    btnProgramas_Click(sender, e);


                }


            }


        }

        protected void btnResfrescar_Click(object sender, EventArgs e)
        {
            btnPOA_Click(sender, e);
        }

        protected void btnGenerar_Click(object sender, EventArgs e)
        {




            sql = "SELECT SPPO$AUTORIDAD, SPPO$CARGO  FROM SCHE$SIPLAN20.SP20$POM WHERE SPPO$ID_POM = " + Convert.ToInt32(Session["pom"].ToString());
            estado = dao.consulta(sql);

            if (estado == 1)
            {
                tabla = dao.tabla;
                if (tabla.Rows.Count > 0)
                {
                    if (tabla.Rows[0]["SPPO$AUTORIDAD"] != DBNull.Value)
                        txtAutoridad2.Text = tabla.Rows[0]["SPPO$AUTORIDAD"].ToString();
                    else
                        txtAutoridad2.Text = "";

                    if (tabla.Rows[0]["SPPO$CARGO"] != DBNull.Value)
                        txtCargo2.Text = tabla.Rows[0]["SPPO$CARGO"].ToString();
                    else
                        txtCargo2.Text = "";

                }
            }
            else
            {
                mensaje = dao.mensaje;
                //Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
                ScriptManager.RegisterStartupScript(UpdatePanel2, UpdatePanel2.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
            }

            popReportePOA.ShowOnPageLoad = true;



        }

        protected void gvProductos_CellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e)
        {
            Session["carga"] = 1;
            ASPxGridView grid = sender as ASPxGridView;
            if (e.Column.FieldName == "MFIN2020" || e.Column.FieldName == "MFIN2021" || e.Column.FieldName == "MFIN2022" || e.Column.FieldName == "MFIN2023" || e.Column.FieldName == "MFIN2024" || e.Column.FieldName == "MFIN2025" || e.Column.FieldName == "MFIN2026" || e.Column.FieldName == "MFIN2027" || e.Column.FieldName == "MFIN2028" || e.Column.FieldName == "MFIN2029")
            {
                e.Editor.ClientEnabled = false;
                e.Editor.BackColor = System.Drawing.Color.LightBlue;
                e.Editor.ForeColor = System.Drawing.Color.Blue;
                e.Editor.Font.Bold = true;
            }

            if (e.Column.FieldName == "MF2020")
            {
                if (grid.GetRowValues(Convert.ToInt32(e.VisibleIndex), "TP2020") != DBNull.Value)
                {
                    int valor = Convert.ToInt32(grid.GetRowValues(Convert.ToInt32(e.VisibleIndex), "TP2020").ToString());
                    if (valor == 1)
                    {
                        e.Editor.ClientEnabled = false;
                        e.Editor.BackColor = System.Drawing.Color.Blue;
                        e.Editor.ForeColor = System.Drawing.Color.White;
                        e.Editor.Font.Bold = true;
                    }

                }
                else if (Convert.ToInt32(Session["periodo"]) == 20 || Convert.ToInt32(Session["periodo"]) == 21 || Convert.ToInt32(Session["periodo"]) == 22 || Convert.ToInt32(Session["periodo"]) == 23 || Convert.ToInt32(Session["periodo"]) == 24 ||  Convert.ToInt32(Session["periodo"]) == 25)
                {
                    e.Editor.ClientEnabled = false;
                }
            }

            if (e.Column.FieldName == "MF2021")
            {
                if (grid.GetRowValues(Convert.ToInt32(e.VisibleIndex), "TP2021") != DBNull.Value)
                {
                    int valor = Convert.ToInt32(grid.GetRowValues(Convert.ToInt32(e.VisibleIndex), "TP2021").ToString());
                    if (valor == 1)
                    {
                        e.Editor.ClientEnabled = false;
                        e.Editor.BackColor = System.Drawing.Color.Blue;
                        e.Editor.ForeColor = System.Drawing.Color.White;
                        e.Editor.Font.Bold = true;
                    }

                }

                else if (Convert.ToInt32(Session["periodo"]) == 21 || Convert.ToInt32(Session["periodo"]) == 22 || Convert.ToInt32(Session["periodo"]) == 23 || Convert.ToInt32(Session["periodo"]) == 24 || Convert.ToInt32(Session["periodo"]) == 25)
                {
                    e.Editor.ClientEnabled = false;
                }

            }

            if (e.Column.FieldName == "MF2022")
            {
                if (grid.GetRowValues(Convert.ToInt32(e.VisibleIndex), "TP2022") != DBNull.Value)
                {
                    int valor = Convert.ToInt32(grid.GetRowValues(Convert.ToInt32(e.VisibleIndex), "TP2022").ToString());
                    if (valor == 1)
                    {
                        e.Editor.ClientEnabled = false;
                        e.Editor.BackColor = System.Drawing.Color.Blue;
                        e.Editor.ForeColor = System.Drawing.Color.White;
                        e.Editor.Font.Bold = true;
                    }

                }
            }

            if (e.Column.FieldName == "MF2023")
            {
                if (grid.GetRowValues(Convert.ToInt32(e.VisibleIndex), "TP2023") != DBNull.Value)
                {
                    int valor = Convert.ToInt32(grid.GetRowValues(Convert.ToInt32(e.VisibleIndex), "TP2023").ToString());
                    if (valor == 1)
                    {
                        e.Editor.ClientEnabled = false;
                        e.Editor.BackColor = System.Drawing.Color.Blue;
                        e.Editor.ForeColor = System.Drawing.Color.White;
                        e.Editor.Font.Bold = true;
                    }

                }
            }

            if (e.Column.FieldName == "MF2024")
            {
                if (grid.GetRowValues(Convert.ToInt32(e.VisibleIndex), "TP2024") != DBNull.Value)
                {
                    int valor = Convert.ToInt32(grid.GetRowValues(Convert.ToInt32(e.VisibleIndex), "TP2024").ToString());
                    if (valor == 1)
                    {
                        e.Editor.ClientEnabled = false;
                        e.Editor.BackColor = System.Drawing.Color.Blue;
                        e.Editor.ForeColor = System.Drawing.Color.White;
                        e.Editor.Font.Bold = true;
                    }

                }
            }


            if (e.Column.FieldName == "MF2025")
            {
                if (grid.GetRowValues(Convert.ToInt32(e.VisibleIndex), "TP2025") != DBNull.Value)
                {
                    int valor = Convert.ToInt32(grid.GetRowValues(Convert.ToInt32(e.VisibleIndex), "TP2025").ToString());
                    if (valor == 1)
                    {
                        e.Editor.ClientEnabled = false;
                        e.Editor.BackColor = System.Drawing.Color.Blue;
                        e.Editor.ForeColor = System.Drawing.Color.White;
                        e.Editor.Font.Bold = true;
                    }

                }


                else if (Convert.ToInt32(Session["periodo"]) == 0 || Convert.ToInt32(Session["periodo"]) == 25)
                {

                    e.Editor.ClientEnabled = false;



                }
            }


            if (e.Column.FieldName == "MF2026")
            {
                if (grid.GetRowValues(Convert.ToInt32(e.VisibleIndex), "TP2026") != DBNull.Value)
                {
                    int valor = Convert.ToInt32(grid.GetRowValues(Convert.ToInt32(e.VisibleIndex), "TP2026").ToString());
                    if (valor == 1)
                    {
                        e.Editor.ClientEnabled = false;
                        e.Editor.BackColor = System.Drawing.Color.Blue;
                        e.Editor.ForeColor = System.Drawing.Color.White;
                        e.Editor.Font.Bold = true;
                    }

                }


                else if (Convert.ToInt32(Session["periodo"]) == 0 || Convert.ToInt32(Session["periodo"]) == 20)
                {

                    e.Editor.ClientEnabled = false;



                }
            }


            if (e.Column.FieldName == "MF2027")
            {
                int maiden = Convert.ToInt32(Session["periodo"]);
                if (grid.GetRowValues(Convert.ToInt32(e.VisibleIndex), "TP2027") != DBNull.Value)
                {
                    int valor = Convert.ToInt32(grid.GetRowValues(Convert.ToInt32(e.VisibleIndex), "TP2027").ToString());
                    if (valor == 1)
                    {
                        e.Editor.ClientEnabled = false;
                        e.Editor.BackColor = System.Drawing.Color.Blue;
                        e.Editor.ForeColor = System.Drawing.Color.White;
                        e.Editor.Font.Bold = true;
                    }

                }


                else if (maiden != 22 && maiden != 23 && maiden != 24 && maiden != 25)
                {

                    e.Editor.ClientEnabled = false;



                }
            }



            if (e.Column.FieldName == "MF2028")
            {
                if (grid.GetRowValues(Convert.ToInt32(e.VisibleIndex), "TP2028") != DBNull.Value)
                {
                    int valor = Convert.ToInt32(grid.GetRowValues(Convert.ToInt32(e.VisibleIndex), "TP2028").ToString());
                    if (valor == 1)
                    {
                        e.Editor.ClientEnabled = false;
                        e.Editor.BackColor = System.Drawing.Color.Blue;
                        e.Editor.ForeColor = System.Drawing.Color.White;
                        e.Editor.Font.Bold = true;
                    }

                }


                else if (Convert.ToInt32(Session["periodo"]) != 23 && Convert.ToInt32(Session["periodo"]) != 24 && Convert.ToInt32(Session["periodo"]) != 25)
                {

                    e.Editor.ClientEnabled = false;



                }
            }


            if (e.Column.FieldName == "MF2029")
            {
                if (grid.GetRowValues(Convert.ToInt32(e.VisibleIndex), "TP2029") != DBNull.Value)
                {
                    int valor = Convert.ToInt32(grid.GetRowValues(Convert.ToInt32(e.VisibleIndex), "TP2029").ToString());
                    if (valor == 1)
                    {
                        e.Editor.ClientEnabled = false;
                        e.Editor.BackColor = System.Drawing.Color.Blue;
                        e.Editor.ForeColor = System.Drawing.Color.White;
                        e.Editor.Font.Bold = true;
                    }

                }


                if (Convert.ToInt32(Session["periodo"]) == 24 || Convert.ToInt32(Session["periodo"]) == 25)               

                    e.Editor.ClientEnabled = true;                
                else
                    e.Editor.ClientEnabled = false;

            }

            if (e.Column.FieldName == "MF2030")
            {
                if (grid.GetRowValues(Convert.ToInt32(e.VisibleIndex), "TP2030") != DBNull.Value)
                {
                    int valor = Convert.ToInt32(grid.GetRowValues(Convert.ToInt32(e.VisibleIndex), "TP2030").ToString());
                    if (valor == 1)
                    {
                        e.Editor.ClientEnabled = false;
                        e.Editor.BackColor = System.Drawing.Color.Blue;
                        e.Editor.ForeColor = System.Drawing.Color.White;
                        e.Editor.Font.Bold = true;
                    }

                }


                if (Convert.ToInt32(Session["periodo"]) != 25)
                {

                    e.Editor.ClientEnabled = false;



                }
            }

            if (e.Column.FieldName == "MFIN2025")
            {
                if (Convert.ToInt32(Session["periodo"]) != 20 || Convert.ToInt32(Session["periodo"]) != 21 || Convert.ToInt32(Session["periodo"]) != 25)
                {

                    e.Editor.ClientEnabled = false;



                }
            }


            if (e.Column.FieldName == "MFIN2026")
            {
                if (Convert.ToInt32(Session["periodo"]) != 21 && Convert.ToInt32(Session["periodo"]) != 22 && Convert.ToInt32(Session["periodo"]) != 23 && Convert.ToInt32(Session["periodo"]) != 24 && Convert.ToInt32(Session["periodo"]) != 25) 
                {

                    e.Editor.ClientEnabled = false;



                }
            }

            if (e.Column.FieldName == "MFIN2027")
            {
                if (Convert.ToInt32(Session["periodo"]) != 22 || Convert.ToInt32(Session["periodo"]) != 23 || Convert.ToInt32(Session["periodo"]) != 24 || Convert.ToInt32(Session["periodo"]) != 25)
                {

                    e.Editor.ClientEnabled = false;



                }
            }

            if (e.Column.FieldName == "MFIN2028")
            {
                if (Convert.ToInt32(Session["periodo"]) != 23 && Convert.ToInt32(Session["periodo"]) != 24 && Convert.ToInt32(Session["periodo"]) != 25)
                {

                    e.Editor.ClientEnabled = false;



                }
            }

            if (e.Column.FieldName == "MFIN2029")
            {
                if (Convert.ToInt32(Session["periodo"]) != 24 || Convert.ToInt32(Session["periodo"]) != 25)
                {

                    e.Editor.ClientEnabled = false;



                }
            }

            if (e.Column.FieldName == "MFIN2030")
            {
                if (Convert.ToInt32(Session["periodo"]) != 25)
                {

                    e.Editor.ClientEnabled = false;



                }
            }

        }

        protected void gvPOAproductos_CellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e)
        {
            Session["carga"] = 2;
            if (e.Column.FieldName == "MFPRODANUAL" || e.Column.FieldName == "MFINPRODANUAL" || e.Column.FieldName == "MFINMES1" || e.Column.FieldName == "MFINMES2" || e.Column.FieldName == "MFINMES3" || e.Column.FieldName == "MFINMES4" || e.Column.FieldName == "MFINMES4" || e.Column.FieldName == "MFINMES5" || e.Column.FieldName == "MFINMES6" || e.Column.FieldName == "MFINMES7" || e.Column.FieldName == "MFINMES8" || e.Column.FieldName == "MFINMES9" || e.Column.FieldName == "MFINMES10" || e.Column.FieldName == "MFINMES11" || e.Column.FieldName == "MFINMES12" || e.Column.FieldName == "CUAFIS1" || e.Column.FieldName == "CUAFINPROD1" || e.Column.FieldName == "CUAFIS2" || e.Column.FieldName == "CUAFINPROD2" || e.Column.FieldName == "CUAFIS3" || e.Column.FieldName == "CUAFINPROD3")
            {
                e.Editor.ClientEnabled = false;
                e.Editor.BackColor = System.Drawing.Color.LightBlue;
                e.Editor.ForeColor = System.Drawing.Color.Blue;
                e.Editor.Font.Bold = true;
            }
        }

        protected void gvPOASubproductos_CellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e)
        {
            Session["carga"] = 2;
            if (e.Column.FieldName == "ANUALFISICO" || e.Column.FieldName == "ANUALFINANCIERO" || e.Column.FieldName == "MFCUA1" || e.Column.FieldName == "MFINCUA1" || e.Column.FieldName == "MFCUA2" || e.Column.FieldName == "MFINCUA2" || e.Column.FieldName == "MFCUA3" || e.Column.FieldName == "MFINCUA3")
            {
                e.Editor.ClientEnabled = false;
                e.Editor.BackColor = System.Drawing.Color.LightBlue;
                e.Editor.ForeColor = System.Drawing.Color.Blue;
                e.Editor.Font.Bold = true;
            }
        }

        protected void CallbackControl_Callback(object source, DevExpress.Web.CallbackEventArgs e)
        {
            Exception innerException = new Exception("NoReport");
            throw new Exception("This Exception is thrown to demonstrate the ASPxWebControl.CallbackError Event.", innerException);
        }


        protected void gvResultados_BeforePerformDataSelect(object sender, EventArgs e)
        {
            Session["carga"] = 1;
            ASPxGridView gvDetail = sender as ASPxGridView;
            DataTable resultados = new DataTable();
            Double programa;
            programa = Convert.ToDouble((sender as ASPxGridView).GetMasterRowFieldValues("SPPRO$ID_PROGRAMA_PRESUPUESTO"));

            resultados = cargaResultados(programa, Convert.ToInt32(cbTipoProduccion.Value));

            if (Convert.ToInt32(resultados.Rows[0]["TIPO"]) == 0 || Convert.ToInt32(resultados.Rows[0]["TIPO"]) == 1)
            {
                gvDetail.Columns["RESULTADO"].Visible = true;
                gvDetail.Columns["EJE_ACCION"].Visible = true;


                gvDetail.Columns["RED"].Visible = false;


            }
            else if (Convert.ToInt32(resultados.Rows[0]["TIPO"]) == 2)
            {
                gvDetail.Columns["RESULTADO"].Visible = false;
                gvDetail.Columns["EJE_ACCION"].Visible = true;

                gvDetail.Columns["RED"].Visible = true;

            }

            gvDetail.DataSource = resultados;







        }

        protected void gvResultadosPOA_BeforePerformDataSelect(object sender, EventArgs e)
        {
            Session["carga"] = 2;
            ASPxGridView gvDetail = sender as ASPxGridView;
            DataTable resultados = new DataTable();
            Double programa;
            programa = Convert.ToDouble((sender as ASPxGridView).GetMasterRowFieldValues("SPPRO$ID_PROGRAMA_PRESUPUESTO"));
            resultados = cargaResultados(programa, Convert.ToInt32(cbTipoProd.Value));

            if (Convert.ToInt32(resultados.Rows[0]["TIPO"]) == 0 || Convert.ToInt32(resultados.Rows[0]["TIPO"]) == 1)
            {
                gvDetail.Columns["RESULTADO"].Visible = true;
                gvDetail.Columns["EJE_ACCION"].Visible = true;
                gvDetail.Columns["RED"].Visible = false;
            }
            else if (Convert.ToInt32(resultados.Rows[0]["TIPO"]) == 2)
            {
                gvDetail.Columns["RESULTADO"].Visible = false;
                gvDetail.Columns["EJE_ACCION"].Visible = true;
                gvDetail.Columns["RED"].Visible = true;
            }


            gvDetail.DataSource = resultados;
        }

        protected void gvProduccion_DetailRowExpandedChanged(object sender, ASPxGridViewDetailRowEventArgs e)
        {
            Session["carga"] = 1;

            int posi;

            try
            {
                posi = Convert.ToInt32(e.VisibleIndex.ToString());
                Session["indexProduccion"] = posi;
                gvProduccion.FocusedRowIndex = posi;

            }
            catch (Exception error)
            {
                posi = -1;
                Session["indexProduccion"] = posi;
            }
        }

        protected void gvResultados_DetailRowExpandedChanged(object sender, ASPxGridViewDetailRowEventArgs e)
        {
            Session["carga"] = 1;
            ASPxGridView gvDetail = sender as ASPxGridView;
            int posi;

            try
            {
                posi = Convert.ToInt32(e.VisibleIndex.ToString());
                Session["indexResultados"] = posi;
                gvDetail.FocusedRowIndex = posi;

            }
            catch (Exception error)
            {
                posi = -1;
                Session["indexResultados"] = posi;
            }

        }

        protected void gvProductos_DetailRowExpandedChanged(object sender, ASPxGridViewDetailRowEventArgs e)
        {
            ASPxGridView gvDetail = sender as ASPxGridView;
            int posi;
            Session["carga"] = 1;
            try
            {
                posi = Convert.ToInt32(e.VisibleIndex.ToString());
                Session["indexProductos"] = posi;
                gvDetail.FocusedRowIndex = posi;

            }
            catch (Exception error)
            {
                posi = -1;
                Session["indexProductos"] = posi;
            }
        }

        protected void gvPOA_DetailRowExpandedChanged(object sender, ASPxGridViewDetailRowEventArgs e)
        {
            Session["carga"] = 2;
            int posi;

            try
            {
                posi = Convert.ToInt32(e.VisibleIndex.ToString());
                Session["indexPOA"] = posi;
                gvPOA.FocusedRowIndex = posi;

            }
            catch (Exception error)
            {
                posi = -1;
                Session["indexPOA"] = posi;
            }
        }

        protected void gvResultadosPOA_DetailRowExpandedChanged(object sender, ASPxGridViewDetailRowEventArgs e)
        {
            Session["carga"] = 2;
            ASPxGridView gvDetail = sender as ASPxGridView;
            int posi;

            try
            {
                posi = Convert.ToInt32(e.VisibleIndex.ToString());
                Session["indexPOAResultados"] = posi;
                gvDetail.FocusedRowIndex = posi;

            }
            catch (Exception error)
            {
                posi = -1;
                Session["indexPOAResultados"] = posi;
            }
        }

        protected void gvPOAproductos_DetailRowExpandedChanged(object sender, ASPxGridViewDetailRowEventArgs e)
        {
            Session["carga"] = 2;
            ASPxGridView gvDetail = sender as ASPxGridView;
            int posi;

            try
            {
                posi = Convert.ToInt32(e.VisibleIndex.ToString());
                Session["indexPOAProductos"] = posi;
                gvDetail.FocusedRowIndex = posi;

            }
            catch (Exception error)
            {
                posi = -1;
                Session["indexPOAProductos"] = posi;
            }
        }

        protected void gvProgramas_CommandButtonInitialize(object sender, ASPxGridViewCommandButtonEventArgs e)
        {
            if (e.VisibleIndex != -1)
            {
                if (Session["ROL"].ToString() == "ENTIDAD" || Convert.ToInt32(Session["abierto"]) == 1)
                {

                    e.Visible = false;


                }



            }
        }

        protected void gvProductos_CommandButtonInitialize(object sender, ASPxGridViewCommandButtonEventArgs e)
        {
            ASPxGridView grid = sender as ASPxGridView;
            Session["carga"] = 1;
            if (e.VisibleIndex != -1)
            {

                if (Session["ROL"].ToString() == "ENTIDAD" || Convert.ToInt32(Session["abierto"]) == 1)
                {
                    e.Visible = false;
                }




            }
        }

        protected void gvsubProductos_CommandButtonInitialize(object sender, ASPxGridViewCommandButtonEventArgs e)
        {
            ASPxGridView grid = sender as ASPxGridView;
            Session["carga"] = 1;
            if (e.VisibleIndex != -1)
            {

                if (Session["ROL"].ToString() == "ENTIDAD" || Convert.ToInt32(Session["abierto"]) == 1)
                {
                    e.Visible = false;
                }




            }
        }

        protected void gvProductos_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            Session["carga"] = 1;
            ASPxGridView grid = sender as ASPxGridView;
            if (e.DataColumn.FieldName == "MF2020")
            {
                if (grid.GetRowValues(Convert.ToInt32(e.VisibleIndex), "TP2020") != DBNull.Value)
                {
                    int valor = Convert.ToInt32(grid.GetRowValues(Convert.ToInt32(e.VisibleIndex), "TP2020").ToString());
                    if (valor == 1)
                    {
                        e.Cell.BackColor = System.Drawing.Color.Green;
                        e.Cell.ForeColor = System.Drawing.Color.White;
                    }

                }

            }

            if (e.DataColumn.FieldName == "MF2021")
            {
                if (grid.GetRowValues(Convert.ToInt32(e.VisibleIndex), "TP2021") != DBNull.Value)
                {
                    int valor = Convert.ToInt32(grid.GetRowValues(Convert.ToInt32(e.VisibleIndex), "TP2021").ToString());
                    if (valor == 1)
                    {
                        e.Cell.BackColor = System.Drawing.Color.Green;
                        e.Cell.ForeColor = System.Drawing.Color.White;
                    }

                }

            }


            if (e.DataColumn.FieldName == "MF2022")
            {
                if (grid.GetRowValues(Convert.ToInt32(e.VisibleIndex), "TP2022") != DBNull.Value)
                {
                    int valor = Convert.ToInt32(grid.GetRowValues(Convert.ToInt32(e.VisibleIndex), "TP2022").ToString());
                    if (valor == 1)
                    {
                        e.Cell.BackColor = System.Drawing.Color.Green;
                        e.Cell.ForeColor = System.Drawing.Color.White;
                    }

                }

            }

            if (e.DataColumn.FieldName == "MF2023")
            {
                if (grid.GetRowValues(Convert.ToInt32(e.VisibleIndex), "TP2023") != DBNull.Value)
                {
                    int valor = Convert.ToInt32(grid.GetRowValues(Convert.ToInt32(e.VisibleIndex), "TP2023").ToString());
                    if (valor == 1)
                    {
                        e.Cell.BackColor = System.Drawing.Color.Green;
                        e.Cell.ForeColor = System.Drawing.Color.White;
                    }

                }

            }

            if (e.DataColumn.FieldName == "MF2024")
            {
                if (grid.GetRowValues(Convert.ToInt32(e.VisibleIndex), "TP2024") != DBNull.Value)
                {
                    int valor = Convert.ToInt32(grid.GetRowValues(Convert.ToInt32(e.VisibleIndex), "TP2024").ToString());
                    if (valor == 1)
                    {
                        e.Cell.BackColor = System.Drawing.Color.Green;
                        e.Cell.ForeColor = System.Drawing.Color.White;
                    }

                }

            }

            if (e.DataColumn.FieldName == "MF2025")
            {
                if (grid.GetRowValues(Convert.ToInt32(e.VisibleIndex), "TP2025") != DBNull.Value)
                {
                    int valor = Convert.ToInt32(grid.GetRowValues(Convert.ToInt32(e.VisibleIndex), "TP2025").ToString());
                    if (valor == 1)
                    {
                        e.Cell.BackColor = System.Drawing.Color.Green;
                        e.Cell.ForeColor = System.Drawing.Color.White;
                    }

                }

            }


            if (e.DataColumn.FieldName == "MF2026")
            {
                if (grid.GetRowValues(Convert.ToInt32(e.VisibleIndex), "TP2026") != DBNull.Value)
                {
                    int valor = Convert.ToInt32(grid.GetRowValues(Convert.ToInt32(e.VisibleIndex), "TP2026").ToString());
                    if (valor == 1)
                    {
                        e.Cell.BackColor = System.Drawing.Color.Green;
                        e.Cell.ForeColor = System.Drawing.Color.White;
                    }

                }

            }


            if (e.DataColumn.FieldName == "MF2027")
            {
                if (grid.GetRowValues(Convert.ToInt32(e.VisibleIndex), "TP2027") != DBNull.Value)
                {
                    int valor = Convert.ToInt32(grid.GetRowValues(Convert.ToInt32(e.VisibleIndex), "TP2027").ToString());
                    if (valor == 1)
                    {
                        e.Cell.BackColor = System.Drawing.Color.Green;
                        e.Cell.ForeColor = System.Drawing.Color.White;
                    }

                }

            }


            if (e.DataColumn.FieldName == "MF2028")
            {
                if (grid.GetRowValues(Convert.ToInt32(e.VisibleIndex), "TP2028") != DBNull.Value)
                {
                    int valor = Convert.ToInt32(grid.GetRowValues(Convert.ToInt32(e.VisibleIndex), "TP2028").ToString());
                    if (valor == 1)
                    {
                        e.Cell.BackColor = System.Drawing.Color.Green;
                        e.Cell.ForeColor = System.Drawing.Color.White;
                    }

                }

            }


            if (e.DataColumn.FieldName == "MF2029")
            {
                if (grid.GetRowValues(Convert.ToInt32(e.VisibleIndex), "TP2029") != DBNull.Value)
                {
                    int valor = Convert.ToInt32(grid.GetRowValues(Convert.ToInt32(e.VisibleIndex), "TP2029").ToString());
                    if (valor == 1)
                    {
                        e.Cell.BackColor = System.Drawing.Color.Green;
                        e.Cell.ForeColor = System.Drawing.Color.White;
                    }

                }

            }

            if (e.DataColumn.FieldName == "MF2030")
            {
                if (grid.GetRowValues(Convert.ToInt32(e.VisibleIndex), "TP2030") != DBNull.Value)
                {
                    int valor = Convert.ToInt32(grid.GetRowValues(Convert.ToInt32(e.VisibleIndex), "TP2030").ToString());
                    if (valor == 1)
                    {
                        e.Cell.BackColor = System.Drawing.Color.Green;
                        e.Cell.ForeColor = System.Drawing.Color.White;
                    }

                }

            }

        }

        protected void gvsubProductos_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            Session["carga"] = 1;
            ASPxGridView grid = sender as ASPxGridView;
            if (e.DataColumn.FieldName == "MFSUB2020")
            {
                if (grid.GetRowValues(Convert.ToInt32(e.VisibleIndex), "TSF2020") != DBNull.Value)
                {
                    int valor = Convert.ToInt32(grid.GetRowValues(Convert.ToInt32(e.VisibleIndex), "TSF2020").ToString());
                    if (valor == 1)
                    {
                        e.Cell.BackColor = System.Drawing.Color.Green;
                        e.Cell.ForeColor = System.Drawing.Color.White;
                    }

                }

            }

            if (e.DataColumn.FieldName == "MFINSUB2020")
            {
                if (grid.GetRowValues(Convert.ToInt32(e.VisibleIndex), "TSFIN2020") != DBNull.Value)
                {
                    int valor = Convert.ToInt32(grid.GetRowValues(Convert.ToInt32(e.VisibleIndex), "TSFIN2020").ToString());
                    if (valor == 1)
                    {
                        e.Cell.BackColor = System.Drawing.Color.Green;
                        e.Cell.ForeColor = System.Drawing.Color.White;
                    }

                }

            }



            if (e.DataColumn.FieldName == "MFSUB2021")
            {
                if (grid.GetRowValues(Convert.ToInt32(e.VisibleIndex), "TSF2021") != DBNull.Value)
                {
                    int valor = Convert.ToInt32(grid.GetRowValues(Convert.ToInt32(e.VisibleIndex), "TSF2021").ToString());
                    if (valor == 1)
                    {
                        e.Cell.BackColor = System.Drawing.Color.Green;
                        e.Cell.ForeColor = System.Drawing.Color.White;
                    }

                }

            }

            if (e.DataColumn.FieldName == "MFINSUB2021")
            {
                if (grid.GetRowValues(Convert.ToInt32(e.VisibleIndex), "TSFIN2021") != DBNull.Value)
                {
                    int valor = Convert.ToInt32(grid.GetRowValues(Convert.ToInt32(e.VisibleIndex), "TSFIN2021").ToString());
                    if (valor == 1)
                    {
                        e.Cell.BackColor = System.Drawing.Color.Green;
                        e.Cell.ForeColor = System.Drawing.Color.White;
                    }

                }

            }



            if (e.DataColumn.FieldName == "MFSUB2022")
            {
                if (grid.GetRowValues(Convert.ToInt32(e.VisibleIndex), "TSF2022") != DBNull.Value)
                {
                    int valor = Convert.ToInt32(grid.GetRowValues(Convert.ToInt32(e.VisibleIndex), "TSF2022").ToString());
                    if (valor == 1)
                    {
                        e.Cell.BackColor = System.Drawing.Color.Green;
                        e.Cell.ForeColor = System.Drawing.Color.White;
                    }

                }

            }

            if (e.DataColumn.FieldName == "MFINSUB2022")
            {
                if (grid.GetRowValues(Convert.ToInt32(e.VisibleIndex), "TSFIN2022") != DBNull.Value)
                {
                    int valor = Convert.ToInt32(grid.GetRowValues(Convert.ToInt32(e.VisibleIndex), "TSFIN2022").ToString());
                    if (valor == 1)
                    {
                        e.Cell.BackColor = System.Drawing.Color.Green;
                        e.Cell.ForeColor = System.Drawing.Color.White;
                    }

                }

            }


            if (e.DataColumn.FieldName == "MFSUB2023")
            {
                if (grid.GetRowValues(Convert.ToInt32(e.VisibleIndex), "TSF2023") != DBNull.Value)
                {
                    int valor = Convert.ToInt32(grid.GetRowValues(Convert.ToInt32(e.VisibleIndex), "TSF2023").ToString());
                    if (valor == 1)
                    {
                        e.Cell.BackColor = System.Drawing.Color.Green;
                        e.Cell.ForeColor = System.Drawing.Color.White;
                    }

                }

            }

            if (e.DataColumn.FieldName == "MFINSUB2023")
            {
                if (grid.GetRowValues(Convert.ToInt32(e.VisibleIndex), "TSFIN2023") != DBNull.Value)
                {
                    int valor = Convert.ToInt32(grid.GetRowValues(Convert.ToInt32(e.VisibleIndex), "TSFIN2023").ToString());
                    if (valor == 1)
                    {
                        e.Cell.BackColor = System.Drawing.Color.Green;
                        e.Cell.ForeColor = System.Drawing.Color.White;
                    }

                }

            }


            if (e.DataColumn.FieldName == "MFSUB2024")
            {
                if (grid.GetRowValues(Convert.ToInt32(e.VisibleIndex), "TSF2024") != DBNull.Value)
                {
                    int valor = Convert.ToInt32(grid.GetRowValues(Convert.ToInt32(e.VisibleIndex), "TSF2024").ToString());
                    if (valor == 1)
                    {
                        e.Cell.BackColor = System.Drawing.Color.Green;
                        e.Cell.ForeColor = System.Drawing.Color.White;
                    }

                }

            }

            if (e.DataColumn.FieldName == "MFINSUB2024")
            {
                if (grid.GetRowValues(Convert.ToInt32(e.VisibleIndex), "TSFIN2024") != DBNull.Value)
                {
                    int valor = Convert.ToInt32(grid.GetRowValues(Convert.ToInt32(e.VisibleIndex), "TSFIN2024").ToString());
                    if (valor == 1)
                    {
                        e.Cell.BackColor = System.Drawing.Color.Green;
                        e.Cell.ForeColor = System.Drawing.Color.White;
                    }

                }

            }



            if (e.DataColumn.FieldName == "MFSUB2025")
            {
                if (grid.GetRowValues(Convert.ToInt32(e.VisibleIndex), "TSF2025") != DBNull.Value)
                {
                    int valor = Convert.ToInt32(grid.GetRowValues(Convert.ToInt32(e.VisibleIndex), "TSF2025").ToString());
                    if (valor == 1)
                    {
                        e.Cell.BackColor = System.Drawing.Color.Green;
                        e.Cell.ForeColor = System.Drawing.Color.White;
                    }

                }

            }

            if (e.DataColumn.FieldName == "MFINSUB2025")
            {
                if (grid.GetRowValues(Convert.ToInt32(e.VisibleIndex), "TSFIN2025") != DBNull.Value)
                {
                    int valor = Convert.ToInt32(grid.GetRowValues(Convert.ToInt32(e.VisibleIndex), "TSFIN2025").ToString());
                    if (valor == 1)
                    {
                        e.Cell.BackColor = System.Drawing.Color.Green;
                        e.Cell.ForeColor = System.Drawing.Color.White;
                    }

                }

            }



            if (e.DataColumn.FieldName == "MFSUB2026")
            {
                if (grid.GetRowValues(Convert.ToInt32(e.VisibleIndex), "TSF2026") != DBNull.Value)
                {
                    int valor = Convert.ToInt32(grid.GetRowValues(Convert.ToInt32(e.VisibleIndex), "TSF2026").ToString());
                    if (valor == 1)
                    {
                        e.Cell.BackColor = System.Drawing.Color.Green;
                        e.Cell.ForeColor = System.Drawing.Color.White;
                    }

                }

            }

            if (e.DataColumn.FieldName == "MFINSUB2026")
            {
                if (grid.GetRowValues(Convert.ToInt32(e.VisibleIndex), "TSFIN2026") != DBNull.Value)
                {
                    int valor = Convert.ToInt32(grid.GetRowValues(Convert.ToInt32(e.VisibleIndex), "TSFIN2026").ToString());
                    if (valor == 1)
                    {
                        e.Cell.BackColor = System.Drawing.Color.Green;
                        e.Cell.ForeColor = System.Drawing.Color.White;
                    }

                }

            }




            if (e.DataColumn.FieldName == "MFSUB2027")
            {
                if (grid.GetRowValues(Convert.ToInt32(e.VisibleIndex), "TSF2027") != DBNull.Value)
                {
                    int valor = Convert.ToInt32(grid.GetRowValues(Convert.ToInt32(e.VisibleIndex), "TSF2027").ToString());
                    if (valor == 1)
                    {
                        e.Cell.BackColor = System.Drawing.Color.Green;
                        e.Cell.ForeColor = System.Drawing.Color.White;
                    }

                }

            }

            if (e.DataColumn.FieldName == "MFINSUB2027")
            {
                if (grid.GetRowValues(Convert.ToInt32(e.VisibleIndex), "TSFIN2027") != DBNull.Value)
                {
                    int valor = Convert.ToInt32(grid.GetRowValues(Convert.ToInt32(e.VisibleIndex), "TSFIN2027").ToString());
                    if (valor == 1)
                    {
                        e.Cell.BackColor = System.Drawing.Color.Green;
                        e.Cell.ForeColor = System.Drawing.Color.White;
                    }

                }

            }



            if (e.DataColumn.FieldName == "MFSUB2028")
            {
                if (grid.GetRowValues(Convert.ToInt32(e.VisibleIndex), "TSF2028") != DBNull.Value)
                {
                    int valor = Convert.ToInt32(grid.GetRowValues(Convert.ToInt32(e.VisibleIndex), "TSF2028").ToString());
                    if (valor == 1)
                    {
                        e.Cell.BackColor = System.Drawing.Color.Green;
                        e.Cell.ForeColor = System.Drawing.Color.White;
                    }

                }

            }

            if (e.DataColumn.FieldName == "MFINSUB2028")
            {
                if (grid.GetRowValues(Convert.ToInt32(e.VisibleIndex), "TSFIN2028") != DBNull.Value)
                {
                    int valor = Convert.ToInt32(grid.GetRowValues(Convert.ToInt32(e.VisibleIndex), "TSFIN2028").ToString());
                    if (valor == 1)
                    {
                        e.Cell.BackColor = System.Drawing.Color.Green;
                        e.Cell.ForeColor = System.Drawing.Color.White;
                    }

                }

            }




            if (e.DataColumn.FieldName == "MFSUB2029")
            {
                if (grid.GetRowValues(Convert.ToInt32(e.VisibleIndex), "TSF2029") != DBNull.Value)
                {
                    int valor = Convert.ToInt32(grid.GetRowValues(Convert.ToInt32(e.VisibleIndex), "TSF2029").ToString());
                    if (valor == 1)
                    {
                        e.Cell.BackColor = System.Drawing.Color.Green;
                        e.Cell.ForeColor = System.Drawing.Color.White;
                    }

                }

            }

            if (e.DataColumn.FieldName == "MFINSUB2029")
            {
                if (grid.GetRowValues(Convert.ToInt32(e.VisibleIndex), "TSFIN2029") != DBNull.Value)
                {
                    int valor = Convert.ToInt32(grid.GetRowValues(Convert.ToInt32(e.VisibleIndex), "TSFIN2029").ToString());
                    if (valor == 1)
                    {
                        e.Cell.BackColor = System.Drawing.Color.Green;
                        e.Cell.ForeColor = System.Drawing.Color.White;
                    }

                }

            }



            if (e.DataColumn.FieldName == "MFSUB2030")
            {
                if (grid.GetRowValues(Convert.ToInt32(e.VisibleIndex), "TSF2030") != DBNull.Value)
                {
                    int valor = Convert.ToInt32(grid.GetRowValues(Convert.ToInt32(e.VisibleIndex), "TSF2030").ToString());
                    if (valor == 1)
                    {
                        e.Cell.BackColor = System.Drawing.Color.Green;
                        e.Cell.ForeColor = System.Drawing.Color.White;
                    }

                }

            }

            if (e.DataColumn.FieldName == "MFINSUB2030")
            {
                if (grid.GetRowValues(Convert.ToInt32(e.VisibleIndex), "TSFIN2030") != DBNull.Value)
                {
                    int valor = Convert.ToInt32(grid.GetRowValues(Convert.ToInt32(e.VisibleIndex), "TSFIN2030").ToString());
                    if (valor == 1)
                    {
                        e.Cell.BackColor = System.Drawing.Color.Green;
                        e.Cell.ForeColor = System.Drawing.Color.White;
                    }

                }

            }


        }

        protected void gvsubProductos_CellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e)
        {
            Session["carga"] = 1;
            ASPxGridView grid = sender as ASPxGridView;
            if (e.Column.FieldName == "MFSUB2020")
            {
                if (grid.GetRowValues(Convert.ToInt32(e.VisibleIndex), "TSF2020") != DBNull.Value)
                {
                    int valor = Convert.ToInt32(grid.GetRowValues(Convert.ToInt32(e.VisibleIndex), "TSF2020").ToString());
                    if (valor == 1)
                    {
                        e.Editor.ClientEnabled = false;
                        e.Editor.BackColor = System.Drawing.Color.Green;
                        e.Editor.ForeColor = System.Drawing.Color.White;
                        e.Editor.Font.Bold = true;
                    }

                }
                else if (Convert.ToInt32(Session["periodo"]) == 20 || Convert.ToInt32(Session["periodo"]) == 21 || Convert.ToInt32(Session["periodo"]) == 22 || Convert.ToInt32(Session["periodo"]) == 23 || Convert.ToInt32(Session["periodo"]) == 24 || Convert.ToInt32(Session["periodo"]) == 25)
                {
                    e.Editor.ClientEnabled = false;
                }
            }

            if (e.Column.FieldName == "MFINSUB2020")
            {
                if (grid.GetRowValues(Convert.ToInt32(e.VisibleIndex), "TSFIN2020") != DBNull.Value)
                {
                    int valor = Convert.ToInt32(grid.GetRowValues(Convert.ToInt32(e.VisibleIndex), "TSFIN2020").ToString());
                    if (valor == 1)
                    {
                        e.Editor.ClientEnabled = false;
                        e.Editor.BackColor = System.Drawing.Color.Green;
                        e.Editor.ForeColor = System.Drawing.Color.White;
                        e.Editor.Font.Bold = true;
                    }

                }

                else if (Convert.ToInt32(Session["periodo"]) != 0)
                {
                    e.Editor.ClientEnabled = false;
                }
            }


            if (e.Column.FieldName == "MFSUB2021")
            {
                if (grid.GetRowValues(Convert.ToInt32(e.VisibleIndex), "TSF2021") != DBNull.Value)
                {
                    int valor = Convert.ToInt32(grid.GetRowValues(Convert.ToInt32(e.VisibleIndex), "TSF2021").ToString());
                    if (valor == 1)
                    {
                        e.Editor.ClientEnabled = false;
                        e.Editor.BackColor = System.Drawing.Color.Green;
                        e.Editor.ForeColor = System.Drawing.Color.White;
                        e.Editor.Font.Bold = true;
                    }

                }

                else if (Convert.ToInt32(Session["periodo"]) == 21 || Convert.ToInt32(Session["periodo"]) == 22 || Convert.ToInt32(Session["periodo"]) == 23 || Convert.ToInt32(Session["periodo"]) == 24 || Convert.ToInt32(Session["periodo"]) == 25)
                {
                    e.Editor.ClientEnabled = false;
                }
            }

            if (e.Column.FieldName == "MFINSUB2021")
            {
                if (grid.GetRowValues(Convert.ToInt32(e.VisibleIndex), "TSFIN2021") != DBNull.Value)
                {
                    int valor = Convert.ToInt32(grid.GetRowValues(Convert.ToInt32(e.VisibleIndex), "TSFIN2021").ToString());
                    if (valor == 1)
                    {
                        e.Editor.ClientEnabled = false;
                        e.Editor.BackColor = System.Drawing.Color.Green;
                        e.Editor.ForeColor = System.Drawing.Color.White;
                        e.Editor.Font.Bold = true;
                    }

                }

                else if (Convert.ToInt32(Session["periodo"]) == 21 || Convert.ToInt32(Session["periodo"]) == 23 || Convert.ToInt32(Session["periodo"]) == 24 || Convert.ToInt32(Session["periodo"]) == 25)
                {
                    e.Editor.ClientEnabled = false;
                }
            }



            if (e.Column.FieldName == "MFSUB2022")
            {
                if (grid.GetRowValues(Convert.ToInt32(e.VisibleIndex), "TSF2022") != DBNull.Value)
                {
                    int valor = Convert.ToInt32(grid.GetRowValues(Convert.ToInt32(e.VisibleIndex), "TSF2022").ToString());
                    if (valor == 1)
                    {
                        e.Editor.ClientEnabled = false;
                        e.Editor.BackColor = System.Drawing.Color.Green;
                        e.Editor.ForeColor = System.Drawing.Color.White;
                        e.Editor.Font.Bold = true;
                    }

                }

                else if (Convert.ToInt32(Session["periodo"]) == 22 || Convert.ToInt32(Session["periodo"]) == 23 || Convert.ToInt32(Session["periodo"]) == 24 || Convert.ToInt32(Session["periodo"]) == 25)
                {
                    e.Editor.ClientEnabled = false;
                }
            }

            if (e.Column.FieldName == "MFINSUB2022")
            {
                if (grid.GetRowValues(Convert.ToInt32(e.VisibleIndex), "TSFIN2022") != DBNull.Value)
                {
                    int valor = Convert.ToInt32(grid.GetRowValues(Convert.ToInt32(e.VisibleIndex), "TSFIN2022").ToString());
                    if (valor == 1)
                    {
                        e.Editor.ClientEnabled = false;
                        e.Editor.BackColor = System.Drawing.Color.Green;
                        e.Editor.ForeColor = System.Drawing.Color.White;
                        e.Editor.Font.Bold = true;
                    }

                }

                else if (Convert.ToInt32(Session["periodo"]) == 22 || Convert.ToInt32(Session["periodo"]) == 23 || Convert.ToInt32(Session["periodo"]) == 24 || Convert.ToInt32(Session["periodo"]) == 25)
                {
                    e.Editor.ClientEnabled = false;
                }
            }


            if (e.Column.FieldName == "MFSUB2023")
            {
                if (grid.GetRowValues(Convert.ToInt32(e.VisibleIndex), "TSF2023") != DBNull.Value)
                {
                    int valor = Convert.ToInt32(grid.GetRowValues(Convert.ToInt32(e.VisibleIndex), "TSF2023").ToString());
                    if (valor == 1)
                    {
                        e.Editor.ClientEnabled = false;
                        e.Editor.BackColor = System.Drawing.Color.Green;
                        e.Editor.ForeColor = System.Drawing.Color.White;
                        e.Editor.Font.Bold = true;
                    }

                    else if (Convert.ToInt32(Session["periodo"]) == 23 || Convert.ToInt32(Session["periodo"]) == 24 || Convert.ToInt32(Session["periodo"]) == 25)
                    {
                        e.Editor.ClientEnabled = false;
                    }

                }
            }

            if (e.Column.FieldName == "MFINSUB2023")
            {
                if (grid.GetRowValues(Convert.ToInt32(e.VisibleIndex), "TSFIN2023") != DBNull.Value)
                {
                    int valor = Convert.ToInt32(grid.GetRowValues(Convert.ToInt32(e.VisibleIndex), "TSFIN2023").ToString());
                    if (valor == 1)
                    {
                        e.Editor.ClientEnabled = false;
                        e.Editor.BackColor = System.Drawing.Color.Green;
                        e.Editor.ForeColor = System.Drawing.Color.White;
                        e.Editor.Font.Bold = true;
                    }

                    else if (Convert.ToInt32(Session["periodo"]) == 23 || Convert.ToInt32(Session["periodo"]) == 24 || Convert.ToInt32(Session["periodo"]) == 25)
                    {
                        e.Editor.ClientEnabled = false;
                    }

                }
            }


            if (e.Column.FieldName == "MFSUB2024")
            {
                if (grid.GetRowValues(Convert.ToInt32(e.VisibleIndex), "TSF2024") != DBNull.Value)
                {
                    int valor = Convert.ToInt32(grid.GetRowValues(Convert.ToInt32(e.VisibleIndex), "TSF2024").ToString());
                    if (valor == 1)
                    {
                        e.Editor.ClientEnabled = false;
                        e.Editor.BackColor = System.Drawing.Color.Green;
                        e.Editor.ForeColor = System.Drawing.Color.White;
                        e.Editor.Font.Bold = true;
                    }

                }
            }

            if (e.Column.FieldName == "MFINSUB2024")
            {
                if (grid.GetRowValues(Convert.ToInt32(e.VisibleIndex), "TSFIN2024") != DBNull.Value)
                {
                    int valor = Convert.ToInt32(grid.GetRowValues(Convert.ToInt32(e.VisibleIndex), "TSFIN2024").ToString());
                    if (valor == 1)
                    {
                        e.Editor.ClientEnabled = false;
                        e.Editor.BackColor = System.Drawing.Color.Green;
                        e.Editor.ForeColor = System.Drawing.Color.White;
                        e.Editor.Font.Bold = true;
                    }

                }
            }


            if (e.Column.FieldName == "MFSUB2025")
            {
                int periodo = Convert.ToInt32(Session["periodo"]);
                if (grid.GetRowValues(Convert.ToInt32(e.VisibleIndex), "TSF2025") != DBNull.Value)
                {
                    int valor = Convert.ToInt32(grid.GetRowValues(Convert.ToInt32(e.VisibleIndex), "TSF2025").ToString());
                    if (valor == 1)
                    {
                        e.Editor.ClientEnabled = false;
                        e.Editor.BackColor = System.Drawing.Color.Green;
                        e.Editor.ForeColor = System.Drawing.Color.White;
                        e.Editor.Font.Bold = true;
                    }

                }

                else if (Convert.ToInt32(Session["periodo"]) == 0 || Convert.ToInt32(Session["periodo"]) == 25)
                {
                    e.Editor.ClientEnabled = false;
                }
            }

            if (e.Column.FieldName == "MFINSUB2025")
            {
                if (grid.GetRowValues(Convert.ToInt32(e.VisibleIndex), "TSFIN2025") != DBNull.Value)
                {
                    int valor = Convert.ToInt32(grid.GetRowValues(Convert.ToInt32(e.VisibleIndex), "TSFIN2025").ToString());
                    if (valor == 1)
                    {
                        e.Editor.ClientEnabled = false;
                        e.Editor.BackColor = System.Drawing.Color.Green;
                        e.Editor.ForeColor = System.Drawing.Color.White;
                        e.Editor.Font.Bold = true;
                    }

                }

                else if (Convert.ToInt32(Session["periodo"]) == 0 || Convert.ToInt32(Session["periodo"]) == 25)
                {
                    e.Editor.ClientEnabled = false;
                }
            }



            if (e.Column.FieldName == "MFSUB2026")
            {
                int periodo = Convert.ToInt32(Session["periodo"]);

                if (grid.GetRowValues(Convert.ToInt32(e.VisibleIndex), "TSF2026") != DBNull.Value)
                {
                    int valor = Convert.ToInt32(grid.GetRowValues(Convert.ToInt32(e.VisibleIndex), "TSF2026").ToString());
                    if (valor == 1)
                    {
                        e.Editor.ClientEnabled = false;
                        e.Editor.BackColor = System.Drawing.Color.Green;
                        e.Editor.ForeColor = System.Drawing.Color.White;
                        e.Editor.Font.Bold = true;
                    }

                }

                else if (Convert.ToInt32(Session["periodo"]) == 20 || Convert.ToInt32(Session["periodo"]) == 0)

                {
                    e.Editor.ClientEnabled = false;
                }
            }

            if (e.Column.FieldName == "MFINSUB2026")
            {
                if (grid.GetRowValues(Convert.ToInt32(e.VisibleIndex), "TSFIN2026") != DBNull.Value)
                {
                    int valor = Convert.ToInt32(grid.GetRowValues(Convert.ToInt32(e.VisibleIndex), "TSFIN2026").ToString());
                    if (valor == 1)
                    {
                        e.Editor.ClientEnabled = false;
                        e.Editor.BackColor = System.Drawing.Color.Green;
                        e.Editor.ForeColor = System.Drawing.Color.White;
                        e.Editor.Font.Bold = true;
                    }

                }

                else if (Convert.ToInt32(Session["periodo"]) == 20 || Convert.ToInt32(Session["periodo"]) == 0)
                {
                    e.Editor.ClientEnabled = false;
                }
            }



            if (e.Column.FieldName == "MFSUB2027")
            {
                if (grid.GetRowValues(Convert.ToInt32(e.VisibleIndex), "TSF2027") != DBNull.Value)
                {
                    int valor = Convert.ToInt32(grid.GetRowValues(Convert.ToInt32(e.VisibleIndex), "TSF2027").ToString());
                    if (valor == 1)
                    {
                        e.Editor.ClientEnabled = false;
                        e.Editor.BackColor = System.Drawing.Color.Green;
                        e.Editor.ForeColor = System.Drawing.Color.White;
                        e.Editor.Font.Bold = true;
                    }

                }

                else if (Convert.ToInt32(Session["periodo"]) != 22 && Convert.ToInt32(Session["periodo"]) != 23 && Convert.ToInt32(Session["periodo"]) != 24 && Convert.ToInt32(Session["periodo"]) != 25)
                {
                    e.Editor.ClientEnabled = false;
                }
            }

            if (e.Column.FieldName == "MFINSUB2027")
            {
                if (grid.GetRowValues(Convert.ToInt32(e.VisibleIndex), "TSFIN2027") != DBNull.Value)
                {
                    int valor = Convert.ToInt32(grid.GetRowValues(Convert.ToInt32(e.VisibleIndex), "TSFIN2027").ToString());
                    if (valor == 1)
                    {
                        e.Editor.ClientEnabled = false;
                        e.Editor.BackColor = System.Drawing.Color.Green;
                        e.Editor.ForeColor = System.Drawing.Color.White;
                        e.Editor.Font.Bold = true;
                    }

                }

                else if (Convert.ToInt32(Session["periodo"]) != 22 && Convert.ToInt32(Session["periodo"]) != 23 && Convert.ToInt32(Session["periodo"]) != 24 && Convert.ToInt32(Session["periodo"]) != 25)
                {
                    e.Editor.ClientEnabled = false;
                }
            }




            if (e.Column.FieldName == "MFSUB2028")
            {
                if (grid.GetRowValues(Convert.ToInt32(e.VisibleIndex), "TSF2028") != DBNull.Value)
                {
                    int valor = Convert.ToInt32(grid.GetRowValues(Convert.ToInt32(e.VisibleIndex), "TSF2028").ToString());
                    if (valor == 1)
                    {
                        e.Editor.ClientEnabled = false;
                        e.Editor.BackColor = System.Drawing.Color.Green;
                        e.Editor.ForeColor = System.Drawing.Color.White;
                        e.Editor.Font.Bold = true;
                    }

                }

                else if (Convert.ToInt32(Session["periodo"]) != 23 && Convert.ToInt32(Session["periodo"]) != 24 && Convert.ToInt32(Session["periodo"]) != 25)
                {
                    e.Editor.ClientEnabled = false;
                }
            }

            if (e.Column.FieldName == "MFINSUB2028")
            {
                if (grid.GetRowValues(Convert.ToInt32(e.VisibleIndex), "TSFIN2028") != DBNull.Value)
                {
                    int valor = Convert.ToInt32(grid.GetRowValues(Convert.ToInt32(e.VisibleIndex), "TSFIN2028").ToString());
                    if (valor == 1)
                    {
                        e.Editor.ClientEnabled = false;
                        e.Editor.BackColor = System.Drawing.Color.Green;
                        e.Editor.ForeColor = System.Drawing.Color.White;
                        e.Editor.Font.Bold = true;
                    }

                }

                else if (Convert.ToInt32(Session["periodo"]) != 23 && Convert.ToInt32(Session["periodo"]) != 24 && Convert.ToInt32(Session["periodo"]) != 25)
                {
                    e.Editor.ClientEnabled = false;
                }
            }

            //NUEVA PGG
            if (e.Column.FieldName == "MFSUB2029")
            {
                if (grid.GetRowValues(Convert.ToInt32(e.VisibleIndex), "TSF2029") != DBNull.Value)
                {
                    int valor = Convert.ToInt32(grid.GetRowValues(Convert.ToInt32(e.VisibleIndex), "TSF2029").ToString());
                    if (valor == 1)
                    {
                        e.Editor.ClientEnabled = false;
                        e.Editor.BackColor = System.Drawing.Color.Green;
                        e.Editor.ForeColor = System.Drawing.Color.White;
                        e.Editor.Font.Bold = true;
                    }

                }

                else if (Convert.ToInt32(Session["periodo"]) != 24 && Convert.ToInt32(Session["periodo"]) != 25)
                {
                    e.Editor.ClientEnabled = false;
                }
            }

            if (e.Column.FieldName == "MFINSUB2029")
            {
                if (grid.GetRowValues(Convert.ToInt32(e.VisibleIndex), "TSFIN2029") != DBNull.Value)
                {
                    int valor = Convert.ToInt32(grid.GetRowValues(Convert.ToInt32(e.VisibleIndex), "TSFIN2029").ToString());
                    if (valor == 1)
                    {
                        e.Editor.ClientEnabled = false;
                        e.Editor.BackColor = System.Drawing.Color.Green;
                        e.Editor.ForeColor = System.Drawing.Color.White;
                        e.Editor.Font.Bold = true;
                    }

                }

                else if (Convert.ToInt32(Session["periodo"]) != 24 && Convert.ToInt32(Session["periodo"]) != 25)
                {
                    e.Editor.ClientEnabled = false;
                }
            }


            if (e.Column.FieldName == "MFINSUB2030")
            {
                if (grid.GetRowValues(Convert.ToInt32(e.VisibleIndex), "TSFIN2030") != DBNull.Value)
                {
                    int valor = Convert.ToInt32(grid.GetRowValues(Convert.ToInt32(e.VisibleIndex), "TSFIN2030").ToString());
                    if (valor == 1)
                    {
                        e.Editor.ClientEnabled = false;
                        e.Editor.BackColor = System.Drawing.Color.Green;
                        e.Editor.ForeColor = System.Drawing.Color.White;
                        e.Editor.Font.Bold = true;
                    }

                }

                else if (Convert.ToInt32(Session["periodo"]) != 25)
                {
                    e.Editor.ClientEnabled = false;
                }
            }
            //NUEVA PGG





        }

        protected void btnRegreso_Click(object sender, EventArgs e)
        {
            Response.Redirect("../modulos/frmModulos.aspx");
        }

        protected void btnInsumos_Click(object sender, EventArgs e)
        {
            string programas = "";


            lblprograma.Text = "";
            lblPrograma2.Text = "";
            lblsubprograma.Text = "";
            lblSubprograma2.Text = "";
            lblproducto.Text = "";
            lblProducto2.Text = "";
            lblsubproducto.Text = "";
            lblSubproducto2.Text = "";
            cbFuente.SelectedIndex = -1;
            cbRenglon.SelectedIndex = -1;
            txtUnidadEjecutora.Text = "";
            cbInsumo.SelectedIndex = -1;
            cbInsumo.Items.Clear();
            txtDescripcionInsumo.Text = "";
            cbUnidadMedida.SelectedIndex = -1;
            txtCantidad.Text = "";
            txtPrecio.Text = "";

            descripcionyMedida.Visible = true;


            cargaFuentesyRenglones();

            ASPxGridView gvDetailResult = gvPOA.FindDetailRowTemplateControl(Convert.ToInt32(gvPOA.FocusedRowIndex), "gvResultadosPOA") as ASPxGridView;

            ASPxGridView gvDetailProducto = gvDetailResult.FindDetailRowTemplateControl(Convert.ToInt32(gvDetailResult.FocusedRowIndex.ToString()), "gvPOAproductos") as ASPxGridView;

            ASPxGridView gvDetailSubproducto = gvDetailProducto.FindDetailRowTemplateControl(Convert.ToInt32(gvDetailProducto.FocusedRowIndex.ToString()), "gvPOASubproductos") as ASPxGridView;

            int idproducto = Convert.ToInt32(gvDetailSubproducto.GetRowValues(gvDetailSubproducto.FocusedRowIndex, "SPPRO$ID_PRODUCTO").ToString());
            int idsubproducto = Convert.ToInt32(gvDetailSubproducto.GetRowValues(gvDetailSubproducto.FocusedRowIndex, "SPPSUB$ID_SUBPRODUCTO").ToString());
            string subproductos = gvDetailSubproducto.GetRowValues(gvDetailSubproducto.FocusedRowIndex, "SUBPRODUCTO").ToString();

            Session["ID_PRODUCTO"] = idproducto;
            Session["ID_SUBPRODUCTO"] = idsubproducto;
            Session["SUBPRODUCTOS"] = subproductos;

            sql = "SELECT * FROM SCHE$SIPLAN20.SP20$PRODUCTO WHERE SPPRO$ID_PRODUCTO = " + idproducto + " AND SPPRO$RESTRICTIVA = 'N'";
            estado = dao.consulta(sql);





            if (estado == 1)
                tabla = dao.tabla;

            if (tabla.Rows.Count > 0)
            {
                lblproducto.Text = tabla.Rows[0]["SPPRO$DESCRIPCION"].ToString();
                lblProducto2.Text = tabla.Rows[0]["SPPRO$DESCRIPCION"].ToString();
                lblsubproducto.Text = subproductos;
                lblSubproducto2.Text = subproductos;
                sql = "SELECT SPPRO$ID_PROGRAMA_PRESUPUESTO||' '||SPPRO$DESCRIPCION PROGRAM, SPPRO$ID_PROGRAMA_DEPENDE, SPPRO$ID_POM, SPPRO$ID_INSTITUCION FROM SCHE$SIPLAN20.SP20$PROGRAMA_PRESUPUESTARIO WHERE SPPRO$ID_PROGRAMA_PRESUPUESTO   = " + tabla.Rows[0]["SPPRO$PRESUPUESTO"] + " AND SPPRO$ID_POM = " + tabla.Rows[0]["SPPRO$POM"] + " AND  SPPRO$ID_INSTITUCION = " + tabla.Rows[0]["SPPRO$INSTO"] + " AND SPPRO$RESTRICTIVA = 'N'";
                estado = dao.consulta(sql);
                if (estado == 1)
                {
                    tabla = dao.tabla;
                    if (tabla.Rows.Count > 0)
                    {
                        programas = tabla.Rows[0]["PROGRAM"].ToString();

                        if (tabla.Rows[0]["SPPRO$ID_PROGRAMA_DEPENDE"] == DBNull.Value)
                        {
                            lblprograma.Text = tabla.Rows[0]["PROGRAM"].ToString();
                            lblsubprograma.Text = "SIN SUBPROGRAMA";
                        }
                        else
                        {
                            sql = "SELECT SPPRO$ID_PROGRAMA_PRESUPUESTO||' '||SPPRO$DESCRIPCION PROGRAM FROM SCHE$SIPLAN20.SP20$PROGRAMA_PRESUPUESTARIO WHERE SPPRO$ID_PROGRAMA_PRESUPUESTO = " + tabla.Rows[0]["SPPRO$ID_PROGRAMA_PRESUPUESTO"] + " AND SPPRO$ID_POM = " + tabla.Rows[0]["SPPRO$ID_POM"] + " AND SPPRO$ID_INSTITUCION = " + tabla.Rows[0]["SPPRO$ID_INSTITUCION"] + " AND SPPRO$RESTRICTIVA = 'N'";
                            estado = dao.consulta(sql);
                            if (estado == 1)
                            {
                                tabla = dao.tabla;
                                if (tabla.Rows.Count > 0)
                                {
                                    lblprograma.Text = tabla.Rows[0]["PROGRAM"].ToString();
                                    lblPrograma2.Text = tabla.Rows[0]["PROGRAM"].ToString();
                                    lblsubprograma.Text = programas;
                                    lblSubprograma2.Text = programas;
                                }
                            }
                        }
                    }
                }

            }

            cargaInsumos(Convert.ToInt32(cbAniPOA.Value.ToString()), Convert.ToInt32(cbAniPOA.Text), idsubproducto);
            if (Convert.ToInt32(Session["abierto"]) == 1)
                btnGrabaIns.Enabled = false;
            else
                btnGrabaIns.Enabled = true;

            popInsumo.ShowOnPageLoad = true;


        }

        protected void cargaFuentesyRenglones()
        {
            string sql;
            sql = "SELECT ADSCGFF$ID, ADSCGFF$ID||' '||ADSCGFF$DESCRIPCION FUENTE FROM SCHE$ADSIS.ADSTBCG$FUENTES_FINANCIAMIENTO ORDER BY ADSCGFF$ID ASC";
            estado = dao.consulta(sql);
            if (estado == 1)
                tabla = dao.tabla;

            if (tabla.Rows.Count > 0)
            {
                cbFuente.DataSource = tabla;
                cbFuente.ValueField = "ADSCGFF$ID";
                cbFuente.TextField = "FUENTE";
                cbFuente.DataBind();
            }

            sql = "SELECT ADSRNP$ID, ADSRNP$DESCRIPCION RENGLON FROM SCHE$ADSIS.ADSTBCG$RENGLONES_PRESUP ORDER BY ADSRNP$ID ASC";
            estado = dao.consulta(sql);
            if (estado == 1)
                tabla = dao.tabla;


            if (tabla.Rows.Count > 0)
            {
                cbRenglon.DataSource = tabla;
                cbRenglon.ValueField = "ADSRNP$ID";
                cbRenglon.TextField = "RENGLON";
                cbRenglon.DataBind();
            }

            sql = "SELECT UNIDAD_MEDIDA, NOMBRE FROM SINIP.CP_UNIDADES_MEDIDA  ORDER BY UNIDAD_MEDIDA ASC";

            /*sql = @"SELECT UNIDAD_MEDIDA
                    ,NOMBRE 
                    FROM 
                    (SELECT 0 UNIDAD_MEDIDA, 'SIN INFORMACIÓN DISPONIBLE' NOMBRE FROM SINIP.CP_UNIDADES_MEDIDA 
                    UNION
                    SELECT UNIDAD_MEDIDA, NOMBRE FROM SINIP.CP_UNIDADES_MEDIDA) 
                    ORDER BY UNIDAD_MEDIDA ASC";*/
            estado = dao.consulta(sql);
            if (estado == 1)
                tabla = dao.tabla;
            if (tabla.Rows.Count > 0)
            {
                cbUnidadMedida.DataSource = tabla;
                cbUnidadMedida.ValueField = "UNIDAD_MEDIDA";
                cbUnidadMedida.TextField = "NOMBRE";
                cbUnidadMedida.DataBind();

            }

        }



        protected void cargaInsumos(int poa, int anio, int sub)
        {
            DataTable insumos = new DataTable();

            sql = @"SELECT
SPINS$ID
,SPINS$UNIDAD_EJECUTORA
,SPINS$FUENTE
,SPINS$RENGLON
,SPINS$CODIGO
,CASE WHEN SPINS$CODIGO = -1 THEN SPINS$INSUMO ELSE INSUMO END AS INSUMO_DESC
, UNIDAD_MEDIDA
, SPINS$PRESENTACION
,SPINS$PRECIO_UNITARIO
,SPINS$CANTIDAD
,TOTAL_PROGRAMADO
,SPINS$CUATRIMESTRE1
,SPINS$CUATRIMESTRE2
,SPINS$CUATRIMESTRE3
,TOTAL_CUATRIMESTRE
FROM
(SELECT SPINS$ID
, SPINS$UNIDAD_EJECUTORA
, SPINS$FUENTE
, SPINS$RENGLON
, SPINS$CODIGO
, SPINS$INSUMO
, (SELECT MCLISS$CODIGO_INSUMO || '-' || MCLISS$NOMBRE FROM SCHE$MCLAS.MCLTBL$INSUMO_SIGES WHERE MCLISS$RENGLON = SPINS$RENGLON AND MCLISS$CODIGO_INSUMO = SPINS$CODIGO AND MCLISS$CODIGO_PRESENTACION = SPINS$PRESENTACION) INSUMO
,UNIDAD_MEDIDA
,SPINS$PRESENTACION
,SPINS$PRECIO_UNITARIO
,SPINS$CANTIDAD
,ROUND((SPINS$PRECIO_UNITARIO * SPINS$CANTIDAD),2) TOTAL_PROGRAMADO
,SPINS$CUATRIMESTRE1
,SPINS$CUATRIMESTRE2
,SPINS$CUATRIMESTRE3
,ROUND(NVL(SPINS$CUATRIMESTRE1, 0) + NVL(SPINS$CUATRIMESTRE2, 0) + NVL(SPINS$CUATRIMESTRE3, 0), 2) TOTAL_CUATRIMESTRE
        FROM SCHE$SIPLAN20.SP20$INSUMOS WHERE SPINS$POA = " + poa + " AND SPINS$ANIO = " + anio + " AND SPINS$SUBPRODUCTO = " + sub + " AND SPINS$RESTRICTIVA = 'N') ORDER BY SPINS$ID ASC";

            estado = dao.consulta(sql);
            if (estado == 1)
            {
                insumos = dao.tabla;
                if (insumos.Rows.Count > 0)
                {
                    Session["insumos"] = insumos;
                    gvInsumos.DataSource = insumos;
                    gvInsumos.DataBind();


                }
            }


        }

        protected void btnGrabaIns_Click(object sender, EventArgs e)
        {
            string medida = "";
            int presentacion = -1;
            int insumo = -1;
            DataTable medidas = new DataTable();


            if (cbFuente.SelectedIndex == -1)
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel3, GetType(), "script", "Alerta('Seleccione la fuente de financiamiento <br/>',2);", true);
                cbFuente.Focus();
            }

            else if (cbRenglon.SelectedIndex == -1)
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel3, GetType(), "script", "Alerta('Seleccione el renglon presupuestario <br/>',2);", true);
                cbRenglon.Focus();
            }

            else if (txtUnidadEjecutora.Text == "")
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel3, GetType(), "script", "Alerta('Ingrese la unidad ejecutora <br/>',2);", true);
                txtUnidadEjecutora.Focus();
            }




            else if (!cbInsumo.Text.Equals("NO APLICA") && cbInsumo.SelectedIndex == -1)
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel3, GetType(), "script", "Alerta('El codigo de insumo es necesario <br/>',2);", true);
                cbInsumo.Focus();
            }

            else if (txtDescripcionInsumo.Text == "" && cbInsumo.Text.Equals("NO APLICA"))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel3, GetType(), "script", "Alerta('La descripción del insumo es necesario <br/>',2);", true);
                txtDescripcionInsumo.Focus();
            }

            else if (cbInsumo.Text.Equals("NO APLICA") && cbUnidadMedida.SelectedIndex == -1)
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel3, GetType(), "script", "Alerta('Seleccione la unidad de medida del insumo <br/>',2);", true);
                cbUnidadMedida.Focus();
            }

            else if (txtCantidad.Text == "")
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel3, GetType(), "script", "Alerta('Ingrese la cantidad programada para el insumo <br/>',2);", true);
                txtCantidad.Focus();
            }

            else if (txtPrecio.Text == "")
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel3, GetType(), "script", "Alerta('Ingrese el precio unitario del insumo <br/>',2);", true);
                txtPrecio.Focus();
            }

            else if (txtCUA1.Text == "")
            {
                //ScriptManager.RegisterStartupScript(this.UpdatePanel3, GetType(), "script", "Alerta('Ingrese la programación para el cuatrimestre I <br/>',2);", true);
                txtCUA1.Text = "0";
            }

            else if (txtCUA2.Text == "")
            {
                //ScriptManager.RegisterStartupScript(this.UpdatePanel3, GetType(), "script", "Alerta('Ingrese la programación para el cuatrimestre II <br/>',2);", true);
                txtCUA2.Text = "0";
            }

            else if (txtCUA3.Text == "")
            {
                //ScriptManager.RegisterStartupScript(this.UpdatePanel3, GetType(), "script", "Alerta('Ingrese la programación para el cuatrimestre III <br/>',2);", true);
                txtCUA3.Text = "0";
            }
            else
            {
                if (!cbInsumo.Text.Equals("NO APLICA"))
                {
                    sql = "SELECT MCLISS$CODIGO_INSUMO, MCLISS$NOMBRE_PRESENTACION, MCLISS$CODIGO_PRESENTACION FROM SCHE$MCLAS.MCLTBL$INSUMO_SIGES WHERE MCLISS$RENGLON = " + cbRenglon.Value.ToString() + " AND MCLISS$CODIGO_INSUMO ||'-'|| MCLISS$CODIGO_PRESENTACION = '" + cbInsumo.Value.ToString() + "'";
                    estado = dao.consulta(sql);
                    if (estado == 1)
                    {
                        medidas = dao.tabla;
                        if (medidas.Rows.Count > 0)
                        {
                            medida = medidas.Rows[0]["MCLISS$NOMBRE_PRESENTACION"].ToString();
                            medida = medida.Replace(" ", "");
                            presentacion = Convert.ToInt32(medidas.Rows[0]["MCLISS$CODIGO_PRESENTACION"].ToString());
                            insumo = Convert.ToInt32(medidas.Rows[0]["MCLISS$CODIGO_INSUMO"].ToString());
                        }


                        else
                        {
                            medida = "NO APLICA";
                            presentacion = -1;
                        }


                    }
                }




                sql = "INSERT INTO SCHE$SIPLAN20.SP20$INSUMOS (SPINS$FUENTE, SPINS$UNIDAD_EJECUTORA,SPINS$CODIGO,SPINS$INSUMO,SPINS$CANTIDAD,SPINS$PRECIO_UNITARIO,SPINS$CUATRIMESTRE1,SPINS$CUATRIMESTRE2,SPINS$CUATRIMESTRE3,SPINS$INSERT,SPINS$SUBPRODUCTO,SPINS$POA,SPINS$ANIO,SPINS$RENGLON,SPINS$IDUNIDAD_MEDIDA,UNIDAD_MEDIDA, SPINS$PRESENTACION) VALUES(";

                sql = sql + cbFuente.Value.ToString() + ",'" + txtUnidadEjecutora.Text + "',";
                if (cbInsumo.Text.Equals("NO APLICA"))
                    sql = sql + -1 + "," + "'" + txtDescripcionInsumo.Text + "',";
                else
                    sql = sql + insumo + "," + "NULL,";

                sql = sql + txtCantidad.Text + "," + txtPrecio.Text + "," + txtCUA1.Text + "," + txtCUA2.Text + "," + txtCUA3.Text + ",'INSERTA = ' || TO_CHAR(SYSDATE, 'DD/MM/YYYY HH:MI') || ' ' || '" + Session["USUARIO"].ToString() + "'," + Session["ID_SUBPRODUCTO"] + "," + cbAniPOA.Value.ToString() + "," + cbAniPOA.Text + "," + cbRenglon.Value.ToString() + ",";
                if (cbInsumo.Text.Equals("NO APLICA"))
                    sql = sql + cbUnidadMedida.Value.ToString() + "," + "'" + cbUnidadMedida.Text + "'," + -1 + ")";
                else
                    sql = sql + -1 + "," + "'" + medida + "'," + presentacion + ")";


                estado = dao.comando(sql);

                if (estado == 1)
                {
                    // lblprograma.Text = "";
                    //lblsubprograma.Text = "";
                    //lblproducto.Text = "";
                    //lblsubproducto.Text = "";

                    cargaInsumos(Convert.ToInt32(cbAniPOA.Value.ToString()), Convert.ToInt32(cbAniPOA.Text), Convert.ToInt32(Session["ID_SUBPRODUCTO"].ToString()));
                    cbFuente.SelectedIndex = -1;
                    cbRenglon.SelectedIndex = -1;
                    txtUnidadEjecutora.Text = "";
                    cbInsumo.Text = "";
                    cbInsumo.SelectedIndex = -1;
                    txtDescripcionInsumo.Text = "";
                    cbUnidadMedida.SelectedIndex = -1;
                    txtCantidad.Text = "";
                    txtPrecio.Text = "";
                    txtCUA1.Text = "";
                    txtCUA2.Text = "";
                    txtCUA3.Text = "";
                    descripcionyMedida.Visible = true;
                    cbInsumo.Items.Clear();
                    ScriptManager.RegisterStartupScript(this.UpdatePanel3, GetType(), "script", "Alerta('Insumo registrado correctamente <br/>',1);", true);

                }

                else
                {
                    mensaje = dao.mensaje;
                    ScriptManager.RegisterStartupScript(this.UpdatePanel3, GetType(), "script", "Alerta('" + mensaje + " <br/>',2);", true);

                }



            }

        }

        protected void btnCancelaIns_Click(object sender, EventArgs e)
        {
            lblprograma.Text = "";
            lblPrograma2.Text = "";
            lblsubprograma.Text = "";
            lblSubprograma2.Text = "";
            lblproducto.Text = "";
            lblProducto2.Text = "";
            lblsubproducto.Text = "";
            lblSubproducto2.Text = "";
            cbFuente.SelectedIndex = -1;
            cbRenglon.SelectedIndex = -1;
            txtUnidadEjecutora.Text = "";
            cbInsumo.SelectedIndex = -1;
            cbInsumo.Text = "";
            txtDescripcionInsumo.Text = "";
            cbUnidadMedida.SelectedIndex = -1;
            txtCantidad.Text = "";
            txtPrecio.Text = "";
            txtCUA1.Text = "";
            txtCUA2.Text = "";
            txtCUA3.Text = "";
            cbInsumo.Items.Clear();
            descripcionyMedida.Visible = true;
            popInsumo.ShowOnPageLoad = false;
        }


        protected void cbRenglon_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable ins = new DataTable();
            cbInsumo.Items.Clear();

            string insumo = "SELECT MCLISS$CODIGO_INSUMO ||'-'|| MCLISS$CODIGO_PRESENTACION AS CODIGO, MCLISS$CODIGO_INSUMO||'-presentacion-'||MCLISS$CODIGO_PRESENTACION||' - '|| MCLISS$NOMBRE || ' - ' || MCLISS$NOMBRE_PRESENTACION AS INSUMO   FROM SCHE$MCLAS.MCLTBL$INSUMO_SIGES I WHERE I.MCLISS$RENGLON = " + cbRenglon.Value.ToString();
            estado = dao.consulta(insumo);

            if (estado == 1)
            {
                ins = dao.tabla;

                if (ins.Rows.Count > 0)
                {
                    cbInsumo.DataSource = ins;
                    Session["insumo"] = ins;
                    cbInsumo.TextField = "INSUMO";
                    cbInsumo.ValueField = "CODIGO";
                    cbInsumo.DataBind();
                    descripcionyMedida.Visible = false;
                }
                else
                {
                    cbInsumo.Text = "NO APLICA";
                    descripcionyMedida.Visible = true;
                }

            }


        }

        protected void gvInsumos_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            DataTable tempo = new DataTable();
            tempo = (DataTable)Session["insumos"];

            if (tempo.Rows.Count > 0)
            {

                for (int i = 0; i <= tempo.Rows.Count - 1; i++)
                {
                    if (tempo.Rows[i]["SPINS$ID"].ToString() == e.Keys["SPINS$ID"].ToString())
                    {
                        sql = "UPDATE SCHE$SIPLAN20.SP20$INSUMOS SET SPINS$RESTRICTIVA = 'S' WHERE SPINS$ID = " + tempo.Rows[i]["SPINS$ID"].ToString();
                        estado = dao.comando(sql);
                        if (estado == 1)
                        {
                            mensaje = dao.mensaje;
                            gvInsumos.JSProperties["cpError"] = "Operacion correcta: Registro borrado correctamente";
                            e.Cancel = true;
                            cargaInsumos(Convert.ToInt32(cbAniPOA.Value.ToString()), Convert.ToInt32(cbAniPOA.Text), Convert.ToInt32(Session["ID_SUBPRODUCTO"]));
                            break;
                        }
                        else
                        {
                            mensaje = dao.mensaje;
                            gvInsumos.JSProperties["cpError"] = "Ocurrio un error: " + mensaje;
                            e.Cancel = true;
                            break;
                        }
                    }

                }


            }


        }

        protected void gvInsumos_CommandButtonInitialize(object sender, ASPxGridViewCommandButtonEventArgs e)
        {

            ASPxGridView grid = sender as ASPxGridView;

            if (e.VisibleIndex != -1)
            {

                if (Session["ROL"].ToString() == "ENTIDAD" || Convert.ToInt32(Session["abierto"]) == 1)
                {
                    e.Visible = false;
                }





            }
        }

        protected void btnPOM_Click(object sender, EventArgs e)
        {
            sql = "SELECT SPPO$AUTORIDAD, SPPO$CARGO  FROM SCHE$SIPLAN20.SP20$POM WHERE SPPO$ID_POM = " + Convert.ToInt32(Session["pom"].ToString());
            estado = dao.consulta(sql);

            if (estado == 1)
            {
                tabla = dao.tabla;
                if (tabla.Rows.Count > 0)
                {
                    if (tabla.Rows[0]["SPPO$AUTORIDAD"] != DBNull.Value)
                        txtAutoridad1.Text = tabla.Rows[0]["SPPO$AUTORIDAD"].ToString();
                    else
                        txtAutoridad1.Text = "";

                    if (tabla.Rows[0]["SPPO$CARGO"] != DBNull.Value)
                        txtCargo1.Text = tabla.Rows[0]["SPPO$CARGO"].ToString();
                    else
                        txtCargo1.Text = "";

                }
            }
            else
            {
                mensaje = dao.mensaje;
                Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
            }

            popReportePOM.ShowOnPageLoad = true;

        }

        protected void btnGeneraPOM_Click(object sender, EventArgs e)
        {


            String link;




            sql = "UPDATE SCHE$SIPLAN20.SP20$POM SET ";
            if (txtAutoridad1.Text != "")
                sql = sql + "SPPO$AUTORIDAD = '" + txtAutoridad1.Text + "', ";
            else
                sql = sql + "SPPO$AUTORIDAD = NULL, ";

            if (txtCargo1.Text != "")
                sql = sql + "SPPO$CARGO = '" + txtCargo1.Text + "'";
            else
                sql = sql + "SPPO$CARGO = NULL";

            sql = sql + " WHERE SPPO$ID_POM = " + Convert.ToInt32(Session["pom"].ToString());

            estado = dao.comando(sql);
            if (estado == 0)
            {
                mensaje = dao.mensaje;
                ScriptManager.RegisterStartupScript(this.upEstrategicos, GetType(), "script", "Alerta('" + mensaje + " <br/>',2);", true);
            }


            /* if (Convert.ToInt32(Session["periodo"]) == 21)
             {
                 //link = "https://rpts.segeplan.gob.gt:8080/jasperserver/flow.html?_flowId=viewReportFlow&_flowId=viewReportFlow&pom=" + Convert.ToInt32(Session["pom"]) + "&insto=" + Convert.ToInt32(Session["insto"]) + "&j_username=reportes&j_password=reporte&ParentFolderUri=%2Freports%2FSIPLANDESA&reportUnit=%2Freports%2FSIPLANDESA%2FPOM2226_RESULTADOS&standAlone=true&output=pdf&userLocale=es_GT";
                 link = "https://rpts.segeplan.gob.gt:8080/jasperserver/flow.html?_flowId=viewReportFlow&_flowId=viewReportFlow&pom=" + Convert.ToInt32(Session["pom"]) + "&insto=" + Convert.ToInt32(Session["insto"]) + "&j_username=reportes&j_password=reporte&ParentFolderUri=%2Freports%2FSIPLAN&reportUnit=%2Freports%2FSIPLAN%2FPOM2226_RESULTADOS&standAlone=true&output=pdf&userLocale=es_GT";
                 ScriptManager.RegisterStartupScript(upEstrategicos, upEstrategicos.GetType(), "script", "window.open('" + link + "');", true);
             }

             else if (Convert.ToInt32(Session["periodo"]) == 22)
             {
                 //link = "https://rpts.segeplan.gob.gt:8080/jasperserver/flow.html?_flowId=viewReportFlow&_flowId=viewReportFlow&pom=" + Convert.ToInt32(Session["pom"]) + "&insto=" + Convert.ToInt32(Session["insto"]) + "&j_username=reportes&j_password=reporte&ParentFolderUri=%2Freports%2FSIPLANDESA&reportUnit=%2Freports%2FSIPLANDESA%2FPOMResultados20232027&standAlone=true&output=pdf&userLocale=es_GT";
                 link = "https://rpts.segeplan.gob.gt:8080/jasperserver/flow.html?_flowId=viewReportFlow&_flowId=viewReportFlow&pom=" + Convert.ToInt32(Session["pom"]) + "&insto=" + Convert.ToInt32(Session["insto"]) + "&j_username=reportes&j_password=reporte&ParentFolderUri=%2Freports%2FSIPLAN&reportUnit=%2Freports%2FSIPLAN%2FPOMResultados20232027&standAlone=true&output=pdf&userLocale=es_GT";
                 ScriptManager.RegisterStartupScript(upEstrategicos, upEstrategicos.GetType(), "script", "window.open('" + link + "');", true);
             }

             else if (Convert.ToInt32(Session["periodo"]) == 23)
             {
                 //link = "https://rpts.segeplan.gob.gt:8080/jasperserver/flow.html?_flowId=viewReportFlow&_flowId=viewReportFlow&pom=" + Convert.ToInt32(Session["pom"]) + "&insto=" + Convert.ToInt32(Session["insto"]) + "&j_username=reportes&j_password=reporte&ParentFolderUri=%2Freports%2FSIPLANDESA&reportUnit=%2Freports%2FSIPLANDESA%2FPOMResultados20242028&standAlone=true&output=pdf&userLocale=es_GT";
                 link = "https://rpts.segeplan.gob.gt:8080/jasperserver/flow.html?_flowId=viewReportFlow&_flowId=viewReportFlow&pom=" + Convert.ToInt32(Session["pom"]) + "&insto=" + Convert.ToInt32(Session["insto"]) + "&j_username=reportes&j_password=reporte&ParentFolderUri=%2Freports%2FSIPLAN&reportUnit=%2Freports%2FSIPLAN%2FPOMResultados20242028&standAlone=true&output=pdf&userLocale=es_GT";
                 ScriptManager.RegisterStartupScript(upEstrategicos, upEstrategicos.GetType(), "script", "window.open('" + link + "');", true);
             }

             else if (Convert.ToInt32(Session["periodo"]) == 24)
             {
                 //link = "https://rpts.segeplan.gob.gt:8080/jasperserver/flow.html?_flowId=viewReportFlow&_flowId=viewReportFlow&pom=" + Convert.ToInt32(Session["pom"]) + "&insto=" + Convert.ToInt32(Session["insto"]) + "&j_username=reportes&j_password=reporte&ParentFolderUri=%2Freports%2FSIPLANDESA&reportUnit=%2Freports%2FSIPLANDESA%2FPOMResultados20252029&standAlone=true&output=pdf&userLocale=es_GT";
                 link = "https://rpts.segeplan.gob.gt:8080/jasperserver/flow.html?_flowId=viewReportFlow&_flowId=viewReportFlow&pom=" + Convert.ToInt32(Session["pom"]) + "&insto=" + Convert.ToInt32(Session["insto"]) + "&j_username=reportes&j_password=reporte&ParentFolderUri=%2Freports%2FSIPLAN&reportUnit=%2Freports%2FSIPLAN%2FPOMResultados20252029&standAlone=true&output=pdf&userLocale=es_GT";
                 ScriptManager.RegisterStartupScript(upEstrategicos, upEstrategicos.GetType(), "script", "window.open('" + link + "');", true);
             }

             else if (Convert.ToInt32(Session["periodo"]) == 25)
             {
                 //link = "https://rpts.segeplan.gob.gt:8080/jasperserver/flow.html?_flowId=viewReportFlow&_flowId=viewReportFlow&pom=" + Convert.ToInt32(Session["pom"]) + "&insto=" + Convert.ToInt32(Session["insto"]) + "&j_username=reportes&j_password=reporte&ParentFolderUri=%2Freports%2FSIPLANDESA&reportUnit=%2Freports%2FSIPLANDESA%2FPOMResultados20262028&standAlone=true&output=pdf&userLocale=es_GT";
                 link = "https://rpts.segeplan.gob.gt:8080/jasperserver/flow.html?_flowId=viewReportFlow&_flowId=viewReportFlow&pom=" + Convert.ToInt32(Session["pom"]) + "&insto=" + Convert.ToInt32(Session["insto"]) + "&j_username=reportes&j_password=reporte&ParentFolderUri=%2Freports%2FSIPLAN&reportUnit=%2Freports%2FSIPLAN%2FPOMResultados20262028&standAlone=true&output=pdf&userLocale=es_GT";
                 ScriptManager.RegisterStartupScript(upEstrategicos, upEstrategicos.GetType(), "script", "window.open('" + link + "');", true);
             }*/

            sql = "SELECT SPPER$ENLACE1, SPPER$ENLACE2 FROM SCHE$SIPLAN20.SP20$ENLACES_REPORTES WHERE SPPER$TIPO = 0 AND SPPER$REPORTE_VIGENTE = 1 AND SPPER$PERIODO_POM = " + Convert.ToInt32(Session["periodo"]);

            estado = dao.consulta(sql);
            if (estado == 1)
            {
                tabla = dao.tabla;
                if (tabla.Rows.Count > 0)

                    link = tabla.Rows[0]["SPPER$ENLACE1"].ToString() + Convert.ToInt32(Session["pom"]) + "&insto=" + Convert.ToInt32(Session["insto"]) + tabla.Rows[0]["SPPER$ENLACE2"].ToString();


                else
                    link = "";
            }
            else
                link = "";

            if (link != "")
                ScriptManager.RegisterStartupScript(upEstrategicos, upEstrategicos.GetType(), "script", "window.open('" + link + "');", true);
            else
                ScriptManager.RegisterStartupScript(upEstrategicos, upEstrategicos.GetType(), "script", "Alerta('Reporte no disponible', 2);", true);
        }

        protected void btnCancelaPOM_Click(object sender, EventArgs e)
        {
            popReportePOM.ShowOnPageLoad = false;
        }

        protected void btnGeneraPOA_Click(object sender, EventArgs e)
        {



            sql = "UPDATE SCHE$SIPLAN20.SP20$POM SET ";
            if (txtAutoridad2.Text != "")
                sql = sql + "SPPO$AUTORIDAD = '" + txtAutoridad2.Text + "', ";
            else
                sql = sql + "SPPO$AUTORIDAD = NULL, ";

            if (txtCargo2.Text != "")
                sql = sql + "SPPO$CARGO = '" + txtCargo2.Text + "'";
            else
                sql = sql + "SPPO$CARGO = NULL";

            sql = sql + " WHERE SPPO$ID_POM = " + Convert.ToInt32(Session["pom"].ToString());

            estado = dao.comando(sql);
            if (estado == 0)
            {
                mensaje = dao.mensaje;
                ScriptManager.RegisterStartupScript(UpdatePanel5, GetType(), "script", "Alerta('" + mensaje + " <br/>',2);", true);
            }


            String link;
            //versiones de reportes de produccion


            link = "https://rpts.segeplan.gob.gt:8080/jasperserver/flow.html?_flowId=viewReportFlow&_flowId=viewReportFlow&pom=" + Convert.ToInt32(Session["pom"]) + "&insto=" + Convert.ToInt32(Session["insto"]) + "&anio=" + Convert.ToInt32(cbAniPOA.Text) + "&j_username=reportes&j_password=reporte&ParentFolderUri=%2Freports%2FSIPLANV2&reportUnit=%2Freports%2FSIPLANV2%2Fpoa_programacion&standAlone=true&output=pdf&userLocale=es_GT";

            //link = "https://rpts.segeplan.gob.gt:8080/jasperserver/flow.html?_flowId=viewReportFlow&_flowId=viewReportFlow&pom=" + Convert.ToInt32(Session["pom"]) + "&insto=" + Convert.ToInt32(Session["insto"]) + "&anio=" + Convert.ToInt32(cbAniPOA.Text) + "&j_username=reportes&j_password=reporte&ParentFolderUri=%2Freports%2Finteractive&reportUnit=%2Freports%2Finteractive%2Fpoa_programacion&standAlone=true&output=pdf&userLocale=es_GT";

            //versiones de reportes de desarrollo


            //link = "https://rpts.segeplan.gob.gt:8080/jasperserver/flow.html?_flowId=viewReportFlow&_flowId=viewReportFlow&pom=" + Convert.ToInt32(Session["pom"]) + "&insto=" + Convert.ToInt32(Session["insto"]) + "&anio=" + Convert.ToInt32(cbAniPOA.Text) + "&j_username=reportes&j_password=reporte&ParentFolderUri=%2Freports%2Fsye&reportUnit=%2Freports%2Fsye%2Fpoa_programacion&standAlone=true&output=pdf&userLocale=es_GT";
            //ScriptManager.RegisterStartupScript(UpdatePanel5, UpdatePanel5.GetType(), "script", "window.open('" + link + "');", true);

            sql = "SELECT SPPER$ENLACE1, SPPER$ENLACE2 FROM SCHE$SIPLAN20.SP20$ENLACES_REPORTES WHERE SPPER$TIPO = 1 AND SPPER$REPORTE_VIGENTE = 1 AND SPPER$RESTRICTIVA = 'N'";
            estado = dao.consulta(sql);
            if (estado == 1)
            {
                tabla = dao.tabla;
                if (tabla.Rows.Count > 0)
                    link = tabla.Rows[0]["SPPER$ENLACE1"].ToString() + Convert.ToInt32(Session["pom"]) + "&insto=" + Convert.ToInt32(Session["insto"]) + "&anio=" + Convert.ToInt32(cbAniPOA.Text) + tabla.Rows[0]["SPPER$ENLACE2"].ToString();
                else
                    link = "";
            }
            else
                link = "";

            if (link != "")
                ScriptManager.RegisterStartupScript(UpdatePanel5, UpdatePanel5.GetType(), "script", "window.open('" + link + "');", true);
            else
                ScriptManager.RegisterStartupScript(UpdatePanel5, UpdatePanel5.GetType(), "script", "Alerta('Reporte no disponible', 2);", true);



        }

        protected void btnCancelaPOA_Click(object sender, EventArgs e)
        {
            popReportePOA.ShowOnPageLoad = false;
        }

        //protected void gvResultados_DataBound(object sender, EventArgs e)
        //{
        //    ASPxGridView gvDetail = sender as ASPxGridView;
        //    gvDetail.Columns["RESULTADO"].Visible = false;
        //    gvDetail.Columns["EJE_ACCION"].Visible = false;
        //    gvDetail.Columns["RED"].Visible = false;

        //    foreach (DataRow row in ((DataTable)gvDetail.DataSource).Rows)
        //    {
        //        // Mostrar la columna 'AdditionalInfo' si algún valor de la columna 'Status' es "Show"
        //        if (Convert.ToInt32(row["TIPO"].ToString()) == 0 || Convert.ToInt32(row["TIPO"].ToString()) == 1)
        //        {
        //            gvDetail.Columns["RESULTADO"].Visible = true;
        //            gvDetail.Columns["EJE_ACCION"].Visible = true;
        //            gvDetail.Columns["RED"].Visible = false;
        //            break;
        //        }
        //        else if (Convert.ToInt32(row["TIPO"].ToString()) == 2)
        //        {
        //            gvDetail.Columns["RESULTADO"].Visible = false;
        //            gvDetail.Columns["EJE_ACCION"].Visible = false;
        //            gvDetail.Columns["RED"].Visible = true;
        //            break;
        //        }

        //    }
        //}

        //private void CollapseAllDetailRows()
        //{

        //    gvProduccion.CollapseAll();
        //}

    }
}