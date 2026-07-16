<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="SIPLAN2._0.login.login" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
     <style>
        *{
            padding:0;
            box-sizing:border-box;
        }
        .mainLogin{
            /*display:flex;*/
            position:relative;
            width:100%;
            height:79vh;
            background-color:transparent;
            /*justify-content:center;
            align-items:center;*/
        }
        .box{
            position:absolute;
            padding:15px;
            width:400px;
            height:300px;
            background-color:#dcdcdc;
            top:50%;
            left:50%;
            transform:translateX(-50%) translateY(-50%);
        }
        .box h2{
            text-align:center;
            font-weight:700;
        }

        @media (max-width: 479px) {
            .box {
                width: 320px;
                height: 300px;
            }
        }
    </style>

    <script type="text/javascript">
         window.onload = function () {
            for (var i = 0, l = document.getElementsByTagName('input').length; i < l; i++) {
                if (document.getElementsByTagName('input').item(i).type == 'text') {
                    document.getElementsByTagName('input').item(i).setAttribute('autocomplete', 'off');
                };
            };
        };


        function mostrar() {
            var checkBox = document.getElementById("mostrar_contrasena");
            if (checkBox.checked == true) {
                $('#MainContent_txtPass').attr('type', 'text');

            }
            else {
                $('#MainContent_txtPass').attr('type', 'password');

            }

        }

        function Alerta(mensaje, tipo) {          
            alertify.set('notifier', 'position', 'top-right'); 
            if (tipo == 1)
                alertify.success(mensaje);
            if (tipo == 2)
                alertify.error(mensaje);          
        };  

       function aMayusculas(obj, id) {
            obj = obj.toUpperCase();
            document.getElementById(id).value = obj;

        }

        function enter()
        {
            $(document).keypress(function(event){
    var keycode = (event.keyCode ? event.keyCode : event.which);
    if(keycode == '13'){
        alert('You pressed a "enter" key in somewhere');    
    }
});
        }
       
    </script>
    
    <div class="mainLogin">
        
        <div class="box">
            <h2>Iniciar Sesión</h2>
            <br>
             
            <div id="ContentPlaceHolder1_UpdatePanel1">
	
                    <div class="form-group has-feedback">               
                        <asp:TextBox ID="txtUser" CssClass="form-control" runat="server" placeholder="Usuario" onblur="aMayusculas(this.value,this.id)"></asp:TextBox>
                        <span class="glyphicon glyphicon-user form-control-feedback text-primary"></span>
            </div> 
                
</div>
             
            <div id="ContentPlaceHolder1_UpdatePanel2">
	
                     <div class="form-group has-feedback">
                         <asp:TextBox ID="txtPass" runat="server"  CssClass="form-control" placeholder="Contraseña"  TextMode="Password" OnTextChanged="txtPass_TextChanged"></asp:TextBox>
                         
                    <span class="glyphicon glyphicon-lock form-control-feedback text-primary"></span>
                          <div>
  <label for="password"></label>
  <!-- checkbox que nos permite activar o desactivar la opcion -->
  <div class="form-group has-feedback">
    <input style="margin-left:0px;" type="checkbox" id="mostrar_contrasena" title="clic para mostrar contraseña" onclick=" mostrar()"/>
    &nbsp;&nbsp;Mostrar contraseña</div>
</div>



            </div> 
                
</div> 
            
            <div id="ContentPlaceHolder1_UpdatePanel3">
                    
                <asp:Button ID="btnLogin"  runat="server" Text="Ingresar" CssClass="btn btn-primary btn-block" OnClick="btnLogin_Click" />
               
</div>         
            
            <div id="ContentPlaceHolder1_UpdatePanel4">
                    
                <p><a href="../Login/Contrasenia">¿Ha olvidado su contraseña? presione este enlace</a></p>
               
</div>         

        </div>
          
    </div>
   
</asp:Content>
