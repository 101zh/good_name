using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class credits : MonoBehaviour
{
    [SerializeField] GameObject skipMenu;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(WaitGoToNextScene());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKey && skipMenu.activeSelf)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
        }
        else if (Input.anyKey)
        {
            skipMenu.SetActive(true);
            StartCoroutine(DisableSkipMenu());
        }
    }

    IEnumerator DisableSkipMenu()
    {
        yield return new WaitForSeconds(3f);
        skipMenu.SetActive(false);
    }

    IEnumerator WaitGoToNextScene()
    {
        yield return new WaitForSeconds(35f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
    }
}
