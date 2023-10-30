using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BestScoreMainMenu : MonoBehaviour
{
    void Start()
    {
        GetComponent<TextMeshProUGUI>().text = PlayerPrefs.GetInt("bestScore").ToString();
    }

}
