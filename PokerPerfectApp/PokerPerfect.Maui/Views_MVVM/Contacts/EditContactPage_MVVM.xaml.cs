using PokerPerfect.Maui.ViewModels.Contacts;

namespace PokerPerfect.Maui.Views_MVVM.Contacts;

[QueryProperty(nameof(ContactId), "ContactId")]

public partial class EditContactPage_MVVM : ContentPage
{
  private readonly ContactViewModel contactViewModel;

  public EditContactPage_MVVM(ContactViewModel contactViewModel)
  {
    InitializeComponent();

    BindingContext = this.contactViewModel = contactViewModel;
  }

  public string ContactId
  {
    set
    {
      if (!string.IsNullOrWhiteSpace(value) && int.TryParse(value, out int contactId))
        LoadContact(contactId);
    }
  }

  private async void LoadContact(int contactId)
  {
    await contactViewModel.LoadContact(contactId);
  }
}