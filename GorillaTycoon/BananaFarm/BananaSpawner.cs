using System;
using System.Collections;
using System.Collections.Generic;
using GorillaTycoon.DataManagement;
using GorillaTycoon.Resources;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

// ReSharper disable Unity.PerformanceCriticalCodeInvocation

namespace GorillaTycoon.BananaFarm;

public class BananaSpawner : MonoBehaviour
{
    public static BananaSpawner Ins;
    public GameObject bananaPrefab;
    public float cd;
    public List<Banana> activeBananas = new List<Banana>();
    
    public void Start()
    {
        Ins = this;
        GetComponent<Collider>().enabled = false;
        transform.position = new Vector3(-52, 10, -62);
        transform.localScale = new Vector3(49, 1, 47);
        
        // bananaPrefab = GameObject.CreatePrimitive(PrimitiveType.Cube);
        bananaPrefab = AssetContainer.Ins.BananaObj;
        
        StartCoroutine(BananaSpawningLoop());
    }

    private IEnumerator BananaSpawningLoop()
    {
        while (true)
        {
            SpawnBanana();
            yield return new WaitForSeconds(cd);
        }
    }

    private void FixedUpdate()
    {
        cd = (27f - (DataContainer.Ins.BananaCooldown * 3f));
    }

    private void SpawnBanana()
    {
        Bounds bounds = GetComponent<Renderer>().bounds;

        float x = Random.Range(bounds.min.x, bounds.max.x);
        float y = Random.Range(bounds.min.y, bounds.max.y);
        float z = Random.Range(bounds.min.z, bounds.max.z);

        Vector3 randomPosition = new Vector3(x, y, z);

        GameObject banana = Instantiate(bananaPrefab, randomPosition, Quaternion.identity);
        banana.AddComponent<Banana>();
    }
}