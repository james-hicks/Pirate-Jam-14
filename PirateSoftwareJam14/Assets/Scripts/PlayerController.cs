using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement Variables")]
    [SerializeField] private float _moveSpeed = 5f;
    public Vector2 moveInput;
    [SerializeField] private Transform _camera;

    [Header("Visuals Variables")]
    [SerializeField] private GameObject _playerModel;
    [SerializeField] private float _turnSmoothTime = 0.1f; // turn smooth time set up for 
    float turnSmoothVelocity; // ref float for SmoothDampAngle Function

    private bool hasInput => moveInput != Vector2.zero; // simple bool set up to check if the user is giving a movement input
    private Rigidbody _rb;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();

        // lock the cursor on start for testing
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void FixedUpdate()
    {
        if (hasInput)
        {
            // Rotate player model to direction of travel
            float targetAngle = Mathf.Atan2(moveInput.x, moveInput.y) * Mathf.Rad2Deg + _camera.eulerAngles.y;
            float smoothAngle = Mathf.SmoothDampAngle(_playerModel.transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, _turnSmoothTime);
            transform.rotation = Quaternion.Euler(0, smoothAngle, 0);

            // TODO: If the player is using their weapon, the player must always be facing the camera direction, even when walking backwards

            Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            _rb.AddForce(moveDirection.normalized * (_moveSpeed * 10), ForceMode.Acceleration);
        }

        // on Escape always release the cursor so that the user is never locked in the screen

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }


    #region Inputs
    // get the movement input from the PlayerInput in the Settings folder
    // set up for both controller and WASD currently, the sensitivity is not calibrated for controller however
    private void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    #endregion

}
