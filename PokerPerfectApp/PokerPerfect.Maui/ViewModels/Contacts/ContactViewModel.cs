using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PokerPerfect.Maui.Views_MVVM.Contacts;
using PokerPerfect.UseCases.Interfaces.Contacts;
using Contact = PokerPerfect.CoreBusiness.Contact;

namespace PokerPerfect.Maui.ViewModels.Contacts
{
	public partial class ContactViewModel : ObservableObject
	{
		private Contact contact;
		private readonly IViewContactUseCase viewContactUseCase;
		private readonly IEditContactUseCase editContactUseCase;
		private readonly IAddContactUseCase addContactUseCase;

		public Contact Contact
		{
			get => contact;
			set => SetProperty(ref contact, value);
		}

		public bool IsNameProvided { get; set; }
		public bool IsEmailProvided { get; set; }
		public bool IsEmailFormatValid { get; set; }


		public ContactViewModel(
				IViewContactUseCase viewContactUseCase,
				IEditContactUseCase editContactUseCase,
				IAddContactUseCase addContactUseCase)
		{
			Contact = new Contact();
			this.viewContactUseCase = viewContactUseCase;
			this.editContactUseCase = editContactUseCase;
			this.addContactUseCase = addContactUseCase;
		}

		public async Task LoadContact(int contactId)
		{
			Contact = await viewContactUseCase.ExecuteAsync(contactId);
		}

		[RelayCommand]
		public async Task EditContact()
		{
			if (await ValidateContact())
			{
				await editContactUseCase.ExecuteAsync(contact.ContactId, this.contact);
				await Shell.Current.GoToAsync("..");
			}
		}

		[RelayCommand]
		public async Task AddContact()
		{
			if (await ValidateContact())
			{
				await addContactUseCase.ExecuteAsync(contact);
				await Shell.Current.GoToAsync("..");
			}
		}

		[RelayCommand]
		public async Task BackToContacts()
		{
			await Shell.Current.GoToAsync("..");
		}

		private async Task<bool> ValidateContact()
		{
			if (!IsNameProvided)
			{
				await Application.Current.MainPage.DisplayAlert("Error", "Name is required.", "OK");
				return false;
			}

			return true;
		}
	}
}
