using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System.Collections.Generic;

public class ARObjectPlacer : MonoBehaviour
{
    public GameObject markerPrefab;
    public GameObject cubePrefab;
    private GameObject spawnedMarker;
    private GameObject spawnedCube;

    private ARRaycastManager raycastManager;
    private List<ARRaycastHit> hits = new List<ARRaycastHit>();

    private bool objectPlaced = false;

    void Start()
    {
        raycastManager = GetComponent<ARRaycastManager>();
    }

    void Update()
    {
        if (objectPlaced) return;

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                if (raycastManager.Raycast(touch.position, hits, TrackableType.PlaneWithinPolygon))
                {
                    Pose hitPose = hits[0].pose;

                    spawnedMarker = Instantiate(markerPrefab, hitPose.position, hitPose.rotation);
                    spawnedCube = Instantiate(cubePrefab, hitPose.position + Vector3.up * 0.1f, Quaternion.identity);
                    objectPlaced = true;
                }
            }
        }
    }
}
