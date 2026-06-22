<%@ Page Language="C#" MasterPageFile="~/Site.Master"  AutoEventWireup="true" CodeBehind="poa.aspx.cs" Inherits="SIPLAN2._0.poa.poa1" %>
<%@ Register Assembly="DevExpress.Web.v17.2, Version=17.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <script>

function onCollapseClick(s, e) {
            grid_resultados.CollapseAllDetailRows();
        }



function onCollapseClick2(s, e) {
            grid_resultados_POA.CollapseAllDetailRows();
        }

        function Alerta(mensaje, tipo) {          
            alertify.set('notifier', 'position', 'top-right'); 
            if (tipo == 1)
                alertify.success(mensaje);
            if (tipo == 2)
                alertify.error(mensaje);    
            if (tipo == 3)        
                alertify.alert(mensaje);
                  
        };  


function suma(_this)
{
   var cantidad = $("#MainContent_popInsumo_VistaInsumos_txtCantidad").val();
   var precio = $("#MainContent_popInsumo_VistaInsumos_txtPrecio").val();

console.log("cantidad: "+cantidad);
console.log("precio: "+precio);

var total = cantidad*precio;

console.log("total: "+total.toLocaleString());

document.getElementById("MainContent_popInsumo_VistaInsumos_lblTotal").style.color = "red";
document.getElementById("MainContent_popInsumo_VistaInsumos_lblTotal").style.fontSize = "14px";

document.getElementById("MainContent_popInsumo_VistaInsumos_lblTotal").innerHTML=" "+total.toLocaleString('mx-ES', { style: 'decimal', decimal: '2' });


}

function sumaCuat(_this)
{
    var cua1 = $("#MainContent_popInsumo_VistaInsumos_txtCUA1").val();
    var cua2 = $("#MainContent_popInsumo_VistaInsumos_txtCUA2").val();
    var cua3 = $("#MainContent_popInsumo_VistaInsumos_txtCUA3").val();

if(cua1 == "")
cua1 = 0;

if (cua2 == "")
cua2 = 0;

if (cua3 == "")
cua3 = 0;

cua1 = Number(cua1);
cua2 = Number(cua2);
cua3 = Number(cua3);
var totalCUA = cua1+cua2+cua3;

document.getElementById("MainContent_popInsumo_VistaInsumos_lblCuatrimestre").style.color = "red";
document.getElementById("MainContent_popInsumo_VistaInsumos_lblCuatrimestre").style.fontSize = "14px";

document.getElementById("MainContent_popInsumo_VistaInsumos_lblCuatrimestre").innerHTML=" "+totalCUA.toLocaleString('mx-ES', { style: 'decimal', decimal: '2' });




}
</script>
  
        <h4 style="color:blue">Programación de metas físicas y financieras (POA) <asp:Label ID="lblTitulo" runat="server"></asp:Label></h4>
<div style="float:right">
       
        <div style="align-content:flex-start">
            <asp:Button ID="btnProgramas" runat="server" Text="Programas"  CssClass="btn btn-primary" ToolTip="Programas presupuestarios" OnClick="btnProgramas_Click"/>
            <asp:Button ID="btnSuproductos" runat="server" Text="Metas multianules"   CssClass="btn btn-success" ToolTip="Programar metas multianuales para productos y subproductos" OnClick="btnSuproductos_Click"/>
            <asp:Button ID="btnPOA" runat="server" Text="Programar POA"   CssClass="btn" Style ="color:white; background-color:#0080FF" ToolTip="Programar metas mensuales para productos y subproductos"  OnClick="btnPOA_Click"/>
            <asp:Button ID="bntEnviar" runat="server" Text="Enviar Programación"  CssClass="btn btn-danger"  OnClick="bntEnviar_Click" />
            <asp:Button ID="btnRegresa" runat="server" Text="Formulación multianual"   CssClass="btn btn-warning" ToolTip="Formulación multianual" OnClick="btnRegresa_Click"/>
            <asp:Button ID="btnRegreso" runat="server" Text="Ir a pagina principal"  CssClass="btn " ToolTip="Regresar a pagina de principal para acceder a los otros modulos del sistema"   OnClick="btnRegreso_Click"  style="color:white;background-color:#F28B08" />
            </div>
               
    </div>    
    
    
    <div>

        <hr>
        <asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex="0">
             <asp:View ID="vistaPresupuesto" runat="server">
                  <div  style ="overflow:auto; width:100%; height:90%; padding: 1px 10px 1px 10px;   ">
                      <h4 style="color:#2d572c">Asignación de presupuesto a programas presupuestarios</h4>
                  <hr>
                      <asp:UpdatePanel ID="upProgramas" runat="server">
                          <ContentTemplate>                
                              <dx:ASPxGridView ID="gvProgramas" runat="server" KeyFieldName="NOPROGRAMA"   Theme="Office2010Blue" Width="100%"  SettingsBehavior-AllowFocusedRow ="true" SettingsBehavior-ConfirmDelete="true" OnRowUpdating="gvProgramas_RowUpdating"  OnCommandButtonInitialize="gvProgramas_CommandButtonInitialize" AutoGenerateColumns="true">
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
                             <%--<SettingsContextMenu Enabled="True">
             </SettingsContextMenu>--%>
             <SettingsPager AlwaysShowPager="True" PageSize="200" ShowSeparators="True">
                 <PageSizeItemSettings Visible="True">
                 </PageSizeItemSettings>
             </SettingsPager>
             <%--<Settings ShowFilterRow="True" ShowGroupPanel="True" />--%>
             <SettingsBehavior AllowFocusedRow="True" ConfirmDelete="True" />
             <SettingsCommandButton DeleteButton-Text="Quitar" DeleteButton-ButtonType="Button">
                                        <UpdateButton ButtonType="Button" Text="Guardar presupuesto">
                                            <Styles>
                                                <Style CssClass="btn-warging">
                                                </Style>
                                            </Styles>
                                        </UpdateButton>
                                        <CancelButton ButtonType="Button" Text="cancelar">
                                            <Styles>
                                                <FocusRectStyle CssClass="btn-danger">
                                                </FocusRectStyle>
                                            </Styles>
                                        </CancelButton>
                                        <EditButton ButtonType="Button" Text="Asignar presupuesto">
                                        </EditButton>
                                        <DeleteButton ButtonType="Button">
                                           
                                        </DeleteButton>
                                    </SettingsCommandButton>
             <SettingsDataSecurity AllowDelete="true" AllowEdit="true" AllowInsert="False" />
             <SettingsLoadingPanel Mode="ShowOnStatusBar" />
             <%--<SettingsText CommandApplyFilter="Aplicar filtro" CommandApplySearchPanelFilter="asdasfasdfasdf" ContextMenuHideColumn="Ocultar Columna" ContextMenuShowCustomizationWindow="Mostrar Columnas" ContextMenuShowFilterRow="Fila de Filtros" ContextMenuShowFilterRowMenu="Seleccione Tipo de Filtro" ContextMenuShowSearchPanel="Buscar" CustomizationWindowCaption="Columnas Ocultas" GroupPanel="Arrastre las columnas que desee agrupar" />--%>
                <Columns>
                    <dx:GridViewDataTextColumn FieldName="NOPROGRAMA"  Name="NOPROGRAMA" Caption="NUMERO PROGRAMA" ReadOnly="true" Visible="true" VisibleIndex="0" Width="10%">
                                     <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="PROGRAMA"  Name="PROGRAMA" Caption="PROGRAMA PRESUPUESTARIO" ReadOnly="true" Visible="true" VisibleIndex="1" Width="25%">
                                     <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="ESSUBPROGRAMADE"  Name="ESSUBPROGRAMADE" Caption="PROGRAMA" ReadOnly="true" Visible="true" VisibleIndex="2" Width="10%" GroupIndex="0">
                                     <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="SPPRO$ID_POM"  Name="SPPRO$ID_POM"  ReadOnly="true" Visible="false" VisibleIndex="3" Width="0%" >
                                     <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn>
                    
                     <dx:GridViewDataTextColumn FieldName="SPPRO$ID_INSTITUCION"  Name="SPPRO$ID_INSTITUCION"  ReadOnly="true" Visible="false" VisibleIndex="4" Width="0%">
                                     <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataSpinEditColumn FieldName="PRESUPUESTO2020" Width="10%" Caption="PRESUPUESTO 2,020" VisibleIndex="5" ReadOnly="false" Visible="true">
            <PropertiesSpinEdit DisplayFormatString="N2" />
        </dx:GridViewDataSpinEditColumn>
                     
                    <dx:GridViewDataSpinEditColumn FieldName="PRESUPUESTO2021" Width="10%" Caption="PRESUPUESTO 2,021" VisibleIndex="6" ReadOnly="false" Visible="true">
            <PropertiesSpinEdit DisplayFormatString="N2" />
        </dx:GridViewDataSpinEditColumn>
                    <dx:GridViewDataSpinEditColumn FieldName="PRESUPUESTO2022" Width="10%" Caption="PRESUPUESTO 2,022" VisibleIndex="7" ReadOnly="false" Visible="true">
            <PropertiesSpinEdit DisplayFormatString="N2" />
        </dx:GridViewDataSpinEditColumn>
                    <dx:GridViewDataSpinEditColumn FieldName="PRESUPUESTO2023" Width="10%" Caption="PRESUPUESTO 2,023" VisibleIndex="8" ReadOnly="false" Visible="true">
            <PropertiesSpinEdit DisplayFormatString="N2" />
        </dx:GridViewDataSpinEditColumn>
                    <dx:GridViewDataSpinEditColumn FieldName="PRESUPUESTO2024" Width="10%" Caption="PRESUPUESTO 2,024" VisibleIndex="9" ReadOnly="false" Visible="true">
            <PropertiesSpinEdit DisplayFormatString="N2" />
        </dx:GridViewDataSpinEditColumn>
                     <dx:GridViewDataSpinEditColumn FieldName="PRESUPUESTO2025" Width="10%" Caption="PRESUPUESTO 2,025" VisibleIndex="10" ReadOnly="false" Visible="true">
            <PropertiesSpinEdit DisplayFormatString="N2" />
        </dx:GridViewDataSpinEditColumn>
                     <dx:GridViewDataSpinEditColumn FieldName="PRESUPUESTO2026" Width="10%" Caption="PRESUPUESTO 2,026" VisibleIndex="11" ReadOnly="false" Visible="true">
            <PropertiesSpinEdit DisplayFormatString="N2" />
        </dx:GridViewDataSpinEditColumn>

                <dx:GridViewDataSpinEditColumn FieldName="PRESUPUESTO2027" Width="10%" Caption="PRESUPUESTO 2,027" VisibleIndex="12" ReadOnly="false" Visible="true">
            <PropertiesSpinEdit DisplayFormatString="N2" />
        </dx:GridViewDataSpinEditColumn>

                     <dx:GridViewDataSpinEditColumn FieldName="PRESUPUESTO2028" Width="10%" Caption="PRESUPUESTO 2,028" VisibleIndex="13" ReadOnly="false" Visible="true">
            <PropertiesSpinEdit DisplayFormatString="N2" />
        </dx:GridViewDataSpinEditColumn>

                    <dx:GridViewDataSpinEditColumn FieldName="PRESUPUESTO2029" Width="10%" Caption="PRESUPUESTO 2,029" VisibleIndex="14" ReadOnly="false" Visible="true">
            <PropertiesSpinEdit DisplayFormatString="N2" />
        </dx:GridViewDataSpinEditColumn>

                    
                    <dx:GridViewDataSpinEditColumn FieldName="PRESUPUESTO2030" Width="10%" Caption="PRESUPUESTO 2,030" VisibleIndex="15" ReadOnly="false" Visible="true">
            <PropertiesSpinEdit DisplayFormatString="N2" />
        </dx:GridViewDataSpinEditColumn>

                    
                    <dx:GridViewDataSpinEditColumn FieldName="PRESUPUESTO2031" Width="10%" Caption="PRESUPUESTO 2,031" VisibleIndex="16" ReadOnly="false" Visible="true">
            <PropertiesSpinEdit DisplayFormatString="N2" />
        </dx:GridViewDataSpinEditColumn>



                     
                    <dx:GridViewDataTextColumn FieldName="PROGRA2020"  Name="PROGRA2020"  ReadOnly="true" Visible="false" VisibleIndex="17" Width="0%">
                                     <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="PROGRA2021"  Name="PROGRA2021" ReadOnly="true" Visible="false" VisibleIndex="18" Width="0%">
                                     <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="PROGRA2022"  Name="PROGRA2022"  ReadOnly="true" Visible="false" VisibleIndex="19" Width="0%">
                                     <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="PROGRA2023"  Name="PROGRA2023"  ReadOnly="true" Visible="false" VisibleIndex="20" Width="0%">
                                     <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="PROGRA2024"  Name="PROGRA2024"  ReadOnly="true" Visible="false" VisibleIndex="21" Width="0%">
                                     <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn>
                     <dx:GridViewDataTextColumn FieldName="PROGRA2025"  Name="PROGRA2025"  ReadOnly="true" Visible="false" VisibleIndex="22" Width="0%">
                                     <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn>
                     <dx:GridViewDataTextColumn FieldName="PROGRA2026"  Name="PROGRA2026"  ReadOnly="true" Visible="false" VisibleIndex="23" Width="0%">
                                     <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn>

                           <dx:GridViewDataTextColumn FieldName="PROGRA2027"  Name="PROGRA2027"  ReadOnly="true" Visible="false" VisibleIndex="24" Width="0%">
                                     <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn>

                           <dx:GridViewDataTextColumn FieldName="PROGRA2028"  Name="PROGRA2028"  ReadOnly="true" Visible="false" VisibleIndex="25" Width="0%">
                                     <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn>

                      <dx:GridViewDataTextColumn FieldName="PROGRA2029"  Name="PROGRA2029"  ReadOnly="true" Visible="false" VisibleIndex="26" Width="0%">
                                     <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn>

                    <dx:GridViewDataTextColumn FieldName="PROGRA2030"  Name="PROGRA2030"  ReadOnly="true" Visible="false" VisibleIndex="27" Width="0%">
                                     <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn>

                       <dx:GridViewDataTextColumn FieldName="PROGRA2031"  Name="PROGRA2031"  ReadOnly="true" Visible="false" VisibleIndex="28" Width="0%">
                    <HeaderStyle Wrap="True" />
                    <Settings />
                   </dx:GridViewDataTextColumn>
                   
                    <dx:GridViewDataTextColumn FieldName="POA2020"  Name="POA2020"  ReadOnly="true" Visible="false" VisibleIndex="29" Width="0%">
                                     <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="POA2021"  Name="POA2021"  ReadOnly="true" Visible="false" VisibleIndex="30" Width="0%">
                                     <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="POA2022"  Name="POA2022"  ReadOnly="true" Visible="false" VisibleIndex="31" Width="0%">
                                     <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="POA2023"  Name="POA2023"  ReadOnly="true" Visible="false" VisibleIndex="32" Width="0%">
                                     <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn>
                     <dx:GridViewDataTextColumn FieldName="POA2024"  Name="POA2024"  ReadOnly="true" Visible="false" VisibleIndex="33" Width="0%">
                                     <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="POA2025"  Name="POA2025"  ReadOnly="true" Visible="false" VisibleIndex="34" Width="0%">
                                     <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn>
                     <dx:GridViewDataTextColumn FieldName="POA2026"  Name="POA2026"  ReadOnly="true" Visible="false" VisibleIndex="35" Width="0%">
                                     <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="POA2027"  Name="POA2027"  ReadOnly="true" Visible="false" VisibleIndex="36" Width="0%">
                                     <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="POA2028"  Name="POA2028"  ReadOnly="true" Visible="false" VisibleIndex="37" Width="0%">
                                     <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn>

                    <dx:GridViewDataTextColumn FieldName="POA2029"  Name="POA2029"  ReadOnly="true" Visible="false" VisibleIndex="38" Width="0%">
                                     <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn>

                     <dx:GridViewDataTextColumn FieldName="POA2030"  Name="POA2030"  ReadOnly="true" Visible="false" VisibleIndex="39" Width="0%">
                                     <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn>

                     <dx:GridViewDataTextColumn FieldName="POA2031"  Name="POA2031"  ReadOnly="true" Visible="false" VisibleIndex="40" Width="0%">
                 <HeaderStyle Wrap="True" />
                 <Settings />
                </dx:GridViewDataTextColumn>

                    <dx:GridViewCommandColumn ShowDeleteButton="false" ShowEditButton="true" ShowNewButtonInHeader="false" VisibleIndex="41">
                     <HeaderStyle Wrap="True" />
                 </dx:GridViewCommandColumn>

                    </Columns>
                                   <SettingsEditing Mode="inline" />
                                   <Settings ShowGroupPanel="true" ShowFooter="true" ShowGroupFooter="VisibleIfExpanded" />
       <TotalSummary>
            <dx:ASPxSummaryItem FieldName="PRESUPUESTO2020" SummaryType="Sum" />
            <dx:ASPxSummaryItem FieldName="PRESUPUESTO2021" SummaryType="Sum" />
           <dx:ASPxSummaryItem FieldName="PRESUPUESTO2022" SummaryType="Sum" />
            <dx:ASPxSummaryItem FieldName="PRESUPUESTO2023" SummaryType="Sum" />
           <dx:ASPxSummaryItem FieldName="PRESUPUESTO2024" SummaryType="Sum" />
            <dx:ASPxSummaryItem FieldName="PRESUPUESTO2025" SummaryType="Sum" />
            <dx:ASPxSummaryItem FieldName="PRESUPUESTO2026" SummaryType="Sum" />
            <dx:ASPxSummaryItem FieldName="PRESUPUESTO2027" SummaryType="Sum" />
           <dx:ASPxSummaryItem FieldName="PRESUPUESTO2028" SummaryType="Sum" />
           <dx:ASPxSummaryItem FieldName="PRESUPUESTO2029" SummaryType="Sum" />
           <dx:ASPxSummaryItem FieldName="PRESUPUESTO2030" SummaryType="Sum" />
             <dx:ASPxSummaryItem FieldName="PRESUPUESTO2031" SummaryType="Sum" />
        </TotalSummary>

                                  <GroupSummary>
            <dx:ASPxSummaryItem FieldName="PRESUPUESTO2020" ShowInGroupFooterColumn="PRESUPUESTO2020" SummaryType="Sum" />
             <dx:ASPxSummaryItem FieldName="PRESUPUESTO2021" ShowInGroupFooterColumn="PRESUPUESTO2021" SummaryType="Sum" />
            <dx:ASPxSummaryItem FieldName="PRESUPUESTO2022" ShowInGroupFooterColumn="PRESUPUESTO2022" SummaryType="Sum" />
            <dx:ASPxSummaryItem FieldName="PRESUPUESTO2023" ShowInGroupFooterColumn="PRESUPUESTO2023" SummaryType="Sum" />
            <dx:ASPxSummaryItem FieldName="PRESUPUESTO2024" ShowInGroupFooterColumn="PRESUPUESTO2024" SummaryType="Sum" />
             <dx:ASPxSummaryItem FieldName="PRESUPUESTO2025" ShowInGroupFooterColumn="PRESUPUESTO2025" SummaryType="Sum" />
             <dx:ASPxSummaryItem FieldName="PRESUPUESTO2026" ShowInGroupFooterColumn="PRESUPUESTO2026" SummaryType="Sum" />
             <dx:ASPxSummaryItem FieldName="PRESUPUESTO2027" ShowInGroupFooterColumn="PRESUPUESTO2027" SummaryType="Sum" />
             <dx:ASPxSummaryItem FieldName="PRESUPUESTO2028" ShowInGroupFooterColumn="PRESUPUESTO2028" SummaryType="Sum" />
             <dx:ASPxSummaryItem FieldName="PRESUPUESTO2029" ShowInGroupFooterColumn="PRESUPUESTO2029" SummaryType="Sum" />
             <dx:ASPxSummaryItem FieldName="PRESUPUESTO2030" ShowInGroupFooterColumn="PRESUPUESTO2030" SummaryType="Sum" />
             <dx:ASPxSummaryItem FieldName="PRESUPUESTO2031" ShowInGroupFooterColumn="PRESUPUESTO2031" SummaryType="Sum" />
        </GroupSummary>
                            </dx:ASPxGridView>
                              </ContentTemplate> 
                          </asp:UpdatePanel>
                           </div>
             </asp:View>
             <asp:View ID="vstSubsos" runat="server">
                 <asp:Panel ID="estrategico" runat="server">
                     <div  style ="overflow:auto; width:100%; height:90%; padding: 1px 10px 1px 10px;   ">
                      <h4 style="color:#2d572c">Asignación de metas multianuales físicas y financieras para productos/subproductos</h4>
                  
                         <!--<asp:UpdatePanel ID="UpdatePanel1" runat="server">
                          <ContentTemplate> -->   
                         <dx:ASPxComboBox ID="cbTipoProduccion" runat="server" NullText="Seleccione el tipo productos que necesite desplegar" OnValueChanged="cbTipoProduccion_ValueChanged" AutoPostBack="True" SelectedIndex="0" Theme="Office2010Blue" CssClass="form-control" Width="400">
                           
                             <Items>
                                 <dx:ListEditItem Selected="True" Text="Resultado Institucional" Value="0" />
                                 <dx:ListEditItem Text="Resultados Estratégicos RE" Value="1" />
                             </Items>
                           
                         </dx:ASPxComboBox>
                         <!--</ContentTemplate>
                             </asp:UpdatePanel>-->
                        
                         
                              

                          <!--<asp:UpdatePanel ID="upEstrategicos" runat="server">
                          <ContentTemplate>-->     
                              <h4><asp:Label ID="lblTipodeProduccion" runat="server" style="color:black"></asp:Label></h4>
                              <div class="form-group">
                                             <div class="btn-group" style="display:flex; justify-content:flex-start;">                                  
                                              <%--   <asp:UpdatePanel ID="panBotonArchivos" runat="server">   
                                                     <ContentTemplate>--%>
                                     <asp:Button ID="btnMetas" runat="server" Text="Actualizar montos"  CssClass="btn btn-primary"  OnClick="btnMetas_Click" />  
                                     <asp:Button ID="btnPOM" runat="server" Text="Reporte POM"  CssClass="btn btn-warning"  OnClick="btnPOM_Click" />  
                                        
                                  <%-- </ContentTemplate>
                                                         </asp:UpdatePanel>--%>
                                                     </div>
                                         </div>
                              <dx:ASPxGridView ID="gvProduccion" runat="server" KeyFieldName="SPPRO$ID_PROGRAMA_PRESUPUESTO"  AutoGenerateColumns="False"  Theme="Office2010Blue" Width="100%"  SettingsBehavior-AllowFocusedRow ="true" SettingsBehavior-ConfirmDelete="true"  SettingsDetail-AllowOnlyOneMasterRowExpanded="true"   OnDetailRowExpandedChanged="gvProduccion_DetailRowExpandedChanged"   ClientInstanceName="grid_resultados">
             <Settings ShowFooter="True"/>
                           
                            <%--<Settings ShowGroupPanel="True"  VerticalScrollBarMode="Visible"  HorizontalScrollBarMode="Visible" />--%>
                    
                          
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
             <SettingsCommandButton DeleteButton-ButtonType="Button" DeleteButton-Text="Quitar">
                 <DeleteButton ButtonType="Button">
                 </DeleteButton>
             </SettingsCommandButton>
             <SettingsDataSecurity AllowDelete="true" AllowEdit="true" AllowInsert="False" />
             <SettingsLoadingPanel Mode="ShowOnStatusBar" />
             <SettingsText CommandApplyFilter="Aplicar filtro" CommandApplySearchPanelFilter="asdasfasdfasdf" ContextMenuHideColumn="Ocultar Columna" ContextMenuShowCustomizationWindow="Mostrar Columnas" ContextMenuShowFilterRow="Fila de Filtros" ContextMenuShowFilterRowMenu="Seleccione Tipo de Filtro" ContextMenuShowSearchPanel="Buscar" CustomizationWindowCaption="Columnas Ocultas" GroupPanel="Arrastre las columnas que desee agrupar" />
                                  <Columns>
                                       <%--<dx:GridViewDataTextColumn FieldName="NUMERO"  Name="NUMERO"  ReadOnly="true" Visible="false" VisibleIndex="0" Width="0%">
                                     <HeaderStyle Wrap="True" />
                                    </dx:GridViewDataTextColumn>       --%>                              
                                      
                                                <dx:GridViewDataTextColumn FieldName="SPPRO$ID_PROGRAMA_PRESUPUESTO"  Name="SPPRO$ID_PROGRAMA_PRESUPUESTO"   ReadOnly="true" Visible="false" VisibleIndex="0" >
                                     <HeaderStyle Wrap="True" />
                                    </dx:GridViewDataTextColumn>
                                      <dx:GridViewDataTextColumn FieldName="PROGRAMA"  Name="PROGRAMA"  ReadOnly="true" Visible="true" VisibleIndex="1" Width="30%" Caption="Programa Presupuestario" >
                                     <PropertiesTextEdit>
                    <Style HorizontalAlign="Center">
                    </Style>
                </PropertiesTextEdit>
                                    </dx:GridViewDataTextColumn>                                
                                                                   
                                                                  
                                       <dx:GridViewDataTextColumn FieldName="P0A2020"  Name="P0A2020"  ReadOnly="true" Visible="false" VisibleIndex="2" Width="0%">
                                     <HeaderStyle Wrap="True" />
                                    </dx:GridViewDataTextColumn>
                                      <dx:GridViewBandColumn Caption="2,020"  Name="CLASPRES2020" VisibleIndex="3" Visible="True">
                                          <Columns>
                                               <dx:GridViewDataTextColumn FieldName="PRESUPUESTO2020"  Name="PRESUPUESTO2020"  ReadOnly="true" Visible="true" VisibleIndex="1" Width="7%" Caption="Presupuesto">
                                     <HeaderStyle Wrap="True" />
                                                   <PropertiesTextEdit DisplayFormatString="{0:N2}">
                                                            </PropertiesTextEdit>
                                                            <FilterCellStyle HorizontalAlign="Center"/>
                                  </dx:GridViewDataTextColumn>
                                              <dx:GridViewDataTextColumn FieldName="PR0GRAMADO2020"  Name="PR0GRAMADO2020"  ReadOnly="true" Visible="true" VisibleIndex="2" Width="7%" Caption="Asignado">
                                     <PropertiesTextEdit DisplayFormatString="{0:N2}">
                                                            </PropertiesTextEdit>
                                                            <FilterCellStyle HorizontalAlign="Center"/>
                                                            <HeaderStyle Wrap="True" />
                                  </dx:GridViewDataTextColumn>
                                          </Columns>
                                      </dx:GridViewBandColumn>
                                     <dx:GridViewDataTextColumn FieldName="P0A2021"  Name="P0A2021"  ReadOnly="true" Visible="false" VisibleIndex="4" Width="0%">
                                     
                                                            <HeaderStyle Wrap="True" />
                                    </dx:GridViewDataTextColumn>
                                      <dx:GridViewBandColumn Caption="2,021"  Name="CLASPRES2021" VisibleIndex="5" Visible="True">
                                          <Columns>
                                               <dx:GridViewDataTextColumn FieldName="PRESUPUESTO2021"  Name="PRESUPUESTO2021"  ReadOnly="true" Visible="true" VisibleIndex="1" Width="7%" Caption="Presupuesto">
                                     <PropertiesTextEdit DisplayFormatString="{0:N2}">
                                                            </PropertiesTextEdit>
                                                            <FilterCellStyle HorizontalAlign="Center"/>
                                                            <HeaderStyle Wrap="True" />
                                  </dx:GridViewDataTextColumn>
                                              <dx:GridViewDataTextColumn FieldName="PR0GRAMADO2021"  Name="PR0GRAMADO2021"  ReadOnly="true" Visible="true" VisibleIndex="2" Width="7%" Caption="Asignado">
                                    <PropertiesTextEdit DisplayFormatString="{0:N2}">
                                                            </PropertiesTextEdit>
                                                            <FilterCellStyle HorizontalAlign="Center"/>
                                                            <HeaderStyle Wrap="True" />
                                  </dx:GridViewDataTextColumn>
                                          </Columns>
                                      </dx:GridViewBandColumn>
                                      <dx:GridViewDataTextColumn FieldName="P0A2022"  Name="P0A2022"  ReadOnly="true" Visible="false" VisibleIndex="6" Width="0%">
                                     <HeaderStyle Wrap="True" />
                                    </dx:GridViewDataTextColumn>
                                      <dx:GridViewBandColumn Caption="2,022"  Name="CLASPRES2022" VisibleIndex="7" Visible="True">
                                          <Columns>
                                               <dx:GridViewDataTextColumn FieldName="PRESUPUESTO2022"  Name="PRESUPUESTO2022"  ReadOnly="true" Visible="true" VisibleIndex="1" Width="7%" Caption="Presupuesto">
                                     <PropertiesTextEdit DisplayFormatString="{0:N2}">
                                                            </PropertiesTextEdit>
                                                            <FilterCellStyle HorizontalAlign="Center"/>
                                                            <HeaderStyle Wrap="True" />
                                  </dx:GridViewDataTextColumn>
                                              <dx:GridViewDataTextColumn FieldName="PR0GRAMADO2022"  Name="PR0GRAMADO2022"  ReadOnly="true" Visible="true" VisibleIndex="2" Width="7%" Caption="Asignado">
                                    <PropertiesTextEdit DisplayFormatString="{0:N2}">
                                                            </PropertiesTextEdit>
                                                            <FilterCellStyle HorizontalAlign="Center"/>
                                                            <HeaderStyle Wrap="True" />
                                  </dx:GridViewDataTextColumn>
                                          </Columns>
                                      </dx:GridViewBandColumn>

                                      <dx:GridViewDataTextColumn FieldName="P0A2023"  Name="P0A2023"  ReadOnly="true" Visible="false" VisibleIndex="8" Width="0%">
                                     <HeaderStyle Wrap="True" />
                                    </dx:GridViewDataTextColumn>
                                      <dx:GridViewBandColumn Caption="2,023"  Name="CLASPRES2023" VisibleIndex="9" Visible="True">
                                          <Columns>
                                               <dx:GridViewDataTextColumn FieldName="PRESUPUESTO2023"  Name="PRESUPUESTO2023"  ReadOnly="true" Visible="true" VisibleIndex="1" Width="7%" Caption="Presupuesto">
                                     <PropertiesTextEdit DisplayFormatString="{0:N2}">
                                                            </PropertiesTextEdit>
                                                            <FilterCellStyle HorizontalAlign="Center"/>
                                                            <HeaderStyle Wrap="True" />
                                  </dx:GridViewDataTextColumn>
                                              <dx:GridViewDataTextColumn FieldName="PR0GRAMADO2023"  Name="PR0GRAMADO2023"  ReadOnly="true" Visible="true" VisibleIndex="2" Width="7%" Caption="Asignado">
                                    <PropertiesTextEdit DisplayFormatString="{0:N2}">
                                                            </PropertiesTextEdit>
                                                            <FilterCellStyle HorizontalAlign="Center"/>
                                                            <HeaderStyle Wrap="True" />
                                  </dx:GridViewDataTextColumn>
                                          </Columns>
                                      </dx:GridViewBandColumn>
                                       <dx:GridViewDataTextColumn FieldName="P0A2024"  Name="P0A2024"  ReadOnly="true" Visible="false" VisibleIndex="10" Width="0%">
                                     <HeaderStyle Wrap="True" />
                                    </dx:GridViewDataTextColumn>
                                      <dx:GridViewBandColumn Caption="2,024"  Name="CLASPRES2024" VisibleIndex="11" Visible="True">
                                          <Columns>
                                               <dx:GridViewDataTextColumn FieldName="PRESUPUESTO2024"  Name="PRESUPUESTO2024"  ReadOnly="true" Visible="true" VisibleIndex="1" Width="7%" Caption="Presupuesto">
                                     <PropertiesTextEdit DisplayFormatString="{0:N2}">
                                                            </PropertiesTextEdit>
                                                            <FilterCellStyle HorizontalAlign="Center"/>
                                                            <HeaderStyle Wrap="True" />
                                  </dx:GridViewDataTextColumn>
                                              <dx:GridViewDataTextColumn FieldName="PR0GRAMADO2024"  Name="PR0GRAMADO2024"  ReadOnly="true" Visible="true" VisibleIndex="2" Width="7%" Caption="Asignado">
                                    <PropertiesTextEdit DisplayFormatString="{0:N2}">
                                                            </PropertiesTextEdit>
                                                            <FilterCellStyle HorizontalAlign="Center"/>
                                                            <HeaderStyle Wrap="True" />
                                  </dx:GridViewDataTextColumn>

                                              </Columns>
                                      </dx:GridViewBandColumn>                                             
                                                   <dx:GridViewBandColumn Caption="2,025"  Name="CLASPRES2025" VisibleIndex="11" Visible="True">
                                          <Columns>
                                               <dx:GridViewDataTextColumn FieldName="PRESUPUESTO2025"  Name="PRESUPUESTO2025"  ReadOnly="true" Visible="true" VisibleIndex="1" Width="7%" Caption="Presupuesto">
                                     <PropertiesTextEdit DisplayFormatString="{0:N2}">
                                                            </PropertiesTextEdit>
                                                            <FilterCellStyle HorizontalAlign="Center"/>
                                                            <HeaderStyle Wrap="True" />
                                  </dx:GridViewDataTextColumn>
                                              <dx:GridViewDataTextColumn FieldName="PR0GRAMADO2025"  Name="PR0GRAMADO2025"  ReadOnly="true" Visible="true" VisibleIndex="2" Width="7%" Caption="Asignado">
                                    <PropertiesTextEdit DisplayFormatString="{0:N2}">
                                                            </PropertiesTextEdit>
                                                            <FilterCellStyle HorizontalAlign="Center"/>
                                                            <HeaderStyle Wrap="True" />
                                  </dx:GridViewDataTextColumn>

                                              </Columns>
                                      </dx:GridViewBandColumn>              
                                      

                                       <dx:GridViewBandColumn Caption="2,026"  Name="CLASPRES2026" VisibleIndex="12" Visible="True">
                                          <Columns>
                                               <dx:GridViewDataTextColumn FieldName="PRESUPUESTO2026"  Name="PRESUPUESTO2026"  ReadOnly="true" Visible="true" VisibleIndex="1" Width="7%" Caption="Presupuesto">
                                     <PropertiesTextEdit DisplayFormatString="{0:N2}">
                                                            </PropertiesTextEdit>
                                                            <FilterCellStyle HorizontalAlign="Center"/>
                                                            <HeaderStyle Wrap="True" />
                                  </dx:GridViewDataTextColumn>
                                              <dx:GridViewDataTextColumn FieldName="PR0GRAMADO2026"  Name="PR0GRAMADO2026"  ReadOnly="true" Visible="true" VisibleIndex="2" Width="7%" Caption="Asignado">
                                    <PropertiesTextEdit DisplayFormatString="{0:N2}">
                                                            </PropertiesTextEdit>
                                                            <FilterCellStyle HorizontalAlign="Center"/>
                                                            <HeaderStyle Wrap="True" />
                                  </dx:GridViewDataTextColumn>

                                              </Columns>
                                      </dx:GridViewBandColumn>              

                                       
                                      
                                      <dx:GridViewBandColumn Caption="2,027"  Name="CLASPRES2027" VisibleIndex="12" Visible="True">
                                          <Columns>
                                               <dx:GridViewDataTextColumn FieldName="PRESUPUESTO2027"  Name="PRESUPUESTO2027"  ReadOnly="true" Visible="true" VisibleIndex="1" Width="7%" Caption="Presupuesto">
                                     <PropertiesTextEdit DisplayFormatString="{0:N2}">
                                                            </PropertiesTextEdit>
                                                            <FilterCellStyle HorizontalAlign="Center"/>
                                                            <HeaderStyle Wrap="True" />
                                  </dx:GridViewDataTextColumn>
                                              <dx:GridViewDataTextColumn FieldName="PR0GRAMADO2027"  Name="PR0GRAMADO2027"  ReadOnly="true" Visible="true" VisibleIndex="2" Width="7%" Caption="Asignado">
                                    <PropertiesTextEdit DisplayFormatString="{0:N2}">
                                                            </PropertiesTextEdit>
                                                            <FilterCellStyle HorizontalAlign="Center"/>
                                                            <HeaderStyle Wrap="True" />
                                  </dx:GridViewDataTextColumn>

                                              </Columns>
                                      </dx:GridViewBandColumn>




                                       <dx:GridViewBandColumn Caption="2,028"  Name="CLASPRES2028" VisibleIndex="13" Visible="True">
                                          <Columns>
                                               <dx:GridViewDataTextColumn FieldName="PRESUPUESTO2028"  Name="PRESUPUESTO2028"  ReadOnly="true" Visible="true" VisibleIndex="1" Width="7%" Caption="Presupuesto">
                                     <PropertiesTextEdit DisplayFormatString="{0:N2}">
                                                            </PropertiesTextEdit>
                                                            <FilterCellStyle HorizontalAlign="Center"/>
                                                            <HeaderStyle Wrap="True" />
                                  </dx:GridViewDataTextColumn>
                                              <dx:GridViewDataTextColumn FieldName="PR0GRAMADO2028"  Name="PR0GRAMADO2028"  ReadOnly="true" Visible="true" VisibleIndex="2" Width="7%" Caption="Asignado">
                                    <PropertiesTextEdit DisplayFormatString="{0:N2}">
                                                            </PropertiesTextEdit>
                                                            <FilterCellStyle HorizontalAlign="Center"/>
                                                            <HeaderStyle Wrap="True" />
                                  </dx:GridViewDataTextColumn>

                                              </Columns>
                                      </dx:GridViewBandColumn>

                                                                             <dx:GridViewBandColumn Caption="2,029"  Name="CLASPRES2029" VisibleIndex="14" Visible="True">
                                          <Columns>
                                               <dx:GridViewDataTextColumn FieldName="PRESUPUESTO2029"  Name="PRESUPUESTO2029"  ReadOnly="true" Visible="true" VisibleIndex="1" Width="7%" Caption="Presupuesto">
                                     <PropertiesTextEdit DisplayFormatString="{0:N2}">
                                                            </PropertiesTextEdit>
                                                            <FilterCellStyle HorizontalAlign="Center"/>
                                                            <HeaderStyle Wrap="True" />
                                  </dx:GridViewDataTextColumn>
                                              <dx:GridViewDataTextColumn FieldName="PR0GRAMADO2029"  Name="PR0GRAMADO2029"  ReadOnly="true" Visible="true" VisibleIndex="2" Width="7%" Caption="Asignado">
                                    <PropertiesTextEdit DisplayFormatString="{0:N2}">
                                                            </PropertiesTextEdit>
                                                            <FilterCellStyle HorizontalAlign="Center"/>
                                                            <HeaderStyle Wrap="True" />
                                  </dx:GridViewDataTextColumn>

                                              </Columns>
                                      </dx:GridViewBandColumn>


                                      <dx:GridViewBandColumn Caption="2,030"  Name="CLASPRES2030" VisibleIndex="15" Visible="True">
                                          <Columns>
                                               <dx:GridViewDataTextColumn FieldName="PRESUPUESTO2030"  Name="PRESUPUESTO2030"  ReadOnly="true" Visible="true" VisibleIndex="1" Width="7%" Caption="Presupuesto">
                                     <PropertiesTextEdit DisplayFormatString="{0:N2}">
                                                            </PropertiesTextEdit>
                                                            <FilterCellStyle HorizontalAlign="Center"/>
                                                            <HeaderStyle Wrap="True" />
                                  </dx:GridViewDataTextColumn>
                                              <dx:GridViewDataTextColumn FieldName="PR0GRAMADO2030"  Name="PR0GRAMADO2030"  ReadOnly="true" Visible="true" VisibleIndex="2" Width="7%" Caption="Asignado">
                                    <PropertiesTextEdit DisplayFormatString="{0:N2}">
                                                            </PropertiesTextEdit>
                                                            <FilterCellStyle HorizontalAlign="Center"/>
                                                            <HeaderStyle Wrap="True" />
                                  </dx:GridViewDataTextColumn>

                                              </Columns>
                                      </dx:GridViewBandColumn>


                                          <dx:GridViewBandColumn Caption="2,031"  Name="CLASPRES2031" VisibleIndex="16" Visible="True">
        <Columns>
             <dx:GridViewDataTextColumn FieldName="PRESUPUESTO2031"  Name="PRESUPUESTO2031"  ReadOnly="true" Visible="true" VisibleIndex="1" Width="7%" Caption="Presupuesto">
   <PropertiesTextEdit DisplayFormatString="{0:N2}">
                          </PropertiesTextEdit>
                          <FilterCellStyle HorizontalAlign="Center"/>
                          <HeaderStyle Wrap="True" />
</dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="PR0GRAMADO2031"  Name="PR0GRAMADO2031"  ReadOnly="true" Visible="true" VisibleIndex="2" Width="7%" Caption="Asignado">
  <PropertiesTextEdit DisplayFormatString="{0:N2}">
                          </PropertiesTextEdit>
                          <FilterCellStyle HorizontalAlign="Center"/>
                          <HeaderStyle Wrap="True" />
</dx:GridViewDataTextColumn>

            </Columns>
    </dx:GridViewBandColumn>


                                          </Columns>
                                  <SettingsDetail ShowDetailRow="true" />
                                 <Templates>
                        <DetailRow>
                            <dx:ASPxGridView ID="gvResultados"  KeyFieldName="LLAVE" SettingsBehavior-AllowFocusedRow ="true"  runat="server"  Width="100%" AutoGenerateColumns="False"    Theme="Office2010Blue"  OnBeforePerformDataSelect="gvResultados_BeforePerformDataSelect"  SettingsDetail-AllowOnlyOneMasterRowExpanded="true"   OnDetailRowExpandedChanged="gvResultados_DetailRowExpandedChanged" >
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
             <SettingsCommandButton DeleteButton-ButtonType="Button" DeleteButton-Text="Quitar">
                 <DeleteButton ButtonType="Button">
                 </DeleteButton>
             </SettingsCommandButton>
             <SettingsDataSecurity AllowDelete="true" AllowEdit="true" AllowInsert="False" />
             <SettingsLoadingPanel Mode="ShowOnStatusBar" />
             <SettingsText CommandApplyFilter="Aplicar filtro" CommandApplySearchPanelFilter="asdasfasdfasdf" ContextMenuHideColumn="Ocultar Columna" ContextMenuShowCustomizationWindow="Mostrar Columnas" ContextMenuShowFilterRow="Fila de Filtros" ContextMenuShowFilterRowMenu="Seleccione Tipo de Filtro" ContextMenuShowSearchPanel="Buscar" CustomizationWindowCaption="Columnas Ocultas" GroupPanel="Arrastre las columnas que desee agrupar" />
                                  <Columns>
                                      <dx:GridViewDataTextColumn FieldName="SPPRO$ID_PROGRAMA_PRESUPUESTO" VisibleIndex="0" Visible="false">
                                    </dx:GridViewDataTextColumn>
                                      <dx:GridViewDataTextColumn FieldName="ID_RESULTADO"  Name="ID_RESULTADO"  ReadOnly="true" Visible="false" VisibleIndex="1" Width="0%">
                                     <HeaderStyle Wrap="True" />
                                    </dx:GridViewDataTextColumn>
                                      
                                     
                                             <dx:GridViewDataTextColumn FieldName="RESULTADO"  Name="RESULTADO" Caption="VINCULACIÓN RESULTADO INSTITUCIONAL"  ReadOnly="true"  VisibleIndex="1" Width="50%">
                                     <HeaderStyle Wrap="True" />
                                    </dx:GridViewDataTextColumn> 
                                             <dx:GridViewDataTextColumn FieldName="EJE_ACCION"  Name="EJE_ACCION" Caption="VINCULACIÓN PGG" ReadOnly="true" VisibleIndex="2" Width="50%">
                                     <HeaderStyle Wrap="True" />
                                    </dx:GridViewDataTextColumn> 
                                         




                                       
                                             <dx:GridViewDataTextColumn FieldName="RED"  Name="RED" Caption="VINCULACIÓN RESULTADO ESTRATÉGICO RE"  ReadOnly="true"  VisibleIndex="3" Width="50%">
                                     <HeaderStyle Wrap="True" />
                                    </dx:GridViewDataTextColumn> 
                                             
                                      
                                             <dx:GridViewDataTextColumn FieldName="SPPRO$RESULTADO2"  Name="SPPRO$RESULTADO2"   ReadOnly="true"  VisibleIndex="4" Width="50%" Visible="false">
                                     <HeaderStyle Wrap="True" />
                                    </dx:GridViewDataTextColumn> 
                                        
                                               <dx:GridViewDataTextColumn FieldName="LLAVE"  Name="LLAVE"   ReadOnly="true"  VisibleIndex="5" Width="50%" Visible="false">
                                <HeaderStyle Wrap="True" />
                                    </dx:GridViewDataTextColumn> 

                                      </Columns>
                                  <SettingsDetail ShowDetailRow="true" />
                                 <Templates>
                                     <DetailRow>         
                          
                            <div class="row productHeader">
                                  <div style="justify-content:flex-start">
                              <h4> Ingreso de metas de Productos, presione el botón "Ingresar metas" (no puede programar metas financieras para productos)</h4>
                                    </div>
                              
                                    </div>
                            <dx:ASPxGridView ID="gvProductos"  KeyFieldName="SPPRO$ID_PRODUCTO" SettingsBehavior-AllowFocusedRow ="true"  runat="server"  Width="100%" AutoGenerateColumns="False"   OnBeforePerformDataSelect="gvProductos_BeforePerformDataSelect" Theme="Office2010Blue"   OnRowUpdating="gvProductos_RowUpdating"   OnCellEditorInitialize="gvProductos_CellEditorInitialize" SettingsDetail-AllowOnlyOneMasterRowExpanded="true"   OnDetailRowExpandedChanged="gvProductos_DetailRowExpandedChanged" OnCommandButtonInitialize="gvProductos_CommandButtonInitialize"  OnHtmlDataCellPrepared="gvProductos_HtmlDataCellPrepared">
                                <%--<Settings VerticalScrollBarMode="Visible" HorizontalScrollBarMode="Visible" />--%>
                                 <SettingsCommandButton DeleteButton-Text="Quitar" DeleteButton-ButtonType="Button">
                                        <UpdateButton ButtonType="Button" Text="Guardar">
                                            <Styles>
                                                <Style CssClass="btn-warging">
                                                </Style>
                                            </Styles>
                                        </UpdateButton>
                                        <CancelButton ButtonType="Button" Text="cancelar">
                                            <Styles>
                                                <FocusRectStyle CssClass="btn-danger">
                                                </FocusRectStyle>
                                            </Styles>
                                        </CancelButton>
                                        <EditButton ButtonType="Button" Text="Ingresar">
                                        </EditButton>
                                        <DeleteButton ButtonType="Button">
                                           
                                        </DeleteButton>
                                    </SettingsCommandButton>
                                <SettingsEditing Mode="Inline" />
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
                                <Columns>
                                     <dx:GridViewDataTextColumn FieldName="ID_RESULTADO" VisibleIndex="0" Visible="false">
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn FieldName="SPPRO$ID_PROGRAMA_PRESUPUESTO" VisibleIndex="1" Visible="false">
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn FieldName="SPPRO$ID_PRODUCTO" VisibleIndex="2" Visible="false">
                                    </dx:GridViewDataTextColumn>
                                   
                                    <dx:GridViewDataTextColumn FieldName="PRODUCTO" VisibleIndex="3" ReadOnly="true" Visible="true" Width="20%">
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn FieldName="MEDIDA_PRODUCTO" VisibleIndex="4" Caption="Medida" ReadOnly="true"  Width="8%" Visible="true">
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewCommandColumn ShowEditButton="True" Caption="Ingresar metas" VisibleIndex="5" Width="180">
                                        <HeaderStyle Wrap="True" />
                                    </dx:GridViewCommandColumn>
                                    <dx:GridViewBandColumn Caption="2020" VisibleIndex="6" Name="2020">
                                        <Columns>
                                            <dx:GridViewDataTextColumn FieldName="IDFP2020"  VisibleIndex="1" ReadOnly="true" Visible="false"></dx:GridViewDataTextColumn>
                                             <dx:GridViewDataTextColumn FieldName="TP2020"  VisibleIndex="2" ReadOnly="true" Visible="false"></dx:GridViewDataTextColumn>
                                            <dx:GridViewDataSpinEditColumn FieldName="MF2020" Width="8%" Caption="FISICA" VisibleIndex="3" ReadOnly="false" Visible="true">
                                                <PropertiesSpinEdit DisplayFormatString="N2" />
                                            </dx:GridViewDataSpinEditColumn>                                            
                                            <dx:GridViewDataTextColumn FieldName="MFIN2020"  Width="8%" ShowInCustomizationForm="True" Caption="FINANCIERA" VisibleIndex="4" ReadOnly="true">
                                                <PropertiesTextEdit DisplayFormatString="{0:N2}">
                                                            </PropertiesTextEdit>
                                                <CellStyle HorizontalAlign="Right"></CellStyle>
                                            </dx:GridViewDataTextColumn>   
                                            
                                        </Columns>
                                    </dx:GridViewBandColumn>
                                    <dx:GridViewBandColumn Caption="2021" VisibleIndex="7" Name ="2021">
                                        <Columns>
                                            <dx:GridViewDataTextColumn FieldName="IDFP2021"  VisibleIndex="1" ReadOnly="true" Visible="false"></dx:GridViewDataTextColumn>
                                             <dx:GridViewDataTextColumn FieldName="TP2021"  VisibleIndex="2" ReadOnly="true" Visible="false"></dx:GridViewDataTextColumn>
                                            <dx:GridViewDataSpinEditColumn FieldName="MF2021" Width="8%" Caption="FISICA" VisibleIndex="3" ReadOnly="false" Visible="true">
                                                <PropertiesSpinEdit DisplayFormatString="N2" />
                                            </dx:GridViewDataSpinEditColumn>                                            
                                            <dx:GridViewDataTextColumn FieldName="MFIN2021"  Width="8%" ShowInCustomizationForm="True" Caption="FINANCIERA" VisibleIndex="4" ReadOnly="true">
                                                <PropertiesTextEdit DisplayFormatString="{0:N2}">
                                                            </PropertiesTextEdit>
                                                <CellStyle HorizontalAlign="Right"></CellStyle>
                                            </dx:GridViewDataTextColumn> 
                                            
                                        </Columns>
                                    </dx:GridViewBandColumn>
                                    <dx:GridViewBandColumn Caption="2022" VisibleIndex="8" Name="2022">
                                        <Columns>
                                            <dx:GridViewDataTextColumn FieldName="IDFP2022"  VisibleIndex="1" ReadOnly="true" Visible="false"></dx:GridViewDataTextColumn>
                                             <dx:GridViewDataTextColumn FieldName="TP2022"  VisibleIndex="2" ReadOnly="true" Visible="false"></dx:GridViewDataTextColumn>
                                            <dx:GridViewDataSpinEditColumn FieldName="MF2022" Width="8%" Caption="FISICA" VisibleIndex="3" ReadOnly="false" Visible="true">
                                                <PropertiesSpinEdit DisplayFormatString="N2" />
                                            </dx:GridViewDataSpinEditColumn>                                            
                                            <dx:GridViewDataTextColumn FieldName="MFIN2022"  Width="8%" ShowInCustomizationForm="True" Caption="FINANCIERA" VisibleIndex="4" ReadOnly="true">
                                                <PropertiesTextEdit DisplayFormatString="{0:N2}">
                                                            </PropertiesTextEdit>
                                                <CellStyle HorizontalAlign="Right"></CellStyle>
                                            </dx:GridViewDataTextColumn>   
                                            
                                        </Columns>
                                    </dx:GridViewBandColumn>
                                    <dx:GridViewBandColumn Caption="2023" VisibleIndex="9" Name="2023">
                                        <Columns>
                                            <dx:GridViewDataTextColumn FieldName="IDFP2023"  VisibleIndex="1" ReadOnly="true" Visible="false"></dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn FieldName="TP2023"  VisibleIndex="2" ReadOnly="true" Visible="false"></dx:GridViewDataTextColumn>
                                            <dx:GridViewDataSpinEditColumn FieldName="MF2023" Width="8%" Caption="FISICA" VisibleIndex="3" ReadOnly="false" Visible="true">
                                                <PropertiesSpinEdit DisplayFormatString="N2" />
                                            </dx:GridViewDataSpinEditColumn>                                            
                                            <dx:GridViewDataTextColumn FieldName="MFIN2023"  Width="8%" ShowInCustomizationForm="True" Caption="FINANCIERA" VisibleIndex="4" ReadOnly="true">
                                                <PropertiesTextEdit DisplayFormatString="{0:N2}">
                                                            </PropertiesTextEdit>
                                                <CellStyle HorizontalAlign="Right"></CellStyle>
                                            </dx:GridViewDataTextColumn>  
                                            
                                        </Columns>
                                    </dx:GridViewBandColumn>
                                     <dx:GridViewBandColumn Caption="2024" VisibleIndex="10" Name="2024">
                                        <Columns>
                                            <dx:GridViewDataTextColumn FieldName="IDFP2024"  VisibleIndex="1" ReadOnly="true" Visible="false"></dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn FieldName="TP2024"  VisibleIndex="2" ReadOnly="true" Visible="false"></dx:GridViewDataTextColumn>
                                            <dx:GridViewDataSpinEditColumn FieldName="MF2024" Width="8%" Caption="FISICA" VisibleIndex="3" ReadOnly="false" Visible="true">
                                                <PropertiesSpinEdit DisplayFormatString="N2" />
                                            </dx:GridViewDataSpinEditColumn>                                            
                                            <dx:GridViewDataTextColumn FieldName="MFIN2024"  Width="8%" ShowInCustomizationForm="True" Caption="FINANCIERA" VisibleIndex="4" ReadOnly="true">
                                                <PropertiesTextEdit DisplayFormatString="{0:N2}">
                                                            </PropertiesTextEdit>
                                                <CellStyle HorizontalAlign="Right"></CellStyle>
                                            </dx:GridViewDataTextColumn>  
                                            
                                        </Columns>
                                    </dx:GridViewBandColumn>
                                    <dx:GridViewBandColumn Caption="2025" VisibleIndex="11" Name="2025">
                                        <Columns>
                                            <dx:GridViewDataTextColumn FieldName="IDFP2025"  VisibleIndex="1" ReadOnly="true" Visible="false"></dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn FieldName="TP2025"  VisibleIndex="2" ReadOnly="true" Visible="false"></dx:GridViewDataTextColumn>
                                            <dx:GridViewDataSpinEditColumn FieldName="MF2025" Width="8%" Caption="FISICA" VisibleIndex="3" ReadOnly="false" Visible="true">
                                                <PropertiesSpinEdit DisplayFormatString="N2" />
                                            </dx:GridViewDataSpinEditColumn>                                            
                                            <dx:GridViewDataTextColumn FieldName="MFIN2025"  Width="8%" ShowInCustomizationForm="True" Caption="FINANCIERA" VisibleIndex="4" ReadOnly="true">
                                                <PropertiesTextEdit DisplayFormatString="{0:N2}">
                                                            </PropertiesTextEdit>
                                                <CellStyle HorizontalAlign="Right"></CellStyle>
                                            </dx:GridViewDataTextColumn>  
                                            
                                        </Columns>
                                    </dx:GridViewBandColumn>


                                    <dx:GridViewBandColumn Caption="2026" VisibleIndex="11" Name="2026">
                                        <Columns>
                                            <dx:GridViewDataTextColumn FieldName="IDFP2026"  VisibleIndex="1" ReadOnly="true" Visible="false"></dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn FieldName="TP2026"  VisibleIndex="2" ReadOnly="true" Visible="false"></dx:GridViewDataTextColumn>
                                            <dx:GridViewDataSpinEditColumn FieldName="MF2026" Width="8%" Caption="FISICA" VisibleIndex="3" ReadOnly="false" Visible="true">
                                                <PropertiesSpinEdit DisplayFormatString="N2" />
                                            </dx:GridViewDataSpinEditColumn>                                            
                                            <dx:GridViewDataTextColumn FieldName="MFIN2026"  Width="8%" ShowInCustomizationForm="True" Caption="FINANCIERA" VisibleIndex="4" ReadOnly="true">
                                                <PropertiesTextEdit DisplayFormatString="{0:N2}">
                                                            </PropertiesTextEdit>
                                                <CellStyle HorizontalAlign="Right"></CellStyle>
                                            </dx:GridViewDataTextColumn>  
                                            
                                        </Columns>
                                    </dx:GridViewBandColumn>


                                    <dx:GridViewBandColumn Caption="2027" VisibleIndex="12" Name="2027">
                                        <Columns>
                                            <dx:GridViewDataTextColumn FieldName="IDFP2027"  VisibleIndex="1" ReadOnly="true" Visible="false"></dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn FieldName="TP2027"  VisibleIndex="2" ReadOnly="true" Visible="false"></dx:GridViewDataTextColumn>
                                            <dx:GridViewDataSpinEditColumn FieldName="MF2027" Width="8%" Caption="FISICA" VisibleIndex="3" ReadOnly="false" Visible="true">
                                                <PropertiesSpinEdit DisplayFormatString="N2" />
                                            </dx:GridViewDataSpinEditColumn>                                            
                                            <dx:GridViewDataTextColumn FieldName="MFIN2027"  Width="8%" ShowInCustomizationForm="True" Caption="FINANCIERA" VisibleIndex="4" ReadOnly="true">
                                                <PropertiesTextEdit DisplayFormatString="{0:N2}">
                                                            </PropertiesTextEdit>
                                                <CellStyle HorizontalAlign="Right"></CellStyle>
                                            </dx:GridViewDataTextColumn>  
                                            
                                        </Columns>
                                    </dx:GridViewBandColumn>


                                     <dx:GridViewBandColumn Caption="2028" VisibleIndex="12" Name="2028">
                                        <Columns>
                                            <dx:GridViewDataTextColumn FieldName="IDFP2028"  VisibleIndex="1" ReadOnly="true" Visible="false"></dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn FieldName="TP2028"  VisibleIndex="2" ReadOnly="true" Visible="false"></dx:GridViewDataTextColumn>
                                            <dx:GridViewDataSpinEditColumn FieldName="MF2028" Width="8%" Caption="FISICA" VisibleIndex="3" ReadOnly="false" Visible="true">
                                                <PropertiesSpinEdit DisplayFormatString="N2" />
                                            </dx:GridViewDataSpinEditColumn>                                            
                                            <dx:GridViewDataTextColumn FieldName="MFIN2028"  Width="8%" ShowInCustomizationForm="True" Caption="FINANCIERA" VisibleIndex="4" ReadOnly="true">
                                                <PropertiesTextEdit DisplayFormatString="{0:N2}">
                                                            </PropertiesTextEdit>
                                                <CellStyle HorizontalAlign="Right"></CellStyle>
                                            </dx:GridViewDataTextColumn>  
                                            
                                        </Columns>
                                    </dx:GridViewBandColumn>

                                    <dx:GridViewBandColumn Caption="2029" VisibleIndex="13" Name="2029">
                                        <Columns>
                                            <dx:GridViewDataTextColumn FieldName="IDFP2029"  VisibleIndex="1" ReadOnly="true" Visible="false"></dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn FieldName="TP2029"  VisibleIndex="2" ReadOnly="true" Visible="false"></dx:GridViewDataTextColumn>
                                            <dx:GridViewDataSpinEditColumn FieldName="MF2029" Width="8%" Caption="FISICA" VisibleIndex="3" ReadOnly="false" Visible="true">
                                                <PropertiesSpinEdit DisplayFormatString="N2" />
                                            </dx:GridViewDataSpinEditColumn>                                            
                                            <dx:GridViewDataTextColumn FieldName="MFIN2029"  Width="8%" ShowInCustomizationForm="True" Caption="FINANCIERA" VisibleIndex="4" ReadOnly="true">
                                                <PropertiesTextEdit DisplayFormatString="{0:N2}">
                                                            </PropertiesTextEdit>
                                                <CellStyle HorizontalAlign="Right"></CellStyle>
                                            </dx:GridViewDataTextColumn>  
                                            
                                        </Columns>
                                    </dx:GridViewBandColumn>

                                     <dx:GridViewBandColumn Caption="2030" VisibleIndex="14" Name="2030">
                                        <Columns>
                                            <dx:GridViewDataTextColumn FieldName="IDFP2030"  VisibleIndex="1" ReadOnly="true" Visible="false"></dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn FieldName="TP2030"  VisibleIndex="2" ReadOnly="true" Visible="false"></dx:GridViewDataTextColumn>
                                            <dx:GridViewDataSpinEditColumn FieldName="MF2030" Width="8%" Caption="FISICA" VisibleIndex="3" ReadOnly="false" Visible="true">
                                                <PropertiesSpinEdit DisplayFormatString="N2" />
                                            </dx:GridViewDataSpinEditColumn>                                            
                                            <dx:GridViewDataTextColumn FieldName="MFIN2030"  Width="8%" ShowInCustomizationForm="True" Caption="FINANCIERA" VisibleIndex="4" ReadOnly="true">
                                                <PropertiesTextEdit DisplayFormatString="{0:N2}">
                                                            </PropertiesTextEdit>
                                                <CellStyle HorizontalAlign="Right"></CellStyle>
                                            </dx:GridViewDataTextColumn>  
                                            
                                        </Columns>
                                    </dx:GridViewBandColumn>


                                    <dx:GridViewBandColumn Caption="2031" VisibleIndex="15" Name="2031">
                                        <Columns>
                                            <dx:GridViewDataTextColumn FieldName="IDFP2031"  VisibleIndex="1" ReadOnly="true" Visible="false"></dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn FieldName="TP2031"  VisibleIndex="2" ReadOnly="true" Visible="false"></dx:GridViewDataTextColumn>
                                            <dx:GridViewDataSpinEditColumn FieldName="MF2031" Width="8%" Caption="FISICA" VisibleIndex="3" ReadOnly="false" Visible="true">
                                                <PropertiesSpinEdit DisplayFormatString="N2" />
                                            </dx:GridViewDataSpinEditColumn>                                            
                                            <dx:GridViewDataTextColumn FieldName="MFIN2031"  Width="8%" ShowInCustomizationForm="True" Caption="FINANCIERA" VisibleIndex="4" ReadOnly="true">
                                                <PropertiesTextEdit DisplayFormatString="{0:N2}">
                                                            </PropertiesTextEdit>
                                                <CellStyle HorizontalAlign="Right"></CellStyle>
                                            </dx:GridViewDataTextColumn>  
                                            
                                        </Columns>
                                    </dx:GridViewBandColumn>

                                    </Columns>   
                                <SettingsDetail ShowDetailRow="true" />
                                 <Templates>
                                      <DetailRow>
                                          <div class="row productHeader">
                                  <div style="justify-content:flex-start">
                              <h4>&nbsp;&nbsp;&nbsp;Programación de metas de Subproductos, presione el botón "Ingresar metas"</h4>
                                    </div>
                              
                                    </div>
                                          <dx:ASPxGridView ID="gvsubProductos"    runat="server"  Theme="Office2010Blue" OnBeforePerformDataSelect="gvsubProductos_BeforePerformDataSelect"  KeyFieldName="SPPSUB$ID_SUBPRODUCTO" SettingsBehavior-AllowFocusedRow="true"    Width="100%" AutoGenerateColumns="False" OnRowUpdating="gvsubProductos_RowUpdating" OnCommandButtonInitialize="gvsubProductos_CommandButtonInitialize"  OnHtmlDataCellPrepared="gvsubProductos_HtmlDataCellPrepared"  OnCellEditorInitialize="gvsubProductos_CellEditorInitialize">
                                           <SettingsCommandButton DeleteButton-Text="Quitar" DeleteButton-ButtonType="Button">
                                        <UpdateButton ButtonType="Button" Text="Guardar">
                                            <Styles>
                                                <Style CssClass="btn-warging">
                                                </Style>
                                            </Styles>
                                        </UpdateButton>
                                        <CancelButton ButtonType="Button" Text="cancelar">
                                            <Styles>
                                                <FocusRectStyle CssClass="btn-danger">
                                                </FocusRectStyle>
                                            </Styles>
                                        </CancelButton>
                                        <EditButton ButtonType="Button" Text="Ingresar">
                                        </EditButton>
                                        <DeleteButton ButtonType="Button">
                                           
                                        </DeleteButton>
                                    </SettingsCommandButton>
                                <SettingsEditing Mode="Inline" />
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
                                              
                                <Columns>
                                              <dx:GridViewDataTextColumn FieldName="SPPRO$ID_PRODUCTO" VisibleIndex="0" Visible="false">
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn FieldName="SPPSUB$ID_SUBPRODUCTO" VisibleIndex="1" Visible="false">
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn FieldName="SUBPRODUCTO"  Caption="Subproducto" VisibleIndex="2" ReadOnly="true" Visible="true" Width="20%">
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Metas por territorio" VisibleIndex="3" Width="10%">
    <DataItemTemplate>
        <asp:Panel ID="pnlBtn" runat="server">
          <dx:ASPxButton ID="btnMetasMuno" runat="server"
    Text="Metas por territorio"
    AutoPostBack="true"
    UseSubmitBehavior="true"
    CausesValidation="false"
    CommandArgument='<%# Eval("SPPSUB$ID_SUBPRODUCTO") %>'
    OnClick="btnMetasMuno_Click" />
        </asp:Panel>
    </DataItemTemplate>
</dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn FieldName="MUNICIPIOS"  
                                       Caption="Número de municipios<br/>priorizados" 
                                        VisibleIndex="3" ReadOnly="true" Visible="true" Width="20%">
                                         <HeaderStyle HorizontalAlign="Center" />
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn FieldName="MEDIDA_SUBPRODUCTO" VisibleIndex="4" Caption="Medida" ReadOnly="true"  Width="8%" Visible="true">
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn FieldName="SPPSUB$SNIP" VisibleIndex="5" Caption="SNIP" ReadOnly="true"  Width="8%" Visible="true">
                                    </dx:GridViewDataTextColumn>
                                     <dx:GridViewCommandColumn ShowEditButton="True" Caption="Ingresar metas" VisibleIndex="6" Width="180">
                                        <HeaderStyle Wrap="True" />
                                    </dx:GridViewCommandColumn>
                                    <dx:GridViewBandColumn Caption="2020" VisibleIndex="7" Name="2020">
                                        <Columns>
                                            <dx:GridViewDataTextColumn FieldName="IDFISUB2020"  VisibleIndex="1" ReadOnly="true" Visible="false"></dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn FieldName="IDFINSUB2020" VisibleIndex="2"  ReadOnly="true" Visible="false"></dx:GridViewDataTextColumn>
                                             <dx:GridViewDataTextColumn FieldName="TSF2020"  VisibleIndex="3" ReadOnly="true" Visible="false"></dx:GridViewDataTextColumn>
                                             <dx:GridViewDataTextColumn FieldName="TSFIN2020"  VisibleIndex="4" ReadOnly="true" Visible="false"></dx:GridViewDataTextColumn>
                                            <dx:GridViewDataSpinEditColumn FieldName="MFSUB2020" Width="7%" Caption="FISICA" VisibleIndex="5" ReadOnly="false" Visible="true">
                                                <PropertiesSpinEdit DisplayFormatString="N2" />
                                            </dx:GridViewDataSpinEditColumn> 
                                            <dx:GridViewDataSpinEditColumn FieldName="MFINSUB2020" Width="7%" Caption="FINANCIERA" VisibleIndex="6" ReadOnly="false" Visible="true">
                                                <PropertiesSpinEdit DisplayFormatString="N2" />
                                            </dx:GridViewDataSpinEditColumn>                                              
                                                                                            
                                        </Columns>
                                    </dx:GridViewBandColumn>
                                    <dx:GridViewBandColumn Caption="2021" VisibleIndex="6" Name="2021">
                                        <Columns>
                                            <dx:GridViewDataTextColumn FieldName="IDFISUB2021"  VisibleIndex="1" ReadOnly="true" Visible="false"></dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn FieldName="IDFINSUB2021" VisibleIndex="2"  ReadOnly="true" Visible="false"></dx:GridViewDataTextColumn>
                                             <dx:GridViewDataTextColumn FieldName="TSF2021"  VisibleIndex="3" ReadOnly="true" Visible="false"></dx:GridViewDataTextColumn>
                                             <dx:GridViewDataTextColumn FieldName="TSFIN2021"  VisibleIndex="4" ReadOnly="true" Visible="false"></dx:GridViewDataTextColumn>                                            
                                            <dx:GridViewDataSpinEditColumn FieldName="MFSUB2021" Width="7%" Caption="FISICA" VisibleIndex="5" ReadOnly="false" Visible="true">
                                                <PropertiesSpinEdit DisplayFormatString="N2" />
                                            </dx:GridViewDataSpinEditColumn> 
                                            <dx:GridViewDataSpinEditColumn FieldName="MFINSUB2021" Width="7%" Caption="FINANCIERA" VisibleIndex="6" ReadOnly="false" Visible="true">
                                                <PropertiesSpinEdit DisplayFormatString="N2" />
                                            </dx:GridViewDataSpinEditColumn> 
                                            
                                        </Columns>
                                    </dx:GridViewBandColumn>
                                    <dx:GridViewBandColumn Caption="2022" VisibleIndex="7" Name="2022">
                                        <Columns>
                                            <dx:GridViewDataTextColumn FieldName="IDFISUB2022"  VisibleIndex="1" ReadOnly="true" Visible="false"></dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn FieldName="IDFINSUB2022" VisibleIndex="2"  ReadOnly="true" Visible="false"></dx:GridViewDataTextColumn>
                                             <dx:GridViewDataTextColumn FieldName="TSF2022"  VisibleIndex="3" ReadOnly="true" Visible="false"></dx:GridViewDataTextColumn>
                                             <dx:GridViewDataTextColumn FieldName="TSFIN2022"  VisibleIndex="4" ReadOnly="true" Visible="false"></dx:GridViewDataTextColumn>
                                            
                                            <dx:GridViewDataSpinEditColumn FieldName="MFSUB2022" Width="7%" Caption="FISICA" VisibleIndex="4" ReadOnly="false" Visible="true">
                                                <PropertiesSpinEdit DisplayFormatString="N2" />
                                            </dx:GridViewDataSpinEditColumn> 
                                            <dx:GridViewDataSpinEditColumn FieldName="MFINSUB2022" Width="7%" Caption="FINANCIERA" VisibleIndex="5" ReadOnly="false" Visible="true">
                                                <PropertiesSpinEdit DisplayFormatString="N2" />
                                            </dx:GridViewDataSpinEditColumn> 
                                            
                                        </Columns>
                                    </dx:GridViewBandColumn>
                                    <dx:GridViewBandColumn Caption="2023" VisibleIndex="8" Name="2023">
                                        <Columns>
                                            <dx:GridViewDataTextColumn FieldName="IDFISUB2023"  VisibleIndex="1" ReadOnly="true" Visible="false"></dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn FieldName="IDFINSUB2023" VisibleIndex="2"  ReadOnly="true" Visible="false"></dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn FieldName="TSF2023"  VisibleIndex="3" ReadOnly="true" Visible="false"></dx:GridViewDataTextColumn>
                                             <dx:GridViewDataTextColumn FieldName="TSFIN2023"  VisibleIndex="4" ReadOnly="true" Visible="false"></dx:GridViewDataTextColumn>                                            
                                            <dx:GridViewDataSpinEditColumn FieldName="MFSUB2023" Width="7%" Caption="FISICA" VisibleIndex="5" ReadOnly="false" Visible="true">
                                                <PropertiesSpinEdit DisplayFormatString="N2" />
                                            </dx:GridViewDataSpinEditColumn> 
                                            <dx:GridViewDataSpinEditColumn FieldName="MFINSUB2023" Width="7%" Caption="FINANCIERA" VisibleIndex="6" ReadOnly="false" Visible="true">
                                                <PropertiesSpinEdit DisplayFormatString="N2" />
                                            </dx:GridViewDataSpinEditColumn>    
                                            
                                        </Columns>
                                    </dx:GridViewBandColumn>
                                    <dx:GridViewBandColumn Caption="2024" VisibleIndex="9" Name="2024">
                                        <Columns>
                                            <dx:GridViewDataTextColumn FieldName="IDFISUB2024"  VisibleIndex="1" ReadOnly="true" Visible="false"></dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn FieldName="IDFINSUB2024" VisibleIndex="2"  ReadOnly="true" Visible="false"></dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn FieldName="TSF2024"  VisibleIndex="3" ReadOnly="true" Visible="false"></dx:GridViewDataTextColumn>
                                             <dx:GridViewDataTextColumn FieldName="TSFIN2024"  VisibleIndex="4" ReadOnly="true" Visible="false"></dx:GridViewDataTextColumn>                                            
                                            <dx:GridViewDataSpinEditColumn FieldName="MFSUB2024" Width="7%" Caption="FISICA" VisibleIndex="5" ReadOnly="false" Visible="true">
                                                <PropertiesSpinEdit DisplayFormatString="N2" />
                                            </dx:GridViewDataSpinEditColumn> 
                                            <dx:GridViewDataSpinEditColumn FieldName="MFINSUB2024" Width="7%" Caption="FINANCIERA" VisibleIndex="6" ReadOnly="false" Visible="true">
                                                <PropertiesSpinEdit DisplayFormatString="N2" />
                                            </dx:GridViewDataSpinEditColumn>    
                                            
                                        </Columns>
                                    </dx:GridViewBandColumn>

                                    <dx:GridViewBandColumn Caption="2025" VisibleIndex="10" Name="2025">
                                        <Columns>
                                           <dx:GridViewDataTextColumn FieldName="IDFISUB2025"  VisibleIndex="1" ReadOnly="true" Visible="false"></dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn FieldName="IDFINSUB2025" VisibleIndex="2"  ReadOnly="true" Visible="false"></dx:GridViewDataTextColumn>
                                             <dx:GridViewDataTextColumn FieldName="TSF2025"  VisibleIndex="3" ReadOnly="true" Visible="false"></dx:GridViewDataTextColumn>
                                             <dx:GridViewDataTextColumn FieldName="TSFIN2025"  VisibleIndex="4" ReadOnly="true" Visible="false"></dx:GridViewDataTextColumn>                                            
                                            <dx:GridViewDataSpinEditColumn FieldName="MFSUB2025" Width="7%" Caption="FISICA" VisibleIndex="5" ReadOnly="false" Visible="true">
                                                <PropertiesSpinEdit DisplayFormatString="N2" />
                                            </dx:GridViewDataSpinEditColumn> 
                                            <dx:GridViewDataSpinEditColumn FieldName="MFINSUB2025" Width="7%" Caption="FINANCIERA" VisibleIndex="6" ReadOnly="false" Visible="true">
                                                <PropertiesSpinEdit DisplayFormatString="N2" />
                                            </dx:GridViewDataSpinEditColumn> 
                                            
                                        </Columns>
                                    </dx:GridViewBandColumn>



                                     <dx:GridViewBandColumn Caption="2026" VisibleIndex="10" Name="2026">
                                        <Columns>
                                           <dx:GridViewDataTextColumn FieldName="IDFISUB2026"  VisibleIndex="1" ReadOnly="true" Visible="false"></dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn FieldName="IDFINSUB2026" VisibleIndex="2"  ReadOnly="true" Visible="false"></dx:GridViewDataTextColumn>
                                             <dx:GridViewDataTextColumn FieldName="TSF2026"  VisibleIndex="3" ReadOnly="true" Visible="false"></dx:GridViewDataTextColumn>
                                             <dx:GridViewDataTextColumn FieldName="TSFIN2026"  VisibleIndex="4" ReadOnly="true" Visible="false"></dx:GridViewDataTextColumn>                                            
                                            <dx:GridViewDataSpinEditColumn FieldName="MFSUB2026" Width="7%" Caption="FISICA" VisibleIndex="5" ReadOnly="false" Visible="true">
                                                <PropertiesSpinEdit DisplayFormatString="N2" />
                                            </dx:GridViewDataSpinEditColumn> 
                                            <dx:GridViewDataSpinEditColumn FieldName="MFINSUB2026" Width="7%" Caption="FINANCIERA" VisibleIndex="6" ReadOnly="false" Visible="true">
                                                <PropertiesSpinEdit DisplayFormatString="N2" />
                                            </dx:GridViewDataSpinEditColumn> 
                                            
                                        </Columns>
                                    </dx:GridViewBandColumn>



                                     <dx:GridViewBandColumn Caption="2027" VisibleIndex="10" Name="2027">
                                        <Columns>
                                           <dx:GridViewDataTextColumn FieldName="IDFISUB2027"  VisibleIndex="1" ReadOnly="true" Visible="false"></dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn FieldName="IDFINSUB2027" VisibleIndex="2"  ReadOnly="true" Visible="false"></dx:GridViewDataTextColumn>
                                             <dx:GridViewDataTextColumn FieldName="TSF2027"  VisibleIndex="3" ReadOnly="true" Visible="false"></dx:GridViewDataTextColumn>
                                             <dx:GridViewDataTextColumn FieldName="TSFIN2027"  VisibleIndex="4" ReadOnly="true" Visible="false"></dx:GridViewDataTextColumn>                                            
                                            <dx:GridViewDataSpinEditColumn FieldName="MFSUB2027" Width="7%" Caption="FISICA" VisibleIndex="5" ReadOnly="false" Visible="true">
                                                <PropertiesSpinEdit DisplayFormatString="N2" />
                                            </dx:GridViewDataSpinEditColumn> 
                                            <dx:GridViewDataSpinEditColumn FieldName="MFINSUB2027" Width="7%" Caption="FINANCIERA" VisibleIndex="6" ReadOnly="false" Visible="true">
                                                <PropertiesSpinEdit DisplayFormatString="N2" />
                                            </dx:GridViewDataSpinEditColumn> 
                                            
                                        </Columns>
                                    </dx:GridViewBandColumn>




                                     <dx:GridViewBandColumn Caption="2028" VisibleIndex="11" Name="2028">
                                        <Columns>
                                           <dx:GridViewDataTextColumn FieldName="IDFISUB2028"  VisibleIndex="1" ReadOnly="true" Visible="false"></dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn FieldName="IDFINSUB2028" VisibleIndex="2"  ReadOnly="true" Visible="false"></dx:GridViewDataTextColumn>
                                             <dx:GridViewDataTextColumn FieldName="TSF2028"  VisibleIndex="3" ReadOnly="true" Visible="false"></dx:GridViewDataTextColumn>
                                             <dx:GridViewDataTextColumn FieldName="TSFIN2028"  VisibleIndex="4" ReadOnly="true" Visible="false"></dx:GridViewDataTextColumn>                                            
                                            <dx:GridViewDataSpinEditColumn FieldName="MFSUB2028" Width="7%" Caption="FISICA" VisibleIndex="5" ReadOnly="false" Visible="true">
                                                <PropertiesSpinEdit DisplayFormatString="N2" />
                                            </dx:GridViewDataSpinEditColumn> 
                                            <dx:GridViewDataSpinEditColumn FieldName="MFINSUB2028" Width="7%" Caption="FINANCIERA" VisibleIndex="6" ReadOnly="false" Visible="true">
                                                <PropertiesSpinEdit DisplayFormatString="N2" />
                                            </dx:GridViewDataSpinEditColumn> 
                                            
                                        </Columns>
                                    </dx:GridViewBandColumn>


                                    <dx:GridViewBandColumn Caption="2029" VisibleIndex="12" Name="2029">
                                        <Columns>
                                           <dx:GridViewDataTextColumn FieldName="IDFISUB2029"  VisibleIndex="1" ReadOnly="true" Visible="false"></dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn FieldName="IDFINSUB2029" VisibleIndex="2"  ReadOnly="true" Visible="false"></dx:GridViewDataTextColumn>
                                             <dx:GridViewDataTextColumn FieldName="TSF2029"  VisibleIndex="3" ReadOnly="true" Visible="false"></dx:GridViewDataTextColumn>
                                             <dx:GridViewDataTextColumn FieldName="TSFIN2029"  VisibleIndex="4" ReadOnly="true" Visible="false"></dx:GridViewDataTextColumn>                                            
                                            <dx:GridViewDataSpinEditColumn FieldName="MFSUB2029" Width="7%" Caption="FISICA" VisibleIndex="5" ReadOnly="false" Visible="true">
                                                <PropertiesSpinEdit DisplayFormatString="N2" />
                                            </dx:GridViewDataSpinEditColumn> 
                                            <dx:GridViewDataSpinEditColumn FieldName="MFINSUB2029" Width="7%" Caption="FINANCIERA" VisibleIndex="6" ReadOnly="false" Visible="true">
                                                <PropertiesSpinEdit DisplayFormatString="N2" />
                                            </dx:GridViewDataSpinEditColumn> 
                                            
                                        </Columns>
                                    </dx:GridViewBandColumn>


                                      <dx:GridViewBandColumn Caption="2030" VisibleIndex="13" Name="2030">
                                        <Columns>
                                           <dx:GridViewDataTextColumn FieldName="IDFISUB2030"  VisibleIndex="1" ReadOnly="true" Visible="false"></dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn FieldName="IDFINSUB2030" VisibleIndex="2"  ReadOnly="true" Visible="false"></dx:GridViewDataTextColumn>
                                             <dx:GridViewDataTextColumn FieldName="TSF2030"  VisibleIndex="3" ReadOnly="true" Visible="false"></dx:GridViewDataTextColumn>
                                             <dx:GridViewDataTextColumn FieldName="TSFIN2030"  VisibleIndex="4" ReadOnly="true" Visible="false"></dx:GridViewDataTextColumn>                                            
                                            <dx:GridViewDataSpinEditColumn FieldName="MFSUB2030" Width="7%" Caption="FISICA" VisibleIndex="5" ReadOnly="false" Visible="true">
                                                <PropertiesSpinEdit DisplayFormatString="N2" />
                                            </dx:GridViewDataSpinEditColumn> 
                                            <dx:GridViewDataSpinEditColumn FieldName="MFINSUB2030" Width="7%" Caption="FINANCIERA" VisibleIndex="6" ReadOnly="false" Visible="true">
                                                <PropertiesSpinEdit DisplayFormatString="N2" />
                                            </dx:GridViewDataSpinEditColumn> 
                                            
                                        </Columns>
                                    </dx:GridViewBandColumn>

                                      <dx:GridViewBandColumn Caption="2031" VisibleIndex="14" Name="2031">
    <Columns>
       <dx:GridViewDataTextColumn FieldName="IDFISUB2031"  VisibleIndex="1" ReadOnly="true" Visible="false"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="IDFINSUB2031" VisibleIndex="2"  ReadOnly="true" Visible="false"></dx:GridViewDataTextColumn>
         <dx:GridViewDataTextColumn FieldName="TSF2031"  VisibleIndex="3" ReadOnly="true" Visible="false"></dx:GridViewDataTextColumn>
         <dx:GridViewDataTextColumn FieldName="TSFIN2031"  VisibleIndex="4" ReadOnly="true" Visible="false"></dx:GridViewDataTextColumn>                                            
        <dx:GridViewDataSpinEditColumn FieldName="MFSUB2031" Width="7%" Caption="FISICA" VisibleIndex="5" ReadOnly="false" Visible="true">
            <PropertiesSpinEdit DisplayFormatString="N2" />
        </dx:GridViewDataSpinEditColumn> 
        <dx:GridViewDataSpinEditColumn FieldName="MFINSUB2031" Width="7%" Caption="FINANCIERA" VisibleIndex="6" ReadOnly="false" Visible="true">
            <PropertiesSpinEdit DisplayFormatString="N2" />
        </dx:GridViewDataSpinEditColumn> 
        
    </Columns>
</dx:GridViewBandColumn>

                                            
                                    </Columns>
                                              </dx:ASPxGridView>
                                          </DetailRow>
                                     </Templates>
                        </dx:ASPxGridView>
                        </DetailRow>
                                 </Templates>

                            </dx:ASPxGridView>

                        </DetailRow>
                    </Templates>
                              </dx:ASPxGridView>
                          <!-- </ContentTemplate>
                          </asp:UpdatePanel>-->
                         </div>
                 </asp:Panel>

             </asp:View>
            <asp:View runat="server" ID="vstPOA">
                  <asp:Panel ID="panPOA" runat="server">
                    <%-- <div  style ="overflow:auto; width:100%; height:90%; padding: 1px 10px 1px 10px;   ">--%>
                      <h4 style="color:#2d572c">Programación POA, asignación de metas financieras  mensuales para productos/subproductos</h4>
                  <div class="form-group" >
                           <label for="txtsubCod" aria-required="true"><font color="red">*</font>Seleccione año de POA a programar: </label>
                         <asp:UpdatePanel ID="upNRD" runat="server">
                          <ContentTemplate>    
                         <dx:ASPxComboBox ID="cbAniPOA" runat="server" NullText="Seleccione al anio de POA"  AutoPostBack="True" SelectedIndex="0" Theme="Office2010Blue" CssClass="form-control" Width="400" OnValueChanged="cbAniPOA_ValueChanged">                         
                                               
                         </dx:ASPxComboBox>
                         </ContentTemplate>
                             </asp:UpdatePanel>
                      </div>
                         <div class="form-group" >
                           <label for="txtsubCod" aria-required="true"><font color="red">*</font>Tipo de producción: </label>
                         <asp:UpdatePanel ID="upNS" runat="server">
                          <ContentTemplate>    
                         <dx:ASPxComboBox ID="cbTipoProd" runat="server" NullText="Seleccione el tipo productos que necesite desplegar"  AutoPostBack="True" SelectedIndex="0" Theme="Office2010Blue" CssClass="form-control" Width="400"  OnValueChanged="cbTipoProd_ValueChanged">
                           
                             <Items>
                                 <dx:ListEditItem Selected="True" Text="Resultado Institucional" Value="0" />
                                 <dx:ListEditItem Text="Resultados Estratégicos RE" Value="1" />
                             </Items>
                           
                         </dx:ASPxComboBox>
                         </ContentTemplate>
                             </asp:UpdatePanel>
                             </div>
                           <div class="form-group">
                                             <div class="btn-group" style="display:flex; justify-content:flex-start;">                                  
                                                 <asp:UpdatePanel ID="UpdatePanel2" runat="server">   
                                                     <ContentTemplate>
                                     <asp:Button ID="btnResfrescar" runat="server" Text="Actualizar montos"  CssClass="btn btn-primary"  OnClick="btnResfrescar_Click" />   
                                     
                                         
                                                         <asp:Button ID="btnGenerar" runat="server" Text="Reporte"  CssClass="btn" style ="background-color:darkcyan;color:white"  OnClick="btnGenerar_Click" />  
                                   </ContentTemplate>
                                                         </asp:UpdatePanel>
                                                     </div>
                                         </div>
                        <%-- <asp:UpdatePanel ID="upNT" runat="server">
                             <ContentTemplate>--%>
                                 <dx:ASPxGridView ID="gvPOA" runat="server"  KeyFieldName="SPPRO$ID_PROGRAMA_PRESUPUESTO"  Theme="Office2010Blue" Width="100%"  SettingsBehavior-AllowFocusedRow ="true" SettingsBehavior-ConfirmDelete="true"  SettingsDetail-AllowOnlyOneMasterRowExpanded="true"   OnDetailRowExpandedChanged="gvPOA_DetailRowExpandedChanged" ClientInstanceName="grid_resultados_POA">
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
             <SettingsCommandButton DeleteButton-ButtonType="Button" DeleteButton-Text="Quitar">
                 <DeleteButton ButtonType="Button">
                 </DeleteButton>
             </SettingsCommandButton>
             <SettingsDataSecurity AllowDelete="true" AllowEdit="true" AllowInsert="False" />
             <SettingsLoadingPanel Mode="ShowOnStatusBar" />
             <SettingsText CommandApplyFilter="Aplicar filtro" CommandApplySearchPanelFilter="asdasfasdfasdf" ContextMenuHideColumn="Ocultar Columna" ContextMenuShowCustomizationWindow="Mostrar Columnas" ContextMenuShowFilterRow="Fila de Filtros" ContextMenuShowFilterRowMenu="Seleccione Tipo de Filtro" ContextMenuShowSearchPanel="Buscar" CustomizationWindowCaption="Columnas Ocultas" GroupPanel="Arrastre las columnas que desee agrupar" />
                                 <Columns>
                                       
                                      <dx:GridViewDataTextColumn FieldName="SPPRO$ID_PROGRAMA_PRESUPUESTO"  Name="SPPRO$ID_PROGRAMA_PRESUPUESTO"   ReadOnly="true" Visible="false" VisibleIndex="0" >
                                     <HeaderStyle Wrap="True" />
                                    </dx:GridViewDataTextColumn>
                                      <dx:GridViewDataTextColumn FieldName="PROGRAMA"  Name="PROGRAMA"  ReadOnly="true" Visible="true" VisibleIndex="1"  Caption="Programa Presupuestario" >
                                     <PropertiesTextEdit>
                    <Style HorizontalAlign="Center">
                    </Style>
                </PropertiesTextEdit>
                                    </dx:GridViewDataTextColumn>
                                     <dx:GridViewDataTextColumn FieldName="ANUAL"  Name="ANUAL"  ReadOnly="true" Visible="true" VisibleIndex="2"  Caption="PRESUPUESTOANUAL">
                                     <PropertiesTextEdit DisplayFormatString="{0:N2}">
                                                            </PropertiesTextEdit>
                                                            <FilterCellStyle HorizontalAlign="Center"/>
                                                            <HeaderStyle Wrap="True" />
                                  </dx:GridViewDataTextColumn>
                                     <dx:GridViewDataTextColumn FieldName="MES1"  Name="MES1"  ReadOnly="true" Visible="true" VisibleIndex="3"  Caption="Enero">
                                     <PropertiesTextEdit DisplayFormatString="{0:N2}">
                                                            </PropertiesTextEdit>
                                                            <FilterCellStyle HorizontalAlign="Center"/>
                                                            <HeaderStyle Wrap="True" />
                                  </dx:GridViewDataTextColumn>
                                              <dx:GridViewDataTextColumn FieldName="MES2"  Name="MES2"  ReadOnly="true" Visible="true" VisibleIndex="4"  Caption="Febrero">
                                     <PropertiesTextEdit DisplayFormatString="{0:N2}">
                                                            </PropertiesTextEdit>
                                                            <FilterCellStyle HorizontalAlign="Center"/>
                                                            <HeaderStyle Wrap="True" />
                                  </dx:GridViewDataTextColumn>
                                                          <dx:GridViewDataTextColumn FieldName="MES3"  Name="MES3"  ReadOnly="true" Visible="true" VisibleIndex="5" Caption="Marzo">
                                     <PropertiesTextEdit DisplayFormatString="{0:N2}">
                                                            </PropertiesTextEdit>
                                                            <FilterCellStyle HorizontalAlign="Center"/>
                                                            <HeaderStyle Wrap="True" />
                                  </dx:GridViewDataTextColumn>     
                                      <dx:GridViewDataTextColumn FieldName="MES4"  Name="MES4"  ReadOnly="true" Visible="true" VisibleIndex="6"  Caption="Abril">
                                     <PropertiesTextEdit DisplayFormatString="{0:N2}">
                                                            </PropertiesTextEdit>
                                                            <FilterCellStyle HorizontalAlign="Center"/>
                                                            <HeaderStyle Wrap="True" />
                                  </dx:GridViewDataTextColumn> 

                                      <dx:GridViewDataTextColumn FieldName="CUA1"  Name="CUA1"  ReadOnly="true" Visible="true" VisibleIndex="7" Caption="1er cuatrimestre">
                                     <PropertiesTextEdit DisplayFormatString="{0:N2}">
                                                            </PropertiesTextEdit>
                                                            <FilterCellStyle HorizontalAlign="Center"/>
                                                            <HeaderStyle Wrap="True" />
                                  </dx:GridViewDataTextColumn>

                                      <dx:GridViewDataTextColumn FieldName="MES5"  Name="MES5"  ReadOnly="true" Visible="true" VisibleIndex="8"  Caption="Mayo">
                                     <PropertiesTextEdit DisplayFormatString="{0:N2}">
                                                            </PropertiesTextEdit>
                                                            <FilterCellStyle HorizontalAlign="Center"/>
                                                            <HeaderStyle Wrap="True" />
                                  </dx:GridViewDataTextColumn> 
                                      <dx:GridViewDataTextColumn FieldName="MES6"  Name="MES6"  ReadOnly="true" Visible="true" VisibleIndex="9" Caption="Junio">
                                     <PropertiesTextEdit DisplayFormatString="{0:N2}">
                                                            </PropertiesTextEdit>
                                                            <FilterCellStyle HorizontalAlign="Center"/>
                                                            <HeaderStyle Wrap="True" />
                                  </dx:GridViewDataTextColumn> 
                                      <dx:GridViewDataTextColumn FieldName="MES7"  Name="MES7"  ReadOnly="true" Visible="true" VisibleIndex="10" Caption="Julio">
                                     <PropertiesTextEdit DisplayFormatString="{0:N2}">
                                                            </PropertiesTextEdit>
                                                            <FilterCellStyle HorizontalAlign="Center"/>
                                                            <HeaderStyle Wrap="True" />
                                  </dx:GridViewDataTextColumn> 
                                       <dx:GridViewDataTextColumn FieldName="MES8"  Name="MES8"  ReadOnly="true" Visible="true" VisibleIndex="11" Caption="Agosto">
                                     <PropertiesTextEdit DisplayFormatString="{0:N2}">
                                                            </PropertiesTextEdit>
                                                            <FilterCellStyle HorizontalAlign="Center"/>
                                                            <HeaderStyle Wrap="True" />
                                  </dx:GridViewDataTextColumn> 
                                     <dx:GridViewDataTextColumn FieldName="CUA2"  Name="CUA2"  ReadOnly="true" Visible="true" VisibleIndex="12"  Caption="2do cuatrimestre">
                                     <PropertiesTextEdit DisplayFormatString="{0:N2}">
                                                            </PropertiesTextEdit>
                                                            <FilterCellStyle HorizontalAlign="Center"/>
                                                            <HeaderStyle Wrap="True" />
                                  </dx:GridViewDataTextColumn>
                                     <dx:GridViewDataTextColumn FieldName="MES9"  Name="MES9"  ReadOnly="true" Visible="true" VisibleIndex="13" Caption="Septiembre">
                                     <PropertiesTextEdit DisplayFormatString="{0:N2}">
                                                            </PropertiesTextEdit>
                                                            <FilterCellStyle HorizontalAlign="Center"/>
                                                            <HeaderStyle Wrap="True" />
                                  </dx:GridViewDataTextColumn> 
                                      <dx:GridViewDataTextColumn FieldName="MES10"  Name="MES10"  ReadOnly="true" Visible="true" VisibleIndex="14"  Caption="Octubre">
                                     <PropertiesTextEdit DisplayFormatString="{0:N2}">
                                                            </PropertiesTextEdit>
                                                            <FilterCellStyle HorizontalAlign="Center"/>
                                                            <HeaderStyle Wrap="True" />
                                  </dx:GridViewDataTextColumn>
                                      <dx:GridViewDataTextColumn FieldName="MES11"  Name="MES11"  ReadOnly="true" Visible="true" VisibleIndex="15"  Caption="Noviembre">
                                     <PropertiesTextEdit DisplayFormatString="{0:N2}">
                                                            </PropertiesTextEdit>
                                                            <FilterCellStyle HorizontalAlign="Center"/>
                                                            <HeaderStyle Wrap="True" />
                                  </dx:GridViewDataTextColumn>
                                      <dx:GridViewDataTextColumn FieldName="MES12"  Name="MES12"  ReadOnly="true" Visible="true" VisibleIndex="16" Caption="Diciembre">
                                     <PropertiesTextEdit DisplayFormatString="{0:N2}">
                                                            </PropertiesTextEdit>
                                                            <FilterCellStyle HorizontalAlign="Center"/>
                                                            <HeaderStyle Wrap="True" />
                                  </dx:GridViewDataTextColumn>
                                     <dx:GridViewDataTextColumn FieldName="CUA3"  Name="CUA3"  ReadOnly="true" Visible="true" VisibleIndex="17"  Caption="3er cuatrimestre">
                                     <PropertiesTextEdit DisplayFormatString="{0:N2}">
                                                            </PropertiesTextEdit>
                                                            <FilterCellStyle HorizontalAlign="Center"/>
                                                            <HeaderStyle Wrap="True" />
                                  </dx:GridViewDataTextColumn>
                                 </Columns>    
                                     <SettingsDetail ShowDetailRow="true" />
                                      <Templates>
                                          <DetailRow>
                                      <dx:ASPxGridView ID="gvResultadosPOA"  KeyFieldName="LLAVE" SettingsBehavior-AllowFocusedRow ="true"  runat="server"  Width="100%" AutoGenerateColumns="False"    Theme="Office2010Blue"  OnBeforePerformDataSelect="gvResultadosPOA_BeforePerformDataSelect" SettingsDetail-AllowOnlyOneMasterRowExpanded="true"   OnDetailRowExpandedChanged="gvResultadosPOA_DetailRowExpandedChanged" >
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
             <SettingsCommandButton DeleteButton-ButtonType="Button" DeleteButton-Text="Quitar">
                 <DeleteButton ButtonType="Button">
                 </DeleteButton>
             </SettingsCommandButton>
             <SettingsDataSecurity AllowDelete="true" AllowEdit="true" AllowInsert="False" />
             <SettingsLoadingPanel Mode="ShowOnStatusBar" />
             <SettingsText CommandApplyFilter="Aplicar filtro" CommandApplySearchPanelFilter="asdasfasdfasdf" ContextMenuHideColumn="Ocultar Columna" ContextMenuShowCustomizationWindow="Mostrar Columnas" ContextMenuShowFilterRow="Fila de Filtros" ContextMenuShowFilterRowMenu="Seleccione Tipo de Filtro" ContextMenuShowSearchPanel="Buscar" CustomizationWindowCaption="Columnas Ocultas" GroupPanel="Arrastre las columnas que desee agrupar" />
                              <Columns>
                                      <dx:GridViewDataTextColumn FieldName="SPPRO$ID_PROGRAMA_PRESUPUESTO" VisibleIndex="0" Visible="false">
                                    </dx:GridViewDataTextColumn>
                                      <dx:GridViewDataTextColumn FieldName="ID_RESULTADO"  Name="ID_RESULTADO"  ReadOnly="true" Visible="false" VisibleIndex="1" Width="0%">
                                     <HeaderStyle Wrap="True" />
                                    </dx:GridViewDataTextColumn>
                                      <dx:GridViewDataTextColumn FieldName="RESULTADO"  Name="RESULTADO" Caption="VINCULACIÓN RESULTADO INSTITUCIONAL" ReadOnly="true" Visible="true" VisibleIndex="2" Width="50%">
                                     <HeaderStyle Wrap="True" />
                                    </dx:GridViewDataTextColumn>             
                                  <dx:GridViewDataTextColumn FieldName="EJE_ACCION"  Name="EJE_ACCCION" Caption="VINCULACIÓN PGG"  ReadOnly="true" Visible="true" VisibleIndex="3" Width="50%">
                                     <HeaderStyle Wrap="True" />
                                    </dx:GridViewDataTextColumn>   
                                    <dx:GridViewDataTextColumn FieldName="RED"  Name="RED" Caption="VINCULACIÓN CON RESULTADO ESTRATÉGICO (RE)"  ReadOnly="true" Visible="true" VisibleIndex="4" Width="100%">
                                     <HeaderStyle Wrap="True" />
                                    </dx:GridViewDataTextColumn>    
                                  <dx:GridViewDataTextColumn FieldName="TIPO"  Name="TIPO"  ReadOnly="true" Visible="false" VisibleIndex="5" Width="50%">
                                     <HeaderStyle Wrap="True" />
                                    </dx:GridViewDataTextColumn>    
                                  <dx:GridViewDataTextColumn FieldName="SPPRO$RESULTADO2"  Name="SPPRO$RESULTADO2"  ReadOnly="true" Visible="false" VisibleIndex="6" Width="50%">
                                     <HeaderStyle Wrap="True" />
                                    </dx:GridViewDataTextColumn> 
                                   <dx:GridViewDataTextColumn FieldName="LLAVE"  Name="LLAVE"  ReadOnly="true" Visible="false" VisibleIndex="7" Width="50%">
                                        <HeaderStyle Wrap="True" />
                                    </dx:GridViewDataTextColumn>  
                                  </Columns>
                                         <SettingsDetail ShowDetailRow="true" />
                                          <Templates>
                                              <DetailRow>
                                                  <div class="row productHeader">
                                  <div style="justify-content:flex-start">
                              <h4> Ingreso de metas de Productos, presione el botón "Metas" (no puede programar metas financieras para productos)</h4>
                                    </div>
                              
                                    </div>
                                              <dx:ASPxGridView ID="gvPOAproductos"  runat="server" KeyFieldName="SPPRO$ID_PRODUCTO" SettingsBehavior-AllowFocusedRow ="true"   Theme="Office2010Blue" AutoGenerateColumns="False" OnBeforePerformDataSelect="gvPOAproductos_BeforePerformDataSelect" Width="100%" OnRowUpdating="gvPOAproductos_RowUpdating"  OnCommandButtonInitialize="gvPOAproductos_CommandButtonInitialize"  OnCellEditorInitialize="gvPOAproductos_CellEditorInitialize"  SettingsDetail-AllowOnlyOneMasterRowExpanded="true"   OnDetailRowExpandedChanged="gvPOAproductos_DetailRowExpandedChanged"  >
                                                   <Settings VerticalScrollableHeight="400" VerticalScrollBarMode="Visible" HorizontalScrollBarMode="Visible" />
                                                  <SettingsCommandButton DeleteButton-Text="Quitar" DeleteButton-ButtonType="Button">
                                        <UpdateButton ButtonType="Button" Text="Guardar">
                                            <Styles>
                                                <Style CssClass="btn-warging">
                                                </Style>
                                            </Styles>
                                        </UpdateButton>
                                        <CancelButton ButtonType="Button" Text="cancelar">
                                            <Styles>
                                                <FocusRectStyle CssClass="btn-danger">
                                                </FocusRectStyle>
                                            </Styles>
                                        </CancelButton>
                                        <EditButton ButtonType="Button" Text="Metas">
                                        </EditButton>
                                        <DeleteButton ButtonType="Button">
                                           
                                        </DeleteButton>
                                    </SettingsCommandButton>
                                <SettingsEditing   Mode="Inline" />
                                <ClientSideEvents EndCallback="function(s,e)
             {
                if(s.cpError)
                {
                    //alert(s.cpError);
                    alertify.success(s.cpError);
                    delete s.cpError;
                }     
                   else if(s.cp1)
                {
                    //alert(s.cpError);
                    alertify.error(s.cp1);
                    delete s.cpError;
                }                    

                else if(s.cp3)
                {
                    //alert(s.cpError);
                    alertify.alert('Metas fisicas', s.cp3);
                    delete s.cpError;
                }   
             
             }           
             " />
                                                  <Columns>

                                                       <dx:GridViewDataTextColumn FieldName="ID_RESULTADO" VisibleIndex="0" Visible="false">
                                    </dx:GridViewDataTextColumn>
                                                      <dx:GridViewDataTextColumn FieldName="SPPRO$ID_PROGRAMA_PRESUPUESTO" VisibleIndex="1" Visible="false">
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn FieldName="SPPRO$ID_PRODUCTO" VisibleIndex="2" Visible="false">
                                    </dx:GridViewDataTextColumn>
                                   
                                    <dx:GridViewDataTextColumn FieldName="PRODUCTO" VisibleIndex="3" ReadOnly="true" Visible="true" >
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn FieldName="MEDIDA_PRODUCTO" VisibleIndex="4" Caption="Medida" ReadOnly="true"   Visible="true">
                                    </dx:GridViewDataTextColumn>
                                                     <dx:GridViewBandColumn Caption="Metas anuales" VisibleIndex="5">
                                        <Columns>                                         
                                                                                   
                                            <dx:GridViewDataTextColumn FieldName="MFPRODANUAL"   ShowInCustomizationForm="True" Caption="FISICA" VisibleIndex="1" ReadOnly="true">
                                                <PropertiesTextEdit DisplayFormatString="{0:N2}">
                                                            </PropertiesTextEdit>
                                                <CellStyle HorizontalAlign="Right"></CellStyle>
                                            </dx:GridViewDataTextColumn>   
                                            <dx:GridViewDataTextColumn FieldName="MFINPRODANUAL"   ShowInCustomizationForm="True" Caption="FINANCIERA" VisibleIndex="2" ReadOnly="true">
                                                <PropertiesTextEdit DisplayFormatString="{0:N2}">
                                                            </PropertiesTextEdit>
                                                <CellStyle HorizontalAlign="Right"></CellStyle>
                                            </dx:GridViewDataTextColumn>
                                            
                                        </Columns>
                                    </dx:GridViewBandColumn>


                                    <dx:GridViewCommandColumn ShowEditButton="True" Caption="Metas POA" VisibleIndex="6" Width="180" >
                                        <HeaderStyle Wrap="True" />
                                    </dx:GridViewCommandColumn>

                                                      <dx:GridViewBandColumn Caption="Enero" VisibleIndex="7">
                                        <Columns>                                         
                                                             <dx:GridViewDataTextColumn FieldName="IDMETMES1" Name="IDMETMES1"  ReadOnly="true"  Visible="false">
                                                                 </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataSpinEditColumn FieldName="MFISMES1"  Caption="FISICA" VisibleIndex="1" ReadOnly="false" Visible="true">
                                                <PropertiesSpinEdit DisplayFormatString="N2" />
                                            </dx:GridViewDataSpinEditColumn> 
                                            <dx:GridViewDataTextColumn FieldName="MFINMES1"   ShowInCustomizationForm="True" Caption="FINANCIERA" VisibleIndex="2" ReadOnly="true">
                                                <PropertiesTextEdit DisplayFormatString="{0:N2}">
                                                            </PropertiesTextEdit>
                                                <CellStyle HorizontalAlign="Right"></CellStyle>
                                            </dx:GridViewDataTextColumn>
                                            
                                        </Columns>
                                    </dx:GridViewBandColumn>

                                                      <dx:GridViewBandColumn Caption="Febrero" VisibleIndex="8">
                                        <Columns>                                         
                                                             <dx:GridViewDataTextColumn FieldName="IDMETMES2" Name="IDMETMES2"  ReadOnly="true"  Visible="false">
                                                                 </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataSpinEditColumn FieldName="MFISMES2"  Caption="FISICA" VisibleIndex="1" ReadOnly="false" Visible="true">
                                                <PropertiesSpinEdit DisplayFormatString="N2" />
                                            </dx:GridViewDataSpinEditColumn> 
                                            <dx:GridViewDataTextColumn FieldName="MFINMES2"   ShowInCustomizationForm="True" Caption="FINANCIERA" VisibleIndex="2" ReadOnly="true">
                                                <PropertiesTextEdit DisplayFormatString="{0:N2}">
                                                            </PropertiesTextEdit>
                                                <CellStyle HorizontalAlign="Right"></CellStyle>
                                            </dx:GridViewDataTextColumn>
                                            
                                        </Columns>
                                    </dx:GridViewBandColumn>

                                                      <dx:GridViewBandColumn Caption="Marzo" VisibleIndex="9">
                                        <Columns>                                         
                                                             <dx:GridViewDataTextColumn FieldName="IDMETMES3" Name="IDMETMES3"  ReadOnly="true"  Visible="false">
                                                                 </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataSpinEditColumn FieldName="MFISMES3"  Caption="FISICA" VisibleIndex="1" ReadOnly="false" Visible="true">
                                                <PropertiesSpinEdit DisplayFormatString="N2" />
                                            </dx:GridViewDataSpinEditColumn> 
                                            <dx:GridViewDataTextColumn FieldName="MFINMES3"   ShowInCustomizationForm="True" Caption="FINANCIERA" VisibleIndex="2" ReadOnly="true">
                                                <PropertiesTextEdit DisplayFormatString="{0:N2}">
                                                            </PropertiesTextEdit>
                                                <CellStyle HorizontalAlign="Right"></CellStyle>
                                            </dx:GridViewDataTextColumn>
                                            
                                        </Columns>
                                    </dx:GridViewBandColumn>

                                                      <dx:GridViewBandColumn Caption="Abril" VisibleIndex="10">
                                        <Columns>                                         
                                                             <dx:GridViewDataTextColumn FieldName="IDMETMES4" Name="IDMETMES4"  ReadOnly="true"  Visible="false">
                                                                 </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataSpinEditColumn FieldName="MFISMES4"  Caption="FISICA" VisibleIndex="1" ReadOnly="false" Visible="true">
                                                <PropertiesSpinEdit DisplayFormatString="N2" />
                                            </dx:GridViewDataSpinEditColumn> 
                                            <dx:GridViewDataTextColumn FieldName="MFINMES4"  ShowInCustomizationForm="True" Caption="FINANCIERA" VisibleIndex="2" ReadOnly="true">
                                                <PropertiesTextEdit DisplayFormatString="{0:N2}">
                                                            </PropertiesTextEdit>
                                                <CellStyle HorizontalAlign="Right"></CellStyle>
                                            </dx:GridViewDataTextColumn>
                                            
                                        </Columns>
                                    </dx:GridViewBandColumn>

                                                      <dx:GridViewBandColumn Caption="1er Cuatrimestre" VisibleIndex="11">
                                                          <Columns>                                        
                                                                                                        
                                            <dx:GridViewDataTextColumn FieldName="CUAFIS1"  ShowInCustomizationForm="True" Caption="FISICO" VisibleIndex="1" ReadOnly="true">
                                                <PropertiesTextEdit DisplayFormatString="{0:N2}">
                                                            </PropertiesTextEdit>
                                                <CellStyle HorizontalAlign="Right"></CellStyle>
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn FieldName="CUAFINPROD1"   ShowInCustomizationForm="True" Caption="FINANCIERA" VisibleIndex="2" ReadOnly="true">
                                                <PropertiesTextEdit DisplayFormatString="{0:N2}">
                                                            </PropertiesTextEdit>
                                                <CellStyle HorizontalAlign="Right"></CellStyle>
                                            </dx:GridViewDataTextColumn>
                                            </Columns>
                                    </dx:GridViewBandColumn>
                                            <dx:GridViewBandColumn Caption="Mayo" VisibleIndex="12">
                                        <Columns>                                         
                                                             <dx:GridViewDataTextColumn FieldName="IDMETMES5" Name="IDMETMES5"  ReadOnly="true"  Visible="false">
                                                                 </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataSpinEditColumn FieldName="MFISMES5"  Caption="FISICA" VisibleIndex="1" ReadOnly="false" Visible="true">
                                                <PropertiesSpinEdit DisplayFormatString="N2" />
                                            </dx:GridViewDataSpinEditColumn> 
                                            <dx:GridViewDataTextColumn FieldName="MFINMES5" ShowInCustomizationForm="True" Caption="FINANCIERA" VisibleIndex="2" ReadOnly="true">
                                                <PropertiesTextEdit DisplayFormatString="{0:N2}">
                                                            </PropertiesTextEdit>
                                                <CellStyle HorizontalAlign="Right"></CellStyle>
                                            </dx:GridViewDataTextColumn>
                                            
                                        </Columns>
                                    </dx:GridViewBandColumn>
                                            <dx:GridViewBandColumn Caption="Junio" VisibleIndex="13">
                                        <Columns>                                         
                                                             <dx:GridViewDataTextColumn FieldName="IDMETMES6" Name="IDMETMES6"  ReadOnly="true"  Visible="false">
                                                                 </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataSpinEditColumn FieldName="MFISMES6" Caption="FISICA" VisibleIndex="1" ReadOnly="false" Visible="true">
                                                <PropertiesSpinEdit DisplayFormatString="N2" />
                                            </dx:GridViewDataSpinEditColumn> 
                                            <dx:GridViewDataTextColumn FieldName="MFINMES6"  ShowInCustomizationForm="True" Caption="FINANCIERA" VisibleIndex="2" ReadOnly="true">
                                                <PropertiesTextEdit DisplayFormatString="{0:N2}">
                                                            </PropertiesTextEdit>
                                                <CellStyle HorizontalAlign="Right"></CellStyle>
                                            </dx:GridViewDataTextColumn>
                                            
                                        </Columns>
                                    </dx:GridViewBandColumn>

                                             <dx:GridViewBandColumn Caption="Julio" VisibleIndex="14">
                                        <Columns>                                         
                                                             <dx:GridViewDataTextColumn FieldName="IDMETMES7" Name="IDMETMES7"  ReadOnly="true"  Visible="false">
                                                                 </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataSpinEditColumn FieldName="MFISMES7"  Caption="FISICA" VisibleIndex="1" ReadOnly="false" Visible="true">
                                                <PropertiesSpinEdit DisplayFormatString="N2" />
                                            </dx:GridViewDataSpinEditColumn> 
                                            <dx:GridViewDataTextColumn FieldName="MFINMES7"   ShowInCustomizationForm="True" Caption="FINANCIERA" VisibleIndex="2" ReadOnly="true">
                                                <PropertiesTextEdit DisplayFormatString="{0:N2}">
                                                            </PropertiesTextEdit>
                                                <CellStyle HorizontalAlign="Right"></CellStyle>
                                            </dx:GridViewDataTextColumn>
                                            
                                        </Columns>
                                    </dx:GridViewBandColumn>

                                            <dx:GridViewBandColumn Caption="Agosto" VisibleIndex="15">
                                        <Columns>                                         
                                                             <dx:GridViewDataTextColumn FieldName="IDMETMES8" Name="IDMETMES8"  ReadOnly="true"  Visible="false">
                                                                 </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataSpinEditColumn FieldName="MFISMES8"  Caption="FISICA" VisibleIndex="1" ReadOnly="false" Visible="true">
                                                <PropertiesSpinEdit DisplayFormatString="N2" />
                                            </dx:GridViewDataSpinEditColumn> 
                                            <dx:GridViewDataTextColumn FieldName="MFINMES8"   ShowInCustomizationForm="True" Caption="FINANCIERA" VisibleIndex="2" ReadOnly="true">
                                                <PropertiesTextEdit DisplayFormatString="{0:N2}">
                                                            </PropertiesTextEdit>
                                                <CellStyle HorizontalAlign="Right"></CellStyle>
                                            </dx:GridViewDataTextColumn>
                                            
                                        </Columns>
                                    </dx:GridViewBandColumn>

                                                      <dx:GridViewBandColumn Caption="2do Cuatrimestre" VisibleIndex="16">
                                                          <Columns>                                        
                                                                                                        
                                            <dx:GridViewDataTextColumn FieldName="CUAFIS2"   ShowInCustomizationForm="True" Caption="FISICO" VisibleIndex="1" ReadOnly="true">
                                                <PropertiesTextEdit DisplayFormatString="{0:N2}">
                                                            </PropertiesTextEdit>
                                                <CellStyle HorizontalAlign="Right"></CellStyle>
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn FieldName="CUAFINPROD2"  ShowInCustomizationForm="True" Caption="FINANCIERA" VisibleIndex="2" ReadOnly="true">
                                                <PropertiesTextEdit DisplayFormatString="{0:N2}">
                                                            </PropertiesTextEdit>
                                                <CellStyle HorizontalAlign="Right"></CellStyle>
                                            </dx:GridViewDataTextColumn>
                                            </Columns>
                                    </dx:GridViewBandColumn>
                                    <dx:GridViewBandColumn Caption="Septiembre" VisibleIndex="17">
                                        <Columns>                                         
                                                             <dx:GridViewDataTextColumn FieldName="IDMETMES9" Name="IDMETMES9"  ReadOnly="true"  Visible="false">
                                                                 </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataSpinEditColumn FieldName="MFISMES9" Caption="FISICA" VisibleIndex="1" ReadOnly="false" Visible="true">
                                                <PropertiesSpinEdit DisplayFormatString="N2" />
                                            </dx:GridViewDataSpinEditColumn> 
                                            <dx:GridViewDataTextColumn FieldName="MFINMES9"  ShowInCustomizationForm="True" Caption="FINANCIERA" VisibleIndex="2" ReadOnly="true">
                                                <PropertiesTextEdit DisplayFormatString="{0:N2}">
                                                            </PropertiesTextEdit>
                                                <CellStyle HorizontalAlign="Right"></CellStyle>
                                            </dx:GridViewDataTextColumn>
                                            
                                        </Columns>
                                    </dx:GridViewBandColumn>
                                                       <dx:GridViewBandColumn Caption="Octubre" VisibleIndex="18">
                                        <Columns>                                         
                                                             <dx:GridViewDataTextColumn FieldName="IDMETMES10" Name="IDMETMES10"  ReadOnly="true"  Visible="false">
                                                                 </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataSpinEditColumn FieldName="MFISMES10" Caption="FISICA" VisibleIndex="1" ReadOnly="false" Visible="true">
                                                <PropertiesSpinEdit DisplayFormatString="N2" />
                                            </dx:GridViewDataSpinEditColumn> 
                                            <dx:GridViewDataTextColumn FieldName="MFINMES10"  ShowInCustomizationForm="True" Caption="FINANCIERA" VisibleIndex="2" ReadOnly="true">
                                                <PropertiesTextEdit DisplayFormatString="{0:N2}">
                                                            </PropertiesTextEdit>
                                                <CellStyle HorizontalAlign="Right"></CellStyle>
                                            </dx:GridViewDataTextColumn>
                                            
                                        </Columns>
                                    </dx:GridViewBandColumn>
                                                      <dx:GridViewBandColumn Caption="Noviembre" VisibleIndex="19">
                                        <Columns>                                         
                                                             <dx:GridViewDataTextColumn FieldName="IDMETMES11" Name="IDMETMES11"  ReadOnly="true"  Visible="false">
                                                                 </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataSpinEditColumn FieldName="MFISMES11"  Caption="FISICA" VisibleIndex="1" ReadOnly="false" Visible="true">
                                                <PropertiesSpinEdit DisplayFormatString="N2" />
                                            </dx:GridViewDataSpinEditColumn> 
                                            <dx:GridViewDataTextColumn FieldName="MFINMES11"  ShowInCustomizationForm="True" Caption="FINANCIERA" VisibleIndex="2" ReadOnly="true">
                                                <PropertiesTextEdit DisplayFormatString="{0:N2}">
                                                            </PropertiesTextEdit>
                                                <CellStyle HorizontalAlign="Right"></CellStyle>
                                            </dx:GridViewDataTextColumn>
                                            
                                        </Columns>
                                    </dx:GridViewBandColumn>
                                                      <dx:GridViewBandColumn Caption="Diciembre" VisibleIndex="20">
                                        <Columns>                                         
                                                             <dx:GridViewDataTextColumn FieldName="IDMETMES12" Name="IDMETMES12"  ReadOnly="true"  Visible="false">
                                                                 </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataSpinEditColumn FieldName="MFISMES12" Caption="FISICA" VisibleIndex="1" ReadOnly="false" Visible="true">
                                                <PropertiesSpinEdit DisplayFormatString="N2" />
                                            </dx:GridViewDataSpinEditColumn> 
                                            <dx:GridViewDataTextColumn FieldName="MFINMES12"   ShowInCustomizationForm="True" Caption="FINANCIERA" VisibleIndex="2" ReadOnly="true">
                                                <PropertiesTextEdit DisplayFormatString="{0:N2}">
                                                            </PropertiesTextEdit>
                                                <CellStyle HorizontalAlign="Right"></CellStyle>
                                            </dx:GridViewDataTextColumn>
                                            
                                        </Columns>
                                    </dx:GridViewBandColumn>
                                                      <dx:GridViewBandColumn Caption="3er Cuatrimestre" VisibleIndex="21">
                                                          <Columns>                                        
                                                                                                        
                                            <dx:GridViewDataTextColumn FieldName="CUAFIS3"   ShowInCustomizationForm="True" Caption="FISICO" VisibleIndex="1" ReadOnly="true">
                                                <PropertiesTextEdit DisplayFormatString="{0:N2}">
                                                            </PropertiesTextEdit>
                                                <CellStyle HorizontalAlign="Right"></CellStyle>
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn FieldName="CUAFINPROD3"   ShowInCustomizationForm="True" Caption="FINANCIERA" VisibleIndex="2" ReadOnly="true">
                                                <PropertiesTextEdit DisplayFormatString="{0:N2}">
                                                            </PropertiesTextEdit>
                                                <CellStyle HorizontalAlign="Right"></CellStyle>
                                            </dx:GridViewDataTextColumn>
                                            </Columns>
                                    </dx:GridViewBandColumn>
                                                      <dx:GridViewDataTextColumn FieldName="ESTADO" NAME="ESTADO"  Caption="ESTADO" VisibleIndex="16" ReadOnly="true" Visible="false">
                                                
                                            </dx:GridViewDataTextColumn>
                                                  </Columns>
                                                   <SettingsDetail ShowDetailRow="true" />
                                      <Templates>
                                          <DetailRow>
                                              <div class="row productHeader">
                                  <div style="justify-content:flex-start">
                              <h4>&nbsp;&nbsp;&nbsp;Programación de metas de Subproductos, presione el botón "Metas" </h4>
                                    </div>
                              
                                    </div>
                                              <dx:ASPxGridView ID="gvPOASubproductos"  runat="server"  SettingsBehavior-AllowFocusedRow ="true"     Theme="Office2010Blue" AutoGenerateColumns="false" OnCommandButtonInitialize="gvPOASubproductos_CommandButtonInitialize"  OnBeforePerformDataSelect="gvPOASubproductos_BeforePerformDataSelect"  KeyFieldName="SPPSUB$ID_SUBPRODUCTO" OnRowUpdating="gvPOASubproductos_RowUpdating"   OnCellEditorInitialize="gvPOASubproductos_CellEditorInitialize" OnHtmlDataCellPrepared="gvPOAsubProductos_HtmlDataCellPrepared" Width="100%" SettingsBehavior-ColumnResizeMode="Control">
                                                  <Settings VerticalScrollableHeight="300" VerticalScrollBarMode="Visible" HorizontalScrollBarMode="Auto"  />
                                                  <SettingsCommandButton DeleteButton-Text="Quitar" DeleteButton-ButtonType="Button">
                                        <UpdateButton ButtonType="Button" Text="Guardar">
                                            <Styles>
                                                <Style CssClass="btn-warging">
                                                </Style>
                                            </Styles>
                                        </UpdateButton>
                                        <CancelButton ButtonType="Button" Text="cancelar">
                                            <Styles>
                                                <FocusRectStyle CssClass="btn-danger">
                                                </FocusRectStyle>
                                            </Styles>
                                        </CancelButton>
                                        <EditButton ButtonType="Button" Text="Metas">
                                        </EditButton>
                                        <DeleteButton ButtonType="Button">
                                           
                                        </DeleteButton>
                                    </SettingsCommandButton>
                                <SettingsEditing Mode="Inline" />
                                <ClientSideEvents EndCallback="function(s,e)
             {
                if(s.cpError)
                {
                    //alert(s.cpError);
                    alertify.success(s.cpError);
                    delete s.cpError;
                }
                                    
                else if(s.cp1)
                {
                    //alert(s.cpError);
                    alertify.alert('Metas financieras', s.cp1);
                    delete s.cpError;
                }    
                                    
else if(s.cp3)
                {
                    //alert(s.cpError);
                    alertify.alert('Metas fisicas', s.cp3);
                    delete s.cpError;
                }     

                else if(s.cp2)
                {
                    //alert(s.cpError);
                    alertify.error( s.cp2);
                    delete s.cpError;
                }      
             }           
             " />
           <Columns>
                 <dx:GridViewDataTextColumn FieldName="SPPRO$ID_PRODUCTO" VisibleIndex="0" Visible="false">
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn FieldName="SPPSUB$ID_SUBPRODUCTO" VisibleIndex="1" Visible="false">
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn FieldName="SUBPRODUCTO"  Caption="Subproducto" VisibleIndex="2" ReadOnly="true" Visible="true" >
                                    </dx:GridViewDataTextColumn>


                <dx:GridViewDataTextColumn Caption="Metas <br/>por territorio" VisibleIndex="3" Width="70">
    <DataItemTemplate>
        <asp:Panel ID="pnlBtn1" runat="server">
          <dx:ASPxButton ID="btnMetasMuno1" runat="server"
    Text="Territorio"
    AutoPostBack="true"
    UseSubmitBehavior="true"
    CausesValidation="false"
    CommandArgument='<%# Eval("SPPSUB$ID_SUBPRODUCTO") %>'
    OnClick="btnMetasMuno1_Click" />
        </asp:Panel>
    </DataItemTemplate>
</dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn FieldName="MUNICIPIOS"  
                                       Caption="Número de municipios<br/>priorizados" 
                                        VisibleIndex="4" ReadOnly="true" Visible="true" Width="90">
                                         <HeaderStyle HorizontalAlign="Center" />
                                    </dx:GridViewDataTextColumn>

                                    <dx:GridViewDataTextColumn FieldName="MEDIDA_SUBPRODUCTO" VisibleIndex="5" Caption="Medida" ReadOnly="true"  Visible="true">
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn FieldName="SPPSUB$SNIP" VisibleIndex="6" Caption="SNIP" ReadOnly="true"  Visible="true">
                                    </dx:GridViewDataTextColumn>
               <dx:GridViewBandColumn Caption="Metas anuales" VisibleIndex="7">
                                        <Columns>                                         
                                                                                   
                                            <dx:GridViewDataTextColumn FieldName="ANUALFISICO"   ShowInCustomizationForm="True" Caption="FISICA" VisibleIndex="1" ReadOnly="true">
                                                <PropertiesTextEdit DisplayFormatString="{0:N2}">
                                                            </PropertiesTextEdit>
                                                <CellStyle HorizontalAlign="Right"></CellStyle>
                                            </dx:GridViewDataTextColumn>   
                                            <dx:GridViewDataTextColumn FieldName="ANUALFINANCIERO"   ShowInCustomizationForm="True" Caption="FINANCIERA" VisibleIndex="2" ReadOnly="true">
                                                <PropertiesTextEdit DisplayFormatString="{0:N2}">
                                                            </PropertiesTextEdit>
                                                <CellStyle HorizontalAlign="Right"></CellStyle>
                                            </dx:GridViewDataTextColumn>
                                            
                                        </Columns>
                                    </dx:GridViewBandColumn>

                <dx:GridViewBandColumn Caption="Operaciones" VisibleIndex="8">
                    <Columns>
                         <dx:GridViewDataTextColumn Caption="Insumos" VisibleIndex="0" Width="90" Visible="true">
                            <DataItemTemplate>
                                <asp:Button ID="btnInsumos" CssClass="btn btn-warning btn-sm" runat="server" OnClick="btnInsumos_Click" Text="Insumos" />
                            </DataItemTemplate>
                        </dx:GridViewDataTextColumn>

                                     <dx:GridViewCommandColumn ShowEditButton="True" Caption="Metas" VisibleIndex="1" Width="150">
                                        <HeaderStyle Wrap="True" />
                                    </dx:GridViewCommandColumn>
                        </Columns>
                    </dx:GridViewBandColumn>

                <dx:GridViewBandColumn Caption="Enero" VisibleIndex="9">
                                        <Columns>                                         
                                                             <dx:GridViewDataTextColumn FieldName="IDFISUB1" Name="IDFISUB1"  VisibleIndex="1" ReadOnly="true"  Visible="false">
                                                                 </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn FieldName="IDFINSUB1" Name="IDFINSUB1"  ReadOnly="true"  VisibleIndex="2" Visible="false">
                                                                 </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataSpinEditColumn FieldName="MFSUB1" Caption="FISICA" VisibleIndex="3" ReadOnly="false" Visible="true" Width="90">

                                                <PropertiesSpinEdit DisplayFormatString="N2" />
                                            </dx:GridViewDataSpinEditColumn> 
                                           <dx:GridViewDataSpinEditColumn FieldName="MFINSUB1" Caption="FINANCIERA" VisibleIndex="4" ReadOnly="false" Visible="true" Width="90">
                                                <PropertiesSpinEdit DisplayFormatString="N2" />
                                            </dx:GridViewDataSpinEditColumn>
                                            
                                        </Columns>
                                    </dx:GridViewBandColumn>

               <dx:GridViewBandColumn Caption="Febrero" VisibleIndex="10">
                                        <Columns>                                         
                                                             <dx:GridViewDataTextColumn FieldName="IDFISUB2" Name="IDFISUB2"  VisibleIndex="1" ReadOnly="true"  Visible="false">
                                                                 </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn FieldName="IDFINSUB2" Name="IDFINSUB2"  ReadOnly="true"  VisibleIndex="2" Visible="false">
                                                                 </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataSpinEditColumn FieldName="MFSUB2" Caption="FISICA" VisibleIndex="3" ReadOnly="false" Visible="true" Width="90">
                                                <PropertiesSpinEdit DisplayFormatString="N2" />
                                            </dx:GridViewDataSpinEditColumn> 
                                           <dx:GridViewDataSpinEditColumn FieldName="MFINSUB2" Caption="FINANCIERA" VisibleIndex="4" ReadOnly="false" Visible="true" Width="90">
                                                <PropertiesSpinEdit DisplayFormatString="N2" />
                                            </dx:GridViewDataSpinEditColumn>
                                            
                                        </Columns>
                                    </dx:GridViewBandColumn>

               <dx:GridViewBandColumn Caption="Marzo" VisibleIndex="11">
                                        <Columns>                                         
                                                             <dx:GridViewDataTextColumn FieldName="IDFISUB3" Name="IDFISUB3"  VisibleIndex="1" ReadOnly="true"  Visible="false">
                                                                 </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn FieldName="IDFINSUB3" Name="IDFINSUB3"  ReadOnly="true"  VisibleIndex="2" Visible="false">
                                                                 </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataSpinEditColumn FieldName="MFSUB3" Caption="FISICA" VisibleIndex="3" ReadOnly="false" Visible="true" Width="90">
                                                <PropertiesSpinEdit DisplayFormatString="N2" />
                                            </dx:GridViewDataSpinEditColumn> 
                                           <dx:GridViewDataSpinEditColumn FieldName="MFINSUB3" Caption="FINANCIERA" VisibleIndex="4" ReadOnly="false" Visible="true" Width="90">
                                                <PropertiesSpinEdit DisplayFormatString="N2" />
                                            </dx:GridViewDataSpinEditColumn>
                                            
                                        </Columns>
                                    </dx:GridViewBandColumn>

               <dx:GridViewBandColumn Caption="Abril" VisibleIndex="12">
                                        <Columns>                                         
                                                             <dx:GridViewDataTextColumn FieldName="IDFISUB4" Name="IDFISUB4"  VisibleIndex="1" ReadOnly="true"  Visible="false">
                                                                 </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn FieldName="IDFINSUB4" Name="IDFINSUB4"  ReadOnly="true"  VisibleIndex="2" Visible="false">
                                                                 </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataSpinEditColumn FieldName="MFSUB4" Caption="FISICA" VisibleIndex="3" ReadOnly="false" Visible="true" Width="90">
                                                <PropertiesSpinEdit DisplayFormatString="N2" />
                                            </dx:GridViewDataSpinEditColumn> 
                                           <dx:GridViewDataSpinEditColumn FieldName="MFINSUB4" Caption="FINANCIERA" VisibleIndex="4" ReadOnly="false" Visible="true" Width="90">
                                                <PropertiesSpinEdit DisplayFormatString="N2" />
                                            </dx:GridViewDataSpinEditColumn>
                                            
                                        </Columns>
                                    </dx:GridViewBandColumn>

               <dx:GridViewBandColumn Caption="1er Cuatrimestre" VisibleIndex="13">
                                        <Columns>                                         
                                                             <dx:GridViewDataTextColumn FieldName="MFCUA1" Name="MFCUA1" Caption="FISICA"  VisibleIndex="1" ReadOnly="true"  Visible="true" Width="90">
                                                                 </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn FieldName="MFINCUA1" Name="MFINCUA1"  ReadOnly="true" Caption="FINANCIERA"  VisibleIndex="2" Visible="true" Width="90">
                                                                 </dx:GridViewDataTextColumn>
                                           
                                            
                                        </Columns>
                                    </dx:GridViewBandColumn>


               <dx:GridViewBandColumn Caption="Mayo" VisibleIndex="14">
                                        <Columns>                                         
                                                             <dx:GridViewDataTextColumn FieldName="IDFISUB5" Name="IDFISUB5"  VisibleIndex="1" ReadOnly="true"  Visible="false">
                                                                 </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn FieldName="IDFINSUB5" Name="IDFINSUB5"  ReadOnly="true"  VisibleIndex="2" Visible="false">
                                                                 </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataSpinEditColumn FieldName="MFSUB5" Caption="FISICA" VisibleIndex="3" ReadOnly="false" Visible="true" Width="90">
                                                <PropertiesSpinEdit DisplayFormatString="N2" />
                                            </dx:GridViewDataSpinEditColumn> 
                                           <dx:GridViewDataSpinEditColumn FieldName="MFINSUB5" Caption="FINANCIERA" VisibleIndex="4" ReadOnly="false" Visible="true" Width="90">
                                                <PropertiesSpinEdit DisplayFormatString="N2" />
                                            </dx:GridViewDataSpinEditColumn>
                                            
                                        </Columns>
                                    </dx:GridViewBandColumn>

               <dx:GridViewBandColumn Caption="Junio" VisibleIndex="15">
                                        <Columns>                                         
                                                             <dx:GridViewDataTextColumn FieldName="IDFISUB6" Name="IDFISUB6"  VisibleIndex="1" ReadOnly="true"  Visible="false">
                                                                 </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn FieldName="IDFINSUB6" Name="IDFINSUB6"  ReadOnly="true"  VisibleIndex="2" Visible="false">
                                                                 </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataSpinEditColumn FieldName="MFSUB6" Caption="FISICA" VisibleIndex="3" ReadOnly="false" Visible="true" Width="90">
                                                <PropertiesSpinEdit DisplayFormatString="N2" />
                                            </dx:GridViewDataSpinEditColumn> 
                                           <dx:GridViewDataSpinEditColumn FieldName="MFINSUB6" Caption="FINANCIERA" VisibleIndex="4" ReadOnly="false" Visible="true" Width="90">
                                                <PropertiesSpinEdit DisplayFormatString="N2" />
                                            </dx:GridViewDataSpinEditColumn>
                                            
                                        </Columns>
                                    </dx:GridViewBandColumn>

               <dx:GridViewBandColumn Caption="Julio" VisibleIndex="16">
                                        <Columns>                                         
                                                             <dx:GridViewDataTextColumn FieldName="IDFISUB7" Name="IDFISUB7"  VisibleIndex="1" ReadOnly="true"  Visible="false">
                                                                 </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn FieldName="IDFINSUB7" Name="IDFINSUB7"  ReadOnly="true"  VisibleIndex="2" Visible="false">
                                                                 </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataSpinEditColumn FieldName="MFSUB7" Caption="FISICA" VisibleIndex="3" ReadOnly="false" Visible="true" Width="90">
                                                <PropertiesSpinEdit DisplayFormatString="N2" />
                                            </dx:GridViewDataSpinEditColumn> 
                                           <dx:GridViewDataSpinEditColumn FieldName="MFINSUB7" Caption="FINANCIERA" VisibleIndex="4" ReadOnly="false" Visible="true" Width="90">
                                                <PropertiesSpinEdit DisplayFormatString="N2" />
                                            </dx:GridViewDataSpinEditColumn>
                                            
                                        </Columns>
                                    </dx:GridViewBandColumn>
               <dx:GridViewBandColumn Caption="Agosto" VisibleIndex="17">
                                        <Columns>                                         
                                                             <dx:GridViewDataTextColumn FieldName="IDFISUB8" Name="IDFISUB8"  VisibleIndex="1" ReadOnly="true"  Visible="false">
                                                                 </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn FieldName="IDFINSUB8" Name="IDFINSUB8"  ReadOnly="true"  VisibleIndex="2" Visible="false">
                                                                 </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataSpinEditColumn FieldName="MFSUB8" Caption="FISICA" VisibleIndex="3" ReadOnly="false" Visible="true" Width="90">
                                                <PropertiesSpinEdit DisplayFormatString="N2" />
                                            </dx:GridViewDataSpinEditColumn> 
                                           <dx:GridViewDataSpinEditColumn FieldName="MFINSUB8" Caption="FINANCIERA" VisibleIndex="4" ReadOnly="false" Visible="true" Width="90">
                                                <PropertiesSpinEdit DisplayFormatString="N2" />
                                            </dx:GridViewDataSpinEditColumn>
                                            
                                        </Columns>
                                    </dx:GridViewBandColumn>
                <dx:GridViewBandColumn Caption="2do Cuatrimestre" VisibleIndex="18">
                                        <Columns>                                         
                                                             <dx:GridViewDataTextColumn FieldName="MFCUA2" Name="MFCUA2"  Caption="FISICA" VisibleIndex="1" ReadOnly="true"  Visible="true" Width="90">
                                                                 </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn FieldName="MFINCUA2" Name="MFINCUA2"  ReadOnly="true"  Caption="FINANCIERA" VisibleIndex="2" Visible="true" Width="90">
                                                                 </dx:GridViewDataTextColumn>
                                           
                                            
                                        </Columns>
                                    </dx:GridViewBandColumn>
               <dx:GridViewBandColumn Caption="Septiembre" VisibleIndex="19">
                                        <Columns>                                         
                                                             <dx:GridViewDataTextColumn FieldName="IDFISUB9" Name="IDFISUB9"  VisibleIndex="1" ReadOnly="true"  Visible="false">
                                                                 </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn FieldName="IDFINSUB9" Name="IDFINSUB9"  ReadOnly="true"  VisibleIndex="2" Visible="false">
                                                                 </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataSpinEditColumn FieldName="MFSUB9" Caption="FISICA" VisibleIndex="3" ReadOnly="false" Visible="true" Width="90">
                                                <PropertiesSpinEdit DisplayFormatString="N2" />
                                            </dx:GridViewDataSpinEditColumn> 
                                           <dx:GridViewDataSpinEditColumn FieldName="MFINSUB9" Caption="FINANCIERA" VisibleIndex="4" ReadOnly="false" Visible="true" Width="90">
                                                <PropertiesSpinEdit DisplayFormatString="N2" />
                                            </dx:GridViewDataSpinEditColumn>
                                            
                                        </Columns>
                                    </dx:GridViewBandColumn>

               <dx:GridViewBandColumn Caption="Octubre" VisibleIndex="20">
                                        <Columns>                                         
                                                             <dx:GridViewDataTextColumn FieldName="IDFISUB10" Name="IDFISUB10"  VisibleIndex="1" ReadOnly="true"  Visible="false">
                                                                 </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn FieldName="IDFINSUB10" Name="IDFINSUB10"  ReadOnly="true"  VisibleIndex="2" Visible="false">
                                                                 </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataSpinEditColumn FieldName="MFSUB10" Caption="FISICA" VisibleIndex="3" ReadOnly="false" Visible="true" Width="90">
                                                <PropertiesSpinEdit DisplayFormatString="N2" />
                                            </dx:GridViewDataSpinEditColumn> 
                                           <dx:GridViewDataSpinEditColumn FieldName="MFINSUB10" Caption="FINANCIERA" VisibleIndex="4" ReadOnly="false" Visible="true" Width="90">
                                                <PropertiesSpinEdit DisplayFormatString="N2" />
                                            </dx:GridViewDataSpinEditColumn>
                                            
                                        </Columns>
                                    </dx:GridViewBandColumn>

               <dx:GridViewBandColumn Caption="Noviembre" VisibleIndex="21">
                                        <Columns>                                         
                                                             <dx:GridViewDataTextColumn FieldName="IDFISUB11" Name="IDFISUB11"  VisibleIndex="1" ReadOnly="true"  Visible="false">
                                                                 </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn FieldName="IDFINSUB11" Name="IDFINSUB11"  ReadOnly="true"  VisibleIndex="2" Visible="false">
                                                                 </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataSpinEditColumn FieldName="MFSUB11" Caption="FISICA" VisibleIndex="3" ReadOnly="false" Visible="true" Width="90">
                                                <PropertiesSpinEdit DisplayFormatString="N2" />
                                            </dx:GridViewDataSpinEditColumn> 
                                           <dx:GridViewDataSpinEditColumn FieldName="MFINSUB11" Caption="FINANCIERA" VisibleIndex="4" ReadOnly="false" Visible="true" Width="90">
                                                <PropertiesSpinEdit DisplayFormatString="N2" />
                                            </dx:GridViewDataSpinEditColumn>
                                            
                                        </Columns>
                                    </dx:GridViewBandColumn>

               <dx:GridViewBandColumn Caption="Diciembre" VisibleIndex="22">
                                        <Columns>                                         
                                                             <dx:GridViewDataTextColumn FieldName="IDFISUB12" Name="IDFISUB12"  VisibleIndex="1" ReadOnly="true"  Visible="false">
                                                                 </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn FieldName="IDFINSUB12" Name="IDFINSUB12"  ReadOnly="true"  VisibleIndex="2" Visible="false">
                                                                 </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataSpinEditColumn FieldName="MFSUB12" Caption="FISICA" VisibleIndex="3" ReadOnly="false" Visible="true" Width="90">
                                                <PropertiesSpinEdit DisplayFormatString="N2" />
                                            </dx:GridViewDataSpinEditColumn> 
                                           <dx:GridViewDataSpinEditColumn FieldName="MFINSUB12" Caption="FINANCIERA" VisibleIndex="4" ReadOnly="false" Visible="true" Width="90">
                                                <PropertiesSpinEdit DisplayFormatString="N2" />
                                            </dx:GridViewDataSpinEditColumn>
                                            
                                        </Columns>
                                    </dx:GridViewBandColumn>
               
                <dx:GridViewBandColumn Caption="3er Cuatrimestre" VisibleIndex="23">
                                        <Columns>                                         
                                                             <dx:GridViewDataTextColumn FieldName="MFCUA3" Name="MFCUA3"  Caption="FISICA"  VisibleIndex="1" ReadOnly="true"  Visible="true" Width="90" >
                                                                 </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn FieldName="MFINCUA3" Name="MFINCUA3"  ReadOnly="true" Caption="FINANCIERA"  VisibleIndex="2" Visible="true" Width="90">
                                                                 </dx:GridViewDataTextColumn>
                                           
                                            
                                        </Columns>
                                    </dx:GridViewBandColumn>
                <dx:GridViewDataTextColumn FieldName="ESTADOFISICO" Name="ESTADOFISICO"  ReadOnly="true"  Width="0"  VisibleIndex="22" Visible="false">
                                                                 </dx:GridViewDataTextColumn>
               <dx:GridViewDataTextColumn FieldName="ESTADOFINANCIERO" Name="ESTADOFINANCIERO"  ReadOnly="true"  Width="0"   VisibleIndex="23" Visible="false">
                                                                 </dx:GridViewDataTextColumn>
              

           </Columns>
                                                  </dx:ASPxGridView>
                                                  </DetailRow>
                                          </Templates>
                                                 <%-- fin de grid--%>
                                                  </dx:ASPxGridView>






                                              </DetailRow>
                                              
                                          </Templates>
                                          
                                      </dx:ASPxGridView>

                                          </DetailRow>
                                         </Templates>
                                 </dx:ASPxGridView>
                           <%--    </ContentTemplate>
                         </asp:UpdatePanel>--%>

                        <%-- </div>--%>
                         </asp:Panel>
            
            
            </asp:View>
              <%--Vista de registro de metas de municipios--%>

            <asp:View runat="server" ID="vstMetasMunicipio">
              <asp:Panel ID="panMetaMunos" runat="server">
                   <div  style ="overflow:auto; width:100%; height:90%; padding: 1px 10px 1px 10px;   ">
                   <h4 style="color:#2d572c">Programación de metas multianuales por municipio</h4>
                  
                       <div class="row">
    <div class="col-md-4">
        <asp:HiddenField ID="hfIDSubProducto" runat="server" />
        <asp:HiddenField ID="HfIDPom" runat="server" /> 

        <label><b>Año POM:</b></label>
        <dx:ASPxComboBox ID="cbComboAnioMuno" runat="server"
            Width="400"
            AutoPostBack="True"
            Theme="Office2010Blue"
            CssClass="form-control" 
            OnValueChanged="cbComboAnioMuno_ValueChanged" />
    </div>

    <div class="col-md-8">
        <div class="pull-right" style="margin-top:25px;">
               <asp:Button ID="BtnRegresaPantalla" runat="server"
       Text="Regresar"
       CssClass="btn btn-danger btn-sm"
       OnClick="BtnRegresaPantalla_Click" />

            <asp:Button ID="btnTodoExcel" runat="server"
                Text="Excel todos los municipios priorizados"
                CssClass="btn btn-primary btn-sm"
                OnClick="btnTodoExcel_Click" />

            <asp:Button ID="BtnUno" runat="server"
                Text="Excel de municipios de este subproducto"
                CssClass="btn btn-success btn-sm"
                OnClick="BtnUno_Click" />

         
        </div>
    </div>
</div>
<div class="well well-sm" style="margin-top:10px;">
    <div class="row">

        <div class="col-md-2"><b>Periodo</b><br /><asp:Label ID="periodoMuno" runat="server" /></div>
        <div class="col-md-3"><b>Subproducto</b><br /><asp:Label ID="SubproductoMuno" runat="server" /></div>
        <div class="col-md-2"><b>Medida</b><br /><asp:Label ID="MedidaMuno" runat="server" /></div>
        <div class="col-md-2"><b>Física</b><br /><asp:Label ID="FisicaMuno" runat="server" /></div>
        <div class="col-md-3"><b>Financiera</b><br /><asp:Label ID="FinanceraMuno" runat="server" /></div>

    </div>
</div>


                       <div class="row">
    <div class="col-md-6">
        <div class="form-inline">
            <span class="text-muted">
     Suba el archivo Excel para cargar metas automáticamente
 </span>

            <asp:FileUpload ID="fuExcel" runat="server" class="form-control" />

            <asp:Button ID="btnSubir" runat="server"
                Text="Subir"
                CssClass="btn btn-primary btn-sm"
                OnClick="btnSubir_Click" />
        </div>
    </div>

   
</div>
                       
                  
                 
                       <div style="margin-top:10px;">

                       <dx:ASPxGridView ID="gvMunosMetas" runat="server"  KeyFieldName="NUMERO;ID_MUNO_MUNICIPIO;ID_METAS_MUNICIPIO;REGISTRA_FISICA;REGISTRA_FINANCIERA;POA;ANIO;FISICA_MUNICIPIO;FINANCIERA_MUNICIPIO"  Theme="Office2010Blue" Width="100%"  SettingsBehavior-AllowFocusedRow ="true"   ClientInstanceName="gvMunosMetas" OnCellEditorInitialize="gvMunosMetas_CellEditorInitialize" OnBatchUpdate="gvMunosMetas_BatchUpdate">
                            <Settings ShowFilterRow="True"/>
                           <SettingsPager Mode="ShowAllRecords" />
                            <SettingsEditing Mode="Batch">      
                            <BatchEditSettings EditMode="Cell" />
                            </SettingsEditing>
                           <ClientSideEvents EndCallback="function(s,e)
{
   if(s.cpError)
   {
       //alert(s.cpError);
       alertify.success(s.cpError);
       s.batchEditApi.EndEdit(); // limpia estado batch
       s.PerformCallback();
       delete s.cpError;
   }             
}           
" />
                           <SettingsCommandButton>                                  
                                    <UpdateButton Text="Guardar metas" Styles-Style-CssClass="btn btn-primary btn-xs"></UpdateButton>
                                    <CancelButton Text="Cancelar" Styles-Style-CssClass="btn btn-primary btn-xs"></CancelButton>
                                </SettingsCommandButton>
                            <Columns>
                                          <dx:GridViewDataTextColumn FieldName="NUMERO"     
                                            VisibleIndex="0"
                                            ReadOnly="True" 
                                            Visible ="false"  />
                                <dx:GridViewDataTextColumn FieldName="POA"     
                                VisibleIndex="1"
                                ReadOnly="True" 
                                Visible ="false"/>

                               <dx:GridViewDataTextColumn FieldName="SPPO$ID_PERIODO"     
                                VisibleIndex="2"
                                ReadOnly="True" 
                                Visible ="false"/>

                                 <dx:GridViewDataTextColumn FieldName="PERIODO"     
                                VisibleIndex="3"
                                ReadOnly="True" 
                                Visible ="false"  />

                                <dx:GridViewDataTextColumn FieldName="ID_MUNO_MUNICIPIO"     
                                VisibleIndex="4"
                                ReadOnly="True" 
                                Visible ="false"  />

                                <dx:GridViewDataTextColumn FieldName="DEPTO"     
VisibleIndex="5"
ReadOnly="True" 
Visible ="false"  />
                                 <dx:GridViewDataTextColumn FieldName="SPSM$GEOGRAFICO"     
VisibleIndex="6"
ReadOnly="True" 
Visible ="false"  />

                                 <dx:GridViewDataTextColumn FieldName="SPPSUB$ID_SUBPRODUCTO"     
VisibleIndex="7"
ReadOnly="True" 
Visible ="false"  />
                                <dx:GridViewDataTextColumn FieldName="ID_METAS_MUNICIPIO"     
VisibleIndex="8"
ReadOnly="True" 
Visible ="false"  />
                          <dx:GridViewDataTextColumn FieldName="SUBPRODUCTO"     
VisibleIndex="9"
ReadOnly="True" 
Visible ="false"  />
             <dx:GridViewDataTextColumn FieldName="MEDIDA"     
VisibleIndex="10"
ReadOnly="True" 
Visible ="false"  />
             <dx:GridViewDataTextColumn FieldName="DEPARTAMENTO"     
VisibleIndex="11" Caption="Departamento"
ReadOnly="True" 
Visible ="True"  />   

                                             <dx:GridViewDataTextColumn FieldName="MUNICIPIO"     
VisibleIndex="12" Caption="Municipio"
ReadOnly="True" 
Visible ="True"  />   
        

<dx:GridViewDataSpinEditColumn
    FieldName="FISICA_MUNICIPIO"
    Caption="Total meta física"
    ReadOnly="True" 
    VisibleIndex="13">

    <PropertiesSpinEdit
        NumberType="Float"
        DisplayFormatString="n2"
        MinValue="0">
    </PropertiesSpinEdit>

</dx:GridViewDataSpinEditColumn>

                <dx:GridViewDataTextColumn FieldName="FINANCIERA_MUNICIPIO" 
             Caption="Total meta financiera"
VisibleIndex="14"
ReadOnly="True" 
Visible ="True"  />

                                <dx:GridViewDataSpinEditColumn
    FieldName="REGISTRA_FISICA"
    Caption="Ingresar meta física"
    VisibleIndex="15">

    <PropertiesSpinEdit
        NumberType="Float"
        DisplayFormatString="n2"
        MinValue="0">
    </PropertiesSpinEdit>

</dx:GridViewDataSpinEditColumn>

                                                                <dx:GridViewDataSpinEditColumn
    FieldName="REGISTRA_FINANCIERA"
    Caption="Ingresar meta financiera"
    VisibleIndex="16">

    <PropertiesSpinEdit
        NumberType="Float"
        DisplayFormatString="n2"
        MinValue="0">
    </PropertiesSpinEdit>

</dx:GridViewDataSpinEditColumn>

                                                                             <dx:GridViewDataTextColumn FieldName="RN"     
VisibleIndex="17" 
ReadOnly="True" 
Visible ="false"  />   

                            </columns>
                                <Settings ShowGroupPanel="true" ShowFooter="true" ShowGroupFooter="VisibleIfExpanded" />
                           <TotalSummary>
                               <dx:ASPxSummaryItem FieldName="FISICA_MUNICIPIO" SummaryType="Sum" />
                               <dx:ASPxSummaryItem FieldName="FINANCIERA_MUNICIPIO" SummaryType="Sum" />
                                <dx:ASPxSummaryItem FieldName="REGISTRA_FISICA" SummaryType="Sum" />
                               <dx:ASPxSummaryItem FieldName="REGISTRA_FINANCIERA" SummaryType="Sum" />
                           </TotalSummary>
                       </dx:ASPxGridView>
                   
             </div>

                        </div>
              </asp:Panel>
               
            </asp:View>



                      <!--vista de municipio-->
                      <asp:View runat="server" ID="ViewMunoPOA">
              <asp:Panel ID="Panel1" runat="server">
                   <div  style ="overflow:auto; width:100%; height:90%; padding: 1px 10px 1px 10px;   ">
                   <h4 style="color:#2d572c">Registros de POA de metas mensuales por municipio</h4>
                   
                       <div class="row">
    <div class="col-md-6">
        <h4 style="margin:0; color:#2d572c;">
            POA de metas por municipio
             <asp:HiddenField ID="hfIDSubProducto1" runat="server" />
        </h4>
    </div>

    <div class="col-md-6">
        <div class="pull-right">
            <asp:Button ID="BtnRegresaPantalla1" runat="server"
                Text="Regresar"
                CssClass="btn btn-danger btn-sm"
                OnClick="BtnRegresaPantalla1_Click"
                />

            <asp:Button ID="btnTodoExcel1" runat="server"
                Text="Excel todos los municipios priorizados"
                CssClass="btn btn-primary btn-sm"
                OnClick="btnTodoExcel1_Click"
                 />

            <asp:Button ID="BtnUno1" runat="server"
                Text="Excel de municipios de este subproducto"
                CssClass="btn btn-success btn-sm"
                OnClick="BtnUno1_Click"
                 />
        </div>
    </div>
</div>

                       <div class="well well-sm" style="margin-top:10px;">

    <div class="row">

        <div class="col-md-2">
            <b>Año</b><br />
            <asp:Label ID="lblAnioRegistroMuno" runat="server" />
        </div>

        <div class="col-md-3">
            <b>Subproducto</b><br />
            <asp:Label ID="SubproductoPOAMuno" runat="server" />
        </div>

        <div class="col-md-2">
            <b>Medida</b><br />
            <asp:Label ID="MedidaMunoPOA" runat="server" />
        </div>

        <div class="col-md-2">
            <b>Física</b><br />
            <asp:Label ID="FisicaMunoPOA" runat="server" />
        </div>

        <div class="col-md-3">
            <b>Financiera</b><br />
            <asp:Label ID="FinanceraMunoPOA" runat="server" />
        </div>

    </div>

</div>

               <div class="row">
    <div class="col-md-6">
        <div class="form-inline">
            <span class="text-muted">
    Carga masiva desde Excel
</span>
            <asp:FileUpload ID="fuExcel1" runat="server" class="form-control" />

            <asp:Button ID="btnSubir1" runat="server"
                Text="Subir"
                CssClass="btn btn-primary btn-sm"
                OnClick="btnSubir1_Click"
                 />
        </div>
    </div>

         

                  

                      <!-- <asp:UpdatePanel ID="UpdatePanel6" runat="server">
    <ContentTemplate>-->
                   <div style="margin-top:10px;">
                       <dx:ASPxGridView ID="gvPOAMUNO" runat="server"  KeyFieldName="ID_UNICO"  Theme="Office2010Blue" Width="100%"  SettingsBehavior-AllowFocusedRow ="true"   ClientInstanceName="gvPoaMunosMetas" OnBatchUpdate="gvPOAMUNO_BatchUpdate">
                            <Settings ShowFilterRow="True" ShowGroupPanel="True" />
                           <SettingsPager Mode="ShowAllRecords" />
                            <SettingsEditing Mode="Batch">      
                            <BatchEditSettings EditMode="Cell" />
                            </SettingsEditing>
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
                           <SettingsCommandButton>                                  
                                    <UpdateButton Text="Guardar metas" Styles-Style-CssClass="btn btn-primary btn-xs"></UpdateButton>
                                    <CancelButton Text="Cancelar" Styles-Style-CssClass="btn btn-primary btn-xs"></CancelButton>
                                </SettingsCommandButton>
                            <Columns>
                                          
              <dx:GridViewDataTextColumn FieldName="ID_UNICO" VisibleIndex="0" ReadOnly="True" Visible ="false"/>  
              <dx:GridViewDataTextColumn FieldName="POA" VisibleIndex="1" ReadOnly="True" Visible ="false"/>                    

             <dx:GridViewDataTextColumn FieldName="ANIO" Caption ="Año" VisibleIndex="2" ReadOnly="True" Visible ="True"/>

             <dx:GridViewDataTextColumn FieldName="SPPSUB$ID_SUBPRODUCTO"   VisibleIndex="3" ReadOnly="True" Visible ="false"/>

              <dx:GridViewDataTextColumn FieldName="ID_MUNOS"  VisibleIndex="4"  ReadOnly="True"  Visible ="false"  />

               <dx:GridViewDataTextColumn FieldName="ID_METAS" VisibleIndex="5" ReadOnly="True" Visible ="false"  />

               <dx:GridViewDataTextColumn FieldName="ID_EJECUCION" VisibleIndex="6" ReadOnly="True"  Visible ="false"  />

               <dx:GridViewDataTextColumn FieldName="SUBPRODUCTO" VisibleIndex="7" ReadOnly="True" Visible ="False"  />

              <dx:GridViewDataTextColumn FieldName="MEDIDA" VisibleIndex="8" ReadOnly="True" Visible ="False"  />                               
                                                   
              <dx:GridViewDataTextColumn FieldName="METAFISICA_ANUAL"  VisibleIndex="9" ReadOnly="True" Visible ="False"  />
                                 
              <dx:GridViewDataTextColumn FieldName="METAFINANCIERA_ANUAL" VisibleIndex="10" ReadOnly="True" Visible ="False"  />
             
             <dx:GridViewDataTextColumn FieldName="DEPARTAMENTO" VisibleIndex="11" Caption="Departamento" ReadOnly="True" Visible ="True"  />   

             <dx:GridViewDataTextColumn FieldName="MUNICIPIO"  VisibleIndex="12" Caption="Municipio" ReadOnly="True" Visible ="True"  />   

              <dx:GridViewBandColumn Caption="Metas anuales de municipio" VisibleIndex="13">
                <Columns>
                    <dx:GridViewDataTextColumn FieldName="METAFISCA_MUNO" ShowInCustomizationForm="True" Caption="Meta física" VisibleIndex="8" ReadOnly="true">
                    <PropertiesTextEdit DisplayFormatString="{0:N2}"></PropertiesTextEdit>
                    <CellStyle HorizontalAlign="Right"></CellStyle>
                    </dx:GridViewDataTextColumn>  

                    <dx:GridViewDataTextColumn FieldName="METAFINANCIERA_MUNO" ShowInCustomizationForm="True" Caption="Meta financiera" VisibleIndex="9" ReadOnly="true">
                    <PropertiesTextEdit DisplayFormatString="{0:N2}"></PropertiesTextEdit>
                    <CellStyle HorizontalAlign="Right"></CellStyle>
                    </dx:GridViewDataTextColumn>  
                </Columns>                                          
        </dx:GridViewBandColumn>

           <dx:GridViewBandColumn Caption="Total de programación de de metas" VisibleIndex="14">
                    <Columns>
                    <dx:GridViewDataTextColumn FieldName="ANUAL_FISICO" ShowInCustomizationForm="True" Caption="Totales meta mensual física" VisibleIndex="1" ReadOnly="true">
                    <PropertiesTextEdit DisplayFormatString="{0:N2}"></PropertiesTextEdit>
                    <CellStyle HorizontalAlign="Right"></CellStyle>
                </dx:GridViewDataTextColumn>  

                 

            <dx:GridViewDataTextColumn FieldName="ANUAL_FINANCIERO" ShowInCustomizationForm="True" Caption="Totales meta mensual financiera" VisibleIndex="3" ReadOnly="true">
    <PropertiesTextEdit DisplayFormatString="{0:N2}"></PropertiesTextEdit>
    <CellStyle HorizontalAlign="Right"></CellStyle>
</dx:GridViewDataTextColumn>  

                                      </Columns>
                                          
        </dx:GridViewBandColumn>

<dx:GridViewBandColumn Caption="ENERO" VisibleIndex="15">
    <Columns>
                                    
               <dx:GridViewDataSpinEditColumn FieldName="ENE_FIS"  Caption="FISICO" VisibleIndex="1" ReadOnly="false" Visible="true">
    <PropertiesSpinEdit DisplayFormatString="N2" />
</dx:GridViewDataSpinEditColumn>
              <dx:GridViewDataSpinEditColumn FieldName="ENE_FIN"  Caption="FINANCIERO" VisibleIndex="2" ReadOnly="false" Visible="true">
    <PropertiesSpinEdit DisplayFormatString="N2" />
</dx:GridViewDataSpinEditColumn>
                                </Columns>

        </dx:GridViewBandColumn>


                                <dx:GridViewBandColumn Caption="FEBRERO" VisibleIndex="16">
    <Columns>
                                    
                                    <dx:GridViewDataSpinEditColumn FieldName="FEB_FIS"  Caption="FISICO" VisibleIndex="1" ReadOnly="false" Visible="true">
    <PropertiesSpinEdit DisplayFormatString="N2" />
</dx:GridViewDataSpinEditColumn>
                                    <dx:GridViewDataSpinEditColumn FieldName="FEB_FIN"  Caption="FINANCIERO" VisibleIndex="2" ReadOnly="false" Visible="true">
    <PropertiesSpinEdit DisplayFormatString="N2" />
</dx:GridViewDataSpinEditColumn>
                                </Columns>

        </dx:GridViewBandColumn>

                                
                                <dx:GridViewBandColumn Caption="MARZO" VisibleIndex="17">
    <Columns>
                                    
                                    <dx:GridViewDataSpinEditColumn FieldName="MAR_FIS"  Caption="FISICO" VisibleIndex="1" ReadOnly="false" Visible="true">
    <PropertiesSpinEdit DisplayFormatString="N2" />
</dx:GridViewDataSpinEditColumn>
                                    <dx:GridViewDataSpinEditColumn FieldName="MAR_FIN"  Caption="FINANCIERO" VisibleIndex="2" ReadOnly="false" Visible="true">
    <PropertiesSpinEdit DisplayFormatString="N2" />
</dx:GridViewDataSpinEditColumn>
                                </Columns>

        </dx:GridViewBandColumn>

                  <dx:GridViewBandColumn Caption="ABRIL" VisibleIndex="18">
    <Columns>
                                    
                                    <dx:GridViewDataSpinEditColumn FieldName="ABR_FIS"  Caption="FISICO" VisibleIndex="1" ReadOnly="false" Visible="true">
    <PropertiesSpinEdit DisplayFormatString="N2" />
</dx:GridViewDataSpinEditColumn>
                                    <dx:GridViewDataSpinEditColumn FieldName="ABR_FIN"  Caption="FINANCIERO" VisibleIndex="2" ReadOnly="false" Visible="true">
    <PropertiesSpinEdit DisplayFormatString="N2" />
</dx:GridViewDataSpinEditColumn>
                                </Columns>

        </dx:GridViewBandColumn>

             <dx:GridViewBandColumn Caption="MAYO" VisibleIndex="19">
    <Columns>
                                    
                                    <dx:GridViewDataSpinEditColumn FieldName="MAY_FIS"  Caption="FISICO" VisibleIndex="1" ReadOnly="false" Visible="true">
    <PropertiesSpinEdit DisplayFormatString="N2" />
</dx:GridViewDataSpinEditColumn>
                                    <dx:GridViewDataSpinEditColumn FieldName="MAY_FIN"  Caption="FINANCIERO" VisibleIndex="2" ReadOnly="false" Visible="true">
    <PropertiesSpinEdit DisplayFormatString="N2" />
</dx:GridViewDataSpinEditColumn>
                                </Columns>

        </dx:GridViewBandColumn>

  <dx:GridViewBandColumn Caption="JUNIO" VisibleIndex="20">
    <Columns>
                                    
                                    <dx:GridViewDataSpinEditColumn FieldName="JUN_FIS"  Caption="FISICO" VisibleIndex="1" ReadOnly="false" Visible="true">
    <PropertiesSpinEdit DisplayFormatString="N2" />
</dx:GridViewDataSpinEditColumn>
                                    <dx:GridViewDataSpinEditColumn FieldName="JUN_FIN"  Caption="FINANCIERO" VisibleIndex="2" ReadOnly="false" Visible="true">
    <PropertiesSpinEdit DisplayFormatString="N2" />
</dx:GridViewDataSpinEditColumn>
                                </Columns>

        </dx:GridViewBandColumn>

      <dx:GridViewBandColumn Caption="JULIO" VisibleIndex="21">
    <Columns>
                                    
                                    <dx:GridViewDataSpinEditColumn FieldName="JUL_FIS"  Caption="FISICO" VisibleIndex="1" ReadOnly="false" Visible="true">
    <PropertiesSpinEdit DisplayFormatString="N2" />
</dx:GridViewDataSpinEditColumn>
                                    <dx:GridViewDataSpinEditColumn FieldName="JUL_FIN"  Caption="FINANCIERO" VisibleIndex="2" ReadOnly="false" Visible="true">
    <PropertiesSpinEdit DisplayFormatString="N2" />
</dx:GridViewDataSpinEditColumn>
                                </Columns>

        </dx:GridViewBandColumn>

              <dx:GridViewBandColumn Caption="AGOSTO" VisibleIndex="22">
    <Columns>
                                    
                                    <dx:GridViewDataSpinEditColumn FieldName="AGO_FIS"  Caption="FISICO" VisibleIndex="1" ReadOnly="false" Visible="true">
    <PropertiesSpinEdit DisplayFormatString="N2" />
</dx:GridViewDataSpinEditColumn>
                                    <dx:GridViewDataSpinEditColumn FieldName="AGO_FIN"  Caption="FINANCIERO" VisibleIndex="2" ReadOnly="false" Visible="true">
    <PropertiesSpinEdit DisplayFormatString="N2" />
</dx:GridViewDataSpinEditColumn>
                                </Columns>

        </dx:GridViewBandColumn>

                      <dx:GridViewBandColumn Caption="SEPTIEMBRE" VisibleIndex="23">
    <Columns>
                                    
                                    <dx:GridViewDataSpinEditColumn FieldName="SEP_FIS"  Caption="FISICO" VisibleIndex="1" ReadOnly="false" Visible="true">
    <PropertiesSpinEdit DisplayFormatString="N2" />
</dx:GridViewDataSpinEditColumn>
                                    <dx:GridViewDataSpinEditColumn FieldName="SEP_FIN"  Caption="FINANCIERO" VisibleIndex="2" ReadOnly="false" Visible="true">
    <PropertiesSpinEdit DisplayFormatString="N2" />
</dx:GridViewDataSpinEditColumn>
                                </Columns>

        </dx:GridViewBandColumn>

                      <dx:GridViewBandColumn Caption="OCTUBRE" VisibleIndex="24">
    <Columns>
                                    
                                    <dx:GridViewDataSpinEditColumn FieldName="OCT_FIS"  Caption="FISICO" VisibleIndex="1" ReadOnly="false" Visible="true">
    <PropertiesSpinEdit DisplayFormatString="N2" />
</dx:GridViewDataSpinEditColumn>
                                    <dx:GridViewDataSpinEditColumn FieldName="OCT_FIN"  Caption="FINANCIERO" VisibleIndex="2" ReadOnly="false" Visible="true">
    <PropertiesSpinEdit DisplayFormatString="N2" />
</dx:GridViewDataSpinEditColumn>
                                </Columns>

        </dx:GridViewBandColumn>

                              <dx:GridViewBandColumn Caption="NOVIEMBRE" VisibleIndex="25">
    <Columns>
                                    
                                    <dx:GridViewDataSpinEditColumn FieldName="NOV_FIS"  Caption="FISICO" VisibleIndex="1" ReadOnly="false" Visible="true">
    <PropertiesSpinEdit DisplayFormatString="N2" />
</dx:GridViewDataSpinEditColumn>
                                    <dx:GridViewDataSpinEditColumn FieldName="NOV_FIN"  Caption="FINANCIERO" VisibleIndex="2" ReadOnly="false" Visible="true">
    <PropertiesSpinEdit DisplayFormatString="N2" />
</dx:GridViewDataSpinEditColumn>
                                </Columns>

        </dx:GridViewBandColumn>

                                      <dx:GridViewBandColumn Caption="DICIEMBRE" VisibleIndex="26">
    <Columns>
                                    
                                    <dx:GridViewDataSpinEditColumn FieldName="DIC_FIS"  Caption="FISICO" VisibleIndex="1" ReadOnly="false" Visible="true">
    <PropertiesSpinEdit DisplayFormatString="N2" />
</dx:GridViewDataSpinEditColumn>
                                    <dx:GridViewDataSpinEditColumn FieldName="DIC_FIN"  Caption="FINANCIERO" VisibleIndex="2" ReadOnly="false" Visible="true">
    <PropertiesSpinEdit DisplayFormatString="N2" />
</dx:GridViewDataSpinEditColumn>
                                </Columns>

        </dx:GridViewBandColumn>


                                  <dx:GridViewBandColumn Caption="1ER CUAT" VisibleIndex="27">
    <Columns>
                                    
                                    <dx:GridViewDataSpinEditColumn FieldName="CUAFIS1"  Caption="FISICO" VisibleIndex="1" ReadOnly="true" Visible="true">
    <PropertiesSpinEdit DisplayFormatString="N2" />
</dx:GridViewDataSpinEditColumn>
                                    <dx:GridViewDataSpinEditColumn FieldName="CUAFIN1"  Caption="FINANCIERO" VisibleIndex="2" ReadOnly="true" Visible="true">
    <PropertiesSpinEdit DisplayFormatString="N2" />
</dx:GridViewDataSpinEditColumn>
                                </Columns>

        </dx:GridViewBandColumn>

                                          <dx:GridViewBandColumn Caption="2DO CUAT" VisibleIndex="28">
    <Columns>
                                    
                                    <dx:GridViewDataSpinEditColumn FieldName="CUAFIS2"  Caption="FISICO" VisibleIndex="1" ReadOnly="true" Visible="true">
    <PropertiesSpinEdit DisplayFormatString="N2" />
</dx:GridViewDataSpinEditColumn>
                                    <dx:GridViewDataSpinEditColumn FieldName="CUAFIN2"  Caption="FINANCIERO" VisibleIndex="2" ReadOnly="true" Visible="true">
    <PropertiesSpinEdit DisplayFormatString="N2" />
</dx:GridViewDataSpinEditColumn>
                                </Columns>

        </dx:GridViewBandColumn>

                                                          <dx:GridViewBandColumn Caption="3DO CUAT" VisibleIndex="29">
    <Columns>
                                    
                                    <dx:GridViewDataSpinEditColumn FieldName="CUAFIS3"  Caption="FISICO" VisibleIndex="1" ReadOnly="true" Visible="true">
    <PropertiesSpinEdit DisplayFormatString="N2" />
</dx:GridViewDataSpinEditColumn>
                                    <dx:GridViewDataSpinEditColumn FieldName="CUAFIN3"  Caption="FINANCIERO" VisibleIndex="2" ReadOnly="true" Visible="true">
    <PropertiesSpinEdit DisplayFormatString="N2" />
</dx:GridViewDataSpinEditColumn>
                                </Columns>

        </dx:GridViewBandColumn>
                                 

                            </columns>
                                <Settings ShowGroupPanel="true" ShowFooter="true" ShowGroupFooter="VisibleIfExpanded" />
                           
                       </dx:ASPxGridView>
                       </div>
                   
             <!--</ContentTemplate>
              </asp:UpdatePanel>-->

                        </div>
              </asp:Panel>
               
            </asp:View>


          <!--vista de municipio-->

        </asp:MultiView>
        
          <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                          <ContentTemplate>    
        <dx:ASPxPopupControl ID="popInsumo" runat="server" AllowDragging ="true" Width ="800px" Height="400px" HeaderText="Programación de insumos" PopupHorizontalAlign="WindowCenter"  PopupVerticalAlign="WindowCenter"  CloseAction="CloseButton"  >
                                <HeaderStyle Font-Bold="False" Font-Size="10"  HorizontalAlign="Left" />
                               <ContentCollection>
                                   <dx:PopupControlContentControl>
                                       <asp:Panel ID="panInsumos" runat="server">
                     
                                       <dx:ASPxPageControl ID="VistaInsumos" Width="100%" runat="server" CssClass="dxtcFixed" ActiveTabIndex="0" EnableHierarchyRecreation="True"  Theme="Office2010Blue">
        <TabPages>
            <dx:TabPage Text="Formulario de ingreso de insumos">
                <ContentCollection>
                    <dx:ContentControl ID="ContentControl1" runat="server">
                        <div>
                             <h4 style="color:#2d572c">Ingreso de nuevos insumos</h4>
                    <h5>Campos marcados con <font color="red">*</font> son obligatorios </h5>
                            <div class="form-group"> 
                           <label for="txtsubCod"aria-required="true"><font color="red">*</font>Programa: </label><asp:Label runat="server" id="lblprograma" styleCss="color:red;font-size:bold"></asp:Label>
                          </div> 
                             <div class="form-group"> 
                           <label for="txtsubCod"aria-required="true"><font color="red">*</font>Subprograma: </label><asp:Label runat="server" id="lblsubprograma" styleCss="color:red;font-size:bold"></asp:Label>
                          </div> 
                            <div class="form-group"> 
                           <label for="txtsubCod"aria-required="true"><font color="red">*</font>Producto: </label><asp:Label runat="server" id="lblproducto" styleCss="color:red;font-size:bold"></asp:Label>
                          </div> 
                            <div class="form-group"> 
                           <label for="txtsubCod"aria-required="true"><font color="red">*</font>Subproducto: </label><asp:Label runat="server" id="lblsubproducto" styleCss="color:red;font-size:bold"></asp:Label>
                          </div> 

                            <div class="form-group">
                           <label for="txtsubCod"aria-required="true"><font color="red">*</font>Fuente: </label>
                           <dx:ASPxComboBox ID="cbFuente" runat="server" ValueType="System.String" NullText="Seleccione fuente" CssClass="form-control" Width="500"></dx:ASPxComboBox>
                    </div> 
                            <div class="form-group">
                           <label for="txtsubCod"aria-required="true"><font color="red">*</font>Renglón presupuestario: </label>
                           <dx:ASPxComboBox ID="cbRenglon" runat="server" ValueType="System.String" NullText="Seleccione renglón" CssClass="form-control" Width="500"  OnSelectedIndexChanged="cbRenglon_SelectedIndexChanged" AutoPostBack="true"></dx:ASPxComboBox>
                    </div> 
                           
                            <div class="form-group">
                           <label for="txtsubCod"aria-required="true"><font color="red">*</font>Unidad ejecutora</label>
                           <asp:TextBox ID="txtUnidadEjecutora" runat="server" CssClass="form-control"  PlaceHolder="Ingrese unidad ejecutora" ></asp:TextBox>
                    </div> 

                             <div class="form-group">
                           <label for="txtsubCod"aria-required="true"><font color="red">*</font>Código de insumo: </label>
                           <dx:ASPxComboBox ID="cbInsumo" runat="server" ValueType="System.String" NullText="Seleccione insumo" CssClass="form-control"   ></dx:ASPxComboBox>
                           
                    </div> 
                            <asp:Panel runat="server" ID="descripcionyMedida">
                             <div class="form-group">
                  <label for="txt">Descripción insumo</label>
                
          <asp:TextBox ID="txtDescripcionInsumo" runat="server" CssClass="form-control"  PlaceHolder="Descripción insumo"></asp:TextBox>                              
     </div>
                               
                             <div class="form-group">
                           <label for="txtsubCod"aria-required="true"><font color="red">*</font>Unidad de medida: </label>
                           <dx:ASPxComboBox ID="cbUnidadMedida" runat="server" ValueType="System.String" NullText="Seleccione unidad de medida" CssClass="form-control" Width="500" >
                                
                           </dx:ASPxComboBox>
                    </div> 

                              </asp:Panel>

                            <div class="form-group">
                           <label for="txtsubCod"aria-required="true"><font color="red">*</font>Cantidad a programar: </label>
                           <asp:TextBox ID="txtCantidad" runat="server" CssClass="form-control"  PlaceHolder="Ingrese cantidad"  onkeypress="return filterFloat(event,this);"></asp:TextBox>
                           
                    </div> 

                              <div class="form-group">
                           <label for="txtsubCod"aria-required="true"><font color="red">*</font>Precio unitario del insumo: </label>
                           <asp:TextBox ID="txtPrecio" runat="server" CssClass="form-control"  PlaceHolder="Ingrese precio unitario" onkeypress="return filterFloat(event,this);"></asp:TextBox>
                           
                    </div> 
                            
                            <div class="form-group"> 
                           <label for="txtsubCod"aria-required="true"><font color="red">*</font>Total programado: </label><asp:Label runat="server" id="lblTotal" styleCss="color:red;font-size:bold"></asp:Label>
                          </div> 

                            <hr />
                            <label for="txtsubCod"aria-required="true"><font color="red">*</font>PROGRAMACIÓN</label>
                             <div class="form-group">
                           <label for="txtsubCod"aria-required="true"><font color="red">*</font>Cuatrimestre I: </label>
                           <asp:TextBox ID="txtCUA1" runat="server" CssClass="form-control"  PlaceHolder="Programación cuatrimestre I" onkeypress="return filterFloat(event,this);"></asp:TextBox>
                           
                    </div> 

                             <div class="form-group">
                           <label for="txtsubCod"aria-required="true"><font color="red">*</font>Cuatrimestre II:</label>
                           <asp:TextBox ID="txtCUA2" runat="server" CssClass="form-control"  PlaceHolder="Programación cuatrimestre II" onkeypress="return filterFloat(event,this);"></asp:TextBox>
                           
                    </div> 

                             <div class="form-group">
                           <label for="txtsubCod"aria-required="true"><font color="red">*</font>Cuatrimestre III:</label>
                           <asp:TextBox ID="txtCUA3" runat="server" CssClass="form-control"  PlaceHolder="Programación cuatrimestre III" onkeypress="return filterFloat(event,this);"></asp:TextBox>
                           
                    </div> 

                              <div class="form-group"> 
                           <label for="txtsubCod"aria-required="true"><font color="red">*</font>Total programado en cuatrimestres: </label><asp:Label runat="server" id="lblCuatrimestre" styleCss="color:red;font-size:bold"></asp:Label>
                          </div> 

                              <div class="form-group" style="display:flex; justify-content:flex-start;">                          
                             <asp:Button ID="btnGrabaIns" runat="server" Text="Guardar insumo"  CssClass="btn btn-primary" OnClick="btnGrabaIns_Click" />                                 
                                                       
                             <asp:Button ID="btnCancelaIns" runat="server" Text="Cancelar/Regresar"  CssClass="btn btn-warning" OnClick="btnCancelaIns_Click" />
                          </div> 
                        
                        </div>
                    </dx:ContentControl>
                </ContentCollection>
            </dx:TabPage>
            
                        <dx:TabPage Text="Ver insumos vinculados a este subproducto">
                <ContentCollection>
                    <dx:ContentControl ID="ContentControl3" runat="server">

                        <div class="form-group"> 
                           <label for="txtsubCod"aria-required="true"><font color="red">*</font>Programa: </label><asp:Label runat="server" id="lblPrograma2" styleCss="color:red;font-size:bold"></asp:Label>
                          </div> 
                             <div class="form-group"> 
                           <label for="txtsubCod"aria-required="true"><font color="red">*</font>Subprograma: </label><asp:Label runat="server" id="lblSubprograma2" styleCss="color:red;font-size:bold"></asp:Label>
                          </div> 
                            <div class="form-group"> 
                           <label for="txtsubCod"aria-required="true"><font color="red">*</font>Producto: </label><asp:Label runat="server" id="lblProducto2" styleCss="color:red;font-size:bold"></asp:Label>
                          </div> 
                            <div class="form-group"> 
                           <label for="txtsubCod"aria-required="true"><font color="red">*</font>Subproducto: </label><asp:Label runat="server" id="lblSubproducto2" styleCss="color:red;font-size:bold"></asp:Label>
                          </div> 


                        
                                                               <dx:ASPxGridView ID="gvInsumos" runat="server" KeyFieldName="SPINS$ID"   Theme="Office2010Blue"  AutoGenerateColumns="False" SettingsBehavior-AllowFocusedRow ="true" SettingsBehavior-ConfirmDelete="true" OnRowDeleting="gvInsumos_RowDeleting"  OnCommandButtonInitialize="gvInsumos_CommandButtonInitialize">
                             
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
                                       
                                       
                                    </SettingsCommandButton>
            
             <SettingsLoadingPanel Mode="ShowOnStatusBar" />
             <SettingsText CommandApplyFilter="Aplicar filtro" CommandApplySearchPanelFilter="asdasfasdfasdf" ContextMenuHideColumn="Ocultar Columna" ContextMenuShowCustomizationWindow="Mostrar Columnas" ContextMenuShowFilterRow="Fila de Filtros" ContextMenuShowFilterRowMenu="Seleccione Tipo de Filtro" ContextMenuShowSearchPanel="Buscar" CustomizationWindowCaption="Columnas Ocultas" GroupPanel="Arrastre las columnas que desee agrupar" />
                             <Columns>
                                  <dx:GridViewDataTextColumn FieldName="SPINS$ID"  Name="SPINS$ID" ReadOnly="true" Visible="false" VisibleIndex="0">
                                     <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn>
                                 <dx:GridViewDataTextColumn FieldName="SPINS$UNIDAD_EJECUTORA" Caption="Unidad ejecutora"  Name="SPINS$UNIDAD_EJECUTORA" ReadOnly="true" Visible="true" VisibleIndex="1">
                                     <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn>
                                 <dx:GridViewDataComboBoxColumn FieldName="SPINS$FUENTE" Name="SPINS$FUENTE" Caption="Fuente" VisibleIndex="2">
                
                                  </dx:GridViewDataComboBoxColumn>


                                 <dx:GridViewDataTextColumn FieldName="SPINS$RENGLON"  Name="SPINS$RENGLON"  Caption="Renglon" ReadOnly="true" Visible="true"  VisibleIndex="3">
                                     <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn>

                                                                                                   
                                 <dx:GridViewDataTextColumn FieldName="SPINS$CODIGO"  Name="SPINS$CODIGO"    ReadOnly="false" Visible="false" VisibleIndex="4">
                                     <HeaderStyle Wrap="True" />
                                    <Settings  AutoFilterCondition="Contains" />
                                    </dx:GridViewDataTextColumn>

                                  <dx:GridViewDataTextColumn FieldName="INSUMO_DESC"  Name="INSUMO_DESC"  ReadOnly="true" Visible="true"  VisibleIndex="5" Caption="Insumo">
                                     <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn>

                                 <dx:GridViewDataTextColumn FieldName="UNIDAD_MEDIDA"  Name="UNIDAD_MEDIDA"  caption="Unidad de medida" ReadOnly="false" Visible="true"  VisibleIndex="6">
                                     <HeaderStyle Wrap="True" />
                                     <Settings  AutoFilterCondition="Contains" />
                                    </dx:GridViewDataTextColumn>
                                 
                                 <dx:GridViewDataTextColumn FieldName="SPINS$PRESENTACION"  Name="SPINS$PRESENTACION" ReadOnly="true"  Visible="false" VisibleIndex="7">
                                     <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn>

                                  <dx:GridViewBandColumn Caption="Programado" VisibleIndex="8">
                                        <Columns>   
                                            <dx:GridViewDataTextColumn FieldName="SPINS$PRECIO_UNITARIO"  Name="SPINS$PRECIO_UNITARIO" Caption="Precio unitario"  ReadOnly="true"  Visible="true" VisibleIndex="0">
                                     <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn>

                                             <dx:GridViewDataTextColumn FieldName="SPINS$CANTIDAD"  Name="SPINS$CANTIDAD" Caption="Cantidad"  ReadOnly="true"  Visible="true" VisibleIndex="1">
                                     <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn>

                                            <dx:GridViewDataTextColumn FieldName="TOTAL_PROGRAMADO"  Name="TOTAL_PROGRAMADO" Caption="Total programado" ReadOnly="true"  Visible="true" VisibleIndex="2">
                                     <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn>
                                            </Columns>
                                </dx:GridViewBandColumn>

                                 <dx:GridViewBandColumn Caption="Programado en cuatrimestres" VisibleIndex="8">
                                        <Columns>
                                               <dx:GridViewDataTextColumn FieldName="SPINS$CUATRIMESTRE1"  Name="SPINS$CUATRIMESTRE1"  Caption="1er Cuatrimestre" ReadOnly="true"  Visible="true" VisibleIndex="0">
                                     <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn>

                                            <dx:GridViewDataTextColumn FieldName="SPINS$CUATRIMESTRE2"  Name="SPINS$CUATRIMESTRE2"  Caption="2do Cuatrimestre" ReadOnly="true"  Visible="true" VisibleIndex="1">
                                     <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn>

                                            <dx:GridViewDataTextColumn FieldName="SPINS$CUATRIMESTRE3"  Name="SPINS$CUATRIMESTRE3"  Caption="3er Cuatrimestre" ReadOnly="true"  Visible="true" VisibleIndex="2">
                                     <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn>

                                                <dx:GridViewDataTextColumn FieldName="TOTAL_CUATRIMESTRE"  Name="TOTAL_CUATRIMESTRE"  Caption="Total cuatrimestre" ReadOnly="true"  Visible="true" VisibleIndex="3">
                                     <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn>


                                            </Columns>
                                 
                                </dx:GridViewBandColumn>
                                     <dx:GridViewCommandColumn ShowNewButtonInHeader ="false"  ShowDeleteButton="true" VisibleIndex="10">
                                                </dx:GridViewCommandColumn>
                                
                             </Columns>         
                                                                   <SettingsEditing Mode="inline" />

                                                                  <TotalSummary>
                                                                      <dx:ASPxSummaryItem FieldName="TOTAL_PROGRAMADO" SummaryType="Sum" />
                                                                      <dx:ASPxSummaryItem FieldName="SPINS$CUATRIMESTRE1" SummaryType="Sum" />
                                                                      <dx:ASPxSummaryItem FieldName="SPINS$CUATRIMESTRE2" SummaryType="Sum" />
                                                                      <dx:ASPxSummaryItem FieldName="SPINS$CUATRIMESTRE3" SummaryType="Sum" />
                                                                      <dx:ASPxSummaryItem FieldName="TOTAL_CUATRIMESTRE" SummaryType="Sum" />
                                                                      </TotalSummary>
                              
                                                               
                                                               </dx:ASPxGridView>
                        
                   
          




                        
                    </dx:ContentControl>
                </ContentCollection>
            </dx:TabPage>                       
        </TabPages>
    </dx:ASPxPageControl>
</asp:Panel>

                                             </dx:PopupControlContentControl>
                                   </ContentCollection>
                           </dx:ASPxPopupControl>
                                 </ContentTemplate>
              </asp:UpdatePanel>
        


         <%--   <asp:UpdatePanel ID="UpdatePanel4" runat="server">
        <ContentTemplate>--%>
            <dx:ASPxPopupControl ID="popReportePOM" runat="server" CssClass="pop" AllowDragging="true" Width="500px" Height="350" PopupHorizontalAlign="WindowCenter" HeaderText="Generar reporte POM" PopupVerticalAlign="WindowCenter" AllowResize="true" Modal="true">
                <HeaderStyle Font-Bold="true" Font-Size="X-Large" ForeColor="#003399" HorizontalAlign="Center" />
                <ContentCollection>
                    <dx:PopupControlContentControl>
                        <div>
                            <div style="text-align:justify">
                             <label for="txtsubCod" aria-required="true"><font color="red">*</font>Ingrese nombre y cargo de la maxima autoridad institucional, esta información se desplegará en la sección de firmas del reporte</label>
                                 </div>              
                            <br />
                            
                             <div class="form-group">
                           <label for="txtsubCod" aria-required="true"><font color="red">*</font>Nombre de la máxima autoridad </label>
                             <asp:TextBox ID="txtAutoridad1" PlaceHolder="Nombre de la máxima autoridad" runat="server" CssClass="form-control"  MaxLength="200"></asp:TextBox>
                              </div> 

                             <div class="form-group">
                           <label for="txtsubCod" aria-required="true"><font color="red">*</font>Cargo de la máxima autoridad   </label>
                             <asp:TextBox ID="txtCargo1" PlaceHolder="Cargo de la máxima autoridad" runat="server" CssClass="form-control"  MaxLength="200"></asp:TextBox>
                              </div> 

                            <p>
                                <br />
                                <br />
                            </p>
                            <div class="form-group">                                
                                <div class="col-lg-4 col-lg-offset-1">
                                    <asp:Button ID="btnGeneraPOM" runat="server" Text="Generar" CssClass="btn btn-success btn-block" OnClick="btnGeneraPOM_Click" />
                                </div>
                                <div class="col-lg-4 col-lg-offset-2">
                                    <asp:Button ID="btnCancelaPOM" runat="server" Text="Cerrar" CssClass="btn btn-primary btn-block" OnClick="btnCancelaPOM_Click"   />
                                </div>
                            </div>
                        </div>
                        <p>
                            <br />
                            <br />                            
                            <br />
                            <br />
                        </p>
                    </dx:PopupControlContentControl>
                </ContentCollection>
            </dx:ASPxPopupControl>            
       <%-- </ContentTemplate>
    </asp:UpdatePanel>--%>


        <asp:UpdatePanel ID="UpdatePanel5" runat="server">
        <ContentTemplate>
            <dx:ASPxPopupControl ID="popReportePOA" runat="server" CssClass="pop" AllowDragging="true" Width="500px" Height="350" PopupHorizontalAlign="WindowCenter" HeaderText="Generar reporte POA" PopupVerticalAlign="WindowCenter" AllowResize="true" Modal="true">
                <HeaderStyle Font-Bold="true" Font-Size="X-Large" ForeColor="#003399" HorizontalAlign="Center" />
                <ContentCollection>
                    <dx:PopupControlContentControl>
                        <div>
                            <div style="text-align:justify">
                             <label for="txtsubCod" aria-required="true"><font color="red">*</font>Ingrese nombre y cargo de la maxima autoridad institucional, esta información se desplegará en la sección de firmas del reporte</label>
                                 </div>              
                            <br />
                            
                             <div class="form-group">
                           <label for="txtsubCod" aria-required="true"><font color="red">*</font>Nombre de la máxima autoridad </label>
                             <asp:TextBox ID="txtAutoridad2" PlaceHolder="Nombre de la máxima autoridad" runat="server" CssClass="form-control"  MaxLength="200"></asp:TextBox>
                              </div> 

                             <div class="form-group">
                           <label for="txtsubCod" aria-required="true"><font color="red">*</font>Cargo de la máxima autoridad   </label>
                             <asp:TextBox ID="txtCargo2" PlaceHolder="Cargo de la máxima autoridad" runat="server" CssClass="form-control"  MaxLength="200"></asp:TextBox>
                              </div> 

                            <p>
                                <br />
                                <br />
                            </p>
                            <div class="form-group">                                
                                <div class="col-lg-4 col-lg-offset-1">
                                    <asp:Button ID="btnGeneraPOA" runat="server" Text="Generar" CssClass="btn btn-success btn-block" OnClick="btnGeneraPOA_Click"  />
                                </div>
                                <div class="col-lg-4 col-lg-offset-2">
                                    <asp:Button ID="btnCancelaPOA" runat="server" Text="Cerrar" CssClass="btn btn-primary btn-block" OnClick="btnCancelaPOA_Click"   />
                                </div>
                            </div>
                        </div>
                        <p>
                            <br />
                            <br />                            
                            <br />
                            <br />
                        </p>
                    </dx:PopupControlContentControl>
                </ContentCollection>
            </dx:ASPxPopupControl>            
        </ContentTemplate>
    </asp:UpdatePanel>

    </div>
    <script>
function filterFloat(evt,input){
    // Backspace = 8, Enter = 13, ‘0′ = 48, ‘9′ = 57, ‘.’ = 46, ‘-’ = 43
    var key = window.Event ? evt.which : evt.keyCode;    
    var chark = String.fromCharCode(key);
    var tempValue = input.value+chark;
    if(key >= 48 && key <= 57){
        if(filter(tempValue)=== false){
            return false;
        }else{       
            return true;
        }
    }else{
          if(key == 8 || key == 13 || key == 0) {     
              return true;              
          }else if(key == 46){
                if(filter(tempValue)=== false){
                    return false;
                }else{       
                    return true;
                }
          }else{
              return false;
          }
    }
}
function filter(__val__){
    var preg = /^([0-9]+\.?[0-9]{0,2})$/; 
    if(preg.test(__val__) === true){
        return true;
    }else{
       return false;
    }
    
}
    </script>
                     
</asp:Content>