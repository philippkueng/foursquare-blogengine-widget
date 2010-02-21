#region Using

using System;
using System.Xml;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Globalization;
using BlogEngine.Core;
using System.IO;
using System.Web.Hosting;
using BlogEngine.Core.DataStore;
using System.Text.RegularExpressions;
using System.Linq;
using System.Xml.Linq;

#endregion

public partial class widgets_Foursquare_widget : WidgetBase
{
    private const string FOURSQUARE_SETTINGS_CACHE_KEY = "foursquare-settings";
    private const string FOURSQUARE_KML_FEED_CACHE_KEY = "foursquare-kml-feed";

    public string message = "";
    public override string Name
    {
        get { return "Foursquare"; }
    }

    public override bool IsEditable
    {
        get { return false; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        Title = "Foursquare";
        if (!IsPostBack)
            LoadCheckIns();
        
    }

    public override void LoadWidget()
    {
        //// Hier den Feed cachen...
        //string feed = "http://feeds.foursquare.com/history/AIASJNH1WMBVW0LHHSYBS2Z4GQXEFRCZ.kml";
        //XDocument doc = XDocument.Load(feed);
        //BindFeed(doc, 3);
    }

    private void LoadCheckIns()
    {
        FoursquareSettings settings = GetFoursquareSettings();
        try
        {
            XDocument kml_doc = (XDocument)HttpRuntime.Cache[FOURSQUARE_KML_FEED_CACHE_KEY];
            if (kml_doc != null)
                BindFeed(kml_doc, settings.maxitems);
            else
            {
                kml_doc = new XDocument();
                kml_doc = XDocument.Load(settings.feedurl);
                BindFeed(kml_doc, settings.maxitems);
                HttpRuntime.Cache[FOURSQUARE_KML_FEED_CACHE_KEY] = kml_doc;
            }
        }
        catch (Exception ex)
        {
            message = ex.Message.ToString();
        }
    }

    protected void repItems_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        Label text = (Label)e.Item.FindControl("lblItem");
        Label date = (Label)e.Item.FindControl("lblDate");
        CheckIn checkin = (CheckIn)e.Item.DataItem;
        text.Text = checkin.Title;
        date.Text = checkin.PubDate.ToString("MMMM d. HH:mm");
    }

    private void BindFeed(XDocument kml_doc, int maxItems)
    {
        List<CheckIn> checkins = new List<CheckIn>();
        foreach (var item in kml_doc.Element("kml").Element("Folder").Elements("Placemark"))
        {
            CheckIn checkin = new CheckIn();
            checkin.Title = item.Element("name").Value.ToString();
            checkin.PubDate = DateTime.Parse(item.Element("published").Value.ToString(), CultureInfo.InvariantCulture);
            checkin.Url = new Uri("http://example.com", UriKind.Absolute);
            checkins.Add(checkin);
        }
        repItems.DataSource = checkins;
        repItems.DataBind();
    }

    private FoursquareSettings GetFoursquareSettings()
    {
        FoursquareSettings foursquareSettings = (FoursquareSettings)HttpRuntime.Cache[FOURSQUARE_SETTINGS_CACHE_KEY];
        if (foursquareSettings != null)
            return foursquareSettings;

        foursquareSettings = new FoursquareSettings();
        StringDictionary settings = GetSettings();

        if (settings.ContainsKey("feedurl") && !string.IsNullOrEmpty(settings["feedurl"]))
        {
            foursquareSettings.feedurl = settings["feedurl"].ToString();
        }
        if (settings.ContainsKey("accounturl") && !string.IsNullOrEmpty(settings["accounturl"]))
        {
            foursquareSettings.accounturl = settings["accounturl"].ToString();
        }
        if (settings.ContainsKey("maxitems") && !string.IsNullOrEmpty(settings["maxitems"]))
        {
            foursquareSettings.maxitems = int.Parse(settings["maxitems"].ToString());
        }
        if (settings.ContainsKey("pollinginterval") && !string.IsNullOrEmpty(settings["pollinginterval"]))
        {
            foursquareSettings.pollinginterval = int.Parse(settings["pollinginterval"].ToString());
        }

        HttpRuntime.Cache[FOURSQUARE_SETTINGS_CACHE_KEY] = foursquareSettings;
        return foursquareSettings;

    }

    internal class FoursquareSettings
    {
        public string feedurl;
        public string accounturl;
        public int maxitems;
        public int pollinginterval;
    }

    private struct CheckIn : IComparable<CheckIn>
    {
        public string Title;
        public Uri Url;
        public DateTime PubDate;

        public int CompareTo(CheckIn other)
        {
            return other.PubDate.CompareTo(this.PubDate);
        }
    }

}
