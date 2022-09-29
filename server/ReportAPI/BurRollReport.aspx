<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BurRollReport.aspx.cs" Inherits="ReportAPI.BurRollReport" %>

<%@ Register assembly="Microsoft.ReportViewer.WebForms" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
    <div>
            <div>
                <asp:ScriptManager ID="ScriptManager1" runat="server">
                </asp:ScriptManager>
            </div>
            <rsweb:ReportViewer ID="RdlcBurRollReportViewer" runat="server" Width="100%" BackColor="White" ZoomMode="PageWidth"
                        BorderWidth="1px" DocumentMapCollapsed="True" PageCountMode="Actual" BorderColor="#CCCCCC" BorderStyle="Solid" Height="100%" ShowBackButton="False" ShowZoomControl="False"   >
            </rsweb:ReportViewer>
    </div>
        </div>
    </form>
</body>
</html>
