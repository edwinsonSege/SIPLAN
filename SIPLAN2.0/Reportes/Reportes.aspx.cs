using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web;
using SIPLAN2._0.DataAccess;
namespace SIPLAN2._0.Reportes
{
    public partial class Reportes : System.Web.UI.Page
    {
        DateTime date1 = new DateTime();
        int anio;
        string sql, mensaje, enlace, parametro;
       
        
        int estado = 0;
        int detalle = -1;
        clsAccesoBBDD dao = new clsAccesoBBDD();
        DataTable tabla = new DataTable();
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Usuario"] == null)
                Response.Redirect("../login/login.aspx");
            else
            {


                cargacomboEjecucion();


                if (Session["ROL"].ToString() == "ADMIN" || Session["ROL"].ToString() == "ENTIDAD")
                {
                    if (Session["ROL"].ToString() == "ADMIN")
                        VistaReportes.TabPages[2].ClientVisible = true;
                    else
                        VistaReportes.TabPages[2].ClientVisible = false;


                    if (!IsPostBack)
                    {
                        cargaCombos();
                        cargaPeriodos();
                        anios();
                        cargaEjecución();
                        cargaProductosInsto();
                        cargaComboTematica1();                      
                        cargaComboT(Convert.ToInt32(cbTematica.Value));


                    }
                    else
                    {
                        cargaEjecución();
                        cargaProductosInsto();
                        /*
                        if (cbDetalleTematica2.Text != "")
                            detalle = Convert.ToInt32(cbDetalleTematica2.Value);
                        else
                            detalle = -1;
                        cargaProdVinculada(detalle);
                        */
                    }
                       
                }
                else
                {
                    Response.Redirect("../login/login.aspx");
                }
                
            }
            
           
          
         
        }

        protected void cargacomboEjecucion()
        {
            sql = @"SELECT SPPFC$ID
                        ,TO_CHAR(SPPFC$FECHA_FIN,'dd/mm/yyyy') FECHA_CORTE
                        ,SPPFC$REALIZADO FROM SCHE$SIPLAN20.SP20$FECHA_CORTE WHERE
                        SPPFC$RESTRICTIVA = 'N' AND SPPFC$REALIZADO = 0 order by SPPFC$REALIZADO ASC, SPPFC$FECHA_FIN ASC";
            estado = dao.consulta(sql);

            if (estado == 1)
            {
                cbFechaCorte.DataSource = null;
                cbFechaCorte.Items.Clear();
                tabla = dao.tabla;
                cbFechaCorte.DataSource = tabla;
                cbFechaCorte.ValueField = "SPPFC$ID";
                cbFechaCorte.TextField = "FECHA_CORTE";
                cbFechaCorte.DataBind();
                if (!IsPostBack)
                    cbFechaCorte.SelectedIndex = 0;
            }
        }

        protected void cargaEjecución()
        {
            DataTable dtDatos = new DataTable();
            DataTable poms = new DataTable();
            sql = "SELECT * FROM SCHE$SIPLAN20.SP20$POM WHERE SPPO$ID_INSTITUCION = " + cbInstituiciones.Value + " AND SPPO$ID_PERIODO = " + cbPeriodo.Value + " AND SPPO$RESTRICTIVA = 'N'";
            estado = dao.consulta(sql);

            if (estado == 1)
            {
                poms = dao.tabla;
                if (poms.Rows.Count > 0)
                {
                    gvEjecucion.DataSource = null;
                    gvEjecucion.DataBind();
                    dtDatos = loadProductos(Convert.ToInt32(cbPeriodo.Value), Convert.ToInt32(cbInstituiciones.Value), Convert.ToInt32(cbanio.Value), Convert.ToInt32(poms.Rows[0]["SPPO$ID_POM"].ToString()));
                    gvEjecucion.DataSource = dtDatos;
                    gvEjecucion.DataBind();
                }

                
            }

           

        }
        protected void cargaCombos()
        {

            sql = "SELECT CG.ENTIDAD, CG.NOMBRE||' '||CG.SIGLAS INSTITUCION, CG.SECTOR FROM SINIP.CG_ENTIDADES CG WHERE (ENTIDAD > 1000 OR ENTIDAD = 69 OR ENTIDAD = 20) AND CG.RESTRICTIVA = 'N' AND CG.NOMBRE NOT LIKE ('%MUNICIPALIDAD%%') AND CG.ENTIDAD NOT IN 777777 ORDER BY CG.NOMBRE ASC";
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
                cbInstituiciones.DataSource = tabla;
                cbInstituiciones.ValueField = "ENTIDAD";
                cbInstituiciones.TextField = "INSTITUCION";
                cbInstituiciones.DataBind();

                if (Session["ROL"].ToString() == "USER")
                {
                    cbInstituiciones.Value = Convert.ToInt32(Session["Insto"]).ToString();
                    cbInstituiciones.Enabled = false;
                }
                else
                {
                    cbInstituiciones.Value = Convert.ToInt32(Session["Insto"]).ToString();
                    cbInstituiciones.Enabled = true;
                }


            }


        }

       protected void cargaPeriodos()
        {
            sql = "SELECT SPP$ID_PERIODO, SPP$INICIO||'-'||SPP$FINAL PERIODO FROM SCHE$SIPLAN20.SP20$PERIODO WHERE SPP$RESTRICTIVA = 'N' ORDER BY SPP$VIGENTE DESC, SPP$ORDEN DESC";
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
                cbPeriodo.DataSource = tabla;
                cbPeriodo.ValueField = "SPP$ID_PERIODO";
                cbPeriodo.TextField = "PERIODO";
                cbPeriodo.DataBind();

                if (!IsPostBack)
                    cbPeriodo.SelectedIndex= 0;

            }

        }
        protected void ReporteAvance_Click(object sender, EventArgs e)
        {
            MultiView1.ActiveViewIndex = 0;
        }

        protected void btnRegresarinicio_Click(object sender, EventArgs e)
        {
            Response.Redirect("../modulos/frmModulos.aspx");
        }

        protected void btnReporte_Click(object sender, EventArgs e)
        {
            /*sql = @"SELECT CG.NOMBRE INSTITUCION, NULL 'RESULTADO INSTITUCIONAL', VES.PILAR_PGG 'PILAR PGG', NULL 'OBJETIVO SECTORIAL', NULL 'ACCION ESTRATEGICA',VES.SPPRES$DESCRIPCION METAPGG, VES.SPPRO$ID_PRODUCTO 'CODIGO PRODUCTO', VES.SPPRO$DESCRIPCION PRODUCTO, VES.SNCGUM$NOMBRE 'MEDIDA PRODUCTO'
                    ,NVL((SELECT SPPMFS$META  FROM SCHE$SIPLAN20.SP20$PROGRAMACION_MFPRODUCTO WHERE SPPMFS$ID_PRODUCTO = VES.SPPRO$ID_PRODUCTO AND SPPMFS$RESTRICTIVA = 'N' AND SPPMF$TPROGRA = 0 AND SPPMFS$ID_POA = POA.SPOA$ID_POA AND SPPMFS$ANIO = POA.SPOA$ANIO AND SPPMF$TIPO = 0),0) 'FISICA INICIAL'
                    ,NVL(SCHE$SIPLAN20.FNC$NUMEROMETAFISICAS(POA.SPOA$ID_POA, POA.SPOA$ANIO, VES.SPPRO$ID_PRODUCTO), 0) 'NO. DE MODIFICACIONES'
                    ,NVL((SELECT SPPMFS$META  FROM SCHE$SIPLAN20.SP20$PROGRAMACION_MFPRODUCTO WHERE SPPMFS$ID_PRODUCTO = VES.SPPRO$ID_PRODUCTO AND SPPMFS$RESTRICTIVA = 'N' AND SPPMFS$VIGENTE = 0 AND SPPMFS$ID_POA = POA.SPOA$ID_POA AND SPPMFS$ANIO = POA.SPOA$ANIO AND SPPMF$TIPO = 0),0) 'FISICA VIGENTE'
                    ,SCHE$SIPLAN20.FNC$OBTIENEMETAFINACIERAPROD(POA.SPOA$ID_POA, POA.SPOA$ANIO, VES.SPPRO$ID_PRODUCTO, 1) 'FINANCIERA INICIAL'
                    ,SCHE$SIPLAN20.FNC$OBTIENEMETAFINACIERAPROD(POA.SPOA$ID_POA, POA.SPOA$ANIO, VES.SPPRO$ID_PRODUCTO, 3) 'NO REPROGRAMACIONES'
                    ,SCHE$SIPLAN20.FNC$OBTIENEMETAFINACIERAPROD(POA.SPOA$ID_POA, POA.SPOA$ANIO, VES.SPPRO$ID_PRODUCTO, 0) 'FINANCIERA VIGENTE'
                    ,VES.SPPSUB$ID_SUBPRODUCTO 'CODIGO SUBPRODUCTO'
                    ,CASE WHEN VES.SPPSUB$SNIP IS NULL THEN VES.SPPSUB$DESCRIPCION ELSE VES.NOMBRE_SNIP END AS SUBPRODUCTO
                    , CASE WHEN VES.SPPSUB$SNIP IS NULL THEN VES.MEDIDA_SUB ELSE SCHE$SIPLAN20.FNCOBTIENESNIP(VES.SPPSUB$SNIP, POA.SPOA$ANIO) END AS 'MEDIDA SUBPRODUCTO'
                    ,NVL(SCHE$SIPLAN20.FNC$OBTIENEMETAFISICASUB(POA.SPOA$ID_POA, POA.SPOA$ANIO, VES.SPPSUB$ID_SUBPRODUCTO, 1), 0) 'FISICO ANUAL'
                    ,(SELECT COUNT(SPPMFS$ID_PROGRAMACION_FISFIN) CONTFI FROM SCHE$SIPLAN20.SP20$PROGRAMETA_FI_FIN_SUB SUBFIN
                    INNER JOIN SCHE$SIPLAN20.SP20$REPROGRASUB RS ON SUBFIN.SPPMFS$ID_PROGRAMACION_FISFIN = RS.SPPRS$PROGRASUB AND SUBFIN.SPPMFS$RESTRICTIVA = 'N' AND RS.SPPRS$RESTRICTIVA = 'N'
                    INNER JOIN SCHE$SIPLAN20.SP20$REPROGRAMACION RE ON RS.SPPRS$REPROGRAMACION = RE.SPPRE$ID_REPRO AND RS.SPPRS$RESTRICTIVA = 'N' AND RE.SPPRE$RESTRICTIVA = 'N' AND RE.SPPRE$APROBADO = 1
                    WHERE SPPMFS$ID_SUBPRODUCTO = VES.SPPSUB$ID_SUBPRODUCTO AND SPPMFS$RESTRICTIVA = 'N' AND SPPMFS$ID_POA = POA.SPOA$ID_POA AND SPPMFS$ANIO = POA.SPOA$ANIO AND SPPMFS$TIPO_PROGRAMACION = 0) 'NO. DE REPROGRAMACIONES'
                    ,NVL(SCHE$SIPLAN20.FNC$OBTIENEMETAFISICASUB(POA.SPOA$ID_POA, POA.SPOA$ANIO, VES.SPPSUB$ID_SUBPRODUCTO, 0), 0)'FISICO VIGENTE'
                    ,NVL(SCHE$SIPLAN20.FNC$METAFINANCIERASUB(POA.SPOA$ID_POA, POA.SPOA$ANIO, VES.SPPSUB$ID_SUBPRODUCTO, 1), 0) "FINANCIERO ANUAL"
,(SELECT COUNT(SPPMFS$ID_PROGRAMACION_FISFIN) CONTFI FROM SCHE$SIPLAN20.SP20$PROGRAMETA_FI_FIN_SUB SUBFIN
INNER JOIN SCHE$SIPLAN20.SP20$REPROGRASUB RS ON SUBFIN.SPPMFS$ID_PROGRAMACION_FISFIN = RS.SPPRS$PROGRASUB AND SUBFIN.SPPMFS$RESTRICTIVA = 'N' AND RS.SPPRS$RESTRICTIVA = 'N'
INNER JOIN SCHE$SIPLAN20.SP20$REPROGRAMACION RE ON RS.SPPRS$REPROGRAMACION = RE.SPPRE$ID_REPRO AND RS.SPPRS$RESTRICTIVA = 'N' AND RE.SPPRE$RESTRICTIVA = 'N' AND RE.SPPRE$APROBADO = 1
WHERE SPPMFS$ID_SUBPRODUCTO = VES.SPPSUB$ID_SUBPRODUCTO AND SPPMFS$RESTRICTIVA = 'N' AND SPPMFS$ID_POA = POA.SPOA$ID_POA AND SPPMFS$ANIO = POA.SPOA$ANIO AND SPPMFS$TIPO_PROGRAMACION = 1 ) "REPROGRAMACIONES"
,NVL(SCHE$SIPLAN20.FNC$METAFINANCIERASUB(POA.SPOA$ID_POA, POA.SPOA$ANIO, VES.SPPSUB$ID_SUBPRODUCTO, 0), 0) "FINANCIERO VIGENTE"
FROM SINIP.CG_ENTIDADES CG INNER JOIN
SCHE$SIPLAN20.SP20$POM POM ON CG.ENTIDAD = POM.SPPO$ID_INSTITUCION AND CG.RESTRICTIVA = 'N'  AND POM.SPPO$RESTRICTIVA = 'N'
INNER JOIN SCHE$SIPLAN20.SP20$PERIODO PE ON POM.SPPO$ID_PERIODO = PE.SPP$ID_PERIODO AND POM.SPPO$RESTRICTIVA = 'N'  AND PE.SPP$RESTRICTIVA = 'N'
INNER JOIN SCHE$SIPLAN20.SP20$POA POA ON POM.SPPO$ID_POM = POA.SPOA$ID_POM AND POM.SPPO$ID_INSTITUCION = POA.SPOA$ID_INSTITUCION AND POM.SPPO$RESTRICTIVA = 'N' AND POA.SPOA$RESTRICTIVA = 'N'
INNER JOIN SCHE$SIPLAN20.SPPVST$PRODUCCIONINSTO VES ON VES.SPRES$POM = POM.SPPO$ID_POM
WHERE  POA.SPOA$ANIO = 2020 AND PE.SPP$ID_PERIODO = 0  AND VES.PILAR_PGG IS NOT NULL AND VES.SPRES$INSTITUCION = 16220
UNION
SELECT CG.NOMBRE INSTITUCION, VES.SPPRES$DESCRIPCION "RESULTADO INSTITUCIONAL", VES.METAPGG "PILAR PGG", VES.OBJETIVO_CENTRAL "OBJETIVO SECTORIAL", VES.ACCION_ESTRATEGICA "ACCION ESTRATEGICA", NULL METAPGG, VES.SPPRO$ID_PRODUCTO "CODIGO PRODUCTO", VES.SPPRO$DESCRIPCION PRODUCTO, VES.SNCGUM$NOMBRE "MEDIDA PRODUCTO"
,NVL((SELECT SPPMFS$META  FROM SCHE$SIPLAN20.SP20$PROGRAMACION_MFPRODUCTO WHERE SPPMFS$ID_PRODUCTO = VES.SPPRO$ID_PRODUCTO AND SPPMFS$RESTRICTIVA = 'N' AND SPPMF$TPROGRA = 0 AND SPPMFS$ID_POA = POA.SPOA$ID_POA AND SPPMFS$ANIO = POA.SPOA$ANIO AND SPPMF$TIPO = 0),0) "FISICA INICIAL"
,NVL(SCHE$SIPLAN20.FNC$NUMEROMETAFISICAS(POA.SPOA$ID_POA, POA.SPOA$ANIO, VES.SPPRO$ID_PRODUCTO), 0) "NO.DE MODIFICACIONES"
,NVL((SELECT SPPMFS$META  FROM SCHE$SIPLAN20.SP20$PROGRAMACION_MFPRODUCTO WHERE SPPMFS$ID_PRODUCTO = VES.SPPRO$ID_PRODUCTO AND SPPMFS$RESTRICTIVA = 'N' AND SPPMFS$VIGENTE = 0 AND SPPMFS$ID_POA = POA.SPOA$ID_POA AND SPPMFS$ANIO = POA.SPOA$ANIO AND SPPMF$TIPO = 0),0) "FISICA VIGENTE"
,SCHE$SIPLAN20.FNC$OBTIENEMETAFINACIERAPROD(POA.SPOA$ID_POA, POA.SPOA$ANIO, VES.SPPRO$ID_PRODUCTO, 1) "FINANCIERA INICIAL"
,SCHE$SIPLAN20.FNC$OBTIENEMETAFINACIERAPROD(POA.SPOA$ID_POA, POA.SPOA$ANIO, VES.SPPRO$ID_PRODUCTO, 3) "NO REPROGRAMACIONES"
,SCHE$SIPLAN20.FNC$OBTIENEMETAFINACIERAPROD(POA.SPOA$ID_POA, POA.SPOA$ANIO, VES.SPPRO$ID_PRODUCTO, 0) "FINANCIERA VIGENTE"
,VES.SPPSUB$ID_SUBPRODUCTO "CODIGO SUBPRODUCTO"
,CASE WHEN VES.SPPSUB$SNIP IS NULL THEN VES.SPPSUB$DESCRIPCION ELSE VES.NOMBRE_SNIP END AS SUBPRODUCTO
, CASE WHEN VES.SPPSUB$SNIP IS NULL THEN VES.MEDIDA_SUB ELSE SCHE$SIPLAN20.FNCOBTIENESNIP(VES.SPPSUB$SNIP, POA.SPOA$ANIO) END AS "MEDIDA SUBPRODUCTO"
,NVL(SCHE$SIPLAN20.FNC$OBTIENEMETAFISICASUB(POA.SPOA$ID_POA, POA.SPOA$ANIO, VES.SPPSUB$ID_SUBPRODUCTO,1),0) "FISICO ANUAL"
,(SELECT COUNT(SPPMFS$ID_PROGRAMACION_FISFIN) CONTFI FROM SCHE$SIPLAN20.SP20$PROGRAMETA_FI_FIN_SUB SUBFIN
INNER JOIN SCHE$SIPLAN20.SP20$REPROGRASUB RS ON SUBFIN.SPPMFS$ID_PROGRAMACION_FISFIN = RS.SPPRS$PROGRASUB AND SUBFIN.SPPMFS$RESTRICTIVA = 'N' AND RS.SPPRS$RESTRICTIVA = 'N'
INNER JOIN SCHE$SIPLAN20.SP20$REPROGRAMACION RE ON RS.SPPRS$REPROGRAMACION = RE.SPPRE$ID_REPRO AND RS.SPPRS$RESTRICTIVA  = 'N' AND RE.SPPRE$RESTRICTIVA = 'N' AND RE.SPPRE$APROBADO = 1
WHERE SPPMFS$ID_SUBPRODUCTO = VES.SPPSUB$ID_SUBPRODUCTO AND SPPMFS$RESTRICTIVA = 'N' AND SPPMFS$ID_POA  = POA.SPOA$ID_POA AND SPPMFS$ANIO = POA.SPOA$ANIO AND SPPMFS$TIPO_PROGRAMACION = 0) "NO. DE REPROGRAMACIONES"
,NVL(SCHE$SIPLAN20.FNC$OBTIENEMETAFISICASUB(POA.SPOA$ID_POA, POA.SPOA$ANIO, VES.SPPSUB$ID_SUBPRODUCTO,0),0) "FISICO VIGENTE"
,NVL(SCHE$SIPLAN20.FNC$METAFINANCIERASUB(POA.SPOA$ID_POA, POA.SPOA$ANIO,VES.SPPSUB$ID_SUBPRODUCTO,1),0) "FINANCIERO ANUAL"
,(SELECT COUNT(SPPMFS$ID_PROGRAMACION_FISFIN) CONTFI FROM SCHE$SIPLAN20.SP20$PROGRAMETA_FI_FIN_SUB SUBFIN
INNER JOIN SCHE$SIPLAN20.SP20$REPROGRASUB RS ON SUBFIN.SPPMFS$ID_PROGRAMACION_FISFIN = RS.SPPRS$PROGRASUB AND SUBFIN.SPPMFS$RESTRICTIVA = 'N' AND RS.SPPRS$RESTRICTIVA = 'N'
INNER JOIN SCHE$SIPLAN20.SP20$REPROGRAMACION RE ON RS.SPPRS$REPROGRAMACION = RE.SPPRE$ID_REPRO AND RS.SPPRS$RESTRICTIVA  = 'N' AND RE.SPPRE$RESTRICTIVA = 'N' AND RE.SPPRE$APROBADO = 1
WHERE SPPMFS$ID_SUBPRODUCTO = VES.SPPSUB$ID_SUBPRODUCTO AND SPPMFS$RESTRICTIVA = 'N' AND SPPMFS$ID_POA  = POA.SPOA$ID_POA AND SPPMFS$ANIO = POA.SPOA$ANIO  AND SPPMFS$TIPO_PROGRAMACION = 1 ) "REPROGRAMACIONES"
,NVL(SCHE$SIPLAN20.FNC$METAFINANCIERASUB(POA.SPOA$ID_POA, POA.SPOA$ANIO,VES.SPPSUB$ID_SUBPRODUCTO,0),0) "FINANCIERO VIGENTE"
FROM SINIP.CG_ENTIDADES CG INNER JOIN
SCHE$SIPLAN20.SP20$POM POM ON CG.ENTIDAD = POM.SPPO$ID_INSTITUCION AND CG.RESTRICTIVA = 'N'  AND POM.SPPO$RESTRICTIVA = 'N' 
INNER JOIN SCHE$SIPLAN20.SP20$PERIODO PE ON POM.SPPO$ID_PERIODO = PE.SPP$ID_PERIODO AND POM.SPPO$RESTRICTIVA = 'N'  AND PE.SPP$RESTRICTIVA = 'N'
INNER JOIN SCHE$SIPLAN20.SP20$POA POA ON POM.SPPO$ID_POM = POA.SPOA$ID_POM AND POM.SPPO$ID_INSTITUCION = POA.SPOA$ID_INSTITUCION AND POM.SPPO$RESTRICTIVA = 'N' AND POA.SPOA$RESTRICTIVA = 'N'
INNER JOIN SCHE$SIPLAN20.SPPVST$PRODINSTITUCIONAL VES ON VES.SPRES$POM = POM.SPPO$ID_POM
WHERE  POA.SPOA$ANIO = 2020 AND PE.SPP$ID_PERIODO = 0 AND VES.SPRES$INSTITUCION = 16220
";*/
        }

        protected void btnTodasInsto_Click(object sender, EventArgs e)
        {
            int anio = Convert.ToInt32(cbanio.Value);
            string fecha = "";
            fecha = "01/01/" + anio.ToString();
            DateTime hoy = DateTime.Now;
            string fecha_hoy = hoy.ToString("dd/MM/yyyy");

            //if (fecha_hoy.Equals(cbFechaCorte.Text))
            //{
                sql = "SELECT * FROM SCHE$SIPLAN20.SP20$FECHA_CORTE WHERE SPPFC$ID = "+cbFechaCorte.Value;
                estado = dao.consulta(sql);
                if (estado == 1)
                {
                    tabla = dao.tabla;
                    if (tabla.Rows.Count > 0)
                    {
                        if (Convert.ToInt32(tabla.Rows[0]["SPPFC$REALIZADO"]) == 1)
                        {
                            mensaje = "El proceso corte para el " + cbFechaCorte.Text+" ya ha sido realizado, esto proceso solo puede hacerse una unica vez";
                            Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
                        }
                        else
                        {
                            sql = @"UPDATE SCHE$SIPLAN20.SP20$DETALLEAVANCEFI_PROD SET SPPAVP$AVANCE_CORTE = SPPAVP$AVANCE_FISICO, SPPAVP$UPDATE_CORTE = 'FECHA_CORTE = '||TO_CHAR(SYSDATE,'DD/MM/YYYY HH:MI')||' '||'" + Session["USUARIO"].ToString() + @"', SPPAV$ID_CORTE = " + cbFechaCorte.Value + @" WHERE SPPAVP$AVANCE_CORTE IS NULL AND SPPAVP$ID_PROGRA_FISICA IN
                    (SELECT 
                    AP.SPPAVP$ID_PROGRA_FISICA
                    FROM SCHE$SIPLAN20.SP20$POM POM
                    INNER JOIN SCHE$SIPLAN20.SP20$POA POA ON POM.SPPO$ID_POM = POA.SPOA$ID_POM AND POM.SPPO$RESTRICTIVA = 'N' AND POA.SPOA$RESTRICTIVA = 'N'
                    INNER JOIN SCHE$SIPLAN20.SP20$ENCABEZA_FISICO_PROD EP ON EP.SPPAVF$POA = POA.SPOA$ID_POA AND EP.SPPAVF$RESTRICTIVA = 'N' AND POA.SPOA$RESTRICTIVA = 'N'
                    INNER JOIN SCHE$SIPLAN20.SP20$DETALLEAVANCEFI_PROD AP ON EP.SPPAVF$ID_ENCABEZADO = AP.SPPAVP$ID_ENCABEZADO AND EP.SPPAVF$RESTRICTIVA = 'N' AND AP.SPPAVP$RESTRICTIVA = 'N'
                    WHERE POM.SPPO$RESTRICTIVA = 'N' AND POM.SPPO$ID_PERIODO = " + cbPeriodo.Value + " AND POA.SPOA$ANIO = " + cbanio.Value + @" AND AP.SPPAVP$AVANCE_FISICO IS NOT NULL AND EP.SPPAVF$MES
                    IN
                    (select EXTRACT(MONTH FROM fecha) mes
                    from
                    (SELECT
                    to_date(YEARMONTH,'dd/mm/yyyy') fecha
                    FROM

                    (SELECT TO_CHAR(
                                    add_months(to_date('" + fecha + @"', 'dd/mm/yyyy'), ROWNUM - 1),
                                    'dd/mm/yyyy'
                                  ) as yearMonth
                      FROM DUAL
                    CONNECT BY ROWNUM <= (
                                           select round(months_between(to_date('" + cbFechaCorte.Text + @"', 'dd/mm/yyyy'),
                                           to_date('" + fecha + @"', 'dd/mm/yyyy')),0) from dual
                                         )))))
                                         ";
                            estado = dao.comando(sql);

                            sql = @"UPDATE SCHE$SIPLAN20.SP20$AVANCEFIFISUB SET SPPAFF$AVANCECORTE = SPPAFF$AVANCE WHERE SPPAFF$AVANCECORTE IS NULL AND SPPAFF$PROGRA_FISICO_FIN IN
                            (SELECT
                            AV.SPPAFF$PROGRA_FISICO_FIN
                            FROM SCHE$SIPLAN20.SP20$POM POM
                            INNER JOIN SCHE$SIPLAN20.SP20$POA POA ON POM.SPPO$ID_POM = POA.SPOA$ID_POM AND POM.SPPO$RESTRICTIVA = 'N' AND POA.SPOA$RESTRICTIVA = 'N'
                            INNER JOIN SCHE$SIPLAN20.SP20$ENCABEZ_MFIFISSUB ES ON ES.SPPEFFS$POA = POA.SPOA$ID_POA AND ES.SPPEFFS$RESTRICTIVA = 'N' AND POA.SPOA$RESTRICTIVA = 'N'
                            INNER JOIN SCHE$SIPLAN20.SP20$AVANCEFIFISUB AV ON ES.SPPEFFS$ID_ENCABEZADO = AV.SPPAFF$ENCABEZADOSUB AND ES.SPPEFFS$RESTRICTIVA = 'N' AND AV.SPPAFF$RESTRICTIVA = 'N'
                            WHERE POM.SPPO$ID_PERIODO = " + cbPeriodo.Value + " AND POA.SPOA$ANIO = " + cbanio.Value + @"AND AV.SPPAFF$AVANCE IS NOT NULL AND ES.SPPEFFS$MES IN
                            (select EXTRACT(MONTH FROM fecha) mes
                            from
                            (SELECT
                            to_date(YEARMONTH, 'dd/mm/yyyy') fecha
                            FROM

                            (SELECT TO_CHAR(
                                            add_months(to_date('" + fecha + @"', 'dd/mm/yyyy'), ROWNUM - 1),
                                            'dd/mm/yyyy'
                                          ) as yearMonth
                              FROM DUAL
                            CONNECT BY ROWNUM <= (
                                                   select round(months_between(to_date('" + cbFechaCorte.Text + @"', 'dd/mm/yyyy'),
                                                   to_date('" + fecha + @"', 'dd/mm/yyyy')), 0) from dual
                                                 )))))
                            ";

                            estado = dao.comando(sql);

                            sql = "UPDATE SCHE$SIPLAN20.SP20$FECHA_CORTE SET SPPFC$REALIZADO = 1, SPPFC$FECHA_REALIZADO = 'FECHA_CORTE = '||TO_CHAR(SYSDATE,'DD/MM/YYYY HH:MI')||' '||'" + Session["USUARIO"].ToString() + "' WHERE SPPFC$ID = " + cbFechaCorte.Value;
                            estado = dao.comando(sql);

                            if (estado == 1)
                            {

                                cargacomboEjecucion();
                                mensaje = "El proceso de corte de registros se realizo correctamente";
                                Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',1);", true);
                            }

                            else
                            {
                                mensaje = dao.mensaje;
                                Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
                            }
                        }

                    }
                }


               

            //}

            //else
            //{
            //    mensaje = "El proceso de corte de la ejecución se puede realizar unicamente durante el dia de la fecha determinada "+cbFechaCorte.Text;
            //    Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);

            //}
            


        }

        protected void btnReportes_Click(object sender, EventArgs e)
        {

        }

        protected void btnSinfiltro_Click(object sender, EventArgs e)
        {
            parametro = "";
            enlace = "https://rpts.segeplan.gob.gt:8080/jasperserver/flow.html?_flowId=viewReportFlow&_flowId=viewReportFlow&anio="+cbanio.Value+"&institucion="+parametro+ "&periodo="+cbPeriodo.Value+"&j_username=reportes&j_password=reporte&ParentFolderUri=%2Freports%2Finteractive&reportUnit=%2Freports%2Finteractive%2FEjecucionSIPLAN&standAlone=true&output=xlsx&userLocale=es_GT";            
            //ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "script", "window.open('" + enlace + "');", true);

            this.Page.ClientScript.RegisterStartupScript(base.GetType(), "script", string.Concat("window.open('", enlace, "');"), true);
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            if (cbInstituiciones.SelectedIndex == -1)
                parametro = "";
            else
                parametro = "AND POM.SPPO$ID_INSTITUCION =" + cbInstituiciones.Value;
            enlace = "https://rpts.segeplan.gob.gt:8080/jasperserver/flow.html?_flowId=viewReportFlow&_flowId=viewReportFlow&anio=" + cbanio.Value + "&institucion=" + parametro + "&periodo=" + cbPeriodo.Value + "&j_username=reportes&j_password=reporte&ParentFolderUri=%2Freports%2Finteractive&reportUnit=%2Freports%2Finteractive%2FEjecucionSIPLAN&standAlone=true&output=xlsx&userLocale=es_GT";
            //ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "script", "window.open('" + enlace + "');", true);
            this.Page.ClientScript.RegisterStartupScript(base.GetType(), "script", string.Concat("window.open('", enlace, "');"), true);
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



        private DataTable loadProductos(int pPeriodo, int pInst, int pAnio, int pom)
        {


            DataTable dt = new DataTable();
            string query = @"SELECT ID_TIPO
                                ,TIPO
                                ,ID_RESULTADO
                                ,CASE WHEN ID_TIPO = 0 THEN METAPGG ELSE RESULTADO END AS RESULTADO
                                ,ID_RPRESUP
                                ,RPRESUP
                                ,ID_PRODUCTO
                                ,PRODUCTO
                                ,POA
                                ,AFIS
                                ,MFISINICIAL
                                ,MFIS
                                ,PORFIS
                                ,AFIN
                                ,MFININCIAL
                                ,MFIN
                                ,CASE WHEN  MFIN = 0 THEN 0||'%' ELSE TO_CHAR(NVL(ROUND(AFIN/MFIN*100,2),0) || '%') END AS PORFIN 
                                FROM 
                                (
                                    SELECT 
                                    R.SPRES$TIPO ID_TIPO
                                    ,CASE WHEN R.SPRES$TIPO=0 THEN 'METAS PGG' ELSE 'RESULTADO INSTITUCIONAL' END AS TIPO
                                    ,R.SPRES$ID_RESULTADO ID_RESULTADO
                                    ,R.SPRES$DESCRIPCION RESULTADO
                                    ,(SELECT SPPRES$CODIGO||'-'||SPPRES$DESCRIPCION FROM SCHE$SIPLAN20.SP20$RESULTADOS_ESTRATEGICOS WHERE SPPRES$ID_RESULTADO = R.SPRES$COD_ESTRATEGICO AND SPPRES$RESTRICTIVA = 'N' AND SPPRES$NIVEL = 2) METAPGG
                                    ,PROD.SPPRO$PRESUPUESTO ID_RPRESUP
                                    ,PP.SPPRO$DESCRIPCION RPRESUP
                                    ,PROD.SPPRO$ID_PRODUCTO ID_PRODUCTO
                                    ,PROD.SPPRO$DESCRIPCION PRODUCTO
                                    ,POA.SPOA$ID_POA POA
                                    ,(SELECT SUM(APROD.SPPAVP$AVANCE_FISICO) FROM SCHE$SIPLAN20.SP20$DETALLEAVANCEFI_PROD APROD LEFT JOIN SCHE$SIPLAN20.SP20$ENCABEZA_FISICO_PROD EPROD ON APROD.SPPAVP$ID_ENCABEZADO = EPROD.SPPAVF$ID_ENCABEZADO AND EPROD.SPPAVF$MES IN (1,2,3,4,5,6,7,8,9,10,11,12) AND APROD.SPPAVP$RESTRICTIVA = 'N' WHERE PROD.SPPRO$ID_PRODUCTO = APROD.SPPAVP$ID_PRODUCTO AND EPROD.SPPAVF$POA=POA.SPOA$ID_POA AND EPROD.SPPAVF$ANIO=POA.SPOA$ANIO AND APROD.SPPAVP$TIPO = 0  AND APROD.SPPAVP$RESTRICTIVA = 'N') AFIS
                                    ,SCHE$SIPLAN20.FNC$REPROGRAMACIONPROD(PROD.SPPRO$ID_PRODUCTO,POA.SPOA$ID_POA,-1,1) MFISINICIAL                            
                                    ,SCHE$SIPLAN20.FNC$OBTIENEMETAFISICAPRO(POA.SPOA$ID_POA,POA.SPOA$ANIO,PROD.SPPRO$ID_PRODUCTO) MFIS
                                    ,CASE WHEN SCHE$SIPLAN20.FNC$OBTIENEMETAFISICAPRO(POA.SPOA$ID_POA,POA.SPOA$ANIO,PROD.SPPRO$ID_PRODUCTO) = 0 THEN 0||'%' ELSE TO_CHAR(NVL(ROUND((SELECT SUM(APROD.SPPAVP$AVANCE_FISICO) FROM SCHE$SIPLAN20.SP20$DETALLEAVANCEFI_PROD APROD LEFT JOIN SCHE$SIPLAN20.SP20$ENCABEZA_FISICO_PROD EPROD ON APROD.SPPAVP$ID_ENCABEZADO = EPROD.SPPAVF$ID_ENCABEZADO AND EPROD.SPPAVF$MES IN (1,2,3,4,5,6,7,8,9,10,11,12) AND APROD.SPPAVP$RESTRICTIVA = 'N' WHERE PROD.SPPRO$ID_PRODUCTO = APROD.SPPAVP$ID_PRODUCTO AND EPROD.SPPAVF$POA=POA.SPOA$ID_POA AND EPROD.SPPAVF$ANIO=POA.SPOA$ANIO AND APROD.SPPAVP$TIPO = 0) /SCHE$SIPLAN20.FNC$OBTIENEMETAFISICAPRO(POA.SPOA$ID_POA,POA.SPOA$ANIO,PROD.SPPRO$ID_PRODUCTO)*100,2),0) || '%') END AS PORFIS
                                    ,(SELECT SUM(APROD.SPPAVP$AVANCE_FISICO) FROM SCHE$SIPLAN20.SP20$DETALLEAVANCEFI_PROD APROD LEFT JOIN SCHE$SIPLAN20.SP20$ENCABEZA_FISICO_PROD EPROD ON APROD.SPPAVP$ID_ENCABEZADO = EPROD.SPPAVF$ID_ENCABEZADO AND EPROD.SPPAVF$MES IN (1,2,3,4,5,6,7,8,9,10,11,12) WHERE PROD.SPPRO$ID_PRODUCTO = APROD.SPPAVP$ID_PRODUCTO AND EPROD.SPPAVF$POA=POA.SPOA$ID_POA AND EPROD.SPPAVF$ANIO=POA.SPOA$ANIO AND APROD.SPPAVP$TIPO = 1) AFIN
                                    ,SCHE$SIPLAN20.FNC$OBTIENEMETAFINACIERAPROD(POA.SPOA$ID_POA,POA.SPOA$ANIO,PROD.SPPRO$ID_PRODUCTO,1) MFININCIAL
                                    ,SCHE$SIPLAN20.FNC$OBTIENEMETAFINACIERAPROD(POA.SPOA$ID_POA,POA.SPOA$ANIO,PROD.SPPRO$ID_PRODUCTO,0) MFIN                                
                                    FROM SCHE$SIPLAN20.SP20$PRODUCTO PROD
                                    INNER JOIN SCHE$SIPLAN20.SP20$PROGRAMA_PRESUPUESTARIO PP ON PP.SPPRO$ID_PROGRAMA_PRESUPUESTO=PROD.SPPRO$PRESUPUESTO AND PP.SPPRO$RESTRICTIVA='N' AND PP.SPPRO$ID_POM =" + pom + @" AND PP.SPPRO$ID_INSTITUCION = " + pInst + @"
                                    INNER JOIN SCHE$SIPLAN20.SP20$POM POM ON PROD.SPPRO$POM=POM.SPPO$ID_POM AND PROD.SPPRO$INSTO=POM.SPPO$ID_INSTITUCION AND POM.SPPO$ID_PERIODO=" + cbPeriodo.Value + @" AND POM.SPPO$RESTRICTIVA='N'
                                    INNER JOIN SCHE$SIPLAN20.SP20$POA POA ON POM.SPPO$ID_POM=POA.SPOA$ID_POM AND POM.SPPO$ID_INSTITUCION=POA.SPOA$ID_INSTITUCION AND POA.SPOA$ANIO=" + cbanio.Value + @" AND POA.SPOA$RESTRICTIVA='N' AND POA.SPOA$ID_POM =" + pom + @"  AND POA.SPOA$ID_INSTITUCION = " + pInst + @"
                                    INNER JOIN SCHE$SIPLAN20.SP20$RESULTADOS R ON PROD.SPPRO$ID_RESULTADO=R.SPRES$ID_RESULTADO AND R.SPRES$RESTRICTIVA='N' AND R.SPRES$POM = " + pom + @" AND R.SPRES$INSTITUCION = " + pInst + @"
                            
                                    WHERE PROD.SPPRO$RESTRICTIVA='N' AND PROD.SPPRO$INSTO=" + pInst + " AND PROD.SPPRO$POM =" + pom + @") ORDER BY ID_RPRESUP ASC";

            estado = dao.consulta(query);
            if (estado != 0)
                dt = dao.tabla;

            return dt;
        }

        private DataTable loadSubProductos(int pPeri, int pInst, int pAnio, string pProd)
        {
            DataTable dt = new DataTable();
            string query = "";

            query = @"SELECT ID_PROD
                            ,ID_SUBP
                             ,CASE WHEN SUBP IS NULL THEN NOMBRE_SNIP ELSE SUBP END AS SUBP
                             ,POA
                             ,MFISC1
                             ,MFISC2
                             ,MFISC3
                             ,MFISINICIAL
                             ,MFIS
                             ,MFINC1
                             ,MFINC2
                             ,MFINC3
                             ,MFININCIAL
                             ,MFIN
                             ,PORFIS
                             ,PORFIN
                             ,TFIS
                             ,TFIN
                             ,AFIS1,AFIN1
                             ,AFIS2,AFIN2
                             ,AFIS3,AFIN3
                             ,AFIS4,AFIN4
                             ,AFIS5,AFIN5
                             ,AFIS6,AFIN6
                             ,AFIS7,AFIN7
                             ,AFIS8,AFIN8
                             ,AFIS9,AFIN9
                             ,AFIS10,AFIN10
                             ,AFIS11,AFIN11
                             ,AFIS12,AFIN12
                             ,AFISC1,AFINC1
                             ,AFISC2,AFINC2
                             ,AFISC3,AFINC3
                             FROM (SELECT SUBP.SPPSUB$ID_PRODUCTO ID_PROD
                                    ,SUBP.SPPSUB$ID_SUBPRODUCTO ID_SUBP
                                    ,SUBP.SPPSUB$DESCRIPCION SUBP
                                    ,POA.SPOA$ID_POA POA
                                    ,SCHE$SIPLAN20.FNC$POAMETAFISCUATRIMESTRAL(POA.SPOA$ID_POA,POA.SPOA$ANIO,SUBP.SPPSUB$ID_SUBPRODUCTO,1,4) MFISC1
                                    ,SCHE$SIPLAN20.FNC$POAMETAFISCUATRIMESTRAL(POA.SPOA$ID_POA,POA.SPOA$ANIO,SUBP.SPPSUB$ID_SUBPRODUCTO,5,8) MFISC2
                                    ,SCHE$SIPLAN20.FNC$POAMETAFISCUATRIMESTRAL(POA.SPOA$ID_POA,POA.SPOA$ANIO,SUBP.SPPSUB$ID_SUBPRODUCTO,9,12) MFISC3
                                    ,SCHE$SIPLAN20.FNC$OBTIENEMETAFISICASUB(POA.SPOA$ID_POA," + pAnio + @",SUBP.SPPSUB$ID_SUBPRODUCTO,1) MFISINICIAL
                                    ,SCHE$SIPLAN20.FNC$OBTIENEMETAFISICASUB(POA.SPOA$ID_POA," + pAnio + @",SUBP.SPPSUB$ID_SUBPRODUCTO,0) MFIS
                                    ,SCHE$SIPLAN20.FNC$POAMETAFINCUATSUB(POA.SPOA$ID_POA,POA.SPOA$ANIO,SUBP.SPPSUB$ID_SUBPRODUCTO,1,4) MFINC1
                                    ,SCHE$SIPLAN20.FNC$POAMETAFINCUATSUB(POA.SPOA$ID_POA,POA.SPOA$ANIO,SUBP.SPPSUB$ID_SUBPRODUCTO,5,8) MFINC2
                                    ,SCHE$SIPLAN20.FNC$POAMETAFINCUATSUB(POA.SPOA$ID_POA,POA.SPOA$ANIO,SUBP.SPPSUB$ID_SUBPRODUCTO,9,12) MFINC3
                                    ,SCHE$SIPLAN20.FNC$METAFINANCIERASUB(POA.SPOA$ID_POA," + pAnio + @",SUBP.SPPSUB$ID_SUBPRODUCTO,1) MFININCIAL
                                    ,SCHE$SIPLAN20.FNC$METAFINANCIERASUB(POA.SPOA$ID_POA," + pAnio + @",SUBP.SPPSUB$ID_SUBPRODUCTO,0) MFIN

                    ,CASE 
                        WHEN SCHE$SIPLAN20.FNC$OBTIENEMETAFISICASUB(POA.SPOA$ID_POA," + pAnio + @",SUBP.SPPSUB$ID_SUBPRODUCTO,0) = 0 THEN '0%'
                        WHEN SCHE$SIPLAN20.FNC$OBTIENEMETAFISICASUB(POA.SPOA$ID_POA," + pAnio + @",SUBP.SPPSUB$ID_SUBPRODUCTO,0) IS NULL THEN '0%'
                        WHEN NVL(SCHE$SIPLAN20.FNC$POAMETAFISCUATRIMESTRAL(POA.SPOA$ID_POA,POA.SPOA$ANIO,SUBP.SPPSUB$ID_SUBPRODUCTO,1,12),0)=0 THEN '0%' 
                    ELSE
                        TO_CHAR(ROUND(NVL(SCHE$SIPLAN20.FNC$SUBACUMULADOANUAL(POA.SPOA$ID_POA,SUBP.SPPSUB$ID_SUBPRODUCTO,0),0)/
                        NVL(SCHE$SIPLAN20.FNC$OBTIENEMETAFISICASUB(POA.SPOA$ID_POA," + pAnio + @",SUBP.SPPSUB$ID_SUBPRODUCTO,0),0)*100,2) || '%') 
                    END AS PORFIS
      
                    ,CASE 
                        WHEN  SCHE$SIPLAN20.FNC$METAFINANCIERASUB(POA.SPOA$ID_POA," + pAnio + @",SUBP.SPPSUB$ID_SUBPRODUCTO,0)=0 THEN  '0%'
                         WHEN SCHE$SIPLAN20.FNC$METAFINANCIERASUB(POA.SPOA$ID_POA," + pAnio + @",SUBP.SPPSUB$ID_SUBPRODUCTO,0) IS NULL THEN  '0%'
                        WHEN SCHE$SIPLAN20.FNC$POAMETAFINCUATSUB(POA.SPOA$ID_POA,POA.SPOA$ANIO,SUBP.SPPSUB$ID_SUBPRODUCTO,1,12)=0 THEN '0%'
                    ELSE 
                        TO_CHAR(NVL(ROUND(SCHE$SIPLAN20.FNC$SUBACUMULADOANUAL(POA.SPOA$ID_POA,SUBP.SPPSUB$ID_SUBPRODUCTO,1)/
                        SCHE$SIPLAN20.FNC$METAFINANCIERASUB(POA.SPOA$ID_POA," + pAnio + @",SUBP.SPPSUB$ID_SUBPRODUCTO,0)*100,2),0) || '%') 
                    END AS PORFIN

                    ,SCHE$SIPLAN20.FNC$SUBACUMULADOANUAL(POA.SPOA$ID_POA,SUBP.SPPSUB$ID_SUBPRODUCTO,0) TFIS
                    ,SCHE$SIPLAN20.FNC$SUBACUMULADOANUAL(POA.SPOA$ID_POA,SUBP.SPPSUB$ID_SUBPRODUCTO,1) TFIN
                    ,(SELECT ASP.SPPAFF$AVANCE FROM SCHE$SIPLAN20.SP20$AVANCEFIFISUB ASP LEFT JOIN SCHE$SIPLAN20.SP20$ENCABEZ_MFIFISSUB ESP ON ASP.SPPAFF$ENCABEZADOSUB = ESP.SPPEFFS$ID_ENCABEZADO AND ESP.SPPEFFS$MES=1 WHERE SUBP.SPPSUB$ID_SUBPRODUCTO = ASP.SPPAFF$SUBPRODUCTO AND ESP.SPPEFFS$POA=POA.SPOA$ID_POA AND ESP.SPPEFFS$ANIO=POA.SPOA$ANIO AND ASP.SPPAFF$TIPO_AVANCE=0 AND ASP.SPPAFF$RESTRICTIVA = 'N' AND ESP.SPPEFFS$RESTRICTIVA = 'N') AFIS1
                    ,(SELECT ASP.SPPAFF$AVANCE FROM SCHE$SIPLAN20.SP20$AVANCEFIFISUB ASP LEFT JOIN SCHE$SIPLAN20.SP20$ENCABEZ_MFIFISSUB ESP ON ASP.SPPAFF$ENCABEZADOSUB = ESP.SPPEFFS$ID_ENCABEZADO AND ESP.SPPEFFS$MES=1 WHERE SUBP.SPPSUB$ID_SUBPRODUCTO = ASP.SPPAFF$SUBPRODUCTO AND ESP.SPPEFFS$POA=POA.SPOA$ID_POA AND ESP.SPPEFFS$ANIO=POA.SPOA$ANIO AND ASP.SPPAFF$TIPO_AVANCE=1 AND ASP.SPPAFF$RESTRICTIVA = 'N' AND ESP.SPPEFFS$RESTRICTIVA = 'N') AFIN1                    
                    ,(SELECT ASP.SPPAFF$AVANCE FROM SCHE$SIPLAN20.SP20$AVANCEFIFISUB ASP LEFT JOIN SCHE$SIPLAN20.SP20$ENCABEZ_MFIFISSUB ESP ON ASP.SPPAFF$ENCABEZADOSUB = ESP.SPPEFFS$ID_ENCABEZADO AND ESP.SPPEFFS$MES=2 WHERE SUBP.SPPSUB$ID_SUBPRODUCTO = ASP.SPPAFF$SUBPRODUCTO AND ESP.SPPEFFS$POA=POA.SPOA$ID_POA AND ESP.SPPEFFS$ANIO=POA.SPOA$ANIO AND ASP.SPPAFF$TIPO_AVANCE=0 AND ASP.SPPAFF$RESTRICTIVA = 'N' AND ESP.SPPEFFS$RESTRICTIVA = 'N') AFIS2
                    ,(SELECT ASP.SPPAFF$AVANCE FROM SCHE$SIPLAN20.SP20$AVANCEFIFISUB ASP LEFT JOIN SCHE$SIPLAN20.SP20$ENCABEZ_MFIFISSUB ESP ON ASP.SPPAFF$ENCABEZADOSUB = ESP.SPPEFFS$ID_ENCABEZADO AND ESP.SPPEFFS$MES=2 WHERE SUBP.SPPSUB$ID_SUBPRODUCTO = ASP.SPPAFF$SUBPRODUCTO AND ESP.SPPEFFS$POA=POA.SPOA$ID_POA AND ESP.SPPEFFS$ANIO=POA.SPOA$ANIO AND ASP.SPPAFF$TIPO_AVANCE=1 AND ASP.SPPAFF$RESTRICTIVA = 'N' AND ESP.SPPEFFS$RESTRICTIVA = 'N') AFIN2

                    ,(SELECT ASP.SPPAFF$AVANCE FROM SCHE$SIPLAN20.SP20$AVANCEFIFISUB ASP LEFT JOIN SCHE$SIPLAN20.SP20$ENCABEZ_MFIFISSUB ESP ON ASP.SPPAFF$ENCABEZADOSUB = ESP.SPPEFFS$ID_ENCABEZADO AND ESP.SPPEFFS$MES=3 WHERE SUBP.SPPSUB$ID_SUBPRODUCTO = ASP.SPPAFF$SUBPRODUCTO AND ESP.SPPEFFS$POA=POA.SPOA$ID_POA AND ESP.SPPEFFS$ANIO=POA.SPOA$ANIO AND ASP.SPPAFF$TIPO_AVANCE=0 AND ASP.SPPAFF$RESTRICTIVA = 'N' AND ESP.SPPEFFS$RESTRICTIVA = 'N') AFIS3
                    ,(SELECT ASP.SPPAFF$AVANCE FROM SCHE$SIPLAN20.SP20$AVANCEFIFISUB ASP LEFT JOIN SCHE$SIPLAN20.SP20$ENCABEZ_MFIFISSUB ESP ON ASP.SPPAFF$ENCABEZADOSUB = ESP.SPPEFFS$ID_ENCABEZADO AND ESP.SPPEFFS$MES=3 WHERE SUBP.SPPSUB$ID_SUBPRODUCTO = ASP.SPPAFF$SUBPRODUCTO AND ESP.SPPEFFS$POA=POA.SPOA$ID_POA AND ESP.SPPEFFS$ANIO=POA.SPOA$ANIO AND ASP.SPPAFF$TIPO_AVANCE=1 AND ASP.SPPAFF$RESTRICTIVA = 'N' AND ESP.SPPEFFS$RESTRICTIVA = 'N') AFIN3
                   
                    ,(SELECT ASP.SPPAFF$AVANCE FROM SCHE$SIPLAN20.SP20$AVANCEFIFISUB ASP LEFT JOIN SCHE$SIPLAN20.SP20$ENCABEZ_MFIFISSUB ESP ON ASP.SPPAFF$ENCABEZADOSUB = ESP.SPPEFFS$ID_ENCABEZADO AND ESP.SPPEFFS$MES=4 WHERE SUBP.SPPSUB$ID_SUBPRODUCTO = ASP.SPPAFF$SUBPRODUCTO AND ESP.SPPEFFS$POA=POA.SPOA$ID_POA AND ESP.SPPEFFS$ANIO=POA.SPOA$ANIO AND ASP.SPPAFF$TIPO_AVANCE=0 AND ASP.SPPAFF$RESTRICTIVA = 'N' AND ESP.SPPEFFS$RESTRICTIVA = 'N') AFIS4
                    ,(SELECT ASP.SPPAFF$AVANCE FROM SCHE$SIPLAN20.SP20$AVANCEFIFISUB ASP LEFT JOIN SCHE$SIPLAN20.SP20$ENCABEZ_MFIFISSUB ESP ON ASP.SPPAFF$ENCABEZADOSUB = ESP.SPPEFFS$ID_ENCABEZADO AND ESP.SPPEFFS$MES=4 WHERE SUBP.SPPSUB$ID_SUBPRODUCTO = ASP.SPPAFF$SUBPRODUCTO AND ESP.SPPEFFS$POA=POA.SPOA$ID_POA AND ESP.SPPEFFS$ANIO=POA.SPOA$ANIO AND ASP.SPPAFF$TIPO_AVANCE=1 AND ASP.SPPAFF$RESTRICTIVA = 'N' AND ESP.SPPEFFS$RESTRICTIVA = 'N') AFIN4
       
                    ,(SELECT SUM(ASP.SPPAFF$AVANCE) FROM SCHE$SIPLAN20.SP20$AVANCEFIFISUB ASP LEFT JOIN SCHE$SIPLAN20.SP20$ENCABEZ_MFIFISSUB ESP ON ASP.SPPAFF$ENCABEZADOSUB = ESP.SPPEFFS$ID_ENCABEZADO AND ESP.SPPEFFS$MES IN (1,2,3,4) WHERE SUBP.SPPSUB$ID_SUBPRODUCTO = ASP.SPPAFF$SUBPRODUCTO AND ESP.SPPEFFS$POA=POA.SPOA$ID_POA AND ESP.SPPEFFS$ANIO=POA.SPOA$ANIO AND ASP.SPPAFF$TIPO_AVANCE=0 AND ASP.SPPAFF$RESTRICTIVA = 'N' AND ESP.SPPEFFS$RESTRICTIVA = 'N') AFISC1
                    ,(SELECT SUM(ASP.SPPAFF$AVANCE) FROM SCHE$SIPLAN20.SP20$AVANCEFIFISUB ASP LEFT JOIN SCHE$SIPLAN20.SP20$ENCABEZ_MFIFISSUB ESP ON ASP.SPPAFF$ENCABEZADOSUB = ESP.SPPEFFS$ID_ENCABEZADO AND ESP.SPPEFFS$MES IN (1,2,3,4) WHERE SUBP.SPPSUB$ID_SUBPRODUCTO = ASP.SPPAFF$SUBPRODUCTO AND ESP.SPPEFFS$POA=POA.SPOA$ID_POA AND ESP.SPPEFFS$ANIO=POA.SPOA$ANIO AND ASP.SPPAFF$TIPO_AVANCE=1 AND ASP.SPPAFF$RESTRICTIVA = 'N' AND ESP.SPPEFFS$RESTRICTIVA = 'N') AFINC1
                    ,(SELECT ASP.SPPAFF$AVANCE FROM SCHE$SIPLAN20.SP20$AVANCEFIFISUB ASP LEFT JOIN SCHE$SIPLAN20.SP20$ENCABEZ_MFIFISSUB ESP ON ASP.SPPAFF$ENCABEZADOSUB = ESP.SPPEFFS$ID_ENCABEZADO AND ESP.SPPEFFS$MES=5 WHERE SUBP.SPPSUB$ID_SUBPRODUCTO = ASP.SPPAFF$SUBPRODUCTO AND ESP.SPPEFFS$POA=POA.SPOA$ID_POA AND ESP.SPPEFFS$ANIO=POA.SPOA$ANIO AND ASP.SPPAFF$TIPO_AVANCE=0 AND ASP.SPPAFF$RESTRICTIVA = 'N' AND ESP.SPPEFFS$RESTRICTIVA = 'N') AFIS5
                    ,(SELECT ASP.SPPAFF$AVANCE FROM SCHE$SIPLAN20.SP20$AVANCEFIFISUB ASP LEFT JOIN SCHE$SIPLAN20.SP20$ENCABEZ_MFIFISSUB ESP ON ASP.SPPAFF$ENCABEZADOSUB = ESP.SPPEFFS$ID_ENCABEZADO AND ESP.SPPEFFS$MES=5 WHERE SUBP.SPPSUB$ID_SUBPRODUCTO = ASP.SPPAFF$SUBPRODUCTO AND ESP.SPPEFFS$POA=POA.SPOA$ID_POA AND ESP.SPPEFFS$ANIO=POA.SPOA$ANIO AND ASP.SPPAFF$TIPO_AVANCE=1 AND ASP.SPPAFF$RESTRICTIVA = 'N' AND ESP.SPPEFFS$RESTRICTIVA = 'N') AFIN5

                    ,(SELECT ASP.SPPAFF$AVANCE FROM SCHE$SIPLAN20.SP20$AVANCEFIFISUB ASP LEFT JOIN SCHE$SIPLAN20.SP20$ENCABEZ_MFIFISSUB ESP ON ASP.SPPAFF$ENCABEZADOSUB = ESP.SPPEFFS$ID_ENCABEZADO AND ESP.SPPEFFS$MES=6 WHERE SUBP.SPPSUB$ID_SUBPRODUCTO = ASP.SPPAFF$SUBPRODUCTO AND ESP.SPPEFFS$POA=POA.SPOA$ID_POA AND ESP.SPPEFFS$ANIO=POA.SPOA$ANIO AND ASP.SPPAFF$TIPO_AVANCE=0 AND ASP.SPPAFF$RESTRICTIVA = 'N' AND ESP.SPPEFFS$RESTRICTIVA = 'N') AFIS6
                    ,(SELECT ASP.SPPAFF$AVANCE FROM SCHE$SIPLAN20.SP20$AVANCEFIFISUB ASP LEFT JOIN SCHE$SIPLAN20.SP20$ENCABEZ_MFIFISSUB ESP ON ASP.SPPAFF$ENCABEZADOSUB = ESP.SPPEFFS$ID_ENCABEZADO AND ESP.SPPEFFS$MES=6 WHERE SUBP.SPPSUB$ID_SUBPRODUCTO = ASP.SPPAFF$SUBPRODUCTO AND ESP.SPPEFFS$POA=POA.SPOA$ID_POA AND ESP.SPPEFFS$ANIO=POA.SPOA$ANIO AND ASP.SPPAFF$TIPO_AVANCE=1 AND ASP.SPPAFF$RESTRICTIVA = 'N' AND ESP.SPPEFFS$RESTRICTIVA = 'N') AFIN6

                    ,(SELECT ASP.SPPAFF$AVANCE FROM SCHE$SIPLAN20.SP20$AVANCEFIFISUB ASP LEFT JOIN SCHE$SIPLAN20.SP20$ENCABEZ_MFIFISSUB ESP ON ASP.SPPAFF$ENCABEZADOSUB = ESP.SPPEFFS$ID_ENCABEZADO AND ESP.SPPEFFS$MES=7 WHERE SUBP.SPPSUB$ID_SUBPRODUCTO = ASP.SPPAFF$SUBPRODUCTO AND ESP.SPPEFFS$POA=POA.SPOA$ID_POA AND ESP.SPPEFFS$ANIO=POA.SPOA$ANIO AND ASP.SPPAFF$TIPO_AVANCE=0 AND ASP.SPPAFF$RESTRICTIVA = 'N' AND ESP.SPPEFFS$RESTRICTIVA = 'N') AFIS7
                    ,(SELECT ASP.SPPAFF$AVANCE FROM SCHE$SIPLAN20.SP20$AVANCEFIFISUB ASP LEFT JOIN SCHE$SIPLAN20.SP20$ENCABEZ_MFIFISSUB ESP ON ASP.SPPAFF$ENCABEZADOSUB = ESP.SPPEFFS$ID_ENCABEZADO AND ESP.SPPEFFS$MES=7 WHERE SUBP.SPPSUB$ID_SUBPRODUCTO = ASP.SPPAFF$SUBPRODUCTO AND ESP.SPPEFFS$POA=POA.SPOA$ID_POA AND ESP.SPPEFFS$ANIO=POA.SPOA$ANIO AND ASP.SPPAFF$TIPO_AVANCE=1 AND ASP.SPPAFF$RESTRICTIVA = 'N' AND ESP.SPPEFFS$RESTRICTIVA = 'N') AFIN7

                    ,(SELECT ASP.SPPAFF$AVANCE FROM SCHE$SIPLAN20.SP20$AVANCEFIFISUB ASP LEFT JOIN SCHE$SIPLAN20.SP20$ENCABEZ_MFIFISSUB ESP ON ASP.SPPAFF$ENCABEZADOSUB = ESP.SPPEFFS$ID_ENCABEZADO AND ESP.SPPEFFS$MES=8 WHERE SUBP.SPPSUB$ID_SUBPRODUCTO = ASP.SPPAFF$SUBPRODUCTO AND ESP.SPPEFFS$POA=POA.SPOA$ID_POA AND ESP.SPPEFFS$ANIO=POA.SPOA$ANIO AND ASP.SPPAFF$TIPO_AVANCE=0 AND ASP.SPPAFF$RESTRICTIVA = 'N' AND ESP.SPPEFFS$RESTRICTIVA = 'N') AFIS8
                    ,(SELECT ASP.SPPAFF$AVANCE FROM SCHE$SIPLAN20.SP20$AVANCEFIFISUB ASP LEFT JOIN SCHE$SIPLAN20.SP20$ENCABEZ_MFIFISSUB ESP ON ASP.SPPAFF$ENCABEZADOSUB = ESP.SPPEFFS$ID_ENCABEZADO AND ESP.SPPEFFS$MES=8 WHERE SUBP.SPPSUB$ID_SUBPRODUCTO = ASP.SPPAFF$SUBPRODUCTO AND ESP.SPPEFFS$POA=POA.SPOA$ID_POA AND ESP.SPPEFFS$ANIO=POA.SPOA$ANIO AND ASP.SPPAFF$TIPO_AVANCE=1 AND ASP.SPPAFF$RESTRICTIVA = 'N' AND ESP.SPPEFFS$RESTRICTIVA = 'N') AFIN8
                    
                    ,(SELECT SUM(ASP.SPPAFF$AVANCE) FROM SCHE$SIPLAN20.SP20$AVANCEFIFISUB ASP LEFT JOIN SCHE$SIPLAN20.SP20$ENCABEZ_MFIFISSUB ESP ON ASP.SPPAFF$ENCABEZADOSUB = ESP.SPPEFFS$ID_ENCABEZADO AND ESP.SPPEFFS$MES IN (5,6,7,8) WHERE SUBP.SPPSUB$ID_SUBPRODUCTO = ASP.SPPAFF$SUBPRODUCTO AND ESP.SPPEFFS$POA=POA.SPOA$ID_POA AND ESP.SPPEFFS$ANIO=POA.SPOA$ANIO AND ASP.SPPAFF$TIPO_AVANCE=0 AND ASP.SPPAFF$RESTRICTIVA = 'N' AND ESP.SPPEFFS$RESTRICTIVA = 'N') AFISC2
                    ,(SELECT SUM(ASP.SPPAFF$AVANCE) FROM SCHE$SIPLAN20.SP20$AVANCEFIFISUB ASP LEFT JOIN SCHE$SIPLAN20.SP20$ENCABEZ_MFIFISSUB ESP ON ASP.SPPAFF$ENCABEZADOSUB = ESP.SPPEFFS$ID_ENCABEZADO AND ESP.SPPEFFS$MES IN (5,6,7,8) WHERE SUBP.SPPSUB$ID_SUBPRODUCTO = ASP.SPPAFF$SUBPRODUCTO AND ESP.SPPEFFS$POA=POA.SPOA$ID_POA AND ESP.SPPEFFS$ANIO=POA.SPOA$ANIO AND ASP.SPPAFF$TIPO_AVANCE=1 AND ASP.SPPAFF$RESTRICTIVA = 'N' AND ESP.SPPEFFS$RESTRICTIVA = 'N') AFINC2
                    ,(SELECT ASP.SPPAFF$AVANCE FROM SCHE$SIPLAN20.SP20$AVANCEFIFISUB ASP LEFT JOIN SCHE$SIPLAN20.SP20$ENCABEZ_MFIFISSUB ESP ON ASP.SPPAFF$ENCABEZADOSUB = ESP.SPPEFFS$ID_ENCABEZADO AND ESP.SPPEFFS$MES=9 WHERE SUBP.SPPSUB$ID_SUBPRODUCTO = ASP.SPPAFF$SUBPRODUCTO AND ESP.SPPEFFS$POA=POA.SPOA$ID_POA AND ESP.SPPEFFS$ANIO=POA.SPOA$ANIO AND ASP.SPPAFF$TIPO_AVANCE=0 AND ASP.SPPAFF$RESTRICTIVA = 'N' AND ESP.SPPEFFS$RESTRICTIVA = 'N') AFIS9
                    ,(SELECT ASP.SPPAFF$AVANCE FROM SCHE$SIPLAN20.SP20$AVANCEFIFISUB ASP LEFT JOIN SCHE$SIPLAN20.SP20$ENCABEZ_MFIFISSUB ESP ON ASP.SPPAFF$ENCABEZADOSUB = ESP.SPPEFFS$ID_ENCABEZADO AND ESP.SPPEFFS$MES=9 WHERE SUBP.SPPSUB$ID_SUBPRODUCTO = ASP.SPPAFF$SUBPRODUCTO AND ESP.SPPEFFS$POA=POA.SPOA$ID_POA AND ESP.SPPEFFS$ANIO=POA.SPOA$ANIO AND ASP.SPPAFF$TIPO_AVANCE=1 AND ASP.SPPAFF$RESTRICTIVA = 'N' AND ESP.SPPEFFS$RESTRICTIVA = 'N') AFIN9

                    ,(SELECT ASP.SPPAFF$AVANCE FROM SCHE$SIPLAN20.SP20$AVANCEFIFISUB ASP LEFT JOIN SCHE$SIPLAN20.SP20$ENCABEZ_MFIFISSUB ESP ON ASP.SPPAFF$ENCABEZADOSUB = ESP.SPPEFFS$ID_ENCABEZADO AND ESP.SPPEFFS$MES=10 WHERE SUBP.SPPSUB$ID_SUBPRODUCTO = ASP.SPPAFF$SUBPRODUCTO AND ESP.SPPEFFS$POA=POA.SPOA$ID_POA AND ESP.SPPEFFS$ANIO=POA.SPOA$ANIO AND ASP.SPPAFF$TIPO_AVANCE=0 AND ASP.SPPAFF$RESTRICTIVA = 'N' AND ESP.SPPEFFS$RESTRICTIVA = 'N') AFIS10
                    ,(SELECT ASP.SPPAFF$AVANCE FROM SCHE$SIPLAN20.SP20$AVANCEFIFISUB ASP LEFT JOIN SCHE$SIPLAN20.SP20$ENCABEZ_MFIFISSUB ESP ON ASP.SPPAFF$ENCABEZADOSUB = ESP.SPPEFFS$ID_ENCABEZADO AND ESP.SPPEFFS$MES=10 WHERE SUBP.SPPSUB$ID_SUBPRODUCTO = ASP.SPPAFF$SUBPRODUCTO AND ESP.SPPEFFS$POA=POA.SPOA$ID_POA AND ESP.SPPEFFS$ANIO=POA.SPOA$ANIO AND ASP.SPPAFF$TIPO_AVANCE=1 AND ASP.SPPAFF$RESTRICTIVA = 'N' AND ESP.SPPEFFS$RESTRICTIVA = 'N') AFIN10

                    ,(SELECT ASP.SPPAFF$AVANCE FROM SCHE$SIPLAN20.SP20$AVANCEFIFISUB ASP LEFT JOIN SCHE$SIPLAN20.SP20$ENCABEZ_MFIFISSUB ESP ON ASP.SPPAFF$ENCABEZADOSUB = ESP.SPPEFFS$ID_ENCABEZADO AND ESP.SPPEFFS$MES=11 WHERE SUBP.SPPSUB$ID_SUBPRODUCTO = ASP.SPPAFF$SUBPRODUCTO AND ESP.SPPEFFS$POA=POA.SPOA$ID_POA AND ESP.SPPEFFS$ANIO=POA.SPOA$ANIO AND ASP.SPPAFF$TIPO_AVANCE=0 AND ASP.SPPAFF$RESTRICTIVA = 'N' AND ESP.SPPEFFS$RESTRICTIVA = 'N') AFIS11
                    ,(SELECT ASP.SPPAFF$AVANCE FROM SCHE$SIPLAN20.SP20$AVANCEFIFISUB ASP LEFT JOIN SCHE$SIPLAN20.SP20$ENCABEZ_MFIFISSUB ESP ON ASP.SPPAFF$ENCABEZADOSUB = ESP.SPPEFFS$ID_ENCABEZADO AND ESP.SPPEFFS$MES=11 WHERE SUBP.SPPSUB$ID_SUBPRODUCTO = ASP.SPPAFF$SUBPRODUCTO AND ESP.SPPEFFS$POA=POA.SPOA$ID_POA AND ESP.SPPEFFS$ANIO=POA.SPOA$ANIO AND ASP.SPPAFF$TIPO_AVANCE=1 AND ASP.SPPAFF$RESTRICTIVA = 'N' AND ESP.SPPEFFS$RESTRICTIVA = 'N') AFIN11

                    ,(SELECT ASP.SPPAFF$AVANCE FROM SCHE$SIPLAN20.SP20$AVANCEFIFISUB ASP LEFT JOIN SCHE$SIPLAN20.SP20$ENCABEZ_MFIFISSUB ESP ON ASP.SPPAFF$ENCABEZADOSUB = ESP.SPPEFFS$ID_ENCABEZADO AND ESP.SPPEFFS$MES=12 WHERE SUBP.SPPSUB$ID_SUBPRODUCTO = ASP.SPPAFF$SUBPRODUCTO AND ESP.SPPEFFS$POA=POA.SPOA$ID_POA AND ESP.SPPEFFS$ANIO=POA.SPOA$ANIO AND ASP.SPPAFF$TIPO_AVANCE=0 AND ASP.SPPAFF$RESTRICTIVA = 'N' AND ESP.SPPEFFS$RESTRICTIVA = 'N') AFIS12
                    ,(SELECT ASP.SPPAFF$AVANCE FROM SCHE$SIPLAN20.SP20$AVANCEFIFISUB ASP LEFT JOIN SCHE$SIPLAN20.SP20$ENCABEZ_MFIFISSUB ESP ON ASP.SPPAFF$ENCABEZADOSUB = ESP.SPPEFFS$ID_ENCABEZADO AND ESP.SPPEFFS$MES=12 WHERE SUBP.SPPSUB$ID_SUBPRODUCTO = ASP.SPPAFF$SUBPRODUCTO AND ESP.SPPEFFS$POA=POA.SPOA$ID_POA AND ESP.SPPEFFS$ANIO=POA.SPOA$ANIO AND ASP.SPPAFF$TIPO_AVANCE=1 AND ASP.SPPAFF$RESTRICTIVA = 'N' AND ESP.SPPEFFS$RESTRICTIVA = 'N') AFIN12

                    ,(SELECT SUM(ASP.SPPAFF$AVANCE) FROM SCHE$SIPLAN20.SP20$AVANCEFIFISUB ASP LEFT JOIN SCHE$SIPLAN20.SP20$ENCABEZ_MFIFISSUB ESP ON ASP.SPPAFF$ENCABEZADOSUB = ESP.SPPEFFS$ID_ENCABEZADO AND ESP.SPPEFFS$MES IN (9,10,11,12) WHERE SUBP.SPPSUB$ID_SUBPRODUCTO = ASP.SPPAFF$SUBPRODUCTO AND ESP.SPPEFFS$POA=POA.SPOA$ID_POA AND ESP.SPPEFFS$ANIO=POA.SPOA$ANIO AND ASP.SPPAFF$TIPO_AVANCE=0 AND ASP.SPPAFF$RESTRICTIVA = 'N' AND ESP.SPPEFFS$RESTRICTIVA = 'N') AFISC3
                    ,(SELECT SUM(ASP.SPPAFF$AVANCE) FROM SCHE$SIPLAN20.SP20$AVANCEFIFISUB ASP LEFT JOIN SCHE$SIPLAN20.SP20$ENCABEZ_MFIFISSUB ESP ON ASP.SPPAFF$ENCABEZADOSUB = ESP.SPPEFFS$ID_ENCABEZADO AND ESP.SPPEFFS$MES IN (9,10,11,12) WHERE SUBP.SPPSUB$ID_SUBPRODUCTO = ASP.SPPAFF$SUBPRODUCTO AND ESP.SPPEFFS$POA=POA.SPOA$ID_POA AND ESP.SPPEFFS$ANIO=POA.SPOA$ANIO AND ASP.SPPAFF$TIPO_AVANCE=1 AND ASP.SPPAFF$RESTRICTIVA = 'N' AND ESP.SPPEFFS$RESTRICTIVA = 'N') AFINC3
                    ,(SELECT 'SNIP: '||a.proyecto||'-'||a.nombre PROYECTO  FROM SINIP.BP_PROYECTO_ID a ,SINIP.ci_unidad_ejecutora b WHERE a.proyecto = SUBP.SPPSUB$SNIP  AND a.restrictiva      = 'N'  AND a.entidad = b.entidad   AND a.unidad_ejecutora = b.unidad_ejecutora) NOMBRE_SNIP
                    FROM SCHE$SIPLAN20.SP20$SUB_PRODUCTO SUBP
                    INNER JOIN SCHE$SIPLAN20.SP20$PRODUCTO PROD ON PROD.SPPRO$ID_PRODUCTO = SUBP.SPPSUB$ID_PRODUCTO AND PROD.SPPRO$RESTRICTIVA='N'
                    INNER JOIN SCHE$SIPLAN20.SP20$POM POM ON PROD.SPPRO$POM = POM.SPPO$ID_POM AND PROD.SPPRO$INSTO=POM.SPPO$ID_INSTITUCION AND POM.SPPO$RESTRICTIVA='N' AND POM.SPPO$ID_PERIODO=" + pPeri +
                    @"LEFT JOIN SCHE$SIPLAN20.SP20$POA POA ON POM.SPPO$ID_POM=POA.SPOA$ID_POM AND POM.SPPO$ID_INSTITUCION = POA.SPOA$ID_INSTITUCION AND POA.SPOA$ANIO=" + pAnio + @" AND POA.SPOA$ID_INSTITUCION=PROD.SPPRO$INSTO
                    WHERE PROD.SPPRO$ID_PRODUCTO=" + pProd + " AND PROD.SPPRO$RESTRICTIVA='N' AND PROD.SPPRO$INSTO=" + pInst + " AND SUBP.SPPSUB$RESTRICTIVA = 'N' AND POA.SPOA$RESTRICTIVA = 'N')";

            estado = dao.consulta(query);
            if (estado != 0)
                dt = dao.tabla;
            return dt;
        }

        protected void gvSubProductos_BeforePerformDataSelect(object sender, EventArgs e)
        {
            ASPxGridView gvDetail = sender as ASPxGridView;
            int masterProdID = Convert.ToInt32(gvDetail.GetMasterRowKeyValue());
            int peri = -1;
            int inst = -1;
            int anio = -1;

            try
            {
                peri = Convert.ToInt32(cbPeriodo.Value);
                inst = Convert.ToInt32(cbInstituiciones.Value);
                anio = Convert.ToInt32(cbanio.Value);
            }
            catch
            {
                peri = -1;
                inst = -1;
                anio = -1;
            }

            if (Session["ROL"].ToString() == "USER" || Session["ROL"].ToString() == "CAPA")
                //if (Session["USUARIO"].ToString() == "SIPLANCAPA" || Session["USUARIO"].ToString() == "BGMARROQUIN" || Session["USUARIO"].ToString() == "RDELEON")
                gvDetail.Columns[0].Visible = true;
            else
                gvDetail.Columns[0].Visible = false;

            if (peri != -1 && inst != -1 && anio != -1)
            {
                gvDetail.DataSource = loadSubProductos(peri, inst, anio, masterProdID.ToString());
                Session["subsos"] = loadSubProductos(peri, inst, anio, masterProdID.ToString());
            }

            else
                gvDetail.DataSource = null;

            Session["masterProdID"] = masterProdID.ToString();
            Session["objSubP"] = sender;
        }

        protected void gvSubProductos_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            if (e.DataColumn.FieldName == "MFIS" || e.DataColumn.FieldName == "TFIS" || e.DataColumn.FieldName == "PORFIS" || e.DataColumn.FieldName == "MFIN" || e.DataColumn.FieldName == "TFIN" || e.DataColumn.FieldName == "PORFIN")
            {
                e.Cell.Font.Bold = true;
                e.Cell.ForeColor = System.Drawing.Color.Blue;
            }
            if (e.DataColumn.FieldName == "MFISC1" || e.DataColumn.FieldName == "MFINC1" || e.DataColumn.FieldName == "AFISC1" || e.DataColumn.FieldName == "AFINC1")
            {
                e.Cell.Enabled = false;
                //e.Cell.BackColor = System.Drawing.Color.LightGray;
                e.Cell.ForeColor = System.Drawing.Color.Black;
                e.Cell.Font.Bold = true;
            }



        }

        protected void gvProductosInsto_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
        {
            int valor = 0;
            valor = Convert.ToInt32(e.GetValue("ID_DET_TEMATICA"));
            if (valor == -1)
                e.Row.ForeColor = System.Drawing.Color.Red;
            else
                e.Row.ForeColor = System.Drawing.Color.Blue;
        }

        

        protected void gvEjecucion_DetailRowExpandedChanged(object sender, DevExpress.Web.ASPxGridViewDetailRowEventArgs e)
        {
            int posi;

            try
            {
                posi = Convert.ToInt32(e.VisibleIndex.ToString());
                Session["indexProducto"] = posi;
                gvEjecucion.FocusedRowIndex = posi;

            }
            catch (Exception error)
            {
                posi = -1;
                Session["indexProducto"] = posi;
            }
        }

       

        protected void gvEjecucion_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            if (e.DataColumn.FieldName == "MFIS" || e.DataColumn.FieldName == "AFIS" || e.DataColumn.FieldName == "PORFIS" || e.DataColumn.FieldName == "MFIN" || e.DataColumn.FieldName == "AFIN" || e.DataColumn.FieldName == "PORFIN")
            {
                e.Cell.Font.Bold = true;
                e.Cell.ForeColor = System.Drawing.Color.Blue;
            }
        }

        

        

        protected void cargaProductosInsto()
        {
            sql = @"SELECT 
                            SPPO$ID_POM
                            ,SPPO$ID_INSTITUCION
                            ,SPP$ID_PERIODO
                            ,PROGRAMA_PRESUPUESTARIO||' '||PROGRAMA PROGRAMA 
                            ,CASE WHEN ID_SUBPROGRAMA = -1 THEN SUB_PROGRAMA ELSE ID_SUBPROGRAMA||' '||SUB_PROGRAMA END AS SUBPROGRAMA
                            ,SPPRO$ID_PRODUCTO 
                            ,PRODUCTO
                            ,MEDIDA
                            ,METAFISICA
                            ,METAFINANCIERA
                            ,ID_TEMATICA
                            ,TEMATICA
                            ,ID_DET_TEMATICA
                            ,DETALLE_TEMATICA
                            ,SPOA$ID_POA
                            ,SPOA$ANIO
                            FROM
                            (SELECT
                            SPPO$ID_POM
                            ,SPPO$ID_INSTITUCION
                            ,SPP$ID_PERIODO
                            ,CASE WHEN SPPRO$ID_PROGRAMA_DEPENDE = -1 THEN  SPPRO$ID_PROGRAMA_PRESUPUESTO ELSE SPPRO$ID_PROGRAMA_DEPENDE END AS PROGRAMA_PRESUPUESTARIO
                            ,CASE WHEN SPPRO$ID_PROGRAMA_DEPENDE = -1 THEN SPPRO$DESCRIPCION ELSE PROGRAMA_DEPENDE END AS PROGRAMA
                            ,CASE WHEN SPPRO$ID_PROGRAMA_DEPENDE = -1 THEN SPPRO$ID_PROGRAMA_DEPENDE ELSE SPPRO$ID_PROGRAMA_PRESUPUESTO END AS ID_SUBPROGRAMA
                            ,CASE WHEN SPPRO$ID_PROGRAMA_DEPENDE = -1 THEN 'SIN SUBPROGRAMA' ELSE SPPRO$DESCRIPCION END AS SUB_PROGRAMA
                            ,SPPRO$ID_PRODUCTO 
                            ,PRODUCTO
                            ,MEDIDA
                            ,SCHE$SIPLAN20.FNC$OBTIENEMETAFISICAPRO(SPOA$ID_POA,SPOA$ANIO,SPPRO$ID_PRODUCTO) METAFISICA
                            ,SCHE$SIPLAN20.FNC$OBTIENEMETAFINACIERAPROD(SPOA$ID_POA,SPOA$ANIO,SPPRO$ID_PRODUCTO,0) METAFINANCIERA
                            ,ID_TEMATICA
                            ,(SELECT SPPTEM$DESCRIPCION FROM SCHE$SIPLAN20.SP20$TEMATICA WHERE SPPTEM$ID = ID_TEMATICA) TEMATICA
                            ,SPPRO$TEMATICA ID_DET_TEMATICA
                            ,(SELECT SPPTD$DESCRIPCION FROM SCHE$SIPLAN20.SP20$DETALLE_TEMATICA WHERE SPPTD$ID = SPPRO$TEMATICA) DETALLE_TEMATICA
                            ,SPOA$ID_POA
                            ,SPOA$ANIO
                            FROM
                            (SELECT 
                            POM.SPPO$ID_POM
                            ,POM.SPPO$ID_INSTITUCION
                            ,PE.SPP$ID_PERIODO
                            ,CASE WHEN PR.SPPRO$ID_PROGRAMA_DEPENDE IS NULL THEN -1 ELSE PR.SPPRO$ID_PROGRAMA_DEPENDE END AS SPPRO$ID_PROGRAMA_DEPENDE
                            ,(SELECT SPPRO$DESCRIPCION FROM SCHE$SIPLAN20.SP20$PROGRAMA_PRESUPUESTARIO WHERE SPPRO$ID_PROGRAMA_PRESUPUESTO = PR.SPPRO$ID_PROGRAMA_DEPENDE AND SPPRO$ID_POM = POM.SPPO$ID_POM AND SPPRO$ID_INSTITUCION = POM.SPPO$ID_INSTITUCION AND SPPRO$RESTRICTIVA = 'N') PROGRAMA_DEPENDE
                            ,PR.SPPRO$ID_PROGRAMA_PRESUPUESTO
                            ,PR.SPPRO$DESCRIPCION 
                            ,PO.SPPRO$ID_PRODUCTO 
                            ,PO.SPPRO$DESCRIPCION PRODUCTO
                            ,U.NOMBRE MEDIDA
                            ,PO.SPPRO$TEMATICA
                            ,(SELECT SPPTD$ID_TEMATICA FROM SCHE$SIPLAN20.SP20$DETALLE_TEMATICA WHERE SPPTD$ID = PO.SPPRO$TEMATICA AND SPPTD$RESTRICTIVA = 'N') ID_TEMATICA
                            ,(SELECT SPPTD$DESCRIPCION FROM SCHE$SIPLAN20.SP20$DETALLE_TEMATICA WHERE SPPTD$ID = PO.SPPRO$TEMATICA AND SPPTD$RESTRICTIVA = 'N') DETALLE_TEMATICA
                            ,POA.SPOA$ID_POA
                            ,POA.SPOA$ANIO

                            FROM SCHE$SIPLAN20.SP20$PERIODO PE
                            INNER JOIN SCHE$SIPLAN20.SP20$POM POM ON POM.SPPO$ID_PERIODO = PE.SPP$ID_PERIODO AND POM.SPPO$RESTRICTIVA = 'N' AND PE.SPP$RESTRICTIVA = 'N'
                            INNER JOIN SCHE$SIPLAN20.SP20$PRODUCTO PO ON PO.SPPRO$POM = POM.SPPO$ID_POM AND PO.SPPRO$RESTRICTIVA = 'N' AND POM.SPPO$RESTRICTIVA = 'N'
                            INNER JOIN SINIP.CP_UNIDADES_MEDIDA U ON U.UNIDAD_MEDIDA = PO.SPPRO$ID_MEDIDA AND PO.SPPRO$RESTRICTIVA = 'N'
                            INNER JOIN SCHE$SIPLAN20.SP20$PROGRAMA_PRESUPUESTARIO PR ON PR.SPPRO$ID_PROGRAMA_PRESUPUESTO = PO.SPPRO$PRESUPUESTO AND PR.SPPRO$ID_INSTITUCION = PO.SPPRO$INSTO AND PR.SPPRO$ID_POM = PO.SPPRO$POM AND PR.SPPRO$RESTRICTIVA = 'N' AND PO.SPPRO$RESTRICTIVA = 'N'
                            INNER JOIN SCHE$SIPLAN20.SP20$POA POA ON POA.SPOA$ID_POM = POM.SPPO$ID_POM AND POM.SPPO$RESTRICTIVA = 'N' AND POA.SPOA$RESTRICTIVA = 'N'
                            WHERE PE.SPP$ID_PERIODO = "+cbPeriodo.Value+@" AND POA.SPOA$ANIO = "+cbanio.Value+@" AND POM.SPPO$ID_INSTITUCION = "+cbInstituiciones.Value+@"
                            ))
                            ORDER BY
                            SPPO$ID_INSTITUCION
                            ,PROGRAMA_PRESUPUESTARIO
                            ,ID_SUBPROGRAMA
                            ,SPPRO$ID_PRODUCTO
                            ,ID_TEMATICA
                            ,ID_DET_TEMATICA
                            ASC";

            estado = dao.consulta(sql);
            if (estado == 1)
            {
                tabla = dao.tabla;
                if (tabla.Rows.Count > 0)
                {
                    gvProductosInsto.DataSource = tabla;
                    gvProductosInsto.DataBind();
                }
                else
                {
                    gvProductosInsto.DataSource = null;
                    gvProductosInsto.DataBind();
                    mensaje = "La institución seleccionada, no tiene produción registrada para este periodo";
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
                }
            }

            else
            {
                mensaje = dao.mensaje;
                Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);

            }
        }

        protected void cbTematica_SelectedIndexChanged(object sender, EventArgs e)
        {
            cargaComboT(Convert.ToInt32(cbTematica.Value));
        }

        protected void btnGrabaUna_Click(object sender, EventArgs e)
        {
            if (gvProductosInsto.FocusedRowIndex != -1)
            {
                if (cbTematica.Text == "")
                {
                    mensaje = "Debe seleccionar una temática";
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
                    cbTematica.Focus();
                }

                else if (cbDetalleTematica.Text == "")
                {
                    mensaje = "Debe seleccionar un detalle de la temática";
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
                    cbDetalleTematica.Focus();
                }
                else
                {
                    sql = "UPDATE SCHE$SIPLAN20.SP20$PRODUCTO SET SPPRO$TEMATICA = " + cbDetalleTematica.Value + " WHERE SPPRO$ID_PRODUCTO = " + gvProductosInsto.GetRowValues(gvProductosInsto.FocusedRowIndex, "SPPRO$ID_PRODUCTO").ToString();
                    estado = dao.comando(sql);
                    if (estado == 1)
                    {
                        mensaje = "La vinculación del producto a la temática se ha realizado correctamente";
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',1);", true);
                        cargaProductosInsto();
                        popVinculaUna.ShowOnPageLoad = false;
                    }
                    else
                    {
                        mensaje = dao.mensaje;
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
                        cargaProductosInsto();
                        popVinculaUna.ShowOnPageLoad = false;
                    }
                }
                
            }
            else
            {
                mensaje = "Seleccione un producto de la tabla para vincular con una temática";
                Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
            }
        }

        protected void btnCancelarUna_Click(object sender, EventArgs e)
        {
            popVinculaUna.ShowOnPageLoad = false;
        }

        protected void btnProductoaTema_Click(object sender, EventArgs e)
        {
            popVinculaUna.ShowOnPageLoad = true;
        }

        protected void btnDesProductoaTema_Click(object sender, EventArgs e)
        {
            if (gvProductosInsto.FocusedRowIndex != -1)
            {
                sql = "UPDATE SCHE$SIPLAN20.SP20$PRODUCTO SET SPPRO$TEMATICA = -1 WHERE SPPRO$ID_PRODUCTO = "+ gvProductosInsto.GetRowValues(gvProductosInsto.FocusedRowIndex, "SPPRO$ID_PRODUCTO").ToString();
                estado = dao.comando(sql);
                if (estado == 1)
                {
                    mensaje = "La desvinculación del producto a la temática se ha realizado correctamente";
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',1);", true);
                    cargaProductosInsto();

                }
                else
                {
                    mensaje = dao.mensaje;
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
                    cargaProductosInsto();
                }
            }
            else
            {
                mensaje = "Seleccione un producto de la tabla para desvincular con una temática";
                Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
            }
                
        }

        protected void cbDetalleTematica2_SelectedIndexChanged(object sender, EventArgs e)
        {
            cargaProdVinculada(Convert.ToInt32(cbDetalleTematica2.Value));
        }

        protected void btnVariosaTema_Click1(object sender, EventArgs e)
        {
            cargaComboTematica2();
            cargaComboT2(Convert.ToInt32(cbDetalleTematica2.Value));
            if (cbDetalleTematica2.Text != "")
                detalle = Convert.ToInt32(cbDetalleTematica2.Value);
            else
                detalle = -1;
            cargaProdVinculada(detalle);
            popUpmuchos.ShowOnPageLoad = true;
        }

        protected void cargaComboTematica1()
        {
            sql = "SELECT SPPTEM$ID, SPPTEM$DESCRIPCION FROM SCHE$SIPLAN20.SP20$TEMATICA WHERE SPPTEM$RESCTRICTIVA = 'N'";
            estado = dao.consulta(sql);
            if (estado == 1)
            {
                tabla = dao.tabla;
                if (tabla.Rows.Count > 0)
                {
                    cbTematica.DataSource = tabla;
                    cbTematica.ValueField = "SPPTEM$ID";
                    cbTematica.TextField = "SPPTEM$DESCRIPCION";
                    cbTematica.DataBind();
                }
                else
                {
                    mensaje = "No se pudo cargar ninguna temática, comuniquese con el administrador del sistema";
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
                }
            }
            else
            {
                mensaje = dao.mensaje;
                Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
            }
        }

        protected void cargaComboT(int tema)
        {

            sql = "SELECT SPPTD$ID, SPPTD$DESCRIPCION FROM  SCHE$SIPLAN20.SP20$DETALLE_TEMATICA WHERE SPPTD$ID_TEMATICA = "+tema+ " AND SPPTD$RESTRICTIVA = 'N'";
            estado = dao.consulta(sql);
            if (estado == 1)
            {
                tabla = dao.tabla;
                if (tabla.Rows.Count > 0)
                {
                    cbDetalleTematica.DataSource = tabla;
                    cbDetalleTematica.ValueField = "SPPTD$ID";
                    cbDetalleTematica.TextField = "SPPTD$DESCRIPCION";
                    cbDetalleTematica.DataBind();
                }
                else
                {
                    mensaje = "No se pudo cargar ninguna temática, comuniquese con el administrador del sistema";
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
                }
            }
            else
            {
                mensaje = dao.mensaje;
                Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
            }

        }

        protected void btnGraTematica2_Click(object sender, EventArgs e)
        {
            int producto = -1;
            if (cbTematica2.Text == "")
            {
                mensaje = "Debe seleccionar una tematica";
                Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
                cbTematica2.Focus();
            }
            else if (cbDetalleTematica2.Text == "")
            {
                mensaje = "Debe seleccionar el detalle de una tematica";
                Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
                cbTematica2.Focus();
            }
            else
            {
                for (int i = 0; i < gvProdVincula.VisibleRowCount; i++)
                {
                    if (gvProdVincula.Selection.IsRowSelected(i))
                    {
                        producto = Convert.ToInt32(gvProdVincula.GetRowValues(i, "SPPRO$ID_PRODUCTO"));
                        sql = "UPDATE SCHE$SIPLAN20.SP20$PRODUCTO SET SPPRO$TEMATICA = " + cbDetalleTematica2.Value + " WHERE SPPRO$ID_PRODUCTO = " + producto;
                    }
                    else
                    {
                        producto = Convert.ToInt32(gvProdVincula.GetRowValues(i, "SPPRO$ID_PRODUCTO"));
                        sql = "UPDATE SCHE$SIPLAN20.SP20$PRODUCTO SET SPPRO$TEMATICA = -1 WHERE SPPRO$ID_PRODUCTO = " + producto;
                    }

                    estado = dao.comando(sql);
                    if (estado == 0)
                        break;
                }

                if (estado == 1)
                {
                    mensaje = "La tematica ha sido vinculada correctamente a los productos seleccionados";
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',1);", true);
                    cargaProductosInsto();
                    popUpmuchos.ShowOnPageLoad = false;

                }
                else
                {
                    mensaje = dao.mensaje;
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
                    cargaProductosInsto();
                    popUpmuchos.ShowOnPageLoad = false;
                }
                
                   
            }

        }

        protected void btnCierraTematica2_Click(object sender, EventArgs e)
        {
            popUpmuchos.ShowOnPageLoad = false;
        }

        protected void cbTematica2_SelectedIndexChanged(object sender, EventArgs e)
        {
            cargaComboT2(Convert.ToInt32(cbTematica2.Value));
            cargaProdVinculada(Convert.ToInt32(cbDetalleTematica2.Value));
        }

        protected void cargaComboTematica2()
        {
            sql = "SELECT SPPTEM$ID, SPPTEM$DESCRIPCION FROM SCHE$SIPLAN20.SP20$TEMATICA WHERE SPPTEM$RESCTRICTIVA = 'N'";
            estado = dao.consulta(sql);
            if (estado == 1)
            {
                tabla = dao.tabla;
                if (tabla.Rows.Count > 0)
                {
                    cbTematica2.DataSource = tabla;
                    cbTematica2.ValueField = "SPPTEM$ID";
                    cbTematica2.TextField = "SPPTEM$DESCRIPCION";
                    cbTematica2.DataBind();
                }
                else
                {
                    mensaje = "No se pudo cargar ninguna temática, comuniquese con el administrador del sistema";
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
                }
            }
            else
            {
                mensaje = dao.mensaje;
                Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
            }
        }

       

        
        protected void cargaComboT2(int tema)
        {

            sql = "SELECT SPPTD$ID, SPPTD$DESCRIPCION FROM  SCHE$SIPLAN20.SP20$DETALLE_TEMATICA WHERE SPPTD$ID_TEMATICA = " + tema + " AND SPPTD$RESTRICTIVA = 'N'";
            estado = dao.consulta(sql);
            if (estado == 1)
            {
                tabla = dao.tabla;
                if (tabla.Rows.Count > 0)
                {
                    cbDetalleTematica2.DataSource = tabla;
                    cbDetalleTematica2.ValueField = "SPPTD$ID";
                    cbDetalleTematica2.TextField = "SPPTD$DESCRIPCION";
                    cbDetalleTematica2.DataBind();
                }
                else
                {
                    mensaje = "No se pudo cargar ninguna temática, comuniquese con el administrador del sistema";
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
                }
            }
            else
            {
                mensaje = dao.mensaje;
                Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
            }

        }

        protected void btnGeneraReporte_Click(object sender, EventArgs e)
        {
            string parametros ="";
            string tematicass = "";
            int conteo = 0;
            string link;
            if (Convert.ToInt32(cbInstitucionesFiltro.Value) != 0)
                parametros = " AND POM.SPPO$ID_INSTITUCION = "+cbInstitucionesFiltro.Value;

            

            tematicass = " AND T.SPPTEM$ID IN(";
            for (int i = 0; i < tematicas.Items.Count; i++)
            {
                if (tematicas.Items[i].Selected)
                {
                    conteo = conteo + 1;
                    if (i == 0 && (tematicas.Items.Count - 1) == 0)
                        tematicass = tematicass + tematicas.Items[i].Value;                                    

                    else 
                        tematicass = tematicass + tematicas.Items[i].Value+",";
                    

                }
            
             }





            if (conteo == 0)
                tematicass = "";
            else if (conteo == 1)
            {
                tematicass = tematicass + ")";
                tematicass = tematicass.Replace(",",String.Empty);
            }
                 
            else if (conteo > 1)
                tematicass = tematicass+"-1)";

            if (Convert.ToInt32(ViewState["reporte"]) == 0)
            {
                link = "https://rpts.segeplan.gob.gt:8080/jasperserver/flow.html?_flowId=viewReportFlow&_flowId=viewReportFlow&periodo=" + cbPeriodo.Value + "&parametrosTematica=" + tematicass + "&parametros=" + parametros + "&anio=" + cbanio.Value + "&j_username=reportes&j_password=reporte&ParentFolderUri=%2Freports%2FSIPLAN&reportUnit=%2Freports%2FSIPLAN%2Ftematicas&standAlone=true&output=xlsx&userLocale=es_GT";
                this.Page.ClientScript.RegisterStartupScript(base.GetType(), "script", string.Concat("window.open('", link, "');"), true);

            }
            else
            {
                link = "https://rpts.segeplan.gob.gt:8080/jasperserver/flow.html?_flowId=viewReportFlow&_flowId=viewReportFlow&periodo=" + cbPeriodo.Value + "&tematicas=" + tematicass + "&parametros=" + parametros + "&anio=" + cbanio.Value + "&j_username=reportes&j_password=reporte&ParentFolderUri=%2Freports%2FSIPLAN&reportUnit=%2Freports%2FSIPLAN%2Ftematica_depto&standAlone=true&output=xlsx&userLocale=es_GT";
                this.Page.ClientScript.RegisterStartupScript(base.GetType(), "script", string.Concat("window.open('", link, "');"), true);

            }

           
        }

        protected void btnCierraReporte_Click(object sender, EventArgs e)
        {
            popReportes.ShowOnPageLoad = false;
        }

        protected void btnReporteDepto_Click(object sender, EventArgs e)
        {
            cargaInstoTematicas();
            ViewState["reporte"] = 1;
        }

        protected void cargaProdVinculada(int tema)
        {
            int m1 = -1;
            sql = @"SELECT 
                            SPPO$ID_POM
                            ,SPPO$ID_INSTITUCION
                            ,SPP$ID_PERIODO
                            ,PROGRAMA_PRESUPUESTARIO||' '||PROGRAMA PROGRAMA 
                            ,CASE WHEN ID_SUBPROGRAMA = -1 THEN SUB_PROGRAMA ELSE ID_SUBPROGRAMA||' '||SUB_PROGRAMA END AS SUBPROGRAMA
                            ,SPPRO$ID_PRODUCTO 
                            ,PRODUCTO
                            ,MEDIDA
                            ,METAFISICA
                            ,METAFINANCIERA
                            ,ID_TEMATICA
                            ,TEMATICA
                            ,ID_DET_TEMATICA
                            ,DETALLE_TEMATICA
                            ,SPOA$ID_POA
                            ,SPOA$ANIO
                            FROM
                            (SELECT
                            SPPO$ID_POM
                            ,SPPO$ID_INSTITUCION
                            ,SPP$ID_PERIODO
                            ,CASE WHEN SPPRO$ID_PROGRAMA_DEPENDE = -1 THEN  SPPRO$ID_PROGRAMA_PRESUPUESTO ELSE SPPRO$ID_PROGRAMA_DEPENDE END AS PROGRAMA_PRESUPUESTARIO
                            ,CASE WHEN SPPRO$ID_PROGRAMA_DEPENDE = -1 THEN SPPRO$DESCRIPCION ELSE PROGRAMA_DEPENDE END AS PROGRAMA
                            ,CASE WHEN SPPRO$ID_PROGRAMA_DEPENDE = -1 THEN SPPRO$ID_PROGRAMA_DEPENDE ELSE SPPRO$ID_PROGRAMA_PRESUPUESTO END AS ID_SUBPROGRAMA
                            ,CASE WHEN SPPRO$ID_PROGRAMA_DEPENDE = -1 THEN 'SIN SUBPROGRAMA' ELSE SPPRO$DESCRIPCION END AS SUB_PROGRAMA
                            ,SPPRO$ID_PRODUCTO 
                            ,PRODUCTO
                            ,MEDIDA
                            ,SCHE$SIPLAN20.FNC$OBTIENEMETAFISICAPRO(SPOA$ID_POA,SPOA$ANIO,SPPRO$ID_PRODUCTO) METAFISICA
                            ,SCHE$SIPLAN20.FNC$OBTIENEMETAFINACIERAPROD(SPOA$ID_POA,SPOA$ANIO,SPPRO$ID_PRODUCTO,0) METAFINANCIERA
                            ,ID_TEMATICA
                            ,(SELECT SPPTEM$DESCRIPCION FROM SCHE$SIPLAN20.SP20$TEMATICA WHERE SPPTEM$ID = ID_TEMATICA) TEMATICA
                            ,SPPRO$TEMATICA ID_DET_TEMATICA
                            ,(SELECT SPPTD$DESCRIPCION FROM SCHE$SIPLAN20.SP20$DETALLE_TEMATICA WHERE SPPTD$ID = SPPRO$TEMATICA) DETALLE_TEMATICA
                            ,SPOA$ID_POA
                            ,SPOA$ANIO
                            FROM
                            (SELECT 
                            POM.SPPO$ID_POM
                            ,POM.SPPO$ID_INSTITUCION
                            ,PE.SPP$ID_PERIODO
                            ,CASE WHEN PR.SPPRO$ID_PROGRAMA_DEPENDE IS NULL THEN -1 ELSE PR.SPPRO$ID_PROGRAMA_DEPENDE END AS SPPRO$ID_PROGRAMA_DEPENDE
                            ,(SELECT SPPRO$DESCRIPCION FROM SCHE$SIPLAN20.SP20$PROGRAMA_PRESUPUESTARIO WHERE SPPRO$ID_PROGRAMA_PRESUPUESTO = PR.SPPRO$ID_PROGRAMA_DEPENDE AND SPPRO$ID_POM = POM.SPPO$ID_POM AND SPPRO$ID_INSTITUCION = POM.SPPO$ID_INSTITUCION AND SPPRO$RESTRICTIVA = 'N') PROGRAMA_DEPENDE
                            ,PR.SPPRO$ID_PROGRAMA_PRESUPUESTO
                            ,PR.SPPRO$DESCRIPCION 
                            ,PO.SPPRO$ID_PRODUCTO 
                            ,PO.SPPRO$DESCRIPCION PRODUCTO
                            ,U.NOMBRE MEDIDA
                            ,PO.SPPRO$TEMATICA
                            ,(SELECT SPPTD$ID_TEMATICA FROM SCHE$SIPLAN20.SP20$DETALLE_TEMATICA WHERE SPPTD$ID = PO.SPPRO$TEMATICA AND SPPTD$RESTRICTIVA = 'N') ID_TEMATICA
                            ,(SELECT SPPTD$DESCRIPCION FROM SCHE$SIPLAN20.SP20$DETALLE_TEMATICA WHERE SPPTD$ID = PO.SPPRO$TEMATICA AND SPPTD$RESTRICTIVA = 'N') DETALLE_TEMATICA
                            ,POA.SPOA$ID_POA
                            ,POA.SPOA$ANIO

                            FROM SCHE$SIPLAN20.SP20$PERIODO PE
                            INNER JOIN SCHE$SIPLAN20.SP20$POM POM ON POM.SPPO$ID_PERIODO = PE.SPP$ID_PERIODO AND POM.SPPO$RESTRICTIVA = 'N' AND PE.SPP$RESTRICTIVA = 'N'
                            INNER JOIN SCHE$SIPLAN20.SP20$PRODUCTO PO ON PO.SPPRO$POM = POM.SPPO$ID_POM AND PO.SPPRO$RESTRICTIVA = 'N' AND POM.SPPO$RESTRICTIVA = 'N'
                            INNER JOIN SINIP.CP_UNIDADES_MEDIDA U ON U.UNIDAD_MEDIDA = PO.SPPRO$ID_MEDIDA AND PO.SPPRO$RESTRICTIVA = 'N'
                            INNER JOIN SCHE$SIPLAN20.SP20$PROGRAMA_PRESUPUESTARIO PR ON PR.SPPRO$ID_PROGRAMA_PRESUPUESTO = PO.SPPRO$PRESUPUESTO AND PR.SPPRO$ID_INSTITUCION = PO.SPPRO$INSTO AND PR.SPPRO$ID_POM = PO.SPPRO$POM AND PR.SPPRO$RESTRICTIVA = 'N' AND PO.SPPRO$RESTRICTIVA = 'N'
                            INNER JOIN SCHE$SIPLAN20.SP20$POA POA ON POA.SPOA$ID_POM = POM.SPPO$ID_POM AND POM.SPPO$RESTRICTIVA = 'N' AND POA.SPOA$RESTRICTIVA = 'N'
                            WHERE PE.SPP$ID_PERIODO = "+cbPeriodo.Value+@" AND POA.SPOA$ANIO = "+cbanio.Value+@" AND POM.SPPO$ID_INSTITUCION = "+cbInstituiciones.Value+@" AND PO.SPPRO$TEMATICA = "+tema+@"
                            
                            UNION
                            
                            SELECT 
                            POM.SPPO$ID_POM
                            ,POM.SPPO$ID_INSTITUCION
                            ,PE.SPP$ID_PERIODO
                            ,CASE WHEN PR.SPPRO$ID_PROGRAMA_DEPENDE IS NULL THEN -1 ELSE PR.SPPRO$ID_PROGRAMA_DEPENDE END AS SPPRO$ID_PROGRAMA_DEPENDE
                            ,(SELECT SPPRO$DESCRIPCION FROM SCHE$SIPLAN20.SP20$PROGRAMA_PRESUPUESTARIO WHERE SPPRO$ID_PROGRAMA_PRESUPUESTO = PR.SPPRO$ID_PROGRAMA_DEPENDE AND SPPRO$ID_POM = POM.SPPO$ID_POM AND SPPRO$ID_INSTITUCION = POM.SPPO$ID_INSTITUCION AND SPPRO$RESTRICTIVA = 'N') PROGRAMA_DEPENDE
                            ,PR.SPPRO$ID_PROGRAMA_PRESUPUESTO
                            ,PR.SPPRO$DESCRIPCION 
                            ,PO.SPPRO$ID_PRODUCTO 
                            ,PO.SPPRO$DESCRIPCION PRODUCTO
                            ,U.NOMBRE MEDIDA
                            ,PO.SPPRO$TEMATICA
                            ,(SELECT SPPTD$ID_TEMATICA FROM SCHE$SIPLAN20.SP20$DETALLE_TEMATICA WHERE SPPTD$ID = PO.SPPRO$TEMATICA AND SPPTD$RESTRICTIVA = 'N') ID_TEMATICA
                            ,(SELECT SPPTD$DESCRIPCION FROM SCHE$SIPLAN20.SP20$DETALLE_TEMATICA WHERE SPPTD$ID = PO.SPPRO$TEMATICA AND SPPTD$RESTRICTIVA = 'N') DETALLE_TEMATICA
                            ,POA.SPOA$ID_POA
                            ,POA.SPOA$ANIO

                            FROM SCHE$SIPLAN20.SP20$PERIODO PE
                            INNER JOIN SCHE$SIPLAN20.SP20$POM POM ON POM.SPPO$ID_PERIODO = PE.SPP$ID_PERIODO AND POM.SPPO$RESTRICTIVA = 'N' AND PE.SPP$RESTRICTIVA = 'N'
                            INNER JOIN SCHE$SIPLAN20.SP20$PRODUCTO PO ON PO.SPPRO$POM = POM.SPPO$ID_POM AND PO.SPPRO$RESTRICTIVA = 'N' AND POM.SPPO$RESTRICTIVA = 'N'
                            INNER JOIN SINIP.CP_UNIDADES_MEDIDA U ON U.UNIDAD_MEDIDA = PO.SPPRO$ID_MEDIDA AND PO.SPPRO$RESTRICTIVA = 'N'
                            INNER JOIN SCHE$SIPLAN20.SP20$PROGRAMA_PRESUPUESTARIO PR ON PR.SPPRO$ID_PROGRAMA_PRESUPUESTO = PO.SPPRO$PRESUPUESTO AND PR.SPPRO$ID_INSTITUCION = PO.SPPRO$INSTO AND PR.SPPRO$ID_POM = PO.SPPRO$POM AND PR.SPPRO$RESTRICTIVA = 'N' AND PO.SPPRO$RESTRICTIVA = 'N'
                            INNER JOIN SCHE$SIPLAN20.SP20$POA POA ON POA.SPOA$ID_POM = POM.SPPO$ID_POM AND POM.SPPO$RESTRICTIVA = 'N' AND POA.SPOA$RESTRICTIVA = 'N'
                            WHERE PE.SPP$ID_PERIODO = "+cbPeriodo.Value+@" AND POA.SPOA$ANIO = "+cbanio.Value+@" AND POM.SPPO$ID_INSTITUCION = "+cbInstituiciones.Value+@" AND PO.SPPRO$TEMATICA = -1
                            ))
                            ORDER BY
                            SPPO$ID_INSTITUCION
                            ,PROGRAMA_PRESUPUESTARIO
                            ,ID_SUBPROGRAMA
                            ,SPPRO$ID_PRODUCTO
                            ,ID_TEMATICA
                            ,ID_DET_TEMATICA
                            ASC";

            estado = dao.consulta(sql);
            if (estado == 1)
            {
                tabla = dao.tabla;
                if (tabla.Rows.Count > 0)
                {
                    gvProdVincula.DataSource = tabla;
                    gvProdVincula.DataBind();
                    gvProdVincula.Selection.UnselectAll();
                    for (int i = 0; i <= gvProdVincula.VisibleRowCount - 1; i++)
                    {
                        m1 = Convert.ToInt32(gvProdVincula.GetRowValues(i, "ID_DET_TEMATICA"));
                        if (m1 != -1)
                            gvProdVincula.Selection.SelectRow(i);
                    }

                }
                else
                {
                    gvProdVincula.DataSource = null;
                    gvProdVincula.DataBind();
                }
            }
            else
            {
                mensaje = dao.mensaje;
                Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
            }
        }

        protected void cargaInstoTematicas()
        {

            sql = @"SELECT  ENTIDAD
                            ,INSTITUCION
                            ,SECTOR
                   FROM
                    (SELECT CG.ENTIDAD
                          , CG.NOMBRE || ' ' || CG.SIGLAS INSTITUCION
                          , CG.SECTOR
                          FROM SINIP.CG_ENTIDADES CG WHERE(ENTIDAD > 1000 OR ENTIDAD = 69 OR ENTIDAD = 20) AND CG.RESTRICTIVA = 'N'
                          AND CG.NOMBRE NOT LIKE('%MUNICIPALIDAD%%')
                          AND CG.ENTIDAD NOT IN 777777
                          UNION
                          SELECT
                          0 ENTIDAD
                          , 'TODAS LAS INSTITUCIONES' INSTITUCION
                          , -1 SECTOR
                          FROM SCHE$SIPLAN20.SP20$PRODUCTO  WHERE SPPRO$ID_PRODUCTO = 80)
                          ORDER BY ENTIDAD, INSTITUCION ASC";

            estado = dao.consulta(sql);
            if (estado == 1)
            {
                tabla = dao.tabla;
                if (tabla.Rows.Count > 0)
                {
                    cbInstitucionesFiltro.DataSource = tabla;
                    cbInstitucionesFiltro.ValueField = "ENTIDAD";
                    cbInstitucionesFiltro.TextField = "INSTITUCION";
                    cbInstitucionesFiltro.DataBind();
                    cbInstitucionesFiltro.SelectedIndex = 0;
                }
                else
                {
                    mensaje = "No se devolvío ningun valor, comuniquese con el administrador del sistema";
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
                }

            }
            else
            {
                mensaje = dao.mensaje;
                Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
            }

            sql = "SELECT SPPTEM$ID, SPPTEM$DESCRIPCION FROM SCHE$SIPLAN20.SP20$TEMATICA WHERE SPPTEM$RESCTRICTIVA = 'N' ORDER BY SPPTEM$ID ASC";
            estado = dao.consulta(sql);
            if (estado == 1)
            {
                tabla = dao.tabla;
                if (tabla.Rows.Count > 0)
                {
                    tematicas.DataSource = tabla;
                    tematicas.ValueField = "SPPTEM$ID";
                    tematicas.TextField = "SPPTEM$DESCRIPCION";
                    tematicas.DataBind();
                    tematicas.UnselectAll();
                }
                else
                {
                    mensaje = "No se devolvío ningun valor, comuniquese con el administrador del sistema";
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
                }
            }
            else
            {
                mensaje = dao.mensaje;
                Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
            }

            if (estado == 1)
                popReportes.ShowOnPageLoad = true;
        }

        protected void btnReporteTematica_Click(object sender, EventArgs e)
        {
            cargaInstoTematicas();
            ViewState["reporte"] = 0;
        }



    }
}