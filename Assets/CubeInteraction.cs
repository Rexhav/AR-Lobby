using UnityEngine;
using Unity.Netcode;

public class CubeInteraction : NetworkBehaviour
{
    private Vector2 startTouchPos;
    private float initialDistance;
    private Vector3 initialScale;
    private float rotationSpeed = 0.2f;

    void Update()
    {
        if (!IsOwner) return;

        if (Input.touchCount == 1)
        {
            // Drag
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Moved)
            {
                Ray ray = Camera.main.ScreenPointToRay(touch.position);
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    transform.position = hit.point;
                }
            }
        }
        else if (Input.touchCount == 2)
        {
            // Pinch to Scale
            Touch touch0 = Input.GetTouch(0);
            Touch touch1 = Input.GetTouch(1);

            if (touch1.phase == TouchPhase.Began)
            {
                initialDistance = Vector2.Distance(touch0.position, touch1.position);
                initialScale = transform.localScale;
            }
            else
            {
                float currentDistance = Vector2.Distance(touch0.position, touch1.position);
                if (Mathf.Approximately(initialDistance, 0)) return;

                float scaleFactor = currentDistance / initialDistance;
                transform.localScale = initialScale * scaleFactor;
            }

            // Rotate (based on difference in movement)
            Vector2 prevDir = touch1.position - touch1.deltaPosition - (touch0.position - touch0.deltaPosition);
            Vector2 currDir = touch1.position - touch0.position;
            float angle = Vector2.SignedAngle(prevDir, currDir);
            transform.Rotate(0, -angle * rotationSpeed, 0);
        }
    }
}