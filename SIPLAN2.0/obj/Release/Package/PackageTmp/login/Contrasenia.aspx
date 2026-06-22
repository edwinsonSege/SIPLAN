
<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Contrasenia.aspx.cs" Inherits="SIPLAN2._0.login.Contrasenia" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <script>    
        function Alerta(mensaje, tipo) {          
            alertify.set('notifier', 'position', 'top-right'); 
            if (tipo == 1)
                alertify.success(mensaje);
            if (tipo == 2)
                alertify.error(mensaje);          
        };  

         function validapass(e) {
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
                       
        }

    //    $('#mostrar_contrasena').click(function () {
    //if ($('#mostrar_contrasena').is(':checked')) {
    //    //$('#MainContent_txtContrasena').attr('type', 'text');
    //    alert("La nalgas ricas Divas");
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
    <h2>Solicitud de cambio de contraseña</h2>
          
          
          <div style="float:right">
          <h4 class="text-danger">         ATENCIÓN:
                   Se le asignará una contraseña temporal, esta clave se enviará al correo electrónico que se registró en su solicitud de usuario.  
                   <br />Una vez recibida la contraseña temporal ingrese de nuevo al sistema, se le redigira a la pantalla para que pueda cambiar su contraseña oficial.<br />
                   <b>Esta contraseña tiene una temporabilidad de 10 minutos</b>, luego de transcurrido este tiempo y si no pudo realizar el cambio de su contraseña oficial, ingrese una nueva solicitud de cambio de contraseña.
                   <br /><br />En caso de no recibir el correo con la clave temporal en los proximos 5 minutos, favor de dirigir su solicitud a <b>soporte_tecnico@segeplan.gob.gt</b>                    
                   <br />Cuando ingrese la contraseña temporal en la pantalla de ingreso, el sistema reconocerá entre mayúsculas y minúsculas.
                   <br />Al terminar de generar su contraseña temporal, se le redirigirá a la pantalla de ingreso.
          </h4>
              </div>
  
             
                <div class="form-group">
                  <label for="txtproducto">Ingrese cuenta de usuario</label>
                     <asp:UpdatePanel ID="UpdatePanel3" runat="server">
               <ContentTemplate>
                    <asp:TextBox ID="txtUsuario" runat="server" CssClass="form-control"  PlaceHolder="Usuario" Width="400"   minlength="6" onkeypress="return validapass(event);" onpaste="alert('En este formulario no puede copiar y pegar');return false"></asp:TextBox>
                   </ContentTemplate>
                         </asp:UpdatePanel>
                   
                 
                 
                </div>
    <div>
        <label for="password"></label>
        <!-- checkbox que nos permite activar o desactivar la opcion -->
       
      </div>

    <div class="form-group">
                  <label for="txt">Ingrese los ultimo cuatro digitos del número teléfonico que se registro en su solicitud de usuario</label>
                  <asp:UpdatePanel ID="UpdatePanel4" runat="server">
               <ContentTemplate>  
         <asp:TextBox ID="txtTelefono" runat="server" CssClass="form-control"  PlaceHolder="Ultimos cuatro digitos"  Width="400"  minlength="4" onkeypress="return validapass(event);" onpaste="alert('En este formulario no puede copiar y pegar');return false"></asp:TextBox>

    </ContentTemplate>
                      </asp:UpdatePanel>
                               
     </div>
               
     <asp:UpdatePanel ID="UpdatePanel5" runat="server">
               <ContentTemplate>  
    <asp:Button ID="btnContrasena"   runat="server" cssclass="btn btn-primary" Text="Generar contraseña" OnClick="btnContrasena_Click"  />
   <asp:Button ID="btnCancela"   runat="server" cssclass="btn btn-warning" Text="Cancelar"   OnClick="btnCancela_Click"/>
</ContentTemplate>
           </asp:UpdatePanel>
             </div>
        </div>
</asp:Content>
