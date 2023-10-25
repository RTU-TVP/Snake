using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float _basePlayerSpeed;
    [SerializeField] int _speedupBoostTime;
    [SerializeField] float _speedWhenBoosted;
    [SerializeField] int _shieldTime;
    [SerializeField] GameObject _stonePrefab;
    float playerSpeed;
    Direction currentDirection;
    Direction nextDirection;
    Transform playerTransform;
    Vector2 playerPosition;
    Ability currentAbility;
    void Start()
    {
        currentAbility = Ability.no;
        playerSpeed = _basePlayerSpeed;
        playerTransform = GetComponent<Transform>();
    }

    void Update()
    {
        ChooseNextDirection();
        ChangeDirection();
        Movement();
        if(Input.GetKeyDown(KeyCode.Space)) UseAbility(currentAbility);
        Debug.Log(currentAbility);
    }
    void ChooseNextDirection()
    {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (currentDirection != Direction.up) nextDirection = Direction.up;
        }
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (currentDirection != Direction.left) nextDirection = Direction.left;
        }
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (currentDirection != Direction.right) nextDirection = Direction.right;
        }
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (currentDirection != Direction.down) nextDirection = Direction.down;
        }
    }
    void ChangeDirection()
    {
        if(nextDirection != currentDirection)
        {
            if((Mathf.Round(playerTransform.position.x * 10f) / 10f) % 1.00 == 0 && (Mathf.Round(playerTransform.position.y * 10f) / 10f) % 1.00 == 0)
            {
                currentDirection = nextDirection;
            }
        }
    }
    void Movement()
    {
        playerPosition = playerTransform.position;
        if(currentDirection == Direction.up)
        {
            playerPosition.y += playerSpeed * Time.deltaTime;
            playerPosition.x = Mathf.Round(playerPosition.x);
        }
        if (currentDirection == Direction.down)
        {
            playerPosition.y -= playerSpeed * Time.deltaTime;
            playerPosition.x = Mathf.Round(playerPosition.x);
        }
        if (currentDirection == Direction.left)
        {
            playerPosition.x -= playerSpeed * Time.deltaTime;
            playerPosition.y = Mathf.Round(playerPosition.y);
        }
        if (currentDirection == Direction.right)
        {
            playerPosition.x += playerSpeed * Time.deltaTime;
            playerPosition.y = Mathf.Round(playerPosition.y);
        }
        playerTransform.position = playerPosition;
    }
    void UseAbility(Ability thisAbility)
    {
        if(thisAbility == Ability.no)
        {
            return;
        }
        else
        {
            if (thisAbility == Ability.speedBoost)
            {
                StartCoroutine(SpeedUpAbilityTimer(_speedupBoostTime));
            }
            if(thisAbility == Ability.shield)
            {
                StartCoroutine(ShieldAbilityTimer(_shieldTime));
            }
            if(thisAbility == Ability.stone)
            {
                GameObject stone = Instantiate(_stonePrefab,null);
                stone.GetComponent<Transform>().position = playerPosition;
                stone.GetComponent<Stone>().throwDirection = currentDirection;
            }
            SetAbility(Ability.no);
        }
    }
    public void SetAbility(Ability newAbility)
    {
        currentAbility = newAbility;
    }
    IEnumerator SpeedUpAbilityTimer(int time)
    {
        playerSpeed = _speedWhenBoosted;
        yield return new WaitForSeconds(time);
        playerSpeed = _basePlayerSpeed;
        yield break;
    }
    IEnumerator ShieldAbilityTimer(int time)
    {
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        yield return new WaitForSeconds(time);
        gameObject.GetComponent<BoxCollider2D>().enabled = true;
        yield break;
    }
}
public enum Direction
{
    left,
    right,
    up,
    down
}
public enum Ability
{
    no,
    shield,
    speedBoost,
    stone,
    plusPoints
}
