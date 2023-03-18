using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lifeBlockCollectable : MonoBehaviour
{
    public GameObject deathParticle;
    public PlayerController playerControllerScript;
    public Collider2D cubeCollider;
    private bool canObtain;
    // Start is called before the first frame update
    private void Start()
    {
        cubeCollider = GetComponent<Collider2D>();
    }
    private void Update()
    {
        if (playerControllerScript.playerLives < 3)
        {
            cubeCollider.enabled = true;
        }
        else if (playerControllerScript.playerLives >= 3)
        {
            cubeCollider.enabled = false;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Reset jump count when the player lands on the ground
        if (collision.gameObject.CompareTag("Player") && playerControllerScript.playerLives < 3)
        {
            Destroy(gameObject, .01f);
            Instantiate(deathParticle, transform.position, Quaternion.identity);
            Instantiate(deathParticle, transform.position, Quaternion.identity);
            Instantiate(deathParticle, transform.position, Quaternion.identity);
            Instantiate(deathParticle, transform.position, Quaternion.identity);
            Instantiate(deathParticle, transform.position, Quaternion.identity);
            Instantiate(deathParticle, transform.position, Quaternion.identity);
            Instantiate(deathParticle, transform.position, Quaternion.identity);
            Instantiate(deathParticle, transform.position, Quaternion.identity);
            Debug.Log("life block collected");
            playerControllerScript.GainLife();
        }
    }
}
