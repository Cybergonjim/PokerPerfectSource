using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PokerPerfect.CoreBusiness;
using PokerPerfect.Maui.Views_MVVM;
using PokerPerfect.Maui.Views_MVVM.Contacts;
using PokerPerfect.Maui.Views_MVVM.Players;
using PokerPerfect.UseCases.Interfaces.Contacts;
using PokerPerfect.UseCases.Interfaces.Games;
using PokerPerfect.UseCases.Interfaces.Players;
using Microsoft.Maui.ApplicationModel.Communication;
using System.Collections.ObjectModel;
using Contact = PokerPerfect.CoreBusiness.Contact;

namespace PokerPerfect.Maui.ViewModels.Contacts
{
	public partial class ContactsViewModel : ObservableObject
	{
		private readonly IViewContactsUseCase viewContactsUseCase;
		private readonly IDeleteContactUseCase deleteContactUseCase;
		private readonly IViewContactUseCase viewContactUseCase;
		private readonly IAddPlayerUseCase addPlayerUseCase;
		private readonly IViewGameUseCase viewGameUseCase;
		private readonly IViewPlayersUseCase viewPlayersUseCase;
		private string filterText;

		public ObservableCollection<Contact> Contacts { get; set; }

		public string FilterText
		{
			get => filterText;
			set => _ = LoadContacts(filterText = value);
		}

		public ContactsViewModel(
				IViewContactsUseCase viewContactsUseCase,
				IDeleteContactUseCase deleteContactUseCase,
				IViewContactUseCase viewContactUseCase,
				IAddPlayerUseCase addPlayerUseCase,
				IViewGameUseCase viewGameUseCase,
				IViewPlayersUseCase viewPlayersUseCase
				)
		{
			Contacts = new ObservableCollection<Contact>();

			this.viewContactsUseCase = viewContactsUseCase;
			this.deleteContactUseCase = deleteContactUseCase;
			this.viewContactUseCase = viewContactUseCase;
			this.addPlayerUseCase = addPlayerUseCase;
			this.viewGameUseCase = viewGameUseCase;
			this.viewPlayersUseCase = viewPlayersUseCase;
		}

		public async Task LoadContacts(string filterText = null)
		{
			Contacts.Clear();

			// if Helper.GameId == 0 then contacts came from MainPage
			if (Helper.GameId == 0)
			{
				var contacts = await viewContactsUseCase.ExecuteAsync(filterText);

				if (contacts != null && contacts.Count > 0)
					foreach (var contact in contacts)
					{
						contact.Use = "F";
						Contacts.Add(contact);
					}
			}
			else // else Contacts_Page_MVVM came from Players_Page_MVVM
			{
				// list can be filtered by search bar
				var contacts = await viewContactsUseCase.ExecuteAsync(filterText);
				var players = await viewPlayersUseCase.ExecuteAsync(Helper.GameId.ToString());

				// if the current list of players are found in filtered contacts list, the contact is removed from list
				foreach (var player in players)
				{
					int index = contacts.IndexOf(contacts.Where(p => p.ContactId == player.ContactId).FirstOrDefault());

					if (index != -1)
						contacts.RemoveAt(index);
				}

				// the remaining contacts are displayed
				foreach (var contact in contacts)
				{
					contact.Use = "T";
					Contacts.Add(contact);
				}
			}
		}

		[RelayCommand]
		public async Task AddContact(int contactId)
		{
			Contact contact = await viewContactUseCase.ExecuteAsync(contactId);
			Game game = await viewGameUseCase.ExecuteAsync(Helper.GameId);

      Player player = new()
      {
        GameId = Helper.GameId,
        ContactId = contactId,
        Name = contact.Name,
        Handle = contact.Handle,
        Amount = game.ChipsStart,
        Rebuys = game.Rebuys
      };

      await addPlayerUseCase.ExecuteAsync(player);

			await LoadContacts();
		}

		[RelayCommand]
		public async Task DeleteContact(int contactId)
		{
			await deleteContactUseCase.ExecuteAsync(contactId);

			await LoadContacts(null);
		}

		[RelayCommand]
		public async Task GotoEditContact(int contactId)
		{
			await Shell.Current.GoToAsync($"{nameof(EditContactPage_MVVM)}?ContactId={contactId}");
		}

		[RelayCommand]
		public async Task GotoAddContact()
		{
			await Shell.Current.GoToAsync(nameof(AddContactPage_MVVM));
		}

		[RelayCommand]
		public async Task GotoHome()
		{
			await Shell.Current.GoToAsync("..");
		}
	}
}
