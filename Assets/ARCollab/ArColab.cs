using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class ArColab : NetworkBehaviour
{
    public Button playerListBtn;
    public Button objectListBtn;
    public Button playerListCloseBtn;
    public Button objectListCloseBtn;

    public GameObject[] spawnObjectsList;
    public Transform marker;
    public GameObject minUI;
    public GameObject[] playerList;

    private SceneManager main;

    void Start()
    {
        playerListBtn.onClick.AddListener(() => activateUI(2));
        objectListBtn.onClick.AddListener(() => activateUI(3));
        playerListCloseBtn.onClick.AddListener(() => activateUI(1));
        objectListCloseBtn.onClick.AddListener(() => activateUI(1));
    }

    private void Update()
    {
        if (main.joinedGame)
        {
            placeMarker();
        }
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
        if (Input.touchCount == 0)
            return;

        Vector2 screenPoint = Input.GetTouch(0).position;
        if (main.ARType == ARType.AR) // Assuming ARType is defined elsewhere
        {
            // Marker placement logic (e.g., RaycastHit etc.)
        }
    }
}
