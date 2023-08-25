using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_controller : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody2D rb;
    [SerializeField] private float moveSpeed;
    private Animator animator;
    private SpriteRenderer sprite;
    private enum animState { witch_idle, witch_walk };
    private string currentState = "";
    [SerializeField] private HUD_bar healthBar;
    [SerializeField] private HUD_bar defenseBar;
    private Health health;
    public int coins;
    coinText coinTextScript;
    public bool movmentOverride = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        health = GetComponent<Health>();
        coinTextScript = GameObject.FindWithTag("CoinUI").GetComponentInChildren<coinText>();
        // rb.GetComponent<PhysicsMaterial2D>().bounciness=1;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        bool moving = Input.GetButton("Horizontal") || Input.GetButton("Vertical");
        float movementX = Input.GetAxisRaw("Horizontal");
        float movementY = Input.GetAxisRaw("Vertical");
        playerMove(movementX, movementY, moving);
        updateAnimation(movementX, movementY);
    }

    private void playerMove(float movementX, float movementY, bool moving)
    {
        if (movmentOverride)
        {

        }
        else if (moving)
        {
            rb.velocity = new Vector2(movementX, movementY).normalized * moveSpeed;
        }
        else
        {
            rb.velocity = new Vector2(0, 0);
        }
    }

    private void updateAnimation(float dirX, float dirY)
    {
        string state;
        // Debug.Log("Updated!");
        if (dirX > 0)
        {
            sprite.flipX = false;
            state = nameof(animState.witch_walk);
        }
        else if (dirX < 0)
        {
            sprite.flipX = true;
            state = nameof(animState.witch_walk);
        }
        else if (dirY > 0 || dirY < 0)
        {
            state = nameof(animState.witch_walk);
        }
        else
        {
            state = nameof(animState.witch_idle);
        }
        changeAnimationState(state);
    }

    private void changeAnimationState(string newState)
    {
        if (currentState == newState) return;

        animator.Play(newState);

        currentState = newState;
    }

    private void Die()
    {
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        gameObject.GetComponent<player_controller>().enabled = false;
        rb.velocity = new Vector2(0, 0);
    }

    public void incrementCoins(int amount)
    {
        coins += amount;
        coinTextScript.UpdateCoins(coins);
    }

    private void updateHUD()
    {
        healthBar.setValue(health.currentHealth, health.maxHealth);
        defenseBar.setValue(health.currentDefense, health.maxDefense);
    }

    private void OnEnable()
    {
        Health.onHitEvent += updateHUD;
        Health.OnPlayerDie += Die;
    }

    private void OnDisable()
    {
        Health.onHitEvent -= updateHUD;
        Health.OnPlayerDie -= Die;
    }

}





// [fail]: OmniSharp.MSBuild.ProjectManager
// Failure while loading the analyzer reference 'Unity.SourceGenerators': Could not load file or assembly 'netstandard, Version=2.1.0.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51' or one of its dependencies. The system cannot find the file specified.
