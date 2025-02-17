using ZXing.Net.Maui;

namespace PokerPerfect.Maui.Views_MVVM.ScanIp;

public partial class ScanIpAddressPage : ContentPage
{

  private const string WebApiUrlKey = "WebApiBaseUrl"; // Key for stored API URL
  private bool isAlertShowing = false; // Add this flag

  public ScanIpAddressPage()
	{
		InitializeComponent();

    string savedFeature = Preferences.Get(WebApiUrlKey, "No IP Address Saved");
    savedFeatureLabel.Text = $"Saved: {savedFeature}";
  }
    private async void CloseButton_Clicked(object sender, EventArgs e)
  {
    await Shell.Current.GoToAsync(".."); // Navigate back
  }

  protected void BarcodesDetected(object sender, BarcodeDetectionEventArgs e)
  {
    var first = e.Results?.FirstOrDefault();
    if (first == null) return;

    string address = first.Value;

    if (!string.IsNullOrWhiteSpace(address))
    {
      Preferences.Set(WebApiUrlKey, address); // Save the address

      string savedFeature = Preferences.Get(WebApiUrlKey, "No IP Address Saved");

      MainThread.BeginInvokeOnMainThread(() =>
      {
        savedFeatureLabel.Text = $"Saved: {savedFeature}";
      });

      if (!isAlertShowing) // Check if alert is already showing
      {
        isAlertShowing = true; // Set flag to prevent multiple alerts
        MainThread.BeginInvokeOnMainThread(async () =>
        {
          await DisplayAlert("Barcode Detected", first.Value, "OK");
          isAlertShowing = false; // Reset flag after alert is dismissed
        });
      }
    }
  }


}