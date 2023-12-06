using UnityEngine;

public class TopViewFollowPlayer : MonoBehaviour
{
    // Reference to the player's transform.
    public Transform playerTransform;

    // The offset distance between the player and camera.
    public Vector3 offset = new(0f, 7f, 0f);

    // Optional: for smoothing the camera movement.
    [Range(0.01f, 1.0f)]
    public float smoothFactor = 0.05f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }   

    void LateUpdate()
    {
        // Calculate the desired position of the camera by adding the offset to the player's position.
        Vector3 desiredPosition = playerTransform.position + offset;

        // Optional: Smoothly interpolate between the camera's current position and its desired position.
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothFactor);

        // Update the position of the camera to follow the player.
        transform.position = smoothedPosition;
    }
}
