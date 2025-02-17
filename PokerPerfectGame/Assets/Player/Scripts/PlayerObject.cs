using static FunctionsNameSpace.Functions;
using UnityEngine;
using System.Collections.Generic;
using System;
using TMPro;
using System.Collections;
using FunctionsNameSpace;
using RankHands;
using static GameUtil;
using static PlayerControl;
using HutongGames.PlayMaker;

public class WrappedList<T> : List<T>
{
  public WrappedList() { }

  public WrappedList(IEnumerable<T> collection) : base(collection) { }

  public new T this[int index]
  {
    get => base[index % Count];
    set => base[index % Count] = value;
  }
}

public partial class PlayerObject : MonoBehaviour
{
  public ButtonObject buttonPrefab;

  public GameObject background;
  public TMP_Text positionText;
  public TMP_Text nameText;
  public TMP_Text amountText;
  public TMP_Text betText;

  public GameObject statusButton;
  public TMP_Text statusText;

  public GameObject handButton;
  public TMP_Text rankText;
  public TMP_Text kickersText;

  public GameObject statusScreen;
  public GameObject historyScreen;
  public GameObject peekBox;
  public GameObject sphere;
  public RackObject rackObject;

  public GameObject spinCube;
  public List<TMP_Text> spinCubeTexts;

  public Fsm Fsm => GetComponent<PlayMakerFSM>().Fsm;

  public Material TextMaterial { get; set; }

  public List<string> StandingsTexts { get; set; } = new() { "", "", "" };

  public List<string> HistoryTexts { get; set; } = new() { "", "" };

  public List<CardObject> HistoryCards { get; set; } = new(2);

  public List<TMP_Text> cardPeekRank;
  public List<GameObject> cardPeekSymbol;
  public List<CardObject> cardPositions;
  public List<CardObject> cardDealers;

  private readonly List<int> dealers = new() { 0, 1, 1, 0, 1, 0, 1, 1, 0, 1 };
  private readonly List<float> angles = new() { 0.0f, 0.0f, 0.0f, 90.0f, 90.0f, 180.0f, 180.0f, 180.0f, -90.0f, -90.0f };

  private const float xPlayerSpacing = 2.0f;
  private const float yPlayerSpacing = 1.0f;
  private const float xPlayerSize = 88.0f;
  private const float yPlayerSize = 60.0f;

  private const float xButtonSpacing = -1.0f;
  private const float yButtonSpacing = 0.5f;
  private const float xButtonSize = 16.0f;
  private const float yButtonSize = 8.0f;
  private const float yButtonOffset = 1.0f;

  public CardObject CardDealer => cardDealers[dealers[Index]];

  private List<ButtonObject> Buttons { get; set; }

  // these are for the spinning position types
  public List<Material> dealerMaterials;
  public List<string> dealerNames;

  // these items are for the peeks
  public List<Material> rankMaterials;
  public List<Material> symbolMaterials;
  
  public Rank Rank { get; set; }

  private int MaxRaises { get; set; }

  public int CommitAmount { get; set; }

  public bool AllIn => BetAmount == StartAmount && !Folded;

  public bool Busted => Amount == 0;

  public bool Folded { get; set; }

  public int Index { get; set; }

  private int StartAmount { get; set; }

  public CardListCustom Cards { get; set; }

  private int raises;
  public int Raises
  {
    get => raises;

    set
    {
      raises = value;

      if (raises < 0)
        raises = 0;

      if (raises > MaxRaises)
        raises = MaxRaises;

      if (Raises == 0)
        BetAmount = CommitAmount;
      else
        BetAmount = CallAmount + raises * LastRaiseAmount;
    }
  }

  private int amount;
  public int Amount
  {
    get => amount;

    set
    {
      amount = value;

      amountText.text = amount == 0 ? "" : amount.ToString("N0");

      rackObject.AmountTarget = amount;

      StandingsTexts[1] = amount == 0 ? "ALL-IN" : DenominationToString(amount);
    }
  }

  private int betAmount;
  public int BetAmount
  {
    get => betAmount;

    set
    {
      betAmount = value;

      if (betAmount < CommitAmount)
        betAmount = CommitAmount;
      else if (betAmount > StartAmount)
        betAmount = StartAmount;

      betText.text = betAmount == 0 ? "" : betAmount.ToString("N0");

      if (Focus)
        Amount = StartAmount - betAmount;
    }
  }

  private string action;
  private string Action
  {
    get => action;

    set
    {
      action = value;
      StandingsTexts[2] = Action;
    }
  }

  private string handle = string.Empty;
  public string Handle
  {
    get => handle;

    set
    {
      handle = value;
      StandingsTexts[0] = handle;
    }
  }

  private bool focus;
  public bool Focus
  {
    get => focus;

    set
    {
      background.SetActive(value);

      focus = value;

      CheckButtons();
    }
  }

  private PositionTypes positionType;
  public PositionTypes PositionType
  {
    get => positionType;

    set
    {
      positionType = value;

      Handle = $"{nameText.text} {positionType.GetDescription()}";

      sphere.SetActive(positionType != PositionTypes.None);

      if (positionType != PositionTypes.None)
      {
        // set dealer colors
        spinCube.GetComponent<Renderer>().material = dealerMaterials[(int)positionType];
        
        // set dealer names on 3 upper faces of spinning cube
        foreach (var spinCubeText in spinCubeTexts)
          spinCubeText.text = dealerNames[(int)positionType];

        Focus = true;

        if (positionType == PositionTypes.SmallBlind)
          BetAmount = CommitAmount = SmallBlindAmount;
        else if (positionType == PositionTypes.BigBlind)
          BetAmount = CommitAmount = BigBlindAmount;

        Focus = false;

        Action = dealerNames[(int)positionType];
      }
    }
  }

  public PlayerObject()
  {
    Cards = new CardListCustom(this); // Pass the instance of the containing class

    // Initialize other properties if needed...
    betAmount = CommitAmount = 0;
    positionType = PositionTypes.None;
  }

  public void StartBetting()
  {
    // set up initial conditions for betting which allows the bet amount to fluctuate based on previous raise
    // where player can call or fold, or change bet amount by using All-in, +, - and Reset
    // min reset bet amount to the committed amount. Once bet value is set, the raise button commits the bet amount.
    // the last raise in minimum amount a player can raise so pressing + adds one or more raises to the bet amount up to
    // the total amount. Buttons turn on and off as bet amount changes.

    StartAmount = Amount + BetAmount;

    if (AllIn || Folded || Busted)
      PlayMakerFSM.BroadcastEvent(EventTypes.GameBetEndEvent.ToString());
    else
    {
      // the committed amount is stored so that if player messes with the bet amount and then folds, the committed amount is forfeited
      CommitAmount = BetAmount;
      Raises = 0;
      MaxRaises = (int)Math.Ceiling((double)(StartAmount - CallAmount) / LastRaiseAmount);

      if (CallAmount == 0)
        statusText.text = " CHECK OR RAISE";
      else if (CallAmount > StartAmount)
        statusText.text = "ALL-IN TO CALL";
      else
        statusText.text = CallAmount.ToString() + " TO CALL";

      statusButton.SetActive(true);

      Focus = true;
    }
  }

  public void EndBetting()
  {
    statusText.text = "";

    statusButton.SetActive(false);

    // the last raise is the amount over the callAmount
    if (BetAmount > CallAmount)
    {
      CallAmount = BetAmount;

      if (BetAmount - CallAmount > LastRaiseAmount)
        LastRaiseAmount = BetAmount - CallAmount;

      LastActionIndex = ActionIndex - 1;
    }

    CommitAmount = 0;

    Focus = false;

    Game_.Fsm.Event(EventTypes.GameBetEndEvent.ToString());
  }

  public void ShowPeekCards()
  {
    if (Cards.Count != 0)
      for (int i = 0; i < 2; i++)
      {
        cardPeekRank[i].text = Cards[i].RankType.GetDescription();
        cardPeekRank[i].GetComponent<TMP_Text>().fontMaterial = rankMaterials[(int)Cards[i].SuitType];
        cardPeekSymbol[i].GetComponent<Renderer>().material = symbolMaterials[(int)Cards[i].SuitType];

        peekBox.SetActive(true);
    }
  }

  public void ShowCards()
  {
    Cards.ForEach(card => card.transform.Rotate(0, 180.0f, 0));
  }

  public void HandBegin()
  {
    StartAmount = Amount;

    BetAmount = CommitAmount = 0;

    Action = "";

    HistoryCards.Clear();
    HistoryCards.AddRange(Cards);

    PositionType = PositionTypes.None;

    HistoryTexts[0] = $"{nameText.text}";
    HistoryTexts[1] = Rank.HandDescription + "-" + Rank.KickersDescription;

    Cards.Clear();
    CheckButtons();
  }

  public void FillStatusScreen()
  {
    historyScreen.SetActive(false);
    statusScreen.SetActive(!statusScreen.activeSelf);
  }

  public void FillHistoryScreen()
  {
    statusScreen.SetActive(false);
    historyScreen.SetActive(!historyScreen.activeSelf);
  }

  public void Fold()
  {
    BetAmount = CommitAmount;

    Folded = true;

    EndBetting();

    PlayMakerFSM fsm = GetComponent<PlayMakerFSM>();

    Fsm.Event(EventTypes.PlayerFoldEvent.ToString());
  }

  public void ProcessButtons(ButtonTypes buttonType, bool isLeft)
  {
    switch (buttonType)
    {
      case ButtonTypes.Peek:
        ShowPeekCards();
        StartCoroutine(PeekBoxOff());
        break;

      case ButtonTypes.Status:
        if (isLeft)
          FillStatusScreen();
        else
          FillHistoryScreen();
        break;

      case ButtonTypes.Fold:
        Action = "Fold";
        Fold();
        break;

      case ButtonTypes.Call:
        BetAmount = CallAmount;
        Action = "Call " + DenominationToString(CallAmount);
        EndBetting();
        break;

      case ButtonTypes.Raise:

        Action = "Raise " + DenominationToString(BetAmount - CallAmount) + " to " + DenominationToString(BetAmount);
        EndBetting();
        break;

      case ButtonTypes.Reset:
        Raises = 0;
        BetAmount = 0;
        break;

      case ButtonTypes.Minus:
        if (isLeft)
          Raises--;
        else
          Raises -= 10;
        break;

      case ButtonTypes.Plus:
        if (isLeft)
          Raises++;
        else
          Raises += 10;
        break;

      case ButtonTypes.AllIn:
        Raises = MaxRaises;
        Action = "All In for " + DenominationToString(BetAmount);
        break;
    }

    CheckButtons();
  }

  private void CheckButtons()
  {
    // the blocker is an object existing over the button that darkens the button and blocks any button clicks
    // when the blocker is active so the button doesn't function.
    foreach (var button in Buttons)
      button.blocker.SetActive(!CheckButton(button.buttonType));
  }

  private bool CheckButton(ButtonTypes buttonType)
  {
    bool result = false;

    switch (buttonType)
    {
      case ButtonTypes.Peek:
        result = !Folded && Cards.Count > 0;
        break;

      case ButtonTypes.Status:
        result = Cards.Count > 0;
        break;

      case ButtonTypes.Fold:
        result = Focus && !Folded && BetAmount != CallAmount;
        break;

      case ButtonTypes.Call:
        result = Focus;
        break;

      case ButtonTypes.AllIn:
        result = Focus && !AllIn && StartAmount > CallAmount;
        break;

      case ButtonTypes.Plus:
        result = Focus && Raises != MaxRaises && StartAmount > CallAmount;
        break;

      case ButtonTypes.Minus:
        result = Focus && Raises != 0 && StartAmount > CallAmount;
        break;

      case ButtonTypes.Reset:
        result = Focus && BetAmount != CommitAmount && StartAmount > CallAmount;
        break;

      case ButtonTypes.Raise:
        result = Focus && BetAmount > CallAmount && StartAmount > CallAmount;
        break;
    }

    return result;
  }

  IEnumerator PeekBoxOff()
  {
    const float delay = 2.0f;

    yield return new WaitForSeconds(delay);

    peekBox.gameObject.SetActive(false);
  }
    
  void Start()
  {
//    if (Index == 0)
//      Tester.TestHands();
  }

  public void SetupPlayerObject()
  {
    Rank = new Rank(this);

    positionText.text = $"#{Index + 1}";

    CreateButtons();

    name = $"Player #{Index}";
    tag = $"Player{Index}";

    Vector3 position = GetPosition(Index);
    transform.position = position;
    transform.Rotate(0.0f, 0.0f, angles[Index], Space.Self);

    nameText.text = FilteredPlayers[Index].handle;
    nameText.gameObject.GetComponent<Renderer>().material = TextMaterial;

    rackObject.AmountTarget = StartAmount = Amount = FilteredPlayers[Index].amount;

    BetAmount = CommitAmount = 0;
    PositionType = PositionTypes.None;

    background.SetActive(false);
    statusScreen.SetActive(false);
    historyScreen.SetActive(false);
  }

  private Vector3 GetPosition(int index)
  {
    float playerX, playerY;

    xOffset = (xTableSize - xPlayerSpacing * 2 - xPlayerSize * 3) / 2;
    yOffset = (yTableSize - yPlayerSpacing - xPlayerSize * 2) / 2;

    if (index < 3)
    {
      playerX = -xTableSize / 2 + xOffset + xPlayerSize / 2 + index * (xPlayerSize + xPlayerSpacing);
      playerY = -yTableSize / 2 + yPlayerSize / 2;
    }
    else if (index < 5)
    {
      playerX = xTableSize / 2 - yPlayerSize / 2;
      playerY = -yTableSize / 2 + yOffset + xPlayerSize / 2 + (index - 3) * (xPlayerSize + yPlayerSpacing);
    }
    else if (index < 8)
    {
      playerX = xTableSize / 2 - xOffset + xPlayerSize / 2 - (index - 4) * (xPlayerSize + xPlayerSpacing);
      playerY = yTableSize / 2 - yPlayerSize / 2;
    }
    else
    {
      playerX = -xTableSize / 2 + yPlayerSize / 2;
      playerY = yTableSize / 2 - yOffset + xPlayerSize / 2 - (index - 7) * (xPlayerSize + yPlayerSpacing);
    }

    return new Vector3(playerX, playerY, 0);
  }

  private void CreateButtons()
  {
    const int NumButtons = 5;
    const int NumSpaces = 4;

    Buttons = new List<ButtonObject>();

    float xOffset = (xPlayerSize - xButtonSpacing * NumSpaces - xButtonSize * NumButtons) / 2;

    foreach (ButtonTypes buttonType in Enum.GetValues(typeof(ButtonTypes)))
    {
      ButtonObject buttonObject = Instantiate(buttonPrefab, Vector3.zero, Quaternion.identity);

      int index = (int)buttonType;

      buttonObject.transform.parent = transform;
      buttonObject.index = index;
      buttonObject.buttonType = buttonType;

      float buttonX = (index < NumButtons) ? -xPlayerSize / 2 + xOffset + xButtonSize / 2 + index * (xButtonSize + xButtonSpacing) : xPlayerSize / 2 - xOffset - xButtonSize / 2;
      float buttonY = -yPlayerSize / 2 + yButtonOffset + yButtonSize / 2 + ((index < NumButtons) ? 0 : (index - NumSpaces) * (yButtonSize + yButtonSpacing));

      buttonObject.position = new Vector3(buttonX, buttonY, 0);

      Buttons.Add(buttonObject);
    }
  }
}