using System.Collections;
using UnityEngine;

public class BurnableObject : MonoBehaviour
{
    [Header("Stats")]

    [Tooltip("Fire level increases as the object is on fire, once the object is a level 3 fire, it may start spreading to other objects.")]
    [SerializeField] private int _fireLevel;
    [SerializeField] private int _fireHP = 500;
    [SerializeField] private int _currentFireHP;
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
    private bool _reIgniting = false;

    [Space]
    [Header("Prefabs")]
    [SerializeField] private GameObject[] _fireEffects;
    [SerializeField] private GameObject[] _objectPrefabs;


    public void SetObjectOnFire()
    {
        _currentFireHP = _fireHP;
        OnFire = true;
    }

    public void PutOutFire()
    {
        OnFire = false;
        PlayerController.PlayerInstance.Money += Random.Range(10, 25);
        if (!Burnt)
        {
            _fireLevel = 0;
        }
    }

    private void Start()
    {
        if (OnFire)
        {
            _currentFireHP = _fireHP;
        }
    }

    private void Update()
    {

        if (_fireEffects[0] != null) _fireEffects[0].SetActive(!Burnt && OnFire);
        if (_objectPrefabs[0] != null) _objectPrefabs[0].SetActive(!Burnt);

        if (_fireEffects[1] != null) _fireEffects[1].SetActive(Burnt && OnFire);
        if (_objectPrefabs[1] != null) _objectPrefabs[1].SetActive(Burnt);


        if (!OnFire)
        {
            if (Burnt && !_reIgniting)
            {
                StartCoroutine(ReIgnite(_spreadCooldown/4));
            }

            return;
        }


        if (_fireLevel >= 3)
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

        if (Burnt) return;

        _currentBurnTime += Time.deltaTime;

        if (_currentBurnTime >= _burnTimes[_fireLevel])
        {
            _currentBurnTime = 0;
            _fireLevel++;
            Debug.Log("Increasing Fire Level to: " + _fireLevel);
        }

        if (_fireLevel >= 5)
        {
            // Object is Burnt, and can no longer be set on fire
            Burnt = true;
            gameObject.layer = 13;

        }

    }

    private IEnumerator ReIgnite(float t)
    {
        _reIgniting = true;
        bool relit = false;
        do
        {
            yield return new WaitForSeconds(t);
            int i = Random.Range(0, 2);
            Debug.Log(i);
            if (i == 1)
            {
                SetObjectOnFire();
                relit = true;
            }

        } while(!relit);
        _reIgniting = false;

    }


    private void SpreadFire()
    {
        // Get all objects in the target layer within the spread range of the current object
        int numObjects = Physics.OverlapSphereNonAlloc(transform.position, _spreadRange, nearbyObjects, _targetLayer);

        // Do not continue with the function if there are no objects left to burn
        if (numObjects == 0) return;

        bool validTarget = false;
        int maxTry = 5;

        // Look for a valid target within the nearby burnable objects and set it on fire aswell
        do
        {
            int objectIndex = UnityEngine.Random.Range(0, numObjects);
            if (nearbyObjects[objectIndex].TryGetComponent(out BurnableObject obj))
            {
                if (!obj.OnFire && !obj.Burnt)
                {
                    obj.SetObjectOnFire();
                    validTarget = true;
                }
            }
            maxTry--;

        } while (!validTarget || maxTry > 0);

        Debug.Log("Spread Fire");

    }

    private void OnParticleCollision(GameObject other)
    {
        if (!OnFire) return;

        //Debug.Log("Water Hit Object");
        _currentFireHP--;

        if(_currentFireHP < 1)
        {
            PutOutFire();
            // TODO: Give player money when they put out a fire
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the objects's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _spreadRange);
    }
}
