#region Using

using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Collections.Specialized;

#endregion


public partial class widgets_Foursquare_edit : WidgetEditBase
{
    private const string FOURSQUARE_SETTINGS_CACHE_KEY = "foursquare-settings";  // same key used in widget.ascx.cs.

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            StringDictionary settings = GetSettings();
            if (settings.ContainsKey("kmlfeedurl"))
            {
                txtFoursquareKMLFeedURL.Text = settings["kmlfeedurl"];
                txtFoursquareRSSFeedURL.Text = settings["rssfeedurl"];
                txtAccountURL.Text = settings["accounturl"];
                txtCheckIns.Text = settings["maxitems"];
                txtPolling.Text = settings["pollinginterval"];
            }
        }
    }

    public override void Save()
    {
        StringDictionary settings = GetSettings();
        settings["kmlfeedurl"] = txtFoursquareKMLFeedURL.Text;
        settings["rssfeedurl"] = txtFoursquareRSSFeedURL.Text;
        settings["accounturl"] = txtAccountURL.Text;
        settings["maxitems"] = txtCheckIns.Text;
        settings["pollinginterval"] = txtPolling.Text;
        SaveSettings(settings);

        //Don't need to clear Feed out of cache because when the Settings are cleared,
        //the last modified date (i.e. TwitterSettings.LastModified) will reset to
        //DateTime.MinValue and Twitter will be re-queried.
        HttpRuntime.Cache.Remove(FOURSQUARE_SETTINGS_CACHE_KEY);
    }
}
