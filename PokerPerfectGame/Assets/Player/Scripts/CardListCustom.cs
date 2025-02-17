using System.Collections.Generic;

public partial class PlayerObject
{
  public class CardListCustom : List<CardObject>
  {
    private PlayerObject ownerInstance;

    public CardListCustom(PlayerObject owner)
    {
      ownerInstance = owner;
    }

    // Override the Clear method to include additional behavior
    public new void Clear()
    {
      base.Clear();

      ownerInstance.CheckButtons();
    }

    // Override the Add method to include additional behavior
    public new void Add(CardObject item)
    {
      base.Add(item);

      ownerInstance.Folded = false;

      // Call CheckButtons when a card is added
      ownerInstance.CheckButtons();
    }

    // Override the Remove method to include additional behavior
    public new bool Remove(CardObject item)
    {
      bool result = base.Remove(item);

      if (result)
        ownerInstance.CheckButtons();

      return result;
    }
  }
}