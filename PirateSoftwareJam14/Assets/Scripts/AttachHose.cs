using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AttachHose : MonoBehaviour
{
    [SerializeField] private GameObject hoseAttach;
    [SerializeField] private InputActionReference interact;
    private Hose hose;

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag == "Hose")
        {
            Debug.Log("YAY");
            hose = other.gameObject.GetComponentInParent<Hose>();
            interact.action.performed += Attach;
        }
    }


    private void OnTriggerExit(Collider other)
    {

        if (other.gameObject.tag == "Hose")
        {
            interact.action.performed -= Attach;
        }
    }

    private void Attach(InputAction.CallbackContext context)
    {
        hose.Attach(hoseAttach);
    }

}
