using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    private float _droneSpeed = 3;

    private List<Banana> _magnifiedBananas = new List<Banana>();
    private Vector3 _currentTargetPos;
    private Vector3 _basePosition = new Vector3(-60.7f, 15, -41f);
    private bool _returningHome;
    private float _timeSinceHome;
    
    private void Start()
    {
        gameObject.SetLayer(UnityLayer.GorillaBodyCollider);
        transform.localScale = new Vector3(0.125f, 0.125f, 0.125f);
        _currentTargetPos = _basePosition;
        transform.position = _basePosition;
    }

    private void Update()
    {
        if (_returningHome)
            _timeSinceHome = 0;
        else _timeSinceHome += Time.deltaTime;
    }

    private void FixedUpdate()
    {
        foreach (Banana magnifiedBanana in _magnifiedBananas)
        {
            Vector3 dPos = transform.position;
            magnifiedBanana.transform.position = new Vector3(dPos.x, dPos.y - 0.4f, dPos.z);
        }
        if (Vector3.Distance(transform.position, BananaBucket.Ins.transform.position) >= 4)
        {
            foreach (var cleanBanana in BananaSpawner.Ins.activeBananas
                         .Where(bananaThing => !bananaThing.grabbed && !bananaThing.magnified).ToList()
                         .Where(cleanBanana =>
                             Vector3.Distance(cleanBanana.transform.position, transform.position) <= 1.5f))
            {
                if (_magnifiedBananas.Count >= _maxBananas) return;

                cleanBanana.magnified = true;
                _magnifiedBananas.Add(cleanBanana);
            }
        }

        if (_returningHome)
        {
            if (Vector3.Distance(transform.position, _currentTargetPos) <= 0.05f)
            {
                transform.position = _basePosition;
                foreach (Banana magnifiedBananas in _magnifiedBananas)
                {
                    magnifiedBananas.magnified = false;
                    transform.position = BananaBucket.Ins.transform.position;
                }
                _magnifiedBananas.Clear();
                _returningHome = false;
            }
        }
        else
        {
            if (Vector3.Distance(transform.position, _currentTargetPos) <= 0.2f)
            {
                _currentTargetPos = GetTargetPosition();
            }
        }
        
        FlyTowardsTarget();
    }

    private void FlyTowardsTarget()
    {
        Vector3 direction = (_currentTargetPos - transform.position).normalized;

        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            targetRotation *= Quaternion.Euler(0, 90, 0); 

            if (Quaternion.Angle(transform.rotation, targetRotation) > 0.1f)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * (3 * _droneSpeed));
                return;
            }
        }

        transform.position += direction * _droneSpeed * Time.deltaTime;
    }

    
    private Vector3 GetTargetPosition()
    {
        List<Banana> activeUntouchedBananas = BananaSpawner.Ins.activeBananas
            .Where(bananaThing => !bananaThing.grabbed && !bananaThing.magnified).ToList();

        if (activeUntouchedBananas.Count == 0 || _magnifiedBananas.Count >= _maxBananas)
        {
            _returningHome = true;
            return _basePosition;
        }
        if (activeUntouchedBananas.Count == 1)
            return BananaSpawner.Ins.activeBananas[0].transform.position;
        
        int midNum = (int)Math.Round((float)(BananaSpawner.Ins.activeBananas.Count / 2), 0);
        Vector3 midPos = BananaSpawner.Ins.activeBananas[midNum].transform.position;
        midPos.y += 1;
        return midPos;
    }
}