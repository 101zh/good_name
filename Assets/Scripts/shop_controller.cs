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

    void Start()
    {
        for (int i = 0; i < 3; i++)
        {
            SpawnRandom(displayTables.GetChild(i));
        }
    }

    void SetUpShop()
    {
        gameObject.SetActive(true);
        for (int i = 0; i < 3; i++)
        {
            Destroy(displayTables.GetChild(i).GetChild(0).gameObject);
            SpawnRandom(displayTables.GetChild(i));
        }
    }

    void TakeDownShop()
    {
        gameObject.SetActive(false);
    }

    void SpawnRandom(Transform tableTransforms)
    {
        Object random = prefabs[Random.Range(0, prefabs.Length - 1)];
        Instantiate(random, tableTransforms.position, Quaternion.identity, tableTransforms);
    }

    private void OnEnable()
    {
        WaveSpawner.OnWaveComplete += SetUpShop;
        WaveSpawner.OnWaveStart += TakeDownShop;
    }

    private void OnDisable()
    {
        WaveSpawner.OnWaveComplete -= SetUpShop;
        WaveSpawner.OnWaveStart -= TakeDownShop;
    }

}
