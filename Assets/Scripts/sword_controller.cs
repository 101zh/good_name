using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sword_controller : MonoBehaviour
{
    private Camera mainCam;
    private SpriteRenderer sprite;
    private GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        mainCam = Camera.main;
        player = GameObject.FindWithTag("Player");
        sprite = GetComponentInChildren<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!pause_menu.gamePaused)
        {
            //Makes Gun follow the player, so it looks like the player is always holding it
            transform.position = new Vector2(player.transform.position.x, player.transform.position.y - 0.5f);
            swordRotate();
            if (Input.GetButtonDown("Fire1")) //checks if player has pressed the shoot button
            {
                Debug.Log("Swing!!");
                // have yet to implement swinging a sword
            }
        }
    }

    private void swordRotate()
    {
        // finds the position of the mouse using camera (has to be relative to it) and position of gun
        Vector2 dir = mainCam.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        // Finding the angle to rotate using math
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        // Rotates the gun using math
        transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
        if (!(angle <= 90 && angle >= -90))
        {
            sprite.flipX = true;
        }
        else
        {
            sprite.flipX = false;
        }
    }
}
