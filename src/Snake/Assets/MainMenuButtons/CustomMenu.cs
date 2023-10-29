using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomMenu : MonoBehaviour
{
    [SerializeField] Button _playButton;
    [SerializeField] Button _chooseButton;
    [SerializeField] Button _exitButton;
    [SerializeField] Button _skinAppleButton;
    [SerializeField] Button _skinPineappleButton;
    [SerializeField] Button _skinAvocadoButton;
    [SerializeField] Image _backgroundApple;
    [SerializeField] Image _backgroundPineapple;
    [SerializeField] Image _backgroundAvocado;
    Action appleSkin;
    Action pineappleSkin;
    Action avocadoSkin;
    AudioManager audioManager;
    private void Awake()
    {
        audioManager = GetComponent<AudioManager>();
        appleSkin = SelectApple;
        pineappleSkin = SelectPineapple;
        avocadoSkin = SelectAvocado;
        _skinAppleButton.onClick.RemoveAllListeners();
        _skinPineappleButton.onClick.RemoveAllListeners();
        _skinAvocadoButton.onClick.RemoveAllListeners();
        _skinAppleButton.onClick.AddListener(appleSkin.Invoke);
        _skinPineappleButton.onClick.AddListener(pineappleSkin.Invoke);
        _skinAvocadoButton.onClick.AddListener(avocadoSkin.Invoke);
    }


    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            _playButton.interactable = true;
            _chooseButton.interactable = true;
            _exitButton.interactable = true;
            gameObject.SetActive(false);
        }
    }

    public void SelectApple()
    {
        _backgroundApple.color = new Color32(148, 250, 171, 255);
        _backgroundPineapple.color = new Color32(255, 155, 155, 255);
        _backgroundAvocado.color = new Color32(255, 155, 155, 255);
        SpriteManager._skinNumber = 0;
        audioManager.Play($"Button{UnityEngine.Random.Range(1,5)}");
        Debug.Log("SelectApple");
    }
    public void SelectPineapple()
    {
        _backgroundPineapple.color = new Color32(148, 250, 171, 255);
        _backgroundApple.color = new Color32(255, 155, 155, 255);
        _backgroundAvocado.color = new Color32(255, 155, 155, 255);
        SpriteManager._skinNumber = 1;
        audioManager.Play($"Button{UnityEngine.Random.Range(1, 5)}");
        Debug.Log("SelectPineapple");
    }

    public void SelectAvocado()
    {
        _backgroundAvocado.color = new Color32(148, 250, 171, 255);
        _backgroundPineapple.color = new Color32(255, 155, 155, 255);
        _backgroundApple.color = new Color32(255, 155, 155, 255);
        SpriteManager._skinNumber = 2;
        audioManager.Play($"Button{UnityEngine.Random.Range(1,5)}");
        Debug.Log("SelectAvocado");
    }
}
