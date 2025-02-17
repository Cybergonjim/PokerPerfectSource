using System.Net;

namespace PokerPerfect.Plugins.DataStore.WebApi
{
  public class Constants
  {
    private static string _webApiBaseUrl;
    private const string WebApiUrlKey = "WebApiBaseUrl"; // Key for stored API URL

    public static string WebApiBaseUrl
    {
      get
      {
        if (_webApiBaseUrl == null)
        {
          // Retrieve stored IP address from Preferences
          string storedIp = Preferences.Get(WebApiUrlKey, string.Empty);

          //if (DeviceInfo.Platform == DevicePlatform.Android)
          //{
            // Use 10.0.2.2 if running on an Android emulator
           // _webApiBaseUrl = $"http://10.0.2.2:5097/api";
          //}
          //else
          {
            // Use the actual server IP on other platforms
            _webApiBaseUrl = $"http://{storedIp}:5097/api";
          }
        }

        return _webApiBaseUrl;
      }
    }
  }
}
