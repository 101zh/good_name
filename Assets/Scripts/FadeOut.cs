using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeOut : MonoBehaviour
{

    private void PlayFadeOut()
    {
        GetComponent<Animator>().Play("FadeOut");
    }
    private void OnEnable()
    {
        Leave.onFadeOut += PlayFadeOut;

    }

    private void OnDisable()
    {
        Leave.onFadeOut -= PlayFadeOut;
    }
}
