using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gun : MonoBehaviour
{
    // Start is called before the first frame update

    private Camera mainCam;
    GameObject player;
    void Start()
    {
        mainCam=Camera.main;
        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        //Makes Gun follow the player, so it looks like the player is always holding it
        transform.position= player.transform.position;
        gunRotate();
    }

    private void gunRotate()
    {
        // finds the position of the mouse using camera (has to be relative to it) and position of gun
        Vector2 dir = mainCam.ScreenToWorldPoint(Input.mousePosition)-transform.position;
        // Finding the angle to rotate using math
        float angle = Mathf.Atan2(dir.y, dir.x)*Mathf.Rad2Deg;
        // Rotates the gun using math
        transform.rotation = Quaternion.AngleAxis(angle,Vector3.forward);
    }
}
