using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuButtons : MonoBehaviour
{
    [SerializeField] Button _playButton;
    [SerializeField] Button _exitButton;
    [SerializeField] GameObject _modeChooseCanvas;
    Action play;
    Action exit;
    private void Awake()
    {
        _modeChooseCanvas.SetActive(false);
        play = PlayButton;
        exit = ExitButton;
        _playButton.onClick.RemoveAllListeners();
        _exitButton.onClick.RemoveAllListeners();
        _playButton.onClick.AddListener(play.Invoke);
        _exitButton.onClick.AddListener(exit.Invoke);
    }
    public void ExitButton()
    {
        Debug.Log("exit");
        Application.Quit();
    }
    public void PlayButton()
    {
        _modeChooseCanvas.SetActive(true);
        _playButton.interactable = false;
        _exitButton.interactable = false;

    }
}
