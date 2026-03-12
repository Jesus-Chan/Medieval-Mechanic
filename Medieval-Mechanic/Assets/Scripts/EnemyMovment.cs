using UnityEngine;

public class EnemyMovment : MonoBehaviour
{
    [SerializeField] protected float moveSpeed = 2.0f;
    [SerializeField] private float bounciness = 100f;
    [SerializeField] private float knockbackForce = 200f;
    [SerializeField] private float upwardForce = 100f;
    [SerializeField] private int damageGiven = 1;


    [SerializeField] private AudioClip deathSound;
    [SerializeField] private GameObject deathParticles;

    protected bool canMove = true;

    protected SpriteRenderer Rend;

    private AudioSource audioSource;

    private void Start()
    {
        Rend = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();

    }
    void FixedUpdate()
    {
        if (!canMove) 
            return ;
        transform.Translate(new Vector2 (moveSpeed, 0)* Time.deltaTime);
        
        if (moveSpeed > 0)
        {
            Rend.flipX = true;
        }

        if (moveSpeed < 0)
        {
            Rend.flipX = false;
        }

    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("EnemyBlock"))
        {
            moveSpeed = -moveSpeed;
        }

        if (other.gameObject.CompareTag("Enemy"))
        {
            moveSpeed = -moveSpeed;
        }

        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerMovment>().TakeDamage(damageGiven);

            if(other.transform.position.x > transform.position.x)
            {
                other.gameObject.GetComponent<PlayerMovment>().TakeKnockback(knockbackForce, upwardForce);
            }
            else 
            {
                other.gameObject.GetComponent<PlayerMovment>().TakeKnockback(-knockbackForce, upwardForce);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<Rigidbody2D>().linearVelocity =
                new Vector2(other.GetComponent<Rigidbody2D>().linearVelocityX, 0);

            other.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, bounciness));

            GetComponent<Animator>().SetTrigger("Hit");

            //Disable enemy
            GetComponent<BoxCollider2D>().enabled = false;
            GetComponent<CircleCollider2D>().enabled = false;
            GetComponent<Rigidbody2D>().gravityScale = 0;
            canMove = false;
            GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;

            //Play death sound
            if (deathSound != null)
            {
                audioSource.pitch = Random.Range(0.9f, 1.1f);
                audioSource.PlayOneShot(deathSound, 0.4f);
            }

            // Spawn death particles
            if (deathParticles != null)
            {
                Instantiate(deathParticles, transform.position + Vector3.up * 0.3f, Quaternion.identity);
            }

            Destroy(gameObject, 0.5f);
        }
    }

}
