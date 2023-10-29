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
    Animator animator;
    float realPlayerSpeed;
    float theoreticalPlayerSpeed;
    Direction currentDirection;
    Direction nextDirection;
    Transform playerTransform;
    Vector2 playerPosition;
    Ability currentAbility;
    bool isPlayerInCellCenter;
    Vector2 playerCellCoordinates;
    IngameUI ingameUI;
    IEnumerator speedUp;
    IEnumerator shield;
    void Start()
    {
        animator = GetComponent<Animator>();
        ingameUI = GameObject.FindObjectOfType<IngameUI>().GetComponent<IngameUI>();
        SetAbility(Ability.no);
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
                Points.AddPoints(100);
            }
            Destroy(collision.gameObject);
        }
        else
        {
            if(collision.gameObject.layer == 9)
            {
                //lose
                OnLose();
            }
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
        AnimationSpeed();
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
        if(playerCellCoordinates != null)
        {
            PlayerCellCoordinates.SetPlayerCellCoordinates(playerCellCoordinates);
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
        if ((nextDirection != currentDirection) && isPlayerInCellCenter)
        {
            currentDirection = nextDirection;
            realPlayerSpeed = theoreticalPlayerSpeed;
        }
    }
    void Movement()
    {
        playerPosition = playerTransform.position;
        if(currentDirection == Direction.up)
        {
            animator.SetBool("up", true);
            animator.SetBool("down", false);
            animator.SetBool("right", false);
            animator.SetBool("left", false);
            playerPosition.y += realPlayerSpeed * Time.deltaTime;
            playerPosition.x = Mathf.Round(playerPosition.x * 10) / 10;
        }
        if (currentDirection == Direction.down)
        {
            animator.SetBool("up", false);
            animator.SetBool("down", true);
            animator.SetBool("right", false);
            animator.SetBool("left", false);
            playerPosition.y -= realPlayerSpeed * Time.deltaTime;
            playerPosition.x = Mathf.Round(playerPosition.x * 10) / 10;
        }
        if (currentDirection == Direction.left)
        {
            animator.SetBool("up", false);
            animator.SetBool("down", false);
            animator.SetBool("right", false);
            animator.SetBool("left", true);
            playerPosition.x -= realPlayerSpeed * Time.deltaTime;
            playerPosition.y = Mathf.Round(playerPosition.y * 10) / 10;
        }
        if (currentDirection == Direction.right)
        {
            animator.SetBool("up", false);
            animator.SetBool("down", false);
            animator.SetBool("right", true);
            animator.SetBool("left", false);
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
                if(speedUp != null) StopCoroutine(speedUp);
                speedUp = SpeedUpAbilityTimer(_speedupBoostTime);
                StartCoroutine(speedUp);
            }
            if(thisAbility == Ability.shield)
            {
                if(shield != null) StopCoroutine(shield);
                shield = ShieldAbilityTimer(_shieldTime);
                StartCoroutine(shield);
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
        ingameUI.SetAbilityImage(newAbility);

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
        gameObject.layer = 8;
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
    void OnLose()
    {
        GameObject loseMenu = Instantiate(Resources.Load("OnDeathMenu"), null) as GameObject;
        ingameUI.StopTimer();
        loseMenu.GetComponent<UIOnDeath>().SetScore(Points.GetPoints().ToString());
        if (Points.GetPoints() > PlayerPrefs.GetInt("bestScore")) PlayerPrefs.SetInt("bestScore",Points.GetPoints());
        Destroy(gameObject);
        Time.timeScale = 0f;
    }
    void AnimationSpeed()
    {
        animator.speed = realPlayerSpeed / 5;
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
