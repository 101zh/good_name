using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HealthPotion : MonoBehaviour
{
    private Camera mainCam;
    private SpriteRenderer sprite;
    private Renderer gunRenderer;
    Transform playerTransform;
    bool nearTo;
    Transform gameObjects;
    Transform nameTransform;
    TMP_Text nameText;
    Health healthtesting;
    bool isErrorMessage;
    [SerializeField] private int cost;
    [SerializeField] private string displayName;
    private void Start()
    {
        mainCam = Camera.main;
        sprite = GetComponent<SpriteRenderer>();
        gunRenderer = GetComponent<Renderer>();
        gameObjects = GameObject.FindGameObjectWithTag("GameObjects").GetComponent<Transform>();
        nameTransform = transform.GetChild(1);
        nameText = nameTransform.GetComponent<TMP_Text>();
        healthtesting = GameObject.FindWithTag("Player").GetComponent<Health>();
        nameText.text += " <color=yellow>" + cost.ToString() + "</color>";
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            RevealName();
            playerTransform = collision.gameObject.transform;
            nearTo = true;
        }
    }
    private void Update()
    {
        if (pause_menu.gameIsPaused) return;
        if (nearTo && Input.GetButtonDown("Interact"))
        {
            player_controller playerScript = playerTransform.GetComponent<player_controller>();
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
                playerScript.incrementCoins(-cost);
                FindObjectOfType<AudioManager>().Play("PotionDrink");
            }

            healthtesting.currentHealth += 5;
            if(healthtesting.currentHealth>healthtesting.maxHealth){
                healthtesting.currentHealth=healthtesting.maxHealth;
            }
            healthtesting.OnChangeHealth(0);
            Destroy(gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        HideName();
        nearTo = false;
    }

    private void HideName()
    {
        gunRenderer.material.SetFloat("_Thickness", 0.0f);
        nameTransform.gameObject.SetActive(false);
    }

    private void RevealName()
    {
        gunRenderer.material.SetFloat("_Thickness", 0.06f);
        nameTransform.gameObject.SetActive(true);
    }

    private void DisableErrorMessage()
    {
        isErrorMessage = false;
        nameText.text = displayName;
    }
}
