<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Error.aspx.cs" Inherits="SIPLAN2._0.error.Error" %>
<%@ Register Assembly="DevExpress.Web.v17.2, Version=17.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:LinkButton ID="BackLinkButton" runat="server" Text="Back to Example" PostBackUrl="~/poa/poa.aspx"></asp:LinkButton><br />
    <br />
    Error log:
    <dx:ASPxMemo ID="Memo" runat="server" Height="500px" Width="100%">
    
    </dx:ASPxMemo>
    <asp:LinkButton ID="ClearLinkButton" runat="server" Text="Clear" OnClick="ClearLinkButton_Click"></asp:LinkButton>
</asp:Content>
