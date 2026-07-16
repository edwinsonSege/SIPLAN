using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SIPLAN2._0.DataAccess;
namespace SIPLAN2._0.procesos
{
    public partial class procesos : System.Web.UI.Page
    {
        string sql, mensaje;
        int estado; clsAccesoBBDD dao = new clsAccesoBBDD();
        DataTable tabla = new DataTable();
        DateTime date1 = new DateTime();
        int anio;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Usuario"] == null)
                Response.Redirect("../login/login.aspx");
            else
            {
                if (Session["ROL"].ToString() == "ADMIN")
                {
                    if (!IsPostBack)
                    {
                        cargaPeriodos();
                        anios();
                        cargaMetas(Convert.ToInt32(cbanio.Value));
                    }
                   else
                        cargaMetas(Convert.ToInt32(cbanio.Value));

                }
                else
                {
                    Response.Redirect("../login/login.aspx");
                }

                if (Session["carga"] != null)
                {
                    if (Convert.ToInt32(Session["carga"]) == 0)
                        cargaAvances(Convert.ToInt32(cbanio.Value), Convert.ToInt32(cbMes.Value));
                }
            }
           
        }

        protected void anios()
        {
            date1 = DateTime.Today;
            anio = date1.Year;
            for (int i = 2000; i <= 2050; i++)
            {

                cbanio.Items.Add(i.ToString());
            }

            cbanio.Value = anio;

        }

        protected void btnMetas_Click(object sender, EventArgs e)
        {
            MultiView1.ActiveViewIndex = 0;
            Session["carga"] = 1;
        }

        protected void btnVista2_Click(object sender, EventArgs e)
        {
            Session["carga"] = 0;
            cargaAvances(Convert.ToInt32(cbanio.Value),Convert.ToInt32(cbMes.Value));
            MultiView1.ActiveViewIndex = 1;
        }

        protected void cargaMetas(int anio)
        {
            sql = @"SELECT POA, SPRES$POM, SPRES$INSTITUCION, SPPSUB$ID_SUBPRODUCTO, SPPSUB$SNIP, NOMBRE_SNIP, (SELECT SPPMFS$ID_PROGRAMACION_FISFIN FROM SCHE$SIPLAN20.SP20$PROGRAMETA_FI_FIN_SUB SU WHERE SU.SPPMFS$ID_SUBPRODUCTO = SPPSUB$ID_SUBPRODUCTO AND SU.SPPMFS$ID_POA = POA AND SU.SPPMFS$VIGENTE = 0 AND SU.SPPMFS$RESTRICTIVA = 'N' AND SU.SPPMFS$TIPO_PROGRAMACION = 0) IDFISICO"
                  + ",(SELECT a.snsgma$cantidad FROM sntbsg$metas_anuales a WHERE a.snsgma$ejercicio = "+anio+" and a.snsgma$id_proyecto = SPPSUB$SNIP and a.snsgma$fase = 2) METAFISICA ,0 TIPO ,(SELECT SPPMFS$ID_PROGRAMACION_FISFIN FROM SCHE$SIPLAN20.SP20$PROGRAMETA_FI_FIN_SUB SU WHERE SU.SPPMFS$ID_SUBPRODUCTO = SPPSUB$ID_SUBPRODUCTO AND SU.SPPMFS$ID_POA = POA AND SU.SPPMFS$VIGENTE = 0 AND SU.SPPMFS$RESTRICTIVA = 'N' AND SU.SPPMFS$TIPO_PROGRAMACION = 1) IDFINANCIERO"
                 + ",(SELECT nvl(a.snsgrfm$gc_asignado_acum, 0) + nvl(a.snsgrfm$ex_asignado_acum, 0)  FROM SINIP.sntbsg$rfin_multia a WHERE a.snsgrfm$ejercicio = "+anio+"  and a.snsgrfm$proyecto = SPPSUB$SNIP and a.snsgrfm$fase = 2 and a.snsgrfm$restrictiva = 'N') METAFINANCIERA,1 TIPO FROM(SELECT vs.SPRES$POM, vs.SPRES$INSTITUCION, vs.SPPSUB$ID_SUBPRODUCTO, vs.SPPSUB$SNIP, vs.NOMBRE_SNIP, (SELECT SPOA$ID_POA FROM SCHE$SIPLAN20.SP20$POA WHERE SPOA$ID_POM = vs.SPRES$POM AND  SPOA$ANIO = "+anio+" AND SPOA$RESTRICTIVA = 'N')POA FROM SCHE$SIPLAN20.SPPVST$PRODUCCIONINSTO vs " +
                 "INNER JOIN SCHE$SIPLAN20.SP20$POM POM ON vs.SPRES$POM = POM.SPPO$ID_POM AND vs.SPRES$INSTITUCION = POM.SPPO$ID_INSTITUCION AND POM.SPPO$RESTRICTIVA = 'N' INNER JOIN SCHE$SIPLAN20.SP20$PERIODO P ON POM.SPPO$ID_PERIODO = P.SPP$ID_PERIODO AND POM.SPPO$RESTRICTIVA = 'N' AND P.SPP$RESTRICTIVA = 'N' AND P.SPP$ID_PERIODO = " +cbPeriodos.Value 
                + " WHERE SPPSUB$SNIP IS NOT NULL) WHERE SPRES$POM = 720 AND SPRES$INSTITUCION = 13000";

            estado = dao.consulta(sql);
            if (estado == 1)
            {
                tabla = dao.tabla;
                gvmetas.DataSource = tabla;
                gvmetas.DataBind();
            }
        }

        protected void cbMes_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session["carga"] = 0;
            cargaAvances(Convert.ToInt32(cbanio.Value), Convert.ToInt32(cbMes.Value));
        }

        protected void btnMetasfyf_Click(object sender, EventArgs e)
        {
            int anio = 0;
            anio = Convert.ToInt32(cbanio.Value);
            /*sql = @"SELECT POA, SPRES$POM, SPRES$INSTITUCION, SPPSUB$ID_SUBPRODUCTO, SPPSUB$SNIP, NOMBRE_SNIP, (SELECT SPPMFS$ID_PROGRAMACION_FISFIN FROM SCHE$SIPLAN20.SP20$PROGRAMETA_FI_FIN_SUB SU WHERE SU.SPPMFS$ID_SUBPRODUCTO = SPPSUB$ID_SUBPRODUCTO AND SU.SPPMFS$ID_POA = POA AND SU.SPPMFS$VIGENTE = 0 AND SU.SPPMFS$RESTRICTIVA = 'N' AND SU.SPPMFS$TIPO_PROGRAMACION = 0) IDFISICO"
                 + ",(SELECT a.snsgma$cantidad FROM SCHE$SINIP.sntbsg$metas_anuales a WHERE a.snsgma$ejercicio = " + anio + " and a.snsgma$id_proyecto = SPPSUB$SNIP and a.snsgma$fase = 2) METAFISICA ,0 TIPO ,(SELECT SPPMFS$ID_PROGRAMACION_FISFIN FROM SCHE$SIPLAN20.SP20$PROGRAMETA_FI_FIN_SUB SU WHERE SU.SPPMFS$ID_SUBPRODUCTO = SPPSUB$ID_SUBPRODUCTO AND SU.SPPMFS$ID_POA = POA AND SU.SPPMFS$VIGENTE = 0 AND SU.SPPMFS$RESTRICTIVA = 'N' AND SU.SPPMFS$TIPO_PROGRAMACION = 1) IDFINANCIERO"
                + ",(SELECT nvl(a.snsgrfm$gc_asignado_acum, 0) + nvl(a.snsgrfm$ex_asignado_acum, 0)  FROM SINIP.sntbsg$rfin_multia a WHERE a.snsgrfm$ejercicio = " + anio + "  and a.snsgrfm$proyecto = SPPSUB$SNIP and a.snsgrfm$fase = 2 and a.snsgrfm$restrictiva = 'N') METAFINANCIERA,1 TIPO FROM(SELECT vs.SPRES$POM, vs.SPRES$INSTITUCION, vs.SPPSUB$ID_SUBPRODUCTO, vs.SPPSUB$SNIP, vs.NOMBRE_SNIP, (SELECT SPOA$ID_POA FROM SCHE$SIPLAN20.SP20$POA WHERE SPOA$ID_POM = vs.SPRES$POM AND  SPOA$ANIO = " + anio + " AND SPOA$RESTRICTIVA = 'N'" +
               ")POA FROM SCHE$SIPLAN20.SPPVST$PRODUCCIONINSTO vs " +
                " INNER JOIN SCHE$SIPLAN20.SP20$POM POM ON vs.SPRES$POM = POM.SPPO$ID_POM AND vs.SPRES$INSTITUCION = POM.SPPO$ID_INSTITUCION AND POM.SPPO$RESTRICTIVA = 'N'" +
                " INNER JOIN SCHE$SIPLAN20.SP20$PERIODO P ON POM.SPPO$ID_PERIODO = P.SPP$ID_PERIODO AND POM.SPPO$RESTRICTIVA = 'N' AND P.SPP$RESTRICTIVA = 'N' AND P.SPP$ID_PERIODO = " + cbPeriodos.Value +
                "WHERE SPPSUB$SNIP IS NOT NULL AND SPRES$INSTITUCION = 75000)";*/

            sql = @"SELECT POA, SPRES$POM, SPRES$INSTITUCION, SPPSUB$ID_SUBPRODUCTO, SPPSUB$SNIP, NOMBRE_SNIP, (SELECT SPPMFS$ID_PROGRAMACION_FISFIN FROM SCHE$SIPLAN20.SP20$PROGRAMETA_FI_FIN_SUB SU WHERE SU.SPPMFS$ID_SUBPRODUCTO = SPPSUB$ID_SUBPRODUCTO AND SU.SPPMFS$ID_POA = POA AND SU.SPPMFS$VIGENTE = 0 AND SU.SPPMFS$RESTRICTIVA = 'N' AND SU.SPPMFS$TIPO_PROGRAMACION = 0) IDFISICO"
                 + ",(SELECT a.snsgma$cantidad FROM SCHE$SINIP.sntbsg$metas_anuales a WHERE a.snsgma$ejercicio = " + anio + " and a.snsgma$id_proyecto = SPPSUB$SNIP and a.snsgma$fase = 2) METAFISICA ,0 TIPO ,(SELECT SPPMFS$ID_PROGRAMACION_FISFIN FROM SCHE$SIPLAN20.SP20$PROGRAMETA_FI_FIN_SUB SU WHERE SU.SPPMFS$ID_SUBPRODUCTO = SPPSUB$ID_SUBPRODUCTO AND SU.SPPMFS$ID_POA = POA AND SU.SPPMFS$VIGENTE = 0 AND SU.SPPMFS$RESTRICTIVA = 'N' AND SU.SPPMFS$TIPO_PROGRAMACION = 1) IDFINANCIERO"
                + ",(SELECT nvl(a.snsgrfm$gc_asignado_acum, 0) + nvl(a.snsgrfm$ex_asignado_acum, 0)  FROM SINIP.sntbsg$rfin_multia a WHERE a.snsgrfm$ejercicio = " + anio + "  and a.snsgrfm$proyecto = SPPSUB$SNIP and a.snsgrfm$fase = 2 and a.snsgrfm$restrictiva = 'N') METAFINANCIERA,1 TIPOFINANCIERO FROM(SELECT vs.SPRES$POM, vs.SPRES$INSTITUCION, vs.SPPSUB$ID_SUBPRODUCTO, vs.SPPSUB$SNIP, vs.NOMBRE_SNIP, (SELECT SPOA$ID_POA FROM SCHE$SIPLAN20.SP20$POA WHERE SPOA$ID_POM = vs.SPRES$POM AND  SPOA$ANIO = " + anio + " AND SPOA$RESTRICTIVA = 'N'" +
               ")POA FROM SCHE$SIPLAN20.SPPVST$PRODUCCIONINSTO vs " +
                " INNER JOIN SCHE$SIPLAN20.SP20$POM POM ON vs.SPRES$POM = POM.SPPO$ID_POM AND vs.SPRES$INSTITUCION = POM.SPPO$ID_INSTITUCION AND POM.SPPO$RESTRICTIVA = 'N'" +
                " INNER JOIN SCHE$SIPLAN20.SP20$PERIODO P ON POM.SPPO$ID_PERIODO = P.SPP$ID_PERIODO AND POM.SPPO$RESTRICTIVA = 'N' AND P.SPP$RESTRICTIVA = 'N' AND P.SPP$ID_PERIODO = " + cbPeriodos.Value +
                "WHERE SPPSUB$SNIP IS NOT NULL)  WHERE SPRES$POM = 720 AND SPRES$INSTITUCION = 13000";

            estado = dao.consulta(sql);
            if (estado == 1)           
                tabla = dao.tabla;
            if (tabla.Rows.Count > 0)
            {
                for (int i = 0; i < tabla.Rows.Count; i++)
                {
                    if (tabla.Rows[i]["POA"] != DBNull.Value)
                    {
                        if (tabla.Rows[i]["IDFISICO"] == DBNull.Value)
                        {
                            sql = "INSERT INTO SCHE$SIPLAN20.SP20$PROGRAMETA_FI_FIN_SUB(SPPMFS$TIPO_PROGRAMACION, SPPMFS$META,SPPMFS$ID_POA,SPPMFS$ANIO, SPPMFS$ID_SUBPRODUCTO,SPPMFS$FECHA_INSERTA) VALUES(" + 0+","+ tabla.Rows[i]["METAFISICA"]+","+ tabla.Rows[i]["POA"] + ","+anio+", "+ tabla.Rows[i]["SPPSUB$ID_SUBPRODUCTO"] + ",'INSERTA = '||TO_CHAR(SYSDATE,'DD/MM/YYYY HH:MI')||' '||'" + Session["USUARIO"].ToString() + "')";
                            estado = dao.comando(sql);
                            /*if (estado == 0)
                                break;*/
                        }

                        else
                        {
                            sql = "UPDATE SCHE$SIPLAN20.SP20$PROGRAMETA_FI_FIN_SUB SET SPPMFS$META  = "+ tabla.Rows[i]["METAFISICA"] + ",  SPPMFS$FECHA_UPDATE = 'UPDATE = '||TO_CHAR(SYSDATE,'DD/MM/YYYY HH:MI')||' '||'" + Session["USUARIO"].ToString() + "'  WHERE SPPMFS$ID_PROGRAMACION_FISFIN = " + tabla.Rows[i]["IDFISICO"]+ " AND SPPMFS$ID_SUBPRODUCTO = "+ tabla.Rows[i]["SPPSUB$ID_SUBPRODUCTO"] + " AND SPPMFS$TIPO_PROGRAMACION = 0 AND SPPMFS$RESTRICTIVA ='N'";
                            estado = dao.comando(sql);
                            /*if (estado == 0)
                                break;*/
                        }

                        if (tabla.Rows[i]["IDFINANCIERO"] == DBNull.Value)
                        {
                            sql = "INSERT INTO SCHE$SIPLAN20.SP20$PROGRAMETA_FI_FIN_SUB(SPPMFS$TIPO_PROGRAMACION, SPPMFS$META,SPPMFS$ID_POA,SPPMFS$ANIO, SPPMFS$ID_SUBPRODUCTO,SPPMFS$FECHA_INSERTA) VALUES(" + 1 + "," + tabla.Rows[i]["METAFINANCIERA"] + "," + tabla.Rows[i]["POA"] + "," + anio + ", " + tabla.Rows[i]["SPPSUB$ID_SUBPRODUCTO"] + ",'INSERTA = '||TO_CHAR(SYSDATE,'DD/MM/YYYY HH:MI')||' '||'" + Session["USUARIO"].ToString() + "')";
                            /*estado = dao.comando(sql);
                            if (estado == 0)
                                break;*/
                        }
                        else
                        {
                            sql = "UPDATE SCHE$SIPLAN20.SP20$PROGRAMETA_FI_FIN_SUB SET SPPMFS$META  = " + tabla.Rows[i]["METAFINANCIERA"] + ",  SPPMFS$FECHA_UPDATE = 'UPDATE = '||TO_CHAR(SYSDATE,'DD/MM/YYYY HH:MI')||' '||'" + Session["USUARIO"].ToString() + "'  WHERE SPPMFS$ID_PROGRAMACION_FISFIN = " + tabla.Rows[i]["IDFINANCIERO"] + " AND SPPMFS$ID_SUBPRODUCTO = " + tabla.Rows[i]["SPPSUB$ID_SUBPRODUCTO"] + " AND SPPMFS$TIPO_PROGRAMACION = 1 AND SPPMFS$RESTRICTIVA = 'N'";
                            estado = dao.comando(sql);
                            /*if (estado == 0)
                                break;*/
                        }
                    }

                }
            }
                           

        }

        
        protected void cargaAvances(int anio, int mes)
        {
            sql = @"SELECT POA, POM, INSTO, IDSUB, SNIP, NOMBRESUB, ENCABEZADO ,(SELECT AV.SPPAFF$PROGRA_FISICO_FIN FROM SCHE$SIPLAN20.SP20$AVANCEFIFISUB AV WHERE AV.SPPAFF$ENCABEZADOSUB = ENCABEZADO AND AV.SPPAFF$TIPO_AVANCE = 0 AND AV.SPPAFF$SUBPRODUCTO = IDSUB) IDFISICO
                    ,sche$siplan.splfnc$avance_meta_snip("+anio+@",SNIP,"+mes+@") AVANCEFISICO
                    ,(SELECT AV.SPPAFF$PROGRA_FISICO_FIN FROM SCHE$SIPLAN20.SP20$AVANCEFIFISUB AV WHERE AV.SPPAFF$ENCABEZADOSUB = ENCABEZADO AND AV.SPPAFF$TIPO_AVANCE = 1 AND AV.SPPAFF$SUBPRODUCTO = IDSUB) IDFINANCIERO
                    ,sche$siplan.splfnc$avance_pres_snip("+anio+@",SNIP,"+mes+@") AVANCEFINANCIERO
                    FROM (SELECT POA, SPRES$POM POM, SPRES$INSTITUCION INSTO, SPPSUB$ID_SUBPRODUCTO IDSUB, SPPSUB$SNIP SNIP, NOMBRE_SNIP NOMBRESUB
                    ,(SELECT EN.SPPEFFS$ID_ENCABEZADO FROM SCHE$SIPLAN20.SP20$ENCABEZ_MFIFISSUB EN WHERE EN.SPPEFFS$POA = POA AND EN.SPPEFFS$MES = "+mes+@" AND EN.SPPEFFS$RESTRICTIVA = 'N') ENCABEZADO 
                    FROM (SELECT vs.SPRES$POM, vs.SPRES$INSTITUCION, vs.SPPSUB$ID_SUBPRODUCTO, vs.SPPSUB$SNIP, vs.NOMBRE_SNIP, (SELECT SPOA$ID_POA FROM SCHE$SIPLAN20.SP20$POA WHERE SPOA$ID_POM = vs.SPRES$POM AND  SPOA$ANIO = "+anio+ @" AND SPOA$RESTRICTIVA = 'N' )POA  FROM SCHE$SIPLAN20.SPPVST$PRODUCCIONINSTO vs
INNER JOIN SCHE$SIPLAN20.SP20$POM POM ON vs.SPRES$POM = POM.SPPO$ID_POM AND vs.SPRES$INSTITUCION = POM.SPPO$ID_INSTITUCION AND POM.SPPO$RESTRICTIVA = 'N'
INNER JOIN SCHE$SIPLAN20.SP20$PERIODO P ON POM.SPPO$ID_PERIODO = P.SPP$ID_PERIODO AND POM.SPPO$RESTRICTIVA = 'N' AND P.SPP$RESTRICTIVA = 'N' AND P.SPP$ID_PERIODO = "+cbPeriodos.Value+ @"

WHERE SPPSUB$SNIP IS NOT NULL)) WHERE POM = 720 AND INSTO = 13000";
            estado = dao.consulta(sql);
            if (estado == 1)
            {
                tabla = dao.tabla;
                gvAvances.DataSource = tabla;
                gvAvances.DataBind();
            }
        }


        protected void btnAvances_Click(object sender, EventArgs e)
        {
            int anio = 0;
            int mes = 0;
            anio = Convert.ToInt32(cbanio.Value);
            mes = Convert.ToInt32(cbMes.Value);
            int idDet = -1;

            sql = @"SELECT POA, POM, INSTO, IDSUB, SNIP, NOMBRESUB, ENCABEZADO ,(SELECT AV.SPPAFF$PROGRA_FISICO_FIN FROM SCHE$SIPLAN20.SP20$AVANCEFIFISUB AV WHERE AV.SPPAFF$ENCABEZADOSUB = ENCABEZADO AND AV.SPPAFF$TIPO_AVANCE = 0 AND AV.SPPAFF$SUBPRODUCTO = IDSUB) IDFISICO
                    ,sche$siplan.splfnc$avance_meta_snip(" + anio + @",SNIP," + mes + @") AVANCEFISICO
                    ,(SELECT AV.SPPAFF$PROGRA_FISICO_FIN FROM SCHE$SIPLAN20.SP20$AVANCEFIFISUB AV WHERE AV.SPPAFF$ENCABEZADOSUB = ENCABEZADO AND AV.SPPAFF$TIPO_AVANCE = 1 AND AV.SPPAFF$SUBPRODUCTO = IDSUB) IDFINANCIERO
                    ,sche$siplan.splfnc$avance_pres_snip(" + anio + @",SNIP," + mes + @") AVANCEFINANCIERO
                    FROM (SELECT POA, SPRES$POM POM, SPRES$INSTITUCION INSTO, SPPSUB$ID_SUBPRODUCTO IDSUB, SPPSUB$SNIP SNIP, NOMBRE_SNIP NOMBRESUB
                    ,(SELECT EN.SPPEFFS$ID_ENCABEZADO FROM SCHE$SIPLAN20.SP20$ENCABEZ_MFIFISSUB EN WHERE EN.SPPEFFS$POA = POA AND EN.SPPEFFS$MES = " + mes + @" AND EN.SPPEFFS$RESTRICTIVA = 'N') ENCABEZADO 
                    FROM (SELECT vs.SPRES$POM, vs.SPRES$INSTITUCION, vs.SPPSUB$ID_SUBPRODUCTO, vs.SPPSUB$SNIP, vs.NOMBRE_SNIP, (SELECT SPOA$ID_POA FROM SCHE$SIPLAN20.SP20$POA WHERE SPOA$ID_POM = vs.SPRES$POM AND  SPOA$ANIO = " + anio + @" AND SPOA$RESTRICTIVA = 'N' )POA  FROM SCHE$SIPLAN20.SPPVST$PRODUCCIONINSTO vs 
INNER JOIN SCHE$SIPLAN20.SP20$POM POM ON vs.SPRES$POM = POM.SPPO$ID_POM AND vs.SPRES$INSTITUCION = POM.SPPO$ID_INSTITUCION AND POM.SPPO$RESTRICTIVA = 'N'
INNER JOIN SCHE$SIPLAN20.SP20$PERIODO P ON POM.SPPO$ID_PERIODO = P.SPP$ID_PERIODO AND POM.SPPO$RESTRICTIVA = 'N' AND P.SPP$RESTRICTIVA = 'N' AND P.SPP$ID_PERIODO = "+cbPeriodos.Value+ @"
WHERE SPPSUB$SNIP IS NOT NULL)) WHERE POM = 720 AND INSTO = 13000";

            estado = dao.consulta(sql);
            if (estado == 1)            
                tabla = dao.tabla;

            if (tabla.Rows.Count > 0)
            {
                for (int i = 0; i < tabla.Rows.Count; i++)
                {
                    if (tabla.Rows[i]["POA"] != DBNull.Value)
                    {
                        if (tabla.Rows[i]["ENCABEZADO"] != DBNull.Value)
                        {
                            if (tabla.Rows[i]["IDFISICO"] == DBNull.Value)
                            {
                                idDet = CUENTA("SCHE$SIPLAN20.SP20$AVANCEFIFISUB", "", "SPPAFF$PROGRA_FISICO_FIN");

                                sql = "INSERT INTO SCHE$SIPLAN20.SP20$AVANCEFIFISUB (SPPAFF$PROGRA_FISICO_FIN, SPPAFF$AVANCE, SPPAFF$TIPO_AVANCE, SPPAFF$SUBPRODUCTO, SPPAFF$ENCABEZADOSUB, SPPAFF$FECHA_INSERTA) VALUES ("+idDet+"," + tabla.Rows[i]["AVANCEFISICO"] + ", 0," + tabla.Rows[i]["IDSUB"] + ", " + tabla.Rows[i]["ENCABEZADO"] + ", 'INSERTA = '||TO_CHAR(SYSDATE,'DD/MM/YYYY HH:MI')||' '||'" + Session["USUARIO"].ToString() + "' )";
                                estado = dao.comando(sql);
                                /*if (estado == 0)
                                    break;*/
                            }
                            else
                            {
                                sql = "UPDATE SCHE$SIPLAN20.SP20$AVANCEFIFISUB SET SPPAFF$AVANCE = "+ tabla.Rows[i]["AVANCEFISICO"]+ ", SPPAFF$FECHA_UPDATE = 'UPDATE = '||TO_CHAR(SYSDATE,'DD/MM/YYYY HH:MI')||' '||'" + Session["USUARIO"].ToString() + "' WHERE SPPAFF$PROGRA_FISICO_FIN = "+ tabla.Rows[i]["IDFISICO"] + " AND SPPAFF$TIPO_AVANCE = 0 AND SPPAFF$ENCABEZADOSUB = " + tabla.Rows[i]["ENCABEZADO"]+ " AND SPPAFF$SUBPRODUCTO = "+ tabla.Rows[i]["IDSUB"]+ " AND SPPAFF$RESTRICTIVA = 'N'";
                                estado = dao.comando(sql);
                                /*if (estado == 0)
                                    break;*/
                            }


                            if (tabla.Rows[i]["IDFINANCIERO"] == DBNull.Value)
                            {
                                idDet = CUENTA("SCHE$SIPLAN20.SP20$AVANCEFIFISUB", "", "SPPAFF$PROGRA_FISICO_FIN");
                                sql = "INSERT INTO SCHE$SIPLAN20.SP20$AVANCEFIFISUB (SPPAFF$PROGRA_FISICO_FIN, SPPAFF$AVANCE, SPPAFF$TIPO_AVANCE, SPPAFF$SUBPRODUCTO, SPPAFF$ENCABEZADOSUB, SPPAFF$FECHA_INSERTA) VALUES ("+idDet+"," + tabla.Rows[i]["AVANCEFINANCIERO"] + ", 1," + tabla.Rows[i]["IDSUB"] + ", " + tabla.Rows[i]["ENCABEZADO"] + ", 'INSERTA = '||TO_CHAR(SYSDATE,'DD/MM/YYYY HH:MI')||' '||'" + Session["USUARIO"].ToString() + "' )";
                                estado = dao.comando(sql);
                                /*if (estado == 0)
                                    break;*/
                            }
                            else
                            {
                                sql = "UPDATE SCHE$SIPLAN20.SP20$AVANCEFIFISUB SET SPPAFF$AVANCE = " + tabla.Rows[i]["AVANCEFINANCIERO"] + ", SPPAFF$FECHA_UPDATE = 'UPDATE = '||TO_CHAR(SYSDATE,'DD/MM/YYYY HH:MI')||' '||'" + Session["USUARIO"].ToString() + "' WHERE SPPAFF$PROGRA_FISICO_FIN = " + tabla.Rows[i]["IDFINANCIERO"] + " AND SPPAFF$TIPO_AVANCE = 1 AND SPPAFF$ENCABEZADOSUB = " + tabla.Rows[i]["ENCABEZADO"] + " AND SPPAFF$SUBPRODUCTO = " + tabla.Rows[i]["IDSUB"] + " AND SPPAFF$RESTRICTIVA = 'N'";
                                estado = dao.comando(sql);
                                /*if (estado == 0)
                                    break;*/
                            }

                        }
                    }
                }

            }
            


        }


        private int CUENTA(string pTabla, string pCondicion, string pCampo)
        {
            int res = 0;
            string query = "";
            DataTable tablas = new DataTable();

            query = "SELECT MAX(" + pCampo + ") FROM " + pTabla;
            if (pCondicion != "")
                query += " WHERE " + pCondicion;

            try
            {
                estado = dao.consulta(query);
                if (estado != 0)
                {
                    tablas = dao.tabla;
                    res = Convert.ToInt32(tablas.Rows[0][0].ToString()) + 1;
                }

            }
            catch
            {
                res = 1;
            }

            return res;
        }

        protected void cargaPeriodos()
        {
            sql = "SELECT SPP$ID_PERIODO, SPP$INICIO||'-'||SPP$FINAL PERIODO FROM SCHE$SIPLAN20.SP20$PERIODO WHERE SPP$RESTRICTIVA = 'N'";
            estado = dao.consulta(sql);
            if (estado == 0)
            {
                mensaje = dao.mensaje;
                Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
            }
            else
            {
                tabla.Rows.Clear();
                tabla = dao.tabla;
                cbPeriodos.DataSource = tabla;
                cbPeriodos.ValueField = "SPP$ID_PERIODO";
                cbPeriodos.TextField = "PERIODO";
                cbPeriodos.DataBind();

                if (!IsPostBack)
                    cbPeriodos.SelectedIndex = 0;

            }

        }


    }
}