using CommunityToolkit.Maui.Behaviors;
using System.Runtime.CompilerServices;

namespace PokerPerfect.Maui.Views_MVVM.Chips;

public partial class ChipControl_MVVM : ContentView
{
  public bool IsForEdit { get; set; }
  public bool IsForAdd { get; set; }

  private readonly Controls controls;

  private class Controls
  {
    public enum ChipPos
    {
      Base = 0,
      Spoke = 1,
      Dot = 2,
      None = 3
    }

    private ChipControl_MVVM chipControl;
    private Button buttonToModify;
    private Image imageToModify;
    private Color pickedColor;
    private Entry entryR;
    private Entry entryG;
    private Entry entryB;
    private ChipPos chipLocation;
    private bool okayToProcess;

    public Controls(ChipControl_MVVM chipControl_MVVM)
    {
      chipControl = chipControl_MVVM;

      okayToProcess = false;
      ChipLocation = ChipPos.None;
      PickedColor = Color.FromInt(0);
//      Helper.ShowMessage("Control Created");
    }

    private ChipControl_MVVM ChipControl
    {
      get => chipControl;
    }

    public bool OkayToProcess
    {
      get => okayToProcess;
      set => okayToProcess = value;
    }

    public ChipPos ChipLocation
    {
      set
      {
        chipLocation = value;

        switch (chipLocation)
        {
          case ChipPos.Base:
            {
              okayToProcess = true;
              buttonToModify = ChipControl.BaseButton;
              imageToModify = ChipControl.BaseImage;
              entryR = ChipControl.BaseEntryR;
              entryG = ChipControl.BaseEntryG;
              entryB = ChipControl.BaseEntryB;
              break;
            }
          case ChipPos.Spoke:
            {
              okayToProcess = true;
              buttonToModify = ChipControl.SpokeButton;
              imageToModify = ChipControl.SpokeImage;
              entryR = ChipControl.SpokeEntryR;
              entryG = ChipControl.SpokeEntryG;
              entryB = ChipControl.SpokeEntryB;
              break;
            }
          case ChipPos.Dot:
            {
              okayToProcess = true;
              buttonToModify = ChipControl.DotButton;
              imageToModify = ChipControl.DotImage;
              entryR = ChipControl.DotEntryR;
              entryG = ChipControl.DotEntryG;
              entryB = ChipControl.DotEntryB;
              break;
            }
          default:
            {
              break;
            }
        }
      }
    }

    public Color PickedColor
    {
      set
      {
        pickedColor = value;

        if (chipLocation == ChipPos.None)
        {
          ChipControl.BaseImage.Behaviors.Add(new IconTintColorBehavior() { TintColor = ChipControl.BaseButton.BackgroundColor });
          ChipControl.SpokeImage.Behaviors.Add(new IconTintColorBehavior() { TintColor = ChipControl.SpokeButton.BackgroundColor });
          ChipControl.DotImage.Behaviors.Add(new IconTintColorBehavior() { TintColor = ChipControl.DotButton.BackgroundColor });
        }
        else
        {
          buttonToModify.BackgroundColor = pickedColor;
          imageToModify.Behaviors.Add(new IconTintColorBehavior() { TintColor = pickedColor });

          pickedColor.ToRgb(out byte r, out byte g, out byte b);

          //Helper.ShowMessage("colors set to " + r.ToString() + " " + g.ToString() + " " + b.ToString());

          entryR.Text = r.ToString();
          entryG.Text = g.ToString();
          entryB.Text = b.ToString();
        }
      }
    }
  }

  public ChipControl_MVVM()
  {
    InitializeComponent();
    controls = new(this);
  }

  protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
  {
    base.OnPropertyChanged(propertyName);

    if (IsForAdd && !IsForEdit)
      btnSave.SetBinding(Button.CommandProperty, "AddChipCommand");
    else if (!IsForAdd && IsForEdit)
      btnSave.SetBinding(Button.CommandProperty, "EditChipCommand");
  }

  private void BaseBtnColorClicked(object sender, EventArgs e)
  {
    controls.ChipLocation = Controls.ChipPos.Base;
  }

  private void SpokeBtnColorClicked(object sender, EventArgs e)
  {
    controls.ChipLocation = Controls.ChipPos.Spoke;
  }

  private void DotBtnColorClicked(object sender, EventArgs e)
  {
    controls.ChipLocation = Controls.ChipPos.Dot;
  }

  private void ColorPickerChanged(object sender, Color e)
  {
    controls.PickedColor = e;
  }

  private void DenomEntryPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
  {
    if ((DenomLabel != null) && (DenomEntry != null))
      DenomLabel.Text = DenomEntry.Text;
  }

  private void CheckEntries(Controls.ChipPos chipPos, params Entry[] entries)
  {
    if (controls.OkayToProcess == false)
      return;

    foreach (Entry entry in entries)
      if ((entry == null) || String.IsNullOrWhiteSpace(entry.Text))
        return;

    controls.ChipLocation = chipPos;
    controls.PickedColor = new(byte.Parse(entries[0].Text), byte.Parse(entries[1].Text), byte.Parse(entries[2].Text));
  }

  private void BaseRGBEntryChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
  {
    CheckEntries(Controls.ChipPos.Base, new[] { BaseEntryR, BaseEntryG, BaseEntryB });
  }

  private void SpokeRGBEntryChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
  {
    CheckEntries(Controls.ChipPos.Spoke, new[] { SpokeEntryR, SpokeEntryG, SpokeEntryB });
  }

  private void DotRGBEntryChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
  {
    CheckEntries(Controls.ChipPos.Dot, new[] { DotEntryR, DotEntryG, DotEntryB });
  }

  private void EntryFocused(object sender, FocusEventArgs e)
  {
    controls.OkayToProcess = true;

    Dispatcher.Dispatch(() =>
    {
      var entry = sender as Entry;

      entry.CursorPosition = 0;
      entry.SelectionLength = entry.Text == null ? 0 : entry.Text.Length;
    });
  }
}