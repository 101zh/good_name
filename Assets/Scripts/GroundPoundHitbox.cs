using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class GroundPoundHitbox : MonoBehaviour
{
    Animator animator;
    CapsuleCollider2D hitbox;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        hitbox = GetComponent<CapsuleCollider2D>();
    }

    public void Shockwave()
    {
        gameObject.SetActive(true);
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
        Debug.Log("Stage 1");
    }

    private void ShockwaveStage2Hitbox()
    {
        hitbox.offset = new Vector2(-0.13f, 0.35f);
        hitbox.size = new Vector2(1.9f, 0.7f);
        Debug.Log("Stage 2");
    }

    private void ShockwaveStage3Hitbox()
    {
        hitbox.offset = new Vector2(0, 0.04f);
        hitbox.size = new Vector2(2.84f, 1.4f);
        Debug.Log("Stage 3");
    }

    private void ShockwaveStage4Hitbox()
    {
        hitbox.offset = new Vector2(-0.16f, -0.04f);
        hitbox.size = new Vector2(3.5f, 2f);
        Debug.Log("Stage 4");
    }

    private void ShockwaveStage5Hitbox()
    {
        hitbox.offset = new Vector2(0f, -0.16f);
        hitbox.size = new Vector2(4.16f, 2.4f);
        Debug.Log("Stage 5");
    }

    private void DiableShockwave()
    {
        Debug.Log("Disabled");
        gameObject.SetActive(false);
    }

}
