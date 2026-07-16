<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="frmEjecucionPOA.aspx.cs" Inherits="SIPLAN2._0.ejecucion.frmEjecucionPOA" %>

<%@ Register Assembly="DevExpress.Web.v17.2, Version=17.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<asp:Content ID="Content2"  ContentPlaceHolderID="MainContent"  runat="server">
   <style>
        .ajs-message.ajs-custom { color: #31708f;  background-color: #d9edf7;  border-color: #31708f; font-size:22px; z-index:5000 !important; }

        .productColumn {
    background-color: #f4f6f7;
    color:#2980b9;
    font-weight:bold;

}

       .btn-light-meta {
           background-color: #7fb3d5; /* Gris neutro */
           color: white;
          
          min-width: 150px; /* Ancho mínimo */
      padding: 10px 20px;
      font-size: 16px;
      border: none;
      border-radius: 5px;
     
      text-align: center;
      cursor: pointer;

       }

        .btn-light-gray {
      background-color: #2c3e50; /* Gris neutro */
      color: white;

     min-width: 150px; /* Ancho mínimo */
      padding: 10px 20px;
      font-size: 16px;
      border: none;
      border-radius: 5px;
      
      text-align: center;
      cursor: pointer;
  }

        .btn-light-meta:hover {
      background-color:#ecf0f1; /* Gris más claro al pasar el mouse */
      color:black;
  }

         .btn-light-gray:hover {
      background-color: #c6c8ca; /* Gris más claro al pasar el mouse */
  }
    </style>
    <script>
function soloNumeros(evt)
{
    var charCode = (evt.which) ? evt.which : event.keyCode;
  if (charCode != 46 && charCode > 31 
    && (charCode < 48 || charCode > 57)) 
    return false;

  return true;
}


        function Alerta(mensaje, tipo) {                        
            alertify.set('notifier', 'position', 'top-right');
            if (tipo == 1)
                alertify.success(mensaje);
            if (tipo == 2)
                alertify.error(mensaje).delay(10);
            if (tipo == 3)
                alertify.alert(mensaje);
        };

        function sumafisicas1er()
        {
            var mes1, mes2, mes3, mes4, cua1;
            if ($("#MainContent_wEjecFisProducto_TextBox1").val() == "")
                mes1 = 0
            else
                mes1 = $("#MainContent_wEjecFisProducto_TextBox1").val()

            if ($("#MainContent_wEjecFisProducto_TextBox2").val() == "")
                mes2 = 0
            else
                mes2 = $("#MainContent_wEjecFisProducto_TextBox2").val()


            if ($('#MainContent_wEjecFisProducto_TextBox3').val() == "")
                mes3 = 0
            else
                mes3 = $('#MainContent_wEjecFisProducto_TextBox3').val()


            if ($('#MainContent_wEjecFisProducto_TextBox4').val() == "")
                mes4 = 0
            else
                mes4 = $('#MainContent_wEjecFisProducto_TextBox4').val()


                 if (mes1 != 0)
            {
                if(mes1.indexOf(',') != -1)
                mes1 = mes1.replace(/,/g, "");
            }
            
            if (mes2 != 0)
            {
                if(mes2.indexOf(',') != -1)
                mes2 = mes2.replace(/,/g, "");
            }
             
            if (mes3 != 0)
            {
                if(mes3.indexOf(',') != -1)
                mes3 = mes3.replace(/,/g, "");
            }

            if (mes4 != 0)
            {
                if(mes4.indexOf(',') != -1)
                mes4 = mes4.replace(/,/g, "");
            }


            cua1 = parseInt(mes1) + parseInt(mes2) +parseInt(mes3) + parseInt(mes4);
           

            $('#MainContent_wEjecFisProducto_TextBox13').val(cua1);

            
        };





function sumafisicasTotal()
        {
            var mes1, mes2, mes3, mes4, mes5, mes6, mes7, mes8, mes9, mes10, mes11, mes12, total;
            if ($("#MainContent_popAvanceGERO_txtfg1").val() == "")
                mes1 = 0
            else
                mes1 = $("#MainContent_popAvanceGERO_txtfg1").val()

            if ($("#MainContent_popAvanceGERO_txtfg2").val() == "")
                mes2 = 0
            else
                mes2 = $("#MainContent_popAvanceGERO_txtfg2").val()


            if ($('#MainContent_popAvanceGERO_txtfg3').val() == "")
                mes3 = 0
            else
                mes3 = $('#MainContent_popAvanceGERO_txtfg3').val()


            if ($('#MainContent_popAvanceGERO_txtfg4').val() == "")
                mes4 = 0
            else
                mes4 = $('#MainContent_popAvanceGERO_txtfg4').val()


            if ($("#MainContent_popAvanceGERO_txtfg5").val() == "")
                mes5 = 0
            else
                mes5 = $("#MainContent_popAvanceGERO_txtfg5").val()

            if ($("#MainContent_popAvanceGERO_txtfg6").val() == "")
                mes6 = 0
            else
                mes6 = $("#MainContent_popAvanceGERO_txtfg6").val()


            if ($('#MainContent_popAvanceGERO_txtfg7').val() == "")
                mes7 = 0
            else
                mes7 = $('#MainContent_popAvanceGERO_txtfg7').val()


            if ($('#MainContent_popAvanceGERO_txtfg8').val() == "")
                mes8 = 0
            else
                mes8 = $('#MainContent_popAvanceGERO_txtfg8').val()



            if ($("#MainContent_popAvanceGERO_txtfg9").val() == "")
                mes9 = 0
            else
                mes9 = $("#MainContent_popAvanceGERO_txtfg9").val()

            if ($("#MainContent_popAvanceGERO_txtfg10").val() == "")
                mes10 = 0
            else
                mes10 = $("#MainContent_popAvanceGERO_txtfg10").val()


            if ($('#MainContent_popAvanceGERO_txtfg11').val() == "")
                mes11 = 0
            else
                mes11 = $('#MainContent_popAvanceGERO_txtfg11').val()


            if ($('#MainContent_popAvanceGERO_txtfg12').val() == "")
                mes12 = 0
            else
                mes12 = $('#MainContent_popAvanceGERO_txtfg12').val()



                 if (mes1 != 0)
            {
                if(mes1.indexOf(',') != -1)
                mes1 = mes1.replace(/,/g, "");
            }
            
            if (mes2 != 0)
            {
                if(mes2.indexOf(',') != -1)
                mes2 = mes2.replace(/,/g, "");
            }
             
            if (mes3 != 0)
            {
                if(mes3.indexOf(',') != -1)
                mes3 = mes3.replace(/,/g, "");
            }

            if (mes4 != 0)
            {
                if(mes4.indexOf(',') != -1)
                mes4 = mes4.replace(/,/g, "");
            }



 if (mes5 != 0)
            {
                if(mes5.indexOf(',') != -1)
                mes5 = mes5.replace(/,/g, "");
            }

 if (mes6 != 0)
            {
                if(mes6.indexOf(',') != -1)
                mes6 = mes6.replace(/,/g, "");
            }

 if (mes7 != 0)
            {
                if(mes7.indexOf(',') != -1)
                mes7 = mes7.replace(/,/g, "");
            }

 if (mes8 != 0)
            {
                if(mes8.indexOf(',') != -1)
                mes8 = mes8.replace(/,/g, "");
            }



if (mes9 != 0)
            {
                if(mes9.indexOf(',') != -1)
                mes9 = mes9.replace(/,/g, "");
            }

 if (mes10 != 0)
            {
                if(mes10.indexOf(',') != -1)
                mes10 = mes10.replace(/,/g, "");
            }

 if (mes11 != 0)
            {
                if(mes11.indexOf(',') != -1)
                mes11 = mes11.replace(/,/g, "");
            }

 if (mes12 != 0)
            {
                if(mes12.indexOf(',') != -1)
                mes12 = mes12.replace(/,/g, "");
            }

            total = parseInt(mes1) + parseInt(mes2) +parseInt(mes3) + parseInt(mes4) + parseInt(mes5) + parseInt(mes6) +parseInt(mes7) + parseInt(mes8) + parseInt(mes9) + parseInt(mes10) +parseInt(mes11) + parseInt(mes12);
           

            $('#MainContent_popAvanceGERO_txtAnualFisico').val(total);

            
        };







function sumafinancieraTotal()
        {
            var mes1, mes2, mes3, mes4, mes5, mes6, mes7, mes8, mes9, mes10, mes11, mes12, total;
            if ($("#MainContent_popAvanceGERO_txtfng1").val() == "")
                mes1 = 0
            else
                mes1 = $("#MainContent_popAvanceGERO_txtfng1").val()

            if ($("#MainContent_popAvanceGERO_txtfng2").val() == "")
                mes2 = 0
            else
                mes2 = $("#MainContent_popAvanceGERO_txtfng2").val()


            if ($('#MainContent_popAvanceGERO_txtfng3').val() == "")
                mes3 = 0
            else
                mes3 = $('#MainContent_popAvanceGERO_txtfng3').val()


            if ($('#MainContent_popAvanceGERO_txtfng4').val() == "")
                mes4 = 0
            else
                mes4 = $('#MainContent_popAvanceGERO_txtfng4').val()


            if ($("#MainContent_popAvanceGERO_txtfng5").val() == "")
                mes5 = 0
            else
                mes5 = $("#MainContent_popAvanceGERO_txtfng5").val()

            if ($("#MainContent_popAvanceGERO_txtfng6").val() == "")
                mes6 = 0
            else
                mes6 = $("#MainContent_popAvanceGERO_txtfng6").val()


            if ($('#MainContent_popAvanceGERO_txtfng7').val() == "")
                mes7 = 0
            else
                mes7 = $('#MainContent_popAvanceGERO_txtfng7').val()


            if ($('#MainContent_popAvanceGERO_txtfng8').val() == "")
                mes8 = 0
            else
                mes8 = $('#MainContent_popAvanceGERO_txtfng8').val()



            if ($("#MainContent_popAvanceGERO_txtfng9").val() == "")
                mes9 = 0
            else
                mes9 = $("#MainContent_popAvanceGERO_txtfng9").val()

            if ($("#MainContent_popAvanceGERO_txtfng10").val() == "")
                mes10 = 0
            else
                mes10 = $("#MainContent_popAvanceGERO_txtfng10").val()


            if ($('#MainContent_popAvanceGERO_txtfng11').val() == "")
                mes11 = 0
            else
                mes11 = $('#MainContent_popAvanceGERO_txtfng11').val()


            if ($('#MainContent_popAvanceGERO_txtfng12').val() == "")
                mes12 = 0
            else
                mes12 = $('#MainContent_popAvanceGERO_txtfng12').val()



                 if (mes1 != 0)
            {
                if(mes1.indexOf(',') != -1)
                mes1 = mes1.replace(/,/g, "");
            }
            
            if (mes2 != 0)
            {
                if(mes2.indexOf(',') != -1)
                mes2 = mes2.replace(/,/g, "");
            }
             
            if (mes3 != 0)
            {
                if(mes3.indexOf(',') != -1)
                mes3 = mes3.replace(/,/g, "");
            }

            if (mes4 != 0)
            {
                if(mes4.indexOf(',') != -1)
                mes4 = mes4.replace(/,/g, "");
            }



 if (mes5 != 0)
            {
                if(mes5.indexOf(',') != -1)
                mes5 = mes5.replace(/,/g, "");
            }

 if (mes6 != 0)
            {
                if(mes6.indexOf(',') != -1)
                mes6 = mes6.replace(/,/g, "");
            }

 if (mes7 != 0)
            {
                if(mes7.indexOf(',') != -1)
                mes7 = mes7.replace(/,/g, "");
            }

 if (mes8 != 0)
            {
                if(mes8.indexOf(',') != -1)
                mes8 = mes8.replace(/,/g, "");
            }



if (mes9 != 0)
            {
                if(mes9.indexOf(',') != -1)
                mes9 = mes9.replace(/,/g, "");
            }

 if (mes10 != 0)
            {
                if(mes10.indexOf(',') != -1)
                mes10 = mes10.replace(/,/g, "");
            }

 if (mes11 != 0)
            {
                if(mes11.indexOf(',') != -1)
                mes11 = mes11.replace(/,/g, "");
            }

 if (mes12 != 0)
            {
                if(mes12.indexOf(',') != -1)
                mes12 = mes12.replace(/,/g, "");
            }

            total = parseInt(mes1) + parseInt(mes2) +parseInt(mes3) + parseInt(mes4) + parseInt(mes5) + parseInt(mes6) +parseInt(mes7) + parseInt(mes8) + parseInt(mes9) + parseInt(mes10) +parseInt(mes11) + parseInt(mes12);
           

            $('#MainContent_popAvanceGERO_txtAnualFinanciero').val(total);

            
        };




function sumafisicasg1er()
        {
            var mes1, mes2, mes3, mes4, cua1;
            if ($("#MainContent_popAvanceGERO_txtfg1").val() == "")
                mes1 = 0
            else
                mes1 = $("#MainContent_popAvanceGERO_txtfg1").val()

            if ($("#MainContent_popAvanceGERO_txtfg2").val() == "")
                mes2 = 0
            else
                mes2 = $("#MainContent_popAvanceGERO_txtfg2").val()


            if ($('#MainContent_popAvanceGERO_txtfg3').val() == "")
                mes3 = 0
            else
                mes3 = $('#MainContent_popAvanceGERO_txtfg3').val()


            if ($('#MainContent_popAvanceGERO_txtfg4').val() == "")
                mes4 = 0
            else
                mes4 = $('#MainContent_popAvanceGERO_txtfg4').val()


                 if (mes1 != 0)
            {
                if(mes1.indexOf(',') != -1)
                mes1 = mes1.replace(/,/g, "");
            }
            
            if (mes2 != 0)
            {
                if(mes2.indexOf(',') != -1)
                mes2 = mes2.replace(/,/g, "");
            }
             
            if (mes3 != 0)
            {
                if(mes3.indexOf(',') != -1)
                mes3 = mes3.replace(/,/g, "");
            }

            if (mes4 != 0)
            {
                if(mes4.indexOf(',') != -1)
                mes4 = mes4.replace(/,/g, "");
            }


            cua1 = parseInt(mes1) + parseInt(mes2) +parseInt(mes3) + parseInt(mes4);
           

            $('#MainContent_popAvanceGERO_txtfgc1').val(cua1);

            sumafinancieraTotal();
            sumafisicasTotal();

            
        };


    function sumafinancierasg1er()
        {
            var mes1, mes2, mes3, mes4, cua1;
            if ($("#MainContent_popAvanceGERO_txtfng1").val() == "")
                mes1 = 0
            else
                mes1 = $("#MainContent_popAvanceGERO_txtfng1").val()

            if ($("#MainContent_popAvanceGERO_txtfng2").val() == "")
                mes2 = 0
            else
                mes2 = $("#MainContent_popAvanceGERO_txtfng2").val()


            if ($('#MainContent_popAvanceGERO_txtfng3').val() == "")
                mes3 = 0
            else
                mes3 = $('#MainContent_popAvanceGERO_txtfng3').val()


            if ($('#MainContent_popAvanceGERO_txtfng4').val() == "")
                mes4 = 0
            else
                mes4 = $('#MainContent_popAvanceGERO_txtfng4').val()

            if (mes1 != 0)
            {
                if(mes1.indexOf(',') != -1)
                mes1 = mes1.replace(/,/g, "");
            }
            
            if (mes2 != 0)
            {
                if(mes2.indexOf(',') != -1)
                mes2 = mes2.replace(/,/g, "");
            }
             
            if (mes3 != 0)
            {
                if(mes3.indexOf(',') != -1)
                mes3 = mes3.replace(/,/g, "");
            }

            if (mes4 != 0)
            {
                if(mes4.indexOf(',') != -1)
                mes4 = mes4.replace(/,/g, "");
            }

             

            
            cua1 = parseFloat(mes1) + parseFloat(mes2) + parseFloat(mes3) + parseFloat(mes4);
            console.log(cua1);

            $('#MainContent_popAvanceGERO_txtfngc1').val(cua1);
            sumafinancieraTotal();
            sumafisicasTotal();

        };


        function sumafinancieras1er()
        {
            var mes1, mes2, mes3, mes4, cua1;
            if ($("#MainContent_wEjecFisProducto_txtFinM1").val() == "")
                mes1 = 0
            else
                mes1 = $("#MainContent_wEjecFisProducto_txtFinM1").val()

            if ($("#MainContent_wEjecFisProducto_txtFinM2").val() == "")
                mes2 = 0
            else
                mes2 = $("#MainContent_wEjecFisProducto_txtFinM2").val()


            if ($('#MainContent_wEjecFisProducto_txtFinM3').val() == "")
                mes3 = 0
            else
                mes3 = $('#MainContent_wEjecFisProducto_txtFinM3').val()


            if ($('#MainContent_wEjecFisProducto_txtFinM4').val() == "")
                mes4 = 0
            else
                mes4 = $('#MainContent_wEjecFisProducto_txtFinM4').val()

            if (mes1 != 0)
            {
                if(mes1.indexOf(',') != -1)
                mes1 = mes1.replace(/,/g, "");
            }
            
            if (mes2 != 0)
            {
                if(mes2.indexOf(',') != -1)
                mes2 = mes2.replace(/,/g, "");
            }
             
            if (mes3 != 0)
            {
                if(mes3.indexOf(',') != -1)
                mes3 = mes3.replace(/,/g, "");
            }

            if (mes4 != 0)
            {
                if(mes4.indexOf(',') != -1)
                mes4 = mes4.replace(/,/g, "");
            }

             

            
            cua1 = parseFloat(mes1) + parseFloat(mes2) + parseFloat(mes3) + parseFloat(mes4);
            console.log(cua1);

            $('#MainContent_wEjecFisProducto_txtFinC1').val(cua1);

        };



function sumafisicasg2do()
        {
            var mes5, mes6, mes7, mes8, cua2;
            if ($("#MainContent_popAvanceGERO_txtfg5").val() == "")
                mes5 = 0
            else
                mes5 = $("#MainContent_popAvanceGERO_txtfg5").val()

            if ($("#MainContent_popAvanceGERO_txtfg6").val() == "")
                mes6 = 0
            else
                mes6 = $("#MainContent_popAvanceGERO_txtfg6").val()


            if ($('#MainContent_popAvanceGERO_txtfg7').val() == "")
                mes7 = 0
            else
                mes7 = $('#MainContent_popAvanceGERO_txtfg7').val()


            if ($('#MainContent_popAvanceGERO_txtfg8').val() == "")
                mes8 = 0
            else
                mes8 = $('#MainContent_popAvanceGERO_txtfg8').val()


                 if (mes5 != 0)
            {
                if(mes5.indexOf(',') != -1)
                mes5 = mes5.replace(/,/g, "");
            }
            
            if (mes6 != 0)
            {
                if(mes6.indexOf(',') != -1)
                mes6 = mes6.replace(/,/g, "");
            }
             
            if (mes7 != 0)
            {
                if(mes7.indexOf(',') != -1)
                mes7 = mes7.replace(/,/g, "");
            }

            if (mes8 != 0)
            {
                if(mes8.indexOf(',') != -1)
                mes8 = mes8.replace(/,/g, "");
            }


            cua2 = parseInt(mes5) + parseInt(mes6) +parseInt(mes7) + parseInt(mes8);
           

            $('#MainContent_popAvanceGERO_txtfgc2').val(cua2);

            sumafinancieraTotal();
            sumafisicasTotal();

            
        };



        function sumafisicas2do()
        {
            var mes5, mes6, mes7, mes8, cua2;
            if ($("#MainContent_wEjecFisProducto_TextBox5").val() == "")
                mes5 = 0
            else
                mes5 = $("#MainContent_wEjecFisProducto_TextBox5").val()

            if ($("#MainContent_wEjecFisProducto_TextBox6").val() == "")
                mes6 = 0
            else
                mes6 = $("#MainContent_wEjecFisProducto_TextBox6").val()


            if ($('#MainContent_wEjecFisProducto_TextBox7').val() == "")
                mes7 = 0
            else
                mes7 = $('#MainContent_wEjecFisProducto_TextBox7').val()


            if ($('#MainContent_wEjecFisProducto_TextBox8').val() == "")
                mes8 = 0
            else
                mes8 = $('#MainContent_wEjecFisProducto_TextBox8').val()


                 if (mes5 != 0)
            {
                if(mes5.indexOf(',') != -1)
                mes5 = mes5.replace(/,/g, "");
            }
            
            if (mes6 != 0)
            {
                if(mes6.indexOf(',') != -1)
                mes6 = mes6.replace(/,/g, "");
            }
             
            if (mes7 != 0)
            {
                if(mes7.indexOf(',') != -1)
                mes7 = mes7.replace(/,/g, "");
            }

            if (mes8 != 0)
            {
                if(mes8.indexOf(',') != -1)
                mes8 = mes8.replace(/,/g, "");
            }


            cua2 = parseInt(mes5) + parseInt(mes6) +parseInt(mes7) + parseInt(mes8);
           

            $('#MainContent_wEjecFisProducto_TextBox14').val(cua2);

            
        };




 function sumafinancierasg2do()
        {
            var mes5, mes6, mes7, mes8, cua2;
            if ($("#MainContent_popAvanceGERO_txtfng5").val() == "")
                mes5 = 0
            else
                mes5 = $("#MainContent_popAvanceGERO_txtfng5").val()

            if ($("#MainContent_popAvanceGERO_txtfng6").val() == "")
                mes6 = 0
            else
                mes6 = $("#MainContent_popAvanceGERO_txtfng6").val()


            if ($('#MainContent_popAvanceGERO_txtfng7').val() == "")
                mes7 = 0
            else
                mes7 = $('#MainContent_popAvanceGERO_txtfng7').val()


            if ($('#MainContent_popAvanceGERO_txtfng8').val() == "")
                mes8 = 0
            else
                mes8 = $('#MainContent_popAvanceGERO_txtfng8').val()

            if (mes5 != 0)
            {
                if(mes5.indexOf(',') != -1)
                mes5 = mes5.replace(/,/g, "");
            }
            
            if (mes6 != 0)
            {
                if(mes6.indexOf(',') != -1)
                mes6 = mes6.replace(/,/g, "");
            }
             
            if (mes7 != 0)
            {
                if(mes7.indexOf(',') != -1)
                mes7 = mes7.replace(/,/g, "");
            }

            if (mes8 != 0)
            {
                if(mes8.indexOf(',') != -1)
                mes8 = mes8.replace(/,/g, "");
            }

             

            
            cua2 = parseFloat(mes5) + parseFloat(mes6) + parseFloat(mes7) + parseFloat(mes8);
            console.log(cua2);

            $('#MainContent_popAvanceGERO_txtfngc2').val(cua2);

            sumafinancieraTotal();
            sumafisicasTotal();

        };


        function sumafinancieras2do()
        {
            var mes5, mes6, mes7, mes8, cua2;
            if ($("#MainContent_wEjecFisProducto_txtFinM5").val() == "")
                mes5 = 0
            else
                mes5 = $("#MainContent_wEjecFisProducto_txtFinM5").val()

            if ($("#MainContent_wEjecFisProducto_txtFinM6").val() == "")
                mes6 = 0
            else
                mes6 = $("#MainContent_wEjecFisProducto_txtFinM6").val()


            if ($('#MainContent_wEjecFisProducto_txtFinM7').val() == "")
                mes7 = 0
            else
                mes7 = $('#MainContent_wEjecFisProducto_txtFinM7').val()


            if ($('#MainContent_wEjecFisProducto_txtFinM8').val() == "")
                mes8 = 0
            else
                mes8 = $('#MainContent_wEjecFisProducto_txtFinM8').val()

            if (mes5 != 0)
            {
                if(mes5.indexOf(',') != -1)
                mes5 = mes5.replace(/,/g, "");
            }
            
            if (mes6 != 0)
            {
                if(mes6.indexOf(',') != -1)
                mes6 = mes6.replace(/,/g, "");
            }
             
            if (mes7 != 0)
            {
                if(mes7.indexOf(',') != -1)
                mes7 = mes7.replace(/,/g, "");
            }

            if (mes8 != 0)
            {
                if(mes8.indexOf(',') != -1)
                mes8 = mes8.replace(/,/g, "");
            }

             

            
            cua2 = parseFloat(mes5) + parseFloat(mes6) + parseFloat(mes7) + parseFloat(mes8);
            console.log(cua2);

            $('#MainContent_wEjecFisProducto_txtFinC2').val(cua2);

        };


        function sumafisicas3er()
        {
            var mes9, mes10, mes11, mes12, cua3;
            if ($("#MainContent_wEjecFisProducto_TextBox9").val() == "")
                mes9 = 0
            else
                mes9 = $("#MainContent_wEjecFisProducto_TextBox9").val()

            if ($("#MainContent_wEjecFisProducto_TextBox10").val() == "")
                mes10 = 0
            else
                mes10 = $("#MainContent_wEjecFisProducto_TextBox10").val()


            if ($('#MainContent_wEjecFisProducto_TextBox11').val() == "")
                mes11 = 0
            else
                mes11 = $('#MainContent_wEjecFisProducto_TextBox11').val()


            if ($('#MainContent_wEjecFisProducto_TextBox12').val() == "")
                mes12 = 0
            else
                mes12 = $('#MainContent_wEjecFisProducto_TextBox12').val()


                 if (mes9 != 0)
            {
                if(mes9.indexOf(',') != -1)
                mes9 = mes9.replace(/,/g, "");
            }
            
            if (mes10 != 0)
            {
                if(mes10.indexOf(',') != -1)
                mes10 = mes10.replace(/,/g, "");
            }
             
            if (mes11 != 0)
            {
                if(mes11.indexOf(',') != -1)
                mes11 = mes11.replace(/,/g, "");
            }

            if (mes12 != 0)
            {
                if(mes12.indexOf(',') != -1)
                mes12 = mes12.replace(/,/g, "");
            }


            cua3 = parseInt(mes9) + parseInt(mes10) +parseInt(mes11) + parseInt(mes12);
           

            $('#MainContent_wEjecFisProducto_TextBox14').val(cua3);

            
        };



function sumafisicasg3er()
        {
            var mes9, mes10, mes11, mes12, cua3;
            if ($("#MainContent_popAvanceGERO_txtfg9").val() == "")
                mes9 = 0
            else
                mes9 = $("#MainContent_popAvanceGERO_txtfg9").val()

            if ($("#MainContent_popAvanceGERO_txtfg10").val() == "")
                mes10 = 0
            else
                mes10 = $("#MainContent_popAvanceGERO_txtfg10").val()


            if ($('#MainContent_popAvanceGERO_txtfg11').val() == "")
                mes11 = 0
            else
                mes11 = $('#MainContent_popAvanceGERO_txtfg11').val()


            if ($('#MainContent_popAvanceGERO_txtfg12').val() == "")
                mes12 = 0
            else
                mes12 = $('#MainContent_popAvanceGERO_txtfg12').val()


                 if (mes9 != 0)
            {
                if(mes9.indexOf(',') != -1)
                mes9 = mes9.replace(/,/g, "");
            }
            
            if (mes10 != 0)
            {
                if(mes10.indexOf(',') != -1)
                mes10 = mes10.replace(/,/g, "");
            }
             
            if (mes11 != 0)
            {
                if(mes11.indexOf(',') != -1)
                mes11 = mes11.replace(/,/g, "");
            }

            if (mes12 != 0)
            {
                if(mes12.indexOf(',') != -1)
                mes12 = mes12.replace(/,/g, "");
            }


            cua3 = parseInt(mes9) + parseInt(mes10) +parseInt(mes11) + parseInt(mes12);
           

            $('#MainContent_popAvanceGERO_txtfgc3').val(cua3);

            sumafinancieraTotal();
            sumafisicasTotal();

            
        };




function sumafinancierasg3er()
        {
            var mes9, mes10, mes11, mes12, cua3;
            if ($("#MainContent_popAvanceGERO_txtfng9").val() == "")
                mes9 = 0
            else
                mes9 = $("#MainContent_popAvanceGERO_txtfng9").val()

            if ($("#MainContent_popAvanceGERO_txtfng10").val() == "")
                mes10 = 0
            else
                mes10 = $("#MainContent_popAvanceGERO_txtfng10").val()


            if ($('#MainContent_popAvanceGERO_txtfng11').val() == "")
                mes11 = 0
            else
                mes11 = $('#MainContent_popAvanceGERO_txtfng11').val()


            if ($('#MainContent_popAvanceGERO_txtfng12').val() == "")
                mes12 = 0
            else
                mes12 = $('#MainContent_popAvanceGERO_txtfng12').val()

            if (mes9 != 0)
            {
                if(mes9.indexOf(',') != -1)
                mes9 = mes9.replace(/,/g, "");
            }
            
            if (mes10 != 0)
            {
                if(mes10.indexOf(',') != -1)
                mes10 = mes10.replace(/,/g, "");
            }
             
            if (mes11 != 0)
            {
                if(mes11.indexOf(',') != -1)
                mes11 = mes11.replace(/,/g, "");
            }

            if (mes12 != 0)
            {
                if(mes12.indexOf(',') != -1)
                mes12 = mes12.replace(/,/g, "");
            }

             

            
            cua3 = parseFloat(mes9) + parseFloat(mes10) + parseFloat(mes11) + parseFloat(mes12);
            console.log(cua3);

            $('#MainContent_popAvanceGERO_txtfngc3').val(cua3);

            sumafinancieraTotal();
            sumafisicasTotal();

        };
        
        function sumafinancieras3er()
        {
            var mes9, mes10, mes11, mes12, cua3;
            if ($("#MainContent_wEjecFisProducto_txtFinM9").val() == "")
                mes9 = 0
            else
                mes9 = $("#MainContent_wEjecFisProducto_txtFinM9").val()

            if ($("#MainContent_wEjecFisProducto_txtFinM10").val() == "")
                mes10 = 0
            else
                mes10 = $("#MainContent_wEjecFisProducto_txtFinM10").val()


            if ($('#MainContent_wEjecFisProducto_txtFinM11').val() == "")
                mes11 = 0
            else
                mes11 = $('#MainContent_wEjecFisProducto_txtFinM11').val()


            if ($('#MainContent_wEjecFisProducto_txtFinM12').val() == "")
                mes12 = 0
            else
                mes12 = $('#MainContent_wEjecFisProducto_txtFinM12').val()

            if (mes9 != 0)
            {
                if(mes9.indexOf(',') != -1)
                mes9 = mes9.replace(/,/g, "");
            }
            
            if (mes10 != 0)
            {
                if(mes10.indexOf(',') != -1)
                mes10 = mes10.replace(/,/g, "");
            }
             
            if (mes11 != 0)
            {
                if(mes11.indexOf(',') != -1)
                mes11 = mes11.replace(/,/g, "");
            }

            if (mes12 != 0)
            {
                if(mes12.indexOf(',') != -1)
                mes12 = mes12.replace(/,/g, "");
            }

             

            
            cua3 = parseFloat(mes9) + parseFloat(mes10) + parseFloat(mes11) + parseFloat(mes12);
            console.log(cua3);

            $('#MainContent_wEjecFisProducto_txtFinC3').val(cua3);

        };

    function restringe(e) {
        var k;
        document.all ? k = e.keyCode : k = e.which;
        return ((k > 64 && k < 91) || (k > 96 && k < 123) || k == 110 || k == 8 || k == 32 || (k >= 48 && k <= 57));
    }

    </script>
    
    <br />
      <asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex="0">
         <asp:View ID="View1" runat="server">
    <div class="btn-group" style="display:flex; justify-content:flex-start;">                                  
                                              
                                     <asp:Button ID="btnProgramacion" runat="server" Text="Formulación"  CssClass="btn btn-primary" ToolTip="Regresar a formulación multianual"  OnClick="btnProgramacion_Click"/> 
                                     <asp:Button ID="btnReporte" runat="server" Text="Generar reporte ejecución"  CssClass="btn btn-success" ToolTip="Generar reporte del primer cuatrimestre"  OnClick="btnReporte_Click" /> 
                                       <asp:Button ID="btnEncuesta" runat="server" Text="Encuesta electrónica"  CssClass="btn btn-danger" ToolTip="Presione aquí para ir a encuesta electrónica"  OnClick="btnEncuesta_Click"/> 
                                       <!--button type="button" class="btn btn-danger" id="encuesta" data-toggle="tooltip" data-placement="top" title="Presione aquí para ir a encuesta electrónica" onclick="abrirEnlace()">Encuesta electrónica</!--button>-->
                                        <asp:Button ID="btnGero" runat="server" Text="Seguimiento proyectos SNIP GERO"  CssClass="btn " ToolTip="Seguimiento a proyectos SNIP GERO"  OnClick="btnGero_Click"  style="color:white;background-color:dimgrey"  />
                                       <asp:Button ID="btnRegresarinicio" runat="server" Text="Ir a pagina de inicio"  CssClass="btn " ToolTip="Regresar a pagina de inicio"  OnClick="btnRegresarinicio_Click" style="color:white;background-color:#F28B08" />
                                   <asp:Button ID="btnIngresoFechas" runat="server" Text="Gestión de fechas de cierre"  CssClass="btn " ToolTip="Gestión de fechas de cierre"  OnClick="btnIngresoFechas_Click" Visible="false" style="color:white;background-color:black" />
                                   
                                                     </div>
    
    <br />
      <!--<asp:UpdatePanel ID="UpdatePanel5" runat="server">
            <ContentTemplate>-->
                       <div class="row" style="background-color:#39a9dc">
            <div class="col-sm-2">
                <div style="text-align:center;margin-top:20px;margin-left:50px">
                <img src="../images/mensaje.png"  width="80%"/>
                    </div>
            </div>


            <div class="col-sm-10">
               <div style="text-align:center">
                <h3 style="color:white">INFORMACIÓN IMPORTANTE</h3>
                   </div>
                <div style="text-align:justify;margin-right:30px">
                    <p style="font-weight:bold;font-size:14px;color:darkblue"><asp:Label ID="instos"  runat="server"  ForeColor="white"></asp:Label></p>
                    <p style="font-weight:bold;font-size:14px;color:white"><asp:Label ID="lblmensaje" runat="server"></asp:Label></p>
                <p style="font-weight:bold;font-size:18px;color:white">INFORMACIÓN IMPORTANTE: Los productos/subproductos que se desplieguen en letras rojas exceden mas del 100% en la ejecución física/financiera, se recomienda revisar las metas vigentes, reprogramaciones de metas de productos y subproductos, así como el registro de las ejecuciones mensuales para actualizar el porcentaje de ejecución</p>
                </div>
            </div>

        </div>
                <!--</ContentTemplate>
          </asp:UpdatePanel>-->
   <h4>Los calculos de avance y porcentaje de ejecución se realizan en base a la meta vigente de productos y subproductos</h4>

               <!-- <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>-->
                    <dx:ASPxGridView ID="gvEjecucion" runat="server" AutoGenerateColumns="False" KeyFieldName="ID_PRODUCTO" SettingsDetail-AllowOnlyOneMasterRowExpanded="true" SettingsBehavior-AllowFixedGroups="true" SettingsBehavior-AllowFocusedRow ="true" Width="100%" OnDetailRowExpandedChanged="gvEjecucion_DetailRowExpandedChanged" OnSelectionChanged="gvEjecucion_SelectionChanged" Theme="Office2010Blue"  OnHtmlDataCellPrepared="gvEjecucion_HtmlDataCellPrepared">
                    <SettingsBehavior AllowFixedGroups="True" AllowFocusedRow="True" />
                         <Settings ShowFilterRow="True" ShowGroupPanel="True" />
                    <Columns>
                        <dx:GridViewDataTextColumn Caption="EJECUCI&#211;N" VisibleIndex="0">
                            <DataItemTemplate>
                                <asp:Button ID="btnEjectProducto" CssClass="btn btn-info btn-sm" runat="server" OnClick="btnEjectProducto_Click" Text="Ingresar" />
                            </DataItemTemplate>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn FieldName="ID_TIPO" VisibleIndex="0" Visible="false"></dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn FieldName="TIPO" VisibleIndex="1" GroupIndex="0"></dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn FieldName="ID_RPRESUP" VisibleIndex="2" Visible="false"></dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn FieldName="RPRESUP" Caption="PROGRAMA PRESUPUESTARIO" VisibleIndex ="3" GroupIndex="1"></dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn FieldName="ID_RESULTADO" VisibleIndex="4" Visible="false"></dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn FieldName="RESULTADO" VisibleIndex="5" Width="30%">
                            <Settings AutoFilterCondition="Contains" />
                        </dx:GridViewDataTextColumn>                        
                        <dx:GridViewDataTextColumn FieldName="ID_PRODUCTO" VisibleIndex="6" Visible="false"></dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn FieldName="PRODUCTO" VisibleIndex="7" Width="30%">
                            <Settings AutoFilterCondition="Contains" />
                        </dx:GridViewDataTextColumn>
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
                         <dx:GridViewDataTextColumn FieldName="PFINNUMBER" VisibleIndex="8" Visible="false"></dx:GridViewDataTextColumn>
                         <dx:GridViewDataTextColumn FieldName="PFNUMBER" VisibleIndex="9" Visible="false"></dx:GridViewDataTextColumn>
                    </Columns>
                    <SettingsDetail ShowDetailRow="true" />
                    <Templates>
                        <DetailRow>
                            <h5><asp:Label ID="porcentaje2" Visible ="false" runat="server"></asp:Label></h5>
                            <dx:ASPxGridView ID="gvSubProductos" KeyFieldName="ID_SUBP" SettingsBehavior-AllowFocusedRow="true" runat="server" Width="100%" OnBeforePerformDataSelect="gvSubProductos_BeforePerformDataSelect" AutoGenerateColumns="False" Theme="Office2010Blue" OnRowUpdating="gvSubProductos_RowUpdating" OnCellEditorInitialize="gvSubProductos_CellEditorInitialize" OnHtmlDataCellPrepared="gvSubProductos_HtmlDataCellPrepared">
                                 <Settings ShowFilterRow="True" ShowGroupPanel="True" />
                                <SettingsEditing Mode="Inline" />
                                <SettingsCommandButton>
                                    <EditButton Text="Ingresar" Styles-Style-CssClass="btn btn-info btn-sm"></EditButton>
                                    <UpdateButton Text="Actualizar" Styles-Style-CssClass="btn btn-info btn-xs"></UpdateButton>
                                    <CancelButton Text="Cancelar" Styles-Style-CssClass="btn btn-info btn-xs"></CancelButton>
                                </SettingsCommandButton>
                                <Settings VerticalScrollableHeight="400" VerticalScrollBarMode="Visible" HorizontalScrollBarMode="Visible" />
                                <SettingsBehavior AllowFocusedRow="True" />
                                <Styles>
                                    <%--<SelectedRow BackColor="Red" ForeColor="White"></SelectedRow>--%>
                                    <%--<FocusedRow BackColor="Red"></FocusedRow>--%>
                                </Styles>
                                <Columns>
                                    <dx:GridViewCommandColumn ShowEditButton="True" Caption="EJECUCIÓN" VisibleIndex="0" Width="130" CellStyle-Wrap="True" ButtonRenderMode="Button" ButtonType="Button" AllowTextTruncationInAdaptiveMode="True">
                                        
                                    </dx:GridViewCommandColumn>
                                    <dx:GridViewDataTextColumn FieldName="ID_PROD" VisibleIndex="1" Visible="false">
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn FieldName="ID_SUBP" VisibleIndex="2" Visible="false">
                                    </dx:GridViewDataTextColumn>

                                    <dx:GridViewDataTextColumn Caption="Metas por territorio" VisibleIndex="3" Visible="true">
    <DataItemTemplate>
        <asp:Panel ID="pnlBtn" runat="server">
          <dx:ASPxButton ID="btnMetasMuno" runat="server"
    Text="Metas por territorio"
   
    CommandArgument='<%# Eval("ID_SUBP") %>'
    OnClick="btnMetasMuno_Click"/>
        </asp:Panel>
    </DataItemTemplate>
</dx:GridViewDataTextColumn>

                                    <dx:GridViewDataTextColumn FieldName="MUNICIPIOS"  
                                       Caption="Número de municipios<br/>priorizados" 
                                        VisibleIndex="4" ReadOnly="true" Visible="true">
                                         <HeaderStyle HorizontalAlign="Center" />
                                    </dx:GridViewDataTextColumn>


                                    <dx:GridViewDataTextColumn Caption="SUBPRODUCTO" FieldName="SUBP" VisibleIndex="5" Width="300" ReadOnly="true">
                                         <Settings AutoFilterCondition="Contains" />
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewBandColumn Caption="FÍSICO ANUAL" VisibleIndex="6">
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
                                    <dx:GridViewBandColumn Caption="FINANCIERO ANUAL" VisibleIndex="7">
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
                                    <dx:GridViewBandColumn Caption="ENERO" VisibleIndex="8">
                                        <Columns>
                                            
                                            <dx:GridViewDataSpinEditColumn FieldName="AFIS1"  Caption="FISICO" VisibleIndex="1" ReadOnly="false" Visible="true">
            <PropertiesSpinEdit DisplayFormatString="N2" />
        </dx:GridViewDataSpinEditColumn>
                                            <dx:GridViewDataSpinEditColumn FieldName="AFIN1"  Caption="FINANCIERO" VisibleIndex="2" ReadOnly="false" Visible="true">
            <PropertiesSpinEdit DisplayFormatString="N2" />
        </dx:GridViewDataSpinEditColumn>
                                            
                                            <dx:GridViewDataTextColumn FieldName="SFIS1" Caption="ESTADO" VisibleIndex="3" Visible="false"></dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn FieldName="SFIN1" Caption="ESTADO" VisibleIndex="4" Visible="false"></dx:GridViewDataTextColumn>
                                        </Columns>
                                    </dx:GridViewBandColumn>
                                    <dx:GridViewBandColumn Caption="FEBRERO" VisibleIndex="9">
                                        <Columns>
                                            <dx:GridViewDataSpinEditColumn FieldName="AFIS2"  Caption="FISICO" VisibleIndex="1" ReadOnly="false" Visible="true">
            <PropertiesSpinEdit DisplayFormatString="N2" />
        </dx:GridViewDataSpinEditColumn>
                                            <dx:GridViewDataSpinEditColumn FieldName="AFIN2"  Caption="FINANCIERO" VisibleIndex="2" ReadOnly="false" Visible="true">
            <PropertiesSpinEdit DisplayFormatString="N2" />
        </dx:GridViewDataSpinEditColumn>
                                            <dx:GridViewDataTextColumn FieldName="SFIS2" Caption="ESTADO" VisibleIndex="3" Visible="false"></dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn FieldName="SFIN2" Caption="ESTADO" VisibleIndex="4" Visible="false"></dx:GridViewDataTextColumn>
                                        </Columns>
                                    </dx:GridViewBandColumn>
                                    <dx:GridViewBandColumn Caption="MARZO" VisibleIndex="10">
                                        <Columns> 
                                            <dx:GridViewDataSpinEditColumn FieldName="AFIS3"  Caption="FISICO" VisibleIndex="1" ReadOnly="false" Visible="true">
            <PropertiesSpinEdit DisplayFormatString="N2" />
        </dx:GridViewDataSpinEditColumn>
                                            <dx:GridViewDataSpinEditColumn FieldName="AFIN3"  Caption="FINANCIERO" VisibleIndex="2" ReadOnly="false" Visible="true">
            <PropertiesSpinEdit DisplayFormatString="N2" />
        </dx:GridViewDataSpinEditColumn>
                                            <dx:GridViewDataTextColumn FieldName="SFIS3" Caption="ESTADO" VisibleIndex="3" Visible="false"></dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn FieldName="SFIN3" Caption="ESTADO" VisibleIndex="4" Visible="false"></dx:GridViewDataTextColumn>
                                        </Columns>
                                    </dx:GridViewBandColumn>
                                    <dx:GridViewBandColumn Caption="ABRIL" VisibleIndex="11">
                                        <Columns>
                                           <dx:GridViewDataSpinEditColumn FieldName="AFIS4"  Caption="FISICO" VisibleIndex="1" ReadOnly="false" Visible="true">
            <PropertiesSpinEdit DisplayFormatString="N2" />
        </dx:GridViewDataSpinEditColumn>
                                            <dx:GridViewDataSpinEditColumn FieldName="AFIN4"  Caption="FINANCIERO" VisibleIndex="2" ReadOnly="false" Visible="true">
            <PropertiesSpinEdit DisplayFormatString="N2" />
        </dx:GridViewDataSpinEditColumn>
                                            <dx:GridViewDataTextColumn FieldName="SFIS4" Caption="ESTADO" VisibleIndex="3" Visible="false"></dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn FieldName="SFIN4" Caption="ESTADO" VisibleIndex="4" Visible="false"></dx:GridViewDataTextColumn>
                                        </Columns>
                                    </dx:GridViewBandColumn>
                                    <dx:GridViewBandColumn Caption="CUATRIMESTRE 1" VisibleIndex="12">
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
                                    <dx:GridViewBandColumn Caption="MAYO" VisibleIndex="13" >
                                        <Columns>
                                            <dx:GridViewDataSpinEditColumn FieldName="AFIS5"  Caption="FISICO" VisibleIndex="1" ReadOnly="false" Visible="true">
            <PropertiesSpinEdit DisplayFormatString="N2" />
        </dx:GridViewDataSpinEditColumn>
                                            <dx:GridViewDataSpinEditColumn FieldName="AFIN5"  Caption="FINANCIERO" VisibleIndex="2" ReadOnly="false" Visible="true">
            <PropertiesSpinEdit DisplayFormatString="N2" />
        </dx:GridViewDataSpinEditColumn>
                                            <dx:GridViewDataTextColumn FieldName="SFIS5" Caption="ESTADO" VisibleIndex="3" Visible="false"></dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn FieldName="SFIN5" Caption="ESTADO" VisibleIndex="4" Visible="false"></dx:GridViewDataTextColumn>
                                        </Columns>
                                    </dx:GridViewBandColumn>
                                    <dx:GridViewBandColumn Caption="JUNIO" VisibleIndex="14">
                                        <Columns>
                                            <dx:GridViewDataSpinEditColumn FieldName="AFIS6"  Caption="FISICO" VisibleIndex="1" ReadOnly="false" Visible="true">
            <PropertiesSpinEdit DisplayFormatString="N2" />
        </dx:GridViewDataSpinEditColumn>
                                            <dx:GridViewDataSpinEditColumn FieldName="AFIN6"  Caption="FINANCIERO" VisibleIndex="2" ReadOnly="false" Visible="true">
            <PropertiesSpinEdit DisplayFormatString="N2" />
        </dx:GridViewDataSpinEditColumn>
                                            <dx:GridViewDataTextColumn FieldName="SFIS6" Caption="ESTADO" VisibleIndex="3" Visible="false"></dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn FieldName="SFIN6" Caption="ESTADO" VisibleIndex="4" Visible="false"></dx:GridViewDataTextColumn>
                                        </Columns>
                                    </dx:GridViewBandColumn>
                                    <dx:GridViewBandColumn Caption="JULIO" VisibleIndex="15">
                                        <Columns>
                                          <dx:GridViewDataSpinEditColumn FieldName="AFIS7"  Caption="FISICO" VisibleIndex="1" ReadOnly="false" Visible="true">
            <PropertiesSpinEdit DisplayFormatString="N2" />
        </dx:GridViewDataSpinEditColumn>
                                            <dx:GridViewDataSpinEditColumn FieldName="AFIN7"  Caption="FINANCIERO" VisibleIndex="2" ReadOnly="false" Visible="true">
            <PropertiesSpinEdit DisplayFormatString="N2" />
        </dx:GridViewDataSpinEditColumn>
                                            <dx:GridViewDataTextColumn FieldName="SFIS7" Caption="ESTADO" VisibleIndex="3" Visible="false"></dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn FieldName="SFIN7" Caption="ESTADO" VisibleIndex="4" Visible="false"></dx:GridViewDataTextColumn>
                                        </Columns>
                                    </dx:GridViewBandColumn>
                                    <dx:GridViewBandColumn Caption="AGOSTO" VisibleIndex="16">
                                        <Columns>
                                            <dx:GridViewDataSpinEditColumn FieldName="AFIS8"  Caption="FISICO" VisibleIndex="1" ReadOnly="false" Visible="true">
            <PropertiesSpinEdit DisplayFormatString="N2" />
        </dx:GridViewDataSpinEditColumn>
                                            <dx:GridViewDataSpinEditColumn FieldName="AFIN8"  Caption="FINANCIERO" VisibleIndex="2" ReadOnly="false" Visible="true">
            <PropertiesSpinEdit DisplayFormatString="N2" />
        </dx:GridViewDataSpinEditColumn>
                                            <dx:GridViewDataTextColumn FieldName="SFIS8" Caption="ESTADO" VisibleIndex="3" Visible="false"></dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn FieldName="SFIN8" Caption="ESTADO" VisibleIndex="4" Visible="false"></dx:GridViewDataTextColumn>
                                        </Columns>
                                    </dx:GridViewBandColumn>
                                    <dx:GridViewBandColumn Caption="CUATRIMESTRE 2" VisibleIndex="17">
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
                                    <dx:GridViewBandColumn Caption="SEPTIEMBRE" VisibleIndex="18" Visible="true">
                                        <Columns>
                                            <dx:GridViewDataSpinEditColumn FieldName="AFIS9"  Caption="FISICO" VisibleIndex="1" ReadOnly="false" Visible="true">
            <PropertiesSpinEdit DisplayFormatString="N2" />
        </dx:GridViewDataSpinEditColumn>
                                            <dx:GridViewDataSpinEditColumn FieldName="AFIN9"  Caption="FINANCIERO" VisibleIndex="2" ReadOnly="false" Visible="true">
            <PropertiesSpinEdit DisplayFormatString="N2" />
        </dx:GridViewDataSpinEditColumn>
                                            <dx:GridViewDataTextColumn FieldName="SFIS9" Caption="ESTADO" VisibleIndex="3" Visible="false"></dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn FieldName="SFIN9" Caption="ESTADO" VisibleIndex="4" Visible="false"></dx:GridViewDataTextColumn>
                                        </Columns>
                                    </dx:GridViewBandColumn>
                                    <dx:GridViewBandColumn Caption="OCTUBRE" VisibleIndex="19" Visible="true">
                                        <Columns>
                                            <dx:GridViewDataSpinEditColumn FieldName="AFIS10"  Caption="FISICO" VisibleIndex="1" ReadOnly="false" Visible="true">
            <PropertiesSpinEdit DisplayFormatString="N2" />
        </dx:GridViewDataSpinEditColumn>
                                            <dx:GridViewDataSpinEditColumn FieldName="AFIN10"  Caption="FINANCIERO" VisibleIndex="2" ReadOnly="false" Visible="true">
            <PropertiesSpinEdit DisplayFormatString="N2" />
        </dx:GridViewDataSpinEditColumn>
                                            <dx:GridViewDataTextColumn FieldName="SFIS10" Caption="ESTADO" VisibleIndex="3" Visible="false"></dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn FieldName="SFIN10" Caption="ESTADO" VisibleIndex="4" Visible="false"></dx:GridViewDataTextColumn>
                                        </Columns>
                                    </dx:GridViewBandColumn>
                                    <dx:GridViewBandColumn Caption="NOVIEMBRE" VisibleIndex="20" Visible="true">
                                        <Columns>
                                            <dx:GridViewDataSpinEditColumn FieldName="AFIS11"  Caption="FISICO" VisibleIndex="1" ReadOnly="false" Visible="true">
            <PropertiesSpinEdit DisplayFormatString="N2" />
        </dx:GridViewDataSpinEditColumn>
                                            <dx:GridViewDataSpinEditColumn FieldName="AFIN11"  Caption="FINANCIERO" VisibleIndex="2" ReadOnly="false" Visible="true">
            <PropertiesSpinEdit DisplayFormatString="N2" />
        </dx:GridViewDataSpinEditColumn>
                                            <dx:GridViewDataTextColumn FieldName="SFIS11" Caption="ESTADO" VisibleIndex="3" Visible="false"></dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn FieldName="SFIN11" Caption="ESTADO" VisibleIndex="4" Visible="false"></dx:GridViewDataTextColumn>
                                        </Columns>
                                    </dx:GridViewBandColumn>
                                    <dx:GridViewBandColumn Caption="DICIEMBRE" VisibleIndex="21 " Visible="true">
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
                                            <dx:GridViewDataTextColumn FieldName="SFIS12" Caption="ESTADO" VisibleIndex="3" Visible="false"></dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn FieldName="SFIN12" Caption="ESTADO" VisibleIndex="4" Visible="false"></dx:GridViewDataTextColumn>
                                        </Columns>
                                    </dx:GridViewBandColumn>                           
                                    <dx:GridViewBandColumn Caption="CUATRIMESTRE 3" VisibleIndex="22" Visible="true">
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

                                    <dx:GridViewDataTextColumn  FieldName="PORFISNUMBER" VisibleIndex="23"  Visible="false" ReadOnly="true"/>
                                          <dx:GridViewDataTextColumn  FieldName="PORFINNUMBER" VisibleIndex="24"  Visible="false" ReadOnly="true"/>
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
                                                                    alertify.error(s.cpInfo).delay(10);
                                                                    delete s.cpInfo;
                                                                }
                                                             }           
                                                             "
                                            />
                        </dx:ASPxGridView>
                        </DetailRow>
                    </Templates>
                </dx:ASPxGridView>   
           <!-- </ContentTemplate>
        </asp:UpdatePanel>-->

             <dx:ASPxPopupControl ID="popAviso" runat="server" CssClass="pop" AllowDragging="true" Width="500px" Height="400" PopupHorizontalAlign="WindowCenter" HeaderText="Seguimiento de producción institucional/proyectos" PopupVerticalAlign="WindowCenter" AllowResize="true" Modal="true" ScrollBars="Vertical" >
                <HeaderStyle Font-Bold="true" Font-Size="X-Large" ForeColor="#003399" HorizontalAlign="Center" />
                <ContentCollection>
                    <dx:PopupControlContentControl>
                        <div>                          
                           
                            <div class="form-group">
                                <label>Se ha detectado que la institución puede realizar el seguimiento de proyectos SNIP GERO, por favor seleccione en una de las opciones el seguimiento que necesita realizar:</label>                                    
                                <dx:ASPxComboBox ID="cbSeguimiento" runat="server" ValueType="System.String" Theme="iOS" CssClass="list-group" Width="100%">
                                    <Items>
                                                <dx:ListEditItem Text="Registro de ejecución mensual POM-POA" Value="0" Selected="true" />
                                                <dx:ListEditItem Text="Seguimiento de proyectos SNIP GERO" Value="1" />
                                        </Items>
                                </dx:ASPxComboBox>                                                         
                            </div> 
                            <p>
                                <br />
                                <br />
                            </p>
                            <div class="form-group">                                
                                <div class="col-lg-4 col-lg-offset-1">
                                    <asp:Button ID="btnBuscaProduccion" runat="server" Text="Aceptar" CssClass="btn btn-success btn-block" OnClick="btnBuscaProduccion_Click"/>
                                </div>
                                <div class="col-lg-4 col-lg-offset-2">
                                    <asp:Button ID="btnCancelaBuscaProduccion" runat="server" Text="Cancelar" CssClass="btn btn-primary btn-block" OnClick="btnCancelaBuscaProduccion_Click"/>
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

             <dx:ASPxPopupControl ID="popSNIGERO" runat="server" CssClass="pop" AllowDragging="true" Width="500px" Height="350" PopupHorizontalAlign="WindowCenter" HeaderText="Seguimiento GERO" PopupVerticalAlign="WindowCenter" AllowResize="true" Modal="true" ScrollBars="Vertical" >
                <HeaderStyle Font-Bold="true" Font-Size="X-Large" ForeColor="#003399" HorizontalAlign="Center" />
                <ContentCollection>
                    <dx:PopupControlContentControl>
                        <div>
                             <label>Seleccione el periodo de la planificación en donde el proyeto SNIP esta formulado como subproducto y el año de avance físico del proyecto</label>  
                            <br />
                            <div class="form-group">
                                <label>Periodo de formulación en SIPLAN:</label>                                    
                                <dx:ASPxComboBox ID="cbPeriodoGERO" runat="server" ValueType="System.String" Width="100%" Theme="iOS" CssClass="list-group" OnSelectedIndexChanged="cbPeriodoGERO_SelectedIndexChanged" AutoPostBack ="true">
                                    <%--<Items>
                                        <dx:ListEditItem Text="2020 - 2024" Value="0" Selected="true"/>
                                    </Items>--%>
                                </dx:ASPxComboBox>
                            </div>
                            <div class="form-group">
                                <label>Institución:</label>                                    
                                <dx:ASPxComboBox ID="CbInstitucionesGERO" runat="server" ValueType="System.String" Width="100%" Theme="iOS" CssClass="list-group"></dx:ASPxComboBox>
                            </div> 
                            <div class="form-group">
                                <label>Año de avance del proyecto en SINIP:</label>                                    
                                <dx:ASPxComboBox ID="cbAnioPOAGERO" runat="server" ValueType="System.String" Theme="iOS" CssClass="list-group"></dx:ASPxComboBox>                                                         
                            </div> 
                            <p>
                                <br />
                                <br />
                            </p>
                            <div class="form-group">                                
                                <div class="col-lg-4 col-lg-offset-1">
                                    <asp:Button ID="btnCargaProduccionGERO" runat="server" Text="Aceptar" CssClass="btn btn-success btn-block" OnClick="btnCargaProduccionGERO_Click" />
                                </div>
                                <div class="col-lg-4 col-lg-offset-2">
                                    <asp:Button ID="btnCancelaProduccionGERO" runat="server" Text="Cancelar" CssClass="btn btn-primary btn-block" OnClick="btnCancelaProduccionGERO_Click" />
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

  
    <!--<asp:UpdatePanel ID="UpdatePanel2" runat="server">
        <ContentTemplate>-->
            <dx:ASPxPopupControl ID="wConsultaPOA" runat="server" CssClass="pop" AllowDragging="true" Width="500px" Height="350" PopupHorizontalAlign="WindowCenter" HeaderText="CONSULTA POA" PopupVerticalAlign="WindowCenter" AllowResize="true" Modal="true" ScrollBars="Vertical" >
                <HeaderStyle Font-Bold="true" Font-Size="X-Large" ForeColor="#003399" HorizontalAlign="Center" />
                <ContentCollection>
                    <dx:PopupControlContentControl>
                        <div>
                            <div class="form-group">
                                <label>Periodo:</label>                                    
                                <dx:ASPxComboBox ID="cbPeriodoPOA" runat="server" ValueType="System.String" Width="100%" Theme="iOS" CssClass="list-group">
                                    <%--<Items>
                                        <dx:ListEditItem Text="2020 - 2024" Value="0" Selected="true"/>
                                    </Items>--%>
                                </dx:ASPxComboBox>
                            </div>
                            <div class="form-group">
                                <label>Institución:</label>                                    
                                <dx:ASPxComboBox ID="cbInstPOA" runat="server" ValueType="System.String" Width="100%" Theme="iOS" CssClass="list-group"></dx:ASPxComboBox>
                            </div> 
                            <div class="form-group">
                                <label>POA:</label>                                    
                                <dx:ASPxComboBox ID="cbAnioPOA" runat="server" ValueType="System.String" Theme="iOS" CssClass="list-group"></dx:ASPxComboBox>                                                         
                            </div> 
                            <p>
                                <br />
                                <br />
                            </p>
                            <div class="form-group">                                
                                <div class="col-lg-4 col-lg-offset-1">
                                    <asp:Button ID="btnConsultaPOA" runat="server" Text="Aceptar" CssClass="btn btn-success btn-block" OnClick="btnConsultaPOA_Click"/>
                                </div>
                                <div class="col-lg-4 col-lg-offset-2">
                                    <asp:Button ID="btnCancelarPOA" runat="server" Text="Cancelar" CssClass="btn btn-primary btn-block" OnClick="btnCancelarPOA_Click"/>
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
        <!--</ContentTemplate>
    </asp:UpdatePanel>-->

    <!--<asp:UpdatePanel ID="UpdatePanel3" runat="server">
        <ContentTemplate>-->
            <dx:ASPxPopupControl ID="wEjecFisProducto" runat="server" CssClass="pop" AllowDragging="true" Width="900" Height="500" PopupHorizontalAlign="WindowCenter" HeaderText="PRODUCTOS EJECUCIÓN" PopupVerticalAlign="WindowCenter" AllowResize="true" Modal="true" ScrollBars="Vertical">
                <HeaderStyle Font-Bold="true" Font-Size="X-Large" ForeColor="#003399" HorizontalAlign="Center" />
                <ContentCollection>
                    <dx:PopupControlContentControl>
                        <div>
                            <h5><asp:Label ID="porcentaje" Visible ="false" runat="server"></asp:Label></h5>
                            <div class="row text-center bg-warning col-lg-10 col-lg-offset-1">
                                <label>CUATRIMESTRE I</label>
                            </div>
                            <div class="form-group row col-lg-12">
                                <div class="form-group col-lg-1 text-center" style="display:flex; flex-direction:column; justify-content:center; align-items:flex-start; height:100%; align-content:space-between; flex-grow:1;">
                                    <label>AVANCE</label><br />
                                    <label>Físico</label><br />                                    
                                    <label>Financiero</label>                                    
                                </div>
                                <div class="form-group col-lg-offset-1">                                
                                    <div class="form-group col-lg-2 text-center">
                                        <label>ENERO</label>
                                        <asp:TextBox ID="TextBox1" runat="server" CssClass="form-control" width="100%" onblur="sumafisicas1er()"  Enabled="false"></asp:TextBox>
                                        <asp:TextBox ID="txtFinM1" runat="server" CssClass="form-control text-right" onblur="sumafinancieras1er()" Enabled="false" ></asp:TextBox>
                                    </div>
                                    <div class="form-group col-lg-2 text-center">
                                        <label>FEBRERO</label>
                                        <asp:TextBox ID="TextBox2" runat="server" CssClass="form-control" onblur="sumafisicas1er()"  Enabled="false"></asp:TextBox> 
                                        <asp:TextBox ID="txtFinM2" runat="server" CssClass="form-control text-right"  onblur="sumafinancieras1er()" Enabled="false"   ></asp:TextBox>
                                    </div>
                                    <div class="form-group col-lg-2 text-center">
                                        <label>MARZO</label>
                                        <asp:TextBox ID="TextBox3" runat="server" CssClass="form-control"  onblur="sumafisicas1er()"  Enabled="false"></asp:TextBox>
                                        <asp:TextBox ID="txtFinM3" runat="server" CssClass="form-control text-right" onblur="sumafinancieras1er()" Enabled="false" ></asp:TextBox>
                                    </div>
                                    <div class="form-group col-lg-2 text-center">
                                        <label>ABRIL</label>
                                        <asp:TextBox ID="TextBox4" runat="server" CssClass="form-control" onblur="sumafisicas1er()"  Enabled="false"></asp:TextBox>
                                        <asp:TextBox ID="txtFinM4" runat="server" CssClass="form-control text-right" onblur="sumafinancieras1er()" Enabled="false"></asp:TextBox>
                                    </div>
                                    <div class="form-group col-lg-2 text-center">
                                        <label>ACUMULADO</label>
                                        <asp:TextBox ID="TextBox13" runat="server" CssClass="form-control text-right" Enabled="False"></asp:TextBox>
                                        <asp:TextBox ID="txtFinC1" runat="server" CssClass="form-control text-right" Enabled="false"></asp:TextBox>
                                    </div>
                                    <div class="form-group col-lg-2 text-center">
                                        <label>META</label>
                                        <asp:TextBox ID="txtMFisC1" runat="server" CssClass="form-control " Enabled="False"></asp:TextBox>
                                        <asp:TextBox ID="txtMFinC1" runat="server" CssClass="form-control text-right" Enabled="false"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="row text-center bg-warning col-lg-10 col-lg-offset-1">
                                <label>CUATRIMESTRE II</label>
                            </div>
                            <div class="form-group row col-lg-12">
                                <div class="form-group col-lg-1" style="display:flex; flex-direction:column; justify-content:center; align-items:flex-start; height:100%; align-content:space-between; flex-grow:1;">
                                    <label>&nbsp;</label><br />
                                    <label>Físico</label><br />                                    
                                    <label>Financiero</label>                                    
                                </div>
                                <div class="form-group col-lg-offset-1">
                                <div class="form-group col-lg-2 text-center">
                                    <label>MAYO</label>
                                    <asp:TextBox ID="TextBox5" runat="server" CssClass="form-control" onblur=" sumafisicas2d0()"  Enabled="false"></asp:TextBox>
                                    <asp:TextBox ID="txtFinM5" runat="server" CssClass="form-control text-right"  onblur="sumafinancieras2do()" Enabled="false"></asp:TextBox>
                                </div>
                                <div class="form-group col-lg-2 text-center">
                                    <label>JUNIO</label>
                                    <asp:TextBox ID="TextBox6" runat="server" CssClass="form-control"  onblur=" sumafisicas2do()" Enabled="false"></asp:TextBox>
                                    <asp:TextBox ID="txtFinM6" runat="server" CssClass="form-control text-right" Enabled="false" onblur="sumafinancieras2do()"></asp:TextBox>
                                </div>
                                <div class="form-group col-lg-2 text-center">
                                    <label>JULIO</label>
                                    <asp:TextBox ID="TextBox7" runat="server" CssClass="form-control" onblur=" sumafisicas2do()"  Enabled="false"></asp:TextBox>
                                    <asp:TextBox ID="txtFinM7" runat="server" CssClass="form-control text-right" Enabled="false" onblur="sumafinancieras2do()" ></asp:TextBox>
                                </div>
                                <div class="form-group col-lg-2 text-center">
                                    <label>AGOSTO</label>
                                    <asp:TextBox ID="TextBox8" runat="server" CssClass="form-control" onblur=" sumafisicas2do()" Enabled="false"></asp:TextBox>
                                    <asp:TextBox ID="txtFinM8" runat="server" CssClass="form-control text-right" Enabled="false"  onblur="sumafinancieras2do()"></asp:TextBox>
                                </div>
                                <div class="form-group col-lg-2 text-center">
                                    <label>ACUMULADO</label>
                                    <asp:TextBox ID="TextBox14" runat="server" CssClass="form-control text-right" Enabled="False"></asp:TextBox>
                                    <asp:TextBox ID="txtFinC2" runat="server" CssClass="form-control text-right" Enabled="false"></asp:TextBox>
                                </div>
                                <div class="form-group col-lg-2 text-center">
                                    <label>META</label>
                                    <asp:TextBox ID="txtMFisC2" runat="server" CssClass="form-control" Enabled="False"></asp:TextBox>
                                    <asp:TextBox ID="txtMFinC2" runat="server" CssClass="form-control text-right" Enabled="false" ></asp:TextBox>
                                </div>
                                </div>
                            </div>
                            <div class="row text-center bg-warning col-lg-10 col-lg-offset-1">
                                <label>CUATRIMESTRE III</label>
                            </div>
                            <div class="form-group row col-lg-12">
                                <div class="form-group col-lg-1" style="display:flex; flex-direction:column; justify-content:center; align-items:flex-start; height:100%; align-content:space-between; flex-grow:1;">
                                    <label>&nbsp;</label><br />
                                    <label>Físico</label><br />                                    
                                    <label>Financiero</label>                                    
                                </div>
                            <div class="form-group col-lg-offset-1">
                                <div class="form-group col-lg-2 text-center">
                                    <label>SEPTIEMBRE</label>
                                    <asp:TextBox ID="TextBox9" runat="server" CssClass="form-control" Enabled="false"  onblur="sumafisicas3er()"></asp:TextBox>
                                    <asp:TextBox ID="txtFinM9" runat="server" CssClass="form-control text-right" Enabled="false"   onblur="sumafinancieras3er()"></asp:TextBox>
                                </div>
                                <div class="form-group col-lg-2 text-center">
                                    <label>OCTUBRE</label>
                                    <asp:TextBox ID="TextBox10" runat="server" CssClass="form-control" Enabled="false" onblur="sumafisicas3er()"></asp:TextBox>
                                    <asp:TextBox ID="txtFinM10" runat="server" CssClass="form-control text-right" Enabled="false" onblur="sumafinancieras3er()"></asp:TextBox>
                                </div>
                                <div class="form-group col-lg-2 text-center">
                                    <label>NOVIEMBRE</label>
                                    <asp:TextBox ID="TextBox11" runat="server" CssClass="form-control"  Enabled="false" onblur="sumafisicas3er()"></asp:TextBox>
                                    <asp:TextBox ID="txtFinM11" runat="server" CssClass="form-control text-right" Enabled="false" onblur="sumafinancieras3er()"></asp:TextBox>
                                </div>
                                <div class="form-group col-lg-2 text-center">
                                    <label>DICIEMBRE</label>
                                    <asp:TextBox ID="TextBox12" runat="server" CssClass="form-control" Enabled="false" onblur="sumafisicas3er()"></asp:TextBox>
                                    <asp:TextBox ID="txtFinM12" runat="server" CssClass="form-control text-right" Enabled="false"  onblur="sumafinancieras3er()" ></asp:TextBox>
                                </div>
                                <div class="form-group col-lg-2 text-center">
                                    <label>ACUMULADO</label> 
                                    <asp:TextBox ID="TextBox15" runat="server" CssClass="form-control text-right" Enabled="False"></asp:TextBox>
                                    <asp:TextBox ID="txtFinC3" runat="server" CssClass="form-control text-right" Enabled="false"></asp:TextBox>
                                </div>
                                <div class="form-group col-lg-2 text-center">
                                    <label>META</label>
                                    <asp:TextBox ID="txtMFisC3" runat="server" CssClass="form-control" Enabled="False"></asp:TextBox>
                                    <asp:TextBox ID="txtMFinC3" runat="server" CssClass="form-control text-right" Enabled="false"></asp:TextBox>
                                </div>
                            </div>
                            </div>
                            <p>
                                <br />
                                <br />
                            </p>
                            <div class="form-group">                                
                                <div class="col-lg-4 col-lg-offset-1">
                                    <asp:Button ID="btnSaveEject" runat="server" Text="Guardar" CssClass="btn btn-success btn-block" OnClick="btnSaveEject_Click"/>
                                </div>
                                <div class="col-lg-4 col-lg-offset-2">
                                    <asp:Button ID="btnCancelEject" runat="server" Text="Cancelar" CssClass="btn btn-primary btn-block" OnClick="btnCancelEject_Click"/>
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
            
        <!--</ContentTemplate>
    </asp:UpdatePanel>-->


    
             </asp:View>
           <asp:View ID="View2" runat="server" >
               <h3>Gestión de fechas de cierre de cuatrimeste, para el ejercicio: <asp:Label ID="ejercicioVig" runat="server"></asp:Label></h3>
               <dx:ASPxPageControl ID="VistaFechas" Width="100%" runat="server" CssClass="dxtcFixed" ActiveTabIndex="0" EnableHierarchyRecreation="True"  Theme="Office2010Blue">
                   <TabPages>
                       <dx:TabPage Text="Fechas de cierre de ejecución cuatrimestral">
                            <ContentCollection>
                                <dx:ContentControl ID="fechasOrdinarias" runat="server">
                                    <h4>Estas fechas son validas para el ejercicio: <asp:Label runat="server" ID="ejercicioVigente"></asp:Label></h4>

                                    <dx:ASPxGridView ID="gvFechasCierre" runat="server" KeyFieldName="NUMERO"   Theme="Office2010Blue" Width="40%" AutoGenerateColumns="true" SettingsBehavior-AllowFocusedRow ="true" SettingsBehavior-ConfirmDelete="true"  OnRowDeleting="gvFechasCierre_RowDeleting"  OnRowUpdating="gvFechasCierre_RowUpdating" OnCellEditorInitialize ="gvFechasCierre_CellEditorInitialize">
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
                                        <UpdateButton ButtonType="Button" Text="Actualizar">
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
                                            <dx:GridViewDataTextColumn FieldName="NUMERO"  Name="NUMERO" ReadOnly="true" Visible="false" VisibleIndex="0" Width="0">
                     <HeaderStyle Wrap="True" />
                     <Settings />
                 </dx:GridViewDataTextColumn>

                                           
                                            <dx:GridViewDataTextColumn FieldName="EJERCICIO"  Name="EJERCICIO" Caption="Ejercicio" ReadOnly="true" Visible="true" VisibleIndex="1" Width="22%">
                     <HeaderStyle Wrap="True" />
                     <Settings />
                 </dx:GridViewDataTextColumn>

                                            <dx:GridViewDataTextColumn FieldName="CUATRIMESTRE"  Name="CUATRIMESTRE" Caption="Cuatrimestre" ReadOnly="true" Visible="true" VisibleIndex="2" Width="22%">
                     <HeaderStyle Wrap="True" />
                     <Settings />
                 </dx:GridViewDataTextColumn>

                                             <dx:GridViewDataDateColumn FieldName="SPFC$FECHA_INICIO"  Name="SPFC$FECHA_INICIO" Caption ="Fecha inicio" ReadOnly="false" Visible="true" VisibleIndex="3" Width="22%">
                     <HeaderStyle Wrap="True" />
                                                   <PropertiesDateEdit 
        DisplayFormatString="dd/MM/yyyy"
        EditFormatString="dd/MM/yyyy"
        EditFormat="Date">
    </PropertiesDateEdit>
                     <Settings />
                 </dx:GridViewDataDateColumn>
                                            <dx:GridViewDataDateColumn FieldName="SPFC$FECHA_CIERRE"  Name="SPFC$FECHA_CIERRE" Caption="Fecha cierre" ReadOnly="false" Visible="true" VisibleIndex="4" Width="22%">
                     <HeaderStyle Wrap="True" />
                                                  <PropertiesDateEdit 
        DisplayFormatString="dd/MM/yyyy"
        EditFormatString="dd/MM/yyyy"
        EditFormat="Date">
    </PropertiesDateEdit>
                     <Settings />
                 </dx:GridViewDataDateColumn>
                                              <dx:GridViewCommandColumn ShowDeleteButton="true" ShowEditButton="true" ShowNewButtonInHeader="false" VisibleIndex="5" Width="12%">
                     <HeaderStyle Wrap="True" />
                 </dx:GridViewCommandColumn>
                                         </Columns>
                                    </dx:ASPxGridView>

                                    </dx:ContentControl>
                             </ContentCollection>
                        </dx:TabPage>
                       <dx:TabPage Text="Prorroga de fechas de cierre de cuatrimestres, para instituciones">
                            <ContentCollection>
                                <dx:ContentControl ID="fechasExtraordinarias" runat="server">
                                    <h4>Estas fechas de prorroga, son validas para el ejercicio: <asp:Label runat="server" ID="ejerciciVigenteInsto"></asp:Label></h4>

                                    <dx:ASPxGridView ID="gvFechasExtrordinarias" runat="server" KeyFieldName="NUMERO"   Theme="Office2010Blue" Width="100%" AutoGenerateColumns="true" SettingsBehavior-AllowFocusedRow ="true" SettingsBehavior-ConfirmDelete="true"  >
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
                                            <dx:GridViewDataTextColumn FieldName="NUMERO"  Name="NUMERO" ReadOnly="true" Visible="false" VisibleIndex="0" Width="0">
                     <HeaderStyle Wrap="True" />
                     <Settings />
                 </dx:GridViewDataTextColumn>

                                           
                                            <dx:GridViewDataTextColumn FieldName="EJERCICIO"  Name="EJERCICIO" Caption="Ejercicio" ReadOnly="true" Visible="true" VisibleIndex="1" Width="0">
                     <HeaderStyle Wrap="True" />
                     <Settings />
                 </dx:GridViewDataTextColumn>

                                            <dx:GridViewDataTextColumn FieldName="CUATRIMESTRE"  Name="CUATRIMESTRE" Caption="Cuatrimestre" ReadOnly="true" Visible="true" VisibleIndex="2" Width="0">
                     <HeaderStyle Wrap="True" />
                     <Settings />
                 </dx:GridViewDataTextColumn>

                                             
                                            <dx:GridViewDataDateColumn FieldName="SPFC$FECHA_CIERRE"  Name="SPFC$FECHA_CIERRE" Caption="Fecha cierre" ReadOnly="false" Visible="true" VisibleIndex="3" Width="0">
                     <HeaderStyle Wrap="True" />
                     <Settings />
                 </dx:GridViewDataDateColumn>
                                            <dx:GridViewDataDateColumn FieldName="SPFC$POM"  Name="SPFC$POM" ReadOnly="true" Visible="false" VisibleIndex="4" Width="0">
                     <HeaderStyle Wrap="True" />
                     <Settings />
                 </dx:GridViewDataDateColumn>
                                            <dx:GridViewDataDateColumn FieldName="SPFC$INSTITUCION"  Name="SPFC$INSTITUCION" ReadOnly="true" Visible="false" VisibleIndex="5" Width="0">
                     <HeaderStyle Wrap="True" />
                     <Settings />
                 </dx:GridViewDataDateColumn>
                                            <dx:GridViewDataDateColumn FieldName="INSTITUCION"  Name="INSTITUCION"  Caption = "Institución" ReadOnly="true" Visible="true" VisibleIndex="6" Width="0">
                     <HeaderStyle Wrap="True" />
                     <Settings />
                 </dx:GridViewDataDateColumn>
                                              <dx:GridViewCommandColumn ShowDeleteButton="true" ShowEditButton="true" ShowNewButtonInHeader="false" VisibleIndex="7">
                     <HeaderStyle Wrap="True" />
                 </dx:GridViewCommandColumn>
                                         </Columns>
                                    </dx:ASPxGridView>
                                    </dx:ContentControl>
                             </ContentCollection>
                        </dx:TabPage>
                    </TabPages>
               </dx:ASPxPageControl>
               </asp:View>
          <asp:View ID="VistaGero" runat="server">
               <h3>Seguimiento de proyectos de inversión GERO  <asp:Label ID="lblInstoGero" runat="server"></asp:Label></h3>
              <div class="btn-group" style="display:flex; justify-content:flex-start;">                                  
                                              
                                     <asp:Button ID="btnMedidas" runat="server" Text="Unidad de medida a varios proyectos"  CssClass="btn btn-primary" ToolTip="Asignar unidad de medida a varios proyectos" OnClick="btnMedidas_Click"/> 
                                     <asp:Button ID="btnRegresaInicio" runat="server" Text="Regresar a pantalla principal"  CssClass="btn btn-success" ToolTip="Regresar a pantalla principal" OnClick="btnRegresaInicio_Click" /> 
                  </div>
               <dx:ASPxPageControl ID="viewGero" Width="100%" runat="server" CssClass="dxtcFixed" ActiveTabIndex="2" EnableHierarchyRecreation="True"  Theme="Office2010Blue">
                  <TabPages>
                       <dx:TabPage Text="Seguimiento de metas proyectos de inversión GERO">
                            <ContentCollection>
                                <dx:ContentControl ID="coontentGERO" runat="server">
                                    <!--inicio de grid-->
                                    <div class="form-group">
                                        <label>Año seguimiento GERO:</label>                                    
                                <dx:ASPxComboBox ID="cbAnioSeguimientoGERO" runat="server" ValueType="System.String" Width="9%" Theme="iOS" CssClass="list-group"  OnSelectedIndexChanged="cbAnioSeguimientoGERO_SelectedIndexChanged" AutoPostBack="true" ></dx:ASPxComboBox>
                                        </div>
                                     <dx:ASPxGridView ID="gvProyectosGERO" runat="server" AutoGenerateColumns="False" KeyFieldName="SPPSUB$ID_SUBPRODUCTO" SettingsBehavior-AllowFocusedRow ="true" Width="100%" Theme="Office2010Blue" OnHtmlRowPrepared="gvProyectosGERO_HtmlRowPrepared" OnCustomColumnSort="gvProyectosGERO_CustomColumnSort">
                    <SettingsBehavior AllowFixedGroups="True" AllowFocusedRow="True" />
                         <Settings ShowFilterRow="True" ShowGroupPanel="True" />
                    <Columns>
                          <dx:GridViewCommandColumn ShowSelectCheckbox="True" SelectAllCheckboxMode="Page" Width="4%" VisibleIndex="0">
                                </dx:GridViewCommandColumn>
                        <dx:GridViewDataTextColumn Caption="Acciones" VisibleIndex="1" FieldName="ACCIONES">
                            <DataItemTemplate>
                                <asp:Button ID="btnRegistra" Text="Metas" runat="server" CssClass="btn btn-light-meta" OnClick="btnGuardarGERO_Click"/>
                                 <asp:Button ID="btnAvances" Text="Avances" runat="server" CssClass="btn btn-light-gray" OnClick="btnAvances_Click"/>
                            </DataItemTemplate>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn FieldName="ID_TIPO" VisibleIndex="2" Visible="false"></dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn FieldName="TIPO" VisibleIndex="3" GroupIndex="0"></dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn FieldName="ID_RPRESUP" VisibleIndex="4" Visible="false"></dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn FieldName="RPRESUP" Caption="PROGRAMA PRESUPUESTARIO" VisibleIndex ="5" Visible="false"></dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn FieldName="ID_RESULTADO" VisibleIndex="6" Visible="false"></dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn FieldName="RESULTADO" VisibleIndex="7" Width="30%">
                            <Settings AutoFilterCondition="Contains" />
                        </dx:GridViewDataTextColumn>                        
                        <dx:GridViewDataTextColumn FieldName="ID_PRODUCTO" VisibleIndex="8" Visible="false"></dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn FieldName="PRODUCTO" VisibleIndex="9" Width="30%" Caption="Producto">
                            <Settings AutoFilterCondition="Contains" />
                        </dx:GridViewDataTextColumn>
                       
                         <dx:GridViewDataTextColumn FieldName="SPPSUB$ID_SUBPRODUCTO" VisibleIndex="10" Width="30%" Visible="false">                            
                        </dx:GridViewDataTextColumn>

                            <dx:GridViewDataTextColumn FieldName="SPOA$ANIO" VisibleIndex="11" Width="30%" Caption="Año avance SNIP">
                            <Settings AutoFilterCondition="Contains" />
                        </dx:GridViewDataTextColumn>


                            <dx:GridViewDataTextColumn FieldName="ANIO_GERO" VisibleIndex="12" Width="30%" Caption="Año seguimiento GERO">
                            <Settings AutoFilterCondition="Contains" />
                        </dx:GridViewDataTextColumn>

                          <dx:GridViewDataTextColumn FieldName="SPPSUB$SNIP" VisibleIndex="13" Width="30%" Caption="SNIP">
                            <Settings AutoFilterCondition="Contains" />
                        </dx:GridViewDataTextColumn>

                           <dx:GridViewDataTextColumn FieldName="PROYECTO" VisibleIndex="14" Width="30%" Caption="Proyecto">
                            <Settings AutoFilterCondition="Contains" />
                        </dx:GridViewDataTextColumn>

                         <dx:GridViewBandColumn Caption="Metas y avance físico SINIP" VisibleIndex="15">
                             <Columns>
                                  <dx:GridViewDataTextColumn FieldName="MEDIDASNIP" Caption="Medida SINIP" VisibleIndex="0" Width="10%">
                                    <CellStyle HorizontalAlign="Right"></CellStyle>
                                       <CellStyle CssClass="productColumn" />
                                      <Settings AutoFilterCondition="Contains" />
                                </dx:GridViewDataTextColumn>
                                 <dx:GridViewDataTextColumn FieldName="METAFISICASNIP" Caption="Meta física SNIP" VisibleIndex="1" Width="10%">
                                    <CellStyle HorizontalAlign="Right"></CellStyle>
                                      <CellStyle CssClass="productColumn" />
                                     <Settings AutoFilterCondition="Contains" />
                                </dx:GridViewDataTextColumn>
                                  <dx:GridViewDataTextColumn FieldName="AVANCE_FISICO" Caption="Avance físico" VisibleIndex="2" Width="10%">
                                    <CellStyle HorizontalAlign="Right"></CellStyle>
                                       <CellStyle CssClass="productColumn" />
                                     <Settings AutoFilterCondition="Contains" />
                                </dx:GridViewDataTextColumn>
                                   <dx:GridViewDataTextColumn FieldName="POREJECTFIS" Caption="% Avance físico" VisibleIndex="2" Width="10%">
                                    <CellStyle HorizontalAlign="Right"></CellStyle>
                                        <CellStyle CssClass="productColumn" />
                                     <Settings AutoFilterCondition="Contains" />
                                </dx:GridViewDataTextColumn>
                                 <dx:GridViewDataTextColumn FieldName="POREJECTFISNUM" VisibleIndex="4" Width="10%" Visible="false">
                                    <CellStyle HorizontalAlign="Right"></CellStyle>
                                      <CellStyle CssClass="productColumn" />
                                     <Settings AutoFilterCondition="Contains" />
                                </dx:GridViewDataTextColumn>

                             </Columns>
                          </dx:GridViewBandColumn>

                        <dx:GridViewBandColumn Caption="Asignado y avance financiero SINIP" VisibleIndex="16">
                             <Columns>                                  
                                 <dx:GridViewDataTextColumn FieldName="NASIGNADOTOTAL" Caption="Asignado a proyecto" VisibleIndex="0" Width="10%">
                                    <CellStyle HorizontalAlign="Right"></CellStyle>
                                      <CellStyle CssClass="productColumn" />
                                     <Settings AutoFilterCondition="Contains" />
                                </dx:GridViewDataTextColumn>
                                   <dx:GridViewDataTextColumn FieldName="NEJECUTADOTOTAL" Caption="Avance financiero" VisibleIndex="1" Width="10%">
                                    <CellStyle HorizontalAlign="Right"></CellStyle>
                                        <CellStyle CssClass="productColumn" />
                                     <Settings AutoFilterCondition="Contains" />
                                </dx:GridViewDataTextColumn>

                                 <dx:GridViewDataTextColumn FieldName="PFIN" Caption="% Avance financiero" VisibleIndex="2" Width="10%">
                                    <CellStyle HorizontalAlign="Right"></CellStyle>
                                      <CellStyle CssClass="productColumn" />
                                     <Settings AutoFilterCondition="Contains" />
                                </dx:GridViewDataTextColumn>

                                  <dx:GridViewDataTextColumn FieldName="PFINNUMBER"  VisibleIndex="3"  Visible="false" Width="10%">
                                    <CellStyle HorizontalAlign="Right"></CellStyle>
                                       <CellStyle CssClass="productColumn" />
                                     <Settings AutoFilterCondition="Contains" />
                                </dx:GridViewDataTextColumn>     
                                 

                             </Columns>
                          </dx:GridViewBandColumn>


                        <dx:GridViewBandColumn Caption="Metas y avance físico Seguimiento GERO" VisibleIndex="17">
                              <Columns>
                                  <dx:GridViewDataTextColumn FieldName="MEDIDA_GERO" Caption="Medida GERO" VisibleIndex="0" Width="10%">
                                    <CellStyle HorizontalAlign="Right"></CellStyle>
                                     <Settings AutoFilterCondition="Contains" />
                                </dx:GridViewDataTextColumn>
                                  <dx:GridViewDataTextColumn FieldName="MFGERO" Caption="Meta física" VisibleIndex="1" Width="10%">
                                    <CellStyle HorizontalAlign="Right"></CellStyle>
                                     <Settings AutoFilterCondition="Contains" />
                                </dx:GridViewDataTextColumn>
                                  <dx:GridViewDataTextColumn FieldName="AFISGANUAL" Caption="Avance físico" VisibleIndex="2" Width="10%">
                                    <CellStyle HorizontalAlign="Right"></CellStyle>
                                     <Settings AutoFilterCondition="Contains" />
                                </dx:GridViewDataTextColumn>
                                   <dx:GridViewDataTextColumn FieldName="PAFISGANUAL" Caption="% Avance físico" VisibleIndex="2" Width="10%">
                                    <CellStyle HorizontalAlign="Right"></CellStyle>
                                     <Settings AutoFilterCondition="Contains" />
                                </dx:GridViewDataTextColumn>

                                  </Columns>
             </dx:GridViewBandColumn>

                        <dx:GridViewBandColumn Caption="Metas y avance financiero Seguimiento GERO" VisibleIndex="18">
                                  <Columns>                                  
                                
                                   <dx:GridViewDataTextColumn FieldName="MFINGERO" Caption="Meta financiera" VisibleIndex="0" Width="10%">
                                    <CellStyle HorizontalAlign="Right"></CellStyle>
                                     <Settings AutoFilterCondition="Contains" />
                                </dx:GridViewDataTextColumn>
                                  <dx:GridViewDataTextColumn FieldName="AFINGANUAL" Caption="Avance financiero anual GERO" VisibleIndex="1" Width="10%">
                                    <CellStyle HorizontalAlign="Right"></CellStyle>
                                     <Settings AutoFilterCondition="Contains" />
                                </dx:GridViewDataTextColumn>

                                 <dx:GridViewDataTextColumn FieldName="PAFINGANUAL" Caption="% Avance financiero anual GERO" VisibleIndex="2" Width="10%">
                                    <CellStyle HorizontalAlign="Right"></CellStyle>
                                     <Settings AutoFilterCondition="Contains" />
                                </dx:GridViewDataTextColumn>

                             </Columns>
                            </dx:GridViewBandColumn>

                         <dx:GridViewBandColumn Caption="enero" VisibleIndex="19">
                                        <Columns> 
                                            <dx:GridViewDataSpinEditColumn FieldName="AFIS1"  Caption="FISICO" VisibleIndex="0"  Visible="true">
            <PropertiesSpinEdit DisplayFormatString="N2" />
        </dx:GridViewDataSpinEditColumn>
                                            <dx:GridViewDataSpinEditColumn FieldName="AFIN1"  Caption="FINANCIERO" VisibleIndex="1" Visible="true">
            <PropertiesSpinEdit DisplayFormatString="N2" />
        </dx:GridViewDataSpinEditColumn>
                                            
                                        </Columns>
                                    </dx:GridViewBandColumn>

                          <dx:GridViewBandColumn Caption="febrero" VisibleIndex="20">
                                        <Columns> 
                                            <dx:GridViewDataSpinEditColumn FieldName="AFIS2"  Caption="FISICO" VisibleIndex="0"  Visible="true">
            <PropertiesSpinEdit DisplayFormatString="N2" />
        </dx:GridViewDataSpinEditColumn>
                                            <dx:GridViewDataSpinEditColumn FieldName="AFIN2"  Caption="FINANCIERO" VisibleIndex="1" Visible="true">
            <PropertiesSpinEdit DisplayFormatString="N2" />
        </dx:GridViewDataSpinEditColumn>
                                            
                                        </Columns>
                                    </dx:GridViewBandColumn>

                         <dx:GridViewBandColumn Caption="marzo" VisibleIndex="21">
                                        <Columns> 
                                            <dx:GridViewDataSpinEditColumn FieldName="AFIS3"  Caption="FISICO" VisibleIndex="0"  Visible="true">
            <PropertiesSpinEdit DisplayFormatString="N2" />
        </dx:GridViewDataSpinEditColumn>
                                            <dx:GridViewDataSpinEditColumn FieldName="AFIN3"  Caption="FINANCIERO" VisibleIndex="1" Visible="true">
            <PropertiesSpinEdit DisplayFormatString="N2" />
        </dx:GridViewDataSpinEditColumn>
                                            
                                        </Columns>
                                    </dx:GridViewBandColumn>

                        
                         <dx:GridViewBandColumn Caption="abril" VisibleIndex="22">
                                        <Columns> 
                                            <dx:GridViewDataSpinEditColumn FieldName="AFIS4"  Caption="FISICO" VisibleIndex="0"  Visible="true">
            <PropertiesSpinEdit DisplayFormatString="N2" />
        </dx:GridViewDataSpinEditColumn>
                                            <dx:GridViewDataSpinEditColumn FieldName="AFIN4"  Caption="FINANCIERO" VisibleIndex="1" Visible="true">
            <PropertiesSpinEdit DisplayFormatString="N2" />
        </dx:GridViewDataSpinEditColumn>
                                            
                                        </Columns>
                                    </dx:GridViewBandColumn>

                         <dx:GridViewBandColumn Caption="mayo" VisibleIndex="23">
                                        <Columns> 
                                            <dx:GridViewDataSpinEditColumn FieldName="AFIS5"  Caption="FISICO" VisibleIndex="0"  Visible="true">
            <PropertiesSpinEdit DisplayFormatString="N2" />
        </dx:GridViewDataSpinEditColumn>
                                            <dx:GridViewDataSpinEditColumn FieldName="AFIN5"  Caption="FINANCIERO" VisibleIndex="1" Visible="true">
            <PropertiesSpinEdit DisplayFormatString="N2" />
        </dx:GridViewDataSpinEditColumn>
                                            
                                        </Columns>
                                    </dx:GridViewBandColumn>

                        
                        <dx:GridViewBandColumn Caption="junio" VisibleIndex="24">
                                        <Columns> 
                                            <dx:GridViewDataSpinEditColumn FieldName="AFIS6"  Caption="FISICO" VisibleIndex="0"  Visible="true">
            <PropertiesSpinEdit DisplayFormatString="N2" />
        </dx:GridViewDataSpinEditColumn>
                                            <dx:GridViewDataSpinEditColumn FieldName="AFIN6"  Caption="FINANCIERO" VisibleIndex="1" Visible="true">
            <PropertiesSpinEdit DisplayFormatString="N2" />
        </dx:GridViewDataSpinEditColumn>
                                            
                                        </Columns>
                                    </dx:GridViewBandColumn>

                         <dx:GridViewBandColumn Caption="julio" VisibleIndex="25">
                                        <Columns> 
                                            <dx:GridViewDataSpinEditColumn FieldName="AFIS7"  Caption="FISICO" VisibleIndex="0"  Visible="true">
            <PropertiesSpinEdit DisplayFormatString="N2" />
        </dx:GridViewDataSpinEditColumn>
                                            <dx:GridViewDataSpinEditColumn FieldName="AFIN7"  Caption="FINANCIERO" VisibleIndex="1" Visible="true">
            <PropertiesSpinEdit DisplayFormatString="N2" />
        </dx:GridViewDataSpinEditColumn>
                                            
                                        </Columns>
                                    </dx:GridViewBandColumn>

                          <dx:GridViewBandColumn Caption="agosto" VisibleIndex="26">
                                        <Columns> 
                                            <dx:GridViewDataSpinEditColumn FieldName="AFIS8"  Caption="FISICO" VisibleIndex="0"  Visible="true">
            <PropertiesSpinEdit DisplayFormatString="N2" />
        </dx:GridViewDataSpinEditColumn>
                                            <dx:GridViewDataSpinEditColumn FieldName="AFIN8"  Caption="FINANCIERO" VisibleIndex="1" Visible="true">
            <PropertiesSpinEdit DisplayFormatString="N2" />
        </dx:GridViewDataSpinEditColumn>
                                            
                                        </Columns>
                                    </dx:GridViewBandColumn>

                          <dx:GridViewBandColumn Caption="septiembre" VisibleIndex="27">
                                        <Columns> 
                                            <dx:GridViewDataSpinEditColumn FieldName="AFIS9"  Caption="FISICO" VisibleIndex="0"  Visible="true">
            <PropertiesSpinEdit DisplayFormatString="N2" />
        </dx:GridViewDataSpinEditColumn>
                                            <dx:GridViewDataSpinEditColumn FieldName="AFIN9"  Caption="FINANCIERO" VisibleIndex="1" Visible="true">
            <PropertiesSpinEdit DisplayFormatString="N2" />
        </dx:GridViewDataSpinEditColumn>
                                            
                                        </Columns>
                                    </dx:GridViewBandColumn>

                              <dx:GridViewBandColumn Caption="octubre" VisibleIndex="28">
                                        <Columns> 
                                            <dx:GridViewDataSpinEditColumn FieldName="AFIS10"  Caption="FISICO" VisibleIndex="0"  Visible="true">
            <PropertiesSpinEdit DisplayFormatString="N2" />
        </dx:GridViewDataSpinEditColumn>
                                            <dx:GridViewDataSpinEditColumn FieldName="AFIN10"  Caption="FINANCIERO" VisibleIndex="1" Visible="true">
            <PropertiesSpinEdit DisplayFormatString="N2" />
        </dx:GridViewDataSpinEditColumn>
                                            
                                        </Columns>
                                    </dx:GridViewBandColumn>

                           <dx:GridViewBandColumn Caption="noviembre" VisibleIndex="29">
                                        <Columns> 
                                            <dx:GridViewDataSpinEditColumn FieldName="AFIS11"  Caption="FISICO" VisibleIndex="0"  Visible="true">
            <PropertiesSpinEdit DisplayFormatString="N2" />
        </dx:GridViewDataSpinEditColumn>
                                            <dx:GridViewDataSpinEditColumn FieldName="AFIN11"  Caption="FINANCIERO" VisibleIndex="1" Visible="true">
            <PropertiesSpinEdit DisplayFormatString="N2" />
        </dx:GridViewDataSpinEditColumn>
                                            
                                        </Columns>
                                    </dx:GridViewBandColumn>

                            <dx:GridViewBandColumn Caption="diciembre" VisibleIndex="30">
                                        <Columns> 
                                            <dx:GridViewDataSpinEditColumn FieldName="AFIS12"  Caption="FISICO" VisibleIndex="0"  Visible="true">
            <PropertiesSpinEdit DisplayFormatString="N2" />
        </dx:GridViewDataSpinEditColumn>
                                            <dx:GridViewDataSpinEditColumn FieldName="AFIN12"  Caption="FINANCIERO" VisibleIndex="1" Visible="true">
            <PropertiesSpinEdit DisplayFormatString="N2" />
        </dx:GridViewDataSpinEditColumn>
                                            
                                        </Columns>
                                    </dx:GridViewBandColumn>

                         <dx:GridViewDataTextColumn FieldName="SPPSUB$GERO" VisibleIndex="31" Visible="false"></dx:GridViewDataTextColumn>
                         <dx:GridViewDataTextColumn FieldName="POA" VisibleIndex="32" Visible="false"></dx:GridViewDataTextColumn>

                        <dx:GridViewDataTextColumn FieldName="ENC1" VisibleIndex="33" Visible="false"></dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn FieldName="ENC2" VisibleIndex="34" Visible="false"></dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn FieldName="ENC3" VisibleIndex="35" Visible="false"></dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn FieldName="ENC4" VisibleIndex="36" Visible="false"></dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn FieldName="ENC5" VisibleIndex="37" Visible="false"></dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn FieldName="ENC6" VisibleIndex="38" Visible="false"></dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn FieldName="ENC7" VisibleIndex="39" Visible="false"></dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn FieldName="ENC8" VisibleIndex="40" Visible="false"></dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn FieldName="ENC9" VisibleIndex="41" Visible="false"></dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn FieldName="ENC10" VisibleIndex="42" Visible="false"></dx:GridViewDataTextColumn>
                       <dx:GridViewDataTextColumn FieldName="ENC11" VisibleIndex="43" Visible="false"></dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn FieldName="ENC12" VisibleIndex="44" Visible="false"></dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn FieldName="SPPSUB$MEDIDA_GERO" VisibleIndex="45" Visible="false"></dx:GridViewDataTextColumn>
                         
                       
                       
                        
                    </Columns>
                    
                </dx:ASPxGridView>   



                                    <!--fin de grid-->
                                      </dx:ContentControl>
                                  </ContentCollection>
                     </dx:TabPage>
                        </TabPages>
                    </dx:ASPxPageControl>

            
            <dx:ASPxPopupControl ID="MedidaGero" runat="server" CssClass="pop" AllowDragging="true" Width="700px" Height="400" PopupHorizontalAlign="WindowCenter" HeaderText="Medida y metas GERO" PopupVerticalAlign="WindowCenter" AllowResize="true" Modal="true" ScrollBars="Vertical" >
                <HeaderStyle Font-Bold="true" Font-Size="X-Large" ForeColor="#003399" HorizontalAlign="Center" />
                <ContentCollection>
                    <dx:PopupControlContentControl>
                        <div>
                            <div class="form-group">
                                <label>Proyecto: <asp:label runat="server" ID="proyectoGERO" CssClass="text-danger fw-bold"/> </label>
                                <br />
                                <label>Ejercicio/año de avance SINIP: <asp:label runat="server" ID="anioGERO" CssClass="text-danger fw-bold"/> </label>
                                <br />
                                <label>Año seguimiento GERO:</label>                                    
                                <dx:ASPxComboBox ID="cbEjercicioGERO" runat="server" ValueType="System.String" Width="100%" Theme="iOS" CssClass="list-group" Enabled ="false"></dx:ASPxComboBox>
                                <label>Unidad de medida:</label>                                    
                                <dx:ASPxComboBox ID="cbMedidaBGERO" runat="server"  ValueType="System.String" NullText="Seleccione la unidad de medida" CssClass="form-control"></dx:ASPxComboBox>
                                  
                            </div>
                        

                            <div class="form-group">
                                <label>Meta Física:</label> <asp:TextBox runat="server" ID="txtMetaFisica" onkeypress="return soloNumeros(event)" CssClass="form-control"/>
                            </div>
                            <div class="form-group">
                                <label>Meta financiera:</label> 
                           <asp:TextBox runat="server" ID="txtFinancieraGERO" onkeypress="return soloNumeros(event)" CssClass="form-control"/>
                               </div>                       
                               
                            
                            <p>
                                <br />
                                <br />
                            </p>
                            <div class="form-group">                                
                                <div class="col-lg-4 col-lg-offset-1">
                                    <asp:Button ID="btnGuardarGERO" runat="server" Text="Aceptar" CssClass="btn btn-success btn-block"  OnClick="btnGuardarGERO_Click2"/>
                                </div>
                                <div class="col-lg-4 col-lg-offset-2">
                                    <asp:Button ID="btnCancelaGERO" runat="server" Text="Cancelar" CssClass="btn btn-primary btn-block" OnClick="btnCancelaGERO_Click"/>
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
    


              <dx:ASPxPopupControl ID="popMedidaVarios" runat="server" CssClass="pop" AllowDragging="true" Width="700px" Height="400" PopupHorizontalAlign="WindowCenter" HeaderText="Asignar unidad de medida para varios proyectos" PopupVerticalAlign="WindowCenter" AllowResize="true" Modal="true" ScrollBars="Vertical" >
                <HeaderStyle Font-Bold="true" Font-Size="X-Large" ForeColor="#003399" HorizontalAlign="Center" />
                <ContentCollection>
                    <dx:PopupControlContentControl>
                        <div>
                            <div class="form-group">
                                <label>Ejercicio/año: <asp:label runat="server" ID="lblAnioVarios" CssClass="text-danger fw-bold"/> </label>
                                <br />
                                <label>Año seguimiento GERO:</label>                                    
                                <dx:ASPxComboBox ID="cbAnioVarios" runat="server" ValueType="System.String" Width="100%" Theme="iOS" CssClass="list-group" Enabled ="false"></dx:ASPxComboBox>
                                <label>Unidad de medida:</label>                                    
                                <dx:ASPxComboBox ID="cbUnidadMedidaVarios" runat="server"  ValueType="System.String" NullText="Seleccione la unidad de medida" CssClass="form-control"></dx:ASPxComboBox>
                                  
                            </div>
                        
                            <p>
                                <br />
                                <br />
                            </p>
                            <div class="form-group">                                
                                <div class="col-lg-4 col-lg-offset-1">
                                    <asp:Button ID="btnVariosMed" runat="server" Text="Aceptar" CssClass="btn btn-success btn-block" OnClick="btnVariosMed_Click"/>
                                </div>
                                <div class="col-lg-4 col-lg-offset-2">
                                    <asp:Button ID="btnCierraVa" runat="server" Text="Cancelar" CssClass="btn btn-primary btn-block" OnClick="btnCierraVa_Click"/>
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
              


               <dx:ASPxPopupControl ID="popAvanceGERO" runat="server" CssClass="pop" AllowDragging="true" Width="900" Height="500" PopupHorizontalAlign="WindowCenter" HeaderText="Avance de proyectos GERO" PopupVerticalAlign="WindowCenter" AllowResize="true" Modal="true" ScrollBars="Vertical">
                <HeaderStyle Font-Bold="true" Font-Size="X-Large" ForeColor="#003399" HorizontalAlign="Center" />
                <ContentCollection>
                    <dx:PopupControlContentControl>
                        <div>
                            <h5>Proyecto: <asp:Label ID="lbl_subproducto"  runat="server"></asp:Label></h5>                              
                            <h5>Año del proyecto: <asp:Label ID="lbl_anio"  runat="server"></asp:Label></h5>
                            <h5>Año seguimiento GERO: <asp:Label ID="lblANIOGERO"  runat="server"></asp:Label></h5>
                             <h5>Meta física: <asp:Label ID="lbl_medida"  runat="server"></asp:Label></h5>
                             <h5>Meta financiera: <asp:Label ID="lbl_meta_financiera"  runat="server"></asp:Label></h5>
                            
                            <asp:HiddenField ID="ENC1" runat="server" />
                           <asp:HiddenField ID="ENC2" runat="server" />
                            <asp:HiddenField ID="ENC3" runat="server" />
                            <asp:HiddenField ID="ENC4" runat="server" />
                            <asp:HiddenField ID="ENC5" runat="server" />
                            <asp:HiddenField ID="ENC6" runat="server" />
                            <asp:HiddenField ID="ENC7" runat="server" />
                            <asp:HiddenField ID="ENC8" runat="server" />
                            <asp:HiddenField ID="ENC9" runat="server" />
                            <asp:HiddenField ID="ENC10" runat="server" />
                            <asp:HiddenField ID="ENC11" runat="server" />
                            <asp:HiddenField ID="ENC12" runat="server" />


                            <asp:HiddenField ID="HiddenField11" runat="server" />
                            <div class="row text-center bg-warning col-lg-10 col-lg-offset-1">
                                <label>CUATRIMESTRE I</label>
                            </div>
                            <div class="form-group row col-lg-12">
                                <div class="form-group col-lg-1 text-center" style="display:flex; flex-direction:column; justify-content:center; align-items:flex-start; height:100%; align-content:space-between; flex-grow:1;">
                                    <label>AVANCE</label><br />
                                    <label>Físico</label><br />                                    
                                    <label>Financiero</label>                                    
                                </div>
                                <div class="form-group col-lg-offset-1">                                
                                    <div class="form-group col-lg-2 text-center">
                                        <label>ENERO</label>
                                        <asp:TextBox ID="txtfg1" runat="server" CssClass="form-control text-right" width="100%" onblur="sumafisicasg1er()" onkeypress="return soloNumeros(event)" Enabled="false"></asp:TextBox>
                                        <asp:TextBox ID="txtfng1" runat="server" CssClass="form-control text-right" onblur="sumafinancierasg1er()" onkeypress="return soloNumeros(event)" Enabled="false" ></asp:TextBox>
                                    </div>
                                    <div class="form-group col-lg-2 text-center">
                                        <label>FEBRERO</label>
                                        <asp:TextBox ID="txtfg2" runat="server" CssClass="form-control text-right" onblur="sumafisicasg1er()" onkeypress="return soloNumeros(event)" Enabled="false"></asp:TextBox> 
                                        <asp:TextBox ID="txtfng2" runat="server" CssClass="form-control text-right"  onblur="sumafinancierasg1er()"  onkeypress="return soloNumeros(event)" Enabled="false"   ></asp:TextBox>
                                    </div>
                                    <div class="form-group col-lg-2 text-center">
                                        <label>MARZO</label>
                                        <asp:TextBox ID="txtfg3" runat="server" CssClass="form-control text-right"  onblur="sumafisicasg1er()" onkeypress="return soloNumeros(event)"  Enabled="false"></asp:TextBox>
                                        <asp:TextBox ID="txtfng3" runat="server" CssClass="form-control text-right" onblur="sumafinancierasg1er()" onkeypress="return soloNumeros(event)" Enabled="false" ></asp:TextBox>
                                    </div>
                                    <div class="form-group col-lg-2 text-center">
                                        <label>ABRIL</label>
                                        <asp:TextBox ID="txtfg4" runat="server" CssClass="form-control text-right" onblur="sumafisicasg1er()" onkeypress="return soloNumeros(event)"  Enabled="false"></asp:TextBox>
                                        <asp:TextBox ID="txtfng4" runat="server" CssClass="form-control text-right" onblur="sumafinancierasg1er()"  onkeypress="return soloNumeros(event)" Enabled="false"></asp:TextBox>
                                    </div>
                                    <div class="form-group col-lg-2 text-center">
                                        <label>ACUMULADO</label>
                                        <asp:TextBox ID="txtfgc1" runat="server" CssClass="form-control text-right" Enabled="False"></asp:TextBox>
                                        <asp:TextBox ID="txtfngc1" runat="server" CssClass="form-control text-right" Enabled="false"></asp:TextBox>
                                    </div>
                                   
                                </div>
                            </div>
                            <div class="row text-center bg-warning col-lg-10 col-lg-offset-1">
                                <label>CUATRIMESTRE II</label>
                            </div>
                            <div class="form-group row col-lg-12">
                                <div class="form-group col-lg-1" style="display:flex; flex-direction:column; justify-content:center; align-items:flex-start; height:100%; align-content:space-between; flex-grow:1;">
                                    <label>&nbsp;</label><br />
                                    <label>Físico</label><br />                                    
                                    <label>Financiero</label>                                    
                                </div>
                                <div class="form-group col-lg-offset-1">
                                <div class="form-group col-lg-2 text-center">
                                    <label>MAYO</label>
                                    <asp:TextBox ID="txtfg5" runat="server" CssClass="form-control text-right" onblur=" sumafisicasg2do()" onkeypress="return soloNumeros(event)" Enabled="false"></asp:TextBox>
                                    <asp:TextBox ID="txtfng5" runat="server" CssClass="form-control text-right"  onblur="sumafinancierasg2do()" onkeypress="return soloNumeros(event)" Enabled="false"></asp:TextBox>
                                </div>
                                <div class="form-group col-lg-2 text-center">
                                    <label>JUNIO</label>
                                    <asp:TextBox ID="txtfg6" runat="server" CssClass="form-control text-right"  onblur="sumafisicasg2do()"  onkeypress="return soloNumeros(event)" Enabled="false"></asp:TextBox>
                                    <asp:TextBox ID="txtfng6" runat="server" CssClass="form-control text-right" Enabled="false" onblur="sumafinancierasg2do()" onkeypress="return soloNumeros(event)"></asp:TextBox>
                                </div>
                                <div class="form-group col-lg-2 text-center">
                                    <label>JULIO</label>
                                    <asp:TextBox ID="txtfg7" runat="server" CssClass="form-control text-right" onblur="sumafisicasg2do()" onkeypress="return soloNumeros(event)"  Enabled="false"></asp:TextBox>
                                    <asp:TextBox ID="txtfng7" runat="server" CssClass="form-control text-right" Enabled="false" onkeypress="return soloNumeros(event)" onblur="sumafinancierasg2do()" ></asp:TextBox>
                                </div>
                                <div class="form-group col-lg-2 text-center">
                                    <label>AGOSTO</label>
                                    <asp:TextBox ID="txtfg8" runat="server" CssClass="form-control text-right" onblur="sumafisicasg2do()" onkeypress="return soloNumeros(event)" Enabled="false"></asp:TextBox>
                                    <asp:TextBox ID="txtfng8" runat="server" CssClass="form-control text-right" Enabled="false"  onkeypress="return soloNumeros(event)" onblur="sumafinancierasg2do()"></asp:TextBox>
                                </div>
                                <div class="form-group col-lg-2 text-center">
                                    <label>ACUMULADO</label>
                                    <asp:TextBox ID="txtfgc2" runat="server" CssClass="form-control text-right" Enabled="False"></asp:TextBox>
                                    <asp:TextBox ID="txtfngc2" runat="server" CssClass="form-control text-right" Enabled="false"></asp:TextBox>
                                </div>
                               
                                </div>
                            </div>
                            <div class="row text-center bg-warning col-lg-10 col-lg-offset-1">
                                <label>CUATRIMESTRE III</label>
                            </div>
                            <div class="form-group row col-lg-12">
                                <div class="form-group col-lg-1" style="display:flex; flex-direction:column; justify-content:center; align-items:flex-start; height:100%; align-content:space-between; flex-grow:1;">
                                    <label>&nbsp;</label><br />
                                    <label>Físico</label><br />                                    
                                    <label>Financiero</label>                                    
                                </div>
                            <div class="form-group col-lg-offset-1">
                                <div class="form-group col-lg-2 text-center">
                                    <label>SEPTIEMBRE</label>
                                    <asp:TextBox ID="txtfg9" runat="server" CssClass="form-control text-right" Enabled="false"  onkeypress="return soloNumeros(event)" onblur="sumafisicasg3er()"></asp:TextBox>
                                    <asp:TextBox ID="txtfng9" runat="server" CssClass="form-control text-right" Enabled="false"   onkeypress="return soloNumeros(event)" onblur="sumafinancierasg3er()"></asp:TextBox>
                                </div>
                                <div class="form-group col-lg-2 text-center">
                                    <label>OCTUBRE</label>
                                    <asp:TextBox ID="txtfg10" runat="server" CssClass="form-control text-right" Enabled="false" onkeypress="return soloNumeros(event)" onblur="sumafisicasg3er()"></asp:TextBox>
                                    <asp:TextBox ID="txtfng10" runat="server" CssClass="form-control text-right" Enabled="false" onkeypress="return soloNumeros(event)" onblur="sumafinancierasg3er()"></asp:TextBox>
                                </div>
                                <div class="form-group col-lg-2 text-center">
                                    <label>NOVIEMBRE</label>
                                    <asp:TextBox ID="txtfg11" runat="server" CssClass="form-control text-right"  Enabled="false" onkeypress="return soloNumeros(event)" onblur="sumafisicasg3er()"></asp:TextBox>
                                    <asp:TextBox ID="txtfng11" runat="server" CssClass="form-control text-right" Enabled="false" onkeypress="return soloNumeros(event)" onblur="sumafinancierasg3er()"></asp:TextBox>
                                </div>
                                <div class="form-group col-lg-2 text-center">
                                    <label>DICIEMBRE</label>
                                    <asp:TextBox ID="txtfg12" runat="server" CssClass="form-control text-right" Enabled="false" onkeypress="return soloNumeros(event)" onblur="sumafisicasg3er()"></asp:TextBox>
                                    <asp:TextBox ID="txtfng12" runat="server" CssClass="form-control text-right" Enabled="false" onkeypress="return soloNumeros(event)"  onblur="sumafinancierasg3er()" ></asp:TextBox>
                                </div>
                                <div class="form-group col-lg-2 text-center">
                                    <label>ACUMULADO</label> 
                                    <asp:TextBox ID="txtfgc3" runat="server" CssClass="form-control text-right" Enabled="False"></asp:TextBox>
                                    <asp:TextBox ID="txtfngc3" runat="server" CssClass="form-control text-right" Enabled="false"></asp:TextBox>
                                </div>
                               
                            </div>
                            </div>


                            <div class="row text-center bg-warning col-lg-10 col-lg-offset-1">
                                <label>TOTAL ANUAL</label>
                            </div>
                            <div class="form-group row col-lg-12">
                                <div class="form-group col-lg-1" style="display:flex; flex-direction:column; justify-content:center; align-items:flex-start; height:100%; align-content:space-between; flex-grow:1;">
                                    <label>&nbsp;</label><br />
                                    <label>Físico</label><br />                                    
                                    <label>Financiero</label>                                    
                                </div>
                            <div class="form-group col-lg-offset-1">
                                                           
                                
                               
                                <div class="form-group col-lg-2 text-center">
                                    <label>ANUAL</label> 
                                    <asp:TextBox ID="txtAnualFisico" runat="server" CssClass="form-control text-right" Enabled="False"></asp:TextBox>
                                    <asp:TextBox ID="txtAnualFinanciero" runat="server" CssClass="form-control text-right" Enabled="false"></asp:TextBox>
                                </div>
                               
                            </div>
                            </div>

                            <p>
                                <br />
                                <br />
                            </p>
                            <div class="form-group">                                
                                <div class="col-lg-4 col-lg-offset-1">
                                    <asp:Button ID="btnGuardaAvanceGERO" runat="server" Text="Guardar" CssClass="btn btn-success btn-block" OnClick="btnGuardaAvanceGERO_Click"/>
                                </div>
                                <div class="col-lg-4 col-lg-offset-2">
                                    <asp:Button ID="btnCierraAvanceGERO" runat="server" Text="Cancelar" CssClass="btn btn-primary btn-block"  OnClick="btnCierraAvanceGERO_Click"/>
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

                </asp:View>
          <!--vista de municipio-->
                      <asp:View runat="server" ID="vstMetasMunicipio">
              <asp:Panel ID="panMetaMunos" runat="server">
                   <div  style ="overflow:auto; width:100%; height:90%; padding: 1px 10px 1px 10px;   ">
                   <h4 style="color:#2d572c">Registros de ejecución de metas anuales por municipio</h4>
                   
                       <div class="row">
    <div class="col-md-6">
        <h4 style="margin:0; color:#2d572c;">
            Ejecución de metas por municipio
             <asp:HiddenField ID="hfIDSubProducto" runat="server" />
        </h4>
    </div>

    <div class="col-md-6">
        <div class="pull-right">
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

        <div class="col-md-2">
            <b>Año</b><br />
            <asp:Label ID="lblAnioRegistroMuno" runat="server" />
        </div>

        <div class="col-md-3">
            <b>Subproducto</b><br />
            <asp:Label ID="SubproductoMuno" runat="server" />
        </div>

        <div class="col-md-2">
            <b>Medida</b><br />
            <asp:Label ID="MedidaMuno" runat="server" />
        </div>

        <div class="col-md-2">
            <b>Física</b><br />
            <asp:Label ID="FisicaMuno" runat="server" />
        </div>

        <div class="col-md-3">
            <b>Financiera</b><br />
            <asp:Label ID="FinanceraMuno" runat="server" />
        </div>

    </div>

</div>

               <div class="row">
    <div class="col-md-6">
        <div class="form-inline">
            <span class="text-muted">
    Carga masiva desde Excel
</span>
            <asp:FileUpload ID="fuExcel" runat="server" class="form-control" />

            <asp:Button ID="btnSubir" runat="server"
                Text="Subir"
                CssClass="btn btn-primary btn-sm"
                OnClick="btnSubir_Click" />
        </div>
    </div>

         

                  

                      <!-- <asp:UpdatePanel ID="UpdatePanel6" runat="server">
    <ContentTemplate>-->
                   <div style="margin-top:10px;">
                       <dx:ASPxGridView ID="gvMunosMetas" runat="server"  KeyFieldName="ID_UNICO"  Theme="Office2010Blue" Width="100%"  SettingsBehavior-AllowFocusedRow ="true"   ClientInstanceName="gvMunosMetas" OnCellEditorInitialize="gvMunosMetas_CellEditorInitialize" OnBatchUpdate="gvMunosMetas_BatchUpdate">
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

           <dx:GridViewBandColumn Caption="Total de ejecución de metas" VisibleIndex="14">
                    <Columns>
                    <dx:GridViewDataTextColumn FieldName="ANUAL_FISICO" ShowInCustomizationForm="True" Caption="Totales de avances físico" VisibleIndex="1" ReadOnly="true">
                    <PropertiesTextEdit DisplayFormatString="{0:N2}"></PropertiesTextEdit>
                    <CellStyle HorizontalAlign="Right"></CellStyle>
                </dx:GridViewDataTextColumn>  

                  <dx:GridViewDataTextColumn FieldName="PORCENTAJE_FISICO" ShowInCustomizationForm="True" Caption="Porcentaje avance" VisibleIndex="2" ReadOnly="true">
    <PropertiesTextEdit DisplayFormatString="{0:N2}"></PropertiesTextEdit>
    <CellStyle HorizontalAlign="Right"></CellStyle>
</dx:GridViewDataTextColumn>  

            <dx:GridViewDataTextColumn FieldName="ANUAL_FINANCIERO" ShowInCustomizationForm="True" Caption="Totales de avances financiero" VisibleIndex="3" ReadOnly="true">
    <PropertiesTextEdit DisplayFormatString="{0:N2}"></PropertiesTextEdit>
    <CellStyle HorizontalAlign="Right"></CellStyle>
</dx:GridViewDataTextColumn>  
                                                                                                                       <dx:GridViewDataTextColumn FieldName="PORCENTAJE_FINANCIERO" ShowInCustomizationForm="True" Caption="Porcentaje de avances financiero" VisibleIndex="4" ReadOnly="true">
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


    <dx:ASPxPopupControl ID="popFechaCierre" runat="server" CssClass="pop" AllowDragging="true" Width="500px" Height="350" PopupHorizontalAlign="WindowCenter" HeaderText="Ingreso de nueva fecha de corte" PopupVerticalAlign="WindowCenter" AllowResize="true" Modal="true">
                <HeaderStyle Font-Bold="true" Font-Size="X-Large" ForeColor="#003399" HorizontalAlign="Center" />
                <ContentCollection>
                    <dx:PopupControlContentControl>
                        <div>
                            
                            <div class="form-group">
                                <label>Cuatrimestre a generar:</label>                                    
                                <dx:ASPxComboBox ID="ASPxComboBox1" runat="server" ValueType="System.String" Width="100%" Theme="iOS" CssClass="list-group">
                                    <Items>
                                 <dx:ListEditItem Selected="True" Text="1er Cuatrimestre Enero-Abril" Value="0" />
                                 <dx:ListEditItem Text="2do Cuatrimestre Mayo-Agosto" Value="1" />
                                  <dx:ListEditItem Text="3er Cuatrimestre Septiembre-Diciembre" Value="2" />

                             </Items>

                                </dx:ASPxComboBox>
                            </div> 
                            
                            <p>
                                <br />
                                <br />
                            </p>
                            <div class="form-group">                                
                                <div class="col-lg-4 col-lg-offset-1">
                                    <asp:Button ID="Button1" runat="server" Text="Generar" CssClass="btn btn-success btn-block" OnClick="btnGenerar_Click" />
                                </div>
                                <div class="col-lg-4 col-lg-offset-2">
                                    <asp:Button ID="Button2" runat="server" Text="Cerrar" CssClass="btn btn-primary btn-block" OnClick="btnCancela_Click" />
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


    <!--<asp:UpdatePanel ID="UpdatePanel4" runat="server">
        <ContentTemplate>-->
            <dx:ASPxPopupControl ID="popReporte" runat="server" CssClass="pop" AllowDragging="true" Width="500px" Height="350" PopupHorizontalAlign="WindowCenter" HeaderText="Generar reporte de ejecución" PopupVerticalAlign="WindowCenter" AllowResize="true" Modal="true">
                <HeaderStyle Font-Bold="true" Font-Size="X-Large" ForeColor="#003399" HorizontalAlign="Center" />
                <ContentCollection>
                    <dx:PopupControlContentControl>
                        <div>
                            
                            <div class="form-group">
                                <label>Cuatrimestre a generar:</label>                                    
                                <dx:ASPxComboBox ID="cbCuatrimestre" runat="server" ValueType="System.String" Width="100%" Theme="iOS" CssClass="list-group">
                                    <Items>
                                 <dx:ListEditItem Selected="True" Text="1er Cuatrimestre Enero-Abril" Value="0" />
                                 <dx:ListEditItem Text="2do Cuatrimestre Mayo-Agosto" Value="1" />
                                  <dx:ListEditItem Text="3er Cuatrimestre Septiembre-Diciembre" Value="2" />

                             </Items>

                                </dx:ASPxComboBox>
                            </div> 
                            
                             <div class="form-group">
                           <label for="txtsubCod" aria-required="true"><font color="red">*</font>Nombre de la máxima autoridad </label>
                             <asp:TextBox ID="txtAutoridad" PlaceHolder="Nombre de la máxima autoridad" runat="server" CssClass="form-control"  MaxLength="200"></asp:TextBox>
                              </div> 

                             <div class="form-group">
                           <label for="txtsubCod" aria-required="true"><font color="red">*</font>Cargo de la máxima autoridad   </label>
                             <asp:TextBox ID="txtCargo" PlaceHolder="Cargo de la máxima autoridad" runat="server" CssClass="form-control"  MaxLength="200"></asp:TextBox>
                              </div> 

                            <p>
                                <br />
                                <br />
                            </p>
                            <div class="form-group">                                
                                <div class="col-lg-4 col-lg-offset-1">
                                    <asp:Button ID="btnGenerar" runat="server" Text="Generar" CssClass="btn btn-success btn-block" OnClick="btnGenerar_Click" />
                                </div>
                                <div class="col-lg-4 col-lg-offset-2">
                                    <asp:Button ID="btnCancela" runat="server" Text="Cerrar" CssClass="btn btn-primary btn-block" OnClick="btnCancela_Click" />
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
        <!--</ContentTemplate>
    </asp:UpdatePanel>-->
       
       <script>
  function abrirEnlace() {
    // URL del enlace que deseas abrir
    var url = 'https://forms.office.com/Pages/ResponsePage.aspx?id=gZplVIGYMECK9vYVEqi82Ffn_AktAo1DhPtpDkT_wy9URUExS0kxMUpINFo1NTNZNEpQWFhCU0UwQy4u';
    // Abrir la URL en una nueva ventana o pestaña
    window.open(url, '_blank');
  }
</script>
       
    </asp:Content>
