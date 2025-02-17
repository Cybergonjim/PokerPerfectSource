using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PokerPerfect.UseCases.Interfaces.Tables;
using PokerPerfect.CoreBusiness;

namespace PokerPerfect.Maui.ViewModels.Tables
{
	public partial class TableViewModel : ObservableObject
	{
		private Table table;
		private readonly IViewTableUseCase viewTableUseCase;
		private readonly IEditTableUseCase editTableUseCase;
		private readonly IAddTableUseCase addTableUseCase;

		public Table Table
		{
			get => table;
			set => SetProperty(ref table, value);
		}

		public bool IsNameProvided { get; set; }

		public TableViewModel(
				IViewTableUseCase viewTableUseCase,
				IEditTableUseCase editTableUseCase,
				IAddTableUseCase addTableUseCase)
		{
			Table = new Table();

			this.viewTableUseCase = viewTableUseCase;
			this.editTableUseCase = editTableUseCase;
			this.addTableUseCase = addTableUseCase;
		}

		public async Task LoadTable(int tableId)
		{
			Table = await viewTableUseCase.ExecuteAsync(tableId);
		}

		[RelayCommand]
		public async Task EditTable()
		{
			await editTableUseCase.ExecuteAsync(table.TableId, table);
			await Shell.Current.GoToAsync("..");
		}

		[RelayCommand]
		public async Task AddTable()
		{

			await addTableUseCase.ExecuteAsync(table);
			await Shell.Current.GoToAsync("..");
		}

		[RelayCommand]
		public async Task BackToTables()
		{
			await Shell.Current.GoToAsync("..");
		}
	}
}
