using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGeneration : MonoBehaviour
{
    [SerializeField] private int _width;
    [SerializeField] private int _height;

    [SerializeField] private GameObject _tilePrefab;

    [SerializeField] private Sprite[] _sprites;


    private void Awake()
    {
        GenerateMap();
    }

    public void GenerateMap()
    {
        ClearMap();

        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
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
