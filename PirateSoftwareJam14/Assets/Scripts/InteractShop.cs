using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractShop : MonoBehaviour
{

    [SerializeField] private InputActionReference interact;

    [SerializeField] private Shop shop;

    [SerializeField] private GameObject[] TurnOns;

    private bool flipFlop = true;

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag == "Hose")
        {
            interact.action.performed += OpenShop;

            PlayerController.PlayerInstance.SetInteractPrompt(true);
        }
    }


    private void OnTriggerExit(Collider other)
    {

        if (other.gameObject.tag == "Hose")
        {
            interact.action.performed -= OpenShop;

            PlayerController.PlayerInstance.SetInteractPrompt(false);
        }
    }

    private void OpenShop(InputAction.CallbackContext context)
    {
        OpenShop();
    }
    public void OpenShop()
    {
        if (flipFlop)
        {
            foreach (var t in TurnOns) { t.SetActive(true); }
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            Time.timeScale = 0;
        }
        else
        {
            foreach (var t in TurnOns) { t.SetActive(false); }
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            Time.timeScale = 1;
        }
        flipFlop = !flipFlop;
    }
}
