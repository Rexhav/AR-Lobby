using UnityEngine;

public class CubeGestureController : MonoBehaviour
{
    private float initialDistance;
    private Vector3 initialScale;

    private float rotationSpeed = 0.2f;

    void Update()
    {
        // Scale with pinch
        if (Input.touchCount == 2)
        {
            Touch t0 = Input.GetTouch(0);
            Touch t1 = Input.GetTouch(1);

            if (t0.phase == TouchPhase.Began || t1.phase == TouchPhase.Began)
            {
                initialDistance = Vector2.Distance(t0.position, t1.position);
                initialScale = transform.localScale;
            }
            else if (t0.phase == TouchPhase.Moved || t1.phase == TouchPhase.Moved)
            {
                float currentDistance = Vector2.Distance(t0.position, t1.position);
                if (Mathf.Approximately(initialDistance, 0)) return;

                float factor = currentDistance / initialDistance;
                transform.localScale = initialScale * factor;
            }
        }

        // Rotate with single finger horizontal swipe
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Moved)
            {
                float rotation = touch.deltaPosition.x * rotationSpeed;
                transform.Rotate(0, -rotation, 0, Space.World);
            }
        }
    }
}
