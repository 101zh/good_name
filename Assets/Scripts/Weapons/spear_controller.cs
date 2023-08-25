using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class spear_controller : MonoBehaviour
{
    // Start is called before the first frame update

    private Camera mainCam;
    private SpriteRenderer sprite;
    [SerializeField] private float thrustSpeed;
    [SerializeField] private int thrustsPerClick;
    [SerializeField] private float thrustDelay;
    [SerializeField] private float thrustLength;
    [SerializeField] private float coolDown; //after each shot
    public bool held; //is hte player holding the gun
    float angle;
    private float coolDownTimer;
    private Renderer gunRenderer;
    Transform playerTransform;
    bool nearTo;
    Transform gameObjects;
    Transform gunName;
    TMP_Text gunNameText;
    private void Start()
    {
        mainCam = Camera.main;
        sprite = GetComponent<SpriteRenderer>();
        gunRenderer = GetComponent<Renderer>();
        gameObjects = GameObject.FindGameObjectWithTag("GameObjects").GetComponent<Transform>();
        gunName = transform.GetChild(1);
        TMP_Text gunNameText = gunName.GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (!pause_menu.gamePaused || !pause_menu.playerDead)
        {
            if (coolDownTimer > 0) { coolDownTimer = Mathf.Max(coolDownTimer - Time.deltaTime, 0f); }
            if (nearTo && Input.GetButtonDown("Interact")) { PickUpGun(playerTransform); }
            if (held)
            {
                Rotate();
                if (Input.GetButtonDown("Fire1") && coolDownTimer == 0) //checks if player has pressed the shoot button
                {
                    Thrust();
                    coolDownTimer = coolDown;
                }
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            if (!held)
            {
                gunRenderer.material.SetFloat("_Thickness", 0.06f);
                gunName.gameObject.SetActive(true);
            }
            else
            {
                gunRenderer.material.SetFloat("_Thickness", 0.0f);
                gunName.gameObject.SetActive(false);
            }
            playerTransform = collision.gameObject.transform;
            nearTo = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        gunRenderer.material.SetFloat("_Thickness", 0.0f);
        gunName.gameObject.SetActive(false);
        nearTo = false;
    }

    private void PickUpGun(Transform player)
    {
        Transform weaponInventory = player.GetChild(2);
        weapon_switching script = weaponInventory.GetComponent<weapon_switching>();
        Transform currentHeldWeapon = weaponInventory.GetChild(script.heldWeaponIndex);

        if (weaponInventory.childCount >= 2)
        {
            if (currentHeldWeapon.tag.Equals("Gun")) { currentHeldWeapon.GetComponent<gun_controller>().held = false; }
            else if (currentHeldWeapon.tag.Equals("Spear")) { currentHeldWeapon.GetComponent<spear_controller>().held = false; }
            else { currentHeldWeapon.GetComponent<sword_controller>().held = false; }
            currentHeldWeapon.SetParent(gameObjects, true);
            currentHeldWeapon.GetComponent<SpriteRenderer>().sortingOrder = 0;
        }
        else
        {
            currentHeldWeapon.gameObject.SetActive(false);
        }

        //Makes Gun follow the player, so it looks like the player is always holding it
        transform.SetParent(weaponInventory, true);
        script.heldWeaponIndex = transform.GetSiblingIndex();
        held = true;
        sprite.sortingOrder = 2;
        transform.position = new Vector2(player.position.x, player.position.y - 0.5f);
        script.SelectWeapon();
    }

    private void Rotate()
    {
        // finds the position of the mouse using camera (has to be relative to it) and position of gun
        Vector2 dir = mainCam.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        // Finding the angle to rotate using math
        angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        // Rotates the gun using math
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        if (!(angle <= -90 && angle >= 90))
        {
            sprite.flipY = true;
        }
        else
        {
            sprite.flipY = false;
        }
    }

    private IEnumerator Thrust()
    {
        Debug.Log("Thrust");
        var radians = angle*Mathf.Deg2Rad;
        Vector2 destination = new Vector2(Mathf.Cos(radians), Mathf.Sin(radians));
        destination*=thrustLength;
        while(new Vector2(transform.position.x, transform.position.y)==destination){
            transform.position = Vector2.Lerp(transform.position, destination, thrustSpeed);
            yield return null;
        }
        yield return null;
    }
}
