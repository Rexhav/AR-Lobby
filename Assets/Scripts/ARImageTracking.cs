using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System.Collections.Generic;

public class NCU_ImageTracking: MonoBehaviour
{
    private ARTrackedImageManager trackedImageManager;

   
    public List<GameObject> prefabs; 
    private Dictionary<string, GameObject> spawnedObjects = new Dictionary<string, GameObject>();
    private Dictionary<string, GameObject> prefabDictionary = new Dictionary<string, GameObject>();

    void Awake()
    {
        trackedImageManager = GetComponent<ARTrackedImageManager>();

        
        prefabDictionary.Add("marker", prefabs[0]); 
        prefabDictionary.Add("coke", prefabs[1]);  
        prefabDictionary.Add("no entry", prefabs[2]); 
        prefabDictionary.Add("nose", prefabs[3]);  
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