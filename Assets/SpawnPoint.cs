using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SpawnPoints : MonoBehaviour
{
    public GameObject spherePrefab; // Assign a small sphere prefab in Unity Editor
    // Update the file path to point to your point cloud file
    private string filePath = @"C:\Users\ragha\Downloads\PointCloudData.txt";

    void Start()
    {
        LoadAndSpawnSpheres();
    }

    void LoadAndSpawnSpheres()
    {
        if (!File.Exists(filePath))
        {
            Debug.LogError("File not found: " + filePath);
            return;
        }

        string[] lines = File.ReadAllLines(filePath);
        List<Vector3> points = new List<Vector3>();

        foreach (string line in lines)
        {
            if (string.IsNullOrWhiteSpace(line))
                continue; // Skip empty lines

            // Split the line by commas
            string[] values = line.Split(',');
            if (values.Length == 3)
            {
                // Trim any extra whitespace and try parsing each component
                if (float.TryParse(values[0].Trim(), out float x) &&
                    float.TryParse(values[1].Trim(), out float y) &&
                    float.TryParse(values[2].Trim(), out float z))
                {
                    points.Add(new Vector3(x, y, z));
                }
                else
                {
                    Debug.LogError("Invalid point data: " + line);
                }
            }
            else
            {
                Debug.LogWarning("Unexpected data format in line: " + line);
            }
        }

        // Spawn a sphere for each point in the point cloud
        foreach (Vector3 point in points)
        {
            GameObject sphere = Instantiate(spherePrefab, point, Quaternion.identity);
            sphere.transform.localScale = Vector3.one * 0.01f; // Adjust sphere size as needed
        }

        Debug.Log("Total Spheres Spawned: " + points.Count);
    }
}