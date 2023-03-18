using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTitleScreen : MonoBehaviour
{
    public GameObject player;

    public float enemyJumpForce = 6.0f;
    public float enemyHomingForce = 1.0f;
    public float jumpCoolDown = 2.0f;
    public float verticalDelta = 2.0f;

    public float Hitpoints;
    public float MaxHitpoints = 50;
    public HealthBarBehaviour Healthbar;

    [SerializeField] float moveSpeed = 1.0f;
    [SerializeField] private Animator myAnimationController;

    private Rigidbody2D rb;
    private bool isJumping = false;
    private bool isInRange = false;
    private float nextJumpTime = 0.0f;

    public SpriteRenderer spriteRenderer;

    private float alphaFadeTimer = 0.0f;
    private float inverseFadeTimer;

    private bool fadeAlpha = false;

    private float bulletDamage;

    public GameObject deathParticle;

    public bool hasLife;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.Find("Player");
        Hitpoints = MaxHitpoints;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (transform.position.x > 5)
        {
            Hitpoints = 1;
            takeHit(1);
            Debug.Log("Die");
        }
        if (!isJumping)
        {
            EnemyMove();
        }
        if (isInRange)
        {
            PlayerInRange();
        }
        if (Input.GetKeyDown(KeyCode.KeypadMinus))
        {
            takeHit(25);
        }
        if (fadeAlpha)
        {
            FadeToHealth();
        }
    }

    private bool IsFacingRight()
    {
        return transform.localScale.x > Mathf.Epsilon;
    }

    private void EnemyMove()
    {
        if (IsFacingRight())
        {
            rb.velocity = new Vector2(-moveSpeed, 0.0f);
        }
        else
        {
            rb.velocity = new Vector2(moveSpeed, 0.0f);
        }
    }

    private void JumpAtPlayer()
    {
        myAnimationController.SetBool("coolDownParameter", false);
        Debug.Log("coolDown is false");

        myAnimationController.SetBool("pauseParameter", true);
        Debug.Log("pause is true");

        Vector3 dir = (this.transform.position - player.transform.position).normalized;
        rb.AddForce(new Vector2((-dir.x * enemyJumpForce) / 2, (Mathf.Abs(dir.y) * enemyJumpForce) + verticalDelta), ForceMode2D.Impulse);
    }

    private void MoveToPlayer()
    {
        Vector3 dir = (this.transform.position - player.transform.position).normalized;
        rb.AddForce(new Vector2(-dir.x * enemyHomingForce * 2, Mathf.Abs(dir.y) * enemyHomingForce), ForceMode2D.Impulse);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //myAnimationController.SetBool("coolDownParameter", true);
            //myAnimationController.SetBool("pauseParameter", false);

            //isJumping = false;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isInRange = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("bullet"))
        {
            bulletDamage = 25;
            takeHit(bulletDamage);
        }
    }

    private void PlayerInRange()
    {
        //MoveToPlayer();
        if (Time.time > nextJumpTime)
        {
            myAnimationController.SetBool("coolDownParameter", true);
            myAnimationController.SetBool("pauseParameter", false);

            nextJumpTime = Time.time + jumpCoolDown;
            isJumping = true;
            JumpAtPlayer();
        }
    }

    public void takeHit(float damage)
    {
        Hitpoints -= damage;
        fadeAlpha = true;
        hasLife = true;
        Instantiate(deathParticle, transform.position, Quaternion.identity);
        Instantiate(deathParticle, transform.position, Quaternion.identity);
        Instantiate(deathParticle, transform.position, Quaternion.identity);
        Instantiate(deathParticle, transform.position, Quaternion.identity);

        if (Hitpoints <= 0)
        {
            hasLife = false;
            Destroy(gameObject);
            Instantiate(deathParticle, transform.position, Quaternion.identity);
            Instantiate(deathParticle, transform.position, Quaternion.identity);
            Instantiate(deathParticle, transform.position, Quaternion.identity);
            Instantiate(deathParticle, transform.position, Quaternion.identity);
            Instantiate(deathParticle, transform.position, Quaternion.identity);
            Instantiate(deathParticle, transform.position, Quaternion.identity);
            Instantiate(deathParticle, transform.position, Quaternion.identity);
            Instantiate(deathParticle, transform.position, Quaternion.identity);
            Instantiate(deathParticle, transform.position, Quaternion.identity);
            Instantiate(deathParticle, transform.position, Quaternion.identity);
            Instantiate(deathParticle, transform.position, Quaternion.identity);
            Instantiate(deathParticle, transform.position, Quaternion.identity);
            Instantiate(deathParticle, transform.position, Quaternion.identity);
            Instantiate(deathParticle, transform.position, Quaternion.identity);
            Instantiate(deathParticle, transform.position, Quaternion.identity);
        }
    }

    public void FadeToHealth()
    {
        if (alphaFadeTimer <= 200)
        {
            alphaFadeTimer++;
            inverseFadeTimer = 200 - alphaFadeTimer;
            spriteRenderer.color = new Color((Hitpoints / MaxHitpoints) + inverseFadeTimer / (200 * MaxHitpoints), (Hitpoints / MaxHitpoints) + inverseFadeTimer / (200 * MaxHitpoints), (Hitpoints / MaxHitpoints) + inverseFadeTimer / (200 * MaxHitpoints), 1);
            Debug.Log(inverseFadeTimer/(200*MaxHitpoints));
        }
        if (alphaFadeTimer == 200)
        {
            fadeAlpha = false;
            alphaFadeTimer = 0;
        }
        
    }
}
