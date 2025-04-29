using UnityEngine;
using System.Collections.Generic;


namespace Rummy.SingleDevice {
    // Singleton Pattern
    public class GameplayManager : MonoBehaviour
    {
        public static GameplayManager Instance { get; private set; }

        public UIManager uIManager;
        public int noOfCardDecksToUse = 1;
        public int currentPlayerIndex = -1;
        public int noOfPlayers;
        public int maxNoOfCardsPerPlayer;
        public Deck deck;
        public List<Player> players;
        public List<Card> discardPile = new List<Card>();
        public Dictionary<int, List<int>> validCardsSubmission = new Dictionary<int, List<int>>
                                                      {
                                                        {6,new List<int>{3,3}},
                                                        {7,new List<int>{3,4}},
                                                        {10,new List<int>{3,3,4}},
                                                        {13,new List<int>{3,3,3,4}},
                                                      };

        void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);

            uIManager = GetComponent<UIManager>();
        }

        void Start()
        {
            startGame();
        }

        void startGame()
        {
            deck = new Deck(noOfCardDecksToUse);
            deck.Shuffle();

            players = new List<Player>();
            for (int i = 0; i < noOfPlayers; i++)
            {
                Player _player = new Player($"Player {i + 1}");
                players.Add(_player);
                _player.generateRummyHand(validCardsSubmission, maxNoOfCardsPerPlayer);
            }

            DealCards();

            uIManager.populateGameView();

            Card card = deck.DrawCard();
            discardPile.Add(card);

            NextTurn();
        }

        void DealCards()
        {
            for (int i = 0; i < maxNoOfCardsPerPlayer; i++)
            {
                foreach (var player in players) DrawCardFromDeck(player);
            }
        }

        // Player Action Functions
        // Draw Deck Check
        public void AllowDrawCardFromDeck()
        {
            if (players[currentPlayerIndex].hand.Count == maxNoOfCardsPerPlayer + 1)
            {
                Debug.Log($"Cannot draw another card.");
                return;
            }
            DrawCardFromDeck(players[currentPlayerIndex]);
        }
        void DrawCardFromDeck(Player player)
        {
            Card card = deck.DrawCard();
            if (card != null)
            {
                player.hand.Add(card);
            }
            else
            {
                Debug.Log($"No card available in Deck");
            }
            uIManager.updateUI();
        }
        // Draw Discard Pile Check
        public void AllowDrawFromDiscardPile()
        {
            if (players[currentPlayerIndex].hand.Count == maxNoOfCardsPerPlayer + 1)
            {
                Debug.Log($"Cannot draw another card.");
                return;
            }
            DrawCardFromDiscardPile(players[currentPlayerIndex]);
        }
        void DrawCardFromDiscardPile(Player player)
        {
            Card card = DrawFromDiscardPile();
            if (card != null)
            {
                player.hand.Add(card);
            }
            else
            {
                Debug.Log($"No card available in DiscardPile");
            }
            uIManager.updateUI();
        }
        // Throw Discard Pile Check
        public void AllowCardToDiscardPile(int throwCardIndex)
        {
            Card _card = players[currentPlayerIndex].hand[throwCardIndex];
            if (!players[currentPlayerIndex].hand.Contains(_card))
            {
                Debug.Log("Card doesn't exist. Try to throw another.");
                return;
            }
            if (_card.isLocked)
            {
                Debug.Log("Card is locked in rummy hand. Unlocking it.");
                players[currentPlayerIndex].unlockCard(throwCardIndex);
                uIManager.updateUI();
                return;
            }
            if (players[currentPlayerIndex].hand.Count <= maxNoOfCardsPerPlayer)
            {
                Debug.Log("Please draw a card before throw.");
                return;
            }
            Debug.Log($"Throwing card at index: {throwCardIndex}");

            players[currentPlayerIndex].unlockCard(throwCardIndex);
            players[currentPlayerIndex].hand.Remove(_card);
            discardPile.Add(_card);

            NextTurn();
            uIManager.viewHand();
        }

        // Utility Functions
        // Func 1 
        Card DrawFromDiscardPile()
        {
            if (discardPile.Count == 0) return null;
            Card card = discardPile[discardPile.Count - 1];
            discardPile.RemoveAt(discardPile.Count - 1);
            return card;
        }
        // Func 2
        public void NextTurn()
        {
            currentPlayerIndex = (currentPlayerIndex + 1) % players.Count;
            uIManager.updateUI();
        }
        // Func 3
        public void validateWin()
        {

        }

        public void DeclareWinner(Player winner)
        {
            Debug.Log($"{winner.Name} has won the game!");
        }
    }
}
