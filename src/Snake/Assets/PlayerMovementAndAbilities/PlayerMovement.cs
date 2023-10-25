using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float _basePlayerSpeed;
    [SerializeField] int _speedupBoostTime;
    [SerializeField] float _speedWhenBoosted;
    [SerializeField] int _shieldTime;
    [SerializeField] GameObject _stonePrefab;
    float realPlayerSpeed;
    float theoreticalPlayerSpeed;
    Direction currentDirection;
    Direction nextDirection;
    Transform playerTransform;
    Vector2 playerPosition;
    Ability currentAbility;
    bool isPlayerInCellCenter;
    Vector2 playerCellCoordinates;
    void Start()
    {
        currentAbility = Ability.no;
        theoreticalPlayerSpeed = _basePlayerSpeed;
        realPlayerSpeed = 0;
        playerTransform = GetComponent<Transform>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<AbilityCheck>(out AbilityCheck abilityCheck))
        {
            if (abilityCheck._thisItem == Ability.speedBoost)
            {
                SetAbility(Ability.speedBoost);
            }
            if (abilityCheck._thisItem == Ability.stone)
            {
                SetAbility(Ability.stone);
            }
            if (abilityCheck._thisItem == Ability.shield)
            {
                SetAbility(Ability.shield);
            }
            if (abilityCheck._thisItem == Ability.plusPoints)
            {
                // ƒобавить к текущему количеству очков столько, сколько надо за предмет с доп очками (ещЄ, вроде, зме€ вырастает)
            }
            Destroy(collision.gameObject);
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.gameObject.TryGetComponent<Barrier>(out Barrier barrier))
        {
            playerPosition.x = (float)Math.Round(playerPosition.x,1);
            playerPosition.y = (float)Math.Round(playerPosition.y,1);
            realPlayerSpeed = 0;
            StartCoroutine(BarrierDisappear(barrier.gameObject));
        }
    }
    void Update()
    {
        isPlayerInCellCenter = ((Mathf.Round((playerTransform.position.x - 0.5f) * 10f) / 10f) % 1.00 == 0 && (Mathf.Round((playerTransform.position.y - 0.5f) * 10f) / 10f) % 1.00 == 0);
        ChooseNextDirection();
        ChangeDirection();
        Movement();
        if(Input.GetKeyDown(KeyCode.Space)) UseAbility(currentAbility);
        if (realPlayerSpeed != 0)
        {
            realPlayerSpeed = theoreticalPlayerSpeed;
        }
        if (isPlayerInCellCenter)
        {
            playerCellCoordinates.x = Convert.ToInt32(playerTransform.position.x - 0.5f);
            playerCellCoordinates.y = Convert.ToInt32(playerTransform.position.y - 0.5f);
        }
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
        if (nextDirection != currentDirection)
        {
            if(isPlayerInCellCenter)
            {
                currentDirection = nextDirection;
            }
            realPlayerSpeed = theoreticalPlayerSpeed;
        }
    }
    void Movement()
    {
        playerPosition = playerTransform.position;
        if(currentDirection == Direction.up)
        {
            playerPosition.y += realPlayerSpeed * Time.deltaTime;
            playerPosition.x = Mathf.Round(playerPosition.x * 10) / 10;
        }
        if (currentDirection == Direction.down)
        {
            playerPosition.y -= realPlayerSpeed * Time.deltaTime;
            playerPosition.x = Mathf.Round(playerPosition.x * 10) / 10;
        }
        if (currentDirection == Direction.left)
        {
            playerPosition.x -= realPlayerSpeed * Time.deltaTime;
            playerPosition.y = Mathf.Round(playerPosition.y * 10) / 10;
        }
        if (currentDirection == Direction.right)
        {
            playerPosition.x += realPlayerSpeed * Time.deltaTime;
            playerPosition.y = Mathf.Round(playerPosition.y * 10) / 10;
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
        theoreticalPlayerSpeed = _speedWhenBoosted;
        yield return new WaitForSeconds(time);
        theoreticalPlayerSpeed = _basePlayerSpeed;
        yield break;
    }
    IEnumerator ShieldAbilityTimer(int time)
    {
        gameObject.layer = 6;
        yield return new WaitForSeconds(time);
        gameObject.layer = 0;
        yield break;
    }
    IEnumerator BarrierDisappear(GameObject go)
    {
        while(go != null)
        {
            yield return null;
        }
        realPlayerSpeed = theoreticalPlayerSpeed;
        yield break;
    }
    public Vector2 SendPlayerCoordinates()
    {
        return playerCellCoordinates;
    }
}
public enum Direction
{
    no,
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
