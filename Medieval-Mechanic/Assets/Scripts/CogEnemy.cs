using UnityEngine;

public class CogEnemy : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private int damageGiven = 1;
    [SerializeField] private float knockbackForce = 200f;
    [SerializeField] private float upwardForce = 100f;

    private int moveDirection = 1;
    private Rigidbody2D rb;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(moveDirection * speed, rb.linearVelocity.y);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        // 🔹 Turn when hitting Player or another Enemy
        if (other.gameObject.CompareTag("Player") ||
            other.gameObject.CompareTag("Enemy"))
        {
            Turn();
        }

        // 🔹 Damage player
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerMovment player = other.gameObject.GetComponent<PlayerMovment>();

            if (player != null)
            {
                player.TakeDamage(damageGiven);

                float knockDir = other.transform.position.x > transform.position.x
                    ? knockbackForce
                    : -knockbackForce;

                player.TakeKnockback(knockDir, upwardForce);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 🔹 Turn when hitting TurnPoint trigger
        if (other.CompareTag("EnemyBlock"))
        {
            Turn();
        }
    }

    private void Turn()
    {
        moveDirection *= -1;

        // Optional: flip sprite visually
        //transform.localScale = new Vector3(moveDirection, 1, 1);
    }

    public void Die()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            player.GetComponent<PlayerMovment>().AddCogKill();
        }

        Destroy(gameObject);
    }
}
