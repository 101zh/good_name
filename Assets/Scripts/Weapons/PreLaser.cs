using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreLaser : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(WaitToDisappear());
    }

    IEnumerator WaitToDisappear(){
        yield return new WaitForSeconds(1f);
        Destroy(gameObject, 0.1f);
        StopAllCoroutines();
    }
}
