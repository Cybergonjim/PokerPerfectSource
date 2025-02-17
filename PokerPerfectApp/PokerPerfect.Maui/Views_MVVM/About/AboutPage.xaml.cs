using PokerPerfect.Maui.Views_MVVM.ScanIp;

namespace PokerPerfect.Maui.Views_MVVM.About;

public partial class AboutPage : ContentPage
{
  private const string WebApiUrlKey = "WebApiBaseUrl"; // Key for stored API URL

  public AboutPage()
	{
		InitializeComponent();

    string savedFeature = Preferences.Get(WebApiUrlKey, "No IP Address Saved");
    savedFeatureLabel.Text = $"Saved: {savedFeature}";
  }

  private async void CloseButton_Clicked(object sender, EventArgs e)
  {
    await Shell.Current.GoToAsync("..");
  }

  private async void SavePreference_Clicked(object sender, EventArgs e)
  {
    await Shell.Current.GoToAsync(nameof(ScanIpAddressPage));
  }
}