using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor.Build;
using UnityEngine;

public class BurnableObject : MonoBehaviour
{
    [Header("Stats")]

    [Tooltip("Fire level increases as the object is on fire, once the object is a level 3 fire, it may start spreading to other objects.")]
    [SerializeField] private int _fireLevel;
    [SerializeField] private float _spreadCooldown = 60f;
    [SerializeField] private float _spreadRange = 5f;
    [SerializeField] private LayerMask _targetLayer;

    [Tooltip("The amount of time an object takes to go from level 1 > 5")]
    [SerializeField] private float[] _burnTimes = new float[5];

    public bool OnFire = false;
    public bool Burnt = false;

    private Collider[] nearbyObjects = new Collider[10];

    private float _currentBurnTime = 0;
    private float _currentSpreadTime = 0;

    [Space]
    [Header("Prefabs")]
    [SerializeField] private GameObject[] _fireEffects;
    [SerializeField] private GameObject[] _treePrefabs;


    private void Update()
    {

        if (_fireEffects[0] != null) _fireEffects[0].SetActive(!Burnt && OnFire);
        if (_treePrefabs[0] != null) _treePrefabs[0].SetActive(!Burnt);

        if (_fireEffects[1] != null) _fireEffects[1].SetActive(Burnt && OnFire);
        if (_treePrefabs[1] != null) _treePrefabs[1].SetActive(Burnt);

        if (Burnt || !OnFire) return;
        _currentBurnTime += Time.deltaTime;

        if (_currentBurnTime >= _burnTimes[_fireLevel])
        {
            _currentBurnTime = 0;
            _fireLevel++;
            Debug.Log("Increasing Fire Level to: " + _fireLevel);
        }

        if (_fireLevel >= 5)
        {
            // Object is Burnt, and can no longer be put out, or set on fire
            Burnt = true;
            gameObject.layer = 13;

        } else if (_fireLevel >= 3)
        {
            // If the fire level is greater than or equals 3 then find other flamable objects in range and try to spread the fire
            // Only spread every x seconds, that way the player has time to control the flame.
            _currentSpreadTime -= Time.deltaTime;

            if (_currentSpreadTime <= 0)
            {
                _currentSpreadTime = _spreadCooldown;
                SpreadFire();
            }
        }
    }


    private void SpreadFire()
    {
        // Get all objects in the target layer within the spread range of the current object
        int numObjects = Physics.OverlapSphereNonAlloc(transform.position, _spreadRange, nearbyObjects, _targetLayer);

        // Do not continue with the function if there are no objects left to burn
        if (numObjects == 0) return;

        bool validTarget = false;

        // Look for a valid target within the nearby burnable objects and set it on fire aswell
        do
        {
            int objectIndex = UnityEngine.Random.Range(0, numObjects);
            if (nearbyObjects[objectIndex].TryGetComponent(out BurnableObject obj))
            {
                if(!obj.OnFire && !obj.Burnt)
                {
                    obj.OnFire = true;
                    validTarget = true;
                }
            }
        } while (!validTarget);

        Debug.Log("Spread Fire");

    }

    private void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the objects's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _spreadRange);
    }
}
