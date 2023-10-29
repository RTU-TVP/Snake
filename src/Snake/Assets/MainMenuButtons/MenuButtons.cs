using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuButtons : MonoBehaviour
{
    [SerializeField] Button _playButton;
    [SerializeField] Button _chooseButton;
    [SerializeField] Button _exitButton;
    [SerializeField] GameObject _modeChooseCanvas;
    [SerializeField] GameObject _modeChooseWearCanvas;
    Action play;
    Action choose;
    Action exit;
    AudioManager audioManager;
    private void Awake()
    {
        audioManager = GetComponent<AudioManager>();
        _modeChooseCanvas.SetActive(false);
        _modeChooseWearCanvas.SetActive(false);
        play = PlayButton;
        choose = ChooseButton;
        exit = ExitButton;
        _playButton.onClick.RemoveAllListeners();
        _chooseButton.onClick.RemoveAllListeners();
        _exitButton.onClick.RemoveAllListeners();
        _playButton.onClick.AddListener(play.Invoke);
        _chooseButton.onClick.AddListener(choose.Invoke);
        _exitButton.onClick.AddListener(exit.Invoke);
    }
    public void ExitButton()
    {
        audioManager.Play($"Button{UnityEngine.Random.Range(1,5)}");
        Debug.Log("exit");
        Application.Quit();
    }
    public void PlayButton()
    {
        _modeChooseCanvas.SetActive(true);
        _playButton.interactable = false;
        _chooseButton.interactable = false;
        _exitButton.interactable = false;
        audioManager.Play($"Button{UnityEngine.Random.Range(1, 5)}");
    }

    public void ChooseButton()
    {
        _modeChooseWearCanvas.SetActive(true);
        _playButton.interactable = false;
        _chooseButton.interactable = false;
        _exitButton.interactable = false;
        audioManager.Play($"Button{UnityEngine.Random.Range(1, 5)}");
    }
}
