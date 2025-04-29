using UnityEngine;

public class CubeHandler : MonoBehaviour
{
    public float rotationSpeed = 0.2f;
    public float pinchSpeed = 0.001f;
    public float minScale = 0.2f;
    public float maxScale = 2f;

    private float initialDistance;
    private Vector3 initialScale;

    void Update()
    {
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Moved)
            {
                float rotX = touch.deltaPosition.x * rotationSpeed;
                transform.Rotate(Vector3.up, -rotX, Space.World);
            }
        }

        if (Input.touchCount == 2)
        {
            Touch t0 = Input.GetTouch(0);
            Touch t1 = Input.GetTouch(1);

            if (t1.phase == TouchPhase.Began)
            {
                initialDistance = Vector2.Distance(t0.position, t1.position);
                initialScale = transform.localScale;
            }
            else if (t0.phase == TouchPhase.Moved || t1.phase == TouchPhase.Moved)
            {
                float currentDistance = Vector2.Distance(t0.position, t1.position);
                if (Mathf.Approximately(initialDistance, 0)) return;

                float scaleFactor = currentDistance / initialDistance;
                Vector3 newScale = initialScale * scaleFactor;
                newScale = Vector3.ClampMagnitude(newScale, maxScale);
                newScale = Vector3.Max(newScale, Vector3.one * minScale);

                transform.localScale = newScale;
            }
        }
    }
}
