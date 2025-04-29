using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SpawnSpheresFromFile : MonoBehaviour
{
    public GameObject spherePrefab; // Assign a small sphere prefab in Unity Editor
    private string filePath = @"C:\Users\ragha\Downloads\FaceVertices.txt"; // Ensure this path is correct

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
        List<Vector3> vertices = new List<Vector3>();

        foreach (string line in lines)
        {
            if (string.IsNullOrWhiteSpace(line)) continue; // Skip empty lines

            string[] values = line.Split(','); // Split by comma
            if (values.Length == 3) // Ensure correct format
            {
                float x, y, z;
                if (float.TryParse(values[0], out x) && float.TryParse(values[1], out y) && float.TryParse(values[2], out z))
                {
                    vertices.Add(new Vector3(x, y, z));
                }
                else
                {
                    Debug.LogError("Invalid vertex data: " + line);
                }
            }
        }

       
        foreach (Vector3 vertex in vertices)
        {
            GameObject sphere = Instantiate(spherePrefab, vertex, Quaternion.identity);
            sphere.transform.localScale = Vector3.one * 0.01f; // Small sphere
        }

        Debug.Log("Total Spheres Spawned: " + vertices.Count);
    }
}