using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeleteHeart : MonoBehaviour
{
    public void DestroyHeart()
    {
        Destroy(transform.GetChild(0).gameObject);
        if (transform.childCount == 1)
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
}
