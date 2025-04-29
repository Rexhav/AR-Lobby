using System.Collections.Generic;
using UnityEngine;

namespace Rummy.SingleDevice
{
    // Inheritance
    public class Player
    {
        public string Name { get; private set; }
        public List<Card> hand;
        public List<List<Card>> rummyHand;

        public Player(string name)
        {
            Name = name;
            hand = new List<Card>();
            rummyHand = new List<List<Card>>();
        }

        public void generateRummyHand(Dictionary<int, List<int>> validCardsSubmission, int maxNoOfCardsPerPlayer)
        {
            for (int j = 0; j < validCardsSubmission[maxNoOfCardsPerPlayer].Count; j++)
            {
                List<Card> _cards = new List<Card>();
                for (int k = 0; k < validCardsSubmission[maxNoOfCardsPerPlayer][j]; k++)
                {
                    _cards.Add(new Card());
                }
                rummyHand.Add(_cards);
            }
        }

        public void placeInRummyHand(int rowIdx, int colIdx, int inHandIdx)
        {
            hand[inHandIdx].isLocked = true;
            rummyHand[rowIdx][colIdx] = hand[inHandIdx];
            //Debug.Log($"Rummy Hand At: {rowIdx}:{colIdx} :: Hand At: {inHandIdx}");
        }

        public void swapInRummyHand(int rowIdx1, int colIdx1, int rowIdx2, int colIdx2)
        {
            Card _card = rummyHand[rowIdx1][colIdx1];
            rummyHand[rowIdx1][colIdx1] = rummyHand[rowIdx2][colIdx2];
            rummyHand[rowIdx2][colIdx2] = _card;
        }

        public void unlockCard(int inHandIdx)
        {
            for (int i = 0; i < rummyHand.Count; i++)
            {
                for (int j = 0; j < rummyHand[i].Count; j++)
                {
                    if (hand[inHandIdx].Id == rummyHand[i][j].Id)
                    {
                        rummyHand[i][j] = new Card();
                        hand[inHandIdx].isLocked = false;
                        return;
                    }
                }
            }
        }
    }
}
