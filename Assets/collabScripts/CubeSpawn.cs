using UnityEngine;

public class CubeSpawn : MonoBehaviour
{
    public GameObject marker;
    public GameObject cubePrefab;

    private GameObject spawnedCube;

    void Update()
    {
        if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.gameObject == marker && spawnedCube == null)
                {
                    spawnedCube = Instantiate(cubePrefab, marker.transform.position, Quaternion.identity);
                }
            }
        }
    }
}
