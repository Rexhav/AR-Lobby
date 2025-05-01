using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System.Collections.Generic;

public enum ARType { AR, VR }

public class ArColab : NetworkBehaviour
{
    [Header("UI")]
    public Button playerListBtn;
    public Button objectListBtn;
    public Button playerListCloseBtn;
    public Button objectListCloseBtn;
    public GameObject minUI;
    public GameObject[] playerList;

    [Header("Spawning")]
    public GameObject[] spawnObjectsList;
    public Transform marker;

    [Header("AR Components")]
    public ARRaycastManager arRaycastManager;
    public GameObject markerPrefab; // A marker GameObject like an image/plane
    public GameObject anchorPrefab; // Optional anchor prefab (can just use marker)

    private static List<ARRaycastHit> hits = new List<ARRaycastHit>();

    private SceneManager main; // Reference to your custom scene manager

    void Awake()
    {
        main = FindObjectOfType<SceneManager>();
    }

    void Start()
    {
        playerListBtn.onClick.AddListener(() => activateUI(2));
        objectListBtn.onClick.AddListener(() => activateUI(3));
        playerListCloseBtn.onClick.AddListener(() => activateUI(1));
        objectListCloseBtn.onClick.AddListener(() => activateUI(1));
    }

    private void Update()
    {
        if (!IsOwner || main == null || !main.joinedGame)
            return;

        placeMarker();
    }

    void activateUI(int uiIndex)
    {
        minUI.SetActive(uiIndex == 1);
        playerList[0].SetActive(uiIndex == 2);
        playerList[1].SetActive(uiIndex == 3);
    }

    public void spawnObject(int index)
    {
        if (!IsOwner) return;

        main.scenMag.localPlayerNet.spawnObjServerRPC(index);
    }

    void placeMarker()
    {
        if (Input.touchCount == 0 || Input.GetTouch(0).phase != TouchPhase.Began)
            return;

        Vector2 screenPoint = Input.GetTouch(0).position;

        if (main.ARType == ARType.AR && arRaycastManager.Raycast(screenPoint, hits, TrackableType.PlaneWithinPolygon))
        {
            Pose hitPose = hits[0].pose;

            // Spawn marker locally (visual)
            if (markerPrefab)
                Instantiate(markerPrefab, hitPose.position, hitPose.rotation);

            // Place anchor and sync via network
            if (IsOwner)
            {
                main.scenMag.localPlayerNet.spawnAnchorServerRPC(hitPose.position, hitPose.rotation);
            }
        }
    }
}
