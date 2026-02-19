using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float ProjectileSpeed = 0f;
    [SerializeField] private int damageGiven = 1;
    [SerializeField] private float knockbackForce = 100f;
    [SerializeField] private float upwardForce = 50f;
    [SerializeField] private float Projectilelifetime = 6f;
    private Vector2 moveDirection = Vector2.right;
     private SpriteRenderer rend;

    private float ProjectileTimer = 6f;
    private void Awake()
    {
        rend = GetComponent<SpriteRenderer>();
        ProjectileTimer = Projectilelifetime;

    }
    public void SetDirection(Vector2 dir)
    {
        moveDirection = dir.normalized;

        if (rend != null)
            rend.flipX = (moveDirection.x > 0);

    }

    // Update is called once per frame
    void Update()
    {
        ProjectileTimer -= Time.deltaTime;
        if (ProjectileTimer <= 0)
        {
            Destroy(gameObject);
            ProjectileTimer = Projectilelifetime;
        }
        transform.Translate(moveDirection * ProjectileSpeed * Time.deltaTime);
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
       

        if (other.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerMovment>().TakeDamage(damageGiven);
            Destroy(gameObject);

            if (other.transform.position.x > transform.position.x)
            {
                other.gameObject.GetComponent<PlayerMovment>().TakeKnockback(knockbackForce, upwardForce);
            }
            else
            {
                other.gameObject.GetComponent<PlayerMovment>().TakeKnockback(-knockbackForce, upwardForce);
            }
        }
    }
}
