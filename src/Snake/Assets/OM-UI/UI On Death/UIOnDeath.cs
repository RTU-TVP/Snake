using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIOnDeath : MonoBehaviour
{
    [SerializeField] Button _restartButton;
    [SerializeField] Button _menuButton;
    [SerializeField] TextMeshProUGUI _score;
    [SerializeField] TextMeshProUGUI _deathReason;
    [SerializeField] GameObject _onDeathMenu;
    void Awake()
    {
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
    public void SetDeathReason(string reason)
    {
        if(reason != null)
        {
            _deathReason.text = "Причина смерти: " + reason;
        }
    }
    public void DeathMenuAppear()
    {
        _onDeathMenu.SetActive(true);
    }
}
