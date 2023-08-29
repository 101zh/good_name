using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    Transform rotationCenter;
    float posX, posY, angle = 0f;
    [SerializeField] float rotationRadius = 2f, angularSpeed = 2f;

    float coolDownTimer;
    [SerializeField] float coolDown;
    [SerializeField] bool coolDownLock;
    Transform projectileLauncher;
    BossGunController projectileLauncherScript;

    SpriteRenderer spriteRenderer;

    private void Start()
    {
        projectileLauncher = transform.GetChild(0);
        projectileLauncherScript = projectileLauncher.GetComponent<BossGunController>();

        spriteRenderer = GetComponent<SpriteRenderer>();

        rotationCenter = GameObject.FindGameObjectWithTag("Player").transform;
        coolDownTimer = coolDown;
    }
    void Update()
    {
        if (pause_menu.gameIsPaused) return;
        if (coolDownTimer > 0 && !coolDownLock) { coolDownTimer = Mathf.Max(coolDownTimer - Time.deltaTime, 0f); }
        if (coolDownTimer == 0)
        {
            int rand = Random.Range(1, 11);
            if (rand <= 11)
            {
                StartCoroutine(FireWall());
            }
            else if (rand <= 8)
            {
                EmitFireBalls();
            }
            else
            {
                StartCoroutine(Invisibility());
            }
            coolDownTimer = coolDown;
        }
    }

    void FixedUpdate()
    {
        if (pause_menu.gameIsPaused) return;
        Move();
    }

    private void Move()
    {
        posX = rotationCenter.position.x + Mathf.Cos(angle) * rotationRadius;
        posY = rotationCenter.position.y + Mathf.Sin(angle) * rotationRadius;
        transform.position = new Vector2(posX, posY);
        angle = angle + Time.deltaTime * angularSpeed;

        if (angle >= 360f)
        {
            angle = 0f;
        }
    }

    IEnumerator Invisibility()
    {
        Coroutine _coroutine;
        spriteRenderer.enabled = false;
        coolDownLock = true;
        _coroutine = StartCoroutine(ThrowFireBallsMadly());
        yield return new WaitForSeconds(5f);
        StopCoroutine(_coroutine);
        coolDownLock = false;
        spriteRenderer.enabled = true;
    }

    IEnumerator ThrowFireBallsMadly()
    {
        yield return new WaitForSeconds(.5f);
        projectileLauncherScript.bulletSpeed = 15f;
        while (true)
        {
            ThrowFireBall();
            yield return new WaitForSeconds(.16f);
        }
    }

    [SerializeField] GameObject preFireWall;
    [SerializeField] GameObject fireWall;
    private int fireWallCount;
    private IEnumerator FireWall()
    {
        Vector2 location = rotationCenter.position;
        location.x = 0;
        fireWallCount+=1;
        GameObject preFireWallInstance = Instantiate(preFireWall, location, preFireWall.transform.rotation);
        yield return new WaitForSeconds(1.5f);
        Instantiate(fireWall, location, fireWall.transform.rotation);
        yield return new WaitForSeconds(5.6f);
        fireWallCount-=1;
        Destroy(preFireWallInstance);
    }

    private void ThrowFireBall()
    {
        projectileLauncherScript.shootBulletAtPlayer();
    }

    private void ThrowHomingFireBall()
    {
        projectileLauncherScript.shootHomingBulletAtPlayer();
    }

    private void EmitFireBalls()
    {
        projectileLauncherScript.bulletSpeed = 7f;
        float[] angles = { 0, 15, 30, 45, 60, 75, 90, 105, 120, 135, 150, 165, 180, 195, 210, 225, 240, 255, 270, 285, 300, 315, 330, 345, 360 };
        projectileLauncherScript.shootBulletToAngles(angles);
    }
}
