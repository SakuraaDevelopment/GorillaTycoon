using System;
using System.Collections.Generic;
using GorillaTycoon.DataManagement;
using UnityEngine;
using UnityEngine.Serialization;

namespace GorillaTycoon.BananaFarm;

public class BananaBucket : MonoBehaviour
{
    public static BananaBucket Ins;
    public List<Banana> bananaBucketList = new List<Banana>();
    
    private void Start()
    {
        Ins = this;
        gameObject.GetComponent<Renderer>().enabled = false;
        gameObject.GetComponent<BoxCollider>().isTrigger = true;
    }
    
    public void SellBucket()
    {
        float sellValue = 0;
        foreach (Banana banana in bananaBucketList)
        {
            sellValue += banana.value;
            Destroy(banana.gameObject);
        }
        bananaBucketList.Clear();
        DataContainer.Ins.Coins += sellValue;
    }
    
    private void OnTriggerEnter(Collider collider)
    {
        if (!collider.transform.TryGetComponent(out Banana bananaInBucket)) return;
        
        if (!bananaBucketList.Contains(bananaInBucket))
            bananaBucketList.Add(bananaInBucket);
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.transform.TryGetComponent(out Banana bananaInBucket))
        {
            bananaBucketList.Remove(bananaInBucket);
        }
    }
}