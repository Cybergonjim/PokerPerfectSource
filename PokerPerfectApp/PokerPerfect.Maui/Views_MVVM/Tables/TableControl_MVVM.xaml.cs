using System.Runtime.CompilerServices;

namespace PokerPerfect.Maui.Views_MVVM.Tables;

public partial class TableControl_MVVM : ContentView
{
  public bool IsForEdit { get; set; }
  public bool IsForAdd { get; set; }

  public TableControl_MVVM()
  {
    InitializeComponent();
  }

  protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
  {
    base.OnPropertyChanged(propertyName);

    if (IsForAdd && !IsForEdit)
      btnSave.SetBinding(Button.CommandProperty, "AddTableCommand");
    else if (!IsForAdd && IsForEdit)
      btnSave.SetBinding(Button.CommandProperty, "EditTableCommand");
  }

  private void EntryFocused(object sender, FocusEventArgs e)
  {
    Dispatcher.Dispatch(() =>
    {
      var entry = sender as Entry;

      entry.CursorPosition = 0;
      entry.SelectionLength = entry.Text == null ? 0 : entry.Text.Length;
    });
  }
}