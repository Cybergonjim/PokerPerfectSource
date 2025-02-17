using PokerPerfect.Maui.ViewModels.Contacts;

namespace PokerPerfect.Maui.Views_MVVM.Contacts;

public partial class Contacts_Page_MVVM : ContentPage
{
  private readonly ContactsViewModel contactsViewModel;

  public Contacts_Page_MVVM(ContactsViewModel contactsViewModel)
  {
    InitializeComponent();

    BindingContext = this.contactsViewModel = contactsViewModel;
  }

  protected override async void OnAppearing()
  {
    base.OnAppearing();

    await contactsViewModel.LoadContacts();
  }
}