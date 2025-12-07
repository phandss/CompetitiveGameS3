using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    //player stats
    public float moveSpeed = 5f;
    private float playerHealthValue = 100f;
    public float playerHealth
    {
        set
        {
            playerHealthValue = value;

            if (playerHealthValue <= 0)
            {
                RespawnPlayer();
            }
        }
        get
        {
            return playerHealthValue;
        }

    }


    public ContactFilter2D movementFilter;
    public float collisionOffset = 0.05f;
    private Vector2 lastMoveDirection;
    public MeleeAttack meleeAttack;
    public int AbilityNumber;


    // Abilities
    public FireBallAbility fireBallAbility;
    private bool canUseAbility = true;
    private Vector2 lastMousePos;

    //movement ui and anims

    Vector2 movementInput;
    Rigidbody2D rb;
    public Animator anim;
    UIManager uiManager;

    //game manager
    public GameObject respawnPos;

    List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();
    private bool canMove;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        uiManager = FindAnyObjectByType<UIManager>();
        canMove = true;
    }

    private void Update()
    {
        UpdateAnims();
        lastMousePos = GetMousePos();
    }

    private void FixedUpdate()
    {
        if (canMove)
        {
            HandleMovement();

        }

    }

    private void HandleMovement()
    {
        if (movementInput != Vector2.zero)
        {

            bool canMove = TryMove(movementInput);

            if (!canMove)
            {
                canMove = TryMove(new Vector2(movementInput.x, 0));
                if (!canMove)
                {
                    canMove = TryMove(new Vector2(0, movementInput.y));
                }
            }

        }
    }

    void OnMove(InputValue movementValue)
    {
        movementInput = movementValue.Get<Vector2>();

        if (movementInput.x == 0 && movementInput.y == 0)
        {
            lastMoveDirection = movementInput;
        }

    }


    private bool TryMove(Vector2 direction)
    {
        if (direction != Vector2.zero)
        {
            int count = rb.Cast(direction, movementFilter, castCollisions, moveSpeed * Time.fixedDeltaTime + collisionOffset);
            if (count == 0)
            {
                rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    public void RespawnPlayer()
    {


        GameManager.instance.LoseLife();
        playerHealth = 100;
        uiManager.ChangeHealthText(playerHealth);


    }

    void OnAbility1(InputValue value)
    {
        if (value != null && canUseAbility)
        {
            Debug.Log("Ability 1 Activated");
            AbilityNumber = 1;

            Vector2 targetPos = GetMousePos();

            fireBallAbility.TryExecuteAbility(transform.position,targetPos);

        }
    }

    public void OnAbility2(InputValue value) {
        if (value != null)
        {
            Debug.Log("Ability 2 Activated");
            AbilityNumber = 2;

        }
    }


    public void OnAbility3(InputValue value) {
        if (value != null)
        {
            Debug.Log("Ability 3 Activated");
            AbilityNumber = 3;
        }
    }

    void UpdateAnims()
    {
        anim.SetFloat("moveX", movementInput.x);
        anim.SetFloat("moveY", movementInput.y);
        anim.SetFloat("moveSpeed", movementInput.sqrMagnitude);
    }

    void OnAttack()
    {
        anim.SetTrigger("MeleeAttack1");
        LockMove();

        if (movementInput.x < 0)
        {
            meleeAttack.AttackLeft();

        }
        else if (movementInput.x > 0)
        {
            meleeAttack.AttackRight();
        }

    }




    public void TakeDamage(float damage)
    {
        playerHealth -= 10;
        uiManager.healthText.text = "Health: " + playerHealth.ToString();

    }
    public void EndAttack()
    {

        UnlockMove();
        meleeAttack.StopAttack();
    }



    public void LockMove()
    {
            canMove = false;
    }

    public void UnlockMove()
    {
            canMove = true;
    }


    private Vector2 GetMousePos()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Mathf.Abs(Camera.main.transform.position.z);
        return Camera.main.ScreenToWorldPoint(mousePos);
    }

}
