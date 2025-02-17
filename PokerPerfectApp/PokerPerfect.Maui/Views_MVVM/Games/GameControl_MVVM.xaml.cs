using System.Runtime.CompilerServices;

namespace PokerPerfect.Maui.Views_MVVM.Games;

public partial class GameControl_MVVM : ContentView
{
  public bool IsForEdit { get; set; }
  public bool IsForAdd { get; set; }

  public GameControl_MVVM()
  {
    InitializeComponent();

    ChipsetNamesPicker.ItemsSource = Helper.Names;
  }

  protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
  {
    base.OnPropertyChanged(propertyName);

    if (IsForAdd && !IsForEdit)
      btnSave.SetBinding(Button.CommandProperty, "AddGameCommand");
    else if (!IsForAdd && IsForEdit)
      btnSave.SetBinding(Button.CommandProperty, "EditGameCommand");
  }

  private void RebuysClicked(object sender, EventArgs e)
  {
    RebuysStack.IsVisible = !RebuysStack.IsVisible;
  }

  private void AddonsClicked(object sender, EventArgs e)
  {
    AddonsStack.IsVisible = !AddonsStack.IsVisible;
  }

  private void TimersClicked(object sender, EventArgs e)
  {
    TimersStack.IsVisible = !TimersStack.IsVisible;
  }

  private void InitailClicked(object sender, EventArgs e)
  {
    InitialStack.IsVisible = !InitialStack.IsVisible;
  }

  private void GeneralClicked(object sender, EventArgs e)
  {
    GeneralStack.IsVisible = !GeneralStack.IsVisible;
  }

  private void FinishClicked(object sender, EventArgs e)
  {
    FinishStack.IsVisible = !FinishStack.IsVisible;
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