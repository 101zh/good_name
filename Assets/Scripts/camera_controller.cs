
using UnityEngine;

public class camera_controller : MonoBehaviour
{
    public Transform target;
    public float smoothSpeed;
    public Vector3 offset;
    private Vector3 velocity = Vector3.zero;

    void FixedUpdate()
    {
        Vector3 desiredPos = target.position + offset;
        desiredPos.z = transform.position.z;
        Vector3 smoothedPos = Vector3.SmoothDamp(transform.position, desiredPos, ref velocity, smoothSpeed);
        transform.position = smoothedPos;
    }

}
