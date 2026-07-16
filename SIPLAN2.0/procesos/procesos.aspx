<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="procesos.aspx.cs" Inherits="SIPLAN2._0.procesos.procesos"  MasterPageFile="~/Site.Master"%>
<%@ Register Assembly="DevExpress.Web.v17.2, Version=17.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
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

#contenedor3 {
  width: 100%;
}

#update3 {
  display: inline-block;
  margin-left:25px;
}
   </style>
    <h3 style="color:blue;margin-left:10px">Vinculacion de metas fisicas y financieras SNIP, ejecucion SNIP-SIPLAN<asp:Label ID="lblTitulo" runat="server"></asp:Label></h3>
    <div class="form-group" style="margin-left:10px">
                           <label for="txtsubCod"aria-required="true"><font color="red">*</font>Seleccione el periodo del POM correspondiente: </label>
                           <dx:ASPxComboBox ID="cbPeriodos" runat="server" ValueType="System.String" NullText="Seleccione periodo de POM" CssClass="form-control" AutoPostBack="true" Width="300"></dx:ASPxComboBox>
     </div> 
     <div class="form-group" style="margin-left:10px">
                           <label for="txtsubCod"aria-required="true">Año: </label>
                           <dx:ASPxComboBox ID="cbanio" runat="server" ValueType="System.String" NullText="Seleccione año a vincular" CssClass="form-control" Width="300"  AutoPostBack="true"></dx:ASPxComboBox>
                    </div> 
     <div id="contenedor" class="row">
    <div id="update" >
          <asp:Button ID="btnMetas" runat="server" Text="Metas físicas y financieras"  CssClass="btn btn-primary" ToolTip="Ver metas físicas y financieras"  Style="font-size:0.7em" OnClick="btnMetas_Click"/>
                  <asp:Button ID="btnVista2" runat="server" Text="Avances fisico y financiero"  CssClass="btn btn-success" ToolTip="Ver avances físicos y financieros"  OnClick="btnVista2_Click" Style="font-size:0.7em"/>
        </div>
         </div>
    <asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex="0">
       <asp:View ID="View1" runat="server">
           
           <br />
           <div id="contenedor2" class="row">
    <div id="update2" >
          <asp:Button ID="btnMetasfyf" runat="server" Text="Actualizar metas"  CssClass="btn btn-primary" ToolTip="Actualizar metas físicas y financieras"  Style="font-size:0.7em" OnClick="btnMetasfyf_Click" />
                 
        </div>
         </div>
           <br />
        <dx:ASPxGridView ID="gvmetas" runat="server" KeyFieldName="SPPSUB$ID_SUBPRODUCTO"   Theme="Office2010Blue" Width="100%" >
                  <Settings ShowFilterRow="True" ShowGroupPanel="True" />
            
        </dx:ASPxGridView>   
        
        </asp:View>
        <asp:View ID="View2" runat="server">
              <div class="form-group" style="margin-left:10px">
                           <label for="txtsubCod"aria-required="true">Año: </label>
                            <dx:ASPxComboBox ID="cbMes" runat="server" NullText="Seleccione el tipo productos que necesite desplegar"  AutoPostBack="True" SelectedIndex="0" Theme="Office2010Blue" CssClass="form-control" Width="400"  OnSelectedIndexChanged="cbMes_SelectedIndexChanged">
                           
                             <Items>
                                 <dx:ListEditItem Selected="True" Text="ENERO" Value="1" />
                                 <dx:ListEditItem Text="FEBRERO" Value="2" />
                                  <dx:ListEditItem Text="MARZO" Value="3" />
                                  <dx:ListEditItem Text="ABRIL" Value="4" />
                                  <dx:ListEditItem Text="MAYO" Value="5" />
                                  <dx:ListEditItem Text="JUNIO" Value="6" />
                                  <dx:ListEditItem Text="JULIO" Value="7" />
                                  <dx:ListEditItem Text="AGOSTO" Value="8" />
                                  <dx:ListEditItem Text="SEPTIEMBRE" Value="9" />
                                  <dx:ListEditItem Text="OCTUBRE" Value="10" />
                                  <dx:ListEditItem Text="NOVIEMBRE" Value="11" />
                                 <dx:ListEditItem Text="DICIEMBRE" Value="12" />
                             </Items>
                           
                         </dx:ASPxComboBox>
                    </div> 
            <div id="contenedor3" class="row">
    <div id="update3" >
          <asp:Button ID="btnAvances" runat="server" Text="Actualizar avances"  CssClass="btn btn-primary" ToolTip="Actualizar avances de metas fisicas y financieras"  Style="font-size:0.7em"  OnClick="btnAvances_Click"/>
                 
        </div>
         </div>
           <br />
        <dx:ASPxGridView ID="gvAvances" runat="server" KeyFieldName="SPPSUB$ID_SUBPRODUCTO"   Theme="Office2010Blue" Width="100%" >
                 <Settings ShowFilterRow="True" ShowGroupPanel="True" />
        </dx:ASPxGridView>   

        </asp:View>
    </asp:MultiView>

</asp:Content>