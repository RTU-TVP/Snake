using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Points
{
    static int points = 0;
    public static int GetPoints()
    {
        return points;
    }
    public static void AddPoints(int amount)
    {
        points += amount;
    }
    public static void SetPoints(int amount)
    {
        points = amount;
    }
    public static void SaveNewBest()
    {
        PlayerPrefs.SetInt("bestScore",GetPoints());
    }
}
