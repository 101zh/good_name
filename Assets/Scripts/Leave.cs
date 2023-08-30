using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Leave : MonoBehaviour
{
    [SerializeField] GameObject leaveText;
    private bool canLeave;
    public delegate void Fade();
    public static event Fade onFadeOut;
    void Update()
    {
        if (pause_menu.gameIsPaused) return;
        if (canLeave && Input.GetButtonDown("Interact"))
        {
            GameObject.FindGameObjectWithTag("Canvas").transform.GetChild(7).gameObject.SetActive(true);
            onFadeOut?.Invoke();
            StartCoroutine(WaitForFadeOut());
            Debug.Log("leaving");
        }
    }

    IEnumerator WaitForFadeOut(){
        yield return new WaitForSeconds(4f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag.Equals("Player"))
        {
            leaveText.SetActive(true);
            canLeave = true;
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.tag.Equals("Player"))
        {
            leaveText.SetActive(false);
            canLeave = false;
        }
    }
}
