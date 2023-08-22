using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class GroundPoundHitbox : MonoBehaviour
{
    Animator animator;
    CapsuleCollider2D hitbox;
    Health healthScript;
    [SerializeField] float knockbackForce;
    SpriteRenderer spriteRenderer;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        hitbox = GetComponent<CapsuleCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        DiableShockwave();

    }

    public void Shockwave()
    {
        EnableShockWave();
        animator.Play("shockwave");
        AdjustShockWaveHitbox(ShockwaveStage1Hitbox, 0f);
        AdjustShockWaveHitbox(ShockwaveStage2Hitbox, 0.1f);
        AdjustShockWaveHitbox(ShockwaveStage3Hitbox, 0.2f);
        AdjustShockWaveHitbox(ShockwaveStage4Hitbox, 0.3f);
        AdjustShockWaveHitbox(ShockwaveStage5Hitbox, 0.4f);
        AdjustShockWaveHitbox(ShockwaveStage3Hitbox, 0.5f);
        AdjustShockWaveHitbox(DiableShockwave, 0.6f);
    }

    private void AdjustShockWaveHitbox(Action func, float delayTime)
    {
        StartCoroutine(AdjustShockWaveHitboxEnum(func, delayTime));
    }

    private IEnumerator AdjustShockWaveHitboxEnum(Action func, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        func();
    }

    private void ShockwaveStage1Hitbox()
    {
        hitbox.offset = new Vector2(0f, 0.2f);
        hitbox.size = new Vector2(1.7f, 0.7f);
    }

    private void ShockwaveStage2Hitbox()
    {
        hitbox.offset = new Vector2(-0.13f, 0.35f);
        hitbox.size = new Vector2(1.9f, 0.7f);
    }

    private void ShockwaveStage3Hitbox()
    {
        hitbox.offset = new Vector2(0, 0.04f);
        hitbox.size = new Vector2(2.84f, 1.4f);
    }

    private void ShockwaveStage4Hitbox()
    {
        hitbox.offset = new Vector2(-0.16f, -0.04f);
        hitbox.size = new Vector2(3.5f, 2f);
    }

    private void ShockwaveStage5Hitbox()
    {
        hitbox.offset = new Vector2(0f, -0.16f);
        hitbox.size = new Vector2(4.16f, 2.4f);
    }

    private void DiableShockwave()
    {
        animator.Play("Null");
        spriteRenderer.enabled=false;
        hitbox.enabled=false;
    }

    void EnableShockWave()
    {
        spriteRenderer.enabled=true;
        hitbox.enabled=true;
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        Debug.Log("triggered");
    
        if (collider.tag.Equals("Player"))
        {
            healthScript = collider.gameObject.GetComponent<Health>();
            healthScript.OnChangeHealth(1);
            Debug.Log("I've been hit!");

            Rigidbody2D playerRb = collider.GetComponent<Rigidbody2D>();
            Vector2 dir = hitbox.transform.position - playerRb.transform.position;
            dir = -dir.normalized;
            StartCoroutine(OverrideMovement(playerRb));
            playerRb.AddForce(dir * knockbackForce, ForceMode2D.Impulse);
            Debug.Log("Made it to end");

        }
    }

    IEnumerator OverrideMovement(Rigidbody2D rb)
    {
        player_controller script = rb.GetComponent<player_controller>();
        script.movmentOverride = true;
        Debug.Log("Överride");
        yield return new WaitForSeconds(0.25f);
        script.movmentOverride = false;
        Debug.Log("Returned perms");
    }

}
