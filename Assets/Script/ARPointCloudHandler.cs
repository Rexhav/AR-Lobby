using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

[RequireComponent(typeof(ARPointCloudManager))]
public class ArPointCloudHandler : MonoBehaviour
{
    private HashSet<Vector3> pointsCloud = new HashSet<Vector3>(); // Prevents duplicates
    private ARPointCloudManager _arPointCloudManager;
    public TMPro.TMP_Text TMPText;

    private string filePath;

    private void Awake()
    {
        _arPointCloudManager = GetComponent<ARPointCloudManager>();

        // Changed file path to work on mobile
        filePath = Path.Combine(Application.persistentDataPath, "PointCloudData.txt");

        LoadPointCloudFromFile(); // Load previously saved points
    }

    private void OnEnable()
    {
        _arPointCloudManager.pointCloudsChanged += OnPointCloudChanged;
    }

    private void Update()
    {
        TMPText.text = $"Points cloud count: {pointsCloud.Count}";
    }

    private void OnPointCloudChanged(ARPointCloudChangedEventArgs obj)
    {
        bool newPointsAdded = false;

        foreach (var pointCloud in obj.updated)
        {
            if (pointCloud.positions.HasValue)
            {
                foreach (Vector3 point in pointCloud.positions.Value)
                {
                    if (pointsCloud.Add(point)) // Adds only if it's unique
                    {
                        newPointsAdded = true;
                    }
                }
            }
        }

        if (newPointsAdded)
        {
            SavePointCloudToFile();
        }
    }

    private void SavePointCloudToFile()
    {
        using (StreamWriter writer = new StreamWriter(filePath, false)) // Overwrites with updated points
        {
            foreach (var point in pointsCloud)
            {
                writer.WriteLine($"{point.x}, {point.y}, {point.z}");
            }
        }

        Debug.Log("Point cloud saved at: " + filePath);
    }

    private void LoadPointCloudFromFile()
    {
        if (!File.Exists(filePath))
            return;

        foreach (var line in File.ReadAllLines(filePath))
        {
            var parts = line.Split(',');
            if (parts.Length == 3 &&
                float.TryParse(parts[0], out float x) &&
                float.TryParse(parts[1], out float y) &&
                float.TryParse(parts[2], out float z))
            {
                pointsCloud.Add(new Vector3(x, y, z)); // Load existing points
            }
        }

        Debug.Log("Loaded previous point cloud data.");
    }

    private void OnDisable()
    {
        _arPointCloudManager.pointCloudsChanged -= OnPointCloudChanged;
    }
}
