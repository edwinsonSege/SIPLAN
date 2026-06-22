<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="reprogramacion.aspx.cs"  MasterPageFile="~/Site.Master" Inherits="SIPLAN2._0.reprogramacion.reprogramacion" %>
<%@ Register Assembly="DevExpress.Web.v17.2, Version=17.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
   

   <script>
        function Alerta(mensaje, tipo) {          
            alertify.set('notifier', 'position', 'top-right'); 
            if (tipo == 1)
                alertify.success(mensaje);
            if (tipo == 2)
                alertify.error(mensaje);          
            if (tipo == 3)
                alertify.alert(mensaje);      

       };  

      

          function NumCheckmoney(e, field) {
            key = e.keyCode ? e.keyCode : e.which
            // backspace
            if (key == 8) return true
            // 0-9

            if (key > 47 && key < 58) {
                if (field.value == "") return true
                regexp = /.[0-9]{15}$/
                // regexp= /^(\d{1}\.)?(\d+\.?)+(,\d{2})?$/
                return !(regexp.test(field.value))
            }
            // .
            if (key == 46) {
                if (field.value == "") return false
                regexp = /^[0-9]+$/



                return regexp.test(field.value)
            }


            // other key
            return false

       };

       function isNumberKey(evt)
       {
          var charCode = (evt.which) ? evt.which : evt.keyCode;
          if (charCode != 46 && charCode > 31 
            && (charCode < 48 || charCode > 57))
             return false;

          return true;
       }

       function limpia()
       {
          
       rbApprovalCodeData.SetValue(-1); 
           
       }

       function limpia2()
       {
          
       subsos.SetValue(-1); 
           
       }
       
</script>
    <style>
        #contenedor {
  width: 120%;
}

#update {
  display: inline-block;
  margin-left:25px;
}

#sinupdate {
  display: inline-block;


}

#siguiente {
  display: inline-block;


}
 #contenedor2 {
  width: 100%;
}

#update2 {
  display: inline-block;
  margin-left:25px;
}

#sinupdate2 {
  display: inline-block;
}

#contenedor3 {
  width: 100%;
}

#update3 {
  display: inline-block;
  margin-left:25px;
}

#sinupdate3 {
  display: inline-block;

    </style>
     <asp:UpdatePanel ID="UpdatePanel10" runat="server">
            <ContentTemplate>
                       <div class="row" style="background-color:#39a9dc">
            <div class="col-sm-2">
                <div style="text-align:center;margin-top:20px;margin-left:50px">
                <img src="../images/mensaje.png"  width="50%"/>
                    </div>
            </div>


            <div class="col-sm-10">
               <div style="text-align:center">
                <h3 style="color:white">INFORMACIÓN IMPORTANTE</h3>
                   </div>
                <div style="text-align:justify;margin-right:30px">
                    <p style="font-size:14px;color:azure"><b>Información importante, para el presente ejercicio:</b> 1) Debe ingresar la reprogramaciones en orden cronológico segun la fecha de resolución, de las más antiguas a las más recientes, el sistema le desplegará error si ingresa la información de forma desordenada<br /> 2) Los administradores del SIPLAN solo podrán habilitar la ultima reprogramación aprobada por la institución dentro del sistema</p>
                </div>
            </div>

        </div>
                </ContentTemplate>
          </asp:UpdatePanel>



    <h3 style="color:blue;margin-left:10px">Reprogramación de metas físicas y financieras para productos y subproductos <asp:Label ID="lblTitulo" runat="server"></asp:Label></h3>
     <div class="form-group" style="margin-left:10px">
                           <label for="txtsubCod"aria-required="true"><font color="red">*</font>Seleccione el periodo del POM correspondiente: </label>
                           <dx:ASPxComboBox ID="cbPeriodos" runat="server" ValueType="System.String" NullText="Seleccione periodo de POM" CssClass="form-control" AutoPostBack="true" Width="300"></dx:ASPxComboBox>
     </div> 
    <div class="form-group" style="margin-left:10px">
                           <label for="txtsubCod"aria-required="true">Año: </label>
                           <dx:ASPxComboBox ID="cbanio" runat="server" ValueType="System.String" NullText="Seleccione año a reprogramar" CssClass="form-control" Width="300"  AutoPostBack="true"></dx:ASPxComboBox>
                    </div> 
                    <div class="form-group" style="margin-left:10px">
                        <label for="txtsubCod"aria-required="true">Institución: </label>
                        <dx:ASPxComboBox ID="cbInstituiciones" runat="server" ValueType="System.String" NullText="Seleccione institución" CssClass="form-control" AutoPostBack="true"></dx:ASPxComboBox>
                    </div>
    <asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex="0">
         <asp:View ID="vistaReprogramacion" runat="server">
   <div id="contenedor" class="row">
    <div id="update" >
           <asp:UpdatePanel ID="UpdatePanel1" runat="server">
               <ContentTemplate>
                   <asp:Button ID="btnNuevo" runat="server" Text="Nueva reprogramación"  CssClass="btn btn-primary" ToolTip="Nuevo encabezado reprogramación" OnClick="btnNuevo_Click" Style="font-size:0.7em"/>
                  <asp:Button ID="btnVista" runat="server" Text="Ver resolución"  CssClass="btn btn-warning" ToolTip="Vista previa resolución" OnClick="btnVista_Click" Style="font-size:0.7em"/>
                   <asp:Button ID="btnEditar" runat="server" Text="Editar resolución"  CssClass="btn btn-success" ToolTip="Editar encabezado reprogramación" OnClick="btnEditar_Click"  Style="font-size:0.7em"/>                    
               </ContentTemplate>  
             </asp:UpdatePanel>         
         </div>
       <div id="sinupdate" > 
        <asp:Button ID="btnProd" runat="server" Text="Reprogramar productos"  CssClass="btn " ToolTip="Reprogramar productos"  OnClick="btnProd_Click" style="color:white;background-color:#2E9AFE;font-size:0.7em" />
        <asp:Button ID="btnSubprod" runat="server" Text="Reprogramar Subproductos"  CssClass="btn " ToolTip="Reprogramar subproductos"  OnClick="btnSubprod_Click" style="color:white;background-color:#075F6A;font-size:0.7em" />      
       </div>

       <div id ="siguiente">
            <asp:UpdatePanel ID="UpdatePanel8" runat="server">
               <ContentTemplate>
                   <asp:Button ID="btnBorrar" runat="server" Text="Borrar reprogramación"  CssClass="btn btn-danger" ToolTip="Borrar reprogramación"  OnClick="btnBorrar_Click" Style="font-size:0.7em" />
                   <asp:Button ID="btnVerproductos" runat="server" Text="Ver metas reprogramadas"  CssClass="btn" ToolTip="Visualizar los productos/subproductos modificados en esta reprogramación" style="color:white;background-color:#90570E;font-size:0.7em"  OnClick="btnVerproductos_Click"/> 
                   <asp:Button ID="btnAprobar" runat="server" Text="Aprobar reprogramación"  CssClass="btn" ToolTip="Aprobar reprogramación" style="color:white;background-color:#979FA0;font-size:0.7em" OnClick="btnAprobar_Click"/> 
                   <asp:Button ID="btnDesaprobar" runat="server" Text="Abrir reprogramación"  CssClass="btn" ToolTip="Abrir reprogramación aprobada" style="color:white;background-color:#000000;font-size:0.7em"  OnClick="btnDesaprobar_Click" Visible="false"/>            
                  <asp:Button ID="btnRegresarinicio" runat="server" Text="Ir a pagina principal"  CssClass="btn " ToolTip="Regresar a pagina de inicio"   OnClick="btnRegresarinicio_Click" style="color:white;background-color:#F28B08;font-size:0.6em" />
                   </ContentTemplate>
                </asp:UpdatePanel>
       </div>
       </div>
    
            <div  style ="overflow:auto; width:100%; height:90%; padding: 1px 10px 1px 10px;   ">                     
               <asp:UpdatePanel ID="panGrid" runat="server">
                          <ContentTemplate>
                              <h4>Reprogramaciones en letras azules, se encuentran aprobadas</h4>
                              <dx:ASPxGridView ID="gvReprogramaciónes" runat="server" KeyFieldName="SPPRE$ID_REPRO"   Theme="Office2010Blue" Width="100%"   SettingsBehavior-ConfirmDelete="true"  OnHtmlDataCellPrepared="gvReprogramaciónes_HtmlDataCellPrepared" OnHtmlRowPrepared="gvReprogramaciónes_HtmlRowPrepared" >
                                  <Settings ShowFooter="True"/>                           
                            <SettingsLoadingPanel Text="Cargando&amp;hellip;" />
          <Settings ShowFilterRow="True" ShowGroupPanel="True" />

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
                  <SettingsPager AlwaysShowPager="True" PageSize="200" ShowSeparators="True">
                 <PageSizeItemSettings Visible="True">
                 </PageSizeItemSettings>
             </SettingsPager>
             <SettingsLoadingPanel Mode="ShowOnStatusBar" />
                                  <Columns>
                                      <dx:GridViewDataTextColumn FieldName="SPPRE$ID_REPRO"  Name="SPPRE$ID_REPRO"   Visible="false" VisibleIndex="0" >
                                     <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn>
                                     
                                        <dx:GridViewDataTextColumn FieldName="SPPRE$RESOLUCION"  Name="SPPRE$RESOLUCION"  Caption="No. De resolución"  Visible="true" VisibleIndex="1" >
                                     <CellStyle HorizontalAlign="center"></CellStyle>
                                            <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn>
                                      <dx:GridViewDataDateColumn FieldName="SPPRE$FECHA_RESOLUCION"  Name="SPPRE$FECHA_RESOLUCION"  Caption="Fecha de resolución"  Visible="true" VisibleIndex="2" >
                                     <PropertiesDateEdit DisplayFormatString="dd/MM/yyyy" />
                                    <CellStyle HorizontalAlign="center"></CellStyle>
                                     <Settings />
                                    </dx:GridViewDataDateColumn>
                                       <dx:GridViewDataTextColumn FieldName="SPPRE$OBSERVACION"  Name="SPPRE$OBSERVACION"  Caption="Observaciones"  Visible="false" VisibleIndex="3" >
                                     <CellStyle HorizontalAlign="justify"></CellStyle>
                                     <Settings />
                                    </dx:GridViewDataTextColumn>
                                      <dx:GridViewDataDateColumn FieldName="SPPRE$FECHA_INGRESO"  Name="SPPRE$FECHA_INGRESO"  Caption="Fecha de ingreso al sistema"  Visible="true" VisibleIndex="4" >
                                    <PropertiesDateEdit DisplayFormatString="dd/MM/yy" />
                                    <CellStyle HorizontalAlign="center"></CellStyle>
                                          <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataDateColumn>
                                      <dx:GridViewDataDateColumn FieldName="SPPRE$FECHA_APRUEBA"  Name="SPPRE$FECHA_APRUEBA"  Caption="Fecha de aprobación"  Visible="true" VisibleIndex="5" >
                                     <PropertiesDateEdit DisplayFormatString="dd/MM/yy" />
                                    <CellStyle HorizontalAlign="center"></CellStyle>
                                          <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataDateColumn>
                                      <dx:GridViewDataTextColumn FieldName="ARCHIVO"  Name="ARCHIVO"  Caption="Archivo adjunto"  Visible="true" VisibleIndex="6" >
                                     <CellStyle HorizontalAlign="center"></CellStyle>
                                           <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn>
                                       <dx:GridViewDataTextColumn FieldName="SPPRE$APROBADO"  Name="SPPRE$APROBADO"  Caption="Aprobado"  Visible="true" VisibleIndex="7" >
                                     <CellStyle HorizontalAlign="center"></CellStyle>
                                          <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn>
                                       
                                      <dx:GridViewDataTextColumn FieldName="SPPRE$USUARIOAPRUEBA"  Name="SPPRE$USUARIOAPRUEBA"  Caption="Usuario que aprobó"  Visible="true" VisibleIndex="8" >
                                     <CellStyle HorizontalAlign="justify"></CellStyle>
                                          <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn>
                                       <dx:GridViewDataTextColumn FieldName="VIGENTE"  Name="VIGENTE"  Caption="Reprogramación Vigente"  Visible="false" VisibleIndex="9" >
                                     <CellStyle HorizontalAlign="center"></CellStyle>
                                           <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn>
                                  <dx:GridViewDataTextColumn FieldName="SPPRE$ARCHIVO"  Name="SPPRE$ARCHIVO"   Visible="false" VisibleIndex="10" >
                                     
                                     <Settings />
                                    </dx:GridViewDataTextColumn>    
                                   <dx:GridViewDataTextColumn FieldName="SPPRE$POM"  Name="SPPRE$POM"   Visible="false" VisibleIndex="11" >
                                     
                                     <Settings />
                                    </dx:GridViewDataTextColumn>   

                                       <dx:GridViewDataTextColumn FieldName="SPPRE$POA"  Name="SPPRE$POA"   Visible="false" VisibleIndex="12" >
                                     
                                     <Settings />
                                    </dx:GridViewDataTextColumn>   

                                  </Columns>
                                  <SettingsBehavior AllowFocusedRow="true" />  
               
                              </dx:ASPxGridView>
                              </ContentTemplate>
                   </asp:UpdatePanel>
                </div>
             </asp:View>    
        <asp:View ID="vistaProd" runat="server">
            <h4 style="margin-left:10px"><asp:Label ID="lbResolucion" runat="server"></asp:Label></h4>
            <h4 style="margin-left:10px"><asp:Label ID="lblFecha" runat="server"></asp:Label></h4>
            <h5 style="color:red;margin-left:10px">Reprogramación de metas físicas para productos<asp:Label ID="lblmgAp" runat="server"></asp:Label></h5>
         <div id="contenedor2" class="row">
             <div id="update2" >
           <asp:UpdatePanel ID="UpdatePanel2" runat="server">
               <ContentTemplate>
                   <asp:Button ID="btnReprogra" runat="server" Text="Reprogramar metas físicas"  CssClass="btn btn-primary" ToolTip="Reprogramar productos" OnClick="btnReprogra_Click"/>
                   <asp:Button ID="btnBorrarRepro" runat="server" Text="Borrar metas físicas"  CssClass="btn btn-danger" ToolTip="Borrar reprogramación"   OnClick="btnBorrarRepro_Click"/>
                  <asp:Button ID="btnFinancieraProd" runat="server" Text="Reprogramar metas financieras"  CssClass="btn btn-success" ToolTip="Reprogramar metas financieras productos"  OnClick="btnFinancieraProd_Click"/>
                   <asp:Button ID="btnBorrarReproFin" runat="server" Text="Borrar metas metas financieras"  CssClass="btn btn-danger" ToolTip="Borrar metas financieras productos"  OnClick="btnBorrarReproFin_Click" />
                   </ContentTemplate>
               </asp:UpdatePanel>
         </div>
             <div id="sinupdate2" > 
        <asp:Button ID="btnRegresar" runat="server" Text="Regresar"  CssClass="btn " ToolTip="Regresar a vista principal"  style="color:white;background-color:#FFC133"  OnClick="btnRegresar_Click"/>
     </div>
             </div>
         <div  style ="overflow:auto; width:100%; height:90%; padding: 1px 10px 1px 10px;   ">                     
               <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                          <ContentTemplate>
                              <dx:ASPxGridView ID="gvProductos" runat="server" KeyFieldName="SPPMFS$ID_PRODUCTO"   Theme="Office2010Blue" Width="100%"   SettingsBehavior-ConfirmDelete="true" >
                                   <Settings ShowFooter="True"/>                           
                            <SettingsLoadingPanel Text="Cargando&amp;hellip;" />
          <Settings ShowFilterRow="True" ShowGroupPanel="True" />

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
                  <SettingsPager AlwaysShowPager="True" PageSize="200" ShowSeparators="True">
                 <PageSizeItemSettings Visible="True">
                 </PageSizeItemSettings>
             </SettingsPager>
             <SettingsLoadingPanel Mode="ShowOnStatusBar" />
                                  <Columns>
                                      <dx:GridViewDataTextColumn FieldName="SPRES$TIPO"  Name="SPRES$TIPO"   Visible="false" VisibleIndex="0" >
                                     <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn>
                                      <dx:GridViewDataTextColumn FieldName="SPPRO$ID_RESULTADO"  Name="SPPRO$ID_RESULTADO"   Visible="false" VisibleIndex="1" >
                                     <CellStyle HorizontalAlign="center"></CellStyle>
                                          <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn>
                                       <dx:GridViewDataTextColumn FieldName="TIPO"  Name="TIPO"  Caption="Tipo de producto"  Visible="true" VisibleIndex="2" GroupIndex="0" >
                                     <CellStyle HorizontalAlign="center"></CellStyle>
                                           <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn FieldName="NOMBRE"  Name="NOMBRE"  Caption="RI-PGG/RE"  Visible="true" VisibleIndex="3" GroupIndex="1" >
                                     <CellStyle HorizontalAlign="justify"></CellStyle>
                                            <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn>
                                      <dx:GridViewDataTextColumn FieldName="SPPMFS$ID_PROGRAMACION_FISICA"  Name="SPPMFS$ID_PROGRAMACION_FISICA"    Visible="false" VisibleIndex="4" >
                                     <CellStyle HorizontalAlign="justify"></CellStyle>
                                            <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn>
                                      
                                       <dx:GridViewDataTextColumn FieldName="SPPMFS$ID_PRODUCTO"  Name="SPPMFS$ID_PRODUCTO"  Visible="false" VisibleIndex="5" >
                                     <CellStyle HorizontalAlign="justify"></CellStyle>
                                     <Settings />
                                    </dx:GridViewDataTextColumn>

                                      <dx:GridViewDataTextColumn FieldName="SPPRO$PRESUPUESTO"  Name="SPPRO$PRESUPUESTO"  Visible="true" VisibleIndex="6"  Caption="No. de Programa Presupuestario">
                                     <CellStyle HorizontalAlign="justify"></CellStyle>
                                     <Settings />
                                    </dx:GridViewDataTextColumn>
                                     
                                      <dx:GridViewDataTextColumn FieldName="SPPRO$DESCRIPCION"  Name="SPPRO$DESCRIPCION"  Caption="Producto"  Visible="true" VisibleIndex="7" >
                                     <CellStyle HorizontalAlign="justify"></CellStyle>
                                          <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn>
                                       <dx:GridViewDataTextColumn FieldName="SNCGUM$NOMBRE"  Name="SNCGUM$NOMBRE"  Caption="Unidad de medida producto"  Visible="true" VisibleIndex="8" >
                                     <CellStyle HorizontalAlign="center"></CellStyle>
                                           <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn>
                                  <dx:GridViewDataTextColumn FieldName="METAFINANCIERA"  Name="METAFINANCIERA"  Caption="Meta financiera vigente producto"  Visible="true" VisibleIndex="9" >
                                         <PropertiesTextEdit DisplayFormatString="{0:N2}">
                                                            </PropertiesTextEdit>
                                       <CellStyle HorizontalAlign="center"></CellStyle>
                                           <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn>   
                                       <dx:GridViewDataTextColumn FieldName="METAFISICAINICIAL"  Name="METAFISICAINICIAL" Caption="Meta física inicial"  Visible="true" VisibleIndex="10" >
                                         <PropertiesTextEdit DisplayFormatString="{0:N2}">
                                                            </PropertiesTextEdit>
                                       <CellStyle HorizontalAlign="center"></CellStyle>
                                           <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn> 
                                       <dx:GridViewDataTextColumn FieldName="METAVIGENTE"  Name="METAVIGENTE" Caption="Meta física vigente"  Visible="true" VisibleIndex="11" >
                                         <PropertiesTextEdit DisplayFormatString="{0:N2}">
                                                            </PropertiesTextEdit>
                                       <CellStyle HorizontalAlign="center"></CellStyle>
                                           <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn>   
                                       <dx:GridViewDataTextColumn FieldName="OPERACIONREALIZADA"  Name="OPERACIONREALIZADA"  Caption="Operación realizada (+-)"  Visible="true" VisibleIndex="12" >
                                          <CellStyle HorizontalAlign="center"></CellStyle>
                                           <HeaderStyle Wrap="True" />
                                           
                                     <Settings />
                                    </dx:GridViewDataTextColumn>   

                                      <dx:GridViewDataTextColumn FieldName="REPROFISICA"  Name="REPROFISICA" Caption="Meta física para esta reprogramación"  Visible="true" VisibleIndex="13" >
                                         <PropertiesTextEdit DisplayFormatString="{0:N2}">
                                                            </PropertiesTextEdit>
                                       <CellStyle HorizontalAlign="center"></CellStyle>
                                           <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn>  
                                                                  

                                    <dx:GridViewDataTextColumn FieldName="NIDMF"  Name="NIDMF"   Visible="false" VisibleIndex="14" >
                                         
                                           <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn>   

                                      <dx:GridViewDataTextColumn FieldName="IDREPROGRA"  Name="IDREPROGRA"   Visible="false" VisibleIndex="15" >
                                         
                                           <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn>   
                                     
                                      <dx:GridViewDataTextColumn FieldName="OPERACION"  Name="OPERACION"   Visible="false" VisibleIndex="17" >
                                         
                                           <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn> 
                                      <dx:GridViewDataTextColumn FieldName="CANTIDAD"  Name="CANTIDAD"   Visible="false" VisibleIndex="18" >
                                         
                                           <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn>
                                      

                                      <dx:GridViewDataTextColumn FieldName="NIDMFIN"  Name="NIDMFIN"   Visible="false" VisibleIndex="19" >
                                         
                                           <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn>   

                                      <dx:GridViewDataTextColumn FieldName="REPROFINANCIERA"  Name="REPROFINANCIERA"   Visible="false" VisibleIndex="20" >
                                         
                                           <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn>   
                                     
                                      <dx:GridViewDataTextColumn FieldName="OPERACIONFINANCEIRA"  Name="OPERACIONFINANCEIRA"  Caption="Operación realizada (+-)"  Visible="true" VisibleIndex="21" >
                                         
                                           <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn> 
                                      <dx:GridViewDataTextColumn FieldName="CANTIDADFINANCIERA"  Name="CANTIDADFINANCIERA"   Visible="false" VisibleIndex="22" >
                                         
                                           <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn>

                                      <dx:GridViewDataTextColumn FieldName="NUEVAMETAFINANCIERA"  Name="NUEVAMETAFINANCIERA"   Caption="Meta financiera para esta reprogramación" Visible="true" VisibleIndex="23" >
                                         
                                           <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn>
                                      <dx:GridViewDataTextColumn FieldName="OPERAFINANCIERA"  Name="OPERAFINANCIERA"    Visible="false" VisibleIndex="24" >
                                         
                                           <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn>
                                  </Columns>
                                  <SettingsBehavior AllowFocusedRow="true" />  
                                  </dx:ASPxGridView>
                              </ContentTemplate>
                   </asp:UpdatePanel>
             </div>


        </asp:View>
        <asp:View ID="subproductos" runat="server">
            <h4 style="margin-left:10px"><asp:Label ID="lblreSub" runat="server"></asp:Label></h4>
            <h4 style="margin-left:10px"><asp:Label ID="lblFechsub" runat="server"></asp:Label></h4>
               <h4 style="color:red;margin-left:10px">Reprogramación de metas físicas y financieras para Subproductos<asp:Label ID="lblmsgSub" runat="server"></asp:Label></h4>
         <div id="contenedor3" class="row">
             <div id="update3" >
           <asp:UpdatePanel ID="UpdatePanel5" runat="server">
               <ContentTemplate>
                   <asp:Button ID="btnReprosub" runat="server" Text="Reprogramar metas físicas"  CssClass="btn btn-primary" ToolTip="Reprogramar metas físicas de subproductos" OnClick="btnReprosub_Click" Style="font-size:0.8em"/>
                    <asp:Button ID="btnBorrarSub" runat="server" Text="Borrar metas físicas"  CssClass="btn btn-danger" ToolTip="Borrar reprogramación de metas físicas"  OnClick="btnBorrarSub_Click" Style="font-size:0.8em" />
                   <asp:Button ID="btnMetasfin" runat="server" Text="Reprogramar metas financieras"  CssClass="btn btn-success" ToolTip="Reprogramar metas financieras de subproductos" OnClick="btnMetasfin_Click" Style="font-size:0.8em"/>
                  <asp:Button ID="btnBorrarSubfin" runat="server" Text="Borrar metas financieras"  CssClass="btn btn-danger" ToolTip="Borrar reprogramación de metas financieras"  OnClick="btnBorrarSubfin_Click" Style="font-size:0.8em"/>
                   </ContentTemplate>
               </asp:UpdatePanel>
         </div>
             <div id="sinupdate3" > 
        <asp:Button ID="btnRegresaPrin" runat="server" Text="Regresar"  CssClass="btn " ToolTip="Regresar a vista principal"  style="color:white;background-color:#FFC133"  OnClick="btnRegresar_Click"/>
     </div>
             </div>

            <div  style ="overflow:auto; width:100%; height:90%; padding: 1px 10px 1px 10px;   ">                     
               <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                          <ContentTemplate>
                              <dx:ASPxGridView ID="gvSubproductos" runat="server" KeyFieldName="SPPMFS$ID_SUBPRODUCTO"   Theme="Office2010Blue" Width="100%"   SettingsBehavior-ConfirmDelete="true" >
                                   <Settings ShowFooter="True"/>                           
                            <SettingsLoadingPanel Text="Cargando&amp;hellip;" />
          <Settings ShowFilterRow="True" ShowGroupPanel="True" />

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
                  <SettingsPager AlwaysShowPager="True" PageSize="200" ShowSeparators="True">
                 <PageSizeItemSettings Visible="True">
                 </PageSizeItemSettings>
             </SettingsPager>
             <SettingsLoadingPanel Mode="ShowOnStatusBar" />
                                  <Columns>
                                       <dx:GridViewDataTextColumn FieldName="SPPMFS$ID_SUBPRODUCTO"  Name="SPPMFS$ID_SUBPRODUCTO"   Visible="false" VisibleIndex="0" >
                                     <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn>
                                      
                                       <dx:GridViewDataTextColumn FieldName="TIPO"  Name="TIPO"  Caption="Tipo de subproducto"  Visible="true" VisibleIndex="1" GroupIndex="0" >
                                     <CellStyle HorizontalAlign="center"></CellStyle>
                                           <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn>

                                       <dx:GridViewDataTextColumn FieldName="SPRES$TIPO"  Name="SPRES$TIPO"    Visible="false" VisibleIndex="2" >
                                     <CellStyle HorizontalAlign="center"></CellStyle>
                                           <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn>

                                        <dx:GridViewDataTextColumn FieldName="RESULTADO"  Name="RESULTADO"  Caption="RI-PGG/RE"  Visible="true" VisibleIndex="3" GroupIndex="1" >
                                     <CellStyle HorizontalAlign="justify"></CellStyle>
                                            <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn>
                                       <dx:GridViewDataTextColumn FieldName="SPPRO$PRESUPUESTO"  Name="SPPRO$PRESUPUESTO"  Caption="No. Programa presupuestario"  Visible="true" VisibleIndex="4">
                                     <CellStyle HorizontalAlign="justify"></CellStyle>
                                            <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn>

                                      <dx:GridViewDataTextColumn FieldName="PRODUCTO"  Name="PRODUCTO"  Caption="Producto"  Visible="true" VisibleIndex="5" GroupIndex="2" >
                                     <CellStyle HorizontalAlign="justify"></CellStyle>
                                            <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn>
                                      <dx:GridViewDataTextColumn FieldName="METAFISICAPROD"  Name="METAFISICAPROD"  Caption="Meta física vigente de producto"  Visible="true" VisibleIndex="6" >
                                      <PropertiesTextEdit DisplayFormatString="{0:N2}">
                                                            </PropertiesTextEdit>
                                          <CellStyle HorizontalAlign="center"></CellStyle>
                                            <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn>
                                      <dx:GridViewDataTextColumn FieldName="METAFINANCIERAPROD"  Name="METAFINANCIERAPROD"  Caption="Meta financiera vigente de producto"  Visible="true" VisibleIndex="7" >
                                      <PropertiesTextEdit DisplayFormatString="{0:N2}">
                                                            </PropertiesTextEdit>
                                          <CellStyle HorizontalAlign="center"></CellStyle>
                                            <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn>
                                       <dx:GridViewDataTextColumn FieldName="SUBPRODUCTO"  Name="SUBPRODUCTO"  Caption="Subproducto"  Visible="true" VisibleIndex="8">
                                     <CellStyle HorizontalAlign="justify"></CellStyle>
                                            <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn>
                                      <dx:GridViewDataTextColumn FieldName="MEDIDASUBPRODUCTO"  Name="MEDIDASUBPRODUCTO"  Caption="Unidad de medida subproducto"  Visible="true" VisibleIndex="9">
                                     <CellStyle HorizontalAlign="justify"></CellStyle>
                                            <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn>
                                      <dx:GridViewDataTextColumn FieldName="SPPMFS$ID_POA"  Name="SPPMFS$ID_POA"    Visible="false" VisibleIndex="10">
                                     
                                     <Settings />
                                    </dx:GridViewDataTextColumn>
                                       <dx:GridViewDataTextColumn FieldName="SPPMFS$ANIO"  Name="SPPMFS$ANIO"    Visible="false" VisibleIndex="11">                                     
                                     <Settings />
                                    </dx:GridViewDataTextColumn>                                       
                                       <dx:GridViewDataTextColumn FieldName="METAFISICAINICIAL"  Name="METAFISICAINICIAL"  Caption="Meta física de subproducto inicial"  Visible="true" VisibleIndex="12" >
                                      <PropertiesTextEdit DisplayFormatString="{0:N2}">
                                                            </PropertiesTextEdit>
                                          <CellStyle HorizontalAlign="center"></CellStyle>
                                            <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn>
                                       <dx:GridViewDataTextColumn FieldName="METAFISICAVIGENTE"  Name="METAFISICAVIGENTE"  Caption="Meta física de subproducto vigente"  Visible="true" VisibleIndex="13" >
                                      <PropertiesTextEdit DisplayFormatString="{0:N2}">
                                                            </PropertiesTextEdit>
                                          <CellStyle HorizontalAlign="center"></CellStyle>
                                            <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn>
                                      <dx:GridViewDataTextColumn FieldName="OPERACIONREALIZADAFISICA"  Name="OPERACIONREALIZADAFISICA"  Caption="Operación realizada (+ -)"  Visible="true" VisibleIndex="14" >                                      
                                          <CellStyle HorizontalAlign="center"></CellStyle>
                                            <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn>
                                     
                                      <dx:GridViewDataTextColumn FieldName="NUEVAMETAFISICA"  Name="NUEVAMETAFISICA"  Caption="Meta física de subproducto en esta reprogramación"  Visible="true" VisibleIndex="15" >                                      
                                          <PropertiesTextEdit DisplayFormatString="{0:N2}">
                                                            </PropertiesTextEdit>
                                       <CellStyle HorizontalAlign="center"></CellStyle>
                                           <HeaderStyle Wrap="True" />
                                     <Settings />                                     
                                                                                
                                    </dx:GridViewDataTextColumn>                                     
                                      

                                      <dx:GridViewDataTextColumn FieldName="METAFINANCIERAINICIAL"  Name="METAFINANCIERAINICIAL"  Caption="Meta financiera de subproducto inicial"  Visible="true" VisibleIndex="16" >
                                      <PropertiesTextEdit DisplayFormatString="{0:N2}">
                                                            </PropertiesTextEdit>
                                          <CellStyle HorizontalAlign="center"></CellStyle>
                                            <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn>
                                      <dx:GridViewDataTextColumn FieldName="METAFINANCIERAVIGENTE"  Name="METAFINANCIERAVIGENTE"  Caption="Meta financiera de subproducto vigente"  Visible="true" VisibleIndex="17" >
                                      <PropertiesTextEdit DisplayFormatString="{0:N2}">
                                                            </PropertiesTextEdit>
                                          <CellStyle HorizontalAlign="center"></CellStyle>
                                            <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn>
                                      <dx:GridViewDataTextColumn FieldName="OPERACIONREALIZADAFINANCIERA"  Name="OPERACIONREALIZADAFINANCIERA"  Caption="Operación realizada (+ -)"  Visible="true" VisibleIndex="18" >                                      
                                          <CellStyle HorizontalAlign="center"></CellStyle>
                                            <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn>
                                         
                                      <dx:GridViewDataTextColumn FieldName="NUEVAMETAFINANCIERA"  Name="NUEVAMETAFINANCIERA"  Caption="Meta financiera de subproducto en esta reprogramación"  Visible="true" VisibleIndex="19" >                                      
                                           <PropertiesTextEdit DisplayFormatString="{0:N2}">
                                                            </PropertiesTextEdit>
                                       <CellStyle HorizontalAlign="center"></CellStyle>
                                           <HeaderStyle Wrap="True" />
                                     <Settings />                             
                                    </dx:GridViewDataTextColumn>
                                   
                                      
                                      <dx:GridViewDataTextColumn FieldName="OPERACIONFISICA"  Name="OPERACIONFISICA"    Visible="false" VisibleIndex="20" >                                     
                                              
                                    </dx:GridViewDataTextColumn>
                                      <dx:GridViewDataTextColumn FieldName="CANTIDADFISICA"  Name="CANTIDADFISICA"    Visible="false" VisibleIndex="21" >                                      
                                              
                                    </dx:GridViewDataTextColumn>
                                      <dx:GridViewDataTextColumn FieldName="OPERACIONFINANCIERA"  Name="OPERACIONFINANCIERA"    Visible="false" VisibleIndex="22" >                                      
                                              
                                    </dx:GridViewDataTextColumn>
                                      <dx:GridViewDataTextColumn FieldName="CANTIDADFINANCIERA"  Name="CANTIDADFINANCIERA"    Visible="false" VisibleIndex="23" >                                      
                                              
                                    </dx:GridViewDataTextColumn>
                                       <dx:GridViewDataTextColumn FieldName="NIDMFS"  Name="NIDMFS"    Visible="false" VisibleIndex="24" >                                    
                                       </dx:GridViewDataTextColumn>
                                      <dx:GridViewDataTextColumn FieldName="IDREPROMFS"  Name="IDREPROMFS"    Visible="false" VisibleIndex="25" >                                      
                                      </dx:GridViewDataTextColumn>
                                      <dx:GridViewDataTextColumn FieldName="NIDMFIN"  Name="NIDMFIN"    Visible="false" VisibleIndex="26" >                                    
                                       </dx:GridViewDataTextColumn>
                                      <dx:GridViewDataTextColumn FieldName="IDREPROMFIN"  Name="IDREPROMFIN"    Visible="false" VisibleIndex="27" >                                      
                                      </dx:GridViewDataTextColumn>
                                  </Columns>
                                   <SettingsBehavior AllowFocusedRow="true" />  
                                  </dx:ASPxGridView>
                                  </ContentTemplate>
                                  </asp:UpdatePanel>
                                  </div>
        </asp:View>
    </asp:MultiView>

      <asp:UpdatePanel ID="upPopInstrument"  UpdateMode="Always" runat="server"  >                       
                             <ContentTemplate>
                                <dx:ASPxPopupControl ID="popProgramacion" runat="server" AllowDragging ="true" Width ="600px" Height="450px"  PopupHorizontalAlign="WindowCenter"  PopupVerticalAlign="WindowCenter"  CloseAction="CloseButton"  >
    <HeaderStyle Font-Bold="False" Font-Size="10"  HorizontalAlign="Left" />
                                    <ContentCollection>
                                    <dx:PopupControlContentControl>
                                    <div  style ="overflow:auto; width:100%; height:90%; padding: 1px 10px 1px 10px;   ">
                                         <label for="txtsubCod"aria-required="true">Campos marcados con <font color="red">*</font> son obligatorios</label>
           <div class="form-group">
                           <label for="txtsubCod"aria-required="true"><font color="red">*</font>No. De resolución: </label>
                            <asp:TextBox ID="txtResolución" PlaceHolder="No de resolución" CssClass="form-control"  runat="server"></asp:TextBox>
                    </div>

                                                                  <div  style ="overflow:auto; width:100%; height:90%; padding: 1px 10px 1px 10px;   ">
           <div class="form-group">
                           <label for="txtsubCod"aria-required="true"><font color="red">*</font>Fecha de resolución: </label>
                            <dx:ASPxDateEdit ID="Fecha_resolucion" runat="server" CssClass="form-control" Width="200px" ClientInstanceName="fecha_resolucion" DisplayFormatString="dd/MM/yyyy" EditFormatString="dd/MM/yyyy">                                   
                                                              
                                </dx:ASPxDateEdit>
                    </div>
                    <div class="form-group">
                        <asp:Label ID="archivo" Visible="false" runat="server"></asp:Label>
                        </div>

           <asp:Panel ID="panArchivo" runat="server">
                       <label style="color:red" for="txtsubCod"aria-required="true"><font color="red">*</font>Adjunte el documento con la resolución, el archivo no debe exceder de 10 MB: </label>
                      
                       <asp:FileUpload ID="fileInstrumento"   runat="server" accept=".pdf" />
                     <asp:CustomValidator ID="CustomValidator1" runat="server" ControlToValidate="fileInstrumento"
   ErrorMessage="File size should not be greater than 1 KB." OnServerValidate="CustomValidator1_ServerValidate"></asp:CustomValidator>
                       <asp:RequiredFieldValidator ID="RequiredFieldValidator4" ForeColor="Red" runat="server"
                        ValidationGroup="gr1" ControlToValidate="fileInstrumento">Solo archivos en formato PDF</asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="regval1" ForeColor="Red" runat="server" ValidationGroup="gr1"
                        ControlToValidate="fileInstrumento" ValidationExpression="^([a-zA-Z].*|[1-9].*)\.(((p|P)(d|D)(f|F)))$">Solo se puede adjuntar archivo en formato PDF </asp:RegularExpressionValidator>
                   </asp:Panel>
                                           <div class="form-group">
                           <label for="txtsubCod"aria-required="true"><font color="red">*</font>Breve descripción de la reprogramación (maximo 3,000 caracteres): </label>
                            <asp:TextBox ID="txtObservacion" PlaceHolder="Breve descripción de la reprogramación" CssClass="form-control"  TextMode="MultiLine" runat="server"></asp:TextBox>
                    </div>

                    <div>                   
                    <asp:Button ID="btnGrabaRepro" runat="server"   CssClass="btn btn-success" OnClick="btnGrabaRepro_Click"/>              
                     <asp:Button ID="btnCancelarRepro" runat="server" Text="Cancelar/Cerrar" CssClass="btn btn-warning" OnClick="btnCancelarRepro_Click" />
                </div>

           </div>
                                  
                                        </div>

                                        </dx:PopupControlContentControl>
                                        </ContentCollection>
                                    </dx:ASPxPopupControl>
      </ContentTemplate>
          
          </asp:UpdatePanel>

    <asp:UpdatePanel ID="poupVistaPrevia" runat="server"  >                
                             <ContentTemplate>
                                 <dx:ASPxPopupControl ID="popDocumentores" runat="server" AllowDragging ="true" Width ="900px" Height="400px" HeaderText="Resolución" PopupHorizontalAlign="WindowCenter"  PopupVerticalAlign="WindowCenter"  CloseAction="CloseButton"  >
                                <HeaderStyle Font-Bold="False" Font-Size="10"  HorizontalAlign="Left" />
                               <ContentCollection>
                                   <dx:PopupControlContentControl>
             <%-- <iframe id="vstPrevia" runat="server"   Width ="900" Height="400" ></iframe>--%>
            </dx:PopupControlContentControl>
                                   </ContentCollection>
                           </dx:ASPxPopupControl>

                                 </ContentTemplate>
                </asp:UpdatePanel>


       <asp:UpdatePanel ID="UpdatePanel4"  UpdateMode="Always" runat="server"  >                       
                             <ContentTemplate>
                                <dx:ASPxPopupControl ID="popMFproducto" runat="server" AllowDragging ="true" Width ="600px" Height="500px"  PopupHorizontalAlign="WindowCenter"  PopupVerticalAlign="WindowCenter"  CloseAction="CloseButton"  >
    <HeaderStyle Font-Bold="False" Font-Size="10"  HorizontalAlign="Left" />
                                    <ContentCollection>
                                    <dx:PopupControlContentControl>
                                    <div  style ="overflow:auto; width:100%; height:90%; padding: 1px 10px 1px 10px;   ">
                                        <div class="form-group"> 
                                        <label for="txtsubCod"aria-required="true">Los calculos de la nueva meta se realizan en base a la meta vigente
                                           </div>  
                                         <div class="form-group">               
               <label for="txtsubCod"aria-required="true">Producto:
                            <asp:Label ID="lblProducto" Style="color:blue" runat="server"></asp:Label>
                    </div>
                                        <div class="form-group">               
               <label for="txtsubCod"aria-required="true">Unidad de medida:
                            <asp:Label ID="lblMedidaProd"   runat="server"></asp:Label>
                    </div>
           <div class="form-group">               
               <label for="txtsubCod"aria-required="true">Meta inicial:
                            <asp:Label ID="mInicial"    runat="server"></asp:Label>
                    </div>
                           <div class="form-group">    
                              <label for="txtsubCod"aria-required="true">Meta vigente:
                            <asp:Label ID="mVigente"    runat="server"></asp:Label>
                    </div>

         <div class="form-group">
                           <label for="txtsubCod"aria-required="true">Ingrese la cantidad de aumento o disminución de la meta fisica (ingrese la diferencia): </label>
                            <asp:TextBox ID="txtdiferencia" PlaceHolder="999999" CssClass="form-control" onkeypress="return NumCheckmoney(event, this)"  onblur="limpia()" runat="server"></asp:TextBox>
                    </div>

               <div  style ="overflow:auto; width:100%; height:90%; padding: 1px 10px 1px 10px;   ">
                   <label for="txtsubCod"aria-required="true">Seleccione la operación:
           <div class ="form-group">
                        <dx:ASPxRadioButtonList ID="operacion" runat="server" ForeColor="Blue" ValueField="ID" TextField="Name2" RepeatColumns="2" RepeatLayout="Table"   Border-BorderStyle= "Double" Font-Size="Medium"   ClientInstanceName="rbApprovalCodeData" AutoPostBack="true" OnSelectedIndexChanged="resultados_SelectedIndexChanged">
                                      
                           
                                         <CaptionSettings Position="Top" />
                                        <Items>
                                            <dx:ListEditItem Text="suma" Value="0"    />
                                            <dx:ListEditItem Text="Resta" Value="1" />
                                         
                                        </Items>
                                       
                                </dx:ASPxRadioButtonList>
                        </div>

          
                                           <div class="form-group">
                           <label for="txtsubCod"aria-required="true">Meta reprogramada </label>
                            <asp:TextBox ID="txtNuevameta" PlaceHolder="9999999" CssClass="form-control"  runat="server"  Enabled="false"></asp:TextBox>
                    </div>

                    <div>                   
                    <asp:Button ID="btnmeta" runat="server"  OnClick="btnmeta_Click" OnClientClick="this.disabled=true;" UseSubmitBehavior="false"/>              
                     <asp:Button ID="btnCerrarMeta" runat="server" Text="Cancelar/Cerrar" CssClass="btn btn-warning"  OnClick="btnCerrarMeta_Click" />
                </div>

           </div>
                                  
                                        </div>

                                        </dx:PopupControlContentControl>
                                        </ContentCollection>
                                    </dx:ASPxPopupControl>
      </ContentTemplate>
          
          </asp:UpdatePanel>




    <asp:UpdatePanel ID="UpdatePanel7"  UpdateMode="Always" runat="server"  >                       
                             <ContentTemplate>
                                <dx:ASPxPopupControl ID="popMetaSub" runat="server" AllowDragging ="true" Width ="600px" Height="540px"  PopupHorizontalAlign="WindowCenter"  PopupVerticalAlign="WindowCenter"  CloseAction="CloseButton"  >
    <HeaderStyle Font-Bold="False" Font-Size="10"  HorizontalAlign="Left" />
                                    <ContentCollection>
                                    <dx:PopupControlContentControl>
                                    <div  style ="overflow:auto; width:100%; height:90%; padding: 1px 10px 1px 10px;   ">
                                         <div class="form-group"> 
                                        <label for="txtsubCod"aria-required="true">Los calculos de la nueva meta se realizan en base a la meta vigente
                                           </div>  
                      <div class="form-group">               
                               <asp:Label ID="lbltipoprogra" Style="color:red" runat="server"></asp:Label>
                    </div>
                                         <div class="form-group">               
               <label for="txtsubCod"aria-required="true">subproducto:
                            <asp:Label ID="lblSuproducto" Style="color:blue" runat="server"></asp:Label>
                    </div>
                                        <div class="form-group">               
               <label for="txtsubCod"aria-required="true">Unidad de medida:
                            <asp:Label ID="lblUnidadMedidaSub"   runat="server"></asp:Label>
                    </div>
           <div class="form-group">               
               <label for="txtsubCod"aria-required="true">Meta inicial:
                            <asp:Label ID="lblMetaInicialsub"    runat="server"></asp:Label>
                    </div>
                           <div class="form-group">    
                              <label for="txtsubCod"aria-required="true">Meta vigente:
                            <asp:Label ID="lblMetavigentesub"    runat="server"></asp:Label>
                    </div>

         <div class="form-group">
                           <label for="txtsubCod"aria-required="true">Ingrese la cantidad de aumento o disminución de la meta (ingrese la diferencia): </label>
                            <asp:TextBox ID="txtdiferenciasub" PlaceHolder="999999" CssClass="form-control" onkeypress="return isNumberKey(event)"  runat="server" onblur="limpia2()"></asp:TextBox>
                    </div>

               <div  style ="overflow:auto; width:100%; height:90%; padding: 1px 10px 1px 10px;   ">
                   <label for="txtsubCod"aria-required="true">Seleccione la operación:
           <div class ="form-group">
                        <dx:ASPxRadioButtonList ID="rbOperacion" runat="server" ForeColor="Blue" ValueField="ID" TextField="Name2" RepeatColumns="2" RepeatLayout="Table"   Border-BorderStyle= "Double" Font-Size="Medium"  ClientInstanceName="subsos"  AutoPostBack="true"  OnSelectedIndexChanged="rbOperacion_SelectedIndexChanged">
                                      
                           
                                         <CaptionSettings Position="Top" />
                                        <Items>
                                            <dx:ListEditItem Text="suma" Value="0"    />
                                            <dx:ListEditItem Text="Resta" Value="1" />
                                         
                                        </Items>
                                       
                                </dx:ASPxRadioButtonList>
                        </div>

          
                                           <div class="form-group">
                           <label for="txtsubCod"aria-required="true">Meta reprogramada </label>
                            <asp:TextBox ID="txtNuevaMetasub" PlaceHolder="9999999" CssClass="form-control"  runat="server"  Enabled="false"></asp:TextBox>
                    </div>

                    <div>                   
                    <asp:Button ID="btnGrabaMetasub" runat="server"  OnClick="btnGrabaMetasub_Click" OnClientClick="this.disabled=true;" UseSubmitBehavior="false"/>              
                     <asp:Button ID="btnCerrarSub" runat="server" Text="Cancelar/Cerrar" CssClass="btn btn-warning"   OnClick="btnCerrarSub_Click" />
                </div>

           </div>
                                  
                                        </div>

                                        </dx:PopupControlContentControl>
                                        </ContentCollection>
                                    </dx:ASPxPopupControl>
      </ContentTemplate>
          
          </asp:UpdatePanel>



    <asp:UpdatePanel ID="UpdatePanel9"  UpdateMode="Always" runat="server"  >                       
                             <ContentTemplate>
                                <dx:ASPxPopupControl ID="popVerReprogramaciones" runat="server" AllowDragging ="true" Width ="800px" Height="540px"  PopupHorizontalAlign="WindowCenter"  PopupVerticalAlign="WindowCenter"  CloseAction="CloseButton"  HeaderText="Ver productos/Suproductos reprogramados" >
    <HeaderStyle Font-Bold="False" Font-Size="10"  HorizontalAlign="Left" />
                                    <ContentCollection>
                                    <dx:PopupControlContentControl>
                                        <div>                 
                                 
                     <asp:Button ID="btnCerrarPOP" runat="server" Text="Cerrar" CssClass="btn btn-danger" OnClick="btnCerrarPOP_Click"   />
                </div>
                                    <div  style ="overflow:auto; width:100%; height:90%; padding: 1px 10px 1px 10px;   ">
                                          <h4 style="margin-left:10px"><asp:Label ID="lblResolucionres" runat="server"></asp:Label></h4>
                                            <h4 style="margin-left:10px"><asp:Label ID="lblFechares" runat="server"></asp:Label></h4>
                      <h4>Breve descripción de la reprogramación: <asp:Label ID="lblobservacion" runat="server"></asp:Label></h4>
                                        <h4>Productos</h4>
                                        <dx:ASPxGridView ID="gvVerProductos" runat="server" KeyFieldName="SPPMFS$ID_PRODUCTO"   Theme="Office2010Blue" Width="100%"   SettingsBehavior-ConfirmDelete="true" >
                                   <Settings ShowFooter="True"/>                           
                            <SettingsLoadingPanel Text="Cargando&amp;hellip;" />
          <Settings ShowFilterRow="True" ShowGroupPanel="True" />

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
                  <SettingsPager AlwaysShowPager="True" PageSize="200" ShowSeparators="True">
                 <PageSizeItemSettings Visible="True">
                 </PageSizeItemSettings>
             </SettingsPager>
             <SettingsLoadingPanel Mode="ShowOnStatusBar" />
                                 <Columns>
                                      <dx:GridViewDataTextColumn FieldName="SPRES$TIPO"  Name="SPRES$TIPO"   Visible="false" VisibleIndex="0" >
                                     <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn>
                                      <dx:GridViewDataTextColumn FieldName="SPPRO$ID_RESULTADO"  Name="SPPRO$ID_RESULTADO"   Visible="false" VisibleIndex="1" >
                                     <CellStyle HorizontalAlign="center"></CellStyle>
                                          <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn>
                                       <dx:GridViewDataTextColumn FieldName="TIPO"  Name="TIPO"  Caption="Tipo de producto"  Visible="true" VisibleIndex="2" GroupIndex="0" >
                                     <CellStyle HorizontalAlign="center"></CellStyle>
                                           <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn FieldName="NOMBRE"  Name="NOMBRE"  Caption="RI-PGG/RE"  Visible="true" VisibleIndex="3" GroupIndex="1" >
                                     <CellStyle HorizontalAlign="justify"></CellStyle>
                                            <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn>
                                      <dx:GridViewDataTextColumn FieldName="SPPMFS$ID_PROGRAMACION_FISICA"  Name="SPPMFS$ID_PROGRAMACION_FISICA"    Visible="false" VisibleIndex="4" >
                                     <CellStyle HorizontalAlign="justify"></CellStyle>
                                            <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn>
                                      
                                       <dx:GridViewDataTextColumn FieldName="SPPMFS$ID_PRODUCTO"  Name="SPPMFS$ID_PRODUCTO"  Visible="false" VisibleIndex="5" >
                                     <CellStyle HorizontalAlign="justify"></CellStyle>
                                     <Settings />
                                    </dx:GridViewDataTextColumn>
                                     
                                      <dx:GridViewDataTextColumn FieldName="SPPRO$DESCRIPCION"  Name="SPPRO$DESCRIPCION"  Caption="Producto"  Visible="true" VisibleIndex="6" >
                                     <CellStyle HorizontalAlign="justify"></CellStyle>
                                          <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn>
                                       <dx:GridViewDataTextColumn FieldName="SNCGUM$NOMBRE"  Name="SNCGUM$NOMBRE"  Caption="Unidad de medida producto"  Visible="true" VisibleIndex="7" >
                                     <CellStyle HorizontalAlign="center"></CellStyle>
                                           <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn>
                                  <dx:GridViewDataTextColumn FieldName="METAFINANCIERA"  Name="METAFINANCIERA"  Caption="Meta financiera vigente producto"  Visible="true" VisibleIndex="8" >
                                         <PropertiesTextEdit DisplayFormatString="{0:N2}">
                                                            </PropertiesTextEdit>
                                       <CellStyle HorizontalAlign="center"></CellStyle>
                                           <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn>   
                                       <dx:GridViewDataTextColumn FieldName="METAFISICAINICIAL"  Name="METAFISICAINICIAL" Caption="Meta física inicial"  Visible="true" VisibleIndex="9" >
                                         <PropertiesTextEdit DisplayFormatString="{0:N2}">
                                                            </PropertiesTextEdit>
                                       <CellStyle HorizontalAlign="center"></CellStyle>
                                           <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn> 
                                       <dx:GridViewDataTextColumn FieldName="METAVIGENTE"  Name="METAVIGENTE" Caption="Meta física vigente"  Visible="true" VisibleIndex="10" >
                                         <PropertiesTextEdit DisplayFormatString="{0:N2}">
                                                            </PropertiesTextEdit>
                                       <CellStyle HorizontalAlign="center"></CellStyle>
                                           <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn>   
                                       <dx:GridViewDataTextColumn FieldName="OPERACIONREALIZADA"  Name="OPERACIONREALIZADA"  Caption="Operación realizada (+-)"  Visible="true" VisibleIndex="11" >
                                          <CellStyle HorizontalAlign="center"></CellStyle>
                                           <HeaderStyle Wrap="True" />
                                           
                                     <Settings />
                                    </dx:GridViewDataTextColumn>   

                                      <dx:GridViewDataTextColumn FieldName="REPROFISICA"  Name="REPROFISICA" Caption="Meta física para esta reprogramación"  Visible="true" VisibleIndex="12" >
                                         <PropertiesTextEdit DisplayFormatString="{0:N2}">
                                                            </PropertiesTextEdit>
                                       <CellStyle HorizontalAlign="center"></CellStyle>
                                           <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn>  
                                                                  

                                    <dx:GridViewDataTextColumn FieldName="NIDMF"  Name="NIDMF"   Visible="false" VisibleIndex="13" >
                                         
                                           <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn>   

                                      <dx:GridViewDataTextColumn FieldName="IDREPROGRA"  Name="IDREPROGRA"   Visible="false" VisibleIndex="14" >
                                         
                                           <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn>   
                                     
                                      <dx:GridViewDataTextColumn FieldName="OPERACION"  Name="OPERACION"   Visible="false" VisibleIndex="15" >
                                         
                                           <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn> 
                                      <dx:GridViewDataTextColumn FieldName="CANTIDAD"  Name="CANTIDAD"   Visible="false" VisibleIndex="16" >
                                         
                                           <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn>
                                      

                                      <dx:GridViewDataTextColumn FieldName="NIDMFIN"  Name="NIDMFIN"   Visible="false" VisibleIndex="17" >
                                         
                                           <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn>   

                                      <dx:GridViewDataTextColumn FieldName="REPROFINANCIERA"  Name="REPROFINANCIERA"   Visible="false" VisibleIndex="18" >
                                         
                                           <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn>   
                                     
                                      <dx:GridViewDataTextColumn FieldName="OPERACIONFINANCEIRA"  Name="OPERACIONFINANCEIRA"  Caption="Operación realizada (+-)"  Visible="true" VisibleIndex="19" >
                                         
                                           <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn> 
                                      <dx:GridViewDataTextColumn FieldName="CANTIDADFINANCIERA"  Name="CANTIDADFINANCIERA"   Visible="false" VisibleIndex="20" >
                                         
                                           <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn>

                                      <dx:GridViewDataTextColumn FieldName="NUEVAMETAFINANCIERA"  Name="NUEVAMETAFINANCIERA"   Caption="Meta financiera para esta reprogramación" Visible="true" VisibleIndex="21" >
                                         
                                           <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn>
                                      <dx:GridViewDataTextColumn FieldName="OPERAFINANCIERA"  Name="OPERAFINANCIERA"    Visible="false" VisibleIndex="22" >
                                         
                                           <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn>
                                  </Columns>
                                  <SettingsBehavior AllowFocusedRow="true" />  
                                  </dx:ASPxGridView>
                                         
                                        <h4>Subproductos</h4>
           <dx:ASPxGridView ID="gvVerSubproductos" runat="server" KeyFieldName="SPPMFS$ID_SUBPRODUCTO"   Theme="Office2010Blue" Width="100%"   SettingsBehavior-ConfirmDelete="true" >
                                   <Settings ShowFooter="True"/>                           
                            <SettingsLoadingPanel Text="Cargando&amp;hellip;" />
          <Settings ShowFilterRow="True" ShowGroupPanel="True" />

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
                  <SettingsPager AlwaysShowPager="True" PageSize="200" ShowSeparators="True">
                 <PageSizeItemSettings Visible="True">
                 </PageSizeItemSettings>
             </SettingsPager>
             <SettingsLoadingPanel Mode="ShowOnStatusBar" />
                                  <Columns>
                                       <dx:GridViewDataTextColumn FieldName="SPPMFS$ID_SUBPRODUCTO"  Name="SPPMFS$ID_SUBPRODUCTO"   Visible="false" VisibleIndex="0" >
                                     <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn>
                                      
                                       <dx:GridViewDataTextColumn FieldName="TIPO"  Name="TIPO"  Caption="Tipo de subproducto"  Visible="true" VisibleIndex="1" GroupIndex="0" >
                                     <CellStyle HorizontalAlign="center"></CellStyle>
                                           <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn>

                                       <dx:GridViewDataTextColumn FieldName="SPRES$TIPO"  Name="SPRES$TIPO"    Visible="false" VisibleIndex="2" >
                                     <CellStyle HorizontalAlign="center"></CellStyle>
                                           <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn>

                                        <dx:GridViewDataTextColumn FieldName="RESULTADO"  Name="RESULTADO"  Caption="RI-PGG/RE"  Visible="true" VisibleIndex="3" >
                                     <CellStyle HorizontalAlign="justify"></CellStyle>
                                            <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn>
                                      <dx:GridViewDataTextColumn FieldName="PRODUCTO"  Name="PRODUCTO"  Caption="Producto"  Visible="true" VisibleIndex="4" GroupIndex="2" >
                                     <CellStyle HorizontalAlign="justify"></CellStyle>
                                            <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn>
                                      <dx:GridViewDataTextColumn FieldName="METAFISICAPROD"  Name="METAFISICAPROD"  Caption="Meta física vigente de producto"  Visible="true" VisibleIndex="5" >
                                      <PropertiesTextEdit DisplayFormatString="{0:N2}">
                                                            </PropertiesTextEdit>
                                          <CellStyle HorizontalAlign="center"></CellStyle>
                                            <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn>
                                      <dx:GridViewDataTextColumn FieldName="METAFINANCIERAPROD"  Name="METAFINANCIERAPROD"  Caption="Meta financiera vigente de producto"  Visible="true" VisibleIndex="6" >
                                      <PropertiesTextEdit DisplayFormatString="{0:N2}">
                                                            </PropertiesTextEdit>
                                          <CellStyle HorizontalAlign="center"></CellStyle>
                                            <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn>
                                       <dx:GridViewDataTextColumn FieldName="SUBPRODUCTO"  Name="SUBPRODUCTO"  Caption="Subproducto"  Visible="true" VisibleIndex="7">
                                     <CellStyle HorizontalAlign="justify"></CellStyle>
                                            <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn>
                                      <dx:GridViewDataTextColumn FieldName="MEDIDASUBPRODUCTO"  Name="MEDIDASUBPRODUCTO"  Caption="Unidad de medida subproducto"  Visible="true" VisibleIndex="8">
                                     <CellStyle HorizontalAlign="justify"></CellStyle>
                                            <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn>
                                      <dx:GridViewDataTextColumn FieldName="SPPMFS$ID_POA"  Name="SPPMFS$ID_POA"    Visible="false" VisibleIndex="9">
                                     
                                     <Settings />
                                    </dx:GridViewDataTextColumn>
                                       <dx:GridViewDataTextColumn FieldName="SPPMFS$ANIO"  Name="SPPMFS$ANIO"    Visible="false" VisibleIndex="10">                                     
                                     <Settings />
                                    </dx:GridViewDataTextColumn>                                       
                                       <dx:GridViewDataTextColumn FieldName="METAFISICAINICIAL"  Name="METAFISICAINICIAL"  Caption="Meta física de subproducto inicial"  Visible="true" VisibleIndex="11" >
                                      <PropertiesTextEdit DisplayFormatString="{0:N2}">
                                                            </PropertiesTextEdit>
                                          <CellStyle HorizontalAlign="center"></CellStyle>
                                            <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn>
                                       <dx:GridViewDataTextColumn FieldName="METAFISICAVIGENTE"  Name="METAFISICAVIGENTE"  Caption="Meta física de subproducto vigente"  Visible="true" VisibleIndex="12" >
                                      <PropertiesTextEdit DisplayFormatString="{0:N2}">
                                                            </PropertiesTextEdit>
                                          <CellStyle HorizontalAlign="center"></CellStyle>
                                            <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn>
                                      <dx:GridViewDataTextColumn FieldName="OPERACIONREALIZADAFISICA"  Name="OPERACIONREALIZADAFISICA"  Caption="Operación realizada (+ -)"  Visible="true" VisibleIndex="13" >                                      
                                          <CellStyle HorizontalAlign="center"></CellStyle>
                                            <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn>
                                     
                                      <dx:GridViewDataTextColumn FieldName="NUEVAMETAFISICA"  Name="NUEVAMETAFISICA"  Caption="Meta física de subproducto en esta reprogramación"  Visible="true" VisibleIndex="14" >                                      
                                          <PropertiesTextEdit DisplayFormatString="{0:N2}">
                                                            </PropertiesTextEdit>
                                       <CellStyle HorizontalAlign="center"></CellStyle>
                                           <HeaderStyle Wrap="True" />
                                     <Settings />                                     
                                                                                
                                    </dx:GridViewDataTextColumn>                                     
                                      

                                      <dx:GridViewDataTextColumn FieldName="METAFINANCIERAINICIAL"  Name="METAFINANCIERAINICIAL"  Caption="Meta financiera de subproducto inicial"  Visible="true" VisibleIndex="15" >
                                      <PropertiesTextEdit DisplayFormatString="{0:N2}">
                                                            </PropertiesTextEdit>
                                          <CellStyle HorizontalAlign="center"></CellStyle>
                                            <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn>
                                      <dx:GridViewDataTextColumn FieldName="METAFINANCIERAVIGENTE"  Name="METAFINANCIERAVIGENTE"  Caption="Meta financiera de subproducto vigente"  Visible="true" VisibleIndex="16" >
                                      <PropertiesTextEdit DisplayFormatString="{0:N2}">
                                                            </PropertiesTextEdit>
                                          <CellStyle HorizontalAlign="center"></CellStyle>
                                            <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn>
                                      <dx:GridViewDataTextColumn FieldName="OPERACIONREALIZADAFINANCIERA"  Name="OPERACIONREALIZADAFINANCIERA"  Caption="Operación realizada (+ -)"  Visible="true" VisibleIndex="17" >                                      
                                          <CellStyle HorizontalAlign="center"></CellStyle>
                                            <HeaderStyle Wrap="True" />
                                     <Settings />
                                    </dx:GridViewDataTextColumn>
                                         
                                      <dx:GridViewDataTextColumn FieldName="NUEVAMETAFINANCIERA"  Name="NUEVAMETAFINANCIERA"  Caption="Meta financiera de subproducto en esta reprogramación"  Visible="true" VisibleIndex="18" >                                      
                                           <PropertiesTextEdit DisplayFormatString="{0:N2}">
                                                            </PropertiesTextEdit>
                                       <CellStyle HorizontalAlign="center"></CellStyle>
                                           <HeaderStyle Wrap="True" />
                                     <Settings />                             
                                    </dx:GridViewDataTextColumn>
                                   
                                      
                                      <dx:GridViewDataTextColumn FieldName="OPERACIONFISICA"  Name="OPERACIONFISICA"    Visible="false" VisibleIndex="19" >                                     
                                              
                                    </dx:GridViewDataTextColumn>
                                      <dx:GridViewDataTextColumn FieldName="CANTIDADFISICA"  Name="CANTIDADFISICA"    Visible="false" VisibleIndex="20" >                                      
                                              
                                    </dx:GridViewDataTextColumn>
                                      <dx:GridViewDataTextColumn FieldName="OPERACIONFINANCIERA"  Name="OPERACIONFINANCIERA"    Visible="false" VisibleIndex="21" >                                      
                                              
                                    </dx:GridViewDataTextColumn>
                                      <dx:GridViewDataTextColumn FieldName="CANTIDADFINANCIERA"  Name="CANTIDADFINANCIERA"    Visible="false" VisibleIndex="22" >                                      
                                              
                                    </dx:GridViewDataTextColumn>
                                       <dx:GridViewDataTextColumn FieldName="NIDMFS"  Name="NIDMFS"    Visible="false" VisibleIndex="23" >                                    
                                       </dx:GridViewDataTextColumn>
                                      <dx:GridViewDataTextColumn FieldName="IDREPROMFS"  Name="IDREPROMFS"    Visible="false" VisibleIndex="24" >                                      
                                      </dx:GridViewDataTextColumn>
                                      <dx:GridViewDataTextColumn FieldName="NIDMFIN"  Name="NIDMFIN"    Visible="false" VisibleIndex="25" >                                    
                                       </dx:GridViewDataTextColumn>
                                      <dx:GridViewDataTextColumn FieldName="IDREPROMFIN"  Name="IDREPROMFIN"    Visible="false" VisibleIndex="26" >                                      
                                      </dx:GridViewDataTextColumn>
                                  </Columns>
                                   <SettingsBehavior AllowFocusedRow="true" />  
                                  </dx:ASPxGridView>
                           

         

             
                                  
                                        </div>

                                        </dx:PopupControlContentControl>
                                        </ContentCollection>
                                    </dx:ASPxPopupControl>
      </ContentTemplate>
          
          </asp:UpdatePanel>

 </asp:Content>
