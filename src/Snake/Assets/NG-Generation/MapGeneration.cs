using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGeneration : MonoBehaviour
{
    [SerializeField] private float _width;
    [SerializeField] private float _height;

    [SerializeField] private int _minXPosition;
    [SerializeField] private int _maxXPosition;
    [SerializeField] private int _minYPosition;
    [SerializeField] private int _maxYPosition;

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

        for (float x = 0.5f; x < _width; x++)
        {
            if (!_horizontalSpawned)
            {
                float specialObjectXPosition = x;
                float specialObjectYPosition = Random.Range(_minYPosition, _maxYPosition);
                Vector3 specialObjectPosition = new Vector3(Mathf.RoundToInt(specialObjectXPosition), Mathf.RoundToInt(specialObjectYPosition), 0);

                GameObject specialObject = Instantiate(_specialObjectPrefab, specialObjectPosition, Quaternion.identity);
                specialObject.transform.SetParent(transform);
                _horizontalSpawned = true;
            }
            for (float y = 0.5f; y < _height; y++)
            {
                GameObject tile = Instantiate(_tilePrefab, new Vector3(x, y, 1), Quaternion.identity);

                Sprite randomSprite = _sprites[Random.Range(0, _sprites.Length)];
                tile.GetComponent<SpriteRenderer>().sprite = randomSprite;
                tile.transform.SetParent(transform);

                if (!_verticalSpawned)
                {
                    float specialObjectXPosition = Random.Range(_minXPosition, _maxXPosition);
                    float specialObjectYPosition = y;
                    Vector3 specialObjectPosition = new Vector3(Mathf.RoundToInt(specialObjectXPosition), Mathf.RoundToInt(specialObjectYPosition), 0);

                    GameObject specialObject = Instantiate(_specialObjectPrefab, specialObjectPosition, Quaternion.identity);
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