<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="reincia.aspx.cs" Inherits="SIPLAN2._0.login.reincia" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <script>    
        function Alerta(mensaje, tipo) {          
            alertify.set('notifier', 'position', 'top-right'); 
            if (tipo == 1)
                alertify.success(mensaje);
            if (tipo == 2)
                alertify.error(mensaje);          
        };  

         /*function validapass(e) {
            key = e.keyCode || e.which;
            tecla = String.fromCharCode(key).toString();
            //Se define todo lo que se quiere que se muestre
            caracter = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            
            especiales = [8, 37, 39, 46, 6];

            tecla_especial = false;
            for (var i in especiales) {
                if (key == especiales[i]) {
                    tecla_especial = false;
                    break;
                }
            }
            if (caracter.indexOf(tecla) == -1 && !tecla_especial) {
                alert('Tecla no aceptada');
                return false;
            }
                       
        }*/

        function validapass(e) {
            var key = e.key;
            var regex = /^[a-zA-Z0-9+\-_!]$/;

            if (!regex.test(key) && !["Backspace", "ArrowLeft", "ArrowRight", "Delete"].includes(key)) {
                alert('Tecla no aceptada');
                e.preventDefault();
            }
        }

    //    $('#mostrar_contrasena').click(function () {
    //if ($('#mostrar_contrasena').is(':checked')) {
    //    //$('#MainContent_txtContrasena').attr('type', 'text');
   
    //} else {
    //  $('#MainContent_txtContrasena').attr('type', 'password');
    //}
    //    });

        function mostrar()
        {
            var checkBox = document.getElementById("mostrar_contrasena");
            if (checkBox.checked == true){
                $('#MainContent_txtContrasena').attr('type', 'text');
                $('#MainContent_txtConfirme').attr('type', 'text');
            }
            else
            {
                $('#MainContent_txtContrasena').attr('type', 'password');
                $('#MainContent_txtConfirme').attr('type', 'password');
            }

        }
    </script>
    <div style="float:right">
         <div  style ="overflow:auto; width:100%; height:90%; padding: 1px 10px 1px 10px;   ">
    <h2> Cambio de credenciales</h2>
          
          
          <div style="float:right">
          <h4 class="text-danger">         ATENCIÓN:
                   Luego de grabar el cambio de contraseña, el sistema
                   pedirá de nuevo la conexión con la nueva contraseña.
                   Por favor atienda la siguentes recomendaciones para una contraseña segura.
                   El sistema reconocerá entre mayúsculas y minúsculas.
          </h4>
<h4 class="text-danger">La contraseña debe tener una longitud de entre 8 y 12 caracteres</h4>
<h4 class="text-danger">Debe tener al menos una letra mayuscula</h4>
              <h4 class="text-danger">Debe tener letras minusculas</h4>
              <h4 class="text-danger">Debe tener al menos uno de estos caracteres especiales: + - _ !</h4>
              <h4 class="text-danger">La contraseña no debe contener el mismo nombre del usuario</h4>
              </div>
    <div class="form-group">
                  <label for="lblUseroi">Usuario</label>:<asp:label ID="lblUsseroi" runat="server" Font-Bold="true"></asp:label>
                      <br />
                      <label for="txtproducto">Nombre usuario</label>: <asp:label ID="lblNombrecompleto" runat="server"></asp:label>
                      <br />
                      <label for="txtproducto">Dependencia</label>: <asp:label ID="lblDependencia" runat="server"></asp:label>
                      </div>
             
                <div class="form-group">
                  <label for="txtproducto">Nueva contraseña</label>
                     <asp:UpdatePanel ID="UpdatePanel3" runat="server">
               <ContentTemplate>
                    <asp:TextBox ID="txtContrasena" runat="server" CssClass="form-control"  PlaceHolder="Nueva contraseña" Width="400"  TextMode="Password" minlength="6" onkeypress="return validapass(event);" onpaste="alert('En este formulario no puede copiar y pegar');return false"></asp:TextBox>
                   </ContentTemplate>
                         </asp:UpdatePanel>
                   
                 
                 
                </div>
    <div>
        <label for="password"></label>
        <!-- checkbox que nos permite activar o desactivar la opcion -->
        <div style="margin-top:-10px">
          <input style="margin-left:450px;" type="checkbox" id="mostrar_contrasena" title="clic para mostrar contraseña" onclick=" mostrar()"/>
          &nbsp;&nbsp;Mostrar contraseña</div>
      </div>

    <div class="form-group">
                  <label for="txt">Confirme la nueva contraseña</label>
                  <asp:UpdatePanel ID="UpdatePanel4" runat="server">
               <ContentTemplate>  
         <asp:TextBox ID="txtConfirme" runat="server" CssClass="form-control"  PlaceHolder="Nueva contraseña"  Width="400" TextMode="Password" minlength="6" onkeypress="return validapass(event);" onpaste="alert('En este formulario no puede copiar y pegar');return false"></asp:TextBox>

    </ContentTemplate>
                      </asp:UpdatePanel>
                               
     </div>
               
     <asp:UpdatePanel ID="UpdatePanel5" runat="server">
               <ContentTemplate>  
    <asp:Button ID="btnContrasena"   runat="server" cssclass="btn btn-primary" Text="Grabar contraseña"   OnClick="btnContrasena_Click" />
                   <asp:Button ID="btnCancela"   runat="server" cssclass="btn btn-warning" Text="Cancelar"  OnClick="btnCancela_Click" />
</ContentTemplate>
           </asp:UpdatePanel>
             </div>
        </div>
</asp:Content>

