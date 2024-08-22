using System.Collections;
using GorillaTycoon.DataManagement;
using UnityEngine;

// ReSharper disable Unity.PerformanceCriticalCodeNullComparison
// ReSharper disable Unity.PerformanceCriticalCodeInvocation

namespace GorillaTycoon.BananaFarm;

public class Banana : MonoBehaviour
{
    public bool grabbed;
    public bool magnified;
    public float value = 5;
    
    private Transform _leftHand;
    private Transform _rightHand;
    private MeshCollider _collider;
    private Rigidbody _rb;
    
    private bool _grabbedWithRightHand;

    public void Start()
    {
        StartCoroutine(DelayDestroyBanana());
        _collider = gameObject.GetComponent<MeshCollider>();
        _collider.enabled = true;
        // _collider.size = new Vector3(0.2f, 0.2f, 0.2f);
        gameObject.SetLayer(UnityLayer.GorillaBodyCollider);
        _rb = gameObject.AddComponent<Rigidbody>();
        _rb.useGravity = true;
        
        gameObject.GetComponent<Renderer>().material.shader = Shader.Find("GorillaTag/" + "UberShader");
        transform.localScale *= 2.5f;
        BananaSpawner.Ins.activeBananas.Add(this);
        value = CalcValue();
    }

    private float CalcValue()
    {
        float value = (float)((5 * (0.5 * DataContainer.Ins.ValuableBananas)) + 2.5);
        return value;
    }

    private void FixedUpdate()
    {
        if (transform.position.y <= -15)
        {
            var newPos = transform.position;
            newPos = new Vector3(newPos.x, newPos.y + 40, newPos.z);
            transform.position = newPos;
        }
        
        float pickupDistance = DataContainer.Ins.Collection * 0.5f;
        _leftHand = Plugin.Ins.myRig.leftIndex.fingerBone3;
        _rightHand = Plugin.Ins.myRig.rightIndex.fingerBone3;
        if (Vector3.Distance(_rightHand.position, transform.position) <= pickupDistance && InputManager.Ins.rightGrip)
        {
            if (magnified) return;
            grabbed = true;
            _grabbedWithRightHand = true;
            
        }
        else if (Vector3.Distance(_leftHand.position, transform.position) <= pickupDistance && InputManager.Ins.leftGrip)
        {
            if (magnified) return;
            grabbed = true;
            _grabbedWithRightHand = false;
        }
        else
        {
            if (!grabbed) return;

            if ((_grabbedWithRightHand && !InputManager.Ins.rightGrip) ||
                (!_grabbedWithRightHand && !InputManager.Ins.leftGrip))
            {
                if (magnified) return;
                grabbed = false;
                Vector3 bucketPos = BananaBucket.Ins.transform.position;
                if (Vector3.Distance(bucketPos, transform.position) <= 2)
                    transform.position = bucketPos; // idk if this would look good
            }
        }
    }

    private void Update()
    {
        _rb.useGravity = (!grabbed && !magnified);
        _collider.enabled = (!grabbed && !magnified);

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
        yield return new WaitForSeconds(90);
        if (!magnified && !grabbed &&
            Vector3.Distance(transform.position, _rightHand.position) > 5 &&
            Vector3.Distance(transform.position, _leftHand.position) > 5 &&
            !BananaBucket.Ins.bananaBucketList.Contains(this))
        {
            BananaSpawner.Ins.activeBananas.Remove(this);
            Destroy(gameObject);
        }
        else
        {
            StartCoroutine(DelayDestroyBanana());
        }
    }
}