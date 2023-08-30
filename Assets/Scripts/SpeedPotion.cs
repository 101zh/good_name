using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SpeedPotion : MonoBehaviour
{
    private Camera mainCam;
    private SpriteRenderer sprite;
    private Renderer gunRenderer;
    Transform playerTransform;
    bool nearTo;
    Transform gameObjects;
    Transform nameTransform;
    TMP_Text nameText;
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
            }

            playerScript.moveSpeed += 3; 
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
