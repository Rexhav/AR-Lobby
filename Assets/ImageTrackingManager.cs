using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System.Collections.Generic;

public class ImageTrackingManager: MonoBehaviour
{
    private ARTrackedImageManager trackedImageManager;

    // Assign 4 different prefabs for each image in the Inspector
    public List<GameObject> prefabs; 
    private Dictionary<string, GameObject> spawnedObjects = new Dictionary<string, GameObject>();
    private Dictionary<string, GameObject> prefabDictionary = new Dictionary<string, GameObject>();

    void Awake()
    {
        trackedImageManager = GetComponent<ARTrackedImageManager>();

        // Map each prefab to a specific image name
        prefabDictionary.Add("Image1", prefabs[0]);  // Prefab for Image1
        prefabDictionary.Add("Image2", prefabs[1]);  // Prefab for Image2
        prefabDictionary.Add("Image3", prefabs[2]);  // Prefab for Image3
        prefabDictionary.Add("Image4", prefabs[3]);  // Prefab for Image4
    }

    void OnEnable()
    {
        trackedImageManager.trackedImagesChanged += OnTrackedImagesChanged;
    }

    void OnDisable()
    {
        trackedImageManager.trackedImagesChanged -= OnTrackedImagesChanged;
    }

    private void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        foreach (ARTrackedImage trackedImage in eventArgs.added)
        {
            SpawnPrefab(trackedImage);
        }

        foreach (ARTrackedImage trackedImage in eventArgs.updated)
        {
            UpdatePrefab(trackedImage);
        }

        foreach (ARTrackedImage trackedImage in eventArgs.removed)
        {
            RemovePrefab(trackedImage);
        }
    }

    private void SpawnPrefab(ARTrackedImage trackedImage)
    {
        string imageName = trackedImage.referenceImage.name;

        if (!spawnedObjects.ContainsKey(imageName) && prefabDictionary.ContainsKey(imageName))
        {
            GameObject newObject = Instantiate(prefabDictionary[imageName], trackedImage.transform.position, Quaternion.identity);
            spawnedObjects[imageName] = newObject;
        }
    }

    private void UpdatePrefab(ARTrackedImage trackedImage)
    {
        if (spawnedObjects.TryGetValue(trackedImage.referenceImage.name, out GameObject obj))
        {
            obj.transform.position = trackedImage.transform.position;
            obj.transform.rotation = trackedImage.transform.rotation;
        }
    }

    private void RemovePrefab(ARTrackedImage trackedImage)
    {
        if (spawnedObjects.TryGetValue(trackedImage.referenceImage.name, out GameObject obj))
        {
            Destroy(obj);
            spawnedObjects.Remove(trackedImage.referenceImage.name);
        }
    }
}
