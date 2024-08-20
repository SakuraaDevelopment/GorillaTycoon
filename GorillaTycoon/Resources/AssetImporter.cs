using System.IO;
using System.Reflection;
using UnityEngine;

namespace GorillaTycoon.Resources;

public class AssetImporter
{
    private AssetBundle _ab;
    public void Start()
    {
        RetrieveAssetBundle();
        Debug.Log("Imported asset bundle");
        ImportBananaFarmAssets();
        Debug.Log("Imported banana farm assets");
        
    }

    private void RetrieveAssetBundle()
    {
        Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("GorillaTycoon.Resources.tycoonab");
        _ab = AssetBundle.LoadFromStream(stream);
        // ReSharper disable once PossibleNullReferenceException
        stream.Close();
        
        if (_ab == null) Debug.LogError("Failed to load GorillaTycoon Asset Bundle!");
    }

    private void ImportBananaFarmAssets()
    {
        AssetContainer ac = AssetContainer.Ins;
        ac.BananaFarmerDesk = _ab.LoadAsset<GameObject>("BananaFarmerDesk");
        ac.BananaObj = _ab.LoadAsset<GameObject>("banana");
        ac.DroneObj = _ab.LoadAsset<GameObject>("drone");
    }
}