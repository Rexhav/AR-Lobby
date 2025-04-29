using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class NetworkUI : MonoBehaviour
{
    public Button hostBtn, clientBtn;

    public void StartHostFromUI()
    {
        NetworkManager.Singleton.StartHost();
    }

    public void StartClientFromUI()
    {
        NetworkManager.Singleton.StartClient();
    }
}
