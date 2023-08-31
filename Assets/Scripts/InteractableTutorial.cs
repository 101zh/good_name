using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InteractableTutorial : MonoBehaviour
{
    bool pushed = false;
    bool nearTo;
    bool canPush;
    Rigidbody2D parentRb;
    [SerializeField] GameObject pushTextGameObject;
    [SerializeField] GameObject tips;
    [SerializeField] TMP_Text pushText;
    // Start is called before the first frame update
    void Start()
    {
        parentRb = GetComponentInParent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (pause_menu.gameIsPaused) return;

        canPush = nearTo && !pushed;
        if (canPush && Input.GetButtonDown("Interact"))
        {
            StartCoroutine(Push());
            pushed=true;
            pushText.text="I'm not gonna let you push me again! >:(";
        }
    }

    IEnumerator Push()
    {
        parentRb.constraints = ~RigidbodyConstraints2D.FreezePosition;
        parentRb.AddForce(parentRb.transform.right * 6, ForceMode2D.Impulse);
        yield return new WaitForSeconds(1.25f);
        parentRb.constraints = RigidbodyConstraints2D.FreezeAll;
    }

    private void OnTriggerEnter2D()
    {
        nearTo = true;
        pushTextGameObject.SetActive(true);
        tips.SetActive(true);
    }

    private void OnTriggerExit2D()
    {
        nearTo = false;
        pushTextGameObject.SetActive(false);
        tips.SetActive(false);
    }
}
