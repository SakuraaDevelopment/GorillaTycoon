﻿using System.Reflection;
using UnityEngine;

namespace GorillaTycoon.Resources;

public class AssetContainer
{
    public static AssetContainer Ins;
    
    public GameObject BananaFarmerDesk;
    public GameObject BananaObj;
    public GameObject DroneObj;

    public void Start()
    {
        Ins = this;
    }
}