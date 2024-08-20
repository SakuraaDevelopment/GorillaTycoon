using System;
using System.Collections;
using System.Collections.Generic;
using GorillaTycoon.DataManagement;
using UnityEngine;
using UnityEngine.Serialization;
using Random = System.Random;
using UnityEngine.AI;
// ReSharper disable Unity.InefficientMultiplicationOrder

// ReSharper disable Unity.PerformanceCriticalCodeNullComparison
// ReSharper disable Unity.PerformanceCriticalCodeInvocation

namespace GorillaTycoon.BananaFarm;

public class Drone : MonoBehaviour
{
    private int _maxBananas = 3;
    private float _droneSpeed = 1;
    
    private float _avoidanceStrength = 10f;
    private float _detectionRange = 2f;

    private List<Banana> _magnifiedBananas = new List<Banana>();
    private Vector3 _currentTargetPos;
    private Vector3 _basePosition = new Vector3(-61f, 16f, -42f);
    
    private void Start()
    {
        gameObject.SetLayer(UnityLayer.GorillaBodyCollider);
        transform.localScale = new Vector3(0.125f, 0.125f, 0.125f);
        _currentTargetPos = _basePosition;
        transform.position = _basePosition;
    }

    private void Update()
    {
        if (Vector3.Distance(transform.position, _currentTargetPos) <= 0.2f)
            _currentTargetPos = GetTargetPosition();

        FlyTowardsTarget();
    }

    private void FlyTowardsTarget()
    {
        Vector3 direction = (_currentTargetPos - transform.position).normalized;

        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
        
            if (Quaternion.Angle(transform.rotation, targetRotation) > 0.1f)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * _droneSpeed);
                return;
            }
        }

        transform.position += direction * _droneSpeed * Time.deltaTime;
    }



    private Vector3 GetTargetPosition()
    {
        List<Banana> activeUntouchedBananas = new List<Banana>();
        foreach (Banana bananaThing in BananaSpawner.Ins.activeBananas)
        {
            if (bananaThing.grabbed || bananaThing.magnified)
                continue;
            
            activeUntouchedBananas.Add(bananaThing);
        }

        if (activeUntouchedBananas.Count == 0)
            return _basePosition;
        if (activeUntouchedBananas.Count == 1)
            return BananaSpawner.Ins.activeBananas[0].transform.position;
        
        int midNum = (int)Math.Round((float)(BananaSpawner.Ins.activeBananas.Count / 2), 0);
        Vector3 midPos = BananaSpawner.Ins.activeBananas[midNum].transform.position;
        midPos.y += 1;
        return midPos;
    }
}