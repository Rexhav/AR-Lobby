using UnityEngine;

public class CenterMarkerController : MonoBehaviour
{
    public float distanceFromCamera = 1.5f;

    void Update()
    {
        if (Camera.main == null) return;

        // Always keep the marker in front of the camera
        Transform cam = Camera.main.transform;
        transform.position = cam.position + cam.forward * distanceFromCamera;
        transform.rotation = Quaternion.LookRotation(cam.forward); // face same direction
    }
}
