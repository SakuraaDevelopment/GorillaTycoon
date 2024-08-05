using System;
using System.Collections;
using System.IO;
using UnityEngine;
// ReSharper disable Unity.PerformanceCriticalCodeInvocation

namespace GorillaTycoon.DataManagement;

using Newtonsoft.Json;

public class DataSaver
{
    public void Start()
    {
        ImportData();
        Plugin.Ins.StartCoroutine(DataSavingLoop());
    }
    
    private IEnumerator DataSavingLoop()
    {
        while (true)
        {
            ExportData();
            yield return new WaitForSeconds(60f);
        }
    }

    public void ExportData()
    {
        var json = JsonConvert.SerializeObject(DataContainer.Ins);
        var saveFilePath = Path.Combine(BepInEx.Paths.ConfigPath, "TycDataSave.json");

        File.WriteAllText(saveFilePath, json);
            
        Debug.Log("Saved tycoon.");
    }
    
    private void ImportData()
    {
        var saveFilePath = Path.Combine(BepInEx.Paths.ConfigPath, "TycDataSave.json");
        if (File.Exists(saveFilePath))
        {
            var json = File.ReadAllText(saveFilePath);
            var saveData = JsonConvert.DeserializeObject<DataContainer>(json);
            DataContainer.Ins.Coins = saveData.Coins;
            DataContainer.Ins.BananaCooldown = saveData.BananaCooldown;
            DataContainer.Ins.Collection = saveData.Collection;
            DataContainer.Ins.ValuableBananas = saveData.ValuableBananas;
            DataContainer.Ins.BananaDuration = saveData.BananaDuration;
        }
    }
}