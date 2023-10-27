using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteHeart : MonoBehaviour
{
    private void Start()
    {
        DestroyHeart();
    }
    public void DestroyHeart()
    {
        Destroy(transform.GetChild(0).gameObject);
    }
}
