<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="WB_CFDI_V1.Vistas.Login" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
        <script type="text/javascript">
            $(document).ready(function () {

                var auto = true;
                /*******************************************************************************/

                function getData(cUrl, cData) {
                    var result = null;

                    $.ajax({
                        type: 'POST',
                        contentType: "application/json; charset=utf-8",
                        url: cUrl,
                        data: JSON.stringify(cData),
                        dataType: "json",
                        async: false,
                        success: function (data) {
                            try {
                                result = JSON.parse(data.d);
                            } catch (err) {
                                try {
                                    result = data.d;
                                } catch (err) {
                                    result = data;
                                }
                            }
                        },
                        error: function (data) {
                            try {
                                console.debug("************* Error ***************");
                                console.debug(data);
                            } catch (err) { }

                            if (data.status == 401) {
                                window.location.href = "Login.aspx";
                            }
                        }
                    });


                    return result;
                }


                /*---------------------------------------------*/

                function getAdapter(cUrl, cData) {
                    auto = true;

                    var cSource = getData(cUrl, cData);

                    if (!$.isEmptyObject(cSource)) {

                        return new $.jqx.dataAdapter(cSource, {
                            autoBind: true,
                            loadComplete: function (data) {
                                if (data.length > 10) {
                                    auto = false;
                                }
                            }
                        });
                    }
                    return null;
                }

                /*---------------------------------------------*/


                //$.jqx.theme = "bootstrap";
                $('.tx').jqxInput({ height: 22, width: '86%' });
                $('.tx').css('margin-top', '5px');
                $(".btn1").button();
                //$(".btn").jqxButton();

                

                $(".jCb").jqxDropDownList({ height: 25, width: '93%', searchMode: 'containsignorecase',
                    dropDownHeight: 200, autoDropDownHeight: true, promptText: '------------------------------------',
                    displayMember: 'DESCRIPCION', valueMember: 'ID',
                    source: getAdapter("Login.aspx/GetEmpresas", {}),
                    //source: ['PSI - compras', 'PSI - gastos y servicios'],
                    theme: 'Theme2',
                    autoDropDownHeight: auto
                });

                var id = getData("Login.aspx/GetEmpresaID", {});

                $("#cbBD").bind('select', function (event) {
                    if (event.args && event.args.item) {

                        var PATH = "<%=GetPath() %>";
                        getData("Login.aspx/SetEmpresaID", { ID: event.args.item.value });

                        if (PATH != event.args.item.originalItem.PATH) {
                            window.location.href = event.args.item.originalItem.PATH;
                        }

                        $("#<%=txEmpresa.ClientID %>").val(event.args.item.originalItem.ID);

                    }
                });

                
                $(".jCb").jqxDropDownList('val', id);

            });
        </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Contenido" runat="server">
    <div class="content">
        <%--<asp:Image id="imgLogin" runat="server" ImageUrl="~/Images/inventario2.jpg" />--%>
        <form id="form1" runat="server">
            <asp:HiddenField runat="server" runat="server" ID="txEmpresa" />
            
            <table id="Form">
                <tr>
                    <td colspan="2" style="border: solid 1px rgb(217, 217, 217); background-color: #f6f6f6; font-size: 1.3em; height: 34px;">
                            Login
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="padding-top: 8px; padding-bottom: 10px;">
                        <asp:Label ID="label3" Text="Empresa" runat="server"></asp:Label>
                        <div class="jCb" id="cbBD" style="margin-left: 10px; margin-top: 5px;"></div>

                    </td>
                </tr>
                <tr>
                    <td style="vertical-align: middle;width: 120px;">
                        <asp:Image ID="Image1"  runat="server" ImageUrl="~/Img/candado.png" />
                    </td>
                    <td style="vertical-align: middle;text-align: center">
                        <asp:Label ID="label1" Text="Usuario" runat="server"></asp:Label>
                        <asp:TextBox id="txUser" CssClass="tx" runat="server"></asp:TextBox>
                        <br /><br />
                        <asp:Label ID="label2" Text="Contraseña" runat="server"></asp:Label>
                        <asp:TextBox id="txPass" CssClass="tx" TextMode="Password" runat="server"></asp:TextBox>
                        <br/><br/>
                        <asp:Button ID="btEntrar" Text="Entrar" BorderStyle="None" CssClass="btn1 Button" 
                            runat="server" onclick="btEntrar_Click"/>

                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="padding-bottom: 5px;">
                        <asp:Label ID="lbMsg" Font-Bold="True"  ForeColor="Red" runat="server"></asp:Label>
                    </td>
                </tr>
            </table>

        </form>
    </div>
</asp:Content>
