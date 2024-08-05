using System.Reflection;
using UnityEngine;

namespace GorillaTycoon.Resources;

public class AssetContainer
{
    public static AssetContainer Ins;
    
    public GameObject BananaFarmerDesk;
    public GameObject BananaObj;

    public void Start()
    {
        Ins = this;
    }
}