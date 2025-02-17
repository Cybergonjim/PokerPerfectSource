using System.Collections.Generic;
using System.Text;
using FunctionsNameSpace;
using System.Linq;
using System;
using static RankHands.RankUtil;
using System.ComponentModel;
using static FunctionsNameSpace.EnumExtensions;

namespace RankHands
{
  public static class RankUtil
  {
    public enum SuitTypes
    {
      Clubs,
      Diamonds,
      Hearts,
      Spades,
      None,
    }

    public enum RankTypes
    {
      [Description("A")]
      AceLow,
      [Description("2")]
      Two,
      [Description("3")]
      Three,
      [Description("4")]
      Four,
      [Description("5")]
      Five,
      [Description("6")]
      Six,
      [Description("7")]
      Seven,
      [Description("8")]
      Eight,
      [Description("9")]
      Nine,
      [Description("10")]
      Ten,
      [Description("J")]
      Jack,
      [Description("Q")]
      Queen,
      [Description("K")]
      King,
      [Description("A")]
      AceHigh,
      [Description("None")]
      None,
    }

    public enum HandTypes
    {
      [Description("-")]
      Folded,
      [Description("High Card")]
      High_Card,
      [Description("Pair")]
      One_Pair,
      [Description("2 Pair")]
      Two_Pair,
      [Description("3 of a Kind")]
      Three_of_a_Kind,
      [Description("Straight")]
      Straight,
      [Description("Flush")]
      Flush,
      [Description("Full House")]
      Full_House,
      [Description("4 of a Kind")]
      Four_of_a_Kind,
      [Description("Straight Flush")]
      Straight_Flush,
      [Description("Royal Flush")]
      Royal_Flush,
    }
  }

  public class RankCard
  {
    public RankCard(RankTypes rankType, SuitTypes suitType)
    {
      RankType = rankType;
      SuitType = suitType;
    }

    public RankTypes RankType { get; set; }

    public SuitTypes SuitType { get; set; }
  }

  public class Rank
  {
    public const int NumSuits = 4;
    public const int NumRanks = 14; // Ranks are from 0 to 13 (e.g., Ace=0 2=1 ... K=12 Ace=13)
    public const int NumCards = 5; // number of cards in straight
    public const int HighCard = 1;
    public const int TwoOfAKind = 2;
    public const int ThreeOfAKind = 3;
    public const int FourOfAKind = 4;
    public const int NotFound = -1;

    public PlayerObject PlayerObject { get; set; }

    public List<RankCard> CardKickers { get; set; }

    public Rank(PlayerObject playerObject)
    {
      PlayerObject = playerObject;

      CardKickers = new List<RankCard>
      {
        new RankCard(RankTypes.None, SuitTypes.None),
        new RankCard(RankTypes.None, SuitTypes.None),
        new RankCard(RankTypes.None, SuitTypes.None),
        new RankCard(RankTypes.None, SuitTypes.None),
        new RankCard(RankTypes.None, SuitTypes.None)
      };
    }

    public HandTypes HandType { get; set; }

    public int Value
    {
      get
      {
        // Player hands can be compared directly to determine winners
        //                                              0-9     0-14      0-14      0-14      0-14      0-14
        // Value is a 24 bit number 6 x 4 bits where | Value |Kicker[0]|Kicker[1]|Kicker[2]|Kicker[3]|Kicker[4]|
        
        // this is a value between 0 and 9
        int value = (int)HandType;

        value = CardKickers.Aggregate(value, (currentValue, card) =>
        {
          currentValue *= 16;
          if (card.RankType != RankTypes.None)
            currentValue += (int)card.RankType;
          return currentValue;
        });

        return value;
      }
    }

    public string Description
    {
      get
      {
        StringBuilder sb = new StringBuilder();

        sb.Append(HandType.GetDescription() + " - ");

        for (int i = 0; i < CardKickers.Count; i++)
          if (CardKickers[i].RankType != RankTypes.None)
            sb.Append(CardKickers[i].RankType.GetDescription() + ", ");

        string str = sb.ToString();

        // remove last ", "
        return str.Substring(0, str.Length - 2);
      }
    }

    public string HandDescription
    {
      get
      {
        return HandType.GetDescription();
      }
    }

    public string KickersDescription
    {
      get
      {
        StringBuilder sb = new StringBuilder();
        int start = 0;
        int count = 0;

        switch (HandType)
        {
          case HandTypes.Folded:
            break;

          case HandTypes.High_Card:
            count = 5;
            break;

          case HandTypes.One_Pair:
            sb.Append(CardKickers[start++].RankType.GetDescription() + "'s|");
            count = 3;
            break;

          case HandTypes.Two_Pair:
            sb.Append(CardKickers[start++].RankType.GetDescription() + "'s & " + CardKickers[start++].RankType.GetDescription() + "'s|");
            count = 1;
            break;

          case HandTypes.Three_of_a_Kind:
            sb.Append(CardKickers[start++].RankType.GetDescription() + "'s|");
            count = 2;
            break;

          case HandTypes.Straight:
            sb.Append(CardKickers[start++].RankType.GetDescription() + " high");
            count = 0;
            break;

          case HandTypes.Flush:
            sb.Append(CardKickers[start++].RankType.GetDescription() + " high|");
            count = 4;
            break;

          case HandTypes.Full_House:
            sb.Append(CardKickers[start++].RankType.GetDescription() + "'s full of " + CardKickers[start++].RankType.GetDescription() + "'s");
            count = 0;
            break;

          case HandTypes.Four_of_a_Kind:
            sb.Append(CardKickers[start++].RankType.GetDescription() + "'s|");
            count = 1;
            break;

          case HandTypes.Straight_Flush:
            sb.Append(CardKickers[start++].RankType.GetDescription() + " high");
            count = 0;
            break;

          case HandTypes.Royal_Flush:
            count = 0;
            break;
        }

        const string separator = ".";
        if (count > 0)
        {
          for (int i = start; i < start + count; i++)
            sb.Append(CardKickers[i].RankType.GetDescription() + separator);

          // remove last " · "
          string str = sb.ToString();
          return str.Substring(0, str.Length - separator.Length);
        }

        return sb.ToString();
      }
    }

    public void RankHand()
    {
      if (PlayerObject.Cards.Count == 0)
      {
        HandType = HandTypes.Folded;
        ClearKickers();

        return;
      }

      List<List<int>> deck = new List<List<int>>();

      for (int i = 0; i < NumSuits; i++)
        deck.Add(Enumerable.Repeat(0, NumRanks).ToList());

      // clear card kickers
      CardKickers.ForEach(cardKicker => 
      {
        cardKicker.RankType = RankTypes.None;
        cardKicker.SuitType = SuitTypes.None;
      });

      PopulateDeck(deck, DealerUtil.CommunityCards);
      PopulateDeck(deck, PlayerObject.Cards);

      CopyAces(deck);

      if (!IsStraightFlush(deck))
        if (!IsFlush(deck))
          if (!IsStraight(deck))
            CheckSameKind(deck);

      Console.WriteLine(Description);
    }

    private void PopulateDeck(List<List<int>> deck, List<CardObject> cards)
    {
      foreach (var card in cards)
        deck[(int)card.SuitType][(int)card.RankType] = 1;
    }

    public void CopyAces(List<List<int>> deck)
    {
      for (int suit = 0; suit < NumSuits; suit++)
        deck[suit][(int)RankTypes.AceHigh] = deck[suit][(int)RankTypes.AceLow];
    }

    private void ClearKickers(int offset = 0)
    {
      var skippedCardKickers = CardKickers.Skip(offset);

      foreach (RankCard cardKicker in skippedCardKickers)
      {
        cardKicker.RankType = RankTypes.None;
        cardKicker.SuitType = SuitTypes.None;
      }
    }

    public bool IsStraightFlush(List<List<int>> deck)
    {
      for (int suit = 0; suit < NumSuits; suit++)
      {
        for (int offset = NumRanks - NumCards; offset >= 0; offset--)
        {
          int pos = 0;
          bool isStraightFlush = true;

          for (int rank = NumCards - 1; rank >= 0; rank--) // go backwards to maximize Kickers values
          {
            CardKickers[pos].RankType = (RankTypes)(offset + rank);
            CardKickers[pos++].SuitType = (SuitTypes)(suit);

            if (deck[suit][offset + rank] == 0) // if any sequence has zero, the straight part fails
            {
              isStraightFlush = false;
              break;
            }
          }

          if (isStraightFlush) // if gets to this point then 5 cards exist in a sequence and is a straight flush
          {
            if (CardKickers[0].RankType == RankTypes.AceHigh)
            {
              HandType = HandTypes.Royal_Flush;
              ClearKickers();
            }
            else
            {
              HandType = HandTypes.Straight_Flush;
              ClearKickers(1); // keep first kicker
            }

            return true;
          }
        }
      }

      ClearKickers();
      return false;
    }

    public bool IsFlush(List<List<int>> deck)
    {
      for (int suit = 0; suit < NumSuits; suit++)
      {
        int pos = 0;

        // Go backwards to find the highest cards (excluding low Ace)
        for (int rank = NumRanks - 1; rank > 0; rank--)
          if (deck[suit][rank] == 1)
          {
            CardKickers[pos].RankType = (RankTypes)(rank);
            CardKickers[pos++].SuitType = (SuitTypes)(suit);

            if (pos == NumCards) // if it gets here it is a flush
            {
              HandType = HandTypes.Flush;
              return true;
            }
          }
      }

      ClearKickers();

      return false;
    }

    public bool IsStraight(List<List<int>> deck)
    {
      List<int> rankSums = CalculateRankSums(deck);

      for (int offset = NumRanks - NumCards; offset >= 0; offset--)
      {
        bool isStraight = true;

        for (int rank = 0; rank < NumCards; rank++)
          if (rankSums[offset + rank] == 0)
          {
            isStraight = false;
            break;
          }

        if (isStraight)
        {
          HandType = HandTypes.Straight;
          CardKickers[0].RankType = (RankTypes)(offset + 4); // Use offset + 4 to get the highest card in the straight
                                                                  // no other kickers are needed
          return true;
        }
      }

      return false;
    }

    private List<int> CalculateRankSums(List<List<int>> deck)
    {
      return Enumerable.Range(0, NumRanks)
          .Select(rank => Enumerable.Range(0, NumSuits).Sum(suit => deck[suit][rank]))
          .ToList();
    }

    private int ContainsRankCount(List<int> rankSums, int count)
    {
      return rankSums.Count(rankSum => rankSum == count);
    }

    public bool CheckSameKind(List<List<int>> deck)
    {
      List<int> rankSums = CalculateRankSums(deck);

      // blank low ace position
      rankSums[0] = 0;

      // this may be redundant
      ClearKickers();

      if (ContainsRankCount(rankSums, FourOfAKind) > 0)
        SetFourOfAKind(rankSums);
      else if (ContainsRankCount(rankSums, ThreeOfAKind) > 1)
        SetFullHouse(rankSums);
      else if (ContainsRankCount(rankSums, ThreeOfAKind) > 0 && ContainsRankCount(rankSums, TwoOfAKind) > 0)
        SetFullHouse(rankSums);
      else if (ContainsRankCount(rankSums, ThreeOfAKind) > 0)
        SetThreeOfAKind(rankSums);
      else if (ContainsRankCount(rankSums, TwoOfAKind) > 1)
        SetTwoPair(rankSums);
      else if (ContainsRankCount(rankSums, TwoOfAKind) > 0)
        SetOnePair(rankSums);
      else
        SetHighCard(rankSums);

      return true;
    }

    private void SetFourOfAKind(List<int> rankSums)
    {
      int pos = 0;

      HandType = HandTypes.Four_of_a_Kind;
      CardKickers[pos++].RankType = (RankTypes)rankSums.LastIndexOf(FourOfAKind); // since higher cards have higher indexes, use LastIndexes

      List<int> indices = new()
      {
        rankSums.LastIndexOf(HighCard),
        rankSums.LastIndexOf(TwoOfAKind),
        rankSums.LastIndexOf(ThreeOfAKind)
      };

      int highestIndex = indices.Max();

      CardKickers[pos++].RankType = (RankTypes)rankSums.LastIndexOf(highestIndex); // this is last (highest) high card
    }

    private void SetFullHouse(List<int> rankSums)
    {
      int pos = 0;

      HandType = HandTypes.Full_House;
      CardKickers[pos++].RankType = (RankTypes)rankSums.LastIndexOf(ThreeOfAKind); // this is last set

      int indexTwoOfAKind = rankSums.LastIndexOf(TwoOfAKind);
      int indexThreeOfAKind = rankSums.IndexOf(ThreeOfAKind);

      if (indexTwoOfAKind == NotFound)
        CardKickers[pos++].RankType = (RankTypes)indexThreeOfAKind;
      else
        CardKickers[pos++].RankType = (RankTypes)indexTwoOfAKind;
    }

    private void SetThreeOfAKind(List<int> rankSums)
    {
      int pos = 0;

      HandType = HandTypes.Three_of_a_Kind;
      CardKickers[pos++].RankType = (RankTypes)rankSums.LastIndexOf(ThreeOfAKind); // this is only set

      // only 2 high cards are kickers
      int indexHighCard = rankSums.LastIndexOf(HighCard);
      CardKickers[pos++].RankType = (RankTypes)indexHighCard;
      CardKickers[pos++].RankType = (RankTypes)rankSums.LastIndexOf(HighCard, indexHighCard - 1);
    }

    private void SetTwoPair(List<int> rankSums)
    {
      int pos = 0;

      HandType = HandTypes.Two_Pair;

      // since a hand can have 3 pair, get the last 2
      int indexTwoOfAKind = rankSums.LastIndexOf(TwoOfAKind);
      CardKickers[pos++].RankType = (RankTypes)indexTwoOfAKind;

      indexTwoOfAKind = rankSums.LastIndexOf(TwoOfAKind, indexTwoOfAKind - 1);
      CardKickers[pos++].RankType = (RankTypes)indexTwoOfAKind;

      // need to find highest between third pair or high card
      List<int> indices = new()
      {
        rankSums.LastIndexOf(TwoOfAKind, indexTwoOfAKind - 1),
        rankSums.LastIndexOf(HighCard),
      };

      int highestIndex = indices.Max();
      CardKickers[pos++].RankType = (RankTypes)highestIndex;
    }

    private void SetOnePair(List<int> rankSums)
    {
      int pos = 0;

      HandType = HandTypes.One_Pair;

      int indexTwoOfAKind = rankSums.LastIndexOf(TwoOfAKind);
      CardKickers[pos++].RankType = (RankTypes)indexTwoOfAKind;

      // only 3 high card kickers remain
      int indexHighCard = rankSums.LastIndexOf(HighCard);
      CardKickers[pos++].RankType = (RankTypes)indexHighCard;

      // only 2 high card kickers remain
      for (int i = 0; i < 2; i++)
      {
        indexHighCard = rankSums.LastIndexOf(HighCard, indexHighCard - 1);
        CardKickers[pos++].RankType = (RankTypes)indexHighCard;
      }
    }

    private void SetHighCard(List<int> rankSums)
    {
      int pos = 0;

      HandType = HandTypes.High_Card;

      int indexHighCard = rankSums.LastIndexOf(HighCard);
      CardKickers[pos++].RankType = (RankTypes)indexHighCard;

      // 4 high card kickers remain
      for (int i = 0; i < 4; i++)
      {
        indexHighCard = rankSums.LastIndexOf(HighCard, indexHighCard - 1);
        CardKickers[pos++].RankType = (RankTypes)indexHighCard;
      }
    }
  }
}
