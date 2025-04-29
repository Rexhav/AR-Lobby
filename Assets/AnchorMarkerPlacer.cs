using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class AnchorMarkerPlacer : MonoBehaviour
{
    [SerializeField] private ARRaycastManager raycastManager;
    [SerializeField] private ARAnchorManager anchorManager;
    [SerializeField] private GameObject markerPrefab;
    [SerializeField] private GameObject cubePrefab;
    [SerializeField] private GameObject anchorSpritePrefab; 



    private ARAnchor placedAnchor;
    private GameObject spawnedMarker;

    static List<ARRaycastHit> hits = new List<ARRaycastHit>();

    void Update()
    {
        if (Input.touchCount == 0) return;

        Touch touch = Input.GetTouch(0);
        if (touch.phase != TouchPhase.Began) return;

        if (raycastManager.Raycast(touch.position, hits, TrackableType.PlaneWithinPolygon))
        {
            Pose hitPose = hits[0].pose;

            // If no anchor yet, place one
            if (placedAnchor == null)
            {
                placedAnchor = anchorManager.AddAnchor(hitPose);
                if (placedAnchor != null)
                {
                    Debug.Log("✅ Anchor placed.");

                    // Spawn 2D sprite at anchor location
                    Instantiate(anchorSpritePrefab, placedAnchor.transform.position, placedAnchor.transform.rotation, placedAnchor.transform);
                }
            }


            else if (spawnedMarker == null)
            {
                spawnedMarker = Instantiate(markerPrefab, hitPose.position, hitPose.rotation, placedAnchor.transform);
                Debug.Log("Marker spawned on plane near anchor.");
            }
        }
    }
}