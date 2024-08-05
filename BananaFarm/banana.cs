using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
// ReSharper disable Unity.PerformanceCriticalCodeNullComparison
// ReSharper disable Unity.PerformanceCriticalCodeInvocation

namespace GorillaTycoon.BananaFarm;

public class Banana : MonoBehaviour
{
    public bool grabbed;
    public int value = 5;
    
    private Transform _leftHand;
    private Transform _rightHand;
    private BoxCollider _boxCollider;
    private Rigidbody _rb;
    
    private bool _grabbedWithRightHand;
    
    public void Start()
    {
        StartCoroutine(DelayDestroyBanana());
        _boxCollider = gameObject.AddComponent<BoxCollider>();
        _boxCollider.enabled = true;
        _boxCollider.size = new Vector3(0.2f, 0.2f, 0.2f);
        gameObject.SetLayer(UnityLayer.GorillaBodyCollider);
        _rb = gameObject.AddComponent<Rigidbody>();
        _rb.useGravity = true;
        
        gameObject.GetComponent<Renderer>().material.shader = Shader.Find("GorillaTag/" + "UberShader");
        transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
    }

    private void FixedUpdate()
    {
        _leftHand = Plugin.Ins.myRig.leftIndex.fingerBone3;
        _rightHand = Plugin.Ins.myRig.rightIndex.fingerBone3;
        if (Vector3.Distance(_rightHand.position, transform.position) <= 0.5f && InputManager.Ins.rightGrip)
        {
            grabbed = true;
            _grabbedWithRightHand = true;
            
        }
        else if (Vector3.Distance(_leftHand.position, transform.position) <= 0.5f && InputManager.Ins.leftGrip)
        {
            grabbed = true;
            _grabbedWithRightHand = false;
        }
        else
        {
            if (!grabbed) return;

            if ((_grabbedWithRightHand && !InputManager.Ins.rightGrip) ||
                (!_grabbedWithRightHand && !InputManager.Ins.leftGrip))
                grabbed = false;
        }
    }

    private void Update()
    {
        _rb.useGravity = !grabbed;
        _boxCollider.enabled = !grabbed;

        if (grabbed)
        {
            transform.position = _grabbedWithRightHand ? _rightHand.position : _leftHand.position;
        }
        else
        {
            transform.parent = null;
        }
    }

    private IEnumerator DelayDestroyBanana()
    {
        yield return new WaitForSeconds(90f);
        if (!grabbed && 
            Vector3.Distance(transform.position, _rightHand.position) > 5 && 
            Vector3.Distance(transform.position, _leftHand.position) > 5 &&
            !BananaBucket.Ins.bananaBucketList.Contains(this))
            Destroy(gameObject);
        else
        {
            StartCoroutine(DelayDestroyBanana());
        }
    }
}