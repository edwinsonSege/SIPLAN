using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SIPLAN2._0.DataAccess;
using SIPLAN2._0.Rutas;
namespace SIPLAN2._0.reprogramacion
{
    public partial class reprogramacion : System.Web.UI.Page
    {
        string sql, mensaje;
        int estado;
        string paths = "";
        string full_path = "";
        clsAccesoBBDD dao = new clsAccesoBBDD();
        Rutas.Rutas path = new Rutas.Rutas();
        DataTable tabla = new DataTable();
        DataTable tablaPeriodo = new DataTable();
        DateTime date1 = new DateTime();
        int anio;
        int numero_productos = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            paths = path.carpetas_resolucion();
            full_path = path.full_path_resoluciones();
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
                  

                    if (Session["ROL"].ToString() == "ADMIN" || Session["ROL"].ToString() == "CAPA")
                        btnDesaprobar.Visible = true;
                    else
                        btnDesaprobar.Visible = false;

                    if (Session["ROL"].ToString() == "ENTIDAD")
                    {
                        btnNuevo.Enabled = false;
                        btnEditar.Enabled = false;
                        btnProd.Enabled = false;
                        btnSubprod.Enabled = false;
                        btnBorrar.Enabled = false;
                        btnAprobar.Enabled = false;
                    }

                   
                        
                    if (Session["ROL"].ToString() == "ADMIN" || Convert.ToInt32(Session["Insto"]) == 54000)
                    {
                        btnFinancieraProd.Visible = true;
                        btnBorrarReproFin.Visible = true;
                    }
                    else
                    {
                        btnFinancieraProd.Visible = false;
                        btnBorrarReproFin.Visible = false;
                    }
                        if (!IsPostBack)
                    {
                        cargaPeriodos();
                        


                        sql = "SELECT * FROM SCHE$SIPLAN20.SP20$POM P WHERE P.SPPO$RESTRICTIVA ='N' AND P.SPPO$ID_INSTITUCION = " + Session["Insto"]+ " AND P.SPPO$ID_PERIODO ="+cbPeriodos.Value;

                        estado = dao.consulta(sql);
                        if (estado == 1)
                        {
                            tabla = dao.tabla;
                            if (tabla.Rows.Count > 0)
                            {
                                Session["pom"] = tabla.Rows[0]["SPPO$ID_POM"];
                            }
                               
                            else
                            {
                                Session["pom"] = null;
                                mensaje = cbInstituiciones.Text+ " no tiene programación ingresada para el año " + cbanio.Value + " periodo " + cbPeriodos.Text; ;
                                gvReprogramaciónes.DataSource = null;
                                gvReprogramaciónes.DataBind();
                                Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
                            }

                                
                        }

                        anios();
                        //cargaPeriodos();
                        cargaCombos();
                        if (Session["pom"] != null)
                        {



                            sql = "SELECT * FROM SCHE$SIPLAN20.SP20$POA WHERE SPOA$ANIO = " + cbanio.Value + " AND SPOA$ID_POM = " + Session["pom"] + " AND SPOA$ID_INSTITUCION = " + cbInstituiciones.Value + " AND SPOA$RESTRICTIVA = 'N'";
                            estado = dao.consulta(sql);
                            if (estado == 1)
                            {
                                tabla = dao.tabla;
                                if (tabla.Rows.Count > 0)
                                {
                                    Session["poa"] = tabla.Rows[0]["SPOA$ID_POA"];
                                }
                                else
                                {
                                    Session["poa"] = null;
                                    gvReprogramaciónes.DataSource = null;
                                    gvReprogramaciónes.DataBind();
                                    mensaje = cbInstituiciones.Text + " no tiene programación ingresada para el año "+cbanio.Value+" periodo "+cbPeriodos.Text;
                                    Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);

                                }
                                

                            }
                        }

                        if(Session["pom"] != null && Session["poa"]!= null)
                        cargaReprogrogramaciones(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["poa"]));
                    }

                   else
                    {



                        sql = "SELECT * FROM SCHE$SIPLAN20.SP20$PERIODO WHERE SPP$VIGENTE = 1 AND SPP$RESTRICTIVA = 'N' AND SPP$ID_PERIODO = " + cbPeriodos.Value;
                        estado = dao.consulta(sql);
                        if (estado == 1)
                        {
                            tablaPeriodo = dao.tabla;
                            if (tablaPeriodo.Rows.Count > 0)
                                Session["VIGENTE"] = 1;
                            else
                                Session["VIGENTE"] = null;
                        }

                        if (Session["VIGENTE"] == null || Convert.ToInt32(Session["VIGENTE"]) != 1)
                        {
                            btnNuevo.Enabled = false;
                            btnEditar.Enabled = false;
                            btnProd.Enabled = false;
                            btnSubprod.Enabled = false;
                            btnBorrar.Enabled = false;
                            btnAprobar.Enabled = false;
                        }

                        else if (Convert.ToInt32(Session["VIGENTE"]) == 1 && Session["ROL"].ToString() != "ENTIDAD")
                        {
                            btnNuevo.Enabled = true;
                            btnEditar.Enabled = true;
                            btnProd.Enabled = true;
                            btnSubprod.Enabled = true;
                            btnBorrar.Enabled = true;
                        }





                        sql = "SELECT * FROM SCHE$SIPLAN20.SP20$POM P WHERE P.SPPO$RESTRICTIVA ='N' AND P.SPPO$ID_INSTITUCION = " + cbInstituiciones.Value + " AND P.SPPO$ID_PERIODO =" + cbPeriodos.Value;
                        estado = dao.consulta(sql);
                        tabla = dao.tabla;

                        if (tabla.Rows.Count > 0)
                        {
                            Session["pom"] = tabla.Rows[0]["SPPO$ID_POM"];



                            if (Session["carga"] == null)
                            {
                                if (Session["ROL"].ToString() != "USER")
                                {
                                    sql = "SELECT * FROM SCHE$SIPLAN20.SP20$POM P WHERE P.SPPO$RESTRICTIVA ='N' AND P.SPPO$ID_INSTITUCION = " + cbInstituiciones.Value + " AND P.SPPO$ID_PERIODO = " + cbPeriodos.Value;
                                    estado = dao.consulta(sql);
                                    if (estado == 1)
                                    {
                                        tabla = dao.tabla;
                                        if (tabla.Rows.Count > 0)
                                            Session["pom"] = tabla.Rows[0]["SPPO$ID_POM"];
                                        else
                                        {
                                            Session["pom"] = null;
                                            gvReprogramaciónes.DataSource = null;
                                            gvReprogramaciónes.DataBind();
                                            mensaje = cbInstituiciones.Text + " no tiene programación ingresada para el año " + cbanio.Value + " periodo " + cbPeriodos.Text;
                                            Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
                                        }

                                    }

                                    if (Session["pom"] != null)
                                    {
                                        sql = "SELECT * FROM SCHE$SIPLAN20.SP20$POA WHERE SPOA$ANIO = " + cbanio.Value + " AND SPOA$ID_POM = " + Session["pom"] + " AND SPOA$ID_INSTITUCION = " + cbInstituiciones.Value + " AND SPOA$RESTRICTIVA = 'N'";
                                        estado = dao.consulta(sql);
                                        if (estado == 1)
                                        {
                                            tabla = dao.tabla;
                                            if (tabla.Rows.Count > 0)
                                                Session["poa"] = tabla.Rows[0]["SPOA$ID_POA"];
                                            else
                                            {
                                                Session["poa"] = null;
                                                gvReprogramaciónes.DataSource = null;
                                                gvReprogramaciónes.DataBind();
                                                mensaje = cbInstituiciones.Text + " no tiene programación ingresada para el año " + cbanio.Value + " periodo " + cbPeriodos.Text;
                                                Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);


                                            }
                                        }
                                    }

                                }


                                if (Session["pom"] != null)
                                {
                                    sql = "SELECT * FROM SCHE$SIPLAN20.SP20$POA WHERE SPOA$ANIO = " + cbanio.Value + " AND SPOA$ID_POM = " + Session["pom"] + " AND SPOA$ID_INSTITUCION = " + cbInstituiciones.Value + " AND SPOA$RESTRICTIVA = 'N'";
                                    estado = dao.consulta(sql);
                                    if (estado == 1)
                                    {
                                        tabla = dao.tabla;
                                        if (tabla.Rows.Count > 0)
                                            Session["poa"] = tabla.Rows[0]["SPOA$ID_POA"];
                                        else
                                        {
                                            Session["poa"] = null;
                                            gvReprogramaciónes.DataSource = null;
                                            gvReprogramaciónes.DataBind();
                                            mensaje = cbInstituiciones.Text + " no tiene programación ingresada para el año " + cbanio.Value + " periodo " + cbPeriodos.Text;
                                            Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);


                                        }
                                    }
                                }

                                if (Session["pom"] != null && Session["poa"] != null)
                                    cargaReprogrogramaciones(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["poa"]));

                                ScriptManager scripManager = ScriptManager.GetCurrent(this.Page);

                                scripManager.RegisterPostBackControl(btnGrabaRepro);
                            }

                            else if (Session["carga"] != null && Convert.ToInt32(Session["carga"]) == 1)
                            {
                                //Session["poa"] = gvReprogramaciónes.GetRowValues(gvReprogramaciónes.FocusedRowIndex, "SPPRE$POA");
                                //Session["reprogramacion"] = gvReprogramaciónes.GetRowValues(gvReprogramaciónes.FocusedRowIndex, "SPPRE$ID_REPRO");                           
                                cargaproductos(Convert.ToInt32(Session["poa"]), Convert.ToInt32(Session["reprogramacion"]), Convert.ToInt32(cbanio.Value));
                            }

                            else if (Session["carga"] != null && Convert.ToInt32(Session["carga"]) == 2)
                            {
                                cargasubproductos(Convert.ToInt32(Session["poa"]), Convert.ToInt32(Session["reprogramacion"]), Convert.ToInt32(cbanio.Value));
                            }
                        }

                        else
                        {
                            Session["pom"] = null;
                            gvReprogramaciónes.DataSource = null;
                            gvReprogramaciónes.DataBind();
                            mensaje = cbInstituiciones.Text + " no tiene programación ingresada para el año " + cbanio.Value + " periodo " + cbPeriodos.Text;
                            Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "Alerta('" + mensaje + " ',2);", true);
                        }

                        

                        
                    }


                    btnAprobar.Attributes.Add("onclick", "return confirm('Esta por aprobar esta reprogramación, las metas modificadas en esta reprogramación quedarán vigentes. Luego de aprobar la misma no podra modificar la reprogramación seleccionada, por favor confirme la operación')");

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

        protected void btnNuevo_Click(object sender, EventArgs e)
        {
            int estados;
            Session["operacion"] = 0;
            DataTable tablas = new DataTable();

            if (Session["pom"] != null && Session["poa"] != null)
            {

                tablas = noAProbados(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["poa"]));

                if (tablas.Rows.Count > 0)
                {
                    mensaje = "Hay reprogramaciónes pendientes de aprobar. Deber de aprobar las que esten pendientes antes de ingresar una reprogramación nueva.";
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, GetType(), "script", "Alerta('" + mensaje + " <br/>',2);", true);
                }

                else
                {
                    estados = poaEnviado(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["poa"]));

                    if (estados == 1)
                    {
                        Fecha_resolucion.Text = "";
                        txtResolución.Text = "";
                        txtObservacion.Text = "";
                        archivo.Visible = false;
                        txtResolución.Enabled = true;
                        Fecha_resolucion.Enabled = true;
                        txtObservacion.Enabled = true;
                        btnGrabaRepro.Enabled = true;
                        popProgramacion.HeaderText = "Resolución";
                        btnGrabaRepro.Text = "Grabar";
                        btnGrabaRepro.CssClass = "btn btn-success";
                        popProgramacion.ShowOnPageLoad = true;
                    }

                    else
                    {
                        if (Session["FECHA_CIERRA"] != null)
                        {
                            mensaje = "Su programación no se encuentra cerrada, el periodo para ingreso y modificación de la programación y metas se encuentra vigente, puede hacer uso para este modulo de reprogramaciones a partir del " + Session["FECHA_CIERRA"].ToString();
                            ScriptManager.RegisterStartupScript(this.UpdatePanel1, GetType(), "script", "Alerta('" + mensaje + " <br/>',3);", true);
                        }
                        
                    }
                }

                
            }
               
            else
            {
                mensaje = cbInstituiciones.Text +" , no tiene programación ingresada para el año "+cbanio.Value+" periodo "+cbPeriodos.Text;
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, GetType(), "script", "Alerta('" + mensaje + " <br/>',2);", true);
            }
        }

        protected void cargaReprogrogramaciones(int pom, int poa)
        {
            int periodo = 0;
            sql = @"SELECT R.SPPRE$ID_REPRO
                    ,CASE WHEN R.SPPRE$APROBADO = 0 THEN 'NO' ELSE 'SI' END AS SPPRE$APROBADO
                    ,CASE WHEN R.SPPRE$ARCHIVO IS NULL THEN 'NO' ELSE 'SI' END AS ARCHIVO
                    ,R.SPPRE$RESOLUCION
                    ,TO_CHAR(R.SPPRE$FECHA_RESOLUCION, 'DD/MM/YYYY') SPPRE$FECHA_RESOLUCION
                    ,R.SPPRE$OBSERVACION
                    ,R.SPPRE$FECHA_INGRESO
                    ,R.SPPRE$FECHA_APRUEBA
                    ,R.SPPRE$USUARIOAPRUEBA
                    ,CASE WHEN R.SPPRE$VIGENTE = 0 THEN 'NO' ELSE 'SI' END AS VIGENTE
                    ,R.SPPRE$ARCHIVO
                    ,R.SPPRE$POM
                    ,R.SPPRE$POA  
                    FROM SCHE$SIPLAN20.SP20$REPROGRAMACION R 
                    WHERE R.SPPRE$POM = " + pom+ @" 
                    AND R.SPPRE$POA = "+poa+ @"  
                    AND R.SPPRE$RESTRICTIVA = 'N' 
                    ORDER BY R.SPPRE$FECHA_RESOLUCION, R.SPPRE$ID_REPRO ASC";
            estado = dao.consulta(sql);
            if (estado == 1)
            {
                tabla = dao.tabla;
                gvReprogramaciónes.DataSource = tabla;
                gvReprogramaciónes.DataBind();

                sql = "SELECT * FROM SCHE$SIPLAN20.SP20$POM WHERE SPPO$ID_POM = "+pom+ " AND SPPO$RESTRICTIVA = 'N'";
                estado = dao.consulta(sql);
                if (estado == 1)
                {
                    tabla = dao.tabla;
                    if (tabla.Rows.Count > 0)
                    {
                        periodo = Convert.ToInt32(tabla.Rows[0]["SPPO$ID_PERIODO"]);
                        if (periodo == 23 || periodo == 24)
                        {
                            
                                sql = @"SELECT COUNT(*) CONTEO FROM SCHE$SIPLAN20.SP20$PRODUCTO P 
                                        INNER JOIN SCHE$SIPLAN20.SP20$RESULTADOS R ON P.SPPRO$ID_RESULTADO = R.SPRES$ID_RESULTADO AND P.SPPRO$RESTRICTIVA = 'N' AND R.SPRES$RESTRICTIVA = 'N'
                                        INNER JOIN SCHE$SIPLAN20.SP20$RESULTADOS_ESTRATEGICOS ER ON ER.SPPRES$ID_RESULTADO = R.SPRES$COD_ESTRATEGICO AND ER.SPPRES$RESTRICTIVA = 'N' AND R.SPRES$RESTRICTIVA = 'N'
                                        INNER JOIN SCHE$SIPLAN20.SP20$RESULTADOS_ESTRATEGICOS PGG ON ER.SPPRES$DEPENDE = PGG.SPPRES$ID_RESULTADO AND ER.SPPRES$RESTRICTIVA = 'N' AND PGG.SPPRES$RESTRICTIVA = 'N'
                                        INNER JOIN SCHE$SIPLAN20.S20$PERIODO_RESULTADO PR ON PR.SPR$ID_RESULTADO = PGG.SPPRES$ID_RESULTADO AND PGG.SPPRES$RESTRICTIVA = 'N' AND PR.SPR$RESTRICTIVA = 'N'
                                        INNER JOIN SCHE$SIPLAN20.SP20$PERIODO_GOBIERNO PG ON PG.SPG$ID_PERIODO = PR.SPR$ID_PERIODO AND PG.SPG$RESTRICTIVA = 'N' AND PR.SPR$RESTRICTIVA = 'N'
                                        INNER JOIN SCHE$SIPLAN20.SP20$POM POM ON POM.SPPO$ID_POM = P.SPPRO$POM AND P.SPPRO$RESTRICTIVA = 'N' AND POM.SPPO$RESTRICTIVA = 'N'
                                        WHERE PG.SPG$ID_PERIODO = 0 AND POM.SPPO$ID_PERIODO = " + periodo + @" AND POM.SPPO$ID_POM = " +pom+ " AND R.SPRES$TIPO = 0";


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
                            

                        }

                    }
                }
            }

        }

        protected bool valida_fecha(int pom, int poa, string fecha)
        {
            bool valida = false;
            string fecha2 = "";
            DateTime date1 = new DateTime(2015, 1, 1, 0, 0, 0);
            DateTime date2 = new DateTime(2015, 1, 1, 0, 0, 0);

            sql = @"SELECT R.SPPRE$ID_REPRO
                    ,TO_CHAR(R.SPPRE$FECHA_RESOLUCION,'dd/mm/yyyy') SPPRE$FECHA_RESOLUCION
                    FROM SCHE$SIPLAN20.SP20$REPROGRAMACION R 
                    WHERE R.SPPRE$POM = " + pom+@" 
                    AND R.SPPRE$POA = "+poa+@" 
                    AND R.SPPRE$RESTRICTIVA = 'N' 
                    AND R.SPPRE$APROBADO = 1 
                    ORDER BY R.SPPRE$FECHA_RESOLUCION DESC";

            estado = dao.consulta(sql);
            if (estado == 1)
            {
                tabla = dao.tabla;
                if (tabla.Rows.Count > 0)
                {
                    fecha2 = tabla.Rows[0]["SPPRE$FECHA_RESOLUCION"].ToString();
                    date1 = DateTime.ParseExact(fecha, "dd/MM/yyyy", null);
                    date2 = DateTime.ParseExact(fecha2, "dd/MM/yyyy", null);
                    //if (Convert.ToDateTime(fecha).Date < Convert.ToDateTime(tabla.Rows[0]["SPPRE$FECHA_RESOLUCION"]).Date)
                    if (date1 < date2)
                        valida = false;
                    else
                        valida = true;
                }
                else
                    valida = true;

            }
            else
                valida = false;

            return valida;

        }
        protected void btnGrabaRepro_Click(object sender, EventArgs e)
        {
            bool valida =  false;

            valida = valida_fecha(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["poa"]), Fecha_resolucion.Text);
            if (valida == false)
            {
                mensaje = "La reprogramación no puede ser registrada ya que la resolución tiene una fecha menor a la ultima reprogramación aprobada, debe ingresar las mismas en orden cronológico";
                ScriptManager.RegisterStartupScript(this.upPopInstrument, GetType(), "script", "Alerta('" + mensaje + " <br/>',2);", true);
                Fecha_resolucion.Focus();
            }

            else if (txtResolución.Text == "")
            {
                mensaje = "El numero de resolución es necesario";
                ScriptManager.RegisterStartupScript(this.upPopInstrument, GetType(), "script", "Alerta('" + mensaje + " <br/>',2);", true);
                txtResolución.Focus();
            }

            else if (Fecha_resolucion.Text == "")
            {
                mensaje = "La fecha de la resolución es necesaria";
                ScriptManager.RegisterStartupScript(this.upPopInstrument, GetType(), "script", "Alerta('" + mensaje + " <br/>',2);", true);
                Fecha_resolucion.Focus();
            }

            else if (txtObservacion.Text == "")
            {
                mensaje = "Ingrese una breve descripción de la reprogramación";
                ScriptManager.RegisterStartupScript(this.upPopInstrument, GetType(), "script", "Alerta('" + mensaje + " <br/>',2);", true);
                Fecha_resolucion.Focus();
            }



            else
            {
                if (Convert.ToInt32(Session["operacion"]) == 0)
                {
                    sql = "INSERT INTO SCHE$SIPLAN20.SP20$REPROGRAMACION(SPPRE$RESOLUCION, SPPRE$ARCHIVO, SPPRE$RUTA, SPPRE$OBSERVACION, SPPRE$POM, SPPRE$POA, SPPRE$FECHA_INSERT, SPPRE$FECHA_RESOLUCION, SPPRE$FECHA_INGRESO) VALUES ('" + txtResolución.Text + "',";
                    if (fileInstrumento.HasFile)
                    {
                        SaveFile(fileInstrumento.PostedFile);
                        if (Session["archivo"] != null && Session["carpeta"] != null)
                        {
                            sql = sql + "'" + Session["archivo"] + "', '" + Session["carpeta"] + "',";
                        }
                        else
                        {
                            sql = sql + "NULL, NULL,";
                        }
                    }
                    else
                    {
                        sql = sql + "NULL, NULL,";
                    }

                    if (txtObservacion.Text != "")
                        sql = sql + "'" + txtObservacion.Text + "',";
                    else
                        sql = sql + "NULL,";

                    sql = sql + Convert.ToInt32(Session["pom"]) + "," + Convert.ToInt32(Session["poa"]) + ", 'INSERT = '||TO_CHAR(SYSDATE,'DD/MM/YYYY HH24:MI')||' '||'" + Session["USUARIO"].ToString() + "',  TO_DATE('" + Fecha_resolucion.Text + "','DD/MM/YYYY HH24:MI'),  TO_CHAR(SYSDATE,'DD/MM/YYYY HH24:MI')) ";


                    estado = dao.comando(sql);
                    if (estado == 1)
                    {
                        mensaje = "El encabezado de la reprogramación ha sido creado correctamente";
                        ScriptManager.RegisterStartupScript(this.upPopInstrument, GetType(), "script", "Alerta('" + mensaje + " <br/>',1);", true);
                        txtResolución.Text = "";
                        Fecha_resolucion.Text = "";
                        txtObservacion.Text = "";
                        popProgramacion.ShowOnPageLoad = false;
                        cargaReprogrogramaciones(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["poa"]));
                    }
                    else
                    {
                        mensaje = dao.mensaje;
                        ScriptManager.RegisterStartupScript(this.upPopInstrument, GetType(), "script", "Alerta('" + mensaje + " <br/>',2);", true);
                        cargaReprogrogramaciones(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["poa"]));
                    }
                }

                else if (Convert.ToInt32(Session["operacion"]) == 1)
                {
                    sql = "UPDATE SCHE$SIPLAN20.SP20$REPROGRAMACION SET SPPRE$RESOLUCION = '" + txtResolución.Text + "',";
                    if (fileInstrumento.HasFile)
                    {
                        SaveFile(fileInstrumento.PostedFile);
                        if (Session["archivo"] != null && Session["carpeta"] != null)
                        {
                            sql = sql + "SPPRE$ARCHIVO = '" + Session["archivo"] + "', SPPRE$RUTA = '" + Session["carpeta"] + "',";
                        }

                    }


                    if (txtObservacion.Text != "")
                        sql = sql + "SPPRE$OBSERVACION = '" + txtObservacion.Text + "',";
                    else
                        sql = sql + " SPPRE$OBSERVACION = NULL,";

                    sql = sql + "SPPRE$FECHA_UPDATE = 'UPDATE = '||TO_CHAR(SYSDATE,'DD/MM/YYYY HH24:MI')||' '||'" + Session["USUARIO"].ToString() + "', SPPRE$FECHA_RESOLUCION =  TO_DATE('" + Fecha_resolucion.Text + "','DD/MM/YYYY HH24:MI') WHERE SPPRE$ID_REPRO = " + gvReprogramaciónes.GetRowValues(gvReprogramaciónes.FocusedRowIndex, "SPPRE$ID_REPRO") + " AND  SPPRE$POM = " + gvReprogramaciónes.GetRowValues(gvReprogramaciónes.FocusedRowIndex, "SPPRE$POM") + " AND SPPRE$POA = " + gvReprogramaciónes.GetRowValues(gvReprogramaciónes.FocusedRowIndex, "SPPRE$POA");


                    estado = dao.comando(sql);
                    if (estado == 1)
                    {
                        mensaje = "El encabezado de la reprogramación ha sido editado correctamente";
                        ScriptManager.RegisterStartupScript(this.upPopInstrument, GetType(), "script", "Alerta('" + mensaje + " <br/>',1);", true);
                        txtResolución.Text = "";
                        Fecha_resolucion.Text = "";
                        txtObservacion.Text = "";

                        popProgramacion.ShowOnPageLoad = false;
                        cargaReprogrogramaciones(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["poa"]));
                    }
                    else
                    {
                        mensaje = dao.mensaje;
                        ScriptManager.RegisterStartupScript(this.upPopInstrument, GetType(), "script", "Alerta('" + mensaje + " <br/>',2);", true);
                        cargaReprogrogramaciones(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["poa"]));
                    }
                }


                else if (Convert.ToInt32(Session["operacion"]) == 2)
                {
                    valida = borra_reprog(Convert.ToInt32(gvReprogramaciónes.GetRowValues(gvReprogramaciónes.FocusedRowIndex, "SPPRE$ID_REPRO")));
                    if (valida == true)
                    {

                        sql = "UPDATE SCHE$SIPLAN20.SP20$REPROGRAMACION SET SPPRE$RESTRICTIVA = 'S',  SPPRE$FECHA_DELETE = 'DELETE = '||TO_CHAR(SYSDATE,'DD/MM/YYYY HH24:MI')||' '||'" + Session["USUARIO"].ToString() + "'  WHERE SPPRE$ID_REPRO = " + gvReprogramaciónes.GetRowValues(gvReprogramaciónes.FocusedRowIndex, "SPPRE$ID_REPRO");
                        estado = dao.comando(sql);
                        if (estado == 1)
                        {
                            sql = "UPDATE SCHE$SIPLAN20.SP20$REPROGRAPRODUCTO SET SPPRP$RESTRICTIVA = 'S', SPPRP$FECHA_DELETE = 'DELETE = '||TO_CHAR(SYSDATE,'DD/MM/YYYY HH24:MI')||' '||'" + Session["USUARIO"].ToString() + "' WHERE SPPRP$REPROGRAMACION = " + gvReprogramaciónes.GetRowValues(gvReprogramaciónes.FocusedRowIndex, "SPPRE$ID_REPRO");
                            estado = dao.comando(sql);
                            if (estado == 1)
                            {
                                sql = "UPDATE SCHE$SIPLAN20.SP20$REPROGRASUB SET SPPRS$RESTRICTIVA = 'S', SPPRS$FECHA_DELETE = 'DELETE = '||TO_CHAR(SYSDATE,'DD/MM/YYYY HH24:MI')||' '||'" + Session["USUARIO"].ToString() + "' WHERE SPPRS$REPROGRAMACION = " + gvReprogramaciónes.GetRowValues(gvReprogramaciónes.FocusedRowIndex, "SPPRE$ID_REPRO");
                                estado = dao.comando(sql);
                                if (estado == 1)
                                {
                                    mensaje = "Reprogramación eliminada correctamente";
                                    ScriptManager.RegisterStartupScript(this.upPopInstrument, GetType(), "script", "Alerta('" + mensaje + " <br/>',1);", true);
                                    popProgramacion.ShowOnPageLoad = false;
                                    cargaReprogrogramaciones(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["poa"]));
                                }
                                else
                                {
                                    mensaje = dao.mensaje;
                                    ScriptManager.RegisterStartupScript(this.upPopInstrument, GetType(), "script", "Alerta('" + mensaje + " <br/>',2);", true);
                                    popProgramacion.ShowOnPageLoad = false;
                                    cargaReprogrogramaciones(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["poa"]));
                                }
                            }
                        }


                    }
                    else
                    {
                        mensaje = "Ha ocurrido un error y la reprogramación no ha podido ser borrada correctamente, contacte al administrador";
                        ScriptManager.RegisterStartupScript(this.upPopInstrument, GetType(), "script", "Alerta('" + mensaje + " <br/>',2);", true);
                        popProgramacion.ShowOnPageLoad = false;
                        cargaReprogrogramaciones(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["poa"]));
                    }
                }

            }
        }

        protected void btnCancelarRepro_Click(object sender, EventArgs e)
        {
            txtResolución.Text = "";
            Fecha_resolucion.Text = "";
            txtObservacion.Text = "";
            popProgramacion.ShowOnPageLoad = false;
        }

        protected void SaveFile(HttpPostedFile file)
        {

            // Specify the path to save the uploaded file to.

           string savePath = full_path;

        //string savePath = "E:\\Documentos\\resoluciones\\";

            //string savePath = "C:\\Fuentes\\SIPLAN\\SIPLAN2.0(MIGRACIONES)\\SIPLAN2.0\\Documentos\\";
           

            //string savePath = Server.MapPath("\\") + "Documentos\\";
            //string savePath = Server.MapPath("\\siplan\\") + "Documentos\\";
            // Get the name of the file to upload.
            string fileName = Convert.ToString(cbInstituiciones.Value)+"_"+Convert.ToString(Session["pom"]) + "_" + Convert.ToString(Session["poa"]) + "_" + fileInstrumento.FileName;




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

        protected void gvReprogramaciónes_HtmlDataCellPrepared(object sender, DevExpress.Web.ASPxGridViewTableDataCellEventArgs e)
        {
            String valor, valor2;
            valor2 = "";
            if (e.DataColumn.FieldName == "ARCHIVO")
            {
                if (e.CellValue != DBNull.Value)
                {
                    valor = e.CellValue.ToString();
                    if (valor == "NO")
                    {
                        e.Cell.ForeColor = Color.Red;

                    }
                    else if (valor == "SI" )
                    {

                        valor2 = gvReprogramaciónes.GetRowValues(Convert.ToInt32(e.VisibleIndex), "SPPRE$APROBADO").ToString();

                        if (valor2 == "SI")
                        e.Cell.ForeColor = Color.Blue;
                        else
                            e.Cell.ForeColor = Color.Green;
                    }

                }
            }

            if (e.DataColumn.FieldName == "SPPRE$APROBADO")
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
                        e.Cell.ForeColor = Color.Blue;
                    }

                }
            }

            if (e.DataColumn.FieldName == "VIGENTE")
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
                        e.Cell.ForeColor = Color.Blue;
                    }

                }
            }
        }

        protected void btnVista_Click(object sender, EventArgs e)
        {
             String path;
             String nombredocumento;
             String url;
             if (gvReprogramaciónes.FocusedRowIndex != -1)
             {

                 if (gvReprogramaciónes.GetRowValues(gvReprogramaciónes.FocusedRowIndex, "ARCHIVO").ToString() == "NO")
                 {
                     mensaje = "La reprogramación consultada no tiene archivo adjunto, por favor publique el mismo para desplegarlo en vista previa";
                     ScriptManager.RegisterStartupScript(this.UpdatePanel1, GetType(), "script", "Alerta('" + mensaje + " <br/>',2);", true);
                 }
                 else
                 {

                     path = Path.GetFullPath("E:\\Documentos\\");//@"http://siseg.segeplan.gob.gt/Documentos/";
                     nombredocumento = gvReprogramaciónes.GetRowValues(gvReprogramaciónes.FocusedRowIndex, "SPPRE$ARCHIVO").ToString();
                    //url = "../../Documentos/"+nombredocumento; 
                    //ScriptManager.RegisterStartupScript(panBotonArchivos, panBotonArchivos.GetType(), "script", "abrirVentana('"+url+"');", true);
                    
                   popDocumentores.ContentUrl = paths + gvReprogramaciónes.GetRowValues(gvReprogramaciónes.FocusedRowIndex, "SPPRE$ARCHIVO").ToString();
                   //popDocumentores.ContentUrl = "../../Documentos/resoluciones/" + gvReprogramaciónes.GetRowValues(gvReprogramaciónes.FocusedRowIndex, "SPPRE$ARCHIVO").ToString();
                    
                    //vstPrevia.Src = url;
                    //vstPrevia.Src = "../Documentos/"+ gvDocumentos.GetRowValues(gvDocumentos.FocusedRowIndex, "SPPDOC$NOMARCHIVO").ToString();
                    //vstPrevia.Src = "../Documentos/" + gvDocumentos.GetRowValues(gvDocumentos.FocusedRowIndex, "SPPDOC$NOMARCHIVO").ToString();
                    popDocumentores.ShowOnPageLoad = true;
                 }

             }
             else
             {
                 mensaje = "Seleccione una fila con una reprogramación";
                 ScriptManager.RegisterStartupScript(this.UpdatePanel1, GetType(), "script", "Alerta('" + mensaje + " <br/>',2);", true);
             }
            
        }

        protected void btnEditar_Click(object sender, EventArgs e)
        {
            Session["operacion"] = 1;
            if (gvReprogramaciónes.FocusedRowIndex != -1)
            {
                if (gvReprogramaciónes.GetRowValues(gvReprogramaciónes.FocusedRowIndex, "SPPRE$APROBADO").ToString() == "SI")
                {
                    txtResolución.Enabled = false;
                    Fecha_resolucion.Enabled = false;
                    btnGrabaRepro.Enabled = false;
                    txtObservacion.Enabled = false;
                    if (gvReprogramaciónes.GetRowValues(gvReprogramaciónes.FocusedRowIndex, "SPPRE$RESOLUCION").ToString() != "")
                    {
                        archivo.Visible = true;
                        archivo.Text = gvReprogramaciónes.GetRowValues(gvReprogramaciónes.FocusedRowIndex, "SPPRE$RESOLUCION").ToString();
                    }
                    else
                        archivo.Visible = false;

                    txtResolución.Text = gvReprogramaciónes.GetRowValues(gvReprogramaciónes.FocusedRowIndex, "SPPRE$RESOLUCION").ToString();
                    Fecha_resolucion.Text = gvReprogramaciónes.GetRowValues(gvReprogramaciónes.FocusedRowIndex, "SPPRE$FECHA_RESOLUCION").ToString();
                    txtObservacion.Text = gvReprogramaciónes.GetRowValues(gvReprogramaciónes.FocusedRowIndex, "SPPRE$OBSERVACION").ToString();
                    mensaje = "Reprogramación aprobada, la misma no puede ser editada";
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, GetType(), "script", "Alerta('" + mensaje + " <br/>',2);", true);
                    popProgramacion.HeaderText = "Editar resolución";
                    btnGrabaRepro.Text = "Editar";
                    btnGrabaRepro.CssClass = "btn btn-success";
                    popProgramacion.ShowOnPageLoad = true;

                }
                else
                {
                    txtResolución.Enabled = true;
                    Fecha_resolucion.Enabled = true;
                    btnGrabaRepro.Enabled = true;
                    txtObservacion.Enabled = true;
                    if (gvReprogramaciónes.GetRowValues(gvReprogramaciónes.FocusedRowIndex, "SPPRE$ARCHIVO").ToString() != "")
                    {
                        archivo.Visible = true;
                        archivo.Text = "Archivo adjunto: "+gvReprogramaciónes.GetRowValues(gvReprogramaciónes.FocusedRowIndex, "SPPRE$ARCHIVO").ToString();
                    }
                    else
                        archivo.Visible = false;
                    txtResolución.Text = gvReprogramaciónes.GetRowValues(gvReprogramaciónes.FocusedRowIndex, "SPPRE$RESOLUCION").ToString();
                    Fecha_resolucion.Text = gvReprogramaciónes.GetRowValues(gvReprogramaciónes.FocusedRowIndex, "SPPRE$FECHA_RESOLUCION").ToString();
                    txtObservacion.Text = gvReprogramaciónes.GetRowValues(gvReprogramaciónes.FocusedRowIndex, "SPPRE$OBSERVACION").ToString();
                    popProgramacion.HeaderText = "Editar resolución";
                    btnGrabaRepro.Text = "Editar";
                    btnGrabaRepro.CssClass = "btn btn-success";
                    popProgramacion.ShowOnPageLoad = true;
                }
                    
            }
            else
            {
                mensaje = "Seleccione una fila con una reprogramación";
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, GetType(), "script", "Alerta('" + mensaje + " <br/>',2);", true);
            }
        }

        protected void btnBorrar_Click(object sender, EventArgs e)
        {
            Session["operacion"] = 2;
            if (gvReprogramaciónes.FocusedRowIndex != -1)
            {
                if (gvReprogramaciónes.GetRowValues(gvReprogramaciónes.FocusedRowIndex, "SPPRE$APROBADO").ToString() == "SI")
                {
                    txtResolución.Enabled = false;
                    Fecha_resolucion.Enabled = false;
                    btnGrabaRepro.Enabled = false;
                    txtObservacion.Enabled = false;
                    if (gvReprogramaciónes.GetRowValues(gvReprogramaciónes.FocusedRowIndex, "SPPRE$ARCHIVO").ToString() != "")
                    {
                        archivo.Visible = true;
                        archivo.Text = gvReprogramaciónes.GetRowValues(gvReprogramaciónes.FocusedRowIndex, "SPPRE$ARCHIVO").ToString();
                    }
                    else
                        archivo.Visible = false;

                    txtResolución.Text = gvReprogramaciónes.GetRowValues(gvReprogramaciónes.FocusedRowIndex, "SPPRE$RESOLUCION").ToString();
                    Fecha_resolucion.Text = gvReprogramaciónes.GetRowValues(gvReprogramaciónes.FocusedRowIndex, "SPPRE$FECHA_RESOLUCION").ToString();
                    txtObservacion.Text = gvReprogramaciónes.GetRowValues(gvReprogramaciónes.FocusedRowIndex, "SPPRE$OBSERVACION").ToString();
                    mensaje = "Reprogramación aprobada, no puede ser borrada";
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, GetType(), "script", "Alerta('" + mensaje + " <br/>',2);", true);
                    popProgramacion.HeaderText = "Reprogramación aprobada no puede ser borrada";
                    btnGrabaRepro.CssClass = "btn btn-danger";
                    btnGrabaRepro.Text = "Borrar";
                    popProgramacion.ShowOnPageLoad = true;
                }

                else
                {
                    txtResolución.Enabled = false;
                    Fecha_resolucion.Enabled = false;
                    btnGrabaRepro.Enabled = true;
                    txtObservacion.Enabled = false;
                    if (gvReprogramaciónes.GetRowValues(gvReprogramaciónes.FocusedRowIndex, "SPPRE$ARCHIVO").ToString() != "")
                    {
                        archivo.Visible = true;
                        archivo.Text = gvReprogramaciónes.GetRowValues(gvReprogramaciónes.FocusedRowIndex, "SPPRE$ARCHIVO").ToString();
                    }
                    else
                        archivo.Visible = false;

                    txtResolución.Text = gvReprogramaciónes.GetRowValues(gvReprogramaciónes.FocusedRowIndex, "SPPRE$RESOLUCION").ToString();
                    Fecha_resolucion.Text = gvReprogramaciónes.GetRowValues(gvReprogramaciónes.FocusedRowIndex, "SPPRE$FECHA_RESOLUCION").ToString();
                    txtObservacion.Text = gvReprogramaciónes.GetRowValues(gvReprogramaciónes.FocusedRowIndex, "SPPRE$OBSERVACION").ToString();                    
                    popProgramacion.HeaderText ="Borrar reprogramación, confirme el proceso";
                    btnGrabaRepro.CssClass = "btn btn-danger";
                    btnGrabaRepro.Text = "Borrar";
                    popProgramacion.ShowOnPageLoad = true;
                }

            }
            else
            {
                mensaje = "Seleccione una fila con una reprogramación";
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, GetType(), "script", "Alerta('" + mensaje + " <br/>',2);", true);
            }
        }

        protected void btnProd_Click(object sender, EventArgs e)
        {
            string aprobado;
            if (gvReprogramaciónes.FocusedRowIndex != -1)
            {
                Session["carga"] = 1;
                Session["poa"] = gvReprogramaciónes.GetRowValues(gvReprogramaciónes.FocusedRowIndex, "SPPRE$POA");
                Session["reprogramacion"] = gvReprogramaciónes.GetRowValues(gvReprogramaciónes.FocusedRowIndex, "SPPRE$ID_REPRO");
                aprobado = gvReprogramaciónes.GetRowValues(gvReprogramaciónes.FocusedRowIndex, "SPPRE$APROBADO").ToString();
                lbResolucion.Text = "No. De resolución: "+ gvReprogramaciónes.GetRowValues(gvReprogramaciónes.FocusedRowIndex, "SPPRE$RESOLUCION").ToString();
                lblFecha.Text = "Fecha de la resolución: "+ gvReprogramaciónes.GetRowValues(gvReprogramaciónes.FocusedRowIndex, "SPPRE$FECHA_RESOLUCION").ToString();
                if (aprobado == "SI")
                {
                    lblmgAp.Text = ". Esta reprogramación se encuentra aprobada, los botones de agregar/edición y borrado se encuentran inhabilitados";
                    btnReprogra.Enabled = false;
                    btnBorrarRepro.Enabled = false;

                }
                else
                {
                    lblmgAp.Text = "";
                    btnReprogra.Enabled = true;
                    btnBorrarRepro.Enabled = true;
                }

                if (aprobado == "SI" && (Session["ROL"].ToString() == "ADMIN" || Convert.ToInt32(Session["Insto"]) == 54000))
                {
                    btnFinancieraProd.Enabled = false;
                    btnBorrarReproFin.Enabled = false;

                }

                else
                {
                    btnFinancieraProd.Enabled = true;
                    btnBorrarReproFin.Enabled = true;
                }
                    cargaproductos(Convert.ToInt32(Session["poa"]), Convert.ToInt32(Session["reprogramacion"]),Convert.ToInt32(cbanio.Value));
                MultiView1.ActiveViewIndex = 1;

            }

            else
            {
                mensaje = "Seleccione una fila con una reprogramación";
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, GetType(), "script", "Alerta('" + mensaje + " <br/>',2);", true);
            }
        }

        protected void btnRegresar_Click(object sender, EventArgs e)
        {
            Session["carga"] = null;
            cargaReprogrogramaciones(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["poa"]));
            MultiView1.ActiveViewIndex = 0;
        }

        protected int poaEnviado(int pom, int poa)
        {
            int  enviado = -1;
            //sql = "SELECT CASE WHEN SPOA$ESTADO = 'A' THEN 1 ELSE 0 END AS ESTADO FROM SCHE$SIPLAN20.SP20$POA WHERE SPOA$ID_POA = " + poa + " AND SPOA$ID_POM = " + pom + " AND SPOA$RESTRICTIVA = 'N'";
            /*sql = @"SELECT
                                CASE WHEN SPOA$ESTADO = 'A' THEN 1 WHEN SPOA$ESTADO = 'D'  AND SYSDATE <= FECHA_CIERRE THEN 0  WHEN SPOA$ESTADO = 'D'  AND SYSDATE > FECHA_CIERRE  THEN 1  WHEN SPOA$ESTADO = 'D'  AND FECHA_CIERRE IS NULL THEN 1 END AS ESTADO
                                FROM
                                (SELECT
                                    POA.SPOA$ESTADO
                                    ,(SELECT SPFC$FECHA_CIERRE FROM SCHE$SIPLAN20.SP20$FECHAS_CIERRE WHERE SPFC$TIPO_FECHA = 2 AND SPFC$RESTRICTIVA = 'N' AND SPFC$PERIODO_POM = POM.SPPO$ID_PERIODO) FECHA_CIERRE
                                    FROM SCHE$SIPLAN20.SP20$POA POA
                                    INNER JOIN SCHE$SIPLAN20.SP20$POM POM ON POM.SPPO$ID_POM = POA.SPOA$ID_POM AND POM.SPPO$ID_INSTITUCION = POA.SPOA$ID_INSTITUCION AND POM.SPPO$RESTRICTIVA = 'N' AND POA.SPOA$RESTRICTIVA = 'N'
                                    WHERE POA.SPOA$ID_POA = " + poa + " AND POA.SPOA$ID_POM = " + pom;*/



            /*sql = @"SELECT 
                        CASE WHEN SPPO$ESTADO = 'A' THEN 1 
                        WHEN SPPO$ESTADO = 'D' AND CONTEO = 0 THEN 1 
                        WHEN SPPO$ESTADO = 'D' AND CONTEO > 0 AND TRUNC(SYSDATE) > TRUNC(FECHA_CIERRE) THEN 1
                        WHEN SPPO$ESTADO = 'D' AND CONTEO > 0 AND TRUNC(SYSDATE) <= TRUNC(FECHA_CIERRE) THEN 0
                        WHEN SPPO$ESTADO = 'D' AND CONTEO > 0 AND FECHA_CIERRE IS NULL THEN 0
                        END AS ESTADO
                        ,NVL(TO_CHAR(FECHA_CIERRE+1,'DD/MM/YYYY'),'FECHA NO DEFINIDA PARA ESTE PERIODO') FECHA
                        FROM
                        (SELECT 
                             POM.SPPO$ESTADO
                            ,(SELECT SPFC$FECHA_CIERRE FROM SCHE$SIPLAN20.SP20$FECHAS_CIERRE WHERE SPFC$TIPO_FECHA = 2 AND SPFC$RESTRICTIVA = 'N' AND SPFC$PERIODO_POM = POM.SPPO$ID_PERIODO AND SPFC$EJERCICIO = POA.SPOA$ANIO AND ) FECHA_CIERRE
                            ,(SELECT COUNT(SPFC$FECHA_CIERRE) CONTEO FROM SCHE$SIPLAN20.SP20$FECHAS_CIERRE WHERE SPFC$TIPO_FECHA = 2 AND SPFC$RESTRICTIVA = 'N' AND SPFC$PERIODO_POM = POM.SPPO$ID_PERIODO) CONTEO
                            FROM SCHE$SIPLAN20.SP20$POA POA
                            INNER JOIN SCHE$SIPLAN20.SP20$POM POM ON POM.SPPO$ID_POM = POA.SPOA$ID_POM AND POM.SPPO$ID_INSTITUCION = POA.SPOA$ID_INSTITUCION AND POM.SPPO$RESTRICTIVA = 'N' AND POA.SPOA$RESTRICTIVA = 'N'
                            WHERE POA.SPOA$ID_POA = " + poa + " AND POA.SPOA$ANIO = " + cbanio.Value + " AND POA.SPOA$ID_POM = " + pom + " AND POA.SPOA$ID_INSTITUCION = " + cbInstituiciones.Value + ")";
            */

            sql = @"SELECT
                            SPP$ID_PERIODO
                            ,SPP$INICIO
                            ,SPP$FINAL
                            ,SPPO$ID_POM
                            ,SPPO$ID_INSTITUCION
                            ,CASE WHEN SPPO$ESTADO = 'A' THEN 1
                                  WHEN SPPO$ESTADO = 'D' AND FECHA_PRORROGA IS NOT NULL AND TRUNC(SYSDATE) <= TRUNC(FECHA_PRORROGA) THEN 0
                                  WHEN  SPPO$ESTADO = 'D' AND SYSDATE <= FECHA_CIERRE then 0
                                  WHEN  SPPO$ESTADO = 'D' AND SYSDATE > FECHA_CIERRE THEN  1
                                  WHEN SPPO$ESTADO = 'D' AND FECHA_CIERRE IS NULL THEN  1
                             END AS ESTADO
                            ,NVL(TO_CHAR(FECHA_CIERRE+1,'DD/MM/YYYY'),'FECHA NO DEFINIDA PARA ESTE PERIODO') FECHA
                            ,SPP$ABIERTO 
                            ,SPP$ORDEN
                            FROM
                            (SELECT P.SPP$ID_PERIODO
                                    ,P.SPP$INICIO
                                    ,P.SPP$FINAL
                                    ,POM.SPPO$ID_POM
                                    ,POM.SPPO$ID_INSTITUCION                                  
                                   
                                    ,(SELECT MAX(SPFC$FECHA_CIERRE) FECHA_CIERRA FROM SCHE$SIPLAN20.SP20$FECHAS_CIERRE WHERE SPFC$TIPO_FECHA = 2 AND SPFC$RESTRICTIVA = 'N' AND SPFC$PERIODO_POM = POM.SPPO$ID_PERIODO AND SPFC$POM = POM.SPPO$ID_POM AND SPFC$INSTITUCION = POM.SPPO$ID_INSTITUCION) FECHA_PRORROGA
                                    ,(SELECT COUNT(SPFC$FECHA_CIERRE) CONTEO FROM SCHE$SIPLAN20.SP20$FECHAS_CIERRE WHERE SPFC$TIPO_FECHA = 2 AND SPFC$RESTRICTIVA = 'N' AND SPFC$PERIODO_POM = POM.SPPO$ID_PERIODO AND SPFC$POM = -1 AND SPFC$INSTITUCION = -1) CONTEO
                                    ,P.SPP$ABIERTO 
                                    ,POM.SPPO$ESTADO
                                    ,P.SPP$ORDEN      
                                    ,(SELECT SPFC$FECHA_CIERRE FROM SCHE$SIPLAN20.SP20$FECHAS_CIERRE WHERE SPFC$TIPO_FECHA = 2 AND SPFC$RESTRICTIVA = 'N' AND SPFC$PERIODO_POM = POM.SPPO$ID_PERIODO AND SPFC$EJERCICIO = POA.SPOA$ANIO AND SPFC$POM = -1 AND SPFC$INSTITUCION = -1) FECHA_CIERRE
                                    FROM SCHE$SIPLAN20.SP20$PERIODO P 
                                    INNER JOIN SCHE$SIPLAN20.SP20$POM POM ON P.SPP$ID_PERIODO = POM.SPPO$ID_PERIODO AND P.SPP$RESTRICTIVA = 'N' AND POM.SPPO$RESTRICTIVA = 'N' 
                                    INNER JOIN SCHE$SIPLAN20.SP20$POA POA ON POA.SPOA$ID_POM = POM.SPPO$ID_POM AND POM.SPPO$ID_INSTITUCION = POA.SPOA$ID_INSTITUCION AND POM.SPPO$RESTRICTIVA = 'N' AND POA.SPOA$RESTRICTIVA = 'N'
                                    WHERE POM.SPPO$ID_POM = " + pom + " AND POM.SPPO$ID_INSTITUCION = " + cbInstituiciones.Value + @" AND POA.SPOA$ID_POA = " + poa + " AND POA.SPOA$ANIO = " + cbanio.Value + ")";

            estado = dao.consulta(sql);
            if (estado == 1)
            {
                tabla = dao.tabla;
                enviado = Convert.ToInt32(tabla.Rows[0]["ESTADO"]);
                Session["FECHA_CIERRA"] = tabla.Rows[0]["FECHA"].ToString();
            }
            return enviado;
        
        }



        protected void btnmeta_Click(object sender, EventArgs e)
        {
            int producto = -1;
            int reprogramacion = -1;
            int programeta = -1;
            int reprograma = -1;

            if (Convert.ToInt32(Session["operaProd"]) == 0)
            {
                if (txtdiferencia.Text == "")
                {
                    mensaje = "La cantidad a operar es requerdida";
                    ScriptManager.RegisterStartupScript(this.UpdatePanel4, GetType(), "script", "Alerta('" + mensaje + " <br/>',2);", true);
                    txtdiferencia.Focus();
                }
                else if (operacion.SelectedIndex == -1)
                {
                    mensaje = "Seleccione el tipo de operación que se va realizar suma o resta";
                    ScriptManager.RegisterStartupScript(this.UpdatePanel4, GetType(), "script", "Alerta('" + mensaje + " <br/>',2);", true);
                    operacion.Focus();
                }
                else
                {
                    reprogramacion = Convert.ToInt32(Session["reprogramacion"]);
                    //reprogramacion = Convert.ToInt32(gvReprogramaciónes.GetRowValues(gvReprogramaciónes.FocusedRowIndex, "SPPRE$ID_REPRO"));
                    producto = Convert.ToInt32(gvProductos.GetRowValues(gvProductos.FocusedRowIndex, "SPPMFS$ID_PRODUCTO").ToString());
                    sql = "INSERT INTO SCHE$SIPLAN20.SP20$PROGRAMACION_MFPRODUCTO (SPPMFS$META, SPPMFS$ID_POA, SPPMFS$ANIO, SPPMFS$ID_PRODUCTO, SPPMFS$FECHA_INSERTA, SPPMF$TPROGRA, SPPMFS$VIGENTE) VALUES(" + Convert.ToDouble(txtNuevameta.Text) + ", " + Convert.ToInt32(Session["poa"]) + ", " + Convert.ToInt32(cbanio.Value) + ", " + producto + ", 'INSERT = '||TO_CHAR(SYSDATE,'DD/MM/YYYY HH24:MI')||' '||'" + Session["USUARIO"].ToString() + "', 1,1)";
                    estado = dao.comando(sql);
                    if (estado == 1)
                    {
                        sql = "SELECT MAX(SPPMFS$ID_PROGRAMACION_FISICA) AS IDMAX FROM SCHE$SIPLAN20.SP20$PROGRAMACION_MFPRODUCTO WHERE SPPMFS$ID_POA = " + Convert.ToInt32(Session["poa"]) + " AND SPPMFS$ANIO = " + cbanio.Value + " AND SPPMFS$ID_PRODUCTO = " + producto + " AND SPPMF$TPROGRA = 1";
                        estado = dao.consulta(sql);
                        if (estado == 1)
                        {
                            tabla = dao.tabla;
                            sql = "INSERT INTO SCHE$SIPLAN20.SP20$REPROGRAPRODUCTO (SPPRP$REPROGRAMACION, SPPRP$PROGRAPRODCUTO, SPPRP$OPERACION, SPPRP$CANTIDAD, SPPRP$FECHA_INSERT) VALUES (" + reprogramacion + ", " + Convert.ToInt32(tabla.Rows[0]["IDMAX"]) + ", " + operacion.SelectedIndex + ", " + Convert.ToDouble(txtdiferencia.Text) + ", 'INSERT = '||TO_CHAR(SYSDATE,'DD/MM/YYYY HH24:MI')||' '||'" + Session["USUARIO"].ToString() + "')";
                            estado = dao.comando(sql);
                            if (estado == 1)
                            {
                                mensaje = "Reprogramación guardada correctamente, la misma estará vigente hasta que se apruebe la reprogramación";
                                ScriptManager.RegisterStartupScript(this.UpdatePanel4, GetType(), "script", "Alerta('" + mensaje + " <br/>',1);", true);
                                cargaproductos(Convert.ToInt32(Session["poa"]), Convert.ToInt32(Session["reprogramacion"]), Convert.ToInt32(cbanio.Value));
                                popMFproducto.ShowOnPageLoad = false;
                            }
                            else
                            {
                                mensaje = dao.mensaje;
                                ScriptManager.RegisterStartupScript(this.UpdatePanel4, GetType(), "script", "Alerta('" + mensaje + " <br/>',2);", true);
                            }
                        }
                        else
                        {
                            mensaje = dao.mensaje;
                            ScriptManager.RegisterStartupScript(this.UpdatePanel4, GetType(), "script", "Alerta('" + mensaje + " <br/>',2);", true);
                        }
                    }
                    else
                    {
                        mensaje = dao.mensaje;
                        ScriptManager.RegisterStartupScript(this.UpdatePanel4, GetType(), "script", "Alerta('" + mensaje + " <br/>',2);", true);
                    }
                }
            }
            else if (Convert.ToInt32(Session["operaProd"]) == 1)
            {

                if (txtdiferencia.Text == "")
                {
                    mensaje = "La cantidad a operar es requerdida";
                    ScriptManager.RegisterStartupScript(this.UpdatePanel4, GetType(), "script", "Alerta('" + mensaje + " <br/>',2);", true);
                    txtdiferencia.Focus();
                }
                else if (operacion.SelectedIndex == -1)
                {
                    mensaje = "Seleccione el tipo de operación que se va realizar suma o resta";
                    ScriptManager.RegisterStartupScript(this.UpdatePanel4, GetType(), "script", "Alerta('" + mensaje + " <br/>',2);", true);
                    operacion.Focus();
                }
                else
                {
                    reprogramacion = Convert.ToInt32(Session["reprogramacion"]);
                    programeta = Convert.ToInt32(gvProductos.GetRowValues(gvProductos.FocusedRowIndex, "NIDMF").ToString());
                    reprograma  = Convert.ToInt32(gvProductos.GetRowValues(gvProductos.FocusedRowIndex, "IDREPROGRA").ToString());
                    producto = Convert.ToInt32(gvProductos.GetRowValues(gvProductos.FocusedRowIndex, "SPPMFS$ID_PRODUCTO").ToString());

                    sql = "UPDATE SCHE$SIPLAN20.SP20$PROGRAMACION_MFPRODUCTO SET SPPMFS$META = " + Convert.ToInt32(txtNuevameta.Text) + ", SPPMFS$FECHA_UPDATE = 'UPDATE = '||TO_CHAR(SYSDATE,'DD/MM/YYYY HH24:MI')||' '||'" + Session["USUARIO"].ToString() + "' WHERE  SPPMFS$ID_PROGRAMACION_FISICA = " + programeta + " AND  SPPMFS$ID_PRODUCTO = " + producto + " AND SPPMFS$ID_POA = " + Convert.ToInt32(Session["poa"]) + " AND  SPPMFS$ANIO = " + Convert.ToInt32(cbanio.Value);
                    estado = dao.comando(sql);
                    if (estado == 1)
                    {


                        sql = "UPDATE SCHE$SIPLAN20.SP20$REPROGRAPRODUCTO SET SPPRP$OPERACION = " + operacion.SelectedIndex + ", SPPRP$CANTIDAD =" + Convert.ToInt32(txtdiferencia.Text) + ", SPPRP$FECHA_UPDATE = 'UPDATE = '||TO_CHAR(SYSDATE,'DD/MM/YYYY HH24:MI')||' '||'" + Session["USUARIO"].ToString() + "'  WHERE SPPRP$ID_REPROGRA =  " + reprograma + "  AND  SPPRP$REPROGRAMACION = " + reprogramacion + " AND SPPRP$PROGRAPRODCUTO =  " + programeta;
                            estado = dao.comando(sql);
                            if (estado == 1)
                            {
                                mensaje = "Reprogramación editada correctamente, la misma estará vigente hasta que se apruebe la reprogramación";
                                ScriptManager.RegisterStartupScript(this.UpdatePanel4, GetType(), "script", "Alerta('" + mensaje + " <br/>',1);", true);
                                cargaproductos(Convert.ToInt32(Session["poa"]), Convert.ToInt32(Session["reprogramacion"]), Convert.ToInt32(cbanio.Value));
                                popMFproducto.ShowOnPageLoad = false;
                            }
                            else
                            {
                                mensaje = dao.mensaje;
                                ScriptManager.RegisterStartupScript(this.UpdatePanel4, GetType(), "script", "Alerta('" + mensaje + " <br/>',2);", true);
                            }
                        
                        
                    }
                    else
                    {
                        mensaje = dao.mensaje;
                        ScriptManager.RegisterStartupScript(this.UpdatePanel4, GetType(), "script", "Alerta('" + mensaje + " <br/>',2);", true);
                    }
                }
            }

            else if (Convert.ToInt32(Session["operaProd"]) == 2)
            {
                              
                    reprogramacion = Convert.ToInt32(Session["reprogramacion"]);
                    programeta = Convert.ToInt32(gvProductos.GetRowValues(gvProductos.FocusedRowIndex, "NIDMF").ToString());
                    reprograma = Convert.ToInt32(gvProductos.GetRowValues(gvProductos.FocusedRowIndex, "IDREPROGRA").ToString());
                    producto = Convert.ToInt32(gvProductos.GetRowValues(gvProductos.FocusedRowIndex, "SPPMFS$ID_PRODUCTO").ToString());

                    sql = "UPDATE SCHE$SIPLAN20.SP20$PROGRAMACION_MFPRODUCTO SET SPPMFS$RESTRICTIVA = 'S', SPPMFS$FECHA_DELETE = 'DELETE = '||TO_CHAR(SYSDATE,'DD/MM/YYYY HH24:MI')||' '||'" + Session["USUARIO"].ToString() + "' WHERE  SPPMFS$ID_PROGRAMACION_FISICA = " + programeta + " AND  SPPMFS$ID_PRODUCTO = " + producto + " AND SPPMFS$ID_POA = " + Convert.ToInt32(Session["poa"]) + " AND  SPPMFS$ANIO = " + Convert.ToInt32(cbanio.Value);
                    estado = dao.comando(sql);
                    if (estado == 1)
                    {


                        sql = "UPDATE SCHE$SIPLAN20.SP20$REPROGRAPRODUCTO SET SPPRP$RESTRICTIVA = 'S',  SPPRP$FECHA_DELETE = 'DELETE = '||TO_CHAR(SYSDATE,'DD/MM/YYYY HH24:MI')||' '||'" + Session["USUARIO"].ToString() + "'  WHERE SPPRP$ID_REPROGRA =  " + reprograma + "  AND  SPPRP$REPROGRAMACION = " + reprogramacion + " AND SPPRP$PROGRAPRODCUTO =  " + programeta;
                        estado = dao.comando(sql);
                        if (estado == 1)
                        {
                            mensaje = "Reprogramación borrada correctamente";
                            ScriptManager.RegisterStartupScript(this.UpdatePanel4, GetType(), "script", "Alerta('" + mensaje + " <br/>',1);", true);
                            cargaproductos(Convert.ToInt32(Session["poa"]), Convert.ToInt32(Session["reprogramacion"]), Convert.ToInt32(cbanio.Value));
                            popMFproducto.ShowOnPageLoad = false;
                        }
                        else
                        {
                            mensaje = dao.mensaje;
                            ScriptManager.RegisterStartupScript(this.UpdatePanel4, GetType(), "script", "Alerta('" + mensaje + " <br/>',2);", true);
                        }


                    }
                    else
                    {
                        mensaje = dao.mensaje;
                        ScriptManager.RegisterStartupScript(this.UpdatePanel4, GetType(), "script", "Alerta('" + mensaje + " <br/>',2);", true);
                    }
                
            }

            if (Convert.ToInt32(Session["operaProd"]) == 10)
            {
                if (txtdiferencia.Text == "")
                {
                    mensaje = "La cantidad a operar es requerdida";
                    ScriptManager.RegisterStartupScript(this.UpdatePanel4, GetType(), "script", "Alerta('" + mensaje + " <br/>',2);", true);
                    txtdiferencia.Focus();
                }
                else if (operacion.SelectedIndex == -1)
                {
                    mensaje = "Seleccione el tipo de operación que se va realizar suma o resta";
                    ScriptManager.RegisterStartupScript(this.UpdatePanel4, GetType(), "script", "Alerta('" + mensaje + " <br/>',2);", true);
                    operacion.Focus();
                }
                else
                {
                    reprogramacion = Convert.ToInt32(Session["reprogramacion"]);
                    //reprogramacion = Convert.ToInt32(gvReprogramaciónes.GetRowValues(gvReprogramaciónes.FocusedRowIndex, "SPPRE$ID_REPRO"));
                    producto = Convert.ToInt32(gvProductos.GetRowValues(gvProductos.FocusedRowIndex, "SPPMFS$ID_PRODUCTO").ToString());
                    sql = "INSERT INTO SCHE$SIPLAN20.SP20$PROGRAMACION_MFPRODUCTO (SPPMFS$META, SPPMFS$ID_POA, SPPMFS$ANIO, SPPMFS$ID_PRODUCTO, SPPMFS$FECHA_INSERTA, SPPMF$TPROGRA, SPPMFS$VIGENTE, SPPMF$TIPO) VALUES(" + Convert.ToDouble(txtNuevameta.Text) + ", " + Convert.ToInt32(Session["poa"]) + ", " + Convert.ToInt32(cbanio.Value) + ", " + producto + ", 'INSERT = '||TO_CHAR(SYSDATE,'DD/MM/YYYY HH24:MI')||' '||'" + Session["USUARIO"].ToString() + "', 1,1,1)";
                    estado = dao.comando(sql);
                    if (estado == 1)
                    {
                        sql = "SELECT MAX(SPPMFS$ID_PROGRAMACION_FISICA) AS IDMAX FROM SCHE$SIPLAN20.SP20$PROGRAMACION_MFPRODUCTO WHERE SPPMFS$ID_POA = " + Convert.ToInt32(Session["poa"]) + " AND SPPMFS$ANIO = " + cbanio.Value + " AND SPPMFS$ID_PRODUCTO = " + producto + " AND SPPMF$TPROGRA = 1";
                        estado = dao.consulta(sql);
                        if (estado == 1)
                        {
                            tabla = dao.tabla;
                            sql = "INSERT INTO SCHE$SIPLAN20.SP20$REPROGRAPRODUCTO (SPPRP$REPROGRAMACION, SPPRP$PROGRAPRODCUTO, SPPRP$OPERACION, SPPRP$CANTIDAD, SPPRP$FECHA_INSERT,SPPRP$TIPO) VALUES (" + reprogramacion + ", " + Convert.ToInt32(tabla.Rows[0]["IDMAX"]) + ", " + operacion.SelectedIndex + ", " + Convert.ToDouble(txtdiferencia.Text) + ", 'INSERT = '||TO_CHAR(SYSDATE,'DD/MM/YYYY HH24:MI')||' '||'" + Session["USUARIO"].ToString() + "',1)";
                            estado = dao.comando(sql);
                            if (estado == 1)
                            {
                                mensaje = "Reprogramación guardada correctamente, la misma estará vigente hasta que se apruebe la reprogramación";
                                ScriptManager.RegisterStartupScript(this.UpdatePanel4, GetType(), "script", "Alerta('" + mensaje + " <br/>',1);", true);
                                cargaproductos(Convert.ToInt32(Session["poa"]), Convert.ToInt32(Session["reprogramacion"]), Convert.ToInt32(cbanio.Value));
                                popMFproducto.ShowOnPageLoad = false;
                            }
                            else
                            {
                                mensaje = dao.mensaje;
                                ScriptManager.RegisterStartupScript(this.UpdatePanel4, GetType(), "script", "Alerta('" + mensaje + " <br/>',2);", true);
                            }
                        }
                        else
                        {
                            mensaje = dao.mensaje;
                            ScriptManager.RegisterStartupScript(this.UpdatePanel4, GetType(), "script", "Alerta('" + mensaje + " <br/>',2);", true);
                        }
                    }
                    else
                    {
                        mensaje = dao.mensaje;
                        ScriptManager.RegisterStartupScript(this.UpdatePanel4, GetType(), "script", "Alerta('" + mensaje + " <br/>',2);", true);
                    }
                }
            }


            else if (Convert.ToInt32(Session["operaProd"]) == 11)
            {

                if (txtdiferencia.Text == "")
                {
                    mensaje = "La cantidad a operar es requerdida";
                    ScriptManager.RegisterStartupScript(this.UpdatePanel4, GetType(), "script", "Alerta('" + mensaje + " <br/>',2);", true);
                    txtdiferencia.Focus();
                }
                else if (operacion.SelectedIndex == -1)
                {
                    mensaje = "Seleccione el tipo de operación que se va realizar suma o resta";
                    ScriptManager.RegisterStartupScript(this.UpdatePanel4, GetType(), "script", "Alerta('" + mensaje + " <br/>',2);", true);
                    operacion.Focus();
                }
                else
                {
                    reprogramacion = Convert.ToInt32(Session["reprogramacion"]);
                    programeta = Convert.ToInt32(gvProductos.GetRowValues(gvProductos.FocusedRowIndex, "NIDMFIN").ToString());
                    reprograma = Convert.ToInt32(gvProductos.GetRowValues(gvProductos.FocusedRowIndex, "REPROFINANCIERA").ToString());
                    producto = Convert.ToInt32(gvProductos.GetRowValues(gvProductos.FocusedRowIndex, "SPPMFS$ID_PRODUCTO").ToString());

                    sql = "UPDATE SCHE$SIPLAN20.SP20$PROGRAMACION_MFPRODUCTO SET SPPMFS$META = " + Convert.ToDouble(txtNuevameta.Text) + ", SPPMFS$FECHA_UPDATE = 'UPDATE = '||TO_CHAR(SYSDATE,'DD/MM/YYYY HH24:MI')||' '||'" + Session["USUARIO"].ToString() + "' WHERE  SPPMFS$ID_PROGRAMACION_FISICA = " + programeta + " AND  SPPMFS$ID_PRODUCTO = " + producto + " AND SPPMFS$ID_POA = " + Convert.ToInt32(Session["poa"]) + " AND  SPPMFS$ANIO = " + Convert.ToInt32(cbanio.Value)+ " AND SPPMF$TIPO = 1";
                    estado = dao.comando(sql);
                    if (estado == 1)
                    {


                        sql = "UPDATE SCHE$SIPLAN20.SP20$REPROGRAPRODUCTO SET SPPRP$OPERACION = " + operacion.SelectedIndex + ", SPPRP$CANTIDAD =" + Convert.ToDouble(txtdiferencia.Text) + ", SPPRP$FECHA_UPDATE = 'UPDATE = '||TO_CHAR(SYSDATE,'DD/MM/YYYY HH24:MI')||' '||'" + Session["USUARIO"].ToString() + "'  WHERE SPPRP$ID_REPROGRA =  " + reprograma + "  AND  SPPRP$REPROGRAMACION = " + reprogramacion + " AND SPPRP$PROGRAPRODCUTO =  " + programeta+ " AND SPPRP$TIPO = 1";
                        estado = dao.comando(sql);
                        if (estado == 1)
                        {
                            mensaje = "Reprogramación editada correctamente, la misma estará vigente hasta que se apruebe la reprogramación";
                            ScriptManager.RegisterStartupScript(this.UpdatePanel4, GetType(), "script", "Alerta('" + mensaje + " <br/>',1);", true);
                            cargaproductos(Convert.ToInt32(Session["poa"]), Convert.ToInt32(Session["reprogramacion"]), Convert.ToInt32(cbanio.Value));
                            popMFproducto.ShowOnPageLoad = false;
                        }
                        else
                        {
                            mensaje = dao.mensaje;
                            ScriptManager.RegisterStartupScript(this.UpdatePanel4, GetType(), "script", "Alerta('" + mensaje + " <br/>',2);", true);
                        }


                    }
                    else
                    {
                        mensaje = dao.mensaje;
                        ScriptManager.RegisterStartupScript(this.UpdatePanel4, GetType(), "script", "Alerta('" + mensaje + " <br/>',2);", true);
                    }
                }
            }

            else if (Convert.ToInt32(Session["operaProd"]) == 12)
            {

                reprogramacion = Convert.ToInt32(Session["reprogramacion"]);
                programeta = Convert.ToInt32(gvProductos.GetRowValues(gvProductos.FocusedRowIndex, "NIDMFIN").ToString());
                reprograma = Convert.ToInt32(gvProductos.GetRowValues(gvProductos.FocusedRowIndex, "REPROFINANCIERA").ToString());
                producto = Convert.ToInt32(gvProductos.GetRowValues(gvProductos.FocusedRowIndex, "SPPMFS$ID_PRODUCTO").ToString());

                sql = "UPDATE SCHE$SIPLAN20.SP20$PROGRAMACION_MFPRODUCTO SET SPPMFS$RESTRICTIVA = 'S', SPPMFS$FECHA_DELETE = 'DELETE = '||TO_CHAR(SYSDATE,'DD/MM/YYYY HH24:MI')||' '||'" + Session["USUARIO"].ToString() + "' WHERE  SPPMFS$ID_PROGRAMACION_FISICA = " + programeta + " AND  SPPMFS$ID_PRODUCTO = " + producto + " AND SPPMFS$ID_POA = " + Convert.ToInt32(Session["poa"]) + " AND  SPPMFS$ANIO = " + Convert.ToInt32(cbanio.Value)+ " AND SPPMF$TIPO = 1";
                estado = dao.comando(sql);
                if (estado == 1)
                {


                    sql = "UPDATE SCHE$SIPLAN20.SP20$REPROGRAPRODUCTO SET SPPRP$RESTRICTIVA = 'S',  SPPRP$FECHA_DELETE = 'DELETE = '||TO_CHAR(SYSDATE,'DD/MM/YYYY HH24:MI')||' '||'" + Session["USUARIO"].ToString() + "'  WHERE SPPRP$ID_REPROGRA =  " + reprograma + "  AND  SPPRP$REPROGRAMACION = " + reprogramacion + " AND SPPRP$PROGRAPRODCUTO =  " + programeta+ " AND SPPRP$TIPO = 1";
                    estado = dao.comando(sql);
                    if (estado == 1)
                    {
                        mensaje = "Reprogramación borrada correctamente";
                        ScriptManager.RegisterStartupScript(this.UpdatePanel4, GetType(), "script", "Alerta('" + mensaje + " <br/>',1);", true);
                        cargaproductos(Convert.ToInt32(Session["poa"]), Convert.ToInt32(Session["reprogramacion"]), Convert.ToInt32(cbanio.Value));
                        popMFproducto.ShowOnPageLoad = false;
                    }
                    else
                    {
                        mensaje = dao.mensaje;
                        ScriptManager.RegisterStartupScript(this.UpdatePanel4, GetType(), "script", "Alerta('" + mensaje + " <br/>',2);", true);
                    }


                }
                else
                {
                    mensaje = dao.mensaje;
                    ScriptManager.RegisterStartupScript(this.UpdatePanel4, GetType(), "script", "Alerta('" + mensaje + " <br/>',2);", true);
                }

            }

        }

        protected void btnCerrarMeta_Click(object sender, EventArgs e)
        {
            popMFproducto.ShowOnPageLoad = false;
        }

        protected void resultados_SelectedIndexChanged(object sender, EventArgs e)
        {

            Double nueva_meta, meta_vigente, diferencia;

            if (txtdiferencia.Text == "")
            {
                mensaje = "Ingrese la cantidad a restar o sumar";
                txtdiferencia.Focus();
                operacion.SelectedIndex = -1;
                ScriptManager.RegisterStartupScript(this.UpdatePanel4, GetType(), "script", "Alerta('" + mensaje + " <br/>',2);", true);
            }
            else
            {
                if (mVigente.Text == "")
                    meta_vigente = 0;
                else
                    meta_vigente = Convert.ToDouble(mVigente.Text);
                diferencia = Convert.ToDouble(txtdiferencia.Text);
                if (operacion.SelectedIndex == 0)
                    nueva_meta = meta_vigente + diferencia;
                else
                    nueva_meta = meta_vigente - diferencia;

                txtNuevameta.Text = Convert.ToString(nueva_meta);
            }
            


        }

        protected void btnReprogra_Click(object sender, EventArgs e)
        {
            int nivel;
            if (gvProductos.FocusedRowIndex != -1)
            {
                nivel = gvProductos.GetRowLevel(gvProductos.FocusedRowIndex);
                if (nivel == 2)
                {
                    txtdiferencia.Text = "";
                    operacion.SelectedIndex = -1;
                    txtNuevameta.Text = "";
                    txtdiferencia.Enabled = true;
                    operacion.Enabled = true;

                    lblProducto.Text = gvProductos.GetRowValues(gvProductos.FocusedRowIndex, "SPPRO$DESCRIPCION").ToString();
                    lblMedidaProd.Text = gvProductos.GetRowValues(gvProductos.FocusedRowIndex, "SNCGUM$NOMBRE").ToString();
                    mInicial.Text = gvProductos.GetRowValues(gvProductos.FocusedRowIndex, "METAFISICAINICIAL").ToString();
                    mVigente.Text = gvProductos.GetRowValues(gvProductos.FocusedRowIndex, "METAVIGENTE").ToString();
                    if (gvProductos.GetRowValues(gvProductos.FocusedRowIndex, "CANTIDAD") != DBNull.Value)
                    {
                        txtdiferencia.Text = gvProductos.GetRowValues(gvProductos.FocusedRowIndex, "CANTIDAD").ToString();
                    }

                    //if (gvProductos.GetRowValues(gvProductos.FocusedRowIndex, "OPERACION") != DBNull.Value)
                    //{
                    //    operacion.SelectedIndex  = Convert.ToInt32(gvProductos.GetRowValues(gvProductos.FocusedRowIndex, "OPERACION").ToString());
                    //}

                    if (gvProductos.GetRowValues(gvProductos.FocusedRowIndex, "REPROFISICA") != DBNull.Value)
                    {
                        txtNuevameta.Text = gvProductos.GetRowValues(gvProductos.FocusedRowIndex, "REPROFISICA").ToString();
                    }
                    if (gvProductos.GetRowValues(gvProductos.FocusedRowIndex, "NIDMF") == DBNull.Value && gvProductos.GetRowValues(gvProductos.FocusedRowIndex, "IDREPROGRA") == DBNull.Value)
                    {
                        Session["operaProd"] = 0;
                        popMFproducto.HeaderText = "Nueva programación de meta física de producto";
                        btnmeta.Text = "Grabar nueva meta";
                        btnmeta.CssClass = "btn btn-primary";
                    }
                    else
                    {
                        Session["operaProd"] = 1;
                        popMFproducto.HeaderText = "Editar programación de meta física de producto";
                        btnmeta.Text = "Editar reprogramación";
                        btnmeta.CssClass = "btn btn-success";
                    }

                   
                    popMFproducto.ShowOnPageLoad = true;
                }
                else
                {
                    mensaje = "Seleccione una fila con un producto";
                    ScriptManager.RegisterStartupScript(this.UpdatePanel2, GetType(), "script", "Alerta('" + mensaje + " <br/>',2);", true);
                }
            }

            else
            {
                mensaje = "Seleccione una fila con un producto";
                ScriptManager.RegisterStartupScript(this.UpdatePanel2, GetType(), "script", "Alerta('" + mensaje + " <br/>',2);", true);
            }
        }

        protected void btnBorrarRepro_Click(object sender, EventArgs e)
        {
            int nivel;
            if (gvProductos.FocusedRowIndex != -1)
            {
                nivel = gvProductos.GetRowLevel(gvProductos.FocusedRowIndex);
                if (nivel == 2)
                {
                    txtdiferencia.Text = "";
                    operacion.SelectedIndex = -1;
                    txtNuevameta.Text = "";


                    lblProducto.Text = gvProductos.GetRowValues(gvProductos.FocusedRowIndex, "SPPRO$DESCRIPCION").ToString();
                    lblMedidaProd.Text = gvProductos.GetRowValues(gvProductos.FocusedRowIndex, "SNCGUM$NOMBRE").ToString();
                    mInicial.Text = gvProductos.GetRowValues(gvProductos.FocusedRowIndex, "METAFISICAINICIAL").ToString();
                    mVigente.Text = gvProductos.GetRowValues(gvProductos.FocusedRowIndex, "METAVIGENTE").ToString();
                    if (gvProductos.GetRowValues(gvProductos.FocusedRowIndex, "CANTIDAD") != DBNull.Value)
                    {
                        txtdiferencia.Text = gvProductos.GetRowValues(gvProductos.FocusedRowIndex, "CANTIDAD").ToString();
                    }

                    if (gvProductos.GetRowValues(gvProductos.FocusedRowIndex, "OPERACION") != DBNull.Value)
                    {
                        operacion.SelectedIndex = Convert.ToInt32(gvProductos.GetRowValues(gvProductos.FocusedRowIndex, "OPERACION").ToString());
                    }

                    if (gvProductos.GetRowValues(gvProductos.FocusedRowIndex, "REPROFISICA") != DBNull.Value)
                    {
                        txtNuevameta.Text = gvProductos.GetRowValues(gvProductos.FocusedRowIndex, "REPROFISICA").ToString();
                    }
                    if (gvProductos.GetRowValues(gvProductos.FocusedRowIndex, "NIDMF") == DBNull.Value && gvProductos.GetRowValues(gvProductos.FocusedRowIndex, "IDREPROGRA") == DBNull.Value)
                    {
                        Session["operaProd"] = 0;
                        popMFproducto.HeaderText = "Nueva programación de meta física de producto";
                        btnmeta.Text = "Grabar nueva meta";
                        btnmeta.CssClass = "btn btn-primary";
                    }
                    else
                    {
                        Session["operaProd"] = 2;
                        
                        txtdiferencia.Enabled = false;
                        operacion.Enabled = false;
                        popMFproducto.HeaderText = "Borrar programación de meta física de producto";
                        btnmeta.Text = "Borrar reprogramación";
                        btnmeta.CssClass = "btn btn-danger";
                    }


                    popMFproducto.ShowOnPageLoad = true;
                }
                else
                {
                    mensaje = "Seleccione una fila con un producto";
                    ScriptManager.RegisterStartupScript(this.UpdatePanel2, GetType(), "script", "Alerta('" + mensaje + " <br/>',2);", true);
                }
            }

            else
            {
                mensaje = "Seleccione una fila con un producto";
                ScriptManager.RegisterStartupScript(this.UpdatePanel2, GetType(), "script", "Alerta('" + mensaje + " <br/>',2);", true);
            }
        }

        protected void btnAprobar_Click(object sender, EventArgs e)
        {
            int reprogramacion = -1;
            DataTable productos = new DataTable();
            DataTable finproductos = new DataTable();
            DataTable subproductos = new DataTable();
            string aprobado="";
            if (gvReprogramaciónes.FocusedRowIndex != -1)
            {
                aprobado = gvReprogramaciónes.GetRowValues(gvReprogramaciónes.FocusedRowIndex, "SPPRE$APROBADO").ToString();
                if (aprobado == "SI")
                {
                    mensaje = "Esta reprogramación se encuentra aprobada";
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, GetType(), "script", "Alerta('" + mensaje + " <br/>',1);", true);
                    }

                else if (gvReprogramaciónes.GetRowValues(gvReprogramaciónes.FocusedRowIndex, "SPPRE$ARCHIVO") != DBNull.Value)
                {
                    reprogramacion = Convert.ToInt32(gvReprogramaciónes.GetRowValues(gvReprogramaciónes.FocusedRowIndex, "SPPRE$ID_REPRO").ToString());
                    sql = "SELECT * FROM SCHE$SIPLAN20.SP20$REPROGRAPRODUCTO WHERE SPPRP$REPROGRAMACION = " + reprogramacion+ " AND SPPRP$RESTRICTIVA = 'N' AND SPPRP$TIPO = 0";
                    estado = dao.consulta(sql);
                    if (estado == 1)
                        productos = dao.tabla;
                    sql = "SELECT * FROM SCHE$SIPLAN20.SP20$REPROGRASUB WHERE SPPRS$REPROGRAMACION = " + reprogramacion+ " AND SPPRS$RESTRICTIVA = 'N'";
                    estado = dao.consulta(sql);
                    if (estado == 1)
                        subproductos = dao.tabla;

                    sql = "SELECT * FROM SCHE$SIPLAN20.SP20$REPROGRAPRODUCTO WHERE SPPRP$REPROGRAMACION = " + reprogramacion + " AND SPPRP$RESTRICTIVA = 'N' AND SPPRP$TIPO = 1";
                    estado = dao.consulta(sql);
                    if (estado == 1)
                        finproductos = dao.tabla;

                    if (subproductos.Rows.Count <= 0 && productos.Rows.Count <= 0 && finproductos.Rows.Count <= 0)
                    {
                        mensaje = "Debe de reprogramar las metas productos o subprodutos, para aprobar esta reprogramación";
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, GetType(), "script", "Alerta('" + mensaje + " <br/>',2);", true);

                    }

                    else
                    {
                        if (productos.Rows.Count > 0)
                        {
                            sql = "SELECT  * FROM SCHE$SIPLAN20.SP20$PROGRAMACION_MFPRODUCTO WHERE SPPMFS$ID_PROGRAMACION_FISICA IN (SELECT SPPRP$PROGRAPRODCUTO FROM SCHE$SIPLAN20.SP20$REPROGRAPRODUCTO WHERE SPPRP$REPROGRAMACION = " + reprogramacion + " AND SPPRP$RESTRICTIVA = 'N' AND SPPRP$TIPO = 0)";
                            estado = dao.consulta(sql);
                            if (estado == 1)
                            {
                                tabla = dao.tabla;
                                for (int i = 0; i < tabla.Rows.Count; i++)
                                {
                                    sql = "UPDATE SCHE$SIPLAN20.SP20$PROGRAMACION_MFPRODUCTO SET SPPMFS$VIGENTE = 1 WHERE SPPMFS$ID_PRODUCTO = " + tabla.Rows[i]["SPPMFS$ID_PRODUCTO"] + " AND SPPMFS$ID_POA = " + Convert.ToInt32(Session["poa"]) + " AND SPPMFS$ANIO = "+Convert.ToInt32(cbanio.Value)+ " AND SPPMFS$RESTRICTIVA = 'N' AND SPPMF$TIPO = 0";
                                    estado = dao.comando(sql);
                                    if(estado == 0)
                                        break;
                                }

                                if (estado == 1)
                                {
                                    for (int i = 0; i < productos.Rows.Count; i++)
                                    {
                                        sql = "UPDATE SCHE$SIPLAN20.SP20$PROGRAMACION_MFPRODUCTO SET SPPMFS$VIGENTE = 0 WHERE SPPMFS$ID_PROGRAMACION_FISICA = " + productos.Rows[i]["SPPRP$PROGRAPRODCUTO"] + " AND SPPMFS$ID_POA = " + Convert.ToInt32(Session["poa"]) + " AND SPPMFS$ANIO = " + Convert.ToInt32(cbanio.Value) + " AND SPPMFS$RESTRICTIVA = 'N' AND SPPMF$TIPO = 0 ";
                                        estado = dao.comando(sql);
                                        if (estado == 0)
                                            break;
                                    }
                                }
                            }
                        
                        }

                        if (subproductos.Rows.Count > 0)
                        {
                            sql = "SELECT  * FROM SCHE$SIPLAN20.SP20$PROGRAMETA_FI_FIN_SUB WHERE SPPMFS$ID_PROGRAMACION_FISFIN IN (SELECT SPPRS$PROGRASUB FROM SCHE$SIPLAN20.SP20$REPROGRASUB WHERE SPPRS$REPROGRAMACION = " + reprogramacion + " AND SPPRS$RESTRICTIVA = 'N')";
                            estado = dao.consulta(sql);
                            if (estado == 1)
                            {
                                tabla = dao.tabla;
                                for (int i = 0; i < tabla.Rows.Count; i++)
                                {
                                    sql = "UPDATE SCHE$SIPLAN20.SP20$PROGRAMETA_FI_FIN_SUB SET SPPMFS$VIGENTE = 1 WHERE SPPMFS$ID_SUBPRODUCTO= " + tabla.Rows[i]["SPPMFS$ID_SUBPRODUCTO"] + " AND SPPMFS$ID_POA = " + Convert.ToInt32(Session["poa"]) + " AND SPPMFS$ANIO = " + Convert.ToInt32(cbanio.Value) + " AND SPPMFS$RESTRICTIVA = 'N' AND SPPMFS$TIPO_PROGRAMACION = " + tabla.Rows[i]["SPPMFS$TIPO_PROGRAMACION"];
                                    estado = dao.comando(sql);
                                    if (estado == 0)
                                        break;
                                }

                                if (estado == 1)
                                {
                                    for (int i = 0; i < subproductos.Rows.Count; i++)
                                    {
                                        sql = "UPDATE SCHE$SIPLAN20.SP20$PROGRAMETA_FI_FIN_SUB SET SPPMFS$VIGENTE = 0 WHERE SPPMFS$ID_PROGRAMACION_FISFIN = " + subproductos.Rows[i]["SPPRS$PROGRASUB"] + " AND SPPMFS$ID_POA = " + Convert.ToInt32(Session["poa"]) + " AND SPPMFS$ANIO = " + Convert.ToInt32(cbanio.Value) + " AND SPPMFS$RESTRICTIVA = 'N' AND SPPMFS$TIPO_PROGRAMACION = "+subproductos.Rows[i]["SPPRS$TIPO"];
                                        estado = dao.comando(sql);
                                        if (estado == 0)
                                            break;
                                    }
                                }
                            }

                        }


                        if (finproductos.Rows.Count > 0)
                        {
                            sql = "SELECT  * FROM SCHE$SIPLAN20.SP20$PROGRAMACION_MFPRODUCTO WHERE SPPMFS$ID_PROGRAMACION_FISICA IN (SELECT SPPRP$PROGRAPRODCUTO FROM SCHE$SIPLAN20.SP20$REPROGRAPRODUCTO WHERE SPPRP$REPROGRAMACION = " + reprogramacion + " AND SPPRP$RESTRICTIVA = 'N' AND SPPRP$TIPO = 1)";
                            estado = dao.consulta(sql);
                            if (estado == 1)
                            {
                                tabla = dao.tabla;
                                for (int i = 0; i < tabla.Rows.Count; i++)
                                {
                                    sql = "UPDATE SCHE$SIPLAN20.SP20$PROGRAMACION_MFPRODUCTO SET SPPMFS$VIGENTE = 1 WHERE SPPMFS$ID_PRODUCTO = " + tabla.Rows[i]["SPPMFS$ID_PRODUCTO"] + " AND SPPMFS$ID_POA = " + Convert.ToInt32(Session["poa"]) + " AND SPPMFS$ANIO = " + Convert.ToInt32(cbanio.Value) + " AND SPPMFS$RESTRICTIVA = 'N' AND SPPMF$TIPO = 1";
                                    estado = dao.comando(sql);
                                    if (estado == 0)
                                        break;
                                }

                                if (estado == 1)
                                {
                                    for (int i = 0; i < finproductos.Rows.Count; i++)
                                    {
                                        sql = "UPDATE SCHE$SIPLAN20.SP20$PROGRAMACION_MFPRODUCTO SET SPPMFS$VIGENTE = 0 WHERE SPPMFS$ID_PROGRAMACION_FISICA = " + finproductos.Rows[i]["SPPRP$PROGRAPRODCUTO"] + " AND SPPMFS$ID_POA = " + Convert.ToInt32(Session["poa"]) + " AND SPPMFS$ANIO = " + Convert.ToInt32(cbanio.Value) + " AND SPPMFS$RESTRICTIVA = 'N' AND SPPMF$TIPO = 1";
                                        estado = dao.comando(sql);
                                        if (estado == 0)
                                            break;
                                    }
                                }
                            }

                        }

                        if (estado == 1)
                        {

                            sql = "UPDATE SCHE$SIPLAN20.SP20$REPROGRAMACION SET SPPRE$APROBADO = 1, SPPRE$FECHA_APRUEBA = TO_CHAR(SYSDATE,'DD/MM/YYYY HH24:MI'), SPPRE$USUARIOAPRUEBA = '" + Session["USUARIO"].ToString() + "' WHERE SPPRE$ID_REPRO = " + reprogramacion;
                            estado = dao.comando(sql);
                            if (estado == 1)
                            {
                                mensaje = "Reprogramación aprobada, correctamente";
                                ScriptManager.RegisterStartupScript(this.UpdatePanel1, GetType(), "script", "Alerta('" + mensaje + " <br/>',1);", true);
                                cargaReprogrogramaciones(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["poa"]));
                            }
                            else
                            {
                                mensaje = dao.mensaje;
                                ScriptManager.RegisterStartupScript(this.UpdatePanel1, GetType(), "script", "Alerta('" + mensaje + " <br/>',2);", true);
                            }
                        }
                        else
                        {
                            mensaje = dao.mensaje;
                            ScriptManager.RegisterStartupScript(this.UpdatePanel1, GetType(), "script", "Alerta('" + mensaje + " <br/>',2);", true);
                        }


                    }
                }
                else
                {
                    mensaje = "Tiene que adjuntar el archivo de la resolución";
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, GetType(), "script", "Alerta('" + mensaje + " <br/>',2);", true);
                }
                

            }
            else
            {
                mensaje = "Seleccione una fila con reprogramación";
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, GetType(), "script", "Alerta('" + mensaje + " <br/>',2);", true);

            }
        }

        protected void btnSubprod_Click(object sender, EventArgs e)
        {
            string aprobado;
            if (gvReprogramaciónes.FocusedRowIndex != -1)
            {
                Session["carga"] = 2;
                Session["poa"] = gvReprogramaciónes.GetRowValues(gvReprogramaciónes.FocusedRowIndex, "SPPRE$POA");
                Session["reprogramacion"] = gvReprogramaciónes.GetRowValues(gvReprogramaciónes.FocusedRowIndex, "SPPRE$ID_REPRO");
                aprobado = gvReprogramaciónes.GetRowValues(gvReprogramaciónes.FocusedRowIndex, "SPPRE$APROBADO").ToString();
                lblreSub.Text = "No. De resolución: " + gvReprogramaciónes.GetRowValues(gvReprogramaciónes.FocusedRowIndex, "SPPRE$RESOLUCION").ToString();
                lblFechsub.Text = "Fecha de la resolución: " + gvReprogramaciónes.GetRowValues(gvReprogramaciónes.FocusedRowIndex, "SPPRE$FECHA_RESOLUCION").ToString();
                if (aprobado == "SI")
                {
                    lblmsgSub.Text = ". Esta reprogramación se encuentra aprobada, los botones de agregar/edición y borrado se encuentran inhabilitados";
                    btnReprosub.Enabled = false;
                    btnMetasfin.Enabled = false;
                    btnBorrarSub.Enabled = false;
                    btnBorrarSubfin.Enabled = false;
                }
                else
                {
                    lblmsgSub.Text = "";
                    btnMetasfin.Enabled = true;
                    btnReprosub.Enabled = true;
                    btnBorrarSub.Enabled = true;
                    btnBorrarSubfin.Enabled = true;
                }

                cargasubproductos(Convert.ToInt32(Session["poa"]), Convert.ToInt32(Session["reprogramacion"]), Convert.ToInt32(cbanio.Value));
                MultiView1.ActiveViewIndex = 2;

            }

            else
            {
                mensaje = "Seleccione una fila con una reprogramación";
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, GetType(), "script", "Alerta('" + mensaje + " <br/>',2);", true);
            }
        }

        protected void cargaproductos(int poa, int reprogramacion, int anio)
        {

            /*
            sql = @"SELECT * FROM (SELECT SPRES$TIPO, SPPRO$ID_RESULTADO, SPPMFS$ID_PRODUCTO, CASE WHEN SPRES$TIPO = 0 THEN 'ACCIONES PGG' WHEN SPRES$TIPO = 1 THEN 'INSTITUCIONAL' END AS TIPO,CASE WHEN SPRES$DESCRIPCION IS NULL THEN  CASE WHEN NIVEL = 2 THEN 'META PGG 2020-2024 - '||DESCRIPCION WHEN NIVEL = 3 THEN 'ACCION: '||DESCRIPCION END ELSE SPRES$DESCRIPCION END AS NOMBRE
                   ,SPPRO$PRESUPUESTO,SPPRO$DESCRIPCION,SNCGUM$NOMBRE, METAFINANCIERA, METAFISICAINICIAL, METAVIGENTE, NIDMF,IDREPROGRA
                    ,CASE WHEN OPERACION IS NULL THEN NULL WHEN OPERACION = 0  THEN '+' || CANTIDAD WHEN OPERACION = 1 THEN '-' || CANTIDAD  END AS OPERACIONREALIZADA
                    ,REPROFISICA, OPERACION, CANTIDAD, NIDMFIN
                    ,CASE WHEN OPERAFINANCIERA IS NULL THEN NULL WHEN OPERAFINANCIERA = 0  THEN '+' || CANTIDADFINANCIERA WHEN OPERAFINANCIERA = 1 THEN '-' || CANTIDADFINANCIERA  END AS OPERACIONFINANCEIRA
                    ,CANTIDADFINANCIERA
                    ,NUEVAMETAFINANCIERA
                    ,REPROFINANCIERA
                    ,OPERAFINANCIERA
                    FROM(
                    SELECT R.SPRES$TIPO, PO.SPPRO$ID_RESULTADO, MF.SPPMFS$ID_PROGRAMACION_FISICA, MF.SPPMFS$ID_PRODUCTO, R.SPRES$DESCRIPCION
                    ,(SELECT SPPRES$CODIGO || ' ' || SPPRES$DESCRIPCION FROM SCHE$SIPLAN20.SP20$RESULTADOS_ESTRATEGICOS WHERE SPPRES$ID_RESULTADO = R.SPRES$COD_ESTRATEGICO) DESCRIPCION
                    ,(SELECT SPPRES$NIVEL FROM SCHE$SIPLAN20.SP20$RESULTADOS_ESTRATEGICOS WHERE SPPRES$ID_RESULTADO = R.SPRES$COD_ESTRATEGICO) NIVEL
                    ,PO.SPPRO$PRESUPUESTO,PO.SPPRO$DESCRIPCION, UM.SNCGUM$NOMBRE
                    ,SCHE$SIPLAN20.FNC$OBTIENEMETAFINACIERAPROD(MF.SPPMFS$ID_POA, " + anio + @", MF.SPPMFS$ID_PRODUCTO, 0) METAFINANCIERA
                    ,SCHE$SIPLAN20.FNC$REPROGRAMACIONPROD(MF.SPPMFS$ID_PRODUCTO, MF.SPPMFS$ID_POA, -1, 1) METAFISICAINICIAL
                    ,SCHE$SIPLAN20.FNC$REPROGRAMACIONPROD(MF.SPPMFS$ID_PRODUCTO, MF.SPPMFS$ID_POA, -1, 2) METAVIGENTE
                    ,SCHE$SIPLAN20.FNC$REPROGRAMACIONPROD(MF.SPPMFS$ID_PRODUCTO, MF.SPPMFS$ID_POA, " + reprogramacion + @", 3) NIDMF
                    ,SCHE$SIPLAN20.FNC$REPROGRAMACIONPROD(MF.SPPMFS$ID_PRODUCTO, MF.SPPMFS$ID_POA, " + reprogramacion + @", 4) IDREPROGRA
                    ,SCHE$SIPLAN20.FNC$REPROGRAMACIONPROD(MF.SPPMFS$ID_PRODUCTO, MF.SPPMFS$ID_POA, " + reprogramacion + @", 5) OPERACION
                    ,SCHE$SIPLAN20.FNC$REPROGRAMACIONPROD(MF.SPPMFS$ID_PRODUCTO, MF.SPPMFS$ID_POA, " + reprogramacion + @", 6) CANTIDAD
                    ,SCHE$SIPLAN20.FNC$REPROGRAMACIONPROD(MF.SPPMFS$ID_PRODUCTO, MF.SPPMFS$ID_POA,  " + reprogramacion + @", 7) REPROFISICA
                    ,SCHE$SIPLAN20.FNC$REPROGRAMACIONPROD(MF.SPPMFS$ID_PRODUCTO, MF.SPPMFS$ID_POA, " + reprogramacion + @", 9) NIDMFIN
                    ,SCHE$SIPLAN20.FNC$REPROGRAMACIONPROD(MF.SPPMFS$ID_PRODUCTO, MF.SPPMFS$ID_POA, " + reprogramacion + @", 10) OPERAFINANCIERA
                    ,SCHE$SIPLAN20.FNC$REPROGRAMACIONPROD(MF.SPPMFS$ID_PRODUCTO, MF.SPPMFS$ID_POA, " + reprogramacion + @", 11) CANTIDADFINANCIERA
                    ,SCHE$SIPLAN20.FNC$REPROGRAMACIONPROD(MF.SPPMFS$ID_PRODUCTO, MF.SPPMFS$ID_POA,  " + reprogramacion + @", 12) NUEVAMETAFINANCIERA
                    ,SCHE$SIPLAN20.FNC$REPROGRAMACIONPROD(MF.SPPMFS$ID_PRODUCTO, MF.SPPMFS$ID_POA, " + reprogramacion + @" , 13) REPROFINANCIERA
                    ,MF.SPPMFS$ID_POA, MF.SPPMFS$ANIO FROM SCHE$SIPLAN20.SP20$PROGRAMACION_MFPRODUCTO MF INNER JOIN
                    SCHE$SIPLAN20.SP20$PRODUCTO PO ON PO.SPPRO$ID_PRODUCTO = MF.SPPMFS$ID_PRODUCTO AND PO.SPPRO$RESTRICTIVA = 'N' AND MF.SPPMFS$RESTRICTIVA = 'N' INNER JOIN
                    SINIP.SNTBCG$UNIDAD_MEDIDA UM ON PO.SPPRO$ID_MEDIDA = UM.SNCGUM$UNIDAD_MEDIDA INNER JOIN
                    SCHE$SIPLAN20.SP20$RESULTADOS R ON PO.SPPRO$ID_RESULTADO = R.SPRES$ID_RESULTADO AND PO.SPPRO$RESTRICTIVA = 'N' AND R.SPRES$RESTRICTIVA = 'N' WHERE MF.SPPMFS$ID_POA = " + poa + @" AND MF.SPPMFS$RESTRICTIVA = 'N' AND MF.SPPMFS$VIGENTE = 0
                    
                    ORDER BY  R.SPRES$TIPO, PO.SPPRO$PRESUPUESTO, MF.SPPMFS$ID_PRODUCTO ASC))
                    GROUP BY
                    SPRES$TIPO, SPPRO$ID_RESULTADO, SPPRO$PRESUPUESTO, SPPMFS$ID_PRODUCTO, TIPO, NOMBRE,SPPRO$DESCRIPCION,SNCGUM$NOMBRE, METAFINANCIERA, METAFISICAINICIAL, METAVIGENTE, NIDMF,IDREPROGRA,OPERACIONREALIZADA,REPROFISICA, OPERACION, CANTIDAD, NIDMFIN, OPERACIONFINANCEIRA,CANTIDADFINANCIERA, NUEVAMETAFINANCIERA,REPROFINANCIERA,OPERAFINANCIERA";
            */
            sql = @"SELECT * FROM (SELECT SPRES$TIPO                    
                    ,SPPRO$ID_RESULTADO
                    ,SPPMFS$ID_PRODUCTO
                    ,CASE WHEN SPRES$TIPO = 0 THEN 'RI/PGG' WHEN SPRES$TIPO = 1 THEN 'RI/PGG' WHEN SPRES$TIPO = 2 THEN 'RE'  END AS TIPO
                    ,CASE WHEN SPRES$TIPO = 0 THEN EJE||ACCION||RESULTADO2 WHEN  SPRES$TIPO = 1 THEN RESULTADO1||EJE2||ACCION2  WHEN SPRES$TIPO = 2 THEN RED END AS NOMBRE
                   ,SPPRO$PRESUPUESTO,SPPRO$DESCRIPCION,SNCGUM$NOMBRE, METAFINANCIERA, METAFISICAINICIAL, METAVIGENTE, NIDMF,IDREPROGRA
                    ,CASE WHEN OPERACION IS NULL THEN NULL WHEN OPERACION = 0  THEN '+' || CANTIDAD WHEN OPERACION = 1 THEN '-' || CANTIDAD  END AS OPERACIONREALIZADA
                    ,REPROFISICA, OPERACION, CANTIDAD, NIDMFIN
                    ,CASE WHEN OPERAFINANCIERA IS NULL THEN NULL WHEN OPERAFINANCIERA = 0  THEN '+' || CANTIDADFINANCIERA WHEN OPERAFINANCIERA = 1 THEN '-' || CANTIDADFINANCIERA  END AS OPERACIONFINANCEIRA
                    ,CANTIDADFINANCIERA
                    ,NUEVAMETAFINANCIERA
                    ,REPROFINANCIERA
                    ,OPERAFINANCIERA
                    FROM(
                    
                    SELECT R.SPRES$TIPO
                    ,PO.SPPRO$ID_RESULTADO
                    ,'EJE:'||SCHE$SIPLAN20.FCN$BUSCA_EJE(PO.SPPRO$ID_RESULTADO)||'-'  EJE
                    ,'ACCION:'||SCHE$SIPLAN20.FCN$BUSCA_ACCION(PO.SPPRO$ID_RESULTADO)||'-'  ACCION
                    ,'EJE:'||SCHE$SIPLAN20.FCN$BUSCA_EJE(PO.SPPRO$RESULTADO2)||'-' EJE2
                    ,'ACCION:'||SCHE$SIPLAN20.FCN$BUSCA_ACCION(PO.SPPRO$RESULTADO2)||'-'  ACCION2
                    ,'RI-'||R.SPRES$DESCRIPCION RESULTADO1
                    ,(SELECT 'RI-'||SPRES$DESCRIPCION FROM SCHE$SIPLAN20.SP20$RESULTADOS WHERE SPRES$ID_RESULTADO = PO.SPPRO$RESULTADO2) RESULTADO2
                    ,(SELECT SPPRED$RED||' '||SPPRED$DESCRIPCION FROM SCHE$SIPLAN20.SP20$RED WHERE SPPRED$ID = R.SPRES$COD_ESTRATEGICO) RED
                    ,MF.SPPMFS$ID_PROGRAMACION_FISICA
                    ,MF.SPPMFS$ID_PRODUCTO
                    , R.SPRES$DESCRIPCION
                    
                    ,PO.SPPRO$PRESUPUESTO,PO.SPPRO$DESCRIPCION, UM.SNCGUM$NOMBRE
                    ,SCHE$SIPLAN20.FNC$OBTIENEMETAFINACIERAPROD(MF.SPPMFS$ID_POA, "+anio+@", MF.SPPMFS$ID_PRODUCTO, 0) METAFINANCIERA
                    ,SCHE$SIPLAN20.FNC$REPROGRAMACIONPROD(MF.SPPMFS$ID_PRODUCTO, MF.SPPMFS$ID_POA, -1, 1) METAFISICAINICIAL
                    ,SCHE$SIPLAN20.FNC$REPROGRAMACIONPROD(MF.SPPMFS$ID_PRODUCTO, MF.SPPMFS$ID_POA, -1, 2) METAVIGENTE
                    ,SCHE$SIPLAN20.FNC$REPROGRAMACIONPROD(MF.SPPMFS$ID_PRODUCTO, MF.SPPMFS$ID_POA, " + reprogramacion + @", 3) NIDMF
                    ,SCHE$SIPLAN20.FNC$REPROGRAMACIONPROD(MF.SPPMFS$ID_PRODUCTO, MF.SPPMFS$ID_POA, " + reprogramacion + @", 4) IDREPROGRA
                    ,SCHE$SIPLAN20.FNC$REPROGRAMACIONPROD(MF.SPPMFS$ID_PRODUCTO, MF.SPPMFS$ID_POA, " + reprogramacion + @", 5) OPERACION
                    ,SCHE$SIPLAN20.FNC$REPROGRAMACIONPROD(MF.SPPMFS$ID_PRODUCTO, MF.SPPMFS$ID_POA, " + reprogramacion + @", 6) CANTIDAD
                    ,SCHE$SIPLAN20.FNC$REPROGRAMACIONPROD(MF.SPPMFS$ID_PRODUCTO, MF.SPPMFS$ID_POA,  " + reprogramacion + @"9, 7) REPROFISICA
                    ,SCHE$SIPLAN20.FNC$REPROGRAMACIONPROD(MF.SPPMFS$ID_PRODUCTO, MF.SPPMFS$ID_POA, " + reprogramacion + @", 9) NIDMFIN
                    ,SCHE$SIPLAN20.FNC$REPROGRAMACIONPROD(MF.SPPMFS$ID_PRODUCTO, MF.SPPMFS$ID_POA, " + reprogramacion + @", 10) OPERAFINANCIERA
                    ,SCHE$SIPLAN20.FNC$REPROGRAMACIONPROD(MF.SPPMFS$ID_PRODUCTO, MF.SPPMFS$ID_POA, " + reprogramacion + @", 11) CANTIDADFINANCIERA
                    ,SCHE$SIPLAN20.FNC$REPROGRAMACIONPROD(MF.SPPMFS$ID_PRODUCTO, MF.SPPMFS$ID_POA, " + reprogramacion + @", 12) NUEVAMETAFINANCIERA
                    ,SCHE$SIPLAN20.FNC$REPROGRAMACIONPROD(MF.SPPMFS$ID_PRODUCTO, MF.SPPMFS$ID_POA, " + reprogramacion + @" , 13) REPROFINANCIERA
                    ,MF.SPPMFS$ID_POA, MF.SPPMFS$ANIO FROM SCHE$SIPLAN20.SP20$PROGRAMACION_MFPRODUCTO MF INNER JOIN
                    SCHE$SIPLAN20.SP20$PRODUCTO PO ON PO.SPPRO$ID_PRODUCTO = MF.SPPMFS$ID_PRODUCTO AND PO.SPPRO$RESTRICTIVA = 'N' AND MF.SPPMFS$RESTRICTIVA = 'N' INNER JOIN
                    SINIP.SNTBCG$UNIDAD_MEDIDA UM ON PO.SPPRO$ID_MEDIDA = UM.SNCGUM$UNIDAD_MEDIDA INNER JOIN
                    SCHE$SIPLAN20.SP20$RESULTADOS R ON PO.SPPRO$ID_RESULTADO = R.SPRES$ID_RESULTADO AND PO.SPPRO$RESTRICTIVA = 'N' AND R.SPRES$RESTRICTIVA = 'N' WHERE MF.SPPMFS$ID_POA = " + poa+@" AND MF.SPPMFS$RESTRICTIVA = 'N' AND MF.SPPMFS$VIGENTE = 0
                    
                   ))
                    GROUP BY
                    SPRES$TIPO, SPPRO$ID_RESULTADO, SPPRO$PRESUPUESTO, SPPMFS$ID_PRODUCTO, TIPO, NOMBRE,SPPRO$DESCRIPCION,SNCGUM$NOMBRE, METAFINANCIERA, METAFISICAINICIAL, METAVIGENTE, NIDMF,IDREPROGRA,OPERACIONREALIZADA,REPROFISICA, OPERACION, CANTIDAD, NIDMFIN, OPERACIONFINANCEIRA,CANTIDADFINANCIERA, NUEVAMETAFINANCIERA,REPROFINANCIERA,OPERAFINANCIERA
                    ORDER BY  SPRES$TIPO, SPPRO$PRESUPUESTO, SPPMFS$ID_PRODUCTO ASC";
            estado = dao.consulta(sql);
            if (estado == 1)
            {
                tabla = dao.tabla;
                gvProductos.DataSource = tabla;
                gvProductos.DataBind();                
            }
        }

        protected void rbOperacion_SelectedIndexChanged(object sender, EventArgs e)
        {
            Double nueva_meta, meta_vigente, diferencia;

            if (txtdiferenciasub.Text == "")
            {
                mensaje = "Ingrese la diferencia a restar o sumar";
                ScriptManager.RegisterStartupScript(this.UpdatePanel7, GetType(), "script", "Alerta('" + mensaje + " <br/>',2);", true);
                txtdiferencia.Focus();
                rbOperacion.SelectedIndex = -1;


            }
            else
            {
                if (lblMetavigentesub.Text == "")
                    meta_vigente = 0;
                else
                    meta_vigente = Convert.ToDouble(lblMetavigentesub.Text);
                diferencia = Convert.ToDouble(txtdiferenciasub.Text);
                if (rbOperacion.SelectedIndex == 0)
                    nueva_meta = meta_vigente + diferencia;
                else
                    nueva_meta = meta_vigente - diferencia;

                txtNuevaMetasub.Text = Convert.ToString(nueva_meta);
            }
            
        }

        protected void btnGrabaMetasub_Click(object sender, EventArgs e)
        {
            int subproducto = -1;
            int reprogramacion = -1;
            int subprogrameta = -1;
            int subreprograma = -1;

           
         

            if (Convert.ToInt32(Session["operaSub"]) == 0)
            {
                if (txtdiferenciasub.Text == "")
                {
                    mensaje = "La cantidad a operar es requerdida";
                    ScriptManager.RegisterStartupScript(this.UpdatePanel7, GetType(), "script", "Alerta('" + mensaje + " <br/>',2);", true);
                    txtdiferenciasub.Focus();
                }
                else if (rbOperacion.SelectedIndex == -1)
                {
                    mensaje = "Seleccione el tipo de operación que se va realizar suma o resta";
                    ScriptManager.RegisterStartupScript(this.UpdatePanel7, GetType(), "script", "Alerta('" + mensaje + " <br/>',2);", true);
                    rbOperacion.Focus();
                }
                else
                {
                    reprogramacion = Convert.ToInt32(Session["reprogramacion"]);
                    //reprogramacion = Convert.ToInt32(gvReprogramaciónes.GetRowValues(gvReprogramaciónes.FocusedRowIndex, "SPPRE$ID_REPRO"));
                    subproducto = Convert.ToInt32(gvSubproductos.GetRowValues(gvSubproductos.FocusedRowIndex, "SPPMFS$ID_SUBPRODUCTO").ToString());
                    sql = "INSERT INTO SCHE$SIPLAN20.SP20$PROGRAMETA_FI_FIN_SUB (SPPMFS$META, SPPMFS$ID_POA, SPPMFS$ANIO, SPPMFS$ID_SUBPRODUCTO, SPPMFS$FECHA_INSERTA, SPPMFS$TPROGRA, SPPMFS$VIGENTE, SPPMFS$TIPO_PROGRAMACION) VALUES(";
                    if (Convert.ToInt32(Session["tipo"]) == 0)
                        sql = sql + Convert.ToDouble(txtNuevaMetasub.Text);
                    else
                        sql = sql + Convert.ToDouble(txtNuevaMetasub.Text);
                   sql = sql+", " + Convert.ToInt32(Session["poa"]) + ", " + Convert.ToInt32(cbanio.Value) + ", " + subproducto + ", 'INSERT = '||TO_CHAR(SYSDATE,'DD/MM/YYYY HH24:MI')||' '||'" + Session["USUARIO"].ToString() + "', 1,1,"+ Convert.ToInt32(Session["tipo"]) + ")";
                    estado = dao.comando(sql);
                    if (estado == 1)
                    {
                        sql = "SELECT MAX(SPPMFS$ID_PROGRAMACION_FISFIN) AS IDMAX FROM SCHE$SIPLAN20.SP20$PROGRAMETA_FI_FIN_SUB WHERE SPPMFS$ID_POA = " + Convert.ToInt32(Session["poa"]) + " AND SPPMFS$ANIO = " + cbanio.Value + " AND SPPMFS$ID_SUBPRODUCTO = " + subproducto + " AND SPPMFS$TPROGRA = 1 AND SPPMFS$TIPO_PROGRAMACION = " + Convert.ToInt32(Session["tipo"]);
                        estado = dao.consulta(sql);
                        if (estado == 1)
                        {
                            tabla = dao.tabla;
                            sql = "INSERT INTO SCHE$SIPLAN20.SP20$REPROGRASUB (SPPRS$REPROGRAMACION, SPPRS$PROGRASUB, SPPRS$OPERACION, SPPRS$CANTIDAD, SPPRS$FECHA_INSERT, SPPRS$TIPO) VALUES (" + reprogramacion + ", " + Convert.ToInt32(tabla.Rows[0]["IDMAX"]) + ", " + rbOperacion.SelectedIndex + ", ";
                            if (Convert.ToInt32(Session["tipo"]) == 0)
                                sql = sql + Convert.ToDouble(txtdiferenciasub.Text);
                            else
                                sql = sql + Convert.ToDouble(txtdiferenciasub.Text);
                            sql=sql+", 'INSERT = '||TO_CHAR(SYSDATE,'DD/MM/YYYY HH24:MI')||' '||'" + Session["USUARIO"].ToString() + "', "+ Convert.ToInt32(Session["tipo"]) + ")";
                            estado = dao.comando(sql);
                            if (estado == 1)
                            {
                                
                                mensaje = "Reprogramación guardada correctamente, la misma estará vigente hasta que se apruebe la reprogramación";
                                ScriptManager.RegisterStartupScript(this.UpdatePanel4, GetType(), "script", "Alerta('" + mensaje + " <br/>',1);", true);
                                cargasubproductos(Convert.ToInt32(Session["poa"]), Convert.ToInt32(Session["reprogramacion"]), Convert.ToInt32(cbanio.Value));
                                
                                popMetaSub.ShowOnPageLoad = false;
                            }
                            else
                            {
                                
                                mensaje = dao.mensaje;
                                ScriptManager.RegisterStartupScript(this.UpdatePanel7, GetType(), "script", "Alerta('" + mensaje + " <br/>',2);", true);
                            }
                        }
                        else
                        {
                            
                            mensaje = dao.mensaje;
                            ScriptManager.RegisterStartupScript(this.UpdatePanel7, GetType(), "script", "Alerta('" + mensaje + " <br/>',2);", true);
                        }
                    }
                    else
                    {
                        mensaje = dao.mensaje;
                        ScriptManager.RegisterStartupScript(this.UpdatePanel7, GetType(), "script", "Alerta('" + mensaje + " <br/>',2);", true);
                    }
                }
            }
            else if (Convert.ToInt32(Session["operaSub"]) == 1)
            {

                if (txtdiferenciasub.Text == "")
                {
                    mensaje = "La cantidad a operar es requerdida";
                    ScriptManager.RegisterStartupScript(this.UpdatePanel7, GetType(), "script", "Alerta('" + mensaje + " <br/>',2);", true);
                    txtdiferencia.Focus();
                }
                else if (rbOperacion.SelectedIndex == -1)
                {
                    mensaje = "Seleccione el tipo de operación que se va realizar suma o resta";
                    ScriptManager.RegisterStartupScript(this.UpdatePanel7, GetType(), "script", "Alerta('" + mensaje + " <br/>',2);", true);
                    operacion.Focus();
                }
                else
                {
                    reprogramacion = Convert.ToInt32(Session["reprogramacion"]);
                    if (Convert.ToInt32(Session["tipo"]) == 0)
                    {
                        subprogrameta = Convert.ToInt32(gvSubproductos.GetRowValues(gvSubproductos.FocusedRowIndex, "NIDMFS").ToString());
                        subreprograma = Convert.ToInt32(gvSubproductos.GetRowValues(gvSubproductos.FocusedRowIndex, "IDREPROMFS").ToString());

                    }
                    else
                    {
                        subprogrameta = Convert.ToInt32(gvSubproductos.GetRowValues(gvSubproductos.FocusedRowIndex, "NIDMFIN").ToString());
                        subreprograma = Convert.ToInt32(gvSubproductos.GetRowValues(gvSubproductos.FocusedRowIndex, "IDREPROMFIN").ToString());
                    }
                        
                    subproducto = Convert.ToInt32(gvSubproductos.GetRowValues(gvSubproductos.FocusedRowIndex, "SPPMFS$ID_SUBPRODUCTO").ToString());

                    sql = "UPDATE SCHE$SIPLAN20.SP20$PROGRAMETA_FI_FIN_SUB SET SPPMFS$META = ";
                    if (Convert.ToInt32(Session["tipo"]) == 0)
                        sql = sql + Convert.ToInt32(txtNuevaMetasub.Text);
                    else
                        sql = sql + Convert.ToDouble(txtNuevaMetasub.Text);
                    sql= sql+", SPPMFS$FECHA_UPDATE = 'UPDATE = '||TO_CHAR(SYSDATE,'DD/MM/YYYY HH24:MI')||' '||'" + Session["USUARIO"].ToString() + "' WHERE  SPPMFS$ID_PROGRAMACION_FISFIN = " + subprogrameta + " AND  SPPMFS$ID_SUBPRODUCTO = " + subproducto + " AND SPPMFS$ID_POA = " + Convert.ToInt32(Session["poa"]) + " AND  SPPMFS$ANIO = " + Convert.ToInt32(cbanio.Value) + " AND SPPMFS$TIPO_PROGRAMACION =" + Convert.ToInt32(Session["tipo"]);
                    estado = dao.comando(sql);
                    if (estado == 1)
                    {
                        sql = "UPDATE SCHE$SIPLAN20.SP20$REPROGRASUB SET SPPRS$OPERACION= " + rbOperacion.SelectedIndex + ", SPPRS$CANTIDAD =";
                        if (Convert.ToInt32(Session["tipo"]) == 0)
                            sql = sql + Convert.ToInt32(txtdiferenciasub.Text);
                        else
                            sql = sql + Convert.ToDouble(txtdiferenciasub.Text);
                            sql= sql  + ", SPPRS$FECHA_UPDATE = 'UPDATE = '||TO_CHAR(SYSDATE,'DD/MM/YYYY HH24:MI')||' '||'" + Session["USUARIO"].ToString() + "'  WHERE SPPRS$ID =  " + subreprograma + "  AND  SPPRS$REPROGRAMACION = " + reprogramacion + " AND SPPRS$PROGRASUB =  " + subprogrameta+ " AND SPPRS$TIPO = "+ Convert.ToInt32(Session["tipo"]);
                        estado = dao.comando(sql);
                        if (estado == 1)
                        {
                            mensaje = "Reprogramación editada correctamente, la misma estará vigente hasta que se apruebe la reprogramación";
                            ScriptManager.RegisterStartupScript(this.UpdatePanel7, GetType(), "script", "Alerta('" + mensaje + " <br/>',1);", true);
                            cargasubproductos(Convert.ToInt32(Session["poa"]), Convert.ToInt32(Session["reprogramacion"]), Convert.ToInt32(cbanio.Value));
                           
                            popMetaSub.ShowOnPageLoad = false;
                        }
                        else
                        {
                            
                            mensaje = dao.mensaje;
                            ScriptManager.RegisterStartupScript(this.UpdatePanel7, GetType(), "script", "Alerta('" + mensaje + " <br/>',2);", true);
                        }


                    }
                    else
                    {
                       
                        mensaje = dao.mensaje;
                        ScriptManager.RegisterStartupScript(this.UpdatePanel7, GetType(), "script", "Alerta('" + mensaje + " <br/>',2);", true);
                    }
                }
            }

            else if (Convert.ToInt32(Session["operaSub"]) == 2)
            {

                reprogramacion = Convert.ToInt32(Session["reprogramacion"]);
                if (Convert.ToInt32(Session["tipo"]) == 0)
                {
                    subprogrameta = Convert.ToInt32(gvSubproductos.GetRowValues(gvSubproductos.FocusedRowIndex, "NIDMFS").ToString());
                    subreprograma = Convert.ToInt32(gvSubproductos.GetRowValues(gvSubproductos.FocusedRowIndex, "IDREPROMFS").ToString());

                }
                else
                {
                    subprogrameta = Convert.ToInt32(gvSubproductos.GetRowValues(gvSubproductos.FocusedRowIndex, "NIDMFIN").ToString());
                    subreprograma = Convert.ToInt32(gvSubproductos.GetRowValues(gvSubproductos.FocusedRowIndex, "IDREPROMFIN").ToString());
                }
                subproducto = Convert.ToInt32(gvSubproductos.GetRowValues(gvSubproductos.FocusedRowIndex, "SPPMFS$ID_SUBPRODUCTO").ToString());

                sql = "UPDATE SCHE$SIPLAN20.SP20$PROGRAMETA_FI_FIN_SUB SET SPPMFS$RESTRICTIVA = 'S', SPPMFS$FECHA_DELETE = 'DELETE = '||TO_CHAR(SYSDATE,'DD/MM/YYYY HH24:MI')||' '||'" + Session["USUARIO"].ToString() + "' WHERE  SPPMFS$ID_PROGRAMACION_FISFIN = " + subprogrameta + " AND  SPPMFS$ID_SUBPRODUCTO = " + subproducto + " AND SPPMFS$ID_POA = " + Convert.ToInt32(Session["poa"]) + " AND  SPPMFS$ANIO = " + Convert.ToInt32(cbanio.Value)+ " AND SPPMFS$TIPO_PROGRAMACION = "+Convert.ToInt32(Session["tipo"]);
                estado = dao.comando(sql);
                if (estado == 1)
                {


                    sql = "UPDATE SCHE$SIPLAN20.SP20$REPROGRASUB SET SPPRS$RESTRICTIVA = 'S',  SPPRS$FECHA_DELETE = 'DELETE = '||TO_CHAR(SYSDATE,'DD/MM/YYYY HH24:MI')||' '||'" + Session["USUARIO"].ToString() + "'  WHERE SPPRS$ID =  " + subreprograma + "  AND  SPPRS$REPROGRAMACION = " + reprogramacion + " AND SPPRS$PROGRASUB =  " + subprogrameta+ " AND SPPRS$TIPO = "+ Convert.ToInt32(Session["tipo"]);
                    estado = dao.comando(sql);
                    if (estado == 1)
                    {
                       
                        mensaje = "Reprogramación borrada correctamente";
                        ScriptManager.RegisterStartupScript(this.UpdatePanel4, GetType(), "script", "Alerta('" + mensaje + " <br/>',1);", true);
                        cargasubproductos(Convert.ToInt32(Session["poa"]), Convert.ToInt32(Session["reprogramacion"]), Convert.ToInt32(cbanio.Value));
                      
                        popMetaSub.ShowOnPageLoad = false;
                    }
                    else
                    {
                        
                        mensaje = dao.mensaje;
                        ScriptManager.RegisterStartupScript(this.UpdatePanel7, GetType(), "script", "Alerta('" + mensaje + " <br/>',2);", true);
                    }


                }
                else
                {
                  
                    mensaje = dao.mensaje;
                    ScriptManager.RegisterStartupScript(this.UpdatePanel7, GetType(), "script", "Alerta('" + mensaje + " <br/>',2);", true);
                }

            }
        }

        protected void btnCerrarSub_Click(object sender, EventArgs e)
        {
            popMetaSub.ShowOnPageLoad = false;
        }

        protected void btnReprosub_Click(object sender, EventArgs e)
        {
            int nivel;
            if (gvSubproductos.FocusedRowIndex != -1)
            {
                nivel = gvSubproductos.GetRowLevel(gvSubproductos.FocusedRowIndex);
                if (nivel == 3)
                {
                    lbltipoprogra.Text = "";
                    lblSuproducto.Text = "";
                    txtdiferenciasub.Text = "";
                    rbOperacion.SelectedIndex = -1;
                    txtNuevaMetasub.Text = "";
                    txtdiferenciasub.Enabled = true;
                    rbOperacion.Enabled = true;

                    lbltipoprogra.Text = "Reprogramación de metas físicas de subproductos";
                    lblSuproducto.Text = gvSubproductos.GetRowValues(gvSubproductos.FocusedRowIndex, "SUBPRODUCTO").ToString();
                    lblUnidadMedidaSub.Text = gvSubproductos.GetRowValues(gvSubproductos.FocusedRowIndex, "MEDIDASUBPRODUCTO").ToString();
                    lblMetaInicialsub.Text = gvSubproductos.GetRowValues(gvSubproductos.FocusedRowIndex, "METAFISICAINICIAL").ToString();
                    lblMetavigentesub.Text = gvSubproductos.GetRowValues(gvSubproductos.FocusedRowIndex, "METAFISICAVIGENTE").ToString();
                    if (gvSubproductos.GetRowValues(gvSubproductos.FocusedRowIndex, "CANTIDADFISICA") != DBNull.Value)
                    {
                        txtdiferenciasub.Text = gvSubproductos.GetRowValues(gvSubproductos.FocusedRowIndex, "CANTIDADFISICA").ToString();
                    }

                    //if (gvProductos.GetRowValues(gvProductos.FocusedRowIndex, "OPERACION") != DBNull.Value)
                    //{
                    //    operacion.SelectedIndex  = Convert.ToInt32(gvProductos.GetRowValues(gvProductos.FocusedRowIndex, "OPERACION").ToString());
                    //}

                    if (gvSubproductos.GetRowValues(gvSubproductos.FocusedRowIndex, "NUEVAMETAFISICA") != DBNull.Value)
                    {
                        txtNuevaMetasub.Text = gvSubproductos.GetRowValues(gvSubproductos.FocusedRowIndex, "NUEVAMETAFISICA").ToString();
                    }
                    if (gvSubproductos.GetRowValues(gvSubproductos.FocusedRowIndex, "NIDMFS") == DBNull.Value && gvSubproductos.GetRowValues(gvSubproductos.FocusedRowIndex, "IDREPROMFS") == DBNull.Value)
                    {
                        Session["operaSub"] = 0;
                        Session["tipo"] = 0;
                        popMetaSub.HeaderText = "Nueva programación de meta física de subproducto";
                        btnGrabaMetasub.Text = "Grabar nueva meta";
                        btnGrabaMetasub.CssClass = "btn btn-primary";
                    }
                    else
                    {
                        Session["operaSub"] = 1;
                        Session["tipo"] = 0;
                        popMetaSub.HeaderText = "Editar programación de meta física de subproducto";
                        btnGrabaMetasub.Text = "Editar reprogramación";
                        btnGrabaMetasub.CssClass = "btn btn-success";
                    }


                    popMetaSub.ShowOnPageLoad = true;
                }
                else
                {
                    mensaje = "Seleccione una fila con un subproducto";
                    ScriptManager.RegisterStartupScript(this.UpdatePanel5, GetType(), "script", "Alerta('" + mensaje + " <br/>',2);", true);
                }
            }

            else
            {
                mensaje = "Seleccione una fila con un subproducto";
                ScriptManager.RegisterStartupScript(this.UpdatePanel5, GetType(), "script", "Alerta('" + mensaje + " <br/>',2);", true);
            }
        }

        protected void btnMetasfin_Click(object sender, EventArgs e)
        {
            int nivel;
            if (gvSubproductos.FocusedRowIndex != -1)
            {
                nivel = gvSubproductos.GetRowLevel(gvSubproductos.FocusedRowIndex);
                if (nivel == 3)
                {
                    lbltipoprogra.Text = "";
                    lblSuproducto.Text = "";
                    txtdiferenciasub.Text = "";
                    rbOperacion.SelectedIndex = -1;
                    txtNuevaMetasub.Text = "";
                    txtdiferenciasub.Enabled = true;
                    rbOperacion.Enabled = true;

                    lbltipoprogra.Text = "Reprogramación de metas financieras de subproductos";
                    lblSuproducto.Text = gvSubproductos.GetRowValues(gvSubproductos.FocusedRowIndex, "SUBPRODUCTO").ToString();
                    lblUnidadMedidaSub.Text = gvSubproductos.GetRowValues(gvSubproductos.FocusedRowIndex, "MEDIDASUBPRODUCTO").ToString();
                    lblMetaInicialsub.Text = gvSubproductos.GetRowValues(gvSubproductos.FocusedRowIndex, "METAFINANCIERAINICIAL").ToString();
                    lblMetavigentesub.Text = gvSubproductos.GetRowValues(gvSubproductos.FocusedRowIndex, "METAFINANCIERAVIGENTE").ToString();
                    if (gvSubproductos.GetRowValues(gvSubproductos.FocusedRowIndex, "CANTIDADFINANCIERA") != DBNull.Value)
                    {
                        txtdiferenciasub.Text = gvSubproductos.GetRowValues(gvSubproductos.FocusedRowIndex, "CANTIDADFINANCIERA").ToString();
                    }

                    //if (gvProductos.GetRowValues(gvProductos.FocusedRowIndex, "OPERACION") != DBNull.Value)
                    //{
                    //    operacion.SelectedIndex  = Convert.ToInt32(gvProductos.GetRowValues(gvProductos.FocusedRowIndex, "OPERACION").ToString());
                    //}

                    if (gvSubproductos.GetRowValues(gvSubproductos.FocusedRowIndex, "NUEVAMETAFINANCIERA") != DBNull.Value)
                    {
                        txtNuevaMetasub.Text = gvSubproductos.GetRowValues(gvSubproductos.FocusedRowIndex, "NUEVAMETAFINANCIERA").ToString();
                    }
                    if (gvSubproductos.GetRowValues(gvSubproductos.FocusedRowIndex, "NIDMFIN") == DBNull.Value && gvSubproductos.GetRowValues(gvSubproductos.FocusedRowIndex, "IDREPROMFIN") == DBNull.Value)
                    {
                        Session["operaSub"] = 0;
                        Session["tipo"] = 1;
                        popMetaSub.HeaderText = "Nueva programación de meta financiera de subproducto";
                        btnGrabaMetasub.Text = "Grabar nueva meta";
                        btnGrabaMetasub.CssClass = "btn btn-primary";
                    }
                    else
                    {
                        Session["operaSub"] = 1;
                        Session["tipo"] = 1;
                        popMetaSub.HeaderText = "Editar programación de meta financiera de subproducto";
                        btnGrabaMetasub.Text = "Editar reprogramación";
                        btnGrabaMetasub.CssClass = "btn btn-success";
                    }


                    popMetaSub.ShowOnPageLoad = true;
                }
                else
                {
                    mensaje = "Seleccione una fila con un subproducto";
                    ScriptManager.RegisterStartupScript(this.UpdatePanel5, GetType(), "script", "Alerta('" + mensaje + " <br/>',2);", true);
                }
            }

            else
            {
                mensaje = "Seleccione una fila con un subproducto";
                ScriptManager.RegisterStartupScript(this.UpdatePanel5, GetType(), "script", "Alerta('" + mensaje + " <br/>',2);", true);
            }
        }

        protected void btnBorrarSub_Click(object sender, EventArgs e)
        {
            int nivel;
            if (gvSubproductos.FocusedRowIndex != -1)
            {
                nivel = gvSubproductos.GetRowLevel(gvSubproductos.FocusedRowIndex);
                if (nivel == 3)
                {
                    txtdiferenciasub.Text = "";
                    rbOperacion.SelectedIndex = -1;
                    txtNuevaMetasub.Text = "";

                    lbltipoprogra.Text = "Reprogramación de metas físicas de Subprodutos";
                    lblSuproducto.Text = gvSubproductos.GetRowValues(gvSubproductos.FocusedRowIndex, "SUBPRODUCTO").ToString();
                    lblUnidadMedidaSub.Text = gvSubproductos.GetRowValues(gvSubproductos.FocusedRowIndex, "MEDIDASUBPRODUCTO").ToString();
                    lblMetaInicialsub.Text = gvSubproductos.GetRowValues(gvSubproductos.FocusedRowIndex, "METAFISICAINICIAL").ToString();
                    lblMetavigentesub.Text = gvSubproductos.GetRowValues(gvSubproductos.FocusedRowIndex, "METAFISICAVIGENTE").ToString();
                    if (gvSubproductos.GetRowValues(gvSubproductos.FocusedRowIndex, "CANTIDADFISICA") != DBNull.Value)
                    {
                       txtdiferenciasub.Text = gvSubproductos.GetRowValues(gvSubproductos.FocusedRowIndex, "CANTIDADFISICA").ToString();
                    }

                    if (gvSubproductos.GetRowValues(gvSubproductos.FocusedRowIndex, "OPERACIONFISICA") != DBNull.Value)
                    {
                        rbOperacion.SelectedIndex = Convert.ToInt32(gvSubproductos.GetRowValues(gvSubproductos.FocusedRowIndex, "OPERACIONFISICA").ToString());
                    }

                    if (gvSubproductos.GetRowValues(gvSubproductos.FocusedRowIndex, "NUEVAMETAFISICA") != DBNull.Value)
                    {
                        txtNuevaMetasub.Text = gvSubproductos.GetRowValues(gvSubproductos.FocusedRowIndex, "NUEVAMETAFISICA").ToString();
                    }
                    if (gvSubproductos.GetRowValues(gvSubproductos.FocusedRowIndex, "NIDMFS") == DBNull.Value && gvSubproductos.GetRowValues(gvSubproductos.FocusedRowIndex, "IDREPROMFS") == DBNull.Value)
                    {
                        Session["operaSub"] = 0;
                        Session["tipo"] = 0;
                        popMetaSub.HeaderText = "Nueva programación de meta física de subproducto";
                        btnGrabaMetasub.Text = "Grabar nueva meta";
                        btnGrabaMetasub.CssClass = "btn btn-primary";
                    }
                    else
                    {
                        Session["operaSub"] = 2;
                        Session["tipo"] = 0;
                        txtdiferenciasub.Enabled = false;
                        rbOperacion.Enabled = false;
                        popMetaSub.HeaderText = "Borrar programación de meta física de subproducto";
                        btnGrabaMetasub.Text = "Borrar reprogramación";
                        btnGrabaMetasub.CssClass = "btn btn-danger";
                    }


                    popMetaSub.ShowOnPageLoad = true;
                }
                else
                {
                    mensaje = "Seleccione una fila con un subproducto";
                    ScriptManager.RegisterStartupScript(this.UpdatePanel7, GetType(), "script", "Alerta('" + mensaje + " <br/>',2);", true);
                }
            }

            else
            {
                mensaje = "Seleccione una fila con un subproducto";
                ScriptManager.RegisterStartupScript(this.UpdatePanel7, GetType(), "script", "Alerta('" + mensaje + " <br/>',2);", true);
            }
        }

        protected void btnBorrarSubfin_Click(object sender, EventArgs e)
        {
            int nivel;
            if (gvSubproductos.FocusedRowIndex != -1)
            {
                nivel = gvSubproductos.GetRowLevel(gvSubproductos.FocusedRowIndex);
                if (nivel == 3)
                {
                    txtdiferenciasub.Text = "";
                    rbOperacion.SelectedIndex = -1;
                    txtNuevaMetasub.Text = "";

                    lbltipoprogra.Text = "Reprogramación de metas financieras de Subprodutos";
                    lblSuproducto.Text = gvSubproductos.GetRowValues(gvSubproductos.FocusedRowIndex, "SUBPRODUCTO").ToString();
                    lblUnidadMedidaSub.Text = gvSubproductos.GetRowValues(gvSubproductos.FocusedRowIndex, "MEDIDASUBPRODUCTO").ToString();
                    lblMetaInicialsub.Text = gvSubproductos.GetRowValues(gvSubproductos.FocusedRowIndex, "METAFINANCIERAINICIAL").ToString();
                    lblMetavigentesub.Text = gvSubproductos.GetRowValues(gvSubproductos.FocusedRowIndex, "METAFINANCIERAVIGENTE").ToString();
                    if (gvSubproductos.GetRowValues(gvSubproductos.FocusedRowIndex, "CANTIDADFINANCIERA") != DBNull.Value)
                    {
                        txtdiferenciasub.Text = gvSubproductos.GetRowValues(gvSubproductos.FocusedRowIndex, "CANTIDADFINANCIERA").ToString();
                    }

                    if (gvSubproductos.GetRowValues(gvSubproductos.FocusedRowIndex, "OPERACIONFINANCIERA") != DBNull.Value)
                    {
                        rbOperacion.SelectedIndex = Convert.ToInt32(gvSubproductos.GetRowValues(gvSubproductos.FocusedRowIndex, "OPERACIONFINANCIERA").ToString());
                    }

                    if (gvSubproductos.GetRowValues(gvSubproductos.FocusedRowIndex, "NUEVAMETAFINANCIERA") != DBNull.Value)
                    {
                        txtNuevaMetasub.Text = gvSubproductos.GetRowValues(gvSubproductos.FocusedRowIndex, "NUEVAMETAFINANCIERA").ToString();
                    }
                    if (gvSubproductos.GetRowValues(gvSubproductos.FocusedRowIndex, "NIDMFIN") == DBNull.Value && gvSubproductos.GetRowValues(gvSubproductos.FocusedRowIndex, "IDREPROMFIN") == DBNull.Value)
                    {
                        Session["operaSub"] = 0;
                        Session["tipo"] = 1;
                        txtdiferenciasub.Enabled = true;
                        rbOperacion.Enabled = true;
                        popMetaSub.HeaderText = "Nueva programación de meta financiera de de subproducto";
                        btnGrabaMetasub.Text = "Grabar nueva meta";
                        btnGrabaMetasub.CssClass = "btn btn-primary";
                    }
                    else
                    {
                        Session["operaSub"] = 2;
                        Session["tipo"] = 1;
                        txtdiferenciasub.Enabled = false;
                        rbOperacion.Enabled = false;
                        popMetaSub.HeaderText = "Borrar programación de meta financiera de subproducto";
                        btnGrabaMetasub.Text = "Borrar reprogramación";
                        btnGrabaMetasub.CssClass = "btn btn-danger";
                    }


                    popMetaSub.ShowOnPageLoad = true;
                }
                else
                {
                    mensaje = "Seleccione una fila con un subproducto";
                    ScriptManager.RegisterStartupScript(this.UpdatePanel7, GetType(), "script", "Alerta('" + mensaje + " <br/>',2);", true);
                }
            }

            else
            {
                mensaje = "Seleccione una fila con un subproducto";
                ScriptManager.RegisterStartupScript(this.UpdatePanel7, GetType(), "script", "Alerta('" + mensaje + " <br/>',2);", true);
            }
        }

        protected void btnRegresarinicio_Click(object sender, EventArgs e)
        {
            Response.Redirect("../modulos/frmModulos.aspx");
        }

        protected void btnDesaprobar_Click(object sender, EventArgs e)
        {
            int reprogramacion = -1;
            DataTable productos = new DataTable();
            DataTable subproductos = new DataTable();
            string aprobado = "";
            bool valida = consultaNoAprobados(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["poa"]));
            int ultima = ultima_repro(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["poa"]));
            if (gvReprogramaciónes.FocusedRowIndex != -1)
            {
                aprobado = gvReprogramaciónes.GetRowValues(gvReprogramaciónes.FocusedRowIndex, "SPPRE$APROBADO").ToString();
                if (valida == true)
                {
                    mensaje = "Hay reprogramaciones pendientes de aprobar, todas la reprogramaciones deben de estar aprobadas para habilitar la ultima ingresada";
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, GetType(), "script", "Alerta('" + mensaje + " <br/>',2);", true);
                }

                else if (ultima != Convert.ToInt32(gvReprogramaciónes.GetRowValues(gvReprogramaciónes.FocusedRowIndex, "SPPRE$ID_REPRO").ToString()))
                {
                    mensaje = "Esta reprogramación, no corresponde a la ultima que ha registrado, solo puede habilitar la ultima reprogramación ingresada";
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, GetType(), "script", "Alerta('" + mensaje + " <br/>',2);", true);
                }

                else if (aprobado == "NO")
                {
                    mensaje = "Esta reprogramación no se encuentra aprobada";
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, GetType(), "script", "Alerta('" + mensaje + " <br/>',2);", true);
                }
                else
                {
                    reprogramacion = Convert.ToInt32(gvReprogramaciónes.GetRowValues(gvReprogramaciónes.FocusedRowIndex, "SPPRE$ID_REPRO").ToString());
                    valida = desaprueba_prod_sub(reprogramacion);
                    if (valida == true)
                    {
                        sql = "UPDATE SCHE$SIPLAN20.SP20$REPROGRAMACION SET SPPRE$APROBADO = 0,  SPPRE$FECHA_APRUEBA = NULL, SPPRE$USUARIOAPRUEBA = NULL ,  SPPRE$FECHA_DESAPRUEBA = TO_CHAR(SYSDATE,'DD/MM/YYYY HH24:MI'), SPPRE$USUARIODESAPRUEBA = '" + Session["USUARIO"].ToString() + "'  WHERE SPPRE$ID_REPRO = " + reprogramacion;
                        estado = dao.comando(sql);
                        if (estado == 1)
                        {
                            mensaje = "La Reprogramación se ha habilitado, correctamente";
                            ScriptManager.RegisterStartupScript(this.UpdatePanel1, GetType(), "script", "Alerta('" + mensaje + " <br/>',1);", true);
                            cargaReprogrogramaciones(Convert.ToInt32(Session["pom"]), Convert.ToInt32(Session["poa"]));
                        }
                        else
                        {
                            mensaje = "Acaba de ocurrir un error y la reprogrmación no ha podido ser habilitada, consulte con el administrador del sistema";
                            ScriptManager.RegisterStartupScript(this.UpdatePanel1, GetType(), "script", "Alerta('" + mensaje + " <br/>',2);", true);
                        }
                    }
                    else
                    {
                        mensaje = dao.mensaje;
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, GetType(), "script", "Alerta('" + mensaje + " <br/>',2);", true);
                    }

                    
                }

            }
            else
            {
                mensaje = "Seleccione una fila con una reprogramación";
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, GetType(), "script", "Alerta('" + mensaje + " <br/>',2);", true);


            }
        }

        protected void cargasubproductos(int poa, int reprogramacion, int anio)
        {

            gvSubproductos.DataSource = null;
            /* sql = @"SELECT SPPMFS$ID_SUBPRODUCTO
                            , CASE WHEN SPRES$TIPO = 0 THEN 'ACCIÓN PGG' WHEN SPRES$TIPO = 1 THEN 'INSTITUCIONAL' END AS TIPO
                            , SPRES$TIPO,CASE WHEN SPRES$TIPO = 0 THEN  CASE WHEN NIVEL = 2 THEN 'META PGG 2020-2024 - '||DESCRIPCION WHEN NIVEL = 3 THEN 'ACCION: '||DESCRIPCION END WHEN SPRES$TIPO = 1 THEN SPRES$DESCRIPCION END AS RESULTADO
                            ,SPPRO$PRESUPUESTO, PRODUCTO, METAFISICAPROD, METAFINANCIERAPROD, CASE WHEN SPPSUB$SNIP IS NULL THEN SUBPRO ELSE NOMBRE_SNIP END AS SUBPRODUCTO
                            , CASE WHEN SPPSUB$SNIP IS NULL THEN MEDIDA_SUB ELSE MEDIDASNIP END AS MEDIDASUBPRODUCTO
                            , SPPMFS$ID_POA, SPPMFS$ANIO,METAFISICAINICIAL,METAFISICAVIGENTE,METAFINANCIERAINICIAL,METAFINANCIERAVIGENTE
                            ,NIDMFS,NIDMFIN,IDREPROMFS,IDREPROMFIN, CASE WHEN OPERACIONFISICA IS NULL THEN NULL WHEN OPERACIONFISICA = 0 THEN '+'||CANTIDADFISICA WHEN OPERACIONFISICA = 1 THEN '-'||CANTIDADFISICA END AS OPERACIONREALIZADAFISICA
                            ,NUEVAMETAFISICA, CASE WHEN OPERACIONFINANCIERA IS NULL THEN NULL WHEN OPERACIONFINANCIERA = 0 THEN '+'||CANTIDADFINANCIERA WHEN OPERACIONFINANCIERA = 1 THEN '-'||CANTIDADFINANCIERA END AS OPERACIONREALIZADAFINANCIERA
                            , NUEVAMETAFINANCIERA,OPERACIONFISICA, CANTIDADFISICA, OPERACIONFINANCIERA, CANTIDADFINANCIERA FROM 
                            (SELECT SPPMFS$ID_SUBPRODUCTO, SPRES$TIPO, SPRES$COD_ESTRATEGICO, SPRES$DESCRIPCION
                            ,(SELECT SPPRES$CODIGO || ' ' || SPPRES$DESCRIPCION FROM SCHE$SIPLAN20.SP20$RESULTADOS_ESTRATEGICOS WHERE SPPRES$ID_RESULTADO = SPRES$COD_ESTRATEGICO) DESCRIPCION
                             ,(SELECT SPPRES$NIVEL FROM SCHE$SIPLAN20.SP20$RESULTADOS_ESTRATEGICOS WHERE SPPRES$ID_RESULTADO = SPRES$COD_ESTRATEGICO) NIVEL
                            ,SPPRO$PRESUPUESTO,SPPRO$DESCRIPCION PRODUCTO, METAFISICAPROD, METAFINANCIERAPROD
                            , SPPSUB$DESCRIPCION SUBPRO
                            ,(SELECT 'SNIP: ' || a.proyecto || '-' || a.nombre PROYECTO  FROM SINIP.BP_PROYECTO_ID a, SINIP.ci_unidad_ejecutora b WHERE a.proyecto = SPPSUB$SNIP  AND a.restrictiva = 'N'  AND a.entidad = b.entidad   AND a.unidad_ejecutora = b.unidad_ejecutora) NOMBRE_SNIP
                            ,SPPSUB$SNIP, SPPSUB$DESCRIPCION, SPPSUB$ID_MEDIDA
                            ,(SELECT a.sncgum$nombre FROM sinip.sntbcg$unidad_medida a WHERE a.sncgum$unidad_medida >= 1000 AND a.sncgum$sigla IS NOT NULL AND a.sncgum$unidad_medida = SPPSUB$ID_MEDIDA) MEDIDA_SUB 
                            ,SCHE$SIPLAN20.FNCOBTIENESNIP(SPPSUB$SNIP, " + anio + @") AS MEDIDASNIP, SPPMFS$ID_POA
                            ,SPPMFS$ANIO,METAFISICAINICIAL,METAFISICAVIGENTE,METAFINANCIERAINICIAL
                            ,METAFINANCIERAVIGENTE,NIDMFS,NIDMFIN,IDREPROMFS
                            ,IDREPROMFIN,OPERACIONFISICA,CANTIDADFISICA,NUEVAMETAFISICA,OPERACIONFINANCIERA,CANTIDADFINANCIERA,NUEVAMETAFINANCIERA FROM 
                            (SELECT SPPMFS$ID_SUBPRODUCTO, R.SPRES$TIPO, R.SPRES$COD_ESTRATEGICO, R.SPRES$DESCRIPCION, PR.SPPRO$PRESUPUESTO, PR.SPPRO$DESCRIPCION
                            ,SCHE$SIPLAN20.FNC$OBTIENEMETAFISICAPRO(SPPMFS$ID_POA, " + anio+@", PR.SPPRO$ID_PRODUCTO) METAFISICAPROD
                            ,SCHE$SIPLAN20.FNC$OBTIENEMETAFINACIERAPROD(SPPMFS$ID_POA, " + anio+@", PR.SPPRO$ID_PRODUCTO,0) METAFINANCIERAPROD
                            ,SUB.SPPSUB$SNIP, SUB.SPPSUB$DESCRIPCION, SUB.SPPSUB$ID_MEDIDA, SPPMFS$ID_POA, SPPMFS$ANIO
                            ,SCHE$SIPLAN20.FNC$REPROGRAMACIONSUB(SPPMFS$ID_SUBPRODUCTO, SPPMFS$ID_POA, SPPMFS$ANIO, 0, -1, 1) METAFISICAINICIAL
                            ,SCHE$SIPLAN20.FNC$REPROGRAMACIONSUB(SPPMFS$ID_SUBPRODUCTO, SPPMFS$ID_POA, SPPMFS$ANIO, 0, -1, 2) METAFISICAVIGENTE
                            ,SCHE$SIPLAN20.FNC$REPROGRAMACIONSUB(SPPMFS$ID_SUBPRODUCTO, SPPMFS$ID_POA, SPPMFS$ANIO, 1, -1, 1) METAFINANCIERAINICIAL
                            ,SCHE$SIPLAN20.FNC$REPROGRAMACIONSUB(SPPMFS$ID_SUBPRODUCTO, SPPMFS$ID_POA, SPPMFS$ANIO, 1, -1, 2) METAFINANCIERAVIGENTE
                            ,SCHE$SIPLAN20.FNC$REPROGRAMACIONSUB(SPPMFS$ID_SUBPRODUCTO, SPPMFS$ID_POA, SPPMFS$ANIO, 0, " + reprogramacion + ", 3) NIDMFS ,SCHE$SIPLAN20.FNC$REPROGRAMACIONSUB(SPPMFS$ID_SUBPRODUCTO, SPPMFS$ID_POA, SPPMFS$ANIO, 1, " + reprogramacion + @", 3) NIDMFIN
                            ,SCHE$SIPLAN20.FNC$REPROGRAMACIONSUB(SPPMFS$ID_SUBPRODUCTO, SPPMFS$ID_POA, SPPMFS$ANIO, 0, "+reprogramacion+", 4) IDREPROMFS,SCHE$SIPLAN20.FNC$REPROGRAMACIONSUB(SPPMFS$ID_SUBPRODUCTO, SPPMFS$ID_POA, SPPMFS$ANIO, 1, " + reprogramacion + @", 4) IDREPROMFIN
                            ,SCHE$SIPLAN20.FNC$REPROGRAMACIONSUB(SPPMFS$ID_SUBPRODUCTO, SPPMFS$ID_POA, SPPMFS$ANIO, 0, "+reprogramacion+", 5) OPERACIONFISICA ,SCHE$SIPLAN20.FNC$REPROGRAMACIONSUB(SPPMFS$ID_SUBPRODUCTO, SPPMFS$ID_POA, SPPMFS$ANIO, 0, " + reprogramacion + @", 6) CANTIDADFISICA
                            ,SCHE$SIPLAN20.FNC$REPROGRAMACIONSUB(SPPMFS$ID_SUBPRODUCTO, SPPMFS$ID_POA, SPPMFS$ANIO, 0, " + reprogramacion + ", 7) NUEVAMETAFISICA ,SCHE$SIPLAN20.FNC$REPROGRAMACIONSUB(SPPMFS$ID_SUBPRODUCTO, SPPMFS$ID_POA, SPPMFS$ANIO, 1, " + reprogramacion + @", 5) OPERACIONFINANCIERA
                            ,SCHE$SIPLAN20.FNC$REPROGRAMACIONSUB(SPPMFS$ID_SUBPRODUCTO, SPPMFS$ID_POA, SPPMFS$ANIO, 1, " + reprogramacion + ", 6) CANTIDADFINANCIERA ,SCHE$SIPLAN20.FNC$REPROGRAMACIONSUB(SPPMFS$ID_SUBPRODUCTO, SPPMFS$ID_POA, SPPMFS$ANIO, 1, " + reprogramacion + @", 7) NUEVAMETAFINANCIERA FROM
                            (SELECT PRS.SPPMFS$ID_SUBPRODUCTO, PRS.SPPMFS$ID_POA, PRS.SPPMFS$ANIO FROM SCHE$SIPLAN20.SP20$PROGRAMETA_FI_FIN_SUB PRS
                            WHERE PRS.SPPMFS$RESTRICTIVA = 'N' AND PRS.SPPMFS$ID_POA = "+poa+" AND PRS.SPPMFS$VIGENTE = 0 AND PRS.SPPMFS$ANIO = "+anio+ @") 
                            INNER JOIN SCHE$SIPLAN20.SP20$SUB_PRODUCTO SUB ON SUB.SPPSUB$ID_SUBPRODUCTO = SPPMFS$ID_SUBPRODUCTO AND SUB.SPPSUB$RESTRICTIVA = 'N'
                            INNER JOIN SCHE$SIPLAN20.SP20$PRODUCTO PR ON SUB.SPPSUB$ID_PRODUCTO = PR.SPPRO$ID_PRODUCTO AND SUB.SPPSUB$RESTRICTIVA = 'N' AND PR.SPPRO$RESTRICTIVA = 'N' 
                            INNER JOIN SCHE$SIPLAN20.SP20$RESULTADOS R ON R.SPRES$ID_RESULTADO = PR.SPPRO$ID_RESULTADO AND R.SPRES$RESTRICTIVA = 'N' AND PR.SPPRO$RESTRICTIVA = 'N'
                            GROUP BY SPPMFS$ID_SUBPRODUCTO, R.SPRES$TIPO, R.SPRES$COD_ESTRATEGICO, R.SPRES$DESCRIPCION, PR.SPPRO$PRESUPUESTO, PR.SPPRO$ID_PRODUCTO, PR.SPPRO$DESCRIPCION, SUB.SPPSUB$SNIP, SUB.SPPSUB$DESCRIPCION, SUB.SPPSUB$ID_MEDIDA, SPPMFS$ID_POA, SPPMFS$ANIO ORDER BY R.SPRES$TIPO ASC))";*/

            sql = @"SELECT SPPMFS$ID_SUBPRODUCTO
                           , CASE WHEN SPRES$TIPO = 0 THEN 'RI/PGG' WHEN SPRES$TIPO = 1 THEN 'RI/PGG' WHEN SPRES$TIPO = 2 THEN 'RE' END AS TIPO
                           , SPRES$TIPO
                           ,CASE WHEN SPRES$TIPO = 0 THEN EJE|| ACCION || RESULTADO2 WHEN SPRES$TIPO = 1 THEN RESULTADO1|| EJE2 || ACCION2  WHEN SPRES$TIPO = 2 THEN RED END AS RESULTADO
                           ,SPPRO$PRESUPUESTO
                           ,PRODUCTO
                           ,METAFISICAPROD
                           ,METAFINANCIERAPROD, CASE WHEN SPPSUB$SNIP IS NULL THEN SUBPRO ELSE NOMBRE_SNIP END AS SUBPRODUCTO
                           , CASE WHEN SPPSUB$SNIP IS NULL THEN MEDIDA_SUB ELSE MEDIDASNIP END AS MEDIDASUBPRODUCTO
                           , SPPMFS$ID_POA, SPPMFS$ANIO,METAFISICAINICIAL
                           ,ROUND(METAFISICAVIGENTE,28) METAFISICAVIGENTE
                            ,METAFINANCIERAINICIAL,METAFINANCIERAVIGENTE
                           ,NIDMFS,NIDMFIN,IDREPROMFS,IDREPROMFIN, CASE WHEN OPERACIONFISICA IS NULL THEN NULL WHEN OPERACIONFISICA = 0 THEN '+' || CANTIDADFISICA WHEN OPERACIONFISICA = 1 THEN '-' || CANTIDADFISICA END AS OPERACIONREALIZADAFISICA
                           ,NUEVAMETAFISICA, CASE WHEN OPERACIONFINANCIERA IS NULL THEN NULL WHEN OPERACIONFINANCIERA = 0 THEN '+' || CANTIDADFINANCIERA WHEN OPERACIONFINANCIERA = 1 THEN '-' || CANTIDADFINANCIERA END AS OPERACIONREALIZADAFINANCIERA
                           , NUEVAMETAFINANCIERA,OPERACIONFISICA, CANTIDADFISICA, OPERACIONFINANCIERA, CANTIDADFINANCIERA FROM
                           (SELECT SPPMFS$ID_SUBPRODUCTO, SPRES$TIPO
                           , EJE
                           , ACCION
                           , EJE2
                           , ACCION2
                           , RESULTADO1
                           , RESULTADO2
                           , RED
                           , SPPRO$PRESUPUESTO, SPPRO$DESCRIPCION PRODUCTO, METAFISICAPROD, METAFINANCIERAPROD
                           , SPPSUB$DESCRIPCION SUBPRO
                           , (SELECT 'SNIP: ' || a.proyecto || '-' || a.nombre PROYECTO FROM SINIP.BP_PROYECTO_ID a, SINIP.ci_unidad_ejecutora b WHERE a.proyecto = SPPSUB$SNIP AND a.restrictiva = 'N'  AND a.entidad = b.entidad   AND a.unidad_ejecutora = b.unidad_ejecutora) NOMBRE_SNIP
                           ,SPPSUB$SNIP, SPPSUB$DESCRIPCION, SPPSUB$ID_MEDIDA
                           ,(SELECT a.sncgum$nombre FROM sinip.sntbcg$unidad_medida a WHERE a.sncgum$unidad_medida >= 1000 AND a.sncgum$sigla IS NOT NULL AND a.sncgum$unidad_medida = SPPSUB$ID_MEDIDA) MEDIDA_SUB
                           ,SCHE$SIPLAN20.FNCOBTIENESNIP(SPPSUB$SNIP, 2025) AS MEDIDASNIP, SPPMFS$ID_POA
                           ,SPPMFS$ANIO,METAFISICAINICIAL,METAFISICAVIGENTE,METAFINANCIERAINICIAL
                           ,METAFINANCIERAVIGENTE,NIDMFS,NIDMFIN,IDREPROMFS
                           ,IDREPROMFIN,OPERACIONFISICA,CANTIDADFISICA
                           ,ROUND(NUEVAMETAFISICA,28) NUEVAMETAFISICA
                            ,OPERACIONFINANCIERA,CANTIDADFINANCIERA,NUEVAMETAFINANCIERA FROM
                           (SELECT SPPMFS$ID_SUBPRODUCTO
                              , R.SPRES$TIPO
                              ,'EJE:' || SCHE$SIPLAN20.FCN$BUSCA_EJE(PR.SPPRO$ID_RESULTADO) || '-'  EJE
                              ,'ACCION:' || SCHE$SIPLAN20.FCN$BUSCA_ACCION(PR.SPPRO$ID_RESULTADO) || '-'  ACCION
                              ,'EJE:' || SCHE$SIPLAN20.FCN$BUSCA_EJE(PR.SPPRO$RESULTADO2) || '-' EJE2
                              ,'ACCION:' || SCHE$SIPLAN20.FCN$BUSCA_ACCION(PR.SPPRO$RESULTADO2) || '-'  ACCION2
                              ,'RI-' || R.SPRES$DESCRIPCION RESULTADO1
                                ,(SELECT 'RI-' || SPRES$DESCRIPCION FROM SCHE$SIPLAN20.SP20$RESULTADOS WHERE SPRES$ID_RESULTADO = PR.SPPRO$RESULTADO2) RESULTADO2
                            ,(SELECT SPPRED$RED || ' ' || SPPRED$DESCRIPCION FROM SCHE$SIPLAN20.SP20$RED WHERE SPPRED$ID = R.SPRES$COD_ESTRATEGICO) RED
                           ,PR.SPPRO$PRESUPUESTO, PR.SPPRO$DESCRIPCION
                           ,SCHE$SIPLAN20.FNC$OBTIENEMETAFISICAPRO(SPPMFS$ID_POA, 2025, PR.SPPRO$ID_PRODUCTO) METAFISICAPROD
                           ,SCHE$SIPLAN20.FNC$OBTIENEMETAFINACIERAPROD(SPPMFS$ID_POA, 2025, PR.SPPRO$ID_PRODUCTO, 0) METAFINANCIERAPROD
                           ,SUB.SPPSUB$SNIP, SUB.SPPSUB$DESCRIPCION, SUB.SPPSUB$ID_MEDIDA, SPPMFS$ID_POA, SPPMFS$ANIO
                           ,SCHE$SIPLAN20.FNC$REPROGRAMACIONSUB(SPPMFS$ID_SUBPRODUCTO, SPPMFS$ID_POA, SPPMFS$ANIO, 0, -1, 1) METAFISICAINICIAL
                           ,SCHE$SIPLAN20.FNC$REPROGRAMACIONSUB(SPPMFS$ID_SUBPRODUCTO, SPPMFS$ID_POA, SPPMFS$ANIO, 0, -1, 2) METAFISICAVIGENTE
                           ,SCHE$SIPLAN20.FNC$REPROGRAMACIONSUB(SPPMFS$ID_SUBPRODUCTO, SPPMFS$ID_POA, SPPMFS$ANIO, 1, -1, 1) METAFINANCIERAINICIAL
                           ,SCHE$SIPLAN20.FNC$REPROGRAMACIONSUB(SPPMFS$ID_SUBPRODUCTO, SPPMFS$ID_POA, SPPMFS$ANIO, 1, -1, 2) METAFINANCIERAVIGENTE
                           ,SCHE$SIPLAN20.FNC$REPROGRAMACIONSUB(SPPMFS$ID_SUBPRODUCTO, SPPMFS$ID_POA, SPPMFS$ANIO, 0, " + reprogramacion + @", 3) NIDMFS ,SCHE$SIPLAN20.FNC$REPROGRAMACIONSUB(SPPMFS$ID_SUBPRODUCTO, SPPMFS$ID_POA, SPPMFS$ANIO, 1, " + reprogramacion + @", 3) NIDMFIN
                           ,SCHE$SIPLAN20.FNC$REPROGRAMACIONSUB(SPPMFS$ID_SUBPRODUCTO, SPPMFS$ID_POA, SPPMFS$ANIO, 0, " + reprogramacion+ @", 4) IDREPROMFS,SCHE$SIPLAN20.FNC$REPROGRAMACIONSUB(SPPMFS$ID_SUBPRODUCTO, SPPMFS$ID_POA, SPPMFS$ANIO, 1, " + reprogramacion + @", 4) IDREPROMFIN
                           ,SCHE$SIPLAN20.FNC$REPROGRAMACIONSUB(SPPMFS$ID_SUBPRODUCTO, SPPMFS$ID_POA, SPPMFS$ANIO, 0, " + reprogramacion+ @", 5) OPERACIONFISICA ,SCHE$SIPLAN20.FNC$REPROGRAMACIONSUB(SPPMFS$ID_SUBPRODUCTO, SPPMFS$ID_POA, SPPMFS$ANIO, 0, " + reprogramacion + @", 6) CANTIDADFISICA
                           ,SCHE$SIPLAN20.FNC$REPROGRAMACIONSUB(SPPMFS$ID_SUBPRODUCTO, SPPMFS$ID_POA, SPPMFS$ANIO, 0, " + reprogramacion+ @", 7) NUEVAMETAFISICA ,SCHE$SIPLAN20.FNC$REPROGRAMACIONSUB(SPPMFS$ID_SUBPRODUCTO, SPPMFS$ID_POA, SPPMFS$ANIO, 1, " + reprogramacion + @", 5) OPERACIONFINANCIERA
                           ,SCHE$SIPLAN20.FNC$REPROGRAMACIONSUB(SPPMFS$ID_SUBPRODUCTO, SPPMFS$ID_POA, SPPMFS$ANIO, 1, " + reprogramacion+ @", 6) CANTIDADFINANCIERA ,SCHE$SIPLAN20.FNC$REPROGRAMACIONSUB(SPPMFS$ID_SUBPRODUCTO, SPPMFS$ID_POA, SPPMFS$ANIO, 1, " + reprogramacion + @", 7) NUEVAMETAFINANCIERA FROM
                           (SELECT PRS.SPPMFS$ID_SUBPRODUCTO, PRS.SPPMFS$ID_POA, PRS.SPPMFS$ANIO FROM SCHE$SIPLAN20.SP20$PROGRAMETA_FI_FIN_SUB PRS
                           WHERE PRS.SPPMFS$RESTRICTIVA = 'N' AND PRS.SPPMFS$ID_POA = "+poa+@" AND PRS.SPPMFS$VIGENTE = 0 AND PRS.SPPMFS$ANIO = "+anio+@")
                           INNER JOIN SCHE$SIPLAN20.SP20$SUB_PRODUCTO SUB ON SUB.SPPSUB$ID_SUBPRODUCTO = SPPMFS$ID_SUBPRODUCTO AND SUB.SPPSUB$RESTRICTIVA = 'N'
                           INNER JOIN SCHE$SIPLAN20.SP20$PRODUCTO PR ON SUB.SPPSUB$ID_PRODUCTO = PR.SPPRO$ID_PRODUCTO AND SUB.SPPSUB$RESTRICTIVA = 'N' AND PR.SPPRO$RESTRICTIVA = 'N'
                           INNER JOIN SCHE$SIPLAN20.SP20$RESULTADOS R ON R.SPRES$ID_RESULTADO = PR.SPPRO$ID_RESULTADO AND R.SPRES$RESTRICTIVA = 'N' AND PR.SPPRO$RESTRICTIVA = 'N'
                          GROUP BY SPPMFS$ID_SUBPRODUCTO, R.SPRES$TIPO, R.SPRES$COD_ESTRATEGICO, R.SPRES$DESCRIPCION, PR.SPPRO$PRESUPUESTO, PR.SPPRO$ID_PRODUCTO, PR.SPPRO$DESCRIPCION, SUB.SPPSUB$SNIP, SUB.SPPSUB$DESCRIPCION, SUB.SPPSUB$ID_MEDIDA, SPPMFS$ID_POA, SPPMFS$ANIO, PR.SPPRO$ID_RESULTADO, PR.SPPRO$RESULTADO2 ))
                            ORDER BY SPRES$TIPO ASC";
            estado = dao.consulta(sql);
            if (estado == 1)
            {
                tabla = dao.tabla;
                gvSubproductos.DataSource = tabla;
                gvSubproductos.DataBind();

            }
        }

        protected void btnVerproductos_Click(object sender, EventArgs e)
        {
            if (gvReprogramaciónes.FocusedRowIndex != -1)
            {
                Session["poa"] = gvReprogramaciónes.GetRowValues(gvReprogramaciónes.FocusedRowIndex, "SPPRE$POA");
                Session["reprogramacion"] = gvReprogramaciónes.GetRowValues(gvReprogramaciónes.FocusedRowIndex, "SPPRE$ID_REPRO");
                lblobservacion.Text = gvReprogramaciónes.GetRowValues(gvReprogramaciónes.FocusedRowIndex, "SPPRE$OBSERVACION").ToString();
                lblResolucionres.Text = "No. De resolución: " + gvReprogramaciónes.GetRowValues(gvReprogramaciónes.FocusedRowIndex, "SPPRE$RESOLUCION").ToString();
                lblFechares.Text = "Fecha de la resolución: " + gvReprogramaciónes.GetRowValues(gvReprogramaciónes.FocusedRowIndex, "SPPRE$FECHA_RESOLUCION").ToString();
                verproductos(Convert.ToInt32(Session["poa"]), Convert.ToInt32(Session["reprogramacion"]), Convert.ToInt32(cbanio.Value));
                versubproductos(Convert.ToInt32(Session["poa"]), Convert.ToInt32(Session["reprogramacion"]), Convert.ToInt32(cbanio.Value));
                popVerReprogramaciones.ShowOnPageLoad = true;

            }

            else
            {
                mensaje = "Seleccione una fila con una reprogramación";
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, GetType(), "script", "Alerta('" + mensaje + " <br/>',2);", true);
            }
        }

        protected void btnCerrarPOP_Click(object sender, EventArgs e)
        {
            popVerReprogramaciones.ShowOnPageLoad = false;
        }

        protected void verproductos(int poa, int reprogramacion, int anio)
        {
           
           /* sql = @"SELECT * FROM (SELECT SPRES$TIPO, SPPRO$ID_RESULTADO, SPPMFS$ID_PRODUCTO, CASE WHEN SPRES$TIPO = 0 THEN 'ACCIONES PGG' WHEN SPRES$TIPO = 1 THEN 'INSTITUCIONAL' END AS TIPO,CASE WHEN SPRES$DESCRIPCION IS NULL THEN CASE WHEN NIVEL = 2 THEN 'META PGG 2020-2024 - '||DESCRIPCION WHEN NIVEL = 3 THEN 'ACCION: '||DESCRIPCION END ELSE SPRES$DESCRIPCION END AS NOMBRE
                   ,SPPRO$DESCRIPCION,SNCGUM$NOMBRE, METAFINANCIERA, METAFISICAINICIAL, METAVIGENTE, NIDMF,IDREPROGRA
                    ,CASE WHEN OPERACION IS NULL THEN NULL WHEN OPERACION = 0  THEN '+' || CANTIDAD WHEN OPERACION = 1 THEN '-' || CANTIDAD  END AS OPERACIONREALIZADA
                    ,REPROFISICA, OPERACION, CANTIDAD, NIDMFIN
                    ,CASE WHEN OPERAFINANCIERA IS NULL THEN NULL WHEN OPERAFINANCIERA = 0  THEN '+' || CANTIDADFINANCIERA WHEN OPERAFINANCIERA = 1 THEN '-' || CANTIDADFINANCIERA  END AS OPERACIONFINANCEIRA
                    ,CANTIDADFINANCIERA
                    ,NUEVAMETAFINANCIERA
                    ,REPROFINANCIERA
                    ,OPERAFINANCIERA
                    FROM(
                    SELECT R.SPRES$TIPO, PO.SPPRO$ID_RESULTADO, MF.SPPMFS$ID_PROGRAMACION_FISICA, MF.SPPMFS$ID_PRODUCTO, R.SPRES$DESCRIPCION
                    ,(SELECT SPPRES$CODIGO || ' ' || SPPRES$DESCRIPCION FROM SCHE$SIPLAN20.SP20$RESULTADOS_ESTRATEGICOS WHERE SPPRES$ID_RESULTADO = R.SPRES$COD_ESTRATEGICO) DESCRIPCION
                    ,(SELECT SPPRES$NIVEL FROM SCHE$SIPLAN20.SP20$RESULTADOS_ESTRATEGICOS WHERE SPPRES$ID_RESULTADO = R.SPRES$COD_ESTRATEGICO) NIVEL
                    ,PO.SPPRO$DESCRIPCION, UM.SNCGUM$NOMBRE
                    ,SCHE$SIPLAN20.FNC$OBTIENEMETAFINACIERAPROD(MF.SPPMFS$ID_POA, " + anio + @", MF.SPPMFS$ID_PRODUCTO, 0) METAFINANCIERA
                    ,SCHE$SIPLAN20.FNC$REPROGRAMACIONPROD(MF.SPPMFS$ID_PRODUCTO, MF.SPPMFS$ID_POA, -1, 1) METAFISICAINICIAL
                    ,SCHE$SIPLAN20.FNC$REPROGRAMACIONPROD(MF.SPPMFS$ID_PRODUCTO, MF.SPPMFS$ID_POA, -1, 2) METAVIGENTE
                    ,SCHE$SIPLAN20.FNC$REPROGRAMACIONPROD(MF.SPPMFS$ID_PRODUCTO, MF.SPPMFS$ID_POA, " + reprogramacion + @", 3) NIDMF
                    ,SCHE$SIPLAN20.FNC$REPROGRAMACIONPROD(MF.SPPMFS$ID_PRODUCTO, MF.SPPMFS$ID_POA, " + reprogramacion + @", 4) IDREPROGRA
                    ,SCHE$SIPLAN20.FNC$REPROGRAMACIONPROD(MF.SPPMFS$ID_PRODUCTO, MF.SPPMFS$ID_POA, " + reprogramacion + @", 5) OPERACION
                    ,SCHE$SIPLAN20.FNC$REPROGRAMACIONPROD(MF.SPPMFS$ID_PRODUCTO, MF.SPPMFS$ID_POA, " + reprogramacion + @", 6) CANTIDAD
                    ,SCHE$SIPLAN20.FNC$REPROGRAMACIONPROD(MF.SPPMFS$ID_PRODUCTO, MF.SPPMFS$ID_POA,  " + reprogramacion + @", 7) REPROFISICA
                    ,SCHE$SIPLAN20.FNC$REPROGRAMACIONPROD(MF.SPPMFS$ID_PRODUCTO, MF.SPPMFS$ID_POA, " + reprogramacion + @", 9) NIDMFIN
                    ,SCHE$SIPLAN20.FNC$REPROGRAMACIONPROD(MF.SPPMFS$ID_PRODUCTO, MF.SPPMFS$ID_POA, " + reprogramacion + @", 10) OPERAFINANCIERA
                    ,SCHE$SIPLAN20.FNC$REPROGRAMACIONPROD(MF.SPPMFS$ID_PRODUCTO, MF.SPPMFS$ID_POA, " + reprogramacion + @", 11) CANTIDADFINANCIERA
                    ,SCHE$SIPLAN20.FNC$REPROGRAMACIONPROD(MF.SPPMFS$ID_PRODUCTO, MF.SPPMFS$ID_POA,  " + reprogramacion + @", 12) NUEVAMETAFINANCIERA
                    ,SCHE$SIPLAN20.FNC$REPROGRAMACIONPROD(MF.SPPMFS$ID_PRODUCTO, MF.SPPMFS$ID_POA, " + reprogramacion + @" , 13) REPROFINANCIERA
                    ,MF.SPPMFS$ID_POA, MF.SPPMFS$ANIO FROM SCHE$SIPLAN20.SP20$PROGRAMACION_MFPRODUCTO MF INNER JOIN
                    SCHE$SIPLAN20.SP20$PRODUCTO PO ON PO.SPPRO$ID_PRODUCTO = MF.SPPMFS$ID_PRODUCTO AND PO.SPPRO$RESTRICTIVA = 'N' AND MF.SPPMFS$RESTRICTIVA = 'N' INNER JOIN
                    SINIP.SNTBCG$UNIDAD_MEDIDA UM ON PO.SPPRO$ID_MEDIDA = UM.SNCGUM$UNIDAD_MEDIDA INNER JOIN
                    SCHE$SIPLAN20.SP20$RESULTADOS R ON PO.SPPRO$ID_RESULTADO = R.SPRES$ID_RESULTADO AND PO.SPPRO$RESTRICTIVA = 'N' AND R.SPRES$RESTRICTIVA = 'N' WHERE MF.SPPMFS$ID_POA = " + poa + @" AND MF.SPPMFS$RESTRICTIVA = 'N' AND MF.SPPMFS$VIGENTE = 0 ORDER BY  R.SPRES$TIPO, MF.SPPMFS$ID_PRODUCTO ASC)) WHERE IDREPROGRA IS NOT NULL OR REPROFINANCIERA IS NOT NULL
                    GROUP BY
                    SPRES$TIPO, SPPRO$ID_RESULTADO, SPPMFS$ID_PRODUCTO, TIPO, NOMBRE,SPPRO$DESCRIPCION,SNCGUM$NOMBRE, METAFINANCIERA, METAFISICAINICIAL, METAVIGENTE, NIDMF,IDREPROGRA,OPERACIONREALIZADA,REPROFISICA, OPERACION, CANTIDAD, NIDMFIN, OPERACIONFINANCEIRA,CANTIDADFINANCIERA, NUEVAMETAFINANCIERA,REPROFINANCIERA,OPERAFINANCIERA";
            */

            sql = @"SELECT * FROM (SELECT SPRES$TIPO                    
                    ,SPPRO$ID_RESULTADO
                    ,SPPMFS$ID_PRODUCTO
                    ,CASE WHEN SPRES$TIPO = 0 THEN 'RI/PGG' WHEN SPRES$TIPO = 1 THEN 'RI/PGG' WHEN SPRES$TIPO = 2 THEN 'RE'  END AS TIPO
                    ,CASE WHEN SPRES$TIPO = 0 THEN EJE||ACCION||RESULTADO2 WHEN  SPRES$TIPO = 1 THEN RESULTADO1||EJE2||ACCION2  WHEN SPRES$TIPO = 2 THEN RED END AS NOMBRE
                   ,SPPRO$PRESUPUESTO,SPPRO$DESCRIPCION,SNCGUM$NOMBRE, METAFINANCIERA, METAFISICAINICIAL, METAVIGENTE, NIDMF,IDREPROGRA
                    ,CASE WHEN OPERACION IS NULL THEN NULL WHEN OPERACION = 0  THEN '+' || CANTIDAD WHEN OPERACION = 1 THEN '-' || CANTIDAD  END AS OPERACIONREALIZADA
                    ,REPROFISICA, OPERACION, CANTIDAD, NIDMFIN
                    ,CASE WHEN OPERAFINANCIERA IS NULL THEN NULL WHEN OPERAFINANCIERA = 0  THEN '+' || CANTIDADFINANCIERA WHEN OPERAFINANCIERA = 1 THEN '-' || CANTIDADFINANCIERA  END AS OPERACIONFINANCEIRA
                    ,CANTIDADFINANCIERA
                    ,NUEVAMETAFINANCIERA
                    ,REPROFINANCIERA
                    ,OPERAFINANCIERA
                    FROM(
                    
                    SELECT R.SPRES$TIPO
                    ,PO.SPPRO$ID_RESULTADO
                    ,'EJE:'||SCHE$SIPLAN20.FCN$BUSCA_EJE(PO.SPPRO$ID_RESULTADO)||'-'  EJE
                    ,'ACCION:'||SCHE$SIPLAN20.FCN$BUSCA_ACCION(PO.SPPRO$ID_RESULTADO)||'-'  ACCION
                    ,'EJE:'||SCHE$SIPLAN20.FCN$BUSCA_EJE(PO.SPPRO$RESULTADO2)||'-' EJE2
                    ,'ACCION:'||SCHE$SIPLAN20.FCN$BUSCA_ACCION(PO.SPPRO$RESULTADO2)||'-'  ACCION2
                    ,'RI-'||R.SPRES$DESCRIPCION RESULTADO1
                    ,(SELECT 'RI-'||SPRES$DESCRIPCION FROM SCHE$SIPLAN20.SP20$RESULTADOS WHERE SPRES$ID_RESULTADO = PO.SPPRO$RESULTADO2) RESULTADO2
                    ,(SELECT SPPRED$RED||' '||SPPRED$DESCRIPCION FROM SCHE$SIPLAN20.SP20$RED WHERE SPPRED$ID = R.SPRES$COD_ESTRATEGICO) RED
                    ,MF.SPPMFS$ID_PROGRAMACION_FISICA
                    ,MF.SPPMFS$ID_PRODUCTO
                    , R.SPRES$DESCRIPCION
                    
                    ,PO.SPPRO$PRESUPUESTO,PO.SPPRO$DESCRIPCION, UM.SNCGUM$NOMBRE
                    ,SCHE$SIPLAN20.FNC$OBTIENEMETAFINACIERAPROD(MF.SPPMFS$ID_POA, "+anio+@", MF.SPPMFS$ID_PRODUCTO, 0) METAFINANCIERA
                    ,SCHE$SIPLAN20.FNC$REPROGRAMACIONPROD(MF.SPPMFS$ID_PRODUCTO, MF.SPPMFS$ID_POA, -1, 1) METAFISICAINICIAL
                    ,SCHE$SIPLAN20.FNC$REPROGRAMACIONPROD(MF.SPPMFS$ID_PRODUCTO, MF.SPPMFS$ID_POA, -1, 2) METAVIGENTE
                    ,SCHE$SIPLAN20.FNC$REPROGRAMACIONPROD(MF.SPPMFS$ID_PRODUCTO, MF.SPPMFS$ID_POA, "+ reprogramacion+ @", 3) NIDMF
                    ,SCHE$SIPLAN20.FNC$REPROGRAMACIONPROD(MF.SPPMFS$ID_PRODUCTO, MF.SPPMFS$ID_POA, " + reprogramacion + @", 4) IDREPROGRA
                    ,SCHE$SIPLAN20.FNC$REPROGRAMACIONPROD(MF.SPPMFS$ID_PRODUCTO, MF.SPPMFS$ID_POA, " + reprogramacion + @", 5) OPERACION
                    ,SCHE$SIPLAN20.FNC$REPROGRAMACIONPROD(MF.SPPMFS$ID_PRODUCTO, MF.SPPMFS$ID_POA, " + reprogramacion + @", 6) CANTIDAD
                    ,SCHE$SIPLAN20.FNC$REPROGRAMACIONPROD(MF.SPPMFS$ID_PRODUCTO, MF.SPPMFS$ID_POA,  " + reprogramacion + @", 7) REPROFISICA
                    ,SCHE$SIPLAN20.FNC$REPROGRAMACIONPROD(MF.SPPMFS$ID_PRODUCTO, MF.SPPMFS$ID_POA, " + reprogramacion + @", 9) NIDMFIN
                    ,SCHE$SIPLAN20.FNC$REPROGRAMACIONPROD(MF.SPPMFS$ID_PRODUCTO, MF.SPPMFS$ID_POA, " + reprogramacion + @", 10) OPERAFINANCIERA
                    ,SCHE$SIPLAN20.FNC$REPROGRAMACIONPROD(MF.SPPMFS$ID_PRODUCTO, MF.SPPMFS$ID_POA, " + reprogramacion + @", 11) CANTIDADFINANCIERA
                    ,SCHE$SIPLAN20.FNC$REPROGRAMACIONPROD(MF.SPPMFS$ID_PRODUCTO, MF.SPPMFS$ID_POA, " + reprogramacion + @", 12) NUEVAMETAFINANCIERA
                    ,SCHE$SIPLAN20.FNC$REPROGRAMACIONPROD(MF.SPPMFS$ID_PRODUCTO, MF.SPPMFS$ID_POA, " + reprogramacion + @" , 13) REPROFINANCIERA
                    ,MF.SPPMFS$ID_POA, MF.SPPMFS$ANIO FROM SCHE$SIPLAN20.SP20$PROGRAMACION_MFPRODUCTO MF INNER JOIN
                    SCHE$SIPLAN20.SP20$PRODUCTO PO ON PO.SPPRO$ID_PRODUCTO = MF.SPPMFS$ID_PRODUCTO AND PO.SPPRO$RESTRICTIVA = 'N' AND MF.SPPMFS$RESTRICTIVA = 'N' INNER JOIN
                    SINIP.SNTBCG$UNIDAD_MEDIDA UM ON PO.SPPRO$ID_MEDIDA = UM.SNCGUM$UNIDAD_MEDIDA INNER JOIN
                    SCHE$SIPLAN20.SP20$RESULTADOS R ON PO.SPPRO$ID_RESULTADO = R.SPRES$ID_RESULTADO AND PO.SPPRO$RESTRICTIVA = 'N' AND R.SPRES$RESTRICTIVA = 'N' WHERE MF.SPPMFS$ID_POA = "+poa+@" AND MF.SPPMFS$RESTRICTIVA = 'N' AND MF.SPPMFS$VIGENTE = 0
                    
                   ))
                    GROUP BY
                    SPRES$TIPO, SPPRO$ID_RESULTADO, SPPRO$PRESUPUESTO, SPPMFS$ID_PRODUCTO, TIPO, NOMBRE,SPPRO$DESCRIPCION,SNCGUM$NOMBRE, METAFINANCIERA, METAFISICAINICIAL, METAVIGENTE, NIDMF,IDREPROGRA,OPERACIONREALIZADA,REPROFISICA, OPERACION, CANTIDAD, NIDMFIN, OPERACIONFINANCEIRA,CANTIDADFINANCIERA, NUEVAMETAFINANCIERA,REPROFINANCIERA,OPERAFINANCIERA
                    ORDER BY  SPRES$TIPO, SPPRO$PRESUPUESTO, SPPMFS$ID_PRODUCTO ASC";


            estado = dao.consulta(sql);
            if (estado == 1)
            {
                tabla = dao.tabla;
                gvVerProductos.DataSource = tabla;
                gvVerProductos.DataBind();
                gvVerProductos.ExpandAll();
            }
        }

        protected void versubproductos(int poa, int reprogramacion, int anio)
        {
           
           /* sql = @"SELECT SPPMFS$ID_SUBPRODUCTO
,CASE WHEN SPRES$TIPO = 0 THEN 'ACCIÓN PGG' WHEN SPRES$TIPO = 1 THEN 'INSTITUCIONAL' END AS TIPO
,SPRES$TIPO
,CASE WHEN SPRES$TIPO = 0 THEN CASE WHEN NIVEL = 2 THEN 'META PGG 2020-2024 - '||DESCRIPCION WHEN NIVEL = 3 THEN 'ACCION: '||DESCRIPCION END WHEN SPRES$TIPO = 1 THEN SPRES$DESCRIPCION END AS RESULTADO
, PRODUCTO
, METAFISICAPROD
, METAFINANCIERAPROD
, CASE WHEN SPPSUB$SNIP IS NULL THEN SUBPRO ELSE NOMBRE_SNIP END AS SUBPRODUCTO
, CASE WHEN SPPSUB$SNIP IS NULL THEN MEDIDA_SUB ELSE MEDIDASNIP END AS MEDIDASUBPRODUCTO
, SPPMFS$ID_POA
, SPPMFS$ANIO
,METAFISICAINICIAL,METAFISICAVIGENTE,METAFINANCIERAINICIAL,METAFINANCIERAVIGENTE,NIDMFS,NIDMFIN,IDREPROMFS,IDREPROMFIN
, CASE WHEN OPERACIONFISICA IS NULL THEN NULL WHEN OPERACIONFISICA = 0 THEN '+'||CANTIDADFISICA WHEN OPERACIONFISICA = 1 THEN '-'||CANTIDADFISICA END AS OPERACIONREALIZADAFISICA
,NUEVAMETAFISICA
, CASE WHEN OPERACIONFINANCIERA IS NULL THEN NULL WHEN OPERACIONFINANCIERA = 0 THEN '+'||CANTIDADFINANCIERA WHEN OPERACIONFINANCIERA = 1 THEN '-'||CANTIDADFINANCIERA END AS OPERACIONREALIZADAFINANCIERA
, NUEVAMETAFINANCIERA,OPERACIONFISICA, CANTIDADFISICA, OPERACIONFINANCIERA
, CANTIDADFINANCIERA FROM 
(SELECT SPPMFS$ID_SUBPRODUCTO
  , SPRES$TIPO
  , SPRES$COD_ESTRATEGICO
  , SPRES$DESCRIPCION
  , (SELECT SPPRES$CODIGO || ' ' || SPPRES$DESCRIPCION FROM SCHE$SIPLAN20.SP20$RESULTADOS_ESTRATEGICOS WHERE SPPRES$ID_RESULTADO = SPRES$COD_ESTRATEGICO) DESCRIPCION
, (SELECT SPPRES$NIVEL FROM SCHE$SIPLAN20.SP20$RESULTADOS_ESTRATEGICOS WHERE SPPRES$ID_RESULTADO = SPRES$COD_ESTRATEGICO) NIVEL
  , SPPRO$DESCRIPCION PRODUCTO, METAFISICAPROD
  , METAFINANCIERAPROD, SPPSUB$DESCRIPCION SUBPRO
  , (SELECT 'SNIP: ' || a.proyecto || '-' || a.nombre PROYECTO  FROM SINIP.BP_PROYECTO_ID a, SINIP.ci_unidad_ejecutora b WHERE a.proyecto = SPPSUB$SNIP  AND a.restrictiva = 'N'  AND a.entidad = b.entidad   AND a.unidad_ejecutora = b.unidad_ejecutora) NOMBRE_SNIP
  ,SPPSUB$SNIP
  , SPPSUB$DESCRIPCION
  , SPPSUB$ID_MEDIDA
  ,(SELECT a.sncgum$nombre FROM sinip.sntbcg$unidad_medida a WHERE a.sncgum$unidad_medida >= 1000 AND a.sncgum$sigla IS NOT NULL AND a.sncgum$unidad_medida = SPPSUB$ID_MEDIDA) MEDIDA_SUB  
  ,SCHE$SIPLAN20.FNCOBTIENESNIP(SPPSUB$SNIP, 2024) AS MEDIDASNIP
  , SPPMFS$ID_POA
  , SPPMFS$ANIO
  ,METAFISICAINICIAL,METAFISICAVIGENTE,METAFINANCIERAINICIAL,METAFINANCIERAVIGENTE,NIDMFS,NIDMFIN,IDREPROMFS,IDREPROMFIN,OPERACIONFISICA,CANTIDADFISICA,NUEVAMETAFISICA,OPERACIONFINANCIERA,CANTIDADFINANCIERA,NUEVAMETAFINANCIERA FROM 
  (SELECT SPPMFS$ID_SUBPRODUCTO
  , R.SPRES$TIPO
  , R.SPRES$COD_ESTRATEGICO
  , R.SPRES$DESCRIPCION
  , PR.SPPRO$DESCRIPCION
  , SCHE$SIPLAN20.FNC$OBTIENEMETAFISICAPRO(SPPMFS$ID_POA, "+anio+@", PR.SPPRO$ID_PRODUCTO) METAFISICAPROD
  , SCHE$SIPLAN20.FNC$OBTIENEMETAFINACIERAPROD(SPPMFS$ID_POA, "+anio+@", PR.SPPRO$ID_PRODUCTO,0) METAFINANCIERAPROD
  , SUB.SPPSUB$SNIP, SUB.SPPSUB$DESCRIPCION
  , SUB.SPPSUB$ID_MEDIDA, SPPMFS$ID_POA, SPPMFS$ANIO
  , SCHE$SIPLAN20.FNC$REPROGRAMACIONSUB(SPPMFS$ID_SUBPRODUCTO, SPPMFS$ID_POA, SPPMFS$ANIO, 0, -1, 1) METAFISICAINICIAL
  , SCHE$SIPLAN20.FNC$REPROGRAMACIONSUB(SPPMFS$ID_SUBPRODUCTO, SPPMFS$ID_POA, SPPMFS$ANIO, 0, -1, 2) METAFISICAVIGENTE
  , SCHE$SIPLAN20.FNC$REPROGRAMACIONSUB(SPPMFS$ID_SUBPRODUCTO, SPPMFS$ID_POA, SPPMFS$ANIO, 1, -1, 1) METAFINANCIERAINICIAL
  , SCHE$SIPLAN20.FNC$REPROGRAMACIONSUB(SPPMFS$ID_SUBPRODUCTO, SPPMFS$ID_POA, SPPMFS$ANIO, 1, -1, 2) METAFINANCIERAVIGENTE
  , SCHE$SIPLAN20.FNC$REPROGRAMACIONSUB(SPPMFS$ID_SUBPRODUCTO, SPPMFS$ID_POA, SPPMFS$ANIO, 0, "+reprogramacion+ @", 3) NIDMFS 
  ,SCHE$SIPLAN20.FNC$REPROGRAMACIONSUB(SPPMFS$ID_SUBPRODUCTO, SPPMFS$ID_POA, SPPMFS$ANIO, 1, " + reprogramacion + @", 3) NIDMFIN
  , SCHE$SIPLAN20.FNC$REPROGRAMACIONSUB(SPPMFS$ID_SUBPRODUCTO, SPPMFS$ID_POA, SPPMFS$ANIO, 0, " + reprogramacion + @", 4) IDREPROMFS
  , SCHE$SIPLAN20.FNC$REPROGRAMACIONSUB(SPPMFS$ID_SUBPRODUCTO, SPPMFS$ID_POA, SPPMFS$ANIO, 1, " + reprogramacion + @", 4) IDREPROMFIN
  , SCHE$SIPLAN20.FNC$REPROGRAMACIONSUB(SPPMFS$ID_SUBPRODUCTO, SPPMFS$ID_POA, SPPMFS$ANIO, 0, " + reprogramacion + @", 5) OPERACIONFISICA
  , SCHE$SIPLAN20.FNC$REPROGRAMACIONSUB(SPPMFS$ID_SUBPRODUCTO, SPPMFS$ID_POA, SPPMFS$ANIO, 0, " + reprogramacion + @", 6) CANTIDADFISICA
  , SCHE$SIPLAN20.FNC$REPROGRAMACIONSUB(SPPMFS$ID_SUBPRODUCTO, SPPMFS$ID_POA, SPPMFS$ANIO, 0, " + reprogramacion + @", 7) NUEVAMETAFISICA
  , SCHE$SIPLAN20.FNC$REPROGRAMACIONSUB(SPPMFS$ID_SUBPRODUCTO, SPPMFS$ID_POA, SPPMFS$ANIO, 1, " + reprogramacion + @", 5) OPERACIONFINANCIERA 
  , SCHE$SIPLAN20.FNC$REPROGRAMACIONSUB(SPPMFS$ID_SUBPRODUCTO, SPPMFS$ID_POA, SPPMFS$ANIO, 1, " + reprogramacion + @", 6) CANTIDADFINANCIERA
  , SCHE$SIPLAN20.FNC$REPROGRAMACIONSUB(SPPMFS$ID_SUBPRODUCTO, SPPMFS$ID_POA, SPPMFS$ANIO, 1, " + reprogramacion + @", 7) NUEVAMETAFINANCIERA FROM  
  (SELECT PRS.SPPMFS$ID_SUBPRODUCTO
  , PRS.SPPMFS$ID_POA
  , PRS.SPPMFS$ANIO FROM SCHE$SIPLAN20.SP20$PROGRAMETA_FI_FIN_SUB PRS
  WHERE PRS.SPPMFS$RESTRICTIVA = 'N' AND PRS.SPPMFS$ID_POA = "+poa+@" AND PRS.SPPMFS$VIGENTE = 0 AND PRS.SPPMFS$ANIO = "+anio+@") 
  INNER JOIN SCHE$SIPLAN20.SP20$SUB_PRODUCTO SUB ON SUB.SPPSUB$ID_SUBPRODUCTO = SPPMFS$ID_SUBPRODUCTO AND SUB.SPPSUB$RESTRICTIVA = 'N' 
  INNER JOIN SCHE$SIPLAN20.SP20$PRODUCTO PR ON SUB.SPPSUB$ID_PRODUCTO = PR.SPPRO$ID_PRODUCTO AND SUB.SPPSUB$RESTRICTIVA = 'N' AND PR.SPPRO$RESTRICTIVA = 'N'
  INNER JOIN SCHE$SIPLAN20.SP20$RESULTADOS R ON R.SPRES$ID_RESULTADO = PR.SPPRO$ID_RESULTADO AND R.SPRES$RESTRICTIVA = 'N' AND PR.SPPRO$RESTRICTIVA = 'N' 
  GROUP BY SPPMFS$ID_SUBPRODUCTO, R.SPRES$TIPO, R.SPRES$COD_ESTRATEGICO, R.SPRES$DESCRIPCION, PR.SPPRO$ID_PRODUCTO, PR.SPPRO$DESCRIPCION, SUB.SPPSUB$SNIP, SUB.SPPSUB$DESCRIPCION, SUB.SPPSUB$ID_MEDIDA, SPPMFS$ID_POA, SPPMFS$ANIO ORDER BY R.SPRES$TIPO ASC)) 
  WHERE IDREPROMFS IS NOT NULL OR IDREPROMFIN IS NOT NULL";
            */

            sql = @"SELECT SPPMFS$ID_SUBPRODUCTO
                           , CASE WHEN SPRES$TIPO = 0 THEN 'RI/PGG' WHEN SPRES$TIPO = 1 THEN 'RI/PGG' WHEN SPRES$TIPO = 2 THEN 'RE' END AS TIPO
                           , SPRES$TIPO
                           ,CASE WHEN SPRES$TIPO = 0 THEN EJE|| ACCION || RESULTADO2 WHEN SPRES$TIPO = 1 THEN RESULTADO1|| EJE2 || ACCION2  WHEN SPRES$TIPO = 2 THEN RED END AS RESULTADO
                           ,SPPRO$PRESUPUESTO
                           ,PRODUCTO
                           ,METAFISICAPROD
                           ,METAFINANCIERAPROD, CASE WHEN SPPSUB$SNIP IS NULL THEN SUBPRO ELSE NOMBRE_SNIP END AS SUBPRODUCTO
                           , CASE WHEN SPPSUB$SNIP IS NULL THEN MEDIDA_SUB ELSE MEDIDASNIP END AS MEDIDASUBPRODUCTO
                           , SPPMFS$ID_POA, SPPMFS$ANIO,METAFISICAINICIAL
                            ,ROUND(METAFISICAVIGENTE,28) METAFISICAVIGENTE
                            ,METAFINANCIERAINICIAL,METAFINANCIERAVIGENTE
                           ,NIDMFS,NIDMFIN,IDREPROMFS,IDREPROMFIN, CASE WHEN OPERACIONFISICA IS NULL THEN NULL WHEN OPERACIONFISICA = 0 THEN '+' || CANTIDADFISICA WHEN OPERACIONFISICA = 1 THEN '-' || CANTIDADFISICA END AS OPERACIONREALIZADAFISICA
                          ,ROUND(NUEVAMETAFISICA,28) NUEVAMETAFISICA
                            , CASE WHEN OPERACIONFINANCIERA IS NULL THEN NULL WHEN OPERACIONFINANCIERA = 0 THEN '+' || CANTIDADFINANCIERA WHEN OPERACIONFINANCIERA = 1 THEN '-' || CANTIDADFINANCIERA END AS OPERACIONREALIZADAFINANCIERA
                           , NUEVAMETAFINANCIERA,OPERACIONFISICA, CANTIDADFISICA, OPERACIONFINANCIERA, CANTIDADFINANCIERA FROM
                           (SELECT SPPMFS$ID_SUBPRODUCTO, SPRES$TIPO
                           , EJE
                           , ACCION
                           , EJE2
                           , ACCION2
                           , RESULTADO1
                           , RESULTADO2
                           , RED
                           , SPPRO$PRESUPUESTO, SPPRO$DESCRIPCION PRODUCTO, METAFISICAPROD, METAFINANCIERAPROD
                           , SPPSUB$DESCRIPCION SUBPRO
                           , (SELECT 'SNIP: ' || a.proyecto || '-' || a.nombre PROYECTO FROM SINIP.BP_PROYECTO_ID a, SINIP.ci_unidad_ejecutora b WHERE a.proyecto = SPPSUB$SNIP AND a.restrictiva = 'N'  AND a.entidad = b.entidad   AND a.unidad_ejecutora = b.unidad_ejecutora) NOMBRE_SNIP
                           ,SPPSUB$SNIP, SPPSUB$DESCRIPCION, SPPSUB$ID_MEDIDA
                           ,(SELECT a.sncgum$nombre FROM sinip.sntbcg$unidad_medida a WHERE a.sncgum$unidad_medida >= 1000 AND a.sncgum$sigla IS NOT NULL AND a.sncgum$unidad_medida = SPPSUB$ID_MEDIDA) MEDIDA_SUB
                           ,SCHE$SIPLAN20.FNCOBTIENESNIP(SPPSUB$SNIP, " + anio+@") AS MEDIDASNIP, SPPMFS$ID_POA
                           ,SPPMFS$ANIO,METAFISICAINICIAL,METAFISICAVIGENTE,METAFINANCIERAINICIAL
                           ,METAFINANCIERAVIGENTE,NIDMFS,NIDMFIN,IDREPROMFS
                           ,IDREPROMFIN,OPERACIONFISICA,CANTIDADFISICA,NUEVAMETAFISICA,OPERACIONFINANCIERA,CANTIDADFINANCIERA,NUEVAMETAFINANCIERA FROM
                           (SELECT SPPMFS$ID_SUBPRODUCTO
                              , R.SPRES$TIPO
                              ,'EJE:' || SCHE$SIPLAN20.FCN$BUSCA_EJE(PR.SPPRO$ID_RESULTADO) || '-'  EJE
                              ,'ACCION:' || SCHE$SIPLAN20.FCN$BUSCA_ACCION(PR.SPPRO$ID_RESULTADO) || '-'  ACCION
                              ,'EJE:' || SCHE$SIPLAN20.FCN$BUSCA_EJE(PR.SPPRO$RESULTADO2) || '-' EJE2
                              ,'ACCION:' || SCHE$SIPLAN20.FCN$BUSCA_ACCION(PR.SPPRO$RESULTADO2) || '-'  ACCION2
                              ,'RI-' || R.SPRES$DESCRIPCION RESULTADO1
                                ,(SELECT 'RI-' || SPRES$DESCRIPCION FROM SCHE$SIPLAN20.SP20$RESULTADOS WHERE SPRES$ID_RESULTADO = PR.SPPRO$RESULTADO2) RESULTADO2
                            ,(SELECT SPPRED$RED || ' ' || SPPRED$DESCRIPCION FROM SCHE$SIPLAN20.SP20$RED WHERE SPPRED$ID = R.SPRES$COD_ESTRATEGICO) RED
                           ,PR.SPPRO$PRESUPUESTO, PR.SPPRO$DESCRIPCION
                           ,SCHE$SIPLAN20.FNC$OBTIENEMETAFISICAPRO(SPPMFS$ID_POA, 2025, PR.SPPRO$ID_PRODUCTO) METAFISICAPROD
                           ,SCHE$SIPLAN20.FNC$OBTIENEMETAFINACIERAPROD(SPPMFS$ID_POA, 2025, PR.SPPRO$ID_PRODUCTO, 0) METAFINANCIERAPROD
                           ,SUB.SPPSUB$SNIP, SUB.SPPSUB$DESCRIPCION, SUB.SPPSUB$ID_MEDIDA, SPPMFS$ID_POA, SPPMFS$ANIO
                           ,SCHE$SIPLAN20.FNC$REPROGRAMACIONSUB(SPPMFS$ID_SUBPRODUCTO, SPPMFS$ID_POA, SPPMFS$ANIO, 0, -1, 1) METAFISICAINICIAL
                           ,SCHE$SIPLAN20.FNC$REPROGRAMACIONSUB(SPPMFS$ID_SUBPRODUCTO, SPPMFS$ID_POA, SPPMFS$ANIO, 0, -1, 2) METAFISICAVIGENTE
                           ,SCHE$SIPLAN20.FNC$REPROGRAMACIONSUB(SPPMFS$ID_SUBPRODUCTO, SPPMFS$ID_POA, SPPMFS$ANIO, 1, -1, 1) METAFINANCIERAINICIAL
                           ,SCHE$SIPLAN20.FNC$REPROGRAMACIONSUB(SPPMFS$ID_SUBPRODUCTO, SPPMFS$ID_POA, SPPMFS$ANIO, 1, -1, 2) METAFINANCIERAVIGENTE
                           ,SCHE$SIPLAN20.FNC$REPROGRAMACIONSUB(SPPMFS$ID_SUBPRODUCTO, SPPMFS$ID_POA, SPPMFS$ANIO, 0, " + reprogramacion + @", 3) NIDMFS ,SCHE$SIPLAN20.FNC$REPROGRAMACIONSUB(SPPMFS$ID_SUBPRODUCTO, SPPMFS$ID_POA, SPPMFS$ANIO, 1, " + reprogramacion + @", 3) NIDMFIN
                           ,SCHE$SIPLAN20.FNC$REPROGRAMACIONSUB(SPPMFS$ID_SUBPRODUCTO, SPPMFS$ID_POA, SPPMFS$ANIO, 0, " + reprogramacion + @", 4) IDREPROMFS,SCHE$SIPLAN20.FNC$REPROGRAMACIONSUB(SPPMFS$ID_SUBPRODUCTO, SPPMFS$ID_POA, SPPMFS$ANIO, 1, " + reprogramacion + @", 4) IDREPROMFIN
                           ,SCHE$SIPLAN20.FNC$REPROGRAMACIONSUB(SPPMFS$ID_SUBPRODUCTO, SPPMFS$ID_POA, SPPMFS$ANIO, 0, " + reprogramacion + @", 5) OPERACIONFISICA ,SCHE$SIPLAN20.FNC$REPROGRAMACIONSUB(SPPMFS$ID_SUBPRODUCTO, SPPMFS$ID_POA, SPPMFS$ANIO, 0, " + reprogramacion + @", 6) CANTIDADFISICA
                           ,SCHE$SIPLAN20.FNC$REPROGRAMACIONSUB(SPPMFS$ID_SUBPRODUCTO, SPPMFS$ID_POA, SPPMFS$ANIO, 0, " + reprogramacion + @", 7) NUEVAMETAFISICA ,SCHE$SIPLAN20.FNC$REPROGRAMACIONSUB(SPPMFS$ID_SUBPRODUCTO, SPPMFS$ID_POA, SPPMFS$ANIO, 1, " + reprogramacion + @", 5) OPERACIONFINANCIERA
                           ,SCHE$SIPLAN20.FNC$REPROGRAMACIONSUB(SPPMFS$ID_SUBPRODUCTO, SPPMFS$ID_POA, SPPMFS$ANIO, 1, " + reprogramacion + @", 6) CANTIDADFINANCIERA ,SCHE$SIPLAN20.FNC$REPROGRAMACIONSUB(SPPMFS$ID_SUBPRODUCTO, SPPMFS$ID_POA, SPPMFS$ANIO, 1, " + reprogramacion + @", 7) NUEVAMETAFINANCIERA FROM
                           (SELECT PRS.SPPMFS$ID_SUBPRODUCTO, PRS.SPPMFS$ID_POA, PRS.SPPMFS$ANIO FROM SCHE$SIPLAN20.SP20$PROGRAMETA_FI_FIN_SUB PRS
                           WHERE PRS.SPPMFS$RESTRICTIVA = 'N' AND PRS.SPPMFS$ID_POA = " + poa+@" AND PRS.SPPMFS$VIGENTE = 0 AND PRS.SPPMFS$ANIO = "+anio+@")
                           INNER JOIN SCHE$SIPLAN20.SP20$SUB_PRODUCTO SUB ON SUB.SPPSUB$ID_SUBPRODUCTO = SPPMFS$ID_SUBPRODUCTO AND SUB.SPPSUB$RESTRICTIVA = 'N'
                           INNER JOIN SCHE$SIPLAN20.SP20$PRODUCTO PR ON SUB.SPPSUB$ID_PRODUCTO = PR.SPPRO$ID_PRODUCTO AND SUB.SPPSUB$RESTRICTIVA = 'N' AND PR.SPPRO$RESTRICTIVA = 'N'
                           INNER JOIN SCHE$SIPLAN20.SP20$RESULTADOS R ON R.SPRES$ID_RESULTADO = PR.SPPRO$ID_RESULTADO AND R.SPRES$RESTRICTIVA = 'N' AND PR.SPPRO$RESTRICTIVA = 'N'
                          GROUP BY SPPMFS$ID_SUBPRODUCTO, R.SPRES$TIPO, R.SPRES$COD_ESTRATEGICO, R.SPRES$DESCRIPCION, PR.SPPRO$PRESUPUESTO, PR.SPPRO$ID_PRODUCTO, PR.SPPRO$DESCRIPCION, SUB.SPPSUB$SNIP, SUB.SPPSUB$DESCRIPCION, SUB.SPPSUB$ID_MEDIDA, SPPMFS$ID_POA, SPPMFS$ANIO, PR.SPPRO$ID_RESULTADO, PR.SPPRO$RESULTADO2 ))
                            ORDER BY SPRES$TIPO ASC";

            estado = dao.consulta(sql);
            if (estado == 1)
            {
                tabla = dao.tabla;
                gvVerSubproductos.DataSource = tabla;
                gvVerSubproductos.DataBind();
                gvVerSubproductos.ExpandAll();

            }

        }

        protected void gvReprogramaciónes_HtmlRowPrepared(object sender, DevExpress.Web.ASPxGridViewTableRowEventArgs e)
        {
            string valor = "";
            valor = Convert.ToString(e.GetValue("SPPRE$APROBADO"));
            if (valor == "SI")
            {
                e.Row.ForeColor = System.Drawing.Color.Blue;
                //e.Row.BackColor = System.Drawing.Color.MediumSeaGreen;
            }
                
        }

        protected void btnFinancieraProd_Click(object sender, EventArgs e)
        {
            int nivel;
            if (gvProductos.FocusedRowIndex != -1)
            {
                nivel = gvProductos.GetRowLevel(gvProductos.FocusedRowIndex);
                if (nivel == 2)
                {
                    txtdiferencia.Text = "";
                    operacion.SelectedIndex = -1;
                    txtNuevameta.Text = "";
                    txtdiferencia.Enabled = true;
                    operacion.Enabled = true;

                    lblProducto.Text = gvProductos.GetRowValues(gvProductos.FocusedRowIndex, "SPPRO$DESCRIPCION").ToString();
                    lblMedidaProd.Text = gvProductos.GetRowValues(gvProductos.FocusedRowIndex, "SNCGUM$NOMBRE").ToString();
                    mInicial.Text = gvProductos.GetRowValues(gvProductos.FocusedRowIndex, "METAFINANCIERA").ToString();
                    mVigente.Text = gvProductos.GetRowValues(gvProductos.FocusedRowIndex, "METAFINANCIERA").ToString();
                    if (gvProductos.GetRowValues(gvProductos.FocusedRowIndex, "CANTIDADFINANCIERA") != DBNull.Value)
                    {
                        txtdiferencia.Text = gvProductos.GetRowValues(gvProductos.FocusedRowIndex, "CANTIDADFINANCIERA").ToString();
                    }

                    //if (gvProductos.GetRowValues(gvProductos.FocusedRowIndex, "OPERACION") != DBNull.Value)
                    //{
                    //    operacion.SelectedIndex  = Convert.ToInt32(gvProductos.GetRowValues(gvProductos.FocusedRowIndex, "OPERACION").ToString());
                    //}

                    if (gvProductos.GetRowValues(gvProductos.FocusedRowIndex, "NUEVAMETAFINANCIERA") != DBNull.Value)
                    {
                        txtNuevameta.Text = gvProductos.GetRowValues(gvProductos.FocusedRowIndex, "NUEVAMETAFINANCIERA").ToString();
                    }
                    if (gvProductos.GetRowValues(gvProductos.FocusedRowIndex, "NIDMFIN") == DBNull.Value && gvProductos.GetRowValues(gvProductos.FocusedRowIndex, "REPROFINANCIERA") == DBNull.Value)
                    {
                        Session["operaProd"] = 10;
                        popMFproducto.HeaderText = "Nueva programación de meta financiera de producto";
                        btnmeta.Text = "Grabar nueva meta";
                        btnmeta.CssClass = "btn btn-primary";
                    }
                    else
                    {
                        Session["operaProd"] = 11;
                        popMFproducto.HeaderText = "Editar programación de meta física de producto";
                        btnmeta.Text = "Editar reprogramación";
                        btnmeta.CssClass = "btn btn-success";
                    }


                    popMFproducto.ShowOnPageLoad = true;
                }
                else
                {
                    mensaje = "Seleccione una fila con un producto";
                    ScriptManager.RegisterStartupScript(this.UpdatePanel2, GetType(), "script", "Alerta('" + mensaje + " <br/>',2);", true);
                }
            }

            else
            {
                mensaje = "Seleccione una fila con un producto";
                ScriptManager.RegisterStartupScript(this.UpdatePanel2, GetType(), "script", "Alerta('" + mensaje + " <br/>',2);", true);
            }
        }

        protected void btnBorrarReproFin_Click(object sender, EventArgs e)
        {
            int nivel;
            if (gvProductos.FocusedRowIndex != -1)
            {
                nivel = gvProductos.GetRowLevel(gvProductos.FocusedRowIndex);
                if (nivel == 2)
                {
                    txtdiferencia.Text = "";
                    operacion.SelectedIndex = -1;
                    txtNuevameta.Text = "";


                    lblProducto.Text = gvProductos.GetRowValues(gvProductos.FocusedRowIndex, "SPPRO$DESCRIPCION").ToString();
                    lblMedidaProd.Text = gvProductos.GetRowValues(gvProductos.FocusedRowIndex, "SNCGUM$NOMBRE").ToString();
                    mInicial.Text = gvProductos.GetRowValues(gvProductos.FocusedRowIndex, "METAFINANCIERA").ToString();
                    mVigente.Text = gvProductos.GetRowValues(gvProductos.FocusedRowIndex, "METAFINANCIERA").ToString();
                    if (gvProductos.GetRowValues(gvProductos.FocusedRowIndex, "CANTIDADFINANCIERA") != DBNull.Value)
                    {
                        txtdiferencia.Text = gvProductos.GetRowValues(gvProductos.FocusedRowIndex, "CANTIDADFINANCIERA").ToString();
                    }

                    if (gvProductos.GetRowValues(gvProductos.FocusedRowIndex, "OPERAFINANCIERA") != DBNull.Value)
                    {
                        operacion.SelectedIndex = Convert.ToInt32(gvProductos.GetRowValues(gvProductos.FocusedRowIndex, "OPERAFINANCIERA").ToString());
                    }

                    if (gvProductos.GetRowValues(gvProductos.FocusedRowIndex, "REPROFINANCIERA") != DBNull.Value)
                    {
                        txtNuevameta.Text = gvProductos.GetRowValues(gvProductos.FocusedRowIndex, "NUEVAMETAFINANCIERA").ToString();
                    }
                    if (gvProductos.GetRowValues(gvProductos.FocusedRowIndex, "NIDMFIN") == DBNull.Value && gvProductos.GetRowValues(gvProductos.FocusedRowIndex, "REPROFINANCIERA") == DBNull.Value)
                    {
                        Session["operaProd"] = 0;
                        popMFproducto.HeaderText = "Nueva programación de meta financiera de producto";
                        btnmeta.Text = "Grabar nueva meta";
                        btnmeta.CssClass = "btn btn-primary";
                    }
                    else
                    {
                        Session["operaProd"] = 12;

                        txtdiferencia.Enabled = false;
                        operacion.Enabled = false;
                        popMFproducto.HeaderText = "Borrar programación de meta financiera de producto";
                        btnmeta.Text = "Borrar reprogramación";
                        btnmeta.CssClass = "btn btn-danger";
                    }


                    popMFproducto.ShowOnPageLoad = true;
                }
                else
                {
                    mensaje = "Seleccione una fila con un producto";
                    ScriptManager.RegisterStartupScript(this.UpdatePanel2, GetType(), "script", "Alerta('" + mensaje + " <br/>',2);", true);
                }
            }

            else
            {
                mensaje = "Seleccione una fila con un producto";
                ScriptManager.RegisterStartupScript(this.UpdatePanel2, GetType(), "script", "Alerta('" + mensaje + " <br/>',2);", true);
            }
        }

        protected DataTable noAProbados(int pom, int poa)
        {
            DataTable tablas = new DataTable();
            sql = "SELECT R.SPPRE$APROBADO FROM SCHE$SIPLAN20.SP20$REPROGRAMACION R WHERE R.SPPRE$POM = " + pom + " AND R.SPPRE$POA = " + poa + " AND R.SPPRE$RESTRICTIVA = 'N' AND R.SPPRE$APROBADO = 0 ORDER BY R.SPPRE$ID_REPRO ASC";
            estado = dao.consulta(sql);
            if (estado == 1)
            {
                tablas = dao.tabla;
               
            }

            return tablas;
        }


        protected void cargaPeriodos()
        {
            sql = "SELECT SPP$ID_PERIODO, SPP$INICIO||'-'||SPP$FINAL PERIODO FROM SCHE$SIPLAN20.SP20$PERIODO WHERE SPP$RESTRICTIVA = 'N'  ORDER BY SPP$VIGENTE DESC,  SPP$ORDEN ASC";
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

        protected bool consultaNoAprobados(int pom, int poa)
        {
            bool valida = false;
            
            sql = @"SELECT R.SPPRE$ID_REPRO
                           ,R.SPPRE$FECHA_RESOLUCION 
                            FROM SCHE$SIPLAN20.SP20$REPROGRAMACION R 
                            WHERE R.SPPRE$POM = "+pom+@" 
                            AND R.SPPRE$POA = "+poa+@" 
                            AND R.SPPRE$RESTRICTIVA = 'N' 
                            AND R.SPPRE$APROBADO = 0 
                            ORDER BY R.SPPRE$FECHA_RESOLUCION DESC";
            estado = dao.consulta(sql);
            if (estado == 1)
                tabla = dao.tabla;
            if (tabla.Rows.Count > 0)
                valida = true;
            else
                valida = false;
            return valida;
        }

        protected int ultima_repro(int pom, int poa)
        {
            int ultima = -1;

            sql = "SELECT R.SPPRE$ID_REPRO, R.SPPRE$FECHA_RESOLUCION FROM SCHE$SIPLAN20.SP20$REPROGRAMACION R WHERE R.SPPRE$POM = "+pom+@" AND R.SPPRE$POA = "+poa+ @" AND R.SPPRE$RESTRICTIVA = 'N' AND R.SPPRE$APROBADO = 1 ORDER BY R.SPPRE$FECHA_RESOLUCION DESC, R.SPPRE$ID_REPRO DESC";
            estado = dao.consulta(sql);
            if (estado == 1)
            {
                tabla = dao.tabla;
                if (tabla.Rows.Count > 0)
                    ultima = Convert.ToInt32(tabla.Rows[0]["SPPRE$ID_REPRO"]);
                else
                    ultima = -1;
            }
                
            else
                ultima = -1;
            return ultima;
        }

        protected bool desaprueba_prod_sub(int reprogramacion)
        {
            bool resultado1 = false;
            bool resultado2 = false;
            bool resultado = false;
            DataTable productos = new DataTable();
            DataTable subproductos = new DataTable();
            DataTable max = new DataTable();
            int id_progra = -1;

            sql = @"SELECT RP.SPPRP$PROGRAPRODCUTO
                           ,RP.SPPRP$TIPO 
                           ,MFP.SPPMFS$ID_PRODUCTO
                           ,MFP.SPPMF$TIPO
                           ,MFP.SPPMFS$ANIO
                           FROM SCHE$SIPLAN20.SP20$REPROGRAPRODUCTO RP 
                           INNER JOIN SCHE$SIPLAN20.SP20$PROGRAMACION_MFPRODUCTO MFP ON RP.SPPRP$PROGRAPRODCUTO = MFP.SPPMFS$ID_PROGRAMACION_FISICA AND RP.SPPRP$RESTRICTIVA = 'N' AND MFP.SPPMFS$RESTRICTIVA = 'N'
                           WHERE RP.SPPRP$REPROGRAMACION = "+reprogramacion+@" AND RP.SPPRP$RESTRICTIVA = 'N'";
            estado = dao.consulta(sql);
            if (estado == 1)
            {
                productos = dao.tabla;
                if (productos.Rows.Count > 0)
                {
                    for (int i = 0; i < productos.Rows.Count; i++)
                    {
                        sql = @"UPDATE SCHE$SIPLAN20.SP20$PROGRAMACION_MFPRODUCTO SET SPPMFS$VIGENTE = 1, SPPMFS$FECHA_UPDATE = 'UPDATE = '||TO_CHAR(SYSDATE,'DD/MM/YYYY HH24:MI')||' '||'" + Session["USUARIO"].ToString() + "' WHERE SPPMFS$ID_PRODUCTO = " + productos.Rows[i]["SPPMFS$ID_PRODUCTO"] +@" 
                                AND SPPMF$TIPO = "+productos.Rows[i]["SPPMF$TIPO"] +@" AND SPPMFS$RESTRICTIVA = 'N' AND SPPMFS$ANIO = "+productos.Rows[i]["SPPMFS$ANIO"];
                        estado = dao.comando(sql);
                        if (estado == 1)
                            resultado1 = true;
                        else
                        {
                            resultado1 = false;
                            break;
                        }
                    }


                    for (int i = 0; i < productos.Rows.Count; i++)
                    {
                        sql = @"SELECT MAX(SPPMFS$ID_PROGRAMACION_FISICA) MAXI FROM SCHE$SIPLAN20.SP20$PROGRAMACION_MFPRODUCTO WHERE SPPMF$TIPO = " + productos.Rows[i]["SPPMF$TIPO"] + " AND SPPMFS$RESTRICTIVA = 'N' AND SPPMFS$ID_PRODUCTO = " + productos.Rows[i]["SPPMFS$ID_PRODUCTO"] + " AND SPPMFS$ANIO = " + productos.Rows[i]["SPPMFS$ANIO"] + " AND SPPMFS$ID_PROGRAMACION_FISICA NOT IN("+ productos.Rows[i]["SPPRP$PROGRAPRODCUTO"] + ")";
                        estado = dao.consulta(sql);
                        if (estado == 1)
                        {
                            max = dao.tabla;
                            if (max.Rows.Count > 0)
                            {
                                id_progra = Convert.ToInt32(max.Rows[0]["MAXI"]);
                                sql = "UPDATE SCHE$SIPLAN20.SP20$PROGRAMACION_MFPRODUCTO SET SPPMFS$VIGENTE = 0, SPPMFS$FECHA_UPDATE = 'UPDATE = '||TO_CHAR(SYSDATE,'DD/MM/YYYY HH24:MI')||' '||'" + Session["USUARIO"].ToString() + "' WHERE SPPMFS$ID_PROGRAMACION_FISICA = " + id_progra+@" AND SPPMFS$RESTRICTIVA = 'N' AND SPPMFS$ID_PRODUCTO = "+ productos.Rows[i]["SPPMFS$ID_PRODUCTO"];
                                estado = dao.comando(sql);
                                if (estado == 1)
                                {
                                    resultado1 = true;
                                    max.Clear();
                                }
                                else
                                {
                                    resultado1 = false;
                                    max.Clear();
                                    break;
                                }
                            }
                            else
                            {
                                resultado1 = false;
                                break;
                            }
                                
                        }
                        else
                        {
                            resultado1 = false;
                            break;
                        }
                    }


                }
                else
                    resultado1 = true;
            }
            else
                resultado1 = false;


            sql = @"SELECT RS.SPPRS$PROGRASUB
                           ,RS.SPPRS$TIPO 
                           ,MFS.SPPMFS$ID_SUBPRODUCTO
                           ,MFS.SPPMFS$TIPO_PROGRAMACION
                           ,MFS.SPPMFS$ANIO
                           FROM SCHE$SIPLAN20.SP20$REPROGRASUB RS 
                           INNER JOIN SCHE$SIPLAN20.SP20$PROGRAMETA_FI_FIN_SUB MFS ON RS.SPPRS$PROGRASUB = MFS.SPPMFS$ID_PROGRAMACION_FISFIN AND RS.SPPRS$RESTRICTIVA = 'N' AND MFS.SPPMFS$RESTRICTIVA = 'N'
                           WHERE RS.SPPRS$REPROGRAMACION = "+reprogramacion+@" AND RS.SPPRS$RESTRICTIVA = 'N'";
            estado = dao.consulta(sql);
            if (estado == 1)
            {
                subproductos = dao.tabla;
                if (subproductos.Rows.Count > 0)
                {
                    for (int i = 0; i < subproductos.Rows.Count; i++)
                    {
                        sql = @"UPDATE SCHE$SIPLAN20.SP20$PROGRAMETA_FI_FIN_SUB SET SPPMFS$VIGENTE = 1, SPPMFS$FECHA_UPDATE = 'UPDATE = '||TO_CHAR(SYSDATE,'DD/MM/YYYY HH24:MI')||' '||'" + Session["USUARIO"].ToString() + "' WHERE SPPMFS$ID_SUBPRODUCTO = " + subproductos.Rows[i]["SPPMFS$ID_SUBPRODUCTO"] +" AND SPPMFS$TIPO_PROGRAMACION = "+ subproductos.Rows[i]["SPPMFS$TIPO_PROGRAMACION"] + " AND SPPMFS$RESTRICTIVA = 'N' AND SPPMFS$ANIO = "+ subproductos.Rows[i]["SPPMFS$ANIO"];
                        estado = dao.comando(sql);
                        if (estado == 1)
                            resultado2 = true;
                        else
                        {
                            resultado2 = false;
                            break;
                        }
                    }


                    for (int i = 0; i < subproductos.Rows.Count; i++)
                    {
                        sql = @"SELECT MAX(SPPMFS$ID_PROGRAMACION_FISFIN) MAXI FROM SCHE$SIPLAN20.SP20$PROGRAMETA_FI_FIN_SUB WHERE SPPMFS$TIPO_PROGRAMACION = "+subproductos.Rows[i]["SPPMFS$TIPO_PROGRAMACION"] +" AND SPPMFS$RESTRICTIVA = 'N' AND SPPMFS$ID_SUBPRODUCTO = "+ subproductos.Rows[i]["SPPMFS$ID_SUBPRODUCTO"] + " AND  SPPMFS$ANIO = "+ subproductos.Rows[i]["SPPMFS$ANIO"] + " AND SPPMFS$ID_PROGRAMACION_FISFIN NOT IN ("+ subproductos.Rows[i]["SPPRS$PROGRASUB"] + ")";
                        estado = dao.consulta(sql);
                        if (estado == 1)
                        {
                            max = dao.tabla;
                            if (max.Rows.Count > 0)
                            {
                                id_progra = Convert.ToInt32(max.Rows[0]["MAXI"]);
                                sql = "UPDATE SCHE$SIPLAN20.SP20$PROGRAMETA_FI_FIN_SUB SET SPPMFS$VIGENTE = 0, SPPMFS$FECHA_UPDATE = 'UPDATE = '||TO_CHAR(SYSDATE,'DD/MM/YYYY HH24:MI')||' '||'" + Session["USUARIO"].ToString() + "' WHERE SPPMFS$ID_PROGRAMACION_FISFIN = " + id_progra + " AND SPPMFS$RESTRICTIVA = 'N' AND SPPMFS$ID_SUBPRODUCTO = "+subproductos.Rows[i]["SPPMFS$ID_SUBPRODUCTO"];
                                estado = dao.comando(sql);
                                if (estado == 1)
                                {
                                    resultado2 = true;
                                    max.Clear();
                                }
                                else
                                {
                                    resultado2 = false;
                                    max.Clear();
                                    break;
                                }
                            }
                            else
                            {
                                resultado2 = false;
                                break;
                            }

                        }
                        else
                        {
                            resultado2 = false;
                            break;
                        }
                    }


                }
                else
                    resultado2 = true;
            }
            else
                resultado2 = false;




            if (resultado1 == true && resultado2 == true)
                resultado = true;
            else
                resultado = false;

            return resultado;

        }

        protected bool borra_reprog(int reprogramacion)
        {
            bool resultado = false;
            bool resultado1 = false;
            bool resultado2 = false;
            DataTable producto = new DataTable();
            DataTable subproducto = new DataTable();
            sql = @"SELECT RP.SPPRP$PROGRAPRODCUTO
                           ,RP.SPPRP$TIPO
                           ,MFP.SPPMFS$ID_PRODUCTO
                           ,MFP.SPPMF$TIPO
                           ,MFP.SPPMFS$ANIO
                           FROM SCHE$SIPLAN20.SP20$REPROGRAPRODUCTO RP
                           INNER JOIN SCHE$SIPLAN20.SP20$PROGRAMACION_MFPRODUCTO MFP ON RP.SPPRP$PROGRAPRODCUTO = MFP.SPPMFS$ID_PROGRAMACION_FISICA AND RP.SPPRP$RESTRICTIVA = 'N' AND MFP.SPPMFS$RESTRICTIVA = 'N'
                           WHERE RP.SPPRP$REPROGRAMACION = "+reprogramacion+@" AND RP.SPPRP$RESTRICTIVA = 'N'";
            estado = dao.consulta(sql);

            if (estado == 1)
            {
                producto = dao.tabla;
                if (producto.Rows.Count > 0)
                {
                    for (int i = 0; i < producto.Rows.Count; i++)
                    {
                        sql = @"UPDATE SCHE$SIPLAN20.SP20$REPROGRAPRODUCTO SET SPPRP$RESTRICTIVA = 'S', SPPRP$FECHA_DELETE = 'DELETE = '||TO_CHAR(SYSDATE,'DD/MM/YYYY HH24:MI')||' '||'" + Session["USUARIO"].ToString() + "' WHERE SPPRP$PROGRAPRODCUTO = "+producto.Rows[i]["SPPRP$PROGRAPRODCUTO"];
                        estado = dao.comando(sql);
                        if (estado == 1)
                            resultado1 = true;
                        else
                        {
                            resultado1 = false;
                            break;
                        }
                            
                    }


                    for (int i = 0; i < producto.Rows.Count; i++)
                    {
                        sql = @"UPDATE SCHE$SIPLAN20.SP20$PROGRAMACION_MFPRODUCTO SET SPPMFS$RESTRICTIVA = 'S', SPPMFS$FECHA_DELETE = 'DELETE = '||TO_CHAR(SYSDATE,'DD/MM/YYYY HH24:MI')||' '||'" + Session["USUARIO"].ToString() + "' WHERE SPPMFS$ID_PROGRAMACION_FISICA = " + producto.Rows[i]["SPPRP$PROGRAPRODCUTO"];
                        estado = dao.comando(sql);
                        if (estado == 1)
                            resultado1 = true;
                        else
                        {
                            resultado1 = false;
                            break;
                        }
                            
                    }
                }
                else
                    resultado1 = true;
            }
            else
                resultado1 = false;


            sql = @"SELECT RS.SPPRS$PROGRASUB
                           ,RS.SPPRS$TIPO 
                           ,MFS.SPPMFS$ID_SUBPRODUCTO
                           ,MFS.SPPMFS$TIPO_PROGRAMACION
                           ,MFS.SPPMFS$ANIO
                           FROM SCHE$SIPLAN20.SP20$REPROGRASUB RS 
                           INNER JOIN SCHE$SIPLAN20.SP20$PROGRAMETA_FI_FIN_SUB MFS ON RS.SPPRS$PROGRASUB = MFS.SPPMFS$ID_PROGRAMACION_FISFIN AND RS.SPPRS$RESTRICTIVA = 'N' AND MFS.SPPMFS$RESTRICTIVA = 'N'
                           WHERE RS.SPPRS$REPROGRAMACION = " + reprogramacion + @" AND RS.SPPRS$RESTRICTIVA = 'N'";
            estado = dao.consulta(sql);
            if (estado == 1)
            {
                subproducto = dao.tabla;
                if (subproducto.Rows.Count > 0)
                {
                    for (int i = 0; i < subproducto.Rows.Count; i++)
                    {
                        sql = @"UPDATE SCHE$SIPLAN20.SP20$REPROGRASUB SET SPPRS$RESTRICTIVA = 'S', SPPRS$FECHA_DELETE = 'DELETE = '||TO_CHAR(SYSDATE,'DD/MM/YYYY HH24:MI')||' '||'" + Session["USUARIO"].ToString() + "' WHERE SPPRS$PROGRASUB = " + subproducto.Rows[i]["SPPRS$PROGRASUB"];
                        estado = dao.comando(sql);
                        if (estado == 1)
                            resultado2 = true;
                        else
                        {
                            resultado2 = false;
                            break;
                        }

                    }


                    for (int i = 0; i < subproducto.Rows.Count; i++)
                    {
                        sql = @"UPDATE SCHE$SIPLAN20.SP20$PROGRAMETA_FI_FIN_SUB SET SPPMFS$RESTRICTIVA = 'S', SPPMFS$FECHA_DELETE = 'DELETE = '||TO_CHAR(SYSDATE,'DD/MM/YYYY HH24:MI')||' '||'" + Session["USUARIO"].ToString() + "' WHERE SPPMFS$ID_PROGRAMACION_FISFIN = " + subproducto.Rows[i]["SPPRS$PROGRASUB"];
                        estado = dao.comando(sql);
                        if (estado == 1)
                            resultado2 = true;
                        else
                        {
                            resultado2 = false;
                            break;
                        }

                    }
                }
                else
                    resultado2 = true;
            }
            else
                resultado2 = false;

            if (resultado1 == true && resultado2 == true)
                resultado = true;
            else
                resultado = false;

            return resultado;
        }

        



    }
}