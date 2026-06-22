<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmModulos.aspx.cs" Inherits="SIPLAN2._0.modulos.frmModulos"  MasterPageFile="~/Site.Master" %>
<%@ Register Assembly="DevExpress.Web.v17.2, Version=17.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">  
    <style>        
        *{
            font-family:Arial,'Century Gothic';
        }
        /*img:hover{
            opacity:0.5;
            cursor:pointer;           
        }*/
        
        A:link { color: darkblue; font-size: 10px; font-family: arial; text-decoration: none }
        A:hover { color: darkblue; font-size: 10px; font-family: arial; text-decoration: none }
        A:visited { color: darkblue; font-size: 10px; font-family: arial; text-decoration: none }

        .popupImage {
    width: 100%;
    height: auto;
    display: block;
    max-width: 100%;
    max-height: 100%;
}

    </style>

    <script>
        function Alerta(mensaje, tipo) {
            alertify.set('notifier', 'position', 'top-right');
            if (tipo == 1)
                alertify.success(mensaje);
            if (tipo == 2)
                alertify.error(mensaje);
            if (tipo == 3)
                alertify.confirm('Sistemas SEGEPLAN', mensaje, function () { window.location.href = "../login/reset"; }, 3);

        };

        function select(x) {
            x.style.opacity = "0.5";
            x.style.cursor = "pointer";
            if (x.id == "img1") {
                document.getElementById('desc1').style.display = 'block'
                document.getElementById('desc2').style.display = 'none'
                document.getElementById('desc3').style.display = 'none'
                document.getElementById('desc4').style.display = 'none'
                document.getElementById('desc5').style.display = 'none'




            }
            else if (x.id == "img2") {
                document.getElementById('desc1').style.display = 'none'
                document.getElementById('desc2').style.display = 'block'
                document.getElementById('desc3').style.display = 'none'
                document.getElementById('desc4').style.display = 'none'
                document.getElementById('desc5').style.display = 'none'
            }

            else if (x.id == "img3") {
                document.getElementById('desc1').style.display = 'none'
                document.getElementById('desc2').style.display = 'none'
                document.getElementById('desc3').style.display = 'block'
                document.getElementById('desc4').style.display = 'none'
                document.getElementById('desc5').style.display = 'none'
            }

            else if (x.id == "img4") {
                document.getElementById('desc1').style.display = 'none'
                document.getElementById('desc2').style.display = 'none'
                document.getElementById('desc3').style.display = 'none'
                document.getElementById('desc4').style.display = 'block'
                document.getElementById('desc5').style.display = 'none'
            }

            else if (x.id == "img5") {
                document.getElementById('desc1').style.display = 'none'
                document.getElementById('desc2').style.display = 'none'
                document.getElementById('desc3').style.display = 'none'
                document.getElementById('desc4').style.display = 'none'
                document.getElementById('desc5').style.display = 'block'
            }
        }

        function deselect(x) {
            x.style.opacity = "1";

        }

    </script>   
   
        <div class="row" style="background-color:#39a9dc">
            <div class="col-sm-2">
                <div style="text-align:center;margin-top:20px;margin-left:50px">
                <a href="https://segeplangt-my.sharepoint.com/personal/byron_marroquin_segeplan_gob_gt/_layouts/15/stream.aspx?id=%2Fpersonal%2Fbyron%5Fmarroquin%5Fsegeplan%5Fgob%5Fgt%2FDocuments%2FDatos%20adjuntos%2FTutorial%20%2D%20M%C3%B3dulo%20de%20Reprogramaci%C3%B3n%20de%20metas%20f%C3%ADsicas%20y%20financieras%20%2D%20SIPLAN%2Emp4&ct=1727736697491&or=OWA%2DNT%2DMail&cid=713c8638%2D1b27%2Db5ff%2D6e46%2D07c90abcf9fe&ga=1&referrer=StreamWebApp%2EWeb&referrerScenario=AddressBarCopied%2Eview%2Ebe89e837%2D6e77%2D49d0%2Daf1e%2D9ff19907391b" target="_blank"><img src="../images/mensaje.png"  width="80%"/></a>
                    </div>
            </div>


            <div class="col-sm-10">
               <div style="text-align:center">
                <a href="https://segeplangt-my.sharepoint.com/personal/byron_marroquin_segeplan_gob_gt/_layouts/15/stream.aspx?id=%2Fpersonal%2Fbyron%5Fmarroquin%5Fsegeplan%5Fgob%5Fgt%2FDocuments%2FDatos%20adjuntos%2FTutorial%20%2D%20M%C3%B3dulo%20de%20Reprogramaci%C3%B3n%20de%20metas%20f%C3%ADsicas%20y%20financieras%20%2D%20SIPLAN%2Emp4&ct=1727736697491&or=OWA%2DNT%2DMail&cid=713c8638%2D1b27%2Db5ff%2D6e46%2D07c90abcf9fe&ga=1&referrer=StreamWebApp%2EWeb&referrerScenario=AddressBarCopied%2Eview%2Ebe89e837%2D6e77%2D49d0%2Daf1e%2D9ff19907391b" target="_blank"><h3 style="color:white"><b>INFORMACIÓN IMPORTANTE: Se encuentra disponible el videotutorial para el registro y gestión de las reprogramaciones de metas fisicas y financieras de productos y subproductos, puede acceder al video presionando este enlace</b></h3></a>
                   </div>
                <div style="text-align:justify;margin-right:30px">
                    <p style="font-weight:bold;font-size:16px;color:darkblue"></p>
                </div>
            </div>

        </div>
   
    
   
    
    <div style="display:flex; height:80vh; width:100%; border:solid 5px #023a5f; border-top:none; margin-bottom:-40px;">
        
        <div style="display:flex; width:50%;" runat="server" id="panel1">
            
            <div style="padding:20px;" Align="center">
                <a href="../pom/pom">
                    <img id="img1" onmouseover="select(this)" onmouseout="deselect(this)" src="../images/formulacion.png" width="150" height="150"/><br />
                    <span><b>Formulación y programación multianual</b></span>
                </a>
            </div>
            <div style="padding:20px;" Align="center">
                <a href="../ejecucion/frmEjecucionPOA">
                    <img id="img2" onmouseover="select(this)" onmouseout="deselect(this)" src="../images/ejecucion.png" width="150" /><br />
                    <span><b>Ejecución POA</b></span>
                </a>
            </div>          
            
            <div style="padding:20px;" Align="center">
                <a href="../reprogramacion/reprogramacion">
                    <img id="img3" onmouseover="select(this)" onmouseout="deselect(this)" src="../images/reprogramaciones12.png" width="150"  height="150"/><br />
                    <span><b>Reprogramación de metas físicas y financieras</b></span>
                </a>
            </div>    
            <div style="padding:20px;display:none" Align="center" runat="server" id="reportes">
                <a href="../reportes/reportes">
                    <img id="img4" onmouseover="select(this)" onmouseout="deselect(this)" src="../images/reporte-annual.png" width="150"  height="150"/><br />
                    <span><b>Generar reportes</b></span>
                </a>
            </div>      
           
          
           
                 
            
          
        </div>
      
        <div style="display:initial; width:50%;" runat="server" id="panel2">
             <div style="padding:20px;display:none" Align="center" runat="server" id="procesos">
                <a href="../procesos/procesos">
                    <img id="img5" onmouseover="select(this)" onmouseout="deselect(this)" src="../images/SINIP.png" width="150"  height="150"/><br />
                    <span><b>Metas SNIP a SIPLAN</b></span>
                </a>
            </div>      
        </div>

        <div style="width:35%; border-left:solid 5px #023a5f;">
            <div id="desc1" style="border:none; font-weight:700; display:none;">
                <br />
                <p>
                Módulo que permite lo siguiente:<br />
                </p>
                
                - Registrar Programación Multianual del periodo vigente<br />
                - Registro y publicación de documentos, con los instrumentos de programación multianual<br />
                - Registo de Resultados Institucionales, Producción vinculada a Metas PGG<br />
                - Registro de Programas Presupuestarios<br />
                - Registro de Productos y subproductos estratégicos e institucionales<br />
                - Asignación de presupuesto multianual para Programas Presupuestarios<br />
                - Programación de metas multianuales, fisicas y financieras para productos/subproductos estratégicos e institucionales<br />
                - Programación de metas fisicas y finacieras mensuales para productos/subproductos para el POA vigente en el  año actual
                
            </div>
            <div id="desc2" style="border:none; font-weight:700; display:none;">
                <br />
                Módulo que permite lo siguiente:<br />
                - Registrar la ejecución de los productos/subproductos para metas fisicas y financieras, programadas para el POA vigente <br />
                <%--Módulo en construcción--%>
            </div>       
            
            <div id="desc3" style="border:none; font-weight:700; display:none;">
                <br />
                Módulo que permite lo siguiente:<br />
                -Adjuntar documento de resolución de reprogramación de metas físicas y financieras <br />
                -Reprogramar metas físicas de productos <br />
                -Reprogramar metas físicas y financieras de subproductos <br />
                <%--Módulo en construcción--%>
            </div>       
            <div id="desc4" style="border:none; font-weight:700; display:none;">
                <br />
                Módulo que permite lo siguiente:<br />
                -Generar reportes del sistema (Solo disponible para especialistas de SEGEPLAN y administradores del sistema) <br />
                
                <%--Módulo en construcción--%>
            </div> 
            
               <div id="desc5" style="border:none; font-weight:700; display:none;">
                <br />
                Módulo que permite lo siguiente:<br />
                -Migrar metas y avances financieros de proyectos SNIP a subproductos vinculados, solo disponbile para personal de la Dirección de Sistemas de Información de SEGEPLAN con perfil de administrador <br />
                
                <%--Módulo en construcción--%>
            </div>       

        </div>
        <div>
              
            <asp:UpdatePanel ID="poupVistaPrevia" runat="server"  >                
                             <ContentTemplate>

                                      <dx:ASPxPopupControl ID="popDocumento" runat="server" AllowDragging ="true" Width ="900px" Height="400px"  PopupHorizontalAlign="WindowCenter"  PopupVerticalAlign="WindowCenter"  CloseAction="CloseButton"  >
                                <HeaderStyle Font-Bold="False" Font-Size="10"  HorizontalAlign="Left" />
                               <ContentCollection>
                                   <dx:PopupControlContentControl>
                          <asp:Image ID="imgPopup" runat="server" CssClass="popupImage" />
                                   </dx:PopupControlContentControl>
                                   </ContentCollection>
                           </dx:ASPxPopupControl>
                                 </ContentTemplate>
                </asp:UpdatePanel>
                 
        </div>
    </div>

</asp:Content>
