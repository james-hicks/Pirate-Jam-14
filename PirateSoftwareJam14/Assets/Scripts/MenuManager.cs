using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject SettingsMenu;
    [SerializeField] private GameObject Menu;
    [SerializeField] private GameObject HTPMenu;
    public void OnPlay()
    {
        SceneManager.LoadScene(1);
    }

    public void OnSettings()
    {
        throw new NotImplementedException();
    }

    public void OnResume()
    {
        PlayerController.PlayerInstance.OnResume();
    }

    public void OnHTP()
    {
        Menu.SetActive(false);
        HTPMenu.SetActive(true);
    }

    public void OnQuit()
    {
        Debug.LogWarning("Quitting Game...");
        Application.Quit();
    }

    public void BackToBaseMenu()
    {
        Menu.SetActive(true);
        HTPMenu.SetActive(false);
    }

    public void GoToMenu()
    {
        SceneManager.LoadScene(0);
    }
}
