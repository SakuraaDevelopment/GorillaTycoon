using System;
using System.Collections;
using System.Collections.Generic;
using GorillaTycoon.DataManagement;
using UnityEngine;
using UnityEngine.Serialization;
using Random = System.Random;

// ReSharper disable Unity.PerformanceCriticalCodeNullComparison
// ReSharper disable Unity.PerformanceCriticalCodeInvocation

namespace GorillaTycoon.BananaFarm;

public class Drone : MonoBehaviour
{
    private void Start()
    {
        
    }

    private Vector3 GetTargetPosition()
    {
        int activeBananas = BananaSpawner.Ins.activeBananas.Count;
        
        if (activeBananas == 0)
            return droneStation.Ins.dronePad.position;
        else if (activeBananas == 1)
            return BananaSpawner.Ins.activeBananas[0].transform.position;
        
        int midNum = Math.Round((BananaSpawner.Ins.activeBananas.Count / 2), 0)
            return BananaSpawner.Ins.activeBananas[midNum].transform.position;
    }
}