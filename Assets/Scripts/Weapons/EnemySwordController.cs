using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySwordController : MonoBehaviour
{

    private SpriteRenderer sprite;
    private Animator animator;
    [SerializeField] private int swingsPerClick;
    [SerializeField] private float swingDelay; // delay between swings of one click
    [SerializeField] private float swingTime; // amount of time sword is swinging
    [SerializeField] private float coolDown; //after each click
    [SerializeField] private bool coolDownLock;
    [SerializeField] private string idleAnimName = "blood_blade_idle";
    [SerializeField] private string swingAnimName = "blooad_blade_swing";
    public bool passive = false;
    float angle;
    private float coolDownTimer;
    Transform hitBox;
    enemy_controller enemyControllerScript;
    [SerializeField] Transform parent;
    private void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        hitBox = transform.GetChild(0);
        animator = GetComponent<Animator>();
        enemyControllerScript = GetComponentInParent<enemy_controller>();
        parent = transform.parent;
    }

    // Update is called once per frame
    private void Update()
    {
        if (pause_menu.gameIsPaused) return;

        if (coolDownTimer > 0 && !coolDownLock) { coolDownTimer = Mathf.Max(coolDownTimer - Time.deltaTime, 0f); }
        if (!passive)
        {
            Rotate();
            if (coolDownTimer == 0 && enemyControllerScript.currentMovementState == enemy_controller.movementState.safe) //checks if player has pressed the shoot button
            {
                StartCoroutine(Swing());
                coolDownTimer = coolDown;
            }
        }
    }
    private void Rotate()
    {
        
        Vector2 dir = enemyControllerScript.player.transform.position - transform.position;
        // Finding the angle to rotate using math
        angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        // Rotates the sword using math
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        if (angle <= -90 || angle >= 90)
        {
            sprite.flipY = true;
            transform.position = new Vector2(parent.position.x - 0.3f, parent.position.y - 0.55f);
        }
        else
        {
            sprite.flipY = false;
            transform.position = new Vector2(parent.position.x + 0.3f, parent.position.y - 0.55f);
        }
    }

    IEnumerator Swing()
    {
        coolDownLock=true;
        for (int i = 0; i < swingsPerClick; i++)
        {
            hitBox.gameObject.SetActive(true);
            animator.Play(swingAnimName);
            yield return new WaitForSeconds(swingTime);
            animator.Play(idleAnimName);
            hitBox.gameObject.SetActive(false);
            yield return new WaitForSeconds(swingDelay);
        }
        coolDownLock=false;
    }

}
