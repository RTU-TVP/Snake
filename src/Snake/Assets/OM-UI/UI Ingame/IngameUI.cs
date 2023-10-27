using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IngameUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _score;
    [SerializeField] TextMeshProUGUI _timer;
    [SerializeField] Image _currentAbility;
    [SerializeField] GameObject _IngameUIObject;
    [SerializeField] List<NamedImage> _abilityImageList;
    bool timerActive = true;
    void Start()
    {
        StartCoroutine(Timer());
    }
    void Update()
    {
    }
    public void SetScore(string score)
    {
        if (score != null)
        {
            _score.text = "Ñ÷¸ò: " + score;
        }
    }
    IEnumerator Timer()
    {
        int seconds = 0;
        int minutes = 0;
        while(timerActive)
        {
            seconds++;
            if (seconds > 59)
            {
                minutes++;
                seconds = 0;
            }
            if(seconds <=9)
            {
                _timer.text = minutes.ToString() + ":0" + seconds.ToString();
            }
            else
            {
                _timer.text = minutes.ToString() + ":" + seconds.ToString();
            }
            yield return new WaitForSeconds(1);
        }
        yield break;
    }
    public void StopTimer()
    {
        timerActive = false;
    }
    public void SetAbilityImage(Ability ability)
    {
        foreach (NamedImage abilityImage in _abilityImageList)
        {
            if(abilityImage.nameOfAbility == ability)
            {
                _currentAbility.sprite = abilityImage.sprite;
                break;
            }
        }
    }
    public void IngameUIDisappear()
    {
        _IngameUIObject.SetActive(false);
    }
    public void IngameUIAppear()
    {
        _IngameUIObject.SetActive(true);
    }
}






















[Serializable]
public class NamedImage
{
    public Ability nameOfAbility;
    public Sprite sprite;
}
