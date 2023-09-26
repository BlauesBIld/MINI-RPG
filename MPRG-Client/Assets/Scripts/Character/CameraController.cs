using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Vector3 offset;

    void Start()
    {
        offset = transform.position - transform.parent.transform.position;
    }

    void Update()
    {
        
    }
}
