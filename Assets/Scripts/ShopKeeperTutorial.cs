using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShopKeeperTutorial : MonoBehaviour
{
    bool talkedTo = false;
    bool nearTo = false;
    bool canTalk;
    bool talking = false;
    [SerializeField] TMP_Text dialogueBoxText;
    [SerializeField] GameObject dialogueBox;
    [SerializeField] GameObject gun;
    [SerializeField] Transform playerTransform;
    SpriteRenderer spriteRenderer;
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (pause_menu.gameIsPaused) return;

        canTalk = nearTo && !talking;

        if ((canTalk && talkedTo) && Input.GetButtonDown("Interact"))
        {
            StartCoroutine(IWillNeverForgiveYou());
        }
        else if (canTalk && Input.GetButtonDown("Interact"))
        {
            StartCoroutine(Talk());
            talkedTo = true;
        }

        if (playerTransform.position.x < transform.position.x)
        {
            spriteRenderer.flipX = true;
        }
        else
        {
            spriteRenderer.flipX = false;
        }
    }

    IEnumerator Talk()
    {
        talking = true;
        dialogueBoxText.text = "I saw what you did to my table...";
        yield return new WaitForSeconds(4f);
        dialogueBoxText.text = "I'm a forgiving person, so here's a gun";
        yield return new WaitForSeconds(2.5f);
        dialogueBoxText.text = "You won't survive down here without one";
        Instantiate(gun, new Vector2(transform.position.x, transform.position.y - 3f), gun.transform.rotation);
        yield return new WaitForSeconds(3.5f);
        dialogueBoxText.text = "The first one is free";
        yield return new WaitForSeconds(3.5f);
        dialogueBoxText.text = "Talk?";
        talking = false;
    }
    IEnumerator IWillNeverForgiveYou()
    {
        talking = true;
        dialogueBoxText.text = "I will never forgive you for what you have done to my table";
        yield return new WaitForSeconds(2.5f);
        dialogueBoxText.text = "I'll steal one of your guns later...";
        yield return new WaitForSeconds(4f);
        dialogueBoxText.text = "Talk?";
        talking = false;
    }

    private void OnTriggerEnter2D()
    {
        dialogueBox.SetActive(true);
        nearTo = true;
    }

    private void OnTriggerExit2D()
    {
        dialogueBox.SetActive(false);
        nearTo = false;
    }
}
