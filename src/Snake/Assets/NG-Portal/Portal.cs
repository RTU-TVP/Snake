using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    AudioManager audioManager;
    public Transform _destinationPortal;
    [SerializeField] private float _cooldownDuration;
    private static bool _canTeleport = true;

    [SerializeField] private Sprite _sprite;
    [SerializeField] private Sprite _altSprite;

    private void Start()
    {
        audioManager = GetComponent<AudioManager>();
        _canTeleport = true;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_canTeleport && collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(TeleportPlayer(collision.gameObject));
        }
    }

    private IEnumerator TeleportPlayer(GameObject player)
    {
        audioManager.Play("teleportation");
        gameObject.GetComponent<SpriteRenderer>().sprite = _altSprite;
        _canTeleport = false;
        gameObject.layer = 11;
        _destinationPortal.gameObject.layer = 11;
        _destinationPortal.gameObject.gameObject.GetComponent<SpriteRenderer>().sprite = _altSprite;
        player.transform.position = _destinationPortal.position;
        yield return new WaitForSeconds(_cooldownDuration);
        _canTeleport = true;
        audioManager.Play("active");
        gameObject.layer = 0;
        _destinationPortal.gameObject.layer = 0;
        gameObject.GetComponent<SpriteRenderer>().sprite = _sprite;
        _destinationPortal.gameObject.gameObject.GetComponent<SpriteRenderer>().sprite = _sprite;
    }
}
