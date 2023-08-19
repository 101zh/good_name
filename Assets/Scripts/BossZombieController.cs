using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BossZombieController : MonoBehaviour
{
    
    [SerializeField] private float movementSpeed;
    public GameObject player;
    private Rigidbody2D enemyrb;
    [SerializeField] private float attackRange;
    [SerializeField] private float dangerRange;
    [SerializeField] Vector3 desiredPos;
    public enum movementState { chase, safe, runAway };
    public static movementState currentMovementState;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        enemyrb = GetComponent<Rigidbody2D>();
        desiredPos = transform.position;
        Jump();
    }

    // Update is called once per frame
    private void Jump(){
        Vector3 pos= transform.position;
        Vector3 end = player.transform.position;
        StartCoroutine(5f.Tweeng((p)=> transform.position=p, pos, end));
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.DrawWireSphere(transform.position, dangerRange);
    }
}
