using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid 
{
    private int _width;
    private int _height;
    private int[,] _gridArray;
    public Grid(int width, int height)
    {
        _width = width;
        _height = height;
        _gridArray = new int[width,height];
    }
}
