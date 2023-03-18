using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetectionScript : MonoBehaviour
{
    public GameObject crab;
    public Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = crab.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("floorTag"))
        {
            transform.localScale = new Vector2((Mathf.Sign(rb.velocity.x)), transform.localScale.y);
        }
    }
}
