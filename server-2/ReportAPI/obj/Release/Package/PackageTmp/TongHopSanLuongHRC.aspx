<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TongHopSanLuongHRC.aspx.cs" Inherits="ReportAPI.TongHopSanLuongHRC" %>

<%@ Register assembly="Microsoft.ReportViewer.WebForms" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style>
        .Css-Report {
            margin: 0px;
        }
        .Css-Report #TongHopSanLuongHRCViewer_ctl13 {
            overflow-y: hidden !important;
            height: 100% !important;
        }
        
        @media only screen and (min-width: 1601px) {
            .Css-Report #TongHopSanLuongHRCViewer {
            min-height: 100vh !important;
        }
        }
        @media only screen and (max-width: 1900px) {
          .Css-Report #TongHopSanLuongHRCViewer {
            max-height: calc(100vh - 200px) !important;
            overflow-y: scroll;
            overflow-x: hidden !important;
          }
        }
    </style>
</head>
<body class="Css-Report">
    <div>
        <form id="form1" runat="server" style="width:100%; height:100%;">
            <div>
                <asp:ScriptManager ID="ScriptManager1" runat="server">
                </asp:ScriptManager>
            </div>
            <rsweb:ReportViewer ID="TongHopSanLuongHRCViewer" runat="server" Width="100%" BackColor="White" 
                        BorderWidth="1px" DocumentMapCollapsed="True" PageCountMode="Actual" BorderColor="#CCCCCC" BorderStyle="Solid" Height="100%" ShowBackButton="False" ShowZoomControl="False"   >
            </rsweb:ReportViewer>
        </form>
    </div>
</body>
</html>
