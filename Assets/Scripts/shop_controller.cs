using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shop_controller : MonoBehaviour
{
    Object[] prefabs;
    Transform displayTables;
    void Awake()
    {
        prefabs = Resources.LoadAll("Shopkeeper");
        displayTables = transform.GetChild(2);
    }

    public void SetUpShop()
    {
        gameObject.SetActive(true);
        Debug.Log("Setting Up Shop");
        for (int i = 0; i < 3; i++)
        {
            try
            {
                Destroy(displayTables.GetChild(i).GetChild(0).gameObject);
            }
            catch { }
            SpawnRandom(displayTables.GetChild(i));
        }
    }

    public void TakeDownShop()
    {
        Debug.Log("Taking Down Shop");
        gameObject.SetActive(false);
    }

    private void SpawnRandom(Transform tableTransforms)
    {
        Object random = prefabs[Random.Range(0, prefabs.Length)];
        Instantiate(random, tableTransforms.position, Quaternion.identity, tableTransforms);
    }

}
