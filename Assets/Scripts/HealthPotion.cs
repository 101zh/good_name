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
    Transform gunName;
    TMP_Text gunNameText;
    Health healthtesting;
    private void Start()
    {
        mainCam = Camera.main;
        sprite = GetComponent<SpriteRenderer>();
        gunRenderer = GetComponent<Renderer>();
        gameObjects = GameObject.FindGameObjectWithTag("GameObjects").GetComponent<Transform>();
        gunName = transform.GetChild(1);
        TMP_Text gunNameText = gunName.GetComponent<TMP_Text>();
        healthtesting = GameObject.FindWithTag("Player").GetComponent<Health>();
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            gunRenderer.material.SetFloat("_Thickness", 0.06f);
            gunName.gameObject.SetActive(true);
            playerTransform = collision.gameObject.transform;
            nearTo = true;
        }
    }
    private void Update()
    {
        if (nearTo && Input.GetButtonDown("Interact"))
        {
            healthtesting.OnChangeHealth(-5);
            Destroy(gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        gunRenderer.material.SetFloat("_Thickness", 0.0f);
        gunName.gameObject.SetActive(false);
        nearTo = false;
    }
}
