using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGeneration : MonoBehaviour
{
    [SerializeField] private int _width;
    [SerializeField] private int _height;

    [SerializeField] private float _minXPosition;
    [SerializeField] private float _maxXPosition;
    [SerializeField] private float _minYPosition;
    [SerializeField] private float _maxYPosition;

    [SerializeField] private GameObject _tilePrefab;
    [SerializeField] private GameObject _specialObjectPrefab;

    [SerializeField] private Sprite[] _sprites;


    private bool _horizontalSpawned = false;
    private bool _verticalSpawned = false;

    private void Awake()
    {
        GenerateMap();
    }

    public void GenerateMap()
    {
        ClearMap();

        for (int x = 0; x < _width; x++)
        {
            if (!_horizontalSpawned)
            {
                GameObject specialObject = Instantiate(_specialObjectPrefab, new Vector3(x, Random.Range(_minYPosition, _maxYPosition), 0), Quaternion.identity);
                specialObject.transform.SetParent(transform);
                _horizontalSpawned = true;
            }
            for (int y = 0; y < _height; y++)
            {
                GameObject tile = Instantiate(_tilePrefab, new Vector3(x, y, 0), Quaternion.identity);

                Sprite randomSprite = _sprites[Random.Range(0, _sprites.Length)];
                tile.GetComponent<SpriteRenderer>().sprite = randomSprite;
                tile.transform.SetParent(transform);

                if (!_verticalSpawned)
                {
                    GameObject specialObject = Instantiate(_specialObjectPrefab, new Vector3(Random.Range(_minXPosition, _maxXPosition), y, 0), Quaternion.identity);
                    specialObject.transform.SetParent(transform);
                    _verticalSpawned = true;
                }
            }
        }

    }


    private void ClearMap()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }

}
