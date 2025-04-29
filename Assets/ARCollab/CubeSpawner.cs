using Unity.Netcode;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System.Collections.Generic;


public class CubeSpawner2 : NetworkBehaviour
{
    public GameObject cubePrefab;
    private ARRaycastManager raycastManager;
    private List<ARRaycastHit> hits = new();

    void Start()
    {
        raycastManager = GetComponent<ARRaycastManager>();
    }

    void Update()
    {
        if (!IsOwner || !Input.touchCount.Equals(1)) return;

        Touch touch = Input.GetTouch(0);

        if (touch.phase == TouchPhase.Began)
        {
            if (raycastManager.Raycast(touch.position, hits, TrackableType.PlaneWithinPolygon))
            {
                Pose hitPose = hits[0].pose;
                SpawnCubeServerRpc(hitPose.position, hitPose.rotation);
            }
        }
    }

    [ServerRpc(RequireOwnership = false)]
    void SpawnCubeServerRpc(Vector3 position, Quaternion rotation, ServerRpcParams rpcParams = default)
    {
        GameObject cube = Instantiate(cubePrefab, position, rotation);
        cube.GetComponent<NetworkObject>().SpawnWithOwnership(rpcParams.Receive.SenderClientId);
    }
}
