using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using Unity.Netcode;

public class CubeSpawner : NetworkBehaviour
{
    public GameObject cubePrefab;
    private ARRaycastManager raycastManager;
    static List<ARRaycastHit> hits = new List<ARRaycastHit>();

    void Start()
    {
        raycastManager = FindObjectOfType<ARRaycastManager>();
    }

    void Update()
    {
        if (!IsOwner || cubePrefab == null || raycastManager == null)
            return;

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            Vector2 touchPosition = Input.GetTouch(0).position;

            if (raycastManager.Raycast(touchPosition, hits, TrackableType.PlaneWithinPolygon))
            {
                Pose hitPose = hits[0].pose;

                GameObject cube = Instantiate(cubePrefab, hitPose.position, hitPose.rotation);
                cube.GetComponent<NetworkObject>().Spawn(true); // spawn with ownership
            }
            else
            {
                Debug.LogWarning("Raycast hit nothing!");
            }
        }
    }
}