using UnityEngine;
using Unity.Netcode;

public class CubeController : NetworkBehaviour
{
    private float initialDistance;
    private Vector3 initialScale;

    void Update()
    {
        if (!IsOwner) return;

        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);
            transform.Rotate(0f, -touch.deltaPosition.x * 0.2f, 0f);
        }
        else if (Input.touchCount == 2)
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
                float currDistance = Vector2.Distance(t0.position, t1.position);
                float factor = currDistance / initialDistance;
                transform.localScale = initialScale * factor;
            }
        }
    }
}
