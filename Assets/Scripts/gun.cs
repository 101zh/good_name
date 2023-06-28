using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gun : MonoBehaviour
{
    // Start is called before the first frame update

    private Camera mainCam;
    void Start()
    {
        mainCam=Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 dir = mainCam.ScreenToWorldPoint(Input.mousePosition)-transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x)*Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle,Vector3.forward);
    }
}
