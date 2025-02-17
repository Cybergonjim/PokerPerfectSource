using System.Runtime.CompilerServices;

namespace PokerPerfect.Maui.Views_MVVM.Chipsets;

public partial class ChipsetControl_MVVM : ContentView
{
  public bool IsForEdit { get; set; }
  public bool IsForAdd { get; set; }

  public ChipsetControl_MVVM()
  {
    InitializeComponent();
  }

  protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
  {
    base.OnPropertyChanged(propertyName);

    if (IsForAdd && !IsForEdit)
      btnSave.SetBinding(Button.CommandProperty, "AddChipsetCommand");
    else if (!IsForAdd && IsForEdit)
      btnSave.SetBinding(Button.CommandProperty, "EditChipsetCommand");
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