using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stone : MonoBehaviour
{
    [SerializeField] float _throwSpeed;
    [HideInInspector]
    public Direction throwDirection;
    Vector2 stonePosition;
    Transform stoneTransform;
    void Start()
    {
        stoneTransform = GetComponent<Transform>();
        StartCoroutine(MaxStoneLifeTime(5));
    }
    void Update()
    {
        StoneMovement();
    }
    void StoneMovement()
    {
        stonePosition = stoneTransform.position;
        if (throwDirection == Direction.up)
        {
            stonePosition.y += _throwSpeed * Time.deltaTime;
        }
        if (throwDirection == Direction.down)
        {
            stonePosition.y -= _throwSpeed * Time.deltaTime;
        }
        if (throwDirection == Direction.left)
        {
            stonePosition.x -= _throwSpeed * Time.deltaTime;
        }
        if (throwDirection == Direction.right)
        {
            stonePosition.x += _throwSpeed * Time.deltaTime;
        }
        stoneTransform.position = stonePosition;
    }
    IEnumerator MaxStoneLifeTime(int time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
        yield break;
    }
}
