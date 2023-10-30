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


    [SerializeField] private Sprite[] _sprites;


    private void Awake()
    {
        GenerateMap();
    }

    public void GenerateMap()
    {
        ClearMap();

        for (float x = 0.5f; x < _width; x++)
        {
            for (float y = 0.5f; y < _height; y++)
            {
                GameObject tile = Instantiate(_tilePrefab, new Vector3(x, y, 1), Quaternion.identity);

                Sprite randomSprite = _sprites[Random.Range(0, _sprites.Length)];
                tile.GetComponent<SpriteRenderer>().sprite = randomSprite;
                tile.transform.SetParent(transform);
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