using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed;
    private void Update()
    {
        Vector3 movement = Vector3.zero;
        if (Input.GetKey(KeyCode.W)) movement += transform.forward;
        if (Input.GetKey(KeyCode.S)) movement -= transform.forward;
        if (Input.GetKey(KeyCode.A)) movement -= transform.right;
        if (Input.GetKey(KeyCode.D)) movement += transform.right;
        
        transform.position += Time.deltaTime * speed * movement;
    }
}