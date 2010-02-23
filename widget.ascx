<%@ Control Language="C#" AutoEventWireup="true" CodeFile="widget.ascx.cs" Inherits="widgets_Foursquare_widget" %>
<ul>
<asp:Repeater runat="server" ID="repItems" OnItemDataBound="repItems_ItemDataBound">
  <ItemTemplate>
    <li>
        <img src="<%=BlogEngine.Core.Utils.RelativeWebRoot %>widgets/foursquare/foursquare.ico" alt="Foursquare" />
        <asp:Label runat="server" ID="lblDate" style="color:gray" /><br />
        <asp:HyperLink runat="server" ID="lblItem" />
        <%--<asp:Label runat="server" ID="lblLink" />--%>
    </li>
  </ItemTemplate>
</asp:Repeater>
<%= message %>
</ul>

