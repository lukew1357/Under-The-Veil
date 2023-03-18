using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int speed = 10;
    public GameObject player;
    public Transform playerTransform;

    private void Awake()
    {
        player = GameObject.Find("Player");
        playerTransform = player.GetComponent<Transform>();
        // Find the direction of the mouse from the bullet's position
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 10; // Distance from the camera
        Vector3 direction = Camera.main.ScreenToWorldPoint(mousePos) - playerTransform.position;
        direction.z = 0f; // Keep the bullet in 2D space

        // Rotate the bullet to face the direction it's traveling
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        // Normalize the direction and set the bullet's velocity
        GetComponent<Rigidbody2D>().velocity = direction.normalized * speed;

        // Destroy the bullet after 3 seconds
        Destroy(gameObject, 3f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var enemy = collision.collider.GetComponent<EnemyBehaviour>();
        if (enemy)
        {
            enemy.takeHit(1);
        }

        Debug.Log("Collided");

        // Destroy the bullet when it hits something
        //Destroy(gameObject);
    }
}