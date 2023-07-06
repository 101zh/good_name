
using UnityEngine;

public class camera_controller : MonoBehaviour
{
    public Transform target;
    public float smoothSpeed;
    public Vector3 offset;


    void FixedUpdate()
    {
        Vector3 desiredPos=target.position+offset;
        Vector3 smoothedPos= Vector3.Lerp(transform.position, desiredPos, smoothSpeed*Time.deltaTime);
        transform.position= smoothedPos;
    }

}
