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

<label for="<%txtCheckIns %>">Number of displayed Check-ins</label>
<asp:TextBox runat="server" ID="txtCheckIns" Width="30" />
<asp:RequiredFieldValidator runat="server" ControlToValidate="txtCheckIns" ErrorMessage="Please enter a number" Display="Dynamic" />
<asp:CompareValidator runat="server" ControlToValidate="txtCheckIns" Type="Integer" Operator="DataTypeCheck" ErrorMessage="A real number please" /><br /><br />

<label for="<%txtPolling %>">Polling Interval (#/minutes)</label>
<asp:TextBox runat="server" ID="txtPolling" Width="30" />
<asp:CompareValidator runat="server" ControlToValidate="txtPolling" Type="Integer" Operator="DataTypeCheck" ErrorMessage="A real number please" /><br /><br />