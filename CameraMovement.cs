using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float RotX = 1.0f;
    public float RotY = 1.0f;
    public float RotZ = 1.0f;

    void Start()
    {
        
    }

    void Update()
    {
        transform.Rotate(
            Time.deltaTime * RotX,
            Time.deltaTime * RotY,
            Time.deltaTime * RotZ
        );
    }
}
