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
    private const string FOURSQUARE_RSS_FEED_CACHE_KEY = "foursquare-rss-feed";

    public string message = "";
    public string latArray = "";
    public string lonArray = "";
    public string TitleArray = "";
    public string DescriptionArray = "";

    public override string Name
    {
        get { return "Foursquare"; }
    }

    public override bool IsEditable
    {
        get { return true; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {        
        
    }

    public override void LoadWidget()
    {
        Title = "Foursquare";
        if (!IsPostBack)
            LoadCheckIns();
    }

    private void LoadCheckIns()
    {
        FoursquareSettings settings = GetFoursquareSettings();
        try
        {
            if (!string.IsNullOrEmpty(settings.kmlfeedurl) && !string.IsNullOrEmpty(settings.rssfeedurl))
            {
                XDocument kml_doc = (XDocument)HttpRuntime.Cache[FOURSQUARE_KML_FEED_CACHE_KEY];
                XDocument rss_doc = (XDocument)HttpRuntime.Cache[FOURSQUARE_RSS_FEED_CACHE_KEY];

                if (kml_doc != null && rss_doc != null && !(DateTime.Now > settings.lastModified.AddMinutes(settings.pollinginterval)))
                {
                    BindFeed(kml_doc, rss_doc, settings.maxitems);
                }
                else
                {
                    // If older versions of the feeds are already written to disk load them first to reduce page loading time.
                    kml_doc = GetLastKMLFeed();
                    rss_doc = GetLastRSSFeed();

                    // There are no older versions, we have to load them from foursquare now
                    if (kml_doc == null || rss_doc == null || (DateTime.Now > settings.lastModified.AddMinutes(settings.pollinginterval)))
                    {
                        kml_doc = new XDocument();
                        kml_doc = XDocument.Load(settings.kmlfeedurl); // put here some better error handling here
                        rss_doc = new XDocument();
                        rss_doc = XDocument.Load(settings.rssfeedurl); // put here some better error handling here
                    }
                    settings.lastModified = DateTime.Now;
                    BindFeed(kml_doc, rss_doc, settings.maxitems);
                    HttpRuntime.Cache[FOURSQUARE_KML_FEED_CACHE_KEY] = kml_doc;
                    HttpRuntime.Cache[FOURSQUARE_RSS_FEED_CACHE_KEY] = rss_doc;
                    SaveLastKMLFeed(kml_doc);
                    SaveLastRSSFeed(rss_doc);


                }
            }
            else
            {
                message = "Please enter your feed URLs";
            }
        }
        catch (Exception ex)
        {
            message = ex.Message.ToString();
        }
    }

    protected void repItems_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        HyperLink text = (HyperLink)e.Item.FindControl("lblItem");
        Label date = (Label)e.Item.FindControl("lblDate");
        CheckIn checkin = (CheckIn)e.Item.DataItem;
        HtmlInputHidden lat = (HtmlInputHidden)e.Item.FindControl("lat");
        HtmlInputHidden lon = (HtmlInputHidden)e.Item.FindControl("lon");
        text.Text = checkin.Title;
        text.NavigateUrl = checkin.Url.ToString();
        date.Text = checkin.PubDate.ToString("MMMM d. HH:mm");
        
    }

    private void BindFeed(XDocument kml_doc, XDocument rss_doc, int maxItems)
    {
        List<CheckIn> checkins = new List<CheckIn>();
        // Here we mesh the two feeds, kml and rss together to get both the link to the venue and the coordinates.
        latArray = "[";
        lonArray = "[";
        TitleArray = "[";
        DescriptionArray = "[";
        foreach (var item in kml_doc.Element("kml").Element("Folder").Elements("Placemark").Take(maxItems))
        {
            CheckIn checkin = new CheckIn();
            checkin.Title = item.Element("name").Value.ToString();
            checkin.PubDate = DateTime.Parse(item.Element("published").Value.ToString(), CultureInfo.InvariantCulture);
            TitleArray += "\"" + checkin.Title + "\",";
            DescriptionArray += "\"" + checkin.PubDate + "\",";
            string correctURL = rss_doc.Element("rss").Element("channel").Elements("item").Where(t => (DateTime.Parse(t.Element("pubDate").Value.ToString(), CultureInfo.InvariantCulture) == checkin.PubDate)).Single().Element("link").Value.ToString();
            checkin.Url = new Uri(correctURL, UriKind.Absolute);
            
            // Read latitude and longitude...
            string[] coordinates = item.Element("Point").Element("coordinates").Value.ToString().Split(',');
            latArray += "'" + coordinates[1] + "',";
            lonArray += "'" + coordinates[0] + "',";

            checkins.Add(checkin);
        }
        latArray += "]";
        lonArray += "]";
        TitleArray += "]";
        DescriptionArray += "]";
        repItems.DataSource = checkins;
        repItems.DataBind();
    }

    private FoursquareSettings GetFoursquareSettings()
    {
        FoursquareSettings foursquareSettings = (FoursquareSettings)HttpRuntime.Cache[FOURSQUARE_SETTINGS_CACHE_KEY];
        if (foursquareSettings != null)
        {
            return foursquareSettings;
        }

        foursquareSettings = new FoursquareSettings();
        StringDictionary settings = GetSettings();

        if (settings.ContainsKey("kmlfeedurl") && !string.IsNullOrEmpty(settings["kmlfeedurl"]))
        {
            foursquareSettings.kmlfeedurl = settings["kmlfeedurl"].ToString();
        }
        if (settings.ContainsKey("rssfeedurl") && !string.IsNullOrEmpty(settings["rssfeedurl"]))
        {
            foursquareSettings.rssfeedurl = settings["rssfeedurl"].ToString();
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
        public string kmlfeedurl;
        public string rssfeedurl;
        public string accounturl;
        public int maxitems;
        public int pollinginterval;
        public DateTime lastModified;
    }

    private struct CheckIn : IComparable<CheckIn>
    {
        public string Title;
        public Uri Url;
        public DateTime PubDate;
        public string longitude;
        public string latitude;

        public int CompareTo(CheckIn other)
        {
            return other.PubDate.CompareTo(this.PubDate);
        }
    }
    private static string _lastKMLFeedDataFileName;
    private static string GetLastKMLFeedDataFileName()
    {
        if (string.IsNullOrEmpty(_lastKMLFeedDataFileName))
        {
            _lastKMLFeedDataFileName = HostingEnvironment.MapPath(Path.Combine(BlogSettings.Instance.StorageLocation, "foursquare_kml_feeds.xml"));
        }

        return _lastKMLFeedDataFileName;
    }
    private static string _lastRSSFeedDataFileName;
    private static string GetLastRSSFeedDataFileName()
    {
        if (string.IsNullOrEmpty(_lastRSSFeedDataFileName))
        {
            _lastRSSFeedDataFileName = HostingEnvironment.MapPath(Path.Combine(BlogSettings.Instance.StorageLocation, "foursquare_rss_feeds.xml"));
        }

        return _lastRSSFeedDataFileName;
    }
    private static void SaveLastKMLFeed(XDocument doc)
    {
        try
        {
            string file = GetLastKMLFeedDataFileName();
            doc.Save(file);
        }
        catch (Exception ex)
        {
            Utils.Log("Error saving last twitter feed load to data store.  Error: " + ex.Message);
        }
    }
    private static void SaveLastRSSFeed(XDocument doc)
    {
        try
        {
            string file = GetLastRSSFeedDataFileName();
            doc.Save(file);
        }
        catch (Exception ex)
        {
            Utils.Log("Error saving last twitter feed load to data store.  Error: " + ex.Message);
        }
    }
    private XDocument GetLastKMLFeed()
    {
        string file = GetLastKMLFeedDataFileName();
        XDocument doc = null;

        try
        {
            if (File.Exists(file))
            {
                doc = new XDocument();
                doc = XDocument.Load(file);
            }
        }
        catch (Exception ex)
        {
            Utils.Log("Error retrieving last foursquare KML feed load from data store.  Error: " + ex.Message);
        }

        return doc;
    }
    private XDocument GetLastRSSFeed()
    {
        string file = GetLastRSSFeedDataFileName();
        XDocument doc = null;

        try
        {
            if (File.Exists(file))
            {
                doc = new XDocument();
                doc = XDocument.Load(file);
            }
        }
        catch (Exception ex)
        {
            Utils.Log("Error retrieving last foursquare RSS feed load from data store.  Error: " + ex.Message);
        }

        return doc;
    }

}
