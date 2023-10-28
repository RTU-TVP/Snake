using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public Transform _destinationPortal;
    [SerializeField] private float _cooldownDuration;
    private static bool _canTeleport = true;

    [SerializeField] private Sprite _sprite;
    [SerializeField] private Sprite _altSprite;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_canTeleport && collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(TeleportPlayer(collision.gameObject));
        }
    }

    private IEnumerator TeleportPlayer(GameObject player)
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = _altSprite;
        _canTeleport = false;
        gameObject.layer = 11;
        _destinationPortal.gameObject.layer = 11;
        _destinationPortal.gameObject.gameObject.GetComponent<SpriteRenderer>().sprite = _altSprite;
        player.transform.position = _destinationPortal.position + new Vector3(1f, 0f, 0f);
        yield return new WaitForSeconds(_cooldownDuration);
        _canTeleport = true;
        gameObject.layer = 0;
        _destinationPortal.gameObject.layer = 0;
        gameObject.GetComponent<SpriteRenderer>().sprite = _sprite;
        _destinationPortal.gameObject.gameObject.GetComponent<SpriteRenderer>().sprite = _sprite;
    }
}
