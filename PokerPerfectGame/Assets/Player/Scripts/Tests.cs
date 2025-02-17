using System.Collections.Generic;
using System;
using System.ComponentModel;
using FunctionsNameSpace;
using System.Linq;
using RankHands;
using UnityEngine;

namespace Tests
{
  public class Tester
  {
    // Create an array of matrices
    public List<List<int>>[] arrayOfMatrices = new List<List<int>>[Enum.GetValues(typeof(TestTypes)).Length];

    public enum TestTypes
    {
      [Description("High Card - A, Q, 10, 8, 6")]
      High_Card_Ace,
      [Description("High Card - 9, 8, 7, 5, 4")]
      High_Card_Nine,
      [Description("One Pair - 2, J, 9, 7")]
      One_Pair,
      [Description("Two Pair - A, 8, 6")]
      Two_Pair,
      [Description("Three Pair - A, 8, 5")]
      Three_Pair,
      [Description("Three of a Kind - A, 8, 5")]
      Three_of_a_Kind,
      [Description("Two Three of a Kind - 9, 3")]
      Two_Three_of_a_Kind,
      [Description("Straight Ace High - A")]
      Straight_Ace_High,
      [Description("Straight Five High - 5")]
      Straight_Five_High,
      [Description("Flush Ace High - A, 10, 8, 6, 2")]
      Flush_Ace_High,
      [Description("Flush Eight High - 8, 7, 6, 4, 2")]
      Flush_Eight_High,
      [Description("Full House - Q, 8")]
      Full_House,
      [Description("Four of a Kind - 4, 9")]
      Four_of_a_Kind,
      [Description("Straight Flush - 9")]
      Straight_Flush,
      [Description("Royal Flush")]
      Royal_Flush,
    }

    public void MakeArrays()
    {
      // Initialize each element in the array
      foreach (TestTypes type in Enum.GetValues(typeof(TestTypes)))
        arrayOfMatrices[(int)type] = new List<List<int>>();

      // High Card A, Q, 10, 8, 6
      //                                                                A  2  3  4  5  6  7  8  9 10  J  Q  K  A
      arrayOfMatrices[(int)TestTypes.High_Card_Ace].Add(new List<int> { 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 1 });
      arrayOfMatrices[(int)TestTypes.High_Card_Ace].Add(new List<int> { 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 1, 0, 0 });
      arrayOfMatrices[(int)TestTypes.High_Card_Ace].Add(new List<int> { 0, 0, 1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0 });
      arrayOfMatrices[(int)TestTypes.High_Card_Ace].Add(new List<int> { 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0 });

      // High Card 9, 8, 7, 5, 4                                                
      //                                                                 A  2  3  4  5  6  7  8  9 10  J  Q  K  A
      arrayOfMatrices[(int)TestTypes.High_Card_Nine].Add(new List<int> { 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 });
      arrayOfMatrices[(int)TestTypes.High_Card_Nine].Add(new List<int> { 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0 });
      arrayOfMatrices[(int)TestTypes.High_Card_Nine].Add(new List<int> { 0, 0, 1, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0 });
      arrayOfMatrices[(int)TestTypes.High_Card_Nine].Add(new List<int> { 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0 });

      // One Pair 2, J, 9, 7
      //                                                           A  2  3  4  5  6  7  8  9 10  J  Q  K  A
      arrayOfMatrices[(int)TestTypes.One_Pair].Add(new List<int> { 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0 });
      arrayOfMatrices[(int)TestTypes.One_Pair].Add(new List<int> { 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 });
      arrayOfMatrices[(int)TestTypes.One_Pair].Add(new List<int> { 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0 });
      arrayOfMatrices[(int)TestTypes.One_Pair].Add(new List<int> { 0, 1, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0 });

      // Two Pair A, 8, 6
      //                                                           A  2  3  4  5  6  7  8  9 10  J  Q  K  A
      arrayOfMatrices[(int)TestTypes.Two_Pair].Add(new List<int> { 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 });
      arrayOfMatrices[(int)TestTypes.Two_Pair].Add(new List<int> { 0, 0, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0, 0, 0 });
      arrayOfMatrices[(int)TestTypes.Two_Pair].Add(new List<int> { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 });
      arrayOfMatrices[(int)TestTypes.Two_Pair].Add(new List<int> { 0, 0, 0, 0, 0, 1, 0, 1, 0, 0, 0, 0, 0, 1 });

      // Three Pair A, 8, 5
      //                                                             A  2  3  4  5  6  7  8  9 10  J  Q  K  A
      arrayOfMatrices[(int)TestTypes.Three_Pair].Add(new List<int> { 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0 });
      arrayOfMatrices[(int)TestTypes.Three_Pair].Add(new List<int> { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 });
      arrayOfMatrices[(int)TestTypes.Three_Pair].Add(new List<int> { 0, 0, 0, 1, 1, 0, 0, 1, 0, 0, 0, 0, 0, 1 });
      arrayOfMatrices[(int)TestTypes.Three_Pair].Add(new List<int> { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 });

      // Three of a Kind A, 8, 6
      //                                                                  A  2  3  4  5  6  7  8  9 10  J  Q  K  A
      arrayOfMatrices[(int)TestTypes.Three_of_a_Kind].Add(new List<int> { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 });
      arrayOfMatrices[(int)TestTypes.Three_of_a_Kind].Add(new List<int> { 0, 0, 1, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 1 });
      arrayOfMatrices[(int)TestTypes.Three_of_a_Kind].Add(new List<int> { 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1 });
      arrayOfMatrices[(int)TestTypes.Three_of_a_Kind].Add(new List<int> { 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 });

      // Two Three of a Kind 9, 3
      //                                                                      A  2  3  4  5  6  7  8  9 10  J  Q  K  A
      arrayOfMatrices[(int)TestTypes.Two_Three_of_a_Kind].Add(new List<int> { 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 });
      arrayOfMatrices[(int)TestTypes.Two_Three_of_a_Kind].Add(new List<int> { 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1 });
      arrayOfMatrices[(int)TestTypes.Two_Three_of_a_Kind].Add(new List<int> { 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0 });
      arrayOfMatrices[(int)TestTypes.Two_Three_of_a_Kind].Add(new List<int> { 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0 });

      // Straight Ace High A
      //                                                                    A  2  3  4  5  6  7  8  9 10  J  Q  K  A
      arrayOfMatrices[(int)TestTypes.Straight_Ace_High].Add(new List<int> { 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 1 });
      arrayOfMatrices[(int)TestTypes.Straight_Ace_High].Add(new List<int> { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0 });
      arrayOfMatrices[(int)TestTypes.Straight_Ace_High].Add(new List<int> { 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0 });
      arrayOfMatrices[(int)TestTypes.Straight_Ace_High].Add(new List<int> { 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0 });

      // Straight Ace High 5
      //                                                                     A  2  3  4  5  6  7  8  9 10  J  Q  K  A
      arrayOfMatrices[(int)TestTypes.Straight_Five_High].Add(new List<int> { 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0 });
      arrayOfMatrices[(int)TestTypes.Straight_Five_High].Add(new List<int> { 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1 });
      arrayOfMatrices[(int)TestTypes.Straight_Five_High].Add(new List<int> { 0, 1, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0 });
      arrayOfMatrices[(int)TestTypes.Straight_Five_High].Add(new List<int> { 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 });

      // Flush_Ace_High A, 10, 8, 6, 2
      //                                                                 A  2  3  4  5  6  7  8  9 10  J  Q  K  A
      arrayOfMatrices[(int)TestTypes.Flush_Ace_High].Add(new List<int> { 0, 1, 0, 0, 0, 1, 0, 1, 0, 1, 0, 0, 0, 1 });
      arrayOfMatrices[(int)TestTypes.Flush_Ace_High].Add(new List<int> { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 });
      arrayOfMatrices[(int)TestTypes.Flush_Ace_High].Add(new List<int> { 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 });
      arrayOfMatrices[(int)TestTypes.Flush_Ace_High].Add(new List<int> { 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 });

      // Flush Eight High 8, 7, 6, 4, 2
      //                                                                   A  2  3  4  5  6  7  8  9 10  J  Q  K  A
      arrayOfMatrices[(int)TestTypes.Flush_Eight_High].Add(new List<int> { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 });
      arrayOfMatrices[(int)TestTypes.Flush_Eight_High].Add(new List<int> { 0, 1, 0, 1, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0 });
      arrayOfMatrices[(int)TestTypes.Flush_Eight_High].Add(new List<int> { 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0 });
      arrayOfMatrices[(int)TestTypes.Flush_Eight_High].Add(new List<int> { 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 });

      // Full House Q, 8
      //                                                             A  2  3  4  5  6  7  8  9 10  J  Q  K  A
      arrayOfMatrices[(int)TestTypes.Full_House].Add(new List<int> { 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 1, 0, 0 });
      arrayOfMatrices[(int)TestTypes.Full_House].Add(new List<int> { 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0 });
      arrayOfMatrices[(int)TestTypes.Full_House].Add(new List<int> { 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 1, 0, 0 });
      arrayOfMatrices[(int)TestTypes.Full_House].Add(new List<int> { 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 1, 0, 0 });

      // Four of a Kind - 4, 9
      //                                                                 A  2  3  4  5  6  7  8  9 10  J  Q  K  A
      arrayOfMatrices[(int)TestTypes.Four_of_a_Kind].Add(new List<int> { 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 });
      arrayOfMatrices[(int)TestTypes.Four_of_a_Kind].Add(new List<int> { 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0 });
      arrayOfMatrices[(int)TestTypes.Four_of_a_Kind].Add(new List<int> { 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0 });
      arrayOfMatrices[(int)TestTypes.Four_of_a_Kind].Add(new List<int> { 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0 });

      // Straight Flush - 9
      //                                                                 A  2  3  4  5  6  7  8  9 10  J  Q  K  A
      arrayOfMatrices[(int)TestTypes.Straight_Flush].Add(new List<int> { 0, 0, 0, 0, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0 });
      arrayOfMatrices[(int)TestTypes.Straight_Flush].Add(new List<int> { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 });
      arrayOfMatrices[(int)TestTypes.Straight_Flush].Add(new List<int> { 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 });
      arrayOfMatrices[(int)TestTypes.Straight_Flush].Add(new List<int> { 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0 });

      // Royal Flush
      //                                                              A  2  3  4  5  6  7  8  9 10  J  Q  K  A
      arrayOfMatrices[(int)TestTypes.Royal_Flush].Add(new List<int> { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 });
      arrayOfMatrices[(int)TestTypes.Royal_Flush].Add(new List<int> { 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1 });
      arrayOfMatrices[(int)TestTypes.Royal_Flush].Add(new List<int> { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 });
      arrayOfMatrices[(int)TestTypes.Royal_Flush].Add(new List<int> { 0, 0, 0, 0, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0 });
    }

    public static void TestHands()
    {
      Tester tester = new();
      tester.MakeArrays();

      PlayerObject playerObject = new();

      List<List<int>> deck = new List<List<int>>();

      for (int suit = 0; suit < Rank.NumSuits; suit++)
        deck.Add(Enumerable.Repeat(0, Rank.NumRanks).ToList());

      foreach (Tester.TestTypes type in Enum.GetValues(typeof(Tester.TestTypes)))
      {
        for (int suit = 0; suit < Rank.NumSuits; suit++)
          for (int rank = 0; rank < Rank.NumRanks; rank++)
            deck[suit][rank] = tester.arrayOfMatrices[(int)type][suit][rank];

        playerObject.Rank.CopyAces(deck);

        if (!playerObject.Rank.IsStraightFlush(deck))
          if (!playerObject.Rank.IsFlush(deck))
            if (!playerObject.Rank.IsStraight(deck))
              playerObject.Rank.CheckSameKind(deck);

        Debug.Log(type.GetDescription());
        Debug.Log(playerObject.Rank.Description);
      }
    }

  }
}