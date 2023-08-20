using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundPoundHitbox : MonoBehaviour
{
    Animator animator;
    CapsuleCollider2D collider;
    private void Start()
    {
        animator = GetComponent<Animator>();
        collider = GetComponent<CapsuleCollider2D>();
    }

    public void Shockwave()
    {
        gameObject.SetActive(true);
        animator.Play("shockwave");
        Invoke("ShockWaveStage1Hitbox", 0f);
        Invoke("ShockWaveStage2Hitbox", 0.1f);
        Invoke("ShockWaveStage3Hitbox", 0.2f);
        Invoke("ShockWaveStage4Hitbox", 0.3f);
        Invoke("ShockWaveStage5Hitbox", 0.4f);
        Invoke("ShockWaveStage3Hitbox", 0.5f);
    }

    private void ShockwaveStage1Hitbox()
    {
        collider.offset.Set(0f, 0.2f);
        collider.size.Set(1.7f, 0.7f);
    }

    private void ShockwaveStage2Hitbox()
    {
        collider.offset.Set(-0.13f, 0.35f);
        collider.size.Set(1.9f, 0.7f);
    }

    private void ShockwaveStage3Hitbox()
    {
        collider.offset.Set(0, 0.04f);
        collider.size.Set(2.84f, 1.4f);
    }

    private void ShockwaveStage4Hitbox()
    {
        collider.offset.Set(-0.16f, -0.04f);
        collider.size.Set(3.5f, 2f);
    }

    private void ShockwaveStage5Hitbox()
    {
        collider.offset.Set(0f, -0.16f);
        collider.size.Set(4.16f, 2.4f);
    }

}
