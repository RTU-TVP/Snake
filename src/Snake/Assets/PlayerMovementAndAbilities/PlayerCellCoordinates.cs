using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerCellCoordinates
{
    static Vector2 playerCell;
    public static Vector2 GetPlayerCellCoordinates()
    {
        return playerCell;
    }
    public static void SetPlayerCellCoordinates(Vector2 coordinates)
    {
        playerCell = coordinates;
    }
}
