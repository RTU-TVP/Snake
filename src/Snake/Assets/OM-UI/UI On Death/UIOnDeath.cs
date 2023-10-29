using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIOnDeath : MonoBehaviour
{
    [SerializeField] Button _restartButton;
    [SerializeField] Button _menuButton;
    [SerializeField] TextMeshProUGUI _score;
    [SerializeField] TextMeshProUGUI _timer;
    [SerializeField] GameObject _onDeathMenu;
    IngameUI ingameMenu;
    Action restart;
    Action menu;
    AudioManager audioManager;
    void Awake()
    {
        ingameMenu = GameObject.FindObjectOfType<IngameUI>();
        SetTimer();
        SetActionToRestart(RestartLevel);
        SetActionToMenu(GoToMenu);
        audioManager = GameObject.FindObjectOfType<Camera>().GetComponent<AudioManager>();
        audioManager.SoftStop("SceneTheme");
    }
    void Update()
    {
    }
    void SetActionToRestart(Action action)
    {
        _restartButton.onClick.RemoveAllListeners();
        if(action != null)
        {
            _restartButton.onClick.AddListener(action.Invoke);
        }
    }
    void SetActionToMenu(Action action)
    {
        _menuButton.onClick.RemoveAllListeners();
        if(action != null)
        {
            _menuButton.onClick.AddListener(action.Invoke);
        }
    }
    public void SetScore(string score)
    {
        if(score != null)
        {
            _score.text = "Ваш счёт: " + score;
        }
    }
    public void SetTimer()
    {
        _timer.text = "Ваше время: " + ingameMenu.GetTimer();
    }
    public void DeathMenuAppear()
    {
        _onDeathMenu.SetActive(true);
    }
    void RestartLevel()
    {
        InstantiateButtonSound();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1f;
    }
    void GoToMenu()
    {
        InstantiateButtonSound();
        SceneManager.LoadScene("MainMenu");
        Time.timeScale = 1f;
    }
    void InstantiateButtonSound()
    {
        Instantiate(Resources.Load("ButtonSoundPlayer"));
    }
}
