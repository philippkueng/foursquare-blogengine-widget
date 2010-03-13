<%@ Control Language="C#" AutoEventWireup="true" CodeFile="widget.ascx.cs" Inherits="widgets_Foursquare_widget" %>
     
      <asp:PlaceHolder runat="server" ID="foursquareMap" Visible="false">
          <script type="text/javascript" src="http://ecn.dev.virtualearth.net/mapcontrol/mapcontrol.ashx?v=6.2"></script>
          <script type="text/javascript">
          var map = null;

          function AddPushpinWithGeoCoordinates(lat, lon, title, description) {
              var latTemp = parseFloat(lat);
              var lonTemp = parseFloat(lon);
              var pnt = new VELatLong(lat, lon);
              var shape = new VEShape(VEShapeType.Pushpin, pnt);
              shape.SetTitle(title);
              shape.SetDescription(description);
              map.SetCenter(pnt);
              map.AddShape(shape);
          }
          
          function GetMap() {
              map = new VEMap('<%=myMap.ClientID %>');
              map.HideDashboard();
              map.LoadMap();
              map.SetZoomLevel(12);
              var LatArray = <%=latArray %>;
              var LonArray = <%=lonArray %>;
              var Title = <%= TitleArray %>;
              var Description = <%= DescriptionArray %>;
              for (var mapsCounter = 0; mapsCounter < LatArray.length; mapsCounter++) {
                AddPushpinWithGeoCoordinates(LatArray[LatArray.length-mapsCounter-1], LonArray[LatArray.length-mapsCounter-1], Title[LatArray.length-mapsCounter-1], Description[LatArray.length-mapsCounter-1]);
              }
          }          

          if (window.attachEvent) {
              window.attachEvent('onload', GetMap);
          } else {
              if (window.onload) {
                  var curronload = window.onload;
                  var newonload = function() {
                      curronload();
                      GetMap();
                  };
                  window.onload = newonload;
              } else {
                  window.onload = GetMap;
              }
          }
          </script>
          <div id="myMap" runat="server" style="position: relative;">
          </div>
      </asp:PlaceHolder>
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

