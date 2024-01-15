using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] private float _turnSmoothTime = 0.1f;
    float turnSmoothVelocity;



    private bool hasInput => moveInput != Vector2.zero;
    private Rigidbody _rb;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();

        // Just for Testing
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void FixedUpdate()
    {
        if (hasInput)
        {
            // Rotate player model to direction of travel
            float targetAngle = Mathf.Atan2(moveInput.x, moveInput.y) * Mathf.Rad2Deg + _camera.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(_playerModel.transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, _turnSmoothTime);
            transform.rotation = Quaternion.Euler(0, angle, 0);

            // TODO: If the player is shooting their weapon, the player must always be facing the camera direction, even when walking backwards.

            Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            _rb.AddForce(moveDirection.normalized * (_moveSpeed * 10), ForceMode.Acceleration);
        }


    }


    #region Inputs
    private void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    #endregion

}
