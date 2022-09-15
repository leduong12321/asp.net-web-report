<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TongHopSanLuong.aspx.cs" Inherits="ReportAPI.TongHopSanLuong" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
             <div>
                <asp:ScriptManager ID="ScriptManager1" runat="server">
                </asp:ScriptManager>
            </div>
            <rsweb:TongHopSanLuong ID="TongHopSanLuongHRCViewer" runat="server" Width="100%" BackColor="White" ZoomMode="PageWidth"
                        BorderWidth="1px" DocumentMapCollapsed="True" PageCountMode="Actual" BorderColor="#CCCCCC" BorderStyle="Solid" Height="100%"   >
            </rsweb:TongHopSanLuong>
        </div>
    </form>
</body>
</html>
