using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PokerPerfect.Maui.Views_MVVM.Payouts;
using System.Collections.ObjectModel;
using PokerPerfect.CoreBusiness;
using PokerPerfect.UseCases.Interfaces.Payouts;
using PokerPerfect.Maui.Views_MVVM.Games;

namespace PokerPerfect.Maui.ViewModels.Payouts
{
	public partial class PayoutsViewModel : ObservableObject
	{
		private readonly IViewPayoutsUseCase viewPayoutsUseCase;
		private readonly IDeletePayoutUseCase deletePayoutUseCase;
		private readonly IEditPayoutUseCase editPayoutUseCase;

		public ObservableCollection<Payout> Payouts { get; set; }

		public PayoutsViewModel(
				IViewPayoutsUseCase viewPayoutsUseCase,
				IDeletePayoutUseCase deletePayoutUseCase,
				IEditPayoutUseCase editPayoutUseCase
				)
		{
			Payouts = new ObservableCollection<Payout>();

			this.viewPayoutsUseCase = viewPayoutsUseCase;
			this.deletePayoutUseCase = deletePayoutUseCase;
			this.editPayoutUseCase = editPayoutUseCase;
		}

		public async Task LoadPayouts()
		{
			Payouts.Clear();

			var payouts = await viewPayoutsUseCase.ExecuteAsync(Helper.GameId.ToString());

			payouts.Sort((x, y) => x.PayoutNo.CompareTo(y.PayoutNo));

			if (payouts != null && payouts.Count > 0)
				foreach (var payout in payouts)
					Payouts.Add(payout);
		}

		[RelayCommand]
		public async Task DeletePayout(int payoutId)
		{
			await deletePayoutUseCase.ExecuteAsync(payoutId);

			await LoadPayouts();
		}

		[RelayCommand]
		public async Task GotoEditPayout(int payoutId)
		{
			await Shell.Current.GoToAsync($"{nameof(EditPayoutPage_MVVM)}?PayoutId={payoutId}");
		}

		[RelayCommand]
		public async Task GotoAddPayout()
		{
			await Shell.Current.GoToAsync(nameof(AddPayoutPage_MVVM));
		}

		[RelayCommand]
		public async Task GoBack()
		{
			await Shell.Current.GoToAsync("..");
		}
	}
}
