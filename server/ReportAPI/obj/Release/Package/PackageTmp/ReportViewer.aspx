<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ReportViewer.aspx.cs" Inherits="ReportAPI.ReportViewer" %>

<%@ Register assembly="Microsoft.ReportViewer.WebForms" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style>
        .Css-Report {
            overflow: hidden;
            margin: 0px;
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
                        BorderWidth="1px" DocumentMapCollapsed="True" PageCountMode="Actual" BorderColor="#CCCCCC" BorderStyle="Solid" Height="100%"   >
            </rsweb:ReportViewer>
        </form>
    </div>
</body>
</html>
