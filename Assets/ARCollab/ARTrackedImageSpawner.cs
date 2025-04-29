using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using Unity.Netcode;

public class ARTrackedImageSpawner : MonoBehaviour
{
    public ARTrackedImageManager trackedImageManager;
    public GameObject cubePrefab;

    private void OnEnable()
    {
        trackedImageManager.trackedImagesChanged += OnTrackedImagesChanged;
    }

    private void OnDisable()
    {
        trackedImageManager.trackedImagesChanged -= OnTrackedImagesChanged;
    }

    void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        if (!NetworkManager.Singleton.IsHost) return;

        foreach (var trackedImage in eventArgs.added)
        {
            SpawnCube(trackedImage);
        }
    }

    void SpawnCube(ARTrackedImage trackedImage)
    {
        var cube = Instantiate(cubePrefab, trackedImage.transform.position, Quaternion.identity);
        cube.GetComponent<NetworkObject>().Spawn(true);
    }
}
