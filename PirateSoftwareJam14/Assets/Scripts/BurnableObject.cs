using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurnableObject : MonoBehaviour
{
    [Header("Stats")]
    [Tooltip("Fire level increases as the object is on fire, once the object is a level 3 fire, it may start spreading to other objects.")]
    [SerializeField] private int _fireLevel;
    [Tooltip("The amount of time an object takes to go from level 1 > 5")]
    [SerializeField] private float[] _burnTimes =  new float[5];
    public bool OnFire = false;
    public bool Burnt = false;

    [Space]
    [Header("Prefabs")]
    [SerializeField] private GameObject[] _fireEffects;


    private void Update()
    {
        // Currently only set up for One fire effect, will implement multiple
        _fireEffects[0].SetActive(OnFire);


        if(_fireLevel >= 3)
        {
            // If the fire level is greater than or equals 3 then find other flamable objects in range and try to spread the fire
        } else if (_fireLevel >= 5)
        {
            // Object is Burnt, and can no longer be put out, or set on fire
        }
    }
}
