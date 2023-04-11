using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostController : MonoBehaviour
{
    public float speed = 2.0f;
    public GameObject player;
    public bool isChasingPlayer = false;

    private Rigidbody2D rb2d;
    private Vector2 randomDirection;


    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        rb2d = GetComponent<Rigidbody2D>();
        SetRandomDirection();

    }

    void Update()
    {
        if (isChasingPlayer)
        {
            ChasePlayer();
        }
        else
        {
            MoveRandomly();
        }
    }

    void SetRandomDirection()
    {
        randomDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
    }

    void ChasePlayer()
    {
        Vector2 targetPosition = player.gameObject.transform.position;
        Vector2 ghostPosition = transform.position;
        Vector2 direction = (targetPosition - ghostPosition).normalized;
        transform.position = ghostPosition + direction * speed * Time.deltaTime;
    }

    void MoveRandomly()
    {
        rb2d.velocity = randomDirection * speed;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // Oyuncu hayalet tarafından yakalandı, oyunu kaybettiniz
            Debug.Log("Oyuncu hayalet tarafından yakalandı, oyunu kaybettiniz.");
            GameManager.instance.GameOver();
        }
        else if (other.gameObject.CompareTag("Wall"))
        {
            // Duvarla çarpışıldığında hareketi durdur
            rb2d.velocity = Vector2.zero;
            SetRandomDirection();
        }


    }
}

