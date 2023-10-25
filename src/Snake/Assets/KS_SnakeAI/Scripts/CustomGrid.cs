using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomGrid<TGridObject> 
{
    private int _width;
    private int _height;
    private float _cellSize;
    private Vector2 _originPos;
    private TGridObject[,] _gridArray;
    
    //DEBUG
    //------------------------------
    private bool debugMode = true;
    private TextMesh[,] debugTextAr;
    //------------------------------

    public CustomGrid(int width, int height, float cellSize, Vector2 originPos,
        Func<CustomGrid<TGridObject>, int, int, TGridObject> createGridObject)
    {
        _width = width;
        _height = height; 
        _cellSize = cellSize;
        _originPos = originPos;
        
        _gridArray = new TGridObject[width,height];
        for (int i = 0; i < _gridArray.GetLength(0); i++)
        {
            for (int j = 0; j < _gridArray.GetLength(1); j++)
            {
                _gridArray[i, j] = createGridObject(this, i, j);
            }
        }

        if (debugMode)
        {
            //DEBUG
            debugTextAr = new TextMesh[width,height];
            for (int i = 0; i < _gridArray.GetLength(0); i++)
            {
                for (int j = 0; j < _gridArray.GetLength(1); j++)
                {
                    debugTextAr[i, j] = CreateWText(_gridArray[i, j].ToString(), null, GetWorldPos(i, j) +
                        new Vector2(_cellSize, _cellSize) * 0.5f, 20);
                    Debug.DrawLine(GetWorldPos(i, j), GetWorldPos(i, j + 1), Color.white, 100f);
                    Debug.DrawLine(GetWorldPos(i, j), GetWorldPos(i + 1, j), Color.white, 100f);
                }
            }

            Debug.DrawLine(GetWorldPos(0, _height), GetWorldPos(_width, _height), Color.white, 100f);
            Debug.DrawLine(GetWorldPos(_width, 0), GetWorldPos(_width, _height), Color.white, 100f);
            //------------------------------
        }
    }
    
    
    //DEBUG
    //------------------------------
    private TextMesh CreateWText(string text, Transform parrent = null, Vector3 localPos = default(Vector3),
        int fontSize = 40)
    {
        GameObject gameObject = new GameObject("WText", typeof(TextMesh));
        Transform transform = gameObject.transform;
        transform.SetParent(parrent, false);
        transform.localPosition = localPos;
        TextMesh textMesh = gameObject.GetComponent<TextMesh>();
        textMesh.text = text;
        textMesh.fontSize = fontSize;
        textMesh.anchor = TextAnchor.MiddleCenter;
        return textMesh;
    }
//------------------------------

    private Vector2 GetWorldPos(int x, int y)
    {
        return new Vector2(x, y) * _cellSize + _originPos;
    }

    public void SetValue(int x, int y, TGridObject value)
    {
        if (x >= 0 && x < _width && y < _height && y >= 0)
        {
            _gridArray[x, y] = value;
            //DEBUG
            if (debugMode) debugTextAr[x, y].text = value.ToString();
        }
    }
    public TGridObject GetValue(int x, int y)
    {
        if (x >= 0 && x < _width && y < _height && y >= 0)
        {
            return _gridArray[x, y];
        }

        return  default(TGridObject);
    }

    public int GetWidth()
    {
        return _width;
    }

    public int GetHeight()
    {
        return _height;
    }
}
