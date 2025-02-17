using PokerPerfect.Maui.Views_MVVM.About;
using PokerPerfect.Maui.Views_MVVM.Chipsets;
using PokerPerfect.Maui.Views_MVVM.Contacts;
using PokerPerfect.Maui.Views_MVVM.Games;

namespace PokerPerfect.Maui.Views_MVVM;

public partial class MainPage : ContentPage
{
	public MainPage()
	{
		InitializeComponent();
	}

	private async void OnContactClicked(object sender, EventArgs e)
	{
		Helper.GameId = 0;
		await Shell.Current.GoToAsync(nameof(Contacts_Page_MVVM));
	}

	private async void OnGameClicked(object sender, EventArgs e)
	{
		await Shell.Current.GoToAsync(nameof(Games_Page_MVVM));
	}

	private async void OnChipsetClicked(object sender, EventArgs e)
	{
    await Shell.Current.GoToAsync(nameof(Chipsets_Page_MVVM));
  }

  private async void OnAboutClicked(object sender, EventArgs e)
	{
    await Shell.Current.GoToAsync(nameof(AboutPage));
  }

  private void OnEndClicked(object sender, EventArgs e)
	{
		Application.Current.Quit();
	}

}

