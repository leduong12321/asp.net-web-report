<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ReportViewer.aspx.cs" Inherits="ReportAPI.ReportViewer" %>

<%@ Register assembly="Microsoft.ReportViewer.WebForms" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style>
        .Css-Report {
            margin: 0px;
        }
        .Css-Report #RdlcReportViewer_ctl13 {
            overflow-y: hidden !important;
            height: 100% !important;
        }
/*        .Css-Report .aspNetHidden::before {
            content: 'Không có dữ liệu';
        }*/
        @media only screen and (min-width: 1601px) {
            .Css-Report #RdlcReportViewer {
            min-height: 100vh !important;
        }
        }
        @media only screen and (max-width: 1600px) {
          .Css-Report #RdlcReportViewer {
            max-height: calc(100vh - 175px) !important;
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
            <rsweb:ReportViewer ID="RdlcReportViewer" runat="server" Width="100%" BackColor="White" ZoomMode="PageWidth"
                        BorderWidth="1px" DocumentMapCollapsed="True" PageCountMode="Actual" BorderColor="#CCCCCC" BorderStyle="Solid" Height="100%" ShowBackButton="False" ShowZoomControl="False"   >
            </rsweb:ReportViewer>
        </form>
    </div>
</body>
</html>
