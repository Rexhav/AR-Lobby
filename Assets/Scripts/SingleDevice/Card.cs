using UnityEngine;

namespace Rummy.SingleDevice
{
    // Encapsulation & Abstraction
    [System.Serializable]
    public class Card
    {
        public int Id { get; set; }
        public string Suit { get; set; }
        public string Rank { get; set; }
        public int Value { get; set; }
        public int SpriteIndex { get; set; }
        public bool isLocked { get; set; }

        public Card()
        {
            Id = -1;
            Suit = "";
            Rank = "";
            Value = 0;
            SpriteIndex = 0;
            isLocked = false;
        }

        public Card(int id, string suit, string rank, int value, int spriteIndex)
        {
            Id = id;
            Suit = suit;
            Rank = rank;
            Value = value;
            SpriteIndex = spriteIndex;
            isLocked = false;
        }

        public override string ToString()
        {
            return $"{Rank} of {Suit}";
        }
    }
}
