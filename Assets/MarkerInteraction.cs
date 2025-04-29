using Unity.Netcode;
using UnityEngine;

public class MarkerInteraction : MonoBehaviour
{
    [SerializeField] private GameObject cubePrefab;

    private void OnMouseDown()
    {
        if (NetworkManager.Singleton.IsServer)
        {
            SpawnCube();
        }
        else
        {
            // Ask server to spawn via RPC
            SpawnCubeServerRpc(transform.position);
        }
    }

    private void SpawnCube()
    {
        var cube = Instantiate(cubePrefab, transform.position, Quaternion.identity);
        cube.GetComponent<NetworkObject>().SpawnWithOwnership(NetworkManager.Singleton.LocalClientId); // Host owns it
    }

    [ServerRpc(RequireOwnership = false)]
    private void SpawnCubeServerRpc(Vector3 position, ServerRpcParams rpcParams = default)
    {
        var cube = Instantiate(cubePrefab, position, Quaternion.identity);
        cube.GetComponent<NetworkObject>().SpawnWithOwnership(rpcParams.Receive.SenderClientId); // Assign ownership to the client who clicked
    }

}