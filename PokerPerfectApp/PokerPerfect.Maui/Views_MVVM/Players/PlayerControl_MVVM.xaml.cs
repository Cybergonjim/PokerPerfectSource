using System.Runtime.CompilerServices;

namespace PokerPerfect.Maui.Views_MVVM.Players;

public partial class PlayerControl_MVVM : ContentView
{
  public bool IsForEdit { get; set; }
  public bool IsForAdd { get; set; }

  public PlayerControl_MVVM()
  {
    InitializeComponent();
  }

  protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
  {
    base.OnPropertyChanged(propertyName);

    if (IsForAdd && !IsForEdit)
      btnSave.SetBinding(Button.CommandProperty, "AddPlayerCommand");
    else if (!IsForAdd && IsForEdit)
      btnSave.SetBinding(Button.CommandProperty, "EditPlayerCommand");
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