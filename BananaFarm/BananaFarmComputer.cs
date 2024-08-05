﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using GorillaTycoon.DataManagement;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using Debug = UnityEngine.Debug;
// ReSharper disable Unity.PerformanceCriticalCodeInvocation

namespace GorillaTycoon.BananaFarm;

public class BananaFarmComputer : MonoBehaviour
{
    public static BananaFarmComputer Ins;
    private int _currentPage = 1;
    private Transform _textContainer;
    private TMP_Text _pageText;
    private TMP_Text _p1Text;
    private TMP_Text _p2Text;
    private TMP_Text _p3Text;
    private TMP_Text _ugInfoText;
    
    private bool _confirmationChoice = false;
    private int _selectedLine;
    private int _selectedUpgrade;

    private void Start()
    {
        Ins = this;
        SetupButtons();
        DefinePages();
        SetComputerPage(1);
    }

    private void Update()
    {
        UpdatePages();
    }

    private void DefinePages()
    {
        _textContainer = transform.Find("Computer").Find("Text");
        _pageText = _textContainer.Find("PageText").GetComponent<TextMeshPro>();
        _p1Text = _textContainer.Find("P1Text").GetComponent<TextMeshPro>();
        _p2Text = _textContainer.Find("P2Text").GetComponent<TextMeshPro>();
        _p3Text = _textContainer.Find("P3Text").GetComponent<TextMeshPro>();
        _ugInfoText = _textContainer.Find("UpgradeInfoText").GetComponent<TextMeshPro>();
    }

    private void SetComputerPage(int targetPage)
    {
        if (targetPage <= 0) targetPage = 1;
        _selectedLine = 1;
        switch (targetPage)
        {
            case 1:
                _pageText.gameObject.SetActive(true);
                _pageText.text = "PAGE: [1] 2 3";
                _p1Text.gameObject.SetActive(true);
                _p2Text.gameObject.SetActive(false);
                _p3Text.gameObject.SetActive(false);
                _ugInfoText.gameObject.SetActive(false);
                break;
            
            case 2:
                _pageText.text = "PAGE: 1 [2] 3";
                _pageText.gameObject.SetActive(true);
                _p1Text.gameObject.SetActive(false);
                _p2Text.gameObject.SetActive(true);
                _p3Text.gameObject.SetActive(false);
                _ugInfoText.gameObject.SetActive(false);
                break;
            
            case 3:
                _pageText.gameObject.SetActive(true);
                _p1Text.gameObject.SetActive(false);
                _p2Text.gameObject.SetActive(false);
                _p3Text.gameObject.SetActive(true);
                _ugInfoText.gameObject.SetActive(false);
                _pageText.text = "PAGE: 1 2 [3]";
                break;
            
            case 4:
                _pageText.gameObject.SetActive(false);
                _p1Text.gameObject.SetActive(false);
                _p2Text.gameObject.SetActive(false);
                _p3Text.gameObject.SetActive(false);
                _ugInfoText.gameObject.SetActive(true);
                _confirmationChoice = false;
                break;
        }
        _currentPage = targetPage;
    }

    private void UpdatePages()
    {
        switch (_currentPage)
        {
            case 1:
                RenderText(new ComputerLineInfo[] 
                {
                    new ComputerLineInfo() { Text = $"BANANA'S DETECTED: {BananaBucket.Ins.bananaBucketList.Count}" },
                    new ComputerLineInfo() { Text = $"COINS: {DataContainer.Ins.Coins}" },
                    new ComputerLineInfo() { Text = "\n" },
                    new ComputerLineInfo() { Text = "SELL BANANAS", IsSelectable = true, IsSelected = true}
                }, _p1Text);
                break;
            case 2:
                RenderText(new ComputerLineInfo[] 
                {
                    new ComputerLineInfo() { Text = $"UPGRADES:" },
                    new ComputerLineInfo() { Text = $"" },
                    new ComputerLineInfo() { Text = $"VALUABLE BANANAS: TIER {DataContainer.Ins.ValuableBananas}", 
                        IsSelectable = true, IsSelected = (_selectedLine == 1), HasLeftPad = true},
                    new ComputerLineInfo() { Text = $"BANANA COOLDOWN: {DataContainer.Ins.BananaCooldown} SEC", 
                        IsSelectable = true, IsSelected = (_selectedLine == 2), HasLeftPad = true},
                    new ComputerLineInfo() { Text = $"BANANA DURATION: {DataContainer.Ins.BananaDuration} SEC", 
                        IsSelectable = true, IsSelected = (_selectedLine == 3), HasLeftPad = true},
                    new ComputerLineInfo() { Text = $"COLLECTION: {DataContainer.Ins.Collection} SEC", 
                        IsSelectable = true, IsSelected = (_selectedLine == 4), HasLeftPad = true}
                }, _p2Text);
                break;
            case 3:
                RenderText(new ComputerLineInfo[] 
                {
                    new ComputerLineInfo() { Text = $"SPAWNING STATS:" },
                    new ComputerLineInfo() { Text = $"COMING SOON"}
                }, _p3Text);
                break;
            case 4:
                int ugLevel = GetUpgradeLevel(_selectedUpgrade);
                RenderText(new ComputerLineInfo[] 
                {
                    new ComputerLineInfo() { Text = $"{GetUpgradeName(_selectedUpgrade)}:" },
                    new ComputerLineInfo() { Text = $""},
                    new ComputerLineInfo() { Text = $"${CalcUgCost(_selectedUpgrade, ugLevel)}"},
                    new ComputerLineInfo() { Text = $"TIER {ugLevel} -> TIER {ugLevel + 1}"},
                    new ComputerLineInfo() { Text = $""},
                    new ComputerLineInfo() { Text = $"CONFIRM?"},
                    new ComputerLineInfo() { Text = $"NO", IsSelected = true},
                    new ComputerLineInfo() { Text = $"YES", IsSelected = true}
                }, _ugInfoText);
                break;
        }
    }

    private String GetUpgradeName(int upgrade)
    {
        String ugName = "";
        switch (upgrade)
        {
            case 1:
                ugName = "VALUABLE BANANAS";
                break;
            case 2:
                ugName = "BANANA COOLDOWN";
                break;
            case 3:
                ugName = "BANANA DURATION";
                break;
            case 4:
                ugName = "COLLECTION";
                break;
        }
        return ugName;
    }

    private int GetUpgradeLevel(int upgrade)
    {
        int lvl = 0;
        switch (upgrade)
        {
            case 1:
                lvl = DataContainer.Ins.ValuableBananas;
                break;
            case 2:
                lvl = DataContainer.Ins.BananaCooldown;
                break;
            case 3:
                lvl = DataContainer.Ins.BananaDuration;
                break;
            case 4:
                lvl = DataContainer.Ins.Collection;
                break;
        }
        return lvl;
    }

    private float CalcUgCost(int upgrade, int level)
    {
        switch (upgrade)
        {
            case 1: // VALUABLE BANANAS
                return (float)Math.Round(65 * Math.Pow(level, 2), 0);
            case 2: // BANANA COOLDOWN
                return (float)Math.Round(15 * Math.Pow(level, 2), 0);
            case 3: // BANANA DURATION
                return (float)Math.Round(20 * Math.Pow(level, 2), 0);
            case 4: // COLLECTION
                return (float)Math.Round(100 * Math.Pow(level, 2), 0);
        }
        return 0;
    }

    public void OnLeftArrowPress()
    {
        if (_currentPage == 4)
        {
            _confirmationChoice = !_confirmationChoice;
            return;
        }

        int maxAmount = 0;
        switch (_currentPage)
        {
            case 1:
                maxAmount = 1;
                break;
            case 2:
                maxAmount = 3;
                break;
            case 4:
                maxAmount = 2;
                break;
        }
        int targetPage = _currentPage - 1;
        if (targetPage < 1)
            targetPage = 3;
        SetComputerPage(targetPage);
    }

    public void OnRightArrowPress()
    {
        if (_currentPage == 4)
        {
            _confirmationChoice = !_confirmationChoice;
            return;
        }
        
        int targetPage = _currentPage + 1;
        if (targetPage > 3)
            targetPage = 1;
        SetComputerPage(targetPage);
    }

    public void OnSelectPress()
    {
        switch (_currentPage)
        {
            case 1:
            {
                BananaBucket.Ins.SellBucket();
                break;
            }
            case 2:
                _selectedUpgrade = _selectedLine;
                SetComputerPage(4);
                break;
        }
    }

    public void OnUpArrowPress()
    {
        int targetLine = _selectedLine - 1;
        if (targetLine < 1)
            targetLine = 4;
        _selectedLine = targetLine;
    }

    public void OnDownArrowPress()
    {
        int targetLine = _selectedLine + 1;
        if (targetLine > 4)
            targetLine = 1;
        _selectedLine = targetLine;
    }
    
    private void SetupButtons()
    {
        void SetButtonListener(Transform parent, string buttonName, UnityAction action)
        {
            var buttonGameObject = parent.gameObject;
            buttonGameObject.layer = 18;

            GorillaPressableButton button = buttonGameObject.AddComponent<GorillaPressableButton>();
            var ue = new UnityEvent();
            ue.AddListener(action);
            button.onPressButton = ue;
        }
        
        SetButtonListener(transform.GetChild(2).GetChild(1), "LeftArrowBtn", OnLeftArrowPress);
        SetButtonListener(transform.GetChild(3).GetChild(1), "DownArrowBtn", OnDownArrowPress);
        SetButtonListener(transform.GetChild(4).GetChild(1), "SelectBtn", OnSelectPress);
        SetButtonListener(transform.GetChild(5).GetChild(1), "UpArrowBtn", OnUpArrowPress);
        SetButtonListener(transform.GetChild(6).GetChild(1), "RightArrowBtn", OnRightArrowPress);
    }

    private void RenderText(ComputerLineInfo[] lines, TMP_Text pageText)
    {
        string text = "";
        foreach (ComputerLineInfo line in lines)
        {
            string prefix = "";
            if (line.IsSelected) prefix = "> ";
            else if (line.HasLeftPad) prefix = "  ";
            text += $"{prefix}{line.Text} \n";
        }
        pageText.text = text;
    }
    
    private struct ComputerLineInfo()
    {
        public bool IsSelectable = false;
        public bool IsSelected = false;
        public bool HasLeftPad = false;
        public string Text = "";
    }
}