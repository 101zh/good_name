using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_animation_controller : MonoBehaviour
{

    private Animator animator;
    private SpriteRenderer sprite;
    private enum animState {witch_idle, witch_walk};
    private string currentState = "";
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
    }

    public void updateAnimation(float dirX, float dirY)
    {
        string state;
        Debug.Log("Updated!");
        if (dirX>0){
            sprite.flipX= false;
            state=nameof(animState.witch_walk);
        } else if (dirX<0){
            sprite.flipX=true;
            state=nameof(animState.witch_walk);
        }
        else if (dirY>0 || dirY<0){
            state=nameof(animState.witch_walk);
        }
        else {
            state=nameof(animState.witch_idle);
        }
        changeAnimationState(state);
    }

    private void changeAnimationState(string newState)
    {
        if (currentState==newState) return;

        animator.Play(newState);

        currentState=newState;
    }
}
