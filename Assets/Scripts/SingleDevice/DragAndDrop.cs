using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Rummy.SingleDevice
{
    public class DragAndDrop : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        private Image sourceImage;
        private Transform originalParent;
        private Canvas rootCanvas;
        private Vector2 originalPosition;
        private GameObject tempDragObject;

        private void Start()
        {
            sourceImage = GetComponent<Image>();
            rootCanvas = GetComponentInParent<Canvas>(); // Find the root canvas
            originalParent = transform.parent;
            originalPosition = transform.position;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            originalPosition = transform.position;

            // Create a temporary object for dragging
            tempDragObject = new GameObject("DraggedImage");
            tempDragObject.transform.SetParent(rootCanvas.transform, false); // Move to root Canvas

            // Add Image component and copy properties
            Image tempImage = tempDragObject.AddComponent<Image>();
            tempImage.sprite = sourceImage.sprite;
            tempImage.rectTransform.sizeDelta = sourceImage.rectTransform.sizeDelta;
            tempImage.raycastTarget = false; // Prevent interaction issues

            // Set position to match original
            tempDragObject.transform.position = transform.position;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (tempDragObject)
                tempDragObject.transform.position = eventData.position; // Move freely
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (tempDragObject)
            {
                GameObject target = eventData.pointerEnter;

                if (target != null && target.CompareTag("CardHolder"))
                {
                    if (sourceImage.CompareTag("CardHolder"))
                    {
                        Sprite _sprite = target.GetComponent<Image>().sprite;
                        target.GetComponent<Image>().sprite = sourceImage.sprite; // Assign new sprite
                        sourceImage.sprite = _sprite; // Clear original
                        int _rowIdx1 = sourceImage.transform.parent.GetSiblingIndex();
                        int _colIdx1 = sourceImage.transform.GetSiblingIndex();
                        int _rowIdx2 = target.transform.parent.GetSiblingIndex();
                        int _colIdx2 = target.transform.GetSiblingIndex();
                        GameplayManager.Instance.players[GameplayManager.Instance.currentPlayerIndex].swapInRummyHand(_rowIdx1, _colIdx1, _rowIdx2, _colIdx2);
                    }
                    else
                    {
                        int _inHandIdx = sourceImage.transform.GetSiblingIndex();
                        if (!GameplayManager.Instance.players[GameplayManager.Instance.currentPlayerIndex].hand[_inHandIdx].isLocked)
                        {
                            target.GetComponent<Image>().sprite = sourceImage.sprite; // Assign new sprite
                            int _rowIdx = target.transform.parent.GetSiblingIndex();
                            int _colIdx = target.transform.GetSiblingIndex();
                            GameplayManager.Instance.players[GameplayManager.Instance.currentPlayerIndex].placeInRummyHand(_rowIdx, _colIdx, _inHandIdx);
                        }
                        else
                        {
                            Debug.Log("Card exists is rummy hand");
                        }
                    }
                    GameplayManager.Instance.uIManager.updateUI();
                }

                Destroy(tempDragObject); // Remove temporary object
            }

            //transform.position = originalPosition; // Reset original position
        }
    }
}
