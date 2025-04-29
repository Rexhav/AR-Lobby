using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Rummy.SingleDevice
{
    public class AIPlayer : Player
    {
        public AIPlayer(string name) : base(name) { }

        //public override void PlayTurn()
        //{
        //    Debug.Log($"AI {Name} is analyzing its hand...");

        //    // Step 1: Evaluate Hand (Check for Sets & Sequences)
        //    List<List<Card>> sets = FindSets();
        //    List<List<Card>> sequences = FindSequences();

        //    // Step 2: Decide Pickup (Discard Pile vs. Deck)
        //    Card topDiscard = GameplayManager.Instance.GetTopDiscard();
        //    bool shouldPickupDiscard = ShouldPickupDiscard(topDiscard, sets, sequences);

        //    if (shouldPickupDiscard)
        //    {
        //        //DrawFromDiscardPile();
        //    }
        //    else
        //    {
        //        //DrawFromDeck();
        //    }

        //    // Step 3: Choose the Best Card to Discard
        //    Card cardToDiscard = ChooseBestDiscard();
        //    GameplayManager.Instance.DiscardCard(cardToDiscard);

        //    // Step 4: Check if AI Should Declare Win
        //    if (CanDeclareWin())
        //    {
        //        GameplayManager.Instance.DeclareWinner(this);
        //    }
        //    else
        //    {
        //        GameplayManager.Instance.NextTurn();
        //    }
        //}

        //// Check if a card helps in forming a set or sequence
        //private bool ShouldPickupDiscard(Card discard, List<List<Card>> sets, List<List<Card>> sequences)
        //{
        //    foreach (var set in sets)
        //    {
        //        if (set.Count == 2 && IsSameRank(set[0], discard)) return true;
        //    }

        //    foreach (var sequence in sequences)
        //    {
        //        if (sequence.Count == 2 && IsSequential(sequence, discard)) return true;
        //    }

        //    return false;
        //}

        //// Finds sets (same rank, different suits)
        //private List<List<Card>> FindSets()
        //{
        //    return hand.GroupBy(card => card.Rank)
        //               .Where(group => group.Count() >= 2)
        //               .Select(group => group.ToList())
        //               .ToList();
        //}

        //// Finds sequences (consecutive numbers in the same suit)
        //private List<List<Card>> FindSequences()
        //{
        //    List<List<Card>> sequences = new List<List<Card>>();
        //    var groupedBySuit = hand.GroupBy(card => card.Suit);

        //    foreach (var suitGroup in groupedBySuit)
        //    {
        //        var sortedCards = suitGroup.OrderBy(card => card.Value).ToList();
        //        List<Card> currentSequence = new List<Card>();

        //        for (int i = 0; i < sortedCards.Count - 1; i++)
        //        {
        //            if (sortedCards[i + 1].Value == sortedCards[i].Value + 1)
        //            {
        //                if (currentSequence.Count == 0) currentSequence.Add(sortedCards[i]);
        //                currentSequence.Add(sortedCards[i + 1]);
        //            }
        //            else if (currentSequence.Count >= 2)
        //            {
        //                sequences.Add(new List<Card>(currentSequence));
        //                currentSequence.Clear();
        //            }
        //        }
        //        if (currentSequence.Count >= 2) sequences.Add(currentSequence);
        //    }

        //    return sequences;
        //}

        //// Decide the least useful card to discard
        //private Card ChooseBestDiscard()
        //{
        //    Dictionary<Card, int> cardScores = new Dictionary<Card, int>();

        //    foreach (Card card in hand)
        //    {
        //        int score = 0;

        //        if (FindSets().Any(set => set.Contains(card))) score += 5;
        //        if (FindSequences().Any(seq => seq.Contains(card))) score += 5;
        //        if (card.Rank == "J" || card.Rank == "Q" || card.Rank == "K") score -= 2;  // Face cards are risky

        //        cardScores[card] = score;
        //    }

        //    return cardScores.OrderBy(kvp => kvp.Value).First().Key;
        //}

        //// Check if AI can declare win
        //private bool CanDeclareWin()
        //{
        //    int deadwood = CalculateDeadwood();
        //    return deadwood == 0 || deadwood <= 10; // AI will declare win if deadwood <= 10
        //}

        //// Calculate deadwood (unmatched cards' sum)
        //private int CalculateDeadwood()
        //{
        //    int totalDeadwood = 0;
        //    List<Card> usedCards = new List<Card>();

        //    foreach (var set in FindSets()) usedCards.AddRange(set);
        //    foreach (var seq in FindSequences()) usedCards.AddRange(seq);

        //    foreach (var card in hand)
        //    {
        //        if (!usedCards.Contains(card)) totalDeadwood += card.Value;
        //    }

        //    return totalDeadwood;
        //}

        //private bool IsSameRank(Card a, Card b) => a.Rank == b.Rank;

        //private bool IsSequential(List<Card> sequence, Card card)
        //{
        //    return sequence.Any(c => c.Suit == card.Suit &&
        //                             (c.Value == card.Value - 1 || c.Value == card.Value + 1));
        //}
    }
}
