using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Rummy.SingleDevice
{
    // Encapsulation & Polymorphism
    [System.Serializable]
    public class Deck
    {
        public List<Card> cards;

        public Deck()
        {
            populateDeck(1);
        }

        public Deck(int noOfCardDecksToUse)
        {
            populateDeck(noOfCardDecksToUse);
        }

        void populateDeck(int noOfCardDecksToUse)
        {
            cards = new List<Card>();
            string[] suits = { "Hearts", "Diamonds", "Clubs", "Spades" };
            string[] ranks = { "A", "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K" };

            for (int j = 0; j < noOfCardDecksToUse; j++)
            {
                int suitsCount = -1;
                foreach (string suit in suits)
                {
                    suitsCount += 1;
                    for (int i = 0; i < ranks.Length; i++)
                    {
                        cards.Add(new Card(i + suitsCount * 13 + j * 52, suit, ranks[i], i + 1, i + suitsCount * 13));
                    }
                }
            }
        }

        public void Shuffle()
        {
            cards = cards.OrderBy(x => Random.value).ToList();
        }

        public Card DrawCard()
        {
            if (cards.Count == 0) return null;
            Card drawnCard = cards[0];
            cards.RemoveAt(0);
            return drawnCard;
        }
    }
}
