using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameModeMenu : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] Button _playButton;
    [SerializeField] Button _chooseButton;
    [SerializeField] Button _exitButton;
    [SerializeField] Button _classicModeButton;
    [SerializeField] Button _infiniteModeButton;
    [SerializeField] GameObject _classicDescription;
    [SerializeField] GameObject _infiniteDescription;
    [SerializeField] GameObject _descriptionBackground;
    Action classicStart;
    Action infiniteStart;

    private void Awake()
    {
        _descriptionBackground.SetActive(false);
        infiniteStart = StartInfiniteMode;
        classicStart = StartClassicMode;
        _classicModeButton.onClick.RemoveAllListeners();
        _infiniteModeButton.onClick.RemoveAllListeners();
        _classicModeButton.onClick.AddListener(classicStart.Invoke);
        _infiniteModeButton.onClick.AddListener(infiniteStart.Invoke);
        _classicDescription.SetActive(false);
        _infiniteDescription.SetActive(false);
    }
    private void Update()
    {
        //if (EventSystem.current.currentSelectedGameObject == _classicModeButton)
        //{
        //    Debug.Log("selected");
        //}
        if (Input.GetMouseButtonUp(0))
        {
            _playButton.interactable = true;
            _chooseButton.interactable = true;
            _exitButton.interactable = true;
            gameObject.SetActive(false);
        }
    }
    public void StartClassicMode()
    {
        Debug.Log("ClassicModeStart");
    }
    public void StartInfiniteMode()
    {
        Debug.Log("InfiniteModeStart");
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        
        foreach (GameObject g in eventData.hovered)
        {
            if (g == _classicModeButton.gameObject)
            {
                _descriptionBackground.SetActive(true);
                _classicDescription.SetActive(true);
                break;
            }
            if (g == _infiniteModeButton.gameObject)
            {
                _descriptionBackground.SetActive(true);
                _infiniteDescription.SetActive(true);
                break;
            }
            _classicDescription.gameObject.SetActive(false);
            _infiniteDescription.gameObject.SetActive(false);
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        _descriptionBackground.SetActive(false);
        _classicDescription.gameObject.SetActive(false);
        _infiniteDescription.gameObject.SetActive(false);
    }

}
