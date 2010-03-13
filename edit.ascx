<%@ Control Language="C#" AutoEventWireup="true" CodeFile="edit.ascx.cs" Inherits="widgets_Foursquare_edit" %>
<%@ Reference Control="~/widgets/Foursquare/widget.ascx" %>

<label for="<%=txtAccountURL %>">Your Foursquare Account URL</label>
<asp:TextBox runat="server" ID="txtAccountURL" Width="300" />
<asp:RequiredFieldValidator runat="server" ControlToValidate="txtAccountURL" ErrorMessage="Please enter a URL" Display="Dynamic" /><br /><br />

<label for="<%=txtFoursquareKMLFeedURL %>">Your Foursquare KML Feed URL</label>
<asp:TextBox runat="server" ID="txtFoursquareKMLFeedURL" Width="300" />
<asp:RequiredFieldValidator runat="server" ControlToValidate="txtFoursquareKMLFeedURL" ErrorMessage="Please enter a URL" Display="Dynamic" /><br /><br />

<label for="<%=txtFoursquareRSSFeedURL %>">Your Foursquare RSS Feed URL</label>
<asp:TextBox runat="server" ID="txtFoursquareRSSFeedURL" Width="300" />
<asp:RequiredFieldValidator runat="server" ControlToValidate="txtFoursquareRSSFeedURL" ErrorMessage="Please enter a URL" Display="Dynamic" /><br /><br />

<label for="<%=txtCheckIns %>">Number of displayed Check-ins</label>
<asp:TextBox runat="server" ID="txtCheckIns" Width="30" />
<asp:RequiredFieldValidator runat="server" ControlToValidate="txtCheckIns" ErrorMessage="Please enter a number" Display="Dynamic" />
<asp:CompareValidator runat="server" ControlToValidate="txtCheckIns" Type="Integer" Operator="DataTypeCheck" ErrorMessage="A real number please" /><br /><br />

<label for="<%=txtPolling %>">Polling Interval (#/minutes)</label>
<asp:TextBox runat="server" ID="txtPolling" Width="30" />
<asp:CompareValidator runat="server" ControlToValidate="txtPolling" Type="Integer" Operator="DataTypeCheck" ErrorMessage="A real number please" /><br /><br />

<label for="<%=txtDisplayMap %>">Display you're last check-ins on a map</label>
<asp:CheckBox runat="server" Width="30" ID="txtDisplayMap" /><br /><br />

<label for="<%=txtMapWidth %>">Width of the map (pixel)</label>
<asp:TextBox runat="server" ID="txtMapWidth" Width="30" />
<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtMapWidth" ErrorMessage="Please enter a number" Display="Dynamic" />
<asp:CompareValidator ID="CompareValidator1" runat="server" ControlToValidate="txtMapWidth" Type="Integer" Operator="DataTypeCheck" ErrorMessage="A real number please" /><br /><br />

<label for="<%=txtMapHeight %>">Height of the map (pixel)</label>
<asp:TextBox runat="server" ID="txtMapHeight" Width="30" />
<asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtMapHeight" ErrorMessage="Please enter a number" Display="Dynamic" />
<asp:CompareValidator ID="CompareValidator2" runat="server" ControlToValidate="txtMapHeight" Type="Integer" Operator="DataTypeCheck" ErrorMessage="A real number please" /><br /><br />