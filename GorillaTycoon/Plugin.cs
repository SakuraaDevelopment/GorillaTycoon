using System;
using BepInEx;
using GorillaTycoon.BananaFarm;
using GorillaTycoon.DataManagement;
using GorillaTycoon.Resources;
using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;

// ReSharper disable Unity.PerformanceCriticalCodeNullComparison

namespace GorillaTycoon
{
	[BepInDependency("com.ahauntedarmy.gorillatag.tmploader")]
	[BepInPlugin(PluginInfo.GUID, PluginInfo.Name, PluginInfo.Version)]
	public class Plugin : BaseUnityPlugin
	{
		public static Plugin Ins;
		public VRRig myRig;
		public bool started;
		
		void Start()
		{
			Ins = this;
			
			AssetContainer assetContainer = new AssetContainer();
			assetContainer.Start();
			AssetImporter assetImporter = new AssetImporter();
			assetImporter.Start();
			DataContainer dataContainer = new DataContainer();
			dataContainer.Start();
			DataSaver dataSaver = new DataSaver();
			dataSaver.Start();
			
			Invoke(nameof(StartBananaFarm), 10);
		}

		private void Update()
		{
			if (!started) return;
			
			myRig = PhotonNetwork.InRoom ? 
				GorillaParent.instance.vrrigDict[NetworkSystem.Instance.LocalPlayer] : 
				GorillaTagger.Instance.offlineVRRig;

			UpdateTesterButtons();
		}

		private void UpdateTesterButtons()
		{
			if (Keyboard.current.digit6Key.wasPressedThisFrame) BananaFarmComputer.Ins.OnLeftArrowPress();
			if (Keyboard.current.digit7Key.wasPressedThisFrame) BananaFarmComputer.Ins.OnDownArrowPress();
			if (Keyboard.current.digit8Key.wasPressedThisFrame) BananaFarmComputer.Ins.OnSelectPress();
			if (Keyboard.current.digit9Key.wasPressedThisFrame) BananaFarmComputer.Ins.OnUpArrowPress();
			if (Keyboard.current.digit0Key.wasPressedThisFrame) BananaFarmComputer.Ins.OnRightArrowPress();
		}

		private void StartBananaFarm()
		{
			started = true;
			GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			obj.AddComponent<BananaSpawner>();
			obj.AddComponent<InputManager>();
			
			GameObject desk = Instantiate(AssetContainer.Ins.BananaFarmerDesk);
			desk.transform.position = new Vector3(-61.4f, 15.23f, -42.3237f);
			desk.transform.rotation = Quaternion.Euler(0, 285, 0);
			desk.transform.Find("BananaBucket").Find("Collector").AddComponent<BananaBucket>();
			desk.transform.Find("Table").AddComponent<BananaFarmComputer>();
		}
	}
}
