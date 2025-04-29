using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Rummy.SingleDevice
{
    public class UIManager : MonoBehaviour
    {
        public List<Sprite> cards = new List<Sprite>();

        public TMPro.TMP_Text currentPlayerText;
        public Image deckImgHolder;
        public Image discardImgHolder;

        public GameObject cardInHandView;
        public GameObject cardInHandList;
        public GameObject cardInHandListPrefab;
        public GameObject rummyList;
        public GameObject rummyListPrefabCards_3;
        public GameObject rummyListPrefabCards_4;
        public List<List<GameObject>> rummyHandObjList;

        public GameObject playersView;
        public GameObject playersViewList;
        public GameObject playersViewListPrefab;

        public void populateGameView()
        {
            // Players List
            for (int i = 0; i < GameplayManager.Instance.noOfPlayers; i++)
            {
                GameObject playersViewPrefabInst = Instantiate(playersViewListPrefab);
                playersViewPrefabInst.transform.GetChild(0).GetComponent<TMPro.TMP_Text>().text = GameplayManager.Instance.players[i].Name;
                playersViewPrefabInst.transform.SetParent(playersViewList.transform);
            }

            // Hand List
            for (int i = 0; i < GameplayManager.Instance.maxNoOfCardsPerPlayer + 1; i++)
            {
                int iTemp = i;
                GameObject handViewPrefabInst = Instantiate(cardInHandListPrefab);
                handViewPrefabInst.GetComponent<Image>().sprite = null;
                handViewPrefabInst.GetComponent<Button>().onClick.AddListener(() => GameplayManager.Instance.AllowCardToDiscardPile(iTemp));
                handViewPrefabInst.transform.SetParent(cardInHandList.transform);
            }

            // Rummy List
            List<int> _rummySets = GameplayManager.Instance.validCardsSubmission[GameplayManager.Instance.maxNoOfCardsPerPlayer];
            rummyHandObjList = new List<List<GameObject>>();
            for (int i = 0; i < _rummySets.Count; i++)
            {
                GameObject _rummyListPrefabInst = null;
                if (_rummySets[i] == 3)
                {
                    _rummyListPrefabInst = Instantiate(rummyListPrefabCards_3);
                    _rummyListPrefabInst.transform.SetParent(rummyList.transform);
                }
                if (_rummySets[i] == 4)
                {
                    _rummyListPrefabInst = Instantiate(rummyListPrefabCards_4);
                    _rummyListPrefabInst.transform.SetParent(rummyList.transform);
                }
                List<GameObject> _childern = new List<GameObject>();
                for (int j = 0; j < _rummyListPrefabInst.transform.childCount; j++)
                {
                    _childern.Add(_rummyListPrefabInst.transform.GetChild(j).gameObject);
                }
                rummyHandObjList.Add(_childern);
            }
        }

        public void viewPlayers()
        {
            if (!playersView.activeInHierarchy)
            {
                playersView.SetActive(true);
            }
            else
            {
                playersView.SetActive(false);
            }
        }

        public void viewHand()
        {
            if (!cardInHandView.activeInHierarchy)
            {
                cardInHandView.SetActive(true);
            }
            else
            {
                cardInHandView.SetActive(false);
            }
        }

        public void updateUI()
        {
            if (GameplayManager.Instance.currentPlayerIndex == -1) return;

            // Local Variables
            List<Player> _players = GameplayManager.Instance.players;
            int _currentPlayerIndex = GameplayManager.Instance.currentPlayerIndex;
            Player _currentplayer = _players[_currentPlayerIndex];
            List<Card> _discardPile = GameplayManager.Instance.discardPile;

            // Player List
            if (_currentPlayerIndex == 0)
            {
                playersViewList.transform.GetChild(_players.Count - 1).GetComponent<Image>().color = Color.grey;
            }
            else
            {
                playersViewList.transform.GetChild(_currentPlayerIndex - 1).GetComponent<Image>().color = Color.grey;
            }
            playersViewList.transform.GetChild(_currentPlayerIndex).GetComponent<Image>().color = Color.black;
            currentPlayerText.text = _players[_currentPlayerIndex].Name;

            // Hand View
            for (int i = 0; i < _currentplayer.hand.Count; i++)
            {
                cardInHandList.transform.GetChild(i).GetComponent<Image>().sprite = cards[_currentplayer.hand[i].SpriteIndex];
                if (_currentplayer.hand[i].isLocked)
                {
                    cardInHandList.transform.GetChild(i).GetComponent<Image>().color = new Color(125 / 255f, 125 / 255f, 125 / 255f);
                }
                else
                {
                    cardInHandList.transform.GetChild(i).GetComponent<Image>().color = new Color(1, 1, 1);
                }
                cardInHandList.transform.GetChild(i).GetComponent<Image>().gameObject.SetActive(true);
            }
            if (_currentplayer.hand.Count == GameplayManager.Instance.maxNoOfCardsPerPlayer)
            {
                //handViewList.transform.GetChild(_currentplayer.hand.Count).GetComponent<Image>().sprite = deckImgHolder.GetComponent<Image>().sprite;
                cardInHandList.transform.GetChild(_currentplayer.hand.Count).GetComponent<Image>().gameObject.SetActive(false);
            }

            // Rummy Hand View
            for (int i = 0; i < _currentplayer.rummyHand.Count; i++)
            {
                for (int j = 0; j < _currentplayer.rummyHand[i].Count; j++)
                {
                    if (_currentplayer.rummyHand[i][j].Id == -1)
                    {
                        rummyHandObjList[i][j].GetComponent<Image>().sprite = deckImgHolder.GetComponent<Image>().sprite;
                        //Debug.Log($"Rummy Hand At: {i}:{j} is not set :: {rummyHandObjList[i][j].name}");
                    }
                    else
                    {
                        rummyHandObjList[i][j].GetComponent<Image>().sprite = cards[_currentplayer.rummyHand[i][j].SpriteIndex];
                        //Debug.Log($"Rummy Hand At: {i}:{j} is set to {cards[_currentplayer.rummyHand[i][j].SpriteIndex].name}");
                    }
                }
            }

            // Discard Hand
            if (GameplayManager.Instance.discardPile.Count == 0)
            {
                discardImgHolder.gameObject.SetActive(false);
            }
            else
            {
                discardImgHolder.gameObject.SetActive(true);
                discardImgHolder.sprite = cards[_discardPile[_discardPile.Count - 1].SpriteIndex];
            }

            // Deck Hand
            if (GameplayManager.Instance.deck.cards.Count > 0)
            {
                deckImgHolder.gameObject.SetActive(true);
            }
            else
            {
                deckImgHolder.gameObject.SetActive(false);
            }
        }
    }
}
