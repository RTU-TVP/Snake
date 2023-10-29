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
    AudioManager audioManager;

    private void Awake()
    {
        audioManager = GetComponent<AudioManager>();
        home = toMenu;
        _mainMenuButton.onClick.RemoveAllListeners();
        _mainMenuButton.onClick.AddListener(home.Invoke);
    }

    void InstantiateButtonSound()
    {
        Instantiate(Resources.Load("ButtonSoundPlayer"));
    }

    private void toMenu()
    {
        InstantiateButtonSound();
        SceneManager.LoadScene("MainMenu");
    }
}
