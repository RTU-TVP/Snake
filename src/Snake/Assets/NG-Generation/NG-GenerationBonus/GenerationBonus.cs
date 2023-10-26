using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerationBonus : MonoBehaviour
{
    public GameObject[] _bonusObject;
    private string _groundTag = "Ground";
    [SerializeField] private float _initialDelay;
    [SerializeField] private float _cooldown;

    private Transform[] _spawnPoints;
    private float _timer;
    private bool _canSpawn;

    private void Start()
    {
        _spawnPoints = FindObjectsWithTag(_groundTag);
        _timer = _initialDelay;
        _canSpawn = false;
    }

    private void Update()
    {
        if (!_canSpawn)
        {
            _timer -= Time.deltaTime;
            if (_timer <= 0f)
            {
                _canSpawn = true;
            }
        }

        if (_canSpawn && _timer <= 0f)
        {
            GenerateObject();
            _timer = _cooldown;
            _canSpawn = false;
        }
    }

    private void GenerateObject()
    {
        GameObject _prefab = _bonusObject[Random.Range(0, _bonusObject.Length)];
        Transform _spawnPoint = _spawnPoints[Random.Range(0, _spawnPoints.Length)];

        Instantiate(_prefab, _spawnPoint.position, Quaternion.identity);
    }

    private Transform[] FindObjectsWithTag(string tag)
    {
        GameObject[] taggedObjects = GameObject.FindGameObjectsWithTag(tag);
        Transform[] transforms = new Transform[taggedObjects.Length];
        for (int i = 0; i < taggedObjects.Length; i++)
        {
            transforms[i] = taggedObjects[i].transform;
        }
        return transforms;
    }
}