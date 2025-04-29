using Unity.Netcode;
using UnityEngine;

public class PlayerNetwork : NetworkBehaviour
{
    public ArColab arColab;

    [ServerRpc]
    public void spawnObjServerRPC(int index, ServerRpcParams rpcParams = default)
    {
        GameObject instance = Instantiate(arColab.spawnObjectsList[index]);
        NetworkObject netObj = instance.GetComponent<NetworkObject>();
        netObj.SpawnWithOwnership(rpcParams.Receive.SenderClientId);

        instance.transform.position = arColab.marker.transform.position;
        arColab.spawnedObjectList.Add(instance); // If you maintain a list
    }

    [ServerRpc]
    public void sendMsgServerRPC(ChatMessageNetPack _msg)
    {
        sendMsgClientRPC(_msg);
    }

    [ClientRpc]
    public void sendMsgClientRPC(ChatMessageNetPack _msg)
    {
        if (IsLocalPlayer || _msg.receiverId == main.scenMag.localPlayer.GetComponent<NetworkObject>().OwnerClientId)
        {
            Debug.Log($"Received Message: {_msg.message} from {_msg.senderId}");
        }
    }
}
