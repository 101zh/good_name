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
    [SerializeField] private int cost;
    [SerializeField] private bool sold;
    [SerializeField] private string swordName;
    [SerializeField] private string idleAnimName = "blood_blade_idle";
    [SerializeField] private string swingAnimName = "blooad_blade_swing";
    public bool held; //is hte player holding the sword
    float angle;
    private float coolDownTimer;
    private Renderer swordRenderer;
    Transform playerTransform;
    Transform hitBox;
    bool nearTo;
    Transform gameObjects;
    Transform nameTransform;
    TMP_Text nameText;
    private bool isErrorMessage;
    private void Start()
    {
        mainCam = Camera.main;
        sprite = GetComponent<SpriteRenderer>();
        swordRenderer = GetComponent<Renderer>();
        gameObjects = GameObject.FindGameObjectWithTag("GameObjects").GetComponent<Transform>();
        nameTransform = transform.GetChild(1);
        nameText = nameTransform.GetComponent<TMP_Text>();
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
            if (nearTo && Input.GetButtonDown("Interact")) { PickUpsword(); }
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
            if (isErrorMessage)
            {
                // Do nothing
            }
            else if (!sold)
            {
                nameText.text = swordName + " <color=yellow>" + cost.ToString() + "</color>";
            }
            else
            {
                nameText.text = swordName;
            }

            if (!held)
            {
                RevealName();
            }
            else
            {
                HideName();
            }
            playerTransform = collision.gameObject.transform;
            nearTo = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        HideName();
        nearTo = false;
    }

    private void PickUpsword()
    {
        if (!sold)
        {
            player_controller playerScript = this.playerTransform.GetComponent<player_controller>();
            if (playerScript.coins <= cost && isErrorMessage)
            {
                return;
            }
            else if (playerScript.coins <= cost)
            {
                nameText.text = "<color=red>Not Enough Money</color>";
                RevealName();
                isErrorMessage = true;
                Invoke("DisableErrorMessage", 1.5f);
                return;
            }
            else
            {
                sold = true;
                playerScript.incrementCoins(-cost);
            }
        }
        Transform weaponInventory = playerTransform.GetChild(2);
        weapon_switching script = weaponInventory.GetComponent<weapon_switching>();
        Transform currentHeldWeapon = weaponInventory.GetChild(script.heldWeaponIndex);

        if (weaponInventory.childCount >= 2)
        {
            if (currentHeldWeapon.tag.Equals("sword")) { currentHeldWeapon.GetComponent<sword_controller>().held = false; }
            else if (currentHeldWeapon.tag.Equals("Spear")) { currentHeldWeapon.GetComponent<spear_controller>().held = false; }
            else { currentHeldWeapon.GetComponent<gun_controller>().held = false; }
            currentHeldWeapon.SetParent(gameObjects, true);
            currentHeldWeapon.GetComponent<SpriteRenderer>().sortingOrder = 0;
        }
        else
        {
            currentHeldWeapon.gameObject.SetActive(false);
        }

        //Makes sword follow the player, so it looks like the player is always holding it
        transform.SetParent(weaponInventory, true);
        script.heldWeaponIndex = transform.GetSiblingIndex();
        held = true;
        sprite.sortingOrder = 2;
        transform.position = new Vector2(playerTransform.position.x + 0.15f, playerTransform.position.y - 0.55f);
        script.SelectWeapon();
    }

    private void Rotate()
    {
        // finds the position of the mouse using camera (has to be relative to it) and position of sword
        Vector2 dir = mainCam.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        // Finding the angle to rotate using math
        angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        // Rotates the sword using math
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        if (angle <= -90 || angle >= 90)
        {
            sprite.flipY = true;
            transform.position = new Vector2(playerTransform.position.x - 0.3f, playerTransform.position.y - 0.55f);
        }
        else
        {
            sprite.flipY = false;
            transform.position = new Vector2(playerTransform.position.x + 0.3f, playerTransform.position.y - 0.55f);
        }
    }

    private void Swinging()
    {
        for (int i = 0; i < swingsPerClick; i++)
        {
            Invoke("Swing", swingDelay * i);
        }
    }

    private void Swing()
    {
        hitBox.gameObject.SetActive(true);
        Invoke("Hold", swingTime);
        animator.Play(swingAnimName);
    }

    private void Hold()
    {
        animator.Play(idleAnimName);
        hitBox.gameObject.SetActive(false);
    }

    private void HideName()
    {
        swordRenderer.material.SetFloat("_Thickness", 0.0f);
        nameTransform.gameObject.SetActive(false);
    }

    private void RevealName()
    {
        swordRenderer.material.SetFloat("_Thickness", 0.06f);
        nameTransform.gameObject.SetActive(true);
    }

    private void DisableErrorMessage()
    {
        isErrorMessage = false;
    }

}
