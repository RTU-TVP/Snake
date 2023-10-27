using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public Transform destinationPortal; // ѕортал, в который будет осуществлен переход

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.transform.position = destinationPortal.position; // ѕеремещаем игрока к позиции портала назначени€
        }
    }
}
