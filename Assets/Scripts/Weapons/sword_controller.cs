using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class sword_controller : MonoBehaviour
{
    // Start is called before the first frame update

    private Camera mainCam;
    private SpriteRenderer sprite;
    private Animator animator;
    [SerializeField] private int swingsPerClick;
    [SerializeField] private float swingDelay; // delay between swings of one click
    [SerializeField] private float swingTime; // amount of time sword is swinging
    [SerializeField] private float coolDown; //after each click
    [SerializeField] private string idleAnimName = "blood_blade_idle";
    [SerializeField] private string swingAnimName = "blooad_blade_swing";
    public bool held; //is hte player holding the gun
    float angle;
    private float coolDownTimer;
    private Renderer swordRenderer;
    Transform playerTransform;
    Transform hitBox;
    bool nearTo;
    Transform gameObjects;
    Transform swordName;
    TMP_Text swordNameText;
    private void Start()
    {
        mainCam = Camera.main;
        sprite = GetComponent<SpriteRenderer>();
        swordRenderer = GetComponent<Renderer>();
        gameObjects = GameObject.FindGameObjectWithTag("GameObjects").GetComponent<Transform>();
        swordName = transform.GetChild(1);
        TMP_Text gunNameText = swordName.GetComponent<TMP_Text>();
        hitBox = transform.GetChild(0);
        animator = GetComponent<Animator>();
        swingDelay += swingTime;
    }

    // Update is called once per frame
    private void Update()
    {
        if (!pause_menu.gamePaused)
        {
            if (coolDownTimer > 0) { coolDownTimer = Mathf.Max(coolDownTimer - Time.deltaTime, 0f); }
            if (nearTo && Input.GetButtonDown("Interact")) { PickUpGun(playerTransform); }
            if (held)
            {
                Rotate();
                if (Input.GetButtonDown("Fire1") && coolDownTimer == 0) //checks if player has pressed the shoot button
                {
                    Swinging();
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
                swordRenderer.material.SetFloat("_Thickness", 0.06f);
                swordName.gameObject.SetActive(true);
            }
            else
            {
                swordRenderer.material.SetFloat("_Thickness", 0.0f);
                swordName.gameObject.SetActive(false);
            }
            playerTransform = collision.gameObject.transform;
            nearTo = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        swordRenderer.material.SetFloat("_Thickness", 0.0f);
        swordName.gameObject.SetActive(false);
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
        transform.position = new Vector2(player.position.x+0.45f, player.position.y - 0.55f);
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
            transform.position = new Vector2(playerTransform.position.x-0.45f, playerTransform.position.y - 0.55f);
        }
        else
        {
            sprite.flipY = false;
            transform.position = new Vector2(playerTransform.position.x+0.45f, playerTransform.position.y - 0.55f);
        }
    }

    int i;
    private void Swinging()
    {
        for (i = 0; i < swingsPerClick; i++)
        {
            Invoke("Swing", swingDelay * i);
            Debug.Log(i);
        }
    }

    private void Swing()
    {
        Debug.Log("Swing");
        hitBox.gameObject.SetActive(true);
        Invoke("Hold", swingTime);
        animator.Play(swingAnimName);
    }

    private void Hold()
    {
        animator.Play(idleAnimName);
        hitBox.gameObject.SetActive(false);
    }
}
