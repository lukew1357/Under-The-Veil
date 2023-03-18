using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class deathParticleScript : MonoBehaviour
{
    private Rigidbody2D rb;
    public EnemyBehaviour enemyBehaviour;
    private float scaleNum;
    private float deathParticleAlpha = 0.0f;
    private SpriteRenderer deathParticleRenderer;

    // Start is called before the first frame update
    void Awake()
    {
        deathParticleRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        scaleNum = Random.Range(0.2f, 0.8f);
        var scaleValue = new Vector3(scaleNum * scaleNum * scaleNum, scaleNum * scaleNum * scaleNum, 1);
        transform.localScale = scaleValue;
        rb.velocity = new Vector2(Random.Range(-5f, 5f), Random.Range(2f, 5f));

        Destroy(gameObject, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        deathParticleAlpha += Time.deltaTime;
        deathParticleRenderer.color = new Color(255, 255, 255, 1 - deathParticleAlpha/0.8f);
    }
}
