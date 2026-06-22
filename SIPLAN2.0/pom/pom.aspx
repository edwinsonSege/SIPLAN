<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="pom.aspx.cs" Inherits="SIPLAN2._0.pom.pom" %>

<%@ Register Assembly="DevExpress.Web.v17.2, Version=17.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxTreeList.v17.2, Version=17.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxTreeList" TagPrefix="dy" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    
       <style>
           .frame {
        border: 2px solid #ccc; /* Agrega un borde al frame */
        padding: 10px; /* Agrega espacio interno al frame */
        width: 500px; /* Establece el ancho del frame */
        margin: 5px; /* Establece el margen exterior del frame */
    }

     
       </style>
   
    <script>
             

       $(document).ready(function(){
  $('[data-toggle="tooltip"]').tooltip();   
});
         function abrirVentana(url) {
            window.open(url, '_blank');
        };
    
        function Alerta(mensaje, tipo) {          
            alertify.set('notifier', 'position', 'top-right'); 
            if (tipo == 1)
                alertify.success(mensaje);
            if (tipo == 2)
                alertify.error(mensaje);       
           
            
   
        };  

        function cambioResulta(s, e) {
            var valor = s.GetValue();

            if (valor == 0) {
                var div = document.getElementById('MainContent_Institucionales');
                div.style.display = 'none';

                var div1 = document.getElementById('MainContent_Estrategicos');
                div1.style.display = 'block';
            }
            else
            {
                var div = document.getElementById('MainContent_Institucionales');
                div.style.display = 'block';

                var div1 = document.getElementById('MainContent_Estrategicos');
                div1.style.display = 'none';

            }

           
            
        }


        function cambioPrograma(s, e)
        {
            var valor = s.GetValue();

                 if (valor == 0) {
                     var div = document.getElementById('MainContent_PanelPrograma');
                     div.style.display = 'none';
                 }
                 else if(valor = 1)
                 {
                     var div = document.getElementById('MainContent_PanelPrograma');
                     div.style.display = 'block';
                 }

            

           
            
        }


function cambioPGG(s, e)
        {
             var valor = s.GetValue();
            if (valor == 0) {
                     var div = document.getElementById('MainContent_panPGGProductos');
                     div.style.display = 'none';
                 }
                 else if(valor = 1)
                 {
                     var div = document.getElementById('MainContent_panPGGProductos');
                     div.style.display = 'block';
                 }
        }


        function cambioSubprograma(s, e)
        {
             var valor = s.GetValue();
            if (valor == 0) {
                     var div = document.getElementById('MainContent_subProgramaPresupuestario');
                     div.style.display = 'none';
                 }
                 else if(valor = 1)
                 {
                     var div = document.getElementById('MainContent_subProgramaPresupuestario');
                     div.style.display = 'block';
                 }
        }

        function cambioInstrumento(s, e)
        {
             var valor = s.GetValue();
            if (valor == 0) {
                     var div = document.getElementById('MainContent_popInstrumento_panInstrumento');
                     div.style.display = 'none';
                 }
                 else if(valor = 1)
                 {
                     var div = document.getElementById('MainContent_popInstrumento_panInstrumento');
                     div.style.display = 'block';
                 }
        }

        function cambioSuEstrategico(s, e)
        {
             var valor = s.GetValue();
            if (valor == 0) {
                     var div = document.getElementById('MainContent_panSubEstrategico');
                     div.style.display = 'none';
                 }
                 else if(valor = 1)
                 {
                     var div = document.getElementById('MainContent_panSubEstrategico');
                     div.style.display = 'block';
                 }
        }

        function cambioAccion(s, e)
        {
             var valor = s.GetValue();
            if (valor == 0) {
                     var div = document.getElementById('MainContent_AccionEstrategica');
                     div.style.display = 'none';
                 }
                 else if(valor = 1)
                 {
                     var div = document.getElementById('MainContent_AccionEstrategica');
                     div.style.display = 'block';
                 }
        }


        /*
         function cambioSNIP(s, e)
        {
             var valor = s.GetValue();
            if (valor == 0) {
                     var div = document.getElementById('MainContent_PanSNIP');
                     div.style.display = 'none';
                    div = document.getElementById('MainContent_cbMunosPriorizados_VI');
                    console.log(div);
                    div.disabled = false;
                    
                 }
                 else if(valor = 1)
                 {
                     var div = document.getElementById('MainContent_PanSNIP');
                     div.style.display = 'block';
                     div = document.getElementById('MainContent_cbMunosPriorizados_VI');
                     console.log(div);
                     div.disabled = true;
                 }
        }
*/
         /*function cambioPriorizados(s, e)
        {
             var valor = s.GetValue();
            if (valor == 0) {
                     var div = document.getElementById('MainContent_munoPriorizados');
                     div.style.display = 'none';
                 }
                 else if(valor = 1)
                 {
                     var div = document.getElementById('MainContent_munoPriorizados');
                     div.style.display = 'block';
                 }
        }
*/

        
         function cambioProducto(s, e)
        {
            var valor = s.GetValue();   
             console.log(valor);
             if (valor == 1)
             {
                 if(s.cpShow)
                popupCommon.Show();
            }
           
        }


        function restringe(e) {
        var k;
        document.all ? k = e.keyCode : k = e.which;
        return ((k > 64 && k < 91) || (k > 96 && k < 123) || k == 8 || k == 32 || (k >= 48 && k <= 57));
    }
        function soloNumeros(e)
{
	var key = window.Event ? e.which : e.keyCode
	return ((key >= 48 && key <= 57) || (key==8) || (key!=190))
}
    </script>
   
  

        <h3 style="color:blue">Configuración Proceso de Planificación <asp:Label ID="Instos" runat="server"></asp:Label></h3>

    
    <div style="float:right">
       
        <div style="align-content:flex-start">
            <asp:Button ID="Button1" runat="server" Text="Matrices de vinculación"   CssClass="btn" Style="color:white; background-color:#198A36" ToolTip="Descargue documento con las matrices de vinculación de metas PGG" OnClick="Button1_Click"/>
            <asp:Button ID="btnEncabezado" runat="server" Text="Institución" OnClick="btnEncabezado_Click"  CssClass="btn btn-primary" ToolTip="Instituciones"/>       
            <asp:Button ID="btnDocumento" runat="server" Text="Documentos" OnClick="btnDocumento_Click"  CssClass="btn" style="color:white; background-color:#848484" ToolTip="Subir documentos con Instrumentos de planificación"/>
            <asp:Button ID="btnResultados" runat="server" Text="Resultados"   CssClass="btn btn-success" OnClick="btnResultados_Click" ToolTip="Acciones estratégicas PGG y Resultados institucionales"/>            
            <asp:Button ID="btnProgra" runat="server" Text="Programas"   CssClass="btn btn-warning"  OnClick="btnProgra_Click" ToolTip="Programas presuestarios"/>
             <asp:Button ID="btnProductos" runat="server" Text="Productos"   CssClass="btn btn-primary"  OnClick="btnProductos_Click" ToolTip="Productos vinculados a Resultado Institucional"/>
            
            <asp:Button ID="btnPOA" runat="server" Text="Programación"   CssClass="btn"  OnClick="btnPOA_Click"  ToolTip="Programas metas físicas y financieras" style="color:white;background-color:#2E9AFE" />
        </div>
               
    </div>

    
       <hr>
    <asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex="0">
         
        <asp:View ID="vistaPOM" runat="server">
             <div  style ="overflow:auto; width:100%; height:90%; padding: 1px 10px 1px 10px;   ">
                <h3 style="color:#2d572c">Instituciones</h3>
                  
                 
  <div class="btn-group" style="display:flex; justify-content:flex-start;">                                  
                                              
                                     <asp:Button ID="btnNuevoPom" runat="server" Text="Nuevo" OnClick="btnNuevoPom_Click"  CssClass="btn btn-primary" ToolTip="Configurar nuevo POM"/>                             
    <%--  <asp:UpdatePanel ID="UpdatePanel8" runat="server">     
          <ContentTemplate>--%>
              <asp:Button ID="btnProgramas" runat="server" Text="Programación"  OnClick="btnProgramas_Click"  CssClass="btn btn-warning"/>
      <asp:Button ID="btnEjecución" runat="server" Text="Ir a ejecución" OnClick="btnEjecución_Click"  CssClass="btn btn-success" ToolTip="Ingresar ejecución productos/subproductos"/>
      <asp:Button ID="btnRegreso" runat="server" Text="Ir a pagina principal"  CssClass="btn " ToolTip="Regresar a pagina de principal para acceder a los otros modulos del sistema"   OnClick="btnRegreso_Click"  style="color:white;background-color:#F28B08" />
       <asp:Button ID="btnAbrir" runat="server" Text="Abrir programación"  CssClass="btn " ToolTip="Abrir programación de POM envidado para actualizaciones/modificaciones"   style="color:white;background-color:black" OnClick="btnAbrir_Click"/>
              <%--</ContentTemplate> 
              </asp:UpdatePanel>  --%>                  
                                                     </div>
                 <br />
                 <h5 style="color:red"><b>"Programación no enviada"</b>, es información que no ha sido enviada y <b>pueden realizarse actualizaciones de metas físicas financieras</b> en el modulo de programación, <b>"Programación enviada"</b>, es información enviada donde <b>no es posible realizar</b> actualizaciones de metas físicas y financieras en el modulo de programación <asp:Label ID="Label1" runat="server"></asp:Label></h5>
            <dx:ASPxGridView ID="gvPOMInsto" runat="server" KeyFieldName="SPPO$ID_POM"   Theme="Office2010Blue" Width="100%" AutoGenerateColumns="false" SettingsBehavior-AllowFocusedRow ="true" SettingsBehavior-ConfirmDelete="true"  OnRowDeleting="gvPOMInsto_RowDeleting" EnableCallBacks="false" OnCommandButtonInitialize="gvPOMInsto_CommandButtonInitialize"  OnHtmlRowPrepared="gvPOMInsto_HtmlRowPrepared">
                                           
                             <Settings ShowFooter="True"/>                           
                            <SettingsLoadingPanel Text="Cargando&amp;hellip;" />

          
             <ClientSideEvents EndCallback="function(s,e)
             {
                if(s.cpError)
                {
                    //alert(s.cpError);
                    alertify.success(s.cpError);
                    delete s.cpError;
                }  
                 

             }           
             " />
             <SettingsContextMenu Enabled="True">
             </SettingsContextMenu>
             <SettingsPager AlwaysShowPager="True" PageSize="200" ShowSeparators="True">
                 <PageSizeItemSettings Visible="True">
                 </PageSizeItemSettings>
             </SettingsPager>
             <Settings ShowFilterRow="True" ShowGroupPanel="True" />
             <SettingsBehavior AllowFocusedRow="True" ConfirmDelete="True" />
             <SettingsCommandButton DeleteButton-ButtonType="Button" DeleteButton-Text="Quitar">
                 <DeleteButton ButtonType="Button">
                 </DeleteButton>
             </SettingsCommandButton>
             <SettingsDataSecurity AllowDelete="true" AllowEdit="true" AllowInsert="False" />
             <SettingsLoadingPanel Mode="ShowOnStatusBar" />
             <SettingsText CommandApplyFilter="Aplicar filtro" CommandApplySearchPanelFilter="asdasfasdfasdf" ContextMenuHideColumn="Ocultar Columna" ContextMenuShowCustomizationWindow="Mostrar Columnas" ContextMenuShowFilterRow="Fila de Filtros" ContextMenuShowFilterRowMenu="Seleccione Tipo de Filtro" ContextMenuShowSearchPanel="Buscar" CustomizationWindowCaption="Columnas Ocultas" GroupPanel="Arrastre las columnas que desee agrupar" />
                <Columns>
                     <dx:GridViewDataTextColumn FieldName="SPPO$ID_PERIODO"  Name="SPPO$ID_PERIODO" ReadOnly="true" Visible="false" VisibleIndex="0" Width="0">
                     <HeaderStyle Wrap="True" />
                     <Settings />
                 </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="SPPO$ID_INSTITUCION"  Name="SPPO$ID_INSTITUCION" ReadOnly="true" Visible="false" VisibleIndex="1" Width="0">
                     <HeaderStyle Wrap="True" />
                     <Settings />
                 </dx:GridViewDataTextColumn>

                    <dx:GridViewDataTextColumn FieldName="SPPO$ID_POM"  Name="SPPO$ID_POM" ReadOnly="true" Visible="false" VisibleIndex="2" Width="0">
                     <HeaderStyle Wrap="True" />
                     <Settings />
                 </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="NOMBRE" Caption="Institución"  Name="NOMBRE" ReadOnly="true" Visible="true" VisibleIndex="3" Width="18%">
                     <HeaderStyle Wrap="True" />
                         <Settings AutoFilterCondition="Contains" />
                     <Settings />
                 </dx:GridViewDataTextColumn>
                     <dx:GridViewBandColumn Caption="Periodo de vigencia" Name="vigencia" Visible="True" VisibleIndex="4" >
                          <Columns>
                              <dx:GridViewDataTextColumn Caption="Inicio" FieldName="SPPO$INICIO" Name="SPPO$INICIO" ReadOnly="true" VisibleIndex="0" Width="18%" Visible="true">
                             <HeaderStyle Wrap="True" />
                             <FilterCellStyle HorizontalAlign="Center">
                             </FilterCellStyle>
                                   <Settings AutoFilterCondition="Contains" />
                         </dx:GridViewDataTextColumn>
                              <dx:GridViewDataTextColumn Caption="Final" FieldName="SPPO$FINAL" Name="SPPO$FINAL" ReadOnly="true" VisibleIndex="1" Width="18%" Visible="true">
                             <HeaderStyle Wrap="True" />
                             <FilterCellStyle HorizontalAlign="Center">
                             </FilterCellStyle>
                                   <Settings AutoFilterCondition="Contains" />
                         </dx:GridViewDataTextColumn>

                              </Columns>
                         </dx:GridViewBandColumn>

                  

                 

                    <dx:GridViewDataTextColumn FieldName="ESTADO" Caption="Estado programación"  Name="ESTADO" ReadOnly="true" Visible="true" VisibleIndex="5" Width="18%">
                     <HeaderStyle Wrap="True" />
                     <Settings />
                 </dx:GridViewDataTextColumn>

                    <dx:GridViewDataTextColumn FieldName="CODESTADO"   Name="CODESTADO" ReadOnly="true" Visible="false" VisibleIndex="6" Width="500">
                     <HeaderStyle Wrap="True" />
                     <Settings />
                 </dx:GridViewDataTextColumn>

                    <dx:GridViewDataTextColumn FieldName="SPP$ABIERTO"   Name="SPP$ABIERTO" ReadOnly="true" Visible="false" VisibleIndex="7" Width="18%">
                     <HeaderStyle Wrap="True" />
                     <Settings />
                 </dx:GridViewDataTextColumn>


                    <dx:GridViewCommandColumn ShowDeleteButton="true" ShowEditButton="false" ShowNewButtonInHeader="false" VisibleIndex="8" Width="10%" Caption="Acciones">
                     <HeaderStyle Wrap="True" />
                 </dx:GridViewCommandColumn>


                </Columns>
            </dx:ASPxGridView>
 </div>
            </asp:View>

        <asp:View ID="VistaNuevoPom" runat="server">
                <div  style ="overflow:auto; width:100%; height:90%; padding: 1px 10px 1px 10px;   ">
                      <h4 style="color:#2d572c">Nueva programación multianual</h4>
                    <h5>Campos marcados con <font color="red">*</font> son obligatorios </h5>
                    <hr>
                    <div class="form-group">
                           <label for="txtsubCod"aria-required="true"><font color="red">*</font>Periodos vigentes: </label>
                           <dx:ASPxComboBox ID="cbPeriodosPom" runat="server" ValueType="System.String" NullText="Seleccione periodo de POM" CssClass="form-control"></dx:ASPxComboBox>
                    </div> 
                    <div class="form-group">
                        <label for="txtsubCod"aria-required="true"><font color="red">*</font>Institución: </label>
                        <dx:ASPxComboBox ID="cbInstituiciones" runat="server" ValueType="System.String" NullText="Seleccione institución a configurar" CssClass="form-control"></dx:ASPxComboBox>
                    </div>
                    
                    <div class="form-group">
                        <asp:Button ID="btnPOM" runat="server" Text="Configura nuevo POM"  CssClass="btn btn-primary" OnClick="btnPOM_Click"/>
                         <asp:Button ID="btnCancelPOM" CssClass="btn btn-warning " runat="server" Text="Cancelar/Regresar" OnClick="btnCancelPOM_Click"/>
                    </div>
            </div>

        </asp:View>

        <asp:View ID="Resultado" runat="server">
                <div  style ="overflow:auto; width:100%; height:90%; padding: 1px 10px 1px 10px;   ">
                    <h3 style="color:#2d572c">Resultados multianuales, seleccione una opción</h3>                   
                    <div class ="form-group">
                        <dx:ASPxRadioButtonList ID="resultados" runat="server" ForeColor="Blue"
                                        ValueField="ID" TextField="Name2" RepeatColumns="2" RepeatLayout="Table"   Border-BorderStyle= "Double" Font-Size="Medium"  ClientInstanceName="viviendas" >
                                       <ClientSideEvents SelectedIndexchanged="cambioResulta" />
                           
                                         <CaptionSettings Position="Top" />
                                        <Items>
                                               
                                            <dx:ListEditItem Text="Resultados institucionales" Value="1" />
                                         
                                        </Items>
                                       
                                </dx:ASPxRadioButtonList>
                        </div>
                                <asp:Panel ID="Estrategicos" runat="server"  style="display:none">
                         <%-- <h4 style="color:#2d572c">Resultados estratégicos</h4>--%>
                        <%-- <hr>--%>
                          <h4 style="color:red">Programa de gobierno 2024-2028, presione el botón para vincular o desvincular la acción estratégica a la Programación Multianual</h4>
                          
                         <div class="form-group" style="display:flex; justify-content:flex-start;">                          
                         <asp:UpdatePanel ID="upPanelVincula" runat="server">
                            <ContentTemplate>
                                    <asp:Button ID="btnVincula" runat="server" Text="Vincular/Desvincular"  CssClass="btn btn-primary" OnClick="btnVincula_Click"/>                                      
                               </ContentTemplate>
                           </asp:UpdatePanel>
                             <asp:Button ID="btnCancelaRe" runat="server" Text="Cancelar/Regresar"  CssClass="btn btn-warning"  OnClick="btnCancelaR_Click1"/>
                          </div> 

                          <asp:UpdatePanel ID="upEstrategicos" runat="server">
                                <ContentTemplate> 
                                <dx:ASPxGridView ID="gvEstrategicos" runat="server" KeyFieldName="SPRES$ID_RESULTADO"   Theme="Office2010Blue"  AutoGenerateColumns="False" SettingsBehavior-AllowFocusedRow ="true" SettingsBehavior-ConfirmDelete="true" Width="100%" >
                                     <Settings ShowFooter="True"/>                           
                            <SettingsLoadingPanel Text="Cargando&amp;hellip;" />


                                <Settings ShowFooter="true" />
                                <ClientSideEvents EndCallback="function(s,e)
                                {
                                    if(s.cpError)
                                    {
                                        //alert(s.cpError);
                                        alertify.success(s.cpError);
                                        delete s.cpError;
                                    }             
                                }           
                              " />
             <SettingsContextMenu Enabled="True">
             </SettingsContextMenu>
             <SettingsPager AlwaysShowPager="True" PageSize="200" ShowSeparators="True">
                 <PageSizeItemSettings Visible="True">
                 </PageSizeItemSettings>
             </SettingsPager>
             <Settings ShowFilterRow="True" ShowGroupPanel="True" />
             <SettingsBehavior AllowFocusedRow="True" ConfirmDelete="True" />
             <SettingsCommandButton DeleteButton-Text="Quitar" DeleteButton-ButtonType="Button">
                                        <UpdateButton ButtonType="Button">
                                            <Styles>
                                                <Style CssClass="btn-warging">
                                                </Style>
                                            </Styles>
                                        </UpdateButton>
                                        <CancelButton ButtonType="Button">
                                            <Styles>
                                                <FocusRectStyle CssClass="btn-danger">
                                                </FocusRectStyle>
                                            </Styles>
                                        </CancelButton>
                                        <EditButton ButtonType="Button" Text="Editar">
                                        </EditButton>
                                        <DeleteButton ButtonType="Button">
                                           
                                        </DeleteButton>
                                    </SettingsCommandButton>
             <SettingsDataSecurity AllowDelete="true" AllowEdit="true" AllowInsert="False" />
             <SettingsLoadingPanel Mode="ShowOnStatusBar" />
             <SettingsText CommandApplyFilter="Aplicar filtro" CommandApplySearchPanelFilter="asdasfasdfasdf" ContextMenuHideColumn="Ocultar Columna" ContextMenuShowCustomizationWindow="Mostrar Columnas" ContextMenuShowFilterRow="Fila de Filtros" ContextMenuShowFilterRowMenu="Seleccione Tipo de Filtro" ContextMenuShowSearchPanel="Buscar" CustomizationWindowCaption="Columnas Ocultas" GroupPanel="Arrastre las columnas que desee agrupar" />
                             <Columns>
                                  <dx:GridViewDataTextColumn FieldName="SPRES$ID_RESULTADO"  Name="SPRES$ID_RESULTADO" ReadOnly="true" Visible="false" VisibleIndex="0" Width="0">
                                     <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn>
                                 <dx:GridViewDataTextColumn FieldName="SPRES$TIPO"  Name="SPRES$TIPO" ReadOnly="true" Visible="false" VisibleIndex="1" Width="0">
                                     <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn>
                                 <dx:GridViewDataTextColumn FieldName="SPRES$COD_ESTRATEGICO"  Name="SPRES$COD_ESTRATEGICO"   ReadOnly="false" Visible="false" VisibleIndex="2" Width="0">
                                     <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn>
                                 <dx:GridViewDataTextColumn FieldName="SPPRES$ID_RESULTADO"  Name="SPPRES$ID_RESULTADO" ReadOnly="true"  Visible="false" VisibleIndex="3" Width="0">
                                     <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn>
                                 
                                 <dx:GridViewDataTextColumn FieldName="EJE_ESTRATEGICO"  Name="EJE_ESTRATEGICO"  ReadOnly="true"  Caption="EJE ESTRATÉGICO" Visible="true" VisibleIndex="4" Width="100%" GroupIndex="0">
                                     <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn>
                                
                                  <dx:GridViewDataTextColumn FieldName="META_PRESIDENCIAL"  Name="META_PRESIDENCIAL"  ReadOnly="true"  Caption="META PRESIDENCIAL" Visible="true" VisibleIndex="5" Width="60%" GroupIndex="1">
                                     <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn>

                                 <dx:GridViewDataTextColumn FieldName="ACCION_ESTRATEGICA"  Name="ACCION_ESTRATEGICA"  ReadOnly="true"  Caption="ACCIÓN ESTRATÉGICA" Visible="true" VisibleIndex="6" Width="60%">
                                     <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn>

                                 <dx:GridViewDataTextColumn FieldName="PROGRAMA_PRESUPUESTARIO"  Name="PROGRAMA_PRESUPUESTARIO"  ReadOnly="true"  Caption="Numero de productos vinculados" Visible="true" VisibleIndex="7" Width="40%">
                                     <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn>

                                  
                                
                             </Columns>                                  
                              </dx:ASPxGridView>
                                    </ContentTemplate>
                        </asp:UpdatePanel>
                         <asp:UpdatePanel ID="upModalEstrategicos" runat="server">
                             <ContentTemplate>
                        
                           <dx:ASPxPopupControl ID="estrategicosR" runat="server" AllowDragging ="true" Width ="600px" Height="700px" HeaderText="Vincular PGG 2024-2028" PopupHorizontalAlign="WindowCenter"  PopupVerticalAlign="WindowCenter"  CloseAction="CloseButton"  >
                                <HeaderStyle Font-Bold="False" Font-Size="10"  HorizontalAlign="Left" />
                               <ContentCollection>
                                   <dx:PopupControlContentControl>
               <div  style ="overflow:auto; width:100%; height:90%; padding: 1px 10px 1px 10px;   ">
                     <h3> PGG 2024-2028 a vincular: <asp:Label ID="lblDireccion" runat="server" CssClass="text-danger"></asp:Label></h3>
                <div>
                    <asp:Button ID="btnVincularRes" runat="server" Text="Guardar" CssClass="btn btn-primary"  OnClick="btnVincularRes_Click"/>
                     <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" CssClass="btn btn-warning"   OnClick="btnCancelar_Click"/>
                </div>
                   <br/>
                   <dx:ASPxGridView ID="gvResEstrategicos" runat="server" Theme="Office2010Blue" Width="100%"   KeyFieldName="ID_ACCION"  SettingsPager-Mode="ShowAllRecords" >
                   <Settings ShowFooter="True"/>                           
                            <SettingsLoadingPanel Text="Cargando&amp;hellip;" />

                                <Settings ShowFooter="true" />
                                <ClientSideEvents EndCallback="function(s,e)
                                {
                                    if(s.cpError)
                                    {
                                        //alert(s.cpError);
                                        alertify.success(s.cpError);
                                        delete s.cpError;
                                    }             
                                }           
                              " />
             <SettingsContextMenu Enabled="True">
             </SettingsContextMenu>
             <SettingsPager AlwaysShowPager="True" PageSize="200" ShowSeparators="True">
                 <PageSizeItemSettings Visible="True">
                 </PageSizeItemSettings>
             </SettingsPager>
             <Settings ShowFilterRow="True" ShowGroupPanel="True" />
             <SettingsBehavior AllowFocusedRow="True" ConfirmDelete="True" />
             <SettingsCommandButton DeleteButton-Text="Quitar" DeleteButton-ButtonType="Button">
                                        <UpdateButton ButtonType="Button">
                                            <Styles>
                                                <Style CssClass="btn-warging">
                                                </Style>
                                            </Styles>
                                        </UpdateButton>
                                        <CancelButton ButtonType="Button">
                                            <Styles>
                                                <FocusRectStyle CssClass="btn-danger">
                                                </FocusRectStyle>
                                            </Styles>
                                        </CancelButton>
                                        <EditButton ButtonType="Button" Text="Editar">
                                        </EditButton>
                                        <DeleteButton ButtonType="Button">
                                           
                                        </DeleteButton>
                                    </SettingsCommandButton>
             <SettingsDataSecurity AllowDelete="true" AllowEdit="true" AllowInsert="False" />
             <SettingsLoadingPanel Mode="ShowOnStatusBar" />
             <SettingsText CommandApplyFilter="Aplicar filtro" CommandApplySearchPanelFilter="asdasfasdfasdf" ContextMenuHideColumn="Ocultar Columna" ContextMenuShowCustomizationWindow="Mostrar Columnas" ContextMenuShowFilterRow="Fila de Filtros" ContextMenuShowFilterRowMenu="Seleccione Tipo de Filtro" ContextMenuShowSearchPanel="Buscar" CustomizationWindowCaption="Columnas Ocultas" GroupPanel="Arrastre las columnas que desee agrupar" />
                    <Columns>
          
                              
         
                              <dx:GridViewCommandColumn ShowSelectCheckbox="True" SelectAllCheckboxMode="Page" Width="4%" VisibleIndex="0">
                                </dx:GridViewCommandColumn>


                          <dx:GridViewDataTextColumn FieldName="ID_ACCION" Name="ID_ACCION"   VisibleIndex="1" Visible="false">
                                       <HeaderStyle Wrap="True"></HeaderStyle>
                 </dx:GridViewDataTextColumn>

                        

                           <dx:GridViewDataTextColumn FieldName="EJE" Name="EJE"  Caption="EJE ESTRATÉGICO" VisibleIndex="2" Visible="true" GroupIndex="0">
                                       <HeaderStyle Wrap="True"></HeaderStyle>
                 </dx:GridViewDataTextColumn>


              <dx:GridViewDataTextColumn FieldName="META" Name="META"  Caption="META PRESIDENCIAL" VisibleIndex="3" Visible="true" GroupIndex="1">
                                       <HeaderStyle Wrap="True"></HeaderStyle>
                 </dx:GridViewDataTextColumn>

                 <dx:GridViewDataTextColumn FieldName="ACCION_ESTRATEGICA" Name="ACCION_ESTRATEGICA" Caption="ACCIÓN ESTRATÉGICA"   VisibleIndex="4" Visible="true"  >
                                       <HeaderStyle Wrap="True"></HeaderStyle>
                 </dx:GridViewDataTextColumn>

                        
                      
               

         </Columns>

                    </dx:ASPxGridView>
                   
             

                </div>
                
               
            </dx:PopupControlContentControl>
                                   </ContentCollection>
                           </dx:ASPxPopupControl>
                                     </ContentTemplate>
                              </asp:UpdatePanel>



                                    <%--nuevo popupvinculacion directa al eje--%>

                                    
                    <asp:UpdatePanel ID="upaneUnEje" runat="server">
                             <ContentTemplate>
                        
                           <dx:ASPxPopupControl ID="popUnEje" runat="server" AllowDragging ="true" Width ="600px" Height="700px" HeaderText="Vincular PGG 2024-2028" PopupHorizontalAlign="WindowCenter"  PopupVerticalAlign="WindowCenter"  CloseAction="CloseButton"  >
                                <HeaderStyle Font-Bold="False" Font-Size="10"  HorizontalAlign="Left" />
                               <ContentCollection>
                                   <dx:PopupControlContentControl>
               <div  style ="overflow:auto; width:100%; height:90%; padding: 1px 10px 1px 10px;   ">
                     <h3> PGG 2024-2028 a vincular: <asp:Label ID="Label2" runat="server" CssClass="text-danger"></asp:Label></h3>
                <div>
                    <asp:Button ID="btnUnEjej" runat="server" Text="Guardar" CssClass="btn btn-primary"  OnClick="btnUnEjej_Click"/>
                     <asp:Button ID="btnCancelaUnEje" runat="server" Text="Cancelar" CssClass="btn btn-warning"   Onclick="btnCancelaUnEje_Click"/>
                </div>
                   <br/>
                   <dx:ASPxGridView ID="gvUnEje" runat="server" Theme="Office2010Blue" Width="100%"   KeyFieldName="ID_EJE"  SettingsPager-Mode="ShowAllRecords" >
                   <Settings ShowFooter="True"/>                           
                            <SettingsLoadingPanel Text="Cargando&amp;hellip;" />

                                <Settings ShowFooter="true" />
                                <ClientSideEvents EndCallback="function(s,e)
                                {
                                    if(s.cpError)
                                    {
                                        //alert(s.cpError);
                                        alertify.success(s.cpError);
                                        delete s.cpError;
                                    }             
                                }           
                              " />
             <SettingsContextMenu Enabled="True">
             </SettingsContextMenu>
             <SettingsPager AlwaysShowPager="True" PageSize="200" ShowSeparators="True">
                 <PageSizeItemSettings Visible="True">
                 </PageSizeItemSettings>
             </SettingsPager>
             <Settings ShowFilterRow="True" ShowGroupPanel="True" />
             <SettingsBehavior AllowFocusedRow="True" ConfirmDelete="True" />
             <SettingsCommandButton DeleteButton-Text="Quitar" DeleteButton-ButtonType="Button">
                                        <UpdateButton ButtonType="Button">
                                            <Styles>
                                                <Style CssClass="btn-warging">
                                                </Style>
                                            </Styles>
                                        </UpdateButton>
                                        <CancelButton ButtonType="Button">
                                            <Styles>
                                                <FocusRectStyle CssClass="btn-danger">
                                                </FocusRectStyle>
                                            </Styles>
                                        </CancelButton>
                                        <EditButton ButtonType="Button" Text="Editar">
                                        </EditButton>
                                        <DeleteButton ButtonType="Button">
                                           
                                        </DeleteButton>
                                    </SettingsCommandButton>
             <SettingsDataSecurity AllowDelete="true" AllowEdit="true" AllowInsert="False" />
             <SettingsLoadingPanel Mode="ShowOnStatusBar" />
             <SettingsText CommandApplyFilter="Aplicar filtro" CommandApplySearchPanelFilter="asdasfasdfasdf" ContextMenuHideColumn="Ocultar Columna" ContextMenuShowCustomizationWindow="Mostrar Columnas" ContextMenuShowFilterRow="Fila de Filtros" ContextMenuShowFilterRowMenu="Seleccione Tipo de Filtro" ContextMenuShowSearchPanel="Buscar" CustomizationWindowCaption="Columnas Ocultas" GroupPanel="Arrastre las columnas que desee agrupar" />
                    <Columns>
          
                              
         
                              <dx:GridViewCommandColumn ShowSelectCheckbox="True" SelectAllCheckboxMode="Page" Width="4%" VisibleIndex="0">
                                </dx:GridViewCommandColumn>


                          <dx:GridViewDataTextColumn FieldName="ID_EJE" Name="ID_EJE"   VisibleIndex="1" Visible="false">
                                       <HeaderStyle Wrap="True"></HeaderStyle>
                 </dx:GridViewDataTextColumn>

                        

                           <dx:GridViewDataTextColumn FieldName="EJE" Name="EJE"  Caption="EJE ESTRATÉGICO" VisibleIndex="2" Visible="true">
                                       <HeaderStyle Wrap="True"></HeaderStyle>
                 </dx:GridViewDataTextColumn>

         </Columns>

                    </dx:ASPxGridView>
                   
             

                </div>
                
               
            </dx:PopupControlContentControl>
                                   </ContentCollection>
                           </dx:ASPxPopupControl>
                                     </ContentTemplate>
                              </asp:UpdatePanel>
                   

                                    <%--nuevo popup vinculacion directa al eje--%>

                      </asp:Panel>














                     <asp:Panel ID="Institucionales" runat="server"  style="display:none">
                          <%-- <h5 style="color:#2d572c">Resultados institucionales</h5>--%>
                        <%-- <hr>--%>
                         <h4 style="color:red">Resultados Institucionales, por favor evite utilizar en sus descripciones caratecteres como ' "" (apostrofes y comillas) y así evitar errores de grabación</h4>
                         <h5 style="color:red">Campos marcados con <font color="red">*</font> son obligatorios </h5>
                         <div class="form-group" >
                           <label for="txtsubCod" aria-required="true"><font color="red">*</font>Resultado Institucional: </label>
                             <asp:TextBox ID="txtResultado" runat="server" onkeypress="return restringe(event)" CssClass="form-control" data-toggle="tooltip" title="Redactar el resultado de acuerdo a los campos establecidos: ¿Que cambia?, ¿Cual es el cambio (+,-)?, ¿En quienes?, ¿En donde?, Meta ¿Cuando?, Meta ¿Cuanto?"></asp:TextBox>
                              </div> 
                          <div class="form-group">
                              <div class="btn-group" style="display:flex; justify-content:flex-start;">                                  
                                    <asp:Button ID="btnInstores" runat="server" Text="Resultado Institucional"  CssClass="btn btn-primary" OnClick="btnInstores_Click"/>                             
                                    <asp:Button ID="btnInCancelaRes" CssClass="btn btn-warning " runat="server" Text="Cancelar/regresar" OnClick="btnCancelPOM_Click"/>
                               </div>
                          </div>                           
                        
                                 <dx:ASPxGridView ID="gvResInsto" runat="server"   KeyFieldName="SPRES$ID_RESULTADO"  SettingsBehavior-AllowFocusedRow ="true" SettingsBehavior-ConfirmDelete="true" Theme="Office2010Blue"  OnRowUpdating="gvResInsto_RowUpdating" OnRowDeleting="gvResInsto_RowDeleting" Width="100%" >     
                                           <Settings ShowFooter="True"/>                           
                                <Settings ShowFooter="True"/>                           
                            <SettingsLoadingPanel Text="Cargando&amp;hellip;" />
                                <ClientSideEvents EndCallback="function(s,e)
                                {
                                    if(s.cpError)
                                    {
                                        //alert(s.cpError);
                                        alertify.success(s.cpError);
                                        delete s.cpError;
                                    }   
                                    
                                    if(s.cpShow)
                                        popupCommon.Show();
                                }           
                                " />
                               <SettingsContextMenu Enabled="True">
                               </SettingsContextMenu>
                               <SettingsPager AlwaysShowPager="True" PageSize="200" ShowSeparators="True">
                                    <PageSizeItemSettings Visible="True">
                                    </PageSizeItemSettings>
                                </SettingsPager>
                               <Settings ShowFilterRow="True" ShowGroupPanel="True" />
                               <SettingsBehavior AllowFocusedRow="True" ConfirmDelete="True" />
                                <SettingsCommandButton DeleteButton-Text="Quitar" DeleteButton-ButtonType="Button">
                                        <UpdateButton ButtonType="Button" Text="Guardar">
                                            <Styles>
                                                <Style CssClass="btn-warging">
                                                </Style>
                                            </Styles>
                                        </UpdateButton>
                                        <CancelButton ButtonType="Button" Text="Cancelar">
                                            <Styles>
                                                <FocusRectStyle CssClass="btn-danger">
                                                </FocusRectStyle>
                                            </Styles>
                                        </CancelButton>
                                        <EditButton ButtonType="Button" Text="Editar">
                                        </EditButton>
                                        <DeleteButton ButtonType="Button">
                                           
                                        </DeleteButton>
                                    </SettingsCommandButton>
             <SettingsDataSecurity AllowDelete="true" AllowEdit="true" AllowInsert="False" />
             <SettingsLoadingPanel Mode="ShowOnStatusBar" />
             <SettingsText CommandApplyFilter="Aplicar filtro" CommandApplySearchPanelFilter="asdasfasdfasdf" ContextMenuHideColumn="Ocultar Columna" ContextMenuShowCustomizationWindow="Mostrar Columnas" ContextMenuShowFilterRow="Fila de Filtros" ContextMenuShowFilterRowMenu="Seleccione Tipo de Filtro" ContextMenuShowSearchPanel="Buscar" CustomizationWindowCaption="Columnas Ocultas" GroupPanel="Arrastre las columnas que desee agrupar" />
                                     <Columns>
                                  <dx:GridViewDataTextColumn FieldName="SPRES$ID_RESULTADO"  Name="SPRES$ID_RESULTADO" ReadOnly="true" Visible="false" VisibleIndex="0" Width="0">
                                     <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn>
                                 <dx:GridViewDataTextColumn FieldName="SPRES$TIPO"  Name="SPRES$TIPO" ReadOnly="true" Visible="false" VisibleIndex="1" Width="0">
                                     <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn>
                                 <dx:GridViewDataTextColumn FieldName="SPRES$DESCRIPCION"  Name="SPRES$DESCRIPCION"  Caption="Resultado Institucional" Visible="true" VisibleIndex="2" Width="40%">
                                     <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn>
                                 <dx:GridViewDataTextColumn FieldName="PROGRAMA_PRESUPUESTARIO"  Name="PROGRAMA_PRESUPUESTARIO"  Caption="Numero de productos vinculados" ReadOnly="true"  Visible="true" VisibleIndex="3" Width="60%">
                                     <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn>
                                 <dx:GridViewDataTextColumn FieldName="SPRES$PROPIETARIO"  Name="SPRES$PROPIETARIO"  ReadOnly="true"  Visible="false" VisibleIndex="4" Width="0%">
                                     <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn>
                                         
                                  <dx:GridViewCommandColumn ShowNewButtonInHeader ="false" ShowEditButton="true" ShowDeleteButton="true" VisibleIndex="5" Width="15%">
                                                </dx:GridViewCommandColumn>
                             </Columns>

                                 </dx:ASPxGridView>

                        

                     </asp:Panel>
                                
                    

         
                   
                    
                 
            </div>

        </asp:View>
       


         <asp:View ID="programas" runat="server">
             <h4 style="color:#2d572c">Programas presupuestarios</h4>
            <h5>Campos marcados con <font color="red">*</font> son obligatorios </h5>
            
            <div  style ="overflow:auto; width:100%; height:90%; padding: 1px 10px 1px 10px;   ">                     
                    <div class="form-group">
                           <label for="txtsubCod"aria-required="true"><font color="red">*</font>Codigo programa presupuetario: </label>
                        <asp:TextBox ID="txtCodPrograma" runat="server"  PlaceHolder="Codigo del programa"  CssClass="form-control" onkeypress="return soloNumeros(event)"></asp:TextBox>
                    </div> 
                    <div class="form-group">
                        <label for="txtsubCod"aria-required="true"><font color="red">*</font>Programa presupuestario: </label>
                        <asp:TextBox ID="txtPrograma" runat="server" ValueType="System.String" PlaceHolder="Programa presupuestario" CssClass="form-control" onkeypress="return restringe(event)"></asp:TextBox>
                    </div>
                    
                <div class ="form-group">
                    <label for="txtsubCod"aria-required="true"><font color="red">*</font>Seleccione el tipo de programa: </label>
                        <dx:ASPxRadioButtonList ID="selectProgra" runat="server"
                                        ValueField="ID" TextField="Name2" RepeatColumns="2" RepeatLayout="Table"   Border-BorderStyle="Double" ClientInstanceName="viviendas" Font-Size= "Small" >
                                       <ClientSideEvents SelectedIndexchanged="cambioPrograma" />
                                         <CaptionSettings Position="Top" />
                                        <Items>
                                            <dx:ListEditItem Text="Es programa" Value="0"   />
                                            <dx:ListEditItem Text="Es subprograma" Value="1" />
                                         
                                        </Items>
                                       
                                </dx:ASPxRadioButtonList>
                        </div>
                 <asp:Panel ID="PanelPrograma" runat="server"  style="display:none">
                 <div class="form-group">
                           <label for="txtsubCod"aria-required="true"><font color="red">*</font>Programa presupuestario: </label>
                           <dx:ASPxComboBox ID="cbPrograma" runat="server" ValueType="System.String" NullText="Seleccione programa presupuestario" CssClass="form-control"></dx:ASPxComboBox>
                    </div> 
                     </asp:Panel>
                    <div class="form-group">
                        <asp:Button ID="btnPrograma" runat="server" Text="Nuevo programa"  CssClass="btn btn-primary"  OnClick="btnPrograma_Click"/>
                         <asp:Button ID="btnPrograCancelar" CssClass="btn btn-warning " runat="server" Text="Cancelar/Regresar"  OnClick="btnPrograCancelar_Click"/>
                        
                    </div>
            </div>
            <%--<div  style ="overflow:auto; width:100%; height:90%; padding: 1px 10px 1px 10px;   ">--%>
                
                                                               <dx:ASPxGridView ID="gvPrograPresupuestario" runat="server" KeyFieldName="PROGRAMA"   Theme="Office2010Blue"  AutoGenerateColumns="False" SettingsBehavior-AllowFocusedRow ="true" SettingsBehavior-ConfirmDelete="true" EnableCallBacks="true"  OnRowUpdating="gvPrograPresupuestario_RowUpdating" OnRowDeleting="gvPrograPresupuestario_RowDeleting" Width="100%" OnCommandButtonInitialize="gvPrograPresupuestario_CommandButtonInitialize">
                             
                          <Settings ShowFooter="True"/>                           
                            <SettingsLoadingPanel Text="Cargando&amp;hellip;" />

          
             <ClientSideEvents EndCallback="function(s,e)
             {
                if(s.cpError)
                {
                    //alert(s.cpError);
                    alertify.success(s.cpError);
                    delete s.cpError;
                }   
                 if(s.cpShow)
                    popupCommon.Show();
             }           
             " />
             <SettingsContextMenu Enabled="True">
             </SettingsContextMenu>
             <SettingsPager AlwaysShowPager="True" PageSize="200" ShowSeparators="True">
                 <PageSizeItemSettings Visible="True">
                 </PageSizeItemSettings>
             </SettingsPager>
             <Settings ShowFilterRow="True" ShowGroupPanel="True" />
             <SettingsBehavior AllowFocusedRow="True" ConfirmDelete="True" />
             <SettingsCommandButton DeleteButton-Text="Quitar" DeleteButton-ButtonType="Button">
                                        <UpdateButton ButtonType="Button" Text="Guardar">
                                            <Styles>
                                                <Style CssClass="btn-warging">
                                                </Style>
                                            </Styles>
                                        </UpdateButton>
                                        <CancelButton ButtonType="Button" Text="Cancelar">
                                            <Styles>
                                                <FocusRectStyle CssClass="btn-danger">
                                                </FocusRectStyle>
                                            </Styles>
                                        </CancelButton>
                                        <EditButton ButtonType="Button" Text="Editar">
                                        </EditButton>
                                        <DeleteButton ButtonType="Button">
                                           
                                        </DeleteButton>
                                    </SettingsCommandButton>
             <SettingsDataSecurity AllowDelete="true" AllowEdit="true" AllowInsert="False" />
             <SettingsLoadingPanel Mode="ShowOnStatusBar" />
             <SettingsText CommandApplyFilter="Aplicar filtro" CommandApplySearchPanelFilter="asdasfasdfasdf" ContextMenuHideColumn="Ocultar Columna" ContextMenuShowCustomizationWindow="Mostrar Columnas" ContextMenuShowFilterRow="Fila de Filtros" ContextMenuShowFilterRowMenu="Seleccione Tipo de Filtro" ContextMenuShowSearchPanel="Buscar" CustomizationWindowCaption="Columnas Ocultas" GroupPanel="Arrastre las columnas que desee agrupar" />
                             <Columns>
                                  <dx:GridViewDataTextColumn FieldName="PROGRAMA"  Name="PROGRAMA" ReadOnly="true" Visible="false" VisibleIndex="0">
                                     <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn>
                                 <dx:GridViewDataTextColumn FieldName="TIPO"  Name="TIPO" ReadOnly="true" Visible="false" VisibleIndex="1">
                                     <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn>
                                 <dx:GridViewDataComboBoxColumn FieldName="SPPRO$ID_PROGRAMA_PRESUPUESTO" Name="SPPRO$ID_PROGRAMA_PRESUPUESTO" Caption="PROGRAMA PRESUPUESTARIO" VisibleIndex="2">
                
                                  </dx:GridViewDataComboBoxColumn>


                                 <dx:GridViewDataTextColumn FieldName="SPPRO$ID_PROGRAMA_DEPENDE"  Name="SPPRO$ID_PROGRAMA_DEPENDE"  ReadOnly="true" Visible="false"  VisibleIndex="3">
                                     <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn>

                                                                                                   
                                 <dx:GridViewDataTextColumn FieldName="SPPRO$DESCRIPCION"  Name="SPPRO$DESCRIPCION"   Caption="PROGRAMA" ReadOnly="false" Visible="true" VisibleIndex="4">
                                     <HeaderStyle Wrap="True" />
                                    <Settings  AutoFilterCondition="Contains" />
                                    </dx:GridViewDataTextColumn>

                                  <dx:GridViewDataTextColumn FieldName="ID_SUBPROGRAMA"  Name="ID_SUBPROGRAMA"  ReadOnly="true" Visible="true"  VisibleIndex="5" Caption="SUBPROGRAMA PRESUPUESTARIO">
                                     <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn>

                                 <dx:GridViewDataTextColumn FieldName="SUBPROGRAMA_PRESUPUESTARIO"  Name="SUBPROGRAMA_PRESUPUESTARIO"  caption="SUBPROGRAMA" ReadOnly="false" Visible="true"  VisibleIndex="6">
                                     <HeaderStyle Wrap="True" />
                                     <Settings  AutoFilterCondition="Contains" />
                                    </dx:GridViewDataTextColumn>
                                 
                                 <dx:GridViewDataTextColumn FieldName="SPPRO$ID_POM"  Name="SPPRO$ID_POM" ReadOnly="true"  Visible="false" VisibleIndex="7">
                                     <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn>
                                 <dx:GridViewDataTextColumn FieldName="SPPRO$ID_INSTITUCION"  Name="SPPRO$ID_INSTITUCION"  ReadOnly="true"  Visible="false" VisibleIndex="8">
                                     <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn>
                                 <dx:GridViewDataTextColumn FieldName="SPPRO$PROPIETARIO"  Name="SPPRO$PROPIETARIO"  ReadOnly="true"   Visible="false" VisibleIndex="9">
                                     <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn>
                                
                                     <dx:GridViewCommandColumn ShowNewButtonInHeader ="false" ShowEditButton="true" ShowDeleteButton="true" VisibleIndex="10">
                                                </dx:GridViewCommandColumn>
                                
                             </Columns>         
                                                                   <SettingsEditing Mode="inline" />
                              </dx:ASPxGridView>
                        
                   
          <%--  </div>--%>
            </asp:View>



         <asp:View ID="Productos" runat="server">
              <h4 style="color:#2d572c">Productos multianuales</h4>
             <%--<hr>--%>
            <div  style ="overflow:auto; width:100%; height:90%; padding: 1px 10px 1px 10px;   "> 
                <div class ="form-group">
                    <asp:UpdatePanel ID="panCambiaproductos" runat="server">
                        <ContentTemplate>
                        <dx:ASPxRadioButtonList ID="rbProductos" runat="server"
                                        ValueField="ID" TextField="Name2" RepeatColumns="2" RepeatLayout="Table"   Border-BorderStyle="None" ClientInstanceName="viviendas"  AutoPostBack="true" OnSelectedIndexChanged="rbProductos_SelectedIndexChanged">
                                      
                     
                            
                            <CaptionSettings Position="Top" />
                                        <Items>
                                            <dx:ListEditItem Text="Productos vinculados a Resultado Institucional" Value="0"   />
                                            <dx:ListEditItem Text="Productos vinculados a Resultados Estrátegicos (RE)" Value="1" />
                                         
                                        </Items>
                                       <%--<ClientSideEvents SelectedIndexchanged="OnSelectedIndexChanged" />--%>
                                </dx:ASPxRadioButtonList>
                        </ContentTemplate>
                       </asp:UpdatePanel>
                        </div>
                <div class="form-group">
                                             <div class="btn-group" style="display:flex; justify-content:flex-start;" id="botonera">  
                                                 
                                    <asp:Button ID="btnNuevo" runat="server" Text="Nuevo producto"  CssClass="btn btn-primary"  OnClick="btnNuevo_Click"/>                             
                                    <asp:Button ID="btnEdit" CssClass="btn btn-warning" runat="server" Text="Editar producto" OnClick="btnEdit_Click"/>
                                    <asp:Button ID="btnDelete" CssClass="btn btn-danger" runat="server" Text="Eliminar producto" OnClick="btnDelete_Click"/>
                                    <asp:Button ID="btnSubIns" CssClass="btn " runat="server" Text="Subproductos" Style="background-color:#0489B1;color:white"   OnClick="btnSubIns_Click"/>
                                     <asp:Button ID="btnSNIP" CssClass="btn " runat="server" Text="Vincular varios SNIP a producto" Style="background-color:#3c3e42;color:white"   OnClick="btnSNIP_Click"/>
                                     <asp:Button ID="btnPGG" CssClass="btn " runat="server" Text="Vincular producción a RI/RE/PGG 2024-2028" Style="background-color:#076783;color:white" OnClick="btnPGG_Click" />
                                             </div>
                                         </div>
                     <asp:UpdatePanel ID="panProdInstitucional" runat="server" >
                        <ContentTemplate>                
                            
                <asp:Panel ID="ProdInsitucionales" runat="server"  style="display:none">                                       
                                         
                             <h4 style="color:#2d572c">Productos institucionales</h4>
                              <dx:ASPxGridView ID="gvProdInsto" runat="server" KeyFieldName="SPPRO$ID_PRODUCTO"   Theme="Office2010Blue" Width="100%" AutoGenerateColumns="False"  SettingsBehavior-ConfirmDelete="true" SettingsBehavior-AllowFocusedRow ="true" SettingsBehavior-ProcessFocusedRowChangedOnServer ="true" >
                              <Settings ShowFooter="True"/>                           
                            <SettingsLoadingPanel Text="Cargando&amp;hellip;" />
                          
                           

          <Settings ShowFooter="true" />
             <ClientSideEvents EndCallback="function(s,e)
             {
                if(s.cpError)
                {
                    //alert(s.cpError);
                    alertify.success(s.cpError);
                    delete s.cpError;
                }             
             }           
             " />
             <SettingsContextMenu Enabled="True">
             </SettingsContextMenu>
             <SettingsPager AlwaysShowPager="True" PageSize="200" ShowSeparators="True">
                 <PageSizeItemSettings Visible="True">
                 </PageSizeItemSettings>
             </SettingsPager>
             <Settings ShowFilterRow="True" ShowGroupPanel="True" />
             <SettingsBehavior AllowFocusedRow="True" ConfirmDelete="True" />
             <SettingsCommandButton DeleteButton-Text="Quitar" DeleteButton-ButtonType="Button">
                                        <UpdateButton ButtonType="Button">
                                            <Styles>
                                                <Style CssClass="btn-warging">
                                                </Style>
                                            </Styles>
                                        </UpdateButton>
                                        <CancelButton ButtonType="Button">
                                            <Styles>
                                                <FocusRectStyle CssClass="btn-danger">
                                                </FocusRectStyle>
                                            </Styles>
                                        </CancelButton>
                                        <EditButton ButtonType="Button" Text="Editar">
                                        </EditButton>
                                        <DeleteButton ButtonType="Button">
                                           
                                        </DeleteButton>
                                    </SettingsCommandButton>
             <SettingsDataSecurity AllowDelete="true" AllowEdit="true" AllowInsert="False" />
             <SettingsLoadingPanel Mode="ShowOnStatusBar" />
             <SettingsText CommandApplyFilter="Aplicar filtro" CommandApplySearchPanelFilter="asdasfasdfasdf" ContextMenuHideColumn="Ocultar Columna" ContextMenuShowCustomizationWindow="Mostrar Columnas" ContextMenuShowFilterRow="Fila de Filtros" ContextMenuShowFilterRowMenu="Seleccione Tipo de Filtro" ContextMenuShowSearchPanel="Buscar" CustomizationWindowCaption="Columnas Ocultas" GroupPanel="Arrastre las columnas que desee agrupar" />
                             <Columns>
                                  <dx:GridViewDataTextColumn FieldName="SPRES$ID_RESULTADO"  Name="SPRES$ID_RESULTADO" ReadOnly="true" Visible="false" VisibleIndex="0" Width="0">
                                     <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn>
                                 <dx:GridViewDataTextColumn FieldName="SPRES$DESCRIPCION"  Name="SPRES$DESCRIPCION" Caption="RESULTADO INSTITUCIONAL" ReadOnly="true" Visible="true" VisibleIndex="1" Width="100%">
                                     <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn>
                                 <dx:GridViewDataTextColumn FieldName="ID_EJE"  Name="ID_EJE"  ReadOnly="true" Visible="false" VisibleIndex="2" Width="100%">
                                     <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn>
                                 <dx:GridViewDataTextColumn FieldName="EJE_ESTRATEGICO"  Name="EJE_ESTRATEGICO"  Caption="EJE ESTRATÉGICO" ReadOnly="true" Visible="true" VisibleIndex="3" Width="100%">
                                     <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn>

                                 <dx:GridViewDataTextColumn FieldName="SPPRES$NIVEL"  Name="SPPRES$NIVEL"  ReadOnly="true" Visible="false" VisibleIndex="4" Width="100%">
                                     <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn>

                                 <dx:GridViewDataTextColumn FieldName="META_PRESIDENCIAL"  Name="META_PRESIDENCIAL" Caption ="META PRESIDENCIAL" ReadOnly="true" Visible="true" VisibleIndex="5" Width="100%">
                                     <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn>

                                  <dx:GridViewDataTextColumn FieldName="ACCION_ESTRATEGICA"  Name="ACCION_ESTRATEGICA" Caption = "ACCIÓN ESTRATÉGICA" ReadOnly="true" Visible="true" VisibleIndex="6" Width="100%">
                                     <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn>


                                 <dx:GridViewDataTextColumn FieldName="SPPRO$ID_PROGRAMA_PRESUPUESTO"  Name="SPPRO$ID_PROGRAMA_PRESUPUESTO"   ReadOnly="false" Visible="true" VisibleIndex="7" Width="10%" Caption="PROGRAMA PRESUPUESTARIO">
                                     <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn>
                                 <dx:GridViewDataTextColumn FieldName="PRESUPUESTO"  Name="PRESUPUESTO"  Caption="PROGRAMA" ReadOnly="true"  Visible="true" VisibleIndex="8" Width="15%">
                                     <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn>
                                 <dx:GridViewDataTextColumn FieldName="SPPRO$ID_PRODUCTO"  Name="SPPRO$ID_PRODUCTO"  ReadOnly="true"  Visible="false" VisibleIndex="9" Width="0%">
                                     <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn>

                                 <dx:GridViewDataTextColumn FieldName="PRODUCTO"  Name="PRODUCTO" Caption="PRODUCTO INSTITUCIONAL"  ReadOnly="true"  Visible="true" VisibleIndex="10" Width="50%">
                                     <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn>

                                  <dx:GridViewDataTextColumn FieldName="SPPRO$ID_MEDIDA"  Name="SPPRO$ID_MEDIDA"   ReadOnly="true"  Visible="false" VisibleIndex="11" Width="0">
                                     <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn>

                                 <dx:GridViewDataTextColumn FieldName="SNCGUM$NOMBRE"  Name="SNCGUM$NOMBRE"   ReadOnly="true" Caption="UNIDAD DE MEDIDA" Visible="true" VisibleIndex="12" Width="40%">
                                     <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn>

                                  <dx:GridViewDataTextColumn FieldName="SPPRO$OBJETIVO_CENTRAL"  Name="SPPRO$OBJETIVO_CENTRAL"   ReadOnly="true"  Visible="false" VisibleIndex="13" Width="0%">
                                     <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn>

                                 <dx:GridViewDataTextColumn FieldName="SPPRO$ACCION_ESTRATEGICA"  Name="SPPRO$ACCION_ESTRATEGICA"   ReadOnly="true"  Visible="false" VisibleIndex="14" Width="0%">
                                     <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn>

                                 <dx:GridViewDataTextColumn FieldName="OBJETIVO_SECTORIAL"  Name="OBJETIVO_SECTORIAL"   Caption="OBJETIVO SECTORIAL" ReadOnly="true"  Visible="false" VisibleIndex="15" Width="30%">
                                     <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn>

                                 <dx:GridViewDataTextColumn FieldName="ACCCION_ESTRATEGICA"  Name="ACCCION_ESTRATEGICA"   Caption="ACCION ESTRATEGICA" ReadOnly="true"  Visible="false" VisibleIndex="16" Width="30%">
                                     <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn>

                                 <dx:GridViewDataTextColumn FieldName="SPPRO$PROPIETARIO"  Name="SPPRO$PROPIETARIO"   ReadOnly="true"  Visible="false" VisibleIndex="17" Width="0">
                                     <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn>

                                   <dx:GridViewDataTextColumn FieldName="MUNICIPIOS"  Name="MUNICIPIOS"  Caption="MUNICIPIOS PRIORIZADOS" ReadOnly="true"  Visible="false" VisibleIndex="18" Width="30%">
                                     <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn>

                                  <dx:GridViewDataTextColumn FieldName="SPRES$COD_ESTRATEGICO"  Name="SPRES$COD_ESTRATEGICO"   ReadOnly="true"  Visible="false" VisibleIndex="19" Width="30%">
                                     <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn>
                                  <dx:GridViewDataTextColumn FieldName="SPRES$TIPO"  Name="SPRES$TIPO"   ReadOnly="true"  Visible="false" VisibleIndex="20" Width="30%">
                                     <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn>

                                     <dx:GridViewDataTextColumn FieldName="SPPRO$RESULTADO2"  Name="SPPRO$RESULTADO2"   ReadOnly="true"  Visible="false" VisibleIndex="21" Width="30%">
                                     <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn>

                                 
                             </Columns>
                                  
                              </dx:ASPxGridView> 
                            </asp:Panel>
                            </ContentTemplate>
                        
                         </asp:UpdatePanel>
                <asp:UpdatePanel ID="panProdEstrategicos" runat="server">
                        <ContentTemplate>      
                     <asp:Panel ID="ProdEstrategicos" runat="server"  style="display:none">
                         <%-- <div class="form-group">--%>
                                            <%-- <div class="btn-group" style="display:flex; justify-content:flex-start;">    --%>                              
                                               
                                    <%-- <asp:Button ID="btnNuevoEstra" runat="server" Text="Nuevo producto"  CssClass="btn btn-primary" OnClick="btnNuevoEstra_Click" />--%>                             
                                    <%--<asp:Button ID="btnEditEstra" CssClass="btn btn-warning" runat="server" Text="Editar producto"  OnClick="btnEditEstra_Click"/>--%>
                                   <%-- <asp:Button ID="btnDeleEstra" CssClass="btn btn-danger" runat="server" Text="Eliminar producto"  OnClick="btnDeleEstra_Click" />--%>
                                   <%-- <asp:Button ID="btnSuProductos" CssClass="btn " runat="server" Text="Subproductos" Style="background-color:#0489B1;color:white"  OnClick="btnSuProductos_Click"/>--%>
                                                    <%-- </div>--%>
                                        <%-- </div>--%>
                          <h4 style="color:#2d572c">Productos vinculados a Resultados Estrátegicos RE(si la tabla se muestra en blanco, es posible que necesite migrar los productos RE del periodo anterior, presione el botón "Migrar producto")</h4><asp:Button ID="btnMigrar" CssClass="btn " runat="server" Text="Migrar producto" Style="background-color:#0489B1;color:white"  OnClick="btnMigrar_Click"/>
                       
                         <dx:ASPxGridView ID="gvProdEstrategicos" runat="server" KeyFieldName="SPPRO$ID_PRODUCTO" AutoGenerateColumns="False"  Theme="Office2010Blue" Width="100%"   SettingsBehavior-AllowFocusedRow ="true">
                           <Settings ShowFooter="True"/>                           
                            <SettingsLoadingPanel Text="Cargando&amp;hellip;" />
          
             <ClientSideEvents EndCallback="function(s,e)
             {
                if(s.cpError)
                {
                    //alert(s.cpError);
                    alertify.success(s.cpError);
                    delete s.cpError;
                }             
             }           
             " />
                             <SettingsContextMenu Enabled="True">
             </SettingsContextMenu>
             <SettingsPager AlwaysShowPager="True" PageSize="200" ShowSeparators="True">
                 <PageSizeItemSettings Visible="True">
                 </PageSizeItemSettings>
             </SettingsPager>
             <Settings ShowFilterRow="True" ShowGroupPanel="True" />
             <SettingsBehavior AllowFocusedRow="True" ConfirmDelete="True" />
             <SettingsCommandButton DeleteButton-Text="Quitar" DeleteButton-ButtonType="Button">
                                        <UpdateButton ButtonType="Button">
                                            <Styles>
                                                <Style CssClass="btn-warging">
                                                </Style>
                                            </Styles>
                                        </UpdateButton>
                                        <CancelButton ButtonType="Button">
                                            <Styles>
                                                <FocusRectStyle CssClass="btn-danger">
                                                </FocusRectStyle>
                                            </Styles>
                                        </CancelButton>
                                        <EditButton ButtonType="Button" Text="Editar">
                                        </EditButton>
                                        <DeleteButton ButtonType="Button">
                                           
                                        </DeleteButton>
                                    </SettingsCommandButton>
             <SettingsDataSecurity AllowDelete="true" AllowEdit="true" AllowInsert="False" />
             <SettingsLoadingPanel Mode="ShowOnStatusBar" />
             <SettingsText CommandApplyFilter="Aplicar filtro" CommandApplySearchPanelFilter="asdasfasdfasdf" ContextMenuHideColumn="Ocultar Columna" ContextMenuShowCustomizationWindow="Mostrar Columnas" ContextMenuShowFilterRow="Fila de Filtros" ContextMenuShowFilterRowMenu="Seleccione Tipo de Filtro" ContextMenuShowSearchPanel="Buscar" CustomizationWindowCaption="Columnas Ocultas" GroupPanel="Arrastre las columnas que desee agrupar" />
 
                             <Columns>
                                 <dx:GridViewDataTextColumn FieldName="SPPRED$ID"  Name="SPPRED$ID"   ReadOnly="true"  Visible="false" VisibleIndex="0" Width="10%">
                                     <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn>

                                 <dx:GridViewDataTextColumn FieldName="RED"  Name="RED"  caption="RESULTADO ESTRATÉGICO (RE)" ReadOnly="true"  Visible="true" VisibleIndex="1" Width="10%">
                                     <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn>

                                  <dx:GridViewDataTextColumn FieldName="EJE"  Name="EJE"  caption="EJE ESTRATÉGICO" ReadOnly="true"  Visible="true" VisibleIndex="2" Width="10%">
                                     <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn>

                                  <dx:GridViewDataTextColumn FieldName="META"  Name="META"  caption="META PRESIDENCIAL" ReadOnly="true"  Visible="true" VisibleIndex="3" Width="10%">
                                     <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn>

                                 <dx:GridViewDataTextColumn FieldName="ACCION"  Name="ACCION"  caption="ACCIÓN ESTRATÉGICA" ReadOnly="true"  Visible="true" VisibleIndex="4" Width="10%">
                                     <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn>

                                 
                                 <dx:GridViewDataTextColumn FieldName="SPPRO$ID_PROGRAMA_PRESUPUESTO"  Name="SPPRO$ID_PROGRAMA_PRESUPUESTO"  Caption="PROGRAMA" ReadOnly="true"  Visible="true" VisibleIndex="5" Width="10%">
                                     <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn>
                                 <dx:GridViewDataTextColumn FieldName="PROGRAMA_PRESUPUETARIO"  Name="PROGRAMA_PRESUPUETARIO"  Caption="PROGRAMA PRESUPUESTARIO" ReadOnly="true"  Visible="true" VisibleIndex="6" Width="10%">
                                     <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn>
                                 <dx:GridViewDataTextColumn FieldName="SPPRO$ID_PRODUCTO"  Name="SPPRO$ID_PRODUCTO"  ReadOnly="true"  Visible="false" VisibleIndex="7" Width="0">
                                     <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn>

                                 <dx:GridViewDataTextColumn FieldName="PRODUCTO"  Name="PRODUCTO" Caption="PRODUCTO VINCULADO A RESULTADO ESTRÁTEGICO RE"  ReadOnly="true"  Visible="true" VisibleIndex="8" Width="10%">
                                     <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn>

                                  <dx:GridViewDataTextColumn FieldName="SPPRO$ID_MEDIDA"  Name="SPPRO$ID_MEDIDA"   ReadOnly="true"  Visible="false" VisibleIndex="9" Width="0">
                                     <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn>

                                 <dx:GridViewDataTextColumn FieldName="SNCGUM$NOMBRE"  Name="SNCGUM$NOMBRE"   ReadOnly="true" Caption="UNIDAD DE MEDIDA" Visible="true" VisibleIndex="10" Width="10%">
                                     <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn>

                                  <dx:GridViewDataTextColumn FieldName="SPPRO$OBJETIVO_CENTRAL"  Name="SPPRO$OBJETIVO_CENTRAL"   ReadOnly="true"  Visible="false" VisibleIndex="11" Width="0%">
                                     <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn>

                                 <dx:GridViewDataTextColumn FieldName="SPPRO$ACCION_ESTRATEGICA"  Name="SPPRO$ACCION_ESTRATEGICA"   ReadOnly="true"  Visible="false" VisibleIndex="12" Width="0%">
                                     <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn>

                                 <dx:GridViewDataTextColumn FieldName="OBJETIVO_SECTORIAL"  Name="OBJETIVO_SECTORIAL"   Caption="OBJETIVO SECTORIAL" ReadOnly="true"  Visible="false" VisibleIndex="13" Width="10%">
                                     <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn>

                                 <dx:GridViewDataTextColumn FieldName="ACCCION_ESTRATEGICA"  Name="ACCCION_ESTRATEGICA"   Caption="ACCION ESTRATEGICA" ReadOnly="true"  Visible="false" VisibleIndex="14" Width="10%">
                                     <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn>

                                 <dx:GridViewDataTextColumn FieldName="SPPRO$PROPIETARIO"  Name="SPPRO$PROPIETARIO"   ReadOnly="true"  Visible="false" VisibleIndex="15" Width="0">
                                     <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn>
                                 <dx:GridViewDataTextColumn FieldName="MUNICIPIOS"  Name="MUNICIPIOS"   Caption="MUNICIPIOS PRIORIZADOS" ReadOnly="true"  Visible="false" VisibleIndex="16" Width="30%">
                                     <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn>

                                   <dx:GridViewDataTextColumn FieldName="ID_RESULTADO"  Name="ID_RESULTADO"  ReadOnly="true"  Visible="false" VisibleIndex="17" Width="30%">
                                     <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn>
                                   <dx:GridViewDataTextColumn FieldName="SPPRO$RESULTADO2"  Name="SPPRO$RESULTADO2"  ReadOnly="true"  Visible="false" VisibleIndex="18" Width="30%">
                                     <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn>
                             </Columns>

                         </dx:ASPxGridView>
                      </asp:Panel>
                </ContentTemplate>
                    
                    </asp:UpdatePanel>
                </div>
             </asp:View>
      
            
        <asp:View ID="FormProductos" runat="server">
            <br />
            <br />
            
            <h4><asp:Label ID="lblProducto" runat="server"></asp:Label></h4>
            <h4><asp:Label ID="lblPilar" runat="server"></asp:Label></h4>
            <h4><asp:Label ID="lblResultado" runat="server"></asp:Label></h4>
                         <h5 style="color:red"><asp:Label ID="lblTipo" runat="server"></asp:Label>, por favor evite utilizar en sus descripciones caratecteres como ' "" (apostrofes y comillas) y así evitar errores de grabación</h5>
                         <h5>Campos marcados con <font color="red">*</font> son obligatorios </h5>
            <asp:Panel ID="panResultadoInstitucionales" runat="server" Style="display:none">       
            <p>Vinculación con resultados institucionales y PGG 2024-2028</p>
               <label for="txtsubCod" aria-required="true"><font color="red">*</font>Resultado institucional: </label>      
                              <asp:UpdatePanel ID="UpdatePanel9" runat="server"> 
                    <ContentTemplate>
                              <dx:ASPxComboBox ID="cbResultados" runat="server" ValueType="System.String" NullText="Seleccione el resultado institucional" CssClass="form-control"></dx:ASPxComboBox>
                              </ContentTemplate>
                                  </asp:UpdatePanel>


            
                                      

                 </asp:Panel>
            


                    <asp:Panel ID="panRed" runat="server" Style="display:none">     
                        <label for="txtsubCod" aria-required="true"><font color="red">*</font>Vincular a Resultados Estratégicos (RE): </label>
                            <asp:UpdatePanel ID="UpdatePanel10" runat="server"> 
                    <ContentTemplate>
                              <dx:ASPxComboBox ID="cbRed" runat="server" ValueType="System.String" NullText="Seleccione Resultado Estratégico (RE)" CssClass="form-control"></dx:ASPxComboBox>
                              </ContentTemplate>
                                  </asp:UpdatePanel>
                        </asp:Panel>



               <div class="form-group">
                           <label for="txtsubCod" aria-required="true"><font color="red">*</font>¿Este producto estará vinculado a la PGG 2024-2028:? </label>
                              <dx:ASPxRadioButtonList ID="cbPGG" RepeatColumns="2" RepeatLayout="Table"   Border-BorderStyle="None" ClientInstanceName="viviendas" runat="server" >
                                       <ClientSideEvents SelectedIndexchanged="cambioPGG" />
                                         <CaptionSettings Position="Top" />
                                        <Items>
                                            <dx:ListEditItem Text="No" Value="0"   />
                                            <dx:ListEditItem Text="Sí" Value="1" />
                                         
                                        </Items>
                                  
                                       </dx:ASPxRadioButtonList>
                              </div> 

            <asp:Panel ID="panPGGProductos" runat="server" Style="display:none">             

                           <label for="txtsubCod" aria-required="true"><font color="red">*</font>Eje estratégico: </label>
                <asp:UpdatePanel ID="UpdatePanel1" runat="server"> 
                    <ContentTemplate>
                <dx:ASPxComboBox ID="cbPilar" runat="server" ValueType="System.String" NullText="Seleccione Eje estratégico PGG" CssClass="form-control" OnValueChanged="cbPilar_ValueChanged" AutoPostBack="true"></dx:ASPxComboBox>
                       </ContentTemplate>        
            </asp:UpdatePanel>   
            

                          
                           <label for="txtsubCod" aria-required="true"><font color="red">*</font></label>
                        <label for="txtsubCod" aria-required="true"><font color="red">*</font>Metas presidenciales y acción estratética (si este control permanece inactivo vincule solo el eje estratégico): </label>      
                        <asp:UpdatePanel ID="UpdatePanel2" runat="server"> 
                    <ContentTemplate>
                              <dx:ASPxComboBox ID="cbAcciones" runat="server" ValueType="System.String" NullText="Seleccione eje estratégico/acción estratégica" CssClass="form-control"></dx:ASPxComboBox>
                              </ContentTemplate>
                                  </asp:UpdatePanel>
                             

             
                            
            </asp:Panel>



            <br />
                                          <div class="form-group">
                           <label for="txtsubCod" aria-required="true"><font color="red">*</font>Programa presupuestario: </label>
                            <asp:UpdatePanel ID="UpdatePanel3" runat="server"> 
                    <ContentTemplate>
                                              <dx:ASPxComboBox ID="cbProgramaPresupuestario" runat="server" ValueType="System.String" NullText="Seleccione programa presupuestario" CssClass="form-control" OnValueChanged="cbProgramaPresupuestario_ValueChanged" AutoPostBack="true"></dx:ASPxComboBox>
                        </ContentTemplate>
                                </asp:UpdatePanel>
                        </div> 

            <div class="form-group">
                           <label for="txtsubCod" aria-required="true"><font color="red">*</font>¿Necesita vincular el producto a un subprograma presupuestario:? </label>
                              <dx:ASPxRadioButtonList ID="cbSuprograma" runat="server"
                                        ValueField="ID" TextField="Name2" RepeatColumns="2" RepeatLayout="Table"   Border-BorderStyle="None" ClientInstanceName="viviendas" >
                                       <ClientSideEvents SelectedIndexchanged="cambioSubprograma" />
                                         <CaptionSettings Position="Top" />
                                        <Items>
                                            <dx:ListEditItem Text="No" Value="0"   />
                                            <dx:ListEditItem Text="Sí" Value="1" />
                                         
                                        </Items>
                                  
                                       </dx:ASPxRadioButtonList>
                              </div> 

                                         <asp:Panel ID="subProgramaPresupuestario" runat="server"  style="display:none">
                 <div class="form-group">
                           <label for="txtsubCod" aria-required="true"><font color="red">*</font>Subprograma presupuestario: </label>
                           <asp:UpdatePanel ID="UpdatePanel4" runat="server"> 
                    <ContentTemplate>
                     <dx:ASPxComboBox ID="cbSuProgramaPresupuestario" runat="server" ValueType="System.String" NullText="Seleccione subprograma presupuestario" CssClass="form-control"></dx:ASPxComboBox>
                    </ContentTemplate>
                               </asp:UpdatePanel>
                        </div> 
    
    
                     </asp:Panel>

                                         <div class="form-group">
                           <label for="txtsubCod" aria-required="true"><font color="red">*</font>Descripción <asp:Label ID="lblTipoDescripcion" runat="server"></asp:Label>  (no use comillas y apostrofes): </label>
                             <asp:TextBox ID="txtProductoInsto" runat="server" PlaceHolder="Descripción del producto" CssClass="form-control" onkeypress="return restringe(event)" data-toggle="tooltip" title="Redactar el producto de acuerdo a los campos establecidos: ¿Que?, ¿Quienes?, ¿En quienes?, ¿Donde?, Meta ¿Cuando?, Meta ¿Cuanto?"></asp:TextBox>
                              </div> 
                            <div class="form-group">
                           <label for="txtsubCod" aria-required="true"><font color="red">*</font>Unidad de medida: </label>
                             <dx:ASPxComboBox ID="cbUnidadMedida" runat="server" ValueType="System.String" NullText="Seleccione unidad de medida" CssClass="form-control"></dx:ASPxComboBox>
                              </div> 

<div class="form-group" style="display:none">
                           <label for="txtsubCod" aria-required="true"><font color="red">*</font>Vincular producto a acción estratégica (no aplica en productos vinculados  PGG 2024-2028): </label>
                              <dx:ASPxRadioButtonList ID="rbaccion" runat="server"
                                        ValueField="ID" TextField="Name2" RepeatColumns="2" RepeatLayout="Table"   Border-BorderStyle="Double" ClientInstanceName="viviendas"  Font-Size="Small">
                                       <ClientSideEvents SelectedIndexchanged="cambioAccion" />
                                         <CaptionSettings Position="Top" />
                                        <Items>
                                            <dx:ListEditItem Text="No" Value="0"   />
                                            <dx:ListEditItem Text="Sí" Value="1" />
                                         
                                        </Items>
                                       </dx:ASPxRadioButtonList>
                              </div> 
<asp:Panel ID="AccionEstrategica" runat="server"  style="display:none">
                 <div class="form-group">
                           <label for="txtsubCod" aria-required="true"><font color="red">*</font>Pilar PGGG: </label>
                     <asp:UpdatePanel ID="UpdatePanel5" runat="server"> 
                    <ContentTemplate>      
                     <dx:ASPxComboBox ID="cpPilarPGG" runat="server" ValueType="System.String" NullText="Seleccione Eje estratégico"  OnValueChanged="cpPilarPGG_ValueChanged" AutoPostBack="true" CssClass="form-control"></dx:ASPxComboBox>
                    </ContentTemplate>
                         </asp:UpdatePanel>
                        </div> 
    <div class="form-group" >
                           <label for="txtsubCod" aria-required="true"><font color="red">*</font>Objetivo Sectorial: </label>
          <asp:UpdatePanel ID="UpdatePanel6" runat="server"> 
                    <ContentTemplate>                    
        <dx:ASPxComboBox ID="cbObjSectorial" runat="server" ValueType="System.String" NullText="Seleccione Objetivo Sectorial" CssClass="form-control" OnValueChanged="cbObjSectorial_ValueChanged" AutoPostBack="true"></dx:ASPxComboBox>
                    </ContentTemplate>
              </asp:UpdatePanel>
                        </div> 
    <div class="form-group">
                           <label for="txtsubCod" aria-required="true"><font color="red">*</font>Eje estratégico/accion Estrategica (para instituciones vinculadas directamente a eje estratégico, favor de seleccionar tambien):  </label>
        <asp:UpdatePanel ID="UpdatePanel7" runat="server"> 
                    <ContentTemplate>                      
        <dx:ASPxComboBox ID="CbAccionEstrategica" runat="server" ValueType="System.String" NullText="Seleccione eje estratégico/acción estrategica" CssClass="form-control"></dx:ASPxComboBox>
                    </ContentTemplate>
            </asp:UpdatePanel>
                        </div> 
                     </asp:Panel>
            
            
                          <div class="form-group">
                              <div class="btn-group" style="display:flex; justify-content:flex-start;">
                            
                                    <asp:Button ID="btnGrabaProdInsto" runat="server" Text="Producto Institucional"  CssClass="btn btn-primary"  OnClick="btnGrabaProdInsto_Click"/>
                               
                                  <asp:Button ID="btnCancelaProdInsto" CssClass="btn btn-warning " runat="server" Text="Cancelar/regresar"  OnClick="btnCancelaProdInsto_Click"/>
                  
                                      </div>
                              </div>

        </asp:View>

           
        <asp:View ID="gridSubproductos" runat="server">
            <br />
            <br />
             <div class="form-group">
                                             <div class="btn-group" style="display:flex; justify-content:flex-start;">                                  
                                               
                                     <asp:Button ID="btnNuevoSub" runat="server" Text="Nuevo subproducto"  CssClass="btn btn-primary" OnClick="btnNuevoSub_Click" />                             
                                    <asp:Button ID="btnEditSub" CssClass="btn btn-warning" runat="server" Text="Editar subproducto"  OnClick="btnEditSub_Click" />
                                    <asp:Button ID="btnDelSub" CssClass="btn btn-danger" runat="server" Text="Eliminar subproducto" OnClick="btnDelSub_Click" />
                                     <asp:Button ID="btnPoliticas" CssClass="btn btn-success" runat="server" Text="Vincular subproductos a políticas públicas" OnClick="btnPoliticas_Click"/>
                                      <asp:Button ID="btnRegresa" CssClass="btn " runat="server" Text="Regresar a productos"  OnClick="btnRegresa_Click" style="color:white;background-color:#2E9AFE"/>
                                    
                                                     </div>
                                         </div>
            <h5><asp:Label ID="lbltitulo" runat="server" style="text-align: justify"></asp:Label></h5>
            <h5><asp:Label ID="lblPilarsub" runat="server"></asp:Label></h5>
            <h5><asp:Label ID="lblResultasub" runat="server"></asp:Label></h5>
            <h5><asp:Label ID="lblProdsub" runat="server"></asp:Label></h5>
            <h5 style="font-weight:bold; color:blue">Para consultar el territorio vinculado al subproducto despliegue la flecha en la primera columa de esta tabla</h5>
            <dx:ASPxGridView ID="gvSubproductos" runat="server"  KeyFieldName="SPPSUB$ID_SUBPRODUCTO" AutoGenerateColumns="False"  Theme="Office2010Blue" Width="100%"   SettingsBehavior-AllowFocusedRow ="true">
                 <Settings ShowFooter="True"/>                           
                            <SettingsLoadingPanel Text="Cargando&amp;hellip;" />
          
             <ClientSideEvents EndCallback="function(s,e)
             {
                if(s.cpError)
                {
                    //alert(s.cpError);
                    alertify.success(s.cpError);
                    delete s.cpError;
                }             
             }           
             " />
                             <SettingsContextMenu Enabled="True">
             </SettingsContextMenu>
             <SettingsPager AlwaysShowPager="True" PageSize="200" ShowSeparators="True">
                 <PageSizeItemSettings Visible="True">
                 </PageSizeItemSettings>
             </SettingsPager>
             <Settings ShowFilterRow="True" ShowGroupPanel="True" />
             <SettingsBehavior AllowFocusedRow="True" ConfirmDelete="True" />
             <SettingsCommandButton DeleteButton-Text="Quitar" DeleteButton-ButtonType="Button">
                                        <UpdateButton ButtonType="Button">
                                            <Styles>
                                                <Style CssClass="btn-warging">
                                                </Style>
                                            </Styles>
                                        </UpdateButton>
                                        <CancelButton ButtonType="Button">
                                            <Styles>
                                                <FocusRectStyle CssClass="btn-danger">
                                                </FocusRectStyle>
                                            </Styles>
                                        </CancelButton>
                                        <EditButton ButtonType="Button" Text="Editar">
                                        </EditButton>
                                        <DeleteButton ButtonType="Button">
                                           
                                        </DeleteButton>
                                    </SettingsCommandButton>
             <SettingsDataSecurity AllowDelete="true" AllowEdit="true" AllowInsert="False" />
             <SettingsLoadingPanel Mode="ShowOnStatusBar" />
             <SettingsText CommandApplyFilter="Aplicar filtro" CommandApplySearchPanelFilter="asdasfasdfasdf" ContextMenuHideColumn="Ocultar Columna" ContextMenuShowCustomizationWindow="Mostrar Columnas" ContextMenuShowFilterRow="Fila de Filtros" ContextMenuShowFilterRowMenu="Seleccione Tipo de Filtro" ContextMenuShowSearchPanel="Buscar" CustomizationWindowCaption="Columnas Ocultas" GroupPanel="Arrastre las columnas que desee agrupar" />
 <Columns>
     
       <dx:GridViewDataTextColumn FieldName="ID_RESULTADO"  Name="ID_RESULTADO" ReadOnly="true" Visible="false" VisibleIndex="2" Width="0%">
                                     <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn>
        

     <dx:GridViewDataTextColumn FieldName="SPPRO$ID_PROGRAMA_PRESUPUESTO"  Name="SPPRO$ID_PROGRAMA_PRESUPUESTO" Caption="PROGRAMA" ReadOnly="true" Visible="true" VisibleIndex="4" Width="20%">
                                     <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn>
     <dx:GridViewDataTextColumn FieldName="PROGRAMA"  Name="PROGRAMA" Caption="PROGRAMA" ReadOnly="true" Visible="true" VisibleIndex="5" Width="20%">
                                     <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn>
     <dx:GridViewDataTextColumn FieldName="SPPRO$ID_PRODUCTO"  Name="SPPRO$ID_PRODUCTO" ReadOnly="true" Visible="false" VisibleIndex="6" Width="0%">
                                     <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn>
     <dx:GridViewDataTextColumn FieldName="PRODUCTO"  Name="PRODUCTO" Caption="PRODUCTO" ReadOnly="true" Visible="true" VisibleIndex="7" Width="20%">
                                     <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn>

     <dx:GridViewDataTextColumn FieldName="MEDIDA_PRODUCTO"  Name="MEDIDA_PRODUCTO" Caption="MEDIDA PRODUCTO" ReadOnly="true" Visible="true" VisibleIndex="8" Width="30%">
                                     <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn>

     <dx:GridViewDataTextColumn FieldName="SPPRO$ID_MEDIDA"  Name="SPPRO$ID_MEDIDA" ReadOnly="true" Visible="false" VisibleIndex="9" Width="0%">
                                     <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn>

     <dx:GridViewDataTextColumn FieldName="SPPSUB$ID_SUBPRODUCTO"  Name="SPPSUB$ID_SUBPRODUCTO" ReadOnly="true" Visible="false" VisibleIndex="10" Width="0%">
                                     <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn>

     <dx:GridViewDataTextColumn FieldName="SUBPRODUCTO"  Name="SUBPRODUCTO" ReadOnly="true" Visible="true" VisibleIndex="11" Width="40%">
                                     <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn>

      <dx:GridViewDataTextColumn FieldName="SPPSUB$ID_MEDIDA"  Name="SPPSUB$ID_MEDIDA" ReadOnly="true" Visible="false" VisibleIndex="12" Width="0%">
                                     <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn>

     <dx:GridViewDataTextColumn FieldName="MEDIDA_SUBPRODUCTO"  Name="MEDIDA_SUBPRODUCTO" Caption ="MEDIDA SUBPRODUCTO" ReadOnly ="true" Visible="true" VisibleIndex="13" Width="30%">
                                     <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn>

     <dx:GridViewDataTextColumn FieldName="SPPSUB$SNIP"  Name="SPPSUB$SNIP" ReadOnly="true" Visible="false" VisibleIndex="14" Width="0%">
                                     <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn>

     <dx:GridViewDataTextColumn FieldName="SPRES$POM"  Name="SPRES$POM" ReadOnly="true" Visible="false" VisibleIndex="15" Width="0%">
                                     <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn>
     <dx:GridViewDataTextColumn FieldName="SPRES$INSTITUCION"  Name="SPRES$INSTITUCION" ReadOnly="true" Visible="false" VisibleIndex="16" Width="0%">
                                     <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn>
     <dx:GridViewDataTextColumn FieldName="SPPSUB$PROPIETARIO"  Name="SPPSUB$PROPIETARIO" ReadOnly="true" Visible="false" VisibleIndex="17" Width="0%">
                                     <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn>

     <dx:GridViewDataTextColumn FieldName="MUNICIPIOS"  Name="MUNICIPIOS" Caption="NÚM. MUNICIPIOS IDENTIFICADOS" ReadOnly="true" Visible="true" VisibleIndex="18">
                                     <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn>

 </Columns>

                <SettingsDetail ShowDetailRow="true" />
                                      <Templates>
                                          <DetailRow>
                                               <dx:ASPxGridView ID="gvMunicipio_vinculado"  KeyFieldName="SPSM$ID" SettingsBehavior-AllowFocusedRow ="true"  runat="server"  Width="30%" AutoGenerateColumns="False"    Theme="Office2010Blue" OnBeforePerformDataSelect="gvMunicipio_vinculado_BeforePerformDataSelect" OnRowDeleting="gvMunicipio_vinculado_RowDeleting">
                                                      <Settings ShowFooter="True"/>                           
                            <SettingsLoadingPanel Text="Cargando&amp;hellip;" />

                                <Settings ShowFooter="true" />
                                <ClientSideEvents EndCallback="function(s,e)
                                {
                                    if(s.cpError)
                                    {
                                        //alert(s.cpError);
                                        alertify.success(s.cpError);
                                        delete s.cpError;
                                    }             
                                }           
                              " />
             <SettingsContextMenu Enabled="True">
             </SettingsContextMenu>
             <SettingsPager AlwaysShowPager="True" PageSize="200" ShowSeparators="True">
                 <PageSizeItemSettings Visible="True">
                 </PageSizeItemSettings>
             </SettingsPager>
             <Settings ShowFilterRow="True" ShowGroupPanel="True" />
             <SettingsBehavior AllowFocusedRow="True" ConfirmDelete="false" />
             <SettingsCommandButton DeleteButton-Text="Desvincular" DeleteButton-ButtonType="Button">
                                        <UpdateButton ButtonType="Button">
                                            <Styles>
                                                <Style CssClass="btn-warging">
                                                </Style>
                                            </Styles>
                                        </UpdateButton>
                                        <CancelButton ButtonType="Button">
                                            <Styles>
                                                <FocusRectStyle CssClass="btn-danger">
                                                </FocusRectStyle>
                                            </Styles>
                                        </CancelButton>
                                        <EditButton ButtonType="Button" Text="Editar">
                                        </EditButton>
                                        <DeleteButton ButtonType="Button">
                                           
                                        </DeleteButton>
                                    </SettingsCommandButton>
             <SettingsDataSecurity AllowDelete="true" AllowEdit="true" AllowInsert="False" />
             <SettingsLoadingPanel Mode="ShowOnStatusBar" />
             <SettingsText CommandApplyFilter="Aplicar filtro" CommandApplySearchPanelFilter="asdasfasdfasdf" ContextMenuHideColumn="Ocultar Columna" ContextMenuShowCustomizationWindow="Mostrar Columnas" ContextMenuShowFilterRow="Fila de Filtros" ContextMenuShowFilterRowMenu="Seleccione Tipo de Filtro" ContextMenuShowSearchPanel="Buscar" CustomizationWindowCaption="Columnas Ocultas" GroupPanel="Arrastre las columnas que desee agrupar" />    
                                               <Columns>
                  <dx:GridViewCommandColumn ShowDeleteButton="true" ShowEditButton="false" ShowNewButtonInHeader="false" VisibleIndex="0">
                     <HeaderStyle Wrap="True" />
                 </dx:GridViewCommandColumn>

                                                    <dx:GridViewDataTextColumn FieldName="SPSM$ID" Name="SPSM$ID"   VisibleIndex="1" Visible="false">
                                       <HeaderStyle Wrap="True"></HeaderStyle>
                 </dx:GridViewDataTextColumn>
                                                    <dx:GridViewDataTextColumn FieldName="DEPARTAMENTO" Name="DEPARTAMENTO" Caption="DEPARTAMENTO"   VisibleIndex="2" Visible="true">
                                       <HeaderStyle Wrap="True"></HeaderStyle>
                 </dx:GridViewDataTextColumn>
                                                     <dx:GridViewDataTextColumn FieldName="MUNICIPIO" Name="MUNICIPIO" Caption="MUNICIPIO"   VisibleIndex="3" Visible="true">
                                       <HeaderStyle Wrap="True"></HeaderStyle>
                       <Settings AutoFilterCondition="Contains" />
                     <Settings />
                 </dx:GridViewDataTextColumn>

                                                   <dx:GridViewDataTextColumn FieldName="DEPTO" Name="DEPTO"  VisibleIndex="4" Visible="false">
                                       <HeaderStyle Wrap="True"></HeaderStyle>
                       
                     <Settings />
                 </dx:GridViewDataTextColumn>

                                                    <dx:GridViewDataTextColumn FieldName="COD_MUNO" Name="COD_MUNO"  VisibleIndex="5" Visible="false">
                                       <HeaderStyle Wrap="True"></HeaderStyle>
                       
                     <Settings />
                 </dx:GridViewDataTextColumn>

                                                   
                                               </Columns>
                                               </dx:ASPxGridView>
                                </DetailRow>
                                 </Templates>


            </dx:ASPxGridView>
        </asp:View>

         <asp:View ID="frmSubProd" runat="server">
             <br />
             <br />
            <h5><asp:Label ID="lblPilarRes" runat="server"></asp:Label></h5>
            <h5><asp:Label ID="lblResub2" runat="server"></asp:Label></h5>
            <h5><asp:Label ID="lblProduEstra2" runat="server"></asp:Label></h5>
             <h5><asp:Label ID="lblSuproducto" runat="server"></asp:Label></h5>
                         <h5 style="color:red"><asp:Label ID="lbldescsubs" runat="server"></asp:Label> Por favor evite utilizar en sus descripciones caratecteres como ' "" (apostrofes y comillas) y así evitar errores de grabación</h5>
                         <h5>Campos marcados con <font color="red">*</font> son obligatorios </h5>
            <div class="form-group">
                           <label for="txtsubCod" aria-required="true"><font color="red">*</font>Descripción <asp:Label ID="lblsubso" runat="server"></asp:Label>  (no use comillas y apostrofes): si esta vinculando a un proyecto SNIP la descripción no es necesaria </label>
                             <asp:TextBox ID="txtSubproducto" PlaceHolder="Descripción del subproducto" runat="server" CssClass="form-control" onkeypress="return restringe(event)" data-toggle="tooltip" title="Redactar el producto de acuerdo a los campos establecidos: ¿Que?, ¿Quienes?, ¿Donde?, Meta ¿Cuando?, Meta ¿Cuanto?"></asp:TextBox>
                              </div> 

                           <div class="form-group">
                           <label for="txtsubCod" aria-required="true"><font color="red">*</font> Seleccione unidad de medida, si esta vinculando a un proyecto SNIP la unidad de medida no es necesaria</label>
                             <dx:ASPxComboBox ID="cbUnidadesSub" runat="server" ValueType="System.String" NullText="Seleccione unidad de medida" CssClass="form-control"></dx:ASPxComboBox>
                               </div>
            <br />
                                         

                                         <div class="form-group">
                           <label for="txtsubCod" aria-required="true"><font color="red">*</font>¿Necesita vincular el subproducto a un proyecto SNIP: ? </label>
                              <dx:ASPxRadioButtonList ID="rbSNIP" runat="server" OnSelectedIndexChanged ="rbSNIP_SelectedIndexChanged" AutoPostBack ="true"
                                        ValueField="ID" TextField="Name2" RepeatColumns="2" RepeatLayout="Table"   Border-BorderStyle="None" ClientInstanceName="viviendas" >
                                       
                                         <CaptionSettings Position="Top" />
                                        <Items>
                                            <dx:ListEditItem Text="No" Value="0"   />
                                            <dx:ListEditItem Text="Sí" Value="1" />
                                         
                                        </Items>
                                  
                                       </dx:ASPxRadioButtonList>
                              </div> 
                                         <asp:Panel ID="PanSNIP" runat="server"  style="display:none">
                 <div class="form-group">
                           <label for="txtsubCod" aria-required="true"><font color="red">*</font>Codigo SNIP: </label>
                           <dx:ASPxComboBox ID="cbSNIP" runat="server" ValueType="System.String" NullText="Seleccione codigo SNIP" CssClass="form-control" AutoPostBack="true" OnValueChanged="cbSNIP_ValueChanged"></dx:ASPxComboBox>
                    </div> 
    
    
                     </asp:Panel>
          
                                         
                             <div class="frame" id="politicas" runat="server">
                           <label for="txtsubCod" aria-required="true"><font color="red">*</font>Número de lineas de acción de políticas públicas vinculadas: <asp:Label ID="numPoliticas" runat="server" CssClass="text-secondary"></asp:Label></label>
                                 <br />
                                  <asp:Button ID="btnPolitics" runat="server" OnClick="btnPolitics_Click" Text="Políticas públicas" CssClass="btn" style="background-color:#858687; color:white"/>
                          
                    </div>
             
                 <div class="frame" id="territorio" runat="server">
                           <label for="txtsubCod" aria-required="true"><font color="red">*</font>Número de municipios priorizados: <asp:Label ID="lblSubMunos" runat="server" CssClass="text-secondary"></asp:Label></label>

                     <div class="form-group">
                           <label for="txtsubCod" aria-required="true"><font color="red">*</font>¿Necesita vincular este subproducto a territorio? Al desplegar la tabla seleccione el(los) municipio(s) a vincular: </label>
                              <dx:ASPxRadioButtonList ID="cbMunosPriorizados" runat="server" ValueField="ID" TextField="Name2" RepeatColumns="2" RepeatLayout="Table"   Border-BorderStyle="Double" ClientInstanceName="viviendas"  Font-Size="Small" OnSelectedIndexChanged="cbMunosPriorizados_SelectedIndexChanged" AutoPostBack="true">
                                     
                                         <CaptionSettings Position="Top" />
                                        <Items>
                                            <dx:ListEditItem Text="No" Value="0"   />
                                            <dx:ListEditItem Text="Sí" Value="1" />
                                         
                                        </Items>
                                       </dx:ASPxRadioButtonList>
                              </div> 
                                  
                          
                    </div>

             <asp:Panel ID="munoPriorizados" runat="server"  style="display:none">
                 <div class="form-group">
                          
                                <dx:ASPxGridView ID="gvTerritorio" runat="server" Theme="Office2010Blue" Width="100%"   KeyFieldName="MUNICIPIO"  SettingsPager-Mode="ShowAllRecords" OnSelectionChanged="gvTerritorio_SelectionChanged" EnableCallBacks="false">
                                    <Settings ShowFooter="True"/>                           
                            <SettingsLoadingPanel Text="Cargando&amp;hellip;" />

                                <Settings ShowFooter="true" />
                                <ClientSideEvents EndCallback="function(s,e)
                                {
                                    if(s.cpError)
                                    {
                                        //alert(s.cpError);
                                        alertify.success(s.cpError);
                                        delete s.cpError;
                                    }             
                                }           
                              " />
                        <SettingsContextMenu Enabled="True">
             </SettingsContextMenu>
             <SettingsPager AlwaysShowPager="True" PageSize="20" ShowSeparators="True">
                 <PageSizeItemSettings Visible="True">
                 </PageSizeItemSettings>
             </SettingsPager>
             
             <SettingsBehavior AllowFocusedRow="True" ConfirmDelete="True" />
              <Settings ShowFilterRow="True" ShowGroupPanel="True" />
             <SettingsCommandButton DeleteButton-Text="Quitar" DeleteButton-ButtonType="Button">
                                        <UpdateButton ButtonType="Button">
                                            <Styles>
                                                <Style CssClass="btn-warging">
                                                </Style>
                                            </Styles>
                                        </UpdateButton>
                                        <CancelButton ButtonType="Button">
                                            <Styles>
                                                <FocusRectStyle CssClass="btn-danger">
                                                </FocusRectStyle>
                                            </Styles>
                                        </CancelButton>
                                        <EditButton ButtonType="Button" Text="Editar">
                                        </EditButton>
                                        <DeleteButton ButtonType="Button">
                                           
                                        </DeleteButton>
                                    </SettingsCommandButton>
                                    <SettingsDataSecurity AllowDelete="true" AllowEdit="true" AllowInsert="False" />
             <SettingsLoadingPanel Mode="ShowOnStatusBar" />
             <SettingsText CommandApplyFilter="Aplicar filtro" CommandApplySearchPanelFilter="asdasfasdfasdf" ContextMenuHideColumn="Ocultar Columna" ContextMenuShowCustomizationWindow="Mostrar Columnas" ContextMenuShowFilterRow="Fila de Filtros" ContextMenuShowFilterRowMenu="Seleccione Tipo de Filtro" ContextMenuShowSearchPanel="Buscar" CustomizationWindowCaption="Columnas Ocultas" GroupPanel="Arrastre las columnas que desee agrupar" />
                                    <Columns>
                                         <dx:GridViewCommandColumn ShowSelectCheckbox="True"  SelectAllCheckboxMode="Page"  Width="4%" VisibleIndex="0">
                                </dx:GridViewCommandColumn>
                                        <dx:GridViewDataTextColumn FieldName="COD_REGION" Name="COD_REGION"   VisibleIndex="1" Visible="false">
                                       <HeaderStyle Wrap="True"></HeaderStyle>
                 </dx:GridViewDataTextColumn>

                                         <dx:GridViewDataTextColumn FieldName="COD_DEPTO" Name="COD_DEPTO"   VisibleIndex="2" Visible="false">
                                       <HeaderStyle Wrap="True"></HeaderStyle>
                 </dx:GridViewDataTextColumn>

                                         <dx:GridViewDataTextColumn FieldName="MUNICIPIO" Name="MUNICIPIO"   VisibleIndex="3" Visible="false">
                                       <HeaderStyle Wrap="True"></HeaderStyle>
                 </dx:GridViewDataTextColumn>


                                        <dx:GridViewDataTextColumn FieldName="NOMBRE_REGION" Name="NOMBRE_REGION"  Caption="Región" VisibleIndex="4" Visible="true" GroupIndex="0">
                                       <HeaderStyle Wrap="True"></HeaderStyle>
                 </dx:GridViewDataTextColumn>

                                         <dx:GridViewDataTextColumn FieldName="DEPTOS" Name="DEPTOS"  Caption="Departamento" VisibleIndex="5" Visible="true">
                                       <HeaderStyle Wrap="True"></HeaderStyle>
                 </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn FieldName="NOMBRE_MUNICIPIO" Name="NOMBRE_MUNICIPIO"  Caption="Municipio" VisibleIndex="6" Visible="true" >
                                       <HeaderStyle Wrap="True"></HeaderStyle>
                 </dx:GridViewDataTextColumn>

                                    </Columns>
                                
                                </dx:ASPxGridView>
                                    
                        </div> 
    
    
                     </asp:Panel>


                          <div class="form-group">
                              <div class="btn-group" style="display:flex; justify-content:flex-start;">
                            
                                    <asp:Button ID="btnSubProducto" runat="server" OnClick= "btnSubProducto_Click" />
                               
                                  <asp:Button ID="btnCancelaSub" CssClass="btn btn-warning " runat="server" Text="Cancelar/regresar"  OnClick="btnCancelaSub_Click"/>
                  
                                      </div>
                              </div>

             <asp:HiddenField ID="aspCod" runat="server"/>
        </asp:View>
        <asp:View ID="vwDocumentos" runat="server">
            <h3 style="color:#2d572c">Publicación de documentos de programación multianual</h3>

            <div class="form-group">
                                             <div class="btn-group" style="display:flex; justify-content:flex-start;">                                  
                                                 <asp:UpdatePanel ID="panBotonArchivos" runat="server">   
                                                     <ContentTemplate>
                                     <asp:Button ID="btnNuevoInstrumento" runat="server" Text="Agregar instrumento"  CssClass="btn btn-primary"  OnClick="btnNuevoInstrumento_Click" />                             
                                    <asp:Button ID="btnVerArchivo" CssClass="btn btn-success" runat="server" Text="Ver archivo"  OnClick ="btnVerArchivo_Click"/>
                                    <asp:Button ID="btnAdjuntarArchivo" CssClass="btn btn-warning" runat="server" Text="Editar instrumento" OnClick="btnAdjuntarArchivo_Click"/>
                                    <asp:Button ID="btnEliminarArchivo" CssClass="btn btn-danger" runat="server" Text="Eliminar instrumento" OnClick="btnEliminarArchivo_Click"/>
                                     
                                   </ContentTemplate>
                                                         </asp:UpdatePanel>
                                                     </div>
                                         </div>
            <asp:UpdatePanel ID="pangvDocumentos" runat="server">
                <ContentTemplate>
            <dx:ASPxGridView ID="gvDocumentos" runat="server"  KeyFieldName="SPPDOC$ID_DOCUMENTO"  Theme="Office2010Blue"   SettingsBehavior-AllowFocusedRow ="true" SettingsBehavior-ConfirmDelete="true" Width="100%"  OnHtmlDataCellPrepared="gvDocumentos_HtmlDataCellPrepared">
                  <Settings ShowFooter="True"/>                           
                            <SettingsLoadingPanel Text="Cargando&amp;hellip;" />
          
             <ClientSideEvents EndCallback="function(s,e)
             {
                if(s.cpError)
                {
                    //alert(s.cpError);
                    alertify.success(s.cpError);
                    delete s.cpError;
                }             
             }           
             " />
                             <SettingsContextMenu Enabled="True">
             </SettingsContextMenu>
             <SettingsPager AlwaysShowPager="True" PageSize="200" ShowSeparators="True">
                 <PageSizeItemSettings Visible="True">
                 </PageSizeItemSettings>
             </SettingsPager>
             <Settings ShowFilterRow="True" ShowGroupPanel="True" />
             <SettingsBehavior AllowFocusedRow="True" ConfirmDelete="True" />
             <SettingsCommandButton DeleteButton-Text="Quitar" DeleteButton-ButtonType="Button">
                                        <UpdateButton ButtonType="Button">
                                            <Styles>
                                                <Style CssClass="btn-warging">
                                                </Style>
                                            </Styles>
                                        </UpdateButton>
                                        <CancelButton ButtonType="Button">
                                            <Styles>
                                                <FocusRectStyle CssClass="btn-danger">
                                                </FocusRectStyle>
                                            </Styles>
                                        </CancelButton>
                                        <EditButton ButtonType="Button" Text="Editar">
                                        </EditButton>
                                        <DeleteButton ButtonType="Button">
                                           
                                        </DeleteButton>
                                    </SettingsCommandButton>
             <SettingsDataSecurity AllowDelete="true" AllowEdit="true" AllowInsert="False" />
             <SettingsLoadingPanel Mode="ShowOnStatusBar" />
             <SettingsText CommandApplyFilter="Aplicar filtro" CommandApplySearchPanelFilter="asdasfasdfasdf" ContextMenuHideColumn="Ocultar Columna" ContextMenuShowCustomizationWindow="Mostrar Columnas" ContextMenuShowFilterRow="Fila de Filtros" ContextMenuShowFilterRowMenu="Seleccione Tipo de Filtro" ContextMenuShowSearchPanel="Buscar" CustomizationWindowCaption="Columnas Ocultas" GroupPanel="Arrastre las columnas que desee agrupar" />
                <Columns>
                <dx:GridViewDataTextColumn FieldName="SPPDOC$ID_DOCUMENTO"  Name="SPPDOC$ID_DOCUMENTO" ReadOnly="true" Visible="false" VisibleIndex="0" Width="0%">
                                     <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="SPPDOC$POM"  Name="SPPDOC$POM" ReadOnly="true" Visible="false" VisibleIndex="1" Width="0%">
                                     <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="SPPDOC$INSTO"  Name="SPPDOC$INSTO" ReadOnly="true" Visible="false" VisibleIndex="2" Width="0%">
                                     <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn>
                     <dx:GridViewDataTextColumn FieldName="SPPDOC$TIPO"  Name="SPPDOC$TIPO" ReadOnly="true" Visible="false"  VisibleIndex="3" Width="0%">
                                     <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn>
                     <dx:GridViewDataTextColumn FieldName="SPPD$DESCRIPCION"  Name="SPPD$DESCRIPCION" Caption="NOMBRE DEL INSTRUMENTO" ReadOnly="true" Visible="true" VisibleIndex="4" Width="40%">
                                     <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn>

                    <dx:GridViewDataTextColumn FieldName="SPPD$SIGLAS"  Name="SPPD$SIGLAS" Caption="SIGLAS" ReadOnly="true" Visible="true" VisibleIndex="5" Width="10%">
                                     <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn>

                     <dx:GridViewDataTextColumn FieldName="SPPDOC$DESCRIPCION"  Name="SPPDOC$DESCRIPCION" Caption="DESCRIPCIÓN DEL DOCUMENTO" ReadOnly="true" Visible="true" VisibleIndex="6" Width="40%">
                                     <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn>
                     <dx:GridViewDataTextColumn FieldName="ARCHIVO_ADJUNTO"  Name="ARCHIVO_ADJUNTO" Caption="Archivo adjunto" ReadOnly="true" Visible="true" VisibleIndex="7" Width="10%">
                                     <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="SPPDOC$NOMARCHIVO"  Name="SPPDOC$NOMARCHIVO"  ReadOnly="true" Visible="false" VisibleIndex="8" Width="0%">
                                     <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn>

                     <dx:GridViewDataTextColumn FieldName="SPPDOC$RUTA"  Name="SPPDOC$RUTA" ReadOnly="true" Visible="false" VisibleIndex="9" Width="0%">
                                     <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn>

                    <dx:GridViewDataTextColumn FieldName="SPPDOC$PROPIETARIO"  Name="SPPDOC$PROPIETARIO" ReadOnly="true" Visible="false" VisibleIndex="10" Width="0%">
                                     <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn>

                    
                </Columns>
            </dx:ASPxGridView>
                    </ContentTemplate>
            </asp:UpdatePanel>
          
                  <asp:UpdatePanel ID="upPopInstrument"  UpdateMode="Always" runat="server"  >                       
                             <ContentTemplate>
                        
                           <dx:ASPxPopupControl ID="popInstrumento" runat="server" AllowDragging ="true" Width ="600px" Height="400px" HeaderText="Instrumento de Planificación" PopupHorizontalAlign="WindowCenter"  PopupVerticalAlign="WindowCenter"  CloseAction="CloseButton"  >
                                <HeaderStyle Font-Bold="False" Font-Size="10"  HorizontalAlign="Left" />
                               <ContentCollection>
                                   <dx:PopupControlContentControl>
               <div  style ="overflow:auto; width:100%; height:90%; padding: 1px 10px 1px 10px;   ">
                   <h5><asp:Label ID="lblInstrumentoOpera" runat="server"></asp:Label></h5>
                     <div class="form-group">
                           <label for="txtsubCod"aria-required="true"><font color="red">*</font>Seleccione el instrumento de planificación: </label>
                           <dx:ASPxComboBox ID="cbInstrumento" runat="server" ValueType="System.String" NullText="Seleccione instrumento de planificación" CssClass="form-control"></dx:ASPxComboBox>
                    </div> 

             <div class="form-group">
                           <label for="txtsubCod"aria-required="true">Descripción del instrumento de planificación: </label>
                            <asp:TextBox ID="txtDescripcionInstrumento" PlaceHolder="Descripción del instrumento de planificación" CssClass="form-control"  runat="server"></asp:TextBox>
                    </div> 
                   <asp:Panel ID="panArchivo" runat="server">
                      <label for="txtsubCod"aria-required="true">Este instrumento de planificación tiene un archivo adjunto: </label>
                       <br></br><asp:Label ID="lblarchivo" runat="server"></asp:Label>
                
                   </asp:Panel>
                   <br>
                   </br>
                   <div class="form-group">
                           <label for="txtsubCod" aria-required="true"><font color="red">*</font>¿Va adjuntar el documento en formato PDF del Instrumento de Planificación? : </label>
                              <dx:ASPxRadioButtonList ID="cbInstrumentos" runat="server"
                                        ValueField="ID" TextField="Name2" RepeatColumns="2" RepeatLayout="Table"   Border-BorderStyle="None" ClientInstanceName="viviendas" >
                                       <ClientSideEvents SelectedIndexchanged="cambioInstrumento" />
                                         <CaptionSettings Position="Top" />
                                        <Items>
                                            <dx:ListEditItem Text="No" Value="0"   />
                                            <dx:ListEditItem Text="Sí" Value="1" />
                                         
                                        </Items>
                                       </dx:ASPxRadioButtonList>
                              </div> 
                   <asp:Panel ID="panInstrumento" runat="server" style="display:none">
                       <label style="color:red" for="txtsubCod"aria-required="true"><font color="red">*</font>Adjunte el documento de resolución, el archivo no debe exceder de 10 MB: </label>
                      
                       <asp:FileUpload ID="fileInstrumento"   runat="server" accept=".pdf" />
                     <asp:CustomValidator ID="CustomValidator1" runat="server" ControlToValidate="fileInstrumento"
   ErrorMessage="File size should not be greater than 1 KB." OnServerValidate="CustomValidator1_ServerValidate"></asp:CustomValidator>
                       <asp:RequiredFieldValidator ID="RequiredFieldValidator4" ForeColor="Red" runat="server"
                        ValidationGroup="gr1" ControlToValidate="fileInstrumento">Solo archivos en formato PDF</asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="regval1" ForeColor="Red" runat="server" ValidationGroup="gr1"
                        ControlToValidate="fileInstrumento" ValidationExpression="^([a-zA-Z].*|[1-9].*)\.(((p|P)(d|D)(f|F)))$">Solo se puede adjuntar archivo en formato PDF </asp:RegularExpressionValidator>
                   
                   </asp:Panel>

                </div>
                                      
                
                <div>
                   
                    <asp:Button ID="btnGrabaInstrumento" runat="server"  OnClick="btnGrabaInstrumento_Click"/>
              
                     <asp:Button ID="btnCancelaInstrumento" runat="server" Text="Cancelar" CssClass="btn btn-warning"   OnClick="btnCancelaInstrumento_Click"/>
                </div>
            </dx:PopupControlContentControl>
                                   </ContentCollection>
                           </dx:ASPxPopupControl>
                                 
                                     </ContentTemplate>
                  
                      </asp:UpdatePanel>

            
            <asp:UpdatePanel ID="poupVistaPrevia" runat="server"  >                
                             <ContentTemplate>
                                 <dx:ASPxPopupControl ID="popDocumento" runat="server" AllowDragging ="true" Width ="900px" Height="400px" HeaderText="Instrumento de Planificación" PopupHorizontalAlign="WindowCenter"  PopupVerticalAlign="WindowCenter"  CloseAction="CloseButton"  >
                                <HeaderStyle Font-Bold="False" Font-Size="10"  HorizontalAlign="Left" />
                               <ContentCollection>
                                   <dx:PopupControlContentControl>
             <%-- <iframe id="vstPrevia" runat="server"   Width ="900" Height="400" ></iframe>--%>
            </dx:PopupControlContentControl>
                                   </ContentCollection>
                           </dx:ASPxPopupControl>

                                 </ContentTemplate>
                </asp:UpdatePanel>

           
                
        </asp:View>

        <asp:View ID="SNIP" runat="server">
            <h3 style="color:#2d572c">Vinculación de proyectos SNIP a productos (Este proceso generará subproductos vinculados a proyectos SNIP)</h3>
            <h2 style="color:#2d572c">Producto: <asp:Label ID="lblproductoSNIP" runat="server"></asp:Label></h2>
            <div>
                    <asp:Button ID="btnGrabaSNIP" runat="server" Text="Guardar" CssClass="btn btn-primary"  OnClick ="btnGrabaSNIP_Click"/>
                     <asp:Button ID="btnCancelaSNIP" runat="server" Text="Cancelar" CssClass="btn btn-warning"   OnClick ="btnCancelaSNIP_Click"/>
                </div>
                   <br/>
                   <dx:ASPxGridView ID="gvSNIP" runat="server" Theme="Office2010Blue" Width="100%"   KeyFieldName="PROYECTO"  SettingsPager-Mode="ShowAllRecords" >
                   <Settings ShowFooter="True"/>                           
                            <SettingsLoadingPanel Text="Cargando&amp;hellip;" />

                                <Settings ShowFooter="true" />
                                <ClientSideEvents EndCallback="function(s,e)
                                {
                                    if(s.cpError)
                                    {
                                        //alert(s.cpError);
                                        alertify.success(s.cpError);
                                        delete s.cpError;
                                    }             
                                }           
                              " />
             <SettingsContextMenu Enabled="True">
             </SettingsContextMenu>
             <SettingsPager AlwaysShowPager="True" PageSize="200" ShowSeparators="True">
                 <PageSizeItemSettings Visible="True">
                 </PageSizeItemSettings>
             </SettingsPager>
             <Settings ShowFilterRow="True" ShowGroupPanel="True" />
             <SettingsBehavior AllowFocusedRow="True" ConfirmDelete="True" />
             <SettingsCommandButton DeleteButton-Text="Quitar" DeleteButton-ButtonType="Button">
                                        <UpdateButton ButtonType="Button">
                                            <Styles>
                                                <Style CssClass="btn-warging">
                                                </Style>
                                            </Styles>
                                        </UpdateButton>
                                        <CancelButton ButtonType="Button">
                                            <Styles>
                                                <FocusRectStyle CssClass="btn-danger">
                                                </FocusRectStyle>
                                            </Styles>
                                        </CancelButton>
                                        <EditButton ButtonType="Button" Text="Editar">
                                        </EditButton>
                                        <DeleteButton ButtonType="Button">
                                           
                                        </DeleteButton>
                                    </SettingsCommandButton>
             <SettingsDataSecurity AllowDelete="true" AllowEdit="true" AllowInsert="False" />
             <SettingsLoadingPanel Mode="ShowOnStatusBar" />
             <SettingsText CommandApplyFilter="Aplicar filtro" CommandApplySearchPanelFilter="asdasfasdfasdf" ContextMenuHideColumn="Ocultar Columna" ContextMenuShowCustomizationWindow="Mostrar Columnas" ContextMenuShowFilterRow="Fila de Filtros" ContextMenuShowFilterRowMenu="Seleccione Tipo de Filtro" ContextMenuShowSearchPanel="Buscar" CustomizationWindowCaption="Columnas Ocultas" GroupPanel="Arrastre las columnas que desee agrupar" />
                    <Columns>
          
                              
         
                              <dx:GridViewCommandColumn ShowSelectCheckbox="True" SelectAllCheckboxMode="Page" Width="4%" VisibleIndex="0">
                                </dx:GridViewCommandColumn>


                          <dx:GridViewDataTextColumn FieldName="PROYECTO" Name="PROYECTO" Caption="CODIGO SNIP"   VisibleIndex="1" Visible="true">
                                       <HeaderStyle Wrap="True"></HeaderStyle>
                 </dx:GridViewDataTextColumn>

                        <dx:GridViewDataTextColumn FieldName="PROYECTOSNIP" Name="PROYECTOSNIP"   VisibleIndex="2" Visible="true" Caption="PROYECTO">
                                       <HeaderStyle Wrap="True"></HeaderStyle>
                 </dx:GridViewDataTextColumn>
                                    
               

         </Columns>

                    </dx:ASPxGridView>
                  
        
        </asp:View>

        <asp:View ID="vincula_pgg" runat="server">
            <br />
             <br />
            <h4 style="color:#2d572c">Vinculación de la producción institucional a la PGG 2024-2028/Resultado institucional, seleccione los productos a vincular y presione el botón "Vincular PGG 2024-2028" para asociar a la PGG vigente o presione el botón "Vincular a resultado institucional" si necesita vincular el producto a un resultado institucional vigente </h4>
            <h4>Productos en letras rojas siguen vinculados a la anterior estructura PGG 2020-2024</h4>
             <div class="form-group">
                       <div class="btn-group" style="display:flex; justify-content:flex-start;" id="botoneraPGG">  
                            <asp:Button ID="btnVinculaPGG2425" runat="server" Text="Vincular a Resultado Institucional"  CssClass="btn btn-primary"  OnClick="btnVinculaPGG2425_Click" />
                           <%--<asp:Button ID="btnVinculaPconRT" runat="server" Text="Vincular a resultado institucional"  CssClass="btn btn-success" OnClick="btnVinculaPconRT_Click" />--%>
                           <asp:Button ID="btnRED" runat="server" Text="Vincular a Resultado Estratégico RE"  CssClass="btn btn-danger" OnClick="btnRED_Click" />
                           <asp:Button ID="btnRegresaProdInsto" runat="server" Text="Cancelar/Regresar" CssClass="btn btn-warning" OnClick="btnRegresaProdInsto_Click" />
                           </div>
                   </div>
            <dx:ASPxGridView ID="gvProduccionInsto" runat="server" Theme="Office2010Blue" Width="100%"   KeyFieldName="SPPRO$ID_PRODUCTO"  SettingsPager-Mode="ShowAllRecords" OnHtmlRowPrepared="gvProduccionInsto_HtmlRowPrepared">
             <Settings ShowFooter="True"/>                           
                            <SettingsLoadingPanel Text="Cargando&amp;hellip;" />

                                <Settings ShowFooter="true" />
                                <ClientSideEvents EndCallback="function(s,e)
                                {
                                    if(s.cpError)
                                    {
                                        //alert(s.cpError);
                                        alertify.success(s.cpError);
                                        delete s.cpError;
                                    }             
                                }           
                              " />
             <SettingsContextMenu Enabled="True">
             </SettingsContextMenu>
             <SettingsPager AlwaysShowPager="True" PageSize="200" ShowSeparators="True">
                 <PageSizeItemSettings Visible="True">
                 </PageSizeItemSettings>
             </SettingsPager>
             <Settings ShowFilterRow="True" ShowGroupPanel="True" />
             <SettingsBehavior AllowFocusedRow="True" ConfirmDelete="True" />
             <SettingsCommandButton DeleteButton-Text="Quitar" DeleteButton-ButtonType="Button">
                                        <UpdateButton ButtonType="Button">
                                            <Styles>
                                                <Style CssClass="btn-warging">
                                                </Style>
                                            </Styles>
                                        </UpdateButton>
                                        <CancelButton ButtonType="Button">
                                            <Styles>
                                                <FocusRectStyle CssClass="btn-danger">
                                                </FocusRectStyle>
                                            </Styles>
                                        </CancelButton>
                                        <EditButton ButtonType="Button" Text="Editar">
                                        </EditButton>
                                        <DeleteButton ButtonType="Button">
                                           
                                        </DeleteButton>
                                    </SettingsCommandButton>
             <SettingsDataSecurity AllowDelete="true" AllowEdit="true" AllowInsert="False" />
             <SettingsLoadingPanel Mode="ShowOnStatusBar" />
             <SettingsText CommandApplyFilter="Aplicar filtro" CommandApplySearchPanelFilter="asdasfasdfasdf" ContextMenuHideColumn="Ocultar Columna" ContextMenuShowCustomizationWindow="Mostrar Columnas" ContextMenuShowFilterRow="Fila de Filtros" ContextMenuShowFilterRowMenu="Seleccione Tipo de Filtro" ContextMenuShowSearchPanel="Buscar" CustomizationWindowCaption="Columnas Ocultas" GroupPanel="Arrastre las columnas que desee agrupar" />    
            <Columns>
                 <dx:GridViewCommandColumn ShowSelectCheckbox="True" SelectAllCheckboxMode="Page" Width="4%" VisibleIndex="0">
                                </dx:GridViewCommandColumn>
                <dx:GridViewDataTextColumn FieldName="SPRES$ID_RESULTADO" Name="SPRES$ID_RESULTADO" Caption="CODIGO SNIP"   VisibleIndex="1" Visible="false">
                                       <HeaderStyle Wrap="True"></HeaderStyle>
                 </dx:GridViewDataTextColumn>

                   <dx:GridViewDataTextColumn FieldName="SPPRO$RESULTADO2" Name="SPPRO$RESULTADO2" Caption="CODIGO SNIP"   VisibleIndex="2" Visible="false">
                                       <HeaderStyle Wrap="True"></HeaderStyle>
                 </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="SPRES$COD_ESTRATEGICO" Name="SPRES$COD_ESTRATEGICO" Caption="CODIGO SNIP"   VisibleIndex="2" Visible="false">
                                       <HeaderStyle Wrap="True"></HeaderStyle>
                 </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="RESULTADO" Name="RESULTADO" Caption="VINCULACION 1"   VisibleIndex="3" Visible="true">
                                       <HeaderStyle Wrap="True"></HeaderStyle>
                       <Settings AutoFilterCondition="Contains" />
                     <Settings />
                 </dx:GridViewDataTextColumn>

                   <dx:GridViewDataTextColumn FieldName="RESULTADO2" Name="RESULTADO2" Caption="VINCULACION 2"   VisibleIndex="3" Visible="true">
                                       <HeaderStyle Wrap="True"></HeaderStyle>
                       <Settings AutoFilterCondition="Contains" />
                     <Settings />
                 </dx:GridViewDataTextColumn>

                <dx:GridViewDataTextColumn FieldName="SPPRO$ID_PRODUCTO" Name="SPPRO$ID_PRODUCTO" Caption="RESULTADO"   VisibleIndex="4" Visible="false">
                                       <HeaderStyle Wrap="True"></HeaderStyle>
                 </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="SPPRO$DESCRIPCION" Name="SPPRO$DESCRIPCION" Caption="PRODUCTO"   VisibleIndex="5" Visible="true">
                                       <HeaderStyle Wrap="True"></HeaderStyle>
                       <Settings AutoFilterCondition="Contains" />
                     <Settings />
                 </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="MEDIDA" Name="MEDIDA" Caption="MEDIDA PRODUCTO"   VisibleIndex="6" Visible="true">
                                       <HeaderStyle Wrap="True"></HeaderStyle>
                       <Settings AutoFilterCondition="Contains" />
                     <Settings />
                 </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="SPPRO$TEMPORAL" Name="SPPRO$TEMPORAL" Caption="SPPRO$TEMPORAL"   VisibleIndex="7" Visible="false">
                                       <HeaderStyle Wrap="True"></HeaderStyle>
                 </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="NIVEL" Name="NIVEL" Caption="NIVEL"   VisibleIndex="8" Visible="false">
                                       <HeaderStyle Wrap="True"></HeaderStyle>
                 </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="SPRES$TIPO" Name="SPRES$TIPO"  VisibleIndex="9" Visible="false">
                                       <HeaderStyle Wrap="True"></HeaderStyle>
                 </dx:GridViewDataTextColumn>
                </Columns>
            </dx:ASPxGridView>
            </asp:View>

        
                                
        


        <asp:View ID="ViewSubprodcutoPolitica" runat="server">
            <br />
             <br />
            <h4 style="color:#2d572c">Vinculación de subproductos de la producción instituicional a Políticas Públicas </h4>
            <h4>Para visualizar el detalle de las Políticas Públicas a las que el subprodcto se encuentra vinculado, despliegue el detalle en la flecha en la columna izquierda </h4>
             <div class="form-group">
                       <div class="btn-group" style="display:flex; justify-content:flex-start;" id="botoneraPP">  
                            <asp:Button ID="btnPPModal" runat="server" Text="Vincular a Política Pública"  CssClass="btn btn-primary" OnClick="btnPPModal_Click"/>
                         
                           <asp:Button ID="btnRegresaSubs" runat="server" Text="Cancelar/Regresar" CssClass="btn btn-warning"  OnClick="btnRegresaSubs_Click" />
                           </div>
                   </div>
            <dx:ASPxGridView ID="gvSubproductosPP" runat="server" Theme="Office2010Blue" Width="100%"   KeyFieldName="SPPSUB$ID_SUBPRODUCTO"  SettingsPager-Mode="ShowAllRecords" OnHtmlRowPrepared ="gvSubproductosPP_HtmlRowPrepared" OnDetailRowExpandedChanged="gvSubproductosPP_DetailRowExpandedChanged" SettingsDetail-AllowOnlyOneMasterRowExpanded="true"  OnSelectionChanged="gvSubproductosPP_SelectionChanged">
             <Settings ShowFooter="True"/>                           
                            <SettingsLoadingPanel Text="Cargando&amp;hellip;" />

                                <Settings ShowFooter="true" />
                                <ClientSideEvents EndCallback="function(s,e)
                                {
                                    if(s.cpError)
                                    {
                                        //alert(s.cpError);
                                        alertify.success(s.cpError);
                                        delete s.cpError;
                                    }             
                                }           
                              " />
             <SettingsContextMenu Enabled="True">
             </SettingsContextMenu>
             <SettingsPager AlwaysShowPager="True" PageSize="200" ShowSeparators="True">
                 <PageSizeItemSettings Visible="True">
                 </PageSizeItemSettings>
             </SettingsPager>
             <Settings ShowFilterRow="True" ShowGroupPanel="True" />
             <SettingsBehavior AllowFocusedRow="True" ConfirmDelete="True" />
               
             <SettingsCommandButton DeleteButton-Text="Quitar" DeleteButton-ButtonType="Button">
                                        <UpdateButton ButtonType="Button">
                                            <Styles>
                                                <Style CssClass="btn-warging">
                                                </Style>
                                            </Styles>
                                        </UpdateButton>
                                        <CancelButton ButtonType="Button">
                                            <Styles>
                                                <FocusRectStyle CssClass="btn-danger">
                                                </FocusRectStyle>
                                            </Styles>
                                        </CancelButton>
                                        <EditButton ButtonType="Button" Text="Editar">
                                        </EditButton>
                                        <DeleteButton ButtonType="Button">
                                           
                                        </DeleteButton>
                                    </SettingsCommandButton>
             <SettingsDataSecurity AllowDelete="true" AllowEdit="true" AllowInsert="False" />
             <SettingsLoadingPanel Mode="ShowOnStatusBar" />
             <SettingsText CommandApplyFilter="Aplicar filtro" CommandApplySearchPanelFilter="asdasfasdfasdf" ContextMenuHideColumn="Ocultar Columna" ContextMenuShowCustomizationWindow="Mostrar Columnas" ContextMenuShowFilterRow="Fila de Filtros" ContextMenuShowFilterRowMenu="Seleccione Tipo de Filtro" ContextMenuShowSearchPanel="Buscar" CustomizationWindowCaption="Columnas Ocultas" GroupPanel="Arrastre las columnas que desee agrupar" />    
            <Columns>
                 <dx:GridViewCommandColumn ShowSelectCheckbox="True" SelectAllCheckboxMode="Page" Width="4%" VisibleIndex="0">
                                </dx:GridViewCommandColumn>
                <dx:GridViewDataTextColumn FieldName="SPPRO$ID_PRODUCTO" Name="SPPRO$ID_PRODUCTO" VisibleIndex="0" Visible="false">
                                       <HeaderStyle Wrap="True"></HeaderStyle>
                 </dx:GridViewDataTextColumn>

                   <dx:GridViewDataTextColumn FieldName="PRODUCTO" Name="PRODUCTO" Caption="PRODUCTO"   VisibleIndex="1" Visible="true">
                                       <HeaderStyle Wrap="True"></HeaderStyle>
                 </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="SPPSUB$ID_SUBPRODUCTO" Name="SPPSUB$ID_SUBPRODUCTO"   VisibleIndex="2" Visible="false">
                                       <HeaderStyle Wrap="True"></HeaderStyle>
                 </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="SUBPRODUCTO" Name="SUBPRODUCTO" Caption="SUBPRODUCTO"   VisibleIndex="3" Visible="true">
                                       <HeaderStyle Wrap="True"></HeaderStyle>
                       <Settings AutoFilterCondition="Contains" />
                     <Settings />
                 </dx:GridViewDataTextColumn>

                   <dx:GridViewDataTextColumn FieldName="CONTEO" Name="CONTEO" Caption="No. POLÍTICAS VINCULADAS"   VisibleIndex="4" Visible="true">
                                       <HeaderStyle Wrap="True"></HeaderStyle>
                       <Settings AutoFilterCondition="Contains" />
                     <Settings />
                 </dx:GridViewDataTextColumn>                
                </Columns>                 
                 <SettingsDetail ShowDetailRow="true" />
                                      <Templates>
                                          <DetailRow>
                                               <dx:ASPxGridView ID="gvDetallePolitica"  KeyFieldName="SPS$ID" SettingsBehavior-AllowFocusedRow ="true"  runat="server"  Width="30%" AutoGenerateColumns="False"    Theme="Office2010Blue"  OnBeforePerformDataSelect ="gvDetallePolitica_BeforePerformDataSelect"    OnRowDeleting="gvDetallePolitica_RowDeleting">
                                                      <Settings ShowFooter="True"/>                           
                            <SettingsLoadingPanel Text="Cargando&amp;hellip;" />

                                <Settings ShowFooter="true" />
                                <ClientSideEvents EndCallback="function(s,e)
                                {
                                    if(s.cpError)
                                    {
                                        //alert(s.cpError);
                                        alertify.success(s.cpError);
                                        delete s.cpError;
                                    }             
                                }           
                              " />
             <SettingsContextMenu Enabled="True">
             </SettingsContextMenu>
             <SettingsPager AlwaysShowPager="True" PageSize="200" ShowSeparators="True">
                 <PageSizeItemSettings Visible="True">
                 </PageSizeItemSettings>
             </SettingsPager>
             <Settings ShowFilterRow="True" ShowGroupPanel="True" />
             <SettingsBehavior AllowFocusedRow="True" ConfirmDelete="false" />
             <SettingsCommandButton DeleteButton-Text="Desvincular" DeleteButton-ButtonType="Button">
                                        <UpdateButton ButtonType="Button">
                                            <Styles>
                                                <Style CssClass="btn-warging">
                                                </Style>
                                            </Styles>
                                        </UpdateButton>
                                        <CancelButton ButtonType="Button">
                                            <Styles>
                                                <FocusRectStyle CssClass="btn-danger">
                                                </FocusRectStyle>
                                            </Styles>
                                        </CancelButton>
                                        <EditButton ButtonType="Button" Text="Editar">
                                        </EditButton>
                                        <DeleteButton ButtonType="Button">
                                           
                                        </DeleteButton>
                                    </SettingsCommandButton>
             <SettingsDataSecurity AllowDelete="true" AllowEdit="true" AllowInsert="False" />
             <SettingsLoadingPanel Mode="ShowOnStatusBar" />
             <SettingsText CommandApplyFilter="Aplicar filtro" CommandApplySearchPanelFilter="asdasfasdfasdf" ContextMenuHideColumn="Ocultar Columna" ContextMenuShowCustomizationWindow="Mostrar Columnas" ContextMenuShowFilterRow="Fila de Filtros" ContextMenuShowFilterRowMenu="Seleccione Tipo de Filtro" ContextMenuShowSearchPanel="Buscar" CustomizationWindowCaption="Columnas Ocultas" GroupPanel="Arrastre las columnas que desee agrupar" />    
                                               <Columns>
                  <dx:GridViewCommandColumn ShowDeleteButton="true" ShowEditButton="false" ShowNewButtonInHeader="false" VisibleIndex="0">
                     <HeaderStyle Wrap="True" />
                 </dx:GridViewCommandColumn>

                                                    <dx:GridViewDataTextColumn FieldName="SPS$ID" Name="SPS$ID"   VisibleIndex="1" Visible="false">
                                       <HeaderStyle Wrap="True"></HeaderStyle>
                 </dx:GridViewDataTextColumn>
                                                    <dx:GridViewDataTextColumn FieldName="POLITICA" Name="POLITICA" Caption="POLÍTICA"   VisibleIndex="2" Visible="true">
                                       <HeaderStyle Wrap="True"></HeaderStyle>
                 </dx:GridViewDataTextColumn>
                                                     <dx:GridViewDataTextColumn FieldName="LINEAMIENTO" Name="LINEAMIENTO" Caption="LINEAMIENTO"   VisibleIndex="3" Visible="true">
                                       <HeaderStyle Wrap="True"></HeaderStyle>
                       <Settings AutoFilterCondition="Contains" />
                     <Settings />
                 </dx:GridViewDataTextColumn>

                                                   <dx:GridViewDataTextColumn FieldName="LINEA_ACCION" Name="LINEA_ACCION" Caption="LÍNEA DE ACCIÓN"   VisibleIndex="4" Visible="true">
                                       <HeaderStyle Wrap="True"></HeaderStyle>
                       <Settings AutoFilterCondition="Contains" />
                     <Settings />
                 </dx:GridViewDataTextColumn>

                                                   
                                               </Columns>
                                               </dx:ASPxGridView>
                                </DetailRow>
                                 </Templates>
                 

            </dx:ASPxGridView>
            </asp:View>

                           

        </asp:MultiView>

    <dx:ASPxPopupControl ID="popPGG2428" runat="server" AllowDragging ="true" Width ="900px" Height="400px" HeaderText="Vinculación con RI/PGG 2024-2028" PopupHorizontalAlign="WindowCenter"  PopupVerticalAlign="WindowCenter"  CloseAction="CloseButton"  >
                                <HeaderStyle Font-Bold="False" Font-Size="10"  HorizontalAlign="Left" />
                               <ContentCollection>
                                   <dx:PopupControlContentControl>                                   

                                       
                                       <asp:Button ID="btnVinculaPGG" runat="server" Text="Vincular RI/PGG" CssClass="btn btn-primary" OnClick="btnVinculaPGG_Click"/>
             <asp:Button ID="btnCancelaPGG" runat="server" Text="Cancelar" CssClass="btn btn-warning" OnClick="btnCancelaPGG_Click"/>

                                         <h3 style="color:#2d572c">Seleccione el resultado institucional</h3>

                                       <dx:ASPxComboBox ID="cbResultados_tempo" runat="server" ValueType="System.String" NullText="Seleccione resultado" CssClass="form-control"></dx:ASPxComboBox>


                                       <h3 style="color:#2d572c">Si necesita vincular los productos a la PGG 2024-2028, seleccione la acción en el listado de abajo</h3>

                                       <dx:ASPxGridView ID="gvPGG2428" runat="server" Theme="Office2010Blue" Width="100%"   KeyFieldName="ID_ACCION"  SettingsPager-Mode="ShowAllRecords" >
             <Settings ShowFooter="True"/>                           
                            <SettingsLoadingPanel Text="Cargando&amp;hellip;" />

                                <Settings ShowFooter="true" />
                                <ClientSideEvents EndCallback="function(s,e)
                                {
                                    if(s.cpError)
                                    {
                                        //alert(s.cpError);
                                        alertify.success(s.cpError);
                                        delete s.cpError;
                                    }             
                                }           
                              " />
             <SettingsContextMenu Enabled="True">
             </SettingsContextMenu>
             <SettingsPager AlwaysShowPager="True" PageSize="200" ShowSeparators="True">
                 <PageSizeItemSettings Visible="True">
                 </PageSizeItemSettings>
             </SettingsPager>
             <Settings ShowFilterRow="True" ShowGroupPanel="True" />
             <SettingsBehavior AllowFocusedRow="True" ConfirmDelete="True" />
             <SettingsCommandButton DeleteButton-Text="Quitar" DeleteButton-ButtonType="Button">
                                        <UpdateButton ButtonType="Button">
                                            <Styles>
                                                <Style CssClass="btn-warging">
                                                </Style>
                                            </Styles>
                                        </UpdateButton>
                                        <CancelButton ButtonType="Button">
                                            <Styles>
                                                <FocusRectStyle CssClass="btn-danger">
                                                </FocusRectStyle>
                                            </Styles>
                                        </CancelButton>
                                        <EditButton ButtonType="Button" Text="Editar">
                                        </EditButton>
                                        <DeleteButton ButtonType="Button">
                                           
                                        </DeleteButton>
                                    </SettingsCommandButton>
             <SettingsDataSecurity AllowDelete="true" AllowEdit="true" AllowInsert="False" />
             <SettingsLoadingPanel Mode="ShowOnStatusBar" />
             <SettingsText CommandApplyFilter="Aplicar filtro" CommandApplySearchPanelFilter="asdasfasdfasdf" ContextMenuHideColumn="Ocultar Columna" ContextMenuShowCustomizationWindow="Mostrar Columnas" ContextMenuShowFilterRow="Fila de Filtros" ContextMenuShowFilterRowMenu="Seleccione Tipo de Filtro" ContextMenuShowSearchPanel="Buscar" CustomizationWindowCaption="Columnas Ocultas" GroupPanel="Arrastre las columnas que desee agrupar" />    
            <Columns>
                 
                <dx:GridViewDataTextColumn FieldName="ID_ACCION" Name="ID_ACCION" Caption="PERIODO"   VisibleIndex="0" Visible="false">
                                       <HeaderStyle Wrap="True"></HeaderStyle>
                 </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="EJE" Name="EJE" Caption="EJE ESTRATÉGICO"   VisibleIndex="1" Visible="true" GroupIndex="0">
                                       <HeaderStyle Wrap="True"></HeaderStyle>
                 </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="META" Name="META" Caption="META PRESIDENCIAL"   VisibleIndex="2" Visible="true" GroupIndex="1">
                                       <HeaderStyle Wrap="True"></HeaderStyle>
                 </dx:GridViewDataTextColumn>     
                 <dx:GridViewDataTextColumn FieldName="ACCION_ESTRATEGICA" Name="ACCION_ESTRATEGICA" Caption="ACCIÓN ESTRATÉGICA"   VisibleIndex="3" Visible="true" >
                                       <HeaderStyle Wrap="True"></HeaderStyle>
                 </dx:GridViewDataTextColumn>    
               
               
                </Columns>
            </dx:ASPxGridView>
            
            </dx:PopupControlContentControl>
                                   </ContentCollection>
                           </dx:ASPxPopupControl>



    <dx:ASPxPopupControl ID="pop1eje" runat="server" AllowDragging ="true" Width ="900px" Height="400px" HeaderText="Vinculación con PGG 2024-2028" PopupHorizontalAlign="WindowCenter"  PopupVerticalAlign="WindowCenter"  CloseAction="CloseButton"  >
                                <HeaderStyle Font-Bold="False" Font-Size="10"  HorizontalAlign="Left" />
                               <ContentCollection>
                                   <dx:PopupControlContentControl>
                                         <div class="form-group">
                                       <asp:Button ID="btn1eje" runat="server" Text="Vincular PGG" CssClass="btn btn-primary" OnClick="btn1eje_Click"/>
             <asp:Button ID="btnCancela1eje" runat="server" Text="Cancelar" CssClass="btn btn-warning"  OnClick="btnCancela1eje_Click"/>
                                      
                            
                              </div> 
                                       <h3 style="color:#2d572c">Seleccione el resultado institucional</h3>
                                       <dx:ASPxComboBox ID="cbResto1Eje" runat="server" ValueType="System.String" NullText="Seleccione resultado" CssClass="form-control"></dx:ASPxComboBox>
                                       <h3 style="color:#2d572c">Si necesita vincular los productos a la PGG 2024-2028, seleccione el eje estratégico en el listado de abajo"</h3>
                                       <dx:ASPxGridView ID="gv1eje" runat="server" Theme="Office2010Blue" Width="100%"   KeyFieldName="ID_EJE"  SettingsPager-Mode="ShowAllRecords" >
             <Settings ShowFooter="True"/>                           
                            <SettingsLoadingPanel Text="Cargando&amp;hellip;" />

                                <Settings ShowFooter="true" />
                                <ClientSideEvents EndCallback="function(s,e)
                                {
                                    if(s.cpError)
                                    {
                                        //alert(s.cpError);
                                        alertify.success(s.cpError);
                                        delete s.cpError;
                                    }             
                                }           
                              " />
             <SettingsContextMenu Enabled="True">
             </SettingsContextMenu>
             <SettingsPager AlwaysShowPager="True" PageSize="200" ShowSeparators="True">
                 <PageSizeItemSettings Visible="True">
                 </PageSizeItemSettings>
             </SettingsPager>
             <Settings ShowFilterRow="True" ShowGroupPanel="True" />
             <SettingsBehavior AllowFocusedRow="True" ConfirmDelete="True" />
             <SettingsCommandButton DeleteButton-Text="Quitar" DeleteButton-ButtonType="Button">
                                        <UpdateButton ButtonType="Button">
                                            <Styles>
                                                <Style CssClass="btn-warging">
                                                </Style>
                                            </Styles>
                                        </UpdateButton>
                                        <CancelButton ButtonType="Button">
                                            <Styles>
                                                <FocusRectStyle CssClass="btn-danger">
                                                </FocusRectStyle>
                                            </Styles>
                                        </CancelButton>
                                        <EditButton ButtonType="Button" Text="Editar">
                                        </EditButton>
                                        <DeleteButton ButtonType="Button">
                                           
                                        </DeleteButton>
                                    </SettingsCommandButton>
             <SettingsDataSecurity AllowDelete="true" AllowEdit="true" AllowInsert="False" />
             <SettingsLoadingPanel Mode="ShowOnStatusBar" />
             <SettingsText CommandApplyFilter="Aplicar filtro" CommandApplySearchPanelFilter="asdasfasdfasdf" ContextMenuHideColumn="Ocultar Columna" ContextMenuShowCustomizationWindow="Mostrar Columnas" ContextMenuShowFilterRow="Fila de Filtros" ContextMenuShowFilterRowMenu="Seleccione Tipo de Filtro" ContextMenuShowSearchPanel="Buscar" CustomizationWindowCaption="Columnas Ocultas" GroupPanel="Arrastre las columnas que desee agrupar" />    
            <Columns>
                 
                <dx:GridViewDataTextColumn FieldName="ID_EJE" Name="ID_EJE"    VisibleIndex="0" Visible="false">
                                       <HeaderStyle Wrap="True"></HeaderStyle>
                 </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="EJE" Name="EJE" Caption="EJE ESTRATÉGICO"   VisibleIndex="1" Visible="true">
                                       <HeaderStyle Wrap="True"></HeaderStyle>
                 </dx:GridViewDataTextColumn>
                
               
                </Columns>
            </dx:ASPxGridView>
            
            </dx:PopupControlContentControl>
                                   </ContentCollection>
                           </dx:ASPxPopupControl>



<%--    <dx:ASPxPopupControl ID="popResultadosTempo" runat="server" AllowDragging ="true" Width ="900px" Height="400px" HeaderText="Vincular productos con resultados institucionales" PopupHorizontalAlign="WindowCenter"  PopupVerticalAlign="WindowCenter"  CloseAction="CloseButton"  >
                                <HeaderStyle Font-Bold="False" Font-Size="10"  HorizontalAlign="Left" />
                               <ContentCollection>
                                   <dx:PopupControlContentControl>
                                        <h3 style="color:#2d572c">Seleccione el resultado institucional a vincular, luego presione el botón "Vincular con resultado"</h3>
                                       <asp:Button ID="btnVincRtempo" runat="server" Text="Vincular con resultado" CssClass="btn btn-primary" OnClick="btnVincRtempo_Click"/>
                                       <div class="form-group">
                           <label for="txtsubCod" aria-required="true"><font color="red">*</font>Está por vincular los productos marcados a un resultado institucional, algunos pueden estar vinculados a la PGG 2024-2028, los productos pueden vincularse a la PGG 2024-2028 y resultados institucionales ¿Necesita que estos productos queden desvinculados de la PGG 2024-2028?</label>
                              <dx:ASPxRadioButtonList ID="cbDesvincularestos" runat="server"
                                        ValueField="ID" TextField="Name2" RepeatColumns="2" RepeatLayout="Table"   Border-BorderStyle="None" >
                                       
                                         <CaptionSettings Position="Top" />
                                        <Items>
                                            <dx:ListEditItem Text="No" Value="0"   />
                                            <dx:ListEditItem Text="Sí" Value="1" />
                                         
                                        </Items>
                                       </dx:ASPxRadioButtonList>
                              </div> 
             <asp:Button ID="btnCancelaRtempo" runat="server" Text="Cancelar" CssClass="btn btn-warning" OnClick="btnCancelaPGG_Click"/>
               
            <dx:ASPxComboBox ID="cbResultados_tempo" runat="server" ValueType="System.String" NullText="Seleccione resultado" CssClass="form-control"></dx:ASPxComboBox>
            </dx:PopupControlContentControl>
                                   </ContentCollection>
                           </dx:ASPxPopupControl>--%>


    <dx:ASPxPopupControl ID="popRed" runat="server" AllowDragging ="true" Width ="900px" Height="400px" HeaderText="Vincular productos a Resultado Estratégicos de Desarrollo" PopupHorizontalAlign="WindowCenter"  PopupVerticalAlign="WindowCenter"  CloseAction="CloseButton"  >
                                <HeaderStyle Font-Bold="False" Font-Size="10"  HorizontalAlign="Left" />
                               <ContentCollection>
                                   <dx:PopupControlContentControl>                                                                              
                            
                                       <asp:Button ID="btnVincRED" runat="server" Text="Vincular con RE" CssClass="btn btn-primary" OnClick="btnVincRED_Click" />
                                      
             <asp:Button ID="btnCancelaRED" runat="server" Text="Cancelar" CssClass="btn btn-warning" OnClick="btnCancelaRED_Click"/>
                                       <h3 style="color:#2d572c">Seleccione el Resultado Estratégico a vincular</h3>               
            <dx:ASPxComboBox ID="cbRedTempo" runat="server" ValueType="System.String" NullText="Seleccione RE" CssClass="form-control"></dx:ASPxComboBox>
                                                <h3 style="color:#2d572c">Si necesita vincular los productos a la PGG 2024-2028, seleccione la acción en el listado de abajo</h3>

                                       <dx:ASPxGridView ID="gv2428red" runat="server" Theme="Office2010Blue" Width="100%"   KeyFieldName="ID_ACCION"  SettingsPager-Mode="ShowAllRecords" >
             <Settings ShowFooter="True"/>                           
                            <SettingsLoadingPanel Text="Cargando&amp;hellip;" />

                                <Settings ShowFooter="true" />
                                <ClientSideEvents EndCallback="function(s,e)
                                {
                                    if(s.cpError)
                                    {
                                        //alert(s.cpError);
                                        alertify.success(s.cpError);
                                        delete s.cpError;
                                    }             
                                }           
                              " />
             <SettingsContextMenu Enabled="True">
             </SettingsContextMenu>
             <SettingsPager AlwaysShowPager="True" PageSize="200" ShowSeparators="True">
                 <PageSizeItemSettings Visible="True">
                 </PageSizeItemSettings>
             </SettingsPager>
             <Settings ShowFilterRow="True" ShowGroupPanel="True" />
             <SettingsBehavior AllowFocusedRow="True" ConfirmDelete="True" />
             <SettingsCommandButton DeleteButton-Text="Quitar" DeleteButton-ButtonType="Button">
                                        <UpdateButton ButtonType="Button">
                                            <Styles>
                                                <Style CssClass="btn-warging">
                                                </Style>
                                            </Styles>
                                        </UpdateButton>
                                        <CancelButton ButtonType="Button">
                                            <Styles>
                                                <FocusRectStyle CssClass="btn-danger">
                                                </FocusRectStyle>
                                            </Styles>
                                        </CancelButton>
                                        <EditButton ButtonType="Button" Text="Editar">
                                        </EditButton>
                                        <DeleteButton ButtonType="Button">
                                           
                                        </DeleteButton>
                                    </SettingsCommandButton>
             <SettingsDataSecurity AllowDelete="true" AllowEdit="true" AllowInsert="False" />
             <SettingsLoadingPanel Mode="ShowOnStatusBar" />
             <SettingsText CommandApplyFilter="Aplicar filtro" CommandApplySearchPanelFilter="asdasfasdfasdf" ContextMenuHideColumn="Ocultar Columna" ContextMenuShowCustomizationWindow="Mostrar Columnas" ContextMenuShowFilterRow="Fila de Filtros" ContextMenuShowFilterRowMenu="Seleccione Tipo de Filtro" ContextMenuShowSearchPanel="Buscar" CustomizationWindowCaption="Columnas Ocultas" GroupPanel="Arrastre las columnas que desee agrupar" />    
            <Columns>
                 
                <dx:GridViewDataTextColumn FieldName="ID_ACCION" Name="ID_ACCION" Caption="PERIODO"   VisibleIndex="0" Visible="false">
                                       <HeaderStyle Wrap="True"></HeaderStyle>
                 </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="EJE" Name="EJE" Caption="EJE ESTRATÉGICO"   VisibleIndex="1" Visible="true" GroupIndex="0">
                                       <HeaderStyle Wrap="True"></HeaderStyle>
                 </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="META" Name="META" Caption="META PRESIDENCIAL"   VisibleIndex="2" Visible="true" GroupIndex="1">
                                       <HeaderStyle Wrap="True"></HeaderStyle>
                 </dx:GridViewDataTextColumn>     
                 <dx:GridViewDataTextColumn FieldName="ACCION_ESTRATEGICA" Name="ACCION_ESTRATEGICA" Caption="ACCIÓN ESTRATÉGICA"   VisibleIndex="3" Visible="true" >
                                       <HeaderStyle Wrap="True"></HeaderStyle>
                 </dx:GridViewDataTextColumn>    
               
               
                </Columns>
            </dx:ASPxGridView>
            </dx:PopupControlContentControl>
                                   </ContentCollection>
                           </dx:ASPxPopupControl>

    

   


    <dx:ASPxPopupControl ID="popRed1eje" runat="server" AllowDragging ="true" Width ="900px" Height="400px" HeaderText="Vincular productos a Resultado Estratégicos de Desarrollo" PopupHorizontalAlign="WindowCenter"  PopupVerticalAlign="WindowCenter"  CloseAction="CloseButton"  >
                                <HeaderStyle Font-Bold="False" Font-Size="10"  HorizontalAlign="Left" />
                               <ContentCollection>
                                   <dx:PopupControlContentControl>                                                                              
                            
                                       <asp:Button ID="btnRed1eje" runat="server" Text="Vincular con RE" CssClass="btn btn-primary"  OnClick="btnRed1eje_Click"/>
                                      
             <asp:Button ID="btnCancelaRed1" runat="server" Text="Cancelar" CssClass="btn btn-warning" OnClick="btnCancelaRed1_Click"/>
                                       <h3 style="color:#2d572c">Seleccione el Resultado Estratégico a vincular</h3>               
            <dx:ASPxComboBox ID="cbRed1EJE" runat="server" ValueType="System.String" NullText="Seleccione RE" CssClass="form-control"></dx:ASPxComboBox>
                                                <h3 style="color:#2d572c">Si necesita vincular los productos a la PGG 2024-2028, seleccione la acción en el listado de abajo</h3>

                              <dx:ASPxGridView ID="gvRed1Eeje" runat="server" Theme="Office2010Blue" Width="100%"   KeyFieldName="ID_EJE"  SettingsPager-Mode="ShowAllRecords" >
             <Settings ShowFooter="True"/>                           
                            <SettingsLoadingPanel Text="Cargando&amp;hellip;" />

                                <Settings ShowFooter="true" />
                                <ClientSideEvents EndCallback="function(s,e)
                                {
                                    if(s.cpError)
                                    {
                                        //alert(s.cpError);
                                        alertify.success(s.cpError);
                                        delete s.cpError;
                                    }             
                                }           
                              " />
             <SettingsContextMenu Enabled="True">
             </SettingsContextMenu>
             <SettingsPager AlwaysShowPager="True" PageSize="200" ShowSeparators="True">
                 <PageSizeItemSettings Visible="True">
                 </PageSizeItemSettings>
             </SettingsPager>
             <Settings ShowFilterRow="True" ShowGroupPanel="True" />
             <SettingsBehavior AllowFocusedRow="True" ConfirmDelete="True" />
             <SettingsCommandButton DeleteButton-Text="Quitar" DeleteButton-ButtonType="Button">
                                        <UpdateButton ButtonType="Button">
                                            <Styles>
                                                <Style CssClass="btn-warging">
                                                </Style>
                                            </Styles>
                                        </UpdateButton>
                                        <CancelButton ButtonType="Button">
                                            <Styles>
                                                <FocusRectStyle CssClass="btn-danger">
                                                </FocusRectStyle>
                                            </Styles>
                                        </CancelButton>
                                        <EditButton ButtonType="Button" Text="Editar">
                                        </EditButton>
                                        <DeleteButton ButtonType="Button">
                                           
                                        </DeleteButton>
                                    </SettingsCommandButton>
             <SettingsDataSecurity AllowDelete="true" AllowEdit="true" AllowInsert="False" />
             <SettingsLoadingPanel Mode="ShowOnStatusBar" />
             <SettingsText CommandApplyFilter="Aplicar filtro" CommandApplySearchPanelFilter="asdasfasdfasdf" ContextMenuHideColumn="Ocultar Columna" ContextMenuShowCustomizationWindow="Mostrar Columnas" ContextMenuShowFilterRow="Fila de Filtros" ContextMenuShowFilterRowMenu="Seleccione Tipo de Filtro" ContextMenuShowSearchPanel="Buscar" CustomizationWindowCaption="Columnas Ocultas" GroupPanel="Arrastre las columnas que desee agrupar" />    
            <Columns>
                 
                <dx:GridViewDataTextColumn FieldName="ID_EJE" Name="ID_EJE"    VisibleIndex="0" Visible="false">
                                       <HeaderStyle Wrap="True"></HeaderStyle>
                 </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="EJE" Name="EJE" Caption="EJE ESTRATÉGICO"   VisibleIndex="1" Visible="true">
                                       <HeaderStyle Wrap="True"></HeaderStyle>
                 </dx:GridViewDataTextColumn>
                
               
                </Columns>
            </dx:ASPxGridView>
            </dx:PopupControlContentControl>
                                   </ContentCollection>
                           </dx:ASPxPopupControl>
    
       
      <dx:ASPxPopupControl ID="Confirma" runat="server" AllowDragging ="true" Width ="900px" Height="400px" HeaderText="Confirmar migrar información estructura de programación" PopupHorizontalAlign="WindowCenter"  PopupVerticalAlign="WindowCenter"  CloseAction="CloseButton"  ClientInstanceName="popupCommon">
                                <HeaderStyle Font-Bold="False" Font-Size="10"  HorizontalAlign="Left" />
                               <ContentCollection>
                                   <dx:PopupControlContentControl>
                                        <h3 style="color:#2d572c"><asp:Label ID="Confirmamigracion" runat="server" /></h3>
                                       <asp:Button ID="btnConfirma" runat="server" Text="Si" CssClass="btn btn-primary" OnClick="btnConfirma_Click" />
             <asp:Button ID="btnConfirmaCancela" runat="server" Text="No" CssClass="btn btn-warning"  OnClick="btnConfirmaCancela_Click"/>
               
            
            </dx:PopupControlContentControl>
                                   </ContentCollection>
                           </dx:ASPxPopupControl>


      <dx:ASPxPopupControl ID="popPoliticas" runat="server" AllowDragging ="true" Width ="700px" Height="300px" HeaderText="Vinculación con Polítca(s) Pública(s)" PopupHorizontalAlign="WindowCenter"  PopupVerticalAlign="WindowCenter"  CloseAction="CloseButton"  ClientInstanceName="popupCommon">
                                <HeaderStyle Font-Bold="False" Font-Size="10"  HorizontalAlign="Left" />
                               <ContentCollection>
                                   <dx:PopupControlContentControl>
                                        <h3 style="color:#2d572c">Políticas públicas</h3>
                                       <dx:ASPxGridView ID="gvPoliticasLA" runat="server" Theme="Office2010Blue" Width="100%"   KeyFieldName="ID_LINEA_ACCION"  SettingsPager-Mode="ShowAllRecords" AutoGenerateColumns="false" OnCustomColumnSort ="gvPoliticasLA_CustomColumnSort">
             <Settings ShowFooter="True"/> 
                            <SettingsLoadingPanel Text="Cargando&amp;hellip;" />

                                <Settings ShowFooter="true" />
                                <ClientSideEvents EndCallback="function(s,e)
                                {
                                    if(s.cpError)
                                    {
                                        //alert(s.cpError);
                                        alertify.success(s.cpError);
                                        delete s.cpError;
                                    }             
                                }           
                              " />
             <SettingsContextMenu Enabled="True">
             </SettingsContextMenu>
             <SettingsPager AlwaysShowPager="True" PageSize="2" ShowSeparators="True">
                 <PageSizeItemSettings Visible="True">
                 </PageSizeItemSettings>
             </SettingsPager>


             <Settings ShowFilterRow="True" ShowGroupPanel="True" />
             <SettingsBehavior AllowFocusedRow="True" ConfirmDelete="True" />
             <SettingsCommandButton DeleteButton-Text="Quitar" DeleteButton-ButtonType="Button">
                                        <UpdateButton ButtonType="Button">
                                            <Styles>
                                                <Style CssClass="btn-warging">
                                                </Style>
                                            </Styles>
                                        </UpdateButton>
                                        <CancelButton ButtonType="Button">
                                            <Styles>
                                                <FocusRectStyle CssClass="btn-danger">
                                                </FocusRectStyle>
                                            </Styles>
                                        </CancelButton>
                                        <EditButton ButtonType="Button" Text="Editar">
                                        </EditButton>
                                        <DeleteButton ButtonType="Button">
                                           
                                        </DeleteButton>
                                    </SettingsCommandButton>
             <SettingsDataSecurity AllowDelete="true" AllowEdit="true" AllowInsert="False" />
             <SettingsLoadingPanel Mode="ShowOnStatusBar" />
             <SettingsText CommandApplyFilter="Aplicar filtro" CommandApplySearchPanelFilter="asdasfasdfasdf" ContextMenuHideColumn="Ocultar Columna" ContextMenuShowCustomizationWindow="Mostrar Columnas" ContextMenuShowFilterRow="Fila de Filtros" ContextMenuShowFilterRowMenu="Seleccione Tipo de Filtro" ContextMenuShowSearchPanel="Buscar" CustomizationWindowCaption="Columnas Ocultas"  />    
            <Columns>
                 <dx:GridViewCommandColumn ShowSelectCheckbox="True" SelectAllCheckboxMode="Page" Width="4%" VisibleIndex="0">
                                </dx:GridViewCommandColumn>
                <dx:GridViewDataTextColumn FieldName="ID_LINEA_ACCION" Name="ID_LINEA_ACCION"  VisibleIndex="0" Visible="false">
                                       <HeaderStyle Wrap="True"></HeaderStyle>
                 </dx:GridViewDataTextColumn>

                   <dx:GridViewDataTextColumn FieldName="SPPLE$ID" Name="SPPLE$ID" VisibleIndex="1" Visible="false">
                                       <HeaderStyle Wrap="True"></HeaderStyle>
                 </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="POLITICA" Name="POLITICA" Caption="POLÍTICA PÚBLICA"   VisibleIndex="2" Visible="true" GroupIndex="0" Width="10%">
                                       <HeaderStyle Wrap="True"></HeaderStyle>
                 </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="LINEAMIENTO" Name="LINEAMIENTO" Caption="LINEAMIENTO"   VisibleIndex="3" Visible="true" GroupIndex="1" Width="10%">
                                       <HeaderStyle Wrap="True"></HeaderStyle>
                       <Settings AutoFilterCondition="Contains" />
                     <Settings />
                 </dx:GridViewDataTextColumn>

                   <dx:GridViewDataTextColumn FieldName="LINEA_ACCION" Name="LINEA_ACCION" Caption="LÍNEA DE ACCIÓN"   VisibleIndex="4" Visible="true" Width="60%">
                                       <HeaderStyle Wrap="True"></HeaderStyle>
                       <Settings AutoFilterCondition="Contains" />
                     <Settings />
                 </dx:GridViewDataTextColumn>

                <dx:GridViewDataTextColumn FieldName="SPPLE$ORDEN" Name="SPPLE$ORDEN" VisibleIndex="5" Visible="false">
                                       <HeaderStyle Wrap="True"></HeaderStyle>
                 </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="SPPLI$ORDEN" Name="SPPLI$ORDEN"   VisibleIndex="6" Visible="false">
                                       <HeaderStyle Wrap="True"></HeaderStyle>
                 </dx:GridViewDataTextColumn>
               
                </Columns>
            </dx:ASPxGridView>

                                     
                                       <br />
                                       <br />
                                       <asp:Button ID="btnVinculaPolitica" runat="server" Text="Guardar" CssClass="btn btn-primary" OnClick="btnVinculaPolitica_Click" />
             <asp:Button ID="btnCancelaVincula" runat="server" Text="Cancelar" CssClass="btn btn-warning"  OnClick="btnCancelaVincula_Click"/>
               
            
            </dx:PopupControlContentControl>
                                   </ContentCollection>
                           </dx:ASPxPopupControl>



    
      <dx:ASPxPopupControl ID="popPoliticasFormulario" runat="server" AllowDragging ="true" Width ="700px" Height="300px" HeaderText="Vinculación con Polítca(s) Pública(s)" PopupHorizontalAlign="WindowCenter"  PopupVerticalAlign="WindowCenter"  CloseAction="CloseButton"  ClientInstanceName="popupCommon">
                                <HeaderStyle Font-Bold="False" Font-Size="10"  HorizontalAlign="Left" />
                               <ContentCollection>
                                   <dx:PopupControlContentControl>
                                        <h3 style="color:#2d572c">Políticas públicas</h3>
                                           <asp:Button ID="btnVincularCerrar" runat="server" Text="Guardar y cerrar" CssClass="btn btn-primary" OnClick="btnVinculayCerrar_Click" />
                                             <asp:Button ID="btnCancelaSubPoli" runat="server" Text="Cancelar" CssClass="btn btn-warning"  OnClick="btnCancelaPoliticasSUb_Click"/>
                                 <dx:ASPxGridView ID="gvDetallePoliticaSub"  KeyFieldName="ID_LINEA_ACCION" SettingsBehavior-AllowFocusedRow ="true"  runat="server" Width="100%" AutoGenerateColumns="False"    Theme="Office2010Blue">
                                                      <Settings ShowFooter="True"/>                           
                            <SettingsLoadingPanel Text="Cargando&amp;hellip;" />

                                <Settings ShowFooter="true" />
                                <ClientSideEvents EndCallback="function(s,e)
                                {
                                    if(s.cpError)
                                    {
                                        //alert(s.cpError);
                                        alertify.success(s.cpError);
                                        delete s.cpError;
                                    }             
                                }           
                              " />
             <SettingsContextMenu Enabled="True">
             </SettingsContextMenu>
             <SettingsPager AlwaysShowPager="True" PageSize="10" ShowSeparators="True">
                 <PageSizeItemSettings Visible="True">
                 </PageSizeItemSettings>
             </SettingsPager>
             <Settings ShowFilterRow="True" ShowGroupPanel="True" />
             <SettingsBehavior AllowFocusedRow="True" ConfirmDelete="false" />
             <SettingsCommandButton DeleteButton-Text="Desvincular" DeleteButton-ButtonType="Button">
                                        <UpdateButton ButtonType="Button">
                                            <Styles>
                                                <Style CssClass="btn-warging">
                                                </Style>
                                            </Styles>
                                        </UpdateButton>
                                        <CancelButton ButtonType="Button">
                                            <Styles>
                                                <FocusRectStyle CssClass="btn-danger">
                                                </FocusRectStyle>
                                            </Styles>
                                        </CancelButton>
                                        <EditButton ButtonType="Button" Text="Editar">
                                        </EditButton>
                                        <DeleteButton ButtonType="Button">
                                           
                                        </DeleteButton>
                                    </SettingsCommandButton>
             <SettingsDataSecurity AllowDelete="true" AllowEdit="true" AllowInsert="False" />
             <SettingsLoadingPanel Mode="ShowOnStatusBar" />
             <SettingsText CommandApplyFilter="Aplicar filtro" CommandApplySearchPanelFilter="asdasfasdfasdf" ContextMenuHideColumn="Ocultar Columna" ContextMenuShowCustomizationWindow="Mostrar Columnas" ContextMenuShowFilterRow="Fila de Filtros" ContextMenuShowFilterRowMenu="Seleccione Tipo de Filtro" ContextMenuShowSearchPanel="Buscar" CustomizationWindowCaption="Columnas Ocultas" GroupPanel="Arrastre las columnas que desee agrupar" />    
                                               <Columns>

                                                   <dx:GridViewCommandColumn ShowSelectCheckbox="True" SelectAllCheckboxMode="Page" Width="4%" VisibleIndex="0">
                                </dx:GridViewCommandColumn>
                <dx:GridViewDataTextColumn FieldName="ID_LINEA_ACCION" Name="ID_LINEA_ACCION"  VisibleIndex="0" Visible="false">
                                       <HeaderStyle Wrap="True"></HeaderStyle>
                 </dx:GridViewDataTextColumn>

                   <dx:GridViewDataTextColumn FieldName="SPPLE$ID" Name="SPPLE$ID" VisibleIndex="1" Visible="false">
                                       <HeaderStyle Wrap="True"></HeaderStyle>
                 </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="POLITICA" Name="POLITICA" Caption="POLÍTICA PÚBLICA"   VisibleIndex="2" Visible="true" GroupIndex="0" Width="10%">
                                       <HeaderStyle Wrap="True"></HeaderStyle>
                 </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="LINEAMIENTO" Name="LINEAMIENTO" Caption="LINEAMIENTO"   VisibleIndex="3" Visible="true" GroupIndex="1" Width="10%">
                                       <HeaderStyle Wrap="True"></HeaderStyle>
                       <Settings AutoFilterCondition="Contains" />
                     <Settings />
                 </dx:GridViewDataTextColumn>

                   <dx:GridViewDataTextColumn FieldName="LINEA_ACCION" Name="LINEA_ACCION" Caption="LÍNEA DE ACCIÓN"   VisibleIndex="4" Visible="true" Width="60%">
                                       <HeaderStyle Wrap="True"></HeaderStyle>
                       <Settings AutoFilterCondition="Contains" />
                     <Settings />
                 </dx:GridViewDataTextColumn>

                <dx:GridViewDataTextColumn FieldName="SPPLE$ORDEN" Name="SPPLE$ORDEN" VisibleIndex="5" Visible="false">
                                       <HeaderStyle Wrap="True"></HeaderStyle>
                 </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="SPPLI$ORDEN" Name="SPPLI$ORDEN"   VisibleIndex="6" Visible="false">
                                       <HeaderStyle Wrap="True"></HeaderStyle>
                 </dx:GridViewDataTextColumn>
               
                  
                                               </Columns>
                                               </dx:ASPxGridView>


            
            </dx:PopupControlContentControl>
                                   </ContentCollection>
                           </dx:ASPxPopupControl>



        <dx:ASPxPopupControl ID="poProrroga" runat="server" AllowDragging ="true" Width ="550px" Height="250px" HeaderText="Prorroga para habilitar programación multianual" PopupHorizontalAlign="WindowCenter"  PopupVerticalAlign="WindowCenter"  CloseAction="CloseButton"  >
                                <HeaderStyle Font-Bold="False" Font-Size="10"  HorizontalAlign="Left" />
                               <ContentCollection>
                                   <dx:PopupControlContentControl>       
                                       <h3 style="color:#2d572c">Fecha de prorroga de cierre de la programación multianual</h3>               
                           <label class="form-label">Periodo de programación POM: <asp:Label ID ="periodoPOM" runat ="server"></asp:Label> </label>
                                       <br />
                           <label class="form-label">Institución: <asp:Label ID ="instoPOM" runat ="server"></asp:Label> </label>
                                       <br />
                        <label class="form-label"><asp:Label ID ="lbAbierto" runat ="server"></asp:Label> </label>
                                       <br />
                                       <br /> 
                           <label class="form-label">Fecha de cierre de periodo: <asp:Label ID ="cierrePeriodo" runat ="server"></asp:Label> </label>
                          <asp:HiddenField id="PERIODO" runat="server"/>
                           <asp:HiddenField id="POM" runat="server"/>
                            <asp:HiddenField id="INSTO" runat="server"/>
                            <asp:HiddenField id="ABIERTO" runat="server"/>
                                       <br />
                        <dx:ASPxDateEdit ID="txtdateFecha" runat="server" Culture="es-ES" CssClass="form-control" Width="60%"> </dx:ASPxDateEdit>
                                       <br />
                       <label class="form-label">Seleccione fecha de prorroga:</label>
                                       <br />
                       <dx:ASPxDateEdit ID="txtFechaPorroga" runat="server" Culture="es-ES" CssClass="form-control" Width="60%"> </dx:ASPxDateEdit>
                                       <br />
                                          
                                      
                        <asp:Button ID="btnGuardaFecha" runat="server" Text="Guardar fecha prorroga" CssClass="btn btn-primary" OnClick="btnGuardaFecha_Click"/>                                      
                        <asp:Button ID="btnCancelaFecha" runat="server" Text="Cancelar" CssClass="btn btn-warning"  OnClick="btnCancelaFecha_Click"/>
                                           
            </dx:PopupControlContentControl>
                                   </ContentCollection>
                           </dx:ASPxPopupControl>

</asp:Content>
