<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Reportes.aspx.cs" Inherits="SIPLAN2._0.Reportes.Reportes" MasterPageFile="~/Site.Master" %>
<%@ Register Assembly="DevExpress.Web.v17.2, Version=17.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <script>
function Alerta(mensaje, tipo) {          
            alertify.set('notifier', 'position', 'top-right'); 
            if (tipo == 1)
                alertify.success(mensaje);
            if (tipo == 2)
                alertify.error(mensaje);          
        };  


    </script>
    
    <style>
         #contenedor {
  width: 120%;
}

#update {
  display: inline-block;
  margin-left:25px;
}

#contenedor2 {
  width: 100%;
}

#update2 {
  display: inline-block;
  margin-left:25px;
}

#update3 {
  display: inline-block;
  margin-left:25px;
}

#contenedor3 {
  width: 100%;
}

#update3 {
  display: inline-block;
  margin-left:25px;
}

#update4 {
  display: inline-block;
  margin-left:25px;
}

#contenedor4 {
  width: 100%;
}

   </style>
     <h3 style="color:blue;margin-left:10px">Modulo en construcción los reportes estarán disponibles conforme se vayan solicitando</h3>

   <div class="form-group" style="margin-left:10px">
                           <label for="txtsubCod"aria-required="true">Año: </label>
                           <dx:ASPxComboBox ID="cbanio" runat="server" ValueType="System.String" NullText="Seleccione año a reprogramar" CssClass="form-control" Width="300"  AutoPostBack="true"></dx:ASPxComboBox>
                    </div> 
                    <div class="form-group" style="margin-left:10px">
                        <label for="txtsubCod"aria-required="true">Institución: </label>
                        <dx:ASPxComboBox ID="cbInstituiciones" runat="server" ValueType="System.String" NullText="Seleccione institución" CssClass="form-control"   AutoPostBack="true"></dx:ASPxComboBox>
                    </div>
    <div class="form-group" style="margin-left:10px">
                        <label for="txtsubCod"aria-required="true">Periodo del POM: </label>
                        <dx:ASPxComboBox ID="cbPeriodo" runat="server" ValueType="System.String" NullText="Seleccione periodo del POM" CssClass="form-control" AutoPostBack="true"></dx:ASPxComboBox>
                    </div>


     <dx:ASPxPageControl ID="VistaReportes" Width="100%" runat="server" CssClass="dxtcFixed" ActiveTabIndex="0" EnableHierarchyRecreation="True"  Theme="Office2010Blue">
         <TabPages>
             <dx:TabPage Text="Reportes y consolidados para analisis">
                 <ContentCollection>
                     <dx:ContentControl ID="ContentControl1" runat="server">
     <div id="contenedor" class="row">
         <div id="update" >
         
                   <asp:Button ID="ReporteAvance" runat="server" Text="INFORME DE PROGRAMACION Y AVANCE METAS FISICAS Y FINANCIERAS"  CssClass="btn btn-primary" ToolTip="INFORME DE PROGRAMACION Y AVANCE METAS FISICAS Y FINANCIERAS"  OnClick="ReporteAvance_Click"
                       Style="font-size:0.7em"/> 
             
                   <%--<asp:Button ID="btnReporte" runat="server" Text="INFORME CUATRIMESTRAL DE AVANCE DE METAS Y CALIDAD DE GASTO"  CssClass="btn btn-SUCCESS" ToolTip="INFORME CUATRIMESTRAL DE AVANCE DE METAS Y CALIDAD DE GASTO"  OnClick="btnReporte_Click"
                       Style="font-size:0.7em"/> --%>
             <asp:Button ID="btnRegresarinicio" runat="server" Text="Ir a pagina principal"  CssClass="btn " ToolTip="Regresar a pagina de inicio"  style="color:white;background-color:#F28B08;font-size:0.7em" OnClick="btnRegresarinicio_Click" />
                  <h4>Los reportes sin filtros tardan varios minutos en generarse</h4>    
         </div>
     </div>
    <br />
      <asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex="0">
         <asp:View ID="View1" runat="server">
            <div id="contenedor1" class="row">
         <div id="update2" >
            <%--<asp:UpdatePanel ID="UpdatePanel1" runat="server">
               <ContentTemplate>--%>
             <asp:Button ID="btnSinfiltro" runat="server" Text="Generar reporte sin filtros"  CssClass="btn btn-success" ToolTip="Generar reporte sin filtros"  OnClick="btnSinfiltro_Click"
                       Style="font-size:0.7em"/>  
             <asp:Button ID="btnConFiltro" runat="server" Text="Generar reporte con filtros"  CssClass="btn btn-warning" ToolTip="Generar reporte con filtros"   OnClick="Button2_Click"
                       Style="font-size:0.7em"/>  
                   <%--</ContentTemplate>
                </asp:UpdatePanel>--%>
         </div>
                </div>
             </asp:View>
          <%--<asp:View ID="View2" runat="server">
               <dx:ASPxGridView ID="gvReporte" runat="server"   Theme="Office2010Blue"  AutoGenerateColumns="False" SettingsBehavior-AllowFocusedRow ="true" SettingsBehavior-ConfirmDelete="true" Width="100%" >
                   </dx:ASPxGridView>
              </asp:View>--%>
      </asp:MultiView>
                         </dx:ContentControl>
                     </ContentCollection>
                 </dx:TabPage>

              <dx:TabPage Text="Vinculación de productos a tematicas">
                   <ContentCollection>
                         <dx:ContentControl ID="ContentControl3" runat="server">
                              <div id="contenedor4" class="row">
         <div id="update4" >
            <%--<asp:UpdatePanel ID="UpdatePanel1" runat="server">
               <ContentTemplate>--%>
             <asp:Button ID="btnProductoaTema" runat="server" Text="Vincular un producto a temática"  CssClass="btn btn-primary" ToolTip="Vincular un producto a tematica" OnClick="btnProductoaTema_Click"   Style="font-size:0.9em"/>  
             <asp:Button ID="btnDesProductoaTema" runat="server" Text="Desvincular producto de temática"  CssClass="btn btn-danger" ToolTip="Desvincular producto de temática" OnClick="btnDesProductoaTema_Click"   Style="font-size:0.9em"/> 
             <asp:Button ID="btnVariosaTema" runat="server" Text="Vincular varios productos a temática"  CssClass="btn btn-success" ToolTip="Vincular varios productos a temática" OnClick="btnVariosaTema_Click1"  Style="font-size:0.9em"/>  
             <asp:Button ID="btnReporteTematica" runat="server" Text="Generar reporte de productos y temáticas"  CssClass="btn btn-warning" ToolTip="Generar reporte de productos y temáticas" OnClick="btnReporteTematica_Click" Style="font-size:0.9em"/>
              <asp:Button ID="btnReporteDepto" runat="server" Text="Reporte por temáticas y departamento"  CssClass="btn btn-warning" ToolTip="Reporte por temáticas y departamento"  OnClick="btnReporteDepto_Click"  Style="color:white;background-color:#979FA0;font-size:0.9em"/>  
                   <%--</ContentTemplate>
                </asp:UpdatePanel>--%>
         </div>
                </div>
                             <br />
                          <h5>Los productos en filas rojas no tienen temática vinculada, productos en filas azules tiene temática vinculada</h5>
                             <dx:ASPxGridView ID="gvProductosInsto" runat="server" KeyFieldName="SPPRO$ID_PRODUCTO"   Theme="Office2010Blue" Width="100%" AutoGenerateColumns="False" SettingsBehavior-AllowFocusedRow ="true"    EnableCallBacks="false" OnHtmlRowPrepared="gvProductosInsto_HtmlRowPrepared">
                                           
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
                     <dx:GridViewDataTextColumn FieldName="SPPO$ID_POM"  Name="SPPO$ID_POM" ReadOnly="true" Visible="false" VisibleIndex="0" Width="0">
                     <HeaderStyle Wrap="True" />
                     <Settings />
                 </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="SPPO$ID_INSTITUCION"  Name="SPPO$ID_INSTITUCION" ReadOnly="true" Visible="false" VisibleIndex="1" Width="0">
                     <HeaderStyle Wrap="True" />
                     <Settings />
                 </dx:GridViewDataTextColumn>

                    <dx:GridViewDataTextColumn FieldName="SPP$ID_PERIODO"  Name="SSPP$ID_PERIODO" ReadOnly="true" Visible="false" VisibleIndex="2" Width="0">
                     <HeaderStyle Wrap="True" />
                     <Settings />
                 </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="PROGRAMA" Caption="Programa presupuestario"  Name="PROGRAMA" ReadOnly="true" Visible="true" VisibleIndex="3" Width="500">
                     <HeaderStyle Wrap="True" />
                         <Settings AutoFilterCondition="Contains" />
                     <Settings />
                 </dx:GridViewDataTextColumn>
                     

                    <dx:GridViewDataTextColumn FieldName="SUBPROGRAMA" Caption="Subprograma presupuestario"  Name="SUBPROGRAMA" ReadOnly="true" Visible="true" VisibleIndex="4" Width="500">
                     <HeaderStyle Wrap="True" />
                     <Settings />
                         <Settings AutoFilterCondition="Contains" />
                 </dx:GridViewDataTextColumn>

                     <dx:GridViewDataTextColumn FieldName="SPPRO$ID_PRODUCTO" Name="SPPRO$ID_PRODUCTO" ReadOnly="true" Visible="false" VisibleIndex="5" Width="500">
                     <HeaderStyle Wrap="True" />
                     <Settings />
                 </dx:GridViewDataTextColumn>

                  <dx:GridViewDataTextColumn FieldName="PRODUCTO" Caption="Producto"  Name="PRODUCTO" ReadOnly="true" Visible="true" VisibleIndex="6" Width="500">
                     <HeaderStyle Wrap="True" />
                         <Settings AutoFilterCondition="Contains" />
                     <Settings />
                 </dx:GridViewDataTextColumn>

                    <dx:GridViewDataTextColumn FieldName="MEDIDA" Caption="Unidad de medida"  Name="MEDIDA" ReadOnly="true" Visible="true" VisibleIndex="7" Width="500">
                     <HeaderStyle Wrap="True" />
                         <Settings AutoFilterCondition="Contains" />
                     <Settings />
                 </dx:GridViewDataTextColumn>


                    <dx:GridViewDataTextColumn FieldName="METAFISICA"   Name="METAFISICA" ReadOnly="true" Visible="true" VisibleIndex="8" Width="500">
                     <HeaderStyle Wrap="True" />
                     <Settings />
                 </dx:GridViewDataTextColumn>

                     <dx:GridViewDataTextColumn FieldName="METAFINANCIERA"   Name="METAFINANCIERA" ReadOnly="true" Visible="true" VisibleIndex="9" Width="500">
                     <HeaderStyle Wrap="True" />
                     <Settings />
                 </dx:GridViewDataTextColumn>


                    <dx:GridViewDataTextColumn FieldName="ID_TEMATICA"   Name="ID_TEMATICA" ReadOnly="true" Visible="false" VisibleIndex="10" Width="500">
                     <HeaderStyle Wrap="True" />
                     <Settings />
                 </dx:GridViewDataTextColumn>


                     <dx:GridViewDataTextColumn FieldName="TEMATICA" Caption="Temática"  Name="TEMATICA" ReadOnly="true" Visible="true" VisibleIndex="11" Width="500">
                     <HeaderStyle Wrap="True" />
                         <Settings AutoFilterCondition="Contains" />
                     <Settings />
                 </dx:GridViewDataTextColumn>

                     <dx:GridViewDataTextColumn FieldName="ID_DET_TEMATICA"   Name="ID_DET_TEMATICA" ReadOnly="true" Visible="false" VisibleIndex="12" Width="500">
                     <HeaderStyle Wrap="True" />
                     <Settings />
                 </dx:GridViewDataTextColumn>

                     <dx:GridViewDataTextColumn FieldName="DETALLE_TEMATICA" Caption="Detalle temática"  Name="DETALLE_TEMATICA" ReadOnly="true" Visible="true" VisibleIndex="13" Width="500">
                     <HeaderStyle Wrap="True" />
                         <Settings AutoFilterCondition="Contains" />
                     <Settings />
                 </dx:GridViewDataTextColumn>
                    
                    <dx:GridViewDataTextColumn FieldName="SPOA$ID_POA"   Name="SPOA$ID_POA" ReadOnly="true" Visible="false" VisibleIndex="14" Width="500">
                     <HeaderStyle Wrap="True" />
                     <Settings />
                 </dx:GridViewDataTextColumn>

                    <dx:GridViewDataTextColumn FieldName="SPOA$ANIO"   Name="SPOA$ANIO" ReadOnly="true" Visible="false" VisibleIndex="15" Width="500">
                     <HeaderStyle Wrap="True" />
                     <Settings />
                 </dx:GridViewDataTextColumn>

                </Columns>
            </dx:ASPxGridView>

                             
                         </dx:ContentControl>
                       </ContentCollection>
                  </dx:TabPage>

             
             <dx:TabPage Text="Realizar corte de reporte de ejecución">
                 <ContentCollection>
                     <dx:ContentControl ID="ContentControl2" runat="server">
                         <label for="txtsubCod"aria-required="true">Fecha de corte, este proceso solo puede realizarse una sola vez: </label>

                         <div style="padding: 0.1em;overflow:hidden">
                             <div style="margin:0.5em;padding:0.5em;float:left">
                                <dx:ASPxComboBox ID="cbFechaCorte" runat="server" ValueType="System.String" NullText="Selecione fecha de corte" CssClass="form-control" Width="200"></dx:ASPxComboBox>
                             </div>
                             <div style="margin:1em;padding:0.5em;float:left">
                                 <asp:Button ID="btnTodasInsto" runat="server" Text="Todas las instituciones"  CssClass="btn btn-success" ToolTip="Realizar corte de ejecución para todas las instituciones"   OnClick="btnTodasInsto_Click"
                       />  
                             </div>
                           
                             <div style="margin:1em;padding:0.5em;float:left">
                                <asp:Button ID="btnReportes" runat="server" Text="Reporte con fecha de corte"  CssClass="btn btn-warning" ToolTip="Generar reporte" OnClick="btnReportes_Click"
                       />  
                             </div>
                         </div>


                         <dx:ASPxGridView ID="gvEjecucion" runat="server" AutoGenerateColumns="False" KeyFieldName="ID_PRODUCTO" SettingsDetail-AllowOnlyOneMasterRowExpanded="true" SettingsBehavior-AllowFixedGroups="true" SettingsBehavior-AllowFocusedRow ="true" Width="100%" OnDetailRowExpandedChanged="gvEjecucion_DetailRowExpandedChanged"  Theme="Office2010Blue"  OnHtmlDataCellPrepared="gvEjecucion_HtmlDataCellPrepared">
                    <SettingsBehavior AllowFixedGroups="True" AllowFocusedRow="True" />
                    <Columns>
                        <dx:GridViewDataTextColumn Caption="EJECUCI&#211;N" VisibleIndex="0">
                            
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn FieldName="ID_TIPO" VisibleIndex="0" Visible="false"></dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn FieldName="TIPO" VisibleIndex="1" GroupIndex="0"></dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn FieldName="ID_RPRESUP" VisibleIndex="2" Visible="false"></dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn FieldName="RPRESUP" Caption="PROGRAMA PRESUPUESTARIO" VisibleIndex ="3" GroupIndex="1"></dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn FieldName="ID_RESULTADO" VisibleIndex="4" Visible="false"></dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn FieldName="RESULTADO" VisibleIndex="5" Width="30%"></dx:GridViewDataTextColumn>                        
                        <dx:GridViewDataTextColumn FieldName="ID_PRODUCTO" VisibleIndex="6" Visible="false"></dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn FieldName="PRODUCTO" VisibleIndex="7" Width="30%"></dx:GridViewDataTextColumn>
                        <dx:GridViewBandColumn Caption="FÍSICO ANUAL">
                            <Columns>
                                 <dx:GridViewDataTextColumn FieldName="MFISINICIAL" Caption="META INICIAL" VisibleIndex="0" Width="10%">
                                    <CellStyle HorizontalAlign="Right"></CellStyle>
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn FieldName="MFIS" Caption="META VIGENTE" VisibleIndex="1" Width="10%">
                                    <CellStyle HorizontalAlign="Right"></CellStyle>
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn FieldName="AFIS" Caption="AVANCE" VisibleIndex="2" Width="10%">
                                    <CellStyle HorizontalAlign="Right"></CellStyle>
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn FieldName="PORFIS" Caption="PORCENTAJE" VisibleIndex="3" Width="10%">
                                    <CellStyle HorizontalAlign="Right"></CellStyle>
                                </dx:GridViewDataTextColumn>
                                
                            </Columns>
                        </dx:GridViewBandColumn>
                        <dx:GridViewBandColumn Caption="FINANCIERO ANUAL">
                            <Columns>
                                <dx:GridViewDataTextColumn FieldName="MFININCIAL" Caption="META INICIAL" VisibleIndex="0" Width="10%">
                                    <PropertiesTextEdit DisplayFormatString="{0:N2}"></PropertiesTextEdit>
                                    <CellStyle HorizontalAlign="Right"></CellStyle>
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn FieldName="MFIN" Caption="META VIGENTE" VisibleIndex="0" Width="10%">
                                    <PropertiesTextEdit DisplayFormatString="{0:N2}"></PropertiesTextEdit>
                                    <CellStyle HorizontalAlign="Right"></CellStyle>
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn FieldName="AFIN" Caption="AVANCE" VisibleIndex="1" Width="10%">
                                    <PropertiesTextEdit DisplayFormatString="{0:N2}"></PropertiesTextEdit>
                                    <CellStyle HorizontalAlign="Right"></CellStyle>
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn FieldName="PORFIN" Caption="PORCENTAJE" VisibleIndex="2" Width="10%">
                                    <CellStyle HorizontalAlign="Right"></CellStyle>
                                </dx:GridViewDataTextColumn>
                            </Columns>
                        </dx:GridViewBandColumn>
                    </Columns>
                    <SettingsDetail ShowDetailRow="true" />
                    <Templates>
                        <DetailRow>
                            <dx:ASPxGridView ID="gvSubProductos" KeyFieldName="ID_SUBP" SettingsBehavior-AllowFocusedRow="true" runat="server" Width="100%" OnBeforePerformDataSelect="gvSubProductos_BeforePerformDataSelect" AutoGenerateColumns="False" Theme="Office2010Blue"   OnHtmlDataCellPrepared="gvSubProductos_HtmlDataCellPrepared" >
                                
                                
                                <Settings VerticalScrollableHeight="400" VerticalScrollBarMode="Visible" HorizontalScrollBarMode="Visible" />
                                <SettingsBehavior AllowFocusedRow="True" />
                                <Styles>
                                    <%--<SelectedRow BackColor="Red" ForeColor="White"></SelectedRow>--%>
                                    <%--<FocusedRow BackColor="Red"></FocusedRow>--%>
                                </Styles>
                                <Columns>
                                   
                                    <dx:GridViewDataTextColumn FieldName="ID_PROD" VisibleIndex="1" Visible="false">
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn FieldName="ID_SUBP" VisibleIndex="2" Visible="false">
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="SUBPRODUCTO" FieldName="SUBP" VisibleIndex="3" Width="300" ReadOnly="true">
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewBandColumn Caption="FÍSICO ANUAL" VisibleIndex="4">
                                        <Columns>
                                            <dx:GridViewDataTextColumn FieldName="MFISINICIAL" Caption="META INICIAL" VisibleIndex="1" ReadOnly="true">
                                                <CellStyle HorizontalAlign="Right"></CellStyle>                                                
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn FieldName="MFIS" Caption="META VIGENTE" VisibleIndex="1" ReadOnly="true">
                                                <CellStyle HorizontalAlign="Right"></CellStyle>                                                
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn FieldName="TFIS" Caption="AVANCE" VisibleIndex="2" ReadOnly="true">
                                                <CellStyle HorizontalAlign="Right"></CellStyle>
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn FieldName="PORFIS" Caption="PORCENTAJE" VisibleIndex="3" ReadOnly="true">
                                                <CellStyle HorizontalAlign="Right"></CellStyle>
                                            </dx:GridViewDataTextColumn>                                            
                                        </Columns>
                                    </dx:GridViewBandColumn>
                                    <dx:GridViewBandColumn Caption="FINANCIERO ANUAL" VisibleIndex="4">
                                        <Columns>
                                            <dx:GridViewDataTextColumn FieldName="MFININCIAL" ShowInCustomizationForm="True" Caption="META INICIAL" VisibleIndex="1" ReadOnly="true">
                                                <PropertiesTextEdit DisplayFormatString="{0:N2}"></PropertiesTextEdit>
                                                <CellStyle HorizontalAlign="Right"></CellStyle>
                                            </dx:GridViewDataTextColumn>  
                                            <dx:GridViewDataTextColumn FieldName="MFIN" ShowInCustomizationForm="True" Caption="META VIGENTE" VisibleIndex="1" ReadOnly="true">
                                                <PropertiesTextEdit DisplayFormatString="{0:N2}"></PropertiesTextEdit>
                                                <CellStyle HorizontalAlign="Right"></CellStyle>
                                            </dx:GridViewDataTextColumn>                                            
                                            <dx:GridViewDataTextColumn FieldName="TFIN" ShowInCustomizationForm="True" Caption="AVANCE" VisibleIndex="2" ReadOnly="true">
                                                <PropertiesTextEdit DisplayFormatString="{0:N2}"></PropertiesTextEdit>
                                                <CellStyle HorizontalAlign="Right"></CellStyle>
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn FieldName="PORFIN" Caption="PORCENTAJE" VisibleIndex="3" ReadOnly="true">
                                                <CellStyle HorizontalAlign="Right"></CellStyle>
                                            </dx:GridViewDataTextColumn>
                                        </Columns>
                                    </dx:GridViewBandColumn>
                                    <dx:GridViewBandColumn Caption="ENERO" VisibleIndex="5">
                                        <Columns>
                                            
                                            <dx:GridViewDataSpinEditColumn FieldName="AFIS1"  Caption="FISICO" VisibleIndex="1" ReadOnly="false" Visible="true">
            <PropertiesSpinEdit DisplayFormatString="N2" />
        </dx:GridViewDataSpinEditColumn>
                                            <dx:GridViewDataSpinEditColumn FieldName="AFIN1"  Caption="FINANCIERO" VisibleIndex="2" ReadOnly="false" Visible="true">
            <PropertiesSpinEdit DisplayFormatString="N2" />
        </dx:GridViewDataSpinEditColumn>
                                            
                                            
                                        </Columns>
                                    </dx:GridViewBandColumn>
                                    <dx:GridViewBandColumn Caption="FEBRERO" VisibleIndex="6">
                                        <Columns>
                                            <dx:GridViewDataSpinEditColumn FieldName="AFIS2"  Caption="FISICO" VisibleIndex="1" ReadOnly="false" Visible="true">
            <PropertiesSpinEdit DisplayFormatString="N2" />
        </dx:GridViewDataSpinEditColumn>
                                            <dx:GridViewDataSpinEditColumn FieldName="AFIN2"  Caption="FINANCIERO" VisibleIndex="2" ReadOnly="false" Visible="true">
            <PropertiesSpinEdit DisplayFormatString="N2" />
        </dx:GridViewDataSpinEditColumn>
                                           
                                        </Columns>
                                    </dx:GridViewBandColumn>
                                    <dx:GridViewBandColumn Caption="MARZO" VisibleIndex="7">
                                        <Columns> 
                                            <dx:GridViewDataSpinEditColumn FieldName="AFIS3"  Caption="FISICO" VisibleIndex="1" ReadOnly="false" Visible="true">
            <PropertiesSpinEdit DisplayFormatString="N2" />
        </dx:GridViewDataSpinEditColumn>
                                            <dx:GridViewDataSpinEditColumn FieldName="AFIN3"  Caption="FINANCIERO" VisibleIndex="2" ReadOnly="false" Visible="true">
            <PropertiesSpinEdit DisplayFormatString="N2" />
        </dx:GridViewDataSpinEditColumn>
                                            
                                        </Columns>
                                    </dx:GridViewBandColumn>
                                    <dx:GridViewBandColumn Caption="ABRIL" VisibleIndex="8">
                                        <Columns>
                                           <dx:GridViewDataSpinEditColumn FieldName="AFIS4"  Caption="FISICO" VisibleIndex="1" ReadOnly="false" Visible="true">
            <PropertiesSpinEdit DisplayFormatString="N2" />
        </dx:GridViewDataSpinEditColumn>
                                            <dx:GridViewDataSpinEditColumn FieldName="AFIN4"  Caption="FINANCIERO" VisibleIndex="2" ReadOnly="false" Visible="true">
            <PropertiesSpinEdit DisplayFormatString="N2" />
        </dx:GridViewDataSpinEditColumn>
                                            
                                        </Columns>
                                    </dx:GridViewBandColumn>
                                    <dx:GridViewBandColumn Caption="CUATRIMESTRE 1" VisibleIndex="8">
                                        <Columns>
                                            <dx:GridViewDataTextColumn FieldName="MFISC1" Caption="META FISICA" VisibleIndex="1" ReadOnly="true">
                                                <CellStyle HorizontalAlign="Right"></CellStyle>
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn FieldName="MFINC1" ShowInCustomizationForm="True" Caption="META FINANCIERA" VisibleIndex="2" Width="120" ReadOnly="true">
                                                <PropertiesTextEdit DisplayFormatString="{0:N2}"></PropertiesTextEdit>
                                                <CellStyle HorizontalAlign="Right"></CellStyle>
                                            </dx:GridViewDataTextColumn>                
                                            <dx:GridViewDataTextColumn FieldName="AFISC1" Caption="AVANCE FISICO" VisibleIndex="3" ReadOnly="true">
                                                <CellStyle HorizontalAlign="Right"></CellStyle>
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn FieldName="AFINC1" ShowInCustomizationForm="True" Caption="AVANCE FINANCIERO" VisibleIndex="4" Width="140" ReadOnly="true">
                                                <PropertiesTextEdit DisplayFormatString="{0:N2}"></PropertiesTextEdit>
                                                <CellStyle HorizontalAlign="Right"></CellStyle>
                                            </dx:GridViewDataTextColumn>                                            
                                        </Columns>
                                    </dx:GridViewBandColumn>
                                    <dx:GridViewBandColumn Caption="MAYO" VisibleIndex="9" >
                                        <Columns>
                                            <dx:GridViewDataSpinEditColumn FieldName="AFIS5"  Caption="FISICO" VisibleIndex="1" ReadOnly="false" Visible="true">
            <PropertiesSpinEdit DisplayFormatString="N2" />
        </dx:GridViewDataSpinEditColumn>
                                            <dx:GridViewDataSpinEditColumn FieldName="AFIN5"  Caption="FINANCIERO" VisibleIndex="2" ReadOnly="false" Visible="true">
            <PropertiesSpinEdit DisplayFormatString="N2" />
        </dx:GridViewDataSpinEditColumn>
                                           
                                        </Columns>
                                    </dx:GridViewBandColumn>
                                    <dx:GridViewBandColumn Caption="JUNIO" VisibleIndex="10">
                                        <Columns>
                                            <dx:GridViewDataSpinEditColumn FieldName="AFIS6"  Caption="FISICO" VisibleIndex="1" ReadOnly="false" Visible="true">
            <PropertiesSpinEdit DisplayFormatString="N2" />
        </dx:GridViewDataSpinEditColumn>
                                            <dx:GridViewDataSpinEditColumn FieldName="AFIN6"  Caption="FINANCIERO" VisibleIndex="2" ReadOnly="false" Visible="true">
            <PropertiesSpinEdit DisplayFormatString="N2" />
        </dx:GridViewDataSpinEditColumn>
                                           
                                        </Columns>
                                    </dx:GridViewBandColumn>
                                    <dx:GridViewBandColumn Caption="JULIO" VisibleIndex="11">
                                        <Columns>
                                          <dx:GridViewDataSpinEditColumn FieldName="AFIS7"  Caption="FISICO" VisibleIndex="1" ReadOnly="false" Visible="true">
            <PropertiesSpinEdit DisplayFormatString="N2" />
        </dx:GridViewDataSpinEditColumn>
                                            <dx:GridViewDataSpinEditColumn FieldName="AFIN7"  Caption="FINANCIERO" VisibleIndex="2" ReadOnly="false" Visible="true">
            <PropertiesSpinEdit DisplayFormatString="N2" />
        </dx:GridViewDataSpinEditColumn>
                                           
                                        </Columns>
                                    </dx:GridViewBandColumn>
                                    <dx:GridViewBandColumn Caption="AGOSTO" VisibleIndex="12">
                                        <Columns>
                                            <dx:GridViewDataSpinEditColumn FieldName="AFIS8"  Caption="FISICO" VisibleIndex="1" ReadOnly="false" Visible="true">
            <PropertiesSpinEdit DisplayFormatString="N2" />
        </dx:GridViewDataSpinEditColumn>
                                            <dx:GridViewDataSpinEditColumn FieldName="AFIN8"  Caption="FINANCIERO" VisibleIndex="2" ReadOnly="false" Visible="true">
            <PropertiesSpinEdit DisplayFormatString="N2" />
        </dx:GridViewDataSpinEditColumn>
                                          
                                        </Columns>
                                    </dx:GridViewBandColumn>
                                    <dx:GridViewBandColumn Caption="CUATRIMESTRE 2" VisibleIndex="12">
                                        <Columns>
                                            <dx:GridViewDataTextColumn FieldName="MFISC2" Caption="META FISICA" VisibleIndex="1">
                                                <CellStyle HorizontalAlign="Right"></CellStyle>
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn FieldName="MFINC2" ShowInCustomizationForm="True" Caption="META FINANCIERA" VisibleIndex="2">
                                                <PropertiesTextEdit DisplayFormatString="{0:N2}"></PropertiesTextEdit>
                                                <CellStyle HorizontalAlign="Right"></CellStyle>
                                            </dx:GridViewDataTextColumn>                
                                            <dx:GridViewDataTextColumn FieldName="AFISC2" Caption="AVANCE FISICO" VisibleIndex="3">
                                                <CellStyle HorizontalAlign="Right"></CellStyle>
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn FieldName="AFINC2" ShowInCustomizationForm="True" Caption="AVANCE FINANCIERO" VisibleIndex="4">
                                                <PropertiesTextEdit DisplayFormatString="{0:N2}"></PropertiesTextEdit>
                                                <CellStyle HorizontalAlign="Right"></CellStyle>
                                            </dx:GridViewDataTextColumn>                                            
                                        </Columns>
                                    </dx:GridViewBandColumn>
                                    <dx:GridViewBandColumn Caption="SEPTIEMBRE" VisibleIndex="13" Visible="true">
                                        <Columns>
                                            <dx:GridViewDataSpinEditColumn FieldName="AFIS9"  Caption="FISICO" VisibleIndex="1" ReadOnly="false" Visible="true">
            <PropertiesSpinEdit DisplayFormatString="N2" />
        </dx:GridViewDataSpinEditColumn>
                                            <dx:GridViewDataSpinEditColumn FieldName="AFIN9"  Caption="FINANCIERO" VisibleIndex="2" ReadOnly="false" Visible="true">
            <PropertiesSpinEdit DisplayFormatString="N2" />
        </dx:GridViewDataSpinEditColumn>
                                          
                                        </Columns>
                                    </dx:GridViewBandColumn>
                                    <dx:GridViewBandColumn Caption="OCTUBRE" VisibleIndex="14" Visible="true">
                                        <Columns>
                                            <dx:GridViewDataSpinEditColumn FieldName="AFIS10"  Caption="FISICO" VisibleIndex="1" ReadOnly="false" Visible="true">
            <PropertiesSpinEdit DisplayFormatString="N2" />
        </dx:GridViewDataSpinEditColumn>
                                            <dx:GridViewDataSpinEditColumn FieldName="AFIN10"  Caption="FINANCIERO" VisibleIndex="2" ReadOnly="false" Visible="true">
            <PropertiesSpinEdit DisplayFormatString="N2" />
        </dx:GridViewDataSpinEditColumn>
                                           
                                        </Columns>
                                    </dx:GridViewBandColumn>
                                    <dx:GridViewBandColumn Caption="NOVIEMBRE" VisibleIndex="15" Visible="true">
                                        <Columns>
                                            <dx:GridViewDataSpinEditColumn FieldName="AFIS11"  Caption="FISICO" VisibleIndex="1" ReadOnly="false" Visible="true">
            <PropertiesSpinEdit DisplayFormatString="N2" />
        </dx:GridViewDataSpinEditColumn>
                                            <dx:GridViewDataSpinEditColumn FieldName="AFIN11"  Caption="FINANCIERO" VisibleIndex="2" ReadOnly="false" Visible="true">
            <PropertiesSpinEdit DisplayFormatString="N2" />
        </dx:GridViewDataSpinEditColumn>
                                            
                                        </Columns>
                                    </dx:GridViewBandColumn>
                                    <dx:GridViewBandColumn Caption="DICIEMBRE" VisibleIndex="16" Visible="true">
                                        <Columns>
                                           <%-- <dx:GridViewDataTextColumn FieldName="AFIS12" Caption="FISICO" VisibleIndex="1">
                                                <CellStyle HorizontalAlign="Right"></CellStyle>
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn FieldName="AFIN12" ShowInCustomizationForm="True" Caption="FINANCIERO" VisibleIndex="2">
                                                <PropertiesTextEdit DisplayFormatString="{0:N2}"></PropertiesTextEdit>
                                                <CellStyle HorizontalAlign="Right"></CellStyle>
                                            </dx:GridViewDataTextColumn>--%>
                                            <dx:GridViewDataSpinEditColumn FieldName="AFIS12"  Caption="FISICO" VisibleIndex="1" ReadOnly="false" Visible="true">
            <PropertiesSpinEdit DisplayFormatString="N2" />
        </dx:GridViewDataSpinEditColumn>
                                            <dx:GridViewDataSpinEditColumn FieldName="AFIN12"  Caption="FINANCIERO" VisibleIndex="2" ReadOnly="false" Visible="true">
            <PropertiesSpinEdit DisplayFormatString="N2" />
        </dx:GridViewDataSpinEditColumn>
                                           
                                        </Columns>
                                    </dx:GridViewBandColumn>                           
                                    <dx:GridViewBandColumn Caption="CUATRIMESTRE 3" VisibleIndex="16" Visible="true">
                                        <Columns>
                                            <dx:GridViewDataTextColumn FieldName="MFISC3" Caption="META FISICA" VisibleIndex="1">
                                                <CellStyle HorizontalAlign="Right"></CellStyle>
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn FieldName="MFINC3" ShowInCustomizationForm="True" Caption="META FINANCIERA" VisibleIndex="2">
                                                <PropertiesTextEdit DisplayFormatString="{0:N2}"></PropertiesTextEdit>
                                                <CellStyle HorizontalAlign="Right"></CellStyle>
                                            </dx:GridViewDataTextColumn>                
                                            <dx:GridViewDataTextColumn FieldName="AFISC3" Caption="AVANCE FISICO" VisibleIndex="3">
                                                <CellStyle HorizontalAlign="Right"></CellStyle>
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn FieldName="AFINC3" ShowInCustomizationForm="True" Caption="AVANCE FINANCIERO" VisibleIndex="4">
                                                <PropertiesTextEdit DisplayFormatString="{0:N2}"></PropertiesTextEdit>
                                                <CellStyle HorizontalAlign="Right"></CellStyle>
                                            </dx:GridViewDataTextColumn>                                            
                                        </Columns>
                                    </dx:GridViewBandColumn>
                                </Columns>
                                <ClientSideEvents EndCallback ="function(s,e)
                                                             {
                                                                if(s.cpError)
                                                                {
                                                                    //alert(s.cpError);
                                                                    alertify.error(s.cpError);
                                                                    delete s.cpError;
                                                                }
                                                                else
                                                                if (s.cpGood){
                                                                    alertify.success(s.cpGood);
                                                                    delete s.cpGood;
                                                                }
                                                                else
                                                                if (s.cpInfo){
                                                                    alertify.error(s.cpInfo);
                                                                    delete s.cpInfo;
                                                                }
                                                             }           
                                                             "
                                            />
                        </dx:ASPxGridView>
                        </DetailRow>
                    </Templates>
                </dx:ASPxGridView>
            
                         </dx:ContentControl>
                     </ContentCollection>
                 </dx:TabPage>

             </TabPages>
         </dx:ASPxPageControl>

                  <dx:ASPxPopupControl ID="popVinculaUna" runat="server" AllowDragging ="true" Width ="600px" Height="300px"  PopupHorizontalAlign="WindowCenter"  PopupVerticalAlign="WindowCenter"  CloseAction="CloseButton"  >
    <HeaderStyle Font-Bold="False" Font-Size="10"  HorizontalAlign="Left" />
                                    <ContentCollection>
                                    <dx:PopupControlContentControl>
                                    <div  style ="overflow:auto; width:100%; height:90%; padding: 1px 10px 1px 10px;   ">
                                         <label for="txtsubCod"aria-required="true">Campos marcados con <font color="red">*</font> son obligatorios</label>
           <div class="form-group">
                           <label for="txtsubCod"aria-required="true"><font color="red">*</font>Seleccione temática: </label>
                           <dx:ASPxComboBox ID="cbTematica" runat="server" ValueType="System.String" NullText="Seleccione la temática" CssClass="form-control" Width="300"  OnSelectedIndexChanged="cbTematica_SelectedIndexChanged" AutoPostBack="true"></dx:ASPxComboBox>
                    </div>

                                                                 
           <div class="form-group">
                           <label for="txtsubCod"aria-required="true"><font color="red">*</font>Seleccione detalle de temática: </label>
                          <dx:ASPxComboBox ID="cbDetalleTematica" runat="server" ValueType="System.String" NullText="Seleccione detalle de temática" CssClass="form-control" Width="500"></dx:ASPxComboBox>
                    </div>
                    

                                          

                    <div>                   
                    <asp:Button ID="btnGrabaUna" runat="server" Text="Grabar temática"   CssClass="btn btn-success" OnClick ="btnGrabaUna_Click" />              
                     <asp:Button ID="btnCancelarUna" runat="server" Text="Cancelar/Cerrar" CssClass="btn btn-warning" OnClick="btnCancelarUna_Click" />
                </div>

          
                                  
                                        </div>

                                        </dx:PopupControlContentControl>
                                        </ContentCollection>
                                    </dx:ASPxPopupControl>



    <dx:ASPxPopupControl ID="popUpmuchos" runat="server" AllowDragging ="true" Width ="100%" Height="100%"  PopupHorizontalAlign="WindowCenter"  PopupVerticalAlign="WindowCenter"  CloseAction="CloseButton"  >
    <HeaderStyle Font-Bold="False" Font-Size="10"  HorizontalAlign="Left" />
                                    <ContentCollection>
                                    <dx:PopupControlContentControl>
                                    <div  style ="overflow:auto; width:100%; height:90%; padding: 1px 10px 1px 10px;   ">
                                         <label for="txtsubCod"aria-required="true">Campos marcados con <font color="red">*</font> son obligatorios</label>
           <div class="form-group">
                           <label for="txtsubCod"aria-required="true"><font color="red">*</font>Seleccione temática: </label>
                           <dx:ASPxComboBox ID="cbTematica2" runat="server" ValueType="System.String" NullText="Seleccione la temática" CssClass="form-control" Width="300"   OnSelectedIndexChanged="cbTematica2_SelectedIndexChanged" AutoPostBack="true"></dx:ASPxComboBox>
                    </div>

                                                                 
           <div class="form-group">
                           <label for="txtsubCod"aria-required="true"><font color="red">*</font>Seleccione detalle de temática: </label>
                          <dx:ASPxComboBox ID="cbDetalleTematica2" runat="server" ValueType="System.String" NullText="Seleccione detalle de temática" CssClass="form-control" Width="500" OnSelectedIndexChanged="cbDetalleTematica2_SelectedIndexChanged" AutoPostBack="true"></dx:ASPxComboBox>
                    </div>
                    <div>                   
                    <asp:Button ID="btnGraTematica2" runat="server" Text="Grabar temática"   CssClass="btn btn-success" OnClick="btnGraTematica2_Click" />              
                     <asp:Button ID="btnCierraTematica2" runat="server" Text="Cancelar/Cerrar" CssClass="btn btn-warning" OnClick="btnCierraTematica2_Click"  />
                </div>

                             <dx:ASPxGridView ID="gvProdVincula" runat="server" KeyFieldName="SPPRO$ID_PRODUCTO"   Theme="Office2010Blue" Width="100%"   SettingsPager-Mode="ShowAllRecords">
                                           
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
                    <dx:GridViewCommandColumn ShowSelectCheckbox="True" SelectAllCheckboxMode="Page" Width="4%" VisibleIndex="0">
                                </dx:GridViewCommandColumn>
                     <dx:GridViewDataTextColumn FieldName="SPPO$ID_POM"  Name="SPPO$ID_POM" ReadOnly="true" Visible="false" VisibleIndex="1" Width="0">
                     <HeaderStyle Wrap="True" />
                     <Settings />
                 </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="SPPO$ID_INSTITUCION"  Name="SPPO$ID_INSTITUCION" ReadOnly="true" Visible="false" VisibleIndex="2" Width="0">
                     <HeaderStyle Wrap="True" />
                     <Settings />
                 </dx:GridViewDataTextColumn>

                    <dx:GridViewDataTextColumn FieldName="SPP$ID_PERIODO"  Name="SSPP$ID_PERIODO" ReadOnly="true" Visible="false" VisibleIndex="3" Width="0">
                     <HeaderStyle Wrap="True" />
                     <Settings />
                 </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="PROGRAMA" Caption="Programa presupuestario"  Name="PROGRAMA" ReadOnly="true" Visible="true" VisibleIndex="4" Width="500">
                     <HeaderStyle Wrap="True" />
                         <Settings AutoFilterCondition="Contains" />
                     <Settings />
                 </dx:GridViewDataTextColumn>
                     

                    <dx:GridViewDataTextColumn FieldName="SUBPROGRAMA" Caption="Subprograma presupuestario"  Name="SUBPROGRAMA" ReadOnly="true" Visible="true" VisibleIndex="5" Width="500">
                     <HeaderStyle Wrap="True" />
                     <Settings />
                         <Settings AutoFilterCondition="Contains" />
                 </dx:GridViewDataTextColumn>

                     <dx:GridViewDataTextColumn FieldName="SPPRO$ID_PRODUCTO" Name="SPPRO$ID_PRODUCTO" ReadOnly="true" Visible="false" VisibleIndex="6" Width="500">
                     <HeaderStyle Wrap="True" />
                     <Settings />
                 </dx:GridViewDataTextColumn>

                  <dx:GridViewDataTextColumn FieldName="PRODUCTO" Caption="Producto"  Name="PRODUCTO" ReadOnly="true" Visible="true" VisibleIndex="7" Width="500">
                     <HeaderStyle Wrap="True" />
                         <Settings AutoFilterCondition="Contains" />
                     <Settings />
                 </dx:GridViewDataTextColumn>

                    <dx:GridViewDataTextColumn FieldName="MEDIDA" Caption="Unidad de medida"  Name="MEDIDA" ReadOnly="true" Visible="true" VisibleIndex="8" Width="500">
                     <HeaderStyle Wrap="True" />
                         <Settings AutoFilterCondition="Contains" />
                     <Settings />
                 </dx:GridViewDataTextColumn>


                    <dx:GridViewDataTextColumn FieldName="METAFISICA"   Name="METAFISICA" ReadOnly="true" Visible="true" VisibleIndex="9" Width="500">
                     <HeaderStyle Wrap="True" />
                     <Settings />
                 </dx:GridViewDataTextColumn>

                     <dx:GridViewDataTextColumn FieldName="METAFINANCIERA"   Name="METAFINANCIERA" ReadOnly="true" Visible="true" VisibleIndex="10" Width="500">
                     <HeaderStyle Wrap="True" />
                     <Settings />
                 </dx:GridViewDataTextColumn>


                    <dx:GridViewDataTextColumn FieldName="ID_TEMATICA"   Name="ID_TEMATICA" ReadOnly="true" Visible="false" VisibleIndex="11" Width="500">
                     <HeaderStyle Wrap="True" />
                     <Settings />
                 </dx:GridViewDataTextColumn>


                     <dx:GridViewDataTextColumn FieldName="TEMATICA" Caption="Temática"  Name="TEMATICA" ReadOnly="true" Visible="true" VisibleIndex="12" Width="500">
                     <HeaderStyle Wrap="True" />
                         <Settings AutoFilterCondition="Contains" />
                     <Settings />
                 </dx:GridViewDataTextColumn>

                     <dx:GridViewDataTextColumn FieldName="ID_DET_TEMATICA"   Name="ID_DET_TEMATICA" ReadOnly="true" Visible="false" VisibleIndex="13" Width="500">
                     <HeaderStyle Wrap="True" />
                     <Settings />
                 </dx:GridViewDataTextColumn>

                     <dx:GridViewDataTextColumn FieldName="DETALLE_TEMATICA" Caption="Detalle temática"  Name="DETALLE_TEMATICA" ReadOnly="true" Visible="true" VisibleIndex="14" Width="500">
                     <HeaderStyle Wrap="True" />
                         <Settings AutoFilterCondition="Contains" />
                     <Settings />
                 </dx:GridViewDataTextColumn>
                    
                    <dx:GridViewDataTextColumn FieldName="SPOA$ID_POA"   Name="SPOA$ID_POA" ReadOnly="true" Visible="false" VisibleIndex="15" Width="500">
                     <HeaderStyle Wrap="True" />
                     <Settings />
                 </dx:GridViewDataTextColumn>

                    <dx:GridViewDataTextColumn FieldName="SPOA$ANIO"   Name="SPOA$ANIO" ReadOnly="true" Visible="false" VisibleIndex="16" Width="500">
                     <HeaderStyle Wrap="True" />
                     <Settings />
                 </dx:GridViewDataTextColumn>

                </Columns>
            </dx:ASPxGridView>              

                    

          
                                  
                                        </div>

                                        </dx:PopupControlContentControl>
                                        </ContentCollection>
                                    </dx:ASPxPopupControl>



    <dx:ASPxPopupControl ID="popReportes" runat="server" AllowDragging ="true" Width ="800px" Height="450px"  PopupHorizontalAlign="WindowCenter"  PopupVerticalAlign="WindowCenter"  CloseAction="CloseButton"  HeaderText="Reportes de produtos y temáticas" >
    <HeaderStyle Font-Bold="False" Font-Size="10"  HorizontalAlign="Left" />
                                    <ContentCollection>
                                    <dx:PopupControlContentControl>
                                          <div class="form-group" style="margin-left:10px">
                        <label for="txtsubCod"aria-required="true">Institución: </label>
                        <dx:ASPxComboBox ID="cbInstitucionesFiltro" runat="server" ValueType="System.String" NullText="Seleccione institución" CssClass="form-control"></dx:ASPxComboBox>
                    </div>
                                        <div class="form-group">
                           <label for="txtsubCod"aria-required="true"><font color="red">*</font>Marque las tematicas que necesita ser filtradas, si no marca ninguna se le mostrarán todas la tematicas en el reporte </label>
                                            <dx:ASPxCheckBoxList ID="tematicas" runat="server"   RepeatColumns="4" RepeatLayout ="Table" >
        <CaptionSettings Position="Top" />
    </dx:ASPxCheckBoxList>
                          
                    </div>
                                        <div>                   
                    <asp:Button ID="btnGeneraReporte" runat="server" Text="Generar reporte"   CssClass="btn btn-success" OnClick="btnGeneraReporte_Click" />              
                     <asp:Button ID="btnCierraReporte" runat="server" Text="Cancelar/Cerrar" CssClass="btn btn-warning" OnClick="btnCierraReporte_Click"  />
                </div>
                                        </dx:PopupControlContentControl>
                                        </ContentCollection>
                                    </dx:ASPxPopupControl>
</asp:Content>
