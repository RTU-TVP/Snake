using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WinnerCanvas : MonoBehaviour
{
    [SerializeField] Button _mainMenuButton;
    Action home;

    private void Awake()
    {
        home = toMenu;
        _mainMenuButton.onClick.RemoveAllListeners();
        _mainMenuButton.onClick.AddListener(home.Invoke);
    }

    private void toMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
