using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using Unity.Netcode;
using System.Collections.Generic;

public class MarkerCubeSpawner : NetworkBehaviour
{
    [SerializeField] private ARTrackedImageManager trackedImageManager;
    [SerializeField] private ARRaycastManager raycastManager;
    [SerializeField] private GameObject cubePrefab;

    private List<ARRaycastHit> hits = new List<ARRaycastHit>();
    private bool cubeSpawned = false;

    void Update()
    {
        if (!IsHost || cubeSpawned) return;

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
        {
            Vector2 touchPos = Input.GetTouch(0).position;

            if (raycastManager.Raycast(touchPos, hits, TrackableType.Image))
            {
                Pose hitPose = hits[0].pose;
                var cube = Instantiate(cubePrefab, hitPose.position, hitPose.rotation);
                cube.GetComponent<NetworkObject>().Spawn(true);
                cubeSpawned = true;
            }
        }
    }
}
