<%@ Control Language="C#" AutoEventWireup="true" CodeFile="widget.ascx.cs" Inherits="widgets_Foursquare_widget" %>
<ul>
<asp:Repeater runat="server" ID="repItems" OnItemDataBound="repItems_ItemDataBound">
  <ItemTemplate>
    <li style='padding-left: 20px; background: url(<%=BlogEngine.Core.Utils.RelativeWebRoot %>widgets/foursquare/foursquare.ico) 4px left no-repeat;' >
        <asp:Label runat="server" ID="lblDate" style="color:gray" /><br />
        <asp:HyperLink runat="server" ID="lblItem" />
    </li>
  </ItemTemplate>
</asp:Repeater>
<%= message %>
</ul>

