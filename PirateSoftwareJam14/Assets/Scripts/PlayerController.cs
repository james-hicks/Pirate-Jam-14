using Cinemachine;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    public static PlayerController PlayerInstance { get => _instance; }
    private static PlayerController _instance;

    [Header("Player Variables")]
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _sprintMultiplier;
    [SerializeField] private float _stamina = 100;
    private bool _isSprinting;

    [SerializeField] private Transform _camera;

    [Space]
    public int Money = 0;

    [Space]
    [SerializeField] private float _gravityScale = 1f;
    public static float GlobalGravity = -9.81f;

    [Header("Visuals Variables")]
    [SerializeField] private GameObject _playerModel;
    [SerializeField] private float _turnSmoothTime = 0.1f; // turn smooth time set up for 
    [SerializeField] private CinemachineFreeLook _freeLookCamera;
    float turnSmoothVelocity; // ref float for SmoothDampAngle Function

    [Header("Hose Variables")]
    [SerializeField] private float _hoseMaxCapacity;
    [SerializeField] private float _currentHoseCapacity;
    public bool Firing;
    [SerializeField] private ParticleSystem _hoseParticleEffect;

    [Header("HUD Elements")]
    [SerializeField] private Slider _waterSlider;
    [SerializeField] private Slider _staminaSlider;
    [SerializeField] private TextMeshProUGUI _moneyText;
    [SerializeField] private GameObject _pauseMenu;
    [SerializeField] private GameObject _interactPrompt;

    [Space]
    public Vector2 moveInput;
    private Animator _animator;
    private bool weaponIsFiring => Firing && _currentHoseCapacity > 0;
    private bool hasMoveInput => moveInput != Vector2.zero; // simple bool set up to check if the user is giving a movement input

    private Rigidbody _rb;

    private bool _gameIsPaused;

    private void Awake()
    {
        if (_instance == null)
            _instance = this;
        else
            Destroy(this);
    }

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _animator = GetComponentInChildren<Animator>();
        _currentHoseCapacity = _hoseMaxCapacity;
        _waterSlider.maxValue = _hoseMaxCapacity;
        _rb.useGravity = false;


        // lock the cursor on start for testing
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void FixedUpdate()
    {
        float smoothAngle;
        float targetAngle;

        if (hasMoveInput)
        {
            //Rotate player model to direction of travel
            targetAngle = Mathf.Atan2(moveInput.x, moveInput.y) * Mathf.Rad2Deg + _camera.eulerAngles.y;
            smoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, _turnSmoothTime);
            //float smoothAngle = Mathf.LerpAngle(transform.rotation.y, targetAngle, Time.time);
            transform.rotation = Quaternion.Euler(0, smoothAngle, 0);

            Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            if(!_isSprinting)
            {
                _rb.AddForce(moveDirection.normalized * (_moveSpeed * 10), ForceMode.Acceleration);
            }
            else
            {
                _rb.AddForce(moveDirection.normalized * (_moveSpeed * 10 * _sprintMultiplier), ForceMode.Acceleration);
            }

        }

        //If the player is using their weapon, the player must always be facing the camera direction, even when walking backwards
        if (weaponIsFiring)
        {
            targetAngle = _camera.eulerAngles.y;
            transform.rotation = Quaternion.Euler(0, targetAngle, 0);
        }

        _animator.SetBool("Running", hasMoveInput);

        // Apply gravity
        Vector3 gravity = GlobalGravity * _gravityScale * Vector3.up;
        _rb.AddForce(gravity, ForceMode.Acceleration);

        // on Escape always release the cursor so that the user is never locked in the screen

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }

    private void Update()
    {
        _waterSlider.value = _currentHoseCapacity;
        _staminaSlider.value = _stamina;
        _moneyText.text = $"${Money}";


        if (Firing && _currentHoseCapacity > 0)
        {
            _currentHoseCapacity -= Time.deltaTime;
            _hoseParticleEffect.Play();
        }
        else
        {
            _hoseParticleEffect.Stop();
        }

        if(_stamina <= 0 || weaponIsFiring)
        {
            _isSprinting = false;
        }

        _animator.SetBool("Sprinting", _isSprinting);

        if (_isSprinting)
        {
            _stamina -= Time.deltaTime * 10;
        }
        else
        {
            _stamina += Time.deltaTime * 5;
        }

    }

    public void OnResume()
    {
        if (!_gameIsPaused) return;

        _pauseMenu.SetActive(false);
        _gameIsPaused = false;
        Time.timeScale = 1.0f;
    }

    public void regainWater(float refillAmount)
    {
        _currentHoseCapacity += refillAmount;
    }

    public void SetInteractPrompt(bool b)
    {
        _interactPrompt.SetActive(b);
    }

    #region Inputs
    // get the movement input from the PlayerInput in the Settings folder
    // set up for both controller and WASD currently, the sensitivity is not calibrated for controller however
    private void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    private void OnFirePress(InputValue value)
    {
        Firing = true;
    }

    private void OnFireRelease(InputValue value)
    {
        Firing = false;
    }

    private void OnSprintPress(InputValue value)
    {
        if(_stamina > 0)
        {
            _isSprinting = true;
        }
    }

    private void OnSprintRelease(InputValue value)
    {
        _isSprinting = false;
    }

    private void OnInteract(InputValue value)
    {
        // TODO: Interact
    }

    private void OnPause(InputValue value)
    {
        if(_gameIsPaused)
        {
            _gameIsPaused = false;
            _pauseMenu?.SetActive(false);
            Time.timeScale = 1.0f;
        }
        else
        {
            _gameIsPaused = true;
            _pauseMenu?.SetActive(true);
            Time.timeScale = 0f;
        }

    }

    #endregion

}
