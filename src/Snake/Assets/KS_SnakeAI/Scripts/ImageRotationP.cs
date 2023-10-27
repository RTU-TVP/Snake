using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageRotationP : MonoBehaviour
{
    public SnakePart Snake;
    public void UpdateImageRotation()
    {
        var rot =Snake.GetDirection();
        if (rot == Vector2.up) transform.rotation = Quaternion.Euler(0, 0, 0);
        else if (rot == Vector2.down) transform.rotation = Quaternion.Euler(0, 0, 180);
        else if (rot == Vector2.left) transform.rotation = Quaternion.Euler(0, 0, 90);
        else  transform.rotation = Quaternion.Euler(0, 0, 270);
    }
}
