using DevExpress.Web;
using DevExpress.XtraExport.Xls;
using DevExpress.XtraRichEdit.Layout.Export;
using SIPLAN2._0.DataAccess;
using SIPLAN2._0.Rutas;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace SIPLAN2._0.pom
{
    public partial class pom : System.Web.UI.Page
    {
        string sql;
        int estado;
        string mensaje;
        string paths = "";
        string full_path = "";
        DateTime fecha = DateTime.Today;
        clsAccesoBBDD dao = new clsAccesoBBDD();
        Rutas.Rutas path = new Rutas.Rutas();
        DataTable tabla = new DataTable();
        DataTable snips = new DataTable();
        DateTime anio = DateTime.Today;
        DataTable politicasSub = new DataTable();
        protected void Page_Load(object sender, EventArgs e)
        {
            fecha = DateTime.Today;
            
            paths = path.carpetas();
            full_path = path.full_path();

            int codproducto = 0;
            if (Session["Usuario"] == null)
                Response.Redirect("../login/login.aspx");
            else
            {
                if (Session["Insto"] == null)
                {
                    Response.Redirect("../login/login.aspx");
                }
                else
                {
                    
                    if (IsPostBack)
                    {
                        if (Session["cargaSnip"] != null && Convert.ToInt32(Session["cargaSnip"]) == 1)
                        {
                            snips = (DataTable)Session["snip"];
                            gvSNIP.DataSource = snips;
                            gvSNIP.DataBind();
                        }

                        if (Session["POLITCAS"] != null)
                        {
                            tabla = (DataTable)Session["POLITCAS"];
                            if (tabla.Rows.Count > 0)
                            {
                                gvPoliticasLA.DataSource = tabla;
                                gvPoliticasLA.DataBind();
                                gvPoliticasLA.ExpandAll();
                            }
                        }

                        if (Session["POLITCAS_SUBFORM"] != null)
                        {
                            tabla = (DataTable)Session["POLITCAS_SUBFORM"];
                            if (tabla.Rows.Count > 0)
                            {
                                gvDetallePoliticaSub.DataSource = tabla;
                                gvDetallePoliticaSub.DataBind();
                                gvDetallePoliticaSub.ExpandAll();
                            }
                        }

                        if (Session["MUNICIPIOS"] != null)
                        {
                            tabla = (DataTable)Session["MUNICIPIOS"];
                            if (tabla.Rows.Count > 0)
                            {
                                gvTerritorio.DataSource = tabla;
                                gvTerritorio.DataBind();
                                gvTerritorio.ExpandAll();
                            }
                        }

                   }
                    
                    if (Session["ROL"].ToString() == "ADMIN")
                        btnAbrir.Visible = true;
                    else
                        btnAbrir.Visible = false;
                    

                    if (Session["ROL"].ToString() == "ENTIDAD")
                    {
                        btnNuevoPom.Enabled = false;
                        btnNuevoInstrumento.Enabled = false;
                        btnAdjuntarArchivo.Enabled = false;
                        btnEliminarArchivo.Enabled = false;
                        btnVincula.Enabled = false;
                        btnInstores.Enabled = false;
                        btnPrograma.Enabled = false;
                        btnNuevo.Enabled = false;
                        btnEdit.Enabled = false;
                        btnDelete.Enabled = false;
                        btnSNIP.Enabled = false;
                        btnVincRED.Enabled = false;
                        btnVinculaPGG.Enabled = false;

                    }

                    if (gvPOMInsto.FocusedRowIndex != -1)
                    {
                        Instos.Text = gvPOMInsto.GetRowValues(gvPOMInsto.FocusedRowIndex, "NOMBRE").ToString() + " " + gvPOMInsto.GetRowValues(gvPOMInsto.FocusedRowIndex, "SPPO$INICIO").ToString() + "-" + gvPOMInsto.GetRowValues(gvPOMInsto.FocusedRowIndex, "SPPO$FINAL").ToString();
                    }

                    cargaPOMS();
                    //cargaCombos();



                    if (selectProgra.SelectedIndex == 1)
                    {
                        PanelPrograma.Style.Add("display", "block");
                    }
                    else if (selectProgra.SelectedIndex == 0)
                    {
                        PanelPrograma.Style.Add("display", "none");
                    }

                    if (Session["pom"] != null && Session["insto"] != null)
                    {


                        if (!IsPostBack)
                        {
                            cargaResInsto(Convert.ToInt32(Session["pom"].ToString()), Convert.ToInt32(Session["insto"].ToString()));
                            cargaPrograma(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]));
                            cargaProductosInsto(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]));
                            cargaProductoRed(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]));

                            cargaProdutosTemporal(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]));
                            cargaPGG2428(Convert.ToInt32(Session["insto"]));
                            carga1eje();
                        }

                        else
                        {
                            if (Session["Institucionales"] != null)
                            {
                                gvResInsto.DataSource = (DataTable)Session["Institucionales"];
                                gvResInsto.DataBind();
                            }

                            if (Session["presupesto"] != null)
                                carga_programa_presupuestario(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]));
                            if (Session["prod_institucionales"] != null)
                            {

                                gvProdInsto.DataSource = (DataTable)Session["prod_institucionales"];
                                gvProdInsto.DataBind();
                            }

                            if (Session["productosRED"] != null)
                            {
                                gvProdEstrategicos.DataSource = (DataTable)Session["productosRED"];
                                gvProdEstrategicos.DataBind();
                            }

                            if (Session["temporales"] != null)
                            {
                                gvProduccionInsto.DataSource = (DataTable)Session["temporales"];
                                gvProduccionInsto.DataBind();
                            }
                        }


                        if (Session["tiposub"] != null)
                        {
                            if (gvProdEstrategicos.FocusedRowIndex != -1 && Session["tiposub"].ToString() == "subes")
                            {
                                codproducto = Convert.ToInt32(gvProdEstrategicos.GetRowValues(gvProdEstrategicos.FocusedRowIndex, "SPPRO$ID_PRODUCTO").ToString());
                                cargaSubEstrategicos(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), codproducto);
                            }

                            else if (gvProdInsto.FocusedRowIndex != -1 && Session["tiposub"].ToString() == "subins")
                            {
                                codproducto = Convert.ToInt32(gvProdInsto.GetRowValues(gvProdInsto.FocusedRowIndex, "SPPRO$ID_PRODUCTO").ToString());
                                cargaSubInstos(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), codproducto);
                            }
                        }

                        if (Session["subsPP"] != null)
                        {
                            gvSubproductosPP.DataSource = (DataTable)Session["subsPP"];
                            gvSubproductosPP.DataBind();
                        }

                        ScriptManager scripManager = ScriptManager.GetCurrent(this.Page);

                        scripManager.RegisterPostBackControl(btnGrabaInstrumento);

                    }
                    btnEliminarArchivo.Attributes.Add("onclick", "return confirm('Esta seguro(a) de eliminar este instrumento de planificación, por favor confirme la operación')");


                }


            }


        }




        protected void gvPOMInsto_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            DataTable tempo = new DataTable();
            DataTable tempo1 = new DataTable();
            DataTable dtpoa = new DataTable();
            DataTable tempos2 = new DataTable();
            tempo = (DataTable)Session["POMS"];
            String usuario = Session["Usuario"].ToString();

            for (int i = 0; i <= tempo.Rows.Count - 1; i++)
            {

                if (tempo.Rows[i]["SPPO$ID_POM"].ToString() == e.Keys["SPPO$ID_POM"].ToString())
                {



                    if (Session["ROL"].ToString() != "ENTIDAD")
                    {
                        int codigo = Convert.ToInt32(e.Values["SPPO$ID_POM"]);
                        sql = "UPDATE SCHE$SIPLAN20.SP20$POM SET SPPO$RESTRICTIVA = 'S', SPPO$FECHA_DELETE = 'DELETE = '||TO_CHAR(SYSDATE,'DD/MM/YYYY HH24:MI')||' '||'" + Session["USUARIO"].ToString() + "' WHERE SPPO$ID_POM = " + codigo;
                        estado = dao.comando(sql);
                        if (estado == 1)
                        {
                            mensaje = dao.mensaje;
                            gvPOMInsto.JSProperties["cpError"] = "Operacion correcta: Registro borrado correctamente";
                            e.Cancel = true;
                            cargaPOMS();
                            break;
                        }
                        else
                        {
                            mensaje = dao.mensaje;
                            gvPOMInsto.JSProperties["cpError"] = "Ocurrio un error: " + mensaje;
                            e.Cancel = true;
                            break;
                        }

                    }

                    else
                    {
                        gvPOMInsto.JSProperties["cpError"] = "Usted no esta autorizado para borrar este registro";
                        e.Cancel = true;
                        break;
                    }

                }




            }

        }



        protected void btnNuevoPom_Click(object sender, EventArgs e)
        {
            cargaCombos();
            MultiView1.ActiveViewIndex = 1;
        }

        protected void cargaPOMS()
        {
            if (Session["ROL"].ToString() == "USER")
            {
                /*sql = "SELECT POM.SPPO$ID_PERIODO, POM.SPPO$ID_INSTITUCION,  POM.SPPO$ID_POM,  CG.NOMBRE, POM.SPPO$INICIO, POM.SPPO$FINAL, POM.SPPO$PROPIETARIO FROM SCHE$SIPLAN20.SP20$POM POM INNER JOIN SCHE$SIPLAN20.SP20$PERIODO PE ON POM.SPPO$ID_PERIODO = PE.SPP$ID_PERIODO AND PE.SPP$RESTRICTIVA = 'N' AND POM.SPPO$RESTRICTIVA = 'N'";
                sql = sql + "INNER JOIN SINIP.CG_ENTIDADES CG ON POM.SPPO$ID_INSTITUCION = CG.ENTIDAD AND POM.SPPO$RESTRICTIVA = 'N' AND CG.RESTRICTIVA = 'N' WHERE POM.SPPO$RESTRICTIVA = 'N' AND POM.SPPO$FINAL BETWEEN " + Convert.ToInt32(anio.Year) + " AND POM.SPPO$FINAL AND CG.ENTIDAD = " + Convert.ToInt32(Session["Insto"]);*/

                sql = @"SELECT 
                        SPPO$ID_PERIODO
                        ,SPPO$ID_INSTITUCION
                        ,SPPO$ID_POM
                        ,NOMBRE||'-'||SIGLA NOMBRE
                        ,SPPO$INICIO
                        ,SPPO$FINAL
                        ,SPPO$PROPIETARIO
                        
                        
                        ,CASE WHEN SPPO$ESTADO = 'A' THEN 'PROGRAMACIÓN CERRADA' 
                              WHEN SPPO$ESTADO = 'D' AND FECHA_PRORROGA IS NOT NULL AND TRUNC(SYSDATE) <= TRUNC(FECHA_PRORROGA) THEN 'PROGRAMACIÓN ABIERTA'          
                              WHEN SPPO$ESTADO = 'D' AND TRUNC(SYSDATE) <= TRUNC(FECHA_CIERRE) THEN 'PROGRAMACIÓN ABIERTA' 
                              WHEN SPPO$ESTADO = 'D' AND  TRUNC(SYSDATE) > TRUNC(FECHA_CIERRE) THEN 'PROGRAMACIÓN CERRADA' 
                              WHEN SPPO$ESTADO = 'D' AND FECHA_CIERRE IS NULL THEN 'PROGRAMACIÓN CERRADA'
                                                 
                         END AS ESTADO  
                        ,CASE WHEN SPPO$ESTADO = 'A' THEN 1 
                              WHEN SPPO$ESTADO = 'D' AND FECHA_PRORROGA IS NOT NULL AND TRUNC(SYSDATE) <= TRUNC(FECHA_PRORROGA) THEN 0 
                              WHEN SPPO$ESTADO = 'D'  AND TRUNC(SYSDATE) <= TRUNC(FECHA_CIERRE) THEN 0  
                              WHEN SPPO$ESTADO = 'D'  AND TRUNC(SYSDATE) > TRUNC(FECHA_CIERRE)  THEN 1  
                              WHEN SPPO$ESTADO = 'D'  AND FECHA_CIERRE IS NULL THEN 1 
                                                              
                         END AS CODESTADO
                        ,SPP$ABIERTO
                        FROM
                        (SELECT POM.SPPO$ID_PERIODO
                        ,POM.SPPO$ID_INSTITUCION
                        ,POM.SPPO$ID_POM
                        ,CG.NOMBRE
                        ,CG.SIGLA
                        ,POM.SPPO$INICIO
                        ,POM.SPPO$FINAL
                        ,POM.SPPO$PROPIETARIO 
                        
                        ,POM.SPPO$ESTADO
                        ,PE.SPP$ABIERTO
                        ,(SELECT SPFC$FECHA_CIERRE FROM SCHE$SIPLAN20.SP20$FECHAS_CIERRE WHERE SPFC$TIPO_FECHA = 2 AND SPFC$RESTRICTIVA = 'N' AND SPFC$PERIODO_POM = POM.SPPO$ID_PERIODO AND SPFC$POM = -1 AND SPFC$INSTITUCION = -1) FECHA_CIERRE
                        ,(SELECT MAX(SPFC$FECHA_CIERRE) FECHA_CIERRA FROM SCHE$SIPLAN20.SP20$FECHAS_CIERRE WHERE SPFC$TIPO_FECHA = 2 AND SPFC$RESTRICTIVA = 'N' AND SPFC$PERIODO_POM = POM.SPPO$ID_PERIODO AND SPFC$POM = POM.SPPO$ID_POM AND SPFC$INSTITUCION = POM.SPPO$ID_INSTITUCION) FECHA_PRORROGA
                        FROM SCHE$SIPLAN20.SP20$POM POM 
                        INNER JOIN SCHE$SIPLAN20.SP20$PERIODO PE ON POM.SPPO$ID_PERIODO = PE.SPP$ID_PERIODO AND PE.SPP$RESTRICTIVA = 'N' AND POM.SPPO$RESTRICTIVA = 'N'
                        INNER JOIN SINIP.CG_ENTIDADES CG ON POM.SPPO$ID_INSTITUCION = CG.ENTIDAD AND POM.SPPO$RESTRICTIVA = 'N' AND CG.RESTRICTIVA = 'N' 
                        WHERE POM.SPPO$RESTRICTIVA = 'N' AND POM.SPPO$FINAL BETWEEN " + Convert.ToInt32(anio.Year) + " AND POM.SPPO$FINAL AND CG.ENTIDAD = " + Convert.ToInt32(Session["Insto"]) + ") ORDER BY SPPO$ID_PERIODO DESC, SPPO$ID_INSTITUCION ASC";


            }
            else
            {
                /* sql = "SELECT POM.SPPO$ID_PERIODO, POM.SPPO$ID_INSTITUCION,  POM.SPPO$ID_POM,  CG.NOMBRE, POM.SPPO$INICIO, POM.SPPO$FINAL, POM.SPPO$PROPIETARIO FROM SCHE$SIPLAN20.SP20$POM POM INNER JOIN SCHE$SIPLAN20.SP20$PERIODO PE ON POM.SPPO$ID_PERIODO = PE.SPP$ID_PERIODO AND PE.SPP$RESTRICTIVA = 'N' AND POM.SPPO$RESTRICTIVA = 'N'";
                 sql = sql + "INNER JOIN SINIP.CG_ENTIDADES CG ON POM.SPPO$ID_INSTITUCION = CG.ENTIDAD AND POM.SPPO$RESTRICTIVA = 'N' AND CG.RESTRICTIVA = 'N' WHERE POM.SPPO$RESTRICTIVA = 'N' AND POM.SPPO$FINAL BETWEEN " + Convert.ToInt32(anio.Year) + " AND POM.SPPO$FINAL";
                 */
                sql = @"SELECT 
                        SPPO$ID_PERIODO
                        ,SPPO$ID_INSTITUCION
                        ,SPPO$ID_POM
                        ,NOMBRE||'-'||SIGLA NOMBRE
                        ,SPPO$INICIO
                        ,SPPO$FINAL
                        ,SPPO$PROPIETARIO 
                        
                        
                        ,CASE WHEN SPPO$ESTADO = 'A' THEN 'PROGRAMACIÓN CERRADA' 
                              WHEN SPPO$ESTADO = 'D' AND FECHA_PRORROGA IS NOT NULL AND TRUNC(SYSDATE) <= TRUNC(FECHA_PRORROGA) THEN 'PROGRAMACIÓN ABIERTA'   
                              WHEN SPPO$ESTADO = 'D' AND TRUNC(SYSDATE) <= TRUNC(FECHA_CIERRE) THEN 'PROGRAMACIÓN ABIERTA' 
                              WHEN SPPO$ESTADO = 'D' AND  TRUNC(SYSDATE) > TRUNC(FECHA_CIERRE) THEN 'PROGRAMACIÓN CERRADA' 
                              WHEN SPPO$ESTADO = 'D' AND FECHA_CIERRE IS NULL THEN 'PROGRAMACIÓN CERRADA'
                                                       
                         END AS ESTADO  
                        ,CASE WHEN SPPO$ESTADO = 'A' THEN 1 
                              WHEN SPPO$ESTADO = 'D' AND FECHA_PRORROGA IS NOT NULL AND TRUNC(SYSDATE) <= TRUNC(FECHA_PRORROGA) THEN 0 
                              WHEN SPPO$ESTADO = 'D'  AND TRUNC(SYSDATE) <= TRUNC(FECHA_CIERRE) THEN 0  
                              WHEN SPPO$ESTADO = 'D'  AND TRUNC(SYSDATE) > TRUNC(FECHA_CIERRE)  THEN 1  
                              WHEN SPPO$ESTADO = 'D'  AND FECHA_CIERRE IS NULL THEN 1 
                                                              
                         END AS CODESTADO
                        ,SPP$ABIERTO                        
                        FROM
                        (SELECT POM.SPPO$ID_PERIODO
                        ,POM.SPPO$ID_INSTITUCION
                        ,POM.SPPO$ID_POM
                        ,CG.NOMBRE
                        ,NVL(CG.SIGLA,' ') SIGLA
                        ,POM.SPPO$INICIO
                        ,POM.SPPO$FINAL
                        ,POM.SPPO$PROPIETARIO 
                        
                        ,POM.SPPO$ESTADO
                         ,CASE WHEN SPPO$ESTADO = 'A' THEN 1 WHEN SPPO$ESTADO = 'D' THEN 0  END AS CODESTADO  
                        ,PE.SPP$ABIERTO 
                        ,(SELECT SPFC$FECHA_CIERRE FROM SCHE$SIPLAN20.SP20$FECHAS_CIERRE WHERE SPFC$TIPO_FECHA = 2 AND SPFC$RESTRICTIVA = 'N' AND SPFC$PERIODO_POM = POM.SPPO$ID_PERIODO AND SPFC$POM = -1 AND SPFC$INSTITUCION = -1) FECHA_CIERRE
                        ,(SELECT MAX(SPFC$FECHA_CIERRE) FECHA_CIERRA FROM SCHE$SIPLAN20.SP20$FECHAS_CIERRE WHERE SPFC$TIPO_FECHA = 2 AND SPFC$RESTRICTIVA = 'N' AND SPFC$PERIODO_POM = POM.SPPO$ID_PERIODO AND SPFC$POM = POM.SPPO$ID_POM AND SPFC$INSTITUCION = POM.SPPO$ID_INSTITUCION) FECHA_PRORROGA
                        FROM 
                        SCHE$SIPLAN20.SP20$POM POM 
                        INNER JOIN SCHE$SIPLAN20.SP20$PERIODO PE ON POM.SPPO$ID_PERIODO = PE.SPP$ID_PERIODO AND PE.SPP$RESTRICTIVA = 'N' AND POM.SPPO$RESTRICTIVA = 'N'
                        INNER JOIN SINIP.CG_ENTIDADES CG ON POM.SPPO$ID_INSTITUCION = CG.ENTIDAD AND POM.SPPO$RESTRICTIVA = 'N' AND CG.RESTRICTIVA = 'N' 
                        WHERE POM.SPPO$RESTRICTIVA = 'N' AND POM.SPPO$FINAL BETWEEN " + Convert.ToInt32(anio.Year) + " AND POM.SPPO$FINAL) ORDER BY SPPO$ID_PERIODO DESC, SPPO$ID_INSTITUCION ASC";
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
                Session["POMS"] = tabla;
                gvPOMInsto.DataSource = tabla;
                gvPOMInsto.DataBind();

            }
        }

        protected void cargaCombos()
        {
            sql = "SELECT P.SPP$ID_PERIODO, P.SPP$INICIO||'-'||P.SPP$FINAL VIGENCIA FROM SCHE$SIPLAN20.SP20$PERIODO P WHERE P.SPP$RESTRICTIVA = 'N' AND P.SPP$ABIERTO = 0";
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
                    cbPeriodosPom.DataSource = tabla;
                    cbPeriodosPom.ValueField = "SPP$ID_PERIODO";
                    cbPeriodosPom.TextField = "VIGENCIA";
                    cbPeriodosPom.DataBind();

                    sql = "SELECT CG.ENTIDAD, CG.NOMBRE||' '||CG.SIGLA INSTITUCION, CG.SECTOR FROM SINIP.CG_ENTIDADES CG WHERE ENTIDAD > 1000 AND CG.RESTRICTIVA = 'N' AND CG.NOMBRE NOT LIKE ('%MUNICIPALIDAD%%') AND CG.ENTIDAD NOT IN 777777 ORDER BY CG.NOMBRE ASC";
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
                            cbInstituiciones.SelectedIndex = -1;
                            cbInstituiciones.Enabled = true;
                        }

                    }
                }

            }

        }

        protected void btnPOM_Click(object sender, EventArgs e)
        {
            sql = "SELECT P.SPP$ID_PERIODO, P.SPP$INICIO, P.SPP$FINAL, P.SPP$ORDEN FROM SCHE$SIPLAN20.SP20$PERIODO P INNER JOIN SCHE$SIPLAN20.SP20$POM POM ON P.SPP$ID_PERIODO = POM.SPPO$ID_PERIODO AND P.SPP$RESTRICTIVA = 'N' AND POM.SPPO$RESTRICTIVA = 'N' WHERE POM.SPPO$ID_PERIODO = " + Convert.ToInt32(cbPeriodosPom.Value) + " AND SPPO$ID_INSTITUCION = " + Convert.ToInt32(cbInstituiciones.Value);
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
                    mensaje = "Ya ha una Programación Multianual programada para " + cbInstituiciones.Text + ", por favor revise su configuración";
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
                }
                else
                {
                    sql = "SELECT * FROM SCHE$SIPLAN20.SP20$PERIODO P WHERE P.SPP$RESTRICTIVA = 'N' AND  P.SPP$ID_PERIODO = " + Convert.ToInt32(cbPeriodosPom.Value);

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
                            if (cbPeriodosPom.Text == "")
                            {
                                mensaje = "Seleccione el periodo de vigencia";
                                Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
                                cbPeriodosPom.Focus();
                            }

                            else if (cbInstituiciones.Text == "")
                            {
                                mensaje = "Seleccione la institución";
                                Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
                                cbInstituiciones.Focus();
                            }
                            else
                            {
                                sql = "INSERT INTO SCHE$SIPLAN20.SP20$POM (SPPO$ID_INSTITUCION, SPPO$ID_PERIODO, SPPO$INICIO, SPPO$FINAL,SPPO$FECHA_INSERT, SPPO$PROPIETARIO) VALUES(" + Convert.ToInt32(cbInstituiciones.Value) + ", " + Convert.ToInt32(tabla.Rows[0]["SPP$ID_PERIODO"]) + "," + Convert.ToInt32(tabla.Rows[0]["SPP$INICIO"]) + "," + Convert.ToInt32(tabla.Rows[0]["SPP$FINAL"]) + ", 'INSERT = '||TO_CHAR(SYSDATE,'DD/MM/YYYY HH24:MI')||' '||'" + Session["USUARIO"].ToString() + "', '" + Session["USUARIO"].ToString() + "')";
                                estado = dao.comando(sql);
                                if (estado == 0)
                                {
                                    mensaje = dao.mensaje;
                                    Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
                                }
                                else
                                {
                                    cargaPOMS();
                                    cargaCombos();
                                    mensaje = "Programación Multianual configurada correctamente";
                                    Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',1);", true);
                                    MultiView1.ActiveViewIndex = 0;
                                }
                            }
                        }
                        else
                        {
                            mensaje = "Periodo invalido, por favor contacte al administrador";
                            Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
                        }


                    }

                }



            }

        }

        protected void btnCancelPOM_Click(object sender, EventArgs e)
        {
            MultiView1.ActiveViewIndex = 0;
        }

        protected void btnResultados_Click(object sender, EventArgs e)
        {
            int pom, insto, periodo;
            DataTable poa;
            DataTable periodos;
            if (gvPOMInsto.FocusedRowIndex == -1)
            {
                mensaje = "Debe seleccionar primero el encabezado de su Programación Multianual";
                Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
                MultiView1.ActiveViewIndex = 0;

            }
            else
            {
                Session["pom"] = Convert.ToInt32(gvPOMInsto.GetRowValues(gvPOMInsto.FocusedRowIndex, "SPPO$ID_POM").ToString());
                Session["insto"] = Convert.ToInt32(gvPOMInsto.GetRowValues(gvPOMInsto.FocusedRowIndex, "SPPO$ID_INSTITUCION").ToString());
                Session["abierto"] = Convert.ToInt32(gvPOMInsto.GetRowValues(gvPOMInsto.FocusedRowIndex, "SPP$ABIERTO").ToString());
                // Session["nombre"] = gvPOMInsto.GetRowValues(gvPOMInsto.FocusedRowIndex, "NOMBRE").ToString();
                pom = Convert.ToInt32(gvPOMInsto.GetRowValues(gvPOMInsto.FocusedRowIndex, "SPPO$ID_POM").ToString());
                insto = Convert.ToInt32(gvPOMInsto.GetRowValues(gvPOMInsto.FocusedRowIndex, "SPPO$ID_INSTITUCION").ToString());
                periodo = Convert.ToInt32(gvPOMInsto.GetRowValues(gvPOMInsto.FocusedRowIndex, "SPPO$ID_PERIODO").ToString());
                sql = "SELECT P.SPP$ID_PERIODO, POA.SPOA$ID_POA, POA.SPOA$ANIO, POA.SPOA$ID_POM, POA.SPOA$ID_INSTITUCION FROM SCHE$SIPLAN20.SP20$POA POA INNER JOIN SCHE$SIPLAN20.SP20$POM POM ON POM.SPPO$ID_POM = POA.SPOA$ID_POM AND POM.SPPO$ID_INSTITUCION = POA.SPOA$ID_INSTITUCION AND POM.SPPO$RESTRICTIVA = 'N' AND POA.SPOA$RESTRICTIVA = 'N' ";
                sql = sql + "INNER JOIN SCHE$SIPLAN20.SP20$PERIODO P ON P.SPP$ID_PERIODO = POM.SPPO$ID_PERIODO AND P.SPP$RESTRICTIVA = 'N' AND POM.SPPO$RESTRICTIVA = 'N' WHERE POA.SPOA$ID_POM = " + pom + " AND POA.SPOA$ID_INSTITUCION =" + insto + " AND P.SPP$ID_PERIODO = " + periodo;

                estado = dao.consulta(sql);
                if (estado != 0)
                {
                    poa = dao.tabla;
                    if (poa.Rows.Count == 0)
                    {
                        sql = "SELECT * FROM SCHE$SIPLAN20.SP20$PERIODO P WHERE P.SPP$ID_PERIODO = " + periodo + " AND P.SPP$RESTRICTIVA = 'N'";
                        estado = dao.consulta(sql);
                        periodos = dao.tabla;
                        if (periodos.Rows.Count > 0)
                        {

                            for (int i = Convert.ToInt32(periodos.Rows[0]["SPP$INICIO"]); i <= Convert.ToInt32(periodos.Rows[0]["SPP$FINAL"]); i++)
                            {
                                sql = "INSERT INTO SCHE$SIPLAN20.SP20$POA (SPOA$ANIO, SPOA$ID_POM, SPOA$ID_INSTITUCION, SPOA$FECHA_INSERT, SPOA$PROPIETARIO) VALUES (" + i + ", " + pom + ", " + insto + ", 'INSERT = '||TO_CHAR(SYSDATE,'DD/MM/YYYY HH24:MI')||' '||'" + Session["USUARIO"].ToString() + "', '" + Session["USUARIO"].ToString() + "' )";
                                estado = dao.comando(sql);
                                if (estado == 0)
                                {

                                    break;
                                }


                            }
                        }

                    }
                }





                cargaResInsto(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]));

                resultados.SelectedIndex = 1;
                resultados.Enabled = false;
                Institucionales.Style.Add("display", "block");
                Estrategicos.Style.Add("display", "none");

                MultiView1.ActiveViewIndex = 2;




            }
        }


        protected void cargaResInsto(int pom, int insto)
        {

            int respuesta = 0;
            respuesta = busca_resultadoInstitucional(pom, insto);
            if (respuesta == 0)
            {
                Session["CONFIRMA"] = 1; //variable de migración de resultados institucionales
                Confirmamigracion.Text = "No se han encontrado resultados institucionales para el periodo de esta programación, ¿Necesita trasladar la información de los resultados del periodo anterior a la presente programación?";
                Session["Institucionales"] = null;
                gvResInsto.DataSource = null;
                gvResInsto.DataBind();
                Confirma.ShowOnPageLoad = true;



            }
            else
                carga_resultado_insto(pom, insto);


        }


        protected void cargaProductoRed(int pom, int insto)
        {

            int respuesta = 0;
            respuesta = busca_ProductoRED(pom, insto);
            if (respuesta == 0)
            {
                Session["CONFIRMA"] = 4; //variable de migración de productos RED
                Confirmamigracion.Text = "No se han encontrado productos vinculados a Resultados Estrátegicos, para el periodo de esta programación, ¿Necesita trasladar la información de productos RE del periodo anterior a la presente programación?";
                Session["productosRED"] = null;
                gvProdEstrategicos.DataSource = null;
                gvProdEstrategicos.DataBind();
                Confirma.ShowOnPageLoad = true;



            }
            else
                CargaProductosRed(pom, insto);


        }


        protected int busca_ProductoRED(int pom, int insto)
        {
            int resultado = 0;
            sql = "SELECT P.* FROM SCHE$SIPLAN20.SP20$PRODUCTO  P INNER JOIN SCHE$SIPLAN20.SP20$RESULTADOS R ON R.SPRES$ID_RESULTADO = P.SPPRO$ID_RESULTADO AND R.SPRES$RESTRICTIVA = 'N' AND P.SPPRO$RESTRICTIVA = 'N' WHERE P.SPPRO$RESTRICTIVA = 'N' AND P.SPPRO$POM = " + pom + " AND P.SPPRO$INSTO =" + insto + " AND R.SPRES$TIPO = 2";
            estado = dao.consulta(sql);
            if (estado == 1)
                tabla = dao.tabla;
            if (tabla.Rows.Count > 0)
                resultado = 1;
            else
                resultado = 0;



            return resultado;

        }

        protected void CargaProductosRed(int pom, int insto)
        {
            /*sql = @"SELECT 
                    RED.SPPRED$ID
                    ,RED.SPPRED$RED || ' ' || RED.SPPRED$DESCRIPCION RED
                    , PR.SPPRO$ID_PROGRAMA_PRESUPUESTO
                    ,PR.SPPRO$DESCRIPCION PROGRAMA_PRESUPUETARIO
                    ,PO.SPPRO$ID_PRODUCTO
                    ,PO.SPPRO$DESCRIPCION PRODUCTO
                    ,PO.SPPRO$ID_MEDIDA
                    ,UM.SNCGUM$NOMBRE
                    ,PO.SPPRO$OBJETIVO_CENTRAL
                    ,PO.SPPRO$ACCION_ESTRATEGICA
                    ,(SELECT R.SPPRES$DESCRIPCION FROM SCHE$SIPLAN20.SP20$RESULTADOS_ESTRATEGICOS R WHERE R.SPPRES$RESTRICTIVA = 'N' AND R.SPPRES$ID_RESULTADO = PO.SPPRO$OBJETIVO_CENTRAL AND R.SPPRES$NIVEL = 3) OBJETIVO_SECTORIAL
                    ,(SELECT R.SPPRES$DESCRIPCION FROM SCHE$SIPLAN20.SP20$RESULTADOS_ESTRATEGICOS R WHERE R.SPPRES$RESTRICTIVA = 'N' AND R.SPPRES$ID_RESULTADO = PO.SPPRO$ACCION_ESTRATEGICA AND R.SPPRES$NIVEL = 4) ACCCION_ESTRATEGICA
                    ,PO.SPPRO$PROPIETARIO
                    ,(SELECT CASE WHEN COUNT(M.NOMBRE) > 0 THEN 'SI' ELSE 'NO' END AS  MUNICIPIO FROM SINIP.CG_GEOGRAFICO M INNER JOIN SCHE$SIPLAN20.SP20$MUNOSPRIORIZADOS MP ON M.GEOGRAFICO = MP.GEOGRAFICO AND MP.RESTRICTIVA = 'N' WHERE MP.SPPRO$ID_PRODUCTO = PO.SPPRO$ID_PRODUCTO) MUNICIPIOS
                    ,R.SPRES$ID_RESULTADO ID_RESULTADO
                    FROM SCHE$SIPLAN20.SP20$RESULTADOS R
                    INNER JOIN SCHE$SIPLAN20.SP20$PRODUCTO PO ON R.SPRES$ID_RESULTADO = PO.SPPRO$ID_RESULTADO AND R.SPRES$POM = PO.SPPRO$POM AND R.SPRES$INSTITUCION = PO.SPPRO$INSTO AND R.SPRES$RESTRICTIVA = 'N' AND PO.SPPRO$RESTRICTIVA = 'N'
                    INNER JOIN SCHE$SIPLAN20.SP20$PROGRAMA_PRESUPUESTARIO PR ON PO.SPPRO$PRESUPUESTO = PR.SPPRO$ID_PROGRAMA_PRESUPUESTO AND PO.SPPRO$POM = PR.SPPRO$ID_POM AND PR.SPPRO$ID_INSTITUCION = PO.SPPRO$INSTO AND PR.SPPRO$RESTRICTIVA = 'N' AND PO.SPPRO$RESTRICTIVA = 'N'
                    INNER JOIN SINIP.SNTBCG$UNIDAD_MEDIDA UM ON PO.SPPRO$ID_MEDIDA = UM.SNCGUM$UNIDAD_MEDIDA
                    INNER JOIN SCHE$SIPLAN20.SP20$RED RED ON RED.SPPRED$ID = R.SPRES$COD_ESTRATEGICO AND RED.SPPRED$RESTRICTIVA = 'N' AND R.SPRES$RESTRICTIVA = 'N'
                    WHERE
                    R.SPRES$POM = " + pom + " AND R.SPRES$INSTITUCION = " + insto + @" AND R.SPRES$TIPO = 2


                    ORDER BY RED.SPPRED$ID, PR.SPPRO$ID_PROGRAMA_PRESUPUESTO, PO.SPPRO$ID_PRODUCTO ASC";*/

            sql = @"SELECT 
                    RED.SPPRED$ID
                    ,RED.SPPRED$RED || ' ' || RED.SPPRED$DESCRIPCION RED
                    ,SCHE$SIPLAN20.FCN$BUSCA_EJE(PO.SPPRO$RESULTADO2) EJE
                    ,SCHE$SIPLAN20.FCN$BUSCA_META(PO.SPPRO$RESULTADO2) META
                    ,SCHE$SIPLAN20.FCN$BUSCA_ACCION(PO.SPPRO$RESULTADO2)ACCION
                    , PR.SPPRO$ID_PROGRAMA_PRESUPUESTO
                    ,PR.SPPRO$DESCRIPCION PROGRAMA_PRESUPUETARIO
                    ,CASE WHEN PR.SPPRO$ES_ADMINISTRATIVO = 0 THEN 'PROGRAMAS SUSTANTIVOS' ELSE 'PROGRAMAS ADMINISTRATIVOS' END AS NATURALEZA
                    ,PR.SPPRO$ES_ADMINISTRATIVO
                    ,PO.SPPRO$ID_PRODUCTO
                    ,PO.SPPRO$DESCRIPCION PRODUCTO
                    ,PO.SPPRO$ID_MEDIDA
                    ,UM.SNCGUM$NOMBRE
                    ,PO.SPPRO$OBJETIVO_CENTRAL
                    ,PO.SPPRO$ACCION_ESTRATEGICA
                    ,(SELECT R.SPPRES$DESCRIPCION FROM SCHE$SIPLAN20.SP20$RESULTADOS_ESTRATEGICOS R WHERE R.SPPRES$RESTRICTIVA = 'N' AND R.SPPRES$ID_RESULTADO = PO.SPPRO$OBJETIVO_CENTRAL AND R.SPPRES$NIVEL = 3) OBJETIVO_SECTORIAL
                    ,(SELECT R.SPPRES$DESCRIPCION FROM SCHE$SIPLAN20.SP20$RESULTADOS_ESTRATEGICOS R WHERE R.SPPRES$RESTRICTIVA = 'N' AND R.SPPRES$ID_RESULTADO = PO.SPPRO$ACCION_ESTRATEGICA AND R.SPPRES$NIVEL = 4) ACCCION_ESTRATEGICA
                    ,PO.SPPRO$PROPIETARIO
                    ,(SELECT CASE WHEN COUNT(M.NOMBRE) > 0 THEN 'SI' ELSE 'NO' END AS  MUNICIPIO FROM SINIP.CG_GEOGRAFICO M INNER JOIN SCHE$SIPLAN20.SP20$MUNOSPRIORIZADOS MP ON M.GEOGRAFICO = MP.GEOGRAFICO AND MP.RESTRICTIVA = 'N' WHERE MP.SPPRO$ID_PRODUCTO = PO.SPPRO$ID_PRODUCTO) MUNICIPIOS
                    ,R.SPRES$ID_RESULTADO ID_RESULTADO
                    ,PO.SPPRO$RESULTADO2
                    FROM SCHE$SIPLAN20.SP20$RESULTADOS R
                    INNER JOIN SCHE$SIPLAN20.SP20$PRODUCTO PO ON R.SPRES$ID_RESULTADO = PO.SPPRO$ID_RESULTADO AND R.SPRES$POM = PO.SPPRO$POM AND R.SPRES$INSTITUCION = PO.SPPRO$INSTO AND R.SPRES$RESTRICTIVA = 'N' AND PO.SPPRO$RESTRICTIVA = 'N'
                    INNER JOIN SCHE$SIPLAN20.SP20$PROGRAMA_PRESUPUESTARIO PR ON PO.SPPRO$PRESUPUESTO = PR.SPPRO$ID_PROGRAMA_PRESUPUESTO AND PO.SPPRO$POM = PR.SPPRO$ID_POM AND PR.SPPRO$ID_INSTITUCION = PO.SPPRO$INSTO AND PR.SPPRO$RESTRICTIVA = 'N' AND PO.SPPRO$RESTRICTIVA = 'N'
                    INNER JOIN SINIP.SNTBCG$UNIDAD_MEDIDA UM ON PO.SPPRO$ID_MEDIDA = UM.SNCGUM$UNIDAD_MEDIDA
                    INNER JOIN SCHE$SIPLAN20.SP20$RED RED ON RED.SPPRED$ID = R.SPRES$COD_ESTRATEGICO AND RED.SPPRED$RESTRICTIVA = 'N' AND R.SPRES$RESTRICTIVA = 'N'
                    WHERE
                    R.SPRES$POM = " + pom + " AND R.SPRES$INSTITUCION = " + insto + @" AND R.SPRES$TIPO = 2
                    ORDER BY RED.SPPRED$ID, PR.SPPRO$ID_PROGRAMA_PRESUPUESTO, PO.SPPRO$ID_PRODUCTO ASC";

            estado = dao.consulta(sql);
            if (estado == 1)
            {
                tabla = dao.tabla;
                if (tabla.Rows.Count > 0)
                {
                    Session["productosRED"] = tabla;
                    gvProdEstrategicos.DataSource = tabla;
                    gvProdEstrategicos.DataBind();

                }
                else
                {
                    mensaje = "No hay productos vinculados a RE en el periodo anterior";
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',1);", true);
                    ScriptManager.RegisterStartupScript(this.panProdEstrategicos, GetType(), "script", "Alerta('" + mensaje + " <br/>',1);", true);
                }
            }
            else
            {
                mensaje = dao.mensaje;
                Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
                ScriptManager.RegisterStartupScript(this.panProdEstrategicos, GetType(), "script", "Alerta('" + mensaje + " <br/>',2);", true);
            }

        }
        protected int busca_resultadoInstitucional(int pom, int insto)
        {
            int resultado = 0;
            sql = "SELECT * FROM SCHE$SIPLAN20.SP20$RESULTADOS WHERE SPRES$POM = " + pom + " AND SPRES$INSTITUCION = " + insto + " AND SPRES$RESTRICTIVA = 'N' AND SPRES$TIPO = 1";
            estado = dao.consulta(sql);
            if (estado == 1)
                tabla = dao.tabla;
            if (tabla.Rows.Count > 0)
                resultado = 1;
            else
                resultado = 0;
            return resultado;
        }
        protected void migra_resultados_institucionales(int pom, int insto)
        {
            int orden = -1;
            DataTable poms = new DataTable();
            DataTable resultado = new DataTable();

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
                            if (poms.Rows.Count > 0)
                            {
                                sql = "SELECT * FROM SCHE$SIPLAN20.SP20$RESULTADOS R WHERE R.SPRES$POM = " + poms.Rows[0]["SPPO$ID_POM"] + " AND R.SPRES$INSTITUCION = " + poms.Rows[0]["SPPO$ID_INSTITUCION"] + " AND R.SPRES$RESTRICTIVA = 'N' AND R.SPRES$TIPO = 1";
                                estado = dao.consulta(sql);
                                if (estado == 1)
                                {
                                    resultado = dao.tabla;
                                    if (resultado.Rows.Count > 0)
                                    {
                                        for (int i = 0; i < resultado.Rows.Count; i++)
                                        {
                                            sql = "INSERT INTO SCHE$SIPLAN20.SP20$RESULTADOS (SPRES$TIPO,SPRES$DESCRIPCION,SPRES$FECHA_INSERT, SPRES$POM, SPRES$INSTITUCION, SPRES$PROPIETARIO, SPRES$ID_ANTERIOR) VALUES (1,'" + resultado.Rows[i]["SPRES$DESCRIPCION"] + "','INSERT = '||TO_CHAR(SYSDATE,'DD/MM/YYYY HH24:MI')||' '||'" + Session["USUARIO"].ToString() + "'," + pom + "," + insto + ",'" + Session["USUARIO"].ToString() + "', "+ resultado.Rows[i]["SPRES$ID_RESULTADO"] + ")";
                                            estado = dao.comando(sql);
                                            if (estado == 0)
                                                break;
                                        }

                                        if (estado == 1)
                                        {
                                            sql = "SELECT R.SPRES$ID_RESULTADO, R.SPRES$TIPO, R.SPRES$DESCRIPCION,  (SELECT  rtrim(xmlagg(xmlelement(e, P.SPPRO$ID_PROGRAMA_PRESUPUESTO || ' ' || P.SPPRO$DESCRIPCION || ', ')).extract('//text()'), ',') PROGRAMA  FROM SCHE$SIPLAN20.SP20$PROGRAMA_PRESUPUESTARIO P INNER JOIN SCHE$SIPLAN20.SP20$PRODUCTO PO ON P.SPPRO$ID_PROGRAMA_PRESUPUESTO = PO.SPPRO$PRESUPUESTO AND P.SPPRO$ID_POM = PO.SPPRO$POM AND P.SPPRO$ID_INSTITUCION = PO.SPPRO$INSTO AND  P.SPPRO$RESTRICTIVA = 'N' AND PO.SPPRO$RESTRICTIVA = 'N' INNER JOIN ";
                                            sql = sql + "SCHE$SIPLAN20.SP20$RESULTADOS RP ON RP.SPRES$ID_RESULTADO = PO.SPPRO$ID_RESULTADO AND  RP.SPRES$POM = PO.SPPRO$POM AND RP.SPRES$INSTITUCION = PO.SPPRO$INSTO AND  RP.SPRES$RESTRICTIVA = 'N' AND PO.SPPRO$RESTRICTIVA = 'N' WHERE P.SPPRO$ID_POM = " + Convert.ToInt32(Session["pom"]) + " AND P.SPPRO$ID_INSTITUCION = " + Convert.ToInt32(Session["insto"]) + " AND RP.SPRES$ID_RESULTADO = R.SPRES$ID_RESULTADO) AS PROGRAMA_PRESUPUESTARIO, R.SPRES$PROPIETARIO FROM SCHE$SIPLAN20.SP20$RESULTADOS R WHERE R.SPRES$POM = " + Convert.ToInt32(Session["pom"]) + " AND R.SPRES$INSTITUCION = " + Convert.ToInt32(Session["insto"]) + " AND R.SPRES$RESTRICTIVA = 'N' AND R.SPRES$TIPO = 1";

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
                                                    Session["Institucionales"] = tabla;
                                                    gvResInsto.DataSource = tabla;
                                                    gvResInsto.DataBind();
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


        protected void migra_resultados_RED(int pom, int insto)
        {
            int orden = -1;
            DataTable poms = new DataTable();
            DataTable resultado = new DataTable();

            sql = "SELECT * FROM SCHE$SIPLAN20.SP20$RESULTADOS WHERE SPRES$TIPO = 2 AND SPRES$POM = " + pom + " AND SPRES$INSTITUCION = " + insto + " AND SPRES$RESTRICTIVA = 'N'";
            estado = dao.consulta(sql);
            if (estado == 1)
            {
                tabla = dao.tabla;
                if (tabla.Rows.Count <= 0)
                {
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
                                    if (poms.Rows.Count > 0)
                                    {
                                        sql = "SELECT * FROM SCHE$SIPLAN20.SP20$RESULTADOS R WHERE R.SPRES$POM = " + poms.Rows[0]["SPPO$ID_POM"] + " AND R.SPRES$INSTITUCION = " + poms.Rows[0]["SPPO$ID_INSTITUCION"] + " AND R.SPRES$RESTRICTIVA = 'N' AND R.SPRES$TIPO = 2";
                                        estado = dao.consulta(sql);
                                        if (estado == 1)
                                        {
                                            resultado = dao.tabla;
                                            if (resultado.Rows.Count > 0)
                                            {
                                                for (int i = 0; i < resultado.Rows.Count; i++)
                                                {
                                                    sql = "INSERT INTO SCHE$SIPLAN20.SP20$RESULTADOS (SPRES$TIPO,SPRES$COD_ESTRATEGICO,SPRES$FECHA_INSERT, SPRES$POM, SPRES$INSTITUCION, SPRES$PROPIETARIO,SPRES$ID_ANTERIOR) VALUES (2,'" + resultado.Rows[i]["SPRES$COD_ESTRATEGICO"] + "','INSERT = '||TO_CHAR(SYSDATE,'DD/MM/YYYY HH24:MI')||' '||'" + Session["USUARIO"].ToString() + "'," + pom + "," + insto + ",'" + Session["USUARIO"].ToString() + "',"+ resultado.Rows[i]["SPRES$ID_RESULTADO"] + ")";
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

        protected void migra_programas_presupuestarios(int pom, int insto)
        {
            int orden = -1;
            DataTable poms = new DataTable();
            DataTable programas = new DataTable();
            string codigo;
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
                            if (poms.Rows.Count > 0)
                            {
                                sql = "SELECT * FROM SCHE$SIPLAN20.SP20$PROGRAMA_PRESUPUESTARIO WHERE SPPRO$ID_POM = " + poms.Rows[0]["SPPO$ID_POM"] + " AND SPPRO$ID_INSTITUCION = " + poms.Rows[0]["SPPO$ID_INSTITUCION"] + " AND SPPRO$RESTRICTIVA = 'N'";
                                estado = dao.consulta(sql);
                                if (estado == 1)
                                {
                                    programas = dao.tabla;
                                    for (int i = 0; i < programas.Rows.Count; i++)
                                    {
                                        sql = "INSERT INTO SCHE$SIPLAN20.SP20$PROGRAMA_PRESUPUESTARIO (SPPRO$ID_PROGRAMA_PRESUPUESTO, SPPRO$ID_PROGRAMA_DEPENDE, SPPRO$DESCRIPCION, SPPRO$ID_POM, SPPRO$ID_INSTITUCION, SPPRO$FECHA_INSERT, SPPRO$PROPIETARIO, SPPRO$ID_PROGRA_ANTERIOR, SPPRO$POM_ANTERIOR, SPPRO$INSTO_ANTERIOR, SPPRO$DEPENDE_ANTERIOR) VALUES (" + programas.Rows[i]["SPPRO$ID_PROGRAMA_PRESUPUESTO"];
                                        if (programas.Rows[i]["SPPRO$ID_PROGRAMA_DEPENDE"] == DBNull.Value)
                                            sql = sql + ",NULL";
                                        else
                                            sql = sql + "," + programas.Rows[i]["SPPRO$ID_PROGRAMA_DEPENDE"];
                                        sql = sql + ",'" + programas.Rows[i]["SPPRO$DESCRIPCION"] + "'," + pom + "," + insto + ",'INSERT = ' || TO_CHAR(SYSDATE, 'DD/MM/YYYY HH24:MI') || ' ' || '" + Session["USUARIO"].ToString() + "','" + Session["USUARIO"].ToString() + "'," + programas.Rows[i]["SPPRO$ID_PROGRAMA_PRESUPUESTO"] + ", " + programas.Rows[i]["SPPRO$ID_POM"] + "," + programas.Rows[i]["SPPRO$ID_INSTITUCION"];


                                        if (programas.Rows[i]["SPPRO$ID_PROGRAMA_DEPENDE"] == DBNull.Value)
                                            sql = sql + ",NULL";
                                        else
                                            sql = sql + "," + programas.Rows[i]["SPPRO$ID_PROGRAMA_DEPENDE"];

                                        sql=sql + ")";
                                        estado = dao.comando(sql);
                                        if (estado == 0)
                                        {
                                            mensaje = dao.mensaje;
                                            codigo = mensaje.Substring(0, 9);
                                            if (codigo == "ORA-00001")
                                            {
                                                sql = "UPDATE SCHE$SIPLAN20.SP20$PROGRAMA_PRESUPUESTARIO SET SPPRO$RESTRICTIVA = 'N' WHERE SPPRO$ID_PROGRAMA_PRESUPUESTO = " + programas.Rows[i]["SPPRO$ID_PROGRAMA_PRESUPUESTO"] + " AND SPPRO$ID_POM = " + pom + " AND SPPRO$ID_INSTITUCION = " + insto;
                                                estado = dao.comando(sql);
                                                if (estado == 0)
                                                    break;
                                            }
                                            else
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

        protected void migra_productos_institucionales(int pom, int insto)
        {
            sql = "SELECT * FROM SCHE$SIPLAN20.SP20$RESULTADOS WHERE SPRES$TIPO = 0 AND SPRES$POM = " + pom + " AND SPRES$INSTITUCION = " + insto + " AND SPRES$RESTRICTIVA = 'N'";
            estado = dao.consulta(sql);
            if (estado == 1)
                tabla = dao.tabla;

            if (tabla.Rows.Count <= 0)
                cargaResEstrategico(pom, insto);

            sql = "SELECT P.* FROM SCHE$SIPLAN20.SP20$PRODUCTO P INNER JOIN SCHE$SIPLAN20.SP20$RESULTADOS R ON R.SPRES$ID_RESULTADO = P.SPPRO$ID_RESULTADO AND P.SPPRO$RESTRICTIVA = 'N' AND R.SPRES$RESTRICTIVA = 'N' WHERE R.SPRES$TIPO = 0 AND P.SPPRO$POM = " + pom + " AND P.SPPRO$INSTO = " + insto;
            estado = dao.consulta(sql);

            if (estado == 1)
                tabla = dao.tabla;

            if (tabla.Rows.Count <= 0)
                cargaProdEstrategicos_SIPLAN(pom, insto);

            sql = "SELECT P.* FROM SCHE$SIPLAN20.SP20$PRODUCTO P INNER JOIN SCHE$SIPLAN20.SP20$RESULTADOS R ON R.SPRES$ID_RESULTADO = P.SPPRO$ID_RESULTADO AND P.SPPRO$RESTRICTIVA = 'N' AND R.SPRES$RESTRICTIVA = 'N' WHERE R.SPRES$TIPO = 1  AND P.SPPRO$POM = " + pom + " AND P.SPPRO$INSTO = " + insto;
            estado = dao.consulta(sql);

            if (estado == 1)
                tabla = dao.tabla;

            if (tabla.Rows.Count <= 0)
                cargaProductosResultadosInstitucionales(pom, insto);


        }


        protected void cargaResEstrategico(int pom, int insto)
        {

            DataTable periodos = new DataTable();
            DataTable poms = new DataTable();
            DataTable resultado = new DataTable();
            int orden = -1;
            DataTable pgg_antes = new DataTable();
            DataTable pgg_antes1 = new DataTable();



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

                            if (poms.Rows.Count > 0)
                            {

                                sql = "SELECT * FROM SCHE$SIPLAN20.SP20$RESULTADOS R WHERE R.SPRES$POM = " + poms.Rows[0]["SPPO$ID_POM"] + " AND R.SPRES$INSTITUCION = " + poms.Rows[0]["SPPO$ID_INSTITUCION"] + " AND R.SPRES$RESTRICTIVA = 'N' AND R.SPRES$TIPO = 0";
                                estado = dao.consulta(sql);
                                if (estado == 1)
                                {
                                    resultado = dao.tabla;
                                    if (resultado.Rows.Count > 0)
                                    {
                                        if (pom != 3202)
                                        {
                                            for (int i = 0; i < resultado.Rows.Count; i++)
                                            {
                                                sql = "INSERT INTO SCHE$SIPLAN20.SP20$RESULTADOS (SPRES$TIPO,SPRES$COD_ESTRATEGICO,SPRES$DESCRIPCION,SPRES$FECHA_INSERT, SPRES$POM, SPRES$INSTITUCION, SPRES$PROPIETARIO, SPRES$ID_ANTERIOR) VALUES (0," + resultado.Rows[i]["SPRES$COD_ESTRATEGICO"];
                                                if (resultado.Rows[i]["SPRES$DESCRIPCION"] != DBNull.Value)
                                                    sql = sql + ",'" + resultado.Rows[i]["SPRES$DESCRIPCION"] + "'";
                                                else
                                                    sql = sql + ",NULL";
                                                sql = sql + ",'INSERT = '||TO_CHAR(SYSDATE,'DD/MM/YYYY HH24:MI')||' '||'" + Session["USUARIO"].ToString() + "'," + pom + "," + insto + ",'" + Session["USUARIO"].ToString() + "', "+ resultado.Rows[i]["SPRES$ID_RESULTADO"] + ")";
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



        protected void carga_resultado_insto(int pom, int insto)
        {
            sql = @"SELECT R.SPRES$ID_RESULTADO
                    ,R.SPRES$TIPO
                    ,R.SPRES$DESCRIPCION
                    ,(SELECT COUNT(P.SPPRO$ID_PROGRAMA_PRESUPUESTO) PROGRAMA  FROM SCHE$SIPLAN20.SP20$PROGRAMA_PRESUPUESTARIO P INNER JOIN SCHE$SIPLAN20.SP20$PRODUCTO PO ON P.SPPRO$ID_PROGRAMA_PRESUPUESTO = PO.SPPRO$PRESUPUESTO AND P.SPPRO$ID_POM = PO.SPPRO$POM AND P.SPPRO$ID_INSTITUCION = PO.SPPRO$INSTO AND  P.SPPRO$RESTRICTIVA = 'N' AND PO.SPPRO$RESTRICTIVA = 'N' INNER JOIN 
                     SCHE$SIPLAN20.SP20$RESULTADOS RP ON RP.SPRES$ID_RESULTADO = PO.SPPRO$ID_RESULTADO AND  RP.SPRES$POM = PO.SPPRO$POM AND RP.SPRES$INSTITUCION = PO.SPPRO$INSTO AND  RP.SPRES$RESTRICTIVA = 'N' AND PO.SPPRO$RESTRICTIVA = 'N' WHERE P.SPPRO$ID_POM = " + Convert.ToInt32(Session["pom"]) + " AND P.SPPRO$ID_INSTITUCION = " + Convert.ToInt32(Session["insto"]) + @" AND RP.SPRES$ID_RESULTADO = R.SPRES$ID_RESULTADO) AS PROGRAMA_PRESUPUESTARIO
                    ,R.SPRES$PROPIETARIO FROM SCHE$SIPLAN20.SP20$RESULTADOS R WHERE R.SPRES$POM = " + Convert.ToInt32(Session["pom"]) + " AND R.SPRES$INSTITUCION = " + Convert.ToInt32(Session["insto"]) + " AND R.SPRES$RESTRICTIVA = 'N' AND R.SPRES$TIPO = 1";

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
                    Session["Institucionales"] = tabla;
                    gvResInsto.DataSource = tabla;
                    gvResInsto.DataBind();

                }
                else
                    Session["Institucionales"] = null;
            }

        }
        protected void gvResInsto_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            DataTable tempo1 = new DataTable();
            tempo1 = (DataTable)Session["Institucionales"];
            string valor;

            for (int i = 0; i <= tempo1.Rows.Count - 1; i++)
            {
                if (tempo1.Rows[i]["SPRES$ID_RESULTADO"].ToString() == e.Keys["SPRES$ID_RESULTADO"].ToString())
                {
                    //if ((tempo1.Rows[i]["SPRES$PROPIETARIO"].ToString() == Session["Usuario"].ToString()) || Session["ROL"].ToString() == "ADMIN")
                    if (Session["ROL"].ToString() != "ENTIDAD" || Convert.ToInt32(Session["abierto"]) != 1)
                    {
                        int codigo = Convert.ToInt32(e.Keys["SPRES$ID_RESULTADO"]);
                        valor = e.NewValues["SPRES$DESCRIPCION"].ToString();
                        sql = "UPDATE SCHE$SIPLAN20.SP20$RESULTADOS SET SPRES$DESCRIPCION = '" + valor + "', SPRES$FECHA_UPDATE = 'UPDATE = '||TO_CHAR(SYSDATE,'DD/MM/YYYY HH24:MI')||' '||'" + Session["USUARIO"].ToString() + "'  WHERE SPRES$ID_RESULTADO = " + codigo + " AND SPRES$RESTRICTIVA = 'N'";
                        estado = dao.comando(sql);
                        if (estado == 0)
                        {
                            mensaje = dao.mensaje;
                            gvResInsto.JSProperties["cpError"] = "Ocurrio un error: " + mensaje;
                            e.Cancel = true;
                            gvResInsto.CancelEdit();
                            cargaResInsto(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]));
                            break;

                        }
                        else
                        {
                            mensaje = dao.mensaje;
                            gvResInsto.JSProperties["cpError"] = "Operacion correcta: Registro acualizado correctamente";
                            e.Cancel = true;
                            gvResInsto.CancelEdit();
                            cargaResInsto(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]));
                            break;
                        }

                    }
                    else
                    {
                        gvResInsto.JSProperties["cpError"] = "Su perfil de usuario no esta autorizado para editar este registro";
                        e.Cancel = true;
                        gvResInsto.CancelEdit();
                        cargaResInsto(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]));
                        break;
                    }

                }
            }

        }

        protected void gvResInsto_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            DataTable tempo1 = new DataTable();
            tempo1 = (DataTable)Session["Institucionales"];
            int resultado = 0;
            for (int i = 0; i <= tempo1.Rows.Count - 1; i++)
            {
                if (tempo1.Rows[i]["SPRES$ID_RESULTADO"].ToString() == e.Keys["SPRES$ID_RESULTADO"].ToString())
                {
                    //if ((tempo1.Rows[i]["SPRES$PROPIETARIO"].ToString() == Session["Usuario"].ToString()) || Session["ROL"].ToString() == "ADMIN")
                    if (Session["ROL"].ToString() != "ENTIDAD")
                    {
                        int codigo = Convert.ToInt32(e.Keys["SPRES$ID_RESULTADO"]);
                        //sql = "SELECT * FROM SCHE$SIPLAN20.SP20$RESULTA_PRESU P WHERE P.SPRP$RESULTADO = " + codigo + " AND SPRP$RESTRICTIVA = 'N'";
                        sql = "SELECT * FROM SCHE$SIPLAN20.SP20$RESULTADOS R INNER JOIN SCHE$SIPLAN20.SP20$PRODUCTO P ON R.SPRES$ID_RESULTADO = P.SPPRO$ID_RESULTADO AND R.SPRES$POM = P.SPPRO$POM AND R.SPRES$INSTITUCION = P.SPPRO$INSTO  AND R.SPRES$RESTRICTIVA = 'N' AND P.SPPRO$RESTRICTIVA = 'N' WHERE R.SPRES$TIPO = 1 AND R.SPRES$ID_RESULTADO  = " + codigo + " AND R.SPRES$POM = " + Convert.ToInt32(Session["pom"]) + " AND R.SPRES$INSTITUCION = " + Convert.ToInt32(Session["insto"]) + " AND R.SPRES$RESTRICTIVA = 'N' ";
                        estado = dao.consulta(sql);
                        if (estado == 0)
                        {
                            mensaje = dao.mensaje;
                            gvResInsto.JSProperties["cpError"] = "Ocurrio un error: " + mensaje;
                            e.Cancel = true;
                            cargaResInsto(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]));
                            break;
                        }
                        else
                        {
                            tabla.Clear();
                            tabla = dao.tabla;
                            if (tabla.Rows.Count > 0)
                            {
                                gvPOMInsto.JSProperties["cpError"] = "Este resultado institucional esta vinculado a programas presupestarios, tiene que borrarlos primero para inactivar este registro";
                                e.Cancel = true;
                                cargaResInsto(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]));
                                break;
                            }
                            else
                            {
                                sql = "UPDATE SCHE$SIPLAN20.SP20$RESULTADOS SET SPRES$RESTRICTIVA = 'S', SPRES$FECHA_DELETE = 'DELETE = '||TO_CHAR(SYSDATE,'DD/MM/YYYY HH24:MI')||' '||'" + Session["USUARIO"].ToString() + "' WHERE SPRES$ID_RESULTADO = " + codigo + " AND SPRES$POM = " + Convert.ToInt32(Session["pom"]) + " AND SPRES$INSTITUCION = " + Convert.ToInt32(Session["insto"]);
                                estado = dao.comando(sql);

                                if (estado == 0)
                                {
                                    mensaje = dao.mensaje;
                                    gvResInsto.JSProperties["cpError"] = "Ocurrio un error: " + mensaje;
                                    resultado = busca_resultadoInstitucional(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]));
                                    if (resultado == 0)
                                        gvResInsto.JSProperties["cpShow"] = "1";
                                    else
                                        gvResInsto.JSProperties["cpShow"] = null;
                                    e.Cancel = true;
                                    cargaResInsto(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]));
                                    break;

                                }
                                else
                                {
                                    mensaje = dao.mensaje;
                                    gvResInsto.JSProperties["cpError"] = "Operacion correcta: Registro borrado correctamente";
                                    resultado = busca_resultadoInstitucional(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]));
                                    if (resultado == 0)
                                        gvResInsto.JSProperties["cpShow"] = "1";
                                    else
                                        gvResInsto.JSProperties["cpShow"] = null;
                                    e.Cancel = true;

                                    cargaResInsto(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]));
                                    break;

                                }
                            }
                        }
                    }
                    else
                    {
                        gvResInsto.JSProperties["cpError"] = "Usted no esta autorizado para borrar este registro";
                        e.Cancel = true;
                        cargaResInsto(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]));
                        break;
                    }
                }
            }


        }

        protected void btnInstores_Click(object sender, EventArgs e)
        {

            if (txtResultado.Text == "")
            {
                mensaje = "La descripción del resultado institucional es requerida";
                Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
            }
            else
            {
                sql = "INSERT INTO SCHE$SIPLAN20.SP20$RESULTADOS (SPRES$TIPO, SPRES$DESCRIPCION, SPRES$FECHA_INSERT, SPRES$POM, SPRES$INSTITUCION, SPRES$PROPIETARIO) VALUES (1, '" + txtResultado.Text + "', 'INSERT = '||TO_CHAR(SYSDATE,'DD/MM/YYYY HH24:MI')||' '||'" + Session["USUARIO"].ToString() + "', " + Convert.ToInt32(Session["pom"]) + ", " + Convert.ToInt32(Session["insto"]) + ", '" + Session["USUARIO"].ToString() + "')";
                estado = dao.comando(sql);
                if (estado == 0)
                {
                    mensaje = dao.mensaje;
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
                }
                else

                {
                    mensaje = "Resultado institucional grabado correctamente";
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',1);", true);
                    txtResultado.Text = "";

                    cargaResInsto(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]));
                    if (resultados.SelectedIndex == 1)
                    {
                        Estrategicos.Style.Add("display", "none");
                        Institucionales.Style.Add("display", "block");
                    }


                }
            }



        }

        protected void btnVincularRes_Click(object sender, EventArgs e)
        {
            int cod_estrategico;
            int estados = 0;
            DataTable resto = new DataTable();

            for (int i = 0; i < gvResEstrategicos.VisibleRowCount; i++)
            {

                if (gvResEstrategicos.Selection.IsRowSelected(i))
                {
                    cod_estrategico = Convert.ToInt32(gvResEstrategicos.GetRowValues(i, "ID_ACCION"));
                    sql = "SELECT * FROM SCHE$SIPLAN20.SP20$RESULTADOS WHERE SPRES$RESTRICTIVA = 'N' AND SPRES$COD_ESTRATEGICO = " + cod_estrategico + " AND SPRES$POM = " + Convert.ToInt32(Session["pom"]) + " AND SPRES$INSTITUCION = " + Convert.ToInt32(Session["insto"]) + " AND SPRES$TIPO = 0";
                    estado = dao.consulta(sql);
                    if (estado == 0)
                    {
                        mensaje = dao.mensaje;
                        //Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
                        ScriptManager.RegisterStartupScript(this.upModalEstrategicos, GetType(), "script", "Alerta('" + mensaje + " <br/>',2);", true);
                        estados = 0;
                        break;

                    }

                    else
                    {
                        tabla = dao.tabla;

                        if (tabla.Rows.Count > 0)
                            continue;
                        else
                        {
                            sql = "INSERT INTO SCHE$SIPLAN20.SP20$RESULTADOS(SPRES$TIPO,SPRES$COD_ESTRATEGICO, SPRES$FECHA_INSERT, SPRES$POM, SPRES$INSTITUCION, SPRES$PROPIETARIO) VALUES (0, " + cod_estrategico + ", 'INSERT = '||TO_CHAR(SYSDATE,'DD/MM/YYYY HH24:MI')||' '||'" + Session["USUARIO"].ToString() + "', " + Convert.ToInt32(Session["pom"]) + ", " + Convert.ToInt32(Session["insto"]) + ", '" + Session["USUARIO"].ToString() + "')";
                            estado = dao.comando(sql);
                            if (estado == 0)
                            {
                                mensaje = dao.mensaje;
                                //Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
                                ScriptManager.RegisterStartupScript(this.upModalEstrategicos, GetType(), "script", "Alerta('" + mensaje + " <br/>',2);", true);
                                estados = 0;
                                break;
                            }
                            else
                                estados = 1;

                        }



                    }



                }
                else
                {
                    cod_estrategico = Convert.ToInt32(gvResEstrategicos.GetRowValues(i, "ID_ACCION"));
                    sql = "SELECT * FROM SCHE$SIPLAN20.SP20$RESULTADOS R INNER JOIN SCHE$SIPLAN20.SP20$PRODUCTO P ON R.SPRES$ID_RESULTADO = P.SPPRO$ID_RESULTADO AND R.SPRES$POM = P.SPPRO$POM AND R.SPRES$INSTITUCION = P.SPPRO$INSTO  AND R.SPRES$RESTRICTIVA = 'N' AND P.SPPRO$RESTRICTIVA = 'N' WHERE R.SPRES$TIPO = 0 AND R.SPRES$COD_ESTRATEGICO = " + cod_estrategico + " AND R.SPRES$POM = " + Convert.ToInt32(Session["pom"]) + " AND R.SPRES$INSTITUCION = " + Convert.ToInt32(Session["insto"]) + " AND R.SPRES$RESTRICTIVA = 'N'";
                    estado = dao.consulta(sql);
                    if (estado == 0)
                    {
                        mensaje = dao.mensaje;
                        //Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
                        ScriptManager.RegisterStartupScript(this.upModalEstrategicos, GetType(), "script", "Alerta('" + mensaje + " <br/>',2);", true);
                        estados = 0;
                        break;
                    }
                    else
                    {
                        resto = dao.tabla;
                        if (resto.Rows.Count > 0)
                        {

                            estados = 1;
                            continue;

                        }

                        else
                        {
                            sql = "UPDATE SCHE$SIPLAN20.SP20$RESULTADOS SET SPRES$RESTRICTIVA = 'S', SPRES$FECHA_DELETE = 'DELETE = '||TO_CHAR(SYSDATE,'DD/MM/YYYY HH24:MI')||' '||'" + Session["USUARIO"].ToString() + "' WHERE SPRES$TIPO = 0 AND SPRES$COD_ESTRATEGICO = " + cod_estrategico + " AND SPRES$POM = " + Convert.ToInt32(Session["pom"]) + " AND SPRES$INSTITUCION = " + Convert.ToInt32(Session["insto"]);
                            estado = dao.comando(sql);
                            if (estado == 0)
                            {
                                mensaje = dao.mensaje;
                                //Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
                                ScriptManager.RegisterStartupScript(this.upModalEstrategicos, GetType(), "script", "Alerta('" + mensaje + " <br/>',2);", true);
                                estados = 0;
                                break;
                            }
                            else
                            {
                                estados = 1;
                            }
                        }
                    }
                }
            }

            if (estados == 0)
            {
                mensaje = "Es posible que errores del sistema hayan ocasionado una operación incorrecta, por favor contacte al administrador";
                //Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
                ScriptManager.RegisterStartupScript(this.upModalEstrategicos, GetType(), "script", "Alerta('" + mensaje + " <br/>',1);", true);

                //MultiView1.ActiveViewIndex = 2;
                //Estrategicos.Style.Add("display", "block");
                //Institucionales.Style.Add("display", "none");

                if (resultados.SelectedIndex == 0)
                {
                    Estrategicos.Style.Add("display", "none");
                    Institucionales.Style.Add("display", "block");
                }
                estrategicosR.ShowOnPageLoad = false;
            }
            else if (estados == 1)
            {

                mensaje = "Se han vinculado/desvinculado a programa 2024-2028 correctamente";
                //Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
                ScriptManager.RegisterStartupScript(this.upModalEstrategicos, GetType(), "script", "Alerta('" + mensaje + " <br/>',1);", true);

                //MultiView1.ActiveViewIndex = 2;
                //Estrategicos.Style.Add("display", "block");
                //Institucionales.Style.Add("display", "none");
                if (resultados.SelectedIndex == 0)
                {
                    Estrategicos.Style.Add("display", "none");
                    Institucionales.Style.Add("display", "block");
                }
                estrategicosR.ShowOnPageLoad = false;
            }

        }

        protected void btnVincula_Click(object sender, EventArgs e)
        {
            sql = "SELECT * FROM SCHE$SIPLAN20.SPPSVST$PGG2024_2028 WHERE RESPONSABLE = " + Convert.ToInt32(Session["insto"]);
            estado = dao.consulta(sql);
            if (estado == 1)
                tabla = dao.tabla;
            else
            {
                mensaje = dao.mensaje;
                //Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
                ScriptManager.RegisterStartupScript(this.upPanelVincula, GetType(), "script", "Alerta('" + mensaje + " <br/>',2);", true);
            }

            if (tabla.Rows.Count > 1)
            {
                String sql2;
                DataTable resultadosVin = new DataTable();
                int m1, m2;
                m1 = 0;
                m2 = 0;

                /*
            sql = @"SELECT ID_PILAR
                    ,NIVEL_PADRE
                    ,PILARPGG
                    ,OBJETIVOPGG
                    ,NIVEL_HIJO
                    ,OBJETIVO_SECTORIAL
                    ,SPPRES$ORDEN FROM(SELECT
                    RE.SPPRES$ID_RESULTADO ID_PILAR
                    ,RE.SPPRES$NIVEL NIVEL_PADRE
                    ,RE.SPPRES$CODIGO|| ' ' || RE.SPPRES$DESCRIPCION PILARPGG
                    ,RES.SPPRES$ID_RESULTADO OBJETIVOPGG
                    ,RES.SPPRES$NIVEL NIVEL_HIJO
                    ,RES.SPPRES$CODIGO || ' ' || RES.SPPRES$DESCRIPCION OBJETIVO_SECTORIAL
                    ,(SELECT SPPRES$ORDEN FROM SCHE$SIPLAN20.SP20$RESULTADOS_ESTRATEGICOS WHERE  SPPRES$ID_RESULTADO = RES.SPPRES$ID_RESULTADO AND SPPRES$RESTRICTIVA = 'N') SPPRES$ORDEN
                    FROM SCHE$SIPLAN20.SP20$RESULTADOS_ESTRATEGICOS RE
                    INNER JOIN SCHE$SIPLAN20.SP20$RESULTADOS_ESTRATEGICOS RES ON RES.SPPRES$DEPENDE = RE.SPPRES$ID_RESULTADO AND RE.SPPRES$RESTRICTIVA = 'N' AND RES.SPPRES$RESTRICTIVA = 'N' WHERE RE.SPPRES$NIVEL = 1 AND RES.SPPRES$NIVEL = 2 AND RES.SPPRES$RESPONSABLE = -1
                    UNION

                    SELECT
                    RE.SPPRES$ID_RESULTADO ID_PILAR
                    ,RE.SPPRES$NIVEL NIVEL_PADRE
                    ,RE.SPPRES$CODIGO || ' ' || RE.SPPRES$DESCRIPCION PILARPGG
                    ,RES.SPPRES$ID_RESULTADO OBJETIVOPGG
                    ,RES.SPPRES$NIVEL NIVEL_HIJO
                    ,RES.SPPRES$CODIGO || ' ' || RES.SPPRES$DESCRIPCION OBJETIVO_SECTORIAL
                    ,(SELECT SPPRES$ORDEN FROM SCHE$SIPLAN20.SP20$RESULTADOS_ESTRATEGICOS WHERE  SPPRES$ID_RESULTADO = RES.SPPRES$ID_RESULTADO AND SPPRES$RESTRICTIVA = 'N') SPPRES$ORDEN
                    FROM SCHE$SIPLAN20.SP20$RESULTADOS_ESTRATEGICOS RE
                    INNER JOIN SCHE$SIPLAN20.SP20$RESULTADOS_ESTRATEGICOS RES ON RES.SPPRES$DEPENDE = RE.SPPRES$ID_RESULTADO AND RE.SPPRES$RESTRICTIVA = 'N' AND RES.SPPRES$RESTRICTIVA = 'N'  
                    INNER JOIN SCHE$SIPLAN20.SP20$INSTOAPRIORIZA PRI ON PRI.SPINSTO$ID_RESULTADO =  RES.SPPRES$ID_RESULTADO AND PRI.SPINSTO$TIPO_PRIORIZACION = 1 
                    WHERE RE.SPPRES$NIVEL = 1 AND RES.SPPRES$NIVEL = 2 AND PRI.SPINSTO$INSTO = "+ Convert.ToInt32(Session["insto"]) + @" AND RES.SPPRES$RESPONSABLE = 0
                    ) ORDER BY  SPPRES$ORDEN ASC";

            sql2 = "SELECT RE.SPRES$ID_RESULTADO, RE.SPRES$COD_ESTRATEGICO FROM SCHE$SIPLAN20.SP20$RESULTADOS RE INNER JOIN SCHE$SIPLAN20.SP20$RESULTADOS_ESTRATEGICOS RES ON RE.SPRES$COD_ESTRATEGICO = RES.SPPRES$ID_RESULTADO AND RE.SPRES$RESTRICTIVA = 'N' AND RES.SPPRES$RESTRICTIVA = 'N' WHERE RE.SPRES$POM = " + Convert.ToInt32(Session["pom"]) + " AND RE.SPRES$INSTITUCION = " + Convert.ToInt32(Session["insto"]);
            */

                sql = "SELECT * FROM SCHE$SIPLAN20.SPPSVST$PGG2024_2028 WHERE RESPONSABLE = " + Convert.ToInt32(Session["insto"]);
                sql2 = @"SELECT RE.SPRES$ID_RESULTADO
                    ,RE.SPRES$COD_ESTRATEGICO 
                    FROM SCHE$SIPLAN20.SP20$RESULTADOS RE 
                    INNER JOIN SCHE$SIPLAN20.SP20$RESULTADOS_ESTRATEGICOS RES ON RE.SPRES$COD_ESTRATEGICO = RES.SPPRES$ID_RESULTADO AND RE.SPRES$RESTRICTIVA = 'N' AND RES.SPPRES$RESTRICTIVA = 'N' AND RES.SPPRES$NIVEL = 3                    
                    INNER JOIN SCHE$SIPLAN20.SP20$RESULTADOS_ESTRATEGICOS RM ON RM.SPPRES$ID_RESULTADO = RES.SPPRES$DEPENDE AND RES.SPPRES$RESTRICTIVA = 'N' AND RM.SPPRES$RESTRICTIVA = 'N'  AND RM.SPPRES$NIVEL = 2
                    INNER JOIN SCHE$SIPLAN20.SP20$RESULTADOS_ESTRATEGICOS RE ON RM.SPPRES$DEPENDE = RE.SPPRES$ID_RESULTADO AND RM.SPPRES$RESTRICTIVA = 'N' AND RE.SPPRES$RESTRICTIVA = 'N' AND RE.SPPRES$NIVEL = 1
                    INNER JOIN SCHE$SIPLAN20.S20$PERIODO_RESULTADO PE ON PE.SPR$ID_RESULTADO = RE.SPPRES$ID_RESULTADO AND  RE.SPPRES$RESTRICTIVA = 'N' AND PE.SPR$RESTRICTIVA = 'N'
                    INNER JOIN SCHE$SIPLAN20.SP20$PERIODO_GOBIERNO PG ON PG.SPG$ID_PERIODO = PE.SPR$ID_PERIODO AND PE.SPR$RESTRICTIVA = 'N' AND PG.SPG$RESTRICTIVA = 'N'
                    WHERE RE.SPRES$POM = " + Convert.ToInt32(Session["pom"]) + " AND RE.SPRES$INSTITUCION = " + Convert.ToInt32(Session["insto"]) + @" AND PG.SPG$VIGENTE = 1 AND RES.SPPRES$EJE_LINEA = -1 
                    UNION 
                    SELECT RE.SPRES$ID_RESULTADO ,RE.SPRES$COD_ESTRATEGICO FROM SCHE$SIPLAN20.SP20$RESULTADOS RE 
                      INNER JOIN SCHE$SIPLAN20.SP20$RESULTADOS_ESTRATEGICOS RES ON RE.SPRES$COD_ESTRATEGICO = RES.SPPRES$ID_RESULTADO AND RE.SPRES$RESTRICTIVA = 'N' AND RES.SPPRES$RESTRICTIVA = 'N' AND RES.SPPRES$NIVEL = 3  
                      INNER JOIN SCHE$SIPLAN20.SP20$RESULTADOS_ESTRATEGICOS RM ON RM.SPPRES$ID_RESULTADO = RES.SPPRES$DEPENDE AND RES.SPPRES$RESTRICTIVA = 'N' AND RM.SPPRES$RESTRICTIVA = 'N'  AND RM.SPPRES$NIVEL = 2
                      INNER JOIN SCHE$SIPLAN20.SP20$RESULTADOS_ESTRATEGICOS RE ON RES.SPPRES$EJE_LINEA = RE.SPPRES$ID_RESULTADO AND RES.SPPRES$RESTRICTIVA = 'N' AND RE.SPPRES$RESTRICTIVA = 'N' AND RE.SPPRES$NIVEL = 1 AND RES.SPPRES$NIVEL = 3 
                      INNER JOIN SCHE$SIPLAN20.S20$PERIODO_RESULTADO PE ON PE.SPR$ID_RESULTADO = RE.SPPRES$ID_RESULTADO AND  RE.SPPRES$RESTRICTIVA = 'N' AND PE.SPR$RESTRICTIVA = 'N'
                    INNER JOIN SCHE$SIPLAN20.SP20$PERIODO_GOBIERNO PG ON PG.SPG$ID_PERIODO = PE.SPR$ID_PERIODO AND PE.SPR$RESTRICTIVA = 'N' AND PG.SPG$RESTRICTIVA = 'N'
                    WHERE RE.SPRES$POM = " + Convert.ToInt32(Session["pom"]) + " AND RE.SPRES$INSTITUCION = " + Convert.ToInt32(Session["insto"]) + " AND PG.SPG$VIGENTE = 1 AND RES.SPPRES$EJE_LINEA != -1";

                estado = dao.consulta(sql);
                if (estado == 0)
                {
                    mensaje = dao.mensaje;
                    //Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
                    ScriptManager.RegisterStartupScript(this.upPanelVincula, GetType(), "script", "Alerta('" + mensaje + " <br/>',2);", true);
                }
                else
                {
                    tabla = dao.tabla;





                    gvResEstrategicos.DataSource = tabla;
                    gvResEstrategicos.DataBind();
                    gvResEstrategicos.Selection.UnselectAll();
                    gvResEstrategicos.ExpandAll();

                    estado = dao.consulta(sql2);

                    if (estado == 0)
                    {
                        mensaje = dao.mensaje;
                        //Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
                        ScriptManager.RegisterStartupScript(this.upPanelVincula, GetType(), "script", "Alerta('" + mensaje + " <br/>',2);", true);
                    }
                    else
                    {

                        resultadosVin = dao.tabla;

                        if (resultadosVin.Rows.Count > 0)
                        {

                            for (int i = 0; i <= gvResEstrategicos.VisibleRowCount - 1; i++)
                            {
                                m1 = Convert.ToInt32(gvResEstrategicos.GetRowValues(i, "ID_ACCION"));

                                for (int o = 0; o <= resultadosVin.Rows.Count - 1; o++)
                                {
                                    m2 = Convert.ToInt32(resultadosVin.Rows[o]["SPRES$COD_ESTRATEGICO"]);
                                    if (m1 == m2)
                                    {
                                        gvResEstrategicos.Selection.SelectRow(i);
                                    }
                                }

                            }
                            gvResEstrategicos.DataBind();
                            //MultiView1.ActiveViewIndex = 2;
                            //Estrategicos.Style.Add("display", "block");
                            //Institucionales.Style.Add("display", "none");
                            cargaResInsto(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]));
                            estrategicosR.ShowOnPageLoad = true;
                        }
                        else
                        {
                            //MultiView1.ActiveViewIndex = 2;
                            //Estrategicos.Style.Add("display", "block");
                            //Institucionales.Style.Add("display", "none");
                            estrategicosR.ShowOnPageLoad = true;
                        }

                    }




                }



            }


            else
            {
                cargaUnEje();

            }






        }

        protected void cargaUnEje()
        {
            string sql2 = "";
            DataTable resultadosVin = new DataTable();
            int m1, m2;
            m1 = 0;
            m2 = 0;

            sql = @"SELECT 
                          ID_EJE
                           ,EJE
                           FROM
                            (SELECT ID_EJE, EJE FROM SCHE$SIPLAN20.SPPSVST$PGG2024_2028
                            UNION
                            SELECT R.SPPRES$ID_RESULTADO ID_EJE, R.SPPRES$CODIGO || '-' || R.SPPRES$DESCRIPCION EJE FROM SCHE$SIPLAN20.SP20$RESULTADOS_ESTRATEGICOS R
                            INNER JOIN SCHE$SIPLAN20.S20$PERIODO_RESULTADO PR ON PR.SPR$ID_RESULTADO = R.SPPRES$ID_RESULTADO AND PR.SPR$RESTRICTIVA = 'N' AND R.SPPRES$RESTRICTIVA = 'N'
                            INNER JOIN SCHE$SIPLAN20.SP20$PERIODO_GOBIERNO PG ON PG.SPG$ID_PERIODO = PR.SPR$ID_PERIODO AND PG.SPG$RESTRICTIVA = 'N' AND PR.SPR$RESTRICTIVA = 'N' AND PG.SPG$VIGENTE = 1
                        )
                    GROUP BY
                    ID_EJE
                    ,EJE
                    ORDER BY ID_EJE";

            sql2 = @"SELECT RE.SPRES$ID_RESULTADO
                    ,RE.SPRES$COD_ESTRATEGICO 
                    FROM SCHE$SIPLAN20.SP20$RESULTADOS RE 
                    INNER JOIN SCHE$SIPLAN20.SP20$RESULTADOS_ESTRATEGICOS RES ON RE.SPRES$COD_ESTRATEGICO = RES.SPPRES$ID_RESULTADO AND RE.SPRES$RESTRICTIVA = 'N' AND RES.SPPRES$RESTRICTIVA = 'N' AND RES.SPPRES$NIVEL = 1         
                    INNER JOIN SCHE$SIPLAN20.S20$PERIODO_RESULTADO PE ON PE.SPR$ID_RESULTADO = RES.SPPRES$ID_RESULTADO AND  RES.SPPRES$RESTRICTIVA = 'N' AND PE.SPR$RESTRICTIVA = 'N'
                    INNER JOIN SCHE$SIPLAN20.SP20$PERIODO_GOBIERNO PG ON PG.SPG$ID_PERIODO = PE.SPR$ID_PERIODO AND PE.SPR$RESTRICTIVA = 'N' AND PG.SPG$RESTRICTIVA = 'N'
                    WHERE RE.SPRES$POM = " + Convert.ToInt32(Session["pom"]) + " AND RE.SPRES$INSTITUCION = " + Convert.ToInt32(Session["insto"]) + @" AND PG.SPG$VIGENTE = 1 AND RES.SPPRES$EJE_LINEA = -1";

            estado = dao.consulta(sql);
            if (estado == 0)
            {
                mensaje = dao.mensaje;
                //Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
                ScriptManager.RegisterStartupScript(this.upPanelVincula, GetType(), "script", "Alerta('" + mensaje + " <br/>',2);", true);
            }
            else
            {
                tabla = dao.tabla;
                gvUnEje.DataSource = tabla;
                gvUnEje.DataBind();
                gvUnEje.Selection.UnselectAll();

                estado = dao.consulta(sql2);

                if (estado == 0)
                {
                    mensaje = dao.mensaje;
                    //Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
                    ScriptManager.RegisterStartupScript(this.upPanelVincula, GetType(), "script", "Alerta('" + mensaje + " <br/>',2);", true);
                }
                else
                {

                    resultadosVin = dao.tabla;

                    if (resultadosVin.Rows.Count > 0)
                    {

                        for (int i = 0; i <= gvUnEje.VisibleRowCount - 1; i++)
                        {
                            m1 = Convert.ToInt32(gvUnEje.GetRowValues(i, "ID_EJE"));

                            for (int o = 0; o <= resultadosVin.Rows.Count - 1; o++)
                            {
                                m2 = Convert.ToInt32(resultadosVin.Rows[o]["SPRES$COD_ESTRATEGICO"]);
                                if (m1 == m2)
                                {
                                    gvUnEje.Selection.SelectRow(i);
                                }
                            }

                        }
                        gvUnEje.DataBind();
                        //MultiView1.ActiveViewIndex = 2;
                        //Estrategicos.Style.Add("display", "block");
                        //Institucionales.Style.Add("display", "none");
                        cargaResInsto(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]));
                        popUnEje.ShowOnPageLoad = true;
                    }
                    else
                    {
                        //MultiView1.ActiveViewIndex = 2;
                        //Estrategicos.Style.Add("display", "block");
                        //Institucionales.Style.Add("display", "none");
                        popUnEje.ShowOnPageLoad = true;
                    }

                }


            }


        }
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            estrategicosR.ShowOnPageLoad = false;
            MultiView1.ActiveViewIndex = 2;
            Estrategicos.Style.Add("display", "none");
            Institucionales.Style.Add("display", "block");

        }

        protected void btnEncabezado_Click(object sender, EventArgs e)
        {
            MultiView1.ActiveViewIndex = 0;
        }

        protected void btnCancelaR_Click1(object sender, EventArgs e)
        {
            MultiView1.ActiveViewIndex = 0;
        }

        protected void btnPrograma_Click(object sender, EventArgs e)
        {
            if (txtCodPrograma.Text == "")
            {
                mensaje = "El codigo del programa presupuestario es necesario";
                Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
                txtCodPrograma.Focus();
            }
            else if (txtPrograma.Text == "")
            {
                mensaje = "El nombre del programa presupuestario es necesario";
                Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
                txtPrograma.Focus();
            }

            else if (selectProgra.SelectedIndex == -1)
            {
                mensaje = "Debe seleccionar el tipo de programa que esta ingresando";
                Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
                cbPrograma.Focus();
            }

            else if (selectProgra.SelectedIndex == 1)
            {
                if (cbPrograma.Text == "")
                {
                    mensaje = "Esta ingresando un sub programa debe seleccionar el programa presupuestario del que el subprograma depende";
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
                    cbPrograma.Focus();
                }

                else
                    graba_programa(1, Convert.ToDouble(txtCodPrograma.Text), txtPrograma.Text, Convert.ToDouble(cbPrograma.Value), Convert.ToInt32(cbComboProgrma.Value));
            }

            else if (selectProgra.SelectedIndex == 0)
                graba_programa(0, Convert.ToDouble(txtCodPrograma.Text), txtPrograma.Text, 0, Convert.ToInt32(cbComboProgrma.Value));

        }

        protected void graba_programa(int tipo, Double programa, string descripcion, Double sub, int administrativo)
        {
            String codigo;
            if (tipo == 0)
                sql = "INSERT INTO SCHE$SIPLAN20.SP20$PROGRAMA_PRESUPUESTARIO (SPPRO$ID_PROGRAMA_PRESUPUESTO, SPPRO$DESCRIPCION, SPPRO$ID_POM, SPPRO$ID_INSTITUCION, SPPRO$FECHA_INSERT, SPPRO$PROPIETARIO, SPPRO$ES_ADMINISTRATIVO) VALUES (" + programa + ", '" + descripcion + "', " + Convert.ToInt32(Session["pom"]) + ", " + Convert.ToInt32(Session["insto"]) + ", 'INSERT = '||TO_CHAR(SYSDATE,'DD/MM/YYYY HH24:MI')||' '||'" + Session["USUARIO"].ToString() + "', '" + Session["USUARIO"].ToString() + "', "+administrativo+" )";
            else if (tipo == 1)
                sql = "INSERT INTO SCHE$SIPLAN20.SP20$PROGRAMA_PRESUPUESTARIO (SPPRO$ID_PROGRAMA_PRESUPUESTO, SPPRO$ID_PROGRAMA_DEPENDE, SPPRO$DESCRIPCION, SPPRO$ID_POM, SPPRO$ID_INSTITUCION, SPPRO$FECHA_INSERT, SPPRO$PROPIETARIO,SPPRO$ES_ADMINISTRATIVO) VALUES (" + programa + ", " + sub + ", '" + descripcion + "', " + Convert.ToInt32(Session["pom"]) + ", " + Convert.ToInt32(Session["insto"]) + ", 'INSERT = '||TO_CHAR(SYSDATE,'DD/MM/YYYY HH24:MI')||' '||'" + Session["USUARIO"].ToString() + "', '" + Session["USUARIO"].ToString() + "',"+administrativo+" )";
            estado = dao.comando(sql);
            if (estado == 0)
            {
                mensaje = dao.mensaje;
                codigo = mensaje.Substring(0, 9);
                if (codigo == "ORA-00001")
                {
                    if (tipo == 0)
                        sql = "UPDATE SCHE$SIPLAN20.SP20$PROGRAMA_PRESUPUESTARIO SET SPPRO$RESTRICTIVA = 'N', SPPRO$ID_PROGRAMA_DEPENDE = NULL, SPPRO$ES_ADMINISTRATIVO = "+administrativo+" WHERE SPPRO$ID_PROGRAMA_PRESUPUESTO = " + programa + " AND SPPRO$ID_POM = " + Convert.ToInt32(Session["pom"]) + " AND SPPRO$ID_INSTITUCION = " + Convert.ToInt32(Session["insto"]);
                    else if (tipo == 1)
                        sql = "UPDATE SCHE$SIPLAN20.SP20$PROGRAMA_PRESUPUESTARIO SET SPPRO$RESTRICTIVA = 'N', SPPRO$ID_PROGRAMA_DEPENDE =" + cbPrograma.Value + ", SPPRO$ES_ADMINISTRATIVO = "+administrativo+" WHERE SPPRO$ID_PROGRAMA_PRESUPUESTO = " + programa + " AND SPPRO$ID_POM = " + Convert.ToInt32(Session["pom"]) + " AND SPPRO$ID_INSTITUCION = " + Convert.ToInt32(Session["insto"]);

                    estado = dao.comando(sql);
                    if (estado == 0)
                    {
                        mensaje = dao.mensaje;
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
                    }
                    else
                    {
                        mensaje = "Ya habia ingresado un programa presupuestario con el codigo " + programa + " el mismo ha sido activado, utilice el botón editar de la tabla para modificar la información del programa";
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
                        txtCodPrograma.Text = "";
                        txtPrograma.Text = "";
                        selectProgra.SelectedIndex = -1;
                        PanelPrograma.Style.Add("display", "none");
                        cbPrograma.SelectedIndex = -1;
                        cargaPrograma(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]));
                        //btnProgra_Click(new object(), new EventArgs());
                    }
                }
                else
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);

            }
            else
            {
                txtCodPrograma.Text = "";
                txtPrograma.Text = "";
                selectProgra.SelectedIndex = -1;
                PanelPrograma.Style.Add("display", "none");
                mensaje = "El programa presupuestario ha sido grabado correctamente";
                Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',1);", true);
                llenaPrograma(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]));
                cargaPrograma(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]));

            }

        }
        protected void btnProgra_Click(object sender, EventArgs e)
        {

            if (gvPOMInsto.FocusedRowIndex == -1)
            {
                mensaje = "Debe seleccionar primero el encabezado de su Programación Multianual";
                Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
                MultiView1.ActiveViewIndex = 0;
            }
            else
            {
                Session["pom"] = Convert.ToInt32(gvPOMInsto.GetRowValues(gvPOMInsto.FocusedRowIndex, "SPPO$ID_POM").ToString());
                Session["insto"] = Convert.ToInt32(gvPOMInsto.GetRowValues(gvPOMInsto.FocusedRowIndex, "SPPO$ID_INSTITUCION").ToString());
                Session["abierto"] = Convert.ToInt32(gvPOMInsto.GetRowValues(gvPOMInsto.FocusedRowIndex, "SPP$ABIERTO").ToString());
                //Session["nombre"] = Convert.ToInt32(gvPOMInsto.GetRowValues(gvPOMInsto.FocusedRowIndex, "NOMBRE").ToString());
                gvEstrategicos.DataSource = null;
                gvResEstrategicos.DataSource = null;
                llenaPrograma(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]));
                cargaPrograma(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]));
                if (Convert.ToInt32(Session["abierto"]) == 1)
                    btnPrograma.Enabled = false;
                else
                    btnPrograma.Enabled = true;
                MultiView1.ActiveViewIndex = 3;
            }


        }

        //protected void cargaPrograma(int pom, int insto)
        //{
        //    int orden = -1;
        //    DataTable poms = new DataTable();
        //    DataTable programas = new DataTable();

        //    sql = "SELECT ROWNUM PROGRAMA, P.SPPRO$ID_PROGRAMA_PRESUPUESTO, P.SPPRO$DESCRIPCION, P.SPPRO$ID_PROGRAMA_DEPENDE, ";
        //    sql = sql + "P.SPPRO$ID_POM, P.SPPRO$ID_INSTITUCION, P.SPPRO$PROPIETARIO FROM SCHE$SIPLAN20.SP20$PROGRAMA_PRESUPUESTARIO P WHERE P.SPPRO$RESTRICTIVA = 'N' AND P.SPPRO$ID_POM = " + pom + " AND P.SPPRO$ID_INSTITUCION = " + insto + " ORDER BY P.SPPRO$ID_PROGRAMA_PRESUPUESTO ASC";
        //    estado = dao.consulta(sql);
        //    if (estado == 0)
        //    {
        //        mensaje = dao.mensaje;
        //        Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
        //    }
        //    else
        //    {
        //        tabla = dao.tabla;
        //        //ordenes = 1;
        //        if (tabla.Rows.Count > 0)
        //        //if(ordenes == 0)
        //        {
        //            Session["presupesto"] = tabla;
        //            gvPrograPresupuestario.DataSource = tabla;
        //            GridViewDataComboBoxColumn column = (gvPrograPresupuestario.Columns["SPPRO$ID_PROGRAMA_DEPENDE"] as GridViewDataComboBoxColumn);
        //            sql = "SELECT PE.SPPRO$ID_PROGRAMA_PRESUPUESTO, CASE WHEN ROUND(PE.SPPRO$ID_PROGRAMA_PRESUPUESTO,0) = 0 THEN '0'||PE.SPPRO$ID_PROGRAMA_PRESUPUESTO|| '-' || PE.SPPRO$DESCRIPCION ELSE PE.SPPRO$ID_PROGRAMA_PRESUPUESTO|| '-' || PE.SPPRO$DESCRIPCION END AS PROGRAMA FROM SCHE$SIPLAN20.SP20$PROGRAMA_PRESUPUESTARIO PE WHERE PE.SPPRO$ID_POM = " + pom + " AND PE.SPPRO$ID_INSTITUCION = " + insto + " AND PE.SPPRO$ID_PROGRAMA_DEPENDE IS NULL AND PE.SPPRO$RESTRICTIVA = 'N'";
        //            estado = dao.consulta(sql);
        //            if (estado == 0)
        //            {
        //                mensaje = dao.mensaje;
        //                Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
        //            }
        //            else

        //            {

        //                tabla = dao.tabla;
        //                column.PropertiesComboBox.DataSource = tabla;
        //                column.PropertiesComboBox.ValueField = "SPPRO$ID_PROGRAMA_PRESUPUESTO";
        //                column.PropertiesComboBox.TextField = "PROGRAMA";
        //                gvPrograPresupuestario.DataBind();
        //                //MultiView1.ActiveViewIndex = 3;
        //            }
        //        }
        //        else
        //        {
        //            sql = "SELECT P.SPP$ORDEN FROM SCHE$SIPLAN20.SP20$PERIODO P INNER JOIN SCHE$SIPLAN20.SP20$POM PO ON P.SPP$ID_PERIODO = PO.SPPO$ID_PERIODO AND P.SPP$RESTRICTIVA = 'N' AND PO.SPPO$RESTRICTIVA = 'N' WHERE PO.SPPO$ID_POM = " + pom + " AND PO.SPPO$ID_INSTITUCION = " + insto;
        //            estado = dao.consulta(sql);
        //            if (estado == 1)
        //            {
        //                tabla = dao.tabla;
        //                if (Convert.ToInt32(tabla.Rows[0]["SPP$ORDEN"]) != 1)
        //                {
        //                    orden = Convert.ToInt32(tabla.Rows[0]["SPP$ORDEN"]) - 1;
        //                    sql = "SELECT PO.SPPO$ID_POM, PO.SPPO$ID_INSTITUCION, PO.SPPO$ID_PERIODO FROM SCHE$SIPLAN20.SP20$PERIODO P INNER JOIN SCHE$SIPLAN20.SP20$POM PO ON P.SPP$ID_PERIODO = PO.SPPO$ID_PERIODO AND P.SPP$RESTRICTIVA = 'N' AND PO.SPPO$RESTRICTIVA = 'N' WHERE PO.SPPO$ID_INSTITUCION= " + insto + " AND P.SPP$ORDEN =" + orden;
        //                    estado = dao.consulta(sql);
        //                    if (estado == 1)
        //                    {
        //                        poms = dao.tabla;
        //                        if (poms.Rows.Count > 0)
        //                        {
        //                            sql = "SELECT * FROM SCHE$SIPLAN20.SP20$PROGRAMA_PRESUPUESTARIO WHERE SPPRO$ID_POM = " + poms.Rows[0]["SPPO$ID_POM"] + " AND SPPRO$ID_INSTITUCION = " + poms.Rows[0]["SPPO$ID_INSTITUCION"] + " AND SPPRO$RESTRICTIVA = 'N'";
        //                            estado = dao.consulta(sql);
        //                            if (estado == 1)
        //                            {
        //                                programas = dao.tabla;
        //                                for (int i = 0; i < programas.Rows.Count; i++)
        //                                {
        //                                    sql = "INSERT INTO SCHE$SIPLAN20.SP20$PROGRAMA_PRESUPUESTARIO (SPPRO$ID_PROGRAMA_PRESUPUESTO, SPPRO$ID_PROGRAMA_DEPENDE, SPPRO$DESCRIPCION, SPPRO$ID_POM, SPPRO$ID_INSTITUCION, SPPRO$FECHA_INSERT, SPPRO$PROPIETARIO) VALUES (" + programas.Rows[i]["SPPRO$ID_PROGRAMA_PRESUPUESTO"];
        //                                    if (programas.Rows[i]["SPPRO$ID_PROGRAMA_DEPENDE"] == DBNull.Value)
        //                                        sql = sql + ",NULL";
        //                                    else
        //                                        sql = sql + "," + programas.Rows[i]["SPPRO$ID_PROGRAMA_DEPENDE"];
        //                                    sql = sql + ",'" + programas.Rows[i]["SPPRO$DESCRIPCION"] + "'," + pom + "," + insto + ",'INSERT = ' || TO_CHAR(SYSDATE, 'DD/MM/YYYY HH24:MI') || ' ' || '" + Session["USUARIO"].ToString() + "','" + Session["USUARIO"].ToString() + "')";
        //                                    estado = dao.comando(sql);
        //                                    if (estado == 0)
        //                                        break;
        //                                }

        //                                if (estado == 1)
        //                                {
        //                                    sql = "SELECT ROWNUM PROGRAMA, P.SPPRO$ID_PROGRAMA_PRESUPUESTO, P.SPPRO$DESCRIPCION, P.SPPRO$ID_PROGRAMA_DEPENDE, ";
        //                                    sql = sql + "P.SPPRO$ID_POM, P.SPPRO$ID_INSTITUCION, P.SPPRO$PROPIETARIO FROM SCHE$SIPLAN20.SP20$PROGRAMA_PRESUPUESTARIO P WHERE P.SPPRO$RESTRICTIVA = 'N' AND P.SPPRO$ID_POM = " + pom + " AND P.SPPRO$ID_INSTITUCION = " + insto + " ORDER BY P.SPPRO$ID_PROGRAMA_PRESUPUESTO ASC";
        //                                    estado = dao.consulta(sql);
        //                                    if (estado == 0)
        //                                    {
        //                                        mensaje = dao.mensaje;
        //                                        Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
        //                                    }
        //                                    else
        //                                    {
        //                                        tabla = dao.tabla;
        //                                        if (tabla.Rows.Count > 0)
        //                                        {
        //                                            if (tabla.Rows.Count > 0)
        //                                            {
        //                                                Session["presupesto"] = tabla;
        //                                                gvPrograPresupuestario.DataSource = tabla;
        //                                                GridViewDataComboBoxColumn column = (gvPrograPresupuestario.Columns["SPPRO$ID_PROGRAMA_DEPENDE"] as GridViewDataComboBoxColumn);
        //                                                sql = "SELECT PE.SPPRO$ID_PROGRAMA_PRESUPUESTO, CASE WHEN ROUND(PE.SPPRO$ID_PROGRAMA_PRESUPUESTO,0) = 0 THEN '0'||PE.SPPRO$ID_PROGRAMA_PRESUPUESTO|| '-' || PE.SPPRO$DESCRIPCION ELSE PE.SPPRO$ID_PROGRAMA_PRESUPUESTO|| '-' || PE.SPPRO$DESCRIPCION END AS PROGRAMA FROM SCHE$SIPLAN20.SP20$PROGRAMA_PRESUPUESTARIO PE WHERE PE.SPPRO$ID_POM = " + pom + " AND PE.SPPRO$ID_INSTITUCION = " + insto + " AND PE.SPPRO$ID_PROGRAMA_DEPENDE IS NULL AND PE.SPPRO$RESTRICTIVA = 'N'";
        //                                                estado = dao.consulta(sql);
        //                                                if (estado == 0)
        //                                                {
        //                                                    mensaje = dao.mensaje;
        //                                                    Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
        //                                                }
        //                                                else

        //                                                {

        //                                                    tabla = dao.tabla;
        //                                                    column.PropertiesComboBox.DataSource = tabla;
        //                                                    column.PropertiesComboBox.ValueField = "SPPRO$ID_PROGRAMA_PRESUPUESTO";
        //                                                    column.PropertiesComboBox.TextField = "PROGRAMA";
        //                                                    gvPrograPresupuestario.DataBind();
        //                                                    //MultiView1.ActiveViewIndex = 3;
        //                                                }
        //                                            }
        //                                        }

        //                                    }

        //                                }
        //                            }
        //                        }

        //                    }

        //                }

        //            }
        //        }


        //    }
        //}


        protected void cargaPrograma(int pom, int insto)
        {
            int resulta = 0;
            resulta = busca_programa(pom, insto);
            if (resulta == 0)
            {

                Session["CONFIRMA"] = 2; //variable de migración de programas presupustarios
                Confirmamigracion.Text = "No se han encontrado programas presupuestarios para el periodo de esta programación, ¿Necesita trasladar la información de los programas presupuestarios del periodo anterior a la presente programación?";
                Session["presupesto"] = null;
                gvPrograPresupuestario.DataSource = null;
                Confirma.ShowOnPageLoad = true;


            }
            else
                carga_programa_presupuestario(pom, insto);


        }

        protected void carga_programa_presupuestario(int pom, int insto)
        {
            sql = @"SELECT 
                    PROGRAMA
                    ,TIPO
                    ,SPPRO$ID_PROGRAMA_PRESUPUESTO
                    ,SPPRO$ID_PROGRAMA_DEPENDE
                    ,ID_SUBPROGRAMA 
                    ,SPPRO$ES_ADMINISTRATIVO
                    ,CASE WHEN TIPO = 0 THEN SPPRO$DESCRIPCION ELSE ' ' END AS SPPRO$DESCRIPCION
                    ,CASE WHEN TIPO = 1 AND SPPRO$ID_PROGRAMA_PRESUPUESTO = SPPRO$ID_PROGRAMA_DEPENDE THEN SPPRO$DESCRIPCION ELSE ' ' END AS SUBPROGRAMA_PRESUPUESTARIO
                    ,SPPRO$ID_POM   
                    ,SPPRO$ID_INSTITUCION
                    ,SPPRO$PROPIETARIO 
                    FROM 
                    (SELECT
                        PROGRAMA
                        ,TIPO
                        ,SPPRO$ID_PROGRAMA_PRESUPUESTO
                        ,SPPRO$ID_PROGRAMA_DEPENDE
                        ,ID_SUBPROGRAMA
                        ,SPPRO$ES_ADMINISTRATIVO
                        ,SPPRO$DESCRIPCION
                        ,SPPRO$ID_POM
                        ,SPPRO$ID_INSTITUCION
                        ,SPPRO$PROPIETARIO 

                        FROM
                        (SELECT ROWNUM PROGRAMA
                            ,CASE WHEN P.SPPRO$ID_PROGRAMA_DEPENDE IS NULL THEN 0 ELSE 1 END AS TIPO
                            ,CASE WHEN P.SPPRO$ID_PROGRAMA_DEPENDE IS NULL THEN P.SPPRO$ID_PROGRAMA_PRESUPUESTO ELSE P.SPPRO$ID_PROGRAMA_DEPENDE END AS SPPRO$ID_PROGRAMA_PRESUPUESTO
                            ,CASE WHEN P.SPPRO$ID_PROGRAMA_DEPENDE IS NOT NULL THEN P.SPPRO$ID_PROGRAMA_PRESUPUESTO END AS ID_SUBPROGRAMA
                            ,P.SPPRO$ES_ADMINISTRATIVO
                            ,P.SPPRO$DESCRIPCION
                            ,P.SPPRO$ID_PROGRAMA_DEPENDE
                            ,P.SPPRO$ID_POM
                            ,P.SPPRO$ID_INSTITUCION
                            ,P.SPPRO$PROPIETARIO 
                            FROM SCHE$SIPLAN20.SP20$PROGRAMA_PRESUPUESTARIO P
                            WHERE 
                            P.SPPRO$RESTRICTIVA = 'N' 
                            AND P.SPPRO$ID_POM = " + pom + @" 
                            AND P.SPPRO$ID_INSTITUCION = " + insto + @" 
                            ))
                            ORDER BY SPPRO$ID_PROGRAMA_PRESUPUESTO, TIPO, ID_SUBPROGRAMA  ASC";

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
                    Session["presupesto"] = tabla;
                    gvPrograPresupuestario.DataSource = tabla;
                    //gvPrograPresupuestario.DataBind();
                    GridViewDataComboBoxColumn column = (gvPrograPresupuestario.Columns["SPPRO$ID_PROGRAMA_PRESUPUESTO"] as GridViewDataComboBoxColumn);
                    sql = "SELECT PE.SPPRO$ID_PROGRAMA_PRESUPUESTO, PE.SPPRO$ID_PROGRAMA_PRESUPUESTO PROGRAMA FROM SCHE$SIPLAN20.SP20$PROGRAMA_PRESUPUESTARIO PE WHERE PE.SPPRO$ID_POM = " + pom + " AND PE.SPPRO$ID_INSTITUCION = " + insto + " AND PE.SPPRO$ID_PROGRAMA_DEPENDE IS NULL AND PE.SPPRO$RESTRICTIVA = 'N'";
                    estado = dao.consulta(sql);
                    if (estado == 0)
                    {
                        mensaje = dao.mensaje;
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
                    }

                    else
                    {
                        tabla = dao.tabla;
                        column.PropertiesComboBox.DataSource = tabla;
                        column.PropertiesComboBox.ValueField = "SPPRO$ID_PROGRAMA_PRESUPUESTO";
                        column.PropertiesComboBox.TextField = "PROGRAMA";
                        gvPrograPresupuestario.DataBind();
                        //MultiView1.ActiveViewIndex = 3;
                    }
                }
            }

        }

        protected int busca_programa(int pom, int insto)
        {
            int resultado = 0;
            sql = "SELECT * FROM SCHE$SIPLAN20.SP20$PROGRAMA_PRESUPUESTARIO WHERE SPPRO$ID_POM = " + pom + " AND SPPRO$ID_INSTITUCION = " + insto + " AND SPPRO$RESTRICTIVA = 'N'";
            estado = dao.consulta(sql);
            if (estado == 1)
                tabla = dao.tabla;
            if (tabla.Rows.Count > 0)
                resultado = 1;
            else
                resultado = 0;
            return resultado;
        }
        protected void llenaPrograma(int pom, int insto)
        {
            sql = "SELECT P.SPPRO$ID_PROGRAMA_PRESUPUESTO, P.SPPRO$ID_PROGRAMA_PRESUPUESTO||'-'||P.SPPRO$DESCRIPCION PROGRAMA FROM SCHE$SIPLAN20.SP20$PROGRAMA_PRESUPUESTARIO P WHERE P.SPPRO$ID_POM = " + pom + " AND P.SPPRO$ID_INSTITUCION =" + insto + "  AND P.SPPRO$RESTRICTIVA = 'N' AND  P.SPPRO$ID_PROGRAMA_DEPENDE IS NULL ORDER BY P.SPPRO$ID_PROGRAMA_PRESUPUESTO ASC";
            estado = dao.consulta(sql);
            if (estado == 0)
            {
                mensaje = dao.mensaje;
                Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
            }
            else
            {
                tabla = dao.tabla;
                cbPrograma.Items.Clear();
                cbPrograma.DataSource = tabla;
                cbPrograma.ValueField = "SPPRO$ID_PROGRAMA_PRESUPUESTO";
                cbPrograma.TextField = "PROGRAMA";
                cbPrograma.DataBind();
            }
        }
        protected void btnPrograCancelar_Click(object sender, EventArgs e)
        {
            MultiView1.ActiveViewIndex = 0;
        }

        protected void gvPrograPresupuestario_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            String sql, descripcion;
            Double sub, depende, programa;
            int naturaleza = 0;
            DataTable programas = new DataTable();
            DataTable subsos = new DataTable();

            programas = (DataTable)Session["presupesto"];

            for (int i = 0; i <= programas.Rows.Count - 1; i++)
            {
                if (programas.Rows[i]["PROGRAMA"].ToString() == e.Keys["PROGRAMA"].ToString())
                {
                    //if ((Session["USUARIO"].ToString() == programas.Rows[i]["SPPRO$PROPIETARIO"].ToString())|| Session["ROL"].ToString() == "ADMIN")
                    if (Session["ROL"].ToString() != "ENTIDAD")
                    {
                        descripcion = e.NewValues["SPPRO$DESCRIPCION"].ToString();

                        if (e.NewValues["SPPRO$ES_ADMINISTRATIVO"] != null)
                        {
                            naturaleza = Convert.ToInt32(e.NewValues["SPPRO$ES_ADMINISTRATIVO"].ToString());
                        }
                        else
                        {
                            naturaleza = Convert.ToInt32(e.OldValues["SPPRO$ES_ADMINISTRATIVO"]?.ToString());
                        }


                        if (naturaleza == 1)
                        {
                            sql = "SELECT SM.SPSM$ID_SUB FROM SCHE$SIPLAN20.SP20$SUB_MUNOS SM INNER JOIN SCHE$SIPLAN20.SP20$SUB_PRODUCTO S ON SM.SPSM$ID_SUB = S.SPPSUB$ID_SUBPRODUCTO AND SM.SPSM$RESTRICTIVA = 'N' AND S.SPPSUB$RESTRICTIVA = 'N' INNER JOIN SCHE$SIPLAN20.SP20$PRODUCTO P ON P.SPPRO$ID_PRODUCTO = S.SPPSUB$ID_PRODUCTO AND P.SPPRO$RESTRICTIVA = 'N' AND S.SPPSUB$RESTRICTIVA = 'N' WHERE P.SPPRO$PRESUPUESTO =  " + Convert.ToDouble(programas.Rows[i]["SPPRO$ID_PROGRAMA_PRESUPUESTO"].ToString()) + " AND P.SPPRO$POM = " + Convert.ToInt32(Session["pom"]) + " AND P.SPPRO$INSTO = " + Convert.ToInt32(Session["insto"]) + " GROUP BY SM.SPSM$ID_SUB";
                            estado = dao.consulta(sql);
                            if (estado == 1)
                            {
                                subsos.Clear();
                                subsos = dao.tabla;
                                if (subsos.Rows.Count > 0)
                                {
                                    for (int c = 0; c < subsos.Rows.Count; c++)
                                    {
                                        sql = "UPDATE SCHE$SIPLAN20.SP20$SUB_MUNOS SET SPSM$RESTRICTIVA = 'S', SPSM$FECHA_ELIMINA = 'BORRA = '||TO_CHAR(SYSDATE,'DD/MM/YYYY HH24:MI')||' '||'" + Session["USUARIO"].ToString() + "' WHERE SPSM$ID_SUB = " + subsos.Rows[c]["SPSM$ID_SUB"];
                                        estado = dao.comando(sql);
                                    }
                                }
                            }
                        }


                        if (programas.Rows[i]["SPPRO$ID_PROGRAMA_DEPENDE"] == DBNull.Value || Convert.ToDouble(programas.Rows[i]["SPPRO$ID_PROGRAMA_DEPENDE"]) == 0)
                        {
                            sql = "UPDATE SCHE$SIPLAN20.SP20$PROGRAMA_PRESUPUESTARIO SET SPPRO$DESCRIPCION = '" + descripcion + "', SPPRO$ID_PROGRAMA_DEPENDE = NULL, SPPRO$FECHA_UPDATE = 'UPDATE = '||TO_CHAR(SYSDATE,'DD/MM/YYYY HH24:MI')||' '||'" + Session["USUARIO"].ToString() + "', SPPRO$ES_ADMINISTRATIVO = "+naturaleza+"  WHERE SPPRO$RESTRICTIVA = 'N' AND SPPRO$ID_PROGRAMA_PRESUPUESTO = " + Convert.ToDouble(programas.Rows[i]["SPPRO$ID_PROGRAMA_PRESUPUESTO"].ToString()) + " AND SPPRO$ID_POM = " + Convert.ToInt32(Session["pom"]) + " AND SPPRO$ID_INSTITUCION = " + Convert.ToInt32(Session["insto"]);

                        }

                        else
                        {

                            depende = Convert.ToDouble(e.NewValues["SPPRO$ID_PROGRAMA_DEPENDE"]);
                            programa = Convert.ToDouble(programas.Rows[i]["SPPRO$ID_PROGRAMA_PRESUPUESTO"]);

                            if (depende == programa)
                            //if (Convert.ToInt32(e.NewValues["SPPRO$ID_PROGRAMA_DEPENDE"]) == Convert.ToInt32(programas.Rows[i]["PROGRAMA"]))
                            {
                                mensaje = "No puede vincular un programa presupuestario consigo mismo";
                                gvPrograPresupuestario.JSProperties["cpError"] = "Ocurrio un error: " + mensaje;
                                e.Cancel = true;
                                gvPrograPresupuestario.CancelEdit();
                                cargaPrograma(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]));
                                break;
                            }
                            else
                            {
                                
                                descripcion = e.NewValues["SUBPROGRAMA_PRESUPUESTARIO"].ToString();
                                sub = Convert.ToDouble(e.NewValues["ID_SUBPROGRAMA"]);

                                sql = "UPDATE SCHE$SIPLAN20.SP20$PROGRAMA_PRESUPUESTARIO SET SPPRO$DESCRIPCION = '" + descripcion + "', SPPRO$ID_PROGRAMA_DEPENDE = " + Convert.ToDouble(programas.Rows[i]["SPPRO$ID_PROGRAMA_PRESUPUESTO"].ToString()) + ", SPPRO$FECHA_UPDATE = 'UPDATE = '||TO_CHAR(SYSDATE,'DD/MM/YYYY HH24:MI')||' '||'" + Session["USUARIO"].ToString() + "', SPPRO$ES_ADMINISTRATIVO = "+naturaleza+" WHERE SPPRO$RESTRICTIVA = 'N' AND SPPRO$ID_PROGRAMA_PRESUPUESTO = " + sub + " AND SPPRO$ID_POM = " + Convert.ToInt32(Session["pom"]) + " AND SPPRO$ID_INSTITUCION = " + Convert.ToInt32(Session["insto"]);
                            }


                        }
                        estado = dao.comando(sql);
                        if (estado == 0)
                        {
                            mensaje = dao.mensaje;
                            gvPrograPresupuestario.JSProperties["cpError"] = "Ocurrio un error: " + mensaje;
                            e.Cancel = true;
                            gvPrograPresupuestario.CancelEdit();
                            cargaPrograma(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]));
                            break;
                        }
                        else
                        {
                            mensaje = "Registro editado correctamente";
                            gvPrograPresupuestario.JSProperties["cpError"] = "Operación correcta: " + mensaje;
                            e.Cancel = true;
                            gvPrograPresupuestario.CancelEdit();
                            cargaPrograma(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]));
                            break;
                        }


                    }
                    else
                    {
                        gvPrograPresupuestario.JSProperties["cpError"] = "Su perfil de usuario no esta autorizado para editar este registro";
                        e.Cancel = true;
                        gvPrograPresupuestario.CancelEdit();
                        cargaPrograma(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]));
                        break;
                    }
                }
            }


        }

        protected void gvPrograPresupuestario_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            DataTable tempo = new DataTable();
            DataTable tempo1 = new DataTable();
            tempo = (DataTable)Session["presupesto"];
            int resultado = -1;

            for (int i = 0; i <= tempo.Rows.Count - 1; i++)
            {

                if (tempo.Rows[i]["PROGRAMA"].ToString() == e.Keys["PROGRAMA"].ToString())
                {
                    //if ((tempo.Rows[i]["SPPRO$PROPIETARIO"].ToString() == Session["Usuario"].ToString()) || Session["ROL"].ToString() == "ADMIN")
                    if (Session["ROL"].ToString() != "ENTIDAD")
                    {
                        Double codigo = Convert.ToDouble(e.Values["SPPRO$ID_PROGRAMA_PRESUPUESTO"]);

                        if (tempo.Rows[i]["ID_SUBPROGRAMA"] == DBNull.Value)
                        //if (e.Values["SPPRO$ID_PROGRAMA_DEPENDE"] == DBNull.Value)
                        {

                            sql = "SELECT * FROM SCHE$SIPLAN20.SP20$PROGRAMA_PRESUPUESTARIO WHERE SPPRO$ID_PROGRAMA_DEPENDE = " + codigo + " AND SPPRO$ID_POM = " + Convert.ToInt32(Session["pom"]) + " AND SPPRO$ID_INSTITUCION = " + Convert.ToInt32(Session["insto"]) + " AND SPPRO$RESTRICTIVA = 'N'";
                            estado = dao.consulta(sql);
                            if (estado == 0)
                            {
                                mensaje = dao.mensaje;
                                gvPrograPresupuestario.JSProperties["cpError"] = "Ocurrio un error: " + mensaje;
                                e.Cancel = true;
                                cargaPrograma(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]));
                                break;
                            }
                            else
                            {
                                tempo1 = dao.tabla;

                                if (tempo1.Rows.Count > 0)
                                {
                                    mensaje = "El programa seleccionado tiene asociado varios subprogramas, debe eliminarlos primero para incativar este registro";
                                    gvPrograPresupuestario.JSProperties["cpError"] = "Ocurrio un error: " + mensaje;
                                    e.Cancel = true;
                                    cargaPrograma(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]));

                                    break;
                                }
                                else
                                {
                                    sql = "SELECT * FROM SCHE$SIPLAN20.SP20$PRODUCTO WHERE SPPRO$POM = " + Convert.ToInt32(Session["pom"]) + " AND SPPRO$INSTO = " + Convert.ToInt32(Session["insto"]) + " AND SPPRO$PRESUPUESTO = " + codigo + " AND SPPRO$RESTRICTIVA = 'N'";
                                    estado = dao.consulta(sql);
                                    if (estado == 0)
                                    {
                                        mensaje = dao.mensaje;
                                        gvPrograPresupuestario.JSProperties["cpError"] = "Ocurrio un error: " + mensaje;
                                        e.Cancel = true;
                                        cargaPrograma(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]));

                                        break;
                                    }
                                    else
                                    {
                                        tempo1 = dao.tabla;
                                        if (tempo1.Rows.Count > 0)
                                        {
                                            mensaje = "El programa seleccionado tiene vinculado varios productos, debe eliminarlos primero para incativar este registro";
                                            gvPrograPresupuestario.JSProperties["cpError"] = "Ocurrio un error: " + mensaje;
                                            e.Cancel = true;
                                            cargaPrograma(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]));

                                            break;
                                        }
                                        else
                                        {
                                            sql = "UPDATE SCHE$SIPLAN20.SP20$PROGRAMA_PRESUPUESTARIO SET SPPRO$RESTRICTIVA = 'S', SPPRO$FECHA_DELETE = 'UPDATE = '||TO_CHAR(SYSDATE,'DD/MM/YYYY HH24:MI')||' '||'" + Session["USUARIO"].ToString() + "' WHERE SPPRO$ID_PROGRAMA_PRESUPUESTO = " + codigo + " AND SPPRO$ID_POM = " + Convert.ToInt32(Session["pom"]) + " AND SPPRO$ID_INSTITUCION = " + Convert.ToInt32(Session["insto"]) + " AND SPPRO$RESTRICTIVA = 'N'";
                                            estado = dao.comando(sql);
                                            if (estado == 0)
                                            {
                                                mensaje = dao.mensaje;
                                                gvPrograPresupuestario.JSProperties["cpError"] = "Ocurrio un error: " + mensaje;

                                                e.Cancel = true;
                                                cargaPrograma(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]));
                                                resultado = busca_programa(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]));
                                                if (resultado == 0)
                                                    gvPrograPresupuestario.JSProperties["cpShow"] = "1";
                                                else
                                                    gvPrograPresupuestario.JSProperties["cpShow"] = null;
                                                break;
                                            }
                                            else
                                            {
                                                mensaje = dao.mensaje;
                                                gvPrograPresupuestario.JSProperties["cpError"] = "Operacion correcta: Registro borrado correctamente";

                                                e.Cancel = true;
                                                cargaPrograma(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]));
                                                resultado = busca_programa(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]));
                                                if (resultado == 0)
                                                    gvPrograPresupuestario.JSProperties["cpShow"] = "1";
                                                else
                                                    gvPrograPresupuestario.JSProperties["cpShow"] = null;
                                                break;
                                            }
                                        }
                                    }

                                }



                            }

                        }

                        else
                        {
                            //sql = "SELECT * FROM SCHE$SIPLAN20.SP20$PROGRAMA_PRESUPUESTARIO WHERE SPPRO$ID_POM = " + Convert.ToInt32(Session["pom"]) + " AND SPPRO$ID_INSTITUCION = " + Convert.ToInt32(Session["insto"]) + " AND SPPRO$ID_PROGRAMA_PRESUPUESTO = " + codigo;
                            //sql = "SELECT* FROM SCHE$SIPLAN20.SP20$PRODUCTO WHERE SPPRO$POM = " + Convert.ToInt32(Session["pom"]) + " AND SPPRO$INSTO = " + Convert.ToInt32(Session["insto"]) + " AND SPPRO$PRESUPUESTO = " + codigo+ " AND SPPRO$RESTRICTIVA = 'N'";
                            sql = "SELECT* FROM SCHE$SIPLAN20.SP20$PRODUCTO WHERE SPPRO$POM = " + Convert.ToInt32(Session["pom"]) + " AND SPPRO$INSTO = " + Convert.ToInt32(Session["insto"]) + " AND SPPRO$PRESUPUESTO = " + tempo.Rows[i]["ID_SUBPROGRAMA"] + " AND SPPRO$RESTRICTIVA = 'N'";
                            estado = dao.consulta(sql);
                            if (estado == 0)
                            {
                                mensaje = dao.mensaje;
                                gvPrograPresupuestario.JSProperties["cpError"] = "Ocurrio un error: " + mensaje;
                                e.Cancel = true;
                                cargaPrograma(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]));

                                break;
                            }
                            else
                            {
                                tempo1 = dao.tabla;
                                if (tempo1.Rows.Count > 0)
                                {
                                    mensaje = "El programa seleccionado tiene vinculado varios productos, debe eliminarlos primero para incativar este registro";
                                    gvPrograPresupuestario.JSProperties["cpError"] = "Ocurrio un error: " + mensaje;
                                    e.Cancel = true;
                                    cargaPrograma(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]));

                                    break;
                                }
                                else
                                {
                                    sql = "UPDATE SCHE$SIPLAN20.SP20$PROGRAMA_PRESUPUESTARIO SET SPPRO$RESTRICTIVA = 'S', SPPRO$FECHA_DELETE = 'UPDATE = '||TO_CHAR(SYSDATE,'DD/MM/YYYY HH24:MI')||' '||'" + Session["USUARIO"].ToString() + "' WHERE SPPRO$ID_PROGRAMA_PRESUPUESTO = " + tempo.Rows[i]["ID_SUBPROGRAMA"] + " AND SPPRO$ID_POM = " + Convert.ToInt32(Session["pom"]) + " AND SPPRO$ID_INSTITUCION = " + Convert.ToInt32(Session["insto"]) + " AND SPPRO$RESTRICTIVA = 'N'";
                                    estado = dao.comando(sql);
                                    if (estado == 0)
                                    {
                                        mensaje = dao.mensaje;
                                        gvPrograPresupuestario.JSProperties["cpError"] = "Ocurrio un error: " + mensaje;

                                        e.Cancel = true;
                                        cargaPrograma(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]));
                                        resultado = busca_programa(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]));
                                        if (resultado == 0)
                                            gvPrograPresupuestario.JSProperties["cpShow"] = "1";
                                        else
                                            gvPrograPresupuestario.JSProperties["cpShow"] = null;
                                        break;
                                    }
                                    else
                                    {
                                        mensaje = dao.mensaje;
                                        gvPrograPresupuestario.JSProperties["cpError"] = "Operacion correcta: Registro borrado correctamente";
                                        e.Cancel = true;
                                        cargaPrograma(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]));
                                        resultado = busca_programa(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]));
                                        if (resultado == 0)
                                            gvPrograPresupuestario.JSProperties["cpShow"] = "1";
                                        else
                                            gvPrograPresupuestario.JSProperties["cpShow"] = null;
                                        break;
                                    }
                                }
                            }

                        }



                    }
                    else
                    {

                        gvPOMInsto.JSProperties["cpError"] = "Usted no esta autorizado para borrar este registro";
                        e.Cancel = true;
                        break;
                    }
                }


            }

        }

        protected void btnProductos_Click(object sender, EventArgs e)
        {
            if (gvPOMInsto.FocusedRowIndex == -1)
            {
                mensaje = "Debe seleccionar primero el encabezado de su Programación Multianual";
                Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
                MultiView1.ActiveViewIndex = 0;
            }
            else
            {
                Session["pom"] = Convert.ToInt32(gvPOMInsto.GetRowValues(gvPOMInsto.FocusedRowIndex, "SPPO$ID_POM").ToString());
                Session["insto"] = Convert.ToInt32(gvPOMInsto.GetRowValues(gvPOMInsto.FocusedRowIndex, "SPPO$ID_INSTITUCION").ToString());
                /*es posible que se necesite descomentar por validación de subproducto*/
                //Session["prioriza"] = Convert.ToInt32(gvPOMInsto.GetRowValues(gvPOMInsto.FocusedRowIndex, "PRIORIZA").ToString());
                /*es posible que se necesite descomentar por validación de subproducto*/
                Session["abierto"] = Convert.ToInt32(gvPOMInsto.GetRowValues(gvPOMInsto.FocusedRowIndex, "SPP$ABIERTO").ToString());

                gvProdInsto.DataSource = null;
                gvProdEstrategicos.DataSource = null;
                cargaProductosInsto(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]));
                //cargaProductoRed(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]));
                cargaResultadoInsto(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), -1);
                cargaProgramaPresu(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]));
                cargaUnidades();
                cargaPilares();
                if (Convert.ToInt32(Session["abierto"]) == 1)
                {
                    btnNuevo.Enabled = false;
                    btnEdit.Enabled = false;
                    btnDelete.Enabled = false;
                    btnSNIP.Enabled = false;
                }
                else
                {
                    btnNuevo.Enabled = true;
                    btnEdit.Enabled = true;
                    btnDelete.Enabled = true;
                    btnSNIP.Enabled = true;
                }
                rbProductos.SelectedIndex = 0;
                ProdInsitucionales.Style.Add("display", "block");
                MultiView1.ActiveViewIndex = 4;
            }

        }

        protected void cargaProductosInsto(int pom, int insto)
        {
            int resultado = -1;
            resultado = busca_productos_insto(pom, insto);
            if (resultado == 0)
            {
                Session["CONFIRMA"] = 3; //variable de migración de productos institucionales
                Confirmamigracion.Text = "No se han encontrado productos institucionales para el periodo de esta programación, ¿Necesita trasladar la información de los productos institucionales del periodo anterior a la presente programación?";
                Session["prod_institucionales"] = null;
                gvProdInsto.DataSource = null;
                Confirma.ShowOnPageLoad = true;
            }
            else
                cargaProductosInstitucionales(pom, insto);

        }

        protected int busca_productos_insto(int pom, int insto)
        {
            int resultado = -1;
            sql = @"      
SELECT R.SPRES$ID_RESULTADO
                    ,R.SPRES$DESCRIPCION
                    ,SCHE$SIPLAN20.FCN$BUSCA_ID_EJE(R.SPRES$ID_RESULTADO) ID_EJE
                    ,SCHE$SIPLAN20.FCN$BUSCA_EJE(R.SPRES$ID_RESULTADO) EJE_ESTRATEGICO
                    ,SCHE$SIPLAN20.FCN$BUSCA_NIVEL(R.SPRES$ID_RESULTADO) SPPRES$NIVEL
                    ,SCHE$SIPLAN20.FCN$BUSCA_META(R.SPRES$ID_RESULTADO) META_PRESIDENCIAL
                    ,SCHE$SIPLAN20.FCN$BUSCA_ACCION(R.SPRES$ID_RESULTADO) ACCION_ESTRATEGICA
                    ,PR.SPPRO$ID_PROGRAMA_PRESUPUESTO
                    ,PR.SPPRO$DESCRIPCION PRESUPUESTO
                    ,PO.SPPRO$ID_PRODUCTO
                    ,PO.SPPRO$DESCRIPCION PRODUCTO
                    ,PO.SPPRO$ID_MEDIDA
                    ,UM.SNCGUM$NOMBRE
                    ,PO.SPPRO$OBJETIVO_CENTRAL
                    ,PO.SPPRO$ACCION_ESTRATEGICA
                    ,(SELECT R.SPPRES$DESCRIPCION FROM SCHE$SIPLAN20.SP20$RESULTADOS_ESTRATEGICOS R WHERE R.SPPRES$RESTRICTIVA ='N' AND R.SPPRES$ID_RESULTADO = PO.SPPRO$OBJETIVO_CENTRAL AND R.SPPRES$NIVEL = 3) OBJETIVO_SECTORIAL
                    ,(SELECT R.SPPRES$DESCRIPCION FROM SCHE$SIPLAN20.SP20$RESULTADOS_ESTRATEGICOS R WHERE R.SPPRES$RESTRICTIVA ='N' AND R.SPPRES$ID_RESULTADO = PO.SPPRO$ACCION_ESTRATEGICA AND R.SPPRES$NIVEL = 4) ACCCION_ESTRATEGICA
                    ,PO.SPPRO$PROPIETARIO
                    ,(SELECT CASE WHEN COUNT(M.NOMBRE) > 0 THEN 'SI' ELSE 'NO' END AS  MUNICIPIO FROM SINIP.CG_GEOGRAFICO M INNER JOIN SCHE$SIPLAN20.SP20$MUNOSPRIORIZADOS MP ON M.GEOGRAFICO = MP.GEOGRAFICO AND MP.RESTRICTIVA = 'N' WHERE MP.SPPRO$ID_PRODUCTO = PO.SPPRO$ID_PRODUCTO) MUNICIPIOS  
                    ,R.SPRES$COD_ESTRATEGICO             
FROM SCHE$SIPLAN20.SP20$RESULTADOS R 
                    INNER JOIN SCHE$SIPLAN20.SP20$PRODUCTO PO ON R.SPRES$ID_RESULTADO = PO.SPPRO$ID_RESULTADO AND R.SPRES$POM = PO.SPPRO$POM AND R.SPRES$INSTITUCION = PO.SPPRO$INSTO AND R.SPRES$RESTRICTIVA = 'N' AND PO.SPPRO$RESTRICTIVA = 'N' 
                    INNER JOIN SCHE$SIPLAN20.SP20$PROGRAMA_PRESUPUESTARIO PR ON PO.SPPRO$PRESUPUESTO = PR.SPPRO$ID_PROGRAMA_PRESUPUESTO AND PO.SPPRO$POM = PR.SPPRO$ID_POM AND PR.SPPRO$ID_INSTITUCION = PO.SPPRO$INSTO AND PR.SPPRO$RESTRICTIVA = 'N' AND PO.SPPRO$RESTRICTIVA = 'N' 
                    INNER JOIN SINIP.SNTBCG$UNIDAD_MEDIDA UM ON PO.SPPRO$ID_MEDIDA = UM.SNCGUM$UNIDAD_MEDIDA WHERE R.SPRES$POM = " + pom + " AND R.SPRES$INSTITUCION = " + insto + @" AND R.SPRES$TIPO = 1   

                   UNION                   

                   SELECT RE.SPRES$ID_RESULTADO
                  ,RE.SPRES$DESCRIPCION
                  ,RP.SPPRES$ID_RESULTADO ID_EJE
                  ,'PILAR PGG 2020-2024 '||RP.SPPRES$DESCRIPCION EJE_ESTRATEGICO
                  ,R.SPPRES$NIVEL
                  ,'META PGG 2020-2024 '||R.SPPRES$CODIGO || '-' || R.SPPRES$DESCRIPCION META_PRESIDENCIAL
                  ,' ' ACCION_ESTRATEGICA
                  ,PR.SPPRO$ID_PROGRAMA_PRESUPUESTO
                  ,PR.SPPRO$DESCRIPCION PROGRAMA_PRESUPUETARIO
                  ,P.SPPRO$ID_PRODUCTO
                  ,P.SPPRO$DESCRIPCION PRODUCTO
                  ,P.SPPRO$ID_MEDIDA
                  ,UM.SNCGUM$NOMBRE
                  ,P.SPPRO$OBJETIVO_CENTRAL
                  ,P.SPPRO$ACCION_ESTRATEGICA
                  ,(SELECT R.SPPRES$DESCRIPCION FROM SCHE$SIPLAN20.SP20$RESULTADOS_ESTRATEGICOS R WHERE R.SPPRES$RESTRICTIVA = 'N' AND R.SPPRES$ID_RESULTADO = P.SPPRO$OBJETIVO_CENTRAL AND R.SPPRES$NIVEL = 3) OBJETIVO_SECTORIAL
                  ,(SELECT R.SPPRES$DESCRIPCION FROM SCHE$SIPLAN20.SP20$RESULTADOS_ESTRATEGICOS R WHERE R.SPPRES$RESTRICTIVA = 'N' AND R.SPPRES$ID_RESULTADO = P.SPPRO$ACCION_ESTRATEGICA AND R.SPPRES$NIVEL = 4) ACCCION_ESTRATEGICA
                  ,P.SPPRO$PROPIETARIO
                  ,(SELECT CASE WHEN COUNT(M.NOMBRE) > 0 THEN 'SI' ELSE 'NO' END AS  MUNICIPIO FROM SINIP.CG_GEOGRAFICO M INNER JOIN SCHE$SIPLAN20.SP20$MUNOSPRIORIZADOS MP ON M.GEOGRAFICO = MP.GEOGRAFICO AND MP.RESTRICTIVA = 'N' WHERE MP.SPPRO$ID_PRODUCTO = P.SPPRO$ID_PRODUCTO) MUNICIPIOS  
                  ,RE.SPRES$COD_ESTRATEGICO                    
FROM SCHE$SIPLAN20.SP20$RESULTADOS_ESTRATEGICOS R 
                  INNER JOIN SCHE$SIPLAN20.SP20$RESULTADOS RE ON RE.SPRES$COD_ESTRATEGICO = R.SPPRES$ID_RESULTADO AND R.SPPRES$RESTRICTIVA = 'N' AND RE.SPRES$RESTRICTIVA = 'N' AND R.SPPRES$NIVEL = 2 AND RE.SPRES$TIPO = 0
                  INNER JOIN SCHE$SIPLAN20.SP20$RESULTADOS_ESTRATEGICOS RP ON RP.SPPRES$ID_RESULTADO = R.SPPRES$DEPENDE AND RP.SPPRES$RESTRICTIVA = 'N' AND R.SPPRES$RESTRICTIVA = 'N' AND RP.SPPRES$NIVEL = 1
                  INNER JOIN SCHE$SIPLAN20.S20$PERIODO_RESULTADO PRE ON PRE.SPR$ID_RESULTADO = RP.SPPRES$ID_RESULTADO AND PRE.SPR$RESTRICTIVA = 'N' AND RP.SPPRES$RESTRICTIVA = 'N' AND RP.SPPRES$NIVEL = 1
                  INNER JOIN SCHE$SIPLAN20.SP20$PERIODO_GOBIERNO PG ON PG.SPG$ID_PERIODO = PRE.SPR$ID_PERIODO AND PG.SPG$RESTRICTIVA = 'N' AND PRE.SPR$RESTRICTIVA = 'N'
                  INNER JOIN SCHE$SIPLAN20.SP20$PRODUCTO P ON RE.SPRES$ID_RESULTADO = P.SPPRO$ID_RESULTADO AND RE.SPRES$POM = P.SPPRO$POM AND RE.SPRES$INSTITUCION = P.SPPRO$INSTO AND RE.SPRES$RESTRICTIVA = 'N' AND P.SPPRO$RESTRICTIVA = 'N' 
                  INNER JOIN SCHE$SIPLAN20.SP20$PROGRAMA_PRESUPUESTARIO PR ON PR.SPPRO$ID_PROGRAMA_PRESUPUESTO = P.SPPRO$PRESUPUESTO AND PR.SPPRO$ID_POM = P.SPPRO$POM AND PR.SPPRO$ID_INSTITUCION = P.SPPRO$INSTO AND PR.SPPRO$RESTRICTIVA = 'N' AND P.SPPRO$RESTRICTIVA = 'N' 
                  INNER JOIN SINIP.SNTBCG$UNIDAD_MEDIDA UM ON P.SPPRO$ID_MEDIDA = UM.SNCGUM$UNIDAD_MEDIDA WHERE R.SPPRES$RESTRICTIVA = 'N' AND R.SPPRES$NIVEL = 2 AND RE.SPRES$POM = " + pom + " AND RE.SPRES$INSTITUCION = " + insto + @"  AND RE.SPRES$TIPO = 0  AND PG.SPG$ID_PERIODO = 0 
                  
                  UNION
                  
                  SELECT RE.SPRES$ID_RESULTADO
,RE.SPRES$DESCRIPCION
,RP.SPPRES$ID_RESULTADO ID_EJE
,RP.SPPRES$DESCRIPCION EJE_ESTRATEGICO
,R.SPPRES$NIVEL
,RM.SPPRES$CODIGO ||'-'|| RM.SPPRES$DESCRIPCION META_PRESIDENCIAL
,R.SPPRES$CODIGO||'-'||R.SPPRES$DESCRIPCION ACCION_ESTRATEGICA
,PR.SPPRO$ID_PROGRAMA_PRESUPUESTO
                  ,PR.SPPRO$DESCRIPCION PROGRAMA_PRESUPUETARIO
                  ,P.SPPRO$ID_PRODUCTO
                  ,P.SPPRO$DESCRIPCION PRODUCTO
                  ,P.SPPRO$ID_MEDIDA
                  ,UM.SNCGUM$NOMBRE
                  ,P.SPPRO$OBJETIVO_CENTRAL
                  ,P.SPPRO$ACCION_ESTRATEGICA
                  ,(SELECT R.SPPRES$DESCRIPCION FROM SCHE$SIPLAN20.SP20$RESULTADOS_ESTRATEGICOS R WHERE R.SPPRES$RESTRICTIVA = 'N' AND R.SPPRES$ID_RESULTADO = P.SPPRO$OBJETIVO_CENTRAL AND R.SPPRES$NIVEL = 3) OBJETIVO_SECTORIAL
                  ,(SELECT R.SPPRES$DESCRIPCION FROM SCHE$SIPLAN20.SP20$RESULTADOS_ESTRATEGICOS R WHERE R.SPPRES$RESTRICTIVA = 'N' AND R.SPPRES$ID_RESULTADO = P.SPPRO$ACCION_ESTRATEGICA AND R.SPPRES$NIVEL = 4) ACCCION_ESTRATEGICA
                  ,P.SPPRO$PROPIETARIO
                  ,(SELECT CASE WHEN COUNT(M.NOMBRE) > 0 THEN 'SI' ELSE 'NO' END AS  MUNICIPIO FROM SINIP.CG_GEOGRAFICO M INNER JOIN SCHE$SIPLAN20.SP20$MUNOSPRIORIZADOS MP ON M.GEOGRAFICO = MP.GEOGRAFICO AND MP.RESTRICTIVA = 'N' WHERE MP.SPPRO$ID_PRODUCTO = P.SPPRO$ID_PRODUCTO) MUNICIPIOS  
                  ,RE.SPRES$COD_ESTRATEGICO        
FROM SCHE$SIPLAN20.SP20$RESULTADOS_ESTRATEGICOS R 
        INNER JOIN SCHE$SIPLAN20.SP20$RESULTADOS RE ON RE.SPRES$COD_ESTRATEGICO = R.SPPRES$ID_RESULTADO AND R.SPPRES$RESTRICTIVA = 'N' AND RE.SPRES$RESTRICTIVA = 'N' AND R.SPPRES$NIVEL = 3 AND RE.SPRES$TIPO = 0 
        INNER JOIN SCHE$SIPLAN20.SP20$RESULTADOS_ESTRATEGICOS RM ON RM.SPPRES$ID_RESULTADO = R.SPPRES$DEPENDE AND RM.SPPRES$RESTRICTIVA = 'N' AND R.SPPRES$RESTRICTIVA = 'N' AND RM.SPPRES$NIVEL = 2 AND R.SPPRES$NIVEL = 3
        INNER JOIN SCHE$SIPLAN20.SP20$RESULTADOS_ESTRATEGICOS RP ON RP.SPPRES$ID_RESULTADO = RM.SPPRES$DEPENDE AND RP.SPPRES$RESTRICTIVA = 'N' AND RM.SPPRES$RESTRICTIVA = 'N' AND RP.SPPRES$NIVEL = 1 AND RM.SPPRES$NIVEL = 2
        INNER JOIN SCHE$SIPLAN20.S20$PERIODO_RESULTADO PRE ON PRE.SPR$ID_RESULTADO = RP.SPPRES$ID_RESULTADO AND PRE.SPR$RESTRICTIVA = 'N' AND RP.SPPRES$RESTRICTIVA = 'N' AND RP.SPPRES$NIVEL = 1
        INNER JOIN SCHE$SIPLAN20.SP20$PERIODO_GOBIERNO PG ON PG.SPG$ID_PERIODO = PRE.SPR$ID_PERIODO AND PG.SPG$RESTRICTIVA = 'N' AND PRE.SPR$RESTRICTIVA = 'N' AND PG.SPG$VIGENTE = 1 
        INNER JOIN SCHE$SIPLAN20.SP20$PRODUCTO P ON RE.SPRES$ID_RESULTADO = P.SPPRO$ID_RESULTADO AND RE.SPRES$POM = P.SPPRO$POM AND RE.SPRES$INSTITUCION = P.SPPRO$INSTO AND RE.SPRES$RESTRICTIVA = 'N' AND P.SPPRO$RESTRICTIVA = 'N' 
        INNER JOIN SCHE$SIPLAN20.SP20$PROGRAMA_PRESUPUESTARIO PR ON PR.SPPRO$ID_PROGRAMA_PRESUPUESTO = P.SPPRO$PRESUPUESTO AND PR.SPPRO$ID_POM = P.SPPRO$POM AND PR.SPPRO$ID_INSTITUCION = P.SPPRO$INSTO AND PR.SPPRO$RESTRICTIVA = 'N' AND P.SPPRO$RESTRICTIVA = 'N' 
        INNER JOIN SINIP.SNTBCG$UNIDAD_MEDIDA UM ON P.SPPRO$ID_MEDIDA = UM.SNCGUM$UNIDAD_MEDIDA WHERE R.SPPRES$RESTRICTIVA = 'N' AND RE.SPRES$POM = " + pom + " AND RE.SPRES$INSTITUCION = " + insto + @"  AND RE.SPRES$TIPO = 0 AND R.SPPRES$EJE_LINEA = -1
                  
                  UNION
                  
                  SELECT RE.SPRES$ID_RESULTADO
                  ,RE.SPRES$DESCRIPCION
                  ,RP.SPPRES$ID_RESULTADO ID_EJE
                  ,RP.SPPRES$DESCRIPCION EJE_ESTRATEGICO
,R.SPPRES$NIVEL
,RM.SPPRES$CODIGO ||'-'|| RM.SPPRES$DESCRIPCION META_PRESIDENCIAL
,R.SPPRES$CODIGO||'-'||R.SPPRES$DESCRIPCION ACCION_ESTRATEGICA
,PR.SPPRO$ID_PROGRAMA_PRESUPUESTO
                  ,PR.SPPRO$DESCRIPCION PROGRAMA_PRESUPUETARIO
                  ,P.SPPRO$ID_PRODUCTO
                  ,P.SPPRO$DESCRIPCION PRODUCTO
                  ,P.SPPRO$ID_MEDIDA
                  ,UM.SNCGUM$NOMBRE
                  ,P.SPPRO$OBJETIVO_CENTRAL
                  ,P.SPPRO$ACCION_ESTRATEGICA
                  ,(SELECT R.SPPRES$DESCRIPCION FROM SCHE$SIPLAN20.SP20$RESULTADOS_ESTRATEGICOS R WHERE R.SPPRES$RESTRICTIVA = 'N' AND R.SPPRES$ID_RESULTADO = P.SPPRO$OBJETIVO_CENTRAL AND R.SPPRES$NIVEL = 3) OBJETIVO_SECTORIAL
                  ,(SELECT R.SPPRES$DESCRIPCION FROM SCHE$SIPLAN20.SP20$RESULTADOS_ESTRATEGICOS R WHERE R.SPPRES$RESTRICTIVA = 'N' AND R.SPPRES$ID_RESULTADO = P.SPPRO$ACCION_ESTRATEGICA AND R.SPPRES$NIVEL = 4) ACCCION_ESTRATEGICA
                  ,P.SPPRO$PROPIETARIO
                  ,(SELECT CASE WHEN COUNT(M.NOMBRE) > 0 THEN 'SI' ELSE 'NO' END AS  MUNICIPIO FROM SINIP.CG_GEOGRAFICO M INNER JOIN SCHE$SIPLAN20.SP20$MUNOSPRIORIZADOS MP ON M.GEOGRAFICO = MP.GEOGRAFICO AND MP.RESTRICTIVA = 'N' WHERE MP.SPPRO$ID_PRODUCTO = P.SPPRO$ID_PRODUCTO) MUNICIPIOS  
                  ,RE.SPRES$COD_ESTRATEGICO  
FROM SCHE$SIPLAN20.SP20$RESULTADOS_ESTRATEGICOS R 
        INNER JOIN SCHE$SIPLAN20.SP20$RESULTADOS RE ON RE.SPRES$COD_ESTRATEGICO = R.SPPRES$ID_RESULTADO AND R.SPPRES$RESTRICTIVA = 'N' AND RE.SPRES$RESTRICTIVA = 'N' AND R.SPPRES$NIVEL = 3 AND RE.SPRES$TIPO = 0 
        INNER JOIN SCHE$SIPLAN20.SP20$RESULTADOS_ESTRATEGICOS RM ON RM.SPPRES$ID_RESULTADO = R.SPPRES$DEPENDE AND RM.SPPRES$RESTRICTIVA = 'N' AND R.SPPRES$RESTRICTIVA = 'N' AND RM.SPPRES$NIVEL = 2 AND R.SPPRES$NIVEL = 3
        INNER JOIN SCHE$SIPLAN20.SP20$RESULTADOS_ESTRATEGICOS RP ON RP.SPPRES$ID_RESULTADO = R.SPPRES$EJE_LINEA AND RP.SPPRES$RESTRICTIVA = 'N' AND R.SPPRES$RESTRICTIVA = 'N' AND RP.SPPRES$NIVEL = 1 AND R.SPPRES$NIVEL = 3
        INNER JOIN SCHE$SIPLAN20.S20$PERIODO_RESULTADO PRE ON PRE.SPR$ID_RESULTADO = RP.SPPRES$ID_RESULTADO AND PRE.SPR$RESTRICTIVA = 'N' AND RP.SPPRES$RESTRICTIVA = 'N' AND RP.SPPRES$NIVEL = 1
        INNER JOIN SCHE$SIPLAN20.SP20$PERIODO_GOBIERNO PG ON PG.SPG$ID_PERIODO = PRE.SPR$ID_PERIODO AND PG.SPG$RESTRICTIVA = 'N' AND PRE.SPR$RESTRICTIVA = 'N' AND PG.SPG$VIGENTE = 1 
        INNER JOIN SCHE$SIPLAN20.SP20$PRODUCTO P ON RE.SPRES$ID_RESULTADO = P.SPPRO$ID_RESULTADO AND RE.SPRES$POM = P.SPPRO$POM AND RE.SPRES$INSTITUCION = P.SPPRO$INSTO AND RE.SPRES$RESTRICTIVA = 'N' AND P.SPPRO$RESTRICTIVA = 'N' 
        INNER JOIN SCHE$SIPLAN20.SP20$PROGRAMA_PRESUPUESTARIO PR ON PR.SPPRO$ID_PROGRAMA_PRESUPUESTO = P.SPPRO$PRESUPUESTO AND PR.SPPRO$ID_POM = P.SPPRO$POM AND PR.SPPRO$ID_INSTITUCION = P.SPPRO$INSTO AND PR.SPPRO$RESTRICTIVA = 'N' AND P.SPPRO$RESTRICTIVA = 'N' 
        INNER JOIN SINIP.SNTBCG$UNIDAD_MEDIDA UM ON P.SPPRO$ID_MEDIDA = UM.SNCGUM$UNIDAD_MEDIDA WHERE R.SPPRES$RESTRICTIVA = 'N' AND RE.SPRES$POM = " + pom + " AND RE.SPRES$INSTITUCION = " + insto + @"   AND RE.SPRES$TIPO = 0  AND R.SPPRES$EJE_LINEA != -1

UNION

 SELECT RE.SPRES$ID_RESULTADO

,RE.SPRES$DESCRIPCION
,R.SPPRES$ID_RESULTADO ID_EJE
,R.SPPRES$DESCRIPCION EJE_ESTRATEGICO
,R.SPPRES$NIVEL
,' ' META_PRESIDENCIAL
,' ' ACCION_ESTRATEGICA
,PR.SPPRO$ID_PROGRAMA_PRESUPUESTO
                  ,PR.SPPRO$DESCRIPCION PROGRAMA_PRESUPUETARIO
                  ,P.SPPRO$ID_PRODUCTO
                  ,P.SPPRO$DESCRIPCION PRODUCTO
                  ,P.SPPRO$ID_MEDIDA
                  ,UM.SNCGUM$NOMBRE
                  ,P.SPPRO$OBJETIVO_CENTRAL
                  ,P.SPPRO$ACCION_ESTRATEGICA
                  ,(SELECT R.SPPRES$DESCRIPCION FROM SCHE$SIPLAN20.SP20$RESULTADOS_ESTRATEGICOS R WHERE R.SPPRES$RESTRICTIVA = 'N' AND R.SPPRES$ID_RESULTADO = P.SPPRO$OBJETIVO_CENTRAL AND R.SPPRES$NIVEL = 3) OBJETIVO_SECTORIAL
                  ,(SELECT R.SPPRES$DESCRIPCION FROM SCHE$SIPLAN20.SP20$RESULTADOS_ESTRATEGICOS R WHERE R.SPPRES$RESTRICTIVA = 'N' AND R.SPPRES$ID_RESULTADO = P.SPPRO$ACCION_ESTRATEGICA AND R.SPPRES$NIVEL = 4) ACCCION_ESTRATEGICA
                  ,P.SPPRO$PROPIETARIO
                  ,(SELECT CASE WHEN COUNT(M.NOMBRE) > 0 THEN 'SI' ELSE 'NO' END AS  MUNICIPIO FROM SINIP.CG_GEOGRAFICO M INNER JOIN SCHE$SIPLAN20.SP20$MUNOSPRIORIZADOS MP ON M.GEOGRAFICO = MP.GEOGRAFICO AND MP.RESTRICTIVA = 'N' WHERE MP.SPPRO$ID_PRODUCTO = P.SPPRO$ID_PRODUCTO) MUNICIPIOS  
        ,RE.SPRES$COD_ESTRATEGICO  
FROM SCHE$SIPLAN20.SP20$RESULTADOS_ESTRATEGICOS R 
        INNER JOIN SCHE$SIPLAN20.SP20$RESULTADOS RE ON RE.SPRES$COD_ESTRATEGICO = R.SPPRES$ID_RESULTADO AND R.SPPRES$RESTRICTIVA = 'N' AND RE.SPRES$RESTRICTIVA = 'N' AND R.SPPRES$NIVEL = 1 AND RE.SPRES$TIPO = 0 
        
        INNER JOIN SCHE$SIPLAN20.S20$PERIODO_RESULTADO PRE ON PRE.SPR$ID_RESULTADO = R.SPPRES$ID_RESULTADO AND PRE.SPR$RESTRICTIVA = 'N' AND R.SPPRES$RESTRICTIVA = 'N' AND R.SPPRES$NIVEL = 1
        INNER JOIN SCHE$SIPLAN20.SP20$PERIODO_GOBIERNO PG ON PG.SPG$ID_PERIODO = PRE.SPR$ID_PERIODO AND PG.SPG$RESTRICTIVA = 'N' AND PRE.SPR$RESTRICTIVA = 'N' AND PG.SPG$VIGENTE = 1 
        INNER JOIN SCHE$SIPLAN20.SP20$PRODUCTO P ON RE.SPRES$ID_RESULTADO = P.SPPRO$ID_RESULTADO AND RE.SPRES$POM = P.SPPRO$POM AND RE.SPRES$INSTITUCION = P.SPPRO$INSTO AND RE.SPRES$RESTRICTIVA = 'N' AND P.SPPRO$RESTRICTIVA = 'N' 
        INNER JOIN SCHE$SIPLAN20.SP20$PROGRAMA_PRESUPUESTARIO PR ON PR.SPPRO$ID_PROGRAMA_PRESUPUESTO = P.SPPRO$PRESUPUESTO AND PR.SPPRO$ID_POM = P.SPPRO$POM AND PR.SPPRO$ID_INSTITUCION = P.SPPRO$INSTO AND PR.SPPRO$RESTRICTIVA = 'N' AND P.SPPRO$RESTRICTIVA = 'N' 
        INNER JOIN SINIP.SNTBCG$UNIDAD_MEDIDA UM ON P.SPPRO$ID_MEDIDA = UM.SNCGUM$UNIDAD_MEDIDA WHERE R.SPPRES$RESTRICTIVA = 'N' AND RE.SPRES$POM = " + pom + " AND RE.SPRES$INSTITUCION = " + insto + @"   AND RE.SPRES$TIPO = 0 ";
            estado = dao.consulta(sql);
            if (estado == 1)
                tabla = dao.tabla;
            if (tabla.Rows.Count > 0)
                resultado = 1;
            else
                resultado = 0;

            return resultado;


        }


        protected void cargaProductosInstitucionales(int pom, int insto)
        {
            sql = @" 
                    SELECT R.SPRES$ID_RESULTADO
                    ,R.SPRES$DESCRIPCION
                    ,SCHE$SIPLAN20.FCN$BUSCA_ID_EJE(PO.SPPRO$RESULTADO2) ID_EJE
                    ,SCHE$SIPLAN20.FCN$BUSCA_EJE(PO.SPPRO$RESULTADO2) EJE_ESTRATEGICO
                    ,SCHE$SIPLAN20.FCN$BUSCA_NIVEL(PO.SPPRO$RESULTADO2) SPPRES$NIVEL
                    ,SCHE$SIPLAN20.FCN$BUSCA_META(PO.SPPRO$RESULTADO2) META_PRESIDENCIAL
                    ,SCHE$SIPLAN20.FCN$BUSCA_ACCION(PO.SPPRO$RESULTADO2) ACCION_ESTRATEGICA
                    ,PR.SPPRO$ID_PROGRAMA_PRESUPUESTO
                    ,PR.SPPRO$DESCRIPCION PRESUPUESTO
                    ,CASE WHEN PR.SPPRO$ES_ADMINISTRATIVO = 0 THEN 'PROGRAMAS SUSTANTIVOS' ELSE 'PROGRAMAS ADMINISTRATIVOS' END AS NATURALEZA
                    ,PR.SPPRO$ES_ADMINISTRATIVO
                    ,PO.SPPRO$ID_PRODUCTO
                    ,PO.SPPRO$DESCRIPCION PRODUCTO
                    ,PO.SPPRO$ID_MEDIDA
                    ,UM.SNCGUM$NOMBRE
                    ,PO.SPPRO$OBJETIVO_CENTRAL
                    ,PO.SPPRO$ACCION_ESTRATEGICA
                    ,(SELECT R.SPPRES$DESCRIPCION FROM SCHE$SIPLAN20.SP20$RESULTADOS_ESTRATEGICOS R WHERE R.SPPRES$RESTRICTIVA ='N' AND R.SPPRES$ID_RESULTADO = PO.SPPRO$OBJETIVO_CENTRAL AND R.SPPRES$NIVEL = 3) OBJETIVO_SECTORIAL
                    ,(SELECT R.SPPRES$DESCRIPCION FROM SCHE$SIPLAN20.SP20$RESULTADOS_ESTRATEGICOS R WHERE R.SPPRES$RESTRICTIVA ='N' AND R.SPPRES$ID_RESULTADO = PO.SPPRO$ACCION_ESTRATEGICA AND R.SPPRES$NIVEL = 4) ACCCION_ESTRATEGICA
                    ,PO.SPPRO$PROPIETARIO
                    ,(SELECT CASE WHEN COUNT(M.NOMBRE) > 0 THEN 'SI' ELSE 'NO' END AS  MUNICIPIO FROM SINIP.CG_GEOGRAFICO M INNER JOIN SCHE$SIPLAN20.SP20$MUNOSPRIORIZADOS MP ON M.GEOGRAFICO = MP.GEOGRAFICO AND MP.RESTRICTIVA = 'N' WHERE MP.SPPRO$ID_PRODUCTO = PO.SPPRO$ID_PRODUCTO) MUNICIPIOS  
                    ,R.SPRES$COD_ESTRATEGICO    
                    ,R.SPRES$TIPO
                    ,PO.SPPRO$RESULTADO2
                    
                    FROM SCHE$SIPLAN20.SP20$RESULTADOS R 
                    INNER JOIN SCHE$SIPLAN20.SP20$PRODUCTO PO ON R.SPRES$ID_RESULTADO = PO.SPPRO$ID_RESULTADO AND R.SPRES$POM = PO.SPPRO$POM AND R.SPRES$INSTITUCION = PO.SPPRO$INSTO AND R.SPRES$RESTRICTIVA = 'N' AND PO.SPPRO$RESTRICTIVA = 'N' 
                    INNER JOIN SCHE$SIPLAN20.SP20$PROGRAMA_PRESUPUESTARIO PR ON PO.SPPRO$PRESUPUESTO = PR.SPPRO$ID_PROGRAMA_PRESUPUESTO AND PO.SPPRO$POM = PR.SPPRO$ID_POM AND PR.SPPRO$ID_INSTITUCION = PO.SPPRO$INSTO AND PR.SPPRO$RESTRICTIVA = 'N' AND PO.SPPRO$RESTRICTIVA = 'N' 
                    INNER JOIN SINIP.SNTBCG$UNIDAD_MEDIDA UM ON PO.SPPRO$ID_MEDIDA = UM.SNCGUM$UNIDAD_MEDIDA WHERE R.SPRES$POM = " + pom + " AND R.SPRES$INSTITUCION = " + insto + @" AND R.SPRES$TIPO = 1  

                   UNION                   

                   SELECT RE.SPRES$ID_RESULTADO
                  ,(SELECT SPRES$DESCRIPCION FROM SCHE$SIPLAN20.SP20$RESULTADOS WHERE SPRES$ID_RESULTADO = P.SPPRO$RESULTADO2) SPRES$DESCRIPCION
                  ,RP.SPPRES$ID_RESULTADO ID_EJE
                  ,'PILAR PGG 2020-2024 '||RP.SPPRES$DESCRIPCION EJE_ESTRATEGICO
                  ,R.SPPRES$NIVEL
                  ,'META PGG 2020-2024 '||R.SPPRES$CODIGO || '-' || R.SPPRES$DESCRIPCION META_PRESIDENCIAL
                  ,' ' ACCION_ESTRATEGICA
                  ,PR.SPPRO$ID_PROGRAMA_PRESUPUESTO
                  ,PR.SPPRO$DESCRIPCION PROGRAMA_PRESUPUETARIO
                  ,CASE WHEN PR.SPPRO$ES_ADMINISTRATIVO = 0 THEN 'PROGRAMAS SUSTANTIVOS' ELSE 'PROGRAMAS ADMINISTRATIVOS' END AS NATURALEZA
                  ,PR.SPPRO$ES_ADMINISTRATIVO
                  ,P.SPPRO$ID_PRODUCTO
                  ,P.SPPRO$DESCRIPCION PRODUCTO
                  ,P.SPPRO$ID_MEDIDA
                  ,UM.SNCGUM$NOMBRE
                  ,P.SPPRO$OBJETIVO_CENTRAL
                  ,P.SPPRO$ACCION_ESTRATEGICA
                  ,(SELECT R.SPPRES$DESCRIPCION FROM SCHE$SIPLAN20.SP20$RESULTADOS_ESTRATEGICOS R WHERE R.SPPRES$RESTRICTIVA = 'N' AND R.SPPRES$ID_RESULTADO = P.SPPRO$OBJETIVO_CENTRAL AND R.SPPRES$NIVEL = 3) OBJETIVO_SECTORIAL
                  ,(SELECT R.SPPRES$DESCRIPCION FROM SCHE$SIPLAN20.SP20$RESULTADOS_ESTRATEGICOS R WHERE R.SPPRES$RESTRICTIVA = 'N' AND R.SPPRES$ID_RESULTADO = P.SPPRO$ACCION_ESTRATEGICA AND R.SPPRES$NIVEL = 4) ACCCION_ESTRATEGICA
                  ,P.SPPRO$PROPIETARIO
                  ,(SELECT CASE WHEN COUNT(M.NOMBRE) > 0 THEN 'SI' ELSE 'NO' END AS  MUNICIPIO FROM SINIP.CG_GEOGRAFICO M INNER JOIN SCHE$SIPLAN20.SP20$MUNOSPRIORIZADOS MP ON M.GEOGRAFICO = MP.GEOGRAFICO AND MP.RESTRICTIVA = 'N' WHERE MP.SPPRO$ID_PRODUCTO = P.SPPRO$ID_PRODUCTO) MUNICIPIOS  
                  ,RE.SPRES$COD_ESTRATEGICO   
                  ,RE.SPRES$TIPO
                  ,P.SPPRO$RESULTADO2
                  FROM SCHE$SIPLAN20.SP20$RESULTADOS_ESTRATEGICOS R 
                  INNER JOIN SCHE$SIPLAN20.SP20$RESULTADOS RE ON RE.SPRES$COD_ESTRATEGICO = R.SPPRES$ID_RESULTADO AND R.SPPRES$RESTRICTIVA = 'N' AND RE.SPRES$RESTRICTIVA = 'N' AND R.SPPRES$NIVEL = 2 AND RE.SPRES$TIPO = 0
                  INNER JOIN SCHE$SIPLAN20.SP20$RESULTADOS_ESTRATEGICOS RP ON RP.SPPRES$ID_RESULTADO = R.SPPRES$DEPENDE AND RP.SPPRES$RESTRICTIVA = 'N' AND R.SPPRES$RESTRICTIVA = 'N' AND RP.SPPRES$NIVEL = 1
                  INNER JOIN SCHE$SIPLAN20.S20$PERIODO_RESULTADO PRE ON PRE.SPR$ID_RESULTADO = RP.SPPRES$ID_RESULTADO AND PRE.SPR$RESTRICTIVA = 'N' AND RP.SPPRES$RESTRICTIVA = 'N' AND RP.SPPRES$NIVEL = 1
                  INNER JOIN SCHE$SIPLAN20.SP20$PERIODO_GOBIERNO PG ON PG.SPG$ID_PERIODO = PRE.SPR$ID_PERIODO AND PG.SPG$RESTRICTIVA = 'N' AND PRE.SPR$RESTRICTIVA = 'N'
                  INNER JOIN SCHE$SIPLAN20.SP20$PRODUCTO P ON RE.SPRES$ID_RESULTADO = P.SPPRO$ID_RESULTADO AND RE.SPRES$POM = P.SPPRO$POM AND RE.SPRES$INSTITUCION = P.SPPRO$INSTO AND RE.SPRES$RESTRICTIVA = 'N' AND P.SPPRO$RESTRICTIVA = 'N' 
                  INNER JOIN SCHE$SIPLAN20.SP20$PROGRAMA_PRESUPUESTARIO PR ON PR.SPPRO$ID_PROGRAMA_PRESUPUESTO = P.SPPRO$PRESUPUESTO AND PR.SPPRO$ID_POM = P.SPPRO$POM AND PR.SPPRO$ID_INSTITUCION = P.SPPRO$INSTO AND PR.SPPRO$RESTRICTIVA = 'N' AND P.SPPRO$RESTRICTIVA = 'N' 
                  INNER JOIN SINIP.SNTBCG$UNIDAD_MEDIDA UM ON P.SPPRO$ID_MEDIDA = UM.SNCGUM$UNIDAD_MEDIDA WHERE R.SPPRES$RESTRICTIVA = 'N' AND R.SPPRES$NIVEL = 2 AND RE.SPRES$POM = " + pom + " AND RE.SPRES$INSTITUCION = " + insto + @"  AND RE.SPRES$TIPO = 0  AND PG.SPG$ID_PERIODO = 0 
                  
                  UNION
                  
                  SELECT RE.SPRES$ID_RESULTADO
,(SELECT SPRES$DESCRIPCION FROM SCHE$SIPLAN20.SP20$RESULTADOS WHERE SPRES$ID_RESULTADO = P.SPPRO$RESULTADO2) SPRES$DESCRIPCION
,RP.SPPRES$ID_RESULTADO ID_EJE
,RP.SPPRES$DESCRIPCION EJE_ESTRATEGICO
,R.SPPRES$NIVEL
,RM.SPPRES$CODIGO ||'-'|| RM.SPPRES$DESCRIPCION META_PRESIDENCIAL
,R.SPPRES$CODIGO||'-'||R.SPPRES$DESCRIPCION ACCION_ESTRATEGICA
,PR.SPPRO$ID_PROGRAMA_PRESUPUESTO
                  ,PR.SPPRO$DESCRIPCION PROGRAMA_PRESUPUETARIO
                ,CASE WHEN PR.SPPRO$ES_ADMINISTRATIVO = 0 THEN 'PROGRAMAS SUSTANTIVOS' ELSE 'PROGRAMAS ADMINISTRATIVOS' END AS NATURALEZA
                  ,PR.SPPRO$ES_ADMINISTRATIVO
                  ,P.SPPRO$ID_PRODUCTO
                  ,P.SPPRO$DESCRIPCION PRODUCTO
                  ,P.SPPRO$ID_MEDIDA
                  ,UM.SNCGUM$NOMBRE
                   ,P.SPPRO$OBJETIVO_CENTRAL
                  ,P.SPPRO$ACCION_ESTRATEGICA
                  ,(SELECT R.SPPRES$DESCRIPCION FROM SCHE$SIPLAN20.SP20$RESULTADOS_ESTRATEGICOS R WHERE R.SPPRES$RESTRICTIVA = 'N' AND R.SPPRES$ID_RESULTADO = P.SPPRO$OBJETIVO_CENTRAL AND R.SPPRES$NIVEL = 3) OBJETIVO_SECTORIAL
                  ,(SELECT R.SPPRES$DESCRIPCION FROM SCHE$SIPLAN20.SP20$RESULTADOS_ESTRATEGICOS R WHERE R.SPPRES$RESTRICTIVA = 'N' AND R.SPPRES$ID_RESULTADO = P.SPPRO$ACCION_ESTRATEGICA AND R.SPPRES$NIVEL = 4) ACCCION_ESTRATEGICA
                  ,P.SPPRO$PROPIETARIO
                  ,(SELECT CASE WHEN COUNT(M.NOMBRE) > 0 THEN 'SI' ELSE 'NO' END AS  MUNICIPIO FROM SINIP.CG_GEOGRAFICO M INNER JOIN SCHE$SIPLAN20.SP20$MUNOSPRIORIZADOS MP ON M.GEOGRAFICO = MP.GEOGRAFICO AND MP.RESTRICTIVA = 'N' WHERE MP.SPPRO$ID_PRODUCTO = P.SPPRO$ID_PRODUCTO) MUNICIPIOS  
                  ,RE.SPRES$COD_ESTRATEGICO       
                    ,RE.SPRES$TIPO
                   ,P.SPPRO$RESULTADO2
FROM SCHE$SIPLAN20.SP20$RESULTADOS_ESTRATEGICOS R 
        INNER JOIN SCHE$SIPLAN20.SP20$RESULTADOS RE ON RE.SPRES$COD_ESTRATEGICO = R.SPPRES$ID_RESULTADO AND R.SPPRES$RESTRICTIVA = 'N' AND RE.SPRES$RESTRICTIVA = 'N' AND R.SPPRES$NIVEL = 3 AND RE.SPRES$TIPO = 0
        INNER JOIN SCHE$SIPLAN20.SP20$RESULTADOS_ESTRATEGICOS RM ON RM.SPPRES$ID_RESULTADO = R.SPPRES$DEPENDE AND RM.SPPRES$RESTRICTIVA = 'N' AND R.SPPRES$RESTRICTIVA = 'N' AND RM.SPPRES$NIVEL = 2 AND R.SPPRES$NIVEL = 3
        INNER JOIN SCHE$SIPLAN20.SP20$RESULTADOS_ESTRATEGICOS RP ON RP.SPPRES$ID_RESULTADO = RM.SPPRES$DEPENDE AND RP.SPPRES$RESTRICTIVA = 'N' AND RM.SPPRES$RESTRICTIVA = 'N' AND RP.SPPRES$NIVEL = 1 AND RM.SPPRES$NIVEL = 2
        INNER JOIN SCHE$SIPLAN20.S20$PERIODO_RESULTADO PRE ON PRE.SPR$ID_RESULTADO = RP.SPPRES$ID_RESULTADO AND PRE.SPR$RESTRICTIVA = 'N' AND RP.SPPRES$RESTRICTIVA = 'N' AND RP.SPPRES$NIVEL = 1
        INNER JOIN SCHE$SIPLAN20.SP20$PERIODO_GOBIERNO PG ON PG.SPG$ID_PERIODO = PRE.SPR$ID_PERIODO AND PG.SPG$RESTRICTIVA = 'N' AND PRE.SPR$RESTRICTIVA = 'N' AND PG.SPG$VIGENTE = 1 
        INNER JOIN SCHE$SIPLAN20.SP20$PRODUCTO P ON RE.SPRES$ID_RESULTADO = P.SPPRO$ID_RESULTADO AND RE.SPRES$POM = P.SPPRO$POM AND RE.SPRES$INSTITUCION = P.SPPRO$INSTO AND RE.SPRES$RESTRICTIVA = 'N' AND P.SPPRO$RESTRICTIVA = 'N' 
        INNER JOIN SCHE$SIPLAN20.SP20$PROGRAMA_PRESUPUESTARIO PR ON PR.SPPRO$ID_PROGRAMA_PRESUPUESTO = P.SPPRO$PRESUPUESTO AND PR.SPPRO$ID_POM = P.SPPRO$POM AND PR.SPPRO$ID_INSTITUCION = P.SPPRO$INSTO AND PR.SPPRO$RESTRICTIVA = 'N' AND P.SPPRO$RESTRICTIVA = 'N' 
        INNER JOIN SINIP.SNTBCG$UNIDAD_MEDIDA UM ON P.SPPRO$ID_MEDIDA = UM.SNCGUM$UNIDAD_MEDIDA WHERE R.SPPRES$RESTRICTIVA = 'N' AND RE.SPRES$POM = " + pom + " AND RE.SPRES$INSTITUCION = " + insto + @" AND RE.SPRES$TIPO = 0 AND R.SPPRES$EJE_LINEA = -1
                  
                  UNION
                  
                  SELECT RE.SPRES$ID_RESULTADO
                  ,(SELECT SPRES$DESCRIPCION FROM SCHE$SIPLAN20.SP20$RESULTADOS WHERE SPRES$ID_RESULTADO = P.SPPRO$RESULTADO2) SPRES$DESCRIPCION
                  ,RP.SPPRES$ID_RESULTADO ID_EJE
                  ,RP.SPPRES$DESCRIPCION EJE_ESTRATEGICO
,R.SPPRES$NIVEL
,RM.SPPRES$CODIGO ||'-'|| RM.SPPRES$DESCRIPCION META_PRESIDENCIAL
,R.SPPRES$CODIGO||'-'||R.SPPRES$DESCRIPCION ACCION_ESTRATEGICA
,PR.SPPRO$ID_PROGRAMA_PRESUPUESTO
                  ,PR.SPPRO$DESCRIPCION PROGRAMA_PRESUPUETARIO
                  ,CASE WHEN PR.SPPRO$ES_ADMINISTRATIVO = 0 THEN 'PROGRAMAS SUSTANTIVOS' ELSE 'PROGRAMAS ADMINISTRATIVOS' END AS NATURALEZA
                  ,PR.SPPRO$ES_ADMINISTRATIVO
                  ,P.SPPRO$ID_PRODUCTO
                  ,P.SPPRO$DESCRIPCION PRODUCTO
                  ,P.SPPRO$ID_MEDIDA
                  ,UM.SNCGUM$NOMBRE
                   ,P.SPPRO$OBJETIVO_CENTRAL
                  ,P.SPPRO$ACCION_ESTRATEGICA
                  ,(SELECT R.SPPRES$DESCRIPCION FROM SCHE$SIPLAN20.SP20$RESULTADOS_ESTRATEGICOS R WHERE R.SPPRES$RESTRICTIVA = 'N' AND R.SPPRES$ID_RESULTADO = P.SPPRO$OBJETIVO_CENTRAL AND R.SPPRES$NIVEL = 3) OBJETIVO_SECTORIAL
                  ,(SELECT R.SPPRES$DESCRIPCION FROM SCHE$SIPLAN20.SP20$RESULTADOS_ESTRATEGICOS R WHERE R.SPPRES$RESTRICTIVA = 'N' AND R.SPPRES$ID_RESULTADO = P.SPPRO$ACCION_ESTRATEGICA AND R.SPPRES$NIVEL = 4) ACCCION_ESTRATEGICA
                  ,P.SPPRO$PROPIETARIO
                  ,(SELECT CASE WHEN COUNT(M.NOMBRE) > 0 THEN 'SI' ELSE 'NO' END AS  MUNICIPIO FROM SINIP.CG_GEOGRAFICO M INNER JOIN SCHE$SIPLAN20.SP20$MUNOSPRIORIZADOS MP ON M.GEOGRAFICO = MP.GEOGRAFICO AND MP.RESTRICTIVA = 'N' WHERE MP.SPPRO$ID_PRODUCTO = P.SPPRO$ID_PRODUCTO) MUNICIPIOS  
                  ,RE.SPRES$COD_ESTRATEGICO  
                    ,RE.SPRES$TIPO
                   ,P.SPPRO$RESULTADO2
FROM SCHE$SIPLAN20.SP20$RESULTADOS_ESTRATEGICOS R 
        INNER JOIN SCHE$SIPLAN20.SP20$RESULTADOS RE ON RE.SPRES$COD_ESTRATEGICO = R.SPPRES$ID_RESULTADO AND R.SPPRES$RESTRICTIVA = 'N' AND RE.SPRES$RESTRICTIVA = 'N' AND R.SPPRES$NIVEL = 3 AND RE.SPRES$TIPO = 0
        INNER JOIN SCHE$SIPLAN20.SP20$RESULTADOS_ESTRATEGICOS RM ON RM.SPPRES$ID_RESULTADO = R.SPPRES$DEPENDE AND RM.SPPRES$RESTRICTIVA = 'N' AND R.SPPRES$RESTRICTIVA = 'N' AND RM.SPPRES$NIVEL = 2 AND R.SPPRES$NIVEL = 3
        INNER JOIN SCHE$SIPLAN20.SP20$RESULTADOS_ESTRATEGICOS RP ON RP.SPPRES$ID_RESULTADO = R.SPPRES$EJE_LINEA AND RP.SPPRES$RESTRICTIVA = 'N' AND R.SPPRES$RESTRICTIVA = 'N' AND RP.SPPRES$NIVEL = 1 AND R.SPPRES$NIVEL = 3
        INNER JOIN SCHE$SIPLAN20.S20$PERIODO_RESULTADO PRE ON PRE.SPR$ID_RESULTADO = RP.SPPRES$ID_RESULTADO AND PRE.SPR$RESTRICTIVA = 'N' AND RP.SPPRES$RESTRICTIVA = 'N' AND RP.SPPRES$NIVEL = 1
        INNER JOIN SCHE$SIPLAN20.SP20$PERIODO_GOBIERNO PG ON PG.SPG$ID_PERIODO = PRE.SPR$ID_PERIODO AND PG.SPG$RESTRICTIVA = 'N' AND PRE.SPR$RESTRICTIVA = 'N' AND PG.SPG$VIGENTE = 1 
        INNER JOIN SCHE$SIPLAN20.SP20$PRODUCTO P ON RE.SPRES$ID_RESULTADO = P.SPPRO$ID_RESULTADO AND RE.SPRES$POM = P.SPPRO$POM AND RE.SPRES$INSTITUCION = P.SPPRO$INSTO AND RE.SPRES$RESTRICTIVA = 'N' AND P.SPPRO$RESTRICTIVA = 'N'
        INNER JOIN SCHE$SIPLAN20.SP20$PROGRAMA_PRESUPUESTARIO PR ON PR.SPPRO$ID_PROGRAMA_PRESUPUESTO = P.SPPRO$PRESUPUESTO AND PR.SPPRO$ID_POM = P.SPPRO$POM AND PR.SPPRO$ID_INSTITUCION = P.SPPRO$INSTO AND PR.SPPRO$RESTRICTIVA = 'N' AND P.SPPRO$RESTRICTIVA = 'N' 
        INNER JOIN SINIP.SNTBCG$UNIDAD_MEDIDA UM ON P.SPPRO$ID_MEDIDA = UM.SNCGUM$UNIDAD_MEDIDA WHERE R.SPPRES$RESTRICTIVA = 'N' AND RE.SPRES$POM = " + pom + " AND RE.SPRES$INSTITUCION = " + insto + @" AND RE.SPRES$TIPO = 0  AND R.SPPRES$EJE_LINEA != -1

UNION

 SELECT RE.SPRES$ID_RESULTADO
,(SELECT SPRES$DESCRIPCION FROM SCHE$SIPLAN20.SP20$RESULTADOS WHERE SPRES$ID_RESULTADO = P.SPPRO$RESULTADO2) SPRES$DESCRIPCION
,R.SPPRES$ID_RESULTADO ID_EJE
,R.SPPRES$DESCRIPCION EJE_ESTRATEGICO
,R.SPPRES$NIVEL
,' ' META_PRESIDENCIAL
,' ' ACCION_ESTRATEGICA
,PR.SPPRO$ID_PROGRAMA_PRESUPUESTO
                  ,PR.SPPRO$DESCRIPCION PROGRAMA_PRESUPUETARIO
                    ,CASE WHEN PR.SPPRO$ES_ADMINISTRATIVO = 0 THEN 'PROGRAMAS SUSTANTIVOS' ELSE 'PROGRAMAS ADMINISTRATIVOS' END AS NATURALEZA
                  ,PR.SPPRO$ES_ADMINISTRATIVO
                  ,P.SPPRO$ID_PRODUCTO
                  ,P.SPPRO$DESCRIPCION PRODUCTO
                  ,P.SPPRO$ID_MEDIDA
                  ,UM.SNCGUM$NOMBRE
                   ,P.SPPRO$OBJETIVO_CENTRAL
                  ,P.SPPRO$ACCION_ESTRATEGICA
                  ,(SELECT R.SPPRES$DESCRIPCION FROM SCHE$SIPLAN20.SP20$RESULTADOS_ESTRATEGICOS R WHERE R.SPPRES$RESTRICTIVA = 'N' AND R.SPPRES$ID_RESULTADO = P.SPPRO$OBJETIVO_CENTRAL AND R.SPPRES$NIVEL = 3) OBJETIVO_SECTORIAL
                  ,(SELECT R.SPPRES$DESCRIPCION FROM SCHE$SIPLAN20.SP20$RESULTADOS_ESTRATEGICOS R WHERE R.SPPRES$RESTRICTIVA = 'N' AND R.SPPRES$ID_RESULTADO = P.SPPRO$ACCION_ESTRATEGICA AND R.SPPRES$NIVEL = 4) ACCCION_ESTRATEGICA
                  ,P.SPPRO$PROPIETARIO
                  ,(SELECT CASE WHEN COUNT(M.NOMBRE) > 0 THEN 'SI' ELSE 'NO' END AS  MUNICIPIO FROM SINIP.CG_GEOGRAFICO M INNER JOIN SCHE$SIPLAN20.SP20$MUNOSPRIORIZADOS MP ON M.GEOGRAFICO = MP.GEOGRAFICO AND MP.RESTRICTIVA = 'N' WHERE MP.SPPRO$ID_PRODUCTO = P.SPPRO$ID_PRODUCTO) MUNICIPIOS  
        ,RE.SPRES$COD_ESTRATEGICO  
         ,RE.SPRES$TIPO
         ,P.SPPRO$RESULTADO2
FROM SCHE$SIPLAN20.SP20$RESULTADOS_ESTRATEGICOS R 
        INNER JOIN SCHE$SIPLAN20.SP20$RESULTADOS RE ON RE.SPRES$COD_ESTRATEGICO = R.SPPRES$ID_RESULTADO AND R.SPPRES$RESTRICTIVA = 'N' AND RE.SPRES$RESTRICTIVA = 'N' AND R.SPPRES$NIVEL = 1 AND RE.SPRES$TIPO = 0
        
        INNER JOIN SCHE$SIPLAN20.S20$PERIODO_RESULTADO PRE ON PRE.SPR$ID_RESULTADO = R.SPPRES$ID_RESULTADO AND PRE.SPR$RESTRICTIVA = 'N' AND R.SPPRES$RESTRICTIVA = 'N' AND R.SPPRES$NIVEL = 1
        INNER JOIN SCHE$SIPLAN20.SP20$PERIODO_GOBIERNO PG ON PG.SPG$ID_PERIODO = PRE.SPR$ID_PERIODO AND PG.SPG$RESTRICTIVA = 'N' AND PRE.SPR$RESTRICTIVA = 'N' AND PG.SPG$VIGENTE = 1 
        INNER JOIN SCHE$SIPLAN20.SP20$PRODUCTO P ON RE.SPRES$ID_RESULTADO = P.SPPRO$ID_RESULTADO AND RE.SPRES$POM = P.SPPRO$POM AND RE.SPRES$INSTITUCION = P.SPPRO$INSTO AND RE.SPRES$RESTRICTIVA = 'N' AND P.SPPRO$RESTRICTIVA = 'N' 
        INNER JOIN SCHE$SIPLAN20.SP20$PROGRAMA_PRESUPUESTARIO PR ON PR.SPPRO$ID_PROGRAMA_PRESUPUESTO = P.SPPRO$PRESUPUESTO AND PR.SPPRO$ID_POM = P.SPPRO$POM AND PR.SPPRO$ID_INSTITUCION = P.SPPRO$INSTO AND PR.SPPRO$RESTRICTIVA = 'N' AND P.SPPRO$RESTRICTIVA = 'N' 
        INNER JOIN SINIP.SNTBCG$UNIDAD_MEDIDA UM ON P.SPPRO$ID_MEDIDA = UM.SNCGUM$UNIDAD_MEDIDA WHERE R.SPPRES$RESTRICTIVA = 'N' AND RE.SPRES$POM = " + pom + " AND RE.SPRES$INSTITUCION = " + insto + @"   AND RE.SPRES$TIPO = 0 
                   ";
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
                    Session["prod_institucionales"] = tabla;
                    gvProdInsto.DataSource = tabla;
                    gvProdInsto.DataBind();
                }

            }


        }
        protected void cargaProdEstrategicos_SIPLAN(int pom, int insto)
        {
            DataTable temp1 = new DataTable();
            DataTable tempo = new DataTable();
            DataTable subproductos = new DataTable();
            DataTable resultado = new DataTable();
            DataTable producto = new DataTable();
            DataTable poms = new DataTable();
            DataTable resultados = new DataTable();
            int orden = -1;
            int resultado2 = -1;
            int tipo = -1;



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
                            if (poms.Rows.Count > 0)
                            {
                                sql = "SELECT P.SPPRO$ID_PRODUCTO, R.SPRES$COD_ESTRATEGICO, P.SPPRO$DESCRIPCION, P.SPPRO$ID_MEDIDA, P.SPPRO$OBJETIVO_CENTRAL, P.SPPRO$ACCION_ESTRATEGICA, P.SPPRO$ID_RESULTADO, P.SPPRO$PRESUPUESTO, P.SPPRO$POM, P.SPPRO$INSTO, P.SPPRO$RESULTADO2 FROM SCHE$SIPLAN20.SP20$PRODUCTO P INNER JOIN SCHE$SIPLAN20.SP20$RESULTADOS R ON P.SPPRO$ID_RESULTADO = R.SPRES$ID_RESULTADO AND P.SPPRO$RESTRICTIVA = 'N' AND R.SPRES$RESTRICTIVA = 'N'  WHERE P.SPPRO$POM = " + poms.Rows[0]["SPPO$ID_POM"] + " AND P.SPPRO$INSTO = " + poms.Rows[0]["SPPO$ID_INSTITUCION"] + " AND R.SPRES$TIPO = 0";
                                estado = dao.consulta(sql);
                                producto = dao.tabla;
                                if (producto.Rows.Count > 0)
                                {
                                    for (int i = 0; i < producto.Rows.Count; i++)
                                    {
                                        sql = "SELECT * FROM SCHE$SIPLAN20.SP20$RESULTADOS WHERE SPRES$COD_ESTRATEGICO = " + producto.Rows[i]["SPRES$COD_ESTRATEGICO"] + " AND SPRES$RESTRICTIVA = 'N' AND SPRES$POM = " + pom + " AND SPRES$INSTITUCION = " + insto + " AND SPRES$TIPO = 0";
                                        estado = dao.consulta(sql);
                                        if (estado == 1)
                                        {
                                            resultado = dao.tabla;
                                            if (resultado.Rows.Count > 0)
                                            {
                                                if (Convert.ToInt32(producto.Rows[i]["SPPRO$RESULTADO2"]) != -1)
                                                {
                                                    sql = "SELECT * FROM SCHE$SIPLAN20.SP20$RESULTADOS WHERE SPRES$ID_RESULTADO = " + producto.Rows[i]["SPPRO$RESULTADO2"] + " AND SPRES$RESTRICTIVA = 'N' AND SPRES$POM = " + producto.Rows[i]["SPPRO$POM"] + " AND SPRES$INSTITUCION = " + producto.Rows[i]["SPPRO$INSTO"];
                                                    estado = dao.consulta(sql);
                                                    if (estado == 1)
                                                    {
                                                        resultados = dao.tabla;
                                                        if (resultados.Rows.Count > 0)
                                                        {
                                                            tipo = Convert.ToInt32(resultados.Rows[0]["SPRES$TIPO"]);
                                                            sql = "SELECT * FROM SCHE$SIPLAN20.SP20$RESULTADOS WHERE ";
                                                            if (tipo == 0)
                                                                sql = sql + " SPRES$COD_ESTRATEGICO= " + resultados.Rows[0]["SPRES$COD_ESTRATEGICO"];
                                                            else if (tipo == 1)
                                                                sql = sql + " SPRES$DESCRIPCION = " + resultados.Rows[0]["SPRES$DESCRIPCION"];
                                                            else if (tipo == 2)
                                                                sql = sql + " SPRES$COD_ESTRATEGICO= " + resultados.Rows[0]["SPRES$COD_ESTRATEGICO"];
                                                            sql = sql + " AND SPRES$RESTRICTIVA = 'N' AND SPRES$POM = " + pom + " AND SPRES$INSTITUCION = " + insto + " AND SPRES$TIPO = " + tipo;

                                                            estado = dao.consulta(sql);
                                                            if (estado == 1)
                                                            {
                                                                tabla = dao.tabla;
                                                                if (tabla.Rows.Count > 0)
                                                                    resultado2 = Convert.ToInt32(tabla.Rows[0]["SPRES$ID_RESULTADO"]);
                                                                else
                                                                    resultado2 = -1;

                                                            }
                                                            else
                                                                resultado2 = -1;
                                                        }
                                                        else
                                                            resultado2 = -1;
                                                    }
                                                    else

                                                        resultado2 = -1;
                                                }
                                                else
                                                    resultado2 = -1;


                                                sql = "INSERT INTO SCHE$SIPLAN20.SP20$PRODUCTO (SPPRO$DESCRIPCION, SPPRO$ID_MEDIDA, SPPRO$OBJETIVO_CENTRAL, SPPRO$ACCION_ESTRATEGICA, SPPRO$ID_RESULTADO, SPPRO$FECHA_INSERTA, SPPRO$PRESUPUESTO, SPPRO$POM, SPPRO$INSTO,SPPRO$RESULTADO2, SPPRO$ID_ANTERIOR) VALUES ('" + producto.Rows[i]["SPPRO$DESCRIPCION"] + "'," + producto.Rows[i]["SPPRO$ID_MEDIDA"];
                                                if (producto.Rows[i]["SPPRO$OBJETIVO_CENTRAL"] == DBNull.Value)
                                                    sql = sql + ",NULL,NULL";
                                                else
                                                    sql = sql + "," + producto.Rows[i]["SPPRO$OBJETIVO_CENTRAL"] + "," + producto.Rows[i]["SPPRO$ACCION_ESTRATEGICA"];
                                                sql = sql + "," + resultado.Rows[0]["SPRES$ID_RESULTADO"] + ",'INSERT = '||TO_CHAR(SYSDATE,'DD/MM/YYYY HH24:MI')||' '||'" + Session["USUARIO"].ToString() + "'," + producto.Rows[i]["SPPRO$PRESUPUESTO"] + "," + pom + "," + insto + "," + resultado2 + "," + producto.Rows[i]["SPPRO$ID_PRODUCTO"] + ")";

                                                estado = dao.comando(sql);
                                                if (estado == 0)
                                                    break;
                                            }
                                           
                                        }
                                    }

                                    if (estado == 1)
                                    {
                                        sql = @"SELECT RE.SPRES$ID_RESULTADO
                                                            ,RP.SPPRES$ID_RESULTADO ID_EJE
                                                            ,'PILAR PGG 2020-2024 '||RP.SPPRES$DESCRIPCION EJE_ESTRATEGICO
                                                            ,R.SPPRES$NIVEL
                                                            ,'META PGG 20202-2024 '||R.SPPRES$CODIGO || '-' || R.SPPRES$DESCRIPCION META_PRESIDENCIAL
                                                            ,' ' ACCION_ESTRATEGICA
                                                            ,PR.SPPRO$ID_PROGRAMA_PRESUPUESTO
                                                            ,PR.SPPRO$DESCRIPCION PROGRAMA_PRESUPUETARIO
                                                            ,P.SPPRO$ID_PRODUCTO
                                                            ,P.SPPRO$DESCRIPCION PRODUCTO
                                                            ,P.SPPRO$ID_MEDIDA
                                                            ,UM.SNCGUM$NOMBRE
                                                            ,(SELECT R.SPPRES$DESCRIPCION FROM SCHE$SIPLAN20.SP20$RESULTADOS_ESTRATEGICOS R WHERE R.SPPRES$RESTRICTIVA = 'N' AND R.SPPRES$ID_RESULTADO = P.SPPRO$OBJETIVO_CENTRAL AND R.SPPRES$NIVEL = 3) OBJETIVO_SECTORIAL
                                                            ,(SELECT R.SPPRES$DESCRIPCION FROM SCHE$SIPLAN20.SP20$RESULTADOS_ESTRATEGICOS R WHERE R.SPPRES$RESTRICTIVA = 'N' AND R.SPPRES$ID_RESULTADO = P.SPPRO$ACCION_ESTRATEGICA AND R.SPPRES$NIVEL = 4) ACCCION_ESTRATEGICA
                                                            ,P.SPPRO$PROPIETARIO
                                                            ,(SELECT CASE WHEN COUNT(M.NOMBRE) > 0 THEN 'SI' ELSE 'NO' END AS  MUNICIPIO FROM SINIP.CG_GEOGRAFICO M INNER JOIN SCHE$SIPLAN20.SP20$MUNOSPRIORIZADOS MP ON M.GEOGRAFICO = MP.GEOGRAFICO AND MP.RESTRICTIVA = 'N' WHERE MP.SPPRO$ID_PRODUCTO = P.SPPRO$ID_PRODUCTO) MUNICIPIOS  
                                                             FROM SCHE$SIPLAN20.SP20$RESULTADOS_ESTRATEGICOS R 
                                                            INNER JOIN SCHE$SIPLAN20.SP20$RESULTADOS RE ON RE.SPRES$COD_ESTRATEGICO = R.SPPRES$ID_RESULTADO AND R.SPPRES$RESTRICTIVA = 'N' AND RE.SPRES$RESTRICTIVA = 'N' AND R.SPPRES$NIVEL = 2
                                                            INNER JOIN SCHE$SIPLAN20.SP20$RESULTADOS_ESTRATEGICOS RP ON RP.SPPRES$ID_RESULTADO = R.SPPRES$DEPENDE AND RP.SPPRES$RESTRICTIVA = 'N' AND R.SPPRES$RESTRICTIVA = 'N' AND RP.SPPRES$NIVEL = 1
                                                            INNER JOIN SCHE$SIPLAN20.S20$PERIODO_RESULTADO PRE ON PRE.SPR$ID_RESULTADO = RP.SPPRES$ID_RESULTADO AND PRE.SPR$RESTRICTIVA = 'N' AND RP.SPPRES$RESTRICTIVA = 'N' AND RP.SPPRES$NIVEL = 1
                                                            INNER JOIN SCHE$SIPLAN20.SP20$PERIODO_GOBIERNO PG ON PG.SPG$ID_PERIODO = PRE.SPR$ID_PERIODO AND PG.SPG$RESTRICTIVA = 'N' AND PRE.SPR$RESTRICTIVA = 'N'
                                                            INNER JOIN SCHE$SIPLAN20.SP20$PRODUCTO P ON RE.SPRES$ID_RESULTADO = P.SPPRO$ID_RESULTADO AND RE.SPRES$POM = P.SPPRO$POM AND RE.SPRES$INSTITUCION = P.SPPRO$INSTO AND RE.SPRES$RESTRICTIVA = 'N' AND P.SPPRO$RESTRICTIVA = 'N' 
                                                            INNER JOIN SCHE$SIPLAN20.SP20$PROGRAMA_PRESUPUESTARIO PR ON PR.SPPRO$ID_PROGRAMA_PRESUPUESTO = P.SPPRO$PRESUPUESTO AND PR.SPPRO$ID_POM = P.SPPRO$POM AND PR.SPPRO$ID_INSTITUCION = P.SPPRO$INSTO AND PR.SPPRO$RESTRICTIVA = 'N' AND P.SPPRO$RESTRICTIVA = 'N' 
                                                            INNER JOIN SINIP.SNTBCG$UNIDAD_MEDIDA UM ON P.SPPRO$ID_MEDIDA = UM.SNCGUM$UNIDAD_MEDIDA WHERE R.SPPRES$RESTRICTIVA = 'N' AND R.SPPRES$NIVEL = 2 AND RE.SPRES$POM = " + pom + " AND RE.SPRES$INSTITUCION = " + insto + @"  AND RE.SPRES$TIPO = 0  AND PG.SPG$ID_PERIODO = 0 
                  
                                                            UNION
                  
                                                            SELECT RE.SPRES$ID_RESULTADO
                                                            ,RP.SPPRES$ID_RESULTADO ID_EJE
                                                            ,RP.SPPRES$DESCRIPCION EJE_ESTRATEGICO
                                                        ,R.SPPRES$NIVEL
,RM.SPPRES$CODIGO ||'-'|| RM.SPPRES$DESCRIPCION META_PRESIDENCIAL
,R.SPPRES$CODIGO||'-'||R.SPPRES$DESCRIPCION ACCION_ESTRATEGICA
,PR.SPPRO$ID_PROGRAMA_PRESUPUESTO
                  ,PR.SPPRO$DESCRIPCION PROGRAMA_PRESUPUETARIO
                  ,P.SPPRO$ID_PRODUCTO
                  ,P.SPPRO$DESCRIPCION PRODUCTO
                  ,P.SPPRO$ID_MEDIDA
                  ,UM.SNCGUM$NOMBRE
                  ,(SELECT R.SPPRES$DESCRIPCION FROM SCHE$SIPLAN20.SP20$RESULTADOS_ESTRATEGICOS R WHERE R.SPPRES$RESTRICTIVA = 'N' AND R.SPPRES$ID_RESULTADO = P.SPPRO$OBJETIVO_CENTRAL AND R.SPPRES$NIVEL = 3) OBJETIVO_SECTORIAL
                  ,(SELECT R.SPPRES$DESCRIPCION FROM SCHE$SIPLAN20.SP20$RESULTADOS_ESTRATEGICOS R WHERE R.SPPRES$RESTRICTIVA = 'N' AND R.SPPRES$ID_RESULTADO = P.SPPRO$ACCION_ESTRATEGICA AND R.SPPRES$NIVEL = 4) ACCCION_ESTRATEGICA
                  ,P.SPPRO$PROPIETARIO
                  ,(SELECT CASE WHEN COUNT(M.NOMBRE) > 0 THEN 'SI' ELSE 'NO' END AS  MUNICIPIO FROM SINIP.CG_GEOGRAFICO M INNER JOIN SCHE$SIPLAN20.SP20$MUNOSPRIORIZADOS MP ON M.GEOGRAFICO = MP.GEOGRAFICO AND MP.RESTRICTIVA = 'N' WHERE MP.SPPRO$ID_PRODUCTO = P.SPPRO$ID_PRODUCTO) MUNICIPIOS  
        FROM SCHE$SIPLAN20.SP20$RESULTADOS_ESTRATEGICOS R 
        INNER JOIN SCHE$SIPLAN20.SP20$RESULTADOS RE ON RE.SPRES$COD_ESTRATEGICO = R.SPPRES$ID_RESULTADO AND R.SPPRES$RESTRICTIVA = 'N' AND RE.SPRES$RESTRICTIVA = 'N' AND R.SPPRES$NIVEL = 3
        INNER JOIN SCHE$SIPLAN20.SP20$RESULTADOS_ESTRATEGICOS RM ON RM.SPPRES$ID_RESULTADO = R.SPPRES$DEPENDE AND RM.SPPRES$RESTRICTIVA = 'N' AND R.SPPRES$RESTRICTIVA = 'N' AND RM.SPPRES$NIVEL = 2 AND R.SPPRES$NIVEL = 3
        INNER JOIN SCHE$SIPLAN20.SP20$RESULTADOS_ESTRATEGICOS RP ON RP.SPPRES$ID_RESULTADO = RM.SPPRES$DEPENDE AND RP.SPPRES$RESTRICTIVA = 'N' AND RM.SPPRES$RESTRICTIVA = 'N' AND RP.SPPRES$NIVEL = 1 AND RM.SPPRES$NIVEL = 2
        INNER JOIN SCHE$SIPLAN20.S20$PERIODO_RESULTADO PRE ON PRE.SPR$ID_RESULTADO = RP.SPPRES$ID_RESULTADO AND PRE.SPR$RESTRICTIVA = 'N' AND RP.SPPRES$RESTRICTIVA = 'N' AND RP.SPPRES$NIVEL = 1
        INNER JOIN SCHE$SIPLAN20.SP20$PERIODO_GOBIERNO PG ON PG.SPG$ID_PERIODO = PRE.SPR$ID_PERIODO AND PG.SPG$RESTRICTIVA = 'N' AND PRE.SPR$RESTRICTIVA = 'N' AND PG.SPG$VIGENTE = 1 
        INNER JOIN SCHE$SIPLAN20.SP20$PRODUCTO P ON RE.SPRES$ID_RESULTADO = P.SPPRO$ID_RESULTADO AND RE.SPRES$POM = P.SPPRO$POM AND RE.SPRES$INSTITUCION = P.SPPRO$INSTO AND RE.SPRES$RESTRICTIVA = 'N' AND P.SPPRO$RESTRICTIVA = 'N' 
        INNER JOIN SCHE$SIPLAN20.SP20$PROGRAMA_PRESUPUESTARIO PR ON PR.SPPRO$ID_PROGRAMA_PRESUPUESTO = P.SPPRO$PRESUPUESTO AND PR.SPPRO$ID_POM = P.SPPRO$POM AND PR.SPPRO$ID_INSTITUCION = P.SPPRO$INSTO AND PR.SPPRO$RESTRICTIVA = 'N' AND P.SPPRO$RESTRICTIVA = 'N' 
        INNER JOIN SINIP.SNTBCG$UNIDAD_MEDIDA UM ON P.SPPRO$ID_MEDIDA = UM.SNCGUM$UNIDAD_MEDIDA WHERE R.SPPRES$RESTRICTIVA = 'N' AND RE.SPRES$POM = " + pom + " AND RE.SPRES$INSTITUCION = " + insto + @"   AND RE.SPRES$TIPO = 0 AND R.SPPRES$EJE_LINEA = -1
                  
                  UNION
                  
                  SELECT RE.SPRES$ID_RESULTADO
,RP.SPPRES$ID_RESULTADO ID_EJE
,RP.SPPRES$DESCRIPCION EJE_ESTRATEGICO
,R.SPPRES$NIVEL
,RM.SPPRES$CODIGO ||'-'|| RM.SPPRES$DESCRIPCION META_PRESIDENCIAL
,R.SPPRES$CODIGO||'-'||R.SPPRES$DESCRIPCION ACCION_ESTRATEGICA
,PR.SPPRO$ID_PROGRAMA_PRESUPUESTO
                  ,PR.SPPRO$DESCRIPCION PROGRAMA_PRESUPUETARIO
                  ,P.SPPRO$ID_PRODUCTO
                  ,P.SPPRO$DESCRIPCION PRODUCTO
                  ,P.SPPRO$ID_MEDIDA
                  ,UM.SNCGUM$NOMBRE
                  ,(SELECT R.SPPRES$DESCRIPCION FROM SCHE$SIPLAN20.SP20$RESULTADOS_ESTRATEGICOS R WHERE R.SPPRES$RESTRICTIVA = 'N' AND R.SPPRES$ID_RESULTADO = P.SPPRO$OBJETIVO_CENTRAL AND R.SPPRES$NIVEL = 3) OBJETIVO_SECTORIAL
                  ,(SELECT R.SPPRES$DESCRIPCION FROM SCHE$SIPLAN20.SP20$RESULTADOS_ESTRATEGICOS R WHERE R.SPPRES$RESTRICTIVA = 'N' AND R.SPPRES$ID_RESULTADO = P.SPPRO$ACCION_ESTRATEGICA AND R.SPPRES$NIVEL = 4) ACCCION_ESTRATEGICA
                  ,P.SPPRO$PROPIETARIO
                  ,(SELECT CASE WHEN COUNT(M.NOMBRE) > 0 THEN 'SI' ELSE 'NO' END AS  MUNICIPIO FROM SINIP.CG_GEOGRAFICO M INNER JOIN SCHE$SIPLAN20.SP20$MUNOSPRIORIZADOS MP ON M.GEOGRAFICO = MP.GEOGRAFICO AND MP.RESTRICTIVA = 'N' WHERE MP.SPPRO$ID_PRODUCTO = P.SPPRO$ID_PRODUCTO) MUNICIPIOS  
        FROM SCHE$SIPLAN20.SP20$RESULTADOS_ESTRATEGICOS R 
        INNER JOIN SCHE$SIPLAN20.SP20$RESULTADOS RE ON RE.SPRES$COD_ESTRATEGICO = R.SPPRES$ID_RESULTADO AND R.SPPRES$RESTRICTIVA = 'N' AND RE.SPRES$RESTRICTIVA = 'N' AND R.SPPRES$NIVEL = 3
        INNER JOIN SCHE$SIPLAN20.SP20$RESULTADOS_ESTRATEGICOS RM ON RM.SPPRES$ID_RESULTADO = R.SPPRES$DEPENDE AND RM.SPPRES$RESTRICTIVA = 'N' AND R.SPPRES$RESTRICTIVA = 'N' AND RM.SPPRES$NIVEL = 2 AND R.SPPRES$NIVEL = 3
        INNER JOIN SCHE$SIPLAN20.SP20$RESULTADOS_ESTRATEGICOS RP ON RP.SPPRES$ID_RESULTADO = R.SPPRES$EJE_LINEA AND RP.SPPRES$RESTRICTIVA = 'N' AND R.SPPRES$RESTRICTIVA = 'N' AND RP.SPPRES$NIVEL = 1 AND R.SPPRES$NIVEL = 3
        INNER JOIN SCHE$SIPLAN20.S20$PERIODO_RESULTADO PRE ON PRE.SPR$ID_RESULTADO = RP.SPPRES$ID_RESULTADO AND PRE.SPR$RESTRICTIVA = 'N' AND RP.SPPRES$RESTRICTIVA = 'N' AND RP.SPPRES$NIVEL = 1
        INNER JOIN SCHE$SIPLAN20.SP20$PERIODO_GOBIERNO PG ON PG.SPG$ID_PERIODO = PRE.SPR$ID_PERIODO AND PG.SPG$RESTRICTIVA = 'N' AND PRE.SPR$RESTRICTIVA = 'N' AND PG.SPG$VIGENTE = 1 
        INNER JOIN SCHE$SIPLAN20.SP20$PRODUCTO P ON RE.SPRES$ID_RESULTADO = P.SPPRO$ID_RESULTADO AND RE.SPRES$POM = P.SPPRO$POM AND RE.SPRES$INSTITUCION = P.SPPRO$INSTO AND RE.SPRES$RESTRICTIVA = 'N' AND P.SPPRO$RESTRICTIVA = 'N' 
        INNER JOIN SCHE$SIPLAN20.SP20$PROGRAMA_PRESUPUESTARIO PR ON PR.SPPRO$ID_PROGRAMA_PRESUPUESTO = P.SPPRO$PRESUPUESTO AND PR.SPPRO$ID_POM = P.SPPRO$POM AND PR.SPPRO$ID_INSTITUCION = P.SPPRO$INSTO AND PR.SPPRO$RESTRICTIVA = 'N' AND P.SPPRO$RESTRICTIVA = 'N' 
        INNER JOIN SINIP.SNTBCG$UNIDAD_MEDIDA UM ON P.SPPRO$ID_MEDIDA = UM.SNCGUM$UNIDAD_MEDIDA WHERE R.SPPRES$RESTRICTIVA = 'N' AND RE.SPRES$POM = " + pom + " AND RE.SPRES$INSTITUCION = " + insto + "   AND RE.SPRES$TIPO = 0  AND R.SPPRES$EJE_LINEA != -1";
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
                                                                sql = "SELECT * FROM SCHE$SIPLAN20.SP20$PRODUCTO WHERE SPPRO$DESCRIPCION = '" + producto.Rows[i]["SPPRO$DESCRIPCION"] + "'  AND SPPRO$PRESUPUESTO = " + producto.Rows[i]["SPPRO$PRESUPUESTO"] + "   AND SPPRO$POM = " + pom + " AND SPPRO$INSTO = " + insto + " AND SPPRO$RESTRICTIVA ='N'";
                                                                estado = dao.consulta(sql);
                                                                if (estado == 1)
                                                                {
                                                                    tempo = dao.tabla;
                                                                    if (tempo.Rows.Count > 0)
                                                                    {
                                                                        sql = "INSERT INTO SCHE$SIPLAN20.SP20$SUB_PRODUCTO (SPPSUB$DESCRIPCION, SPPSUB$ID_MEDIDA, SPPSUB$SNIP, SPPSUB$ID_PRODUCTO,SPPSUB$FECHA_INSERTA,SPPSUB$PROPIETARIO,SPPSUB$ID_ANTERIOR) VALUES (";
                                                                        if (subproductos.Rows[j]["SPPSUB$SNIP"] == DBNull.Value)
                                                                        {
                                                                            sql = sql + "'" + subproductos.Rows[j]["SPPSUB$DESCRIPCION"] + "'," + subproductos.Rows[j]["SPPSUB$ID_MEDIDA"] + ",NULL," + tempo.Rows[0]["SPPRO$ID_PRODUCTO"] + ",'INSERT = '||TO_CHAR(SYSDATE,'DD/MM/YYYY HH24:MI')||' '||'" + Session["USUARIO"].ToString() + "', '" + Session["USUARIO"].ToString() + "',"+ subproductos.Rows[j]["SPPSUB$ID_SUBPRODUCTO"] + ")";
                                                                        }

                                                                        else
                                                                        {
                                                                            sql = sql + "NULL,NULL," + subproductos.Rows[j]["SPPSUB$SNIP"] + "," + tempo.Rows[0]["SPPRO$ID_PRODUCTO"] + ",'INSERT = '||TO_CHAR(SYSDATE,'DD/MM/YYYY HH24:MI')||' '||'" + Session["USUARIO"].ToString() + "', '" + Session["USUARIO"].ToString() + "',"+ subproductos.Rows[j]["SPPSUB$ID_SUBPRODUCTO"] + ")";
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

        protected void cargaProductosResultadosInstitucionales(int pom, int insto)
        {
            int orden = -1;
            DataTable poms = new DataTable();
            DataTable producto = new DataTable();
            DataTable resultado = new DataTable();
            DataTable resultados = new DataTable();
            DataTable subproductos = new DataTable();
            DataTable tempo = new DataTable();
            int tipo = -1;
            int resultado2 = -1;
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
                                    sql = "SELECT P.SPPRO$ID_PRODUCTO, R.SPRES$DESCRIPCION RESULTADO, P.SPPRO$DESCRIPCION, P.SPPRO$ID_MEDIDA, P.SPPRO$OBJETIVO_CENTRAL, P.SPPRO$ACCION_ESTRATEGICA, P.SPPRO$ID_RESULTADO, P.SPPRO$PRESUPUESTO, P.SPPRO$POM, P.SPPRO$INSTO, P.SPPRO$RESULTADO2 FROM SCHE$SIPLAN20.SP20$PRODUCTO P INNER JOIN SCHE$SIPLAN20.SP20$RESULTADOS R ON P.SPPRO$ID_RESULTADO = R.SPRES$ID_RESULTADO AND P.SPPRO$RESTRICTIVA = 'N' AND R.SPRES$RESTRICTIVA = 'N'  WHERE P.SPPRO$POM = " + poms.Rows[0]["SPPO$ID_POM"] + " AND P.SPPRO$INSTO = " + poms.Rows[0]["SPPO$ID_INSTITUCION"] + " AND R.SPRES$TIPO = 1";
                                    estado = dao.consulta(sql);
                                    if (estado == 1)
                                    {
                                        producto = dao.tabla;
                                        if (producto.Rows.Count > 0)
                                        {
                                            for (int i = 0; i < producto.Rows.Count; i++)
                                            {
                                                sql = "SELECT * FROM SCHE$SIPLAN20.SP20$RESULTADOS WHERE SPRES$DESCRIPCION = '" + producto.Rows[i]["RESULTADO"] + "' AND SPRES$RESTRICTIVA = 'N' AND SPRES$POM = " + pom + " AND SPRES$INSTITUCION = " + insto + " AND SPRES$TIPO = 1";
                                                estado = dao.consulta(sql);
                                                if (estado == 1)
                                                {
                                                    resultado = dao.tabla;
                                                    if (resultado.Rows.Count > 0)
                                                    {

                                                        if (Convert.ToInt32(producto.Rows[i]["SPPRO$RESULTADO2"]) != -1)
                                                        {
                                                            sql = "SELECT * FROM SCHE$SIPLAN20.SP20$RESULTADOS WHERE SPRES$ID_RESULTADO = " + producto.Rows[i]["SPPRO$RESULTADO2"] + " AND SPRES$RESTRICTIVA = 'N' AND SPRES$POM = " + producto.Rows[i]["SPPRO$POM"] + " AND SPRES$INSTITUCION = " + producto.Rows[i]["SPPRO$INSTO"];
                                                            estado = dao.consulta(sql);
                                                            if (estado == 1)
                                                            {
                                                                resultados = dao.tabla;
                                                                if (resultados.Rows.Count > 0)
                                                                {
                                                                    tipo = Convert.ToInt32(resultados.Rows[0]["SPRES$TIPO"]);
                                                                    sql = "SELECT * FROM SCHE$SIPLAN20.SP20$RESULTADOS WHERE ";
                                                                    if (tipo == 0)
                                                                        sql = sql + " SPRES$COD_ESTRATEGICO= " + resultados.Rows[0]["SPRES$COD_ESTRATEGICO"];
                                                                    else if (tipo == 1)
                                                                        sql = sql + " SPRES$DESCRIPCION = " + resultados.Rows[0]["SPRES$DESCRIPCION"];
                                                                    else if (tipo == 2)
                                                                        sql = sql + " SPRES$COD_ESTRATEGICO= " + resultados.Rows[0]["SPRES$COD_ESTRATEGICO"];
                                                                    sql = sql + " AND SPRES$RESTRICTIVA = 'N' AND SPRES$POM = " + pom + " AND SPRES$INSTITUCION = " + insto + " AND SPRES$TIPO = " + tipo;

                                                                    estado = dao.consulta(sql);
                                                                    if (estado == 1)
                                                                    {
                                                                        tabla = dao.tabla;
                                                                        if (tabla.Rows.Count > 0)
                                                                            resultado2 = Convert.ToInt32(tabla.Rows[0]["SPRES$ID_RESULTADO"]);
                                                                        else
                                                                            resultado2 = -1;

                                                                    }
                                                                    else
                                                                        resultado2 = -1;
                                                                }
                                                                else
                                                                    resultado2 = -1;
                                                            }
                                                            else

                                                                resultado2 = -1;
                                                        }
                                                        else
                                                            resultado2 = -1;




                                                        sql = "INSERT INTO SCHE$SIPLAN20.SP20$PRODUCTO (SPPRO$DESCRIPCION, SPPRO$ID_MEDIDA, SPPRO$OBJETIVO_CENTRAL, SPPRO$ACCION_ESTRATEGICA, SPPRO$ID_RESULTADO, SPPRO$FECHA_INSERTA, SPPRO$PRESUPUESTO, SPPRO$POM, SPPRO$INSTO,SPPRO$RESULTADO2,SPPRO$ID_ANTERIOR) VALUES ('" + producto.Rows[i]["SPPRO$DESCRIPCION"] + "'," + producto.Rows[i]["SPPRO$ID_MEDIDA"];
                                                        if (producto.Rows[i]["SPPRO$OBJETIVO_CENTRAL"] == DBNull.Value)
                                                            sql = sql + ",NULL,NULL";
                                                        else
                                                            sql = sql + "," + producto.Rows[i]["SPPRO$OBJETIVO_CENTRAL"] + "," + producto.Rows[i]["SPPRO$ACCION_ESTRATEGICA"];
                                                        sql = sql + "," + resultado.Rows[0]["SPRES$ID_RESULTADO"] + ",'INSERT = '||TO_CHAR(SYSDATE,'DD/MM/YYYY HH24:MI')||' '||'" + Session["USUARIO"].ToString() + "'," + producto.Rows[i]["SPPRO$PRESUPUESTO"] + "," + pom + "," + insto + "," + resultado2 + ","+ producto.Rows[i]["SPPRO$ID_PRODUCTO"] + ")";
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
                                                                                sql = "INSERT INTO SCHE$SIPLAN20.SP20$SUB_PRODUCTO (SPPSUB$DESCRIPCION, SPPSUB$ID_MEDIDA, SPPSUB$SNIP, SPPSUB$ID_PRODUCTO,SPPSUB$FECHA_INSERTA,SPPSUB$PROPIETARIO,SPPSUB$ID_ANTERIOR) VALUES (";
                                                                                if (subproductos.Rows[j]["SPPSUB$SNIP"] == DBNull.Value)
                                                                                {
                                                                                    sql = sql + "'" + subproductos.Rows[j]["SPPSUB$DESCRIPCION"] + "'," + subproductos.Rows[j]["SPPSUB$ID_MEDIDA"] + ",NULL," + tempo.Rows[0]["SPPRO$ID_PRODUCTO"] + ",'INSERT = '||TO_CHAR(SYSDATE,'DD/MM/YYYY HH24:MI')||' '||'" + Session["USUARIO"].ToString() + "', '" + Session["USUARIO"].ToString() + "',"+ subproductos.Rows[j]["SPPSUB$ID_SUBPRODUCTO"] + ")";
                                                                                }

                                                                                else
                                                                                {
                                                                                    sql = sql + "NULL,NULL," + subproductos.Rows[j]["SPPSUB$SNIP"] + "," + tempo.Rows[0]["SPPRO$ID_PRODUCTO"] + ",'INSERT = '||TO_CHAR(SYSDATE,'DD/MM/YYYY HH24:MI')||' '||'" + Session["USUARIO"].ToString() + "', '" + Session["USUARIO"].ToString() + "',"+ subproductos.Rows[j]["SPPSUB$ID_SUBPRODUCTO"] + ")";
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



        protected void migraproductosRED(int pom, int insto)
        {
            int orden = -1;
            DataTable poms = new DataTable();
            DataTable producto = new DataTable();
            DataTable resultado = new DataTable();
            DataTable resultados = new DataTable();
            DataTable subproductos = new DataTable();
            DataTable tempo = new DataTable();
            int tipo = -1;
            int resultado2 = -1;

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
                                    sql = "SELECT P.SPPRO$ID_PRODUCTO, R.SPRES$COD_ESTRATEGICO RESULTADO, P.SPPRO$DESCRIPCION, P.SPPRO$ID_MEDIDA, P.SPPRO$OBJETIVO_CENTRAL, P.SPPRO$ACCION_ESTRATEGICA, P.SPPRO$ID_RESULTADO, P.SPPRO$PRESUPUESTO, P.SPPRO$POM, P.SPPRO$INSTO, P.SPPRO$RESULTADO2 FROM SCHE$SIPLAN20.SP20$PRODUCTO P INNER JOIN SCHE$SIPLAN20.SP20$RESULTADOS R ON P.SPPRO$ID_RESULTADO = R.SPRES$ID_RESULTADO AND P.SPPRO$RESTRICTIVA = 'N' AND R.SPRES$RESTRICTIVA = 'N'  WHERE P.SPPRO$POM = " + poms.Rows[0]["SPPO$ID_POM"] + " AND P.SPPRO$INSTO = " + poms.Rows[0]["SPPO$ID_INSTITUCION"] + " AND R.SPRES$TIPO = 2";
                                    estado = dao.consulta(sql);
                                    if (estado == 1)
                                    {
                                        producto = dao.tabla;
                                        if (producto.Rows.Count > 0)
                                        {
                                            for (int i = 0; i < producto.Rows.Count; i++)
                                            {
                                                //sql = "SELECT * FROM SCHE$SIPLAN20.SP20$RESULTADOS WHERE SPRES$DESCRIPCION = '" + producto.Rows[i]["RESULTADO"] + "' AND SPRES$RESTRICTIVA = 'N' AND SPRES$POM = " + pom + " AND SPRES$INSTITUCION = " + insto + " AND SPRES$TIPO = 2";
                                                sql = "SELECT * FROM SCHE$SIPLAN20.SP20$RESULTADOS WHERE SPRES$COD_ESTRATEGICO = '" + producto.Rows[i]["RESULTADO"] + "' AND SPRES$RESTRICTIVA = 'N' AND SPRES$POM = " + pom + " AND SPRES$INSTITUCION = " + insto + " AND SPRES$TIPO = 2";
                                                estado = dao.consulta(sql);
                                                if (estado == 1)
                                                {
                                                    resultado = dao.tabla;
                                                    if (resultado.Rows.Count > 0)
                                                    {

                                                        if (Convert.ToInt32(producto.Rows[i]["SPPRO$RESULTADO2"]) != -1)
                                                        {
                                                            sql = "SELECT * FROM SCHE$SIPLAN20.SP20$RESULTADOS WHERE SPRES$ID_RESULTADO = " + producto.Rows[i]["SPPRO$RESULTADO2"] + " AND SPRES$RESTRICTIVA = 'N' AND SPRES$POM = " + producto.Rows[i]["SPPRO$POM"] + " AND SPRES$INSTITUCION = " + producto.Rows[i]["SPPRO$INSTO"];
                                                            estado = dao.consulta(sql);
                                                            if (estado == 1)
                                                            {
                                                                resultados = dao.tabla;
                                                                if (resultados.Rows.Count > 0)
                                                                {
                                                                    tipo = Convert.ToInt32(resultados.Rows[0]["SPRES$TIPO"]);
                                                                    sql = "SELECT * FROM SCHE$SIPLAN20.SP20$RESULTADOS WHERE ";
                                                                    if (tipo == 0)
                                                                        sql = sql + " SPRES$COD_ESTRATEGICO= " + resultados.Rows[0]["SPRES$COD_ESTRATEGICO"];
                                                                    else if (tipo == 1)
                                                                        sql = sql + " SPRES$DESCRIPCION = " + resultados.Rows[0]["SPRES$DESCRIPCION"];
                                                                    else if (tipo == 2)
                                                                        sql = sql + " SPRES$COD_ESTRATEGICO= " + resultados.Rows[0]["SPRES$COD_ESTRATEGICO"];
                                                                    sql = sql + " AND SPRES$RESTRICTIVA = 'N' AND SPRES$POM = " + pom + " AND SPRES$INSTITUCION = " + insto + " AND SPRES$TIPO = " + tipo;

                                                                    estado = dao.consulta(sql);
                                                                    if (estado == 1)
                                                                    {
                                                                        tabla = dao.tabla;
                                                                        if (tabla.Rows.Count > 0)
                                                                            resultado2 = Convert.ToInt32(tabla.Rows[0]["SPRES$ID_RESULTADO"]);
                                                                        else
                                                                            resultado2 = -1;

                                                                    }
                                                                    else
                                                                        resultado2 = -1;
                                                                }
                                                                else
                                                                    resultado2 = -1;
                                                            }
                                                            else

                                                                resultado2 = -1;
                                                        }
                                                        else
                                                            resultado2 = -1;


                                                        sql = "INSERT INTO SCHE$SIPLAN20.SP20$PRODUCTO (SPPRO$DESCRIPCION, SPPRO$ID_MEDIDA, SPPRO$OBJETIVO_CENTRAL, SPPRO$ACCION_ESTRATEGICA, SPPRO$ID_RESULTADO, SPPRO$FECHA_INSERTA, SPPRO$PRESUPUESTO, SPPRO$POM, SPPRO$INSTO,SPPRO$RESULTADO2,SPPRO$ID_ANTERIOR) VALUES ('" + producto.Rows[i]["SPPRO$DESCRIPCION"] + "'," + producto.Rows[i]["SPPRO$ID_MEDIDA"];
                                                        if (producto.Rows[i]["SPPRO$OBJETIVO_CENTRAL"] == DBNull.Value)
                                                            sql = sql + ",NULL,NULL";
                                                        else
                                                            sql = sql + "," + producto.Rows[i]["SPPRO$OBJETIVO_CENTRAL"] + "," + producto.Rows[i]["SPPRO$ACCION_ESTRATEGICA"];
                                                        sql = sql + "," + resultado.Rows[0]["SPRES$ID_RESULTADO"] + ",'INSERT = '||TO_CHAR(SYSDATE,'DD/MM/YYYY HH24:MI')||' '||'" + Session["USUARIO"].ToString() + "'," + producto.Rows[i]["SPPRO$PRESUPUESTO"] + "," + pom + "," + insto + "," + resultado2 + ","+ producto.Rows[i]["SPPRO$ID_PRODUCTO"] + ")";
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
                                                sql = sql + " INNER JOIN SINIP.SNTBCG$UNIDAD_MEDIDA UM ON PO.SPPRO$ID_MEDIDA = UM.SNCGUM$UNIDAD_MEDIDA WHERE R.SPRES$POM = " + pom + " AND R.SPRES$INSTITUCION = " + insto + " AND R.SPRES$TIPO = 2 ORDER BY R.SPRES$ID_RESULTADO, PR.SPPRO$ID_PROGRAMA_PRESUPUESTO, PO.SPPRO$ID_PRODUCTO ASC";

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
                                                                                sql = "INSERT INTO SCHE$SIPLAN20.SP20$SUB_PRODUCTO (SPPSUB$DESCRIPCION, SPPSUB$ID_MEDIDA, SPPSUB$SNIP, SPPSUB$ID_PRODUCTO,SPPSUB$FECHA_INSERTA,SPPSUB$PROPIETARIO,SPPSUB$ID_ANTERIOR) VALUES (";
                                                                                if (subproductos.Rows[j]["SPPSUB$SNIP"] == DBNull.Value)
                                                                                {
                                                                                    sql = sql + "'" + subproductos.Rows[j]["SPPSUB$DESCRIPCION"] + "'," + subproductos.Rows[j]["SPPSUB$ID_MEDIDA"] + ",NULL," + tempo.Rows[0]["SPPRO$ID_PRODUCTO"] + ",'INSERT = '||TO_CHAR(SYSDATE,'DD/MM/YYYY HH24:MI')||' '||'" + Session["USUARIO"].ToString() + "', '" + Session["USUARIO"].ToString() + "',"+ subproductos.Rows[j]["SPPSUB$ID_SUBPRODUCTO"] + ")";
                                                                                }

                                                                                else
                                                                                {
                                                                                    sql = sql + "NULL,NULL," + subproductos.Rows[j]["SPPSUB$SNIP"] + "," + tempo.Rows[0]["SPPRO$ID_PRODUCTO"] + ",'INSERT = '||TO_CHAR(SYSDATE,'DD/MM/YYYY HH24:MI')||' '||'" + Session["USUARIO"].ToString() + "', '" + Session["USUARIO"].ToString() + "',"+ subproductos.Rows[j]["SPPSUB$ID_SUBPRODUCTO"] + ")";
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


        protected void cargaResultadoInsto(int pom, int insto, int resultado)
        {
            sql = "SELECT * FROM SCHE$SIPLAN20.SP20$RESULTADOS R WHERE R.SPRES$POM = " + pom + " AND R.SPRES$INSTITUCION = " + insto + " AND R.SPRES$TIPO = 1 AND R.SPRES$RESTRICTIVA = 'N'";
            estado = dao.consulta(sql);
            if (estado == 0)
            {
                mensaje = dao.mensaje;
                Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
            }
            else
            {
                tabla = dao.tabla;
                cbResultados.DataSource = tabla;
                cbResultados.ValueField = "SPRES$ID_RESULTADO";
                cbResultados.TextField = "SPRES$DESCRIPCION";
                cbResultados.DataBind();

                if (resultado != -1)
                    cbResultados.Value = resultado.ToString();
                else
                    cbResultados.SelectedIndex = -1;

            }
        }


        protected void cargaProgramaPresu(int pom, int insto)
        {
            sql = "SELECT P.SPPRO$ID_PROGRAMA_PRESUPUESTO, CASE WHEN ROUND(P.SPPRO$ID_PROGRAMA_PRESUPUESTO,0) = 0 THEN '0'||P.SPPRO$ID_PROGRAMA_PRESUPUESTO|| '-' || P.SPPRO$DESCRIPCION ELSE P.SPPRO$ID_PROGRAMA_PRESUPUESTO|| '-' || P.SPPRO$DESCRIPCION END AS PROGRAMA FROM SCHE$SIPLAN20.SP20$PROGRAMA_PRESUPUESTARIO P WHERE P.SPPRO$ID_POM = " + pom + " AND P.SPPRO$ID_INSTITUCION = " + insto + " AND P.SPPRO$ID_PROGRAMA_DEPENDE IS NULL AND P.SPPRO$RESTRICTIVA = 'N' ORDER BY P.SPPRO$ID_PROGRAMA_PRESUPUESTO ASC";
            estado = dao.consulta(sql);
            if (estado == 0)
            {
                mensaje = dao.mensaje;
                Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
            }
            else
            {
                tabla = dao.tabla;
                cbProgramaPresupuestario.DataSource = tabla;
                cbProgramaPresupuestario.ValueField = "SPPRO$ID_PROGRAMA_PRESUPUESTO";
                cbProgramaPresupuestario.TextField = "PROGRAMA";
                cbProgramaPresupuestario.DataBind();



            }
        }

        protected void cargaUnidades()
        {
            sql = "SELECT a.sncgum$unidad_medida, a.sncgum$nombre FROM sinip.sntbcg$unidad_medida a WHERE a.sncgum$unidad_medida >= 1000 AND a.sncgum$sigla IS NOT NULL ORDER BY a.sncgum$nombre ASC";
            estado = dao.consulta(sql);
            if (estado == 0)
            {
                mensaje = dao.mensaje;
                Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
            }
            else
            {
                tabla = dao.tabla;
                cbUnidadMedida.DataSource = tabla;
                cbUnidadMedida.ValueField = "sncgum$unidad_medida";
                cbUnidadMedida.TextField = "sncgum$nombre";
                cbUnidadMedida.DataBind();



            }
        }

        protected void cargaPilares()
        {
            sql = "SELECT R.SPPRES$ID_RESULTADO, R.SPPRES$CODIGO||'-'||R.SPPRES$DESCRIPCION PILAR FROM SCHE$SIPLAN20.SP20$RESULTADOS_ESTRATEGICOS R WHERE R.SPPRES$NIVEL = 1 AND R.SPPRES$DEPENDE IS NULL AND R.SPPRES$RESTRICTIVA = 'N' ";
            estado = dao.consulta(sql);
            if (estado == 0)
            {
                mensaje = dao.mensaje;
                Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
            }
            else
            {
                tabla = dao.tabla;
                cpPilarPGG.DataSource = tabla;
                cpPilarPGG.ValueField = "SPPRES$ID_RESULTADO";
                cpPilarPGG.TextField = "PILAR";
                cpPilarPGG.DataBind();
            }
        }
        protected void cbProgramaPresupuestario_ValueChanged(object sender, EventArgs e)
        {
            DataTable esAdmino = new DataTable();
            sql = "SELECT  P.SPPRO$ID_PROGRAMA_PRESUPUESTO, P.SPPRO$ID_PROGRAMA_PRESUPUESTO||'-'||P.SPPRO$DESCRIPCION PROGRAMA FROM SCHE$SIPLAN20.SP20$PROGRAMA_PRESUPUESTARIO P WHERE P.SPPRO$ID_PROGRAMA_DEPENDE = " + Convert.ToDouble(cbProgramaPresupuestario.Value) + " AND P.SPPRO$ID_POM = " + Convert.ToInt32(Session["pom"]) + " AND SPPRO$ID_INSTITUCION = " + Convert.ToInt32(Session["insto"]) + " AND P.SPPRO$RESTRICTIVA = 'N'";
            estado = dao.consulta(sql);
            if (estado == 0)
            {
                mensaje = dao.mensaje;
                Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
            }
            else
            {
                tabla = dao.tabla;

                sql = "SELECT SPPRO$ES_ADMINISTRATIVO FROM SCHE$SIPLAN20.SP20$PROGRAMA_PRESUPUESTARIO WHERE SPPRO$ID_PROGRAMA_PRESUPUESTO = " + cbProgramaPresupuestario.Value + " AND SPPRO$ID_POM = " + Convert.ToInt32(Session["pom"]) + " AND SPPRO$ID_INSTITUCION = " + Convert.ToInt32(Session["insto"]) + " AND SPPRO$RESTRICTIVA = 'N'";
                estado = dao.consulta(sql);
                if (estado == 1)
                {
                    esAdmino = dao.tabla;
                    if (esAdmino.Rows.Count > 0)
                    {
                        esProdAdministartivo.Value = esAdmino.Rows[0]["SPPRO$ES_ADMINISTRATIVO"].ToString();
                        if (Convert.ToInt32(esProdAdministartivo.Value) == 1)
                        {
                            panPGGProductos.Style.Add("display", "none");
                            cbPGG.SelectedIndex = -1;
                            cbPGG.Enabled = true;
                        }
                        else if (Convert.ToInt32(esProdAdministartivo.Value) == 0)
                        {
                            cbPGG.SelectedIndex = 1;
                            cbPGG.Enabled = false;
                            panPGGProductos.Style.Add("display", "block");
                        }

                    }
                }

              
                if (tabla.Rows.Count > 0)
                {
                    cbSuProgramaPresupuestario.DataSource = tabla;                  
                    cbSuProgramaPresupuestario.ValueField = "SPPRO$ID_PROGRAMA_PRESUPUESTO";
                    cbSuProgramaPresupuestario.TextField = "PROGRAMA";
                    cbSuProgramaPresupuestario.DataBind();
                    if (cbSuprograma.SelectedIndex == 1)
                    {
                        subProgramaPresupuestario.Style.Add("display", "block");
                        cbSuProgramaPresupuestario.SelectedIndex = -1;
                    }

                }
                
                //MultiView1.ActiveViewIndex = 4;
            }
        }


        protected void cpPilarPGG_ValueChanged(object sender, EventArgs e)
        {
            sql = "SELECT R.SPPRES$ID_RESULTADO, R.SPPRES$CODIGO||'-'||R.SPPRES$DESCRIPCION OBJETIVO FROM SCHE$SIPLAN20.SP20$RESULTADOS_ESTRATEGICOS R WHERE R.SPPRES$NIVEL = 3 AND R.SPPRES$DEPENDE = " + Convert.ToInt32(cpPilarPGG.Value) + " AND R.SPPRES$RESTRICTIVA = 'N' ";
            estado = dao.consulta(sql);
            if (estado == 0)
            {
                mensaje = dao.mensaje;
                Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
            }
            else
            {
                tabla = dao.tabla;
                cbObjSectorial.DataSource = tabla;
                cbObjSectorial.ValueField = "SPPRES$ID_RESULTADO";
                cbObjSectorial.TextField = "OBJETIVO";
                cbObjSectorial.DataBind();
                AccionEstrategica.Style.Add("display", "block");
                //MultiView1.ActiveViewIndex = 4;
                cbObjSectorial.SelectedIndex = -1;
                CbAccionEstrategica.SelectedIndex = -1;
                cbObjSectorial.Focus();
            }
        }

        protected void cbObjSectorial_ValueChanged(object sender, EventArgs e)
        {
            sql = "SELECT R.SPPRES$ID_RESULTADO, R.SPPRES$CODIGO||'-'||R.SPPRES$DESCRIPCION ACCION FROM SCHE$SIPLAN20.SP20$RESULTADOS_ESTRATEGICOS R WHERE R.SPPRES$NIVEL = 4 AND R.SPPRES$DEPENDE = " + Convert.ToInt32(cbObjSectorial.Value) + " AND R.SPPRES$RESTRICTIVA = 'N' ";
            estado = dao.consulta(sql);
            if (estado == 0)
            {
                mensaje = dao.mensaje;
                Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
            }
            else
            {
                tabla = dao.tabla;
                CbAccionEstrategica.DataSource = tabla;
                CbAccionEstrategica.ValueField = "SPPRES$ID_RESULTADO";
                CbAccionEstrategica.TextField = "ACCION";
                CbAccionEstrategica.DataBind();
                AccionEstrategica.Style.Add("display", "block");
                //MultiView1.ActiveViewIndex = 4;
                CbAccionEstrategica.SelectedIndex = -1;
                CbAccionEstrategica.Focus();
            }
        }

        protected void btnGrabaProdInsto_Click(object sender, EventArgs e)
        {
            int codigo;
            int municipios = 0;
            int responsable = -1;
            int resulta1 = -1;
            int resulta2 = -1;
            int resultado = -1;

            int red_producto = -1;
            DataTable red_resultado = new DataTable();

            if (Session["tipo"].ToString() == "institucional")
            {
                if (Convert.ToInt32(Session["operacion"]) == 2)
                {
                    //if ((gvProdInsto.GetRowValues(gvProdInsto.FocusedRowIndex, "SPPRO$PROPIETARIO").ToString() == Session["USUARIO"].ToString()) || Session["ROL"].ToString() == "ADMIN")
                    if (Session["ROL"].ToString() != "ENTIDAD")
                    {
                        codigo = Convert.ToInt32(gvProdInsto.GetRowValues(gvProdInsto.FocusedRowIndex, "SPPRO$ID_PRODUCTO").ToString());
                        sql = "SELECT * FROM SCHE$SIPLAN20.SP20$SUB_PRODUCTO S WHERE S.SPPSUB$ID_PRODUCTO = " + codigo + " AND S.SPPSUB$RESTRICTIVA = 'N'";

                        estado = dao.consulta(sql);
                        if (estado == 0)
                        {
                            mensaje = dao.mensaje;
                            Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
                            //cbResultadoInsitucional.SelectedIndex = -1;
                            cbProgramaPresupuestario.SelectedIndex = -1;
                            cbSuprograma.SelectedIndex = -1;
                            subProgramaPresupuestario.Style.Add("display", "none");
                            cbSuProgramaPresupuestario.SelectedIndex = -1;
                            txtProductoInsto.Text = "";
                            cbUnidadMedida.SelectedIndex = -1;
                            rbaccion.SelectedIndex = -1;
                            AccionEstrategica.Style.Add("display", "none");
                            cpPilarPGG.SelectedIndex = -1;
                            cbObjSectorial.SelectedIndex = -1;
                            CbAccionEstrategica.SelectedIndex = -1;
                            cargaProductosInsto(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]));
                            cargaResultadoInsto(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), -1);
                            cargaProgramaPresu(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]));
                            cargaUnidades();
                            cargaPilares();
                            rbProductos.SelectedIndex = 0;
                            ProdInsitucionales.Style.Add("display", "block");
                            MultiView1.ActiveViewIndex = 4;
                        }
                        else
                        {
                            tabla = dao.tabla;

                            sql = "UPDATE SCHE$SIPLAN20.SP20$PRODUCTO SET SPPRO$RESTRICTIVA = 'S', SPPRO$FECHA_DELETE = 'DELETE = '||TO_CHAR(SYSDATE,'DD/MM/YYYY HH24:MI')||' '||'" + Session["USUARIO"].ToString() + "' WHERE SPPRO$ID_PRODUCTO = " + codigo + " AND SPPRO$POM = " + Convert.ToInt32(Session["pom"]) + " AND SPPRO$INSTO = " + Convert.ToInt32(Session["insto"]);
                            estado = dao.comando(sql);
                            if (estado == 0)
                            {
                                mensaje = dao.mensaje;
                                Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
                                //cbResultadoInsitucional.SelectedIndex = -1;
                                cbProgramaPresupuestario.SelectedIndex = -1;
                                cbSuprograma.SelectedIndex = -1;
                                subProgramaPresupuestario.Style.Add("display", "none");
                                cbSuProgramaPresupuestario.SelectedIndex = -1;
                                txtProductoInsto.Text = "";
                                cbUnidadMedida.SelectedIndex = -1;
                                rbaccion.SelectedIndex = -1;
                                AccionEstrategica.Style.Add("display", "none");
                                cpPilarPGG.SelectedIndex = -1;
                                cbObjSectorial.SelectedIndex = -1;
                                CbAccionEstrategica.SelectedIndex = -1;
                                cargaProductosInsto(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]));
                                cargaResultadoInsto(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), -1);
                                cargaProgramaPresu(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]));
                                cargaUnidades();
                                cargaPilares();
                                rbProductos.SelectedIndex = 0;
                                ProdInsitucionales.Style.Add("display", "block");
                                MultiView1.ActiveViewIndex = 4;
                            }
                            else
                            {

                                if (tabla.Rows.Count > 0)
                                {
                                    for (int i = 0; i < tabla.Rows.Count; i++)
                                    {
                                        sql = "UPDATE SCHE$SIPLAN20.SP20$SUB_PRODUCTO SET SPPSUB$RESTRICTIVA = 'S', SPPSUB$FECHA_DELETE = 'DELETE = '||TO_CHAR(SYSDATE,'DD/MM/YYYY HH24:MI')||' '||'" + Session["USUARIO"].ToString() + "' WHERE  SPPSUB$ID_PRODUCTO =  " + tabla.Rows[i]["SPPSUB$ID_PRODUCTO"];
                                        estado = dao.comando(sql);
                                        if (estado == 0)
                                            break;
                                        sql = "UPATE SCHE$SIPLAN20.SP20$PROGRAMETA_FI_FIN_SUB SET SPPMFS$RESTRICTIVA = 'N', SPPMFS$FECHA_DELETE = 'DELETE = '||TO_CHAR(SYSDATE,'DD/MM/YYYY HH24:MI')||' '||'" + Session["USUARIO"].ToString() + "' WHERE SPPMFS$ID_SUBPRODUCTO = " + tabla.Rows[i]["SPPSUB$ID_SUBPRODUCTO"];
                                        estado = dao.comando(sql);
                                        if (estado == 0)
                                            break;

                                        sql = "UPDATE SCHE$SIPLAN20.SP20$POASUBPRODUCTOS SET SPOAS$RESTRICTIVA = 'S', SPOAS$FECHA_DELETE = 'DELETE = '||TO_CHAR(SYSDATE,'DD/MM/YYYY HH24:MI')||' '||'" + Session["USUARIO"].ToString() + "' WHERE SPOAS$SUBPRODUCTO = " + tabla.Rows[i]["SPPSUB$ID_SUBPRODUCTO"]; ;
                                        estado = dao.comando(sql);
                                        if (estado == 0)
                                            break;
                                    }
                                }

                                sql = "UPDATE SCHE$SIPLAN20.SP20$PROGRAMACION_MFPRODUCTO SET SPPMFS$RESTRICTIVA = 'S', SPPMFS$FECHA_DELETE = 'DELETE = '||TO_CHAR(SYSDATE,'DD/MM/YYYY HH24:MI')||' '||'" + Session["USUARIO"].ToString() + "' WHERE SPPMFS$ID_PRODUCTO = " + codigo;
                                estado = dao.comando(sql);

                                sql = "UPDATE SCHE$SIPLAN20.SP20$POAPRODUCTOS SET SPPR$RESTRICTIVA = 'S', SPPR$FECHA_DELETE = 'DELETE = '||TO_CHAR(SYSDATE,'DD/MM/YYYY HH24:MI')||' '||'" + Session["USUARIO"].ToString() + "' WHERE SPPR$PRODUCTO = " + codigo;
                                estado = dao.comando(sql);


                                sql = "UPDATE SCHE$SIPLAN20.SP20$MUNOSPRIORIZADOS SET RESTRICTIVA = 'S', FECHA_BORRADO = 'BORRADO = '||TO_CHAR(SYSDATE,'DD/MM/YYYY HH24:MI')||' '||'" + Session["USUARIO"].ToString() + "' WHERE SPPRO$ID_PRODUCTO = " + codigo;
                                estado = dao.comando(sql);



                                mensaje = "Producto eliminado correctamente";
                                Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',1);", true);
                                //cbResultadoInsitucional.SelectedIndex = -1;
                                cbProgramaPresupuestario.SelectedIndex = -1;
                                cbSuprograma.SelectedIndex = -1;
                                subProgramaPresupuestario.Style.Add("display", "none");
                                cbSuProgramaPresupuestario.SelectedIndex = -1;
                                txtProductoInsto.Text = "";
                                cbUnidadMedida.SelectedIndex = -1;
                                rbaccion.SelectedIndex = -1;
                                AccionEstrategica.Style.Add("display", "none");
                                cpPilarPGG.SelectedIndex = -1;
                                cbObjSectorial.SelectedIndex = -1;
                                CbAccionEstrategica.SelectedIndex = -1;
                                cargaProductosInsto(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]));
                                cargaResultadoInsto(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), -1);
                                cargaProgramaPresu(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]));
                                cargaUnidades();
                                cargaPilares();
                                rbProductos.SelectedIndex = 0;
                                ProdInsitucionales.Style.Add("display", "block");
                                MultiView1.ActiveViewIndex = 4;
                            }
                            //}
                        }


                    }

                    else
                    {
                        mensaje = "Su perfil de usuario no le permite eliminar este producto";
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
                    }
                }

                else
                {

                    responsable = busca_responsable(Convert.ToInt32(Session["insto"]));

                    if (responsable == 1)
                    {
                        if (cbPGG.SelectedIndex == 0)
                        {
                            if (cbResultados.SelectedIndex != -1)
                                resulta1 = Convert.ToInt32(cbResultados.Value);
                        }
                        else if (cbAcciones.SelectedIndex == -1 && cbPilar.SelectedIndex != -1 && cbResultados.SelectedIndex != -1)
                        {
                            resulta2 = Convert.ToInt32(cbResultados.Value);
                            resulta1 = Convert.ToInt32(cbPilar.Value);
                        }

                        else if (cbAcciones.SelectedIndex != -1)
                        {
                            resulta1 = Convert.ToInt32(cbAcciones.Value);
                            if (cbResultados.SelectedIndex != -1)
                                resulta2 = Convert.ToInt32(cbResultados.Value);
                        }
                        /*else if (cbPilar.SelectedIndex != -1 && cbAcciones.SelectedIndex == -1 && cbResultados.SelectedIndex != -1)
                        {
                            resulta1 = Convert.ToInt32(cbResultados.Value);
                            resulta2 = Convert.ToInt32(cbPilar.Value);
                        }
                        */
                        else if (cbPilar.SelectedIndex != -1 && cbAcciones.SelectedIndex == -1 && cbResultados.SelectedIndex == -1)

                            resulta1 = Convert.ToInt32(cbPilar.Value);



                    }

                    else
                    {
                        if (cbPGG.SelectedIndex == 0)
                        {
                            if (cbResultados.SelectedIndex != -1)
                                resulta1 = Convert.ToInt32(cbResultados.Value);
                        }

                        else if (cbPilar.SelectedIndex == -1 && cbResultados.SelectedIndex != -1)
                            resulta1 = Convert.ToInt32(cbResultados.Value);
                        else if (cbPilar.SelectedIndex != -1)
                        {
                            resulta1 = Convert.ToInt32(cbPilar.Value);
                            if (cbResultados.SelectedIndex != -1)
                                resulta2 = Convert.ToInt32(cbResultados.Value);
                        }

                    }






                    /*
                    if (cbResultadoInsitucional.SelectedIndex == -1)
                    {
                        mensaje = "Seleccione el resultado institucional";
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
                        cbResultadoInsitucional.Focus();
                    }
                    */
                    if (cbPGG.SelectedIndex == - 1)
                    {
                        mensaje = "Conteste la pregunta ¿Este producto estará vinculado a la PGG 2024-2028:? ";
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
                        cbPGG.Focus();

                    }
                    else if (cbProgramaPresupuestario.SelectedIndex == -1)
                    {
                        mensaje = "Seleccione el programa presupuestario";
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
                        cbProgramaPresupuestario.Focus();
                    }

                    else if (cbSuprograma.SelectedIndex == -1)
                    {

                        mensaje = "Conteste la pregunta ¿Necesita vincular el producto a un subprograma presupuestario:?";
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
                        cbSuprograma.Focus();
                    }

                    else if (cbSuprograma.SelectedIndex == 1 && cbSuProgramaPresupuestario.SelectedIndex == -1)
                    {
                        mensaje = "Esta vinculando a un suprograma presupuestario, la descripción de subprograma es necesario";
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
                        cbSuProgramaPresupuestario.Focus();
                    }
                    else if (txtProductoInsto.Text == "")
                    {
                        mensaje = "La descripción del producto es necesaria";
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
                        txtProductoInsto.Focus();
                    }

                    else if (cbUnidadMedida.SelectedIndex == -1)
                    {
                        mensaje = "La unidad de medida del producto es necesaria";
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
                        cbUnidadMedida.Focus();
                    }
                    else if (resulta1 == -1 || resulta2 == -1 && Convert.ToInt32(esProdAdministartivo.Value) == 0)
                    {
                        mensaje = "Debe vincular este producto a un Eje Estratégico/Meta Presidencial, como a un resultado institucional";
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
                        cbUnidadMedida.Focus();
                    }



                    else
                    {
                        if (Convert.ToInt32(Session["operacion"]) == 0)
                        {

                            if (responsable == 1)
                            {
                                sql = "SELECT *  FROM SCHE$SIPLAN20.SPPSVST$PGG2024_2028 WHERE ID_ACCION = " + resulta1;
                                estado = dao.consulta(sql);
                                if (estado == 1)
                                {
                                    tabla = dao.tabla;
                                    if (tabla.Rows.Count > 0)
                                    {
                                        sql = "SELECT * FROM SCHE$SIPLAN20.SP20$RESULTADOS WHERE SPRES$COD_ESTRATEGICO = " + tabla.Rows[0]["ID_ACCION"] + " AND SPRES$TIPO = 0 AND SPRES$RESTRICTIVA = 'N' AND SPRES$POM = " + Convert.ToInt32(Session["pom"]) + " AND SPRES$INSTITUCION = " + Convert.ToInt32(Session["insto"]);
                                        estado = dao.consulta(sql);
                                        if (estado == 1)
                                        {
                                            tabla = dao.tabla;
                                            if (tabla.Rows.Count > 0)
                                                resultado = Convert.ToInt32(tabla.Rows[0]["SPRES$ID_RESULTADO"]);
                                            else
                                            {
                                                sql = "INSERT INTO SCHE$SIPLAN20.SP20$RESULTADOS(SPRES$TIPO, SPRES$COD_ESTRATEGICO, SPRES$POM, SPRES$INSTITUCION, SPRES$FECHA_INSERT, SPRES$PROPIETARIO) VALUES (0, " + resulta1 + ", " + Convert.ToInt32(Session["pom"]) + ", " + Convert.ToInt32(Session["insto"]) + ", 'INSERT = '||TO_CHAR(SYSDATE,'DD/MM/YYYY HH24:MI')||' '||'" + Session["USUARIO"].ToString() + "', '" + Session["USUARIO"].ToString() + "')";
                                                estado = dao.comando(sql);
                                                if (estado == 1)
                                                {
                                                    sql = "SELECT MAX(SPRES$ID_RESULTADO) ID FROM SCHE$SIPLAN20.SP20$RESULTADOS WHERE SPRES$COD_ESTRATEGICO = " + resulta1 + " AND SPRES$RESTRICTIVA = 'N' AND SPRES$POM = " + Convert.ToInt32(Session["pom"]) + " AND SPRES$INSTITUCION = " + Convert.ToInt32(Session["insto"]) + " AND SPRES$TIPO = 0";
                                                    estado = dao.consulta(sql);
                                                    if (estado == 1)
                                                    {
                                                        tabla = dao.tabla;
                                                        if (tabla.Rows.Count > 0)
                                                        {
                                                            resultado = Convert.ToInt32(tabla.Rows[0]["ID"]);
                                                        }
                                                        else
                                                            resultado = resulta1;
                                                    }
                                                }

                                            }

                                        }
                                    }

                                    else if (tabla.Rows.Count <= 0)
                                    {
                                        sql = @" SELECT
                                                    RG.SPRPG$ID_PGG ID_EJE
                                                    ,R.SPPRES$DESCRIPCION EJE                         
                                                    FROM SCHE$SIPLAN20.SP20$RESULTADOS_ESTRATEGICOS R 
                                                    INNER JOIN SCHE$SIPLAN20.SP20$RESPONSABLESPGG RG ON RG.SPRPG$ID_PGG = R.SPPRES$ID_RESULTADO AND RG.SPRPG$NIVEL  = 1 AND R.SPPRES$RESTRICTIVA = 'N' AND RG.SPRPG$RESTRICTIVA = 'N'
                                                    WHERE RG.SPRPG$RESPONSABLE = " + Convert.ToInt32(Session["insto"]) + @" AND RG.SPRPG$ID_PGG = " + resulta1;
                                        estado = dao.consulta(sql);
                                        if (estado == 1)
                                        {
                                            tabla = dao.tabla;
                                            if (tabla.Rows.Count > 0)
                                            {
                                                sql = "SELECT * FROM SCHE$SIPLAN20.SP20$RESULTADOS WHERE SPRES$COD_ESTRATEGICO = " + tabla.Rows[0]["ID_EJE"] + " AND SPRES$TIPO = 0 AND SPRES$RESTRICTIVA = 'N' AND SPRES$POM = " + Convert.ToInt32(Session["pom"]) + " AND SPRES$INSTITUCION = " + Convert.ToInt32(Session["insto"]);
                                                estado = dao.consulta(sql);

                                                if (estado == 1)
                                                {
                                                    tabla = dao.tabla;
                                                    if (tabla.Rows.Count > 0)
                                                        resultado = Convert.ToInt32(tabla.Rows[0]["SPRES$ID_RESULTADO"]);
                                                    else
                                                    {
                                                        sql = "INSERT INTO SCHE$SIPLAN20.SP20$RESULTADOS(SPRES$TIPO, SPRES$COD_ESTRATEGICO, SPRES$POM, SPRES$INSTITUCION, SPRES$FECHA_INSERT, SPRES$PROPIETARIO) VALUES (0, " + resulta1 + ", " + Convert.ToInt32(Session["pom"]) + ", " + Convert.ToInt32(Session["insto"]) + ", 'INSERT = '||TO_CHAR(SYSDATE,'DD/MM/YYYY HH24:MI')||' '||'" + Session["USUARIO"].ToString() + "', '" + Session["USUARIO"].ToString() + "')";
                                                        estado = dao.comando(sql);
                                                        if (estado == 1)
                                                        {
                                                            sql = "SELECT MAX(SPRES$ID_RESULTADO) ID FROM SCHE$SIPLAN20.SP20$RESULTADOS WHERE SPRES$COD_ESTRATEGICO = " + resulta1 + " AND SPRES$RESTRICTIVA = 'N' AND SPRES$POM = " + Convert.ToInt32(Session["pom"]) + " AND SPRES$INSTITUCION = " + Convert.ToInt32(Session["insto"]) + " AND SPRES$TIPO = 0";
                                                            estado = dao.consulta(sql);
                                                            if (estado == 1)
                                                            {
                                                                tabla = dao.tabla;
                                                                if (tabla.Rows.Count > 0)
                                                                {
                                                                    resultado = Convert.ToInt32(tabla.Rows[0]["ID"]);
                                                                }
                                                                else
                                                                    resultado = resulta1;
                                                            }
                                                        }

                                                    }

                                                }

                                            }
                                            else
                                                resultado = resulta1;
                                        }


                                    }

                                    else
                                        resultado = resulta1;
                                }



                            }

                            else if (responsable == 0)

                            {

                                //sql = "SELECT *  FROM SCHE$SIPLAN20.SPPSVST$PGG2024_2028 WHERE ID_EJE = " + resulta1;
                                sql = @"SELECT 
                          ID_EJE
                           ,EJE
                           FROM
                            (SELECT ID_EJE, EJE FROM SCHE$SIPLAN20.SPPSVST$PGG2024_2028
                            UNION
                            SELECT R.SPPRES$ID_RESULTADO ID_EJE, R.SPPRES$CODIGO || '-' || R.SPPRES$DESCRIPCION EJE FROM SCHE$SIPLAN20.SP20$RESULTADOS_ESTRATEGICOS R
                            INNER JOIN SCHE$SIPLAN20.S20$PERIODO_RESULTADO PR ON PR.SPR$ID_RESULTADO = R.SPPRES$ID_RESULTADO AND PR.SPR$RESTRICTIVA = 'N' AND R.SPPRES$RESTRICTIVA = 'N'
                            INNER JOIN SCHE$SIPLAN20.SP20$PERIODO_GOBIERNO PG ON PG.SPG$ID_PERIODO = PR.SPR$ID_PERIODO AND PG.SPG$RESTRICTIVA = 'N' AND PR.SPR$RESTRICTIVA = 'N' AND PG.SPG$VIGENTE = 1) WHERE ID_EJE = " + resulta1;
                                estado = dao.consulta(sql);
                                if (estado == 1)
                                {
                                    tabla = dao.tabla;
                                    if (tabla.Rows.Count > 0)
                                    {
                                        sql = "SELECT * FROM SCHE$SIPLAN20.SP20$RESULTADOS WHERE SPRES$COD_ESTRATEGICO = " + tabla.Rows[0]["ID_EJE"] + " AND SPRES$TIPO = 0 AND SPRES$RESTRICTIVA = 'N' AND SPRES$POM = " + Convert.ToInt32(Session["pom"]) + " AND SPRES$INSTITUCION = " + Convert.ToInt32(Session["insto"]);
                                        estado = dao.consulta(sql);
                                        if (estado == 1)
                                        {
                                            tabla = dao.tabla;
                                            if (tabla.Rows.Count > 0)
                                                resultado = Convert.ToInt32(tabla.Rows[0]["SPRES$ID_RESULTADO"]);
                                            else
                                            {
                                                sql = "INSERT INTO SCHE$SIPLAN20.SP20$RESULTADOS(SPRES$TIPO, SPRES$COD_ESTRATEGICO, SPRES$POM, SPRES$INSTITUCION, SPRES$FECHA_INSERT, SPRES$PROPIETARIO) VALUES (0, " + resulta1 + ", " + Convert.ToInt32(Session["pom"]) + ", " + Convert.ToInt32(Session["insto"]) + ", 'INSERT = '||TO_CHAR(SYSDATE,'DD/MM/YYYY HH24:MI')||' '||'" + Session["USUARIO"].ToString() + "', '" + Session["USUARIO"].ToString() + "')";
                                                estado = dao.comando(sql);
                                                if (estado == 1)
                                                {
                                                    sql = "SELECT MAX(SPRES$ID_RESULTADO) ID FROM SCHE$SIPLAN20.SP20$RESULTADOS WHERE SPRES$COD_ESTRATEGICO = " + resulta1 + " AND SPRES$RESTRICTIVA = 'N' AND SPRES$POM = " + Convert.ToInt32(Session["pom"]) + " AND SPRES$INSTITUCION = " + Convert.ToInt32(Session["insto"]) + " AND SPRES$TIPO = 0";
                                                    estado = dao.consulta(sql);
                                                    if (estado == 1)
                                                    {
                                                        tabla = dao.tabla;
                                                        if (tabla.Rows.Count > 0)
                                                        {
                                                            resultado = Convert.ToInt32(tabla.Rows[0]["ID"]);
                                                        }
                                                        else
                                                            resultado = resulta1;
                                                    }
                                                }

                                            }

                                        }
                                    }
                                    else
                                        resultado = resulta1;
                                }
                            }



                            if (rbaccion.SelectedIndex == 0)
                            {
                                sql = "INSERT INTO SCHE$SIPLAN20.SP20$PRODUCTO (SPPRO$DESCRIPCION, SPPRO$ID_MEDIDA, SPPRO$ID_RESULTADO, SPPRO$FECHA_INSERTA, SPPRO$PRESUPUESTO, SPPRO$POM, SPPRO$INSTO, SPPRO$PROPIETARIO, SPPRO$RESULTADO2) VALUES ('" + txtProductoInsto.Text + "', " + Convert.ToInt32(cbUnidadMedida.Value) + ", " + resultado + ", 'INSERT = '||TO_CHAR(SYSDATE,'DD/MM/YYYY HH24:MI')||' '||'" + Session["USUARIO"].ToString() + "', ";
                                if (cbSuprograma.SelectedIndex == 0)
                                {
                                    sql = sql + Convert.ToDouble(cbProgramaPresupuestario.Value) + ", ";
                                    sql = sql + Convert.ToInt32(Session["pom"]) + ", " + Convert.ToInt32(Session["insto"]) + ", '" + Session["USUARIO"].ToString() + "', " + resulta2 + ")";
                                }
                                else
                                {
                                    sql = sql + Convert.ToDouble(cbSuProgramaPresupuestario.Value) + ", ";
                                    sql = sql + Convert.ToInt32(Session["pom"]) + ", " + Convert.ToInt32(Session["insto"]) + ", '" + Session["USUARIO"].ToString() + "'," + resulta2 + ")";
                                }

                            }


                        }

                        else if (Convert.ToInt32(Session["operacion"]) == 1)
                        {

                            if (responsable == 1)
                            {
                                sql = "SELECT *  FROM SCHE$SIPLAN20.SPPSVST$PGG2024_2028 WHERE ID_ACCION = " + resulta1;
                                estado = dao.consulta(sql);
                                if (estado == 1)
                                {
                                    tabla = dao.tabla;
                                    if (tabla.Rows.Count > 0)
                                    {
                                        sql = "SELECT * FROM SCHE$SIPLAN20.SP20$RESULTADOS WHERE SPRES$COD_ESTRATEGICO = " + tabla.Rows[0]["ID_ACCION"] + " AND SPRES$TIPO = 0 AND SPRES$RESTRICTIVA = 'N' AND SPRES$POM = " + Convert.ToInt32(Session["pom"]) + " AND SPRES$INSTITUCION = " + Convert.ToInt32(Session["insto"]);
                                        estado = dao.consulta(sql);
                                        if (estado == 1)
                                        {
                                            tabla = dao.tabla;
                                            if (tabla.Rows.Count > 0)
                                                resultado = Convert.ToInt32(tabla.Rows[0]["SPRES$ID_RESULTADO"]);
                                            else
                                            {
                                                sql = "INSERT INTO SCHE$SIPLAN20.SP20$RESULTADOS(SPRES$TIPO, SPRES$COD_ESTRATEGICO, SPRES$POM, SPRES$INSTITUCION, SPRES$FECHA_INSERT, SPRES$PROPIETARIO) VALUES (0, " + resulta1 + ", " + Convert.ToInt32(Session["pom"]) + ", " + Convert.ToInt32(Session["insto"]) + ", 'INSERT = '||TO_CHAR(SYSDATE,'DD/MM/YYYY HH24:MI')||' '||'" + Session["USUARIO"].ToString() + "', '" + Session["USUARIO"].ToString() + "')";
                                                estado = dao.comando(sql);
                                                if (estado == 1)
                                                {
                                                    sql = "SELECT MAX(SPRES$ID_RESULTADO) ID FROM SCHE$SIPLAN20.SP20$RESULTADOS WHERE SPRES$COD_ESTRATEGICO = " + resulta1 + " AND SPRES$RESTRICTIVA = 'N' AND SPRES$POM = " + Convert.ToInt32(Session["pom"]) + " AND SPRES$INSTITUCION = " + Convert.ToInt32(Session["insto"]) + " AND SPRES$TIPO = 0";
                                                    estado = dao.consulta(sql);
                                                    if (estado == 1)
                                                    {
                                                        tabla = dao.tabla;
                                                        if (tabla.Rows.Count > 0)
                                                        {
                                                            resultado = Convert.ToInt32(tabla.Rows[0]["ID"]);
                                                        }
                                                        else
                                                            resultado = resulta1;
                                                    }
                                                }

                                            }

                                        }
                                    }


                                    else if (tabla.Rows.Count <= 0)
                                    {
                                        sql = @" SELECT
                                                    RG.SPRPG$ID_PGG ID_EJE
                                                    ,R.SPPRES$DESCRIPCION EJE                         
                                                    FROM SCHE$SIPLAN20.SP20$RESULTADOS_ESTRATEGICOS R 
                                                    INNER JOIN SCHE$SIPLAN20.SP20$RESPONSABLESPGG RG ON RG.SPRPG$ID_PGG = R.SPPRES$ID_RESULTADO AND RG.SPRPG$NIVEL  = 1 AND R.SPPRES$RESTRICTIVA = 'N' AND RG.SPRPG$RESTRICTIVA = 'N'
                                                    WHERE RG.SPRPG$RESPONSABLE = " + Convert.ToInt32(Session["insto"]) + @" AND RG.SPRPG$ID_PGG = " + resulta1;
                                        estado = dao.consulta(sql);
                                        if (estado == 1)
                                        {
                                            tabla = dao.tabla;
                                            if (tabla.Rows.Count > 0)
                                            {
                                                sql = "SELECT * FROM SCHE$SIPLAN20.SP20$RESULTADOS WHERE SPRES$COD_ESTRATEGICO = " + tabla.Rows[0]["ID_EJE"] + " AND SPRES$TIPO = 0 AND SPRES$RESTRICTIVA = 'N' AND SPRES$POM = " + Convert.ToInt32(Session["pom"]) + " AND SPRES$INSTITUCION = " + Convert.ToInt32(Session["insto"]);
                                                estado = dao.consulta(sql);

                                                if (estado == 1)
                                                {
                                                    tabla = dao.tabla;
                                                    if (tabla.Rows.Count > 0)
                                                        resultado = Convert.ToInt32(tabla.Rows[0]["SPRES$ID_RESULTADO"]);
                                                    else
                                                    {
                                                        sql = "INSERT INTO SCHE$SIPLAN20.SP20$RESULTADOS(SPRES$TIPO, SPRES$COD_ESTRATEGICO, SPRES$POM, SPRES$INSTITUCION, SPRES$FECHA_INSERT, SPRES$PROPIETARIO) VALUES (0, " + resulta1 + ", " + Convert.ToInt32(Session["pom"]) + ", " + Convert.ToInt32(Session["insto"]) + ", 'INSERT = '||TO_CHAR(SYSDATE,'DD/MM/YYYY HH24:MI')||' '||'" + Session["USUARIO"].ToString() + "', '" + Session["USUARIO"].ToString() + "')";
                                                        estado = dao.comando(sql);
                                                        if (estado == 1)
                                                        {
                                                            sql = "SELECT MAX(SPRES$ID_RESULTADO) ID FROM SCHE$SIPLAN20.SP20$RESULTADOS WHERE SPRES$COD_ESTRATEGICO = " + resulta1 + " AND SPRES$RESTRICTIVA = 'N' AND SPRES$POM = " + Convert.ToInt32(Session["pom"]) + " AND SPRES$INSTITUCION = " + Convert.ToInt32(Session["insto"]) + " AND SPRES$TIPO = 0";
                                                            estado = dao.consulta(sql);
                                                            if (estado == 1)
                                                            {
                                                                tabla = dao.tabla;
                                                                if (tabla.Rows.Count > 0)
                                                                {
                                                                    resultado = Convert.ToInt32(tabla.Rows[0]["ID"]);
                                                                }
                                                                else
                                                                    resultado = resulta1;
                                                            }
                                                        }

                                                    }

                                                }

                                            }
                                            else
                                                resultado = resulta1;
                                        }


                                    }

                                    else
                                        resultado = resulta1;
                                }



                            }

                            else if (responsable == 0)

                            {
                                //sql = "SELECT *  FROM SCHE$SIPLAN20.SPPSVST$PGG2024_2028 WHERE ID_EJE = " + resulta1;
                                sql = @"SELECT 
                          ID_EJE
                           ,EJE
                           FROM
                            (SELECT ID_EJE, EJE FROM SCHE$SIPLAN20.SPPSVST$PGG2024_2028
                            UNION
                            SELECT R.SPPRES$ID_RESULTADO ID_EJE, R.SPPRES$CODIGO || '-' || R.SPPRES$DESCRIPCION EJE FROM SCHE$SIPLAN20.SP20$RESULTADOS_ESTRATEGICOS R
                            INNER JOIN SCHE$SIPLAN20.S20$PERIODO_RESULTADO PR ON PR.SPR$ID_RESULTADO = R.SPPRES$ID_RESULTADO AND PR.SPR$RESTRICTIVA = 'N' AND R.SPPRES$RESTRICTIVA = 'N'
                            INNER JOIN SCHE$SIPLAN20.SP20$PERIODO_GOBIERNO PG ON PG.SPG$ID_PERIODO = PR.SPR$ID_PERIODO AND PG.SPG$RESTRICTIVA = 'N' AND PR.SPR$RESTRICTIVA = 'N' AND PG.SPG$VIGENTE = 1) WHERE ID_EJE = " + resulta1;
                                estado = dao.consulta(sql);
                                if (estado == 1)
                                {
                                    tabla = dao.tabla;
                                    if (tabla.Rows.Count > 0)
                                    {
                                        sql = "SELECT * FROM SCHE$SIPLAN20.SP20$RESULTADOS WHERE SPRES$COD_ESTRATEGICO = " + tabla.Rows[0]["ID_EJE"] + " AND SPRES$TIPO = 0 AND SPRES$RESTRICTIVA = 'N' AND SPRES$POM = " + Convert.ToInt32(Session["pom"]) + " AND SPRES$INSTITUCION = " + Convert.ToInt32(Session["insto"]);
                                        estado = dao.consulta(sql);
                                        if (estado == 1)
                                        {
                                            tabla = dao.tabla;
                                            if (tabla.Rows.Count > 0)
                                                resultado = Convert.ToInt32(tabla.Rows[0]["SPRES$ID_RESULTADO"]);
                                            else
                                            {
                                                sql = "INSERT INTO SCHE$SIPLAN20.SP20$RESULTADOS(SPRES$TIPO, SPRES$COD_ESTRATEGICO, SPRES$POM, SPRES$INSTITUCION, SPRES$FECHA_INSERT, SPRES$PROPIETARIO) VALUES (0, " + resulta1 + ", " + Convert.ToInt32(Session["pom"]) + ", " + Convert.ToInt32(Session["insto"]) + ", 'INSERT = '||TO_CHAR(SYSDATE,'DD/MM/YYYY HH24:MI')||' '||'" + Session["USUARIO"].ToString() + "', '" + Session["USUARIO"].ToString() + "')";
                                                estado = dao.comando(sql);
                                                if (estado == 1)
                                                {
                                                    sql = "SELECT MAX(SPRES$ID_RESULTADO) ID FROM SCHE$SIPLAN20.SP20$RESULTADOS WHERE SPRES$COD_ESTRATEGICO = " + resulta1 + " AND SPRES$RESTRICTIVA = 'N' AND SPRES$POM = " + Convert.ToInt32(Session["pom"]) + " AND SPRES$INSTITUCION = " + Convert.ToInt32(Session["insto"]) + " AND SPRES$TIPO = 0";
                                                    estado = dao.consulta(sql);
                                                    if (estado == 1)
                                                    {
                                                        tabla = dao.tabla;
                                                        if (tabla.Rows.Count > 0)
                                                        {
                                                            resultado = Convert.ToInt32(tabla.Rows[0]["ID"]);
                                                        }
                                                        else
                                                            resultado = resulta1;
                                                    }
                                                }

                                            }

                                        }
                                    }
                                    else
                                        resultado = resulta1;
                                }
                            }

                            //if ((gvProdInsto.GetRowValues(gvProdInsto.FocusedRowIndex, "SPPRO$PROPIETARIO").ToString() == Session["USUARIO"].ToString()) || Session["ROL"].ToString() == "ADMIN")
                            if (Session["ROL"].ToString() != "ENTIDAD")
                            {
                                codigo = Convert.ToInt32(gvProdInsto.GetRowValues(gvProdInsto.FocusedRowIndex, "SPPRO$ID_PRODUCTO").ToString());
                                sql = "UPDATE SCHE$SIPLAN20.SP20$PRODUCTO SET SPPRO$DESCRIPCION ='" + txtProductoInsto.Text + "', SPPRO$ID_MEDIDA = " + Convert.ToInt32(cbUnidadMedida.Value) + ", SPPRO$ID_RESULTADO =" + resultado + ", SPPRO$RESULTADO2 = " + resulta2 + "  , SPPRO$FECHA_UPDATE = 'UPDATE = '||TO_CHAR(SYSDATE,'DD/MM/YYYY HH24:MI')||' '||'" + Session["USUARIO"].ToString() + "', ";
                                if (rbaccion.SelectedIndex == 0)
                                {

                                    if (cbSuprograma.SelectedIndex == 0)
                                    {
                                        sql = sql + "SPPRO$PRESUPUESTO = " + Convert.ToDouble(cbProgramaPresupuestario.Value) + ", SPPRO$OBJETIVO_CENTRAL = NULL, SPPRO$ACCION_ESTRATEGICA = NULL  ";

                                    }
                                    else
                                    {
                                        sql = sql + "SPPRO$PRESUPUESTO = " + Convert.ToDouble(cbSuProgramaPresupuestario.Value) + ", SPPRO$OBJETIVO_CENTRAL = NULL, SPPRO$ACCION_ESTRATEGICA = NULL ";
                                    }

                                }

                                else if (rbaccion.SelectedIndex == 1)
                                {
                                    if (cbSuprograma.SelectedIndex == 0)
                                    {
                                        sql = sql + " SPPRO$PRESUPUESTO = " + Convert.ToDouble(cbProgramaPresupuestario.Value) + ", SPPRO$OBJETIVO_CENTRAL = " + Convert.ToInt32(cbObjSectorial.Value) + ", SPPRO$ACCION_ESTRATEGICA = " + Convert.ToInt32(CbAccionEstrategica.Value);
                                    }
                                    else
                                    {
                                        sql = sql + " SPPRO$PRESUPUESTO = " + Convert.ToDouble(cbSuProgramaPresupuestario.Value) + ", SPPRO$OBJETIVO_CENTRAL = " + Convert.ToInt32(cbObjSectorial.Value) + ", SPPRO$ACCION_ESTRATEGICA = " + Convert.ToInt32(CbAccionEstrategica.Value);
                                    }


                                }
                                sql = sql + " WHERE SPPRO$POM = " + Convert.ToInt32(Session["pom"]) + " AND SPPRO$INSTO = " + Convert.ToInt32(Session["insto"]) + " AND SPPRO$ID_PRODUCTO = " + codigo + " AND SPPRO$RESTRICTIVA = 'N'";
                            }
                            else
                            {
                                mensaje = "Su perfil no usuario no esta autorizado para modificar este registro";
                                Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
                            }




                        }

                        estado = dao.comando(sql);

                        if (estado == 0)
                        {
                            mensaje = dao.mensaje;
                            Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
                            //cbResultadoInsitucional.SelectedIndex = -1;
                            cbProgramaPresupuestario.SelectedIndex = -1;
                            cbSuprograma.SelectedIndex = -1;
                            subProgramaPresupuestario.Style.Add("display", "none");
                            cbSuProgramaPresupuestario.SelectedIndex = -1;
                            txtProductoInsto.Text = "";
                            cbUnidadMedida.SelectedIndex = -1;
                            rbaccion.SelectedIndex = -1;
                            AccionEstrategica.Style.Add("display", "none");
                            cpPilarPGG.SelectedIndex = -1;
                            cbObjSectorial.SelectedIndex = -1;
                            CbAccionEstrategica.SelectedIndex = -1;
                            cargaProductosInsto(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]));
                            //cargaResultadoInsto(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]));
                            cargaProgramaPresu(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]));
                            cargaUnidades();
                            cargaPilares();
                            rbProductos.SelectedIndex = 0;
                            ProdInsitucionales.Style.Add("display", "block");
                            MultiView1.ActiveViewIndex = 4;
                        }
                        else
                        {


                            Session["CODIGO"] = null;
                            mensaje = "Producto registrado correctamente";
                            Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',1);", true);
                            //cbResultadoInsitucional.SelectedIndex = -1;
                            cbProgramaPresupuestario.SelectedIndex = -1;
                            cbSuprograma.SelectedIndex = -1;
                            subProgramaPresupuestario.Style.Add("display", "none");
                            cbSuProgramaPresupuestario.SelectedIndex = -1;
                            txtProductoInsto.Text = "";
                            cbUnidadMedida.SelectedIndex = -1;
                            rbaccion.SelectedIndex = -1;
                            AccionEstrategica.Style.Add("display", "none");
                            cpPilarPGG.SelectedIndex = -1;
                            cbAcciones.SelectedIndex = -1;
                            cbResultados.SelectedIndex = -1;
                            cargaProductosInsto(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]));
                            cargaResultadoInsto(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), -1);
                            cargaProgramaPresu(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]));
                            cargaUnidades();
                            cargaPilares();
                            rbProductos.SelectedIndex = 0;
                            ProdInsitucionales.Style.Add("display", "block");
                            MultiView1.ActiveViewIndex = 4;
                        }

                    }
                }
            }
            //Operaciones para el producto estrategico
            //*----------------------------------------------------------------------*
            else if (Session["tipo"].ToString() == "estrategico")
            {

                if (Convert.ToInt32(Session["operacion"]) == 12)
                {
                    //if ((gvProdEstrategicos.GetRowValues(gvProdEstrategicos.FocusedRowIndex, "SPPRO$PROPIETARIO").ToString() == Session["USUARIO"].ToString()) || Session["ROL"].ToString() == "ADMIN")
                    if (Session["ROL"].ToString() != "ENTIDAD")
                    {
                        codigo = Convert.ToInt32(gvProdEstrategicos.GetRowValues(gvProdEstrategicos.FocusedRowIndex, "SPPRO$ID_PRODUCTO").ToString());
                        sql = "SELECT * FROM SCHE$SIPLAN20.SP20$SUB_PRODUCTO S WHERE S.SPPSUB$ID_PRODUCTO = " + codigo + " AND S.SPPSUB$RESTRICTIVA = 'N'";

                        estado = dao.consulta(sql);
                        if (estado == 0)
                        {
                            mensaje = dao.mensaje;
                            Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
                            //cbResultadoInsitucional.SelectedIndex = -1;
                            cbProgramaPresupuestario.SelectedIndex = -1;
                            cbSuprograma.SelectedIndex = -1;
                            subProgramaPresupuestario.Style.Add("display", "none");
                            cbSuProgramaPresupuestario.SelectedIndex = -1;
                            txtProductoInsto.Text = "";
                            cbUnidadMedida.SelectedIndex = -1;
                            rbaccion.SelectedIndex = -1;
                            AccionEstrategica.Style.Add("display", "none");
                            cpPilarPGG.SelectedIndex = -1;
                            cbObjSectorial.SelectedIndex = -1;
                            CbAccionEstrategica.SelectedIndex = -1;
                            cargaProductosInsto(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]));

                            cargaResultadoInsto(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), -1);
                            cargaProgramaPresu(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]));
                            cargaUnidades();
                            cargaPilares();
                            rbProductos.SelectedIndex = 1;
                            ProdEstrategicos.Style.Add("display", "block");
                            ProdInsitucionales.Style.Add("display", "none");
                            MultiView1.ActiveViewIndex = 4;
                        }
                        else
                        {
                            tabla = dao.tabla;

                            sql = "UPDATE SCHE$SIPLAN20.SP20$PRODUCTO SET SPPRO$RESTRICTIVA = 'S', SPPRO$FECHA_DELETE = 'DELETE = '||TO_CHAR(SYSDATE,'DD/MM/YYYY HH24:MI')||' '||'" + Session["USUARIO"].ToString() + "' WHERE SPPRO$ID_PRODUCTO = " + codigo + " AND SPPRO$POM = " + Convert.ToInt32(Session["pom"]) + " AND SPPRO$INSTO = " + Convert.ToInt32(Session["insto"]);
                            estado = dao.comando(sql);
                            if (estado == 0)
                            {
                                mensaje = dao.mensaje;
                                Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
                                //cbResultadoInsitucional.SelectedIndex = -1;
                                cbProgramaPresupuestario.SelectedIndex = -1;
                                cbSuprograma.SelectedIndex = -1;
                                subProgramaPresupuestario.Style.Add("display", "none");
                                cbSuProgramaPresupuestario.SelectedIndex = -1;
                                txtProductoInsto.Text = "";
                                cbUnidadMedida.SelectedIndex = -1;
                                rbaccion.SelectedIndex = -1;
                                AccionEstrategica.Style.Add("display", "none");
                                cpPilarPGG.SelectedIndex = -1;
                                cbObjSectorial.SelectedIndex = -1;
                                CbAccionEstrategica.SelectedIndex = -1;
                                cargaProductosInsto(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]));

                                cargaResultadoInsto(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), -1);
                                cargaProgramaPresu(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]));
                                cargaUnidades();
                                cargaPilares();
                                rbProductos.SelectedIndex = 1;
                                ProdEstrategicos.Style.Add("display", "block");
                                ProdInsitucionales.Style.Add("display", "none");
                                MultiView1.ActiveViewIndex = 4;
                            }
                            else
                            {
                                if (tabla.Rows.Count > 0)
                                {
                                    for (int i = 0; i < tabla.Rows.Count; i++)
                                    {
                                        sql = "UPDATE SCHE$SIPLAN20.SP20$SUB_PRODUCTO SET SPPSUB$RESTRICTIVA = 'S', SPPSUB$FECHA_DELETE = 'DELETE = '||TO_CHAR(SYSDATE,'DD/MM/YYYY HH24:MI')||' '||'" + Session["USUARIO"].ToString() + "' WHERE  SPPSUB$ID_PRODUCTO =  " + tabla.Rows[i]["SPPSUB$ID_PRODUCTO"];
                                        estado = dao.comando(sql);
                                        if (estado == 0)
                                            break;
                                        sql = "UPATE SCHE$SIPLAN20.SP20$PROGRAMETA_FI_FIN_SUB SET SPPMFS$RESTRICTIVA = 'N', SPPMFS$FECHA_DELETE = 'DELETE = '||TO_CHAR(SYSDATE,'DD/MM/YYYY HH24:MI')||' '||'" + Session["USUARIO"].ToString() + "' WHERE SPPMFS$ID_SUBPRODUCTO = " + tabla.Rows[i]["SPPSUB$ID_SUBPRODUCTO"];
                                        estado = dao.comando(sql);
                                        if (estado == 0)
                                            break;

                                        sql = "UPDATE SCHE$SIPLAN20.SP20$POASUBPRODUCTOS SET SPOAS$RESTRICTIVA = 'S', SPOAS$FECHA_DELETE = 'DELETE = '||TO_CHAR(SYSDATE,'DD/MM/YYYY HH24:MI')||' '||'" + Session["USUARIO"].ToString() + "' WHERE SPOAS$SUBPRODUCTO = " + tabla.Rows[i]["SPPSUB$ID_SUBPRODUCTO"]; ;
                                        estado = dao.comando(sql);
                                        if (estado == 0)
                                            break;
                                    }

                                    sql = "UPDATE SCHE$SIPLAN20.SP20$PROGRAMACION_MFPRODUCTO SET SPPMFS$RESTRICTIVA = 'S', SPPMFS$FECHA_DELETE = 'DELETE = '||TO_CHAR(SYSDATE,'DD/MM/YYYY HH24:MI')||' '||'" + Session["USUARIO"].ToString() + "' WHERE SPPMFS$ID_PRODUCTO = " + codigo;
                                    estado = dao.comando(sql);

                                    sql = "UPDATE SCHE$SIPLAN20,SP20$POAPRODUCTOS SET SPPR$RESTRICTIVA = 'S', SPPR$FECHA_DELETE = 'DELETE = '||TO_CHAR(SYSDATE,'DD/MM/YYYY HH24:MI')||' '||'" + Session["USUARIO"].ToString() + "' WHERE SPPR$PRODUCTO = " + codigo;
                                    estado = dao.comando(sql);

                                    sql = "UPDATE SCHE$SIPLAN20.SP20$MUNOSPRIORIZADOS SET RESTRICTIVA = 'S', FECHA_BORRADO = 'BORRADO = '||TO_CHAR(SYSDATE,'DD/MM/YYYY HH24:MI')||' '||'" + Session["USUARIO"].ToString() + "' WHERE SPPRO$ID_PRODUCTO = " + codigo;
                                    estado = dao.comando(sql);

                                    mensaje = "Producto eliminado correctamente";
                                    Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',1);", true);
                                    //cbResultadoInsitucional.SelectedIndex = -1;
                                    cbProgramaPresupuestario.SelectedIndex = -1;
                                    cbSuprograma.SelectedIndex = -1;
                                    subProgramaPresupuestario.Style.Add("display", "none");
                                    cbSuProgramaPresupuestario.SelectedIndex = -1;
                                    txtProductoInsto.Text = "";
                                    cbUnidadMedida.SelectedIndex = -1;
                                    rbaccion.SelectedIndex = -1;
                                    AccionEstrategica.Style.Add("display", "none");
                                    cpPilarPGG.SelectedIndex = -1;
                                    cbObjSectorial.SelectedIndex = -1;
                                    CbAccionEstrategica.SelectedIndex = -1;
                                    cargaProductosInsto(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]));
                                    cargaProductoRed(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]));
                                    cargaResultadoInsto(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), -1);
                                    cargaProgramaPresu(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]));
                                    cargaUnidades();
                                    cargaPilares();
                                    rbProductos.SelectedIndex = 1;
                                    ProdEstrategicos.Style.Add("display", "block");
                                    ProdInsitucionales.Style.Add("display", "none");
                                    MultiView1.ActiveViewIndex = 4;
                                    Session["CODIGO"] = null;

                                }

                                else
                                {
                                    sql = "UPDATE SCHE$SIPLAN20.SP20$PROGRAMACION_MFPRODUCTO SET SPPMFS$RESTRICTIVA = 'S', SPPMFS$FECHA_DELETE = 'DELETE = '||TO_CHAR(SYSDATE,'DD/MM/YYYY HH24:MI')||' '||'" + Session["USUARIO"].ToString() + "' WHERE SPPMFS$ID_PRODUCTO = " + codigo;
                                    estado = dao.comando(sql);

                                    sql = "UPDATE SCHE$SIPLAN20,SP20$POAPRODUCTOS SET SPPR$RESTRICTIVA = 'S', SPPR$FECHA_DELETE = 'DELETE = '||TO_CHAR(SYSDATE,'DD/MM/YYYY HH24:MI')||' '||'" + Session["USUARIO"].ToString() + "' WHERE SPPR$PRODUCTO = " + codigo;
                                    estado = dao.comando(sql);

                                    sql = "UPDATE SCHE$SIPLAN20.SP20$MUNOSPRIORIZADOS SET RESTRICTIVA = 'S', FECHA_BORRADO = 'BORRADO = '||TO_CHAR(SYSDATE,'DD/MM/YYYY HH24:MI')||' '||'" + Session["USUARIO"].ToString() + "' WHERE SPPRO$ID_PRODUCTO = " + codigo;
                                    estado = dao.comando(sql);

                                    mensaje = "Producto eliminado correctamente";
                                    Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',1);", true);
                                    //cbResultadoInsitucional.SelectedIndex = -1;
                                    cbProgramaPresupuestario.SelectedIndex = -1;
                                    cbSuprograma.SelectedIndex = -1;
                                    subProgramaPresupuestario.Style.Add("display", "none");
                                    cbSuProgramaPresupuestario.SelectedIndex = -1;
                                    txtProductoInsto.Text = "";
                                    cbUnidadMedida.SelectedIndex = -1;
                                    rbaccion.SelectedIndex = -1;
                                    AccionEstrategica.Style.Add("display", "none");
                                    cpPilarPGG.SelectedIndex = -1;
                                    cbObjSectorial.SelectedIndex = -1;
                                    CbAccionEstrategica.SelectedIndex = -1;
                                    cargaProductosInsto(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]));
                                    cargaProductoRed(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]));
                                    cargaResultadoInsto(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), -1);
                                    cargaProgramaPresu(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]));
                                    cargaUnidades();
                                    cargaPilares();
                                    rbProductos.SelectedIndex = 1;
                                    ProdEstrategicos.Style.Add("display", "block");
                                    ProdInsitucionales.Style.Add("display", "none");
                                    MultiView1.ActiveViewIndex = 4;
                                    Session["CODIGO"] = null;

                                }




                            }

                            //}
                        }


                    }

                    else
                    {
                        mensaje = "Su perfil de usuario no le permite eliminar este producto";
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
                    }
                }








                else
                {

                    if (cbRed.SelectedIndex == -1)
                    {
                        mensaje = "Seleccione el Resultado Estratégico RE";
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
                        cbRed.Focus();
                    }
                    else if (cbPGG.SelectedIndex == 1 && cbPilar.SelectedIndex == -1 && cbAcciones.SelectedIndex == -1)
                    {
                        mensaje = "Seleccione el eje estratégico PGG/meta presidencial PGG a vincular";
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
                    }
                    /*
            else if (cbResultadoInsitucional.SelectedIndex == -1)
            {
                mensaje = "Seleccione la Meta PGG";
                Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
                cbResultadoInsitucional.Focus();
            }
            */
                    else if (cbProgramaPresupuestario.SelectedIndex == -1)
                    {
                        mensaje = "Seleccione el programa presupuestario";
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
                        cbProgramaPresupuestario.Focus();
                    }

                    else if (cbSuprograma.SelectedIndex == -1)
                    {

                        mensaje = "Conteste la pregunta ¿Necesita vincular el producto a un subprograma presupuestario:?";
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
                        cbSuprograma.Focus();
                    }


                    else if (cbSuprograma.SelectedIndex == 1 && cbSuProgramaPresupuestario.SelectedIndex == -1)
                    {
                        mensaje = "Esta vinculando a un suprograma presupuestario, la descripción de subprograma es necesario";
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
                        cbSuProgramaPresupuestario.Focus();
                    }

                    else if (txtProductoInsto.Text == "")
                    {
                        mensaje = "La descripción del producto es necesaria";
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
                        txtProductoInsto.Focus();
                    }

                    else if (cbUnidadMedida.SelectedIndex == -1)
                    {
                        mensaje = "La unidad de medida del producto es necesaria";
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
                        cbUnidadMedida.Focus();
                    }

                    else if (rbaccion.SelectedIndex == -1 && Session["tipo"].ToString() == "institucional")
                    {
                        mensaje = "Conteste la pregunta ¿Su producto esta vinculado a alguna acción estratégica?";
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
                        cbUnidadMedida.Focus();
                    }



                    else if (rbaccion.SelectedIndex == 1)
                    {
                        if (cbRed.SelectedIndex == -1)
                        {
                            mensaje = "Seleccione el Resultado Estratégico RE";
                            Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
                            cbRed.Focus();
                        }
                        /*else if (cbObjSectorial.SelectedIndex == -1)
                        {
                            mensaje = "Seleccione el objetivo sectorial";
                            Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
                            cbObjSectorial.Focus();
                        }
                        */

                        /*else if (CbAccionEstrategica.SelectedIndex == -1)
                        {
                            mensaje = "Seleccione la acción estrategica";
                            Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
                            CbAccionEstrategica.Focus();
                        }
                        */
                        else
                        {

                            if (Convert.ToInt32(Session["operacion"]) == 10)//INICIO DE GRABAR PRODUCTO ESTRATEGICO
                            {
                                //if (rbaccion.SelectedIndex == 0)
                                //{
                                sql = "INSERT INTO SCHE$SIPLAN20.SP20$PRODUCTO (SPPRO$DESCRIPCION, SPPRO$ID_MEDIDA, SPPRO$ID_RESULTADO, SPPRO$FECHA_INSERTA, SPPRO$PRESUPUESTO, SPPRO$POM, SPPRO$INSTO, SPPRO$PROPIETARIO) VALUES ('" + txtProductoInsto.Text + "', " + Convert.ToInt32(cbUnidadMedida.Value) + ", " + Convert.ToInt32(cbAcciones.Value) + ", 'INSERT = '||TO_CHAR(SYSDATE,'DD/MM/YYYY HH24:MI')||' '||'" + Session["USUARIO"].ToString() + "', ";
                                if (cbSuprograma.SelectedIndex == 0)
                                {
                                    sql = sql + Convert.ToDouble(cbProgramaPresupuestario.Value) + ", ";
                                    sql = sql + Convert.ToInt32(Session["pom"]) + ", " + Convert.ToInt32(Session["insto"]) + ", '" + Session["USUARIO"].ToString() + "')";
                                }
                                else
                                {
                                    sql = sql + Convert.ToDouble(cbSuProgramaPresupuestario.Value) + ", ";
                                    sql = sql + Convert.ToInt32(Session["pom"]) + ", " + Convert.ToInt32(Session["insto"]) + ", '" + Session["USUARIO"].ToString() + "')";
                                }

                                //}

                                //else if (rbaccion.SelectedIndex == 1)
                                //{
                                //    sql = "INSERT INTO SCHE$SIPLAN20.SP20$PRODUCTO (SPPRO$DESCRIPCION, SPPRO$ID_MEDIDA, SPPRO$OBJETIVO_CENTRAL, SPPRO$ACCION_ESTRATEGICA, SPPRO$ID_RESULTADO, SPPRO$FECHA_INSERTA, SPPRO$PRESUPUESTO, SPPRO$POM, SPPRO$INSTO, SPPRO$PROPIETARIO) VALUES ('" + txtProductoInsto.Text + "', " + Convert.ToInt32(cbUnidadMedida.Value) + ", " + Convert.ToInt32(cbObjSectorial.Value) + ", " + Convert.ToInt32(CbAccionEstrategica.Value) + ", " + Convert.ToInt32(cbResultadoInsitucional.Value) + ", 'INSERT = '||TO_CHAR(SYSDATE,'DD/MM/YYYY HH24:MI')||' '||'" + Session["USUARIO"].ToString() + "', ";
                                //    if (cbSuprograma.SelectedIndex == 0)
                                //    {
                                //        sql = sql + Convert.ToInt32(cbProgramaPresupuestario.Value) + ", ";
                                //        sql = sql + Convert.ToInt32(Session["pom"]) + ", " + Convert.ToInt32(Session["insto"]) + ", '" + Session["USUARIO"].ToString() + "')";
                                //    }

                                //    else
                                //    {
                                //        sql = sql + Convert.ToInt32(cbSuProgramaPresupuestario.Value) + ", ";
                                //        sql = sql + Convert.ToInt32(Session["pom"]) + ", " + Convert.ToInt32(Session["insto"]) + ", '" + Session["USUARIO"].ToString() + "')";
                                //    }
                                //}


                            }//FIN DE GRABADO ESTRATEGICO

                            else if (Convert.ToInt32(Session["operacion"]) == 11)//INICIO EDICION ESTRATEGICO
                            {
                                //if ((gvProdEstrategicos.GetRowValues(gvProdEstrategicos.FocusedRowIndex, "SPPRO$PROPIETARIO").ToString() == Session["USUARIO"].ToString()) || Session["ROL"].ToString() == "ADMIN")
                                if (Session["ROL"].ToString() != "ENTIDAD")
                                {

                                    resultado = -1;
                                    responsable = busca_responsable(Convert.ToInt32(Session["insto"]));

                                    if (responsable == 1)
                                    {
                                        if (cbAcciones.SelectedIndex != -1)
                                            resulta2 = Convert.ToInt32(cbAcciones.Value);
                                    }
                                    else
                                    {
                                        if (cbPilar.SelectedIndex != -1)
                                            resulta2 = Convert.ToInt32(cbPilar.Value);
                                    }




                                    if (resulta2 != -1)
                                    {
                                        if (responsable == 1)
                                        {
                                            sql = "SELECT *  FROM SCHE$SIPLAN20.SPPSVST$PGG2024_2028 WHERE ID_ACCION = " + resulta2;
                                            estado = dao.consulta(sql);
                                            if (estado == 1)
                                            {
                                                tabla = dao.tabla;
                                                if (tabla.Rows.Count > 0)
                                                {
                                                    sql = "SELECT * FROM SCHE$SIPLAN20.SP20$RESULTADOS WHERE SPRES$COD_ESTRATEGICO = " + tabla.Rows[0]["ID_ACCION"] + " AND SPRES$TIPO = 0 AND SPRES$RESTRICTIVA = 'N' AND SPRES$POM = " + Convert.ToInt32(Session["pom"]) + " AND SPRES$INSTITUCION = " + Convert.ToInt32(Session["insto"]);
                                                    estado = dao.consulta(sql);
                                                    if (estado == 1)
                                                    {
                                                        tabla = dao.tabla;
                                                        if (tabla.Rows.Count > 0)
                                                            resultado = Convert.ToInt32(tabla.Rows[0]["SPRES$ID_RESULTADO"]);
                                                        else
                                                        {
                                                            sql = "INSERT INTO SCHE$SIPLAN20.SP20$RESULTADOS(SPRES$TIPO, SPRES$COD_ESTRATEGICO, SPRES$POM, SPRES$INSTITUCION, SPRES$FECHA_INSERT, SPRES$PROPIETARIO) VALUES (0, " + resulta1 + ", " + Convert.ToInt32(Session["pom"]) + ", " + Convert.ToInt32(Session["insto"]) + ", 'INSERT = '||TO_CHAR(SYSDATE,'DD/MM/YYYY HH24:MI')||' '||'" + Session["USUARIO"].ToString() + "', '" + Session["USUARIO"].ToString() + "')";
                                                            estado = dao.comando(sql);
                                                            if (estado == 1)
                                                            {
                                                                sql = "SELECT MAX(SPRES$ID_RESULTADO) ID FROM SCHE$SIPLAN20.SP20$RESULTADOS WHERE SPRES$COD_ESTRATEGICO = " + resulta1 + " AND SPRES$RESTRICTIVA = 'N' AND SPRES$POM = " + Convert.ToInt32(Session["pom"]) + " AND SPRES$INSTITUCION = " + Convert.ToInt32(Session["insto"]) + " AND SPRES$TIPO = 0";
                                                                estado = dao.consulta(sql);
                                                                if (estado == 1)
                                                                {
                                                                    tabla = dao.tabla;
                                                                    if (tabla.Rows.Count > 0)
                                                                    {
                                                                        resultado = Convert.ToInt32(tabla.Rows[0]["ID"]);
                                                                    }
                                                                    else
                                                                        resultado = -1;
                                                                }
                                                            }

                                                        }

                                                    }
                                                }
                                                else
                                                    resultado = -1;
                                            }



                                        }

                                        else if (responsable == 0)

                                        {
                                            //sql = "SELECT *  FROM SCHE$SIPLAN20.SPPSVST$PGG2024_2028 WHERE ID_EJE = " + resulta2;
                                            sql = @"SELECT 
                                                    ID_EJE
                                                    ,EJE
                                                    FROM
                                                    (SELECT ID_EJE, EJE FROM SCHE$SIPLAN20.SPPSVST$PGG2024_2028
                                                    UNION
                                                    SELECT R.SPPRES$ID_RESULTADO ID_EJE, R.SPPRES$CODIGO || '-' || R.SPPRES$DESCRIPCION EJE FROM SCHE$SIPLAN20.SP20$RESULTADOS_ESTRATEGICOS R
                                                    INNER JOIN SCHE$SIPLAN20.S20$PERIODO_RESULTADO PR ON PR.SPR$ID_RESULTADO = R.SPPRES$ID_RESULTADO AND PR.SPR$RESTRICTIVA = 'N' AND R.SPPRES$RESTRICTIVA = 'N'
                                                    INNER JOIN SCHE$SIPLAN20.SP20$PERIODO_GOBIERNO PG ON PG.SPG$ID_PERIODO = PR.SPR$ID_PERIODO AND PG.SPG$RESTRICTIVA = 'N' AND PR.SPR$RESTRICTIVA = 'N' AND PG.SPG$VIGENTE = 1) WHERE ID_EJE = " + resulta2;

                                            estado = dao.consulta(sql);
                                            if (estado == 1)
                                            {
                                                tabla = dao.tabla;
                                                if (tabla.Rows.Count > 0)
                                                {
                                                    sql = "SELECT * FROM SCHE$SIPLAN20.SP20$RESULTADOS WHERE SPRES$COD_ESTRATEGICO = " + tabla.Rows[0]["ID_EJE"] + " AND SPRES$TIPO = 0 AND SPRES$RESTRICTIVA = 'N' AND SPRES$POM = " + Convert.ToInt32(Session["pom"]) + " AND SPRES$INSTITUCION = " + Convert.ToInt32(Session["insto"]);
                                                    estado = dao.consulta(sql);
                                                    if (estado == 1)
                                                    {
                                                        tabla = dao.tabla;
                                                        if (tabla.Rows.Count > 0)
                                                            resultado = Convert.ToInt32(tabla.Rows[0]["SPRES$ID_RESULTADO"]);
                                                        else
                                                        {
                                                            sql = "INSERT INTO SCHE$SIPLAN20.SP20$RESULTADOS(SPRES$TIPO, SPRES$COD_ESTRATEGICO, SPRES$POM, SPRES$INSTITUCION, SPRES$FECHA_INSERT, SPRES$PROPIETARIO) VALUES (0, " + resulta1 + ", " + Convert.ToInt32(Session["pom"]) + ", " + Convert.ToInt32(Session["insto"]) + ", 'INSERT = '||TO_CHAR(SYSDATE,'DD/MM/YYYY HH24:MI')||' '||'" + Session["USUARIO"].ToString() + "', '" + Session["USUARIO"].ToString() + "')";
                                                            estado = dao.comando(sql);
                                                            if (estado == 1)
                                                            {
                                                                sql = "SELECT MAX(SPRES$ID_RESULTADO) ID FROM SCHE$SIPLAN20.SP20$RESULTADOS WHERE SPRES$COD_ESTRATEGICO = " + resulta1 + " AND SPRES$RESTRICTIVA = 'N' AND SPRES$POM = " + Convert.ToInt32(Session["pom"]) + " AND SPRES$INSTITUCION = " + Convert.ToInt32(Session["insto"]) + " AND SPRES$TIPO = 0";
                                                                estado = dao.consulta(sql);
                                                                if (estado == 1)
                                                                {
                                                                    tabla = dao.tabla;
                                                                    if (tabla.Rows.Count > 0)
                                                                    {
                                                                        resultado = Convert.ToInt32(tabla.Rows[0]["ID"]);
                                                                    }
                                                                    else
                                                                        resultado = -1;
                                                                }
                                                            }

                                                        }

                                                    }
                                                }
                                                else
                                                    resultado = -1;
                                            }
                                        }


                                    }



                                    sql = "SELECT * FROM SCHE$SIPLAN20.SP20$RESULTADOS R WHERE R.SPRES$POM = " + Convert.ToInt32(Session["pom"]) + " AND R.SPRES$INSTITUCION = " + Convert.ToInt32(Session["insto"]) + " AND R.SPRES$TIPO = 2 AND R.SPRES$COD_ESTRATEGICO = " + cbRed.Value + " AND SPRES$RESTRICTIVA = 'N'";
                                    estado = dao.consulta(sql);
                                    if (estado == 1)
                                    {
                                        red_resultado = dao.tabla;
                                        if (red_resultado.Rows.Count > 0)
                                            red_producto = Convert.ToInt32(red_resultado.Rows[0]["SPRES$ID_RESULTADO"]);
                                        else
                                        {
                                            sql = "INSERT  INTO SCHE$SIPLAN20.SP20$RESULTADOS (SPRES$TIPO, SPRES$COD_ESTRATEGICO, SPRES$FECHA_INSERT, SPRES$POM, SPRES$INSTITUCION, SPRES$PROPIETARIO) VALUES(2, " + cbRed.Value + ", 'INSERT = '||TO_CHAR(SYSDATE,'DD/MM/YYYY HH24:MI')||' '||'" + Session["USUARIO"].ToString() + "', " + Convert.ToInt32(Session["pom"]) + ", " + Convert.ToInt32(Session["insto"]) + ", '" + Session["USUARIO"].ToString() + "')";
                                            estado = dao.comando(sql);
                                            if (estado == 1)
                                            {
                                                sql = "SELECT MAX(SPRES$ID_RESULTADO) ID FROM SCHE$SIPLAN20.SP20$RESULTADOS R WHERE R.SPRES$POM = " + Convert.ToInt32(Session["pom"]) + " AND R.SPRES$INSTITUCION = " + Convert.ToInt32(Session["insto"]) + " AND R.SPRES$TIPO = 2 AND R.SPRES$COD_ESTRATEGICO = " + cbRed.Value + " AND SPRES$RESTRICTIVA = 'N'";
                                                estado = dao.consulta(sql);
                                                if (estado == 1)
                                                {
                                                    red_resultado = dao.tabla;
                                                    if (red_resultado.Rows.Count > 0)
                                                        red_producto = Convert.ToInt32(red_resultado.Rows[0]["ID"]);
                                                }
                                            }
                                        }



                                    }


                                    codigo = Convert.ToInt32(gvProdEstrategicos.GetRowValues(gvProdEstrategicos.FocusedRowIndex, "SPPRO$ID_PRODUCTO").ToString());
                                    sql = "UPDATE SCHE$SIPLAN20.SP20$PRODUCTO SET SPPRO$DESCRIPCION ='" + txtProductoInsto.Text + "', SPPRO$ID_MEDIDA = " + Convert.ToInt32(cbUnidadMedida.Value) + ", SPPRO$ID_RESULTADO =" + red_producto + ", SPPRO$RESULTADO2 = " + resultado + ", SPPRO$FECHA_UPDATE = 'UPDATE = '||TO_CHAR(SYSDATE,'DD/MM/YYYY HH24:MI')||' '||'" + Session["USUARIO"].ToString() + "', ";
                                    //if (rbaccion.SelectedIndex == 0)
                                    //{

                                    if (cbSuprograma.SelectedIndex == 0)
                                    {
                                        sql = sql + "SPPRO$PRESUPUESTO = " + Convert.ToDouble(cbProgramaPresupuestario.Value) + ", SPPRO$OBJETIVO_CENTRAL = NULL, SPPRO$ACCION_ESTRATEGICA = NULL  ";

                                    }
                                    else
                                    {
                                        sql = sql + "SPPRO$PRESUPUESTO = " + Convert.ToDouble(cbSuProgramaPresupuestario.Value) + ", SPPRO$OBJETIVO_CENTRAL = NULL, SPPRO$ACCION_ESTRATEGICA = NULL ";
                                    }

                                    //}

                                    //else if (rbaccion.SelectedIndex == 1)
                                    //{
                                    //    if (cbSuprograma.SelectedIndex == 0)
                                    //    {
                                    //        sql = sql + " SPPRO$PRESUPUESTO = " + Convert.ToInt32(cbProgramaPresupuestario.Value) + ", SPPRO$OBJETIVO_CENTRAL = " + Convert.ToInt32(cbObjSectorial.Value) + ", SPPRO$ACCION_ESTRATEGICA = " + Convert.ToInt32(CbAccionEstrategica.Value);
                                    //    }
                                    //    else
                                    //    {
                                    //        sql = sql + " SPPRO$PRESUPUESTO = " + Convert.ToInt32(cbSuProgramaPresupuestario.Value) + ", SPPRO$OBJETIVO_CENTRAL = " + Convert.ToInt32(cbObjSectorial.Value) + ", SPPRO$ACCION_ESTRATEGICA = " + Convert.ToInt32(CbAccionEstrategica.Value);
                                    //    }


                                    //}
                                    sql = sql + " WHERE SPPRO$POM = " + Convert.ToInt32(Session["pom"]) + " AND SPPRO$INSTO = " + Convert.ToInt32(Session["insto"]) + " AND SPPRO$ID_PRODUCTO = " + codigo + " AND SPPRO$RESTRICTIVA = 'N'";


                                }

                                else
                                {
                                    mensaje = "Su perfil no usuario no esta autorizado para modificar este registro";
                                    Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
                                }

                            }//FIN EDICION ESTRATEGICO

                            estado = dao.comando(sql);

                            if (estado == 0)
                            {
                                mensaje = dao.mensaje;
                                Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
                                //cbResultadoInsitucional.SelectedIndex = -1;
                                cbProgramaPresupuestario.SelectedIndex = -1;
                                cbSuprograma.SelectedIndex = -1;
                                subProgramaPresupuestario.Style.Add("display", "none");
                                cbSuProgramaPresupuestario.SelectedIndex = -1;
                                txtProductoInsto.Text = "";
                                cbUnidadMedida.SelectedIndex = -1;
                                rbaccion.SelectedIndex = -1;
                                AccionEstrategica.Style.Add("display", "none");
                                cpPilarPGG.SelectedIndex = -1;
                                cbObjSectorial.SelectedIndex = -1;
                                CbAccionEstrategica.SelectedIndex = -1;
                                cargaProductosInsto(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]));

                                cargaResultadoInsto(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), -1);
                                cargaProgramaPresu(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]));
                                cargaUnidades();
                                cargaPilares();
                                rbProductos.SelectedIndex = 1;
                                ProdEstrategicos.Style.Add("display", "block");
                                ProdInsitucionales.Style.Add("display", "none");
                                MultiView1.ActiveViewIndex = 4;
                            }

                            else
                            {




                                Session["CODIGO"] = null;
                                mensaje = "EL producto se registró correctamente";
                                Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',1);", true);
                                //cbResultadoInsitucional.SelectedIndex = -1;
                                cbProgramaPresupuestario.SelectedIndex = -1;
                                cbSuprograma.SelectedIndex = -1;
                                subProgramaPresupuestario.Style.Add("display", "none");
                                cbSuProgramaPresupuestario.SelectedIndex = -1;
                                txtProductoInsto.Text = "";
                                cbUnidadMedida.SelectedIndex = -1;
                                rbaccion.SelectedIndex = -1;
                                AccionEstrategica.Style.Add("display", "none");
                                cpPilarPGG.SelectedIndex = -1;
                                cbObjSectorial.SelectedIndex = -1;
                                CbAccionEstrategica.SelectedIndex = -1;
                                cargaProductosInsto(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]));

                                cargaResultadoInsto(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), -1);
                                cargaProgramaPresu(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]));
                                cargaUnidades();
                                cargaPilares();
                                rbProductos.SelectedIndex = 1;
                                ProdEstrategicos.Style.Add("display", "block");
                                ProdInsitucionales.Style.Add("display", "none");
                                MultiView1.ActiveViewIndex = 4;
                            }

                        }
                    }

                    else
                    {
                        if (Convert.ToInt32(Session["operacion"]) == 10)//INICIO GRABADO ESTRATEGICO
                        {
                            resultado = -1;
                            responsable = busca_responsable(Convert.ToInt32(Session["insto"]));

                            if (responsable == 1)
                            {
                                if (cbAcciones.SelectedIndex != -1)
                                    resulta2 = Convert.ToInt32(cbAcciones.Value);
                                else if (cbAcciones.SelectedIndex == -1 && cbPilar.SelectedIndex != -1)
                                    resulta2 = Convert.ToInt32(cbPilar.Value);

                            }
                            else
                            {
                                if (cbPilar.SelectedIndex != -1)
                                    resulta2 = Convert.ToInt32(cbPilar.Value);
                            }




                            if (resulta2 != -1)
                            {
                                if (responsable == 1)
                                {
                                    sql = "SELECT *  FROM SCHE$SIPLAN20.SPPSVST$PGG2024_2028 WHERE ID_ACCION = " + resulta2;
                                    estado = dao.consulta(sql);
                                    if (estado == 1)
                                    {
                                        tabla = dao.tabla;
                                        if (tabla.Rows.Count > 0)
                                        {
                                            sql = "SELECT * FROM SCHE$SIPLAN20.SP20$RESULTADOS WHERE SPRES$COD_ESTRATEGICO = " + tabla.Rows[0]["ID_ACCION"] + " AND SPRES$TIPO = 0 AND SPRES$RESTRICTIVA = 'N' AND SPRES$POM = " + Convert.ToInt32(Session["pom"]) + " AND SPRES$INSTITUCION = " + Convert.ToInt32(Session["insto"]);
                                            estado = dao.consulta(sql);
                                            if (estado == 1)
                                            {
                                                tabla = dao.tabla;
                                                if (tabla.Rows.Count > 0)
                                                    resultado = Convert.ToInt32(tabla.Rows[0]["SPRES$ID_RESULTADO"]);
                                                else
                                                {
                                                    sql = "INSERT INTO SCHE$SIPLAN20.SP20$RESULTADOS(SPRES$TIPO, SPRES$COD_ESTRATEGICO, SPRES$POM, SPRES$INSTITUCION, SPRES$FECHA_INSERT, SPRES$PROPIETARIO) VALUES (0, " + resulta2 + ", " + Convert.ToInt32(Session["pom"]) + ", " + Convert.ToInt32(Session["insto"]) + ", 'INSERT = '||TO_CHAR(SYSDATE,'DD/MM/YYYY HH24:MI')||' '||'" + Session["USUARIO"].ToString() + "', '" + Session["USUARIO"].ToString() + "')";
                                                    estado = dao.comando(sql);
                                                    if (estado == 1)
                                                    {
                                                        sql = "SELECT MAX(SPRES$ID_RESULTADO) ID FROM SCHE$SIPLAN20.SP20$RESULTADOS WHERE SPRES$COD_ESTRATEGICO = " + resulta2 + " AND SPRES$RESTRICTIVA = 'N' AND SPRES$POM = " + Convert.ToInt32(Session["pom"]) + " AND SPRES$INSTITUCION = " + Convert.ToInt32(Session["insto"]) + " AND SPRES$TIPO = 0";
                                                        estado = dao.consulta(sql);
                                                        if (estado == 1)
                                                        {
                                                            tabla = dao.tabla;
                                                            if (tabla.Rows.Count > 0)
                                                            {
                                                                resultado = Convert.ToInt32(tabla.Rows[0]["ID"]);
                                                            }
                                                            else
                                                                resultado = -1;
                                                        }
                                                    }

                                                }

                                            }
                                        }


                                        else if (tabla.Rows.Count <= 0)
                                        {
                                            sql = @" SELECT
                                                    RG.SPRPG$ID_PGG ID_EJE
                                                    ,R.SPPRES$DESCRIPCION EJE                         
                                                    FROM SCHE$SIPLAN20.SP20$RESULTADOS_ESTRATEGICOS R 
                                                    INNER JOIN SCHE$SIPLAN20.SP20$RESPONSABLESPGG RG ON RG.SPRPG$ID_PGG = R.SPPRES$ID_RESULTADO AND RG.SPRPG$NIVEL  = 1 AND R.SPPRES$RESTRICTIVA = 'N' AND RG.SPRPG$RESTRICTIVA = 'N'
                                                    WHERE RG.SPRPG$RESPONSABLE = " + Convert.ToInt32(Session["insto"]) + @" AND RG.SPRPG$ID_PGG = " + resulta2;
                                            estado = dao.consulta(sql);
                                            if (estado == 1)
                                            {
                                                tabla = dao.tabla;
                                                if (tabla.Rows.Count > 0)
                                                {
                                                    sql = "SELECT * FROM SCHE$SIPLAN20.SP20$RESULTADOS WHERE SPRES$COD_ESTRATEGICO = " + tabla.Rows[0]["ID_EJE"] + " AND SPRES$TIPO = 0 AND SPRES$RESTRICTIVA = 'N' AND SPRES$POM = " + Convert.ToInt32(Session["pom"]) + " AND SPRES$INSTITUCION = " + Convert.ToInt32(Session["insto"]);
                                                    estado = dao.consulta(sql);

                                                    if (estado == 1)
                                                    {
                                                        tabla = dao.tabla;
                                                        if (tabla.Rows.Count > 0)
                                                            resultado = Convert.ToInt32(tabla.Rows[0]["SPRES$ID_RESULTADO"]);
                                                        else
                                                        {
                                                            sql = "INSERT INTO SCHE$SIPLAN20.SP20$RESULTADOS(SPRES$TIPO, SPRES$COD_ESTRATEGICO, SPRES$POM, SPRES$INSTITUCION, SPRES$FECHA_INSERT, SPRES$PROPIETARIO) VALUES (0, " + resulta2 + ", " + Convert.ToInt32(Session["pom"]) + ", " + Convert.ToInt32(Session["insto"]) + ", 'INSERT = '||TO_CHAR(SYSDATE,'DD/MM/YYYY HH24:MI')||' '||'" + Session["USUARIO"].ToString() + "', '" + Session["USUARIO"].ToString() + "')";
                                                            estado = dao.comando(sql);
                                                            if (estado == 1)
                                                            {
                                                                sql = "SELECT MAX(SPRES$ID_RESULTADO) ID FROM SCHE$SIPLAN20.SP20$RESULTADOS WHERE SPRES$COD_ESTRATEGICO = " + resulta2 + " AND SPRES$RESTRICTIVA = 'N' AND SPRES$POM = " + Convert.ToInt32(Session["pom"]) + " AND SPRES$INSTITUCION = " + Convert.ToInt32(Session["insto"]) + " AND SPRES$TIPO = 0";
                                                                estado = dao.consulta(sql);
                                                                if (estado == 1)
                                                                {
                                                                    tabla = dao.tabla;
                                                                    if (tabla.Rows.Count > 0)
                                                                    {
                                                                        resultado = Convert.ToInt32(tabla.Rows[0]["ID"]);
                                                                    }
                                                                    else
                                                                        resultado = resulta2;
                                                                }
                                                            }

                                                        }

                                                    }

                                                }
                                                else
                                                    resultado = resulta2;
                                            }


                                        }

                                        else
                                            resultado = -1;
                                    }



                                }

                                else if (responsable == 0)

                                {
                                    //sql = "SELECT *  FROM SCHE$SIPLAN20.SPPSVST$PGG2024_2028 WHERE ID_EJE = " + resulta2;
                                    sql = @"SELECT 
                                                    ID_EJE
                                                    ,EJE
                                                    FROM
                                                    (SELECT ID_EJE, EJE FROM SCHE$SIPLAN20.SPPSVST$PGG2024_2028
                                                    UNION
                                                    SELECT R.SPPRES$ID_RESULTADO ID_EJE, R.SPPRES$CODIGO || '-' || R.SPPRES$DESCRIPCION EJE FROM SCHE$SIPLAN20.SP20$RESULTADOS_ESTRATEGICOS R
                                                    INNER JOIN SCHE$SIPLAN20.S20$PERIODO_RESULTADO PR ON PR.SPR$ID_RESULTADO = R.SPPRES$ID_RESULTADO AND PR.SPR$RESTRICTIVA = 'N' AND R.SPPRES$RESTRICTIVA = 'N'
                                                    INNER JOIN SCHE$SIPLAN20.SP20$PERIODO_GOBIERNO PG ON PG.SPG$ID_PERIODO = PR.SPR$ID_PERIODO AND PG.SPG$RESTRICTIVA = 'N' AND PR.SPR$RESTRICTIVA = 'N' AND PG.SPG$VIGENTE = 1) WHERE ID_EJE = " + resulta2;
                                    estado = dao.consulta(sql);
                                    if (estado == 1)
                                    {
                                        tabla = dao.tabla;
                                        if (tabla.Rows.Count > 0)
                                        {
                                            sql = "SELECT * FROM SCHE$SIPLAN20.SP20$RESULTADOS WHERE SPRES$COD_ESTRATEGICO = " + tabla.Rows[0]["ID_EJE"] + " AND SPRES$TIPO = 0 AND SPRES$RESTRICTIVA = 'N' AND SPRES$POM = " + Convert.ToInt32(Session["pom"]) + " AND SPRES$INSTITUCION = " + Convert.ToInt32(Session["insto"]);
                                            estado = dao.consulta(sql);
                                            if (estado == 1)
                                            {
                                                tabla = dao.tabla;
                                                if (tabla.Rows.Count > 0)
                                                    resultado = Convert.ToInt32(tabla.Rows[0]["SPRES$ID_RESULTADO"]);
                                                else
                                                {
                                                    
                                                    sql = "INSERT INTO SCHE$SIPLAN20.SP20$RESULTADOS(SPRES$TIPO, SPRES$COD_ESTRATEGICO, SPRES$POM, SPRES$INSTITUCION, SPRES$FECHA_INSERT, SPRES$PROPIETARIO) VALUES (0, " + resulta2 + ", " + Convert.ToInt32(Session["pom"]) + ", " + Convert.ToInt32(Session["insto"]) + ", 'INSERT = '||TO_CHAR(SYSDATE,'DD/MM/YYYY HH24:MI')||' '||'" + Session["USUARIO"].ToString() + "', '" + Session["USUARIO"].ToString() + "')";
                                                    estado = dao.comando(sql);
                                                    if (estado == 1)
                                                    {
                                                        sql = "SELECT MAX(SPRES$ID_RESULTADO) ID FROM SCHE$SIPLAN20.SP20$RESULTADOS WHERE SPRES$COD_ESTRATEGICO = " + resulta2 + " AND SPRES$RESTRICTIVA = 'N' AND SPRES$POM = " + Convert.ToInt32(Session["pom"]) + " AND SPRES$INSTITUCION = " + Convert.ToInt32(Session["insto"]) + " AND SPRES$TIPO = 0";
                                                        estado = dao.consulta(sql);
                                                        if (estado == 1)
                                                        {
                                                            tabla = dao.tabla;
                                                            if (tabla.Rows.Count > 0)
                                                            {
                                                                resultado = Convert.ToInt32(tabla.Rows[0]["ID"]);
                                                            }
                                                            else
                                                                resultado = -1;
                                                        }
                                                    }

                                                }

                                            }
                                        }
                                        else
                                            resultado = -1;
                                    }
                                }


                            }



                            sql = "SELECT * FROM SCHE$SIPLAN20.SP20$RESULTADOS R WHERE R.SPRES$POM = " + Convert.ToInt32(Session["pom"]) + " AND R.SPRES$INSTITUCION = " + Convert.ToInt32(Session["insto"]) + " AND R.SPRES$TIPO = 2 AND R.SPRES$COD_ESTRATEGICO = " + cbRed.Value + " AND SPRES$RESTRICTIVA = 'N'";
                            estado = dao.consulta(sql);
                            if (estado == 1)
                            {
                                red_resultado = dao.tabla;
                                if (red_resultado.Rows.Count > 0)
                                    red_producto = Convert.ToInt32(red_resultado.Rows[0]["SPRES$ID_RESULTADO"]);
                                else
                                {
                                    sql = "INSERT  INTO SCHE$SIPLAN20.SP20$RESULTADOS (SPRES$TIPO, SPRES$COD_ESTRATEGICO, SPRES$FECHA_INSERT, SPRES$POM, SPRES$INSTITUCION, SPRES$PROPIETARIO) VALUES(2, " + cbRed.Value + ", 'INSERT = '||TO_CHAR(SYSDATE,'DD/MM/YYYY HH24:MI')||' '||'" + Session["USUARIO"].ToString() + "', " + Convert.ToInt32(Session["pom"]) + ", " + Convert.ToInt32(Session["insto"]) + ", '" + Session["USUARIO"].ToString() + "')";
                                    estado = dao.comando(sql);
                                    if (estado == 1)
                                    {
                                        sql = "SELECT MAX(SPRES$ID_RESULTADO) ID FROM SCHE$SIPLAN20.SP20$RESULTADOS R WHERE R.SPRES$POM = " + Convert.ToInt32(Session["pom"]) + " AND R.SPRES$INSTITUCION = " + Convert.ToInt32(Session["insto"]) + " AND R.SPRES$TIPO = 2 AND R.SPRES$COD_ESTRATEGICO = " + cbRed.Value + " AND SPRES$RESTRICTIVA = 'N'";
                                        estado = dao.consulta(sql);
                                        if (estado == 1)
                                        {
                                            red_resultado = dao.tabla;
                                            if (red_resultado.Rows.Count > 0)
                                                red_producto = Convert.ToInt32(red_resultado.Rows[0]["ID"]);
                                        }
                                    }
                                }



                            }



                            sql = "INSERT INTO SCHE$SIPLAN20.SP20$PRODUCTO (SPPRO$DESCRIPCION, SPPRO$ID_MEDIDA, SPPRO$ID_RESULTADO, SPPRO$FECHA_INSERTA, SPPRO$PRESUPUESTO, SPPRO$POM, SPPRO$INSTO, SPPRO$PROPIETARIO,SPPRO$RESULTADO2) VALUES ('" + txtProductoInsto.Text + "', " + Convert.ToInt32(cbUnidadMedida.Value) + ", " + Convert.ToInt32(red_producto) + ", 'INSERT = '||TO_CHAR(SYSDATE,'DD/MM/YYYY HH24:MI')||' '||'" + Session["USUARIO"].ToString() + "', ";
                            if (cbSuprograma.SelectedIndex == 0)
                            {
                                sql = sql + Convert.ToDouble(cbProgramaPresupuestario.Value) + ", ";
                                sql = sql + Convert.ToInt32(Session["pom"]) + ", " + Convert.ToInt32(Session["insto"]) + ", '" + Session["USUARIO"].ToString() + "'";
                            }
                            else
                            {
                                sql = sql + Convert.ToDouble(cbSuProgramaPresupuestario.Value) + ", ";
                                sql = sql + Convert.ToInt32(Session["pom"]) + ", " + Convert.ToInt32(Session["insto"]) + ", '" + Session["USUARIO"].ToString() + "'";
                            }

                            sql = sql + "," + resultado + ")";


                        }//FIN GRABADO ESTRATEGICO
                        else if (Convert.ToInt32(Session["operacion"]) == 11)//INICIO EDICION ESTRATEGICO
                        {


                            responsable = busca_responsable(Convert.ToInt32(Session["insto"]));

                            if (responsable == 1)
                            {
                                if (cbAcciones.SelectedIndex != -1)
                                    resulta1 = Convert.ToInt32(cbAcciones.Value);
                                else if (cbPilar.SelectedIndex != -1)
                                    resulta1 = Convert.ToInt32(cbPilar.Value);

                            }
                            else
                            {
                                if (cbPilar.SelectedIndex != -1)
                                    resulta1 = Convert.ToInt32(cbPilar.Value);
                            }

                            if (responsable == 1)
                            {
                                sql = "SELECT *  FROM SCHE$SIPLAN20.SPPSVST$PGG2024_2028 WHERE ID_ACCION = " + resulta1;
                                estado = dao.consulta(sql);
                                if (estado == 1)
                                {
                                    tabla = dao.tabla;
                                    if (tabla.Rows.Count > 0)
                                    {
                                        sql = "SELECT * FROM SCHE$SIPLAN20.SP20$RESULTADOS WHERE SPRES$COD_ESTRATEGICO = " + tabla.Rows[0]["ID_ACCION"] + " AND SPRES$TIPO = 0 AND SPRES$RESTRICTIVA = 'N' AND SPRES$POM = " + Convert.ToInt32(Session["pom"]) + " AND SPRES$INSTITUCION = " + Convert.ToInt32(Session["insto"]);
                                        estado = dao.consulta(sql);
                                        if (estado == 1)
                                        {
                                            tabla = dao.tabla;
                                            if (tabla.Rows.Count > 0)
                                                resultado = Convert.ToInt32(tabla.Rows[0]["SPRES$ID_RESULTADO"]);
                                            else
                                            {
                                                sql = "INSERT INTO SCHE$SIPLAN20.SP20$RESULTADOS(SPRES$TIPO, SPRES$COD_ESTRATEGICO, SPRES$POM, SPRES$INSTITUCION, SPRES$FECHA_INSERT, SPRES$PROPIETARIO) VALUES (0, " + resulta1 + ", " + Convert.ToInt32(Session["pom"]) + ", " + Convert.ToInt32(Session["insto"]) + ", 'INSERT = '||TO_CHAR(SYSDATE,'DD/MM/YYYY HH24:MI')||' '||'" + Session["USUARIO"].ToString() + "', '" + Session["USUARIO"].ToString() + "')";
                                                estado = dao.comando(sql);
                                                if (estado == 1)
                                                {
                                                    sql = "SELECT MAX(SPRES$ID_RESULTADO) ID FROM SCHE$SIPLAN20.SP20$RESULTADOS WHERE SPRES$COD_ESTRATEGICO = " + resulta1 + " AND SPRES$RESTRICTIVA = 'N' AND SPRES$POM = " + Convert.ToInt32(Session["pom"]) + " AND SPRES$INSTITUCION = " + Convert.ToInt32(Session["insto"]) + " AND SPRES$TIPO = 0";
                                                    estado = dao.consulta(sql);
                                                    if (estado == 1)
                                                    {
                                                        tabla = dao.tabla;
                                                        if (tabla.Rows.Count > 0)
                                                        {
                                                            resultado = Convert.ToInt32(tabla.Rows[0]["ID"]);
                                                        }
                                                        else
                                                            resultado = -1;
                                                    }
                                                }

                                            }

                                        }
                                    }
                                    else if (tabla.Rows.Count <= 0)
                                    {
                                        sql = @"SELECT
                                                    RG.SPRPG$ID_PGG ID_EJE
                                                    , R.SPPRES$DESCRIPCION EJE
                                                    FROM SCHE$SIPLAN20.SP20$RESULTADOS_ESTRATEGICOS R
                                                    INNER JOIN SCHE$SIPLAN20.SP20$RESPONSABLESPGG RG ON RG.SPRPG$ID_PGG = R.SPPRES$ID_RESULTADO AND RG.SPRPG$NIVEL = 1 AND R.SPPRES$RESTRICTIVA = 'N' AND RG.SPRPG$RESTRICTIVA = 'N'
                                                    WHERE RG.SPRPG$RESPONSABLE = " + Convert.ToInt32(Session["insto"]) + " AND RG.SPRPG$ID_PGG = " + resulta1;
                                        estado = dao.consulta(sql);
                                        if (estado == 1)
                                        {
                                            tabla = dao.tabla;
                                            sql = "SELECT * FROM SCHE$SIPLAN20.SP20$RESULTADOS WHERE SPRES$COD_ESTRATEGICO = " + tabla.Rows[0]["ID_EJE"] + " AND SPRES$TIPO = 0 AND SPRES$RESTRICTIVA = 'N' AND SPRES$POM = " + Convert.ToInt32(Session["pom"]) + " AND SPRES$INSTITUCION = " + Convert.ToInt32(Session["insto"]);
                                            estado = dao.consulta(sql);
                                            if (estado == 1)
                                            {
                                                tabla = dao.tabla;
                                                if (tabla.Rows.Count > 0)
                                                    resultado = Convert.ToInt32(tabla.Rows[0]["SPRES$ID_RESULTADO"]);
                                                else
                                                {
                                                    sql = "INSERT INTO SCHE$SIPLAN20.SP20$RESULTADOS(SPRES$TIPO, SPRES$COD_ESTRATEGICO, SPRES$POM, SPRES$INSTITUCION, SPRES$FECHA_INSERT, SPRES$PROPIETARIO) VALUES (0, " + resulta1 + ", " + Convert.ToInt32(Session["pom"]) + ", " + Convert.ToInt32(Session["insto"]) + ", 'INSERT = '||TO_CHAR(SYSDATE,'DD/MM/YYYY HH24:MI')||' '||'" + Session["USUARIO"].ToString() + "', '" + Session["USUARIO"].ToString() + "')";
                                                    estado = dao.comando(sql);
                                                    if (estado == 1)
                                                    {
                                                        sql = "SELECT MAX(SPRES$ID_RESULTADO) ID FROM SCHE$SIPLAN20.SP20$RESULTADOS WHERE SPRES$COD_ESTRATEGICO = " + resulta1 + " AND SPRES$RESTRICTIVA = 'N' AND SPRES$POM = " + Convert.ToInt32(Session["pom"]) + " AND SPRES$INSTITUCION = " + Convert.ToInt32(Session["insto"]) + " AND SPRES$TIPO = 0";
                                                        estado = dao.consulta(sql);
                                                        if (estado == 1)
                                                        {
                                                            tabla = dao.tabla;
                                                            if (tabla.Rows.Count > 0)
                                                            {
                                                                resultado = Convert.ToInt32(tabla.Rows[0]["ID"]);
                                                            }
                                                            else
                                                                resultado = -1;
                                                        }
                                                    }

                                                }

                                            }
                                        }



                                    }
                                    else
                                        resultado = -1;
                                }



                            }

                            else
                            {
                                //sql = "SELECT *  FROM SCHE$SIPLAN20.SPPSVST$PGG2024_2028 WHERE ID_EJE = " + resulta1;
                                sql = @"SELECT 
                                                    ID_EJE
                                                    ,EJE
                                                    FROM
                                                    (SELECT ID_EJE, EJE FROM SCHE$SIPLAN20.SPPSVST$PGG2024_2028
                                                    UNION
                                                    SELECT R.SPPRES$ID_RESULTADO ID_EJE, R.SPPRES$CODIGO || '-' || R.SPPRES$DESCRIPCION EJE FROM SCHE$SIPLAN20.SP20$RESULTADOS_ESTRATEGICOS R
                                                    INNER JOIN SCHE$SIPLAN20.S20$PERIODO_RESULTADO PR ON PR.SPR$ID_RESULTADO = R.SPPRES$ID_RESULTADO AND PR.SPR$RESTRICTIVA = 'N' AND R.SPPRES$RESTRICTIVA = 'N'
                                                    INNER JOIN SCHE$SIPLAN20.SP20$PERIODO_GOBIERNO PG ON PG.SPG$ID_PERIODO = PR.SPR$ID_PERIODO AND PG.SPG$RESTRICTIVA = 'N' AND PR.SPR$RESTRICTIVA = 'N' AND PG.SPG$VIGENTE = 1) WHERE ID_EJE = " + resulta1;
                                estado = dao.consulta(sql);
                                if (estado == 1)
                                {
                                    tabla = dao.tabla;
                                    if (tabla.Rows.Count > 0)
                                    {
                                        sql = "SELECT * FROM SCHE$SIPLAN20.SP20$RESULTADOS WHERE SPRES$COD_ESTRATEGICO = " + tabla.Rows[0]["ID_EJE"] + " AND SPRES$TIPO = 0 AND SPRES$RESTRICTIVA = 'N' AND SPRES$POM = " + Convert.ToInt32(Session["pom"]) + " AND SPRES$INSTITUCION = " + Convert.ToInt32(Session["insto"]);
                                        estado = dao.consulta(sql);
                                        if (estado == 1)
                                        {
                                            tabla = dao.tabla;
                                            if (tabla.Rows.Count > 0)
                                                resultado = Convert.ToInt32(tabla.Rows[0]["SPRES$ID_RESULTADO"]);
                                            else
                                            {
                                                sql = "INSERT INTO SCHE$SIPLAN20.SP20$RESULTADOS(SPRES$TIPO, SPRES$COD_ESTRATEGICO, SPRES$POM, SPRES$INSTITUCION, SPRES$FECHA_INSERT, SPRES$PROPIETARIO) VALUES (0, " + resulta1 + ", " + Convert.ToInt32(Session["pom"]) + ", " + Convert.ToInt32(Session["insto"]) + ", 'INSERT = '||TO_CHAR(SYSDATE,'DD/MM/YYYY HH24:MI')||' '||'" + Session["USUARIO"].ToString() + "', '" + Session["USUARIO"].ToString() + "')";
                                                estado = dao.comando(sql);
                                                if (estado == 1)
                                                {
                                                    sql = "SELECT MAX(SPRES$ID_RESULTADO) ID FROM SCHE$SIPLAN20.SP20$RESULTADOS WHERE SPRES$COD_ESTRATEGICO = " + resulta1 + " AND SPRES$RESTRICTIVA = 'N' AND SPRES$POM = " + Convert.ToInt32(Session["pom"]) + " AND SPRES$INSTITUCION = " + Convert.ToInt32(Session["insto"]) + " AND SPRES$TIPO = 0";
                                                    estado = dao.consulta(sql);
                                                    if (estado == 1)
                                                    {
                                                        tabla = dao.tabla;
                                                        if (tabla.Rows.Count > 0)
                                                        {
                                                            resultado = Convert.ToInt32(tabla.Rows[0]["ID"]);
                                                        }
                                                        else
                                                            resultado = -1;
                                                    }
                                                }

                                            }

                                        }
                                    }
                                    else
                                        resultado = -1;


                                }
                            }

                            //if ((gvProdEstrategicos.GetRowValues(gvProdEstrategicos.FocusedRowIndex, "SPPRO$PROPIETARIO").ToString() == Session["USUARIO"].ToString()) || Session["ROL"].ToString() == "ADMIN")
                            if (Session["ROL"].ToString() != "ENTIDAD")
                            {

                                sql = "SELECT * FROM SCHE$SIPLAN20.SP20$RESULTADOS R WHERE R.SPRES$POM = " + Convert.ToInt32(Session["pom"]) + " AND R.SPRES$INSTITUCION = " + Convert.ToInt32(Session["insto"]) + " AND R.SPRES$TIPO = 2 AND R.SPRES$COD_ESTRATEGICO = " + Convert.ToInt32(cbRed.Value) + " AND SPRES$RESTRICTIVA = 'N'";
                                estado = dao.consulta(sql);
                                if (estado == 1)
                                {
                                    red_resultado = dao.tabla;
                                    if (red_resultado.Rows.Count > 0)
                                        red_producto = Convert.ToInt32(red_resultado.Rows[0]["SPRES$ID_RESULTADO"]);
                                    else
                                    {
                                        sql = "INSERT  INTO SCHE$SIPLAN20.SP20$RESULTADOS (SPRES$TIPO, SPRES$COD_ESTRATEGICO, SPRES$FECHA_INSERT, SPRES$POM, SPRES$INSTITUCION, SPRES$PROPIETARIO) VALUES(2, " + cbRed.Value + ", 'INSERT = '||TO_CHAR(SYSDATE,'DD/MM/YYYY HH24:MI')||' '||'" + Session["USUARIO"].ToString() + "', " + Convert.ToInt32(Session["pom"]) + ", " + Convert.ToInt32(Session["insto"]) + ", '" + Session["USUARIO"].ToString() + "')";
                                        estado = dao.comando(sql);
                                        if (estado == 1)
                                        {
                                            sql = "SELECT MAX(SPRES$ID_RESULTADO) ID FROM SCHE$SIPLAN20.SP20$RESULTADOS R WHERE R.SPRES$POM = " + Convert.ToInt32(Session["pom"]) + " AND R.SPRES$INSTITUCION = " + Convert.ToInt32(Session["insto"]) + " AND R.SPRES$TIPO = 2 AND R.SPRES$COD_ESTRATEGICO = " + cbRed.Value + " AND SPRES$RESTRICTIVA = 'N'";
                                            estado = dao.consulta(sql);
                                            if (estado == 1)
                                            {
                                                red_resultado = dao.tabla;
                                                if (red_resultado.Rows.Count > 0)
                                                    red_producto = Convert.ToInt32(red_resultado.Rows[0]["ID"]);
                                            }
                                        }
                                    }



                                }


                                codigo = Convert.ToInt32(gvProdEstrategicos.GetRowValues(gvProdEstrategicos.FocusedRowIndex, "SPPRO$ID_PRODUCTO").ToString());
                                sql = "UPDATE SCHE$SIPLAN20.SP20$PRODUCTO SET SPPRO$DESCRIPCION ='" + txtProductoInsto.Text + "', SPPRO$ID_MEDIDA = " + Convert.ToInt32(cbUnidadMedida.Value) + ", SPPRO$ID_RESULTADO =" + red_producto + ", SPPRO$FECHA_UPDATE = 'UPDATE = '||TO_CHAR(SYSDATE,'DD/MM/YYYY HH24:MI')||' '||'" + Session["USUARIO"].ToString() + "', SPPRO$RESULTADO2 = " + resultado + ",";
                                //if (rbaccion.SelectedIndex == 0)
                                //{

                                if (cbSuprograma.SelectedIndex == 0)
                                {
                                    sql = sql + "SPPRO$PRESUPUESTO = " + Convert.ToDouble(cbProgramaPresupuestario.Value) + ", SPPRO$OBJETIVO_CENTRAL = NULL, SPPRO$ACCION_ESTRATEGICA = NULL  ";

                                }
                                else
                                {
                                    sql = sql + "SPPRO$PRESUPUESTO = " + Convert.ToDouble(cbSuProgramaPresupuestario.Value) + ", SPPRO$OBJETIVO_CENTRAL = NULL, SPPRO$ACCION_ESTRATEGICA = NULL ";
                                }

                                //}

                                //else if (rbaccion.SelectedIndex == 1)
                                //{
                                //    if (cbSuprograma.SelectedIndex == 0)
                                //    {
                                //        sql = sql + " SPPRO$PRESUPUESTO = " + Convert.ToInt32(cbProgramaPresupuestario.Value) + ", SPPRO$OBJETIVO_CENTRAL = " + Convert.ToInt32(cbObjSectorial.Value) + ", SPPRO$ACCION_ESTRATEGICA = " + Convert.ToInt32(CbAccionEstrategica.Value);
                                //    }
                                //    else
                                //    {
                                //        sql = sql + " SPPRO$PRESUPUESTO = " + Convert.ToInt32(cbSuProgramaPresupuestario.Value) + ", SPPRO$OBJETIVO_CENTRAL = " + Convert.ToInt32(cbObjSectorial.Value) + ", SPPRO$ACCION_ESTRATEGICA = " + Convert.ToInt32(CbAccionEstrategica.Value);
                                //    }


                                //}
                                sql = sql + " WHERE SPPRO$POM = " + Convert.ToInt32(Session["pom"]) + " AND SPPRO$INSTO = " + Convert.ToInt32(Session["insto"]) + " AND SPPRO$ID_PRODUCTO = " + codigo + " AND SPPRO$RESTRICTIVA = 'N'";


                            }

                            else
                            {
                                mensaje = "Su perfil no usuario no esta autorizado para modificar este registro";
                                Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
                            }

                        }//FIN EDICION ESTRATEGICO

                        estado = dao.comando(sql);

                        if (estado == 0)
                        {
                            mensaje = dao.mensaje;
                            Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
                            //cbResultadoInsitucional.SelectedIndex = -1;
                            cbProgramaPresupuestario.SelectedIndex = -1;
                            cbSuprograma.SelectedIndex = -1;
                            subProgramaPresupuestario.Style.Add("display", "none");
                            cbSuProgramaPresupuestario.SelectedIndex = -1;
                            txtProductoInsto.Text = "";
                            cbUnidadMedida.SelectedIndex = -1;
                            rbaccion.SelectedIndex = -1;
                            AccionEstrategica.Style.Add("display", "none");
                            cpPilarPGG.SelectedIndex = -1;
                            cbObjSectorial.SelectedIndex = -1;
                            CbAccionEstrategica.SelectedIndex = -1;
                            cargaProductosInsto(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]));
                            cargaProductoRed(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]));
                            cargaResultadoInsto(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), -1);
                            cargaProgramaPresu(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]));
                            cargaUnidades();
                            cargaPilares();
                            rbProductos.SelectedIndex = 1;
                            ProdEstrategicos.Style.Add("display", "block");
                            ProdInsitucionales.Style.Add("display", "none");
                            MultiView1.ActiveViewIndex = 4;
                        }

                        else
                        {

                            Session["CODIGO"] = null;
                            mensaje = "El producto se registró correctamente";
                            Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',1);", true);
                            //cbResultadoInsitucional.SelectedIndex = -1;
                            cbProgramaPresupuestario.SelectedIndex = -1;
                            cbSuprograma.SelectedIndex = -1;
                            subProgramaPresupuestario.Style.Add("display", "none");
                            cbSuProgramaPresupuestario.SelectedIndex = -1;
                            txtProductoInsto.Text = "";
                            cbUnidadMedida.SelectedIndex = -1;
                            rbaccion.SelectedIndex = -1;
                            AccionEstrategica.Style.Add("display", "none");
                            cpPilarPGG.SelectedIndex = -1;
                            cbObjSectorial.SelectedIndex = -1;
                            CbAccionEstrategica.SelectedIndex = -1;
                            cargaProductosInsto(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]));
                            CargaProductosRed(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]));
                            cargaResultadoInsto(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), -1);
                            cargaProgramaPresu(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]));
                            cargaUnidades();
                            cargaPilares();
                            rbProductos.SelectedIndex = 1;
                            ProdEstrategicos.Style.Add("display", "block");
                            ProdInsitucionales.Style.Add("display", "none");


                            MultiView1.ActiveViewIndex = 4;
                        }


                    }

                }







            }


        }

        protected void btnCancelaProdInsto_Click(object sender, EventArgs e)
        {
            //cbResultadoInsitucional.SelectedIndex = -1;
            cbProgramaPresupuestario.SelectedIndex = -1;
            cbSuprograma.SelectedIndex = -1;
            subProgramaPresupuestario.Style.Add("display", "none");
            cbSuProgramaPresupuestario.SelectedIndex = -1;
            txtProductoInsto.Text = "";
            cbUnidadMedida.SelectedIndex = -1;
            rbaccion.SelectedIndex = -1;
            AccionEstrategica.Style.Add("display", "none");
            cpPilarPGG.SelectedIndex = -1;
            cbObjSectorial.SelectedIndex = -1;
            CbAccionEstrategica.SelectedIndex = -1;
            cargaProductosInsto(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]));
            cargaResultadoInsto(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), -1);
            cargaProgramaPresu(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]));
            cargaUnidades();
            cargaPilares();
            rbProductos.SelectedIndex = 0;
            ProdInsitucionales.Style.Add("display", "block");
            ProdEstrategicos.Style.Add("display", "none");
            MultiView1.ActiveViewIndex = 4;

        }

        protected void btnNuevo_Click(object sender, EventArgs e)
        {

            Session["CODIGO"] = -1;
            //cargaMunicipios(-1);
            int busca_responsables = -1;          
            cbPGG.SelectedIndex = 1;
            cbPGG.Enabled = false;
            panPGGProductos.Style.Add("display", "block");
            esProdAdministartivo.Value = "-1";

            if (rbProductos.SelectedIndex == 0)
            {
                panRed.Style.Add("display", "none");
                panResultadoInstitucionales.Style.Add("display", "block");

                busca_responsables = busca_responsable(Convert.ToInt32(Session["insto"]));
                lblProducto.Text = "Nuevo producto";
                lblTipo.Text = "Producto institucional";
                lblTipoDescripcion.Text = "producto institucional";
                lblProducto.Style.Add("color", "#2d572c");

                lblPilar.Visible = false;
                lblResultado.Visible = false;
                Session["tipo"] = "institucional";
                Session["operacion"] = 0;
                //cbResultadoInsitucional.Enabled = true;
                cbResultados.Enabled = true;
                cbProgramaPresupuestario.Enabled = true;
                cbSuprograma.Enabled = true;
                subProgramaPresupuestario.Enabled = true;
                cbSuProgramaPresupuestario.Enabled = true;
                txtProductoInsto.Enabled = true;
                cbUnidadMedida.Enabled = true;
                rbaccion.Enabled = false;
                rbaccion.SelectedIndex = 0;
                //cpPilarPGG.Enabled = true;

                cargaPilares(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), -1, busca_responsables);
                cargaResultadoInsto(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), -1);
                /*------------*/
                cbObjSectorial.Enabled = true;
                CbAccionEstrategica.Enabled = true;
                /*------------*/
                btnGrabaProdInsto.CssClass = "btn btn-primary";
                btnGrabaProdInsto.Text = "Producto institucional";
                MultiView1.ActiveViewIndex = 5;
            }
            else if (rbProductos.SelectedIndex == 1)//inicio para productos RED
            {
                busca_responsables = busca_responsable(Convert.ToInt32(Session["insto"]));
                panResultadoInstitucionales.Style.Add("display", "none");
                //panPGGProductos.Style.Add("display", "none");
                cargaPilares(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), -1, busca_responsables);
                panRed.Style.Add("display", "block");

                lblProducto.Text = "Nuevo producto vinculado a Resultado Estratégico (RE)";
                lblTipo.Text = "Producto vinculado a Resultado Estratégico (RE)";
                lblTipoDescripcion.Text = "producto vinculado a Resultado Estratégico (RE)";
                lblProducto.Style.Add("color", "#2d572c");
                busca_responsables = busca_responsable_red(Convert.ToInt32(Session["insto"]));
                busca_red(Convert.ToInt32(Session["insto"]), busca_responsables, -1);
                lblPilar.Visible = false;
                lblResultado.Visible = false;
                Session["tipo"] = "estrategico";
                Session["operacion"] = 10;
                cbPilar.Enabled = true;
                //cbResultadoInsitucional.Enabled = true;
                cbProgramaPresupuestario.Enabled = true;
                cbSuprograma.Enabled = true;
                subProgramaPresupuestario.Enabled = true;
                cbSuProgramaPresupuestario.Enabled = true;
                txtProductoInsto.Enabled = true;
                cbUnidadMedida.Enabled = true;
                rbaccion.Enabled = true;
                cpPilarPGG.Enabled = true;
                cbObjSectorial.Enabled = true;
                CbAccionEstrategica.Enabled = true;
                rbaccion.Enabled = false;
                btnGrabaProdInsto.CssClass = "btn btn-primary";
                btnGrabaProdInsto.Text = "Producto RE";
                MultiView1.ActiveViewIndex = 5;

            }


        }

        protected int busca_responsable(int insto)
        {
            DataTable respo = new DataTable();
            int resultado = 0;
            //sql = "SELECT * FROM SCHE$SIPLAN20.SPPSVST$PGG2024_2028 WHERE RESPONSABLE = " + insto;

            sql = @"SELECT
                    count(*) conteo
                    FROM
                    (SELECT ID_ACCION
                            , EJE
                            , META
                            , ACCION_ESTRATEGICA
                            , ID_EJE FROM SCHE$SIPLAN20.SPPSVST$PGG2024_2028 WHERE RESPONSABLE = " + insto + @"
                            UNION
                            SELECT ID_ACCION
                            , EJE
                            , META
                            , ACCION_ESTRATEGICA
                            , ID_EJE FROM SCHE$SIPLAN20.SPPSVST$PGG2024_2028 INNER JOIN SCHE$SIPLAN20.SP20$RESPONSABLESPGG RG ON RG.SPRPG$ID_PGG = ID_ACCION AND SPRPG$NIVEL = 3 AND SPRPG$RESPONSABLE = " + insto + @"

                            UNION

                            SELECT
                            -1 ID_ACCION
                            ,R.SPPRES$DESCRIPCION EJE
                            ,' ' META
                            ,' ' ACCION_ESTRATEGICA
                            ,RG.SPRPG$ID_PGG ID_EJE
                            FROM SCHE$SIPLAN20.SP20$RESULTADOS_ESTRATEGICOS R 
                            INNER JOIN SCHE$SIPLAN20.SP20$RESPONSABLESPGG RG ON RG.SPRPG$ID_PGG = R.SPPRES$ID_RESULTADO AND RG.SPRPG$NIVEL  = 1 AND R.SPPRES$RESTRICTIVA = 'N' AND RG.SPRPG$RESTRICTIVA = 'N'
                            WHERE RG.SPRPG$RESPONSABLE = " + insto + @"
                    )

                ORDER BY ID_EJE
                ,ID_ACCION
                ASC";
            estado = dao.consulta(sql);
            if (estado == 1)
                respo = dao.tabla;
            if (respo.Rows.Count > 0)
            {
                if (Convert.ToInt32(respo.Rows[0]["CONTEO"]) > 0)
                    resultado = 1;
                else
                    resultado = 0;
            }
               
            else
                resultado = 0;

            return resultado;
        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            int nivel, codproducto, codresultado, codRed;
            String producto, red;
            DataTable tempo1 = new DataTable();
            DataTable tempo2 = new DataTable();
            DataTable cod_estrategico = new DataTable();
            DataTable tabla_eje = new DataTable();
            int eje = -1;
            int esAdmin = -1; 

            AccionEstrategica.Style.Add("display", "none");
            int responsable = -1;

            if (rbProductos.SelectedIndex == 0)
            {

                if (gvProdInsto.FocusedRowIndex != -1)
                {
                    nivel = gvProdInsto.GetRowLevel(gvProdInsto.FocusedRowIndex);
                    producto = gvProdInsto.GetRowValues(gvProdInsto.FocusedRowIndex, "PRODUCTO").ToString();
                    esAdmin = Convert.ToInt32(gvProdInsto.GetRowValues(gvProdInsto.FocusedRowIndex, "SPPRO$ES_ADMINISTRATIVO").ToString());
                    if (nivel == 0)
                    {
                        //if ((gvProdInsto.GetRowValues(gvProdInsto.FocusedRowIndex, "SPPRO$PROPIETARIO").ToString() == Session["USUARIO"].ToString()) || Session["ROL"].ToString() == "ADMIN")
                        if (Session["ROL"].ToString() != "ENTIDAD")
                        {
                            panResultadoInstitucionales.Style.Add("display", "block");
                            panRed.Style.Add("display", "none");


                            responsable = busca_responsable(Convert.ToInt32(Session["insto"]));

                            if (responsable == 0)
                            {
                                if (Convert.ToInt32(gvProdInsto.GetRowValues(gvProdInsto.FocusedRowIndex, "SPRES$TIPO").ToString()) == 1)
                                {
                                    cargaResultadoInsto(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), Convert.ToInt32(gvProdInsto.GetRowValues(gvProdInsto.FocusedRowIndex, "SPRES$ID_RESULTADO").ToString()));
                                    if (Convert.ToInt32(gvProdInsto.GetRowValues(gvProdInsto.FocusedRowIndex, "SPPRO$RESULTADO2").ToString()) != -1)
                                    {
                                        cbPGG.SelectedIndex = 1;
                                        sql = "SELECT * FROM SCHE$SIPLAN20.SP20$RESULTADOS WHERE SPRES$ID_RESULTADO = " + Convert.ToInt32(gvProdInsto.GetRowValues(gvProdInsto.FocusedRowIndex, "SPPRO$RESULTADO2").ToString()) + " AND SPRES$RESTRICTIVA = 'N'";
                                        estado = dao.consulta(sql);
                                        if (estado == 1)
                                        {
                                            cod_estrategico = dao.tabla;
                                            if (cod_estrategico.Rows.Count > 0)
                                            {
                                                cargaPilares(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), Convert.ToInt32(cod_estrategico.Rows[0]["SPRES$COD_ESTRATEGICO"]), responsable);
                                                cbAcciones.DataSource = null;
                                                cbAcciones.Enabled = false;
                                                panPGGProductos.Style.Add("display", "block");
                                            }


                                        }


                                    }
                                    else
                                    {
                                        if (esAdmin == 0)
                                        {
                                            cbPGG.SelectedIndex = 1;
                                            cbPGG.Enabled = false;
                                            cargaPilares(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), -1, responsable);
                                            cbAcciones.DataSource = null;
                                            cbAcciones.Enabled = false;
                                            cbAcciones.Items.Clear();
                                            panPGGProductos.Style.Add("display", "block");
                                        }
                                        else if (esAdmin == 1)
                                        {
                                            cbPGG.SelectedIndex = -1;
                                            cbPGG.Enabled = true;
                                            cargaPilares(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), -1, responsable);
                                            cbAcciones.DataSource = null;
                                            cbAcciones.Enabled = false;
                                            cbAcciones.Items.Clear();
                                            panPGGProductos.Style.Add("display", "none");
                                        }
                                    }
                                       
                                }

                                else
                                {

                                    cbPGG.SelectedIndex = 1;
                                    panPGGProductos.Style.Add("display", "block");
                                    cargaPilares(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), Convert.ToInt32(gvProdInsto.GetRowValues(gvProdInsto.FocusedRowIndex, "SPRES$COD_ESTRATEGICO").ToString()), responsable);
                                    cbAcciones.DataSource = null;
                                    cbAcciones.Enabled = false;
                                    cbAcciones.Items.Clear();

                                    if (Convert.ToInt32(gvProdInsto.GetRowValues(gvProdInsto.FocusedRowIndex, "SPPRO$RESULTADO2").ToString()) != -1)
                                        cargaResultadoInsto(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), Convert.ToInt32(gvProdInsto.GetRowValues(gvProdInsto.FocusedRowIndex, "SPPRO$RESULTADO2").ToString()));
                                    else
                                        cargaResultadoInsto(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), -1);



                                }



                            }
                            else
                            {
                                if (Convert.ToInt32(gvProdInsto.GetRowValues(gvProdInsto.FocusedRowIndex, "SPRES$TIPO").ToString()) == 1)
                                {
                                    cargaResultadoInsto(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), Convert.ToInt32(gvProdInsto.GetRowValues(gvProdInsto.FocusedRowIndex, "SPRES$ID_RESULTADO").ToString()));
                                    if (Convert.ToInt32(gvProdInsto.GetRowValues(gvProdInsto.FocusedRowIndex, "SPPRO$RESULTADO2").ToString()) != -1)
                                    {
                                        cbPGG.SelectedIndex = 1;
                                        sql = "SELECT * FROM SCHE$SIPLAN20.SP20$RESULTADOS WHERE SPRES$ID_RESULTADO = " + Convert.ToInt32(gvProdInsto.GetRowValues(gvProdInsto.FocusedRowIndex, "SPPRO$RESULTADO2").ToString()) + " AND SPRES$RESTRICTIVA = 'N'";
                                        estado = dao.consulta(sql);
                                        if (estado == 1)
                                        {
                                            cod_estrategico = dao.tabla;
                                            if (cod_estrategico.Rows.Count > 0)
                                            {
                                                sql = "SELECT ID_EJE FROM SCHE$SIPLAN20.SPPSVST$PGG2024_2028 WHERE ID_ACCION = " + cod_estrategico.Rows[0]["SPRES$COD_ESTRATEGICO"] + " GROUP BY ID_EJE";
                                                estado = dao.consulta(sql);
                                                if (estado == 1)
                                                {
                                                    tabla_eje = dao.tabla;
                                                    if (tabla_eje.Rows.Count > 0)
                                                    {
                                                        cargaPilares(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), Convert.ToInt32(tabla_eje.Rows[0]["ID_EJE"]), responsable);
                                                        cargaAccionesEstrategicas(Convert.ToInt32(cod_estrategico.Rows[0]["SPRES$COD_ESTRATEGICO"]));
                                                        panPGGProductos.Style.Add("display", "block");

                                                    }

                                                }
                                            }


                                        }


                                    }
                                    else
                                    {
                                      
                                        cbPGG.SelectedIndex = 0;
                                        cargaPilares(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), -1, responsable);
                                        panResultadoInstitucionales.Style.Add("display", "block");
                                        panPGGProductos.Style.Add("display", "none");
                                    }
                                }
                                else
                                {

                                    if (Convert.ToInt32(gvProdInsto.GetRowValues(gvProdInsto.FocusedRowIndex, "SPPRO$RESULTADO2").ToString()) != -1)
                                        cargaResultadoInsto(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), Convert.ToInt32(gvProdInsto.GetRowValues(gvProdInsto.FocusedRowIndex, "SPPRO$RESULTADO2").ToString()));
                                    else
                                        cargaResultadoInsto(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), -1);


                                    cbPGG.SelectedIndex = 1;
                                    panPGGProductos.Style.Add("display", "block");
                                    sql = "SELECT ID_EJE FROM SCHE$SIPLAN20.SPPSVST$PGG2024_2028 WHERE ID_ACCION = " + Convert.ToInt32(gvProdInsto.GetRowValues(gvProdInsto.FocusedRowIndex, "SPRES$COD_ESTRATEGICO").ToString()) + " GROUP BY ID_EJE";
                                    estado = dao.consulta(sql);
                                    if (estado == 1)
                                    {
                                        tabla_eje = dao.tabla;
                                        if (tabla_eje.Rows.Count > 0)
                                        {
                                            cargaPilares(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), Convert.ToInt32(tabla_eje.Rows[0]["ID_EJE"]), responsable);
                                            cargaAccionesEstrategicas(Convert.ToInt32(gvProdInsto.GetRowValues(gvProdInsto.FocusedRowIndex, "SPRES$COD_ESTRATEGICO").ToString()));

                                        }
                                        else if (tabla_eje.Rows.Count <= 0)
                                        {
                                            sql = @"SELECT 
                                                    ID_EJE
                                                    ,EJE
                                                    FROM
                                                    (SELECT ID_EJE, EJE FROM SCHE$SIPLAN20.SPPSVST$PGG2024_2028 PG INNER JOIN 
                                                        SCHE$SIPLAN20.SP20$RESULTADOS R ON PG.ID_ACCION = R.SPRES$COD_ESTRATEGICO AND R.SPRES$RESTRICTIVA = 'N' AND R.SPRES$POM = " + Convert.ToInt32(Session["pom"]) + " AND R.SPRES$INSTITUCION = " + Convert.ToInt32(Session["insto"]) + @"

                                                        UNION

                                                        SELECT RE.SPPRES$ID_RESULTADO ID_EJE
                                                        ,RE.SPPRES$CODIGO||'-'||RE.SPPRES$DESCRIPCION EJE
                                                        FROM SCHE$SIPLAN20.SP20$RESULTADOS_ESTRATEGICOS RE 
                                                        INNER JOIN SCHE$SIPLAN20.SP20$RESULTADOS R ON RE.SPPRES$ID_RESULTADO = R.SPRES$COD_ESTRATEGICO AND RE.SPPRES$NIVEL = 1 AND R.SPRES$RESTRICTIVA = 'N' AND RE.SPPRES$RESTRICTIVA = 'N'
                                                        INNER JOIN SCHE$SIPLAN20.S20$PERIODO_RESULTADO PR ON PR.SPR$ID_RESULTADO = RE.SPPRES$ID_RESULTADO AND PR.SPR$RESTRICTIVA = 'N' AND RE.SPPRES$RESTRICTIVA = 'N' AND RE.SPPRES$NIVEL = 1
                                                        INNER JOIN SCHE$SIPLAN20.SP20$PERIODO_GOBIERNO PG ON PG.SPG$ID_PERIODO = PR.SPR$ID_PERIODO AND PG.SPG$RESTRICTIVA = 'N' AND PR.SPR$RESTRICTIVA = 'N' AND PG.SPG$VIGENTE = 1
                                                        WHERE RE.SPPRES$NIVEL = 1 AND RE.SPPRES$RESTRICTIVA = 'N' AND R.SPRES$POM = " + Convert.ToInt32(Session["pom"]) + " AND R.SPRES$INSTITUCION = " + Convert.ToInt32(Session["insto"]) + @"

                                                        UNION 
                    
                                                        SELECT 
                                                        ID_EJE, EJE
                                                        FROM
                                                        (SELECT ID_ACCION
                                                                ,EJE
                                                                ,META
                                                                ,ACCION_ESTRATEGICA
                                                                ,ID_EJE FROM SCHE$SIPLAN20.SPPSVST$PGG2024_2028 WHERE RESPONSABLE = " + Convert.ToInt32(Session["insto"]) + @"
                                                                UNION
                                                                SELECT ID_ACCION
                                                                ,EJE
                                                                ,META
                                                                ,ACCION_ESTRATEGICA
                                                                ,ID_EJE FROM SCHE$SIPLAN20.SPPSVST$PGG2024_2028 INNER JOIN SCHE$SIPLAN20.SP20$RESPONSABLESPGG RG ON RG.SPRPG$ID_PGG = ID_ACCION AND SPRPG$NIVEL = 3 AND SPRPG$RESPONSABLE = " + Convert.ToInt32(Session["insto"]) + @"
                                                        )

                                                    UNION

                                                    SELECT
                                                    RG.SPRPG$ID_PGG ID_EJE
                                                    ,R.SPPRES$CODIGO||'-'||R.SPPRES$DESCRIPCION EJE
                                                    FROM SCHE$SIPLAN20.SP20$RESULTADOS_ESTRATEGICOS R 
                                                    INNER JOIN SCHE$SIPLAN20.SP20$RESPONSABLESPGG RG ON RG.SPRPG$ID_PGG = R.SPPRES$ID_RESULTADO AND RG.SPRPG$NIVEL  = 1 AND R.SPPRES$RESTRICTIVA = 'N' AND RG.SPRPG$RESTRICTIVA = 'N'
                                                    WHERE RG.SPRPG$RESPONSABLE = " + Convert.ToInt32(Session["insto"]) + @"

                                                    )
                                                GROUP BY
                                                ID_EJE
                                                 ,EJE
                                                ORDER BY ID_EJE ASC";

                                            estado = dao.consulta(sql);
                                            if (estado == 1)
                                            {
                                                tabla_eje = dao.tabla;
                                                if (tabla_eje.Rows.Count > 0)
                                                {

                                                    cargaPilares(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), Convert.ToInt32(gvProdInsto.GetRowValues(gvProdInsto.FocusedRowIndex, "SPRES$COD_ESTRATEGICO").ToString()), responsable);
                                                    cargaAccionesEstrategicas(Convert.ToInt32(cbPilar.Value));

                                                }
                                            }

                                        }
                                        else
                                        {
                                            cbPGG.SelectedIndex = 0;
                                            panPGGProductos.Style.Add("display", "none");
                                            cargaPilares(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), -1, responsable);
                                            cbAcciones.DataSource = null;
                                            cbAcciones.Items.Clear();
                                        }


                                    }
                                }
                            }

                            Session["tipo"] = "institucional";
                            Session["operacion"] = 1;
                            lblProducto.Text = "Producto institucional" + producto;
                            lblTipo.Text = "Producto institucional";
                            lblTipoDescripcion.Text = "producto institucional";
                            //lbTipoResultado.Text = "Resultado Institucional: ";
                            lblProducto.Style.Add("color", "#2d572c");
                            lblPilar.Visible = false;
                            lblResultado.Visible = false;
                            cbResultados.Enabled = true;

                            codproducto = Convert.ToInt32(gvProdInsto.GetRowValues(gvProdInsto.FocusedRowIndex, "SPPRO$ID_PRODUCTO").ToString());
                            sql = "SELECT * FROM SCHE$SIPLAN20.SP20$PRODUCTO WHERE SPPRO$ID_PRODUCTO = " + codproducto + " AND SPPRO$RESTRICTIVA = 'N'";
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
                                    //cbResultadoInsitucional.Value = tabla.Rows[0]["SPPRO$ID_RESULTADO"].ToString();
                                    sql = "SELECT * FROM SCHE$SIPLAN20.SP20$PROGRAMA_PRESUPUESTARIO P WHERE P.SPPRO$ID_PROGRAMA_PRESUPUESTO = " + Convert.ToDouble(tabla.Rows[0]["SPPRO$PRESUPUESTO"]) + " AND P.SPPRO$ID_POM = " + Convert.ToInt32(Session["pom"]) + " AND P.SPPRO$ID_INSTITUCION = " + Convert.ToInt32(Session["insto"]) + " AND P.SPPRO$RESTRICTIVA = 'N'";
                                    estado = dao.consulta(sql);
                                    if (estado == 0)
                                    {
                                        mensaje = dao.mensaje;
                                        Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
                                    }
                                    else
                                    {
                                        tempo1 = dao.tabla;
                                        if (tempo1.Rows[0]["SPPRO$ID_PROGRAMA_DEPENDE"] == DBNull.Value)
                                        {
                                            cbSuprograma.SelectedIndex = 0;
                                            cbProgramaPresupuestario.Value = tabla.Rows[0]["SPPRO$PRESUPUESTO"].ToString();
                                            Cargasubprogramas(Convert.ToDouble(tabla.Rows[0]["SPPRO$PRESUPUESTO"]), Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), -1);
                                            subProgramaPresupuestario.Style.Add("display", "none");
                                        }

                                        else
                                        {
                                            cbSuprograma.SelectedIndex = 1;
                                            cbProgramaPresupuestario.Value = tempo1.Rows[0]["SPPRO$ID_PROGRAMA_DEPENDE"].ToString();
                                            Cargasubprogramas(Convert.ToDouble(tempo1.Rows[0]["SPPRO$ID_PROGRAMA_DEPENDE"]), Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), Convert.ToInt32(tabla.Rows[0]["SPPRO$PRESUPUESTO"]));
                                            subProgramaPresupuestario.Style.Add("display", "block");

                                        }

                                        responsable = busca_responsable(Convert.ToInt32(Session["insto"]));
                                        if (responsable == 0)
                                        {
                                            if (gvProdInsto.GetRowValues(gvProdInsto.FocusedRowIndex, "SPPRES$NIVEL") != DBNull.Value)
                                            {
                                                if (Convert.ToInt32(gvProdInsto.GetRowValues(gvProdInsto.FocusedRowIndex, "SPPRES$NIVEL")) == 1)
                                                {
                                                    eje = Convert.ToInt32(gvProdInsto.GetRowValues(gvProdInsto.FocusedRowIndex, "ID_EJE"));
                                                    cargaPilares(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), eje, responsable);
                                                    cbAcciones.Enabled = false;
                                                    cbAcciones.Items.Clear();

                                                }



                                            }

                                        }

                                    }

                                    


                                    txtProductoInsto.Text = tabla.Rows[0]["SPPRO$DESCRIPCION"].ToString();
                                    cbUnidadMedida.Value = tabla.Rows[0]["SPPRO$ID_MEDIDA"].ToString();
                                    //cbResultadoInsitucional.Enabled = true;
                                    cbProgramaPresupuestario.Enabled = true;
                                    cbSuprograma.Enabled = true;
                                    subProgramaPresupuestario.Enabled = true;
                                    cbSuProgramaPresupuestario.Enabled = true;
                                    txtProductoInsto.Enabled = true;
                                    cbUnidadMedida.Enabled = true;
                                    rbaccion.Enabled = false;
                                    rbaccion.SelectedIndex = 0;
                                    cpPilarPGG.Enabled = true;
                                    cbObjSectorial.Enabled = true;
                                    CbAccionEstrategica.Enabled = true;
                                    btnGrabaProdInsto.CssClass = "btn btn-success";
                                    btnGrabaProdInsto.Text = "Editar producto";
                                    MultiView1.ActiveViewIndex = 5;

                                }
                            }
                        }
                        else
                        {
                            mensaje = "Su perfil no esta autorizado para modificar este registro";
                            Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);

                        }




                    }
                    else
                    {
                        mensaje = "Debe seleccionar una fila la cual contenga un producto";
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);

                    }
                }


            }

            else if (rbProductos.SelectedIndex == 1) //EDICION ESTRATEGICO
            {
                if (gvProdEstrategicos.FocusedRowIndex != -1)
                {//INICIO SELECCION GRID
                    nivel = gvProdEstrategicos.GetRowLevel(gvProdEstrategicos.FocusedRowIndex);
                    producto = gvProdEstrategicos.GetRowValues(gvProdEstrategicos.FocusedRowIndex, "PRODUCTO").ToString();
                    red = gvProdEstrategicos.GetRowValues(gvProdEstrategicos.FocusedRowIndex, "RED").ToString();

                    codproducto = Convert.ToInt32(gvProdEstrategicos.GetRowValues(gvProdEstrategicos.FocusedRowIndex, "SPPRO$ID_PRODUCTO").ToString());
                    codresultado = Convert.ToInt32(gvProdEstrategicos.GetRowValues(gvProdEstrategicos.FocusedRowIndex, "ID_RESULTADO").ToString());
                    codRed = Convert.ToInt32(gvProdEstrategicos.GetRowValues(gvProdEstrategicos.FocusedRowIndex, "SPPRED$ID").ToString());
                    rbaccion.Enabled = false;
                    cbPGG.SelectedIndex = 1;
                    panResultadoInstitucionales.Style.Add("display", "none");
                    panPGGProductos.Style.Add("display", "block");

                    panRed.Style.Add("display", "block");
                    responsable = busca_responsable_red(Convert.ToInt32(Session["insto"]));
                    busca_red(Convert.ToInt32(Session["insto"]), responsable, codRed);



                    if (nivel == 0)
                    {//INICIO NIVEL
                        if (Session["ROL"].ToString() != "ENTIDAD")
                        //if ((gvProdEstrategicos.GetRowValues(gvProdEstrategicos.FocusedRowIndex, "SPPRO$PROPIETARIO").ToString() == Session["USUARIO"].ToString()) || Session["ROL"].ToString() == "ADMIN")
                        {//INICIO PROPIETARIO
                            Session["tipo"] = "estrategico";
                            Session["operacion"] = 11;
                            lblProducto.Text = "Producto vinculado a Resultado Estratégico (RE): " + producto;
                            lblTipo.Text = "Producto vinculado a Resultado Estratégico (RE)";
                            lblTipoDescripcion.Text = "Producto vinculado a Resultado Estratégico (RE)";
                            //lbTipoResultado.Text = "Eje estratégico/Acción estratégica vinculada: ";
                            lblProducto.Style.Add("color", "#2d572c");
                            lblPilar.Text = "RE: " + red;
                            lblPilar.Visible = true;
                            lblResultado.Visible = true;



                            responsable = busca_responsable(Convert.ToInt32(Session["insto"]));

                            if (Convert.ToInt32(gvProdEstrategicos.GetRowValues(gvProdEstrategicos.FocusedRowIndex, "SPPRO$RESULTADO2").ToString()) != -1)
                            {
                                cbPGG.SelectedIndex = 1;
                                panPGGProductos.Style.Add("display", "block");

                                if (responsable == 0)
                                {
                                    sql = "SELECT * FROM SCHE$SIPLAN20.SP20$RESULTADOS WHERE SPRES$ID_RESULTADO = " + Convert.ToInt32(gvProdEstrategicos.GetRowValues(gvProdEstrategicos.FocusedRowIndex, "SPPRO$RESULTADO2").ToString()) + " AND SPRES$RESTRICTIVA = 'N'";
                                    estado = dao.consulta(sql);
                                    if (estado == 1)
                                    {
                                        cod_estrategico = dao.tabla;
                                        if (cod_estrategico.Rows.Count > 0)
                                        {
                                            cargaPilares(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), Convert.ToInt32(cod_estrategico.Rows[0]["SPRES$COD_ESTRATEGICO"]), responsable);
                                            panPGGProductos.Style.Add("display", "block");
                                            cbPilar.Enabled = true;
                                            cbAcciones.DataSource = null;
                                            cbAcciones.Enabled = false;
                                        }


                                    }
                                }
                                else
                                {
                                    sql = "SELECT * FROM SCHE$SIPLAN20.SP20$RESULTADOS WHERE SPRES$ID_RESULTADO = " + Convert.ToInt32(gvProdEstrategicos.GetRowValues(gvProdEstrategicos.FocusedRowIndex, "SPPRO$RESULTADO2").ToString()) + " AND SPRES$RESTRICTIVA = 'N'";
                                    estado = dao.consulta(sql);
                                    if (estado == 1)
                                    {
                                        cod_estrategico = dao.tabla;
                                        if (cod_estrategico.Rows.Count > 0)
                                        {
                                            sql = "SELECT ID_EJE FROM SCHE$SIPLAN20.SPPSVST$PGG2024_2028 WHERE ID_ACCION = " + cod_estrategico.Rows[0]["SPRES$COD_ESTRATEGICO"] + " GROUP BY ID_EJE";
                                            estado = dao.consulta(sql);
                                            if (estado == 1)
                                            {
                                                tabla_eje = dao.tabla;
                                                if (tabla_eje.Rows.Count > 0)
                                                {
                                                    cargaPilares(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), Convert.ToInt32(tabla_eje.Rows[0]["ID_EJE"]), responsable);
                                                    cargaAccionesEstrategicas(Convert.ToInt32(cod_estrategico.Rows[0]["SPRES$COD_ESTRATEGICO"]));
                                                    cbPilar.Enabled = true;
                                                    panPGGProductos.Style.Add("display", "block");

                                                }
                                                else

                                                {
                                                    sql = @"SELECT
                                                    RG.SPRPG$ID_PGG ID_EJE
                                                    , R.SPPRES$DESCRIPCION EJE
                                                    FROM SCHE$SIPLAN20.SP20$RESULTADOS_ESTRATEGICOS R
                                                    INNER JOIN SCHE$SIPLAN20.SP20$RESPONSABLESPGG RG ON RG.SPRPG$ID_PGG = R.SPPRES$ID_RESULTADO AND RG.SPRPG$NIVEL = 1 AND R.SPPRES$RESTRICTIVA = 'N' AND RG.SPRPG$RESTRICTIVA = 'N'
                                                    WHERE RG.SPRPG$RESPONSABLE = " + Convert.ToInt32(Session["insto"]);
                                                    estado = dao.consulta(sql);
                                                    if (estado == 1)
                                                    {
                                                        tabla_eje = dao.tabla;

                                                        tabla_eje = dao.tabla;
                                                        if (tabla_eje.Rows.Count > 0)
                                                        {
                                                            cargaPilares(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), Convert.ToInt32(cod_estrategico.Rows[0]["SPRES$COD_ESTRATEGICO"]), responsable);
                                                            cargaAccionesEstrategicas(-1);
                                                            cbPilar.Enabled = true;
                                                            panPGGProductos.Style.Add("display", "block");

                                                        }

                                                    }
                                                }

                                            }
                                        }




                                    }

                                }

                            }

                            else
                            {
                                cbPGG.SelectedIndex = 1;
                                cbPGG.Enabled = false;
                                panPGGProductos.Style.Add("display", "block");
                                cargaPilares(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), -1, responsable);
                            }




                            //cpPilarPGG.Value = codPilar.ToString();
                            //cargaResultadoEstrategico(); ATENCION!!
                            //cbResultadoInsitucional.Value = codResultado.ToString();



                            sql = "SELECT * FROM SCHE$SIPLAN20.SP20$PRODUCTO WHERE SPPRO$ID_PRODUCTO = " + codproducto + " AND SPPRO$RESTRICTIVA = 'N'";
                            estado = dao.consulta(sql);
                            if (estado == 0)
                            {
                                mensaje = dao.mensaje;
                                Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
                            }

                            tabla = dao.tabla;
                            if (tabla.Rows.Count > 0)
                            {//INICIO TABLA LLENA
                                //cbResultadoInsitucional.Value = tabla.Rows[0]["SPPRO$ID_RESULTADO"].ToString();
                                sql = "SELECT * FROM SCHE$SIPLAN20.SP20$PROGRAMA_PRESUPUESTARIO P WHERE P.SPPRO$ID_PROGRAMA_PRESUPUESTO = " + Convert.ToDouble(tabla.Rows[0]["SPPRO$PRESUPUESTO"]) + " AND P.SPPRO$ID_POM = " + Convert.ToInt32(Session["pom"]) + " AND P.SPPRO$ID_INSTITUCION = " + Convert.ToInt32(Session["insto"]) + " AND P.SPPRO$RESTRICTIVA = 'N'";
                                estado = dao.consulta(sql);
                                if (estado == 0)
                                {
                                    mensaje = dao.mensaje;
                                    Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
                                }
                                else
                                {
                                    tempo1 = dao.tabla;
                                    if (tempo1.Rows[0]["SPPRO$ID_PROGRAMA_DEPENDE"] == DBNull.Value)
                                    {
                                        cbSuprograma.SelectedIndex = 0;
                                        cbProgramaPresupuestario.Value = tabla.Rows[0]["SPPRO$PRESUPUESTO"].ToString();
                                        Cargasubprogramas(Convert.ToDouble(tabla.Rows[0]["SPPRO$PRESUPUESTO"]), Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), -1);
                                        subProgramaPresupuestario.Style.Add("display", "none");
                                    }

                                    else
                                    {
                                        cbSuprograma.SelectedIndex = 1;
                                        cbProgramaPresupuestario.Value = tempo1.Rows[0]["SPPRO$ID_PROGRAMA_DEPENDE"].ToString();
                                        Cargasubprogramas(Convert.ToDouble(tempo1.Rows[0]["SPPRO$ID_PROGRAMA_DEPENDE"]), Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), Convert.ToDouble(tabla.Rows[0]["SPPRO$PRESUPUESTO"]));
                                        subProgramaPresupuestario.Style.Add("display", "block");

                                    }

                                    if (tabla.Rows[0]["SPPRO$OBJETIVO_CENTRAL"] == DBNull.Value)
                                    {
                                        rbaccion.SelectedIndex = 0;
                                        AccionEstrategica.Style.Add("display", "none");
                                    }
                                    else
                                    {
                                        rbaccion.SelectedIndex = 1;
                                        //AccionEstrategica.Style.Add("display", "block");
                                        sql = "SELECT * FROM SCHE$SIPLAN20.SP20$RESULTADOS_ESTRATEGICOS R WHERE R.SPPRES$ID_RESULTADO = " + Convert.ToInt32(tabla.Rows[0]["SPPRO$OBJETIVO_CENTRAL"]) + " AND R.SPPRES$NIVEL = 3 AND R.SPPRES$RESTRICTIVA = 'N'";
                                        estado = dao.consulta(sql);
                                        if (estado == 0)
                                        {
                                            mensaje = dao.mensaje;
                                            Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
                                        }
                                        else
                                        {
                                            tempo1 = dao.tabla;
                                            cpPilarPGG.Enabled = false;
                                            cpPilarPGG.Value = tempo1.Rows[0]["SPPRES$DEPENDE"].ToString();
                                            //cargaobjetivos(Convert.ToInt32(cpPilarPGG.Value), Convert.ToInt32(tabla.Rows[0]["SPPRO$OBJETIVO_CENTRAL"]));
                                            //cargaAcciones(Convert.ToInt32(cbObjSectorial.Value), Convert.ToInt32(tabla.Rows[0]["SPPRO$ACCION_ESTRATEGICA"]));

                                        }

                                    }
                                }


                              


                                txtProductoInsto.Text = tabla.Rows[0]["SPPRO$DESCRIPCION"].ToString();
                                cbUnidadMedida.Value = tabla.Rows[0]["SPPRO$ID_MEDIDA"].ToString();
                                //cbResultadoInsitucional.Enabled = true;
                                cbProgramaPresupuestario.Enabled = true;
                                cbSuprograma.Enabled = true;
                                subProgramaPresupuestario.Enabled = true;
                                cbSuProgramaPresupuestario.Enabled = true;
                                txtProductoInsto.Enabled = true;
                                cbUnidadMedida.Enabled = true;
                                rbaccion.Enabled = true;
                                cpPilarPGG.Enabled = false;
                                cbObjSectorial.Enabled = true;
                                CbAccionEstrategica.Enabled = true;
                                btnGrabaProdInsto.CssClass = "btn btn-success";
                                btnGrabaProdInsto.Text = "Editar producto";
                                MultiView1.ActiveViewIndex = 5;

                            }//FIN TABLA LLENA

                        }

                        else
                        {
                            mensaje = "Su perfil de usuario no esta autorizado para modificar este registro";
                            Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
                            ProdEstrategicos.Style.Add("display", "block");
                            ProdInsitucionales.Style.Add("display", "none");
                        }//FIN PROPIETARIO

                    }

                    else
                    {
                        mensaje = "Debe seleccionar una fila la cual contenga un producto";
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
                        ProdEstrategicos.Style.Add("display", "block");
                        ProdInsitucionales.Style.Add("display", "none");
                    }//FIN NIVELG
                }//FIN SELECCION GRID

            }


        }

        protected void Cargasubprogramas(Double programa, int pom, int insto, Double sub)
        {
            DataTable subs = new DataTable();
            //sql = "SELECT * FROM SCHE$SIPLAN20.SP20$PROGRAMA_PRESUPUESTARIO P WHERE P.SPPRO$ID_PROGRAMA_DEPENDE = " + programa + " AND P.SPPRO$ID_POM = "+pom + " AND P.SPPRO$ID_INSTITUCION = "+insto+ " AND P.SPPRO$RESTRICTIVA = 'N'";
            sql = "SELECT P.SPPRO$ID_PROGRAMA_PRESUPUESTO, CASE WHEN ROUND(P.SPPRO$ID_PROGRAMA_PRESUPUESTO,0) = 0 THEN '0'||P.SPPRO$ID_PROGRAMA_PRESUPUESTO|| '-' || P.SPPRO$DESCRIPCION ELSE P.SPPRO$ID_PROGRAMA_PRESUPUESTO|| '-' || P.SPPRO$DESCRIPCION END AS PROGRAMA FROM SCHE$SIPLAN20.SP20$PROGRAMA_PRESUPUESTARIO P WHERE P.SPPRO$ID_PROGRAMA_DEPENDE = " + programa + " AND P.SPPRO$ID_POM = " + pom + " AND SPPRO$ID_INSTITUCION = " + insto + " AND P.SPPRO$RESTRICTIVA = 'N'";
            estado = dao.consulta(sql);
            if (estado == 0)
            {
                mensaje = dao.mensaje;
                Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
            }
            else
            {
                subs = dao.tabla;
                cbSuProgramaPresupuestario.DataSource = subs;
                cbSuProgramaPresupuestario.ValueField = "SPPRO$ID_PROGRAMA_PRESUPUESTO";
                cbSuProgramaPresupuestario.TextField = "PROGRAMA";
                cbSuProgramaPresupuestario.DataBind();
                if (sub != -1)
                    cbSuProgramaPresupuestario.Value = sub.ToString();
                else
                    cbSuProgramaPresupuestario.SelectedIndex = -1;
            }

        }
        /*
        protected void cargaobjetivos(int resultado, int objetivo)
        {
            DataTable resultados = new DataTable();
            sql = "SELECT R.SPPRES$ID_RESULTADO, R.SPPRES$CODIGO||'-'||R.SPPRES$DESCRIPCION OBJETIVO FROM SCHE$SIPLAN20.SP20$RESULTADOS_ESTRATEGICOS R WHERE R.SPPRES$NIVEL = 3 AND R.SPPRES$DEPENDE = " + resultado + " AND R.SPPRES$RESTRICTIVA = 'N' ";
            estado = dao.consulta(sql);
            if (estado == 0)
            {
                mensaje = dao.mensaje;
                Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
            }
            else
            {
                resultados = dao.tabla;
                cbObjSectorial.DataSource = resultados;
                cbObjSectorial.ValueField = "SPPRES$ID_RESULTADO";
                cbObjSectorial.TextField = "OBJETIVO";
                cbObjSectorial.DataBind();
                if (objetivo != -1)
                    cbObjSectorial.Value = objetivo.ToString();
                else
                    cbObjSectorial.SelectedIndex = -1;
            }
        }
        */
        protected void cargaAcciones(int objetivo, int accion)
        {
            DataTable resultados = new DataTable();
            sql = "SELECT R.SPPRES$ID_RESULTADO, R.SPPRES$CODIGO||'-'||R.SPPRES$DESCRIPCION ACCION FROM SCHE$SIPLAN20.SP20$RESULTADOS_ESTRATEGICOS R WHERE R.SPPRES$NIVEL = 4 AND R.SPPRES$DEPENDE = " + objetivo + " AND R.SPPRES$RESTRICTIVA = 'N' ";
            estado = dao.consulta(sql);
            if (estado == 0)
            {
                mensaje = dao.mensaje;
                Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
            }
            else
            {
                resultados = dao.tabla;
                CbAccionEstrategica.DataSource = resultados;
                CbAccionEstrategica.ValueField = "SPPRES$ID_RESULTADO";
                CbAccionEstrategica.TextField = "ACCION";
                CbAccionEstrategica.DataBind();
                if (accion != -1)
                    CbAccionEstrategica.Value = accion.ToString();
                else
                    CbAccionEstrategica.SelectedIndex = -1;
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            int nivel, codproducto, codResultado, codPilar, responsable;

            String producto, pilar, resultado;
            DataTable tempo1 = new DataTable();
            DataTable cod_estrategico = new DataTable();
            DataTable tabla_eje = new DataTable();
            if (rbProductos.SelectedIndex == 0)
            {
                if (gvProdInsto.FocusedRowIndex != -1)
                {
                    nivel = gvProdInsto.GetRowLevel(gvProdInsto.FocusedRowIndex);
                    producto = gvProdInsto.GetRowValues(gvProdInsto.FocusedRowIndex, "PRODUCTO").ToString();
                    if (nivel == 0)
                    {
                        //if ((gvProdInsto.GetRowValues(gvProdInsto.FocusedRowIndex, "SPPRO$PROPIETARIO").ToString() == Session["USUARIO"].ToString()) || Session["ROL"].ToString() == "ADMIN")
                        if (Session["ROL"].ToString() != "ENTIDAD")
                        {
                            Session["tipo"] = "institucional";
                            Session["operacion"] = 2;
                            lblPilar.Visible = false;
                            lblResultado.Visible = false;

                            responsable = busca_responsable(Convert.ToInt32(Session["insto"]));



                            if (responsable == 0)
                            {
                                if (Convert.ToInt32(gvProdInsto.GetRowValues(gvProdInsto.FocusedRowIndex, "SPRES$TIPO").ToString()) == 1)
                                {
                                    cargaResultadoInsto(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), Convert.ToInt32(gvProdInsto.GetRowValues(gvProdInsto.FocusedRowIndex, "SPRES$ID_RESULTADO").ToString()));
                                    if (Convert.ToInt32(gvProdInsto.GetRowValues(gvProdInsto.FocusedRowIndex, "SPPRO$RESULTADO2").ToString()) != -1)
                                    {
                                        cbPGG.SelectedIndex = 1;
                                        sql = "SELECT * FROM SCHE$SIPLAN20.SP20$RESULTADOS WHERE SPRES$ID_RESULTADO = " + Convert.ToInt32(gvProdInsto.GetRowValues(gvProdInsto.FocusedRowIndex, "SPPRO$RESULTADO2").ToString()) + " AND SPRES$RESTRICTIVA = 'N'";
                                        estado = dao.consulta(sql);
                                        if (estado == 1)
                                        {
                                            cod_estrategico = dao.tabla;
                                            if (cod_estrategico.Rows.Count > 0)
                                            {
                                                cargaPilares(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), Convert.ToInt32(cod_estrategico.Rows[0]["SPRES$COD_ESTRATEGICO"]), responsable);
                                                cbAcciones.DataSource = null;
                                                cbAcciones.Enabled = false;
                                                panPGGProductos.Style.Add("display", "block");
                                            }


                                        }


                                    }
                                    else
                                    {
                                        cbPGG.SelectedIndex = 0;
                                        cargaPilares(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), -1, responsable);
                                        cbAcciones.DataSource = null;
                                        cbAcciones.Enabled = false;
                                        cbAcciones.Items.Clear();
                                        panPGGProductos.Style.Add("display", "none");
                                    }
                                }

                                else
                                {
                                    cbPGG.SelectedIndex = 1;
                                    panPGGProductos.Style.Add("display", "block");
                                    cargaPilares(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), Convert.ToInt32(gvProdInsto.GetRowValues(gvProdInsto.FocusedRowIndex, "SPRES$COD_ESTRATEGICO").ToString()), responsable);
                                    cbAcciones.DataSource = null;
                                    cbAcciones.Enabled = false;
                                    cbAcciones.Items.Clear();

                                    if (Convert.ToInt32(gvProdInsto.GetRowValues(gvProdInsto.FocusedRowIndex, "SPPRO$RESULTADO2").ToString()) != -1)
                                        cargaResultadoInsto(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), Convert.ToInt32(gvProdInsto.GetRowValues(gvProdInsto.FocusedRowIndex, "SPPRO$RESULTADO2").ToString()));
                                    else
                                        cargaResultadoInsto(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), -1);



                                }



                            }
                            else
                            {
                                if (Convert.ToInt32(gvProdInsto.GetRowValues(gvProdInsto.FocusedRowIndex, "SPRES$TIPO").ToString()) == 1)
                                {
                                    panRed.Style.Add("display", "none");
                                    cargaResultadoInsto(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), Convert.ToInt32(gvProdInsto.GetRowValues(gvProdInsto.FocusedRowIndex, "SPRES$ID_RESULTADO").ToString()));
                                    if (Convert.ToInt32(gvProdInsto.GetRowValues(gvProdInsto.FocusedRowIndex, "SPPRO$RESULTADO2").ToString()) != -1)
                                    {
                                        cbPGG.SelectedIndex = 1;
                                        sql = "SELECT * FROM SCHE$SIPLAN20.SP20$RESULTADOS WHERE SPRES$ID_RESULTADO = " + Convert.ToInt32(gvProdInsto.GetRowValues(gvProdInsto.FocusedRowIndex, "SPPRO$RESULTADO2").ToString()) + " AND SPRES$RESTRICTIVA = 'N'";
                                        estado = dao.consulta(sql);
                                        if (estado == 1)
                                        {
                                            cod_estrategico = dao.tabla;
                                            if (cod_estrategico.Rows.Count > 0)
                                            {
                                                sql = "SELECT ID_EJE FROM SCHE$SIPLAN20.SPPSVST$PGG2024_2028 WHERE ID_ACCION = " + cod_estrategico.Rows[0]["SPRES$COD_ESTRATEGICO"] + " GROUP BY ID_EJE";
                                                if (estado == 1)
                                                {
                                                    tabla_eje = dao.tabla;
                                                    if (tabla_eje.Rows.Count > 0)
                                                    {
                                                        cargaPilares(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), Convert.ToInt32(tabla_eje.Rows[0]["EJE"]), responsable);
                                                        cargaAccionesEstrategicas(Convert.ToInt32(cod_estrategico.Rows[0]["SPRES$COD_ESTRATEGICO"]));
                                                        panPGGProductos.Style.Add("display", "block");

                                                    }

                                                }
                                            }


                                        }


                                    }
                                    else
                                    {
                                        cbPGG.SelectedIndex = 0;
                                        cargaPilares(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), -1, responsable);
                                        panPGGProductos.Style.Add("display", "none");
                                    }
                                }
                                else
                                {

                                    if (Convert.ToInt32(gvProdInsto.GetRowValues(gvProdInsto.FocusedRowIndex, "SPPRO$RESULTADO2").ToString()) != -1)
                                        cargaResultadoInsto(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), Convert.ToInt32(gvProdInsto.GetRowValues(gvProdInsto.FocusedRowIndex, "SPPRO$RESULTADO2").ToString()));
                                    else
                                        cargaResultadoInsto(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), -1);


                                    cbPGG.SelectedIndex = 1;
                                    panPGGProductos.Style.Add("display", "block");
                                    sql = "SELECT ID_EJE FROM SCHE$SIPLAN20.SPPSVST$PGG2024_2028 WHERE ID_ACCION = " + Convert.ToInt32(gvProdInsto.GetRowValues(gvProdInsto.FocusedRowIndex, "SPRES$COD_ESTRATEGICO").ToString()) + " GROUP BY ID_EJE";
                                    estado = dao.consulta(sql);
                                    if (estado == 1)
                                    {
                                        tabla_eje = dao.tabla;
                                        if (tabla_eje.Rows.Count > 0)
                                        {
                                            cargaPilares(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), Convert.ToInt32(tabla_eje.Rows[0]["ID_EJE"]), responsable);
                                            cargaAccionesEstrategicas(Convert.ToInt32(gvProdInsto.GetRowValues(gvProdInsto.FocusedRowIndex, "SPRES$COD_ESTRATEGICO").ToString()));

                                        }
                                        else
                                        {
                                            cbPGG.SelectedIndex = 0;
                                            panPGGProductos.Style.Add("display", "none");
                                            cargaPilares(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), -1, responsable);
                                            cbAcciones.DataSource = null;
                                            cbAcciones.Items.Clear();
                                        }


                                    }
                                }
                            }


                            lblProducto.Text = "Esta por eliminar el Producto institucional: '" + producto + "'. Por favor confirme la operación presionando el boton 'Elminar producto'";
                            lblProducto.Style.Add("color", "red");
                            lblTipo.Text = "Producto institucional";
                            lblTipoDescripcion.Text = "producto institucional";
                            //lbTipoResultado.Text = "Resultado Institucional :";
                            codproducto = Convert.ToInt32(gvProdInsto.GetRowValues(gvProdInsto.FocusedRowIndex, "SPPRO$ID_PRODUCTO").ToString());
                            sql = "SELECT * FROM SCHE$SIPLAN20.SP20$PRODUCTO WHERE SPPRO$ID_PRODUCTO = " + codproducto + " AND SPPRO$RESTRICTIVA = 'N'";
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
                                    //cbResultadoInsitucional.Value = tabla.Rows[0]["SPPRO$ID_RESULTADO"].ToString();
                                    sql = "SELECT * FROM SCHE$SIPLAN20.SP20$PROGRAMA_PRESUPUESTARIO P WHERE P.SPPRO$ID_PROGRAMA_PRESUPUESTO = " + Convert.ToDouble(tabla.Rows[0]["SPPRO$PRESUPUESTO"]) + " AND P.SPPRO$ID_POM = " + Convert.ToInt32(Session["pom"]) + " AND P.SPPRO$ID_INSTITUCION = " + Convert.ToInt32(Session["insto"]) + " AND P.SPPRO$RESTRICTIVA = 'N'";
                                    estado = dao.consulta(sql);
                                    if (estado == 0)
                                    {
                                        mensaje = dao.mensaje;
                                        Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
                                    }
                                    else
                                    {
                                        tempo1 = dao.tabla;
                                        if (tempo1.Rows[0]["SPPRO$ID_PROGRAMA_DEPENDE"] == DBNull.Value)
                                        {
                                            cbSuprograma.SelectedIndex = 0;
                                            cbProgramaPresupuestario.Value = tabla.Rows[0]["SPPRO$PRESUPUESTO"].ToString();
                                            Cargasubprogramas(Convert.ToDouble(tabla.Rows[0]["SPPRO$PRESUPUESTO"]), Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), -1);
                                            subProgramaPresupuestario.Style.Add("display", "none");
                                        }

                                        else
                                        {
                                            cbSuprograma.SelectedIndex = 1;
                                            cbProgramaPresupuestario.Value = tempo1.Rows[0]["SPPRO$ID_PROGRAMA_DEPENDE"].ToString();
                                            //Cargasubprogramas(Convert.ToDouble(tempo1.Rows[0]["SPPRO$ID_PROGRAMA_DEPENDE"]), Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), Convert.ToInt32(tabla.Rows[0]["SPPRO$PRESUPUESTO"]));
                                            Cargasubprogramas(Convert.ToDouble(tempo1.Rows[0]["SPPRO$ID_PROGRAMA_DEPENDE"]), Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), Convert.ToDouble(tabla.Rows[0]["SPPRO$PRESUPUESTO"]));
                                            subProgramaPresupuestario.Style.Add("display", "block");

                                        }

                                        if (tabla.Rows[0]["SPPRO$OBJETIVO_CENTRAL"] == DBNull.Value)
                                        {
                                            rbaccion.SelectedIndex = 0;
                                            AccionEstrategica.Style.Add("display", "none");
                                        }
                                        else
                                        {
                                            rbaccion.SelectedIndex = 1;
                                            AccionEstrategica.Style.Add("display", "block");
                                            sql = "SELECT * FROM SCHE$SIPLAN20.SP20$RESULTADOS_ESTRATEGICOS R WHERE R.SPPRES$ID_RESULTADO = " + Convert.ToInt32(tabla.Rows[0]["SPPRO$OBJETIVO_CENTRAL"]) + " AND R.SPPRES$NIVEL = 3 AND R.SPPRES$RESTRICTIVA = 'N'";
                                            estado = dao.consulta(sql);
                                            if (estado == 0)
                                            {
                                                mensaje = dao.mensaje;
                                                Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
                                            }
                                            else
                                            {
                                                tempo1 = dao.tabla;
                                                cpPilarPGG.Value = tempo1.Rows[0]["SPPRES$DEPENDE"].ToString();
                                                // cargaobjetivos(Convert.ToInt32(cpPilarPGG.Value), Convert.ToInt32(tabla.Rows[0]["SPPRO$OBJETIVO_CENTRAL"]));
                                                //cargaAcciones(Convert.ToInt32(cbObjSectorial.Value), Convert.ToInt32(tabla.Rows[0]["SPPRO$ACCION_ESTRATEGICA"]));

                                            }

                                        }
                                    }
                                    cbResultados.Enabled = false;
                                    txtProductoInsto.Text = tabla.Rows[0]["SPPRO$DESCRIPCION"].ToString();
                                    cbUnidadMedida.Value = tabla.Rows[0]["SPPRO$ID_MEDIDA"].ToString();
                                    //cbResultadoInsitucional.Enabled = false;
                                    cbProgramaPresupuestario.Enabled = false;
                                    cbSuprograma.Enabled = false;
                                    subProgramaPresupuestario.Enabled = false;
                                    cbSuProgramaPresupuestario.Enabled = false;
                                    txtProductoInsto.Enabled = false;
                                    cbUnidadMedida.Enabled = false;
                                    rbaccion.Enabled = false;
                                    cpPilarPGG.Enabled = false;
                                    cbObjSectorial.Enabled = false;
                                    CbAccionEstrategica.Enabled = false;
                                    btnGrabaProdInsto.CssClass = "btn btn-danger";
                                    btnGrabaProdInsto.Text = "Eliminar producto";
                                    MultiView1.ActiveViewIndex = 5;

                                }
                            }
                        }
                        else
                        {
                            mensaje = "Su perfil no esta autorizado para modificar este registro";
                            Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);

                        }




                    }
                    else
                    {
                        mensaje = "Debe seleccionar una fila la cual contenga un producto";
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);

                    }
                }
            }

            else if (rbProductos.SelectedIndex == 1)
            {
                if (gvProdEstrategicos.FocusedRowIndex != -1)
                {//INICIO SELECCION GRID

                    int red = -1;
                    int respo = -1;
                    nivel = gvProdEstrategicos.GetRowLevel(gvProdEstrategicos.FocusedRowIndex);
                    producto = gvProdEstrategicos.GetRowValues(gvProdEstrategicos.FocusedRowIndex, "PRODUCTO").ToString();

                    codproducto = Convert.ToInt32(gvProdEstrategicos.GetRowValues(gvProdEstrategicos.FocusedRowIndex, "SPPRO$ID_PRODUCTO").ToString());

                    red = Convert.ToInt32(gvProdEstrategicos.GetRowValues(gvProdEstrategicos.FocusedRowIndex, "SPPRED$ID").ToString());
                    respo = busca_responsable_red(Convert.ToInt32(Session["insto"]));
                    busca_red(Convert.ToInt32(Session["insto"]), respo, red);


                    panResultadoInstitucionales.Style.Add("display", "none");
                    panPGGProductos.Style.Add("display", "none");
                    panRed.Style.Add("display", "block");
                    cbRed.Enabled = false;

                    if (nivel == 0)
                    {
                        //if ((gvProdEstrategicos.GetRowValues(gvProdEstrategicos.FocusedRowIndex, "SPPRO$PROPIETARIO").ToString() == Session["USUARIO"].ToString()) || Session["ROL"].ToString() == "ADMIN")
                        if (Session["ROL"].ToString() != "ENTIDAD")
                        {
                            Session["tipo"] = "estrategico";
                            Session["operacion"] = 12;

                            lblPilar.Visible = true;

                            lblResultado.Visible = true;
                            lblProducto.Text = "Esta por eliminar el Producto vinculado a Resultado Estratégico (RE): '" + producto + "'. Por favor confirme la operación presionando el boton 'Elminar producto'";
                            lblProducto.Style.Add("color", "red");

                            //cpPilarPGG.Value = codPilar.ToString();
                            //cargaResultadoEstrategico();ATENCION!!
                            //cbResultadoInsitucional.Value = codResultado.ToString();


                            lblTipo.Text = "Producto vinculado a Resultado Estratégico (RE)";
                            lblTipoDescripcion.Text = "Producto vinculado a Resultado Estratégico (RE)";
                            //lbTipoResultado.Text = "Meta PGG :";
                            sql = "SELECT * FROM SCHE$SIPLAN20.SP20$PRODUCTO WHERE SPPRO$ID_PRODUCTO = " + codproducto + " AND SPPRO$RESTRICTIVA = 'N'";


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
                                    //cbResultadoInsitucional.Value = tabla.Rows[0]["SPPRO$ID_RESULTADO"].ToString();
                                    sql = "SELECT * FROM SCHE$SIPLAN20.SP20$PROGRAMA_PRESUPUESTARIO P WHERE P.SPPRO$ID_PROGRAMA_PRESUPUESTO = " + Convert.ToDouble(tabla.Rows[0]["SPPRO$PRESUPUESTO"]) + " AND P.SPPRO$ID_POM = " + Convert.ToInt32(Session["pom"]) + " AND P.SPPRO$ID_INSTITUCION = " + Convert.ToInt32(Session["insto"]) + " AND P.SPPRO$RESTRICTIVA = 'N'";
                                    estado = dao.consulta(sql);
                                    if (estado == 0)
                                    {
                                        mensaje = dao.mensaje;
                                        Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
                                    }
                                    else
                                    {
                                        tempo1 = dao.tabla;
                                        if (tempo1.Rows[0]["SPPRO$ID_PROGRAMA_DEPENDE"] == DBNull.Value)
                                        {
                                            cbSuprograma.SelectedIndex = 0;
                                            cbProgramaPresupuestario.Value = tabla.Rows[0]["SPPRO$PRESUPUESTO"].ToString();
                                            Cargasubprogramas(Convert.ToDouble(tabla.Rows[0]["SPPRO$PRESUPUESTO"]), Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), -1);
                                            subProgramaPresupuestario.Style.Add("display", "none");
                                        }

                                        else
                                        {
                                            cbSuprograma.SelectedIndex = 1;
                                            cbProgramaPresupuestario.Value = tempo1.Rows[0]["SPPRO$ID_PROGRAMA_DEPENDE"].ToString();
                                            Cargasubprogramas(Convert.ToDouble(tempo1.Rows[0]["SPPRO$ID_PROGRAMA_DEPENDE"]), Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), Convert.ToInt32(tabla.Rows[0]["SPPRO$PRESUPUESTO"]));
                                            subProgramaPresupuestario.Style.Add("display", "block");

                                        }

                                        if (tabla.Rows[0]["SPPRO$OBJETIVO_CENTRAL"] == DBNull.Value)
                                        {
                                            rbaccion.SelectedIndex = 0;
                                            AccionEstrategica.Style.Add("display", "none");
                                        }
                                        else
                                        {
                                            rbaccion.SelectedIndex = 1;
                                            AccionEstrategica.Style.Add("display", "block");
                                            sql = "SELECT * FROM SCHE$SIPLAN20.SP20$RESULTADOS_ESTRATEGICOS R WHERE R.SPPRES$ID_RESULTADO = " + Convert.ToInt32(tabla.Rows[0]["SPPRO$OBJETIVO_CENTRAL"]) + " AND R.SPPRES$NIVEL = 3 AND R.SPPRES$RESTRICTIVA = 'N'";
                                            estado = dao.consulta(sql);
                                            if (estado == 0)
                                            {
                                                mensaje = dao.mensaje;
                                                Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
                                            }
                                            else
                                            {
                                                tempo1 = dao.tabla;
                                                cpPilarPGG.Value = tempo1.Rows[0]["SPPRES$DEPENDE"].ToString();
                                                //cargaobjetivos(Convert.ToInt32(cpPilarPGG.Value), Convert.ToInt32(tabla.Rows[0]["SPPRO$OBJETIVO_CENTRAL"]));
                                                //cargaAcciones(Convert.ToInt32(cbObjSectorial.Value), Convert.ToInt32(tabla.Rows[0]["SPPRO$ACCION_ESTRATEGICA"]));

                                            }

                                        }
                                    }

                                    txtProductoInsto.Text = tabla.Rows[0]["SPPRO$DESCRIPCION"].ToString();
                                    cbUnidadMedida.Value = tabla.Rows[0]["SPPRO$ID_MEDIDA"].ToString();
                                    cbPilar.Enabled = false;
                                    //cbResultadoInsitucional.Enabled = false;
                                    cbProgramaPresupuestario.Enabled = false;
                                    cbSuprograma.Enabled = false;
                                    subProgramaPresupuestario.Enabled = false;
                                    cbSuProgramaPresupuestario.Enabled = false;
                                    txtProductoInsto.Enabled = false;
                                    cbUnidadMedida.Enabled = false;
                                    rbaccion.Enabled = false;
                                    cpPilarPGG.Enabled = false;
                                    cbObjSectorial.Enabled = false;
                                    CbAccionEstrategica.Enabled = false;
                                    btnGrabaProdInsto.CssClass = "btn btn-danger";
                                    btnGrabaProdInsto.Text = "Eliminar producto";
                                    MultiView1.ActiveViewIndex = 5;

                                }
                            }



                        }
                        else
                        {
                            mensaje = "Su perfil de usuario no tiene autorización para eliminiar este producto";
                            Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
                        }
                    }
                    else
                    {
                        mensaje = "Seleccione una fila la cual contenga un producto";
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
                    }
                }
                else
                {
                    mensaje = "Seleccione el producto a eliminar";
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
                }//FIN SELECCION GRID
            }

        }

        protected void btnNuevoEstra_Click(object sender, EventArgs e)
        {
            lblProducto.Text = "Nuevo Producto";
            lblTipo.Text = "Producto";
            lblTipoDescripcion.Text = "Producto";
            lblProducto.Style.Add("color", "#2d572c");

            //cbResultadoInsitucional.Items.Clear();
            //lbTipoResultado.Text = "Meta PGG: ";


            lblPilar.Visible = false;
            lblResultado.Visible = false;
            Session["tipo"] = "estrategico";
            Session["operacion"] = 10;
            cbPilar.Enabled = true;
            //cbResultadoInsitucional.Enabled = true;
            cbProgramaPresupuestario.Enabled = true;
            cbSuprograma.Enabled = true;
            subProgramaPresupuestario.Enabled = true;
            cbSuProgramaPresupuestario.Enabled = true;
            txtProductoInsto.Enabled = true;
            cbUnidadMedida.Enabled = true;
            rbaccion.Enabled = true;
            cpPilarPGG.Enabled = true;
            cbObjSectorial.Enabled = true;
            CbAccionEstrategica.Enabled = true;
            btnGrabaProdInsto.CssClass = "btn btn-primary";
            btnGrabaProdInsto.Text = "Nuevo Producto";
            MultiView1.ActiveViewIndex = 5;
        }

        protected void cargaPilares(int pom, int insto, int pilar, int responsable)
        {
            DataTable pgg = new DataTable();
            if (responsable == 1)
            {
                /*sql = @"SELECT 
                    ID_EJE
                    ,EJE
                    FROM
                    (SELECT ID_EJE, EJE FROM SCHE$SIPLAN20.SPPSVST$PGG2024_2028 PG INNER JOIN 
                    SCHE$SIPLAN20.SP20$RESULTADOS R ON PG.ID_ACCION = R.SPRES$COD_ESTRATEGICO AND R.SPRES$RESTRICTIVA = 'N' AND R.SPRES$POM = " + pom + " AND R.SPRES$INSTITUCION = " + insto + @"

                    UNION

                    SELECT RE.SPPRES$ID_RESULTADO ID_EJE
                           ,RE.SPPRES$CODIGO||'-'||RE.SPPRES$DESCRIPCION EJE
                            FROM SCHE$SIPLAN20.SP20$RESULTADOS_ESTRATEGICOS RE 
                            INNER JOIN SCHE$SIPLAN20.SP20$RESULTADOS R ON RE.SPPRES$ID_RESULTADO = R.SPRES$COD_ESTRATEGICO AND RE.SPPRES$NIVEL = 1 AND R.SPRES$RESTRICTIVA = 'N' AND RE.SPPRES$RESTRICTIVA = 'N'
                            INNER JOIN SCHE$SIPLAN20.S20$PERIODO_RESULTADO PR ON PR.SPR$ID_RESULTADO = RE.SPPRES$ID_RESULTADO AND PR.SPR$RESTRICTIVA = 'N' AND RE.SPPRES$RESTRICTIVA = 'N' AND RE.SPPRES$NIVEL = 1
                            INNER JOIN SCHE$SIPLAN20.SP20$PERIODO_GOBIERNO PG ON PG.SPG$ID_PERIODO = PR.SPR$ID_PERIODO AND PG.SPG$RESTRICTIVA = 'N' AND PR.SPR$RESTRICTIVA = 'N' AND PG.SPG$VIGENTE = 1
                            WHERE RE.SPPRES$NIVEL = 1 AND RE.SPPRES$RESTRICTIVA = 'N' AND R.SPRES$POM = " + pom + " AND R.SPRES$INSTITUCION = " + insto + @"

                    UNION 
                    
                        SELECT ID_EJE, EJE
                        FROM SCHE$SIPLAN20.SPPSVST$PGG2024_2028 WHERE RESPONSABLE = " + insto + @"

                        


                    )
                    GROUP BY
                    ID_EJE
                    ,EJE";*/


                sql = @"SELECT 
                    ID_EJE
                    ,EJE
                    FROM
                    (SELECT ID_EJE, EJE FROM SCHE$SIPLAN20.SPPSVST$PGG2024_2028 PG INNER JOIN 
                    SCHE$SIPLAN20.SP20$RESULTADOS R ON PG.ID_ACCION = R.SPRES$COD_ESTRATEGICO AND R.SPRES$RESTRICTIVA = 'N' AND R.SPRES$POM = " + pom + " AND R.SPRES$INSTITUCION = " + insto + @"

                    UNION

                    SELECT RE.SPPRES$ID_RESULTADO ID_EJE
                           ,RE.SPPRES$CODIGO||'-'||RE.SPPRES$DESCRIPCION EJE
                            FROM SCHE$SIPLAN20.SP20$RESULTADOS_ESTRATEGICOS RE 
                            INNER JOIN SCHE$SIPLAN20.SP20$RESULTADOS R ON RE.SPPRES$ID_RESULTADO = R.SPRES$COD_ESTRATEGICO AND RE.SPPRES$NIVEL = 1 AND R.SPRES$RESTRICTIVA = 'N' AND RE.SPPRES$RESTRICTIVA = 'N'
                            INNER JOIN SCHE$SIPLAN20.S20$PERIODO_RESULTADO PR ON PR.SPR$ID_RESULTADO = RE.SPPRES$ID_RESULTADO AND PR.SPR$RESTRICTIVA = 'N' AND RE.SPPRES$RESTRICTIVA = 'N' AND RE.SPPRES$NIVEL = 1
                            INNER JOIN SCHE$SIPLAN20.SP20$PERIODO_GOBIERNO PG ON PG.SPG$ID_PERIODO = PR.SPR$ID_PERIODO AND PG.SPG$RESTRICTIVA = 'N' AND PR.SPR$RESTRICTIVA = 'N' AND PG.SPG$VIGENTE = 1
                            WHERE RE.SPPRES$NIVEL = 1 AND RE.SPPRES$RESTRICTIVA = 'N' AND R.SPRES$POM = " + pom + " AND R.SPRES$INSTITUCION = " + insto + @"

                    UNION 
                    
                        SELECT 
                        ID_EJE, EJE
                        FROM
                        (SELECT ID_ACCION
                                ,EJE
                                ,META
                                ,ACCION_ESTRATEGICA
                                ,ID_EJE FROM SCHE$SIPLAN20.SPPSVST$PGG2024_2028 WHERE RESPONSABLE = " + insto + @"
                                UNION
                                SELECT ID_ACCION
                                ,EJE
                                ,META
                                ,ACCION_ESTRATEGICA
                                ,ID_EJE FROM SCHE$SIPLAN20.SPPSVST$PGG2024_2028 INNER JOIN SCHE$SIPLAN20.SP20$RESPONSABLESPGG RG ON RG.SPRPG$ID_PGG = ID_ACCION AND SPRPG$NIVEL = 3 AND SPRPG$RESPONSABLE = " + insto + @"
                            )

                        UNION

                        SELECT
                            RG.SPRPG$ID_PGG ID_EJE
                            ,R.SPPRES$CODIGO||'-'||R.SPPRES$DESCRIPCION EJE
                            FROM SCHE$SIPLAN20.SP20$RESULTADOS_ESTRATEGICOS R 
                            INNER JOIN SCHE$SIPLAN20.SP20$RESPONSABLESPGG RG ON RG.SPRPG$ID_PGG = R.SPPRES$ID_RESULTADO AND RG.SPRPG$NIVEL  = 1 AND R.SPPRES$RESTRICTIVA = 'N' AND RG.SPRPG$RESTRICTIVA = 'N'
                            WHERE RG.SPRPG$RESPONSABLE = " + insto + @"

                    )
                    GROUP BY
                    ID_EJE
                    ,EJE
                    ORDER BY ID_EJE ASC";


            }
            else
            {
                /*sql = @"SELECT ID_EJE, EJE FROM SCHE$SIPLAN20.SPPSVST$PGG2024_2028 
                        GROUP BY ID_EJE, EJE
                        ORDER BY ID_EJE ";
                        */

                sql = @"SELECT 
                          ID_EJE
                           ,EJE
                           FROM
                            (SELECT ID_EJE, EJE FROM SCHE$SIPLAN20.SPPSVST$PGG2024_2028
                            UNION
                            SELECT R.SPPRES$ID_RESULTADO ID_EJE, R.SPPRES$CODIGO || '-' || R.SPPRES$DESCRIPCION EJE FROM SCHE$SIPLAN20.SP20$RESULTADOS_ESTRATEGICOS R
                            INNER JOIN SCHE$SIPLAN20.S20$PERIODO_RESULTADO PR ON PR.SPR$ID_RESULTADO = R.SPPRES$ID_RESULTADO AND PR.SPR$RESTRICTIVA = 'N' AND R.SPPRES$RESTRICTIVA = 'N'
                            INNER JOIN SCHE$SIPLAN20.SP20$PERIODO_GOBIERNO PG ON PG.SPG$ID_PERIODO = PR.SPR$ID_PERIODO AND PG.SPG$RESTRICTIVA = 'N' AND PR.SPR$RESTRICTIVA = 'N' AND PG.SPG$VIGENTE = 1)";

            }

            estado = dao.consulta(sql);
            if (estado == 0)
            {
                mensaje = dao.mensaje;
                Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
            }
            else
            {
                pgg = dao.tabla;
                cbPilar.DataSource = pgg;
                cbPilar.ValueField = "ID_EJE";
                cbPilar.TextField = "EJE";
                cbPilar.DataBind();
                if (pilar != -1)
                    cbPilar.Value = pilar.ToString();
                else
                    cbPilar.SelectedIndex = -1;
                cbAcciones.DataSource = null;
                cbAcciones.Items.Clear();
                MultiView1.ActiveViewIndex = 5;
            }


        }

        protected void cbPilar_ValueChanged(object sender, EventArgs e)
        {
            cargaAccionesEstrategicas(-1);


        }

        protected void cargaAccionesEstrategicas(int acciones)
        {
            int resultado = busca_responsable(Convert.ToInt32(Session["insto"]));

            if (resultado == 1)
            {
                /*sql = @"SELECT
                    SPRES$ID_RESULTADO
                    ,RESULTADO_ESTRATEGICO
                    FROM
                    (SELECT PG.ID_ACCION SPRES$ID_RESULTADO
                    ,'ACCION ESTRATÉGICA: '||PG.ACCION_ESTRATEGICA||' META PRESIDENCIAL: '||PG.META RESULTADO_ESTRATEGICO
                    FROM SCHE$SIPLAN20.SPPSVST$PGG2024_2028 PG
                    INNER JOIN SCHE$SIPLAN20.SP20$RESULTADOS R ON PG.ID_ACCION = R.SPRES$COD_ESTRATEGICO AND R.SPRES$RESTRICTIVA = 'N' AND R.SPRES$POM = " + Convert.ToInt32(Session["pom"]) + " AND R.SPRES$INSTITUCION = " + Convert.ToInt32(Session["insto"]) + @"
                    WHERE PG.ID_EJE = " + Convert.ToInt32(cbPilar.Value) + @"

UNION 

SELECT ID_ACCION SPRES$ID_RESULTADO
,'ACCION ESTRATÉGICA: '||ACCION_ESTRATEGICA||' META PRESIDENCIAL: '||META RESULTADO_ESTRATEGICO
FROM SCHE$SIPLAN20.SPPSVST$PGG2024_2028 WHERE ID_EJE = " + Convert.ToInt32(cbPilar.Value) + @" AND RESPONSABLE = " + Convert.ToInt32(Session["insto"]) + @"



                 


                    )

                    GROUP BY 
                    SPRES$ID_RESULTADO
                    ,RESULTADO_ESTRATEGICO";*/

                sql = @"SELECT
                    SPRES$ID_RESULTADO
                    ,RESULTADO_ESTRATEGICO
                    FROM
                    (SELECT PG.ID_ACCION SPRES$ID_RESULTADO
                    ,'ACCION ESTRATÉGICA: '||PG.ACCION_ESTRATEGICA||' META PRESIDENCIAL: '||PG.META RESULTADO_ESTRATEGICO
                    FROM SCHE$SIPLAN20.SPPSVST$PGG2024_2028 PG
                    INNER JOIN SCHE$SIPLAN20.SP20$RESULTADOS R ON PG.ID_ACCION = R.SPRES$COD_ESTRATEGICO AND R.SPRES$RESTRICTIVA = 'N' AND R.SPRES$POM = " + Convert.ToInt32(Session["pom"]) + " AND R.SPRES$INSTITUCION = " + Convert.ToInt32(Session["insto"]) + @"
                    WHERE PG.ID_EJE = " + Convert.ToInt32(cbPilar.Value) + @"

                    UNION 

                    SELECT ID_ACCION SPRES$ID_RESULTADO
                    ,'ACCION ESTRATÉGICA: '||ACCION_ESTRATEGICA||' META PRESIDENCIAL: '||META RESULTADO_ESTRATEGICO
                    FROM SCHE$SIPLAN20.SPPSVST$PGG2024_2028 WHERE ID_EJE = " + Convert.ToInt32(cbPilar.Value) + @" AND RESPONSABLE = " + Convert.ToInt32(Session["insto"]) + @"
                    UNION
                    SELECT PG.ID_ACCION SPRES$ID_RESULTADO
                    ,'ACCION ESTRATÉGICA: '||PG.ACCION_ESTRATEGICA||' META PRESIDENCIAL: '||PG.META RESULTADO_ESTRATEGICO
                    FROM SCHE$SIPLAN20.SPPSVST$PGG2024_2028 PG
                    INNER JOIN SCHE$SIPLAN20.SP20$RESPONSABLESPGG RG ON RG.SPRPG$ID_PGG = PG.ID_ACCION AND RG.SPRPG$NIVEL = 3
                    WHERE PG.ID_EJE = " + Convert.ToInt32(cbPilar.Value) + " AND RG.SPRPG$RESPONSABLE = " + Convert.ToInt32(Session["insto"]) + @"
                    )
                    GROUP BY 
                    SPRES$ID_RESULTADO
                    ,RESULTADO_ESTRATEGICO
                    ORDER BY SPRES$ID_RESULTADO ASC";

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
                        cbAcciones.Enabled = true;
                        cbAcciones.DataSource = tabla;
                        cbAcciones.ValueField = "SPRES$ID_RESULTADO";
                        cbAcciones.TextField = "RESULTADO_ESTRATEGICO";
                        cbAcciones.DataBind();
                        if (acciones != -1)
                            cbAcciones.Value = acciones.ToString();
                        else
                            cbAcciones.SelectedIndex = 0;
                    }
                    else
                    {

                        cbAcciones.Items.Clear();
                        cbAcciones.Enabled = false;
                        cbAcciones.Text = "";
                    }

                    cpPilarPGG.Value = cbPilar.Value;
                    cpPilarPGG.Enabled = false;

                    //cargaobjetivos(Convert.ToInt32(cpPilarPGG.Value), -1);
                    CbAccionEstrategica.Items.Clear();
                    CbAccionEstrategica.SelectedIndex = -1;
                    MultiView1.ActiveViewIndex = 5;

                }



            }
            else
            {
                cbAcciones.Enabled = false;
                cbAcciones.DataSource = null;
                cbAcciones.Items.Clear();
            }
        }

        protected void btnEditEstra_Click(object sender, EventArgs e)
        {//INICIO CARGA DE PRODUCTO ESTRATEGICO
            int nivel, codproducto, codResultado, codPilar;
            String producto, pilar, resultado;
            DataTable tempo1 = new DataTable();
            if (gvProdEstrategicos.FocusedRowIndex != -1)
            {//INICIO SELECCION GRID
                nivel = gvProdEstrategicos.GetRowLevel(gvProdEstrategicos.FocusedRowIndex);
                producto = gvProdEstrategicos.GetRowValues(gvProdEstrategicos.FocusedRowIndex, "PRODUCTO").ToString();
                pilar = gvProdEstrategicos.GetRowValues(gvProdEstrategicos.FocusedRowIndex, "PILAR_PGG").ToString();
                resultado = gvProdEstrategicos.GetRowValues(gvProdEstrategicos.FocusedRowIndex, "RESULTADO_ESTRATEGICO").ToString();
                codproducto = Convert.ToInt32(gvProdEstrategicos.GetRowValues(gvProdEstrategicos.FocusedRowIndex, "SPPRO$ID_PRODUCTO").ToString());
                codResultado = Convert.ToInt32(gvProdEstrategicos.GetRowValues(gvProdEstrategicos.FocusedRowIndex, "SPRES$ID_RESULTADO").ToString());
                codPilar = Convert.ToInt32(gvProdEstrategicos.GetRowValues(gvProdEstrategicos.FocusedRowIndex, "ID_PILAR").ToString());
                if (nivel == 3)
                {//INICIO NIVEL

                    //if ((gvProdEstrategicos.GetRowValues(gvProdEstrategicos.FocusedRowIndex, "SPPRO$PROPIETARIO").ToString() == Session["USUARIO"].ToString()) || Session["ROL"].ToString() == "ADMIN")
                    if (Session["ROL"].ToString() != "ENTIDAD")
                    {//INICIO PROPIETARIO
                        Session["tipo"] = "estrategico";
                        Session["operacion"] = 11;
                        lblProducto.Text = "Producto  vinculado a Metas PGG: " + producto;
                        lblTipo.Text = "Producto vinculado a Metas PGG";
                        lblTipoDescripcion.Text = "Producto vinculado a Metas PGG";
                        //lbTipoResultado.Text = "Meta PGG: ";
                        lblProducto.Style.Add("color", "#2d572c");
                        lblPilar.Text = "Pilar: " + pilar;
                        lblResultado.Text = "Resultado: " + resultado;
                        lblPilar.Visible = true;
                        lblResultado.Visible = true;

                        //cpPilarPGG.Value = codPilar.ToString();
                        //cargaResultadoEstrategico(); ATENCION!!
                        //cbResultadoInsitucional.Value = codResultado.ToString();


                        sql = "SELECT * FROM SCHE$SIPLAN20.SP20$PRODUCTO WHERE SPPRO$ID_PRODUCTO = " + codproducto + " AND SPPRO$RESTRICTIVA = 'N'";
                        estado = dao.consulta(sql);
                        if (estado == 0)
                        {
                            mensaje = dao.mensaje;
                            Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
                        }

                        tabla = dao.tabla;
                        if (tabla.Rows.Count > 0)
                        {//INICIO TABLA LLENA
                            //cbResultadoInsitucional.Value = tabla.Rows[0]["SPPRO$ID_RESULTADO"].ToString();
                            sql = "SELECT * FROM SCHE$SIPLAN20.SP20$PROGRAMA_PRESUPUESTARIO P WHERE P.SPPRO$ID_PROGRAMA_PRESUPUESTO = " + Convert.ToDouble(tabla.Rows[0]["SPPRO$PRESUPUESTO"]) + " AND P.SPPRO$ID_POM = " + Convert.ToInt32(Session["pom"]) + " AND P.SPPRO$ID_INSTITUCION = " + Convert.ToInt32(Session["insto"]) + " AND P.SPPRO$RESTRICTIVA = 'N'";
                            estado = dao.consulta(sql);
                            if (estado == 0)
                            {
                                mensaje = dao.mensaje;
                                Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
                            }
                            else
                            {
                                tempo1 = dao.tabla;
                                if (tempo1.Rows[0]["SPPRO$ID_PROGRAMA_DEPENDE"] == DBNull.Value)
                                {
                                    cbSuprograma.SelectedIndex = 0;
                                    cbProgramaPresupuestario.Value = tabla.Rows[0]["SPPRO$PRESUPUESTO"].ToString();
                                    Cargasubprogramas(Convert.ToDouble(tabla.Rows[0]["SPPRO$PRESUPUESTO"]), Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), -1);
                                    subProgramaPresupuestario.Style.Add("display", "none");
                                }

                                else
                                {
                                    cbSuprograma.SelectedIndex = 1;
                                    cbProgramaPresupuestario.Value = tempo1.Rows[0]["SPPRO$ID_PROGRAMA_DEPENDE"].ToString();
                                    Cargasubprogramas(Convert.ToDouble(tempo1.Rows[0]["SPPRO$ID_PROGRAMA_DEPENDE"]), Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), Convert.ToDouble(tabla.Rows[0]["SPPRO$PRESUPUESTO"]));
                                    subProgramaPresupuestario.Style.Add("display", "block");

                                }

                                if (tabla.Rows[0]["SPPRO$OBJETIVO_CENTRAL"] == DBNull.Value)
                                {
                                    rbaccion.SelectedIndex = 0;
                                    AccionEstrategica.Style.Add("display", "none");
                                }
                                else
                                {
                                    rbaccion.SelectedIndex = 1;
                                    AccionEstrategica.Style.Add("display", "block");
                                    sql = "SELECT * FROM SCHE$SIPLAN20.SP20$RESULTADOS_ESTRATEGICOS R WHERE R.SPPRES$ID_RESULTADO = " + Convert.ToInt32(tabla.Rows[0]["SPPRO$OBJETIVO_CENTRAL"]) + " AND R.SPPRES$NIVEL = 3 AND R.SPPRES$RESTRICTIVA = 'N'";
                                    estado = dao.consulta(sql);
                                    if (estado == 0)
                                    {
                                        mensaje = dao.mensaje;
                                        Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
                                    }
                                    else
                                    {
                                        tempo1 = dao.tabla;
                                        cpPilarPGG.Enabled = false;
                                        cpPilarPGG.Value = tempo1.Rows[0]["SPPRES$DEPENDE"].ToString();
                                        //cargaobjetivos(Convert.ToInt32(cpPilarPGG.Value), Convert.ToInt32(tabla.Rows[0]["SPPRO$OBJETIVO_CENTRAL"]));
                                        cargaAcciones(Convert.ToInt32(cbObjSectorial.Value), Convert.ToInt32(tabla.Rows[0]["SPPRO$ACCION_ESTRATEGICA"]));

                                    }

                                }
                            }

                            txtProductoInsto.Text = tabla.Rows[0]["SPPRO$DESCRIPCION"].ToString();
                            cbUnidadMedida.Value = tabla.Rows[0]["SPPRO$ID_MEDIDA"].ToString();
                            //cbResultadoInsitucional.Enabled = true;
                            cbProgramaPresupuestario.Enabled = true;
                            cbSuprograma.Enabled = true;
                            subProgramaPresupuestario.Enabled = true;
                            cbSuProgramaPresupuestario.Enabled = true;
                            txtProductoInsto.Enabled = true;
                            cbUnidadMedida.Enabled = true;
                            rbaccion.Enabled = true;
                            cpPilarPGG.Enabled = false;
                            cbObjSectorial.Enabled = true;
                            CbAccionEstrategica.Enabled = true;
                            btnGrabaProdInsto.CssClass = "btn btn-success";
                            btnGrabaProdInsto.Text = "Editar producto";
                            MultiView1.ActiveViewIndex = 5;

                        }//FIN TABLA LLENA

                    }

                    else
                    {
                        mensaje = "Su perfil de usuario no esta autorizado para modificar este registro";
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
                        ProdEstrategicos.Style.Add("display", "block");
                        ProdInsitucionales.Style.Add("display", "none");
                    }//FIN PROPIETARIO

                }

                else
                {
                    mensaje = "Debe seleccionar una fila la cual contenga un producto";
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
                    ProdEstrategicos.Style.Add("display", "block");
                    ProdInsitucionales.Style.Add("display", "none");
                }//FIN NIVELG
            }//FIN SELECCION GRID


        }//FIN CARGA DE PRODUCTO ESTRATEGICO

        /* protected void cargaResultadoEstrategico()
         {

             sql = @"SELECT
                     SPRES$ID_RESULTADO
                     ,RESULTADO_ESTRATEGICO
                     FROM
                     (SELECT SPRES$ID_RESULTADO
                     ,'ACCIÓN ESTRATÉGICA: '||PG.ACCION_ESTRATEGICA||' META PRESIDENCIAL: '||PG.META RESULTADO_ESTRATEGICO
                     FROM SCHE$SIPLAN20.SPPSVST$PGG2024_2028 PG
                     INNER JOIN SCHE$SIPLAN20.SP20$RESULTADOS R ON PG.ID_ACCION = R.SPRES$COD_ESTRATEGICO AND R.SPRES$RESTRICTIVA = 'N' AND R.SPRES$POM = "+ Convert.ToInt32(Session["pom"]) + " AND R.SPRES$INSTITUCION = "+ Convert.ToInt32(Session["insto"]) + @"
                     WHERE PG.ID_EJE = "+ Convert.ToInt32(cbPilar.Value) + @"

                     UNION

                      SELECT R.SPRES$ID_RESULTADO
                     ,RE.SPPRES$CODIGO||'-'||RE.SPPRES$DESCRIPCION RESULTADO_ESTRATEGICO
                     FROM SCHE$SIPLAN20.SP20$RESULTADOS R INNER JOIN
                     SCHE$SIPLAN20.SP20$RESULTADOS_ESTRATEGICOS RE ON RE.SPPRES$ID_RESULTADO = R.SPRES$COD_ESTRATEGICO AND RE.SPPRES$RESTRICTIVA = 'N' AND R.SPRES$RESTRICTIVA = 'N' AND RE.SPPRES$NIVEL = 1
                     WHERE R.SPRES$POM = "+ Convert.ToInt32(Session["pom"]) + " AND R.SPRES$INSTITUCION = "+ Convert.ToInt32(Session["insto"]) + " AND R.SPRES$TIPO = 0 AND R.SPRES$RESTRICTIVA = 'N' AND RE.SPPRES$ID_RESULTADO = "+ Convert.ToInt32(cbPilar.Value) + @"


                     )

                     GROUP BY 
                     SPRES$ID_RESULTADO
                     ,RESULTADO_ESTRATEGICO";
             estado = dao.consulta(sql);
             if (estado == 0)
             {
                 mensaje = dao.mensaje;
                 Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
             }
             else
             {
                 tabla = dao.tabla;
                 cbResultadoInsitucional.DataSource = tabla;
                 cbResultadoInsitucional.ValueField = "SPRES$ID_RESULTADO";
                 cbResultadoInsitucional.TextField = "RESULTADO_ESTRATEGICO";
                 cbResultadoInsitucional.DataBind();
                 cpPilarPGG.Value = cbPilar.Value;
                 cpPilarPGG.Enabled = false;
                 //cargaobjetivos(Convert.ToInt32(cpPilarPGG.Value), -1);
                 MultiView1.ActiveViewIndex = 5;
             }
         }
         */
        protected void rbProductos_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rbProductos.SelectedIndex == 0)
            {
                cargaProductosInsto(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]));
                ProdInsitucionales.Style.Add("display", "block");
                ProdEstrategicos.Style.Add("display", "none");
            }
            else if (rbProductos.SelectedIndex == 1)
            {
                cargaProductoRed(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]));
                ProdInsitucionales.Style.Add("display", "none");
                ProdEstrategicos.Style.Add("display", "block");

            }

        }

        protected void btnDeleEstra_Click(object sender, EventArgs e)
        {
            int nivel, codproducto, codResultado, codPilar;
            String producto, pilar, resultado;
            DataTable tempo1 = new DataTable();
            if (gvProdEstrategicos.FocusedRowIndex != -1)
            {//INICIO SELECCION GRID
                nivel = gvProdEstrategicos.GetRowLevel(gvProdEstrategicos.FocusedRowIndex);
                producto = gvProdEstrategicos.GetRowValues(gvProdEstrategicos.FocusedRowIndex, "PRODUCTO").ToString();
                pilar = gvProdEstrategicos.GetRowValues(gvProdEstrategicos.FocusedRowIndex, "PILAR_PGG").ToString();
                resultado = gvProdEstrategicos.GetRowValues(gvProdEstrategicos.FocusedRowIndex, "RESULTADO_ESTRATEGICO").ToString();
                codproducto = Convert.ToInt32(gvProdEstrategicos.GetRowValues(gvProdEstrategicos.FocusedRowIndex, "SPPRO$ID_PRODUCTO").ToString());
                codResultado = Convert.ToInt32(gvProdEstrategicos.GetRowValues(gvProdEstrategicos.FocusedRowIndex, "SPRES$ID_RESULTADO").ToString());
                codPilar = Convert.ToInt32(gvProdEstrategicos.GetRowValues(gvProdEstrategicos.FocusedRowIndex, "ID_PILAR").ToString());
                if (nivel == 3)
                {
                    //if ((gvProdEstrategicos.GetRowValues(gvProdEstrategicos.FocusedRowIndex, "SPPRO$PROPIETARIO").ToString() == Session["USUARIO"].ToString()) || Session["ROL"].ToString() == "ADMIN")
                    if (Session["ROL"].ToString() != "ENTIDAD")
                    {
                        Session["tipo"] = "estrategico";
                        Session["operacion"] = 12;
                        lblPilar.Text = "Pilar: " + pilar;
                        lblPilar.Visible = true;
                        lblResultado.Text = "Resultado: " + resultado;
                        lblResultado.Visible = true;
                        lblProducto.Text = "Esta por eliminar el Producto institucional: '" + producto + "'. Por favor confirme la operación presionando el boton 'Elminar producto'";
                        lblProducto.Style.Add("color", "red");

                        //cpPilarPGG.Value = codPilar.ToString();
                        //cargaResultadoEstrategico();
                        //cbResultadoInsitucional.Value = codResultado.ToString();


                        lblTipo.Text = "Producto vinculado a Metas PGG";
                        lblTipoDescripcion.Text = "Producto vinculado a Metas PGG";
                        //lbTipoResultado.Text = "Meta PGG :";
                        sql = "SELECT * FROM SCHE$SIPLAN20.SP20$PRODUCTO WHERE SPPRO$ID_PRODUCTO = " + codproducto + " AND SPPRO$RESTRICTIVA = 'N'";
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
                                //cbResultadoInsitucional.Value = tabla.Rows[0]["SPPRO$ID_RESULTADO"].ToString();
                                sql = "SELECT * FROM SCHE$SIPLAN20.SP20$PROGRAMA_PRESUPUESTARIO P WHERE P.SPPRO$ID_PROGRAMA_PRESUPUESTO = " + Convert.ToDouble(tabla.Rows[0]["SPPRO$PRESUPUESTO"]) + " AND P.SPPRO$ID_POM = " + Convert.ToInt32(Session["pom"]) + " AND P.SPPRO$ID_INSTITUCION = " + Convert.ToInt32(Session["insto"]) + " AND P.SPPRO$RESTRICTIVA = 'N'";
                                estado = dao.consulta(sql);
                                if (estado == 0)
                                {
                                    mensaje = dao.mensaje;
                                    Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
                                }
                                else
                                {
                                    tempo1 = dao.tabla;
                                    if (tempo1.Rows[0]["SPPRO$ID_PROGRAMA_DEPENDE"] == DBNull.Value)
                                    {
                                        cbSuprograma.SelectedIndex = 0;
                                        cbProgramaPresupuestario.Value = tabla.Rows[0]["SPPRO$PRESUPUESTO"].ToString();
                                        Cargasubprogramas(Convert.ToDouble(tabla.Rows[0]["SPPRO$PRESUPUESTO"]), Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), -1);
                                        subProgramaPresupuestario.Style.Add("display", "none");
                                    }

                                    else
                                    {
                                        cbSuprograma.SelectedIndex = 1;
                                        cbProgramaPresupuestario.Value = tempo1.Rows[0]["SPPRO$ID_PROGRAMA_DEPENDE"].ToString();
                                        Cargasubprogramas(Convert.ToDouble(tempo1.Rows[0]["SPPRO$ID_PROGRAMA_DEPENDE"]), Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), Convert.ToDouble(tabla.Rows[0]["SPPRO$PRESUPUESTO"]));
                                        subProgramaPresupuestario.Style.Add("display", "block");

                                    }

                                    if (tabla.Rows[0]["SPPRO$OBJETIVO_CENTRAL"] == DBNull.Value)
                                    {
                                        rbaccion.SelectedIndex = 0;
                                        AccionEstrategica.Style.Add("display", "none");
                                    }
                                    else
                                    {
                                        rbaccion.SelectedIndex = 1;
                                        AccionEstrategica.Style.Add("display", "block");
                                        sql = "SELECT * FROM SCHE$SIPLAN20.SP20$RESULTADOS_ESTRATEGICOS R WHERE R.SPPRES$ID_RESULTADO = " + Convert.ToInt32(tabla.Rows[0]["SPPRO$OBJETIVO_CENTRAL"]) + " AND R.SPPRES$NIVEL = 3 AND R.SPPRES$RESTRICTIVA = 'N'";
                                        estado = dao.consulta(sql);
                                        if (estado == 0)
                                        {
                                            mensaje = dao.mensaje;
                                            Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
                                        }
                                        else
                                        {
                                            tempo1 = dao.tabla;
                                            cpPilarPGG.Value = tempo1.Rows[0]["SPPRES$DEPENDE"].ToString();
                                            //cargaobjetivos(Convert.ToInt32(cpPilarPGG.Value), Convert.ToInt32(tabla.Rows[0]["SPPRO$OBJETIVO_CENTRAL"]));
                                            //cargaAcciones(Convert.ToInt32(cbObjSectorial.Value), Convert.ToInt32(tabla.Rows[0]["SPPRO$ACCION_ESTRATEGICA"]));

                                        }

                                    }
                                }

                                txtProductoInsto.Text = tabla.Rows[0]["SPPRO$DESCRIPCION"].ToString();
                                cbUnidadMedida.Value = tabla.Rows[0]["SPPRO$ID_MEDIDA"].ToString();
                                cbPilar.Enabled = false;
                                //cbResultadoInsitucional.Enabled = false;
                                cbProgramaPresupuestario.Enabled = false;
                                cbSuprograma.Enabled = false;
                                subProgramaPresupuestario.Enabled = false;
                                cbSuProgramaPresupuestario.Enabled = false;
                                txtProductoInsto.Enabled = false;
                                cbUnidadMedida.Enabled = false;
                                rbaccion.Enabled = false;
                                cpPilarPGG.Enabled = false;
                                cbObjSectorial.Enabled = false;
                                CbAccionEstrategica.Enabled = false;
                                btnGrabaProdInsto.CssClass = "btn btn-danger";
                                btnGrabaProdInsto.Text = "Eliminar producto";
                                MultiView1.ActiveViewIndex = 5;

                            }
                        }



                    }
                    else
                    {
                        mensaje = "Su perfil de usuario no tiene autorización para eliminiar este producto";
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
                    }
                }
                else
                {
                    mensaje = "Seleccione una fila la cual contenga un producto";
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
                }
            }
            else
            {
                mensaje = "Seleccione el producto a eliminar";
                Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
            }//FIN SELECCION GRID
        }

        protected void btnSuProductos_Click(object sender, EventArgs e)
        {
            int nivel, codproducto;
            String producto, pilar, resultado;
            DataTable tempo1 = new DataTable();

            if (gvProdEstrategicos.FocusedRowIndex != -1)
            {
                nivel = gvProdEstrategicos.GetRowLevel(gvProdEstrategicos.FocusedRowIndex);
                producto = gvProdEstrategicos.GetRowValues(gvProdEstrategicos.FocusedRowIndex, "PRODUCTO").ToString();
                pilar = gvProdEstrategicos.GetRowValues(gvProdEstrategicos.FocusedRowIndex, "PILAR_PGG").ToString();
                resultado = gvProdEstrategicos.GetRowValues(gvProdEstrategicos.FocusedRowIndex, "RESULTADO_ESTRATEGICO").ToString();
                codproducto = Convert.ToInt32(gvProdEstrategicos.GetRowValues(gvProdEstrategicos.FocusedRowIndex, "SPPRO$ID_PRODUCTO").ToString());
                Session["pom"] = Convert.ToInt32(gvPOMInsto.GetRowValues(gvPOMInsto.FocusedRowIndex, "SPPO$ID_POM").ToString());
                Session["insto"] = Convert.ToInt32(gvPOMInsto.GetRowValues(gvPOMInsto.FocusedRowIndex, "SPPO$ID_INSTITUCION").ToString());
                if (nivel == 3)
                {

                    Session["tiposub"] = "subes";
                    sql = "SELECT * FROM SCHE$SIPLAN20.SPPVST$SUBESTRATEGICO SE WHERE SE.SPRES$POM = " + Convert.ToInt32(Session["pom"]) + " AND SE.SPRES$INSTITUCION = " + Convert.ToInt32(Session["insto"]) + " AND SE.SPPRO$ID_PRODUCTO = " + codproducto;
                    estado = dao.consulta(sql);
                    if (estado == 0)
                    {
                        mensaje = dao.mensaje;
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
                    }
                    else
                    {
                        lbltitulo.Text = "Subproductos: " + producto;
                        lblPilarsub.Visible = true;
                        lblPilarsub.Text = "Pilar: " + pilar;
                        lblResultasub.Text = "Meta PGG: " + resultado;
                        lblProdsub.Text = "Producto  vinculado a Metas PGG: " + producto;
                        tabla = dao.tabla;
                        //gvSubproductos.Columns.Clear();
                        //GridViewDataColumn c = new GridViewDataColumn();
                        //c.FieldName = "PILAR_PGG";
                        //c.VisibleIndex = 1;
                        //c.GroupIndex = 0;
                        //c.Visible = true;

                        //gvSubproductos.Columns.Add(c);
                        gvSubproductos.DataSource = tabla;
                        gvSubproductos.DataBind();
                        gvSubproductos.ExpandAll();
                        MultiView1.ActiveViewIndex = 6;


                    }
                }
                else
                {
                    mensaje = "Seleccione una fila la cual contenga un producto";
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
                }
            }

            else
            {
                mensaje = "Seleccione el producto al cual se van crear subproductos";
                Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
            }
        }

        protected void cargaSubEstrategicos(int pom, int insto, int producto)
        {
            sql = "SELECT * FROM SCHE$SIPLAN20.SPPSVST$RED SE WHERE SE.SPRES$POM = " + Convert.ToInt32(Session["pom"]) + " AND SE.SPRES$INSTITUCION = " + Convert.ToInt32(Session["insto"]) + " AND SE.SPPRO$ID_PRODUCTO = " + producto;
            estado = dao.consulta(sql);
            if (estado == 0)
            {
                mensaje = dao.mensaje;
                Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
            }
            else
            {

                tabla = dao.tabla;
                gvSubproductos.DataSource = tabla;
                gvSubproductos.DataBind();
                gvSubproductos.ExpandAll();


            }
        }

        protected void cargaSubInstos(int pom, int insto, int producto)
        {
            sql = "SELECT * FROM SCHE$SIPLAN20.SPPSVT$SUBINSTITUCIONALV2 SE WHERE SE.SPRES$POM = " + Convert.ToInt32(Session["pom"]) + " AND SE.SPRES$INSTITUCION = " + Convert.ToInt32(Session["insto"]) + " AND SE.SPPRO$ID_PRODUCTO = " + producto;
            estado = dao.consulta(sql);
            if (estado == 0)
            {
                mensaje = dao.mensaje;
                Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
            }
            else
            {

                tabla = dao.tabla;
                gvSubproductos.DataSource = tabla;
                gvSubproductos.DataBind();
                gvSubproductos.ExpandAll();



            }
        }

        protected void btnCancelaSub_Click(object sender, EventArgs e)
        {
            int codproducto;
            lblPilarRes.Text = "";
            lblResub2.Text = "";
            lblProduEstra2.Text = "";             lblsubso.Text = "subproducto vinculado a Metas PGG";
            txtSubproducto.Text = "";
            cargaUnidadesSub();
            cbUnidadesSub.SelectedIndex = -1;
            rbSNIP.SelectedIndex = 1;
            PanSNIP.Style.Add("display", "none");
            cargaSNIP(Convert.ToInt32(Session["insto"]));
            cbSNIP.SelectedIndex = -1;
            txtSubproducto.Enabled = true;
            cbUnidadesSub.Enabled = true;
            cbSNIP.Enabled = true;

            rbSNIP.Enabled = true;
            if (Session["tiposub"].ToString() == "subes")
            {
                codproducto = Convert.ToInt32(gvProdEstrategicos.GetRowValues(gvProdEstrategicos.FocusedRowIndex, "SPPRO$ID_PRODUCTO").ToString());

                cargaSubEstrategicos(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), codproducto);
            }

            MultiView1.ActiveViewIndex = 6;
        }

        protected void cargaUnidadesSub()
        {
            sql = "SELECT a.sncgum$unidad_medida, a.sncgum$nombre FROM sinip.sntbcg$unidad_medida a WHERE a.sncgum$unidad_medida >= 1000 AND a.sncgum$sigla IS NOT NULL ORDER BY a.sncgum$nombre ASC";
            estado = dao.consulta(sql);
            if (estado == 0)
            {
                mensaje = dao.mensaje;
                Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
            }
            else
            {
                tabla = dao.tabla;
                cbUnidadesSub.DataSource = tabla;
                cbUnidadesSub.ValueField = "sncgum$unidad_medida";
                cbUnidadesSub.TextField = "sncgum$nombre";
                cbUnidadesSub.DataBind();
            }
        }

        protected void cargaSNIP(int insto)
        {
            DataTable periodo = new DataTable();
            sql = "SELECT P.SPP$INICIO, P.SPP$FINAL FROM SCHE$SIPLAN20.SP20$PERIODO P INNER JOIN SCHE$SIPLAN20.SP20$POM PO ON P.SPP$ID_PERIODO = PO.SPPO$ID_PERIODO AND P.SPP$RESTRICTIVA = 'N' AND PO.SPPO$RESTRICTIVA = 'N' AND SPPO$ID_POM = " + Convert.ToInt32(Session["pom"]);
            estado = dao.consulta(sql);
            if (estado == 1)
                periodo = dao.tabla;

            if (periodo.Rows.Count > 0)
            {
                if (insto == 54000)

                    sql = @"SELECT 
                            a.proyecto
                           ,a.proyecto||'-'||a.nombre proyectoSNIP
                           ,b.sigla FROM 
                            SINIP.BP_PROYECTO_ID a
                            ,SINIP.ci_unidad_ejecutora b
                            ,SINIP.BP_ETAPAS_FINANCIAR PO  WHERE a.restrictiva  = 'N'  
                            AND a.entidad = b.entidad AND a.unidad_ejecutora = b.unidad_ejecutora and a.ENTIDAD = " + insto + " AND a.PROYECTO = PO.PROYECTO AND PO.EJERCICIO BETWEEN 2018 AND " + periodo.Rows[0]["SPP$FINAL"] +
                            " UNION " +
                            @"SELECT a.proyecto, a.proyecto||'-'||a.nombre proyectoSNIP, b.sigla 
                            FROM SINIP.BP_PROYECTO_ID a
                            ,SINIP.ci_unidad_ejecutora b
                            WHERE a.restrictiva = 'N'
                            AND a.entidad = b.entidad AND a.unidad_ejecutora = b.unidad_ejecutora and a.ENTIDAD = " + insto + @" AND a.proyecto = 263493";
                else
                    sql = "SELECT a.proyecto, a.proyecto||'-'||a.nombre proyectoSNIP, b.sigla FROM SINIP.BP_PROYECTO_ID a, SINIP.ci_unidad_ejecutora b, SINIP.BP_ETAPAS_FINANCIAR PO  WHERE a.restrictiva  = 'N'  AND a.entidad = b.entidad AND a.unidad_ejecutora = b.unidad_ejecutora and a.ENTIDAD = " + insto + " AND a.PROYECTO = PO.PROYECTO AND PO.EJERCICIO BETWEEN " + periodo.Rows[0]["SPP$INICIO"] + " AND " + periodo.Rows[0]["SPP$FINAL"];
            }

            else
                sql = "SELECT a.proyecto, a.proyecto||'-'||a.nombre proyectoSNIP, b.sigla FROM SINIP.BP_PROYECTO_ID a, SINIP.ci_unidad_ejecutora b, SINIP.BP_ETAPAS_FINANCIAR PO  WHERE a.restrictiva  = 'N'  AND a.entidad = b.entidad AND a.unidad_ejecutora = b.unidad_ejecutora and a.ENTIDAD = " + insto + " AND a.PROYECTO = PO.PROYECTO AND PO.EJERCICIO = " + anio.Year;

            estado = dao.consulta(sql);
            if (estado == 0)
            {
                mensaje = dao.mensaje;
                Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
            }
            else
            {
                tabla = dao.tabla;                
                cbSNIP.Items.Clear();
                cbSNIP.DataSource = tabla;
                cbSNIP.ValueField = "proyecto";
                cbSNIP.TextField = "proyectoSNIP";
                cbSNIP.DataBind();
                /*
                cbMunosPriorizados.SelectedIndex = 0;
                cbMunosPriorizados.Enabled = false;
                munoPriorizados.Attributes.Add("style", "display:none");
                */

            }
        }

        protected void btnNuevoSub_Click(object sender, EventArgs e)
        {

            Session["tiposubopera"] = 20;

            cbMunosPriorizados.Enabled = true;

            if (Session["tiposub"].ToString() == "subes")
            {

                lblProduEstra2.Text = gvProdEstrategicos.GetRowValues(gvProdEstrategicos.FocusedRowIndex, "PRODUCTO").ToString();
                lblsubso.Text = "subproducto vinculado a Resultado Estratégico (RE)";
            }
            else if (Session["tiposub"].ToString() == "subins")
            {
                //lblPilarRes.Text = gvProdEstrategicos.GetRowValues(gvProdEstrategicos.FocusedRowIndex, "PILAR_PGG").ToString(); ;
                lblPilarRes.Visible = false;
                lblResub2.Text = gvProdInsto.GetRowValues(gvProdInsto.FocusedRowIndex, "SPRES$DESCRIPCION").ToString();
                lblProduEstra2.Text = "Producto: " + gvProdInsto.GetRowValues(gvProdInsto.FocusedRowIndex, "PRODUCTO").ToString();
                lblsubso.Text = "subproducto institucional";
            }

            numPoliticas.Text = "0";
            lblSubMunos.Text = "0";


            lblSuproducto.Visible = false;

            txtSubproducto.Text = "";
            cargaUnidadesSub();
            cbUnidadesSub.SelectedIndex = -1;
            rbSNIP.SelectedIndex = -1;
            PanSNIP.Style.Add("display", "none");
            cargaSNIP(Convert.ToInt32(Session["insto"]));
            rbSNIP.SelectedIndex = 0;
            cbSNIP.SelectedIndex = -1;
            txtSubproducto.Enabled = true;
            cbUnidadesSub.Enabled = true;
            cbSNIP.Enabled = true;

            rbSNIP.Enabled = true;
            btnSubProducto.CssClass = "btn btn-primary";
            btnSubProducto.Text = "Nuevo subproducto";
            aspCod.Value = "-1";
            if (Convert.ToInt32(hfNaturaleza.Value) == 0)
            {
                cbMunosPriorizados.SelectedIndex = 1;
                cbMunosPriorizados.Enabled = false;
                cbMunosPriorizados_SelectedIndexChanged(sender, e);
                munoPriorizados.Attributes.Add("style", "display:block");
            }
            else
            {
                cbMunosPriorizados.SelectedIndex = 0;
                cbMunosPriorizados.Enabled = false;
                cbMunosPriorizados_SelectedIndexChanged(sender, e);
                munoPriorizados.Attributes.Add("style", "display:none");

            }

                MultiView1.ActiveViewIndex = 7;
        }

        protected void cbSNIP_ValueChanged(object sender, EventArgs e)
        {
            int codproducto = 0;
            if (Session["tiposub"].ToString() == "subes")
            {
                codproducto = Convert.ToInt32(gvProdEstrategicos.GetRowValues(gvProdEstrategicos.FocusedRowIndex, "SPPRO$ID_PRODUCTO").ToString());
            }


            sql = "SELECT * FROM SCHE$SIPLAN20.SP20$SUB_PRODUCTO WHERE SPPSUB$SNIP = " + cbSNIP.Value + " AND SPPSUB$RESTRICTIVA = 'N' AND SPPSUB$ID_PRODUCTO = " + codproducto;
            estado = dao.consulta(sql);
            if (estado == 0)
            {
                mensaje = dao.mensaje;
                Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
                PanSNIP.Style.Add("display", "block");
                MultiView1.ActiveViewIndex = 7;
            }
            else
            {
                tabla = dao.tabla;
                if (tabla.Rows.Count > 0)
                {
                    mensaje = "Ya hay un suproducto asociado a este SNIP, por favor seleccione otro proyecto";
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
                    PanSNIP.Style.Add("display", "block");
                    cbSNIP.SelectedIndex = -1;
                    cbSNIP.Focus();
                    MultiView1.ActiveViewIndex = 7;
                }
                else
                {
                    PanSNIP.Style.Add("display", "block");
                    cbSNIP.Focus();
                    MultiView1.ActiveViewIndex = 7;
                }
            }
        }

        protected void btnSubProducto_Click(object sender, EventArgs e)
        {
            int codproducto = 0;
            int codSuProducto = 0;
            int busca = 0;
            if (Session["tiposub"].ToString() == "subes" && Convert.ToInt32(Session["tiposubopera"]) == 20)
            {
                codproducto = Convert.ToInt32(gvProdEstrategicos.GetRowValues(gvProdEstrategicos.FocusedRowIndex, "SPPRO$ID_PRODUCTO").ToString());
            }
            else if (Session["tiposub"].ToString() == "subins" && Convert.ToInt32(Session["tiposubopera"]) == 20)
            {
                codproducto = Convert.ToInt32(gvProdInsto.GetRowValues(gvProdInsto.FocusedRowIndex, "SPPRO$ID_PRODUCTO").ToString());
            }

            if (Session["tiposub"].ToString() == "subes" && Convert.ToInt32(Session["tiposubopera"]) == 21)
            {
                codproducto = Convert.ToInt32(gvSubproductos.GetRowValues(gvSubproductos.FocusedRowIndex, "SPPRO$ID_PRODUCTO").ToString());
                codSuProducto = Convert.ToInt32(gvSubproductos.GetRowValues(gvSubproductos.FocusedRowIndex, "SPPSUB$ID_SUBPRODUCTO").ToString());
            }

            else if (Session["tiposub"].ToString() == "subins" && Convert.ToInt32(Session["tiposubopera"]) == 21)
            {
                codproducto = Convert.ToInt32(gvSubproductos.GetRowValues(gvSubproductos.FocusedRowIndex, "SPPRO$ID_PRODUCTO").ToString());
                codSuProducto = Convert.ToInt32(gvSubproductos.GetRowValues(gvSubproductos.FocusedRowIndex, "SPPSUB$ID_SUBPRODUCTO").ToString());
            }

            if (Session["tiposub"].ToString() == "subes" && Convert.ToInt32(Session["tiposubopera"]) == 22)
            {
                codproducto = Convert.ToInt32(gvSubproductos.GetRowValues(gvSubproductos.FocusedRowIndex, "SPPRO$ID_PRODUCTO").ToString());
                codSuProducto = Convert.ToInt32(gvSubproductos.GetRowValues(gvSubproductos.FocusedRowIndex, "SPPSUB$ID_SUBPRODUCTO").ToString());
            }
            else if (Session["tiposub"].ToString() == "subins" && Convert.ToInt32(Session["tiposubopera"]) == 22)
            {
                codproducto = Convert.ToInt32(gvSubproductos.GetRowValues(gvSubproductos.FocusedRowIndex, "SPPRO$ID_PRODUCTO").ToString());
                codSuProducto = Convert.ToInt32(gvSubproductos.GetRowValues(gvSubproductos.FocusedRowIndex, "SPPSUB$ID_SUBPRODUCTO").ToString());
            }



            if (Convert.ToInt32(Session["tiposubopera"]) == 22)
            {

                //if ((gvSubproductos.GetRowValues(gvSubproductos.FocusedRowIndex, "SPPSUB$PROPIETARIO").ToString() == Session["USUARIO"].ToString()) || Session["ROL"].ToString() == "ADMIN")
                if (Session["ROL"].ToString() != "ENTIDAD")
                {
                    sql = "SELECT * FROM SCHE$SIPLAN20.SP20$PROGRAMETA_FI_FIN_SUB WHERE SPPMFS$ID_SUBPRODUCTO = " + codSuProducto + " AND SPPMFS$RESTRICTIVA ='N'";
                    estado = dao.consulta(sql);
                    if (estado == 0)
                    {
                        mensaje = dao.mensaje;
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
                    }
                    else
                    {
                        //tabla = dao.tabla;
                        //if (tabla.Rows.Count > 0)
                        //{
                        //mensaje = "El subproducto tiene definidas metas fisicas o financieras multianuales, no puede ser eliminado a menos que inactive primero estos resgistros";
                        //Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
                        //}
                        //else
                        //{
                        sql = "UPDATE SCHE$SIPLAN20.SP20$SUB_PRODUCTO SET SPPSUB$RESTRICTIVA = 'S' WHERE SPPSUB$ID_SUBPRODUCTO = " + codSuProducto + " AND SPPSUB$ID_PRODUCTO  = " + codproducto;
                        estado = dao.comando(sql);
                        if (estado == 0)
                        {
                            mensaje = dao.mensaje;
                            Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
                            lblPilarRes.Text = "";
                            lblResub2.Text = "";
                            lblProduEstra2.Text = "";

                            txtSubproducto.Text = "";
                            cargaUnidadesSub();
                            cbUnidadesSub.SelectedIndex = -1;
                            rbSNIP.SelectedIndex = 1;
                            PanSNIP.Style.Add("display", "none");
                            cargaSNIP(Convert.ToInt32(Session["insto"]));
                            cbSNIP.SelectedIndex = -1;
                            txtSubproducto.Enabled = true;
                            cbUnidadesSub.Enabled = true;
                            cbSNIP.Enabled = true;

                            rbSNIP.Enabled = true;
                            if (Session["tiposub"].ToString() == "subes")
                            {
                                lblsubso.Text = "subproducto vinculado a Metas PGG";
                                codproducto = Convert.ToInt32(gvProdEstrategicos.GetRowValues(gvProdEstrategicos.FocusedRowIndex, "SPPRO$ID_PRODUCTO").ToString());
                                cargaSubEstrategicos(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), codproducto);
                            }

                            else if (Session["tiposub"].ToString() == "subins")
                            {
                                lblsubso.Text = "subproducto institucional";
                                codproducto = Convert.ToInt32(gvProdInsto.GetRowValues(gvProdInsto.FocusedRowIndex, "SPPRO$ID_PRODUCTO").ToString());
                                cargaSubInstos(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), codproducto);
                            }
                            MultiView1.ActiveViewIndex = 6;
                        }

                        else
                        {
                            

                            sql = "UPDATE SCHE$SIPLAN20.SP20$PROGRAMETA_FI_FIN_SUB SET SPPMFS$RESTRICTIVA = 'S', SPPMFS$FECHA_DELETE = 'DELETE = '||TO_CHAR(SYSDATE,'DD/MM/YYYY HH24:MI')||' '||'" + Session["USUARIO"].ToString() + "' WHERE SPPMFS$ID_SUBPRODUCTO = " + codSuProducto;
                            estado = dao.comando(sql);

                            sql = "UPDATE SCHE$SIPLAN20.SP20$POASUBPRODUCTOS SET SPOAS$RESTRICTIVA = 'S', SPOAS$FECHA_DELETE = 'DELETE = '||TO_CHAR(SYSDATE,'DD/MM/YYYY HH24:MI')||' '||'" + Session["USUARIO"].ToString() + "' WHERE SPOAS$SUBPRODUCTO = " + codSuProducto;
                            estado = dao.comando(sql);

                            sql = "UPDATE SCHE$SIPLAN20.SP20$SUB_MUNOS SET SPSM$RESTRICTIVA = 'S', SPSM$FECHA_ELIMINA  = 'BORRA = '||TO_CHAR(SYSDATE,'DD/MM/YYYY HH24:MI')||' '||'" + Session["USUARIO"].ToString() + "' WHERE SPSM$ID_SUB = " + codSuProducto + " AND SPSM$RESTRICTIVA = 'N'";
                            estado = dao.comando(sql);

                            mensaje = "Registro eliminado correctamente";
                            Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',1);", true);
                            lblPilarRes.Text = "";
                            lblResub2.Text = "";
                            lblProduEstra2.Text = "";

                            txtSubproducto.Text = "";
                            cargaUnidadesSub();
                            cbUnidadesSub.SelectedIndex = -1;
                            rbSNIP.SelectedIndex = 1;
                            PanSNIP.Style.Add("display", "none");
                            cargaSNIP(Convert.ToInt32(Session["insto"]));
                            cbSNIP.SelectedIndex = -1;
                            txtSubproducto.Enabled = true;
                            cbUnidadesSub.Enabled = true;
                            cbSNIP.Enabled = true;

                            rbSNIP.Enabled = true;
                            // codproducto = Convert.ToInt32(gvProdEstrategicos.GetRowValues(gvProdEstrategicos.FocusedRowIndex, "SPPRO$ID_PRODUCTO").ToString());
                            if (Session["tiposub"].ToString() == "subes")
                            {
                                lblsubso.Text = "subproducto vinculado a Metas PGG";
                                codproducto = Convert.ToInt32(gvProdEstrategicos.GetRowValues(gvProdEstrategicos.FocusedRowIndex, "SPPRO$ID_PRODUCTO").ToString());
                                cargaSubEstrategicos(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), codproducto);
                            }
                            else if (Session["tiposub"].ToString() == "subins")
                            {
                                lblsubso.Text = "subproducto institucional";
                                codproducto = Convert.ToInt32(gvProdInsto.GetRowValues(gvProdInsto.FocusedRowIndex, "SPPRO$ID_PRODUCTO").ToString());
                                cargaSubInstos(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), codproducto);
                            }

                            MultiView1.ActiveViewIndex = 6;
                        }
                        //}

                    }


                }
                else
                {
                    mensaje = "Su perfil de usuario no tiene autorización para eliminar este registro";
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
                }
            }
            else
            {

                if (rbSNIP.SelectedIndex == 1)
                {
                    if (cbSNIP.SelectedIndex == -1)
                    {
                        mensaje = "Esta vinculado el subproducto a un SNIP, seleccione el proyecto por favor";
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
                        cbSNIP.Focus();
                    }
                    else
                    {
                        if (Convert.ToInt32(Session["tiposubopera"]) == 20)
                        {
                            sql = "INSERT INTO SCHE$SIPLAN20.SP20$SUB_PRODUCTO (SPPSUB$SNIP, SPPSUB$ID_PRODUCTO, SPPSUB$FECHA_INSERTA, SPPSUB$PROPIETARIO) VALUES (" + cbSNIP.Value + ", " + codproducto + ", 'INSERT = '||TO_CHAR(SYSDATE,'DD/MM/YYYY HH24:MI')||' '||'" + Session["USUARIO"].ToString() + "', '" + Session["USUARIO"].ToString() + "')";

                        }

                        else if (Convert.ToInt32(Session["tiposubopera"]) == 21)
                        {

                            sql = "UPDATE SCHE$SIPLAN20.SP20$SUB_PRODUCTO SET  SPPSUB$DESCRIPCION = NULL, SPPSUB$ID_MEDIDA = NULL, SPPSUB$SNIP = " + cbSNIP.Value + ",  SPPSUB$FECHA_UPDATE = 'UPDATE = '||TO_CHAR(SYSDATE,'DD/MM/YYYY HH24:MI')||' '||'" + Session["USUARIO"].ToString() + "' WHERE SPPSUB$ID_SUBPRODUCTO = " + codSuProducto + " AND SPPSUB$ID_PRODUCTO = " + codproducto + " AND SPPSUB$RESTRICTIVA= 'N'";

                        }

                        estado = dao.comando(sql);

                        if (estado == 0)
                        {
                            mensaje = dao.mensaje;
                            Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
                            lblPilarRes.Text = "";
                            lblResub2.Text = "";
                            lblProduEstra2.Text = "";
                            txtSubproducto.Text = "";
                            cargaUnidadesSub();
                            cbUnidadesSub.SelectedIndex = -1;
                            rbSNIP.SelectedIndex = 1;
                            PanSNIP.Style.Add("display", "none");
                            cargaSNIP(Convert.ToInt32(Session["insto"]));
                            cbSNIP.SelectedIndex = -1;
                            txtSubproducto.Enabled = true;
                            cbUnidadesSub.Enabled = true;
                            cbSNIP.Enabled = true;

                            rbSNIP.Enabled = true;
                            if (Session["tiposub"].ToString() == "subes")
                            {
                                lblsubso.Text = "subproducto vinculado a Metas PGG";
                                codproducto = Convert.ToInt32(gvProdEstrategicos.GetRowValues(gvProdEstrategicos.FocusedRowIndex, "SPPRO$ID_PRODUCTO").ToString());
                                cargaSubEstrategicos(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), codproducto);
                            }
                            else if (Session["tiposub"].ToString() == "subins")
                            {
                                lblsubso.Text = "subproducto institucional";
                                codproducto = Convert.ToInt32(gvProdInsto.GetRowValues(gvProdInsto.FocusedRowIndex, "SPPRO$ID_PRODUCTO").ToString());
                                cargaSubInstos(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), codproducto);
                            }
                            MultiView1.ActiveViewIndex = 6;
                        }

                        else
                        {

                            if (Convert.ToInt32(aspCod.Value) == -1 && Convert.ToInt32(Session["tiposubopera"]) == 20)
                            {
                                if (Session["POLITICASSUB"] != null)
                                {                                  
                                    sql = "SELECT MAX(SPPSUB$ID_SUBPRODUCTO) ID  FROM SCHE$SIPLAN20.SP20$SUB_PRODUCTO WHERE SPPSUB$RESTRICTIVA = 'N' AND SPPSUB$ID_PRODUCTO = "+ codproducto;
                                    estado = dao.consulta(sql);
                                    if (estado == 1)
                                    {
                                        tabla = dao.tabla;
                                        if (tabla.Rows.Count > 0)
                                        {
                                            codSuProducto = Convert.ToInt32(tabla.Rows[0]["ID"]);
                                            tabla.Clear();
                                            tabla = (DataTable)Session["POLITICASSUB"];
                                            if (tabla.Rows.Count > 0)
                                            {
                                                for (int i = 0; i < tabla.Rows.Count; i++)
                                                {
                                                    sql= "INSERT INTO SCHE$SIPLAN20.SP20$POLITICA_SUBPRODUCTO (SPS$POLITICA_LINEAMIENTO, SPS$SUBPRODCUTO, SPS$FECHA_INSERTA) VALUES (" + tabla.Rows[i]["SPS$ID"] +","+codSuProducto+ ",'INSERTA = ' || TO_CHAR(SYSDATE, 'DD/MM/YYYY HH24:MI') || ' ' || '" + Session["USUARIO"].ToString() + "')";
                                                    estado = dao.comando(sql);
                                                    if (estado == 0)
                                                        break;
                                                }
                                                //GRABAMunicipios
                                            }


                                        }


                                    }
                                }

                                if (Convert.ToInt32(cbMunosPriorizados.Value) == 1)
                                {
                                    sql = "SELECT MAX(SPPSUB$ID_SUBPRODUCTO) ID  FROM SCHE$SIPLAN20.SP20$SUB_PRODUCTO WHERE SPPSUB$RESTRICTIVA = 'N' AND SPPSUB$ID_PRODUCTO = " + codproducto;
                                    estado = dao.consulta(sql);
                                    if (estado == 1)
                                    {
                                        tabla = dao.tabla;
                                        if (tabla.Rows.Count > 0)
                                        {
                                            codSuProducto = Convert.ToInt32(tabla.Rows[0]["ID"]);
                                            grabaMunicipios(codSuProducto);
                                        }


                                    }
                                }
                            }

                            if (estado == 0)
                            {
                                mensaje = dao.mensaje;
                                Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
                                MultiView1.ActiveViewIndex = 6;
                            }

                            else
                            {
                                mensaje = "Subproducto registrado correctamene";
                                Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',1);", true);
                                lblPilarRes.Text = "";
                                lblResub2.Text = "";
                                lblProduEstra2.Text = "";

                                txtSubproducto.Text = "";
                                cargaUnidadesSub();
                                cbUnidadesSub.SelectedIndex = -1;
                                rbSNIP.SelectedIndex = 1;
                                PanSNIP.Style.Add("display", "none");
                                cargaSNIP(Convert.ToInt32(Session["insto"]));
                                cbSNIP.SelectedIndex = -1;
                                txtSubproducto.Enabled = true;
                                cbUnidadesSub.Enabled = true;
                                cbSNIP.Enabled = true;

                                rbSNIP.Enabled = true;
                                if (Session["tiposub"].ToString() == "subes")
                                {
                                    lblsubso.Text = "subproducto vinculado a Metas PGG";
                                    codproducto = Convert.ToInt32(gvProdEstrategicos.GetRowValues(gvProdEstrategicos.FocusedRowIndex, "SPPRO$ID_PRODUCTO").ToString());
                                    cargaSubEstrategicos(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), codproducto);
                                }
                                else if (Session["tiposub"].ToString() == "subins")
                                {
                                    lblsubso.Text = "subproducto institucional";
                                    codproducto = Convert.ToInt32(gvProdInsto.GetRowValues(gvProdInsto.FocusedRowIndex, "SPPRO$ID_PRODUCTO").ToString());
                                    cargaSubInstos(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), codproducto);
                                }
                                MultiView1.ActiveViewIndex = 6;
                            }
                            
                        }
                    }

                }

                if (txtSubproducto.Text == "")
                {
                    mensaje = "La descripción de subproducto es necesaria";
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
                    txtSubproducto.Focus();
                }

                else if (rbSNIP.SelectedIndex == -1)
                {
                    mensaje = "Conteste a la pregunta ¿Necesita vincular el subproducto a un proyecto SNIP?";
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
                    rbSNIP.Focus();
                }


                else if (rbSNIP.SelectedIndex == 0 && cbMunosPriorizados.SelectedIndex == 1 && gvTerritorio.Selection.Count == 0)
                {

                    mensaje = "Si el subproducto no esta vinculado a un proyecto SNIP, es obligatorio priorizar territorio, debe selecionar al menos un municipio en el listado de abajo";
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
                    gvTerritorio.Focus();
                }

                else if (cbUnidadesSub.SelectedIndex == -1)
                {
                    mensaje = "Seleccione la unidad de medida del subproducto";
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
                    cbUnidadesSub.Focus();
                }
                else
                {
                    if (Convert.ToInt32(Session["tiposubopera"]) == 20)
                    {
                        sql = "INSERT INTO SCHE$SIPLAN20.SP20$SUB_PRODUCTO (SPPSUB$DESCRIPCION, SPPSUB$ID_MEDIDA, SPPSUB$ID_PRODUCTO, SPPSUB$FECHA_INSERTA, SPPSUB$PROPIETARIO) VALUES ('" + txtSubproducto.Text + "', " + cbUnidadesSub.Value + ", " + codproducto + ", 'INSERT = '||TO_CHAR(SYSDATE,'DD/MM/YYYY HH24:MI')||' '||'" + Session["USUARIO"].ToString() + "', '" + Session["USUARIO"].ToString() + "')";

                    }
                    else if (Convert.ToInt32(Session["tiposubopera"]) == 21)
                    {
                        if (Session["ROL"].ToString() != "ENTIDAD")
                        //if ((gvSubproductos.GetRowValues(gvSubproductos.FocusedRowIndex, "SPPSUB$PROPIETARIO").ToString() == Session["USUARIO"].ToString()) || Session["ROL"].ToString() == "ADMIN")
                        {
                            sql = "UPDATE SCHE$SIPLAN20.SP20$SUB_PRODUCTO SET SPPSUB$DESCRIPCION = '" + txtSubproducto.Text + "', SPPSUB$ID_MEDIDA =  " + Convert.ToInt32(cbUnidadesSub.Value) + ", SPPSUB$SNIP = NULL, SPPSUB$FECHA_UPDATE = 'UPDATE = '||TO_CHAR(SYSDATE,'DD/MM/YYYY HH24:MI')||' '||'" + Session["USUARIO"].ToString() + "' WHERE SPPSUB$ID_SUBPRODUCTO = " + codSuProducto + " AND SPPSUB$ID_PRODUCTO = " + codproducto + " AND SPPSUB$RESTRICTIVA= 'N'";
                        }
                        else
                        {
                            mensaje = "Su perfil de usuario no tiene autorización para editar este registro";
                            Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
                        }

                    }

                    estado = dao.comando(sql);
                    if (estado == 0)
                    {
                        mensaje = dao.mensaje;
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
                        lblPilarRes.Text = "";
                        lblResub2.Text = "";
                        lblProduEstra2.Text = "";

                        txtSubproducto.Text = "";
                        cargaUnidadesSub();
                        cbUnidadesSub.SelectedIndex = -1;
                        rbSNIP.SelectedIndex = 1;
                        PanSNIP.Style.Add("display", "none");
                        cargaSNIP(Convert.ToInt32(Session["insto"]));
                        cbSNIP.SelectedIndex = -1;
                        txtSubproducto.Enabled = true;
                        cbUnidadesSub.Enabled = true;
                        cbSNIP.Enabled = true;

                        rbSNIP.Enabled = true;
                        if (Session["tiposub"].ToString() == "subes")
                        {
                            lblsubso.Text = "subproducto vinculado a Metas PGG";
                            codproducto = Convert.ToInt32(gvProdEstrategicos.GetRowValues(gvProdEstrategicos.FocusedRowIndex, "SPPRO$ID_PRODUCTO").ToString());
                            cargaSubEstrategicos(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), codproducto);
                        }
                        else if (Session["tiposub"].ToString() == "subins")
                        {
                            lblsubso.Text = "subproducto institucional";
                            codproducto = Convert.ToInt32(gvProdInsto.GetRowValues(gvProdInsto.FocusedRowIndex, "SPPRO$ID_PRODUCTO").ToString());
                            cargaSubInstos(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), codproducto);
                        }
                        MultiView1.ActiveViewIndex = 6;
                    }

                    else
                    {

                        if (Convert.ToInt32(aspCod.Value) == -1 && Convert.ToInt32(Session["tiposubopera"]) == 20)
                        {
                            if (Session["POLITICASSUB"] != null)
                            {
                                sql = "SELECT MAX(SPPSUB$ID_SUBPRODUCTO) ID  FROM SCHE$SIPLAN20.SP20$SUB_PRODUCTO WHERE SPPSUB$RESTRICTIVA = 'N' AND SPPSUB$ID_PRODUCTO = " + codproducto;
                                estado = dao.consulta(sql);
                                if (estado == 1)
                                {
                                    tabla = dao.tabla;
                                    if (tabla.Rows.Count > 0)
                                    {
                                        codSuProducto = Convert.ToInt32(tabla.Rows[0]["ID"]);
                                        tabla.Clear();
                                        tabla = (DataTable)Session["POLITICASSUB"];
                                        if (tabla.Rows.Count > 0)
                                        {
                                            for (int i = 0; i < tabla.Rows.Count; i++)
                                            {
                                                sql = "INSERT INTO SCHE$SIPLAN20.SP20$POLITICA_SUBPRODUCTO (SPS$POLITICA_LINEAMIENTO, SPS$SUBPRODCUTO, SPS$FECHA_INSERTA) VALUES (" + tabla.Rows[i]["SPS$ID"] + "," + codSuProducto + ",'INSERTA = ' || TO_CHAR(SYSDATE, 'DD/MM/YYYY HH24:MI') || ' ' || '" + Session["USUARIO"].ToString() + "')";
                                                estado = dao.comando(sql);
                                                if (estado == 0)
                                                    break;
                                            }
                                        }
                                    }
                                }
                            }

                            if (Convert.ToInt32(cbMunosPriorizados.Value) == 1)
                            {
                                sql = "SELECT MAX(SPPSUB$ID_SUBPRODUCTO) ID  FROM SCHE$SIPLAN20.SP20$SUB_PRODUCTO WHERE SPPSUB$RESTRICTIVA = 'N' AND SPPSUB$ID_PRODUCTO = " + codproducto;
                                estado = dao.consulta(sql);
                                if (estado == 1)
                                {
                                    tabla = dao.tabla;
                                    if (tabla.Rows.Count > 0)
                                    {
                                        codSuProducto = Convert.ToInt32(tabla.Rows[0]["ID"]);
                                        grabaMunicipios(codSuProducto);
                                    }


                                }
                            }
                        }
                        else if (Convert.ToInt32(aspCod.Value) != -1 && Convert.ToInt32(Session["tiposubopera"]) == 21)
                        {
                            if (Session["POLITICASSUB"] != null)
                            {


                                codSuProducto = Convert.ToInt32(aspCod.Value);
                                tabla.Clear();
                                tabla = (DataTable)Session["POLITICASSUB"];
                                if (tabla.Rows.Count > 0)
                                {
                                    for (int i = 0; i < tabla.Rows.Count; i++)
                                    {
                                        busca = buscaPoliticasub(Convert.ToInt32(tabla.Rows[i]["SPS$ID"]), codSuProducto);
                                        if (busca == -1)
                                            break;
                                        else if (busca == 0)
                                        {
                                            sql = "INSERT INTO SCHE$SIPLAN20.SP20$POLITICA_SUBPRODUCTO (SPS$POLITICA_LINEAMIENTO, SPS$SUBPRODCUTO, SPS$FECHA_INSERTA) VALUES (" + tabla.Rows[i]["SPS$ID"] + "," + codSuProducto + ",'INSERTA = ' || TO_CHAR(SYSDATE, 'DD/MM/YYYY HH24:MI') || ' ' || '" + Session["USUARIO"].ToString() + "')";
                                            estado = dao.comando(sql);
                                            if (estado == 0)
                                                break;
                                        }

                                    }
                                }

                            }

                            if (Convert.ToInt32(cbMunosPriorizados.Value) == 1)
                            {
                                codproducto = Convert.ToInt32(aspCod.Value);
                                grabaMunicipios(codSuProducto);
                            }
                        }


                        if (estado == 0)
                        {
                            mensaje = dao.mensaje;
                            Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
                            MultiView1.ActiveViewIndex = 6;
                        }
                        else
                        {
                            mensaje = "Subproducto registrado correctamene";
                            Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',1);", true);
                            lblPilarRes.Text = "";
                            lblResub2.Text = "";
                            lblProduEstra2.Text = "";

                            txtSubproducto.Text = "";
                            cargaUnidadesSub();
                            cbUnidadesSub.SelectedIndex = -1;
                            rbSNIP.SelectedIndex = 1;
                            PanSNIP.Style.Add("display", "none");
                            cargaSNIP(Convert.ToInt32(Session["insto"]));
                            cbSNIP.SelectedIndex = -1;
                            txtSubproducto.Enabled = true;
                            cbUnidadesSub.Enabled = true;
                            cbSNIP.Enabled = true;

                            rbSNIP.Enabled = true;
                            if (Session["tiposub"].ToString() == "subes")
                            {
                                lblsubso.Text = "subproducto vinculado a Metas PGG";
                                codproducto = Convert.ToInt32(gvProdEstrategicos.GetRowValues(gvProdEstrategicos.FocusedRowIndex, "SPPRO$ID_PRODUCTO").ToString());
                                cargaSubEstrategicos(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), codproducto);
                            }
                            else if (Session["tiposub"].ToString() == "subins")
                            {
                                lblsubso.Text = "subproducto institucional";
                                codproducto = Convert.ToInt32(gvProdInsto.GetRowValues(gvProdInsto.FocusedRowIndex, "SPPRO$ID_PRODUCTO").ToString());
                                cargaSubInstos(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]), codproducto);
                            }
                            MultiView1.ActiveViewIndex = 6;
                        }

                    }
                }

            }


        }

        protected void btnRegresa_Click(object sender, EventArgs e)
        {
            if (Session["tiposub"].ToString() == "subes")
            {
                ProdEstrategicos.Style.Add("display", "block");
                ProdInsitucionales.Style.Add("display", "none");
            }

            if (Session["tiposub"].ToString() == "subins")
            {
                ProdEstrategicos.Style.Add("display", "none");
                ProdInsitucionales.Style.Add("display", "block");
            }

            MultiView1.ActiveViewIndex = 4;
        }

        protected void btnEditSub_Click(object sender, EventArgs e)
        {
            int nivel, codSub;
            DataTable temp1 = new DataTable();
            //Session["tiposub"] = "subes";
            Session["tiposubopera"] = 21;

            




            if (gvSubproductos.FocusedRowIndex != -1)
            {

                
                nivel = gvSubproductos.GetRowLevel(gvSubproductos.FocusedRowIndex);
                if (Session["tiposub"].ToString() == "subins")
                {
                    lblPilarRes.Visible = false;
                    lblsubso.Text = "subproducto institucional";
                }
                else if (Session["tiposub"].ToString() == "subes")
                {

                    lblsubso.Text = "subproducto vinculado a Resultado Estratégico (RE)";
                }



                lblProduEstra2.Text = "Producto: " + gvSubproductos.GetRowValues(gvSubproductos.FocusedRowIndex, "PRODUCTO").ToString();
                lblSuproducto.Visible = true;
                lblSuproducto.Text = "Subproducto: " + gvSubproductos.GetRowValues(gvSubproductos.FocusedRowIndex, "SUBPRODUCTO").ToString();
                lblProducto.Style.Add("color", "#2d572c");

                codSub = Convert.ToInt32(gvSubproductos.GetRowValues(gvSubproductos.FocusedRowIndex, "SPPSUB$ID_SUBPRODUCTO").ToString());

                if (Convert.ToInt32(hfNaturaleza.Value) == 0)
                {
                    cbMunosPriorizados.Enabled = false;
                    cbMunosPriorizados.SelectedIndex = 1;
                    munoPriorizados.Attributes.Add("style", "display:block");


                }
                else
                {
                    cbMunosPriorizados.Enabled = false;
                    cbMunosPriorizados.SelectedIndex = 0;
                    munoPriorizados.Attributes.Add("style", "display:none");                 
                }




                if (nivel == 0)
                {
                    //if ((gvSubproductos.GetRowValues(gvSubproductos.FocusedRowIndex, "SPPSUB$PROPIETARIO").ToString() == Session["USUARIO"].ToString()) || Session["ROL"].ToString() == "ADMIN")
                    if (Session["ROL"].ToString() != "ENTIDAD")
                    {
                        sql = "SELECT * FROM SCHE$SIPLAN20.SP20$SUB_PRODUCTO WHERE SPPSUB$ID_SUBPRODUCTO = " + codSub;
                        estado = dao.consulta(sql);
                        if (estado == 0)
                        {
                            mensaje = dao.mensaje;
                            Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);

                        }
                        else
                        {
                            temp1 = dao.tabla;
                            if (temp1.Rows.Count > 0)
                            {
                                if (temp1.Rows[0]["SPPSUB$SNIP"] != DBNull.Value)
                                {

                                    cargaSNIP(Convert.ToInt32(Session["insto"]));
                                    cbSNIP.Value = temp1.Rows[0]["SPPSUB$SNIP"].ToString();
                                    PanSNIP.Style.Add("display", "block");
                                    rbSNIP.SelectedIndex = 1;
                                    txtSubproducto.Text = "";
                                    cargaUnidadesSub();
                                    cbUnidadesSub.SelectedIndex = -1;
                                    cbMunosPriorizados.Enabled = false;
                                    cbMunosPriorizados.SelectedIndex = 0;
                                    hfSNIP.Value = temp1.Rows[0]["SPPSUB$SNIP"].ToString();
                                }
                                else
                                {
                                    hfSNIP.Value = "-1";
                                    txtSubproducto.Text = temp1.Rows[0]["SPPSUB$DESCRIPCION"].ToString();
                                    cargaUnidadesSub();
                                    cbUnidadesSub.Value = temp1.Rows[0]["SPPSUB$ID_MEDIDA"].ToString();
                                    PanSNIP.Style.Add("display", "none");
                                    rbSNIP.SelectedIndex = 0;
                                    cargaSNIP(Convert.ToInt32(Session["insto"]));
                                    cbSNIP.SelectedIndex = 1;
                                    cbMunosPriorizados.Enabled = false;
                                    //aqui poner la logica del municipio
                                    cbMunosPriorizados.SelectedIndex = 1;
                                    sql = "SELECT COUNT(*) CONTEO FROM SCHE$SIPLAN20.SP20$SUB_MUNOS WHERE SPSM$ID_SUB = " + codSub + " AND SPSM$RESTRICTIVA = 'N'";
                                    estado = dao.consulta(sql);
                                    if (estado == 1)
                                    {
                                        temp1 = dao.tabla;

                                        if (Convert.ToInt32(hfNaturaleza.Value) == 1)
                                        {
                                            munoPriorizados.Attributes.Add("style", "display:none");
                                            cbMunosPriorizados.Enabled = false;
                                            cbMunosPriorizados.SelectedIndex = 0;
                                        }

                                        else
                                        {
                                            if (temp1.Rows.Count > 0)
                                            {
                                                if (Convert.ToInt32(temp1.Rows[0]["CONTEO"]) > 0)
                                                {
                                                    lblSubMunos.Text = temp1.Rows[0]["CONTEO"].ToString();
                                                    munoPriorizados.Attributes.Add("style", "display:block");
                                                    cbMunosPriorizados.SelectedIndex = 1;
                                                    cargaMunicipios(codSub);
                                                }
                                                else
                                                {
                                                    if (Convert.ToInt32(hfNaturaleza.Value) == 0)
                                                    {
                                                        lblSubMunos.Text = "0";
                                                        munoPriorizados.Attributes.Add("style", "display:block");
                                                        cbMunosPriorizados.SelectedIndex = 1;
                                                        cbMunosPriorizados.Enabled = false;
                                                        aspCod.Value = "-1";
                                                        cbMunosPriorizados_SelectedIndexChanged(sender, e);
                                                        gvTerritorio.Selection.UnselectAll();

                                                    }
                                                    else
                                                    {
                                                        lblSubMunos.Text = "0";
                                                        munoPriorizados.Attributes.Add("style", "display:none");
                                                        cbMunosPriorizados.SelectedIndex = 0;
                                                        cbMunosPriorizados.Enabled = false;
                                                        aspCod.Value = "-1";
                                                        cbMunosPriorizados_SelectedIndexChanged(sender, e);
                                                        gvTerritorio.Selection.UnselectAll();

                                                    }

                                                       
                                                }
                                            }
                                            else
                                            {
                                                lblSubMunos.Text = "0";
                                                munoPriorizados.Attributes.Add("style", "display:none");
                                                cbMunosPriorizados.Enabled = false;
                                                gvTerritorio.Selection.UnselectAll();
                                                cbMunosPriorizados.SelectedIndex = 0;


                                            }

                                        }
                                    }
                                    else
                                    {
                                        lblSubMunos.Text = "0";
                                        mensaje = dao.mensaje;
                                        cbMunosPriorizados.Enabled = false;
                                        Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
                                        munoPriorizados.Attributes.Add("style", "display:none");
                                        gvTerritorio.Selection.UnselectAll();

                                    }





                                }
                                txtSubproducto.Enabled = true;
                                cbUnidadesSub.Enabled = true;
                                cbSNIP.Enabled = true;

                                rbSNIP.Enabled = true;
                                btnSubProducto.CssClass = "btn btn-success";
                                btnSubProducto.Text = "Editar subproducto";
                                MultiView1.ActiveViewIndex = 7;
                                aspCod.Value = codSub.ToString();
                            }

                            sql = "SELECT COUNT(*) CONTEO FROM SCHE$SIPLAN20.SP20$POLITICA_SUBPRODUCTO WHERE SPS$RESTRICTIVA = 'N' AND SPS$SUBPRODCUTO = " + codSub;
                            estado = dao.consulta(sql);
                            if (estado == 1)
                            {
                                temp1 = dao.tabla;
                                if (temp1.Rows.Count > 0)
                                {
                                    numPoliticas.Text = temp1.Rows[0]["CONTEO"].ToString();
                                    //cbMunosPriorizados.SelectedIndex = 1;
                                }

                                else
                                {
                                    numPoliticas.Text = "0";
                                    //cbMunosPriorizados.SelectedIndex = -1;
                                }

                            }

                            else
                            {
                                mensaje = dao.mensaje;
                                Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
                            }

                            sql = "SELECT COUNT(*) CONTEO FROM SCHE$SIPLAN20.SP20$SUB_MUNOS WHERE SPSM$ID_SUB = " + codSub + " AND SPSM$RESTRICTIVA = 'N'";
                            estado = dao.consulta(sql);
                            if (estado == 1)
                            {
                                temp1 = dao.tabla;
                                if (temp1.Rows.Count > 0)
                                {
                                    if (Convert.ToInt32(temp1.Rows[0]["CONTEO"]) > 0)
                                    {
                                        lblSubMunos.Text = temp1.Rows[0]["CONTEO"].ToString();
                                        munoPriorizados.Attributes.Add("style", "display:block");
                                        cbMunosPriorizados.SelectedIndex = 1;
                                        cargaMunicipios(codSub);
                                    }
                                    else if (Convert.ToInt32(hfSNIP.Value) > 0)
                                    {
                                        lblSubMunos.Text = "0";
                                        munoPriorizados.Attributes.Add("style", "display:none");
                                        cbMunosPriorizados.SelectedIndex = 0;
                                        aspCod.Value = "-1";
                                        cbMunosPriorizados.Enabled = false;
                                        gvTerritorio.Selection.UnselectAll();
                                    }
                                    else
                                    {
                                        if (Convert.ToInt32(hfNaturaleza.Value) == 0)
                                        {
                                            lblSubMunos.Text = "0";
                                            munoPriorizados.Attributes.Add("style", "display:block");
                                            cbMunosPriorizados.SelectedIndex = 1;
                                            cbMunosPriorizados.Enabled = false;
                                            cargaMunicipios(-1);
                                        }
                                        else
                                        {
                                            lblSubMunos.Text = "0";
                                            munoPriorizados.Attributes.Add("style", "display:none");
                                            cbMunosPriorizados.SelectedIndex = 0;
                                            cbMunosPriorizados.Enabled = false;
                                            cargaMunicipios(-1);
                                        }
                                            

                                    }
                                }
                                else
                                {
                                    if (Convert.ToInt32(hfNaturaleza.Value) == 0)
                                    {
                                        lblSubMunos.Text = "0";
                                        munoPriorizados.Attributes.Add("style", "display:block");
                                        gvTerritorio.Selection.UnselectAll();
                                        cbMunosPriorizados.SelectedIndex = 1;
                                    }
                                    else
                                    {
                                        lblSubMunos.Text = "0";
                                        munoPriorizados.Attributes.Add("style", "display:none");
                                        gvTerritorio.Selection.UnselectAll();
                                        cbMunosPriorizados.SelectedIndex = 0;

                                    }
                                       
                                }
                            }
                            else
                            {
                                lblSubMunos.Text = "0";
                                mensaje = dao.mensaje;
                                Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
                                munoPriorizados.Attributes.Add("style", "display:none");
                                gvTerritorio.Selection.UnselectAll();

                            }


                        }
                    }

                    else
                    {
                        mensaje = "Su perfil de usuario no tiene autorización para editar este registro";
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
                    }


                }

                else
                {
                    mensaje = "Seleccione una fila que contenga un subproducto";
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
                }

            }


            else
            {
                mensaje = "Seleccione el subproducto a editar";
                Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
            }


        }

        protected void btnDelSub_Click(object sender, EventArgs e)
        {
            int nivel, codSub;
            DataTable temp1 = new DataTable();
            //Session["tiposub"] = "subes";
            Session["tiposubopera"] = 22;
            if (gvSubproductos.FocusedRowIndex != -1)
            {
                nivel = gvSubproductos.GetRowLevel(gvSubproductos.FocusedRowIndex);
                if (Session["tiposub"].ToString() == "subins")
                {
                    lblPilarRes.Visible = false;
                    lblsubso.Text = "subproducto institucional";
                    lblResub2.Text = "Resultado: " + gvSubproductos.GetRowValues(gvSubproductos.FocusedRowIndex, "RESULTADO").ToString();

                }

                else if (Session["tiposub"].ToString() == "subes")
                {

                    lblsubso.Text = "subproducto vinculado a Resultado Estratégico (RE)";
                }


                lblProduEstra2.Text = "Producto: " + gvSubproductos.GetRowValues(gvSubproductos.FocusedRowIndex, "PRODUCTO").ToString();
                lblSuproducto.Visible = true;
                lblSuproducto.Text = "Esta por eliminar el subproducto " + gvSubproductos.GetRowValues(gvSubproductos.FocusedRowIndex, "SUBPRODUCTO").ToString() + " confirme su operación, presionando el botón 'Eliminar producto'";
                lblSuproducto.Style.Add("color", "red");

                codSub = Convert.ToInt32(gvSubproductos.GetRowValues(gvSubproductos.FocusedRowIndex, "SPPSUB$ID_SUBPRODUCTO").ToString());
                if (nivel == 0)
                {
                    //if ((gvSubproductos.GetRowValues(gvSubproductos.FocusedRowIndex, "SPPSUB$PROPIETARIO").ToString() == Session["USUARIO"].ToString()) || Session["ROL"].ToString() == "ADMIN")
                    if (Session["ROL"].ToString() != "ENTIDAD")
                    {
                        sql = "SELECT * FROM SCHE$SIPLAN20.SP20$SUB_PRODUCTO WHERE SPPSUB$ID_SUBPRODUCTO = " + codSub;
                        estado = dao.consulta(sql);
                        if (estado == 0)
                        {
                            mensaje = dao.mensaje;
                            Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);

                        }
                        else
                        {
                            temp1 = dao.tabla;
                            if (temp1.Rows.Count > 0)
                            {
                                if (temp1.Rows[0]["SPPSUB$SNIP"] != DBNull.Value)
                                {
                                    cargaSNIP(Convert.ToInt32(Session["insto"]));
                                    cbSNIP.Value = temp1.Rows[0]["SPPSUB$SNIP"].ToString();
                                    PanSNIP.Style.Add("display", "block");
                                    rbSNIP.SelectedIndex = 1;
                                    txtSubproducto.Text = "";
                                    cargaUnidadesSub();
                                    cbUnidadesSub.SelectedIndex = -1;
                                }
                                else
                                {
                                    txtSubproducto.Text = temp1.Rows[0]["SPPSUB$DESCRIPCION"].ToString();
                                    cargaUnidadesSub();
                                    cbUnidadesSub.Value = temp1.Rows[0]["SPPSUB$ID_MEDIDA"].ToString();
                                    PanSNIP.Style.Add("display", "none");
                                    rbSNIP.SelectedIndex = 0;
                                    cargaSNIP(Convert.ToInt32(Session["insto"]));
                                    cbSNIP.SelectedIndex = -1;

                                }
                                txtSubproducto.Enabled = false;
                                cbUnidadesSub.Enabled = false;
                                cbSNIP.Enabled = false;
                                rbSNIP.Enabled = false;
                                btnSubProducto.CssClass = "btn btn-danger";
                                btnSubProducto.Text = "Eliminar subproducto";
                                MultiView1.ActiveViewIndex = 7;
                            }
                        }
                    }

                    else
                    {
                        mensaje = "Su perfil de usuario no tiene autorización para editar este registro";
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
                    }


                }

                else
                {
                    mensaje = "Seleccione una fila que contenga un subproducto";
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
                }

            }


            else
            {
                mensaje = "Seleccione el subproducto a editar";
                Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
            }

        }

        protected void btnSubIns_Click(object sender, EventArgs e)
        {
            int nivel, codproducto;
            String producto, pilar, resultado;
            DataTable tempo1 = new DataTable();
            bool responsable = false;



            if (rbProductos.SelectedIndex == 0)
            {
                if (gvProdInsto.FocusedRowIndex != -1)
                {


                    nivel = gvProdInsto.GetRowLevel(gvProdInsto.FocusedRowIndex);
                    producto = gvProdInsto.GetRowValues(gvProdInsto.FocusedRowIndex, "PRODUCTO").ToString();
                    hfNaturaleza.Value = gvProdInsto.GetRowValues(gvProdInsto.FocusedRowIndex, "SPPRO$ES_ADMINISTRATIVO").ToString();
                    codproducto = Convert.ToInt32(gvProdInsto.GetRowValues(gvProdInsto.FocusedRowIndex, "SPPRO$ID_PRODUCTO").ToString());
                    Session["pom"] = Convert.ToInt32(gvPOMInsto.GetRowValues(gvPOMInsto.FocusedRowIndex, "SPPO$ID_POM").ToString());
                    Session["insto"] = Convert.ToInt32(gvPOMInsto.GetRowValues(gvPOMInsto.FocusedRowIndex, "SPPO$ID_INSTITUCION").ToString());
                    Session["abierto"] = Convert.ToInt32(gvPOMInsto.GetRowValues(gvPOMInsto.FocusedRowIndex, "SPP$ABIERTO").ToString());

                    //responsable = buscaRespo_politica(Convert.ToInt32(Session["insto"]));
                    responsable = true;
                    if (responsable == true)
                    {
                        btnPoliticas.Visible = true;
                        politicas.Style.Add("display", "block");
                        btnPolitics.Visible = true;

                    }
                    else
                    {
                        btnPoliticas.Visible = false;
                        politicas.Style.Add("display", "none");
                        btnPolitics.Visible = false;
                    }

                    if (nivel == 0)
                    {




                        Session["tiposub"] = "subins";
                        sql = "SELECT * FROM SCHE$SIPLAN20.SPPSVT$SUBINSTITUCIONALV2 SE WHERE SE.SPRES$POM = " + Convert.ToInt32(Session["pom"]) + " AND SE.SPRES$INSTITUCION = " + Convert.ToInt32(Session["insto"]) + " AND SE.SPPRO$ID_PRODUCTO = " + codproducto;
                        estado = dao.consulta(sql);
                        if (estado == 0)
                        {
                            mensaje = dao.mensaje;
                            Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
                        }
                        else
                        {
                            lbltitulo.Text = "Gestión de subproductos para: " + producto;
                            lblPilarsub.Visible = false;


                            lblProdsub.Text = "Producto  institucional/PGG 2024-2028: " + producto;
                            tabla = dao.tabla;
                            //gvSubproductos.Columns.Clear();
                            gvSubproductos.DataSource = tabla;
                            gvSubproductos.DataBind();
                            gvSubproductos.ExpandAll();

                            if (Convert.ToInt32(Session["abierto"]) == 1)
                            {
                                btnNuevoSub.Enabled = false;
                                btnEditSub.Enabled = false;
                                btnDelSub.Enabled = false;
                            }
                            else
                            {
                                btnNuevoSub.Enabled = true;
                                btnEditSub.Enabled = true;
                                btnDelSub.Enabled = true;
                            }


                            MultiView1.ActiveViewIndex = 6;


                        }
                    }
                    else
                    {
                        mensaje = "Seleccione una fila la cual contenga un producto";
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
                    }




                }

                else
                {
                    mensaje = "Seleccione el producto al cual se van crear subproductos";
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
                }
            }
            else if (rbProductos.SelectedIndex == 1)
            {

                if (gvProdEstrategicos.FocusedRowIndex != -1)
                {
                    nivel = gvProdEstrategicos.GetRowLevel(gvProdEstrategicos.FocusedRowIndex);
                    producto = gvProdEstrategicos.GetRowValues(gvProdEstrategicos.FocusedRowIndex, "PRODUCTO").ToString();
                    hfNaturaleza.Value = gvProdEstrategicos.GetRowValues(gvProdEstrategicos.FocusedRowIndex, "SPPRO$ES_ADMINISTRATIVO").ToString();
                    codproducto = Convert.ToInt32(gvProdEstrategicos.GetRowValues(gvProdEstrategicos.FocusedRowIndex, "SPPRO$ID_PRODUCTO").ToString());
                    Session["pom"] = Convert.ToInt32(gvPOMInsto.GetRowValues(gvPOMInsto.FocusedRowIndex, "SPPO$ID_POM").ToString());
                    Session["insto"] = Convert.ToInt32(gvPOMInsto.GetRowValues(gvPOMInsto.FocusedRowIndex, "SPPO$ID_INSTITUCION").ToString());
                    Session["abierto"] = Convert.ToInt32(gvPOMInsto.GetRowValues(gvPOMInsto.FocusedRowIndex, "SPP$ABIERTO").ToString());

                    //responsable = buscaRespo_politica(Convert.ToInt32(Session["insto"]));
                    responsable = true;
                    if (responsable == true)
                    {
                        btnPoliticas.Visible = true;
                        politicas.Style.Add("display", "block");
                        btnPolitics.Visible = true;

                    }
                    else
                    {
                        btnPoliticas.Visible = false;
                        politicas.Style.Add("display", "none");
                        btnPolitics.Visible = false;
                    }

                    if (nivel == 0)
                    {

                        Session["tiposub"] = "subes";
                        sql = "SELECT * FROM SCHE$SIPLAN20.SPPSVST$RED SE WHERE SE.SPRES$POM = " + Convert.ToInt32(Session["pom"]) + " AND SE.SPRES$INSTITUCION = " + Convert.ToInt32(Session["insto"]) + " AND SE.SPPRO$ID_PRODUCTO = " + codproducto;
                        estado = dao.consulta(sql);
                        if (estado == 0)
                        {
                            mensaje = dao.mensaje;
                            Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
                        }
                        else
                        {
                            lbltitulo.Text = "Gestión de subproductos para : " + producto;
                            lblPilarsub.Visible = true;

                            lblProdsub.Text = "Producto vinculado a Resultado Estratégic (RE): " + producto;
                            tabla = dao.tabla;
                            //gvSubproductos.Columns.Clear();
                            //GridViewDataColumn c = new GridViewDataColumn();
                            //c.FieldName = "PILAR_PGG";
                            //c.VisibleIndex = 1;
                            //c.GroupIndex = 0;
                            //c.Visible = true;

                            //gvSubproductos.Columns.Add(c);
                            gvSubproductos.DataSource = tabla;
                            gvSubproductos.DataBind();
                            gvSubproductos.ExpandAll();

                            if (Convert.ToInt32(Session["abierto"]) == 1)
                            {
                                btnNuevoSub.Enabled = false;
                                btnEditSub.Enabled = false;
                                btnDelSub.Enabled = false;
                            }
                            else
                            {
                                btnNuevoSub.Enabled = true;
                                btnEditSub.Enabled = true;
                                btnDelSub.Enabled = true;
                            }

                            MultiView1.ActiveViewIndex = 6;


                        }
                    }
                    else
                    {
                        mensaje = "Seleccione una fila la cual contenga un producto";
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
                    }
                }

                else
                {
                    mensaje = "Seleccione el producto al cual se van crear subproductos";
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
                }

            }
        }

        protected void btnDocumento_Click(object sender, EventArgs e)
        {
            int pom, insto, periodo;
            DataTable poa = new DataTable();
            DataTable periodos = new DataTable();
            DataTable documentos = new DataTable();
            if (gvPOMInsto.FocusedRowIndex != -1)
            {
                Session["pom"] = Convert.ToInt32(gvPOMInsto.GetRowValues(gvPOMInsto.FocusedRowIndex, "SPPO$ID_POM").ToString());
                Session["insto"] = Convert.ToInt32(gvPOMInsto.GetRowValues(gvPOMInsto.FocusedRowIndex, "SPPO$ID_INSTITUCION").ToString());
                Session["abierto"] = Convert.ToInt32(gvPOMInsto.GetRowValues(gvPOMInsto.FocusedRowIndex, "SPP$ABIERTO").ToString());
                //sql = "SELECT SPPDOC$ID_DOCUMENTO, D.SPPDOC$POM, D.SPPDOC$INSTO, D.SPPDOC$TIPO, TD.SPPD$DESCRIPCION, TD.SPPD$SIGLAS, D.SPPDOC$DESCRIPCION, CASE WHEN D.SPPDOC$NOMARCHIVO IS NULL THEN 'NO' WHEN D.SPPDOC$NOMARCHIVO IS NOT NULL THEN 'SI' END AS ARCHIVO_ADJUNTO, D.SPPDOC$NOMARCHIVO, D.SPPDOC$RUTA, D.SPPDOC$PROPIETARIO FROM SCHE$SIPLAN20.SP20$POMDOCUMENTO D INNER JOIN SCHE$SIPLAN20.SPP20$TIPODOCUMENTO TD ON D.SPPDOC$TIPO = TD.SPPD$ID_DOCUMENTO AND D.SPPDOC$RESTRICTIVA = 'N' AND TD.SPPD$RESTRICTIVA = 'N' WHERE D.SPPDOC$POM = " + Convert.ToInt32(Session["pom"]) + " AND D.SPPDOC$INSTO = " + Convert.ToInt32(Session["insto"]) + " AND D.SPPDOC$RESTRICTIVA = 'N' AND TD.SPPD$SIGLAS IN('POM', 'PEI')";
                sql = "SELECT SPPDOC$ID_DOCUMENTO, SPPDOC$POM, SPPDOC$INSTO, SPPDOC$TIPO, CASE WHEN SPPDOC$TIPO = 2 THEN 'PLAN OPERATIVO ANUAL '||'- '||ANIO ELSE SPPD$DESCRIPCION END AS SPPD$DESCRIPCION, SPPD$SIGLAS, SPPDOC$DESCRIPCION, ARCHIVO_ADJUNTO, SPPDOC$NOMARCHIVO, SPPDOC$NOMARCHIVO, SPPDOC$RUTA, SPPDOC$PROPIETARIO, SPPDOC$POA FROM ( SELECT SPPDOC$ID_DOCUMENTO, D.SPPDOC$POM, D.SPPDOC$INSTO, D.SPPDOC$TIPO, TD.SPPD$DESCRIPCION, TD.SPPD$SIGLAS, D.SPPDOC$DESCRIPCION, (SELECT SPOA$ANIO FROM SCHE$SIPLAN20.SP20$POA WHERE SPOA$ID_POA = D.SPPDOC$POA AND SPOA$ID_POM = D.SPPDOC$POM AND SPOA$ID_INSTITUCION = D.SPPDOC$INSTO) ANIO,  CASE WHEN D.SPPDOC$NOMARCHIVO IS NULL THEN 'NO' WHEN D.SPPDOC$NOMARCHIVO IS NOT NULL THEN 'SI' END AS ARCHIVO_ADJUNTO, D.SPPDOC$NOMARCHIVO, D.SPPDOC$RUTA, D.SPPDOC$PROPIETARIO,  D.SPPDOC$POA FROM SCHE$SIPLAN20.SP20$POMDOCUMENTO D INNER JOIN SCHE$SIPLAN20.SPP20$TIPODOCUMENTO TD ON D.SPPDOC$TIPO = TD.SPPD$ID_DOCUMENTO AND D.SPPDOC$RESTRICTIVA = 'N' AND TD.SPPD$RESTRICTIVA = 'N' WHERE D.SPPDOC$POM = " + Convert.ToInt32(Session["pom"]) + " AND D.SPPDOC$INSTO = " + Convert.ToInt32(Session["insto"]) + " AND D.SPPDOC$RESTRICTIVA = 'N') ORDER BY SPPDOC$TIPO, SPPDOC$POA ASC ";
                estado = dao.consulta(sql);
                if (estado == 0)
                {
                    mensaje = dao.mensaje;
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
                }

                else
                {
                    documentos = dao.tabla;
                    pom = Convert.ToInt32(gvPOMInsto.GetRowValues(gvPOMInsto.FocusedRowIndex, "SPPO$ID_POM").ToString());
                    insto = Convert.ToInt32(gvPOMInsto.GetRowValues(gvPOMInsto.FocusedRowIndex, "SPPO$ID_INSTITUCION").ToString());
                    periodo = Convert.ToInt32(gvPOMInsto.GetRowValues(gvPOMInsto.FocusedRowIndex, "SPPO$ID_PERIODO").ToString());
                    sql = "SELECT P.SPP$ID_PERIODO, POA.SPOA$ID_POA, POA.SPOA$ANIO, POA.SPOA$ID_POM, POA.SPOA$ID_INSTITUCION FROM SCHE$SIPLAN20.SP20$POA POA INNER JOIN SCHE$SIPLAN20.SP20$POM POM ON POM.SPPO$ID_POM = POA.SPOA$ID_POM AND POM.SPPO$ID_INSTITUCION = POA.SPOA$ID_INSTITUCION AND POM.SPPO$RESTRICTIVA = 'N' AND POA.SPOA$RESTRICTIVA = 'N' ";
                    sql = sql + "INNER JOIN SCHE$SIPLAN20.SP20$PERIODO P ON P.SPP$ID_PERIODO = POM.SPPO$ID_PERIODO AND P.SPP$RESTRICTIVA = 'N' AND POM.SPPO$RESTRICTIVA = 'N' WHERE POA.SPOA$ID_POM = " + pom + " AND POA.SPOA$ID_INSTITUCION =" + insto + " AND P.SPP$ID_PERIODO = " + periodo;

                    estado = dao.consulta(sql);
                    if (estado != 0)
                    {
                        poa = dao.tabla;
                        if (poa.Rows.Count == 0)
                        {
                            sql = "SELECT * FROM SCHE$SIPLAN20.SP20$PERIODO P WHERE P.SPP$ID_PERIODO = " + periodo + " AND P.SPP$RESTRICTIVA = 'N'";
                            estado = dao.consulta(sql);
                            periodos = dao.tabla;
                            if (periodos.Rows.Count > 0)
                            {

                                for (int i = Convert.ToInt32(periodos.Rows[0]["SPP$INICIO"]); i <= Convert.ToInt32(periodos.Rows[0]["SPP$FINAL"]); i++)
                                {
                                    sql = "INSERT INTO SCHE$SIPLAN20.SP20$POA (SPOA$ANIO, SPOA$ID_POM, SPOA$ID_INSTITUCION, SPOA$FECHA_INSERT, SPOA$PROPIETARIO) VALUES (" + i + ", " + pom + ", " + insto + ", 'INSERT = '||TO_CHAR(SYSDATE,'DD/MM/YYYY HH24:MI')||' '||'" + Session["USUARIO"].ToString() + "', '" + Session["USUARIO"].ToString() + "' )";
                                    estado = dao.comando(sql);
                                    if (estado == 0)
                                    {

                                        break;
                                    }


                                }
                            }

                        }
                    }


                    if (Convert.ToInt32(Session["abierto"]) == 1)
                    {
                        btnNuevoInstrumento.Enabled = false;
                        btnAdjuntarArchivo.Enabled = false;
                        btnEliminarArchivo.Enabled = false;
                    }
                    else
                    {
                        btnNuevoInstrumento.Enabled = true;
                        btnAdjuntarArchivo.Enabled = true;
                        btnEliminarArchivo.Enabled = true;
                    }


                    gvDocumentos.DataSource = documentos;
                    gvDocumentos.DataBind();
                    MultiView1.ActiveViewIndex = 8;
                }
            }
        }

        protected void btnNuevoInstrumento_Click(object sender, EventArgs e)
        {
            Session["operainstrumento"] = "OI0";
            lblInstrumentoOpera.Text = "Agregar nuevo instrumento de planificación";
            cargaInstrumentofalta(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]));
            txtDescripcionInstrumento.Text = "";
            cbInstrumento.SelectedIndex = -1;
            panArchivo.Style.Add("display", "none");
            btnGrabaInstrumento.CssClass = "btn btn-primary";
            btnGrabaInstrumento.Text = "Nuevo instrumento";
            cbInstrumento.Enabled = true;
            popInstrumento.ShowOnPageLoad = true;

        }

        protected void cargaInstrumentofalta(int pom, int insto)
        {
            //sql = "SELECT D.SPPD$ID_DOCUMENTO, D.SPPD$DESCRIPCION FROM SCHE$SIPLAN20.SPP20$TIPODOCUMENTO D LEFT JOIN SCHE$SIPLAN20.SP20$POMDOCUMENTO PD ON PD.SPPDOC$TIPO = D.SPPD$ID_DOCUMENTO AND PD.SPPDOC$RESTRICTIVA = 'N' AND D.SPPD$RESTRICTIVA = 'N' AND PD.SPPDOC$POM = "+pom+" AND PD.SPPDOC$INSTO = "+insto+ " WHERE D.SPPD$RESTRICTIVA = 'N' AND D.SPPD$SIGLAS IN('POM', 'PEI')  AND PD.SPPDOC$TIPO IS NULL";
            DateTime fecha = DateTime.Now;
            int anio = fecha.Year;
            int inicio = 0;
            inicio = Convert.ToInt32(gvPOMInsto.GetRowValues(gvPOMInsto.FocusedRowIndex, "SPPO$INICIO").ToString());

            /*sql = "SELECT * FROM ( SELECT D.SPPD$ID_DOCUMENTO, D.SPPD$DESCRIPCION FROM SCHE$SIPLAN20.SPP20$TIPODOCUMENTO D LEFT JOIN SCHE$SIPLAN20.SP20$POMDOCUMENTO PD ON PD.SPPDOC$TIPO = D.SPPD$ID_DOCUMENTO AND PD.SPPDOC$RESTRICTIVA = 'N' AND D.SPPD$RESTRICTIVA = 'N' AND PD.SPPDOC$POM = " + pom + " AND PD.SPPDOC$INSTO = " + insto + " WHERE D.SPPD$RESTRICTIVA = 'N' AND D.SPPD$SIGLAS IN('POM', 'PEI')  AND PD.SPPDOC$TIPO IS NULL";
            sql = sql+" UNION SELECT POA.SPOA$ANIO SPPD$ID_DOCUMENTO, 'POA-' || POA.SPOA$ANIO SPPD$DESCRIPCION FROM SCHE$SIPLAN20.SP20$POA POA LEFT JOIN SCHE$SIPLAN20.SP20$POMDOCUMENTO PDOC ON PDOC.SPPDOC$POA = POA.SPOA$ID_POA AND PDOC.SPPDOC$POM = POA.SPOA$ID_POM AND PDOC.SPPDOC$INSTO = POA.SPOA$ID_INSTITUCION AND POA.SPOA$RESTRICTIVA = 'N' AND PDOC.SPPDOC$RESTRICTIVA = 'N' WHERE POA.SPOA$ID_POM = "+pom+" AND POA.SPOA$ID_INSTITUCION = "+insto+ "  AND PDOC.SPPDOC$POA IS NULL AND POA.SPOA$ANIO = "+anio+" )  ORDER BY SPPD$ID_DOCUMENTO";*/
            sql = "SELECT * FROM ( SELECT D.SPPD$ID_DOCUMENTO, D.SPPD$DESCRIPCION FROM SCHE$SIPLAN20.SPP20$TIPODOCUMENTO D LEFT JOIN SCHE$SIPLAN20.SP20$POMDOCUMENTO PD ON PD.SPPDOC$TIPO = D.SPPD$ID_DOCUMENTO AND PD.SPPDOC$RESTRICTIVA = 'N' AND D.SPPD$RESTRICTIVA = 'N' AND PD.SPPDOC$POM = " + pom + " AND PD.SPPDOC$INSTO = " + insto + " WHERE D.SPPD$RESTRICTIVA = 'N' AND D.SPPD$SIGLAS IN('POM', 'PEI')  AND PD.SPPDOC$TIPO IS NULL";
            sql = sql + " UNION SELECT POA.SPOA$ANIO SPPD$ID_DOCUMENTO, 'POA-' || POA.SPOA$ANIO SPPD$DESCRIPCION FROM SCHE$SIPLAN20.SP20$POA POA LEFT JOIN SCHE$SIPLAN20.SP20$POMDOCUMENTO PDOC ON PDOC.SPPDOC$POA = POA.SPOA$ID_POA AND PDOC.SPPDOC$POM = POA.SPOA$ID_POM AND PDOC.SPPDOC$INSTO = POA.SPOA$ID_INSTITUCION AND POA.SPOA$RESTRICTIVA = 'N' AND PDOC.SPPDOC$RESTRICTIVA = 'N' WHERE POA.SPOA$ID_POM = " + pom + " AND POA.SPOA$ID_INSTITUCION = " + insto + "  AND PDOC.SPPDOC$POA IS NULL AND POA.SPOA$ANIO = " + inicio + " )  ORDER BY SPPD$ID_DOCUMENTO";

            estado = dao.consulta(sql);
            if (estado == 0)
            {
                mensaje = dao.mensaje;
                Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
            }

            else
            {
                tabla = dao.tabla;
                cbInstrumento.DataSource = tabla;
                cbInstrumento.ValueField = "SPPD$ID_DOCUMENTO";
                cbInstrumento.TextField = "SPPD$DESCRIPCION";
                cbInstrumento.DataBind();
            }

        }

        protected void btnCancelaInstrumento_Click(object sender, EventArgs e)
        {
            txtDescripcionInstrumento.Text = "";
            cbInstrumento.SelectedIndex = -1;
            panArchivo.Style.Add("display", "none");
            panInstrumento.Style.Add("display", "none");
            cbInstrumentos.SelectedIndex = -1;
            cbInstrumento.Enabled = true;
            popInstrumento.ShowOnPageLoad = false;
            Session["operainstrumento"] = null;
        }

        protected void btnGrabaInstrumento_Click(object sender, EventArgs e)
        {//inicio de metodo            
            //inicio de validacion    
            DataTable poa;
            String poas;
            if (cbInstrumento.SelectedIndex == -1 && Session["operainstrumento"].ToString() == "OI0")
            {
                mensaje = "Seleccione el instrumento de planificación";
                ScriptManager.RegisterStartupScript(this.upPopInstrument, GetType(), "script", "Alerta('" + mensaje + " <br/>',2);", true);
                cbInstrumento.Focus();
            }
            else if (cbInstrumentos.SelectedIndex == -1)
            {
                mensaje = "Conteste la pregunta ¿Va adjuntar el documento en formato PDF del Instrumento de Planificación?";
                ScriptManager.RegisterStartupScript(this.upPopInstrument, GetType(), "script", "Alerta('" + mensaje + " <br/>',2);", true);
                cbInstrumento.Focus();
            }
            //fin de validacion
            else
            {//inicio de operaciones
                if (Session["operainstrumento"].ToString() == "OI0")//inicio inserta
                {
                    if (cbInstrumentos.SelectedIndex == 1)
                    {
                        if (fileInstrumento.HasFile)
                        {
                            SaveFile(fileInstrumento.PostedFile);
                            if (Session["archivo"] != null && Session["carpeta"] != null)
                            {
                                if (Convert.ToInt32(cbInstrumento.Value) > 1)
                                    sql = "INSERT INTO SCHE$SIPLAN20.SP20$POMDOCUMENTO (SPPDOC$POM, SPPDOC$INSTO,SPPDOC$TIPO, SPPDOC$DESCRIPCION, SPPDOC$NOMARCHIVO, SPPDOC$RUTA, SPPDOC$FECHA_INSERTA,SPPDOC$PROPIETARIO, SPPDOC$POA) VALUES (" + Convert.ToInt32(Session["pom"]) + ", " + Session["insto"] + ", 2, ";
                                else
                                    sql = "INSERT INTO SCHE$SIPLAN20.SP20$POMDOCUMENTO (SPPDOC$POM, SPPDOC$INSTO,SPPDOC$TIPO, SPPDOC$DESCRIPCION, SPPDOC$NOMARCHIVO, SPPDOC$RUTA, SPPDOC$FECHA_INSERTA,SPPDOC$PROPIETARIO) VALUES (" + Convert.ToInt32(Session["pom"]) + ", " + Session["insto"] + ", " + Convert.ToInt32(cbInstrumento.Value) + ", ";
                                if (txtDescripcionInstrumento.Text == "")
                                    sql = sql + "NULL, ";
                                else
                                    sql = sql + "'" + txtDescripcionInstrumento.Text + "', ";
                                if (Convert.ToInt32(cbInstrumento.Value) > 1)
                                {
                                    poas = "SELECT * FROM SCHE$SIPLAN20.SP20$POA POA WHERE SPOA$ANIO = " + Convert.ToInt32(cbInstrumento.Value) + " AND SPOA$ID_POM = " + Convert.ToInt32(Session["pom"]) + " AND SPOA$ID_INSTITUCION = " + Convert.ToInt32(Session["insto"]) + " AND SPOA$RESTRICTIVA = 'N'";
                                    estado = dao.consulta(poas);
                                    if (estado != 0)
                                    {
                                        poa = dao.tabla;
                                        sql = sql + "'" + Session["archivo"].ToString() + "', '" + Session["carpeta"] + "', 'INSERT = '||TO_CHAR(SYSDATE,'DD/MM/YYYY HH24:MI')||' '||'" + Session["USUARIO"].ToString() + "', '" + Session["USUARIO"].ToString() + "', " + Convert.ToInt32(poa.Rows[0]["SPOA$ID_POA"]) + ")";

                                    }

                                }

                                else
                                    sql = sql + "'" + Session["archivo"].ToString() + "', '" + Session["carpeta"] + "', 'INSERT = '||TO_CHAR(SYSDATE,'DD/MM/YYYY HH24:MI')||' '||'" + Session["USUARIO"].ToString() + "', '" + Session["USUARIO"].ToString() + "')";

                                estado = dao.comando(sql);
                                if (estado == 0)
                                {
                                    mensaje = dao.mensaje;
                                    ScriptManager.RegisterStartupScript(this.upPopInstrument, GetType(), "script", "Alerta('" + mensaje + " <br/>',2);", true);
                                    cbInstrumento.SelectedIndex = -1;
                                    txtDescripcionInstrumento.Text = "";
                                    cbInstrumentos.SelectedIndex = -1;
                                    panArchivo.Style.Add("display", "none");
                                    panInstrumento.Style.Add("display", "none");
                                    popInstrumento.ShowOnPageLoad = false;
                                    Session["operainstrumento"] = null;
                                    cargaInstrumentos(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]));

                                }
                                else
                                {
                                    mensaje = "Instrumento registrado correctamente";
                                    ScriptManager.RegisterStartupScript(this.upPopInstrument, GetType(), "script", "Alerta('" + mensaje + " <br/>',1);", true);
                                    cbInstrumento.SelectedIndex = -1;
                                    txtDescripcionInstrumento.Text = "";
                                    cbInstrumentos.SelectedIndex = -1;
                                    panArchivo.Style.Add("display", "none");
                                    panInstrumento.Style.Add("display", "none");
                                    popInstrumento.ShowOnPageLoad = false;
                                    Session["operainstrumento"] = null;
                                    cargaInstrumentos(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]));
                                    //btnDocumento_Click(sender, e);
                                }
                            }

                        }


                        else
                        {

                            mensaje = "Adjunte el archivo en PDF con el instrumento de planificación";
                            ScriptManager.RegisterStartupScript(this.upPopInstrument, GetType(), "script", "Alerta('" + mensaje + " <br/>',2);", true);
                            fileInstrumento.Focus();
                        }
                    }

                    else if (cbInstrumentos.SelectedIndex == 0)
                    {
                        if (Convert.ToInt32(cbInstrumento.Value) > 1)
                            sql = "INSERT INTO SCHE$SIPLAN20.SP20$POMDOCUMENTO (SPPDOC$POM, SPPDOC$INSTO,SPPDOC$TIPO, SPPDOC$DESCRIPCION, SPPDOC$FECHA_INSERTA,SPPDOC$PROPIETARIO, SPPDOC$POA) VALUES (" + Convert.ToInt32(Session["pom"]) + ", " + Session["insto"] + ", 2, ";
                        else
                            sql = "INSERT INTO SCHE$SIPLAN20.SP20$POMDOCUMENTO (SPPDOC$POM, SPPDOC$INSTO,SPPDOC$TIPO, SPPDOC$DESCRIPCION, SPPDOC$FECHA_INSERTA,SPPDOC$PROPIETARIO) VALUES (" + Convert.ToInt32(Session["pom"]) + ", " + Session["insto"] + ", " + Convert.ToInt32(cbInstrumento.Value) + ", ";
                        if (txtDescripcionInstrumento.Text == "")
                        {
                            sql = sql + " NULL, ";

                        }
                        else
                        {
                            sql = sql + " '" + txtDescripcionInstrumento.Text + "', ";
                        }

                        if (Convert.ToInt32(cbInstrumento.Value) > 1)
                        {
                            poas = "SELECT * FROM SCHE$SIPLAN20.SP20$POA POA WHERE SPOA$ANIO = " + Convert.ToInt32(cbInstrumento.Value) + " AND SPOA$ID_POM = " + Convert.ToInt32(Session["pom"]) + " AND SPOA$ID_INSTITUCION = " + Convert.ToInt32(Session["insto"]) + " AND SPOA$RESTRICTIVA = 'N'";
                            estado = dao.consulta(poas);
                            if (estado != 0)
                            {
                                poa = dao.tabla;
                                //sql = sql + "'" + Session["archivo"].ToString() + "', '" + Session["carpeta"] + "', 'INSERT = '||TO_CHAR(SYSDATE,'DD/MM/YYYY HH24:MI')||' '||'" + Session["USUARIO"].ToString() + "', '" + Session["USUARIO"].ToString() + "', " + Convert.ToInt32(poa.Rows[0]["SPOA$ID_POA"]) + ")";
                                sql = sql + " 'INSERT = '||TO_CHAR(SYSDATE,'DD/MM/YYYY HH24:MI')||' '||'" + Session["USUARIO"].ToString() + "', '" + Session["USUARIO"].ToString() + "', " + Convert.ToInt32(poa.Rows[0]["SPOA$ID_POA"]) + ")";
                            }

                        }

                        else
                            //sql = sql + "'" + Session["archivo"].ToString() + "', '" + Session["carpeta"] + "', 'INSERT = '||TO_CHAR(SYSDATE,'DD/MM/YYYY HH24:MI')||' '||'" + Session["USUARIO"].ToString() + "', '" + Session["USUARIO"].ToString() + "')";
                            sql = sql + " 'INSERT = '||TO_CHAR(SYSDATE,'DD/MM/YYYY HH24:MI')||' '||'" + Session["USUARIO"].ToString() + "', '" + Session["USUARIO"].ToString() + "')";

                        estado = dao.comando(sql);
                        if (estado == 0)
                        {

                            mensaje = dao.mensaje;
                            ScriptManager.RegisterStartupScript(this.upPopInstrument, GetType(), "script", "Alerta('" + mensaje + " <br/>',2);", true);
                            cbInstrumento.SelectedIndex = -1;
                            txtDescripcionInstrumento.Text = "";
                            cbInstrumentos.SelectedIndex = -1;
                            panInstrumento.Style.Add("display", "none");
                            popInstrumento.ShowOnPageLoad = false;
                            Session["operainstrumento"] = null;
                            cargaInstrumentos(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]));
                        }
                        else
                        {

                            mensaje = "Instrumento registrado, correctamente";
                            ScriptManager.RegisterStartupScript(this.upPopInstrument, GetType(), "script", "Alerta('" + mensaje + " <br/>',1);", true);
                            cbInstrumento.SelectedIndex = -1;
                            txtDescripcionInstrumento.Text = "";
                            cbInstrumentos.SelectedIndex = -1;
                            panArchivo.Style.Add("display", "none");
                            panInstrumento.Style.Add("display", "none");
                            popInstrumento.ShowOnPageLoad = false;
                            Session["operainstrumento"] = null;
                            cargaInstrumentos(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]));

                        }



                    }
                }//Fin inserta

                else if (Session["operainstrumento"].ToString() == "OI1")
                {//inicio adjunta archivo
                    if (cbInstrumentos.SelectedIndex == 1)
                    {
                        if (fileInstrumento.HasFile)
                        {
                            SaveFile(fileInstrumento.PostedFile);
                            if (Session["archivo"] != null && Session["carpeta"] != null)
                            {
                                sql = "UPDATE SCHE$SIPLAN20.SP20$POMDOCUMENTO SET SPPDOC$NOMARCHIVO = '" + Session["archivo"].ToString() + "', SPPDOC$RUTA = '" + Session["carpeta"].ToString() + "'";
                                if (txtDescripcionInstrumento.Text == "")
                                    sql = sql + ", SPPDOC$DESCRIPCION = NULL, ";
                                else
                                    sql = sql + ", SPPDOC$DESCRIPCION = '" + txtDescripcionInstrumento.Text + "', ";
                                sql = sql + "SPPDOC$FECHA_UPDATE = 'UPDATE = '||TO_CHAR(SYSDATE,'DD/MM/YYYY HH24:MI')||' '||'" + Session["USUARIO"].ToString() + "' ";
                                sql = sql + " WHERE SPPDOC$POM = " + Convert.ToInt32(gvDocumentos.GetRowValues(gvDocumentos.FocusedRowIndex, "SPPDOC$POM").ToString()) + " AND SPPDOC$INSTO = " + Convert.ToInt32(gvDocumentos.GetRowValues(gvDocumentos.FocusedRowIndex, "SPPDOC$INSTO").ToString()) + " AND SPPDOC$ID_DOCUMENTO = " + Convert.ToInt32(gvDocumentos.GetRowValues(gvDocumentos.FocusedRowIndex, "SPPDOC$ID_DOCUMENTO").ToString()) + " AND SPPDOC$RESTRICTIVA = 'N'";

                                estado = dao.comando(sql);
                                if (estado == 0)
                                {
                                    mensaje = dao.mensaje;
                                    ScriptManager.RegisterStartupScript(this.upPopInstrument, GetType(), "script", "Alerta('" + mensaje + " <br/>',2);", true);
                                    cbInstrumento.SelectedIndex = -1;
                                    txtDescripcionInstrumento.Text = "";
                                    cbInstrumentos.SelectedIndex = -1;
                                    panArchivo.Style.Add("display", "none");
                                    panInstrumento.Style.Add("display", "none");
                                    popInstrumento.ShowOnPageLoad = false;
                                    Session["operainstrumento"] = null;
                                    cargaInstrumentos(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]));
                                }
                                else
                                {
                                    mensaje = "Instrumento registrado, correctamente";
                                    ScriptManager.RegisterStartupScript(this.upPopInstrument, GetType(), "script", "Alerta('" + mensaje + " <br/>',1);", true);
                                    cbInstrumento.SelectedIndex = -1;
                                    txtDescripcionInstrumento.Text = "";
                                    cbInstrumentos.SelectedIndex = -1;
                                    panArchivo.Style.Add("display", "none");
                                    panInstrumento.Style.Add("display", "none");
                                    popInstrumento.ShowOnPageLoad = false;
                                    Session["operainstrumento"] = null;
                                    cargaInstrumentos(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]));
                                }




                            }

                        }

                    }

                    else
                    {
                        sql = "UPDATE SCHE$SIPLAN20.SP20$POMDOCUMENTO SET ";

                        if (txtDescripcionInstrumento.Text == "")
                            sql = sql + "SPPDOC$DESCRIPCION = NULL, ";
                        else
                            sql = sql + "SPPDOC$DESCRIPCION = '" + txtDescripcionInstrumento.Text + "', ";
                        if (gvDocumentos.GetRowValues(gvDocumentos.FocusedRowIndex, "SPPDOC$POM") == DBNull.Value)
                            sql = sql + "SPPDOC$NOMARCHIVO = NULL, SPPDOC$RUTA = NULL, ";

                        sql = sql + "SPPDOC$FECHA_UPDATE = 'UPDATE = '||TO_CHAR(SYSDATE,'DD/MM/YYYY HH24:MI')||' '||'" + Session["USUARIO"].ToString() + "' ";
                        sql = sql + "WHERE SPPDOC$POM = " + Convert.ToInt32(gvDocumentos.GetRowValues(gvDocumentos.FocusedRowIndex, "SPPDOC$POM").ToString()) + " AND SPPDOC$INSTO = " + Convert.ToInt32(gvDocumentos.GetRowValues(gvDocumentos.FocusedRowIndex, "SPPDOC$INSTO").ToString()) + " AND SPPDOC$ID_DOCUMENTO = " + Convert.ToInt32(gvDocumentos.GetRowValues(gvDocumentos.FocusedRowIndex, "SPPDOC$ID_DOCUMENTO").ToString()) + " AND SPPDOC$RESTRICTIVA = 'N'";

                        estado = dao.comando(sql);
                        if (estado == 0)
                        {
                            mensaje = dao.mensaje;
                            ScriptManager.RegisterStartupScript(this.upPopInstrument, GetType(), "script", "Alerta('" + mensaje + " <br/>',2);", true);
                            cbInstrumento.SelectedIndex = -1;
                            txtDescripcionInstrumento.Text = "";
                            cbInstrumentos.SelectedIndex = -1;
                            panArchivo.Style.Add("display", "none");
                            panInstrumento.Style.Add("display", "none");
                            popInstrumento.ShowOnPageLoad = false;
                            Session["operainstrumento"] = null;
                            cargaInstrumentos(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]));
                        }
                        else
                        {
                            mensaje = "Instrumento registrado correctamente";
                            ScriptManager.RegisterStartupScript(this.upPopInstrument, GetType(), "script", "Alerta('" + mensaje + " <br/>',1);", true);
                            cbInstrumento.SelectedIndex = -1;
                            txtDescripcionInstrumento.Text = "";
                            cbInstrumentos.SelectedIndex = -1;
                            panArchivo.Style.Add("display", "none");
                            panInstrumento.Style.Add("display", "none");
                            popInstrumento.ShowOnPageLoad = false;
                            Session["operainstrumento"] = null;
                            cargaInstrumentos(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]));
                        }


                    }
                }//fin adjunta archivo


            }//fin de operaciones



        }//fin de metodo

        protected void cargaInstrumentos(int pom, int insto)
        {
            //sql = "SELECT SPPDOC$ID_DOCUMENTO, D.SPPDOC$POM, D.SPPDOC$INSTO, D.SPPDOC$TIPO, TD.SPPD$DESCRIPCION, TD.SPPD$SIGLAS, D.SPPDOC$DESCRIPCION, CASE WHEN D.SPPDOC$NOMARCHIVO IS NULL THEN 'NO' WHEN D.SPPDOC$NOMARCHIVO IS NOT NULL THEN 'SI' END AS ARCHIVO_ADJUNTO, D.SPPDOC$NOMARCHIVO, D.SPPDOC$RUTA, D.SPPDOC$PROPIETARIO FROM SCHE$SIPLAN20.SP20$POMDOCUMENTO D INNER JOIN SCHE$SIPLAN20.SPP20$TIPODOCUMENTO TD ON D.SPPDOC$TIPO = TD.SPPD$ID_DOCUMENTO AND D.SPPDOC$RESTRICTIVA = 'N' AND TD.SPPD$RESTRICTIVA = 'N' WHERE D.SPPDOC$POM = " + pom + " AND D.SPPDOC$INSTO = " + insto + " AND D.SPPDOC$RESTRICTIVA = 'N' AND TD.SPPD$SIGLAS IN('POM', 'PEI')";
            sql = "SELECT SPPDOC$ID_DOCUMENTO, SPPDOC$POM, SPPDOC$INSTO, SPPDOC$TIPO, CASE WHEN SPPDOC$TIPO = 2 THEN 'PLAN OPERATIVO ANUAL '||'- '||ANIO ELSE SPPD$DESCRIPCION END AS SPPD$DESCRIPCION, SPPD$SIGLAS, SPPDOC$DESCRIPCION, ARCHIVO_ADJUNTO, SPPDOC$NOMARCHIVO, SPPDOC$NOMARCHIVO, SPPDOC$RUTA, SPPDOC$PROPIETARIO, SPPDOC$POA  FROM ( SELECT SPPDOC$ID_DOCUMENTO, D.SPPDOC$POM, D.SPPDOC$INSTO, D.SPPDOC$TIPO, TD.SPPD$DESCRIPCION, TD.SPPD$SIGLAS, D.SPPDOC$DESCRIPCION, (SELECT SPOA$ANIO FROM SCHE$SIPLAN20.SP20$POA WHERE SPOA$ID_POA = D.SPPDOC$POA AND SPOA$ID_POM = D.SPPDOC$POM AND SPOA$ID_INSTITUCION = D.SPPDOC$INSTO) ANIO,  CASE WHEN D.SPPDOC$NOMARCHIVO IS NULL THEN 'NO' WHEN D.SPPDOC$NOMARCHIVO IS NOT NULL THEN 'SI' END AS ARCHIVO_ADJUNTO, D.SPPDOC$NOMARCHIVO, D.SPPDOC$RUTA, D.SPPDOC$PROPIETARIO, D.SPPDOC$POA FROM SCHE$SIPLAN20.SP20$POMDOCUMENTO D INNER JOIN SCHE$SIPLAN20.SPP20$TIPODOCUMENTO TD ON D.SPPDOC$TIPO = TD.SPPD$ID_DOCUMENTO AND D.SPPDOC$RESTRICTIVA = 'N' AND TD.SPPD$RESTRICTIVA = 'N' WHERE D.SPPDOC$POM = " + pom + " AND D.SPPDOC$INSTO = " + insto + " AND D.SPPDOC$RESTRICTIVA = 'N') ORDER BY SPPDOC$TIPO, SPPDOC$POA ASC";
            estado = dao.consulta(sql);
            if (estado == 0)
            {
                mensaje = dao.mensaje;
                Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
            }

            else
            {
                tabla = dao.tabla;
                gvDocumentos.DataSource = tabla;
                gvDocumentos.DataBind();

            }
        }
        protected void SaveFile(HttpPostedFile file)
        {

            // Specify the path to save the uploaded file to.
            //string savePath = "E:\\Documentos\\";
            string savePath = full_path;
            //string savePath = Server.MapPath("\\") + "Documentos\\";
            //string savePath = Server.MapPath("\\siplan\\") + "Documentos\\";
            // Get the name of the file to upload.
            string fileName = Convert.ToString(Session["pom"]) + "_" + Convert.ToString(Session["insto"]) + "_" + fileInstrumento.FileName;




            // Create the path and file name to check for duplicates.
            string pathToCheck = savePath + fileName;

            // Create a temporary file name to use for checking duplicates.
            string tempfileName = "";

            // Check to see if a file already exists with the
            // same name as the file to upload.        
            if (System.IO.File.Exists(pathToCheck))
            {
                int counter = 2;
                while (System.IO.File.Exists(pathToCheck))
                {
                    // if a file with this name already exists,
                    // prefix the filename with a number.
                    tempfileName = counter.ToString() + fileName;
                    pathToCheck = savePath + tempfileName;
                    counter++;
                }

                fileName = tempfileName;

                // Notify the user that the file name was changed.

            }


            // Append the name of the file to upload to the path.
            savePath += fileName;

            // Call the SaveAs method to save the uploaded
            // file to the specified directory.
            fileInstrumento.SaveAs(savePath);
            Session["archivo"] = fileName;
            Session["carpeta"] = savePath;


        }

        protected void CustomValidator1_ServerValidate(object source, ServerValidateEventArgs args)

        {

            if (fileInstrumento.FileBytes.Length > 1024)

            {

                args.IsValid = false;

            }

            else

            {

                args.IsValid = true;

            }

        }

        protected void btnVerArchivo_Click(object sender, EventArgs e)
        {
            String path;
            String nombredocumento;
            String url;
            if (gvDocumentos.FocusedRowIndex != -1)
            {

                if (gvDocumentos.GetRowValues(gvDocumentos.FocusedRowIndex, "ARCHIVO_ADJUNTO").ToString() == "NO")
                {
                    mensaje = "El instrumento consultado no tiene archivo adjunto, por favor publique el mismo para desplegarlo en vista previa";
                    ScriptManager.RegisterStartupScript(this.panBotonArchivos, GetType(), "script", "Alerta('" + mensaje + " <br/>',2);", true);
                }
                else
                {

                    path = Path.GetFullPath("E:\\Documentos\\");//@"http://siseg.segeplan.gob.gt/Documentos/";
                    nombredocumento = gvDocumentos.GetRowValues(gvDocumentos.FocusedRowIndex, "SPPDOC$NOMARCHIVO").ToString();
                    //url = "../../Documentos/"+nombredocumento; 
                    //ScriptManager.RegisterStartupScript(panBotonArchivos, panBotonArchivos.GetType(), "script", "abrirVentana('"+url+"');", true);
                    //popDocumento.ContentUrl = "../../Documentos/" + gvDocumentos.GetRowValues(gvDocumentos.FocusedRowIndex, "SPPDOC$NOMARCHIVO").ToString();
                    popDocumento.ContentUrl = paths + gvDocumentos.GetRowValues(gvDocumentos.FocusedRowIndex, "SPPDOC$NOMARCHIVO").ToString();
                    //vstPrevia.Src = url;
                    //vstPrevia.Src = "../Documentos/"+ gvDocumentos.GetRowValues(gvDocumentos.FocusedRowIndex, "SPPDOC$NOMARCHIVO").ToString();
                    //vstPrevia.Src = "../Documentos/" + gvDocumentos.GetRowValues(gvDocumentos.FocusedRowIndex, "SPPDOC$NOMARCHIVO").ToString();
                    popDocumento.ShowOnPageLoad = true;
                }

            }
            else
            {
                mensaje = "Seleccione una fila con un instrumento de planificación";
                ScriptManager.RegisterStartupScript(this.panBotonArchivos, GetType(), "script", "Alerta('" + mensaje + " <br/>',2);", true);
            }

        }

        protected void gvDocumentos_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            String valor;
            if (e.DataColumn.FieldName == "ARCHIVO_ADJUNTO")
            {
                if (e.CellValue != DBNull.Value)
                {
                    valor = e.CellValue.ToString();
                    if (valor == "NO")
                    {
                        e.Cell.ForeColor = Color.Red;

                    }
                    else if (valor == "SI")
                    {
                        e.Cell.ForeColor = Color.Green;
                    }

                }
            }
        }

        protected void btnAdjuntarArchivo_Click(object sender, EventArgs e)
        {
            if (gvDocumentos.FocusedRowIndex != -1)
            {
                //if ((gvDocumentos.GetRowValues(gvDocumentos.FocusedRowIndex, "SPPDOC$PROPIETARIO").ToString() == Session["USUARIO"].ToString()) || Session["ROL"].ToString() == "ADMIN")
                if (Session["ROL"].ToString() != "ENTIDAD")
                {
                    Session["operainstrumento"] = "OI1";
                    lblInstrumentoOpera.Text = "Edición de instrumento de planificación";
                    cargaInstrumentofalta(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]));
                    cbInstrumento.Enabled = false;
                    cbInstrumento.Text = gvDocumentos.GetRowValues(gvDocumentos.FocusedRowIndex, "SPPD$DESCRIPCION").ToString();
                    if (gvDocumentos.GetRowValues(gvDocumentos.FocusedRowIndex, "SPPDOC$DESCRIPCION") != DBNull.Value)
                        txtDescripcionInstrumento.Text = gvDocumentos.GetRowValues(gvDocumentos.FocusedRowIndex, "SPPDOC$DESCRIPCION").ToString();
                    else
                        txtDescripcionInstrumento.Text = "";
                    if (gvDocumentos.GetRowValues(gvDocumentos.FocusedRowIndex, "SPPDOC$NOMARCHIVO") != DBNull.Value)
                    {
                        panArchivo.Style.Add("display", "block");
                        lblarchivo.Text = gvDocumentos.GetRowValues(gvDocumentos.FocusedRowIndex, "SPPDOC$NOMARCHIVO").ToString();
                    }
                    else
                        panArchivo.Style.Add("display", "none");

                    cbInstrumentos.SelectedIndex = -1;
                    panInstrumento.Style.Add("display", "none");
                    btnGrabaInstrumento.CssClass = "btn btn-primary";
                    btnGrabaInstrumento.Text = "Editar instrumento";
                    popInstrumento.ShowOnPageLoad = true;
                }

                else
                {
                    mensaje = "Su perfil de usuario no le permite editar el instrumento de planificación";
                    ScriptManager.RegisterStartupScript(this.panBotonArchivos, GetType(), "script", "Alerta('" + mensaje + " <br/>',2);", true);
                }


            }

            else
            {
                mensaje = "Seleccione una fila con un instrumento de planificación";
                ScriptManager.RegisterStartupScript(this.panBotonArchivos, GetType(), "script", "Alerta('" + mensaje + " <br/>',2);", true);
            }

        }

        protected void btnEliminarArchivo_Click(object sender, EventArgs e)
        {
            int pom, insto, docto;
            if (gvDocumentos.FocusedRowIndex != -1)
            {
                //if ((gvDocumentos.GetRowValues(gvDocumentos.FocusedRowIndex, "SPPDOC$PROPIETARIO").ToString() == Session["USUARIO"].ToString()) || Session["ROL"].ToString() == "ADMIN")
                if (Session["ROL"].ToString() != "ENTIDAD")
                {
                    pom = Convert.ToInt32(gvDocumentos.GetRowValues(gvDocumentos.FocusedRowIndex, "SPPDOC$POM").ToString());
                    insto = Convert.ToInt32(gvDocumentos.GetRowValues(gvDocumentos.FocusedRowIndex, "SPPDOC$INSTO").ToString());
                    docto = Convert.ToInt32(gvDocumentos.GetRowValues(gvDocumentos.FocusedRowIndex, "SPPDOC$ID_DOCUMENTO").ToString());
                    sql = "UPDATE SCHE$SIPLAN20.SP20$POMDOCUMENTO SET SPPDOC$RESTRICTIVA = 'S', SPPDOC$FECHA_DELETE = 'DELETE = '||TO_CHAR(SYSDATE,'DD/MM/YYYY HH24:MI')||' '||'" + Session["USUARIO"].ToString() + "' WHERE SPPDOC$POM = " + pom + " AND SPPDOC$INSTO = " + insto + " AND SPPDOC$ID_DOCUMENTO = " + docto;

                    estado = dao.comando(sql);

                    if (estado == 0)
                    {
                        mensaje = dao.mensaje;
                        ScriptManager.RegisterStartupScript(this.panBotonArchivos, GetType(), "script", "Alerta('" + mensaje + " <br/>',2);", true);
                    }
                    else
                    {
                        mensaje = "Instrumento de planificación eliminado correctamente";
                        ScriptManager.RegisterStartupScript(this.panBotonArchivos, GetType(), "script", "Alerta('" + mensaje + " <br/>',1);", true);
                        cargaInstrumentos(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]));
                    }
                }
                else
                {
                    mensaje = "Su perfil de usuario no le permite editar el instrumento de planificación";
                    ScriptManager.RegisterStartupScript(this.panBotonArchivos, GetType(), "script", "Alerta('" + mensaje + " <br/>',2);", true);
                }
            }
            else
            {
                mensaje = "Seleccione una fila con un instrumento de planificación";
                ScriptManager.RegisterStartupScript(this.panBotonArchivos, GetType(), "script", "Alerta('" + mensaje + " <br/>',2);", true);
            }

        }

        protected void btnPOA_Click(object sender, EventArgs e)
        {
            int insto, pom, periodo;
            String institucion;
            int estados = 0;
            DataTable periodos = new DataTable();
            DataTable poa = new DataTable();
            if (gvPOMInsto.FocusedRowIndex != -1)
            {
                pom = Convert.ToInt32(gvPOMInsto.GetRowValues(gvPOMInsto.FocusedRowIndex, "SPPO$ID_POM").ToString());
                insto = Convert.ToInt32(gvPOMInsto.GetRowValues(gvPOMInsto.FocusedRowIndex, "SPPO$ID_INSTITUCION").ToString());
                periodo = Convert.ToInt32(gvPOMInsto.GetRowValues(gvPOMInsto.FocusedRowIndex, "SPPO$ID_PERIODO").ToString());
                institucion = gvPOMInsto.GetRowValues(gvPOMInsto.FocusedRowIndex, "NOMBRE").ToString();
                sql = "SELECT P.SPP$ID_PERIODO, POA.SPOA$ID_POA, POA.SPOA$ANIO, POA.SPOA$ID_POM, POA.SPOA$ID_INSTITUCION FROM SCHE$SIPLAN20.SP20$POA POA INNER JOIN SCHE$SIPLAN20.SP20$POM POM ON POM.SPPO$ID_POM = POA.SPOA$ID_POM AND POM.SPPO$ID_INSTITUCION = POA.SPOA$ID_INSTITUCION AND POM.SPPO$RESTRICTIVA = 'N' AND POA.SPOA$RESTRICTIVA = 'N' ";
                sql = sql + "INNER JOIN SCHE$SIPLAN20.SP20$PERIODO P ON P.SPP$ID_PERIODO = POM.SPPO$ID_PERIODO AND P.SPP$RESTRICTIVA = 'N' AND POM.SPPO$RESTRICTIVA = 'N' WHERE POA.SPOA$ID_POM = " + pom + " AND POA.SPOA$ID_INSTITUCION =" + insto + " AND P.SPP$ID_PERIODO = " + periodo;

                estado = dao.consulta(sql);

                if (estado == 0)
                {
                    mensaje = dao.mensaje;
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
                }
                else
                {
                    poa = dao.tabla;
                    if (poa.Rows.Count > 0)
                    {
                        Session["pom"] = pom;
                        Session["insto"] = insto;
                        Session["periodo"] = periodo;
                        Session["institucion"] = institucion;
                        Response.Redirect("../poa/poa.aspx");
                        /*sql = "SELECT SPOA$ID_POA FROM SCHE$SIPLAN20.SP20$POA P INNER JOIN SCHE$SIPLAN20.SP20$POM PO ON P.SPOA$ID_POM = PO.SPPO$ID_POM AND P.SPOA$RESTRICTIVA = 'N' AND PO.SPPO$RESTRICTIVA = 'N' INNER JOIN SCHE$SIPLAN20.SP20$PERIODO PE ON PO.SPPO$ID_PERIODO = PE.SPP$ID_PERIODO AND PO.SPPO$RESTRICTIVA = 'N' AND PE.SPP$RESTRICTIVA = 'N' WHERE P.SPOA$ID_POM = " + pom+ " AND P.SPOA$ID_INSTITUCION = "+insto+ " AND PE.SPP$ID_PERIODO = 0 AND P.SPOA$ANIO = 2025";
                        estado = dao.consulta(sql);
                        poa = dao.tabla;
                        if (poa.Rows.Count > 0)
                        {
                            Response.Redirect("../poa/poa.aspx");
                        }
                        else
                        {
                            sql = "INSERT INTO SCHE$SIPLAN20.SP20$POA (SPOA$ANIO, SPOA$ID_POM, SPOA$ID_INSTITUCION, SPOA$FECHA_INSERT, SPOA$PROPIETARIO) VALUES (2025, " + pom + ", " + insto + ", 'INSERT = '||TO_CHAR(SYSDATE,'DD/MM/YYYY HH24:MI')||' '||'" + Session["USUARIO"].ToString() + "', '" + Session["USUARIO"].ToString() + "' )";
                            estado = dao.comando(sql);
                            if (estado == 0)
                            {
                                mensaje = dao.mensaje;
                                Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
                            }
                            else
                                Response.Redirect("../poa/poa.aspx");
                        }*/

                    }
                    else
                    {
                        sql = "SELECT * FROM SCHE$SIPLAN20.SP20$PERIODO P WHERE P.SPP$ID_PERIODO = " + periodo + " AND P.SPP$RESTRICTIVA = 'N'";
                        estado = dao.consulta(sql);
                        if (estado == 0)
                        {
                            mensaje = dao.mensaje;
                            Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
                        }
                        else
                        {
                            periodos = dao.tabla;
                            if (periodos.Rows.Count > 0)
                            {

                                for (int i = Convert.ToInt32(periodos.Rows[0]["SPP$INICIO"]); i <= Convert.ToInt32(periodos.Rows[0]["SPP$FINAL"]); i++)
                                {
                                    sql = "INSERT INTO SCHE$SIPLAN20.SP20$POA (SPOA$ANIO, SPOA$ID_POM, SPOA$ID_INSTITUCION, SPOA$FECHA_INSERT, SPOA$PROPIETARIO) VALUES (" + i + ", " + pom + ", " + insto + ", 'INSERT = '||TO_CHAR(SYSDATE,'DD/MM/YYYY HH24:MI')||' '||'" + Session["USUARIO"].ToString() + "', '" + Session["USUARIO"].ToString() + "' )";
                                    estado = dao.comando(sql);
                                    if (estado == 0)
                                    {
                                        mensaje = dao.mensaje;
                                        Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
                                        estados = 0;
                                        break;
                                    }
                                    else
                                    {
                                        estados = 1;
                                    }

                                }

                                if (estados == 0)
                                {
                                    mensaje = "Ha ocurrido un error en el sistema, contacte al administrador";
                                    Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
                                }

                                else if (estados == 1)
                                {
                                    Session["pom"] = pom;
                                    Session["insto"] = insto;
                                    Session["periodo"] = periodo;
                                    Session["institucion"] = institucion;
                                    Response.Redirect("../poa/poa.aspx");
                                }

                            }

                        }


                    }
                }



            }
            else
            {
                mensaje = "Seleccione el encabezado de la programación multianual";
                Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
            }

        }

        protected void btnEjecución_Click(object sender, EventArgs e)
        {
            Response.Redirect("../ejecucion/frmEjecucionPOA.aspx");
        }

        protected void cargaEstrategicos(int pom, int insto)
        {
            String sql2;
            DataTable resultadosVin = new DataTable();
            int m1, m2;
            m1 = 0;
            m2 = 0;
            // sql = "SELECT RE.SPPRES$ID_RESULTADO ID_PILAR, RE.SPPRES$NIVEL NIVEL_PADRE, RE.SPPRES$CODIGO||' '||RE.SPPRES$DESCRIPCION PILARPGG, RES.SPPRES$ID_RESULTADO OBJETIVOPGG, RES.SPPRES$NIVEL NIVEL_HIJO, RES.SPPRES$CODIGO||' '||RES.SPPRES$DESCRIPCION OBJETIVO_SECTORIAL FROM SCHE$SIPLAN20.SP20$RESULTADOS_ESTRATEGICOS RE INNER JOIN SCHE$SIPLAN20.SP20$RESULTADOS_ESTRATEGICOS RES ON RES.SPPRES$DEPENDE = RE.SPPRES$ID_RESULTADO AND RE.SPPRES$RESTRICTIVA = 'N' AND RES.SPPRES$RESTRICTIVA = 'N' WHERE RE.SPPRES$NIVEL = 1 AND RES.SPPRES$NIVEL = 2 ORDER BY RES.SPPRES$ORDEN ASC";

            sql = @"SELECT ID_PILAR
                    ,NIVEL_PADRE
                    ,PILARPGG
                    ,OBJETIVOPGG
                    ,NIVEL_HIJO
                    ,OBJETIVO_SECTORIAL
                    ,SPPRES$ORDEN FROM(SELECT
                    RE.SPPRES$ID_RESULTADO ID_PILAR
                    ,RE.SPPRES$NIVEL NIVEL_PADRE
                    ,RE.SPPRES$CODIGO|| ' ' || RE.SPPRES$DESCRIPCION PILARPGG
                    ,RES.SPPRES$ID_RESULTADO OBJETIVOPGG
                    ,RES.SPPRES$NIVEL NIVEL_HIJO
                    ,RES.SPPRES$CODIGO || ' ' || RES.SPPRES$DESCRIPCION OBJETIVO_SECTORIAL
                    ,(SELECT SPPRES$ORDEN FROM SCHE$SIPLAN20.SP20$RESULTADOS_ESTRATEGICOS WHERE  SPPRES$ID_RESULTADO = RES.SPPRES$ID_RESULTADO AND SPPRES$RESTRICTIVA = 'N') SPPRES$ORDEN
                    FROM SCHE$SIPLAN20.SP20$RESULTADOS_ESTRATEGICOS RE
                    INNER JOIN SCHE$SIPLAN20.SP20$RESULTADOS_ESTRATEGICOS RES ON RES.SPPRES$DEPENDE = RE.SPPRES$ID_RESULTADO AND RE.SPPRES$RESTRICTIVA = 'N' AND RES.SPPRES$RESTRICTIVA = 'N' WHERE RE.SPPRES$NIVEL = 1 AND RES.SPPRES$NIVEL = 2 AND RES.SPPRES$RESPONSABLE = -1
                    UNION
                    SELECT
                    RE.SPPRES$ID_RESULTADO ID_PILAR
                    ,RE.SPPRES$NIVEL NIVEL_PADRE
                    ,RE.SPPRES$CODIGO || ' ' || RE.SPPRES$DESCRIPCION PILARPGG
                    ,RES.SPPRES$ID_RESULTADO OBJETIVOPGG
                    ,RES.SPPRES$NIVEL NIVEL_HIJO
                    ,RES.SPPRES$CODIGO || ' ' || RES.SPPRES$DESCRIPCION OBJETIVO_SECTORIAL
                    ,(SELECT SPPRES$ORDEN FROM SCHE$SIPLAN20.SP20$RESULTADOS_ESTRATEGICOS WHERE  SPPRES$ID_RESULTADO = RES.SPPRES$ID_RESULTADO AND SPPRES$RESTRICTIVA = 'N') SPPRES$ORDEN
                    FROM SCHE$SIPLAN20.SP20$RESULTADOS_ESTRATEGICOS RE
                    INNER JOIN SCHE$SIPLAN20.SP20$RESULTADOS_ESTRATEGICOS RES ON RES.SPPRES$DEPENDE = RE.SPPRES$ID_RESULTADO AND RE.SPPRES$RESTRICTIVA = 'N' AND RES.SPPRES$RESTRICTIVA = 'N' WHERE RE.SPPRES$NIVEL = 1 AND RES.SPPRES$NIVEL = 2 AND RES.SPPRES$RESPONSABLE = " + insto + ") ORDER BY  SPPRES$ORDEN ASC";

            sql2 = "SELECT RE.SPRES$ID_RESULTADO, RE.SPRES$COD_ESTRATEGICO FROM SCHE$SIPLAN20.SP20$RESULTADOS RE INNER JOIN SCHE$SIPLAN20.SP20$RESULTADOS_ESTRATEGICOS RES ON RE.SPRES$COD_ESTRATEGICO = RES.SPPRES$ID_RESULTADO AND RE.SPRES$RESTRICTIVA = 'N' AND RES.SPPRES$RESTRICTIVA = 'N' WHERE RE.SPRES$POM = " + pom + " AND RE.SPRES$INSTITUCION = " + insto;

            estado = dao.consulta(sql);
            if (estado == 0)
            {
                mensaje = dao.mensaje;
                //Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
                ScriptManager.RegisterStartupScript(this.upPanelVincula, GetType(), "script", "Alerta('" + mensaje + " <br/>',2);", true);
            }
            else
            {


                tabla = dao.tabla;
                gvResEstrategicos.DataSource = tabla;
                gvResEstrategicos.DataBind();
                //gvResEstrategicos.Selection.UnselectAll();
                gvResEstrategicos.ExpandAll();

                estado = dao.consulta(sql2);

                if (estado == 0)
                {
                    mensaje = dao.mensaje;
                    //Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
                    ScriptManager.RegisterStartupScript(this.upPanelVincula, GetType(), "script", "Alerta('" + mensaje + " <br/>',2);", true);
                }
                else
                {

                    resultadosVin = dao.tabla;

                    if (resultadosVin.Rows.Count > 0)
                    {

                        for (int i = 0; i <= gvResEstrategicos.VisibleRowCount - 1; i++)
                        {
                            m1 = Convert.ToInt32(gvResEstrategicos.GetRowValues(i, "OBJETIVOPGG"));

                            for (int o = 0; o <= resultadosVin.Rows.Count - 1; o++)
                            {
                                m2 = Convert.ToInt32(resultadosVin.Rows[o]["SPRES$COD_ESTRATEGICO"]);
                                if (m1 == m2)
                                {
                                    gvResEstrategicos.Selection.SelectRow(i);
                                }
                            }

                        }
                        gvResEstrategicos.DataBind();
                        //MultiView1.ActiveViewIndex = 2;
                        //Estrategicos.Style.Add("display", "block");
                        //Institucionales.Style.Add("display", "none");
                        //cargaResInsto(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]));
                        //estrategicosR.ShowOnPageLoad = true;
                    }


                }




            }

        }

        protected void cargaMunicipios(int codigo)
        {
            Session["CODIGO"] = codigo;
            DataTable munosVin = new DataTable();
            DataTable tablas = new DataTable();
            
            sql = @"SELECT 
                    COD_REGION
                    ,NOMBRE_REGION
                    ,SIGLA_GEOGRAFICO
                    ,COD_DEPTO
                    ,DEPTOS
                    ,SIGLA_DEPTO
                    ,b.geografico Municipio
                    ,a.nombre Nombre_municipio
                    FROM
                    (SELECT G.REGION COD_REGION
                    ,G.NOMBRE NOMBRE_REGION
                    ,G.SIGLA SIGLA_GEOGRAFICO
                    ,DE.GEOGRAFICO COD_DEPTO
                    ,DE.NOMBRE DEPTOS
                    ,DE.SIGLA SIGLA_DEPTO
                    FROM SINIP.CG_REGIONES G
                    INNER JOIN SINIP.CG_GEOGRAFICO DE ON G.REGION = DE.REGION AND G.RESTRICTIVA = 'N'
                    WHERE G.RESTRICTIVA = 'N' AND G.REGION NOT IN(9, 12, 99) AND DE.SIGLA IS NOT NULL  ORDER BY G.REGION, DE.GEOGRAFICO ASC)
                    ,cg_geografico a
                    ,cg_geografico b
                    ,cg_geografico c
                    ,cg_regiones d
                    where
                    b.geografico = a.geografico and substr(lpad(b.geografico,4,0),1,2)|| '00' = c.geografico and a.region = d.region and(substr(lpad(b.geografico, 4, 0), 1, 2) || '00' = '0' || COD_DEPTO OR substr(lpad(b.geografico, 4, 0), 1, 2) || '00' = COD_DEPTO)  AND b.geografico != COD_DEPTO  AND b.geografico NOT IN(SELECT G.geografico from SINIP.CG_GEOGRAFICO G  WHERE G.geografico like '%99')
                    ORDER BY
                    COD_REGION
                    ,COD_DEPTO
                    ,b.geografico
                    ASC";
            estado = dao.consulta(sql);
            if(estado == 1)
            {
                tablas = dao.tabla;
                if (tablas.Rows.Count > 0)
                {
                    gvTerritorio.DataSource = tablas;
                    gvTerritorio.DataBind();
                    gvTerritorio.ExpandAll();
                    gvTerritorio.Selection.UnselectAll();                   
                    Session["MUNICIPIOS"] = tablas;
                    marcaMunicipios(codigo);


                }
                else
                    Session["MUNICIPIOS"] = null;




            }
            
            /*if (estado == 1)
            {
                tablas = dao.tabla;
                gvTerritorio.DataSource = tablas;
                gvTerritorio.DataBind();
                gvTerritorio.ExpandAll();
                gvTerritorio.Selection.UnselectAll();
                if (codigo != -1)
                {

                    estado = dao.consulta(sql2);
                    if (estado == 1)
                    {
                        munosVin = dao.tabla;

                        if (munosVin.Rows.Count > 0)
                        {

                            for (int i = 0; i <= gvTerritorio.VisibleRowCount - 1; i++)
                            {
                                m1 = Convert.ToInt32(gvTerritorio.GetRowValues(i, "MUNICIPIO"));

                                for (int o = 0; o < munosVin.Rows.Count; o++)
                                {
                                    m2 = Convert.ToInt32(munosVin.Rows[o]["GEOGRAFICO"]);
                                    if (m1 == m2)
                                    {
                                        gvTerritorio.Selection.SelectRow(i);
                                    }
                                }

                            }
                            gvTerritorio.DataBind();


                        }


                    }
                }


            }
            */
        }

        protected void marcaMunicipios(int sub)
        {
            int m1, m2;
            if (sub != -1)
            {
                sql = "SELECT SPSM$GEOGRAFICO FROM SCHE$SIPLAN20.SP20$SUB_MUNOS WHERE SPSM$ID_SUB = "+sub+ " AND SPSM$RESTRICTIVA = 'N'";
                estado = dao.consulta(sql);
                if (estado == 1)
                {
                    tabla = dao.tabla;
                    if (tabla.Rows.Count > 0)
                    {
                        for (int i = 0; i <= gvTerritorio.VisibleRowCount - 1; i++)
                        {
                            m1 = Convert.ToInt32(gvTerritorio.GetRowValues(i, "MUNICIPIO"));
                            for (int o = 0; o < tabla.Rows.Count; o++)
                            {
                                m2 = Convert.ToInt32(tabla.Rows[o]["SPSM$GEOGRAFICO"]);
                                if (m1 == m2)
                                {
                                    gvTerritorio.Selection.SelectRow(i);
                                }
                            }

                        }
                    }
                    else
                        gvTerritorio.Selection.UnselectAll();
                }
            }
        }
        protected void grabaMunicipios(int codigo)
        {
            int cod_muno = -1;
            int busca = -1;
            DataTable munSel = new DataTable();
            DataTable munDes = new DataTable();


            if (Session["MUN_MARCADOS"] != null)
            {
                if (Session["MUN_MARCADOS"] is List<object> selectedKeys && selectedKeys.Count > 0)
                {
                    munSel = ConvertListToDataTableMun(selectedKeys);
                }

            }
            else
                munSel.Clear();


            if (Session["MUN_DESMARCADOS"] != null)
            {
                if (Session["MUN_DESMARCADOS"] is List<object> selectedKeys && selectedKeys.Count > 0)
                {
                    munDes = ConvertListToDataTableMun(selectedKeys);
                }

            }
            else
                munDes.Clear();


            if (munSel.Rows.Count > 0)
            {
                for (int i = 0; i < munSel.Rows.Count; i++)
                {
                    cod_muno = Convert.ToInt32(munSel.Rows[i]["MUNICIPIO"]);
                    busca = buscaMunicipio(codigo, cod_muno);
                    if (busca == -1)
                        break;
                    else if (busca == 0)
                    {
                        sql = "INSERT INTO SCHE$SIPLAN20.SP20$SUB_MUNOS (SPSM$ID_SUB, SPSM$GEOGRAFICO, SPSM$FECHA_INGRESO) VALUES (" + codigo + "," + cod_muno + ",'INSERT = '||TO_CHAR(SYSDATE,'DD/MM/YYYY HH24:MI')||' '||'" + Session["USUARIO"].ToString() + "')";
                        estado = dao.comando(sql);
                        if (estado == 0)
                            break;
                    }
                }

            }


            if (munDes.Rows.Count > 0)
            {
                for (int i = 0; i < munDes.Rows.Count; i++)
                {
                    cod_muno = Convert.ToInt32(munDes.Rows[i]["MUNICIPIO"]);
                    busca = buscaMunicipio(codigo, cod_muno);
                    if (busca == -1)
                        break;
                    else if (busca == 1)
                    {
                        sql = "UPDATE SCHE$SIPLAN20.SP20$SUB_MUNOS SET SPSM$RESTRICTIVA= 'S',  SPSM$FECHA_ELIMINA = 'BORRA = '||TO_CHAR(SYSDATE,'DD/MM/YYYY HH24:MI')||' '||'" + Session["USUARIO"].ToString() + "' WHERE SPSM$ID_SUB  = " + codigo + " AND SPSM$GEOGRAFICO = " + cod_muno;
                        estado = dao.comando(sql);
                        if (estado == 0)
                            break;
                    }
                }

            }


            /*for (int i = 0; i < gvTerritorio.VisibleRowCount; i++)
            {
                if (gvTerritorio.Selection.IsRowSelected(i))
                {
                    cod_muno = Convert.ToInt32(gvTerritorio.GetRowValues(i, "MUNICIPIO"));
                    busca = buscaMunicipio(codigo,cod_muno);
                    if (busca == -1)
                        break;
                    else if (busca == 0)
                    {
                        sql = "INSERT INTO SCHE$SIPLAN20.SP20$SUB_MUNOS (SPSM$ID_SUB, SPSM$GEOGRAFICO, SPSM$FECHA_INGRESO) VALUES (" + codigo + "," + cod_muno + ",'INSERT = '||TO_CHAR(SYSDATE,'DD/MM/YYYY HH24:MI')||' '||'" + Session["USUARIO"].ToString() + "')";
                        estado = dao.comando(sql);
                        if (estado == 0)
                            break;
                    }      
                    
                }
                else
                {
                    cod_muno = Convert.ToInt32(gvTerritorio.GetRowValues(i, "MUNICIPIO"));
                    busca = buscaMunicipio(codigo, cod_muno);
                    if (busca == -1)
                        break;
                    else if (busca == 1)
                    {
                        sql = "UPDATE SCHE$SIPLAN20.SP20$SUB_MUNOS SET SPSM$RESTRICTIVA= 'S',  SPSM$FECHA_ELIMINA = 'BORRA = '||TO_CHAR(SYSDATE,'DD/MM/YYYY HH24:MI')||' '||'" + Session["USUARIO"].ToString() + "' WHERE SPSM$ID_SUB  = " + codigo + " AND SPSM$GEOGRAFICO = " + cod_muno;
                        estado = dao.comando(sql);
                        if (estado == 0)
                            break;
                    }
                    
                }

            }
            */

            
        }

        protected int buscaMunicipio(int sub, int muno)
        {
            int busca = -1;

            if (sub != -1)
            {
                sql = "SELECT COUNT(*) CONTEO FROM SCHE$SIPLAN20.SP20$SUB_MUNOS WHERE SPSM$ID_SUB = "+sub+ " AND SPSM$RESTRICTIVA = 'N' AND SPSM$GEOGRAFICO = "+muno;
                estado = dao.consulta(sql);
                if (estado == 1)
                {
                    tabla = dao.tabla;
                    if (tabla.Rows.Count > 0)
                    {
                        if (Convert.ToInt32(tabla.Rows[0]["CONTEO"]) > 0)
                            busca = 1;
                        else
                            busca = 0;
                    }
                }
                else
                    busca = -1;
            }
            else
                busca = 0;

            return busca;
        }

        public int verificaMunos()
        {
            int retorna = 0;

            /*for (int i = 0; i < gvTerritorio.VisibleRowCount; i++)
            {
                if (gvTerritorio.Selection.IsRowSelected(i))
                {
                    retorna = 1;
                    break;
                }
                else
                    retorna = 0;
            }*/

            return retorna;
            
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            Response.Redirect("../Documentos/matrices_de_ vinculacion_SIPLAN_INSTITUCIONES.xlsx");
        }

        protected void btnProgramas_Click(object sender, EventArgs e)
        {
            btnPOA_Click(sender, e);
        }

        protected void gvPOMInsto_CommandButtonInitialize(object sender, ASPxGridViewCommandButtonEventArgs e)
        {
            if (e.VisibleIndex != -1)
            {
                int estado = Convert.ToInt32(gvPOMInsto.GetRowValues(e.VisibleIndex, "SPP$ABIERTO").ToString());

                if (Session["ROL"].ToString() == "ENTIDAD" || estado == 1)
                {

                    e.Visible = false;


                }



            }
        }

        protected void gvResInsto_CommandButtonInitialize(object sender, ASPxGridViewCommandButtonEventArgs e)
        {
            if (e.VisibleIndex != -1)
            {

                if (Session["ROL"].ToString() == "ENTIDAD" || Convert.ToInt32(Session["abierto"]) == 1)
                {

                    e.Visible = false;


                }

                cargaResInsto(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]));

            }

        }

        protected void gvPrograPresupuestario_CommandButtonInitialize(object sender, ASPxGridViewCommandButtonEventArgs e)
        {
            if (e.VisibleIndex != -1)
            {
                if (Session["ROL"].ToString() == "ENTIDAD" || Convert.ToInt32(Session["abierto"]) == 1)
                {

                    e.Visible = false;


                }



            }
        }

        protected void btnRegreso_Click(object sender, EventArgs e)
        {
            Response.Redirect("../modulos/frmModulos.aspx");
        }

        protected void btnSNIP_Click(object sender, EventArgs e)
        {
            DataTable periodo = new DataTable();
            int inicio = 0;
            int final = 0;
            int nivel = 0;
            string producto;
            Session["cargaSnip"] = 1;
            sql = "SELECT P.SPP$INICIO, P.SPP$FINAL FROM SCHE$SIPLAN20.SP20$PERIODO P INNER JOIN SCHE$SIPLAN20.SP20$POM PO ON P.SPP$ID_PERIODO = PO.SPPO$ID_PERIODO AND P.SPP$RESTRICTIVA = 'N' AND PO.SPPO$RESTRICTIVA = 'N' AND SPPO$ID_INSTITUCION = " + Convert.ToInt32(Session["insto"]);
            estado = dao.consulta(sql);
            if (estado == 1)
            {
                periodo = dao.tabla;
                inicio = Convert.ToInt32(periodo.Compute("min(SPP$INICIO)", string.Empty));
                final = Convert.ToInt32(periodo.Compute("max(SPP$FINAL)", string.Empty));

                if (rbProductos.SelectedIndex == 0)
                {
                    if (gvProdInsto.FocusedRowIndex != -1)
                    {
                        nivel = gvProdInsto.GetRowLevel(gvProdInsto.FocusedRowIndex);
                        producto = gvProdInsto.GetRowValues(gvProdInsto.FocusedRowIndex, "PRODUCTO").ToString();
                        Session["codProducto"] = Convert.ToInt32(gvProdInsto.GetRowValues(gvProdInsto.FocusedRowIndex, "SPPRO$ID_PRODUCTO").ToString());
                        Session["pom"] = Convert.ToInt32(gvPOMInsto.GetRowValues(gvPOMInsto.FocusedRowIndex, "SPPO$ID_POM").ToString());
                        Session["insto"] = Convert.ToInt32(gvPOMInsto.GetRowValues(gvPOMInsto.FocusedRowIndex, "SPPO$ID_INSTITUCION").ToString());
                        if (nivel == 0)
                        {
                            sql = "select s.SPPSUB$SNIP from sche$siplan20.SP20$SUB_PRODUCTO s inner join sche$siplan20.SP20$PRODUCTO p on p.SPPRO$ID_PRODUCTO = s.SPPSUB$ID_PRODUCTO and p.SPPRO$RESTRICTIVA = 'N' and s.SPPSUB$RESTRICTIVA = 'N' where s.SPPSUB$SNIP is not null and p.SPPRO$POM = " + Convert.ToInt32(Session["pom"]) + " and p.SPPRO$INSTO = " + Convert.ToInt32(Session["insto"]);
                            estado = dao.consulta(sql);
                            if (estado == 1)
                            {
                                tabla = dao.tabla;
                                if (tabla.Rows.Count > 0)
                                {
                                    sql = "SELECT a.proyecto, a.proyecto||'-'||a.nombre proyectoSNIP, b.sigla FROM SINIP.BP_PROYECTO_ID a, SINIP.ci_unidad_ejecutora b, SINIP.BP_ETAPAS_FINANCIAR PO  WHERE a.restrictiva  = 'N'  AND a.entidad = b.entidad AND a.unidad_ejecutora = b.unidad_ejecutora and a.ENTIDAD = " + Convert.ToInt32(Session["insto"]) + " AND a.PROYECTO = PO.PROYECTO AND PO.EJERCICIO BETWEEN " + inicio + " AND " + final + " and a.proyecto not in (";
                                    for (int i = 0; i < tabla.Rows.Count; i++)
                                    {
                                        if (i == 0)
                                            sql = sql + tabla.Rows[i]["SPPSUB$SNIP"];
                                        else
                                            sql = sql + "," + tabla.Rows[i]["SPPSUB$SNIP"];
                                    }
                                    sql = sql + ") group by a.proyecto, a.nombre, b.sigla order by a.proyecto asc";
                                }

                                else
                                    sql = "SELECT a.proyecto, a.proyecto||'-'||a.nombre proyectoSNIP, b.sigla FROM SINIP.BP_PROYECTO_ID a, SINIP.ci_unidad_ejecutora b, SINIP.BP_ETAPAS_FINANCIAR PO  WHERE a.restrictiva  = 'N'  AND a.entidad = b.entidad AND a.unidad_ejecutora = b.unidad_ejecutora and a.ENTIDAD = " + Convert.ToInt32(Session["insto"]) + " AND a.PROYECTO = PO.PROYECTO AND PO.EJERCICIO BETWEEN " + inicio + " AND " + final + " group by a.proyecto, a.nombre, b.sigla order by a.proyecto asc";

                                estado = dao.consulta(sql);
                                if (estado == 1)
                                {
                                    tabla = dao.tabla;
                                    Session["snip"] = tabla;
                                    gvSNIP.DataSource = tabla;
                                    gvSNIP.DataBind();
                                    lblproductoSNIP.Text = producto;
                                    MultiView1.ActiveViewIndex = 9;
                                }
                            }
                        }
                        else
                        {
                            mensaje = "Seleccione una fila la cual contenga un producto";
                            Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
                        }
                    }

                    else
                    {
                        mensaje = "Seleccione el producto al cual se van crear subproductos";
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
                    }
                }

                else if (rbProductos.SelectedIndex == 1)
                {

                    if (gvProdEstrategicos.FocusedRowIndex != -1)
                    {
                        nivel = gvProdEstrategicos.GetRowLevel(gvProdEstrategicos.FocusedRowIndex);
                        producto = gvProdEstrategicos.GetRowValues(gvProdEstrategicos.FocusedRowIndex, "PRODUCTO").ToString();
                        Session["codProducto"] = Convert.ToInt32(gvProdEstrategicos.GetRowValues(gvProdEstrategicos.FocusedRowIndex, "SPPRO$ID_PRODUCTO").ToString());
                        Session["pom"] = Convert.ToInt32(gvPOMInsto.GetRowValues(gvPOMInsto.FocusedRowIndex, "SPPO$ID_POM").ToString());
                        Session["insto"] = Convert.ToInt32(gvPOMInsto.GetRowValues(gvPOMInsto.FocusedRowIndex, "SPPO$ID_INSTITUCION").ToString());
                        if (nivel == 0)
                        {
                            sql = "select s.SPPSUB$SNIP from sche$siplan20.SP20$SUB_PRODUCTO s inner join sche$siplan20.SP20$PRODUCTO p on p.SPPRO$ID_PRODUCTO = s.SPPSUB$ID_PRODUCTO and p.SPPRO$RESTRICTIVA = 'N' and s.SPPSUB$RESTRICTIVA = 'N' where s.SPPSUB$SNIP is not null and p.SPPRO$POM = " + Convert.ToInt32(Session["pom"]) + " and p.SPPRO$INSTO = " + Convert.ToInt32(Session["insto"]);
                            estado = dao.consulta(sql);
                            if (estado == 1)
                            {
                                tabla = dao.tabla;
                                if (tabla.Rows.Count > 0)
                                {
                                    sql = "SELECT a.proyecto, a.proyecto||'-'||a.nombre proyectoSNIP, b.sigla FROM SINIP.BP_PROYECTO_ID a, SINIP.ci_unidad_ejecutora b, SINIP.BP_ETAPAS_FINANCIAR PO  WHERE a.restrictiva  = 'N'  AND a.entidad = b.entidad AND a.unidad_ejecutora = b.unidad_ejecutora and a.ENTIDAD = " + Convert.ToInt32(Session["insto"]) + " AND a.PROYECTO = PO.PROYECTO AND PO.EJERCICIO BETWEEN " + inicio + " AND " + final + " and a.proyecto not in (";
                                    for (int i = 0; i < tabla.Rows.Count; i++)
                                    {
                                        if (i == 0)
                                            sql = sql + tabla.Rows[i]["SPPSUB$SNIP"];
                                        else
                                            sql = sql + "," + tabla.Rows[i]["SPPSUB$SNIP"];
                                    }
                                    sql = sql + ") group by a.proyecto, a.nombre, b.sigla order by a.proyecto asc";
                                }

                                else
                                    sql = "SELECT a.proyecto, a.proyecto||'-'||a.nombre proyectoSNIP, b.sigla FROM SINIP.BP_PROYECTO_ID a, SINIP.ci_unidad_ejecutora b, SINIP.BP_ETAPAS_FINANCIAR PO  WHERE a.restrictiva  = 'N'  AND a.entidad = b.entidad AND a.unidad_ejecutora = b.unidad_ejecutora and a.ENTIDAD = " + Convert.ToInt32(Session["insto"]) + " AND a.PROYECTO = PO.PROYECTO AND PO.EJERCICIO BETWEEN " + inicio + " AND " + final + " group by a.proyecto, a.nombre, b.sigla order by a.proyecto asc";

                                estado = dao.consulta(sql);
                                if (estado == 1)
                                {
                                    tabla = dao.tabla;
                                    Session["snip"] = tabla;
                                    gvSNIP.DataSource = tabla;
                                    gvSNIP.DataBind();
                                    lblproductoSNIP.Text = producto;
                                    MultiView1.ActiveViewIndex = 9;
                                }
                            }

                        }
                        else
                        {
                            mensaje = "Seleccione una fila la cual contenga un producto";
                            Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
                        }
                    }

                    else
                    {
                        mensaje = "Seleccione el producto al cual se van crear subproductos";
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
                    }

                }





            }
        }

        protected void btnGrabaSNIP_Click(object sender, EventArgs e)
        {
            int SNIP = 0;
            for (int i = 0; i < gvSNIP.VisibleRowCount; i++)
            {

                if (gvSNIP.Selection.IsRowSelected(i))
                {
                    SNIP = Convert.ToInt32(gvSNIP.GetRowValues(i, "PROYECTO"));
                    sql = "INSERT INTO SCHE$SIPLAN20.SP20$SUB_PRODUCTO (SPPSUB$SNIP, SPPSUB$ID_PRODUCTO, SPPSUB$FECHA_INSERTA, SPPSUB$PROPIETARIO) VALUES (" + SNIP + ", " + Session["codProducto"] + ", 'INSERT = '||TO_CHAR(SYSDATE,'DD/MM/YYYY HH24:MI')||' '||'" + Session["USUARIO"].ToString() + "', '" + Session["USUARIO"].ToString() + "')";
                    estado = dao.comando(sql);
                    /*if (estado == 0)
                        break;*/

                }

            }

            if (estado == 1)
            {
                Session["cargaSnip"] = null;
                Session["snip"] = null;
                mensaje = "Subproductos registrado correctamente";
                Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',1);", true);
                MultiView1.ActiveViewIndex = 6;

            }
            else
            {
                Session["cargaSnip"] = null;
                Session["snip"] = null;

                mensaje = dao.mensaje;
                Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
                MultiView1.ActiveViewIndex = 6;
            }

        }

        protected void btnCancelaSNIP_Click(object sender, EventArgs e)
        {
            Session["cargaSnip"] = null;
            Session["snip"] = null;
            MultiView1.ActiveViewIndex = 6;
        }

        protected void gvPOMInsto_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
        {
            int valor = 0;
            valor = Convert.ToInt32(e.GetValue("CODESTADO"));
            if (valor == 1)

                e.Row.ForeColor = System.Drawing.Color.Blue;
            //e.Row.BackColor = System.Drawing.Color.MediumSeaGreen;
        }

        protected void btnAbrir_Click(object sender, EventArgs e)
        {
            DateTime fechaCierre = new DateTime(2015, 1, 1, 0, 0, 0);
            if (gvPOMInsto.FocusedRowIndex != -1)
            {
                Session["pom"] = Convert.ToInt32(gvPOMInsto.GetRowValues(gvPOMInsto.FocusedRowIndex, "SPPO$ID_POM").ToString());
                Session["insto"] = Convert.ToInt32(gvPOMInsto.GetRowValues(gvPOMInsto.FocusedRowIndex, "SPPO$ID_INSTITUCION").ToString());
                sql = "SELECT TO_CHAR(SPFC$FECHA_CIERRE,'DD/MM/YYYY') SPFC$FECHA_CIERRE FROM SCHE$SIPLAN20.SP20$FECHAS_CIERRE WHERE SPFC$PERIODO_POM  = " + Convert.ToInt32(gvPOMInsto.GetRowValues(gvPOMInsto.FocusedRowIndex, "SPPO$ID_PERIODO").ToString()) + " AND SPFC$RESTRICTIVA = 'N' AND SPFC$TIPO_FECHA = 2 AND SPFC$EJERCICIO = " + fecha.Year + " AND SPFC$POM = -1 AND SPFC$INSTITUCION = -1";
                estado = dao.consulta(sql);
                if (estado == 1)
                {
                    tabla = dao.tabla;
                    if (tabla.Rows.Count > 0)
                    {
                        if (tabla.Rows[0]["SPFC$FECHA_CIERRE"] != DBNull.Value)
                        {
                            fechaCierre = DateTime.ParseExact(tabla.Rows[0]["SPFC$FECHA_CIERRE"].ToString(), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                            if (fecha <= fechaCierre)
                            {
                                /*mensaje = "No es posible realizar una prorroga para esta programación, el periodo para la modificación ";
                                Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
                                cargaPOMS();*/
                                periodoPOM.Text = gvPOMInsto.GetRowValues(gvPOMInsto.FocusedRowIndex, "SPPO$INICIO").ToString() + "-" + gvPOMInsto.GetRowValues(gvPOMInsto.FocusedRowIndex, "SPPO$FINAL").ToString();
                                instoPOM.Text = gvPOMInsto.GetRowValues(gvPOMInsto.FocusedRowIndex, "NOMBRE").ToString();
                                PERIODO.Value = gvPOMInsto.GetRowValues(gvPOMInsto.FocusedRowIndex, "SPPO$ID_PERIODO").ToString();
                                POM.Value = gvPOMInsto.GetRowValues(gvPOMInsto.FocusedRowIndex, "SPPO$ID_POM").ToString();
                                INSTO.Value = gvPOMInsto.GetRowValues(gvPOMInsto.FocusedRowIndex, "SPPO$ID_INSTITUCION").ToString();
                                ABIERTO.Value = "1";
                                txtdateFecha.Enabled = false;
                                txtFechaPorroga.Enabled = false;
                                txtdateFecha.Date = DateTime.ParseExact(tabla.Rows[0]["SPFC$FECHA_CIERRE"].ToString(), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                                lbAbierto.Text = "La fecha de cierre para este periodo aun no esta vigente, favor de confirmar la habilitación de la programación, presionando el boton 'Guardar fecha prorroga'";
                                poProrroga.ShowOnPageLoad = true;
                            }

                            else
                            {
                                ABIERTO.Value = "0";
                                lbAbierto.Text = "";
                                txtdateFecha.Enabled = true;
                                txtFechaPorroga.Enabled = true;
                                txtdateFecha.EditFormat = DevExpress.Web.EditFormat.Custom;
                                txtdateFecha.EditFormatString = "dd/MM/yyyy";
                                txtdateFecha.DisplayFormatString = "dd/MM/yyyy";
                                txtdateFecha.MinDate = new DateTime(1900, 01, 01);
                                txtdateFecha.MaxDate = new DateTime(2100, 12, 31);
                                txtdateFecha.Date = DateTime.ParseExact(tabla.Rows[0]["SPFC$FECHA_CIERRE"].ToString(), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                                txtdateFecha.Enabled = false;

                                sql = "SELECT TO_CHAR(MAX(SPFC$FECHA_CIERRE),'DD/MM/YYYY') FECHA_PRORROGA FROM SCHE$SIPLAN20.SP20$FECHAS_CIERRE WHERE SPFC$RESTRICTIVA = 'N' AND SPFC$CUATRIMESTRE = 0 AND SPFC$EJERCICIO = " + fecha.Year + " AND SPFC$TIPO_FECHA = 2 AND SPFC$INSTITUCION = " + Convert.ToInt32(gvPOMInsto.GetRowValues(gvPOMInsto.FocusedRowIndex, "SPPO$ID_INSTITUCION").ToString()) + " AND SPFC$PERIODO_POM = " + gvPOMInsto.GetRowValues(gvPOMInsto.FocusedRowIndex, "SPPO$ID_PERIODO").ToString() + " AND SPFC$POM = " + gvPOMInsto.GetRowValues(gvPOMInsto.FocusedRowIndex, "SPPO$ID_POM").ToString();

                                estado = dao.consulta(sql);
                                if (estado == 1)
                                    tabla = dao.tabla;
                                if (tabla.Rows.Count > 0)
                                {
                                    if (tabla.Rows[0]["FECHA_PRORROGA"] != DBNull.Value)
                                    {
                                        txtFechaPorroga.EditFormat = DevExpress.Web.EditFormat.Custom;
                                        txtFechaPorroga.EditFormatString = "dd/MM/yyyy";
                                        txtFechaPorroga.DisplayFormatString = "dd/MM/yyyy";
                                        txtFechaPorroga.MinDate = new DateTime(1900, 01, 01);
                                        txtFechaPorroga.MaxDate = new DateTime(2100, 12, 31);
                                        txtFechaPorroga.Date = DateTime.ParseExact(tabla.Rows[0]["FECHA_PRORROGA"].ToString(), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture); ;
                                    }
                                    else
                                    {
                                        txtFechaPorroga.EditFormat = DevExpress.Web.EditFormat.Custom;
                                        txtFechaPorroga.EditFormatString = "dd/MM/yyyy";
                                        txtFechaPorroga.DisplayFormatString = "dd/MM/yyyy";
                                        txtFechaPorroga.MinDate = new DateTime(1900, 01, 01);
                                        txtFechaPorroga.MaxDate = new DateTime(2100, 12, 31);
                                        txtFechaPorroga.Date = fecha;
                                    }
                                }

                                else
                                {
                                    txtFechaPorroga.EditFormat = DevExpress.Web.EditFormat.Custom;
                                    txtFechaPorroga.EditFormatString = "dd/MM/yyyy";
                                    txtFechaPorroga.DisplayFormatString = "dd/MM/yyyy";
                                    txtFechaPorroga.MinDate = new DateTime(1900, 01, 01);
                                    txtFechaPorroga.MaxDate = new DateTime(2100, 12, 31);
                                    txtFechaPorroga.Date = fecha;
                                }



                                periodoPOM.Text = gvPOMInsto.GetRowValues(gvPOMInsto.FocusedRowIndex, "SPPO$INICIO").ToString() + "-" + gvPOMInsto.GetRowValues(gvPOMInsto.FocusedRowIndex, "SPPO$FINAL").ToString();
                                instoPOM.Text = gvPOMInsto.GetRowValues(gvPOMInsto.FocusedRowIndex, "NOMBRE").ToString();
                                PERIODO.Value = gvPOMInsto.GetRowValues(gvPOMInsto.FocusedRowIndex, "SPPO$ID_PERIODO").ToString();
                                POM.Value = gvPOMInsto.GetRowValues(gvPOMInsto.FocusedRowIndex, "SPPO$ID_POM").ToString();
                                INSTO.Value = gvPOMInsto.GetRowValues(gvPOMInsto.FocusedRowIndex, "SPPO$ID_INSTITUCION").ToString();

                                poProrroga.ShowOnPageLoad = true;
                            }
                        }

                        else
                        {
                            mensaje = "No es posible realizar una prorroga para esta programación, no se ha encontrado una fecha de cierra definida";
                            Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
                            cargaPOMS();
                        }
                    }
                    else
                    {
                        mensaje = "No es posible realizar una prorroga para esta programación, no se ha encontrado una fecha de cierra definida";
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
                        cargaPOMS();
                    }

                }
                else
                {
                    mensaje = dao.mensaje;
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
                    cargaPOMS();
                }
                /* if (Convert.ToInt32(gvPOMInsto.GetRowValues(gvPOMInsto.FocusedRowIndex, "CODESTADO").ToString()) == 1)
                 {

                     Session["pom"] = Convert.ToInt32(gvPOMInsto.GetRowValues(gvPOMInsto.FocusedRowIndex, "SPPO$ID_POM").ToString());
                     Session["insto"] = Convert.ToInt32(gvPOMInsto.GetRowValues(gvPOMInsto.FocusedRowIndex, "SPPO$ID_INSTITUCION").ToString());

                     sql = "UPDATE SCHE$SIPLAN20.SP20$POM SET SPPO$ESTADO = 'D', SPPO$FECHA_UPDATE = 'ABRE = ' || TO_CHAR(SYSDATE, 'DD/MM/YYYY HH24:MI') || ' ' || '" + Session["USUARIO"].ToString() + "'  WHERE SPPO$ID_POM  = " + Convert.ToInt32(gvPOMInsto.GetRowValues(gvPOMInsto.FocusedRowIndex, "SPPO$ID_POM").ToString());
                     estado = dao.comando(sql);
                     if (estado == 1)
                     {
                         sql = "UPDATE SCHE$SIPLAN20.SP20$POA SET SPOA$ESTADO = 'D', SPOA$FECHA_APROBACION = 'ABRE = ' || TO_CHAR(SYSDATE, 'DD/MM/YYYY HH24:MI') || ' ' || '" + Session["USUARIO"].ToString() + "'  WHERE SPOA$ID_POM  = " + Convert.ToInt32(gvPOMInsto.GetRowValues(gvPOMInsto.FocusedRowIndex, "SPPO$ID_POM").ToString());
                         estado = dao.comando(sql);
                         if (estado == 1)
                         {
                             mensaje = "La programación ha sido abierta para actualizaciones de metas fisicas y financieras";
                             Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',1);", true);
                             cargaPOMS();
                         }
                         else
                         {
                             mensaje = dao.mensaje;
                             Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
                             cargaPOMS();
                         }

                     }

                     else
                     {
                         mensaje = dao.mensaje;
                         Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
                         cargaPOMS();
                     }

                 }

                 else
                 {
                     mensaje = "Esta programación ya se encuentra enviada, no es necesario habilitarla";
                     Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',1);", true);
                 }*/
            }




        }

        protected void btnPGG_Click(object sender, EventArgs e)
        {
            if (Session["pom"] != null && Session["insto"] != null)
            {
                cargaProdutosTemporal(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]));
                MultiView1.ActiveViewIndex = 10;

            }

        }

        protected void btnVinculaPGG2425_Click(object sender, EventArgs e)
        {
            if (Session["pom"] != null && Session["insto"] != null)
            {
                //sql = "SELECT * FROM SCHE$SIPLAN20.SPPSVST$PGG2024_2028 WHERE RESPONSABLE = "+ Session["insto"];
                sql = @"SELECT 
                        ID_ACCION, EJE, META, ACCION_ESTRATEGICA
                        FROM
                        (
                            SELECT ID_ACCION
                            ,EJE
                            ,META
                            ,ACCION_ESTRATEGICA
                            ,ID_EJE FROM SCHE$SIPLAN20.SPPSVST$PGG2024_2028 WHERE RESPONSABLE = " + Session["insto"] + @"
                            UNION
                            SELECT ID_ACCION
                            ,EJE
                            ,META
                            ,ACCION_ESTRATEGICA
                            ,ID_EJE FROM SCHE$SIPLAN20.SPPSVST$PGG2024_2028 INNER JOIN SCHE$SIPLAN20.SP20$RESPONSABLESPGG RG ON RG.SPRPG$ID_PGG = ID_ACCION AND SPRPG$NIVEL = 3 AND SPRPG$RESPONSABLE = " + Session["insto"] + @"

                            UNION
                                                    SELECT
                                                    RG.SPRPG$ID_PGG ID_ACCION
                                                    , R.SPPRES$DESCRIPCION EJE
                                                    ,R.SPPRES$DESCRIPCION  META
                                                    ,R.SPPRES$DESCRIPCION ACCION_ESTRATEGICA
                                                    ,RG.SPRPG$ID_PGG ID_EJE
                                                    FROM SCHE$SIPLAN20.SP20$RESULTADOS_ESTRATEGICOS R
                                                    INNER JOIN SCHE$SIPLAN20.SP20$RESPONSABLESPGG RG ON RG.SPRPG$ID_PGG = R.SPPRES$ID_RESULTADO AND RG.SPRPG$NIVEL = 1 AND R.SPPRES$RESTRICTIVA = 'N' AND RG.SPRPG$RESTRICTIVA = 'N'
                                                    WHERE RG.SPRPG$RESPONSABLE = " + Session["insto"] + @"



                        )

                    ORDER BY ID_EJE
                    ,ID_ACCION
                    ASC";
                estado = dao.consulta(sql);
                if (estado == 1)
                {
                    tabla = dao.tabla;
                    if (tabla.Rows.Count > 0)
                    {
                        cargaResultadoInstoTempo(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]));
                        cargaPGG2428(Convert.ToInt32(Session["insto"]));
                        popPGG2428.ShowOnPageLoad = true;
                    }
                    else
                    {
                        cargaResultado1EJE(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]));
                        carga1eje();

                        pop1eje.ShowOnPageLoad = true;
                    }
                }

                else
                {
                    mensaje = dao.mensaje;
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
                }



            }
        }

        protected void btnRegresaProdInsto_Click(object sender, EventArgs e)
        {
            MultiView1.ActiveViewIndex = 4;
        }

        protected void cargaProdutosTemporal(int pom, int insto)
        {

            /*sql = @"SELECT
SPRES$ID_RESULTADO
,SPPRO$RESULTADO2
,NIVEL
,RESULTADO 
,CASE WHEN RESULTA2 != 'S/N' THEN RESULTA2 WHEN RESULTA2 = 'S/N' THEN (SELECT 
CASE WHEN NIVEL2 = 3 THEN 'ACCION ESTRATÉGICA '||ES.SPPRES$DESCRIPCION WHEN NIVEL2 = 1 THEN 'EJE ESTRATÉGICO '||ES.SPPRES$DESCRIPCION END AS RESULTADO2 FROM
SCHE$SIPLAN20.SP20$RESULTADOS_ESTRATEGICOS ES WHERE SPPRES$ID_RESULTADO = CODESTRA2 AND SPPRES$RESTRICTIVA = 'N') END AS RESULTADO2
,SPPRO$ID_PRODUCTO
,SPPRO$DESCRIPCION
,MEDIDA
,SPPRO$TEMPORAL
,SPRES$TIPO
,CODESTRA2
,SPRES$COD_ESTRATEGICO
FROM
(SELECT 
SPRES$ID_RESULTADO
,SPPRO$RESULTADO2
,NIVEL
,RESULTADO
,SPPRO$ID_PRODUCTO
,SPPRO$DESCRIPCION
,MEDIDA
,SPPRO$TEMPORAL
,SPRES$TIPO
,CODESTRA2
,RESULTA2 
,CASE WHEN CODESTRA2 != -1 THEN (SELECT SPPRES$NIVEL FROM SCHE$SIPLAN20.SP20$RESULTADOS_ESTRATEGICOS WHERE SPPRES$ID_RESULTADO = CODESTRA2 AND SPPRES$RESTRICTIVA = 'N') ELSE -1 END AS NIVEL2
,SPRES$COD_ESTRATEGICO
FROM

(SELECT
 SPRES$ID_RESULTADO
 ,SPRES$COD_ESTRATEGICO
 ,NIVEL
 ,CASE WHEN NIVEL = -1 AND SPRES$TIPO = 1 THEN RESULTADO WHEN SPRES$TIPO = 2 THEN RED  ELSE RESPGG END AS RESULTADO
 ,SPPRO$ID_PRODUCTO
 ,SPPRO$DESCRIPCION
 ,MEDIDA
 ,SPPRO$TEMPORAL
 ,SPRES$TIPO
 ,CASE WHEN SPRES$TIPO = 1 AND SPPRO$RESULTADO2 != -1 THEN (SELECT SPRES$COD_ESTRATEGICO FROM SCHE$SIPLAN20.SP20$RESULTADOS WHERE SPRES$ID_RESULTADO = SPPRO$RESULTADO2 AND SPRES$RESTRICTIVA = 'N') ELSE -1 END AS CODESTRA2
 ,CASE WHEN SPRES$TIPO = 0 AND SPPRO$RESULTADO2 != -1 THEN (SELECT SPRES$DESCRIPCION FROM SCHE$SIPLAN20.SP20$RESULTADOS WHERE SPRES$ID_RESULTADO = SPPRO$RESULTADO2 AND SPRES$RESTRICTIVA = 'N') ELSE 'S/N' END AS RESULTA2 
  ,SPPRO$RESULTADO2
FROM
                      (SELECT
                        SPRES$ID_RESULTADO
                        
                        , NVL((SELECT R.SPPRES$NIVEL FROM SCHE$SIPLAN20.SP20$RESULTADOS_ESTRATEGICOS R WHERE R.SPPRES$ID_RESULTADO = SPRES$COD_ESTRATEGICO AND SPRES$TIPO = 0), -1) NIVEL
                        ,(SELECT R.SPPRES$DESC_NIVEL || ' ' || R.SPPRES$CODIGO || ' ' || R.SPPRES$DESCRIPCION FROM SCHE$SIPLAN20.SP20$RESULTADOS_ESTRATEGICOS R WHERE R.SPPRES$ID_RESULTADO = SPRES$COD_ESTRATEGICO) RESPGG
                        ,RESULTADO
                        ,ID_RED
                        ,RED
                        ,SPPRO$ID_PRODUCTO
                        ,SPPRO$DESCRIPCION
                        ,MEDIDA
                        ,SPPRO$TEMPORAL
                        ,SPRES$TIPO
                        ,SPPRO$RESULTADO2
                        ,SPRES$COD_ESTRATEGICO
                        FROM
                        (SELECT R.SPRES$ID_RESULTADO
                        , NVL(R.SPRES$COD_ESTRATEGICO, -1)  SPRES$COD_ESTRATEGICO
                        , CASE WHEN R.SPRES$COD_ESTRATEGICO IS NOT NULL  AND R.SPRES$TIPO = 0 THEN 'PRODUCTO VINCULADO A PGG 2020-2024' ELSE R.SPRES$DESCRIPCION END AS RESULTADO
                        ,(SELECT SPPRED$ID FROM SCHE$SIPLAN20.SP20$RED WHERE SPPRED$ID = R.SPRES$COD_ESTRATEGICO AND R.SPRES$TIPO = 2 AND SPPRED$RESTRICTIVA = 'N') ID_RED
                        ,(SELECT SPPRED$RED||' '||SPPRED$DESCRIPCION FROM SCHE$SIPLAN20.SP20$RED WHERE SPPRED$ID = R.SPRES$COD_ESTRATEGICO AND R.SPRES$TIPO = 2 AND SPPRED$RESTRICTIVA = 'N') RED
                        , P.SPPRO$ID_PRODUCTO
                        , P.SPPRO$DESCRIPCION
                        , M.NOMBRE MEDIDA
                        , P.SPPRO$TEMPORAL
                        , R.SPRES$TIPO
                        ,P.SPPRO$RESULTADO2
                        FROM SCHE$SIPLAN20.SP20$RESULTADOS R
                        INNER JOIN SCHE$SIPLAN20.SP20$PRODUCTO P ON P.SPPRO$ID_RESULTADO = R.SPRES$ID_RESULTADO AND P.SPPRO$RESTRICTIVA = 'N' AND R.SPRES$RESTRICTIVA = 'N'
                        INNER JOIN SINIP.CP_UNIDADES_MEDIDA M ON M.UNIDAD_MEDIDA = P.SPPRO$ID_MEDIDA
                        WHERE R.SPRES$POM = "+pom+" AND R.SPRES$INSTITUCION = "+insto+@" AND R.SPRES$RESTRICTIVA = 'N')
                      
                        )))
                        ORDER BY SPRES$COD_ESTRATEGICO DESC, SPRES$ID_RESULTADO,  SPPRO$ID_PRODUCTO ASC";
            */
            sql = @"SELECT
SPRES$ID_RESULTADO
,SPPRO$RESULTADO2
,NIVEL
,RESULTADO 
,CASE WHEN RESULTA2 != 'S/N' THEN RESULTA2 WHEN RESULTA2 = 'S/N' THEN (SELECT 
CASE WHEN NIVEL2 = 3 THEN 'ACCION ESTRATÉGICA '||ES.SPPRES$DESCRIPCION WHEN NIVEL2 = 1 THEN 'EJE ESTRATÉGICO '||ES.SPPRES$DESCRIPCION END AS RESULTADO2 FROM
SCHE$SIPLAN20.SP20$RESULTADOS_ESTRATEGICOS ES WHERE SPPRES$ID_RESULTADO = CODESTRA2 AND SPPRES$RESTRICTIVA = 'N') END AS RESULTADO2
,SPPRO$ID_PRODUCTO
,SPPRO$DESCRIPCION
,MEDIDA
,SPPRO$TEMPORAL
,SPRES$TIPO
,CODESTRA2
,SPRES$COD_ESTRATEGICO
FROM
(SELECT 
SPRES$ID_RESULTADO
,SPPRO$RESULTADO2
,NIVEL
,RESULTADO
,SPPRO$ID_PRODUCTO
,SPPRO$DESCRIPCION
,MEDIDA
,SPPRO$TEMPORAL
,SPRES$TIPO
,CODESTRA2
,RESULTA2 
,CASE WHEN CODESTRA2 != -1 THEN (SELECT SPPRES$NIVEL FROM SCHE$SIPLAN20.SP20$RESULTADOS_ESTRATEGICOS WHERE SPPRES$ID_RESULTADO = CODESTRA2 AND SPPRES$RESTRICTIVA = 'N') ELSE -1 END AS NIVEL2
,SPRES$COD_ESTRATEGICO
FROM

(SELECT
 SPRES$ID_RESULTADO
 ,SPRES$COD_ESTRATEGICO
 ,NIVEL
 ,CASE WHEN NIVEL = -1 AND SPRES$TIPO = 1 THEN RESULTADO WHEN SPRES$TIPO = 2 THEN RED  ELSE RESPGG END AS RESULTADO
 ,SPPRO$ID_PRODUCTO
 ,SPPRO$DESCRIPCION
 ,MEDIDA
 ,SPPRO$TEMPORAL
 ,SPRES$TIPO
 ,CASE WHEN SPRES$TIPO = 1 AND SPPRO$RESULTADO2 != -1 THEN (SELECT SPRES$COD_ESTRATEGICO FROM SCHE$SIPLAN20.SP20$RESULTADOS WHERE SPRES$ID_RESULTADO = SPPRO$RESULTADO2 AND SPRES$RESTRICTIVA = 'N') WHEN SPRES$TIPO = 2 AND SPPRO$RESULTADO2 != -1 THEN (SELECT SPRES$COD_ESTRATEGICO FROM SCHE$SIPLAN20.SP20$RESULTADOS WHERE SPRES$ID_RESULTADO = SPPRO$RESULTADO2 AND SPRES$RESTRICTIVA = 'N') ELSE -1 END AS CODESTRA2
 ,CASE WHEN SPRES$TIPO = 0 AND SPPRO$RESULTADO2 != -1 THEN (SELECT SPRES$DESCRIPCION FROM SCHE$SIPLAN20.SP20$RESULTADOS WHERE SPRES$ID_RESULTADO = SPPRO$RESULTADO2 AND SPRES$RESTRICTIVA = 'N') ELSE 'S/N' END AS RESULTA2 
  ,SPPRO$RESULTADO2
FROM
                      (SELECT
                        SPRES$ID_RESULTADO
                        
                        , NVL((SELECT R.SPPRES$NIVEL FROM SCHE$SIPLAN20.SP20$RESULTADOS_ESTRATEGICOS R WHERE R.SPPRES$ID_RESULTADO = SPRES$COD_ESTRATEGICO AND SPRES$TIPO = 0), -1) NIVEL
                        ,(SELECT R.SPPRES$DESC_NIVEL || ' ' || R.SPPRES$CODIGO || ' ' || R.SPPRES$DESCRIPCION FROM SCHE$SIPLAN20.SP20$RESULTADOS_ESTRATEGICOS R WHERE R.SPPRES$ID_RESULTADO = SPRES$COD_ESTRATEGICO) RESPGG
                        ,RESULTADO
                        ,ID_RED
                        ,RED
                        ,SPPRO$ID_PRODUCTO
                        ,SPPRO$DESCRIPCION
                        ,MEDIDA
                        ,SPPRO$TEMPORAL
                        ,SPRES$TIPO
                        ,SPPRO$RESULTADO2
                        ,SPRES$COD_ESTRATEGICO
                        FROM
                        (SELECT R.SPRES$ID_RESULTADO
                        , NVL(R.SPRES$COD_ESTRATEGICO, -1)  SPRES$COD_ESTRATEGICO
                        , CASE WHEN R.SPRES$COD_ESTRATEGICO IS NOT NULL  AND R.SPRES$TIPO = 0 THEN 'PRODUCTO VINCULADO A PGG 2020-2024' ELSE R.SPRES$DESCRIPCION END AS RESULTADO
                        ,(SELECT SPPRED$ID FROM SCHE$SIPLAN20.SP20$RED WHERE SPPRED$ID = R.SPRES$COD_ESTRATEGICO AND R.SPRES$TIPO = 2 AND SPPRED$RESTRICTIVA = 'N') ID_RED
                        ,(SELECT SPPRED$RED||' '||SPPRED$DESCRIPCION FROM SCHE$SIPLAN20.SP20$RED WHERE SPPRED$ID = R.SPRES$COD_ESTRATEGICO AND R.SPRES$TIPO = 2 AND SPPRED$RESTRICTIVA = 'N') RED
                        
                        , P.SPPRO$ID_PRODUCTO
                        , P.SPPRO$DESCRIPCION
                        , M.NOMBRE MEDIDA
                        , P.SPPRO$TEMPORAL
                        , R.SPRES$TIPO
                        ,P.SPPRO$RESULTADO2
                        FROM SCHE$SIPLAN20.SP20$RESULTADOS R
                        INNER JOIN SCHE$SIPLAN20.SP20$PRODUCTO P ON P.SPPRO$ID_RESULTADO = R.SPRES$ID_RESULTADO AND P.SPPRO$RESTRICTIVA = 'N' AND R.SPRES$RESTRICTIVA = 'N'
                        INNER JOIN SINIP.CP_UNIDADES_MEDIDA M ON M.UNIDAD_MEDIDA = P.SPPRO$ID_MEDIDA
                        WHERE R.SPRES$POM = " + pom + " AND R.SPRES$INSTITUCION = " + insto + @" AND R.SPRES$RESTRICTIVA = 'N')
                      
                        )))
                        ORDER BY SPRES$COD_ESTRATEGICO DESC, SPRES$ID_RESULTADO,  SPPRO$ID_PRODUCTO ASC";
            estado = dao.consulta(sql);
            if (estado == 1)
            {
                tabla = dao.tabla;
                if (tabla.Rows.Count > 0)
                {
                    Session["temporales"] = tabla;
                    gvProduccionInsto.DataSource = tabla;
                    gvProduccionInsto.DataBind();


                }
            }
            else
            {
                mensaje = dao.mensaje;
                Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);

            }
        }
        protected void cargaPGG2428(int insto)
        {
            //sql = "SELECT ID_ACCION, EJE, META, ACCION_ESTRATEGICA FROM SCHE$SIPLAN20.SPPSVST$PGG2024_2028 WHERE RESPONSABLE = " + insto;
            sql = @"SELECT 
                    ID_ACCION
                    ,EJE
                    ,META
                    ,ACCION_ESTRATEGICA
                    FROM
                    ( 
                        SELECT ID_ACCION
                                ,EJE
                                ,META
                                ,ACCION_ESTRATEGICA
                                ,ID_EJE FROM SCHE$SIPLAN20.SPPSVST$PGG2024_2028 WHERE RESPONSABLE = " + insto + @"
                                UNION
                                SELECT ID_ACCION
                                ,EJE
                                ,META
                                ,ACCION_ESTRATEGICA
                                ,ID_EJE FROM SCHE$SIPLAN20.SPPSVST$PGG2024_2028 INNER JOIN SCHE$SIPLAN20.SP20$RESPONSABLESPGG RG ON RG.SPRPG$ID_PGG = ID_ACCION AND SPRPG$NIVEL = 3 AND SPRPG$RESPONSABLE = " + insto + @"

                                UNION

                                SELECT
                                                    RG.SPRPG$ID_PGG ID_ACCION
                                                    , R.SPPRES$DESCRIPCION EJE
                                                    ,R.SPPRES$DESCRIPCION  META
                                                    ,R.SPPRES$DESCRIPCION ACCION_ESTRATEGICA
                                                    ,RG.SPRPG$ID_PGG ID_EJE
                                                    FROM SCHE$SIPLAN20.SP20$RESULTADOS_ESTRATEGICOS R
                                                    INNER JOIN SCHE$SIPLAN20.SP20$RESPONSABLESPGG RG ON RG.SPRPG$ID_PGG = R.SPPRES$ID_RESULTADO AND RG.SPRPG$NIVEL = 1 AND R.SPPRES$RESTRICTIVA = 'N' AND RG.SPRPG$RESTRICTIVA = 'N'
                                                    WHERE RG.SPRPG$RESPONSABLE = " + insto + @" 


                )

                ORDER BY ID_EJE
                ,ID_ACCION
                ASC";
            estado = dao.consulta(sql);
            if (estado == 1)
                tabla = dao.tabla;
            else
            {
                mensaje = dao.mensaje;
                Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',1);", true);
            }

            if (tabla.Rows.Count == 0)
            {
                sql = "SELECT ID_ACCION, EJE, META, ACCION_ESTRATEGICA FROM SCHE$SIPLAN20.SPPSVST$PGG2024_2028";
                estado = dao.consulta(sql);
                if (estado == 1)
                {
                    tabla = dao.tabla;
                }
                else
                {
                    mensaje = dao.mensaje;
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
                }

            }

            gvPGG2428.DataSource = tabla;
            gvPGG2428.DataBind();
            gvPGG2428.ExpandAll();

        }


        protected void cargaPGG24282(int insto)
        {
            //sql = "SELECT ID_ACCION, EJE, META, ACCION_ESTRATEGICA FROM SCHE$SIPLAN20.SPPSVST$PGG2024_2028 WHERE RESPONSABLE = " + insto;

            sql = @"SELECT 
                    ID_ACCION
                    ,EJE
                    ,META
                    ,ACCION_ESTRATEGICA
                    FROM
                    ( 
                        SELECT ID_ACCION
                                ,EJE
                                ,META
                                ,ACCION_ESTRATEGICA
                                ,ID_EJE FROM SCHE$SIPLAN20.SPPSVST$PGG2024_2028 WHERE RESPONSABLE = " + insto + @"
                                UNION
                                SELECT ID_ACCION
                                ,EJE
                                ,META
                                ,ACCION_ESTRATEGICA
                                ,ID_EJE FROM SCHE$SIPLAN20.SPPSVST$PGG2024_2028 INNER JOIN SCHE$SIPLAN20.SP20$RESPONSABLESPGG RG ON RG.SPRPG$ID_PGG = ID_ACCION AND SPRPG$NIVEL = 3 AND SPRPG$RESPONSABLE = " + insto + @"
                                
                                UNION
                                 SELECT 
                                 RG.SPRPG$ID_PGG ID_ACCION
                                                    , R.SPPRES$DESCRIPCION EJE
                                                    ,R.SPPRES$DESCRIPCION  META
                                                    ,R.SPPRES$DESCRIPCION ACCION_ESTRATEGICA
                                                    ,RG.SPRPG$ID_PGG ID_EJE
                                                    FROM SCHE$SIPLAN20.SP20$RESULTADOS_ESTRATEGICOS R
                                                    INNER JOIN SCHE$SIPLAN20.SP20$RESPONSABLESPGG RG ON RG.SPRPG$ID_PGG = R.SPPRES$ID_RESULTADO AND RG.SPRPG$NIVEL = 1 AND R.SPPRES$RESTRICTIVA = 'N' AND RG.SPRPG$RESTRICTIVA = 'N'
                                                    WHERE RG.SPRPG$RESPONSABLE = " + insto + @"
                )

                ORDER BY ID_EJE
                ,ID_ACCION
                ASC";
            estado = dao.consulta(sql);
            if (estado == 1)
                tabla = dao.tabla;
            else
            {
                mensaje = dao.mensaje;
                Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',1);", true);
            }

            if (tabla.Rows.Count == 0)
            {
                sql = "SELECT ID_ACCION, EJE, META, ACCION_ESTRATEGICA FROM SCHE$SIPLAN20.SPPSVST$PGG2024_2028";
                estado = dao.consulta(sql);
                if (estado == 1)
                {
                    tabla = dao.tabla;
                }
                else
                {
                    mensaje = dao.mensaje;
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
                }

            }

            gv2428red.DataSource = tabla;
            gv2428red.DataBind();
            gv2428red.ExpandAll();

        }



        protected void carga1eje()
        {
            sql = @"SELECT 
                    ID_EJE
                    ,EJE
                    FROM
                    (SELECT ID_EJE, EJE FROM SCHE$SIPLAN20.SPPSVST$PGG2024_2028
                    UNION
                    SELECT R.SPPRES$ID_RESULTADO ID_EJE, R.SPPRES$CODIGO || '-' || R.SPPRES$DESCRIPCION EJE FROM SCHE$SIPLAN20.SP20$RESULTADOS_ESTRATEGICOS R   
                    INNER JOIN SCHE$SIPLAN20.S20$PERIODO_RESULTADO PR ON PR.SPR$ID_RESULTADO = R.SPPRES$ID_RESULTADO AND PR.SPR$RESTRICTIVA = 'N' AND R.SPPRES$RESTRICTIVA = 'N'
                    INNER JOIN SCHE$SIPLAN20.SP20$PERIODO_GOBIERNO PG ON PG.SPG$ID_PERIODO = PR.SPR$ID_PERIODO AND PG.SPG$RESTRICTIVA = 'N' AND PR.SPR$RESTRICTIVA = 'N' AND PG.SPG$VIGENTE = 1
                    )
                GROUP BY
                ID_EJE
                ,EJE
                ORDER BY ID_EJE";
            estado = dao.consulta(sql);
            if (estado == 1)
            {
                tabla = dao.tabla;
                gv1eje.DataSource = tabla;
                gv1eje.DataBind();


            }

            else
            {
                mensaje = dao.mensaje;
                Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',1);", true);
            }





        }



        protected void carga1ejered()
        {
            sql = @"SELECT 
                    ID_EJE
                    ,EJE
                    FROM
                    (SELECT ID_EJE, EJE FROM SCHE$SIPLAN20.SPPSVST$PGG2024_2028
                    UNION
                    SELECT R.SPPRES$ID_RESULTADO ID_EJE, R.SPPRES$CODIGO || '-' || R.SPPRES$DESCRIPCION EJE FROM SCHE$SIPLAN20.SP20$RESULTADOS_ESTRATEGICOS R   
                    INNER JOIN SCHE$SIPLAN20.S20$PERIODO_RESULTADO PR ON PR.SPR$ID_RESULTADO = R.SPPRES$ID_RESULTADO AND PR.SPR$RESTRICTIVA = 'N' AND R.SPPRES$RESTRICTIVA = 'N'
                    INNER JOIN SCHE$SIPLAN20.SP20$PERIODO_GOBIERNO PG ON PG.SPG$ID_PERIODO = PR.SPR$ID_PERIODO AND PG.SPG$RESTRICTIVA = 'N' AND PR.SPR$RESTRICTIVA = 'N' AND PG.SPG$VIGENTE = 1
                    )
                GROUP BY
                ID_EJE
                ,EJE
                ORDER BY ID_EJE";
            estado = dao.consulta(sql);
            if (estado == 1)
            {
                tabla = dao.tabla;
                gvRed1Eeje.DataSource = tabla;
                gvRed1Eeje.DataBind();


            }

            else
            {
                mensaje = dao.mensaje;
                Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',1);", true);
            }





        }

        protected void btnVinculaPGG_Click(object sender, EventArgs e)
        {
            int nivel = -1;
            int producto = -1;
            int rest1 = -1;
            int rest2 = -11;
            int tipo = -1;

            if (cbResultados_tempo.SelectedIndex == -1)
            {
                mensaje = "Debe seleccionar al menos un resultado institucional";
                Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
            }

            else
            {
                rest1 = Convert.ToInt32(cbResultados_tempo.Value);
                if (gvPGG2428.FocusedRowIndex != -1)
                {
                    nivel = gvPGG2428.GetRowLevel(gvPGG2428.FocusedRowIndex);
                    if (nivel == 2)
                    {
                        sql = "SELECT * FROM SCHE$SIPLAN20.SP20$RESULTADOS WHERE SPRES$COD_ESTRATEGICO = " + Convert.ToInt32(gvPGG2428.GetRowValues(gvPGG2428.FocusedRowIndex, "ID_ACCION").ToString()) + " AND SPRES$POM = " + Session["pom"] + " AND SPRES$INSTITUCION = " + Session["insto"] + " AND SPRES$RESTRICTIVA = 'N'";
                        estado = dao.consulta(sql);
                        if (estado == 1)
                        {
                            tabla = dao.tabla;
                            if (tabla.Rows.Count > 0)
                            {
                                rest2 = Convert.ToInt32(tabla.Rows[0]["SPRES$ID_RESULTADO"]);
                            }
                            else
                            {
                                sql = "INSERT INTO SCHE$SIPLAN20.SP20$RESULTADOS(SPRES$TIPO,SPRES$COD_ESTRATEGICO, SPRES$FECHA_INSERT, SPRES$POM, SPRES$INSTITUCION, SPRES$PROPIETARIO) VALUES (0, " + Convert.ToInt32(gvPGG2428.GetRowValues(gvPGG2428.FocusedRowIndex, "ID_ACCION").ToString()) + ", 'INSERT = '||TO_CHAR(SYSDATE,'DD/MM/YYYY HH24:MI')||' '||'" + Session["USUARIO"].ToString() + "', " + Convert.ToInt32(Session["pom"]) + ", " + Convert.ToInt32(Session["insto"]) + ", '" + Session["USUARIO"].ToString() + "')";
                                estado = dao.comando(sql);
                                if (estado == 1)
                                {
                                    sql = "SELECT MAX(SPRES$ID_RESULTADO) ID_RESULTADO FROM SCHE$SIPLAN20.SP20$RESULTADOS WHERE SPRES$POM = " + Convert.ToInt32(Session["pom"]) + " AND SPRES$INSTITUCION = " + Convert.ToInt32(Session["insto"]) + " AND SPRES$COD_ESTRATEGICO = " + Convert.ToInt32(gvPGG2428.GetRowValues(gvPGG2428.FocusedRowIndex, "ID_ACCION").ToString()) + " AND SPRES$RESTRICTIVA = 'N'";
                                    estado = dao.consulta(sql);
                                    if (estado == 1)
                                    {
                                        tabla = dao.tabla;
                                        if (tabla.Rows.Count > 0)
                                            rest2 = Convert.ToInt32(tabla.Rows[0]["ID_RESULTADO"]);
                                        else
                                            rest2 = -1;

                                    }
                                    else
                                    {
                                        rest2 = -1;
                                    }


                                }

                            }
                        }
                    }
                    else
                        rest2 = -1;
                }
                else
                {
                    rest2 = -1;
                }


                for (int i = 0; i < gvProduccionInsto.VisibleRowCount; i++)
                {
                    if (gvProduccionInsto.Selection.IsRowSelected(i))
                    {
                        producto = Convert.ToInt32(gvProduccionInsto.GetRowValues(i, "SPPRO$ID_PRODUCTO"));


                        sql = "UPDATE SCHE$SIPLAN20.SP20$PRODUCTO SET SPPRO$ID_RESULTADO = " + rest1 + ",  SPPRO$RESULTADO2 = " + rest2 + ", SPPRO$TEMPORAL = 1, SPPRO$FECHA_UPDATE = 'UDPATE = '||TO_CHAR(SYSDATE,'DD/MM/YYYY HH24:MI')||' '||'" + Session["USUARIO"].ToString() + "' WHERE SPPRO$ID_PRODUCTO = " + producto + " AND SPPRO$POM = " + Session["pom"] + " AND SPPRO$INSTO = " + Session["insto"] + " AND SPPRO$RESTRICTIVA = 'N'";

                        estado = dao.comando(sql);
                        if (estado == 0)
                            break;

                    }

                }


                if (estado == 0)
                {
                    mensaje = dao.mensaje;
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
                    popPGG2428.ShowOnPageLoad = false;
                    cargaProdutosTemporal(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]));
                    gvProduccionInsto.Selection.UnselectAll();
                    MultiView1.ActiveViewIndex = 10;

                }
                else
                {
                    mensaje = "Productos vinculados correctamente a RI/PGG 2024-2028";
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',1);", true);
                    popPGG2428.ShowOnPageLoad = false;
                    cargaProdutosTemporal(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]));
                    gvProduccionInsto.Selection.UnselectAll();
                    MultiView1.ActiveViewIndex = 10;
                }
            }



        }

        protected void btnCancelaPGG_Click(object sender, EventArgs e)
        {
            popPGG2428.ShowOnPageLoad = false;
            //popResultadosTempo.ShowOnPageLoad = false;
        }

        //protected void btnVinculaPconRT_Click(object sender, EventArgs e)
        //{
        //    popResultadosTempo.ShowOnPageLoad = true;
        //    if (Session["pom"] != null && Session["insto"] != null)
        //    {
        //        cbDesvincularestos.SelectedIndex = 0;
        //        cargaResultadoInstoTempo(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]));
        //    }
        //}

        protected void cargaResultadoInstoTempo(int pom, int insto)
        {
            sql = "SELECT * FROM SCHE$SIPLAN20.SP20$RESULTADOS R WHERE R.SPRES$POM = " + pom + " AND R.SPRES$INSTITUCION = " + insto + " AND R.SPRES$TIPO = 1 AND R.SPRES$RESTRICTIVA = 'N'";
            estado = dao.consulta(sql);
            if (estado == 0)
            {
                mensaje = dao.mensaje;
                Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);

            }
            else
            {
                tabla = dao.tabla;
                cbResultados_tempo.DataSource = tabla;
                cbResultados_tempo.ValueField = "SPRES$ID_RESULTADO";
                cbResultados_tempo.TextField = "SPRES$DESCRIPCION";
                cbResultados_tempo.DataBind();
            }
        }





        protected void cargaResultado1EJE(int pom, int insto)
        {
            sql = "SELECT * FROM SCHE$SIPLAN20.SP20$RESULTADOS R WHERE R.SPRES$POM = " + pom + " AND R.SPRES$INSTITUCION = " + insto + " AND R.SPRES$TIPO = 1 AND R.SPRES$RESTRICTIVA = 'N'";
            estado = dao.consulta(sql);
            if (estado == 0)
            {
                mensaje = dao.mensaje;
                Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);

            }
            else
            {
                tabla = dao.tabla;
                cbResto1Eje.DataSource = tabla;
                cbResto1Eje.ValueField = "SPRES$ID_RESULTADO";
                cbResto1Eje.TextField = "SPRES$DESCRIPCION";
                cbResto1Eje.DataBind();
            }
        }
        //protected void btnVincRtempo_Click(object sender, EventArgs e)
        //{
        //    int producto = 0;
        //    int tipo = -1;
        //    int rest1 = -1;
        //    int rest2 = -1;
        //    if (cbResultados_tempo.SelectedIndex == -1)
        //    {
        //        mensaje = "Seleccione el resultado a vincular";
        //        cbResultados_tempo.Focus();
        //        Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);

        //    }
        //    else
        //    {
        //        for (int i = 0; i < gvProduccionInsto.VisibleRowCount; i++)
        //        {

        //            if (gvProduccionInsto.Selection.IsRowSelected(i))
        //            {


        //                producto = Convert.ToInt32(gvProduccionInsto.GetRowValues(i, "SPPRO$ID_PRODUCTO"));
        //                tipo = Convert.ToInt32(gvProduccionInsto.GetRowValues(i, "SPRES$TIPO"));

        //                if (tipo == 1)
        //                {
        //                    if (cbDesvincularestos.SelectedIndex == 0)
        //                    {
        //                        rest1 = Convert.ToInt32(cbResultados_tempo.Value);
        //                        rest2 = Convert.ToInt32(gvProduccionInsto.GetRowValues(i, "SPPRO$RESULTADO2"));
        //                    }
        //                    else
        //                    {
        //                        rest1 = Convert.ToInt32(cbResultados_tempo.Value);
        //                        rest2 = -1;
        //                    }

        //                }
        //                else
        //                {

        //                    if (cbDesvincularestos.SelectedIndex == 0)
        //                    {
        //                        rest1 = Convert.ToInt32(gvProduccionInsto.GetRowValues(i, "SPRES$ID_RESULTADO"));
        //                        rest2 = Convert.ToInt32(cbResultados_tempo.Value);
        //                    }
        //                    else
        //                    {
        //                        rest1 = Convert.ToInt32(cbResultados_tempo.Value);
        //                        rest2 = -1;
        //                    }
        //                }

        //                sql = "UPDATE SCHE$SIPLAN20.SP20$PRODUCTO SET SPPRO$ID_RESULTADO = " + rest1 + ",  SPPRO$RESULTADO2 = "+rest2+" ,SPPRO$TEMPORAL = 1, SPPRO$FECHA_UPDATE = 'UDPATE = '||TO_CHAR(SYSDATE,'DD/MM/YYYY HH24:MI')||' '||'" + Session["USUARIO"].ToString() + "' WHERE SPPRO$ID_PRODUCTO = " + producto + " AND SPPRO$POM = " + Session["pom"] + " AND SPPRO$INSTO = " + Session["insto"] + " AND SPPRO$RESTRICTIVA = 'N'";

        //                estado = dao.comando(sql);
        //                if (estado == 0)
        //                    break;

        //            }

        //        }

        //        if (estado == 0)
        //        {
        //            mensaje = dao.mensaje;
        //            Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);

        //            popResultadosTempo.ShowOnPageLoad = false;
        //            cargaProdutosTemporal(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]));
        //            gvProduccionInsto.Selection.UnselectAll();
        //            MultiView1.ActiveViewIndex = 10;
        //        }

        //        else
        //        {
        //            popResultadosTempo.ShowOnPageLoad = false;
        //            mensaje = "Productos vinculados correctamente a resultados institucionales";
        //            Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',1);", true);
        //            cargaProdutosTemporal(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]));
        //            gvProduccionInsto.Selection.UnselectAll();
        //            MultiView1.ActiveViewIndex = 10;

        //        }
        //    }

        //}

        protected void btnUnEjej_Click(object sender, EventArgs e)
        {

            int cod_estrategico;
            int estados = 0;
            DataTable resto = new DataTable();

            for (int i = 0; i < gvUnEje.VisibleRowCount; i++)
            {

                if (gvUnEje.Selection.IsRowSelected(i))
                {
                    cod_estrategico = Convert.ToInt32(gvUnEje.GetRowValues(i, "ID_EJE"));
                    sql = "SELECT * FROM SCHE$SIPLAN20.SP20$RESULTADOS WHERE SPRES$RESTRICTIVA = 'N' AND SPRES$COD_ESTRATEGICO = " + cod_estrategico + " AND SPRES$POM = " + Convert.ToInt32(Session["pom"]) + " AND SPRES$INSTITUCION = " + Convert.ToInt32(Session["insto"]) + " AND SPRES$TIPO = 0";
                    estado = dao.consulta(sql);
                    if (estado == 0)
                    {
                        mensaje = dao.mensaje;
                        //Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
                        ScriptManager.RegisterStartupScript(this.upaneUnEje, GetType(), "script", "Alerta('" + mensaje + " <br/>',2);", true);
                        estados = 0;
                        break;

                    }

                    else
                    {
                        tabla = dao.tabla;

                        if (tabla.Rows.Count > 0)
                            continue;
                        else
                        {
                            sql = "INSERT INTO SCHE$SIPLAN20.SP20$RESULTADOS(SPRES$TIPO,SPRES$COD_ESTRATEGICO, SPRES$FECHA_INSERT, SPRES$POM, SPRES$INSTITUCION, SPRES$PROPIETARIO) VALUES (0, " + cod_estrategico + ", 'INSERT = '||TO_CHAR(SYSDATE,'DD/MM/YYYY HH24:MI')||' '||'" + Session["USUARIO"].ToString() + "', " + Convert.ToInt32(Session["pom"]) + ", " + Convert.ToInt32(Session["insto"]) + ", '" + Session["USUARIO"].ToString() + "')";
                            estado = dao.comando(sql);
                            if (estado == 0)
                            {
                                mensaje = dao.mensaje;
                                //Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
                                ScriptManager.RegisterStartupScript(this.upaneUnEje, GetType(), "script", "Alerta('" + mensaje + " <br/>',2);", true);
                                estados = 0;
                                break;
                            }
                            else
                                estados = 1;

                        }



                    }



                }
                else
                {
                    cod_estrategico = Convert.ToInt32(gvUnEje.GetRowValues(i, "ID_EJE"));
                    sql = "SELECT * FROM SCHE$SIPLAN20.SP20$RESULTADOS R INNER JOIN SCHE$SIPLAN20.SP20$PRODUCTO P ON R.SPRES$ID_RESULTADO = P.SPPRO$ID_RESULTADO AND R.SPRES$POM = P.SPPRO$POM AND R.SPRES$INSTITUCION = P.SPPRO$INSTO  AND R.SPRES$RESTRICTIVA = 'N' AND P.SPPRO$RESTRICTIVA = 'N' WHERE R.SPRES$TIPO = 0 AND R.SPRES$COD_ESTRATEGICO = " + cod_estrategico + " AND R.SPRES$POM = " + Convert.ToInt32(Session["pom"]) + " AND R.SPRES$INSTITUCION = " + Convert.ToInt32(Session["insto"]) + " AND R.SPRES$RESTRICTIVA = 'N'";
                    estado = dao.consulta(sql);
                    if (estado == 0)
                    {
                        mensaje = dao.mensaje;
                        //Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
                        ScriptManager.RegisterStartupScript(this.upaneUnEje, GetType(), "script", "Alerta('" + mensaje + " <br/>',2);", true);
                        estados = 0;
                        break;
                    }
                    else
                    {
                        resto = dao.tabla;
                        if (resto.Rows.Count > 0)
                        {

                            estados = 1;
                            continue;

                        }

                        else
                        {
                            sql = "UPDATE SCHE$SIPLAN20.SP20$RESULTADOS SET SPRES$RESTRICTIVA = 'S', SPRES$FECHA_DELETE = 'DELETE = '||TO_CHAR(SYSDATE,'DD/MM/YYYY HH24:MI')||' '||'" + Session["USUARIO"].ToString() + "' WHERE SPRES$TIPO = 0 AND SPRES$COD_ESTRATEGICO = " + cod_estrategico + " AND SPRES$POM = " + Convert.ToInt32(Session["pom"]) + " AND SPRES$INSTITUCION = " + Convert.ToInt32(Session["insto"]);
                            estado = dao.comando(sql);
                            if (estado == 0)
                            {
                                mensaje = dao.mensaje;
                                //Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
                                ScriptManager.RegisterStartupScript(this.upaneUnEje, GetType(), "script", "Alerta('" + mensaje + " <br/>',2);", true);
                                estados = 0;
                                break;
                            }
                            else
                            {
                                estados = 1;
                            }
                        }
                    }
                }
            }

            if (estados == 0)
            {
                mensaje = "Es posible que errores del sistema hayan ocasionado una operación incorrecta, por favor contacte al administrador";
                //Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
                ScriptManager.RegisterStartupScript(this.upaneUnEje, GetType(), "script", "Alerta('" + mensaje + " <br/>',1);", true);

                //MultiView1.ActiveViewIndex = 2;
                //Estrategicos.Style.Add("display", "block");
                //Institucionales.Style.Add("display", "none");

                if (resultados.SelectedIndex == 0)
                {
                    Estrategicos.Style.Add("display", "none");
                    Institucionales.Style.Add("display", "block");
                }
                popUnEje.ShowOnPageLoad = false;
            }
            else if (estados == 1)
            {

                mensaje = "Se han vinculado/desvinculado a programa 2024-2028 correctamente";
                //Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
                ScriptManager.RegisterStartupScript(this.upaneUnEje, GetType(), "script", "Alerta('" + mensaje + " <br/>',1);", true);

                //MultiView1.ActiveViewIndex = 2;
                //Estrategicos.Style.Add("display", "block");
                //Institucionales.Style.Add("display", "none");
                if (resultados.SelectedIndex == 0)
                {
                    Estrategicos.Style.Add("display", "none");
                    Institucionales.Style.Add("display", "block");
                }
                popUnEje.ShowOnPageLoad = false;
            }


        }

        protected void btnCancelaUnEje_Click(object sender, EventArgs e)
        {
            popUnEje.ShowOnPageLoad = false;
        }

        protected void btn1eje_Click(object sender, EventArgs e)
        {

            int producto = -1;
            int rest1 = -1;
            int rest2 = -1;

            if (cbResto1Eje.SelectedIndex == -1 || gv1eje.FocusedRowIndex == -1)
            {
                mensaje = "Es obligatorio seleccionar el resultado institucional y el eje estratégico";
                Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
            }

            else
            {
                rest1 = Convert.ToInt32(cbResto1Eje.Value);
                if (gv1eje.FocusedRowIndex != -1)
                {
                    sql = "SELECT * FROM SCHE$SIPLAN20.SP20$RESULTADOS WHERE SPRES$COD_ESTRATEGICO = " + Convert.ToInt32(gv1eje.GetRowValues(gv1eje.FocusedRowIndex, "ID_EJE").ToString()) + " AND SPRES$POM = " + Session["pom"] + " AND SPRES$INSTITUCION = " + Session["insto"] + " AND SPRES$RESTRICTIVA = 'N'";
                    estado = dao.consulta(sql);
                    if (estado == 1)
                    {
                        tabla = dao.tabla;
                        if (tabla.Rows.Count > 0)
                        {
                            rest2 = Convert.ToInt32(tabla.Rows[0]["SPRES$ID_RESULTADO"]);
                        }
                        else
                        {
                            sql = "INSERT INTO SCHE$SIPLAN20.SP20$RESULTADOS(SPRES$TIPO,SPRES$COD_ESTRATEGICO, SPRES$FECHA_INSERT, SPRES$POM, SPRES$INSTITUCION, SPRES$PROPIETARIO) VALUES (0, " + Convert.ToInt32(gv1eje.GetRowValues(gv1eje.FocusedRowIndex, "ID_EJE").ToString()) + ", 'INSERT = '||TO_CHAR(SYSDATE,'DD/MM/YYYY HH24:MI')||' '||'" + Session["USUARIO"].ToString() + "', " + Convert.ToInt32(Session["pom"]) + ", " + Convert.ToInt32(Session["insto"]) + ", '" + Session["USUARIO"].ToString() + "')";
                            estado = dao.comando(sql);
                            if (estado == 1)
                            {
                                sql = "SELECT MAX(SPRES$ID_RESULTADO) ID_RESULTADO FROM SCHE$SIPLAN20.SP20$RESULTADOS WHERE SPRES$POM = " + Convert.ToInt32(Session["pom"]) + " AND SPRES$INSTITUCION = " + Convert.ToInt32(Session["insto"]) + " AND SPRES$COD_ESTRATEGICO = " + Convert.ToInt32(gv1eje.GetRowValues(gv1eje.FocusedRowIndex, "ID_EJE").ToString()) + " AND SPRES$RESTRICTIVA = 'N'";
                                estado = dao.consulta(sql);
                                if (estado == 1)
                                {
                                    tabla = dao.tabla;
                                    if (tabla.Rows.Count > 0)
                                        rest2 = Convert.ToInt32(tabla.Rows[0]["ID_RESULTADO"]);

                                }
                            }
                        }

                    }

                }
                else
                    rest2 = -1;


                for (int i = 0; i < gvProduccionInsto.VisibleRowCount; i++)
                {

                    if (gvProduccionInsto.Selection.IsRowSelected(i))
                    {
                        producto = Convert.ToInt32(gvProduccionInsto.GetRowValues(i, "SPPRO$ID_PRODUCTO"));
                        sql = "UPDATE SCHE$SIPLAN20.SP20$PRODUCTO SET SPPRO$ID_RESULTADO = " + rest1 + ",  SPPRO$RESULTADO2 = " + rest2 + ", SPPRO$TEMPORAL = 1, SPPRO$FECHA_UPDATE = 'UDPATE = '||TO_CHAR(SYSDATE,'DD/MM/YYYY HH24:MI')||' '||'" + Session["USUARIO"].ToString() + "' WHERE SPPRO$ID_PRODUCTO = " + producto + " AND SPPRO$POM = " + Session["pom"] + " AND SPPRO$INSTO = " + Session["insto"] + " AND SPPRO$RESTRICTIVA = 'N'";
                        estado = dao.comando(sql);
                        if (estado == 0)
                            break;
                    }

                }

                if (estado == 0)
                {
                    mensaje = dao.mensaje;
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
                    pop1eje.ShowOnPageLoad = false;
                    cargaProdutosTemporal(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]));
                    gvProduccionInsto.Selection.UnselectAll();
                    MultiView1.ActiveViewIndex = 10;

                }
                else
                {
                    mensaje = "Productos vinculados correctamente a RI/PGG";
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',1);", true);
                    pop1eje.ShowOnPageLoad = false;
                    cargaProdutosTemporal(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]));
                    gvProduccionInsto.Selection.UnselectAll();
                    MultiView1.ActiveViewIndex = 10;
                }

            }





        }

        protected void btnCancela1eje_Click(object sender, EventArgs e)
        {
            pop1eje.ShowOnPageLoad = false;
        }

        protected void gvProduccionInsto_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
        {
            int valor = -1;

            valor = Convert.ToInt32(e.GetValue("NIVEL"));
            if (valor == 2)
            {
                e.Row.ForeColor = System.Drawing.Color.Red;
                //e.Row.BackColor = System.Drawing.Color.MediumSeaGreen;
            }

            else
                e.Row.ForeColor = System.Drawing.Color.Black;

        }



        
        protected void btnConfirma_Click(object sender, EventArgs e)
        {
            if (Session["CONFIRMA"] != null)
            {
                if (Convert.ToInt32(Session["CONFIRMA"]) == 1)
                {
                    migra_resultados_institucionales(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]));
                    carga_resultado_insto(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]));

                    Session["CONFIRMA"] = 0;
                    Confirma.ShowOnPageLoad = false;

                }

                else if (Convert.ToInt32(Session["CONFIRMA"]) == 2)
                {
                    migra_programas_presupuestarios(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]));
                    carga_programa_presupuestario(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]));
                    Session["CONFIRMA"] = 0;
                    Confirma.ShowOnPageLoad = false;
                }

                else if (Convert.ToInt32(Session["CONFIRMA"]) == 3)
                {
                    migra_productos_institucionales(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]));
                    cargaProductosInstitucionales(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]));
                    Session["CONFIRMA"] = 0;
                    Confirma.ShowOnPageLoad = false;
                }

                else if (Convert.ToInt32(Session["CONFIRMA"]) == 4)
                {
                    migra_resultados_RED(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]));
                    migraproductosRED(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]));
                    CargaProductosRed(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]));
                    Session["CONFIRMA"] = 0;
                    Confirma.ShowOnPageLoad = false;
                }

            }
        }

        protected void btnConfirmaCancela_Click(object sender, EventArgs e)
        {

            tabla.Clear();
            if (Session["CONFIRMA"] != null)
            {
                if (Convert.ToInt32(Session["CONFIRMA"]) == 1)
                {
                    Session["CONFIRMA"] = 0;
                    Confirma.ShowOnPageLoad = false;
                }
                else if (Convert.ToInt32(Session["CONFIRMA"]) == 2)
                {
                    Session["CONFIRMA"] = 0;
                    Confirma.ShowOnPageLoad = false;
                }

                else if (Convert.ToInt32(Session["CONFIRMA"]) == 3)
                {
                    Session["CONFIRMA"] = 0;
                    Confirma.ShowOnPageLoad = false;
                }

                else if (Convert.ToInt32(Session["CONFIRMA"]) == 4)
                {
                    Session["CONFIRMA"] = 0;
                    Confirma.ShowOnPageLoad = false;
                }
            }
            else
                Confirma.ShowOnPageLoad = false;


        }

        protected void btnMigrar_Click(object sender, EventArgs e)
        {
            sql = "SELECT P.* FROM SCHE$SIPLAN20.SP20$PRODUCTO P INNER JOIN SCHE$SIPLAN20.SP20$RESULTADOS R ON P.SPPRO$ID_RESULTADO = R.SPRES$ID_RESULTADO AND P.SPPRO$RESTRICTIVA = 'N' AND R.SPRES$RESTRICTIVA = 'N'  WHERE  P.SPPRO$RESTRICTIVA = 'N' AND P.SPPRO$POM = " + Session["pom"] + " AND P.SPPRO$INSTO = " + Session["insto"] + " AND R.SPRES$TIPO = 2";

            estado = dao.consulta(sql);
            if (estado == 1)
                tabla = dao.tabla;
            if (tabla.Rows.Count > 0)
            {
                mensaje = "Ya hay programación de productos RE registrada";
                ScriptManager.RegisterStartupScript(this.panProdEstrategicos, GetType(), "script", "Alerta('" + mensaje + " <br/>',1);", true);
            }
            else
            {
                migra_resultados_RED(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]));
                migraproductosRED(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]));
                CargaProductosRed(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]));
            }

        }

        protected int busca_responsable_red(int responsable)
        {
            int resultado = 0;
            DataTable red = new DataTable();
            sql = "SELECT * FROM SCHE$SIPLAN20.SP20$RESPONSABLES_RED  WHERE SRRED$ID_RESPONSABLE = " + responsable + " ORDER BY SRRED$RED ASC";
            estado = dao.consulta(sql);
            if (estado == 1)
                red = dao.tabla;
            if (red.Rows.Count > 0)
                resultado = 1;
            else
                resultado = 0;
            return resultado;
        }

        protected void busca_red(int insto, int responsable, int red)
        {
            DataTable responsables = new DataTable();
            if (responsable == 0)
                sql = "SELECT SPPRED$ID, SPPRED$RED||' '||SPPRED$DESCRIPCION RED FROM SCHE$SIPLAN20.SP20$RED WHERE SPPRED$RESTRICTIVA = 'N' AND SPPRED$VIGENTE = 1 ORDER BY SPPRED$ID ASC";
            else
                sql = @"SELECT  R.SPPRED$ID
                                ,R.SPPRED$RED || ' ' || R.SPPRED$DESCRIPCION RED
                                FROM SCHE$SIPLAN20.SP20$RED R
                                INNER JOIN SCHE$SIPLAN20.SP20$RESPONSABLES_RED RES
                                ON RES.SRRED$RED = R.SPPRED$ID AND RES.SRRED$RESTRICTIVA = 'N' AND R.SPPRED$RESTRICTIVA = 'N'
                                WHERE R.SPPRED$VIGENTE = 1 AND RES.SRRED$ID_RESPONSABLE = " + insto + " ORDER BY R.SPPRED$ID ASC";
            estado = dao.consulta(sql);
            if (estado == 1)
                responsables = dao.tabla;
            if (responsables.Rows.Count > 0)
            {
                cbRed.DataSource = responsables;
                cbRed.ValueField = "SPPRED$ID";
                cbRed.TextField = "RED";
                cbRed.DataBind();

                if (red != -1)
                    cbRed.Value = red.ToString();
                else
                    cbRed.SelectedIndex = -1;

                cbRed.Enabled = true;


            }



        }


        protected void busca_red_tempo(int insto, int responsable, int red)
        {
            DataTable responsables = new DataTable();
            if (responsable == 0)
                sql = "SELECT SPPRED$ID, SPPRED$RED||' '||SPPRED$DESCRIPCION RED FROM SCHE$SIPLAN20.SP20$RED WHERE SPPRED$RESTRICTIVA = 'N' AND SPPRED$VIGENTE = 1 ORDER BY SPPRED$ID ASC";
            else
                sql = @"SELECT  R.SPPRED$ID
                                ,R.SPPRED$RED || ' ' || R.SPPRED$DESCRIPCION RED
                                FROM SCHE$SIPLAN20.SP20$RED R
                                INNER JOIN SCHE$SIPLAN20.SP20$RESPONSABLES_RED RES
                                ON RES.SRRED$RED = R.SPPRED$ID AND RES.SRRED$RESTRICTIVA = 'N' AND R.SPPRED$RESTRICTIVA = 'N'
                                WHERE R.SPPRED$VIGENTE = 1 AND RES.SRRED$ID_RESPONSABLE = " + insto + " ORDER BY R.SPPRED$ID ASC";
            estado = dao.consulta(sql);
            if (estado == 1)
                responsables = dao.tabla;
            if (responsables.Rows.Count > 0)
            {
                cbRedTempo.DataSource = responsables;
                cbRedTempo.ValueField = "SPPRED$ID";
                cbRedTempo.TextField = "RED";
                cbRedTempo.DataBind();

                if (red != -1)
                    cbRedTempo.Value = red.ToString();
                else
                    cbRedTempo.SelectedIndex = -1;

                cbRedTempo.Enabled = true;


            }



        }


        protected void busca_red_tempo1Eje(int insto, int responsable, int red)
        {
            DataTable responsables = new DataTable();
            if (responsable == 0)
                sql = "SELECT SPPRED$ID, SPPRED$RED||' '||SPPRED$DESCRIPCION RED FROM SCHE$SIPLAN20.SP20$RED WHERE SPPRED$RESTRICTIVA = 'N' AND SPPRED$VIGENTE = 1 ORDER BY SPPRED$ID ASC";
            else
                sql = @"SELECT  R.SPPRED$ID
                                ,R.SPPRED$RED || ' ' || R.SPPRED$DESCRIPCION RED
                                FROM SCHE$SIPLAN20.SP20$RED R
                                INNER JOIN SCHE$SIPLAN20.SP20$RESPONSABLES_RED RES
                                ON RES.SRRED$RED = R.SPPRED$ID AND RES.SRRED$RESTRICTIVA = 'N' AND R.SPPRED$RESTRICTIVA = 'N'
                                WHERE R.SPPRED$VIGENTE = 1 AND RES.SRRED$ID_RESPONSABLE = " + insto + " ORDER BY R.SPPRED$ID ASC";
            estado = dao.consulta(sql);
            if (estado == 1)
                responsables = dao.tabla;
            if (responsables.Rows.Count > 0)
            {
                cbRed1EJE.DataSource = responsables;
                cbRed1EJE.ValueField = "SPPRED$ID";
                cbRed1EJE.TextField = "RED";
                cbRed1EJE.DataBind();

                if (red != -1)
                    cbRed1EJE.Value = red.ToString();
                else
                    cbRed1EJE.SelectedIndex = -1;

                cbRed1EJE.Enabled = true;


            }



        }
        protected void btnRED_Click(object sender, EventArgs e)
        {
            int responsable = -1;

            if (Session["pom"] != null && Session["insto"] != null)
            {
                sql = @"SELECT ID_EJE FROM SCHE$SIPLAN20.SPPSVST$PGG2024_2028 WHERE RESPONSABLE = " + Session["insto"] + @" 
                        UNION 
                        
                        SELECT RG.SPRPG$ID_PGG ID_EJE
                                                    
                                                    FROM SCHE$SIPLAN20.SP20$RESULTADOS_ESTRATEGICOS R
                                                    INNER JOIN SCHE$SIPLAN20.SP20$RESPONSABLESPGG RG ON RG.SPRPG$ID_PGG = R.SPPRES$ID_RESULTADO AND RG.SPRPG$NIVEL = 1 AND R.SPPRES$RESTRICTIVA = 'N' AND RG.SPRPG$RESTRICTIVA = 'N'
                                                    WHERE RG.SPRPG$RESPONSABLE = " + Session["insto"];
                estado = dao.consulta(sql);
                if (estado == 1)
                {
                    tabla = dao.tabla;
                    if (tabla.Rows.Count > 0)
                    {
                        responsable = busca_responsable_red(Convert.ToInt32(Session["insto"]));
                        busca_red_tempo(Convert.ToInt32(Session["insto"]), responsable, -1);
                        cargaPGG24282(Convert.ToInt32(Session["insto"]));
                        popRed.ShowOnPageLoad = true;
                    }
                    else
                    {
                        responsable = busca_responsable_red(Convert.ToInt32(Session["insto"]));
                        busca_red_tempo1Eje(Convert.ToInt32(Session["insto"]), responsable, -1);
                        carga1ejered();
                        popRed1eje.ShowOnPageLoad = true;
                    }
                }


            }






        }

        protected void btnCancelaRED_Click(object sender, EventArgs e)
        {
            cbRedTempo.DataSource = null;
            cbRedTempo.Items.Clear();
            popRed.ShowOnPageLoad = false;
        }

        protected void btnVincRED_Click(object sender, EventArgs e)
        {
            DataTable red_resultado = new DataTable();
            int red_producto = -1;
            int producto;
            int nivel = -1;
            int rest1 = -1;
            int rest2 = -1;

            sql = "SELECT * FROM SCHE$SIPLAN20.SP20$RESULTADOS R WHERE R.SPRES$POM = " + Convert.ToInt32(Session["pom"]) + " AND R.SPRES$INSTITUCION = " + Convert.ToInt32(Session["insto"]) + " AND R.SPRES$TIPO = 2 AND R.SPRES$COD_ESTRATEGICO = " + Convert.ToInt32(cbRedTempo.Value) + " AND SPRES$RESTRICTIVA = 'N'";
            estado = dao.consulta(sql);
            if (estado == 1)
            {
                red_resultado = dao.tabla;
                if (red_resultado.Rows.Count > 0)
                    red_producto = Convert.ToInt32(red_resultado.Rows[0]["SPRES$ID_RESULTADO"]);
                else
                {
                    sql = "INSERT  INTO SCHE$SIPLAN20.SP20$RESULTADOS (SPRES$TIPO, SPRES$COD_ESTRATEGICO, SPRES$FECHA_INSERT, SPRES$POM, SPRES$INSTITUCION, SPRES$PROPIETARIO) VALUES(2, " + cbRedTempo.Value + ", 'INSERT = '||TO_CHAR(SYSDATE,'DD/MM/YYYY HH24:MI')||' '||'" + Session["USUARIO"].ToString() + "', " + Convert.ToInt32(Session["pom"]) + ", " + Convert.ToInt32(Session["insto"]) + ", '" + Session["USUARIO"].ToString() + "')";
                    estado = dao.comando(sql);
                    if (estado == 1)
                    {
                        sql = "SELECT MAX(SPRES$ID_RESULTADO) ID FROM SCHE$SIPLAN20.SP20$RESULTADOS R WHERE R.SPRES$POM = " + Convert.ToInt32(Session["pom"]) + " AND R.SPRES$INSTITUCION = " + Convert.ToInt32(Session["insto"]) + " AND R.SPRES$TIPO = 2 AND R.SPRES$COD_ESTRATEGICO = " + cbRedTempo.Value + " AND SPRES$RESTRICTIVA = 'N'";
                        estado = dao.consulta(sql);
                        if (estado == 1)
                        {
                            red_resultado = dao.tabla;
                            if (red_resultado.Rows.Count > 0)
                                red_producto = Convert.ToInt32(red_resultado.Rows[0]["ID"]);
                        }
                    }
                }

            }

            rest1 = red_producto;

            if (gv2428red.FocusedRowIndex != -1)
            {

                nivel = gv2428red.GetRowLevel(gv2428red.FocusedRowIndex);
                if (nivel == 2)
                {
                    sql = "SELECT * FROM SCHE$SIPLAN20.SP20$RESULTADOS WHERE SPRES$COD_ESTRATEGICO = " + Convert.ToInt32(gv2428red.GetRowValues(gv2428red.FocusedRowIndex, "ID_ACCION").ToString()) + " AND SPRES$POM = " + Session["pom"] + " AND SPRES$INSTITUCION = " + Session["insto"] + " AND SPRES$RESTRICTIVA = 'N'";
                    estado = dao.consulta(sql);
                    if (estado == 1)
                    {
                        tabla = dao.tabla;
                        if (tabla.Rows.Count > 0)
                        {
                            rest2 = Convert.ToInt32(tabla.Rows[0]["SPRES$ID_RESULTADO"]);
                        }
                        else
                        {
                            sql = "INSERT INTO SCHE$SIPLAN20.SP20$RESULTADOS(SPRES$TIPO,SPRES$COD_ESTRATEGICO, SPRES$FECHA_INSERT, SPRES$POM, SPRES$INSTITUCION, SPRES$PROPIETARIO) VALUES (0, " + Convert.ToInt32(gvPGG2428.GetRowValues(gvPGG2428.FocusedRowIndex, "ID_ACCION").ToString()) + ", 'INSERT = '||TO_CHAR(SYSDATE,'DD/MM/YYYY HH24:MI')||' '||'" + Session["USUARIO"].ToString() + "', " + Convert.ToInt32(Session["pom"]) + ", " + Convert.ToInt32(Session["insto"]) + ", '" + Session["USUARIO"].ToString() + "')";
                            estado = dao.comando(sql);
                            if (estado == 1)
                            {
                                sql = "SELECT MAX(SPRES$ID_RESULTADO) ID_RESULTADO FROM SCHE$SIPLAN20.SP20$RESULTADOS WHERE SPRES$POM = " + Convert.ToInt32(Session["pom"]) + " AND SPRES$INSTITUCION = " + Convert.ToInt32(Session["insto"]) + " AND SPRES$COD_ESTRATEGICO = " + Convert.ToInt32(gvPGG2428.GetRowValues(gvPGG2428.FocusedRowIndex, "ID_ACCION").ToString()) + " AND SPRES$RESTRICTIVA = 'N'";
                                estado = dao.consulta(sql);
                                if (estado == 1)
                                {
                                    tabla = dao.tabla;
                                    if (tabla.Rows.Count > 0)
                                        rest2 = Convert.ToInt32(tabla.Rows[0]["ID_RESULTADO"]);
                                    else
                                        rest2 = -1;

                                }
                                else
                                {
                                    rest2 = -1;
                                }
                            }

                        }

                    }
                    else
                        rest2 = -1;
                }
                else
                    rest2 = -1;

            }
            else
                rest2 = -1;



            for (int i = 0; i < gvProduccionInsto.VisibleRowCount; i++)
            {

                if (gvProduccionInsto.Selection.IsRowSelected(i))
                {
                    producto = Convert.ToInt32(gvProduccionInsto.GetRowValues(i, "SPPRO$ID_PRODUCTO"));
                    sql = "UPDATE SCHE$SIPLAN20.SP20$PRODUCTO SET SPPRO$ID_RESULTADO = " + rest1 + ",  SPPRO$RESULTADO2 = " + rest2 + ", SPPRO$TEMPORAL = 1, SPPRO$FECHA_UPDATE = 'UDPATE = '||TO_CHAR(SYSDATE,'DD/MM/YYYY HH24:MI')||' '||'" + Session["USUARIO"].ToString() + "' WHERE SPPRO$ID_PRODUCTO = " + producto + " AND SPPRO$POM = " + Session["pom"] + " AND SPPRO$INSTO = " + Session["insto"] + " AND SPPRO$RESTRICTIVA = 'N'";

                    estado = dao.comando(sql);
                    if (estado == 0)
                        break;


                }

            }

            if (estado == 0)
            {
                mensaje = dao.mensaje;
                Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
                popRed.ShowOnPageLoad = false;
                cargaProdutosTemporal(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]));
                gvProduccionInsto.Selection.UnselectAll();
                MultiView1.ActiveViewIndex = 10;

            }
            else
            {
                mensaje = "Productos vinculados RE/PGG";
                Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',1);", true);
                popRed.ShowOnPageLoad = false;
                cargaProdutosTemporal(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]));
                gvProduccionInsto.Selection.UnselectAll();
                MultiView1.ActiveViewIndex = 10;
            }




        }

        protected void btnCancelaRed1_Click(object sender, EventArgs e)
        {
            popRed1eje.ShowOnPageLoad = false;
        }

        protected void btnRed1eje_Click(object sender, EventArgs e)
        {
            DataTable red_resultado = new DataTable();
            int red_producto = -1;
            int producto;
            int rest1 = -1;
            int rest2 = -1;

            if (cbRed1EJE.SelectedIndex == -1 || gvRed1Eeje.FocusedRowIndex == -1)
            {
                mensaje = "Es obligatorio la vinculación a Eje de la PGG y el Resultado Estratégico";
                Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
                return;
            }

            sql = "SELECT * FROM SCHE$SIPLAN20.SP20$RESULTADOS R WHERE R.SPRES$POM = " + Convert.ToInt32(Session["pom"]) + " AND R.SPRES$INSTITUCION = " + Convert.ToInt32(Session["insto"]) + " AND R.SPRES$TIPO = 2 AND R.SPRES$COD_ESTRATEGICO = " + Convert.ToInt32(cbRed1EJE.Value) + " AND SPRES$RESTRICTIVA = 'N'";
            estado = dao.consulta(sql);

            if (estado == 1)
            {
                red_resultado = dao.tabla;
                if (red_resultado.Rows.Count > 0)
                    red_producto = Convert.ToInt32(red_resultado.Rows[0]["SPRES$ID_RESULTADO"]);
                else
                {
                    sql = "INSERT  INTO SCHE$SIPLAN20.SP20$RESULTADOS (SPRES$TIPO, SPRES$COD_ESTRATEGICO, SPRES$FECHA_INSERT, SPRES$POM, SPRES$INSTITUCION, SPRES$PROPIETARIO) VALUES(2, " + cbRed1EJE.Value + ", 'INSERT = '||TO_CHAR(SYSDATE,'DD/MM/YYYY HH24:MI')||' '||'" + Session["USUARIO"].ToString() + "', " + Convert.ToInt32(Session["pom"]) + ", " + Convert.ToInt32(Session["insto"]) + ", '" + Session["USUARIO"].ToString() + "')";
                    estado = dao.comando(sql);
                    if (estado == 1)
                    {
                        sql = "SELECT MAX(SPRES$ID_RESULTADO) ID FROM SCHE$SIPLAN20.SP20$RESULTADOS R WHERE R.SPRES$POM = " + Convert.ToInt32(Session["pom"]) + " AND R.SPRES$INSTITUCION = " + Convert.ToInt32(Session["insto"]) + " AND R.SPRES$TIPO = 2 AND R.SPRES$COD_ESTRATEGICO = " + cbRed1EJE.Value + " AND SPRES$RESTRICTIVA = 'N'";
                        estado = dao.consulta(sql);
                        if (estado == 1)
                        {
                            red_resultado = dao.tabla;
                            if (red_resultado.Rows.Count > 0)
                                red_producto = Convert.ToInt32(red_resultado.Rows[0]["ID"]);
                        }
                    }
                }

            }

            rest1 = red_producto;
            if (gvRed1Eeje.FocusedRowIndex != -1)
            {
                sql = "SELECT * FROM SCHE$SIPLAN20.SP20$RESULTADOS WHERE SPRES$COD_ESTRATEGICO = " + Convert.ToInt32(gvRed1Eeje.GetRowValues(gvRed1Eeje.FocusedRowIndex, "ID_EJE").ToString()) + " AND SPRES$POM = " + Session["pom"] + " AND SPRES$INSTITUCION = " + Session["insto"] + " AND SPRES$RESTRICTIVA = 'N'";
                estado = dao.consulta(sql);
                if (estado == 1)
                {
                    tabla = dao.tabla;
                    if (tabla.Rows.Count > 0)
                    {
                        rest2 = Convert.ToInt32(tabla.Rows[0]["SPRES$ID_RESULTADO"]);
                    }
                    else
                    {
                        sql = "INSERT INTO SCHE$SIPLAN20.SP20$RESULTADOS(SPRES$TIPO,SPRES$COD_ESTRATEGICO, SPRES$FECHA_INSERT, SPRES$POM, SPRES$INSTITUCION, SPRES$PROPIETARIO) VALUES (0, " + Convert.ToInt32(gvRed1Eeje.GetRowValues(gvRed1Eeje.FocusedRowIndex, "ID_EJE").ToString()) + ", 'INSERT = '||TO_CHAR(SYSDATE,'DD/MM/YYYY HH24:MI')||' '||'" + Session["USUARIO"].ToString() + "', " + Convert.ToInt32(Session["pom"]) + ", " + Convert.ToInt32(Session["insto"]) + ", '" + Session["USUARIO"].ToString() + "')";
                        estado = dao.comando(sql);
                        if (estado == 1)
                        {
                            sql = "SELECT MAX(SPRES$ID_RESULTADO) ID_RESULTADO FROM SCHE$SIPLAN20.SP20$RESULTADOS WHERE SPRES$POM = " + Convert.ToInt32(Session["pom"]) + " AND SPRES$INSTITUCION = " + Convert.ToInt32(Session["insto"]) + " AND SPRES$COD_ESTRATEGICO = " + Convert.ToInt32(gvRed1Eeje.GetRowValues(gvRed1Eeje.FocusedRowIndex, "ID_EJE").ToString()) + " AND SPRES$RESTRICTIVA = 'N'";
                            estado = dao.consulta(sql);
                            if (estado == 1)
                            {
                                tabla = dao.tabla;
                                if (tabla.Rows.Count > 0)
                                    rest2 = Convert.ToInt32(tabla.Rows[0]["ID_RESULTAOD"]);

                            }
                        }
                    }

                }

            }
            else
                rest2 = -1;


            for (int i = 0; i < gvProduccionInsto.VisibleRowCount; i++)
            {

                if (gvProduccionInsto.Selection.IsRowSelected(i))
                {
                    producto = Convert.ToInt32(gvProduccionInsto.GetRowValues(i, "SPPRO$ID_PRODUCTO"));
                    sql = "UPDATE SCHE$SIPLAN20.SP20$PRODUCTO SET SPPRO$ID_RESULTADO = " + rest1 + ",  SPPRO$RESULTADO2 = " + rest2 + ", SPPRO$TEMPORAL = 1, SPPRO$FECHA_UPDATE = 'UDPATE = '||TO_CHAR(SYSDATE,'DD/MM/YYYY HH24:MI')||' '||'" + Session["USUARIO"].ToString() + "' WHERE SPPRO$ID_PRODUCTO = " + producto + " AND SPPRO$POM = " + Session["pom"] + " AND SPPRO$INSTO = " + Session["insto"] + " AND SPPRO$RESTRICTIVA = 'N'";
                    estado = dao.comando(sql);
                    if (estado == 0)
                        break;
                }

            }

            if (estado == 0)
            {
                mensaje = dao.mensaje;
                Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
                popRed1eje.ShowOnPageLoad = false;
                cargaProdutosTemporal(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]));
                gvProduccionInsto.Selection.UnselectAll();
                MultiView1.ActiveViewIndex = 10;

            }
            else
            {
                mensaje = "Productos vinculados correctamente a RE/PGG";
                Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',1);", true);
                popRed1eje.ShowOnPageLoad = false;
                cargaProdutosTemporal(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]));
                gvProduccionInsto.Selection.UnselectAll();
                MultiView1.ActiveViewIndex = 10;
            }

        }

        protected DataTable cargaPoliticas(int insto)
        {
            DataTable politicas = new DataTable();
           
           /* sql = @"SELECT 
                        PL.SPPLI$ID ID_LINEA_ACCION
                        ,PC.SPPLE$ID
                        ,PR.SPRP$INSTITUCION INSTO_RESPONSABLE
                        ,P.SPC$DESCRIPCION POLITICA
                        ,PC.SPPLE$CODIGO||' '||PC.SPPLE$DESCRIPCION LINEAMIENTO
                        ,PL.SPPLI$CODIGO||' '||PL.SPPLI$DESCRIPCION LINEA_ACCION
                        ,PC.SPPLE$ORDEN  
                        ,PL.SPPLI$ORDEN
                        FROM SCHE$SIPLAN20.SP20$POLITICA_LINEAMIENTO_ENCABEZA PC
                        INNER JOIN SCHE$SIPLAN20.SP20$POLITICA_CUMPLIMIENTO P ON P.SPC$ID = PC.SPPLE$ID_POLITICA AND P.SPC$RESTRICTIVA = 'N' AND PC.SPPLE$RESTRICTIVA = 'N'
                        INNER JOIN SCHE$SIPLAN20.SP20$POLITICA_LINEAMIENTOS PL ON PL.SPPLI$ID_ENCABEZADO = PC.SPPLE$ID AND PL.SPPLI$RESTRICTIVA = 'N' AND PC.SPPLE$RESTRICTIVA = 'N'
                        INNER JOIN SCHE$SIPLAN20.SP20$RESPONSABLESPOLITICA PR ON PR.SPRP$POLITICA_LINEAMIENTO = PL.SPPLI$ID AND PR.SPRP$RESTRICTIVA = 'N' AND PL.SPPLI$RESTRICTIVA = 'N'
                        WHERE PC.SPPLE$RESTRICTIVA = 'N' AND PL.SPPLI$ARISTA = 1 AND PR.SPRP$INSTITUCION = "+insto+@"  
                        ORDER BY PC.SPPLE$ORDEN, PL.SPPLI$ORDEN ASC";
            */
            sql = @"SELECT 
                      
                        PL.SPPLI$ID ID_LINEA_ACCION
                        ,PC.SPPLE$ID
                       ,-1  INSTO_RESPONSABLE
                        ,P.SPC$DESCRIPCION POLITICA
                        ,PC.SPPLE$CODIGO||' '||PC.SPPLE$DESCRIPCION LINEAMIENTO
                        ,PL.SPPLI$CODIGO||' '||PL.SPPLI$DESCRIPCION LINEA_ACCION
                        ,TO_NUMBER(PC.SPPLE$ORDEN) SPPLE$ORDEN  
                        ,TO_NUMBER(PL.SPPLI$ORDEN) SPPLI$ORDEN
                        
                        FROM SCHE$SIPLAN20.SP20$POLITICA_LINEAMIENTO_ENCABEZA PC
                        INNER JOIN SCHE$SIPLAN20.SP20$POLITICA_CUMPLIMIENTO P ON P.SPC$ID = PC.SPPLE$ID_POLITICA AND P.SPC$RESTRICTIVA = 'N' AND PC.SPPLE$RESTRICTIVA = 'N'
                        INNER JOIN SCHE$SIPLAN20.SP20$POLITICA_LINEAMIENTOS PL ON PL.SPPLI$ID_ENCABEZADO = PC.SPPLE$ID AND PL.SPPLI$RESTRICTIVA = 'N' AND PC.SPPLE$RESTRICTIVA = 'N'
                      
                        WHERE PC.SPPLE$RESTRICTIVA = 'N' 
                        ORDER BY PC.SPPLE$ORDEN, PL.SPPLI$ORDEN ASC";
            estado = dao.consulta(sql);
            if (estado == 1)
                politicas = dao.tabla;
            return politicas;
            
        }

        protected DataTable cargaSubsPP(int pom, int insto)
        {
            DataTable subsPP = new DataTable();
            sql = @"SELECT
                    SPPRO$ID_PRODUCTO
                    ,SPPRO$DESCRIPCION PRODUCTO
                    ,SPPSUB$ID_SUBPRODUCTO
                    ,CASE WHEN SPPSUB$SNIP IS NULL THEN SPPSUB$DESCRIPCION ELSE 'SNIP: '||SPPSUB$SNIP||'-'||PROYECTO END AS SUBPRODUCTO
                    ,CONTEO
                    FROM

                    (SELECT P.SPPRO$ID_PRODUCTO
                            ,P.SPPRO$DESCRIPCION
                            ,S.SPPSUB$ID_SUBPRODUCTO
                            ,S.SPPSUB$DESCRIPCION
                            ,S.SPPSUB$SNIP
                            ,(SELECT NOMBRE FROM SINIP.BP_PROYECTO_ID WHERE PROYECTO = S.SPPSUB$SNIP) PROYECTO 
                            ,nvl((SELECT count(SPS$SUBPRODCUTO) from SCHE$SIPLAN20.SP20$POLITICA_SUBPRODUCTO WHERE SPS$SUBPRODCUTO = S.SPPSUB$ID_SUBPRODUCTO AND SPS$RESTRICTIVA = 'N'),0) CONTEO
                            FROM SCHE$SIPLAN20.SP20$PRODUCTO P
                            INNER JOIN SCHE$SIPLAN20.SP20$SUB_PRODUCTO S ON S.SPPSUB$ID_PRODUCTO = P.SPPRO$ID_PRODUCTO AND S.SPPSUB$RESTRICTIVA = 'N' AND P.SPPRO$RESTRICTIVA = 'N'
                            WHERE P.SPPRO$RESTRICTIVA = 'N' AND  P.SPPRO$POM = "+pom+" AND P.SPPRO$INSTO = "+insto+@")
                            ORDER BY SPPRO$ID_PRODUCTO, SPPSUB$ID_SUBPRODUCTO ASC";

            estado = dao.consulta(sql);
            if (estado == 1)
                subsPP = dao.tabla;

            return subsPP;
        }
        protected void btnPoliticas_Click(object sender, EventArgs e)
        {
            DataTable subsPP = new DataTable();
            subsPP = cargaSubsPP(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["insto"]));
            if (subsPP.Rows.Count > 0)
            {
                Session["subsPP"] = subsPP;
                gvSubproductosPP.DataSource = subsPP;
                gvSubproductosPP.DataBind();
            }
            else
                Session["subsPP"] = null;
            MultiView1.ActiveViewIndex = 11;
        }

        protected void gvSubproductosPP_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
        {
            int valor = -1;

            valor = Convert.ToInt32(e.GetValue("CONTEO"));
            if (valor > 0)
            {
                e.Row.ForeColor = System.Drawing.Color.Blue;
                //e.Row.BackColor = System.Drawing.Color.MediumSeaGreen;
            }

            else
                e.Row.ForeColor = System.Drawing.Color.Black;
        }

        protected void btnPPModal_Click(object sender, EventArgs e)
        {
            DataTable politicas = new DataTable();
            politicas = cargaPoliticas(Convert.ToInt32(Session["insto"]));
            /*chkPoliticas.DataSource = politicas;
            chkPoliticas.TextField = "SPC$DESCRIPCION";
            chkPoliticas.ValueField = "SPC$ID";
            chkPoliticas.DataBind();
            */
            if (politicas.Rows.Count > 0)
            {
                Session["POLITCAS"]= politicas;
                gvPoliticasLA.DataSource = politicas;
                gvPoliticasLA.DataBind();
                gvPoliticasLA.ExpandAll();
                gvPoliticasLA.Selection.UnselectAll();
                popPoliticas.ShowOnPageLoad = true;
            }
            
        }

        protected void btnRegresaSubs_Click(object sender, EventArgs e)
        {
            MultiView1.ActiveViewIndex = 6;
        }

        protected void btnCancelaVincula_Click(object sender, EventArgs e)
        {
            //chkPoliticas.UnselectAll();
            gvSubproductosPP.Selection.UnselectAll();
            gvPoliticasLA.Selection.UnselectAll();
            popPoliticas.ShowOnPageLoad = false;
            
        }

        protected void btnVinculaPolitica_Click(object sender, EventArgs e)
        {
            int busca = -1;
            int cod_sub = -1;
            int cod_politica = -1;
            DataTable subSel = new DataTable();

            if (Session["SUBSEL"] != null)
            {
                if (Session["SUBSEL"] is List<object> selectedKeys && selectedKeys.Count > 0)
                {
                    subSel = ConvertListToDataTable(selectedKeys);
                }

            }
            else
                subSel.Clear();


            if (subSel.Rows.Count > 0)
            {
                for (int i = 0; i < gvPoliticasLA.VisibleRowCount; i++)
                {
                    if (gvPoliticasLA.Selection.IsRowSelected(i))
                    {
                        cod_politica = Convert.ToInt32(gvPoliticasLA.GetRowValues(i, "ID_LINEA_ACCION"));
                        for (int a = 0; a < subSel.Rows.Count; a++)
                        {                            
                                cod_sub = Convert.ToInt32(subSel.Rows[a]["IDSUB"]);
                                busca = buscaPoliticasub(cod_politica, cod_sub);
                                if (busca == -1)
                                    break;
                                else if (busca == 0)
                                {
                                    sql = "INSERT INTO SCHE$SIPLAN20.SP20$POLITICA_SUBPRODUCTO (SPS$POLITICA_LINEAMIENTO, SPS$SUBPRODCUTO, SPS$FECHA_INSERTA) VALUES (" + cod_politica + "," + cod_sub + ",'INSERT = ' || TO_CHAR(SYSDATE, 'DD/MM/YYYY HH24:MI') || ' ' || '" + Session["USUARIO"].ToString() + "')";
                                    estado = dao.comando(sql);
                                    if (estado == 0)
                                        break;
                                }

                            
                        }
                    }
                }

                if (estado == 1)
                {
                    mensaje = "Registros guardados correctamente";
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',1);", true);
                    gvPoliticasLA.Selection.UnselectAll();
                    gvSubproductosPP.Selection.UnselectAll();
                    popPoliticas.ShowOnPageLoad = false;
                    btnPoliticas_Click(sender, e);

                }
                else
                {
                    mensaje = dao.mensaje;
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
                    gvPoliticasLA.Selection.UnselectAll();
                    gvSubproductosPP.Selection.UnselectAll();
                    popPoliticas.ShowOnPageLoad = false;
                    btnPoliticas_Click(sender, e);
                }
            }
            else
            {
                mensaje = "Seleccione por lo menos un subproducto para vincular a la  línea de acción de la política pública";
                Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
                gvPoliticasLA.Selection.UnselectAll();
                gvSubproductosPP.Selection.UnselectAll();
            }

            
        }

        protected int buscaPoliticasub(int politica, int subproducto)
        {
            int busca = -1;
            DataTable vinculaPolitica = new DataTable();
            sql = "SELECT * FROM SCHE$SIPLAN20.SP20$POLITICA_SUBPRODUCTO WHERE SPS$POLITICA_LINEAMIENTO = " + politica + " AND SPS$SUBPRODCUTO = " + subproducto + " AND SPS$RESTRICTIVA = 'N'";
            estado = dao.consulta(sql);
            if (estado == 1)
            {
                vinculaPolitica = dao.tabla;
                if (vinculaPolitica.Rows.Count > 0)
                    busca = 1;
                else
                    busca = 0;
            }    

            else
                busca = -1;
            return busca;
        }

        protected void gvDetallePolitica_BeforePerformDataSelect(object sender, EventArgs e)
        {
            ASPxGridView gvDetail = sender as ASPxGridView;
            int subproducto = -1;
            DataTable politicas = new DataTable();
            subproducto = Convert.ToInt32((sender as ASPxGridView).GetMasterRowFieldValues("SPPSUB$ID_SUBPRODUCTO"));
            politicas = cargaPoliticasSub(subproducto);
            if (politicas.Rows.Count > 0)
            {
                Session["politicasSUB"] = politicas;
                gvDetail.DataSource = politicas;

            }
            else
            {
                Session["politicasSUB"] = null;
                gvDetail.DataSource = null;
                
            }
        }

       /* protected void gvDetallePolitica_DetailRowExpandedChanged(object sender, ASPxGridViewDetailRowEventArgs e)
        {
           
            ASPxGridView gvDetail = sender as ASPxGridView;
            int posi;

            try
            {
                posi = Convert.ToInt32(e.VisibleIndex.ToString());
                Session["indexPoliticas"] = posi;
                gvDetail.FocusedRowIndex = posi;

            }
            catch (Exception error)
            {
                posi = -1;
                Session["indexPoliticas"] = posi;
            }

        }
        */
        protected void gvDetallePolitica_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            ASPxGridView gvDetail = sender as ASPxGridView;
            DataTable politicas = new DataTable();
            politicas = (DataTable)Session["politicasSUB"];
            int codigo = -1;
            if (politicas.Rows.Count > 0)
            {
                for (int i = 0; i <= politicas.Rows.Count -1; i++)
                {
                    if (politicas.Rows[i]["SPS$ID"].ToString() == e.Keys["SPS$ID"].ToString())
                    {
                        codigo = Convert.ToInt32(e.Values["SPS$ID"]);
                        sql = "UPDATE SCHE$SIPLAN20.SP20$POLITICA_SUBPRODUCTO SET SPS$RESTRICTIVA = 'S', SPS$FECHA_ELIMINA = 'ELIMINA = '||TO_CHAR(SYSDATE,'DD/MM/YYYY HH24:MI')||' '||'" + Session["USUARIO"].ToString() + "' WHERE SPS$ID = " + codigo;
                        estado = dao.comando(sql);
                        if (estado == 1)
                        {
                            mensaje = dao.mensaje;
                            gvDetail.JSProperties["cpError"] = "Operacion correcta: Registro borrado correctamente";
                            e.Cancel = true;
                            break;
                        }
                        else
                        {
                            mensaje = dao.mensaje;
                            gvDetail.JSProperties["cpError"] = "Ocurrio un error: " + mensaje;
                            e.Cancel = true;
                            break;
                        }
                    }

                }

            }

        }

        protected DataTable cargaPoliticasSub(int sub)
        {
            DataTable politicaSub = new DataTable();
            sql = @"SELECT PS.SPS$ID, PC.SPC$DESCRIPCION POLITICA, PE.SPPLE$CODIGO||' '||PE.SPPLE$DESCRIPCION LINEAMIENTO,P.SPPLI$CODIGO||' '||P.SPPLI$DESCRIPCION LINEA_ACCION, P.SPPLI$ID
                            FROM SCHE$SIPLAN20.SP20$POLITICA_LINEAMIENTOS P
                            INNER JOIN SCHE$SIPLAN20.SP20$POLITICA_SUBPRODUCTO PS ON PS.SPS$POLITICA_LINEAMIENTO = P.SPPLI$ID AND PS.SPS$RESTRICTIVA = 'N' AND P.SPPLI$RESTRICTIVA = 'N'
                            INNER JOIN SCHE$SIPLAN20.SP20$POLITICA_LINEAMIENTO_ENCABEZA PE ON PE.SPPLE$ID = P.SPPLI$ID_ENCABEZADO AND PE.SPPLE$RESTRICTIVA = 'N' AND P.SPPLI$RESTRICTIVA = 'N' 
                            INNER JOIN SCHE$SIPLAN20.SP20$POLITICA_CUMPLIMIENTO PC ON PC.SPC$ID = PE.SPPLE$ID_POLITICA AND PC.SPC$RESTRICTIVA = 'N' AND PE.SPPLE$RESTRICTIVA = 'N'
                            WHERE PS.SPS$SUBPRODCUTO = " + sub;
            estado = dao.consulta(sql);
            if (estado == 1)
                politicaSub = dao.tabla;
            return politicaSub;
        }


        protected DataTable munoVinculado(int sub)
        {
            DataTable vinculadoSub = new DataTable();
            sql = @"SELECT 
                        SPSM$ID
                        ,DEPARTAMENTO
                        ,MUNICIPIO
                        ,DEPTO
                        ,COD_MUNO
                        FROM
                        (SELECT SM.SPSM$ID
                                ,G.GEOGRAFICO COD_MUNO
                                ,G.NOMBRE MUNICIPIO
                                ,(SELECT S.NOMBRE FROM SINIP.CG_GEOGRAFICO S WHERE TO_NUMBER(S.GEOGRAFICO) = TO_NUMBER(G.DEPTO)) DEPARTAMENTO
                                ,G.DEPTO
                                FROM SCHE$SIPLAN20.SP20$SUB_MUNOS SM 
                                INNER JOIN SINIP.CG_GEOGRAFICO G ON G.GEOGRAFICO = SPSM$GEOGRAFICO AND SM.SPSM$RESTRICTIVA = 'N'
                                WHERE SM.SPSM$ID_SUB = "+sub+@" AND SM.SPSM$RESTRICTIVA = 'N')
                        ORDER BY DEPTO, COD_MUNO ASC";
            estado = dao.consulta(sql);
            if (estado == 1)
                vinculadoSub = dao.tabla;
            return vinculadoSub;
        }


        protected void cargaPoliticaVinculadaSub(int sub)
        {
            DataTable politicasSub = new DataTable();
            politicasSub = cargaPoliticasSub(sub);
            int m1, m2;
            if (politicasSub.Rows.Count > 0)
            {
                gvDetallePoliticaSub.Selection.UnselectAll();
                if (sub != -1)
                {
                    for (int i = 0; i <= gvDetallePoliticaSub.VisibleRowCount - 1; i++)
                    {

                        m1 = Convert.ToInt32(gvDetallePoliticaSub.GetRowValues(i, "ID_LINEA_ACCION").ToString());
                        for (int o = 0; o <= politicasSub.Rows.Count - 1; o++)
                        {


                            m2 = Convert.ToInt32(politicasSub.Rows[o]["SPPLI$ID"].ToString());
                            if (m1 == m2)
                            {
                                gvDetallePoliticaSub.Selection.SelectRow(i);
                            }

                        }


                    }
                }


            }
            else
            {
                if (Session["POLITICASSUB"] != null)
                {
                    politicasSub.Clear();
                    politicasSub = (DataTable)Session["POLITICASSUB"];
                    if (politicasSub.Rows.Count > 0)
                    {
                        gvDetallePoliticaSub.Selection.UnselectAll();
                        for (int i = 0; i <= gvDetallePoliticaSub.VisibleRowCount - 1; i++)
                        {

                            m1 = Convert.ToInt32(gvDetallePoliticaSub.GetRowValues(i, "ID_LINEA_ACCION").ToString());
                            for (int o = 0; o <= politicasSub.Rows.Count - 1; o++)
                            {


                                m2 = Convert.ToInt32(politicasSub.Rows[o]["SPS$ID"].ToString());
                                if (m1 == m2)
                                {
                                    gvDetallePoliticaSub.Selection.SelectRow(i);
                                }

                            }


                        }

                    }

                }
                else
                    gvDetallePoliticaSub.Selection.UnselectAll();
            }
                

        }
        protected void gvSubproductosPP_DetailRowExpandedChanged(object sender, ASPxGridViewDetailRowEventArgs e)
        {
            
            int posi;

            try
            {
                posi = Convert.ToInt32(e.VisibleIndex.ToString());
                Session["indexProduccion"] = posi;
                gvSubproductosPP.FocusedRowIndex = posi;

            }
            catch (Exception error)
            {
                posi = -1;
                Session["indexProduccion"] = posi;
            }

        }

        protected void btnPolitics_Click(object sender, EventArgs e)
        {
            DataTable politicas = new DataTable();
            politicas = cargaPoliticas(Convert.ToInt32(Session["insto"]));
            if (politicas.Rows.Count > 0)
            {
                Session["POLITCAS_SUBFORM"] = politicas;
                gvDetallePoliticaSub.DataSource = politicas;
                gvDetallePoliticaSub.DataBind();
                gvDetallePoliticaSub.ExpandAll();
            }
            else

                Session["POLITCAS_SUBFORM"] = null;


            cargaPoliticaVinculadaSub(Convert.ToInt32(aspCod.Value.ToString()));
            
            popPoliticasFormulario.ShowOnPageLoad = true;
            
        }

       
        protected void btnPoliticasSub_Click(object sender, EventArgs e)
        {

        }

        protected void btnCancelaPoliticasSUb_Click(object sender, EventArgs e)
        {
            popPoliticasFormulario.ShowOnPageLoad = false;
        }

        protected void btnVinculayCerrar_Click(object sender, EventArgs e)
        {
            int busca = -1;
            int cod_politica = -1;            
            politicasSub.Clear();
            int contador = 0;

            if (Convert.ToInt32(aspCod.Value) == -1)
            {
                politicasSub.Columns.Add("SPS$ID", typeof(int));
            }
            else
            {
                politicasSub.Clear();
                contador = Convert.ToInt32(numPoliticas.Text);
            }
                

            for (int i = 0; i < gvDetallePoliticaSub.VisibleRowCount; i++)
            {
                if (gvDetallePoliticaSub.Selection.IsRowSelected(i))
                {
                    cod_politica = Convert.ToInt32(gvDetallePoliticaSub.GetRowValues(i, "ID_LINEA_ACCION"));
                    if (Convert.ToInt32(aspCod.Value) != -1)
                    {
                        busca = buscaPoliticasub(cod_politica, Convert.ToInt32(aspCod.Value));
                        if (busca == -1)
                            break;
                        else if (busca == 0)
                        {
                            sql = "INSERT INTO SCHE$SIPLAN20.SP20$POLITICA_SUBPRODUCTO (SPS$POLITICA_LINEAMIENTO, SPS$SUBPRODCUTO, SPS$FECHA_INSERTA) VALUES (" + cod_politica + "," + Convert.ToInt32(aspCod.Value) + ",'INSERT = ' || TO_CHAR(SYSDATE, 'DD/MM/YYYY HH24:MI') || ' ' || '" + Session["USUARIO"].ToString() + "')";
                            estado = dao.comando(sql);
                            if (estado == 0)
                                break;
                            else
                                contador = contador + 1;
                        }
                        
                    }
                    else
                    {
                        DataRow fila = politicasSub.NewRow();
                        fila["SPS$ID"] = cod_politica;                        
                        politicasSub.Rows.Add(fila);
                        estado = 1;
                        contador = contador + 1;
                    }
                }
                else
                {
                   if(Convert.ToInt32(aspCod.Value) != -1)
                    {
                        busca = buscaPoliticasub(cod_politica, Convert.ToInt32(aspCod.Value));
                        if (busca == 1)
                        {
                            cod_politica = Convert.ToInt32(gvPoliticasLA.GetRowValues(i, "ID_LINEA_ACCION"));
                            sql = "UPDATE SCHE$SIPLAN20.SP20$POLITICA_SUBPRODUCTO SET SPS$RESTRICTIVA = 'S', SPS$FECHA_ELIMINA = 'ELIMINA = ' || TO_CHAR(SYSDATE, 'DD/MM/YYYY HH24:MI') || ' ' || '" + Session["USUARIO"].ToString() + "'  WHERE SPS$POLITICA_LINEAMIENTO = " + cod_politica + " AND SPS$SUBPRODCUTO = " + Convert.ToInt32(aspCod.Value);
                            estado = dao.comando(sql);
                            if (estado == 0)
                                break;
                            else
                                contador = contador - 1;
                        }
                       

                    }
                }

                
            }

            if (estado == 0)
            {
                mensaje = dao.mensaje;
                Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
                gvDetallePoliticaSub.Selection.UnselectAll();
                popPoliticasFormulario.ShowOnPageLoad = false;
                numPoliticas.Text = "0";

            }
            else
            {
                if (politicasSub.Rows.Count > 0 && estado == 1)
                    Session["POLITICASSUB"] = politicasSub;
                else if (estado == 1 && Convert.ToInt32(aspCod.Value) != -1)
                {
                    politicasSub.Clear();
                    Session["POLITICASSUB"] = null;
                }
                    
                gvDetallePoliticaSub.Selection.UnselectAll();
                popPoliticasFormulario.ShowOnPageLoad = false;
                numPoliticas.Text = contador.ToString();

            }

        }

        protected void gvPoliticasLA_CustomColumnSort(object sender, CustomColumnSortEventArgs e)
        {
            try
            {
                if (e.Column != null && e.Column.FieldName == "LINEAMIENTO")
                {
                    object id1 = e.GetRow1Value("SPPLE$ORDEN");
                    object id2 = e.GetRow2Value("SPPLE$ORDEN");
                    int res = Comparer.Default.Compare(id1, id2);
                    if (res == 0)
                    {
                        object nombre1 = e.Value1;
                        object nombre2 = e.Value2;
                        res = Comparer.Default.Compare(nombre1, nombre2);
                    }
                    e.Result = res;
                    e.Handled = true;
                }
                
            }
            catch (Exception exception)
            {
            }
        }

        protected void gvSubproductosPP_SelectionChanged(object sender, EventArgs e)
        {
            List<object> seleccionSub = gvSubproductosPP.GetSelectedFieldValues("SPPSUB$ID_SUBPRODUCTO");
            if (seleccionSub.Count > 0)
                Session["SUBSEL"] = seleccionSub;
            else
                Session["SUBSEL"] = null;
        }

        public static DataTable ConvertListToDataTable(List<object> list)
        {
            DataTable table = new DataTable();
            table.Columns.Add("IDSUB", typeof(object));

            foreach (var item in list)
            {
                table.Rows.Add(item);
            }

            return table;
        }


        public static DataTable ConvertListToDataTableMun(List<object> list)
        {
            DataTable table = new DataTable();
            table.Columns.Add("MUNICIPIO", typeof(object));

            foreach (var item in list)
            {
                table.Rows.Add(item);
            }

            return table;
        }


        protected void cbMunosPriorizados_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Convert.ToInt32(cbMunosPriorizados.Value) == 0)
            {
                munoPriorizados.Attributes.Add("style", "display:none;");
                gvTerritorio.Selection.UnselectAll();
            }
            else
            {
                //ar tipo = aspCod.GetType;
                munoPriorizados.Attributes.Add("style", "display:block;");
                cargaMunicipios(Convert.ToInt32(aspCod.Value));

            }
        }

        protected void rbSNIP_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Convert.ToInt32(rbSNIP.Value) == 0)
            {
                PanSNIP.Attributes.Add("style", "display:none");
                if (Convert.ToInt32(hfNaturaleza.Value) == 0)
                {
                  
                    cbMunosPriorizados.Enabled = false;
                    cbMunosPriorizados.SelectedIndex = 1;
                    munoPriorizados.Attributes.Add("style", "display:block");
                    cargaMunicipios(Convert.ToInt32(aspCod.Value));
                }
                else
                {
                   
                    cbMunosPriorizados.Enabled = false;
                    cbMunosPriorizados.SelectedIndex = 0;
                    munoPriorizados.Attributes.Add("style", "display:none");
                    
                }
             


            }

            else if (Convert.ToInt32(rbSNIP.Value) == 1)
            {
                PanSNIP.Attributes.Add("style", "display:block");
                cbMunosPriorizados.SelectedIndex = 0;
                cbMunosPriorizados.Enabled = false;
                munoPriorizados.Attributes.Add("style", "display:none");

            }

           }

        protected void gvTerritorio_SelectionChanged(object sender, EventArgs e)
        {
            List<object> marcados = gvTerritorio.GetSelectedFieldValues("MUNICIPIO");
            List<object> desmarcados = new List<Object>();

            for (int i = 0; i < gvTerritorio.VisibleRowCount; i++)
            {
                object key = gvTerritorio.GetRowValues(i, "MUNICIPIO");
                if (!marcados.Contains(key) && gvTerritorio.Selection.IsRowSelected(i) == false)
                    desmarcados.Add(key);
            }

            if (marcados.Count > 0)
                Session["MUN_MARCADOS"] = marcados;
            else
                Session["MUN_MARCADOS"] = null;

            if(desmarcados.Count > 0)
                Session["MUN_DESMARCADOS"] = desmarcados;
            else
                Session["MUN_DESMARCADOS"] = null;
        }

        protected void gvMunicipio_vinculado_BeforePerformDataSelect(object sender, EventArgs e)
        {
            ASPxGridView gvDetail = sender as ASPxGridView;
            int subproducto = -1;
            DataTable vinculado = new DataTable();
            subproducto = Convert.ToInt32((sender as ASPxGridView).GetMasterRowFieldValues("SPPSUB$ID_SUBPRODUCTO"));
            vinculado = munoVinculado(subproducto);
            if (vinculado.Rows.Count > 0)
            {
                Session["SUB_municipios"] = vinculado;
                gvDetail.DataSource = vinculado;

            }
            else
            {
                Session["SUB_municipios"] = null;
                gvDetail.DataSource = null;

            }
        }

        protected void gvMunicipio_vinculado_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            ASPxGridView gvDetail = sender as ASPxGridView;
            DataTable vinculados = new DataTable();
            vinculados = (DataTable)Session["SUB_municipios"];
            int codigo = -1;
            if (vinculados.Rows.Count > 0)
            {
                for (int i = 0; i <= vinculados.Rows.Count - 1; i++)
                {
                    if (vinculados.Rows[i]["SPSM$ID"].ToString() == e.Keys["SPSM$ID"].ToString())
                    {
                        codigo = Convert.ToInt32(e.Values["SPSM$ID"]);
                        sql = "UPDATE SCHE$SIPLAN20.SP20$SUB_MUNOS SET SPSM$RESTRICTIVA = 'S', SPSM$FECHA_ELIMINA = 'ELIMINA = '||TO_CHAR(SYSDATE,'DD/MM/YYYY HH24:MI')||' '||'" + Session["USUARIO"].ToString() + "' WHERE SPSM$ID = " + codigo;
                        estado = dao.comando(sql);
                        if (estado == 1)
                        {
                            mensaje = dao.mensaje;
                            gvDetail.JSProperties["cpError"] = "Operacion correcta: Registro borrado correctamente";
                            e.Cancel = true;
                            break;
                        }
                        else
                        {
                            mensaje = dao.mensaje;
                            gvDetail.JSProperties["cpError"] = "Ocurrio un error: " + mensaje;
                            e.Cancel = true;
                            break;
                        }
                    }
                }
            }
        }

        protected void btnGuardaFecha_Click(object sender, EventArgs e)
        {
            sql = "UPDATE SCHE$SIPLAN20.SP20$POM SET SPPO$ESTADO = 'D', SPPO$FECHA_UPDATE = 'PRORROGA = ' || TO_CHAR(SYSDATE, 'DD/MM/YYYY HH24:MI') || ' ' || '" + Session["USUARIO"].ToString() + "'  WHERE SPPO$ID_POM  = " + Convert.ToInt32(gvPOMInsto.GetRowValues(gvPOMInsto.FocusedRowIndex, "SPPO$ID_POM").ToString());
            estado = dao.comando(sql);
            if (estado == 1)
            {
                sql = "UPDATE SCHE$SIPLAN20.SP20$POA SET SPOA$ESTADO = 'D', SPOA$FECHA_APROBACION = 'PRORROGA = ' || TO_CHAR(SYSDATE, 'DD/MM/YYYY HH24:MI') || ' ' || '" + Session["USUARIO"].ToString() + "'  WHERE SPOA$ID_POM  = " + Convert.ToInt32(gvPOMInsto.GetRowValues(gvPOMInsto.FocusedRowIndex, "SPPO$ID_POM").ToString());
                estado = dao.comando(sql);
                if (estado == 1)
                {
                    sql = "INSERT INTO SCHE$SIPLAN20.SP20$FECHAS_CIERRE(SPFC$FECHA_CIERRE,SPFC$CUATRIMESTRE,SPFC$INSERTA,SPFC$EJERCICIO,SPFC$TIPO_FECHA,SPFC$POM,SPFC$INSTITUCION,SPFC$PERIODO_POM) VALUES(TO_DATE('" + txtFechaPorroga.Text + "','DD/MM/YYYY'), 0, 'INSERTA = ' || TO_CHAR(SYSDATE, 'DD/MM/YYYY HH24:MI') || ' ' || '" + Session["USUARIO"].ToString() + "', " + fecha.Year + ", 2," + POM.Value + "," + INSTO.Value + "," + PERIODO.Value + ")";
                    estado = dao.comando(sql);
                    if (estado == 1)
                    {
                        mensaje = "Fecha de prorroga registrada correctamente";
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',1);", true);
                        cargaPOMS();
                        poProrroga.ShowOnPageLoad = false;
                    }
                    else
                    {
                        mensaje = dao.mensaje;
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
                        cargaPOMS();
                        poProrroga.ShowOnPageLoad = false;

                    }
                }
                else
                {
                    mensaje = dao.mensaje;
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
                    cargaPOMS();
                    poProrroga.ShowOnPageLoad = false;
                }

            }
            else
            {
                mensaje = dao.mensaje;
                Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
                cargaPOMS();
                poProrroga.ShowOnPageLoad = false;
            }
        }

        protected bool buscaRespo_politica(int entidad)
        {
            bool reponsable = false;
            //sql = "SELECT COUNT(*) CONTEO FROM SCHE$SIPLAN20.SP20$RESPONSABLESPOLITICA WHERE SPRP$RESTRICTIVA = 'N' AND SPRP$INSTITUCION = " + entidad;
            sql = "SELECT COUNT(*) CONTEO FROM SCHE$SIPLAN20.SP20$RESPONSABLESPOLITICA WHERE SPRP$RESTRICTIVA = 'N'";
            estado = dao.consulta(sql);
            if (estado == 1)
            {
                tabla = dao.tabla;
                if (tabla.Rows.Count > 0)
                {
                    if (Convert.ToInt32(tabla.Rows[0]["CONTEO"]) > 0)
                        reponsable = true;
                    else
                        reponsable = false;
                }
                else
                    reponsable = false;
            }
            else
                reponsable = false;
            return reponsable;
        }

        protected void btnCancelaFecha_Click(object sender, EventArgs e)
        {
            poProrroga.ShowOnPageLoad = false;
        }
    }


}
