using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PokerPerfect.Maui.Views_MVVM.Tables;
using PokerPerfect.UseCases.Interfaces.Tables;
using System.Collections.ObjectModel;
using PokerPerfect.CoreBusiness;

namespace PokerPerfect.Maui.ViewModels.Tables
{
	public partial class TablesViewModel : ObservableObject
	{
		private readonly IViewTablesUseCase viewTablesUseCase;
		private readonly IDeleteTableUseCase deleteTableUseCase;
		private readonly IEditTableUseCase editTableUseCase;

		public ObservableCollection<Table> Tables { get; set; }

		public TablesViewModel(
						IViewTablesUseCase viewTablesUseCase,
						IDeleteTableUseCase deleteTableUseCase,
						IEditTableUseCase editTableUseCase
						)
		{
			Tables = new ObservableCollection<Table>();

			this.viewTablesUseCase = viewTablesUseCase;
			this.deleteTableUseCase = deleteTableUseCase;
			this.editTableUseCase = editTableUseCase;
		}

		public async Task LoadTables()
		{
			Tables.Clear();

			var tables = await viewTablesUseCase.ExecuteAsync(Helper.GameId.ToString());

			if (tables != null && tables.Count > 0)
				foreach (var table in tables)
					Tables.Add(table);
		}

		[RelayCommand]
		public async Task DeleteTable(int tableId)
		{
			await deleteTableUseCase.ExecuteAsync(tableId);

			await LoadTables();
		}

		[RelayCommand]
		public async Task GotoEditTable(int tableId)
		{
			await Shell.Current.GoToAsync($"{nameof(EditTablePage_MVVM)}?TableId={tableId}");
		}

		[RelayCommand]
		public async Task GotoAddTable()
		{
			await Shell.Current.GoToAsync(nameof(AddTablePage_MVVM));
		}

		[RelayCommand]
		public async Task GoBack()
		{
			await Shell.Current.GoToAsync("..");
		}
	}
}
