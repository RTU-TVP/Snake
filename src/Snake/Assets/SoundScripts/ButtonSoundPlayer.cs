using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSoundPlayer : MonoBehaviour
{
    AudioManager audioManager;
    private void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
        audioManager = GetComponent<AudioManager>();
        audioManager.Play($"Button{UnityEngine.Random.Range(1,5)}");
        StartCoroutine(Lifetime(2));
    }
    IEnumerator Lifetime(int time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
        yield break;
    }
}
