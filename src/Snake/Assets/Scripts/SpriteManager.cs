using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteManager : MonoBehaviour
{
    public PlayerMovement _player;

    public Sprite[] _skins;
    public RuntimeAnimatorController[] _animators;

    public static int _skinNumber;

    private void Start()
    {
        _player = FindAnyObjectByType<PlayerMovement>();
        _player.GetComponent<SpriteRenderer>().sprite = _skins[_skinNumber];
        _player.GetComponent<Animator>().runtimeAnimatorController = _animators[_skinNumber];
    }
}