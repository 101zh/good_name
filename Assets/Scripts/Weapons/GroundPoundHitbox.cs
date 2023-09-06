using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Audio;

public class GroundPoundHitbox : MonoBehaviour
{
    Animator animator;
    CapsuleCollider2D hitbox;
    Health healthScript;
    [SerializeField] float knockbackForce;
    SpriteRenderer spriteRenderer;
    [SerializeField] AudioSource groundPoundSound;
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
        groundPoundSound.Play();
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
        hitbox.offset = new Vector2(0f, 0.5f);
        hitbox.size = new Vector2(3.4f, 1.5f);
    }

    private void ShockwaveStage2Hitbox()
    {
        hitbox.offset = new Vector2(-0.25f, 0.75f);
        hitbox.size = new Vector2(3.75f, 1.5f);
    }

    private void ShockwaveStage3Hitbox()
    {
        hitbox.offset = new Vector2(0, 0.125f);
        hitbox.size = new Vector2(5.75f, 3.25f);
    }

    private void ShockwaveStage4Hitbox()
    {
        hitbox.offset = new Vector2(-0.35f, 0f);
        hitbox.size = new Vector2(7.1f, 4.25f);
    }

    private void ShockwaveStage5Hitbox()
    {
        hitbox.offset = new Vector2(0f, -0.15f);
        hitbox.size = new Vector2(8.25f, 5.25f);
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
    
        if (collider.tag.Equals("Player"))
        {
            healthScript = collider.gameObject.GetComponent<Health>();
            healthScript.OnChangeHealth(1);

            Rigidbody2D playerRb = collider.GetComponent<Rigidbody2D>();
            Vector2 dir = hitbox.transform.position - playerRb.transform.position;
            dir = -dir.normalized;
            StartCoroutine(OverrideMovement(playerRb));
            playerRb.AddForce(dir * knockbackForce, ForceMode2D.Impulse);

        }
    }

    IEnumerator OverrideMovement(Rigidbody2D rb)
    {
        player_controller script = rb.GetComponent<player_controller>();
        script.movmentOverride = true;
        yield return new WaitForSeconds(0.25f);
        script.movmentOverride = false;
    }

}
