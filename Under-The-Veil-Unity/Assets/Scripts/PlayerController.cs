using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public float speed = 10.0f;
    public float jumpForce = 10.0f;

    public float dashSpeed;
    public float dashTime;
    public float startDashTime;
    private int dashState = 0;
    private float dashDirection;
    private float dashCooldown = 3.0f;
    private float dashCooldownCounter = 0.0f;

    private bool dashReady = true;
    public Image dashImage;
    public TextMeshProUGUI dashText;
    public GameObject dashUI;

    public int maxJumps = 2;

    private int jumpCount = 0;

    public BoxCollider2D feetCollider;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Animator animator;

    public int playerLives = 3;
    public bool canGainLife = false;
    public bool alive = true;
    private bool canDamage = true;
    public float invunerabilityTime = 1f;
    public float nextDamageTime = 0.0f;

    public Image deathScreen;
    public Image deathMessage;
    private float deathScreenAlpha = 1.0f;
    private float deathMessageAlpha = 1.0f;

    private float blackScreenTime = 3.0f;

    private bool deathJump = true;

    public float currentBulletDamage = 25;

    public SpriteRenderer playerSpriteRenderer;
    public GameObject laserGun;
    public GameObject redLaser;
    public Sprite chipNoArm;
    public Sprite chipArm;

    public bool gunEquipped = false;

    public Button respawnButton;

    private float flashTimer;

    public GameObject aimObject;

    public Image UILaserGun;

    public GameObject shield;

    //private float flashStarter;

    public GameObject UILife1;
    public GameObject UILife2;
    public GameObject UILife3;

    //items and abilities unlocked
    public bool laserGunUnlocked = true;
    public bool dashUnlocked = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (playerLives > 3)
        {
            playerLives = 3;
        }
        gunEquipped = false;
        respawnButton.interactable = false;
        livesUI();
        playerSpriteRenderer = GetComponent<SpriteRenderer>();
        aimObject.SetActive(false);
        animator.SetBool("gunEquipped", false);
        UILaserGun.enabled = false;
        shield.SetActive(false);
    }

    void Update()
    {
        if (alive)
        {
            if (Input.GetKeyDown(KeyCode.Keypad8))
            {
                dashUnlocked = true;
            }
            if (Input.GetKeyDown(KeyCode.Keypad7))
            {
                dashUnlocked = false;
            }
            if (Input.GetKeyDown(KeyCode.KeypadPeriod))
            {
                alive = false;
            }
            if (Input.GetKeyDown(KeyCode.KeypadMultiply))
            {
                GainLife();
            }

            if (!canDamage)
            {
                flashTimer += Time.deltaTime;
                if (flashTimer < .2f)
                {
                    shield.SetActive(true);
                    redLaser.SetActive(true);
                }
                else if (flashTimer >= .2f && flashTimer < .4f)
                {
                    redLaser.SetActive(false);
                }
                else if (flashTimer >= .4f && flashTimer < .6f)
                {
                    redLaser.SetActive(true);
                }
                else if (flashTimer >= .6f && flashTimer < .8f)
                {
                    redLaser.SetActive(false);
                }
                else if (flashTimer >= .8f && flashTimer < 1f)
                {
                    redLaser.SetActive(true);
                }
                else if (flashTimer >= 1f && flashTimer < 1.2f)
                {
                    redLaser.SetActive(false);
                }
                else if (flashTimer >= 1.2f && flashTimer < 1.4f)
                {
                    redLaser.SetActive(true);
                }
                else if (flashTimer >= 1.4f && flashTimer < 1.6f)
                {
                    redLaser.SetActive(false);
                    shield.SetActive(false);
                }
                else if (flashTimer >= 1.6f && flashTimer < 1.8f)
                {
                    redLaser.SetActive(true);
                    shield.SetActive(true);
                }
                else if (flashTimer >= 1.8f && flashTimer < 2f)
                {
                    redLaser.SetActive(false);
                    shield.SetActive(false);
                }
                else if (flashTimer >= 2f && flashTimer < 2.2f)
                {
                    redLaser.SetActive(true);
                    shield.SetActive(true);
                }
                else if (flashTimer >= 2.2f && flashTimer < 2.6f)
                {
                    redLaser.SetActive(false);
                    shield.SetActive(false);
                }
            }

            PlayerHealth();

            //Horizontal movement
            float horizontal = Input.GetAxis("Horizontal");
            rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);

            if (rb.velocity == Vector2.zero)
            {
                animator.SetBool("isRunning", false);
                if (!gunEquipped)
                {
                    playerSpriteRenderer.sprite = chipNoArm;
                }
                else if (gunEquipped)
                {
                    playerSpriteRenderer.sprite = chipArm;
                }
            }
            else
            {
                animator.SetBool("isRunning", true);
            }
            if (Input.GetKeyDown(KeyCode.Alpha1)&&laserGunUnlocked)
            {
                if (!gunEquipped)
                {
                    gunEquipped = true;
                    playerSpriteRenderer.sprite = chipNoArm;
                    aimObject.SetActive(true);
                    animator.SetBool("gunEquipped", true);
                    Debug.Log("gun equipped");
                    UILaserGun.enabled = true;
                }
                else if (gunEquipped)
                {
                    gunEquipped = false;
                    playerSpriteRenderer.sprite = chipArm;
                    aimObject.SetActive(false);
                    animator.SetBool("gunEquipped", false);
                    Debug.Log("gun dequipped");
                    UILaserGun.enabled = false;
                }
            }

            //Changes character facing direction
            if (!Mathf.Approximately(0, horizontal))
                transform.rotation = horizontal < 0 ? Quaternion.Euler(0, 180, 0) : Quaternion.identity;

            //Infinite Jumps
            //if (Input.GetKeyDown(KeyCode.Space))
            //{
            //    if (IsGrounded())
            //    {
            //        float verticalVelocity = rb.velocity.y;
            //        rb.AddForce(new Vector2(0, jumpForce - verticalVelocity), ForceMode2D.Impulse);
            //    }

            // Jump if the player is on the ground or has jumps left
            if (Input.GetButtonDown("Jump") && (jumpCount < maxJumps || IsGrounded()))
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                jumpCount++;
            }

            dashHandler();
        }
        else
        {
            Die();
        }
    }

    private void dashHandler()
    {
        if (dashUnlocked)
        {
            dashUI.SetActive(true);
        }
        else if (!dashUnlocked)
        {
            dashUI.SetActive(false);
        }
        if (dashUnlocked && dashReady)
        {
            Dash();
            dashImage.color = new Color(1f, 1f, 1f, 1);
            dashText.text = $" ";
        }
        if (!dashReady && dashCooldownCounter < dashCooldown)
        {
            dashCooldownCounter += Time.deltaTime;
            dashImage.color = new Color(0.5f, 0.5f, 0.5f, 1.0f);
            if (dashCooldownCounter > 2)
            {
                dashText.text = $"1";
            }
            else if (dashCooldownCounter > 1 && dashCooldownCounter <= 2)
            {
                dashText.text = $"2";
            }
            else if (dashCooldownCounter > 0 && dashCooldownCounter <= 1)
            {
                dashText.text = $"3";
            }
            else
            {
                dashText.text = $" ";
            }
        }
        else if (!dashReady && dashCooldownCounter >= dashCooldown)
        {
            dashCooldownCounter = 0;
            dashReady = true;
        }
    }

    private void Die()
    {
        animator.SetBool("isRunning", false);
        deathScreenAlpha += Time.deltaTime;
        respawnButton.interactable = true;
        if (deathJump)
        {
            deathJump = false;
            rb.velocity = new Vector3(0.0f, 10.0f, 10.0f);
        }
        if (deathScreenAlpha < blackScreenTime)
        {
            deathMessageAlpha = deathScreenAlpha;
            deathScreen.color = new Color(0, 0, 0, deathScreenAlpha / blackScreenTime);
            transform.localScale = new Vector2((blackScreenTime - deathScreenAlpha) / blackScreenTime, (blackScreenTime - deathScreenAlpha) / blackScreenTime) *((blackScreenTime - deathScreenAlpha) / blackScreenTime) *((blackScreenTime - deathScreenAlpha) / blackScreenTime);
        }
        if (deathScreenAlpha > 3.5)
        {
            deathMessage.color = new Color(255, 255, 255, (deathScreenAlpha - 3.5f) / blackScreenTime);
            respawnButton.image.color = new Color(255, 255, 255, ((deathScreenAlpha - 3.5f) / blackScreenTime));
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Reset jump count when the player lands on the ground
        if (collision.gameObject.CompareTag("floorTag"))
        {
            jumpCount = 0;
        }
        if (collision.gameObject.CompareTag("lifeCube"))
        {
            //GainLife();
        }
        if (collision.gameObject.CompareTag("Enemy"))
        {
            LoseLife();
        }
        if (collision.gameObject.CompareTag("deathBox"))
        {
            alive = false;
        }
    }

    bool IsGrounded()
    {
        // Check if the player is grounded by checking for collisions with the feet collider
        ContactFilter2D filter = new ContactFilter2D();
        filter.SetLayerMask(LayerMask.GetMask("floorTag"));
        BoxCollider2D[] hits = new BoxCollider2D[1];
        int numHits = feetCollider.OverlapCollider(filter, hits);
        return numHits > 0;
    }
    /// <summary>
    private void Dash()
    {
        if (dashState == 0)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                Vector2 direction = transform.right;
                direction.Normalize();
                dashDirection = direction.x;
                dashState = 1;
            }
        }
        else
        {
            if (dashTime <= 0)
            {
                dashState = 0;
                dashTime = startDashTime;
                rb.velocity = new Vector2(0.0f, rb.velocity.y);
                dashReady = false;
            }
            else
            {
                dashTime -= Time.deltaTime;
                if (dashState == 1)
                {
                    rb.velocity = new Vector2(dashDirection, 0.1f) * dashSpeed;
                }
            }
        }
    }
    /// </summary>

    public void PlayerHealth()
    {
        if (playerLives == 3)
        {
            canGainLife = false;
        }
        if (playerLives == 0)
        {
            alive = false;
        }
        if (!canDamage && Time.time > nextDamageTime)
        {
            //nextDamageTime = Time.time + invunerabilityTime;
            canDamage = true;
            flashTimer = 0f;
            redLaser.SetActive(false);
        }
    }

    public void LoseLife()
    {
        if (canDamage)
        {
            nextDamageTime = Time.time + invunerabilityTime;
            playerLives -= 1;
            flashTimer = 0f;
            canDamage = false;
            livesUI();
            rb.AddForce(new Vector3(0, jumpForce, 0), ForceMode2D.Impulse);
        }
    }

    public void GainLife()
    {
        if (playerLives < 3)
        {
            playerLives += 1;
            livesUI();
        }
    }

    private void livesUI()
    {
        if (playerLives >= 3)
        {
            UILife1.SetActive(true);
            UILife2.SetActive(true);
            UILife3.SetActive(true);
        }
        else if (playerLives >= 2)
        {
            UILife1.SetActive(true);
            UILife2.SetActive(true);
            UILife3.SetActive(false);
        }
        else if (playerLives >= 1)
        {
            UILife1.SetActive(true);
            UILife2.SetActive(false);
            UILife3.SetActive(false);
        }
        else if (playerLives <= 0)
        {
            UILife1.SetActive(false);
            UILife2.SetActive(false);
            UILife3.SetActive(false);
        }
    }
}
