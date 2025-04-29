using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class PointCloudVisualiser : MonoBehaviour
{
    public ParticleSystem particleSystem;
    public Color particleColor = Color.white;
    public float particleSize = 0.01f;
    private ParticleSystem.Particle[] particles;
    private List<Vector3> points = new List<Vector3>();
    private const int arraySize = 400;
    private const string filePath = @"C:\Users\ragha\Downloads\PointCloudData.txt"; 

    void Start()
    {
        LoadPointsFromFile();
    }

    void Update()
    {
        SpawnParticles();
    }

    void LoadPointsFromFile()
    {
        points.Clear();
        if (File.Exists(filePath))
        {
            string[] lines = File.ReadAllLines(filePath);
            foreach (string line in lines)
            {
                string[] values = line.Split(','); // Change to split by comma
                if (values.Length == 3 &&
                    float.TryParse(values[0].Trim(), out float x) &&
                    float.TryParse(values[1].Trim(), out float y) &&
                    float.TryParse(values[2].Trim(), out float z))
                {
                    points.Add(new Vector3(x, y, z));
                }
            }
        }
        else
        {
            Debug.LogError("File not found: " + filePath);
        }
        int numParticles = points.Count;
        if (particles == null || particles.Length < numParticles)
            particles = new ParticleSystem.Particle[numParticles];

        var main = particleSystem.main;
        main.startColor = particleColor;
        main.startSize = particleSize;

        for (int i = 0; i < numParticles; i++)
        {
            particles[i].startColor = particleColor;
            particles[i].startSize = particleSize;
            particles[i].position = points[i];
            particles[i].remainingLifetime = 1f;
        }
    }

    void SpawnParticles()
    {
        
        particleSystem.SetParticles(particles);
    }
}